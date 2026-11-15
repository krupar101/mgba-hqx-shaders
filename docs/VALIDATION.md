# Validation

The maintained filters are original portable 32-bit VBA-M HQ2x and HQ4x.
`make verify` checks reproducible generation, bundle metadata, documentation
links and every output pixel against compiled pinned C references across
512 frames per shader. Native 240×160 frames test borders and orientation.
`make package-test` verifies exactly two bundles, eight files and SHA-256.
`make preview` validates all three synthetic comparison panels against C output.
These are headless Mesa tests, not certification of all GPUs or the mGBA GUI.
