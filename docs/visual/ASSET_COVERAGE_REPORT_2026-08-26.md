# ASHFALL Visual Asset Coverage Report

> **Date**: 2026-08-26
> **Source**: `godot --headless --path . -- --asset-coverage-report`
> **Policy**: Report-only / non-gating diagnostic. Hard CI gate remains `--asset-registry-selftest`.

---

## Executive Summary

| Category | Total IDs | Resolved Assets | Missing Assets | Coverage Rate |
|---|---|---|---|---|
| **Items** (`item`) | 315 | 287 | 28 | **91.11%** |
| **Factions** (`faction`) | 43 | 36 | 7 | **83.72%** |
| **Portraits** (`portrait`) | 205 | 108 | 97 | **52.68%** |
| **Locations** (`location`) | 271 | 50 | 221 | **18.45%** |
| **TOTAL** | **834** | **481** | **353** | **57.67%** |

---

## Key Observations & Top Missing Categories

1. **Items (91.1% Coverage)**:
   - High production readiness. Core crafting, food, medical, weapons, ammunition, and gear have dedicated sprites.
   - Missing 28 IDs are primarily niche expansion materials (e.g. `item_ammonium_nitrate_sack`, `item_corrosion_inhibitor_drum`, `item_artillery_fuze_wrench`, `projector_bulb`).

2. **Factions (83.7% Coverage)**:
   - Major factions have authored emblems in `assets/ui/FactionEmblems/` and `assets/sprites/Factions/`.
   - Missing 7 IDs are legacy aliases or sector-specific variants (`iron_garrison`, `cult_of_ash_sign`, `warlords_sector_4`, `faction_scavengers`, `faction_unaligned`, `raiders`, `faction_forward_roster`).

3. **Portraits (52.7% Coverage)**:
   - Core starting crew and major campaign survivors have high-fidelity portraits.
   - Missing 97 IDs comprise procedural cohort names, generic recruits, and contractor pool additions. Fallback portrait (`placeholder_survivor.png`) safely resolves unskinned characters.

4. **Locations (18.5% Coverage)**:
   - Largest art backlog category (221 missing IDs).
   - Core Holdfast rooms and primary map nodes are authored, but wasteland expansion sites and sector POIs rely on danger-colored node markers (`MapLocationMarkerView`) and fallback location illustrations.

---

## Item Missing List (28 IDs)

- `spring_mechanism`
- `phonograph_needle`
- `projector_bulb`
- `film_reel`
- `soldering_kit`
- `item_ammonium_nitrate_sack`
- `item_corrosion_inhibitor_drum`
- `item_icebreaker_rendezvous_flare_rocket`
- `item_artillery_fuze_wrench`
- `item_brass_stamping_die`
- `item_lead_shielding_sheeting`
- `item_pyrite_concentrate_charge`
- `item_heavy_water_phial`
- `item_refractory_furnace_brick`
- `item_hardened_boron_carbide_core`
- `item_meridian_compass`
- `item_black_flotilla_code_ribbon`
- `item_deep_scavenge_hook`
- `item_district8_passage_marker`
- `item_ice_road_survey_transit`
- `item_shelf_diver_knife`
- `item_brine_scale_scraper`
- `item_ice_chain_cleats`
- `item_salt_corrosion_paste`
- `item_steam_accumulator_valve`
- `item_flotilla_diving_helm`
- `item_depth_sounding_lead`
- `item_icebreaker_coal_brick`

---

## Faction Missing List (7 IDs)

- `iron_garrison`
- `cult_of_ash_sign`
- `warlords_sector_4`
- `faction_scavengers`
- `faction_unaligned`
- `raiders`
- `faction_forward_roster`

---

## Portrait Missing List (97 IDs)

- `survivor_family_child`
- `alex_raymond`
- `jamie_chen`
- `taylor_morgan`
- `riley_cooper`
- `jordan_kim`
- `casey_garcia`
- `morgan_lee`
- `drew_paterson`
- `cam_nguyen`
- `avery_ross`
- `sam_hayes`
- `dakota_turner`
- `reese_campbell`
- `quinn_brooks`
- `hayden_ward`
- `rowan_gray`
- `kai_mitchell`
- `sawyer_adams`
- `finley_price`
- `skyler_bennett`
- `charlie_foster`
- `parker_simpson`
- `blake_morris`
- `peyton_hughes`
- `devon_flores`
- `shiloh_graham`
- `tatum_owens`
- `haven_sullivan`
- `lennon_bryant`
- `dallas_alexander`
- `ellis_knight`
- `bellamy_hunt`
- `briar_griffin`
- `carter_barnes`
- `chandler_wells`
- `eden_powers`
- `emerson_cobb`
- `harley_dixon`
- `holland_fletcher`
- `indiana_manning`
- `jules_norton`
- `kelsey_quinn`
- `marlowe_rhodes`
- `monroe_schultz`
- `oakley_thornton`
- `presley_underwood`
- `remy_vance`
- `robin_walsh`
- `salem_york`
- `tobin_zimmerman`
- `val_archer`
- `wren_bowman`
- `august_cross`
- `baker_delaney`
- `callum_english`
- `darcy_frost`
- `eliot_glover`
- `florian_hardy`
- `gentry_irwin`
- `hollis_jarvis`
- `joss_keller`
- `keaton_lombardi`
- `landry_mercer`
- `milo_navarro`
- `noe_ochoa`
- `orion_pollard`
- `perry_robbins`
- `quincy_slater`
- `roan_tanner`
- `sloan_upchurch`
- `thayer_villarreal`
- `uriah_whitaker`
- `vaughn_xenakis`
- `wells_yates`
- `zephyr_zamora`
- `arlo_boone`
- `bodhi_castillo`
- `cyrus_drake`
- `dashiell_eaton`
- `enoch_faulkner`
- `foster_gaines`
- `gideon_holloway`
- `harris_iverson`
- `idris_jennings`
- `judah_kemp`
- `kian_langley`
- `lucian_monroe`
- `maccabee_norris`
- `neal_oakes`
- `otto_pruitt`
- `pierson_quick`
- `rufus_rowland`
- `silas_stafford`
- `titus_underhill`
- `ulysses_vance`
- `viggo_wentworth`

---

## Location Missing List (Top 50 of 221 IDs)

- `loc_veterinary_surgery`
- `loc_school_gymnasium`
- `loc_cider_press`
- `loc_terrace_pumphouse`
- `loc_ration_queue_plaza`
- `loc_conscription_office`
- `loc_municipal_archive`
- `loc_dentists_row`
- `loc_transit_authority_hq`
- `loc_printworks`
- `loc_bakers_cellar`
- `loc_post_sorting_shed`
- `loc_weavers_loft`
- `loc_ironmongers_yard`
- `loc_constabulary_lockup`
- `loc_cartwrights_workshop`
- `loc_tallow_chandlery`
- `loc_bell_foundry`
- `loc_coopers_coppice`
- `loc_dyers_basin`
- `loc_locksmiths_tenement`
- `loc_slaughterhouse_gut`
- `loc_mason_lodge_ruin`
- `loc_grain_merchant_vault`
- `loc_shipwrights_slipway`
- `loc_apothecary_storehouse`
- `loc_ropewalk_gallery`
- `loc_brewery_cellars`
- `loc_wheelwrights_pond`
- `loc_saddlers_garret`
- `loc_glassworks_lehr`
- `loc_potters_kiln`
- `loc_maltings_floor`
- `loc_limekiln_bank`
- `loc_tan_pit_trench`
- `loc_charcoal_burners_hut`
- `loc_brickfields_clamp`
- `loc_fulling_mill_race`
- `loc_bone_mill_hopper`
- `loc_quarrymans_shanty`
- `loc_coppersmiths_forge`
- `loc_millwrights_dam`
- `loc_turners_shed`
- `loc_glaziers_bench`
- `loc_cutlers_grindery`
- `loc_needle_makers_walk`
- `loc_tallow_press_room`
- `loc_soap_boilers_vat`
- `loc_lead_smelters_flue`
- `loc_nailers_smithy`
*(plus 171 additional wasteland & expedition location IDs)*

---

## Runtime Architecture & Gating Notes

- **Report-Only Status**: `--asset-coverage-report` remains report-only. Missing entries in this report do not cause build or CI failures.
- **Authoritative Gate**: The build/CI gate is `--asset-registry-selftest`, which validates:
  - Resolution and loading of top 50 critical gameplay catalog IDs.
  - Presence and non-null texture instantiation of standard fallbacks (`placeholder_survivor.png`, `icon_placeholder.png`).
  - Normalization probes across candidate asset stems and path conventions.
- **Graceful Degradation**: When a referenced asset is unauthored, `AssetRegistry` logs a diagnostic warning and routes to standard fallback textures without raising exceptions or interrupting gameplay loops.
