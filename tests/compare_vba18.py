#!/usr/bin/env python3
"""Compare rendered HQ2x with reference code built from supplied VBA 1.8 sources.

Requires the original interp.h, hq2x.h AND hq2x.cpp. In particular, use the
original hq2x.cpp interpolation macros, rather than the modern VBA-M macros.
The caller must independently verify that the supplied sources are authentic.
SPDX-License-Identifier: GPL-2.0-or-later
"""
from pathlib import Path
import os
import re
import subprocess
import sys

ROOT = Path(__file__).resolve().parents[1]
VENDOR = ROOT / "vendor" / "vbam"
BUILD = ROOT / "build"
USAGE = "Usage: make vba18-test VBA18_SOURCE_DIR=/path/to/VisualBoyAdvance-1.8.0/src"
MACROS = {"IC", "I11", "I211", "I31", "I332", "I431", "I521", "I53", "I611", "I71", "I772", "I97", "I1411", "I151"}


def historical_macros(source: str, bits: int):
    """Extract the original HQ2x C macros, not the LQ2x or modern ones."""
    name = f"hq2x_{'32' if bits == 32 else '16'}_def"
    # The first switch in each function is the HQ2x 256-case dispatch.
    match = re.search(r"\bstatic\s+void\s+" + name + r"\s*\([^)]*\)\s*\{", source)
    if not match:
        raise ValueError(f"Missing original {name} function")
    body = source[match.end():]
    switch = re.search(r"\bswitch\s*\(\s*mask\s*\)", body)
    if not switch:
        raise ValueError(f"Missing {name} HQ2x switch")
    defs = re.findall(r"^\s*#define\s+(I(?:C|\d+))\s*\([^\n]*$", body[:switch.start()], re.M)
    if set(defs) != MACROS or len(defs) != len(MACROS):
        raise ValueError(f"Unexpected interpolation macros in original {name}: {defs}")
    original = re.findall(r"^\s*(#define\s+(I(?:C|\d+))\s*\([^\n]*)$", body[:switch.start()], re.M)
    return {name: definition for definition, name in original}


def use_historical_definitions(wrapper: str, macros: dict):
    # Strip all modern interpolation macro definitions from generated wrappers,
    # including duplicate IC definitions (left by upstream C generator).
    wrapper = re.sub(r"^#define\s+I(?:C|\d+)\s*\([^\n]*\n", "", wrapper, flags=re.M)
    marker = re.compile(r"^(?P<indent>\s*)switch\s*\(\s*mask\s*\)\s*\{", re.M)
    if len(marker.findall(wrapper)) != 1:
        raise ValueError("Unexpected generated wrapper layout")
    def insert(match):
        return "\n".join(macros[name] for name in sorted(macros)) + "\n" + match.group(0)
    return marker.sub(insert, wrapper, count=1)


def main():
    if len(sys.argv) != 2 or not sys.argv[1]:
        raise SystemExit(USAGE)
    source = Path(sys.argv[1]).resolve()
    for name in ("hq2x.h", "interp.h", "hq2x.cpp"):
        if not (source / name).is_file():
            raise SystemExit(f"Missing ORIGINAL VBA 1.8 source file: {source / name}")
    old_interp = (source / "interp.h").read_text(encoding="utf-8")
    old_table = (source / "hq2x.h").read_text(encoding="utf-8")
    old_cpp = (source / "hq2x.cpp").read_text(encoding="utf-8")
    if "#ifndef __INTERP_H" not in old_interp or not re.search(r"case\s+0\s*:", old_table):
        raise SystemExit("Headers do not resemble original VBA 1.8 sources; refusing misleading comparison")
    # A recognizable file layout is NOT an authenticity guarantee. Verify the
    # source archive externally; this never tests the Windows display pipeline.
    BUILD.mkdir(exist_ok=True)
    for bits, variant, src_name in ((16, "classic-565", "ref2-classic-565.cpp"),
                                    (15, "classic-555", "ref2-classic-555.cpp"),
                                    (32, "", "ref2.cpp")):
        wrapper = (BUILD / src_name).read_text(encoding="utf-8")
        wrapper = wrapper.replace('#include "hq_shared.h"\n', '')
        wrapper = wrapper.replace('#include <cstdint>\n',
                                  '#include <cstdint>\nusing u16 = uint16_t;\nusing u32 = uint32_t;\n', 1)
        wrapper = use_historical_definitions(wrapper, historical_macros(old_cpp, bits))
        generated = BUILD / f"ref2-vba18-{bits}.cpp"
        binary = BUILD / f"ref2-vba18-{bits}.so"
        generated.write_text(wrapper, encoding="utf-8")
        compiler = os.environ.get("CXX", "c++")
        subprocess.run([compiler, "-O2", "-std=c++11", "-shared", "-fPIC",
                        "-I", str(source), "-I", str(VENDOR),
                        str(generated), "-o", str(binary)], check=True)
        env = dict(os.environ, HQX_REFERENCE_PATH=str(binary))
        if variant:
            env["HQX_VARIANT"] = variant
        else:
            env.pop("HQX_VARIANT", None)
        env.pop("HQX_SHADER_BUNDLE", None)
        subprocess.run([sys.executable, "-u", "tests/compare.py", "2"],
                       cwd=ROOT, env=env, check=True)
    print("PASS: all 3 shaders match references built from the supplied historical source")
    print("NOTE: source authenticity and VBA 1.8 display-pipeline equivalence are not validated")


if __name__ == "__main__":
    main()
