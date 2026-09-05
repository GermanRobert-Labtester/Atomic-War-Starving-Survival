# Plans 146–149 Authority & Forensic Audit

**Document ID:** ARCH-AUDIT-146-149
**Status:** Canonical architectural baseline
**Project:** ASHFALL (Godot 4.7+ .NET 8 Host / C# Core)
**Date:** 2026-09-05

---

## 1. Executive Summary

This document establishes the verified forensic baseline and authority mapping for the implementation of:
- **Plan 146:** EB-PVD Thermal Barrier Coatings
- **Plan 147:** Mine-Clearing Flail Vehicle Module
- **Plan 148:** Microfluidic Diagnostic Cartridge System
- **Plan 149:** Rail Grinding & Strategic Corridor Rehabilitation

---

## 2. Forensic Mapping of Proposed Dependencies

| Proposed Dependency Name | Status in Repo | Live Production Replacement | Architectural Reason & Seam |
|---|---|---|---|
| `VacuumInductionMeltingEngine` | Absent (superseded) | `SilentFoundrySystem` (`Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs`) | Foundry production authority is centralized in the Silent Foundry system and `foundry_production.json`. EB-PVD consumes superalloy component blanks from Silent Foundry. |
| `RefractoryCeramicsEngine` | Absent (superseded) | `SilentFoundrySystem` & catalog items | Ceramic precursors and crucibles are catalog items/products within the foundry and chemical synthesis domains. |
| `FoundryProductionSystem` | Replaced | `SilentFoundrySystem` | `SilentFoundrySystem` owns smelter bays, heats, products, and inventory ports. |
| `GroundPenetratingRadarEngine` | Absent (unverified) | `GeodeticSurveyEngine` (Plan 79) & `ReconTelemetrySystem` | Minefield detection and road corridor discovery map into `RouteInfrastructureSystem` and `ReconTelemetrySystem`. |
| `DraisineTransmissionEngine` | Absent (unverified) | `RailwaySystem` & `DraisineRecoverySystem` | `RailwaySystem` (`Assets/Ashfall.Core/Expeditions/RailwaySystem.cs`) and `DraisineRecoverySystem` govern rail network segments and derailments. |
| `RailwayInterlockEngine` | Absent (unverified) | `RailwaySystem` (`RailwayNetworkCatalog`) | Network route segments and switchyard flow are managed via `RailwaySystem`. |
| `LyophilizationEngine` | Live | `LyophilizationSystem` (Plans 130–133) | Live system for preserved biologics batches. Microfluidic diagnostics consumes preserved reagents. |
| `MedicalSystem` | Deleted legacy monolith | `MedicalPipelineCoordinator`, `DiseaseSystem`, `DiagnosisKnowledgeStore`, `MedicalWardSystem` | Diagnostic cartridges generate probabilistic clinical evidence routed via `DiagnosisKnowledgeStore` without violating hidden state. |

---

## 3. Data Authority & Schema Contracts

1. **Items Authority:** `Assets/StreamingAssets/Data/items.json`.
   - New items follow `item_` prefix with strict snake_case naming.
2. **Catalogs Authority:**
   - `ebpvd_coating_catalog.json` (Plan 146)
   - `mine_flail_catalog.json` (Plan 147)
   - `microfluidic_diagnostic_catalog.json` (Plan 148)
   - `rail_grinding_catalog.json` (Plan 149)
   Every catalog must feature a top-level `schema_version: 1` and be validated by `CatalogIntegrityValidator`.
3. **Route Topology Authority:**
   - `WastelandMapSystem` remains the single world topology and edge authority.
   - `RouteInfrastructureSystem` (`Assets/Ashfall.Core/World/RouteInfrastructureSystem.cs`) owns mutable route condition: minefield clearance, rail roughness, and speed caps.

---

## 4. Save Architecture & Persistence

All new persistence sections use `SaveStoreHub.FromCodec` and the checksummed `SaveStore<T>` envelope model:
- `route_infrastructure` -> `route_infrastructure_save.json`
- `ebpvd_coating` -> `ebpvd_coating_save.json`
- `microfluidic_diagnostic` -> `microfluidic_diagnostic_save.json`
- `mine_clearing_flail` -> `mine_clearing_flail_save.json`
- `rail_grinding` -> `rail_grinding_save.json`

Registered canonically in `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs`.

---

## 5. Determinism and RNG Stream Policy

In accordance with Core Invariant 4:
- Zero use of `System.Random`, `Guid.NewGuid()`, or system clock.
- All stochastic resolution uses `ISeededRng` (xorshift64*), parameterized per operation.
