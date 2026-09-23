# PLAN-LABOUR-PROFESSIONS-68 — Appendix A: Implementation Scaffold

**Generated:** 2026-09-21 by the session scaffold generator. Built with the two
session toolkit plans as its authority: the integration pattern set of
**PLAN-INTEGRATION-KIT-02** (host session, save store, day owner, panel route,
CLI probe, focused fixture) and the appendix/maintenance convention of
**PLAN-ORPHAN-SEAL-01** (generated, reproducible, never hand-edited).
**Status:** scaffolding only — no production file is created by this appendix.
A claim still owns the edits; this page exists so the claim starts from real
paths, real names, and a fixture skeleton derived from this plan's own packages.

## 1. Source inventory (136 files)

| File | Lines |
|---|---:|
| `DutyRoster/DutyHourLedger.cs` | 126 |
| `DutyRoster/DutyRosterAssignmentEngine.cs` | 320 |
| `DutyRoster/DutyRosterCatalog.cs` | 227 |
| `DutyRoster/DutyRosterChartEngine.cs` | 298 |
| `DutyRoster/DutyRosterHeadlessDemo.cs` | 153 |
| `DutyRoster/DutyRosterHoldfastBridge.cs` | 259 |
| `DutyRoster/DutyRosterIds.cs` | 129 |
| `DutyRoster/DutyRosterOverflowEngine.cs` | 92 |
| `DutyRoster/DutyRosterQuestRuntime.cs` | 484 |
| `DutyRoster/DutyRosterSave.cs` | 229 |
| `DutyRoster/DutyRosterSystem.cs` | 925 |
| `DutyRoster/MoraleMarkSystem.cs` | 176 |
| `DutyRoster/ShelterEncounterSystem.cs` | 394 |
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

… and 96 more.

## 2. Data bindings

| Catalog | Records/shape |
|---|---|
| `duty_roster_locations.json` | object(2 keys) |
| `duty_roster_marks.json` | 43 |
| `duty_roster_quests.json` | object(2 keys) |
| `labor_camps.json` | object(2 keys) |
| `duty_roster_seasons.json` | 8 |
| `duty_roles.json` | object(6 keys) |

## 3. Host attachment

- Candidate host partials: `Main.DutyRoster.cs`, `Main.UiTests.DutyRoster.cs`
- Proposed setup method: `SetupDutyRosterScaffold` · proposed save method: `SaveDutyRosterScaffold` (checked against the 228 existing setup names in Plan 1 Appendix AI — extend the real owner name if one exists)
- Loop coupling: see Plan 1 Appendix AA before adding any day/hour step; a new timer is forbidden.

## 4. Test scaffold (proposed, not created)

Proposed file: `Ashfall.Core.Tests/DutyRoster/LP68ScaffoldTests.cs`

```csharp
// Scaffold skeleton — derived from this plan's packages below.
// Fixtures assert one package each; replace the TODO bodies at claim time.
public class LP68ScaffoldTests
{
    [Fact] public void LB68A_TODO() { /* grievance model: causes from fairness, danger, overwork, favouritism; visible an */ }
    [Fact] public void LB68B_TODO() { /* negotiation: policy decisions (hours, rest, perks, safety) with costs and effect */ }
    [Fact] public void LB68C_TODO() { /* refusal/strike: staged escalation (complaint → refusal → strike), mediation opti */ }
    [Fact] public void LB68D_TODO() { /* craft pride: mastery grants quality/teaching bonuses; a master's departure costs */ }
    [Fact] public void LB68E_TODO() { /* safety enforcement: hazard class policy reduces injuries but costs time; violati */ }
    [Fact] public void LB68F_TODO() { /* records: labour history in the archive; strikes/settlements become chronicle ent */ }
}
```

## 5. Commands

- `bash scripts/run_test.sh Ashfall.Core.Tests/DutyRoster/`
- Reachability/structure checks: re-run Plan 1 Appendix generators after the claim lands (Appendix AJ lists triggers).
- `python3 scripts/ci/generate-docs-index.py --check` if docs change.

## 6. Scaffolding rules

1. No production file is created before a claim; this appendix is documentation.
2. Names adopt existing conventions where an owner exists (Plan 1 Appendices AH/AI).
3. Every fixture above maps to a package in the parent plan; a package without a fixture is a finding.
4. The scaffold is regenerated, not edited — see Appendix AM.
