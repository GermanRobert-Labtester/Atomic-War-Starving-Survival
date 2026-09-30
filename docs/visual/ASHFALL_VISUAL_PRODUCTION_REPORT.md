# ASHFALL Visual Production Report — 2026-09-29 (Forty-Five Tranches)

Sections 1–20 record tranches 1–8. The tranche 9–45 addenda at the end
record the latest 500 assets and supersede their cumulative counts.

## 1. Git SHA

Baseline `b31915ea2`; uncommitted visual tranche.

## 2. Tools Detected

| Tool | Availability | Used for |
|---|---|---|
| ChatGPT image generation | Usage limit reached after ten tranche-35 images; model version not exposed | 392 item images through tranche 35 |
| Inkscape | Available | One hundred fifty-five locally drawn item assets across tranches 35–45 |
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

## Tranche 24 — fifteen trapping and field-survival inventory assets

All fifteen IDs are authored in `Assets/StreamingAssets/Data/items.json` and
had no direct or normalized-prefix art in the current `AssetRegistry` item
search roots. The 512×512 opaque JPEGs continue the isolated, tactile item
style. Each has a Godot-generated `.jpg.import` sidecar.

| Catalog ID | New runtime file |
|---|---|
| `trap_improvised_wire` | `assets/art/trap_improvised_wire.jpg` |
| `trap_box` | `assets/art/trap_box.jpg` |
| `trap_fish` | `assets/art/trap_fish.jpg` |
| `trap_body_grip` | `assets/art/trap_body_grip.jpg` |
| `trap_snare` | `assets/art/trap_snare.jpg` |
| `trap_deadfall` | `assets/art/trap_deadfall.jpg` |
| `trap_pit` | `assets/art/trap_pit.jpg` |
| `trap_net` | `assets/art/trap_net.jpg` |
| `trap_cage` | `assets/art/trap_cage.jpg` |
| `trap_bird_snare` | `assets/art/trap_bird_snare.jpg` |
| `item_decor_trophy_ash_hound_pelt` | `assets/art/item_decor_trophy_ash_hound_pelt.jpg` |
| `item_fur_mittens` | `assets/art/item_fur_mittens.jpg` |
| `item_boiled_roots` | `assets/art/item_boiled_roots.jpg` |
| `item_collectible_hunting_magazine` | `assets/art/item_collectible_hunting_magazine.jpg` |
| `item_vacuum_seal_canner` | `assets/art/item_vacuum_seal_canner.jpg` |

ImageMagick metadata confirms fifteen opaque 512×512 JPEGs. Contact sheets
were inspected at 64 px and the inventory's 26 px icon size. The ten trap
types retain different mechanisms and silhouettes; the pit trap uses a small
ground cutaway. `jq empty Assets/StreamingAssets/Data/items.json` exited 0.
`godot --headless --path . --import` exited 0; matching sidecars are present.
Direct filenames make the images available through `AssetRegistry.GetItem`
without code changes. A live inventory screenshot of these exact items was
not captured.

Cumulative direct item art added in these twenty-four tranches: 232. Under
the prior static candidate-path method, roughly 643 of 967 item IDs now have
direct or prefix art candidates and roughly 324 remain. These estimates
exclude aliases.

## Tranche 25 — fifteen records, manuals, and keepsake inventory assets

All fifteen IDs are authored in `Assets/StreamingAssets/Data/items.json` and
had no direct or normalized-prefix art in the current `AssetRegistry` item
search roots. The 512×512 opaque JPEGs continue the isolated, tactile item
style. Each has a Godot-generated `.jpg.import` sidecar.

| Catalog ID | New runtime file |
|---|---|
| `item_collectible_vinyl_chamber_record` | `assets/art/item_collectible_vinyl_chamber_record.jpg` |
| `item_collectible_vinyl_civil_broadcast` | `assets/art/item_collectible_vinyl_civil_broadcast.jpg` |
| `item_collectible_vinyl_folk_compilation` | `assets/art/item_collectible_vinyl_folk_compilation.jpg` |
| `item_collectible_field_medicine_handbook` | `assets/art/item_collectible_field_medicine_handbook.jpg` |
| `item_collectible_diesel_service_manual` | `assets/art/item_collectible_diesel_service_manual.jpg` |
| `item_collectible_radio_repair_guide` | `assets/art/item_collectible_radio_repair_guide.jpg` |
| `item_collectible_civil_defense_badge` | `assets/art/item_collectible_civil_defense_badge.jpg` |
| `item_collectible_transit_badge` | `assets/art/item_collectible_transit_badge.jpg` |
| `item_collectible_trade_guild_patch` | `assets/art/item_collectible_trade_guild_patch.jpg` |
| `item_collectible_childs_doll` | `assets/art/item_collectible_childs_doll.jpg` |
| `item_collectible_music_box` | `assets/art/item_collectible_music_box.jpg` |
| `item_collectible_prayer_beads` | `assets/art/item_collectible_prayer_beads.jpg` |
| `item_collectible_team_pennant` | `assets/art/item_collectible_team_pennant.jpg` |
| `item_collectible_civic_token` | `assets/art/item_collectible_civic_token.jpg` |
| `item_collectible_folk_craft` | `assets/art/item_collectible_folk_craft.jpg` |

ImageMagick metadata confirms fifteen opaque 512×512 JPEGs. Contact sheets
were inspected at 64 px and the inventory's 26 px icon size. Records,
manuals, badges, and keepsakes have distinct materials and silhouettes; no
readable text or real insignia was observed in the source sheet. `jq empty
Assets/StreamingAssets/Data/items.json` exited 0. `godot --headless --path
. --import` exited 0; matching sidecars are present. Direct filenames make
the images available through `AssetRegistry.GetItem` without code changes.
A live inventory screenshot of these exact items was not captured.

Cumulative direct item art added in these twenty-five tranches: 247. Under
the prior static candidate-path method, roughly 658 of 967 item IDs now have
direct or prefix art candidates and roughly 309 remain. These estimates
exclude aliases.

## Tranche 26 — fifteen industrial and sensing inventory assets

All fifteen IDs are authored in `Assets/StreamingAssets/Data/items.json` and
had no direct or normalized-prefix art in the current `AssetRegistry` item
search roots. The 512×512 opaque JPEGs continue the isolated, tactile item
style. Each has a Godot-generated `.jpg.import` sidecar.

| Catalog ID | New runtime file |
|---|---|
| `item_ecm_jammer_module` | `assets/art/item_ecm_jammer_module.jpg` |
| `item_hydraulic_ram_assembly` | `assets/art/item_hydraulic_ram_assembly.jpg` |
| `item_fog_mesh_roll` | `assets/art/item_fog_mesh_roll.jpg` |
| `item_powered_mist_assist_module` | `assets/art/item_powered_mist_assist_module.jpg` |
| `item_reinforced_support_cable` | `assets/art/item_reinforced_support_cable.jpg` |
| `item_radar_display_tube` | `assets/art/item_radar_display_tube.jpg` |
| `item_hydraulic_actuator` | `assets/art/item_hydraulic_actuator.jpg` |
| `item_iff_beacon` | `assets/art/item_iff_beacon.jpg` |
| `item_low_noise_sensor_amplifier` | `assets/art/item_low_noise_sensor_amplifier.jpg` |
| `item_geophone_probe` | `assets/art/item_geophone_probe.jpg` |
| `item_bedrock_sensor_rig` | `assets/art/item_bedrock_sensor_rig.jpg` |
| `item_mine_flail_module` | `assets/art/item_mine_flail_module.jpg` |
| `item_hydraulic_drive_motor` | `assets/art/item_hydraulic_drive_motor.jpg` |
| `item_aquifer_isolation_module` | `assets/art/item_aquifer_isolation_module.jpg` |
| `item_surgical_arm_servo` | `assets/art/item_surgical_arm_servo.jpg` |

ImageMagick metadata confirms fifteen opaque 512×512 JPEGs. Contact sheets
were inspected at 64 px and the inventory's 26 px icon size. Mesh, cable,
sensors, radio modules, and hydraulic parts retain distinct silhouettes.
`jq empty Assets/StreamingAssets/Data/items.json` exited 0. `godot
--headless --path . --import` exited 0; matching sidecars are present.
Direct filenames make the images available through `AssetRegistry.GetItem`
without code changes. A live inventory screenshot of these exact items was
not captured.

Cumulative direct item art added in these twenty-six tranches: 262. Under
the prior static candidate-path method, roughly 673 of 967 item IDs now have
direct or prefix art candidates and roughly 294 remain. These estimates
exclude aliases.

## Tranche 27 — fifteen shelter daily-life inventory assets

All fifteen IDs are authored in `Assets/StreamingAssets/Data/items.json` and
had no direct or normalized-prefix art in the current `AssetRegistry` item
search roots. The opaque 512×512 JPEGs use the established isolated, tactile
style. Each has a Godot-generated `.jpg.import` sidecar.

| Catalog ID | New runtime file |
|---|---|
| `item_heavy_wool_coat` | `assets/art/item_heavy_wool_coat.jpg` |
| `item_thermal_parka` | `assets/art/item_thermal_parka.jpg` |
| `item_insulated_boots` | `assets/art/item_insulated_boots.jpg` |
| `item_improvised_burn_barrel` | `assets/art/item_improvised_burn_barrel.jpg` |
| `item_portable_kerosene_heater` | `assets/art/item_portable_kerosene_heater.jpg` |
| `item_acoustic_guitar` | `assets/art/item_acoustic_guitar.jpg` |
| `item_harmonica` | `assets/art/item_harmonica.jpg` |
| `item_playing_cards` | `assets/art/item_playing_cards.jpg` |
| `item_carved_figurine` | `assets/art/item_carved_figurine.jpg` |
| `item_wasteland_sketch` | `assets/art/item_wasteland_sketch.jpg` |
| `slate_and_chalk` | `assets/art/slate_and_chalk.jpg` |
| `school_primer` | `assets/art/school_primer.jpg` |
| `item_flatbread` | `assets/art/item_flatbread.jpg` |
| `item_vegetable_soup` | `assets/art/item_vegetable_soup.jpg` |
| `item_dried_herb_packets` | `assets/art/item_dried_herb_packets.jpg` |

ImageMagick metadata confirms fifteen opaque 512×512 JPEGs. Contact sheets
were inspected at 64 px and the inventory's 26 px icon size; the two coats,
two heat sources, paper items, and food retain distinct silhouettes.
`jq empty Assets/StreamingAssets/Data/items.json` exited 0.
`godot --headless --path . --import` exited 0 and all matching import
sidecars are present. Exact filenames make these images available through
`AssetRegistry.GetItem` without code changes. A live inventory screenshot
of these exact items was not captured.

Cumulative direct item art added in these twenty-seven tranches: 277. Under
the prior static candidate-path method, roughly 688 of 967 item IDs now have
direct or prefix art candidates and roughly 279 remain. These estimates
exclude aliases.

## Tranche 28 — fifteen medical, rail, and cultivation inventory assets

All fifteen IDs are authored in `Assets/StreamingAssets/Data/items.json` and
had no direct or normalized-prefix art in the current `AssetRegistry` item
search roots. The opaque 512×512 JPEGs use the established isolated, tactile
style. Each has a Godot-generated `.jpg.import` sidecar.

| Catalog ID | New runtime file |
|---|---|
| `scalpel` | `assets/art/scalpel.jpg` |
| `forceps` | `assets/art/forceps.jpg` |
| `surgical_suture` | `assets/art/surgical_suture.jpg` |
| `surgical_saw` | `assets/art/surgical_saw.jpg` |
| `prosthetic_wooden_arm` | `assets/art/prosthetic_wooden_arm.jpg` |
| `prosthetic_wooden_leg` | `assets/art/prosthetic_wooden_leg.jpg` |
| `bionic_arm_prototype` | `assets/art/bionic_arm_prototype.jpg` |
| `bionic_leg_prototype` | `assets/art/bionic_leg_prototype.jpg` |
| `train_coal` | `assets/art/train_coal.jpg` |
| `steel_rail_segment` | `assets/art/steel_rail_segment.jpg` |
| `railroad_ties` | `assets/art/railroad_ties.jpg` |
| `fungus_spores_common` | `assets/art/fungus_spores_common.jpg` |
| `fungus_spores_bioluminescent` | `assets/art/fungus_spores_bioluminescent.jpg` |
| `fungus_spores_medicinal` | `assets/art/fungus_spores_medicinal.jpg` |
| `harvested_mushrooms_subterranean` | `assets/art/harvested_mushrooms_subterranean.jpg` |

ImageMagick metadata confirms fifteen opaque 512×512 JPEGs. Contact sheets
were inspected at 64 px and the inventory's 26 px icon size. The surgical
tools, wood and metal limbs, rail supplies, and distinct fungal cultures
retain identifiable forms. `jq empty Assets/StreamingAssets/Data/items.json`
exited 0. `godot --headless --path . --import` exited 0 and all matching
sidecars are present. Exact filenames make these images available through
`AssetRegistry.GetItem` without code changes. A live inventory screenshot
of these exact items was not captured.

Cumulative direct item art added in these twenty-eight tranches: 292. Under
the prior static candidate-path method, roughly 703 of 967 item IDs now have
direct or prefix art candidates and roughly 264 remain. These estimates
exclude aliases.

## Tranche 29 — fifteen civic, field, workshop, and personal inventory assets

All fifteen IDs are authored in `Assets/StreamingAssets/Data/items.json` and
had no direct or normalized-prefix art in the current `AssetRegistry` item
search roots. The opaque 512×512 JPEGs use the established isolated, tactile
style. Each has a Godot-generated `.jpg.import` sidecar.

| Catalog ID | New runtime file |
|---|---|
| `item_official_ballot_box` | `assets/art/item_official_ballot_box.jpg` |
| `item_pemmican` | `assets/art/item_pemmican.jpg` |
| `item_travel_ration` | `assets/art/item_travel_ration.jpg` |
| `item_vibration_dampening_mount` | `assets/art/item_vibration_dampening_mount.jpg` |
| `item_iron_pyrite_ore` | `assets/art/item_iron_pyrite_ore.jpg` |
| `item_industrial_acid_carboy` | `assets/art/item_industrial_acid_carboy.jpg` |
| `item_neutralizer_lime_bag` | `assets/art/item_neutralizer_lime_bag.jpg` |
| `item_grain_flour` | `assets/art/item_grain_flour.jpg` |
| `item_oxygen_supply` | `assets/art/item_oxygen_supply.jpg` |
| `item_titanium_breaching_shield` | `assets/art/item_titanium_breaching_shield.jpg` |
| `item_coated_turbine_blade` | `assets/art/item_coated_turbine_blade.jpg` |
| `sandbags` | `assets/art/sandbags.jpg` |
| `item_worn_pet_collar` | `assets/art/item_worn_pet_collar.jpg` |
| `item_pharmacist_ledger` | `assets/art/item_pharmacist_ledger.jpg` |
| `item_grandfathers_soldering_iron` | `assets/art/item_grandfathers_soldering_iron.jpg` |

ImageMagick metadata confirms fifteen opaque 512×512 JPEGs. Contact sheets
were inspected at 64 px and the inventory's 26 px icon size. The two
rations, acid carboy versus oxygen supply, lime versus flour, and civic versus
personal objects retain distinct forms. `jq empty
Assets/StreamingAssets/Data/items.json` exited 0. `godot --headless
--path . --import` exited 0 and all matching sidecars are present. Exact
filenames make these images available through `AssetRegistry.GetItem`
without code changes. A live inventory screenshot of these exact items was
not captured.

Cumulative direct item art added in these twenty-nine tranches: 307. Under
the prior static candidate-path method, roughly 718 of 967 item IDs now have
direct or prefix art candidates and roughly 249 remain. These estimates
exclude aliases.

## Tranche 30 — fifteen trophy, salvage, and archive inventory assets

All fifteen IDs are authored in `Assets/StreamingAssets/Data/items.json` and
had no direct or normalized-prefix art in the current `AssetRegistry` item
search roots. The opaque 512×512 JPEGs use the established isolated, tactile
style. Each has a Godot-generated `.jpg.import` sidecar.

| Catalog ID | New runtime file |
|---|---|
| `item_sludge_cake` | `assets/art/item_sludge_cake.jpg` |
| `item_tailings_drum` | `assets/art/item_tailings_drum.jpg` |
| `item_decor_trophy_beetle_carapace` | `assets/art/item_decor_trophy_beetle_carapace.jpg` |
| `item_decor_trophy_molerat_skull` | `assets/art/item_decor_trophy_molerat_skull.jpg` |
| `item_decor_trophy_crow_feathers` | `assets/art/item_decor_trophy_crow_feathers.jpg` |
| `item_decor_trophy_pheasant_plume` | `assets/art/item_decor_trophy_pheasant_plume.jpg` |
| `item_decor_trophy_gulden_wolf` | `assets/art/item_decor_trophy_gulden_wolf.jpg` |
| `item_decor_trophy_kestrel_wings` | `assets/art/item_decor_trophy_kestrel_wings.jpg` |
| `item_archive_index_cylinder` | `assets/art/item_archive_index_cylinder.jpg` |
| `concrete_rubble` | `assets/art/concrete_rubble.jpg` |
| `empty_toner_cartridge` | `assets/art/empty_toner_cartridge.jpg` |
| `mineral_chunk` | `assets/art/mineral_chunk.jpg` |
| `organic_residue` | `assets/art/organic_residue.jpg` |
| `item_foundry_weather_canister` | `assets/art/item_foundry_weather_canister.jpg` |
| `item_brined_legume_mash` | `assets/art/item_brined_legume_mash.jpg` |

ImageMagick metadata confirms fifteen opaque 512×512 JPEGs. Contact sheets
were inspected at 64 px and the inventory's 26 px icon size. Six non-graphic
fictional creature trophies, industrial waste and salvage, archive hardware,
and a stoneware food crock remain visually distinct. The `gulden_wolf`
and `kestrel_wings` filenames follow their catalog descriptions: dust lynx
and iron crow. `jq empty Assets/StreamingAssets/Data/items.json` exited 0.
`godot --headless --path . --import` exited 0 and all matching sidecars
are present. Exact filenames make these images available through
`AssetRegistry.GetItem` without code changes. A live inventory screenshot
of these exact items was not captured.

Cumulative direct item art added in these thirty tranches: 322. Under the
prior static candidate-path method, roughly 733 of 967 item IDs now have
direct or prefix art candidates and roughly 234 remain. These estimates
exclude aliases.

## Tranche 31 — fifteen expedition and intelligence inventory assets

All fifteen IDs are authored in `Assets/StreamingAssets/Data/items.json` and
had no direct or normalized-prefix art in the current `AssetRegistry` item
search roots. The opaque 512×512 JPEGs use the established isolated, tactile
style. Each has a Godot-generated `.jpg.import` sidecar.

| Catalog ID | New runtime file |
|---|---|
| `camo_ash_cloak` | `assets/art/camo_ash_cloak.jpg` |
| `camo_ghillie_shroud` | `assets/art/camo_ghillie_shroud.jpg` |
| `camo_night_stalker_suit` | `assets/art/camo_night_stalker_suit.jpg` |
| `night_optics_goggles` | `assets/art/night_optics_goggles.jpg` |
| `item_aviation_fuel_canister` | `assets/art/item_aviation_fuel_canister.jpg` |
| `item_aircraft_airframe_spares` | `assets/art/item_aircraft_airframe_spares.jpg` |
| `item_decryption_keycard_prewar` | `assets/art/item_decryption_keycard_prewar.jpg` |
| `item_comm_codebook_alpha` | `assets/art/item_comm_codebook_alpha.jpg` |
| `item_logistics_cipher_sheet` | `assets/art/item_logistics_cipher_sheet.jpg` |
| `dog_tags_personal` | `assets/art/dog_tags_personal.jpg` |
| `stolen_ration_cache` | `assets/art/stolen_ration_cache.jpg` |
| `iron_shackles` | `assets/art/iron_shackles.jpg` |
| `item_warlord_trophy` | `assets/art/item_warlord_trophy.jpg` |
| `sedative_draught` | `assets/art/sedative_draught.jpg` |
| `gene_therapy_retroviral_vial` | `assets/art/gene_therapy_retroviral_vial.jpg` |

ImageMagick metadata confirms fifteen opaque 512×512 JPEGs. Contact sheets
were inspected at 64 px and the inventory's 26 px icon size. The dark night
suit was regenerated once for a clearer outline at 26 px. Concealment
garments, optics, fuel and aircraft parts, coded records, evidence, and
medical vials retain distinct silhouettes. `jq empty
Assets/StreamingAssets/Data/items.json` exited 0. `godot --headless
--path . --import` exited 0 and all matching sidecars are present. Exact
filenames make these images available through `AssetRegistry.GetItem`
without code changes. A live inventory screenshot of these exact items was
not captured.

Cumulative direct item art added in these thirty-one tranches: 337. Under
the prior static candidate-path method, roughly 748 of 967 item IDs now have
direct or prefix art candidates and roughly 219 remain. These estimates
exclude aliases.

## Tranche 32 — fifteen industrial, diagnostic, and aeroponic inventory assets

All fifteen IDs are authored in `Assets/StreamingAssets/Data/items.json` and
had no direct or normalized-prefix art in the current `AssetRegistry` item
search roots. The opaque 512×512 JPEGs use the established isolated, tactile
style. Each has a Godot-generated `.jpg.import` sidecar.

| Catalog ID | New runtime file |
|---|---|
| `item_industrial_cell_anode` | `assets/art/item_industrial_cell_anode.jpg` |
| `item_ebpvd_ceramic_target_ingot` | `assets/art/item_ebpvd_ceramic_target_ingot.jpg` |
| `item_electron_gun_tungsten_filament` | `assets/art/item_electron_gun_tungsten_filament.jpg` |
| `item_mcraly_bond_coat_powder` | `assets/art/item_mcraly_bond_coat_powder.jpg` |
| `item_coated_combustor_tile` | `assets/art/item_coated_combustor_tile.jpg` |
| `item_coated_diesel_injector` | `assets/art/item_coated_diesel_injector.jpg` |
| `item_ebpvd_vacuum_pump_seal` | `assets/art/item_ebpvd_vacuum_pump_seal.jpg` |
| `item_hardened_flail_chain` | `assets/art/item_hardened_flail_chain.jpg` |
| `item_armored_blast_shield` | `assets/art/item_armored_blast_shield.jpg` |
| `item_pdms_silicone_kit` | `assets/art/item_pdms_silicone_kit.jpg` |
| `item_assay_reagent_pack` | `assets/art/item_assay_reagent_pack.jpg` |
| `item_microfluidic_cartridge_general` | `assets/art/item_microfluidic_cartridge_general.jpg` |
| `item_aeroponic_medicinal_root` | `assets/art/item_aeroponic_medicinal_root.jpg` |
| `item_aeroponic_food_leaf` | `assets/art/item_aeroponic_food_leaf.jpg` |
| `item_insect_larvae_meal` | `assets/art/item_insect_larvae_meal.jpg` |

ImageMagick metadata confirms fifteen opaque 512×512 JPEGs. Contact sheets
were inspected at 64 px and the inventory's 26 px icon size. Coating parts,
diagnostic supplies, and crops retain distinct shapes at inventory size.
`jq empty Assets/StreamingAssets/Data/items.json` exited 0.
`godot --headless --path . --import` exited 0 and all matching sidecars
are present. Exact filenames make these images available through
`AssetRegistry.GetItem` without code changes. A live inventory screenshot
of these exact items was not captured.

Cumulative direct item art added in these thirty-two tranches: 352. Under
the prior static candidate-path method, roughly 763 of 967 item IDs now have
direct or prefix art candidates and roughly 204 remain. These estimates
exclude aliases.

## Tranche 33 — fifteen metallurgy, process, and sealed-supply inventory assets

All fifteen IDs are authored in `Assets/StreamingAssets/Data/items.json` and
had no direct or normalized-prefix art in the current `AssetRegistry` item
search roots. The opaque 512×512 JPEGs use the established isolated, tactile
style. Each has a Godot-generated `.jpg.import` sidecar.

| Catalog ID | New runtime file |
|---|---|
| `item_metallurgy_heavy_i_beam` | `assets/art/item_metallurgy_heavy_i_beam.jpg` |
| `item_metallurgy_shoring_plate` | `assets/art/item_metallurgy_shoring_plate.jpg` |
| `item_metallurgy_gear_blank` | `assets/art/item_metallurgy_gear_blank.jpg` |
| `item_metallurgy_shaft_stock` | `assets/art/item_metallurgy_shaft_stock.jpg` |
| `item_metallurgy_tool_blank` | `assets/art/item_metallurgy_tool_blank.jpg` |
| `item_industrial_oxidizer_reagent` | `assets/art/item_industrial_oxidizer_reagent.jpg` |
| `item_battery_electrolyte_concentrate` | `assets/art/item_battery_electrolyte_concentrate.jpg` |
| `item_foundry_pickling_reagent` | `assets/art/item_foundry_pickling_reagent.jpg` |
| `item_battery_maintenance_fluid` | `assets/art/item_battery_maintenance_fluid.jpg` |
| `item_nitrogen_supply` | `assets/art/item_nitrogen_supply.jpg` |
| `item_linear_breach_section` | `assets/art/item_linear_breach_section.jpg` |
| `item_sealed_packaging_foil` | `assets/art/item_sealed_packaging_foil.jpg` |
| `item_silo_pest_treatment` | `assets/art/item_silo_pest_treatment.jpg` |
| `item_rock_salt_sack` | `assets/art/item_rock_salt_sack.jpg` |
| `item_caustic_soda_flakes` | `assets/art/item_caustic_soda_flakes.jpg` |

ImageMagick metadata confirms fifteen opaque 512×512 JPEGs. Contact sheets
were inspected at 64 px and the inventory's 26 px icon size. The beam, plate,
gear, rods, tool blank, bottles, cylinder, pouch, foil, minerals, and sealed
capsule retain distinct silhouettes.
`jq empty Assets/StreamingAssets/Data/items.json` exited 0.
`godot --headless --path . --import` exited 0 and all matching sidecars
are present. Exact filenames make these images available through
`AssetRegistry.GetItem` without code changes. A live inventory screenshot
of these exact items was not captured.

Cumulative direct item art added in these thirty-three tranches: 367. Under
the prior static candidate-path method, roughly 778 of 967 item IDs now have
direct or prefix art candidates and roughly 189 remain. These estimates
exclude aliases.

## Tranche 34 — fifteen foundry, chemistry, and archive inventory assets

All fifteen IDs are authored in `Assets/StreamingAssets/Data/items.json` and
had no direct or normalized-prefix art in the current `AssetRegistry` item
search roots. The opaque 512×512 JPEGs use the established isolated, tactile
style. Each has a Godot-generated `.jpg.import` sidecar.

| Catalog ID | New runtime file |
|---|---|
| `chemical_solvent` | `assets/art/chemical_solvent.jpg` |
| `item_foundry_cast_shot` | `assets/art/item_foundry_cast_shot.jpg` |
| `item_foundry_casing_blanks` | `assets/art/item_foundry_casing_blanks.jpg` |
| `item_liquid_bleach_carboy` | `assets/art/item_liquid_bleach_carboy.jpg` |
| `item_metallurgy_iron_ingot` | `assets/art/item_metallurgy_iron_ingot.jpg` |
| `item_metallurgy_copper_ingot` | `assets/art/item_metallurgy_copper_ingot.jpg` |
| `item_metallurgy_steel_billet` | `assets/art/item_metallurgy_steel_billet.jpg` |
| `item_metallurgy_solder_stock` | `assets/art/item_metallurgy_solder_stock.jpg` |
| `item_metallurgy_spring_steel_billet` | `assets/art/item_metallurgy_spring_steel_billet.jpg` |
| `item_metallurgy_shielding_plate` | `assets/art/item_metallurgy_shielding_plate.jpg` |
| `item_tablet_binder` | `assets/art/item_tablet_binder.jpg` |
| `item_tablet_coating_base` | `assets/art/item_tablet_coating_base.jpg` |
| `paper_stock` | `assets/art/paper_stock.jpg` |
| `microfiche_film` | `assets/art/microfiche_film.jpg` |
| `acetate_blank_disc` | `assets/art/acetate_blank_disc.jpg` |

ImageMagick metadata confirms fifteen opaque 512×512 JPEGs. Contact sheets
were inspected at 64 px and the inventory's 26 px icon size. The bottles,
cast shot, brass housings, metal stock, plate, binder, paper, film, and disc
retain distinct silhouettes. `jq empty Assets/StreamingAssets/Data/items.json`
exited 0. `godot --headless --path . --import` exited 0 and all matching
sidecars are present. Exact filenames make these images available through
`AssetRegistry.GetItem` without code changes. A live inventory screenshot
of these exact items was not captured.

Cumulative direct item art added in these thirty-four tranches: 382. Under
the prior static candidate-path method, roughly 793 of 967 item IDs now have
direct or prefix art candidates and roughly 174 remain. These estimates
exclude aliases.

## Tranche 35 — fifteen dispensary and knowledge-keeping inventory assets

All fifteen IDs are authored in `Assets/StreamingAssets/Data/items.json` and
had no direct or normalized-prefix art in the current `AssetRegistry` item
search roots. Ten lab items were made with ChatGPT image generation. The
image service then reached its usage limit; four manuals and the burial
register were constructed locally as SVG illustrations and rendered with
Inkscape. Those five editable SVG sources are in
`docs/visual/sources/tranche35/`. The final runtime files are fifteen opaque
512×512 JPEGs with Godot-generated `.jpg.import` sidecars.

| Catalog ID | New runtime file |
|---|---|
| `item_medical_precursor_base` | `assets/art/item_medical_precursor_base.jpg` |
| `item_sterile_solvent_pack` | `assets/art/item_sterile_solvent_pack.jpg` |
| `item_chem_hyper_stim` | `assets/art/item_chem_hyper_stim.jpg` |
| `item_chem_dulcimer_tincture` | `assets/art/item_chem_dulcimer_tincture.jpg` |
| `item_chem_clarity_salts` | `assets/art/item_chem_clarity_salts.jpg` |
| `item_chem_haze_resin` | `assets/art/item_chem_haze_resin.jpg` |
| `item_chem_fungal_antibiotic` | `assets/art/item_chem_fungal_antibiotic.jpg` |
| `item_chem_spore_sedative` | `assets/art/item_chem_spore_sedative.jpg` |
| `item_chem_choke_spore_toxin` | `assets/art/item_chem_choke_spore_toxin.jpg` |
| `item_oxidizer_reagent_flask` | `assets/art/item_oxidizer_reagent_flask.jpg` |
| `item_manual_generator_maintenance` | `assets/art/item_manual_generator_maintenance.jpg` |
| `item_manual_field_medicine` | `assets/art/item_manual_field_medicine.jpg` |
| `item_manual_rough_repairs` | `assets/art/item_manual_rough_repairs.jpg` |
| `item_manual_seismology` | `assets/art/item_manual_seismology.jpg` |
| `item_undertakers_register` | `assets/art/item_undertakers_register.jpg` |

ImageMagick metadata confirms fifteen opaque 512×512 JPEGs. Contact sheets
were inspected at 64 px and the inventory's 26 px icon size. The manuals
have a flatter illustration style than the ten rendered lab objects, with
distinct cover colors and symbols at inventory size.
`jq empty Assets/StreamingAssets/Data/items.json` exited 0.
`godot --headless --path . --import` exited 0 and all fifteen JPEG sidecars are present.
Exact filenames make these images available through `AssetRegistry.GetItem`
without code changes. A live inventory screenshot of these exact items was
not captured.

Cumulative direct item art added in these thirty-five tranches: 397. Under
the prior static candidate-path method, roughly 808 of 967 item IDs now have
direct or prefix art candidates and roughly 159 remain. These estimates
exclude aliases.

## Tranche 36 — fifteen archival collectible inventory assets

All fifteen IDs are authored in `Assets/StreamingAssets/Data/items.json` and
had no direct or normalized-prefix art in the current `AssetRegistry` item
search roots. The image service remained within its reported usage-limit
window, so this tranche used local SVG illustrations rendered with Inkscape.
The fifteen editable sources are in `docs/visual/sources/tranche36/`; the
runtime assets are opaque 512×512 JPEGs with Godot import sidecars.

| Catalog ID | New runtime file |
|---|---|
| `item_collectible_family_portrait` | `assets/art/item_collectible_family_portrait.jpg` |
| `item_collectible_unit_photograph` | `assets/art/item_collectible_unit_photograph.jpg` |
| `item_collectible_civil_defense_poster` | `assets/art/item_collectible_civil_defense_poster.jpg` |
| `item_collectible_propaganda_poster` | `assets/art/item_collectible_propaganda_poster.jpg` |
| `item_collectible_concert_poster` | `assets/art/item_collectible_concert_poster.jpg` |
| `item_collectible_pre_war_novel` | `assets/art/item_collectible_pre_war_novel.jpg` |
| `item_collectible_science_magazine` | `assets/art/item_collectible_science_magazine.jpg` |
| `item_collectible_water_treatment_handbook` | `assets/art/item_collectible_water_treatment_handbook.jpg` |
| `item_collectible_air_filter_manual` | `assets/art/item_collectible_air_filter_manual.jpg` |
| `item_collectible_dosimeter_guide` | `assets/art/item_collectible_dosimeter_guide.jpg` |
| `item_collectible_unit_log_fragment` | `assets/art/item_collectible_unit_log_fragment.jpg` |
| `item_collectible_deployment_order` | `assets/art/item_collectible_deployment_order.jpg` |
| `item_collectible_casualty_list` | `assets/art/item_collectible_casualty_list.jpg` |
| `item_collectible_mothers_letter` | `assets/art/item_collectible_mothers_letter.jpg` |
| `item_collectible_soldiers_letter` | `assets/art/item_collectible_soldiers_letter.jpg` |

The portraits, posters, books, records, and letters use distinct silhouettes
and restrained colors. They contain no readable text, real insignia, or
recognizable people. ImageMagick metadata confirms 15/15 opaque 512×512
JPEGs. Contact sheets were inspected at 64 px and the inventory's 26 px
icon size. `jq empty Assets/StreamingAssets/Data/items.json` exited 0.
`godot --headless --path . --import` exited 0 and all fifteen `.jpg.import`
sidecars are present. Exact filenames make these images available through
`AssetRegistry.GetItem` without code changes. A live inventory screenshot
of these exact items was not captured.

Cumulative direct item art added in these thirty-six tranches: 412. Under
the prior static candidate-path method, roughly 823 of 967 item IDs now have
direct or prefix art candidates and roughly 144 remain. These estimates
exclude aliases.

## Tranche 37 — fifteen archival print, map, and document inventory assets

All fifteen IDs are authored in `Assets/StreamingAssets/Data/items.json` and
had no direct or normalized-prefix art in the checked `AssetRegistry` item
search roots. Local SVG illustrations were rendered with Inkscape; the editable
sources are in `docs/visual/sources/tranche37/`. The runtime assets are
opaque 512×512 JPEGs with Godot import sidecars.

| Catalog ID | New runtime file |
|---|---|
| `item_collectible_rejection_letter` | `assets/art/item_collectible_rejection_letter.jpg` |
| `item_collectible_military_patch` | `assets/art/item_collectible_military_patch.jpg` |
| `item_collectible_prayer_book` | `assets/art/item_collectible_prayer_book.jpg` |
| `item_collectible_match_program` | `assets/art/item_collectible_match_program.jpg` |
| `item_collectible_exchange_day_newspaper` | `assets/art/item_collectible_exchange_day_newspaper.jpg` |
| `item_collectible_local_newspaper` | `assets/art/item_collectible_local_newspaper.jpg` |
| `item_collectible_road_map` | `assets/art/item_collectible_road_map.jpg` |
| `item_collectible_topo_map` | `assets/art/item_collectible_topo_map.jpg` |
| `item_collectible_survivor_map` | `assets/art/item_collectible_survivor_map.jpg` |
| `item_document_evacuation_list` | `assets/art/item_document_evacuation_list.jpg` |
| `item_document_ration_record` | `assets/art/item_document_ration_record.jpg` |
| `item_document_blood_trail_note` | `assets/art/item_document_blood_trail_note.jpg` |
| `item_document_barricade_placement` | `assets/art/item_document_barricade_placement.jpg` |
| `item_document_sealed_door_warning` | `assets/art/item_document_sealed_door_warning.jpg` |
| `item_document_family_photograph` | `assets/art/item_document_family_photograph.jpg` |

The patch, books, newspapers, maps, records, notices, and photograph use
distinct silhouettes and restrained fictional colors. They contain no
readable text, real insignia, or recognizable people. ImageMagick metadata
confirms 15/15 opaque 512×512 JPEGs. Contact sheets were inspected at 64 px
and the inventory's 26 px icon size.
`jq empty Assets/StreamingAssets/Data/items.json` exited 0.
`godot --headless --path . --import` exited 0 and all fifteen `.jpg.import`
sidecars are present. Exact filenames make these images available through
`AssetRegistry.GetItem` without code changes. A live inventory screenshot
of these exact items was not captured.

Cumulative direct item art added in these thirty-seven tranches: 427. Under
the prior static candidate-path method, roughly 838 of 967 item IDs now have
direct or prefix art candidates and roughly 129 remain. These estimates
exclude aliases.

## Tranche 38 — fifteen personal effects and record inventory assets

Six personal effects and nine documents were selected from authored
`Assets/StreamingAssets/Data/items.json` IDs with no direct or
normalized-prefix art in the checked `AssetRegistry` item search roots.
Local SVG illustrations were rendered with Inkscape; editable sources are in
`docs/visual/sources/tranche38/`. The runtime assets are opaque 512×512
JPEGs with Godot import sidecars.

| Catalog ID | New runtime file |
|---|---|
| `dog_tags` | `assets/art/dog_tags.jpg` |
| `photo_album` | `assets/art/photo_album.jpg` |
| `childs_drawing` | `assets/art/childs_drawing.jpg` |
| `teddy_bear` | `assets/art/teddy_bear.jpg` |
| `creased_receipt` | `assets/art/creased_receipt.jpg` |
| `undelivered_mail` | `assets/art/undelivered_mail.jpg` |
| `item_document_military_map` | `assets/art/item_document_military_map.jpg` |
| `item_document_broadcast_transcript` | `assets/art/item_document_broadcast_transcript.jpg` |
| `item_document_vandalized_propaganda` | `assets/art/item_document_vandalized_propaganda.jpg` |
| `item_document_handwritten_warning` | `assets/art/item_document_handwritten_warning.jpg` |
| `item_document_maintenance_record` | `assets/art/item_document_maintenance_record.jpg` |
| `item_document_shelter_rejection_list` | `assets/art/item_document_shelter_rejection_list.jpg` |
| `item_document_ration_theft_ledger` | `assets/art/item_document_ration_theft_ledger.jpg` |
| `item_document_water_notice` | `assets/art/item_document_water_notice.jpg` |
| `item_document_repair_note` | `assets/art/item_document_repair_note.jpg` |

The tags, album, drawing, bear, mail, map, sign, clipboard, and other records
use distinct silhouettes and restrained fictional colors. Markings are
abstract and unreadable. ImageMagick metadata confirms 15/15 opaque 512×512
JPEGs. Contact sheets were inspected at 64 px and the inventory's 26 px
icon size. `jq empty Assets/StreamingAssets/Data/items.json` exited 0.
`godot --headless --path . --import` exited 0 and all fifteen `.jpg.import`
sidecars are present. Exact filenames make these images available through
`AssetRegistry.GetItem` without code changes. A live inventory screenshot
of these exact items was not captured.

Cumulative direct item art added in these thirty-eight tranches: 442. Under
the prior static candidate-path method, roughly 853 of 967 item IDs now have
direct or prefix art candidates and roughly 114 remain. These estimates
exclude aliases.

## Tranche 39 — fifteen physical props, ammunition, and records

Four physical props, six ammunition types, and five documents were selected
from authored `Assets/StreamingAssets/Data/items.json` IDs with no direct or
normalized-prefix art in the checked `AssetRegistry` item search roots.
Local SVG illustrations were rendered with Inkscape; editable sources are in
`docs/visual/sources/tranche39/`. The runtime assets are opaque 512×512
JPEGs with Godot import sidecars.

| Catalog ID | New runtime file |
|---|---|
| `forensic_clue_bloodstained` | `assets/art/forensic_clue_bloodstained.jpg` |
| `weapon_suppressor_improvised` | `assets/art/weapon_suppressor_improvised.jpg` |
| `item_chain_gang_shackles` | `assets/art/item_chain_gang_shackles.jpg` |
| `item_slave_collar` | `assets/art/item_slave_collar.jpg` |
| `ammo_76mm_he_flak` | `assets/art/ammo_76mm_he_flak.jpg` |
| `ammo_76mm_proximity_fuse` | `assets/art/ammo_76mm_proximity_fuse.jpg` |
| `ammo_76mm_tungsten_penetrator` | `assets/art/ammo_76mm_tungsten_penetrator.jpg` |
| `ammo_chaff_burst` | `assets/art/ammo_chaff_burst.jpg` |
| `ammo_76mm_beacon_smokey` | `assets/art/ammo_76mm_beacon_smokey.jpg` |
| `ammo_76mm_shaped_charge` | `assets/art/ammo_76mm_shaped_charge.jpg` |
| `item_document_triage_record` | `assets/art/item_document_triage_record.jpg` |
| `item_document_supply_requisition` | `assets/art/item_document_supply_requisition.jpg` |
| `item_document_quarantine_notice` | `assets/art/item_document_quarantine_notice.jpg` |
| `item_document_radio_log` | `assets/art/item_document_radio_log.jpg` |
| `item_document_weather_gate_warning` | `assets/art/item_document_weather_gate_warning.jpg` |

The ammunition uses six distinct case, tip, and payload silhouettes. The
props and records have separate material and color cues; markings remain
abstract and unreadable. ImageMagick metadata confirms 15/15 opaque 512×512
JPEGs. Contact sheets were inspected at full size, 64 px, and the inventory's
26 px icon size. `jq empty Assets/StreamingAssets/Data/items.json` exited 0.
`godot --headless --path . --import` exited 0 and all fifteen `.jpg.import`
sidecars are present. Exact filenames make these images available through
`AssetRegistry.GetItem` without code changes. A live inventory screenshot
of these exact items was not captured.

Cumulative direct item art added in these thirty-nine tranches: 457. Under
the prior static candidate-path method, roughly 868 of 967 item IDs now have
direct or prefix art candidates and roughly 99 remain. These estimates
exclude aliases.

## Tranche 40 — fifteen records and cassette archive inventory assets

Nine records and six cassette archives were selected from authored
`Assets/StreamingAssets/Data/items.json` IDs with no direct or
normalized-prefix art in the checked `AssetRegistry` item search roots.
Local SVG illustrations were rendered with Inkscape; editable sources are in
`docs/visual/sources/tranche40/`. The runtime assets are opaque 512×512
JPEGs with Godot import sidecars.

| Catalog ID | New runtime file |
|---|---|
| `item_document_last_letter` | `assets/art/item_document_last_letter.jpg` |
| `item_document_field_report` | `assets/art/item_document_field_report.jpg` |
| `item_document_journal_fragment` | `assets/art/item_document_journal_fragment.jpg` |
| `item_document_death_certificate` | `assets/art/item_document_death_certificate.jpg` |
| `item_document_supply_inventory` | `assets/art/item_document_supply_inventory.jpg` |
| `item_document_confession` | `assets/art/item_document_confession.jpg` |
| `item_document_will` | `assets/art/item_document_will.jpg` |
| `item_document_debt_default_notice` | `assets/art/item_document_debt_default_notice.jpg` |
| `item_document_patrol_order` | `assets/art/item_document_patrol_order.jpg` |
| `cassette_greenhouse_tapes_1` | `assets/art/cassette_greenhouse_tapes_1.jpg` |
| `cassette_field_hospital_7_1` | `assets/art/cassette_field_hospital_7_1.jpg` |
| `cassette_evacuation_train_1` | `assets/art/cassette_evacuation_train_1.jpg` |
| `cassette_station_14_1` | `assets/art/cassette_station_14_1.jpg` |
| `cassette_fathers_tapes_1` | `assets/art/cassette_fathers_tapes_1.jpg` |
| `cassette_dam_keeper_log_1` | `assets/art/cassette_dam_keeper_log_1.jpg` |

The records use different paper forms, seals, folds, and color cues. The six
cassettes have separate sleeve colors and symbols. Their markings remain
abstract and unreadable. ImageMagick metadata confirms 15/15 opaque 512×512
JPEGs. Contact sheets were inspected at full size, 64 px, and the inventory's
26 px icon size. `jq empty Assets/StreamingAssets/Data/items.json` exited 0.
`godot --headless --path . --import` exited 0 and all fifteen `.jpg.import`
sidecars are present. Exact filenames make these images available through
`AssetRegistry.GetItem` without code changes. A live inventory screenshot
of these exact items was not captured.

Cumulative direct item art added in these forty tranches: 472. Under the
prior static candidate-path method, roughly 883 of 967 item IDs now have
direct or prefix art candidates and roughly 84 remain. These estimates
exclude aliases.

## Tranche 41 — fifteen document variants and cassette archive assets

Five document or metal variants and ten cassette archives were selected from
authored `Assets/StreamingAssets/Data/items.json` IDs with no direct or
normalized-prefix art in the checked `AssetRegistry` item search roots.
`potassium_iodide` was excluded because prefixed
`item_potassium_iodide` art already exists. Local SVG illustrations were
rendered with Inkscape; editable sources are in
`docs/visual/sources/tranche41/`. The runtime assets are opaque 512×512
JPEGs with Godot import sidecars.

| Catalog ID | New runtime file |
|---|---|
| `item_document_casualty_list` | `assets/art/item_document_casualty_list.jpg` |
| `item_document_evacuation_route_map` | `assets/art/item_document_evacuation_route_map.jpg` |
| `item_document_civil_defense_poster` | `assets/art/item_document_civil_defense_poster.jpg` |
| `item_document_child_drawing` | `assets/art/item_document_child_drawing.jpg` |
| `item_dog_tags_scavenged` | `assets/art/item_dog_tags_scavenged.jpg` |
| `cassette_greenhouse_tapes_2` | `assets/art/cassette_greenhouse_tapes_2.jpg` |
| `cassette_field_hospital_7_2` | `assets/art/cassette_field_hospital_7_2.jpg` |
| `cassette_evacuation_train_2` | `assets/art/cassette_evacuation_train_2.jpg` |
| `cassette_station_14_2` | `assets/art/cassette_station_14_2.jpg` |
| `cassette_teachers_recordings_1` | `assets/art/cassette_teachers_recordings_1.jpg` |
| `cassette_quarantine_tapes_1` | `assets/art/cassette_quarantine_tapes_1.jpg` |
| `cassette_checkpoint_kilo_1` | `assets/art/cassette_checkpoint_kilo_1.jpg` |
| `cassette_saint_maren_1` | `assets/art/cassette_saint_maren_1.jpg` |
| `cassette_family_bunker_1` | `assets/art/cassette_family_bunker_1.jpg` |
| `cassette_free_radio_1` | `assets/art/cassette_free_radio_1.jpg` |

The five noncassette variants use different wear and framing than existing
art for similar subjects. The ten tapes have distinct sleeves and symbols
for their archive families. Markings remain abstract and unreadable.
ImageMagick metadata confirms 15/15 opaque 512×512 JPEGs. Contact sheets
were inspected at full size, 64 px, and the inventory's 26 px icon size.
`jq empty Assets/StreamingAssets/Data/items.json` exited 0.
`godot --headless --path . --import` exited 0 and all fifteen `.jpg.import`
sidecars are present. Exact filenames make these images available through
`AssetRegistry.GetItem` without code changes. A live inventory screenshot
of these exact items was not captured.

Cumulative direct item art added in these forty-one tranches: 487. Under
the prior static candidate-path method, roughly 898 of 967 item IDs now
have direct or prefix art candidates and roughly 69 remain. These estimates
exclude aliases.

## Tranche 42 — fifteen cassette archive inventory assets

Fifteen cassette IDs spanning twelve archive series were selected from
authored `Assets/StreamingAssets/Data/items.json` entries with no direct or
normalized-prefix art in the checked `AssetRegistry` item search roots.
Local SVG illustrations were rendered with Inkscape; editable sources are in
`docs/visual/sources/tranche42/`. The runtime assets are opaque 512×512
JPEGs with Godot import sidecars.

| Catalog ID | New runtime file |
|---|---|
| `cassette_greenhouse_tapes_3` | `assets/art/cassette_greenhouse_tapes_3.jpg` |
| `cassette_field_hospital_7_3` | `assets/art/cassette_field_hospital_7_3.jpg` |
| `cassette_field_hospital_7_4` | `assets/art/cassette_field_hospital_7_4.jpg` |
| `cassette_evacuation_train_3` | `assets/art/cassette_evacuation_train_3.jpg` |
| `cassette_station_14_3` | `assets/art/cassette_station_14_3.jpg` |
| `cassette_station_14_4` | `assets/art/cassette_station_14_4.jpg` |
| `cassette_fathers_tapes_2` | `assets/art/cassette_fathers_tapes_2.jpg` |
| `cassette_dam_keeper_log_2` | `assets/art/cassette_dam_keeper_log_2.jpg` |
| `cassette_teachers_recordings_2` | `assets/art/cassette_teachers_recordings_2.jpg` |
| `cassette_quarantine_tapes_2` | `assets/art/cassette_quarantine_tapes_2.jpg` |
| `cassette_quarantine_tapes_3` | `assets/art/cassette_quarantine_tapes_3.jpg` |
| `cassette_checkpoint_kilo_2` | `assets/art/cassette_checkpoint_kilo_2.jpg` |
| `cassette_saint_maren_2` | `assets/art/cassette_saint_maren_2.jpg` |
| `cassette_family_bunker_2` | `assets/art/cassette_family_bunker_2.jpg` |
| `cassette_free_radio_2` | `assets/art/cassette_free_radio_2.jpg` |

Each volume uses a story-specific symbol, sleeve palette, and wear cue;
the cassette silhouette remains consistent across the archive. The marks
are abstract and unreadable. ImageMagick metadata confirms 15/15 opaque
512×512 JPEGs. Contact sheets were inspected at full size, 64 px, and the
inventory's 26 px icon size. `jq empty Assets/StreamingAssets/Data/items.json`
exited 0. `godot --headless --path . --import` exited 0 and all fifteen
`.jpg.import` sidecars are present. Exact filenames make these images
available through `AssetRegistry.GetItem` without code changes. A live
inventory screenshot of these exact items was not captured.

Cumulative direct item art added in these forty-two tranches: 502. Under
the prior static candidate-path method, roughly 913 of 967 item IDs now
have direct or prefix art candidates and roughly 54 remain. These estimates
exclude aliases.

## Tranche 43 — fifteen final-volume cassette archive assets

Fifteen cassette IDs were selected from authored
`Assets/StreamingAssets/Data/items.json` entries with no direct or
normalized-prefix art in the checked `AssetRegistry` item search roots.
Local SVG illustrations were rendered with Inkscape; editable sources are in
`docs/visual/sources/tranche43/`. The runtime assets are opaque 512×512
JPEGs with Godot import sidecars.

| Catalog ID | New runtime file |
|---|---|
| `cassette_field_hospital_7_5` | `assets/art/cassette_field_hospital_7_5.jpg` |
| `cassette_evacuation_train_4` | `assets/art/cassette_evacuation_train_4.jpg` |
| `cassette_station_14_6` | `assets/art/cassette_station_14_6.jpg` |
| `cassette_fathers_tapes_3` | `assets/art/cassette_fathers_tapes_3.jpg` |
| `cassette_fathers_tapes_4` | `assets/art/cassette_fathers_tapes_4.jpg` |
| `cassette_dam_keeper_log_3` | `assets/art/cassette_dam_keeper_log_3.jpg` |
| `cassette_dam_keeper_log_5` | `assets/art/cassette_dam_keeper_log_5.jpg` |
| `cassette_teachers_recordings_3` | `assets/art/cassette_teachers_recordings_3.jpg` |
| `cassette_quarantine_tapes_4` | `assets/art/cassette_quarantine_tapes_4.jpg` |
| `cassette_checkpoint_kilo_3` | `assets/art/cassette_checkpoint_kilo_3.jpg` |
| `cassette_checkpoint_kilo_4` | `assets/art/cassette_checkpoint_kilo_4.jpg` |
| `cassette_saint_maren_3` | `assets/art/cassette_saint_maren_3.jpg` |
| `cassette_family_bunker_3` | `assets/art/cassette_family_bunker_3.jpg` |
| `cassette_free_radio_3` | `assets/art/cassette_free_radio_3.jpg` |
| `cassette_free_radio_4` | `assets/art/cassette_free_radio_4.jpg` |

The sleeves use story-specific symbols and restrained family palettes.
`cassette_station_14_5` and `cassette_dam_keeper_log_4` remain for a later
pass because their descriptions offer less distinctive icon-scale objects.
ImageMagick metadata confirms 15/15 opaque 512×512 JPEGs. Contact sheets
were inspected at full size, 64 px, and the inventory's 26 px icon size.
`jq empty Assets/StreamingAssets/Data/items.json` exited 0.
`godot --headless --path . --import` exited 0 and all fifteen `.jpg.import`
sidecars are present. Exact filenames make these images available through
`AssetRegistry.GetItem` without code changes. A live inventory screenshot
of these exact items was not captured.

Cumulative direct item art added in these forty-three tranches: 517. Under
the prior static candidate-path method, roughly 928 of 967 item IDs now
have direct or prefix art candidates and roughly 39 remain. These estimates
exclude aliases.

## Tranche 44 — final primary cassettes and thirteen greenhouse items

A registry-aware static rescan refined the earlier cumulative coverage
estimate above. Current items.json has 724 IDs; the 967 count is the
aggregate across item catalogs in Godot's coverage report. Of the 724
primary IDs, 722 had a file candidate before this tranche after literal,
alias, and prefix-add lookup through the actual item search roots. The
two unresolved primary IDs were
cassette_station_14_5 and cassette_dam_keeper_log_4. The secondary
greenhouse_items.json catalog had 34 IDs and exactly thirteen unresolved
file candidates. This tranche fills all fifteen gaps without changing data
or lookup code.

| Catalog ID | New runtime file |
|---|---|
| cassette_station_14_5 | assets/art/cassette_station_14_5.jpg |
| cassette_dam_keeper_log_4 | assets/art/cassette_dam_keeper_log_4.jpg |
| item_greenhouse_trowel | assets/art/item_greenhouse_trowel.jpg |
| item_greenhouse_hand_cultivator | assets/art/item_greenhouse_hand_cultivator.jpg |
| item_greenhouse_compost | assets/art/item_greenhouse_compost.jpg |
| item_greenhouse_ash_fertilizer | assets/art/item_greenhouse_ash_fertilizer.jpg |
| item_greenhouse_fish_emulsion | assets/art/item_greenhouse_fish_emulsion.jpg |
| item_greenhouse_insecticidal_soap | assets/art/item_greenhouse_insecticidal_soap.jpg |
| item_greenhouse_sticky_traps | assets/art/item_greenhouse_sticky_traps.jpg |
| item_greenhouse_pest_mesh | assets/art/item_greenhouse_pest_mesh.jpg |
| item_greenhouse_line_filter | assets/art/item_greenhouse_line_filter.jpg |
| item_greenhouse_catchment_kit | assets/art/item_greenhouse_catchment_kit.jpg |
| item_greenhouse_glass_pane | assets/art/item_greenhouse_glass_pane.jpg |
| item_greenhouse_uv_sheeting | assets/art/item_greenhouse_uv_sheeting.jpg |
| item_greenhouse_shade_cloth | assets/art/item_greenhouse_shade_cloth.jpg |

Each image has an editable SVG source under
docs/visual/sources/tranche44/. Catalog descriptions guided the
silhouettes, materials, and fictional cassette sleeve marks. The exact JPEG
filenames resolve through AssetRegistry.GetItem when the canonical item
catalog feeds InventoryPanel. ImageMagick confirmed 15/15 opaque
512×512 JPEGs; contact sheets were inspected at 170, 64, and 26 px.
jq empty passed for both catalogs. godot --headless --path . --import
exited 0 and imported all fifteen images with matching .jpg.import
sidecars. The static candidate counts are therefore 724/724 for primary
items and 34/34 for greenhouse items, inferred from the complete
pre-tranche scan plus these fifteen exact-name additions. A separate
Godot runtime asset coverage report exited 0 and reported 938/967 item
IDs resolved across its aggregate catalog set, with 29 still missing in
other item catalogs. No selected ID appears on that missing list. A live
inventory screenshot was not captured.

Cumulative direct item art added across forty-four tranches: 532.

## Tranche 45 — crossing records, dose instruments, Year of Ash documents, and depot diesel

Godot's prior runtime coverage report listed 29 missing item IDs across
967 aggregate item rows. Fourteen selected IDs had no close existing
unprefixed artwork. The fifteenth, item_diesel_fuel, had only an
unprefixed generic pixel image; its Holdfast catalog description calls
for a Guild-marked depot can. All fifteen new exact-ID images use current
catalog descriptions and load through the existing item lookup.

| Catalog ID | New runtime file |
|---|---|
| item_escort_challenge_ribbon | assets/art/item_escort_challenge_ribbon.jpg |
| item_rejection_notice | assets/art/item_rejection_notice.jpg |
| item_crossing_map | assets/art/item_crossing_map.jpg |
| item_black_market_pouch | assets/art/item_black_market_pouch.jpg |
| item_charter_draft | assets/art/item_charter_draft.jpg |
| item_pocket_dosimeter | assets/art/item_pocket_dosimeter.jpg |
| item_dose_register_book | assets/art/item_dose_register_book.jpg |
| item_cohort_baseline_card | assets/art/item_cohort_baseline_card.jpg |
| item_deserter_coalition_forged_papers | assets/art/item_deserter_coalition_forged_papers.jpg |
| item_long_walk_route_ledger | assets/art/item_long_walk_route_ledger.jpg |
| item_cold_count_provenance_seal | assets/art/item_cold_count_provenance_seal.jpg |
| item_unsigned_debt_ledger_page | assets/art/item_unsigned_debt_ledger_page.jpg |
| item_amnesty_petition_dossier | assets/art/item_amnesty_petition_dossier.jpg |
| item_water_allocation_writ | assets/art/item_water_allocation_writ.jpg |
| item_diesel_fuel | assets/art/item_diesel_fuel.jpg |

Editable SVG sources are under docs/visual/sources/tranche45/. The
fifteen illustrations use cloth, paper, wax, steel, and worn fuel-can
surfaces with distinct icon-scale silhouettes. ImageMagick confirmed
15/15 opaque 512×512 JPEGs, and contact sheets were inspected at 170,
64, and 26 px. jq parsed the five source catalogs. The first sandboxed
Godot import could not write the project's linked .godot cache and left
the new textures unloadable despite exiting 0. A writable-cache headless
import completed without errors and created all fifteen texture cache
entries. The following Godot runtime asset coverage report exited 0:
953/967 aggregate item IDs resolve, up from 938/967, and none of these
fifteen IDs remain missing. The fourteen unresolved IDs all belong to
Holdfast and have semantically matching unprefixed art; the current
registry does not strip the item_ prefix for those IDs. No live inventory
screenshot was captured.

Cumulative direct item art added across forty-five tranches: 547.
