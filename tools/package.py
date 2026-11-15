#!/usr/bin/env python3
"""Create a shader-only installable ZIP."""

from pathlib import Path
import hashlib
import zipfile

root = Path(__file__).resolve().parents[1]
dist = root / "dist"
dist.mkdir(exist_ok=True)
archive = dist / "mgba-vba-hqx-shaders.zip"
# Ship only loadable bundles; development sources remain in the repository.
files = sorted((root / "shaders").glob("*.shader/*"))
assert all(p.is_file() for p in files)
with zipfile.ZipFile(archive, "w", zipfile.ZIP_DEFLATED) as output:
    for path in sorted(files):
        output.write(path, path.relative_to(root / "shaders"))
checksum = hashlib.sha256(archive.read_bytes()).hexdigest()
(dist / "SHA256SUMS").write_text(f"{checksum}  {archive.name}\n")
print(archive)
