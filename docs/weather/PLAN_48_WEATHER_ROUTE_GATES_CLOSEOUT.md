# Plan 48 — Weather Route Gates Closeout

## Completion Status

**COMPLETE — data authority only.**

The weather gate catalog exists as a reference-clean data authority. No runtime consumer currently enforces weather gates on expedition dispatch, caravan routing, or map travel.

## Runtime Support Level

| Feature | Support |
|---|---|
| Weather query | `WeatherSystem.Current` returns `WeatherKind` |
| Forecast | `PeekForecast(daysAhead)` returns deterministic forecast |
| Route safety | `WeatherStationSystem.IsRouteSafe(day)` exists |
| Dispatch blocking | **NONE** |
| Caravan weather gating | **NONE** |
| Force passage | **NOT SUPPORTED** |
| Override evaluation | **NONE** |
| Gate persistence | **NOT NEEDED** (derived from weather) |

## Catalog Summary

| Field | Value |
|---|---|
| File | `Assets/StreamingAssets/Data/weather_route_gates.json` |
| Schema version | 1 |
| Total gates | 15 |
| Blizzard-related | 4 (blocked) + 2 (required) = 6 |
| Contaminated fog | 3 |
| Black rain | 3 |
| Fallout storm | 2 |
| Severe cold required | 2 |
| EMP | 1 |
| Gates with overrides | 4 (gas_mask ×2, hazmat_suit ×2) |
| Positive gates | 2 (severe-cold-required) |
| Negative gates | 13 (blocked-during) |

## Weather States Used

All 7 rollable `WeatherKind` values are referenced:
- `Blizzard` — 4 blocked + 2 required
- `BioFog` — 3 blocked
- `BlackRain` — 3 blocked
- `FalloutStorm` — 2 blocked
- `EMPStorm` — 1 blocked

No dead/placeholder weather states are referenced.

## Target Routes

All 15 targets resolve to active caravan route IDs from `wasteland_trade_caravan_routes.json`. No Plan 32 destination IDs are referenced.

## Save Model

Gate passability is **derived** from current weather + target. No mutable gate state exists. No gate-specific save section needed.

## Deferred Features

| Feature | Reason |
|---|---|
| Expedition dispatch blocking | No dispatch validation system exists |
| Caravan weather gating | Caravans advance unconditionally |
| Force passage consequences | No force-passage action exists |
| Override evaluation at runtime | No gate consumer to evaluate overrides |
| Regional weather gating | Weather is global, not regional |
| Mid-travel weather reevaluation | Not supported by expedition system |

## Integration Path

To activate weather gates at runtime:
1. Add weather check to expedition dispatch validation (if dispatch validation is added)
2. Add weather check to caravan `DailyTick()` (if caravan gating is desired)
3. Evaluate `blocked_weather` / `required_weather` against `WeatherSystem.Current`
4. Evaluate `override_item` against player inventory if gate is blocked
5. Surface `description` as blocked-reason text in dispatch UI
6. No save changes needed — gate state is derived

## Files Created

```
Assets/StreamingAssets/Data/weather_route_gates.json
docs/weather/WEATHER_ROUTE_GATE_RUNTIME_CONTRACT.md
docs/weather/WEATHER_GATE_TARGET_INVENTORY.md
docs/weather/WEATHER_GATE_OVERRIDE_INVENTORY.md
docs/weather/PLAN_48_WEATHER_ROUTE_GATES_CLOSEOUT.md
```

## Verification

| Check | Result |
|---|---|
| JSON parse | valid |
| Gate count | 15/15 |
| Weather distribution | 4+3+3+2+2+1 = 15 |
| Target refs | all 15 resolve to active route IDs |
| Override refs | all 4 resolve to active item IDs |
| Required/blocked overlap | 0 contradictions |
| Build | 0 errors |
