# Plan 96 — Regression & Verification Matrix

**Document ID:** `docs/endgame/PLAN96_REGRESSION_MATRIX.md`
**Test Suite:** `Ashfall.Core.Tests/Endgame/EpilogueChronicleCatalogTests.cs` (14/14 PASS)

---

## 1. Verified Test Execution Summary

| Gate / Command | Target / Scope | Result | Details |
|---|---|---|---|
| `dotnet test Ashfall.Core.Tests --filter EpilogueChronicleCatalogTests` | Catalog integrity, 20 slides, uniqueness, baseline parity, Plan 89 bindings | **PASS** | 14 passed, 0 failed. |
| `dotnet test Ashfall.Core.Tests --filter EpilogueChronicleBuilderTests` | Deterministic sorting, fate cards, metrics, title resolution | **PASS** | 7 passed, 0 failed. |
| `godot --headless --path . -- --data-integrity-selftest` | Whole-workspace JSON integrity gate (216 catalogs) | **PASS** | 0 findings, 0 errors, 216/216 passed. |
| `godot --headless --path . -- --content-utilization-selftest` | Static inventory & runtime simulation evidence | **PASS** | CI Gate PASS (498 catalogs scanned). |
| `godot --headless --path . -- --endgame-v1-selftest` | Campaign termination, state transitions, report generation, sealing | **PASS** | 16/16 passed. |
| `dotnet build Ashfall.csproj` | Godot .NET Host project compilation | **PASS** | 0 warnings, 0 errors. |
| `python3 scripts/ci/scene-lint.py` | Godot production scene AST validation | **PASS** | 27 production scenes clean. |

---

## 2. Invariant Guard Checks

- **Invariant 1 (Zero Engine Coupling):** `EpilogueChronicleCatalog.cs` references zero `Godot.*` or `UnityEngine.*` symbols.
- **Invariant 3 (Save Compatibility):** Save checksums unchanged; catalog data remains external to saves.
- **Invariant 4 (Determinism):** `EpilogueChronicleBuilder` sorts stably by integer order; identical input yields identical sequence.
- **Invariant 5 (No Host Gameplay Logic):** Presentation-only DTOs and loaders; zero simulation logic in Godot nodes.
- **Invariant 6 (JSON Data Authority):** `Assets/StreamingAssets/Data/epilogue_chronicle.json` is the sole authority.
