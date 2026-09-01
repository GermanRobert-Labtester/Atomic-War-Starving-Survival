# Autopsy Knowledge Matrix

> **Document Status:** Authoritative Forensic Pathology Catalog
> **Project:** ASHFALL (Godot 4.7+ / .NET 8 / C# Core)
> **Date:** September 2026

---

## 1. 9 Authoritative Autopsy Procedures

Each autopsy procedure in `Assets/StreamingAssets/Data/autopsy_procedures.json` consumes tools/consumables, carries biological & airborne risks, reveals forensic findings, and unlocks scientific and medical research knowledge.

| Procedure ID | Display Name | Duration | Airborne Risk | Pathogen Risk | Unlocked Knowledge Node | Example Forensic Findings |
|---|---|---|---|---|---|---|
| `procedure_rad_pathology` | Radiation Pathology | 4 hrs | 15% | 5% | `knowledge_radiation_basics` | `finding_acute_rad_burn`, `finding_bone_marrow_failure` |
| `procedure_toxicology` | Toxicology Screen | 3 hrs | 10% | 8% | `knowledge_pathogen_containment` | `finding_chemical_exposure`, `finding_organ_damage` |
| `procedure_containment_autopsy` | Containment Autopsy | 6 hrs | 30% | 20% | `knowledge_pathogen_containment` | `finding_pathogen_strain`, `finding_contamination_source` |
| `procedure_blunt_trauma` | Blunt Force & Crush Forensics | 3 hrs | 5% | 2% | `knowledge_field_trauma_surgery` | `finding_crush_fracture`, `finding_internal_hemorrhage` |
| `procedure_ballistic_forensics` | Ballistic & Shrapnel Extraction | 4 hrs | 5% | 3% | `knowledge_field_trauma_surgery` | `finding_bullet_trajectory`, `finding_shrapnel_fragment` |
| `procedure_respiratory_contamination` | Pulmonary Asbestos & Rad-Dust | 4 hrs | 25% | 5% | `knowledge_radiation_basics` | `finding_pulmonary_silicosis`, `finding_rad_dust_inhalation` |
| `procedure_hypothermia_pathology` | Severe Hypothermia & Frostbite | 3 hrs | 2% | 2% | `knowledge_field_trauma_surgery` | `finding_cellular_frostbite`, `finding_vascular_collapse` |
| `procedure_spore_infection_isolation` | Fungal Spore & Bio-Contaminant | 5 hrs | 35% | 25% | `knowledge_pharmacology_synthesis` | `finding_mycotoxin_spore`, `finding_fungal_hyphae` |
| `procedure_poison_biochemical_assay` | Neurotoxin & Heavy Metal Assay | 5 hrs | 15% | 10% | `knowledge_pharmacology_synthesis` | `finding_organophosphate_toxin`, `finding_heavy_metal_deposit` |
