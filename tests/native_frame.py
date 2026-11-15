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
print(f"PASS {name}: {gl.WIDTH}x{gl.HEIGHT} -> {gl.OUT_W}x{gl.OUT_H}")
