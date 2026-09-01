# Coastal World-State Contract (Plan 23)

One authoritative path for every coastal world-state fact. This document is the
contract between weather, surge, tide, dive, and map systems.

## Producer → consumer contract

| Producer (authority) | Produces | Consumer | Persistence owner |
|---|---|---|---|
| `WeatherSystem` (WeatherKind per day) | `WeatherKind` per day | deep-coast `TickDaily` (surge producer), deep-coast contamination decay | `WorldWeatherState` |
| `District8DeepCoastSystem.TickDaily` | surge begin/recede, contamination | berth gate (`CanStartDockOperation`), narrative markers | `District8DeepCoastState` (additive fields) |
| `District8DeepCoastSystem` narrative markers (`dc8_surge_began/aftermath`) | journal keys (host writes entries via `JournalSystem`) | world-evolution aftermath events (flag-gated) |
| `TideCalendar` (pure, day-derived) | phase/window queries | `CanLaunch` gate, atlas presentation | derived — never serialized |
| `MaritimeDiveSystem` (gear gate + site catalog) | launch eligibility | panels, expeditions | dive state via `MaritimeSaveStore` |
| `WorldEvolutionEngine` + events catalog | lasting map mutations (locks, danger, exposure) | `WastelandMapSystem` | engine's triggered-event registry |
| `EnvironmentalTextCatalog` | coastal state flavor (tide phase, surge debris, rips) | ambient text surfaces | catalog data |

Rules: the map layer is a renderer/consumer of authoritative state; neither the
maritime UI nor radio ever computes tide/surge/flood state. Muster currents
(`currents.json`) are people, never hydrology.

## Chronology guards

- Tide phase: pure function of campaign day (no state, no drift, no real time).
- Surge: starts only on surge-grade weather; recede requires `SurgeRecedeLagDays`
  calm days; both events marked once (`narrativeMarkers` dedupe).
- World-evolution events trigger once (engine `TriggeredEventIds`), gated by day +
  the verified surge narrative flags — no locally fabricated flags.
