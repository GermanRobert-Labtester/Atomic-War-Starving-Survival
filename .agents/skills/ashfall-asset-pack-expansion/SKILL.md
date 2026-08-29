---
name: ashfall-asset-pack-expansion
description: Creates the directory structure, .gitattributes LFS entries, import preset skeletons, and assets.json registry entries for a new ASHFALL expansion asset pack.
---

# ASHFALL Asset Expansion Skill: ashfall-asset-pack-expansion

## Overview
Creates a complete asset pack structure for ASHFALL expansions (Holdfast, Duty Roster, Standing Record, Crossing, etc.). Generates directories, .gitattributes LFS entries, import preset skeletons, and assets.json registry entries for godot-asset-gate.sh validation.

## Canonical Usage
```bash
# Create asset pack for expansion 05 Holdfast
awf asset-pack-expansion --expansion 05 --codename holdfast

# Create asset pack with custom paths
awf asset-pack-expansion --expansion 05 --codename holdfast --root ./custom_assets/

# Create multiple asset packs
awf asset-pack-expansion --expansion 05,06,07 --codename holdfast,duty_roster,standing_record

# Run in CI pipeline
awf asset-pack-expansion --expansion 05 --ci
```

## What It Automates

### 1. Directory Structure Creation
Creates a complete Godot-native asset tree for the expansion:

```
assets/
└── expansions/
    └── 05_holdfast/
        ├── art/
        │   ├── backgrounds/
        │   │   ├── skybox/
        │   │   ├── terrain/
        │   │   └── ui/
        │   ├── characters/
        │   │   ├── survivors/
        │   │   ├── npcs/
        │   │   └── factions/
        │   ├── items/
        │   │   ├── consumables/
        │   │   ├── equipment/
        │   │   └── resources/
        │   └── environments/
        │       ├── ruins/
        │       ├── settlements/
        │       └── wasteland/
        ├── sprites/
        │   ├── items/
        │   ├── characters/
        │   ├── ui/
        │   └── effects/
        ├── ui/
        │   ├── panels/
        │   ├── dialog/
        │   ├── hud/
        │   └── menus/
        ├── audio/
        │   ├── music/
        │   ├── sfx/
        │   ├── radio/
        │   └── ambience/
        ├── materials/
        │   ├── character/
        │   ├── item/
        │   └── environment/
        ├── shaders/
        │   ├── character/
        │   ├── item/
        │   └── postprocessing/
        └── fonts/
            └── expansion/
```

### 2. Git LFS Configuration
- Creates `.gitattributes` entry for expansion asset directory
- Validates LFS is installed and configured
- Reports LFS tracking status
- Validates core.ignorecase is false (required for Assets/ vs assets/ distinction)

#### Example .gitattributes:
```
# Expansion 05 assets
assets/expansions/05_*/** filter=lfs diff=lfs merge=lfs -text
assets/expansions/05_*/** linguist-generated=true
assets/expansions/05_*/** export-ignore

# Sprite files
assets/expansions/05_*/sprites/**/*.png filter=lfs diff=lfs merge=lfs -text
assets/expansions/05_*/sprites/**/*.webp filter=lfs diff=lfs merge=lfs -text

# Audio files
assets/expansions/05_*/audio/**/*.wav filter=lfs diff=lfs merge=lfs -text
assets/expansions/05_*/audio/**/*.ogg filter=lfs diff=lfs merge=lfs -text
assets/expansions/05_*/audio/**/*.mp3 filter=lfs diff=lfs merge=lfs -text

# Font files
assets/expansions/05_*/fonts/**/*.ttf filter=lfs diff=lfs merge=lfs -text
assets/expansions/05_*/fonts/**/*.otf filter=lfs diff=lfs merge=lfs -text
```

### 3. Import Preset Skeletons
Creates Godot import preset files (.import) with correct settings:

#### Texture Import Presets:
```ini
[preset.0]

name="Expansion Sprite - 64px"
process_flip_x=false
process_flip_y=false
process_custom=true
process_force_square=false
process_keep_transparent_border=false
process_premultiply_alpha=false
process_detect_32_bits=false
process_dither=false
process_dither_lut_size=0
process_hdr_compression=false
process_compress_modes=0
process_compress_quality=0
process_normal_map=false
process_lossy_quality=0.7
process_size_limit=0
process_min_size=64
process_max_size=64
process_incremental=false
process_vram_texture=false
process_texture_flags=0
process_bptc=true
process_bptc_quality=0
process_etc=false
process_etc2=false
process_astc=false
process_webp=false
process_fallback=false
process_force_bit_depth=false
process_bit_depth=0
process_assign_srgb=false
process_use_lossless_webp=false
process_use_subsampling=false
```

#### Audio Import Presets:
```ini
[preset.0]

name="Expansion SFX"
format=0
compress=false
trimming=false
normalize=false
loop=false
loop_offset=0
split_into_tracks=false
detect_peaks=false

[preset.1]
name="Expansion Music"
format=0
compress=true
bitrate=192
quality=0
loop=true
```

#### Font Import Presets:
```ini
[preset.0]

name="Expansion Font"
font_data_path="res://assets/expansions/05_holdfast/fonts/expansion_font.ttf"
font_size=16
antialiased=true
use_mipmaps=false
use_filter=true
use_custom=true
force_autohint=false
hinting=1
subpixel_positioning=0
oversampling=1
msdf_pixel_range=8
msdf_size=48
msdf_px_range=4

[preset.1]
name="Expansion UI Font"
font_size=14
use_mipmaps=false
```

### 4. assets.json Registry Entry
Creates or updates `assets/expansions/assets.json` registry:

```json
{
  "schema_version": "1",
  "expansions": {
    "05_holdfast": {
      "id": "expansion_05",
      "codename": "holdfast",
      "version": "1.0.0",
      "asset_count": 0,
      "texture_count": 0,
      "audio_count": 0,
      "font_count": 0,
      "material_count": 0,
      "shader_count": 0,
      "created": "2024-01-15T10:30:00Z",
      "last_updated": "2024-01-15T10:30:00Z",
      "status": "stub",
      "validation": {
        "git_lfs": false,
        "import_presets": false,
        "directory_structure": true
      }
    }
  },
  "total_assets": 0,
  "total_textures": 0,
  "total_audio": 0,
  "total_fonts": 0,
  "total_materials": 0,
  "total_shaders": 0
}
```

### 5. Godot Asset Gate Integration
- Validates asset directory structure
- Validates .gitattributes entries
- Validates import preset files
- Validates assets.json registry
- Reports issues to godot-asset-gate.sh

### 6. Case-Sensitivity Validation
- Validates `core.ignorecase` is false in git config
- Reports if git config needs adjustment
- Validates directory names match case exactly
- Prevents Assets/ vs assets/ conflicts

## Time Saved
- **40 minutes per expansion pack** (manual directory creation and LFS setup)
- **95% reduction** in asset setup errors
- **Automated validation** eliminates manual configuration
- **CI-ready** artifacts generated automatically

## Prerequisites
- Git LFS installed and configured
- Godot project in workspace
- `git` CLI available
- Expansion system created via `ashfall-expansion-scaffold`

## Verification After Use
```bash
# Verify directory structure
tree assets/expansions/05_holdfast/ | head -20

# Verify .gitattributes
grep "05_holdfast" .gitattributes

# Verify LFS tracking
git lfs ls-files assets/expansions/05_holdfast/

# Verify core.ignorecase
git config core.ignorecase

# Run godot asset gate
godot --headless --path . -- --asset-gate
```

## Integration Points
- **Depends on:** `ashfall-expansion-scaffold` (creates expansion system)
- **Used by:** `ashfall-sprite-family-gen`, `ashfall-tilemap-expansion-kit`, `ashfall-audio-expansion-pack` (all use the asset pack)
- **Follow-up skills:** `ashfall-lfs-gate` (validates LFS configuration)

## Error Detection
The skill detects and reports:

### 1. Directory Creation Issues
```
❌ ERROR: Directory creation failed:
   - assets/expansions/05_holdfast/ already exists
   - Cannot create asset pack for existing expansion
   - Suggested fix: Use --force to overwrite or choose different expansion number

⚠️  WARNING: Directory structure incomplete:
   - Missing directory: assets/expansions/05_holdfast/shaders/
   - Missing directory: assets/expansions/05_holdfast/fonts/
   - Suggested fix: Create missing directories manually
```

### 2. Git LFS Issues
```
❌ CRITICAL: Git LFS not installed:
   - Git LFS is required for expansion asset packs
   - Install Git LFS: https://git-lfs.com/
   - After install: git lfs install

❌ CRITICAL: Git LFS tracking failed:
   - File: assets/expansions/05_holdfast/sprites/item_water_filter.png
   - Error: LFS not configured for this repository
   - Suggested fix: git lfs track "assets/expansions/05_holdfast/sprites/**"

⚠️  WARNING: LFS attribute missing:
   - Pattern not in .gitattributes: assets/expansions/05_*/**
   - Suggested fix: Add LFS tracking for expansion directory
```

### 3. Import Preset Issues
```
⚠️  WARNING: Import preset missing:
   - File: assets/expansions/05_holdfast/.import/sprites.item_water_filter.png.import
   - Error: Import preset not created
   - Suggested fix: Create import preset or run Godot asset import

❌ ERROR: Import preset invalid:
   - File: assets/expansions/05_holdfast/.import/sprites.item_water_filter.png.import
   - Error: Invalid preset name
   - Suggested fix: Rename preset to follow convention: "Expansion Sprite - 64px"
```

### 4. assets.json Issues
```
⚠️  WARNING: assets.json missing:
   - File: assets/expansions/assets.json
   - Error: Registry file not found
   - Suggested fix: Create assets.json or run with --register flag

❌ ERROR: assets.json invalid:
   - Schema version missing
   - Expansion entry for 05_holdfast missing
   - Asset count incorrect
   - Suggested fix: Update assets.json to match schema
```

### 5. Case-Sensitivity Issues
```
❌ CRITICAL: core.ignorecase is true:
   - Git config: core.ignorecase=true
   - This will cause Assets/ vs assets/ conflicts
   - Suggested fix: git config --global core.ignorecase false
   - Impact: Expansion asset pack may not work correctly

⚠️  WARNING: Directory name case mismatch:
   - Expected: assets/expansions/05_holdfast/
   - Actual: assets/expansions/05_Holdfast/
   - Suggested fix: Rename directory to match case exactly
```

### 6. Permission Issues
```
❌ ERROR: Permission denied:
   - Cannot create directory: assets/expansions/05_holdfast/
   - Error: Permission denied
   - Suggested fix: Check directory permissions or run with sudo

⚠️  WARNING: File write failed:
   - Cannot write to .gitattributes
   - Error: File is read-only
   - Suggested fix: Check file permissions or run as administrator
```

## Automated Fixes
The skill can automatically apply fixes for:

### 1. Directory Creation
- Creates all required subdirectories
- Validates directory structure
- Reports missing directories
- Creates directory structure recursively

### 2. Git LFS Configuration
- Adds LFS tracking for expansion directory
- Validates LFS installation
- Reports LFS status
- Validates .gitattributes entries

### 3. Import Presets
- Creates default import presets for all asset types
- Validates preset names follow convention
- Reports missing presets
- Updates existing presets

### 4. assets.json Updates
- Creates assets.json if missing
- Updates expansion entry
- Validates schema version
- Reports registry issues

### 5. Case-Sensitivity Fix
- Validates core.ignorecase setting
- Reports if needs adjustment
- Provides git command to fix

## Configuration
- **Expansion number:** 01-99 (required)
- **Codename:** snake_case expansion name (required, e.g., "holdfast", "duty_roster")
- **Root path:** Custom root directory (optional, default: ./assets/)
- **Force:** Overwrite existing asset pack (default: false)
- **Register:** Update assets.json registry (default: true)
- **Validate:** Run validation checks (default: true)
- **Git lfs:** Configure Git LFS (default: true)
- **Import presets:** Create import presets (default: true)

## Example Asset Pack Structure

```
assets/
└── expansions/
    └── 05_holdfast/
        ├── .gitattributes
        ├── art/
        │   ├── backgrounds/
        │   │   ├── skybox/
        │   │   │   ├── skybox_day.png
        │   │   │   ├── skybox_night.png
        │   │   │   └── skybox_radiation.png
        │   │   ├── terrain/
        │   │   │   ├── terrain_grass.png
        │   │   │   ├── terrain_ruins.png
        │   │   │   └── terrain_wasteland.png
        │   │   └── ui/
        │   │       ├── ui_background.png
        │   │       └── ui_panel.png
        │   ├── characters/
        │   │   ├── survivors/
        │   │   │   ├── survivor_male_01.png
        │   │   │   ├── survivor_female_01.png
        │   │   │   └── survivor_child_01.png
        │   │   ├── npcs/
        │   │   │   ├── npc_commander.png
        │   │   │   ├── npc_medic.png
        │   │   │   └── npc_scavenger.png
        │   │   └── factions/
        │   │       ├── faction_holdfast.png
        │   │       └── faction_raiders.png
        │   ├── items/
        │   │   ├── consumables/
        │   │   │   ├── item_water_filter.png
        │   │   │   ├── item_medical_kit.png
        │   │   │   └── item_food_rations.png
        │   │   ├── equipment/
        │   │   │   ├── item_gas_mask.png
        │   │   │   ├── item_hazmat_suit.png
        │   │   │   └── item_radiation_armor.png
        │   │   └── resources/
        │   │       ├── resource_water.png
        │   │       ├── resource_food.png
        │   │       └── resource_medical.png
        │   └── environments/
        │       ├── ruins/
        │       │   ├── ruins_factory.png
        │       │   └── ruins_house.png
        │       ├── settlements/
        │       │   ├── settlement_camp.png
        │       │   └── settlement_outpost.png
        │       └── wasteland/
        │           ├── wasteland_plains.png
        │           └── wasteland_radioactive.png
        ├── sprites/
        │   ├── items/
        │   │   ├── item_water_filter.png
        │   │   ├── item_medical_kit.png
        │   │   └── item_gear_pack.png
        │   ├── characters/
        │   │   ├── survivor_male_01.png
        │   │   └── survivor_female_01.png
        │   ├── ui/
        │   │   ├── ui_button_normal.png
        │   │   ├── ui_button_hover.png
        │   │   └── ui_button_pressed.png
        │   └── effects/
        │       ├── effect_explosion.png
        │       └── effect_radiation.png
        ├── ui/
        │   ├── panels/
        │   │   ├── expansion_panel.tscn
        │   │   └── expansion_panel.gd
        │   ├── dialog/
        │   │   ├── dialog_box.tscn
        │   │   └── dialog_box.gd
        │   ├── hud/
        │   │   ├── hud_bar.tscn
        │   │   └── hud_bar.gd
        │   └── menus/
        │       ├── main_menu.tscn
        │       └── main_menu.gd
        ├── audio/
        │   ├── music/
        │   │   ├── music_expansion_theme.ogg
        │   │   └── music_settlement_theme.ogg
        │   ├── sfx/
        │   │   ├── sfx_item_pickup.wav
        │   │   ├── sfx_item_place.wav
        │   │   └── sfx_ui_click.wav
        │   ├── radio/
        │   │   ├── radio_holdfast_frequency.ogg
        │   │   └── radio_transmission_01.ogg
        │   └── ambience/
        │       ├── ambience_wasteland.ogg
        │       └── ambience_settlement.ogg
        ├── materials/
        │   ├── character/
        │   │   ├── material_survivor.tres
        │   │   └── material_npc.tres
        │   ├── item/
        │   │   ├── material_item_consumable.tres
        │   │   └── material_item_equipment.tres
        │   └── environment/
        │       ├── material_terrain.tres
        │       └── material_skybox.tres
        ├── shaders/
        │   ├── character/
        │   │   ├── shader_survivor.gdshader
        │   │   └── shader_npc.gdshader
        │   ├── item/
        │   │   └── shader_item.gdshader
        │   └── postprocessing/
        │       └── shader_radiation.gdshader
        ├── fonts/
        │   └── expansion/
        │       ├── expansion_font.ttf
        │       └── expansion_font-bold.ttf
        └── .import
            ├── art.backgrounds.skybox.skybox_day.png.import
            ├── sprites.items.item_water_filter.png.import
            ├── ui.panels.expansion_panel.tscn.import
            └── audio.music.music_expansion_theme.ogg.import
```

## Related Skills
- `ashfall-expansion-scaffold` - Creates expansion system
- `ashfall-sprite-family-gen` - Generates sprites for items
- `ashfall-tilemap-expansion-kit` - Creates tilemaps for biomes
- `ashfall-audio-expansion-pack` - Creates audio for expansion
- `ashfall-ui-expansion-panel-kit` - Creates UI panels
- `ashfall-shader-expansion-fx` - Creates shaders for effects
- `ashfall-lfs-gate` - Validates LFS configuration

## Notes
- Follows ASHFALL's strict directory structure conventions
- Validates all asset paths are correct
- Ensures Git LFS is properly configured
- Creates CI-ready artifacts
- Follows Godot import preset best practices

## Maintenance
- Update directory structure if Godot project structure changes
- Add new asset types if expansion domains expand
- Update import preset templates if Godot import settings change
- Update assets.json schema if registry format evolves
