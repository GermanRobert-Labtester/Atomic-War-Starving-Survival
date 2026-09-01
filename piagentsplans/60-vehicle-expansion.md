# Plan 60 — Vehicle Expansion (3 → 10 vehicles)

## Goal (2 lines)
Expand `vehicles.json` from 3 verified entries to 10 vehicles. The expedition vehicle
system (Task #101 — vehicle logistics, breakdown, fuel, cargo capacity) is fully
implemented but only 3 vehicles exist. Add 7 vehicles across 4 tiers: foot, improvised,
civilian, military — each with distinct speed, cargo, fuel consumption, and breakdown
profiles.

## Why (P2)
- Verified: `vehicles.json` has 3 entries (`vehicle_utility_quad` and 2 others, with
  vehicle_id, display_name, max_fuel, cargo_capacity, speed_multiplier, terrain_type,
  condition_max, fuel_consumption_per_km, breakdown_threshold, default_attachments).
  Task #101 vehicle logistics is complete and tested (13 tests + 9 vehicle gates).
- Creates the vehicle-progression pillar: early game is on foot; mid game is improvised
  (bicycle, cart); late game is civilian (motorcycle, car, truck) and military (armored
  transport). Each vehicle changes expedition range, cargo capacity, and risk.
- Pure DATA work — zero new Core code.

## Files to touch
- `Assets/StreamingAssets/Data/vehicles.json` (expand 3 → 10 vehicles)
- `Assets/StreamingAssets/Data/items.json` (vehicle ids must resolve as items for
  acquisition via crafting/trading/scavenging)
- Read-only: `Assets/Ashfall.Core/Expeditions/` (confirm the vehicle schema and how
  speed_multiplier, cargo_capacity, breakdown_threshold, fuel_consumption_per_km affect
  expedition tick math)

## Content grammar (per vehicle)
- snake_case `id` with prefix `vehicle_` (confirmed prefix).
- tier: foot / improvised / civilian / military.
- terrain_type: rough / road / all_terrain / water (affects which routes the vehicle can
  traverse — feeds Plan 48 weather gates).
- speed_multiplier: 0.5 (foot) → 3.0 (military truck) — multiplies travel tick speed.
- cargo_capacity: 0 (foot) → 200 (truck) — determines how much loot the expedition can
  carry back.
- fuel_consumption_per_km: 0 (foot/bicycle) → 0.8 (truck) — determines fuel cost.
- breakdown_threshold: 0.05 (well-maintained) → 0.35 (improvised) — per-tick breakdown
  probability (reverts to foot speed/cargo on breakdown, per Task #101).
- condition_max: 100 for all; degrades with use, repaired with materials (Plan 55
  vehicle repair recipes).
- default_attachments: optional cargo rack, fuel tank, armor plate.

## Steps
1. Read `vehicles.json` to confirm the 3 existing vehicles and their schema.
2. Read the expedition vehicle system (Task #101) to confirm how speed/cargo/breakdown/
   fuel affect the travel tick math.
3. Author 7 new vehicles across 4 tiers:
   - Foot (1): on_foot (speed 0.5, cargo 20, fuel 0, breakdown 0 — the baseline).
   - Improvised (2): bicycle (speed 1.0, cargo 30, fuel 0, breakdown 0.05), cargo cart
     (speed 0.7, cargo 60, fuel 0, breakdown 0.08).
   - Civilian (2): motorcycle (speed 2.0, cargo 50, fuel 0.3, breakdown 0.15), station
     wagon (speed 1.5, cargo 100, fuel 0.4, breakdown 0.12).
   - Military (2): military transport (speed 2.5, cargo 150, fuel 0.6, breakdown 0.10,
     armor plate), armored personnel carrier (speed 2.0, cargo 200, fuel 0.8, breakdown
     0.08, armor plate — rare, faction-only).
4. Add 7 `item_vehicle_*` entries to `items.json` so vehicles are acquirable (crafted,
   traded, scavenged, or faction reward).
5. Wire 3 vehicles into Plan 55 crafting recipes (bicycle, cargo cart, motorcycle —
   craftable with materials and skills).
6. Wire 2 vehicles into Plan 43 settlement trade (station wagon, military transport —
   available only from specific settlements or as faction rewards).
7. Wire 1 vehicle (armored personnel carrier) to Plan 45 faction patrols — only
   factions have these; the player can acquire one by capturing a patrol.
8. Cross-reference: every vehicle `id` resolves in `items.json`; every attachment
   `item_*` id exists.
9. Validate: `--data-integrity-selftest`; run `--expedition-selftest` (9 vehicle gates);
   confirm each vehicle's speed/cargo/fuel/breakdown affects the travel tick math
   correctly in a headless boot.
10. xUnit: vehicle catalog loads, all ids resolve, speed_multiplier/cargo_capacity/
    breakdown_threshold/fuel_consumption apply correctly, breakdown reverts to foot,
    save round-trip preserves vehicle condition.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --expedition-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
LOW — pure data. The one trap is the on_foot baseline (step 3): confirm the system
already has an implicit on-foot state before adding it explicitly.

## Definition of Done
- `vehicles.json` has 10 vehicles (3 existing + 7 new), all ids resolving, 3 craftable,
  2 tradeable, 1 faction-acquired, travel tick math correct for each, breakdown reverts
  to foot, save round-trip green, integrity + tests green.

## Follow-on
- Plan 55 (recipes) — vehicle repair and crafting recipes.
- Plan 48 (weather gates) — terrain_type determines which routes vehicles can traverse.
- Plan 45 (patrols) — armored vehicles used by faction patrols.
- Plan 43 (settlements) — vehicle trade at specific settlements.
- Existing 10C (vehicles 3 → 8) — this plan exceeds that target (10).
