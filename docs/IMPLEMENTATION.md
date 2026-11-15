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
