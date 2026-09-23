# PLAN-GEOTHERMAL-PLANT-TRUTH-191 — Appendix A: Implementation Scaffold

**Generated:** 2026-09-21 by the session scaffold generator (paired variant, batch 3).
**Scaffolding authority:** [`PLAN-GEOTHERMAL-AQUIFER-TRUTH-260`](../EXPANSION_PROGRAM_WAVE18_2026-09-21/PLAN-GEOTHERMAL-AQUIFER-TRUTH-260.md) — adopt the authority's patterns rather than inventing a second architecture. **Convention authority:** `PLAN-ORPHAN-SEAL-01` (generated, reproducible appendices).
**Status:** scaffolding only — no production file is created by this appendix.

## 1. Advisory patterns from the scaffolding authority

| Authority package | Pattern to adopt |
|---|---|
| `GAT-260A` | coupling model + shared-state owner table. |
| `GAT-260B` | interference table + fixture per quadrant. |
| `GAT-260C` | atomic save test (no desync). |

## 2. Source inventory (88 files, Core + host)

| File | Lines |
|---|---:|
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

… and 48 more.

## 3. Data bindings

| Catalog | Records/shape |
|---|---|
| `geothermal_drilling_depths.json` | object(2 keys) |
| `geothermal_strata_catalog.json` | object(2 keys) |
| `geothermal_borehole_logs.json` | 7 |
| `geothermal_steam_vent_diagnostics.json` | 7 |
| `geothermal_steam_well_logs.json` | 8 |

## 4. Host attachment

- Candidate host partials: none — new attachment or headless-only
- Proposed method names: `SetupShelterGTScaffold` / `SaveShelterGTScaffold` (Plan 1 Appendix AI: collision-free)
- Loop/day coupling: consult Plan 1 Appendix AA before adding any step method.

## 5. Test scaffold (proposed, not created)

Proposed file: `Ashfall.Core.Tests/Shelter/GT191ScaffoldTests.cs`

```csharp
// One fixture per package, plus one authority-conformance fixture.
public class GT191ScaffoldTests
{
    [Fact] public void GOT191A_TODO() { /* source model + dependency on Plan 83 state. */ }
    [Fact] public void GOT191B_TODO() { /* output curve + degraded-fluid fixture. */ }
    [Fact] public void GOT191C_TODO() { /* fluid upkeep + conservation check. */ }
    [Fact] public void GOT191D_TODO() { /* grid interface test with Plan 48. */ }
    [Fact] public void GOT191E_TODO() { /* save round-trip; output resumes at stored condition. */ }
    [Fact] public void GT191_AuthorityConformance_TODO() { /* pattern parity with PLAN-GEOTHERMAL-AQUIFER-TRUTH-260 */ }
}}
```

## 6. Commands

- `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/`
- Regenerate, do not edit (Plan 1 Appendices AJ/AM).
- `python3 scripts/ci/generate-docs-index.py --check` if docs change.

## 7. Scaffolding rules

1. No production file before a claim.
2. The authority's patterns win; a deviation is recorded with a reason.
3. Every fixture maps to a package; a package without a fixture is a finding.
