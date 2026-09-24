# UNBLOCK — Expansion 32 (The Wild) & Expansion 33 (The Weather): Host Integration

**Status:** HOST INTEGRATION COMPLETE 2026-09-24 (integrator, user-authorized).
**Claim:** `claim-unblock-expansion-32-33-2026-09-24`.
**Evidence:** `--wildlife-harvest-selftest` 12/12, `--storm-forecast-selftest`
12/12; host + Core builds 0 errors.

## Premise (verified in source before editing)

Both engines are **signed pure-domain authorities** with tests (DEC-86 = 5/5,
DEC-83-lineage DEC-87 = 5/5) but **0 host references**, so their behavior was
unreachable in play. The audit rows are accurate: the gap is host integration.

## Expansion 32 — The Wild (wildlife harvest quota)

Populations/extinction flags stay with `WildlifeMigrationSystem` /
`WildlifeEcosystemSystem`; trap sites stay with `WildlifeTrappingSystem`. The
quota engine is the **sustainable-harvest / predator / taming layer** only.

- **Core:** `WildlifeHarvestLedger` + `WildlifeHarvestState` +
  `WildlifeHarvestCensus`; read-only `EvaluateQuota`, `ApplyHarvest`,
  `BeginSeason`, `EvaluatePredatorConflict`, `EvaluateTamingReadiness`,
  schema-gated `RestoreState`. It records only what was taken — never a population.
- **Host:** `WildlifeHarvestSaveStore` (section `wildlife_harvest`) +
  `WildlifeHarvestHostSession` + `Main.WildlifeHarvest.cs`.
- **Campaign:** phase-5 day owner emits `wildlife_harvest_ticked` with pre-day
  snapshot restore. Population facts are supplied at the call site by their
  canonical owners; the ledger keeps the seasonal harvest counters.

## Expansion 33 — The Weather (storm forecast readiness)

Weather stays with `WeatherSystem` / `WeatherGate`. The forecast engine is the
**observation-post / readiness layer** only.

- **Core:** `StormForecastLedger` + `StormForecastState` +
  `StormForecastCensus`; `TickDay` (observation-post calibration drift +
  drill-recency decay via `CalculateObservationPostDecay`),
  `EvaluateForecast` (from live skill), `IssueWarning`, `AssessReadiness`,
  `RunDrill`, schema-gated `RestoreState`.
- **Host:** `StormForecastSaveStore` (section `storm_forecast`) +
  `StormForecastHostSession` + `Main.StormForecast.cs`.
- **Campaign:** phase-5 day owner decays the post and drill recency and emits
  `storm_forecast_ticked` with pre-day snapshot restore. Readiness inputs
  (shelter seal, filter stock, medical bay) are supplied by their canonical
  owners at the call site.

## Deferred with named reasons

- **Reading readiness inputs live from each owner** (shelter seal integrity,
  filter item stock from the inventory, medical-bay capacity): the host exposes
  `AssessStormReadiness(...)` taking those facts explicitly; binding each to its
  live owner is a follow-up seam decision.
- **Issuing forecasts automatically from the canonical weather event stream**:
  would need a typed storm-onset hook on `WeatherSystem`; the host
  `IssueStormForecast` / `PublishStormWarning` commands are the operational
  surface today.
- **The broader expansion narrative content** (predator packs, rail-head towns,
  black-rain events): design-bible concerns beyond the signed engine surfaces.
- **A dedicated UI panel** for either: concurrently-owned panel-route seams.

## Verification

```
host + Core builds: 0 errors / 0 warnings
--wildlife-harvest-selftest 12/12     --storm-forecast-selftest 12/12
--data-integrity-selftest PASS        --port-contract-selftest PASS (301 seams)
--7-day-smoke-selftest PASS
World + WildlifeHarvestLedgerTests 5/5 + StormForecastLedgerTests 5/5
Save 1496/1496 (section pin 249)
architecture map 249 subsystems (100%) · save-store matrix 251 · selftest manifest 182
agent-fast-verify 10/10
```
