# PLAN-AUTONOMOUS-MACHINES-79 — Appendix A: Implementation Scaffold

**Generated:** 2026-09-21 by the session scaffold generator. Built with the two
session toolkit plans as its authority: the integration pattern set of
**PLAN-INTEGRATION-KIT-02** (host session, save store, day owner, panel route,
CLI probe, focused fixture) and the appendix/maintenance convention of
**PLAN-ORPHAN-SEAL-01** (generated, reproducible, never hand-edited).
**Status:** scaffolding only — no production file is created by this appendix.
A claim still owns the edits; this page exists so the claim starts from real
paths, real names, and a fixture skeleton derived from this plan's own packages.

## 1. Source inventory (116 files)

| File | Lines |
|---|---:|
| `AdvancedMachinery/AdvancedMachineContracts.cs` | 101 |
| `Audio/ScarcityAudioStateMachine.cs` | 300 |
| `Crafting/ChemicalSynthesisCatalog.cs` | 199 |
| `Crafting/ChemicalSynthesisSystem.cs` | 321 |
| `Crafting/CraftContext.cs` | 29 |
| `Crafting/CraftingSystem.cs` | 496 |
| `Crafting/PharmaRecipeCatalogLoader.cs` | 107 |
| `Crafting/RecipeCatalogLoader.cs` | 179 |
| `Crafting/RelicCatalogLoader.cs` | 109 |
| `Crafting/RoboticsSystem.cs` | 367 |
| `Crafting/TrapRecipeIntegrity.cs` | 81 |
| `Foundry/FoundryActionSurface.cs` | 110 |
| `Foundry/GlassworksCatalog.cs` | 166 |
| `Foundry/HydraulicExtrusionEngine.cs` | 377 |
| `Foundry/MaterialProfileCatalog.cs` | 239 |
| `Foundry/MetallurgyHeavyCatalog.cs` | 177 |
| `Foundry/PowderMetallurgySystem.cs` | 330 |
| `Foundry/SaltMineExtractionSystem.cs` | 455 |
| `Foundry/SilentFoundryCatalog.cs` | 304 |
| `Foundry/SilentFoundryConsequencePolicy.cs` | 227 |
| `Foundry/SilentFoundryHeadlessDemo.cs` | 188 |
| `Foundry/SilentFoundrySystem.Glassworks.cs` | 20 |
| `Foundry/SilentFoundrySystem.Heat.cs` | 549 |
| `Foundry/SilentFoundrySystem.Material.cs` | 333 |
| `Foundry/SilentFoundrySystem.Metallurgy.cs` | 222 |
| `Foundry/SilentFoundrySystem.TreatyLabor.cs` | 179 |
| `Foundry/SilentFoundrySystem.cs` | 690 |
| `Foundry/SilentFoundryTypes.cs` | 283 |
| `Medical/VigilStateMachine.cs` | 142 |
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

… and 76 more.

## 2. Data bindings

| Catalog | Records/shape |
|---|---|
| `robotics.json` | object(2 keys) |
| `shelter_machine_identities.json` | object(5 keys) |
| `drone_carrier_blackboxes.json` | 8 |

## 3. Host attachment

- Candidate host partials: none — new attachment or headless-only

- Proposed setup method: `SetupCraftingScaffold` · proposed save method: `SaveCraftingScaffold` (checked against the 228 existing setup names in Plan 1 Appendix AI — extend the real owner name if one exists)
- Loop coupling: see Plan 1 Appendix AA before adding any day/hour step; a new timer is forbidden.

## 4. Test scaffold (proposed, not created)

Proposed file: `Ashfall.Core.Tests/Crafting/AM79ScaffoldTests.cs`

```csharp
// Scaffold skeleton — derived from this plan's packages below.
// Fixtures assert one package each; replace the TODO bodies at claim time.
public class AM79ScaffoldTests
{
    [Fact] public void AM79A_TODO() { /* machine registry: same vehicle/equipment owners; each machine has condition, fue */ }
    [Fact] public void AM79B_TODO() { /* drone recon loop: launch window, path, intel output (PLAN 49 fusion), loss/crash */ }
    [Fact] public void AM79C_TODO() { /* crawler logistics: hauling contracts, breakdowns, recovery missions. */ }
    [Fact] public void AM79D_TODO() { /* sentry platforms: placement, rules of engagement, false alarms, ammo draw. */ }
    [Fact] public void AM79E_TODO() { /* utility automata: task assignment, upkeep, autonomy failure (they stop, they don */ }
    [Fact] public void AM79F_TODO() { /* remote piloting: control link quality, jamming (Plan 49), operator skill. */ }
}
```

## 5. Commands

- `bash scripts/run_test.sh Ashfall.Core.Tests/Crafting/`
- Reachability/structure checks: re-run Plan 1 Appendix generators after the claim lands (Appendix AJ lists triggers).
- `python3 scripts/ci/generate-docs-index.py --check` if docs change.

## 6. Scaffolding rules

1. No production file is created before a claim; this appendix is documentation.
2. Names adopt existing conventions where an owner exists (Plan 1 Appendices AH/AI).
3. Every fixture above maps to a package in the parent plan; a package without a fixture is a finding.
4. The scaffold is regenerated, not edited — see Appendix AM.
