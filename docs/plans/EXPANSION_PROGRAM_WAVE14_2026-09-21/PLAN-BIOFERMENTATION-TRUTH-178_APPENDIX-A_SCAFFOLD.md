# PLAN-BIOFERMENTATION-TRUTH-178 — Appendix A: Implementation Scaffold

**Generated:** 2026-09-21 by the session scaffold generator (paired variant, batch 3).
**Scaffolding authority:** [`PLAN-PRESERVATION-TRUTH-118`](../EXPANSION_PROGRAM_WAVE10_2026-09-21/PLAN-PRESERVATION-TRUTH-118.md) — adopt the authority's patterns rather than inventing a second architecture. **Convention authority:** `PLAN-ORPHAN-SEAL-01` (generated, reproducible appendices).
**Status:** scaffolding only — no production file is created by this appendix.

## 1. Advisory patterns from the scaffolding authority

| Authority package | Pattern to adopt |
|---|---|
| `PRT-118A` | spoil class + curve table (data-backed, schema-checked per Plan 90). |
| `PRT-118B` | condition factors + method factors with one test per method. |
| `PRT-118C` | dead-engine decision record (revive via owner or codex-only). |

## 2. Source inventory (89 files, Core + host)

| File | Lines |
|---|---:|
| `Narrative/FermentationYeastCatalog.cs` | 239 |
| `Narrative/RefrigerationFermentationCatalog.cs` | 239 |
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

… and 49 more.

## 3. Data bindings

| Catalog | Records/shape |
|---|---|
| `bio_fermentation_catalog.json` | object(3 keys) |
| `fermentation_crock_airlock_assays.json` | 7 |

## 4. Host attachment

- Candidate host partials: none — new attachment or headless-only
- Proposed method names: `SetupShelterBFScaffold` / `SaveShelterBFScaffold` (Plan 1 Appendix AI: collision-free)
- Loop/day coupling: consult Plan 1 Appendix AA before adding any step method.

## 5. Test scaffold (proposed, not created)

Proposed file: `Ashfall.Core.Tests/Shelter/BF178ScaffoldTests.cs`

```csharp
// One fixture per package, plus one authority-conformance fixture.
public class BF178ScaffoldTests
{
    [Fact] public void BFT178A_TODO() { /* culture model + propagation rule. */ }
    [Fact] public void BFT178B_TODO() { /* batch lifecycle + clock tests. */ }
    [Fact] public void BFT178C_TODO() { /* contamination inputs + spoiled-outcome fixture. */ }
    [Fact] public void BFT178D_TODO() { /* output routing (inventory/pharma) + conservation check. */ }
    [Fact] public void BFT178E_TODO() { /* save round-trip; no re-roll on load. */ }
    [Fact] public void BF178_AuthorityConformance_TODO() { /* pattern parity with PLAN-PRESERVATION-TRUTH-118 */ }
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
