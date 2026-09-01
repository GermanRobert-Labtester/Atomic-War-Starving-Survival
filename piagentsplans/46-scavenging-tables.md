# Plan 46 — Location-Specific Scavenging Tables (20 tables)

## Goal (2 lines)
Create `scavenging_tables.json` — 20 location-type-specific loot tables that replace the
generic `lootCategories` strings in `expeditions.json` (Plan 32) with structured, weighted
loot tables. Different location types yield logically different materials: hospitals give
medicine, rail yards give mechanical parts, schools give books, military depots give
ammunition. No arbitrary loot tables.

## Why (P2)
- Verified: `expeditions.json` uses bare `lootCategories` strings (e.g. `"scrap_metal"`,
  `"clean_water"`, `"bandages"`, `"food_rations"`) — these are category tags, not weighted
  loot tables. There is no `scavenging_tables.json`. Scavenging has no location identity.
- Creates the scavenging-depth pillar: the player learns that hospitals are worth
  visiting for medicine, rail yards for tools, military depots for ammunition — each
  location type has a loot signature that makes exploration purposeful, not random.
- Pure DATA work — deepens the existing expedition loot system (Plan 32) with structured
  tables.

## Files to touch
- `Assets/StreamingAssets/Data/scavenging_tables.json` (CREATE — 20 tables)
- Read-only: `Assets/Ashfall.Core/ExpeditionSystem.cs` (confirm how loot is resolved — does
  it read `lootCategories` strings, or can it consume weighted tables? If the latter,
  confirm the table schema: entries, weights, item ids, quantity ranges),
  `Assets/StreamingAssets/Data/expeditions.json` (Plan 32 — `lootCategories` will reference
  table ids from this catalog), `Assets/StreamingAssets/Data/items.json` (159 items — all
  loot entries must resolve to real `item_*` ids)
- Check: `grep -rn "loot\|Loot\|scavenge\|Scavenge\|lootTable\|loot_table" Assets/Ashfall.Core/`

## Content grammar (per table)
- snake_case `id` with prefix `loot_` or `scavenge_` (confirm accepted prefix — do not invent).
- location_type: hospital / rail_yard / school / military_depot / apartment_block / fire_station
  / metro_station / police_station / industrial_district / shopping_center / power_substation
  / chemical_plant / warehouse / farm / forestry_compound / hunting_cabin / monastery /
  clinic / observatory / greenhouse.
- entries: list of { item_id, weight, min_quantity, max_quantity } — weight determines
  relative probability; quantity range determines stack size.
- rarity_tier: common / uncommon / rare / unique — each table has a mix; unique items
  appear only in one location type (creates scavenging motivation).
- hazard_modifier: some loot is contaminated (irradiated medicine, spoiled food, leaking
  chemicals) — a chance the loot carries a disease or radiation risk (feeds Plan 112/09A).
- depletion: does the table deplete on scavenging (one-time) or regenerate (renewable)?
  Most urban sites deplete; wilderness/farm sites regenerate slowly.

## Steps
1. Read `ExpeditionSystem.cs` end-to-end: confirm how loot is resolved from
   `lootCategories`. If it only accepts category strings, this plan either (a) extends the
   resolver to accept table ids (minor Core change) or (b) maps table ids to category
   strings. Confirm before authoring — do not guess.
2. Read `items.json` to inventory all 159 items; classify by which location type would
   plausibly contain them (medicine → hospital, tools → rail yard, books → school, etc.).
3. Read `locations.json` to classify the 115 locations by type (the `description` field
   implies the type; there is no explicit `type` field — infer from description).
4. Author 20 scavenging tables, one per location type. Each table: 8-15 weighted entries
   drawn from `items.json`, with rarity tiers, hazard modifiers, and depletion flags.
   Examples:
   - Hospital: medicine (common), surgical_equipment (uncommon), chemicals (uncommon),
     medical_records (rare, journal unlock), contaminated_waste (hazard), unique_relic (rare).
   - Rail yard: mechanical_parts (common), fuel (uncommon), tools (uncommon), steel
     (common), electrical_equipment (rare), maintenance_log (rare, journal unlock).
   - Military depot: ammunition (common), uniforms (common), communications_gear (uncommon),
     repair_parts (uncommon), unexploded_ordnance (hazard), command_documents (unique).
5. Cross-reference: every `item_*` id in every table resolves to `items.json` (add missing
   items in the same commit — e.g. `item_medical_records`, `item_maintenance_log`).
6. Wire 10 tables into `expeditions.json` (Plan 32) by replacing `lootCategories` strings
   with table ids for the corresponding location types.
7. Link 3 tables to journal unlocks (existing 17C codex) — scavenging certain documents
   unlocks codex entries.
8. Link 2 tables to Plan 112 disease hazards (with Plan 09A diagnostic response) — contaminated loot has a disease risk.
9. Validate: `--data-integrity-selftest`; confirm a scavenging roll produces weighted,
   location-appropriate loot in a headless boot; confirm depletion flags work.
10. xUnit: table loads, weights sum correctly, loot rolls are deterministic (seeded),
    quantity ranges respected, hazard modifiers apply, depletion flags persist across save
    round-trips.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --expedition-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
MEDIUM — the loot-resolver question (step 1) is the hazard: if `ExpeditionSystem` only
accepts category strings, extending it to accept weighted tables is a Core change. Confirm
the resolver's capability before authoring. If it can't, map tables to category strings as
a fallback (less granular but no Core change).

## Definition of Done
- `scavenging_tables.json` exists with 20 tables, all `item_*` ids resolving, 10 wired into
  `expeditions.json`, loot rolls deterministic, hazard modifiers apply, depletion persists,
  integrity + tests green.

## Follow-on
- Plan 32 (expedition wiring) — `lootCategories` references these tables.
- Plan 37 (excavation sites) — deep-strata sites have their own loot tables (relics).
- Existing 17C (codex) — scavenged documents unlock codex entries.
- Plan 112/09A (disease and response) — contaminated loot carries disease risk.
- Plan 47 (collectibles) — collectibles appear as rare/unique entries in scavenging tables.
