# Plan 96 — Regression & Verification Matrix

**Document ID:** `docs/endgame/PLAN96_REGRESSION_MATRIX.md`
**Test Suite:** `Ashfall.Core.Tests/Endgame/EpilogueChronicleCatalogTests.cs` (14/14 PASS)

---

## 1. Verified Test Execution Summary

| Gate / Command | Target / Scope | Result | Details |
|---|---|---|---|
| `dotnet test Ashfall.Core.Tests --filter EpilogueChronicleCatalogTests` | Catalog integrity, 20 slides, uniqueness, baseline parity, Plan 89 bindings | **PASS** | 14 passed, 0 failed. |
| `dotnet test Ashfall.Core.Tests --filter EpilogueChronicleBuilderTests` | Deterministic sorting, fate cards, metrics, title resolution | **PASS** | 7 passed, 0 failed. |
| `dotnet test Ashfall.Core.Tests` | Whole-Core xUnit test suite (deterministic, regression-free) | **PASS** | 9,784 passed, 0 failed. |
| `godot --headless --path . -- --data-integrity-selftest` | Whole-workspace JSON integrity gate (298 catalogs) | **PASS** | 0 findings, 0 errors across 298 catalogs. |
| `godot --headless --path . -- --content-utilization-selftest` | Static inventory & runtime simulation evidence | **PASS** | CI Gate PASS (581 catalogs scanned). |
| `godot --headless --path . -- --scene-binding-selftest` | Host panel node binding and scene tree verification | **PASS** | 25 passed, 0 failed. |
| `dotnet build Ashfall.csproj` | Godot .NET Host project compilation | **PASS** | 0 warnings, 0 errors. |
| `python3 scripts/ci/scene-lint.py` | Godot production scene AST validation | **PASS** | 30 production scenes checked; 0 errors. |

---

## 2. Invariant Guard Checks

- **Invariant 1 (Zero Engine Coupling):** `EpilogueChronicleCatalog.cs` references zero `Godot.*` or `UnityEngine.*` symbols.
- **Invariant 3 (Save Compatibility):** Save checksums unchanged; catalog data remains external to saves.
- **Invariant 4 (Determinism):** `EpilogueChronicleBuilder` sorts stably by integer order; identical input yields identical sequence.
- **Invariant 5 (No Host Gameplay Logic):** Presentation-only DTOs and loaders; zero simulation logic in Godot nodes.
- **Invariant 6 (JSON Data Authority):** `Assets/StreamingAssets/Data/epilogue_chronicle.json` is the sole authority.
