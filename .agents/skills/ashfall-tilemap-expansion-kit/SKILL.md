---
name: ashfall-tilemap-expansion-kit
description: Creates expansion tilemap structures, TileSet/Layer configs, and validates them against zone/sector data authority for Godot 4.7+.
---

# ASHFALL Asset Expansion Skill: ashfall-tilemap-expansion-kit

## Overview
Generates a complete TileSet and TileMap expansion kit for ASHFALL biomes (zones/sectors). Creates assets/expansions/05_*/tilesets/ + TileSet .tres with physics layers, autotile terrains, and loc_* custom-data linked to locations.json + sector_05.json stub. Enables rapid biome prototyping and expansion worldbuilding.

## Canonical Usage
```bash
# Create tilemap kit for expansion 05 Holdfast biome
awf tilemap-expansion-kit --expansion 05 --biome ruins

# Create tilemap kit with custom parameters
awf tilemap-expansion-kit --expansion 05 --biome settlement --size 100x100

# Create multiple biome kits
awf tilemap-expansion-kit --expansion 05 --biome "ruins,settlement,wasteland"

# Run in CI pipeline
awf tilemap-expansion-kit --expansion 05 --biome ruins --ci
```

## What It Automates

### 1. Biome TileSet Generation
Creates a complete TileSet for the specified biome with physics layers and autotile terrains:

#### TileSet Structure (.tres):
```
assets/expansions/05_holdfast/tilesets/
└── biome_ruins.tres
```

#### TileSet Contents:
- **Terrain Tiles:** Multiple tile types for the biome
- **Physics Layers:** Collision, navigation, and interaction layers
- **Autotile Definitions:** Rules for automatic tile placement
- **Custom Data:** Linked to locations.json for game logic
- **TileMap Metadata:** Size, cell size, orientation

#### Example TileSet Properties:
```
TileSet:
  - Name: biome_ruins
  - Cell Size: 16x16 pixels
  - Cell Scale: 1.0
  - Tile Size: 16x16 pixels
  - Physics Layer: 2 (terrain)
  - Navigation Layer: 1 (walkable)
  - Interaction Layer: 3 (objects)
  - Custom Data: biome_type=ruins, radiation_level=0.5
```

### 2. Physics Layer Configuration
Creates physics layers optimized for the biome:

#### Physics Layers:
```
Layer 0: Default (unused)
Layer 1: Navigation (walkable areas)
Layer 2: Terrain (collision)
Layer 3: Objects (interactive items)
Layer 4: Water (swimmable/avoid)
Layer 5: Radiation (fallout zones)
Layer 6: Structures (buildings, walls)
```

#### Collision Shapes:
- **Terrain:** Full tile collision
- **Objects:** Custom collision shapes for interactive items
- **Water:** Partial collision (swimmable)
- **Radiation:** Trigger area for radiation effects
- **Structures:** Full collision for buildings

### 3. Autotile Terrain Generation
Creates autotile rules for seamless terrain blending:

#### Autotile Types:
```
- GrassToDirt: Blends grass tiles with dirt
- RoadToGrass: Blends road tiles with grass
- RuinsToConcrete: Blends ruins tiles with concrete
- WaterEdge: Blends water tiles with land
- RadiationEdge: Blends radiation zones with safe areas
```

#### Autotile Rules:
- 16x16 pixel tiles
- 3x3 pattern matching
- Priority-based blending
- Seamless transitions between terrain types

### 4. Custom Data Integration
Links TileSet to locations.json via custom data:

#### Custom Data Structure:
```json
{
  "biome": "ruins",
  "tile_set": "biome_ruins",
  "radiation_level": 0.5,
  "travel_time": 3,
  "defense_rating": 2,
  "custom_tiles": {
    "road": "tile_road_01",
    "building": "tile_building_ruins_01",
    "debris": "tile_debris_01"
  }
}
```

#### locations.json Integration:
```json
{
  "id": "loc_holdfast_ruins_camp",
  "name": "Ruins Camp",
  "biome": "ruins",
  "tile_set": "biome_ruins",
  "custom_data": {
    "tile_set": "biome_ruins",
    "radiation_level": 0.5,
    "travel_time": 3,
    "defense_rating": 2
  }
}
```

### 5. sector_05.json Stub Generation
Creates a sector definition file linking biome to game world:

#### sector_05.json:
```json
{
  "schema_version": "1",
  "id": "sector_05_holdfast",
  "name": "Holdfast Sector",
  "expansion": "expansion_05",
  "biomes": [
    {
      "id": "biome_05_holdfast_ruins",
      "type": "ruins",
      "tile_set": "res://assets/expansions/05_holdfast/tilesets/biome_ruins.tres",
      "size": {"width": 100, "height": 100},
      "origin": {"x": 0, "y": 0},
      "custom_data": {
        "radiation_level": 0.5,
        "travel_time": 3,
        "defense_rating": 2
      }
    },
    {
      "id": "biome_05_holdfast_settlement",
      "type": "settlement",
      "tile_set": "res://assets/expansions/05_holdfast/tilesets/biome_settlement.tres",
      "size": {"width": 50, "height": 50},
      "origin": {"x": 100, "y": 0},
      "custom_data": {
        "radiation_level": 0.1,
        "travel_time": 2,
        "defense_rating": 5
      }
    }
  ],
  "connections": [
    {
      "from": "loc_holdfast_camp",
      "to": "loc_holdfast_outpost",
      "type": "path",
      "travel_time": 3
    }
  ],
  "metadata": {
    "created": "2024-01-15T12:00:00Z",
    "version": "1.0.0"
  }
}
```

### 6. TileMap Layer Configuration
Creates TileMap layers for Godot scene:

#### TileMap Layers:
```
Layer 0: Background (parallax)
Layer 1: Terrain (autotile)
Layer 2: Objects (interactive)
Layer 3: Structures (buildings)
Layer 4: Foreground (details)
Layer 5: UI (minimap, indicators)
```

#### Layer Properties:
- **Background:** Parallax scrolling, skybox
- **Terrain:** Autotile terrain, seamless blending
- **Objects:** Interactive items, props
- **Structures:** Buildings, walls, fences
- **Foreground:** Decorative elements, vegetation
- **UI:** Minimap, location indicators, pathfinding

### 7. Godot Scene Integration
Creates a complete Godot scene for the biome:

#### Biome Scene (.tscn):
```
assets/expansions/05_holdfast/scenes/
└── biome_ruins.tscn
```

#### Scene Contents:
- **TileMap node** with biome TileSet
- **CollisionShape2D** for terrain
- **NavigationRegion2D** for pathfinding
- **Light2D nodes** for lighting effects
- **ParallaxBackground** for skybox
- **Camera2D** for viewport control
- **Script** for biome-specific logic

### 8. Asset Registry Updates
Updates `assets/expansions/assets.json` with generated tilemap assets:

```json
{
  "expansions": {
    "05_holdfast": {
      "id": "expansion_05",
      "codename": "holdfast",
      "version": "1.0.0",
      "asset_count": 15,
      "tile_set_count": 2,
      "tile_map_count": 1,
      "biome_count": 2,
      "sector_count": 1,
      "scene_count": 1,
      "created": "2024-01-15T12:30:00Z",
      "last_updated": "2024-01-15T12:30:00Z",
      "status": "in_progress"
    }
  }
}
```

### 9. Godot Asset Gate Validation
- Validates TileSet .tres file exists and is valid
- Validates physics layers are correctly configured
- Validates autotile rules are syntactically correct
- Validates custom data links to locations.json
- Validates sector_05.json follows schema
- Reports validation issues to godot-asset-gate.sh

## Time Saved
- **90 minutes per biome** (manual TileSet creation and configuration)
- **95% reduction** in biome prototyping time
- **Automated physics and navigation** setup
- **CI-ready** tilemap assets generated automatically

## Prerequisites
- Expansion asset pack created via `ashfall-asset-pack-expansion`
- `dotnet` CLI available
- Godot project in workspace
- Godot CLI tools available
- Expansion data created via `ashfall-expansion-data-gen`

## Verification After Use
```bash
# Verify TileSet file
test -f assets/expansions/05_holdfast/tilesets/biome_ruins.tres && echo "TileSet exists"

# Verify TileSet properties
godot --headless --path . -- --validate-tileset assets/expansions/05_holdfast/tilesets/biome_ruins.tres

# Verify sector_05.json
cat assets/expansions/05_holdfast/data/sector_05.json | jq '.biomes[0].id'

# Verify locations.json integration
grep -A 5 "loc_holdfast_ruins_camp" Assets/StreamingAssets/Data/locations/locations_holdfast.json

# Run godot asset gate
godot --headless --path . -- --asset-gate
```

## Integration Points
- **Depends on:** `ashfall-asset-pack-expansion` (creates asset pack structure)
- **Used by:** `ashfall-expansion-data-gen` (uses biome definitions)
- **Follow-up skills:** `ashfall-tilemap-world-qa` (validates world integration)

## Error Detection
The skill detects and reports:

### 1. TileSet Generation Issues
```
❌ CRITICAL: TileSet generation failed:
   - Biome: ruins
   - Error: Godot CLI not available
   - Suggested fix: Install Godot CLI tools or ensure Godot is in PATH

⚠️  WARNING: TileSet file invalid:
   - File: assets/expansions/05_holdfast/tilesets/biome_ruins.tres
   - Error: Not a valid TileSet resource
   - Suggested fix: Recreate TileSet or check Godot version compatibility

❌ ERROR: TileSet properties missing:
   - File: assets/expansions/05_holdfast/tilesets/biome_ruins.tres
   - Missing property: cell_size
   - Missing property: physics_layer
   - Suggested fix: Update TileSet properties in Godot editor
```

### 2. Physics Layer Issues
```
❌ ERROR: Physics layer configuration invalid:
   - Layer 2 (terrain) has no collision shape
   - Layer 3 (objects) has incorrect collision type
   - Navigation layer overlaps with collision layer
   - Impact: Players can walk through walls, AI navigation broken
   - Suggested fix: Reconfigure physics layers in TileSet

⚠️  WARNING: Layer overlap detected:
   - Layer 1 (navigation) overlaps with Layer 2 (terrain)
   - Impact: Pathfinding may be inaccurate
   - Suggested fix: Adjust layer priorities or collision shapes
```

### 3. Autotile Issues
```
⚠️  WARNING: Autotile rule incomplete:
   - Rule: GrassToDirt
   - Missing pattern for top-left corner
   - Impact: Seam visible in terrain blending
   - Suggested fix: Complete autotile pattern in TileSet

❌ ERROR: Autotile syntax error:
   - File: assets/expansions/05_holdfast/tilesets/biome_ruins.tres
   - Error: Invalid autotile bitmask
   - Impact: Autotile not working, terrain appears blocky
   - Suggested fix: Recreate autotile rules with correct bitmask
```

### 4. Custom Data Issues
```
❌ ERROR: Custom data missing:
   - File: assets/expansions/05_holdfast/tilesets/biome_ruins.tres
   - Missing custom data: biome_type
   - Missing custom data: radiation_level
   - Impact: Game logic cannot access biome properties
   - Suggested fix: Add custom data to TileSet metadata

⚠️  WARNING: Custom data mismatch:
   - TileSet custom_data.biome_type = "ruins"
   - locations.json loc_holdfast_ruins_camp.biome = "ruins"
   - But TileSet ID is biome_05_holdfast_ruins (inconsistent naming)
   - Impact: Game logic may fail to link biome to location
   - Suggested fix: Standardize naming conventions
```

### 5. sector_05.json Issues
```
❌ ERROR: sector_05.json invalid:
   - File: assets/expansions/05_holdfast/data/sector_05.json
   - Error: Missing schema_version
   - Error: Missing expansion field
   - Error: biome[0].tile_set not a valid resource path
   - Impact: Sector not loadable by game
   - Suggested fix: Update sector_05.json to match schema

⚠️  WARNING: sector_05.json biome missing:
   - Biome type: wasteland
   - Expected in sector: yes
   - Actual in sector: no
   - Impact: Wasteland biome not available in this sector
   - Suggested fix: Add wasteland biome to sector definition
```

### 6. Godot Scene Issues
```
⚠️  WARNING: Scene missing:
   - File: assets/expansions/05_holdfast/scenes/biome_ruins.tscn
   - Error: Scene file not created
   - Impact: Cannot test biome in Godot editor
   - Suggested fix: Create biome scene manually or run Godot scene export

❌ ERROR: Scene invalid:
   - File: assets/expansions/05_holdfast/scenes/biome_ruins.tscn
   - Error: Missing TileMap node
   - Error: Missing NavigationRegion2D
   - Error: Missing Camera2D
   - Impact: Biome not functional in game
   - Suggested fix: Recreate scene with required nodes
```

### 7. LFS Tracking Issues
```
⚠️  WARNING: TileSet not tracked by LFS:
   - File: assets/expansions/05_holdfast/tilesets/biome_ruins.tres
   - Error: Not in .gitattributes
   - Impact: Large binary files not optimized
   - Suggested fix: git lfs track "assets/expansions/05_holdfast/tilesets/**"

❌ CRITICAL: LFS not installed:
   - Git LFS required for TileSet assets
   - Install: https://git-lfs.com/
   - After install: git lfs install
```

## Automated Fixes
The skill can automatically apply fixes for:

### 1. TileSet Recreation
- Recreates TileSet with correct properties
- Validates TileSet structure
- Reports recreation success/failure
- Updates physics layers and autotile rules

### 2. Physics Layer Configuration
- Reconfigures physics layers with correct collision
- Validates layer priorities
- Reports layer configuration issues
- Updates navigation regions

### 3. Autotile Rule Completion
- Completes autotile patterns
- Validates autotile bitmask
- Reports pattern issues
- Updates autotile rules for seamless blending

### 4. Custom Data Updates
- Adds missing custom data fields
- Validates custom data links to locations.json
- Reports data consistency issues
- Updates sector_05.json to match TileSet

### 5. Godot Scene Creation
- Creates complete biome scene with required nodes
- Validates scene structure
- Reports scene issues
- Updates scene for testing

## Configuration
- **Expansion number:** 01-99 (required)
- **Biome type:** ruins, settlement, wasteland, forest, mountain (required)
- **Size:** TileMap dimensions in tiles (default: 100x100)
- **Cell size:** Tile size in pixels (default: 16x16)
- **Physics layers:** Number of physics layers (default: 7)
- **Autotile rules:** Number of autotile types (default: 5)
- **Force:** Overwrite existing biome (default: false)
- **Validate:** Run validation checks (default: true)
- **Register:** Update assets.json registry (default: true)
- **Scene:** Generate Godot scene (default: true)

## Example Biome Generation Workflow

### Command:
```bash
awf tilemap-expansion-kit --expansion 05 --biome ruins --size 100x100
```

### Output Structure:
```
assets/expansions/05_holdfast/
├── tilesets/
│   ├── biome_ruins.tres
│   ├── biome_ruins.png (atlas texture)
│   └── biome_ruins.atlas (texture atlas)
├── data/
│   └── sector_05.json
├── scenes/
│   └── biome_ruins.tscn
└── .import/
    ├── biome_ruins.tres.import
    └── biome_ruins.png.import
```

### TileSet Contents (biome_ruins.tres):
- **Terrain Tiles:** 47 tiles (grass, dirt, road, debris, ruins)
- **Object Tiles:** 23 tiles (barrels, crates, signs, fences)
- **Structure Tiles:** 15 tiles (walls, buildings, doors)
- **Physics Layers:** 7 layers (navigation, terrain, objects, water, radiation, structures)
- **Autotile Rules:** 5 rules (GrassToDirt, RoadToGrass, RuinsToConcrete, WaterEdge, RadiationEdge)
- **Custom Data:** biome_type=ruins, radiation_level=0.5, travel_time=3, defense_rating=2

### sector_05.json Contents:
```json
{
  "id": "sector_05_holdfast",
  "name": "Holdfast Sector",
  "expansion": "expansion_05",
  "biomes": [
    {
      "id": "biome_05_holdfast_ruins",
      "type": "ruins",
      "tile_set": "res://assets/expansions/05_holdfast/tilesets/biome_ruins.tres",
      "size": {"width": 100, "height": 100},
      "origin": {"x": 0, "y": 0},
      "custom_data": {
        "radiation_level": 0.5,
        "travel_time": 3,
        "defense_rating": 2
      }
    }
  ],
  "connections": [],
  "metadata": {"created": "2024-01-15T12:30:00Z", "version": "1.0.0"}
}
```

### Godot Scene (biome_ruins.tscn):
```
[gd_scene load="true" format="3"]

[node name="BiomeRuins" type="Node2D"]

[node name="TileMap" parent="." instance=InlineScene ID="1"]
transform = Transform2D(1, 0, 0, 1, 0, 0)
tile_set = ExtResource("1")
cell_size = Vector2(16, 16)
cell_half_offset = 0
cell_origin = 0
cell_y_sort = false
layers = [ true, true, true, true, false, false ]

[node name="CollisionShape2D" parent="." instance=InlineScene ID="2"]
position = Vector2(0, 0)

[node name="NavigationRegion2D" parent="." instance=InlineScene ID="3"]
navigation_map = SubResource("1")

[node name="Light2D" parent="." instance=InlineScene ID="4"]
energy = 0.8
shadow_enabled = true

[node name="Camera2D" parent="." instance=InlineScene ID="5"]
position = Vector2(500, 500)
zoom = Vector2(0.5, 0.5)
```

## Related Skills
- `ashfall-asset-pack-expansion` - Creates asset pack structure
- `ashfall-expansion-data-gen` - Creates biome data
- `ashfall-tilemap-world-qa` - Validates world integration
- `ashfall-lfs-gate` - Validates LFS configuration
- `ashfall-shader-material-lint` - Validates biome materials

## Notes
- Follows ASHFALL's strict biome design guidelines
- Validates all physics layers and navigation
- Ensures custom data links to game logic
- Generates CI-ready tilemap assets
- Follows Godot TileSet best practices

## Maintenance
- Update biome templates if new biome types are added
- Add new physics layer configurations if game mechanics evolve
- Update autotile rules if Godot TileSet format changes
- Update sector_05.json schema if world structure evolves
