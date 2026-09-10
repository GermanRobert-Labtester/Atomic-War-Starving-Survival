# Plan 146 Regression & Verification Matrix

**Document ID:** ARCH-PLAN146-REGRESSION-MATRIX
**Status:** Approved Architectural Specification
**Project:** ASHFALL (Godot 4.7+ .NET 8 Host / C# Core)
**Date:** 2026-09-09

---

## 1. Test Suite Architecture

Verification for Plan 146 is implemented in `Ashfall.Core.Tests/BunkerCourtCatalogTests.cs` and evaluated against the standard xUnit test runner and Godot headless self-tests.

---

## 2. Regression Test Cases

| Test Identifier | Test Name | Assertion & Contract | Target File |
|---|---|---|---|
| `TEST-COURT-01` | `BunkerCourt_LoadsAll24CanonicalTrials` | Loads all 24 records from `narrative/bunker_court_verdicts_codex.json`; asserts exact count 24; verifies Case 1 (`case_01_the_air_duct_moonshine_still`) and Case 24 (`case_24_the_ratification_of_the_century_constitution`). | `BunkerCourtCatalogTests.cs` |
| `TEST-COURT-02` | `BunkerCourt_AllEntriesHaveValidFieldsAndUniqueDockets` | Verifies every entry has non-empty `case_id`, `docket_number`, `defendant_name`, `presiding_magistrate`, `charge_summary`, `evidence_presented`, `verdict_outcome`, `disciplinary_penalty`, `clerk_margin_notes` (> 25 chars), non-empty `tags`, and that all docket numbers and case IDs are strictly unique. | `BunkerCourtCatalogTests.cs` |
| `TEST-COURT-03` | `BunkerCourt_LoadIsIdempotent` | Calls `catalog.Load()` multiple times with the same data; asserts `AllCases.Count` remains exactly 24 and does not double or duplicate. | `BunkerCourtCatalogTests.cs` |
| `TEST-COURT-04` | `BunkerCourt_ClearResetsState` | Populates catalog, calls `Clear()`, asserts count is 0 and lookups return null/empty. | `BunkerCourtCatalogTests.cs` |
| `TEST-COURT-05` | `BunkerCourt_DeterministicSortOrder` | Verifies `AllCases` are deterministically ordered by historical day parsed from docket number ascending (from Day 84 to Day 3650). | `BunkerCourtCatalogTests.cs` |
| `TEST-COURT-06` | `BunkerCourt_DocketNumberParsing` | Asserts `GetDocketDay()` accurately extracts the historical day from dockets like `TRIB-084-MOONSHINE` (84) and `TRIB-3650-CONSTITUTION` (3650). | `BunkerCourtCatalogTests.cs` |
| `TEST-COURT-07` | `BunkerCourt_QueryHelpers` | Tests `GetById`, `GetByDocket`, `GetByDefendant`, `GetByMagistrate`, `GetByVerdict`, `GetByTag`, `GetUnlockedByDay`, and `GetBySearch` with both exact and partial matches, verifying case-insensitive behavior and empty list on non-matches. | `BunkerCourtCatalogTests.cs` |
| `TEST-COURT-08` | `BunkerCourt_RejectsMalformedEntries` | Tests that entries with null/empty `case_id` or null/empty `docket_number` are safely rejected without throwing unhandled exceptions. | `BunkerCourtCatalogTests.cs` |
| `TEST-COURT-09` | `BunkerCourt_LoadFromDirectory` | Verifies `LoadFromDirectory` successfully loads all 24 cases from the `narrative/bunker_court_verdicts_codex.json` path using mocked/real `IFileIO`. | `BunkerCourtCatalogTests.cs` |
| `TEST-COURT-10` | `BunkerCourt_ManifestIntegration` | Verifies all 24 cases have corresponding `discovery_id` entries in `narrative_discovery_manifest.json` and that each discovery entry successfully resolves to a valid record. | `BunkerCourtCatalogTests.cs` |
| `TEST-COURT-11` | `BunkerCourt_ZeroGameplayMutationContract` | Explicitly verifies that querying or examining court records causes zero changes to simulated survivor attributes, inventory items, or morale. | `BunkerCourtCatalogTests.cs` |

---

## 3. Mandatory CI Verification Commands

Per Project Rule 5, all five gate commands must exit 0:
1. `dotnet test Ashfall.Core.Tests`
2. `godot --headless --path . -- --content-utilization-selftest`
3. `godot --headless --path . -- --data-integrity-selftest`
4. `godot --headless --path . -- --scene-binding-selftest`
5. `python3 scripts/ci/scene-lint.py`
