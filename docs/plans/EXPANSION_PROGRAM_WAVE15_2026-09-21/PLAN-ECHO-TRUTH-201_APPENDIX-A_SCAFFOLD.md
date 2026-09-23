# PLAN-ECHO-TRUTH-201 — Appendix A: Implementation Scaffold

**Generated:** 2026-09-21 by the session scaffold generator (paired variant, batch 3).
**Scaffolding authority:** [`PLAN-NARRATIVE-ARC-EVENT-TRUTH-176`](../EXPANSION_PROGRAM_WAVE14_2026-09-21/PLAN-NARRATIVE-ARC-EVENT-TRUTH-176.md) — adopt the authority's patterns rather than inventing a second architecture. **Convention authority:** `PLAN-ORPHAN-SEAL-01` (generated, reproducible appendices).
**Status:** scaffolding only — no production file is created by this appendix.

## 1. Advisory patterns from the scaffolding authority

| Authority package | Pattern to adopt |
|---|---|
| `NAE-176A` | event model + trigger/owner table. |
| `NAE-176B` | once-per-campaign firing ledger + reload tests. |
| `NAE-176C` | deterministic intra-day ordering test (paired runs). |

## 2. Source inventory (115 files, Core + host)

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

… and 75 more.

## 3. Data bindings

| Catalog | Records/shape |
|---|---|
| `echoes.json` | object(2 keys) |

## 4. Host attachment

- Candidate host partials: `Main.Echoes.cs`
- Proposed method names: `SetupNarrativeECHScaffold` / `SaveNarrativeECHScaffold` (Plan 1 Appendix AI: collision-free)
- Loop/day coupling: consult Plan 1 Appendix AA before adding any step method.

## 5. Test scaffold (proposed, not created)

Proposed file: `Ashfall.Core.Tests/Narrative/ECH201ScaffoldTests.cs`

```csharp
// One fixture per package, plus one authority-conformance fixture.
public class ECH201ScaffoldTests
{
    [Fact] public void ECH201A_TODO() { /* trigger model + owner table. */ }
    [Fact] public void ECH201B_TODO() { /* targeting/revelation-scope tests. */ }
    [Fact] public void ECH201C_TODO() { /* once-only/cooldown fixtures. */ }
    [Fact] public void ECH201D_TODO() { /* delivery surface check against Plan 110. */ }
    [Fact] public void ECH201E_TODO() { /* save round-trip; no re-fire on load. */ }
    [Fact] public void ECH201_AuthorityConformance_TODO() { /* pattern parity with PLAN-NARRATIVE-ARC-EVENT-TRUTH-176 */ }
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
