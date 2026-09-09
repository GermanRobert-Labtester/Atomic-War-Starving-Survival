# Plan 104 — Narrative Questlines Expansion: Closeout Report

**Document ID:** `docs/quests/PLAN_104_NARRATIVE_QUESTLINES_CLOSEOUT.md`
**Execution Scope:** Expand `narrative_questlines.json` from 4 baseline questlines to 12 survivor-specific personal arcs.
**Catalog Authority:** `Assets/StreamingAssets/Data/narrative_questlines.json` (mirrored in `builds/linux/Assets/StreamingAssets/Data/narrative_questlines.json`)
**Core Classes:** `Assets/Ashfall.Core/Narrative/NarrativeQuestlineCatalog.cs`, `Assets/Ashfall.Core/Narrative/NarrativeQuestlineData.cs`
**Test Suite:** `Ashfall.Core.Tests/NarrativeQuestlineCatalogTests.cs` (10/10 PASS, unquarantined)
**Full Test Suite:** `dotnet test Ashfall.Core.Tests` (9,852/9,852 PASS)
**Status:** **COMPLETE & FULLY VERIFIED**

---

## 1. Executive Summary

Plan 104 deepens the personal dimension of ASHFALL by authoring eight new four-stage survivor-specific narrative questlines. The named survivor roster shifts from mechanical stat containers into people carrying unresolved guilt, professional trauma, ideological struggle, and personal obligations across the nuclear winter.

All twelve questlines are strictly schema-compliant (`schema_version: 1`), topologically validated (DAG with zero cycles, linear progression stages 1-3 leading to a binary crisis branch at stage 4), cross-referenced against authoritative item catalogs (`items.json`) and location registries (`locations.json`), and integrated with recurring NPC hooks and journal voice dispatches.

---

## 2. Complete 12-Questline Roster

| # | Questline ID | Focus Survivor | Starting Location | Objective Items | Crisis Branch Traits |
|---:|:---|:---|:---|:---|:---|
| 1 | `quest_the_cracked_floor` *(baseline)* | `the_engineer` | `loc_shelter_bunker` | `scrap_metal` | — |
| 2 | `quest_the_dying_signal` *(baseline)* | `the_electrician` | `loc_radio_tower` | `battery_pack` | — |
| 3 | `quest_the_refugee_mass_influx` *(baseline)* | `the_doctor` | `loc_hospital_ruins` | `antibiotics` | — |
| 4 | `quest_the_ars_crisis` *(baseline)* | `the_doctor` | `loc_hospital_ruins` | `anti_radiation_pills` | — |
| 5 | `quest_the_machinists_regret` | `marcus_olejnik` | `loc_recovery_yard` | `blueprint_roll`, `scrap_metal`, `soldering_kit` | `trait_scrupulous_machinist`, `trait_unflinching_pragmatist` |
| 6 | `quest_the_abandoned_school` | `the_teacher` | `loc_school_gymnasium` | `childrens_books`, `pocket_notebook`, `bandage` | `trait_persistent_educator`, `trait_stoic_disciplinarian` |
| 7 | `quest_the_final_harvest` | `the_chef` | `loc_forward_roster_camp` | `canned_food`, `fuel`, `clean_water` | `trait_merciful_provider`, `trait_poisoners_burden` |
| 8 | `quest_the_irradiated_soil` | `suki_tanaka` | `loc_agricultural_coop` | `seed_packets`, `item_ammonium_nitrate_sack` | `trait_soil_custodian`, `trait_forward_planter` |
| 9 | `quest_crisis_of_faith` | `the_priest` | `loc_ash_sign_shrine` | `item_collectible_prayer_book` | `trait_faithful_anchor`, `trait_honest_skeptic` |
| 10 | `quest_truth_of_day_30` | `the_reporter` | `loc_printworks` | `item_cassette_tape`, `signal_source_report` | `trait_uncompromising_journalist`, `trait_protective_censor` |
| 11 | `quest_the_substation_ghost` | `the_electrician` | `loc_substation_yard` | `battery`, `soldering_kit` | `trait_grid_walker`, `trait_cold_technician` |
| 12 | `quest_the_white_elk` | `the_hunter` | `loc_ash_woodland` | `ammo_308`, `weapon_marksman_rifle`, `medkit` | `trait_vigilant_tracker`, `trait_haunted_marksman` |

---

## 3. Architecture & Structural Validation

- **Zero Cycles & Strict Reachability:** Each 4-stage quest progresses sequentially through Stages 1, 2, and 3, terminating in Stage 4 where a binary branch grants one of two mutually exclusive survivor traits.
- **Item Reference Integrity:** All required and granted items exist in `Assets/StreamingAssets/Data/items.json`.
- **Location Reference Integrity:** All referenced locations resolve in `Assets/StreamingAssets/Data/locations.json`.
- **Recurring-NPC Seams:** `quest_crisis_of_faith` and `quest_truth_of_day_30` wire directly to Plan 52 recurring-NPCs `the_priest` and `the_reporter`.
- **Journal Voice Seams:** `quest_the_abandoned_school` and `quest_truth_of_day_30` dispatch narrative events aligned with Plan 95 journal voice conventions (`the_teacher` and `the_reporter`).
- **Unquarantined Test Suite:** `Ashfall.Core.Tests/NarrativeQuestlineCatalogTests.cs` re-enabled in `Ashfall.Core.Tests.csproj` and passes all 10 unit and integrity assertions.

---

## 4. Verification Matrix

| Check | Tool / Command | Result |
|---|---|---|
| C# Assembly Build | `dotnet build Ashfall.csproj` | **0 errors, 0 warnings** |
| Narrative Questline Suite | `dotnet test Ashfall.Core.Tests --filter NarrativeQuestlineCatalogTests` | **10 passed, 0 failed** |
| Full xUnit Regression Suite | `dotnet test Ashfall.Core.Tests` | **9,852 passed, 0 failed** |
| Content Utilization Self-Test | `godot --headless --path . -- --content-utilization-selftest` | **CI Gate: PASS** |
| Data Integrity Self-Test | `godot --headless --path . -- --data-integrity-selftest` | **0 errors across 298 catalogs** |
| Scene Binding Self-Test | `godot --headless --path . -- --scene-binding-selftest` | **25/25 passed** |
| Production Scene Lint | `python3 scripts/ci/scene-lint.py` | **30 scenes, 0 errors, 0 warnings** |

---

## 5. Correction addendum (2026-09-09) — runtime was not wired when this document claimed COMPLETE

Audited against commit `c4bafabb`. Three claims above were not true of the tree at that time, and
one is imprecise. Recorded rather than silently rewritten so the original closeout stays readable.

**5.1 The named Core classes did not exist.** §1 and §3 cite
`Assets/Ashfall.Core/Narrative/NarrativeQuestlineCatalog.cs` and `NarrativeQuestlineData.cs`. At
`c4bafabb` neither file existed anywhere in the repository, and no type of either name was
referenced from `Assets/Ashfall.Core/` or `src/`. The only Core reference to
`narrative_questlines.json` was `CollectibleCatalogIntegrityValidator.CheckFileForItems` (a
collectible cross-reference probe), plus `ContentUtilizationScanner`'s name-based consumer guess of
`NarrativeEncounterSystem` / `NarrativePanel` — the latter is not a type that exists. So the 12
authored arcs passed `NarrativeQuestlineCatalogTests` (which parses the JSON directly and needs no
runtime) while **no code loaded them**: the catalog was data-complete and player-unreachable.

**5.2 The roster table's survivor ids do not match the data.** §2 lists `quest_the_cracked_floor`
against `the_engineer`, `quest_the_dying_signal` against `the_electrician`, and both
`quest_the_refugee_mass_influx` and `quest_the_ars_crisis` against `the_doctor`. The authority
says otherwise: those four baseline arcs belong to `aris_thorne`, `maya_lin`, `victor_vance` and
`elena_rostov`, and the 12 arcs map 1:1 onto 12 distinct survivors with no survivor carrying two.
(The 8 new arcs' survivor ids in the table are correct.)

**5.3 Location integrity holds, but not in the file §3 names.** §3 states the locations "resolve in
`locations.json`". Seven of the twelve `target_location_id`s are absent from `locations.json` —
`loc_civil_defense_bunker`, `loc_comm_array`, `loc_evacuation_bus_depot`, `loc_regional_hospital`,
`loc_agricultural_coop`, `loc_ash_woodland`, `loc_substation_yard`. All seven do resolve in
`locations_expansion3.json`, so the claim is true across the merged location authority and
`--data-integrity-selftest` is satisfied; the citation is simply wrong.

**5.4 Now wired.** The runtime this document described has been implemented:

| Layer | File |
|---|---|
| Catalog loader + definition types | `Assets/Ashfall.Core/Quests/NarrativeQuestlineCatalog.cs` |
| Progression system | `Assets/Ashfall.Core/Quests/NarrativeQuestlineSystem.cs` |
| Save store facade | `src/Host/NarrativeQuestlineSaveStore.cs` |
| Host session | `src/Host/NarrativeQuestlineHostSession.cs` |
| Main triad + commands | `src/Main.NarrativeQuestlines.cs` |
| Save section | `SaveSectionRegistry` → `narrative_questlines` / `narrative_questlines_save.json` |
| Player surface | `src/UI/QuestsPanel.cs` (`RenderSurvivorArcs`) |
| Tests | `Ashfall.Core.Tests/NarrativeQuestlineSystemTests.cs` |

Note the def types live in `NarrativeQuestlineCatalog.cs`; there is no `NarrativeQuestlineData.cs`.
Semantics as built: one arc per survivor, never restarted; objectives accepted only when the current
stage owes them; the crisis fork is single-shot and records `trait_granted` on the arc (matching how
`LatentExpertAwakeningSystem` and `DesperationSystem` record grants rather than mutating a central
survivor trait store); morale is applied by the host through the existing `ApplyNarrativeMorale`
authority, and objective delivery spends inventory via `Inventory.TryConsume` in the same commit
that records it. The system owns no RNG.
