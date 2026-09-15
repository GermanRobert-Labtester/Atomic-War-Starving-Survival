# Plans 122–125 — Late-Tech Mobility Authority Map (MASTER)

**Status:** Phase 1 ACCEPTED (reconnaissance). Premise-verified 2026-09-13 against current
source; no implementation before every cross-domain owner below is resolved.
**Baseline:** `dotnet build Ashfall.csproj` 0 warnings / 0 errors.

This plan text was drafted from a historical task list. Premise verification found
**8 of 11 cited system names do not exist**; real owners were located by concern. All
owner rows below cite current files. Plan-text names that were wrong are recorded in
§3 so builders do not chase ghosts.

---

## 1. Concern owner matrix (required by the plan)

| Concern | Owner (verified) | Read API | Write API | Save owner | Tick owner | Existing tests |
|---|---|---|---|---|---|---|
| Fuel gas | **No biogas/syngas authority exists.** `PowerGridSystem.AddFuel(float)` (`Shelter/PowerGridSystem.cs:324`) is the grid fuel writer; hosts feed it from inventory. `bio_fermentation_catalog.json` produces only preservation/acid/industrial outputs — **no fuel-gas process exists**. | `PowerGridSystem.FuelUnits` | `AddFuel(units)` | `power_grid` | shelter power cadence | Plans74-77 host wiring tests |
| Power generation | `Assets/Ashfall.Core/Shelter/PowerGridSystem.cs` | `GenerationWatts`, `GenerationContributions`, `NetWatts`, `IsBrownout`, `Snapshot()` | `SetGenerationContribution(sourceId, watts)` (:87), `RemoveGenerationContribution` | `power_grid` (`PowerGridSave.cs`) | shelter power cadence | Plans74-77 host wiring; triad gate |
| Waste heat | `Assets/Ashfall.Core/ShelterThermalSystem.cs` | `GetAuxiliaryHeat(roomId)` (:436), room temps | `AddAuxiliaryHeat(roomId, heatKw)` (:427), `SetGeneratorWasteHeat(generatorKw, pumpActive)` (:961) | thermal section (existing) | shelter thermal cadence | thermal suites |
| Hostile-fire events | **No per-strike event stream exists.** `YearOfAsh/FactionWarSystem.cs:201` increments `totalArtilleryStrikesLogged` (day 241+, every 15d) with `OnTerritorialClashOccurred`; `YearOfAshTimelineSystem` counts `artilleryBarragesExperienced`. Both are counters, not observations. | counters only | extension decision needed (§4) | `faction_war` / year-of-ash sections | campaign day | FactionWar suites |
| World threat intel | `Assets/Ashfall.Core/World/WastelandMapSystem.cs` | `Markers`, `GetNodeIntel`, fog/knowledge | `UpsertTrapMarker` (:117) precedent for new marker types | wasteland map section | campaign day | map suites |
| Tool wear | `Assets/Ashfall.Core/EquipmentConditionSystem.cs` + `Inventory/IEquipmentConditionSink.cs` | condition read per item | sink interface | equipment-condition section | workshop/equipment cadence | equipment condition suites |
| Excavation cutter life | `Assets/Ashfall.Core/ExcavationSystem.cs` (`requiredTools` per layer, :27) + `Excavation/ExcavationHazardSystem.cs` | tool gate per layer | tool consumption via equipment sink | excavation section | excavation cadence | excavation suites |
| Vehicle upgrades | `Expeditions/VehicleGarageSystem.cs` (`VehicleCustomizationRecord.installedSlots`, :12) + module precedent `ArmoredCrawlerModuleCatalog.cs` (`CrawlerModuleDefinition`: mass/powerDraw/fuelModifier/cargoModifier/tags, :11–38) | garage records | install/uninstall via garage | vehicle garage section | expedition cadence | crawler module suites |
| Water crossings | `WastelandMapSystem.cs` routes: `TravelDomain` "land"/"water" (:850), water current strength (:853), contamination (:856) — **topology-owned already** | route domain/current | route/marker state via map system | wasteland map section | campaign day | map suites |

---

## 2. Shared infrastructure (all four plans)

| Concern | Owner | Evidence |
|---|---|---|
| Seeded RNG | `Assets/Ashfall.Core/Random/CampaignRngStream.cs` + `CampaignStreamIds` (Weather/Combat/Expedition/Shelter/Foundry…) | Plan 138 fork precedent: new streams fork from a parent key so existing streams cannot shift. No `System.Random` in Core. |
| Save registry | `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs` (`All` rows :54, `SectionFileNames` :257, `SchemaVersions` :447) | New sections register a row + schema version + filename; section-count test must be updated deliberately (171→177→180 precedent). |
| Catalog integrity | `CatalogIntegrityValidator.cs` + `CatalogIntegrityRules.cs` | New catalogs register rules; presence in JSON is not reachability. |
| Content utilization | `Content/ContentUtilizationScanner.cs` | Every authored output needs a live consumer claim. |
| Selftest registry | `Assets/Ashfall.Core/HostCliRegistry.cs` | New `--*-selftest` verbs register here; headless only. |
| Panel routes | `PanelRegistryBootstrap` + `Main.PlayerSurfaces` expandedIds + `OpenExpandedPanel` switch (parity is gate-enforced: `PanelRouteGateTests`). | Plan 138 panel-reachability debt taught this the hard way. |
| Weather/environment | `World/WeatherSystem.cs`, `WeatherGate*` family, `IWeatherSeverityProvider.cs` | Input source for Plans 122 (thermal) and 123 (acoustic confidence). |

---

## 3. Premise drift: plan-text names vs. verified reality

| Plan-text name | Reality | Consequence |
|---|---|---|
| `ShelterPowerGridSystem.cs` | `Shelter/PowerGridSystem.cs` | Use real name |
| `BiogasDigesterSystem.cs` | **Does not exist.** No fuel-gas authority anywhere. | Plan 122 fuel comes through `AddFuel` from items; a biogas authority is out of scope unless foreman opens it |
| `RefractoryCeramicsEngine.cs` | `Narrative/CeramicsKilnCatalog.cs`, `Narrative/CrucibleFoundryCatalog.cs` (catalogs; engines live in kiln/foundry systems) | Precursor chain routes through kiln/foundry owners |
| `AcousticDirectionFindingEngine.cs` | **`Radio/AcousticDirectionFindingCatalog.cs` exists but is dormant: zero consumers, no JSON feeds it** (136 lines, referenced only by itself). Live adjacent authority is `Radio/SignalTriangulationSystem.cs` + `DirectionFindingCatalog.cs` + `acoustic_triangulation_catalog.json` (radio direction finding — different domain). | Plan 123 builds on `SignalTriangulationSystem` concepts and the FactionWar/wasteland seam; the dormant catalog is a quarantine candidate, NOT an extension target |
| `DeepExcavationSystem.cs` | `ExcavationSystem.cs` | Use real name |
| `PrecisionMetrologyEngine.cs` | `Shelter/PrecisionMetrologySystem.cs` | Use real name |
| `ExpeditionVehicleLogistics.cs` | Root `Assets/Ashfall.Core/ExpeditionVehicleSystem.cs` + `Expeditions/VehicleGarageSystem.cs` | Use real owners |
| `TerrainTopologyCatalog.cs` | `WastelandMapSystem.cs` routes (`TravelDomain`, current, contamination) | Topology is map-owned; no separate terrain catalog |
| `WorkshopPanel.cs` | `src/UI/WorkshopPanel.cs` (Godot) | UI layer |
| `TacticalCombatSystem.cs` | `Combat/TacticalCombatSystem.*.cs` (partial) | Use real files |
| `WastelandMapSystem.cs` / `ExpeditionSystem.cs` | Exist as named | OK |
| `items.json` / `recipes.json` | Exist as named | OK |

---

## 4. Cross-domain decisions required before implementation (exit gate)

1. **Plan 123 hostile-fire producer.** No system emits per-strike hostile-fire events with
   location/uncertainty. Options: (a) extend `FactionWarSystem` to emit typed strike
   events; (b) define the `HostileFireObservation` input seam in the new engine and let
   Phase 7 wire a producer. Default: (b) — engine owns the input type; emitter wiring is
   a separate decision recorded before Phase 7.
2. **Plan 122 fuel-gas authority.** No biogas owner exists. SOFC fuel profile reads grid
   fuel units via the existing `AddFuel`/contribution seam; a fuel-*quality* state lives
   in the SOFC save section (derived from consumed item IDs), never a second inventory.
3. **`AcousticSignatureRating` consumer.** No hostile acoustic-detection system is live
   (the ADF catalog is dormant). SOFC exposes the rating; consumer wiring is deferred and
   recorded in the map rather than invented.
4. **Skills.** None of the plan's proposed traits exist (`development_traits.json` has 7
   traits, skill-modifier keyed: engineering, scavenging, stealth, etc.). All four plans
   use existing canonical skill keys (engineering, mechanics, scavenging, expedition
   skills); no new traits without a foreman decision.
5. **`SetGeneratorWasteHeat` is currently unfed (zero callers)** — the CHP seam exists but
   nothing produces into it. SOFC waste heat routes through `AddAuxiliaryHeat` per-room
   and/or `SetGeneratorWasteHeat`; no double-count because no other producer exists yet.

---

## 5. Sequencing (adopted from plan §17, adjusted to evidence)

Phase 2 catalogs → Phase 3 SOFC Core → Phase 4 diamond → Phase 5 sound ranging →
Phase 6 amphibious → Phase 7 cross-wiring → Phase 8 persistence → Phase 9 host/UI →
Phase 10–12 harness/soak/gates. Each plan's detailed map lives beside its domain:
`docs/shelter/PLAN_122_SOFC_AUTHORITY_MAP.md`,
`docs/combat/PLAN_123_SOUND_RANGING_AUTHORITY_MAP.md`,
`docs/shelter/PLAN_124_DIAMOND_AUTHORITY_MAP.md`,
`docs/expeditions/PLAN_125_AMPHIBIOUS_AUTHORITY_MAP.md`.
