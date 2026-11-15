#!/usr/bin/env python3
"""Verify the release archive contains only shader bundle files.
SPDX-License-Identifier: GPL-2.0-or-later
"""

from pathlib import Path
import hashlib
import zipfile

ROOT = Path(__file__).resolve().parents[1]
ARCHIVE = ROOT / "dist" / "mgba-vba-hqx-shaders.zip"
SHA256SUMS = ROOT / "dist" / "SHA256SUMS"
def distribution_files():
    bundles = ROOT / "shaders"
    return {
        str(p.relative_to(bundles)): p
        for p in bundles.glob("*.shader/*")
        if p.is_file()
    }


def main():
    assert ARCHIVE.is_file(), f"Missing archive {ARCHIVE}"
    assert SHA256SUMS.is_file(), f"Missing checksum {SHA256SUMS}"
    actual_hash = hashlib.sha256(ARCHIVE.read_bytes()).hexdigest()
    assert SHA256SUMS.read_text().strip() == f"{actual_hash}  {ARCHIVE.name}", "Archive checksum mismatch"
    expected = distribution_files()
    with zipfile.ZipFile(ARCHIVE) as zf:
        names = zf.namelist()
        assert len(names) == len(set(names)), "Duplicate ZIP member"
        assert set(names) == set(expected), f"Missing/extra ZIP members: {set(names) ^ set(expected)}"
        assert zf.testzip() is None, "Corrupt ZIP member"
        for name, path in expected.items():
            assert zf.read(name) == path.read_bytes(), f"Wrong ZIP bytes: {name}"
    print(f"PASS: ZIP contains all {len(expected)} shader bundle files, contents and SHA-256 verified")


if __name__ == "__main__":
    main()
