# Copyright and attribution

This distribution includes derivatives of GPL-2.0-or-later code from VBA-M.
Original notices and, where present, Andrea Mazzoleni’s MAME linking exception
are retained in the vendored source and generated shaders. See LICENSE for
the GNU General Public License version 2; upstream permits later versions.

- Original HQX algorithm: Maxim Stepin.
- HQ2X / Advance implementation: Copyright (C) 2003 Andrea Mazzoleni.
- VBA / VBA-M pattern tables: Forgotten, the VBA development team, and VBA-M contributors; specific years and notices appear in the source.
- Shader-port tooling, validation and documentation: Copyright (C) 2026 krupar101, GPL-2.0-or-later.
- The synthetic preview scene was created for this project and is distributed under the same license.

Upstream repository: https://github.com/visualboyadvance-m/visualboyadvance-m

The exact source revision, original paths and SHA-256 checksums are recorded in
vendor/vbam/provenance.json. The source files in that directory are unmodified.
The GLSL is generated from those tables, with sampling, integer operations,
output selection and a balanced decision tree adapted for mGBA.
