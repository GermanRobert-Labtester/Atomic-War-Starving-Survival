# Plan 141 — Medical Accuracy & Mechanical Alignment Audit

## 1. Audit Rationale & Clinical Standards

The survival-management genre demands grounded, authentic clinical details. Presenting medical folklore or dangerously inaccurate first-aid advice damages immersion and violates ASHFALL's design principles.

This audit reviews every medical assertion in `medical_texts.json`, documenting verified clinical realities, necessary prose corrections, and presentation safeguards.

---

## 2. Topic-by-Topic Forensic Findings & Remediation

### 2.1 Potassium Iodide (KI)
- **Clinical Reality:** Potassium iodide saturates thyroid iodine receptors, preventing uptake of radioactive iodine isotopes (e.g. Iodine-131). It offers **zero** protection against external gamma radiation, cesium, strontium, americium, or whole-body acute radiation syndrome.
- **Audit Finding in `medical_texts.json`:**
  - `medical_radiation_exposure.treatment_steps`: Correctly states `"Take iodine pills immediately. Protect your thyroid."`
  - `medical_radiation_exposure.system_integration`: Inaccurately stated `"Increases radiation level. Iodine and anti-rad reduce radiation. Can lead to radiation sickness."`
- **Action Taken:**
  - Preserved thyroid-specific wording in player-facing treatment steps.
  - Corrected `system_integration` to specify that potassium iodide protects thyroid uptake only, and chelation agents assist decorporation.
  - Runtime UI explicitly labels the action `IODINE (+RESIST)` without claiming whole-body dose reduction.

### 2.2 Decorporation & Chelation (Prussian Blue / DTPA / Anti-Rad)
- **Clinical Reality:** Decorporation agents bind specific radionuclides internally: Prussian blue binds Cesium-137 and Thallium in the gastrointestinal tract; DTPA chelates Plutonium, Americium, and Curium. They do not reverse tissue ionization damage or reduce absorbed external gamma radiation.
- **Audit Finding:**
  - `medical_texts.json` uses generic `anti_rad` chelation terminology: `"Chelation helps remove radioactive particles."`
- **Action Taken:**
  - Retained as accurate context for radionuclide clearance.
  - Prohibited from rendering claims of instant whole-body biological repair.

### 2.3 Bone Fracture Healing ("Stronger at the Break" Myth)
- **Clinical Reality:** The widespread myth that fractured bone heals "stronger than before" is medically false. Initial woven bone callus is mechanically weaker and more disorganized than lamellar bone. While a temporary callus may be bulkier, fully remodeled bone at best returns to its original baseline strength, and improper immobilization leaves structural weakness.
- **Audit Finding in `medical_fracture`:**
  - `long_term_effects`: Contained the myth: `"The bone is stronger at the break point. But the memory is weaker."`
- **Action Taken:**
  - Corrected `medical_fracture.long_term_effects` to:
    `"The bone remodels at the break point, but requires weeks of protection to regain normal strength. The memory of the break lingers."`

### 2.4 Wound Antisepsis & Stinging Myth
- **Clinical Reality:** The folk belief that "if it stings, it is working" is dangerous. Stinging often indicates cellular lysis and tissue toxicity from harsh chemicals (e.g. undiluted hydrogen peroxide or high-concentration alcohol) damaging exposed fibroblasts and granulation tissue.
- **Audit Finding:**
  - The literal phrase "stinging means it is working" is **not present** in `medical_texts.json`.
  - Pain descriptions in `medical_wound_care`, `medical_cuts_and_scrapes`, and `medical_suturing` describe sensation as `"Stinging. Like the antiseptic is burning."`
- **Action Taken:**
  - Verified that stinging is presented as a sensory patient reaction rather than an indicator of therapeutic efficacy.
  - Enforced via automated unit test that the phrase "means it is working" is barred from catalog prose.

### 2.5 Burn Care
- **Clinical Reality:** Thermal burns require copious irrigation with clean, cool running water (10–20°C for at least 10–20 minutes). Ice must never be applied (causes vasoconstriction and secondary frostbite/tissue necrosis). Blisters should not be debrided in unsterile field conditions. Grease/butter promotes bacterial colonization.
- **Audit Finding in `medical_burn` & Burn Care Entries:**
  - `medical_burn.treatment_steps`: Correctly recommends `"Cool the burn immediately. Clean, cool water. Not ice."` and `"Cover the burn with a clean, dry dressing."`
- **Action Taken:**
  - Retained and approved for player presentation.

### 2.6 Sepsis & Systemic Infection
- **Clinical Reality:** Sepsis is life-threatening organ dysfunction caused by a dysregulated host response to infection. Warning signs: high fever or hypothermia, confusion/altered mental status, rapid heart rate, low blood pressure.
- **Audit Finding in `medical_infection` & `medical_wound_infection`:**
  - `complication_warnings`: Correctly highlights `"Sepsis. If the infection enters your bloodstream, you are in grave danger."`
- **Action Taken:**
  - Retained as high-urgency clinical warnings.

### 2.7 Decorative Success Chances
- **Audit Finding:**
  - Every condition entry in `medical_texts.json` includes a `success_chances` object with arbitrary percentages (e.g. `"bandage": 0.85`, `"herbal_remedy": 0.25`).
- **Clinical & Simulation Reality:**
  - The authoritative treatment engine (`MedicalPipelineCoordinator`, `MedicalWardSystem`, `DiseaseSystem`) uses mechanical formulas, doctor skills, facility tier, and patient state to resolve outcomes.
- **Action Taken:**
  - `success_chances` is classified as **DECORATIVE / NON-AUTHORITATIVE**.
  - Runtime UI is strictly prohibited from rendering these numbers as game truth.

### 2.8 Duplicate Record Remediation
- **Audit Finding:**
  - `medical_dehydration_severe` was defined twice in `medical_texts.json` (at index 14 and index 73).
- **Action Taken:**
  - Consolidated into a single canonical entry, bringing the catalog to exactly 83 distinct condition definitions.

---

## 3. Summary of Catalog Assertions

| Assertion | Original Status | Audited Action | Verification Gate |
|---|---|---|---|
| Potassium Iodide reduces whole-body radiation | Ambiguous in notes | Corrected to thyroid-uptake saturation only | Unit test assertion |
| Fractures heal stronger | Medically false myth | Corrected to remodeling over time | Unit test assertion |
| Stinging indicates efficacy | Absent (sensory only) | Confirmed absent; barred by negative fixture | Negative test fixture |
| Ice on severe burns | Correctly avoided | Retained: cool water specified, not ice | Unit test assertion |
| Sepsis bloodstream warning | Medically accurate | Retained in complication warnings | Unit test assertion |
| Static JSON success percentages | Unlinked to simulation | Strictly hidden from player UI | Unit test assertion |
