# PLAN-HEALTH-HISTORY-TRUTH-196 — Appendix A: Implementation Scaffold

**Generated:** 2026-09-21 by the session scaffold generator (paired variant, batch 3).
**Scaffolding authority:** [`PLAN-ACUTE-TRAUMA-CARE-124`](../EXPANSION_PROGRAM_WAVE10_2026-09-21/PLAN-ACUTE-TRAUMA-CARE-124.md) — adopt the authority's patterns rather than inventing a second architecture. **Convention authority:** `PLAN-ORPHAN-SEAL-01` (generated, reproducible appendices).
**Status:** scaffolding only — no production file is created by this appendix.

## 1. Advisory patterns from the scaffolding authority

| Authority package | Pattern to adopt |
|---|---|
| `ATC-124A` | wound model + severity table (writer = existing health owner). |
| `ATC-124B` | triage ordering + deterministic tie-break test. |
| `ATC-124C` | surgery outcome + complication/rejection seeded tests. |

## 2. Source inventory (51 files, Core + host)

| File | Lines |
|---|---:|
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

… and 11 more.

## 3. Data bindings

No catalog name-matched; verify paths/loaders before claiming a data dependency.

## 4. Host attachment

- Candidate host partials: none — new attachment or headless-only
- Proposed method names: `SetupMedicalHHScaffold` / `SaveMedicalHHScaffold` (Plan 1 Appendix AI: collision-free)
- Loop/day coupling: consult Plan 1 Appendix AA before adding any step method.

## 5. Test scaffold (proposed, not created)

Proposed file: `Ashfall.Core.Tests/Medical/HH196ScaffoldTests.cs`

```csharp
// One fixture per package, plus one authority-conformance fixture.
public class HH196ScaffoldTests
{
    [Fact] public void HHT196A_TODO() { /* record model + write-source table. */ }
    [Fact] public void HHT196B_TODO() { /* continuity read tests (care sees prior history). */ }
    [Fact] public void HHT196C_TODO() { /* privacy/role access test per role class. */ }
    [Fact] public void HHT196D_TODO() { /* retention test (no silent truncation). */ }
    [Fact] public void HHT196E_TODO() { /* save round-trip; no append/truncate on load. */ }
    [Fact] public void HH196_AuthorityConformance_TODO() { /* pattern parity with PLAN-ACUTE-TRAUMA-CARE-124 */ }
}}
```

## 6. Commands

- `bash scripts/run_test.sh Ashfall.Core.Tests/Medical/`
- Regenerate, do not edit (Plan 1 Appendices AJ/AM).
- `python3 scripts/ci/generate-docs-index.py --check` if docs change.

## 7. Scaffolding rules

1. No production file before a claim.
2. The authority's patterns win; a deviation is recorded with a reason.
3. Every fixture maps to a package; a package without a fixture is a finding.
