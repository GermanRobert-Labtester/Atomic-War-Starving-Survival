# PLAN-KINETIC-STORAGE-TRUTH-181 — Flywheels: Stored Energy, Loss & Failure

**Wave 14 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-ENERGY-NUCLEAR-48, PLAN-POWER (Plan 48 grid packages), PLAN-MAINTENANCE-DECAY-TRUTH-119.
**Implementation scaffold:** [`PLAN-KINETIC-STORAGE-TRUTH-181_APPENDIX-A_SCAFFOLD.md`](PLAN-KINETIC-STORAGE-TRUTH-181_APPENDIX-A_SCAFFOLD.md) — paired with `PLAN-ENERGY-NUCLEAR-48` as scaffolding authority: source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no generation model (Plan 48), no grid topology (Plan 48),
no new machine catalog.

## 1. Outcome
`Shelter/KineticStorageSystem.cs` (**615 lines**) is reachable and unaddressed:
mechanical energy storage for the shelter grid. Storage is where loss and
failure matter — a flywheel has standby loss, a rated capacity, and a
catastrophic failure mode. None are stated, so storage is either lossless or
opaque.

| Deliverable | Detail |
|---|---|
| Storage model | state of charge with rated capacity and standby loss per hour (canonical clock) |
| Charge/discharge | rates bounded by the grid's surplus/deficit; no negative or over-capacity values |
| Failure mode | a worn flywheel (Plan 119 condition) can fail catastrophically with a documented consequence and containment requirement |
| Grid interface | charge/discharge appear in Plan 48's grid balance, not a private energy pool |
| Save truth | charge and condition restore; a load continues loss from the stored hour |

## 2. Evidence
- `Assets/Ashfall.Core/Shelter/KineticStorageSystem.cs` (615 lines; unaddressed — Wave 13 audit).
- Plan 48 owns generation and grid balance this plan plugs into.
- Plan 119 supplies condition decay driving the failure mode.
- Difficulty scalars (Plan 73) can scale loss rates; the plan notes the row source.

## 3. Packages
- **KST-181A** storage model + loss table.
- **KST-181B** charge/discharge bound tests (no over/underflow).
- **KST-181C** grid balance integration test with Plan 48.
- **KST-181D** failure mode + containment requirement fixture.
- **KST-181E** save round-trip; loss continues from the stored hour.

## 4. Acceptance & verification
- Charge stays within bounds under surging input; loss accrues per game hour.
- Grid balance includes storage exactly once.
- A failed flywheel produces its documented consequence and consumes containment.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/`.

## 5. Risks
Lossless storage → loss is a data row, visible in the balance.
Failure being punitive → the failure threshold and consequence are documented and surfaced before failure.

---

## 6. Expanded census (1 files · 615 lines)

Scope: `Assets/Ashfall.Core/Shelter/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `KineticStorageSystem.cs` | 615 | System | **yes** | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `kinetic_flywheel_catalog.json` | object[5 keys] |
| `orbital_kinetic_telemetry.json` | array[8] |

**State surfaces:** `KineticStorageSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Shelter/` |
| Test references | 3 name references across the test tree |
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

Domain files: 1. Other plans referencing their names: **1**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-ENERGY-NUCLEAR-48` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `KST-181A` | `KineticStorageSystem.cs` |
| `KST-181B` | no name match — resolve at claim time |
| `KST-181C` | no name match — resolve at claim time |
| `KST-181D` | no name match — resolve at claim time |
| `KST-181E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 4. Host files: **3** · Test files: **3** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 3 | `src/Host/KineticStorageHostSession.cs`, `src/Main.Plans78_81.cs`, `src/UI/KineticStoragePanel.cs` |
| Tests (`Ashfall.Core.Tests/`) | 3 | `Ashfall.Core.Tests/Integration/PrecisionHazardInfrastructureTests.cs`, `Ashfall.Core.Tests/Shelter/KineticStorageSaveTests.cs`, `Ashfall.Core.Tests/Shelter/KineticStorageSystemTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **7** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `anomaly_hazard` |
| `infrastructure` |
| `kinetic_storage` |
| `precision_metrology` |
| `precision_optics` |
| `recon_telemetry` |
| `route_infrastructure` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **13** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--expedition-panel-lifecycle` |
| `--expedition-panel-uitest` |
| `--journal-weather-panel-selftest` |
| `--panel-bind-lifecycle-selftest` |
| `--panel-bind-selftest` |
| `--panel-lifecycle-selftest` |
| `--plans-122-125-balance-soak` |
| `--plans-122-125-selftest` |
| `--plans-139-141-selftest` |
| `--precision-metrology-selftest` |
| `--real-main-journey-selftest` |
| `--shelter-hazard-loop-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **4**.

| Event | First declaration |
|---|---|
| `OnHazardWarning` | `Assets/Ashfall.Core/VentilationSystem.cs` |
| `OnTelemetryChanged` | `Assets/Ashfall.Core/OrbitalHarrowTelemetrySystem.cs` |
| `OnTelemetryLost` | `Assets/Ashfall.Core/World/WeatherSondeSystem.cs` |
| `OnTelemetryReceived` | `Assets/Ashfall.Core/World/WeatherSondeSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/excavation_hazard_mitigation.json` |
| `Assets/StreamingAssets/Data/fluid_infrastructure.json` |
| `Assets/StreamingAssets/Data/kinetic_flywheel_catalog.json` |
| `Assets/StreamingAssets/Data/narrative/canyon_mudflow_hazard_reports.json` |
| `Assets/StreamingAssets/Data/narrative/education_session_records.json` |
| `Assets/StreamingAssets/Data/narrative/orbital_kinetic_telemetry.json` |
| `Assets/StreamingAssets/Data/narrative/therapist_session_notes.json` |
| `Assets/StreamingAssets/Data/narrative/therapist_session_notes_batch_2.json` |
| `Assets/StreamingAssets/Data/narrative/therapist_session_notes_batch_3.json` |
| `Assets/StreamingAssets/Data/narrative/weather_almanac_expansion.json` |
| `Assets/StreamingAssets/Data/orbital_harrow_events.json` |
| `Assets/StreamingAssets/Data/precision_broaching_catalog.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **11** (107 files, 725 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Balance` | 1 | 5 |
| `Campaign` | 32 | 187 |
| `Economy` | 41 | 329 |
| `Education` | 2 | 11 |
| `Events` | 1 | 6 |
| `Excavation` | 1 | 5 |
| `Foundry` | 8 | 73 |
| `Holdfast` | 1 | 13 |
| `Integration` | 16 | 74 |
| `Telemetry` | 2 | 11 |

**Verdict:** 725 cases sit under matching regions — run those first (`Balance`, `Campaign`, `Economy`, `Education`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **522**
(233 of them panels/HUD).

| Host file |
|---|
| `src/Disease/DiseaseHostSession.cs` |
| `src/Economy/EconomyMarketPanel.cs` |
| `src/Economy/TradeScreenGodotPanel.cs` |
| `src/Foundry/SilentFoundryHostSession.cs` |
| `src/Host/AgricultureHostSession.cs` |
| `src/Host/AirlockSecurityHostSession.cs` |
| `src/Host/AmphibiousDraisineHostSession.cs` |
| `src/Host/AnomalyHazardSaveStore.cs` |
| `src/Host/ApprenticeshipHostSession.cs` |
| `src/Host/ArchiveDeskHostSession.cs` |
| `src/Host/AutopsyHostSession.cs` |
| `src/Host/BallisticShieldHostSession.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **33**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `agriculture` | no |
| `airlock_security` | no |
| `amphibious_draisine` | no |
| `anomaly_hazard` | no |
| `apprenticeship` | no |
| `archive_desk` | no |
| `autopsy` | no |
| `ballistic_shield` | no |
| `black_market` | no |
| `black_projects_archive` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **15**.

| Stream |
|---|
| `agriculture_blight` |
| `agriculture_mutation` |
| `agriculture_pest` |
| `amphibious_draisine` |
| `anomaly_hazard` |
| `aquaponics_disease` |
| `black_market_bounty` |
| `black_market_debt_event` |
| `black_market_stock` |
| `cupola_foundry` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **85**
(CODEX_ONLY 49, GAMEPLAY_CONSUMED 22, OPTIONAL 3, UNRESOLVED 11).

| Catalog | Classification |
|---|---|
| `archive_inks.json` | GAMEPLAY_CONSUMED |
| `autopsy_procedures.json` | GAMEPLAY_CONSUMED |
| `ballistic_shield_catalog.json` | UNRESOLVED |
| `caravan_trade_routes.json` | GAMEPLAY_CONSUMED |
| `contagion_events.json` | UNRESOLVED |
| `cultural_archive_tomes.json` | UNRESOLVED |
| `cupola_foundry_catalog.json` | UNRESOLVED |
| `desperation_events.json` | GAMEPLAY_CONSUMED |
| `disease_catalog.json` | GAMEPLAY_CONSUMED |
| `economy_goods.json` | GAMEPLAY_CONSUMED |

**Verdict:** 11 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **2**.

| Flag |
|---|
| `flag_preserved_archive` |
| `flag_repaired_infrastructure` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 1
**Surface:** save sections 33 (laddered 0) · RNG streams 15 · host files 24 · catalogs 22 · test regions 10 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-KINETIC-STORAGE-TRUTH-181
wave: 14
status: PROPOSED — foreman claim required
packages: KST-181A, KST-181B, KST-181C, KST-181D, KST-181E
claim paths:
  - src/Disease/DiseaseHostSession.cs  # §19 candidate host surface
  - src/Economy/EconomyMarketPanel.cs  # §19 candidate host surface
  - src/Economy/TradeScreenGodotPanel.cs  # §19 candidate host surface
  - src/Foundry/SilentFoundryHostSession.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/excavation_hazard_mitigation.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/fluid_infrastructure.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Balance/
  - godot --headless --path . -- --expedition-panel-lifecycle
dependencies:
  - coordinate: 1 other plan(s) name these artifacts (§12)
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
