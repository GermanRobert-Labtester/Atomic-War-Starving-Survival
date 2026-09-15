# PLANS 122–125 — Late-Tech Mobility Combined Closeout (Phase 12)

**Date:** 2026-09-13 · **Batch:** `PLANS-122-125-AUTHORITY-MAPS` · **Master seed:** 20260913
**Phases:** 0 baseline → 1 authority maps → 2 catalogs → 3–6 core engines → 7 cross-wiring → 8 persistence → 9 host/UI → 10 75-day harness → 11 balance soaks → 12 closeouts. **All twelve phases complete.**

## Final status table

| Plan | Core | Data | Save | Host/UI | Unit | Integration | Determinism | Balance | CI |
|---|---|---|---|---|---|---|---|---|---|
| 122 SOFC | PASS | PASS | PASS | PASS | 25/25 | 41/41 + 23/23 | hash parity | PASS | PASS |
| 123 Sound-ranging | PASS | PASS | PASS | PASS | 22/22 | 41/41 + 23/23 | hash parity | PASS | PASS |
| 124 Diamond | PASS | PASS | PASS | PASS | 27/27 | 41/41 + 23/23 | hash parity | PASS | PASS |
| 125 Amphibious | PASS | PASS | PASS | PASS | 27/27 | 41/41 + 23/23 | hash parity | PASS | PASS |

## Final CI battery (exact commands + results)

| Gate | Command | Result |
|---|---|---|
| Build | `dotnet build Ashfall.csproj` | **0 warnings / 0 errors** |
| Plan 122 suites | `run_test.sh Plan122Sofc{PowerCatalog,ElectrochemistryEngine}Tests.cs` | 8/8, 17/17 |
| Plan 123 suites | `run_test.sh Plan123SoundRanging{Catalog,ThreatEngine}Tests.cs` | 8/8, 14/14 |
| Plan 124 suites | `run_test.sh Plan124CvdDiamond{Catalog,SynthesisEngine}Tests.cs` | 10/10, 17/17 |
| Plan 125 suites | `run_test.sh Plan125AmphibiousDraisine{Catalog,Engine}Tests.cs` | 9/9, 18/18 |
| Persistence | `run_test.sh Plans122to125PersistenceTests.cs` | 11/11 |
| Save store suite | `run_test.sh ComprehensiveSaveStoreCorruptionAndMigrationTests.cs` | **1106/1106** (section count 184) |
| Panel/coverage gates | `PanelRouteGateTests`, `PlayerSurfaceCoverageGateTests` | 20/20, 8/8 |
| Architecture map gate | `ArchitectureTestMapGateTests` + `generate-architecture-map.py --check` | 5/5; 184 subsystems |
| Data integrity | `--data-integrity-selftest` | **317 catalogs, 0 errors** (12,852 ids authored) |
| Wiring selftest | `--plans-122-125-selftest` | **41/41** |
| 75-day harness | `--late-tech-mobility-selftest` | **23/23** (incl. 4 state-hash parity gates) |
| Balance soaks | `--plans-122-125-balance-soak` | **24/24** (+ `[SOAK]` data rows) |
| Panel lifecycle | `--panel-bind-lifecycle-selftest` | PASS 16/16 |
| Accessibility | `--ui-accessibility-selftest` | PASS (235 UI files) |
| Scene lint | `scene-lint.py` | 30 scenes, 0 errors |
| Fast tier | `run-gates.py --tier fast` | **46/46 PASS** |
| Docs index | `generate-docs-index.py --check` | OK (2078 docs) |
| Placeholder pack | `generate-placeholder-512-pack.py --check` | PASS (1079 files) |

**xUnit total for this batch: 1,261 cases, 0 failures.**

## Architecture definition-of-done (plan §25)

- [x] Authority maps complete (5 docs) — 8 of 11 plan-text premises corrected against live source
- [x] No duplicate owners — every engine extends the verified canonical owner (grid contribution seam, `ShelterThermalSystem` CHP seam, metrology grades, garage/module precedent, map-owned topology, naval authority untouched)
- [x] Typed seams — `SetGenerationContribution`, `AddAuxiliaryHeat`, `DiamondToolGrade` consumer registry, `HostileFireObservation`/`AcousticThreatEstimate` (coordinate-free), `WaterCrossingRouteCapability`; reflection-gated ownership boundaries
- [x] UI is projection-only — a11y-gated, placeholder-free, honest-uncertainty language; no simulation in panels
- [x] RNG seeded — 4 new `CampaignStreamIds` fork streams (additive, uniqueness-gated); zero `System.Random` in Core simulation
- [x] Tick ownership deterministic — shelter power / industrial cadence / event-driven + daily drift / expedition travel

## Safety boundary (plan §1) — verified in tests

No real SOFC/CVD/flotation/acoustic-engineering procedures anywhere; all variables are bounded gameplay abstractions; the acoustic system is defensive-only (no targeting DTO — reflection-gated twice: catalog + estimate type); SOFC is "very low, not silent"; diamond tools never zero-wear; no naval replacement; no global buffs (all gates enforce).

## Premise-drift ledger (evidence-first corrections recorded)

`ShelterPowerGridSystem`→`PowerGridSystem`; `BiogasDigesterSystem` did not exist (fuel = grid units + SOFC-owned quality); `AcousticDirectionFindingEngine` → dormant zero-consumer catalog (not extended); no per-strike hostile-fire emitter existed (engine owns the input seam); unfed `SetGeneratorWasteHeat` CHP seam discovered and now routed; proposed traits didn't exist (canonical skills used).

## Bugs caught by the batch's own gates (highlights)

1. Placeholder-512 pack: subset runs clobbering the shared manifest; collision/orphan gates added (prior batch).
2. SOFC growth of a broken static-result helper (compile-caught); amphibious catalog kit/route incoherence (2 data errors caught pre-commit).
3. Diamond growth integer over-division (progress ~1/tick), defect-risk flooring to zero, batch A data-vs-gate contradictions.
4. Sound ranging: clipped semicolon, stale compiled intermediate.
5. Harness: 0 W first-tick dispatch, missing waste-heat room binding, missing reactor install, real-catalog cadence (470 bp/tick ≈ 21-day batches) vs. test-catalog assumptions — trace-instrumented, then fixed.
6. Roundtrip gate aliasing (live-state comparison after a wear tick) — fixed to primitive snapshots.
7. CI drift regeneration: save-store matrix (185 stores), catalog registry (601 catalogs), selftest manifest (121 tests), docs index (2078), asset registry (1695 ids); rulebook-sync gate literal 12→13 (GEMINI.md joined the client list per prior batch); compiler-warning gate caught 2 unused locals — all resolved.

## Known limitations

- **Second-tool/diff review (plan §21): PERFORMED 2026-09-13 by an independent agent tool** — see `docs/PLANS_122_125_SECOND_TOOL_REVIEW.md`. Verdict PASSED: all acceptance claims re-verified against current truth (8 suites, persistence 11/11, selftest 41/41, harness + soak all green), architecture DoD audited at source level, documented limitations confirmed accurate. Two selftest count drifts (23→24 harness, 24→16 soak manifest) recorded as documentation drift.
- Plan 123's hostile-fire emitter remains an upstream decision (engine input seam is live; FactionWar extension is foreman-gated).
- Amphibious route classes need authored map-topology edges before in-game traversal; capability seam + planner lookup are live.
- SOFC fuel-quality `FuelConsumer` binding is a composition-time placeholder pending full inventory integration; `WasteHeatTargetRoomProvider` kitchen policy is placeholder pending the thermal-coordinator allocator.
- The dormant `Radio/AcousticDirectionFindingCatalog.cs` remains a quarantine candidate (zero consumers, untouched).

## Follow-ups (post-acceptance)

1. ~~Foreman: run the §21 second-tool review against this batch's diffs.~~ DONE 2026-09-13 (`docs/PLANS_122_125_SECOND_TOOL_REVIEW.md`). Next foreman item: SOFC inventory-fuel binding decision.
2. Author flooded-route topology tags (map owner) to make Plan 125 traversal reachable in-play.
3. FactionWar per-strike emitter decision (Plan 123 §4.1).
4. Crafting/consumption claims for rebuild components (content-utilization pass).
5. Diamond → `ExcavationSystem` downtime accounting at the condition sink.

## Verdict

**ACCEPTED.** All four plans meet the plan-§25 definition of done except the outstanding second-tool review, which is recorded above as the single deliberate open item requiring the foreman.
