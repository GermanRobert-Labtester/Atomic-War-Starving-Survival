# PLAN-SIGNALS-REMOTE-SENSING-49 — Orbital Telemetry, InSAR, Metrology & EW

**Wave:** 5 (2026-09-21) · **Kind:** MAJOR EXPANSION
**Status:** PROPOSED — not a claim.
**Depends on:** PLAN-WEATHER-ATMOSPHERE-28, PLAN-RADIO-MEDIA-42,
PLAN-TRANSPORT-EXPEDITION-30.
**Non-goals:** no real satellite programs or bands, no real EW doctrine, no
second map authority.

---

## 1. Outcome

The late-campaign science layer is authored across many files that never
converge: `OrbitalHarrowTelemetrySystem` (`orbital_harrow_events.json`),
`InSarDeformationEngine` (+ `Main.InSarMapping`), low-background metrology
(`Main.LowBackgroundMetrology`), `GeodeticSurveyEngine`, GPR
(`GroundPenetratingRadarEngine` + catalog), `AtmosphericSoundingCatalog`,
`CommsArraySystem`, `RadarEcmCatalog`, `ReconTelemetrySystem`, direction
finding/acoustic triangulation, and `WeatherStationSystem`. Data exists:
`insar_geodesy_catalog.json`, `geodetic_survey_catalog.json`,
`metrology_standards_catalog.json`, `atmospheric_sounding_catalog.json`,
`radar_ecm_catalog.json`, `recon_telemetry_probes.json`, `comms_targets.json`,
`acoustic_triangulation_catalog.json`.

Player loop: **task sensors → collect → calibrate → fuse → act on what only
you can see**.

| Mechanic | Authority | Player action | Outcome |
|---|---|---|---|
| Orbital passes | `OrbitalHarrowTelemetrySystem`, events catalog | predict, tune, decode | telemetry, warnings, story beats |
| InSAR | `InSarDeformationEngine` | survey a region | ground deformation, hazard forecast, resource hints |
| GPR | `GroundPenetratingRadarEngine` | scan a site | subsurface anomalies, excavation targets |
| Geodesy | `GeodeticSurveyEngine` | establish control points | map accuracy, route correction |
| Metrology | metrology standards + calibration | calibrate instruments | confidence, drift, false positives |
| Sounding | `AtmosphericSoundingCatalog` | launch, analyse | forecast accuracy (links Plan 28) |
| EW | `RadarEcmCatalog`, `CommsArraySystem`, DF | detect, jam, spoof, locate | detection vs counter-detection |
| Fusion | observation board | prioritize, correlate | actionable intel (links Plan 41) |

---

## 2. Evidence

| Item | Detail |
|---|---|
| Core | `OrbitalHarrowTelemetrySystem.cs`, `World/InSarDeformationEngine.cs`, `World/GeodeticSurveyEngine.cs`, `World/GroundPenetratingRadarEngine.cs`, `World/AtmosphericSoundingCatalog.cs`, `World/CommsArraySystem.cs`, `WeatherStationSystem`, `Expeditions/ReconTelemetrySystem`, `Radio/Acoustic*`/`DirectionFinding` |
| Data | `orbital_harrow_events.json`, `insar_geodesy_catalog.json`, `geodetic_survey_catalog.json`, `metrology_standards_catalog.json`, `atmospheric_sounding_catalog.json`, `radar_ecm_catalog.json`, `recon_telemetry_probes.json`, `comms_targets.json`, `acoustic_triangulation_catalog.json` |
| Sealed prior | InSAR deformation intelligence (Plan 139: 17/17 + host), low-background metrology save section, GPR catalog, `DEBT-ARCH-MAP` retargeted nuclear/GPR evidence |
| Contracts | map graph remains canonical; observations are read models; forecast authority stays `WeatherStationSystem` (DEC-15) |

---

## 3. Packages

### SG-49A — Orbital telemetry
- Pass prediction from a deterministic orbit model (authored, fictional);
  tuning and decoding produce telemetry packets and authored events; missed
  passes cost windows, not kills.
- **Acceptance:** passes predictable and visible; decode feeds journal/
  narrative; no RNG wall-clock dependency.
- **Verify:** focused orbital tests + `--data-integrity-selftest`.

### SG-49B — Remote sensing: InSAR, GPR, geodesy
- Surveys produce region/site read models: deformation bands, subsurface
  anomalies, control-point accuracy; outputs feed map markers, hazard forecast,
  and excavation targets (canonical owners).
- **Acceptance:** observations are read-only projections; land/rock hazard
  links to `SubterraneanSubsidenceEngine`; no duplicate map store.
- **Verify:** focused + world suites.

### SG-49C — Metrology and confidence
- Instrument calibration, drift, standards, and confidence bands; uncalibrated
  readings produce false positives that cost time (not fake gameplay state).
- **Acceptance:** every observation carries a confidence; calibration is a
  real cost; no silent accuracy.
- **Verify:** focused metrology tests.

### SG-49D — Atmospheric sounding
- Soundings feed Plan 28 forecast reliability; launch cadence limited by
  consumables and weather; data degrades over time.
- **Acceptance:** measurable forecast improvement; no free perfect forecast.
- **Verify:** weather suite + sounding tests.

### SG-49E — EW: radar, ECM, comms array, direction finding
- Detection vs jamming/spoofing/counter-detection; locating transmitters
  supports espionage (Plan 41) and patrols; ECM affects convoy/air ops
  (Plan 30/42).
- **Acceptance:** deterministic contests; counterplay; no infinite jamming or
  perfect detection.
- **Verify:** direction-finding + patrol radio suites.

### SG-49F — Observation fusion and tasking
- One observation board correlates sensors into actionable intel (named
  threats/opportunities with confidence), tasking consumes time/consumables.
- **Acceptance:** intel is actionable and verifiable in world; no duplicate
  intel store; uncertainty visible.
- **Verify:** espionage + world suites.

### SG-49G — Content volumes
- +6 orbital events, +8 survey rows, +6 metrology standards, +8 GPR anomalies,
  +6 EW scenarios; fictional/abstract; consumer-bound.

---

## 4. Risks

| Risk | Mitigation |
|---|---|
| Sci-fi drift | grounded instrumentation framing; authored, fictional hardware |
| Sensor overload | one observation board; tasking budget per day |
| Perfect information | confidence bands + false positives; verification costs time |
| Overlap with weather/radio | those owners stay canonical; this plan only feeds them |

## 5. Verification

```bash
godot --headless --path . -- --weather-selftest
godot --headless --path . -- --world-selftest
bash scripts/run_test.sh Ashfall.Core.Tests/World/
bash scripts/run_test.sh Ashfall.Core.Tests/Radio/
```

---

## 6. Expanded census (9 files · 2,899 lines)

Scope: `Assets/Ashfall.Core/World/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Catalog 1 · Support 4 · System 4

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `RadioSignalLog.cs` | 149 | Support | — | 0 | 0 | 4 |
| `SignalAuthenticityEvaluator.cs` | 158 | Support | — | 0 | 0 | 0 |
| `SignalTriangulationSystem.cs` | 694 | System | — | 0 | 0 | 2 |
| `SignalTrustAvailability.cs` | 107 | Support | — | 0 | 0 | 0 |
| `SignalTrustLedger.cs` | 179 | Support | — | 0 | 0 | 2 |
| `GroundPenetratingRadarCatalog.cs` | 163 | Catalog | — | 0 | 0 | 0 |
| `GroundPenetratingRadarEngine.cs` | 227 | System | **yes** | 0 | 0 | 2 |
| `InSarDeformationEngine.cs` | 530 | System | **yes** | 0 | 0 | 2 |
| `WeatherSondeSystem.cs` | 692 | System | — | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 6 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `insar_geodesy_catalog.json` | object[2 keys] |
| `radar_ecm_catalog.json` | object[5 keys] |
| `radio_distress_signals_expansion.json` | object[2 keys] |
| `radio_distress_signals.json` | object[2 keys] |

**State surfaces:** `RadioSignalLog.cs`, `SignalTriangulationSystem.cs`, `SignalTrustLedger.cs`, `GroundPenetratingRadarEngine.cs`, `InSarDeformationEngine.cs`, `WeatherSondeSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/World/` |
| Test references | 18 name references across the test tree |
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

Computed across 10 domain files: **7 type-reference edges**.

| File | Lines | In-degree | Out-degree |
|---|---:|---:|---:|
| `SignalTriangulationSystem.cs` | 694 | 0 | 0 |
| `WeatherSondeSystem.cs` | 692 | 0 | 0 |
| `InSarDeformationEngine.cs` | 530 | 0 | 0 |
| `CommsArraySystem.cs` | 346 | 0 | 0 |
| `GroundPenetratingRadarEngine.cs` | 227 | 0 | 6 |
| `SignalTrustLedger.cs` | 179 | 1 | 0 |
| `GroundPenetratingRadarCatalog.cs` | 163 | 6 | 0 |
| `SignalAuthenticityEvaluator.cs` | 158 | 0 | 0 |
| `RadioSignalLog.cs` | 149 | 0 | 0 |
| `SignalTrustAvailability.cs` | 107 | 0 | 1 |

**Highest-coupling files (in×2 + out):**

- `GroundPenetratingRadarCatalog.cs` — in 6, out 0
- `GroundPenetratingRadarEngine.cs` — in 0, out 6
- `SignalTrustLedger.cs` — in 1, out 0
- `SignalTrustAvailability.cs` — in 0, out 1
- `RadioSignalLog.cs` — in 0, out 0
- `SignalAuthenticityEvaluator.cs` — in 0, out 0
- `SignalTriangulationSystem.cs` — in 0, out 0
- `CommsArraySystem.cs` — in 0, out 0

**Ordering implication:** high in-degree files are depended upon — verify or seal
them first. High out-degree files are consumers whose claims should land after
their dependencies; a file with both is the domain's hub and needs its own
bounded package.

---

## 12. Cross-plan coupling

Domain files: 10. Other plans referencing their names: **10**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-HELIOGRAPH-TRUTH-235` | 5 |
| `PLAN-RADIO-FAMILY-TRUTH-266` | 5 |
| `PLAN-ELECTRONICS-COMPUTING-65` | 2 |
| `PLAN-EVENT-WIRING-21` | 1 |
| `PLAN-WEATHER-ATMOSPHERE-28` | 1 |
| `PLAN-ESPIONAGE-COUNTERINTEL-41` | 1 |
| `PLAN-RADIO-MEDIA-42` | 1 |
| `PLAN-DEEP-STRATA-83` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `SG-49A` | no name match — resolve at claim time |
| `SG-49B` | `InSarDeformationEngine.cs` |
| `SG-49C` | no name match — resolve at claim time |
| `SG-49D` | no name match — resolve at claim time |
| `SG-49E` | `CommsArraySystem.cs`, `GroundPenetratingRadarCatalog.cs`, `GroundPenetratingRadarEngine.cs` |
| `SG-49F` | no name match — resolve at claim time |
| `SG-49G` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 11. Host files: **15** · Test files: **26** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 15 | `src/Host/HostCli.AdvancedIndustrialRecon.cs`, `src/Host/HostCli.DynamicWorld.cs`, `src/Host/HostCli.Plans139_141.cs`, `src/Host/HostCli.PlansB86_B89.cs`, `src/Host/HostCli.SkyDefense.cs` |
| Tests (`Ashfall.Core.Tests/`) | 26 | `Ashfall.Core.Tests/ApicultureAndTriangulationIntegrationTests.cs`, `Ashfall.Core.Tests/CampaignContinuityFlagshipB70_B73Tests.cs`, `Ashfall.Core.Tests/Core/DeterminismSeedSweepTests.cs`, `Ashfall.Core.Tests/Integration/Plans198_201_LateGameSystemsIntegrationTests.cs`, `Ashfall.Core.Tests/Integration/Plans46_49_CrossSystemIntegrationTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **8** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `comms_array` |
| `dose_ledger` |
| `insar_deformation` |
| `radio` |
| `radio_program_production` |
| `radio_station` |
| `recon_telemetry` |
| `weather_hardening` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **6** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--dose-ledger-selftest` |
| `--journal-weather-panel-selftest` |
| `--ledger-debt-selftest` |
| `--radio-catalog-selftest` |
| `--radio-selftest` |
| `--weather-save-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **12**.

| Event | First declaration |
|---|---|
| `OnLedgerCalibrated` | `Assets/Ashfall.Core/DoseLedgerSystem.cs` |
| `OnLedgerTampered` | `Assets/Ashfall.Core/LedgerDebtSystem.cs` |
| `OnSondeFailed` | `Assets/Ashfall.Core/World/WeatherSondeSystem.cs` |
| `OnSondeRecovered` | `Assets/Ashfall.Core/World/WeatherSondeSystem.cs` |
| `OnTelemetryChanged` | `Assets/Ashfall.Core/OrbitalHarrowTelemetrySystem.cs` |
| `OnTelemetryLost` | `Assets/Ashfall.Core/World/WeatherSondeSystem.cs` |
| `OnTelemetryReceived` | `Assets/Ashfall.Core/World/WeatherSondeSystem.cs` |
| `OnTriangulationCompleted` | `Assets/Ashfall.Core/Radio/SignalTriangulationSystem.cs` |
| `OnTriangulationFailed` | `Assets/Ashfall.Core/Radio/SignalTriangulationSystem.cs` |
| `OnTrustChanged` | `Assets/Ashfall.Core/Economy/IEconomyInterfaces.cs` |
| `OnWeatherChanged` | `Assets/Ashfall.Core/World/WeatherSystem.cs` |
| `OnWeatherFrontArrived` | `Assets/Ashfall.Core/Weather/WeatherCascadeSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/acoustic_triangulation_catalog.json` |
| `Assets/StreamingAssets/Data/comms_targets.json` |
| `Assets/StreamingAssets/Data/faction_radio_corpus.json` |
| `Assets/StreamingAssets/Data/faction_war_radio.json` |
| `Assets/StreamingAssets/Data/ledger_debt_templates.json` |
| `Assets/StreamingAssets/Data/narrative/bunker_trade_ledger_batch_2.json` |
| `Assets/StreamingAssets/Data/narrative/ground_glass_joint_greasing_audits.json` |
| `Assets/StreamingAssets/Data/narrative/orbital_kinetic_telemetry.json` |
| `Assets/StreamingAssets/Data/narrative/radio_broadcast_rundowns.json` |
| `Assets/StreamingAssets/Data/narrative/radio_mysteries_expansion.json` |
| `Assets/StreamingAssets/Data/narrative/radio_scriptbook.json` |
| `Assets/StreamingAssets/Data/narrative/radio_scripts_expansion.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **3** (51 files, 376 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Radio` | 47 | 354 |
| `Telemetry` | 2 | 11 |
| `Weather` | 2 | 11 |

**Verdict:** 376 cases sit under matching regions — run those first (`Radio`, `Telemetry`, `Weather`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **35**
(16 of them panels/HUD).

| Host file |
|---|
| `src/Host/CommsArraySaveStore.cs` |
| `src/Host/DoseLedgerHostSession.cs` |
| `src/Host/DoseLedgerSaveStore.cs` |
| `src/Host/RadioCatalogSelfTest.cs` |
| `src/Host/RadioHostSession.cs` |
| `src/Host/RadioProgramProductionHostSession.cs` |
| `src/Host/RadioProgramProductionSaveStore.cs` |
| `src/Host/RadioSaveStore.cs` |
| `src/Host/RadioStationSaveStore.cs` |
| `src/Host/ReconTelemetryHostSession.cs` |
| `src/Host/ReconTelemetrySaveStore.cs` |
| `src/Host/WeatherHardeningHostSession.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **8**, of which versioned-ladder sections:
**1**.

| Section key | Laddered |
|---|---|
| `comms_array` | no |
| `dose_ledger` | yes |
| `insar_deformation` | no |
| `radio` | no |
| `radio_program_production` | no |
| `radio_station` | no |
| `recon_telemetry` | no |
| `weather_hardening` | no |

**Verdict:** matched sections include versioned ladders — a schema change here must extend the existing ladder, not fork it.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **3**.

| Stream |
|---|
| `insar_deformation` |
| `radio` |
| `weather` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **30**
(CODEX_ONLY 12, GAMEPLAY_CONSUMED 11, OPTIONAL 1, UNRESOLVED 6).

| Catalog | Classification |
|---|---|
| `acoustic_triangulation_catalog.json` | UNRESOLVED |
| `comms_targets.json` | GAMEPLAY_CONSUMED |
| `faction_radio_corpus.json` | GAMEPLAY_CONSUMED |
| `faction_war_radio.json` | GAMEPLAY_CONSUMED |
| `ledger_debt_templates.json` | UNRESOLVED |
| `narrative/bunker_trade_ledger_batch_2.json` | CODEX_ONLY |
| `narrative/ground_glass_joint_greasing_audits.json` | CODEX_ONLY |
| `narrative/orbital_kinetic_telemetry.json` | CODEX_ONLY |
| `narrative/radio_broadcast_rundowns.json` | CODEX_ONLY |
| `narrative/radio_mysteries_expansion.json` | CODEX_ONLY |

**Verdict:** 6 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **1**.

| Flag |
|---|
| `flag_betrayed_trust` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY-WITH-NOTES (11/12) · **Class:** standard · **Coupling (incoming plans):** 10
**Surface:** save sections 8 (laddered 1) · RNG streams 3 · host files 16 · catalogs 22 · test regions 3 · flags 6

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-SIGNALS-REMOTE-SENSING-49
wave: —
status: PROPOSED — foreman claim required
packages: SG-49A, SG-49B, SG-49C, SG-49D, SG-49E, SG-49F, SG-49G
claim paths:
  - src/Host/CommsArraySaveStore.cs  # §19 candidate host surface
  - src/Host/DoseLedgerHostSession.cs  # §19 candidate host surface
  - src/Host/DoseLedgerSaveStore.cs  # §19 candidate host surface
  - src/Host/RadioCatalogSelfTest.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/acoustic_triangulation_catalog.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/comms_targets.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Radio/
  - godot --headless --path . -- --dose-ledger-selftest
dependencies:
  - coordinate: 10 other plan(s) name these artifacts (§12)
  - governance spine must land first: 56 → 86 → 17 → 100
  - touches 1 versioned save ladder(s) — extend, never fork
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
