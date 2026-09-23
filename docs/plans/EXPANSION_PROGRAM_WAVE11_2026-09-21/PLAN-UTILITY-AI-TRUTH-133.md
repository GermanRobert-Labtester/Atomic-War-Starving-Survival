# PLAN-UTILITY-AI-TRUTH-133 — Deterministic Action Scoring & Selection Traces

**Wave 11 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-DETERMINISM-CROSS-HOST-89, PLAN-ORPHAN-SEAL (UtilityAI package), PLAN-DAILY-ROUTINE-AUTHORITY-107.
**Implementation scaffold:** [`PLAN-UTILITY-AI-TRUTH-133_APPENDIX-A_SCAFFOLD.md`](PLAN-UTILITY-AI-TRUTH-133_APPENDIX-A_SCAFFOLD.md) — paired with `PLAN-DETERMINISM-CROSS-HOST-89` as scaffolding authority: source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no new AI architecture, no behaviour-tree or GOAP introduction, no
panel-visible scores as authority.

## 1. Outcome
`UtilityAI/` contains `UtilityAction.cs`, `UtilityActionScorer.cs`,
`UtilityAiSystem.cs`, and `UtilityAiHeadlessDemo.cs`. Utility selection is the
kind of system that silently becomes nondeterministic (ties, floats, iteration
order). Nothing today proves selection is seed-stable, that ties break by a
documented rule, or that a decision can be explained after the fact.

| Deliverable | Detail |
|---|---|
| Score contract | inputs per action (needs, state, context) with ranges; float comparisons use a documented epsilon |
| Tie-break rule | deterministic (e.g. stable action id order); no iteration-order dependence |
| Selection trace | a bounded trace of the last N selections (scores + winner + reason) available to dev tools, never authoritative |
| Headless parity | the existing headless demo path and in-host selection produce identical choices for the same state |
| Forbidden inputs | wall-clock, unseeded RNG, locale-dependent sorting are banned and scanned |

## 2. Evidence
- `Assets/Ashfall.Core/UtilityAI/`: the four files above (verified).
- `UtilityAiHeadlessDemo.cs` gives an existing headless entry for parity checks.
- Plan 89 supplies the cross-host parity pattern; Plan 107 consumes routine decisions.
- Plan 1 Appendix A: UtilityAI authorities are host-unreachable if their types are in the orphan set (re-verify).

## 3. Packages
- **UAT-133A** score contract doc + range table.
- **UAT-133B** tie-break rule + equal-score fixture test.
- **UAT-133C** selection trace buffer + dev-tool read (no runtime authority).
- **UAT-133D** headless↔host parity test on a scripted state.
- **UAT-133E** forbidden-input scan over `UtilityAI/`.

## 4. Acceptance & verification
- Same state + same seed → identical selection sequence, including ties.
- Parity test passes between the demo path and host path.
- Scan finds no banned input; a deliberate injection fails.
- `bash scripts/run_test.sh Ashfall.Core.Tests/UtilityAI/` (create if absent) + the headless demo.

## 5. Risks
Float drift across platforms → epsilon and integer comparison where possible; the parity test catches it early.
Trace becoming authority → read-only buffer; a test asserts simulation ignores it.

---

## 6. Expanded census (4 files · 474 lines)

Scope: `Assets/Ashfall.Core/UtilityAI/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Demo 1 · Support 2 · System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `UtilityAction.cs` | 161 | Support | — | 0 | 0 | 0 |
| `UtilityActionScorer.cs` | 93 | Support | **yes** | 0 | 0 | 0 |
| `UtilityAiHeadlessDemo.cs` | 96 | Demo | — | 0 | 0 | 0 |
| `UtilityAiSystem.cs` | 124 | System | **yes** | 1 | 0 | 0 |

**Totals:** 1 banned refs · 0 empty catches · 0 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `utility_actions.json` | object[2 keys] |

**State surfaces:** none.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/UtilityAI/` (create if absent) |
| Test references | 14 name references across the test tree |
| Determinism | 1 banned refs to fix or justify |
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

Domain files: 4. Other plans referencing their names: **0**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| — | no other plan references these files |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `UAT-133A` | `UtilityActionScorer.cs` |
| `UAT-133B` | `UtilityActionScorer.cs` |
| `UAT-133C` | no name match — resolve at claim time |
| `UAT-133D` | `UtilityAiHeadlessDemo.cs` |
| `UAT-133E` | `UtilityAiHeadlessDemo.cs`, `UtilityAiSystem.cs` |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 4. Host files: **3** · Test files: **5** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 3 | `src/Host/HostCli.PanelTests.cs`, `src/Host/UtilityAiHostSession.cs`, `src/UtilityAI/UtilityAiPanel.cs` |
| Tests (`Ashfall.Core.Tests/`) | 5 | `Ashfall.Core.Tests/Survivors/Plan88_72ConfessionUtilityAiIntegrationTests.cs`, `Ashfall.Core.Tests/Tooling/ArchitectureAuthorityGateTests.cs`, `Ashfall.Core.Tests/UtilityAiExpandedCatalogTests.cs`, `Ashfall.Core.Tests/UtilityAiProbeTests.cs`, `Ashfall.Core.Tests/UtilityAiTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 11. Intra-domain reference graph (tier-2)

Small domain: 4 files; intra-domain edges: **3**; isolated: **1**.

| From | → To |
|---|---|
| `UtilityAiHeadlessDemo` | `UtilityActionScorer` |
| `UtilityAiHeadlessDemo` | `UtilityAiSystem` |
| `UtilityAiSystem` | `UtilityActionScorer` |

**Reading:** a small isolated cluster gains its value from the host adapter, the save path, or data binding — not from internal callers.

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
| `--ice-road-tick-demo` |
| `--utility-ai-selftest` |
| `--utility-ai-uitest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **4**.

| Event | First declaration |
|---|---|
| `OnActionCompleted` | `Assets/Ashfall.Core/WorkshopReverseEngineeringSystem.cs` |
| `OnActionExecuted` | `Assets/Ashfall.Core/Warlords/WarlordDoctrineSystem.cs` |
| `OnActionResolved` | `Assets/Ashfall.Core/Muster/FactionActionBoard.cs` |
| `OnActionSelected` | `Assets/Ashfall.Core/UtilityAI/UtilityAiSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **1**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/utility_actions.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **0** (0 files, 0 cases).

| Region | Files | Cases |
|---|---:|---:|
| — | no test region shares a token with this domain |

**Verdict:** no test region shares a token with this domain. Region coverage is directory-based, so check root-level test files too (560 exist) before concluding coverage is absent.

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **7**
(2 of them panels/HUD).

| Host file |
|---|
| `src/Host/CoreDemoSession.cs` |
| `src/Host/UtilityAiHostSession.cs` |
| `src/Journal/JournalDemoHarness.cs` |
| `src/Main.UiTests.UtilityAi.cs` |
| `src/Muster/FactionActionPanel.cs` |
| `src/UI/SceneBindingHeadlessProbe.cs` |
| `src/UtilityAI/UtilityAiPanel.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **0**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| — | no section key shares a token with this domain |

**Verdict:** no save section matches — persistence is owned under a differently-named section, or absent.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **0**.

| Stream |
|---|
| — | no seeded stream shares a token with this domain |

**Verdict:** no seeded stream shares a token — either the domain is deterministic without randomness (fine) or it draws from an unlisted source (check `System.Random`/time seeding before shipping).

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **1**
(GAMEPLAY_CONSUMED 1).

| Catalog | Classification |
|---|---|
| `utility_actions.json` | GAMEPLAY_CONSUMED |

**Verdict:** matched catalogs are classified as gameplay-consumed — the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **0**.

| Flag |
|---|
| — | no persistent flag shares a token with this domain |

**Verdict:** no persistent flag matches — the domain's narrative consequences are carried by other state (or the flag set is a candidate for cleanup).

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** free-start · **Coupling (incoming plans):** 0
**Surface:** save sections 0 (laddered 0) · RNG streams 0 · host files 7 · catalogs 2 · test regions 0 · flags 3

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-UTILITY-AI-TRUTH-133
wave: 11
status: PROPOSED — foreman claim required
packages: UAT-133A, UAT-133B, UAT-133C, UAT-133D, UAT-133E
claim paths:
  - src/Host/CoreDemoSession.cs  # §19 candidate host surface
  - src/Host/UtilityAiHostSession.cs  # §19 candidate host surface
  - src/Journal/JournalDemoHarness.cs  # §19 candidate host surface
  - src/Main.UiTests.UtilityAi.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/utility_actions.json  # §17 catalog (verify schema + consumer)
  - utility_actions.json  # §17 catalog (verify schema + consumer)
verification:
  - godot --headless --path . -- --ice-road-tick-demo
dependencies:
  - governance spine must land first: 56 → 86 → 17 → 100
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
