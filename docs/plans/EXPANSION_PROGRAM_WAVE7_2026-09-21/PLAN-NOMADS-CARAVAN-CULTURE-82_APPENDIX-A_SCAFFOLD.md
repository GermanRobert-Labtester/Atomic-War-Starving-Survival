# PLAN-NOMADS-CARAVAN-CULTURE-82 — Appendix A: Implementation Scaffold

**Generated:** 2026-09-21 by the session scaffold generator (paired variant, host-inclusive scan).
**Scaffolding authority:** [`PLAN-FACTION-BRANCH-TRUTH-171`](../EXPANSION_PROGRAM_WAVE13_2026-09-21/PLAN-FACTION-BRANCH-TRUTH-171.md) — this plan adopts the authority's patterns (below) rather than inventing a second architecture. **Convention authority:** `PLAN-ORPHAN-SEAL-01` (generated, reproducible appendices).
**Status:** scaffolding only — no production file is created by this appendix.

## 1. Advisory patterns from the scaffolding authority

| Authority package | Pattern to adopt |
|---|---|
| `FBT-171A` | branch model + inheritance manifest. |
| `FBT-171B` | split partition rules + no-double-count test. |
| `FBT-171C` | treaty/obligation resolution table + one fixture per outcome. |

## 2. Source inventory (64 files, Core + host)

| File | Lines |
|---|---:|
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
| `Economy/SurvivorBarterSystem.cs` | 666 |
| `Economy/TradeCreditCoordinator.cs` | 277 |
| `Economy/TradeEmbargoSystem.cs` | 709 |
| `Economy/TradeRouteContract.cs` | 245 |
| `Economy/TradeRouteMonopolyEngine.cs` | 206 |
| `Economy/TradeRouteRiskBindingEngine.cs` | 130 |
| `Economy/TradeScreenPresenter.cs` | 468 |
| `Economy/TradeScreenScenarios.cs` | 287 |
| `Economy/TradeScreenSeam.cs` | 369 |

… and 19 more.

## 3. Data bindings

| Catalog | Records/shape |
|---|---|
| `caravan_trade_routes.json` | object(2 keys) |
| `caravans.json` | object(2 keys) |
| `merchant_caravans.json` | object(2 keys) |
| `visitor_templates.json` | object(2 keys) |
| `wasteland_trade_caravan_routes.json` | object(3 keys) |

## 4. Host attachment

- Candidate host partials: none — new attachment or headless-only
- Proposed method names: `SetupEconomyNCScaffold` / `SaveEconomyNCScaffold` (Plan 1 Appendix AI: collision-free)
- Loop/day coupling: consult Plan 1 Appendix AA before adding any step method; no new timer.

## 5. Test scaffold (proposed, not created)

Proposed file: `Ashfall.Core.Tests/Economy/NC82ScaffoldTests.cs`

```csharp
// One fixture per package, plus one authority-conformance fixture.
public class NC82ScaffoldTests
{
    [Fact] public void NC82A_TODO() { /* camp host: arrival, duration, needs, departure; hosting costs and goodwill. */ }
    [Fact] public void NC82B_TODO() { /* fairs: calendar windows with trade price effects and contract offers. */ }
    [Fact] public void NC82C_TODO() { /* passage policy: toll/refuse/grant with standing and route consequences. */ }
    [Fact] public void NC82D_TODO() { /* customs/friction: respectful/offensive interactions with authored, fictional cus */ }
    [Fact] public void NC82E_TODO() { /* seasonal prediction: route/arrival forecast from migration engines; expedition w */ }
    [Fact] public void NC82F_TODO() { /* news exchange: rumors/intel via the rumor owner (Plan 203 briefing). */ }
    [Fact] public void NC82_AuthorityConformance_TODO() { /* pattern parity with PLAN-FACTION-BRANCH-TRUTH-171 */ }
}}
```

## 6. Commands

- `bash scripts/run_test.sh Ashfall.Core.Tests/Economy/`
- Regenerate, do not edit (Plan 1 Appendices AJ/AM).
- `python3 scripts/ci/generate-docs-index.py --check` if docs change.

## 7. Scaffolding rules

1. No production file before a claim.
2. The authority's patterns win over new ones; a deviation is recorded with a reason.
3. Every fixture maps to a package; a package without a fixture is a finding.
