# Plan 125 — Moral Choice Flags Expansion (10 → 25 flags)

## Goal (2 lines)
Expand `moral_choice_flags.json` from 10 flags to 25. The moral choice flag
catalog (`MoralChoiceFlagCatalogLoader.cs` confirmed live) defines the
persistent flags that record the player's moral decisions throughout the
campaign — each flag has an id and display_name. 10 flags for the entire
moral choice system is very thin; most moral decisions leave no persistent
trace.

## Why (P1)
- Verified: `moral_choice_flags.json` has 10 flags in `flags` array. Each
  has id, display_name. `MoralChoiceFlagCatalogLoader.cs` loads it;
  `MoralChoiceFlagDefinitions.cs` pins the flag ids.
- Moral choice flags are the persistent-memory layer of the moral system.
  They are referenced by echo quests (Plan 109), faction branch ponr
  triggers (Plans 121–123), faction reactions (Plan 100), gossip (Plan
  110), and epilogues (Plan 89). 10 flags means the moral system has almost
  no memory — most choices don't register, and downstream systems can't
  react to them.
- Pure DATA work — zero new Core code.

## Files to touch
- `Assets/StreamingAssets/Data/moral_choice_flags.json` (expand `flags`
  10 → 25)
- Read-only: `Assets/Ashfall.Core/MoralChoice/MoralChoiceFlagCatalogLoader.cs`
  (confirm flag DTO)
- Read-only: `Assets/Ashfall.Core/MoralChoice/MoralChoiceFlagDefinitions.cs`
  (confirm whether new flag ids must be registered here)

## Content grammar (per flag)
- `id`: snake_case, prefix `flag_` (confirmed convention — e.g.
  `flag_betrayed_ally`).
- `display_name`: short human-readable label ("Betrayed an Ally").

## Steps
1. Read `MoralChoiceFlagCatalogLoader.cs` to confirm the flag DTO (id +
   display_name, or additional fields).
2. Read `MoralChoiceFlagDefinitions.cs` to confirm whether new flag ids
   must be registered in the static definitions class (if so, add them —
   minor integration, not a new system).
3. Inventory the 10 existing flags. Identify which moral choice categories
   (mercy, iron, listener, betrayal, faction-specific) lack flags.
4. Author 15 new flags:
   - `flag_spared_raider`: spared a surrendered raider's life.
   - `flag_executed_prisoner`: executed a surrendered prisoner.
   - `flag_shared_rations`: shared rations with strangers.
   - `flag_hoarded_medicine`: hoarded medicine while others died.
   - `flag_sheltered_refugee`: sheltered a refugee the camp rejected.
   - `flag_expelled_survivor`: expelled a survivor to save resources.
   - `flag_repaired_infrastructure`: repaired shared infrastructure.
   - `flag_sabotaged_rival`: sabotaged a rival faction's equipment.
   - `flag_broke_treaty`: broke a treaty or accord.
   - `flag_honored_debt`: honored a debt at personal cost.
   - `flag_ignored_distress`: ignored a distress signal.
   - `flag_responded_distress`: responded to a distress signal.
   - `flag_forged_record`: forged or altered a record.
   - `flag_preserved_archive`: preserved an archive at personal risk.
   - `flag_chosen_faction_side`: explicitly chose a faction side in a
     conflict.
5. Each flag: distinct id, clear display_name, no duplicates.
6. Cross-reference: every flag id unique; every id follows the `flag_`
   prefix convention.
7. Wire 5 new flags to Plan 109 (echo quests — echoes reference flags).
8. Wire 4 new flags to Plans 121–123 (faction branch ponr_flags — branch
   triggers reference flags).
9. Wire 3 new flags to Plan 100 (faction reactions — reactions fire on
   flags).
10. Wire 3 new flags to Plan 110 (gossip — gossip references flags).
11. Validate: `--data-integrity-selftest` (all flag ids resolve).
12. xUnit: moral choice flag catalog loads 25 flags, all ids unique, all
    display_names non-empty.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
LOW — pure data. The one trap is `MoralChoiceFlagDefinitions.cs` (step 2):
if the static definitions class must be updated, that is a minor Core edit.
Confirm whether it is auto-generated or hand-maintained.

## Definition of Done
- `moral_choice_flags.json` has 25 flags, all ids unique, 5 wired to echo
  quests, 4 to faction branch ponr, 3 to faction reactions, 3 to gossip,
  integrity + tests green.

## Follow-on
- Plan 109 (echo quests) — echoes reference flags.
- Plans 121–123 (faction branches) — ponr_flags reference flags.
- Plan 100 (faction reactions) — reactions fire on flags.
- Plan 110 (gossip) — gossip references flags.
- Plan 89 (epilogues) — flag state determines ending eligibility.
