# Plan 48 — Weather Route Gates (15 weather-gated routes & locations)

## Goal (2 lines)
Create `weather_route_gates.json` — 15 weather-gated routes and locations that are only
accessible or safe under specific weather conditions. A blizzard corridor is impassable in
winter; a contaminated-fog zone is lethal without a gas mask; a frozen lake crossing is
safe only in deep cold. Weather becomes a world-content gate, not just a survival modifier.

## Why (P2)
- Verified: `WeatherSystem` is fully implemented with 22 weather states (existing 13C
  expands crisis events); but weather has no world-content gates — it only modifies
  survival stats, not access. There is no `weather_route_gates.json`.
- Creates the weather-as-content pillar: weather determines where the player can go and
  when, making the calendar a strategic resource. A location visible in summer may be
  unreachable until winter freezes the approach.
- Pure DATA work — gates reference existing weather states and expedition destinations.

## Files to touch
- `Assets/StreamingAssets/Data/weather_route_gates.json` (CREATE — 15 gates)
- Read-only: `Assets/Ashfall.Core/WeatherSystem.cs` (confirm weather-state enum: fallout_storm,
  black_rain, severe_cold, blizzard, emp_condition, contaminated_fog, acid_precipitation,
  wind, etc.), `Assets/StreamingAssets/Data/expeditions.json` (Plan 32 — gates reference
  expedition destination ids), `Assets/StreamingAssets/Data/wasteland_map_v1.json` (gates
  reference map route ids)
- Check: `grep -rn "weather\|Weather\|gate\|route_gate" Assets/Ashfall.Core/` — does the
  expedition or map system already support weather gating, or is this data-first?

## Content grammar (per gate)
- snake_case `id` with prefix `gate_` or `weather_gate_` (confirm accepted prefix — do not invent).
- gate_type: route_gate (blocks a map route) / location_gate (blocks an expedition
  destination) / region_gate (blocks a whole map region).
- target: `loc_*` id (expedition destination) or map route id — what is gated.
- required_weather: the weather state(s) under which the gate is PASSABLE (e.g. a frozen
  lake crossing requires `severe_cold` or `blizzard`; a fog route requires
  `contaminated_fog` to be absent).
- blocked_weather: the weather state(s) under which the gate is BLOCKED (e.g. a mountain
  pass is blocked by `blizzard`; a lowland route is blocked by `black_rain` flooding).
- requirement_override: an item or skill that bypasses the gate (gas mask bypasses
  `contaminated_fog`; `skill_mountaineering` bypasses `blizzard` pass — feeds Plan 33).
- consequence_on_force: what happens if the player forces the gate when blocked (radiation
  exposure, frostbite, vehicle breakdown, lost party, death).
- description: 1-2 sentences of grounded environmental storytelling (why this route is
  weather-dependent — a frozen marsh, a wind-scoured pass, a flood-prone underpass).

## Steps
1. Read `WeatherSystem.cs` to confirm the weather-state enum and how weather is queried
   per-region/per-tick.
2. Read `expeditions.json` (Plan 32) and `wasteland_map_v1.json` to identify routes and
   destinations that would plausibly be weather-gated (mountain passes, frozen crossings,
   fog-prone lowlands, flood-prone routes, EMP-vulnerable exposed ridges).
3. Confirm whether the expedition/map system supports weather gating (step in Files
   section). If not, this is data-first and the wiring is a follow-on integration task.
4. Author 15 weather gates across 5 weather types:
   - 4 blizzard gates (mountain pass, highland route, exposed ridge, frozen lake crossing)
   - 3 contaminated_fog gates (lowland marsh, industrial valley, river basin)
   - 3 black_rain gates (flood-prone underpass, riverside route, drainage culvert)
   - 2 fallout_storm gates (open wasteland route, exposed highway)
   - 2 severe_cold gates (frozen lake crossing — passable ONLY in severe cold; ice road)
   - 1 emp_condition gate (electronics-dependent route — vehicle fails during EMP)
5. Give each gate: type, target, required/blocked weather, override, consequence, description.
6. Cross-reference: every `loc_*` id resolves to Plan 32; every map route id exists; every
   weather state matches the `WeatherSystem` enum; every override `item_*` / `skill_*` id
   resolves.
7. Wire 5 gates into the expedition dispatch system (Plan 32) — gated destinations are
   unavailable when the weather blocks them; the dispatch panel shows the gate reason.
8. Wire 3 gates into the caravan system (existing 16B) — caravans reroute or delay when
   weather gates block their route.
9. Validate: `--data-integrity-selftest`; confirm a gated destination is unavailable under
   blocked weather and available under required weather in a headless boot; confirm the
   override bypasses the gate.
10. xUnit: gate catalog loads, weather-state references match the enum, gates block/unblock
    correctly per weather, override applies, consequence fires on forced passage, save
    round-trip preserves gate state.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
MEDIUM — the gating-support question (step 3) is the hazard: if the expedition system
doesn't check weather before dispatch, wiring gates is a Core change. Confirm before
authoring. If it can't, the gates are data-first and the integration is a follow-on.

## Definition of Done
- `weather_route_gates.json` exists with 15 gates, all references resolving, 5 wired into
  expedition dispatch, 3 wired into caravans, gates block/unblock per weather, override
  works, consequence fires on forced passage, save round-trip green, integrity + tests green.

## Follow-on
- Plan 32 (expedition wiring) — gated destinations show gate reasons in the dispatch panel.
- Plan 33 (skills) — mountaineering and similar skills bypass weather gates.
- Existing 13C (weather crisis events) — crisis events interact with weather gates.
- Existing 16B (caravans) — caravans reroute around weather gates.
- Existing 19A (weather forecasting) — forecasting lets the player plan around gates.
- W48 in roadmap 31 (weather as a world-content gate).
