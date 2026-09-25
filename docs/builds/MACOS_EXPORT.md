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
