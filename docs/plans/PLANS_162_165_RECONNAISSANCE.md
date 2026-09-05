# Plans 162–165 — Repository Reconnaissance (Phase A Exit Gate)

Date: 2026-09-05. Authority map produced before any production edit, per the
flagship integration plan §3. All paths relative to repo root. Live source
wins over documentation; every claim below was verified against current code.

## Purpose

Plans 162–165 add `AgricultureSystem`, `DefenseSystem`, `PsychologicalArcSystem`,
`WildlifeEcosystemSystem` as one interlocking survival layer. This document
records what already owns each domain, the composition decisions, and the
divergences between the plan text and the live repository.

## A. Plan 162 — Agriculture: existing ownership

| Domain | Existing authority | Consequence for Plan 162 |
|---|---|---|
| Soil plots, growth stages, water, soil contamination, blight | `GreenhouseSystem` (`Assets/Ashfall.Core/Greenhouse/GreenhouseSystem.cs`): `GreenhousePlotState {plotIndex, seedItemId, stage, growth 0-100, water, soilContamination, blight, plantedDay}`, `Plant/Water/Harvest/TreatBlight/TickDay(day, growLightHours, ashContaminationRate)`, save section `greenhouse` | `AgricultureSystem` LAYSERS ON plots (keyed by plotIndex); it must not re-implement growth. Day tick wraps `_greenhouse.TickDay` with derived light/contamination parameters. |
| Seed→yield mapping | `GreenhouseExpansionCatalog.CropCatalog` (13 crops, sole authority); micro-location `seed_packets` resolves through it (F18) | `crop_strains.json` strains must reference existing seed item ids; strain = advanced profile above a crop def. |
| Rack hydroponics, mutation traits, power/water callbacks | `HydroponicBiomeSystem` (`Assets/Ashfall.Core/Shelter/HydroponicBiomeSystem.cs`) + `hydroponic_crops.json` (10 cultivars), save `hydroponic_biomes` | OUT OF SCOPE. Plan 162 composes the greenhouse path only; racks keep their own authority. Documented boundary. |
| Fungi strains | `Farming/FungiCultivationSystem.cs` | Untouched. New system lives in `Core/Farming/` alongside it. |
| External environment | `WeatherSystem` (`Core/World/WeatherSystem.cs`): `GetTemperaturePenaltyCelsius()`, `OutdoorRadModifier`, `GetSeasonForDay(day)` (season = `SeasonWindowDef` string ids from `weather_seasons.json`). **No `MacroWeatherSystem` exists.** | Use WeatherSystem directly (plan §2.1 anticipated). No humidity API → humidity only from greenhouse-local state. |
| Water purity | Binary items `clean_water`/`irradiated_water` + `WaterTreatmentSystem` (`incomingContaminationLevel` 0-1, treated-water stocks). No numeric purity API. | Quality bands derived from `WaterTreatmentSystem` contamination + the tainted flag the greenhouse `Water()` call already carries. |
| Power | `PowerGridSystem` (`Core/Shelter/PowerGridSystem.cs`) — per-room model: `IsRoomPowered(roomId)`, `Snapshot()`, `IsBrownout`. No `ShelterPowerGridSystem` class. | `lightingAvailabilityPermille` derived via a `Func<bool>`/room-powered callback (HydroponicBiomeSystem ctor precedent). |
| Nutrition | `NeedsSystem` (`Core/Survivors/NeedsSystem.cs`): single `Hunger` scalar; **no diversity**. Food items carry only `hungerRestore`. `KitchenNutritionSystem.ServeMeal` logs a scalar `nutritionScore`. Eating via `InventoryHostSession.ConsumeResult` → `Needs.Modify`. | New focused `NutritionDiversitySystem` (Core): rolling diet log of category vectors (Calories/Protein/VitaminC/Micronutrients/Fats/Fiber), profiles from a new `nutrition_profiles.json` mapping existing food ids, deficiency consequences applied ONLY through `NeedsSystem.Modify` (morale/health). No changes to NeedsSystem internals. |
| Compost/waste | None exists (no `spoiled_food`/`organic_waste` item; kitchen pantry has `PantryItem.isSpoiled`). | Net-new `TryStartCompostBatch`: inputs = kitchen spoiled servings + crop matter; lossy output = new compost/fertilizer item ids. |
| Save | `greenhouse` → `greenhouse_save.json` (via `GreenhouseSaveStore`, `SaveStoreHub.FromCodec`) | New section `agriculture` → `agriculture_save.json`. |
| Collision | No `AgricultureSystem`, no `crop_strains.json` anywhere. | Clear to create. |

Day-tick seam: `GreenhouseFoundryDayOwner` (`src/Main.CampaignOwners.cs` L182-203, phase 2) currently calls `_greenhouse.TickDay(day, 6f, 0.04f)` directly. Agriculture wraps this: when active, the owner calls `_agriculture.TickDay(day)` which computes light/contamination from power+weather and ticks the greenhouse internally (exactly-once; legacy fallback preserved).

## B. Plan 163 — Defense: existing ownership

| Domain | Existing authority | Consequence for Plan 163 |
|---|---|---|
| Raid trigger/authority | `IronRaidersSystem` (`Core/Muster/IronRaidersSystem.cs`): `EvaluateRaidChance`, `ProvokeRaid/ExecuteRaid` → `OnRaidExecuted`. No `RaidResolutionSystem` exists. | Raid authority untouched. |
| Raid host seam | `Main.Muster.OnIronRaidersRaidExecuted` (`src/Main.Muster.cs` L147-166) → `CombatHostSession.StartCombat` directly. If combat active, raid silently resolves as losses. | **Pre-combat defense phase slots exactly here**: resolve defenses first; only surviving/breaching raiders escalate to `StartCombat` (reduced enemy count) or the raid is repelled. |
| Survivor combat | `TacticalCombatSystem` (`Core/Combat/`) + `CombatHostPorts` for real-state effects. | Untouched; receives the reduced raid. |
| Surface emplacements (walls/turrets/early warning) | `PerimeterDefenseSystem` (`Core/Defense/`) + `perimeter_defenses.json` + `SimulateRaiderAssault(...)` — **zero host callers today**; save `perimeter_defense`; journal wiring exists in `Main.AdvancedShelterSystems.cs`. | `DefenseSystem` COMPOSES it: emplacement contributions + assault simulation come from PerimeterDefenseSystem; DefenseSystem adds the missing trap layer, capture outcomes, raid logs, reset/repair, ammo magazines. |
| Traps (anti-raider) | None (wildlife traps are a different domain: `WildlifeTrappingSystem`). | New `defenses.json` authors the trap/capture/reset/repair layer only (plan §6.3-6.4's `TurretConfiguration`/`DefenseInstallationDefinition` already exist as `PerimeterDefenseDefinition` — do not duplicate). |
| Power | `PowerGridSystem` (`IsRoomPowered`, room priorities). `PerimeterDefenseDefinition.power_draw_watts` + `isEmplacementPowered` callback precedent. | Reuse callback pattern. |
| Ammo | `Inventory.TryConsume/TryConsumeById` atomic pattern (PerimeterDefenseSystem L109-119 precedent); ammo ids `ammo_9x19`, `ammo_556`, … in items.json. | Turret/magazine consumption via inventory transactions; defense stores only loaded-magazine state. |
| Captives | `PrisonerSystem.TakePrisoner(captiveId, sourceFactionId, day)` (`Core/Factions/PrisonerSystem.cs` L156) — **no callers** (intake unwired); interrogation/recruit/release all exist; save `prisoner_management`. | `DefenseSystem` emits `RaiderCaptured`; host handler calls `TakePrisoner` — finally wiring capture intake. No captive state inside DefenseSystem. |
| Blast door | `AirlockSecuritySystem` (`blastDoorIntegrity`). | Untouched; perimeter strength may read it as a wall contribution. |
| Save | New section `settlement_defenses` → `settlement_defenses_save.json`. | |
| Collision | No `DefenseSystem` class, no `defenses.json`. | Clear (composes, not replaces). |

## C. Plan 164 — Psychological arcs: existing ownership

| Domain | Existing authority | Consequence for Plan 164 |
|---|---|---|
| Canonical stress | `NeedsSystem.Morale` (0-100, higher = worse) is the only canonical stress-like value. `ISurvivorConditionPort.GetAcuteStressPermille` (`Core/Sanatorium/PsychologicalSanatoriumSystem.cs` L22-44) is the INTENDED canonical acute-stress surface — **unimplemented**. | `PsychologicalArcSystem` implements the port: permille = f(Morale) + arc progression. |
| Trauma | `CombatTraumaSystem` (`Core/Survivors/`) — hypervigilance authority, surfaced via `CombatTraumaAfflictionHandler`. | Read-only input to arc eligibility. |
| Therapy authority | `PsychologicalSanatoriumSystem` (`Core/Sanatorium/`) EXISTS but is host-unwired (no section, no Main wiring). `MentalHealthCrisisSystem` (wired, `mental_health_crisis`) handles acute crisis cases separately. | Plan §7.14 preferred branch applies: wire the sanatorium as THE arc-treatment authority; arc system reports arcs/stages. `MentalHealthCrisisSystem` stays as-is (acute layer). |
| Affliction pipeline | `MedicalPipelineCoordinator` + `IAfflictionHandler` observer pattern (`Core/Medical/PsychologyAfflictionHandlers.cs`). | Arc conditions surface as observer handlers if needed; arc state itself stays in the arc system. |
| Relations/conflict | `SurvivorRelationsSystem` (`ModifyAffinity/Trust`, conflicts, `Mediate`). | Persecutory-arc relationship effects route through it. |
| Assignment | `ShelterAssignmentSystem` (`Assign/Unassign/CanAssign`), `InstitutionAssignmentLedger` (one claim per survivor), `UtilityAiSystem`. `MentalHealthCrisisSystem.IsEligibleForWork` is precedent for a work gate. | Refusal/isolation = `Unassign` + ledger release via host callbacks; no parallel assignment state. |
| Fire incidents | `ShelterFireHazardSystem.Ignite(incidentId, sourceZoneId, day, zones)` (`Core/Shelter/`). | Fire-fixation arc emits `UnsafeFireIncidentRequested`; host calls `Ignite`. Damage owned by fire system. |
| Private stashes | None exists (global `Inventory` only; narrative contraband has authored stash strings). | New minimal `PrivateStashState` under arc state; transfers via `Inventory.TryConsume` (conservation preserved). |
| Traits | Plain strings on `SurvivorDefinition.traitIds`. | Protective-trait checks = `traitIds.Contains(...)` guards, data-driven from arc definitions. |
| Save | `survivors` section persists `morale` only; trauma in `combat`; crisis in `mental_health_crisis`. | New section `psychological_arcs` → `psychological_arcs_save.json`; sanatorium wiring adds section `psychological_sanatorium` → `psychological_sanatorium_save.json`. |
| Collision | No `PsychologicalArcSystem`, no `mental_arcs.json`. Avoid ids `psychological_sanatorium`/`condition_*`/`therapy_*`/`affliction_*` (owned). | Clear; arc ids `arc_*` (new prefix registration needed in `CatalogIntegrityValidator.IdPrefixes`). |

## D. Plan 165 — Wildlife ecosystem: existing ownership

| Domain | Existing authority | Consequence for Plan 165 |
|---|---|---|
| Population store | `WildlifeMigrationSystem` (`WildlifeMigrationSystem.cs` + `.Live.cs`): packs `{packId, speciesId, currentSectorId, population, starvationLevel, …}`, sector adjacency (`SetSectorAdjacency` from `world_evolution_seeds.json`, 11 sectors), hunger migration, birth recovery, `ApplyHarvestPressure(sector, amount)` (keeps remnant pair), `GetGlobalPopulationRatio()`. Persisted inside the `world` section (`WorldHostSave.Wildlife`). | **Population authority stays here** (plan §8.3 mandatory branch). Ecosystem derives indices from packs and mutates populations ONLY through migration-system APIs (add `AdjustPackPopulation` if needed). No second population store. |
| Trapping | `WildlifeTrappingSystem` + catalog (15 prey, `PreyDefinition.migrationSpeciesId` bridge to `species_*`): downstream consumer via `densityMultiplier` + `SetSelectionContext`; reports pressure back via `ApplyHarvestPressure` (`Main.ShelterSocial.cs` L229-235). Keeps NO abundance state (divergence risk LOW). | Host density multiplier becomes ecosystem-derived per-sector density; feedback observed by ecosystem for per-species pressure. |
| Species semantics | 12 `species_*` archetypes HARDCODED in `WildlifeSeasonalCalendar.ArchetypeOf`. No species JSON. | New catalog `wildlife_ecosystem.json` (NOT `bestiary.json` — the narrative `narrative/wasteland_wildlife_bestiary.json` already owns bestiary lore; do not duplicate) data-drives the 12 species + ecology params, aligned with existing species ids. |
| Seasons | `WeatherSystem.GetSeasonForDay` (string window ids). No SeasonSystem. | Reuse string window ids. |
| Radiation | Global `WeatherSystem.OutdoorRadModifier`; per-location `baseRadsPerHour`; `FalloutSystem.GetZoneRadiationRate`. No per-sector ambient. | Species radiation tolerance reacts to weather modifier + location contamination; no new ambient field. |
| Expeditions | `ExpeditionSystem.SetEncounterChanceMultiplier(locationId)`, `OnEncounterTriggered` → `EnemyCompositionSelector.SelectWildlifeComposition`; `ExpeditionEncounterBridge.Surface` weighted pools; `TravelEncounterCombatBinder` combatant tags (`apex`, `pack_canine`, …). | Ecosystem exposes `WildlifeThreatSnapshot`/density queries; host wires the multiplier; encounters still resolved by the existing encounter/combat authorities. |
| Harvest drops | `ScavengingTableCatalog.RollLoot` (generic weighted tables, `scavenging_tables.json`). | Fauna harvest tables = new table entries consumed via `RollLoot`; no drop lists in ecosystem code. |
| Taming/livestock | None (WarDogKennelPanel is cosmetic prototype; NurseryPanel is childcare). | New focused `DomesticAnimalState` inside the ecosystem system; wild population decremented via migration API on transfer. |
| Save | New section `wildlife_ecosystem` → `wildlife_ecosystem_save.json` (pressure/extinction/apex/taming/bestiary-knowledge state). Pack populations stay in `world`. | |
| Collision | No `WildlifeEcosystemSystem`. | Clear. |

## E. Shared infrastructure (registration recipes verified)

- **Clock/day ticks:** `IDayAdvanceOwner` nested classes registered in `RegisterProductionCampaignOwners()` (`src/Main.CampaignOwners.cs`), phases 1-5 order execution. Double-advance guard via `lastTickDay` required (CLOCK_POLICY.md). Agriculture: phase 2 (wraps greenhouse owner). Psychology: after needs finalization. Wildlife ecology: phase 4 (alongside `EvolvingWorldDayOwner`). Defense: event-driven (raid seam) + daily passive tick.
- **RNG:** `CampaignRngStream`/`CampaignRngManager` (`Core/Random/CampaignRngStream.cs`): `Fork(streamId, day, actionIndex)` derives isolated streams via `StableHash.Of(streamId)`; positions captured/restored in `campaign_day` section. NEW stream consts to add (plan §2.4): `agriculture.mutation`, `agriculture.pest`, `agriculture.blight`, `defense.targeting`, `defense.capture`, `defense.damage`, `psychology.arc_trigger`, `psychology.arc_behavior`, `psychology.recovery`, `wildlife.population`, `wildlife.migration`, `wildlife.apex`, `wildlife.taming`. Adding streams cannot shift existing streams.
- **Inventory:** `Inventory.AddById/RemoveById/TryConsume/TryProduce/CountById` (`Core/Inventory/Inventory.cs`); `ItemCatalog` resolves ids; `ItemAliases.ToCanonical` normalization automatic.
- **Save section triad (per system):** (1) `SaveSectionRegistry.All` entry + `SectionFileNames` mapping; (2) `src/Host/XxxSaveStore.cs` thin `SaveStoreHub.FromCodec` facade (copy `PrisonerSaveStore`); (3) Main partial `SetupXxx/SaveXxx` + hooks in `Main.SaveOrchestrator` (`SaveAll` + `RestoreAllSubsystemsFromDisk`). `CaptureSection(key, TryCapturePersisted(state))`.
- **Catalog integrity:** `CatalogIntegrityValidator.Validate` auto-scans ALL json under `Assets/StreamingAssets/Data/` (TopDirectoryOnly). New id prefixes must be added to `IdPrefixes` (e.g. `arc_`, `pest_`, `defense_` as needed); reference keys to `ReferenceKeys`. All catalogs need top-level `schema_version`.
- **Content utilization:** add consumer-map entries in `ContentUtilizationScanner` (`["<file>.json"] = new[]{ "<LoaderClass>", "<SystemClass>" }`) so files classify `GAMEPLAY_CONSUMED`; regenerate `artifacts/content-utilization-baseline.json` via `--content-utilization-selftest`.
- **Selftests:** HostCliAction enum + `Parse` + `HostCliRegistry` descriptor (Core authority) + `HostCli.*.cs` implementation + `Main.Application.cs` dispatch case.
- **Bound panel end-to-end (KineticStorage model):** Core system → `src/Host/XxxHostSession.cs` (`HostSessionBase`, `LastEvent`, dirty `Save()`) + `XxxSaveStore.cs` → `src/UI/XxxPanel.cs` (Bind/RefreshView/OnActionRequested/Open/Unbind, `AshfallDashboardShell`/`AshfallSidebar`/`AshfallStatusRail`/`AshfallDataGrid`/`AshfallUiHelpers`) → Main partial fields + guarded `SetupXxx()` → construction + subscription in `Main.UiPanels.cs` → `HandleXxxAction` (OPEN bind-guard) → `PanelRegistryBootstrap.R(id, …, maturity: Live)` + `ConfigureActions` in `Main.PlayerSurfaces.cs`. Panels must satisfy the UI audit standard (state → blocker → cost → consequence, real commands, truthful empty states).
- **Determinism:** no `System.Random`/`Guid.NewGuid`/`DateTime.Now` anywhere in Core/src (gated by `DeterminismGuardTests`); randomness only via injected `ISeededRng`/`Fork`.

## F. Documented divergences from the plan text (all minor/material-with-precedent)

1. **`MacroWeatherSystem` does not exist** → WeatherSystem is the external climate input (plan §2.1/§9.1 anticipated this with a conditional).
2. **`RaidResolutionSystem` does not exist** → raid authority is `IronRaidersSystem`; the pre-combat defense phase slots into `Main.Muster.OnIronRaidersRaidExecuted` (plan §6.7's ordering implemented at the live seam).
3. **`PerimeterDefenseSystem` already owns emplacements/walls/turrets** (plan §6.2's conditional "if no other system owns walls") → `DefenseSystem` composes it; `defenses.json` authors only the missing trap/capture layer.
4. **`ShelterPowerGridSystem` is actually `PowerGridSystem`** (room model).
5. **`PsychologicalSanatoriumSystem` exists but is host-unwired** → Plan 164 wires it as the single therapy authority (plan §7.14's preferred branch).
6. **`WildlifeMigrationSystem` is the existing population authority** → ecosystem extends it upstream (plan §8.3's mandatory branch); catalog named `wildlife_ecosystem.json`, not `bestiary.json`, to avoid duplicating the narrative bestiary.
7. **No canonical `stress` field** → `NeedsSystem.Morale` is the stress proxy; acute stress surfaces through the previously-empty `ISurvivorConditionPort`.
8. **Water purity is binary items + `WaterTreatmentSystem` floats** → quality bands are derived, no new purity scale.
9. **Repo plan-numbering mismatch:** the repo's own `Next-steps-plans/Plan_162` is an unrelated "Shelter History & Archive" doc and host code already reaches `Plans178_181`; the pasted flagship spec's Plans 162-165 are EXTERNAL numbering. Implementation uses system names (AgricultureSystem etc.); the `Plans162To165` label applies only to the shared test/replay harness names, matching the pasted spec.

## G. Baseline gates

Recorded in `PLANS_162_165_IMPLEMENTATION_LOG.md` (Phase A section) — see log for exact output. Baseline reflects concurrent-stream work-in-progress in the tree; failures predating this task are not misattributed.
