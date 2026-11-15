VBA-M HQ4X shader for mGBA

Compatibility target: VBA-M current portable 32-bit C implementation.
This is not a claim of equivalence to the legacy 16-bit-input x86 assembly.
The source comparison behavior, integer interpolation and pattern rules
are preserved, including the portable HQ4X packed-color comparison.

GPL-2.0-or-later; upstream copyright notices are in the fragment shader.
Source: https://github.com/visualboyadvance-m/visualboyadvance-m/tree/master/src/components/filters/internal

Validation: OpenGL compilation/linking with mGBA 0.10.5 headers;
pixel-exact output against compiled upstream C pattern/interpolation code
for 512 8x8 frames (256 binary patterns and 256 randomized color frames).
Pattern dispatch uses a balanced tree to avoid the rendering mismatch
observed with a large flat dispatch on the validation driver.

Reload the shader or restart mGBA after installation.
