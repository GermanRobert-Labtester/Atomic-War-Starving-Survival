# PLAN-TEST-WELFARE-17 — Suite Hygiene, Unity-Era Removal & Coverage Gates

**Program:** ASHFALL Expansion & Integration Program — Wave 2 (2026-09-21)
**Status:** PROPOSED — not a claim. Foreman claim required.
**Owner role on execution:** Integrator (test project + gates) with Builders per
suite family.
**Depends on:** PLAN-INTEGRATION-KIT-02 (the new gates need a fast, trustworthy
suite); every wiring wave adds tests.
**Expanded appendix:** [`PLAN-TEST-WELFARE-17_APPENDIX-A_SUITE_MAP.md`](PLAN-TEST-WELFARE-17_APPENDIX-A_SUITE_MAP.md)
— the suite map: **1,378 test files** grouped by directory family with file
counts, `[Fact]`/`[Theory]` case counts, and TEST-AGGREGATION metadata per
family (largest: 560 root files / 5,731 cases; Shelter 87 / 754; World 65 /
539; Survivors 57 / 453; Medical 49 / 435), plus the top-15 split-candidate
shortlist.
**Non-goals:** no coverage-percentage chase, no new test frameworks, no
re-enabling quarantined drafts without the protocol, no full-suite default.

---

## 1. Outcome

1,378 test files protect the game, and `TEST_POLICY.md` already sets the right
rules — but the test project still compiles a **Unity-era source file**
(`Assets/_Game/Shelter/NoiseDisciplineSystem.cs` via `Compile Include`), which
directly contradicts "Unity is retired / never extend `Assets/_Game/`". The
suite is also large enough that its own health (fixtures, static state, flake,
duration) is now a maintenance concern. This plan makes the test project honest
and keepable.

Deliverables:

1. **Unity-era sources removed** from the test project with evidence;
2. a **suite map** (family, count, duration class) with budgets;
3. **fixture consolidation** for the repeated scratch-catalog pattern;
4. a **static-state/flake sweep** with isolation rules;
5. **coverage gates** only where the policy names a gap (save round-trip,
   determinism, H10/H11 systems);
6. **warning/analyzer hygiene** without blanket `NoWarn`.

---

## 2. Premise evidence (2026-09-21)

| Fact | Value | Source |
|---|---:|---|
| Test `.cs` files | 1,378 | `find Ashfall.Core.Tests -name '*.cs'` |
| Top-level entries in project | 671 | `ls Ashfall.Core.Tests` |
| Unity-era `Compile Include` | `..\Assets\_Game\Shelter\NoiseDisciplineSystem.cs` | `Ashfall.Core.Tests.csproj` |
| That source exists | 5,869 bytes | `ls Assets/_Game/Shelter/NoiseDisciplineSystem.cs` |
| Blanket `NoWarn` | xUnit2013/2020, CS8618/8603/8600/8601/8602/8604/8625 | csproj |
| Focused runner | `scripts/run_test.sh`, 180 s cap, rejects excluded targets | `TEST_POLICY.md` |
| Coverage tooling | `coverlet.collector`, coverage gate script | csproj + `scripts/ci` |
| Aggregation metadata | `// TEST-AGGREGATION: source_rows=N aggregate_cases=M saved_cases=K` | `TEST_POLICY.md` |
| Quarantine | reconciled; real-file-only rule enforced | `DEBT-TEST-QUARANTINE-2026-09-12` |
| Flake precedent | `TradeSpecialtySystem` static-state isolation | `DEBT-STANDING-FAILURES-2026-09-17` |
| Scratch fixture precedent | `IntegrityScratchFixture` seeds duty roles | Plan 24 seal |
| Test-gap skills | `ashfall-test-gap` (H10 Needs, H11 Journal) | `.agents/skills` |

---

## 3. Authority map

| Concern | Extend (do not create) |
|---|---|
| Test project | `Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` |
| Runner | `scripts/run_test.sh` (180 s cap) |
| Policy | `TEST_POLICY.md` (selection, aggregation, quarantine) |
| Coverage | existing coverlet + coverage gate |
| Fixtures | `IntegrityScratchFixture` and existing per-suite fixtures |
| Quarantine | `QuarantineManifestGateTests` |

---

## 4. Packages

### TW-17A — Unity-era source removal
- Decide `NoiseDisciplineSystem.cs`: port its behavior into the current
  shelter framing (`ShelterAtmosphereSystem`/`NoiseDiscipline` owners if
  present) or delete it with a `KNOWN_DEBT` row. Either way remove the
  `Compile Include` and the `Assets/_Game` reference from the test project.
- Sweep for any other `Assets/_Game` compile references (audit found none in
  csproj, but a gate should enforce it).
- **Acceptance:** `grep Assets/_Game` in csproj → 0; affected tests either
  retargeted to Core or removed with evidence; all remaining
  `Assets/_Game` mentions are documentation comments only.
- **Verify:** `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/`;
  `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` (0/0).

### TW-17B — Suite map and duration budgets
- Produce `docs/ci/TEST_SUITE_MAP.md`: family → file count → duration class
  (fast <5 s, medium <30 s, slow <180 s) from a bounded timed run per family.
  Set a budget per family; anything over budget is a split candidate.
- **Acceptance:** every directory family appears; slow families have a split
  proposal; the focused runner stays under its cap for a single family.
- **Verify:** `bash scripts/run_test.sh <family>` per row.

### TW-17C — Fixture consolidation
- Consolidate repeated scratch-catalog creation (mandatory-catalog fixtures)
  into the existing scratch fixture pattern with a clear per-test override.
- **Acceptance:** duplicate fixture code reduced; each consolidated fixture
  reports which rows it seeds; no test-only production seams introduced.
- **Verify:** focused suites of the touched families.

### TW-17D — Static-state and flake sweep
- Find static mutable state in Core/test helpers that survives between tests;
  require per-test reset or injectable clocks/rng. Add a determinism seed sweep
  for the flagged systems.
- **Acceptance:** zero flakes across three consecutive runs of the flagged
  suites; the sweep is documented with the flaky test named and the isolation
  fix.
- **Verify:** `bash scripts/run_test.sh` on flagged files, three times.

### TW-17E — Coverage where policy names gaps
- Add coverage only for: save round-trip paths of newly wired systems,
  determinism/replay paths, and the H10/H11 systems named by the test-gap skill
  outcome. Enforce a per-assembly floor only for save + determinism namespaces.
- **Acceptance:** coverage report artifact; new-system save/determinism paths
  covered; no global percentage threshold that encourages trivial tests.
- **Verify:** `bash scripts/ci/coverage-gate.sh` (existing).

### TW-17F — Warning and analyzer hygiene
- Audit the `NoWarn` list: remove blanket suppressions where the warning count
  is zero, keep only justified ones with a comment, and keep the compiler
  warning baseline gate green.
- **Acceptance:** `NoWarn` reduced or each entry justified inline; build 0/0;
  analyzer warnings fixed or suppressed with a file-level reason.
- **Verify:** `dotnet build Ashfall.slnx` (0 warnings) + the warning baseline
  gate.

---

## 5. Risk register

| Risk | Mitigation |
|---|---|
| Removing a Unity-era test source deletes real coverage | default is port, not delete; deletion only with reference evidence |
| Duration measurement itself takes long | one bounded run per family, cached artifact |
| Fixture consolidation hides row seeding | fixture reports seeded rows; aggregated tests keep per-row failure output |
| Coverage gates become the goal | floors limited to save/determinism namespaces |
| Flake sweep becomes a rewrite | flag + isolate only; no behavior change |

## 6. Verification summary

```bash
dotnet build Ashfall.slnx
bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/
bash scripts/run_test.sh Ashfall.Core.Tests/<family>
bash scripts/ci/coverage-gate.sh
bash scripts/run_test.sh Ashfall.Core.Tests/Tooling/QuarantineManifestGateTests.cs
```

## 7. Change control

Aggregation requires the `TEST-AGGREGATION` metadata. Any quarantine must point
to a real file. No test may assert a retired API. New tests are added only for
a confirmed defect or a new public contract (policy).

---

## 6. Expanded census (bespoke: test welfare surface)

This plan governs test health, so the census counts the suite and the
quarantine surface.

| Metric | Value |
|---|---:|
| Test regions | 108 |
| Test files | 818 |
| `[Fact]`/`[Theory]` cases | 11864 |
| Quarantine manifest present | no |

**Largest regions:** `Shelter` 87, `World` 65, `Survivors` 57, `Medical` 49, `Radio` 47, `Economy` 41, `Expeditions` 41, `Tooling` 35

## 7. Expanded surface: welfare contract

| Rule | Detail |
|---|---|
| Focused first | new/changed tests run alone before aggregation |
| Quarantine | a quarantined test states owner, reason, and re-enable condition |
| Aggregation | only homogeneous static mappings aggregate; state/lifecycle tests stay independent |
| Budget | focused runs bounded; no full-suite default |

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Quarantine rows | each has owner + reason + condition |
| New-file rule | new files run alone first (procedural) |
| Duplicates | duplicate test names reported |
| Docs | `python3 scripts/ci/generate-docs-index.py --check` |

## 9. Rollout sequence

1. Census (this section) and quarantine review.
2. Duplicate/dead test report.
3. Aggregation exceptions documented.
4. Regression: region counts and case totals per change.

## 10. Acceptance matrix

| Deliverable class | Acceptance |
|---|---|
| Test file | runs focused; no suite-wide dependency |
| Quarantine | owner, reason, re-enable condition present |
| Aggregation | homogeneous only |
| Budget | bounded per TEST_POLICY |

**Non-goals unchanged:** this expansion adds census and verification detail; it does not rewrite tests.

---

## 12. Cross-plan coupling

This plan governs artifacts rather than a `.cs` domain; the domain set is the
plan's own backticked artifact list (8 files). Other plans referencing
those artifacts: **255**.

**Incoming plan edges (top 8):**

| Plan | Artifact mentions |
|---|---:|
| `PLAN-BUILD-ERGONOMICS-56` | 3 |
| `PLAN-DEPRECATED-TREE-RETIREMENT-94` | 3 |
| `PLAN-NOISE-DISCIPLINE-TRUTH-116` | 3 |
| `PLAN-ORPHAN-SEAL-01` | 2 |
| `PLAN-AGENT-WORKFLOW-GOVERNANCE-59` | 2 |
| `PLAN-SAVE-INTEGRITY-FUZZ-OPERATIONS-98` | 2 |
| `PLAN-INTEGRATION-KIT-02` | 1 |
| `PLAN-UNBLOCK-03` | 1 |

**Artifacts (first 12):**

| Artifact |
|---|
| `Ashfall.Core.Tests.csproj` |
| `Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` |
| `Assets/_Game/Shelter/NoiseDisciplineSystem.cs` |
| `NoiseDisciplineSystem.cs` |
| `PLAN-TEST-WELFARE-17_APPENDIX-A_SUITE_MAP.md` |
| `TEST_POLICY.md` |
| `docs/ci/TEST_SUITE_MAP.md` |
| `scripts/run_test.sh` |

**Package → candidate artifacts (heuristic by name overlap):**

| Package | Candidate artifacts |
|---|---|
| `TW-17A` | no name match — resolve at claim time |
| `TW-17B` | `PLAN-TEST-WELFARE-17_APPENDIX-A_SUITE_MAP.md`, `docs/ci/TEST_SUITE_MAP.md` |
| `TW-17C` | no name match — resolve at claim time |
| `TW-17D` | no name match — resolve at claim time |
| `TW-17E` | `TEST_POLICY.md` |
| `TW-17F` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk; artifacts are a starting point for the touch map, not a decision.

---

## 13. Authority binding map

Symbols used: 6. Host files: **0** · Test files: **3** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 0 | — |
| Tests (`Ashfall.Core.Tests/`) | 3 | `Ashfall.Core.Tests/Endgame/CrossRunProfileStoreTests.cs`, `Ashfall.Core.Tests/Legacy/Plan140GenerationalLegacyIntegrationTests.cs`, `Ashfall.Core.Tests/Performance/NoiseDisciplineBenchmarkTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no host attachment found for these symbols — candidate integration gap (confirm under alternate names); no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **1** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `shelter_noise` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **3** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--save-store-checksum-selftest` |
| `--save-store-checksums-selftest` |
| `--shelter-noise-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **2**.

| Event | First declaration |
|---|---|
| `OnNoiseGenerated` | `Assets/Ashfall.Core/Maritime/SafeCrackingSystem.cs` |
| `OnPolicyChanged` | `Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **5**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/death_legacy_templates.json` |
| `Assets/StreamingAssets/Data/leadership_policies.json` |
| `Assets/StreamingAssets/Data/legacy_traits.json` |
| `Assets/StreamingAssets/Data/noise_sources.json` |
| `Assets/StreamingAssets/Data/store_capability_claims.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **4** (23 files, 111 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Archaeology` | 1 | 4 |
| `Audio` | 5 | 28 |
| `Integration` | 16 | 74 |
| `Legacy` | 1 | 5 |

**Verdict:** 111 cases sit under matching regions — run those first (`Archaeology`, `Audio`, `Integration`, `Legacy`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **204**
(2 of them panels/HUD).

| Host file |
|---|
| `src/Host/AgricultureSaveStore.cs` |
| `src/Host/AirlockSecuritySaveStore.cs` |
| `src/Host/AmphibiousDraisineSaveStore.cs` |
| `src/Host/AmputationSaveStore.cs` |
| `src/Host/AnomalyHazardSaveStore.cs` |
| `src/Host/ApprenticeshipSaveStore.cs` |
| `src/Host/AquaponicsSaveStore.cs` |
| `src/Host/ArchaeologySaveStore.cs` |
| `src/Host/ArmoredCrawlerSaveStore.cs` |
| `src/Host/AshfallInputActions.cs` |
| `src/Host/AutopsySaveStore.cs` |
| `src/Host/AviationSaveStore.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **16**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `agriculture` | no |
| `airlock_security` | no |
| `amphibious_draisine` | no |
| `amputation` | no |
| `anomaly_hazard` | no |
| `apprenticeship` | no |
| `aquaponics` | no |
| `archaeology` | no |
| `armored_crawlers` | no |
| `autopsy` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **8**.

| Stream |
|---|
| `agriculture_blight` |
| `agriculture_mutation` |
| `agriculture_pest` |
| `amphibious_draisine` |
| `anomaly_hazard` |
| `aquaponics_disease` |
| `aquaponics_fry_survival` |
| `metrology_measurement_noise` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **18**
(CODEX_ONLY 8, GAMEPLAY_CONSUMED 5, OPTIONAL 2, UNRESOLVED 3).

| Catalog | Classification |
|---|---|
| `armored_crawler_modules.json` | UNRESOLVED |
| `autopsy_procedures.json` | GAMEPLAY_CONSUMED |
| `development_traits.json` | GAMEPLAY_CONSUMED |
| `excavation_hazard_mitigation.json` | GAMEPLAY_CONSUMED |
| `guilt_sources.json` | OPTIONAL |
| `ledger_debt_templates.json` | UNRESOLVED |
| `muster_faction_actions.json` | UNRESOLVED |
| `narrative/armored_cockroach_hive_logs.json` | CODEX_ONLY |
| `narrative/armored_locomotive_manifests.json` | CODEX_ONLY |
| `narrative/canyon_mudflow_hazard_reports.json` | CODEX_ONLY |

**Verdict:** 3 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **0**.

| Flag |
|---|
| — | no persistent flag shares a token with this domain |

**Verdict:** no persistent flag matches — the domain's narrative consequences are carried by other state (or the flag set is a candidate for cleanup).

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** governance · **Coupling (incoming plans):** 0
**Surface:** save sections 16 (laddered 0) · RNG streams 8 · host files 20 · catalogs 15 · test regions 4 · flags 3

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-TEST-WELFARE-17
wave: —
status: PROPOSED — foreman claim required
packages: TW-17A, TW-17B, TW-17C, TW-17D, TW-17E, TW-17F
claim paths:
  - src/Host/AgricultureSaveStore.cs  # §19 candidate host surface
  - src/Host/AirlockSecuritySaveStore.cs  # §19 candidate host surface
  - src/Host/AmphibiousDraisineSaveStore.cs  # §19 candidate host surface
  - src/Host/AmputationSaveStore.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/death_legacy_templates.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/leadership_policies.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Archaeology/
  - godot --headless --path . -- --save-store-checksum-selftest
dependencies:
  - none identified
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
