# PLAN-BOOTSTRAP-GATE-TRUTH-147 — Manifest Coverage, Lifecycle Phases & Ledger-Truth Gate

**Wave 12 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-HOST-COMPOSITION-GOVERNANCE-71, PLAN-HOST-CLI-CONTRACT-86, PLAN-PROGRAMME-CLOSEOUT-100.
**Implementation scaffold:** [`PLAN-BOOTSTRAP-GATE-TRUTH-147_APPENDIX-A_SCAFFOLD.md`](PLAN-BOOTSTRAP-GATE-TRUTH-147_APPENDIX-A_SCAFFOLD.md) — paired with `PLAN-PROGRAMME-CLOSEOUT-100` as scaffolding authority: source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no new composition framework, no host partial rename sweep (Plan 71
owns naming), no runtime behavior change.

## 1. Outcome
`Orchestration/` holds three gate types — `BootstrapLifecycleGate`,
`LedgerTruthIntegrityGate`, `SubsystemManifest` — and the known gap is stark:
the manifest holds **18 entries** while the host exposes **226 `Setup*`**
methods, so most composition is unmanifested. Without coverage the gates cannot
detect an unregistered or misphased subsystem, and ledger-truth claims are
unverifiable.

| Deliverable | Detail |
|---|---|
| Manifest coverage report | `Setup*` methods (from Plan 71's inventory) vs manifest entries: registered, unregistered, extra |
| Lifecycle phases | every entry's `LifecyclePhase` matches how the host actually invokes it; mismatches reported |
| Generated manifest option | the report is generated with `--check` so new setups must declare themselves |
| Ledger-truth gate | `LedgerTruthIntegrityGate` semantics documented: which ledgers must reconcile at boot, and what a failure does (typed, visible) |
| Bootstrap gate | `BootstrapLifecycleGate` preconditions documented; a partial bootstrap fails loudly, never half-initializes silently |

## 2. Evidence
- `Assets/Ashfall.Core/Orchestration/`: `SubsystemManifest.cs`, `BootstrapLifecycleGate.cs`, `LedgerTruthIntegrityGate.cs` (verified).
- Plan 71 Appendix A: Main partial inventory with Setup/Save methods — the report's left-hand side.
- Plan 86's manifest verb can expose the coverage table to CI.
- AGENTS.md: manifest 18 entries vs 226 `Setup*` known disparity.

## 3. Packages
- **BGT-147A** coverage report generator (manifest ↔ setups) with `--check`.
- **BGT-147B** phase-truth table: entry phase vs actual invocation order.
- **BGT-147C** ledger-gate semantics doc + a failure fixture per ledger class.
- **BGT-147D** bootstrap precondition doc + partial-bootstrap failure test.
- **BGT-147E** CI integration note for Plan 86's manifest verb.

## 4. Acceptance & verification
- Coverage report lists every `Setup*` as registered or not; count reconciliation is exact.
- A phase mismatch fixture fails with both phases named.
- A deliberately failing ledger fixture aborts boot with a typed error (no partial state).
- `bash scripts/run_test.sh Ashfall.Core.Tests/Orchestration/` (create if absent).

## 5. Risks
Manifest as ceremony → generated `--check` makes coverage a mechanical property.
Gate strictness breaking boot → new checks land with the fixture that proves the failure path is clean.

---

## 6. Expanded census (3 files · 579 lines)

Scope: `Assets/Ashfall.Core/Orchestration/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Support 3

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `BootstrapLifecycleGate.cs` | 128 | Support | **yes** | 0 | 0 | 0 |
| `LedgerTruthIntegrityGate.cs` | 137 | Support | **yes** | 0 | 0 | 0 |
| `SubsystemManifest.cs` | 314 | Support | **yes** | 0 | 0 | 0 |

**Totals:** 0 banned refs · 0 empty catches · 0 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `weather_route_gates.json` | object[2 keys] |
| `narrative_discovery_manifest.json` | array[243] |
| `standing_gates.json` | object[2 keys] |
| `mod_manifest_schema.json` | object[9 keys] |
| `armored_locomotive_manifests.json` | array[7] |
| `blast_gate_mechanical_audits.json` | array[8] |

**State surfaces:** none.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Orchestration/` |
| Test references | 4 name references across the test tree |
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

Domain method: plan-body `.cs` enumeration.
Domain files: 3. Other plans referencing them: **9**.

**Incoming plan edges (top 8):**

| Plan | Mentions |
|---|---:|
| `PLAN-LIFECYCLE-SEALING-32` | 3 |
| `PLAN-RUNTIME-RESILIENCE-57` | 3 |
| `PLAN-INTEGRATION-KIT-02` | 2 |
| `EVIDENCE` | 2 |
| `PLAN-ORPHAN-SEAL-01` | 1 |
| `PLAN-UNBLOCK-03` | 1 |
| `PLAN-RUNTIME-PERF-16` | 1 |
| `PLAN-SELFTEST-TRUTH-23` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidates |
|---|---|
| `BGT-147A` | `SubsystemManifest.cs` |
| `BGT-147B` | `LedgerTruthIntegrityGate.cs` |
| `BGT-147C` | `LedgerTruthIntegrityGate.cs` |
| `BGT-147D` | `BootstrapLifecycleGate.cs` |
| `BGT-147E` | `SubsystemManifest.cs` |

**Reading:** incoming edges are coordination risk; candidates are a starting point for the touch map, not a decision.

---

## 13. Authority binding map

Symbols used: 3. Host files: **1** · Test files: **4** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 1 | `src/Main.Lifecycle.cs` |
| Tests (`Ashfall.Core.Tests/`) | 4 | `Ashfall.Core.Tests/Orchestration/BootstrapLifecycleGateTests.cs`, `Ashfall.Core.Tests/Orchestration/LedgerTruthIntegrityGateTests.cs`, `Ashfall.Core.Tests/Orchestration/SubsystemManifestTests.cs`, `Ashfall.Core.Tests/Tooling/BootstrapPathParityGateTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 11. Intra-domain reference graph (tier-2)

Small domain: 3 files; intra-domain edges: **0**; isolated: **3**.

| From | → To |
|---|---|
| — | no intra-domain references found |

**Reading:** a small isolated cluster gains its value from the host adapter, the save path, or data binding — not from internal callers.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **2** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `dose_ledger` |
| `nuclear_core_lifecycle` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **8** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--data-integrity-selftest` |
| `--dose-ledger-selftest` |
| `--expedition-panel-lifecycle` |
| `--ledger-debt-selftest` |
| `--panel-bind-lifecycle-selftest` |
| `--panel-lifecycle-selftest` |
| `--selftest-manifest` |
| `--test-manifest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **2**.

| Event | First declaration |
|---|---|
| `OnLedgerCalibrated` | `Assets/Ashfall.Core/DoseLedgerSystem.cs` |
| `OnLedgerTampered` | `Assets/Ashfall.Core/LedgerDebtSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **5**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/ledger_debt_templates.json` |
| `Assets/StreamingAssets/Data/mod_manifest_schema.json` |
| `Assets/StreamingAssets/Data/narrative/blast_gate_mechanical_audits.json` |
| `Assets/StreamingAssets/Data/narrative/bunker_trade_ledger_batch_2.json` |
| `Assets/StreamingAssets/Data/narrative_discovery_manifest.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **1** (1 files, 5 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Lifecycle` | 1 | 5 |

**Verdict:** 5 cases sit under matching regions — run those first (`Lifecycle`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **10**
(5 of them panels/HUD).

| Host file |
|---|
| `src/Host/DoseLedgerHostSession.cs` |
| `src/Host/DoseLedgerSaveStore.cs` |
| `src/Host/HostCli.SelfTestManifest.cs` |
| `src/Host/PanelBindLifecycleSelfTest.cs` |
| `src/Main.Lifecycle.cs` |
| `src/Main.PanelLifecycle.cs` |
| `src/Main.UiTests.StartingCohortLifecycle.cs` |
| `src/UI/CaravanBarterLedgerPanel.cs` |
| `src/UI/DoseLedgerPanel.cs` |
| `src/UI/SubterraneanDebtLedgerPanel.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **2**, of which versioned-ladder sections:
**1**.

| Section key | Laddered |
|---|---|
| `dose_ledger` | yes |
| `nuclear_core_lifecycle` | no |

**Verdict:** matched sections include versioned ladders — a schema change here must extend the existing ladder, not fork it.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **0**.

| Stream |
|---|
| — | no seeded stream shares a token with this domain |

**Verdict:** no seeded stream shares a token — either the domain is deterministic without randomness (fine) or it draws from an unlisted source (check `System.Random`/time seeding before shipping).

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **3**
(CODEX_ONLY 2, UNRESOLVED 1).

| Catalog | Classification |
|---|---|
| `ledger_debt_templates.json` | UNRESOLVED |
| `narrative/blast_gate_mechanical_audits.json` | CODEX_ONLY |
| `narrative/bunker_trade_ledger_batch_2.json` | CODEX_ONLY |

**Verdict:** 1 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **0**.

| Flag |
|---|
| — | no persistent flag shares a token with this domain |

**Verdict:** no persistent flag matches — the domain's narrative consequences are carried by other state (or the flag set is a candidate for cleanup).

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 9
**Surface:** save sections 2 (laddered 1) · RNG streams 0 · host files 10 · catalogs 8 · test regions 1 · flags 8

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-BOOTSTRAP-GATE-TRUTH-147
wave: 12
status: PROPOSED — foreman claim required
packages: BGT-147A, BGT-147B, BGT-147C, BGT-147D, BGT-147E
claim paths:
  - src/Host/DoseLedgerHostSession.cs  # §19 candidate host surface
  - src/Host/DoseLedgerSaveStore.cs  # §19 candidate host surface
  - src/Host/HostCli.SelfTestManifest.cs  # §19 candidate host surface
  - src/Host/PanelBindLifecycleSelfTest.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/ledger_debt_templates.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/mod_manifest_schema.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Lifecycle/
  - godot --headless --path . -- --data-integrity-selftest
dependencies:
  - coordinate: 9 other plan(s) name these artifacts (§12)
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
