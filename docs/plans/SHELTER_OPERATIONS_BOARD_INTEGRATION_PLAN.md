# Shelter Operations Board Integration

**Status:** COMPLETE  
**Claim:** `claim-shelter-operations-board-2026-09-25`  
**Authority:** User-approved `skill_crafting`; existing shelter, inventory, survivor, outpost, celebration, and save owners remain authoritative.

## Delivered

- Added the routed `shelter_operations` board for construction, excavation, renovation, upgrades, crews, capacity projection, outposts, garrisons, supplies, and cycle-scoped holidays.
- Added atomic inventory-backed construction, outpost establishment/supply, and holiday hold commands. Existing non-atomic APIs remain for compatibility; the board uses the safe `Try*` paths.
- Connected daily crew work to canonical skill practice, fitness, fatigue, morale, journal, and shelter assignment authorities.
- Added the canonical `skill_crafting` definition and authored construction/outpost catalog entries.
- Reused the existing construction, outpost, and celebration save owners. No board-specific save store or save section was added.
- Registered the board as a player-navigable expanded route and marked it as interactive in the player-surface contract.

## Verification

- Focused construction, capacity, outpost, holiday, route, player-surface, Plan 58/156, and skill-catalog suites passed.
- `dotnet build Ashfall.csproj --no-restore`: 0 warnings, 0 errors.
- `--shelter-operations-selftest`: legacy smoke passed; board probe passed 12/12.
- `--player-panels-uitest`: passed, including the board bind/open/visible smoke and 21/21 panel lifecycle gates.
- `--data-integrity-selftest`: 0 errors across 424 catalogs; 5 documented primary-wins warnings.
- Architecture map generator/check passed at 266 subsystems.

## Authority and Persistence Boundaries

- `ShelterExpansionSystem` owns construction projects, crew assignment/progression, and completed-room capacity bonuses; `ShelterAssignmentSystem` applies the bonuses.
- `OutpostSettlementSystem` owns establishment, garrisons, supply reserves, and abandonment.
- `SeasonalCelebrationSystem` owns holiday occurrence resolution and cycle-scoped hold/skip state.
- Canonical inventory owns consumable quantities; survivor fitness, needs, skill, journal, and RNG remain in their existing systems.
- Existing save owners persist their respective state. The board is a read/command adapter only.

## Remaining Limitations

None identified in the accepted scope. Five non-blocking duplicate-ID warnings remain in the data-integrity output for the radio distress expansion catalog; they are unrelated to this package.
