# Survivor Enrichment ID Matrix

## 1. Overview
This matrix documents the canonical mapping between all 76 survivor definitions (`survivors.json`) and the enrichment layers (`expansion_survivor_fields.json`, `deep_lore_survivor_fields.json`, and `antigravity_survivor_fields.json`).

---

## 2. Deep Lore Survivors (Tier 2 Authority)
| Survivor ID | Pre-War Profession | Belief Profile | Personal Keepsake | Phantom Background | Philosophical Stance |
|---|---|---|---|---|---|
| `aris_thorne` | `machinist` | `atheist_rationalist` | `blueprint_roll` | `machinist` | `material_rationalism` |
| `maya_lin` | *(def.profession)* | `collectivist_solidarity` | `radio_headset` | `generic` | `stoicism` |
| `victor_vance` | *(def.profession)* | `military_discipline` | `service_pistol` | `former_soldier` | `stoicism` |
| `elena_rostov` | `nurse` | `atheist_rationalist` | `surgical_mask` | `nurse` | `material_rationalism` |

---

## 3. Archetype Specialists (Tier 3 Overlay)
| Survivor ID | Base Belief Profile | Phantom Background | Keepsake Item | Stance | Manifesto Law Code |
|---|---|---|---|---|---|
| `the_veteran` | `military_discipline` | `former_soldier` | `dog_tags` | `stoicism` | `military_discipline_code` |
| `the_priest` | `religious_faith` | `generic` | `wooden_cross` | `faith` | `moral_code` |
| `the_surgeon` | `atheist_rationalist` | `nurse` | `silver_scalpel` | `material_rationalism` | `medical_ethics_code` |
| `the_cop` | `military_discipline` | `generic` | `badge` | `stoicism` | `justice_code` |
| `the_pacifist` | `pacifist` | `generic` | `pressed_flower` | `faith` | — |
| `the_chemist` | `atheist_rationalist` | `generic` | `glass_vial` | `material_rationalism` | — |
| `the_martyr` | `religious_faith` | `generic` | `burned_scripture` | `faith` | — |
| `the_misanthrope` | `pragmatic_individualism`| `generic` | `locked_journal` | `material_rationalism` | — |
| `the_prepper` | `pragmatic_individualism`| `generic` | `can_opener` | `stoicism` | — |
| `the_general` | `military_discipline` | `former_soldier` | `service_ribbon` | `stoicism` | `military_discipline_code` |
| `the_sheriff` | `military_discipline` | `generic` | `whistle` | `stoicism` | `justice_code` |

---

## 4. Key Representative Baseline Survivors (Tier 1 Authority)
| Survivor ID | Pre-War Profession | Belief Profile | Personal Keepsake Item | Phantom Background |
|---|---|---|---|---|
| `elena_vasquez` | `nurse` | `collectivist_solidarity` | `worn_stethoscope` | `nurse` |
| `marcus_olejnik` | `machinist` | `pragmatic_individualism` | `tarnished_pocket_watch`| `machinist` |
| `suki_tanaka` | *(def.profession)* | `superstitious_traditional` | `family_heirloom_seeds`| `generic` |
| `the_pharmacist` | *(def.profession)* | `atheist_rationalist` | `mortar_and_pestle` | `nurse` |
| `the_vet` | *(def.profession)* | `pragmatic_individualism` | `dog_whistle` | `nurse` |
| `the_therapist` | *(def.profession)* | `collectivist_solidarity` | `leather_notebook` | `generic` |
| `the_undertaker`| *(def.profession)* | `religious_faith` | `funeral_program` | `generic` |
| `dmitri_volkov` | `machinist` | `pragmatic_individualism` | `pipe_wrench` | `machinist` |
| `nikolai_fedorov` | `electrician` | `atheist_rationalist` | `wedding_ring` | `electrician` |
| `lydia_karpova` | `teacher` | `pacifist` | `teddy_bear` | `teacher` |
| `tatyana_voronova`| `teacher` | `collectivist_solidarity` | `childs_drawing` | `child_refugee` |
| `vera_mikhailov` | *(def.profession)* | `superstitious_traditional` | `family_heirloom_seeds`| `generic` |

---

## 5. Roster Aggregate Coverage
- Total Canon Survivors: 76 (100% enriched across Tier 1 and Tier 2).
- Zero Orphan Entries: Every ID exists in `survivors.json`.
- Zero Null Fields: Unspecified professions fall back cleanly to `def.profession`.
