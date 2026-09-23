# PLAN-PROGRAMME-CLOSEOUT-100 — Expansion Programme Status Truth & Promotion Ledger

**Wave 8 · Kind:** GOVERNANCE · **Status:** PROPOSED — foreman claim required.
**Depends on:** every prior wave; executes last.
**Implementation scaffold:** [`PLAN-PROGRAMME-CLOSEOUT-100_APPENDIX-A_SCAFFOLD.md`](PLAN-PROGRAMME-CLOSEOUT-100_APPENDIX-A_SCAFFOLD.md) — paired with `PLAN-AGENT-WORKFLOW-GOVERNANCE-59` as scaffolding authority: source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no production code, no claim creation; the foreman owns
`INTEGRATION_PLANS.md` and `WORKTREE_OWNERSHIP.md`.

## 1. Outcome
The expansion programme now spans eight waves (86 plans before this one) plus
appendix inventories. Its own status is only as true as the hand-maintained
README tables — and one already drifted (a wave README recorded a plan count
that did not match the directory). Closeout makes programme truth **generated
and checked**: an index of plans, appendices, status lines, and verification
commands, with a promotion ledger that maps proposed plans to real claims.

| Deliverable | Detail |
|---|---|
| Programme index | generated table: plan file, wave, kind, status line, depends-on, appendix links; `--check` mode |
| Promotion ledger | each plan row: promoted (claim id) / deferred / retired, with date and evidence link |
| Claim cross-check | plans referenced by no claim; claims referencing no plan (against a snapshot of `WORKTREE_OWNERSHIP.md`, read-only) |
| Verification audit | every command cited in a plan's acceptance section must exist (`scripts/run_test.sh`, `scripts/ci/generate-docs-index.py`, named selftest flag) |
| Sunset rule | criteria under which a plan is retired rather than left ambiguous |

## 2. Evidence
- Programme directories: `docs/plans/EXPANSION_PROGRAM*_2026-09-21/` (8 waves).
- `docs/INDEX.md` is generated with `--check` — the same pattern applies to a programme index.
- `INTEGRATION_PLANS.md` (foreman) and `WORKTREE_OWNERSHIP.md` (claims) are the live ledgers; this plan reads them, never writes them.
- Drift precedent: wave README counts disagreed with directory counts (fixed by this wave's README).

## 3. Packages
- **PCL-100A** index generator (script + `--check`); README tables replaced by generated output.
- **PCL-100B** promotion ledger template + first fill from a read-only claim snapshot.
- **PCL-100C** claim cross-check report.
- **PCL-100D** verification-command audit (flags/scripts existence, no execution of broad suites).
- **PCL-100E** sunset criteria + final programme summary for the foreman.

## 4. Acceptance & verification
- Index `--check` green; adding a plan file without regenerating fails the check.
- Claim cross-check lists every plan with no claim and every claim with no plan.
- Every cited verification command exists (existence check only; no broad runs).
- `python3 scripts/ci/generate-docs-index.py --check` stays green.

## 5. Risks
Governance overhead → the index is generated, not curated; the ledger is one row per plan.
Premise staleness → ledger rows carry a snapshot date and are re-derived on demand.

---

## 6. Expanded census (bespoke: programme self-counts)

This plan governs the programme's own truth, so the census counts the programme.

| Metric | Value |
|---|---:|
| Programme directories | 19 |
| Plan files | 276 |
| Appendix files | 176 |
| Of which scaffolds | 90 |
| Plans with in-body expansions | 189 |
| Versioned generators | 41 |

## 7. Expanded surface: the closeout contract

| Rule | Detail |
|---|---|
| Generated index | plan status table generated with `--check` (no hand tables) |
| Promotion ledger | plan → claim / deferred / retired, dated |
| Claim cross-check | read-only against `WORKTREE_OWNERSHIP.md` |
| Verification audit | every cited command must exist before promotion |
| Sunset | criteria for retire rather than ambiguity |

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Index `--check` | adding a plan without regenerating fails |
| Cross-check | lists both orphans (no claim) and claims (no plan) |
| Command audit | existence check only; no broad runs |
| Docs | `python3 scripts/ci/generate-docs-index.py --check` |

## 9. Rollout sequence

1. Index generator with `--check`.
2. Promotion ledger seeded from a read-only claim snapshot.
3. Claim cross-check report.
4. Verification-command audit.
5. Sunset criteria recorded; programme summary for the foreman.

## 10. Acceptance matrix

| Deliverable class | Acceptance |
|---|---|
| Index | generated and `--check` clean |
| Ledger | every plan has a row (promoted/deferred/retired) |
| Cross-check | gaps listed, no silent omissions |
| Audit | cited commands verified to exist |

**Non-goals unchanged:** this expansion adds census and verification detail; the foreman still owns the live ledgers.

---

## 12. Cross-plan coupling

Domain method: plan-body artifact list.
Governed artifacts: 6. Other plans referencing them: **268**.

**Incoming plan edges (top 8):**

| Plan | Mentions |
|---|---:|
| `PLAN-ORPHAN-SEAL-01` | 4 |
| `PLAN-UNBLOCK-03` | 4 |
| `PLAN-AGENT-WORKFLOW-GOVERNANCE-59` | 4 |
| `PLAN-INTEGRATION-KIT-02` | 3 |
| `PLAN-LAUNCH-FACE-06` | 3 |
| `PLAN-DEBT-DRAIN-24` | 3 |
| `PLAN-VERTICAL-CULTURE-04` | 2 |
| `PLAN-VERTICAL-BODY-INDUSTRY-05` | 2 |

**Governed artifacts (first 12):**

| Artifact |
|---|
| `INTEGRATION_PLANS.md` |
| `PLAN-PROGRAMME-CLOSEOUT-100_APPENDIX-A_SCAFFOLD.md` |
| `WORKTREE_OWNERSHIP.md` |
| `docs/INDEX.md` |
| `scripts/ci/generate-docs-index.py` |
| `scripts/run_test.sh` |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidates |
|---|---|
| `PCL-100A` | `docs/INDEX.md`, `scripts/ci/generate-docs-index.py` |
| `PCL-100B` | no name match — resolve at claim time |
| `PCL-100C` | no name match — resolve at claim time |
| `PCL-100D` | no name match — resolve at claim time |
| `PCL-100E` | `PLAN-PROGRAMME-CLOSEOUT-100_APPENDIX-A_SCAFFOLD.md` |

**Reading:** incoming edges are coordination risk; candidates are a starting point for the touch map, not a decision.

---

## 13. Authority binding map

Symbols used: 6. Host files: **8** · Test files: **3** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 8 | `src/UI/AquiferTreatyConcessionPanel.cs`, `src/UI/ClandestineInsurgencyPanel.cs`, `src/UI/CrossingSafeConductVouchPanel.cs`, `src/UI/InductionCupolaFurnacePanel.cs`, `src/UI/IronCenotaphMemorialPanel.cs` |
| Tests (`Ashfall.Core.Tests/`) | 3 | `Ashfall.Core.Tests/Endgame/CrossRunProfileStoreTests.cs`, `Ashfall.Core.Tests/Legacy/Plan140GenerationalLegacyIntegrationTests.cs`, `Ashfall.Core.Tests/Tooling/DocLinkValidationGateTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **4** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `crossing` |
| `geothermal_aquifer` |
| `memorial` |
| `regional_treaty` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **13** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--crossing-selftest` |
| `--expedition-panel-lifecycle` |
| `--expedition-panel-uitest` |
| `--journal-weather-panel-selftest` |
| `--memorial-wall-selftest` |
| `--panel-bind-lifecycle-selftest` |
| `--panel-bind-selftest` |
| `--panel-lifecycle-selftest` |
| `--plans-122-125-balance-soak` |
| `--plans-122-125-selftest` |
| `--plans-139-141-selftest` |
| `--save-store-checksum-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **10**.

| Event | First declaration |
|---|---|
| `OnSafeInspected` | `Assets/Ashfall.Core/Maritime/SafeCrackingSystem.cs` |
| `OnSafeJammed` | `Assets/Ashfall.Core/Maritime/SafeCrackingSystem.cs` |
| `OnSafeOpened` | `Assets/Ashfall.Core/Maritime/SafeCrackingSystem.cs` |
| `OnTreatyDeliveryAccepted` | `Assets/Ashfall.Core/Foundry/SaltMineExtractionSystem.cs` |
| `OnTreatyDeliveryMissed` | `Assets/Ashfall.Core/Foundry/SaltMineExtractionSystem.cs` |
| `OnTreatyQuotaMet` | `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs` |
| `OnTreatyQuotaMissed` | `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs` |
| `OnTreatyStatusChanged` | `Assets/Ashfall.Core/RegionalTreatySystem.cs` |
| `OnVouchBurned` | `Assets/Ashfall.Core/VouchAccessSystem.cs` |
| `OnVouchGranted` | `Assets/Ashfall.Core/VouchAccessSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/crossing_encounters.json` |
| `Assets/StreamingAssets/Data/crossing_factions.json` |
| `Assets/StreamingAssets/Data/crossing_items.json` |
| `Assets/StreamingAssets/Data/crossing_locations.json` |
| `Assets/StreamingAssets/Data/crossing_quests.json` |
| `Assets/StreamingAssets/Data/cupola_foundry_catalog.json` |
| `Assets/StreamingAssets/Data/death_legacy_templates.json` |
| `Assets/StreamingAssets/Data/foundry_accords.json` |
| `Assets/StreamingAssets/Data/foundry_faction.json` |
| `Assets/StreamingAssets/Data/foundry_items.json` |
| `Assets/StreamingAssets/Data/foundry_production.json` |
| `Assets/StreamingAssets/Data/foundry_treaty_consequences.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **11** (105 files, 773 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Archaeology` | 1 | 4 |
| `Audio` | 5 | 28 |
| `Combat` | 10 | 84 |
| `Economy` | 41 | 329 |
| `Factions` | 10 | 72 |
| `Foundry` | 8 | 73 |
| `Integration` | 16 | 74 |
| `Legacy` | 1 | 5 |
| `Memorial` | 6 | 67 |
| `Production` | 3 | 12 |

**Verdict:** 773 cases sit under matching regions — run those first (`Archaeology`, `Audio`, `Combat`, `Economy`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **468**
(230 of them panels/HUD).

| Host file |
|---|
| `src/Economy/EconomyMarketPanel.cs` |
| `src/Economy/TradeScreenGodotPanel.cs` |
| `src/Foundry/SilentFoundryHostSession.cs` |
| `src/Host/AgricultureSaveStore.cs` |
| `src/Host/AirlockSecuritySaveStore.cs` |
| `src/Host/AmphibiousDraisineSaveStore.cs` |
| `src/Host/AmputationSaveStore.cs` |
| `src/Host/AnomalyHazardSaveStore.cs` |
| `src/Host/ApprenticeshipSaveStore.cs` |
| `src/Host/AquaponicsSaveStore.cs` |
| `src/Host/ArchaeologySaveStore.cs` |
| `src/Host/ArmoredCrawlerSaveStore.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **31**, of which versioned-ladder sections:
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
| `black_market` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **14**.

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

Matching catalogs in `artifacts/content-utilization-baseline.json`: **107**
(CODEX_ONLY 25, GAMEPLAY_CONSUMED 59, OPTIONAL 4, UNRESOLVED 19).

| Catalog | Classification |
|---|---|
| `anomalous_expedition_encounters.json` | UNRESOLVED |
| `armored_crawler_modules.json` | UNRESOLVED |
| `black_flotilla_items.json` | GAMEPLAY_CONSUMED |
| `caravan_trade_routes.json` | GAMEPLAY_CONSUMED |
| `chemical_dependency_items.json` | GAMEPLAY_CONSUMED |
| `crossing_encounters.json` | GAMEPLAY_CONSUMED |
| `crossing_factions.json` | GAMEPLAY_CONSUMED |
| `crossing_items.json` | GAMEPLAY_CONSUMED |
| `crossing_locations.json` | GAMEPLAY_CONSUMED |
| `crossing_quests.json` | GAMEPLAY_CONSUMED |

**Verdict:** 19 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **4**.

| Flag |
|---|
| `flag_betrayed_faction` |
| `flag_branch_iron_way_locked` |
| `flag_broke_treaty` |
| `flag_chosen_faction_side` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** governance · **Coupling (incoming plans):** 268
**Surface:** save sections 31 (laddered 0) · RNG streams 14 · host files 26 · catalogs 22 · test regions 10 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-PROGRAMME-CLOSEOUT-100
wave: 8
status: PROPOSED — foreman claim required
packages: PCL-100A, PCL-100B, PCL-100C, PCL-100D, PCL-100E
claim paths:
  - src/Economy/EconomyMarketPanel.cs  # §19 candidate host surface
  - src/Economy/TradeScreenGodotPanel.cs  # §19 candidate host surface
  - src/Foundry/SilentFoundryHostSession.cs  # §19 candidate host surface
  - src/Host/AgricultureSaveStore.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/crossing_encounters.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/crossing_factions.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Archaeology/
  - godot --headless --path . -- --crossing-selftest
dependencies:
  - coordinate: 268 other plan(s) name these artifacts (§12)
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
