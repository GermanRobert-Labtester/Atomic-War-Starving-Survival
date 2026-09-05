# Plan 79 — Autopsy Coverage Matrix

> **Catalog Scope:** Full 12-procedure clinical and pathological post-mortem investigation matrix.

---

## 1. Complete Twelve-Procedure Roster

| # | Procedure ID | Display Name | Diagnostic Domain | Tools | Consumables | Air Risk | Pathogen Risk | Hours | Research Unlocks |
|---|---|---|---|---|---|:---:|:---:|:---:|---|
| 1 | `procedure_rad_pathology` | Radiation Pathology | Radiation Sickness & Acute Burns | `medical_scissors`, `protective_rubber_gloves`, `field_surgical_kit` | `sterilised_bandage`, `clean_water` | 0.15 | 0.05 | 4 | `knowledge_radiation_basics` |
| 2 | `procedure_toxicology` | Toxicology Screen | Contaminated Water, Food, & Toxins | `medical_scissors`, `protective_rubber_gloves` | `bandage`, `clean_water` | 0.10 | 0.08 | 3 | `knowledge_pathogen_containment` |
| 3 | `procedure_containment_autopsy` | Containment Autopsy | High-Risk Contagious Pathogens | `medical_scissors`, `protective_rubber_gloves`, `field_surgical_kit`, `surgical_mask` | `sterilised_bandage`, `clean_water`, `antibiotics` | 0.30 | 0.20 | 6 | `knowledge_pathogen_containment` |
| 4 | `procedure_blunt_trauma` | Blunt Force & Crush Forensics | Falls, Cave-ins, & Structural Collapse | `medical_scissors`, `field_surgical_kit` | `bandage`, `clean_water` | 0.05 | 0.02 | 3 | `knowledge_field_trauma_surgery` |
| 5 | `procedure_ballistic_forensics` | Ballistic & Shrapnel Extraction Forensics | Gunshot Wounds & Shrapnel Channels | `medical_scissors`, `protective_rubber_gloves`, `field_surgical_kit` | `bandage`, `clean_water` | 0.05 | 0.03 | 4 | `knowledge_field_trauma_surgery` |
| 6 | `procedure_respiratory_contamination` | Pulmonary Asbestos & Rad-Dust Screen | Inhaled Ash, Rad-Dust, & Filtration Failure | `medical_scissors`, `protective_rubber_gloves`, `surgical_mask` | `clean_water`, `sterilised_bandage` | 0.25 | 0.05 | 4 | `knowledge_radiation_basics` |
| 7 | `procedure_hypothermia_pathology` | Severe Hypothermia & Frostbite Pathology | Extreme Winter Exposure & Frostbite | `medical_scissors`, `protective_rubber_gloves` | `clean_water` | 0.02 | 0.02 | 3 | `knowledge_field_trauma_surgery` |
| 8 | `procedure_spore_infection_isolation` | Fungal Spore & Bio-Contaminant Isolation | Spore Zone Inhalation & Mycosis | `medical_scissors`, `protective_rubber_gloves`, `field_surgical_kit`, `surgical_mask` | `sterilised_bandage`, `clean_water`, `antibiotics` | 0.35 | 0.25 | 5 | `knowledge_pharmacology_synthesis` |
| 9 | `procedure_poison_biochemical_assay` | Neurotoxin & Heavy Metal Assay | Chemical Weapons & Organophosphates | `protective_rubber_gloves`, `field_surgical_kit` | `clean_water`, `sterilised_bandage` | 0.15 | 0.10 | 5 | `knowledge_pharmacology_synthesis` |
| 10 | `procedure_deprivation_pathology` | Severe Starvation & Dehydration Pathology | Extreme Malnutrition & Dehydration | `medical_scissors`, `scalpel`, `forceps` | `clean_water`, `bandage` | 0.02 | 0.02 | 3 | `knowledge_food_preservation` |
| 11 | `procedure_blast_overpressure_trauma` | Blast Overpressure & Barotrauma Forensics | Explosion Overpressure & Concussion | `medical_scissors`, `protective_rubber_gloves`, `field_surgical_kit` | `sterilised_bandage`, `clean_water` | 0.05 | 0.03 | 4 | `knowledge_field_trauma_surgery` |
| 12 | `procedure_forensic_inquest_suspicious` | Forensic Inquest & Suspicious Death Examination | Mixed Trauma, Concealed Foul Play, Murder | `medical_scissors`, `protective_rubber_gloves`, `field_surgical_kit`, `surgical_mask` | `sterilised_bandage`, `clean_water` | 0.15 | 0.10 | 6 | `knowledge_pathogen_containment` |

---

## 2. Distinct Forensic Boundaries

- **Ballistic vs. Blast vs. Blunt:**
  - `procedure_ballistic_forensics` investigates penetrating projectile channels and retained metal fragments.
  - `procedure_blast_overpressure_trauma` investigates blast-wave overpressure (tympanic rupture, pulmonary barotrauma, visceral concussions) where penetrating wounds may be absent.
  - `procedure_blunt_trauma` investigates crush fractures, cave-in compression, and internal hemorrhage.
- **Toxicology vs. Chemical/Neurotoxin vs. Suspicious Inquest:**
  - `procedure_toxicology` screens for contaminated water and spoiled food pathogens.
  - `procedure_poison_biochemical_assay` performs deep organ assays for pesticides, synthetic neurotoxins, and heavy metals.
  - `procedure_forensic_inquest_suspicious` conducts a full forensic inquest to differentiate concealed homicide (ligature, smothering) from accidental death.
- **Containment vs. Spore vs. Deprivation:**
  - `procedure_containment_autopsy` is high-containment dissection for aggressive infectious outbreaks.
  - `procedure_spore_infection_isolation` isolates deep-tissue fungal hyphae and mycotoxins from spore-infested wasteland flora.
  - `procedure_deprivation_pathology` investigates lipidosis and organ failure from prolonged rationing or drought.
