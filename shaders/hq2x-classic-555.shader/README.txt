HQ2X RGB555 legacy-color variant.
HQ2X follows classic VisualBoyAdvance 16-bit C filtering, not
the x86 assembly HQ4X implementation.
Quantizes mGBA's RGB888 source to RGB555 before filtering, and converts the
result back to RGB888. Choose the color mode you prefer visually.
Compatibility notes and limitations:
https://github.com/krupar101/mgba-vba-hqx-shaders/blob/main/docs/COMPATIBILITY.md
