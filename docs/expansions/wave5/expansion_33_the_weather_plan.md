# ASHFALL — Expansion 33 Design Bible
# THE WEATHER
### Wave 5 · Forecasting, Storms, Seasons, Ashfall, Black Rain, and Shelter Adaptation

**Document status:** Design plan (pre-integration). Not a claim. Not an authorization.
**Date:** 2026-09-22
**Domain owners touched:** `Ashfall.Core.World` (WeatherSystem, WeatherEffectsCatalog, WeatherGate), `Ashfall.Core.YearOfAsh` (YearOfAshStormCatalog), `Ashfall.Core.Economy` (EconomyWeatherShockRules)
**Proposed host owner:** `SkywatchHostSession` (extends the weather, gate, and shelter surfaces)
**Existing save sections:** weather state, weather gate state, shelter hardening
**Existing CLI verbs:** `--weather-selftest`, `--data-integrity-selftest`, `--content-utilization-selftest`
**Rule compliance:** Godot authoritative; Core engine-free; JSON data authoritative; one authority per concern; deterministic seeded RNG.

---

## 0. HOW TO READ THIS DOCUMENT

This is a **design bible**, not an integration plan. Implementation must later pass
through `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, and `TEST_POLICY.md`.

ASHFALL already has weather. `WeatherSystem` (22 KB) defines `SeasonWindowDef`
with weighted weather kinds — clear, rain, overcast, ashfall, fallout storm,
blizzard, and black rain — and `SeasonProfileDef` with a weather check interval
(default six hours), and it holds `WorldWeatherState` with the current kind,
elapsed hours, and time to the next check. `WeatherEffectsCatalog` (9.1 KB)
consumes effects; `WeatherGate` and `WeatherGateCatalog` gate routes and travel;
`WeatherAtmosphereMap` maps weather over space; `YearOfAshStormCatalog` (5.5 KB)
holds authored storms; `EconomyWeatherShockRules` ties weather to the economy;
`WeatherGateRadioHooks` and `PatrolRadioHooks` surface warnings. The data is
real: `weather_effects.json` (7.6 KB), `weather_route_gates.json` (11 KB),
`seasonal_events.json` (12.3 KB), `weather_seasons.json` (3.2 KB), and
`weather_hardening_upgrades.json` (3.7 KB).

What does not exist: forecasting as a discipline, observation posts and
instruments, warnings the shelter can trust, storm tracks and storm preparation,
ash-season operations, black-rain response, seasonal readiness planning, and a
weather record that becomes an almanac.

**The Weather** turns the live weather engine into the shelter's oldest
adversary and most useful teacher: what the sky will do, what the shelter can do
about it, and what it must simply endure.

Conventions: **`LIVE`** confirmed in source/data; **`GAP`** confirmed thin;
**`PROPOSED`** new.

---

## 1. EXECUTIVE SUMMARY

### 1.1 Pitch

The shelter cannot stop a storm. It can see one coming, warn its people, close
what needs closing, and be ready before the first gust.

**The Weather** is the expansion about the sky: observation posts, instruments,
forecast skill, warnings, storm preparation and recovery, ash seasons, black
rain, seasonal readiness, and the records that become an almanac. It extends the
live weather engine with authored forecasts, tracks, warnings, and response
programs, and it wires every effect through the systems that already own it:
shelter atmosphere, route gates, the economy, and the medical pipeline.

The expansion's hard rules follow the live owners: `WeatherSystem` remains the
weather authority, `WeatherGate` keeps travel gating, `ShelterAtmosphereSystem`
keeps interior comfort, the radiation pipeline keeps dose, and the filter and
scrubber systems of the clean-flow and alarm expansions keep their owners. No
second weather, hazard, or save system is created.

### 1.2 The five loops it adds

```
   Observe ──► Record ──► Forecast ──► Warn ──► Prepare
      │           │           │          │         │
      ▼           ▼           ▼          ▼         ▼
   Posts,      Ledgers,    Signs and  Sirens,   Shutters,
   senses      patterns    models     notices   filters,
                                                closures
                                                   │
                                              Storm hits
                                                   │
                                                   ▼
                                              Damage ──► Recover ──► Learn
                                                                       │
                                                                       ▼
                                                                    Almanac
```

### 1.3 What the player manages

1. **Observation.** Posts, watches, instruments, and what a person can see.
2. **Records.** Daily weather logs that build pattern knowledge.
3. **Forecast.** Confidence, lead time, and the cost of being wrong.
4. **Warnings.** Sirens, notices, radio calls, and door-to-door alerts.
5. **Preparation.** Shutters, sandbags, filters, closures, and staged supplies.
6. **Storms.** Shelter in place, damage control, rescue, and recovery.
7. **Ash seasons.** Long-duration ashfall operations and filter maintenance.
8. **Black rain.** Fallout-laced rain response and the decision to close.
9. **Readiness.** Seasonal prep calendars shared with farming, building, and
   travel.

### 1.4 What it is not

- Not a second weather system. `WeatherSystem` remains the authority.
- Not a second radiation or contamination system. Fallout rain drives the live
  radiation and disease pipelines.
- Not a travel system. `WeatherGate` keeps route authority; roads belong to The
  Long Road.
- Not a construction system. Hardening lands in the live upgrade path.
- Not an economic system. `EconomyWeatherShockRules` keeps economy effects.
- Not a new save section.

---

## 2. EVIDENCE BASE AND GAP ANALYSIS

### 2.1 Live systems (verified)

| File | Role | Status |
|---|---|---|
| `Assets/Ashfall.Core/World/WeatherSystem.cs` | Seasons, weights, kinds, ticks | `LIVE` |
| `Assets/Ashfall.Core/World/WeatherEffectsCatalog.cs` | Authored effects | `LIVE` |
| `Assets/Ashfall.Core/World/WeatherGate.cs` | Route gating | `LIVE` |
| `Assets/Ashfall.Core/World/WeatherGateCatalog.cs` | Gate definitions | `LIVE` |
| `Assets/Ashfall.Core/World/WeatherAtmosphereMap.cs` | Spatial weather | `LIVE` |
| `Assets/Ashfall.Core/YearOfAsh/YearOfAshStormCatalog.cs` | Named storms | `LIVE` |
| `Assets/Ashfall.Core/Economy/EconomyWeatherShockRules.cs` | Economy shocks | `LIVE` |
| `Assets/Ashfall.Core/Radio/WeatherGateRadioHooks.cs` | Radio warnings | `LIVE` |
| `Assets/StreamingAssets/Data/weather_hardening_upgrades.json` | Hardening path | `LIVE` |

### 2.2 Live data (counted)

| Catalog | Size | Notes |
|---|---|---|
| `weather_effects.json` | 7.6 KB | effects exist |
| `weather_route_gates.json` | 11 KB | route gating |
| `seasonal_events.json` | 12.3 KB | authored seasonal events |
| `weather_seasons.json` | 3.2 KB | season windows |
| `weather_hardening_upgrades.json` | 3.7 KB | upgrades |
| Forecast/observation/almanac data | none | confirmed absent |

### 2.3 Confirmed gaps

- **GAP-33-1 — No forecasting.** Weather changes; nothing predicts it.
- **GAP-33-2 — No observation content.** No posts, watches, or instruments.
- **GAP-33-3 — No warnings the shelter issues.** Hooks exist; authored warnings
  and alert chains do not.
- **GAP-33-4 — No storm preparation or recovery content.**
- **GAP-33-5 — No ash-season operations.** Ashfall is a kind with a small
  effects file, not a season the shelter runs.
- **GAP-33-6 — No black-rain response content.**
- **GAP-33-7 — No weather records or almanac.**
- **GAP-33-8 — No seasonal readiness planning** shared across farming,
  building, and travel.
- **GAP-33-9 — No weather locations or instruments as items.**

### 2.4 Non-duplication statement

This expansion will **not** add a second weather, radiation, disease, travel,
construction, atmosphere, economy, or save system. It extends `WeatherSystem`
with authored forecasts and tracks, extends `WeatherEffectsCatalog` and
`seasonal_events.json` with content, uses `WeatherGate` for travel gating,
lands hardening in the live upgrade path, routes dose through `RadiationSystem`
and exposure through `DiseaseSystem.TryExpose`, and adds state only as additive
sub-objects of the existing weather store. No new save section.

---

## 3. DESIGN PILLARS AND TONE

### 3.1 Pillars

**Pillar 1 — The sky has patterns, not moods.** Weather is legible if someone
does the work of watching.

**Pillar 2 — A warning is a promise.** A forecast is only as good as the
shelter's willingness to act on it.

**Pillar 3 — Preparation is cheaper than repair.** Shutters and sandbags cost
hours; a collapsed wing costs a season.

**Pillar 4 — Ash is a long emergency.** Ash seasons are endurance, not a single
dramatic event.

**Pillar 5 — The almanac is memory.** What the shelter records this year saves
the next ten.

### 3.2 Tone calibration

| Element | Do | Do not |
|---|---|---|
| A forecast | Careful uncertainty | Prophecy |
| A storm | Noise, dark, damage | Disaster spectacle |
| Ash season | Grey endurance | Apocalypse porn |
| Black rain | Measurement and closure | Horror |
| A saved wing | Quiet competence | Triumph montage |
| A failed warning | Cost and grief | Blame game |

### 3.3 Content limits

- Storms are dangerous but never disaster tourism; deaths are authored and rare.
- Ash and fallout are handled factually, never luridly.
- No real-world storm names, agencies, or events copied.
- Fear is respected; panic is not simulated as a mechanic.
- Shelter design respects people with asthma, injuries, and age.

---

## 4. THE SKYWATCH WORLD

### 4.1 Interior rooms

- **`room_weather_post`** — instruments, logs, and the duty board.
- **`room_map_room`** — tracks, charts, and pins.
- **`room_warning_room`** — sirens, radio, and the alert board.
- **`room_instrument_bench`** — repair and calibration.
- **`room_filter_room`** — filter cleaning and swap.
- **`room_storm_store`** — shutters, sandbags, rope, and lanterns.
- **`room_archive_weather`** — the record shelves.
- **`room_ready_room`** — staging for seasonal prep.

### 4.2 Exterior locations

| ID | Name | Danger | Purpose |
|---|---|---|---|
| `loc_ridge_post` | The Ridge Post | 4 | Long-range observation |
| `loc_river_gauge` | The River Gauge | 3 | Flood and rain reading |
| `loc_ash_plain` | The Ash Plain | 5 | Ash depth and drift |
| `loc_wind_gap` | The Wind Gap | 4 | Pressure and onset |
| `loc_storm_shelter` | The Storm Shelter | 3 | Safe refuge on the road |
| `loc_lightning_tree` | The Lightning Tree | 5 | Strike scars and lessons |
| `loc_black_rain_basin` | The Black Basin | 6 | Fallout rain collection |
| `loc_snow_post` | The Snow Post | 4 | Depth, drift, and avalanche |
| `loc_signal_hill` | The Signal Hill | 3 | Warning flags and sirens |
| `loc_hail_field` | The Hail Field | 4 | Crop risk and netting tests |

All locations require valid item references and scanner registration.

### 4.3 The weather day

Morning reading, midday check, evening forecast, night watch. The skywatch runs
on the same check interval as the live engine (six hours), so the expansion's
rhythm matches the system it extends.

---

## 5. MAIN STORYLINE — "WHAT THE SKY TOOK"

### 5.1 Central conflict

The shelter has survived three years of weather by reacting to it. Then a storm
comes out of the wind gap with two hours' notice, and the east wing loses its
roof and a watch post loses a wall. The shelter's forecaster, **Juno Hale**,
had seen the signs for two days and had no authority to close the site, no
warning chain, and no place to post a forecast.

At the same time, the ash season arrives early. Filters clog, crops dim, and the
clinic sees the first lung cases. **Piet Marn** at the river gauge says the
black-rain season will be worse than last year, and the shelter has no plan for
it beyond closing the doors.

The expansion's question: **how much of the weather is fate, and how much is
everything the shelter failed to prepare for?**

### 5.2 Theme (unspoken)

**The storm is not the disaster. The unread warning is.**

### 5.3 Principal NPCs

| ID | Name | Role | Function |
|---|---|---|---|
| `npc_forecaster_juno_hale` | Juno Hale | Forecaster | Signs, models, and honest confidence |
| `npc_observer_piet_marn` | Piet Marn | Observer | Posts, gauges, and instruments |
| `npc_builder_marda_fell` | Marda Fell | Shelterwright | Shutters, bracing, and hardening |
| `npc_child_lark` | Lark | Child | First sky lessons and lookout duty |
| `npc_warden_galt` | Galt | Warden | Warnings, closures, and shelter drill |
| `npc_medic_pell` | Pell | Medic | Ash lung, exposure, and care |
| `npc_farmer_gale` | Gale | Grower | Seasonal prep and crop protection |
| `npc_radio_lin` | Lin | Radio operator | Warnings across the region |

### 5.4 Story beats (15)

1. **The Gap Wind.** A storm arrives with two hours' warning.
2. **The Roof.** The east wing fails; the shelter counts the cost.
3. **The Signs.** Juno explains what she saw and what she could not do.
4. **The Post.** A weather post is established on the ridge.
5. **The Gauge.** Piet's river records reveal a pattern.
6. **The Warning.** An alert chain is agreed and tested.
7. **The Almanac.** The first weather record is written.
8. **The Ash.** The ash season arrives early; filters clog.
9. **The Lungs.** Clinic cases force a mask and closure policy.
10. **The Shutters.** Marda leads hardening work.
11. **The Black Rain.** A fallout storm approaches; the shelter closes.
12. **The Fence.** Ash-fall and wind test the new hardening.
13. **The Wrong Call.** A forecast fails and the shelter pays.
14. **The Readiness.** Seasonal prep becomes a shared calendar.
15. **What the Sky Took.** Final disposition of the skywatch.

### 5.5 Branching choices (8)

| Choice | Options | Axis |
|---|---|---|
| Warning policy | strict / balanced / minimal | safety vs. disruption |
| Storm response | shelter-in-place / scatter / partial | risk model |
| Ash policy | close / filter-through / window work | endurance |
| Hardening | full / priority / none | cost vs. safety |
| Forecast | conservative / confident / silent | trust |
| Black rain | total closure / measured exposure | dose vs. work |
| Records | thorough / summary / none | future vs. time |
| Final | skywatch as institution / post / memory | identity |

### 5.6 Endings (5 + fade)

1. **The Read Sky** — forecasts are trusted, warnings are acted on, and weather
   costs the shelter almost nothing structural.
2. **The Hardened Shelter** — shutters, bracing, filters, and sandbags turn
   storms into noise.
3. **The Endured Season** — the ash season passes with damage but no deaths and
   a written record.
4. **The Wrong Year** — the shelter is surprised twice and loses a wing and a
   season.
5. **The Quiet Almanac** — the record is complete, boring, and worth more than
   any single storm story.
6. **Fade** — the shelter keeps reacting to weather it never predicts.

---

## 6. QUEST DESIGN

New IDs use prefix `quest_weather_`. Schema follows existing quest catalogs.

### 6.1 Main questline (15)

`quest_weather_gap_wind`, `quest_weather_roof`, `quest_weather_signs`,
`quest_weather_post`, `quest_weather_gauge`, `quest_weather_warning`,
`quest_weather_almanac`, `quest_weather_ash`, `quest_weather_lungs`,
`quest_weather_shutters`, `quest_weather_black_rain`, `quest_weather_fence`,
`quest_weather_wrong_call`, `quest_weather_readiness`,
`quest_weather_what_sky_took`.

### 6.2 Side quests (30)

**Observation (5)**
- `quest_weather_post_build` — establish a post
- `quest_weather_instruments` — make or repair instruments
- `quest_weather_watch_duty` — stand a weather watch
- `quest_weather_river_gauge` — read the river
- `quest_weather_night_sky` — night observation

**Forecast (5)**
- `quest_weather_signs_study` — study signs
- `quest_weather_model` — build a forecast model
- `quest_weather_confidence` — calibrate confidence
- `quest_weather_verify` — verify predictions
- `quest_weather_wrong_lesson` — learn from a miss

**Storms (5)**
- `quest_weather_storm_prep` — prepare for a storm
- `quest_weather_shutter_drill` — drill closures
- `quest_weather_shelter_drill` — shelter-in-place drill
- `quest_weather_damage` — assess and repair
- `quest_weather_rescue` — storm rescue

**Ash (5)**
- `quest_weather_filter_care` — filter maintenance
- `quest_weather_ash_clean` — ash clearing
- `quest_weather_ash_crops` — protect crops
- `quest_weather_ash_lungs` — clinic response
- `quest_weather_ash_route` — keep routes open

**Hardening (5)**
- `quest_weather_harden_roof` — roof bracing
- `quest_weather_harden_windows` — shutters and covers
- `quest_weather_harden_drains` — flood drainage
- `quest_weather_harden_store` — storm stores
- `quest_weather_harden_travel` — roadside shelters

**Records (5)**
- `quest_weather_log` — start the daily log
- `quest_weather_pattern` — find a pattern
- `quest_weather_almanac_write` — write the almanac
- `quest_weather_share_region` — share warnings
- `quest_weather_archive` — archive the records

### 6.3 Repeatable quests (8)

`quest_weather_repeat_read`, `quest_weather_repeat_log`,
`quest_weather_repeat_prep`, `quest_weather_repeat_filter`,
`quest_weather_repeat_repair`, `quest_weather_repeat_warn`,
`quest_weather_repeat_verify`, `quest_weather_repeat_clear`.

### 6.4 Dynamic hooks

Live events (weather kind changes, season windows, storms, gate closures,
radiation surveys, economy shocks, radio warnings) attach authored follow-ups
through existing seams. No new event bus.

### 6.5 Constraints

- Weather kinds and weights remain in `WeatherSystem`.
- Travel gating remains in `WeatherGate`.
- Interior comfort remains in `ShelterAtmosphereSystem`.
- Dose routes through `RadiationSystem`; exposure through
  `DiseaseSystem.TryExpose`.
- Hardening lands in the live upgrade path.
- Economy effects stay in `EconomyWeatherShockRules`.
- No forecast may claim certainty the engine does not have.

---

## 7. NEW GAMEPLAY SYSTEMS

### 7.1 `ForecastSystem` (new, `Ashfall.Core.World`)

**Owns:** signs, instruments, forecast models, confidence, and warnings.
**Consumes:** `WeatherSystem` state and weights, `SeasonWindowDef`,
`WeatherAtmosphereMap`, `FieldStudySystem` (observations), `SkillProgressionSystem`.
**Data:** `forecast_models.json`, `weather_signs.json`.
**Rules:** forecasts are authored from live weights with explicit confidence; a
forecast never changes the weather; being wrong is possible and informative.

### 7.2 `WarningSystem` (new, thin, `Ashfall.Core.World`)

**Owns:** alert levels, warning chains, sirens, notices, and radio calls.
**Consumes:** `ForecastSystem`, `WeatherGateRadioHooks`, `PatrolRadioHooks`,
`NoticeSystem` (Wave 4), `DutyRoster`. **Data:** `weather_warnings.json`.
**Rules:** warnings are issued, recorded, and reviewed; the shelter can choose
strict or minimal policy; failures are documented.

### 7.3 `StormResponseSystem` (new, `Ashfall.Core.World`)

**Owns:** preparation checklists, closure orders, shelter-in-place, damage
assessment, and recovery. **Consumes:** `WeatherSystem` storm events,
`ShelterAssignmentSystem`, `RouteInfrastructureSystem` (closures through
`WeatherGate`), `Inventory`. **Data:** `storm_tracks.json`,
`hardening_plans.json`. **Rules:** preparation consumes real labor and
supplies; damage is authored from exposure and hardening; recovery is staged.

### 7.4 `AshSeasonSystem` (new, `Ashfall.Core.World`)

**Owns:** ash operations: filters, clearing, crop protection, clinic response,
and route keeping. **Consumes:** `WeatherSystem` ashfall kind,
`ShelterAtmosphereSystem` air purity, filter and scrubber systems,
`Farming`/`GreenhouseSystem`, `MedicalPipelineCoordinator`.
**Data:** `ash_seasons.json`. **Rules:** filters wear with real load; ash depth
is tracked; the season has phases with escalating demands.

### 7.5 `WeatherRecordSystem` (new, `Ashfall.Core.Narrative`)

**Owns:** daily logs, pattern records, the almanac, and regional sharing.
**Consumes:** `WeatherSystem` log, `PressSystem` (Wave 4) for printing,
`SchoolingSystem` for teaching. **Data:** `weather_records.json`.
**Rules:** records are only as good as their coverage; patterns emerge from
authored truth; the almanac becomes a real item.

### 7.6 `SeasonalReadinessSystem` (new, thin, `Ashfall.Core.World`)

**Owns:** readiness calendars shared with farming, building, and travel.
**Consumes:** `SeasonWindowDef`, `CurriculumSystem` where present,
`BuildWorksSystem`, `HuntingSystem`, `Inventory`. **Data:**
`readiness_calendars.json`. **Rules:** readiness is a shared plan, not a bonus;
each season has authored preparation tasks with real effects.

### 7.7 Systems explicitly not added

- No second weather, radiation, disease, travel, or construction system.
- No weather control or cloud seeding.
- No catastrophe spectacle or wipe events.
- No new currency.
- No new RNG stream.
- No new save section.

---

## 8. DATA CATALOG SPECIFICATION

All catalogs snake_case, integer `schema_version: 1`, validated and scanner-registered.

### 8.1 `forecast_models.json` (new)

```json
{
  "schema_version": 1,
  "models": [
    {
      "model_id": "model_gap_wind",
      "display_name": "Gap Wind Model",
      "requires": ["Post Ridge", "gauge river"],
      "lead_hours": 12,
      "confidence_base": 0.55,
      "inputs": ["pressure", "wind", "cloud"],
      "misses": ["calm_shift", "front_split"],
      "tags": ["storm", "regional"]
    }
  ]
}
```

### 8.2 `weather_signs.json` (new)

Signs: kind, observation, confidence weight, and season.

### 8.3 `storm_tracks.json` (new)

Tracks: origin, path, sectors, hours, intensity, and affected gates.

### 8.4 `weather_warnings.json` (new)

Warnings: level, trigger, channels, duration, and actions.

### 8.5 `ash_seasons.json` (new)

Ash seasons: start window, depth projection, filter load, phases, and demands.

### 8.6 `hardening_plans.json` (new)

Plans: structure, risk, materials, labor, days, and live upgrade target.

### 8.7 `weather_records.json` (new)

Records: day, kind, observations, confidence, verification, and pattern tag.

### 8.8 `readiness_calendars.json` (new)

Calendars: season, tasks by domain, owners, and deadlines.

### 8.9 Extend `weather_seasons.json`, `weather_effects.json`, `seasonal_events.json`

Additional season windows, effect rows, and authored seasonal events consistent
with the live schemas.

### 8.10 Items

New items appended to `items.json`: `item_barometer`, `item_thermometer`,
`item_rain_gauge`, `item_wind_vane`, `item_weather_log`, `item_storm_shutter`,
`item_sandbag`, `item_ash_mask`, `item_filter_cartridge`, `item_storm_lantern`,
`item_signal_flags`, `item_storm_rope`, `item_lightning_rod`,
`item_hail_netting`, `item_hygrometer`, `item_cloud_chart`.

---

## 9. SAVE, DETERMINISM, AND PERSISTENCE

### 9.1 Ownership

The weather state and gate state remain the live save owners. New sub-objects
(forecast models, warnings, storm tracks, ash operations, records, readiness)
are additive inside them. No new save section.

### 9.2 State to persist

- Observation coverage and instrument condition.
- Forecast model progress and confidence.
- Warning history and verification.
- Storm tracks and damage records.
- Ash-season phase and filter load.
- Weather records and almanac entries.
- Readiness calendars and completed tasks.

### 9.3 Determinism

- Weather generation stays in `WeatherSystem` with its seeded path.
- Forecasts read the live state and authored weights; they never reroll weather.
- Damage is deterministic from exposure, hardening, and authored risk.
- Paired replay hashes must match.

### 9.4 Migration

Legacy saves load with existing weather, gate, and hardening state untouched; no
forecast, warning, storm, ash, record, or readiness state exists until started.
Existing storms and gates keep working.

### 9.5 Checksum

Invariant-culture floats; integer confidence permille where the schema allows.

---

## 10. UI, ACCESSIBILITY, AND PRESENTATION

### 10.1 Surfaces

| Surface | Purpose | Owner |
|---|---|---|
| `SkywatchPanel` (new) | Current weather and outlook | `SkywatchHostSession` |
| `ForecastPanel` (new) | Signs, models, confidence | same |
| `WarningPanel` (new) | Alert levels and history | same |
| `StormPanel` (new) | Tracks, prep, damage | same |
| `AshPanel` (new) | Season phases, filters | same |
| `RecordPanel` (new) | Logs and almanac | same |
| `ReadinessPanel` (new) | Seasonal tasks | same |

### 10.2 Accessibility and honesty

- Panels show live state and expose existing commands only.
- Forecasts display explicit confidence, never false certainty.
- Warnings state exactly what changes: closures, work, filters, care.
- Keyboard/controller close/back preserved; focus maintained on refresh.
- Warning alarms have visual equivalents, never audio-only.

### 10.3 Presentation

Audio cues appended to `audio_cues.json`: wind rising, rain on metal, ash
tapping glass, a siren, shutters locking, a gauge ticking. No cue is required;
text carries meaning.

---

## 11. INTEGRATION SEAMS

| Existing system | Attachment |
|---|---|
| `WeatherSystem` | Seasons, weights, kinds, ticks |
| `WeatherEffectsCatalog` | Authored effects |
| `WeatherGate` / `WeatherGateCatalog` | Travel gating |
| `WeatherAtmosphereMap` | Spatial weather |
| `YearOfAshStormCatalog` | Named storms |
| `EconomyWeatherShockRules` | Economy effects |
| `ShelterAtmosphereSystem` | Interior air and comfort |
| `RadiationSystem` | Fallout rain dose |
| `DiseaseSystem` | Exposure from ash and rain |
| `MedicalPipelineCoordinator` | Ash lung and exposure care |
| `Farming` / `GreenhouseSystem` | Crop protection |
| `Inventory` / live upgrades | Hardening and supplies |
| `NoticeSystem` (Wave 4) | Authored warnings |
| `PressSystem` (Wave 4) | Almanac printing |
| `EpilogueChronicleBuilder` | Weather milestones |

---

## 12. TECHNICAL IMPLEMENTATION PLAN

### 12.1 Phase order

**Phase 0 — Premise re-audit.** Confirm `WeatherSystem`, effects catalog, gates,
atmosphere map, storm catalog, economy rules, radio hooks, hardening catalog,
and data sizes. Record file:line; change nothing.

**Phase 1 — Data + validators.** Author forecast models, signs, tracks,
warnings, ash seasons, hardening plans, records, readiness calendars; extend
season/effect/event catalogs; append items. Register validators and scanner.

**Phase 2 — Pure Core.** `ForecastSystem`, `WarningSystem`,
`StormResponseSystem`, `AshSeasonSystem`, `WeatherRecordSystem`,
`SeasonalReadinessSystem`.

**Phase 3 — Persistence.** Additive sub-objects, migration, round-trip,
determinism.

**Phase 4 — Host + CLI.** `SkywatchHostSession`, selftest coverage, fresh
journey.

**Phase 5 — UI.** New panels with lifecycle and accessibility.

**Phase 6 — Content.** Locations, rooms, NPCs, quests, items, prose, audio.

**Phase 7 — Balance.** 720-day soak: storms, ash seasons, black rain, hardening,
and warning accuracy.

**Phase 8 — Verification and closeout.**

### 12.2 Content volume

| Content | Count |
|---|---|
| Forecast models | 15 |
| Weather signs | 30 |
| Storm tracks | 12 |
| Warnings | 15 |
| Ash seasons | 6 |
| Hardening plans | 15 |
| Weather records | 40 |
| Readiness calendars | 4 |
| Items | 16 |
| Locations | 10 |
| Rooms | 8 |
| NPCs | 8 |
| Main quests | 15 |
| Side quests | 30 |
| Repeatable | 8 |
| Endings | 5 + fade |
| Prose estimate | 55,000–70,000 words |

### 12.3 Risks

| Risk | Severity | Mitigation |
|---|---|---|
| Second weather system | Critical | Extend `WeatherSystem` |
| False certainty | High | Confidence displayed always |
| Storm as wipe event | High | Authored damage, recovery |
| Hazard authority conflict | Critical | Radiation/disease keep dose |
| Warning fatigue | Medium | Authored trust and cost |
| Ash season grind | Medium | Phases and relief |
| Determinism break | Low | Live seeded generation |
| Content overrun | Medium | Budget §13 |

---

## 13. APPENDIX A — AUTHORING BUDGET

| File | Rows | Est. words |
|---|---|---|
| `forecast_models.json` | 15 | 4,000 |
| `weather_signs.json` | 30 | 5,000 |
| `storm_tracks.json` | 12 | 3,500 |
| `weather_warnings.json` | 15 | 3,500 |
| `ash_seasons.json` | 6 | 3,000 |
| `hardening_plans.json` | 15 | 4,000 |
| `weather_records.json` | 40 | 6,000 |
| `readiness_calendars.json` | 4 | 2,500 |
| Quest objectives | 53 quests | 15,000 |
| NPC prose | 8 NPCs | 7,000 |
| Location prose | 10 | 3,500 |
| Item descriptions | 16 | 2,500 |
| Ending prose | 6 | 3,000 |
| **Total** | | **~64,500** |

---

## 14. APPENDIX B — RISK REGISTER

| ID | Risk | Likelihood | Impact | Mitigation |
|---|---|---|---|---|
| R33-1 | Second weather system | Low | Critical | One weather owner |
| R33-2 | False certainty | Med | High | Confidence model |
| R33-3 | Storm wipe | Low | High | Damage and recovery |
| R33-4 | Hazard authority conflict | Med | Critical | Live pipelines |
| R33-5 | Warning fatigue | Med | Med | Authored tradeoffs |
| R33-6 | Ash grind | Med | Med | Season phases |
| R33-7 | Forecast useless | Med | Med | Warnings change actions |
| R33-8 | Determinism | Low | High | Live weather seed |
| R33-9 | Content overrun | Med | Med | Budget §13 |
| R33-10 | Travel authority conflict | Med | High | `WeatherGate` only |

---

## 15. APPENDIX C — OPEN DECISIONS REQUIRING A FOREMAN SIGNATURE

1. **Can forecasts ever be wrong?** Recommended: yes, with displayed confidence
   and authored miss modes.
2. **Do warnings interrupt work?** Recommended: yes, as policy with real
   disruption costs.
3. **Can the shelter ignore a warning deliberately?** Recommended: yes, and the
   record shows it.
4. **Is black rain a full closure or a measured exposure choice?** Recommended:
   measured, with dose through the live radiation pipeline.
5. **Does the almanac grant a mechanical benefit or only information?**
   Recommended: information and readiness tasks, not a passive bonus.

---

## 17. APPENDIX D — FORECAST MODEL TABLE (15 MODELS)

| # | Model | Requires | Lead | Confidence | Miss modes |
|---|---|---|---|---|---|
| 1 | Gap Wind | ridge post | 12h | 55% | calm shift |
| 2 | River Rise | gauge | 18h | 65% | slow thaw |
| 3 | Ash Drift | ash plain | 24h | 50% | wind change |
| 4 | Front Line | barometer | 24h | 60% | split front |
| 5 | Snow Depth | snow post | 12h | 70% | warm layer |
| 6 | Hail Field | cloud chart | 6h | 50% | dry layer |
| 7 | Black Rain | basin + survey | 24h | 60% | high cloud |
| 8 | Fog Bank | hygrometer | 6h | 75% | breeze |
| 9 | Wind Shift | vane + log | 12h | 65% | local eddy |
| 10 | Pressure Fall | barometer | 18h | 70% | stuck system |
| 11 | Cloud Build | chart | 8h | 55% | dissipate |
| 12 | Season Turn | records | 7d | 60% | late year |
| 13 | Storm Path | tracks | 24h | 50% | path shift |
| 14 | Ash End | depth records | 48h | 55% | second plume |
| 15 | Black Season | basin records | 7d | 60% | early rain |

Every model has an honest confidence ceiling and at least one authored miss
mode. Forecasting is not prophecy; it is a probability the shelter acts on with
its eyes open.

---

## 18. APPENDIX E — WEATHER SIGN TABLE (30 SIGNS)

| # | Sign | Observation | Weight | Season |
|---|---|---|---|---|
| 1 | Falling Pressure | barometer | high | all |
| 2 | Ring Around Sun | eye | medium | all |
| 3 | Wind Backing | vane | high | all |
| 4 | Dead Calm | senses | medium | summer |
| 5 | Cloud Shelf | eye | high | summer |
| 6 | Green Sky | eye | high | summer |
| 7 | Ash Dimming | eye | high | ash |
| 8 | Grit Taste | senses | medium | ash |
| 9 | Filter Whistle | hearing | high | ash |
| 10 | Bird Silence | senses | medium | all |
| 11 | Animal Descent | observation | high | winter |
| 12 | River Colour | gauge | medium | spring |
| 13 | River Sound | hearing | medium | spring |
| 14 | Snow Gloom | eye | medium | winter |
| 15 | Hail Smell | senses | low | summer |
| 16 | Lightning Flash | eye | high | summer |
| 17 | Static Hiss | radio | high | storm |
| 18 | Long Echo | hearing | low | winter |
| 19 | Smoke Lay | eye | medium | all |
| 20 | Door Draft | senses | medium | winter |
| 21 | Lamp Flame | eye | medium | all |
| 22 | Window Rattle | hearing | medium | storm |
| 23 | Knee Ache | body | low | all |
| 24 | Basin Sheen | eye | high | black rain |
| 25 | Soot Streak | eye | high | ash |
| 26 | Fowl Stillness | observation | medium | all |
| 27 | Dog Restless | companion | low | storm |
| 28 | Thermometer Drop | instrument | high | winter |
| 29 | Hygrometer Rise | instrument | medium | fog |
| 30 | Pheasant Call | hearing | low | autumn |

Signs are the raw material of forecasting: what a watcher notices, records, and
learns to weigh. The list rewards players who spend shifts outdoors looking at
the sky instead of a menu.

---

## 19. APPENDIX F — STORM TRACK TABLE (12 TRACKS)

| # | Track | Origin | Path | Hours | Gates affected |
|---|---|---|---|---|---|
| 1 | Gap Storm | wind gap | shelter east | 8 | ridge, east road |
| 2 | River Flood | upstream | river bend | 36 | river gate |
| 3 | Ash Wall | ash plain | whole region | 48 | all open |
| 4 | Blizzard Line | north | shelter north | 24 | north road |
| 5 | Hail Cell | hills | fields | 4 | field gate |
| 6 | Black Rain Front | basin | river valley | 30 | river, south |
| 7 | Ice Fog Bank | river | low ground | 18 | river gate |
| 8 | Dry Lightning | ridge | forest | 6 | forest gate |
| 9 | Funnel Watch | gap | fields | 2 | field road |
| 10 | Second Plume | ash plain | region | 72 | all |
| 11 | Melt Flood | hills | river | 60 | south roads |
| 12 | Long Winter | global | region | season | all |

Tracks make storms physical: they have origins, paths, durations, and specific
gates. The shelter can map them, warn ahead of them, and rebuild behind them.

---

## 20. APPENDIX G — WARNING TABLE (15 WARNINGS)

| # | Warning | Level | Trigger | Channels | Duration |
|---|---|---|---|---|---|
| 1 | Wind Advisory | note | forecast wind | board | 12h |
| 2 | Storm Watch | watch | track formed | radio, board | 24h |
| 3 | Storm Warning | warning | 12h out | siren, radio | 12h |
| 4 | Shelter Call | emergency | 2h out | siren, runners | 6h |
| 5 | River Watch | watch | gauge rise | radio | 24h |
| 6 | Flood Warning | warning | gauge crest | siren | 12h |
| 7 | Ash Advisory | note | ash drift | board | 48h |
| 8 | Ash Warning | warning | depth rise | siren | season |
| 9 | Filter Order | order | filter load | board | season |
| 10 | Black Rain Watch | watch | forecast | radio | 12h |
| 11 | Black Rain Closure | closure | confirmed | siren | 24h |
| 12 | Hail Watch | watch | cell formed | board | 6h |
| 13 | Blizzard Warning | warning | line formed | siren | 24h |
| 14 | Route Closure | order | gate unsafe | radio, board | varies |
| 15 | All Clear | note | storm passed | board | 6h |

Warnings are the expansion's promise to its people: a date, a level, a channel,
and a duration. A warning the shelter issues and then ignores costs more trust
than no warning at all.

---

## 21. APPENDIX H — ASH SEASON TABLE (6 SEASONS)

| # | Season | Window | Depth | Filter load | Phases |
|---|---|---|---|---|---|
| 1 | Early Ash | spring | light | low | settle, clear |
| 2 | Deep Ash | summer | heavy | high | drift, filter, clinic |
| 3 | Second Plume | summer | extreme | critical | close, ration air |
| 4 | Ash End | autumn | falling | medium | clear, repair |
| 5 | Grey Winter | winter | frozen | low | frozen ash, melt risk |
| 6 | Black Season | winter | rain-mixed | high | closure, dose |

Ash seasons are the expansion's endurance test: they are long, they wear the
shelter's equipment, and they end with clearing work. The phases make the grind
legible and give the player decisions instead of a timer.

---

## 22. APPENDIX I — HARDENING PLAN TABLE (15 PLANS)

| # | Plan | Risk | Materials | Labor | Days | Target |
|---|---|---|---|---|---|---|
| 1 | Roof Bracing | wind | timber, nails | 2 | 3 | roof |
| 2 | Shutter Fit | wind | wood, hinges | 2 | 4 | windows |
| 3 | Door Brace | wind | timber | 1 | 1 | doors |
| 4 | Sandbag Ring | flood | bags, sand | 3 | 2 | low openings |
| 5 | Drain Clear | flood | tools | 2 | 3 | grounds |
| 6 | Filter Housing | ash | metal, gasket | 2 | 4 | intake |
| 7 | Intake Hood | ash | metal | 2 | 3 | intake |
| 8 | Chimney Cap | ash | metal | 1 | 2 | stacks |
| 9 | Storm Store | all | crates | 1 | 2 | stores |
| 10 | Roadside Shelter | travel | timber | 3 | 6 | route |
| 11 | Lightning Rod | fire | copper | 2 | 3 | roof |
| 12 | Hail Netting | hail | net, poles | 2 | 4 | fields |
| 13 | Grain Anchor | wind | rope | 1 | 2 | stores |
| 14 | Bunker Hatch | all | metal, seal | 2 | 5 | shelter |
| 15 | Gauge House | flood | timber | 2 | 3 | river |

Hardening is preparation made physical, and every plan lands in the live upgrade
path. The expansion's argument is simple: an hour of bracing beats a season of
repair.

---

## 23. APPENDIX J — WEATHER RECORD TABLE (40 RECORDS — REPRESENTATIVE)

| # | Record | Kind | Confidence | Verification | Pattern |
|---|---|---|---|---|---|
| 1 | First Entry | clear | n/a | n/a | none |
| 2 | Pressure Drop | storm | medium | confirmed | pre-storm |
| 3 | Gap Wind | storm | low | confirmed | gap pattern |
| 4 | Calm After | clear | medium | miss | false calm |
| 5 | Rain Line | rain | high | confirmed | front |
| 6 | Cold Snap | winter | medium | confirmed | polar |
| 7 | Thaw Pause | winter | low | miss | false thaw |
| 8 | Ash First | ash | high | confirmed | ash season |
| 9 | Ash Depth | ash | high | measured | drift |
| 10 | Filter Clog | ash | high | logged | load curve |
| 11 | Second Plume | ash | medium | confirmed | plume |
| 12 | Grey Clear | ash | low | miss | false end |
| 13 | Black Watch | black rain | medium | confirmed | basin |
| 14 | Basin Sheen | black rain | high | confirmed | precursor |
| 15 | Rain Dose | black rain | high | surveyed | season |
| 16 | Hail Cell | hail | medium | confirmed | summer |
| 17 | Green Sky | storm | medium | confirmed | pre-storm |
| 18 | Funnel Watch | storm | low | miss | gap |
| 19 | Blizzard Line | winter | high | confirmed | north |
| 20 | Snow Depth | winter | high | measured | drift |
| 21 | Ice Fog | fog | high | confirmed | river |
| 22 | Melt Flood | flood | medium | confirmed | spring |
| 23 | River Crest | flood | high | measured | flood |
| 24 | Dry Lightning | fire | medium | confirmed | fire risk |
| 25 | Forest Smoke | fire | high | confirmed | smoke |
| 26 | Fowl Still | all | low | n/a | folk sign |
| 27 | Knee Ache | all | low | n/a | folk sign |
| 28 | Radio Static | storm | medium | confirmed | storm |
| 29 | Long Echo | winter | low | n/a | cold |
| 30 | Season Turn | all | medium | confirmed | calendar |
| 31 | Late Frost | spring | medium | confirmed | frost |
| 32 | Early Turn | autumn | low | miss | calendar |
| 33 | Snow Gloom | winter | medium | confirmed | snow |
| 34 | Door Draft | winter | medium | confirmed | wind |
| 35 | Lamp Blue | storm | low | n/a | folk sign |
| 36 | Ash End | ash | medium | confirmed | season end |
| 37 | Black End | black rain | medium | confirmed | season end |
| 38 | Repair Season | all | n/a | n/a | work |
| 39 | Pattern Found | all | high | verified | local rule |
| 40 | Almanac Year | all | high | archived | annual |

Records are how a shelter stops starting from zero every spring. The log entries
are small, and the almanac they become is the expansion's real reward.

---

## 24. APPENDIX K — READINESS CALENDAR TABLE (4 CALENDARS)

| # | Season | Farming | Building | Travel | Shelter |
|---|---|---|---|---|---|
| 1 | Spring | plant, hail nets | cure, inspect | route check | thaw repairs |
| 2 | Summer | water, shade | harden, paint | storm watch | ash filters |
| 3 | Autumn | harvest, store | roof work | stock routes | storm prep |
| 4 | Winter | greenhouse | indoor work | closures | fuel, closures |

Readiness is shared across domains: the grower, the builder, the driver, and the
quartermaster all prepare for the same season, and the calendar makes the
interlock visible.

---

## 25. APPENDIX L — MAIN QUESTLINE STAGE DETAIL

| Quest | Stages | Objective kernel |
|---|---|---|
| `quest_weather_gap_wind` | 4 | Storm arrives; count the warning time |
| `quest_weather_roof` | 4 | Assess and repair the wing |
| `quest_weather_signs` | 4 | Juno explains signs and authority |
| `quest_weather_post` | 5 | Build the ridge post |
| `quest_weather_gauge` | 4 | Read and record the river |
| `quest_weather_warning` | 5 | Agree and test the alert chain |
| `quest_weather_almanac` | 4 | Start the weather record |
| `quest_weather_ash` | 5 | Ash season operations begin |
| `quest_weather_lungs` | 4 | Clinic cases force policy |
| `quest_weather_shutters` | 4 | Lead hardening work |
| `quest_weather_black_rain` | 5 | Forecast, close, measure |
| `quest_weather_fence` | 4 | Storm tests the hardening |
| `quest_weather_wrong_call` | 5 | A miss costs the shelter |
| `quest_weather_readiness` | 4 | Build the seasonal calendar |
| `quest_weather_what_sky_took` | 3 | Final disposition |

---

## 26. APPENDIX M — NPC DOSSIERS (BRIEF)

**Juno Hale** — forecaster. Reads pressure and cloud the way other people read
faces. Furious not at storms but at the shelter's refusal to act on what she
saw. Believes a forecast is a promise the shelter keeps or breaks.

**Piet Marn** — observer. Keeps the river gauge with ritual precision and talks
to the instrument when it drifts. His records are boring and they save lives.

**Marda Fell** — shelterwright. Braces roofs, fits shutters, and believes
preparation is the most honest work in the shelter because nobody praises it.

**Lark** — child. Stands lookout because Lark is small and can climb the post,
and learns the sky from Juno without knowing that is what is happening.

**Galt** — warden. Closes sites, clears streets, and carries the unpopular
weight of warning fatigue. Galt's quiet competence is the alert chain's spine.

**Pell** — medic. Sees ash lung, dose exposure, and cold injuries first and has
the least patience for delay. Pell turns symptoms into policy.

**Gale** — grower. Reads weather like a rival. Loses a season when the forecast
is ignored and gains one when it is not.

**Lin** — radio operator. Sends warnings across the region and hears the replies
from settlements that did not act in time.

---

## 27. APPENDIX N — LOCATION DETAIL

- **The Ridge Post** — long-range observation and the wind's first arrival.
- **The River Gauge** — flood reading, records, and a platform that moves.
- **The Ash Plain** — depth, drift, and a horizon like a wall.
- **The Wind Gap** — pressure funnel and the storm's favourite door.
- **The Storm Shelter** — roadside refuge for anyone caught out.
- **The Lightning Tree** — strike scars and a lesson about rods.
- **The Black Basin** — fallout rain collection and survey sampling.
- **The Snow Post** — depth stakes, drift marks, and avalanche watch.
- **The Signal Hill** — flags, sirens, and line-of-sight warnings.
- **The Hail Field** — netting tests and crop damage samples.

---

## 28. APPENDIX O — FORECAST AND CONFIDENCE MODEL

| Evidence | Confidence shift | Notes |
|---|---|---|
| Instrument reading | +10% | calibrated only |
| Repeated sign | +5% each | capped |
| Historic pattern | +15% | from records |
| Regional report | +10% | radio |
| Stale data | −20% | over 12h |
| Contradiction | −15% | conflicting signs |
| New watcher | −10% | skill |
| Model mismatch | −25% | wrong regime |

Forecast confidence is transparent arithmetic, not a hidden roll. The shelter can
see why a forecast is trusted or doubted, which is what makes a miss a lesson
instead of a betrayal.

---

## 29. APPENDIX P — STORM DAMAGE MODEL

| Exposure | Hardening | Damage | Recovery |
|---|---|---|---|
| low | none | minor | days |
| low | full | none | n/a |
| medium | none | moderate | week |
| medium | partial | light | days |
| high | none | severe | season |
| high | partial | moderate | weeks |
| high | full | light | days |
| extreme | any | severe+ | season+ |

Damage is deterministic from exposure and hardening, so preparation always pays.
The extreme band exists to keep weather dangerous at the margin, but the shelter
can reduce most storms to noise with planning.

---

## 30. APPENDIX Q — WORKED 720-DAY WEATHER SCENARIO

**Days 1–30.** Gap storm; roof lost; warning chain agreed; ridge post built.

**Days 31–90.** River gauge yields a flood pattern; first forecast verified; the
first weather record is written.

**Days 91–150.** Ash season arrives early. Filters clog; clinic cases rise; the
mask policy is written and clearing begins.

**Days 151–220.** Shutters, bracing, and intake hoods installed. A second storm
is forecast and weathered with light damage.

**Days 221–300.** Black-rain watch; basin sampling; closure drills. The first
almanac draft is written.

**Days 301–420.** Deep ash and a second plume test the filter housing; the
shelter closes intake for three days and clears after.

**Days 421–540.** Season turn; readiness calendar adopted across farming,
building, and travel. Two forecasts miss and are logged.

**Days 541–660.** Winter storms and ice fog; roadside shelter saves a convoy;
the river gauge records a melt flood precisely.

**Days 661–720.** The almanac is finished, printed, and shelved. The shelter
reads its own weather year and prepares for the next.

---

## 31. APPENDIX R — VIGNETTE (TONE SAMPLE)

> Juno climbs to the ridge post before sunrise and reads the barometer twice and
the wind once, and writes three numbers in the log, and the numbers agree with
each other, which is the whole reason she trusts them.

> Marda drives the last shutter pin home and steps back and looks at the east
> wall and says nothing, because a wall that is ready is a wall that has nothing
> to say.

> At the intake, Pell changes the filter and holds the old one up to the light
> and can see the grey packed into it, and thinks about the two patients
> downstairs, and changes the next filter early.

---

## 32. APPENDIX S — FAILURE MODES AND RECOVERY

| Failure | Effect | Recovery |
|---|---|---|
| Missed forecast | damage | fix model, log miss |
| Ignored warning | injury, loss | policy change |
| Warning fatigue | no response | trim warnings |
| Filter failure | air quality | swap, close |
| Flood damage | stores lost | drain, raise |
| Roof loss | shelter loss | brace, rebuild |
| Black rain exposure | dose | treat, close |
| Ash crop loss | food loss | nets, greenhouse |
| Route closure | trade loss | wait, shelter, reroute |
| Records lost | pattern loss | rebuild, teach |

No failure is a game over. The deepest failure is a shelter that keeps
rebuilding the same wall every year because nobody wrote down what the sky did.

---

## 33. APPENDIX T — CONTENT REVIEW CHECKLIST

- [ ] `WeatherSystem` remains the weather authority.
- [ ] `WeatherGate` keeps travel gating.
- [ ] Dose routes through `RadiationSystem`; exposure through `DiseaseSystem`.
- [ ] Interior air and comfort stay in `ShelterAtmosphereSystem`.
- [ ] Hardening lands in the live upgrade path.
- [ ] Economy effects stay in `EconomyWeatherShockRules`.
- [ ] Forecasts display honest confidence.
- [ ] No storm is an unrecoverable wipe.
- [ ] Ash and fallout prose is factual, not lurid.
- [ ] Save additions are additive and legacy-neutral.
- [ ] Determinism uses the live weather seed only.

---

## 34. APPENDIX U — GLOSSARY

- **Sign** — an observed indicator with a confidence weight.
- **Model** — a forecast method with requirements and lead time.
- **Confidence** — displayed probability, never certainty.
- **Watch / Warning / Emergency** — alert levels.
- **Track** — a storm's path, duration, and affected gates.
- **Ash season** — a long ashfall period with phases.
- **Black rain** — fallout-laced rain requiring closure and measurement.
- **Hardening** — physical preparation through live upgrades.
- **Record** — a logged observation with verification.
- **Almanac** — the compiled year of weather.

---

## 35. APPENDIX V — INTEGRATION MATRIX

| System | Reads | Writes | Never writes |
|---|---|---|---|
| `WeatherSystem` | seasons | kinds, weights | forecasts |
| `ForecastSystem` | signs | models, confidence | weather |
| `WarningSystem` | triggers | alerts | weather |
| `StormResponseSystem` | tracks | prep, damage | weather |
| `AshSeasonSystem` | ash kind | phases, filters | atmosphere |
| `WeatherRecordSystem` | log | records, almanac | weather |
| `SeasonalReadinessSystem` | seasons | calendars | domains |
| `WeatherGate` | weather | closures | travel |
| `RadiationSystem` | rain | dose | weather |
| `DiseaseSystem` | exposure | illness | weather |
| `ShelterAtmosphereSystem` | filters | air purity | weather |
| `EconomyWeatherShockRules` | weather | shocks | weather |
| `NoticeSystem` | warnings | notices | weather |
| `PressSystem` | almanac | print | weather |
| `EpilogueChronicleBuilder` | milestones | chronicle | — |

---

## 36. APPENDIX W — DATA SCHEMA DETAIL (NEW CATALOGS)

**`forecast_models.json`** — `model_id`, `display_name`, `requires[]`,
`lead_hours`, `confidence_base`, `inputs[]`, `misses[]`, `tags`.

**`weather_signs.json`** — `sign_id`, `display_name`, `observation`,
`weight`, `season`, `tags`.

**`storm_tracks.json`** — `track_id`, `display_name`, `origin`, `path[]`,
`hours`, `intensity`, `gates[]`, `tags`.

**`weather_warnings.json`** — `warning_id`, `display_name`, `level`,
`trigger`, `channels[]`, `duration`, `actions[]`, `tags`.

**`ash_seasons.json`** — `season_id`, `display_name`, `window`, `depth`,
`filter_load`, `phases[]`, `demands[]`, `tags`.

**`hardening_plans.json`** — `plan_id`, `display_name`, `risk`, `materials[]`,
`labor`, `days`, `upgrade_target`, `tags`.

**`weather_records.json`** — `record_id`, `day`, `kind`, `observations[]`,
`confidence`, `verification`, `pattern`, `tags`.

**`readiness_calendars.json`** — `calendar_id`, `season`, `tasks[]`,
`owners[]`, `deadlines[]`, `tags`.

All new catalogs carry `schema_version: 1` and fail the integrity gate on missing
or duplicate IDs, invalid references, or out-of-range numbers.

---

## 37. APPENDIX X — MEASUREMENT AND TELEMETRY

| Metric | Purpose | Source |
|---|---|---|
| Forecast accuracy | trust | ForecastSystem |
| Lead time | warning value | ForecastSystem |
| Warning response rate | alert health | WarningSystem |
| Storm damage | hardening effect | StormResponse |
| Ash filter load | endurance | AshSeason |
| Black rain closures | dose control | AshSeason |
| Record coverage | pattern quality | WeatherRecord |
| Readiness completion | preparation | Readiness |
| Route closures | travel cost | WeatherGate |
| Almanac entries | memory | WeatherRecord |

Telemetry is diagnostic only; it never gates content and never becomes a hidden
score.

---

## 38. APPENDIX Y — IMPLEMENTATION CHECKLIST

- [ ] Phase 0 premise re-audit recorded with file:line evidence.
- [ ] Phase 1 catalogs authored and registered with validators and scanner.
- [ ] Phase 2 Core systems are pure, engine-free, and deterministic.
- [ ] Phase 3 save additions are additive, legacy-neutral, and round-trip tested.
- [ ] Forecasts never mutate weather state.
- [ ] Triad parity (Setup / Save / Flush) holds for every new state.
- [ ] Phase 4 host session exposes live state and real commands only.
- [ ] Phase 5 panels pass bind/unbind/rebind, focus, contrast, and scaling checks.
- [ ] Phase 6 content passes the review checklist in §33.
- [ ] Phase 7 soak shows storms, ash, black rain, and warning accuracy.
- [ ] Phase 8 data integrity and content utilization selftests pass.
- [ ] No parallel weather, hazard, travel, or construction system exists.

---

## 39. APPENDIX Z — OPEN QUESTIONS FOR REVIEW

1. Does a wrong forecast erode trust, and can trust be rebuilt?
2. Can the shelter seed clouds or otherwise modify weather? Recommended: no.
3. Do ash seasons repeat annually with variance, or arrive by authored script?
4. Does black rain create a visible dose zone the shelter can map?
5. Should warning channels include radio even when the radio is damaged?
6. Can forecasts be sold or shared with other settlements?
7. Do animals reliably precede weather, or are folk signs deliberately low
   confidence?
8. Should the almanac be a usable item that improves future forecast models?

None of these may be decided unilaterally; each changes balance and tone.

---

## 40. APPENDIX AA — CROSS-WAVE HOOKS

| Wave | Expansion | Hook |
|---|---|---|
| 1 | 12 The Second Generation | Weather lessons for children |
| 1 | 13 The Faithful | Storm rites and thanks for shelter |
| 1 | 14 Above the Ash | Flight weather minima |
| 1 | 15 The Deep Root | Frost, hail, and crop protection |
| 1 | 16 The Rebuilt Body | Cold and pressure on prosthetics |
| 2 | 17 The Long Evening | Storm nights and indoor culture |
| 2 | 18 The Underneath | Seepage, air, and seismic weather |
| 2 | 19 The Bitter Air | Ash, masks, and air quality |
| 2 | 20 The Quiet Hand | Weather as cover for operations |
| 2 | 21 The Grid | Storm load and outage risk |
| 3 | 22 The Clean Flow | Flood and runoff response |
| 3 | 23 The Alarm | Storm alerts and rescue |
| 3 | 24 The Long Goodbye | Cold and the elderly |
| 3 | 25 The Iron Road | Rail snow and washouts |
| 3 | 26 The Common Table | Season planning and storage |
| 4 | 27 The Thread | Wet clothing and cold exposure |
| 4 | 28 The Lesson | Sky classes and almanac teaching |
| 4 | 29 The Glass | Barometers, optics, skyglass |
| 4 | 30 The Press | Weather sheets and almanacs |
| 4 | 31 The Kiln | Storm bracing and brick footings |
| 5 | 32 The Wild | Weather and migration |
| 5 | 34 The Long Road | Route weather and shelters |
| 5 | 35 The Habit | Cold, pain, and dependency |
| 5 | 36 The Watch | Storm alerts on watch |

Each hook is additive. The Weather can ship alone, and every other expansion can
ship without it.

---

## 41. APPENDIX AB — ENDING PROSE SKETCHES

**The Read Sky.** Forecasts are trusted, warnings are acted on, and the shelter
spends its weather years repairing gates instead of walls.

**The Hardened Shelter.** Shutters, braces, hoods, and nets absorb the season,
and the storm becomes a noise the shelter sleeps through.

**The Endured Season.** The ash passes with wear and no deaths, and the log
records what it cost line by line.

**The Wrong Year.** Two misses and an unread warning, and the shelter loses a
wing it will rebuild in the spring and remember longer.

**The Quiet Almanac.** A complete, unremarkable record of ordinary weather,
worth more than any single storm because it predicts the next one.

**Fade.** The shelter keeps reacting to weather it never predicts, and the roof
holds for another year, and nobody asks why.

---

## 42. APPENDIX AC — COMMON FAILURE PATTERNS TO AVOID

| Pattern | Why it is bad | Correct approach |
|---|---|---|
| Prophecy forecasts | removes agency | confidence and misses |
| Weather as random damage | unfair | tracks and preparation |
| Storm wipes | punishing | damage and recovery |
| Warning spam | fatigue | authored levels and cost |
| Hidden confidence | distrust | visible arithmetic |
| Ash as one event | shallow | phased season |
| Black rain ignored | fantasy | closure and dose |
| Records without use | busywork | almanac improves models |
| Hardening as menu | trivial | labor and materials |
| Forecasts rerolling weather | authority break | read-only models |

The list exists because weather sits under everything else in the game and is
easy to turn into punishment. The live engine keeps it fair; the expansion's job
is to make it legible and survivable.

---

## 43. APPENDIX AD — CONTENT VOLUME SUMMARY

| Category | Rows | Prose estimate |
|---|---|---|
| Forecast models | 15 | 4,000 |
| Weather signs | 30 | 5,000 |
| Storm tracks | 12 | 3,500 |
| Warnings | 15 | 3,500 |
| Ash seasons | 6 | 3,000 |
| Hardening plans | 15 | 4,000 |
| Weather records | 40 | 6,000 |
| Readiness calendars | 4 | 2,500 |
| Quests | 53 | 15,000 |
| NPCs | 8 | 7,000 |
| Locations | 10 | 3,500 |
| Items | 16 | 2,500 |
| Endings | 6 | 3,000 |
| **Total** | | **~64,500** |

---

## 44. APPENDIX AE — SIDE QUEST STAGE DETAIL

| Quest | Stages | Objective kernel |
|---|---|---|
| `quest_weather_post_build` | 4 | Site, build, instrument, staff |
| `quest_weather_instruments` | 4 | Source, calibrate, repair |
| `quest_weather_watch_duty` | 3 | Read, record, report |
| `quest_weather_river_gauge` | 3 | Set, read, log |
| `quest_weather_night_sky` | 3 | Observe, note, compare |
| `quest_weather_signs_study` | 4 | Read, weigh, record |
| `quest_weather_model` | 4 | Gather, build, test |
| `quest_weather_confidence` | 3 | Review, adjust, display |
| `quest_weather_verify` | 3 | Predict, watch, score |
| `quest_weather_wrong_lesson` | 3 | Miss, analyze, fix |
| `quest_weather_storm_prep` | 4 | Stage, brace, close |
| `quest_weather_shutter_drill` | 3 | Time, fit, improve |
| `quest_weather_shelter_drill` | 3 | Alert, gather, account |
| `quest_weather_damage` | 4 | Assess, repair, record |
| `quest_weather_rescue` | 4 | Locate, shelter, return |
| `quest_weather_filter_care` | 4 | Inspect, clean, swap |
| `quest_weather_ash_clean` | 3 | Clear, dispose, verify |
| `quest_weather_ash_crops` | 4 | Cover, water, assess |
| `quest_weather_ash_lungs` | 4 | Screen, treat, policy |
| `quest_weather_ash_route` | 4 | Survey, clear, mark |
| `quest_weather_harden_roof` | 4 | Plan, brace, inspect |
| `quest_weather_harden_windows` | 3 | Fit, test, store |
| `quest_weather_harden_drains` | 3 | Clear, line, test |
| `quest_weather_harden_store` | 3 | List, gather, stock |
| `quest_weather_harden_travel` | 4 | Site, build, supply |
| `quest_weather_log` | 3 | Format, write, keep |
| `quest_weather_pattern` | 4 | Compare, hypothesize, verify |
| `quest_weather_almanac_write` | 4 | Compile, edit, print |
| `quest_weather_share_region` | 3 | Encode, send, confirm |
| `quest_weather_archive` | 3 | Bind, shelve, index |

---

## 45. APPENDIX AF — WARNING TRUST MODEL

| Event | Trust change | Notes |
|---|---|---|
| Accurate warning acted on | +5 | confirmed outcome |
| Accurate warning ignored | −5 | shelter choice |
| False alarm | −3 | logged |
| Missed event | −10 | no warning issued |
| Repeated low-value warnings | −1 each | fatigue |
| Emergency used once | +15 | proven value |
| Emergency misused | −20 | trust collapse |

Warning trust is the expansion's social currency. A shelter that cries wolf
teaches its people to stay in bed; a shelter that warns rarely and accurately
saves lives with a single siren.

---

## 46. APPENDIX AG — FILTER AND AIR MODEL

| Filter state | Load | Air purity | Action |
|---|---|---|---|
| Clean | 0–20% | full | none |
| Working | 20–50% | good | watch |
| Loaded | 50–75% | reduced | plan swap |
| Clogged | 75–90% | poor | swap now |
| Blocked | 90%+ | critical | close intake |
| Damaged | any | leaking | replace housing |

The filter model ties the sky to the clinic: ash seasons consume filters, air
purity drops, and lung cases rise. Every filter swap is a small piece of
preventive medicine.

---

## 47. APPENDIX AH — BLACK RAIN PROTOCOL TABLE

| Step | Action | Owner | Timing |
|---|---|---|---|
| 1 | Watch | forecaster | 12h before |
| 2 | Warning | warden | 12h before |
| 3 | Close intakes | engineer | 6h before |
| 4 | Cover stores | quartermaster | 6h before |
| 5 | Shelter people | warden | 2h before |
| 6 | Sample rain | observer | during |
| 7 | Survey dose | medic | after |
| 8 | Clear surfaces | works crew | after |
| 9 | Test water | water team | after |
| 10 | Record | forecaster | after |

Black rain is the expansion's most serious protocol and it is entirely
procedural: watch, warn, close, shelter, sample, survey, clear, test, record.
The horror is in the numbers, and the numbers are how the shelter survives.

---

## 48. APPENDIX AI — STORM DRILL TABLE

| Drill | Time target | Participants | Failure mode |
|---|---|---|---|
| Shutter fit | 20 min | works crew | loose pin |
| Intake close | 10 min | engineer | missed duct |
| Shelter call | 15 min | all | stragglers |
| Head count | 10 min | warden | unaccounted |
| Store cover | 30 min | quartermaster | open sacks |
| Route close | 20 min | warden | open gate |
| Rescue call | 30 min | rescue team | wrong bearing |
| All clear | 15 min | all | premature exit |

Drills are how the shelter knows its warning system works before the day it has
to. Times are real, failures are recorded, and the next drill fixes the failure.

---

## 49. APPENDIX AJ — SKY OBSERVATION POST TABLE

| Post | Position | Instruments | Coverage | Staff |
|---|---|---|---|---|
| Ridge | high | barometer, vane | 30 km | 1 |
| River | low | gauge, rain | flood valley | 1 |
| Ash Plain | exposed | depth stake | ash drift | 1 |
| Wind Gap | funnel | vane, pressure | onset | 1 |
| Snow Post | north | depth stakes | snow line | 1 |
| Signal Hill | mid | flags, siren | warning | 1 |
| Black Basin | valley | sampler | rain | 1 |
| Shelter Roof | shelter | thermometer | local | 0.5 |

Posts are where observation happens, and they need people, instruments, and
maintenance. The expansion's forecasting is built on shifts and instruments, not
on a menu button.

---

## 50. APPENDIX AK — REGIONAL WEATHER MAP

| Settlement | Exposure | Strength | Need | Trade |
|---|---|---|---|---|
| The shelter | mixed | forecast | instruments | sells warnings |
| Market Town | flood | stores | forecast | buys warnings |
| Fog Ridge Camp | wind | shelter | barometer | sells shelter |
| Spring Village | hail | netting | forecast | buys nets |
| Foundry Enclave | ash | filters | records | buys almanacs |
| Deep Bunker | black rain | depth | samples | shares data |
| River Flotilla | flood | mobility | gauge | moves warnings |
| Coal Stage | storm | fuel | route info | trades fuel |

Weather is the one resource every settlement shares, and the shelter that
forecasts becomes the region's early warning. That reputation is worth trading.

---

## 51. APPENDIX AL — LORE: THE SKY TRADITION

The fiction:

- **The Ridge Post** was a pre-war fire lookout; its tower bones are still the
  best observation point in the valley.
- **The River Gauge** was a municipal flood station; its markings are a century
  old and still accurate.
- **The Ash Plain** was farmland before the Exchange; now it is a source of
  drift, depth, and grim measurements.
- **The Black Basin** collects fallout-laced rain and is the shelter's most
  careful and least visited site.
- **The Lightning Tree** has been struck eleven times and is a teaching object
  for rods, grounding, and fire risk.

No real weather service, storm name, or agency is copied. The tradition is
generic and local.

---

## 52. APPENDIX AM — SENSITIVE TOPICS TABLE

| Topic | Risk | Handling |
|---|---|---|
| Storm deaths | Spectacle | Rare, authored, grieved |
| Ash lung | Medical horror | Clinical, preventive |
| Black rain | Radiation fear | Procedural, factual |
| Warning fatigue | Blame | Trust modeled, not mocked |
| Wrong forecasts | Shame | Logged, instructive |
| Exposure of elderly | Vulnerability | Care and adaptation |
| Children in storms | Fear | Protected, drilled, calm |
| Route closures | Isolation | Shelter and relief |
| Money and storms | Exploitation | Fair trade only |
| Storm chaser | Recklessness | Warned, never rewarded |

The sky is the one antagonist the shelter can never defeat, only prepare for,
and the expansion's contract is that weather is treated with respect and never
as entertainment.

---

## 53. APPENDIX AN — MEASUREMENT AND REVIEW CADENCE

| Gate | Question | Evidence |
|---|---|---|
| Premise | Do live owners still match? | file:line audit |
| Data | Are all rows valid and reachable? | integrity + scanner |
| Core | Are systems pure and deterministic? | unit tests |
| Persistence | Does legacy load neutral? | round-trip tests |
| Host | Is every command real? | selftest + journey |
| UI | Is confidence honest? | lifecycle + a11y tests |
| Tone | Is weather respected? | content review |
| Balance | Is preparation rewarded? | 720-day soak |

---

## 54. APPENDIX AO — FIRST YEAR OF THE SKYWATCH

| Month | Focus | Milestone |
|---|---|---|
| 1 | Storm and repair | warning chain agreed |
| 2 | Ridge post | observation begins |
| 3 | River gauge | flood pattern |
| 4 | First forecast | verified call |
| 5 | Almanac start | daily record |
| 6 | Ash season | filter plan |
| 7 | Clinic policy | masks and closure |
| 8 | Shutters | hardening begins |
| 9 | Storm weathers | light damage |
| 10 | Black rain | protocol executed |
| 11 | Readiness | shared calendar |
| 12 | Almanac year | printed and shelved |

A year of skywatch is a year of attention, and by the end the shelter owns a
record that will outlive the weather that made it.

---

## 55. APPENDIX AP — OPEN IMPLEMENTATION NOTES

- Forecast models must read live `WorldWeatherState` and `SeasonWindowDef`
  weights; never duplicate the generation logic.
- Confidence should be computed and displayed as integer permille.
- Warnings should write to existing notice and radio surfaces rather than a new
  channel.
- Storm damage should be computed from exposure, hardening, and authored risk
  with deterministic output.
- Ash operations should register filter load against the live filter system and
  never write air purity directly.
- Black-rain dose must call the live radiation pipeline; sampling items should
  be real inventory.
- Weather records should be readable content and printable through the press.
- Readiness tasks should register with the duty roster so labor is real.

---

## 56. APPENDIX AQ — STORM SHELTER STOCK LIST

| Stock | Quantity | Use | Restock |
|---|---|---|---|
| Water | 4 days | drinking | weekly |
| Rations | 4 days | food | weekly |
| Lamps | 4 | light | check |
| Batteries | 2 sets | radio | monthly |
| Blankets | 6 | warmth | check |
| First aid | 2 kits | injury | monthly |
| Rope | 30 m | rescue | check |
| Tools | 2 sets | clearing | check |
| Buckets | 4 | leaks | check |
| Gaskets | 6 | seals | stock |
| Sandbags | 20 | water | season |
| Radios | 2 | contact | charge |

Storm shelters are only as good as what is in them. The list is boring on
purpose: a shelter that stocked rope and water before the storm does not need a
hero after it.

---

## 57. APPENDIX AR — FORECAST ACCURACY TABLE

| Skill level | Base confidence | Miss rate | Lead bonus |
|---|---|---|---|
| Novice | 35% | high | 0h |
| Apprentice | 45% | medium | 3h |
| Watcher | 55% | medium | 6h |
| Forecaster | 65% | low | 12h |
| Senior | 75% | low | 18h |
| Archivist | 80% | very low | 24h |

Accuracy is earned by practice, records, and instruments. The archivist's value
is not genius but accumulated pattern, which is exactly what the expansion
argues about knowledge.

---

## 58. APPENDIX AS — ASH SEASON PHASE TABLE

| Phase | Ash depth | Filter load | Clinic | Work |
|---|---|---|---|---|
| Settle | light | low | routine | normal |
| Drift | rising | med | masks | outdoor reduced |
| Deep | heavy | high | screening | indoor priority |
| Plume | extreme | critical | full | closure |
| Clear | falling | med | follow-up | clearing |
| Frozen | capped | low | frost care | low |

Phases give the ash season a shape and a set of decisions. The shelter is never
surprised by the middle of a season it has already survived once.

---

## 59. APPENDIX AT — WEATHER EQUIPMENT UPKEEP TABLE

| Instrument | Check | Clean | Calibrate | Replace |
|---|---|---|---|---|
| Barometer | daily | weekly | monthly | yearly |
| Thermometer | daily | weekly | yearly | broken |
| Rain gauge | daily | weekly | season | cracked |
| Wind vane | daily | monthly | season | bent |
| Hygrometer | daily | weekly | season | worn |
| Weather log | daily | n/a | n/a | filled |
| Siren | weekly | season | yearly | broken |
| Signal flags | weekly | monthly | n/a | torn |

Instruments need care like everything else in the shelter. A drifted barometer
is a wrong forecast, which is why Piet cleans his twice a week and writes it in
the log.

---

## 60. APPENDIX AU — SEASONAL HAZARD SUMMARY TABLE

| Season | Primary hazard | Secondary | Shelter focus |
|---|---|---|---|
| Spring | flood | late frost | drainage, planting |
| Summer | storm | hail, heat | shutters, nets |
| Autumn | wind | early snow | hardening, harvest |
| Winter | cold | blizzard, ice | warmth, closures |
| Ash | ashfall | filters | air, crops |
| Black | fallout rain | dose | closure, survey |

One season, one primary hazard, one shelter focus. The table is how the
skywatch teaches the shelter to stop preparing for everything and start
preparing for the right thing.

---

## 61. CLOSING STATEMENT

ASHFALL already generates seasons, weighted weather kinds, ashfall, fallout
storms, blizzards, black rain, route gates, spatial weather, named storms, and
economy shocks. What it lacks is the watching: posts, instruments, signs,
forecasts, warnings, preparation, ash operations, records, and an almanac. The
Weather adds that world without adding a second weather engine or a second
hazard authority. It adds a forecast that is honest about its confidence, a
warning that closes a site before the roof goes, a filter changed before the
lung, and a written year of sky that the shelter will read next spring.

> Wave 5 note: this plan is one of five Wave 5 expansion bibles (32–36). Each is
> self-contained; none requires another to ship. The shared Wave 5 index lives at
> `docs/expansions/wave5/WAVE5_INDEX.md`. The safe pre-signature step is Phase 1
> (data schemas and validators), which is additive and reversible. Evidence
> anchors: `WeatherSystem` (`SeasonWindowDef` with clear/rain/overcast/ashfall/
> fallout storm/blizzard/black rain, six-hour check interval, `WorldWeatherState`),
> `WeatherEffectsCatalog`, `WeatherGate`/`WeatherGateCatalog`,
> `WeatherAtmosphereMap`, `YearOfAshStormCatalog`, `EconomyWeatherShockRules`,
> `weather_effects.json` (7.6 KB), `weather_route_gates.json` (11 KB),
> `seasonal_events.json` (12.3 KB), `weather_seasons.json` (3.2 KB), and
> `weather_hardening_upgrades.json` (3.7 KB).