# PLAN-DYNAMIC-QUESTLINE-TRUTH-212 — Generated Chains: Selection, Composition & Termination

**Wave 16 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-NARRATIVE-GRAPH-18, PLAN-CONTRACT-BOARD-109, PLAN-DATA-CONSUMER-22, PLAN-REACHABILITY (Plan 34).
**Non-goals:** no authored graph (Plan 18), no contract board (Plan 109), no
new prose generation from the model.

## 1. Outcome
Two reachable systems are unaddressed: `ExpansionQuestSystem.cs` (**383 lines**,
Core root) and `Quests/DynamicQuestlines.cs` (**374 lines**). Both compose
quests dynamically — and both can produce dead ends, unreachable objectives, or
chains that never terminate. The graph (Plan 18) and reachability (Plan 34/125)
own their parts; the **composition contract** is unstated.

| Deliverable | Detail |
|---|---|
| Selection rule | how a chain is chosen (state, region, day, prior chains) with a documented weight source and seeded roll |
| Composition rules | stages drawn from reachable content only; an objective's target must exist and be obtainable in the current world |
| Termination | every chain ends in a success, failure, or expiry state; no chain can run forever |
| Deduplication | chains already completed/served are not re-issued until a documented reset condition |
| Save truth | active chain stage and history restore; a load never re-rolls composition |

## 2. Evidence
- `Assets/Ashfall.Core/ExpansionQuestSystem.cs` (383 lines) and `Assets/Ashfall.Core/Quests/DynamicQuestlines.cs` (374 lines) — both unaddressed (Wave 16 audit).
- Plan 34/125 reachability checks are the source of "content exists".
- Plan 18 owns authored pieces the chains compose.
- Plan 109's board may host generated chains — boundary stated.

## 3. Packages
- **DQT-212A** selection rule + seeded roll test.
- **DQT-212B** composition reachability test (an unreachable objective fails composition).
- **DQT-212C** termination/expiry fixtures.
- **DQT-212D** dedup/reset test.
- **DQT-212E** save round-trip; no re-roll on load.

## 4. Acceptance & verification
- Same seed + same state → identical chain composition.
- No composed objective references missing content; every chain terminates.
- Save/load preserves stage and history.
- `bash scripts/run_test.sh` on the quest test region.

## 5. Risks
Infinite chains → termination is a fixture, not an assumption.
Unreachable objectives → composition validates against reachability before issuing.

---

## 6. Expanded census (4 files · 1,378 lines)

Scope: `Assets/Ashfall.Core/Quests/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Catalog 1 · Support 2 · System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `DynamicQuestGenerator.cs` | 390 | Support | — | 0 | 0 | 2 |
| `DynamicQuestlines.cs` | 374 | Support | **yes** | 0 | 0 | 2 |
| `NarrativeQuestlineCatalog.cs` | 237 | Catalog | — | 0 | 0 | 0 |
| `NarrativeQuestlineSystem.cs` | 377 | System | — | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 3 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `dynamic_questlines.json` | object[2 keys] |
| `narrative_questlines.json` | object[2 keys] |
| `questline_master.json` | array[511] |
| `verdict_questlines.json` | object[2 keys] |
| `year_of_ash_questlines.json` | object[2 keys] |

**State surfaces:** `DynamicQuestGenerator.cs`, `DynamicQuestlines.cs`, `NarrativeQuestlineSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Quests/` |
| Test references | 6 name references across the test tree |
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

Domain files: 4. Other plans referencing their names: **2**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-QUEST-RUNTIME-TRUTH-247` | 4 |
| `PLAN-NARRATIVE-GRAPH-18` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `DQT-212A` | no name match — resolve at claim time |
| `DQT-212B` | no name match — resolve at claim time |
| `DQT-212C` | no name match — resolve at claim time |
| `DQT-212D` | no name match — resolve at claim time |
| `DQT-212E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 5. Host files: **4** · Test files: **7** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 4 | `src/Host/ExpansionQuestHostSession.cs`, `src/Host/HostCli.NpcArcSelfTest.cs`, `src/Host/NarrativeQuestlineHostSession.cs`, `src/Host/ProceduralNarrativeHostSession.cs` |
| Tests (`Ashfall.Core.Tests/`) | 7 | `Ashfall.Core.Tests/ExpansionQuestSystemTests.cs`, `Ashfall.Core.Tests/Narrative/Plan96_104ChronicleQuestIntegrationTests.cs`, `Ashfall.Core.Tests/NarrativeQuestlineSystemTests.cs`, `Ashfall.Core.Tests/NpcArcDataTests.cs`, `Ashfall.Core.Tests/NpcArcSystemTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 11. Intra-domain reference graph (tier-2)

Small domain: 5 files; intra-domain edges: **0**; isolated: **5**.

| From | → To |
|---|---|
| — | no intra-domain references found |

**Reading:** a small isolated cluster gains its value from the host adapter, the save path, or data binding — not from internal callers.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **5** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `dynamic_quests` |
| `expansion_quest` |
| `narrative` |
| `narrative_questlines` |
| `procedural_narrative` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **2** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--narrative-selftest` |
| `--personal-quest-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **12**.

| Event | First declaration |
|---|---|
| `OnBlightNarrative` | `Assets/Ashfall.Core/Farming/AgricultureSystem.cs` |
| `OnNarrativeMarker` | `Assets/Ashfall.Core/District8DeepCoastSystem.cs` |
| `OnNarrativeRequested` | `Assets/Ashfall.Core/Warlords/WarlordDoctrineSystem.cs` |
| `OnQuestChoiceTaken` | `Assets/Ashfall.Core/YearOfAsh/QuestlineSystem.cs` |
| `OnQuestCompleted` | `Assets/Ashfall.Core/ExpansionQuestSystem.cs` |
| `OnQuestFailed` | `Assets/Ashfall.Core/ExpansionQuestSystem.cs` |
| `OnQuestStageAdvanced` | `Assets/Ashfall.Core/DutyRoster/DutyRosterQuestRuntime.cs` |
| `OnQuestStageChanged` | `Assets/Ashfall.Core/HoldfastQuestSystem.cs` |
| `OnQuestStarted` | `Assets/Ashfall.Core/ExpansionQuestSystem.cs` |
| `OnQuestlineResolved` | `Assets/Ashfall.Core/Muster/MusterSystem.cs` |
| `OnQuestlineStarted` | `Assets/Ashfall.Core/YearOfAsh/QuestlineSystem.cs` |
| `OnStageNarrativeEmitted` | `Assets/Ashfall.Core/Crossing/CrossingQuestSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/dynamic_quest_templates.json` |
| `Assets/StreamingAssets/Data/dynamic_questlines.json` |
| `Assets/StreamingAssets/Data/narrative/quest_narrative_documents.json` |
| `Assets/StreamingAssets/Data/narrative_arc_events.json` |
| `Assets/StreamingAssets/Data/narrative_discovery_manifest.json` |
| `Assets/StreamingAssets/Data/narrative_encounters.json` |
| `Assets/StreamingAssets/Data/narrative_encounters_expansion.json` |
| `Assets/StreamingAssets/Data/narrative_encounters_npc_arcs.json` |
| `Assets/StreamingAssets/Data/narrative_progression.json` |
| `Assets/StreamingAssets/Data/narrative_questlines.json` |
| `Assets/StreamingAssets/Data/quest_templates.json` |
| `Assets/StreamingAssets/Data/questline_master.json` |

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

Host files (`src/`) whose names share a domain token: **27**
(6 of them panels/HUD).

| Host file |
|---|
| `src/Host/DynamicQuestSaveStore.cs` |
| `src/Host/ExpansionQuestHostSession.cs` |
| `src/Host/ExpansionQuestSaveStore.cs` |
| `src/Host/HostCli.DynamicWorld.cs` |
| `src/Host/NarrativeArcConsequenceAdapter.cs` |
| `src/Host/NarrativeContinuitySelfTest.cs` |
| `src/Host/NarrativeHostSession.cs` |
| `src/Host/NarrativeQuestlineHostSession.cs` |
| `src/Host/NarrativeQuestlineSaveStore.cs` |
| `src/Host/NarrativeSaveStore.cs` |
| `src/Host/PersonalQuestHostSession.cs` |
| `src/Host/PersonalQuestSaveStore.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **5**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `dynamic_quests` | no |
| `expansion_quest` | no |
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

Matching catalogs in `artifacts/content-utilization-baseline.json`: **290**
(CODEX_ONLY 279, GAMEPLAY_CONSUMED 7, OPTIONAL 3, UNRESOLVED 1).

| Catalog | Classification |
|---|---|
| `dynamic_questlines.json` | GAMEPLAY_CONSUMED |
| `moral_choice_quest_stubs.json` | OPTIONAL |
| `narrative/activated_carbon_adsorption_records.json` | CODEX_ONLY |
| `narrative/ammo_hoist_jam_reports.json` | CODEX_ONLY |
| `narrative/ammonia_chiller_leak_logs.json` | CODEX_ONLY |
| `narrative/annealing_lehr_birefringence_records.json` | CODEX_ONLY |
| `narrative/antler_horn_sawing_records.json` | CODEX_ONLY |
| `narrative/apiculture_red_light_audits.json` | CODEX_ONLY |
| `narrative/aramid_fiber_rot_reports.json` | CODEX_ONLY |
| `narrative/architect_vault_audits.json` | CODEX_ONLY |

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
**Surface:** save sections 5 (laddered 0) · RNG streams 1 · host files 13 · catalogs 22 · test regions 2 · flags 2

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-DYNAMIC-QUESTLINE-TRUTH-212
wave: 16
status: PROPOSED — foreman claim required
packages: DQT-212A, DQT-212B, DQT-212C, DQT-212D, DQT-212E
claim paths:
  - src/Host/DynamicQuestSaveStore.cs  # §19 candidate host surface
  - src/Host/ExpansionQuestHostSession.cs  # §19 candidate host surface
  - src/Host/ExpansionQuestSaveStore.cs  # §19 candidate host surface
  - src/Host/HostCli.DynamicWorld.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/dynamic_quest_templates.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/dynamic_questlines.json  # §17 catalog (verify schema + consumer)
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
