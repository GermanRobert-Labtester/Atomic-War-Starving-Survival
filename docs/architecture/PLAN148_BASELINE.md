# PLAN 148 BASELINE: Engineering Emergencies & Maintenance Log Runtime Activation

## 1. Mission & Objective
Plan 148 activates ASHFALL's subterranean engineering emergency corpus (`narrative/bunker_maintenance_glitches.json`) and reconciles it with the ambient maintenance log corpora (`narrative/engineering_logs_expansion.json`, `bunker_maintenance_logs_batch_2.json`, `bunker_maintenance_logs_batch_3.json`, and `engineering_mod_notes.json`).

The core mandate is to establish an environmental and diagnostic engineering projection layer connected to real shelter systems (Power, Heating/Thermal, Water/Sump, Ventilation/Air, and Structural Airlock) without allowing static narrative text to manufacture false failures, consume fictional parts, or corrupt the authoritative simulation loops.

---

## 2. Problem Statement & Historical Gaps
Prior to Plan 148:
1. **Catalog Vulnerability**: `BunkerMaintenanceCatalog.Load()` was non-idempotent; invoking `Load()` repeatedly appended entries to `_allGlitches` while overwriting `_byId`, causing unbounded memory growth and broken enumeration counts.
2. **Missing Query Surface**: `BunkerMaintenanceCatalog` had minimal query methods (`GetById`, basic `GetBySubsystem`, `GetCriticalEmergencies`, `GetByTag`). Missing search, severity filtering, log code lookup, and canonical room projections.
3. **No Spatial/Subsystem Projection**: Glitch logs referenced free-form descriptive subsystem strings (e.g., `"Sub-Level 2 Heating Manifold & Residential Block B"`) with no structured projection to canonical shelter rooms (`room_*`) or `MachineConditionKeys`.
4. **Disjoint Repair Kit Identifiers**: 79 authored repair kit item strings (e.g. `item_heavy_welding_rig`, `item_hepa_filter_cartridge`) existed purely in narrative prose without explicit mapping to canonical inventory items or fallback component categories in `items.json`.
5. **Disconnected Host Wiring**: No dedicated accessor `GetBunkerMaintenanceCatalog()` existed on `Main.ShelterInfrastructure.cs`, and `ContentUtilizationScanner.cs` lacked explicit loader/registry registration for `bunker_maintenance_glitches.json`.

---

## 3. Core Invariants & Architectural Boundaries
1. **Invariant 1: Simulation Authority**: Live simulation systems (`StartingLevelSystem`, `PowerGridSystem`, `VentilationSystem`, `ShelterThermalSystem`, `SumpFloodingSystem`) own shelter state and condition meters. The engineering glitch catalog is strictly read-only and never mutates machine health, power draw, or resource stockpiles.
2. **Invariant 2: One-Way Projection**: Live machine readings project into matching diagnostic logs. Glitch entries never trigger real mechanical breakdowns or change runtime state.
3. **Invariant 3: Zero Engine Coupling**: `Assets/Ashfall.Core/Narrative/BunkerMaintenanceCatalog.cs` and `BunkerMaintenanceProjection.cs` are pure C# (`netstandard2.1`), with zero references to Godot, Unity, or UnityEngine.
4. **Invariant 4: Symbolic Repair Kit Discipline**: Repair kit entries in `required_repair_kit` are descriptive narrative requirements unless explicitly resolved via `RepairKitIdentityMatrix`. They never deduct inventory items unless validated by gameplay crafting recipes.
5. **Invariant 5: Save State Neutrality**: The engineering catalog introduces no new save keys or save store mutations. Discovered logs route through the existing `JournalSystem` knowledge keys (`KnowledgeKeys.NarrativeDiscovered`), ensuring zero save format breakages.

---

## 4. Scope of Implementation
- **Core Catalog Hardening**:
  - Fix `Load()` idempotence and deduplication in `BunkerMaintenanceCatalog.cs`.
  - Add `Clear()`, `LoadFromDirectory()`, `GetByLogCode()`, `GetBySeverity()`, and `GetBySearch()`.
  - Implement deterministic ordering by severity tier and glitch ID.
- **Subsystem & Spatial Projection**:
  - Implement `BunkerMaintenanceProjection.cs` mapping all 20 glitches to canonical rooms (`room_*`), subsystem categories (`SubsystemCategory`), and `MachineConditionKeys`.
- **Repair Kit Identity**:
  - Document all 79 repair kit items with canonical mappings and fallback component categories.
- **Host Integration**:
  - Add `GetBunkerMaintenanceCatalog()` in `Main.ShelterInfrastructure.cs`.
  - Register loader, registry, and consumer edges in `ContentUtilizationScanner.cs`.
- **Verification**:
  - Comprehensive unit test suite in `Ashfall.Core.Tests/BunkerMaintenanceCatalogTests.cs`.
  - Pass the full CI verification matrix (`dotnet test`, `--content-utilization-selftest`, `--data-integrity-selftest`, `--scene-binding-selftest`, `scene-lint.py`).
