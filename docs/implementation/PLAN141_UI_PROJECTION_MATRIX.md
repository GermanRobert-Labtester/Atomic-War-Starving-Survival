# Plan 141 — UI Projection Matrix

## 1. Information Architecture & Presentation Hierarchy

To deliver clinical depth without cluttering the survival UI, Plan 141 organizes medical data into a 3-tier presentation hierarchy:

```
┌─────────────────────────────────────────────────────────────┐
│ 1. PRIMARY TIER — LIVE MECHANICAL STATE (AUTHORITATIVE)    │
├─────────────────────────────────────────────────────────────┤
│ • Condition / Diagnosis Name                                │
│ • Severity, Clinical Stage, and Vital Deficits             │
│ • Validated Pipeline Treatment Previews & Availability      │
│ • On-Hand Medical Supplies & Item Costs                     │
│ • Active Procedure Timers & Mechanical Prognosis           │
└─────────────────────────────────────────────────────────────┘
                               ▲
                               │
┌─────────────────────────────────────────────────────────────┐
│ 2. SECONDARY TIER — AUTHORED CLINICAL CONTEXT (DESCRIPTIVE) │
├─────────────────────────────────────────────────────────────┤
│ • Concise Diagnostic Overview (1 paragraph)                 │
│ • 1–2 Observable Physical Symptoms (Selected Seededly)      │
│ • Complication & Sepsis Warnings                           │
│ • Convalescence & Recovery Context (Active Recovery Only)   │
└─────────────────────────────────────────────────────────────┘
                               ▲
                               │
┌─────────────────────────────────────────────────────────────┐
│ 3. TERTIARY TIER — HISTORICAL CASE ARCHIVE (LORE / CODEX)   │
├─────────────────────────────────────────────────────────────┤
│ • Historical Casebook Profiles (`DwellerMedicalCaseEntry`)  │
│ • Doctor's Clinical Margin Notes & Intervention Summaries   │
│ • Read-Only Reference Case Notes                           │
└─────────────────────────────────────────────────────────────┘
```

---

## 2. Panel Integration Matrix

| Panel | UI Extensibility Seam | Rendered Clinical Elements | Deterministic Selection Key |
|---|---|---|---|
| `MedicalPanel` (`_healthStats` survivor cards) | Sub-row below vital status line (`AshfallUiHelpers.MakeVBox`) | If survivor has an active affliction (e.g. ARS, lung damage, or critical trauma): renders diagnosis overview, selected observable symptom, and complication warning. | `survivor.Id + condition.Id` hash |
| `MedicalPanel` (`_treatmentList` disease ward) | Sub-row inside disease isolation row | When an illness is identified: renders clinical overview and complications. When masked: displays unidentified illness advisory. | `survivor.Id + diseaseId` hash |
| `AfflictionsPanel` (`_activeList`) | Detail sub-row under active affliction label | Diagnosis summary and key observable symptom. | `survivor.Id + afflictionId` hash |
| `AfflictionsPanel` (`_chronicList`) | Detail sub-row under chronic condition label | Long-term functional implications and management context. | `survivor.Id + conditionId` hash |

---

## 3. Deterministic Text Selection Contract

To prevent text flickering and rerolling across panel re-opens, pauses, and unpauses:
1. When multiple symptom descriptions or pain descriptions exist in a condition entry, selection is derived deterministically:
   ```csharp
   int index = Math.Abs((survivorId.GetHashCode() ^ conditionId.GetHashCode())) % symptoms.Count;
   ```
2. No wall-clock time (`DateTime.Now`) or non-deterministic PRNG (`System.Random`) is permitted.
3. Panel reopening produces 100% identical prose output for the same patient and condition state.
