# Plan 118 — Standing Record Quests Expansion (10 → 20 quests)

## Goal (2 lines)
Expand `standing_record_quests.json` from 10 quests to 20. The Standing
Record expansion's quest catalog (`StandingRecordCatalog.cs` confirmed live;
runtime `StandingRecordEngine.cs`) drives sector-lamp and survey-nail
expeditions with briefings, knowledge keys, target locations, staged
narratives, choices, and complete/fail mutations. 10 quests for the
Standing Record's territorial-record pillar is thin.

## Why (P2)
- Verified: `standing_record_quests.json` has 10 quests. Each has id,
  display_name, type, briefing, prereq_quest_id, min_day, knowledge_key,
  target_location_id, complete_mutation, fail_mutation, stages (array of
  {id, text}), choices. `StandingRecordCatalog.cs` loads it;
  `StandingRecordEngine.cs` runs the runtime.
- The Standing Record expansion is the territorial-record and sector-lamp
  pillar. 10 quests means the survey-nail and plate-stencil arc is short.
  The min_day range (75+) has room for 10 more quests, and the
  complete_mutation/fail_mutation system allows quests to permanently alter
  the world state.
- Pure DATA work — zero new Core code.

## Files to touch
- `Assets/StreamingAssets/Data/standing_record_quests.json` (expand `quests`
  10 → 20)
- Read-only: `Assets/Ashfall.Core/StandingRecord/StandingRecordCatalog.cs`
  (confirm quest/stage/choice DTO and required fields)
- Read-only: `Assets/Ashfall.Core/StandingRecord/StandingRecordEngine.cs`
  (confirm how complete_mutation/fail_mutation resolve)

## Content grammar (per quest)
- `id`: snake_case, prefix `quest_record_` (confirmed convention).
- `display_name`: evocative quest title ("The Plate on the Last Lamp").
- `type`: quest type string (confirm valid set in step 1).
- `briefing`: 2–4 sentences in the established Standing Record voice
  (survey-nail, plate-vs-stencil, sector-lamp prose).
- `prereq_quest_id`: a quest id that must be completed first, or "" (must
  resolve if non-empty).
- `min_day`: integer day the quest becomes available.
- `knowledge_key`: a lore key, or "" (must resolve if non-empty).
- `target_location_id`: a location id (must resolve).
- `complete_mutation`: a mutation id applied on quest completion (must
  resolve against the mutation catalog — confirm in step 2).
- `fail_mutation`: a mutation id applied on quest failure (must resolve).
- `stages`: array of {id, text} — the narrative beats.
- `choices`: array of choice objects (confirm choice DTO in step 1).

## Steps
1. Read `StandingRecordCatalog.cs` to confirm the quest/stage/choice DTO and
   all required vs optional fields, and the valid `type` values.
2. Read `StandingRecordEngine.cs` to confirm how `complete_mutation` and
   `fail_mutation` resolve (against which catalog) and whether they are
   required or optional.
3. Inventory the 10 existing quests: type distribution, min_day range,
   mutation coverage. Identify which Standing Record locations are
   underused.
4. Author 10 new quests:
   - `quest_record_the_survey_nail`: a survey nail marks a contested
     boundary; the player reads it, moves it, or ignores it, with
     territorial consequences.
   - `quest_record_the_overlay_pigment`: overlay pigment in a lamp crate
     suggests the plate was added after the stencil; the player traces the
     forgery.
   - `quest_record_the_second_count`: a second count of the sector lamps
     disagrees with the first; the player reconciles or exposes the
     discrepancy.
   - `quest_record_the_lamp_keepers_oath`: a lamp keeper's oath requires
     relighting a dead lamp; the player provides fuel or lets it stay dark.
   - `quest_record_the_boundary_dispute`: two factions claim a boundary
     marked by a survey nail; the player arbitrates or takes a side.
   - `quest_record_the_missing_plate`: a brass plate is absent from a lamp
     that should have one; the player finds it or records the loss.
   - `quest_record_the_cold_survey`: a winter survey of the sector lamps
     requires cold-weather gear; the player prepares and executes or fails
     to the cold.
   - `quest_record_the_lamp_oil_ledger`: a lamp oil ledger doesn't match
     the actual oil stores; the player investigates the discrepancy.
   - `quest_record_the_rejected_survey`: a survey was rejected and the
     rejection stamp is still visible; the player reads the reason and
     decides whether to resubmit.
   - `quest_record_the_last_sector`: the last sector has no lamps at all;
     the player surveys it and decides whether to light it or leave it
     dark.
5. Each quest: 3–6 stages, 2–4 choices, distinct target_location,
   complete_mutation and fail_mutation that resolve, a briefing in the
   established survey-nail voice.
6. Cross-reference: every id unique; every prereq_quest_id resolves;
   every knowledge_key resolves; every target_location_id resolves;
   every complete_mutation and fail_mutation resolves.
7. Wire 3 new quests to Plan 98 (standing record factions — quests shift
   faction standing).
8. Wire 2 new quests to Plan 76 (expedition destinations — quests target
   new locations).
9. Wire 2 new quests to Plan 82 (Verdict locations — quests reference
   investigation sites).
10. Validate: `--data-integrity-selftest` (all refs resolve).
11. xUnit: Standing Record quest catalog loads 20 quests, all ids unique,
    all refs (prereq, knowledge, location, mutation) resolving.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
MEDIUM — the `complete_mutation`/`fail_mutation` fields are unique to this
catalog and must resolve against the mutation catalog (step 2). If the
mutation catalog is thin, some quests may need mutations authored first or
left empty (confirm whether empty is valid).

## Definition of Done
- `standing_record_quests.json` has 20 quests, all ids unique, all refs
  (prereq, knowledge, location, mutation) resolving, 3 wired to standing
  record factions, 2 to expedition destinations, 2 to Verdict locations,
  integrity + tests green.

## Follow-on
- Plan 98 (standing record factions) — quests shift faction standing.
- Plan 76 (expedition destinations) — quests target new locations.
- Plan 82 (Verdict locations) — quests reference investigation sites.
- Plan 89 (epilogues) — Standing Record outcomes feed endings.
- Plan 109 (echo quests) — Standing Record resolutions may trigger echoes.
