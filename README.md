# VBA-M HQX shaders for mGBA

[![Validate shaders](https://github.com/krupar101/mgba-vba-hqx-shaders/actions/workflows/validate.yml/badge.svg)](https://github.com/krupar101/mgba-vba-hqx-shaders/actions/workflows/validate.yml)
[![License: GPL v2 or later](https://img.shields.io/badge/license-GPL--2.0--or--later-blue)](LICENSE)

**HQ2X and HQ4X shaders that bring VBA-M’s portable 32-bit HQX filters to mGBA.**
Ready to load, with the upstream pattern tables, a reproducible generator, and
pixel-by-pixel tests against compiled VBA-M code.

![Nearest-neighbor, HQ2X and HQ4X rendering of original synthetic pixel art](assets/comparison.png)

The preview uses the actual shaders and original synthetic artwork. HQ2X is
shown at the same final size as HQ4X using nearest-neighbor enlargement.

## Choose a shader

| Shader | Pass resolution for GBA | Useful for |
| --- | --- | --- |
| **HQ2X (VBA-M)** | 480 × 320 | A lighter filter with a 2× output grid |
| **HQ4X (VBA-M)** | 960 × 640 | A finer 4× output grid |

Both preserve VBA-M’s integer interpolation and color comparisons, use nearest
sampling, and clamp samples at frame boundaries through mGBA’s texture setup.
No lookup textures, additional shader passes, or emulator modifications are needed.

**Compatibility target:** the pinned VBA-M **portable 32-bit C implementation**.
These are not advertised as bit-identical to the legacy x86 assembly filters,
FFmpeg HQX, or mGBA’s upstream HQ2X shader. See [the exact scope and limitations](docs/COMPATIBILITY.md).

## Install

1. Download and extract the ZIP from [Releases](https://github.com/krupar101/mgba-vba-hqx-shaders/releases), or clone this repository.
2. Copy `shaders/hq2x.shader` and/or `shaders/hq4x.shader` to a location mGBA can read. Keep each folder intact: it contains both a `manifest.ini` and a fragment shader.
3. Open mGBA’s shader settings and load the desired `.shader` **folder**. Depending on the version, shader settings are available under **Tools → Settings → Shaders**.
4. Reload the shader or restart mGBA after replacing an existing installation.

For the **Linux Flatpak** (`io.mgba.mGBA`), a convenient location is:

```text
~/.var/app/io.mgba.mGBA/config/mgba/shaders/
├── hq2x.shader/
│   ├── manifest.ini
│   └── hq2x.fs
└── hq4x.shader/
    ├── manifest.ini
    └── hq4x.fs
```

Run this from the extracted project directory to install both with a backup:

```bash
shader_dir="$HOME/.var/app/io.mgba.mGBA/config/mgba/shaders"
backup_dir="$HOME/.var/app/io.mgba.mGBA/config/mgba/shader-backups/hqx-$(date +%Y%m%d-%H%M%S)"
mkdir -p "$shader_dir" "$backup_dir"
for shader in hq2x.shader hq4x.shader; do
    if [ -d "$shader_dir/$shader" ]; then
        cp -a "$shader_dir/$shader" "$backup_dir/"
    fi
    cp -a "shaders/$shader" "$shader_dir/"
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
make test
```

The suite regenerates the shaders, compiles an independent C++ reference using
the original VBA-M tables/interpolation code, and checks:

- all 256 binary neighborhood patterns;
- 256 deterministic randomized color frames, including nearby colors;
- every output pixel, including frame boundaries, for each of those 512 frames;
- full-size 240 × 160 GBA frames rendered at both 2× and 4×.

`make verify` also checks that committed shaders match the generator output.
Tests are headless and use Mesa software rendering; they do not certify every
GPU or prove equality with an entire VBA-M display pipeline.

```bash
make generate  # regenerate shaders and C++ reference wrappers
make package   # ZIP with installable shaders, license and corresponding source
make preview   # regenerate the comparison image; requires Pillow
```

For preview generation, install Pillow in your preferred Python environment
(`python3 -m pip install Pillow`). See [implementation notes](docs/IMPLEMENTATION.md)
and [contribution guidelines](CONTRIBUTING.md) before changing the filters.

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
