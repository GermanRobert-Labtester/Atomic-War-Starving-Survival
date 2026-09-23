# PLAN-MUTATION-HEREDITY-81 — Appendix A: Implementation Scaffold

**Generated:** 2026-09-21 by the session scaffold generator (paired variant, host-inclusive scan).
**Scaffolding authority:** [`PLAN-PANDEMIC-PUBLIC-HEALTH-47`](../EXPANSION_PROGRAM_WAVE5_2026-09-21/PLAN-PANDEMIC-PUBLIC-HEALTH-47.md) — this plan adopts the authority's patterns (below) rather than inventing a second architecture. **Convention authority:** `PLAN-ORPHAN-SEAL-01` (generated, reproducible appendices).
**Status:** scaffolding only — no production file is created by this appendix.

## 1. Advisory patterns from the scaffolding authority

| Authority package | Pattern to adopt |
|---|---|

## 2. Source inventory (62 files, Core + host)

| File | Lines |
|---|---:|
| `GenerationalLineageExtension.cs` | 434 |
| `Generations/SecondGenerationMilestoneEngine.cs` | 229 |
| `Legacy/GenerationalSuccessionEngine.cs` | 166 |
| `Medical/AdvancedSurgicalWardSystem.cs` | 381 |
| `Medical/AfflictionContracts.cs` | 128 |
| `Medical/AfflictionDutyBridge.cs` | 91 |
| `Medical/AfflictionId.cs` | 275 |
| `Medical/AfflictionQuestWorkBridge.cs` | 229 |
| `Medical/AmputationSystem.cs` | 528 |
| `Medical/BionicsSystem.cs` | 805 |
| `Medical/ChemicalDependencyAfflictionHandler.cs` | 229 |
| `Medical/ChemicalDependencySystem.cs` | 536 |
| `Medical/ChronicConditionSystem.cs` | 346 |
| `Medical/ClinicalWardTriageEngine.cs` | 307 |
| `Medical/DependencyTaperWithdrawalEngine.cs` | 268 |
| `Medical/DiagnosisKnowledgeStore.cs` | 190 |
| `Medical/DiseaseAfflictionHandler.cs` | 177 |
| `Medical/DiseaseProtocolHandler.cs` | 141 |
| `Medical/HealthHistorySystem.cs` | 411 |
| `Medical/LyophilizationSystem.cs` | 332 |
| `Medical/MedicalConditionResolver.cs` | 118 |
| `Medical/MedicalHeadlessDemo.cs` | 108 |
| `Medical/MedicalPipelineCoordinator.cs` | 747 |
| `Medical/MedicalPipelineSave.cs` | 48 |
| `Medical/MedicalProcedureSchedule.cs` | 267 |
| `Medical/MedicalRecordLog.cs` | 143 |
| `Medical/MedicalReservationLedger.cs` | 185 |
| `Medical/MedicalTextCatalog.cs` | 177 |
| `Medical/MedicalTreatmentCatalog.cs` | 221 |
| `Medical/MedicalWardPipelineBridge.cs` | 88 |
| `Medical/MedicalWardSave.cs` | 80 |
| `Medical/MedicalWardSystem.cs` | 363 |
| `Medical/MicrofluidicDiagnosticCatalogLoader.cs` | 107 |
| `Medical/MicrofluidicDiagnosticEngine.cs` | 609 |
| `Medical/MutationSystem.cs` | 352 |
| `Medical/NarcoticsSystem.cs` | 368 |
| `Medical/PalliativeCareDignityEngine.cs` | 254 |
| `Medical/PatientRecord.cs` | 233 |
| `Medical/PatientRecordIntegrityValidator.cs` | 195 |
| `Medical/PharmaceuticalTabletEngine.cs` | 721 |
| `Medical/ProstheticConditionWearEngine.cs` | 157 |
| `Medical/PsychologyAfflictionHandlers.cs` | 262 |
| `Medical/RadiationAfflictionHandlers.cs` | 239 |
| `Medical/RehabilitationProgressionEngine.cs` | 112 |
| `Medical/RehabilitationSlateProjection.cs` | 126 |

… and 17 more.

## 3. Data bindings

| Catalog | Records/shape |
|---|---|
| `mutations.json` | object(2 keys) |
| `sky_defense_ordnance.json` | object(2 keys) |
| `mutated_botanical_logs.json` | 8 |

## 4. Host attachment

- Candidate host partials: none — new attachment or headless-only
- Proposed method names: `SetupMedicalMHScaffold` / `SaveMedicalMHScaffold` (Plan 1 Appendix AI: collision-free)
- Loop/day coupling: consult Plan 1 Appendix AA before adding any step method; no new timer.

## 5. Test scaffold (proposed, not created)

Proposed file: `Ashfall.Core.Tests/Medical/MH81ScaffoldTests.cs`

```csharp
// One fixture per package, plus one authority-conformance fixture.
public class MH81ScaffoldTests
{
    [Fact] public void MH81A_TODO() { /* chronic management: dose-driven conditions that progress slowly and respond to c */ }
    [Fact] public void MH81B_TODO() { /* screening/counselling: options with knowledge, not reveals; player consent in-wo */ }
    [Fact] public void MH81C_TODO() { /* maternal/child care: support paths, complications handled with restraint and exi */ }
    [Fact] public void MH81D_TODO() { /* fictional heredity: a small trait table expressed through existing survivor trai */ }
    [Fact] public void MH81E_TODO() { /* prevention: shielding/gear/shelter advice surfaced from existing curves. */ }
    [Fact] public void MH81F_TODO() { /* records/consent: medical record log (Plan 198) + archive; privacy stated. */ }
    [Fact] public void MH81_AuthorityConformance_TODO() { /* pattern parity with PLAN-PANDEMIC-PUBLIC-HEALTH-47 */ }
}}
```

## 6. Commands

- `bash scripts/run_test.sh Ashfall.Core.Tests/Medical/`
- Regenerate, do not edit (Plan 1 Appendices AJ/AM).
- `python3 scripts/ci/generate-docs-index.py --check` if docs change.

## 7. Scaffolding rules

1. No production file before a claim.
2. The authority's patterns win over new ones; a deviation is recorded with a reason.
3. Every fixture maps to a package; a package without a fixture is a finding.
