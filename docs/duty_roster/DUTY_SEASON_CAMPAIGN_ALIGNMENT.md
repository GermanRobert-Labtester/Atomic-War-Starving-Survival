# Duty Season Campaign / Weather / Chapter Alignment

One authority per fact. Duty seasons are **roster-pressure phases** selected from the authoritative campaign day; they are not meteorology and not narrative progression.

## Authority map

| Fact | Authority | Plan 77 role |
|---|---|---|
| Campaign day | `SimClock` | input to selection |
| Duty-season definition | `duty_roster_seasons.json` / `DutyRosterCatalog` | authored phase data |
| Active duty season | derived via `GetSeasonForDay(day)` | derived state only |
| Shelter-encounter multiplier state | `ShelterEncounterSystem` | receives the weight (replacement) |
| Weather state | `weather_seasons.json` / `WeatherSystem.GetSeasonForDay` | alignment only — untouched |
| Campaign chapters | `narrative_progression.json` (display metadata) | alignment only — untouched |
| Incidents | incident authority | no season wiring |
| Schedules | schedule authority | not present in this area |
| Expeditions / steam trips | `BrineWaterSystem` et al. | not related (verified — see modifier semantics) |

No circular dependencies: every integration is directional and read-only. The season catalog owns nothing outside its five fields.

## Weather alignment (Plan 48 / `weather_seasons.json` windows)

| Duty season | Days | Weather window(s) active | Alignment note |
|---|---|---|---|
| first_ashfall | 0–7 | Ash Fall (0+) | coherent — ashfall arrival |
| second_winter | 8–12 | Ash Fall | **name is a roster-phase label, not weather** — preserved per ID-stability rule; the weather authority stays canonical |
| settling | 13–30 | Ash Fall | coherent |
| spring_thaw | 31–60 | Ash Fall → Deep Freeze (60) | **social-phase label**: mobility/relief precede meteorological thaw; documented deliberately, weather untouched |
| faction_pressure | 61–120 | Deep Freeze | coherent (pressure during winter) |
| first_siege | 121–180 | Thaw (120+) | coherent — siege pressure amid movement season |
| consolidation | 181–240 | Black Bloom (180+) | coherent — rebuild during ash-return |
| long_winter | 241–365 | High Cold (240) → The Turning (300) | coherent — late cold |

The two deliberate divergences (`second_winter` placement, `spring_thaw` as social phase) follow the plan's rule: existing IDs are never renamed for prose quality, and new labels avoid claiming meteorology the weather system does not guarantee. `WeatherSystem` remains the sole weather authority; no weather state is duplicated in this catalog.

## Chapter alignment (Plan 74)

Chapters are day-agnostic display rows (no runtime triggers), so no day-level synchronization is possible or attempted. Thematic correlation, documented only:

| Duty season | Correlated chapters |
|---|---|
| first_ashfall | 1 The Exchange, 2 Ashfall |
| second_winter | (early pressure pocket) |
| settling | 3 The Bunker, 4 First Contact |
| spring_thaw | 5 The Long Winter tail, 8 The Thaw |
| faction_pressure | 6 The Consolidation |
| first_siege | 9 The Schism, 10 The Black Market |
| consolidation | 12 The Rebuilding |
| long_winter | 13 The Second Winter, 15 The Inheritance |

Chapters own narrative; seasons never advance, gate, or trigger them.

## Schedules (Plan 70) and incidents (Plan 57)

Not present in this area / no season wiring exists. Both recorded as follow-on consumers: schedules may query `GetSeasonForDay` through the existing typed API; incidents must consume encounter pressure without double scaling (see modifier semantics).
