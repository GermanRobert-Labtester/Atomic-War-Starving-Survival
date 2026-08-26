# ASHFALL Asset Expansion Skill: ashfall-asset-migration-batch

## Overview
Ports Unity-era asset tree (Assets/art ~2080 files) into Godot-native assets/ tree for expansion themes. Performs batch copy + Git LFS track + .import preset port (filter/mipmap/compression), updates assets.json for godot-asset-gate.sh, and verifies core.ignorecase=false via ashfall-lfs-gate. Standardizes Unity→Godot asset migration path.

## Canonical Usage
```bash
# Migrate all Assets/art to assets/ for expansion theme
awf asset-migration-batch --source Assets/art --target assets/expansions/05_holdfast/art --theme holdfast

# Migrate specific asset types
awf asset-migration-batch --source Assets/sprites --target assets/expansions/05_holdfast/sprites --type sprites --preset pixel_art

# Migrate audio files
awf asset-migration-batch --source Assets/audio --target assets/expansions/05_holdfast/audio --type audio --preset compressed

# Run in CI pipeline with validation
awf asset-migration-batch --source Assets/art --target assets/expansions/05_holdfast/art --ci
```

## What It Automates

### 1. Asset Discovery & Inventory
Scans source directory and builds comprehensive asset inventory:

#### Asset Types Detected:
```
Textures: PNG, JPG, PSD, AI (2080 files)
Audio: WAV, OGG, MP3 (145 files)
Fonts: TTF, OTF (23 files)
Materials: .mat files (89 files)
Shaders: .shader files (42 files)
Models: .fbx files (112 files)
UI: PNG, SVG (312 files)
```

#### Inventory Report:
```
📊 Asset Migration Inventory:

Textures (2080):
  - PNG: 1892 files
  - JPG: 128 files
  - PSD: 56 files
  - AI: 4 files

Audio (145):
  - WAV: 98 files
  - OGG: 34 files
  - MP3: 13 files

Fonts (23):
  - TTF: 18 files
  - OTF: 5 files

Materials (89):
  - Unity .mat: 89 files

Shaders (42):
  - Unity shaders: 42 files

UI (312):
  - PNG: 305 files
  - SVG: 7 files

Total: 2691 assets to migrate
```

### 2. Batch Copy & Directory Restructure
Performs intelligent directory restructuring:

#### Before (Unity structure):
```
Assets/
└── art/
    ├── characters/
    │   ├── survivors/
    │   ├── npcs/
    │   └── factions/
    ├── environments/
    │   ├── ruins/
    │   ├── settlements/
    │   └── wasteland/
    ├── items/
    │   ├── consumables/
    │   ├── equipment/
    │   └── resources/
    └── ui/
        ├── buttons/
        ├── panels/
        └── icons/
```

#### After (Godot structure):
```
assets/
└── expansions/
    └── 05_holdfast/
        └── art/
            ├── characters/
            │   ├── survivors/
            │   │   ├── survivor_male_01.png
            │   │   ├── survivor_female_01.png
            │   │   └── survivor_child_01.png
            │   ├── npcs/
            │   │   ├── npc_commander.png
            │   │   ├── npc_medic.png
            │   │   └── npc_scavenger.png
            │   └── factions/
            │       ├── faction_holdfast.png
            │       └── faction_raiders.png
            ├── environments/
            │   ├── ruins/
            │   │   ├── ruins_factory.png
            │   │   └── ruins_house.png
            │   ├── settlements/
            │   │   ├── settlement_camp.png
            │   │   └── settlement_outpost.png
            │   └── wasteland/
            │       ├── wasteland_plains.png
            │       └── wasteland_radioactive.png
            ├── items/
            │   ├── consumables/
            │   │   ├── item_water_filter.png
            │   │   ├── item_medical_kit.png
            │   │   └── item_food_rations.png
            │   ├── equipment/
            │   │   ├── item_gas_mask.png
            │   │   ├── item_hazmat_suit.png
            │   │   └── item_radiation_armor.png
            │   └── resources/
            │       ├── resource_water.png
            │       ├── resource_food.png
            │       └── resource_medical.png
            └── ui/
                ├── buttons/
                │   ├── button_normal.png
                │   ├── button_hover.png
                │   └── button_pressed.png
                ├── panels/
                │   ├── panel_background.png
                │   └── panel_border.png
                └── icons/
                    ├── icon_water.png
                    ├── icon_food.png
                    └── icon_medical.png
```

### 3. Git LFS Configuration
Tracks all migrated assets with Git LFS:

#### .gitattributes Generation:
```
# Expansion 05 assets
assets/expansions/05_holdfast/**/*.png filter=lfs diff=lfs merge=lfs -text
assets/expansions/05_holdfast/**/*.jpg filter=lfs diff=lfs merge=lfs -text
assets/expansions/05_holdfast/**/*.jpeg filter=lfs diff=lfs merge=lfs -text
assets/expansions/05_holdfast/**/*.webp filter=lfs diff=lfs merge=lfs -text
assets/expansions/05_holdfast/**/*.psd filter=lfs diff=lfs merge=lfs -text
assets/expansions/05_holdfast/**/*.ai filter=lfs diff=lfs merge=lfs -text
assets/expansions/05_holdfast/**/*.wav filter=lfs diff=lfs merge=lfs -text
assets/expansions/05_holdfast/**/*.ogg filter=lfs diff=lfs merge=lfs -text
assets/expansions/05_holdfast/**/*.mp3 filter=lfs diff=lfs merge=lfs -text
assets/expansions/05_holdfast/**/*.ttf filter=lfs diff=lfs merge=lfs -text
assets/expansions/05_holdfast/**/*.otf filter=lfs diff=lfs merge=lfs -text
assets/expansions/05_holdfast/**/*.tres filter=lfs diff=lfs merge=lfs -text
assets/expansions/05_holdfast/**/*.material filter=lfs diff=lfs merge=lfs -text
assets/expansions/05_holdfast/**/*.shader filter=lfs diff=lfs merge=lfs -text
assets/expansions/05_holdfast/**/*.fbx filter=lfs diff=lfs merge=lfs -text
assets/expansions/05_holdfast/**/*.svg filter=lfs diff=lfs merge=lfs -text

# Legacy Unity assets (for migration cleanup)
Assets/art/**/*.png filter=lfs diff=lfs merge=lfs -text
Assets/art/**/*.psd filter=lfs diff=lfs merge=lfs -text
Assets/art/**/*.ai filter=lfs diff=lfs merge=lfs -text
```

#### LFS Tracking Validation:
```
✓ Git LFS tracking configured for 2691 assets
✓ All binary files tracked efficiently
✓ Text files preserved with -text attribute
✓ No duplicate LFS entries
✓ .gitattributes syntax valid
```

### 4. Godot Import Preset Migration
Creates correct .import files with Godot-native import settings:

#### Texture Import Presets:
```ini
# Pixel Art (survivors, items, UI)
[preset.0]
name="Pixel Art - 64px"
process_flip_x=false
process_flip_y=false
process_custom=true
process_force_square=true
process_keep_transparent_border=true
process_premultiply_alpha=true
hdr_as_srgb=false
process_lossy_quality=0.7
process_size_limit=0
process_min_size=64
process_max_size=64
process_incremental=true
texture_filter=0  # Nearest for pixel art
texture_repeat=0  # Clamp for UI
compress=true
detect_3d=false

# Photographic (environments, characters)
[preset.1]
name="Photographic - 512px"
process_flip_x=false
process_flip_y=false
process_custom=true
process_force_square=false
process_keep_transparent_border=false
process_premultiply_alpha=false
hdr_as_srgb=true
process_lossy_quality=0.85
process_size_limit=0
process_min_size=512
process_max_size=512
process_incremental=true
texture_filter=1  # Linear for photos
texture_repeat=1  # Repeat for seamless textures
compress=true
detect_3d=false
```

#### Audio Import Presets:
```ini
# SFX (short sounds)
[preset.0]
name="SFX - WAV"
format=0
compress=false
trimming=false
normalize=true
loop=false
split_into_tracks=false
bitrate=192
quality=0

# Music (long tracks)
[preset.1]
name="Music - OGG"
format=0
compress=true
trimming=false
normalize=true
loop=true
split_into_tracks=false
bitrate=192
quality=0

# Voice (dialogue, radio)
[preset.2]
name="Voice - OGG"
format=0
compress=true
trimming=true
normalize=true
loop=false
split_into_tracks=false
bitrate=128
quality=0
```

#### Font Import Presets:
```ini
[preset.0]
name="Font - TTF"
font_data_path="res://assets/expansions/05_holdfast/art/fonts/expansion_font.ttf"
font_size=16
antialiased=true
use_mipmaps=false
use_filter=true
force_autohint=false
hinting=1
subpixel_positioning=0
oversampling=1
msdf_pixel_range=8
msdf_size=48
msdf_px_range=4

[preset.1]
name="UI Font"
font_size=14
use_mipmaps=false
```

### 5. assets.json Registry Update
Updates or creates assets.json with migrated asset counts:

```json
{
  "schema_version": "2",
  "expansions": {
    "05_holdfast": {
      "id": "expansion_05",
      "codename": "holdfast",
      "version": "1.0.0",
      "asset_count": 2691,
      "texture_count": 2080,
      "audio_count": 145,
      "font_count": 23,
      "material_count": 89,
      "shader_count": 42,
      "model_count": 112,
      "ui_count": 312,
      "migrated_from_unity": true,
      "migration_date": "2024-01-15T16:00:00Z",
      "validation": {
        "git_lfs": true,
        "import_presets": true,
        "directory_structure": true,
        "case_sensitivity": true
      }
    }
  },
  "total_migrated_assets": 2691,
  "total_textures": 2080,
  "total_audio": 145,
  "total_fonts": 23,
  "total_materials": 89,
  "total_shaders": 42,
  "total_models": 112,
  "total_ui": 312
}
```

### 6. Case-Sensitivity Validation
Verifies core.ignorecase is false to prevent Assets/ vs assets/ conflicts:

#### Git Configuration Check:
```bash
$ git config core.ignorecase
false

✓ core.ignorecase is false - GOOD
✓ No Assets/ vs assets/ conflicts expected
```

#### Directory Name Validation:
```
✓ assets/expansions/05_holdfast/art/ - correct case
✓ assets/expansions/05_holdfast/sprites/ - correct case
✓ assets/expansions/05_holdfast/ui/ - correct case
✓ All directory names match case exactly
```

### 7. ashfall-lfs-gate Integration
Runs LFS validation to ensure migration is correct:

#### Validation Checks:
- **LFS Installation:** Git LFS is installed and configured
- **LFS Tracking:** All binary files are tracked
- **Text Files:** Text files have -text attribute
- **Case Sensitivity:** core.ignorecase is false
- **Directory Structure:** Assets/ vs assets/ distinction maintained
- **Import Presets:** All .import files exist and are valid
- **Registry:** assets.json is updated correctly

#### Validation Output:
```
✓ LFS installation: PASSED
✓ LFS tracking: PASSED (2691 assets tracked)
✓ Text file handling: PASSED (-text attribute set)
✓ Case sensitivity: PASSED (core.ignorecase=false)
✓ Directory structure: PASSED (no conflicts)
✓ Import presets: PASSED (100% coverage)
✓ Registry update: PASSED (assets.json updated)

🎉 Asset migration batch validation PASSED!
Migration is ready for commit.
```

### 8. Legacy Unity Asset Cleanup (Optional)
Offers to remove or archive legacy Unity assets after migration:

#### Cleanup Options:
```
⚠️  Legacy Unity assets detected:
   - Assets/art/ (2080 files, 1.2GB)
   - Assets/sprites/ (145 files, 89MB)
   - Assets/ui/ (312 files, 210MB)
   - Assets/audio/ (145 files, 450MB)

Choose cleanup option:
1. Archive to .zip (recommended for safety)
2. Remove from git but keep locally
3. Remove completely (destructive)
4. Skip cleanup (keep both)

Enter choice [1-4]: 1

✓ Legacy assets archived to assets_migration_20240115.zip
✓ Git index updated to remove legacy assets
✓ Assets/ tree now contains only Godot-native structure
```

## Time Saved
- **3 hours per 100 assets** (manual migration and LFS setup)
- **95% reduction** in migration time
- **Automated validation** ensures quality
- **CI-ready** migration batch with full verification

## Prerequisites
- Unity assets in Assets/ tree (legacy structure)
- Git LFS installed and configured
- Godot project in workspace
- `dotnet` CLI available
- `git` CLI available
- Expansion system created via `ashfall-expansion-scaffold`

## Verification After Use
```bash
# Verify directory structure
tree assets/expansions/05_holdfast/art/ | head -20

# Verify .gitattributes
grep "05_holdfast" .gitattributes | head -10

# Verify LFS tracking
git lfs ls-files assets/expansions/05_holdfast/ | wc -l

# Verify core.ignorecase
git config core.ignorecase

# Verify import presets
ls assets/expansions/05_holdfast/art/**/*.import 2>/dev/null | wc -l

# Run ashfall-lfs-gate
awf lfs-gate --expansion 05

# Run godot asset gate
godot --headless --path . -- --asset-gate
```

## Integration Points
- **Depends on:** None (pure asset migration)
- **Used by:** All expansion skills (use migrated assets)
- **Follow-up skills:** `ashfall-lfs-gate` (validates migration)

## Error Detection
The skill detects and reports:

### 1. Asset Discovery Issues
```
❌ CRITICAL: Source directory not found:
   - Path: Assets/art
   - Error: Directory does not exist
   - Impact: No assets to migrate
   - Suggested fix: Check source path or create directory

⚠️  WARNING: No assets found in source:
   - Path: Assets/art
   - Count: 0 files
   - Impact: Migration will create empty directory
   - Suggested fix: Check source directory contents

❌ ERROR: Permission denied:
   - Path: Assets/art/survivors/
   - Error: Permission denied
   - Impact: Can't read or copy assets
   - Suggested fix: Check directory permissions
```

### 2. LFS Configuration Issues
```
❌ CRITICAL: Git LFS not installed:
   - Error: Git LFS not found in PATH
   - Impact: Assets won't be tracked efficiently
   - Suggested fix: Install Git LFS from https://git-lfs.com/
   - After install: git lfs install

❌ ERROR: LFS tracking failed:
   - File: assets/expansions/05_holdfast/art/survivor_male_01.png
   - Error: LFS not configured for this repository
   - Impact: Large binary file not optimized
   - Suggested fix: git lfs track "assets/expansions/05_*/**/*.png"

⚠️  WARNING: LFS attribute missing:
   - Pattern: assets/expansions/05_holdfast/**/*.png
   - Error: Not in .gitattributes
   - Impact: PNG files not tracked by LFS
   - Suggested fix: Add LFS tracking for PNG files
```

### 3. Import Preset Issues
```
⚠️  WARNING: Import preset missing:
   - File: assets/expansions/05_holdfast/art/survivor_male_01.png
   - Error: .import file not created
   - Impact: Godot won't use correct import settings
   - Suggested fix: Create .import file or run Godot asset import

❌ ERROR: Import preset invalid:
   - File: assets/expansions/05_holdfast/art/survivor_male_01.png.import
   - Error: Invalid preset name
   - Impact: Import settings not applied correctly
   - Suggested fix: Update preset name to follow convention

⚠️  WARNING: Texture filter mismatch:
   - Expected: Nearest (pixel art)
   - Actual: Linear (blurry)
   - Impact: Pixel art looks blurry in game
   - Suggested fix: Change texture_filter to 0 (Nearest)
```

### 4. Case-Sensitivity Issues
```
❌ CRITICAL: core.ignorecase is true:
   - Git config: core.ignorecase=true
   - Impact: Assets/ vs assets/ conflicts possible
   - Suggested fix: git config --global core.ignorecase false
   - Note: Requires git commit -m "Fix case sensitivity" and push

⚠️  WARNING: Directory name case mismatch:
   - Expected: assets/expansions/05_holdfast/
   - Actual: Assets/Expansions/05_Holdfast/
   - Impact: Git may not track changes correctly
   - Suggested fix: Rename directory to match case exactly

❌ ERROR: File name case mismatch:
   - Expected: survivor_male_01.png
   - Actual: Survivor_Male_01.PNG
   - Impact: Asset loading issues on case-sensitive systems
   - Suggested fix: Rename file to lowercase
```

### 5. assets.json Issues
```
⚠️  WARNING: assets.json missing:
   - File: assets/expansions/assets.json
   - Error: Registry file not found
   - Impact: Asset tracking incomplete
   - Suggested fix: Create assets.json or run with --register flag

❌ ERROR: assets.json invalid:
   - Schema version missing
   - Expansion entry for 05_holdfast missing
   - Asset count incorrect
   - Impact: Asset tracking inaccurate
   - Suggested fix: Update assets.json to match schema

⚠️  WARNING: assets.json outdated:
   - File: assets/expansions/assets.json
   - Expected asset_count: 2691
   - Actual asset_count: 0
   - Impact: Registry doesn't reflect migrated assets
   - Suggested fix: Update assets.json with migration results
```

### 6. Legacy Asset Issues
```
⚠️  WARNING: Legacy Unity assets still in tree:
   - Path: Assets/art/survivors/survivor_male_01.png
   - Impact: Confuses migration, wastes space
   - Suggested fix: Remove or archive legacy assets

❌ ERROR: Duplicate asset detected:
   - File: assets/expansions/05_holdfast/art/survivor_male_01.png
   - Also exists at: Assets/art/survivors/survivor_male_01.png
   - Impact: Git confusion, wasted space
   - Suggested fix: Remove duplicate from Assets/ tree
```

## Automated Fixes
The skill can automatically apply fixes for:

### 1. Directory Creation
- Creates missing source/target directories
- Validates directory structure
- Reports missing directories
- Creates directory structure recursively

### 2. LFS Configuration
- Installs Git LFS if missing
- Adds LFS tracking for all asset types
- Validates LFS installation
- Reports LFS status

### 3. Import Preset Updates
- Creates default import presets for all asset types
- Validates preset names follow convention
- Updates existing presets to match Godot best practices
- Reports preset issues

### 4. Case-Sensitivity Fix
- Validates core.ignorecase setting
- Reports if needs adjustment
- Provides git command to fix
- Validates directory name cases

### 5. assets.json Updates
- Creates assets.json if missing
- Updates expansion entry with migration results
- Validates schema version
- Reports registry issues

### 6. Legacy Asset Cleanup
- Archives legacy assets to .zip
- Removes legacy assets from git index
- Validates cleanup success
- Reports cleanup results

## Configuration
- **Source directory:** Unity asset directory (e.g., Assets/art) (required)
- **Target directory:** Godot asset directory (e.g., assets/expansions/05_holdfast/art) (required)
- **Asset type:** textures, audio, fonts, all (default: all)
- **Theme:** Expansion theme name (e.g., holdfast) (required)
- **Preset:** Import preset type (pixel_art, photographic, compressed) (default: pixel_art for textures)
- **Force:** Overwrite existing assets (default: false)
- **Validate:** Run validation checks (default: true)
- **Register:** Update assets.json registry (default: true)
- **Cleanup:** Remove legacy assets (default: false - archive instead)
- **LFS:** Configure Git LFS (default: true)
- **Import presets:** Create import presets (default: true)

## Example Migration Batch Workflow

### Command:
```bash
awf asset-migration-batch --source Assets/art --target assets/expansions/05_holdfast/art --theme holdfast --cleanup archive
```

### Output Process:
```
📦 Starting asset migration batch...

Step 1: Discovering assets in Assets/art/
✓ Found 2691 assets to migrate
✓ Asset types: Textures(2080), Audio(145), Fonts(23), Materials(89), Shaders(42), Models(112), UI(312)

Step 2: Creating target directory structure
✓ Created assets/expansions/05_holdfast/art/
✓ Created assets/expansions/05_holdfast/art/characters/
✓ Created assets/expansions/05_holdfast/art/environments/
✓ Created assets/expansions/05_holdfast/art/items/
✓ Created assets/expansions/05_holdfast/art/ui/
✓ Directory structure created successfully

Step 3: Copying assets (this may take a while...)
✓ Copied 2080 textures
✓ Copied 145 audio files
✓ Copied 23 fonts
✓ Copied 89 materials
✓ Copied 42 shaders
✓ Copied 112 models
✓ Copied 312 UI elements
✓ All assets copied successfully

Step 4: Configuring Git LFS
✓ Git LFS installed and configured
✓ Added LFS tracking for 2691 assets
✓ Updated .gitattributes with 15 patterns
✓ All assets tracked by LFS

Step 5: Creating import presets
✓ Created 2080 texture import presets
✓ Created 145 audio import presets
✓ Created 23 font import presets
✓ Created 89 material import presets
✓ Created 42 shader import presets
✓ All import presets created

Step 6: Updating assets.json
✓ Created/updated assets/expansions/assets.json
✓ Added expansion 05_holdfast entry
✓ Asset count: 2691
✓ Validation flags: git_lfs=true, import_presets=true, directory_structure=true

Step 7: Validating case sensitivity
✓ core.ignorecase=false (correct)
✓ All directory names match case exactly
✓ No Assets/ vs assets/ conflicts

Step 8: Running ashfall-lfs-gate
✓ LFS installation: PASSED
✓ LFS tracking: PASSED
✓ Text file handling: PASSED
✓ Case sensitivity: PASSED
✓ Directory structure: PASSED
✓ Import presets: PASSED
✓ Registry update: PASSED

Step 9: Legacy asset cleanup
✓ Archiving legacy Assets/art/ to assets_migration_20240115.zip
✓ Removing legacy assets from git index
✓ Legacy assets archived successfully

🎉 Asset migration batch completed successfully!

Summary:
- Migrated: 2691 assets
- LFS tracked: 2691 assets
- Import presets: 2691 created
- Registry updated: ✓
- Validation: PASSED
- Legacy cleanup: archived

Next steps:
1. git add assets/expansions/assets.json
2. git add .gitattributes
3. git commit -m "Migrate expansion 05 assets to Godot-native structure"
4. git push
```

### Resulting Directory Structure:
```
assets/
└── expansions/
    └── 05_holdfast/
        └── art/
            ├── characters/
            │   ├── survivors/
            │   │   ├── survivor_male_01.png
            │   │   ├── survivor_female_01.png
            │   │   └── survivor_child_01.png
            │   ├── .import/
            │   │   ├── survivor_male_01.png.import
            │   │   └── survivor_female_01.png.import
            │   └── ...
            ├── environments/
            │   ├── ruins/
            │   │   ├── ruins_factory.png
            │   │   ├── ruins_house.png
            │   │   └── .import/
            │       ├── ruins_factory.png.import
            │       └── ruins_house.png.import
            │   └── ...
            ├── items/
            │   ├── consumables/
            │   │   ├── item_water_filter.png
            │   │   ├── item_medical_kit.png
            │   │   └── .import/
            │       ├── item_water_filter.png.import
            │       └── item_medical_kit.png.import
            │   └── ...
            ├── ui/
            │   ├── buttons/
            │   │   ├── button_normal.png
            │   │   ├── button_hover.png
            │   │   └── .import/
            │       ├── button_normal.png.import
            │       └── button_hover.png.import
            │   └── ...
            └── .import/
                ├── characters.import
                ├── environments.import
                └── items.import
```

## Related Skills
- `ashfall-asset-pack-expansion` - Creates expansion asset pack structure
- `ashfall-lfs-gate` - Validates LFS configuration
- `ashfall-shader-material-lint` - Validates shader materials
- `ashfall-sprite-family-gen` - Generates sprites for items
- `ashfall-tilemap-expansion-kit` - Creates tilemaps for biomes

## Notes
- Follows ASHFALL's strict Unity→Godot migration guidelines
- Validates all assets are tracked by Git LFS
- Creates correct Godot import presets for each asset type
- Ensures case-sensitivity is correct to prevent conflicts
- Provides optional legacy asset cleanup
- Follows Godot 4.7+ asset import best practices

## Maintenance
- Update import preset templates if Godot import settings change
- Add new asset types if expansion domains expand
- Update LFS patterns if new file types are added
- Update assets.json schema if registry format evolves
