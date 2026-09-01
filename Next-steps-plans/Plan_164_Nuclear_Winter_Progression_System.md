# Plan 164 — Nuclear Winter Progression System

## Goal

Create a nuclear winter progression system where the post-nuclear climate worsens over time, creating escalating environmental pressure, seasonal severity variations, and long-term climate change that shapes survival strategy. Currently `WeatherSystem` provides weather variety but nuclear winter is static — it doesn't worsen, doesn't have seasonal patterns, and doesn't create long-term climate pressure. This plan adds temporal depth to the environmental challenge and makes the world feel like it's slowly dying.

## Why

**Repository evidence:** `WeatherSystem.cs` (referenced in `WeatherStationSystem.cs:144`) provides weather variety with seeded RNG. `WeatherStationSystem.cs` (143 lines) offers forecasts. `weather_seasons.json` (1049 bytes, 1 entry) defines minimal seasonal data. Plan 135 (weather cascade) connects weather to gameplay but nuclear winter itself is static — it doesn't worsen over time, doesn't have seasonal severity, and doesn't create long-term climate pressure.

**What is missing:** Nuclear winter doesn't progress. Day 1 and Day 365 have the same climate. There are no seasonal variations (worse winters, brief summers). There is no long-term climate change (gradual cooling, increasing storms). The environment is challenging but static — no sense of the world slowly dying or seasons cycling with increasing severity.

**Why existing plans don't solve it:** Plan 19 (dynamic world systems) mentions seasonal cadence but not nuclear winter progression. Plan 83 (weather seasons) adds seasonal data but not progression. Plan 135 (weather cascade) connects weather to gameplay but doesn't add temporal depth. No plan addresses nuclear winter worsening over time or seasonal severity cycles.

**Player value:** Creates urgency (the world is getting worse), adds strategic depth (prepare for worsening winters), provides seasonal variety (brief respites, severe peaks), and makes the environment feel like a dynamic antagonist rather than static backdrop.

## Files / Systems to Inspect

- `Assets/Ashfall.Core/World/WeatherSystem.cs` — weather system
- `Assets/Ashfall.Core/WeatherStationSystem.cs` — weather forecasting
- `Assets/Ashfall.Core/Campaign/CampaignCalendar.cs` — day tracking
- `Assets/StreamingAssets/Data/weather_seasons.json` — seasonal data
- NEW: `Assets/Ashfall.Core/World/NuclearWinterSystem.cs`
- NEW: `Assets/StreamingAssets/Data/nuclear_winter_phases.json`

## Main Task 1 — Foundation / System Contract

1. Create `NuclearWinterSystem.cs` in `Assets/Ashfall.Core/World/`
2. Define `WinterPhase` DTO: `phaseId`, `phaseName` (initial/escalating/peak/late/stabilizing), `startDay`, `endDay`, `severityModifier` (0.5-2.0), `stormFrequency` (modifier), `temperatureBase` (Celsius), `radiationModifier` (0.5-1.5)
3. Define `SeasonalCycle` DTO: `cycleId`, `seasonName` (winter/spring/summer/autumn), `durationDays`, `severityMultiplier` (per season), `weatherPatterns` (list of weighted weather types), `daylightHours` (6-18)
4. Define `ClimateState` DTO: `currentPhase`, `currentSeason`, `currentDay`, `globalTemperature` (Celsius), `stormIntensity` (0-100), `radiationLevel` (modifier), `climateTrend` (worsening/stabilizing/improving)
5. Implement `CaptureState/RestoreState` with schema versioning
6. Define nuclear winter phases:
   - **Initial** (Days 1-90): moderate winter, occasional storms, radiation settling
   - **Escalating** (Days 91-180): worsening cold, more storms, radiation peaks
   - **Peak** (Days 181-270): extreme cold, frequent storms, high radiation
   - **Late** (Days 271-360): slowly improving, still harsh, radiation decreasing
   - **Stabilizing** (Days 361+): new normal, seasonal cycles established
7. Define seasonal cycles within phases:
   - **Winter**: extreme cold, frequent storms, short days (6-8 hours)
   - **Spring**: moderate cold, occasional storms, medium days (10-12 hours)
   - **Summer**: cool temperatures, rare storms, long days (14-16 hours)
   - **Autumn**: cooling, increasing storms, medium days (10-12 hours)
   - Seasons cycle within each phase, severity modified by phase
8. Define climate mechanics:
   - Global temperature decreases over time (phase-dependent)
   - Storm frequency and intensity increase then decrease
   - Radiation levels peak then gradually decline
   - Daylight hours vary by season
   - Climate trend affects all modifiers
9. Define climate effects on gameplay:
   - Temperature affects shelter heating requirements
   - Storms affect expedition safety and shelter damage
   - Radiation affects outdoor exposure and contamination
   - Daylight affects survivor mood and work hours
   - Seasonal changes affect agriculture and food production
10. Define climate adaptation mechanics:
    - Shelter upgrades improve climate resistance
    - Clothing provides temperature protection (Plan 142 integration)
    - Research unlocks climate adaptation tech (Plan 141 integration)
    - Greenhouse extends growing season
    - Climate monitoring provides early warning
11. Add deterministic seeding: climate progression uses `ISeededRng`
12. Wire into `GameBootstrap`: `SetupNuclearWinter`, `TickClimate`, `SaveNuclearWinter`
13. Create `NuclearWinterPhaseCatalogLoader` for phase definitions
14. Implement climate UI: climate panel showing phase, season, temperature, trends
15. Create climate journal: automatic log of climate changes and events

## Main Task 2 — Implementation / Phases / Seasons / Effects / Adaptation

1. Implement phase progression:
   - Climate advances through phases based on campaign day
   - Each phase has distinct severity and characteristics
   - Phase transitions marked by climate events
   - Phase affects all climate modifiers
2. Implement seasonal cycles:
   - Seasons cycle within each phase (winter → spring → summer → autumn)
   - Season duration varies by phase (shorter seasons in peak winter)
   - Season affects temperature, storms, daylight
   - Seasonal transitions marked by weather events
3. implement temperature system:
   - Global temperature based on phase + season
   - Temperature affects shelter heating requirements
   - Extreme cold increases fuel consumption
   - Temperature monitored by weather station
4. Implement storm system:
   - Storm frequency based on phase + season
   - Storm intensity varies (light/moderate/severe/extreme)
   - Storms affect expedition safety (Plan 135 integration)
   - Storms can damage shelter (Plan 158 integration)
5. Implement radiation system:
   - Radiation levels based on phase (peak then decline)
   - Radiation affects outdoor exposure
   - Radiation monitored by dosimeters
   - Radiation affects contamination spread
6. Implement daylight system:
   - Daylight hours vary by season
   - Short days affect survivor mood
   - Long days improve work efficiency
   - Daylight affects solar power generation
7. Implement climate effects:
   - Temperature → shelter heating, clothing needs
   - Storms → expedition risk, shelter damage
   - Radiation → outdoor exposure, contamination
   - Daylight → mood, work hours, solar power
   - Season → agriculture, food production, morale
8. Implement climate adaptation:
   - Shelter upgrades: insulation, heating, storm shelters
   - Clothing: cold weather gear (Plan 142)
   - Research: climate adaptation tech (Plan 141)
   - Greenhouse: extended growing season
   - Monitoring: early warning systems
9. Create climate events:
   - "The First Frost" — winter begins
   - "The Deep Freeze" — extreme cold snap
   - "The Storm" — severe weather event
   - "The Thaw" — spring arrives
   - "The Heat" — brief summer warmth
   - "The Decline" — radiation decreasing
   - "The New Normal" — climate stabilizes
10. Add climate quest hooks:
    - "The Winter Preparation" — prepare for escalating winter
    - "The Storm Shelter" — build storm-resistant shelter
    - "The Cold Snap" — survive extreme cold event
    - "The Thaw" — celebrate spring arrival
    - "The Adaptation" — research climate adaptation
    - "The Monitoring" — build climate monitoring station
    - "The Legacy" — ensure shelter survives nuclear winter
11. Implement climate integration:
    - Climate affects weather system (Plan 135)
    - Climate affects shelter thermal (heating requirements)
    - Climate affects expeditions (safety, timing)
    - Climate affects agriculture (growing season)
    - Climate affects morale (seasonal affect)
12. Add UI: climate panel showing phase, season, temperature, trends
13. Create climate journal: automatic log of climate changes
14. Implement climate tutorial: first seasonal change explains system
15. Create 5 nuclear winter phases and 4 seasonal cycles in data files

## Main Task 3 — Integration / Consequences / Validation

1. Wire into `WeatherSystem`: climate affects weather generation
2. Connect to `ShelterThermalSystem`: temperature affects heating
3. Integrate with `ExpeditionSystem`: climate affects expedition safety
4. Connect to `ClothingWarmthSystem` (Plan 142): clothing protects from cold
5. Wire into `ResearchSystem` (Plan 141): research unlocks adaptation
6. Connect to `DisasterResponseSystem` (Plan 158): storms trigger disasters
7. Implement old-save compatibility: existing saves get default climate state
8. Add deterministic seeding: climate uses `ISeededRng`
9. Create exploit prevention: climate progression is time-based, can't be rushed
10. Add tests: phase progression, seasonal cycles, climate effects, save round-trip
11. Verify catalog integrity: all phase/season IDs resolve
12. Test edge cases: early phase (mild climate), late phase (stabilized)
13. Verify headless behavior: climate processes correctly without UI
14. Add data-integrity-selftest: climate phases validate against catalogs
15. Create `--nuclear-winter-selftest` verb for CI validation

## State / System Interaction Model

```text
Nuclear winter progression
├─ Phase progression (based on campaign day)
│  ├─ Initial (Days 1-90): moderate winter
│  ├─ Escalating (Days 91-180): worsening cold
│  ├─ Peak (Days 181-270): extreme conditions
│  ├─ Late (Days 271-360): slowly improving
│  └─ Stabilizing (Days 361+): new normal
├─ Seasonal cycles (within each phase)
│  ├─ Winter: extreme cold, frequent storms, short days
│  ├─ Spring: moderate cold, occasional storms, medium days
│  ├─ Summer: cool, rare storms, long days
│  └─ Autumn: cooling, increasing storms, medium days
├─ Climate effects
│  ├─ Temperature → shelter heating, clothing needs
│  ├─ Storms → expedition risk, shelter damage
│  ├─ Radiation → outdoor exposure, contamination
│  ├─ Daylight → mood, work hours, solar power
│  └─ Season → agriculture, food production, morale
├─ Climate adaptation
│  ├─ Shelter upgrades: insulation, heating, storm shelters
│  ├─ Clothing: cold weather gear
│  ├─ Research: climate adaptation tech
│  ├─ Greenhouse: extended growing season
│  └─ Monitoring: early warning systems
└─ Integration
   ├─ Weather system (storm generation)
   ├─ Shelter thermal (heating requirements)
   ├─ Expeditions (safety, timing)
   ├─ Clothing (cold protection)
   ├─ Research (adaptation tech)
   └─ Disasters (storm events)
```

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --nuclear-winter-selftest
```

## Risk

**MEDIUM** — Nuclear winter complexity can overwhelm players if too many phases and seasonal variations exist. Risk of climate feeling oppressive rather than challenging. Mitigation: start with mild climate, escalate gradually, provide adaptation options, show clear progression, and include seasonal respites (brief summers).

## Definition of Done

- `NuclearWinterSystem.cs` exists with full `CaptureState/RestoreState`
- 5 nuclear winter phases implemented (initial through stabilizing)
- 4 seasonal cycles functional (winter, spring, summer, autumn)
- Temperature, storm, radiation, daylight systems working
- Climate effects on shelter, expeditions, clothing, agriculture
- Climate adaptation mechanics (upgrades, research, monitoring)
- Climate events and quest hooks
- Save/load round-trip tested
- Deterministic climate progression verified
- Old saves load without error
- 5 phases + 4 seasons in data authority
- UI panel showing climate status
- Cross-system integration (weather, shelter thermal, expeditions, clothing, research, disasters)

## Follow-On Opportunities

- Climate research (unlock advanced adaptation)
- Climate migration (relocate to better climate)
- Climate legacy (shelter survives nuclear winter)
- Climate quests (adapt to specific climate challenges)
- Climate simulation (predict future climate)
