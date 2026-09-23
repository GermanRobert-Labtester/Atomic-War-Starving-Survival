# PLAN-COLLECTIBLES-RELICS-67 — Appendix A: Implementation Scaffold

**Generated:** 2026-09-21 by the session scaffold generator. Built with the two
session toolkit plans as its authority: the integration pattern set of
**PLAN-INTEGRATION-KIT-02** (host session, save store, day owner, panel route,
CLI probe, focused fixture) and the appendix/maintenance convention of
**PLAN-ORPHAN-SEAL-01** (generated, reproducible, never hand-edited).
**Status:** scaffolding only — no production file is created by this appendix.
A claim still owns the edits; this page exists so the claim starts from real
paths, real names, and a fixture skeleton derived from this plan's own packages.

## 1. Source inventory (182 files)

| File | Lines |
|---|---:|
| `CollectibleCatalog.cs` | 97 |
| `CollectibleDiscoveryState.cs` | 302 |
| `Collectibles/CollectibleEffectDispatcher.cs` | 352 |
| `Collectibles/CollectibleMapProjector.cs` | 153 |
| `Collectibles/CollectibleTutorialTracker.cs` | 182 |
| `Content/CollectibleCatalogIntegrityValidator.cs` | 433 |
| `Crafting/RelicCatalogLoader.cs` | 109 |
| `Narrative/AbyssalAnomaliesCatalog.cs` | 438 |
| `Narrative/AbyssalAnomaliesProjection.cs` | 421 |
| `Narrative/ApicultureBeeCatalog.cs` | 239 |
| `Narrative/BlackProjectsArchiveSystem.cs` | 367 |
| `Narrative/BlackProjectsCatalog.cs` | 260 |
| `Narrative/BoneHornCarvingCatalog.cs` | 97 |
| `Narrative/BunkerBlueprintCatalog.cs` | 121 |
| `Narrative/BunkerContrabandCatalog.cs` | 182 |
| `Narrative/BunkerCourtCatalog.cs` | 278 |
| `Narrative/BunkerGraffitiCatalog.cs` | 209 |
| `Narrative/BunkerGraffitiProjection.cs` | 198 |
| `Narrative/BunkerMaintenanceCatalog.cs` | 245 |
| `Narrative/BunkerMaintenanceProjection.cs` | 151 |
| `Narrative/BureaucraticDocumentCatalog.cs` | 531 |
| `Narrative/CandleMakingWaxCatalog.cs` | 95 |
| `Narrative/CeramicsKilnCatalog.cs` | 95 |
| `Narrative/CeremonySystem.cs` | 360 |
| `Narrative/CharcoalPyrolysisCatalog.cs` | 239 |
| `Narrative/CipherQuestChainEngine.cs` | 192 |
| `Narrative/ContrabandBrokerCaravan.cs` | 76 |
| `Narrative/ContrabandCatalogValidator.cs` | 286 |
| `Narrative/ContrabandStashSystem.cs` | 275 |
| `Narrative/CordageCableCatalog.cs` | 239 |
| `Narrative/CourierDispatchCatalog.cs` | 117 |
| `Narrative/CrucibleFoundryCatalog.cs` | 239 |
| `Narrative/CryoPreservationCatalog.cs` | 239 |
| `Narrative/CulinaryRationCatalog.cs` | 116 |
| `Narrative/CurrentsPamphletCatalog.cs` | 77 |
| `Narrative/DailySurvivalCatalog.cs` | 263 |
| `Narrative/DeadHandDirectiveCatalog.cs` | 117 |
| `Narrative/DwellerHeirloomCatalog.cs` | 103 |
| `Narrative/DwellerMedicalCatalog.cs` | 153 |
| `Narrative/EchoCatalog.cs` | 400 |

… and 142 more.

## 2. Data bindings

| Catalog | Records/shape |
|---|---|
| `phantom_heirlooms.json` | 12 |
| `collectibles.json` | object(2 keys) |
| `relic_recipes.json` | object(2 keys) |
| `dweller_heirlooms_master.json` | object(3 keys) |
| `heirloom_seed_viability_reports.json` | 7 |
| `relic_provenance_dossiers.json` | object(3 keys) |

## 3. Host attachment

- Candidate host partials: `Main.Collectibles.cs`, `Main.UiTests.WorkshopRelic.cs`
- Proposed setup method: `SetupCollectiblesScaffold` · proposed save method: `SaveCollectiblesScaffold` (checked against the 228 existing setup names in Plan 1 Appendix AI — extend the real owner name if one exists)
- Loop coupling: see Plan 1 Appendix AA before adding any day/hour step; a new timer is forbidden.

## 4. Test scaffold (proposed, not created)

Proposed file: `Ashfall.Core.Tests/Collectibles/CR67ScaffoldTests.cs`

```csharp
// Scaffold skeleton — derived from this plan's packages below.
// Fixtures assert one package each; replace the TODO bodies at claim time.
public class CR67ScaffoldTests
{
    [Fact] public void CR67A_TODO() { /* set model: authored sets with completion effects and lore unlocks; progress visi */ }
    [Fact] public void CR67B_TODO() { /* discovery integration: finds come from canonical loot/excavation/maritime tables */ }
    [Fact] public void CR67C_TODO() { /* provenance: item lore research reveals origin; provenance affects value and disp */ }
    [Fact] public void CR67D_TODO() { /* display/exhibition: museum/quarters display gives identity + visitor interest (t */ }
    [Fact] public void CR67E_TODO() { /* collector economy: appraisals and trades through canonical market/black market;  */ }
    [Fact] public void CR67F_TODO() { /* preservation: condition degradation and restoration for display pieces. */ }
}
```

## 5. Commands

- `bash scripts/run_test.sh Ashfall.Core.Tests/Collectibles/`
- Reachability/structure checks: re-run Plan 1 Appendix generators after the claim lands (Appendix AJ lists triggers).
- `python3 scripts/ci/generate-docs-index.py --check` if docs change.

## 6. Scaffolding rules

1. No production file is created before a claim; this appendix is documentation.
2. Names adopt existing conventions where an owner exists (Plan 1 Appendices AH/AI).
3. Every fixture above maps to a package in the parent plan; a package without a fixture is a finding.
4. The scaffold is regenerated, not edited — see Appendix AM.
