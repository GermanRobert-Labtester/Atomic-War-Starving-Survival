# PLAN 148 REGRESSION MATRIX

Comprehensive test and regression plan for Plan 148 Engineering Emergencies & Maintenance Log Activation.

---

## 1. Unit Test Matrix (`Ashfall.Core.Tests/BunkerMaintenanceCatalogTests.cs`)

| Test ID | Test Name | Scope & Assertion | Expected Result |
|---|---|---|---|
| `TEST-ENG-01` | `BunkerMaintenance_LoadsAll20CanonicalGlitches` | Loads `bunker_maintenance_glitches.json`, asserts 20 entries, validates `glitch_01` (Steam) and `glitch_20` (Century Seed), verifies criticals and tags. | PASS |
| `TEST-ENG-02` | `BunkerMaintenance_AllEntriesHaveValidFieldsAndUniqueLogCodes` | Asserts all entries have valid non-empty IDs, unique `ENG-FL-*` log codes, non-empty telemetry, protocols, shift notes, and >= 3 repair kit items. | PASS |
| `TEST-ENG-03` | `BunkerMaintenance_Load_IsIdempotent` | Calls `catalog.Load()` multiple times with identical JSON, asserts `AllGlitches.Count` remains exactly 20 (no duplicates). | PASS |
| `TEST-ENG-04` | `BunkerMaintenance_LoadFromDirectory_ResolvesCanonicalFile` | Tests static factory `BunkerMaintenanceCatalog.LoadFromDirectory()` using `IFileIO` and `IJsonSerializer`. | PASS |
| `TEST-ENG-05` | `BunkerMaintenance_GetByLogCode_ReturnsExactMatch` | Queries known log codes (e.g. `ENG-FL-088-STEAM`, `ENG-FL-3650-TRIUMPH`) case-insensitively; asserts null for invalid codes. | PASS |
| `TEST-ENG-06` | `BunkerMaintenance_GetBySeverity_ReturnsExactTiers` | Queries severity tiers 1 through 5, verifies counts and entry severities match query tier exactly. | PASS |
| `TEST-ENG-07` | `BunkerMaintenance_GetBySearch_FindsRelevantRecords` | Queries full text across subsystem, description, telemetry, notes, and tags (e.g. "steam", "cavitation", "Dmitri"). | PASS |
| `TEST-ENG-08` | `BunkerMaintenance_Projection_MapsCanonicalRooms` | Tests `BunkerMaintenanceProjection.ResolveRoomForGlitch()` on all 20 glitches, asserting canonical `room_*` identifiers. | PASS |
| `TEST-ENG-09` | `BunkerMaintenance_Projection_MapsSubsystemCategories` | Tests `BunkerMaintenanceProjection.ResolveCategory()` on all 20 glitches, verifying valid enum categorization. | PASS |
| `TEST-ENG-10` | `BunkerMaintenance_Projection_ZeroMutationContract` | Verifies querying glitches and projections leaves mock system states completely unchanged. | PASS |
| `TEST-ENG-11` | `BunkerMaintenance_DeterministicOrdering` | Asserts `AllGlitches` can be queried in a deterministic order sorted by severity descending then glitch_id. | PASS |
| `TEST-ENG-12` | `BunkerMaintenance_Clear_ResetsCatalog` | Calls `catalog.Clear()`, asserts count is 0, subsequent reload works cleanly. | PASS |

---

## 2. Verification Gate Matrix (Rule 5)

| Gate Command | Requirement | Pass Criteria |
|---|---|---|
| `dotnet test Ashfall.Core.Tests` | Required | Exit code 0, 0 failed |
| `godot --headless --path . -- --content-utilization-selftest` | Required | Exit code 0, CI gate PASS |
| `godot --headless --path . -- --data-integrity-selftest` | Required | Exit code 0, 0 errors |
| `godot --headless --path . -- --scene-binding-selftest` | Required | Exit code 0, 25/25 passed |
| `python3 scripts/ci/scene-lint.py` | Required | Exit code 0, 0 errors |
