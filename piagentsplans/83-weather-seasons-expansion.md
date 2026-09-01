# Plan 83 — Weather Season Windows Expansion (3 → 10 season windows)

## Goal (2 lines)
Expand `weather_seasons.json` from 3 verified season windows to 10. The weather
system (`WeatherSystem.cs` confirmed live) defines seasonal windows that weight
weather types (clear, rain, overcast, ashfall, fallout storm, blizzard, black
rain). 3 windows (First Thaw, Deep Freeze, Long Winter) cover only the early
and late game — the mid-campaign has no seasonal weather variation.

## Why (P2)
- Verified: `weather_seasons.json` has 3 season windows (id, displayName,
  startDay, clearWeight, rainWeight, overcastWeight, ashfallWeight,
  falloutStormWeight, blizzardWeight, blackRainWeight).
  `WeatherSystem.cs` is confirmed in Core.
- Creates the weather-progression pillar: weather should shift across the
  campaign — early confusion (variable weather), mid-campaign stabilization
  (ashfall dominant), late-game nuclear winter (blizzard and black rain
  dominant). 3 windows means the mid-game weather is a flat line.
- Pure DATA work — zero new Core code.

## Files to touch
- `Assets/StreamingAssets/Data/weather_seasons.json` (expand 3 → 10 windows)
- Read-only: `Assets/Ashfall.Core/World/WeatherSystem.cs` (confirm how season
  windows are selected by startDay and how weights are applied)

## Content grammar (per season window)
- snake_case `id` with prefix `window_` (confirmed prefix).
- startDay: integer — the day this window begins. Windows are ordered by
  startDay and non-overlapping. The system selects the active window by current
  day.
- displayName: 1–3 words evoking the campaign phase (First Thaw, Deep Freeze,
  Long Winter, Ash Settling, Spring Storms, etc.).
- Weather weights: 7 weight fields (clearWeight, rainWeight, overcastWeight,
  ashfallWeight, falloutStormWeight, blizzardWeight, blackRainWeight) — each
  0.0–3.0. Higher weight = more likely. Each window should have a distinct
  weather signature (no two windows with identical weight profiles).
- Campaign arc: early windows (variable, moderate danger) → mid windows
  (ashfall dominant, rising danger) → late windows (blizzard/black rain
  dominant, high danger).

## Steps
1. Read `WeatherSystem.cs` to confirm how season windows are selected (by
   startDay — does it pick the latest window with startDay <= current day?) and
   how weights are normalized.
2. Confirm the existing 3 windows (First Thaw day 0, Deep Freeze day 60, Long
   Winter day 240) and identify the gap (days 61–239 have no window transitions).
3. Author 7 new windows filling the mid-campaign and extending the late game:
   - `window_ash_settling` (day 30): ashfall rising, storms declining.
   - `window_spring_storms` (day 90): rain and overcast dominant, brief clear.
   - `window_dry_ash` (day 120): ashfall dominant, low rain, rising fallout.
   - `window_first_fallout` (day 150): fallout storms peak, black rain appears.
   - `window_false_spring` (day 180): brief clear window, deceptive calm.
   - `window_deep_ash` (day 200): ashfall heavy, blizzard rising.
   - `window_black_rain_season` (day 280): black rain dominant, worst weather.
4. Each window: distinct weight profile across all 7 weather types. No two
   windows should have the same dominant weather.
5. Cross-reference: all window ids unique; startDays are strictly increasing;
   every day 0–365+ falls within a window (or confirm the system handles gaps by
   using the last active window).
6. Wire 2 windows into Plan 48 weather gates (black rain season and first
   fallout season block specific expedition routes).
7. Validate: `--data-integrity-selftest`; run a headless day-advance test to
   confirm windows activate at the correct day.
8. xUnit: weather season catalog loads 10 windows, all ids unique, startDays
   strictly increasing, all weights within valid ranges, no two windows with
   identical weight profiles.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
LOW — pure data. The one trap is startDay gaps (step 5): confirm the system
uses the last active window for days beyond the latest startDay, and that no
two windows share a startDay.

## Definition of Done
- `weather_seasons.json` has 10 windows, all ids unique, startDays strictly
  increasing, 2 wired to weather gates, integrity + tests green.

## Follow-on
- Plan 48 (weather gates) — seasonal windows block expedition routes.
- Plan 81 (dose locations) — seasonal fallout shifts surface dose levels.
- Plan 77 (duty roster seasons) — weather windows should align with roster
  seasons for consistent campaign pacing.
- Plan 74 (campaign chapters) — weather windows mark chapter transitions.
- Existing 19 (dynamic world systems) — this plan provides the seasonal data.
