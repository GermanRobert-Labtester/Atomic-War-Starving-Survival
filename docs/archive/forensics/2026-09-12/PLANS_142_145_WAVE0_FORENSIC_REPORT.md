# Plans 142–145 — Wave 0 Forensic Reconnaissance Report

Date: 2026-09-06 (recon session)
Mode: READ-ONLY forensic audit (ashfall-analyze). No production code or data modified.
Target: Plans 142 (cellulosic biofuel), 143 (ECM), 144 (precision broaching), 145 (fog harvesting) — authority verification and baseline regression per the flagship plan's Phase 0, before any edits.

---

## 1. Executive Finding

All four plans are **genuinely new** — no cellulosic/ECM/broaching/fog-harvesting content, catalogs, engines, panels, or save sections exist. The correct architectural blueprint already exists in the repo: **`PlasticPyrolysisSystem` (Plan 202)** is a near-perfect precedent for a staged, hazard-rolling, inventory-bound, save-safe industrial production system, and the surrounding host wiring (`Main.Plans202_205.cs`, `SaveSectionRegistry`, `PanelRegistryBootstrap`) gives the full pipeline a new system must follow.

Four plan assumptions are contradicted by the active branch:
1. Several named authorities do not exist under their plan names (see matrix below for real equivalents).
2. **No fog/humidity weather state exists** in `WeatherSystem` — Plan 145 needs a weather-signal extension or a derivation rule; `WeatherSondeSystem` already models `humidityPct` for sonde telemetry only.
3. **No hostile sensor-lock mechanic exists** — Plan 143's own gate applies: introduce a general `SensorThreatState` abstraction (StealthSystem has detection-risk, TacticalCombatSystem has combatant targeting, neither has a lock-confidence state machine).
4. **No biomass feedstock items exist** (no crop stalks/straw/husk/compost ids); only `scrap_wood`, `paper_stock`, and scrap-family items. Plan 142's "greenhouse crop waste" source does not exist — GreenhouseSystem emits no waste stream.

The content-utilization selftest **hangs on this branch** (two runs >900 s, no verdict, killed). It is NOT in the CI gate manifest, so CI is unaffected, but any Plans 142–145 closeout that cites it needs owner attention first.

## 2. Baseline Regression (Phase 0.5)

| Check | Result |
|---|---|
| `dotnet build Ashfall.csproj` | **PASS** — 0 warnings, 0 errors |
| `dotnet test Ashfall.Core.Tests` | **PASS** — 10,847/10,847, 56 s |
| `--data-integrity-selftest` | **PASS** — 303 catalogs, 12,742 ids, 0 errors, 0 warnings |
| `--bridge-selftest` | **PASS** — exit 0 (shim-removal verb) |
| `--scene-binding-selftest` | **PASS** — exit 0 |
| `--content-utilization-selftest` | **HANG** — completes static+runtime phases (588 catalogs, 245 gameplay-consumed, 0 orphaned, prints "Complete" banner) then never terminates or prints a verdict; killed at >900 s ×2. Not in CI manifest (`docs/ci/CI_GATE_MANIFEST.json`, 49 gates). Unknown: whether it eventually finishes on slower hardware; earlier closeouts cite stage-4 passes. |

## 3. Authority Matrix — Plan Claim vs Active Branch

| Domain | Plan claims | Active-branch truth | Evidence |
|---|---|---|---|
| Staged fuel production | `BiogasDigesterSystem` / `PlasticPyrolysisEngine` | **`PlasticPyrolysisSystem`** (`Assets/Ashfall.Core/Shelter/PlasticPyrolysisSystem.cs`); biogas exists only as a **fake-success UI prototype** (`AnaerobicBiogasDigesterPanel`, UI-06 register) — no Core digester system | `rg "class .*Pyrolysis"`; `src/UI/AnaerobicBiogasDigesterPanel.cs` |
| Power grid | `ShelterPowerGridSystem` | **`PowerGridSystem`** (`Assets/Ashfall.Core/Shelter/PowerGridSystem.cs`), `AddFuel(float units)` + `TickDay(day, ISeededRng)`; host session `PowerGridHostSession`; energy credit precedent `Main.Plans202_205.cs:298` | `src/Main.World.cs:514` |
| Expedition fuel/vehicles | `ExpeditionSystem` / vehicle logistics | Confirmed: `ExpeditionVehicleProfile` (fuelPerTravelTick, breakdown), `ExpeditionVehicleSystem` embedded in `ExpeditionSystem.cs`; garage in `ExpeditionHostSession` (`RefuelVehicle`, `EstimateExpedition` fuel gate) | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs:87`; `src/Host/ExpeditionHostSession.cs:542` |
| Tactical combat | `TacticalCombatSystem` | Confirmed (partial classes: Targeting/Actions/Breaching/Damage/Persistence) — **no sensor-lock state**; targeting is combatant lookup only | `Assets/Ashfall.Core/Combat/TacticalCombatSystem.Targeting.cs` |
| Detection/stealth | `DirectionFindingEngine`, `InSarInterferometryEngine` | **Neither exists.** `StealthSystem.CalculateDetectionRisk` (weather/light/terrain, deterministic) is the only detection-risk model; `ReconTelemetrySystem` is player-side recon probes | `Assets/Ashfall.Core/Combat/StealthSystem.cs:163` |
| Precision metrology | `PrecisionMetrologyEngine`, `ArmoryCalibrationBenchEngine` | **`PrecisionMetrologySystem`** exists (`Assets/Ashfall.Core/Shelter/PrecisionMetrologySystem.cs`) with `metrology_standards_catalog.json` + tests. **No armory/calibration-bench engine exists**; no broaching capability anywhere | rg sweeps |
| Equipment quality | `EquipmentConditionSystem` | Confirmed: condition/wear/maintenance/jury-rig/repair with `DegradationProfileDef` catalog; `RepairItem(..., repairQuality)` is the bounded quality seam. No `precision_component_quality` field exists | `Assets/Ashfall.Core/EquipmentConditionSystem.cs:340` |
| Water treatment | `WaterTreatmentSystem` | Confirmed: `AddWater(WaterType, float)`, `StartTreatment(mode, amount)`, `SetIncomingContamination`, `RegisterContaminationAdvisory`, intake blocking, `TickDay`, capture/restore. `WaterType = {Clean, Raw, Brackish, Irradiated}` — fog water enters as **Raw** | `Assets/Ashfall.Core/WaterTreatmentSystem.cs:119,680` |
| Aquifer | `AquiferManagementSystem` | **Does not exist.** Real water-surface owners: `GeothermalAquiferSystem`, `AquiferPiezometerEngine` (shelter heat/water) | `Assets/Ashfall.Core/Shelter/GeothermalAquiferSystem.cs` |
| Greenhouse irrigation | `GreenhouseSystem` | Confirmed: `Water(int plotIndex, float waterUnits, bool tainted)`; `ApicultureSystem` has internal humidity. **No crop-waste output stream** | `Assets/Ashfall.Core/Greenhouse/GreenhouseSystem.cs:174` |
| Weather/humidity/fog | macro/local weather authority | `WeatherSystem` (World) — season windows with weights for clear/rain/overcast/ashfall/fallout_storm/blizzard/black_rain + Plan-205 wind (deg, kph). **No humidity, no fog kind.** `WeatherSondeSystem.humidityPct` is sonde-telemetry only | `Assets/Ashfall.Core/World/WeatherSystem.cs:45–86`; `WeatherSondeSystem.cs:19,551` |
| Save | campaign save registry | Confirmed: `SaveSectionRegistry` one-line registration (`new("plastic_pyrolysis", "Save…", "Setup…", group, desc, LifecycleGroup)`) + filename map; `CaptureSection` pattern | `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs:164,361` |
| Kitchen/food waste | `KitchenNutritionSystem` | Exists (`Assets/Ashfall.Core/KitchenNutritionSystem.cs`) — waste-stream role unverified (not needed for Wave 0 go/no-go) | — |
| Fuel item taxonomy | — | Real items: `fuel`, `diesel_fuel`, `fuel_1l`, `fuel_cell`, `fuel_canister`, `synthetic_fuel_canister`, `item_aviation_fuel_canister`. Pyrolysis outputs claimed into inventory (`OnOutputsClaimed`), then fed to grid via `AddFuel` | `items.json`; `PlasticPyrolysisSystem.cs` events |
| Biomass feedstock | crop stalks / straw / wood shavings / paper | **Gaps**: no stalk/straw/husk/compost items; available: `scrap_wood`, `paper_stock`, scrap family | `items.json` sweep |
| Survivor traits | BiofuelChemist, DistilleryMaster, ElectronicWarfareOfficer, RfJammingSpecialist, MasterBroacher, Gunsmith, Meteorologist, AtmosphericHydrologist | **None of these trait ids exist.** Traits are `trait_xxx` snake ids; existing neighbors: `trait_alchemist`, `trait_dark_acclimated`; skills include `skill_drone_operator`, `skill_machinist?` (unconfirmed — `survivor_machinist` is a survivor id). All eight plan traits must be authored new or mapped to existing skill/trait owners | `survivors.json:812`; `skills.json` |
| Vehicle mod framework (Plan 143.7 signature coating) | Plan 50 framework | **No vehicle upgrade/mod-slot framework found** in `ExpeditionSystem` — a bounded `signature_modifier` would be a new vehicle-instance field (extension seam, not a duplicate authority) | rg `upgrade|mod_slot|signature` → no hits |
| Draisine (Plan 144 consumer) | Plan 101 | Confirmed: `RailwaySystem` (`Assets/Ashfall.Core/Expeditions/RailwaySystem.cs`) | — |

## 4. Canonical Blueprint — `PlasticPyrolysisSystem` (follow for all four systems)

- Constructor-injected `ISeededRng` + `ILog` (never `System.Random`).
- `BindInventory(Func getCount, canAdd, addItem, consume)` port pattern — no direct inventory dependency.
- `BindCatalog` / loader; unknown profile under a save → batch fails safely, never throws.
- `ActionResult.Success/Blocked/Failed` atomic transactions — validation before any consume.
- Machine construction/maintenance via item costs; condition gates batch start (`< 25f` blocked).
- `TickDay(gridPowerAvailableKwh)` — power deficit → `phase = "stalling"`, no progress, no hazard roll.
- **Fresh-seed hazard roll**: `new SeededRng(unchecked(CurrentDay() * 7919 + 101))` — day-derived, so save/load-split runs produce identical outcomes without carrying RNG sequence state. Reuse this pattern for all stochastic rolls in Plans 142–145.
- Events: `OnBatchCompleted`, `OnIncident`, `OnGasRelease` (→ ventilation authority), `OnFireIncident` (→ hazard authority), `OnOutputsClaimed`. Consequence ownership stays with destination authorities.
- Output buffer cap (`storage_buffer_max_batches`) — full buffer blocks new batches.
- Host wiring: `Main.Plans202_205.cs` — catalog load → `BindInventory` → `SaveStore.TryLoad` → Setup; `CaptureSection` on save; TickDay with power-grid surplus; completion credit → `_powerGrid.System.AddFuel(creditKwh / 2f)`.
- Save: `PlasticPyrolysisSaveStore` (host façade over Core `SaveStore<T>`); registry section `plastic_pyrolysis`.
- UI: `src/UI/PlasticPyrolysisPanel.cs` — `IBindablePanel`, typed `Bind(HostSession)`, presentation-only; NOT in PanelRegistryBootstrap (no route) — note: pyrolysis panel wiring may itself be un-routed; do not copy that part without checking route/UI-09 rules.

## 5. Confirmed Gaps (must be addressed in planning)

1. **Fog/humidity weather signal (Plan 145)** — absent. Options: extend `weather_seasons.json`/`WeatherSystem` with a fog kind or derive fog presence from overcast + wind + season + region; `WeatherSondeSystem.humidityPct` shows the sampling precedent. Decision belongs to Wave 1/2 planning with the weather authority owner.
2. **Hostile sensor-lock mechanic (Plan 143)** — absent. Plan 143's own gate fires: introduce a general `SensorThreatState` (Searching/Tracking/Locked/AttackCommitted/LostTrack) owned by the expedition-threat/combat authority; ECM only modifies lock confidence. No real RF parameters.
3. **Biomass feedstock economy (Plan 142)** — no crop-waste items; greenhouse emits no waste. Must author feedstock items + give greenhouse/compost/kitchen an opportunity-cost link, or map onto `scrap_wood`/`paper_stock` only.
4. **Vehicle signature-mod seam (Plan 143.7)** — no mod framework; new bounded vehicle-instance field required.
5. **Trait/skill IDs (all plans)** — the eight plan-named traits do not exist; author new `trait_`/`skill_` ids or reuse existing generic skills.
6. **Content-utilization gate health** — hangs on this branch; must be resolved or scoped before closeout claims cite it.

## 6. Risks

- **CRITICAL** — Plan 142 must NOT write to the power grid directly except through the established fuel-item → `AddFuel` route (or documented credit precedent); a parallel fuel pool would fork state authority. The pyrolysis `AddFuel(creditKwh/2f)` precedent exists but is an energy credit, not a fuel item — planning must pick one canonical route and document it.
- **HIGH** — Plan 143 touching combat/expedition threat resolution without the `SensorThreatState` gate would embed lock semantics inside ECM (the exact anti-pattern the plan forbids).
- **HIGH** — All four panels face the UI-09/UI-21 register rules: route → bind → visible → command → state delta → feedback proof via headless gate; no new panel is "done" from file creation.
- **MEDIUM** — New catalogs must pass `CatalogIntegrityValidator` (schema_version, snake_case, id prefixes) and the deep-chain utilization gate once healthy.
- **LOW** — The biogas UI prototype (`AnaerobicBiogasDigesterPanel`) must not be treated as a Core digester; Plan 142's biofuel plant is a separate, real system.

## 7. Constraints for Planning

1. Follow the `PlasticPyrolysisSystem` blueprint exactly (ports, ActionResult, fresh-seed rolls, events, save store façade, registry line).
2. `WaterType.Raw` is the only fog-water entry point; treatment decides potability.
3. Fuel quality tiers must map onto existing fuel item taxonomy + `PowerGridSystem.AddFuel`/vehicle `RefuelVehicle` seams.
4. Metrology integration for Plan 144 goes through `PrecisionMetrologySystem`, not a new metrology authority.
5. All new save sections enter `SaveSectionRegistry` with legacy defaults per the plan's Save Compatibility Matrix (idle plant / inactive ECM / idle bench / no array — never free output or immunity).
6. All stochastic rolls use day-derived fresh seeds (save/load-split determinism).

## 8. Evidence Index

- `Assets/Ashfall.Core/Shelter/PlasticPyrolysisSystem.cs` (whole file — blueprint)
- `Assets/StreamingAssets/Data/plastic_pyrolysis_catalog.json` (schema precedent)
- `src/Main.Plans202_205.cs` (host wiring precedent: catalog bind, save load, CaptureSection, TickDay, grid credit)
- `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs:164,361`
- `Assets/Ashfall.Core/WaterTreatmentSystem.cs:119–686` (WaterType enum at 680)
- `Assets/Ashfall.Core/World/WeatherSystem.cs:45–86`; `WeatherSondeSystem.cs:19,551`
- `Assets/Ashfall.Core/Combat/StealthSystem.cs:163`; `Combat/TacticalCombatSystem.Targeting.cs`
- `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs:87`; `src/Host/ExpeditionHostSession.cs:541–549`
- `Assets/Ashfall.Core/Shelter/PrecisionMetrologySystem.cs`; `metrology_standards_catalog.json`
- `Assets/Ashfall.Core/EquipmentConditionSystem.cs:340`
- `Assets/StreamingAssets/Data/items.json` (fuel ids: `fuel`, `diesel_fuel`, `fuel_1l`, `fuel_cell`, `fuel_canister`, `synthetic_fuel_canister`; feedstock: `scrap_wood`, `paper_stock`)
- Baseline logs: host build 0/0; tests 10,847/10,847 (56 s); data-integrity 303 catalogs 0 errors; scene-binding exit 0; bridge exit 0; content-utilization `/tmp/content-util.log` (hang)

## 9. Confidence & Unknowns

- High confidence: authority matrix (exhaustive name + synonym sweeps), baseline build/test/integrity/scene/bridge, pyrolysis blueprint reading.
- Unknown: content-utilization eventual completion time / whether the hang is environmental; exact `KitchenNutritionSystem` waste-stream shape; whether a `machinist` skill id exists (only `survivor_machinist` and `rule_workshop_machinist` confirmed); pyrolysis panel's route/registration status (relevant as precedent for panel wiring, flagged for Wave 7 checks).
