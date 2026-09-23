# PLAN-TEMPORAL-AUTHORITY-33 — Appendix A: Hour & TickHour Consumer Inventory

**Generated:** 2026-09-21 across Core+host (19 files reference
`TickHour` or `HourOfDay`).
**Use:** TM-33A/33C — every hour consumer must read the canonical clock
(`SimClock`/campaign day hour), never a wall-clock or frame count; the
catch-up probe covers the day-boundary owners among them.

## Consumers

| File | TickHour refs | HourOfDay refs |
|---|---:|---:|
| `src/Host/HostCli.PanelTests.cs` | 8 | 0 |
| `src/Host/Phase0HostSession.cs` | 3 | 0 |
| `Assets/Ashfall.Core/Clock/ISimClock.cs` | 0 | 2 |
| `src/Main.ExpandedShelterSystems.cs` | 1 | 1 |
| `src/Main.Survivors.cs` | 2 | 0 |
| `src/Main.CampaignOwners.cs` | 2 | 0 |
| `src/Host/ShelterScheduleHostSession.cs` | 2 | 0 |
| `Assets/Ashfall.Core/ShelterScheduleSystem.cs` | 1 | 0 |
| `Assets/Ashfall.Core/PhantomMemoryEngine.cs` | 1 | 0 |
| `Assets/Ashfall.Core/Verdict/VerdictCensusBroadcast.cs` | 0 | 1 |
| `Assets/Ashfall.Core/Campaign/CampaignCalendar.cs` | 0 | 1 |
| `Assets/Ashfall.Core/Emergency/EmergencyAlertSystem.cs` | 1 | 0 |
| `src/Main.UiTests.Phase0.cs` | 1 | 0 |
| `src/Main.UiTests.Survivors.cs` | 1 | 0 |
| `src/Main.Phase0.cs` | 1 | 0 |
| `src/Host/HoldfastRuntimeSession.cs` | 1 | 0 |
| `src/Host/PanelBindLifecycleSelfTest.cs` | 1 | 0 |
| `src/Host/SurvivorsHostSession.cs` | 1 | 0 |
| `src/Host/PhantomMemoryHostSession.cs` | 1 | 0 |
