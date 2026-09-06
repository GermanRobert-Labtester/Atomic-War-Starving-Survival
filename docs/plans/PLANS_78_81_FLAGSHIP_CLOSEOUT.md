# Plans 78–81 Flagship Closeout — Decontamination Airlocks, Geodetic Survey, Kinetic Storage & Chemical Reconnaissance

**Status:** Core, data, persistence, host wiring, and Wave-5 test matrix complete. Wave-6 UI panels deferred to `google-stitch` (flagged in `AGENTS.md`).
**Plan class:** Major systemic integration / infrastructure expansion / environmental survival hardening
**Execution:** Evidence-first, deterministic, data-driven, persistence-safe, CI-gated, presentation-decoupled

---

## Baseline (Wave 0)

- Commit family: pre-Plans-78 tree at 7,724 passing Core tests; 236 catalogs; data integrity 0 errors
- Baseline build: `Ashfall.Core` 0 errors; `Ashfall.csproj` 0 errors
- Canonical-owner recon completed for RadiationSystem, WaterTreatmentSystem, EquipmentConditionSystem, WastelandMapSystem, ExpeditionSystem, PowerGridSystem, PharmaLabSystem, AirlockSecuritySystem, and the pre-existing queue-based `DecontaminationSystem`

## Wave 1 — Data (4 catalogs, all `schema_version`-stamped, snake_case IDs)

| Catalog | Content | Validation |
|---|---|---|
| `decontamination_protocol_catalog.json` | 4 protocols (standard 4-stage, emergency rapid, equipment-only, maximum containment), effluent treatment params, gear-disposal rules | Loader-side: unique protocol/stage IDs, ordered stages, positive durations, 0–1 multipliers |
| `geodetic_survey_catalog.json` | **16 survey points** on real `loc_*` nodes (peaks, ridges, tower ruins, datum monuments, bridge abutments, industrial chimneys, water towers, rail markers), equipment, 8 weather modifiers, triangulation bounds, navigation effects | Unique IDs, finite elevations, real `loc_*` references (data-integrity gated) |
| `kinetic_flywheel_catalog.json` | 4 rotor classes (500–4000 kg), 5 surge event profiles, black-start params, containment-hazard params | Mass/radius/RPM > 0, efficiency 0–1, unique IDs |
| `toxic_chemical_catalog.json` | **14 fictionalized hazard profiles** with normalized coefficients, detector bands, filter model, sample collection, map-overlay rules | Concentration 0–1, persistence 0–1, load rates ≥ 0, valid bands/categories |

20 new canonical items added to `items.json` (chelator concentrate, effluent filter, scrub brush, sealed waste bin, theodolite, stadia rod, datum plates, concrete mix, rotor shaft, bearing coils, vacuum pump, containment ring, vault sections, damper pads, pump oil, grease, balancing kit, PID detector, sensor modules, sample ampoules). All 4 catalogs registered with `CatalogBootValidator` and the `ContentUtilizationScanner` (see Wave 8).

## Wave 2 — DTOs & Persistence

- State DTOs with `CaptureState`/`RestoreState` on all four systems (`DecontaminationState` extension, `GeodeticSurveyState`, `KineticStorageState`, `ChemicalReconState`)
- Save sections registered in `SaveSectionRegistry`: `decontamination` (pre-existing), **`geodetic_survey`**, **`kinetic_storage`**, **`chemical_recon`** with `SectionFileNames` entries
- Host save stores (`src/Host/`): `GeodeticSurveySaveStore`, `KineticStorageSaveStore`, `ChemicalReconSaveStore` — thin façades over the Core `SaveStore<T>` service via `SaveStoreHub` + `SchemaVersionedEnvelope` (checksummed, atomic writes, legacy bare-state fallback). Satisfies `SaveStoreCoverageGateTests` delegation requirements
- Contract matrices updated: `ComprehensiveSaveStoreCorruptionAndMigrationTests` (118 sections), `ARCHITECTURE_TEST_MAP.md` rows 116–118
- Old-save defaults: idle decon / empty survey network / no flywheels / no recon data

## Wave 3 — Pure Core

| System | File | Owns | Does not own |
|---|---|---|---|
| Decon airlock extension | `Assets/Ashfall.Core/DecontaminationSystem.cs` | Multi-stage protocol cycles, effluent tank (volume/contamination/filter/sludge), chelator reserve consumption, interlock (`CanOpenInnerDoor`/`InnerDoorFailureReason`), logged manual override, sealed-bin gear disposal | Radiation dose, afflictions, global water inventory, equipment durability, door topology |
| Geodetic survey | `Assets/Ashfall.Core/World/GeodeticSurveyEngine.cs` | Monuments, observations, triangle resolution, network accuracy, shortcut knowledge, corridor drift/speed capabilities | Canonical map coordinates, expedition state, combat resolution, route graph |
| Kinetic storage | `Assets/Ashfall.Core/Shelter/KineticStorageSystem.cs` | Rotor RPM/energy (E=½Iω²), vacuum, bearing thermal, containment, surge buffering, black start, maintenance | Global power balance, generators, breakers, room integrity |
| Chemical recon | `Assets/Ashfall.Core/Expeditions/ChemicalReconEngine.cs` | Detector band scanning, hazard observations (unknown→suspected→identified→quantified), filter consumption/breakthrough, sample collection + lab handoff, safe-corridor discovery | Affliction authority, global map, expedition roster, weather/wind, lab synthesis |

Determinism: `ISeededRng` only; cross-platform-stable FNV-1a `StableStringHash` replaces `string.GetHashCode()` (caught by `CoreInvariantSourceTests`); no wall-clock in simulation.

## Wave 4 — Cross-System Ports

- `src/Main.Plans78_81.cs`: `SetupGeodeticSurvey/SetupKineticStorage/SetupChemicalRecon` + `SetupPlans78To81`, `SaveXxx` triad members, `TickPlans78To81` (single tick owner per system, day cadence), wired into the expanded-shelter setup flow, SaveAll, and the day-tick
- `Main.ShelterBatch3.SetupDecontamination` now loads the protocol catalog and passes it into the runtime `DecontaminationSystem` (data-driven protocols)
- Host sessions (`GeodeticSurveyHostSession`, `KineticStorageHostSession`, `ChemicalReconHostSession`): thin adapters projecting typed Core events into `LastEvent` + dirty tracking; save via save stores
- Deterministic RNG forks: `Shelter`(11)/`WorldEvolution`(15)/`Shelter`(15)/`Expedition`(15) campaign streams

## Wave 5 — Replay / Boundary / Deterministic Tests + Scenarios (93 tests, all passing)

- `Shelter/DeconAirlockSystemTests.cs` (13) + `Shelter/DeconAirlockSaveTests.cs` (7): same-seed post-wash parity, exact stage ordering, interlock boundary (0.05 passes / 0.0501 rewrites), zero-reagent blocks **without consuming water**, full-tank clamp, effluent stability, disposal boundary, override logging, silent restore, reagent-once, recapture normalization, shipped-catalog load
- `World/GeodeticSurveyEngineTests.cs` (11) + `World/GeodeticSurveySaveTests.cs` (5): observation parity, angle normalization, weather monotonicity, triangle resolution, **shortcut unlocked exactly once**, degenerate-baseline rejection, monument destruction → floor accuracy, bounded travel modifiers, network parity, silent restore
- `Shelter/KineticStorageSystemTests.cs` (17) + `Shelter/KineticStorageSaveTests.cs` (5): E=½Iω² physics, J↔kWh, 0-RPM/exact-full/thermal/vacuum boundaries, efficiency, single empty-discharge event, drag decay, surge ≤ peak, black-start exact energy + atomic failure, overspeed determinism + single fire, maintenance, restore at non-zero RPM, trajectory continuation
- `Expeditions/ChemicalReconEngineTests.cs` (12) + `Expeditions/ChemicalReconSaveTests.cs` (5): band compatibility, threshold 0/unreachable, exact battery depletion, filter math (×2.5 penalty), breakthrough boundary, discovery-state ladder, aging, sample limits, lab-once, corridor confidence + idempotence, mid-exposure restore
- `Integration/PrecisionHazardInfrastructureTests.cs` (8) — the four cross-system scenarios:
  - **A — Contaminated survey expedition:** survey network persists + recon identifies hazard + decon reduces surface contamination + gear instance preserved + save/replay parity
  - **B — Grid failure during decon:** charged flywheel supplies the decon-pump surge; empty flywheel delivers 0 and the inner door stays interlocked mid-cycle
  - **C — Precision hazard mapping:** survey accuracy rises while hazard truth is invariant (survey improves knowledge, not hazard behavior); corridor capability independently traceable
  - **D — Black-start emergency:** starter burst consumes exactly the black-start energy; decon cycle completes after restart

**Bugs the test matrix caught and fixed (all Wave-3 code):** `TryResolveTriangle` monument-key mismatch; recon battery ignoring catalog capacity; chelator checked after water consumption (§15 violation).

## Wave 6 — UI (panels landed + integrated 2026-09-05)

`DeconAirlockPanel`, `GeodeticSurveyPanel`, `KineticStoragePanel`, `ChemicalReconPanel` are implemented in `src/UI/` — typed `Bind(XxxHostSession)` bindings, `LastEvent` feedback strip, `OnActionRequested` routed through `Main.UiPanels.cs` → `Main.World.cs` action handlers, covered by `Main.UiTests.Wave6`. Binding + adapter integration completed against the UI-audit findings **UI-11/UI-12**: panels Bind on first OPEN (idempotent guards in `Main.World.cs`), adapters pass typed arguments (queue-resolved decon case, monument-derived survey triples + idempotent resolve, class-rate flywheel power, observation-derived sample location), and the two missing Core commands were added — `KineticStorageSystem.EngageEmergencyBrake` (logged, offline-only, maintenance-release) and `ChemicalReconEngine.SelectFilterCategory`. Binding contracts preserved in `docs/ui/PLANS_78_81_UI_STITCH_SPEC.md`; AGENTS.md table rows removed per the standing rule; auditor re-audit pending per audit policy.

## Wave 8 — Content Utilization & Closeout

- All 4 catalogs registered in `ContentUtilizationScanner` (loader patterns + consumer maps): `DeconProtocolCatalogLoader`/`DecontaminationSystem`, `GeodeticSurveyCatalogLoader`/`GeodeticSurveyEngine`, `KineticFlywheelCatalogLoader`/`KineticStorageSystem`, `ToxicChemicalCatalogLoader`/`ChemicalReconEngine`
- Also mapped the concurrently-landing `infiltrator_profiles.json`/`weather_hardening_upgrades.json` so the refreshed baseline would not bake in orphans
- Baseline regenerated via the sanctioned first-run path (`ContentUtilizationGate.CreateBaseline`/`SaveBaseline` through `--content-utilization-selftest`): **538 catalogs, 160 GAMEPLAY_CONSUMED, 0 ORPHANED**; all four Plans 78–81 catalogs classified `GAMEPLAY_CONSUMED`; CI gate PASS stable on re-run; `CiGate_CommittedBaseline_MatchesCurrentScanWithoutRegressions` green (56/56 utilization+integrity gate tests)

## Regression / CI Evidence (final state at closeout)

| Gate | Result |
|---|---|
| Wave-5 test scope (93 tests) | ✅ PASS |
| Utilization + catalog-integrity gates (56 tests) | ✅ PASS |
| `--data-integrity-selftest` | ✅ PASS (247+ catalogs, 0 errors) |
| `--bridge-selftest` | ✅ PASS |
| Full Core suite | 7,982 passing; 1 transient failure owned by the concurrently-landing Plans-110+/infiltrator wave (unmapped foreign save store), not this package |
| `Ashfall.csproj` | Was 0 errors at Wave-5 closeout; currently mid-churn from the concurrent agent's live edits (`Main.World.cs` referencing an unwritten property) — not this package's files |

## Files Changed

**Core:** `DecontaminationSystem.cs` (extended), `DeconProtocolCatalogLoader.cs` (new), `World/GeodeticSurveyEngine.cs` (new), `Shelter/KineticStorageSystem.cs` (new), `Expeditions/ChemicalReconEngine.cs` (new), `Content/ContentUtilizationScanner.cs` (registrations), `IO/CatalogBootValidator.cs`, `Save/SaveSectionRegistry.cs`, `items.json`
**Host:** `Main.Plans78_81.cs` (new), `Main.ExpandedShelterSystems.cs`, `Main.ShelterBatch3.cs`, `GeodeticSurveySaveStore.cs`/`KineticStorageSaveStore.cs`/`ChemicalReconSaveStore.cs` (new), 3 host sessions (new)
**Data:** 4 new catalogs + `items.json` additions
**Tests:** 9 new files (93 tests), `ComprehensiveSaveStoreCorruptionAndMigrationTests.cs` count contract
**Docs/artifacts:** `ARCHITECTURE_TEST_MAP.md` rows 116–118, `AGENTS.md` Stitch flags, `artifacts/content-utilization{-baseline}.json/.md`
**Cross-agent mechanical fixes (not this package):** `fileIO.Exists→FileExists` ×3, `ActionResult.Failed` messageKey ×2, missing usings ×4, delegate return-type wraps ×4

## Migration Notes

- No on-disk format changes to existing sections. The three new sections appear in fresh campaign envelopes; pre-existing saves load with the sections absent → safe inactive defaults
- `DecontaminationSystem` ctor gained an optional `DeconProtocolCatalog` parameter (after `ILog?`) — existing 6-arg call sites unchanged
- `DeconStatus` gained `RewashRequired`/`GearDisposalRequired`/`QuarantineRequired` members (additive)

## Intentional Deviations

1. **No triangle angle-degeneracy gating** — the engine deliberately owns no coordinates (map authority); degeneracy is rejected via catalog baseline bounds instead. No fake test.
2. **Survey × recon precision coupling** left as a host-wired seam; Scenario C pins the invariant (survey never changes hazard truth) rather than simulating unwired behavior.
3. **Decon catalog thresholds** in tests are calibrated per-scenario (game-arbitrary values); what is pinned is split behavior at boundaries, not the numbers.
4. **UltrasonicDecontaminationAirlockPanel** (pre-existing stub) is a distinct panel from Plan 78's `DeconAirlockPanel` — both flagged in `AGENTS.md`.

## Next Steps

1. **Wave 6:** generate the four panel layouts through `google-stitch` (Antigravity) using the paste-ready handoff spec **`docs/ui/PLANS_78_81_UI_STITCH_SPEC.md`**, reconcile with `AshfallUiHelpers`/`DesignTheme`, bind via `Main` action switches, keep `LastEvent` as the single feedback strip
2. Re-run `--content-utilization-selftest` + full CI after the concurrent Plans-110+/130–133 waves land
3. Remove table rows from `AGENTS.md` as each panel gains a real Core binding
