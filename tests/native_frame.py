"""Smoke-test source dimensions, borders and orientation at native GBA size."""

import gl_render as gl
import os

pixels = [
    (20, 40, 60) if x < gl.WIDTH // 2 else (220, 180, 140)
    for y in range(gl.HEIGHT)
    for x in range(gl.WIDTH)
]
actual = gl.render(pixels)
mode = os.environ.get("HQX_VARIANT", "")
def quantized(rgb):
    if not mode:
        return rgb
    r,g,b = rgb
    if mode == "classic-565":
        return ((r>>3)*255//31, (g>>2)*255//63, (b>>3)*255//31)
    if mode == "classic-555":
        return ((r>>3)*255//31, (g>>3)*255//31, (b>>3)*255//31)
    raise ValueError(mode)
expected = [
    (*quantized(pixels[(y // gl.SCALE) * gl.WIDTH + x // gl.SCALE]), 255)
    for y in range(gl.OUT_H)
    for x in range(gl.OUT_W)
]
assert actual == expected, "Native-size frame: edge, border or dimension mismatch"
name = "HQ2X @4x" if gl.SCALE == 4 and gl.BUNDLE.startswith("hq2x-") else f"HQ{gl.SCALE}X"
# The first frame checks EVERY output pixel in a vertical two-color split.
# That pattern alone cannot detect a top/bottom flip. Use four distinct
# corner colors in a second render, probing well inside each constant region.
# This catches orientation, channel swaps and bad sampling at native size.
if gl.WIDTH >= 16 and gl.HEIGHT >= 16:
    corners = ((31, 89, 203), (222, 42, 72), (14, 183, 43), (245, 215, 109))
    quadrants = [
        corners[(2 if y >= gl.HEIGHT // 2 else 0) + (1 if x >= gl.WIDTH // 2 else 0)]
        for y in range(gl.HEIGHT) for x in range(gl.WIDTH)
    ]
    actual = gl.render(quadrants)
    for x, y, rgb in (
        (4, 4, corners[0]), (gl.WIDTH - 5, 4, corners[1]),
        (4, gl.HEIGHT - 5, corners[2]),
        (gl.WIDTH - 5, gl.HEIGHT - 5, corners[3]),
    ):
        expected_color = (*quantized(rgb), 255)
        for yy in range(y * gl.SCALE, (y + 1) * gl.SCALE):
            for xx in range(x * gl.SCALE, (x + 1) * gl.SCALE):
                assert actual[yy * gl.OUT_W + xx] == expected_color, (
                    'quadrant/orientation/color mismatch', x, y, xx, yy,
                    actual[yy * gl.OUT_W + xx], expected_color
                )
print(f"PASS {name}: {gl.WIDTH}x{gl.HEIGHT} -> {gl.OUT_W}x{gl.OUT_H}; full-frame + four-quadrant checks")
