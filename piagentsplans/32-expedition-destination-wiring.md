# Plan 32 — Expedition Destination Wiring (2 → 50 wired locations)

## Goal (2 lines)
Wire the 115 existing `locations.json` entries into `expeditions.json` — today only **2** of 115
locations are dispatchable expedition destinations. This is the single largest scaffolding gap
in the project: the world exists but the player cannot reach most of it.

## Why this first (P1)
- Verified: `expeditions.json` contains exactly 2 entries (`loc_the_allotments`,
  `loc_denial_cut_substation`); `locations.json` has 115. The expedition dispatch system
  (`ExpeditionSystem.cs`) is fully implemented, save-supported, and UI-wired — the content is
  the only missing layer.
- Unlocks the entire surface-exploration loop: without destinations, the expedition panel,
  vehicle logistics (Task #101), and encounter system have nothing to dispatch to.
- Pure DATA work — zero new Core code, zero save changes, zero determinism risk.

## Files to touch
- `Assets/StreamingAssets/Data/expeditions.json` (add ~48 entries)
- Read-only: `Assets/Ashfall.Core/ExpeditionSystem.cs` (confirm dispatch schema:
  `distanceTicks`, `dangerLevel`, `encounterChancePerTick`, `baseStaminaDrainPerHour`,
  `lootCategories`), `Assets/StreamingAssets/Data/locations.json` (source `id` + `displayName`
  + `dangerLevel` + `travelHours` + `baseRadsPerHour` — reuse these as the basis for expedition
  stats), `CatalogIntegrityValidator` (TIER-1/TIER-2 id resolution)

## Content grammar (per entry)
- `id` must match an existing `locations.json` `id` exactly (TIER-2 reference validation).
- `lootCategories` must use existing `items.json` category strings or item-id prefixes —
  confirm the set the `ExpeditionSystem` loot resolver accepts before authoring.
- `distanceTicks` derived from `travelHours` (verify the tick→hour conversion in
  `ExpeditionSystem` before assigning values; do not guess).
- `dangerLevel` and `encounterChancePerTick` scaled from the location's `dangerLevel` and
  `baseRadsPerHour` — higher radiation and danger → more encounters.
- Grounded tone only in `displayName` (copy from `locations.json`); no new prose required.

## Steps
1. Read `ExpeditionSystem.cs` end-to-end: confirm the dispatch schema, the loot-category
   resolver, the tick→hour conversion, and any caps on `distanceTicks`/`encounterChance`.
2. Read all 115 `locations.json` entries; classify by danger/radiation/travel into tiers
   (scavenge / standard / hazardous / deep).
3. Cross-reference: which 2 locations are already wired; do not duplicate.
4. Author 48 new expedition entries in 4 tiers: 12 scavenge (danger 1-3, low rads),
   18 standard (danger 3-5), 12 hazardous (danger 5-7, high rads), 6 deep (danger 7+, extreme).
5. Assign `lootCategories` per location identity: hospital → medicine/chemicals/records;
   rail yard → mechanical_parts/fuel/tools/steel; school → books/stationery/food_remnants;
   military depot → ammunition/communications/repair_parts; etc. (location-specific, not
   arbitrary — see Plan 46 scavenging tables for the full taxonomy).
6. Verify every `id` resolves to a `locations.json` entry; every `lootCategories` string is
   accepted by the loot resolver.
7. Run `--data-integrity-selftest` (0 errors); run `--expedition-selftest` (9 vehicle gates
   + dispatch smoke); confirm the expedition panel populates in a headless boot.
8. Spot-check 5 entries: dispatch → travel ticks → encounter roll → loot → return, full
   loop, save/reload mid-expedition.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --expedition-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
LOW — data-only; the one trap is inventing `lootCategories` strings the resolver rejects
(step 1 + step 6 prevent this). Id collisions are caught by the integrity validator.

## Definition of Done
- 50 expedition entries (2 existing + 48 new), all ids resolving, all loot categories valid,
  integrity gate green, expedition selftest green, dispatch panel shows 50 destinations.

## Follow-on
- Plan 46 (scavenging tables) deepens the `lootCategories` into per-location-type tables.
- Plan 50 (micro-locations) adds 25 small discoveries that appear along travel routes.
- Plan 43 (settlements) marks which destinations are friendly trade stops vs ruins.
