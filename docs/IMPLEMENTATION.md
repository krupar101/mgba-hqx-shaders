# Implementation

`tools/generate.py` reads the checksummed upstream snapshot and emits both
`.shader` bundles plus C++ reference wrappers under `build/`.

Each fragment samples a 3×3 source neighborhood at texel centers, converts
normalized RGB to 8-bit integer channels, builds the upstream eight-bit neighbor
mask, and evaluates the rules for one of the 4 or 16 output subpixels.
Interpolation uses integer arithmetic, preserving truncation rather than
replacing it with floating-point blending.

The upstream C `switch` is converted to a balanced binary decision tree over
all 256 masks. Initial flat dispatch versions produced a reproducible mismatch
on the validation driver; the balanced form passed the reference suite. The
underlying compiler cause was not established. Do not simplify the tree without
running the pixel comparison tests.

The C++ wrappers keep the original switch tables and interpolation helpers.
`tests/gl_render.py` creates a surfaceless EGL OpenGL context with ctypes,
compiles the same shader header used by mGBA’s desktop path, renders to an RGBA8
framebuffer, and reads the pixels back. No Python OpenGL package is required.

The generator has no network dependency. To update the snapshot, retrieve a
specific upstream revision, review its changes, update provenance and hashes,
then regenerate and validate. Avoid tracking the moving `master` branch in
builds or fetching code during tests.

The preview is generated with `tools/preview.py` and uses the shipped shaders.
Its small original scene is intentionally unrelated to commercial games.

## Classic 16-bit HQ2X generation

`tools/generate_classic.py` runs after `tools/generate.py`. It reuses the
balanced 256-case shader rules generated from the checksummed HQ2x table, then
converts every weighted interpolation expression to packed RGB565/RGB555 math.
The shader quantizes the sampled RGB888 texels **before** computing YUV edge
thresholds, uses the upstream `interp_16_diff` algorithm, and preserves packed
16-bit masked interpolation/truncation.

The 4x display bundles reuse exactly the same HQ2x fragment shader but set the
pass size to 4x. Each of the four output positions across a row uses one of the
two HQ2x subpixels twice (nearest enlargement), retaining classic HQ2x edges.

Independent `build/ref2-classic-*.cpp` wrappers call the pinned upstream
`interp_16_*` and `hq2x.h` C table with `interp_set(15)` or `interp_set(16)`.
The actual OpenGL output is validated against these compiled wrappers, so the
generated GLSL interpolation and 15/16-bit color comparisons are tested end to
end rather than solely against another shader implementation.
