HQ2X RGB565 legacy-color variant.
HQ2X follows classic VisualBoyAdvance 16-bit C filtering, not
the x86 assembly HQ4X implementation.
Quantizes mGBA's RGB888 source to RGB565 before filtering, and converts the
result back to RGB888. Choose the color mode you prefer visually.
See docs/COMPATIBILITY.md for limitations and testing scope.
