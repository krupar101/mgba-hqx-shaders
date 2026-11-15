"""Fast structural check: 4x bundles must be identical HQ2x math at 4x output."""
from pathlib import Path
root = Path(__file__).resolve().parents[1] / "shaders"
for bits in (565, 555):
    base = root / f"hq2x-classic-{bits}.shader"
    zoom = root / f"hq2x-classic-{bits}-4x.shader"
    assert (base / "hq2x.fs").read_bytes() == (zoom / "hq2x.fs").read_bytes(), bits
    norm = (base / "manifest.ini").read_text()
    scaled = (zoom / "manifest.ini").read_text()
    assert "width=-2\nheight=-2" in norm
    assert "width=-4\nheight=-4" in scaled
    assert "fragmentShader=hq2x.fs" in scaled
    print(f"PASS RGB{bits} 4x: exact same shader as 2x, only output grid doubles")
