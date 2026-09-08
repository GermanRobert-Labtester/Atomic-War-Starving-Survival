# Plan 65 — Final Wishes Expansion: Regression Matrix

**Document Version:** 1.0.0
**Authority:** `Assets/StreamingAssets/Data/final_wishes.json` (30 wishes)
**System:** `Assets/Ashfall.Core/Survivors/FinalWishSystem.cs`
**Test Suite:** `Ashfall.Core.Tests/FinalWishPlan65CatalogTests.cs` (7/7 passing), `Ashfall.Core.Tests/FinalWishSystemTests.cs` (22/22 passing)

---

## 1. 20 Regression Scenarios & Verification Matrix

| # | Scenario / Contract Dimension | Expected Invariant & Boundary Condition | Verification Method & Target | Status |
|---|---|---|---|---|
| **1** | **Catalog Size & Schema Version** | Exactly 30 wishes in root `items` array; `schema_version` is integer `1`. | `Catalog_LoadsAndHasExactly30Wishes` | PASS |
| **2** | **Baseline 8 Wishes Preservation** | Original 8 archetypes (`the_surgeon`, `the_soldier`, `the_nurse`, `the_mother`, `the_mechanic`, `the_teacher`, `the_refugee`, `the_electrician`) preserved intact. | `Catalog_LoadsAndHasExactly30Wishes` | PASS |
| **3** | **Plan 65 Wish Type Distribution** | Exactly 22 new wishes matching the 10 requested types: 3 `teach_lesson`, 3 `deliver_letter`, 2 `see_a_place`, 2 `reconcile`, 3 `die_with_dignity`, 2 `last_meal`, 2 `confess`, 2 `protect_someone`, 2 `return_a_relic`, 1 `name_a_successor`. | `Catalog_WishTypeDistribution_MatchesPlan65Specification` | PASS |
| **4** | **Archetype Uniqueness** | All 30 entries possess distinct `archetype_id` strings (zero duplicate archetypes). | `Catalog_AllArchetypesAndTitles_AreUniqueAndValid` | PASS |
| **5** | **Survivor Roster Resolution** | All 22 new `archetype_id` values resolve to valid entries in `survivors.json`. | `Catalog_AllArchetypesAndTitles_AreUniqueAndValid` | PASS |
| **6** | **Wish Title Uniqueness** | All 30 entries possess distinct `wish_title` strings (zero duplicate titles). | `Catalog_AllArchetypesAndTitles_AreUniqueAndValid` | PASS |
| **7** | **Title Word Count Boundaries** | Every title contains between 2 and 5 words inclusive, matching ASHFALL naming conventions. | `Catalog_AllArchetypesAndTitles_AreUniqueAndValid` | PASS |
| **8** | **Description Phrasing & `{name}` Format** | Non-empty description string containing `{name}` placeholder for runtime survivor name formatting. | `Catalog_AllArchetypesAndTitles_AreUniqueAndValid` | PASS |
| **9** | **Dignity Content Guardrail** | All `die_with_dignity` entries strictly focus on privacy, ritual, witness, and palliative comfort; zero graphic/procedural elements. | Narrative audit & schema review | PASS |
| **10** | **Step Count Constraints** | Every wish possesses 2 to 4 executable steps (or legacy 1 step for `see_the_sky`). | `Catalog_AllArchetypesAndTitles_AreUniqueAndValid` | PASS |
| **11** | **Item References Resolution** | All `required_items` in new wishes resolve to existing items in `items.json`. | `Catalog_AllCrossReferences_ResolveInCanonicalCatalogs` | PASS |
| **12** | **Expedition Location Resolution** | All `requires_location` references resolve to canonical entries in `locations.json`. | `Catalog_AllCrossReferences_ResolveInCanonicalCatalogs` | PASS |
| **13** | **NPC Arc Cross-Catalog Resolution** | All `requires_npc` references resolve to valid entries in `npc_arcs.json`. | `Catalog_AllCrossReferences_ResolveInCanonicalCatalogs` | PASS |
| **14** | **Skill Transfer Resolution** | All `skill_transfer` targets resolve to valid skill definitions in `skills.json`. | `Catalog_AllCrossReferences_ResolveInCanonicalCatalogs` | PASS |
| **15** | **Plan 52 NPC Arc Handoff** | ≥5 wishes wire into named NPC arcs (`npc_mara_veln`, `npc_marek_voln`, `npc_ilze_kaar`, `npc_lina`, `npc_niko`, `npc_oskar_ruut`). | Cross-reference assertion & handoff doc | PASS |
| **16** | **Plan 32 Expedition Handoff** | ≥3 wishes wire into canonical expedition destinations (`loc_settlement_cape_beacon`, `location_ash_dune_cemetery`, `loc_terrace_pumphouse`, `loc_shrine_switchback_waystation`). | Cross-reference assertion & handoff doc | PASS |
| **17** | **Progression Across All 10 Types** | `FinalWishSystem.AdvanceWishStep` cleanly increments and completes wishes across all 10 type variants without exceptions. | `FinalWishSystem_All10WishTypes_ProgressAndCompleteDeterministically` | PASS |
| **18** | **Seeded RNG Determinism** | Prognosis duration and deterministic selection produce identical results under matching `ISeededRng` seeds. | `FinalWishSystem_DeterministicSelection_HoldsUnderSeed` | PASS |
| **19** | **Save/Load Round-Trip Fidelity** | Full capture and restore via `FinalWishSaveState` preserves active/completed flags, step counts, wish types, and remaining days. | `FinalWishSystem_SaveLoad_FullRoundTrip_PreservesAllState` | PASS |
| **20** | **Global Project Invariants** | Zero engine coupling in Core (`Ashfall.Core`), deterministic PRNG (`ISeededRng`), and JSON data authority compliance. | `--data-integrity-selftest` & `DeterminismGuardTests` | PASS |

---

## 2. Risk Mitigation Audit

1. **Risk 1: Archetype Collisions**
   *Mitigation:* Validated 30/30 unique archetype IDs across the catalog. Cross-referenced against `survivors.json` to guarantee all new IDs map to genuine survivors in the roster.

2. **Risk 2: Broken Cross-Catalog References**
   *Mitigation:* Mechanically asserted every item (`items.json`), location (`locations.json`), NPC (`npc_arcs.json`), and skill (`skills.json`) in `FinalWishPlan65CatalogTests.Catalog_AllCrossReferences_ResolveInCanonicalCatalogs`.

3. **Risk 3: Runtime Switch Mismatch**
   *Mitigation:* Confirmed `FinalWishSystem.cs` defaults unmapped types to 2 steps via `_ => 2`. Authored all new multi-step wishes with 2 explicit executable steps, ensuring immediate runtime harmony.

4. **Risk 4: Inappropriate Tone / Content Creep**
   *Mitigation:* Audited all 3 `die_with_dignity` wishes against project narrative rules. Focus remains strictly on dignified parting, palliative tea, personal effects, and solemn quietude without procedural or sensationalized elements.

5. **Risk 5: Save State Corruption**
   *Mitigation:* Verified that `FinalWishSaveState` serialization remains backward and forward compatible, with zero changes to schema structures.
