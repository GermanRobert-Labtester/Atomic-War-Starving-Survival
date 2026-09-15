# PLAN 123 — Sound-Ranging Authority Map (Phase 1)

**Status:** ACCEPTED (reconnaissance). Premise-verified against current source.

## Verified owners

| Concern | Owner | Evidence |
|---|---|---|
| Hostile-fire events | **No per-strike emitter exists.** `YearOfAsh/FactionWarSystem.cs` increments `totalArtilleryStrikesLogged` (:201, day 241+, every 15d) and raises `OnTerritorialClashOccurred`; `YearOfAshTimelineSystem.cs` counts `artilleryBarragesExperienced` (:24, Phase 5 siege days 241–300). Both are aggregate counters. | Engine defines `HostileFireObservation` input; emitter wiring = foreman decision (master map §4.1). Default: engine owns input type, Phase 7 wires producer. |
| Radio direction finding (adjacent, live) | `Radio/SignalTriangulationSystem.cs` + `Radio/DirectionFindingCatalog.cs` + `acoustic_triangulation_catalog.json` | Radio-signal triangulation — different domain; Plan 123 does NOT duplicate it, and must not attach hostile-fire fixes to it |
| Dormant acoustic catalog | `Radio/AcousticDirectionFindingCatalog.cs` — **zero consumers, no JSON** (136 lines) | Quarantine candidate; NOT the extension target |
| World map threat intel | `World/WastelandMapSystem.cs` | `UpsertTrapMarker` :117 marker precedent; `GetNodeIntel` :380; fog/knowledge states — threat region = new marker type + node intel view, map stays authoritative |
| Weather/environment | `World/WeatherSystem.cs`, `WeatherGate*`, `IWeatherSeverityProvider.cs` | bounded confidence modifiers only |
| Ally settlements | factions/currents + radio systems (existing owner; named at implementation) | warning delivery through existing radio/diplomacy owners |
| Sensor maintenance | `EquipmentConditionSystem` + `IEquipmentConditionSink` | surface nodes are equipment-condition consumers |
| Save | new `sound_ranging` section via `SaveSectionRegistry` | calibration, node status, observation history, signatures |
| RNG | fork keys `sound_ranging.measurement_noise`, `sound_ranging.environmental_error` | deterministic under same seed |

## Answers to workstream 123-A questions

1. Who emits a hostile-fire event? **Nobody yet, per-strike.** Only aggregate counters
   exist (FactionWar every 15 days post-241; YearOfAsh timeline barrages). §4.1 decision.
2. Does the projectile system distinguish long-range artillery? No projectile system
   exists; `TacticalCombatSystem.*` is close-quarters combat. Not an input source.
3. Is there already an acoustic observation DTO? No. The dormant
   `AcousticDirectionFindingCatalog.cs` has array/sensor profiles but no engine and no
   data file; treat as unrelated dormant code.
4. Does Plan 92 own generic acoustic direction finding? The live DF authority is
   **radio** triangulation (`SignalTriangulationSystem`). Plan 123 is hostile-fire
   acoustic observation — new engine, typed seam to the map; no second triangulator.
5. Does the world map support uncertainty regions? Markers + fog + intel views exist;
   an uncertainty zone renders as a marker/intel property — no new map authority.
6. How are ally settlements warned? Through existing radio/diplomacy owners; Plan 123
   emits a typed warning event, delivery stays with those owners.
7. How are surface sensors maintained? `EquipmentConditionSystem` sink; damage/loss via
   existing encounter/equipment owners; no new sabotage system.

## Defensive-only invariants (hard gates in tests)

- Output is bearing sector + probable region + confidence — never a weapon-quality
  coordinate DTO (negative test required, plan §5.15 case 18).
- No automated counter-battery cueing; strategic artillery (if ever) gets its own
  targeting rules and may not consume Plan 123 output.
- Confidence has a hard max; repeated valid observations cannot increase uncertainty
  absent state change; moving/redeployed source resets correlation.
- All noise seeded (`CampaignRngStream`), never wall-clock.

## Non-goals

No waveform simulation; no continuous audio processing (event-driven only); no strike
solutions; no bypass of map/intel authority; UI labels stay
Low Confidence / Probable Sector / High Confidence Threat Zone.

## New files (planned)

- `Assets/Ashfall.Core/Combat/SoundRangingThreatEngine.cs`
- `Assets/StreamingAssets/Data/sound_ranging_catalog.json`
- `src/Host/SoundRangingHostSession.cs`, `src/Host/SoundRangingSaveStore.cs`
- `src/Main.SoundRanging.cs`, `src/UI/SoundRangingPanel.cs`
- `Ashfall.Core.Tests/Combat/SoundRangingThreatEngineTests.cs`
- Save rows: `sound_ranging`
