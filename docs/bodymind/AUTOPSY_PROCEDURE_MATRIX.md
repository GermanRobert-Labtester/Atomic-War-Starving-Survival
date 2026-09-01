# Autopsy Procedure Matrix

The autopsy catalog (`autopsy_procedures.json`) contains 9 distinct forensic and pathological dissection procedures.

| Procedure ID | Display Name | Required Tools | Consumables | Hours | Airborne / Pathogen Risk | Possible Findings | Research Unlocks |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- | :--- |
| `procedure_rad_pathology` | Radiation Pathology | medical_scissors, protective_rubber_gloves, field_surgical_kit | sterilised_bandage, clean_water | 4 | 0.15 / 0.05 | finding_acute_rad_burn, finding_bone_marrow_failure, finding_organ_fibrosis | knowledge_radiation_basics |
| `procedure_toxicology` | Toxicology Screen | medical_scissors, protective_rubber_gloves | bandage, clean_water | 3 | 0.10 / 0.08 | finding_chemical_exposure, finding_organ_damage | knowledge_pathogen_containment |
| `procedure_containment_autopsy` | Containment Autopsy | medical_scissors, protective_rubber_gloves, field_surgical_kit, surgical_mask | sterilised_bandage, clean_water, antibiotics | 6 | 0.30 / 0.20 | finding_pathogen_strain, finding_contamination_source | knowledge_pathogen_containment |
| `procedure_blunt_trauma` | Blunt Force & Crush Forensics | medical_scissors, field_surgical_kit | bandage, clean_water | 3 | 0.05 / 0.02 | finding_crush_fracture, finding_internal_hemorrhage | knowledge_field_trauma_surgery |
| `procedure_ballistic_forensics` | Ballistic & Shrapnel Extraction | medical_scissors, protective_rubber_gloves, field_surgical_kit | bandage, clean_water | 4 | 0.05 / 0.03 | finding_bullet_trajectory, finding_shrapnel_fragment | knowledge_field_trauma_surgery |
| `procedure_respiratory_contamination`| Pulmonary Asbestos & Rad-Dust Screen| medical_scissors, protective_rubber_gloves, surgical_mask | clean_water, sterilised_bandage | 4 | 0.25 / 0.05 | finding_pulmonary_silicosis, finding_rad_dust_inhalation | knowledge_radiation_basics |
| `procedure_hypothermia_pathology` | Severe Hypothermia & Frostbite | medical_scissors, protective_rubber_gloves | clean_water | 3 | 0.02 / 0.02 | finding_cellular_frostbite, finding_vascular_collapse | knowledge_field_trauma_surgery |
| `procedure_spore_infection_isolation`| Fungal Spore & Bio-Contaminant | medical_scissors, protective_rubber_gloves, field_surgical_kit, surgical_mask | sterilised_bandage, clean_water, antibiotics | 5 | 0.35 / 0.25 | finding_mycotoxin_spore, finding_fungal_hyphae | knowledge_pharmacology_synthesis |
| `procedure_poison_biochemical_assay`| Neurotoxin & Heavy Metal Assay | protective_rubber_gloves, field_surgical_kit | clean_water, sterilised_bandage | 5 | 0.15 / 0.10 | finding_organophosphate_toxin, finding_heavy_metal_deposit | knowledge_pharmacology_synthesis |
