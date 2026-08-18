# ASHFALL — Duplicate Consolidation Plan

Phase 14T. **DOES NOT delete or move files.** Output is a documented plan only.

Total exact-duplicate groups: **182**

Grouped by rank:

| Rank | Groups | Estimated disk waste |
|---|---|---|
| `HIGH` | 2 groups | ~2,242,697 bytes (2190.1 KiB) |
| `MEDIUM` | 113 groups | ~8,114,566 bytes (7924.4 KiB) |
| `LOW` | 67 groups | ~17,630,387 bytes (17217.2 KiB) |
| **Total** | **182 groups** | **~27,987,650 bytes (26.69 MiB)** |

## HIGH-rank groups (deprecated / active pairs)

These have the strongest single deletion signal: byte-identical; relationship is
explicit by naming (e.g. `ammo_9x19.jpg` ↔ `ammo_deprecated_9x19.jpg`).

| Group sha | Stems | Save |
|---|---|---|
| `687db5aef4...` | `ammo_`, `ammo_12ga_ap`, `ammo_12ga_buck`, `ammo_12ga_slug`, `ammo_16ga_buck`, `ammo_16ga_slug`, `ammo_300blk`, `ammo_300blk_ap`, `ammo_300blk_api`, `ammo_300blk_exi`, `ammo_300blk_fmj`, `ammo_338lapua`, `ammo_338lapua_ap`, `ammo_338lapua_api`, `ammo_338lapua_bt`, `ammo_338lapua_exi`, `ammo_380acp_fmj`, `ammo_380acp_jhp`, `ammo_408cheytac`, `ammo_408cheytac_ap`, `ammo_408cheytac_api`, `ammo_408cheytac_bt`, `ammo_408cheytac_exi`, `ammo_408cheytac_jhp_ap`, `ammo_45acp_ap`, `ammo_45acp_fmj`, `ammo_45acp_jhp`, `ammo_46x30_ap`, `ammo_46x30_fmj`, `ammo_50bmg`, `ammo_50bmg_ap`, `ammo_50bmg_api`, `ammo_50bmg_bt`, `ammo_50bmg_exi`, `ammo_50bmg_jhp_ap`, `ammo_545x39`, `ammo_545x39_ap`, `ammo_545x39_fmj`, `ammo_556x45_ap`, `ammo_556x45_fmj`, `ammo_556x45_jhp`, `ammo_556x45_m855a1`, `ammo_57x28`, `ammo_57x28_ap`, `ammo_57x28_fmj`, `ammo_762x25_fmj`, `ammo_762x25_jhp`, `ammo_762x39_ap`, `ammo_762x39_fmj`, `ammo_762x39_jhp`, `ammo_762x51`, `ammo_762x51_ap`, `ammo_762x51_api`, `ammo_762x51_exi`, `ammo_762x51_fmj`, `ammo_762x51_jhp_ap`, `ammo_762x54r`, `ammo_762x54r_ap`, `ammo_762x54r_bt`, `ammo_765x21_fmj`, `ammo_765x21_jhp`, `ammo_9mm`, `ammo_9x19`, `ammo_9x19_ap`, `ammo_9x19_fmj`, `ammo_9x19_jhp`, `ammo_9x21_fmj`, `ammo_9x21_jhp`, `ammo_box`, `ammo_deprecated_`, `ammo_deprecated_12ga`, `ammo_deprecated_16ga`, `ammo_deprecated_300blk`, `ammo_deprecated_338lapua`, `ammo_deprecated_380acp`, `ammo_deprecated_408cheytac`, `ammo_deprecated_45acp`, `ammo_deprecated_46x30`, `ammo_deprecated_50bmg`, `ammo_deprecated_545x39`, `ammo_deprecated_556x45`, `ammo_deprecated_57x28`, `ammo_deprecated_762x25`, `ammo_deprecated_762x39`, `ammo_deprecated_762x51`, `ammo_deprecated_762x54r`, `ammo_deprecated_765x21`, `ammo_deprecated_9x21`, `ammo_deprecated_unknown`, `ammo_expended`, `ammo_pistol`, `ammo_rifle`, `ammo_shotgun`, `ammo_surrendered`, `item_ammo_ap`, `item_ammo_hp`, `item_ammo_standard`, `item_ammo_types` | 2,166,107 bytes |
| `067ce5a0bd...` | `ammo_deprecated_cal_545x39_v2`, `dirty_water_flask`, `sedative_vial`, `shelter_map_table` | 76,590 bytes |

## MEDIUM-rank groups (cross-extension, same stem)

Sample (first 30 by potential save):

| Group sha | Files | Save |
|---|---|---|
| `860590125f…` | `2 files` | 88,029 bytes |
| `06b59757ce…` | `2 files` | 87,500 bytes |
| `ef5c4422c5…` | `2 files` | 86,773 bytes |
| `ba01c6eacf…` | `2 files` | 84,928 bytes |
| `a6c28dd671…` | `2 files` | 82,840 bytes |
| `9cc9265cd3…` | `2 files` | 82,125 bytes |
| `011c7a8bc6…` | `2 files` | 82,014 bytes |
| `35df62605f…` | `2 files` | 81,076 bytes |
| `65e8199944…` | `2 files` | 81,001 bytes |
| `07e04a69e0…` | `2 files` | 79,798 bytes |
| `cdba6bc4cc…` | `2 files` | 79,781 bytes |
| `b9cf2da469…` | `2 files` | 79,475 bytes |
| `56e7e73dc8…` | `2 files` | 78,844 bytes |
| `4a855c62ce…` | `2 files` | 78,468 bytes |
| `717aa9836a…` | `2 files` | 78,165 bytes |
| `858a2ad3c5…` | `2 files` | 77,835 bytes |
| `471d81196d…` | `2 files` | 77,575 bytes |
| `86bf2ecc5f…` | `2 files` | 77,573 bytes |
| `997b52a702…` | `2 files` | 77,215 bytes |
| `eec7ad73c3…` | `2 files` | 77,194 bytes |
| `3e74a57041…` | `2 files` | 77,120 bytes |
| `7c47bd8662…` | `2 files` | 75,777 bytes |
| `94a4767191…` | `2 files` | 75,720 bytes |
| `e536d58481…` | `2 files` | 75,546 bytes |
| `0e4b8a4f14…` | `2 files` | 75,296 bytes |
| `ab38252809…` | `2 files` | 75,257 bytes |
| `eb5e12ffc1…` | `2 files` | 75,119 bytes |
| `344a20dfe3…` | `2 files` | 74,975 bytes |
| `941f948ae9…` | `2 files` | 74,719 bytes |
| `a4a59b1380…` | `2 files` | 74,283 bytes |
| `8cf8e46f9f…` | `2 files` | 74,174 bytes |
| `4de096a041…` | `2 files` | 74,140 bytes |
| `2545c35221…` | `2 files` | 74,134 bytes |
| `bd75f5396b…` | `2 files` | 73,749 bytes |
| `fd53f7ed18…` | `2 files` | 73,727 bytes |
| `3a24449e01…` | `2 files` | 73,668 bytes |
| `04cd96c0aa…` | `2 files` | 73,606 bytes |
| `a2a7e793ae…` | `2 files` | 73,546 bytes |
| `35908badf7…` | `2 files` | 73,438 bytes |
| `705603aa37…` | `2 files` | 73,420 bytes |
| `6b36618ae4…` | `2 files` | 73,409 bytes |
| `fad199e411…` | `2 files` | 73,296 bytes |
| `eae62b9880…` | `2 files` | 73,283 bytes |
| `d38c6cb681…` | `2 files` | 73,273 bytes |
| `455bbab223…` | `2 files` | 73,137 bytes |
| `1e12eeea9b…` | `2 files` | 72,520 bytes |
| `fbe83bbf5b…` | `2 files` | 72,424 bytes |
| `3d0d3705fe…` | `2 files` | 72,365 bytes |
| `854750296c…` | `2 files` | 72,243 bytes |
| `537d10b28a…` | `2 files` | 72,062 bytes |
| `32911b366b…` | `2 files` | 72,014 bytes |
| `8e831d41b7…` | `2 files` | 71,940 bytes |
| `5dbed227b9…` | `2 files` | 71,933 bytes |
| `c84eaa7c48…` | `2 files` | 71,919 bytes |
| `2d37bd6b3f…` | `2 files` | 71,840 bytes |
| `316cdc19f7…` | `2 files` | 71,657 bytes |
| `92718c11d3…` | `2 files` | 71,343 bytes |
| `a2ebe1a771…` | `2 files` | 71,338 bytes |
| `845cdb039e…` | `2 files` | 71,318 bytes |
| `9b65b5cc98…` | `2 files` | 71,052 bytes |
| `64474045f4…` | `2 files` | 70,909 bytes |
| `51921268f4…` | `2 files` | 70,882 bytes |
| `7b75e9b1f0…` | `2 files` | 70,848 bytes |
| `3b3d4220c6…` | `2 files` | 70,798 bytes |
| `f4240031e2…` | `2 files` | 70,735 bytes |
| `8698e32247…` | `2 files` | 70,707 bytes |
| `27b563f52c…` | `2 files` | 70,688 bytes |
| `766f988470…` | `2 files` | 70,276 bytes |
| `5d78523bc7…` | `2 files` | 70,219 bytes |
| `dfef535f10…` | `2 files` | 69,826 bytes |
| `a5e68910ae…` | `2 files` | 69,746 bytes |
| `556dd31704…` | `2 files` | 69,735 bytes |
| `ae70473245…` | `2 files` | 68,892 bytes |
| `09974402dc…` | `2 files` | 68,722 bytes |
| `dd72d9f016…` | `2 files` | 68,608 bytes |
| `5c4519ec7f…` | `2 files` | 68,035 bytes |
| `6250dd6cc3…` | `2 files` | 67,918 bytes |
| `b12549cc8a…` | `2 files` | 67,570 bytes |
| `ee05848be7…` | `2 files` | 67,331 bytes |
| `581f5f9759…` | `2 files` | 67,136 bytes |
| `f5947d36d3…` | `2 files` | 67,049 bytes |
| `e87bc8171c…` | `2 files` | 67,029 bytes |
| `d7145bec33…` | `2 files` | 66,961 bytes |
| `a4923b4daa…` | `2 files` | 66,864 bytes |
| `2be22c6560…` | `2 files` | 66,858 bytes |
| `ff769974c3…` | `2 files` | 66,820 bytes |
| `e74f576fe8…` | `2 files` | 66,702 bytes |
| `5c00a41823…` | `2 files` | 66,689 bytes |
| `fe38853480…` | `2 files` | 66,586 bytes |
| `b3559374bb…` | `2 files` | 66,493 bytes |
| `ee84b8cae2…` | `2 files` | 66,486 bytes |
| `aca54968cd…` | `2 files` | 66,461 bytes |
| `44d6a94e29…` | `2 files` | 66,448 bytes |
| `d5b973c0cc…` | `2 files` | 65,835 bytes |
| `735bf5486d…` | `2 files` | 65,760 bytes |
| `a4dec630e7…` | `2 files` | 65,740 bytes |
| `c9f26819e8…` | `2 files` | 65,662 bytes |
| `6c91e21570…` | `2 files` | 65,650 bytes |
| `e2d4f43351…` | `2 files` | 65,633 bytes |
| `61d45c3390…` | `2 files` | 65,495 bytes |
| `9e901a5b91…` | `2 files` | 65,470 bytes |
| `dc323059bb…` | `2 files` | 65,377 bytes |
| `3331872263…` | `2 files` | 65,244 bytes |
| `2b6bf10396…` | `2 files` | 65,209 bytes |
| `7506799247…` | `2 files` | 65,163 bytes |
| `1cb4eabd0d…` | `2 files` | 65,095 bytes |
| `a999aa6577…` | `2 files` | 64,976 bytes |
| `0eb9cf7e64…` | `2 files` | 64,917 bytes |
| `33d650d0e5…` | `2 files` | 64,744 bytes |
| `793154a11c…` | `2 files` | 64,736 bytes |
| `a44ee9ed5b…` | `2 files` | 64,638 bytes |
| `a839585591…` | `2 files` | 64,182 bytes |
| `4ff9657edf…` | `2 files` | 64,024 bytes |

(113 medium-rank groups)

## LOW-rank groups (multi-stem, manual review)

Sample (first 20):

| `fd00058511…` | `enc_a`, `enc_ambulance_gamble`, `enc_ash_quicksand`, `enc_b` | 1,730,190 bytes |
| `38575dade5…` | `5_factions`, `ammo_slug_shotgun_12ga`, `blood_splatter_impact`, `bunker_tooltip_text` | 1,148,670 bytes |
| `a549c60f00…` | `15_weather_kinds`, `ammo_incendiary_rounds_box`, `antenna_cut`, `bunker_rationing` | 1,132,736 bytes |
| `8f2f7c8b0e…` | `abandoned_supermarket`, `ammo_tranquilizer_dart_vial`, `bandage`, `bones` | 1,107,465 bytes |
| `cf9b82a0d4…` | `9mm_pistol`, `ammo_tracer_rounds_762`, `ash_desert`, `blood_type` | 1,093,260 bytes |
| `66a99fb081…` | `ammo_depleted_uranium_dart`, `august`, `bombed_out_church`, `bunker_fractured` | 1,079,820 bytes |
| `cd2fdd10c0…` | `air_filter`, `ammo_acid_tipped_arrow`, `bottled_water_irradiated`, `bunker_maintenance` | 1,076,376 bytes |
| `2a7afb0e3e…` | `ammo_emp_pulse_cartridge`, `bunker_map_root`, `child_died`, `corpse` | 1,069,444 bytes |
| `c639bef044…` | `42_locations`, `ammo_hollow_point_9mm`, `anti_rad_meds`, `black_rain_streaks` | 1,046,160 bytes |
| `e293a3bc15…` | `affliction_phase`, `ammo_harpoon_tether_spear`, `bunker_leader`, `contaminated_food` | 1,014,858 bytes |
| `d354ee239b…` | `188_items`, `biological_trade_item`, `bunker_social`, `chronic_illness_kind` | 979,754 bytes |
| `e3ad26ffed…` | `all_96_named_survivors`, `bunker_commander_portrait`, `deserter`, `deserter_at_hatch` | 799,200 bytes |
| `263ed04f57…` | `beast_blind_cave_stalker`, `beast_chitinous_beetle_armored`, `beast_giant_fallout_rat`, `beast_infected_bear_grizzly` | 245,970 bytes |
| `7e5bae137d…` | `botany_compost_soil_bag`, `botany_glowing_moss_cluster`, `botany_hydroponic_tomato_plant`, `botany_irradiated_corn_stalk` | 231,732 bytes |
| `4179ad327f…` | `lore_bunker_access_keycard`, `lore_burned_diary_page`, `lore_classified_military_document`, `lore_faded_family_photograph` | 225,027 bytes |
| `d7de3c459f…` | `journal_icon`, `ruin`, `shelter_icon`, `thirst_bar` | 149,035 bytes |
| `e9e5638ef6…` | `building_collapse_dust`, `equip_slot`, `health_bar`, `radiation_bar` | 148,015 bytes |
| `a21cbbdd7f…` | `character_icon`, `location_pin_icon`, `morale_bar`, `suburban_ruin` | 147,770 bytes |
| `aa0ce6eff6…` | `inventory_icon`, `map_icon`, `ui_event_map_rot`, `ui_frame_quest_tracker_panel` | 145,650 bytes |
| `5eb872af26…` | `duct_tape_roll`, `shelter_decontamination_shower`, `stun_baton_electric`, `sugar_cube_box` | 131,705 bytes |

(20 shown; total 67 low-rank groups)

## Recommended deletion policy (NOT executed this phase)

Before any deletion:
1. Confirm canonical survivor (The `ammo_<cal>_<type>.jpg` is canonical; the `ammo_deprecated_*` is mirror).
2. Trace all runtime references — `assets/art/*` is consumed by `src/Host/AssetRegistry.cs` via `ItemSearchPaths`. Identical content means duplicate lookup will return the first match deterministically.
3. Save compatibility is irrelevant: deprecated ammo entries are not stored in save state.
4. Tests pass — the existing `--asset-registry-selftest` continues to find at least one valid file for any deprecated id even after deletion.
5. Move phase — move deprecated to `assets/_legacy_compat/` (gitignore or quarantine dir) before destructive deletion.

Phase 14T only documents. No destructive cleanup is performed.
