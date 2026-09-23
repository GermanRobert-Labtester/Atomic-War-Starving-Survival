# PLAN-SPATIAL-SIM-AUTHORITY-95 — Appendix A: Implementation Scaffold

**Generated:** 2026-09-21 by the session scaffold generator (paired variant, batch 3).
**Scaffolding authority:** [`PLAN-WAYSTATION-NETWORK-TRUTH-153`](../EXPANSION_PROGRAM_WAVE12_2026-09-21/PLAN-WAYSTATION-NETWORK-TRUTH-153.md) — adopt the authority's patterns rather than inventing a second architecture. **Convention authority:** `PLAN-ORPHAN-SEAL-01` (generated, reproducible appendices).
**Status:** scaffolding only — no production file is created by this appendix.

## 1. Advisory patterns from the scaffolding authority

| Authority package | Pattern to adopt |
|---|---|
| `WNT-153A` | node model + service table (catalog-backed). |
| `WNT-153B` | network attachment to Plan 95 route edges + no-orphan-node check. |
| `WNT-153C` | status transition table + event/day tests. |

## 2. Source inventory (69 files, Core + host)

| File | Lines |
|---|---:|
| `Campaign/BriefingRouteMap.cs` | 130 |
| `Economy/BlackMarketSettlementService.cs` | 373 |
| `Economy/CaravanTradeRouteCatalog.cs` | 68 |
| `Economy/RegionalSupplyRouter.cs` | 275 |
| `Economy/TradeRouteContract.cs` | 245 |
| `Economy/TradeRouteMonopolyEngine.cs` | 206 |
| `Economy/TradeRouteRiskBindingEngine.cs` | 130 |
| `Factions/EspionageConsequenceRouter.cs` | 120 |
| `Factions/TerritoryControlSystem.cs` | 451 |
| `Settlements/OutpostSettlementSystem.cs` | 318 |
| `World/AnomalyHazardCatalog.cs` | 249 |
| `World/AnomalyHazardSystem.cs` | 654 |
| `World/AtmosphericSoundingCatalog.cs` | 89 |
| `World/CargoAirdropSystem.cs` | 453 |
| `World/CloudSeedingSystem.cs` | 354 |
| `World/CommsArraySystem.cs` | 346 |
| `World/DamagedMapCatalog.cs` | 317 |
| `World/DamagedMapSystem.cs` | 205 |
| `World/DebtRouteAccessResolver.cs` | 35 |
| `World/FactionTerritoryCatalog.cs` | 155 |
| `World/FalloutSystem.cs` | 369 |
| `World/FieldGuideCatalog.cs` | 199 |
| `World/GeodeticSurveyEngine.cs` | 495 |
| `World/GroundPenetratingRadarCatalog.cs` | 163 |
| `World/GroundPenetratingRadarEngine.cs` | 227 |
| `World/InSarDeformationEngine.cs` | 530 |
| `World/LivingMapRouteProjection.cs` | 51 |
| `World/MapRouteHazardEvaluator.cs` | 96 |
| `World/ModalTravelDispatchEngine.cs` | 200 |
| `World/NightWatchPatrolReadinessEngine.cs` | 279 |
| `World/PatrolTerritoryAuthority.cs` | 259 |
| `World/RouteAvailabilityKind.cs` | 33 |
| `World/RouteAvailabilityPresentation.cs` | 25 |
| `World/RouteGateContextResolver.cs` | 292 |
| `World/RouteInfrastructureSystem.cs` | 373 |
| `World/RouteRegionTopology.cs` | 149 |
| `World/SeasonalEventCatalog.cs` | 67 |
| `World/SeasonalEventSystem.cs` | 200 |
| `World/SettlementCatalog.cs` | 464 |
| `World/StormForecastReadinessEngine.cs` | 276 |

… and 29 more.

## 3. Data bindings

| Catalog | Records/shape |
|---|---|
| `faction_territory.json` | object(4 keys) |
| `wasteland_settlement_npcs.json` | object(3 keys) |
| `caravan_trade_routes.json` | object(2 keys) |
| `weather_route_gates.json` | object(2 keys) |
| `settlements.json` | object(3 keys) |
| `expedition_route_waypoint_notes_batch_2.json` | object(4 keys) |

## 4. Host attachment

- Candidate host partials: none — new attachment or headless-only
- Proposed method names: `SetupWorldSSScaffold` / `SaveWorldSSScaffold` (Plan 1 Appendix AI: collision-free)
- Loop/day coupling: consult Plan 1 Appendix AA before adding any step method.

## 5. Test scaffold (proposed, not created)

Proposed file: `Ashfall.Core.Tests/World/SS95ScaffoldTests.cs`

```csharp
// One fixture per package, plus one authority-conformance fixture.
public class SS95ScaffoldTests
{
    [Fact] public void SPA95A_TODO() { /* authority map doc + row-per-fact table (verified against source). */ }
    [Fact] public void SPA95B_TODO() { /* grid/cell semantics doc with the owning save section per mutable field. */ }
    [Fact] public void SPA95C_TODO() { /* placement determinism test: two same-seed runs compare placed-site sets and rout */ }
    [Fact] public void SPA95D_TODO() { /* orphan linkage notes for the five engines (feeds Plan 1 packages). */ }
    [Fact] public void SPA95E_TODO() { /* read-model guard: a test/scan that projection types are not referenced from simu */ }
    [Fact] public void SS95_AuthorityConformance_TODO() { /* pattern parity with PLAN-WAYSTATION-NETWORK-TRUTH-153 */ }
}}
```

## 6. Commands

- `bash scripts/run_test.sh Ashfall.Core.Tests/World/`
- Regenerate, do not edit (Plan 1 Appendices AJ/AM).
- `python3 scripts/ci/generate-docs-index.py --check` if docs change.

## 7. Scaffolding rules

1. No production file before a claim.
2. The authority's patterns win; a deviation is recorded with a reason.
3. Every fixture maps to a package; a package without a fixture is a finding.
