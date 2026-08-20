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
| items | 499 | 30 | 0 | 50 |
| survivors | 102 | 35 | 0 | 50 |
| locations | 105 | 197 | 0 | 50 |
| characters | 36 | 31 | 31 | 19 |

Manifest actionable rows total: **300**
Surfaced by top-N: **31** (10.3%)
Not surfaced: **269** (89.7%)

### Surfaced by priority band

| Band | Surfaced | Not surfaced |
|---|---|---|
| P0 | 0 | 0 |
| P1 | 30 | 99 |
| P2 | 1 | 137 |
| P3 | 0 | 29 |
| P4 | 0 | 4 |

### Not-surfaced IDs by priority band (drives Batch 1 strategy)

**P1** (99 rows):

- `survivor_family_child` (Survivor-Portrait/Other, importance 5.4)
- `loc_grain_silo` (Location-Art/Food-Water, importance 5.25)
- `loc_ration_queue_plaza` (Location-Art/Food-Water, importance 5.25)
- `survivor_anton_salt_trader` (Survivor-Portrait/Food-Water, importance 4.2)
- `survivor_gregor_salt_miner` (Survivor-Portrait/Food-Water, importance 4.2)
- `checkpoint_kilo_armory` (Location-Art/Other, importance 3.75)
- `collapsed_building` (Location-Art/Other, importance 3.75)
- `concert_hall_ruins` (Location-Art/Other, importance 3.75)
- `convoy_echo7_cache` (Location-Art/Other, importance 3.75)
- `electrical_substation` (Location-Art/Other, importance 3.75)
- `family_bunker_backyard_shed` (Location-Art/Other, importance 3.75)
- `hospital_pharmacy` (Location-Art/Other, importance 3.75)
- `loc_alloc_12b` (Location-Art/Other, importance 3.75)
- `loc_ash_sign_shrine` (Location-Art/Other, importance 3.75)
- `loc_avalanche_gallery` (Location-Art/Other, importance 3.75)
- `loc_bathymetric_boat` (Location-Art/Other, importance 3.75)
- `loc_bridge_seven` (Location-Art/Other, importance 3.75)
- `loc_bus_reversal_loop` (Location-Art/Other, importance 3.75)
- `loc_cider_press` (Location-Art/Other, importance 3.75)
- `loc_cold_store_atlantic` (Location-Art/Other, importance 3.75)
- ... and 79 more

**P2** (137 rows):

- `loc_deep_salt_hospital_sanctuary` (Location-Art/Food-Water, importance 2.45)
- `loc_salt_cavern_medical_depot` (Location-Art/Food-Water, importance 2.45)
- `loc_salt_miners_barter_hall` (Location-Art/Food-Water, importance 2.45)
- `loc_second_winter_homestead` (Location-Art/Food-Water, importance 2.45)
- `loc_shelled_grain_elevator_ruin` (Location-Art/Food-Water, importance 2.45)
- `loc_water_treatment_plant` (Location-Art/Food-Water, importance 2.45)
- `loc_salt_cavern_explosives_magazine` (Location-Art/Ammunition, importance 2.275)
- `loc_ammonium_nitrate_fertilizer_shed` (Location-Art/Crafting-Material, importance 2.1)
- `loc_ash_woodland` (Location-Art/Crafting-Material, importance 2.1)
- `loc_iron_raiders_den` (Location-Art/Crafting-Material, importance 2.1)
- `faction_the_compact` (Faction-Art/Other, importance 2.0)
- `faction_the_cutters` (Faction-Art/Other, importance 2.0)
- `faction_the_fleet` (Faction-Art/Other, importance 2.0)
- `faction_the_office` (Faction-Art/Other, importance 2.0)
- `faction_the_overlay` (Faction-Art/Other, importance 2.0)
- `faction_the_scale` (Faction-Art/Other, importance 2.0)
- `faction_the_underwrite` (Faction-Art/Other, importance 2.0)
- `loc_cluster_block_c` (Location-Art/Other, importance 2.0)
- `loc_cluster_clinic` (Location-Art/Other, importance 2.0)
- `loc_cluster_gatehouse` (Location-Art/Other, importance 2.0)
- ... and 117 more

**P3** (29 rows):

- `item_amnesty_petition_dossier` (Inventory-Item/Other, importance 1.4)
- `item_artillery_fuze_wrench` (Inventory-Item/Other, importance 1.4)
- `item_brass_stamping_die` (Inventory-Item/Other, importance 1.4)
- `item_cold_count_provenance_seal` (Inventory-Item/Other, importance 1.4)
- `item_corrosion_inhibitor_drum` (Inventory-Item/Other, importance 1.4)
- `item_cyanide_antidote_kit` (Inventory-Item/Other, importance 1.4)
- `item_deserter_coalition_forged_papers` (Inventory-Item/Other, importance 1.4)
- `item_garrison_manifest_forgery_kit` (Inventory-Item/Other, importance 1.4)
- `item_icebreaker_rendezvous_flare_rocket` (Inventory-Item/Other, importance 1.4)
- `item_mercury_barometer_station` (Inventory-Item/Other, importance 1.4)
- `item_paraffin_wax_neutron_shield` (Inventory-Item/Other, importance 1.4)
- `item_periscope_optics_prism` (Inventory-Item/Other, importance 1.4)
- `item_potassium_permanganate_crystals` (Inventory-Item/Other, importance 1.4)
- `item_prewar_diagnostic_scanner` (Inventory-Item/Other, importance 1.4)
- `item_railroad_hydraulic_spike_puller` (Inventory-Item/Other, importance 1.4)
- `item_scavenger_guild_claim_marker` (Inventory-Item/Other, importance 1.4)
- `item_tungsten_carbide_drill_bit` (Inventory-Item/Other, importance 1.4)
- `item_zinc_bromide_shielding_window` (Inventory-Item/Other, importance 1.4)
- `location_automated_abattoir` (Location-Art/Other, importance 1.25)
- `location_central_postal_hub` (Location-Art/Other, importance 1.25)
- ... and 9 more

**P4** (4 rows):

- `item_hydro_baron_queue_chit` (Inventory-Item/Special-Resource, importance 0.98)
- `item_long_walk_route_ledger` (Inventory-Item/Special-Resource, importance 0.98)
- `item_telegraph_sounder_relay` (Inventory-Item/Special-Resource, importance 0.98)
- `item_unsigned_debt_ledger_page` (Inventory-Item/Special-Resource, importance 0.98)

