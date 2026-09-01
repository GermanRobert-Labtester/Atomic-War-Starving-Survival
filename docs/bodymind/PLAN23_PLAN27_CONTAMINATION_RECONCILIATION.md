# Plan 23 & Plan 27 Psychological Contamination Reconciliation

## 1. Context & Scope Decision
- **Scope Decision:** **Scope C** (Maritime / disaster location sources, general downstream effects to trauma, insomnia, and morale).
- Plan 23 (The Black Flotilla) establishes maritime deep-dive dread for submerged wrecks, air narcosis, and confined dark compartments.
- Plan 27 reconciles these sources so that land disaster sites (`location_sunshine_daycare`, `location_automated_abattoir`, `location_stadium_evacuation_center`, `location_quarantine_mile`, `location_regional_blood_bank`) and maritime deep wrecks use the same unified `PsychologicalContaminationSystem` API without duplicating IDs or introducing competing systems.

---

## 2. Reconciled Source & ID Registry

| Source ID | Location / Context | Contamination Type | Exposure Trigger | Threshold & Consequence |
| :--- | :--- | :--- | :--- | :--- |
| `source_stadium_mass_grave` | `location_stadium_evacuation_center` | `contam_thousand_yard_stare` | Scavenging stadium triage grounds. | Blocks teaching/storytelling for 3 days; triggers silence chronicle. |
| `source_automated_abattoir` | `location_automated_abattoir` | `contam_disgust_cascade`, `contam_phantom_smell` | Exploring mechanized meat plant. | Blocks cooking/hydroponics for 2 days; olfactory nausea for 5 days. |
| `source_sunshine_daycare` | `location_sunshine_daycare` | `contam_child_cot_trauma` | Exploring ruined nursery/daycare. | Blocks child comforting/teaching for 4 days; red coat chronicle. |
| `source_quarantine_mile` | `location_quarantine_mile` | `contam_thousand_yard_stare` | Traversing execution checkpoint. | Blocks storytelling; triggers mental break if assigned to autopsy. |
| `source_regional_blood_bank`| `location_regional_blood_bank` | `contam_disgust_cascade`, `contam_phantom_smell` | Searching ruined medical storage. | Nausea and cooking refusal for 2 days. |
| `source_deep_wreck_interior`| `location_deep_cargo_hold` / Deep Dive | `contam_claustrophobic_dread` | Extended immersion in submerged hull. | Elevated air consumption; staffing recommendation against repeat dive. |

---

## 3. Reconciliation Rules
1. **Preserve Shipped Types:** `contam_thousand_yard_stare`, `contam_disgust_cascade`, `contam_phantom_smell`, `contam_child_cot_trauma` are canonical constants in `PsychologicalContaminationSystem.cs`.
2. **No Duplication:** Deep-dive missions query `PsychologicalContaminationSystem` directly when survivors navigate flooded compartments.
