# Plan 41 Regression Matrix

| Test Suite | Filter / Scope | Tests Run | Result |
|---|---|:---:|:---:|
| `Ashfall.Core.Tests` | `ShelterRoomCatalog` & `ShelterAssignment` | 37 | **PASS** |
| `dotnet build` | `Ashfall.csproj` (Godot Host) | 1 | **PASS (0 errors)** |
| Data Integrity Selftest | `godot --headless -- --data-integrity-selftest` | 171 catalogs | **PASS (0 errors)** |
| Content Utilization Selftest | `godot --headless -- --content-utilization-selftest` | 452 catalogs | **PASS (0 orphaned)** |
| Scene Binding Selftest | `godot --headless -- --scene-binding-selftest` | 22 scenes | **PASS (22/22)** |
| Scene Linter | `python3 scripts/ci/scene-lint.py` | 26 scenes | **PASS (0 errors)** |
