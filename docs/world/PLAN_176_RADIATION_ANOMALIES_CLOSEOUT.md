# Plan 176 — Radiation Storms & Deep Zone Anomalies: Closeout

**Status:** COMPLETE · **Date:** 2026-09-13 · **Batch:** `PLANS-174-177-FLAGSHIP-SURVIVOR-WORLD`

## Delivered

| Layer | Artifact |
|---|---|
| Core | `Assets/Ashfall.Core/World/AnomalyHazardSystem.cs` (`anomaly_hazard`) — spawn w/ cap, deterministic movement, additive radiation query (capped 600), effect-tag union, bounded wildlife modifier, detection classification, one-shot approach warnings, loot-site single resolution, exact capture/restore; `AnomalyHazardCatalog.cs` strict loader |
| Data | `Assets/StreamingAssets/Data/anomalies.json` — 9 authored anomalies (2 storm fronts, 2 drift bands, 5 deep-zone zones) with canonical `table_loot_*` FKs |
| Host | `src/Host/AnomalyHazardSaveStore.cs`, `src/Main.Anomaly.cs` (wind feed, warnings→journal, tick) |
| Save | `SaveSectionRegistry` row (`anomaly_hazard`, owner `world`) + `anomaly_hazard_save.json`; RNG stream `anomaly_hazard` |
| Consumers | Expedition dose → `ExposureEnvironmentResolver.AnomalyRadRateProvider` (RadiationSystem stays the sole dose writer); detection → `GetAnomalyDetectionCapability()` from canonical devices; loot → `ResolveAnomalyLootSite` through the expedition scavenging authority; wildlife → `WildlifeEcosystemSystem.TickDay(sectorHazardModifiers)` avoidance migrations |
| Tests | Core 19/19 (`Plan176AnomalyHazardTests`) · consumers 8/8 (`Plan176CrossSystemConsumerTests`) · campaign harness |

## Contract guarantees (§32 DoD — all satisfied)

- **One canonical layer, no duplication (Trap G).** `FalloutSystem` remains the nuclear-fallout authority; this layer adds authored anomaly/storm-front zones on the shared wind/dose truth. Positions are km-quantized (0.01) — save/load exact.
- **Movement deterministic.** Storm fronts travel a day-keyed wandered bearing; drift bands ride the Plan-205 wind vector × `wind_response`; world-bounds clamped; stable ordering by hazard id. Continuous == mid-reload track proven.
- **RadiationSystem owns dose.** The anomaly layer only *reports* `GetRadiationRate`; expedition exposure flows through the typed `ExposureEnvironment` handoff — no health mutation anywhere in the layer (structurally guarded).
- **Detection consumes real devices.** Capability from canonical inventory items (geiger 60 / dosimeter 15 rads/hr); no powered device → low-confidence Signature, never hidden truth; rate is still consumable by the dose authority regardless.
- **Loot uses canonical tables.** Resolution flips site state exactly once and names the `table_loot_*` reference; the expedition loot authority grants items; duplicates rejected (`loot_already_resolved` — no duplicate loot after restore).
- **Wildlife reacts through its own authority.** Avoidance modifiers migrate packs via `WildlifeMigrationSystem` with the ecology's own fork; attraction is a documented v1 no-op; no synthetic spawning.
- **Warnings typed, never auto-lockdown.** `OnStormApproaching` (ETA band, cardinal, intensity band, authored confidence) fires once per hazard/target episode; canonical emergency policy untouched.
- **No free late-game rewards (§10).** Old saves restore an empty hazard section; spawning is a host decision (region-driven spawn rolls land with campaign content).

## Deferred (flagged, not silent)

HazardMap UI (presentation wave); automatic region-driven spawn scheduling; expedition-position interpolation during travel legs (current seam samples the target location).
