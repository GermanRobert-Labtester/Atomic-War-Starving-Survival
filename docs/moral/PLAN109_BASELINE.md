# Plan 109 — Moral Choice Echo Quests Expansion: Baseline Reconnaissance

**Document ID:** `docs/moral/PLAN109_BASELINE.md`
**Initiative:** Plan 109 — Moral Choice Echo Quests Expansion (32 → 60 Echo Quests)
**Catalog Authority:** `Assets/StreamingAssets/Data/moral_choice_chains.json` (mirrored to `builds/linux/Assets/StreamingAssets/Data/moral_choice_chains.json`)
**Core Classes:** `Assets/Ashfall.Core/MoralChoice/MoralChoiceChainData.cs`, `Assets/Ashfall.Core/MoralChoice/MoralChoiceSystem.cs`, `Assets/Ashfall.Core/MoralChoice/MoralChoiceChainCatalogLoader.cs`
**Test Suites:** `Ashfall.Core.Tests/MoralChoiceEchoExpansionTests.cs`, `Ashfall.Core.Tests/MoralChoiceBranchGossipTests.cs`

---

## 1. Verified Architecture & Baseline Findings

Prior to expanding the catalog, repository reconnaissance established the following invariants:

1. **Catalog Topology**:
   - `moral_choice_chains.json` contains:
     - 4 permanent branches (`branch_mercy_road`, `branch_iron_way`, `branch_listener_thread`, `branch_broken_compact`).
     - Merge rules and lockout rules (`lock_threshold: 3`, permanent lockouts with journal templates).
     - 88 quest gates (`quest_moral_chain_mercy_04`–`25`, `iron_04`–`25`, `listen_04`–`25`, `betray_04`–`25`).
     - 32 baseline echo quests under `echo_quests.quests`.
2. **Echo DTO Contract (`MoralChoiceChainData.cs` / `MoralChoiceChainCatalogLoader.cs`)**:
   - `quest_id`: Non-empty string prefixed with canonical `quest_moral_echo_`.
   - `triggered_by`: Non-empty string referencing a valid moral choice quest ID.
   - `triggered_by_choice`: Primitive `int` (0-based indexing), strictly matching `trigger.choiceIndex`.
   - `min_days_after`: Positive integer representing the minimum campaign days required between source resolution and echo availability.
   - `branch`: Nullable string. `null` or empty string indicates branch-agnostic eligibility; non-null string binds eligibility to that branch not being locked out by `IsBranchLocked(echo.Branch)`.
3. **Runtime Eligibility & Firing (`MoralChoiceSystem.cs:375-400`)**:
   - `FindAvailableEchoQuests(int currentDay)` iterates through `_chainData.EchoQuests`.
   - Rejects if `_state.firedEchoQuests.Contains(echo.QuestId)`.
   - Rejects if `!TryGetResolution(echo.TriggeredBy, out var trigger)`.
   - Rejects if `trigger.choiceIndex != echo.TriggeredByChoice`.
   - Rejects if `currentDay < trigger.resolvedDay + echo.MinDaysAfter`.
   - Rejects if `!string.IsNullOrEmpty(echo.Branch) && IsBranchLocked(echo.Branch)`.
   - `MarkEchoQuestFired(echoQuestId)` records the echo ID into `_state.firedEchoQuests` to guarantee one-shot presentation.

---

## 2. Baseline 32 Echo Inventory

The 32 pre-existing echoes consisted of:
- **20 Base/Agnostic Echoes** (`branch: null`), triggered by standalone moral quests (`quest_moral_share_child`, `quest_moral_share_family`, `quest_moral_comfort_widow`, etc.).
- **12 Chain Echoes** (3 per branch):
  - Mercy Road: `quest_moral_chain_mercy_05`, `10`, `20`
  - Iron Way: `quest_moral_chain_iron_05`, `10`, `20`
  - Listener Thread: `quest_moral_chain_listen_05`, `10`, `20`
  - Broken Compact: `quest_moral_chain_betray_05`, `10`, `20`

---

## 3. Expansion Target & Distribution

To reach exactly 60 echo quests without source duplication:
- **Mercy Road:** +8 new echoes (total: 11)
- **Iron Way:** +8 new echoes (total: 11)
- **Listener Thread:** +7 new echoes (total: 10)
- **Broken Compact:** +5 new echoes (total: 8)
- **Branch-Agnostic Baseline:** 20 preserved
- **Total Catalog Size:** 32 baseline + 28 new = **60 echo quests**.
