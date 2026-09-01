# Autopsy Finding Provenance Contract

Every autopsy finding in ASHFALL must originate from canonical upstream physical, environmental, or narrative state. Procedural RNG cannot invent a new cause of death out of nothing.

| Finding ID | Procedure(s) | Required Upstream State | Certainty Level | Downstream System Effect |
| :--- | :--- | :--- | :--- | :--- |
| `finding_acute_rad_burn` | `procedure_rad_pathology` | `SurvivorRadState.RadiationDose >= 60f` | High | Grants `knowledge_radiation_basics`; updates death record. |
| `finding_bone_marrow_failure`| `procedure_rad_pathology` | `LifetimeRadiationExposure >= 300f` | High | Corroborates Dose Register Red/Black classification. |
| `finding_organ_fibrosis` | `procedure_rad_pathology` | Chronic radiation sickness history | Moderate | Contributes to radio-pathology research knowledge. |
| `finding_chemical_exposure` | `procedure_toxicology` | Gas storm / toxic zone casualty | High | Informs decontamination protocols. |
| `finding_organ_damage` | `procedure_toxicology` | Sepsis / acute poisoning casualty | Moderate | Clinical diagnostic baseline. |
| `finding_pathogen_strain` | `procedure_containment_autopsy`| Active infection at death (`DiseaseSystem`) | High | Identifies pathogen; boosts cure research rate. |
| `finding_contamination_source`| `procedure_containment_autopsy`| Vector infection (water/vermin) | High | Triggers quarantine / source sanitization alert. |
| `finding_crush_fracture` | `procedure_blunt_trauma` | Cave-in / rubble / blunt combat | High | Corroborates collapse incident or blunt violence. |
| `finding_internal_hemorrhage`| `procedure_blunt_trauma` | Severe impact trauma | High | Distinguishes internal bleeding from poisoning. |
| `finding_bullet_trajectory` | `procedure_ballistic_forensics`| Firearm combat death | High | Corroborates engagement distance and angle. |
| `finding_shrapnel_fragment` | `procedure_ballistic_forensics`| Explosive / grenade combat death | High | Yields recoverable scrap / weapon forensics. |
| `finding_pulmonary_silicosis`| `procedure_respiratory_contamination`| Ash dust / demolition exposure | High | Recommends particulate mask upgrades. |
| `finding_rad_dust_inhalation`| `procedure_respiratory_contamination`| Black rain / fallout storm exposure | High | Informs internal chelation therapy protocols. |
| `finding_cellular_frostbite`| `procedure_hypothermia_pathology`| Sub-zero storm exposure casualty | High | Thermal insulation research data. |
| `finding_vascular_collapse` | `procedure_hypothermia_pathology`| Extreme shock / exposure death | Moderate | Medical emergency triage knowledge. |
| `finding_mycotoxin_spore` | `procedure_spore_infection_isolation`| Fungal spore zone / mold infection | High | Unlocks antifungal synthesis recipe. |
| `finding_fungal_hyphae` | `procedure_spore_infection_isolation`| Deep tissue bio-contamination | High | Establishes bio-hazard quarantine tier. |
| `finding_organophosphate_toxin`| `procedure_poison_biochemical_assay`| Authored poisoning scenario | High | **Forensic Case 1:** Produces murder/poison evidence. |
| `finding_heavy_metal_deposit`| `procedure_poison_biochemical_assay`| Industrial water contamination | High | Triggers water filtration overhaul priority. |
