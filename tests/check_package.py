#!/usr/bin/env python3
"""Assert the release ZIP contains ONLY the two installable shader bundles.
SPDX-License-Identifier: GPL-2.0-or-later
"""
from pathlib import Path
import hashlib
import zipfile

from check_repo import EXPECTED

ROOT = Path(__file__).resolve().parents[1]
ARCHIVE = ROOT / "dist" / "mgba-hqx-shaders.zip"
SHA256SUMS = ROOT / "dist" / "SHA256SUMS"


def main():
    assert ARCHIVE.is_file(), f"Missing {ARCHIVE}"
    assert SHA256SUMS.is_file(), f"Missing {SHA256SUMS}"
    actual_hash = hashlib.sha256(ARCHIVE.read_bytes()).hexdigest()
    assert SHA256SUMS.read_text(encoding="utf-8").strip() == f"{actual_hash}  {ARCHIVE.name}"
    expected = {
        p.relative_to(ROOT / "shaders").as_posix(): p
        for p in (ROOT / "shaders").rglob("*") if p.is_file()
    }
    assert {name.split('/')[0] for name in expected} == set(EXPECTED)
    with zipfile.ZipFile(ARCHIVE) as zf:
        names = zf.namelist()
        assert len(names) == len(set(names)), "Duplicate ZIP paths"
        assert set(names) == set(expected), f"Missing/extra ZIP entries: {set(names) ^ set(expected)}"
        assert zf.testzip() is None, "Corrupted ZIP"
        assert all(item.date_time == (2020, 1, 1, 0, 0, 0) for item in zf.infolist()), "Nonreproducible archive timestamps"
        assert all((item.external_attr >> 16) == 0o100644 for item in zf.infolist()), "Nonreproducible archive mode metadata"
        for name, source in expected.items():
            assert zf.read(name) == source.read_bytes(), f"Bytes differ: {name}"
        assert all(name.count('/') == 1 and (name.endswith(('.fs', '.ini', '.txt')) or name.endswith('/LICENSE')) for name in names)
        assert not any(name.startswith(('tools/', 'vendor/', 'tests/', '.github/', 'docs/')) for name in names)
    print(f"PASS: shader-only release contains exactly {len(EXPECTED)} bundles and {len(expected)} files; bytes and SHA-256 verified")


if __name__ == "__main__":
    main()
