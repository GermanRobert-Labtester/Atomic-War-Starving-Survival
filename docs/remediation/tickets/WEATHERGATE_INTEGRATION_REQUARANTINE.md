# WeatherGate Integration Suite — Quarantine Ticket

**Audit:** local post-PR #36 issue **#48**
**Date:** 2026-09-05
**Status:** Remain Compile-Remove until a dedicated rematch lane

## Quarantined files

- `Ashfall.Core.Tests/World/WeatherGateCrossSystemIntegrationTests.cs`
- `Ashfall.Core.Tests/World/WeatherGateDebtInteractionTests.cs`
- `Ashfall.Core.Tests/World/WeatherGateSeasonalInteractionTests.cs`
- `Ashfall.Core.Tests/World/WeatherGateTerritoryInteractionTests.cs`
- `Ashfall.Core.Tests/World/WeatherGateWarInteractionTests.cs`

## Why still quarantined

These suites couple weather gates to debt / season / territory / war authorities.
They were Compile-Removed before the local audit and are **out of scope** for the
36–50 hygiene wave. Blind dequarantine risks a large red CI surface without a
rematch owner.

## Activation criteria (WG-01)

1. Rebuild one file at a time against current WeatherSystem APIs.
2. Fix or rewrite assertions to current debt/season/territory/war surfaces.
3. Prove with `dotnet test --filter WeatherGate` green.
4. Remove the matching `<Compile Remove>` lines in the same PR.
5. Close this ticket.

**Owner lane:** World / weather integration
**Expiry review:** 2026-Q4
