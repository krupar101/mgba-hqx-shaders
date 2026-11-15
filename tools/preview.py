#!/usr/bin/env python3
"""Render a reproducible three-way HQX comparison from original synthetic art.

The two HQ2x panels are displayed at 4x using *nearest-neighbor* enlargement;
only the VBA-M HQ4x panel uses a genuine HQ4x algorithm.
Requires Pillow and the standard Mesa/EGL shader test harness.
"""

from pathlib import Path
import os
import subprocess
import sys
from PIL import Image, ImageDraw, ImageFont

ROOT = Path(__file__).resolve().parents[1]
BUILD = ROOT / "build"
BUILD.mkdir(exist_ok=True)

# Child mode: render exactly one shader at its native output resolution.
# This uses the same mGBA-compatible OpenGL shader compilation and rendering
# harness as the pixel-exact reference comparisons.
if len(sys.argv) > 1:
    variant = sys.argv[1]
    configs = {
        "modern-hq2x": (2, None),
        "modern-hq4x": (4, None),
    }
    if variant not in configs:
        raise SystemExit(f"Unknown preview variant: {variant}")
    scale, color_variant = configs[variant]
    sys.argv = [sys.argv[0], str(scale), "96", "64"]
    sys.path.insert(0, str(ROOT / "tests"))
    if color_variant:
        os.environ["HQX_VARIANT"] = color_variant
    else:
        os.environ.pop("HQX_VARIANT", None)
    os.environ.pop("HQX_SHADER_BUNDLE", None)
    import gl_render

    source = Image.open(BUILD / "sample.png").convert("RGB")
    pixels = (
        source.get_flattened_data()
        if hasattr(source, "get_flattened_data")
        else source.getdata()
    )
    rendered = gl_render.render(list(pixels))
    image = Image.new("RGBA", (96 * scale, 64 * scale))
    image.putdata(rendered)
    image.convert("RGB").save(BUILD / f"{variant}-preview.png")
    raise SystemExit

# An original 96x64 scene avoids commercial game artwork and makes HQX edge
# behavior easy to inspect at a consistent final 4x size.
source = Image.new("RGB", (96, 64), "#14223a")
draw = ImageDraw.Draw(source)
for y in range(34):
    draw.line((0, y, 95, y), fill=(20 + y, 34 + y, 58 + y))
draw.ellipse((61, 5, 80, 24), fill="#ffdf96")
draw.polygon([(0, 45), (27, 13), (51, 45)], fill="#536ba0")
draw.polygon([(15, 27), (27, 13), (38, 27), (29, 23), (24, 28)], fill="#cee8eb")
draw.polygon([(31, 46), (60, 25), (95, 46)], fill="#379b98")
draw.rectangle((0, 46, 95, 63), fill="#172c46")
for y, x in [(49, 8), (53, 13), (57, 5)]:
    draw.line((x, y, x + 47, y), fill="#58b5bd")
draw.rounded_rectangle(
    (66, 36, 86, 59), radius=5, fill="#f1b75d", outline="#fff0ba", width=1
)
draw.rectangle((70, 43, 73, 46), fill="#14223a")
draw.rectangle((79, 43, 82, 46), fill="#14223a")
draw.line((72, 52, 80, 52), fill="#14223a", width=1)
source.save(BUILD / "sample.png")

for variant in ("modern-hq2x", "modern-hq4x"):
    subprocess.run([sys.executable, str(Path(__file__)), variant], check=True)

# Every panel is exactly 384x256, so the comparison does not favor one filter
# through a different final display size or a second bilinear filtering pass.
canvas = Image.new("RGB", (1660, 400), "#0b1220")
draw = ImageDraw.Draw(canvas)
font = ImageFont.load_default(size=25)
small = ImageFont.load_default(size=16)
draw.text((24, 18), "mGBA / VBA HQX - Shader comparison", fill="#e5efff", font=font)
draw.text(
    (24, 55),
    "One synthetic frame. All panels at 4x. HQ2x panels use nearest-neighbor display scaling.",
    fill="#99acc6",
    font=small,
)
previews = [
    ("NEAREST / 4x", BUILD / "sample.png"),
    ("VBA-M HQ2X / 2x -> 4x", BUILD / "modern-hq2x-preview.png"),
    ("VBA-M HQ4X / native 4x", BUILD / "modern-hq4x-preview.png"),
]
for i, (label, path) in enumerate(previews):
    x = 24 + i * 408
    draw.text((x, 98), label, fill="#70e1cb", font=small)
    image = Image.open(path).convert("RGB").resize((384, 256), Image.Resampling.NEAREST)
    canvas.paste(image, (x, 126))
canvas.save(ROOT / "assets/comparison.png")
print("Saved assets/comparison.png (three panels rendered using shipped shaders).")
