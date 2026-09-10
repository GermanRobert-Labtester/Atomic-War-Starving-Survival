# Plan 153 discovery producer matrix

The producer ID is the activation context. It does not rewrite the source record’s authored group, timestamp or grave-site string. All rows use the existing `NarrativeDiscoveryCatalog` → `JournalSystem` → `JournalCodex` path and are one-time knowledge discoveries.

| Family | Source IDs | Producer | Surface/channel | Records | Effect of reading |
|---|---|---|---|---:|---|
| Cobalt | `liturgy_cobalt_psalm_of_blue_glow`, `liturgy_cobalt_rite_of_the_lead_shroud` | `loc_settlement_pilgrim_hearth` | map detail / location inspection | 2 | codex knowledge only |
| Cobalt | `liturgy_cobalt_scintillation_confessional`, `liturgy_cobalt_hymn_of_the_half_life` | `loc_ash_sign_shrine` | map detail / location inspection | 2 | codex knowledge only |
| Cobalt | `liturgy_cobalt_anointing_of_the_crater_ash`, `liturgy_cobalt_vow_of_the_unborn_eye` | `loc_pilgrim_switchbacks` | map detail / location inspection | 2 | codex knowledge only |
| Cobalt | `liturgy_cobalt_canopy_of_strontium` | `loc_low_background_lab` | map detail / location inspection | 1 | codex knowledge only |
| Cobalt | `liturgy_cobalt_final_radiant_transcendence` | `government_bunker` | archive desk / shelter archive | 1 | codex knowledge only |
| Iron | `canon_synod_first_law_of_temper`, `canon_synod_rite_of_the_slag_baptism` | `room_foundry` | crafting/foundry room archive | 2 | codex knowledge only |
| Iron | `canon_synod_crucible_oath_of_alloy`, `canon_synod_heresy_of_the_plastic_mold` | `loc_iron_crest` | map detail / location inspection | 2 | codex knowledge only |
| Iron | `canon_synod_anointing_with_quench_oil`, `canon_synod_tithing_of_the_casing_brass` | `loc_foundry_west_stacks` | Silent Foundry context | 2 | codex knowledge only |
| Iron | `canon_synod_sacrament_of_the_file_stroke`, `canon_synod_funeral_of_the_iron_ingot` | `loc_settlement_iron_siding` | map detail / location inspection | 2 | codex knowledge only |
| Hymnal | `hymnal_geophone_tuning_of_the_pickup_coil`, `hymnal_geophone_dirge_of_the_p_wave` | `room_radio_tuner` | radio/archive context | 2 | codex knowledge only |
| Hymnal | `hymnal_geophone_chant_of_the_shear_s_wave`, `hymnal_geophone_lament_of_the_roof_fall` | `location_acoustic_testing_facility` | map detail / location inspection | 2 | codex knowledge only |
| Hymnal | `hymnal_geophone_psalm_of_the_fault_silence`, `hymnal_geophone_echo_of_the_deep_bore` | `loc_signal_hill_tower` | map detail / location inspection | 2 | codex knowledge only |
| Hymnal | `hymnal_geophone_final_rest_in_the_bedrock` | `loc_summit_relay` | radio/archive context | 1 | codex knowledge only |
| Epitaph | `epitaph_scav_rusted_license_plate` | `highway_pileup` | map detail / location inspection | 1 | codex knowledge only |
| Epitaph | `epitaph_scav_spent_casing_cairn` | `loc_wind_gap_ridge` | map detail / location inspection | 1 | codex knowledge only |
| Epitaph | `epitaph_scav_respirator_filter_mound` | `loc_sulfur_knob` | map detail / location inspection | 1 | codex knowledge only |
| Epitaph | `epitaph_scav_dosimeter_zero_tomb` | `loc_shelter_infirmary` | map detail / location inspection | 1 | codex knowledge only |
| Epitaph | `epitaph_scav_shattered_windshield_cross` | `location_abandoned_convoy_yard` | map detail / location inspection | 1 | codex knowledge only |
| Epitaph | `epitaph_scav_dry_canteen_memorial` | `loc_north_gate_water_tower` | map detail / location inspection | 1 | codex knowledge only |
| Epitaph | `epitaph_scav_ash_sign_mass_burial_slate` | `room_memorial_wall` | shelter memorial wall | 1 | codex knowledge only |

No global “posted day” unlock exists. Every source record is reachable through a specific context, and the current-day check only enforces the manifest’s minimum day (1 for this historical corpus).
