# Changelog

## Verification and packaging audit — 2026-11-15

- Verify generated shaders without requiring a Git checkout or overwriting the shipped files.
- Add independent shader bundle/manifest checks.
- Package only the six loadable shader bundles in the release ZIP; keep development sources in Git.
- Add archive file-by-file and SHA-256 validation; run this in GitHub Actions.
- Clarify shader-only release packaging and installation instructions.
- Fix misleading classic 4x test labels, serialize `make verify` checks, and avoid a deprecated Pillow preview API.
- Document the full validation results and their limits in `docs/VALIDATION.md`.

## Classic VBA-style HQ2x extension

- Add independent RGB565 and RGB555 classic 16-bit HQ2x shaders, retaining both modern shaders.
- Add 4x-display versions that reproduce 2x HQ2x pixels at a crisp 4x grid (not HQ4x).
- Add generator and compiled C reference tests for 16-bit interpolation and neighborhood differences.
- Document the scope of old-VBA visual similarity separately from guaranteed pixel equality.


## 0.1.0 — 2026-11-15

- Publish standalone HQ2X and HQ4X bundles for mGBA.
- Target pinned VBA-M portable 32-bit C interpolation, comparison and pattern rules.
- Add reproducible shader generation and source checksums.
- Add headless OpenGL-to-C reference comparisons and native GBA-size checks.
- Include installation instructions, original synthetic previews and source-complete ZIP packaging.

The earlier mGBA-upstream HQ2X and FFmpeg-derived HQ4X experiments are not part
of this release. Legacy VBA-M assembly equivalence is not claimed.
