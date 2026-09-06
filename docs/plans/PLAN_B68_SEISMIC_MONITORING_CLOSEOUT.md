# PLAN B68 CLOSEOUT — Geological Faultline Seismic Monitoring & Shock Dampeners

**Date:** 2026-09-06 · **Branch:** `feat/asset-pipeline-flagship`
**Scope:** Core monitoring expansion slice. Host wiring (tick registration,
orbital→`InjectKineticShock` call, seismic UI) remains a follow-up — the
Plan 56 system is Core-only today.

## Architecture decision

**Expansion of the Plan 56 `SeismicDynamicsSystem`** (same partial pattern as
B66): monitoring, dampeners and geophones layer onto the existing
tension→slip→damage-routing authority. Ownership rules enforced:

- **Structural / excavation / thermal authorities keep all damage.** The
  seismic layer emits impulses, warnings and one new *request* payload.
- **Rockburst (68.7):** `OnRockburstRequested(RockburstRequest)` is a
  delegation event — the excavation authority/host consumes it; this system
  never applies tunnel or site damage.
- **Orbital coupling (68.8):** the existing `InjectKineticShock` seam is the
  canonical entry for `SurfaceImpactOccurred`-style events (host wiring follow-up).
- **No new items** — geophones and dampeners use existing authored items:
  `item_geophone_probe`, `item_seismic_damper_pad`, `item_vibration_dampening_mount`.

## Files changed

| File | Change |
|---|---|
| `Assets/Ashfall.Core/Shelter/SeismicDynamicsSystem.Monitoring.cs` | **new partial** — `SeismicWarningStage`, `RockburstRequest`, geophone install/coverage, dampener install/service/wear, P-wave detection, main-arrival ratio helper, arrival estimate, derived warning stages |
| `Assets/Ashfall.Core/Shelter/SeismicDynamicsSystem.cs` | class made `partial`; additive save fields (`geophoneSectors`, `dampenerIntegrity`); 3 minimal hooks: geophone-aware main-arrival threshold in `TickDay`, `CheckPrimaryWave` call, dampener damping + wear + rockburst emission in `TriggerFaultSlip` |
| `Ashfall.Core.Tests/Shelter/SeismicMonitoringB68Tests.cs` | **new** — 14 tests |

## Mechanics summary

- **P/S two-stage window (68.5):** primary tremor warning at tension ratio
  ≥ 0.60 (0.45 with geophones), main-arrival warning at ≥ 0.80 (0.65 with
  geophones). P window opens lead time; `EstimateArrivalDays` gives an
  uncertain day estimate at the current accumulation rate.
- **Geophones (68.9):** `InstallGeophone(sector)` consumes one
  `item_geophone_probe` and lowers both detection thresholds for every fault
  touching that sector.
- **Dampeners (68.6):** `InstallDampener(sector)` consumes one damper pad +
  vibration mount; contributes up to **25 % peak-impulse reduction**
  proportional to integrity; wears `8 + 15·((magnitude−3)/3.5)` per slip;
  `ServiceDampener` restores to 100 (one pad). No real hydraulics.
- **Warning stages (68.10):** `Stable / Elevated / Swarm / Imminent /
  Aftershock` — **derived** from tension ratio and slip recency, never
  persisted (catalog-truth vs runtime-truth rule).
- **Rockburst (68.7):** slips with effective severity ≥ 0.6 emit
  `RockburstRequest { day, faultId, sectors, severity }`.
- **Stabilization:** emergency shoring (Plan 56) already covers the
  `StabilizeFaultZone` game action with authored materials; no grout
  procedures were added.

## Save fields (in `SeismicDynamicsSaveState`, additive)

`geophoneSectors` (legacy: empty — no coverage), `dampenerIntegrity`
(legacy: empty — no dampeners). With nothing installed every threshold and
damping matches Plan 56 exactly (pinned by `LegacyState_…` and the original
6 Plan 56 tests, all passing).

## Verification (2026-09-06)

| Gate | Result |
|---|---|
| `dotnet build Ashfall.csproj` | PASS — 0 warnings, 0 errors |
| `dotnet test` (full suite) | **8823 / 8823 PASS** — includes B68 (14/14) and Plan 56 (6/6) |
| `--data-integrity-selftest` | PASS — 283 catalogs, 0 errors (no new catalog needed) |
| `--bridge-selftest` | PASS |
| `--scene-binding-selftest` | PASS — 25/25 (no scenes changed) |
| Paired determinism | `PairedRuns_SameSeed_ProduceIdenticalDampenedOutcome` |

## Known follow-ups

1. **Host wiring** — construct/tick the system in `Main`/GameBootstrap,
   route orbital impacts to `InjectKineticShock`, persist the section.
2. **UI** — `SeismicMonitorPanel` (risk, pulses, arrival window, dampener
   health, geophone status, event history); `BoreholeSeismographPanel` is a
   UI-06 fake-success prototype and must not be promoted as-is.
3. **Rockburst consumer** — excavation-side handling of
   `OnRockburstRequested` (blocked tunnels, recovery event).
4. **Survivor consequences** — emit canonical stress/injury events on
   severe slips (delegated; not owned here).
