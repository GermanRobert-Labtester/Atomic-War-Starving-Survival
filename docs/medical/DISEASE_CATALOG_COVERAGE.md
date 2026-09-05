# ASHFALL Disease Catalog Coverage Matrix (Plan 63 / B4)

**Document ID:** DOC-MED-P63-001
**Status:** Canonical Reference Specification
**Authority:** `Assets/StreamingAssets/Data/disease_catalog.json` & `Assets/Ashfall.Core/Disease/`
**Associated Plans:** Plan 60 (D1–D5), Plan 63 (B4 Quarantine Policy Loop)

---

## 1. Overview & 8-Stage Clinical Progression Arc

Diseases in ASHFALL follow an authored, staged clinical progression model. Rather than treating infection as a flat timer with a sudden binary death/recovery roll, each disease progresses through discrete, identifiable stages that govern patient capability, symptom presentation, contagion risk, and care response windows.

```text
[ Exposure & Infection ]
         ↓
    Incubating        (Asymptomatic, dormant colonization, 0% shedding)
         ↓
    Prodromal         (Early warning signs, tell emerges, low shedding ~20%)
         ↓
    Symptomatic       (Full clinical expression, actively shedding 100%, treatable window)
         ↓
    Severe            (Aggressive systemic impact, high shedding 120%, bedridden, palliative care)
         ↓
    Critical          (Organ compromise / toxic shock, triage urgency high, heavy sedation floor)
        ↙ ↘
Recovering   Terminal (Organ failure / irreversible collapse → death record)
    ↓            ↓
Recovered    Memorial (Canonical SurvivorFate handoff, exactly once)
    ↓
Temporary Immunity (Persisted resistance window: e.g. 14–30 days)
```

### Stage Definitions & Clinical Contracts

| Stage | Clinical State | Contagion | Labor Capacity | Care Response Window |
|---|---|---|---|---|
| **Incubating** | Pathogen colonizes tissue; patient feels normal. | 0% | 100% | Prophylactic only. |
| **Prodromal** | First tells emerge (headache, slight rasp, low nausea). | 15–25% | 75% | Optimal intervention window. |
| **Symptomatic** | Diagnostic signs clear; acute discomfort; coughing/diarrhea. | 100% | 40% | Curative antimicrobials active. |
| **Severe** | Bedridden; high fever; breathing difficulty; dehydration. | 120% | 0% (Bed bound) | Intensive supportive care needed. |
| **Critical** | Sepsis, shock, cyanosis, delirium; terminal risk high. | 80% (Failing host) | 0% | Heavy sedation / life support. |
| **Recovering** | Fever broken; immune system clearing debris; weak. | 15% | 25% (Light duties only) | Nutritional support & rest. |
| **Recovered** | Infection cleared; full vitality restored; immunity active. | 0% | 100% | Gains temporary immunity window. |
| **Terminal** | Inevitable demise; palliative comfort care only. | 0% | 0% | Routes to SurvivorFate ledger. |

---

## 2. Complete Disease Coverage Inventory (16 Diseases)

| ID | Name | Vector | Incubation | Total Days | Contagious Stages | Lethality | Care & Treatment Paths | Immunity Window | Bridges & Sources |
|---|---|---|---|---|---|---|---|---|---|
| `disease_cholera` | Cholera | Water | 2d | 4d | Prodromal, Symptomatic, Severe | 0.30 | `antibiotics` (curative -0.25), `clean_water` (supportive -0.05) | 21 days | Water treatment, Cisterns |
| `disease_zoonotic_flu` | Zoonotic Flu | Air | 1d | 5d | Prodromal, Symptomatic, Severe | 0.18 | `herbal_tea` (symptomatic), `medical_kit` (supportive -0.10) | 28 days | Wildlife butchery, Autopsy |
| `disease_blood_fever` | Blood Fever | Blood | 3d | 6d | Symptomatic, Severe, Critical | 0.45 | `antibiotics` (curative -0.30), `bandage` (symptomatic) | 30 days | Surgery, Needles, Trauma |
| `disease_spore_blight` | Spore Blight | Spore | 2d | 7d | Symptomatic, Severe, Critical | 0.40 | `inhaler` (suppressive -0.10), `medical_kit` (supportive -0.10) | 14 days | Mold farming, Dust storms |
| `disease_acute_radiation_syndrome` | Acute Rad Syndrome | Water | 0d | 14d | None (Non-communicable) | 0.80 | `rad_away` (-0.20), `item_prussian_blue_chelating_pellets` (-0.25), `item_thiamine_dose` (-0.05) | None (0 days) | Fallout rain, Hot water draw |
| `disease_fungal_respiratory` | Fungal Respiratory | Air | 5d | 10d | Symptomatic, Severe | 0.30 | `inhaler` (symptomatic -0.05), `medical_kit` (supportive -0.10) | 14 days | Unfiltered bunker vents |
| `disease_typhoid_waterborne` | Typhoid | Water | 3d | 8d | Prodromal, Symptomatic, Severe | 0.50 | `antibiotics` (curative -0.25), `clean_water` (supportive -0.05) | 30 days | Stagnant sump pipes |
| `disease_wellspring_cramps` | Wellspring Cramps | Water | 2d | 4d | Symptomatic | 0.20 | `antibiotics` (curative -0.20), `herbal_tea` (symptomatic) | 14 days | Coastal flood draw |
| `disease_silt_jaundice` | Silt Jaundice | Water | 7d | 14d | Symptomatic, Severe | 0.55 | `antibiotics` (curative -0.20), `medical_kit` (supportive -0.10) | 21 days | Rodent-fouled deep wells |
| `disease_condemned_air_cough` | Condemned Air Cough | Air | 4d | 9d | Prodromal, Symptomatic, Severe | 0.25 | `inhaler` (symptomatic -0.05), `herbal_tea` (supportive -0.05) | 21 days | Scavenged dead barns |
| `disease_dry_bunker_hiss` | Dry-Bunker Hiss | Air | 4d | 12d | Symptomatic, Severe | 0.15 | `inhaler` (symptomatic -0.10) | 14 days | Cold recirculated ventilation |
| `disease_septic_rust_wound_fever` | Septic Rust-Wound Fever | Blood | 2d | 7d | Symptomatic, Severe, Critical | 0.40 | `antibiotics` (curative -0.30), `antiseptic_1l_of_1l` (-0.10) | 14 days | Scrap salvage lacerations |
| `disease_reused_needle_fever` | Reused-Needle Fever | Blood | 1d | 5d | Symptomatic, Severe | 0.20 | `antibiotics` (curative -0.25), `medical_kit` (supportive -0.10) | 14 days | Field injections, IV lines |
| `disease_deep_excavation_mold_lung` | Deep Excavation Mold Lung | Spore | 6d | 16d | Symptomatic, Severe, Critical | 0.65 | `inhaler` (suppressive -0.10), `medical_kit` (supportive -0.10) | 14 days | Deep shelter excavation |
| `disease_silo_lung` | Silo Lung | Spore | 9d | 11d | Symptomatic, Severe | 0.30 | `inhaler` (suppressive -0.15), `field_surgical_kit` (-0.05) | 21 days | Compacted grain storage |
| `disease_prion_tremor` | Prion Tremor Syndrome | Blood | 14d | 21d | None (Non-communicable) | 0.85 | `herbal_tea` (palliative comfort -0.05) | None (0 days) | Taboo cannibal rations |

---

## 3. Transmission Vectors & Protocol Countermeasures

Transmission vectors are bounded and require active protocols to suppress:

1. **Water (`water`):** Countermeasure: `clean_water` (Boiling / filtration protocol). Duration: 3 days.
2. **Air (`air`):** Countermeasure: `gas_mask` (Sealing vents / mask rotation). Duration: 2 days.
3. **Blood (`blood`):** Countermeasure: `antibiotics` / `antiseptic_1l_of_1l` (Sterilization of blades/tools). Duration: 5 days.
4. **Spore (`spore`):** Countermeasure: `hazmat_suit` (HEPA air filtration). Duration: 4 days.

Protocols are active maintenance, not toggles: when their duration expires, vectors reopen unless renewed with fresh consumables.

---

## 4. Temporary Immunity Rules & Save Compatibility

1. **Immunity Acquisition:** Survivors who successfully recover from a disease gain temporary immunity (`immunity_duration_days`, typically 14 to 30 days) with `immunity_strength` (typically 0.80 to 1.0).
2. **Immunity Query:** `HasImmunity(survivorId, diseaseId, currentDay)` checks if currentDay < `immunityUntilDay`.
3. **Immunity Evaluation:** If a survivor is exposed while immune, `rng.NextDouble() < immunity_strength` prevents infection.
4. **Persistence Invariant:** `DiseaseSystemState` serializes `immunities` in an ordinal list. Saves without immunities load safely with an empty list. Restoring an immunity record never fires notifications or audio.

---

## 5. Cross-System Source Tuning (Data-Driven Exposure)

Exposure probabilities are moved out of C# code and into `exposure_sources` in `disease_catalog.json`:

```json
"exposure_sources": [
  {
    "source_id": "wildlife_butchery",
    "disease_id": "disease_zoonotic_flu",
    "base_probability": 0.30,
    "mitigating_trait_id": "skill_sanitization_expert"
  },
  {
    "source_id": "autopsy_pathogen",
    "disease_id": "disease_zoonotic_flu",
    "base_probability": 0.25,
    "mitigating_trait_id": "skill_sanitization_expert"
  },
  {
    "source_id": "micro_hazard_contamination",
    "disease_id": "disease_zoonotic_flu",
    "base_probability": 1.00,
    "mitigating_trait_id": ""
  },
  {
    "source_id": "foul_water_draw",
    "disease_id": "disease_cholera",
    "base_probability": 0.40,
    "mitigating_trait_id": ""
  }
]
```
