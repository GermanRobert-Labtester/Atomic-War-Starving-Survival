# PLAN-TRADE-EMBARGO-TRUTH-166 — Appendix A: Implementation Scaffold

**Generated:** 2026-09-21 by the session scaffold generator (paired variant, batch 3).
**Scaffolding authority:** [`PLAN-ECONOMY-LEDGER-TRUTH-96`](../EXPANSION_PROGRAM_WAVE8_2026-09-21/PLAN-ECONOMY-LEDGER-TRUTH-96.md) — adopt the authority's patterns rather than inventing a second architecture. **Convention authority:** `PLAN-ORPHAN-SEAL-01` (generated, reproducible appendices).
**Status:** scaffolding only — no production file is created by this appendix.

## 1. Advisory patterns from the scaffolding authority

| Authority package | Pattern to adopt |
|---|---|
| `ELT-96A` | ledger map doc (owner per fact; no code). |
| `ELT-96B` | reconciliation wrapper + snapshot helper (test-only). |
| `ELT-96C` | scripted trade scenarios: market order, barter, caravan leg, black-market lot, loan repayment. |

## 2. Source inventory (49 files, Core + host)

| File | Lines |
|---|---:|
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
| `Economy/SurvivorBarterSystem.cs` | 666 |
| `Economy/TradeCreditCoordinator.cs` | 277 |
| `Economy/TradeEmbargoSystem.cs` | 709 |
| `Economy/TradeRouteContract.cs` | 245 |
| `Economy/TradeRouteMonopolyEngine.cs` | 206 |

… and 9 more.

## 3. Data bindings

| Catalog | Records/shape |
|---|---|
| `trade_embargoes.json` | object(4 keys) |

## 4. Host attachment

- Candidate host partials: none — new attachment or headless-only
- Proposed method names: `SetupEconomyTEScaffold` / `SaveEconomyTEScaffold` (Plan 1 Appendix AI: collision-free)
- Loop/day coupling: consult Plan 1 Appendix AA before adding any step method.

## 5. Test scaffold (proposed, not created)

Proposed file: `Ashfall.Core.Tests/Economy/TE166ScaffoldTests.cs`

```csharp
// One fixture per package, plus one authority-conformance fixture.
public class TE166ScaffoldTests
{
    [Fact] public void TET166A_TODO() { /* embargo state + scope table. */ }
    [Fact] public void TET166B_TODO() { /* enforcement path tests (passage/market/inspection). */ }
    [Fact] public void TET166C_TODO() { /* smuggling-pressure input to Plan 44 (no private score proof). */ }
    [Fact] public void TET166D_TODO() { /* price rows handed to Plan 96 + reconciliation check. */ }
    [Fact] public void TET166E_TODO() { /* day-boundary expiry with catch-up + save round-trip. */ }
    [Fact] public void TE166_AuthorityConformance_TODO() { /* pattern parity with PLAN-ECONOMY-LEDGER-TRUTH-96 */ }
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
