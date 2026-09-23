# PLAN-HOTFIX-DRILL-99 — Rehearsed Hotfix, Rollback & Save-Compatibility Drills

**Wave 8 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-RELEASE-OPS-20, PLAN-SAVE-MIGRATION-CORRIDOR-87, PLAN-RELEASE-CRAFT (C2[21]).
**Implementation scaffold:** [`PLAN-HOTFIX-DRILL-99_APPENDIX-A_SCAFFOLD.md`](PLAN-HOTFIX-DRILL-99_APPENDIX-A_SCAFFOLD.md) — paired with `PLAN-RELEASE-OPS-20` as scaffolding authority: source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no real tag, no real release, no process rewrite; the release
craft plan (C2[21]/Plan 48) owns the process. This plan rehearses it.

## 1. Outcome
Release tooling already exists: `scripts/release/prepare-release.sh`,
`scripts/release/hotfix.sh`, `scripts/release/generate_changelog.py`. What has
never been proven is the **hotfix path under pressure**: does a rehearsed
hotfix build roll back cleanly, does an old save load under the hotfix build,
does a new save degrade per policy under the previous build, and how long does
each step take? A drill answers those with recorded numbers, in a throwaway
environment, without touching a real release.

| Deliverable | Detail |
|---|---|
| Drill script | dry-run rehearsal in a scratch worktree/branch: version bump, changelog, build, artifact check — no push, no tag |
| Rollback proof | previous build restored; artifact and save-compat state verified after rollback |
| Save matrix | old-save→hotfix build and new-save→previous build, with the expected behavior from Plan 87's policy |
| Timing record | wall-clock per step (bounded); stored beside the drill report |
| Findings route | blockers filed against the owning script/plan; the drill does not patch the release process itself |

## 2. Evidence
- `scripts/release/`: `prepare-release.sh`, `hotfix.sh`, `generate_changelog.py` exist.
- `scripts/run_test.sh` and CI gates (57 per Plan 20 Appendix A) define the verifiable gate set the drill must not skip.
- Plan 87 defines section version ladders; the save matrix here consumes that policy rather than redefining it.
- Release craft C2[21]/Plan 48 is available-unclaimed (AGENTS queue) — the drill must not pre-empt its claim.

## 3. Packages
- **HFD-99A** drill script (scratch environment; hard guard: refuses to run on the default branch or with a clean-release intent).
- **HFD-99B** rollback rehearsal + verification checklist.
- **HFD-99C** save-compat matrix run (uses Plan 87 fixtures).
- **HFD-99D** timing capture + report template.
- **HFD-99E** findings log + handoff to C2[21] owner.

## 4. Acceptance & verification
- Drill completes in a scratch environment with zero pushes/tags (verified by the script's own guard).
- Rollback restores the previous artifact; save matrix outcomes match Plan 87 policy or are filed as findings.
- Timing report produced with per-step numbers.
- No production test-suite expansion; gates run remain the existing CI set.

## 5. Risks
Drill mistaken for release authority → the script is dry-run by construction
and named `drill`; findings go to the process owner.
Environment drift → the drill records tool versions in its report.

---

## 6. Expanded census (bespoke: release tooling)

This plan rehearses the release path, so the census covers the release scripts.

| Script | Lines |
|---|---:|
| `scripts/ci/release-gate.sh` | 102 |
| `scripts/release/generate_changelog.py` | 171 |
| `scripts/release/hotfix.sh` | 130 |
| `scripts/release/prepare-release.sh` | 292 |

## 7. Expanded surface: drill contract

| Rule | Detail |
|---|---|
| Dry-run | the drill refuses default-branch execution and never pushes/tags |
| Rollback | previous artifact restored and verified |
| Save-compat | old save → hotfix build; new save → previous build (Plan 87 policy) |
| Timing | per-step wall time recorded beside the report |

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Guard | drill aborts on the default branch (fixture) |
| Rollback | restored artifact matches the prior build |
| Save matrix | outcomes match Plan 87's policy or are filed as findings |
| Docs | `python3 scripts/ci/generate-docs-index.py --check` |

## 9. Rollout sequence

1. Tooling census (this section).
2. Scratch-environment drill with guard assertions.
3. Rollback rehearsal.
4. Save-compat matrix.
5. Findings hand-off to the release-craft owner.

## 10. Acceptance matrix

| Deliverable class | Acceptance |
|---|---|
| Guard | no push/tag possible by construction |
| Rollback | prior artifact restored and smoke-checked |
| Save matrix | documented outcomes per case |
| Timing | recorded; no unbounded step |

**Non-goals unchanged:** this expansion adds census and verification detail; the release process itself is owned elsewhere.

---

## 12. Cross-plan coupling

Domain method: plan-body artifact list.
Governed artifacts: 9. Other plans referencing them: **256**.

**Incoming plan edges (top 8):**

| Plan | Mentions |
|---|---:|
| `PLAN-UNBLOCK-03` | 2 |
| `PLAN-LAUNCH-FACE-06` | 2 |
| `PLAN-ORPHAN-SEAL-01` | 1 |
| `PLAN-INTEGRATION-KIT-02` | 1 |
| `PLAN-VERTICAL-CULTURE-04` | 1 |
| `PLAN-VERTICAL-BODY-INDUSTRY-05` | 1 |
| `PLAN-SAVE-GOVERNANCE-12` | 1 |
| `PLAN-DETERMINISM-REPLAY-13` | 1 |

**Governed artifacts (first 12):**

| Artifact |
|---|
| `PLAN-HOTFIX-DRILL-99_APPENDIX-A_SCAFFOLD.md` |
| `generate_changelog.py` |
| `hotfix.sh` |
| `prepare-release.sh` |
| `scripts/ci/release-gate.sh` |
| `scripts/release/generate_changelog.py` |
| `scripts/release/hotfix.sh` |
| `scripts/release/prepare-release.sh` |
| `scripts/run_test.sh` |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidates |
|---|---|
| `HFD-99A` | `PLAN-HOTFIX-DRILL-99_APPENDIX-A_SCAFFOLD.md`, `prepare-release.sh`, `scripts/ci/release-gate.sh` |
| `HFD-99B` | no name match — resolve at claim time |
| `HFD-99C` | no name match — resolve at claim time |
| `HFD-99D` | no name match — resolve at claim time |
| `HFD-99E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk; candidates are a starting point for the touch map, not a decision.

---

## 13. Authority binding map

Symbols used: 6. Host files: **0** · Test files: **2** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 0 | — |
| Tests (`Ashfall.Core.Tests/`) | 2 | `Ashfall.Core.Tests/Endgame/CrossRunProfileStoreTests.cs`, `Ashfall.Core.Tests/Legacy/Plan140GenerationalLegacyIntegrationTests.cs` |
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

Matching flags in `HostCliRegistry.cs`: **2** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--save-store-checksum-selftest` |
| `--save-store-checksums-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **1**.

| Event | First declaration |
|---|---|
| `OnDrillFailure` | `Assets/Ashfall.Core/Foundry/SaltMineExtractionSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **6**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/death_legacy_templates.json` |
| `Assets/StreamingAssets/Data/legacy_traits.json` |
| `Assets/StreamingAssets/Data/mine_flail_catalog.json` |
| `Assets/StreamingAssets/Data/narrative/blast_gate_mechanical_audits.json` |
| `Assets/StreamingAssets/Data/narrative/salt_mine_inscriptions.json` |
| `Assets/StreamingAssets/Data/store_capability_claims.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **5** (24 files, 120 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Archaeology` | 1 | 4 |
| `Audio` | 5 | 28 |
| `Integration` | 16 | 74 |
| `Legacy` | 1 | 5 |
| `Release` | 1 | 9 |

**Verdict:** 120 cases sit under matching regions — run those first (`Archaeology`, `Audio`, `Integration`, `Legacy`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **196**
(5 of them panels/HUD).

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
| `src/Host/AutopsySaveStore.cs` |
| `src/Host/AviationSaveStore.cs` |
| `src/Host/BallisticShieldSaveStore.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **17**, of which versioned-ladder sections:
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
| `route_engineering_mine_flail` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **36**
(CODEX_ONLY 29, GAMEPLAY_CONSUMED 3, OPTIONAL 1, UNRESOLVED 3).

| Catalog | Classification |
|---|---|
| `armored_crawler_modules.json` | UNRESOLVED |
| `autopsy_procedures.json` | GAMEPLAY_CONSUMED |
| `ballistic_shield_catalog.json` | UNRESOLVED |
| `development_traits.json` | GAMEPLAY_CONSUMED |
| `excavation_hazard_mitigation.json` | GAMEPLAY_CONSUMED |
| `ledger_debt_templates.json` | UNRESOLVED |
| `narrative/apiculture_red_light_audits.json` | CODEX_ONLY |
| `narrative/architect_vault_audits.json` | CODEX_ONLY |
| `narrative/armored_cockroach_hive_logs.json` | CODEX_ONLY |
| `narrative/armored_locomotive_manifests.json` | CODEX_ONLY |

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

**Readiness:** READY (12/12) · **Class:** governance · **Coupling (incoming plans):** 256
**Surface:** save sections 17 (laddered 0) · RNG streams 8 · host files 20 · catalogs 16 · test regions 5 · flags 2

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-HOTFIX-DRILL-99
wave: 8
status: PROPOSED — foreman claim required
packages: HFD-99A, HFD-99B, HFD-99C, HFD-99D, HFD-99E
claim paths:
  - src/Host/AgricultureSaveStore.cs  # §19 candidate host surface
  - src/Host/AirlockSecuritySaveStore.cs  # §19 candidate host surface
  - src/Host/AmphibiousDraisineSaveStore.cs  # §19 candidate host surface
  - src/Host/AmputationSaveStore.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/death_legacy_templates.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/legacy_traits.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Archaeology/
  - godot --headless --path . -- --save-store-checksum-selftest
dependencies:
  - coordinate: 256 other plan(s) name these artifacts (§12)
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
