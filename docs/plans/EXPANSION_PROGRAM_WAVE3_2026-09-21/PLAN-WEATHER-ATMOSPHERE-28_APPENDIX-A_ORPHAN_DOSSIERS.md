# PLAN-WEATHER-ATMOSPHERE-28 — Appendix A: Orphan Dossiers (weather & atmosphere)

**Generated:** 2026-09-21 from the host-reachability audit, filtered to this
plan's domain: **5 host-unreachable authorities** wired by this plan's
packages (see the parent plan's seam map; the same systems appear in
`PLAN-ORPHAN-SEAL-01` Appendix A with wave assignment).

**Use:** each dossier lists the authority, file, known tests, candidate
catalogs, and the parent-plan mechanic that consumes it. A package claim covers
one or more systems end-to-end (host path, day owner if stateful, save path,
one player surface, focused tests).

## Dossiers

### 01. `CascadeTargetSystem`
- **File:** `Weather/WeatherGameplayCascadeEngine.cs` · **Types:** `CascadeTargetSystem`, `WeatherGameplayCascadeEngine`
- **Known tests (1):** `Weather/Plan135WeatherCascadeIntegrationTests.cs`
- **Candidate catalogs:** `comms_targets.json`, `cascade_rules.json`
- **Parent-plan mechanic:** —
- **Wiring recipe:** host path → day owner if stateful → save path → one player surface → focused tests + reachability re-run.
### 02. `WeatherCascadeSystem`
- **File:** `Weather/WeatherCascadeSystem.cs` · **Types:** `WeatherCascadeSystem`
- **Known tests (1):** `Weather/Plan135WeatherCascadeIntegrationTests.cs`
- **Candidate catalogs:** `weather_hardening_upgrades.json`, `weather_route_gates.json`, `weather_effects.json`, `weather_seasons.json`
- **Parent-plan mechanic:** Host-unreachable authorities
- **Wiring recipe:** host path → day owner if stateful → save path → one player surface → focused tests + reachability re-run.
### 03. `NuclearWinterProgressionSystem`
- **File:** `Weather/NuclearWinterProgressionSystem.cs` · **Types:** `NuclearWinterProgressionSystem`
- **Known tests (1):** `Weather/Plan164NuclearWinterIntegrationTests.cs`
- **Candidate catalogs:** `nuclear_winter_phases.json`
- **Parent-plan mechanic:** Nuclear winter
- **Wiring recipe:** host path → day owner if stateful → save path → one player surface → focused tests + reachability re-run.
### 04. `WeatherForecastReliabilityEngine`
- **File:** `World/WeatherForecastReliabilityEngine.cs` · **Types:** `WeatherForecastReliabilityEngine`
- **Known tests (1):** `World/WeatherForecastReliabilityEngineTests.cs`
- **Candidate catalogs:** none by name match
- **Parent-plan mechanic:** Forecast reliability
- **Wiring recipe:** host path → day owner if stateful → save path → one player surface → focused tests + reachability re-run.
### 05. `StormForecastReadinessEngine`
- **File:** `World/StormForecastReadinessEngine.cs` · **Types:** `StormForecastReadinessEngine`
- **Known tests (1):** `World/StormForecastReadinessEngineTests.cs`
- **Candidate catalogs:** none by name match
- **Parent-plan mechanic:** Forecast → decisions
- **Wiring recipe:** host path → day owner if stateful → save path → one player surface → focused tests + reachability re-run.
