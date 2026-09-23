# PLAN-NPC-ARCS-TRUTH-143 — Arc Stages, Gates & Completion Truth

**Wave 11 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-NARRATIVE-GRAPH-18, PLAN-FAMILY-DYNASTY-43, PLAN-BACKSTORY-REVEAL-TRUTH-126.
**Implementation scaffold:** [`PLAN-NPC-ARCS-TRUTH-143_APPENDIX-A_SCAFFOLD.md`](PLAN-NPC-ARCS-TRUTH-143_APPENDIX-A_SCAFFOLD.md) — paired with `PLAN-NARRATIVE-ARC-EVENT-TRUTH-176` as scaffolding authority: source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no new narrative store (Plan 18), no biography model (Plan 126
owns fact revelation), no relationship graph (Plan 43).

## 1. Outcome
`NpcArcs/` holds `NpcArcCatalog.cs` and `NpcArcSystem.cs` — a personal story
arc per survivor with stages and gates — and no plan states how an arc
progresses, what gates a stage, or what "completed" means for persistence and
the ending evaluators that may read it (Plans 130/137).

| Deliverable | Detail |
|---|---|
| Arc model | stages with entry gates; one owner per transition; an arc cannot skip a stage without the documented bypass |
| Gate sources | gates read existing owners (relationship, revealed facts, duty history); arcs never keep private copies |
| Pause/resume | an interrupted arc resumes at the stage it paused in; a death mid-arc records the terminal stage explicitly |
| Completion truth | `completed` is a stored state with a day; Plan 130/137 read it, they do not recompute it |
| Save truth | stage + gate state restore; loading never replays a stage transition |

## 2. Evidence
- `Assets/Ashfall.Core/NpcArcs/`: the two files above (verified).
- Plan 126 supplies fact revelation gates; Plan 43 supplies relationship state.
- Plan 18 owns the story graph; arcs are per-survivor tracks over it.
- Plan 137's ending inputs may include arc completion — the stored state is the contract.

## 3. Packages
- **NAT-143A** arc model + stage/gate table.
- **NAT-143B** gate-source tests (each source one fixture).
- **NAT-143C** pause/resume + death-mid-arc tests.
- **NAT-143D** completion-state test consumed by a Plan 130/137 fixture.
- **NAT-143E** save round-trip; no stage replay on load.

## 4. Acceptance & verification
- Every arc reaches a terminal stage without skips; bypass is explicit and tested.
- Gate values equal their owners (no drift after a scripted day).
- Save/load mid-arc resumes exactly; no transition re-fires.
- `bash scripts/run_test.sh Ashfall.Core.Tests/NpcArcs/` (create if absent).

## 5. Risks
Private gate copies → gates read owners; the drift test enforces it.
Completion ambiguity → stored state with a day; consumers never recompute.

---

## 6. Expanded census (2 files · 359 lines)

Scope: `Assets/Ashfall.Core/NpcArcs/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Catalog 1 · System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `NpcArcCatalog.cs` | 161 | Catalog | **yes** | 0 | 0 | 0 |
| `NpcArcSystem.cs` | 198 | System | **yes** | 0 | 0 | 0 |

**Totals:** 0 banned refs · 0 empty catches · 0 files with capture/restore.

## 7. Expanded data & state surface

No domain catalog matched; the plan's data path is loader-injected — verify before claiming.

**State surfaces:** none.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/NpcArcs/` (create if absent) |
| Test references | 3 name references across the test tree |
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

Domain files: 2. Other plans referencing their names: **0**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| — | no other plan references these files |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `NAT-143A` | no name match — resolve at claim time |
| `NAT-143B` | no name match — resolve at claim time |
| `NAT-143C` | no name match — resolve at claim time |
| `NAT-143D` | no name match — resolve at claim time |
| `NAT-143E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 3. Host files: **2** · Test files: **2** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 2 | `src/Host/HostCli.NpcArcSelfTest.cs`, `src/Main.NpcArcs.cs` |
| Tests (`Ashfall.Core.Tests/`) | 2 | `Ashfall.Core.Tests/NpcArcDataTests.cs`, `Ashfall.Core.Tests/NpcArcSystemTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **1** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `psychological_arcs` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **1** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--real-main-journey-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **0**.

| Event | First declaration |
|---|---|
| — | no event name shares a token with this domain |

**Verdict:** no event name shares a token with this domain — the domain is command-polled, data-driven, or silently unreachable. Confirm against the event catalog before treating it as a gap.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **4**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/mental_arcs.json` |
| `Assets/StreamingAssets/Data/narrative_encounters_npc_arcs.json` |
| `Assets/StreamingAssets/Data/npc_arcs.json` |
| `Assets/StreamingAssets/Data/quests_npc_arcs.json` |

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

Host files (`src/`) whose names share a domain token: **1**
(0 of them panels/HUD).

| Host file |
|---|
| `src/Main.NpcArcs.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **1**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `psychological_arcs` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

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
(UNRESOLVED 3).

| Catalog | Classification |
|---|---|
| `narrative_encounters_npc_arcs.json` | UNRESOLVED |
| `npc_arcs.json` | UNRESOLVED |
| `quests_npc_arcs.json` | UNRESOLVED |

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

**Readiness:** READY (12/12) · **Class:** free-start · **Coupling (incoming plans):** 0
**Surface:** save sections 1 (laddered 0) · RNG streams 0 · host files 1 · catalogs 7 · test regions 0 · flags 1

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-NPC-ARCS-TRUTH-143
wave: 11
status: PROPOSED — foreman claim required
packages: NAT-143A, NAT-143B, NAT-143C, NAT-143D, NAT-143E
claim paths:
  - src/Main.NpcArcs.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/mental_arcs.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/narrative_encounters_npc_arcs.json  # §17 catalog (verify schema + consumer)
verification:
  - godot --headless --path . -- --real-main-journey-selftest
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
