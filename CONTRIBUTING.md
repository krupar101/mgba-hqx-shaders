# Contributing

Bug reports and small, focused pull requests are welcome.

1. Read [the compatibility notes](docs/COMPATIBILITY.md).
2. Change `tools/generate.py` (modern) or `tools/generate_classic.py` (16-bit), not just the generated `.fs` files.
3. Run `make verify` and `make package-test`; inspect generated shaders and update any affected documentation.
4. Include the tested mGBA version, GPU/driver and any rendering differences.

The primary invariant is pixel equality with the pinned VBA-M portable 32-bit
reference. A different smoothing algorithm or legacy assembly-compatible mode
should be a separately named shader rather than silently changing this target.

To update upstream sources, pin a commit, retain the original files and notices,
update `vendor/vbam/provenance.json` with SHA-256 hashes, and review behavioral
changes before regenerating. Tests run without network access.

Contributions are licensed under GPL-2.0-or-later. Do not include ROMs, BIOS files,
commercial game assets, credentials or machine-specific paths.
