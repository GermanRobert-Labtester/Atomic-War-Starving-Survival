# LINQ / Closure Allocation Sweep — Enhancement Task 5 (2026-09-27)

> **STATUS: COMPLETE — commit `873870de4`. Two sites changed; three left unchanged on cadence evidence.**

## Premise check

The sweep audit ranked five Core sites as hot. Tracing each site's production
caller changed the ranking:

| Site | Audit claim | Verified cadence | Decision |
|---|---|---|---|
| `CampaignDayCoordinator.Owners` | per day + per query | Only read by `Main.WorldPlaytest` and tests; `Advance` iterates `_owners` directly | **Changed**: restores the regressed Plan 82 cached view |
| `HealthHistorySystem.RecordDailyHealthTrend` | per survivor per metric per day | No production daily caller (host session API + selftest); cost grows with an unbounded trend list | **Changed**: sort replaced by one linear scan |
| `EmergencyAlertSystem.TickHour` | hourly (hottest) | Once per campaign day (`Main.TickEmergencyAlerts`, compressed alert clock); non-capturing lambda | Unchanged |
| `MutationSystem.TryMutateSurvivor` | per survivor per day | LINQ chain runs only after a successful mutation roll | Unchanged |
| `RailwaySystem.TickDay` | per day chain | Once per day over a handful of trains; non-capturing lambdas | Unchanged |

## Changes

- `Owners` returns a lazily built `ReadOnlyCollection` view, cleared on
  `Register` and `Unregister`. The read-only wrapper stops a caller from
  mutating the shared view through a `List` cast.
- `RecordDailyHealthTrend` finds the previous trend with a forward scan using
  strict `>`. This keeps the tie rule of the stable `OrderByDescending` it
  replaces (earliest-inserted row wins among equal days). The cost drops from
  O(n log n) plus iterator allocations to O(n) with none.

## Verification

| Command | Result |
|---|---|
| `bash scripts/run_test.sh Ashfall.Core.Tests/Campaign/CampaignDayCoordinatorTests.cs` | 20/20 PASS (new `Owners_CachedViewIsStableReadOnlyAndInvalidatedOnRegisterUnregister`) |
| `bash scripts/run_test.sh Ashfall.Core.Tests/Campaign/CampaignDayCoordinatorSourceGateTests.cs` | 4/4 PASS |
| `bash scripts/run_test.sh Ashfall.Core.Tests/Medical/Plan198HealthHistoryIntegrationTests.cs` | 6/6 PASS |

**Skipped:** the `--runtime-scale-selftest` before/after run. It needs a host
build of `Ashfall.csproj`, which is dirty and claimed by the active
performance-lane build builder. Neither change touches the daily tick path, so
the harness would not show a difference. The existing measurement (~2,640 B
median per day tick, inside budget) stands.

## Follow-ups (not in scope)

Per-day LINQ chains in Agriculture, Espionage, Quest, FluidLogistics, Echo,
ShelterThermal, and WildlifeEcosystem, plus eight method-group `Sort`
delegates in `DailyBriefingReportBuilder`, are cold enough at current roster
sizes to leave. Profile them before changing anything.
