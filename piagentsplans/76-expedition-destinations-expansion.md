# Plan 76 — Expedition Destinations Expansion (2 → 15 destinations)

## Goal (2 lines)
Expand `expeditions.json` from 2 verified expedition destinations to 15. The
`ExpeditionCatalogLoader` loads destinations with distance, danger, encounter chance,
stamina drain, and loot categories. 2 destinations means the expedition system — the
core surface-exploration loop — has almost nowhere to send the player.

## Why (P2)
- Verified: `expeditions.json` has 2 entries (id, displayName, distanceTicks,
  dangerLevel, encounterChancePerTick, baseStaminaDrainPerHour, lootCategories).
  `ExpeditionCatalogLoader.cs` is fully implemented and confirmed in Core. The
  expedition system is the primary exploration loop but has almost no destinations.
- Existing 11 (world exploration) and Plan 32 (expedition wiring) both assumed a
  populated destination catalog; this plan fills it.
- Pure DATA work — zero new Core code.

## Files to touch
- `Assets/StreamingAssets/Data/expeditions.json` (expand 2 → 15 destinations)
- Read-only: `Assets/Ashfall.Core/Expeditions/ExpeditionCatalogLoader.cs` (confirm
  schema fields and how lootCategories resolve)
- `Assets/StreamingAssets/Data/items.json` (loot category ids must resolve as items)

## Content grammar (per destination)
- snake_case `id` with prefix `loc_` (confirmed prefix in existing entries).
- Distinct identity: each destination is a named place with a reason to go there
  (scavenge, rescue, recon, trade, investigate). No generic `loc_area_01`.
- Danger scaling: dangerLevel 1–8, distanceTicks 3–14, encounterChancePerTick 0.05–0.25.
- lootCategories: 3–6 item ids per destination, logically matched to the location
  type (hospital → medicine, rail yard → mechanical parts, military → ammo).
- Grounded tone: pre-war civilian, industrial, military, scientific, or wilderness
  sites — no fantasy, no supernatural.

## Steps
1. Read `ExpeditionCatalogLoader.cs` to confirm the full schema and how
   lootCategories are resolved (item ids vs category tags).
2. Read `items.json` to confirm which loot item ids exist; note gaps for step 6.
3. Author 13 new destinations across 5 families:
   - Urban (3): abandoned hospital, metro station, shopping center.
   - Industrial (3): chemical plant, rail yard, power substation.
   - Military (2): ammunition depot, abandoned checkpoint.
   - Scientific (2): weather station, geological survey site.
   - Wilderness (3): irradiated forest edge, frozen wetland, burned woodland.
4. Each destination: unique displayName, distanceTicks, dangerLevel,
   encounterChancePerTick, baseStaminaDrainPerHour, and 3–6 lootCategories.
5. Cross-reference: every loot category id resolves in `items.json`; every `loc_`
   id is unique within the file.
6. Add any missing loot item ids to `items.json` (e.g. `copper_wire`,
   `surgical_tools`, `rail_spike`) — only if a destination's loot list requires
   an item that does not exist.
7. Wire 3 destinations into Plan 49 micro-location encounters (hospital, metro,
   checkpoint produce micro-location discoveries on approach).
8. Wire 2 destinations into Plan 48 weather gates (frozen wetland and irradiated
   forest are inaccessible during blizzard/fallout storm).
9. Validate: `--data-integrity-selftest` (all ids resolve); `--expedition-selftest`
   (expedition system loads and dispatches to new destinations).
10. xUnit: expedition catalog loads 15 destinations, all ids unique, all
    lootCategories resolve, distanceTicks and dangerLevel within valid ranges.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --expedition-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
LOW — pure data. The one trap is lootCategories resolution (step 1): confirm whether
they are item ids or category tags before authoring.

## Definition of Done
- `expeditions.json` has 15 destinations, all ids resolving, 3 wired to micro-location
  encounters, 2 wired to weather gates, expedition selftest green, integrity + tests
  green.

## Follow-on
- Plan 49 (micro-locations) — destinations produce approach discoveries.
- Plan 48 (weather gates) — weather gates block 2 destinations.
- Plan 46 (scavenging tables) — destination loot lists link to scavenging tables.
- Plan 58 (narrative encounters) — destinations host location-specific encounters.
- Plan 32 (expedition wiring) — this plan provides the destination data it assumed.
