# Plan 144 — Stub Classification Matrix (10 rows, complete)

Built from exact-ID repository search before any edit. Dispositions use the
Plan 144 §3 vocabulary. Evidence per row: reference sites, definition sites,
runtime loader, branch/chain role, save/flag references, canonical status.

Shared context (applies to rows 2–10):
- Reference sites: `moral_choice_chains.json` (`branches[].entry_quests`,
  `quest_gates[].requires` chains via gates 04+ referencing 03);
  `quest_moral_chain_iron_01` additionally referenced by
  `Ashfall.Core.Tests/MoralChoiceBranchGossipTests.cs`.
- Full-definition sites (canonical): `moral_choice_quests_branching.json`
  (loaded by `MoralChoiceBranchQuestCatalogLoader` → runtime authority).
- Stub site (duplicate, divergent, never loaded):
  `moral_choice_quest_stubs.json`.
- Save/flag references: saves store the ID via
  `MoralChoiceResolution.questId`; stub choices carry no `set_flag`, so no
  flag producers exist in the stub definitions.
- Runtime loader for the stub file: **none** (dead data).

| # | Stub ID | Full-definition site | Branch/chain role | Canonical status | Disposition |
|---|---|---|---|---|---|
| 1 | `quest_moral_merge_` | none — only `merge_rules.merge_quest_prefix` in `moral_choice_chains.json` | merge-prefix grammar token, not a quest | prefix sentinel; runtime never consumes the prefix; the stub's "Ghosts of the Wasteland" quest was never loadable | **replace with prefix validation** — retire the stub row; add `merge_quest_prefix` as a prefix-pattern key in the validator (shape check: trailing `_`, rooted in a known id namespace); do NOT author a quest whose ID is the prefix string (Plan §4.2/§12) |
| 2 | `quest_moral_chain_betray_01` | `moral_choice_quests_branching.json` ("The Empty Promise") | `branch_broken_compact` entry quest #1 | canonical, runtime-loaded | **retire duplicate** — stub row (divergent "A Knife in the Dark") deleted; branching remains sole executable definition |
| 3 | `quest_moral_chain_betray_02` | branching ("The Whispered Cache") | broken-compact entry #2 | canonical | **retire duplicate** |
| 4 | `quest_moral_chain_betray_03` | branching ("Stolen Credit") | broken-compact entry #3; gates betray_04 chain from it | canonical | **retire duplicate** |
| 5 | `quest_moral_chain_iron_01` | branching ("The Weight of Filters") | `branch_iron_way` entry #1; referenced in `MoralChoiceBranchGossipTests` | canonical | **retire duplicate** |
| 6 | `quest_moral_chain_iron_02` | branching ("What the Dying Know") | iron-way entry #2 | canonical | **retire duplicate** |
| 7 | `quest_moral_chain_iron_03` | branching ("The Price of Empty Hands") | iron-way entry #3; gate iron_04 requires it | canonical | **retire duplicate** |
| 8 | `quest_moral_chain_listen_01` | branching ("The Weight of a Name") | `branch_listener_thread` entry #1 | canonical | **retire duplicate** |
| 9 | `quest_moral_chain_listen_02` | branching ("Before the Ash") | listener entry #2 | canonical | **retire duplicate** |
| 10 | `quest_moral_chain_listen_03` | branching ("What the Water Knows") | listener entry #3; gate listen_04 requires it | canonical | **retire duplicate** |

## Promotion analysis (Workstream 144E)

No genuinely referenced quest lacks an executable definition:
- All 12 chain entry quests (4 branches × 3) resolve to full definitions in
  `moral_choice_quests_branching.json`.
- All `quest_gates[].quest_id` values (quests 04–25 per branch) resolve in
  the same catalog.
- No quest catalog contains any prefix-matching merge quest, and no chain
  reference points at a merge quest ID.

**Zero promotions.** `quest_moral_merge_` is a prefix, not a quest; Plan
144 explicitly forbids inventing content for it. The orphaned "Ghosts of the
Wasteland" authorship in the stub file was never runtime-loadable; it is
retired with the file. If merge mechanics are implemented later, a merge
quest must be authored under a proper prefix-matching ID (e.g.
`quest_moral_merge_<name>`) in a canonical playable catalog — not as the
bare prefix.

## Stub file disposition

All ten rows resolve to retire/replace. No justified sentinel remains →
**the entire `moral_choice_quest_stubs.json` file is deleted** once the
validator validates `merge_quest_prefix` as a prefix pattern and the
duplicate-executable-definition gate exists to prevent regression.
