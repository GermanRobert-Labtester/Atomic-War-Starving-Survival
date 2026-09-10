# Plan 144 — Baseline Reconnaissance (2026-09-06)

Mission: eliminate ambiguity around `moral_choice_quest_stubs.json` — prove
which of its ten IDs are backed by canonical quests, which are prefix
sentinels, and whether any true unresolved quest reference remains.

## 1. Exact stub inventory

`Assets/StreamingAssets/Data/moral_choice_quest_stubs.json` — `schema_version: 1`,
self-described as *"Fully fleshed out moral choice chains and merge quest."*
**Contrary to the plan's premise ("ID-only records"), the file contains ten
COMPLETE quest definitions** (display_name, trigger, discovery, category,
min_day, choices with moral_delta/empathy_delta/outcome_text/epitaph).
No `set_flag` on any choice (0 occurrences) — the stub content produces no
flags, so it never participates in flag production.

Ten IDs (exact):
1. `quest_moral_merge_` — "Ghosts of the Wasteland" (min_day 100)
2. `quest_moral_chain_betray_01` — "A Knife in the Dark"
3. `quest_moral_chain_betray_02` — "The Viper's Nest"
4. `quest_moral_chain_betray_03` — "Judgment of the Traitor"
5. `quest_moral_chain_iron_01` — "The Triage Doctrine"
6. `quest_moral_chain_iron_02` — "Culling the Weak"
7. `quest_moral_chain_iron_03` — "The Final Equation"
8. `quest_moral_chain_listen_01` — "Whispers in the Static"
9. `quest_moral_chain_listen_02` — "The Dying Breath"
10. `quest_moral_chain_listen_03` — "Out of the Mouths of Babes"

## 2. Runtime loader status — the stub file is DEAD at runtime

Repository-wide search (`Assets`, `src`, `Ashfall.Core.Tests`, `scripts`,
*.cs + *.json): **no C# runtime loader references
`moral_choice_quest_stubs.json`.** The only code references are four
classification entries in `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs`
(lines 64, 590, 728, 1094) which *claim* consumers (`MoralChoiceSystem`,
`MoralChoicePanel`) that never load it.

Actual runtime quest composition for MoralChoiceSystem (`src/Main.MoralChoice.cs`):
- `MoralChoiceCatalogLoader.Load` → `moral_choice_quests.json` (65 quests)
- `MoralChoiceCatalogLoader.LoadFrom(..., "moral_choice_quests_distress.json")` (6)
- `MoralChoiceBranchQuestCatalogLoader.Load` → `moral_choice_quests_branching.json` (100)
- `MoralChoiceExpansionQuestCatalogLoader` → `moral_choice_quests_expansion.json` (50)

Historical note: `MoralChoiceCatalogLoader.LoadStubs()` (used by 5 test files)
is a **misleading legacy name** — it loads base + distress only, never the
stub file.

## 3. Duplicate definition status — all 9 chain IDs are DIVERGENT duplicates

All nine `quest_moral_chain_*` IDs exist as **fully divergent definitions** in
`moral_choice_quests_branching.json` (the runtime authority). Same stable ID,
entirely different quests:

| ID | Stub version | Branching (runtime) version |
|---|---|---|
| betray_01 | "A Knife in the Dark" (armory theft/Elias) | "The Empty Promise" (medicine promise/Maren) |
| betray_02 | "The Viper's Nest" | "The Whispered Cache" |
| betray_03 | "Judgment of the Traitor" | "Stolen Credit" |
| iron_01 | "The Triage Doctrine" | "The Weight of Filters" |
| iron_02 | "Culling the Weak" | "What the Dying Know" |
| iron_03 | "The Final Equation" | "The Price of Empty Hands" |
| listen_01 | "Whispers in the Static" | "The Weight of a Name" |
| listen_02 | "The Dying Breath" | "Before the Ash" |
| listen_03 | "Out of the Mouths of Babes" | "What the Water Knows" |

Categories, triggers, discoveries, locations, min_days and all four choices
per quest diverge. This is the Workstream 144D "same ID with divergent
outcomes" condition — currently invisible because the validator counts
cross-file duplicate definitions as legitimate "reuse".

## 4. The merge-prefix sentinel

`moral_choice_chains.json` → `merge_rules.merge_quest_prefix = "quest_moral_merge_"`.
`MergeQuestPrefix` is loaded into `MoralChoiceChainData` by
`MoralChoiceChainCatalogLoader` but is **consumed by no runtime logic**
(no merge-quest discovery, no `merge_from` markers in any quest catalog,
no merge eligibility code in `MoralChoiceSystem` or `MoralBranchingSystem`).
No quest in any canonical catalog matches the prefix. The prefix string
appears as a quest **only in the stub file** — as a full quest named
"Ghosts of the Wasteland" that runtime never loads.

## 5. Integrity validator baseline + gap reproduction

Baseline: `godot --headless -- --data-integrity-selftest` →
**PASS, 300 catalogs, 0 errors** ("12508 ids authored, 4554 reuses").

Gap reproduction (stub file temporarily removed):
```
[DATA] unresolved id 'quest_moral_merge_' at moral_choice_chains.json/merge_rules/merge_quest_prefix
DATA_INTEGRITY_SELFTEST FAIL (1) — 299 catalogs
```

**Proven root cause:** the stub file exists solely to satisfy Tier-1
(a `quest_`-prefixed string in a non-id position must resolve) on the prefix
string `quest_moral_merge_`. All nine chain IDs resolve fine against the
branching catalog without the stub file. The validator treats a prefix
grammar token as a concrete quest foreign key — the exact defect Plan 144
Workstream 144B/144C targets.

Ordinal file scan order puts `moral_choice_quest_stubs.json` BEFORE
`moral_choice_quests_branching.json` ('_' 0x5F < 's' 0x73), so the stub file
was the *first author* of the nine chain IDs; the branching catalog's real
definitions were counted as "reuses".

## 6. Cross-file quest-ID duplicate sweep (all 411 data JSONs)

289 quest IDs are defined in more than one file:
- 280 = (some catalog) + `questline_master.json` — the identity-only master
  registry (504 entries; union of entry keys contains no
  `choices`/`stages`/`objectives`/`steps`).
- 9 = the moral chain IDs in (branching + stubs) — the divergent duplicates.

## 7. Save state — stable IDs only

`MoralChoiceState` (`Assets/Ashfall.Core/MoralChoiceState.cs`) persists
`MoralChoiceResolution.questId` (string), branchProgress by branch id,
lockedBranches by branch id, firedEchoQuests and activeFlags by id.
**No field records which source file supplied a quest.** Saves round-trip
against catalog contents resolved by stable ID at load time.

## 8. Chain/branch runtime contract

`MoralChoiceSystem.InitializeChainData` maps `entry_quests` and
`quest_gates[].quest_id` → branch; `IsChainQuestAccessible` enforces branch
locks + gates; `TrackBranchProgress` counts entry-quest resolutions and locks
opposing branches at `lock_threshold` (3). All four branches
(mercy/iron/listener/broken-compact) reference exactly the entry IDs
_01.._03, which are defined in the branching catalog.

## 9. Tests/selftests touching the area

- No test asserts stub-file existence, stub count, or loads it.
- `Ashfall.Core.Tests/Radio/*` + `MoralChoiceFlagPlan125Tests` use
  `LoadStubs()` = base + distress (unaffected).
- `ContentUtilizationGraphTests` uses synthetic catalogs (unaffected).
- Host verbs: `--moral-choice-selftest` exists (catalog + scripted arc +
  bands + reconcile events + journal hook + save/tamper checks).

## 10. Git provenance

Stub file introduced in `078f6128 content(data): data-authority batch —
expansion catalogs, narrative corpus, schema sweeps`; touched later by
content batches. Never wired to a loader.
