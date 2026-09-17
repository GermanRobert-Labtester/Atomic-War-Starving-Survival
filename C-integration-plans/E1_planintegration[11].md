---
PLAN_ID: E1-11
PLAN_FAMILY: planintegration
SEQUENCE_INDEX: 11
STATUS: READY_FOR_EXECUTION_WHEN_RAILS_PASS
SOURCE_PLAN: "Plan 164 — Nuclear Winter Progression System"
SEQUENCE_FILENAME: "E1_planintegration[11].md"
PREVIOUS_FILENAME: "E1_planintegration[10].md"
NEXT_FILENAMES:
  - "E1_planintegration[12].md"
  - "E1_planintegration[13].md"
CATEGORY: LINK+CLIMATE+WEATHER+CAMPAIGN_PROGRESSION+PRESENTATION
PRIMARY_INTENT: "Create a deterministic long-horizon climate envelope that evolves across campaign time and drives existing weather, thermal, radiation, daylight, expedition, agriculture, power, and forecasting systems without duplicating them."
EXECUTION_STYLE: "Flagship integration plan"
PREMISE_VERIFICATION_REQUIRED: true
INTAKE_REQUIRED: true
ONE_AUTHORITY_PER_FACT: true
SECOND_WEATHER_SYSTEM_FORBIDDEN: true
SECOND_RADIATION_AUTHORITY_FORBIDDEN: true
SECOND_CALENDAR_FORBIDDEN: true
CAMPAIGN_LENGTH_RECONCILIATION_REQUIRED: true
RUNTIME_RISK: HIGH
SAVE_RISK: MEDIUM_HIGH
BALANCE_RISK: VERY_HIGH
CONTENT_RISK: HIGH
---

# E1 Plan Integration [11] — Nuclear Winter Progression, Seasonal Climate Envelope, Long-Horizon Environmental Escalation, and Adaptation

> **Sequence rule:** this file is `E1_planintegration[11].md`.
> The next files are `E1_planintegration[12].md`, `E1_planintegration[13].md`, and so on.
> The bracketed sequence number remains immediately before `.md`.

## 0. Mission

This plan converts Plan 164 into an implementation-grade climate-progression programme.

The source plan identifies a valuable gap: weather varies, but the larger climate apparently does not change
meaningfully over campaign time. A player can experience storms and cold today, but there is no reliable
long-horizon environmental arc that says the world is entering a deeper freeze, reaching a peak, briefly
relenting, or stabilizing into a harsher new normal.

The correct implementation is **not** a second `WeatherSystem` that rolls temperature, storms, radiation,
daylight, and consequences independently. Nuclear winter should be a slow-moving climate envelope. Existing
systems consume that envelope when deciding daily weather, forecast, shelter heat demand, expedition hazard,
radiation/fallout behavior, agriculture, solar generation, and player-facing warnings.

The first architectural question is also more important than the source plan's five named phases:

**How long is a real ASHFALL campaign at current HEAD, and what climate arc actually fits inside it?**

The source plan proposes Initial Days 1–90, Escalating 91–180, Peak 181–270, Late 271–360, and Stabilizing
361+. If a standard campaign is materially shorter than that, those thresholds would make most of the feature
unreachable. E1-11 therefore treats phase dates as tunable design hypotheses until campaign-calendar evidence
is verified.

## 1. Source Intent Preserved

Plan 164 asks for:

- progressive nuclear-winter phases;
- seasons;
- changing temperature;
- storm frequency/intensity;
- changing radiation pressure;
- seasonal daylight;
- integration with shelter heating;
- expedition risk;
- clothing;
- agriculture;
- solar generation;
- adaptation technologies;
- weather-station forecasting;
- climate events and quests;
- save/load;
- deterministic behavior;
- old-save compatibility;
- data-driven phase/season definitions;
- climate UI;
- headless selftest.

E1-11 retains those goals while splitting **climate drivers** from **weather realization** and from
**domain consequences**.

## 2. Core Architecture Thesis

```text
CampaignCalendar
      |
      v
NuclearWinter climate envelope
      |
      +--> phase / trend
      +--> seasonal climate band
      +--> temperature baseline modifier
      +--> storm probability/intensity envelope
      +--> fallout/radiation pressure input
      +--> daylight envelope
      +--> agriculture/growing-season envelope
      |
      v
Canonical consumers
      |
      +--> WeatherSystem produces actual daily weather
      +--> WeatherStationSystem forecasts
      +--> ShelterThermalSystem computes heat demand
      +--> Radiation/fallout authority computes exposure/contamination
      +--> ExpeditionSystem computes travel/hazard
      +--> Agriculture computes yields/growing windows
      +--> Power/Solar computes generation
      +--> Needs/Morale reacts through existing policies
```

Nuclear winter owns the **long-horizon environmental envelope**. It does not own the resulting day-to-day
state of those systems.

## 3. Non-Negotiable Rules

- `CampaignCalendar` owns campaign day/time.
- `WeatherSystem` owns realized weather.
- `WeatherStationSystem` owns weather/climate forecasting presentation where applicable.
- `ShelterThermalSystem` owns shelter temperatures and heating consequences.
- Radiation/fallout/exposure systems own actual radiation dose and contamination.
- Expedition authority owns expedition safety/travel consequences.
- Agriculture/food-production systems own crop yields and growing.
- Power authority owns solar generation and consumption.
- Needs/morale authorities own survivor physiological/psychological consequences.
- Nuclear winter may supply modifiers/envelopes, not directly write downstream state.
- Phase thresholds must be reconciled against actual campaign duration before implementation.
- No hard-coded 365-day arc unless the campaign contract actually supports it.
- No fixed 6–18 hour daylight range unless geography/calendar rules justify it.
- No generic `globalTemperature` field if actual ambient temperature is already owned by WeatherSystem.
- No generic `radiationLevel` field if radiation authority already owns environmental radiation.
- Climate progression must be deterministic from calendar + scenario seed + authored phase data.
- Randomness is optional and bounded; the long-horizon trend should not reroll unpredictably.
- Save/load should not need to persist purely derivable phase/season state.
- Climate difficulty must ramp gradually enough to preserve strategic adaptation windows.
- Seasonal respite is allowed but must not erase the long-term trend.
- Adaptation reduces consequences; it does not freeze or reverse the world climate unless explicitly designed.
- Climate monitoring improves information/forecast, not the underlying weather.
- Old saves should derive climate state from campaign day where safe.
- Data-driven phase/season definitions must pass integrity and reachability validation.

## 4. Acceptance Slices

### Slice A — Climate envelope only
Campaign day → phase/trend/season → modifiers exposed to WeatherSystem.

### Slice B — Shelter and expedition consequences
Temperature/storm envelope changes real heat and travel risk.

### Slice C — Radiation/daylight/agriculture/power
Only after their canonical authorities are verified.

### Slice D — Forecasting and adaptation
Weather station, clothing, insulation, greenhouse, research, monitoring.

### Slice E — Events/quests/content
Climate milestones become narrative.

Do not begin with five phases, four seasons, seven quest chains, and every downstream system simultaneously.


---

## E1-11A — Premise verification and climate-authority audit

**Goal:** Verify campaign duration, weather, seasonal data, radiation, daylight, thermal, expedition, agriculture, solar, adaptation, and save authorities before creating climate state.

### Required substeps

1. Inspect `CampaignCalendar`, campaign-duration/endgame logic, `WeatherSystem`, `WeatherStationSystem`, `weather_seasons.json`, weather cascade implementations, `ShelterThermalSystem`, radiation/fallout/exposure systems, contamination, expedition hazard, agriculture/greenhouse, solar/power, clothing/warmth, research, disasters, event/quest systems, and save registry.
2. Measure actual campaign duration and reachable day ranges in normal play.
3. Verify whether campaigns can exceed 180, 270, 360, or 365 days.
4. Verify whether weather already has seasonal weighting or temperature baselines.
5. Verify whether radiation has its own time decay/progression.
6. Verify whether daylight is currently modeled and where.
7. Verify whether solar generation uses daylight/weather inputs.
8. Verify whether agriculture has explicit seasons/growing windows.
9. Verify whether the weather station forecasts only daily weather or longer-horizon trends.
10. Search for existing `Climate`, `Season`, `NuclearWinter`, `Fallout`, `SeasonProfile`, or campaign-phase systems.
11. Create `docs/systems/NUCLEAR_WINTER_AUTHORITY_MAP.md`.
12. Create duplicate-search/intake evidence linking Plans 19, 83, 135, 141, 142, 158, and live systems.
13. Set `PREMISE_VERIFIED_AT` to current HEAD.

### Climate integration invariants

- CampaignCalendar owns time; climate evaluates from it.
- WeatherSystem owns realized daily weather.
- Thermal, radiation, expedition, agriculture, power, needs, and morale remain canonical.
- Phase/season thresholds are reachable in supported campaign timelines.
- Climate progression is deterministic and bounded.
- Save state does not duplicate derivable downstream facts.
- Adaptation changes consequences, not climate truth.
- Milestone events are idempotent across save/reload.

### Negative tests

- NuclearWinterSystem generates a second independent weather result.
- Climate state stores actual radiation dose or shelter room temperature.
- A phase is unreachable in every normal campaign.
- Reload re-fires a phase transition event.
- Adding a climate query consumes WeatherSystem RNG and changes unrelated outcomes.
- A weather station changes climate severity instead of information quality.
- Late-game modifiers grow without bound.
- A phase transition directly damages shelter or changes inventory.

### Acceptance evidence

- [ ] Deterministic boundary tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/migration/dedupe tests pass.
- [ ] Phase reachability is documented.
- [ ] Long-run distributions are measured.
- [ ] Adaptation window/balance evidence is captured.
- [ ] Docs/plan metadata are current.

---

## E1-11B — Campaign-length and phase-reachability ADR

**Goal:** Prevent a climate arc whose most important phases occur after the campaign normally ends.

### Required substeps

1. Record canonical min/typical/max campaign duration.
2. List ending triggers that can terminate campaigns before later climate phases.
3. Compare source Plan 164's 90-day phase blocks to actual campaign length.
4. Define whether the climate arc is normalized to campaign progress, absolute day ranges, scenario-specific durations, or a hybrid.
5. Prefer authored phase durations aligned to campaign design rather than percentages if campaign endings are intentionally variable.
6. Require every core climate phase to be reachable in at least one normal supported campaign mode.
7. Define late/stabilizing behavior for campaigns that continue beyond the main arc.
8. Define migration behavior for saves already beyond a changed threshold.
9. Publish an ADR before hard-coding phase day ranges.
10. Add reachability tests for early ending, standard campaign, long campaign, and endless/postgame if supported.

### Climate integration invariants

- CampaignCalendar owns time; climate evaluates from it.
- WeatherSystem owns realized daily weather.
- Thermal, radiation, expedition, agriculture, power, needs, and morale remain canonical.
- Phase/season thresholds are reachable in supported campaign timelines.
- Climate progression is deterministic and bounded.
- Save state does not duplicate derivable downstream facts.
- Adaptation changes consequences, not climate truth.
- Milestone events are idempotent across save/reload.

### Negative tests

- NuclearWinterSystem generates a second independent weather result.
- Climate state stores actual radiation dose or shelter room temperature.
- A phase is unreachable in every normal campaign.
- Reload re-fires a phase transition event.
- Adding a climate query consumes WeatherSystem RNG and changes unrelated outcomes.
- A weather station changes climate severity instead of information quality.
- Late-game modifiers grow without bound.
- A phase transition directly damages shelter or changes inventory.

### Acceptance evidence

- [ ] Deterministic boundary tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/migration/dedupe tests pass.
- [ ] Phase reachability is documented.
- [ ] Long-run distributions are measured.
- [ ] Adaptation window/balance evidence is captured.
- [ ] Docs/plan metadata are current.

---

## E1-11C — Climate-envelope ownership ADR

**Goal:** Define nuclear winter as a slow climate driver rather than a replacement weather/radiation system.

### Required substeps

1. Compare a standalone `NuclearWinterSystem`, extension of WeatherSystem season state, and a pure `ClimateEnvelopeProvider`/campaign-world service.
2. Define climate-owned fields as phase ID, climate trend ID, seasonal index/season reference if not already canonical, and authored envelope parameters.
3. Mark realized temperature, current weather, actual radiation dose, survivor mood, heating fuel, crop yield, and solar output as external facts.
4. Define consumers and adapter interfaces.
5. Define whether climate state is fully derivable from day/seed/data or needs persisted one-time transition state.
6. Define feature flags for each downstream consumer.
7. Define rollback that disables new modifiers without corrupting weather saves.
8. Require second-tool review before code.

### Climate integration invariants

- CampaignCalendar owns time; climate evaluates from it.
- WeatherSystem owns realized daily weather.
- Thermal, radiation, expedition, agriculture, power, needs, and morale remain canonical.
- Phase/season thresholds are reachable in supported campaign timelines.
- Climate progression is deterministic and bounded.
- Save state does not duplicate derivable downstream facts.
- Adaptation changes consequences, not climate truth.
- Milestone events are idempotent across save/reload.

### Negative tests

- NuclearWinterSystem generates a second independent weather result.
- Climate state stores actual radiation dose or shelter room temperature.
- A phase is unreachable in every normal campaign.
- Reload re-fires a phase transition event.
- Adding a climate query consumes WeatherSystem RNG and changes unrelated outcomes.
- A weather station changes climate severity instead of information quality.
- Late-game modifiers grow without bound.
- A phase transition directly damages shelter or changes inventory.

### Acceptance evidence

- [ ] Deterministic boundary tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/migration/dedupe tests pass.
- [ ] Phase reachability is documented.
- [ ] Long-run distributions are measured.
- [ ] Adaptation window/balance evidence is captured.
- [ ] Docs/plan metadata are current.

---

## E1-11D — Climate phase catalog schema

**Goal:** Define data-driven long-horizon phases whose durations, severity, and envelopes are validated against campaign reachability.

### Required substeps

1. Define phase ID, localization key, start/end policy, climate-trend category, base temperature offset/range, weather-pattern multiplier set, storm envelope, fallout-pressure input, daylight/season modifiers, adaptation pressure tags, transition event key, and optional scenario applicability.
2. Do not encode actual current temperature or radiation dose.
3. Allow scenario-specific phase tables if campaign variants differ.
4. Validate phase ordering, gaps, overlaps, unreachable phases, invalid multipliers, unknown weather tags, and missing transition keys.
5. Define terminal/open-ended phase semantics.
6. Version the catalog.
7. Use five named phases only if campaign-length ADR accepts them.
8. Add data-integrity tests.

### Climate integration invariants

- CampaignCalendar owns time; climate evaluates from it.
- WeatherSystem owns realized daily weather.
- Thermal, radiation, expedition, agriculture, power, needs, and morale remain canonical.
- Phase/season thresholds are reachable in supported campaign timelines.
- Climate progression is deterministic and bounded.
- Save state does not duplicate derivable downstream facts.
- Adaptation changes consequences, not climate truth.
- Milestone events are idempotent across save/reload.

### Negative tests

- NuclearWinterSystem generates a second independent weather result.
- Climate state stores actual radiation dose or shelter room temperature.
- A phase is unreachable in every normal campaign.
- Reload re-fires a phase transition event.
- Adding a climate query consumes WeatherSystem RNG and changes unrelated outcomes.
- A weather station changes climate severity instead of information quality.
- Late-game modifiers grow without bound.
- A phase transition directly damages shelter or changes inventory.

### Acceptance evidence

- [ ] Deterministic boundary tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/migration/dedupe tests pass.
- [ ] Phase reachability is documented.
- [ ] Long-run distributions are measured.
- [ ] Adaptation window/balance evidence is captured.
- [ ] Docs/plan metadata are current.

---

## E1-11E — Seasonal cycle schema and authority reconciliation

**Goal:** Represent seasonal variation without duplicating existing weather-season data.

### Required substeps

1. Inspect `weather_seasons.json` and determine whether it should be expanded instead of creating a second seasonal catalog.
2. Define one canonical season ID set.
3. Define season duration/transition policy.
4. Define seasonal temperature envelope, weather weights, daylight curve reference, agriculture tags, and storm modifiers.
5. Do not duplicate weather-type definitions.
6. Support phase × season composition through clear multiplier precedence.
7. Validate complete cycle, no invalid weather references, no zero-length season, and reachability within campaign.
8. Define whether seasons continue across nuclear-winter phase transitions or reset.
9. Add deterministic transition tests.
10. Prefer one season catalog consumed by both climate and WeatherSystem.

### Climate integration invariants

- CampaignCalendar owns time; climate evaluates from it.
- WeatherSystem owns realized daily weather.
- Thermal, radiation, expedition, agriculture, power, needs, and morale remain canonical.
- Phase/season thresholds are reachable in supported campaign timelines.
- Climate progression is deterministic and bounded.
- Save state does not duplicate derivable downstream facts.
- Adaptation changes consequences, not climate truth.
- Milestone events are idempotent across save/reload.

### Negative tests

- NuclearWinterSystem generates a second independent weather result.
- Climate state stores actual radiation dose or shelter room temperature.
- A phase is unreachable in every normal campaign.
- Reload re-fires a phase transition event.
- Adding a climate query consumes WeatherSystem RNG and changes unrelated outcomes.
- A weather station changes climate severity instead of information quality.
- Late-game modifiers grow without bound.
- A phase transition directly damages shelter or changes inventory.

### Acceptance evidence

- [ ] Deterministic boundary tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/migration/dedupe tests pass.
- [ ] Phase reachability is documented.
- [ ] Long-run distributions are measured.
- [ ] Adaptation window/balance evidence is captured.
- [ ] Docs/plan metadata are current.

---

## E1-11F — Deterministic climate evaluation API

**Goal:** Provide pure queries from calendar/scenario to climate envelope wherever possible.

### Required substeps

1. Define `GetClimateEnvelope(day, scenarioId)` or equivalent.
2. Return phase/season IDs and typed modifiers rather than mutable global state.
3. Keep evaluation deterministic.
4. Use interpolation/curves only when authored and testable.
5. Define boundary-day behavior exactly.
6. Use stable numeric precision/rounding.
7. Add tests for every phase boundary and season boundary.
8. Add tests across save/reload and different seeds where seed matters.
9. Cache only if profiling justifies it.
10. Expose debug breakdown showing phase contribution + season contribution + scenario modifiers.

### Climate integration invariants

- CampaignCalendar owns time; climate evaluates from it.
- WeatherSystem owns realized daily weather.
- Thermal, radiation, expedition, agriculture, power, needs, and morale remain canonical.
- Phase/season thresholds are reachable in supported campaign timelines.
- Climate progression is deterministic and bounded.
- Save state does not duplicate derivable downstream facts.
- Adaptation changes consequences, not climate truth.
- Milestone events are idempotent across save/reload.

### Negative tests

- NuclearWinterSystem generates a second independent weather result.
- Climate state stores actual radiation dose or shelter room temperature.
- A phase is unreachable in every normal campaign.
- Reload re-fires a phase transition event.
- Adding a climate query consumes WeatherSystem RNG and changes unrelated outcomes.
- A weather station changes climate severity instead of information quality.
- Late-game modifiers grow without bound.
- A phase transition directly damages shelter or changes inventory.

### Acceptance evidence

- [ ] Deterministic boundary tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/migration/dedupe tests pass.
- [ ] Phase reachability is documented.
- [ ] Long-run distributions are measured.
- [ ] Adaptation window/balance evidence is captured.
- [ ] Docs/plan metadata are current.

---

## E1-11G — WeatherSystem integration

**Goal:** Drive daily weather distributions through the climate envelope while preserving WeatherSystem ownership.

### Required substeps

1. Identify WeatherSystem's weather-selection inputs.
2. Pass temperature baseline/offset, weather-pattern weights, storm probability/intensity envelope, snow/fog/precipitation bias, or other supported parameters.
3. Do not generate weather inside NuclearWinterSystem.
4. Define modifier precedence with existing seasonal/weather data.
5. Use one deterministic RNG stream/order to prevent climate integration from destabilizing unrelated replay behavior.
6. Add tests proving same seed/day yields same weather before/after save.
7. Add baseline comparison with climate modifiers disabled.
8. Add tests for mild, peak, respite, and stabilized envelopes.
9. Measure weather-distribution shift over long simulations.

### Climate integration invariants

- CampaignCalendar owns time; climate evaluates from it.
- WeatherSystem owns realized daily weather.
- Thermal, radiation, expedition, agriculture, power, needs, and morale remain canonical.
- Phase/season thresholds are reachable in supported campaign timelines.
- Climate progression is deterministic and bounded.
- Save state does not duplicate derivable downstream facts.
- Adaptation changes consequences, not climate truth.
- Milestone events are idempotent across save/reload.

### Negative tests

- NuclearWinterSystem generates a second independent weather result.
- Climate state stores actual radiation dose or shelter room temperature.
- A phase is unreachable in every normal campaign.
- Reload re-fires a phase transition event.
- Adding a climate query consumes WeatherSystem RNG and changes unrelated outcomes.
- A weather station changes climate severity instead of information quality.
- Late-game modifiers grow without bound.
- A phase transition directly damages shelter or changes inventory.

### Acceptance evidence

- [ ] Deterministic boundary tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/migration/dedupe tests pass.
- [ ] Phase reachability is documented.
- [ ] Long-run distributions are measured.
- [ ] Adaptation window/balance evidence is captured.
- [ ] Docs/plan metadata are current.

---

## E1-11H — Ambient temperature integration

**Goal:** Use climate as a baseline for realized outdoor temperature without storing a duplicate global temperature.

### Required substeps

1. Identify how WeatherSystem produces ambient temperature.
2. Apply climate phase + season baseline/offset.
3. Allow daily weather to create deviations around that baseline.
4. Define min/max clamps from authored data.
5. Do not write shelter room temperatures directly.
6. Add tests for phase/season combinations, storm/cold-snap deviations, and deterministic replay.
7. Measure temperature distributions by phase.
8. Keep source-plan Celsius numbers as tuning hypotheses until weather scale is audited.

### Climate integration invariants

- CampaignCalendar owns time; climate evaluates from it.
- WeatherSystem owns realized daily weather.
- Thermal, radiation, expedition, agriculture, power, needs, and morale remain canonical.
- Phase/season thresholds are reachable in supported campaign timelines.
- Climate progression is deterministic and bounded.
- Save state does not duplicate derivable downstream facts.
- Adaptation changes consequences, not climate truth.
- Milestone events are idempotent across save/reload.

### Negative tests

- NuclearWinterSystem generates a second independent weather result.
- Climate state stores actual radiation dose or shelter room temperature.
- A phase is unreachable in every normal campaign.
- Reload re-fires a phase transition event.
- Adding a climate query consumes WeatherSystem RNG and changes unrelated outcomes.
- A weather station changes climate severity instead of information quality.
- Late-game modifiers grow without bound.
- A phase transition directly damages shelter or changes inventory.

### Acceptance evidence

- [ ] Deterministic boundary tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/migration/dedupe tests pass.
- [ ] Phase reachability is documented.
- [ ] Long-run distributions are measured.
- [ ] Adaptation window/balance evidence is captured.
- [ ] Docs/plan metadata are current.

---

## E1-11I — Storm frequency and intensity envelope

**Goal:** Change the probability and severity distribution of storms through WeatherSystem rather than a second storm generator.

### Required substeps

1. Identify canonical storm/weather types and severity levels.
2. Define phase and season multipliers for storm occurrence.
3. Define intensity distribution modifiers separately from frequency.
4. Prevent probabilities from exceeding valid bounds.
5. Define rare extreme events with explicit caps/cooldowns if supported.
6. Do not directly damage shelter or expeditions from climate code.
7. Add Monte Carlo/deterministic distribution tests over long seeded runs.
8. Measure expected storm-days per phase/season.
9. Route actual storm consequences through existing weather cascade/disaster rails.

### Climate integration invariants

- CampaignCalendar owns time; climate evaluates from it.
- WeatherSystem owns realized daily weather.
- Thermal, radiation, expedition, agriculture, power, needs, and morale remain canonical.
- Phase/season thresholds are reachable in supported campaign timelines.
- Climate progression is deterministic and bounded.
- Save state does not duplicate derivable downstream facts.
- Adaptation changes consequences, not climate truth.
- Milestone events are idempotent across save/reload.

### Negative tests

- NuclearWinterSystem generates a second independent weather result.
- Climate state stores actual radiation dose or shelter room temperature.
- A phase is unreachable in every normal campaign.
- Reload re-fires a phase transition event.
- Adding a climate query consumes WeatherSystem RNG and changes unrelated outcomes.
- A weather station changes climate severity instead of information quality.
- Late-game modifiers grow without bound.
- A phase transition directly damages shelter or changes inventory.

### Acceptance evidence

- [ ] Deterministic boundary tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/migration/dedupe tests pass.
- [ ] Phase reachability is documented.
- [ ] Long-run distributions are measured.
- [ ] Adaptation window/balance evidence is captured.
- [ ] Docs/plan metadata are current.

---

## E1-11J — Radiation/fallout progression boundary

**Goal:** Use climate phase as an input to environmental radiation/fallout only where physically/gameplay-wise justified, without owning dose.

### Required substeps

1. Audit canonical radiation, fallout, contamination, deposition, weather-washout, and exposure systems.
2. Determine whether radiation should actually peak/decline with climate phase or has its own independent fallout timeline.
3. Do not assume cold itself raises radiation.
4. Define climate-to-fallout coupling only for supported mechanisms such as resuspension, deposition, storm transport, or scenario-level fallout progression.
5. Keep environmental radiation/dose in the radiation authority.
6. Represent climate input as fallout-pressure/deposition/weather modifier where appropriate.
7. Add tests for no-radiation coupling if rail absent, supported coupling, declining fallout timeline, and save/load.
8. Document gameplay abstraction separately from real-world physical claims.

### Climate integration invariants

- CampaignCalendar owns time; climate evaluates from it.
- WeatherSystem owns realized daily weather.
- Thermal, radiation, expedition, agriculture, power, needs, and morale remain canonical.
- Phase/season thresholds are reachable in supported campaign timelines.
- Climate progression is deterministic and bounded.
- Save state does not duplicate derivable downstream facts.
- Adaptation changes consequences, not climate truth.
- Milestone events are idempotent across save/reload.

### Negative tests

- NuclearWinterSystem generates a second independent weather result.
- Climate state stores actual radiation dose or shelter room temperature.
- A phase is unreachable in every normal campaign.
- Reload re-fires a phase transition event.
- Adding a climate query consumes WeatherSystem RNG and changes unrelated outcomes.
- A weather station changes climate severity instead of information quality.
- Late-game modifiers grow without bound.
- A phase transition directly damages shelter or changes inventory.

### Acceptance evidence

- [ ] Deterministic boundary tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/migration/dedupe tests pass.
- [ ] Phase reachability is documented.
- [ ] Long-run distributions are measured.
- [ ] Adaptation window/balance evidence is captured.
- [ ] Docs/plan metadata are current.

---

## E1-11K — Daylight and calendar integration

**Goal:** Model seasonal daylight through one calendar/geography authority rather than arbitrary climate-owned hours.

### Required substeps

1. Audit whether latitude/location is fixed, abstracted, or represented.
2. Audit current day/night cycle.
3. If daylight is already canonical, climate references its season only.
4. If not, define an authored daylight curve tied to canonical season/day-of-cycle.
5. Do not blindly use 6-18 hours unless consistent with setting/geography.
6. Define dawn/dusk or daylight-hours precision needed by gameplay.
7. Add tests for seasonal extrema, transition smoothness, long campaign cycles, and save/load.
8. Expose daylight to power/work/morale consumers through one read-only authority.

### Climate integration invariants

- CampaignCalendar owns time; climate evaluates from it.
- WeatherSystem owns realized daily weather.
- Thermal, radiation, expedition, agriculture, power, needs, and morale remain canonical.
- Phase/season thresholds are reachable in supported campaign timelines.
- Climate progression is deterministic and bounded.
- Save state does not duplicate derivable downstream facts.
- Adaptation changes consequences, not climate truth.
- Milestone events are idempotent across save/reload.

### Negative tests

- NuclearWinterSystem generates a second independent weather result.
- Climate state stores actual radiation dose or shelter room temperature.
- A phase is unreachable in every normal campaign.
- Reload re-fires a phase transition event.
- Adding a climate query consumes WeatherSystem RNG and changes unrelated outcomes.
- A weather station changes climate severity instead of information quality.
- Late-game modifiers grow without bound.
- A phase transition directly damages shelter or changes inventory.

### Acceptance evidence

- [ ] Deterministic boundary tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/migration/dedupe tests pass.
- [ ] Phase reachability is documented.
- [ ] Long-run distributions are measured.
- [ ] Adaptation window/balance evidence is captured.
- [ ] Docs/plan metadata are current.

---

## E1-11L — ShelterThermal integration

**Goal:** Make worsening outdoor climate increase heating pressure through the existing thermal model.

### Required substeps

1. Pass realized outdoor temperature/weather into ShelterThermalSystem through existing boundary.
2. Do not have NuclearWinterSystem calculate fuel consumption.
3. Let insulation/heating upgrades modify thermal response canonically.
4. Define cold-wave/weather effects through WeatherSystem output.
5. Add tests for same shelter in Initial versus Peak envelope, insulated versus uninsulated, heating on/off, and save/load.
6. Measure heating fuel/energy demand distributions by phase.
7. Ensure climate progression cannot bypass thermal caps/physics.

### Climate integration invariants

- CampaignCalendar owns time; climate evaluates from it.
- WeatherSystem owns realized daily weather.
- Thermal, radiation, expedition, agriculture, power, needs, and morale remain canonical.
- Phase/season thresholds are reachable in supported campaign timelines.
- Climate progression is deterministic and bounded.
- Save state does not duplicate derivable downstream facts.
- Adaptation changes consequences, not climate truth.
- Milestone events are idempotent across save/reload.

### Negative tests

- NuclearWinterSystem generates a second independent weather result.
- Climate state stores actual radiation dose or shelter room temperature.
- A phase is unreachable in every normal campaign.
- Reload re-fires a phase transition event.
- Adding a climate query consumes WeatherSystem RNG and changes unrelated outcomes.
- A weather station changes climate severity instead of information quality.
- Late-game modifiers grow without bound.
- A phase transition directly damages shelter or changes inventory.

### Acceptance evidence

- [ ] Deterministic boundary tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/migration/dedupe tests pass.
- [ ] Phase reachability is documented.
- [ ] Long-run distributions are measured.
- [ ] Adaptation window/balance evidence is captured.
- [ ] Docs/plan metadata are current.

---

## E1-11M — Expedition hazard integration

**Goal:** Make climate change travel risk and timing through expedition/weather authorities.

### Required substeps

1. Identify expedition weather/hazard adapters.
2. Use realized weather, ambient temperature, daylight, and radiation exposure from their canonical owners.
3. Do not apply generic climate-risk percentage directly.
4. Allow climate phase to affect forecast/planning windows through weather distributions.
5. Integrate clothing/vehicle/mobile-base/outpost capabilities through their own systems.
6. Add tests for warm respite, peak cold, severe storm, low daylight, radiation/fallout condition, and safe equipment.
7. Measure expedition cancellation/delay/injury rates by climate phase.

### Climate integration invariants

- CampaignCalendar owns time; climate evaluates from it.
- WeatherSystem owns realized daily weather.
- Thermal, radiation, expedition, agriculture, power, needs, and morale remain canonical.
- Phase/season thresholds are reachable in supported campaign timelines.
- Climate progression is deterministic and bounded.
- Save state does not duplicate derivable downstream facts.
- Adaptation changes consequences, not climate truth.
- Milestone events are idempotent across save/reload.

### Negative tests

- NuclearWinterSystem generates a second independent weather result.
- Climate state stores actual radiation dose or shelter room temperature.
- A phase is unreachable in every normal campaign.
- Reload re-fires a phase transition event.
- Adding a climate query consumes WeatherSystem RNG and changes unrelated outcomes.
- A weather station changes climate severity instead of information quality.
- Late-game modifiers grow without bound.
- A phase transition directly damages shelter or changes inventory.

### Acceptance evidence

- [ ] Deterministic boundary tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/migration/dedupe tests pass.
- [ ] Phase reachability is documented.
- [ ] Long-run distributions are measured.
- [ ] Adaptation window/balance evidence is captured.
- [ ] Docs/plan metadata are current.

---

## E1-11N — Clothing and cold-protection integration

**Goal:** Use clothing warmth/protection to mediate cold exposure without duplicating survivor-temperature state.

### Required substeps

1. Audit clothing/equipment warmth system.
2. Expose ambient/weather conditions to clothing/needs exposure calculations.
3. Do not store clothing resistance in climate state.
4. Define warning thresholds for inadequate gear.
5. Add tests for no gear, adequate gear, wet/storm condition if modeled, damaged clothing, and save/load.
6. Feature-gate if clothing rail is not ready.

### Climate integration invariants

- CampaignCalendar owns time; climate evaluates from it.
- WeatherSystem owns realized daily weather.
- Thermal, radiation, expedition, agriculture, power, needs, and morale remain canonical.
- Phase/season thresholds are reachable in supported campaign timelines.
- Climate progression is deterministic and bounded.
- Save state does not duplicate derivable downstream facts.
- Adaptation changes consequences, not climate truth.
- Milestone events are idempotent across save/reload.

### Negative tests

- NuclearWinterSystem generates a second independent weather result.
- Climate state stores actual radiation dose or shelter room temperature.
- A phase is unreachable in every normal campaign.
- Reload re-fires a phase transition event.
- Adding a climate query consumes WeatherSystem RNG and changes unrelated outcomes.
- A weather station changes climate severity instead of information quality.
- Late-game modifiers grow without bound.
- A phase transition directly damages shelter or changes inventory.

### Acceptance evidence

- [ ] Deterministic boundary tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/migration/dedupe tests pass.
- [ ] Phase reachability is documented.
- [ ] Long-run distributions are measured.
- [ ] Adaptation window/balance evidence is captured.
- [ ] Docs/plan metadata are current.

---

## E1-11O — Agriculture and growing-season integration

**Goal:** Make climate affect agriculture through canonical crop/growing authorities.

### Required substeps

1. Audit farming, greenhouse, seasonality, temperature requirements, soil/light, and crop-yield systems.
2. Pass seasonal/daylight/temperature envelopes or realized conditions into agriculture.
3. Do not directly change food inventory.
4. Define open-field versus greenhouse behavior.
5. Allow greenhouse/heating/light capability to extend growing windows if real systems support it.
6. Add tests for winter, brief summer, greenhouse, power loss, crop failure/recovery, and save/load.
7. Measure food-production pressure by climate stage.
8. Defer if agriculture authority does not yet exist.

### Climate integration invariants

- CampaignCalendar owns time; climate evaluates from it.
- WeatherSystem owns realized daily weather.
- Thermal, radiation, expedition, agriculture, power, needs, and morale remain canonical.
- Phase/season thresholds are reachable in supported campaign timelines.
- Climate progression is deterministic and bounded.
- Save state does not duplicate derivable downstream facts.
- Adaptation changes consequences, not climate truth.
- Milestone events are idempotent across save/reload.

### Negative tests

- NuclearWinterSystem generates a second independent weather result.
- Climate state stores actual radiation dose or shelter room temperature.
- A phase is unreachable in every normal campaign.
- Reload re-fires a phase transition event.
- Adding a climate query consumes WeatherSystem RNG and changes unrelated outcomes.
- A weather station changes climate severity instead of information quality.
- Late-game modifiers grow without bound.
- A phase transition directly damages shelter or changes inventory.

### Acceptance evidence

- [ ] Deterministic boundary tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/migration/dedupe tests pass.
- [ ] Phase reachability is documented.
- [ ] Long-run distributions are measured.
- [ ] Adaptation window/balance evidence is captured.
- [ ] Docs/plan metadata are current.

---

## E1-11P — Solar and power integration

**Goal:** Make seasonal daylight and weather influence solar generation through canonical power systems.

### Required substeps

1. Audit solar generation formulas.
2. Pass daylight and weather/cloud/storm conditions.
3. Do not directly add/subtract power from climate code.
4. Define snow/ice/maintenance effects only if solar system supports them.
5. Add tests for short-day winter, long-day respite, storm/cloud cover, damaged panels, and save/load.
6. Measure expected generation by season.

### Climate integration invariants

- CampaignCalendar owns time; climate evaluates from it.
- WeatherSystem owns realized daily weather.
- Thermal, radiation, expedition, agriculture, power, needs, and morale remain canonical.
- Phase/season thresholds are reachable in supported campaign timelines.
- Climate progression is deterministic and bounded.
- Save state does not duplicate derivable downstream facts.
- Adaptation changes consequences, not climate truth.
- Milestone events are idempotent across save/reload.

### Negative tests

- NuclearWinterSystem generates a second independent weather result.
- Climate state stores actual radiation dose or shelter room temperature.
- A phase is unreachable in every normal campaign.
- Reload re-fires a phase transition event.
- Adding a climate query consumes WeatherSystem RNG and changes unrelated outcomes.
- A weather station changes climate severity instead of information quality.
- Late-game modifiers grow without bound.
- A phase transition directly damages shelter or changes inventory.

### Acceptance evidence

- [ ] Deterministic boundary tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/migration/dedupe tests pass.
- [ ] Phase reachability is documented.
- [ ] Long-run distributions are measured.
- [ ] Adaptation window/balance evidence is captured.
- [ ] Docs/plan metadata are current.

---

## E1-11Q — Morale and seasonal-affect boundary

**Goal:** Allow climate to influence psychology only through concrete conditions and existing mental-health systems.

### Required substeps

1. Audit morale/stress/seasonal affect mechanics.
2. Prefer consequences from low daylight, confinement, cold, repeated storms, shortages, or successful respite events rather than a direct `season = winter -> morale -X` rule.
3. Use mental-health/social consequence events.
4. Allow positive respite/spring events if canonical morale policy supports them.
5. Add cooldown/diminishing returns.
6. Add tests for short daylight, adequate lighting, warm shelter, repeated storms, seasonal respite, and no duplicate effects.
7. Do not store climate-owned morale state.

### Climate integration invariants

- CampaignCalendar owns time; climate evaluates from it.
- WeatherSystem owns realized daily weather.
- Thermal, radiation, expedition, agriculture, power, needs, and morale remain canonical.
- Phase/season thresholds are reachable in supported campaign timelines.
- Climate progression is deterministic and bounded.
- Save state does not duplicate derivable downstream facts.
- Adaptation changes consequences, not climate truth.
- Milestone events are idempotent across save/reload.

### Negative tests

- NuclearWinterSystem generates a second independent weather result.
- Climate state stores actual radiation dose or shelter room temperature.
- A phase is unreachable in every normal campaign.
- Reload re-fires a phase transition event.
- Adding a climate query consumes WeatherSystem RNG and changes unrelated outcomes.
- A weather station changes climate severity instead of information quality.
- Late-game modifiers grow without bound.
- A phase transition directly damages shelter or changes inventory.

### Acceptance evidence

- [ ] Deterministic boundary tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/migration/dedupe tests pass.
- [ ] Phase reachability is documented.
- [ ] Long-run distributions are measured.
- [ ] Adaptation window/balance evidence is captured.
- [ ] Docs/plan metadata are current.

---

## E1-11R — Climate adaptation capability model

**Goal:** Treat adaptation as capabilities in existing shelter/equipment/research systems.

### Required substeps

1. Catalog adaptation levers already represented: insulation, heaters, redundant power, storm protection, warm clothing, greenhouse, food reserves, radio/weather monitoring, vehicles, outposts.
2. Define climate-relevant capability tags.
3. Do not create a generic `climateResistance` stat unless a canonical consumer actually needs one.
4. Use each authority's real effect.
5. Create a planning read model summarizing readiness without owning the underlying state.
6. Add tests for individual adaptations and combinations.
7. Measure which adaptations materially reduce climate losses.

### Climate integration invariants

- CampaignCalendar owns time; climate evaluates from it.
- WeatherSystem owns realized daily weather.
- Thermal, radiation, expedition, agriculture, power, needs, and morale remain canonical.
- Phase/season thresholds are reachable in supported campaign timelines.
- Climate progression is deterministic and bounded.
- Save state does not duplicate derivable downstream facts.
- Adaptation changes consequences, not climate truth.
- Milestone events are idempotent across save/reload.

### Negative tests

- NuclearWinterSystem generates a second independent weather result.
- Climate state stores actual radiation dose or shelter room temperature.
- A phase is unreachable in every normal campaign.
- Reload re-fires a phase transition event.
- Adding a climate query consumes WeatherSystem RNG and changes unrelated outcomes.
- A weather station changes climate severity instead of information quality.
- Late-game modifiers grow without bound.
- A phase transition directly damages shelter or changes inventory.

### Acceptance evidence

- [ ] Deterministic boundary tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/migration/dedupe tests pass.
- [ ] Phase reachability is documented.
- [ ] Long-run distributions are measured.
- [ ] Adaptation window/balance evidence is captured.
- [ ] Docs/plan metadata are current.

---

## E1-11S — Research integration

**Goal:** Use research to unlock adaptation capabilities rather than modifying climate directly.

### Required substeps

1. Audit ResearchSystem/Plan 141 status.
2. Reference real research IDs in adaptation blueprints/technology.
3. Research authority owns unlocked state.
4. Climate UI may recommend unavailable technologies but does not unlock them.
5. Define advanced forecast/monitoring research separately from heating/greenhouse technologies.
6. Add tests for locked/unlocked adaptation, removed research ID, and old-save compatibility.
7. Feature-gate if research rail is not ready.

### Climate integration invariants

- CampaignCalendar owns time; climate evaluates from it.
- WeatherSystem owns realized daily weather.
- Thermal, radiation, expedition, agriculture, power, needs, and morale remain canonical.
- Phase/season thresholds are reachable in supported campaign timelines.
- Climate progression is deterministic and bounded.
- Save state does not duplicate derivable downstream facts.
- Adaptation changes consequences, not climate truth.
- Milestone events are idempotent across save/reload.

### Negative tests

- NuclearWinterSystem generates a second independent weather result.
- Climate state stores actual radiation dose or shelter room temperature.
- A phase is unreachable in every normal campaign.
- Reload re-fires a phase transition event.
- Adding a climate query consumes WeatherSystem RNG and changes unrelated outcomes.
- A weather station changes climate severity instead of information quality.
- Late-game modifiers grow without bound.
- A phase transition directly damages shelter or changes inventory.

### Acceptance evidence

- [ ] Deterministic boundary tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/migration/dedupe tests pass.
- [ ] Phase reachability is documented.
- [ ] Long-run distributions are measured.
- [ ] Adaptation window/balance evidence is captured.
- [ ] Docs/plan metadata are current.

---

## E1-11T — Weather-station long-horizon forecasting

**Goal:** Extend forecasting to expose climate trends and seasonal transitions without granting perfect daily foresight.

### Required substeps

1. Audit WeatherStationSystem forecast horizon/accuracy.
2. Expose phase/season transition forecasts as deterministic long-horizon information.
3. Keep daily storm/weather forecast accuracy under WeatherStation authority.
4. Define forecast confidence/horizon by station capability if supported.
5. Monitoring does not change climate.
6. Allow E1-3 information-flow systems to broadcast warnings if active.
7. Add tests for known transition, near-term storm, no station, degraded station, save/load, and deterministic climate forecast.
8. Show distinction between climate trend and weather forecast.

### Climate integration invariants

- CampaignCalendar owns time; climate evaluates from it.
- WeatherSystem owns realized daily weather.
- Thermal, radiation, expedition, agriculture, power, needs, and morale remain canonical.
- Phase/season thresholds are reachable in supported campaign timelines.
- Climate progression is deterministic and bounded.
- Save state does not duplicate derivable downstream facts.
- Adaptation changes consequences, not climate truth.
- Milestone events are idempotent across save/reload.

### Negative tests

- NuclearWinterSystem generates a second independent weather result.
- Climate state stores actual radiation dose or shelter room temperature.
- A phase is unreachable in every normal campaign.
- Reload re-fires a phase transition event.
- Adding a climate query consumes WeatherSystem RNG and changes unrelated outcomes.
- A weather station changes climate severity instead of information quality.
- Late-game modifiers grow without bound.
- A phase transition directly damages shelter or changes inventory.

### Acceptance evidence

- [ ] Deterministic boundary tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/migration/dedupe tests pass.
- [ ] Phase reachability is documented.
- [ ] Long-run distributions are measured.
- [ ] Adaptation window/balance evidence is captured.
- [ ] Docs/plan metadata are current.

---

## E1-11U — Phase-transition events

**Goal:** Create milestone events from authoritative climate transitions rather than random duplicate weather events.

### Required substeps

1. Emit transition event when phase changes.
2. Use stable phase-transition operation IDs.
3. Ensure transition fires once across save/reload.
4. Allow events such as First Frost, Deepening Winter, Peak Freeze, Thaw/Respite, New Normal only if they align with actual phase model.
5. Do not directly apply downstream resource/morale effects.
6. Route journal/quest hooks through canonical systems.
7. Add tests for boundary day, reload boundary, skipped days/time jumps, and already-fired transition.
8. Keep daily storms in weather-event rails.

### Climate integration invariants

- CampaignCalendar owns time; climate evaluates from it.
- WeatherSystem owns realized daily weather.
- Thermal, radiation, expedition, agriculture, power, needs, and morale remain canonical.
- Phase/season thresholds are reachable in supported campaign timelines.
- Climate progression is deterministic and bounded.
- Save state does not duplicate derivable downstream facts.
- Adaptation changes consequences, not climate truth.
- Milestone events are idempotent across save/reload.

### Negative tests

- NuclearWinterSystem generates a second independent weather result.
- Climate state stores actual radiation dose or shelter room temperature.
- A phase is unreachable in every normal campaign.
- Reload re-fires a phase transition event.
- Adding a climate query consumes WeatherSystem RNG and changes unrelated outcomes.
- A weather station changes climate severity instead of information quality.
- Late-game modifiers grow without bound.
- A phase transition directly damages shelter or changes inventory.

### Acceptance evidence

- [ ] Deterministic boundary tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/migration/dedupe tests pass.
- [ ] Phase reachability is documented.
- [ ] Long-run distributions are measured.
- [ ] Adaptation window/balance evidence is captured.
- [ ] Docs/plan metadata are current.

---

## E1-11V — Season-transition events

**Goal:** Surface seasonal rhythm without flooding the journal.

### Required substeps

1. Emit season transition through one canonical event.
2. Deduplicate by cycle/year/season identity.
3. Keep flavor events optional and bounded.
4. Do not generate a separate climate event for every day of weather.
5. Allow seasonal planning prompts.
6. Add tests across campaign start in mid-season, save/load boundary, long campaigns, and time jumps.
7. Use journal significance classes.

### Climate integration invariants

- CampaignCalendar owns time; climate evaluates from it.
- WeatherSystem owns realized daily weather.
- Thermal, radiation, expedition, agriculture, power, needs, and morale remain canonical.
- Phase/season thresholds are reachable in supported campaign timelines.
- Climate progression is deterministic and bounded.
- Save state does not duplicate derivable downstream facts.
- Adaptation changes consequences, not climate truth.
- Milestone events are idempotent across save/reload.

### Negative tests

- NuclearWinterSystem generates a second independent weather result.
- Climate state stores actual radiation dose or shelter room temperature.
- A phase is unreachable in every normal campaign.
- Reload re-fires a phase transition event.
- Adding a climate query consumes WeatherSystem RNG and changes unrelated outcomes.
- A weather station changes climate severity instead of information quality.
- Late-game modifiers grow without bound.
- A phase transition directly damages shelter or changes inventory.

### Acceptance evidence

- [ ] Deterministic boundary tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/migration/dedupe tests pass.
- [ ] Phase reachability is documented.
- [ ] Long-run distributions are measured.
- [ ] Adaptation window/balance evidence is captured.
- [ ] Docs/plan metadata are current.

---

## E1-11W — Climate quest hook contract

**Goal:** Create optional objectives around preparation/adaptation through QuestSystem.

### Required substeps

1. Use phase/season transition and forecast events as provenance.
2. Quest authority owns lifecycle.
3. Implement only a small high-value set first: Winter Preparation, Cold Snap, Monitoring, Adaptation/Storm Shelter if supporting systems exist.
4. Do not require systems that are not implemented.
5. Allow quests to fail/expire if climate window passes.
6. Prevent duplicate quest spawn after reload.
7. Add tests for eligibility, duplicate guard, unavailable adaptation rail, expiration, and completion.
8. Keep celebration/flavor quests optional.

### Climate integration invariants

- CampaignCalendar owns time; climate evaluates from it.
- WeatherSystem owns realized daily weather.
- Thermal, radiation, expedition, agriculture, power, needs, and morale remain canonical.
- Phase/season thresholds are reachable in supported campaign timelines.
- Climate progression is deterministic and bounded.
- Save state does not duplicate derivable downstream facts.
- Adaptation changes consequences, not climate truth.
- Milestone events are idempotent across save/reload.

### Negative tests

- NuclearWinterSystem generates a second independent weather result.
- Climate state stores actual radiation dose or shelter room temperature.
- A phase is unreachable in every normal campaign.
- Reload re-fires a phase transition event.
- Adding a climate query consumes WeatherSystem RNG and changes unrelated outcomes.
- A weather station changes climate severity instead of information quality.
- Late-game modifiers grow without bound.
- A phase transition directly damages shelter or changes inventory.

### Acceptance evidence

- [ ] Deterministic boundary tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/migration/dedupe tests pass.
- [ ] Phase reachability is documented.
- [ ] Long-run distributions are measured.
- [ ] Adaptation window/balance evidence is captured.
- [ ] Docs/plan metadata are current.

---

## E1-11X — Disaster-response integration

**Goal:** Allow severe realized storms/cold events to trigger disaster rails, not climate phases directly.

### Required substeps

1. Audit disaster-response system.
2. Trigger disasters from actual weather/hazard thresholds.
3. Climate increases probability by shifting weather distributions.
4. Do not have phase transition itself damage shelter.
5. Route damage/injuries through canonical disaster/health/shelter systems.
6. Add tests for high-severity realized storm, peak climate with no storm, mitigation capability, and save/load.
7. Feature-gate if disaster rail is not ready.

### Climate integration invariants

- CampaignCalendar owns time; climate evaluates from it.
- WeatherSystem owns realized daily weather.
- Thermal, radiation, expedition, agriculture, power, needs, and morale remain canonical.
- Phase/season thresholds are reachable in supported campaign timelines.
- Climate progression is deterministic and bounded.
- Save state does not duplicate derivable downstream facts.
- Adaptation changes consequences, not climate truth.
- Milestone events are idempotent across save/reload.

### Negative tests

- NuclearWinterSystem generates a second independent weather result.
- Climate state stores actual radiation dose or shelter room temperature.
- A phase is unreachable in every normal campaign.
- Reload re-fires a phase transition event.
- Adding a climate query consumes WeatherSystem RNG and changes unrelated outcomes.
- A weather station changes climate severity instead of information quality.
- Late-game modifiers grow without bound.
- A phase transition directly damages shelter or changes inventory.

### Acceptance evidence

- [ ] Deterministic boundary tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/migration/dedupe tests pass.
- [ ] Phase reachability is documented.
- [ ] Long-run distributions are measured.
- [ ] Adaptation window/balance evidence is captured.
- [ ] Docs/plan metadata are current.

---

## E1-11Y — Climate UI and read model

**Goal:** Present long-horizon environmental progression clearly without pretending to know hidden future weather.

### Required substeps

1. Show current climate phase/trend, current season, current realized weather summary, recent temperature trend, forecasted phase/season transition, daylight trend, and major adaptation warnings.
2. Clearly separate climate trend from daily forecast.
3. Show canonical shelter preparedness summaries: heating reserve, insulation, clothing, greenhouse, monitoring, without duplicating state.
4. Do not expose exact hidden RNG probabilities unless design requires it.
5. Show phase timeline normalized to actual campaign design.
6. Add snapshot tests for early/mid/peak/respite/late phases, no weather station, and old-save migration.
7. Use authoritative read models only.

### Climate integration invariants

- CampaignCalendar owns time; climate evaluates from it.
- WeatherSystem owns realized daily weather.
- Thermal, radiation, expedition, agriculture, power, needs, and morale remain canonical.
- Phase/season thresholds are reachable in supported campaign timelines.
- Climate progression is deterministic and bounded.
- Save state does not duplicate derivable downstream facts.
- Adaptation changes consequences, not climate truth.
- Milestone events are idempotent across save/reload.

### Negative tests

- NuclearWinterSystem generates a second independent weather result.
- Climate state stores actual radiation dose or shelter room temperature.
- A phase is unreachable in every normal campaign.
- Reload re-fires a phase transition event.
- Adding a climate query consumes WeatherSystem RNG and changes unrelated outcomes.
- A weather station changes climate severity instead of information quality.
- Late-game modifiers grow without bound.
- A phase transition directly damages shelter or changes inventory.

### Acceptance evidence

- [ ] Deterministic boundary tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/migration/dedupe tests pass.
- [ ] Phase reachability is documented.
- [ ] Long-run distributions are measured.
- [ ] Adaptation window/balance evidence is captured.
- [ ] Docs/plan metadata are current.

---

## E1-11Z — Climate journal and chronicle

**Goal:** Record major environmental transitions without spamming routine weather.

### Required substeps

1. Use canonical journal/chronicle.
2. Record phase transitions, seasonal transitions where narratively important, record cold/storm events, first successful adaptation, and major climate survival milestones.
3. Do not permanently record every weather tick.
4. Use stable event IDs for dedupe.
5. Allow E1-5 legacy to retain major climate-survival records.
6. Add retention and ordering tests.
7. Keep journal presentation separate from climate save truth.

### Climate integration invariants

- CampaignCalendar owns time; climate evaluates from it.
- WeatherSystem owns realized daily weather.
- Thermal, radiation, expedition, agriculture, power, needs, and morale remain canonical.
- Phase/season thresholds are reachable in supported campaign timelines.
- Climate progression is deterministic and bounded.
- Save state does not duplicate derivable downstream facts.
- Adaptation changes consequences, not climate truth.
- Milestone events are idempotent across save/reload.

### Negative tests

- NuclearWinterSystem generates a second independent weather result.
- Climate state stores actual radiation dose or shelter room temperature.
- A phase is unreachable in every normal campaign.
- Reload re-fires a phase transition event.
- Adding a climate query consumes WeatherSystem RNG and changes unrelated outcomes.
- A weather station changes climate severity instead of information quality.
- Late-game modifiers grow without bound.
- A phase transition directly damages shelter or changes inventory.

### Acceptance evidence

- [ ] Deterministic boundary tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/migration/dedupe tests pass.
- [ ] Phase reachability is documented.
- [ ] Long-run distributions are measured.
- [ ] Adaptation window/balance evidence is captured.
- [ ] Docs/plan metadata are current.

---

## E1-11AA — Old-save compatibility and climate derivation

**Goal:** Load existing saves without inventing inconsistent climate history.

### Required substeps

1. Prefer deriving current phase/season from campaign day + scenario + current catalog.
2. Do not synthesize past journal events for transitions that occurred before the feature existed.
3. Preserve current WeatherSystem state on migration.
4. Preserve radiation/thermal/agriculture state in their canonical owners.
5. Default fired-transition-history appropriately so old saves do not spam every missed transition on first load.
6. Define migration when an old save starts beyond later phase thresholds.
7. Version any persisted transition state.
8. Add fixtures at early, mid, late, boundary, and beyond-arc days.
9. Make migration idempotent.

### Climate integration invariants

- CampaignCalendar owns time; climate evaluates from it.
- WeatherSystem owns realized daily weather.
- Thermal, radiation, expedition, agriculture, power, needs, and morale remain canonical.
- Phase/season thresholds are reachable in supported campaign timelines.
- Climate progression is deterministic and bounded.
- Save state does not duplicate derivable downstream facts.
- Adaptation changes consequences, not climate truth.
- Milestone events are idempotent across save/reload.

### Negative tests

- NuclearWinterSystem generates a second independent weather result.
- Climate state stores actual radiation dose or shelter room temperature.
- A phase is unreachable in every normal campaign.
- Reload re-fires a phase transition event.
- Adding a climate query consumes WeatherSystem RNG and changes unrelated outcomes.
- A weather station changes climate severity instead of information quality.
- Late-game modifiers grow without bound.
- A phase transition directly damages shelter or changes inventory.

### Acceptance evidence

- [ ] Deterministic boundary tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/migration/dedupe tests pass.
- [ ] Phase reachability is documented.
- [ ] Long-run distributions are measured.
- [ ] Adaptation window/balance evidence is captured.
- [ ] Docs/plan metadata are current.

---

## E1-11AB — Save contract and derivable-state minimization

**Goal:** Persist only climate facts that cannot be safely recomputed.

### Required substeps

1. Determine whether phase/season are pure functions of calendar/day.
2. If so, do not persist them as independent mutable truth.
3. Persist scenario/climate-model version reference if needed.
4. Persist fired milestone IDs/cycle counters only if events require them.
5. Persist stochastic climate anomalies only if the design adds them.
6. Do not persist duplicate ambient temperature/weather/radiation/daylight.
7. Define schema version.
8. Add round-trip tests at boundaries.
9. Ensure restore does not replay phase transition events.

### Climate integration invariants

- CampaignCalendar owns time; climate evaluates from it.
- WeatherSystem owns realized daily weather.
- Thermal, radiation, expedition, agriculture, power, needs, and morale remain canonical.
- Phase/season thresholds are reachable in supported campaign timelines.
- Climate progression is deterministic and bounded.
- Save state does not duplicate derivable downstream facts.
- Adaptation changes consequences, not climate truth.
- Milestone events are idempotent across save/reload.

### Negative tests

- NuclearWinterSystem generates a second independent weather result.
- Climate state stores actual radiation dose or shelter room temperature.
- A phase is unreachable in every normal campaign.
- Reload re-fires a phase transition event.
- Adding a climate query consumes WeatherSystem RNG and changes unrelated outcomes.
- A weather station changes climate severity instead of information quality.
- Late-game modifiers grow without bound.
- A phase transition directly damages shelter or changes inventory.

### Acceptance evidence

- [ ] Deterministic boundary tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/migration/dedupe tests pass.
- [ ] Phase reachability is documented.
- [ ] Long-run distributions are measured.
- [ ] Adaptation window/balance evidence is captured.
- [ ] Docs/plan metadata are current.

---

## E1-11AC — Determinism and RNG partitioning

**Goal:** Prevent climate integration from changing unrelated seeded outcomes through RNG-call-order drift.

### Required substeps

1. Audit seeded RNG service and streams.
2. Prefer pure deterministic climate curves with no RNG for base progression.
3. If random climate anomalies exist, use a dedicated deterministic stream keyed by climate/day/event.
4. Do not consume WeatherSystem's RNG merely to evaluate climate.
5. Add tests that climate queries are order-independent.
6. Add same-seed replay tests.
7. Add save/reload mid-day tests.
8. Record deterministic fixtures for phase/season/weather distributions.

### Climate integration invariants

- CampaignCalendar owns time; climate evaluates from it.
- WeatherSystem owns realized daily weather.
- Thermal, radiation, expedition, agriculture, power, needs, and morale remain canonical.
- Phase/season thresholds are reachable in supported campaign timelines.
- Climate progression is deterministic and bounded.
- Save state does not duplicate derivable downstream facts.
- Adaptation changes consequences, not climate truth.
- Milestone events are idempotent across save/reload.

### Negative tests

- NuclearWinterSystem generates a second independent weather result.
- Climate state stores actual radiation dose or shelter room temperature.
- A phase is unreachable in every normal campaign.
- Reload re-fires a phase transition event.
- Adding a climate query consumes WeatherSystem RNG and changes unrelated outcomes.
- A weather station changes climate severity instead of information quality.
- Late-game modifiers grow without bound.
- A phase transition directly damages shelter or changes inventory.

### Acceptance evidence

- [ ] Deterministic boundary tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/migration/dedupe tests pass.
- [ ] Phase reachability is documented.
- [ ] Long-run distributions are measured.
- [ ] Adaptation window/balance evidence is captured.
- [ ] Docs/plan metadata are current.

---

## E1-11AD — Difficulty and adaptation-window tuning

**Goal:** Make worsening climate urgent without becoming an unavoidable death spiral.

### Required substeps

1. Define expected preparation windows before each major escalation.
2. Measure heating demand, expedition risk, food pressure, power pressure, and radiation exposure by phase.
3. Ensure players receive warnings before step changes.
4. Ensure at least one viable adaptation path exists for each pressure.
5. Ensure multiple adaptation strategies remain viable.
6. Include respite periods if they improve pacing.
7. Check late-game climate against expected shelter tech progression.
8. Check no-upgrade/minimal-upgrade survivability baseline according to difficulty design.
9. Run Monte Carlo/seeded balance simulations.
10. Do not set phase modifiers solely from narrative adjectives.

### Climate integration invariants

- CampaignCalendar owns time; climate evaluates from it.
- WeatherSystem owns realized daily weather.
- Thermal, radiation, expedition, agriculture, power, needs, and morale remain canonical.
- Phase/season thresholds are reachable in supported campaign timelines.
- Climate progression is deterministic and bounded.
- Save state does not duplicate derivable downstream facts.
- Adaptation changes consequences, not climate truth.
- Milestone events are idempotent across save/reload.

### Negative tests

- NuclearWinterSystem generates a second independent weather result.
- Climate state stores actual radiation dose or shelter room temperature.
- A phase is unreachable in every normal campaign.
- Reload re-fires a phase transition event.
- Adding a climate query consumes WeatherSystem RNG and changes unrelated outcomes.
- A weather station changes climate severity instead of information quality.
- Late-game modifiers grow without bound.
- A phase transition directly damages shelter or changes inventory.

### Acceptance evidence

- [ ] Deterministic boundary tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/migration/dedupe tests pass.
- [ ] Phase reachability is documented.
- [ ] Long-run distributions are measured.
- [ ] Adaptation window/balance evidence is captured.
- [ ] Docs/plan metadata are current.

---

## E1-11AE — Campaign-ending and climate-arc interaction

**Goal:** Ensure climate progression supports endgame pacing instead of fighting it.

### Required substeps

1. Map climate milestones against canonical ending windows.
2. Determine whether certain endings depend on climate phase.
3. Ensure important climax phases occur before most campaigns end if intended.
4. Define post-ending simulation behavior if campaigns continue.
5. Ensure endless/extended campaigns use stable cyclical late climate rather than unbounded worsening.
6. Add tests for ending before peak, at peak, after stabilization, and continued simulation.
7. Keep ending authority separate.

### Climate integration invariants

- CampaignCalendar owns time; climate evaluates from it.
- WeatherSystem owns realized daily weather.
- Thermal, radiation, expedition, agriculture, power, needs, and morale remain canonical.
- Phase/season thresholds are reachable in supported campaign timelines.
- Climate progression is deterministic and bounded.
- Save state does not duplicate derivable downstream facts.
- Adaptation changes consequences, not climate truth.
- Milestone events are idempotent across save/reload.

### Negative tests

- NuclearWinterSystem generates a second independent weather result.
- Climate state stores actual radiation dose or shelter room temperature.
- A phase is unreachable in every normal campaign.
- Reload re-fires a phase transition event.
- Adding a climate query consumes WeatherSystem RNG and changes unrelated outcomes.
- A weather station changes climate severity instead of information quality.
- Late-game modifiers grow without bound.
- A phase transition directly damages shelter or changes inventory.

### Acceptance evidence

- [ ] Deterministic boundary tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/migration/dedupe tests pass.
- [ ] Phase reachability is documented.
- [ ] Long-run distributions are measured.
- [ ] Adaptation window/balance evidence is captured.
- [ ] Docs/plan metadata are current.

---

## E1-11AF — Climate trend stabilization and long-run behavior

**Goal:** Define what happens after the authored escalation arc ends.

### Required substeps

1. Choose stable seasonal cycle, slow recovery, or scenario-specific late trend.
2. Do not let modifiers grow unbounded.
3. Define terminal phase clamps.
4. Define radiation/fallout late behavior through its own authority.
5. Define weather distributions for long campaigns.
6. Add 720-day or max-supported long-run test if campaign modes permit.
7. Ensure floating-point/interpolation stability.
8. Document late-game model explicitly.

### Climate integration invariants

- CampaignCalendar owns time; climate evaluates from it.
- WeatherSystem owns realized daily weather.
- Thermal, radiation, expedition, agriculture, power, needs, and morale remain canonical.
- Phase/season thresholds are reachable in supported campaign timelines.
- Climate progression is deterministic and bounded.
- Save state does not duplicate derivable downstream facts.
- Adaptation changes consequences, not climate truth.
- Milestone events are idempotent across save/reload.

### Negative tests

- NuclearWinterSystem generates a second independent weather result.
- Climate state stores actual radiation dose or shelter room temperature.
- A phase is unreachable in every normal campaign.
- Reload re-fires a phase transition event.
- Adding a climate query consumes WeatherSystem RNG and changes unrelated outcomes.
- A weather station changes climate severity instead of information quality.
- Late-game modifiers grow without bound.
- A phase transition directly damages shelter or changes inventory.

### Acceptance evidence

- [ ] Deterministic boundary tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/migration/dedupe tests pass.
- [ ] Phase reachability is documented.
- [ ] Long-run distributions are measured.
- [ ] Adaptation window/balance evidence is captured.
- [ ] Docs/plan metadata are current.

---

## E1-11AG — Data-integrity and cross-catalog validation

**Goal:** Validate climate, season, weather, research, adaptation, quest, and event references.

### Required substeps

1. Validate phase IDs/order/ranges.
2. Validate season IDs/cycle completeness.
3. Validate weather-pattern references.
4. Validate modifier ranges and finite values.
5. Validate research/adaptation references.
6. Validate quest/event keys.
7. Validate campaign reachability of phases.
8. Validate no duplicate source-of-truth seasonal definitions.
9. Add data-integrity-selftest checks.
10. Fail fast on invalid climate content.

### Climate integration invariants

- CampaignCalendar owns time; climate evaluates from it.
- WeatherSystem owns realized daily weather.
- Thermal, radiation, expedition, agriculture, power, needs, and morale remain canonical.
- Phase/season thresholds are reachable in supported campaign timelines.
- Climate progression is deterministic and bounded.
- Save state does not duplicate derivable downstream facts.
- Adaptation changes consequences, not climate truth.
- Milestone events are idempotent across save/reload.

### Negative tests

- NuclearWinterSystem generates a second independent weather result.
- Climate state stores actual radiation dose or shelter room temperature.
- A phase is unreachable in every normal campaign.
- Reload re-fires a phase transition event.
- Adding a climate query consumes WeatherSystem RNG and changes unrelated outcomes.
- A weather station changes climate severity instead of information quality.
- Late-game modifiers grow without bound.
- A phase transition directly damages shelter or changes inventory.

### Acceptance evidence

- [ ] Deterministic boundary tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/migration/dedupe tests pass.
- [ ] Phase reachability is documented.
- [ ] Long-run distributions are measured.
- [ ] Adaptation window/balance evidence is captured.
- [ ] Docs/plan metadata are current.

---

## E1-11AH — Performance and long-run climate soak

**Goal:** Prove the climate layer remains negligible relative to weather/thermal simulation.

### Required substeps

1. Run 180-day and max-supported long-run headless simulations.
2. Measure climate-evaluation cost, WeatherSystem integration cost, save size, transition-event count, allocations, and forecast read-model cost.
3. Ensure phase/season lookup is O(1) or O(small catalog).
4. Load catalogs once.
5. Do not recompute expensive historical climate curves every frame.
6. Record median/p95 climate-related tick cost.
7. Add regression thresholds.
8. Test with all downstream adapters enabled and disabled.

### Climate integration invariants

- CampaignCalendar owns time; climate evaluates from it.
- WeatherSystem owns realized daily weather.
- Thermal, radiation, expedition, agriculture, power, needs, and morale remain canonical.
- Phase/season thresholds are reachable in supported campaign timelines.
- Climate progression is deterministic and bounded.
- Save state does not duplicate derivable downstream facts.
- Adaptation changes consequences, not climate truth.
- Milestone events are idempotent across save/reload.

### Negative tests

- NuclearWinterSystem generates a second independent weather result.
- Climate state stores actual radiation dose or shelter room temperature.
- A phase is unreachable in every normal campaign.
- Reload re-fires a phase transition event.
- Adding a climate query consumes WeatherSystem RNG and changes unrelated outcomes.
- A weather station changes climate severity instead of information quality.
- Late-game modifiers grow without bound.
- A phase transition directly damages shelter or changes inventory.

### Acceptance evidence

- [ ] Deterministic boundary tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/migration/dedupe tests pass.
- [ ] Phase reachability is documented.
- [ ] Long-run distributions are measured.
- [ ] Adaptation window/balance evidence is captured.
- [ ] Docs/plan metadata are current.

---

## E1-11AI — Headless nuclear-winter selftest

**Goal:** Create a deterministic CI scenario proving climate progression drives existing systems without duplicating them.

### Required substeps

1. Load campaign calendar and climate catalogs.
2. Evaluate early climate envelope.
3. Advance across one phase boundary and one season boundary.
4. Verify transition events fire once.
5. Generate weather through WeatherSystem using envelope.
6. Verify ShelterThermal sees changed outdoor conditions.
7. Verify expedition hazard changes through canonical weather/hazard adapter.
8. Verify radiation coupling only through radiation authority if enabled.
9. Verify daylight/solar/agriculture adapters if enabled.
10. Save/reload on a boundary and verify no replay/reroll.
11. Validate old-save derivation.
12. Create `--nuclear-winter-selftest`.
13. Assert climate state does not duplicate current weather, radiation dose, room temperatures, crop inventory, or power balance.

### Climate integration invariants

- CampaignCalendar owns time; climate evaluates from it.
- WeatherSystem owns realized daily weather.
- Thermal, radiation, expedition, agriculture, power, needs, and morale remain canonical.
- Phase/season thresholds are reachable in supported campaign timelines.
- Climate progression is deterministic and bounded.
- Save state does not duplicate derivable downstream facts.
- Adaptation changes consequences, not climate truth.
- Milestone events are idempotent across save/reload.

### Negative tests

- NuclearWinterSystem generates a second independent weather result.
- Climate state stores actual radiation dose or shelter room temperature.
- A phase is unreachable in every normal campaign.
- Reload re-fires a phase transition event.
- Adding a climate query consumes WeatherSystem RNG and changes unrelated outcomes.
- A weather station changes climate severity instead of information quality.
- Late-game modifiers grow without bound.
- A phase transition directly damages shelter or changes inventory.

### Acceptance evidence

- [ ] Deterministic boundary tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/migration/dedupe tests pass.
- [ ] Phase reachability is documented.
- [ ] Long-run distributions are measured.
- [ ] Adaptation window/balance evidence is captured.
- [ ] Docs/plan metadata are current.

---

## E1-11AJ — Release gate and closure

**Goal:** Ship a reachable, deterministic climate arc whose pressure is visible and integrated without replacing downstream systems.

### Required substeps

1. Run .NET build/test and game build.
2. Run data-integrity selftest.
3. Run nuclear-winter selftest.
4. Run campaign-length/phase-reachability tests.
5. Run WeatherSystem distribution tests.
6. Run thermal/expedition adapter tests.
7. Run radiation/daylight/agriculture/power adapter tests only where enabled.
8. Run old-save migration and transition dedupe tests.
9. Run determinism/RNG partition tests.
10. Run difficulty/adaptation simulations.
11. Run long-run stabilization soak.
12. Verify climate UI clearly distinguishes climate trend from weather forecast.
13. Update ADR, authority map, docs, plan register, and handoff.
14. Mark DONE only when the climate arc creates strategic preparation pressure during real campaign timelines.

### Climate integration invariants

- CampaignCalendar owns time; climate evaluates from it.
- WeatherSystem owns realized daily weather.
- Thermal, radiation, expedition, agriculture, power, needs, and morale remain canonical.
- Phase/season thresholds are reachable in supported campaign timelines.
- Climate progression is deterministic and bounded.
- Save state does not duplicate derivable downstream facts.
- Adaptation changes consequences, not climate truth.
- Milestone events are idempotent across save/reload.

### Negative tests

- NuclearWinterSystem generates a second independent weather result.
- Climate state stores actual radiation dose or shelter room temperature.
- A phase is unreachable in every normal campaign.
- Reload re-fires a phase transition event.
- Adding a climate query consumes WeatherSystem RNG and changes unrelated outcomes.
- A weather station changes climate severity instead of information quality.
- Late-game modifiers grow without bound.
- A phase transition directly damages shelter or changes inventory.

### Acceptance evidence

- [ ] Deterministic boundary tests pass.
- [ ] Authority-boundary tests pass.
- [ ] Save/migration/dedupe tests pass.
- [ ] Phase reachability is documented.
- [ ] Long-run distributions are measured.
- [ ] Adaptation window/balance evidence is captured.
- [ ] Docs/plan metadata are current.

---

# 5. Canonical Climate Authority Matrix

| Fact | Canonical owner | Nuclear-winter layer role |
|---|---|---|
| Campaign day/time | CampaignCalendar | Read |
| Climate phase/trend | Climate envelope | Own |
| Season | Existing season authority or climate envelope | One owner only |
| Realized weather | WeatherSystem | Modifier input only |
| Outdoor temperature | WeatherSystem/environment | Baseline input only |
| Storm occurrence | WeatherSystem | Distribution modifier |
| Storm damage | Disaster/Shelter systems | No direct write |
| Radiation/fallout | Radiation authority | Optional envelope input |
| Exposure/dose | Radiation/health | No |
| Shelter room temperature | ShelterThermal | No |
| Heating fuel/power | Power/Inventory/Thermal | No |
| Expedition safety | Expedition/Hazard | No |
| Clothing warmth | Clothing/Needs | No |
| Agriculture yield | Agriculture | No |
| Solar generation | Power | Daylight/weather input only |
| Survivor morale | Mental health/Morale | No |
| Forecast | WeatherStation/Info | Climate trend input |
| Adaptation unlock | Research/Knowledge | No |
| Climate milestone history | Journal/event | Stable refs only |

---

# 6. Recommended Climate Envelope

Example shape:

```yaml
day: 92
phase_id: "escalating"
season_id: "winter"

temperature:
  baseline_offset_c: -6.0
  variability_multiplier: 1.1

weather:
  storm_frequency_multiplier: 1.25
  storm_intensity_multiplier: 1.15
  snow_weight_multiplier: 1.3

fallout:
  deposition_pressure_multiplier: 1.05

daylight:
  curve_id: "latvian_winter_abstracted"

agriculture:
  growing_window_tag: "closed_field"
```

The exact values and even the field names depend on current repository authorities.

---

# 7. Phase-Timing Decision

Do not commit this source-plan table blindly:

```text
Initial      1-90
Escalating   91-180
Peak         181-270
Late         271-360
Stabilizing  361+
```

Instead:

1. measure real campaign length;
2. locate intended environmental climax;
3. place adaptation warning windows before it;
4. ensure standard campaigns experience the intended arc;
5. define extended-game behavior separately.

If the campaign is 120–180 days, for example, phase compression or different thresholds would be required.
That decision must come from repository/campaign evidence.

---

# 8. Phase Composition Model

A climate phase should specify the long-term trend.

A season should specify cyclical variation.

```text
final climate envelope =
scenario baseline
+ phase contribution
+ season contribution
+ authored anomaly contribution (optional)
```

Daily weather then samples/derives within that envelope.

Do not stack arbitrary multiplicative modifiers without clamps and tests.

---

# 9. Season Transition Rules

Choose one canonical season cycle.

Possible rules:

- fixed authored day ranges;
- repeating fixed-length seasons;
- calendar-month mapping;
- scenario-specific sequence;
- campaign-relative seasonal windows.

The source plan's “seasons cycle within each phase” may produce unrealistic resets if phase transitions restart
winter→spring→summer→autumn. Verify whether seasons should continue continuously across phases instead.

---

# 10. Temperature Model

Preferred separation:

```text
climate baseline
    +
seasonal offset
    +
daily weather deviation
    =
realized ambient temperature
```

Shelter thermal consumes the realized ambient temperature.

Do not let both climate and WeatherSystem separately calculate a global temperature variable.

---

# 11. Storm Model

Climate modifies distributions:

```text
P(storm today | season, phase, scenario)
IntensityDistribution(storm | season, phase)
```

WeatherSystem produces the actual storm.

Disaster/expedition

<!-- Deliverable capped to remain within the requested 50–90k character envelope. -->
