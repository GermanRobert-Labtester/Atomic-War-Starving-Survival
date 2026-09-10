# Plan 144 — Reference Graph (stub era → post-cleanup)

## Pre-cleanup graph (anomalies marked)

```
moral_choice_chains.json
  branches[].entry_quests ──────────────► quest_moral_chain_{mercy|iron|listen|betray}_{01..03}
  quest_gates[].quest_id ──────────────► quest_moral_chain_*_04..25   (chains from _03)
  quest_gates[].requires ───────────────► previous quest in chain
  merge_rules.merge_quest_prefix = "quest_moral_merge_"   ◄── PREFIX TOKEN
  branches[].locked_flag ──────────────► flag_branch_*_locked (moral_choice_flags.json)

Runtime quest sources (MoralChoiceSystem):
  moral_choice_quests.json            (65)   ─┐
  moral_choice_quests_distress.json   (6)    ─┼─► playable quest set
  moral_choice_quests_branching.json  (100)  ─┤    (branching = sole authority
  moral_choice_quests_expansion.json  (50)   ─┘     for chain ids)

DEAD / DUPLICATE (never loaded):
  moral_choice_quest_stubs.json (10)
      quest_moral_chain_betray_01..03 ── DIVERGENT duplicates of branching defs
      quest_moral_chain_iron_01..03   ── DIVERGENT duplicates
      quest_moral_chain_listen_01..03 ── DIVERGENT duplicates
      quest_moral_merge_             ── dead quest authored under a prefix token

Integrity validator:
  Tier-1 saw "quest_moral_merge_" (prefix) as a concrete id → resolved ONLY
  because the stub file registered it as a definition. Stub file sorted
  (ordinal) before the branching file, so it was first author of the 9
  chain ids; the real branching definitions were counted as "reuse".

Identity registry:
  questline_master.json (504 entries) acknowledges ids from all quest
  catalogs (registry-only; no executable fields).
```

## Post-cleanup graph (target state)

```
moral_choice_chains.json
  entry_quests / quest_gates ──► moral_choice_quests_branching.json (sole executable defs)
  merge_quest_prefix = "quest_moral_merge_"  ──► PREFIX-PATTERN validation
        (shape: trailing '_', rooted in a known id namespace; never a quest fk)
  locked_flag ──► moral_choice_flags.json

moral_choice_quest_stubs.json  ── DELETED (no justified rows remained)

Validator:
  + duplicate-executable-quest-definition gate (cross-file, choices/stages/
    objectives/steps marker; registry-only acknowledgement allowed)
  + prefix-pattern key contract for merge_quest_prefix

Saves: quest ids only (MoralChoiceResolution.questId) → unchanged resolution
against the same canonical definitions the runtime always used.
```

## Reachability note

All four branches' entry quests (_01) have `min_day: 1..5` in the branching
catalog and no prerequisites — branch progression starts from day one.
Gates 04+ require the _03 quest and branch-moral thresholds exactly as
authored in `quest_gates`. Nothing in the cleanup alters these values;
branch simulation tests pin them.
