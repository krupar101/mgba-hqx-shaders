#!/usr/bin/env python3
"""Verify committed shader bundles can be regenerated without changing the tree.

The generator runs in a temporary copy, so this works both in Git checkouts
and downloaded source ZIPs without overwriting edited shader files.
SPDX-License-Identifier: GPL-2.0-or-later
"""

from pathlib import Path
import shutil
import subprocess
import sys
import tempfile

ROOT = Path(__file__).resolve().parents[1]
GENERATORS = ("generate.py", "generate_classic.py")


def files_under(path):
    return {
        p.relative_to(path): p.read_bytes()
        for p in path.rglob("*")
        if p.is_file()
    }


def main():
    with tempfile.TemporaryDirectory(prefix="hqx-generated-") as directory:
        temporary = Path(directory)
        shutil.copytree(ROOT / "vendor", temporary / "vendor")
        (temporary / "tools").mkdir()
        (temporary / "shaders").mkdir()
        for generator in GENERATORS:
            shutil.copy2(ROOT / "tools" / generator, temporary / "tools" / generator)
            subprocess.run(
                [sys.executable, str(temporary / "tools" / generator)],
                cwd=temporary,
                check=True,
            )
        expected = files_under(temporary / "shaders")
        supplied = files_under(ROOT / "shaders")
        # Only generated files are compared: the original 32-bit bundles have
        # hand-maintained README.txt files that the generators do not emit.
        changed = [str(name) for name, content in expected.items() if supplied.get(name) != content]
        if changed:
            print("FAIL: shipped shaders differ from generated output:")
            for name in sorted(changed):
                print("  ", name)
            return 1
        print(f"PASS: {len(expected)} generated shader files reproduce exactly (no Git required)")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
