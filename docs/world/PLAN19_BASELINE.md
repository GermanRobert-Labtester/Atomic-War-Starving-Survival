# Plan 19 — Baseline Reconnaissance & Dynamic World Systems

> **Scope:** Dynamic World Systems: Weather Forecasting, Orbital Harrow & Seasonal Progression.
> **Source-of-Truth:** `Assets/Ashfall.Core/World/WeatherSystem.cs`, `Assets/Ashfall.Core/WeatherStationSystem.cs`, `Assets/Ashfall.Core/OrbitalHarrowTelemetrySystem.cs`, `Assets/StreamingAssets/Data/weather_seasons.json`.

---

## 1. Baseline Summary

Prior to Plan 19, the world state systems operated with minimal systemic interconnectedness:
- Realized weather transitions were simulated via `WeatherSystem`, but forecasting was decoupled from station condition.
- Orbital telemetry existed as dormant state (`OrbitalHarrowTelemetrySystem`), lacking template-driven kinetic strikes, shelter cascades, or post-strike salvage aftermaths.
- Seasonal progression used a 3-window schema (`weather_seasons.json`) without signature seasonal crises or deterministic mitigation choices.

---

## 2. Post-Plan 19 Implemented State

| System | Baseline | Plan 19 Delivered Target | Source Files |
|---|---|---|---|
| **Weather Forecasting** | Basic lookahead | Tiered precision (`Offline`, `Damaged`, `Functional`, `Calibrated`), degradation/repair, preparation payoffs, atmospheric flavor | [WeatherStationSystem.cs](../../Assets/Ashfall.Core/WeatherStationSystem.cs) |
| **Orbital Harrow** | Basic scalar warning | 5 template-driven kinetic strikes, multi-cell spread, sky armor cascades, salvage opportunities, site reveals | [OrbitalHarrowTelemetrySystem.cs](../../Assets/Ashfall.Core/OrbitalHarrowTelemetrySystem.cs), [orbital_harrow_events.json](../../Assets/StreamingAssets/Data/orbital_harrow_events.json) |
| **Seasonal Calendar** | 3 coarse windows | 6 full-year calibrated phases (`window_ashfall`, `window_deep_freeze`, `window_thaw`, `window_black_bloom`, `window_high_cold`, `window_the_turning`) | [weather_seasons.json](../../Assets/StreamingAssets/Data/weather_seasons.json) |
| **Seasonal Hazards** | 0 signature events | 18 signature events with deterministic trigger rolls, category tagging, and mitigation costs | [SeasonalEventSystem.cs](../../Assets/Ashfall.Core/World/SeasonalEventSystem.cs), [seasonal_events.json](../../Assets/StreamingAssets/Data/seasonal_events.json) |
| **Coordinator & Read Model** | Station + Orbital DTO | Single coordinator orchestrating Station, Orbital, and Seasonal events with unified read model | [WeatherIntelligenceCoordinator.cs](../../Assets/Ashfall.Core/World/WeatherIntelligenceCoordinator.cs) |

---

## 3. Invariant Verification

- **Invariant 1 (Engine Agnostic):** Zero engine dependencies in `Assets/Ashfall.Core/`.
- **Invariant 4 (Determinism):** Lookahead and event triggers use `ISeededRng` without modifying simulation roll counts.
- **Invariant 6 (Data Authority):** All event templates, seasonal weights, and phases reside in `Assets/StreamingAssets/Data/`.
