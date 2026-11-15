# Verification report — 2026-11-15 (second audit)

This report records the checks performed on the complete patched source ZIP,
using Linux, Python 3.13, GCC, Mesa software OpenGL 4.5 and the mGBA 0.10.5
shader-header compatibility harness. The actual mGBA GUI, older VBA 1.8
executable and proprietary GPU drivers were **not** run in this environment.

## Shader and reference tests

- `make verify` — **PASS**: regenerates the 22 generated shader files in a
  temporary directory, detects any mismatch without modifying shipped files,
  validates all six shader bundles, compiles the C++ reference implementations,
  and compares actual EGL/OpenGL output pixel-by-pixel against the compiled
  references for 512 frames each of portable VBA-M HQ2x, portable VBA-M HQ4x,
  classic RGB565 HQ2x and classic RGB555 HQ2x.
- `make classic-native-test` — **PASS**: pixel-wide split and four distinct
  quadrants at the full 240×160 source resolution, checked for RGB565/RGB555
  at 480×320 output. The second frame catches vertical inversion and channel swaps.
- `make classic-4x-test` — **PASS**: the 4× display variants of classic RGB565
  and RGB555 match their HQ2x references over 16 test frames each, and render
  full 240×160 source frames to 960×640 without border/orientation mismatch
  on both the split-frame and four-quadrant probes.
- The default native-frame checks in `make verify` — **PASS**: modern HQ2x and
  HQ4x at their 480×320 and 960×640 output resolutions, including four-quadrant
  orientation and color probes.
- The preview-generation command — **PASS**: the newly rendered preview's
  pixel content is identical to the checked-in preview, so the original PNG
  is retained without a nonfunctional encoding-only change.

## Source and packaging checks

- Upstream source hashes in `vendor/vbam/provenance.json` are validated by
  the generators.
- The complete **source repository** includes six shader folders, generators,
  tests, README, documentation, vendor snapshot, preview image, license,
  notices, changelog, contribution guide, GitHub workflows, issue template,
  and hidden repository configuration files. It is not the release ZIP.
- The release now contains **only the six installable shader bundles**
  (`*.shader/` folders with `manifest.ini`, fragment shader and `README.txt`).
  `make package-test` verifies exact contents, rejects unexpected source
  files, checks ZIP CRC and the `dist/SHA256SUMS` SHA-256 digest. The ZIP also uses
  fixed timestamps and file modes so repackaging from a source ZIP produces
  the same archive digest.
- A deliberately corrupted generated shader was caught by
  `tests/check_generated.py`, and the checker did not modify the bad file.
- Python source compilation/syntax checks passed.
- The second audit fixed the optional historical comparison harness for 32-bit
  wrapper formatting and now loads original `hq2x.cpp` interpolation macros
  along with `interp.h` and `hq2x.h`. Its fast structural test uses the pinned
  source as a **synthetic fixture**, not as independent VBA 1.8 evidence.
- The classic shader `README.txt` files now link to online compatibility
  documentation, because the shader-only release deliberately omits `docs/`.

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

## Historical-source comparison (separate from pinned VBA-M tests)

The original VBA 1.8 `hq2x.cpp`, `hq2x.h` and `interp.h` were reviewed via
the Debian source browser. The classic C implementation uses the same
neighbor layout, 256-case HQ2x dispatch shape, 15/16/32-bit interpolation
mask conventions and thresholds seen in the pinned code. **This source
review is not a full old-executable output comparison.**

`tests/compare_vba18.py` provides a stronger optional check: compile an actual
unmodified trio of VBA 1.8 `hq2x.cpp`, `hq2x.h` and `interp.h`
supplied from a trusted source archive, and compare the rendered OpenGL pixels
for all three HQ2x color paths. The harness compiles **original interpolation
macro definitions from the supplied `hq2x.cpp`**, not modern VBA-M macros. It has a mandatory source argument and never silently substitutes
VBA-M headers. Running this check requires a local historical source tree,
which was not available during this package's Mesa verification. A synthetic
fixture (modified header guard plus the pinned VBA-M sources) was exercised
for **two frames per color path** to verify that all three compilation and
comparison code paths actually execute; this does **not** constitute a
historical VBA 1.8 compatibility pass.

## README preview audit — 2026-11-15

The previous README preview showed only nearest-neighbor, modern VBA-M HQ2x and modern VBA-M HQ4x; the recommended classic RGB565 HQ2x filter was absent. The preview generator now produces **four** labeled panels (nearest, VBA-M 32-bit HQ2x, classic RGB565 HQ2x, VBA-M HQ4x) from an original synthetic 96×64 frame, with identical 384×256 display sizes. Both HQ2x results are enlarged from 2× to 4× using nearest-neighbor scaling; only HQ4x performs genuine 4× HQx filtering. The preview does not claim exact old VBA 1.8 on-screen matching.

`make preview` renders the shipped OpenGL shaders and runs `tests/check_preview.py`. That checker independently compares every native preview pixel to its pinned compiled C reference and checks the exact four canvas regions for resampling or substitution. This visual/render check requires Pillow, Mesa OpenGL, a C++11 compiler and Python. The shader-only release bundles are unchanged.
