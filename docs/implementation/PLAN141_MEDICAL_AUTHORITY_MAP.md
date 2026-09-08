# Plan 141 — Medical Authority Map

## 1. System Responsibility & Governance Boundaries

Plan 141 establishes a strict separation between **Simulation Authority** (Core game systems that calculate and mutate state) and **Presentation Authority** (descriptive prose, diagnostic context, and lore documents).

```
┌─────────────────────────────────────────────────────────────┐
│                 SIMULATION AUTHORITY (CORE)                 │
├──────────────────────────┬──────────────────────────────────┤
│ SurvivorsHostSession     │ Health, hunger, thirst, stamina  │
│ DiseaseSystem            │ Vector, incubation, lethality    │
│ RadiationSystem / Dose   │ Exposure, acute/chronic dose mSv │
│ RespiratorySystem        │ Lung degradation, permanent fx   │
│ ChemicalDependencySystem │ Addiction level, detox programs  │
│ MedicalPipelineCoord.    │ Legality, atomic reserve/consume │
│ InventorySystem          │ Supply counts, item ownership    │
└──────────────────────────┴──────────────────────────────────┘
                              │
                              │ (Read-Only Projection)
                              ▼
┌─────────────────────────────────────────────────────────────┐
│                 PRESENTATION AUTHORITY (P141)               │
├──────────────────────────┬──────────────────────────────────┤
│ MedicalTextCatalog       │ Clinical condition prose,        │
│                          │ symptoms, complication warnings  │
│ DwellerMedicalCatalog    │ Historical case records & lore   │
│ MedicalConditionResolver │ Condition ID to prose adapter    │
└──────────────────────────┴──────────────────────────────────┘
                              │
                              │ (Renders Text & Widgets)
                              ▼
┌─────────────────────────────────────────────────────────────┐
│                     UI HOST (GODOT)                         │
├──────────────────────────┬──────────────────────────────────┤
│ MedicalPanel             │ Survivor cards, treatment btns,  │
│                          │ clinical summary & diagnosis     │
│ AfflictionsPanel         │ Active/chronic rows & context    │
└──────────────────────────┴──────────────────────────────────┘
```

---

## 2. Authority Matrix

| Concern | Primary Authority | Plan 141 Role | Prohibited Actions in Plan 141 |
|---|---|---|---|
| **Patient Identity & Vitals** | `SurvivorsHostSession` / `Survivor` | Read-only | Cannot alter HP, hunger, thirst, or survivor status. |
| **Diagnosis Knowledge** | `DiagnosisKnowledgeStore` | Selects text based on status (`suspected` vs `confirmed`) | Cannot confirm, suspect, or rule out a diagnosis. |
| **Disease Simulation** | `DiseaseSystem` (`disease_catalog.json`) | Read-only | Cannot modify vector, incubation, spread, or lethality. |
| **Radiation & Dosimetry** | `RadiationSystem` / `DoseLedgerSystem` | Read-only | Cannot grant rad resistance, alter mSv dose, or heal radiation. |
| **Respiratory Health** | `RespiratoryDegenerationSystem` | Read-only | Cannot change lung damage percentage or inhaler timer. |
| **Chemical Dependency** | `ChemicalDependencySystem` | Read-only | Cannot alter addiction level or change detox duration. |
| **Treatment Eligibility** | `MedicalPipelineCoordinator` | Read-only preview | Cannot bypass contraindications or invent treatments. |
| **Treatment Execution** | `MedicalPipelineCoordinator` + `Inventory` | No ownership | Cannot consume items or apply medical interventions directly. |
| **Treatment Probability** | Authoritative simulation math | No ownership | Must NOT display decorative JSON percentages as runtime odds. |
| **Inventory Supplies** | `InventorySystem` | Display counts only | Cannot add, remove, or modify inventory items. |
| **Condition Prose** | `MedicalTextCatalog` (Plan 141) | **Presentation Authority** | Authoritative provider of diagnosis text, symptoms, and warnings. |
| **Casebook Records** | `DwellerMedicalCatalog` (Plan 141) | **Presentation / Lore Authority** | Authoritative provider of historical patient case records. |
| **UI Presentation** | `MedicalPanel` / `AfflictionsPanel` | Renderer only | Cannot compute game logic or maintain shadow copies. |

---

## 3. The Core Invariant

> **INVARIANT 4.1:**
> Deleting all Plan-141 prose files (`medical_texts.json`, `dweller_medical_casebook.json`, `medical_documents_expansion.json`) from an otherwise valid installation may degrade visual and descriptive presentation, but **MUST NOT** change medical outcomes, treatment success rates, survivor survival, or campaign progression.
