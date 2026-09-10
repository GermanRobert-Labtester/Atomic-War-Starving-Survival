# Plan 144 — Quest Authority Map (moral-choice domain)

Which file owns **identity** versus **executable quest definition**, and
which loader consumes each file.

| File | Role | Loader | Identity? | Executable? |
|---|---|---|---|---|
| `moral_choice_quests.json` | base playable moral quests (65) | `MoralChoiceCatalogLoader.Load` | yes (own ids) | **yes** |
| `moral_choice_quests_branching.json` | branch chain playable quests (100: 4 branches × 25) — **sole executable authority for all `quest_moral_chain_*` ids** | `MoralChoiceBranchQuestCatalogLoader.Load` | yes | **yes** |
| `moral_choice_quests_expansion.json` | expansion playable quests (50) | `MoralChoiceExpansionQuestCatalogLoader` | yes | **yes** |
| `moral_choice_quests_distress.json` | radio-distress playable quests (6) | `MoralChoiceCatalogLoader.LoadFrom(...)` (also via `LoadStubs()` in tests) | yes | **yes** |
| `moral_choice_chains.json` | branch/chain **rules** — branch ids, entry_quests, quest_gates, merge_rules (incl. `merge_quest_prefix`), lockout rules | `MoralChoiceChainCatalogLoader.Load` → `MoralChoiceSystem.InitializeChainData` | no (references quests; registers nothing) | no (rules only) |
| `moral_choice_flags.json` | persistent moral flag definitions | `MoralChoiceFlagCatalogLoader` | yes (flag_ ids) | no |
| `moral_choice_gossip.json` / `moral_choice_faction_reactions.json` | downstream consequences | their loaders → gossip/faction runtime | reference | no |
| `questline_master.json` | **identity-only master registry** for questline ids across the whole game (504 entries; no choices/stages/objectives/steps) | `QuestlineMasterCatalog` (scanner-registered; presentation/metadata) | yes (registry) | **no** |
| `moral_choice_quest_stubs.json` | former integrity shim: full-but-divergent duplicates of 9 chain quests + a dead merge quest authored under a prefix token. **Retired by Plan 144.** | none — never loaded | — | — (dead) |

## Ownership rules established by Plan 144

1. **Executable definition**: exactly one file per quest id. For
   `quest_moral_chain_*` that file is `moral_choice_quests_branching.json`.
2. **Identity registration**: `questline_master.json` may acknowledge any
   quest id without creating a second definition (validator rule:
   registry-only entries never contain `choices`/`stages`/`objectives`/`steps`).
3. **Chain rules** (`moral_choice_chains.json`) reference quests; they never
   define them.
4. **Prefix grammar tokens** (e.g. `merge_quest_prefix`) are validated as
   prefix patterns (trailing `_`, rooted in a known id namespace), not as
   quest foreign keys.
5. Duplicate **executable** definitions across two playable catalogs are a
   blocking integrity error (new validator gate).
