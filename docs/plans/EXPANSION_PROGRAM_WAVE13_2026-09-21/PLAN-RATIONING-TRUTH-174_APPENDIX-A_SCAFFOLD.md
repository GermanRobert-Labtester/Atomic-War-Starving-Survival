# PLAN-RATIONING-TRUTH-174 — Appendix A: Implementation Scaffold

**Generated:** 2026-09-21 by the session scaffold generator (paired variant, batch 3).
**Scaffolding authority:** [`PLAN-FOOD-CUISINE-39`](../EXPANSION_PROGRAM_WAVE4_2026-09-21/PLAN-FOOD-CUISINE-39.md) — adopt the authority's patterns rather than inventing a second architecture. **Convention authority:** `PLAN-ORPHAN-SEAL-01` (generated, reproducible appendices).
**Status:** scaffolding only — no production file is created by this appendix.

## 1. Advisory patterns from the scaffolding authority

| Authority package | Pattern to adopt |
|---|---|

## 2. Source inventory (80 files, Core + host)

| File | Lines |
|---|---:|
| `Crossing/CrossingThirdonaryIntegration.cs` | 170 |
| `CrossingArbitrationHeadlessDemo.cs` | 166 |
| `CrossingArbitrationSystem.cs` | 490 |
| `CryogenicAirSeparationSystem.cs` | 275 |
| `DoseQuestMigration.cs` | 162 |
| `Economy/BiologicalTradeItem.cs` | 23 |
| `Economy/BlackMarketContrabandEngine.cs` | 267 |
| `Economy/BlackMarketHeatAttentionEngine.cs` | 289 |
| `Economy/BlackMarketInventoryCatalog.cs` | 358 |
| `Economy/BlackMarketSettlementService.cs` | 373 |
| `Economy/BlackMarketSystem.cs` | 893 |
| `Economy/CaravanAtomicTrader.cs` | 162 |
| `Economy/CaravanCatalogLoader.cs` | 143 |
| `Economy/CaravanTradeNetworkSystem.cs` | 670 |
| `Economy/CaravanTradeRouteCatalog.cs` | 68 |
| `Economy/ChitPurityAssayEngine.cs` | 228 |
| `Economy/CommodityBaselineCatalog.cs` | 252 |
| `Economy/EconomyHeadlessDemo.cs` | 103 |
| `Economy/EconomyMarketRumorRules.cs` | 41 |
| `Economy/EconomyWeatherShockRules.cs` | 56 |
| `Economy/FactionEventResults.cs` | 59 |
| `Economy/FactionStanceEngine.cs` | 174 |
| `Economy/FactionStanceTypes.cs` | 57 |
| `Economy/FundsLedger.cs` | 227 |
| `Economy/GoodsCatalog.cs` | 356 |
| `Economy/HardcoreEconomyEnums.cs` | 37 |
| `Economy/HardcoreEconomyTuning.cs` | 258 |
| `Economy/HardcoreEconomyTuningDto.cs` | 78 |
| `Economy/HardcoreEconomyTuningLoader.cs` | 148 |
| `Economy/IEconomyInterfaces.cs` | 45 |
| `Economy/LoanSharkEnforcerEngine.cs` | 442 |
| `Economy/MarketSystem.cs` | 1198 |
| `Economy/MercenarySystem.cs` | 358 |
| `Economy/MigrationConsequenceEngine.cs` | 144 |
| `Economy/RegionalPriceAtlas.cs` | 346 |
| `Economy/RegionalSupplyRouter.cs` | 275 |
| `Economy/ResourceRationingSystem.cs` | 631 |
| `Economy/RestockAllocationEngine.cs` | 245 |
| `Economy/SeasonalHumanMigrationEngine.cs` | 162 |
| `Economy/ShelterBarterSystem.cs` | 611 |

… and 40 more.

## 3. Data bindings

| Catalog | Records/shape |
|---|---|
| `desperation_events.json` | object(2 keys) |
| `cryogenic_air_separation.json` | object(3 keys) |
| `electrostatic_filtration_catalog.json` | object(2 keys) |
| `gpr_exploration_catalog.json` | object(5 keys) |
| `espionage_operations.json` | object(3 keys) |
| `shelter_celebrations.json` | object(4 keys) |

## 4. Host attachment

- Candidate host partials: none — new attachment or headless-only
- Proposed method names: `SetupEconomyRATScaffold` / `SaveEconomyRATScaffold` (Plan 1 Appendix AI: collision-free)
- Loop/day coupling: consult Plan 1 Appendix AA before adding any step method.

## 5. Test scaffold (proposed, not created)

Proposed file: `Ashfall.Core.Tests/Economy/RAT174ScaffoldTests.cs`

```csharp
// One fixture per package, plus one authority-conformance fixture.
public class RAT174ScaffoldTests
{
    [Fact] public void RAT174A_TODO() { /* tier model + assignment/exemption table. */ }
    [Fact] public void RAT174B_TODO() { /* fairness index computation + display-from-index test. */ }
    [Fact] public void RAT174C_TODO() { /* escalation steps + one fixture per step. */ }
    [Fact] public void RAT174D_TODO() { /* conservation check across a scripted shortage week. */ }
    [Fact] public void RAT174E_TODO() { /* save round-trip; no re-deal on load. */ }
    [Fact] public void RAT174_AuthorityConformance_TODO() { /* pattern parity with PLAN-FOOD-CUISINE-39 */ }
}}
```

## 6. Commands

- `bash scripts/run_test.sh Ashfall.Core.Tests/Economy/`
- Regenerate, do not edit (Plan 1 Appendices AJ/AM).
- `python3 scripts/ci/generate-docs-index.py --check` if docs change.

## 7. Scaffolding rules

1. No production file before a claim.
2. The authority's patterns win; a deviation is recorded with a reason.
3. Every fixture maps to a package; a package without a fixture is a finding.
