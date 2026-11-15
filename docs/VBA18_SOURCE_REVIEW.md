# VisualBoyAdvance 1.8: historical HQ2x source review

The goal is to reproduce the older **VBA 1.8 C HQ2x filter**, not to use a
similar-looking HQx algorithm and assume it is equivalent. The three original
color paths are 15-bit RGB555, 16-bit RGB565 and 32-bit RGB888. The original
VBA 1.8 release had no HQ4x selection.

## Primary source files inspected

- [VisualBoyAdvance 1.8 `hq2x.cpp`](https://sources.debian.org/src/visualboyadvance/1.8.0.dfsg-4/src/hq2x.cpp/): the original `hq2x_16_def` and `hq2x_32_def` routines, neighbor masks, border handling and table call.
- [VisualBoyAdvance 1.8 `hq2x.h`](https://sources.debian.org/src/visualboyadvance/1.8.0.dfsg-1/src/hq2x.h/): original pattern dispatch, including `MUL`, `MUR`, `MDL`, `MDR` tie-break tests.
- [VisualBoyAdvance 1.8 `interp.h`](https://sources.debian.org/src/visualboyadvance/1.8.0.dfsg-1/src/interp.h/): `interp_set(15|16|32)`, packed color masks, thresholds and weighted interpolation.
- [VBA 1.8 documentation](https://sources.debian.org/src/visualboyadvance/1.8.0.dfsg-6/README/): `hq2x` and `lq2x` are selectable; no `hq4x`.

## Implementation observations

The 15-bit path uses packed masks `0x7C1F` and `0x03E0`; the 16-bit path uses
`0xF81F` and `0x07E0`. `interp_16_diff` applies different R/G shift amounts
in those two formats. Both paths truncate interpolated packed channels with
integer arithmetic. The original 32-bit C path tests whether the top five
bits of each RGB component match before running its Y/U/V difference tests.
The 3×3 neighborhood's eight comparisons create a 256-value HQ2x pattern,
with extra neighbor-to-neighbor decisions in ambiguous patterns.

These semantics correspond to the **VBA-M C lineage** used in the current
GLSL generators. The classic GLSL variants are tested pixel-for-pixel against
compiled 15/16-bit C reference functions based on that pinned lineage.
This is more precise than saying "the output resembles classic HQ2x," but
**is not itself proof of full historical binary compatibility**. In particular,
this source review does not establish full byte-for-byte identity of all
historical and modern headers, nor run original Windows executables.

## How to test against *actual* VBA 1.8 C code

Save the original **unmodified** `hq2x.cpp`, `hq2x.h` and `interp.h` into a single
local directory, usually the extracted historical `src/` folder. Run:

```bash
make vba18-test VBA18_SOURCE_DIR=/absolute/path/to/VBA-1.8.0/src
```

The script compiles a new C++ reference shared library with the **supplied
historical headers and original `hq2x.cpp` interpolation macros** for all three original HQ2x input formats and compares
actual Mesa OpenGL shader output against it for 512 patterns/test frames
per format. It refuses to run without those source files; it never quietly
falls back to the modern vendored headers.

This test does not prove that input RGB888 pixels passed through mGBA match
VBA 1.8's original 15/16/32-bit framebuffer values. Also, 4× HQ2x display
modes are *not* true HQ4x. The separate HQ4x shader preserves VBA-M's
portable HQ4x filter rather than claiming to be from VBA 1.8.

## Release scope

`make package` produces `dist/mgba-vba-hqx-shaders.zip` containing **only**
the six installable `.shader` directories. Each directory includes a copy
of the GPL license as well as its shader source, manifest and README note.
The full project source, tests, workflow and documentation stay out of the
installation release. The shader source files preserve copyright headers.
