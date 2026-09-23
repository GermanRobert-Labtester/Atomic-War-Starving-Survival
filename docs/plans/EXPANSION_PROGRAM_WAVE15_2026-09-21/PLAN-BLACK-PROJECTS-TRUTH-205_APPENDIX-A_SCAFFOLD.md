# PLAN-BLACK-PROJECTS-TRUTH-205 — Appendix A: Implementation Scaffold

**Generated:** 2026-09-21 by the session scaffold generator (paired variant, batch 3).
**Scaffolding authority:** [`PLAN-CULTURAL-ARCHIVE-TRUTH-169`](../EXPANSION_PROGRAM_WAVE13_2026-09-21/PLAN-CULTURAL-ARCHIVE-TRUTH-169.md) — adopt the authority's patterns rather than inventing a second architecture. **Convention authority:** `PLAN-ORPHAN-SEAL-01` (generated, reproducible appendices).
**Status:** scaffolding only — no production file is created by this appendix.

## 1. Advisory patterns from the scaffolding authority

| Authority package | Pattern to adopt |
|---|---|
| `CAT-169A` | deposit model + copy/remove rule. |
| `CAT-169B` | access tiers from Plan 141 + test per tier. |
| `CAT-169C` | threat table + one loss fixture per threat (mitigation consumable). |

## 2. Source inventory (114 files, Core + host)

| File | Lines |
|---|---:|
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
| `Narrative/GeologicalStrataCatalog.cs` | 116 |

… and 74 more.

## 3. Data bindings

No catalog name-matched; verify paths/loaders before claiming a data dependency.

## 4. Host attachment

- Candidate host partials: none — new attachment or headless-only
- Proposed method names: `SetupNarrativeBPScaffold` / `SaveNarrativeBPScaffold` (Plan 1 Appendix AI: collision-free)
- Loop/day coupling: consult Plan 1 Appendix AA before adding any step method.

## 5. Test scaffold (proposed, not created)

Proposed file: `Ashfall.Core.Tests/Narrative/BP205ScaffoldTests.cs`

```csharp
// One fixture per package, plus one authority-conformance fixture.
public class BP205ScaffoldTests
{
    [Fact] public void BPT205A_TODO() { /* seal model + access class table. */ }
    [Fact] public void BPT205B_TODO() { /* access evaluation tests per class. */ }
    [Fact] public void BPT205C_TODO() { /* revelation gate fixtures (mission, relationship, era). */ }
    [Fact] public void BPT205D_TODO() { /* leak routing to Plan 29/121 + state reflection test. */ }
    [Fact] public void BPT205E_TODO() { /* save round-trip; no silent open/re-seal. */ }
    [Fact] public void BP205_AuthorityConformance_TODO() { /* pattern parity with PLAN-CULTURAL-ARCHIVE-TRUTH-169 */ }
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
