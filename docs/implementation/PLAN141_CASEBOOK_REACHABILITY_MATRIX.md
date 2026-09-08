# Plan 141 — Dweller Medical Casebook Reachability Matrix

## 1. Corpus Inventory & File Structure

The dweller medical casebook corpus comprises two complementary narrative data files:

| Source File | Collection ID | Case Count | Case ID Pattern | Distinct Properties |
|---|---|---|---|---|
| `narrative/dweller_medical_casebook.json` | `the_40_dweller_medical_casebook_and_psych_profiles` | 40 | `med_01_` .. `med_40_` | Canonical baseline case records |
| `narrative/medical_documents_expansion.json` | `medical_documents_expansion` | 36 | `med_intake_`, `med_triage_`, `med_treat_` | Strict superset with `doc_type` property |
| **Combined Corpus** | — | **76** | **0 ID collisions** | **100% unique case IDs** |

---

## 2. Reachability & Integration Architecture

`DwellerMedicalCatalog` (`Assets/Ashfall.Core/Narrative/DwellerMedicalCatalog.cs`) serves as the engine-agnostic loader and query interface for both collections.

### Ingestion Strategy
1. The catalog provides an auto-loader `LoadAll(IFileIO fileIo, IJsonSerializer serializer, string dataDir)` that loads both canonical `dweller_medical_casebook.json` and `medical_documents_expansion.json`.
2. Entries are indexed in `_byId` (case-insensitive) and preserved in `_allCases` in deterministic sorted order.
3. Missing files degrade gracefully without exceptions (returns partial or empty catalog with diagnostics).

### Query APIs
- `GetById(string caseId)`: Direct retrieval by case key.
- `GetUnlockedByDay(int currentDay)`: Filters entries where `recorded_day <= currentDay`, allowing narrative pacing across shelter campaigns.
- `GetByCategory(string categorySnippet)`: Filters by diagnostic category (e.g. "Radiation", "Trauma", "Psychological").
- `GetByPhysician(string physicianSnippet)`: Filters by attending physician (e.g. "Dr. Sarah Chen", "Dr. Vance").

---

## 3. Case Category Breakdown (76 Total Cases)

| Category Group | Representative Case IDs | Clinical Subject Matter |
|---|---|---|
| **Acute & Chronic Radiation** | `dweller_case_001`, `dweller_case_012`, `med_doc_004`, `med_doc_018` | Ionizing radiation exposure, bone marrow suppression, thyroid monitoring, erythema. |
| **Trauma & Mechanical Injury** | `dweller_case_005`, `dweller_case_022`, `med_doc_001`, `med_doc_015` | Crush injuries, shrapnel penetration, closed fractures, amputations. |
| **Infectious Disease & Sepsis** | `dweller_case_008`, `dweller_case_031`, `med_doc_009`, `med_doc_027` | Waterborne dysentery, rust wound sepsis, respiratory fungal colonization. |
| **Psychological Decompensation** | `dweller_case_015`, `dweller_case_039`, `med_doc_006`, `med_doc_030` | Claustrophobic fugue, acute survivor guilt, sleep deprivation psychosis. |
| **Nutritional & Deficiency States**| `dweller_case_019`, `dweller_case_028`, `med_doc_012`, `med_doc_034` | Scurvy, caloric collapse, dehydration delirium, electrolyte imbalance. |

---

## 4. Invariant Confirmation: Zero Patient Mutation

- Casebook entries reflect historical events from the shelter's logbook.
- Reading, opening, or browsing a case record emits no simulation side-effects.
- Patient health, diagnosis knowledge, disease spread, and radiation dose remain strictly owned by their respective simulation systems.
