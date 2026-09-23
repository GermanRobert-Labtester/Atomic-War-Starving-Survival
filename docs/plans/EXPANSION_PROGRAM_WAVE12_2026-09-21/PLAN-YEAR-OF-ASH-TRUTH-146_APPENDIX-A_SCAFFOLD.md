# PLAN-YEAR-OF-ASH-TRUTH-146 — Appendix A: Implementation Scaffold

**Generated:** 2026-09-21 by the session scaffold generator (paired variant, batch 3).
**Scaffolding authority:** [`PLAN-WEATHER-ATMOSPHERE-28`](../EXPANSION_PROGRAM_WAVE3_2026-09-21/PLAN-WEATHER-ATMOSPHERE-28.md) — adopt the authority's patterns rather than inventing a second architecture. **Convention authority:** `PLAN-ORPHAN-SEAL-01` (generated, reproducible appendices).
**Status:** scaffolding only — no production file is created by this appendix.

## 1. Advisory patterns from the scaffolding authority

| Authority package | Pattern to adopt |
|---|---|

## 2. Source inventory (78 files, Core + host)

| File | Lines |
|---|---:|
| `ApprenticeshipSystem.cs` | 496 |
| `Campaign/SliceScenario.cs` | 227 |
| `Economy/BlackMarketSettlementService.cs` | 373 |
| `Economy/RegionalPriceAtlas.cs` | 346 |
| `Education/ApprenticeshipCurriculumEngine.cs` | 219 |
| `Expeditions/EncounterChoiceResolver.cs` | 133 |
| `Factions/WeightOfChoicesSave.cs` | 189 |
| `Feedback/FeedbackService.cs` | 129 |
| `Feedback/IFeedbackService.cs` | 22 |
| `Foundry/HydraulicExtrusionEngine.cs` | 377 |
| `IceRoadHeadlessDemo.cs` | 138 |
| `IceRoadSystem.cs` | 458 |
| `Inventory/DeviceState.cs` | 136 |
| `Journal/JournalVoice.cs` | 59 |
| `Journal/JournalVoiceProseCatalog.cs` | 150 |
| `Localization/LocalizationService.cs` | 595 |
| `Medical/MicrofluidicDiagnosticEngine.cs` | 609 |
| `MoralChoice/MoralChoiceBranchQuestCatalogLoader.cs` | 72 |
| `MoralChoice/MoralChoiceCatalogLoader.cs` | 125 |
| `MoralChoice/MoralChoiceChainCatalogLoader.cs` | 189 |
| `MoralChoice/MoralChoiceChainData.cs` | 78 |
| `MoralChoice/MoralChoiceExpansionQuestCatalogLoader.cs` | 71 |
| `MoralChoice/MoralChoiceFactionReactionsCatalogLoader.cs` | 96 |
| `MoralChoice/MoralChoiceFactionReactionsData.cs` | 34 |
| `MoralChoice/MoralChoiceFlagCatalogLoader.cs` | 60 |
| `MoralChoice/MoralChoiceFlagDefinitions.cs` | 21 |
| `MoralChoice/MoralChoiceGossipCatalogLoader.cs` | 145 |
| `MoralChoice/MoralChoiceGossipData.cs` | 59 |
| `MoralChoice/MoralChoiceGossipRuntime.cs` | 185 |
| `MoralChoice/MoralChoiceIds.cs` | 260 |
| `MoralChoice/MoralChoiceState.cs` | 77 |
| `MoralChoice/MoralChoiceSystem.cs` | 688 |
| `Narrative/EncounterChoiceEffectDispatcher.cs` | 95 |
| `Narrative/JusticeSystem.cs` | 445 |
| `Save/SaveSlotService.cs` | 1294 |
| `Survivors/SurvivorEnrichmentService.cs` | 479 |
| `Voice/SurvivorVoiceSystem.cs` | 316 |
| `Voice/VoiceLineDispatchCoordinator.cs` | 253 |
| `Voice/VoiceLineSelectionEngine.cs` | 185 |
| `World/StormForecastReadinessEngine.cs` | 276 |

… and 38 more.

## 3. Data bindings

| Catalog | Records/shape |
|---|---|
| `apprenticeship_catalog.json` | object(2 keys) |
| `journal_voice_prose.json` | object(2 keys) |
| `moral_choice_faction_reactions.json` | object(3 keys) |
| `year_of_ash_storm_windows.json` | 14 |
| `moral_choice_chains.json` | object(9 keys) |
| `moral_choice_flags.json` | object(3 keys) |

## 4. Host attachment

- Candidate host partials: `Main.HydraulicExtrusion.cs`, `Main.MoralChoice.cs`, `Main.CampaignServices.cs`, `Main.YearOfAsh.cs`
- Proposed method names: `SetupYearOfAshYAScaffold` / `SaveYearOfAshYAScaffold` (Plan 1 Appendix AI: collision-free)
- Loop/day coupling: consult Plan 1 Appendix AA before adding any step method.

## 5. Test scaffold (proposed, not created)

Proposed file: `Ashfall.Core.Tests/YearOfAsh/YA146ScaffoldTests.cs`

```csharp
// One fixture per package, plus one authority-conformance fixture.
public class YA146ScaffoldTests
{
    [Fact] public void YAT146A_TODO() { /* timeline model + phase boundary table. */ }
    [Fact] public void YAT146B_TODO() { /* arc contracts (onset/offset/effects) + one scripted arc test each. */ }
    [Fact] public void YAT146C_TODO() { /* war chain stage gates + catalog resolution + stall fallback. */ }
    [Fact] public void YAT146D_TODO() { /* door encounter selection determinism + whitelist integration. */ }
    [Fact] public void YAT146E_TODO() { /* ladder-4 save round-trip mid-arc. */ }
    [Fact] public void YA146_AuthorityConformance_TODO() { /* pattern parity with PLAN-WEATHER-ATMOSPHERE-28 */ }
}}
```

## 6. Commands

- `bash scripts/run_test.sh Ashfall.Core.Tests/YearOfAsh/` (create the region if absent)
- Regenerate, do not edit (Plan 1 Appendices AJ/AM).
- `python3 scripts/ci/generate-docs-index.py --check` if docs change.

## 7. Scaffolding rules

1. No production file before a claim.
2. The authority's patterns win; a deviation is recorded with a reason.
3. Every fixture maps to a package; a package without a fixture is a finding.
