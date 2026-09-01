# Dynamic World Regression Matrix

> **Authority:** Plan 19 Verification Matrix

---

## 1. Test Suite & Verification Matrix

| Verification Target | Command / Test Path | Status | Evidence |
|---|---|---|---|
| **Plan 19 Unit & Determinism Suite** | `dotnet test Ashfall.Core.Tests --filter "Plan19DynamicWorldTests"` | **PASS** | 9/9 tests passed (0 failures) |
| **All Weather Tests** | `dotnet test Ashfall.Core.Tests --filter "Weather"` | **PASS** | 114/114 tests passed (0 failures) |
| **Headless Dynamic World Selftest** | `godot --headless --path . -- --dynamic-world-selftest` | **PASS** | 51/51 assertions passed |
| **Data Integrity Gate** | `godot --headless --path . -- --data-integrity-selftest` | **PASS** | 0 errors across 144 catalogs |
| **Full Core Unit Suite** | `dotnet test Ashfall.Core.Tests` | **PASS** | 5,449 tests passed (0 failures) |

---

## 2. Invariant Check

- **Invariant 1:** Zero engine references in `Assets/Ashfall.Core/`.
- **Invariant 2:** Single coordinator model through `WeatherIntelligenceCoordinator`.
- **Invariant 3:** Cross-save compatibility preserved in `WorldSaveStore`.
- **Invariant 4:** Lookahead and orbital telemetry resolve deterministically with `ISeededRng`.
- **Invariant 5:** Zero gameplay simulation logic in Godot UI nodes.
- **Invariant 6:** JSON catalogs (`weather_seasons.json`, `orbital_harrow_events.json`, `seasonal_events.json`) are the single authority.
