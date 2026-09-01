# Plan 27 Baseline Inventory & Verified Evidence

## 1. Verified Baseline Starting State

### Dose Content
- `dose_registers.json`: 4 bands (`band_green`, `band_amber`, `band_red`, `band_black`), 3 plans (`plan_morphine_tray`, `plan_comfort_rounds`, `plan_nothing`), 3 guesses (`guess_low`, `guess_honest`, `guess_refused`), 4 registers (`register_ledger`, `register_sick`, `register_cohort`, `register_voluntary`), 4 NPCs (`dr_irina_vel`, `wyn_omah`, `piet_abar`, `saria_voss`).
- `dose_quests.json`: 4 active questlines (`quest_the_dose_the_first_reading`, `quest_the_sick_of_room_seven`, `quest_the_childs_number`, `quest_the_signed_hour`).
- `dose_items.json`: 5 items (`item_dose_ledger`, `item_calibration_key`, `item_dosimeter_tag`, `item_palliative_morphine`, `item_cohort_first_board`).
- `dose_locations.json`: 3 locations (`loc_the_dose_room`, `loc_the_calibration_bench`, `loc_the_childrens_baseline_board`).

### Autopsy Content
- `autopsy_procedures.json`: 9 surgical and forensic procedures (`procedure_rad_pathology`, `procedure_toxicology`, `procedure_containment_autopsy`, `procedure_blunt_trauma`, `procedure_ballistic_forensics`, `procedure_respiratory_contamination`, `procedure_hypothermia_pathology`, `procedure_spore_infection_isolation`, `procedure_poison_biochemical_assay`).
- Systems: `AutopsySystem.cs`, `AutopsyProcedureCatalogLoader.cs`.

### Psychological Contamination Content
- `PsychologicalContaminationSystem.cs` (in `Assets/Ashfall.Core/Maritime/`): 4 contamination types (`contam_thousand_yard_stare`, `contam_disgust_cascade`, `contam_phantom_smell`, `contam_child_cot_trauma`) mapped to 5 high-trauma locations (`location_stadium_evacuation_center`, `location_automated_abattoir`, `location_sunshine_daycare`, `location_quarantine_mile`, `location_regional_blood_bank`).
- Downstream systems: `CombatTraumaSystem`, `SomaticFlashbackSystem`, `GuiltInsomniaSystem`.

---

## 2. Core Authority Decisions

1. **Physical Dose vs. Recorded Ledger:**
   - Physical exposure is authoritatively computed and tracked by `RadiationSystem` (`SurvivorRadState`).
   - Administrative records and kept tallies live in `DoseLedgerSystem` (`DoseEntry`).
   - Forged documents, falsified readings, and administrative chits alter institutional belief, not biological dose.

2. **Autopsy Findings & Consent:**
   - `AutopsySystem` owns procedure execution, tool/consumable requirements, and observation generation.
   - Findings route into `ResearchSystem` (knowledge nodes) and `DiseaseSystem` (pathogen identification).
   - Forensic cases produce verified records for evidence and secret systems.
   - Consent checks query kinship, leadership, and public health necessity without creating a parallel legal subsystem.

3. **Psychological Contamination Scope & Non-Overlap:**
   - Scope C: Deep-place and hazard-site exposure sources, routing threshold consequences into existing trauma, flashback, and insomnia systems.
   - No parallel sanity points, madness meters, or mental HP.
