# Plan 45 — Patrol Encounter Schema

## TravelEncounterDefinition (extended)

| Field | Type | Required | Description |
|---|---|---|---|
| id | string | yes | Stable encounter ID |
| title | string | yes | Player-facing title |
| category | string | yes | "Human" for patrols |
| faction_id | string | no | Primary faction (Plan 45 addition) |
| territory_state | string | no | "controlled", "contested", "border" |
| cooldown_group | string | no | Cooldown grouping key for presentation variants (Plan 45 / F13) |
| region_tags | string[] | yes | Region eligibility |
| min_danger_level | float | yes | Minimum danger for eligibility |
| max_danger_level | float | yes | Maximum danger for eligibility |
| base_weight | float | yes | Selection weight |
| stance_weights | dict | yes | Weight multipliers per stance |
| season_tags | string[] | yes | Season eligibility |
| description | string | yes | Encounter description |
| choices | list | yes | Player choices (2-4) |

## TravelEncounterChoice (extended)

| Field | Type | Required | Description |
|---|---|---|---|
| choice_id | string | yes | Stable choice ID |
| text | string | yes | Player-facing text |
| is_nonviolent | bool | yes | Whether choice avoids combat |
| is_avoidance | bool | yes | Whether choice avoids interaction |
| morale_delta | int | yes | Morale change |
| guilt_delta | int | yes | Guilt change |
| faction_id | string | no | Faction affected (Plan 45) |
| faction_standing_delta | int | no | Standing change (Plan 45) |
| cost_items | string[] | no | Items consumed (Plan 45) |
| required_item_id | string | no | Item required (Plan 45) |
| required_item_quantity | int | no | Quantity required (Plan 45) |

## Validation (Plan 45 / F15)

The `PatrolEncounterValidator` mechanically enforces data integrity across all `enc_patrol_*` definitions during CI and `--data-integrity-selftest`:

1. **Category**: Must equal `"Human"`.
2. **Faction Identity**: `faction_id` must resolve to an authored faction in `faction_lore.json`.
3. **Territory State**: Must be one of `controlled`, `contested`, or `border`.
4. **Choice Cardinality**: Each encounter must contain between 2 and 4 choices.
5. **Choice Uniqueness**: Choice IDs and player-facing texts must be unique within the encounter.
6. **Standing Deltas**: Standing changes must fall strictly within `[-25, 10]`.
7. **Item Referential Integrity**: All `cost_items` and `required_item_id` references must resolve in `items.json`.
8. **Item Gate Separation**: An item cannot appear in both `required_item_id` and `cost_items` within the same choice (gates are non-consuming).
9. **Weight & Danger Bounds**: `base_weight` must be between `[0.1, 5.0]`; `0 <= min_danger_level <= max_danger_level`.
10. **Tag Completeness**: `region_tags` and `season_tags` cannot be empty.
11. **Variant Family Invariance**: All encounters sharing a `cooldown_group` must share identical mechanics: category, faction, territory state, chain linkage, and all choice properties/costs.
