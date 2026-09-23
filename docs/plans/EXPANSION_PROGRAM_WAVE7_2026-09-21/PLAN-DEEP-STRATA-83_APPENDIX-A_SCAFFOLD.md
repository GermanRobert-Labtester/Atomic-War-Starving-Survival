# PLAN-DEEP-STRATA-83 — Appendix A: Implementation Scaffold

**Generated:** 2026-09-21 by the session scaffold generator (paired variant, host-inclusive scan).
**Scaffolding authority:** [`PLAN-AQUIFER-MONITORING-TRUTH-164`](../EXPANSION_PROGRAM_WAVE13_2026-09-21/PLAN-AQUIFER-MONITORING-TRUTH-164.md) — this plan adopts the authority's patterns (below) rather than inventing a second architecture. **Convention authority:** `PLAN-ORPHAN-SEAL-01` (generated, reproducible appendices).
**Status:** scaffolding only — no production file is created by this appendix.

## 1. Advisory patterns from the scaffolding authority

| Authority package | Pattern to adopt |
|---|---|
| `AQM-164A` | aquifer state model + recharge/drawdown table. |
| `AQM-164B` | piezometer reading fidelity test (engine value = stored value). |
| `AQM-164C` | sustainability thresholds + yield-drop/dry-well fixtures. |

## 2. Source inventory (106 files, Core + host)

| File | Lines |
|---|---:|
| `DeepWellSystem.cs` | 247 |
| `Excavation/ExcavationCatalogLoader.cs` | 273 |
| `Excavation/ExcavationHazardSystem.cs` | 483 |
| `Excavation/SubterraneanSubsidenceEngine.cs` | 299 |
| `Narrative/GeologicalStrataCatalog.cs` | 116 |
| `Shelter/AeroponicsSystem.cs` | 528 |
| `Shelter/AquaponicsSystem.cs` | 813 |
| `Shelter/AquiferPiezometerEngine.cs` | 829 |
| `Shelter/BioFermentationEngine.cs` | 786 |
| `Shelter/CaptiveInterrogationCatalog.cs` | 97 |
| `Shelter/CarbonCompositeCatalog.cs` | 163 |
| `Shelter/CarbonCompositeEngine.cs` | 237 |
| `Shelter/CascadeCoordinator.cs` | 212 |
| `Shelter/CascadeRuleCatalog.cs` | 169 |
| `Shelter/CellulosicBiofuelCatalog.cs` | 108 |
| `Shelter/ChemicalReagentSynthesisEngine.cs` | 276 |
| `Shelter/ChlorAlkaliSynthesisEngine.cs` | 400 |
| `Shelter/CryoVaultSystem.cs` | 554 |
| `Shelter/CupolaFoundryCatalog.cs` | 167 |
| `Shelter/CupolaFoundryEngine.cs` | 445 |
| `Shelter/CvdDiamondCatalog.cs` | 350 |
| `Shelter/CvdDiamondSynthesisEngine.cs` | 527 |
| `Shelter/DisasterResponseSystem.cs` | 457 |
| `Shelter/EbPvdCoatingCatalogLoader.cs` | 176 |
| `Shelter/EbPvdCoatingEngine.cs` | 537 |
| `Shelter/EmergencyMusterReadinessEngine.cs` | 192 |
| `Shelter/FischerTropschCatalog.cs` | 197 |
| `Shelter/FischerTropschSynthesisEngine.cs` | 396 |
| `Shelter/FluidDeliveryApplicator.cs` | 137 |
| `Shelter/FluidLogisticsSystem.cs` | 695 |
| `Shelter/FluidWaterTreatmentBridge.cs` | 46 |
| `Shelter/FogHarvestingCatalog.cs` | 84 |
| `Shelter/FoodPreservationCatalog.cs` | 140 |
| `Shelter/FoodPreservationSystem.cs` | 327 |
| `Shelter/GeothermalAquiferHeadlessDemo.cs` | 119 |
| `Shelter/GeothermalAquiferState.cs` | 27 |
| `Shelter/GeothermalAquiferSystem.cs` | 303 |
| `Shelter/GeothermalCatalog.cs` | 58 |
| `Shelter/GeothermalCatalogLoader.cs` | 34 |
| `Shelter/GeothermalOrcSystem.cs` | 556 |
| `Shelter/HydroponicBiomeSystem.cs` | 441 |
| `Shelter/HydroponicCropCatalog.cs` | 149 |
| `Shelter/KilnFiringEngine.cs` | 241 |
| `Shelter/KineticStorageSystem.cs` | 615 |
| `Shelter/MaterialShieldingSystem.cs` | 109 |

… and 61 more.

## 3. Data bindings

| Catalog | Records/shape |
|---|---|
| `geothermal_drilling_depths.json` | object(2 keys) |
| `geothermal_strata_catalog.json` | object(2 keys) |
| `subterranean_zones.json` | object(3 keys) |
| `underground_tunnels.json` | object(3 keys) |
| `geological_strata_logs.json` | object(3 keys) |
| `geothermal_borehole_logs.json` | 7 |

## 4. Host attachment

- Candidate host partials: `Main.DeepWell.cs`, `Main.Subterranean.cs`
- Proposed method names: `SetupShelterDSScaffold` / `SaveShelterDSScaffold` (Plan 1 Appendix AI: collision-free)
- Loop/day coupling: consult Plan 1 Appendix AA before adding any step method; no new timer.

## 5. Test scaffold (proposed, not created)

Proposed file: `Ashfall.Core.Tests/Shelter/DS83ScaffoldTests.cs`

```csharp
// One fixture per package, plus one authority-conformance fixture.
public class DS83ScaffoldTests
{
    [Fact] public void DS83A_TODO() { /* shaft/level model: depth bands, shoring requirements, lift logistics, stability. */ }
    [Fact] public void DS83B_TODO() { /* strata survey+extraction: yields from authored strata; GPR reveals seams. */ }
    [Fact] public void DS83C_TODO() { /* hazard cycle: methane accumulation, flooding, subsidence with mitigation protoco */ }
    [Fact] public void DS83D_TODO() { /* deep water/geothermal: yield vs hazard trade; plant builds heat/power. */ }
    [Fact] public void DS83E_TODO() { /* hidden sites: deep-lore exploration outcomes (relics, anomalies, records). */ }
    [Fact] public void DS83F_TODO() { /* deep habitat: shelter rooms underground with thermal/air trade-offs (Plan 40 tie */ }
    [Fact] public void DS83_AuthorityConformance_TODO() { /* pattern parity with PLAN-AQUIFER-MONITORING-TRUTH-164 */ }
}}
```

## 6. Commands

- `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/`
- Regenerate, do not edit (Plan 1 Appendices AJ/AM).
- `python3 scripts/ci/generate-docs-index.py --check` if docs change.

## 7. Scaffolding rules

1. No production file before a claim.
2. The authority's patterns win over new ones; a deviation is recorded with a reason.
3. Every fixture maps to a package; a package without a fixture is a finding.
