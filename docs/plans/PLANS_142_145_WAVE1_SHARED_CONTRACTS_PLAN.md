# Plans 142–145 — Wave 1 Shared-Contract Plan (PR 1)

Status: PLAN (read-only planning; no production code modified)
Inputs: `docs/archive/forensics/2026-09-12/PLANS_142_145_WAVE0_FORENSIC_REPORT.md`, flagship Plans 142–145 plan, active-branch source.
Scope: shared foundations only — authority matrix, catalog schemas, `SensorThreatState` contract, fog-weather-signal decision, ID authoring lists, save-section entries, blocking-item triage. Per-system deep plans (Waves 2–5) consume this document.

---

# 1. Objective

Establish the four systems' shared contracts so Waves 2–5 can be implemented as independent, parallel-safe vertical slices without re-litigating ownership, naming, save shape, or determinism rules.

# 2. Current Reality (verified, condensed)

- Blueprint: `PlasticPyrolysisSystem` (`Assets/Ashfall.Core/Shelter/PlasticPyrolysisSystem.cs`) — constructor-injected `ISeededRng`/`ILog`, `BindInventory` ports, `ActionResult` atomic transactions, power-deficit stall, **day-derived fresh-seed hazard rolls** (`new SeededRng(unchecked(CurrentDay() * 7919 + 101))`), consequence events, `CaptureState/RestoreState`.
- Authorities (corrected names): `PowerGridSystem` (`AddFuel(float)`, `TickDay(day, ISeededRng)`), `WaterTreatmentSystem` (`WaterType {Clean, Raw, Brackish, Irradiated}`, `AddWater`, `StartTreatment`, `SetIncomingContamination`, `RegisterContaminationAdvisory`), `PrecisionMetrologySystem`, `EquipmentConditionSystem` (`RepairItem(..., repairQuality)` quality seam), `ExpeditionSystem` + `ExpeditionVehicleProfile` (fuelPerTravelTick, breakdown), `ExpeditionHostSession` garage (`RefuelVehicle`, fuel-gated estimates), `GreenhouseSystem.Water(plotIndex, units, tainted)`, `RailwaySystem`, `WeatherSystem` (kinds: clear/rain/overcast/ashfall/fallout_storm/blizzard/black_rain + Plan-205 wind; **no humidity, no fog**).
- Threat layer: `TravelEncounterSystem` (`Assets/Ashfall.Core/Narrative/TravelEncounterSystem.cs`) + `ExpeditionEncounterBridge` (`Assets/Ashfall.Core/Expeditions/ExpeditionEncounterBridge.cs`) surface encounters (`EncounterSurfaced`: category, `is_patrol`, faction, choices). **No sensor-lock mechanic exists.**
- Save: `SaveSectionRegistry` one-line entries + filename map + `CaptureSection` in `Main.<Domain>.cs` partials.
- Baseline: build 0/0; tests 10,847/10,847; data-integrity 303 catalogs 0 errors; scene-binding PASS; bridge PASS; **content-utilization HANGS** (see §8).

# 3. Required Delta (Wave 1 only)

New, empty-but-typed shared contracts: four catalog schemas, the `SensorThreatState` DTO/port, the fog-presence evaluator contract, ID authoring lists, save-section registrations, and the determinism/save conventions. No gameplay behavior in this wave beyond contract types + loader plumbing + tests that prove the contracts load and round-trip.

# 4. Revised Authority Matrix (canonical names)

| Concern | Authoritative owner (verified) | Plans 142–145 role |
|---|---|---|
| Staged fuel production precedent | `PlasticPyrolysisSystem` (Shelter) | Blueprint only — Plan 142 is a **new** `CellulosicBiofuelSystem` |
| Fuel items → generators | `items.json` fuel taxonomy + `PowerGridSystem.AddFuel` via `PowerGridHostSession` | Biofuel output = inventory fuel items; grid consumption stays canonical |
| Expedition fuel | `ExpeditionHostSession` garage + `ExpeditionVehicleProfile.fuelPerTravelTick` | Vehicle-grade fuel compatibility check |
| Ventilation/hazard | pyrolysis event pattern (`OnGasRelease` → ventilation authority, `OnFireIncident` → hazard authority) | Plan 142 `IndustrialFireHazardRequested` |
| Water storage/treatment | `WaterTreatmentSystem` | Plan 145 raw water enters as `WaterType.Raw` + `RegisterContaminationAdvisory` |
| Irrigation | `GreenhouseSystem.Water(int, float, bool)` | Treated water routing |
| Weather signal | `WeatherSystem` (read-only) + §6 fog decision | Plan 145 collection input |
| Precision metrology | `PrecisionMetrologySystem` | Plan 144 alignment/quality modifier |
| Equipment quality | `EquipmentConditionSystem` (`repairQuality` seam; `DegradationProfileDef` catalog) | Plan 144 bounded component-quality modifier |
| Expedition threat lifecycle | `TravelEncounterSystem` + `ExpeditionEncounterBridge` | Plan 143 `SensorThreatState` home (§5) |
| ECM simulation | new `ElectronicCountermeasureSystem` (Core, `Ashfall.Core.Expeditions` or `Ashfall.Core.Radio`) | modifies threat state only |
| Production machinery items | `items.json` + `recipes.json` | Plan 144 industrial components |
| Save | `SaveSectionRegistry` + host `*SaveStore` façades over Core `SaveStore<T>` | four new sections (§7) |
| RNG | `ISeededRng` / day-derived fresh seeds | all stochastic rolls |

**Rule:** no system writes another's state directly; all cross-domain effects flow through the owner's public command API or a consequence event (pyrolysis event pattern).

# 5. `SensorThreatState` Design (Plan 143 gate resolution)

**Owner:** `TravelEncounterSystem` (threat lifecycle) — ECM never owns threat truth.

New file `Assets/Ashfall.Core/Expeditions/SensorThreatState.cs` (namespace `Ashfall.Core.Expeditions`):

```csharp
public enum SensorThreatPhase { Searching, Tracking, Locked, AttackCommitted, LostTrack }

public sealed class SensorThreatState            // persisted inside the expedition/travel section
{
    public string threat_id = "";                // encounter-scoped, e.g. "sensor_threat_3"
    public string threat_profile_id = "";        // authored profile (ecm_catalog threat_profiles)
    public SensorThreatPhase phase = SensorThreatPhase.Searching;
    public float lock_confidence = 0f;           // 0..1
    public float detection_confidence = 0f;      // 0..1
    public int false_track_count = 0;            // deception pressure (abstract)
    public bool counter_detection_raised;        // resolved fact — persisted, never rerolled
    public int day_detected;                     // provenance, not gameplay-clock
}

public interface ISensorThreatQuery              // read/modifier port ECM consumes
{
    SensorThreatState? GetActiveThreat(string expeditionId);
    void ApplyEcmModifier(string expeditionId, float lockBreakDelta01,
                          float detectionReduction01, int falseTrackPressure);
}
```

Contract rules:
1. `TravelEncounterSystem` implements `ISensorThreatQuery`; only it transitions `phase` and resolves attacks.
2. ECM calls `ApplyEcmModifier` with **bounded** deltas from its catalog; it cannot set phase, commit attacks, or grant immunity (assertion test: lock_confidence can never be forced to 0 by a single modifier; attack resolution always remains possible).
3. `counter_detection_raised` is a **resolved fact** — set once (fresh-seed roll), persisted, never rerolled on restore (Invariant F).
4. Encounter surfacing: sensor-type encounters (`EncounterSurfaced.category` — exact category tag authored in Wave 5) attach a `SensorThreatState` at surfacing; attack quality uses `lock_confidence` at resolution time.
5. No RF-domain fields anywhere (no frequency/band/waveform data).

# 6. Fog-Weather-Signal Decision Options (Plan 145)

| Option | Description | Pros | Cons |
|---|---|---|---|
| **A. Derive (recommended)** | New Core `FogPresenceEvaluator` computes fog presence 0..1 deterministically from `WeatherSystem` reads: `overcast/black_rain` kind + wind band (Plan-205 `WindSpeedKph`) + season window + region/terrain tag. Fog system owns the evaluator; weather authority untouched. | Zero schema change to the weather authority; save-safe (pure function of persisted weather); reversible; precedent = `StealthSystem` deriving risk from weather inputs | "Fog" is not a first-class weather kind (forecast UI derives, not reads); narrative fog events would need Option B later |
| B. Author fog kind | Add `fogWeight` to `weather_seasons.json` seasons + a `fog` WeatherKind in `WeatherSystem` | First-class fog; forecast/sonde/narrative can reference it | Touches the shared weather authority + schema_version + every WeatherKind consumer (sweeps: hazards, UI, narrative) — too broad for Wave 1 |
| C. Humidity field | Add `humidity_pct` to `WorldWeatherState` (Plan-205 wind pattern) | Enables future apiculture/sonde convergence | New simulation variable needing its own rolls/balance; not needed for fog derivation |

**Decision:** Option A for Wave 1/2. Revisit Option B only if a narrative/fog-event requirement lands. The evaluator's formula (bands × weights) is authored in `fog_harvesting_catalog.json` (`fog_bands` block) so tuning never touches code.

# 7. Save-Section Registry Entries (legacy defaults)

Add to `SaveSectionRegistry` (filename map + section lines), all in the `industry`/`expedition` groups matching siblings:

| Section id | Save/Setup methods | Group | Legacy default (old saves) | Forbidden bad default |
|---|---|---|---|---|
| `cellulosic_biofuel` | `SaveCellulosicBiofuel` / `SetupCellulosicBiofuel` | `industry` | plant not constructed, no batch, empty tank | free completed fuel |
| `radar_ecm` | `SaveRadarEcm` / `SetupRadarEcm` | `expeditions` | no device installed, ECM inactive | permanent immunity / active device |
| `precision_broaching` | `SavePrecisionBroaching` / `SetupPrecisionBroaching` | `crafting` | bench not constructed, tools healthy baseline, no job | free precision output |
| `fog_harvesting` | `SaveFogHarvesting` / `SetupFogHarvesting` | `infrastructure` | no array, empty buffer | infinite clean water |

All four stores: Core `SaveStore<T>` façade via `SaveStoreHub` (checksummed envelope, atomic write, `SchemaVersionedEnvelope` where legacy shape applies), `allowLegacyBareState:false` (new sections have no legacy format). Mid-process persistence: committed inputs, resolved failures, and claimed outputs persist with the batch/job (no duplicate consumption/output on restore).

# 8. BLOCKING ITEM — Content-Utilization Hang (triage plan)

**Known facts:** `src/Host/ContentUtilizationSelfTest.cs` prints all phases (static inventory 588 catalogs → runtime evidence 1,534 events → deep chain → summary → `"=== Content Utilization Self-Test Complete ==="` then `return exitCode;` at :172-ish). The process then **never exits** and `HOST_SELFTEST_SUMMARY` (printed by the caller after return) never appears. Runs: >900 s ×2 (killed). Other selftests (data-integrity, bridge, scene-binding) exit cleanly through the same host. Gate is **not** in `docs/ci/CI_GATE_MANIFEST.json` — CI is unaffected.

**Hypotheses (ordered):**
1. Post-return host shutdown differs on this action (e.g., a non-zero `exitCode` path skipping the quit call; `Unresolved: 71` / deep-chain warnings may set exitCode ≠ 0).
2. `TryGetCurrentCommitHash` spawns `git` (ProcessStartInfo) — subprocess hang in headless env.
3. Deep-chain artifact write to a blocked/unwritable path.
4. Genuine long runtime (full deterministic campaign) — but phases already printed, so only post-banner work remains.

**Triage steps (read-only, owner to execute):**
1. Run with `timeout 300 godot --headless --path . -- --content-utilization-selftest`; capture exit code and whether `HOST_SELFTEST_SUMMARY` appears.
2. Inspect the caller in `src/Host/HostCli.cs` / `src/Main.Application.cs` for the quit path when exitCode ≠ 0.
3. `strace -f -e trace=process,file` the tail of the run to catch a blocked `git` spawn or file write.
4. If environmental: document allowlist; if code: minimal repair (separate PR, not bundled with Waves 2–5).
**Gate rule:** Waves 2–5 proceed gated by data-integrity + unit tests + `ContentDeepChainGate` unit coverage; **no closeout may cite the content-utilization selftest until this item resolves.**

# 9. Four Catalog Schemas (pyrolysis style)

All: `schema_version: 1`, snake_case ids, bounded permille values, no real-world process parameters. Shapes mirror `plastic_pyrolysis_catalog.json` (machine block + profile/entry lists + hazard outcomes).

## 9.1 `cellulosic_ethanol_catalog.json` (Plan 142)

```json
{
  "schema_version": 1,
  "machine": {
    "machine_id": "machine_cellulosic_biofuel_plant",
    "display_name": "Stills_and_Substrate_Bay",
    "construction_required_items": { "scrap_metal": 10, "scrap_wood": 6, "item_internal_spline_hub": 1 },
    "construction_labor_days": 3,
    "max_condition": 100,
    "maintenance_interval_days": 5,
    "maintenance_required_items": { "scrap_metal": 2, "item_separation_media_cartridge": 1 },
    "room_id": "room_generator",
    "power_class": "high",
    "ventilation_class": "industrial",
    "ventilation_load_per_active_day": 0.06
  },
  "processes": [
    {
      "process_id": "biofuel_cellulose_standard",
      "display_name": "Standard Cellulose Run",
      "accepted_item_ids": ["item_crop_waste", "scrap_wood", "paper_stock"],
      "feedstock_tag": "cellulose",
      "batch_input_units": 10,
      "conversion_tier": "standard",
      "conversion_efficiency_permille": 480,
      "fermentation_ticks": 2880,
      "refining_ticks": 1440,
      "energy_cost_kwh_per_day": 7,
      "process_water_units": 4,
      "fuel_output_id": "item_biofuel_generator_grade",
      "yield_permille": 520,
      "fouling_risk_permille": 90,
      "vapor_hazard_tier": "elevated"
    }
  ],
  "fuel_grades": [
    { "grade_id": "fuel_grade_low", "output_item_id": "item_biofuel_low_grade", "grid_worth_units": 0.5, "vehicle_wear_multiplier": 1.5 },
    { "grade_id": "fuel_grade_generator", "output_item_id": "item_biofuel_generator_grade", "grid_worth_units": 1.0, "vehicle_wear_multiplier": 1.2 },
    { "grade_id": "fuel_grade_high", "output_item_id": "item_biofuel_high_grade", "grid_worth_units": 1.3, "vehicle_wear_multiplier": 1.0 }
  ],
  "hazard_outcomes": {
    "faults": ["power_interrupted", "ventilation_unsafe", "culture_failed", "column_fouled", "tank_full", "emergency_stop"],
    "fire_hazard_event": "IndustrialFireHazardRequested"
  },
  "storage": { "tank_max_units": 60 }
}
```

## 9.2 `radar_ecm_catalog.json` (Plan 143)

```json
{
  "schema_version": 1,
  "ecm_profiles": [
    {
      "profile_id": "ecm_vehicle_deception_mk1",
      "display_name": "Vehicle Deception Suite Mk I",
      "modes": ["passive_signature", "noise_jam", "deception", "burst_defense"],
      "power_draw_class": "high",
      "heat_rate_per_active_tick": 4,
      "max_heat_permille": 1000,
      "lock_break_bonus_permille": 220,
      "detection_reduction_permille": 120,
      "counter_detection_risk_permille": 120,
      "condition_wear_per_active_tick": 4,
      "cooldown_ticks": 240,
      "item_id": "item_ecm_jammer_module"
    }
  ],
  "mode_rules": {
    "passive_signature": { "power_draw": "low",   "lock_break_bonus_permille": 0,   "counter_detection_risk_permille": 0 },
    "noise_jam":         { "power_draw": "high",  "lock_break_bonus_permille": 120, "counter_detection_risk_permille": 150 },
    "deception":         { "power_draw": "moderate", "false_track_pressure": 2,    "counter_detection_risk_permille": 60 },
    "burst_defense":     { "power_draw": "peak",  "lock_break_bonus_permille": 320, "counter_detection_risk_permille": 200, "duration_ticks": 12 }
  },
  "threat_profiles": [
    { "threat_profile_id": "threat_aerial_scanner", "display_name": "Aerial Scanner", "base_detection_permille": 400, "lock_rate_permille": 120 }
  ],
  "invariants": { "single_mode_lock_break_cap_permille": 400, "no_immunity": true }
}
```

## 9.3 `precision_broaching_catalog.json` (Plan 144)

```json
{
  "schema_version": 1,
  "bench": {
    "bench_id": "bench_broaching_station",
    "display_name": "Internal Broaching Station",
    "construction_required_items": { "scrap_metal": 12, "scrap_mechanical": 4, "item_hydraulic_ram_assembly": 1 },
    "construction_labor_days": 3,
    "max_condition": 100,
    "maintenance_interval_days": 6,
    "maintenance_required_items": { "scrap_mechanical": 2, "item_cutting_fluid_canister": 1 },
    "room_id": "room_workshop",
    "machine_load_class": "high"
  },
  "operations": [
    { "operation_id": "broach_internal_spline_medium", "display_name": "Internal Spline, Medium", "input_item_id": "item_machined_blank_medium", "tool_class": "spline_broach", "labor_ticks": 240, "tolerance_target": "precision", "output_item_id": "item_internal_spline_hub", "tool_wear_per_job": 12, "machine_load_permille": 700 },
    { "operation_id": "broach_keyed_collar_small", "input_item_id": "item_machined_blank_small", "tool_class": "keyway_broach", "labor_ticks": 150, "tolerance_target": "standard", "output_item_id": "item_keyed_actuator_collar", "tool_wear_per_job": 8, "machine_load_permille": 450 }
  ],
  "failure_states": ["tool_dull", "alignment_fault", "machine_overload", "tool_break", "cancelled"],
  "quality_tiers": [
    { "tier_id": "tolerance_standard", "quality_value": 0.6 },
    { "tier_id": "tolerance_precision", "quality_value": 0.85 },
    { "tier_id": "tolerance_masterwork", "quality_value": 1.0 }
  ]
}
```

## 9.4 `fog_harvesting_catalog.json` (Plan 145)

```json
{
  "schema_version": 1,
  "harvester_profiles": [
    { "profile_id": "fog_net_ridge_standard", "display_name": "Ridge Fog Net", "collection_area_units": 20, "passive": true, "wind_band": "moderate", "humidity_band": "high", "base_yield_units": 12, "wear_rate_per_day": 2, "contamination_profile": "surface_exposed", "output_water_type": "raw", "construction_required_items": { "scrap_wood": 6, "item_fog_mesh_roll": 2, "item_reinforced_support_cable": 2 }, "siting_tags": ["ridge", "exposed_slope"] },
    { "profile_id": "fog_array_powered_assist", "display_name": "Powered Mist-Assist Array", "collection_area_units": 30, "passive": false, "wind_band": "any", "humidity_band": "moderate", "base_yield_units": 20, "wear_rate_per_day": 3, "contamination_profile": "surface_exposed", "output_water_type": "raw", "power_draw_kwh_per_day": 5, "construction_required_items": { "item_fog_mesh_roll": 3, "item_powered_mist_assist_module": 1, "item_internal_spline_hub": 1 }, "siting_tags": ["ridge", "exposed_slope", "coastal_bluff"] }
  ],
  "fog_bands": {
    "presence_by_weather_kind": { "overcast": 0.7, "black_rain": 0.5, "rain": 0.3, "clear": 0.0, "ashfall": 0.1, "fallout_storm": 0.0, "blizzard": 0.0 },
    "wind_band_multipliers": { "calm": 0.6, "moderate": 1.0, "strong": 0.7 },
    "humidity_band_multipliers": { "low": 0.4, "moderate": 0.8, "high": 1.0 },
    "powered_assist_multiplier": 1.5
  },
  "storm_event": { "damage_condition_permille": 250, "disable_below_condition_permille": 200 },
  "buffer": { "max_units": 40 }
}
```

# 10. Feedstock / Item / Trait ID Authoring Lists

**New items (`item_` prefix; TIER-1 validator prefixes):**
- Plan 142: `item_crop_waste`, `item_biofuel_low_grade`, `item_biofuel_generator_grade`, `item_biofuel_high_grade`, `item_biofuel_catalyst_pack`, `item_separation_media_cartridge`, `item_column_packing_set`
- Plan 143: `item_ecm_jammer_module`, `item_signature_reduction_coating`, `item_high_power_transmitter_module`
- Plan 144: `item_machined_blank_small`, `item_machined_blank_medium`, `item_internal_spline_hub`, `item_keyed_actuator_collar`, `item_precision_valve_body`, `item_geared_sleeve`, `item_pump_coupling`, `item_broach_tool_set`, `item_precision_alignment_fixture`, `item_cutting_fluid_canister`, `item_hydraulic_ram_assembly`
- Plan 145: `item_fog_mesh_roll`, `item_powered_mist_assist_module`, `item_drainage_trough`, `item_reinforced_support_cable`

**Feedstock reuse (existing):** `scrap_wood`, `paper_stock`. **Feedstock source gap:** `item_crop_waste` requires a producer — Wave 3 adds a greenhouse waste yield (bounded per harvest, opportunity cost vs compost/feed; exact hook planned in the Plan 142 deep plan; kitchen/compost routing explicitly out of scope here).

**New traits (`trait_` prefix, registered like `trait_alchemist`):** `trait_biofuel_chemist`, `trait_distillery_master`, `trait_ew_officer`, `trait_rf_jamming_specialist`, `trait_master_broacher`, `trait_toolmaker`, `trait_meteorologist` (verify absent before authoring — a "Meteorologist" string exists in `survivors.json`; confirm it is not already a trait id), `trait_atmospheric_hydrologist`.
**Reuse first:** existing machinist-family skills/traits if a generic precision-work bonus already resolves; `Gunsmith`-flavored bonuses stay generic precision-work only.

# 11. Determinism & Save Conventions (shared, all four systems)

1. All stochastic rolls: **day-derived fresh seeds** (`new SeededRng(unchecked(day * prime + streamSalt))`, distinct salt per stream: `biofuel_fault`, `biofuel_yield`, `ecm_counter_detection`, `broach_failure`, `fog_storm`). No engine-carried RNG sequence across saves.
2. Resolved facts (failures, counter-detection, quality, storm damage) persist; never rerolled on restore.
3. `CaptureState/RestoreState` DTOs; unknown catalog refs under a saved state → safe fail (batch/job voided, inputs lost per authored rule — documented, tested).
4. Atomic transactions: validation → single commit; no partial deduction.
5. Tick ordering (per flagship plan): weather → power → ventilation/hazards → fog collection → water treatment → production → expedition/threats → combat → narrative → save. Fog reads same-day weather once; ECM updates threat state before attack resolution.

# 12. Wave 1 Deliverables & Gates

| Deliverable | Gate |
|---|---|
| Catalog loader types + empty-default catalogs (4 files) | `--data-integrity-selftest` 0 errors; loader unit tests |
| `SensorThreatState.cs` + `ISensorThreatQuery` | Unit tests: bounded modifier, no-immunity invariant, resolved-fact persistence |
| `FogPresenceEvaluator` contract (pure function) | Unit tests: zero-yield inputs, band math, determinism |
| `SaveSectionRegistry` entries + filename map | `MainTriadDriftGateTests` stays green (Save/Setup methods exist) |
| ID authoring (items/traits) | CatalogIntegrityValidator TIER-1/TIER-2 clean; dual-spelling check |
| Full suite + host build | 0 errors / 0 warnings; paired-run determinism tests for any roll added |

# 13. Out of Scope (Wave 1)

Per-system engines, panels, host sessions, balance tuning, narrative hooks, greenhouse waste producer design (Wave 3), weather-authority schema changes (Option B), content-utilization repair (separate PR per §8), UI-09 route registration (Waves 2–5 per panel, with headless gates).

# 14. Implementation Handoff

## MUST PRESERVE
- `WeatherSystem` read-only (Option A); `WaterTreatmentSystem` as sole water entry; `PowerGridSystem.AddFuel` as sole generator fuel route; ECM as modifier-only via `ISensorThreatQuery`.

## MUST ADD
- Four catalog JSON files (§9 shapes), `SensorThreatState.cs`, `FogPresenceEvaluator`, four `SaveSectionRegistry` entries + filename map, authored item/trait ids (§10), loader types with `CatalogDiagnostics` hardening.

## MUST NOT DO
- No real chemical/RF/high-voltage/machining-geometry parameters; no weapon-barrel content; no parallel fuel/water/pool authorities; no gameplay in Godot; no closeout citations of the hanging content-utilization gate.

## VERIFY WITH
- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj`; `dotnet build Ashfall.csproj`; `godot --headless --path . -- --data-integrity-selftest`; paired-run determinism tests; `MainTriadDriftGateTests`.

## FIRST SAFE IMPLEMENTATION STEP
- Add the four catalog JSON files + loader types + load tests (no runtime wiring), verify `--data-integrity-selftest` stays at 0 errors.
