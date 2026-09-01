# Plan 45 — Patrol Encounter Schema

## TravelEncounterDefinition (extended)

| Field | Type | Required | Description |
|---|---|---|---|
| id | string | yes | Stable encounter ID |
| title | string | yes | Player-facing title |
| category | string | yes | "Human" for patrols |
| faction_id | string | no | Primary faction (Plan 45 addition) |
| territory_state | string | no | "controlled", "contested", "border" |
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
