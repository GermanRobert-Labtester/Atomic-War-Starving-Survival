# UNBLOCK — Expansion 25 (The Iron Road) & Expansion 29 (The Glass): Host Integration

**Status:** HOST INTEGRATION COMPLETE 2026-09-24 (integrator, user-authorized).
**Claim:** `claim-unblock-expansion-25-29-2026-09-24`.
**Evidence:** `--rail-track-maintenance-selftest` 12/12, `--glassworks-selftest`
12/12; host + Core builds 0 errors.

## Premise (verified in source before editing)

Both engines are **signed pure-domain authorities** with tests (DEC-80 = 5/5,
DEC-83 = 5/5) but **0 host references**, so their behavior was unreachable in
play. The audit rows ("Expansion 25 RailTrackMaintenanceEngine", "Expansion 29
PrecisionGlassworksOpticsEngine") are therefore accurate: the gap is host
integration, not the engine.

## Expansion 25 — The Iron Road (rail track maintenance)

`RailwaySystem` remains the sole owner of topology, dispatch, and pathfinding;
`RailwayInterlockEngine` owns switches/signals/reservations. The maintenance
engine is the **gauge/alignment/wear/bridge-load layer** only.

- **Core:** `RailTrackMaintenanceLedger` (stateful owner) + `RailMaintenanceState`
  + `RailMaintenanceCensus`; `SeedFromTopology`, `Evaluate` (read-only),
  `ApplyRun`, `Maintain`, `SetBlocked`, schema-gated `RestoreState`.
- **Host:** `RailTrackMaintenanceSaveStore` (section `rail_track_maintenance`) +
  `RailTrackMaintenanceHostSession` + `Main.RailTrackMaintenance.cs`.
- **Campaign integration:** the ledger seeds pristine segment records from the
  canonical `RailwaySystem.SegmentDefs` (ids and bridge/mass facts), and
  `Main.Plans190_193.cs` records the wear a departing train puts on its segment
  from the existing `OnTrainDispatched` event, classifying the train mass into
  the engine's locomotive table. A phase-5 day owner emits
  `rail_track_maintenance_ticked` and supports pre-day snapshot restore.
- **It never dispatches and never moves a train.**

## Expansion 29 — The Glass (precision vitrification & optics)

`PrecisionOpticsEngine` keeps the optical catalog; `RadiationSystem` keeps
browning; `GreenhouseSystem` keeps glazing; `GeodeticSurveyEngine` keeps survey.
The glassworks engine is the **batch/vision layer** only.

- **Core:** `GlassworksLedger` (stateful owner) + `GlassworksState` +
  `GlassworksCensus`; `AddBatch`, `AdvanceAnnealing`, `GrindCorrectionLens`
  (consumes grit stock), `CalibrateTheodolite`, `SetVisionPrescription`,
  schema-gated `RestoreState`.
- **Host:** `GlassworksSaveStore` (section `glassworks`) +
  `GlassworksHostSession` + `Main.Glassworks.cs`.
- **Campaign integration:** a phase-5 day owner advances one anneal stage per
  uncracked batch every two days at the authored-optimal kiln temperature
  (deterministic — no RNG stream consumed), emits `glassworks_ticked`, and
  supports pre-day snapshot restore.
- **It never owns the optical catalog, radiation browning, greenhouse glazing,
  or survey.**

## Deferred with named reasons

- **The broader expansion narrative content** (rail towns, interdiction by
  bandits/armored trains, route politics; glass furnaces as a buildable, window
  glazing as an infrastructure decision, mirrors as a light strategy): these are
  design-bible concerns, not the signed engine surfaces. The engines are live and
  reachable; the content layers remain future authoring.
- **A dedicated rail-maintenance / glassworks UI panel**: registering a new
  routed panel needs coordinated edits across the concurrently-owned panel-route
  seams (`PanelRegistryBootstrap`, `OpenPlayerPanel`, `Main.UiPanels`,
  `Main.PlayerSurfaces`, `PlayerSurfaceManifest`). The host commands and probes
  are the operational surface now.
- **Wiring maintenance feasibility into `RailwaySystem` dispatch refusal**: would
  require an optional provider hook on the canonical rail system; the host
  exposes `EvaluateRailTrack` as the advisory seam today.

## Verification

```
host + Core builds: 0 errors / 0 warnings
--rail-track-maintenance-selftest 12/12   --glassworks-selftest 12/12
--data-integrity-selftest PASS            --port-contract-selftest PASS (301 seams)
--7-day-smoke-selftest PASS
Rail 10/10 (new RailTrackMaintenanceLedgerTests 5/5)  Optics 10/10 (new GlassworksLedgerTests 5/5)
Save 1472/1472 (section pin 243 -> 245)
architecture map 245 subsystems (100%) · save-store matrix 247 · selftest manifest 178
agent-fast-verify 10/10
```
