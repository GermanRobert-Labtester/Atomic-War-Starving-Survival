# Plan 124 — Faction War Location Overrides Expansion (9 → 20 overrides)

## Goal (2 lines)
Expand `faction_war_location_overrides.json` from 9 location overrides to 20.
The faction war content catalog (`FactionWarContentCatalog.cs` confirmed
live) defines time-windowed location state changes (pre_strike, post_strike,
occupied, abandoned, etc.) that alter a location's display name and
description during faction conflict. 9 overrides for the entire faction war
is thin; the territorial conflict needs more locations that visibly change
when factions fight over them.

## Why (P2)
- Verified: `faction_war_location_overrides.json` has 9 entries in
  `locationOverrides` array. Each has id, locationId, overrideType,
  activeFromDay, activeUntilDay, displayName, description.
  `FactionWarContentCatalog.cs` loads it.
- The faction war system is the territorial-conflict pillar. 9 location
  overrides means most locations never change during the war — the world
  feels static when factions should be visibly fighting over territory.
  The override types (pre_strike, post_strike, etc.) and day windows
  allow rich temporal storytelling.
- Pure DATA work — zero new Core code.

## Files to touch
- `Assets/StreamingAssets/Data/faction_war_location_overrides.json` (expand
  `locationOverrides` 9 → 20)
- Read-only: `Assets/Ashfall.Core/YearOfAsh/FactionWarContentCatalog.cs`
  (confirm override DTO and how locationId/overrideType resolve)

## Content grammar (per override)
- `id`: snake_case, prefix `loc_override_` (confirmed convention).
- `locationId`: a location id that exists in the location catalog (must
  resolve).
- `overrideType`: string describing the override type (pre_strike,
  post_strike, occupied, abandoned, fortified, liberated, contaminated —
  confirm valid set in step 1).
- `activeFromDay` / `activeUntilDay`: day window when the override is
  active (must be ordered).
- `displayName`: the name the location shows during the override.
- `description`: 2–4 sentences describing the location's state during the
  override.

## Steps
1. Read `FactionWarContentCatalog.cs` to confirm the override DTO, the
   valid `overrideType` values, and how `locationId` resolves (against
   which location catalog).
2. Inventory the 9 existing overrides: which locations are covered, which
   override types are used. Identify locations in the deep lore catalog
   (Plan 116) and Verdict locations (Plan 82) that lack war overrides.
3. Author 11 new overrides:
   - `loc_override_checkpoint_occupied`: a garrison checkpoint under
     military occupation; day 200–250; barricades, sandbags, redirected
     traffic.
   - `loc_override_granary_burned`: the Crossing granary after a raid;
     day 220–260; scorched grain, collapsed roof, rats.
   - `loc_override_well_contaminated`: the Crossing well after sabotage;
     day 240–280; bodies in the water, boil-order notices, abandoned
     buckets.
   - `loc_override_rail_yard_fortified`: a rail yard turned into a
     faction fortress; day 260–320; razor wire, watchtowers, blocked
     tracks.
   - `loc_override_village_abandoned`: a village evacuated during
     faction advance; day 280–340; open doors, left meals, scattered
     belongings.
   - `loc_override_factory_occupied`: a factory seized by rebels;
     day 300–350; machinery running, armed workers, production for the
     cause.
   - `loc_override_bridge_destroyed`: a bridge blown to halt an advance;
     day 310–360; twisted metal, gap in the span, cold water below.
   - `loc_override_roadblock_liberated`: a roadblock dismantled after
     retreat; day 330–370; scattered debris, abandoned weapons, free
     passage.
   - `loc_override_camp_overrun`: a refugee camp overrun in an assault;
     day 340–380; trampled tents, blood trails, silence.
   - `loc_override_station_reclaimed`: a station reclaimed by
     survivors; day 350–400; swept floors, new locks, cautious return.
   - `loc_override_field_scorched`: a battlefield after sustained
     shelling; day 360–400; craters, unexploded ordnance, contaminated
     soil.
4. Each override: distinct locationId, overrideType, day window, and
  evocative description in the established faction-war voice.
5. Cross-reference: every locationId resolves; every id unique; every
  activeFromDay < activeUntilDay.
6. Wire 4 overrides to Plan 116 (deep lore locations — overridden
  locations are deep lore locations).
7. Wire 3 overrides to Plan 114 (Year of Ash questlines — crises
  produce location overrides).
8. Wire 2 overrides to Plan 115 (crossing encounters — Crossing
  locations get war overrides).
9. Validate: `--data-integrity-selftest` (all locationIds resolve).
10. xUnit: faction war override catalog loads 20 overrides, all ids
    unique, all locationIds resolving, all day windows ordered.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
LOW — pure data. The one trap is `locationId` resolution (step 5): every
locationId must exist in the location catalog or the integrity validator
will reject it. Confirm the target locations exist (or will exist after
Plan 116) before authoring.

## Definition of Done
- `faction_war_location_overrides.json` has 20 overrides, all ids unique,
  all locationIds resolving, all day windows ordered, 4 wired to deep lore
  locations, 3 to Year of Ash questlines, 2 to crossing encounters,
  integrity + tests green.

## Follow-on
- Plan 116 (deep lore locations) — overridden locations are deep lore.
- Plan 114 (Year of Ash questlines) — crises produce overrides.
- Plan 115 (crossing encounters) — Crossing locations get overrides.
- Plan 123 (rebel faction branches) — rebel territorial actions trigger.
- Plan 85 (damaged map zones) — overrides complement damaged zones.
