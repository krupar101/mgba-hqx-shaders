#!/usr/bin/env python3
"""Generate optional RGB555/RGB565 classic HQ2x ports and 16-bit references.

Uses the pinned upstream pattern/interpolation tables and a color-packed shader
path, instead of the 32-bit RGB arithmetic used by the default shaders.
This matches the classic VisualBoyAdvance HQ2x 16-bit C algorithm, subject
to the historical source/version limits documented in COMPATIBILITY.md.
SPDX-License-Identifier: GPL-2.0-or-later
"""
from pathlib import Path
import re
import hashlib
import json

ROOT = Path(__file__).resolve().parents[1]
VENDOR = ROOT / 'vendor/vbam'
(ROOT / 'build').mkdir(exist_ok=True)
provenance = json.loads((VENDOR / 'provenance.json').read_text())
for record in provenance['files']:
    assert hashlib.sha256((VENDOR / record['file']).read_bytes()).hexdigest() == record['sha256'], record['file']

# The base generator has already expanded and balanced every rule of the pinned
# C tables. Interpret its output expressions into packed 16-bit interpolation.
expr = re.compile(r'return\s+\((c\[\d+\]\*\d+(?:\+c\[\d+\]\*\d+){1,2})\)/(\d+);')

for bits in (565,555):
    bpp = 16 if bits == 565 else 15
    mode = f'classic-{bits}'
    mask1, mask2 = ((0xF81F,0x07E0) if bits==565 else (0x7C1F,0x03E0))
    pack = (f'((rgb.r>>3)<<11)|((rgb.g>>{2 if bits==565 else 3})<<5)|(rgb.b>>3)' if bits==565
            else '((rgb.r>>3)<<10)|((rgb.g>>3)<<5)|(rgb.b>>3)')
    decode = ('ivec3(((p>>11)&31)*255/31,((p>>5)&63)*255/63,(p&31)*255/31)' if bits==565
              else 'ivec3(((p>>10)&31)*255/31,((p>>5)&31)*255/31,(p&31)*255/31)')
    diff = f'''bool diff16(int a,int b){{
 if(a==b)return false;
 int r=((a&0x{0xF800 if bits==565 else 0x7C00:X})-(b&0x{0xF800 if bits==565 else 0x7C00:X}))>>{8 if bits==565 else 7};
 int g=((a&0x{0x07E0 if bits==565 else 0x03E0:X})-(b&0x{0x07E0 if bits==565 else 0x03E0:X}))>>{3 if bits==565 else 2};
 int bl=((a&31)-(b&31))<<3;
 return abs(r+g+bl)>192 || abs(r-bl)>28 || abs(-r+2*g-bl)>48;
}}'''
    funcs = f'''
int pack16(ivec3 rgb){{return {pack};}}
ivec3 unpack16(int p){{return {decode};}}
{diff}
int yuv16(int p){{
 int r=((p>>{11 if bits==565 else 10})&31)<<3;
 int g=((p>>5)&{63 if bits==565 else 31})<<{2 if bits==565 else 3};
 int b=(p&31)<<3;
 return ((r+g+b)<<14)+((r-b+512)<<4)+((2*g-r-b)>>3)+128;
}}
bool Diff(int a,int b){{
 return (((a&0xff0000)-(b&0xff0000))&0x7fffffff)>0x300000 ||
 (((a&0xff00)-(b&0xff00))&0x7fffffff)>0x700 ||
 (((a&0xff)-(b&0xff))&0x7fffffff)>6;
}}
// Packed interpolation: preserve RGB565/RGB555 truncation and channel masks.
int mix2_16(int a,int b,int w1,int w2,int d){{
 return (((a&0x{mask1:X})*w1+(b&0x{mask1:X})*w2)/d &0x{mask1:X})
       |(((a&0x{mask2:X})*w1+(b&0x{mask2:X})*w2)/d &0x{mask2:X});
}}
int mix3_16(int a,int b,int c,int w1,int w2,int w3,int d){{
 return (((a&0x{mask1:X})*w1+(b&0x{mask1:X})*w2+(c&0x{mask1:X})*w3)/d &0x{mask1:X})
       |(((a&0x{mask2:X})*w1+(b&0x{mask2:X})*w2+(c&0x{mask2:X})*w3)/d &0x{mask2:X});
}}
'''
    for scale in (2,):
        src=(ROOT/'shaders'/f'hq{scale}x.shader'/f'hq{scale}x.fs').read_text()
        lic=src[:src.index('// Direct port')]
        calc=src[src.index('ivec3 calculate('):src.index('void main(){')]
        calc=calc.replace('ivec3 calculate(ivec3 c[10]', 'int calculate(int c[10]')
        calc=calc.replace('diff2(', 'diff16(')
        def fix(m):
            terms=[(int(a),int(w)) for a,w in re.findall(r'c\[(\d+)\]\*(\d+)', m[1])]
            assert sum(w for _,w in terms) == int(m[2]), (terms,m[2])
            assert len(terms) in (2,3)
            suffix=','.join([*(f'c[{i}]' for i,_ in terms), *(str(w) for _,w in terms),m[2]])
            return f'return mix{len(terms)}_16({suffix});'
        calc,count=expr.subn(fix,calc)
        assert count>15,(scale,count)
        # Every pattern-table interpolation has been converted; no RGB vector math may remain.
        assert not re.search(r'return\s+\(c\[',calc)
        main='''void main(){
 int c[10]; int yy[10]; for(int j=0;j<10;j++){c[j]=0; yy[j]=0;}
 vec2 uv=(floor(texCoord*texSize)+0.5)/texSize;
'''
        for i in range(9):
            idx=i if scale==2 else i+1
            main+=f' c[{idx}]=pack16(ivec3(floor(texture2D(tex,uv+vec2({i%3-1}.0,{i//3-1}.0)/texSize).rgb*255.0+0.5))); yy[{idx}]=yuv16(c[{idx}]);\n'
        main+=' int pattern=0;\n'
        for bit,i in enumerate([0,1,2,3,5,6,7,8]):
            cond=f'diff16(c[4],c[{i}])' if scale==2 else f'Diff(yy[5],yy[{i+1}])'
            main+=f' if({cond})pattern|={1<<bit};\n'
        main+=f' ivec2 sub=ivec2(floor(fract(texCoord*texSize)*{scale}.0)); int pixel=sub.y*{scale}+sub.x;\n'
        main+=' gl_FragColor=vec4(vec3(unpack16(calculate(c,yy,pixel,pattern)))/255.0,1.0);\n}\n'
        glsl=lic+f'// Optional RGB{bits} legacy-color port of the pinned VBA-M HQx tables.\n'+'''varying vec2 texCoord;
uniform sampler2D tex;
uniform vec2 texSize;
'''+funcs+calc+main
        out=ROOT/'shaders'/f'hq{scale}x-{mode}.shader'
        out.mkdir(exist_ok=True)
        (out/f'hq{scale}x.fs').write_text(glsl)
        label='Classic VBA'
        (out/'manifest.ini').write_text(f'''[shader]
name=HQ{scale}X ({label} RGB{bits})
author=VBA / VBA-M contributors; GLSL port
description=Legacy packed RGB{bits} HQ{scale}X filtering; selectable alternative
passes=1

[pass.0]
fragmentShader=hq{scale}x.fs
blend=0
filter=0
width=-{scale}
height=-{scale}
''')
        (out/'README.txt').write_text(f'''HQ{scale}X RGB{bits} legacy-color variant.
HQ2X follows classic VisualBoyAdvance 16-bit C filtering, not
the x86 assembly HQ4X implementation.
Quantizes mGBA's RGB888 source to RGB{bits} before filtering, and converts the
result back to RGB888. Choose the color mode you prefer visually.
See docs/COMPATIBILITY.md for limitations and testing scope.
''')
        # Compiled pinned C interpolation/pattern rules as an independent reference.
        cpp='#include <cstdint>\n'
        if bits==555:cpp+='#define RGB555\n'
        cpp+='#include "interp.h"\n#include "hq_shared.h"\n'
        cpp+='extern "C" void reference(const uint32_t* rgb,uint32_t* output){\n'
        cpp+=f' interp_set({bpp});\n uint16_t c[10]={{0}}; uint16_t dst[{scale*scale}]={{0}};\n'
        for i in range(9):
            idx=i if scale==2 else i+1
            if bits==565:
                cpp+=f' c[{idx}]=uint16_t(((rgb[{idx}]>>19)&31)<<11 | ((rgb[{idx}]>>10)&63)<<5 | ((rgb[{idx}]>>3)&31));\n'
            else:
                cpp+=f' c[{idx}]=uint16_t(((rgb[{idx}]>>19)&31)<<10 | ((rgb[{idx}]>>11)&31)<<5 | ((rgb[{idx}]>>3)&31));\n'
        # Colors are packed in the same positions as shaders, with unused c[0] for HQ4X.
        cpp+=' int mask=0;\n'
        for bit,i in enumerate([0,1,2,3,5,6,7,8]):
            cpp+=f' if(interp_16_diff(c[4],c[{i}]))mask|={1<<bit};\n'
        for i in range(4):cpp+=f'#define P{i} dst[{i}]\n'
        for name,a,b in [('MUR',1,5),('MDR',5,7),('MDL',7,3),('MUL',3,1)]:
            cpp+=f'#define {name} interp_16_diff(c[{a}],c[{b}])\n'
        for line in (VENDOR/'hq2x.cpp').read_text().splitlines():
            if line.startswith('#define I') and ('interp_16_' in line or line.startswith('#define IC')):
                cpp+=line+'\n'
        cpp+=' switch(mask){\n#include "hq2x.h"\n}\n'
        for i in range(scale*scale):
            if bits==565:
                cpp+=f' output[{i}]=(((dst[{i}]>>11)&31)*255/31)<<16 | (((dst[{i}]>>5)&63)*255/63)<<8 | ((dst[{i}]&31)*255/31);\n'
            else:
                cpp+=f' output[{i}]=(((dst[{i}]>>10)&31)*255/31)<<16 | (((dst[{i}]>>5)&31)*255/31)<<8 | ((dst[{i}]&31)*255/31);\n'
        cpp+='}\n'
        (ROOT/'build'/f'ref{scale}-{mode}.cpp').write_text(cpp)
        print(f'Generated HQ{scale}X {mode} ({count} weighted rules)')


    # VBA 1.8 exposed HQ2x, not HQ4x. For users who want a bigger
    # classic-VBA-like window, output its 2x pixels on a 4x nearest grid.
    base = ROOT/'shaders'/f'hq2x-{mode}.shader'
    zoom = ROOT/'shaders'/f'hq2x-{mode}-4x.shader'
    zoom.mkdir(exist_ok=True)
    (zoom/'hq2x.fs').write_bytes((base/'hq2x.fs').read_bytes())
    (zoom/'manifest.ini').write_text(f"""[shader]
name=HQ2X (Classic VBA RGB{bits}, 4x display)
author=VBA contributors; GLSL port
description=Classic HQ2X RGB{bits} filtered at 2x then shown at sharp 4x
passes=1

[pass.0]
fragmentShader=hq2x.fs
blend=0
filter=0
width=-4
height=-4
""")
    (zoom/'README.txt').write_text(f"""Classic VBA HQ2X RGB{bits} at a 4x output grid.
The underlying algorithm is still HQ2X, with each computed pixel repeated
2x2 for a larger crisp window. It is NOT the unrelated HQ4X algorithm.
""")
