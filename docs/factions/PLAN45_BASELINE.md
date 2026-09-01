# Plan 45 — Baseline Reconnaissance

## Architecture Decision: Path A — Extend Existing travel_encounters.json

The existing `TravelEncounterSystem` already supports:
- `category: "Human"` with 8 existing non-faction entries
- `region_tags[]` for territory-based eligibility
- `min/max_danger_level` for strength scaling
- `stance_weights{}` for behavioral filtering
- `season_tags[]` for seasonal gating
- `choices[]` with `morale_delta`, `guilt_delta`, `is_nonviolent`, `is_avoidance`
- 5-day cooldown per encounter
- `OnChoiceResolved` event for consequence hooks

## Schema Extension
Added to `TravelEncounterChoice`:
- `faction_id` (string) — which faction the patrol belongs to
- `faction_standing_delta` (int) — standing change on choice
- `cost_items` (List<string>) — items required for toll/bribe
- `required_item_id` (string) — item required for pass/disguise
- `required_item_quantity` (int) — quantity of required item

Added to `TravelEncounterDefinition`:
- `faction_id` (string) — primary faction
- `territory_state` (string) — "controlled", "contested", "border"

All additions are backward-compatible (empty/default values for existing encounters).

## Territory System
- Lives in `WarlordDoctrineSystem.cs`
- States: None, Claimed, Contested, Controlled
- Tracked per locationId via `WarlordTerritoryRecord`
- `TravelDangerModifier(locationId)` returns danger multiplier
- Delegate pattern for cross-system coupling
- Location IDs are data-driven from `warlord_doctrines.json`

## Encounter Systems
- `TravelEncounterSystem` — region/season/chain/cooldown/stance filters
- `NarrativeEncounterSystem` — used by ExpeditionEncounterBridge
- `DoorEncounterSystem` — faction standing, item requirements
- `CrossingEncounterSystem` — cost_items, target_location

## Faction Data
- 22 factions in `faction_lore.json`
- 13 factions represented in patrol encounters
- Patrol references pervasive in narrative JSON (10+ files)

## Existing Patrol Infrastructure
- `patrol_debriefs.json` — structured after-action reports
- `directive_iron_garrison_patrol_order_14` — full patrol order
- `npc_lost_patrol_sergeant` — patrol sergeant survivor profile
- `faction_iron_garrison_patrol` — knowledge key
