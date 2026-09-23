# PLAN-LIFECYCLE-SEALING-32 — Node, Session & Subscription Lifetime

**Wave:** 4 (2026-09-21) · **Kind:** GAP SEALING
**Status:** PROPOSED — not a claim.
**Depends on:** PLAN-ORPHAN-SEAL-01 (new host sessions), PLAN-UI-SURFACE-15.
**Expanded appendix:** [`PLAN-LIFECYCLE-SEALING-32_APPENDIX-A_LIFETIME_INVENTORY.md`](PLAN-LIFECYCLE-SEALING-32_APPENDIX-A_LIFETIME_INVENTORY.md)
— the full lifetime inventory for `src/UI` + `src/Host`: lifecycle hooks,
subscription/release counts, timers, connections, and a verdict per file.
**Refined census: 44 COMPLETE · 66 PARTIAL · 214 MISSING · 324 NONE_NEEDED**
(the MISSING rows are the LF-32B work list, ordered first in the appendix).
**Non-goals:** no UI framework change, no refcount system, no wholesale Node
rewrite.

---

## 1. Outcome

The 2026-09-20 repair removed the reproduced **100-orphan control shutdown
signature** and fixed `ShelterOperationsAudioBridge.Dispose()`. That was one
incident in a surface that is structurally large:

- **517 UI files**; **229** reference `_Process`/`_Ready`/`_ExitTree`;
  only **115** reference `_ExitTree` or `Dispose`.
- **780 host files** with sessions, save stores and dirty flags; the programme
  is about to add ~99 host owners.
- Panel bind/unbind/rebind has a probe (17/17), but there is no general
  lifetime contract for sessions, timers, signals and subscriptions.

This plan makes lifetime a declared, checked property instead of a habit.

Deliverables:

1. a **lifetime inventory** for every UI/host type: how it is created, who owns
   it, what it subscribes to, what it must release, and whether it is
   idempotent;
2. a **subscription contract**: every `+=` to a long-lived event has a matching
   release on teardown (or a documented app-lifetime subscription);
3. **rebind safety**: open/close/reopen any routed panel N times with no
   duplicate subscriptions, timers, or nodes;
4. a **shutdown probe** that fails on orphan controls, live timers, or
   double-dispose at campaign teardown;
5. a **gate** that flags new `_Process` usage without a lifetime declaration.

---

## 2. Evidence (2026-09-21)

| Fact | Value | Command |
|---|---:|---|
| UI files | 517 | `ls src/UI \| wc -l` |
| UI files with `_Process`/`_Ready`/`_ExitTree` | 229 | grep |
| UI files with `_ExitTree`/`Dispose` | 115 | grep |
| Host session/store files | 780 | `ls src/Host \| wc -l` |
| `IDisposable` in Core / src UI | 5 / 5 | grep |
| Lifetime precedent | 100-orphan fix; `ShelterOperationsAudioBridge.Dispose` fix | INTEGRATION_PLANS 2026-09-20 |
| Panel probe | `--panel-bind-lifecycle-selftest` 17/17 | DEC-27 evidence |
| Repeated pair | bind → unbind → rebind exists as a concept | same |

---

## 3. Packages

### LF-32A — Lifetime inventory (generated)
- `docs/architecture/LIFECYCLE_INVENTORY.md`: `type | file | created by | owner |
  subscribes | timers | dispose hooks | idempotent | verdict`.
- Extraction: parse `_Ready`/`_ExitTree`/`Dispose`, `+=`/`-=` pairs, `Timer`,
  `SetProcess`, `Connect`/`Callable`, and session fields in `Main.*` partials.
- **Acceptance:** every UI file and every host session appears once; verdict
  `COMPLETE`/`PARTIAL`/`MISSING`.
- **Verify:** `python3 scripts/ci/generate-lifecycle-inventory.py --check`.

### LF-32B — Subscription contract
- Rule: a subscription to a long-lived Core event from a UI/host type must be
  released in teardown, or declared `AppLifetime` in the inventory.
- Sweep the `PARTIAL`/`MISSING` rows: add `-=`/dispose or record the
  app-lifetime reason.
- **Acceptance:** zero unclassified subscriptions; the event inventory
  (PLAN-EVENT-WIRING-21) and this inventory agree.
- **Verify:** the generator + focused UI tests.

### LF-32C — Rebind safety
- For every routed panel: open → close → reopen five times; assert one
  instance, one subscription set, one timer set, stable memory, and no
  duplicate journal/audio effects.
- **Acceptance:** the probe covers every route in the surface inventory;
  failures name the duplicated resource.
- **Verify:** `godot --headless --path . -- --panel-bind-lifecycle-selftest`
  (extended).

### LF-32D — Shutdown and teardown probe
- At campaign teardown: count live nodes/controls, running timers, connected
  signals, and undisposed sessions; assert zero beyond a declared baseline;
  assert dispose is idempotent (double-dispose does not throw).
- **Acceptance:** the 100-orphan signature is impossible to reintroduce without
  failing this probe.
- **Verify:** `godot --headless --path . -- --shutdown-hygiene-selftest` (new).

### LF-32E — Host session lifetime
- Session contract: created once in `ComposeCampaign` (or the manifest
  bootstrap), idempotent `SetupX`, `SaveX` on teardown if dirty, never a second
  instance per campaign, and a reset path for the session-reset lifecycle mode.
- **Acceptance:** the kit's `--integration-selftest` gains a lifetime probe per
  manifest entry (created exactly once, disposed on reset).
- **Verify:** `godot --headless --path . -- --integration-selftest`.

---

## 4. Risks

| Risk | Mitigation |
|---|---|
| Long-lived game-lifetime subscriptions are legitimate | `AppLifetime` verdict with a reason; not force-released |
| Probe false positives on engine-owned nodes | baseline declared and reviewed; only growth fails |
| Sweeping 229 files is large | generated inventory drives a bounded fix list; fix families, not one-offs |
| Dispose order dependencies | sessions dispose in reverse creation order; documented in the contract |

## 5. Verification

```bash
python3 scripts/ci/generate-lifecycle-inventory.py --check
godot --headless --path . -- --panel-bind-lifecycle-selftest
godot --headless --path . -- --shutdown-hygiene-selftest
godot --headless --path . -- --integration-selftest
bash scripts/run_test.sh Ashfall.Core.Tests/UI/
```

---

## 6. Expanded census (3 files · 579 lines)

Scope: `Assets/Ashfall.Core/Orchestration/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Support 3

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `BootstrapLifecycleGate.cs` | 128 | Support | **yes** | 0 | 0 | 0 |
| `LedgerTruthIntegrityGate.cs` | 137 | Support | — | 0 | 0 | 0 |
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
| `PLAN-RUNTIME-RESILIENCE-57` | 3 |
| `PLAN-BOOTSTRAP-GATE-TRUTH-147` | 3 |
| `PLAN-INTEGRATION-KIT-02` | 2 |
| `EVIDENCE` | 2 |
| `PLAN-ORPHAN-SEAL-01` | 1 |
| `PLAN-UNBLOCK-03` | 1 |
| `PLAN-RUNTIME-PERF-16` | 1 |
| `PLAN-SELFTEST-TRUTH-23` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidates |
|---|---|
| `LF-32A` | no name match — resolve at claim time |
| `LF-32B` | no name match — resolve at claim time |
| `LF-32C` | no name match — resolve at claim time |
| `LF-32D` | no name match — resolve at claim time |
| `LF-32E` | no name match — resolve at claim time |

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

**Readiness:** READY-WITH-NOTES (11/12) · **Class:** standard · **Coupling (incoming plans):** 9
**Surface:** save sections 2 (laddered 1) · RNG streams 0 · host files 10 · catalogs 8 · test regions 1 · flags 8

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-LIFECYCLE-SEALING-32
wave: —
status: PROPOSED — foreman claim required
packages: LF-32A, LF-32B, LF-32C, LF-32D, LF-32E
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
