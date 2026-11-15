"""Compare rendered frames with the compiled, original VBA-M C tables."""

import sys, importlib, ctypes as C, random, os
from pathlib import Path

n = int(sys.argv[1])
zoom = os.environ.get("HQX_SHADER_BUNDLE", "").startswith("hq2x-") and n == 4
reference_scale = 2 if zoom else n
gl = importlib.import_module("gl_render")
ref = C.CDLL(
    str(Path(__file__).resolve().parents[1] / "build" / (f"ref{reference_scale}" + (f"-{os.environ['HQX_VARIANT']}" if os.environ.get("HQX_VARIANT") else "") + ".so"))
).reference
ref.argtypes = [C.POINTER(C.c_uint32), C.POINTER(C.c_uint32)]
assert n in (2, 4)
assert gl.WIDTH == gl.HEIGHT == 8
frames = []
for mask in range(256):
    f = [(0, 0, 0)] * 64
    for bit, i in enumerate([0, 1, 2, 3, 5, 6, 7, 8]):
        if mask & (1 << bit):
            f[(2 + i // 3) * 8 + 2 + i % 3] = (255, 255, 255)
    frames.append(f)
r = random.Random(417)
for k in range(256):
    palette = (
        [tuple(r.randrange(256) for _ in range(3)) for _ in range(8)]
        if k < 128
        else [tuple(100 + r.randrange(64) for _ in range(3)) for _ in range(8)]
    )
    frames.append([r.choice(palette) for _ in range(64)])
for fi, f in enumerate(frames[:int(os.environ.get("HQX_FRAME_LIMIT", len(frames)))]):
    actual = gl.render(f)
    for y in range(8):
        for x in range(8):
            colors = []
            for dy in [-1, 0, 1]:
                for dx in [-1, 0, 1]:
                    rr, g, b = f[max(0, min(7, y + dy)) * 8 + max(0, min(7, x + dx))]
                    colors.append((rr << 16) | (g << 8) | b)
            if reference_scale == 4:
                colors.insert(0, 0)
            inp = (C.c_uint32 * len(colors))(*colors)
            out = (C.c_uint32 * (reference_scale * reference_scale))()
            ref(inp, out)
            for sy in range(n):
                for sx in range(n):
                    v = out[(sy // 2 if zoom else sy) * reference_scale + (sx // 2 if zoom else sx)]
                    expected = ((v >> 16) & 255, (v >> 8) & 255, v & 255, 255)
                    got = actual[(y * n + sy) * 8 * n + x * n + sx]
                    assert got == expected, (fi, x, y, sx, sy, got, expected)
print(
    f"PASS {'HQ2X @4x' if zoom else f'HQ{n}X'} {os.environ.get('HQX_VARIANT','VBA-M 32-bit')}: all pixels match compiled VBA-M C reference across {min(len(frames), int(os.environ.get('HQX_FRAME_LIMIT', len(frames))))} frames"
)
