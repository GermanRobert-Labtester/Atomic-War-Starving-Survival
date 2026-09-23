# PLAN-ECONOMY-LEDGER-TRUTH-96 — Appendix A: Implementation Scaffold

**Generated:** 2026-09-21 by the session scaffold generator (paired variant, batch 3).
**Scaffolding authority:** [`PLAN-ECONOMY-DATA-FAMILY-TRUTH-270`](../EXPANSION_PROGRAM_WAVE19_2026-09-21/PLAN-ECONOMY-DATA-FAMILY-TRUTH-270.md) — adopt the authority's patterns rather than inventing a second architecture. **Convention authority:** `PLAN-ORPHAN-SEAL-01` (generated, reproducible appendices).
**Status:** scaffolding only — no production file is created by this appendix.

## 1. Advisory patterns from the scaffolding authority

| Authority package | Pattern to adopt |
|---|---|
| `EDF-270A` | type-consumption table + dead report. |
| `EDF-270B` | catalog→loader tests. |
| `EDF-270C` | event-result routing tests. |

## 2. Source inventory (74 files, Core + host)

| File | Lines |
|---|---:|
| `DoseLedgerSave.cs` | 164 |
| `DoseLedgerSystem.cs` | 333 |
| `DutyRoster/DutyHourLedger.cs` | 126 |
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

… and 34 more.

## 3. Data bindings

| Catalog | Records/shape |
|---|---|
| `ledger_debt_templates.json` | object(3 keys) |
| `black_market_inventory.json` | 7 |
| `regional_prices.json` | 24 |
| `bunker_trade_ledger_batch_2.json` | object(4 keys) |
| `trade_ledgers_expansion.json` | object(4 keys) |

## 4. Host attachment

- Candidate host partials: `Main.BlackMarket.cs`, `Main.DebtCredit.cs`
- Proposed method names: `SetupEconomyELScaffold` / `SaveEconomyELScaffold` (Plan 1 Appendix AI: collision-free)
- Loop/day coupling: consult Plan 1 Appendix AA before adding any step method.

## 5. Test scaffold (proposed, not created)

Proposed file: `Ashfall.Core.Tests/Economy/EL96ScaffoldTests.cs`

```csharp
// One fixture per package, plus one authority-conformance fixture.
public class EL96ScaffoldTests
{
    [Fact] public void ELT96A_TODO() { /* ledger map doc (owner per fact; no code). */ }
    [Fact] public void ELT96B_TODO() { /* reconciliation wrapper + snapshot helper (test-only). */ }
    [Fact] public void ELT96C_TODO() { /* scripted trade scenarios: market order, barter, caravan leg, black-market lot, l */ }
    [Fact] public void ELT96D_TODO() { /* orphan linkage notes feeding Plan 1 (and Plan 44's appendix). */ }
    [Fact] public void ELT96E_TODO() { /* mismatch report format: fact, expected, actual, owner file. */ }
    [Fact] public void EL96_AuthorityConformance_TODO() { /* pattern parity with PLAN-ECONOMY-DATA-FAMILY-TRUTH-270 */ }
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
