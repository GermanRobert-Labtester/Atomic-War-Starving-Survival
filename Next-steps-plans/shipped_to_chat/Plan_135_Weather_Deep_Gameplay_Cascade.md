# Plan 135 — Weather → Deep Gameplay Cascade

## Goal

Transform weather from a passive UI number into an active gameplay driver that cascades into shelter integrity, survivor behavior, faction operations, expedition risk, economic prices, quest availability, and location accessibility. Weather events create survival pressure, strategic decisions, and emergent stories.

## Why

**Repository evidence:** `WeatherSystem` (referenced in `WeatherStationSystem.cs:144` — already migrated to `SeededRng`). `WeatherStationSystem.cs` (143 lines) provides forecasts with accuracy/horizon. `WeatherKind.cs` defines weather types. `weather_seasons.json` (1049 bytes, 1 entry: `season_second_winter`). But weather doesn't cascade into deep gameplay — it's a number, not a driver.

**What is missing:** Weather doesn't affect faction behavior (military operations halt in storms), survivor psychology (seasonal affective disorder), shelter integrity (storms damage weak structures), expedition risk (routes become impassable), economy (prices spike during harsh weather), quest availability (weather-gated quests), or location accessibility (areas become unreachable). Weather is cosmetic, not consequential.

**Why existing plans don't solve it:** Plan 48 (weather route gates) adds route accessibility gates. Plan 83 (weather seasons expansion) adds seasonal data. Plan 19 (dynamic world systems) mentions weather but doesn't specify cascade mechanics. No plan addresses deep weather→gameplay integration across multiple systems.

**Player value:** Creates survival pressure (prepare for winter), strategic decisions (expedition timing, shelter reinforcement), emergent stories (storm damages shelter during faction negotiation), and a living world where weather matters.

## Files / Systems to Inspect

- `Assets/Ashfall.Core/WeatherStationSystem.cs` — forecast system
- `Assets/Ashfall.Core/WeatherKind.cs` — weather type definitions
- `Assets/StreamingAssets/Data/weather_seasons.json` — seasonal data (1 entry)
- `Assets/Ashfall.Core/ShelterThermalSystem.cs` — shelter temperature
- `Assets/Ashfall.Core/VentilationSystem.cs` — air quality
- `Assets/Ashfall.Core/SumpFloodingSystem.cs` — water infiltration
- `Assets/Ashfall.Core/ExpeditionVehicleSystem.cs` — expedition logistics
- `Assets/Ashfall.Core/TravelingCaravanSystem.cs` — caravan movement
- `Assets/Ashfall.Core/MentalHealthCrisisSystem.cs` — mental health
- `Assets/Ashfall.Core/Survivors/` — survivor behavior
- NEW: `Assets/Ashfall.Core/Weather/WeatherCascadeSystem.cs`
- NEW: `Assets/StreamingAssets/Data/weather_effects.json`

## Main Task 1 — Foundation / System Contract

1. Create `WeatherCascadeSystem.cs` in `Assets/Ashfall.Core/Weather/`
2. Define `WeatherEvent` DTO: `id`, `weatherKind`, `severity` (0-100), `startDay`, `durationDays`, `affectedRegions` (list of location IDs), `effects` (list of `WeatherEffect`)
3. Define `WeatherEffect` DTO: `targetSystem` (shelter/expedition/faction/economy/mental_health/location), `effectType` (damage/delay/price_change/behavior_change/accessibility), `magnitude`, `duration`
4. Define `WeatherCascadeState` DTO: list of active weather events, list of triggered effects, historical weather log
5. Implement `CaptureState/RestoreState` with schema versioning
6. Define weather→shelter cascade:
   - Severe storms damage weak shelter sections (fortification level < 2)
   - Extreme cold increases heating requirements (fuel consumption +)
   - Flooding events trigger sump system overload
   - Radiation storms increase indoor radiation (air filtration stress)
7. Define weather→expedition cascade:
   - Severe weather makes routes impassable (expedition blocked)
   - Moderate weather increases stamina drain and encounter risk
   - Weather-specific hazards: blizzard (visibility 0), radiation storm (dose accumulation), flood (route washed out)
   - Vehicle breakdown chance increases in harsh weather
8. Define weather→faction cascade:
   - Military factions halt operations during severe weather
   - Rebel factions use weather as cover (increased activity during storms)
   - Independent factions offer weather relief (trade opportunity)
   - Faction patrols reduced during harsh weather (safer travel)
9. Define weather→economy cascade:
   - Harsh weather spikes prices for heating fuel, warm clothing, medicine
   - Crop failure during extended winter increases food prices
   - Trade routes disrupted by weather reduce goods availability
   - Weather-related scarcity creates black market opportunities
10. Define weather→mental health cascade:
    - Extended darkness/cold triggers seasonal affective disorder
    - Storm stress increases anxiety/insomnia
    - Weather-related shelter damage causes morale penalty
    - Beautiful weather (rare) provides morale boost
11. Define weather→location cascade:
    - Flooded locations become temporarily inaccessible
    - Radiation storms contaminate outdoor areas
    - Storms reveal hidden caches (erosion)
    - Weather damages location infrastructure (loot quality reduced)
12. Create `IWeatherCascadeSource` interface for weather system to trigger cascade events
13. Add deterministic seeding: weather effects use `ISeededRng`
14. Wire into `GameBootstrap`: `SetupWeatherCascade`, `TickWeatherEffects`, `SaveWeatherCascade`

## Main Task 2 — Implementation / Content / Cascade Effects

1. Implement weather→shelter damage:
   - Severe storm event triggers shelter integrity check
   - Weak sections (fortification < 2) take damage
   - Player can reinforce before storm (cost: resources, labor)
   - Unreinforced shelter: system damage, survivor injury risk
2. Implement weather→expedition blocking:
   - Severe weather blocks expedition departure
   - Player can attempt anyway (increased risk) or wait
   - Weather forecast accuracy affects planning (WeatherStationSystem integration)
   - Expedition caught in sudden storm: emergency return or push through
3. Implement weather→faction behavior:
   - Military faction pauses expansion during severe weather
   - Rebel faction uses storm cover for raids (increased encounter chance)
   - Independent faction offers shelter/refuge during storms (trade opportunity)
   - Faction patrols reduced: safer travel during harsh weather
4. Implement weather→economy price spikes:
   - Cold snap: heating fuel prices +50%, warm clothing prices +30%
   - Extended winter: food prices +40%, medicine prices +20%
   - Radiation storm: anti-rad prices +100%, dosimeter batteries +50%
   - Player can stockpile before predicted weather (forecast integration)
5. Implement weather→mental health:
   - Extended cold/darkness: seasonal affective disorder trigger
   - MentalHealthCrisisSystem integration: weather stress increases crisis probability
   - Player can provide light therapy, warm shelter, community events (mitigation)
   - Untreated: morale penalty, work efficiency reduced
6. Implement weather→location accessibility:
   - Flooded locations: inaccessible for duration
   - Radiation storms: outdoor areas contaminated (dose accumulation)
   - Storm damage: location loot quality reduced (infrastructure damage)
   - Weather reveals hidden caches (erosion exposes buried supplies)
7. Create weather-gated quests:
   - "Survive the storm" quest: shelter preparation, resource management
   - "Storm chaser" quest: explore during severe weather (unique discoveries)
   - "Weather prophet" quest: improve forecast accuracy (WeatherStation upgrade)
   - "Relief convoy" quest: deliver supplies during harsh weather (faction standing)
8. Implement weather prediction mini-game:
   - Player can interpret weather signs (traditional knowledge)
   - Success: early warning, preparation time
   - Failure: caught unprepared
9. Create weather shelter events:
   - Survivors bond during storm (relationship bonuses)
   - Cabin fever: extended confinement increases ideological friction
   - Storm damage reveals hidden shelter section (discovery)
10. Add UI: "Weather Alert" panel showing active weather events, predicted effects, preparation options
11. Create weather journal: automatic log of significant weather events and their consequences
12. Implement weather recovery: after event, locations/shelters gradually recover
13. Create weather legacy: extreme weather events remembered in epilogue evaluation
14. Add 15 weather effect templates in `weather_effects.json`
15. Implement weather interaction with existing systems:
    - `ShelterThermalSystem`: weather affects temperature
    - `VentilationSystem`: radiation storms stress filtration
    - `SumpFloodingSystem`: heavy rain triggers flooding
    - `MentalHealthCrisisSystem`: weather stress increases crisis risk

## Main Task 3 — Integration / Consequences / Validation

1. Wire into `ShelterThermalSystem`: weather affects shelter temperature, heating requirements
2. Connect to `VentilationSystem`: radiation storms increase filtration stress
3. Integrate with `SumpFloodingSystem`: heavy precipitation triggers flooding
4. Connect to `MentalHealthCrisisSystem`: weather stress increases crisis probability
5. Wire into expedition system: weather blocks/increases risk for expeditions
6. Connect to faction systems: weather affects faction behavior and patrol frequency
7. Integrate with economy: weather-driven price spikes and scarcity
8. Connect to quest system: weather-gated quests unlock during specific conditions
9. Implement old-save compatibility: existing saves get empty weather cascade state
10. Add deterministic seeding: weather effects use `ISeededRng`
11. Create exploit prevention: weather events are time-gated, can't be farmed
12. Add tests: weather cascade lifecycle (event → effect → recovery), save round-trip, determinism
13. Verify catalog integrity: weather effect targets resolve to real systems/locations
14. Test edge cases: perpetual good weather (no cascade), perpetual severe weather (constant damage)
15. Verify headless behavior: weather cascade ticks correctly without UI
16. Add data-integrity-selftest: weather effect templates validate against system/location catalogs
17. Create `--weather-cascade-selftest` verb for CI validation

## State / System Interaction Model

```text
Weather event generated (severity, duration, affected regions)
├─ Shelter cascade
│  ├─ Structural damage (weak sections)
│  │  ├─ Player reinforced: no damage
│  │  └─ Player unprepared: system damage, survivor injury
│  ├─ Temperature drop: heating requirements +
│  │  ├─ Fuel sufficient: shelter warm, fuel consumed
│  │  └─ Fuel insufficient: shelter cold, morale penalty
│  ├─ Flooding: sump system overload
│  │  ├─ Sump operational: water pumped
│  │  └─ Sump overwhelmed: water damage, contamination risk
│  └─ Radiation storm: filtration stress
│     ├─ Filters functional: radiation blocked
│     └─ Filters degraded: indoor radiation increase
├─ Expedition cascade
│  ├─ Route blocked: expedition delayed or cancelled
│  ├─ Route hazardous: stamina drain +, encounter risk +
│  └─ Vehicle breakdown chance +
├─ Faction cascade
│  ├─ Military: operations halted
│  ├─ Rebel: increased activity (storm cover)
│  ├─ Independent: relief trade offered
│  └─ Patrols: reduced frequency
├─ Economy cascade
│  ├─ Price spikes: fuel, clothing, medicine
│  ├─ Scarcity: goods unavailable
│  └─ Black market: opportunities emerge
├─ Mental health cascade
│  ├─ Seasonal affective disorder
│  ├─ Storm stress: anxiety, insomnia
│  └─ Cabin fever: ideological friction +
└─ Location cascade
   ├─ Flooded: inaccessible
   ├─ Contaminated: radiation dose risk
   ├─ Damaged: loot quality reduced
   └─ Erosion: hidden caches revealed
```

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --weather-cascade-selftest
```

## Risk

**MEDIUM** — Weather cascade complexity can overwhelm players if too many effects trigger simultaneously. Risk of weather feeling punishing rather than challenging. Mitigation: provide forecast warnings, allow preparation, balance severity/frequency, offer mitigation options.

## Definition of Done

- `WeatherCascadeSystem.cs` exists with full `CaptureState/RestoreState`
- Weather→shelter cascade functional (damage, temperature, flooding, radiation)
- Weather→expedition cascade functional (blocking, risk increase)
- Weather→faction cascade functional (behavior changes)
- Weather→economy cascade functional (price spikes, scarcity)
- Weather→mental health cascade functional (stress, disorders)
- Weather→location cascade functional (accessibility, damage)
- Weather-gated quests implemented
- Save/load round-trip tested
- Deterministic weather effects verified
- Old saves load without error
- 15 weather effect templates in data authority
- UI panel shows weather alerts and preparation options
- Cross-system integration (shelter, expeditions, factions, economy, mental health, locations)

## Follow-On Opportunities

- Weather prediction mini-game (traditional knowledge)
- Weather modification technology (late-game)
- Seasonal migration patterns (faction/npc movement)
- Weather-based resource generation (rainwater collection, wind power)
- Extreme weather events (once-per-campaign superstorms)
