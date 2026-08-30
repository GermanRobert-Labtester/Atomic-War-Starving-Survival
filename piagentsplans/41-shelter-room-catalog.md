# Plan 41 — Shelter Room Catalog (system exists, no data)

## Goal (2 lines)
Create `shelter_rooms.json` for `ShelterAssignmentSystem` — the system is fully implemented
and save-supported but has **no room catalog** (verified: file missing). Add 20 room
definitions and 12 assignment rules that give the shelter internal geography: rooms have
identity, function, capacity, and history (feeds existing 29A room-as-character).

## Why (P2)
- Verified: `ShelterAssignmentSystem.cs` exists in `Assets/Ashfall.Core/Shelter/`; no room
  catalog exists. The system is wired but has no rooms to assign survivors to.
- Shelter interior is the player's home base: without room data, the shelter is a flat
  abstraction. Rooms create space for: work assignments (existing 12B duty roster),
  decor/memorial (existing 12C), machine personality (existing 29B), and room memory (W19).
- Pure DATA work — zero new Core code if a loader exists.

## Files to touch
- `Assets/StreamingAssets/Data/shelter_rooms.json` (CREATE — 20 rooms + 12 rules)
- Read-only: `Assets/Ashfall.Core/Shelter/ShelterAssignmentSystem.cs` (confirm room schema:
  room id, name, function, capacity, upgrade level, condition, assigned survivors),
  `Assets/Ashfall.Core/ExcavationSystem.cs` (confirm how rooms connect to excavation —
  starting rooms are dug via excavation; confirm the room↔excavation link)
- Check loader: `grep -rn "shelter_rooms\|ShelterRoom\|room_def" Assets/Ashfall.Core/`

## Content grammar (per room)
- snake_case `id` with prefix `room_` or `shelter_room_` (confirm accepted prefix — do not invent).
- function: dormitory / workshop / medical_bay / kitchen / storage / greenhouse / radio_room /
  armory / laboratory / common_area / airlock / generator_room.
- capacity: max survivors assignable (affects duty-roster assignments, existing 12B).
- upgrade_level: 0 (bare) → 3 (fully equipped); each level unlocks function bonuses.
- condition: integrity value (0-100); decays over time, repaired with materials.
- history: optional room-memory text (feeds W19 room identity + existing 29A room history).

## Content grammar (per assignment rule)
- snake_case `id` with prefix `rule_` or `assignment_` (confirm accepted prefix).
- room_function: which room types the rule applies to.
- survivor_requirement: skill id (Plan 33) or trait required for optimal assignment.
- bonus: production multiplier, morale bonus, or efficiency gain when the requirement is met.
- penalty: morale loss or inefficiency when a mismatched survivor is assigned.

## Steps
1. Read `ShelterAssignmentSystem.cs` end-to-end: confirm the room schema, the assignment
   logic, the capacity/upgrade mechanics, the condition decay, and the save DTO shape.
2. Confirm the room↔excavation link: are starting rooms dug via `ExcavationSystem` (then
   `shelter_rooms.json` defines room types, not instances) or are rooms placed directly?
3. Confirm loader status; if missing, add a mechanical loader.
4. Author 20 room definitions across 12 functions: 3 dormitory variants (crowded/standard/
   private), 2 workshop variants, 2 medical_bay variants, 1 kitchen, 2 storage variants,
   1 greenhouse, 1 radio_room, 1 armory, 1 laboratory, 2 common_area variants, 1 airlock,
   1 generator_room. Each with capacity, upgrade levels, condition, and history stubs.
5. Author 12 assignment rules: medical_bay requires `skill_field_surgery` for bonus;
   workshop requires `skill_diesel_mechanics`; radio_room requires `skill_radio_repair`;
   kitchen requires `skill_cooking`; laboratory requires `knowledge_*` research node; etc.
6. Cross-reference: every `skill_*` id resolves to Plan 33; every `knowledge_*` id resolves
   to Plan 34; every material `item_*` id exists in `items.json`.
7. Wire 5 rooms into the duty-roster system (existing 12B) so assignments produce output
   (food from kitchen, repairs from workshop, research from laboratory).
8. Validate: `--data-integrity-selftest`; confirm a room → assign survivor → produce →
   condition decay → repair loop works in a headless boot; save round-trip for room state.
9. xUnit: assignment rule applies bonus/penalty correctly, capacity enforced, condition
   decays deterministically, upgrade consumes materials, save round-trip green.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
MEDIUM — the room↔excavation link (step 2) is the hazard: if rooms are excavation instances,
`shelter_rooms.json` defines types not placements, and the schema must match. Confirm first.

## Definition of Done
- `shelter_rooms.json` exists with 20 rooms + 12 assignment rules, all ids resolving,
  assignment loop works end-to-end, bonus/penalty applied, condition decay deterministic,
  save round-trip green, integrity + tests green.

## Follow-on
- Existing 29A (room identity + history) — room history stubs become memory events.
- Existing 12B (duty roster) — room assignments produce duty-roster output.
- Existing 12C (shelter decor) — decor placed in rooms.
- W19 in roadmap 31 (shelter rooms as a content system).
- Plan 33/34 (skills/research) — assignment rules consume skill and research prerequisites.
