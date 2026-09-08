# Plan 141 — Condition ID Reconciliation Matrix

## 1. Reconciliation Principles

The 83 unique conditions in `medical_texts.json` represent a broad spectrum of clinical situations: active diagnoses, systemic illnesses, localized injuries, surgical procedures, preventative hygiene topics, and chronic complaints.

To prevent identity drift and inaccurate clinical representations:
1. **Direct Map:** Exact semantic 1:1 match with a runtime affliction, disease, or vital deficit.
2. **Alias Map:** Resolves runtime synonyms or specific disease manifestations to authored prose (e.g., `disease_acute_radiation_syndrome` -> `medical_radiation_exposure`).
3. **Reference / Procedure Only:** Entries that describe surgical interventions, hygiene education, or conditions not modeled as distinct runtime diagnoses (e.g., `medical_amputation`, `medical_suturing`, `medical_hygiene`, `medical_toothache`). These are retained in the catalog for contextual lore, archive queries, or future procedure expansions, but are **not** forced onto unrelated runtime states.
4. **No Safe Map:** Conditions that must never be bound to runtime patients without explicit diagnostic systems (e.g., mapping pregnancy to a non-pregnant dweller).

---

## 2. Active Runtime Mappings

| Authored Text ID | Runtime Target ID / Concept | Type | Category | Clinical Rationale & Mapping Policy |
|---|---|---|---|---|
| `medical_radiation_exposure` | `affliction_radiation_sickness` | Direct | `illness` | Maps directly to acute radiation sickness when current dose >= 50 mSv. |
| `medical_radiation_exposure` | `disease_acute_radiation_syndrome` | Alias | `illness` | Maps to the ARS disease entity in `disease_catalog.json`. |
| `medical_chronic_radiation` | `rad.HasChronicIllness` | Direct | `chronic` | Maps to chronic radiation illness when lifetime exposure >= 250 mSv. |
| `medical_asthma` | `affliction_respiratory_degeneration` | Direct | `chronic_disease` | Maps to respiratory degeneration / lung irritation. |
| `medical_addiction` | `affliction_chemical_dependency` | Direct | `chronic` | Maps to active substance dependency ledger entries. |
| `medical_laceration` | `affliction_health_deficit` | Direct | `injury` | Primary trauma / wound descriptor when survivor HP is depleted (< 30 HP). |
| `medical_ptsd` | `affliction_combat_trauma` | Direct | `mental` | Maps to Combat Trauma observe-only psychological projection. |
| `medical_panic_attack` | `affliction_somatic_flashback` | Direct | `mental` | Maps to Somatic Flashback observe-only projection. |
| `medical_insomnia` | `affliction_guilt_insomnia` | Direct | `mental` | Maps to Guilt Insomnia observe-only projection. |
| `medical_wound_infection` | `disease_septic_rust_wound_fever` | Alias | `complication` | Direct clinical match for septic rust wound fever. |
| `medical_infection` | `disease_spore_wound_dermatitis` | Alias | `illness` | Spore dermatitis localized infection profile. |
| `medical_dehydration_severe` | `disease_cholera` | Alias | `emergency` | Severe dehydration / fluid-loss profile for confirmed cholera. |
| `medical_dehydration` | `survivor.Thirst > 70` | Direct | `illness` | Maps to high thirst / dehydration status in survivor vitals. |
| `medical_starvation` | `survivor.Hunger > 70` | Direct | `illness` | Maps to high hunger / acute starvation in survivor vitals. |
| `medical_hypothermia` | Cold hazard exposure | Direct | `illness` | Cold shelter environment or winter weather affliction. |
| `medical_heatstroke` | Heat hazard exposure | Direct | `illness` | Thermal overload or summer heatwave affliction. |

---

## 3. Reference & Procedural Records (Retained, Not Forcibly Mapped)

The following records are preserved in `medical_texts.json` as valid clinical reference definitions. They are accessible via `MedicalTextCatalog.TryGetConditionText(id)` but are intentionally not assigned automatic runtime triggers to prevent false diagnoses:

- **Surgical & Treatment Procedures:**
  `medical_amputation`, `medical_suturing`, `medical_bone_setting`, `medical_wound_debridement`, `medical_physical_therapy`, `medical_counseling`, `medical_medication_management`, `medical_grief_counseling`, `medical_trauma_therapy`, `medical_stress_management`.
- **Preventative Hygiene & Care:**
  `medical_wound_care`, `medical_hygiene`, `medical_nutrition`, `medical_rest`, `medical_blister_care`.
- **Reproductive Health:**
  `medical_pregnancy`, `medical_childbirth`, `medical_newborn_care`.
- **Localized / Non-Modeled Trauma & Bites:**
  `medical_burn_first_degree`, `medical_burn_second_degree`, `medical_burn_third_degree`, `medical_chemical_burn`, `medical_electrical_burn`, `medical_concussion`, `medical_frostbite`, `medical_broken_nose`, `medical_dislocated_shoulder`, `medical_sprained_wrist`, `medical_bruised_ribs`, `medical_cuts_and_scrapes`, `medical_deep_wound`, `medical_puncture_wound`, `medical_avulsion`, `medical_incision`, `medical_laceration_deep`, `medical_insect_bite`, `medical_animal_bite`, `medical_snake_bite`, `medical_spider_bite`, `medical_dog_bite`, `medical_cat_scratch`, `medical_horse_kick`.
- **Systemic Chronic & Nutritional Deficiencies:**
  `medical_diabetes`, `medical_arthritis`, `medical_heart_disease`, `medical_cancer`, `medical_epilepsy`, `medical_chronic_pain`, `medical_chronic_fatigue`, `medical_chronic_headache`, `medical_chronic_back_pain`, `medical_scurvy`, `medical_beriberi`, `medical_pellagra`, `medical_rickets`, `medical_anemia`, `medical_goiter`, `medical_toothache`, `medical_eye_infection`, `medical_malaria`, `medical_tapeworm`, `medical_food_poisoning`, `medical_chemical_poisoning`, `medical_allergic_reaction`.

---

## 4. Adapter Architecture

All mappings are centralized in a single Core class:
`Ashfall.Core.Medical.MedicalConditionResolver`
This ensures:
1. UI panels never maintain hardcoded mapping dictionaries.
2. Resolution is deterministic and covered by automated xUnit tests.
3. If an unmapped or unknown ID is queried, the resolver returns `null`, causing the UI to gracefully omit descriptive prose rather than hallucinating clinical text.
