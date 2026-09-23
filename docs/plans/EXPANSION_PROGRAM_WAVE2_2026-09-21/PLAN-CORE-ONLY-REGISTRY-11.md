# PLAN-CORE-ONLY-REGISTRY-11 — Core-Only & Dead Authority Governance

**Program:** ASHFALL Expansion & Integration Program — Wave 2 (2026-09-21)
**Status:** PROPOSED — not a claim. Foreman claim required.
**Owner role on execution:** Integrator (registry + generator) with Builders for
triage deletions.
**Depends on:** PLAN-INTEGRATION-KIT-02 K0 (reachability generator); feeds
PLAN-ORPHAN-SEAL-01 Wave 1.
**Complements:** `KNOWN_DEBT.md` (which has no Core-only class), the retired
Unity rule, and the `ContentOrphanCertificationEngine` (itself an orphan).

---

## 1. Outcome

Give the repository an explicit, gated answer to the question "is this Core
authority supposed to be in the running game?" Today there are three implicit
answers and no registry:

| State | Count (2026-09-21) | Current handling |
|---|---:|---|
| Host-reachable runtime authority | ~340 | implicit |
| Host-unreachable authority with tests | 99 files | *unhandled — read as "integrated" by ledger rows* |
| Type-level dead authority | 5 types | *unhandled* |

Deliverables: `docs/architecture/CORE_ONLY_AUTHORITIES.md` (registry),
a generator that keeps it true, a retirement protocol, and the triage of the
current 5 dead types + the governance/tooling subset of the 99.

**Expanded appendix:** [`PLAN-CORE-ONLY-REGISTRY-11_APPENDIX-A_AUTHORITY_CENSUS.md`](PLAN-CORE-ONLY-REGISTRY-11_APPENDIX-A_AUTHORITY_CENSUS.md)
— the Core authority census: **454 types** classified REACHABLE/ORPHAN/DEAD (registry seed).

**Non-goals:** no gameplay wiring (Plans 01/04/05); no deletion of a system
that a pending signed disposition needs (DEC-21…44); no re-opening RETIRED debt
rows.

---

## 2. Premise evidence (2026-09-21)

| Fact | Value | Command / source |
|---|---:|---|
| Core `.cs` files | 1,168 | `find Assets/Ashfall.Core -name '*.cs'` |
| Core authority types | 442 | reachability audit (`tools/reachability-audit.py`) |
| Host-unreachable authority files | 99 | same |
| Fully dead authority types | 5 | same; `PowderMetallurgyEngine`, `LyophilizationEngine`, `NvisC4ISystem`, `DraisineRecoverySystem`, `ArmoredDraisineRecoverySystem` |
| Registry for Core-only authorities | none | `ls docs/architecture` |
| `ContentOrphanCertificationEngine` (Plan 49) | host-unreachable | 99-file inventory |
| `generate-core-systems-catalog.py` | hand-maintained list | script header |
| `KNOWN_DEBT.md` row types | ACCEPTED/BLOCKED/QUARANTINED/RETIRED/PROMOTED | no `CORE_ONLY` or `DEAD_CODE` |
| Debt rows that are genuinely Core-only in intent | e.g. `SessionDurabilityManager`, `PlayableMetricsAggregationEngine`, `AccessibilitySettingsSystem`, `ModSupportSystem` | audit §2 |

The gap is structural: a Core-only **governance** authority (a certification
engine, a telemetry recorder, a mod contract) is indistinguishable at the
ledger level from a Core-only **forgotten** authority (a gameplay system with
no host path). The first is correct and should be named; the second is the
programme's target. Without the registry, both read as "Core delivered".

---

## 3. Authority map

| Concern | Extend (do not create) |
|---|---|
| Registry | new `docs/architecture/CORE_ONLY_AUTHORITIES.md`, generated; **not** a second `KNOWN_DEBT.md` |
| Generator | `scripts/ci/generate-authority-reachability.py` from KIT-02 gains a `--registry` mode |
| Debt | `KNOWN_DEBT.md` gains two status meanings: `CORE_ONLY` (accepted) and `DEAD_CODE` (triage queue), or the registry references debt ids |
| Content certification | `ContentOrphanCertificationEngine` becomes a CLI-only checker consumed by the kit's gate (or is retired if `--content-utilization-selftest` covers it) |
| CI | `agent-fast-verify.py` gains the registry drift check |

---

## 4. Packages

### CO-11A — Registry and generator
- **Outcome:** one generated markdown registry with one row per Core-only or
  dead authority type: `type | file | class (CORE_ONLY | DEAD_CODE |
  INTEGRATION_PENDING) | reason | owner | promotion condition | evidence`.
- **Rules:** every row needs a reason; `INTEGRATION_PENDING` rows must link the
  PLAN-ORPHAN-SEAL-01 wave that will consume them; `DEAD_CODE` rows must have a
  deletion ticket.
- **Acceptance:** `--check` green; registry enumerates all 99 + 5 today;
  generator re-run is deterministic (sorted output, no timestamps in body).
- **Verify:** `python3 scripts/ci/generate-authority-reachability.py --registry --check`.

### CO-11B — Triage: dead types (5)
- **`PowderMetallurgyEngine`** — wire through `SilentFoundryHostSession`
  (sanctioned B66 powder authority) or delete; decide with the foundry owner.
- **`LyophilizationEngine`** — wire in the table/water package (B5-2) or delete.
- **`NvisC4ISystem`** — wire in the comms package (B5-5) or retire if
  `RadioPropagationEngine` + `CommunicationsSystem` supersede.
- **`DraisineRecoverySystem`/`ArmoredDraisineRecoverySystem`** — retire or fold
  into `RailTrackMaintenanceEngine` (itself integration-pending).
- **Acceptance:** zero `DEAD_CODE` rows; each decision cites focused evidence;
  deletion removes the test file and any catalog if the type is the sole
  consumer.
- **Verify:** reachability audit + focused foundry/rail/medical/comms suites.

### CO-11C — Triage: Core-only governance authorities
Bootstrap allowlist (each must still prove a consumer):
- `ContentOrphanCertificationEngine` → CLI gate consumer;
- `SessionDurabilityManager` → save/soak CLI;
- `PlayableMetricsAggregationEngine` → telemetry CLI + local JSONL;
- `AccessibilitySettingsSystem` → settings pipeline (likely wire to settings,
  not Core-only);
- `ModSupportSystem` → loader pipeline (likely wire to mod layer);
- `DifficultySettingsSystem` → difficulty binding (U7).
- **Acceptance:** every row either becomes reachable or is registered
  `CORE_ONLY` with a named CLI consumer; no "miscellaneous" bucket.
- **Verify:** `--integration-selftest` + `--content-utilization-selftest`.

### CO-11D — Retirement protocol
- Write `docs/architecture/AUTHORITY_RETIREMENT_PROTOCOL.md`: evidence steps
  (repo-wide references, test references, catalog consumer, save references,
  docs references) → delete source + test + catalog + save section if sole
  owner → add a `KNOWN_DEBT` `RETIRED` row with the promotion condition → run
  the reachability/registry gates → record the deletion in the closeout.
- **Acceptance:** protocol exercised once on the draisine pair; no
  half-deleted artifacts (a catalog without a loader, a save section without a
  codec).
- **Verify:** `python3 scripts/ci/generate-authority-reachability.py --registry --check`;
  `python3 scripts/ci/generate-catalog-registry.py --check`.

### CO-11E — Quarterly sweep
- The kit's gate runs per PR; this plan adds a scheduled sweep report
  (`docs/reports/authority-reachability-<date>.md`) so drift is visible even
  when no PR touches Core.
- **Acceptance:** first report committed; it lists counts and new rows only.

---

## 5. Risk register

| Risk | Mitigation |
|---|---|
| A registry becomes a dumping ground | every row has owner + promotion condition; `--check` fails on blank fields |
| Deleting a system a signed disposition needs | cross-check DEC-21…44 artifact table in PLAN-UNBLOCK-03 §2.2 before deletion |
| Registry drifts from the generated truth | registry is generated, never hand-edited; CI `--check` |
| Core-only hides a real gameplay orphan | `CORE_ONLY` requires a named consumer and a test or CLI proof |

## 6. Verification summary

```bash
python3 scripts/ci/generate-authority-reachability.py --registry --check
python3 docs/plans/EXPANSION_PROGRAM_2026-09-21/tools/reachability-audit.py | tail -8
python3 scripts/ci/generate-catalog-registry.py --check
godot --headless --path . -- --integration-selftest
```

## 7. Change control

Registry and protocol are docs + one generator; only CU-11B/11C deletions touch
Core. Each deletion is its own package with a claim. No retirement row may be
deleted from `KNOWN_DEBT.md` without a new forensic gap.

---

## 6. Expanded census (bespoke: authority registry surface)

This plan classifies Core authorities, so the census counts them.

| Metric | Value |
|---|---:|
| Core authority types (`*System|Engine|Coordinator|Manager`) | 454 |
| Known orphan authorities (Plan 1 audit) | 99 |
| Known dead types | 5 |
| Reachable (derived) | 350 |

## 7. Expanded surface: registry contract

| Rule | Detail |
|---|---|
| Reachable | has a named live consumer (host or Core path from host roots) |
| Orphan | no host path; keep for seal (Plan 1) or retire |
| Core-only | governance/tooling types registered with a named consumer and reason |
| Dead | no references anywhere; retire or revive with an owner |

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Census | regenerated per batch; counts compared |
| Registry rows | every authority has a class |
| Drift | a newly reachable authority leaves the orphan list |
| Docs | `python3 scripts/ci/generate-docs-index.py --check` |

## 9. Rollout sequence

1. Census (this section).
2. Registry classification pass.
3. Orphan/dead triage with owners.
4. Regression: census delta.

## 10. Acceptance matrix

| Deliverable class | Acceptance |
|---|---|
| Authority | classified with a reason |
| Consumer | named, live, or explicit Core-only |
| Orphan | routed to Plan 1's seal or retired |
| Dead | owner or retirement recorded |

**Non-goals unchanged:** this expansion adds census and verification detail.

---

## 12. Cross-plan coupling

This plan governs artifacts rather than a `.cs` domain; the domain set is the
plan's own backticked artifact list (8 files). Other plans referencing
those artifacts: **15**.

**Incoming plan edges (top 8):**

| Plan | Artifact mentions |
|---|---:|
| `PLAN-ORPHAN-SEAL-01` | 4 |
| `PLAN-INTEGRATION-KIT-02` | 4 |
| `PLAN-UNBLOCK-03` | 2 |
| `PLAN-LAUNCH-FACE-06` | 2 |
| `EVIDENCE` | 2 |
| `PLAN-SAVE-GOVERNANCE-12` | 1 |
| `PLAN-RUNTIME-PERF-16` | 1 |
| `PLAN-NARRATIVE-GRAPH-18` | 1 |

**Artifacts (first 12):**

| Artifact |
|---|
| `KNOWN_DEBT.md` |
| `PLAN-CORE-ONLY-REGISTRY-11_APPENDIX-A_AUTHORITY_CENSUS.md` |
| `agent-fast-verify.py` |
| `docs/architecture/AUTHORITY_RETIREMENT_PROTOCOL.md` |
| `docs/architecture/CORE_ONLY_AUTHORITIES.md` |
| `generate-core-systems-catalog.py` |
| `scripts/ci/generate-authority-reachability.py` |
| `tools/reachability-audit.py` |

**Package → candidate artifacts (heuristic by name overlap):**

| Package | Candidate artifacts |
|---|---|
| `CO-11A` | `PLAN-CORE-ONLY-REGISTRY-11_APPENDIX-A_AUTHORITY_CENSUS.md` |
| `CO-11B` | no name match — resolve at claim time |
| `CO-11C` | `docs/architecture/CORE_ONLY_AUTHORITIES.md` |
| `CO-11D` | `docs/architecture/AUTHORITY_RETIREMENT_PROTOCOL.md` |
| `CO-11E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk; artifacts are a starting point for the touch map, not a decision.

---

## 13. Authority binding map

Symbols used: 8. Host files: **0** · Test files: **1** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 0 | — |
| Tests (`Ashfall.Core.Tests/`) | 1 | `Ashfall.Core.Tests/Tooling/SaveStateRoundTripCoverageGateTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no host attachment found for these symbols — candidate integration gap (confirm under alternate names); no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **0** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| — | no section key shares a token with this domain |

**Verdict:** no section key shares a token with this domain — either the authority is derived/stateless, or its persistence key is named after a different owner. Not a conclusion; verify in the owning system.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **3** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--asset-coverage-report` |
| `--census-selftest` |
| `--ledger-debt-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **3**.

| Event | First declaration |
|---|---|
| `OnBarterOnlyModeChanged` | `Assets/Ashfall.Core/Economy/IEconomyInterfaces.cs` |
| `OnCensusUpdated` | `Assets/Ashfall.Core/CensusClaimSystem.cs` |
| `OnSteamTrip` | `Assets/Ashfall.Core/BrineWaterSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/decontamination_protocol_catalog.json` |
| `Assets/StreamingAssets/Data/economy_goods.json` |
| `Assets/StreamingAssets/Data/hardcore_economy_tuning.json` |
| `Assets/StreamingAssets/Data/ledger_debt_templates.json` |
| `Assets/StreamingAssets/Data/narrative/blast_gate_mechanical_audits.json` |
| `Assets/StreamingAssets/Data/narrative/brine_pickling_barrel_spoilage.json` |
| `Assets/StreamingAssets/Data/narrative/steam_trap_water_hammer_logs.json` |
| `Assets/StreamingAssets/Data/narrative/sweet_water_glycerin_assays.json` |
| `Assets/StreamingAssets/Data/narrative/water_clock_orifice_silt_records.json` |
| `Assets/StreamingAssets/Data/narrative/water_quality_test_reports_batch_2.json` |
| `Assets/StreamingAssets/Data/radiation_economy_social.json` |
| `Assets/StreamingAssets/Data/water_sources.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **8** (171 files, 1381 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Balance` | 1 | 5 |
| `Combat` | 10 | 84 |
| `Economy` | 41 | 329 |
| `Integration` | 16 | 74 |
| `NarrativeConsequence` | 1 | 20 |
| `Radiation` | 10 | 78 |
| `Shelter` | 87 | 754 |
| `Water` | 5 | 37 |

**Verdict:** 1381 cases sit under matching regions — run those first (`Balance`, `Combat`, `Economy`, `Integration`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **24**
(6 of them panels/HUD).

| Host file |
|---|
| `src/Economy/EconomyMarketPanel.cs` |
| `src/Host/AssetCoverageReport.cs` |
| `src/Host/AssetCoverageScanner.cs` |
| `src/Host/EconomyHostSession.cs` |
| `src/Host/EconomySaveStore.cs` |
| `src/Host/UniqueClaimSaveStore.cs` |
| `src/Host/WaterCondenserHostSession.cs` |
| `src/Host/WaterCondenserSaveStore.cs` |
| `src/Host/WaterTreatmentHostSession.cs` |
| `src/Host/WaterTreatmentSaveStore.cs` |
| `src/Main.AdvancedShelterSystems.cs` |
| `src/Main.DebtCredit.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **25**, of which versioned-ladder sections:
**1**.

| Section key | Laddered |
|---|---|
| `black_market` | no |
| `decontamination` | no |
| `dose_ledger` | yes |
| `economy` | no |
| `expanded_shelter` | no |
| `radiation` | no |
| `shelter` | no |
| `shelter_assignment` | no |
| `shelter_atmosphere` | no |
| `shelter_barter` | no |

**Verdict:** matched sections include versioned ladders — a schema change here must extend the existing ladder, not fork it.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **7**.

| Stream |
|---|
| `advanced_mfg_ebpvd_coating` |
| `black_market_bounty` |
| `black_market_debt_event` |
| `black_market_stock` |
| `economy` |
| `shelter` |
| `social` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **155**
(CODEX_ONLY 143, GAMEPLAY_CONSUMED 5, OPTIONAL 3, UNRESOLVED 4).

| Catalog | Classification |
|---|---|
| `audio_logs_expansion_05.json` | OPTIONAL |
| `decontamination_protocol_catalog.json` | GAMEPLAY_CONSUMED |
| `economy_goods.json` | GAMEPLAY_CONSUMED |
| `guilt_sources.json` | OPTIONAL |
| `hardcore_economy_tuning.json` | GAMEPLAY_CONSUMED |
| `ledger_debt_templates.json` | UNRESOLVED |
| `narrative/ammonia_chiller_leak_logs.json` | CODEX_ONLY |
| `narrative/apiculture_red_light_audits.json` | CODEX_ONLY |
| `narrative/architect_vault_audits.json` | CODEX_ONLY |
| `narrative/armored_cockroach_hive_logs.json` | CODEX_ONLY |

**Verdict:** 4 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

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
**Surface:** save sections 25 (laddered 1) · RNG streams 7 · host files 20 · catalogs 22 · test regions 8 · flags 3

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-CORE-ONLY-REGISTRY-11
wave: —
status: PROPOSED — foreman claim required
packages: CO-11A, CO-11B, CO-11C, CO-11D, CO-11E
claim paths:
  - src/Economy/EconomyMarketPanel.cs  # §19 candidate host surface
  - src/Host/AssetCoverageReport.cs  # §19 candidate host surface
  - src/Host/AssetCoverageScanner.cs  # §19 candidate host surface
  - src/Host/EconomyHostSession.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/decontamination_protocol_catalog.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/economy_goods.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Balance/
  - godot --headless --path . -- --asset-coverage-report
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
