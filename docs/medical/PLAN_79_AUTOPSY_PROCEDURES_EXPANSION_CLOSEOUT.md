# Plan 79 — Autopsy Procedures Expansion (3 → 12) — Closeout Report

> **Mission Complete:** Expanded `autopsy_procedures.json` into a complete twelve-procedure post-mortem investigation catalog covering the full spectrum of survival death families in ASHFALL, supported by 100% item resolution, validated findings, and canonical research unlocks.

---

## 1. Executive Summary

- **Plan:** 79 — Autopsy Procedures Expansion
- **Baseline:** 3 procedures (`procedure_rad_pathology`, `procedure_toxicology`, `procedure_containment_autopsy`)
- **Intermediate Baseline:** 9 procedures (Plan 27 added blunt trauma, ballistics, respiratory, hypothermia, spore, and poison assay)
- **Final Roster:** 12 procedures (added deprivation pathology, blast overpressure trauma, and forensic suspicious death inquest)
- **Hard Constraints Honored:** Pure DATA plan. Zero Core code added or changed. Zero save schema changes.

---

## 2. Complete Twelve-Procedure Roster

| # | Procedure ID | Display Name | Air Risk | Pathogen Risk | Hours | Tools | Consumables | Research Unlocks |
|---|---|---|:---:|:---:|:---:|---|---|---|
| 1 | `procedure_rad_pathology` | Radiation Pathology | 0.15 | 0.05 | 4 | `medical_scissors`, `protective_rubber_gloves`, `field_surgical_kit` | `sterilised_bandage`, `clean_water` | `knowledge_radiation_basics` |
| 2 | `procedure_toxicology` | Toxicology Screen | 0.10 | 0.08 | 3 | `medical_scissors`, `protective_rubber_gloves` | `bandage`, `clean_water` | `knowledge_pathogen_containment` |
| 3 | `procedure_containment_autopsy` | Containment Autopsy | 0.30 | 0.20 | 6 | `medical_scissors`, `protective_rubber_gloves`, `field_surgical_kit`, `surgical_mask` | `sterilised_bandage`, `clean_water`, `antibiotics` | `knowledge_pathogen_containment` |
| 4 | `procedure_blunt_trauma` | Blunt Force & Crush Forensics | 0.05 | 0.02 | 3 | `medical_scissors`, `field_surgical_kit` | `bandage`, `clean_water` | `knowledge_field_trauma_surgery` |
| 5 | `procedure_ballistic_forensics` | Ballistic & Shrapnel Extraction Forensics | 0.05 | 0.03 | 4 | `medical_scissors`, `protective_rubber_gloves`, `field_surgical_kit` | `bandage`, `clean_water` | `knowledge_field_trauma_surgery` |
| 6 | `procedure_respiratory_contamination` | Pulmonary Asbestos & Rad-Dust Screen | 0.25 | 0.05 | 4 | `medical_scissors`, `protective_rubber_gloves`, `surgical_mask` | `clean_water`, `sterilised_bandage` | `knowledge_radiation_basics` |
| 7 | `procedure_hypothermia_pathology` | Severe Hypothermia & Frostbite Pathology | 0.02 | 0.02 | 3 | `medical_scissors`, `protective_rubber_gloves` | `clean_water` | `knowledge_field_trauma_surgery` |
| 8 | `procedure_spore_infection_isolation` | Fungal Spore & Bio-Contaminant Isolation | 0.35 | 0.25 | 5 | `medical_scissors`, `protective_rubber_gloves`, `field_surgical_kit`, `surgical_mask` | `sterilised_bandage`, `clean_water`, `antibiotics` | `knowledge_pharmacology_synthesis` |
| 9 | `procedure_poison_biochemical_assay` | Neurotoxin & Heavy Metal Assay | 0.15 | 0.10 | 5 | `protective_rubber_gloves`, `field_surgical_kit` | `clean_water`, `sterilised_bandage` | `knowledge_pharmacology_synthesis` |
| 10 | `procedure_deprivation_pathology` | Severe Starvation & Dehydration Pathology | 0.02 | 0.02 | 3 | `medical_scissors`, `scalpel`, `forceps` | `clean_water`, `bandage` | `knowledge_food_preservation` |
| 11 | `procedure_blast_overpressure_trauma` | Blast Overpressure & Barotrauma Forensics | 0.05 | 0.03 | 4 | `medical_scissors`, `protective_rubber_gloves`, `field_surgical_kit` | `sterilised_bandage`, `clean_water` | `knowledge_field_trauma_surgery` |
| 12 | `procedure_forensic_inquest_suspicious` | Forensic Inquest & Suspicious Death Examination | 0.15 | 0.10 | 6 | `medical_scissors`, `protective_rubber_gloves`, `field_surgical_kit`, `surgical_mask` | `sterilised_bandage`, `clean_water` | `knowledge_pathogen_containment` |

---

## 3. Forensic Finding Coverage

All 12 procedures define 2–3 distinct findings, registered in `possible_findings`:
- `finding_acute_rad_burn`, `finding_bone_marrow_failure`, `finding_organ_fibrosis`
- `finding_chemical_exposure`, `finding_organ_damage`
- `finding_pathogen_strain`, `finding_contamination_source`
- `finding_crush_fracture`, `finding_internal_hemorrhage`
- `finding_bullet_trajectory`, `finding_shrapnel_fragment`
- `finding_pulmonary_silicosis`, `finding_rad_dust_inhalation`
- `finding_cellular_frostbite`, `finding_vascular_collapse`
- `finding_mycotoxin_spore`, `finding_fungal_hyphae`
- `finding_organophosphate_toxin`, `finding_heavy_metal_deposit`
- `finding_hepatic_lipidosis`, `finding_cachexia_atrophy`, `finding_electrolyte_dehydration`
- `finding_blast_lung_barotrauma`, `finding_tympanic_perforation`, `finding_visceral_concussion`
- `finding_concealed_smothering`, `finding_ligature_strangulation`, `finding_occult_neurotoxin`

---

## 4. Item Resolution & Schema Conformance

- Merged canonical definitions from `tools/item_text_batches/items_out_batch_3.json` into `Assets/StreamingAssets/Data/items.json`:
  - `medical_scissors` (stackMax: 5, weight: 0.2 kg, wear: 80)
  - `protective_rubber_gloves` (stackMax: 5, weight: 0.2 kg, wear: 40)
  - `sterilised_bandage` (stackMax: 10, weight: 0.1 kg, healthEffect: 25)
- All 10 items referenced across all 12 procedures exist and resolve in `items.json`.

---

## 5. Verification Matrix Results

| Verification Gate | Command | Result | Notes |
|---|---|---|---|
| **C# Build** | `dotnet build Ashfall.csproj` | **PASS** | 0 warnings, 0 errors |
| **xUnit Unit Tests** | `dotnet test Ashfall.Core.Tests` | **PASS** | **6,756 passed, 0 failed, 0 skipped** (incl. 12 new tests in `AutopsyProceduresCatalogTests`) |
| **Data Integrity** | `godot --headless --path . -- --data-integrity-selftest` | **PASS** | 0 errors across 208 catalogs (10,698 IDs verified) |
| **Content Utilization** | `godot --headless --path . -- --content-utilization-selftest` | **PASS** | CI gate PASS |
| **Scene Binding** | `godot --headless --path . -- --scene-binding-selftest` | **PASS** | 22/22 production panels passed |
| **Shelter Operations Demo** | `godot --headless --path . -- --shelter-operations-selftest` | **PASS** | Full medical triage, expedition & crafting demo passed |
| **Scene Lint** | `python3 scripts/ci/scene-lint.py` | **PASS** | 27 production scenes checked, 0 errors |
