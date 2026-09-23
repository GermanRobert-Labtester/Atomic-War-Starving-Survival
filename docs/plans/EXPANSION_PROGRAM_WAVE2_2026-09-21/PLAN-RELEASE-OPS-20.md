# PLAN-RELEASE-OPS-20 — Gate Reliability, Workflow Hygiene & Repository Health

**Program:** ASHFALL Expansion & Integration Program — Wave 2 (2026-09-21)
**Status:** PROPOSED — not a claim. Foreman claim required. This plan is
operations, not versioning; versioning/changelog/tagging is PLAN-LAUNCH-FACE-06
F6-2.
**Owner role on execution:** Integrator (CI/workflows, repo hygiene) with
Builders for gate repairs.
**Depends on:** PLAN-LAUNCH-FACE-06 F6-2 (release craft) for `release-gate.sh`
ownership; PLAN-ASSET-PIPELINE-19 AP-19D for LFS findings; PLAN-INTEGRATION-KIT-02
for new gate wiring.
**Expanded appendix:** [`PLAN-RELEASE-OPS-20_APPENDIX-A_GATE_CENSUS.md`](PLAN-RELEASE-OPS-20_APPENDIX-A_GATE_CENSUS.md)
— the full **57-gate census** from `CI_GATE_MANIFEST.json`: id, name,
category, classification, criticality, timeout, and command per gate — the
RO-20A reliability census input.
**Non-goals:** no history rewrite without explicit user approval; no gate
deletion without replacement evidence; no secrets in any artifact.

---

## 1. Outcome

The repository has 57 named gates, 6 workflows, 3,858 LFS objects and a 1.8 GB
`.git`. Audit evidence shows the gate list is strong but uneven: the Plan 48
premise audit found the export workflow could not fail its verification steps
and never booted a build; `release-gate.sh` does not exist; and at audit time
several fast gates were red from the working tree. Operations hygiene is the
difference between a gate list and a trustworthy gate.

Deliverables:

1. a **gate reliability census**: for all 57 gates, what they check, whether
   they can fail, what they report, and their quarantine/flake status;
2. **workflow hygiene**: no dead workflow, honest failure paths, caching and
   concurrency, least-privilege permissions;
3. **repository health**: LFS object health, orphan/duplicate assets, quarantine
   archive verification, clone-size report — report first, delete only with
   approval;
4. **dependency hygiene**: CPM consistency, SDK pin, license/SPDX coverage,
   vulnerability scan;
5. **artifact and generated-file operations**: ownership map, growth policy,
   regeneration gates;
6. **operational runbooks**: rollback, restore, incident, secrets policy.

---

## 2. Premise evidence (2026-09-21)

| Fact | Value | Source |
|---|---:|---|
| CI gates | 57 (53 fast) | `docs/ci/CI_GATE_MANIFEST.json` |
| Workflows | `build.yml`, `ci.yml`, `docs-regen.yml`, `hotfix.yml`, `release.yml`, `selftest-manifest-regen.yml` | `.github/workflows/` |
| Export honesty | Plan 48 audit: raw `godot --export-release`, verification steps cannot fail, no boot smoke | Plan 48 evidence inventory |
| `release-gate.sh` | absent | same |
| Git tags | 0 | Plan 48 item 1 |
| LFS objects | 3,858 | `git lfs ls-files` |
| `.git` / pack | 1.8 GB / 134.17 MiB pack | `git count-objects -vH`, `du` |
| Quarantine archive | `Twin_ASHFall/quarantine/2026-09-12-worktree-deletions/` (2,506 files, SHA256SUMS) | `KNOWN_DEBT` `DEBT-WORKTREE-DECLUTTER-2026-09-12` |
| Docs index | 2,861 documents | `generate-docs-index.py` |
| Rulebook clients | 13 synchronized from `AGENTS.md` | `sync-agent-rulebooks.py` |
| Package pinning | `Directory.Packages.props`, `global.json` | repo root |
| Skills catalog | generated + gated | `generate-agent-skills-catalog.py` |
| License headers | 1,867 C# files SPDX MIT; `license-header-check.sh --strict` | `KNOWN_DEBT` |

---

## 3. Authority map

| Concern | Extend (do not create) |
|---|---|
| Gate registry | `docs/ci/CI_GATE_MANIFEST.json` + `generate-selftest-manifest.py`, `agent-fast-verify.py` |
| Workflows | `.github/workflows/*` (consolidate, do not proliferate) |
| Release gate | `scripts/ci/release-gate.sh` — ownership is F6-2; this plan wires honesty checks |
| Hygiene scripts | `asset-orphan-sweep.sh`, `git-object-inventory.sh`, `detect-corpus-duplicates.py`, `case-collision-gate.sh` |
| Generated docs | `generate-docs-index.py`, `generate-architecture-map.py`, `generate-save-store-matrix.py`, `generate-catalog-registry.py`, `sync-agent-rulebooks.py` |
| Package pinning | `Directory.Packages.props`, `global.json`, `Directory.Build.props` |

---

## 4. Packages

### RO-20A — Gate reliability census
- For each of the 57 gates record: trigger (PR/nightly/release), command, can-it-
  fail (does it exit non-zero on a real defect), output format (summary/JSON),
  runtime class, known flakes, and current status.
- Add a **truthfulness probe** for the highest-value gates: inject a known
  defect into a scratch fixture and assert the gate fails (bounded, not in the
  main suite).
- **Acceptance:** census committed; every gate either proven able to fail or
  flagged `advisory` with a reason; no gate silently passes.
- **Verify:** `python3 scripts/ci/agent-fast-verify.py`; the probe script.

### RO-20B — Workflow hygiene
- Apply: explicit `permissions`, concurrency groups (cancel superseded), cache
  for NuGet/Godot imports, pinned action versions, least-privilege tokens, and
  fail-fast ordering. Remove or repurpose any workflow that cannot fail.
- Coordinate the export workflow with F6-2: use `scripts/ci/godot-export-linux.sh`,
  add a boot smoke that can fail, and upload artifacts only after the smoke.
- **Acceptance:** every workflow has a failure path that fails the run; no
  duplicated job; concurrency + caching in place; run durations measured.
- **Verify:** workflow lint (actionlint if available) + a dry PR run.

### RO-20C — Repository health (report first)
- Run `git-object-inventory.sh`, `git lfs fsck`, orphan/duplicate sweeps.
  Produce `docs/reports/REPO_HEALTH_<date>.md`: LFS health, large objects,
  duplicate corpora, untracked artifact growth, quarantine archive
  verification (checksums).
- Remediation is **proposal-only** in this package: file a list with sizes and
  a recommended action; destructive steps require explicit user approval and a
  separate package.
- **Acceptance:** report committed; quarantine archive checksums verified; no
  deletion performed without approval.
- **Verify:** the listed scripts in read-only/report mode.

### RO-20D — Dependency hygiene
- Verify CPM: every `PackageReference` version comes from
  `Directory.Packages.props`; no floating versions; SDK pinned by
  `global.json`; `Directory.Build.props` target frameworks consistent
  (Core netstandard2.1, host net8.0, tests net9.0).
- Add a vulnerability scan (offline-capable) and an SPDX header check over new
  files; keep the license gate strict.
- **Acceptance:** CPM gate green; no floating versions; header check green;
  vulnerability scan reported.
- **Verify:** `bash scripts/ci/` CPM gate + `license-header-check.sh --strict`.

### RO-20E — Artifact and generated-file operations
- Publish an ownership map: which generated files exist, who regenerates them,
  which gate checks them, and what may be committed. Add a growth policy for
  `artifacts/` (baselines are tracked; run outputs are not).
- **Acceptance:** every generated file has an owner + generator + gate row;
  `artifacts/` growth bounded; no untracked run output in the release tree.
- **Verify:** `generate-docs-index.py --check` and siblings; a new artifact
  policy gate.

### RO-20F — Operational runbooks
- Write: rollback runbook (binary + save compatibility), restore runbook
  (backup/quarantine recovery), incident runbook (red main, broken release),
  and a secrets policy (never request/print/store keys; `.env` excluded and
  never committed).
- **Acceptance:** runbooks reviewed by the foreman; each names exact commands
  and owner; secrets policy stated in the workflow docs.
- **Verify:** docs link gate; a tabletop run of the rollback steps in a scratch
  clone.

---

## 5. Risk register

| Risk | Mitigation |
|---|---|
| Gate growth increases CI time | census includes runtime; consolidation target; caching |
| Removing a "dead" workflow deletes real coverage | replacement evidence required; advisory period first |
| Repo hygiene drifts into history rewrite | report-only; approval gate; no `filter-repo` without explicit user instruction |
| Vulnerability scan false alarms | bounded report, triage list with reasons |
| Runbooks rot | linked from CI docs; reviewed each release |

## 6. Verification summary

```bash
python3 scripts/ci/agent-fast-verify.py
bash scripts/ci/git-object-inventory.sh
git lfs fsck
bash scripts/ci/asset-orphan-sweep.sh
bash scripts/ci/case-collision-gate.sh
python3 scripts/ci/generate-docs-index.py --check
python3 scripts/ci/sync-agent-rulebooks.py --check
bash scripts/ci/license-header-check.sh --strict
```

## 7. Change control

CI and workflow files are integrator-owned. No secret value is ever added to
the repository. Destructive hygiene actions require explicit user approval and
a `KNOWN_DEBT`/verification row. Gates may be demoted to advisory only with a
written reason and a promotion condition.

---

## 6. Expanded census (bespoke: release operations surface)

This plan governs release gates, so the census counts the gates, scripts, and
workflows.

| Metric | Value |
|---|---:|
| CI gates in the manifest | 57 |
| Release/gate scripts | 29 |
| Workflow files | 6 |

**Scripts:** `asset-decode-gate.cpython-312.pyc`, `run-gates.cpython-312.pyc`, `asset-decode-gate.py`, `audio-asset-gate.py`, `case-collision-gate.sh`, `catch-policy-gate.sh`, `content-acceptance-gate.sh`, `coverage-gate.sh`, `doc-link-gate.sh`, `forbidden-api-gate.sh`
**Workflows:** `build.yml`, `ci.yml`, `docs-regen.yml`, `hotfix.yml`, `release.yml`, `selftest-manifest-regen.yml`

## 7. Expanded surface: release contract

| Rule | Detail |
|---|---|
| Gate set | the manifest is the source of truth; a gate cited in a plan must exist |
| Fast/slow split | fast gates run per change; slow gates per release |
| Evidence | a release records gate results and artifact hashes |
| Rollback | the hotfix drill (Plan 99) proves the path |

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Manifest parity | gate ids unique; commands exist |
| Fast gate time | bounded per gate; total under the fast budget |
| Artifact hash | recorded per release |
| Docs | `python3 scripts/ci/generate-docs-index.py --check` |

## 9. Rollout sequence

1. Census (this section).
2. Manifest parity and command-existence check.
3. Fast/slow classification review.
4. Regression: gate set diff per release.

## 10. Acceptance matrix

| Deliverable class | Acceptance |
|---|---|
| Gate | in the manifest; command exists; classification stated |
| Script | referenced by a gate or doc |
| Release | evidence recorded (results + hashes) |
| Rollback | drill-proven |

**Non-goals unchanged:** this expansion adds census and verification detail.

---

## 12. Cross-plan coupling

This plan governs artifacts rather than a `.cs` domain; the domain set is the
plan's own backticked artifact list (33 files). Other plans referencing
those artifacts: **60**.

**Incoming plan edges (top 8):**

| Plan | Artifact mentions |
|---|---:|
| `PLAN-LAUNCH-FACE-06` | 13 |
| `PLAN-INTEGRATION-KIT-02` | 7 |
| `PLAN-ASSET-PIPELINE-19` | 5 |
| `PLAN-INPUT-HARDENING-25` | 5 |
| `PLAN-UNBLOCK-03` | 4 |
| `PLAN-SELFTEST-TRUTH-23` | 4 |
| `PLAN-ARCHITECTURE-BOUNDARY-31` | 4 |
| `PLAN-AGENT-WORKFLOW-GOVERNANCE-59` | 4 |

**Artifacts (first 12):**

| Artifact |
|---|
| `AGENTS.md` |
| `CI_GATE_MANIFEST.json` |
| `PLAN-RELEASE-OPS-20_APPENDIX-A_GATE_CENSUS.md` |
| `agent-fast-verify.py` |
| `asset-decode-gate.py` |
| `asset-orphan-sweep.sh` |
| `audio-asset-gate.py` |
| `build.yml` |
| `case-collision-gate.sh` |
| `catch-policy-gate.sh` |
| `ci.yml` |
| `content-acceptance-gate.sh` |

**Package → candidate artifacts (heuristic by name overlap):**

| Package | Candidate artifacts |
|---|---|
| `RO-20A` | `PLAN-RELEASE-OPS-20_APPENDIX-A_GATE_CENSUS.md` |
| `RO-20B` | no name match — resolve at claim time |
| `RO-20C` | no name match — resolve at claim time |
| `RO-20D` | no name match — resolve at claim time |
| `RO-20E` | no name match — resolve at claim time |
| `RO-20F` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk; artifacts are a starting point for the touch map, not a decision.

---

## 13. Authority binding map

Symbols used: 30. Host files: **71** · Test files: **72** · Data files: **62**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 71 | `src/Audio/AudioEventBridge.cs`, `src/Economy/TradeScreenGodotPanel.cs`, `src/Foundry/SilentFoundryHostSession.cs`, `src/Host/CatalogPath.cs`, `src/Host/ChemicalReconHostSession.cs` |
| Tests (`Ashfall.Core.Tests/`) | 72 | `Ashfall.Core.Tests/ActionResultTests.cs`, `Ashfall.Core.Tests/CatalogIntegrityValidatorTests.cs`, `Ashfall.Core.Tests/Codex/CodexProjectionTests.cs`, `Ashfall.Core.Tests/CollectibleNarrativeQualityTests.cs`, `Ashfall.Core.Tests/CrossingItemsPlan126Tests.cs` |
| Data (`StreamingAssets/Data/`) | 62 | `Assets/StreamingAssets/Data/bunker_graffiti_postings.json`, `Assets/StreamingAssets/Data/confession_secrets.json`, `Assets/StreamingAssets/Data/crossing_quests.json`, `Assets/StreamingAssets/Data/currents.json`, `Assets/StreamingAssets/Data/door_encounters.json` |

**Verdict:** all three layers attach — surface is bound end to end.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **20** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `caravan_trade_network` |
| `chemical_dependency` |
| `chemical_recon` |
| `chemical_synthesis` |
| `collectible_discovery` |
| `crossing` |
| `dynamic_quests` |
| `encounters` |
| `foundry` |
| `holdfast_trade` |
| `host_event` |
| `inventory` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **190** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--7-day-smoke-selftest` |
| `--accessibility-selftest` |
| `--advanced-industrial-recon-selftest` |
| `--agriculture-selftest` |
| `--all-expansions-selftest` |
| `--amphibious-draisine-selftest` |
| `--aquaponics-selftest` |
| `--arbitration-selftest` |
| `--asset-coverage-report` |
| `--asset-registry-selftest` |
| `--atmosphere-selftest` |
| `--audio-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **16**.

| Event | First declaration |
|---|---|
| `OnActionCompleted` | `Assets/Ashfall.Core/WorkshopReverseEngineeringSystem.cs` |
| `OnActionExecuted` | `Assets/Ashfall.Core/Warlords/WarlordDoctrineSystem.cs` |
| `OnActionResolved` | `Assets/Ashfall.Core/Muster/FactionActionBoard.cs` |
| `OnActionSelected` | `Assets/Ashfall.Core/UtilityAI/UtilityAiSystem.cs` |
| `OnBlightNarrative` | `Assets/Ashfall.Core/Farming/AgricultureSystem.cs` |
| `OnCaseCompleted` | `Assets/Ashfall.Core/AutopsySystem.cs` |
| `OnCensusUpdated` | `Assets/Ashfall.Core/CensusClaimSystem.cs` |
| `OnCodexUnlocked` | `Assets/Ashfall.Core/Journal/JournalSystem.cs` |
| `OnCombatEvent` | `Assets/Ashfall.Core/Combat/TacticalCombatSystem.cs` |
| `OnCraftResultOverflow` | `Assets/Ashfall.Core/Crafting/CraftingSystem.cs` |
| `OnEventRaised` | `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs` |
| `OnInventoryChanged` | `Assets/Ashfall.Core/Inventory/Inventory.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/agriculture_items.json` |
| `Assets/StreamingAssets/Data/anomalous_expedition_encounters.json` |
| `Assets/StreamingAssets/Data/asset_registry.json` |
| `Assets/StreamingAssets/Data/audio_accessibility_cues.json` |
| `Assets/StreamingAssets/Data/audio_cues.json` |
| `Assets/StreamingAssets/Data/audio_logs_expansion_05.json` |
| `Assets/StreamingAssets/Data/autopsy_procedures.json` |
| `Assets/StreamingAssets/Data/black_flotilla_items.json` |
| `Assets/StreamingAssets/Data/black_market_inventory.json` |
| `Assets/StreamingAssets/Data/bounty_board.json` |
| `Assets/StreamingAssets/Data/bunker_graffiti_postings.json` |
| `Assets/StreamingAssets/Data/caravan_trade_routes.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **14** (218 files, 1849 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Accessibility` | 1 | 6 |
| `Audio` | 5 | 28 |
| `Codex` | 2 | 29 |
| `Combat` | 10 | 84 |
| `Crafting` | 1 | 11 |
| `Economy` | 41 | 329 |
| `Foundry` | 8 | 73 |
| `Integration` | 16 | 74 |
| `Inventory` | 15 | 114 |
| `Narrative` | 26 | 293 |

**Verdict:** 1849 cases sit under matching regions — run those first (`Accessibility`, `Audio`, `Codex`, `Combat`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **600**
(230 of them panels/HUD).

| Host file |
|---|
| `src/Audio/AudioConditionHostBridge.cs` |
| `src/Audio/AudioCueCatalog.cs` |
| `src/Audio/AudioEventBridge.cs` |
| `src/Audio/AudioManager.cs` |
| `src/Audio/AudioSelfTest.cs` |
| `src/Audio/AudioSettings.cs` |
| `src/Audio/AudioStateCoordinator.cs` |
| `src/Audio/ExpansionAudioBridge.cs` |
| `src/Audio/ShelterAudioController.cs` |
| `src/Audio/ShelterOperationsAudioBridge.cs` |
| `src/Disease/DiseaseHostSession.cs` |
| `src/Economy/EconomyMarketPanel.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **49**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `agriculture` | no |
| `autopsy` | no |
| `black_market` | no |
| `black_projects_archive` | no |
| `caravan` | no |
| `caravan_trade_network` | no |
| `chemical_dependency` | no |
| `chemical_recon` | no |
| `chemical_synthesis` | no |
| `collectible_discovery` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **18**.

| Stream |
|---|
| `agriculture_blight` |
| `agriculture_mutation` |
| `agriculture_pest` |
| `aquaponics_disease` |
| `black_market_bounty` |
| `black_market_debt_event` |
| `black_market_stock` |
| `combat` |
| `cupola_foundry` |
| `disease` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **375**
(CODEX_ONLY 279, GAMEPLAY_CONSUMED 65, OPTIONAL 8, UNRESOLVED 23).

| Catalog | Classification |
|---|---|
| `anomalous_expedition_encounters.json` | UNRESOLVED |
| `audio_cues.json` | UNRESOLVED |
| `audio_logs_expansion_05.json` | OPTIONAL |
| `autopsy_procedures.json` | GAMEPLAY_CONSUMED |
| `black_flotilla_items.json` | GAMEPLAY_CONSUMED |
| `bounty_board.json` | GAMEPLAY_CONSUMED |
| `bunker_graffiti_postings.json` | UNRESOLVED |
| `caravan_trade_routes.json` | GAMEPLAY_CONSUMED |
| `chemical_dependency_items.json` | GAMEPLAY_CONSUMED |
| `chemical_syntheses.json` | UNRESOLVED |

**Verdict:** 23 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **3**.

| Flag |
|---|
| `flag_become_warlord` |
| `flag_betrayed_faction` |
| `flag_chosen_faction_side` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY-WITH-NOTES (11/12) · **Class:** governance · **Coupling (incoming plans):** 0
**Surface:** save sections 49 (laddered 0) · RNG streams 18 · host files 25 · catalogs 22 · test regions 10 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-RELEASE-OPS-20
wave: —
status: PROPOSED — foreman claim required
packages: RO-20A, RO-20B, RO-20C, RO-20D, RO-20E, RO-20F
claim paths:
  - src/Audio/AudioConditionHostBridge.cs  # §19 candidate host surface
  - src/Audio/AudioCueCatalog.cs  # §19 candidate host surface
  - src/Audio/AudioEventBridge.cs  # §19 candidate host surface
  - src/Audio/AudioManager.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/agriculture_items.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/anomalous_expedition_encounters.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Accessibility/
  - godot --headless --path . -- --7-day-smoke-selftest
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
| verification | **no** |
| coupling | yes |
| binding | yes |

**Pre-claim actions:** author or confirm: verification.
