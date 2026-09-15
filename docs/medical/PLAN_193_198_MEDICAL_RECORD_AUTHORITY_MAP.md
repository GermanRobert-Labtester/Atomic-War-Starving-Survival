# Plans 193 / 198 — Chronic conditions + medical history authority map

**Status:** ACCEPTED — §3 signed 2026-09-12  
**Package:** `DEBT-170-199-REMAINING-FAMILY-MAPS`  
**Date:** 2026-09-12

**Stale historical prose:** `Plan_193_*`, `Plan_198_*`. `src/Main` “Plan 198” is chem warfare (**number drift**).

---

## 1. Premise

| Concern | Owner | Save |
|---|---|---|
| Coordination | `MedicalPipelineCoordinator` | `medical_pipeline` |
| Current episode knowledge | `DiagnosisKnowledgeStore` | inside pipeline |
| Patient chart projection | `PatientRecord` | **never saved** |
| Radiation reading history | `DoseLedgerSystem` | `dose_ledger` only |
| Chronic **flags** | `SurvivorRadState.HasChronicIllness`, `RespiratoryDegenerationSystem`, `AmputationSystem`, `ChemicalDependencySystem` | domain saves / `survivors` |
| UI compose | `AfflictionsPanel` | not an owner |
| `ChronicConditionSystem` / `HealthHistorySystem` | **ABSENT** | |

Privacy/retention: **undefined**.

---

## 2. Ownership (proposed)

One medical-record owner: **extend diagnosis/pipeline events**, not a new `HealthHistorySystem`. Chronic presentation is **derived from domain flags**. Dose ledger stays radiation-only.

---

## 3. Recommended defaults

| Item | Default |
|---|---|
| Parallel `ChronicConditionSystem` + `HealthHistorySystem` | **OUT** |
| Persisting `PatientRecord` as truth | **OUT** |
| Promoting dose ledger to general health | **OUT** |
| Record = pipeline/diagnosis event log (append-only, bounded) | **IN** (implement later) |
| Chronic UI rows derived from existing flags | **IN** |
| Retention/privacy policy | **IN** as a written rule before any persist of notes |

**Honesty:** non-stigmatizing copy is product language, not a new ledger.

**Next implement:** `DEBT-198-PIPELINE-EVENT-LOG` — bounded append of pipeline events already emitted; no `chronic_conditions.json` until a later amendment.

---

## 4. Evidence paths

`MedicalPipelineCoordinator.cs`, `PatientRecord.cs`, `MedicalPipelineSave.cs`, `DiagnosisKnowledgeStore.cs`, `DoseLedgerSystem.cs`, `RadiationSystem.cs`, `RespiratoryDegenerationSystem.cs`, `src/UI/AfflictionsPanel.cs`, `src/Main.Medical.cs`

**Signed 2026-09-12.** Implement packages listed in this map stay unclaimed until separately promoted.
