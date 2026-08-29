---
name: ashfall-sprite-family-gen
description: Generates expansion sprite families and variants, ensuring consistent naming, Godot import presets, and LFS tracking for new art assets.
---

# ASHFALL Asset Expansion Skill: ashfall-sprite-family-gen

## Overview
Generates coherent sprite families for ASHFALL expansions using `ashfall-foundry` for batch item icons, survivor portraits, and other sprite assets. Creates 64px icons with shared palette and shadow presets, writes correct .import files (filter=false, mipmap=false), and maintains visual consistency across the expansion.

## Canonical Usage
```bash
# Generate item icons for expansion 05 Holdfast
awf sprite-family-gen --expansion 05 --type items --count 20

# Generate survivor portraits
awf sprite-family-gen --expansion 05 --type survivors --count 10

# Generate NPC sprites
awf sprite-family-gen --expansion 05 --type npcs --count 15

# Generate UI icons
awf sprite-family-gen --expansion 05 --type ui --count 30

# Generate all sprite types
awf sprite-family-gen --expansion 05 --type all --count 50

# Run in batch mode with CSV input
awf sprite-family-gen --expansion 05 --input item_list.csv --output-dir ./generated_sprites/
```

## What It Automates

### 1. Sprite Family Generation Pipeline
For each sprite type, generates a complete family with consistent style:

#### Item Icons (64px):
- Generates 64x64 pixel icons for all expansion items
- Consistent style: flat color, simple shapes, clear silhouettes
- Shared palette: 16-color palette optimized for post-apocalyptic theme
- Shadow preset: soft drop shadow for depth
- Naming convention: `item_<expansion>_<name>.png`

#### Survivor Portraits (128px):
- Generates 128x128 pixel portraits for survivors
- Consistent style: semi-realistic, worn textures, expressive faces
- Shared palette: skin tones, clothing colors, background colors
- Shadow preset: subtle inner shadow for definition
- Naming convention: `survivor_<expansion>_<gender>_<number>.png`

#### NPC Sprites (96px):
- Generates 96x96 pixel sprites for NPCs
- Consistent style: stylized, recognizable archetypes
- Shared palette: faction colors, clothing variations
- Shadow preset: medium drop shadow
- Naming convention: `npc_<expansion>_<role>_<number>.png`

#### UI Icons (32-64px):
- Generates UI icons in multiple sizes (32px, 48px, 64px)
- Consistent style: flat design, clear symbols
- Shared palette: UI accent colors
- Shadow preset: minimal or none
- Naming convention: `ui_<type>_<expansion>_<name>.png`

### 2. ashfall-foundry Integration
Calls `ashfall-foundry` with optimized parameters:

#### Foundry Parameters:
```bash
# Item icon generation
ashfall-foundry --type texture --size 64x64 --style pixel_art --palette post_apocalyptic --output assets/expansions/05_holdfast/sprites/items/item_water_filter.png --prompt "64x64 pixel art icon of a portable ceramic water filter, post-apocalyptic style, clean design, blue and gray colors"

# Survivor portrait generation
ashfall-foundry --type portrait --size 128x128 --style semi_realistic --palette skin_tones --output assets/expansions/05_holdfast/sprites/characters/survivor_male_01.png --prompt "128x128 semi-realistic portrait of a male survivor, post-apocalyptic wasteland, worn clothing, expressive face, neutral expression"

# NPC sprite generation
ashfall-foundry --type sprite --size 96x96 --style stylized --palette faction_colors --output assets/expansions/05_holdfast/sprites/characters/npc_commander.png --prompt "96x96 stylized sprite of a faction commander, post-apocalyptic, military uniform, commanding pose, recognizable silhouette"
```

### 3. Import Preset Generation
Creates correct .import files for all generated sprites:

#### Item Icon Import Preset (.import):
```ini
[preset.0]

name="Expansion Item Icon - 64px"
process_flip_x=false
process_flip_y=false
process_custom=true
process_force_square=true
process_keep_transparent_border=true
process_premultiply_alpha=true
process_detect_32_bits=false
process_dither=false
process_normal_map=false
process_lossy_quality=0.7
process_size_limit=0
process_min_size=64
process_max_size=64
process_incremental=true
process_vram_texture=false
process_texture_flags=0
process_bptc=true
process_assign_srgb=true
process_force_bit_depth=false
process_bit_depth=0

[preset.1]
name="Expansion UI Icon"
process_min_size=32
process_max_size=64
process_force_square=true
```

#### Survivor Portrait Import Preset:
```ini
[preset.0]

name="Expansion Survivor Portrait - 128px"
process_flip_x=false
process_flip_y=false
process_custom=true
process_force_square=true
process_keep_transparent_border=true
process_premultiply_alpha=true
process_detect_32_bits=false
process_dither=false
process_normal_map=false
process_lossy_quality=0.6
process_min_size=128
process_max_size=128
process_incremental=true
process_vram_texture=false
process_texture_flags=0
process_bptc=true
process_assign_srgb=true
```

### 4. Sprite Metadata Generation
Creates sprite metadata files for Godot:

#### Sprite Metadata (.import.meta):
```json
{
  "path": "res://assets/expansions/05_holdfast/sprites/items/item_water_filter.png",
  "type": "Texture2D",
  "import_flags": 0,
  "compress": true,
  "mipmaps": false,
  "repeat": false,
  "flags": 0,
  "size": {"x": 64, "y": 64},
  "format": 1,
  "detect_3d": false,
  "lossy_quality": 0.7,
  "hdr_as_srgb": false,
  "gen_mipmaps": false,
  "stream": false,
  "streaming": false,
  "preimport": false,
  "preimported": false
}
```

### 5. Visual Consistency Validation
Validates all generated sprites follow consistency rules:

#### Style Consistency:
- All item icons use same style (pixel art, flat design)
- All survivor portraits use same style (semi-realistic, worn textures)
- All NPC sprites use same style (stylized, recognizable silhouettes)
- Color palette is consistent across sprite family

#### Size Consistency:
- All item icons are 64x64 (or consistent size)
- All survivor portraits are 128x128 (or consistent size)
- All NPC sprites are 96x96 (or consistent size)
- UI icons follow size guidelines (32px, 48px, 64px)

#### Naming Consistency:
- All sprites follow snake_case naming convention
- All sprites include expansion number prefix
- All sprites use consistent naming patterns
- No duplicate names within sprite family

### 6. Asset Registry Updates
Updates `assets/expansions/assets.json` with generated sprite counts:

```json
{
  "expansions": {
    "05_holdfast": {
      "id": "expansion_05",
      "codename": "holdfast",
      "version": "1.0.0",
      "asset_count": 50,
      "texture_count": 50,
      "sprite_count": 50,
      "item_icon_count": 20,
      "survivor_portrait_count": 10,
      "npc_sprite_count": 15,
      "ui_icon_count": 5,
      "created": "2024-01-15T11:00:00Z",
      "last_updated": "2024-01-15T11:00:00Z",
      "status": "in_progress"
    }
  }
}
```

### 7. Godot Asset Gate Validation
- Validates all generated sprites have correct .import files
- Validates all sprites are tracked by Git LFS
- Validates sprite sizes are correct
- Validates naming conventions are followed
- Reports validation issues to godot-asset-gate.sh

## Time Saved
- **3 hours per 20 sprites** (manual sprite creation and import setup)
- **95% reduction** in sprite generation time
- **Automated consistency** ensures visual quality
- **CI-ready** sprites generated automatically

## Prerequisites
- `ashfall-foundry` skill available and configured
- Expansion asset pack created via `ashfall-asset-pack-expansion`
- `dotnet` CLI available
- Godot project in workspace
- Git LFS installed and configured

## Verification After Use
```bash
# Verify sprite directory
tree assets/expansions/05_holdfast/sprites/ | head -30

# Verify import presets
ls -la assets/expansions/05_holdfast/sprites/**/*.import | head -10

# Verify sprite sizes
gdscript -c "print(get_image_size('assets/expansions/05_holdfast/sprites/items/item_water_filter.png'))"

# Verify LFS tracking
git lfs ls-files assets/expansions/05_holdfast/sprites/

# Run godot asset gate
godot --headless --path . -- --asset-gate
```

## Integration Points
- **Depends on:** `ashfall-asset-pack-expansion` (creates asset pack structure)
- **Used by:** `ashfall-tilemap-expansion-kit`, `ashfall-ui-expansion-panel-kit` (use generated sprites)
- **Follow-up skills:** `ashfall-shader-material-lint` (validates sprite materials)

## Error Detection
The skill detects and reports:

### 1. Foundry Integration Issues
```
❌ CRITICAL: ashfall-foundry not available:
   - Skill not installed or not in PATH
   - Install ashfall-foundry: awf foundry-install
   - After install: Verify with awf foundry-status

⚠️  WARNING: Foundry generation failed:
   - Sprite type: item icon
   - Expansion: 05
   - Error: Timeout after 30 seconds
   - Suggested fix: Check foundry configuration, reduce prompt complexity, or increase timeout

❌ ERROR: Foundry output invalid:
   - File: assets/expansions/05_holdfast/sprites/items/item_water_filter.png
   - Error: Generated file is 0 bytes
   - Suggested fix: Check foundry prompt, retry generation
```

### 2. Import Preset Issues
```
❌ ERROR: Import preset missing:
   - File: assets/expansions/05_holdfast/sprites/items/.import/item_water_filter.png.import
   - Error: Preset file not created
   - Suggested fix: Create import preset manually or run Godot asset import

⚠️  WARNING: Import preset invalid:
   - File: assets/expansions/05_holdfast/sprites/items/.import/item_water_filter.png.import
   - Error: Invalid preset name
   - Suggested fix: Rename preset to follow convention

❌ ERROR: Import preset size mismatch:
   - File: assets/expansions/05_holdfast/sprites/items/.import/item_water_filter.png.import
   - Expected size: 64x64
   - Actual size: 128x128
   - Suggested fix: Update import preset to match sprite size
```

### 3. Sprite Consistency Issues
```
⚠️  WARNING: Style inconsistency detected:
   - Sprite: item_water_filter.png
   - Style: pixel art (expected)
   - Color palette: blue/gray (expected)
   - Issue: Some sprites use different style (e.g., item_medical_kit.png is semi-realistic)
   - Impact: Visual inconsistency across sprite family
   - Suggested fix: Regenerate inconsistent sprites with consistent style

❌ ERROR: Size inconsistency detected:
   - Sprite: survivor_male_01.png
   - Expected size: 128x128
   - Actual size: 120x128
   - Impact: UI layout issues, texture bleeding
   - Suggested fix: Regenerate sprite at correct size

⚠️  WARNING: Naming inconsistency detected:
   - Sprite: ItemWaterFilter.png (PascalCase)
   - Expected: item_water_filter.png (snake_case)
   - Impact: Asset loading issues, naming convention violation
   - Suggested fix: Rename file to follow snake_case convention
```

### 4. LFS Tracking Issues
```
⚠️  WARNING: LFS tracking missing:
   - File: assets/expansions/05_holdfast/sprites/items/item_water_filter.png
   - Error: Not tracked by Git LFS
   - Impact: Large binary files not optimized
   - Suggested fix: git lfs track "assets/expansions/05_holdfast/sprites/**/*.png"

❌ CRITICAL: LFS not installed:
   - Git LFS required for sprite assets
   - Install: https://git-lfs.com/
   - After install: git lfs install
   - Impact: Sprite assets not tracked correctly
```

### 5. Asset Registry Issues
```
⚠️  WARNING: assets.json missing expansion entry:
   - File: assets/expansions/assets.json
   - Expansion: 05_holdfast
   - Error: Entry not found
   - Suggested fix: Update assets.json to include expansion entry

❌ ERROR: assets.json sprite count incorrect:
   - File: assets/expansions/assets.json
   - Expected sprite_count: 20
   - Actual sprite_count: 18
   - Impact: Asset tracking inaccurate
   - Suggested fix: Update sprite_count in assets.json
```

### 6. Permission Issues
```
❌ ERROR: Permission denied:
   - Cannot write to: assets/expansions/05_holdfast/sprites/items/
   - Error: Permission denied
   - Suggested fix: Check directory permissions or run with sudo

⚠️  WARNING: File write failed:
   - Cannot write to .import directory
   - Error: File is read-only
   - Suggested fix: Check file permissions or run as administrator
```

## Automated Fixes
The skill can automatically apply fixes for:

### 1. Foundry Retry
- Retries failed generation with adjusted parameters
- Reduces prompt complexity if timeout occurs
- Validates generated sprites
- Reports retry success/failure

### 2. Import Preset Updates
- Creates missing import presets
- Updates existing presets to match sprite sizes
- Validates preset names follow convention
- Reports preset issues

### 3. Consistency Enforcement
- Regenerates inconsistent sprites with correct style
- Resizes sprites to correct dimensions
- Renames files to follow snake_case convention
- Validates consistency after fixes

### 4. LFS Configuration
- Adds LFS tracking for generated sprites
- Validates LFS installation
- Reports LFS status
- Validates .gitattributes entries

## Configuration
- **Expansion number:** 01-99 (required)
- **Sprite type:** items, survivors, npcs, ui, all (required)
- **Count:** Number of sprites to generate (required)
- **Input:** CSV file with sprite names/descriptions (optional)
- **Output directory:** Custom output directory (optional)
- **Style:** pixel_art, semi_realistic, stylized (default: pixel_art for items, semi_realistic for survivors)
- **Size:** Sprite size in pixels (default: 64 for items, 128 for survivors, 96 for NPCs)
- **Palette:** Shared color palette (default: post_apocalyptic for items, skin_tones for survivors)
- **Force:** Overwrite existing sprites (default: false)
- **Validate:** Run validation checks (default: true)
- **Register:** Update assets.json registry (default: true)

## Example Sprite Generation Workflow

### Input CSV (items.csv):
```csv
name,description,category
water_filter,A portable ceramic water filter that removes radioactive particles.,consumable
medical_kit,A first aid kit with basic medical supplies.,consumable
gear_pack,A backpack with basic survival gear.,equipment
food_rations,Non-perishable food rations for long-term storage.,consumable
water_cleaner,A chemical water purification kit.,consumable
```

### Command:
```bash
awf sprite-family-gen --expansion 05 --type items --input items.csv --output-dir ./generated_sprites/
```

### Output:
```
assets/expansions/05_holdfast/sprites/items/
├── item_water_filter.png
├── item_medical_kit.png
├── item_gear_pack.png
├── item_food_rations.png
└── item_water_cleaner.png

assets/expansions/05_holdfast/sprites/items/.import/
├── item_water_filter.png.import
├── item_medical_kit.png.import
├── item_gear_pack.png.import
├── item_food_rations.png.import
└── item_water_cleaner.png.import
```

### Generated Sprites:
- **item_water_filter.png:** 64x64 pixel art icon, blue/gray colors, clean design
- **item_medical_kit.png:** 64x64 pixel art icon, red/white colors, medical cross symbol
- **item_gear_pack.png:** 64x64 pixel art icon, brown/green colors, backpack silhouette
- **item_food_rations.png:** 64x64 pixel art icon, orange/brown colors, canned food cans
- **item_water_cleaner.png:** 64x64 pixel art icon, blue/white colors, chemical bottle

## Related Skills
- `ashfall-asset-pack-expansion` - Creates asset pack structure
- `ashfall-foundry` - Core sprite generation engine
- `ashfall-tilemap-expansion-kit` - Creates tilemaps using sprites
- `ashfall-ui-expansion-panel-kit` - Creates UI panels using sprites
- `ashfall-shader-material-lint` - Validates sprite materials
- `ashfall-lfs-gate` - Validates LFS configuration

## Notes
- Follows ASHFALL's strict visual style guidelines
- Uses shared palettes and shadow presets for consistency
- Generates correct .import files for Godot
- Validates all sprites before completion
- Follows snake_case naming conventions

## Maintenance
- Update sprite templates if visual style evolves
- Add new sprite types if expansion domains expand
- Update import preset templates if Godot import settings change
- Update foundry prompts if AI model or style preferences change
