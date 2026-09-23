# PLAN-NARRATIVE-CONTINUITY-TRUTH-170 — Runtime Continuity Checks for Authored Story

**Wave 13 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-NARRATIVE-GRAPH-18, PLAN-NARRATIVE-CONSEQUENCE-TRUTH-132, PLAN-MORAL-CHOICE-TRUTH-136.
**Implementation scaffold:** [`PLAN-NARRATIVE-CONTINUITY-TRUTH-170_APPENDIX-A_SCAFFOLD.md`](PLAN-NARRATIVE-CONTINUITY-TRUTH-170_APPENDIX-A_SCAFFOLD.md) — paired with `PLAN-NARRATIVE-FAMILY-TRUTH-261` as scaffolding authority: source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no prose edits, no graph restructure (Plan 18), no consequence
rules (Plan 132).

## 1. Outcome
`Narrative/Continuity/NarrativeContinuityEngine.cs` (671 lines) is reachable and
unaddressed. Plan 18 owns the story graph, Plan 132 the consequence rules,
Plan 136 the moral-choice flags. The continuity engine is the **runtime
validator** that should catch contradictions before a player sees them — but
nothing states which invariants it checks, when it runs, or what a failure does.

| Deliverable | Detail |
|---|---|
| Invariant set | the contradictions it detects (flag A with text B, dead character speaking, stage order violation) with ids and severity |
| Run points | when checks execute (load, day boundary, conversation start) and the bounded cost of each |
| Failure behavior | dev build: loud typed report; player build: documented degradation (a fallback line or omission), never a crash or a silent contradiction |
| Coverage | every invariant has a fixture that triggers it; a clean campaign produces zero reports |
| Reporting | output names the record ids involved so an author can act without a debugger |

## 2. Evidence
- `Assets/Ashfall.Core/Narrative/Continuity/NarrativeContinuityEngine.cs` (671 lines; unmentioned in every plan body — Wave 13 audit).
- The repo also carries a narrative-continuity audit skill for authored data — the runtime engine complements it; the plan notes the relationship.
- Plan 132's validator covers graph shape; this engine covers state-vs-text contradictions at runtime.
- Plan 136's flags are one of the state inputs.

## 3. Packages
- **NCT-170A** invariant catalogue with severity.
- **NCT-170B** run-point table + cost bound.
- **NCT-170C** dev-vs-player failure behavior + one fixture each.
- **NCT-170D** coverage fixtures (one per invariant) + clean-run test.
- **NCT-170E** report format with record ids.

## 4. Acceptance & verification
- Each invariant fixture triggers exactly its report; a clean scripted campaign reports zero.
- Player-build failure degrades per the documented path; no crash, no silent contradiction.
- Run-point costs stay within their documented bounds in the fixture.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Narrative/`.

## 5. Risks
Validator becoming authoring → it reports; authoring changes route to owners.
Overlap with 132 → graph shape vs runtime state; the boundary table states each.

---

## 6. Expanded census (3 files · 919 lines)

Scope: `Assets/Ashfall.Core/Narrative/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Support 2 · System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `NarrativeContinuityAllowlist.cs` | 63 | Support | — | 0 | 0 | 0 |
| `NarrativeContinuityEngine.cs` | 671 | System | **yes** | 0 | 0 | 0 |
| `NarrativeContinuityModel.cs` | 185 | Support | — | 0 | 0 | 0 |

**Totals:** 0 banned refs · 0 empty catches · 0 files with capture/restore.

## 7. Expanded data & state surface

No domain catalog matched; the plan's data path is loader-injected — verify before claiming.

**State surfaces:** none.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Narrative/` |
| Test references | 1 name references across the test tree |
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

Domain files: 3. Other plans referencing their names: **2**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-NARRATIVE-GRAPH-18` | 3 |
| `PLAN-NARRATIVE-FAMILY-TRUTH-261` | 3 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `NCT-170A` | no name match — resolve at claim time |
| `NCT-170B` | no name match — resolve at claim time |
| `NCT-170C` | no name match — resolve at claim time |
| `NCT-170D` | no name match — resolve at claim time |
| `NCT-170E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 3. Host files: **1** · Test files: **1** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 1 | `src/Host/NarrativeContinuitySelfTest.cs` |
| Tests (`Ashfall.Core.Tests/`) | 1 | `Ashfall.Core.Tests/Narrative/NarrativeContinuityTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 11. Intra-domain reference graph (tier-2)

Small domain: 3 files; intra-domain edges: **1**; isolated: **1**.

| From | → To |
|---|---|
| `NarrativeContinuityEngine` | `NarrativeContinuityAllowlist` |

**Reading:** a small isolated cluster gains its value from the host adapter, the save path, or data binding — not from internal callers.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **3** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `narrative` |
| `narrative_questlines` |
| `procedural_narrative` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **1** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--narrative-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **4**.

| Event | First declaration |
|---|---|
| `OnBlightNarrative` | `Assets/Ashfall.Core/Farming/AgricultureSystem.cs` |
| `OnNarrativeMarker` | `Assets/Ashfall.Core/District8DeepCoastSystem.cs` |
| `OnNarrativeRequested` | `Assets/Ashfall.Core/Warlords/WarlordDoctrineSystem.cs` |
| `OnStageNarrativeEmitted` | `Assets/Ashfall.Core/Crossing/CrossingQuestSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **8**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/narrative/quest_narrative_documents.json` |
| `Assets/StreamingAssets/Data/narrative_arc_events.json` |
| `Assets/StreamingAssets/Data/narrative_discovery_manifest.json` |
| `Assets/StreamingAssets/Data/narrative_encounters.json` |
| `Assets/StreamingAssets/Data/narrative_encounters_expansion.json` |
| `Assets/StreamingAssets/Data/narrative_encounters_npc_arcs.json` |
| `Assets/StreamingAssets/Data/narrative_progression.json` |
| `Assets/StreamingAssets/Data/narrative_questlines.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **2** (27 files, 313 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Narrative` | 26 | 293 |
| `NarrativeConsequence` | 1 | 20 |

**Verdict:** 313 cases sit under matching regions — run those first (`Narrative`, `NarrativeConsequence`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **13**
(1 of them panels/HUD).

| Host file |
|---|
| `src/Host/NarrativeArcConsequenceAdapter.cs` |
| `src/Host/NarrativeContinuitySelfTest.cs` |
| `src/Host/NarrativeHostSession.cs` |
| `src/Host/NarrativeQuestlineHostSession.cs` |
| `src/Host/NarrativeQuestlineSaveStore.cs` |
| `src/Host/NarrativeSaveStore.cs` |
| `src/Host/ProceduralNarrativeHostSession.cs` |
| `src/Host/ProceduralNarrativeSaveStore.cs` |
| `src/Main.Narrative.cs` |
| `src/Main.NarrativeQuestlines.cs` |
| `src/Main.SleepNarrative.cs` |
| `src/UI/FactionsNarrativePanel.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **3**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `narrative` | no |
| `narrative_questlines` | no |
| `procedural_narrative` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **1**.

| Stream |
|---|
| `narrative` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **285**
(CODEX_ONLY 279, GAMEPLAY_CONSUMED 3, OPTIONAL 2, UNRESOLVED 1).

| Catalog | Classification |
|---|---|
| `narrative/activated_carbon_adsorption_records.json` | CODEX_ONLY |
| `narrative/ammo_hoist_jam_reports.json` | CODEX_ONLY |
| `narrative/ammonia_chiller_leak_logs.json` | CODEX_ONLY |
| `narrative/annealing_lehr_birefringence_records.json` | CODEX_ONLY |
| `narrative/antler_horn_sawing_records.json` | CODEX_ONLY |
| `narrative/apiculture_red_light_audits.json` | CODEX_ONLY |
| `narrative/aramid_fiber_rot_reports.json` | CODEX_ONLY |
| `narrative/architect_vault_audits.json` | CODEX_ONLY |
| `narrative/armored_cockroach_hive_logs.json` | CODEX_ONLY |
| `narrative/armored_locomotive_manifests.json` | CODEX_ONLY |

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

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 2
**Surface:** save sections 3 (laddered 0) · RNG streams 1 · host files 13 · catalogs 18 · test regions 2 · flags 1

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-NARRATIVE-CONTINUITY-TRUTH-170
wave: 13
status: PROPOSED — foreman claim required
packages: NCT-170A, NCT-170B, NCT-170C, NCT-170D, NCT-170E
claim paths:
  - src/Host/NarrativeArcConsequenceAdapter.cs  # §19 candidate host surface
  - src/Host/NarrativeContinuitySelfTest.cs  # §19 candidate host surface
  - src/Host/NarrativeHostSession.cs  # §19 candidate host surface
  - src/Host/NarrativeQuestlineHostSession.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/narrative/quest_narrative_documents.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/narrative_arc_events.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Narrative/
  - godot --headless --path . -- --narrative-selftest
dependencies:
  - coordinate: 2 other plan(s) name these artifacts (§12)
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
