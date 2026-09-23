# PLAN-BIONICS-ENHANCEMENT-78 — Appendix A: Implementation Scaffold

**Generated:** 2026-09-21 by the session scaffold generator. Built with the two
session toolkit plans as its authority: the integration pattern set of
**PLAN-INTEGRATION-KIT-02** (host session, save store, day owner, panel route,
CLI probe, focused fixture) and the appendix/maintenance convention of
**PLAN-ORPHAN-SEAL-01** (generated, reproducible, never hand-edited).
**Status:** scaffolding only — no production file is created by this appendix.
A claim still owns the edits; this page exists so the claim starts from real
paths, real names, and a fixture skeleton derived from this plan's own packages.

## 1. Source inventory (51 files)

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

## 2. Data bindings

| Catalog | Records/shape |
|---|---|
| `bionics.json` | object(2 keys) |

## 3. Host attachment

- Candidate host partials: `Main.Bionics.cs`
- Proposed setup method: `SetupMedicalScaffold` · proposed save method: `SaveMedicalScaffold` (checked against the 228 existing setup names in Plan 1 Appendix AI — extend the real owner name if one exists)
- Loop coupling: see Plan 1 Appendix AA before adding any day/hour step; a new timer is forbidden.

## 4. Test scaffold (proposed, not created)

Proposed file: `Ashfall.Core.Tests/Medical/BE78ScaffoldTests.cs`

```csharp
// Scaffold skeleton — derived from this plan's packages below.
// Fixtures assert one package each; replace the TODO bodies at claim time.
public class BE78ScaffoldTests
{
    [Fact] public void BI78A_TODO() { /* indication/consent: assessment, expectations, refusal; journal record. */ }
    [Fact] public void BI78B_TODO() { /* fitting loop: rehab phases visible (fitting/adaptation/mastery), quality factor, */ }
    [Fact] public void BI78C_TODO() { /* upkeep: charging, condition wear, service tasks; wear engine consumer. */ }
    [Fact] public void BI78D_TODO() { /* malfunctions/rejection: typed events, diagnosis, repair/removal; graft rejection */ }
    [Fact] public void BI78E_TODO() { /* enhancement choices: authored implant catalogue with caps and tradeoffs (energy  */ }
    [Fact] public void BI78F_TODO() { /* social/policy: reactions by belief/faction; shelter policy on access (ties Plan  */ }
}
```

## 5. Commands

- `bash scripts/run_test.sh Ashfall.Core.Tests/Medical/`
- Reachability/structure checks: re-run Plan 1 Appendix generators after the claim lands (Appendix AJ lists triggers).
- `python3 scripts/ci/generate-docs-index.py --check` if docs change.

## 6. Scaffolding rules

1. No production file is created before a claim; this appendix is documentation.
2. Names adopt existing conventions where an owner exists (Plan 1 Appendices AH/AI).
3. Every fixture above maps to a package in the parent plan; a package without a fixture is a finding.
4. The scaffold is regenerated, not edited — see Appendix AM.
