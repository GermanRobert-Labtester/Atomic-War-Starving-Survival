# PLAN-SELFTEST-TRUTH-23 — CLI Selftest Truthfulness & Probe Coverage

**Wave:** 3 (2026-09-21) · **Kind:** GAP SEALING
**Status:** PROPOSED — not a claim. Foreman claim required.
**Depends on:** PLAN-INTEGRATION-KIT-02 (`--integration-selftest`), PLAN-RELEASE-OPS-20
(gate reliability).
**Expanded appendix:** [`PLAN-SELFTEST-TRUTH-23_APPENDIX-A_VERB_CENSUS.md`](PLAN-SELFTEST-TRUTH-23_APPENDIX-A_VERB_CENSUS.md)
— the CLI verb census: **277 verbs** cross-checked between
`HostCliRegistry` and host code, with per-verb implementation files and a
verdict (`BOTH` · `HOST_ONLY` · `REGISTRY_ONLY`). ST-23A/23D packages work
from this census.
**Non-goals:** no new test framework, no re-running the full suite, no CLI
redesign.

---

## 1. Outcome

The headless selftests are this project's integration evidence: 217 verbs, a
1,413-line `HostCliRegistry`, 29 CLI partials, and a gate that keeps the
manifest in sync. But a selftest is only evidence if it **can fail**. The Plan 48
audit already found the export workflow's verification steps could not fail;
the same class of problem can hide in any probe that prints a summary without a
meaningful assertion.

Deliverables:

1. a **verb census**: for every selftest verb — registered, implemented,
   assertion count, failure path, runtime, side effects;
2. **truthfulness probes** for the highest-value verbs: inject a known defect in
   a scratch fixture and prove the verb fails non-zero;
3. **coverage**: every subsystem in the manifest has at least one probe (ties to
   the kit);
4. **dead verb retirement** with alias handling for documented names;
5. **CI wiring**: summaries parsed, exit codes honored, timeouts bounded.

---

## 2. Evidence

| Fact | Value | Source |
|---|---:|---|
| Selftest verbs registered | 202 strings in `HostCliRegistry.cs` | grep |
| Distinct verbs across `src` | 217 | `grep -rhoP '"--[a-z0-9-]*selftest"'` |
| CLI partial files | 29 | `ls src/Host/HostCli*.cs` |
| Summary format | `[HOST_SELFTEST_SUMMARY]`, `[HOST_SELFTEST_JSON]` | `HostCli.DynamicWorld.cs` |
| Sync gate | "Selftest manifest in sync with HostCliRegistry" | `CI_GATE_MANIFEST.json` (57 gates) |
| Export precedent | verification steps could not fail | Plan 48 audit |
| Manifest subsystems | 18 declared (target 120–180) | `SubsystemManifest.cs` |
| Orphan authorities with no probe | 99-host-unreachable cohort | PLAN-ORPHAN-SEAL-01 |

---

## 3. Packages

### ST-23A — Verb census
- Generate `docs/ci/SELFTEST_CENSUS.md`: `verb | implementation file | line |
  assertions | failure exits non-zero? | runtime class | side effects
  (writes files/saves?) | summary emitted?`.
- Implementation: parse `HostCliRegistry` + dispatch switch + each `Run*`
  method's assertion calls (`Assert`/`if (…) failures++`/`EmitSummary`).
- **Acceptance:** every registered verb has a row; verbs with 0 assertions are
  flagged `SUSPECT`; verbs that write outside `user://`/scratch are flagged.
- **Verify:** `python3 scripts/ci/generate-selftest-census.py --check`.

### ST-23B — Truthfulness probes (sampled)
- For the 10 highest-value verbs (data-integrity, bridge, save-load, panel
  lifecycle, audio, economy, campaign smoke, difficulty, content-utilization,
  integration): create a scratch-fixture fault (a deliberately invalid catalog
  row, a broken binding, a corrupted save) and assert the verb fails with a
  non-zero exit code.
- **Acceptance:** each probe proves the verb can fail; the probe lives in
  `scripts/ci/` and runs in the nightly tier (not the fast tier).
- **Verify:** the probe runner; `[HOST_SELFTEST_SUMMARY] status=FAIL` for the
  injected fault.

### ST-23C — Probe coverage over the manifest
- Extend the kit: every manifest subsystem declares a probe (or an explicit
  `no_probe_reason` for passive data-only subsystems).
- **Acceptance:** coverage report lists each subsystem and its probe; a new
  subsystem without one fails the kit gate.
- **Verify:** `godot --headless --path . -- --integration-selftest`.

### ST-23D — Dead verb retirement
- Verbs that are registered but unreachable from the dispatcher, or that only
  print usage, are retired; documented names get `--alias` rows so scripts keep
  working for one release.
- **Acceptance:** registry ↔ dispatcher ↔ manifest all agree; no orphan string;
  aliases deprecated with a notice.
- **Verify:** the selftest-manifest gate + CLI help catalog drift gate.

### ST-23E — CI wiring
- Ensure each selftest job: parses `[HOST_SELFTEST_JSON]`, fails the step on
  `status=FAIL`, bounds runtime with a timeout, and uploads the log on failure.
- **Acceptance:** no job can pass while a probe fails; timeouts documented.
- **Verify:** a dry run with an injected failure.

---

## 4. Risk register

| Risk | Mitigation |
|---|---|
| Fault injection harms the working tree | probes run in a scratch clone/dir; no writes to `Assets/` |
| Census parsing misses dynamic registration | flagged `UNPARSED` with a reason; coverage from the registry is authoritative |
| Retiring a used verb breaks scripts | alias window + docs update in the same package |
| Runtime growth from probes | nightly tier only; bounded timeout |

## 5. Verification

```bash
python3 scripts/ci/generate-selftest-census.py --check
python3 scripts/ci/generate-selftest-manifest.py --check
godot --headless --path . -- --integration-selftest
godot --headless --path . -- --data-integrity-selftest
python3 scripts/ci/agent-fast-verify.py
```

---

## 6. Expanded census (bespoke: selftest surface)

This plan audits selftest truthfulness, so the census counts the flags, the
test regions they should map to, and the CI gates.

| Metric | Value |
|---|---:|
| Distinct `--*-selftest` flags | 218 |
| Test regions | 109 |
| CI gates | 57 |
| Flags per region (rough) | 2.0 |

**Sample flags:** `--accessibility-selftest`, `--advanced-industrial-recon-selftest`, `--agriculture-selftest`, `--all-expansions-selftest`, `--amphibious-draisine-selftest`, `--aquaponics-selftest`, `--arbitration-selftest`, `--asset-registry-selftest`, `--atmosphere-selftest`, `--audio-selftest`, `--black-flotilla-selftest`, `--bridge-selftest`, `--brine-selftest`, `--campaign-fuzz-selftest`, `--campaign-journey-selftest`, `--caravan-selftest`, `--carbon-composite-selftest`, `--cartography-selftest`, `--census-selftest`, `--checksum-sweep-selftest`, `--chemical-dependency-save-selftest`, `--chemical-recon-selftest`, `--cluster-selftest`, `--cohort-lifecycle-selftest`

## 7. Expanded surface: truthfulness contract

| Rule | Detail |
|---|---|
| Real assertions | a selftest that cannot fail is a defect; each must assert at least one observable |
| Named region | every flag maps to a region or states why it is host-only |
| Exit semantics | nonzero on failure; no silent pass on exception |
| Headless | flags marked headless-compatible run without a window |

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Non-vacuous | mutate one covered behavior; the flag must fail |
| Parity | flags ↔ regions table complete |
| Exit codes | nonzero on a forced failure |
| Docs | `python3 scripts/ci/generate-docs-index.py --check` |

## 9. Rollout sequence

1. Census (this section).
2. Flags↔regions parity table.
3. Non-vacuous mutation fixtures (sample set).
4. Exit-code and headless checks.
5. Regression: parity + sample mutations.

## 10. Acceptance matrix

| Deliverable class | Acceptance |
|---|---|
| Flag | asserts an observable; fails when it should |
| Region | exists and runs under the focused script |
| Exit | nonzero on failure; no swallowed exceptions |
| Headless | classification verified by a run |

**Non-goals unchanged:** this expansion adds census and verification detail; it does not rewrite the suite.

---

## 12. Cross-plan coupling

Domain method: plan-body artifact list.
Governed artifacts: 6. Other plans referencing them: **17**.

**Incoming plan edges (top 8):**

| Plan | Mentions |
|---|---:|
| `PLAN-RUNTIME-PERF-16` | 2 |
| `PLAN-MORTUARY-MEMORIAL-TRUTH-123` | 2 |
| `PLAN-ORPHAN-SEAL-01` | 1 |
| `PLAN-INTEGRATION-KIT-02` | 1 |
| `PLAN-LAUNCH-FACE-06` | 1 |
| `EVIDENCE` | 1 |
| `PLAN-SAVE-GOVERNANCE-12` | 1 |
| `PLAN-DETERMINISM-REPLAY-13` | 1 |

**Governed artifacts (first 12):**

| Artifact |
|---|
| `CI_GATE_MANIFEST.json` |
| `HostCli.DynamicWorld.cs` |
| `HostCliRegistry.cs` |
| `PLAN-SELFTEST-TRUTH-23_APPENDIX-A_VERB_CENSUS.md` |
| `SubsystemManifest.cs` |
| `docs/ci/SELFTEST_CENSUS.md` |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidates |
|---|---|
| `ST-23A` | `PLAN-SELFTEST-TRUTH-23_APPENDIX-A_VERB_CENSUS.md`, `docs/ci/SELFTEST_CENSUS.md` |
| `ST-23B` | no name match — resolve at claim time |
| `ST-23C` | `CI_GATE_MANIFEST.json`, `SubsystemManifest.cs` |
| `ST-23D` | no name match — resolve at claim time |
| `ST-23E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk; candidates are a starting point for the touch map, not a decision.

---

## 13. Authority binding map

Symbols used: 6. Host files: **2** · Test files: **6** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 2 | `src/Host/HostCli.SelfTestManifest.cs`, `src/Main.Lifecycle.cs` |
| Tests (`Ashfall.Core.Tests/`) | 6 | `Ashfall.Core.Tests/Orchestration/SubsystemManifestTests.cs`, `Ashfall.Core.Tests/Tooling/ArchitectureTestMapGateTests.cs`, `Ashfall.Core.Tests/Tooling/BootstrapPathParityGateTests.cs`, `Ashfall.Core.Tests/Tooling/CiGateManifestDriftTests.cs`, `Ashfall.Core.Tests/Tooling/SelfTestManifestGateTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **2** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `dynamic_quests` |
| `nuclear_core_lifecycle` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **184** (matched by domain keyword
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
| `--asset-registry-selftest` |
| `--atmosphere-selftest` |
| `--audio-selftest` |
| `--black-flotilla-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **1**.

| Event | First declaration |
|---|---|
| `OnCensusUpdated` | `Assets/Ashfall.Core/CensusClaimSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **5**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/dynamic_quest_templates.json` |
| `Assets/StreamingAssets/Data/dynamic_questlines.json` |
| `Assets/StreamingAssets/Data/mod_manifest_schema.json` |
| `Assets/StreamingAssets/Data/narrative/blast_gate_mechanical_audits.json` |
| `Assets/StreamingAssets/Data/narrative_discovery_manifest.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **9** (103 files, 839 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Audio` | 5 | 28 |
| `Collectibles` | 11 | 72 |
| `Economy` | 41 | 329 |
| `Holdfast` | 1 | 13 |
| `Integration` | 16 | 74 |
| `Legacy` | 1 | 5 |
| `Lifecycle` | 1 | 5 |
| `Narrative` | 26 | 293 |
| `NarrativeConsequence` | 1 | 20 |

**Verdict:** 839 cases sit under matching regions — run those first (`Audio`, `Collectibles`, `Economy`, `Holdfast`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **214**
(8 of them panels/HUD).

| Host file |
|---|
| `src/Audio/AudioSelfTest.cs` |
| `src/Host/CatalogPath.cs` |
| `src/Host/ChemicalDependencySaveSelfTest.cs` |
| `src/Host/CompletionHistorySelfTest.cs` |
| `src/Host/ContentUtilizationSelfTest.cs` |
| `src/Host/ContrabandStashSelfTest.cs` |
| `src/Host/DynamicQuestSaveStore.cs` |
| `src/Host/HiddenAgendaSelfTest.cs` |
| `src/Host/HoldfastTradeSaveStoreSelfTest.cs` |
| `src/Host/HostCli.AdvancedIndustrialRecon.cs` |
| `src/Host/HostCli.Cartography.cs` |
| `src/Host/HostCli.Collectibles.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **16**, of which versioned-ladder sections:
**1**.

| Section key | Laddered |
|---|---|
| `caravan_trade_network` | no |
| `chemical_dependency` | no |
| `chemical_recon` | no |
| `chemical_synthesis` | no |
| `collectible_discovery` | no |
| `contraband_stash` | no |
| `dynamic_quests` | no |
| `expansion_quest` | no |
| `hidden_agenda` | no |
| `holdfast` | yes |

**Verdict:** matched sections include versioned ladders — a schema change here must extend the existing ladder, not fork it.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **4**.

| Stream |
|---|
| `advanced_mfg_ebpvd_coating` |
| `metrology_calibration_drift` |
| `mineral_chemical` |
| `narrative` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **311**
(CODEX_ONLY 279, GAMEPLAY_CONSUMED 18, OPTIONAL 7, UNRESOLVED 7).

| Catalog | Classification |
|---|---|
| `audio_cues.json` | UNRESOLVED |
| `audio_logs_expansion_05.json` | OPTIONAL |
| `caravan_trade_routes.json` | GAMEPLAY_CONSUMED |
| `chemical_dependency_items.json` | GAMEPLAY_CONSUMED |
| `chemical_syntheses.json` | UNRESOLVED |
| `chemical_weapons.json` | GAMEPLAY_CONSUMED |
| `collectibles.json` | UNRESOLVED |
| `dynamic_questlines.json` | GAMEPLAY_CONSUMED |
| `holdfast_factions.json` | GAMEPLAY_CONSUMED |
| `holdfast_flavor.json` | GAMEPLAY_CONSUMED |

**Verdict:** 7 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **0**.

| Flag |
|---|
| — | no persistent flag shares a token with this domain |

**Verdict:** no persistent flag matches — the domain's narrative consequences are carried by other state (or the flag set is a candidate for cleanup).

---

## 24. Claim readiness

**Readiness:** READY-WITH-NOTES (11/12) · **Class:** hub · **Coupling (incoming plans):** 17
**Surface:** save sections 16 (laddered 1) · RNG streams 4 · host files 16 · catalogs 15 · test regions 9 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-SELFTEST-TRUTH-23
wave: —
status: PROPOSED — foreman claim required
packages: ST-23A, ST-23B, ST-23C, ST-23D, ST-23E
claim paths:
  - src/Audio/AudioSelfTest.cs  # §19 candidate host surface
  - src/Host/CatalogPath.cs  # §19 candidate host surface
  - src/Host/ChemicalDependencySaveSelfTest.cs  # §19 candidate host surface
  - src/Host/CompletionHistorySelfTest.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/dynamic_quest_templates.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/dynamic_questlines.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Audio/
  - godot --headless --path . -- --7-day-smoke-selftest
dependencies:
  - coordinate: 17 other plan(s) name these artifacts (§12)
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
