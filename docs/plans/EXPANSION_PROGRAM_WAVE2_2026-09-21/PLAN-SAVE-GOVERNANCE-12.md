# PLAN-SAVE-GOVERNANCE-12 — Save-Section Budget, Migration Policy & Fuzz Coverage

**Program:** ASHFALL Expansion & Integration Program — Wave 2 (2026-09-21)
**Status:** PROPOSED — not a claim. Foreman claim required.
**Owner role on execution:** Integrator (registry, orchestrator, envelope) with
Builders per section migration.
**Depends on:** PLAN-INTEGRATION-KIT-02 (matrix parity gate), PLAN-ORPHAN-SEAL-01
(every wired system adds save pressure).
**Expanded appendix:** [`PLAN-SAVE-GOVERNANCE-12_APPENDIX-A_SECTION_REGISTRY.md`](PLAN-SAVE-GOVERNANCE-12_APPENDIX-A_SECTION_REGISTRY.md)
— all **204 save sections** from `SaveSectionRegistry` with section key, save
method, setup method, owner, and description. SG-12A/12B packages audit this
table; every new section needs a row here plus a matrix/count update.
**Non-goals:** no save-format rewrite, no new envelope, no re-opening
`DEBT-TEST-QUARANTINE-*`, no full-suite soak in CI.

---

## 1. Outcome

Wiring 99 authorities will multiply save pressure. Before that happens, make the
save system a governed budget instead of an append-only appendage:

1. a **section budget** with a written decision tree ("ride an existing owner"
   is the default; a new section needs an integrator signature and a named
   state list);
2. a single **parity source**: `SaveSectionRegistry.All` (Core), the registry
   matrix (docs), the section-count test, and `SaveOrchestrator` must agree —
   always, in CI;
3. a **migration/support-window policy**: which game versions may load which
   envelopes, with a test that names the window;
4. **fuzz coverage per section families**, bounded and partitioned, not one
   giant fuzz run;
5. a decision on `SessionDurabilityManager` (host-pending per PLAN-UNBLOCK-03).

**Expected effect:** the orphan-seal programme can add ~40 wired stateful
systems without turning the triadic drift gate into a permanent red gate.

---

## 2. Premise evidence (2026-09-21)

| Fact | Value | Source |
|---|---:|---|
| Save stores | 181 | `ls src/Host/*SaveStore.cs \| wc -l` |
| Registry sections | 204 | `grep -cE '^\s+new\("' Assets/Ashfall.Core/Save/SaveSectionRegistry.cs` |
| Registry design | typed `SaveSectionMetadata` + aliases + lifecycle groups | `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs` |
| Section-count gate history | 173→180, then 184→186 | `KNOWN_DEBT.md` (stale-contract seals) |
| Matrix drift today | **RED (pre-existing dirty tree)** | `generate-save-store-matrix.py --check` |
| Envelope | `SchemaVersionedEnvelope`, `manifestVersion` 2, V1→V3 codecs | Plan 48 source audit |
| Fuzz gate | "Campaign Envelope Fuzzing & Mutation Gate (xUnit)" | `docs/ci/CI_GATE_MANIFEST.json` (57 gates, 53 fast) |
| Triad drift gate | "Setup/Save/AllSaveSections Triad Drift Gate" | same |
| Golden saves | `artifacts/golden_saves/` | artifacts listing |
| `SessionDurabilityManager` | host-pending (0 host refs, 10 tests) | PLAN-UNBLOCK-03 §2.2 |
| Save-compat policy | none written | Plan 48 evidence inventory item 7 |

---

## 3. Authority map

| Concern | Extend (do not create) |
|---|---|
| Section registry | `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs` (single authority) |
| Orchestration | `src/Main.SaveOrchestrator.cs`, `src/Host/SaveStoreHub.cs` |
| Envelope/codecs | `SchemaVersionedEnvelope`, per-store `FromCodec` pattern |
| Matrix | `docs/saves/SAVE_STORE_CONTRACT_MATRIX.md` (generated) |
| Count gate | `ComprehensiveSaveStoreCorruptionAndMigrationTests` section-count assertion |
| Fuzz | existing campaign-envelope fuzz suite; partition, do not duplicate |

---

## 4. Packages

### SG-12A — Section budget and decision tree
- Write `docs/saves/SAVE_SECTION_BUDGET.md`: the rule ("one aggregate section
  per owning subsystem, never per entity"), the allowed reasons for a new
  section, the maximum sections before a consolidation review (suggest 220),
  and the naming/id rules (`<domain>_<noun>`, snake_case, unique).
- Add a **size budget**: sections over a threshold (bytes or entries) require a
  justification row; retention for unbounded logs must use `RollingLog<T>`
  (Plan 55 pattern).
- **Acceptance:** every existing section has owner + file + codec; the budget
  is generated into the doc from the registry, not hand-written.
- **Verify:** `python3 scripts/ci/generate-save-store-matrix.py --check`.

### SG-12B — Single parity source
- Make `SaveSectionRegistry.All` the only list. The matrix generator reads it;
  the count gate asserts `All.Count`; the triad gate asserts every
  `SetupMethod`/`SaveMethod` exists in `Main`; `SaveOrchestrator` iterates the
  registry rather than a hand list (or asserts equality).
- **Acceptance:** adding a section without registry + orchestrator + matrix in
  one commit fails three gates, all naming the missing piece.
- **Verify:** `bash scripts/run_test.sh Ashfall.Core.Tests/Save/`;
  `bash scripts/run_test.sh Ashfall.Core.Tests/Save/SaveSectionRegistryTests.cs`.

### SG-12C — Migration and support-window policy
- Write `docs/saves/SAVE_COMPATIBILITY_POLICY.md`: game versions ↔ save
  schema versions; forward-compat rule (unknown fields ignored, never fatal);
  the support window for the current release; the migration rules (additive
  fields default safe; no silent recalculation; no RNG on load).
- Add `SaveCompatibilityWindowTests` that names the window and fails when a
  codec raises the minimum version without a policy edit.
- **Acceptance:** policy cites every codec; the test enumerates every
  `SchemaVersionedEnvelope` version pair; legacy saves load with defaults.
- **Verify:** `bash scripts/run_test.sh Ashfall.Core.Tests/Save/` +
  `godot --headless --path . -- --save-load-selftest`.

### SG-12D — Fuzz coverage per family (bounded)
- Partition the fuzz target by section family (roster/needs, shelter/power,
  economy/trade, medical, world/expedition, narrative, meta/settings) and add
  one mutation profile per family: truncation, field-type swap, out-of-range
  number, unknown enum, duplicate key, oversized string, wrong arity array.
- **Acceptance:** each family proves "load fails safe or loads with defaults,
  never crash, never partial-mutation". Bounded case count per family
  (≤ 60); aggregated reporting with per-row failures.
- **Verify:** `bash scripts/run_test.sh Ashfall.Core.Tests/Save/` (fuzz files
  run alone first per TEST_POLICY).

### SG-12E — `SessionDurabilityManager` decision
- Wire it as the soak/corruption-detection CLI authority behind
  `--session-durability-selftest`, or retire it if the envelope + backup
  stores already own recovery. Record the verdict in the registry (CO-11).
- **Acceptance:** zero host-pending save artifacts remain; no duplicate backup
  mechanism.
- **Verify:** `bash scripts/run_test.sh Ashfall.Core.Tests/Save/SessionDurabilityManagerTests.cs`.

### SG-12F — Save-write failure paths for new systems
- For every wave of PLAN-ORPHAN-SEAL-01: a failure-path probe (disk full,
  permission denied, corrupt existing file, quota) for the new section or the
  extended one, reusing the existing `Save/Load UI Failure Paths Self-Test`.
- **Acceptance:** failure paths surface a typed message and never corrupt the
  previous good save; the probe list grows with the waves.
- **Verify:** `godot --headless --path . -- --save-load-ui-failure-selftest`.

---

## 5. Risk register

| Risk | Mitigation |
|---|---|
| Budget doc becomes obsolete immediately | generated from the registry; `--check` |
| Changing orchestrator iteration risks save-order drift | order preserved (registry order); triad gate + byte-equality golden tests |
| Support window blocks legacy players | window is a policy with an explicit "best-effort" band for older saves; defaulting is tested |
| Fuzz partitioning loses coverage | family profiles derived from the section list, not hand-picked; mutation kinds fixed |
| Pre-existing matrix drift | SG-12B lands first and is allowed to regenerate the matrix once, with a stale-contract note in `KNOWN_DEBT.md` |

## 6. Verification summary

```bash
python3 scripts/ci/generate-save-store-matrix.py --check
bash scripts/run_test.sh Ashfall.Core.Tests/Save/
bash scripts/run_test.sh Ashfall.Core.Tests/Save/SaveSectionRegistryTests.cs
bash scripts/run_test.sh Ashfall.Core.Tests/ComprehensiveSaveStoreCorruptionAndMigrationTests.cs
godot --headless --path . -- --save-load-selftest
godot --headless --path . -- --save-load-ui-failure-selftest
```

## 7. Change control

The registry, orchestrator and matrix are integrator paths. Builders may add a
section only through a package that names its state fields, its failure path,
its retention rule, and its migration default. No save format version is
raised without a policy row and a window test.

---

## 6. Expanded census (bespoke: save governance surface)

This plan governs save sections, so the census reads the registry.

| Metric | Value |
|---|---:|
| Registered sections | 204 |
| Registry file names | 204 |
| Explicit schema ladders | 5 |
| Core files referencing the registry | 6 |

**Versioned sections:** `holdfast` v5, `year_of_ash` v4, `dose_ledger` v2, `expansion_hub` v4, `weight_of_choices` v2

## 7. Expanded surface: governance contract

| Rule | Detail |
|---|---|
| One registry | sections, file names, and ladders live in the registry only |
| Key stability | alias/canonical path for renames; retirement policy per Plan 87 |
| Ownership | one capture/restore pair per section; checksummed on write |
| Cleanup | registry-derived cleanup lists; no ad-hoc file lists |

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Registry parity | sections == file names == cleanup list |
| Round-trip | each section codec round-trips in its focused test |
| Ladder | every section has a declared version (Plan 87) |
| Docs | `python3 scripts/ci/generate-docs-index.py --check` |

## 9. Rollout sequence

1. Census (this section).
2. Parity check across the three registry tables.
3. Ladder declaration for unversioned sections (Plan 87).
4. Regression: parity + round-trips.

## 10. Acceptance matrix

| Deliverable class | Acceptance |
|---|---|
| Section | registered; file name mapped |
| Codec | round-trips; checksummed |
| Ladder | declared or explicitly unversioned |
| Cleanup | registry-derived |

**Non-goals unchanged:** this expansion adds census and verification detail.

---

## 12. Cross-plan coupling

This plan governs artifacts rather than a `.cs` domain; the domain set is the
plan's own backticked artifact list (9 files). Other plans referencing
those artifacts: **17**.

**Incoming plan edges (top 8):**

| Plan | Artifact mentions |
|---|---:|
| `PLAN-ORPHAN-SEAL-01` | 3 |
| `PLAN-INTEGRATION-KIT-02` | 3 |
| `PLAN-UNBLOCK-03` | 1 |
| `PLAN-LAUNCH-FACE-06` | 1 |
| `PLAN-CORE-ONLY-REGISTRY-11` | 1 |
| `PLAN-DETERMINISM-REPLAY-13` | 1 |
| `PLAN-DATA-AUTHORITY-14` | 1 |
| `PLAN-RUNTIME-PERF-16` | 1 |

**Artifacts (first 12):**

| Artifact |
|---|
| `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs` |
| `KNOWN_DEBT.md` |
| `PLAN-SAVE-GOVERNANCE-12_APPENDIX-A_SECTION_REGISTRY.md` |
| `docs/ci/CI_GATE_MANIFEST.json` |
| `docs/saves/SAVE_COMPATIBILITY_POLICY.md` |
| `docs/saves/SAVE_SECTION_BUDGET.md` |
| `docs/saves/SAVE_STORE_CONTRACT_MATRIX.md` |
| `src/Host/SaveStoreHub.cs` |
| `src/Main.SaveOrchestrator.cs` |

**Package → candidate artifacts (heuristic by name overlap):**

| Package | Candidate artifacts |
|---|---|
| `SG-12A` | `docs/saves/SAVE_SECTION_BUDGET.md`, `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs`, `PLAN-SAVE-GOVERNANCE-12_APPENDIX-A_SECTION_REGISTRY.md` |
| `SG-12B` | no name match — resolve at claim time |
| `SG-12C` | `docs/saves/SAVE_COMPATIBILITY_POLICY.md` |
| `SG-12D` | no name match — resolve at claim time |
| `SG-12E` | no name match — resolve at claim time |
| `SG-12F` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk; artifacts are a starting point for the touch map, not a decision.

---

## 13. Authority binding map

Symbols used: 9. Host files: **203** · Test files: **30** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 203 | `src/Host/AgricultureSaveStore.cs`, `src/Host/AirlockSecuritySaveStore.cs`, `src/Host/AmphibiousDraisineSaveStore.cs`, `src/Host/AmputationSaveStore.cs`, `src/Host/AnomalyHazardSaveStore.cs` |
| Tests (`Ashfall.Core.Tests/`) | 30 | `Ashfall.Core.Tests/Economy/Plan211BlackMarketHostWiringTests.cs`, `Ashfall.Core.Tests/Economy/Plan212EconomyHostWiringTests.cs`, `Ashfall.Core.Tests/Expeditions/Plan141RunFlatTireHostWiringTests.cs`, `Ashfall.Core.Tests/Foundry/Plan140HydraulicExtrusionHostWiringTests.cs`, `Ashfall.Core.Tests/Foundry/Plan213MetallurgyReconciliationTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **13** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `agriculture` |
| `airlock_security` |
| `amphibious_draisine` |
| `amputation` |
| `anomaly_hazard` |
| `black_market` |
| `black_projects_archive` |
| `draisine_recovery` |
| `economy` |
| `hydraulic_extrusion` |
| `powder_metallurgy` |
| `runflat_tire` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **18** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--agriculture-selftest` |
| `--amphibious-draisine-selftest` |
| `--black-flotilla-selftest` |
| `--economy-selftest` |
| `--economy-uitest` |
| `--hydraulic-extrusion-selftest` |
| `--ledger-debt-selftest` |
| `--port-contract-selftest` |
| `--real-main-journey-selftest` |
| `--runflat-tire-selftest` |
| `--save-store-checksum-selftest` |
| `--save-store-checksums-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **8**.

| Event | First declaration |
|---|---|
| `OnContractForgiven` | `Assets/Ashfall.Core/LedgerDebtSystem.cs` |
| `OnContractPaid` | `Assets/Ashfall.Core/LedgerDebtSystem.cs` |
| `OnContractRenegotiated` | `Assets/Ashfall.Core/LedgerDebtSystem.cs` |
| `OnContractSigned` | `Assets/Ashfall.Core/LedgerDebtSystem.cs` |
| `OnEconomyChanged` | `Assets/Ashfall.Core/Economy/IEconomyInterfaces.cs` |
| `OnHazardWarning` | `Assets/Ashfall.Core/VentilationSystem.cs` |
| `OnPolicyChanged` | `Assets/Ashfall.Core/Survivors/LeadershipSystem.cs` |
| `OnSecurityChanged` | `Assets/Ashfall.Core/AirlockSecuritySystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/agriculture_items.json` |
| `Assets/StreamingAssets/Data/amphibious_draisine_catalog.json` |
| `Assets/StreamingAssets/Data/black_flotilla_items.json` |
| `Assets/StreamingAssets/Data/black_market_inventory.json` |
| `Assets/StreamingAssets/Data/economy_goods.json` |
| `Assets/StreamingAssets/Data/excavation_hazard_mitigation.json` |
| `Assets/StreamingAssets/Data/hardcore_economy_tuning.json` |
| `Assets/StreamingAssets/Data/hydraulic_extrusion_catalog.json` |
| `Assets/StreamingAssets/Data/leadership_policies.json` |
| `Assets/StreamingAssets/Data/ledger_debt_templates.json` |
| `Assets/StreamingAssets/Data/metallurgy_recipes.json` |
| `Assets/StreamingAssets/Data/mod_manifest_schema.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **8** (81 files, 559 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Archaeology` | 1 | 4 |
| `Balance` | 1 | 5 |
| `Economy` | 41 | 329 |
| `Excavation` | 1 | 5 |
| `Governance` | 5 | 27 |
| `Integration` | 16 | 74 |
| `Inventory` | 15 | 114 |
| `PlayerCommand` | 1 | 1 |

**Verdict:** 559 cases sit under matching regions — run those first (`Archaeology`, `Balance`, `Economy`, `Excavation`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **373**
(24 of them panels/HUD).

| Host file |
|---|
| `src/Economy/EconomyMarketPanel.cs` |
| `src/Host/AgricultureHostSession.cs` |
| `src/Host/AgricultureSaveStore.cs` |
| `src/Host/AirlockSecurityHostSession.cs` |
| `src/Host/AirlockSecuritySaveStore.cs` |
| `src/Host/AmphibiousDraisineHostSession.cs` |
| `src/Host/AmphibiousDraisineSaveStore.cs` |
| `src/Host/AmputationSaveStore.cs` |
| `src/Host/AnomalyHazardSaveStore.cs` |
| `src/Host/ApprenticeshipSaveStore.cs` |
| `src/Host/AquaponicsSaveStore.cs` |
| `src/Host/ArchaeologySaveStore.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **20**, of which versioned-ladder sections:
**1**.

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
| `black_market` | no |
| `black_projects_archive` | no |

**Verdict:** matched sections include versioned ladders — a schema change here must extend the existing ladder, not fork it.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **13**.

| Stream |
|---|
| `agriculture_blight` |
| `agriculture_mutation` |
| `agriculture_pest` |
| `amphibious_draisine` |
| `anomaly_hazard` |
| `aquaponics_disease` |
| `aquaponics_fry_survival` |
| `black_market_bounty` |
| `black_market_debt_event` |
| `black_market_stock` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **35**
(CODEX_ONLY 13, GAMEPLAY_CONSUMED 18, OPTIONAL 1, UNRESOLVED 3).

| Catalog | Classification |
|---|---|
| `black_flotilla_items.json` | GAMEPLAY_CONSUMED |
| `chemical_dependency_items.json` | GAMEPLAY_CONSUMED |
| `crossing_items.json` | GAMEPLAY_CONSUMED |
| `dose_items.json` | GAMEPLAY_CONSUMED |
| `economy_goods.json` | GAMEPLAY_CONSUMED |
| `excavation_hazard_mitigation.json` | GAMEPLAY_CONSUMED |
| `excavation_sites.json` | UNRESOLVED |
| `foundry_items.json` | GAMEPLAY_CONSUMED |
| `greenhouse_items.json` | GAMEPLAY_CONSUMED |
| `hardcore_economy_tuning.json` | GAMEPLAY_CONSUMED |

**Verdict:** 3 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **1**.

| Flag |
|---|
| `flag_honored_debt` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** free-start · **Coupling (incoming plans):** 0
**Surface:** save sections 20 (laddered 1) · RNG streams 13 · host files 23 · catalogs 22 · test regions 8 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-SAVE-GOVERNANCE-12
wave: —
status: PROPOSED — foreman claim required
packages: SG-12A, SG-12B, SG-12C, SG-12D, SG-12E, SG-12F
claim paths:
  - src/Economy/EconomyMarketPanel.cs  # §19 candidate host surface
  - src/Host/AgricultureHostSession.cs  # §19 candidate host surface
  - src/Host/AgricultureSaveStore.cs  # §19 candidate host surface
  - src/Host/AirlockSecurityHostSession.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/agriculture_items.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/amphibious_draisine_catalog.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Archaeology/
  - godot --headless --path . -- --agriculture-selftest
dependencies:
  - governance spine must land first: 56 → 86 → 17 → 100
  - touches 1 versioned save ladder(s) — extend, never fork
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
