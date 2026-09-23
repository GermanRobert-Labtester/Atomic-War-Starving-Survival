# PLAN-ENERGY-NUCLEAR-48 — Generation Portfolio, Grid, Storage & the Reactor

**Wave:** 5 (2026-09-21) · **Kind:** MAJOR EXPANSION
**Status:** PROPOSED — not a claim.
**Depends on:** PLAN-SHELTER-ARCHITECTURE-40 (subgrids), PLAN-INDUSTRY-AUTOMATION-45.
**Non-goals:** no real reactor engineering data, no radiological weapon detail,
no second grid (`PowerGridSystem` remains canonical).

---

## 1. Outcome

Power is the spine of the bunker and its catalogue is already authored:
`PowerGridSystem` + `PowerGridSave`, `PowerDistributionSubgridSystem`,
`ShelterPowerGridCatalog`, `PowerSubgridCatalog`, `NuclearCoreCatalog` +
`NuclearCoreLifecycleSystem`, `SofcElectrochemistryEngine` + `SofcPowerCatalog`,
`CvdDiamondSynthesisEngine`, `MechanicalPowerDrivelineEngine`,
`PowerLoadSheddingEngine`, `SteamTurbinePowerCatalog`, plus
`nuclear_core_profiles.json`, `sofc_power_catalog.json`, `power_grid.json`,
`power_subgrid_nodes.json`, `cvd_diamond_catalog.json`. Prior seals already
fixed SOFC fuel consumption from inventory and the nuclear core publish gate.

This plan makes energy a **portfolio the player operates**: choose sources,
balance a grid, store, ration, maintain, and keep one big dangerous machine
alive.

| Mechanic | Authority | Player action | Outcome |
|---|---|---|---|
| Generation | reactor, SOFC, steam, geothermal, solar/wind | build/start/stop | output, fuel use, heat |
| Grid | `PowerGridSystem`, subgrids | wire, isolate, set priority | delivery, brownouts |
| Storage | batteries/accumulators | charge/discharge | buffer, black-start |
| Fuel cycles | `SofcElectrochemistryEngine`, fuel quality | source, treat, feed | efficiency, contamination |
| Reactor | `NuclearCoreLifecycleSystem` | commission, run, maintain, decommission | output curve, safety envelope |
| Load | `PowerLoadSheddingEngine`, Power Board | ration by circuit | critical load protection |
| Industry | CVD diamond, electrolysis, drilling machinery | schedule heavy loads | products, peak demand |

---

## 2. Evidence

| Item | Detail |
|---|---|
| Core | `Shelter/PowerGridSystem.cs`, `PowerGridSave`, `PowerDistributionSubgridSystem`, `ShelterPowerGridCatalog`, `PowerSubgridCatalog`, `NuclearCoreCatalog`, `NuclearCoreLifecycleSystem`, `SofcElectrochemistryEngine`, `SofcPowerCatalog`, `CvdDiamondSynthesisEngine`, `MechanicalPowerDrivelineEngine`, `PowerLoadSheddingEngine`, `Narrative/SteamTurbinePowerCatalog.cs` |
| Data | `power_grid.json`, `power_subgrid_nodes.json`, `nuclear_core_profiles.json`, `sofc_power_catalog.json`, `cvd_diamond_catalog.json`, `electrostatic_filtration_catalog.json` |
| Sealed prior | SOFC inventory fuel (17/17 + 41/41), nuclear publish gate, Plan 40 subgrids, Plan 213 driveline wear |
| Contracts | fuel priority clean→treated→dirty with grid reserve fallback; no parallel grid; electrostatic filtration contract live |

---

## 3. Packages

### EG-48A — Generation portfolio
- Each source has a start/stop cost, ramp, fuel, heat, and failure mode; the
  portfolio is a player policy (base load vs surge), not automatic.
- **Acceptance:** output = f(condition, fuel, priority) visible; a source can
  fail safely; no hidden generation.
- **Verify:** `--power-grid-selftest` + focused.

### EG-48B — Grid and subgrids
- Extend Plan 40: circuit isolation, cross-ties, fault propagation, and
  black-start ordering; faults trip breakers instead of a silent brownout.
- **Acceptance:** a fault isolates to its circuit; critical loads protected by
  declared priority; repairs needed after trips.
- **Verify:** grid + shelter focused suites.

### EG-48C — Fuel cycles and quality
- SOFC fuel quality bands, reactor fuel batches with provenance, steam boiler
  fuel, and waste accumulation; handling routes through radiation/sanitation
  owners.
- **Acceptance:** fuel quality measurably changes efficiency; waste is a real
  burden; no duplicate fuel store.
- **Verify:** SOFC suite + radiation focused.

### EG-48D — Reactor lifecycle
- Commissioning steps, output curve, thermal envelope, maintenance windows,
  degradation, SCRAM, and decommissioning; safety interlocks from the authored
  profile; incident consequence routing to crisis/alerts.
- **Acceptance:** reactor is powerful and dangerous; interlocks prevent
  instant catastrophe from a single mistake; a serious incident is recoverable
  but costly.
- **Verify:** nuclear core publish tests + crisis suites.

### EG-48E — Storage and demand management
- Accumulators for peak shaving and black-start; load classes (life support,
  clinic, cold chain, industry) with policy bands; a visible reserve forecast.
- **Acceptance:** storage math explainable; reserve never silently negative;
  policy changes preview their effect.
- **Verify:** power focused + `--runtime-scale-selftest` (load phase).

### EG-48F — Industrial power coupling
- CVD diamond, electrolysis, drilling, and foundry lines consume large
  scheduled loads; the Power Board shows the conflict between product output
  and habitability.
- **Acceptance:** heavy loads are scheduleable; overlapping demand causes
  visible sheds; no free capacity.
- **Verify:** industry + grid suites.

### EG-48G — Content volumes
- +4 generation profiles, +6 storage rows, +6 fuel grades, +8 reactor
  lifecycle events, +6 fault scenarios; abstract/industrial; consumer-bound.

---

## 4. Risks

| Risk | Mitigation |
|---|---|
| Power micro-management | one board, policies, defaults; alerts only on thresholds |
| Reactor trivializes survival | fuel/waste/maintenance burden, safety interlocks, incident cost |
| Cascading failures frustrate | staged warnings, isolatable circuits, recovery drills |
| Second grid creep | subgrids remain the same authority with new nodes |

## 5. Verification

```bash
godot --headless --path . -- --power-grid-selftest
godot --headless --path . -- --runtime-scale-selftest
bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/
bash scripts/run_test.sh Ashfall.Core.Tests/Plans122to125/
```

---

## 6. Expanded census (11 files · 3,864 lines)

Scope: `Assets/Ashfall.Core/Shelter/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Catalog 4 · DTO/Type 2 · Save 1 · System 4

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `KineticStorageSystem.cs` | 615 | System | **yes** | 0 | 0 | 2 |
| `MechanicalPowerDrivelineEngine.cs` | 278 | System | — | 0 | 0 | 0 |
| `NuclearCoreCatalog.cs` | 141 | Catalog | — | 0 | 0 | 0 |
| `NuclearCoreLifecycleSystem.cs` | 318 | System | — | 0 | 0 | 2 |
| `PowerDistributionSubgridSystem.cs` | 339 | DTO/Type | — | 0 | 0 | 2 |
| `PowerGridSave.cs` | 101 | Save | — | 0 | 0 | 0 |
| `PowerGridSystem.cs` | 1302 | DTO/Type | — | 0 | 0 | 6 |
| `PowerLoadSheddingEngine.cs` | 212 | System | **yes** | 0 | 0 | 0 |
| `PowerSubgridCatalog.cs` | 62 | Catalog | — | 0 | 0 | 0 |
| `ShelterPowerGridCatalog.cs` | 199 | Catalog | — | 0 | 0 | 0 |
| `SofcPowerCatalog.cs` | 297 | Catalog | — | 0 | 0 | 0 |

**Totals:** 0 banned refs · 0 empty catches · 4 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `kinetic_flywheel_catalog.json` | object[5 keys] |
| `power_subgrid_nodes.json` | object[2 keys] |
| `nuclear_core_profiles.json` | object[2 keys] |
| `power_grid.json` | object[7 keys] |
| `sofc_power_catalog.json` | object[9 keys] |
| `nuclear_winter_phases.json` | object[3 keys] |

**State surfaces:** `KineticStorageSystem.cs`, `NuclearCoreLifecycleSystem.cs`, `PowerDistributionSubgridSystem.cs`, `PowerGridSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Shelter/` |
| Test references | 59 name references across the test tree |
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

## 11. Tier-2: intra-domain reference graph

Computed across 86 domain files: **106 type-reference edges**.

| File | Lines | In-degree | Out-degree |
|---|---:|---:|---:|
| `PowerGridSystem.cs` | 1302 | 8 | 1 |
| `AquiferPiezometerEngine.cs` | 829 | 0 | 1 |
| `AquaponicsSystem.cs` | 813 | 0 | 1 |
| `BioFermentationEngine.cs` | 786 | 1 | 2 |
| `SanitationSystem.cs` | 754 | 1 | 2 |
| `FluidLogisticsSystem.cs` | 695 | 5 | 0 |
| `PrecisionMetrologySystem.cs` | 671 | 1 | 2 |
| `ShelterExpansionSystem.cs` | 665 | 0 | 0 |
| `ShelterMachineTellCatalog.cs` | 654 | 3 | 1 |
| `ShelterWorkshopSystem.cs` | 634 | 2 | 1 |
| `PneumaticDispatchSystem.cs` | 628 | 0 | 1 |
| `KineticStorageSystem.cs` | 615 | 1 | 1 |

**Highest-coupling files (in×2 + out):**

- `CvdDiamondSynthesisEngine.cs` — in 26, out 6
- `PowerGridSystem.cs` — in 8, out 1
- `CvdDiamondCatalog.cs` — in 6, out 0
- `CarbonCompositeCatalog.cs` — in 5, out 0
- `FischerTropschCatalog.cs` — in 5, out 0
- `FluidLogisticsSystem.cs` — in 5, out 0
- `GeothermalAquiferSystem.cs` — in 2, out 5
- `SeismicDynamicsSystem.cs` — in 3, out 3
- `FischerTropschSynthesisEngine.cs` — in 1, out 6
- `SeismicDynamicsSystem.Monitoring.cs` — in 2, out 4

**Ordering implication:** wire in-dependent files first (high in-degree, low
out-degree), then the terminal consumers. A file with many outgoing edges is a
dependency: it should be sealed or verified before its dependents claim work.

---

## 12. Cross-plan coupling

Domain files: 11. Other plans referencing their names: **6**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-VERTICAL-BODY-INDUSTRY-05` | 3 |
| `PLAN-ORPHAN-SEAL-01` | 2 |
| `EVIDENCE` | 2 |
| `PLAN-INDUSTRY-AUTOMATION-45` | 2 |
| `PLAN-SHELTER-ARCHITECTURE-40` | 1 |
| `PLAN-KINETIC-STORAGE-TRUTH-181` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `EG-48A` | no name match — resolve at claim time |
| `EG-48B` | `PowerDistributionSubgridSystem.cs` |
| `EG-48C` | `NuclearCoreLifecycleSystem.cs` |
| `EG-48D` | `NuclearCoreLifecycleSystem.cs` |
| `EG-48E` | `KineticStorageSystem.cs` |
| `EG-48F` | `MechanicalPowerDrivelineEngine.cs`, `PowerDistributionSubgridSystem.cs`, `PowerGridSave.cs` |
| `EG-48G` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 28. Host files: **42** · Test files: **79** · Data files: **1**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 42 | `src/Audio/AudioEventBridge.cs`, `src/Audio/AudioSelfTest.cs`, `src/Audio/ShelterAudioController.cs`, `src/Audio/ShelterOperationsAudioBridge.cs`, `src/Foundry/SilentFoundryHostSession.cs` |
| Tests (`Ashfall.Core.Tests/`) | 79 | `Ashfall.Core.Tests/Balance/ShelterOperationsBalanceSim.cs`, `Ashfall.Core.Tests/BalanceFedLoopTelemetryTests.cs`, `Ashfall.Core.Tests/BalancePowerEconomyTests.cs`, `Ashfall.Core.Tests/CampaignContinuityFlagship54_57Tests.cs`, `Ashfall.Core.Tests/CampaignContinuityFlagshipB70_B73Tests.cs` |
| Data (`StreamingAssets/Data/`) | 1 | `Assets/StreamingAssets/Data/shelter_machine_identities.json` |

**Verdict:** all three layers attach — surface is bound end to end.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **35** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `aquaponics` |
| `bio_fermentation` |
| `chemical_synthesis` |
| `chlor_alkali_synthesis` |
| `cvd_diamond` |
| `expanded_shelter` |
| `fluid_logistics` |
| `geothermal_aquifer` |
| `geothermal_orc` |
| `kinetic_storage` |
| `low_background_metrology` |
| `nuclear_core_lifecycle` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **24** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--aquaponics-selftest` |
| `--carbon-composite-selftest` |
| `--cvd-diamond-selftest` |
| `--expedition-panel-lifecycle` |
| `--panel-bind-lifecycle-selftest` |
| `--panel-lifecycle-selftest` |
| `--precision-metrology-selftest` |
| `--save-load-failure-selftest` |
| `--save-load-failure-uitest` |
| `--save-load-selftest` |
| `--save-load-ui-failure-selftest` |
| `--shelter-actor-physics-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **4**.

| Event | First declaration |
|---|---|
| `OnShelterEncounterResolved` | `Assets/Ashfall.Core/DutyRoster/ShelterEncounterSystem.cs` |
| `OnShelterEncounterStarted` | `Assets/Ashfall.Core/DutyRoster/ShelterEncounterSystem.cs` |
| `OnShelterFalseAlarm` | `Assets/Ashfall.Core/Survivors/CombatTraumaSystem.cs` |
| `OnWorkshopStateChanged` | `Assets/Ashfall.Core/WorkshopReverseEngineeringSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/aquaponics_system_catalog.json` |
| `Assets/StreamingAssets/Data/bio_fermentation_catalog.json` |
| `Assets/StreamingAssets/Data/carbon_composite_catalog.json` |
| `Assets/StreamingAssets/Data/chlor_alkali_synthesis_catalog.json` |
| `Assets/StreamingAssets/Data/cvd_diamond_catalog.json` |
| `Assets/StreamingAssets/Data/fischer_tropsch_catalog.json` |
| `Assets/StreamingAssets/Data/fluid_infrastructure.json` |
| `Assets/StreamingAssets/Data/geothermal_drilling_depths.json` |
| `Assets/StreamingAssets/Data/geothermal_strata_catalog.json` |
| `Assets/StreamingAssets/Data/kinetic_flywheel_catalog.json` |
| `Assets/StreamingAssets/Data/metrology_standards_catalog.json` |
| `Assets/StreamingAssets/Data/mineral_acid_synthesis_catalog.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **2** (88 files, 759 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Lifecycle` | 1 | 5 |
| `Shelter` | 87 | 754 |

**Verdict:** 759 cases sit under matching regions — run those first (`Lifecycle`, `Shelter`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **104**
(26 of them panels/HUD).

| Host file |
|---|
| `src/Audio/ShelterAcousticBridge.cs` |
| `src/Audio/ShelterAudioController.cs` |
| `src/Audio/ShelterOperationsAudioBridge.cs` |
| `src/Host/AquaponicsSaveStore.cs` |
| `src/Host/BioFermentationHostSession.cs` |
| `src/Host/BioFermentationSaveStore.cs` |
| `src/Host/ChemicalSynthesisHostSession.cs` |
| `src/Host/ChemicalSynthesisSaveStore.cs` |
| `src/Host/CvdDiamondHostSession.cs` |
| `src/Host/CvdDiamondSaveStore.cs` |
| `src/Host/FluidLogisticsHostSession.cs` |
| `src/Host/FluidLogisticsSaveStore.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **35**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `aquaponics` | no |
| `bio_fermentation` | no |
| `chemical_synthesis` | no |
| `chlor_alkali_synthesis` | no |
| `cvd_diamond` | no |
| `expanded_shelter` | no |
| `fluid_logistics` | no |
| `geothermal_aquifer` | no |
| `geothermal_orc` | no |
| `kinetic_storage` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **8**.

| Stream |
|---|
| `aquaponics_disease` |
| `aquaponics_fry_survival` |
| `cvd_diamond` |
| `low_background_metrology` |
| `metrology_calibration_drift` |
| `metrology_measurement_noise` |
| `shelter` |
| `sofc_power` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **31**
(CODEX_ONLY 16, GAMEPLAY_CONSUMED 7, UNRESOLVED 8).

| Catalog | Classification |
|---|---|
| `chlor_alkali_synthesis_catalog.json` | UNRESOLVED |
| `geothermal_drilling_depths.json` | UNRESOLVED |
| `kinetic_flywheel_catalog.json` | GAMEPLAY_CONSUMED |
| `mineral_acid_synthesis_catalog.json` | UNRESOLVED |
| `narrative/activated_carbon_adsorption_records.json` | CODEX_ONLY |
| `narrative/blast_gate_mechanical_audits.json` | CODEX_ONLY |
| `narrative/fermentation_crock_airlock_assays.json` | CODEX_ONLY |
| `narrative/geothermal_borehole_logs.json` | CODEX_ONLY |
| `narrative/geothermal_steam_vent_diagnostics.json` | CODEX_ONLY |
| `narrative/geothermal_steam_well_logs.json` | CODEX_ONLY |

**Verdict:** 8 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **0**.

| Flag |
|---|
| — | no persistent flag shares a token with this domain |

**Verdict:** no persistent flag matches — the domain's narrative consequences are carried by other state (or the flag set is a candidate for cleanup).

---

## 24. Claim readiness

**Readiness:** READY-WITH-NOTES (11/12) · **Class:** standard · **Coupling (incoming plans):** 6
**Surface:** save sections 35 (laddered 0) · RNG streams 8 · host files 20 · catalogs 22 · test regions 2 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-ENERGY-NUCLEAR-48
wave: —
status: PROPOSED — foreman claim required
packages: EG-48A, EG-48B, EG-48C, EG-48D, EG-48E, EG-48F, EG-48G
claim paths:
  - src/Audio/ShelterAcousticBridge.cs  # §19 candidate host surface
  - src/Audio/ShelterAudioController.cs  # §19 candidate host surface
  - src/Audio/ShelterOperationsAudioBridge.cs  # §19 candidate host surface
  - src/Host/AquaponicsSaveStore.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/aquaponics_system_catalog.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/bio_fermentation_catalog.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Lifecycle/
  - godot --headless --path . -- --aquaponics-selftest
dependencies:
  - coordinate: 6 other plan(s) name these artifacts (§12)
  - governance spine must land first: 56 → 86 → 17 → 100
```

**Structural checklist**

| Check | Result |
|---|---|
| status | yes |
| wave | **no** |
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

**Pre-claim actions:** author or confirm: wave.
