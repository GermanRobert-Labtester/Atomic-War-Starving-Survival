# Deprecated Item Sprites Quarantine

**Date Quarantined:** 2026-08-27
**Task:** Task 89 - Quarantine deprecated Godot item sprites
**Total Files:** 83 (41 PNG sprites + 41 Godot .import files + 1 prompt file)

## Contents

This folder contains deprecated item sprite assets that were identified as orphans (not referenced by any game data, code, scenes, or snapshot manifests).

### File Categories

1. **Ammo Sprites (38 PNG + 38 .import files)**
   - `ammo_deprecated_12ga.png` through `ammo_deprecated_cal_9x21.png`
   - Various calibers: 12ga, 16ga, 300blk, 338lapua, 380acp, 408cheytac, 45acp, 46x30, 50bmg, 545x39, 556x45, 57x28, 762x25, 762x39, 762x51, 762x54r, 765x21, 9x19, 9x21

2. **Armor & Helmet Sprites (4 PNG + 4 .import files)**

2. **Armor & Helmet Sprites (4 PNG + 4 .import files)**
   - `body_armour_deprecated.png`
   - `body_armour_heavy_deprecated.png`
   - `helmet_deprecated.png`
   - `helmet_heavy_deprecated.png`

3. **Prompt File (1 file)**
   - `Prompts/batch_A1_deprecated_ammo.txt` - Generation prompt for deprecated ammo sprites

## Verification Performed

### References Checked (NONE FOUND in runtime assets):
- ✅ JSON data authority (`Assets/StreamingAssets/Data/`)
- ✅ C# code (`src/`)
- ✅ Godot scenes and resources (`.tscn`, `.tres`)
- ✅ Snapshot manifests (`docs/ui/snapshot_*.json`)

### References Found (documentation only):
- ⚠️  `Assets/sprites/asset_manifest.json` (Unity-era tracking manifest)
- ⚠️  `docs/visual/visual_asset_manifest.json` (documentation)
- ⚠️  `docs/visual/_trace_phase13_baseline.json` (documentation)
- ⚠️  `docs/visual/WIRING_MATRIX.json` (documentation)

These documentation files reference the deprecated assets but are not used at runtime.

## Classification

**Status:** PROVEN ORPHANS - Safe for quarantine/removal

**Rationale:** No runtime code, data, or scenes reference these assets. All references are in documentation/tracking files only.

## Next Steps

These files may be:
1. Permanently deleted after verification gates pass
2. Archived for historical reference
3. Reused if similar assets are needed in future

## Do Not Reintroduce

These assets were deprecated for a reason. If similar functionality is needed, create new assets with proper naming conventions and register them in the appropriate catalogs.
