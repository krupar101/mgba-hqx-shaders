# VBA-M HQX shaders for mGBA

[![Validate shaders](https://github.com/krupar101/mgba-vba-hqx-shaders/actions/workflows/validate.yml/badge.svg)](https://github.com/krupar101/mgba-vba-hqx-shaders/actions/workflows/validate.yml)
[![License: GPL v2 or later](https://img.shields.io/badge/license-GPL--2.0--or--later-blue)](LICENSE)

**HQ2X and HQ4X shaders for mGBA, including legacy 15/16-bit VisualBoyAdvance HQ2X modes.**
Ready to load, with the upstream pattern tables, a reproducible generator, and
pixel-by-pixel tests against compiled VBA-M code.

![Four-way comparison of nearest-neighbor, VBA-M HQ2x, classic RGB565 HQ2x, and VBA-M HQ4x rendered from original synthetic pixel art](assets/comparison.png)

The graphic compares **nearest-neighbor 4×**, **VBA-M 32-bit HQ2x**, **classic RGB565 HQ2x**, and **VBA-M HQ4x** on the *same synthetic 96×64 source image*. The HQ2x outputs are enlarged with nearest-neighbor sampling from 2× to the same 4× final size as HQ4x. The classic RGB555 variant is not pictured. The image is generated with the shipped OpenGL shaders, not hand-drawn filtering; it does **not** establish an exact match to the original VBA 1.8 Windows executable.

## Classic VBA look (new)

If you preferred how the **original VisualBoyAdvance 1.8** rendered HQ2x,
try **HQ2X (Classic VBA RGB565)** first. The 32-bit shader uses a 32-bit RGB
filter path; these new shaders quantize to the original 15/16-bit packed color
representation **before** applying the HQ2x thresholds, pattern rules and
weighted interpolation. That changes both edge decisions and blending.

| New shader folder | GBA pass size | What it does |
| --- | --- | --- |
| `hq2x-classic-565.shader` | 480 × 320 | Classic 16-bit RGB565 HQ2x; recommended starting point |
| `hq2x-classic-555.shader` | 480 × 320 | Alternative RGB555 (15-bit) HQ2x |
| `hq2x-classic-565-4x.shader` | 960 × 640 | **Same HQ2x** at a sharp 4× display size; repeats each output pixel 2×2 |
| `hq2x-classic-555-4x.shader` | 960 × 640 | RGB555 HQ2x at a sharp 4× display size |

**The 4× display versions are deliberately not HQ4x.** Original VBA 1.8 had
HQ2x, not HQ4x, so doubling its display grid is a better match for the classic
HQ2x look than substituting a different filter algorithm. For true HQ4x, keep
using `hq4x.shader` below. All original shaders remain unchanged.

These variants reproduce the *16-bit filtering mathematics* using the pinned
VBA-M pattern tables, and are tested against compiled 16-bit C routines. They
are **not guaranteed pixel-identical to a screenshot from VBA 1.8**: that can
also depend on VBA's original framebuffer pixel format, output conversion,
Windows blitter and display scaling. See [compatibility details](docs/COMPATIBILITY.md).

## Existing modern shaders

| Shader | Pass resolution for GBA | Useful for |
| --- | --- | --- |
| **HQ2X (VBA-M)** | 480 × 320 | A lighter filter with a 2× output grid |
| **HQ4X (VBA-M)** | 960 × 640 | A finer 4× output grid |

The two modern shaders preserve VBA-M’s integer interpolation and color comparisons, use nearest
sampling, and clamp samples at frame boundaries through mGBA’s texture setup.
No lookup textures, additional shader passes, or emulator modifications are needed.

**Compatibility target:** the pinned VBA-M **portable 32-bit C implementation**.
These are not advertised as bit-identical to the legacy x86 assembly filters,
FFmpeg HQX, or mGBA’s upstream HQ2X shader. See [the exact scope and limitations](docs/COMPATIBILITY.md).

## Install

1. Download and extract the **shader-only** ZIP from [Releases](https://github.com/krupar101/mgba-vba-hqx-shaders/releases), or clone this source repository. The release archive contains exactly six `.shader` folders and nothing else. Each bundle carries its manifest, GLSL shader, a short note, and the GPL license.
2. If installing from the release ZIP, copy a `.shader` folder directly from the extracted ZIP. If cloning the source repository, copy it from `shaders/`. For classic HQ2x start with `hq2x-classic-565.shader`. Keep each folder intact: it contains both a `manifest.ini` and a fragment shader.
3. Open mGBA’s shader settings and load the desired `.shader` **folder**. Depending on the version, shader settings are available under **Tools → Settings → Shaders**.
4. Reload the shader or restart mGBA after replacing an existing installation.

For the **Linux Flatpak** (`io.mgba.mGBA`), a convenient location is:

```text
~/.var/app/io.mgba.mGBA/config/mgba/shaders/
├── hq2x-classic-565.shader/
│   ├── manifest.ini
│   └── hq2x.fs
└── hq2x-classic-565-4x.shader/
    ├── manifest.ini
    └── hq2x.fs
```

Run this from the extracted project directory to install the **classic RGB565 pair** with a backup (change the list to include other variants as desired):

```bash
shader_dir="$HOME/.var/app/io.mgba.mGBA/config/mgba/shaders"
backup_dir="$HOME/.var/app/io.mgba.mGBA/config/mgba/shader-backups/hqx-$(date +%Y%m%d-%H%M%S)"
mkdir -p "$shader_dir" "$backup_dir"
for shader in hq2x-classic-565.shader hq2x-classic-565-4x.shader; do
    if [ -d "$shader_dir/$shader" ]; then
        cp -a "$shader_dir/$shader" "$backup_dir/"
    fi
    source_path="shaders/$shader"
    if [ ! -d "$source_path" ]; then source_path="$shader"; fi
    cp -a "$source_path" "$shader_dir/"
done
```

For other installations, choose any readable location and load the folder from
mGBA. The shaders need mGBA’s modern desktop OpenGL renderer; old OpenGL and
OpenGL ES 2.0 paths do not support the integer operations used here.

## Get a fair visual comparison

- Compare the **same source frame** at the same final size.
- Disable bilinear filtering when checking pixel equality.
- Use integer window scaling. Native 2× for HQ2X or 4× for HQ4X avoids a second resize; multiples of those sizes preserve a uniform output grid.
- Match color correction and other display filters between emulators.

HQX smooths pixel-art edges. It does not reconstruct new detail or turn a
limited-color background into a continuous gradient. Shader load time and
performance depend on the GPU and driver; these ports contain the full rules.

## Build and validate

Installing the shaders requires no build. Development tests run on **Linux**
with Python 3.10+, a C++11 compiler, Make, and Mesa EGL/OpenGL libraries.
Python validation uses only the standard library.

Ubuntu/Debian:

```bash
sudo apt-get install python3 g++ make libegl1 libgl1 libgl1-mesa-dri
make test           # all original and classic validations
make classic-test   # pixel comparisons plus 4x bundle identity checks
make classic-native-test # optional slow full-size GBA checks
make classic-4x-test # optional, slower OpenGL tests of the 4x display modes
make verify        # regenerate independently and run the full default test suite
make package-test  # create shader-only release ZIP and verify exact contents/checksum
```

The suite regenerates shaders, compiles independent C++ references using
the pinned VBA-M tables/interpolation code (32-bit and 16-bit), and checks:

- all 256 binary neighborhood patterns;
- 256 deterministic randomized color frames, including nearby colors;
- every output pixel, including frame boundaries, for each of the 512 frames
  at 2× (plus 512 frames at native 4× for modern HQ4x);
- 4× classic display variants checked for exact shader identity with 2× variants;
- optional `make classic-4x-test` samples 4× pixels against the 2× reference;
- full-size 240 × 160 GBA frames rendered at both 2× and 4×, including
  an additional four-quadrant orientation and channel test.

`make verify` also checks that shipped shaders exactly match freshly generated
files in a separate temporary directory. It works from both a Git checkout
and a downloaded source ZIP; it does not overwrite edited shaders.
Tests are headless and use Mesa software rendering; they do not certify every
GPU or prove equality with an entire VBA-M display pipeline.

```bash
make generate  # regenerate original and classic shaders and C++ references
make package   # ZIP containing ONLY the six installable .shader folders
make preview   # regenerate + pixel-validate all four preview panels; requires Pillow
```

For preview generation, install Pillow in your preferred Python environment
(`python3 -m pip install Pillow`). See [implementation notes](docs/IMPLEMENTATION.md),
the [verification report](docs/VALIDATION.md),
and [contribution guidelines](CONTRIBUTING.md) before changing the filters.

### Testing against the historical VBA 1.8 source

The 15/16-bit formulas and HQ2x dispatch in the historical VBA 1.8
[HQ2x implementation](https://sources.debian.org/src/visualboyadvance/1.8.0.dfsg-4/src/hq2x.cpp/)
and [interpolation header](https://sources.debian.org/src/visualboyadvance/1.8.0.dfsg-1/src/interp.h/)
use the same rule names, thresholds and masks as the pinned VBA-M C lineage.
The old 1.8 release did **not** contain a true HQ4x filter.

To do the stronger *independent historical-source* comparison, obtain an
unmodified VBA 1.8.0 source tree with `src/hq2x.cpp`, `src/hq2x.h`
and `src/interp.h`, then run:

```bash
make vba18-test VBA18_SOURCE_DIR=/path/to/VisualBoyAdvance-1.8.0/src
```

This compiles references using the supplied historical headers **and**
interpolation macros extracted from `hq2x.cpp`, then checks the
GLSL output for the 15-, 16- and 32-bit HQ2x paths. The ordinary `make verify`
compares against the pinned VBA-M snapshot instead. Neither test validates
VBA 1.8 Windows output conversion, user settings, drivers, or display scaling.

See [historical VBA source review](docs/VBA18_SOURCE_REVIEW.md) for exactly
what was inspected and what remains unverified.

## Source and license

HQX was originally designed by **Maxim Stepin**. The HQ2X implementation derives
from **Andrea Mazzoleni’s Advance project**; the portable HQ4X implementation and
tables come from **VBA-M and its contributors**.

The source snapshot is pinned and checksummed in
[`vendor/vbam/provenance.json`](vendor/vbam/provenance.json). Original notices
are preserved. This project is distributed under **GPL-2.0-or-later**; see
[LICENSE](LICENSE) and [NOTICE.md](NOTICE.md).

This is an independent shader port, not an official mGBA or VBA-M project.
No ROMs, BIOS files, or commercial game artwork are included.
