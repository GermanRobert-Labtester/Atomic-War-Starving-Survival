# PLAN-CARTOGRAPHY-LANDMARKS-70 — Appendix A: Implementation Scaffold

**Generated:** 2026-09-21 by the session scaffold generator. Built with the two
session toolkit plans as its authority: the integration pattern set of
**PLAN-INTEGRATION-KIT-02** (host session, save store, day owner, panel route,
CLI probe, focused fixture) and the appendix/maintenance convention of
**PLAN-ORPHAN-SEAL-01** (generated, reproducible, never hand-edited).
**Status:** scaffolding only — no production file is created by this appendix.
A claim still owns the edits; this page exists so the claim starts from real
paths, real names, and a fixture skeleton derived from this plan's own packages.

## 1. Source inventory (69 files)

| File | Lines |
|---|---:|
| `Campaign/BriefingRouteMap.cs` | 130 |
| `Economy/CaravanTradeRouteCatalog.cs` | 68 |
| `Economy/RegionalSupplyRouter.cs` | 275 |
| `Economy/TradeRouteContract.cs` | 245 |
| `Economy/TradeRouteMonopolyEngine.cs` | 206 |
| `Economy/TradeRouteRiskBindingEngine.cs` | 130 |
| `Exploration/CartographySystem.cs` | 468 |
| `Factions/EspionageConsequenceRouter.cs` | 120 |
| `LandmarkDegradationSystem.Live.cs` | 44 |
| `LandmarkDegradationSystem.cs` | 130 |
| `Narrative/WastelandCartographyCatalog.cs` | 239 |
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

… and 29 more.

## 2. Data bindings

| Catalog | Records/shape |
|---|---|
| `caravan_trade_routes.json` | object(2 keys) |
| `weather_route_gates.json` | object(2 keys) |
| `expedition_route_waypoint_notes_batch_2.json` | object(4 keys) |
| `scavenger_expedition_route_notes.json` | 8 |
| `wasteland_trade_caravan_routes.json` | object(3 keys) |

## 3. Host attachment

- Candidate host partials: none — new attachment or headless-only

- Proposed setup method: `SetupWorldScaffold` · proposed save method: `SaveWorldScaffold` (checked against the 228 existing setup names in Plan 1 Appendix AI — extend the real owner name if one exists)
- Loop coupling: see Plan 1 Appendix AA before adding any day/hour step; a new timer is forbidden.

## 4. Test scaffold (proposed, not created)

Proposed file: `Ashfall.Core.Tests/World/CL70ScaffoldTests.cs`

```csharp
// Scaffold skeleton — derived from this plan's packages below.
// Fixtures assert one package each; replace the TODO bodies at claim time.
public class CL70ScaffoldTests
{
    [Fact] public void CL70A_TODO() { /* survey depth: regions gain an accuracy/coverage value affecting route risk and e */ }
    [Fact] public void CL70B_TODO() { /* landmarks: authored landmark nodes with functions (resupply, shelter, signal, st */ }
    [Fact] public void CL70C_TODO() { /* damaged-map assembly: fragment puzzles reveal real nodes; false leads are author */ }
    [Fact] public void CL70D_TODO() { /* location evolution: sites change with war/weather/ecology; revisits differ deter */ }
    [Fact] public void CL70E_TODO() { /* naming/notation: player names appear on the map and in chronicle/archive entries */ }
    [Fact] public void CL70F_TODO() { /* sharing: publishing routes improves NPC/caravan behavior and standing; misdirect */ }
}
```

## 5. Commands

- `bash scripts/run_test.sh Ashfall.Core.Tests/World/`
- Reachability/structure checks: re-run Plan 1 Appendix generators after the claim lands (Appendix AJ lists triggers).
- `python3 scripts/ci/generate-docs-index.py --check` if docs change.

## 6. Scaffolding rules

1. No production file is created before a claim; this appendix is documentation.
2. Names adopt existing conventions where an owner exists (Plan 1 Appendices AH/AI).
3. Every fixture above maps to a package in the parent plan; a package without a fixture is a finding.
4. The scaffold is regenerated, not edited — see Appendix AM.
