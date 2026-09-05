# Plan 87 — Relic Recipes Expansion: 6 → 15 Workshop Restoration Relics Closeout Report

## 1. Executive Summary

- **Status:** **COMPLETE**
- **Domain:** Workshop Relic Restoration Catalog Expansion
- **Authoritative Data Authority:** `Assets/StreamingAssets/Data/relic_recipes.json`
- **Scope Compliance:** Pure DATA and narrative authoring pass. Zero Core gameplay code modifications, zero new save DTOs, zero engine coupling.

Plan 87 successfully expands ASHFALL's workshop restoration progression from six pre-war cultural relics to a complete fifteen-relic arc. Each relic offers a distinct human and mechanical payoff, consuming scavenged pre-war components and granting a balanced one-time morale bonus, a narrative event in `events.json`, and a persistent world-state flag.

---

## 2. Authoritative Catalog & Schema Confirmation

- **Ambiguity Cleared:** `relic_inks.json` was an early drafting typo and does not exist in the codebase. `relic_recipes.json` is the sole authoritative catalog.
- **Loader:** `Ashfall.Core.Crafting.RelicCatalogLoader`
- **Runtime System:** `Ashfall.Core.WorkshopReverseEngineeringSystem`
- **Total Relics in File:** 39 entries (15 cultural restoration relics + 24 technical reverse-engineering blueprints from Plan 04).

---

## 3. The 15 Workshop Restoration Relics

### The 6 Baseline Relics (Preserved Verbatim)
1. **`gramophone` (Hand-Crank Gramophone)**
   - Components: `vacuum_tube`, `spring_mechanism`, `phonograph_needle`
   - Repair Time: 8 hours | Morale: +5
   - Narrative Event: `narrative_gramophone_restored`
   - World Flag: `relic_restored_gramophone`
2. **`film_projector` (8mm Film Projector)**
   - Components: `projector_bulb`, `lubricant_oil`, `film_reel`
   - Repair Time: 6 hours | Morale: +5
   - Narrative Event: `narrative_projector_restored`
   - World Flag: `relic_restored_film_projector`
3. **`ham_radio` (Vintage Ham Radio Set)**
   - Components: `vacuum_tube`, `antenna_coil`, `soldering_kit`
   - Repair Time: 12 hours | Morale: +5
   - Narrative Event: `narrative_ham_radio_restored`
   - World Flag: `relic_restored_ham_radio`
4. **`music_box` (Antique Music Box)**
   - Components: `music_box_comb`, `spring_key`
   - Repair Time: 4 hours | Morale: +3
   - Narrative Event: `narrative_music_box_restored`
   - World Flag: `relic_restored_music_box`
5. **`typewriter` (Mechanical Typewriter)**
   - Components: `typewriter_ribbon`, `machine_oil`
   - Repair Time: 3 hours | Morale: +3
   - Narrative Event: `narrative_typewriter_restored`
   - World Flag: `relic_restored_typewriter`
6. **`camera` (Twin-Lens Reflex Camera)**
   - Components: `camera_lens_cleaner`, `photographic_film`
   - Repair Time: 5 hours | Morale: +3
   - Narrative Event: `narrative_camera_restored`
   - World Flag: `relic_restored_camera`

### The 9 Authored Additions (Plan 87)
7. **`mantel_clock` (Brass Mantel Clock)**
   - Niche: Shared Routine & Predictable Hours
   - Components: `spring_mechanism`, `mechanical_parts`, `machine_oil`
   - Repair Time: 6 hours | Morale: +4
   - Narrative Event: `narrative_mantel_clock_restored`
   - World Flag: `relic_restored_mantel_clock`
8. **`sewing_machine` (Treadle Sewing Machine)**
   - Niche: Domestic Craft & Mending as Care
   - Components: `mechanical_parts`, `lubricant_oil`, `leather_strap`
   - Repair Time: 7 hours | Morale: +4
   - Narrative Event: `narrative_sewing_machine_restored`
   - World Flag: `relic_restored_sewing_machine`
9. **`telescope` (Brass Refractor Telescope)**
   - Niche: Wonder & Gazing Beyond Immediate Danger
   - Components: `optical_lens`, `mechanical_parts`, `scrap_metal`
   - Repair Time: 9 hours | Morale: +5
   - Narrative Event: `narrative_telescope_restored`
   - World Flag: `relic_restored_telescope`
10. **`hand_printing_press` (Tabletop Platen Press)**
    - Niche: Public Communication, Memory, & Civic Voice
    - Components: `mechanical_parts`, `empty_toner_cartridge`, `wooden_plank`
    - Repair Time: 10 hours | Morale: +5
    - Narrative Event: `narrative_printing_press_restored`
    - World Flag: `relic_restored_printing_press`
11. **`violin` (Spruce & Maple Violin)**
    - Niche: Live Music Created by the Living
    - Components: `wooden_plank`, `copper_wire_10m_of_10m`, `leather_strap`
    - Repair Time: 8 hours | Morale: +5
    - Narrative Event: `narrative_violin_restored`
    - World Flag: `relic_restored_violin`
12. **`laboratory_microscope` (Monocular Compound Microscope)**
    - Niche: Disciplined Curiosity & Scientific Education *(Replaced proposed Slide Projector to eliminate duplicate visual projection niche)*
    - Components: `optical_lens`, `camera_lens_cleaner`, `mechanical_parts`
    - Repair Time: 8 hours | Morale: +4
    - Narrative Event: `narrative_microscope_restored`
    - World Flag: `relic_restored_laboratory_microscope`
13. **`brass_compass` (Prismatic Marching Compass)**
    - Niche: Direction & Physical Certainty
    - Components: `spring_mechanism`, `mechanical_parts`, `scrap_metal`
    - Repair Time: 4 hours | Morale: +3
    - Narrative Event: `narrative_compass_restored`
    - World Flag: `relic_restored_brass_compass`
14. **`box_kite` (Weather-Station Box Kite)**
    - Niche: Unadulterated Play & Beauty
    - Components: `cloth`, `scrap_wood`, `rope`
    - Repair Time: 3 hours | Morale: +2
    - Narrative Event: `narrative_kite_restored`
    - World Flag: `relic_restored_box_kite`
15. **`coffee_grinder` (Cast-Iron Coffee Mill)**
    - Niche: Domestic Morning Ritual & Hospitality
    - Components: `mechanical_parts`, `scrap_metal`, `machine_oil`
    - Repair Time: 4 hours | Morale: +3
    - Narrative Event: `narrative_coffee_grinder_restored`
    - World Flag: `relic_restored_coffee_grinder`

---

## 4. Component Economy & Item Additions

- **Component Reuse:** 8 of the 9 new relics consume exclusively existing items from `items.json` (`spring_mechanism`, `mechanical_parts`, `machine_oil`, `lubricant_oil`, `leather_strap`, `scrap_metal`, `empty_toner_cartridge`, `wooden_plank`, `copper_wire_10m_of_10m`, `camera_lens_cleaner`, `cloth`, `scrap_wood`, `rope`).
- **Single Added Component:** `optical_lens` ("Optical Lens Element"). A reusable precision glass optical element shared between `telescope` and `laboratory_microscope`. Tagged as `relic_component` in `expansion_item_tags.json`.
- **Item Tagging Parity:** All 9 new relics are registered with the `relic_restorable` tag in `expansion_item_tags.json`.

---

## 5. Integration Hooks (Plans 47 & 76)

- **Plan 47 (Collectibles):**
  The 3 primary cultural collectible candidates are `brass_compass`, `violin`, and `telescope`. Tagged with `relic_restorable` in `expansion_item_tags.json`.
- **Plan 76 (Expedition Destinations):**
  Committed world location anchors identified:
  - `loc_summit_relay` / `loc_snowline_station` for `telescope`
  - `loc_printworks` / `loc_municipal_archive` for `hand_printing_press`
  - `loc_low_background_lab` for `laboratory_microscope`

---

## 6. Verification & Test Evidence

All canonical gates executed and verified clean:

| Gate / Command | Result | Details |
|---|---|---|
| `dotnet test Ashfall.Core.Tests` | **PASS (0 failed)** | 6,893 tests passed (36s duration) |
| `godot --headless --path . -- --data-integrity-selftest` | **PASS (0 findings)** | 10,838 IDs checked across 208 catalogs; 0 errors, 0 warnings |
| `godot --headless --path . -- --content-utilization-selftest` | **PASS (CI gate PASS)** | 490 catalogs scanned; 0 orphans |
| `godot --headless --path . -- --scene-binding-selftest` | **PASS (22/22)** | 22 scenes validated |
| `python3 scripts/ci/scene-lint.py` | **PASS (0 errors)** | 27 production scenes checked |
| `dotnet build Ashfall.csproj` | **PASS (0 errors)** | Clean host compilation |
