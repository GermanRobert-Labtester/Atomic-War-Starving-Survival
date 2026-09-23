# PLAN-WEATHER-ATMOSPHERE-28 — Forecast Truth, Nuclear Winter & Weather Modification

**Wave:** 3 (2026-09-21) · **Kind:** EXPANSION
**Status:** PROPOSED — not a claim. Foreman claim required.
**Depends on:** PLAN-ORPHAN-SEAL-01 Wave 6, PLAN-DETERMINISM-REPLAY-13.
**Expanded appendix:** [`PLAN-WEATHER-ATMOSPHERE-28_APPENDIX-A_ORPHAN_DOSSIERS.md`](PLAN-WEATHER-ATMOSPHERE-28_APPENDIX-A_ORPHAN_DOSSIERS.md)
— domain-filtered orphan dossiers for this plan's weather & atmosphere
systems (5 authorities), each mapped to its parent-plan mechanic row.

**Non-goals:** no second forecast authority (`WeatherStationSystem` remains
sole), no real meteorological service data, no per-frame weather simulation.

---

## 1. Outcome

Weather currently threatens; this plan makes it **predictable, modifiable, and
consequential across decades**. The corpus has the pieces: `WeatherSystem`,
`WeatherStationSystem` (forecast authority per `DEC-15`), `FalloutSystem`,
`CloudSeedingSystem`, `AnomalyHazardSystem`, `AtmosphericSoundingCatalog`,
`weather_hardening_upgrades.json`, plus the host-unreachable
`WeatherForecastReliabilityEngine`, `StormForecastReadinessEngine`,
`WeatherCascadeSystem`, `WeatherGameplayCascadeEngine`, and
`NuclearWinterProgressionSystem`.

Player loop: **forecast → prepare (hardening, routes, stores) → endure →
measure → adapt to the multi-decade winter**.

| Mechanic | Authority | Player action | Outcome |
|---|---|---|---|
| Forecast reliability | `WeatherForecastReliabilityEngine`, `WeatherStationSystem` | calibrate station, read forecast | accuracy band visible; wrong forecasts have cost |
| Forecast → decisions | `StormForecastReadinessEngine`, `weather_route_gates.json` | pre-position, reroute, delay | readiness score, route closures |
| Nuclear winter | `NuclearWinterProgressionSystem`, `nuclear_winter_phases.json` | adapt heat/food/greenhouse | phase transitions, season severity |
| Fallout | `FalloutSystem`, `fallout_patterns.json` | shelter, decontaminate, store | deposition, dose, crop damage |
| Weather modification | `CloudSeedingSystem` | seed clouds, suppress | bounded change with cost/risk |
| Soundings | `AtmosphericSoundingCatalog` | launch soundings | data improves forecast reliability |
| Hardening | `weather_hardening_upgrades.json` | upgrade structures | damage reduction, storm survival |

---

## 2. Evidence

| Item | Detail |
|---|---|
| Host-unreachable authorities | `WeatherForecastReliabilityEngine`, `StormForecastReadinessEngine`, `WeatherCascadeSystem`, `WeatherGameplayCascadeEngine`, `NuclearWinterProgressionSystem`, `CloudSeedingSystem` (6) |
| Data catalogs | 11 weather/atmosphere files (season, effects, phases, hardening, route gates, storm windows, soundings, fallout patterns, profiles) |
| Sealed prior work | Plan 164 (5 phases/4 seasons), Plan 135 (15 cascade templates), Plan 48 (15 route gates), Plan 205 (wind authority), Plan 169 (audio accessibility) |
| Baselines | `DEC-07` radiation/dose baselines, `DEC-15` forecast authority |
| Selftests | `--weather-selftest`, `--dynamic-world-selftest` |

---

## 3. Packages

### WA-28A — Forecast reliability and calibration
- Bind `WeatherForecastReliabilityEngine` to the station owner; every forecast
  carries an accuracy band; station condition/calibration/sounding data shifts
  the band. `StormForecastReadinessEngine` converts forecasts into a readiness
  score per shelter/route.
- **Acceptance:** a calibrated station measurably improves forecast accuracy
  over a seeded 30-day run; wrong forecasts are explainable (band shown before
  the event).
- **Verify:** `bash scripts/run_test.sh Ashfall.Core.Tests/Weather/`;
  `godot --headless --path . -- --weather-selftest`.

### WA-28B — Nuclear winter progression
- `NuclearWinterProgressionSystem` drives temperature, daylight, season
  severity, heating demand, greenhouse multipliers, and severe anomalies
  through the existing weather/crop/heating owners; `phase_*` catalog is the
  authority.
- **Acceptance:** phase transitions fire once with journal/radio coverage;
  crop preservation multipliers applied through `GreenhouseSystem`; no
  parallel climate scalar.
- **Verify:** `bash scripts/run_test.sh Ashfall.Core.Tests/Architecture/` (Plan 164 tests) + focused.

### WA-28C — Fallout, ash and decontamination
- `FalloutSystem` + `fallout_patterns.json` deposit contamination on shelter,
  water, soil, and items by region; `DecontaminationSystem` and
  `WaterTreatmentSystem` consume it through canonical paths; ashfall affects
  solar power and visibility.
- **Acceptance:** deposition is measurable, dose consequences bounded, all
  removal through existing owners; no second contamination store.
- **Verify:** radiation + shelter focused suites.

### WA-28D — Weather modification (cloud seeding)
- `CloudSeedingSystem` becomes a bounded action: cost (materials/aircraft or
  sounding team), success band, and consequences (rain now, drought/hail
  risk later, faction/neighbour perception). No unbounded weather control.
- **Acceptance:** seeding produces a documented, reversible change within one
  window; determinism; failure has a cost.
- **Verify:** focused seeding tests + balance sim.

### WA-28E — Forecast → decision surface
- Route gates (`weather_route_gates.json`) and expedition planning consume the
  forecast band; the map/briefing shows closure risk ahead. Shelter readiness
  shows hardening gaps before the storm.
- **Acceptance:** every gate is forecast-visible; a player who reads the
  forecast can avoid the loss; no hidden closures.
- **Verify:** `--weather-selftest` extended + expedition suites.

### WA-28F — Hardening and storm readiness
- `weather_hardening_upgrades.json` rows become shelter upgrades with
  condition/maintenance cost (routed through shelter maintenance owner);
  damage mitigation verified against cascade damage.
- **Acceptance:** upgrades reduce cascade damage by the authored band; no free
  repair; instability/alert rules preserved.
- **Verify:** shelter focused suites + cascade tests.

### WA-28G — Content volumes
- +6 weather effects, +4 fallout patterns, +4 soundings, +6 hardening rows,
  +4 seeding scenarios; all with consumers; data + content gates green.

---

## 4. Risks

| Risk | Mitigation |
|---|---|
| Cascade magnification after wiring all consumers | authored damage bands + `DEC-07` baselines; balance sim before tuning |
| Forecast shown as certain | band display is mandatory; tests assert the band moves |
| Cloud seeding becomes weather god-mode | bounded window, cost, and side effects authored |
| Nuclear winter too punishing early | phase pacing from catalog (multi-decade), not day 1 |

## 5. Verification

```bash
godot --headless --path . -- --weather-selftest
godot --headless --path . -- --data-integrity-selftest
bash scripts/run_test.sh Ashfall.Core.Tests/Weather/
bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/
```

---

## 6. Expanded census (25 files · 5,512 lines)

Scope: `Assets/Ashfall.Core/Weather/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Catalog 4 · DTO/Type 1 · Loader 2 · Support 9 · System 9

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `NuclearWinterProgressionSystem.cs` | 472 | System | **yes** | 0 | 0 | 2 |
| `WeatherCascadeSystem.cs` | 78 | System | **yes** | 0 | 0 | 4 |
| `WeatherGameplayCascadeEngine.cs` | 438 | System | — | 0 | 0 | 2 |
| `StormForecastReadinessEngine.cs` | 276 | System | — | 0 | 0 | 0 |
| `WeatherAtmosphereMap.cs` | 117 | Support | — | 0 | 0 | 0 |
| `WeatherEffectsCatalog.cs` | 199 | Catalog | — | 0 | 0 | 0 |
| `WeatherForecastReliabilityEngine.cs` | 146 | System | **yes** | 0 | 0 | 0 |
| `WeatherGate.cs` | 94 | Support | — | 0 | 0 | 0 |
| `WeatherGateCatalog.cs` | 194 | Catalog | — | 0 | 0 | 0 |
| `WeatherGateCatalogLoader.cs` | 193 | Loader | — | 0 | 0 | 0 |
| `WeatherGateContextEvaluator.cs` | 228 | Support | — | 0 | 0 | 0 |
| `WeatherGateContextModifier.cs` | 45 | Support | — | 0 | 0 | 0 |
| `WeatherGateEvaluationContext.cs` | 49 | Support | — | 0 | 0 | 0 |
| `WeatherGateEvaluator.cs` | 438 | Support | — | 0 | 0 | 0 |
| `WeatherGateFile.cs` | 45 | Support | — | 0 | 0 | 0 |
| `WeatherGateRadioHooks.cs` | 207 | Support | — | 0 | 0 | 0 |
| `WeatherGateResult.cs` | 35 | Support | — | 0 | 0 | 0 |
| `WeatherHardeningCatalog.cs` | 65 | Catalog | — | 0 | 0 | 0 |
| `WeatherHardeningCatalogLoader.cs` | 34 | Loader | — | 0 | 0 | 0 |
| `WeatherHardeningState.cs` | 39 | DTO/Type | — | 0 | 0 | 0 |
| `WeatherHardeningSystem.cs` | 351 | System | — | 0 | 0 | 2 |
| `WeatherIntelligenceCoordinator.cs` | 356 | System | — | 0 | 0 | 10 |
| `WeatherRouteGateCatalog.cs` | 188 | Catalog | — | 0 | 0 | 0 |
| `WeatherSondeSystem.cs` | 692 | System | — | 0 | 0 | 2 |
| `WeatherSystem.cs` | 533 | System | — | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 7 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `weather_hardening_upgrades.json` | object[2 keys] |
| `weather_route_gates.json` | object[2 keys] |
| `year_of_ash_storm_windows.json` | array[14] |
| `weather_effects.json` | object[2 keys] |
| `weather_seasons.json` | object[5 keys] |
| `cascade_rules.json` | object[3 keys] |

**State surfaces:** `NuclearWinterProgressionSystem.cs`, `WeatherCascadeSystem.cs`, `WeatherGameplayCascadeEngine.cs`, `WeatherHardeningSystem.cs`, `WeatherIntelligenceCoordinator.cs`, `WeatherSondeSystem.cs`, `WeatherSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Weather/` |
| Test references | 102 name references across the test tree |
| Determinism | 0 banned refs to fix or justify |
| Failures | 0 empty-catch sites routed through Plan 35's rules |
| Premise | the **premise** file(s) above must match the plan's stated line counts before edits |

## 9. Rollout sequence

1. Premise re-check: premise files unchanged since authoring (hash/mtime), or update the plan.
2. Interfaces: wire through the named owner; do not add a parallel store.
3. State: if capture/restore exists, register per Plan 1 Appendix Q; else state the system is stateless.
4. Data: resolve domain catalogs or report the loader path.
5. Verification: focused region + the plan's own acceptance table.
6. Regression: re-run this census; a changed file is a finding.

## 10. Acceptance matrix

| Deliverable class | Acceptance |
|---|---|
| Premise file | matches its stated surface; no scope drift |
| Interface/wiring | one owner per state, no parallel store |
| State/save | round-trip or explicit stateless verdict |
| Data binding | catalog resolves or loader path documented |
| Verification | focused region green; census delta recorded |

**Non-goals unchanged:** this expansion adds census and verification detail; it does not widen the plan's scope.

---

## 12. Cross-plan coupling

Domain files: 25. Other plans referencing their names: **8**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-WORLD-FAMILY-TRUTH-267` | 14 |
| `PLAN-ORPHAN-SEAL-01` | 5 |
| `EVIDENCE` | 5 |
| `PLAN-WEATHER-INTELLIGENCE-TRUTH-218` | 5 |
| `PLAN-SPATIAL-SIM-AUTHORITY-95` | 3 |
| `PLAN-WEATHER-SONDE-TRUTH-168` | 2 |
| `PLAN-SIGNALS-REMOTE-SENSING-49` | 1 |
| `PLAN-CASCADE-COORDINATOR-TRUTH-249` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `WA-28A` | `WeatherForecastReliabilityEngine.cs`, `StormForecastReadinessEngine.cs` |
| `WA-28B` | `NuclearWinterProgressionSystem.cs` |
| `WA-28C` | no name match — resolve at claim time |
| `WA-28D` | `WeatherCascadeSystem.cs`, `WeatherGameplayCascadeEngine.cs`, `WeatherAtmosphereMap.cs` |
| `WA-28E` | `StormForecastReadinessEngine.cs`, `WeatherForecastReliabilityEngine.cs` |
| `WA-28F` | `StormForecastReadinessEngine.cs`, `WeatherHardeningCatalog.cs`, `WeatherHardeningCatalogLoader.cs` |
| `WA-28G` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 11. Intra-domain reference graph (tier-2)

Domain files: 25; intra-domain edges: **28**; isolated files:
**5**.

**Top edges (by source name):**

| From | → To |
|---|---|
| `StormForecastReadinessEngine` | `WeatherGate` |
| `StormForecastReadinessEngine` | `WeatherSystem` |
| `WeatherCascadeSystem` | `WeatherGameplayCascadeEngine` |
| `WeatherEffectsCatalog` | `WeatherSystem` |
| `WeatherGateCatalog` | `WeatherGate` |
| `WeatherGateCatalog` | `WeatherGateCatalogLoader` |
| `WeatherGateCatalogLoader` | `WeatherGateCatalog` |
| `WeatherGateCatalogLoader` | `WeatherGateEvaluator` |
| `WeatherGateCatalogLoader` | `WeatherGateFile` |
| `WeatherGateContextEvaluator` | `WeatherGate` |
| `WeatherGateContextEvaluator` | `WeatherGateContextModifier` |
| `WeatherGateContextEvaluator` | `WeatherGateEvaluationContext` |
| `WeatherGateContextEvaluator` | `WeatherGateEvaluator` |
| `WeatherGateEvaluationContext` | `WeatherGateContextEvaluator` |
| `WeatherGateEvaluator` | `WeatherGate` |
| `WeatherGateEvaluator` | `WeatherGateCatalog` |
| `WeatherGateFile` | `WeatherGate` |
| `WeatherGateFile` | `WeatherGateCatalog` |
| `WeatherGateFile` | `WeatherGateCatalogLoader` |
| `WeatherGateRadioHooks` | `WeatherGateEvaluator` |
| `WeatherGateRadioHooks` | `WeatherSystem` |
| `WeatherHardeningCatalogLoader` | `WeatherHardeningCatalog` |
| `WeatherHardeningSystem` | `WeatherHardeningCatalog` |
| `WeatherHardeningSystem` | `WeatherHardeningState` |
| `WeatherHardeningSystem` | `WeatherSystem` |
| `WeatherIntelligenceCoordinator` | `WeatherSystem` |
| `WeatherSondeSystem` | `WeatherSystem` |
| `WeatherSystem` | `WeatherEffectsCatalog` |

**In-degree hubs:**

| File | inbound refs |
|---|---:|
| `WeatherSystem` | 6 |
| `WeatherGate` | 5 |
| `WeatherGateCatalog` | 3 |
| `WeatherGateEvaluator` | 3 |
| `WeatherGateCatalogLoader` | 2 |
| `WeatherHardeningCatalog` | 2 |
| `WeatherEffectsCatalog` | 1 |
| `WeatherGameplayCascadeEngine` | 1 |
| `WeatherGateContextEvaluator` | 1 |
| `WeatherGateContextModifier` | 1 |

**Class split:** hub 8 · sink 5 · source 7 · isolated 5.

**Reading:** sinks are the domain's load-bearing leaves (nothing inside depends
on them); hubs are rewiring risk — changing one fans out across the domain.
Isolated files are integration candidates: they compile but no sibling calls
them, so their value must come from the host adapter or the save path.

---

## 13. Authority binding map

Symbols used: 25. Host files: **26** · Test files: **53** · Data files: **2**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 26 | `src/Audio/AudioEventBridge.cs`, `src/Audio/AudioSelfTest.cs`, `src/Audio/SurfaceAmbienceController.cs`, `src/Host/ContentUtilizationRuntimeCollector.cs`, `src/Host/HostCli.DynamicWorld.cs` |
| Tests (`Ashfall.Core.Tests/`) | 53 | `Ashfall.Core.Tests/AudioEventIntegrationTests.cs`, `Ashfall.Core.Tests/Campaign/CrisisPredictionTests.cs`, `Ashfall.Core.Tests/CampaignContinuityFlagshipB70_B73Tests.cs`, `Ashfall.Core.Tests/Core/DeterminismSeedSweepTests.cs`, `Ashfall.Core.Tests/CrisisPresentationCoordinatorTests.cs` |
| Data (`StreamingAssets/Data/`) | 2 | `Assets/StreamingAssets/Data/slice_seven_days.json`, `Assets/StreamingAssets/Data/standing_gates.json` |

**Verdict:** all three layers attach — surface is bound end to end.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **8** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `counter_intelligence` |
| `nuclear_core_lifecycle` |
| `radio` |
| `radio_program_production` |
| `radio_station` |
| `route_infrastructure` |
| `shelter_atmosphere` |
| `weather_hardening` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **7** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--atmosphere-selftest` |
| `--deep-coast-route-selftest` |
| `--journal-weather-panel-selftest` |
| `--radio-catalog-selftest` |
| `--radio-selftest` |
| `--shelter-atmosphere-selftest` |
| `--weather-save-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **7**.

| Event | First declaration |
|---|---|
| `OnCraftResultOverflow` | `Assets/Ashfall.Core/Crafting/CraftingSystem.cs` |
| `OnForecastConfidenceChanged` | `Assets/Ashfall.Core/World/WeatherSondeSystem.cs` |
| `OnForecastUpdated` | `Assets/Ashfall.Core/WeatherStationSystem.cs` |
| `OnSondeFailed` | `Assets/Ashfall.Core/World/WeatherSondeSystem.cs` |
| `OnSondeRecovered` | `Assets/Ashfall.Core/World/WeatherSondeSystem.cs` |
| `OnWeatherChanged` | `Assets/Ashfall.Core/World/WeatherSystem.cs` |
| `OnWeatherFrontArrived` | `Assets/Ashfall.Core/Weather/WeatherCascadeSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/atmosphere_profiles.json` |
| `Assets/StreamingAssets/Data/cascade_rules.json` |
| `Assets/StreamingAssets/Data/environmental_atmosphere_expansion.json` |
| `Assets/StreamingAssets/Data/faction_intelligence.json` |
| `Assets/StreamingAssets/Data/faction_radio_corpus.json` |
| `Assets/StreamingAssets/Data/faction_war_radio.json` |
| `Assets/StreamingAssets/Data/narrative/blast_gate_mechanical_audits.json` |
| `Assets/StreamingAssets/Data/narrative/expedition_route_waypoint_notes_batch_2.json` |
| `Assets/StreamingAssets/Data/narrative/personal_effects_inventory_batch_2.json` |
| `Assets/StreamingAssets/Data/narrative/radio_broadcast_rundowns.json` |
| `Assets/StreamingAssets/Data/narrative/radio_mysteries_expansion.json` |
| `Assets/StreamingAssets/Data/narrative/radio_scriptbook.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **3** (60 files, 448 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Progression` | 11 | 83 |
| `Radio` | 47 | 354 |
| `Weather` | 2 | 11 |

**Verdict:** 448 cases sit under matching regions — run those first (`Progression`, `Radio`, `Weather`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **39**
(12 of them panels/HUD).

| Host file |
|---|
| `src/Audio/AudioStateCoordinator.cs` |
| `src/Host/CounterIntelligenceHostSession.cs` |
| `src/Host/CounterIntelligenceSaveStore.cs` |
| `src/Host/FactionIconLoader.cs` |
| `src/Host/GodotFileIO.cs` |
| `src/Host/LoaderWiringSelfTest.cs` |
| `src/Host/NuclearCoreSaveStore.cs` |
| `src/Host/RadioCatalogSelfTest.cs` |
| `src/Host/RadioHostSession.cs` |
| `src/Host/RadioProgramProductionHostSession.cs` |
| `src/Host/RadioProgramProductionSaveStore.cs` |
| `src/Host/RadioSaveStore.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **8**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `counter_intelligence` | no |
| `nuclear_core_lifecycle` | no |
| `radio` | no |
| `radio_program_production` | no |
| `radio_station` | no |
| `route_infrastructure` | no |
| `shelter_atmosphere` | no |
| `weather_hardening` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **4**.

| Stream |
|---|
| `radio` |
| `route_engineering_mine_flail` |
| `route_engineering_rail_grinding` |
| `weather` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **27**
(CODEX_ONLY 11, GAMEPLAY_CONSUMED 12, UNRESOLVED 4).

| Catalog | Classification |
|---|---|
| `environmental_atmosphere_expansion.json` | GAMEPLAY_CONSUMED |
| `faction_radio_corpus.json` | GAMEPLAY_CONSUMED |
| `faction_war_radio.json` | GAMEPLAY_CONSUMED |
| `narrative/blast_gate_mechanical_audits.json` | CODEX_ONLY |
| `narrative/expedition_route_waypoint_notes_batch_2.json` | CODEX_ONLY |
| `narrative/personal_effects_inventory_batch_2.json` | CODEX_ONLY |
| `narrative/radio_broadcast_rundowns.json` | CODEX_ONLY |
| `narrative/radio_mysteries_expansion.json` | CODEX_ONLY |
| `narrative/radio_scriptbook.json` | CODEX_ONLY |
| `narrative/radio_scripts_expansion.json` | CODEX_ONLY |

**Verdict:** 4 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **0**.

| Flag |
|---|
| — | no persistent flag shares a token with this domain |

**Verdict:** no persistent flag matches — the domain's narrative consequences are carried by other state (or the flag set is a candidate for cleanup).

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 8
**Surface:** save sections 8 (laddered 0) · RNG streams 4 · host files 16 · catalogs 22 · test regions 3 · flags 7

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-WEATHER-ATMOSPHERE-28
wave: —
status: PROPOSED — foreman claim required
packages: WA-28A, WA-28B, WA-28C, WA-28D, WA-28E, WA-28F, WA-28G
claim paths:
  - src/Audio/AudioStateCoordinator.cs  # §19 candidate host surface
  - src/Host/CounterIntelligenceHostSession.cs  # §19 candidate host surface
  - src/Host/CounterIntelligenceSaveStore.cs  # §19 candidate host surface
  - src/Host/FactionIconLoader.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/atmosphere_profiles.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/cascade_rules.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Progression/
  - godot --headless --path . -- --atmosphere-selftest
dependencies:
  - coordinate: 8 other plan(s) name these artifacts (§12)
  - governance spine must land first: 56 → 86 → 17 → 100
```

**Structural checklist**

| Check | Result |
|---|---|
| status | yes |
| wave | yes |
| depends | yes |
| non_goals | yes |
| outcome | yes |
| evidence | yes |
| packages | yes |
| acceptance | yes |
| risks | yes |
| verification | yes |
| coupling | yes |
| binding | yes |

**Pre-claim actions:** none — claim-ready.
