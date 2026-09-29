# ASHFALL Visual Production Report — 2026-09-29 (Twenty-Three Tranches)

Sections 1–20 record tranches 1–8. The tranche 9–23 addenda at the end
record the latest 170 assets and supersede their cumulative counts.

## 1. Git SHA

Baseline `b31915ea2`; uncommitted visual tranche.

## 2. Tools Detected

| Tool | Availability | Used for |
|---|---|---|
| ChatGPT image generation | Available; model version not exposed | 217 item images across twenty-three tranches |
| ImageMagick | Available | 512 px JPEG processing and 64/26 px visual checks |
| Godot 4.7.1 mono | Available | Headless import |

## 3. Initial Asset Coverage

Read-only inventory found 2,366 non-placeholder `assets/art` visuals, 641 `assets/sprites` visuals, and 338 `assets/ui` visuals. Another 1,079 `assets/art/placeholders-512` PNGs are explicitly labeled placeholders and are outside `AssetRegistry` search paths. Static candidate-path comparison found art for all 265 portrait IDs and 372 location IDs; roughly 411 of 967 item IDs had direct or prefix candidates before this tranche. These are file-path counts, not a Godot load audit, and item aliases are excluded.

## 4. Missing Assets Found

`item_calibrated_dosimeter`, `item_seed_frost_pea`, and `item_foundry_roof_armor_plate` had no direct art in tranche 1. Tranche 2 confirmed four more gaps: `item_radiation_survey_meter`, `item_chelation_decorporation_course`, `crop_frost_pea`, and `item_foundry_shoring_bracket`. Tranche 3 confirmed five more: `item_potassium_iodide_pack`, `item_shielding_apron`, `crop_glacier_greens`, `item_greenhouse_drip_kit`, and `item_foundry_blast_fitting`. Tranche 4 confirmed five more: `item_shielded_badge_case`, `item_greenhouse_watering_can`, `item_greenhouse_pruning_shears`, `item_foundry_crucible_spare`, and `item_foundry_replacement_die`. Tranche 5 confirmed `item_sealed_dive_lamp`, `item_rebreather_canister`, `item_icebreaker_rendezvous_flare_rocket`, `item_seed_glacier_greens`, and `item_hot_dust_drum`. Tranche 6 confirmed ten more foundry outputs: `item_foundry_press_fitting`, `item_foundry_bearing_housing`, `item_foundry_furnace_grate`, `item_foundry_reinforcement_shoe`, `item_foundry_structural_coupling`, `item_foundry_drill_blanks`, `item_hardened_ground_anchor_spikes`, `item_superalloy_turbine_blade_blank`, `item_rail_grinding_head`, and `item_press_tooling_set`. Tranche 7 confirmed nine Black Flotilla objects and `item_signal_lamp_module`. Tranche 8 confirmed five radiation and CBRN objects: `item_forged_clean_bill_chit`, `item_radiation_shielding_panel`, `item_gas_mask_improved`, `item_dosimeter_calibrated`, and `item_cbrn_cartridge`. They are authored in the dose, greenhouse, foundry, Black Flotilla, Year of Ash, and base item catalogs.

## 5. Placeholders Found

The 1,079 explicitly labeled art placeholders remain a separate backlog. No placeholder was overwritten in this tranche.

## 6. Existing Unwired Assets

No new unwired asset was added. The static inventory did not prove whether every existing visual has a runtime consumer.

## 7. Assets Reused

Existing item art, including `item_dosimeter_pen.jpg`, informed the dark-background object treatment. Existing location art was retained.

## 8. Assets Edited

None.

## 9. Assets Generated

| Catalog ID | File | Visual |
|---|---|---|
| `item_calibrated_dosimeter` | `assets/art/item_calibrated_dosimeter.jpg` | Worn quartz electroscope dosimeter |
| `item_seed_frost_pea` | `assets/art/item_seed_frost_pea.jpg` | Six dark peas in opened waxed paper |
| `item_foundry_roof_armor_plate` | `assets/art/item_foundry_roof_armor_plate.jpg` | Layered, bolted steel roof plate |
| `item_radiation_survey_meter` | `assets/art/item_radiation_survey_meter.jpg` | Portable analog radiation meter |
| `item_chelation_decorporation_course` | `assets/art/item_chelation_decorporation_course.jpg` | Two-ampoule treatment kit |
| `crop_frost_pea` | `assets/art/crop_frost_pea.jpg` | Harvested slate-green pea pods |
| `item_foundry_shoring_bracket` | `assets/art/item_foundry_shoring_bracket.jpg` | Cast iron U-shaped support collar |
| `item_potassium_iodide_pack` | `assets/art/item_potassium_iodide_pack.jpg` | Sealed tablet blister pack |
| `item_shielding_apron` | `assets/art/item_shielding_apron.jpg` | Heavy vinyl-clad protective apron |
| `crop_glacier_greens` | `assets/art/crop_glacier_greens.jpg` | Bundled pale greenhouse leaves |
| `item_greenhouse_drip_kit` | `assets/art/item_greenhouse_drip_kit.jpg` | Coiled micro-tubing and emitter kit |
| `item_foundry_blast_fitting` | `assets/art/item_foundry_blast_fitting.jpg` | Blast-door hinge barrel and bracket |
| `item_shielded_badge_case` | `assets/art/item_shielded_badge_case.jpg` | Lead-lined badge case |
| `item_greenhouse_watering_can` | `assets/art/item_greenhouse_watering_can.jpg` | Seam-soldered can with brass rose |
| `item_greenhouse_pruning_shears` | `assets/art/item_greenhouse_pruning_shears.jpg` | Short bypass shears with latch |
| `item_foundry_crucible_spare` | `assets/art/item_foundry_crucible_spare.jpg` | Refractory-lined crucible shell |
| `item_foundry_replacement_die` | `assets/art/item_foundry_replacement_die.jpg` | Paired steel forming dies |
| `item_sealed_dive_lamp` | `assets/art/item_sealed_dive_lamp.jpg` | Brass pressure-cased oil lamp |
| `item_rebreather_canister` | `assets/art/item_rebreather_canister.jpg` | Wax-sealed scrubber cartridge |
| `item_icebreaker_rendezvous_flare_rocket` | `assets/art/item_icebreaker_rendezvous_flare_rocket.jpg` | Red parachute signal rocket |
| `item_seed_glacier_greens` | `assets/art/item_seed_glacier_greens.jpg` | Pale rooted greenhouse cutting |
| `item_hot_dust_drum` | `assets/art/item_hot_dust_drum.jpg` | Welded electrostatic waste drum |
| `item_foundry_press_fitting` | `assets/art/item_foundry_press_fitting.jpg` | Machined hydraulic collar |
| `item_foundry_bearing_housing` | `assets/art/item_foundry_bearing_housing.jpg` | Split pillow-block housing |
| `item_foundry_furnace_grate` | `assets/art/item_foundry_furnace_grate.jpg` | Cast-iron firebox grate segment |
| `item_foundry_reinforcement_shoe` | `assets/art/item_foundry_reinforcement_shoe.jpg` | Anchored column support shoe |
| `item_foundry_structural_coupling` | `assets/art/item_foundry_structural_coupling.jpg` | Flanged conduit coupling sleeve |
| `item_foundry_drill_blanks` | `assets/art/item_foundry_drill_blanks.jpg` | Bundled hexagonal tool-steel blanks |
| `item_hardened_ground_anchor_spikes` | `assets/art/item_hardened_ground_anchor_spikes.jpg` | Tungsten-tipped anchor spikes |
| `item_superalloy_turbine_blade_blank` | `assets/art/item_superalloy_turbine_blade_blank.jpg` | Uncoated turbine blade casting |
| `item_rail_grinding_head` | `assets/art/item_rail_grinding_head.jpg` | Abrasive modular grinding assembly |
| `item_press_tooling_set` | `assets/art/item_press_tooling_set.jpg` | Tablet press die and punches |
| `item_descent_line` | `assets/art/item_descent_line.jpg` | Tarred figure-eight dive line with lead weight |
| `item_salvage_cutting_tool` | `assets/art/item_salvage_cutting_tool.jpg` | Wrapped cold-chisel pry bar |
| `item_deep_service_ribbon` | `assets/art/item_deep_service_ribbon.jpg` | Black ribbon with brass thread |
| `item_claim_tag_stamped` | `assets/art/item_claim_tag_stamped.jpg` | Punched lead salvage claim tag |
| `item_sea_ration` | `assets/art/item_sea_ration.jpg` | Waxed kelp and fish ration brick |
| `item_brine_protein_tin` | `assets/art/item_brine_protein_tin.jpg` | Soldered tin of brined fish |
| `item_marine_sealant_kit` | `assets/art/item_marine_sealant_kit.jpg` | Canvas caulking kit with pitch and oakum |
| `item_ships_bell_picket` | `assets/art/item_ships_bell_picket.jpg` | Patinated bronze ship's bell |
| `item_fleet_log_cylinder` | `assets/art/item_fleet_log_cylinder.jpg` | Waterproof brass document tube |
| `item_signal_lamp_module` | `assets/art/item_signal_lamp_module.jpg` | Shielded amber rail signal module |
| `item_forged_clean_bill_chit` | `assets/art/item_forged_clean_bill_chit.jpg` | Stamped clearance slip with red-pencil mark |
| `item_radiation_shielding_panel` | `assets/art/item_radiation_shielding_panel.jpg` | Layered borated and lead-foil wall panel |
| `item_gas_mask_improved` | `assets/art/item_gas_mask_improved.jpg` | Refit respirator with secondary filter sleeve |
| `item_dosimeter_calibrated` | `assets/art/item_dosimeter_calibrated.jpg` | Digital solid-state dose meter |
| `item_cbrn_cartridge` | `assets/art/item_cbrn_cartridge.jpg` | Replaceable high-grade respirator cartridge |

Each final is an opaque 512×512 JPEG with a Godot `.import` sidecar. Generated source PNGs remain in the ChatGPT image tool's output store; the workspace JPEGs are the game assets.

## 10. Generation Provider Breakdown

Forty-seven final images came from the available ChatGPT image tool. Its callable interface does not expose or confirm a `2.5` model designation. Three location concepts were also generated during the initial audit, then excluded after discovering that their unprefixed catalog IDs already resolve to existing art; no duplicate location file was added.

## 11. UI Redesigned

None.

## 12. Assets Wired

The filenames equal their item catalog IDs. `AssetRegistry.GetItem` searches `assets/art/{id}.jpg`; `AshfallUiHelpers.MakeItemIcon` uses that registry path, and `InventoryPanel` passes `slot.Item.id` to the helper. No code change was needed.

## 13. Godot Scenes Modified

None.

## 14. Import Issues

None observed. `godot --headless --path . --import` exited 0 after each tranche and reimported all forty-seven new files without errors.

## 15. Visual QA Findings

All forty-seven images match the authored item descriptions and remain distinct in review strips. Tranches 3–8 were inspected at the inventory's 26 px icon size. A specific in-game inventory screenshot was not captured.

## 16. Iterations Performed

One final candidate per item across eight tranches; each converted to 512 px JPEG and inspected together at icon scale.

## 17. Final Coverage

Forty-seven previously absent direct item paths now exist and import. The static direct/prefix candidate count is roughly 458 of 967 item IDs; alias resolution and runtime display of these exact items were not separately measured.

## 18. Remaining Visual Gaps

Roughly 509 item IDs still lack direct or prefix art candidates under this static method. The manifest `docs/visual/PRODUCTION_ART_GENERATION_MANIFEST.json` is stale: entries marked `PENDING`, including `loc_grain_silo` and `survivor_family_child`, already have usable direct files.

## 19. Blocked Tool Integrations

The requested `ChatGPT image 2.5` version cannot be confirmed or selected through the available image tool interface.

## 20. Recommended Next Visual Work

Reconcile the stale manifest against current registry candidate paths, then select another small set of missing item IDs with confirmed inventory consumers. Verify each candidate's direct and alias paths before generation.

## Tranche 9 — ten base-catalog inventory assets

All ten IDs are authored in `Assets/StreamingAssets/Data/items.json` and
had no direct art or item sprite in the current `AssetRegistry` search paths.
The generated 512×512 opaque JPEGs follow the existing charcoal-backed item
style. Their matching `.jpg.import` sidecars were created by Godot.

| Catalog ID | New runtime file |
|---|---|
| `item_microfluidic_reader` | `assets/art/item_microfluidic_reader.jpg` |
| `item_biofilter_media` | `assets/art/item_biofilter_media.jpg` |
| `item_aquaponic_fish` | `assets/art/item_aquaponic_fish.jpg` |
| `item_ballistics_cleaning_kit` | `assets/art/item_ballistics_cleaning_kit.jpg` |
| `item_geothermal_descaling_kit` | `assets/art/item_geothermal_descaling_kit.jpg` |
| `item_hydraulic_wire_cutter` | `assets/art/item_hydraulic_wire_cutter.jpg` |
| `item_expedition_winch_kit` | `assets/art/item_expedition_winch_kit.jpg` |
| `item_groundwater_sensor` | `assets/art/item_groundwater_sensor.jpg` |
| `item_well_maintenance_kit` | `assets/art/item_well_maintenance_kit.jpg` |
| `item_switch_stand_module` | `assets/art/item_switch_stand_module.jpg` |

ImageMagick metadata confirms ten 512×512 JPEGs. The ten-image contact sheets
were inspected at 64 px and at the inventory's 26 px icon size. `godot
--headless --path . --import` exited 0 and imported all ten. Direct filename
resolution through `AssetRegistry.GetItem` makes them available to the current
inventory presentation path without code changes. A live inventory screenshot
of these exact items was not captured.

Cumulative direct item art added in these nine tranches: 57. Under the prior
static candidate-path method, roughly 468 of 967 item IDs now have direct or
prefix art candidates and roughly 499 remain. These estimates exclude aliases.

## Tranche 10 — ten Year of Ash inventory assets

All ten IDs are authored in `Assets/StreamingAssets/Data/year_of_ash_items.json`
and had no direct art or item sprite in the current `AssetRegistry` search
paths. The 512×512 opaque JPEGs continue the charcoal-backed item style.
Godot created a matching `.jpg.import` sidecar for each asset.

| Catalog ID | New runtime file |
|---|---|
| `item_corrosion_inhibitor_drum` | `assets/art/item_corrosion_inhibitor_drum.jpg` |
| `item_artillery_fuze_wrench` | `assets/art/item_artillery_fuze_wrench.jpg` |
| `item_brass_stamping_die` | `assets/art/item_brass_stamping_die.jpg` |
| `item_railroad_hydraulic_spike_puller` | `assets/art/item_railroad_hydraulic_spike_puller.jpg` |
| `item_telegraph_sounder_relay` | `assets/art/item_telegraph_sounder_relay.jpg` |
| `item_periscope_optics_prism` | `assets/art/item_periscope_optics_prism.jpg` |
| `item_cyanide_antidote_kit` | `assets/art/item_cyanide_antidote_kit.jpg` |
| `item_mercury_barometer_station` | `assets/art/item_mercury_barometer_station.jpg` |
| `item_tungsten_carbide_drill_bit` | `assets/art/item_tungsten_carbide_drill_bit.jpg` |
| `item_paraffin_wax_neutron_shield` | `assets/art/item_paraffin_wax_neutron_shield.jpg` |

ImageMagick metadata confirms ten 512×512 JPEGs. The ten-image contact sheets
were inspected at 64 px and at the inventory's 26 px icon size. `godot
--headless --path . --import` exited 0 and imported all ten. Their direct
filenames make them available through `AssetRegistry.GetItem` and the current
inventory presentation path without code changes. A live inventory screenshot
of these exact items was not captured.

Cumulative direct item art added in these ten tranches: 67. Under the prior
static candidate-path method, roughly 478 of 967 item IDs now have direct or
prefix art candidates and roughly 489 remain. These estimates exclude aliases.

## Tranche 11 — ten Crossing inventory assets

All ten IDs are authored in `Assets/StreamingAssets/Data/crossing_items.json`
and had no direct art or item sprite in the current `AssetRegistry` search
paths. The 512×512 opaque JPEGs continue the charcoal-backed item style.
Godot created a matching `.jpg.import` sidecar for each asset.

| Catalog ID | New runtime file |
|---|---|
| `item_arbitration_token` | `assets/art/item_arbitration_token.jpg` |
| `item_charter_stamp` | `assets/art/item_charter_stamp.jpg` |
| `item_weighbridge_chit` | `assets/art/item_weighbridge_chit.jpg` |
| `item_smuggled_medicine` | `assets/art/item_smuggled_medicine.jpg` |
| `item_crossing_bread` | `assets/art/item_crossing_bread.jpg` |
| `item_lamp_oil_crossing` | `assets/art/item_lamp_oil_crossing.jpg` |
| `item_filtered_water_crossing` | `assets/art/item_filtered_water_crossing.jpg` |
| `item_quarantine_bands` | `assets/art/item_quarantine_bands.jpg` |
| `item_granary_receipt` | `assets/art/item_granary_receipt.jpg` |
| `item_smugglers_ledger` | `assets/art/item_smugglers_ledger.jpg` |

ImageMagick metadata confirms ten 512×512 JPEGs. The ten-image contact sheets
were inspected at 64 px and at the inventory's 26 px icon size. `godot
--headless --path . --import` exited 0 and imported all ten. Their direct
filenames make them available through `AssetRegistry.GetItem` and the current
inventory presentation path without code changes. A live inventory screenshot
of these exact items was not captured.

Cumulative direct item art added in these eleven tranches: 77. Under the prior
static candidate-path method, roughly 488 of 967 item IDs now have direct or
prefix art candidates and roughly 479 remain. These estimates exclude aliases.

## Tranche 12 — ten Year of Ash inventory assets

These ten IDs were the next selected from sixteen remaining without a direct
art or item-sprite path in `year_of_ash_items.json`. An earlier candidate set
was rejected because those ten already had art. The 512×512 opaque JPEGs
continue the charcoal-backed item style. Godot created a matching
`.jpg.import` sidecar for each asset.

| Catalog ID | New runtime file |
|---|---|
| `item_zinc_bromide_shielding_window` | `assets/art/item_zinc_bromide_shielding_window.jpg` |
| `item_potassium_permanganate_crystals` | `assets/art/item_potassium_permanganate_crystals.jpg` |
| `item_hydro_baron_queue_chit` | `assets/art/item_hydro_baron_queue_chit.jpg` |
| `item_prewar_diagnostic_scanner` | `assets/art/item_prewar_diagnostic_scanner.jpg` |
| `item_scavenger_guild_claim_marker` | `assets/art/item_scavenger_guild_claim_marker.jpg` |
| `item_garrison_manifest_forgery_kit` | `assets/art/item_garrison_manifest_forgery_kit.jpg` |
| `item_seed_packet_nonhybrid` | `assets/art/item_seed_packet_nonhybrid.jpg` |
| `item_military_stimulants` | `assets/art/item_military_stimulants.jpg` |
| `item_meridian_archive_copy` | `assets/art/item_meridian_archive_copy.jpg` |
| `item_vitamin_supplements` | `assets/art/item_vitamin_supplements.jpg` |

ImageMagick metadata confirms ten 512×512 JPEGs. The ten-image contact sheets
were inspected at 64 px and the inventory's 26 px icon size. `godot
--headless --path . --import` exited 0 and imported all ten. Their direct
filenames make them available through `AssetRegistry.GetItem` and the current
inventory presentation path without code changes. A live inventory screenshot
of these exact items was not captured.

Cumulative direct item art added in these twelve tranches: 87. Under the prior
static candidate-path method, roughly 498 of 967 item IDs now have direct or
prefix art candidates and roughly 469 remain. These estimates exclude aliases.

## Tranche 13 — ten solar and optical workshop assets

All ten IDs are authored in `Assets/StreamingAssets/Data/items.json` and had
no direct art or item-sprite path in the current `AssetRegistry` search roots.
The 512×512 opaque JPEGs continue the charcoal-backed item style. Godot
created a matching `.jpg.import` sidecar for each asset.

| Catalog ID | New runtime file |
|---|---|
| `item_solar_inverter` | `assets/art/item_solar_inverter.jpg` |
| `item_parabolic_aluminum_dish_segment` | `assets/art/item_parabolic_aluminum_dish_segment.jpg` |
| `item_dual_axis_tracking_gimbal` | `assets/art/item_dual_axis_tracking_gimbal.jpg` |
| `item_focal_stirling_engine_generator` | `assets/art/item_focal_stirling_engine_generator.jpg` |
| `item_cast_borosilicate_glass_blank` | `assets/art/item_cast_borosilicate_glass_blank.jpg` |
| `item_precision_rangefinder_achromat` | `assets/art/item_precision_rangefinder_achromat.jpg` |
| `item_cerium_oxide_polishing_rouge` | `assets/art/item_cerium_oxide_polishing_rouge.jpg` |
| `item_optical_pitch_lap` | `assets/art/item_optical_pitch_lap.jpg` |
| `item_foucault_tester_rig` | `assets/art/item_foucault_tester_rig.jpg` |
| `item_laminated_ballistic_viewport_glass` | `assets/art/item_laminated_ballistic_viewport_glass.jpg` |

The gimbal's first candidate included a reflector dish, so it was replaced
with an isolated empty two-axis mount matching the catalog item. ImageMagick
metadata confirms ten 512×512 JPEGs; contact sheets were inspected at 64 px
and the inventory's 26 px icon size. `godot --headless --path . --import`
exited 0 and imported all ten. Direct filenames make them available through
`AssetRegistry.GetItem` without code changes. A live inventory screenshot of
these exact items was not captured.

Cumulative direct item art added in these thirteen tranches: 97. Under the
prior static candidate-path method, roughly 508 of 967 item IDs now have
direct or prefix art candidates and roughly 459 remain. These estimates
exclude aliases.

## Tranche 14 — ten rail and field-maintenance inventory assets

All ten IDs are authored in `Assets/StreamingAssets/Data/items.json` and had
no direct art or item-sprite path in the current `AssetRegistry` search roots.
The 512×512 opaque JPEGs continue the charcoal-backed item style. Godot
created a matching `.jpg.import` sidecar for each asset.

| Catalog ID | New runtime file |
|---|---|
| `item_abrasive_grinding_stone` | `assets/art/item_abrasive_grinding_stone.jpg` |
| `item_rail_profiling_cylinder` | `assets/art/item_rail_profiling_cylinder.jpg` |
| `item_spark_suppression_manifold` | `assets/art/item_spark_suppression_manifold.jpg` |
| `item_blowtorch` | `assets/art/item_blowtorch.jpg` |
| `item_pneumatic_capsule_50mm` | `assets/art/item_pneumatic_capsule_50mm.jpg` |
| `item_pneumatic_capsule_100mm` | `assets/art/item_pneumatic_capsule_100mm.jpg` |
| `item_manual_bolt_shears` | `assets/art/item_manual_bolt_shears.jpg` |
| `item_mechanical_breach_ram` | `assets/art/item_mechanical_breach_ram.jpg` |
| `item_rail_control_component` | `assets/art/item_rail_control_component.jpg` |
| `item_track_maintenance_kit` | `assets/art/item_track_maintenance_kit.jpg` |

ImageMagick metadata confirms ten 512×512 JPEGs; contact sheets were
inspected at 64 px and the inventory's 26 px icon size. The 50mm capsule is
long and felt-wrapped; the 100mm capsule is squat with broad locking bands.
`godot --headless --path . --import` exited 0 and imported all ten, with
matching sidecars present. Direct filenames make them available through
`AssetRegistry.GetItem` without code changes. A live inventory screenshot of
these exact items was not captured.

Cumulative direct item art added in these fourteen tranches: 107. Under the
prior static candidate-path method, roughly 518 of 967 item IDs now have
direct or prefix art candidates and roughly 449 remain. These estimates
exclude aliases.

## Tranche 15 — ten personal-keepsake inventory assets

All ten IDs are authored in `Assets/StreamingAssets/Data/items.json` and had
no direct or `item_` prefix art in the current `AssetRegistry` item search
roots. The 512×512 opaque JPEGs continue the charcoal-backed item style.
Godot created a matching `.jpg.import` sidecar for each asset.

| Catalog ID | New runtime file |
|---|---|
| `wedding_ring` | `assets/art/wedding_ring.jpg` |
| `worn_photograph` | `assets/art/worn_photograph.jpg` |
| `recipe_card` | `assets/art/recipe_card.jpg` |
| `recipe_tin` | `assets/art/recipe_tin.jpg` |
| `childs_mitten` | `assets/art/childs_mitten.jpg` |
| `childs_red_scarf` | `assets/art/childs_red_scarf.jpg` |
| `engraved_lighter` | `assets/art/engraved_lighter.jpg` |
| `tarnished_medal` | `assets/art/tarnished_medal.jpg` |
| `pocket_notebook` | `assets/art/pocket_notebook.jpg` |
| `family_apartment_key` | `assets/art/family_apartment_key.jpg` |

ImageMagick metadata confirms ten 512×512 JPEGs; contact sheets were
inspected at 64 px and the inventory's 26 px icon size. The mitten and scarf
were matched to their visible subjects after generation completed out of
request order. `jq empty Assets/StreamingAssets/Data/items.json` exited 0.
`godot --headless --path . --import` exited 0 and imported all ten, with
matching sidecars present. Direct filenames make them available through
`AssetRegistry.GetItem` without code changes. A live inventory screenshot of
these exact items was not captured.

Cumulative direct item art added in these fifteen tranches: 117. Under the
prior static candidate-path method, roughly 528 of 967 item IDs now have
direct or prefix art candidates and roughly 439 remain. These estimates
exclude aliases.

## Tranche 16 — ten precision-workshop and workday-object inventory assets

All ten IDs are authored in `Assets/StreamingAssets/Data/items.json` and had
no direct or `item_` prefix art in the current `AssetRegistry` item search
roots. The 512×512 opaque JPEGs continue the charcoal-backed item style.
Godot created a matching `.jpg.import` sidecar for each asset.

| Catalog ID | New runtime file |
|---|---|
| `machinist_caliper` | `assets/art/machinist_caliper.jpg` |
| `item_gauge_block_set` | `assets/art/item_gauge_block_set.jpg` |
| `item_optical_flat` | `assets/art/item_optical_flat.jpg` |
| `item_surface_plate` | `assets/art/item_surface_plate.jpg` |
| `item_micrometer_set` | `assets/art/item_micrometer_set.jpg` |
| `engineers_slide_rule` | `assets/art/engineers_slide_rule.jpg` |
| `foreman_whistle` | `assets/art/foreman_whistle.jpg` |
| `miners_tag` | `assets/art/miners_tag.jpg` |
| `tram_punch` | `assets/art/tram_punch.jpg` |
| `nurse_fob_watch` | `assets/art/nurse_fob_watch.jpg` |

ImageMagick metadata confirms ten 512×512 JPEGs; contact sheets were
inspected at 64 px and the inventory's 26 px icon size. `jq empty
Assets/StreamingAssets/Data/items.json` exited 0. `godot --headless --path
. --import` exited 0 and imported all ten, with matching sidecars present.
Direct filenames make them available through `AssetRegistry.GetItem` without
code changes. A live inventory screenshot of these exact items was not
captured.

Cumulative direct item art added in these sixteen tranches: 127. Under the
prior static candidate-path method, roughly 538 of 967 item IDs now have
direct or prefix art candidates and roughly 429 remain. These estimates
exclude aliases.

## Tranche 17 — ten everyday-keepsake inventory assets

All ten IDs are authored in `Assets/StreamingAssets/Data/items.json` and had
no direct or `item_` prefix art in the current `AssetRegistry` item search
roots. The 512×512 opaque JPEGs continue the charcoal-backed item style.
Godot created a matching `.jpg.import` sidecar for each asset.

| Catalog ID | New runtime file |
|---|---|
| `tarnished_pocket_watch` | `assets/art/tarnished_pocket_watch.jpg` |
| `farm_ledger` | `assets/art/farm_ledger.jpg` |
| `mechanic_gloves` | `assets/art/mechanic_gloves.jpg` |
| `teachers_stamp` | `assets/art/teachers_stamp.jpg` |
| `bus_ticket` | `assets/art/bus_ticket.jpg` |
| `enamel_mug` | `assets/art/enamel_mug.jpg` |
| `cheap_comb` | `assets/art/cheap_comb.jpg` |
| `matchbook` | `assets/art/matchbook.jpg` |
| `midwife_satchel` | `assets/art/midwife_satchel.jpg` |
| `civil_defense_radio` | `assets/art/civil_defense_radio.jpg` |

ImageMagick metadata confirms ten 512×512 JPEGs; contact sheets were
inspected at 64 px and the inventory's 26 px icon size. The pocket watch has
a cover and chain, distinct from the nurse's fob watch in tranche 16.
`jq empty Assets/StreamingAssets/Data/items.json` exited 0. `godot
--headless --path . --import` exited 0 and imported all ten, with matching
sidecars present. Direct filenames make them available through
`AssetRegistry.GetItem` without code changes. A live inventory screenshot of
these exact items was not captured.

Cumulative direct item art added in these seventeen tranches: 137. Under the
prior static candidate-path method, roughly 548 of 967 item IDs now have
direct or prefix art candidates and roughly 419 remain. These estimates
exclude aliases.

## Tranche 18 — ten records and field-medicine inventory assets

All ten IDs are authored in `Assets/StreamingAssets/Data/items.json` and had
no direct or `item_` prefix art in the current `AssetRegistry` item search
roots. `undelivered_mail` was rejected because its normalized-prefix art
already exists. The 512×512 opaque JPEGs continue the charcoal-backed item
style. Godot created a matching `.jpg.import` sidecar for each asset.

| Catalog ID | New runtime file |
|---|---|
| `shopping_list` | `assets/art/shopping_list.jpg` |
| `keyring_charm` | `assets/art/keyring_charm.jpg` |
| `lighthouse_logbook` | `assets/art/lighthouse_logbook.jpg` |
| `train_ticket_book` | `assets/art/train_ticket_book.jpg` |
| `dog_tags_military` | `assets/art/dog_tags_military.jpg` |
| `blood_sample` | `assets/art/blood_sample.jpg` |
| `silver_scalpel` | `assets/art/silver_scalpel.jpg` |
| `worn_stethoscope` | `assets/art/worn_stethoscope.jpg` |
| `family_heirloom_seeds` | `assets/art/family_heirloom_seeds.jpg` |
| `field_dressing_kit` | `assets/art/field_dressing_kit.jpg` |

ImageMagick metadata confirms ten 512×512 JPEGs; contact sheets were
inspected at 64 px and the inventory's 26 px icon size. `jq empty
Assets/StreamingAssets/Data/items.json` exited 0. `godot --headless --path
. --import` exited 0 and imported all ten, with matching sidecars present.
Direct filenames make them available through `AssetRegistry.GetItem` without
code changes. A live inventory screenshot of these exact items was not
captured.

Cumulative direct item art added in these eighteen tranches: 147. Under the
prior static candidate-path method, roughly 558 of 967 item IDs now have
direct or prefix art candidates and roughly 409 remain. These estimates
exclude aliases.

## Tranche 19 — five seed and harvest inventory pairs

All ten IDs are authored in `Assets/StreamingAssets/Data/items.json` and had
no direct or normalized-prefix art in the current `AssetRegistry` item search
roots. Their tactile isolated-object style follows the existing frost-pea
seed and crop art. The 512×512 opaque JPEGs each have a Godot-generated
`.jpg.import` sidecar.

| Catalog ID | New runtime file |
|---|---|
| `item_seed_hardy_tuber` | `assets/art/item_seed_hardy_tuber.jpg` |
| `crop_hardy_tuber` | `assets/art/crop_hardy_tuber.jpg` |
| `item_seed_ash_grain` | `assets/art/item_seed_ash_grain.jpg` |
| `crop_ash_grain` | `assets/art/crop_ash_grain.jpg` |
| `item_seed_biolum_mushroom` | `assets/art/item_seed_biolum_mushroom.jpg` |
| `crop_biolum_mushroom` | `assets/art/crop_biolum_mushroom.jpg` |
| `item_seed_nutrient_algae` | `assets/art/item_seed_nutrient_algae.jpg` |
| `crop_nutrient_algae` | `assets/art/crop_nutrient_algae.jpg` |
| `item_seed_medicinal_herb` | `assets/art/item_seed_medicinal_herb.jpg` |
| `crop_medicinal_herb` | `assets/art/crop_medicinal_herb.jpg` |

ImageMagick metadata confirms ten 512×512 JPEGs; contact sheets were
inspected at 64 px and the inventory's 26 px icon size. Planting forms are
visibly distinct from their harvests. `jq empty
Assets/StreamingAssets/Data/items.json` exited 0. `godot --headless --path
. --import` exited 0 and imported all ten, with matching sidecars present.
Direct filenames make them available through `AssetRegistry.GetItem` without
code changes. A live inventory screenshot of these exact items was not
captured.

Cumulative direct item art added in these nineteen tranches: 157. Under the
prior static candidate-path method, roughly 568 of 967 item IDs now have
direct or prefix art candidates and roughly 399 remain. These estimates
exclude aliases.

## Tranche 20 — fifteen greenhouse and fermentation inventory assets

All fifteen IDs are authored in `Assets/StreamingAssets/Data/items.json` and
had no direct or normalized-prefix art in the current `AssetRegistry` item
search roots. The 512×512 opaque JPEGs continue the isolated, tactile item
style and each has a Godot-generated `.jpg.import` sidecar.

| Catalog ID | New runtime file |
|---|---|
| `item_seed_leafy_green` | `assets/art/item_seed_leafy_green.jpg` |
| `crop_leafy_green` | `assets/art/crop_leafy_green.jpg` |
| `item_seed_oilseed` | `assets/art/item_seed_oilseed.jpg` |
| `crop_oilseed` | `assets/art/crop_oilseed.jpg` |
| `item_seed_cold_legume` | `assets/art/item_seed_cold_legume.jpg` |
| `crop_cold_legume` | `assets/art/crop_cold_legume.jpg` |
| `item_fermentation_sugar_feedstock` | `assets/art/item_fermentation_sugar_feedstock.jpg` |
| `item_fermentation_starch_feedstock` | `assets/art/item_fermentation_starch_feedstock.jpg` |
| `item_fermentation_culture_starter` | `assets/art/item_fermentation_culture_starter.jpg` |
| `item_fermentation_filter_module` | `assets/art/item_fermentation_filter_module.jpg` |
| `item_fermentation_service_kit` | `assets/art/item_fermentation_service_kit.jpg` |
| `item_fermentation_preservation_concentrate` | `assets/art/item_fermentation_preservation_concentrate.jpg` |
| `item_fermentation_cleaning_reagent` | `assets/art/item_fermentation_cleaning_reagent.jpg` |
| `item_fermented_organic_acid_carboy` | `assets/art/item_fermented_organic_acid_carboy.jpg` |
| `item_fermentation_waste_pomace` | `assets/art/item_fermentation_waste_pomace.jpg` |

ImageMagick metadata confirms fifteen 512×512 JPEGs; contact sheets were
inspected at 64 px and the inventory's 26 px icon size. The three seed/crop
pairs and the fermentation vessels, solids, and kit have distinct silhouettes.
`jq empty Assets/StreamingAssets/Data/items.json` exited 0. `godot
--headless --path . --import` exited 0 and imported all fifteen, with
matching sidecars present. Direct filenames make them available through
`AssetRegistry.GetItem` without code changes. A live inventory screenshot of
these exact items was not captured.

Cumulative direct item art added in these twenty tranches: 172. Under the
prior static candidate-path method, roughly 583 of 967 item IDs now have
direct or prefix art candidates and roughly 384 remain. These estimates
exclude aliases.

## Tranche 21 — fifteen biofuel, machining, and run-flat inventory assets

All fifteen IDs are authored in `Assets/StreamingAssets/Data/items.json` and
had no direct or normalized-prefix art in the current `AssetRegistry` item
search roots. The 512×512 opaque JPEGs continue the isolated, tactile item
style. Each has a Godot-generated `.jpg.import` sidecar.

| Catalog ID | New runtime file |
|---|---|
| `item_crop_waste` | `assets/art/item_crop_waste.jpg` |
| `item_biofuel_low_grade` | `assets/art/item_biofuel_low_grade.jpg` |
| `item_biofuel_generator_grade` | `assets/art/item_biofuel_generator_grade.jpg` |
| `item_biofuel_high_grade` | `assets/art/item_biofuel_high_grade.jpg` |
| `item_separation_media_cartridge` | `assets/art/item_separation_media_cartridge.jpg` |
| `item_machined_blank_small` | `assets/art/item_machined_blank_small.jpg` |
| `item_machined_blank_medium` | `assets/art/item_machined_blank_medium.jpg` |
| `item_internal_spline_hub` | `assets/art/item_internal_spline_hub.jpg` |
| `item_keyed_actuator_collar` | `assets/art/item_keyed_actuator_collar.jpg` |
| `item_cutting_fluid_canister` | `assets/art/item_cutting_fluid_canister.jpg` |
| `item_runflat_insert_utility` | `assets/art/item_runflat_insert_utility.jpg` |
| `item_runflat_insert_reinforced` | `assets/art/item_runflat_insert_reinforced.jpg` |
| `item_rim_bead_kit` | `assets/art/item_rim_bead_kit.jpg` |
| `item_armored_beadlock_set` | `assets/art/item_armored_beadlock_set.jpg` |
| `item_runflat_balancing_kit` | `assets/art/item_runflat_balancing_kit.jpg` |

ImageMagick metadata confirms fifteen opaque 512×512 JPEGs. Contact sheets
were inspected at 64 px and the inventory's 26 px icon size. The fuel grades,
machining stages, and wheel supplies have distinct silhouettes. `jq empty
Assets/StreamingAssets/Data/items.json` exited 0. `godot --headless --path
. --import` exited 0 and imported all fifteen; matching sidecars are present.
Direct filenames make the images available through `AssetRegistry.GetItem`
without code changes. A live inventory screenshot of these exact items was
not captured.

Cumulative direct item art added in these twenty-one tranches: 187. Under the
prior static candidate-path method, roughly 598 of 967 item IDs now have
direct or prefix art candidates and roughly 369 remain. These estimates
exclude aliases.

## Tranche 22 — fifteen apiary and preserved-food inventory assets

All fifteen IDs are authored in `Assets/StreamingAssets/Data/items.json` and
had no direct or normalized-prefix art in the current `AssetRegistry` item
search roots. The 512×512 opaque JPEGs continue the isolated, tactile item
style. Each has a Godot-generated `.jpg.import` sidecar.

| Catalog ID | New runtime file |
|---|---|
| `item_honey_pot` | `assets/art/item_honey_pot.jpg` |
| `item_beeswax_block` | `assets/art/item_beeswax_block.jpg` |
| `item_raw_propolis` | `assets/art/item_raw_propolis.jpg` |
| `item_mead_must_base` | `assets/art/item_mead_must_base.jpg` |
| `item_preservation_salt` | `assets/art/item_preservation_salt.jpg` |
| `item_trade_salt_sack` | `assets/art/item_trade_salt_sack.jpg` |
| `item_medical_saline_salt` | `assets/art/item_medical_saline_salt.jpg` |
| `item_pickled_tubers` | `assets/art/item_pickled_tubers.jpg` |
| `item_dried_mushrooms` | `assets/art/item_dried_mushrooms.jpg` |
| `item_smoked_meat` | `assets/art/item_smoked_meat.jpg` |
| `item_canned_grain_stew` | `assets/art/item_canned_grain_stew.jpg` |
| `item_salted_meat` | `assets/art/item_salted_meat.jpg` |
| `item_fat_confit` | `assets/art/item_fat_confit.jpg` |
| `item_fermented_sauerkraut` | `assets/art/item_fermented_sauerkraut.jpg` |
| `item_honey_preserved_pulp` | `assets/art/item_honey_preserved_pulp.jpg` |

ImageMagick metadata confirms fifteen opaque 512×512 JPEGs. Contact sheets
were inspected at 64 px and the inventory's 26 px icon size. The three salt
forms, honey products, and preserved foods remain distinct at icon scale.
The fat confit was regenerated after review to show its solid fat seal.
`jq empty Assets/StreamingAssets/Data/items.json` exited 0. `godot
--headless --path . --import` exited 0 and imported all fifteen; matching
sidecars are present. Direct filenames make the images available through
`AssetRegistry.GetItem` without code changes. A live inventory screenshot of
these exact items was not captured.

Cumulative direct item art added in these twenty-two tranches: 202. Under the
prior static candidate-path method, roughly 613 of 967 item IDs now have
direct or prefix art candidates and roughly 354 remain. These estimates
exclude aliases.

## Tranche 23 — fifteen advanced equipment inventory assets

All fifteen IDs are authored in `Assets/StreamingAssets/Data/items.json` and
had no direct or normalized-prefix art in the current `AssetRegistry` item
search roots. The 512×512 opaque JPEGs continue the isolated, tactile item
style. Each has a Godot-generated `.jpg.import` sidecar.

| Catalog ID | New runtime file |
|---|---|
| `item_water_filter_advanced` | `assets/art/item_water_filter_advanced.jpg` |
| `item_radio_cipher_rotor` | `assets/art/item_radio_cipher_rotor.jpg` |
| `item_air_filter_hepa_hospital` | `assets/art/item_air_filter_hepa_hospital.jpg` |
| `item_surgical_kit` | `assets/art/item_surgical_kit.jpg` |
| `item_reagent_clean` | `assets/art/item_reagent_clean.jpg` |
| `item_diving_suit_vulcanized` | `assets/art/item_diving_suit_vulcanized.jpg` |
| `item_cloud_seeding_canister` | `assets/art/item_cloud_seeding_canister.jpg` |
| `item_seismic_detector` | `assets/art/item_seismic_detector.jpg` |
| `item_field_guide_annotated` | `assets/art/item_field_guide_annotated.jpg` |
| `item_thermal_lance` | `assets/art/item_thermal_lance.jpg` |
| `item_sentry_targeting_chip` | `assets/art/item_sentry_targeting_chip.jpg` |
| `item_radio_vacuum_tube` | `assets/art/item_radio_vacuum_tube.jpg` |
| `item_battery_reconditioned` | `assets/art/item_battery_reconditioned.jpg` |
| `item_hydroponic_nutrients` | `assets/art/item_hydroponic_nutrients.jpg` |
| `item_military_radio_module` | `assets/art/item_military_radio_module.jpg` |

ImageMagick metadata confirms fifteen opaque 512×512 JPEGs. Contact sheets
were inspected at 64 px and the inventory's 26 px icon size. Filters,
instruments, and electronics retain distinct silhouettes. `jq empty
Assets/StreamingAssets/Data/items.json` exited 0. `godot --headless --path
. --import` exited 0; matching sidecars are present. Direct filenames make
the images available through `AssetRegistry.GetItem` without code changes.
A live inventory screenshot of these exact items was not captured.

Cumulative direct item art added in these twenty-three tranches: 217. Under
the prior static candidate-path method, roughly 628 of 967 item IDs now have
direct or prefix art candidates and roughly 339 remain. These estimates
exclude aliases.
