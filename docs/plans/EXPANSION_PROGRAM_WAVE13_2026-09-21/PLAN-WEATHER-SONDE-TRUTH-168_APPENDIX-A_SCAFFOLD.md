# PLAN-WEATHER-SONDE-TRUTH-168 — Appendix A: Implementation Scaffold

**Generated:** 2026-09-21 by the session scaffold generator (paired variant, batch 3).
**Scaffolding authority:** [`PLAN-WEATHER-INTELLIGENCE-TRUTH-218`](../EXPANSION_PROGRAM_WAVE16_2026-09-21/PLAN-WEATHER-INTELLIGENCE-TRUTH-218.md) — adopt the authority's patterns rather than inventing a second architecture. **Convention authority:** `PLAN-ORPHAN-SEAL-01` (generated, reproducible appendices).
**Status:** scaffolding only — no production file is created by this appendix.

## 1. Advisory patterns from the scaffolding authority

| Authority package | Pattern to adopt |
|---|---|
| `WIT-218A` | fusion rule + weighting table. |
| `WIT-218B` | confidence-band tests (degraded source → lower confidence). |
| `WIT-218C` | hardening cost/damage-reduction fixtures per structure class. |

## 2. Source inventory (59 files, Core + host)

| File | Lines |
|---|---:|
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
| `World/TravelGraphKnowledgeGate.cs` | 136 |
| `World/WastelandMapCatalogLoader.cs` | 383 |
| `World/WastelandMapSystem.cs` | 1265 |
| `World/WeatherAtmosphereMap.cs` | 117 |
| `World/WeatherEffectsCatalog.cs` | 199 |
| `World/WeatherForecastReliabilityEngine.cs` | 146 |
| `World/WeatherGate.cs` | 94 |
| `World/WeatherGateCatalog.cs` | 194 |
| `World/WeatherGateCatalogLoader.cs` | 193 |
| `World/WeatherGateContextEvaluator.cs` | 228 |

… and 19 more.

## 3. Data bindings

No catalog name-matched; verify paths/loaders before claiming a data dependency.

## 4. Host attachment

- Candidate host partials: none — new attachment or headless-only
- Proposed method names: `SetupWorldWSScaffold` / `SaveWorldWSScaffold` (Plan 1 Appendix AI: collision-free)
- Loop/day coupling: consult Plan 1 Appendix AA before adding any step method.

## 5. Test scaffold (proposed, not created)

Proposed file: `Ashfall.Core.Tests/World/WS168ScaffoldTests.cs`

```csharp
// One fixture per package, plus one authority-conformance fixture.
public class WS168ScaffoldTests
{
    [Fact] public void WST168A_TODO() { /* instrument model + condition/calibration table. */ }
    [Fact] public void WST168B_TODO() { /* measurement error-band tests (condition vs band). */ }
    [Fact] public void WST168C_TODO() { /* forecast derivation + lead-time fixture per condition class. */ }
    [Fact] public void WST168D_TODO() { /* no-sonde fallback test (reduced lead time, never perfect). */ }
    [Fact] public void WST168E_TODO() { /* save round-trip; no re-measure on load. */ }
    [Fact] public void WS168_AuthorityConformance_TODO() { /* pattern parity with PLAN-WEATHER-INTELLIGENCE-TRUTH-218 */ }
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
