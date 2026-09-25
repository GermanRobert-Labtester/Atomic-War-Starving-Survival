# ASHFALL — macOS Export Runbook

Status 2026-09-25: **preset added, export blocked on this Linux host by a Godot exporter defect.**
Export from a Mac (or a newer Godot build) with the steps below.

## What is already in the repo

* `export_presets.cfg` → preset `macOS`: universal binary, `builds/macos/ashfall.zip`,
  ETC2 ASTC + S3TC/Bptc texture formats, bundle id `team.ashfall.atomicwar`,
  `exclude_filter="generated_AIassets/*"`, data include filter
  `Assets/StreamingAssets/Data/*,assets/l10n/*.csv`.
* Export template `macos.zip` is installed for `4.7.1.stable.mono`.

## Required project setting (step 1 on any host)

macOS universal/arm64 exports require ETC2 ASTC import variants:

```ini
# project.godot
[rendering]
textures/vram_compression/import_etc2_astc=true
```

This is currently **false** because the macOS export is host-blocked; turning it on re-imports
textures with ETC2/ASTC variants (larger PCK) and is only worth doing on the Mac export host.
After flipping it, run `godot --headless --path . --import` once.

## Export

```bash
godot --headless --path . --import          # bake imports first
godot --headless --path . --export-release "macOS" builds/macos/ashfall.zip
```

The result is a zip containing `ASHFALL...app` with the PCK embedded. Smoke it on the Mac
(`open` the .app) and run the in-game data-integrity self-test once from the shipped app's CLI.

## Signing / notarization (Mac host only)

1. Apple Developer ID Application certificate in the login keychain.
2. In the preset: `codesign/codesign=2` (Xcode codesign), set `codesign/identity` and
   `codesign/apple_team_id`.
3. Export as above, then:
   `xcrun notarytool submit builds/macos/ashfall.zip --keychain-profile <profile> --wait`
   and `xcrun stapler staple <the exported .app>`.
4. Verify: `spctl --assess --type execute -vv <the .app>`.

## Blocked-on-Linux evidence (for whoever picks this up)

With the preset and every import baked (zero `.import` sidecars missing), export on this Linux
host fails deterministically inside Godot's pack writer while the **identical tree** exports
cleanly for Linux/X11 and Windows Desktop:

```
ERROR: Condition "pd->f->get_position() - sd.ofs < (uint64_t)p_data.size()" is true. Returning: ERR_FILE_CANT_WRITE
   at: _save_pack_file (editor/export/editor_export_platform.cpp:455)
ERROR: Save PCK: Failed to export project files.
ERROR: Project export for preset "macOS" failed.
```

Tried and ruled out: stale import cache (`rm -rf .godot/exported`), on-the-fly import races (full
`--import` with the art wave paused), `binary_format/embed_pck=false`, and mid-wave writes
(generator SIGSTOPped during export). The failure point moves between runs (69–71%) which points
at a host-side exporter/pack defect in `4.7.1.stable.mono`, not at project data.

## RESOLVED 2026-09-26 — root cause + working recipe

**The blocker was the `.zip` packaging step, not the pack writer.** Two
independent causes were confirmed:

1. **ETC2 ASTC gate:** the macOS preset (universal) refuses to export while
   `rendering/textures/vram_compression/import_etc2_astc=false`.
2. **`.zip` target defect:** every macOS export that ended in `.zip` failed in
   `_save_pack_file` at 69–71%, independent of the art wave. Exporting to a
   **`.app` bundle target** succeeds on the same tree and the same Godot 4.7.1.

### Working Linux-host recipe

```bash
# 1. enable ETC2 ASTC (required for universal/arm64)
sed -i 's#import_etc2_astc=false#import_etc2_astc=true#' project.godot
# 2. export the bundle (NOT .zip)
godot --headless --path . --export-release "macOS" "builds/macos/ashfall.app"
# 3. package it yourself, preserving symlinks
(cd builds/macos && zip -q -y -r ashfall-macos-universal-$(date +%Y%m%d).zip ashfall.app)
# 4. return the project to the alpha import policy (ASTC off, cache clean)
sed -i 's#import_etc2_astc=true#import_etc2_astc=false#' project.godot
rm -rf .godot/imported && godot --headless --path . --import
```

Measured 2026-09-26: `.app` 531 MB, in-bundle PCK 175.3 MB (universal),
`data_Ashfall_macos_{x86_64,arm64}` present (188 files each), zip 269 MB.

### Cost note

Enabling ETC2 ASTC grows the Linux PCK (128.4 MB → 175.6 MB measured, because
every texture gains ASTC variants). Toggling the setting back **does not**
evict them; the import cache must be cleared and regenerated. Treat ASTC as an
export-host toggle, not a project default, until the alpha download budget is
settled. Residual after a clean re-import: 163.3 MB (the art wave has grown
since the 128.4 MB row — 1,964 JPGs on disk).

### Still requires a Mac

Code signing, notarization, and Gatekeeper validation
(`xcrun notarytool submit … --wait` then `xcrun stapler staple`) cannot run on
Linux. The bundle is unsigned: expect Gatekeeper to quarantine it until the
release-captain runs the runbook's signing steps.
