# Plan 48 — Weather Route Gate Runtime Contract

## Weather System

- **SystemId**: `"world_weather_system"`
- **Granularity**: Global — single `WorldWeatherState` for entire world
- **Tick**: 6-hour check interval, deterministic via `seed * 397 + rollCount`
- **Save**: `WorldWeatherState` registered in `SaveWireContract.cs`

## Rollable Weather States (7 of 22)

| WeatherKind | Enum | Rollable | Season Peak |
|---|---|---|---|
| Clear | 0 | yes | The Turning (day 300+) |
| Rain | 1 | yes | The Thaw (day 120+) |
| Overcast | 2 | yes | all seasons |
| Ashfall | 3 | yes | Ash Fall (day 0+) |
| FalloutStorm | 4 | yes | High Cold (day 240+) |
| Blizzard | 5 | yes | Deep Freeze / High Cold |
| BlackRain | 6 | yes | Black Bloom (day 180+) |

## Dead/Placeholder States (15 of 22)

AcidSnow, BioFog, BlackSnow, BloodRain, EMPStorm, GlassStorm, RadHail, AlgaeBloom, AshLightning, ParticulateFog, ThermalInversion, IceStorm, Silence, FalseSpring, SilentSpring

**These cannot be rolled by the weather system.** They exist in the enum but have no weight entries in `SeasonWindowDef` and no atmosphere mappings.

## Gate Consumer Status

| Consumer | Status |
|---|---|
| Expedition dispatch | **NONE** — no dispatch validation exists |
| Caravan routing | **NONE** — caravans advance unconditionally |
| Force passage | **NOT SUPPORTED** |
| Map route blocking | **NONE** — `MapRouteDef` has no weather field |

## Completion Mode

**COMPLETE — data authority only.**

The catalog exists as a reference-clean data authority. Runtime enforcement requires a weather-gate consumer integration task.

## Query APIs Available

| API | Returns |
|---|---|
| `WeatherSystem.Current` | `WeatherKind` (current) |
| `WeatherSystem.PeekForecast(daysAhead)` | `List<WeatherForecastEntry>` |
| `WeatherStationSystem.IsRouteSafe(day)` | `bool` |
| `WeatherStationSystem.GetForecast()` | `IReadOnlyList<ForecastEntry>` |

## Save Model

Gate passability is **derived** from current weather + target. No mutable gate state exists. No gate-specific save section needed.
