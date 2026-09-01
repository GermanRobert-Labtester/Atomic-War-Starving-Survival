# Plan 71 — Power Grid Rooms Expansion (6 → 18 powered rooms)

## Goal (2 lines)
Expand `power_grid.json` from 6 verified room entries to 18. The `PowerGridSystem` is
fully implemented (generation watts, battery capacity, fuel units, room draw, priority,
failure effects) but only 6 rooms are defined. The shelter's power grid is too simple —
adding rooms for the new shelter functions (Plan 41) makes power management a real
strategic decision.

## Why (P2)
- Verified: `power_grid.json` has 6 rooms (room_air_filtration, room_clinic, and 4
  others with id, display_name, draw_watts, default_priority, failure_effect_id).
  `PowerGridSystem` is confirmed in Core.
- Creates the power-management pillar: every room the player builds draws power. When
  the grid is overloaded, the player must prioritize (air filtration > clinic >
  workshop > greenhouse) or face failure effects. This makes the shelter's growth a
  power-management challenge, not just a resource cost.
- Pure DATA work — zero new Core code.

## Files to touch
- `Assets/StreamingAssets/Data/power_grid.json` (expand 6 → 18 rooms)
- Read-only: `Assets/Ashfall.Core/PowerGridSystem.cs` (confirm room schema: id,
  display_name, draw_watts, default_priority, failure_effect_id; confirm how priority
  affects load shedding when the grid is overloaded)

## Content grammar (per room)
- snake_case `id` with prefix `room_` (confirmed prefix from existing 6).
- draw_watts: power consumption (50–500W; critical systems draw more).
- default_priority: critical / high / medium / low — determines load-shedding order
  (critical is last to be shed; low is first).
- failure_effect_id: `fx_*` id — what happens when this room loses power (air filtration
  off → radiation ingress; clinic off → no medical treatment; workshop off → no
  crafting; greenhouse off → no food production).
- description: 1 sentence of grounded shelter flavor.

## Steps
1. Read `PowerGridSystem.cs` to confirm the room schema, priority/load-shedding logic,
   and failure-effect mechanism.
2. Read the 6 existing rooms to understand the structure.
3. Author 12 new rooms, aligned with Plan 41 shelter room catalog:
   - room_workshop (draw 200W, priority high, failure: no crafting).
   - room_greenhouse (draw 150W, priority medium, failure: no food production).
   - room_radio_room (draw 100W, priority high, failure: no radio reception).
   - room_laboratory (draw 300W, priority high, failure: no research).
   - room_armory (draw 50W, priority medium, failure: no security monitoring).
   - room_kitchen (draw 120W, priority medium, failure: no cooked food, morale penalty).
   - room_storage_cold (draw 80W, priority high, failure: food spoils).
   - room_generator_room (draw 0W, priority critical, failure: no generation — this is
     the source, not a consumer).
   - room_common_area (draw 40W, priority low, failure: morale penalty).
   - room_dormitory (draw 30W, priority low, failure: no heating, morale penalty).
   - room_water_treatment (draw 180W, priority critical, failure: no clean water).
   - room_surveillance (draw 90W, priority medium, failure: no perimeter detection).
4. Give each room: draw_watts, priority, failure_effect_id, description.
5. Cross-reference: every `room_*` id matches Plan 41 shelter room definitions; every
   `fx_*` failure effect id resolves (confirm the failure-effect catalog or create
   inline).
6. Wire 6 rooms to Plan 57 incidents (generator failure, air filter breakdown, water
   pipe burst — incidents cause power-grid failures).
7. Wire 4 rooms to Plan 41 shelter assignments (powered rooms produce output when
   powered; unpowered rooms produce nothing).
8. Validate: `--data-integrity-selftest`; confirm load shedding works (low-priority
   rooms shed first when the grid is overloaded) in a headless boot.
9. xUnit: power grid loads, all room ids resolve, load shedding follows priority,
   failure effects fire on power loss, save round-trip preserves grid state.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
LOW — pure data.

## Definition of Done
- `power_grid.json` has 18 rooms (6 existing + 12 new), all ids resolving to Plan 41,
  load shedding follows priority, failure effects fire on power loss, 6 wired to
  incidents, 4 wired to room assignments, save round-trip green, integrity + tests green.

## Follow-on
- Plan 41 (shelter rooms) — powered rooms produce output.
- Plan 57 (incidents) — equipment failure incidents affect the power grid.
- Plan 70 (shelter schedules) — schedules affect power draw (emergency = more draw).
- Existing 29B (machine personality) — powered rooms have machine personality.
- Plan 55 (recipes) — workshop and kitchen rooms require power for crafting.
