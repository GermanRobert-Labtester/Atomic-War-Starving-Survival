# Moral Choice Echo Quest Schema & Contracts

**Document ID:** `docs/moral/MORAL_ECHO_SCHEMA.md`
**Referenced Files:**
- `Assets/StreamingAssets/Data/moral_choice_chains.json`
- `Assets/Ashfall.Core/MoralChoice/MoralChoiceChainData.cs`
- `Assets/Ashfall.Core/MoralChoice/MoralChoiceChainCatalogLoader.cs`
- `Assets/Ashfall.Core/MoralChoice/MoralChoiceSystem.cs`

---

## 1. Wire Schema (`moral_choice_chains.json`)

Echo quests are serialized within the top-level container `echo_quests.quests`:

```json
{
  "schema_version": 1,
  "description": "...",
  "branches": [ ... ],
  "merge_rules": { ... },
  "lockout_rules": { ... },
  "quest_gates": [ ... ],
  "echo_quests": {
    "description": "Echo quests fire when a specific earlier quest was resolved a certain way...",
    "quests": [
      {
        "quest_id": "quest_moral_echo_raider_repaid_warning",
        "triggered_by": "quest_moral_chain_mercy_16",
        "triggered_by_choice": 0,
        "min_days_after": 25,
        "branch": "branch_mercy_road"
      }
    ]
  }
}
```

---

## 2. Field Specifications

| Field | Type | Nullable? | Semantics |
|---|---|---|---|
| `quest_id` | `string` | No | Unique quest ID. Must start with canonical prefix `quest_moral_echo_`. |
| `triggered_by` | `string` | No | ID of the source quest whose resolution triggers this callback. Must resolve in moral quest catalogs or quest gates. |
| `triggered_by_choice` | `int` | No | **0-based integer choice index** of the source quest option that must have been selected. Evaluated as `trigger.choiceIndex != echo.TriggeredByChoice`. |
| `min_days_after` | `int` | No | Minimum elapsed campaign days since source quest resolution before this echo becomes eligible (`currentDay >= trigger.resolvedDay + echo.MinDaysAfter`). |
| `branch` | `string?` | Yes | Branch ID (`branch_mercy_road`, `branch_iron_way`, `branch_listener_thread`, `branch_broken_compact`) or `null`. If non-null, suppresses eligibility if `MoralChoiceSystem.IsBranchLocked(branch)` is true. |

---

## 3. Invariants & Rules

1. **0-Based Indexing:** Choices are strictly 0-indexed (`0`, `1`, `2`, `3`). There is no support for `null` wildcard choice triggering in `MoralChoiceSystem`; every echo specifies the exact choice taken.
2. **Branch Lockout Enforcement:** When an opposing branch is locked out by reaching `lock_threshold` entry quest resolutions, any echo tagged with that branch is disqualified from `FindAvailableEchoQuests`.
3. **One-Shot Presentation:** Once an echo quest is presented to the player, calling `MarkEchoQuestFired(echo.QuestId)` registers it in `_state.firedEchoQuests`, preventing duplicates across save/load cycles.
4. **Campaign Temporal Reachability:** All echo delays are bounded such that `sourceQuest.MinDay + echo.MinDaysAfter <= 360` (the standard Year of Ash horizon).
