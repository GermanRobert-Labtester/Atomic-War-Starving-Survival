# Plan 49 — Micro-Location Discovery System (25 micro-locations)

## Goal (2 lines)
Create `micro_locations.json` — 25 small discoveries that appear along expedition travel
routes: roadside memorials, crashed trucks, frozen evacuation buses, improvised graves,
collapsed bridges, drainage pipes, rail sidings, dead livestock areas, ruined greenhouses,
shell craters, field kitchens, abandoned generators, shrines, emergency caches, military
observation posts. These make travel feel less empty and reward exploration beyond the
primary destination.

## Why (P2)
- Verified: `locations.json` has 115 locations but they are all primary destinations —
  there are no minor discoveries along travel routes. Expeditions are "dispatch → arrive →
  scavenge → return" with nothing in between. No `micro_locations.json` exists.
- Micro-locations create the travel-texture layer: each one is a 30-second encounter
  (short loot, environmental storytelling, small ethical decision, rumor) that breaks up
  travel and makes the wasteland feel littered with human history.
- Pure DATA work — micro-locations are lightweight location entries with short encounters.

## Files to touch
- `Assets/StreamingAssets/Data/micro_locations.json` (CREATE — 25 micro-locations)
- Read-only: `Assets/Ashfall.Core/ExpeditionSystem.cs` (confirm whether the travel-tick
  loop can spawn mid-route encounters — if yes, micro-locations hook into the per-tick
  encounter roll; if no, this is data-first), `Assets/StreamingAssets/Data/items.json`
  (micro-location loot must resolve), `Assets/StreamingAssets/Data/events.json` (77 events
  — micro-location encounters may reuse the event schema)
- Check: `grep -rn "micro_location\|travel_encounter\|route_encounter\|mid_route" Assets/Ashfall.Core/`

## Content grammar (per micro-location)
- snake_case `id` with prefix `loc_micro_` or `micro_` (confirm accepted prefix — `loc_` is
  known; `micro_` may need validation; prefer `loc_micro_` to stay within the known prefix).
- type: roadside_memorial / crashed_truck / frozen_evacuation_bus / improvised_grave /
  collapsed_bridge / drainage_pipe / rail_siding / dead_livestock / ruined_greenhouse /
  shell_crater / field_kitchen / abandoned_generator / shrine / emergency_cache /
  military_observation_post / abandoned_barricade / hunting_blind / radio_tower /
  destroyed_checkpoint / abandoned_tent / makeshift_clinic / crashed_drone / fuel_cache /
  water_source / supply_drop.
- loot: 1-3 `item_*` ids (one-time loot; depleted on first visit).
- encounter: short event id (from `events.json`) or inline encounter text — a 2-3
  sentence situation with 1-2 player choices (take, leave, investigate, help, ignore).
- environmental_story: 1-2 sentences of physical evidence (no exposition — show, don't
  tell: "A child's shoe sits on the bus seat. The doors are frozen open.").
- ethical_decision: optional — some micro-locations present a small moral choice (take
  from the dead, disturb a grave, ignore a cry for help, loot a shrine).
- rumor: optional — some micro-locations carry a rumor that points to a nearby primary
  location or quest hook (feeds Plan 32 expedition destinations + existing questlines).
- depletion: one-time (most micro-locations are depleted on first visit).

## Steps
1. Read `ExpeditionSystem.cs` to confirm the travel-tick loop: does it spawn mid-route
   encounters? If yes, confirm the encounter trigger schema and hook micro-locations into
   the per-tick roll. If no, this is data-first and the wiring is a follow-on.
2. Read `events.json` to confirm the event schema; micro-location encounters should reuse
   it (do not create a parallel system).
3. Read `locations.json` and `expeditions.json` (Plan 32) to understand which routes
   micro-locations should appear along (they appear between the shelter and the primary
   destination, not as destinations themselves).
4. Author 25 micro-locations across 25 types (one per type for variety):
   - Roadside memorial (candles, a name, a photo — take or leave).
   - Crashed supply truck (fuel, food, medicine — one-time loot).
   - Frozen evacuation bus (bodies, luggage, a child's shoe — environmental story).
   - Improvised grave (a mound, a cross, a name — disturb or respect).
   - Collapsed bridge (a detour, a body in the river, a fuel cache).
   - Drainage pipe (shelter for someone, a cache, a warning scratched in the wall).
   - Rail siding (a derailed car, coal, a maintenance log — journal unlock).
   - Dead livestock (irradiated cattle — disease risk if scavenged).
   - Ruined greenhouse (wilted seedlings, a seed cache — feeds existing 22B greenhouse).
   - Shell crater (unexploded ordnance — hazard; military salvage).
   - Field kitchen (abandoned, a pot of frozen soup, a fuel canister).
   - Abandoned generator (repairable — feeds existing 16B waystation power).
   - Shrine (candles, offerings — take offerings for morale penalty or leave).
   - Emergency cache (sealed, government-issued — medicine, rations, a radio).
   - Military observation post (binoculars, a logbook, a map — location clue).
   - Abandoned barricade (sandbags, ammo, a body — scavenging + environmental story).
   - Hunting blind (a dead hunter, a rifle, a journal — personal story).
   - Radio tower (dismantled, a surviving antenna — feeds existing 24A radio).
   - Destroyed checkpoint (bodies, a logbook, contraband — faction info).
   - Abandoned tent (a family's camp, a letter, a child's drawing — grief hook).
   - Makeshift clinic (abandoned, medical supplies, a triage list — existing 09A disease).
   - Crashed drone (military, salvageable electronics, a flight log — faction intel).
   - Fuel cache (buried, a map scratched on a can — location clue).
   - Water source (a spring, a well — may be contaminated — feeds existing 13A water).
   - Supply drop (parachute, a crate, government markings — rare one-time loot).
5. Give each: type, loot, encounter, environmental story, optional ethical decision,
   optional rumor, depletion flag.
6. Cross-reference: every `item_*` loot id resolves; every `event_*` encounter id resolves;
   every rumor `loc_*` target resolves to Plan 32.
7. Wire 10 micro-locations into the expedition travel-tick loop (if supported — step 1):
   each travel tick has a chance to spawn a micro-location encounter.
8. Validate: `--data-integrity-selftest`; confirm a micro-location appears mid-route in a
   headless boot; confirm loot depletes on first visit; confirm the ethical decision
   produces the correct outcome.
9. xUnit: micro-location catalog loads, all references resolve, loot depletes, ethical
   decisions apply outcomes, save round-trip preserves depleted state.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --expedition-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
MEDIUM — the travel-tick question (step 1) is the hazard: if `ExpeditionSystem` doesn't
spawn mid-route encounters, wiring micro-locations is a Core change. Confirm before
authoring. If it can't, the micro-locations are data-first and the integration is a
follow-on.

## Definition of Done
- `micro_locations.json` exists with 25 micro-locations, all references resolving, 10 wired
  into the travel-tick loop, loot depletes, ethical decisions apply, save round-trip green,
  integrity + tests green.

## Follow-on
- Plan 32 (expedition wiring) — micro-locations appear along travel routes.
- Plan 46 (scavenging tables) — micro-location loot uses the same item pool.
- Existing 17A (environmental storytelling) — micro-locations are the primary delivery
  vehicle for environmental story text.
- Existing 20C (encounter tables) — micro-location encounters are a travel-specific
  encounter category.
- W11 in roadmap 31 (micro-location discovery pillar).
