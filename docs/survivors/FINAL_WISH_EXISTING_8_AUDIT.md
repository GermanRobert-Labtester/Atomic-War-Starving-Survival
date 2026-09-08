# Existing 8 Wishes Audit

**Document:** `docs/survivors/FINAL_WISH_EXISTING_8_AUDIT.md`

---

## 1. Inventory of Canonical Baseline (Entries 1–8)

The 8 baseline wishes in `Assets/StreamingAssets/Data/final_wishes.json` were inspected and preserved intact:

| # | Archetype ID | Type | Title | Steps | Required Items / Locations |
|---|---|---|---|---|---|
| 1 | `the_surgeon` | `teach_lesson` | The Final Surgery | 2 | `scalpel`, `forceps`, `surgical_suture` |
| 2 | `the_soldier` | `build_memorial` | The Fallen's Wall | 3 | `scrap_metal`, `concrete_rubble`, `dog_tags` |
| 3 | `the_nurse` | `teach_lesson` | The Caregiver's Legacy | 2 | `medical_textbook`, `anatomy_chart` |
| 4 | `the_mother` | `reconcile` | The Last Letter | 2 | `clean_paper`, `pen` |
| 5 | `the_mechanic` | `retrieve_heirloom` | The Lost Wrench | 2 | location: `ruined_garage` |
| 6 | `the_teacher` | `teach_lesson` | The Last Period | 2 | `chalk`, `blank_journal` |
| 7 | `the_refugee` | `see_the_sky` | One Last Sunrise | 1 | — |
| 8 | `the_electrician` | `retrieve_heirloom` | The Old Voltmeter | 2 | location: `electrical_substation` |

---

## 2. Integrity & Preservation Notes

- **Preservation Policy:** In accordance with Constraint 2.2, all 8 baseline records are maintained exactly as authored.
- **Prefix Matching in Core:** In `FinalWishSystem.cs:211-217`, `the_surgeon`, `the_soldier`, `the_nurse`, and `the_mother` serve as canonical archetype targets for prefix fallback routing.
- **No Rewrites:** No changes to titles, descriptions, step IDs, or completion texts were made to existing baseline entries.
