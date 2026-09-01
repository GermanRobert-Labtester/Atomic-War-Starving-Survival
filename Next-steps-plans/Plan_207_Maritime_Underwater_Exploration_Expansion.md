# Plan 207 — Maritime & Underwater Exploration Expansion

## Goal

Expand the maritime and underwater exploration system from a single dive mini-game into a comprehensive maritime domain with multiple dive sites, underwater scavenging routes, maritime hazards, sunken vessel exploration, and coastal zone management. Currently `MaritimeDiveSystem.cs` handles a single dive mini-game (4 rooms, noise/air management), and `SafeCrackingSystem.cs` handles safe cracking — but there is no broader maritime exploration system, no dive site catalog, no underwater scavenging routes, no maritime hazards, no sunken vessel exploration, no coastal zone management. The maritime domain is a single mini-game, not a gameplay layer. This plan expands it into a full exploration domain.

## Why

**Repository evidence:** `MaritimeDiveSystem.cs` exists as a self-contained diving mini-game (4 rooms, noise/air management, loot). `SafeCrackingSystem.cs` (532 lines) handles safe cracking. `dive_sites.json` exists as data. But no broader maritime system — no dive site management, no underwater routes, no maritime hazards, no sunken vessels, no coastal zones, no maritime expeditions. The maritime domain is a mini-game, not a gameplay layer.

**What is missing:** No dive site catalog/management. No underwater scavenging routes. No maritime hazard system (currents, pressure, contamination). No sunken vessel exploration. No coastal zone management. No maritime expedition planning. No underwater resource deposits. No diving equipment progression. The maritime domain has one mini-game, not a system.

**Why existing plans don't solve it:** No plan addresses maritime expansion. Plan 101 (expedition vehicles) covers surface vehicles. Plan 160 (expedition colonies) covers land settlements. Plan 133 (expedition consequences) covers discovery effects. No plan expands the maritime domain.

**Player value:** Creates exploration depth (multiple dive sites with different challenges), adds resource variety (underwater scavenging yields unique items), generates emergent stories (maritime hazards, sunken vessel discoveries), and makes the coastal/water domain a meaningful gameplay area.

## Files / Systems to Inspect

- `Assets/Ashfall.Core/Maritime/MaritimeDiveSystem.cs` — existing dive mini-game
- `Assets/Ashfall.Core/Maritime/SafeCrackingSystem.cs` — safe cracking
- `Assets/StreamingAssets/Data/dive_sites.json` — dive site data
- `Assets/Ashfall.Core/ExpeditionSystem.cs` — expedition framework
- NEW: `Assets/Ashfall.Core/Maritime/MaritimeExplorationSystem.cs`
- NEW: `Assets/StreamingAssets/Data/maritime_zones.json`

## Main Task 1 — Foundation / System Contract

1. Create `MaritimeExplorationSystem.cs` in `Assets/Ashfall.Core/Maritime/`
2. Define `DiveSite` DTO: `siteId`, `siteName`, `siteType` (coastal_shallows/deep_ocean/sunken_vessel/underwater_cave/contaminated_zone/flooded_bunker), `depth` (meters), `hazardLevel` (0-100), `lootTable` (list of item_ids with weights), `discoveryStatus` (undiscovered/discovered/explored/fully_salvaged), `discoveredDay`, `lastExploredDay`, `explorationCount`, `coordinates` (location reference)
3. Define `MaritimeZone` DTO: `zoneId`, `zoneName`, `zoneType` (coastal/estuary/open_ocean/deep_trench/underwater_ridge), `waterTemp` (celsius), `radiationLevel` (0-100), `currentStrength` (0-100), `visibility` (0-100), `diveSites` (list of site_ids), `accessRequirement` (equipment/skill needed)
4. Define `MaritimeExpedition` DTO: `expeditionId`, `targetSiteId`, `assignedDivers` (list of survivor_ids), `equipment` (list of equipment_ids), `plannedDay`, `status` (planned/in_progress/completed/failed/aborted), `discoveredLoot` (list of item_ids), `encounteredHazards` (list of hazard events), `duration` (hours)
5. Define `MaritimeHazard` DTO: `hazardId`, `hazardType` (strong_current/underwater_collapse/radiation_hotspot/entrapment/pressure_depth/contaminated_water/marine_creature/equipment_failure), `severity` (0-100), `affectedDivers` (list of survivor_ids), `outcome` (avoided/minor_injury/major_injury/fatal/equipment_lost), `day`
6. Define `DivingEquipment` DTO: `equipmentId`, `equipmentType` (basic_dive_suit/deep_dive_suit/hazmat_dive_suit/rebreather/underwater_light/sonar/cutting_torch/salvage_bag), `condition` (0-100), `depthRating` (max depth in meters), `protectionLevel` (0-100), `owner` (survivor_id or shelter)
7. Define `MaritimeExplorationState` DTO: list of discovered dive sites, list of maritime zones, list of expeditions, list of hazards encountered, list of diving equipment, exploration settings (discovery rate modifier, hazard frequency modifier)
8. Implement `CaptureState/RestoreState` with schema versioning
9. Define dive site types (6+ types):
   - **Coastal Shallows**: easy dives, low hazard, common loot
   - **Deep Ocean**: hard dives, high hazard, rare loot
   - **Sunken Vessel**: medium difficulty, structured exploration, unique loot
   - **Underwater Cave**: navigation challenge, entrapment risk, hidden loot
   - **Contaminated Zone**: radiation hazard, requires hazmat gear, valuable materials
   - **Flooded Bunker**: pre-war structure, safe-cracking integration, high-value loot
10. Define maritime zone mechanics:
    - Zones have water temperature, radiation, current, visibility
    - Zones contain multiple dive sites
    - Zones require specific equipment to access
    - Zones can be explored progressively
    - Zone conditions change over time (seasons, radiation)
11. Define maritime expedition mechanics:
    - Player plans expedition to dive site
    - Assign divers (survivors with diving skill)
    - Assign equipment (suits, rebreathers, tools)
    - Expedition takes time (travel + dive + surface)
    - Expedition encounters hazards (random + site-specific)
    - Expedition returns with loot
12. Define maritime hazard mechanics:
    - Hazards triggered by dive conditions (depth, current, radiation)
    - Hazard severity based on equipment + diver skill
    - Hazard outcomes: avoided, minor injury, major injury, fatal, equipment lost
    - Hazards logged
13. Define diving equipment:
    - Equipment has condition, depth rating, protection level
    - Equipment degrades with use
    - Equipment can be repaired/replaced
    - Different equipment for different depths/hazards
14. Define underwater loot:
    - Dive sites have loot tables (weighted item lists)
    - Rare items only in specific sites
    - Sunken vessels have unique loot
    - Flooded bunkers integrate with safe-cracking
    - Loot logged
15. Add deterministic seeding: maritime events use `ISeededRng`
16. Wire into `GameBootstrap`: `SetupMaritimeExploration`, `TickMaritimeExploration`, `SaveMaritimeExploration`

## Main Task 2 — Implementation / Sites / Zones / Expeditions / Hazards / Equipment / UI

1. Implement dive site discovery:
   - Sites discovered through expeditions, intelligence, events
   - Sites have type, depth, hazard level, loot table
   - Sites can be explored repeatedly
   - Sites become fully salvaged after enough exploration
   - Discovery logged
2. Implement maritime zones:
   - Zones contain multiple dive sites
   - Zones have environmental conditions
   - Zones require specific equipment
   - Zone conditions change over time
   - Zones logged
3. Implement maritime expeditions:
   - Player plans expedition (target, divers, equipment)
   - Expedition takes time
   - Expedition encounters hazards
   - Expedition returns with loot
   - Expedition logged
4. Implement maritime hazards:
   - Hazards triggered by conditions
   - Hazard severity based on equipment/skill
   - Hazard outcomes (injury, death, equipment loss)
   - Hazards logged
5. Implement diving equipment:
   - Equipment has condition, depth rating, protection
   - Equipment degrades with use
   - Equipment repairable
   - Equipment logged
6. Implement underwater loot:
   - Loot tables per dive site
   - Rare items in specific sites
   - Sunken vessel unique loot
   - Flooded bunker + safe-cracking integration
   - Loot logged
7. Implement maritime UI:
   - Maritime map: zones, dive sites, expedition status
   - Dive site detail: type, depth, hazard, loot, exploration status
   - Expedition panel: plan/manage expeditions
   - Equipment panel: diving equipment condition
   - Hazard log: encountered hazards
   - Loot log: recovered items
8. Create maritime events:
    - "The Discovery" — new dive site found
    - "The Dive" — expedition launched
    - "The Hazard" — maritime hazard encountered
    - "The Salvage" — loot recovered
    - "The Loss" — diver injured/killed
    - "The Wreck" — sunken vessel explored
    - "The Deep" — deep ocean dive
    - "The Return" — expedition returned
9. Add maritime quest hooks:
    - "The Diver" — complete 20 dive expeditions
    - "The Explorer" — discover 10 dive sites
    - "The Salvager" — recover 50 items from underwater
    - "The Deep Diver" — explore 5 deep ocean sites
    - "The Wreck Hunter" — explore 3 sunken vessels
    - "The Survivor" — survive 10 maritime hazards
    - "The Treasure Hunter" — find 5 rare underwater items
10. Implement maritime tutorial: first dive expedition explains system
11. Add maritime tooltips: hover over site/zone shows details
12. Create dive site definitions in data file (15+ sites)
13. Create maritime zone definitions in data file (5+ zones)
14. Implement maritime persistence: sites/zones/expeditions saved
15. Integrate with `MaritimeDiveSystem`: existing dive mini-game as core dive mechanic

## Main Task 3 — Integration / Consequences / Validation

1. Wire into `MaritimeDiveSystem`: existing dive as core mechanic
2. Connect to `SafeCrackingSystem`: flooded bunker integration
3. Integrate with `ExpeditionSystem`: expedition framework
4. Connect to `EquipmentConditionSystem`: diving equipment condition
5. Wire into `RadiationSystem`: underwater radiation hazards
6. Connect to `CombatTraumaSystem`: diving injuries
7. Implement old-save compatibility: existing saves get no discovered sites
8. Add deterministic seeding: maritime events use `ISeededRng`
9. Create exploit prevention: dive sites are finite, can't be gamed
10. Add tests: dive sites, zones, expeditions, hazards, equipment, loot, save round-trip
11. Verify all dive site types work correctly
12. Test edge cases: no sites (current behavior), many sites (extensive exploration)
13. Verify headless behavior: maritime processes correctly without UI
14. Add data-integrity-selftest: maritime validates against location/item catalogs
15. Create `--maritime-exploration-selftest` verb for CI validation

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --maritime-exploration-selftest
```

## Risk

**LOW** — Maritime exploration is straightforward with clear inputs (dive sites, expeditions) and outputs (loot, hazards). Risk of diving feeling repetitive. Mitigation: make each site unique, vary hazards, show clear progression, and ensure rare loot rewards deep exploration.

## Definition of Done

- `MaritimeExplorationSystem.cs` exists with full `CaptureState/RestoreState`
- 6+ dive site types (coastal, deep ocean, sunken vessel, cave, contaminated, flooded bunker)
- 5+ maritime zones with environmental conditions
- Maritime expedition mechanics (planning, divers, equipment, duration)
- Maritime hazard system (currents, collapse, radiation, entrapment, creatures)
- Diving equipment (suits, rebreathers, tools, condition, depth rating)
- Underwater loot tables per site
- Maritime events and quest hooks
- Save/load round-trip tested
- Deterministic maritime events verified
- Old saves load with no discovered sites
- Dive site and zone definitions in data authority
- UI maritime map, dive site detail, expedition panel, equipment panel, hazard log, loot log
- Cross-system integration (dive system, safe cracking, expedition, equipment, radiation, trauma)

## Follow-On Opportunities

- Maritime specialization (survivors become expert divers/salvagers)
- Maritime legacy (famous wrecks discovered)
- Maritime quests (specific exploration goals)
- Maritime events (underwater discovery, massive salvage operation)
- Maritime trading (trade underwater salvage with other settlements)
