# Plan 10 — Combat & Expedition Depth: Bestiary, Armory & the Fleet Completion Report

**Document:** `docs/combat/PLAN10_COMPLETION_REPORT.md`
**Status:** COMPLETE & FULLY INTEGRATED
**Date:** 2026-09-01
**Project:** ASHFALL (Godot 4.7+ .NET Mono Host / .NET 8 / .NET 9 Tests)

---

## 1. Executive Summary

Plan 10 expands and integrates the combat, equipment, warlord doctrine, vehicle logistics, and deep-coast dive systems into a cohesive, deep gameplay layer without replacing the proven Core architectures. All authored content is strictly data-authority-backed, engine-agnostic, and verified through both .NET tests and Godot headless self-tests.

---

## 2. Baseline vs Final Delivered Counts

| Area | Baseline | Target | Final Delivered | Authoritative File |
|---|---|---|---|---|
| **Combatant Bestiary** | 0 | 10 | **10** (6 fauna/mutant + 4 human) | `Assets/StreamingAssets/Data/combat_catalog.json` |
| **Warlord Doctrines** | 4 | 8 | **8** (4 core + 4 expanded) | `Assets/StreamingAssets/Data/warlord_doctrines.json` |
| **Weapons** | 5 | 15+ | **15** (improvised, military, relic) | `Assets/StreamingAssets/Data/combat_catalog.json` |
| **Ammunition Types** | 5 | 11+ | **14** (standard, hand-loaded, special) | `Assets/StreamingAssets/Data/combat_catalog.json` |
| **Expedition Vehicles** | 3 | 8 | **8** (specialized logistics fleet) | `Assets/StreamingAssets/Data/vehicles.json` |
| **Deep-Coast Dive Sites** | 4 | 12 | **12** (tiered noise & hazard wrecks) | `Assets/StreamingAssets/Data/dive_sites.json` |

---

## 3. Workstream Deliverables Summary

### 3.1 Bestiary & Warlord Roster (Task 10A)
- **10 Authored Combatants:** 6 fauna/mutants (`combatant_burrower_mite`, `combatant_spore_hound`, `combatant_armored_boar`, `combatant_feral_mutt`, `combatant_pale_crawler`, `combatant_chrome_loper`) with distinct AI stances and special moves (`Burrow`, `Spore`, `Charge`, `Flank`), plus 4 human archetypes (`combatant_conscript_levy`, `combatant_warlord_veteran`, `combatant_flotilla_marine`, `combatant_desperate_scavenger`) retaining non-lethal surrender/bribery/flee thresholds.
- **8 Warlord Doctrines:** 4 original doctrines (`The Toll`, `Holding the Line`, `The Long Reach`, `Gone to Ground`) plus 4 new strategic identities (`The Cold Siege` / Brenner, `The Slave Ledger` / Mireles, `The Ash Cant` / Asha, `The Pincer Manual` / Okov) with 3–4 live response actions, distinct preferred goals, and dynamic transition rules.

### 3.2 Armory & Ammunition Expansion (Task 10B)
- **15 Weapons:** Balanced across improvised availability (`weapon_pipe_rifle`, `weapon_scrap_shotgun`, `weapon_pipe_shotgun`, `weapon_nail_driver`, `weapon_rebar_spear`, `weapon_molotov_thrower`, `weapon_farm_carbine`), pristine military performance (`weapon_assault_rifle`, `weapon_lmg`, `weapon_service_rifle`, `weapon_marksman_rifle`, `weapon_smg`, `weapon_sidearm`), and degraded relic reliability (`weapon_rust_mosin`, `weapon_bolt_rifle`).
- **14 Ammunition Types:** Standard calibers (.357, 12ga, .308, 5.56, 7.62, 9x19, .22LR, 7.62x54R), hand-loaded specialty variants (.357 JHP, 12ga Buckshot, .308 Incendiary, 5.56 Subsonic), and improvised charges (`ammo_improvised_rod`, `ammo_improvised_burn`).
- **Ballistics & Condition Integration:** Verified cover materials (wood, concrete, metal, rebar barricade) and armor classes (padded cloth, kevlar, ceramic plate) with degradation and jam curves.

### 3.3 Vehicle Fleet & Dive Sites (Task 10C)
- **8 Expedition Vehicles:** Differentiated logistics fleet (`vehicle_utility_quad`, `vehicle_dirt_bike`, `vehicle_cargo_truck`, `vehicle_steam_halftrack`, `vehicle_armored_mobile_base`, `vehicle_salvage_dredger`, `vehicle_scout_motorcycle`, `vehicle_ambulance_rig`) with distinct speed, fuel consumption, cargo capacity, terrain restrictions, and breakdown probabilities.
- **12 Deep-Coast Dive Sites:** 12 authored underwater sites with oxygen budgets (70–120 ticks), noise floors (0.40–0.80), 4-room exploration profiles, and tiered hazards/loot.

---

## 4. Canonical Verification Matrix

| Verification Gate | Command | Result |
|---|---|---|
| **Unit Tests** | `dotnet test Ashfall.Core.Tests` | **PASS** (5,317 passed, 0 failed, 16s) |
| **Data Integrity** | `godot --headless --path . -- --data-integrity-selftest` | **PASS** (0 errors across 138 catalogs, 5,563 IDs) |
| **Content Utilization** | `godot --headless --path . -- --content-utilization-selftest` | **PASS** (CI Gate PASS, 413 catalogs) |
| **Scene Bindings** | `godot --headless --path . -- --scene-binding-selftest` | **PASS** (22/22 scenes bound) |
| **Accessibility Gate** | `godot --headless --path . -- --ui-accessibility-selftest` | **PASS** (5/5 gates passed) |
| **Onboarding Journey** | `godot --headless --path . -- --onboarding-journey-selftest` | **PASS** (20/20 assertions passed) |
| **Scene Tree Linter** | `python3 scripts/ci/scene-lint.py` | **PASS** (26 scenes, 0 errors) |
| **Audio Catalog Sync** | `python3 scripts/ci/generate-audio-catalog.py --check` | **PASS** (74 cues in sync) |
