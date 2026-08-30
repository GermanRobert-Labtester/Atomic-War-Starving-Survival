# Plan 122 — Military Faction Branch Expansion (8 → 15 branches)

## Goal (2 lines)
Expand `military_faction_branch.json` from 8 faction branches to 15. The
Military faction branch catalog (`MilitaryBranchCatalog.cs` confirmed live)
defines point-of-no-return character arcs for military-affiliated survivors,
each with a ponr_flag, ponr_trigger, moral band entry range, and multiple
endings. 8 branches for the garrison/military survivor pool is thin.

## Why (P2)
- Verified: `military_faction_branch.json` has 8 branches. Each has id,
  display_name, ponr_flag, ponr_trigger, entry_band_min, entry_band_max,
  endings. `MilitaryBranchCatalog.cs` loads it; `MilitaryBranchIds.cs` pins
  the ids.
- The military faction (central garrison, Year of Ash) is a major
  late-campaign political force. 8 character arcs means most
  military-affiliated survivors have no personal ending path. The Year of
  Ash crises (Plan 114) create military-specific choice moments that
  should feed branch ponr triggers.
- Pure DATA work — zero new Core code.

## Files to touch
- `Assets/StreamingAssets/Data/military_faction_branch.json` (expand
  `branches` 8 → 15)
- Read-only: `Assets/Ashfall.Core/Factions/MilitaryBranchCatalog.cs`
- Read-only: `Assets/Ashfall.Core/Factions/MilitaryBranchIds.cs`

## Content grammar (per branch)
- `id`: snake_case, prefix `branch_mil_` (confirmed convention).
- `display_name`: evocative character title ("The Loyal Soldier").
- `ponr_flag`: flag id (prefix `flag_branch_mil_`).
- `ponr_trigger`: 1 sentence describing the irreversible choice moment.
- `entry_band_min` / `entry_band_max`: moral band range.
- `endings`: array of {ending_id, band_min, band_max}.

## Steps
1. Read `MilitaryBranchCatalog.cs` to confirm the branch/ending DTO.
2. Read `MilitaryBranchIds.cs` to confirm whether new ids must be
   registered (minor integration if so).
3. Inventory the 8 existing branches: moral band coverage, ending
   distribution. Identify military archetypes not yet covered.
4. Author 7 new branches:
   - `branch_mil_2_deserter`: a soldier who abandons post; ponr when they
     choose shelter over duty; endings from integrated civilian to hunted
     fugitive.
   - `branch_mil_3_reformer`: an officer who tries to change the garrison
     from within; ponr when they issue an order against command; endings
     from coup leader to executed mutineer.
   - `branch_mil_4_quartermaster`: a logistics officer who controls
     supplies; ponr when they divert rations to civilians; endings from
     feeding hero to court-martialed thief.
   - `branch_mil_5_medic_soldier`: a combat medic torn between saving and
     fighting; ponr when they refuse to fire on a target; endings from
     respected healer to abandoned by unit.
   - `branch_mil_6_conscript_father`: a conscript with a family in the
     shelter; ponr when ordered to act against their own community;
     endings from protector to deserter.
   - `branch_mil_7_intelligence_officer`: an officer who knows too much;
     ponr when they choose what to report and what to suppress; endings
     from trusted analyst to silenced liability.
   - `branch_mil_8_peacekeeper`: a soldier who tries to maintain order
     without violence; ponr when they refuse a lethal order; endings from
     community shield to overpowered by force.
5. Each branch: distinct ponr_trigger, band range, 2–4 endings, unique
   ending ids.
6. Cross-reference: all ids, ponr_flags, ending_ids unique; all bands
   valid.
7. Wire 3 branches to Plan 114 (Year of Ash questlines — crises trigger
   military ponr moments).
8. Wire 3 branches to Plan 89 (epilogues — military branch endings feed
   the epilogue matrix).
9. Wire 2 branches to Plan 125 (moral choice flags — ponr_flags register).
10. Validate: `--data-integrity-selftest`.
11. xUnit: military branch catalog loads 15 branches, all ids/flags/
    endings unique, all bands valid.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
LOW — pure data. Same `MilitaryBranchIds.cs` consideration as Plan 121.

## Definition of Done
- `military_faction_branch.json` has 15 branches, all ids unique, 3 wired
  to Year of Ash questlines, 3 to epilogues, 2 to moral choice flags,
  integrity + tests green.

## Follow-on
- Plan 114 (Year of Ash questlines) — crises trigger military ponr.
- Plan 89 (epilogues) — military endings feed epilogue matrix.
- Plan 125 (moral choice flags) — ponr_flags register.
- Plan 121/123 (independent/rebel branches) — parallel structure.
- Plan 98 (standing record factions) — military standing shifts.
