#!/usr/bin/env python3
"""Generate mGBA GLSL and C++ reference wrappers from pinned VBA-M sources.

Run from any working directory. No network or third-party packages required.
The generated shader decision tree deliberately preserves all 256 cases.
SPDX-License-Identifier: GPL-2.0-or-later
"""

from pathlib import Path
import re

ROOT = Path(__file__).resolve().parents[1]
p = ROOT / "vendor/vbam"
(ROOT / "build").mkdir(exist_ok=True)
import hashlib
import json

provenance = json.loads((p / "provenance.json").read_text())
for record in provenance["files"]:
    if (
        hashlib.sha256((p / record["file"]).read_bytes()).hexdigest()
        != record["sha256"]
    ):
        raise SystemExit("Upstream source checksum mismatch: " + record["file"])
base = (p / "hq_base.h").read_text().replace("\\\n", " ")
macros = re.findall(
    r"^#define (PIXEL\w+) (.*)", base.split("#ifdef _HQ4X")[1].split("#endif")[0], re.M
)
weights = {
    1: [3, 1],
    2: [2, 1, 1],
    3: [7, 1],
    4: [2, 7, 7],
    5: [1, 1],
    6: [5, 2, 1],
    7: [6, 1, 1],
    8: [5, 3],
}
common = """
varying vec2 texCoord;
uniform sampler2D tex;
uniform vec2 texSize;
bool diff2(ivec3 a, ivec3 b) {
 if(all(equal(a/8,b/8))) return false;
 ivec3 d=a-b;
 return abs(d.r+d.g+d.b)>192 || abs(d.r-d.b)>28 || abs(-d.r+2*d.g-d.b)>48;
}
int yuv(ivec3 c) {return ((c.r+c.g+c.b)<<14)+((c.r-c.b+512)<<4)+((2*c.g-c.r-c.b)>>3)+128;}
bool Diff(int a,int b) {
 return (((a&0xff0000)-(b&0xff0000))&0x7fffffff)>0x300000 ||
 (((a&0xff00)-(b&0xff00))&0x7fffffff)>0x700 ||
 (((a&0xff)-(b&0xff))&0x7fffffff)>6;
}
"""
license = (p / "hq2x.cpp").read_text().split("#include")[0] + (
    p / "hq_shared.h"
).read_text().split("#ifdef")[0]
for scale in [2, 4]:
    out = ROOT / "shaders" / f"hq{scale}x.shader"
    out.mkdir(exist_ok=True)
    shader = (
        license
        + "\n// Direct port of VBA-M portable 32-bit HQX tables. GPL-2.0-or-later.\n"
        + common
    )
    shader += "void main(){\n ivec3 c[10]; int yy[10]; for(int j=0;j<10;j++){c[j]=ivec3(0);yy[j]=0;}\n vec2 uv=(floor(texCoord*texSize)+0.5)/texSize;\n"
    for i in range(9):
        idx = i if scale == 2 else i + 1
        shader += f" c[{idx}]=ivec3(floor(texture2D(tex,uv+vec2({i%3-1}.0,{i//3-1}.0)/texSize).rgb*255.0+0.5)); yy[{idx}]=yuv(c[{idx}]);\n"
    shader += "int pattern=0;\n"
    for bit, i in enumerate([0, 1, 2, 3, 5, 6, 7, 8]):
        cond = f"diff2(c[4],c[{i}])" if scale == 2 else f"Diff(yy[5],yy[{i+1}])"
        shader += f"if({cond}) pattern|={1<<bit};\n"
    shader += f"ivec2 sub=ivec2(floor(fract(texCoord*texSize)*{scale}.0)); int pixel=sub.y*{scale}+sub.x; ivec3 result=c[{4 if scale==2 else 5}];\n"
    if scale == 2:
        table = (p / "hq2x.h").read_text()
        table = re.sub(
            r"I(\d+)\(([^)]+)\)",
            lambda m: "("
            + "+".join(
                f"c[{v.strip()}]*{w}"
                for v, w in zip(
                    m[2].split(","),
                    (
                        [14, 1, 1]
                        if m[1] == "1411"
                        else [15, 1] if m[1] == "151" else list(map(int, m[1]))
                    ),
                )
            )
            + ")/"
            + str(
                sum(
                    [14, 1, 1]
                    if m[1] == "1411"
                    else [15, 1] if m[1] == "151" else list(map(int, m[1]))
                )
            ),
            table,
        )
        table = re.sub(r"IC\((\d)\)", r"c[\1]", table)
        for name, a, b in [("MUR", 1, 5), ("MDR", 5, 7), ("MDL", 7, 3), ("MUL", 3, 1)]:
            table = table.replace(name, f"diff2(c[{a}],c[{b}])")
        table = re.sub(
            r"P([0-3]) = ([^;]+);",
            lambda m: f"{{ if(pixel=={m[1]}) result={m[2]}; }}",
            table,
        )
        shader += "switch(pattern){" + table + "}\n"
    else:
        for name, definition in macros:
            indices = re.findall(r"c\[(\d)\]", definition)
            inter = re.search(r"Interp(\d)", definition)
            expr = (
                "c[5]"
                if not inter
                else "("
                + "+".join(
                    f"c[{i}]*{w}" for i, w in zip(indices, weights[int(inter[1])])
                )
                + ")/"
                + str(sum(weights[int(inter[1])]))
            )
            pix = int(name[5]) * 4 + int(name[6])
            shader += f"#define {name} {{ if(pixel=={pix}) result={expr}; }}\n"
        table = (p / "hq4x_pattern.h").read_text().replace("yuv[", "yy[")
        shader += table
    shader += "gl_FragColor=vec4(vec3(result)/255.0,1.0);\n}\n"
    start = (
        shader.index("switch(pattern){")
        if scale == 2
        else shader.index("#define PIXEL")
    )
    end = shader.rindex("gl_FragColor=")
    decision = shader[start:end]
    decision = re.sub(r"switch\s*\(pattern\)\s*\{", "", decision, count=1)
    decision = decision[: decision.rfind("}")] + decision[decision.rfind("}") + 1 :]
    decision = re.sub(
        r"(?:case \d+:\s*)+",
        lambda m: "if("
        + " || ".join("pattern==" + x for x in re.findall(r"case (\d+):", m[0]))
        + ") ",
        decision,
    )
    decision = decision.replace("break;", "")
    prefix = decision[: decision.index("if(pattern==")]
    cases = {}
    cursor = len(prefix)
    while True:
        match = re.search(r"if\(pattern==[^\n]+?\)\s*\{", decision[cursor:])
        if not match:
            break
        a = cursor + match.start()
        b = cursor + match.end()
        depth = 1
        e = b
        while depth:
            depth += (decision[e] == "{") - (decision[e] == "}")
            e += 1
        for value in re.findall(r"pattern==(\d+)", decision[a:b]):
            cases[int(value)] = decision[b : e - 1]
        cursor = e
    assert len(cases) == 256, len(cases)

    def tree(lo, hi):
        if hi - lo == 1:
            return cases[lo]
        mid = (lo + hi) // 2
        return (
            f"if(pattern<{mid}){{\n"
            + tree(lo, mid)
            + "\n}else{\n"
            + tree(mid, hi)
            + "\n}\n"
        )

    decision = prefix + tree(0, 256)

    decision = re.sub(r"result=([^;]+);", r"return \1;", decision)
    shader = (
        shader[: shader.index("void main()")]
        + f"ivec3 calculate(ivec3 c[10],int yy[10],int pixel,int pattern){{\n"
        + decision
        + f"return c[{4 if scale==2 else 5}];\n}}\n"
        + shader[shader.index("void main()") : start]
        + f"gl_FragColor=vec4(vec3(calculate(c,yy,pixel,pattern))/255.0,1.0);\n}}\n"
    )
    (out / f"hq{scale}x.fs").write_text("\n".join(line.rstrip() for line in shader.splitlines()) + "\n")
    (out / "manifest.ini").write_text(
        f"[shader]\nname=HQ{scale}X (VBA-M)\nauthor=VBA-M contributors / GLSL port\ndescription=VBA-M portable 32-bit HQ{scale}X\npasses=1\n\n[pass.0]\nfragmentShader=hq{scale}x.fs\nblend=0\nfilter=0\nwidth=-{scale}\nheight=-{scale}\n"
    )
    # Reference uses unchanged upstream C interpolation and pattern tables.
    cpp = '#include <cstdint>\n#include "interp.h"\n#include "hq_shared.h"\n'
    cpp += 'extern "C" void reference(const uint32_t* c,uint32_t* dst){\n'
    if scale == 2:
        cpp += "int mask=0;\n"
        for bit, i in enumerate([0, 1, 2, 3, 5, 6, 7, 8]):
            cpp += f"if(interp_32_diff(c[4],c[{i}])) mask|={1<<bit};\n"
        for i in range(4):
            cpp += f"#define P{i} dst[{i}]\n"
        for name, a, b in [("MUR", 1, 5), ("MDR", 5, 7), ("MDL", 7, 3), ("MUL", 3, 1)]:
            cpp += f"#define {name} interp_32_diff(c[{a}],c[{b}])\n"
        defs = (p / "hq2x.cpp").read_text()
        for line in defs.splitlines():
            if (
                line.startswith("#define I")
                and "interp_32_" in line
                or line.startswith("#define IC")
            ):
                cpp += line + "\n"
        cpp += 'switch(mask){\n#include "hq2x.h"\n}\n'
    else:
        cpp += "unsigned int yuv[10];for(int i=1;i<10;i++)yuv[i]=RGBtoYUV_32(c[i]);int pattern=0;\n"
        for bit, i in enumerate([1, 2, 3, 4, 6, 7, 8, 9]):
            cpp += f"if(Diff(yuv[5],yuv[{i}]))pattern|={1<<bit};\n"
        for name, d in macros:
            pix = int(name[5]) * 4 + int(name[6])
            inter = re.search(r"Interp(\d)", d)
            if inter:
                args = ", ".join("c[" + i + "]" for i in re.findall(r"c\[(\d)\]", d))
                expr = f"Interp{inter[1]}_32(&dst[{pix}], {args});"
            else:
                expr = f"dst[{pix}]=c[5];"
            cpp += f"#define {name} {expr}\n"
        cpp += '#include "hq4x_pattern.h"\n'
    cpp += "}\n"
    (ROOT / "build" / f"ref{scale}.cpp").write_text(cpp)

# A shader-only release must still carry the complete GPL license.
for bundle in (ROOT / "shaders").glob("hq[24]x.shader"):
    (bundle / "LICENSE").write_bytes((ROOT / "LICENSE").read_bytes())
