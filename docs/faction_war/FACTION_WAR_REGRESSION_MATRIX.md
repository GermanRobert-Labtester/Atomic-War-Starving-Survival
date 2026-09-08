# Faction War Regression Matrix & Risk Mitigation

> **Scope:** Plan 124 Expansion (9 → 20 Location Overrides)
> **Primary Risk Surfaces:** Catalog Deserialization, Location Orphan References, Day Window Clamping, Precedence Sorting, Save Pollution, Narrative Quality.

---

## 1. Risk Surface & Mitigation Matrix

| Risk ID | Potential Failure Mode | Severity | Mitigation Strategy | Test / Gate Verification |
|---|---|---|---|---|
| **RS-01** | Baseline overrides (0..8) altered or truncated during expansion. | Critical | Preserve entries 0..8 byte-for-byte; append entries 9..19 to the end of the JSON array. | `BaselineOverrides_PreservedAtIndices0Through8` unit test. |
| **RS-02** | New override references non-existent or misspelled `locationId`. | High | Cross-reference all 11 new entries against canonical location files (`deep_lore_locations.json`, `crossing_locations.json`, `year_of_ash_locations.json`, `locations.json`). | `AllOverrides_TargetCanonicalLocations` unit test. |
| **RS-03** | Inverted or negative day window (`activeUntilDay < activeFromDay` or `< 1`). | Medium | Enforce schema rule: `activeFromDay >= 1` and `(activeUntilDay == 0 \|\| activeUntilDay >= activeFromDay)`. | `AllOverrides_DayWindowsAreOrderedAndBounded` and updated catalog test. |
| **RS-04** | Invalid `overrideType` string injected, causing UI parser failures. | Medium | Restrict `overrideType` to the 9 sanctioned types (`pre_strike`, `post_strike`, `ambient_addendum`, `occupied`, `abandoned`, `fortified`, `liberated`, `reclaimed`, `contaminated`). | `LocationOverrides_HaveRequiredFieldsAndConsistentDayWindowRule` test. |
| **RS-05** | Overlapping override conflicts causing non-deterministic state flapping. | High | Target distinct locations for all 11 new overrides; rely on stable `activeFromDay` precedence in `FactionWarContentCatalog`. | `SequentialOverrides_Almshouse_TransitionsAtDay101` and precedence test suite. |
| **RS-06** | Stale Linux build copy causing discrepancy between dev and release builds. | High | Synchronize `builds/linux/Assets/StreamingAssets/Data/faction_war_location_overrides.json` immediately. | Parity verification in build directory. |
| **RS-07** | Dangerous or policy-violating prose (tactical recipes, UXO handling). | High | Strict narrative tone and policy review; avoid actionable synthesis or technical bomb disposal text. | `AllOverrides_NarrativeContentIsSafeAndAtmospheric` unit test. |
| **RS-08** | Save file corruption or migration requirement. | Critical | Keep overrides purely functional and stateless in memory. No save schema changes. | `BaseLocationDisplay_RestoredWhenOverrideExpires` unit test. |

---

## 2. Test Verification Coverage

All 14 dedicated tests in `Ashfall.Core.Tests/FactionWarLocationOverridesExpansionTests.cs` and the updated test in `Ashfall.Core.Tests/FactionWarContentCatalogTests.cs` pass with zero failures:

1. `Catalog_ContainsExactly20LocationOverrides` (PASS)
2. `BaselineOverrides_PreservedAtIndices0Through8` (PASS)
3. `NewOverrides_HaveValidFieldsAndPrefix` (PASS)
4. `AllOverrides_TargetCanonicalLocations` (PASS)
5. `AllOverrides_DayWindowsAreOrderedAndBounded` (PASS)
6. `OverrideTypes_FollowStandardVocabulary` (PASS)
7. `DeepLoreLocations_CorrectlyTargeted` (PASS)
8. `YearOfAshLocations_CorrectlyTargeted` (PASS)
9. `CrossingLocations_CorrectlyTargeted` (PASS)
10. `DayWindowBoundaries_InclusiveAndDeterministic` (PASS)
11. `SequentialOverrides_Almshouse_TransitionsAtDay101` (PASS)
12. `MultipleLocations_CanHaveConcurrentActiveOverrides` (PASS)
13. `BaseLocationDisplay_RestoredWhenOverrideExpires` (PASS)
14. `AllOverrides_NarrativeContentIsSafeAndAtmospheric` (PASS)
