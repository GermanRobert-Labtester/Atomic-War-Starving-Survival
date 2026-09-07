# Plans B86–B89 Authority Map (Reconnaissance Exit Gate)

**Scope:** Plan B86 (Expedition Combat Breaching) · Plan B87 (Closed-Loop Aquaponics) · Plan B88 (HF/DF Direction Finding) · Plan B89 (Precision Metrology)
**Status:** Reconnaissance complete — production implementation authorized after integration plan approval
**Date:** 2026-09-06 · **Branch:** `feat/asset-pipeline-flagship`
**Method:** Five parallel read-only audits (combat/expedition, aquaponics deps, radio/DF, metrology/crafting, shared infra) against current source.
**Companion plan:** [`docs/plans/PLANS_86_89_INTEGRATION_PLAN.md`](plans/PLANS_86_89_INTEGRATION_PLAN.md)
**Naming caution:** Existing `docs/crafting/PLAN_87_*`, `docs/relationships/PLAN_88_*`, and `docs/narrative/PLAN_89_*` are **unrelated content closeouts**. This tranche uses **B86–B89** / `PLANS_86_89_*` filenames only.

Per roadmap §3: **Exit gate = every required cross-domain owner identified.**

---

## 0. Executive verdict (EXTEND vs CREATE)

| Plan | Verdict | Rationale |
|---|---|---|
| **B86** | **CREATE** `CombatBreachingEngine` + **EXTEND** dead `BarrierState` / expedition prep | No `TacticalCombatSystem.Obstacles.cs`. Live “breaching” already exists for **route mines** (`MineClearingFlailEngine` + `RouteInfrastructureSystem`) and **rail sabotage** — wrong owners for in-fight prop clearance. Combat already has unused `BarrierState` + ballistics barrier math. |
| **B87** | **CREATE** `AquaponicsSystem` | No fish/biofilter loop. Closest relatives: `AeroponicsSystem`, `HydroponicBiomeSystem`, `GreenhouseSystem`. Do **not** invent a fictional `FoodProductionSystem`. |
| **B88** | **EXTEND** `SignalTriangulationSystem` | Brief’s `RadioTriangulationEngine` **does not exist**. Continuous bearing+uncertainty DF already lives in `SignalTriangulationSystem`. Exact-fix HF intercepts already live in `ShelterRadioStationSystem` (B67 closed). Do **not** create a third DF engine. |
| **B89** | **CREATE** thin metrology authority + **EXTEND** `ShelterWorkshopSystem.Calibration` | Workshop already gates jobs on float `Calibration`. Ballistics already takes `toolingCalibration` but host hardcodes `0.75f`. No free global breakdown reduction exists today — keep benefits typed/consumer-local. |

---

## 1. Cross-domain owner matrix

| Concern | Current owner | Read API | Write API | Save section | Tick owner | Tests | B86–B89 treatment |
|---|---|---|---|---|---|---|---|
| **Tactical obstacles** | `BarrierState` + `CombatState.Barriers` in `CombatTypes.cs`; lookup `TacticalCombatSystem.FindPlayerLaneBarrier`; ballistics barrier reasons | `FindPlayerLaneBarrier`, `CombatState.Barriers`, `CombatCatalog.GetMaterial` | **None live** — `PlayerFire` nulls `BarrierMaterial` | Inside `combat` | Encounter actions / `EndTurn` | `CombatBallisticsTests.Barrier_Blocks`; no lifecycle tests | **EXTEND** barriers; add breaching actions via new engine |
| **Expedition equipment** | `ExpeditionSystem` / `ExpeditionState`; garage `ExpeditionVehicleSystem` | `hasBicycle`, `hasFlashlight`, `vehicleId`, `ExpeditionEstimate` | `Start(..., vehicle)`; garage attach | `expedition` | Expedition travel ticks | `ExpeditionVehicleLogisticsTests` | **EXTEND** prep/loadout gates for breach tools |
| **Vehicle tow/winch** | Data tags only: `vehicles.json` `winch_kit`; `vmod_heavy_winch` | attachments / garage slots | `AttachEquipment`, `InstallModification` | `vehicle_garage` / expedition aggregate | None for tow | Garage tests; no winch→obstacle | Optional B86 utility on existing tags |
| **Route mine clearance** | `RouteInfrastructureSystem` + `MineClearingFlailEngine` | `FindSegment`, flail progress | `RegisterMinefield`, `ApplyMineClearance`, `StartBreach`/`TickBreach` | `route_infrastructure` + flail section | Flail tick / Plans 146–149 | `RouteInfrastructureSystemTests`, Plans146–149 integration | **KEEP** as route authority |
| **Water treatment** | `WaterTreatmentSystem` | tank levels, filter, `State` | `AddWater`/`RemoveWater`, `StartTreatment`, `TickDay(day, power)` | `water_treatment` | Phase-2 shelter facilities | `WaterTreatmentSystemTests` | B87 **queries/consumes** |
| **Greenhouse** | `GreenhouseSystem` (+ Agriculture overlay) | plots, harvest | `Plant`/`Water`/`Harvest`/`TickDay` | `greenhouse` | GreenhouseFoundryDayOwner | Greenhouse* tests | B87 couples nutrients; greenhouse remains crop authority |
| **Aeroponics / hydro** | `AeroponicsSystem`, `HydroponicBiomeSystem` | chamber/rack state | plant/harvest/nutrient mix | `aeroponics`, `hydroponic_biomes` | Aero: day owner; Hydro: **campaign tick orphan** | Plans74–77, HydroponicBiomeTests | Pattern templates only |
| **Power / brownout** | `PowerGridSystem` | `IsBrownout`, `IsRoomPowered`, `Snapshot` | breakers, fuel, `TickDay` | `power_grid` | Phase-1 PowerGridDayOwner | PowerGrid* tests | B87/B88/B89 **query** room power |
| **Thermal** | `ShelterThermalSystem` | `GetCropTemperatureModifier`, room warmth | boiler/radiators/`TickDay` | `shelter_thermal` | Phase-2 facilities | `ShelterThermalSystemTests` | B87 fish/plant temp |
| **Kitchen / nutrition** | `KitchenNutritionSystem` (+ NutritionDiversity) | pantry/jobs | prep/serve/`TickDay` | `kitchen_nutrition` | Phase-2 facilities | KitchenNutrition* tests | B87 harvest → inventory → kitchen |
| **Radio intercepts (schedule)** | `RadioBroadcastCatalog` + `RadioScheduleCoordinator` + `FactionRadioEngine` | listen/resolve | listen/inject | `radio` | Listen-driven + day for distress | Radio* tests | B88 consumes intercept identity |
| **Distress** | `RadioDistressSystem` | active signals | `Intercept`, `MarkTriangulated`, `DispatchExpedition` | nested in `radio` | `TickDaily` via RadioHostSession | RadioDistress* tests | B88 may drive `MarkTriangulated` from DF complete |
| **Continuous DF** | **`SignalTriangulationSystem`** | observations, candidates, uncertainty | `RecordObservation`, `Triangulate` | Core Capture/Restore exists; **host radio save omits it** | Observation-driven | `SignalTriangulationSystemTests` | **PRIMARY B88 owner — EXTEND** |
| **Exact-fix HF intercepts** | `ShelterRadioStationSystem` | scan/tune | `RecordBearing` → authored `revealed_location_id` | `radio_station` | Plans46–49 day tick | ShelterRadioStation* tests | Keep separate (B67) |
| **Map intel** | `WastelandMapSystem` | node knowledge | `Discover`, **`DiscoverRumor(..., RadioIntercept)`** | world / wasteland map | world/host | map/world tests | B88 continuous path → **DiscoverRumor** |
| **Crafting** | `CraftingSystem` | recipes/stations | `StartCraft` | `crafting` | CraftingHostSession.TickDay | Crafting* tests | B89 may gate tier-5 recipes via grade |
| **Machine tools** | `ShelterWorkshopSystem` | `Calibration`, tooling health, recipes | jobs, overhaul, `TickDay` | `shelter_workshop` | Plans46–49 | ShelterWorkshop* tests | **PRIMARY B89 calibration surface** |
| **Equipment condition** | `EquipmentConditionSystem` | condition %, jam/slip | `ApplyWear`, `RepairItem`, `MaintenanceType.Calibrate` | `equipment_condition` | Expanded shelter tick | EquipmentCondition* tests | Wear for breach tools |
| **Precision optics** | `PrecisionOpticsEngine` | workpiece quality | grind/test/complete | `precision_optics` | Plans110–113 | host present; UI-10 gap | Registered B89 consumer candidate |
| **Ballistics calibration** | `BallisticsWorkbenchSystem` | profiles | `Calibrate(..., toolingCalibration, ...)` | `ballistics_workbench` | Plans74–77 | Plans74To77 tests | Wire live workshop grade (today hardcoded 0.75) |
| **Seismic / vibration** | `SeismicDynamicsSystem` | dampeners, rockburst events | install/service dampeners | `seismic_dynamics` | SeismicGeologyDayOwner | SeismicMonitoringB68* | B89 disturbance producer |
| **Inventory transactions** | `Inventory` / `IPlayerInventoryPort` | `CountById`, `HasSufficient` | `TryExecuteTransaction` / `BeginTransaction` | `inventory` | N/A | InventoryTransactionTests | Sole item mutation authority |
| **Campaign day** | `CampaignDayCoordinator` | phase owners | `Register` / `Advance` | N/A | Main.CampaignOwners | CampaignDay* tests | Register new day owners |
| **Seeded RNG** | `ISeededRng` + `CampaignRngManager` / `CampaignStreamIds` | fork/stream | Next* | campaign streams | per domain | DeterminismGuard | Add stream IDs for tranche domains |
| **Save registry** | `SaveSectionRegistry` + `SaveStoreHub` | section metadata | CaptureSection | campaign envelope | SaveAll | SaveStore* gates | New sections only for owned state |
| **Catalog integrity** | `CatalogIntegrityValidator` + `CatalogIntegrityRules` | prefixes/refs | N/A | N/A | `--data-integrity-selftest` | CatalogIntegrity* | Register new catalogs/prefixes |
| **Content utilization** | `ContentUtilizationScanner` | loader→consumer map | N/A | baseline artifact | `--content-utilization-selftest` | utilization gate | Map new JSON → loader/system names |
| **CLI selftests** | `HostCliRegistry` + `HostCli` | verbs | runners | N/A | headless | selftest manifest | Add feature `--*-selftest` verbs |
| **UI panels** | PanelRegistry + Main.UiPanels / PlayerSurfaces | Bind/Refresh | commands → host | N/A | player | scene-binding / UI audit | Presentation only |

---

## 2. Mandatory brief-file status

| Brief path | Status |
|---|---|
| `TacticalCombatSystem.cs` | **Exists** |
| `TacticalCombatSystem.Obstacles.cs` | **MISSING** — brief assumed a file not in the tree |
| `ExpeditionSystem.cs` | **Exists** |
| `KitchenNutritionSystem.cs` | **Exists** |
| `WaterTreatmentSystem.cs` | **Exists** |
| `GreenhouseSystem.cs` | **Exists** |
| `RadioBroadcastCatalog.cs` | **Exists** |
| `RadioDistressSystem.cs` | **Exists** |
| `RadioTriangulationEngine.cs` | **MISSING** — real owner is `SignalTriangulationSystem.cs` |
| `WorkshopPanel.cs` / `CraftingPanel.cs` | **Exist** |
| `EquipmentConditionSystem.cs` | **Exists** |
| `items.json` / `recipes.json` | **Exist** |

---

## 3. Plan-specific notes

### 3.1 B86 — Combat breaching

- Prefer activating `BarrierState` over a parallel obstacle database.
- Keep mine flail / route infrastructure as corridor mine authority.
- Do not promote `VaultDoorBreachingPanel` (UI-05 stub, no Core owner).
- Items present: `item_thermal_lance`, `item_titanium_breaching_shield`, `item_mine_flail_module`, `item_mine_prod`. Wire-cut tools need authoritative IDs if used.
- Noise: extend `StealthSystem` / stance noise — do not fork a second noise ledger.

### 3.2 B87 — Aquaponics

- No existing fish loop; location constant only (`HydroBaronsAquaponics`).
- Tick after power (phase 1), with water/thermal in phase 2.
- Do not depend on orphaned `TickAdvancedShelterSystems` hydro path.
- Harvest → inventory → kitchen; kitchen does not grow fish.

### 3.3 B88 — HF/DF

- Extend `SignalTriangulationSystem`; keep `ShelterRadioStationSystem` for exact-fix.
- Gaps: radio save persistence, `DiscoverRumor` map handoff, real station baselines, weather on continuous path, optional distress bridge.
- Acoustic DF catalog is early-warning vibration — not HF radio.

### 3.4 B89 — Metrology

- Promote workshop float Calibration toward typed `PrecisionCalibrationGrade`.
- Fix ballistics host hardcoded `toolingCalibration = 0.75f`.
- Wire seismic disturbance → calibration drift.
- No bunker-wide breakdown reduction flag.

---

## 4. Shared infrastructure cookbook

| Step | Touch |
|---|---|
| 1 | Core `XSystem` + state DTO (`CaptureState`/`RestoreState`, `ISeededRng`) |
| 2 | `Assets/StreamingAssets/Data/x_catalog.json` (`schema_version`, snake_case ids) |
| 3 | Catalog loader |
| 4 | `CatalogIntegrityRules` + content utilization scanner |
| 5 | `CampaignStreamIds` domain fork |
| 6 | SaveStore + `SaveSectionRegistry` (only if new owned state) |
| 7 | `Main.PlansB86_B89.cs` Setup/Save/Tick triad |
| 8 | `Main.CampaignOwners.cs` day owner |
| 9 | Optional HostSession |
| 10 | UI panel + registry (Prototype until real commands) |
| 11 | `--x-selftest` in HostCliRegistry |
| 12 | xUnit behavior + save round-trip |

Templates: `src/Main.Plans78_81.cs`, `src/Main.Plans190_193.cs`.

---

## 5. New artifact ownership

| Artifact | Owner | Save |
|---|---|---|
| `breaching_equipment_catalog.json` | B86 → `CombatBreachingEngine` | Prefer embed mid-breach in `combat` |
| `aquaponics_system_catalog.json` | B87 → `AquaponicsSystem` | **New** `aquaponics` section |
| `direction_finding_catalog.json` | B88 array/instrument profiles | Extend **`radio`** |
| `metrology_standards_catalog.json` | B89 → `PrecisionMetrologySystem` | Prefer extend `shelter_workshop` + metrology DTO |

---

## 6. Mutual reinforcement (typed)

```text
PrecisionCalibrationGrade (B89)
  → breach tool quality (B86)
  → aquaponics pump/sensor reliability (B87)
  → DF array calibration (B88)

DirectionFindingCapability (B88)
  → DiscoverRumor → expedition planning (B86)

AquaponicNutrientSource (B87)
  → greenhouse nutrient input (explicit)

ObstacleBreachingCapability (B86)
  → barrier/path clearance

SeismicDynamicsSystem
  → B89 calibration drift only
```

---

## 7. Exit-gate checklist

- [x] Tactical obstacle owner identified
- [x] Expedition equipment owner identified
- [x] Water / greenhouse / power / thermal owners identified
- [x] Radio intercept + triangulation owners identified
- [x] Map intel owner identified
- [x] Crafting / workshop / equipment condition owners identified
- [x] Campaign day, RNG, inventory, save registry, catalog integrity, CLI, UI conventions identified
- [x] EXTEND vs CREATE decisions recorded
- [x] Naming collisions with unrelated Plan 87/88/89 docs recorded

**Reconnaissance gate: PASS.**
