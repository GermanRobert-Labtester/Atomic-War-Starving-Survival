# Plan 16 Cartography & Infrastructure Regression Matrix

**Verification Date:** 2026-09-01
**Project:** ASHFALL (Godot 4.7+ / .NET 8 / C# / xUnit .NET 9)

---

## 1. Test Suite & Verification Results

| Verification Stage | Command Line Verb | Expected | Result | Notes |
|---|---|---|---|---|
| **Dotnet Unit & Integration Suite** | `dotnet test Ashfall.Core.Tests` | 0 failed | **PASS** | Includes `Plan16CartographyTests` (7 test cases, 100% assertions green) |
| **Cartography & Infrastructure Selftest** | `godot --headless --path . -- --cartography-selftest` | 0 failures | **PASS** | Validates 60 nodes, 202 routes, waystations, caravans, treaties, damaged zones |
| **World Exploration Selftest** | `godot --headless --path . -- --world-exploration-selftest` | 17/17 PASS | **PASS** | Deep-strata excavations, cipher decode loop, and living geography |
| **Data Integrity Selftest** | `godot --headless --path . -- --data-integrity-selftest` | 0 errors | **PASS** | 140+ catalogs validated across Tier 1, Tier 2, and definition uniqueness |
| **Content Utilization Selftest** | `godot --headless --path . -- --content-utilization-selftest` | CI gate PASS | **PASS** | 415+ total catalogs scanned and utilized |
| **Scene Binding Selftest** | `godot --headless --path . -- --scene-binding-selftest` | 22/22 passed | **PASS** | All Godot UI scenes cleanly bound |
| **Scene Linter** | `python3 scripts/ci/scene-lint.py` | 0 errors | **PASS** | 26 Godot `.tscn` scenes verified |

---

## 2. Invariant Compliance Checklist

- [x] **Invariant 1 (Zero engine coupling in Core):** `WaystationNetworkSystem.cs`, `WaystationCatalogLoader.cs`, and `CaravanCatalogLoader.cs` in `Assets/Ashfall.Core/` reference zero Godot/Unity types.
- [x] **Invariant 2 (Ports and Adapters):** File I/O and JSON serialization use `IFileIO` and `IJsonSerializer` abstractions.
- [x] **Invariant 3 (Cross-host save compatibility):** State serialization follows standard snake_case and DTO contracts.
- [x] **Invariant 4 (Determinism):** Deterministic route planning (BFS) and deterministic caravan step progressions.
- [x] **Invariant 5 (No gameplay logic in hosts):** Thin nodes and CLI facades; all logistics math lives in Core.
- [x] **Invariant 6 (Data authority is JSON):** `wasteland_map_v1.json`, `waystations.json`, `caravans.json`, `foundry_accords.json`, `foundry_treaty_consequences.json`, `damaged_map_zones.json` serve as single sources of truth.
