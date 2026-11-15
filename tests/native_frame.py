"""Smoke-test source dimensions, borders and orientation at native GBA size."""

import gl_render as gl

pixels = [
    (20, 40, 60) if x < gl.WIDTH // 2 else (220, 180, 140)
    for y in range(gl.HEIGHT)
    for x in range(gl.WIDTH)
]
actual = gl.render(pixels)
expected = [
    (*pixels[(y // gl.SCALE) * gl.WIDTH + x // gl.SCALE], 255)
    for y in range(gl.OUT_H)
    for x in range(gl.OUT_W)
]
assert actual == expected, "Native-size frame: edge, border or dimension mismatch"
print(f"PASS HQ{gl.SCALE}X: {gl.WIDTH}x{gl.HEIGHT} -> {gl.OUT_W}x{gl.OUT_H}")
