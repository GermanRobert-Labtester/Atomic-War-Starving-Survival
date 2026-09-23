# PLAN-CHLOR-ALKALI-TRUTH-199 — Appendix A: Implementation Scaffold

**Generated:** 2026-09-21 by the session scaffold generator (paired variant, batch 3).
**Scaffolding authority:** [`PLAN-PLASTIC-PYROLYSIS-TRUTH-187`](../EXPANSION_PROGRAM_WAVE14_2026-09-21/PLAN-PLASTIC-PYROLYSIS-TRUTH-187.md) — adopt the authority's patterns rather than inventing a second architecture. **Convention authority:** `PLAN-ORPHAN-SEAL-01` (generated, reproducible appendices).
**Status:** scaffolding only — no production file is created by this appendix.

## 1. Advisory patterns from the scaffolding authority

| Authority package | Pattern to adopt |
|---|---|
| `PPT-187A` | feedstock/yield table (data-validated). |
| `PPT-187B` | mass-balance test + deliberate-yield-error fixture. |
| `PPT-187C` | emission path to Plan 183 + containment upkeep. |

## 2. Source inventory (85 files, Core + host)

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

… and 45 more.

## 3. Data bindings

| Catalog | Records/shape |
|---|---|
| `chlor_alkali_synthesis_catalog.json` | object(2 keys) |
| `calcium_hypochlorite_titration_reports.json` | 7 |

## 4. Host attachment

- Candidate host partials: none — new attachment or headless-only
- Proposed method names: `SetupShelterCAT2Scaffold` / `SaveShelterCAT2Scaffold` (Plan 1 Appendix AI: collision-free)
- Loop/day coupling: consult Plan 1 Appendix AA before adding any step method.

## 5. Test scaffold (proposed, not created)

Proposed file: `Ashfall.Core.Tests/Shelter/CAT2199ScaffoldTests.cs`

```csharp
// One fixture per package, plus one authority-conformance fixture.
public class CAT2199ScaffoldTests
{
    [Fact] public void CAT199A_TODO() { /* process/yield table + conservation test. */ }
    [Fact] public void CAT199B_TODO() { /* containment requirement + emission fixture. */ }
    [Fact] public void CAT199C_TODO() { /* consumer table + per-consumer routing tests. */ }
    [Fact] public void CAT199D_TODO() { /* power coupling test (brownout → lower output). */ }
    [Fact] public void CAT199E_TODO() { /* save round-trip; no re-roll on load. */ }
    [Fact] public void CAT2199_AuthorityConformance_TODO() { /* pattern parity with PLAN-PLASTIC-PYROLYSIS-TRUTH-187 */ }
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
