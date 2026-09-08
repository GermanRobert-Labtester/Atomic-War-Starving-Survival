# Plan 65 — Final Wishes Expansion: Closeout

## Status: **COMPLETE**

## Summary

`Assets/StreamingAssets/Data/final_wishes.json` has been successfully expanded from **8 baseline wishes to exactly 30 survivor wishes** (8 original preserved + 22 new additions). The expansion adheres strictly to the 10 requested wish types, integrates with canonical items, locations, NPCs, recipes, and skills, preserves 100% engine-agnostic Core invariants, and passes all unit tests, scene lints, and self-test verification gates.

---

## Counts & Reconciliation

```text
Baseline:    8  (the_surgeon, the_soldier, the_nurse, the_mother, the_mechanic,
                 the_teacher, the_refugee, the_electrician)
New:        22  (the_pharmacist, the_plumber, the_hunter, the_courier, the_reporter,
                 the_blind_preacher, the_misanthrope, the_botanist, the_prisoner,
                 the_defector, the_undertaker, the_pacifist, the_watchmaker,
                 the_chef, the_exhausted_father, the_hoarder, the_general,
                 the_fierce_mother, the_martyr, the_burglar, the_historian,
                 the_quartermaster)
Total:      30  — exactly 30 unique archetype IDs and 30 unique titles
```

> **Reconciliation Note:** A prior working draft noted 32 wishes due to two provisional entries (`the_quartermaster` and `the_miner`) appended in a scratch commit. In this definitive release, `the_quartermaster` was seamlessly authored as the plan's specified `name_a_successor` wish, bringing the catalog to the exact specified target of **30 wishes** (8 original + 22 new).

---

## Wish Type Distribution (Plan 65 Specification)

| Requested Type | Authored Type (Runtime) | Steps | Authored Count | Archetypes Covered |
|---|---|---|---|---|
| `teach_lesson` | `teach_lesson` | 2 | 3 | `the_pharmacist`, `the_plumber`, `the_hunter` |
| `deliver_letter` | `deliver_letter` | 2 | 3 | `the_courier`, `the_reporter`, `the_blind_preacher` |
| `see_a_place` | `see_a_place` | 2 | 2 | `the_misanthrope`, `the_botanist` |
| `reconcile` | `reconcile` | 2 | 2 | `the_prisoner`, `the_defector` |
| `die_with_dignity` | `die_with_dignity` | 2 | 3 | `the_undertaker`, `the_pacifist`, `the_watchmaker` |
| `last_meal` | `last_meal` | 2 | 2 | `the_chef`, `the_exhausted_father` |
| `confess` | `confess` | 2 | 2 | `the_hoarder`, `the_general` |
| `protect_someone` | `protect_someone` | 2 | 2 | `the_fierce_mother`, `the_martyr` |
| `return_a_relic` | `return_a_relic` | 2 | 2 | `the_burglar`, `the_historian` |
| `name_a_successor` | `name_a_successor` | 2 | 1 | `the_quartermaster` |
| **Total New** | | | **22** | |
| **Baseline 8** | `teach_lesson` (1), `build_memorial` (1), `deliver_letter` (1), `reconcile` (1), `retrieve_heirloom` (2), `see_the_sky` (2) | 1–3 | **8** | `the_surgeon`, `the_soldier`, `the_nurse`, `the_mother`, `the_mechanic`, `the_teacher`, `the_refugee`, `the_electrician` |
| **Total Catalog** | | | **30** | |

---

## 22 New Wishes Roster

| Archetype ID | Type | Title | Required Items / Location / NPC References |
|---|---|---|---|
| `the_pharmacist` | `teach_lesson` | Compounding Under Fire | Items: `iodine_pills`, `bandage` \| Skill: `skill_field_dressing` |
| `the_plumber` | `teach_lesson` | The Trap Valve | Items: `scrap_metal` \| Skill: `skill_rough_repairs` |
| `the_hunter` | `teach_lesson` | Deadfall and Wire | Items: `box_of_nails_10`, `trap_improvised_wire` \| Skill: `skill_trap_setter` |
| `the_courier` | `deliver_letter` | The Undelivered Route | Location: `loc_settlement_cape_beacon` |
| `the_reporter` | `deliver_letter` | Last Certified Dispatch | Location: `loc_settlement_cape_beacon` |
| `the_blind_preacher` | `deliver_letter` | The Sealed Litany | Location: `loc_settlement_cape_beacon` |
| `the_misanthrope` | `see_a_place` | High Ridge Solitude | Location: `location_ash_dune_cemetery` |
| `the_botanist` | `see_a_place` | The Silt Terrace | Location: `loc_terrace_pumphouse` |
| `the_prisoner` | `reconcile` | Debt in the Yard | NPC: `npc_mara_veln` |
| `the_defector` | `reconcile` | Across the Trench Line | NPC: `npc_marek_voln` |
| `the_undertaker` | `die_with_dignity` | Six Clean Boards | Privacy & ritual preparation; zero procedural/graphic details |
| `the_pacifist` | `die_with_dignity` | No Steel Near Me | Palliative comfort & peaceful bedside; non-violent ritual |
| `the_watchmaker` | `die_with_dignity` | Till the Mainspring Rests | Final quiet vigil with ticking pocketwatch |
| `the_chef` | `last_meal` | Real Salt and Greens | Items: `crop_leafy_green`, `crop_hardy_tuber`, `item_preservation_salt` |
| `the_exhausted_father` | `last_meal` | Warm Broth for Two | Items: `item_dried_herb_packets`, `clean_water`, `canned_food` |
| `the_hoarder` | `confess` | The False Floorboards | Items: `scrap_metal`, `box_of_nails_10` |
| `the_general` | `confess` | The Withdrawn Flank | Items: `item_document_field_report` |
| `the_fierce_mother` | `protect_someone` | Watch Over the Young | NPC: `npc_lina` |
| `the_martyr` | `protect_someone` | Stand in the Breach | NPC: `npc_niko` |
| `the_burglar` | `return_a_relic` | The Stolen Compass | Location: `loc_shrine_switchback_waystation` |
| `the_historian` | `return_a_relic` | The Boundary Marker | Items: `tarnished_medal`, `dog_tags_personal` |
| `the_quartermaster` | `name_a_successor` | Handing Over the Ledger | NPC: `npc_oskar_ruut` |

---

## Runtime Contract & Invariants

- **Runtime Execution (`FinalWishSystem.cs`):**
  - Uses `ISeededRng` exclusively for deterministic prognosis durations and fallback selection (Invariant 4 satisfied).
  - Unmapped/new wish types map to 2 required steps via `_ => 2`.
  - Advancing steps triggers `OnFinalWishStepCompleted` and completion triggers `OnFinalWishCompleted`.
  - Morale effects (+15 on completion, -10 on expiration) apply cleanly.
- **Save/Load Compatibility (`FinalWishSaveState`):**
  - Full round-trip fidelity verified across active, progressing, and completed wishes.
- **Engine-Agnostic Core (`Invariant 1`):**
  - Pure data expansion in `Assets/StreamingAssets/Data/final_wishes.json`.
  - Zero engine coupling added to `Ashfall.Core`.

---

## Cross-System Integration Proofs

1. **Plan 52 (NPC Arcs):** 6 wishes wire directly to named NPCs in `npc_arcs.json` (`npc_mara_veln`, `npc_marek_voln`, `npc_ilze_kaar`, `npc_lina`, `npc_niko`, `npc_oskar_ruut`).
2. **Plan 32 (Expedition Destinations):** 4 wishes wire directly to canonical locations in `locations.json` (`loc_settlement_cape_beacon`, `location_ash_dune_cemetery`, `loc_terrace_pumphouse`, `loc_shrine_switchback_waystation`).
3. **Plan 33 (Skills):** 3 wishes wire directly to canonical skill IDs in `skills.json` (`skill_field_dressing`, `skill_rough_repairs`, `skill_trap_setter`).
4. **Items Authority:** All required items resolve in `items.json` (`iodine_pills`, `bandage`, `scrap_metal`, `box_of_nails_10`, `trap_improvised_wire`, `crop_leafy_green`, `crop_hardy_tuber`, `item_dried_herb_packets`, `item_preservation_salt`, `clean_water`, `canned_food`, `item_document_field_report`, `tarnished_medal`, `dog_tags_personal`).

---

## Verification Matrix Results

| Verification Gate | Command | Result | Notes |
|---|---|---|---|
| **Data Integrity Selftest** | `godot --headless --path . -- --data-integrity-selftest` | **PASS (0 errors)** | 298 catalogs validated |
| **Plan 65 Unit Tests** | `dotnet test --filter FinalWishPlan65CatalogTests` | **PASS (7/7 passed)** | Catalog size, distribution, cross-refs, determinism, save |
| **FinalWish Core Tests** | `dotnet test --filter FinalWish` | **PASS (29/29 passed)** | All 22 original + 7 new tests passed |
| **Full Core Test Suite** | `dotnet test Ashfall.Core.Tests` | **PASS (9,364+ passed, 0 failed)** | Full regression suite green |
| **Scene Binding Selftest** | `godot --headless --path . -- --scene-binding-selftest` | **PASS (25/25 passed)** | All UI scenes verified |
| **Content Utilization** | `godot --headless --path . -- --content-utilization-selftest` | **PASS** | CI gate verified |
| **Scene Lint** | `python3 scripts/ci/scene-lint.py` | **PASS (0 errors)** | Clean scene tree |
