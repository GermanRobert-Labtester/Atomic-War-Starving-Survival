# Plan 123 — Rebel Faction Branch Expansion (8 → 15 branches)

## Goal (2 lines)
Expand `rebel_faction_branch.json` from 8 faction branches to 15. The Rebel
faction branch catalog (`RebelBranchCatalog.cs` confirmed live) defines
point-of-no-return character arcs for rebel-affiliated survivors, each with
a ponr_flag, ponr_trigger, moral band entry range, and multiple endings. 8
branches for the rebel/insurgent survivor pool is thin.

## Why (P2)
- Verified: `rebel_faction_branch.json` has 8 branches. Each has id,
  display_name, ponr_flag, ponr_trigger, entry_band_min, entry_band_max,
  endings. `RebelBranchCatalog.cs` loads it; `RebelBranchIds.cs` pins the
  ids.
- The rebel faction represents insurgents, resistance fighters, and
  dissidents. 8 character arcs means most rebel-affiliated survivors have
  no personal ending path. The faction war content (Plan 124) creates
  rebel-specific territorial moments that should feed branch ponr triggers.
- Pure DATA work — zero new Core code.

## Files to touch
- `Assets/StreamingAssets/Data/rebel_faction_branch.json` (expand `branches`
  8 → 15)
- Read-only: `Assets/Ashfall.Core/Factions/RebelBranchCatalog.cs`
- Read-only: `Assets/Ashfall.Core/Factions/RebelBranchIds.cs`

## Content grammar (per branch)
- `id`: snake_case, prefix `branch_rebel_` (confirmed convention).
- `display_name`: evocative character title ("The True Rebel").
- `ponr_flag`: flag id (prefix `flag_branch_rebel_`).
- `ponr_trigger`: 1 sentence describing the irreversible choice moment.
- `entry_band_min` / `entry_band_max`: moral band range.
- `endings`: array of {ending_id, band_min, band_max}.

## Steps
1. Read `RebelBranchCatalog.cs` to confirm the branch/ending DTO.
2. Read `RebelBranchIds.cs` to confirm whether new ids must be registered.
3. Inventory the 8 existing branches. Identify rebel archetypes not yet
   covered.
4. Author 7 new branches:
   - `branch_rebel_2_bombmaker`: an insurgent who builds improvised
     devices; ponr when one of their devices kills a civilian; endings
     from repentant defector to unrepentant zealot.
   - `branch_rebel_3_courier`: a rebel who carries messages between cells;
     ponr when they intercept a message that saves an enemy; endings from
     trusted network to executed traitor.
   - `branch_rebel_4_propagandist`: a rebel who writes and broadcasts;
     ponr when their propaganda incites a riot; endings from voice of
     the people to silenced by their own side.
   - `branch_rebel_5_defector`: a rebel who questions the cause; ponr when
     they approach the enemy for terms; endings from reconciled civilian
     to killed in defection.
   - `branch_rebel_6_protector`: a rebel who guards civilian camps; ponr
     when they refuse to fight to protect their post; endings from
     community shield to overwhelmed.
   - `branch_rebel_7_saboteur`: a rebel who destroys infrastructure; ponr
     when they destroy something civilians need; endings from resistance
     hero to starving the people they freed.
   - `branch_rebel_8_negotiator`: a rebel who seeks terms; ponr when they
     sign an accord the movement rejects; endings from peacemaker to
     assassinated collaborator.
5. Each branch: distinct ponr_trigger, band range, 2–4 endings, unique
   ending ids.
6. Cross-reference: all ids, ponr_flags, ending_ids unique; all bands
   valid.
7. Wire 3 branches to Plan 124 (faction war location overrides — rebel
   territorial actions trigger ponr moments).
8. Wire 3 branches to Plan 89 (epilogues — rebel branch endings feed the
   epilogue matrix).
9. Wire 2 branches to Plan 125 (moral choice flags — ponr_flags register).
10. Validate: `--data-integrity-selftest`.
11. xUnit: rebel branch catalog loads 15 branches, all ids/flags/endings
    unique, all bands valid.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
LOW — pure data. Same `RebelBranchIds.cs` consideration as Plans 121–122.

## Definition of Done
- `rebel_faction_branch.json` has 15 branches, all ids unique, 3 wired to
  faction war location overrides, 3 to epilogues, 2 to moral choice flags,
  integrity + tests green.

## Follow-on
- Plan 124 (faction war location overrides) — rebel actions trigger ponr.
- Plan 89 (epilogues) — rebel endings feed epilogue matrix.
- Plan 125 (moral choice flags) — ponr_flags register.
- Plan 121/122 (independent/military branches) — parallel structure.
- Plan 102 (foundry accords) — rebel negotiators may sign treaties.
