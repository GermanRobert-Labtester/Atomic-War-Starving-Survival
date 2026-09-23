# PLAN-CROSSING-QUEST-TRUTH-190 — The Crossing: Passage Decisions, Stages & Outcomes

**Wave 14 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-TRANSPORT-EXPEDITION-30, PLAN-ASYLUM-REFUGEES-85, PLAN-NARRATIVE-GRAPH-18.
**Non-goals:** no travel math (Plan 30), no arrival admission (Plan 85), no
story graph storage (Plan 18).

## 1. Outcome
`Crossing/CrossingQuestSystem.cs` (**490 lines**) is reachable and unaddressed:
a named journey arc with stages and a decision at its heart. The Crossing is
where travel, risk, and story meet — and where a stalled campaign can strand a
player if the contract is unstated.

| Deliverable | Detail |
|---|---|
| Stage model | stages with entry gates and a documented fallback for each gate (never an unpassable state) |
| Passage decision | the central choice with recorded consequences routed to Plan 18/136 owners; one outcome per decision |
| Travel coupling | intermediate legs use Plan 30's travel and Plan 156's context; this system consumes their outcomes |
| Failure/retreat | a failed or abandoned crossing leaves a documented state with a recovery path to the holdfast |
| Save truth | stage, decision, and traveled legs restore; no re-roll or stage skip on load |

## 2. Evidence
- `Assets/Ashfall.Core/Crossing/CrossingQuestSystem.cs` (490 lines; unaddressed — Wave 13 audit).
- Plan 30 supplies leg travel; Plan 156 the context contract.
- Plan 85 owns arrival/admission at the destination.
- Plan 170 catches contradictions the decision may create.

## 3. Packages
- **CQT-190A** stage model + gate fallback table.
- **CQT-190B** passage decision + consequence routing tests.
- **CQT-190C** travel coupling fixtures (success/failure legs).
- **CQT-190D** failure/retreat path + recovery to holdfast.
- **CQT-190E** save round-trip; no skip/reroll on load.

## 4. Acceptance & verification
- Every gate has a reachable fallback; no stage requires an unobtainable item.
- The decision applies once and routes to its stated owners.
- Abandonment recovers to a playable state; save/load preserves the stage.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Crossing/` (create if absent).

## 5. Risks
Campaign stranding → fallbacks are mandatory fixtures.
Decision replay → once-only decision ledger with reload test.

---

## 6. Expanded census (2 files · 660 lines)

Scope: `Assets/Ashfall.Core/Crossing/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Support 1 · System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `CrossingQuestSystem.cs` | 490 | System | **yes** | 0 | 0 | 2 |
| `CrossingThirdonaryIntegration.cs` | 170 | Support | — | 0 | 0 | 0 |

**Totals:** 0 banned refs · 0 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `crossing_locations.json` | object[2 keys] |
| `crossing_quests.json` | object[2 keys] |
| `crossing_encounters.json` | object[3 keys] |
| `crossing_factions.json` | object[2 keys] |
| `crossing_items.json` | array[25] |

**State surfaces:** `CrossingQuestSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Crossing/` (create if absent) |
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

Domain files: 2. Other plans referencing their names: **0**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| — | no other plan references these files |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `CQT-190A` | no name match — resolve at claim time |
| `CQT-190B` | no name match — resolve at claim time |
| `CQT-190C` | no name match — resolve at claim time |
| `CQT-190D` | no name match — resolve at claim time |
| `CQT-190E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 7. Host files: **10** · Test files: **10** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 10 | `src/Host/AssetCoverageScanner.cs`, `src/Host/AssetRegistry.cs`, `src/Host/ContentUtilizationRuntimeCollector.cs`, `src/Host/ExpansionHostSession.cs`, `src/Main.GameFlow.cs` |
| Tests (`Ashfall.Core.Tests/`) | 10 | `Ashfall.Core.Tests/CrossingItemsPlan126Tests.cs`, `Ashfall.Core.Tests/CrossingQuestSystemTests.cs`, `Ashfall.Core.Tests/CrossingThirdonaryIntegrationTests.cs`, `Ashfall.Core.Tests/FactionIconCatalogTests.cs`, `Ashfall.Core.Tests/FactionWarLocationOverridesExpansionTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **10** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `crossing` |
| `dynamic_quests` |
| `encounters` |
| `expansion_quest` |
| `faction_espionage` |
| `factions` |
| `personal_quests` |
| `quests` |
| `thirdonary` |
| `travel_encounters` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **8** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--asset-coverage-report` |
| `--asset-registry-selftest` |
| `--crossing-selftest` |
| `--faction-communique-board-selftest` |
| `--faction-ecology-selftest` |
| `--personal-quest-selftest` |
| `--personal-quests-selftest` |
| `--real-main-journey-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **13**.

| Event | First declaration |
|---|---|
| `OnFactionStandingChanged` | `Assets/Ashfall.Core/YearOfAsh/FactionWarSystem.cs` |
| `OnFactionSuccession` | `Assets/Ashfall.Core/Economy/IEconomyInterfaces.cs` |
| `OnFactionSurrender` | `Assets/Ashfall.Core/Economy/IEconomyInterfaces.cs` |
| `OnLocationMutated` | `Assets/Ashfall.Core/LocationEvolutionSystem.cs` |
| `OnLocationOwnerChanged` | `Assets/Ashfall.Core/LocationEvolutionSystem.cs` |
| `OnLocationRecast` | `Assets/Ashfall.Core/StandingRecord/LocationMemorySystem.cs` |
| `OnLocationRevealed` | `Assets/Ashfall.Core/Radio/SignalTriangulationSystem.cs` |
| `OnQuestChoiceTaken` | `Assets/Ashfall.Core/YearOfAsh/QuestlineSystem.cs` |
| `OnQuestCompleted` | `Assets/Ashfall.Core/ExpansionQuestSystem.cs` |
| `OnQuestFailed` | `Assets/Ashfall.Core/ExpansionQuestSystem.cs` |
| `OnQuestStageAdvanced` | `Assets/Ashfall.Core/DutyRoster/DutyRosterQuestRuntime.cs` |
| `OnQuestStageChanged` | `Assets/Ashfall.Core/HoldfastQuestSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **9**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/crossing_encounters.json` |
| `Assets/StreamingAssets/Data/crossing_factions.json` |
| `Assets/StreamingAssets/Data/crossing_items.json` |
| `Assets/StreamingAssets/Data/crossing_locations.json` |
| `Assets/StreamingAssets/Data/crossing_quests.json` |
| `Assets/StreamingAssets/Data/dynamic_quest_templates.json` |
| `Assets/StreamingAssets/Data/narrative/quest_narrative_documents.json` |
| `Assets/StreamingAssets/Data/quest_templates.json` |
| `Assets/StreamingAssets/Data/thirdonary_quests.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **1** (16 files, 74 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Integration` | 16 | 74 |

**Verdict:** 74 cases sit under matching regions — run those first (`Integration`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **12**
(4 of them panels/HUD).

| Host file |
|---|
| `src/Host/DynamicQuestSaveStore.cs` |
| `src/Host/ExpansionQuestHostSession.cs` |
| `src/Host/ExpansionQuestSaveStore.cs` |
| `src/Host/PersonalQuestHostSession.cs` |
| `src/Host/PersonalQuestSaveStore.cs` |
| `src/Host/PersonalQuestSelfTest.cs` |
| `src/Host/ThirdonaryHostSession.cs` |
| `src/Host/ThirdonarySaveStore.cs` |
| `src/UI/CrossingQuestPanel.cs` |
| `src/UI/CrossingSafeConductVouchPanel.cs` |
| `src/UI/PersonalQuestPanel.cs` |
| `src/UI/QuestDetailPanel.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **3**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `crossing` | no |
| `expansion_quest` | no |
| `thirdonary` | no |

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

Matching catalogs in `artifacts/content-utilization-baseline.json`: **8**
(CODEX_ONLY 1, GAMEPLAY_CONSUMED 6, OPTIONAL 1).

| Catalog | Classification |
|---|---|
| `crossing_encounters.json` | GAMEPLAY_CONSUMED |
| `crossing_factions.json` | GAMEPLAY_CONSUMED |
| `crossing_items.json` | GAMEPLAY_CONSUMED |
| `crossing_locations.json` | GAMEPLAY_CONSUMED |
| `crossing_quests.json` | GAMEPLAY_CONSUMED |
| `moral_choice_quest_stubs.json` | OPTIONAL |
| `narrative/quest_narrative_documents.json` | CODEX_ONLY |
| `thirdonary_quests.json` | GAMEPLAY_CONSUMED |

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
**Surface:** save sections 3 (laddered 0) · RNG streams 0 · host files 12 · catalogs 17 · test regions 1 · flags 8

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-CROSSING-QUEST-TRUTH-190
wave: 14
status: PROPOSED — foreman claim required
packages: CQT-190A, CQT-190B, CQT-190C, CQT-190D, CQT-190E
claim paths:
  - src/Host/DynamicQuestSaveStore.cs  # §19 candidate host surface
  - src/Host/ExpansionQuestHostSession.cs  # §19 candidate host surface
  - src/Host/ExpansionQuestSaveStore.cs  # §19 candidate host surface
  - src/Host/PersonalQuestHostSession.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/crossing_encounters.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/crossing_factions.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Integration/
  - godot --headless --path . -- --asset-coverage-report
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
