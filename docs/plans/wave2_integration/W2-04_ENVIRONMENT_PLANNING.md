# ASHFALL — WAVE 2 INTEGRATION PROGRAM · PLAN 4 OF 6

# ENVIRONMENT PLANNING & HAZARD INTEGRATION PLAN

**Status:** PROPOSAL — planning-only · no production path claimed
**Wave:** W2 (six-plan integration wave)
**Document:** W2-04 · part A of C
**Target size:** ~150,000 characters (this plan)
**Date:** 2026-09-21
**Repo:** `Atomic War` @ `Zcode_Branch`, HEAD `5be1a30a`
**Companion plans:** W2-01 (maintenance), W2-02 (bugs), W2-03 (gameplay), W2-05 (locations), W2-06 (enrichment)
**Plan-unblocking annex:** Annex U at the end — deliberately separated per the Wave 2 rule.

---

## 0. How to read this plan

This plan designs and integrates the **environment**: weather, storms, air,
radiation fields, water under the ground, the underground, fire, flood, sky,
seasons, and the gates/hardening that let the player respond. It extends
existing owners; it does not add a second climate, a second radiation model, or
a second hazard engine.

### 0.1 Two selection levels

| Plan Path | Name | Meaning |
|---|---|---|
| **A** | Data & Truth | fix/author catalog truth, consumption, and clarity; no new mechanics |
| **B** | Deepen the Environment | new read models/consumers on existing owners; hazard interaction; response depth |
| **C** | Living Environment | dynamic coupling (weather↔groundwater↔fire↔sky), long-arc environmental change |

**Level 2:** ten points, each A/B/C (Appendix A).

### 0.2 Default mapping

| Plan Path | A points | B points | C points |
|---|---|---|---|
| A Data & Truth | 1–10 | — | — |
| B Deepen | 1,9 | 2,3,4,5,6,7,8 | 10 |
| C Living | — | 2,7 | 1,3,4,5,6,8,9,10 |

### 0.3 The Wave 2 rule for this plan

> **One environment authority per concern.** Weather owns kinds and windows;
> radiation owns dose fields; groundwater owns head/contamination; subterranean
> owns zones; fire/flood own their state; sky owns ash layer. This plan never
> creates a parallel one, and every new effect must be a read/consumer or an
> additive field on the existing owner.

### 0.4 Design vocabulary

| Term | Meaning |
|---|---|
| window | authored season/weather weighting period |
| field | a spatial/global environmental value (rads, ash, head) |
| gate | authored condition blocking a route/action |
| hardening | authored upgrade reducing a hazard effect |
| exposure | dose/effect accumulated by a survivor |
| coupling | one environment owner's output feeding another's input |

---

## 1. Executive summary

ASHFALL already has a deep environment stack:

- **Weather:** `WeatherSystem` with `SeasonProfileDef`/`SeasonWindowDef`
  weights (Clear/Rain/Overcast/Ashfall/FalloutStorm/Blizzard/BlackRain),
  `WorldWeatherState` (kind, elapsed hours, wind direction/speed, roll count),
  and hard modifiers: `FalloutStormOutdoorRadModifier = 150f`,
  `BlackRainOutdoorRadModifier = 250f`, `BlackRainHazmatMeltMultiplier = 5f`.
- **Weather support systems:** `WeatherStationSystem`, `WeatherSondeSystem`,
  `WeatherIntelligenceCoordinator`, `IWeatherSeverityProvider`,
  `WeatherAtmosphereMap`, plus catalogs `weather_effects.json`,
  `weather_seasons.json`, `weather_route_gates.json`,
  `weather_hardening_upgrades.json`.
- **Radiation:** `RadiationSystem`, `Dosimeter`, `ExposureBreakdown`,
  `ExposureEnvironment` (reads location `baseRadsPerHour` from
  `locations.json` and regional catalogs), `FalloutSystem`, `fallout_patterns.json`,
  `DoseLedger` (`SetupDoseLedger`), `LowBackgroundLeadEngine`,
  `RadiationPhaseProgression`.
- **Groundwater:** `AquiferPiezometerEngine`, `GeothermalAquiferSystem`,
  `GeothermalAquiferState`, `HydroGeologyCatalog`, `HydroGeologyDiscoverySystem`,
  `AquiferPiezometerEngine`, plus the sealed Plan 189 advisory bridge to water
  treatment.
- **Underground:** `SubterraneanSystem`, `SubterraneanZoneCatalog`,
  `SubterraneanSave`, `ExcavationSystem`, `ExcavationHazardSystem`,
  `SeismicDynamicsSystem`, `GeologicalStrataCatalog`.
- **Fire/flood:** `ShelterFireHazardSystem`, `SumpFloodingSystem`,
  `CascadeCoordinator`/`CascadeRuleCatalog`, `ShelterAtmosphereSystem.ThermalComfort`.
- **Sky/ash:** `SkyLayerArmorSystem`/`Catalog`, `SkyDefenseBatterySystem`,
  `YearOfAshStormCatalog`, ash-related atmosphere texts.
- **Data:** `environmental_atmosphere_expansion.json`,
  `environmental_texts_expansion_05.json`, `seasonal_events.json`,
  `WildlifeSeasonalCalendar.cs`, `damaged_map_zones.json`.

The gaps this plan addresses are therefore **coupling, consequence truth, and
player response** — not missing subsystems:

1. Weather weights are authored per window, but their **downstream effects** on
   needs/water/visibility/power are partially implicit (Point 1/2).
2. Air quality/fallout is present as rad modifiers and patterns, but there is no
   single read model for "what is the air doing now and where is the plume"
   (Point 3).
3. Radiation fields come from locations and storms, but the **field's spatial
   gradient and decay** are not surfaced for decisions (Point 4).
4. Groundwater exists with a real advisory bridge, but player-visible
   **head/contamination doctrine** is thin (Point 5).
5. Subterranean zones exist; gas/collapse/ecology consequences need honoring
   (Point 6).
6. Fire/flood coupling exists via the cascade coordinator; the environment's
   part (dry weather → fire risk; rain → sump relief) is the gap (Point 7).
7. Sky/ash layer armor exists; ash accumulation's **environmental meaning**
   (visibility, roof load, filtration) is the gap (Point 8).
8. Seasons drive weather weights and wildlife calendars separately; their
   coupling is authored in two places (Point 9).
9. Weather gates/hardening exist; the **route-planning truth** (does the gate
   reflect current recovery state?) is the gap (Point 10).

---

## 2. Verified current state (environment evidence)

### 2.1 Weather and seasons

```json
// weather_seasons.json (verified excerpt)
{ "id": "default_winter", "displayName": "The Year of Ash and Ice",
  "weatherCheckIntervalHours": 6.0,
  "seasons": [
    { "id": "window_first_thaw", "startDay": 0,
      "clearWeight": 2.6, "rainWeight": 3.0, "overcastWeight": 2.4,
      "ashfallWeight": 1.4, "falloutStormWeight": 0.6,
      "blizzardWeight": 0.7, "blackRainWeight": 0.3 },
    { "id": "window_ash_settling", "startDay": 30,
      "clearWeight": 0.8, "rainWeight": 0.7, "overcastWeight": 1.6,
      "ashfallWeight": 2.0, "falloutStormWeight": 0.3,
      "blizzardWeight": 0.2, "blackRainWeight": 0.1 }
  ] }
```

`WeatherSystem` constants (verified):

| Constant | Value | Meaning |
|---|---|---|
| `FalloutStormOutdoorRadModifier` | 150f | outdoor rad multiplier during fallout storm |
| `BlackRainOutdoorRadModifier` | 250f | outdoor rad multiplier during black rain |
| `BlackRainHazmatMeltMultiplier` | 5f | hazmat degradation multiplier |

State exposed: `currentKind`, `totalElapsedHours`, `hoursUntilNextCheck`,
`rollCount`, `restrictToNonHazardWeather`, `wind_direction_deg`,
`wind_speed_kph`.

### 2.2 Weather-adjacent catalogs

| File | Role |
|---|---|
| `weather_effects.json` | per-kind effect table (verify consumers at P0) |
| `weather_hardening_upgrades.json` | authored upgrades reducing weather effects |
| `weather_route_gates.json` | routes blocked/penalized by weather |
| `seasonal_events.json` | authored season-bound events |
| `duty_roster_seasons.json` | season-aware duty rosters |
| `year_of_ash_locations.json` | 66 storm-era locations |

### 2.3 Radiation and fallout

| Component | Evidence |
|---|---|
| `RadiationSystem` | registered survivors, acute sickness, phases |
| `ExposureEnvironment` | optional location base-rads provider; reads `locations.json`, regional catalogs (`:256–274`) |
| `locations.json` | `baseRadsPerHour` present on 178/179 rows |
| `FalloutSystem` | `SetupFallout`; patterns in `fallout_patterns.json` |
| dose ledger | `SetupDoseLedger`; dose ledger is radiation-only per debt note |
| `damaged_map_zones.json` | zone damage/danger data |

### 2.4 Groundwater

| Component | Evidence |
|---|---|
| `AquiferPiezometerEngine` | head/advisory engine (deterministic; no `System.Random`) |
| `GeothermalAquiferSystem` | geothermal aquifer state/system |
| `HydroGeologyCatalog` / `DiscoverySystem` / `Projection` | discovery + projection surfaces |
| sealed advisory bridge | Plan 189: piezometer `BuildAdvisory` → `RegisterContaminationAdvisory` before the water tick (debt row RETIRED) |
| `sealed piezometer_network` save section | exists (Plan 189 evidence) |

### 2.5 Underground

| Component | Evidence |
|---|---|
| `SubterraneanSystem` + zone catalog + save | node/zone ownership |
| `ExcavationSystem` + hazard system | digging and careful digging |
| `SeismicDynamicsSystem` (+ Monitoring) | seismic events/monitoring |
| `GeologicalStrataCatalog` | strata data |

### 2.6 Fire and flood

| Component | Evidence |
|---|---|
| `ShelterFireHazardSystem.Incidents` | unresolved/unsuppressed incidents (Plan 194 consumer) |
| `SumpFloodingSystem.State.nodes[].isFlooded` | flood state (Plan 194) |
| `CascadeCoordinator` + `CascadeRuleCatalog` | cascading hazard rules |
| ventilation authority | `_silentFoundry.Engine.BindVentilation(_ventilation)` (smoke/CO routing) |

### 2.7 Sky and ash

| Component | Evidence |
|---|---|
| `SkyLayerArmorSystem`/`Catalog` | sky layer armor (weather hardening at sky level) |
| `SkyDefenseBatterySystem`/`OrdnanceCatalog` | defense battery (ordnance read-only catalog) |
| `YearOfAshStormCatalog` | storm catalog for the campaign era |
| ash atmosphere texts | `environmental_atmosphere_expansion.json` types include `ash`-related tags |

---

## 3. Scope, non-goals, rules

### 3.1 In scope

- Weather/season weighting and effect truth (data + consumption).
- Air quality/fallout read model and plume/visibility consequences.
- Radiation-field explanation, gradient/decay surfaces, dosimetry clarity.
- Groundwater doctrine surfaces (head, contamination, advisory) and couplings.
- Subterranean hazard consequences (gas, collapse, water) via existing owners.
- Fire/flood environmental coupling (dryness, rain relief, cascade honesty).
- Sky/ash accumulation meaning (visibility, roof load, filtration) as read
  models and bounded consumers.
- Seasonal coupling of weather and wildlife calendars.
- Weather gates/hardening truth for routes and shelter.
- Environment content **surfaces** (not prose) needed to make mechanics
  legible; prose routes to W2-06.

### 3.2 Non-goals

- New climate/radiation/hazard systems (Rule 5).
- Gameplay tuning of needs/economy (W2-03) — this plan supplies environment
  inputs; W2-03 tunes responses.
- Location tiers/map graph (W2-05) — this plan supplies environment fields that
  W2-05 may read.
- Prose/briefings text (W2-06) — this plan names the surfaces and values.
- Bug repairs (W2-02).
- Save schema (UNBLOCK-01/02).

### 3.3 Rules

1. **Owner discipline.** Weather stays `WeatherSystem`; rads stay
   `RadiationSystem`/`ExposureEnvironment`; head stays piezometer; zones stay
   subterranean; fire/flood stay their systems.
2. **Data-authored thresholds.** New effects go to catalogs, not constants.
3. **Determinism.** All environmental rolls already use seeded RNG; new
   couplings must too (no wall-clock; no `System.Random`).
4. **Truthful surfaces.** UI shows owner values; no fabricated weather/forecast.
5. **Additive persistence only.** If a coupling needs memory, it rides the
   owning section as a nullable field (W-3 discipline).
6. **Reversibility.** One coupling per tranche.

---

## 4. Plan Path and decision index

### 4.1 The ten points

| # | Point | Default |
|---|---|---|
| 1 | Weather-kind effect truth and consumption | B |
| 2 | Severe weather escalation and shelter response | B |
| 3 | Air quality, dust, and fallout plume model (read) | B |
| 4 | Radiation field truth, gradient, and decay | B |
| 5 | Groundwater doctrine and coupling | B |
| 6 | Subterranean hazards and consequences | B |
| 7 | Fire/flood environmental coupling | B |
| 8 | Sky/ash accumulation meaning | B |
| 9 | Seasonal coupling (weather ↔ wildlife ↔ events) | A |
| 10 | Weather gates and hardening truth | B |

### 4.2 Selection sheet

```text
PLAN 4 — ENVIRONMENT
Plan Path: [ ] A Data & Truth  [ ] B Deepen (default)  [ ] C Living Environment

01 weather effects .... [A] [B] [C]   default B
02 severe escalation .. [A] [B] [C]   default B
03 air/plume .......... [A] [B] [C]   default B
04 radiation field .... [A] [B] [C]   default B
05 groundwater ........ [A] [B] [C]   default B
06 subterranean ....... [A] [B] [C]   default B
07 fire/flood ......... [A] [B] [C]   default B
08 sky/ash ............ [A] [B] [C]   default B
09 seasons coupling ... [A] [B] [C]   default A
10 gates/hardening .... [A] [B] [C]   default B
```

---

## 5. Decision Point 1 — Weather-kind effect truth (default B)

### 5.1 The design question

Each weather kind should have a **declared effect set** (rad modifier, wind,
visibility, water gain/loss, power demand, fire risk, mood) that is authored in
one place and consumed by the owners. Today the modifiers exist in code
(constants) and catalogs (`weather_effects.json`), and the coupling to needs,
water, power, and fire is distributed.

### 5.2 Path A — Catalog truth audit

- Compare `weather_effects.json` against the code constants and consumers.
- List kinds with no consumers ("dead weather") and consumers with no authored
  row.
- Fill only authoring gaps; no new consumers.

### 5.3 Path B — One effect table consumed by declared owners

- Make the effect table authoritative for: outdoor rad multiplier, wind
  (direction/speed), visibility band, water collection modifier, power demand
  modifier, fire-risk modifier, morale modifier.
- Each consumer is an existing owner (exposure environment, water collection,
  power grid demand, fire hazard, needs morale).
- The mapping is declared in the catalog; a consumer reads only its row (no
  hardcoded kind checks).
- Tests: per-kind effect assertions (storm multiplier reaches exposed outdoor
  entities; rain raises collection; blizzard raises power demand).

### 5.4 Path C — Weather as a composable field

Path B, plus additive/composable weather states (e.g., ashfall + wind = dust
event) through a small weighted composition derived from the table, still
rolled by the seeded weather system. This adds depth but also complexity; it is
the natural C.

### 5.5 Acceptance

- No kind without a declared effect set.
- No consumer reading kind by hardcoded string beyond the table.
- Storm multipliers verified to reach exposure tests.
- Determinism preserved (roll order unchanged).

---

## 6. Decision Point 2 — Severe weather escalation and shelter response

### 6.1 The design question

Severe weather should escalate in readable stages, threaten specific things,
and give the shelter real responses (seal, heat, filter, ration power). The
`CascadeCoordinator` and shelter systems exist; the gap is **staged
escalation + response legibility**.

### 6.2 Path A — Author escalation windows

- Per severe kind (fallout storm, black rain, blizzard, ashfall), author
  escalation thresholds (duration, intensity steps) in the effect catalog.
- No new response mechanics.

### 6.3 Path B — Response consumption

- Bind shelter responses to storm stages through existing owners: ventilation
  filter load, heating demand, power priority, door/seal state, water
  collection pause.
- Add a **storm response read model** (current stage, next stage, what is being
  consumed, time to recovery) consumed by the briefing/panel.
- Tests: stage transition at authored thresholds; each response reduces a
  measured effect under the storm soak.

### 6.4 Path C — Multi-day storm arcs

Path B, plus authored multi-day storm sequences (approaching → arrival →
sustained → clearing) with pre-warning through the weather intelligence system,
and post-storm aftermath (debris, contamination, repair needs) via existing
owners. This needs W2-06 prose for the warning surfaces.

### 6.5 Acceptance

- Every severe kind has staged thresholds.
- Responses measurably reduce effects.
- Warning precedes the severe stage (with the weather intelligence owner).
- No unbounded storm damage; every stage has an end condition.

---

## 7. Decision Point 3 — Air quality, dust, and fallout plume (read model)

### 7.1 The design question

Players need to answer: is the air dangerous right now, where is it coming
from, and where will it be? The game has fallout patterns, ash, and winds but
no single air read model.

### 7.2 Path A — Truth table

- List every air-relevant value (rad modifier, ash layer, dust, smoke,
  ventilation state) and its owner.
- Identify what the player can see today and the gaps.

### 7.3 Path B — Air read model

- A read-only `AirQualityModel` over existing owners: particulates (ash/dust),
  rad concentration outdoors vs. indoors (exposure environment), wind
  (direction/speed from weather state), ventilation/filtration state, and a
  bounded short-horizon drift estimate using wind.
- Consumed by: briefing warning, shelter panel, travel gate checks.
- No new simulation: the model reads; only weather rolls and existing systems
  mutate.

### 7.4 Path C — Plume propagation

Path B, plus a bounded plume propagation read (a small grid or a distance-band
projection from `fallout_patterns.json` + wind) that estimates arrival windows
for locations. It is still a **read**: no location state is mutated; the model
answers "when will this reach X" for planning. W2-05 may consume it for
location danger.

### 7.5 Acceptance

- Every displayed air value names an owner.
- Drift estimate states its basis (wind + pattern) and horizon.
- No new persistent state without a signed section (likely none needed).

---

## 8. Decision Point 4 — Radiation field truth, gradient, and decay

### 8.1 The design question

`baseRadsPerHour` exists per location; storms multiply outdoor exposure. What
is missing is a **legible field**: how rads change with distance/time, what
decay means, and how dosimeters read it. The dose ledger is radiation-only and
must stay that way.

### 8.2 Path A — Field audit

- Verify every rad source's consumer path: location base rads
  (`ExposureEnvironment`), storm modifiers, artifact/zone rads, and the
  dose-ledger recording.
- Report gradients and decay documented vs. actual.

### 8.3 Path B — Field read model

- A read-only `RadiationFieldModel`: current outdoor rads at the current
  location/zone, dominant source (location base, storm, artifact), decay
  projection at authored half-life (from existing data), and a comparison to
  the dosimeter's reading basis.
- Consumed by the dosimeter panel and briefing; **no new ledger** (the dose
  ledger records exposure; this model explains it).
- Tests: field model vs. source values; decay projection against authored
  half-life; storm multiplier reflected.

### 8.4 Path C — Dynamic decay and contamination memory

Path B, plus authored contamination memory for specific places (a zone's rads
decay over campaign days; recontamination possible via storms) as an **additive
field on the location state owner** — only if a signed persistence line exists;
otherwise the model remains a projection from static data + day.

### 8.5 Acceptance

- Every rad reading has an explained source.
- Decay projection uses authored half-life, not a UI guess.
- Dose ledger untouched (no second ledger).
- No schema change unless signed (C).

---

## 9. Decision Point 5 — Groundwater doctrine and coupling

### 9.1 The design question

The piezometer advisory bridge is sealed; what remains is the **player-facing
doctrine**: what the head means, when to pump, how contamination arrives, and
how aquifer state couples to wells/water treatment/sump.

### 9.2 Path A — Surface truth

- Verify the advisory's consumption (WT `RegisterContaminationAdvisory`,
  sump greywater source, piezometer save section).
- Ensure the panel shows head/trend/advisory from the owner.

### 9.3 Path B — Coupling read model and decisions

- A read-only `GroundwaterModel`: head, trend, contamination advisory, draw
  rate, and a bounded projection of days until a threshold.
- Couplings are consumer binds: well yield reads head; treatment reads
  advisory; sump infiltration reads flood state; excavation reads vicinity.
- Tests: head threshold changes yield; advisory changes treatment behaviour;
  projection matches owner trend.

### 9.4 Path C — Seasonal recharge and extraction doctrine

Path B, plus seasonal recharge (rain/snowmelt windows feeding head via an
additive field on the existing piezometer state) and over-extraction
consequences (a soft floor, recovery time) — authored, deterministic, and
observable. This is a real coupling but bounded and revertible.

### 9.5 Acceptance

- The player can explain why head changed (recharge/draw).
- Advisory reaches treatment (already sealed; verified).
- No parallel water-source system.

---

## 10. Decision Point 6 — Subterranean hazards and consequences

### 10.1 The design question

The underground has zones, excavation, seismic events, and hazard systems. The
improvement is **consequence honesty**: gas pockets, collapse risk, water
intrusion, and seismic aftershocks should be foreseeable and respond to
preparation.

### 10.2 Path A — Hazard audit

- List subterranean hazards and their existing signals/prevention (ExcavationHazardSystem).
- Find hazards without warning or without a countermeasure.

### 10.3 Path B — Preparedness consumption

- Bind existing preparations (supports, sensors, ventilation, sealed lighting)
  to hazard thresholds through their owners.
- Add a **site read model** (risk factors, active hazards, prevention status)
  consumed by the excavation panel.
- Tests: prepared site reduces event probability; unprepared site warns before
  a severe event.

### 10.4 Path C — Deep-zone ecology and long consequences

Path B, plus authored deep-zone ecology (fungal/animal presence via the fungi
and wildlife owners) and long-term effects (ground stability, water table)
through the existing strata/seismic owners. Content routes to W2-06.

### 10.5 Acceptance

- Every severe hazard has a warning and at least one countermeasure.
- Read model values all owner-sourced.
- No new subterranean system.

---

## 11. Decision Point 7 — Fire/flood environmental coupling

### 11.1 The design question

Dry and hot weather should raise fire risk; rain/snow should relieve sumps;
fire should interact with ventilation and water. The cascade coordinator already
chains hazards; the environment's input is the gap.

### 11.2 Path A — Coupling audit

- Verify current weather inputs to `ShelterFireHazardSystem` and
  `SumpFloodingSystem` (do they read weather/season at all?).
- List couplings that exist vs. desired.

### 11.3 Path B — Weather → fire/flood binds

- Fire risk reads weather effects (dry/wind/hot) from the Point 1 table.
- Sump level reads rain/snow windows; outdoor water collection pauses in
  freezing conditions.
- Cascade rules gain environment conditions where authored (dry spell before a
  fire event) without new rules bypassing the coordinator.
- Tests: dry window raises fire probability; rain lowers sump; cascade rule
  fires under its authored condition only.

### 11.4 Path C — Environment-driven disaster arcs

Path B, plus rare authored disaster arcs (a drought month, a monsoon week)
that escalate fire/flood/dust through existing systems and are telegraphed by
weather intelligence. Content via W2-06.

### 11.5 Acceptance

- Fire risk visibly responds to weather.
- Flood responds to precipitation/freeze.
- Cascades stay coordinator-owned.

---

## 12. Decision Point 8 — Sky/ash accumulation meaning

### 12.1 The design question

Ash layer/sky armor exists; what does ash **mean** for the shelter? Visibility,
roof load, filtration, water quality, and defense should read the sky state.

### 12.2 Path A — Truth audit

- Enumerate sky values (ash layer, armor) and their consumers.
- Find claimed effects without consumers.

### 12.3 Path B — Ash consumers

- Visibility reads ash for travel gates (with W2-05 route meaning).
- Roof load reads accumulation; the shelter upgrade path can reinforce.
- Filtration load reads air particulates (Point 3).
- Water collection quality reads ash deposition.
- Tests: accumulation raises filtration load; reinforcement reduces a
  measured failure risk.

### 12.4 Path C — Ash seasonality and sky defense coupling

Path B, plus authored ash seasonality (Year of Ash windows) and coupling to sky
defense operations (battery visibility/ordnance effectiveness under ash) via
existing owners. This connects two existing subsystems without adding a third.

### 12.5 Acceptance

- Every sky/ash value has a consumer or is documented as decorative.
- No fabricated defense numbers.

---

## 13. Decision Point 9 — Seasonal coupling (default A)

### 13.1 The design question

Weather seasons (`weather_seasons.json`), wildlife seasons
(`WildlifeSeasonalCalendar`), and seasonal events (`seasonal_events.json`) each
encode season independently. Divergence would be a truth bug.

### 13.2 Path A — Cross-check and align

- Produce the three calendars side by side.
- Flag windows where, e.g., weather says First Thaw but the wildlife calendar
  says winter.
- Align labels/dates in data only.

### 13.3 Path B — Single season axis

- A shared season id/day mapping consumed by all three systems (read-only
  alignment), with each retaining its own weights.
- Test: for every campaign day, all three systems report the same season id.

### 13.4 Path C — Season-driven event orchestration

Path B, plus event scheduling that reads the shared axis for authored seasonal
events. Content via W2-06. (Likely unnecessary; recommended only if the audit
finds real divergence.)

### 13.5 Acceptance

- One season identity per day across systems.
- No duplicated season tables beyond the weights each owner legitimately needs.

---

## 14. Decision Point 10 — Weather gates and hardening truth

### 14.1 The design question

`weather_route_gates.json` and `weather_hardening_upgrades.json` exist. Do the
gates reflect **current** conditions (weather now, road state) and do the
hardening upgrades actually reduce the gated effect?

### 14.2 Path A — Gate truth audit

- For each gate: condition, current evaluation path, and whether the player can
  see why a route is blocked.
- For each hardening: claimed effect and its consumer.

### 14.3 Path B — Gate/hardening consumption

- Gates evaluate against current weather state and route condition via the
  existing evaluator (`WeatherGateEvaluator` family), with a read surface for
  "why blocked".
- Hardening upgrades bind to their effects (armor, filtration, insulation)
  through existing owners; each upgrade has a measurable reduction test.
- Tests: gate opens/closes at authored thresholds; hardening reduces the
  measured exposure/demand.

### 14.4 Path C — Route-condition memory

Path B, plus route condition as an additive state (a damaged route stays damaged
for N days) on the route owner — only with a signed persistence line. W2-05
owns route meaning; this plan supplies the environment input.

### 14.5 Acceptance

- No gate without a readable reason.
- No hardening without a measured effect.
- Route state ownership coordinated with W2-05.

---

## 15. Execution phases

### E0 — Environment premise freeze (1 day, all paths)

- Verify all owners and catalogs from §2 at HEAD.
- Run the data-integrity and content-utilization selftests against environment
  catalogs; list unconsumed rows.
- Produce `P0_ENVIRONMENT_PREMISE.md`.

### E1 — Weather truth and effects (Point 1; any path)

- Path A audit; Path B effect table + consumers; tests.

### E2 — Severe escalation and response (Point 2)

- Staged thresholds + response binds + storm soak.

### E3 — Air and radiation fields (Points 3 + 4)

- Read models + surfaces; decay/gradient truth; no new ledger.

### E4 — Water and underground (Points 5 + 6)

- Groundwater couplings; subterranean preparedness; tests.

### E5 — Fire/flood and sky/ash (Points 7 + 8)

- Weather→fire/flood binds; ash consumers; cascade rules stay coordinator-owned.

### E6 — Seasons and gates (Points 9 + 10)

- Season axis alignment; gate/hardening consumption.

### E7 — Closeout

- Evidence pack; environment soak results; ledger proposals; Annex U.

### 15.1 Ordering constraints

- E1 before E5 (fire risk reads the effect table).
- E3 before E5/E8-adjacent (air particulates feed filtration).
- E6 may run anytime.
- W2-05 coordination mandatory before route-condition memory (Point 10 C).

---

## 16. Verification plan

### 16.1 Per-point evidence

| Point | Evidence |
|---|---|
| 1 | effect table + per-kind tests + storm multipliers reach exposure |
| 2 | stage transitions + response reduction + warning lead time |
| 3 | air read model vs. owners; drift basis stated |
| 4 | field vs. sources; decay vs. half-life; dose ledger untouched |
| 5 | head threshold → yield; advisory → treatment; projection vs. trend |
| 6 | prepared vs. unprepared hazard probabilities; warnings |
| 7 | dry→fire, rain→sump, cascade conditions |
| 8 | ash→filtration/visibility/load tests |
| 9 | season id agreement across systems |
| 10 | gate open/close; hardening reduction tests |

### 16.2 Commands

```bash
bash scripts/run_test.sh Ashfall.Core.Tests/World
bash scripts/run_test.sh Ashfall.Core.Tests/Radiation
bash scripts/run_test.sh Ashfall.Core.Tests/Shelter
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --content-utilization-selftest
godot --headless --path . -- --7day-smoke-selftest   # when couplings land
```

### 16.3 Environment soak

An environment soak (seeded 20×60 like W2-03's) reporting per-day weather kind,
storm days, fire incidents, sump days, radiation warnings, and gate closures —
the pattern base for "is the environment alive but not punishing".

---

## 17. Risks

| # | Risk | L | I | Mitigation |
|---|---|---|---|---|
| 1 | coupling creates double-counted modifiers | M | H | one owner per modifier; a matrix check |
| 2 | read model drifts from simulation truth | M | H | model reads owners only; tests compare |
| 3 | new persistence sneaks in | M | H | additive-only rule; signed line for any field |
| 4 | route meaning collides with W2-05 | M | M | coordinate claims; environment input only |
| 5 | severe storms become unfair | M | M | staged thresholds + response binds + soak |
| 6 | content surfaces overreach into prose | M | M | prose routes to W2-06 |
| 7 | season alignment churns three catalogs | L | M | data-only alignment under Path A |
| 8 | determinism creep in couplings | L | H | seeded rolls only; grep + parity tests |
| 9 | soak runtime | M | L | bounded seeds; scheduled |
| 10 | deep couplings (C) exceed one package | M | M | C items are separate signed packages |

---

## 18. Ownership and claims

| Phase | Claim | Paths |
|---|---|---|
| E0 | `W2-04-E0-PREMISE` | premise doc + touch list |
| E1 | `W2-04-E1-WEATHER-EFFECTS` | effect catalog + consumer binds + tests |
| E2 | `W2-04-E2-SEVERE-STAGES` | escalation data + response binds + storm soak |
| E3 | `W2-04-E3-AIR-RAD-FIELDS` | read models + surfaces + tests |
| E4 | `W2-04-E4-WATER-UNDERGROUND` | groundwater/subterranean binds + tests |
| E5 | `W2-04-E5-FIRE-FLOOD-SKY` | fire/flood/sky binds + tests |
| E6 | `W2-04-E6-SEASONS-GATES` | season axis + gate/hardening binds |
| E7 | `W2-04-E7-CLOSEOUT` | evidence + ledger proposals |

Coordination: W2-03 tunes responses to these inputs; W2-05 owns route/location
meaning; W2-06 writes prose for surfaced values; W2-02 repairs environment
defects found by E0.

---

## 19. Rollback and decline

| Point | Rollback | Decline consequence |
|---|---|---|
| 1 | restore effect catalog | distributed effects remain |
| 2 | remove stages | storm response implicit |
| 3 | remove read model | air unexplained |
| 4 | remove model | rad sources unexplained |
| 5 | remove binds | groundwater doctrine thin |
| 6 | remove binds | hazards untelegraphed |
| 7 | unbind weather inputs | fire/flood weather-blind |
| 8 | unbind ash | sky decorative |
| 9 | revert alignment | calendars may diverge |
| 10 | remove binds | gates/hardening unverified |

---

## 20. DoD and handoff

**Path A:** environment truth audited; dead rows fixed/annotated; premise and
unconsumed lists attached.

**Path B:** all of A, plus effect table, staged escalation, air/rad read
models, water/underground/fire/flood/sky binds, gate/hardening consumption —
each with tests and soak evidence.

**Path C:** all of B, plus signed dynamic couplings (contamination memory,
seasonal recharge, deep-zone ecology, route-condition memory) as separate
packages.

**Handoff fields:** outcome, files, contract (owners + effects + read models),
commands/soak, limitations, untouched shared paths, ledger proposals, Annex U
status.

### 20.1 First safe step

> E0 only: verify the environment owners/catalogs and produce the premise +
> unconsumed lists. No coupling before the audit.

---

*(Part A ends. Part B continues with worked environment designs, coupling
matrices, scenario walks, risks detail, and Annex U.)*---

# PART B — WORKED ENVIRONMENT DESIGNS AND COUPLING MATRICES

---

## B.1 The environment effect table (Point 1, Path B)

### B.1.1 Proposed catalog shape

```json
{
  "schema_version": 1,
  "kinds": [
    {
      "id": "Clear",
      "display_name": "Clear",
      "outdoor_rad_mult": 1.0,
      "wind_min_kph": 0, "wind_max_kph": 12,
      "visibility_band": "high",
      "water_collect_mult": 1.0,
      "power_demand_mult": 1.0,
      "fire_risk_mult": 1.0,
      "morale_drift_per_day": 0.0,
      "dust_particulates": 0.05,
      "tags": ["mild"]
    },
    {
      "id": "Ashfall",
      "display_name": "Ashfall",
      "outdoor_rad_mult": 1.2,
      "wind_min_kph": 5, "wind_max_kph": 25,
      "visibility_band": "low",
      "water_collect_mult": 0.4,
      "power_demand_mult": 1.1,
      "fire_risk_mult": 0.9,
      "morale_drift_per_day": -0.5,
      "dust_particulates": 0.6,
      "tags": ["ash", "respiratory"]
    },
    {
      "id": "FalloutStorm",
      "display_name": "Fallout Storm",
      "outdoor_rad_mult": 150.0,
      "wind_min_kph": 25, "wind_max_kph": 70,
      "visibility_band": "very_low",
      "water_collect_mult": 0.0,
      "power_demand_mult": 1.6,
      "fire_risk_mult": 0.5,
      "morale_drift_per_day": -2.0,
      "dust_particulates": 1.0,
      "tags": ["rad", "storm", "shelter_in"]
    },
    {
      "id": "BlackRain",
      "display_name": "Black Rain",
      "outdoor_rad_mult": 250.0,
      "wind_min_kph": 10, "wind_max_kph": 40,
      "visibility_band": "low",
      "water_collect_mult": 0.0,
      "power_demand_mult": 1.2,
      "fire_risk_mult": 0.2,
      "morale_drift_per_day": -1.5,
      "dust_particulates": 0.8,
      "hazmat_melt_mult": 5.0,
      "tags": ["rad", "corrosive", "shelter_in"]
    }
  ]
}
```

Notes:

- The values shown match or derive from the **verified constants** (150, 250,
  5) so the catalog is the authored home of values that currently live in code;
  migration of the constants to catalog reads is the Path B work.
- `dust_particulates` feeds the air model (Point 3).
- `visibility_band` feeds travel/gates (Point 10, W2-05).
- `water_collect_mult` feeds collection; `power_demand_mult` feeds the grid
  demand; `fire_risk_mult` feeds the fire hazard system.

### B.1.2 Consumer matrix

| Consumer | Reads | Owner | Test |
|---|---|---|---|
| outdoor exposure | `outdoor_rad_mult`, `hazmat_melt_mult` | `RadiationSystem`/`ExposureEnvironment` | storm > clear on a fixed survivor |
| water collection | `water_collect_mult` | water collection owner | black rain collects 0 |
| power demand | `power_demand_mult` | `PowerGridSystem` demand | fallout storm raises demand |
| fire risk | `fire_risk_mult` | `ShelterFireHazardSystem` | dry/windy vs. clear |
| needs morale | `morale_drift_per_day` | `NeedsSystem` external modifier | storm applies a daily modifier with source id |
| air model | `dust_particulates` | Point 3 read model | ash raises particulates |
| travel gates | `visibility_band`, wind | Point 10 / W2-05 | gate closes at very_low |
| briefing | `display_name`, tags | presentation | warning names the kind |

**Discipline:** the table is read once per weather roll/day and values are
cached with the weather state; consumers never re-read the file per tick.

### B.1.3 The no-double-count check

A common environment bug is two systems applying the same modifier. The test
matrix asserts exactly one application path per effect:

```text
For each effect column:
  grep consumers that read it
  assert exactly one owner mutates state from it
  assert presentation consumers never mutate
```

This check is the plan's equivalent of the maintenance single-writer rule.

---

## B.2 Severe weather escalation stages (Point 2)

### B.2.1 Authored stage model

```json
{
  "kind": "FalloutStorm",
  "stages": [
    { "id": "approach",  "after_hours": 0,  "outdoor_rad_mult": 3,   "warning": true,
      "shelter_actions": ["seal_openings"] },
    { "id": "arrival",   "after_hours": 2,  "outdoor_rad_mult": 40,  "warning": true,
      "shelter_actions": ["seal_openings", "filters_high", "suspend_travel"] },
    { "id": "sustained", "after_hours": 6,  "outdoor_rad_mult": 150, "warning": false,
      "shelter_actions": ["filters_high", "hepa_reserve", "power_priority_life"] },
    { "id": "clearing",  "after_hours": 18, "outdoor_rad_mult": 20,  "warning": true,
      "shelter_actions": ["filters_normal", "decon_entry"] },
    { "id": "aftermath", "after_hours": 24, "outdoor_rad_mult": 5,   "warning": false,
      "shelter_actions": ["washdown", "surface_sweep"] }
  ]
}
```

Rules:

- Stage durations derive from the storm's rolled intensity/duration (already in
  the weather state) — no fixed script that can contradict the roll.
- Each stage's effects are read from the effect table with a stage multiplier.
- The warning stages are surfaced by the weather intelligence owner.
- Aftermath routes through existing decontamination/washdown surfaces.

### B.2.2 Response consumption

| Response | Owner | Effect |
|---|---|---|
| seal openings | shelter openings/fire hazard owner | reduces infiltration |
| filters high | ventilation authority | increases filter load, reduces particulates indoors |
| suspend travel | route gates | gates close |
| power priority | `PowerGridSystem` | life-support priority |
| decon/washdown | `DecontaminationSystem` | reduces carried contamination |
| surface sweep | shelter/facility owner | reduces outdoor surface rads (bounded) |

### B.2.3 Storm soak

Run a seeded storm week and record: stage timeline, rad exposure indoors vs.
outdoors with/without responses, filter depletion, power demand peak, and
post-storm contamination. The soak is the evidence that escalation is readable
and survivable with responses.

---

## B.3 Air quality model (Point 3)

### B.3.1 Model shape

```csharp
public sealed record AirSample(
    float OutdoorParticulates,   // 0..1, from weather dust_particulates + ash layer
    float OutdoorRadsPerHour,    // exposure environment
    float IndoorParticulates,    // filtration-adjusted
    float IndoorRadsPerHour,     // shelter environment
    float WindKph, float WindDirectionDeg,
    string DominantSource,       // weather kind / ash / smoke / plume
    int? ArrivalEstimateDays);   // optional, from plume projection (C)
```

### B.3.2 Filtration math belongs to the owner

Indoor values come from the ventilation/filtration owner's own state; the model
does not compute filter efficiency. This keeps the owner single-writer.

### B.3.3 Plume projection (Path C)

A distance-band projection: given a source pattern (`fallout_patterns.json`),
wind direction/speed, and distance from the current location, estimate arrival
in days/hours with a stated confidence band (e.g., "≈2 days, ±1"). The estimate
is a read; it never sets a location's rad field. W2-05 may consume it.

### B.3.4 Honesty rules

- Always show "estimated" with basis (wind + pattern).
- Never invent precision (bands, not decimals).
- If wind data is missing, show "unknown until next reading".

---

## B.4 Radiation field model (Point 4)

### B.4.1 Source decomposition

| Source | Owner | Field contribution |
|---|---|---|
| location base | `locations.json` via `ExposureEnvironment` | base rads/hr at that place |
| weather storm | weather effect table | multiplier |
| carried contamination | decontamination system | dose contribution |
| artifact/zone | anomaly/zone data | local field |
| dose ledger | radiation system | accumulated dose (history, not field) |

The model shows the current field and its decomposition; the ledger records
exposure history. They are separate by design (the debt note explicitly says
"dose ledger stays radiation-only").

### B.4.2 Decay projection

Using an authored half-life per source class (e.g., location base decays with
campaign days; storm contribution decays with weather state):

```csharp
// projection only; does not mutate
float ProjectedRads(float current, float halfLifeDays, int daysAhead)
    => halfLifeDays <= 0 ? current
       : current * MathF.Pow(0.5f, daysAhead / halfLifeDays);
```

If the current game does not model decay at all (likely: base rads are static
per location), the projection is presented as "if conditions hold" — or the C
path signs a real decay field. The honest default is: **do not show a decay
projection the simulation does not implement**; show the current field and the
source decomposition first.

### B.4.3 Tests

- Decomposition sums to the owner's reported outdoor rads (tolerance 0).
- Storm multiplier changes the field as expected.
- Dosimeter reading basis matches the field model within tolerance.

---

## B.5 Groundwater coupling (Point 5)

### B.5.1 Model

```csharp
public sealed record GroundwaterSample(
    float HeadMeters, float TrendPerDay,
    float ContaminationIndex,      // advisory source
    int? DaysToThreshold,
    float DrawRate,                // current extraction
    bool SumpIntrusion);
```

All fields read from the piezometer/sump owners.

### B.5.2 Consumer binds

| Consumer | Reads | Effect |
|---|---|---|
| well yield | head | extraction rate scaling |
| water treatment | contamination index | treatment load/advisory |
| sump infiltration | head + flood state | infiltration rate |
| excavation | head vicinity | water intrusion risk |
| briefing | advisory status | "well recharge falling" |

### B.5.3 Recharge (Path C)

An additive field on the existing piezometer state:

```csharp
// additive, nullable; old saves deserialize absent
public float rechargePerDayFromPrecip { get; set; } // default 0
```

Semantics: during rain/snow melt windows, head rises by a small authored rate
capped by a ceiling; extraction lowers it; contamination can enter via flooding.
A signed persistence line is required (W-3) — otherwise this remains projection.

---

## B.6 Subterranean preparedness (Point 6)

### B.6.1 Hazard classes

| Hazard | Warning | Countermeasure | Owner |
|---|---|---|---|
| gas pocket | sensor reading | ventilation/purge | excavation hazard |
| collapse | support state | shoring | excavation hazard |
| water intrusion | seep observed | sealing/pump | sump/excavation |
| seismic aftershock | monitoring | evacuation/staging | seismic dynamics |
| unstable floor | survey mark | reinforced path | excavation hazard |

### B.6.2 Read model

```csharp
public sealed record SiteRisk(
    string SiteId, float GasRisk, float CollapseRisk, float WaterRisk,
    float SeismicRisk, float PreparednessScore,
    IReadOnlyList<string> ActiveWarnings);
```

### B.6.3 Tests

- Preparedness score reduces each risk's event probability (authored curve).
- Warning exists before each severe event class.
- No new hazard authority; ExcavationHazardSystem remains the evaluator.

---

## B.7 Fire/flood coupling matrix (Point 7)

| Environment input | Fire effect | Flood/sump effect |
|---|---|---|
| dry spell (no rain N days) | fire risk ↑ | sump level ↓ |
| high wind | spread rate ↑ | — |
| rain window | fire risk ↓ | sump inflow ↑ |
| freeze | — | surface line freeze; indoor burst risk (existing freeze logic) |
| storm surge/ash | — | outdoor collection stops |
| humidity (authored) | risk ↓ | — |

**Rule:** the fire hazard system and sump owner read the effect table; they do
not read weather kind by string except through the table.

**Cascade condition example** (coordinator-owned):

```json
{
  "id": "dry_store_fire",
  "conditions": { "environment": { "dryDaysMin": 5, "fire_risk_mult_min": 1.2 } },
  "effects": ["fire_incident"]
}
```

The coordinator evaluates; the environment only supplies inputs.

---

## B.8 Ash consumers (Point 8)

| Ash value | Consumer | Effect |
|---|---|---|
| layer thickness | roof load owner | structural load; reinforcement reduces risk |
| layer + wind | air model | particulates, visibility |
| deposition on water | water quality owner | collection quality ↓ |
| layer on panels | solar owner | generation ↓ |
| ash in air | filtration | filter load ↑ |
| ash on routes | route gates (W2-05) | speed/traction penalty |

Each consumer has a reduction response (sweep, wash, reinforce) through an
existing action path.

---

## B.9 Season axis (Point 9)

### B.9.1 Proposed shared axis

```json
{
  "schema_version": 1,
  "seasons": [
    { "id": "first_thaw",    "start_day": 0,  "end_day": 29 },
    { "id": "ash_settling",  "start_day": 30, "end_day": 59 },
    { "id": "...",           "start_day": 60, "end_day": 89 }
  ]
}
```

Weather keeps its own weights per window; wildlife keeps its own activity
windows; events keep their authored days. The axis only guarantees the **id and
bounds** agree, so a display can say "Ash Settling" and mean one thing.

### B.9.2 Test

```csharp
[Fact]
public void AllSeasonSystems_AgreeOnSeasonId()
{
    for (int day = 0; day < 180; day++)
    {
        Assert.Equal(SeasonAxis.IdFor(day), Weather.SeasonId(day));
        Assert.Equal(SeasonAxis.IdFor(day), Wildlife.SeasonId(day));
    }
}
```

---

## B.10 Gate and hardening truth (Point 10)

### B.10.1 Gate evaluation truth

`WeatherGateEvaluator` family evaluates conditions; the audit verifies each
authored gate has:

1. a condition referencing current state (not a constant),
2. an evaluator path (not orphaned data),
3. a readable reason string for the UI.

### B.10.2 Hardening consumption test

```csharp
[Fact]
public void Hardening_ReducesMeasuredEffect()
{
    var withReinforcement = new ShelterHarness().Install("roof_reinforcement");
    var without = new ShelterHarness();
    var storm = Storm("FalloutStorm", severity: 0.8);
    Assert.True(withReinforcement.RoofRisk(storm) < without.RoofRisk(storm));
}
```

### B.10.3 Route-condition memory (Path C, signed)

Only with a persistence line: a route's damaged state rides the route owner's
section (nullable, default healthy). W2-05 owns route meaning; this plan
supplies the weather damage event (storm severity crossing a threshold).

---

## B.11 Environment soak design

| Metric | Meaning |
|---|---|
| `kind_days[k]` | days per weather kind (should match season weights) |
| `storm_days` | severe days per 60 |
| `storm_exposure_indoor/outdoor` | response effectiveness |
| `fire_incidents` | fires per 60 and their suppression |
| `sump_flood_days` | flood days per 60 |
| `rad_warning_days` | days with a rad warning |
| `gate_closures` | route closures per 60 |
| `rad_field_sources` | decomposition coverage (no unexplained rads) |
| `air_particulates_peak` | peak air dirtiness and duration |

Bands are set in E0 like W2-03's pacing bands, so "the environment is alive but
not punishing" becomes measurable.

---

## B.12 Coupling anti-patterns

1. **Weather kind string checks in consumers** — read the table instead.
2. **Two modifiers for one effect** — the single-writer matrix check.
3. **UI-computed weather math** — always read the owner.
4. **New persistence for derived values** — derive, don't store.
5. **Storm scripts that contradict the roll** — derive stages from the actual
   state.
6. **Decay projections the sim does not implement** — show current truth, or
   sign the field.
7. **Deep couplings in one package** — C items are separate.

---

*(Part B ends. Part C continues with scenarios, Q&A, Annex U, appendices, and
closeout.)*---

# PART C — OPTION ANALYSIS, SCENARIOS, Q&A, ANNEX U, APPENDICES

---

## C.1 Per-point option analysis

### C.1.1 Point 1 — Weather effects

| Path | Gets you | Costs | Avoids |
|---|---|---|---|
| A | truth audit; dead rows found | no unification | invisible weather |
| B | one table, declared consumers | migration of constants + consumer edits | string-check sprawl |
| C | composable weather (ash+wind) | composition complexity | monotony |

### C.1.2 Point 2 — Severe escalation

| Path | Gets you | Costs | Avoids |
|---|---|---|---|
| A | authored stages | no response binds | flat storms |
| B | responses measurably help | shelter binds + storm soak | storms that ignore preparation |
| C | multi-day arcs | content + warning prose | one-beat disasters |

### C.1.3 Point 3 — Air/plume

| Path | Gets you | Costs | Avoids |
|---|---|---|---|
| A | air value inventory | no model | opaque air |
| B | one air model + surfaces | model + panel | contradictory readings |
| C | arrival estimates | projection tuning | surprise plumes |

### C.1.4 Point 4 — Radiation field

| Path | Gets you | Costs | Avoids |
|---|---|---|---|
| A | source audit | no explanation surface | unexplained ticks |
| B | decomposition + dosimeter basis | model + panel | "why is it rising?" |
| C | real decay/contamination memory | persistence line + balance | static forever-zones |

### C.1.5 Point 5 — Groundwater

| Path | Gets you | Costs | Avoids |
|---|---|---|---|
| A | advisory truth verified | no doctrine | opaque wells |
| B | head/trend/projection surfaces | model + binds | pumping blind |
| C | recharge + over-extraction | persistence + balance | infinite wells |

### C.1.6 Point 6 — Subterranean

| Path | Gets you | Costs | Avoids |
|---|---|---|---|
| A | hazard inventory | no binds | surprise collapses |
| B | preparedness + warnings | binds + site model | untelegraphed hazards |
| C | deep ecology/strata arcs | content | shallow underground |

### C.1.7 Point 7 — Fire/flood coupling

| Path | Gets you | Costs | Avoids |
|---|---|---|---|
| A | coupling audit | no binds | weather-blind hazards |
| B | dry→fire, rain→sump binds | coordinator condition edits | random disasters |
| C | disaster arcs | content | predictable monotony |

### C.1.8 Point 8 — Sky/ash

| Path | Gets you | Costs | Avoids |
|---|---|---|---|
| A | sky truth audit | no consumers | decorative sky |
| B | ash→load/filter/visibility binds | binds + tests | ash that means nothing |
| C | ash seasonality + defense coupling | two-system coupling | disconnected systems |

### C.1.9 Point 9 — Seasons

| Path | Gets you | Costs | Avoids |
|---|---|---|---|
| A | calendar cross-check | misalignment only fixed in data | contradicting calendars |
| B | shared axis | three-system alignment | drift to return |
| C | event orchestration | content | flat seasons |

### C.1.10 Point 10 — Gates/hardening

| Path | Gets you | Costs | Avoids |
|---|---|---|---|
| A | gate truth audit | no binds | gates that lie |
| B | current-condition evaluation + hardening effects | evaluator/consumer work | useless upgrades |
| C | route-condition memory | persistence + W2-05 coordination | instant route recovery |

---

## C.2 Cross-point interactions

| Interaction | Rule |
|---|---|
| 1 → 2 | stages multiply the effect table; no second table |
| 1 → 7 | fire/flood read the effect table |
| 3 ↔ 4 | air particulates and rads are reported together but owned separately |
| 4 ↔ 5 | water contamination is not radiation dose; two owned indices |
| 5 ↔ 7 | sump couples flood to groundwater; owner is sump |
| 8 ↔ 3 | ash feeds air; air model does not own ash |
| 8 ↔ 10 | ash on routes feeds gate conditions |
| 9 ↔ all | season axis aligns identities only |
| 10 ↔ W2-05 | route meaning outside; environment inputs only |
| 2 ↔ W2-03 | storm pressure is an environment input; needs response tunes in W2-03 |

---

## C.3 Scenarios

### C.3.1 Scenario — Path A, one week

1. E0 premise + unconsumed list (2 days).
2. E1-A: effect truth audit; fix dead rows (1 day).
3. E3-A: air/rad source audit (1 day).
4. E6-A: season cross-check (0.5 day).
5. E7 closeout (0.5 day).
Outcome: environment data truth restored; every claimed effect has a consumer
or an annotation.

### C.3.2 Scenario — Path B, one month

1. E0 (1 day).
2. E1-B: effect table + consumer binds + tests (5 days).
3. E2-B: stage model + responses + storm soak (5 days).
4. E3-B: air + rad models + surfaces (4 days).
5. E4-B: groundwater + subterranean binds (4 days).
6. E5-B: fire/flood + ash binds (4 days).
7. E6-B: season axis + gate/hardening binds (3 days).
8. E7 closeout (1 day).
Outcome: weather speaks to every owner; storms have stages and responses; air
and rads explain themselves; water/underground/fire/flood/sky are coupled
through their owners; gates tell the truth.

### C.3.3 Scenario — Path C, a season

Path B, plus separately signed packages: contamination memory, seasonal
recharge, deep-zone ecology, route-condition memory, ash seasonality + defense
coupling, plume arrival projection. Each has its own soak and revert path.

### C.3.4 Scenario — a coupling double-counts

If the fire risk rises twice under a dry spell (weather effect + an old code
path), the single-writer matrix check catches it: one path is removed, the
other retained, and the test asserts exactly one application. Never "halve both
numbers" to compensate.

### C.3.5 Scenario — a read model contradicts the sim

If the air model says "indoor rads low" while an exposure event fires, the
model's inputs are wrong (likely filtration state read from the wrong owner).
The rule: the model reads owners, so a contradiction is a wiring defect — route
it to W2-02 if it is a bug, never paper over it in the model.

### C.3.6 Scenario — route-state ownership conflict

W2-05 proposes route damage memory while W2-04 C proposes the same field. The
plans coordinate: W2-05 owns route state meaning; W2-04 supplies the weather
damage event; one claim edits the route owner. If no agreement, the field stays
a projection.

---

## C.4 Foreman Q&A

**Q1. Are we adding a climate system?**
No. Weather stays `WeatherSystem`; this plan authors its effect table and binds
consumers.

**Q2. Why migrate constants to a catalog?**
Because authored data is the repository's authority (Rule 3) and because the
table is how consumers stop string-checking weather kinds. The migration is
value-preserving (150/250/5 stay identical).

**Q3. Does the air model simulate anything?**
No. It reads weather, ash, and ventilation state and reports. Plume arrival is
a bounded projection.

**Q4. Will radiation decay change saves?**
Only under Point 4 Path C, which needs a signed persistence line. Default Path
B does not change radiation semantics.

**Q5. Does groundwater recharge change the sealed bridge?**
No. The bridge stays; recharge is an additive field on piezometer state under a
signed C line.

**Q6. How does this plan avoid stepping on W2-03?**
It supplies environment inputs and surfaces; W2-03 tunes the survival response.
Claims are disjoint by file.

**Q7. What about weather prose?**
W2-06 writes it. This plan names the values and surfaces.

**Q8. Is the storm stage model a new hazard system?**
No: stages parameterize the existing weather effects; reactions route through
existing shelter/decon/power owners.

**Q9. What is the smallest approval?**
E0 plus E1-A: an environment truth audit with dead rows fixed.

**Q10. What is the largest?**
Path C's six signed couplings, realistically a season of packages.

**Q11. How do we know the environment is "alive but fair"?**
The environment soak bands: storm days, response effectiveness, fire/flood
days, warnings, and gate closures within authored ranges.

**Q12. Can a builder add an environment feature not listed?**
Not without a new decision point and signature; the plan's authority is its ten
points.

---

## C.5 Annex U — Plan-unblocking (separately)

> **Wave 2 rule:** the environment plan's release implications live here, apart
> from the design body. No release without U.2 signatures.

### U.1 What W2-04 releases

| Blocked item | Release mechanism | Gate |
|---|---|---|
| Expansion 14 (Above the Ash) | Sky/ash meaning (Point 8) + weather effects (Point 1) provide the environmental substrate for sky trade/defense content | E1/E5 |
| Expansion 18 (Underneath) | Subterranean preparedness + groundwater (Points 5/6) give the underground its consequence layer | E4 |
| Expansion 21 (Grid) | Weather effects feed power demand (Point 1) and storm response (Point 2) | E1/E2 |
| Expansion 22/23 (Clean Flow / Alarm) | Groundwater + fire/flood coupling (Points 5/7) support their shared surfaces | E4/E5 |
| W2-03 pacing | Weather effect table + storm stages are the environment inputs the pacing curve reads | E1/E2 |
| W2-05 routes | Visibility/gate/ash inputs for location danger | E5/E6 |
| W2-06 prose | Named surfaces for weather/storm/air/ash prose | all |
| C3 192/199 boundaries | No funds/route DTO work; the plan stays read-only on those | n/a |

### U.2 Signatures needed

```text
[ ] I authorize E0 premise and the environment soak/bands.
[ ] I authorize Point 1 effect-table migration (values preserved).
[ ] I authorize Point 2 storm stages and shelter response binds.
[ ] I authorize Point 3 air read model (+ plume projection: [ ] yes [ ] no).
[ ] I authorize Point 4 radiation field model (+ decay field: [ ] no [ ] signed).
[ ] I authorize Point 5 groundwater model (+ recharge field: [ ] no [ ] signed).
[ ] I authorize Point 6 subterranean preparedness binds.
[ ] I authorize Point 7 fire/flood weather binds (coordinator stays owner).
[ ] I authorize Point 8 ash consumers.
[ ] I authorize Point 9 season axis alignment.
[ ] I authorize Point 10 gate/hardening truth (+ route memory: [ ] no [ ] signed, with W2-05).
```

### U.3 What W2-04 never touches for unblocking

- Needs/economy tuning (W2-03).
- Location tiers/map graph/route contracts (W2-05; UNBLOCK-02 for F13 routes).
- Prose (W2-06).
- Save schema without a signed line.
- Weather/rad authority renames.

### U.4 The coupling-release rule

Releasing an expansion (e.g., Expansion 14) requires the environmental substrate
to be **consumed** (a read model with a surface), not merely present. Presence
in a catalog is not gameplay reachability — the repository's own standard.

---

## C.6 Appendices

### C.6.1 Selection sheet

```text
ASHFALL WAVE 2 · PLAN 4 (ENVIRONMENT) · SELECTION
Date: ______  Foreman: ______  HEAD: ______

PLAN PATH: [ ] A Data & Truth  [ ] B Deepen (default)  [ ] C Living

01 weather effects .... [A] [B] [C]   default B
02 severe escalation .. [A] [B] [C]   default B
03 air/plume .......... [A] [B] [C]   default B
04 radiation field .... [A] [B] [C]   default B
05 groundwater ........ [A] [B] [C]   default B
06 subterranean ....... [A] [B] [C]   default B
07 fire/flood ......... [A] [B] [C]   default B
08 sky/ash ............ [A] [B] [C]   default B
09 seasons coupling ... [A] [B] [C]   default A
10 gates/hardening .... [A] [B] [C]   default B

Signature: ________________
```

### C.6.2 Environment band template

```markdown
# ASHFALL Environment Bands (<date>, HEAD <sha>)
| Metric | Band | Basis | Last changed |
|---|---|---|---|
| storm_days / 60 | 3–8 | soak 20×60 | <date> |
| fire_incidents / 60 | 0–3 | soak | <date> |
| sump_flood_days / 60 | 0–6 | soak | <date> |
| rad_warning_days / 60 | 2–10 | soak | <date> |
| unexplained_rad_sources | 0 | decomposition test | <date> |
```

### C.6.3 Coupling record template

```markdown
### Coupling <id>
- Input owner: <owner> → Output consumer: <owner>
- Field: <table column> → <consumed value>
- Single-writer check: <how>
- Test: <name> (<before/after>)
- Revert: <files>
```

### C.6.4 Glossary

| Term | Meaning |
|---|---|
| effect table | per-weather-kind authored effects |
| stage | escalation step derived from the rolled storm |
| field | current environmental value decomposition |
| plume | projected contamination movement |
| preparedness score | aggregate countermeasure readiness |
| season axis | shared season identity across systems |
| gate | authored route/action condition |
| band | intended environment metric range |

### C.6.5 What "done" looks like (default Path B)

| Point | Done when |
|---|---|
| 1 | every kind has a declared effect set; no string checks |
| 2 | stages + responses + storm soak in band |
| 3 | air model surface live; values owner-sourced |
| 4 | field decomposition matches exposure; dosimeter basis explained |
| 5 | head/trend surface live; advisory verified reaching treatment |
| 6 | warnings + countermeasures tested |
| 7 | dry/rain binds tested; cascade conditions authored |
| 8 | ash consumers tested; nothing decorative left unlabelled |
| 9 | season id agreement test green |
| 10 | gate reasons + hardening reductions tested |

---

## C.7 Final statement for W2-04

ASHFALL's environment is already deep; what it lacks is **declared effects,
readable fields, and honest couplings**. This plan authors the effect table,
stages the storms, explains the air and the rads, gives the ground and the
underground their consequences, couples fire/flood/sky through their owners,
aligns the seasons, and makes gates and hardening tell the truth.

Recommended: **Plan Path B** with Point 9 at Path A. Start with E0 — the
owner/catalog premise and the soak bands — because every later coupling is
measured against it.

---

**End of W2-04.** Proposal only; executes nothing; releases nothing without
U.2 signatures.

*Document control: W2-04 · Wave 2 · HEAD 5be1a30a · companion to W2-01/02/03/05/06.*---

# PART D — EXECUTION PLAYBOOK, COUPLING VERIFICATION, AND PER-POINT CHECKLISTS

---

## D.1 The environment tranche lifecycle

```text
1. OWNER   — name the owning system for the value/effect.
2. INPUT   — name the data source (catalog row, state field).
3. CONSUMER— name the reading owner (exactly one writer per effect).
4. TEST    — prove the read/effect with a focused test.
5. SOAK    — run the environment soak; metrics in band.
6. COMMIT  — one coupling per diff.
```

---

## D.2 Per-point checklists

### D.2.1 Point 1 — weather effects

```text
[ ] Effect table authored; values match verified constants (150/250/5)
[ ] Consumers: exposure, water, power, fire, morale, air, gates
[ ] No string kind checks outside the table
[ ] Single-writer matrix check green
[ ] Per-kind tests
[ ] Determinism: roll order unchanged
```

### D.2.2 Point 2 — severe escalation

```text
[ ] Stages authored per severe kind
[ ] Stage durations derive from the rolled state
[ ] Warnings on approach/arrival/clearing
[ ] Response binds: seal, filters, travel suspend, power priority, decon
[ ] Storm soak: indoor/outdoor exposure with/without responses
[ ] Every stage has an end condition
```

### D.2.3 Point 3 — air

```text
[ ] Air model reads weather/ash/ventilation/exposure owners
[ ] No indoor computation duplicated from the filtration owner
[ ] Surface: briefing + shelter panel
[ ] (C) plume projection basis stated; bands not decimals
[ ] No fabricated reading when data missing
```

### D.2.4 Point 4 — radiation field

```text
[ ] Source decomposition matches exposure environment
[ ] Dosimeter basis explained
[ ] No decay projection unless the sim implements it (or signed C field)
[ ] Dose ledger untouched
[ ] Source tests
```

### D.2.5 Point 5 — groundwater

```text
[ ] Advisory bridge verified reaching treatment
[ ] Head/trend/projection surfaced from piezometer owner
[ ] Consumer binds: yield, treatment, sump, excavation
[ ] (C) recharge field: signed + additive + nullable
[ ] Threshold tests
```

### D.2.6 Point 6 — subterranean

```text
[ ] Hazard classes with warning + countermeasure
[ ] Preparedness binds to excavation hazard owner
[ ] Site risk read model owner-sourced
[ ] Prepared vs unprepared probability tests
[ ] No new hazard authority
```

### D.2.7 Point 7 — fire/flood

```text
[ ] Fire risk reads effect table (dry/wind)
[ ] Sump inflow reads precipitation/freeze
[ ] Cascade conditions authored in the coordinator's catalog
[ ] No bypass of the cascade coordinator
[ ] Weather→hazard tests
```

### D.2.8 Point 8 — sky/ash

```text
[ ] Ash values enumerated; each has a consumer or is labelled decorative
[ ] Roof load / filtration / visibility / water quality / solar binds
[ ] Reduction responses exist (sweep/wash/reinforce)
[ ] No fabricated defense numbers
```

### D.2.9 Point 9 — seasons

```text
[ ] Three calendars aligned on id/bounds
[ ] Agreement test per campaign day
[ ] Weights remain owner-local
[ ] No duplicated season tables
```

### D.2.10 Point 10 — gates/hardening

```text
[ ] Every gate evaluates current state; reason readable
[ ] Every hardening has a measured reduction test
[ ] (C) route memory: signed, additive, W2-05 coordinated
[ ] No orphan gate data
```

---

## D.3 Coupling verification detail

### D.3.1 The single-writer matrix (template)

| Effect | Writer (mutates) | Readers (present) | Double-count? |
|---|---|---|---|
| outdoor rad mult | exposure environment | dosimeter, field model | no |
| water collection rate | collection owner | panel | no |
| power demand | grid demand | panel | no |
| fire risk | fire hazard system | briefing | no |
| morale drift | needs system | panel | no |
| indoor particulates | ventilation owner | air model | no |

The matrix is filled at E0 and re-checked per coupling tranche.

### D.3.2 Soak metrics (extended)

| Metric | Band |
|---|---|
| storm_days/60 | 3–8 |
| storm_indoor_rad_ratio | ≥ 5× reduction with responses |
| fire_incidents/60 | 0–3 |
| dry_day_fire_lift | ≥ 1.2× on dry spell |
| sump_flood_days/60 | 0–6 |
| rad_warning_days/60 | 2–10 |
| gate_closures/60 | 1–10 |
| unexplained_rad_sources | 0 |
| air_peak_hours | 6–48 |

### D.3.3 Environment scenario walks

#### Scenario — the effect table changes a constant

The code constant is replaced by a catalog read; the test asserts the value is
identical (150/250/5) so no balance change accompanies the migration.

#### Scenario — a storm stage never ends

If the roll produces an intensity with no clearing stage match, the stage model
clamps to the nearest authored stage and logs a warning; the test covers
out-of-range rolls.

#### Scenario — ash accumulation double-applies

Roof load and filtration both read ash: they read different columns
(`layer_thickness` for load, `dust_particulates` for filtration) — one input per
consumer. The matrix check catches a shared column.

#### Scenario — a read model shows impossible rads

The decomposition test fails; the input owner is wrong (likely filtration or
zone state). Route to W2-02 if it is a wiring defect; never adjust the model to
match a wrong reading.

---

## D.4 The environment knowledge base

### D.4.1 Owners and what they own

| Concern | Owner |
|---|---|
| weather kinds/windows/state | `WeatherSystem` + `weather_seasons.json` |
| weather effects | effect table (new, authored) |
| radiation dose | `RadiationSystem` + dose ledger (radiation-only) |
| exposure environment | `ExposureEnvironment` |
| fallout patterns | `FalloutSystem` + `fallout_patterns.json` |
| groundwater | piezometer/geothermal aquifer owners |
| underground | subterranean/excavation/seismic owners |
| fire | `ShelterFireHazardSystem` |
| flood | `SumpFloodingSystem` |
| cascades | `CascadeCoordinator` + catalog |
| sky/ash | sky layer armor/system |
| seasons | weather + wildlife + seasonal events |

### D.4.2 Anti-patterns

| Anti-pattern | Why |
|---|---|
| new climate system | Rule 5 |
| climate math in the UI | fabrication |
| two writers for one effect | double-count |
| persistence for derived values | schema creep |
| stage script contradicting the roll | lies |
| decay projections the sim lacks | false precision |
| deep couplings in one package | unrevertable |

### D.4.3 One-page summary

- **What:** ten environment points (effects, storms, air, rads, water,
  underground, fire/flood, ash, seasons, gates) with A/B/C each.
- **First:** E0 premise + soak bands.
- **Smallest:** E0 + E1 truth audit.
- **Largest:** Path C's six signed couplings.
- **Never:** new authorities, fabricated values, unsanctioned persistence.
- **Unblocking:** Annex U, signed, separate.

---

## D.5 Final checklist

```text
[ ] E0 premise + bands
[ ] Effect table live; constants migrated value-preserving
[ ] Storm stages + responses + soak
[ ] Air model surface live
[ ] Radiation decomposition matches exposure
[ ] Groundwater binds + advisory verified
[ ] Subterranean preparedness tested
[ ] Fire/flood weather binds tested
[ ] Ash consumers tested
[ ] Season axis agreement test green
[ ] Gate reasons + hardening reductions tested
[ ] Single-writer matrix green
[ ] Annex U signatures recorded
```

**End of W2-04 execution playbook.**

*Document control: W2-04 · Wave 2 · HEAD 5be1a30a · proposal only.*