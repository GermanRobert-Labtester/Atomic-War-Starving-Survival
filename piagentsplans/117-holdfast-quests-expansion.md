# Plan 117 — Holdfast Quests Expansion (10 → 20 quests)

## Goal (2 lines)
Expand `holdfast_quests.json` from 10 quests to 20. The Holdfast expansion's
quest catalog (`HoldfastCatalog.cs` confirmed live; runtime
`Cluster12CHeadlessDemo.cs`) drives ice-road and estuary expeditions with
briefings, knowledge keys, target locations, staged narratives, and choices.
10 quests for the Holdfast's frozen-coast pillar is thin; the ice road,
lamplighter, and estuary trade themes need more cases.

## Why (P2)
- Verified: `holdfast_quests.json` has 10 quests. Each has id, display_name,
  type, briefing, prereq_quest_id, min_day, knowledge_key,
  target_location_id, stages (array of {id, text}), choices.
  `HoldfastCatalog.cs` loads it.
- The Holdfast expansion is the ice-road and estuary pillar. 10 quests means
  the frozen-coast expedition arc is short. The min_day range (90+) has room
  for 10 more quests without overlap, and the knowledge_key system allows
  quest chains that unlock lore.
- Pure DATA work — zero new Core code.

## Files to touch
- `Assets/StreamingAssets/Data/holdfast_quests.json` (expand `quests` 10 → 20)
- Read-only: `Assets/Ashfall.Core/HoldfastCatalog.cs` (confirm quest/stage/
  choice DTO and required fields)
- Read-only: `Assets/Ashfall.Core/Cluster12CHeadlessDemo.cs` (confirm runtime
  consumption and save compatibility)

## Content grammar (per quest)
- `id`: snake_case, prefix `quest_holdfast_` (confirmed convention).
- `display_name`: evocative quest title ("The Sheet That Shouldn't").
- `type`: quest type string (confirm valid set in step 1 — "expedition",
  "investigation", etc.).
- `briefing`: 2–4 sentences in the established Holdfast voice (cold,
  estuary-salt, ice-road prose).
- `prereq_quest_id`: a quest id that must be completed first, or "" for
  none (must resolve if non-empty).
- `min_day`: integer day the quest becomes available.
- `knowledge_key`: a lore key the quest unlocks, or "" (must resolve if
  non-empty, against the knowledge catalog).
- `target_location_id`: a location id the expedition targets (must resolve).
- `stages`: array of {id, text} — the narrative beats.
- `choices`: array of choice objects (confirm choice DTO in step 1).

## Steps
1. Read `HoldfastCatalog.cs` to confirm the quest/stage/choice DTO and all
   required vs optional fields, and the valid `type` values.
2. Read `Cluster12CHeadlessDemo.cs` to confirm runtime consumption and that
   new quest ids are additive (save-safe).
3. Inventory the 10 existing quests: type distribution, min_day range,
   target_location coverage. Identify which Holdfast locations are
   underused.
4. Author 10 new quests:
   - `quest_holdfast_the_lamplighters_debt`: a lamplighter owes fuel to the
     estuary camp; the player collects or covers the debt.
   - `quest_holdfast_the_kittiwake_log`: a launch log contradicts the ice
     road map; the player reconciles the two records.
   - `quest_holdfast_the_frozen_barge`: a barge is frozen mid-channel with
     cargo the camp needs; the player breaks it free or salvages from the
     ice.
   - `quest_holdfast_the_salted_census`: a census of the estuary camp
     doesn't match the headcount; someone is missing or someone is extra.
   - `quest_holdfast_the_lamp_oil_trade`: the camp needs lamp oil; the
     player trades with a distant holdfast or synthesizes from salvage.
   - `quest_holdfast_the_breakup_watch`: spring breakup threatens the ice
     road; the player warns the camp or exploits the chaos.
   - `quest_holdfast_the_drowned_cache`: a pre-war cache is visible at low
     tide; the player times the salvage against the tide and the cold.
   - `quest_holdfast_the_quarantine_post`: a Holdfast quarantine post
     blocks the road; the player negotiates, bribes, or detours.
   - `quest_holdfast_the_estuary_wreck`: a wrecked vessel holds fuel and
     maps; the player salvages before a rival camp does.
   - `quest_holdfast_the_last_lamp`: the last Sector 4 lamp is failing; the
     player repairs it or lets the road go dark.
5. Each quest: 3–6 stages, 2–4 choices, distinct target_location, a
   briefing in the established cold-estuary voice.
6. Cross-reference: every id unique; every prereq_quest_id resolves;
  every knowledge_key resolves (if non-empty); every target_location_id
  resolves.
7. Wire 3 new quests to Plan 76 (expedition destinations — quests target
   new locations).
8. Wire 2 new quests to Plan 80 (library manuals — quests unlock manuals).
9. Wire 2 new quests to Plan 84 (muster witnesses — quests involve
   witnesses).
10. Validate: `--data-integrity-selftest` (all refs resolve).
11. xUnit: Holdfast quest catalog loads 20 quests, all ids unique, all
    prereq/knowledge/location refs resolving.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
MEDIUM — the choice DTO must be confirmed (step 1) before authoring. The
Holdfast voice is distinctive (cold, salt, ice); briefing prose must match
the existing quality bar.

## Definition of Done
- `holdfast_quests.json` has 20 quests, all ids unique, all refs resolving,
  3 wired to expedition destinations, 2 to library manuals, 2 to muster
  witnesses, integrity + tests green.

## Follow-on
- Plan 76 (expedition destinations) — quests target new locations.
- Plan 80 (library manuals) — quests unlock manuals.
- Plan 84 (muster witnesses) — quests involve witnesses.
- Plan 89 (epilogues) — Holdfast outcomes feed endings.
- Plan 109 (echo quests) — Holdfast resolutions may trigger echoes.
