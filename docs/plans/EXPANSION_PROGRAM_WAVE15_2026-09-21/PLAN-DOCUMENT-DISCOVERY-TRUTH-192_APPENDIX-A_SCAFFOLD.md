# PLAN-DOCUMENT-DISCOVERY-TRUTH-192 — Appendix A: Implementation Scaffold

**Generated:** 2026-09-21 by the session scaffold generator (paired variant, batch 3).
**Scaffolding authority:** [`PLAN-CODEX-SURFACE-TRUTH-110`](../EXPANSION_PROGRAM_WAVE9_2026-09-21/PLAN-CODEX-SURFACE-TRUTH-110.md) — adopt the authority's patterns rather than inventing a second architecture. **Convention authority:** `PLAN-ORPHAN-SEAL-01` (generated, reproducible appendices).
**Status:** scaffolding only — no production file is created by this appendix.

## 1. Advisory patterns from the scaffolding authority

| Authority package | Pattern to adopt |
|---|---|
| `CST-110A` | eligibility + surface table generated from catalogs and route registry; `--check` mode. |
| `CST-110B` | reachability probe: render each surface row's entry in a focused UI check. |
| `CST-110C` | freshness check: compare rendered string to authored record for a sampled set; mismatch fails. |

## 2. Source inventory (113 files, Core + host)

| File | Lines |
|---|---:|
| `Culture/DocumentationSystem.cs` | 698 |
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
| `Narrative/EchoSystem.cs` | 398 |
| `Narrative/EncounterCatalog.cs` | 160 |
| `Narrative/EncounterChoiceEffectDispatcher.cs` | 95 |
| `Narrative/FaunaEntomologyCatalog.cs` | 239 |
| `Narrative/FermentationYeastCatalog.cs` | 239 |
| `Narrative/FringeCultsCatalog.cs` | 243 |

… and 73 more.

## 3. Data bindings

| Catalog | Records/shape |
|---|---|
| `quests_bureaucratic_morality.json` | object(2 keys) |
| `documentation_templates.json` | object(2 keys) |
| `bunker_bureaucratic_anomalies.json` | object(3 keys) |
| `bureaucratic_documents_expansion.json` | object(4 keys) |
| `documents_batch_1.json` | object(2 keys) |
| `documents_batch_2.json` | object(3 keys) |

## 4. Host attachment

- Candidate host partials: none — new attachment or headless-only
- Proposed method names: `SetupNarrativeDDCScaffold` / `SaveNarrativeDDCScaffold` (Plan 1 Appendix AI: collision-free)
- Loop/day coupling: consult Plan 1 Appendix AA before adding any step method.

## 5. Test scaffold (proposed, not created)

Proposed file: `Ashfall.Core.Tests/Narrative/DDC192ScaffoldTests.cs`

```csharp
// One fixture per package, plus one authority-conformance fixture.
public class DDC192ScaffoldTests
{
    [Fact] public void DDT192A_TODO() { /* document model + source/condition table. */ }
    [Fact] public void DDT192B_TODO() { /* discovery→read→codex path test. */ }
    [Fact] public void DDT192C_TODO() { /* partial-read fixtures (damage, missing page). */ }
    [Fact] public void DDT192D_TODO() { /* interpretation requirement table + absence-visible test. */ }
    [Fact] public void DDT192E_TODO() { /* persistence round-trip; no re-reveal on load. */ }
    [Fact] public void DDC192_AuthorityConformance_TODO() { /* pattern parity with PLAN-CODEX-SURFACE-TRUTH-110 */ }
}}
```

## 6. Commands

- `bash scripts/run_test.sh Ashfall.Core.Tests/Narrative/`
- Regenerate, do not edit (Plan 1 Appendices AJ/AM).
- `python3 scripts/ci/generate-docs-index.py --check` if docs change.

## 7. Scaffolding rules

1. No production file before a claim.
2. The authority's patterns win; a deviation is recorded with a reason.
3. Every fixture maps to a package; a package without a fixture is a finding.
