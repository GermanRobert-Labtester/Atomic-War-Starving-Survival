# Plan 141 — Baseline Reconnaissance & Execution Contract Proof

## 1. Executive Summary

This document establishes the empirical baseline for **Plan 141 — Medical Narrative Casebook & Condition Text Runtime Activation**. All answers are derived directly from current repository inspection across `Assets/Ashfall.Core/`, `Assets/StreamingAssets/Data/`, `src/UI/`, and `Ashfall.Core.Tests/`.

---

## 2. Answers to the 8 Mandatory Execution Contract Questions

### Question 1: What file(s) `DwellerMedicalCatalog` currently loads by default, if any.
**Answer:** None.
`DwellerMedicalCatalog` (`Assets/Ashfall.Core/Narrative/DwellerMedicalCatalog.cs`) exposes `public void Load(string json, IJsonSerializer serializer)` but has **no production auto-loader** in any runtime host session. Prior to Plan 141, it was invoked solely within `Ashfall.Core.Tests/DwellerMedicalCatalogTests.cs`, where tests manually read `Assets/StreamingAssets/Data/narrative/dweller_medical_casebook.json`.

### Question 2: Whether `MedicalPanel`, `AfflictionsPanel`, patient detail, or another panel already has an extensibility point for descriptive text.
**Answer:** Yes.
- `src/UI/MedicalPanel.cs`: Survivor vital cards (`_healthStats`) and the Disease Ward isolation section (`_treatmentList`) are dynamic `VBoxContainer` hierarchies built with `AshfallUiHelpers.MakeVBox`, `MakeMetadata`, and `MakePanel`. There is direct extensibility to append clinical context (e.g. diagnosis overview, observable presentation, and complication warnings) into the per-survivor card and per-disease row.
- `src/UI/AfflictionsPanel.cs`: Lists (`_activeList`, `_chronicList`, `_treatmentList`) are dynamic `VBoxContainer` hierarchies populated in `RenderActive()`, `RenderChronic()`, and `RenderTreatments()`. Affliction entries can host descriptive sub-rows below the status label.

### Question 3: Which live identifiers represent injuries/conditions versus diseases versus radiation state versus psychological projections.
**Answer:**
- **Injuries / Low Health:**
  - `affliction_health_deficit` (`MedicalTreatmentCatalog.HealthDeficitId`): Treatable health deficit, targeted by `treatment_bandage` (consumes `bandage`).
  - Runtime health < 30 HP is presented as critical health.
- **Respiratory State:**
  - `affliction_respiratory_degeneration` (`MedicalTreatmentCatalog.RespiratoryDegenerationId`): Targeted by `treatment_inhaler`, `treatment_herbal_tea`, `treatment_oxygen_support`.
- **Radiation State:**
  - `affliction_radiation_sickness` (`MedicalTreatmentCatalog.RadiationSicknessId`): Targeted by `treatment_iodine` (potassium iodide, thyroid uptake prevention) and `treatment_anti_rad` (chelation agent).
  - Runtime thresholds: Acute Radiation Sickness (ARS) at dose >= 50 mSv; Chronic Radiation Illness at lifetime exposure >= 250 mSv.
- **Chemical Dependency / Addiction:**
  - `affliction_chemical_dependency` (`MedicalTreatmentCatalog.ChemicalDependencyId`): Targeted by `treatment_managed_detox` and `treatment_cold_turkey`.
- **Diseases (20 authored diseases in `disease_catalog.json`):**
  - All prefixed with `disease_`: `disease_cholera`, `disease_zoonotic_flu`, `disease_blood_fever`, `disease_spore_blight`, `disease_acute_radiation_syndrome`, `disease_fungal_respiratory`, `disease_typhoid_waterborne`, `disease_wellspring_cramps`, `disease_silt_jaundice`, `disease_condemned_air_cough`, `disease_dry_bunker_hiss`, `disease_septic_rust_wound_fever`, `disease_reused_needle_fever`, `disease_deep_excavation_mold_lung`, `disease_silo_lung`, `disease_prion_tremor`, `disease_dysentery`, `disease_meningococcal_fever`, `disease_bloodborne_hepatitis`, `disease_spore_wound_dermatitis`.
  - Masked suspect identity: `affliction_unidentified_illness` (`MedicalTreatmentCatalog.UnidentifiedIllnessId`).
  - Base family identifier: `affliction_disease` (`MedicalTreatmentCatalog.DiseaseId`).
- **Psychological Projections (observe-only):**
  - `affliction_combat_trauma` (`MedicalTreatmentCatalog.CombatTraumaId`).
  - `affliction_somatic_flashback` (`MedicalTreatmentCatalog.SomaticFlashbackId`).
  - `affliction_guilt_insomnia` (`MedicalTreatmentCatalog.GuiltInsomniaId`).

### Question 4: Whether any `medical_texts.json.required_items`, `success_chances`, or `system_integration` fields are consumed mechanically today.
**Answer:** No.
Repository grep confirms zero gameplay consumption:
- `success_chances` appears only in `medical_texts.json` and a single test DTO in `DescriptiveTextsTests.cs`. No simulation system reads it.
- `system_integration` appears only in `medical_texts.json` and `DescriptiveTextsTests.cs`. It contains informal design prose.
- `required_items` in `medical_texts.json` is not deserialized by any runtime system. Authoritative item costs are defined exclusively in `MedicalTreatmentCatalog` (`ItemCosts`) and `disease_catalog.json` (`DiseaseTreatment.item_id`).

### Question 5: Which treatment names and item IDs are canonical after the unified medical-pipeline work.
**Answer:**
- **Authoritative Treatment IDs (`MedicalTreatmentCatalog`):**
  - `treatment_bandage` (consumes `bandage`)
  - `treatment_iodine` (consumes `iodine_pills`)
  - `treatment_anti_rad` (consumes `rad_away`)
  - `treatment_inhaler` (consumes `inhaler`)
  - `treatment_herbal_tea` (consumes `herbal_tea`)
  - `treatment_oxygen_support` (consumes `item_oxygen_supply`)
  - `treatment_quarantine` (no item cost; requires confirmed diagnosis)
  - `treatment_release` (no item cost; requires confirmed diagnosis)
  - `treatment_managed_detox` (no item cost; flips detox program)
  - `treatment_cold_turkey` (no item cost; flips cold turkey program)
- **Authoritative Vector Protocol IDs:**
  - `protocol_purify_water`, `protocol_seal_vents`, `protocol_sterilize_tools`, `protocol_air_filtration`
- **Authoritative Item IDs:**
  - `bandage` (legacy alias `item_bandage`)
  - `iodine_pills` (legacy alias `item_potassium_iodide`)
  - `rad_away` (legacy alias `item_rad_away`)
  - `inhaler`
  - `herbal_tea`
  - `item_oxygen_supply`
  - `antibiotics` (legacy alias `item_antibiotics`)
  - `clean_water`, `gas_mask`, `hazmat_suit`

### Question 6: Whether active patient records expose a stable condition/affliction ID that can map to authored text.
**Answer:** Yes.
`PatientRecordProjector` generates `PatientRecord.Afflictions`, where each `PatientAfflictionView` contains:
- `public string AfflictionId`: exact canonical identifier (e.g. `affliction_respiratory_degeneration`, `affliction_radiation_sickness`, `disease_cholera`, or `affliction_unidentified_illness`).
- `public string StageLabel`: human-readable clinical stage.
- `public string DiagnosisStatus`: `"unknown"`, `"suspected"`, `"confirmed"`, or `"ruled_out"`.
- `public float SeverityValue`: disclosed severity level once confirmed.

### Question 7: Whether casebook entries are tied to survivor IDs, condition IDs, categories, or free prose.
**Answer:**
Casebook entries (`DwellerMedicalCaseEntry`) contain:
- `case_id`: stable record key (e.g. `dweller_case_001` .. `dweller_case_040`, `med_doc_001` .. `med_doc_036`).
- `recorded_day`: day integer.
- `attending_physician`: authoring doctor name.
- `patient_id` / `patient_name`: historical dweller identities from past shelter records.
- `category`: broad category string (e.g. "Acute Radiation Sickness", "Trauma", "Psychological Decompensation").
- `dose_estimate_msv`: estimated radiation exposure.
- `tags`: keyword strings (e.g. `["radiation", "trauma", "burns"]`).
- Free clinical prose: `symptoms`, `intervention`, `outcome`, `doctor_margin_note`.
They are **historical medical archives**, not live campaign survivor state.

### Question 8: How medical UI restores/rebinds after save/load and panel reconstruction.
**Answer:**
`MedicalPanel` and `AfflictionsPanel` implement a pure projection pattern:
- `Bind(...)` passes live host sessions (`MedicalHostSession`, `SurvivorsHostSession`, `InventoryHostSession`, `RespiratoryDegenerationSystem`).
- `RefreshView()` clears children and re-queries live state from the pipeline and sessions.
- Catalog prose is static read-only definition data; no prose text is saved to campaign files.
- Upon reload or re-open, the UI re-queries the static catalog using live condition IDs, ensuring total determinism and zero state mutation.

---

## 3. Checklist Items (1 through 20)

| # | Item | Finding |
|---|---|---|
| 1 | `medical_texts.json` condition count | 84 raw entries in file, 83 unique IDs (1 duplicate entry: `medical_dehydration_severe`). |
| 2 | Field inventory and casing | Snake_case: `id`, `category`, `display_name`, `diagnosis_text`, `symptom_descriptions`, `treatment_steps`, `required_items`, `success_chances`, `failure_consequences`, `recovery_descriptions`, `complication_warnings`, `prevention_advice`, `long_term_effects`, `pain_descriptions`, `mental_state`, `physical_state`, `emotional_impact`, `system_integration`. |
| 3 | `DwellerMedicalCatalog` count & sources | 40 canonical records in `narrative/dweller_medical_casebook.json`. |
| 4 | `medical_documents_expansion.json` | 36 records, strict superset adding `doc_type`. Total combined unique cases: 76 (0 ID collisions). |
| 5 | Production uses of `DwellerMedicalCatalog` | 0 production usages prior to Plan 141 (tested in `DwellerMedicalCatalogTests.cs`). |
| 6 | Production uses of `medical_texts.json` | 0 production runtime usages prior to Plan 141 (tested in `DescriptiveTextsTests.cs`). |
| 7 | Medical panels and render paths | `MedicalPanel` (`src/UI/MedicalPanel.cs`), `AfflictionsPanel` (`src/UI/AfflictionsPanel.cs`), `MedicalWardPanel` (`src/UI/MedicalWardPanel.cs`). |
| 8 | Condition & Affliction IDs | `affliction_respiratory_degeneration`, `affliction_radiation_sickness`, `affliction_chemical_dependency`, `affliction_health_deficit`, `affliction_unidentified_illness`, `affliction_combat_trauma`, `affliction_somatic_flashback`, `affliction_guilt_insomnia`. |
| 9 | `DiseaseSystem` IDs | 20 diseases prefixed with `disease_` (e.g. `disease_cholera`, `disease_zoonotic_flu`, `disease_spore_blight`). |
| 10 | Radiation / dose state IDs | `HasAcuteRadiationSickness` (>= 50 mSv), `HasChronicIllness` (>= 250 mSv), current dose bands (Normal < 25, Caution 25–50, Warn 50–100, Critical >= 100). |
| 11 | Psychological projection IDs | `affliction_combat_trauma`, `affliction_somatic_flashback`, `affliction_guilt_insomnia`. |
| 12 | Treatment IDs & item requirements | 10 treatments in `MedicalTreatmentCatalog` with authoritative `ItemCosts` (e.g. `treatment_bandage` -> 1 `bandage`). |
| 13 | Canonical medical inventory IDs | `bandage`, `iodine_pills`, `rad_away`, `inhaler`, `herbal_tea`, `item_oxygen_supply`, `antibiotics`, `clean_water`. |
| 14 | Treatment preview generation | `_pipeline.PreviewTreatment(survivor, treatmentId)` evaluates legality, contraindications, and supply availability side-effect-free. |
| 15 | Existing clinical guidance | `disease_catalog.json` contains `guidance` strings; `MedicalPanel` contains stage and dose labels. |
| 16 | Localization conventions | Plain English strings, constants in Core catalogs, runtime formatting via `AshfallUiHelpers`. |
| 17 | Save-state boundaries | Patient state is saved in `MedicalSaveStore` and `SurvivorsSaveStore`. Catalog text is static data and never saved. |
| 18 | Panel lifecycle & rebinding | `Bind(...)` followed by `RefreshView()`. Clearing and rebuilding child nodes prevents stale state. |
| 19 | Content utilization scanner | `ContentUtilizationScanner.cs` maps `medical_texts.json` to `MedicalWardSystem`, `MedicalPanel`, and `JournalCodex`. |
| 20 | Baseline selftest results | `--medical-selftest` PASS (15/15), `--data-integrity-selftest` PASS (0 errors), `dotnet test` PASS (10,059/10,059). |
