# PLAN-QUEST-RUNTIME-TRUTH-247 — The Quest Engine Contract: Dispatch, State & Cleanup

**Wave 18 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-NARRATIVE-GRAPH-18, PLAN-DYNAMIC-QUESTLINE-TRUTH-212, PLAN-SAVE-MIGRATION-CORRIDOR-87.
**Non-goals:** no authored graph (Plan 18), no generated chains (Plan 212), no
new content.

## 1. Outcome
`Quests/QuestRuntimeCoordinator.cs` (**223 lines**) is reachable and
unaddressed: the runtime that dispatches quest events, tracks active stage
state, and cleans up finished quests. Plan 18 owns the graph, Plan 212 the
generated chains; the **runtime contract** between them and the save system is
unowned, so quest state can leak or duplicate.

| Deliverable | Detail |
|---|---|
| Dispatch model | events (advance, branch, complete, fail) with one owner per transition and idempotent handling |
| State ownership | active quest state lives in the coordinator; systems report facts, they do not write quest state |
| Cleanup rule | completed/failed quests release resources and listeners; a leak test proves it |
| Reentrancy | duplicate events (same fact twice) advance once; a fixture covers it |
| Save truth | active state restores; completed quests never re-open; no partial stage on load |

## 2. Evidence
- `Assets/Ashfall.Core/Quests/QuestRuntimeCoordinator.cs` (223 lines; unaddressed — Wave 18 audit).
- Plan 18/212 feed the runtime; the save section per Plan 87's ledger.
- Plans 146/185 open/advance quests — boundaries noted.

## 3. Packages
- **QRT-247A** dispatch model + idempotency fixture.
- **QRT-247B** state-ownership audit (no foreign writes).
- **QRT-247C** cleanup/leak test.
- **QRT-247D** reentrancy fixture (duplicate facts).
- **QRT-247E** save round-trip; no re-open/partial stage.

## 4. Acceptance & verification
- Duplicate events advance once; cleanup releases listeners in the leak test.
- Save/load restores stage exactly; completed quests stay closed.
- `bash scripts/run_test.sh` on the quest test region.

## 5. Risks
State leakage → cleanup test is the guard.
Partial loads → stage-level save round-trip.

---

## 6. Expanded census (9 files · 3,483 lines)

Scope: `Assets/Ashfall.Core/Quests/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Catalog 1 · DTO/Type 1 · Demo 1 · Support 3 · System 3

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `DynamicQuestGenerator.cs` | 390 | Support | — | 0 | 0 | 2 |
| `DynamicQuestlines.cs` | 374 | Support | — | 0 | 0 | 2 |
| `HoldfastQuests.cs` | 1185 | Support | — | 0 | 0 | 0 |
| `NarrativeQuestlineCatalog.cs` | 237 | Catalog | — | 0 | 0 | 0 |
| `NarrativeQuestlineSystem.cs` | 377 | System | — | 0 | 0 | 2 |
| `PersonalQuestHeadlessDemo.cs` | 100 | Demo | — | 0 | 0 | 3 |
| `PersonalQuestSystem.cs` | 395 | System | — | 0 | 0 | 2 |
| `QuestRuntimeCoordinator.cs` | 223 | System | **yes** | 0 | 0 | 2 |
| `QuestTypes.cs` | 202 | DTO/Type | — | 0 | 0 | 0 |

**Totals:** 0 banned refs · 0 empty catches · 6 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `dose_quests.json` | object[2 keys] |
| `duty_roster_quests.json` | object[2 keys] |
| `dynamic_questlines.json` | object[2 keys] |
| `holdfast_quests.json` | object[2 keys] |
| `quests_bureaucratic_morality.json` | object[2 keys] |
| `quests_faction_branching.json` | object[2 keys] |

**State surfaces:** `DynamicQuestGenerator.cs`, `DynamicQuestlines.cs`, `NarrativeQuestlineSystem.cs`, `PersonalQuestHeadlessDemo.cs`, `PersonalQuestSystem.cs`, `QuestRuntimeCoordinator.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Quests/` |
| Test references | 13 name references across the test tree |
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

Domain files: 9. Other plans referencing their names: **2**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-DYNAMIC-QUESTLINE-TRUTH-212` | 4 |
| `PLAN-NARRATIVE-GRAPH-18` | 2 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `QRT-247A` | no name match — resolve at claim time |
| `QRT-247B` | no name match — resolve at claim time |
| `QRT-247C` | no name match — resolve at claim time |
| `QRT-247D` | no name match — resolve at claim time |
| `QRT-247E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 11. Intra-domain reference graph (tier-2)

Domain files: 9; intra-domain edges: **2**; isolated files:
**5**.

**Top edges (by source name):**

| From | → To |
|---|---|
| `DynamicQuestGenerator` | `QuestRuntimeCoordinator` |
| `PersonalQuestHeadlessDemo` | `PersonalQuestSystem` |

**In-degree hubs:**

| File | inbound refs |
|---|---:|
| `PersonalQuestSystem` | 1 |
| `QuestRuntimeCoordinator` | 1 |
| `DynamicQuestGenerator` | 0 |
| `DynamicQuestlines` | 0 |
| `HoldfastQuests` | 0 |
| `NarrativeQuestlineCatalog` | 0 |
| `NarrativeQuestlineSystem` | 0 |
| `PersonalQuestHeadlessDemo` | 0 |
| `QuestTypes` | 0 |

**Class split:** hub 0 · sink 2 · source 2 · isolated 5.

**Reading:** sinks are the domain's load-bearing leaves (nothing inside depends
on them); hubs are rewiring risk — changing one fans out across the domain.
Isolated files are integration candidates: they compile but no sibling calls
them, so their value must come from the host adapter or the save path.

---

## 13. Authority binding map

Symbols used: 9. Host files: **5** · Test files: **7** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 5 | `src/Host/NarrativeQuestlineHostSession.cs`, `src/Host/PersonalQuestHostSession.cs`, `src/Host/ProceduralNarrativeHostSession.cs`, `src/Main.Plans166_169.cs`, `src/UI/QuestsPanel.cs` |
| Tests (`Ashfall.Core.Tests/`) | 7 | `Ashfall.Core.Tests/Narrative/Plan96_104ChronicleQuestIntegrationTests.cs`, `Ashfall.Core.Tests/NarrativeQuestlineSystemTests.cs`, `Ashfall.Core.Tests/Plan166_169ReloadReplayTests.cs`, `Ashfall.Core.Tests/Plan169ProceduralNarrativeTests.cs`, `Ashfall.Core.Tests/Quests/DynamicQuestGeneratorTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **9** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `dynamic_quests` |
| `expansion_quest` |
| `holdfast` |
| `holdfast_trade` |
| `narrative` |
| `narrative_questlines` |
| `personal_quests` |
| `procedural_narrative` |
| `quests` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **11** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--holdfast-briefing` |
| `--holdfast-runtime-selftest` |
| `--holdfast-runtime-ui-test` |
| `--holdfast-runtime-uitest` |
| `--holdfast-save-selftest` |
| `--holdfast-selftest` |
| `--holdfast-trade-save-selftest` |
| `--ice-road-tick-demo` |
| `--narrative-selftest` |
| `--personal-quest-selftest` |
| `--personal-quests-selftest` |

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
| `Assets/StreamingAssets/Data/crossing_quests.json` |
| `Assets/StreamingAssets/Data/dose_quests.json` |
| `Assets/StreamingAssets/Data/duty_roster_quests.json` |
| `Assets/StreamingAssets/Data/dynamic_quest_templates.json` |
| `Assets/StreamingAssets/Data/dynamic_questlines.json` |
| `Assets/StreamingAssets/Data/food_types.json` |
| `Assets/StreamingAssets/Data/holdfast_factions.json` |
| `Assets/StreamingAssets/Data/holdfast_flavor.json` |
| `Assets/StreamingAssets/Data/holdfast_items.json` |
| `Assets/StreamingAssets/Data/holdfast_locations.json` |
| `Assets/StreamingAssets/Data/holdfast_npcs.json` |
| `Assets/StreamingAssets/Data/holdfast_quests.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **4** (32 files, 351 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Holdfast` | 1 | 13 |
| `Narrative` | 26 | 293 |
| `NarrativeConsequence` | 1 | 20 |
| `Quests` | 4 | 25 |

**Verdict:** 351 cases sit under matching regions — run those first (`Holdfast`, `Narrative`, `NarrativeConsequence`, `Quests`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **46**
(9 of them panels/HUD).

| Host file |
|---|
| `src/Audio/AudioStateCoordinator.cs` |
| `src/Host/CoreDemoSession.cs` |
| `src/Host/DynamicQuestSaveStore.cs` |
| `src/Host/ExpansionQuestHostSession.cs` |
| `src/Host/ExpansionQuestSaveStore.cs` |
| `src/Host/HoldfastBriefingView.cs` |
| `src/Host/HoldfastDispatchLog.cs` |
| `src/Host/HoldfastFlavorCatalog.cs` |
| `src/Host/HoldfastRuntimeSession.cs` |
| `src/Host/HoldfastSaveStore.cs` |
| `src/Host/HoldfastTerminalPanel.cs` |
| `src/Host/HoldfastTradeSaveStore.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **9**, of which versioned-ladder sections:
**1**.

| Section key | Laddered |
|---|---|
| `dynamic_quests` | no |
| `expansion_quest` | no |
| `holdfast` | yes |
| `holdfast_trade` | no |
| `narrative` | no |
| `narrative_questlines` | no |
| `personal_quests` | no |
| `procedural_narrative` | no |
| `quests` | no |

**Verdict:** matched sections include versioned ladders — a schema change here must extend the existing ladder, not fork it.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **1**.

| Stream |
|---|
| `narrative` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **315**
(CODEX_ONLY 279, GAMEPLAY_CONSUMED 23, OPTIONAL 3, UNRESOLVED 10).

| Catalog | Classification |
|---|---|
| `crossing_quests.json` | GAMEPLAY_CONSUMED |
| `dose_quests.json` | GAMEPLAY_CONSUMED |
| `duty_roster_quests.json` | GAMEPLAY_CONSUMED |
| `dynamic_questlines.json` | GAMEPLAY_CONSUMED |
| `holdfast_factions.json` | GAMEPLAY_CONSUMED |
| `holdfast_flavor.json` | GAMEPLAY_CONSUMED |
| `holdfast_items.json` | GAMEPLAY_CONSUMED |
| `holdfast_locations.json` | GAMEPLAY_CONSUMED |
| `holdfast_npcs.json` | UNRESOLVED |
| `holdfast_quests.json` | GAMEPLAY_CONSUMED |

**Verdict:** 10 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

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
**Surface:** save sections 9 (laddered 1) · RNG streams 1 · host files 13 · catalogs 22 · test regions 4 · flags 11

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-QUEST-RUNTIME-TRUTH-247
wave: 18
status: PROPOSED — foreman claim required
packages: QRT-247A, QRT-247B, QRT-247C, QRT-247D, QRT-247E
claim paths:
  - src/Audio/AudioStateCoordinator.cs  # §19 candidate host surface
  - src/Host/CoreDemoSession.cs  # §19 candidate host surface
  - src/Host/DynamicQuestSaveStore.cs  # §19 candidate host surface
  - src/Host/ExpansionQuestHostSession.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/crossing_quests.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/dose_quests.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Holdfast/
  - godot --headless --path . -- --holdfast-briefing
dependencies:
  - coordinate: 2 other plan(s) name these artifacts (§12)
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
