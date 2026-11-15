# Compatibility and validation scope

## What “VBA-M” means here

The target is VBA-M’s portable 32-bit C implementation at the revision in
`vendor/vbam/provenance.json`. HQ2X uses `hq2x.h` and `interp.h`; HQ4X uses
`hq4x_pattern.h`, `hq_base.h` and `hq_shared.h`.

This differs from the legacy `hq/asm/hq4x_32.asm` path. Despite its name, that
assembly implementation uses 16-bit color lookup tables and produces 32-bit
output. Its comparison behavior is not the target of these shaders.

The portable HQ4X comparison deliberately preserves the original packed YUV
arithmetic and `& 0x7FFFFFFF` difference checks. These are not interchangeable
with a conventional absolute YUV distance. In particular, the upstream check
can be direction-dependent. Changing it to a mathematically conventional
metric would break compatibility; any such alternative needs a separate name.

HQ2X preserves the upstream RGB quantization shortcut, thresholds, integer
interpolation and pattern rules. It therefore differs from the mGBA upstream
HQ2X shader previously used as the starting point for this project.

## Rendering requirements

- mGBA’s modern desktop OpenGL shader path, with GLSL integer/bitwise support.
- mGBA supplies `tex`, `texSize`, `texCoord` and the GLSL version header.
- Fixed 2× or 4× pass dimensions are expressed as negative multiples in the manifest.
- Nearest-neighbor source sampling and disabled blending.
- No support claim for OpenGL ES 2.0 or the legacy OpenGL renderer.

The shader headers were tested against mGBA 0.10.5’s desktop OpenGL interface.
The development validation driver was Mesa software OpenGL. Loading the shader
on a different GPU may take longer or expose compiler differences. Report the
GPU/driver and any compilation log when filing an issue.

## What the tests prove

For the supplied frames, actual OpenGL fragment output matches compiled
upstream C interpolation and table output in every RGB channel. The reference
uses the original C switch tables, not the GLSL decision tree. Alpha is always
opaque in the shader and is not part of VBA-M’s packed RGB comparison.

The 512-frame suite includes all binary neighborhood masks and deterministic
random color neighborhoods, but it does not enumerate every 24-bit color
combination. Native-size tests also check a flat two-color frame and its borders.

This does not prove identical screenshots across emulators: framebuffer color
conversion, color correction, window size, final interpolation, and capture
scaling can all differ. Compare identical RGB input frames at native filter
resolution when diagnosing an algorithm mismatch.

## Classic VBA-style HQ2X (RGB565 and RGB555)

The `hq2x-classic-*.shader` options implement the **16-bit** `hq2x_16_def`
algorithm from the classic VisualBoyAdvance lineage, using the pinned VBA-M
HQ2x pattern table and `interp_16_*` functions. Input RGB888 is quantized to
RGB565 (16-bit) or RGB555 (15-bit), and **all** color differences, interpolation
and truncation then operate on packed 16-bit values. Output channels are
expanded back to RGB888 using integer `value * 255 / max` conversion.

The original 1.8.0 release offered HQ2x (and LQ2x) but did not offer the later
VBA-M HQ4x option. The `-4x.shader` variants therefore compute **HQ2x** and
repeat its 2x pixels over a 4x output, without bilinear resampling. They are
not HQ4x and do not claim to match the legacy x86 HQ4x assembly path.

The HQ2x patterns are taken from the pinned VBA-M source snapshot; this project
does not yet vendor a separately checksummed original VBA 1.8.0 release.
Consequently, it validates equality with the **pinned 16-bit reference code**,
not bit-identity with every historical Windows build or display pipeline.
The old emulator may use different output RGB expansion, color conversion,
filter settings or window resampling. If comparing screenshots, use the same
source frame, disable bilinear filtering, and compare at the same output size.

`make classic-test` compiles upstream 16-bit interpolation/pattern helpers and
compares rendered OpenGL 2× pixels for all 256 pattern masks, 256
randomized frames, verifies the 4× shader files are identical to their 2× counterparts, and
offers optional GBA-sized 2× border/orientation smoke tests via
`make classic-native-test`. The optional
`make classic-4x-test` separately tests 4× OpenGL pixel output and borders. Both original shaders are
still included and validated separately.
