#!/usr/bin/env python3
"""Check shader bundle metadata and files without loading the GPU driver.
SPDX-License-Identifier: GPL-2.0-or-later
"""

import configparser
from pathlib import Path

ROOT = Path(__file__).resolve().parents[1]
BUNDLES = ROOT / "shaders"
EXPECTED = {
    "hq2x.shader": ("hq2x.fs", -2),
    "hq4x.shader": ("hq4x.fs", -4),
    "hq2x-classic-565.shader": ("hq2x.fs", -2),
    "hq2x-classic-555.shader": ("hq2x.fs", -2),
    "hq2x-classic-565-4x.shader": ("hq2x.fs", -4),
    "hq2x-classic-555-4x.shader": ("hq2x.fs", -4),
}


def main():
    actual = {p.name for p in BUNDLES.iterdir() if p.is_dir()}
    assert actual == set(EXPECTED), f"unexpected/missing bundles: {actual ^ set(EXPECTED)}"
    for name, (fragment, scale) in EXPECTED.items():
        folder = BUNDLES / name
        assert {p.name for p in folder.iterdir()} == {"README.txt", "manifest.ini", fragment}, name
        parser = configparser.ConfigParser(interpolation=None)
        with (folder / "manifest.ini").open(encoding="utf-8") as fh:
            parser.read_file(fh)
        assert parser.getint("shader", "passes") == 1, name
        assert parser.get("shader", "name"), name
        assert parser.get("pass.0", "fragmentShader") == fragment, name
        assert parser.getint("pass.0", "width") == scale, name
        assert parser.getint("pass.0", "height") == scale, name
        assert parser.getint("pass.0", "blend") == 0, name
        assert parser.getint("pass.0", "filter") == 0, name
        source = (folder / fragment).read_text(encoding="utf-8")
        assert "void main()" in source and "gl_FragColor" in source, name
        assert (folder / "README.txt").stat().st_size, name
    print(f"PASS: all {len(EXPECTED)} shader bundles have complete, consistent manifests and files")


if __name__ == "__main__":
    main()
