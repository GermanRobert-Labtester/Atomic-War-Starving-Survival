# Hardcore Regression Matrix

## 1. Test Suite Coverage & Regressions Guarded

| Test Class | Test Method | Target Invariant Verified | Status |
|:---|:---|:---|:---:|
| `HardcoreEconomyTuningExpansionTests` | `AuthoritativeCatalog_LoadsSuccessfully` | 8 tiers, 8 factions, 6 shocks loaded from disk | PASS |
| `HardcoreEconomyTuningExpansionTests` | `ScarcityTiers_ExactEightTiers_AndBaselinesPreserved` | Exact 8 tiers; baseline Critical and High preserved | PASS |
| `HardcoreEconomyTuningExpansionTests` | `ScarcityTiers_FullCampaignDayCoverage_AcrossAllTiers` | Continuous timeline coverage from Day 1 to Day 500+ | PASS |
| `HardcoreEconomyTuningExpansionTests` | `MatchesItem_WildcardPrefix_MatchesCorrectly` | `ammo_*` and `*` match item patterns as intended | PASS |
| `HardcoreEconomyTuningExpansionTests` | `FactionPreferences_ExactEightUniqueFactions` | 8 unique canonical faction IDs verified | PASS |
| `HardcoreEconomyTuningExpansionTests` | `FactionPreferences_NoCollisionBetweenPremiumAndRefuses` | Zero overlap between premium and refused item lists | PASS |
| `HardcoreEconomyTuningExpansionTests` | `PriceShocks_ExactSixShocks_AndBaselinesPreserved` | 6 unique shocks; PlumePassing baseline preserved | PASS |
| `HardcoreEconomyTuningExpansionTests` | `PriceShocks_QueryWithinAndBeyondDuration` | Shocks active during duration, inactive when expired | PASS |
| `HardcoreEconomyTuningExpansionTests` | `Stacking_CombinedMultiplierRemainsBounded` | Compounded price multiplier bounded below 10.0x | PASS |
| `HardcoreEconomyTuningExpansionTests` | `NegativeFixture_InvalidTier_ReturnsFailure` | Unknown/unsupported tier string safely rejected | PASS |
| `HardcoreEconomyTuningExpansionTests` | `NegativeFixture_DuplicateFaction_ReturnsFailure` | Duplicate faction ID definition safely rejected | PASS |
| `HardcoreEconomyTuningExpansionTests` | `Persistence_OldSaveSimulation_OperatesSafely` | Old save compatibility and missing faction fallbacks | PASS |
| `Plan23FlotillaFactionDepthTests` | `TradePreference_FlotillaSalvageSpecialty_LoadsFromTuningAuthority` | Flotilla salvage preference preserved | PASS |
| `TradeThemeAndEconomyTests` | `HardcoreEconomyTuning_PriceShocks_LoadsRulesAndCalculatesMultipliers` | Dynamic price shocks and wildcard queries pass | PASS |
| `EconomySystemTests` | `Load_ValidJson_ReturnsSuccess` | Economy tuning loader regression suite passes | PASS |
| `EconomySystemTests` | `Overlay_AppliedBundle_ReturnsScarcityMultiplier` | Day and item gates operate correctly | PASS |
