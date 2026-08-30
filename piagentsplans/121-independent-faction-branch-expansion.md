# Plan 121 — Independent Faction Branch Expansion (8 → 15 branches)

## Goal (2 lines)
Expand `independent_faction_branch.json` from 8 faction branches to 15. The
Independent faction branch catalog (`IndependentBranchCatalog.cs` confirmed
live) defines point-of-no-return character arcs for unaffiliated survivors,
each with a ponr_flag, ponr_trigger, moral band entry range, and multiple
endings. 8 branches for the independent survivor pool is thin; the
non-faction survivor pool needs more character-defining arcs.

## Why (P2)
- Verified: `independent_faction_branch.json` has 8 branches in `branches`
  array. Each has id, display_name, ponr_flag, ponr_trigger, entry_band_min,
  entry_band_max, endings (array of {ending_id, band_min, band_max}).
  `IndependentBranchCatalog.cs` loads it; `IndependentBranchIds.cs` pins
  the ids.
- The independent faction represents survivors with no faction allegiance —
  the largest survivor pool. 8 character arcs means most independent
  survivors have no personal ending path. The moral band entry range
  (very_evil to very_positive) allows branching across the full moral
  spectrum.
- Pure DATA work — zero new Core code.

## Files to touch
- `Assets/StreamingAssets/Data/independent_faction_branch.json` (expand
  `branches` 8 → 15)
- Read-only: `Assets/Ashfall.Core/Factions/IndependentBranchCatalog.cs`
  (confirm branch DTO and required fields)
- Read-only: `Assets/Ashfall.Core/Factions/IndependentBranchIds.cs` (confirm
  id conventions and whether new ids need to be registered here)

## Content grammar (per branch)
- `id`: snake_case, prefix `branch_ind_` (confirmed convention).
- `display_name`: evocative character title ("The Survivor", "The Hermit").
- `ponr_flag`: a flag id set when the point-of-no-return fires (prefix
  `flag_branch_ind_` — confirm in step 1).
- `ponr_trigger`: 1 sentence describing the irreversible choice moment.
- `entry_band_min` / `entry_band_max`: moral band range (very_evil /
  slightly_evil / neutral / slightly_positive / positive / very_positive).
- `endings`: array of ending objects:
  - `ending_id`: snake_case ending id (prefix `ending_ind_`).
  - `band_min` / `band_max`: moral band range for this ending.

## Steps
1. Read `IndependentBranchCatalog.cs` to confirm the branch and ending DTO
   and all required vs optional fields.
2. Read `IndependentBranchIds.cs` to confirm whether new branch ids must be
   registered in the static ids class (if so, add them — this is a minor
   integration step, not a new system).
3. Inventory the 8 existing branches: moral band coverage, ending
   distribution. Identify which moral bands and character archetypes are
   underrepresented.
4. Author 7 new branches:
   - `branch_ind_2_hermit`: a survivor who withdraws entirely; ponr when
     they refuse all human contact; endings range from self-sufficient
     isolation to lonely death.
   - `branch_ind_3_mediator`: a survivor who brokers between factions
     without joining any; ponr when they broker a deal that betrays one
     side; endings range from trusted neutral to expelled pariah.
   - `branch_ind_4_scavenger_king`: a survivor who hoards salvage; ponr
     when they claim a cache others need; endings range from resource
     baron to killed by a mob.
   - `branch_ind_5_caretaker`: a survivor who adopts the vulnerable; ponr
     when they shelter someone the camp wants gone; endings range from
     community pillar to overwhelmed collapse.
   - `branch_ind_6_witness`: a survivor who records everything; ponr when
     they publish a record that destroys someone's reputation; endings
     range from trusted historian to silenced.
   - `branch_ind_7_engineer`: a survivor who fixes infrastructure; ponr
     when they choose what to repair and what to let fail; endings range
     from indispensable to scapegoated when a repair fails.
   - `branch_ind_8_prophet`: a survivor who interprets the exchange as
     judgment; ponr when they call for a reckoning; endings range from
     spiritual leader to dangerous fanatic.
5. Each branch: distinct ponr_trigger, moral band entry range, 2–4 endings
   spanning the band range, no two branches sharing the same ending id.
6. Cross-reference: every branch id unique; every ponr_flag unique; every
   ending_id unique; every ending band_min/band_max is a valid band.
7. Wire 3 new branches to Plan 109 (echo quests — branch ponr triggers may
   fire echoes).
8. Wire 3 new branches to Plan 89 (epilogues — branch endings feed the
   epilogue matrix).
9. Wire 2 new branches to Plan 125 (moral choice flags — ponr_flags
   register in the flag catalog).
10. Validate: `--data-integrity-selftest` (all ids resolve).
11. xUnit: independent branch catalog loads 15 branches, all ids unique,
    all ponr_flags unique, all ending_ids unique, all bands valid.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
LOW — pure data. The one trap is `IndependentBranchIds.cs` (step 2): if the
static ids class must be updated, that is a minor Core edit, not a new
system. Confirm whether it is auto-generated or hand-maintained.

## Definition of Done
- `independent_faction_branch.json` has 15 branches, all ids unique, all
  ponr_flags unique, all ending_ids unique, 3 wired to echo quests, 3 to
  epilogues, 2 to moral choice flags, integrity + tests green.

## Follow-on
- Plan 109 (echo quests) — branch ponr triggers fire echoes.
- Plan 89 (epilogues) — branch endings feed the epilogue matrix.
- Plan 125 (moral choice flags) — ponr_flags register in the flag catalog.
- Plan 122/123 (military/rebel branches) — parallel branch structure.
- Plan 95 (journal voice) — branch ponr moments trigger journal entries.
