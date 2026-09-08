# Plan 106 Baseline — Dose Items Expansion (5 → 15 Dose-Ledger Items)

**Status:** Reconciled against the live repository
**Authority:** `Assets/StreamingAssets/Data/dose_items.json`
**Mirror:** `builds/linux/Assets/StreamingAssets/Data/dose_items.json`
**Catalog Schema Version:** 1
**Baseline Before This Change:** 9 items (5 original baseline + 4 Plan 27 items)
**Target After This Change:** Exactly 15 items (6 new items appended)

---

## 1. Repository Truth & Baseline Reconciliation

The initial Plan 106 brief referenced a 5-item baseline. Forensic inspection of `dose_items.json`, `Ashfall.Core.Tests/DoseContentCatalogTests.cs`, and `docs/bodymind/DOSE_ITEM_MATRIX.md` demonstrates that 4 additional items landed in a previous expansion phase (Plan 27 / Plan 101):
- `item_calibrated_dosimeter` (tool)
- `item_forged_clean_bill_chit` (story)
- `item_chelation_decorporation_course` (medical)
- `item_shielded_badge_case` (tool)

In accordance with **Execution Contract §4.1 ("Repository truth wins")** and **§4.2 ("Preserve the original five")**, all 9 existing items are preserved intact with their original IDs, display names, weights, trade values, categories, and descriptions. Exactly 6 new items are authored and appended to achieve the target catalog size of **15 items**.

---

## 2. Catalog Schema Contract

The file `dose_items.json` follows schema version 1 wrapped in an object root:
```json
{
  "schema_version": 1,
  "items": [
    {
      "id": "item_...",
      "name": "...",
      "weightKg": 0.0,
      "tradeValue": 0,
      "category": "story|tool|medical|protective|consumable",
      "description": "..."
    }
  ]
}
```

### Properties
- `id` (string): Unique identifier with mandatory `item_` prefix, snake_case. Globally collision-safe.
- `name` (string): Clean occupational display name (1–4 words).
- `weightKg` (float): Encumbrance weight in kilograms (0.01–2.0 kg scale).
- `tradeValue` (float): Base barter value (0 for unique records/story items).
- `category` (string): Allowed enum values: `story`, `tool`, `medical`, `protective`, `consumable`.
- `description` (string): 1–2 sentences capturing physical wear, procedural use, bureaucratic markings, or limitations.

---

## 3. Catalog Composition & Dual Consumption Model

Dose items operate under a combined **Model A (canonical item merge)** and **Model C (domain catalog)** architecture:
1. **`ItemCatalogLoader.cs`**: `dose_items.json` is registered in `SecondaryItemFiles`. The loader reads the `items` array and registers definitions into the global `ItemCatalog`. All IDs must be globally unique across all item catalogs.
2. **`DoseContentCatalogLoader.cs`**: Deserializes `dose_items.json` into `DoseContentCatalog.items` (`DoseItemDef`), consumed directly by `DoseLedgerSystem`, `DosePanel`, and dose quests.
3. **`WarlordDoctrineCatalog.cs`**: Performs validation scans over `ItemFiles` (including `dose_items.json`) to ensure tribute item references resolve cleanly.

---

## 4. Complete 15-Item Inventory & Role Distribution

| # | ID | Name | Category | Weight (kg) | Trade Value | Role & Bureaucratic Identity |
|---|---|---|---|---:|---:|---|
| 1 | `item_dose_ledger` | The Dose Ledger | story | 1.20 | 0 | Foundational physical tally book recording intake readings and dates. |
| 2 | `item_calibration_key` | Dosimeter Calibration Key | tool | 0.10 | 40 | Brass socket-wrench key used to reset zero-point drift on quartz dosimeters. |
| 3 | `item_dosimeter_tag` | Dosimeter Tag | tool | 0.05 | 15 | Stamped alloy tag tying a numbered instrument to an individual survivor. |
| 4 | `item_palliative_morphine` | Palliative Morphine Tray | medical | 0.40 | 90 | Pre-Exchange hospital tray for symptom management in Red/Black band cases. |
| 5 | `item_cohort_first_board` | The Children's Baseline Board | story | 0.80 | 0 | Chalkboard section preserving historical intake baselines from the nursery corridor. |
| 6 | `item_calibrated_dosimeter` | Calibrated Quartz Dosimeter | tool | 0.25 | 65 | Precision electroscope dosimeter verified against Piet's master bench standard. |
| 7 | `item_forged_clean_bill_chit` | Forged Clean-Bill Chit | story | 0.02 | 50 | Stamped administrative slip certifying false Green-band status. |
| 8 | `item_chelation_decorporation_course` | Chelation Decorporation Course | medical | 0.35 | 85 | DTPA and zinc decorporation ampoules for internal radionuclide ingestion. |
| 9 | `item_shielded_badge_case` | Lead-Shielded Badge Case | tool | 0.60 | 30 | Lead-lined container preventing dormant film badges from ambient fogging. |
| 10 | `item_pocket_dosimeter` | Pocket Dosimeter | tool | 0.15 | 45 | Clip-on direct-reading quartz-fiber dosimeter for personal exposure tracking. |
| 11 | `item_radiation_survey_meter` | Radiation Survey Meter | tool | 1.40 | 75 | Portable ionization survey meter with beta shield for ambient field surveys. |
| 12 | `item_dose_register_book` | Dose Register Book | story | 0.90 | 0 | Hardbound administrative ledger with carbon duplicates for official certification. |
| 13 | `item_cohort_baseline_card` | Cohort Baseline Card | story | 0.02 | 0 | Stiff index card recording quarterly thyroid palpation and longitudinal baselines. |
| 14 | `item_shielding_apron` | Examiner Shielding Apron | protective | 1.80 | 60 | Vinyl-clad lead composite apron providing localized torso scatter attenuation. |
| 15 | `item_potassium_iodide_pack` | Potassium Iodide Pack | medical | 0.05 | 50 | Stable potassium iodide tablets strictly for thyroid blocking against radioiodine. |

---

## 5. Medical Realism & Physical Semantics

Plan 106 strictly adheres to nuclear and medical realism guidelines:
1. **Dosimeter vs Survey Meter**: `item_pocket_dosimeter` measures cumulative individual worker dose; `item_radiation_survey_meter` measures instantaneous ambient exposure rate. Neither provides protection.
2. **Paperwork vs Physical Reality**: `item_dose_register_book`, `item_cohort_baseline_card`, and `item_dose_ledger` represent administrative recordkeeping and institutional belief. Possession or forgery never alters physical absorbed dose.
3. **Thyroid Prophylaxis**: `item_potassium_iodide_pack` is authored strictly as thyroid blocking against radioactive iodine isotopes ($^{131}\text{I}$). It is explicitly not a universal radiation cure or whole-body dose reducer.
4. **Internal Decorporation**: `item_chelation_decorporation_course` is clinically bounded to internal contaminant removal, not reversing past radiation damage or external exposure.
5. **Procedural Shielding**: `item_shielding_apron` offers localized scatter attenuation for the torso during close examinations; it does not confer blanket immunity to penetrating gamma radiation.

---

## 6. Global ID Collision Verification

All 6 proposed IDs were checked against the entire repository data and source code:
- `item_pocket_dosimeter`: 0 collisions.
- `item_radiation_survey_meter`: 0 collisions.
- `item_dose_register_book`: 0 collisions.
- `item_cohort_baseline_card`: 0 collisions.
- `item_shielding_apron`: 0 collisions.
- `item_potassium_iodide_pack`: 0 collisions (distinct from legacy asset `item_potassium_iodide`).
