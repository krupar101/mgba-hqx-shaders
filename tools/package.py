#!/usr/bin/env python3
"""Build a shader-only release archive. Source belongs in the Git repo, not Releases.
SPDX-License-Identifier: GPL-2.0-or-later
"""

from pathlib import Path
import hashlib
import zipfile

ROOT = Path(__file__).resolve().parents[1]
DIST = ROOT / "dist"
DIST.mkdir(exist_ok=True)
ARCHIVE = DIST / "mgba-vba-hqx-shaders.zip"
BUNDLES = sorted((ROOT / "shaders").glob("*.shader"))
assert BUNDLES and all(folder.is_dir() for folder in BUNDLES)
files = sorted(path for folder in BUNDLES for path in folder.rglob("*") if path.is_file())
assert files, "No shader files to package"

with zipfile.ZipFile(ARCHIVE, "w", compression=zipfile.ZIP_DEFLATED, compresslevel=9) as zf:
    for path in files:
        # ZIP root is the shader folders themselves; no 'shaders/' prefix.
        name = path.relative_to(ROOT / "shaders").as_posix()
        # Fixed metadata makes the binary release reproducible across Git
        # checkouts and extracted source ZIPs with different file timestamps.
        item = zipfile.ZipInfo(name, date_time=(2020, 1, 1, 0, 0, 0))
        item.create_system = 3
        item.external_attr = 0o100644 << 16
        item.compress_type = zipfile.ZIP_DEFLATED
        zf.writestr(item, path.read_bytes(), compress_type=zipfile.ZIP_DEFLATED, compresslevel=9)

checksum = hashlib.sha256(ARCHIVE.read_bytes()).hexdigest()
(DIST / "SHA256SUMS").write_text(f"{checksum}  {ARCHIVE.name}\n", encoding="utf-8")
print(f"Shader-only release: {ARCHIVE} ({len(BUNDLES)} bundles, {len(files)} files)")
