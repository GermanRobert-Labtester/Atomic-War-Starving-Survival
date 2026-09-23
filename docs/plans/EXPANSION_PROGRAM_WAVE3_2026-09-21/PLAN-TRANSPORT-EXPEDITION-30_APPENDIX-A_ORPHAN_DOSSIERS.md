# PLAN-TRANSPORT-EXPEDITION-30 — Appendix A: Orphan Dossiers (transport & expedition)

**Generated:** 2026-09-21 from the host-reachability audit, filtered to this
plan's domain: **5 host-unreachable authorities** wired by this plan's
packages (see the parent plan's seam map; the same systems appear in
`PLAN-ORPHAN-SEAL-01` Appendix A with wave assignment).
**Note:** requested-but-unreachable-listed systems not present: `RailwayInterlockEngine`, `RailTrackMaintenanceEngine`, `VehicleCustomizationSystem` (they are either host-reachable already or type-level dead — see the Plan 1 appendices).
**Use:** each dossier lists the authority, file, known tests, candidate
catalogs, and the parent-plan mechanic that consumes it. A package claim covers
one or more systems end-to-end (host path, day owner if stateful, save path,
one player surface, focused tests).

## Dossiers

### 01. `ColonySystem`
- **File:** `Expeditions/ColonySystem.cs` · **Types:** `ColonySystem`
- **Known tests (2):** `Expeditions/ColonySystemTests.cs`, `Expeditions/Plan160ColonyIntegrationTests.cs`
- **Candidate catalogs:** `colony_blueprints.json`
- **Parent-plan mechanic:** Outposts
- **Wiring recipe:** host path → day owner if stateful → save path → one player surface → focused tests + reachability re-run.
### 02. `AerialReconWindowEngine`
- **File:** `Expeditions/AerialReconWindowEngine.cs` · **Types:** `AerialReconWindowEngine`
- **Known tests (1):** `Expeditions/AerialReconWindowEngineTests.cs`
- **Candidate catalogs:** none by name match
- **Parent-plan mechanic:** Aviation
- **Wiring recipe:** host path → day owner if stateful → save path → one player surface → focused tests + reachability re-run.
### 03. `OutpostSettlementSystem`
- **File:** `Settlements/OutpostSettlementSystem.cs` · **Types:** `OutpostSettlementSystem`
- **Known tests (1):** `Settlements/Plan58OutpostSettlementIntegrationTests.cs`
- **Candidate catalogs:** `wasteland_settlement_npcs.json`, `settlements.json`, `outposts.json`, `wasteland_settlement_gazetteer.json`
- **Parent-plan mechanic:** Outposts
- **Wiring recipe:** host path → day owner if stateful → save path → one player surface → focused tests + reachability re-run.
### 04. `ModalTravelDispatchEngine`
- **File:** `World/ModalTravelDispatchEngine.cs` · **Types:** `ModalTravelDispatchEngine`
- **Known tests (1):** `World/ModalTravelDispatchEngineTests.cs`
- **Candidate catalogs:** none by name match
- **Parent-plan mechanic:** Modal choice
- **Wiring recipe:** host path → day owner if stateful → save path → one player surface → focused tests + reachability re-run.
### 05. `CommunicationsSystem`
- **File:** `Communications/CommunicationsSystem.cs` · **Types:** `CommunicationsSystem`
- **Known tests (1):** `Communications/Plan157CommunicationsIntegrationTests.cs`
- **Candidate catalogs:** `nvis_communications_catalog.json`, `communications_networks.json`
- **Parent-plan mechanic:** —
- **Wiring recipe:** host path → day owner if stateful → save path → one player surface → focused tests + reachability re-run.
