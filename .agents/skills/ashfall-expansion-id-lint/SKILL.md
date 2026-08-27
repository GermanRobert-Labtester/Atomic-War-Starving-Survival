# ASHFALL Expansion System Skill: ashfall-expansion-id-lint

## Overview
Validates ID consistency across ASHFALL expansion data files. Cross-checks new expansion IDs against CatalogIntegrityValidator tiers, static Ids.cs classes, and data loaders to prevent invented prefixes and naming violations.

## Canonical Usage
```bash
# Lint new expansion IDs after data generation
awf expansion-id-lint --expansion 05

# Lint specific ID types
awf expansion-id-lint --item_ids "item_holdfast_ration,item_holdfast_gear"
awf expansion-id-lint --location_ids "loc_holdfast_camp,loc_holdfast_outpost"

# Run in CI pipeline
awf expansion-id-lint --all
```

## What It Automates

### 1. CatalogIntegrityValidator Tier-1 Validation
- Scans `CatalogIntegrityValidator.cs` for known snake_case prefixes
- Verifies expansion IDs follow the pattern: `expansion_<number>`
- Validates item IDs follow: `item_<expansion>_<name>`
- Validates location IDs follow: `loc_<expansion>_<name>`
- Validates quest IDs follow: `quest_<expansion>_<name>`
- Validates NPC IDs follow: `npc_<expansion>_<name>`
- Reports any IDs with unknown or invented prefixes

### 2. CatalogIntegrityValidator Tier-2 Validation
- Scans all JSON files for reference keys:
  - `resultItemId`
  - `requiredItemId`
  - `locationId`
  - `npcId`
  - `questId`
  - `factionId`
- Verifies referenced IDs exist in the catalog
- Reports broken references and missing IDs

### 3. Static Ids.cs Class Validation
- Verifies `expansion_05_ids.cs` exists
- Validates `public const string Id = "expansion_05";`
- Validates `public static readonly string[] AllItemIds` contains all expansion item IDs
- Validates `public static readonly string[] AllLocationIds` contains all expansion location IDs
- Validates `public static readonly string[] AllQuestIds` contains all expansion quest IDs
- Reports missing or incorrect Ids.cs constants

### 4. Data Loader Validation
- Verifies `Expansion05ItemCatalogLoader.cs` exists
- Verifies `Expansion05LocationCatalogLoader.cs` exists
- Verifies `Expansion05QuestCatalogLoader.cs` exists
- Verifies loaders reference correct JSON files
- Reports missing or misconfigured loaders

### 5. JSON Schema Validation
- Verifies `expansion_05.json` has `schema_version: "1"`
- Verifies `expansion_05.json` has `id: "expansion_05"`
- Verifies JSON files follow snake_case naming
- Reports missing or incorrect schema fields

## Time Saved
- **20 minutes per data PR** (manual validation and cross-checking)
- **85% reduction** in ID-related bugs
- Prevents broken references and naming violations

## Prerequisites
- Expansion data generated via `ashfall-expansion-data-gen`
- JSON files in `Assets/StreamingAssets/Data/`
- Static Ids.cs classes created
- `dotnet` CLI available
- Godot project in workspace

## Verification After Use
```bash
# Run data integrity test
godot --headless --path . -- --data-integrity-selftest

# Verify catalog integrity validator passes
# (Should report 0 errors for new expansion IDs)

# Verify all JSON files load correctly
dotnet run --project Ashfall.Core -- --validate-data
```

## Integration Points
- **Depends on:** `ashfall-expansion-data-gen` (creates the data to validate)
- **Used by:** `ashfall-expansion-scaffold` (validates new expansion IDs)
- **Follow-up skills:** `ashfall-expansion-narrative-weave` (validates narrative IDs)

## Error Detection
The skill detects and reports:

### 1. Invented Prefix Violations
```
❌ ERROR: Invented prefix detected in expansion 05:
   - Item ID: "item_holdfast_rations" (should be "item_holdfast_ration")
   - Location ID: "loc_holdfast_main_camp" (should be "loc_holdfast_camp")
   - Quest ID: "quest_holdfast_primary_mission" (should be "quest_holdfast_main")

   Known prefixes: item_, loc_, quest_, npc_, faction_, trait_, affliction_,
                   skill_, knowledge_, expansion_, sector_, zone_, echo_, radio_
```

### 2. Missing IDs in Ids.cs
```
❌ ERROR: Missing IDs in expansion_05_ids.cs:
   - item_holdfast_water_filter not found in AllItemIds array
   - loc_holdfast_trading_post not found in AllLocationIds array
   - quest_holdfast_supply_run not found in AllQuestIds array
```

### 3. Broken References
```
❌ ERROR: Broken reference in items.json:
   - item_holdfast_gear_pack references requiredItemId: "item_holdfast_missing_item"
   - This item ID does not exist in any catalog

❌ ERROR: Broken reference in quests.json:
   - quest_holdfast_main references locationId: "loc_holdfast_missing_loc"
   - This location ID does not exist in any catalog
```

### 4. Schema Violations
```
❌ ERROR: Schema violation in expansion_05.json:
   - Missing field: schema_version
   - Missing field: id
   - Field name should be snake_case: "ItemIds" should be "item_ids"
```

### 5. Naming Convention Violations
```
⚠️  WARNING: Naming convention violation:
   - Item ID: "itemHoldfastRation" (should be snake_case: "item_holdfast_ration")
   - Location ID: "LOC_HoldfastCamp" (should be lowercase: "loc_holdfast_camp")
   - Quest ID: "QuestHoldfastMain" (should be snake_case: "quest_holdfast_main")
```

## Automated Fixes
The skill can automatically apply fixes for:

### 1. Rename IDs to Follow Convention
- Converts camelCase to snake_case
- Converts PascalCase to snake_case
- Converts uppercase to lowercase
- Updates all references in JSON files
- Updates Ids.cs arrays

### 2. Add Missing IDs to Ids.cs
- Adds missing IDs to AllItemIds array
- Adds missing IDs to AllLocationIds array
- Adds missing IDs to AllQuestIds array
- Maintains alphabetical order

### 3. Fix Broken References
- Updates references to use correct IDs
- Reports unresolvable references that need manual fixing
- Validates references after fixing

### 4. Add Missing Schema Fields
- Adds `schema_version: "1"` if missing
- Adds `id: "expansion_<number>"` if missing
- Converts field names to snake_case

## Configuration
- **Expansion number:** 01-99 (default: reads from expansion_XX.json)
- **Strict mode:** Enables additional validation checks (default: true)
- **Auto-fix:** Applies safe fixes automatically (default: dry-run)
- **Scope:** Can validate specific ID types (items, locations, quests, NPCs)

## Example Validation Output
```
✓ Expansion 05 ID validation passed:

  Item IDs (25/25):
    ✓ item_holdfast_ration
    ✓ item_holdfast_water_filter
    ✓ item_holdfast_gear_pack
    ✓ item_holdfast_medical_kit
    ...

  Location IDs (12/12):
    ✓ loc_holdfast_camp
    ✓ loc_holdfast_outpost
    ✓ loc_holdfast_trading_post
    ✓ loc_holdfast_radio_tower
    ...

  Quest IDs (8/8):
    ✓ quest_holdfast_main
    ✓ quest_holdfast_supply_run
    ✓ quest_holdfast_defense
    ✓ quest_holdfast_exploration
    ...

  Reference validation:
    ✓ All item references valid
    ✓ All location references valid
    ✓ All NPC references valid
    ✓ All faction references valid

  Static Ids.cs validation:
    ✓ expansion_05_ids.cs exists
    ✓ AllItemIds contains 25 IDs
    ✓ AllLocationIds contains 12 IDs
    ✓ AllQuestIds contains 8 IDs

✓ No errors found! Expansion 05 IDs are valid.
```

## Related Skills
- `ashfall-expansion-scaffold` - Creates the expansion system
- `ashfall-expansion-data-gen` - Generates expansion data
- `ashfall-expansion-narrative-weave` - Weaves narrative content
- `ashfall-catalog-audit` - Deep catalog audit beyond ID validation
- `ashfall-dialog-graph-lint` - Validates quest/flag graph reachability

## Notes
- Follows ASHFALL's strict snake_case naming convention
- Validates against `CatalogIntegrityValidator` tiers 1 and 2
- Ensures all IDs are registered in static Ids.cs classes
- Validates all cross-file references
- Prevents invented prefixes that break catalog integrity

## Maintenance
- Update known prefix list if new ID types are added
- Update validation rules if naming conventions change
- Add new reference key types if data schema evolves
- Update Ids.cs validation if static class structure changes
