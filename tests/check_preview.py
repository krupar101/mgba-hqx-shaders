#!/usr/bin/env python3
"""Verify the README comparison against each compiled C filter reference.

Run after `make preview`; requires Pillow and the compiled build/ref*.so files.
"""

import ctypes as C
from pathlib import Path
from PIL import Image, ImageChops

ROOT = Path(__file__).resolve().parents[1]
BUILD = ROOT / "build"
SOURCE = Image.open(BUILD / "sample.png").convert("RGB")
assert SOURCE.size == (96, 64)
W, H = SOURCE.size
PIXELS = list(SOURCE.get_flattened_data() if hasattr(SOURCE, "get_flattened_data") else SOURCE.getdata())

# Check the *actual rendered* pixels, not only that each image has the right
# dimensions. Each 3x3 reference neighborhood uses edge clamping matching mGBA.
previews = (
    ("modern-hq2x", 2, "ref2.so"),
    ("classic-565-hq2x", 2, "ref2-classic-565.so"),
    ("modern-hq4x", 4, "ref4.so"),
)
for variant, scale, reference_file in previews:
    img = Image.open(BUILD / f"{variant}-preview.png").convert("RGB")
    assert img.size == (W * scale, H * scale), (variant, img.size)
    raw = img.load()
    ref = C.CDLL(str(BUILD / reference_file)).reference
    ref.argtypes = [C.POINTER(C.c_uint32), C.POINTER(C.c_uint32)]
    for y in range(H):
        for x in range(W):
            neighborhood = []
            for dy in (-1, 0, 1):
                yy = max(0, min(H - 1, y + dy))
                for dx in (-1, 0, 1):
                    xx = max(0, min(W - 1, x + dx))
                    r, g, b = PIXELS[yy * W + xx]
                    neighborhood.append((r << 16) | (g << 8) | b)
            if scale == 4:
                neighborhood.insert(0, 0)
            pixels_in = (C.c_uint32 * len(neighborhood))(*neighborhood)
            pixels_out = (C.c_uint32 * (scale * scale))()
            ref(pixels_in, pixels_out)
            for oy in range(scale):
                for ox in range(scale):
                    v = pixels_out[oy * scale + ox]
                    expected = ((v >> 16) & 255, (v >> 8) & 255, v & 255)
                    actual = raw[x * scale + ox, y * scale + oy]
                    assert actual == expected, (variant, x, y, ox, oy, actual, expected)
    print(f"PASS {variant}: all {W*H*scale*scale:,} preview pixels equal compiled C reference")

# The composited README graphic must contain those exact images (and original
# pixel art) without a hidden resampling/color transformation.
canvas = Image.open(ROOT / "assets" / "comparison.png").convert("RGB")
assert canvas.size == (1660, 400), canvas.size
paths = [BUILD / "sample.png"] + [BUILD / f"{variant}-preview.png" for variant, _, _ in previews]
for i, path in enumerate(paths):
    x, y = 24 + i * 408, 126
    actual = canvas.crop((x, y, x + 384, y + 256))
    expected = Image.open(path).convert("RGB").resize((384, 256), Image.Resampling.NEAREST)
    assert ImageChops.difference(actual, expected).getbbox() is None, f"Incorrect preview panel {i}: {path}"
print("PASS comparison.png: all four labeled panels contain the exact intended output pixels")
