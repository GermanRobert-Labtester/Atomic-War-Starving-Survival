# PLAN-INTEGRATION-KIT-02 — Integration Scaffolding, Registry Unification & Orphan Gate

**Program:** ASHFALL Expansion & Integration Program (2026-09-21)
**Status:** PROPOSED — not a claim. Foreman claim required.
**Owner role on execution:** Integrator (shared seams) + Builder for the
generator/templates; the integrator owns `SubsystemManifest`,
`Main.CampaignServices.cs`, `Main.SaveOrchestrator.cs`, `docs/saves/*`,
`src/Main.PlayerSurfaces.cs`.
**Depends on:** nothing (this is the enabler for all other plans).
**Feeds:** PLAN-ORPHAN-SEAL-01 gate checks, PLAN-UNBLOCK-03 ledger truth,
PLAN-LAUNCH-FACE-06 acceptance.

---

## 1. Outcome

Make "Core delivered, host forgotten" mechanically impossible. Today the
repository has 226 `Setup*` host methods, 780 host session/store files, 217 CLI
selftest verbs, a 301-row decision register, and a 148-row claim ledger — but
the declarative registry that is supposed to bind them
(`SubsystemManifest`) contains **18 entries**. The 99-authority orphan cohort
(see PLAN-ORPHAN-SEAL-01) is the direct consequence: nothing forced a new Core
authority to declare its host owner, save path, and player surface.

Deliverables:

1. an authoritative **reachability generator** and CI `--check` gate;
2. a **complete `SubsystemManifest`** (or a generated equivalent) covering every
   runtime subsystem, with setup/save/day-owner/route declared;
3. an **`--integration-selftest`** that fails when any manifest entry lacks a
   live setup, session, save path, route, or selftest verb;
4. **host-session/save-store scaffolds** and an integration playbook so new
   features are wired by construction;
5. **ledger-truth checks** so "integrated and sealed" rows must cite a
   subsystem, a host path, a save path, and a passing gate.

**Non-goals:** no gameplay behavior change; no new save section; no panel
design; no test-suite expansion beyond the gates themselves.

---

## 2. Premise evidence (2026-09-21)

| Fact | Value | Source |
|---|---:|---|
| Core `.cs` files | 1,168 | `find Assets/Ashfall.Core -name '*.cs'` |
| Host `.cs` files | 844 | `find src -name '*.cs'` |
| Test `.cs` files | 1,378 | `find Ashfall.Core.Tests -name '*.cs'` |
| Data catalogs | 703 | `find Assets/StreamingAssets/Data -name '*.json'` |
| Host `Setup*` methods | 226 | `grep -rhoP 'private void Setup\w+' src/Main*.cs \| sort -u` |
| Host session/store files | 780 | `ls src/Host \| wc -l` |
| CLI selftest verbs | 217 | `grep -rhoP '"--[a-z0-9-]*selftest"' src/` |
| `SubsystemManifest` entries | 18 | `grep -cE '^\s+new\($' Assets/Ashfall.Core/Orchestration/SubsystemManifest.cs` |
| Manifest ids | journal, needs, inventory, weather, radiation, radio, expeditions, duty_roster, crafting, research, medical, factions, economy, greenhouse, shelter_defense, vehicle_garage, black_market, memorial | same |
| Host-unreachable authority files | 99 | reachability audit (PLAN-ORPHAN-SEAL-01 §2) |
| Fully dead authorities | 5 | same |
| Decision register rows | 301 | `grep -c '^| `DEC-' docs/governance/DECISION_REGISTER.md` |
| Non-terminal register rows | 2 (DEC-11, DEC-13) | audit command, §below |

Two structural gaps explain the orphan cohort:

1. **The manifest is a skeleton, not the registry.** `ComposeCampaign()`
   already calls `ExecuteSubsystemManifestBootstrap()` (CF-P28 sealed), but the
   bootstrap can only construct the 18 declared subsystems. A new authority
   that is not added to the manifest is silently optional.
2. **Nothing measures reachability.** `scripts/ci/generate-core-systems-catalog.py`
   checks a hand-maintained `CORE_SYSTEMS` list; it does not detect a Core
   system with zero host consumers. `generate-port-contract.py` counts seams
   (262, 180 HOST_REQUIRED, 0 DEFERRED) but only for seams it already knows.

---

## 3. Deliverable design

### 3.1 `generate-authority-reachability.py` (new, integrator-owned)
- **Inputs:** `Assets/Ashfall.Core/**`, `src/**`, `Ashfall.Core.Tests/**`.
- **Algorithm:** type extraction → token sets → edges → host-name roots → BFS
  closure (exactly the audit method in PLAN-ORPHAN-SEAL-01 §2).
- **Output:** `docs/architecture/AUTHORITY_REACHABILITY.md` — every authority
  type with `reachable | core_only_declared | orphan`, its file, its host
  session if any, its save section if any, its test files.
- **Modes:** `--write` regenerates; `--check` fails on any `orphan` row that is
  not allowlisted in `docs/ci/authority_reachability_allowlist.json`.
- **Allowlist rules:** every entry needs `reason`, `owner`, and `promotion
  condition`; same discipline as `KNOWN_DEBT.md`. The allowlist starts with
  the five fully-dead systems (until Wave 1 triage) and the intentionally
  CLI-only governance systems.
- **CI:** wired into `scripts/ci/agent-fast-verify.py` and the `--check` gate
  job in `.github/workflows/ci.yml`.

### 3.2 `SubsystemManifest` completion
Two acceptable designs; the integrator chooses one in the claim and records it:

- **Design A (declarative, recommended):** expand the manifest to every runtime
  subsystem (target ≈ 120–180 entries: all systems that own state, a day tick,
  or a player surface). `SetupAction` remains the host-bound delegate.
  `ExecuteSubsystemManifestBootstrap()` executes every entry exactly once and
  reports missing delegates as a typed bootstrap error in strict mode.
- **Design B (generated):** annotate each host `SetupX()` and each Core
  authority with `[SubsystemDescriptor(...)]`-style attributes and generate the
  manifest at build time; the static file is emitted and `--check`ed.

Acceptance for either: `--integration-selftest` can enumerate every subsystem
and prove it was constructed on the fresh path (`ComposeCampaign`) and on the
restore path (`RestoreAllSubsystemsFromDisk`), with identical subsystem sets
between the two (extends the existing `BootstrapLifecycleGate`).

### 3.3 `--integration-selftest`
One new CLI verb (host-owned, `src/Host/HostCli.Integration.cs`) that walks the
manifest and asserts per entry:

| Probe | Failure mode |
|---|---|
| setup delegate bound and invoked once | `MISSING_SETUP` |
| host session exists (or explicit CLI-only flag) | `MISSING_SESSION` |
| `SaveSectionKey` present in `SaveOrchestrator` registry when stateful | `UNBOUND_SAVE_SECTION` |
| primary panel route resolves in `Main.PlayerSurfaces` when declared | `UNRESOLVABLE_ROUTE` |
| a selftest verb is registered in the selftest manifest | `MISSING_SELFTEST_VERB` |
| no `System.Random`/`GetHashCode` in the authority file | `NONDETERMINISM` |
| Core file has no `Godot`/`UnityEngine` reference | `ENGINE_LEAK` |

Output: `[HOST_SELFTEST_SUMMARY] test=integration_selftest …` in the existing
format so CI step summaries already parse it.

### 3.4 Scaffolds and playbook
- `scripts/templates/` additions: `HostSession.scaffold.cs`, `SaveStore.scaffold.cs`,
  `MainPartial.scaffold.cs`, `SubsystemEntry.scaffold.md` (generated from the
  manifest entry format).
- `docs/architecture/INTEGRATION_PLAYBOOK.md`: the DoD from PLAN-ORPHAN-SEAL-01
  §4, the save-section decision tree, the day-owner ordering rule, the panel
  contract ("expose a command, never become the authority"), and the exact
  focused verification commands. Links a new builder to the gate they must
  pass.

### 3.5 Ledger-truth checks
- `scripts/ci/check-ledger-truth.py` (new) parses `INTEGRATION_PLANS.md` "DONE"
  rows and requires each to cite: at least one Core path, at least one host
  path (`src/**`), and either a test path or a gate output. Rows without a host
  path are flagged `HOST-PENDING` and cannot use the word "integrated".
- `scripts/ci/check-register-truth.py` (new) enforces the register invariant
  ("no unsigned item without condition") and flags evidence-count drift (the
  DEC-05 14/14→6/6 class of error) by re-running the cited test's count when
  cheap, or by requiring a date stamp in evidence.
- Both run in `agent-fast-verify.py`.

### 3.6 Save-section registry parity
- Regenerate `docs/saves/SAVE_STORE_CONTRACT_MATRIX.md` in the same PR as any
  new store (`generate-save-store-matrix.py --check` already exists; extend it
  to assert the store's codec is referenced by exactly one `SaveOrchestrator`
  registration).
- Add a section-count assertion to the existing comprehensive-save test
  (`ComprehensiveSaveStoreCorruptionAndMigrationTests` section-count gate) so a
  new section cannot land without the count update — this already caught drift
  once (173→180), keep it as the single source.

### 3.7 Selftest manifest
- `generate-selftest-manifest.py` already exists. Extend the generated manifest
  to include the integration selftest and to fail `--check` when a
  manifest-declared subsystem has no probe. `selftest-manifest-regen.yml`
  regenerates on merge.

---

## 4. Phases

| Phase | Package | Deliverable | Acceptance | Verify |
|---|---|---|---|---|
| K0 | `INTEGRATION-KIT-REACHABILITY` | generator + allowlist + docs row | `--check` runs green on current tree with the 99-orphan allowlist seeded | `python3 scripts/ci/generate-authority-reachability.py --check` |
| K1 | `INTEGRATION-KIT-SELFTEST` | `--integration-selftest` + 7 probes | verb registered, runs on the 18 manifest entries, reports them | `godot --headless --path . -- --integration-selftest` |
| K2 | `INTEGRATION-KIT-MANIFEST-A` | manifest expansion (first tranche: shelter, medical, economy, survivors — ≈ 60 entries) | fresh/restore subsystem sets equal; bootstrap strict mode green | `bash scripts/run_test.sh Ashfall.Core.Tests/Orchestration/`; `godot --headless --path . -- --composition-root-selftest` |
| K3 | `INTEGRATION-KIT-SCAFFOLD` | templates + playbook | new scaffold compiles; playbook linked from `AGENTS.md` source-of-truth table by the foreman | `dotnet build Ashfall.csproj`; docs link gate |
| K4 | `INTEGRATION-KIT-LEDGER` | ledger/register truth scripts | current ledger flagged for the known host-pending rows; register invariant green | `python3 scripts/ci/check-ledger-truth.py --check`; `python3 scripts/ci/check-register-truth.py --check` |
| K5 | `INTEGRATION-KIT-MANIFEST-B` | manifest completion (remaining subsystems) + CI wiring | 0 unallowlisted orphans; `agent-fast-verify` green | `python3 scripts/ci/agent-fast-verify.py` |

---

## 5. Risk register

| Risk | Mitigation |
|---|---|
| Manifest expansion is a large mechanical change | phase K2/K5 in two tranches; entries are additive; strict mode behind a flag until K5 |
| Reachability generator false positives (reflection, JSON-only types) | allowlist with reasons; generator reports edges so an allowlist edit is auditable |
| Rewriting `SubsystemManifest` collides with live builders | integrator-only path; claims must exclude it |
| Adding gates slows CI | gates are static Python + one headless selftest; no full xUnit run |
| Ledger checker blocks historical rows | checker flags only new/modified rows (diff-based) and reports historical ones as informational |

## 6. Rollback

Generators and gates are additive. If a gate blocks work incorrectly, the
allowlist is the escape hatch (with a written reason) and the gate can be
demoted to warn-only in `agent-fast-verify.py` without touching production code.

## 7. Verification summary

```bash
python3 scripts/ci/generate-authority-reachability.py --check
godot --headless --path . -- --integration-selftest
godot --headless --path . -- --composition-root-selftest
python3 scripts/ci/generate-save-store-matrix.py --check
python3 scripts/ci/generate-selftest-manifest.py --check
python3 scripts/ci/check-ledger-truth.py --check
python3 scripts/ci/check-register-truth.py --check
python3 scripts/ci/agent-fast-verify.py
```

## 8. Change-control notes

- This plan must land before any "program complete" claim; it is the
  enforcement half of PLAN-ORPHAN-SEAL-01.
- The manifest is a shared integrator path: no builder edits it directly.
- No new gameplay, no new save section, no panel work in this plan.

---

## 6. Expanded census (kit surface)

| Metric | Value |
|---|---:|
| Versioned generators | 50 |
| Pattern set (Appendix C) | 14 patterns |
| Scaffold generators (batches 1–9) | 9 |
| Templates shipped | session adapter, save store, day hook, panel route, CLI probe, fixture |

## 7. Expanded surface: kit contract

| Rule | Detail |
|---|---|
| One authority | each pattern extends an existing owner; no parallel store |
| Determinism | RNG via the registry; no wall-clock in Core |
| Failure | typed errors; no silent catch |
| Verification | focused fixture per pattern |

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Pattern reuse | a new package cites its pattern from Appendix C |
| Scaffold | generated skeleton compiles as a template (not committed) |
| Docs | `python3 scripts/ci/generate-docs-index.py --check` |

## 9. Rollout sequence

1. Adopt the kit in the first claim.
2. Fill the scaffold from a real source inventory.
3. Run the focused fixture; record results.

## 10. Acceptance matrix

| Deliverable class | Acceptance |
|---|---|
| Pattern | cited; owner extended |
| Scaffold | derived from real paths/names |
| Fixture | focused and green |

**Non-goals unchanged:** the kit does not implement features.

---

## 12. Cross-plan coupling

Domain method: plan-body artifact list.
Governed artifacts: 24. Other plans referencing them: **25**.

**Incoming plan edges (top 8):**

| Plan | Mentions |
|---|---:|
| `PLAN-ORPHAN-SEAL-01` | 11 |
| `PLAN-LAUNCH-FACE-06` | 7 |
| `PLAN-UNBLOCK-03` | 6 |
| `PLAN-CORE-ONLY-REGISTRY-11` | 4 |
| `PLAN-RELEASE-OPS-20` | 4 |
| `EVIDENCE` | 3 |
| `PLAN-SAVE-GOVERNANCE-12` | 3 |
| `PLAN-RUNTIME-PERF-16` | 3 |

**Governed artifacts (first 12):**

| Path |
|---|
| `AGENTS.md` |
| `HostSession.scaffold.cs` |
| `INTEGRATION_PLANS.md` |
| `KNOWN_DEBT.md` |
| `Main.CampaignServices.cs` |
| `Main.SaveOrchestrator.cs` |
| `MainPartial.scaffold.cs` |
| `SaveStore.scaffold.cs` |
| `SubsystemEntry.scaffold.md` |
| `agent-fast-verify.py` |
| `docs/architecture/AUTHORITY_REACHABILITY.md` |
| `docs/architecture/INTEGRATION_PLAYBOOK.md` |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidates |
|---|---|

**Reading:** incoming edges are coordination risk; candidates are a starting point for the touch map, not a decision.

---

## 13. Authority binding map

Symbols used: 23. Host files: **10** · Test files: **12** · Data files: **1**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 10 | `src/Host/ChemicalReconHostSession.cs`, `src/Host/CoreDemoSession.cs`, `src/Host/GeodeticSurveyHostSession.cs`, `src/Host/InventoryHostSession.cs`, `src/Host/KineticStorageHostSession.cs` |
| Tests (`Ashfall.Core.Tests/`) | 12 | `Ashfall.Core.Tests/DataRuleComplianceTests.cs`, `Ashfall.Core.Tests/NeedsRadiationSystemTests.cs`, `Ashfall.Core.Tests/NewSaveStoreTriadTests.cs`, `Ashfall.Core.Tests/Plan12AGenerationTests.cs`, `Ashfall.Core.Tests/PlayerCommand/ProseSuccessInferenceSourceGateTests.cs` |
| Data (`StreamingAssets/Data/`) | 1 | `Assets/StreamingAssets/Data/standing_gates.json` |

**Verdict:** all three layers attach — surface is bound end to end.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **11** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `campaign` |
| `campaign_day` |
| `chemical_dependency` |
| `chemical_recon` |
| `chemical_synthesis` |
| `dose_ledger` |
| `geodetic_survey` |
| `inventory` |
| `kinetic_storage` |
| `radiation` |
| `recon_telemetry` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **187** (matched by domain keyword
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

Events whose name shares a domain token: **15**.

| Event | First declaration |
|---|---|
| `OnContractForgiven` | `Assets/Ashfall.Core/LedgerDebtSystem.cs` |
| `OnContractPaid` | `Assets/Ashfall.Core/LedgerDebtSystem.cs` |
| `OnContractRenegotiated` | `Assets/Ashfall.Core/LedgerDebtSystem.cs` |
| `OnContractSigned` | `Assets/Ashfall.Core/LedgerDebtSystem.cs` |
| `OnEntryAdded` | `Assets/Ashfall.Core/Journal/JournalSystem.cs` |
| `OnEntryRead` | `Assets/Ashfall.Core/Verdict/MachineLogSystem.cs` |
| `OnFactionStandingChanged` | `Assets/Ashfall.Core/YearOfAsh/FactionWarSystem.cs` |
| `OnInventoryChanged` | `Assets/Ashfall.Core/Inventory/Inventory.cs` |
| `OnLedgerCalibrated` | `Assets/Ashfall.Core/DoseLedgerSystem.cs` |
| `OnLedgerTampered` | `Assets/Ashfall.Core/LedgerDebtSystem.cs` |
| `OnMoralChronicleEntry` | `Assets/Ashfall.Core/Maritime/PsychologicalContaminationSystem.cs` |
| `OnRadiationDoseResetRequested` | `Assets/Ashfall.Core/Radiation/RadiationPhaseProgression.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/black_market_inventory.json` |
| `Assets/StreamingAssets/Data/campaign_epilogues.json` |
| `Assets/StreamingAssets/Data/chemical_dependency_items.json` |
| `Assets/StreamingAssets/Data/chemical_syntheses.json` |
| `Assets/StreamingAssets/Data/chemical_weapons.json` |
| `Assets/StreamingAssets/Data/dose_items.json` |
| `Assets/StreamingAssets/Data/dose_locations.json` |
| `Assets/StreamingAssets/Data/dose_quests.json` |
| `Assets/StreamingAssets/Data/dose_registers.json` |
| `Assets/StreamingAssets/Data/faction_combat_thresholds.json` |
| `Assets/StreamingAssets/Data/faction_intelligence.json` |
| `Assets/StreamingAssets/Data/faction_lore.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **12** (121 files, 809 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Audio` | 5 | 28 |
| `Balance` | 1 | 5 |
| `Campaign` | 32 | 187 |
| `Combat` | 10 | 84 |
| `Foundry` | 8 | 73 |
| `Integration` | 16 | 74 |
| `Inventory` | 15 | 114 |
| `Needs` | 4 | 21 |
| `Progression` | 11 | 83 |
| `Quests` | 4 | 25 |

**Verdict:** 809 cases sit under matching regions — run those first (`Audio`, `Balance`, `Campaign`, `Combat`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **541**
(37 of them panels/HUD).

| Host file |
|---|
| `src/Disease/DiseaseHostSession.cs` |
| `src/Dose/DoseRegisterSurface.cs` |
| `src/Foundry/SilentFoundryHostSession.cs` |
| `src/Host/AgricultureHostSession.cs` |
| `src/Host/AgricultureSaveStore.cs` |
| `src/Host/AirlockSecurityHostSession.cs` |
| `src/Host/AirlockSecuritySaveStore.cs` |
| `src/Host/AmphibiousDraisineHostSession.cs` |
| `src/Host/AmphibiousDraisineSaveStore.cs` |
| `src/Host/AmputationSaveStore.cs` |
| `src/Host/AnomalyHazardSaveStore.cs` |
| `src/Host/ApprenticeshipHostSession.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **35**, of which versioned-ladder sections:
**1**.

| Section key | Laddered |
|---|---|
| `agriculture` | no |
| `airlock_security` | no |
| `amphibious_draisine` | no |
| `amputation` | no |
| `anomaly_hazard` | no |
| `apprenticeship` | no |
| `black_market` | no |
| `black_projects_archive` | no |
| `campaign` | no |
| `campaign_day` | no |

**Verdict:** matched sections include versioned ladders — a schema change here must extend the existing ladder, not fork it.

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
| `black_market_bounty` |
| `black_market_debt_event` |
| `black_market_stock` |
| `combat` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **108**
(CODEX_ONLY 25, GAMEPLAY_CONSUMED 62, OPTIONAL 2, UNRESOLVED 19).

| Catalog | Classification |
|---|---|
| `black_flotilla_items.json` | GAMEPLAY_CONSUMED |
| `chemical_dependency_items.json` | GAMEPLAY_CONSUMED |
| `chemical_syntheses.json` | UNRESOLVED |
| `chemical_weapons.json` | GAMEPLAY_CONSUMED |
| `combat_catalog.json` | GAMEPLAY_CONSUMED |
| `crossing_items.json` | GAMEPLAY_CONSUMED |
| `crossing_locations.json` | GAMEPLAY_CONSUMED |
| `crossing_quests.json` | GAMEPLAY_CONSUMED |
| `cupola_foundry_catalog.json` | UNRESOLVED |
| `deep_lore_locations.json` | GAMEPLAY_CONSUMED |

**Verdict:** 19 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **3**.

| Flag |
|---|
| `flag_betrayed_faction` |
| `flag_chosen_faction_side` |
| `flag_honored_debt` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY-WITH-NOTES (11/12) · **Class:** governance · **Coupling (incoming plans):** 25
**Surface:** save sections 35 (laddered 1) · RNG streams 14 · host files 25 · catalogs 22 · test regions 10 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-INTEGRATION-KIT-02
wave: —
status: PROPOSED — foreman claim required
packages: author package list at claim time
claim paths:
  - src/Disease/DiseaseHostSession.cs  # §19 candidate host surface
  - src/Dose/DoseRegisterSurface.cs  # §19 candidate host surface
  - src/Foundry/SilentFoundryHostSession.cs  # §19 candidate host surface
  - src/Host/AgricultureHostSession.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/black_market_inventory.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/campaign_epilogues.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Audio/
  - godot --headless --path . -- --7-day-smoke-selftest
dependencies:
  - coordinate: 25 other plan(s) name these artifacts (§12)
  - wave seed — claim before dependent plans
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
| packages | **no** |
| acceptance | yes |
| risks | yes |
| verification | yes |
| coupling | yes |
| binding | yes |

**Pre-claim actions:** author or confirm: packages.
