# Verification report — 2026-11-15

This report records the checks performed on the complete patched source ZIP,
using Linux, Python 3.13, GCC, Mesa software OpenGL 4.5 and the mGBA 0.10.5
shader-header compatibility harness. The actual mGBA GUI, older VBA 1.8
executable and proprietary GPU drivers were **not** run in this environment.

## Shader and reference tests

- `make verify` — **PASS**: regenerates the 16 generated shader files in a
  temporary directory, detects any mismatch without modifying shipped files,
  validates all six shader bundles, compiles the C++ reference implementations,
  and compares actual EGL/OpenGL output pixel-by-pixel against the compiled
  references for 512 frames each of portable VBA-M HQ2x, portable VBA-M HQ4x,
  classic RGB565 HQ2x and classic RGB555 HQ2x.
- `make classic-native-test` — **PASS**: border, orientation and full 240×160
  source-frame smoke tests for RGB565/RGB555 at 480×320 output.
- `make classic-4x-test` — **PASS**: the 4× display variants of classic RGB565
  and RGB555 match their HQ2x references over 16 test frames each, and render
  full 240×160 source frames to 960×640 without border/orientation mismatch.
- The default native-frame checks in `make verify` — **PASS**: modern HQ2x and
  HQ4x at their 480×320 and 960×640 output resolutions.
- The preview-generation command — **PASS**: the newly rendered preview's
  pixel content is identical to the checked-in preview, so the original PNG
  is retained without a nonfunctional encoding-only change.

## Source and packaging checks

- Upstream source hashes in `vendor/vbam/provenance.json` are validated by
  the generators.
- The release ZIP now contains only the six `.shader` folders at its root.
  Development sources, tests, documentation and repository metadata are excluded.
- `make package-test` checks that every shader bundle file has identical bytes,
  ZIP CRC passes, and SHA-256 agrees with `dist/SHA256SUMS`.
- A deliberately corrupted generated shader was caught by
  `tests/check_generated.py`, and the checker did not modify the bad file.
- Python source compilation/syntax checks passed.

## Scope and limitations

These checks validate the generated algorithms against **pinned** upstream
reference functions and the GLSL driver used in the tests. They do **not**
prove an exact match to the on-screen rendering of every classic VBA 1.8
Windows build, nor behavior on every graphics driver. In particular, the
classic HQ2x variants emulate the old 15/16-bit C color paths, while the 4×
versions are 2× HQ2x pixels repeated to a 4× display grid, **not** the old
assembly HQ4x algorithm. For further details, see [COMPATIBILITY.md](COMPATIBILITY.md).

To reproduce the complete set on a compatible Linux system:

```bash
make verify
make classic-native-test
make classic-4x-test
make package-test
make preview  # optional; needs Pillow
```
