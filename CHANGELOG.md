## Unreleased

- Keep only original HQ2x and HQ4x. Remove classic variants and experimental smoothing/preservation filters, their tools and documentation. Release ZIP contains exactly two shader bundles.

## 2026-11-15 — README graphic verification and rendering audit

- Extend the comparison graphic to show classic RGB565 HQ2x beside nearest-neighbor, modern VBA-M HQ2x and true VBA-M HQ4x, all at a fixed 4x final display size.
- Add a full-preview pixel audit against independent compiled C references and check the final composite panels for unintended resizing or color changes.
- Replace ambiguous overview text with precise, explicit labels and compatibility limitations.
- Keep the shader-only release unchanged: these edits affect presentation, tests and source documentation only.

## 2026-11-15 — second-pass independent audit

- Repair the optional historical-source harness to support both reference wrapper layouts (it previously failed for RGB888); require the original `hq2x.cpp` as well as the two headers and extract its interpolation macros rather than reusing pinned modern definitions.
- Add an offline structural regression test of the historical harness, explicitly labeled synthetic rather than a VBA 1.8 compatibility result.
- Strengthen native 240×160 tests with a second, four-quadrant frame to catch top/bottom inversion and RGB channel mix-ups.
- Fix standalone classic shader README instructions that pointed to documentation absent from the shaders-only ZIP.
- Add automated local-link checks for the full Markdown documentation.
- Revalidate shader generation, packed source and shader-only release archives; no unsupported edits to shader interpolation rules.

## 2026-11-15 — historical VBA compatibility review and shader-only releases

- Reviewed original VBA 1.8 HQ2x implementation and 15/16/32-bit interpolation interfaces; kept classic algorithm behavior unchanged rather than inventing unvalidated adjustments.
- Added optional C-vs-GLSL comparison against user-supplied **original VBA 1.8** `hq2x.cpp`, `hq2x.h` and `interp.h` (`make vba18-test`).
- Changed `make package` and GitHub Actions to publish only the six installable `.shader` folders (with their GPL license), not the source repository.
- Strengthened package tests to reject every source/documentation/workflow file outside the shader folders and make ZIP metadata deterministic across checkouts.
- Updated README, historical source review and verification scope. Original VBA 1.8 had HQ2x, not HQ4x.

# Changelog

## Verification and packaging audit — 2026-11-15

- Verify generated shaders without requiring a Git checkout or overwriting the shipped files.
- Add independent shader bundle/manifest checks.
- Include GitHub workflows, issue template and hidden repository configuration in the **source** ZIP (not the shader-only release).
- Add archive file-by-file and SHA-256 validation; run this in GitHub Actions.
- Clarify the full-source archive commands in the README and contribution guide.
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
