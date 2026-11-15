#!/usr/bin/env python3
"""Check that local Markdown links in the full source repository resolve.
SPDX-License-Identifier: GPL-2.0-or-later
"""
from pathlib import Path
import re
from urllib.parse import unquote

root = Path(__file__).resolve().parents[1]
markdown = sorted([*root.glob("*.md"), *root.glob("docs/**/*.md"),
                   *root.glob(".github/**/*.md")])
missing = []
for doc in markdown:
    text = doc.read_text(encoding="utf-8")
    for target in re.findall(r"\]\(([^)]+)\)", text):
        if target.startswith(("#", "http://", "https://", "mailto:")):
            continue
        path = unquote(target.split("#", 1)[0])
        if path and not (doc.parent / path).exists():
            missing.append(f"{doc.relative_to(root)}: {target}")
if missing:
    raise SystemExit("FAIL: broken local documentation links:\n" + "\n".join(missing))
print(f"PASS: local links resolve in {len(markdown)} Markdown documents")
