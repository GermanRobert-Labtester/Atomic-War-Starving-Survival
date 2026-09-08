# Plan 106 Closeout — Dose Items Expansion (5 → 15 Dose-Ledger Items)

**Status:** COMPLETE
**Authority:** `Assets/StreamingAssets/Data/dose_items.json`
**Mirror:** `builds/linux/Assets/StreamingAssets/Data/dose_items.json`
**Catalog Schema Version:** 1
**Initial Baseline:** 9 items (5 legacy baseline + 4 Plan 27 items)
**Final Catalog Size:** Exactly 15 items (+6 new items appended)
**Test Suite:** `Ashfall.Core.Tests/DoseItemExpansionTests.cs` (10 tests) & `Ashfall.Core.Tests/DoseContentCatalogTests.cs` (8 tests)

---

## 1. Executive Summary

Plan 106 expanded `dose_items.json` from the verified repository baseline of 9 items to the target catalog size of exactly 15 items. The expansion deepens the physical equipment and paperwork culture surrounding the dose-ledger bureaucracy without creating parallel radiation mechanics, speculative medical cures, or invalid item namespaces.

In adherence with **Execution Contract §4.1** and **§4.2**, all 9 pre-existing items remain completely unchanged. The 6 new additions introduce grounded equipment for personal exposure tracking, ambient ionization surveys, institutional carbon logbooks, cohort baseline registry cards, localized examiner torso shielding, and radioiodine thyroid blocking.

---

## 2. Authoritative 15-Item Catalog

| # | Item ID | Display Name | Category | Weight (kg) | Trade Value | Conceptual Role |
|---|---|---|---|---:|---:|---|
| 1 | `item_dose_ledger` | The Dose Ledger | story | 1.20 | 0 | Foundational physical tally book for shelter reading bookings. |
| 2 | `item_calibration_key` | Dosimeter Calibration Key | tool | 0.10 | 40 | Brass socket-wrench key to reset quartz dosimeter calibration drift. |
| 3 | `item_dosimeter_tag` | Dosimeter Tag | tool | 0.05 | 15 | Stamped alloy tag tying an instrument to a named survivor. |
| 4 | `item_palliative_morphine` | Palliative Morphine Tray | medical | 0.40 | 90 | Pre-Exchange hospital tray for palliative symptom management in Red/Black bands. |
| 5 | `item_cohort_first_board` | The Children's Baseline Board | story | 0.80 | 0 | Historical chalkboard section preserving nursery corridor baseline guesses. |
| 6 | `item_calibrated_dosimeter` | Calibrated Quartz Dosimeter | tool | 0.25 | 65 | Precision electroscope dosimeter verified against master bench standard. |
| 7 | `item_forged_clean_bill_chit` | Forged Clean-Bill Chit | story | 0.02 | 50 | Administrative slip certifying false Green-band clearance. |
| 8 | `item_chelation_decorporation_course` | Chelation Decorporation Course | medical | 0.35 | 85 | DTPA and zinc decorporation ampoules for internal isotope ingestion. |
| 9 | `item_shielded_badge_case` | Lead-Shielded Badge Case | tool | 0.60 | 30 | Lead-lined container preventing dormant film badges from ambient fogging. |
| 10 | `item_pocket_dosimeter` | Pocket Dosimeter | tool | 0.15 | 45 | Clip-on direct-reading quartz-fiber dosimeter for personal exposure tracking. |
| 11 | `item_radiation_survey_meter` | Radiation Survey Meter | tool | 1.40 | 75 | Portable ionization survey meter with beta shield for ambient area surveys. |
| 12 | `item_dose_register_book` | Dose Register Book | story | 0.90 | 0 | Hardbound institutional ledger with carbon duplicates for official certification. |
| 13 | `item_cohort_baseline_card` | Cohort Baseline Card | story | 0.02 | 0 | Stiff index card recording quarterly thyroid palpation and longitudinal baselines. |
| 14 | `item_shielding_apron` | Examiner Shielding Apron | protective | 1.80 | 60 | Vinyl-clad lead composite apron providing localized torso scatter attenuation. |
| 15 | `item_potassium_iodide_pack` | Potassium Iodide Pack | medical | 0.05 | 50 | Stable potassium iodide tablets strictly for thyroid blocking against radioiodine. |

---

## 3. Medical Realism & Invariants Enforced

1. **No Universal Radiation Cures**: No item description or mechanic implies that potassium iodide, chelation, or palliative supplies erase past whole-body absorbed radiation dose.
2. **Thyroid Protection Specificity**: `item_potassium_iodide_pack` is authored strictly as thyroid blocking against acute radioactive iodine ($^{131}\text{I}$) uptake.
3. **Internal Contaminant Specificity**: `item_chelation_decorporation_course` is clinically bounded to internal contaminant removal, not reversing external tissue damage.
4. **Instrument Functional Separation**: `item_pocket_dosimeter` measures personal cumulative exposure; `item_radiation_survey_meter` measures instantaneous ambient exposure rate. Neither creates directional shielding.
5. **Procedural Torso Shielding**: `item_shielding_apron` provides localized torso scatter attenuation during close-contact examinations; it does not confer blanket penetrating gamma immunity.
6. **Administrative Recordkeeping**: `item_dose_register_book`, `item_cohort_baseline_card`, and `item_dose_ledger` represent institutional paperwork; possession or forgery never alters physical absorbed dose.

---

## 4. Producer and Consumer Mapping

| Item ID | Producers | Consumers | Tradeable | Canonical Registry |
|---|---|---|:---:|:---:|
| `item_dose_ledger` | Quest: First Reading | `DoseLedgerSystem`, `DosePanel`, Quest completion | No (0) | Yes |
| `item_calibration_key` | Quest: The Register Audit, Scavenging | Piet Abar calibration actions, Questline | Yes (40) | Yes |
| `item_dosimeter_tag` | Quest: Stolen Dosimeter, Scavenging | Survivor inspection binding, Quests | Yes (15) | Yes |
| `item_palliative_morphine` | Quest: Sick of Room Seven, Pharma lab | Sister Wyn Omah palliative care plans | Yes (90) | Yes |
| `item_cohort_first_board` | Quest: The Child's Number | Saria Voss cohort baseline memorial | No (0) | Yes |
| `item_calibrated_dosimeter` | Quest: Stolen Dosimeter / Broken Calibration | Field survey operations, high-accuracy readings | Yes (65) | Yes |
| `item_forged_clean_bill_chit` | Quest: Black Market Clean Bill | Checkpoint clearance, black-market trading | Yes (50) | Yes |
| `item_chelation_decorporation_course` | Quest: Exposure for Essential Worker | Internal isotope decorporation care plans | Yes (85) | Yes |
| `item_shielded_badge_case` | Quest: Child Over the Limit, Workshop | Film badge storage, workshop protection | Yes (30) | Yes |
| `item_pocket_dosimeter` | Civil Defense caches, High-rad salvage | Personal worker monitoring, expedition prep | Yes (45) | Yes |
| `item_radiation_survey_meter` | Scientific facilities, Industrial salvage | Sector radiation mapping, decontamination survey | Yes (75) | Yes |
| `item_dose_register_book` | Registrar offices, Bunker archives | Institutional audit evidence, recordkeeping | No (0) | Yes |
| `item_cohort_baseline_card` | Clinic intake archives, Nursery records | Longitudinal child tracking, clinical evidence | No (0) | Yes |
| `item_shielding_apron` | Clinic triage rooms, Decon airlocks | Close-contact examiner protection | Yes (60) | Yes |
| `item_potassium_iodide_pack` | Emergency medical caches, Pharma synthesis | Acute radioiodine thyroid prophylaxis | Yes (50) | Yes |

---

## 5. Verification Matrix Summary

| Gate / Command | Status | Output Evidence |
|---|---|---|
| `dotnet test Ashfall.Core.Tests --filter FullyQualifiedName~DoseItem` | **PASS** | 10 passed, 0 failed (336 ms) |
| `dotnet test Ashfall.Core.Tests --filter FullyQualifiedName~DoseContentCatalogTests` | **PASS** | 8 passed, 0 failed (75 ms) |
| `godot --headless --path . -- --data-integrity-selftest` | **PASS** | 0 findings, 12,245 authored IDs across 298 catalogs |
| `godot --headless --path . -- --content-utilization-selftest` | **PASS** | CI Gate PASS (581 catalogs scanned, 208 gameplay-consumed) |
| `godot --headless --path . -- --scene-binding-selftest` | **PASS** | 25/25 production scenes passed, 0 failed |
| `python3 scripts/ci/scene-lint.py` | **PASS** | 30 production scenes checked; 0 errors; 0 warnings |
| `python3 scripts/ci/generate-asset-registry.py` | **PASS** | Written to `artifacts/asset_registry.json` (1,646 assets tracked) |
| `dotnet build Ashfall.csproj` | **PASS** | 0 warnings, 0 errors |
| `dotnet test Ashfall.Core.Tests` | **PASS** | Full regression suite green (0 failed) |
