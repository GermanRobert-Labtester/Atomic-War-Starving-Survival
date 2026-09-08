# Plan 141 — Medical Text Schema Map

## 1. Catalog Architecture Overview

- **Source File:** `Assets/StreamingAssets/Data/medical_texts.json`
- **Schema Version:** `1` (top-level `schema_version: 1`)
- **Collection ID:** `"medical_texts"`
- **Root Shape:** JSON Object with:
  - `schema_version` (`int`)
  - `collection_id` (`string`)
  - `conditions` (`List<MedicalConditionEntry>`)
- **Raw Entry Count:** 84 condition objects
- **Unique Condition ID Count:** 83 unique IDs (1 duplicated record: `medical_dehydration_severe` at entries 15 and 74)

---

## 2. Field-by-Field Classification Matrix

Every property in `medical_texts.json` is formally mapped, typed, and assigned an authoritative runtime governance status.

| Field Name | JSON Type | C# Type | Runtime Classification | Safety & Presentation Policy |
|---|---|---|---|---|
| `id` | `string` | `string` | **Key / Identifier** | Canonical key. Prefixed with `medical_`. Must be normalized and validated against duplicate definitions. |
| `category` | `string` | `string` | **Categorization** | Medical taxonomy bucket: `injury`, `illness`, `mental`, `chronic`, `surgery`, `burn_care`, `wound_care`, etc. Display-safe tag. |
| `display_name` | `string` | `string` | **Display-Safe** | Clinical condition name presented in UI cards, detail headers, and clinical case summaries. |
| `diagnosis_text` | `string` | `string` | **Display-Safe** | Primary descriptive prose explaining the condition's pathology and physical presentation. Safe for patient cards. |
| `symptom_descriptions` | `string[]` | `List<string>` | **Display-Safe (Deterministic)** | Observable physical symptoms. Seeded/deterministic selection picks 1–2 lines for UI cards to avoid text bloat. |
| `treatment_steps` | `string[]` | `List<string>` | **Conditional Context** | Informational clinical context. Kept as descriptive guidance; never overrides or gates the authoritative medical pipeline. |
| `required_items` | `string[]` | `List<string>` | **Validation / Cross-Reference Only** | Authored item names (e.g. `["clean_water", "bandage"]`). Never consumed as a mechanical crafting recipe; validated against catalog IDs. |
| `success_chances` | `object` | `Dictionary<string, double>` | **Decorative / Non-Authoritative (HIDDEN)** | **STRICTLY HIDDEN FROM RUNTIME UI.** These percentages are flavor only and do not reflect simulation RNG or treatment engine calculations. |
| `failure_consequences` | `string[]` | `List<string>` | **Display-Safe** | Descriptive clinical warning explaining what happens if the condition remains untreated. |
| `recovery_descriptions` | `string[]` | `List<string>` | **Conditional** | Displayed only when the patient is in active convalescence/recovery state. |
| `complication_warnings` | `string[]` | `List<string>` | **Display-Safe** | Critical warning highlighting secondary risks (e.g. sepsis, necrosis, shock). |
| `prevention_advice` | `string[]` | `List<string>` | **Conditional Context** | Preventative measures and hygiene guidance. |
| `long_term_effects` | `string[]` | `List<string>` | **Display-Safe** | Prognostic context describing lasting scars, tissue remodeling, or functional deficits. |
| `pain_descriptions` | `string[]` | `List<string>` | **Display-Safe** | Sensory and pain descriptions. Can be queried deterministically to enrich triage observations. |
| `mental_state` | `string` | `string` | **Display-Safe** | Psychological and cognitive presentation. |
| `physical_state` | `string` | `string` | **Display-Safe** | Somatic posture, respiration, skin tone, and motor indicators. |
| `emotional_impact` | `string` | `string` | **Display-Safe** | Emotional toll on the survivor and shelter cohort. |
| `system_integration` | `string` | `string` | **Developer Notes (HIDDEN)** | Historical design/system notes from early development. Not exposed in player-facing screens. |

---

## 3. Data Integrity & Validation Rules

1. **Non-Empty Text:** `id`, `category`, `display_name`, and `diagnosis_text` must be non-empty strings for every entry.
2. **Deterministic Lookup:** Queries operate strictly on normalized ordinal keys.
3. **No Simulation Mutation:** Deserialization and catalog queries are pure read-only operations.
4. **Fallback Safety:** If a requested condition ID is not found in `medical_texts.json`, the query returns `null` or a safe fallback DTO rather than throwing an exception.
