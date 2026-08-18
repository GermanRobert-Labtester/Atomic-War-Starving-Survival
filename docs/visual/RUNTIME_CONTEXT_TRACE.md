# Runtime-context wiring trace


Phase 15 — panel-level reachability of AssetRegistry from runtime code.


`src/**/*.cs` calls AssetRegistry through the four `Get*` entry points. We report each *panel/host sub-tree* that reaches AssetRegistry, with the methods called. Catalog content IDs flow through variable references (e.g. `AssetRegistry.GetItem(good.id)`), so per-ID inference would need a C# symbol-resolver; we instead report panel-level coverage.


## Method coverage


| Method | Files |
|---|---|

| `GetItem` | 10 |

| `GetPortrait` | 4 |

| `GetLocation` | 4 |

| `GetFaction` | 1 |



## Panel-layer coverage


Each row groups the references by the first subdirectory under src/. A panel that appears here means that, at runtime, that surface calls AssetRegistry and therefore needs resolved art for any item/portrait/location it queries.


| Panel dir | Calls observed |
|---|---|

| `Economy` | 2 |

| `Host` | 4 |

| `Main.cs` | 1 |

| `UI` | 1 |



## Per-file reference detail


| File | Methods |
|---|---|

| `src/Economy/EconomyMarketPanel.cs` | GetItem |

| `src/Economy/TradeScreenGodotPanel.cs` | GetItem |

| `src/Host/AssetRegistry.cs` | GetFaction, GetItem, GetLocation, GetPortrait |

| `src/Main.cs` | GetItem |

| `src/UI/AshfallUiHelpers.cs` | GetItem |



## Runtime-context recommendations for Batch 1


Batch 1 must hit content IDs that flow into the panels above. Prioritize Inventory-Item rows that feed `GetItem` callers (the most frequent entry-point). Survivor-Portrait rows feed `GetPortrait` callers (2 src hits). Location-Art feeds `GetLocation` callers (currently 1 in the AssetRegistrySelfTest; nothing in the live panels yet).


Phase 14's production manifest already weights the most-impacted catalog rows higher, so Batch 1 ranks are well-aligned with this trace.

## Phase 17 — Per-content_id coverage

Taken from the top 50 ids of each master catalog — the slice the AssetRegistrySelfTest probes at runtime. Rows already in the manifest are *runtime-surfaced*; rows not in the manifest are *already resolved* (art exists).

| Category | Catalog total | Manifest actionable | Top-N in manifest | Top-N NOT in manifest |
|---|---|---|---|---|
| items | 499 | 233 | 0 | 50 |
| survivors | 102 | 2 | 0 | 50 |
| locations | 105 | 200 | 3 | 47 |
| characters | 36 | 36 | 36 | 14 |

Manifest actionable rows total: **478**
Surfaced by top-N: **39** (8.2%)
Not surfaced: **439** (91.8%)

### Surfaced by priority band

| Band | Surfaced | Not surfaced |
|---|---|---|
| P0 | 0 | 0 |
| P1 | 38 | 125 |
| P2 | 1 | 198 |
| P3 | 0 | 110 |
| P4 | 0 | 6 |

### Not-surfaced IDs by priority band (drives Batch 1 strategy)

**P1** (125 rows):

- `survivor_family_adult` (Survivor-Portrait/Other, importance 5.4)
- `survivor_family_child` (Survivor-Portrait/Other, importance 5.4)
- `loc_grain_silo` (Location-Art/Food-Water, importance 5.25)
- `loc_ration_queue_plaza` (Location-Art/Food-Water, importance 5.25)
- `iodine_tablets` (Inventory-Item/Medical, importance 4.5)
- `clean_water_jug` (Inventory-Item/Food-Water, importance 4.2)
- `dried_rations` (Inventory-Item/Food-Water, importance 4.2)
- `grain_exchange_scale_weight` (Inventory-Item/Food-Water, importance 4.2)
- `military_rations` (Inventory-Item/Food-Water, importance 4.2)
- `mira_chalk_ration_token` (Inventory-Item/Food-Water, importance 4.2)
- `ration_plaza_paint_stick` (Inventory-Item/Food-Water, importance 4.2)
- `spirits` (Inventory-Item/Food-Water, importance 4.2)
- `water_purification_tablets` (Inventory-Item/Food-Water, importance 4.2)
- `water_sample_contaminated` (Inventory-Item/Food-Water, importance 4.2)
- `checkpoint_kilo_armory` (Location-Art/Other, importance 3.75)
- `collapsed_building` (Location-Art/Other, importance 3.75)
- `concert_hall_ruins` (Location-Art/Other, importance 3.75)
- `convoy_echo7_cache` (Location-Art/Other, importance 3.75)
- `electrical_substation` (Location-Art/Other, importance 3.75)
- `family_bunker_backyard_shed` (Location-Art/Other, importance 3.75)
- ... and 105 more

**P2** (198 rows):

- `loc_deep_salt_hospital_sanctuary` (Location-Art/Food-Water, importance 2.45)
- `loc_salt_cavern_medical_depot` (Location-Art/Food-Water, importance 2.45)
- `loc_salt_miners_barter_hall` (Location-Art/Food-Water, importance 2.45)
- `loc_second_winter_homestead` (Location-Art/Food-Water, importance 2.45)
- `loc_shelled_grain_elevator_ruin` (Location-Art/Food-Water, importance 2.45)
- `loc_water_treatment_plant` (Location-Art/Food-Water, importance 2.45)
- `item_iodine_crystal` (Inventory-Item/Medical, importance 2.4)
- `loc_salt_cavern_explosives_magazine` (Location-Art/Ammunition, importance 2.275)
- `item_alloc7_ration_tin` (Inventory-Item/Food-Water, importance 2.24)
- `item_electrolyte_salts` (Inventory-Item/Food-Water, importance 2.24)
- `item_salt_rash_salve` (Inventory-Item/Food-Water, importance 2.24)
- `item_steam_token` (Inventory-Item/Food-Water, importance 2.24)
- `barrow_fennicks_ledger_page` (Inventory-Item/Special-Resource, importance 2.1)
- `item_antibiotic_saline_infusion` (Inventory-Item/Medical, importance 2.1)
- `item_prussian_blue_chelating_pellets` (Inventory-Item/Medical, importance 2.1)
- `item_sski_iodine_bulk_canister` (Inventory-Item/Medical, importance 2.1)
- `loc_ammonium_nitrate_fertilizer_shed` (Location-Art/Crafting-Material, importance 2.1)
- `loc_ash_woodland` (Location-Art/Crafting-Material, importance 2.1)
- `loc_iron_raiders_den` (Location-Art/Crafting-Material, importance 2.1)
- `music_box_comb` (Inventory-Item/Special-Resource, importance 2.1)
- ... and 178 more

**P3** (110 rows):

- `item_dosimeter_tag` (Inventory-Item/Equipment, importance 1.44)
- `item_foundry_grey_iron_ingot` (Inventory-Item/Crafting-Material, importance 1.44)
- `item_lead_glass_pane` (Inventory-Item/Crafting-Material, importance 1.44)
- `paper_scrap` (Inventory-Item/Crafting-Material, importance 1.44)
- `item_air_filter_heavy` (Inventory-Item/Other, importance 1.4)
- `item_amnesty_petition_dossier` (Inventory-Item/Other, importance 1.4)
- `item_artillery_fuze_wrench` (Inventory-Item/Other, importance 1.4)
- `item_boron_shielding_tile` (Inventory-Item/Other, importance 1.4)
- `item_brass_stamping_die` (Inventory-Item/Other, importance 1.4)
- `item_brass_valve_fitting` (Inventory-Item/Other, importance 1.4)
- `item_calibrated_mass_spectrometer_tube` (Inventory-Item/Other, importance 1.4)
- `item_ceramic_heating_element` (Inventory-Item/Other, importance 1.4)
- `item_charter_three_pages` (Inventory-Item/Other, importance 1.4)
- `item_cold_count_provenance_seal` (Inventory-Item/Other, importance 1.4)
- `item_continental_maritime_transponder` (Inventory-Item/Other, importance 1.4)
- `item_corrosion_inhibitor_drum` (Inventory-Item/Other, importance 1.4)
- `item_crossing_pledge_slip` (Inventory-Item/Other, importance 1.4)
- `item_cryo_flask_rhizome` (Inventory-Item/Other, importance 1.4)
- `item_cyanide_antidote_kit` (Inventory-Item/Other, importance 1.4)
- `item_debt_contract_copy` (Inventory-Item/Other, importance 1.4)
- ... and 90 more

**P4** (6 rows):

- `item_duty_log_fragment` (Inventory-Item/Special-Resource, importance 0.98)
- `item_hydro_baron_queue_chit` (Inventory-Item/Special-Resource, importance 0.98)
- `item_long_walk_route_ledger` (Inventory-Item/Special-Resource, importance 0.98)
- `item_telegraph_sounder_relay` (Inventory-Item/Special-Resource, importance 0.98)
- `item_unsigned_debt_ledger_page` (Inventory-Item/Special-Resource, importance 0.98)
- `item_dose_ledger` (Inventory-Item/Special-Resource, importance 0.84)

