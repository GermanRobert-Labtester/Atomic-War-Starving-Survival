# Shelter Operations Board Integration

**Status:** IMPLEMENTED; documentation-index check blocked by concurrent documentation edits
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
- `MainTriadDriftGateTests` 7/7 and `SaveSectionRegistryTests` 5/5; no new save section was introduced.
- Architecture map generator/check passed at 266 subsystems; self-test manifest check passed at 206 tests; catalog registry check passed at 708 catalogs / 14,391 definitions.
- The documentation index was regenerated to include this plan, but `generate-docs-index.py --check` remains red because other active documentation files changed between generation and verification. Their edits are outside this package; rerun the check after those writers finish.

## Authority and Persistence Boundaries

- `ShelterExpansionSystem` owns construction projects, crew assignment/progression, and completed-room capacity bonuses; `ShelterAssignmentSystem` applies the bonuses.
- `OutpostSettlementSystem` owns establishment, garrisons, supply reserves, and abandonment.
- `SeasonalCelebrationSystem` owns holiday occurrence resolution and cycle-scoped hold/skip state.
- Canonical inventory owns consumable quantities; survivor fitness, needs, skill, journal, and RNG remain in their existing systems.
- Existing save owners persist their respective state. The board is a read/command adapter only.

## Remaining Limitations

No feature limitations were identified in the accepted scope. The docs-index drift is external to this package and awaits a stable documentation snapshot. Five non-blocking duplicate-ID warnings remain in the data-integrity output for the radio distress expansion catalog; they are unrelated to this package.
