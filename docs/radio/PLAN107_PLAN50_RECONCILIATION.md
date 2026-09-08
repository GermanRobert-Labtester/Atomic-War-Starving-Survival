# Plan 107 / Plan 50 Reconciliation

**Date:** 2026-09-08
**Decision:** Plan 107's 5 → 20 expansion target is superseded by repository
truth.

## Reconciliation result

The active primary authority,
`Assets/StreamingAssets/Data/radio_distress_signals.json`, contains **25
unique signals**. This is the Plan 50-landed path, not the five-entry planning
baseline.

The repository also contains a separate
`radio_distress_signals_expansion.json` layer with **16 records**:

- 5 IDs intentionally overlap the primary authority;
- 11 IDs are unique NPC-arc or later expansion signals;
- the two JSON layers therefore represent **36 unique signal IDs**.

`RadioDistressSystem` also retains eight built-in compatibility definitions for
sparse fixtures. Four of those IDs are not present in either JSON layer, so a
fully composed runtime has 36 JSON-backed definitions plus 4 compatibility
fallbacks.

## Non-destructive policy

- Do not truncate the primary catalog from 25 to 20.
- Do not re-author the 15 Plan 107 concepts under duplicate IDs.
- Keep the 16-record expansion layer because four records carry live NPC arc
  links (`npc_id` / `resolve_quest_id`).
- In production, load expansion data before the primary JSON authority. Shared
  IDs therefore retain the primary Plan 50 definition, while unique expansion
  records remain available.

## Verified correction

`freq_distress_166_2` declared `days_to_trace: 5` but had only four message
fragments. A fifth final fragment was added so day numbering, fragment count,
and clarity progression agree.

## Plan status

Plan 50 is **implemented and expanded beyond the Plan 107 target**. Plan 107 is
therefore an audit, layering, reference, pacing, and regression pass rather
than a catalog-growth task.
