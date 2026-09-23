# PLAN-WATER-AGRICULTURE-46 — Appendix A: Orphan Dossiers (water & agriculture)

**Generated:** 2026-09-21 from the host-reachability audit, filtered to this
plan's domain: **3 host-unreachable authorities** wired by this plan's
packages (see the parent plan's seam map; the same systems appear in
`PLAN-ORPHAN-SEAL-01` Appendix A with wave assignment).

**Use:** each dossier lists the authority, file, known tests, candidate
catalogs, and the parent-plan mechanic that consumes it. A package claim covers
one or more systems end-to-end (host path, day owner if stateful, save path,
one player surface, focused tests).

## Dossiers

### 01. `WaterQualityProfileEngine`
- **File:** `Water/WaterQualityProfileEngine.cs` · **Types:** `WaterQualityProfileEngine`
- **Known tests (1):** `Water/WaterQualityProfileEngineTests.cs`
- **Candidate catalogs:** `water_quality_test_reports_batch_2.json`
- **Parent-plan mechanic:** Core
- **Wiring recipe:** host path → day owner if stateful → save path → one player surface → focused tests + reachability re-run.
### 02. `WaterSourceSystem`
- **File:** `Water/WaterSourceSystem.cs` · **Types:** `WaterSourceSystem`
- **Known tests (2):** `Water/Plan189WaterSourceIntegrationTests.cs`, `Emergency/Plan194EmergencyAlertIntegrationTests.cs`
- **Candidate catalogs:** `guilt_sources.json`, `water_sources.json`, `noise_sources.json`, `boiler_feedwater_deaerator_audits.json`
- **Parent-plan mechanic:** Core
- **Wiring recipe:** host path → day owner if stateful → save path → one player surface → focused tests + reachability re-run.
### 03. `SoilReclamationProfileEngine`
- **File:** `Farming/SoilReclamationProfileEngine.cs` · **Types:** `SoilReclamationProfileEngine`
- **Known tests (1):** `Farming/SoilReclamationProfileEngineTests.cs`
- **Candidate catalogs:** none by name match
- **Parent-plan mechanic:** Irrigation
- **Wiring recipe:** host path → day owner if stateful → save path → one player surface → focused tests + reachability re-run.
