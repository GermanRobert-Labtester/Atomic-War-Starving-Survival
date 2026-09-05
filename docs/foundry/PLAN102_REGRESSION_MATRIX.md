# Plan 102 Regression Test Matrix

**Suites:**
- `Ashfall.Core.Tests/FoundryAccordExpansionTests.cs` (10 tests)
- `Ashfall.Core.Tests/World/Plan16CartographyTests.cs` (Regional treaties check)
- `Assets/Ashfall.Core/Foundry/SilentFoundryHeadlessDemo.cs` (Foundry accords check)
- `godot --headless --data-integrity-selftest` (215 catalogs)

---

## 1. Unit Test Matrix

| Suite | Test Method | Scenario Verified | Result |
|---|---|---|---|
| `FoundryAccordExpansionTests` | `Catalog_LoadsAllAccordsWithoutErrors` | Asserts catalog loads $\ge 10$ accords (12 total), schema version 1, non-empty collection ID. | **PASS** |
| `FoundryAccordExpansionTests` | `Parity_BaselineFourDistrict8AccordsPreserved` | Confirms all 4 baseline District 8 accords exist with exact IDs, days (280, 305, 330, 365), titles, and terms. | **PASS** |
| `FoundryAccordExpansionTests` | `TreatyId_AllIdsAreUniqueAndFollowSnakeCasePrefix` | Validates every ID starts with `treaty_`, is snake_case, and is unique. | **PASS** |
| `FoundryAccordExpansionTests` | `Signatories_AllFactionsAreValidAndNonEmpty` | Validates all signatory factions are non-empty and exist in the faction directory. | **PASS** |
| `FoundryAccordExpansionTests` | `Resources_WaterAndPowerAllocationsAreNonNegativeAndPlausible` | Verifies water/power allocations are $\ge 0.0$ and within industrial bounds. | **PASS** |
| `FoundryAccordExpansionTests` | `LegalText_ArticlesFollowNumberedClausesAndPenaltiesAreEnforceable` | Verifies non-empty demarcations, tariffs, `ARTICLE 1/2/3` formatting, and substantive penalties. | **PASS** |
| `FoundryAccordExpansionTests` | `Tags_FollowNormalizedVocabularyWithoutSynonymSplits` | Asserts all tags conform to the 42-keyword normalized vocabulary without synonym drift. | **PASS** |
| `FoundryAccordExpansionTests` | `Timeline_RatificationDaysAreChronologicallyOrdered` | Verifies ratification days span Days 120 to 365 and query-by-day progression works. | **PASS** |
| `FoundryAccordExpansionTests` | `Diversity_CatalogSpansResourceLogisticsTerritorialAndGovernanceRoles` | Asserts functional diversity (resource, logistics, labor, trade, demilitarization, sanctuary). | **PASS** |
| `FoundryAccordExpansionTests` | `ConsequenceSeam_Plan103PoliciesResolveAgainstTheseAccords` | Confirms all 15 Plan 103 consequence policies resolve against these accords. | **PASS** |

---

## 2. Headless Verification Commands

- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter FoundryAccordExpansionTests`: 10/10 PASS.
- `godot --headless --path . -- --silent-foundry-selftest`: 26/26 PASS (Exact 4 District 8 accords verified).
- `godot --headless --path . -- --data-integrity-selftest`: 215 catalogs PASS (0 errors).
- `godot --headless --path . -- --content-utilization-selftest`: CI gate PASS.
- `dotnet build Ashfall.csproj`: 0 warnings, 0 errors.
