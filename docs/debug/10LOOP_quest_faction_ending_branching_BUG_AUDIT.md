# ASHFALL 10-Loop Bug Audit

## 1. Audit Target

**Target:** Player-facing quest-choice, faction-branch, and ending integration surfaces relevant to persistent branching and player action histories.

**Source snapshot:** `5be1a30a63cd86cf23e4034473b739ac514f0f2a` plus the pre-existing working-tree changes present during this audit. Findings below describe the current working tree where noted; the repository was already dirty before this report was written.

## 2. Scope

This was a bounded static audit of:

- Personal quest data, Core choice transitions, host construction, UI choice callbacks, and save state.
- Narrative questline state and its branch model.
- Military, Rebel, and Independent branch admission and resolution.
- The active campaign outcome projection and the separate `UnifiedEndingResolver` utility.
- Focused tests and the references needed to distinguish an active runtime route from a test-only or uncalled class.

This was not a full-repository audit, a runtime playthrough, or a catalog-integrity run. No production code or data was changed. The audit did not run tests or add tests.

## 3. Baseline Verification

- `git rev-parse HEAD`: `5be1a30a63cd86cf23e4034473b739ac514f0f2a`.
- The worktree had 545 modified or untracked entries when the audit and plan files were about to be created. This includes user work in `PersonalQuestSystem.cs`, `WORKTREE_OWNERSHIP.md`, and expansion drafts under Wave 19. Those paths were treated as read-only.
- The personal-quest callback properties and their Core calls are uncommitted working-tree changes. The host wiring gap is reported against that current source state, not as a claim about the bare commit alone.
- Verification was static source/data/test inspection only. No test suite, Godot session, build, or data-integrity command was run.

## 4. Loop Completion Matrix

| Loop | Lens | Candidates examined | Confirmed | Rejected |
|---|---|---|---|---|
| 1 | Structural/static sweep | Optional personal-quest effect delegates; quest completion day; wall-clock ending identifier; narrative branch cardinality | Three concrete risks retained for reachability checks | “All campaign endings are morality-only” not accepted as a general claim |
| 2 | Call graph and runtime reachability | Main menu route → panel → host session → Core choice; active Main endgame projection; resolver callers | Personal-quest route is active; resolver has no production caller found in `src/` | Resolver timestamp bug is not treated as a live player route |
| 3 | State transitions | Choice lookup, selected-choice append, callbacks, completion/next-stage state | Completion uses caller-supplied day; missing callbacks do not block quest completion | No transition defect found in narrative binary branch itself |
| 4 | Save/restore | Personal quest `CaptureState`/`RestoreState`; branch coordinator save route; resolver `lastResult` | Wrong resolved day and absent reward outcome can persist with the quest record; resolver ID is save-visible | No save corruption or migration break was established |
| 5 | Determinism/ordering | Personal quest RNG use; resolver wall-clock ID; production call graph | Resolver output ID varies with UTC ticks for identical context | No production determinism failure attributed to this uncalled resolver |
| 6 | Data/catalog | `personal_quests.json` reward and morale fields; faction branch bands; current faction records | Authored nonzero effects exercise the missing bridge; branch entries use morality bands | Catalog validity was not claimed; no validator was run |
| 7 | Event/lifecycle/integration | Host event subscriptions, callback setup, Main save setup | State-change events are subscribed; reward callbacks are not assigned in the production host path | No duplicate subscription or teardown defect established |
| 8 | UI/player-facing path | Choice label and button handler; completed-quest list; faction panel commit handler | Day 1 is passed from the button; negative morale labels are malformed | No evidence that `CampaignOutcomeEvaluator` is bypassed |
| 9 | Test adversarial review | Plan 200 delegate test and host self-test references | Core test assigns delegates directly and does not exercise Main/host reward binding | Core delegate behavior is not evidence of a working player route |
| 10 | Cross-system synthesis | UI → host → Core → consequence application → save/UI; endgame route distinction | Two active personal-quest defects share the same exposed UI path; resolver issue remains non-production | Moral-band entry restrictions are a design limitation, not a confirmed runtime bug |

## 5. Executive Findings

Two confirmed defects affect the active personal-quest UI route: authored morale/item effects have no production callback bindings, and completed quests are stamped with day 1. A third low-severity display defect renders negative morale deltas with a redundant plus sign. Separately, a deterministic-claim violation exists in `UnifiedEndingResolver`, but no production caller was found; the live Main endgame route uses `CampaignOutcomeEvaluator`.

The audit found a design seam relevant to the requested expansion work, not a bug: narrative arcs currently use a linear objective ladder followed by one binary fork, while the three major faction branch families use morality-band admission. Campaign endgame evaluation already consumes several non-moral campaign facts, so “all endings are morality-only” would be an inaccurate diagnosis.

## 6. Critical Findings

None confirmed in scope.

## 7. High Findings

None confirmed in scope.

## 8. Medium Findings

### BUG-01 — Personal quest effects are not bound in the production host

**Severity:** MEDIUM  
**Confidence:** CONFIRMED in the current working tree  
**Category:** Integration / UI  
**Active Runtime:** YES

**Player impact:** Choosing an authored personal-quest option records the choice and advances or completes the quest, but its configured morale change and item reward are skipped through the active Godot route.

**Trigger:** Complete a personal quest stage whose selected option has a nonzero `morale_delta` or a nonempty `reward_item_id` with a positive `reward_amount`.

**Expected:** The choice applies its authored effects through the existing survivor morale and shelter inventory owners.

**Actual:** `PersonalQuestSystem.ChooseOption` invokes each effect only when its delegate is non-null. `PersonalQuestHostSession` constructs the Core system and subscribes state-change events, but does not assign either delegate. Main constructs this host session without supplying another bridge. Repository-wide search found no production assignment.

**Root cause:** The current uncommitted Core change adds optional delegate seams, but the host path that owns real survivor and inventory authorities does not bind them.

**Evidence:** `Assets/Ashfall.Core/Quests/PersonalQuestSystem.cs:99-100,245-251`; `src/Host/PersonalQuestHostSession.cs:26-39`; `src/Main.PersonalQuests.cs:25-35`; active route `src/Main.ExpandedShelterSystems.cs:637-639` and `src/UI/PersonalQuestPanel.cs:160-171`. Authored examples include antibiotics and scrap rewards at `Assets/StreamingAssets/Data/personal_quests.json:47-65` and `:112-135`.

**Affected systems:** Personal quest Core, Godot host, survivor morale, inventory, player UI.

**Save impact:** The selected choice and quest transition can be saved while the intended effects remain unapplied. The stored choice does not itself prove the reward was delivered.

**Determinism impact:** None established; the issue is omission of effects, not divergent ordering.

**Regression risk:** A future binding must avoid double-applying effects on repeated UI input or restore. This audit does not prescribe a specific repair.

**Suggested next analysis:** Trace the owning morale and inventory APIs and the current claimed integration package before deciding the minimal host binding and focused host-level evidence.

## 9. Low Findings

### BUG-02 — The personal-quest UI records every completion on Day 1

**Severity:** LOW  
**Confidence:** CONFIRMED  
**Category:** UI / State  
**Active Runtime:** YES

**Player impact:** Every quest completed from the visible panel displays `Resolved Day: 1`, regardless of the campaign day. That incorrect value is persisted in the completed quest record.

**Trigger:** Resolve a personal quest using a choice button after the campaign has advanced beyond Day 1.

**Expected:** `resolvedDay` reflects the campaign day on which the choice completed the quest.

**Actual:** The panel calls `_host.ChooseOption(sId, cId, 1, out _)`. Core copies that argument into `instance.resolvedDay`. The completed-quest panel displays the saved value.

**Root cause:** The UI button supplies a constant instead of the campaign calendar’s current day; the panel is bound only to the host session and has no day provider in the inspected source.

**Evidence:** `src/UI/PersonalQuestPanel.cs:164-171`; `Assets/Ashfall.Core/Quests/PersonalQuestSystem.cs:254-260`; completed list at `src/UI/PersonalQuestPanel.cs:205-216`; Main’s current-day projection is `src/Main.cs:53-56`.

**Affected systems:** Quest chronology, completed-quest UI, persisted personal-quest state.

**Save impact:** The incorrect `resolvedDay` is captured as part of the quest instance and survives reload.

**Determinism impact:** None; it is consistently wrong on this route.

**Regression risk:** Correcting the day source must preserve direct Core callers that already pass an explicit day.

**Suggested next analysis:** Confirm the panel’s intended day-source seam and inspect any downstream chronology consumer before a focused correction.

### BUG-03 — Negative morale changes display as “+-X Morale”

**Severity:** LOW  
**Confidence:** CONFIRMED  
**Category:** UI  
**Active Runtime:** YES

**Player impact:** Negative morale values are visually malformed and can make the direction of the consequence harder to scan.

**Trigger:** Display a personal-quest option with negative `morale_delta`.

**Expected:** The label uses a sign format that communicates a negative change once.

**Actual:** The panel unconditionally formats `(+{choice.morale_delta} Morale)`, so an authored value such as `-2.0` becomes `+-2 Morale`.

**Evidence:** `src/UI/PersonalQuestPanel.cs:168`; negative examples exist in `Assets/StreamingAssets/Data/personal_quests.json:28-34,96-102`.

**Affected systems:** Personal-quest choice presentation only.

**Save impact:** None.

**Determinism impact:** None.

**Regression risk:** Low; display formatting only.

**Suggested next analysis:** Review numeric formatting conventions used by adjacent panels before changing this label.

### BUG-04 — Unified ending resolution IDs depend on wall-clock time

**Severity:** LOW; non-production utility path  
**Confidence:** CONFIRMED in the utility  
**Category:** Determinism / Save  
**Active Runtime:** NO production caller found

**Player impact:** No direct active-route impact was established. A consumer of this public Core utility can obtain different `resolutionId` values for the same context when calls occur at different UTC ticks; that identifier is included in the saved `lastResult`.

**Trigger:** Call `UnifiedEndingResolver.ResolveEnding` more than once with identical context at different wall-clock ticks.

**Expected:** A class documented as a pure deterministic function of campaign state returns a stable identity for identical inputs.

**Actual:** `resolutionId` includes `DateTime.UtcNow.Ticks % 100000`.

**Root cause:** The resolver uses wall-clock time in output identity generation.

**Evidence:** `Assets/Ashfall.Core/Endgame/UnifiedEndingResolver.cs:103-108,157-163,431-438`. Production call search found the utility only in its declaration and tests; `src/Main.Endgame.cs:158-191` builds the live `CampaignOutcomeEvaluator` snapshot instead.

**Affected systems:** Direct consumers of the utility and its save DTO, if any are introduced.

**Save impact:** The timestamp-derived identifier is captured in `lastResult`.

**Determinism impact:** Confirmed for the utility; not attributed to the active Main endgame route.

**Regression risk:** Any future runtime integration needs a stable identity and should not assume the existing resolver ID is replay-stable.

**Suggested next analysis:** Confirm whether Plan 145 is intended to remain a test-only utility or to become the active ending authority before prioritizing this separately.

## 10. Suspected / Needs Reproduction

None retained. The above findings are confirmed by the active source path and authored data, but no runtime repro was run because this was a static audit.

## 11. Rejected False Positives

- **“All endings are morality-only.”** Rejected as an overbroad statement. `FactionBranchCoordinator.CanCommit` gates Military/Rebel/Independent entry by morality bands, with additional Independent standing/hostility checks. The active `CampaignOutcomeEvaluator` also considers treaty status, Tempest decommissioning, debt state, children, evidence, deaths, and campaign duration.
- **“The UnifiedEndingResolver timestamp bug is breaking the current epilogue.”** Rejected for active runtime. No production caller was found; Main’s current endgame projection uses `CampaignOutcomeEvaluator`.
- **“The binary narrative fork is itself broken.”** Rejected. `NarrativeQuestlineSystem` documents and enforces a linear objective ladder followed by one authored binary fork. That is a current design limit, not evidence of a failed state transition.
- **“Personal quest save state is not captured.”** Rejected. Main enrolls personal quests during save setup, restores the section, and captures through `PersonalQuestSaveStore`; the confirmed issue is that an unapplied effect and a wrong completion day can be captured consistently.
- **“The Core reward delegate test proves host integration.”** Rejected. `Plan200PersonalQuestsIntegrationTests.RewardBridges_TriggerMoraleAndItemDelegates_OnChoice` assigns delegates directly to a newly constructed Core system. It does not construct the production host session or Main route.

## 12. Root-Cause Clusters

1. **Personal-quest player-route contract is incomplete.** The Core model exposes effects, while host setup omits their owners; the same UI route supplies a placeholder day value. The Core tests exercise the injected Core contract but not the production boundary.
2. **Ending resolver identity uses an unstable input.** This remains isolated from the active Main outcome route until a production consumer is found.

## 13. Cross-System Failure Chains

**Personal quest effect chain:** authored choice (`personal_quests.json`) → panel button → host `ChooseOption` → Core records selected choice and transition → null morale/item callback → save captures resolved choice without authored reward. The UI labels the morale consequence even though the active route does not apply it.

**Quest chronology chain:** panel button passes day `1` → Core writes `resolvedDay = 1` → completed quest save captures it → panel displays the false campaign day after reload.

**Ending identity chain (non-production):** identical ending context → UTC ticks differ → `resolutionId` differs → saved resolver result differs even if narrative fields are otherwise equal.

## 14. Test Coverage Gaps

- The Plan 200 reward test validates Core callbacks after manually assigning both delegates. It does not validate host callback binding, the Main setup route, or actual inventory/morale owners.
- The inspected personal-quest UI route has no focused evidence for passing the campaign day, rendering negative deltas, or applying authored data effects end to end.
- Unified ending tests exercise the resolver directly; that does not establish that the resolver is a production endgame caller.
- No tests were run or added during this audit.

## 15. Migration/Legacy Risks

No Unity or migration defect was confirmed in scope. The uncalled resolver is classified by runtime reachability, not by engine lineage. Current evidence points to a Godot Main outcome route and an engine-free Core utility.

## 16. Save/Determinism Findings

- Personal quest state captures `startedDay`, `resolvedDay`, status, and selected choices. The Day 1 input is therefore durable incorrect state, not a transient label-only error.
- A selected personal-quest choice may be saved without its morale/item effect because the host does not bind those delegates. Existing selected-choice data does not establish whether the side effect ran.
- `UnifiedEndingResolver` stores its result in `lastResult`; its `resolutionId` is wall-clock-derived. This applies to direct utility consumers, not the live endgame route identified in scope.
- No full save round-trip or determinism test was run.

## 17. Recommended Investigation Order

1. Inspect the current ownership claim and real survivor/inventory APIs for the Personal Quest reward bridge.
2. Trace the personal-quest panel’s current-day source and confirm chronology consumers.
3. Review the low-severity negative-delta display with adjacent UI formatting conventions.
4. Confirm the intended runtime status of Plan 145’s `UnifiedEndingResolver`; analyze its deterministic contract if production use is planned.
5. For the requested branch expansion, design around multiple observed actions and persistent outcomes while preserving the current major faction authorities; do not treat the existing morality-band gate as a defect by itself.

## 18. Evidence Index

| Evidence | Current source |
|---|---|
| Core effect delegates and calls | `Assets/Ashfall.Core/Quests/PersonalQuestSystem.cs:99-100,245-251` |
| Host construction without callback binding | `src/Host/PersonalQuestHostSession.cs:26-39` |
| Main host setup and restore | `src/Main.PersonalQuests.cs:25-35` |
| Active UI route | `src/Main.ExpandedShelterSystems.cs:637-639`; `src/UI/PersonalQuestPanel.cs:160-171` |
| Authored quest effects | `Assets/StreamingAssets/Data/personal_quests.json:47-65,112-135` |
| Core-only delegate test | `Ashfall.Core.Tests/Quests/Plan200PersonalQuestsIntegrationTests.cs:156-188` |
| Completion day transition | `Assets/Ashfall.Core/Quests/PersonalQuestSystem.cs:254-260` |
| Completed-quest presentation | `src/UI/PersonalQuestPanel.cs:205-216` |
| Major faction gates | `Assets/Ashfall.Core/Factions/FactionBranchCoordinator.cs:261-330` |
| Narrative arc branch cardinality | `Assets/Ashfall.Core/Quests/NarrativeQuestlineSystem.cs:71-85` |
| Active Main endgame projection | `src/Main.Endgame.cs:158-191`; `Assets/Ashfall.Core/Endgame/CampaignOutcomeEvaluator.cs:60-98` |
| Resolver clock-derived identifier and save | `Assets/Ashfall.Core/Endgame/UnifiedEndingResolver.cs:157-163,431-438` |
| Active supporting-current flags and offers | `Assets/StreamingAssets/Data/currents.json` |

## 19. Audit Confidence

**Overall confidence:** High for static control-flow and source-contract findings; moderate for player impact because no runtime session was run. Active-vs-unreachable classification is based on repository-wide source search in the current working tree. Data validity and full save behavior remain unverified by a validator or execution.

## 20. Audit Completion Statement

All ten audit lenses were completed. Candidates were rechecked against current source, current data, host reachability, save ownership, and test boundaries. Active findings are separated from the uncalled resolver utility and from design limits. No production code or data was changed; no tests or runtime checks were run.
