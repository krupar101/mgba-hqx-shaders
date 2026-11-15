#!/usr/bin/env python3
"""Fast, offline regression checks for the optional historical source harness.

This is a STRUCTURAL test using pinned modern source as a synthetic fixture;
it does not claim to validate authentic VBA 1.8 output.
SPDX-License-Identifier: GPL-2.0-or-later
"""
from pathlib import Path
from compare_vba18 import historical_macros, use_historical_definitions, MACROS

root = Path(__file__).resolve().parents[1]
cpp = (root / "vendor/vbam/hq2x.cpp").read_text(encoding="utf-8")
for bits, wrapper in ((15, "ref2-classic-555.cpp"),
                      (16, "ref2-classic-565.cpp"), (32, "ref2.cpp")):
    macros = historical_macros(cpp, bits)
    assert set(macros) == MACROS
    original = (root / "build" / wrapper).read_text(encoding="utf-8")
    modified = use_historical_definitions(original, macros)
    assert modified.count("#define I11(") == 1, bits
    assert modified.count("#define IC(") == 1, bits
    assert modified.count("switch(mask){") == 1, bits
    assert all(line in modified for line in macros.values()), bits
    # Reject a malformed or unexpected old source layout instead of silently
    # substituting pinned VBA-M macros.
    try:
        historical_macros("static void hq2x_16_def() {}", bits)
    except ValueError:
        pass
    else:
        raise AssertionError("Malformed historical C source was accepted")
print("PASS: historical-source harness handles RGB555/RGB565/RGB888 reference layouts (synthetic fixture only)")
