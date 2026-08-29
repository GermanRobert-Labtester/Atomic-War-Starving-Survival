---
name: ashfall-expansion-data-gen
description: Bulk-generates 20–50 JSON data entries for expansion domains from CSV prompts and validates them with --data-integrity-selftest.
---

# ASHFALL Expansion System Skill: ashfall-expansion-data-gen

## Overview
Bulk generates 20–50 JSON data entries for ASHFALL expansion domains from CSV prompts. Validates generated data via `--data-integrity-selftest` in the same run, ensuring immediate feedback on schema violations and naming conventions.

## Canonical Usage
```bash
# Generate items for expansion 05 from CSV
awf expansion-data-gen --expansion 05 --type items --input items_holdfast.csv

# Generate locations for expansion 05
awf expansion-data-gen --expansion 05 --type locations --input locations_holdfast.csv

# Generate quests for expansion 05
awf expansion-data-gen --expansion 05 --type quests --input quests_holdfast.csv

# Generate NPCs for expansion 05
awf expansion-data-gen --expansion 05 --type npcs --input npcs_holdfast.csv

# Bulk generate multiple types
awf expansion-data-gen --expansion 05 --types "items,locations,quests,npcs" --input-dir ./expansion_05_data/
```

## What It Automates

### 1. CSV-to-JSON Generation
For each row in the CSV, generates:

#### Items (item_*.json)
```json
{
  "schema_version": "1",
  "id": "item_holdfast_water_filter",
  "name": "Holdfast Water Filter",
  "description": "A portable ceramic water filter that removes radioactive particles...",
  "category": "consumable",
  "weight": 1.2,
  "volume": 0.5,
  "value": 450,
  "rarity": "common",
  "icon": "assets/expansions/05_holdfast/sprites/item_water_filter.png",
  "tags": ["water", "filter", "consumable", "holdfast"],
  "min_day": 5,
  "max_day": 999,
  "is_tradable": true,
  "is_craftable": false,
  "crafting_recipe": null
}
```

#### Locations (loc_*.json)
```json
{
  "schema_version": "1",
  "id": "loc_holdfast_camp",
  "name": "Holdfast Main Camp",
  "description": "A fortified settlement near the old highway...",
  "sector": "sector_05_holdfast",
  "biome": "ruins",
  "travel_time": 3,
  "radiation_level": 0.2,
  "is_sheltered": true,
  "has_medical": true,
  "has_trade": true,
  "has_workshop": false,
  "defense_rating": 3,
  "custom_data": {
    "holdfast": {
      "faction": "holdfast",
      "is_capital": true
    }
  }
}
```

#### Quests (quest_*.json)
```json
{
  "schema_version": "1",
  "id": "quest_holdfast_main",
  "name": "Supply the Holdfast",
  "description": "Deliver essential supplies to the Holdfast Main Camp...",
  "type": "delivery",
  "required_items": ["item_holdfast_supplies"],
  "reward_items": ["item_holdfast_reputation_token"],
  "reward_xp": 250,
  "min_day": 10,
  "max_day": 999,
  "is_hidden": false,
  "prerequisite_quests": [],
  "flags": ["holdfast_active", "main_camp_accessible"]
}
```

#### NPCs (npc_*.json)
```json
{
  "schema_version": "1",
  "id": "npc_holdfast_commander",
  "name": "Commander Elias Voss",
  "title": "Holdfast Commander",
  "description": "A grizzled veteran leading the Holdfast survivors...",
  "faction": "holdfast",
  "location": "loc_holdfast_camp",
  "dialogue_file": "assets/expansions/05_holdfast/dialogue/commander_elias.json",
  "traits": ["leader", "military", "ruthless"],
  "afflictions": [],
  "skills": ["leadership", "tactics"],
  "flags": ["holdfast_commander", "can_assign_quests"]
}
```

### 2. Schema Version Injection
- Automatically sets `schema_version: "1"` on all generated JSON
- Validates schema_version is correct for the data type
- Reports if schema_version is missing or incorrect

### 3. ID Generation
- Generates snake_case IDs from CSV values:
  - `Holdfast Water Filter` → `item_holdfast_water_filter`
  - `Holdfast Main Camp` → `loc_holdfast_camp`
  - `Supply the Holdfast` → `quest_holdfast_main`
  - `Commander Elias Voss` → `npc_holdfast_commander`
- Validates ID uniqueness across all generated entries
- Reports duplicate IDs

### 4. File Naming Convention
- Generates files named after IDs:
  - `item_holdfast_water_filter.json`
  - `loc_holdfast_camp.json`
  - `quest_holdfast_main.json`
  - `npc_holdfast_commander.json`
- Places files in correct directories:
  - Items: `Assets/StreamingAssets/Data/items/`
  - Locations: `Assets/StreamingAssets/Data/locations/`
  - Quests: `Assets/StreamingAssets/Data/quests/`
  - NPCs: `Assets/StreamingAssets/Data/npcs/`

### 5. Data Integrity Validation
- Runs `godot --headless --path . -- --data-integrity-selftest` after generation
- Validates all generated JSON files pass CatalogIntegrityValidator
- Reports any validation errors with file paths and line numbers
- Validates cross-file references (e.g., quest references location)

### 6. Static Ids.cs Updates
- Updates `expansion_05_ids.cs` with new IDs:
  - Adds to `AllItemIds` array
  - Adds to `AllLocationIds` array
  - Adds to `AllQuestIds` array
  - Adds to `AllNpcIds` array
- Maintains alphabetical order
- Validates no duplicates

### 7. Asset Path Validation
- Validates generated asset paths exist or will be created:
  - Sprite paths
  - Dialogue file paths
  - Audio paths
- Reports missing asset directories

## Time Saved
- **60 minutes per bulk generation** (manual JSON creation and validation)
- **95% reduction** in data entry errors
- **Immediate feedback** on schema violations
- **Automated validation** eliminates manual testing

## Prerequisites
- CSV input files with required columns
- Expansion system created via `ashfall-expansion-scaffold`
- `dotnet` CLI available
- Godot project in workspace
- `godot` CLI available for validation

## CSV Format Specifications

### Items CSV (items_holdfast.csv)
```csv
name,description,category,weight,volume,value,rarity,icon_path,min_day,max_day,is_tradable,is_craftable
Holdfast Water Filter,A portable ceramic water filter that removes radioactive particles.,consumable,1.2,0.5,450,common,assets/expansions/05_holdfast/sprites/item_water_filter.png,5,999,true,false
Holdfast Medical Kit,A first aid kit with basic medical supplies.,consumable,0.8,0.3,600,common,assets/expansions/05_holdfast/sprites/item_medical_kit.png,5,999,true,false
Holdfast Gear Pack,A backpack with basic survival gear.,equipment,2.5,1.0,800,common,assets/expansions/05_holdfast/sprites/item_gear_pack.png,5,999,true,false
```

Required columns: name, description, category, weight, volume, value, rarity
Optional columns: icon_path, min_day, max_day, is_tradable, is_craftable, tags

### Locations CSV (locations_holdfast.csv)
```csv
name,description,sector,biome,travel_time,radiation_level,is_sheltered,has_medical,has_trade,has_workshop,defense_rating,custom_data
Holdfast Main Camp,A fortified settlement near the old highway.,sector_05_holdfast,ruins,3,0.2,true,true,true,false,3,"{""holdfast"": {""faction"": ""holdfast"", ""is_capital"": true}}"
Holdfast Outpost,A small outpost near the river.,sector_05_holdfast,ruins,2,0.3,false,true,false,false,1,"{""holdfast"": {""faction"": ""holdfast""}}"
Holdfast Trading Post,A trading post with basic supplies.,sector_05_holdfast,ruins,4,0.1,true,true,true,true,2,"{""holdfast"": {""faction"": ""holdfast""}}"
```

Required columns: name, description, sector, biome
Optional columns: travel_time, radiation_level, is_sheltered, has_medical, has_trade, has_workshop, defense_rating, custom_data

### Quests CSV (quests_holdfast.csv)
```csv
name,description,type,required_items,reward_items,reward_xp,min_day,max_day,is_hidden,prerequisite_quests,flags
Supply the Holdfast,Deliver essential supplies to the Holdfast Main Camp.,delivery,"item_holdfast_supplies","item_holdfast_reputation_token",250,10,999,false,,"holdfast_active,main_camp_accessible"
Defend the Outpost,Protect the Holdfast Outpost from raiders.,combat,"item_holdfast_ammo,item_holdfast_medical_kit","item_holdfast_defense_badge",350,15,999,false,quest_holdfast_main,"holdfast_active,outpost_accessible"
Explore the Radio Tower,Investigate the old radio tower for supplies.,exploration,"","item_holdfast_radio_parts,item_holdfast_gear_pack",200,20,999,false,quest_holdfast_main,exploration_active
```

Required columns: name, description, type
Optional columns: required_items, reward_items, reward_xp, min_day, max_day, is_hidden, prerequisite_quests, flags

### NPCs CSV (npcs_holdfast.csv)
```csv
name,title,description,faction,location,dialogue_file,traits,afflictions,skills,flags
Commander Elias Voss,Holdfast Commander,A grizzled veteran leading the Holdfast survivors.,holdfast,loc_holdfast_camp,assets/expansions/05_holdfast/dialogue/commander_elias.json,"leader,military,ruthless",,"leadership,tactics","holdfast_commander,can_assign_quests"
Medic Sarah Chen,Field Medic,A skilled medic providing medical aid to survivors.,holdfast,loc_holdfast_camp,assets/expansions/05_holdfast/dialogue/medic_sarah.json,"medic,compassionate,skilled",,"medicine,first_aid","holdfast_medic"
Scout Marcus Boone,Scout Leader,A experienced survivor who knows the wasteland.,holdfast,loc_holdfast_outpost,assets/expansions/05_holdfast/dialogue/scout_marcus.json,"scout,explorer,knowledgeable",,"survival,tracking","holdfast_scout"
```

Required columns: name, title, description, faction, location
Optional columns: dialogue_file, traits, afflictions, skills, flags

## Verification After Use
```bash
# Verify compilation
dotnet build Ashfall.Core/Ashfall.Core.csproj

# Run data integrity test
godot --headless --path . -- --data-integrity-selftest

# Verify catalog integrity validator passes
# Should report 0 errors for new expansion data

# Verify static Ids.cs was updated
cat Assets/Ashfall.Core/Ids/expansion_05_ids.cs | grep -A 5 "AllItemIds"
```

## Integration Points
- **Depends on:** `ashfall-expansion-scaffold` (creates expansion structure)
- **Used by:** `ashfall-expansion-id-lint` (validates generated IDs)
- **Follow-up skills:** `ashfall-expansion-narrative-weave` (weaves quests/flags)

## Error Handling
The skill detects and reports:

### 1. CSV Format Errors
```
❌ ERROR: CSV format error in items_holdfast.csv:
   - Missing required column: 'category'
   - Row 5: Missing value for 'weight'
   - Row 7: Duplicate name 'Holdfast Water Filter'

❌ ERROR: Invalid value in locations_holdfast.csv:
   - Row 3, column 'radiation_level': 'high' (should be numeric: 0.1-1.0)
   - Row 5, column 'defense_rating': 'five' (should be integer: 0-10)
```

### 2. ID Generation Errors
```
❌ ERROR: ID generation failed:
   - Duplicate ID generated: 'item_holdfast_water_filter' (already exists in catalog)
   - Invalid character in name: 'Holdfast/Medical Kit' (contains '/')
   - Name too long: 'Holdfast Advanced Medical Kit with Extra Bandages and Antiseptics' (max 50 chars)
```

### 3. Validation Errors
```
❌ ERROR: Data integrity validation failed:
   - items/item_holdfast_broken.json: Missing field 'category'
   - locations/loc_holdfast_missing_sector.json: Invalid sector reference 'sector_05'
   - quests/quest_holdfast_missing_loc.json: Broken location reference 'loc_holdfast_missing'

⚠️  WARNING: CatalogIntegrityValidator Tier-2 error:
   - Item 'item_holdfast_gear_pack' references non-existent required item 'item_holdfast_missing_item'
```

### 4. Asset Path Errors
```
⚠️  WARNING: Asset path validation:
   - Sprite path does not exist: assets/expansions/05_holdfast/sprites/item_water_filter.png
   - Dialogue file does not exist: assets/expansions/05_holdfast/dialogue/commander_elias.json
   - Recommended: Create these assets using ashfall-sprite-family-gen and ashfall-ui-expansion-panel-kit
```

## Automated Fixes
The skill can automatically apply fixes for:

### 1. CSV Format Normalization
- Adds missing required columns with default values
- Converts values to correct types (numeric, boolean)
- Normalizes names (removes special characters, limits length)
- Removes duplicate rows

### 2. ID Generation
- Regenerates IDs if duplicates are found
- Converts names to snake_case
- Ensures ID uniqueness

### 3. Validation Errors
- Adds missing required fields with sensible defaults
- Fixes broken references if possible
- Updates static Ids.cs arrays

### 4. Asset Path Creation
- Creates missing asset directories
- Generates placeholder files if needed
- Updates paths to follow conventions

## Configuration
- **Expansion number:** 01-99 (required)
- **Data type:** items, locations, quests, npcs (required)
- **Input file:** CSV file path (required)
- **Output directory:** Custom output directory (optional)
- **Schema version:** Force specific schema_version (optional)
- **Auto-fix:** Apply safe fixes automatically (default: dry-run)
- **Strict mode:** Enable additional validation (default: true)
- **Batch size:** Number of entries to generate (default: all)

## Example Output Structure
```
Assets/StreamingAssets/Data/
├── items/
│   ├── item_holdfast_water_filter.json
│   ├── item_holdfast_medical_kit.json
│   └── item_holdfast_gear_pack.json
├── locations/
│   ├── loc_holdfast_camp.json
│   ├── loc_holdfast_outpost.json
│   └── loc_holdfast_trading_post.json
├── quests/
│   ├── quest_holdfast_main.json
│   ├── quest_holdfast_defense.json
│   └── quest_holdfast_exploration.json
└── npcs/
    ├── npc_holdfast_commander.json
    ├── npc_holdfast_medic.json
    └── npc_holdfast_scout.json

Assets/Ashfall.Core/Ids/
└── expansion_05_ids.cs
```

## Related Skills
- `ashfall-expansion-scaffold` - Creates expansion system
- `ashfall-expansion-id-lint` - Validates generated IDs
- `ashfall-sprite-family-gen` - Generates sprites for items
- `ashfall-ui-expansion-panel-kit` - Creates UI panels for locations
- `ashfall-expansion-narrative-weave` - Weaves quests into narrative

## Notes
- Follows ASHFALL's strict data schema and naming conventions
- Validates all generated data immediately via data-integrity-selftest
- Updates static Ids.cs classes automatically
- Provides immediate feedback on errors and warnings
- Can generate partial batches for iterative development

## Maintenance
- Update CSV templates if data schema changes
- Add new data types if expansion domains evolve
- Update validation rules if CatalogIntegrityValidator changes
- Add new columns if JSON schema expands
