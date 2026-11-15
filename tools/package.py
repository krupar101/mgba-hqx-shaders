#!/usr/bin/env python3
"""Create an installable ZIP, including the corresponding source and license."""

from pathlib import Path
import hashlib
import zipfile

root = Path(__file__).resolve().parents[1]
dist = root / "dist"
dist.mkdir(exist_ok=True)
archive = dist / "mgba-vba-hqx-shaders.zip"
files = [
    root / name
    for name in (
        "LICENSE",
        "README.md",
        "NOTICE.md",
        "CHANGELOG.md",
        "CONTRIBUTING.md",
        "Makefile",
    )
]
for directory in ("shaders", "vendor", "tools", "tests", "docs", "assets"):
    files.extend(
        p
        for p in (root / directory).rglob("*")
        if p.is_file() and "__pycache__" not in p.parts and p.suffix != ".pyc"
    )
with zipfile.ZipFile(archive, "w", zipfile.ZIP_DEFLATED) as output:
    for path in sorted(files):
        output.write(path, path.relative_to(root))
checksum = hashlib.sha256(archive.read_bytes()).hexdigest()
(dist / "SHA256SUMS").write_text(f"{checksum}  {archive.name}\n")
print(archive)
