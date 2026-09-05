# PLAN91 REGRESSION MATRIX

Command-by-command results, before vs. after Plan 91.

| # | Gate | Baseline (Phase 0) | After Plan 91 | Verdict |
|---|---|---|---|---|
| 1 | `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` | PASS 0/0 | PASS 0/0 | ✔ no regression |
| 2 | `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` | 6892/6895 (3 pre-existing muster-ending failures) | **7003/7003 PASS** | ✔ green (muster debt fixed concurrently; +21 Plan 91 tests) |
| 3 | `dotnet build Ashfall.csproj` | PASS 0/0 | PASS 0/0 | ✔ no regression |
| 4 | `godot --headless -- --data-integrity-selftest` | FAIL(13) — muster `ending_*` only | **PASS, 0 errors, 208 catalogs** | ✔ green |
| 5 | `godot --headless -- --greenhouse-selftest` | PASS 24/24 | PASS 24/24 | ✔ no regression |
| 6 | `godot --headless -- --content-utilization-selftest` | CI gate PASS | CI gate PASS | ✔ no regression |
| 7 | `godot --headless -- --asset-registry-selftest` | PASS | PASS | ✔ no regression |
| 8 | New: `GreenhouseItemCatalogTests` (21 tests) | n/a | 21/21 PASS | ✔ new coverage |

## Plan 91 test coverage map (plan §60-64)

| Plan § | Requirement | Test |
|---|---|---|
| §60 catalog | count 30; original 14 present; 16 supplies present; unique IDs | `GreenhouseFile_ContainsExactlyThirtyEntries`, `_PreservesOriginalFourteen`, `_ContainsAllSixteenNewSupplies`, `_HasUniqueIds` |
| §60 schema | valid types; non-empty name/description | `GreenhouseFile_AllTypesAreValidItemTypeValues`, `_NamesAndDescriptionsNonEmpty` |
| §60 numeric | stack/weight/trade validity; tool physicality; value spread | `GreenhouseFile_NumericRangesValid`, `_HandToolsHaveLowStacksAndLowWeight`, `_NewSuppliesAreNotSameValueClones` |
| §61 registry | 30 entries in merged registry; per-category resolution; no new collisions vs `items.json`; dead parity copies gone | `GlobalCatalog_RegistersAllThirtyGreenhouseEntries`, `_NewSuppliesResolveAcrossCategories`, `_NoIdCollisionsAcrossItemFiles`, `GreenhouseFile_DeadParityCopiesRemoved` |
| §62 crafting | 4 unique recipes; outputs + ingredients resolve; no arbitrage | `Crafting_FourGreenhouseRecipesExistAndAreUnique`, `_GreenhouseRecipeOutputsResolveInGlobalRegistry`, `_GreenhouseRecipeIngredientsResolve`, `_GreenhouseOutputsNotPricedBelowInputValue` |
| §63 scavenging | 3 bindings; refs resolve; sane weight/rarity | `Scavenging_GreenhouseTableBindsThreePlan91Items`, `_BoundItemIdsResolveInGlobalRegistry`, `_BoundEntriesUseSaneWeightsAndRarity` |
| §64 save | static defs stay out of saves; no save-schema change | by design — definitions are load-time data; no save code touched |
| §1.11/§49 | no fake effect fields on supplies | `GreenhouseFile_NewSuppliesClaimNoConsumableEffectFields` |

## Known concurrent-modification note

A second agent was actively editing `Ashfall.Core` (muster epilogue /
`DoseLedgerSave`) during Plan 91 execution. One transient Core compile break
(their mid-edit state) delayed a build; it resolved without any Plan 91
action. The muster `ending_*` integrity failures present at Phase 0 baseline
were fixed by that concurrent work and are **not** claimed by Plan 91.
Plan 91's own diff: `greenhouse_items.json`, `recipes.json`,
`scavenging_tables.json`, `Ashfall.Core.Tests/GreenhouseItemCatalogTests.cs`,
`docs/greenhouse/*` — zero Core/host code changes.
