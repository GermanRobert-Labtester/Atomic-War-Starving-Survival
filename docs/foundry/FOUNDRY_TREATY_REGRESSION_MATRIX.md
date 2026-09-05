# Foundry Treaty Consequence Regression Test Matrix

**Suites:**
- `Ashfall.Core.Tests/SilentFoundryConsequenceTests.cs` (27 tests)
- `Ashfall.Core.Tests/FoundryTreatyConsequenceExpansionTests.cs` (14 tests)
- `godot --headless --silent-foundry-selftest` (26 assertion checks)
- `godot --headless --data-integrity-selftest` (215 catalogs checked)

---

## 1. Regression Test Inventory

| Test Class | Method | Scenario Verified | Status |
|---|---|---|---|
| `SilentFoundryConsequenceTests` | `Policy_ExactMappingsLoadAndResolve` | Verifies 15 policies load, District 8 treaties resolve to foundry, regional treaties resolve to signatories, non-empty reasons and valid outcomes. | **PASS** |
| `SilentFoundryConsequenceTests` | `Policy_MarketGoodsResolveInTheActiveEconomyCatalog` | Ensures every policy's market modifiers resolve against `economy_goods.json`. | **PASS** |
| `SilentFoundryConsequenceTests` | `Policy_MetReliefRowsMirrorTheMissPenalties` | Pins symmetric relief vs penalty for baseline District 8 treaties. | **PASS** |
| `SilentFoundryConsequenceTests` | `Data_AllPolicyReferencesResolveInAuthoritativeCatalogs` | Verifies all 15 policies match valid treaty signatories in `foundry_accords.json` and valid goods. | **PASS** |
| `SilentFoundryConsequenceTests` | `SaveRoundTrip_ConsequenceLedgerPreservesIntegrityAndChecksum` | Confirms culture-invariant `SaveChecksum` hash equality across save/load. | **PASS** |
| `FoundryTreatyConsequenceExpansionTests` | `Catalog_LoadsExactlyFifteenPoliciesWithoutErrors` | Asserts `PolicyCount == 15` and `HasErrors == false`. | **PASS** |
| `FoundryTreatyConsequenceExpansionTests` | `ReferenceIntegrity_AllTreatyIdsResolveInFoundryAccords` | Validates every treaty ID against `foundry_accords.json`. | **PASS** |
| `FoundryTreatyConsequenceExpansionTests` | `ReferenceIntegrity_AllFactionIdsAreSignatoriesOfReferencedTreaty` | Validates every faction ID is an official signatory of that specific treaty. | **PASS** |
| `FoundryTreatyConsequenceExpansionTests` | `OutcomeValidation_AllPoliciesUseCanonicalOutcomeVocabulary` | Validates `outcome` is strictly within `{"met", "missed", "violated"}`. | **PASS** |
| `FoundryTreatyConsequenceExpansionTests` | `MechanicalEffectValidation_AllMarketGoodModifiersResolveInEconomyGoods` | Validates `market_modifiers` have non-empty reasons, non-zero deltas, and valid `good_id`. | **PASS** |
| `FoundryTreatyConsequenceExpansionTests` | `PolicyUniqueness_NoDuplicateTreatyAndOutcomeKeys` | Guarantees zero duplicate `(treaty_id, outcome)` tuples in the catalog. | **PASS** |
| `FoundryTreatyConsequenceExpansionTests` | `CoverageMatrix_EightTreatiesCoveredWithRationalDistribution` | Verifies 8 treaties covered (7 with 2 policies, 1 with 1 policy, cluster charter exempt). | **PASS** |
| `FoundryTreatyConsequenceExpansionTests` | `RepresentativePolicy_SalineCorridorMetAndMissed` | Checks `treaty_flotilla_saline_corridor_concordat` met (+3 standing, -fuel, -water) and missed (-5 standing, +fuel). | **PASS** |
| `FoundryTreatyConsequenceExpansionTests` | `RepresentativePolicy_SwitchbackMetAndViolated` | Checks `treaty_switchback_fuel_and_passage_accord` met (+4 standing, -fuel) and violated (-10 standing, +fuel). | **PASS** |
| `FoundryTreatyConsequenceExpansionTests` | `RepresentativePolicy_AquiferProtectionMetAndViolated` | Checks `treaty_deep_coast_aquifer_protection_treaty` met (+3 standing, -water) and violated (-10 standing, +water, +filter). | **PASS** |
| `FoundryTreatyConsequenceExpansionTests` | `RepresentativePolicy_GrainTitheMetAndViolated` | Checks `treaty_garrison_grain_tithe_compact` met (+4 standing, -food) and violated (-12 standing, +food, +fuel). | **PASS** |
| `FoundryTreatyConsequenceExpansionTests` | `RepresentativePolicy_FairTradeMet` | Checks `treaty_scale_suburban_fair_trade_convention` met (+3 standing, -scrap). | **PASS** |
| `FoundryTreatyConsequenceExpansionTests` | `Idempotency_RecordStateTracksAssessmentDayCycleKey` | Verifies `IsApplied` deduplication key by cycle day. | **PASS** |
| `FoundryTreatyConsequenceExpansionTests` | `Balance_StandingDeltasAreBoundedAndProportional` | Tests standing delta bounds: met in [1, 5], missed in [-7, -4], violated in [-16, -7]. | **PASS** |

---

## 2. Host Integration Verification

- `godot --headless --path . -- --silent-foundry-selftest`: 26/26 passed.
- `godot --headless --path . -- --data-integrity-selftest`: 215 catalogs verified, 0 errors.
- `godot --headless --path . -- --content-utilization-selftest`: CI gate PASS.
- `godot --headless --path . -- --scene-binding-selftest`: 22/22 passed.
- `python3 scripts/ci/scene-lint.py`: 27 production scenes clean, 0 errors.
