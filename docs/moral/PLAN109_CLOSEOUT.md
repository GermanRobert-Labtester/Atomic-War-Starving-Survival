# Plan 109 — Moral Choice Echo Quests Expansion: Closeout Report

**Document ID:** `docs/moral/PLAN109_CLOSEOUT.md`
**Execution Scope:** Expand `echo_quests.quests` in `moral_choice_chains.json` from 32 to 60 delayed callback quests.
**Catalog Authority:** `Assets/StreamingAssets/Data/moral_choice_chains.json` (mirrored to `builds/linux/Assets/StreamingAssets/Data/moral_choice_chains.json`)
**Core Classes:** `Assets/Ashfall.Core/MoralChoice/MoralChoiceChainData.cs`, `Assets/Ashfall.Core/MoralChoice/MoralChoiceSystem.cs`, `Assets/Ashfall.Core/MoralChoice/MoralChoiceChainCatalogLoader.cs`
**Test Suite:** `Ashfall.Core.Tests/MoralChoiceEchoExpansionTests.cs` (16/16 PASS), `Ashfall.Core.Tests/MoralChoiceBranchGossipTests.cs` (36/36 PASS)
**Full Test Suite:** `dotnet test Ashfall.Core.Tests` (9,868/9,868 PASS)
**Status:** **COMPLETE & FULLY VERIFIED**

---

## 1. Executive Summary

Plan 109 expands the delayed callback layer of ASHFALL’s moral choice system from 32 to exactly 60 quests. The expansion addresses the ratio between the 88 gated branching moral choices and their long-horizon callbacks, allowing decisions made during early survival to echo weeks and months later into the campaign.

All 32 baseline echoes are preserved verbatim. Exactly 28 new callback quests were authored and verified against the canonical 4-branch architecture:
- **Mercy Road:** 8 new echoes (11 total)
- **Iron Way:** 8 new echoes (11 total)
- **Listener Thread:** 7 new echoes (10 total)
- **Broken Compact:** 5 new echoes (8 total)
- **Branch-Agnostic / Base:** 20 preserved (20 total)

Every new echo references a unique gated moral quest (`quest_gates`), enforces strict 0-based choice index matching against authored options in `moral_choice_quests_branching.json`, and operates within a calibrated delay window (25–50 days) ensuring full reachability within the standard 360-day Year of Ash campaign.

---

## 2. Technical Contracts & Invariants

1. **Pure Data-First Discipline:** Zero changes made to `MoralChoiceSystem.cs` or Core runtime logic. The existing deserializer, eligibility scanner, and branch lockout systems were verified to natively support the expanded catalog.
2. **0-Based Choice Indexing:** Verified that `triggered_by_choice` operates on exact 0-based integer indexes. All 60 entries match valid choices within source quest option bounds.
3. **Branch Lockout Consistency:** All 28 new echoes specify their source branch. If an opposing branch locks out that branch at `lock_threshold: 3`, the echo is suppressed by `MoralChoiceSystem.FindAvailableEchoQuests`.
4. **Export Parity:** `builds/linux/Assets/StreamingAssets/Data/moral_choice_chains.json` was updated in lockstep with the source catalog and verified via byte/hash comparison.

---

## 3. Downstream Narrative Handoffs

- **Journal Voice (Plan 95):** 6 echoes provide rich personal and community reflection opportunities:
  - `quest_moral_echo_betrayers_child_grown` (Mercy)
  - `quest_moral_echo_patrol_reputation_spread` (Mercy)
  - `quest_moral_echo_old_friend_farewell_note` (Iron)
  - `quest_moral_echo_doctors_notes_reinterpreted` (Listener)
  - `quest_moral_echo_librarian_memorial_preserved` (Listener)
  - `quest_moral_echo_kessler_exile_uncovered` (Broken Compact)
- **Confessions (Plan 88):** 4 echoes open deep interpersonal confession opportunities:
  - `quest_moral_echo_soldier_second_confession` (Listener — soldier confesses bridge convoy sabotage)
  - `quest_moral_echo_defector_corroborates_truth` (Listener — defector confesses past atrocities)
  - `quest_moral_echo_plague_secret_infection` (Mercy — healer confesses concealment burden)
  - `quest_moral_echo_voss_blackmail_exposed` (Broken Compact — council member confesses compromised ethics)
- **Endings & Epilogues (Plan 89):** 3 echoes feed directly into ending predicates:
  - `quest_moral_echo_convoy_haven_opened` / `aldric_blockade_retaliation` (Mercy/Iron contrast — valley cohesion vs fortified faction hostility)
  - `quest_moral_echo_cartographer_water_cache_located` (Listener — generational aquifer water security)
  - `quest_moral_echo_pell_hostage_border_locked` (Broken Compact — permanently sealed southern borders)

---

## 4. Verification Matrix Results

| Check | Tool / Command | Result |
|---|---|---|
| C# Assembly Build | `dotnet build Ashfall.csproj` | **0 errors, 0 warnings** |
| Moral Choice Expansion Suite | `dotnet test Ashfall.Core.Tests --filter MoralChoiceEchoExpansionTests` | **16 passed, 0 failed** |
| Moral Choice Branch Gossip Suite | `dotnet test Ashfall.Core.Tests --filter MoralChoiceBranchGossipTests` | **36 passed, 0 failed** |
| All Moral Choice Tests | `dotnet test Ashfall.Core.Tests --filter MoralChoice` | **144 passed, 0 failed** |
| Full xUnit Regression Suite | `dotnet test Ashfall.Core.Tests` | **9,868 passed, 0 failed** |
| Content Utilization Self-Test | `godot --headless --path . -- --content-utilization-selftest` | **CI Gate: PASS** |
| Data Integrity Self-Test | `godot --headless --path . -- --data-integrity-selftest` | **0 findings across 298 catalogs (+28 IDs authored)** |
| Scene Binding Self-Test | `godot --headless --path . -- --scene-binding-selftest` | **25/25 passed** |
| Production Scene Lint | `python3 scripts/ci/scene-lint.py` | **30 scenes, 0 errors, 0 warnings** |
