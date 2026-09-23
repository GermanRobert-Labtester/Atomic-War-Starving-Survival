# PLAN-ANCIENT-RUINS-VAULTS-84 — Appendix A: Implementation Scaffold

**Generated:** 2026-09-21 by the session scaffold generator (paired variant, host-inclusive scan).
**Scaffolding authority:** [`PLAN-ARCHAEOLOGY-TRUTH-152`](../EXPANSION_PROGRAM_WAVE12_2026-09-21/PLAN-ARCHAEOLOGY-TRUTH-152.md) — this plan adopts the authority's patterns (below) rather than inventing a second architecture. **Convention authority:** `PLAN-ORPHAN-SEAL-01` (generated, reproducible appendices).
**Status:** scaffolding only — no production file is created by this appendix.

## 1. Advisory patterns from the scaffolding authority

| Authority package | Pattern to adopt |
|---|---|
| `ARC-152A` | lifecycle + stage clock table. |
| `ARC-152B` | strata/hazard table + supply consumption per stage. |
| `ARC-152C` | find provenance record + display-from-record test. |

## 2. Source inventory (162 files, Core + host)

| File | Lines |
|---|---:|
| `Archaeology/ArchaeologySystem.cs` | 317 |
| `Crafting/RelicCatalogLoader.cs` | 109 |
| `Culture/CulturalArchiveVaultSystem.cs` | 675 |
| `Expeditions/AerialReconWindowEngine.cs` | 200 |
| `Expeditions/AmphibiousDraisineCatalog.cs` | 237 |
| `Expeditions/AmphibiousDraisineEngine.cs` | 516 |
| `Expeditions/ArmoredCrawlerExpeditionSystem.cs` | 402 |
| `Expeditions/ArmoredCrawlerModuleCatalog.cs` | 133 |
| `Expeditions/AviationSystem.cs` | 462 |
| `Expeditions/ChemicalReconEngine.cs` | 593 |
| `Expeditions/ColonySystem.cs` | 517 |
| `Expeditions/DiscoveryConsequenceSystem.cs` | 390 |
| `Expeditions/DiveInstanceRunner.cs` | 117 |
| `Expeditions/DraisineRerailingSystem.cs` | 255 |
| `Expeditions/EncounterChoiceResolver.cs` | 133 |
| `Expeditions/ExpeditionAggregate.cs` | 116 |
| `Expeditions/ExpeditionCatalogLoader.cs` | 130 |
| `Expeditions/ExpeditionEncounterBridge.cs` | 355 |
| `Expeditions/ExpeditionHeadlessDemo.cs` | 120 |
| `Expeditions/ExpeditionLootReferenceResolver.cs` | 93 |
| `Expeditions/ExpeditionLootValidator.cs` | 115 |
| `Expeditions/ExpeditionNavalSystem.cs` | 280 |
| `Expeditions/ExpeditionSystem.cs` | 1580 |
| `Expeditions/ExpeditionTravelStretch.cs` | 54 |
| `Expeditions/MineClearingFlailEngine.cs` | 351 |
| `Expeditions/MineFlailCatalogLoader.cs` | 110 |
| `Expeditions/RadarEcmCatalog.cs` | 96 |
| `Expeditions/RailGrindingCatalogLoader.cs` | 128 |
| `Expeditions/RailGrindingEngine.cs` | 336 |
| `Expeditions/RailLogisticsCatalog.cs` | 59 |
| `Expeditions/RailwayInterlockEngine.cs` | 730 |
| `Expeditions/RailwaySystem.cs` | 831 |
| `Expeditions/ReconTelemetryCatalog.cs` | 83 |
| `Expeditions/ReconTelemetryCatalogLoader.cs` | 34 |
| `Expeditions/ReconTelemetryHeadlessDemo.cs` | 94 |
| `Expeditions/ReconTelemetryState.cs` | 57 |
| `Expeditions/ReconTelemetrySystem.cs` | 331 |
| `Expeditions/RunFlatTireEngine.cs` | 348 |
| `Expeditions/ScavengingTableCatalog.cs` | 209 |
| `Expeditions/TravelEncounterCombatBinder.cs` | 57 |
| `Expeditions/VehicleArmorGradeCatalog.cs` | 155 |
| `Expeditions/VehicleGarageCatalog.cs` | 62 |
| `Expeditions/VehicleGarageSystem.cs` | 865 |
| `Expeditions/VerticalAscentCatalog.cs` | 109 |
| `Narrative/AbyssalAnomaliesCatalog.cs` | 438 |

… and 117 more.

## 3. Data bindings

| Catalog | Records/shape |
|---|---|
| `relic_recipes.json` | object(2 keys) |
| `architect_vault_audits.json` | 7 |
| `relic_provenance_dossiers.json` | object(3 keys) |
| `surface_dragline_ruins.json` | 8 |
| `vault_seal_breach_logs.json` | 7 |

## 4. Host attachment

- Candidate host partials: `Main.UiTests.WorkshopRelic.cs`
- Proposed method names: `SetupNarrativeARScaffold` / `SaveNarrativeARScaffold` (Plan 1 Appendix AI: collision-free)
- Loop/day coupling: consult Plan 1 Appendix AA before adding any step method; no new timer.

## 5. Test scaffold (proposed, not created)

Proposed file: `Ashfall.Core.Tests/Narrative/AR84ScaffoldTests.cs`

```csharp
// One fixture per package, plus one authority-conformance fixture.
public class AR84ScaffoldTests
{
    [Fact] public void RV84A_TODO() { /* site model: authored multi-room complexes with state (power, doors, hazards) and */ }
    [Fact] public void RV84B_TODO() { /* entry puzzles: codes, power, breaching; tools and skills matter; failure is loud */ }
    [Fact] public void RV84C_TODO() { /* interior hazards: authored per site, using canonical hazard owners. */ }
    [Fact] public void RV84D_TODO() { /* records: terminals/tapes/logs (decryption gated) yielding lore, schematics, and  */ }
    [Fact] public void RV84E_TODO() { /* unique salvage: relics/prototypes via `UniqueItemClaimRegistry`; display/study/t */ }
    [Fact] public void RV84F_TODO() { /* inhabitants/decisions: survivors, machines, or faction squads; outcomes (rescue, */ }
    [Fact] public void AR84_AuthorityConformance_TODO() { /* pattern parity with PLAN-ARCHAEOLOGY-TRUTH-152 */ }
}}
```

## 6. Commands

- `bash scripts/run_test.sh Ashfall.Core.Tests/Narrative/`
- Regenerate, do not edit (Plan 1 Appendices AJ/AM).
- `python3 scripts/ci/generate-docs-index.py --check` if docs change.

## 7. Scaffolding rules

1. No production file before a claim.
2. The authority's patterns win over new ones; a deviation is recorded with a reason.
3. Every fixture maps to a package; a package without a fixture is a finding.
