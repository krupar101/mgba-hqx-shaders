#!/usr/bin/env python3
"""Render an original, synthetic pixel-art comparison; requires Pillow."""

from pathlib import Path
import subprocess
import sys
from PIL import Image, ImageDraw, ImageFont

ROOT = Path(__file__).resolve().parents[1]
BUILD = ROOT / "build"
BUILD.mkdir(exist_ok=True)
if len(sys.argv) > 1:
    scale = int(sys.argv[1])
    sys.argv = [sys.argv[0], str(scale), "96", "64"]
    sys.path.insert(0, str(ROOT / "tests"))
    import gl_render

    source = Image.open(BUILD / "sample.png").convert("RGB")
    rendered = gl_render.render(list(source.getdata()))
    image = Image.new("RGBA", (96 * scale, 64 * scale))
    image.putdata(rendered)
    image.convert("RGB").save(BUILD / f"hq{scale}x-preview.png")
    raise SystemExit

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
for scale in (2, 4):
    subprocess.run([sys.executable, str(Path(__file__)), str(scale)], check=True)
canvas = Image.new("RGB", (1248, 400), "#0b1220")
draw = ImageDraw.Draw(canvas)
font = ImageFont.load_default(size=25)
small = ImageFont.load_default(size=16)
draw.text((24, 18), "mGBA / VBA-M HQX", fill="#e5efff", font=font)
draw.text(
    (24, 55),
    "Same original pixel art. Three scaling methods. No game assets.",
    fill="#99acc6",
    font=small,
)
for i, (label, path) in enumerate(
    [
        ("NEAREST / 4x", BUILD / "sample.png"),
        ("HQ2X / displayed at 4x", BUILD / "hq2x-preview.png"),
        ("HQ4X / native 4x", BUILD / "hq4x-preview.png"),
    ]
):
    x = 24 + i * 408
    draw.text((x, 98), label, fill="#70e1cb", font=small)
    image = Image.open(path).resize((384, 256), Image.Resampling.NEAREST)
    canvas.paste(image, (x, 126))
canvas.save(ROOT / "assets/comparison.png")
print("Saved assets/comparison.png (rendered using the shipped shaders).")
