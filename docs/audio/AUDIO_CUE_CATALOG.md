# ASHFALL Audio Cue Architecture Catalog

> **Living Architecture Authority**: Documents all registered audio cues, target Godot audio buses, asset resource paths, loop behavior, volume trim, and cooldown timers in `src/Audio/AudioCueCatalog.cs`.

**Total Registered Cues:** `154`<br>
**Last Verified:** `2026-09-03`<br>
**Drift Gated:** `python3 scripts/ci/generate-audio-catalog.py --check`

---

## 1. Audio Bus Architecture Overview

ASHFALL organizes sound design into 12 dedicated audio buses with independent volume controls and sidechain compression:

| Audio Bus | Purpose | Default Route |
|---|---|---|
| `Master` | Main audio output and final limiting | Hardware Out |
| `Music` | Dynamic score, title theme, exploration underscore | Master |
| `Ambience` | Bunker ventilation hum, wind, weather loop | Master |
| `SFX` | Environmental interactions, explosions, physical items | Master |
| `UI` | Tactile interface clicks, tab switching, confirmations | Master |
| `Voice` | Radio chatter, distress calls, narrator cues | Master |
| `Alerts` | Critical radiation alarms, crisis sirens, warning klaxons | Master |
| `Generator` | Shelter generator rumble and fuel burn | Ambience |
| `Ventilation` | Air intake fan rotation, filter scrubbers | Ambience |
| `Radio` | Tuner static, signal locks, Morse broadcasts | Voice |
| `Medical` | Heartbeat pulse, trauma monitor, resuscitation | Alerts |
| `Surface` | Wasteland dust storms, exterior wind howling | Ambience |

---

## 2. Master Audio Cue Register

| Cue ID | Target Bus | Resource Path | Loop | Volume Trim | Cooldown | Asset Status |
|---|---|---|---|---|---|---|
| `combat_improvised_fire` | `Alerts` | `res://assets/audio/sfx/sfx_weapon_molotov_burst.wav` | No | -2.0 dB | 1.0s | ✅ Exists |
| `combat_last_stand` | `Alerts` | `res://assets/audio/sfx/sfx_combat_last_stand.wav` | No | -2.0 dB | 5.0s | ✅ Exists |
| `combat_start` | `Alerts` | `res://assets/audio/sfx/sfx_combat_start.mp3` | No | -2.0 dB | 5.0s | ✅ Exists |
| `combat_weapon_burst` | `Alerts` | `res://assets/audio/sfx/sfx_weapon_burst_rupture.wav` | No | -1.0 dB | 1.0s | ✅ Exists |
| `danger_alarm_klaxon` | `Alerts` | `res://assets/audio/sfx/sfx_danger_alarm_klaxon.wav` | No | 0.0 dB | 10.0s | ✅ Exists |
| `echo_discovery` | `Alerts` | `res://assets/audio/sfx/sfx_echo_memory_shimmer.wav` | No | -4.0 dB | 3.0s | ✅ Exists |
| `expedition_vehicle_breakdown` | `Alerts` | `res://assets/audio/sfx/sfx_vehicle_breakdown_stall.wav` | No | -2.0 dB | 2.0s | ✅ Exists |
| `flashback_trigger` | `Alerts` | `res://assets/audio/sfx/sfx_flashback_distortion.wav` | No | -3.0 dB | 3.0s | ✅ Exists |
| `hazard_toxic_sizzle` | `Alerts` | `res://assets/audio/sfx/sfx_hazard_toxic_sizzle.mp3` | No | -3.0 dB | 1.0s | ✅ Exists |
| `rad_alert_acute` | `Alerts` | `res://assets/audio/sfx/sfx_radiation_alarm.mp3` | No | -2.0 dB | 5.0s | ✅ Exists |
| `rad_alert_chronic` | `Alerts` | `res://assets/audio/sfx/sfx_radiation_chronic_alarm.wav` | No | -6.0 dB | 10.0s | ✅ Exists |
| `rad_contamination` | `Alerts` | `res://assets/audio/sfx/sfx_contamination_warning.mp3` | No | 0.0 dB | 5.0s | ✅ Exists |
| `radio_ebs_alert` | `Alerts` | `res://assets/audio/radio/radio_ebs_alert.wav` | No | -2.0 dB | 5.0s | ✅ Exists |
| `sfx_artillery_incoming_whistle` | `Alerts` | `res://assets/audio/sfx/sfx_artillery_incoming_whistle.mp3` | No | -3.0 dB | 8.0s | ✅ Exists |
| `shelter_air_filter` | `Alerts` | `res://assets/audio/sfx/sfx_air_filter_degrade.mp3` | No | 0.0 dB | 10.0s | ✅ Exists |
| `train_screech_crash` | `Alerts` | `res://assets/audio/sfx/sfx_train_screech_crash.wav` | No | -2.0 dB | 2.0s | ✅ Exists |
| `trauma_tinnitus` | `Alerts` | `res://assets/audio/sfx/sfx_trauma_tinnitus_ring.wav` | No | -4.0 dB | 4.0s | ✅ Exists |
| `weather_alert` | `Alerts` | `res://assets/audio/sfx/sfx_alarm_klaxon.mp3` | No | -2.0 dB | 5.0s | ✅ Exists |
| `weather_black_rain` | `Alerts` | `res://assets/audio/sfx/sfx_weather_black_rain.wav` | No | 0.0 dB | 10.0s | ✅ Exists |
| `weather_corrosive_precipitation` | `Alerts` | `res://assets/audio/sfx/sfx_weather_corrosive_precipitation.wav` | No | -4.0 dB | 8.0s | ✅ Exists |
| `weather_emp_storm` | `Alerts` | `res://assets/audio/sfx/sfx_weather_emp_storm.wav` | No | -4.0 dB | 8.0s | ✅ Exists |
| `amb_bunker` | `Ambience` | `res://assets/audio/ambience/bunker_ambience.ogg` | Yes | -3.0 dB | — | ✅ Exists |
| `amb_loc_abandoned_hospital` | `Ambience` | `res://assets/audio/ambience/amb_loc_abandoned_hospital.mp3` | Yes | -5.0 dB | — | ✅ Exists |
| `amb_loc_arcology_sector` | `Ambience` | `res://assets/audio/ambience/amb_loc_arcology_sector.mp3` | Yes | -5.0 dB | — | ✅ Exists |
| `amb_loc_granite_quarry` | `Ambience` | `res://assets/audio/ambience/amb_location_granite_quarry_01.wav` | Yes | -8.0 dB | 0.05s | ✅ Exists |
| `amb_loc_military_bunker` | `Ambience` | `res://assets/audio/ambience/amb_loc_military_bunker.mp3` | Yes | -4.0 dB | — | ✅ Exists |
| `expedition_camp_fire` | `Ambience` | `res://assets/audio/ambience/amb_expedition_camp_fire.wav` | Yes | -10.0 dB | — | ✅ Exists |
| `log_tape_hiss` | `Ambience` | `res://assets/audio/sfx/sfx_tape_hiss_loop.wav` | Yes | -18.0 dB | — | ✅ Exists |
| `shelter_water_drip` | `Ambience` | `res://assets/audio/sfx/sfx_water_drip_cave.mp3` | Yes | -15.0 dB | — | ✅ Exists |
| `shelter_water_filtration` | `Ambience` | `res://assets/audio/sfx/sfx_water_filtration_loop.wav` | Yes | -15.0 dB | — | ✅ Exists |
| `trauma_cabin_fever` | `Ambience` | `res://assets/audio/sfx/sfx_trauma_cabin_fever_whisper.wav` | Yes | -14.0 dB | — | ✅ Exists |
| `shelter_generator` | `Generator` | `res://assets/audio/sfx/sfx_generator_cough.mp3` | Yes | -16.0 dB | — | ✅ Exists |
| `shelter_generator_strain` | `Generator` | `res://assets/audio/sfx/sfx_generator_heavy_strain.wav` | Yes | -14.0 dB | — | ✅ Exists |
| `med_infirmary_beep` | `Medical` | `res://assets/audio/sfx/sfx_infirmary_monitor_beep.wav` | No | -12.0 dB | 1.5s | ✅ Exists |
| `med_quarantine_clear` | `Medical` | `res://assets/audio/sfx/sfx_med_quarantine_clear.wav` | No | -8.0 dB | 0.75s | ✅ Exists |
| `med_quarantine_seal` | `Medical` | `res://assets/audio/sfx/sfx_med_quarantine_seal.wav` | No | -7.0 dB | 1.0s | ✅ Exists |
| `med_survivor_death` | `Medical` | `res://assets/audio/sfx/sfx_survivor_death.wav` | No | -6.0 dB | 3.0s | ✅ Exists |
| `combat_defeat` | `Music` | `res://assets/audio/sfx/sfx_combat_defeat.mp3` | No | -8.0 dB | 5.0s | ✅ Exists |
| `game_over` | `Music` | `res://assets/audio/music/game_over.ogg` | No | -10.0 dB | — | ✅ Exists |
| `music_gameplay` | `Music` | `res://assets/audio/music/gameplay_underscore.ogg` | No | -8.0 dB | — | ✅ Exists |
| `music_menu` | `Music` | `res://assets/audio/music/main_menu.ogg` | No | -6.0 dB | — | ✅ Exists |
| `action_crafting` | `SFX` | `res://assets/audio/sfx/sfx_crafting_assemble.mp3` | No | 0.0 dB | 1.0s | ✅ Exists |
| `action_injection` | `SFX` | `res://assets/audio/sfx/sfx_injection.mp3` | No | 0.0 dB | 0.5s | ✅ Exists |
| `action_interrogation_slam` | `SFX` | `res://assets/audio/sfx/sfx_interrogation_slam.mp3` | No | -2.0 dB | 0.5s | ✅ Exists |
| `action_item_pickup` | `SFX` | `res://assets/audio/sfx/sfx_action_item_pickup_01.wav` | No | -4.0 dB | 0.2s | ✅ Exists |
| `action_pill_bottle` | `SFX` | `res://assets/audio/sfx/sfx_pill_bottle.mp3` | No | 0.0 dB | 0.3s | ✅ Exists |
| `action_repair` | `SFX` | `res://assets/audio/sfx/sfx_repair_wrench.mp3` | No | 0.0 dB | 0.5s | ✅ Exists |
| `action_trade` | `SFX` | `res://assets/audio/sfx/sfx_trade_exchange.mp3` | No | 0.0 dB | 0.5s | ✅ Exists |
| `action_water_pour` | `SFX` | `res://assets/audio/sfx/sfx_water_pour.mp3` | No | 0.0 dB | 0.5s | ✅ Exists |
| `bio_mutation_pulse` | `SFX` | `res://assets/audio/sfx/sfx_mutation_pulse.mp3` | No | -4.0 dB | 2.0s | ✅ Exists |
| `combat_casing_drop` | `SFX` | `res://assets/audio/sfx/sfx_shell_casing_drop_01.wav` | No | -8.0 dB | 0.08s | ✅ Exists |
| `combat_decon_flush` | `SFX` | `res://assets/audio/sfx/sfx_combat_decon_spray.wav` | No | -4.0 dB | 1.0s | ✅ Exists |
| `combat_downed` | `SFX` | `res://assets/audio/sfx/sfx_combat_downed.mp3` | No | -4.0 dB | 1.0s | ✅ Exists |
| `combat_dry_fire` | `SFX` | `res://assets/audio/sfx/sfx_weapon_dry_fire_click.wav` | No | -3.0 dB | 0.15s | ✅ Exists |
| `combat_fire` | `SFX` | `res://assets/audio/sfx/sfx_combat_gunshot.mp3` | No | -4.0 dB | 0.3s | ✅ Exists |
| `combat_hit` | `SFX` | `res://assets/audio/sfx/sfx_combat_hit.mp3` | No | -5.0 dB | 0.3s | ✅ Exists |
| `combat_impact_concrete` | `SFX` | `res://assets/audio/sfx/sfx_impact_concrete_crack.wav` | No | -2.0 dB | 0.1s | ✅ Exists |
| `combat_impact_metal` | `SFX` | `res://assets/audio/sfx/sfx_impact_metal_ricochet.wav` | No | -2.0 dB | 0.1s | ✅ Exists |
| `combat_impact_wood` | `SFX` | `res://assets/audio/sfx/sfx_impact_wood_splinter.wav` | No | -3.0 dB | 0.1s | ✅ Exists |
| `combat_improvised_spear` | `SFX` | `res://assets/audio/sfx/sfx_weapon_rebar_spear_thud.wav` | No | -2.0 dB | 0.3s | ✅ Exists |
| `combat_jam` | `SFX` | `res://assets/audio/sfx/sfx_combat_jam.mp3` | No | -6.0 dB | 1.0s | ✅ Exists |
| `combat_reload` | `SFX` | `res://assets/audio/sfx/sfx_combat_reload.mp3` | No | -6.0 dB | 0.5s | ✅ Exists |
| `combat_victory` | `SFX` | `res://assets/audio/sfx/sfx_combat_victory.mp3` | No | -6.0 dB | 5.0s | ✅ Exists |
| `danger_debris` | `SFX` | `res://assets/audio/sfx/sfx_debris_impact.mp3` | No | 0.0 dB | 3.0s | ✅ Exists |
| `danger_explosion` | `SFX` | `res://assets/audio/sfx/sfx_danger_explosion_01.wav` | No | 0.0 dB | 15.0s | ✅ Exists |
| `danger_glass_break` | `SFX` | `res://assets/audio/sfx/sfx_glass_break_small.mp3` | No | 0.0 dB | 1.0s | ✅ Exists |
| `day_transition` | `SFX` | `res://assets/audio/sfx/sfx_day_bell.mp3` | No | -8.0 dB | 2.0s | ✅ Exists |
| `expedition_vehicle_dirtbike` | `SFX` | `res://assets/audio/sfx/sfx_vehicle_engine_dirtbike.wav` | Yes | -10.0 dB | — | ✅ Exists |
| `expedition_vehicle_engine` | `SFX` | `res://assets/audio/sfx/sfx_vehicle_engine_diesel.wav` | Yes | -12.0 dB | — | ✅ Exists |
| `expedition_vehicle_refuel` | `SFX` | `res://assets/audio/sfx/sfx_vehicle_refuel.wav` | No | -4.0 dB | 0.5s | ✅ Exists |
| `expedition_vehicle_repair` | `SFX` | `res://assets/audio/sfx/sfx_vehicle_repair.wav` | No | -4.0 dB | 0.5s | ✅ Exists |
| `expedition_vehicle_truck` | `SFX` | `res://assets/audio/sfx/sfx_vehicle_engine_truck.wav` | Yes | -11.0 dB | — | ✅ Exists |
| `flashback_grounded` | `SFX` | `res://assets/audio/sfx/sfx_flashback_grounded.wav` | No | -4.0 dB | 2.0s | ✅ Exists |
| `footstep_dirt` | `SFX` | `res://assets/audio/sfx/sfx_footstep_dirt_01.wav` | No | -6.0 dB | 0.05s | ✅ Exists |
| `footstep_glass` | `SFX` | `res://assets/audio/sfx/sfx_footstep_glass_01.wav` | No | -5.0 dB | 0.05s | ✅ Exists |
| `footstep_granite` | `SFX` | `res://assets/audio/sfx/sfx_footstep_granite_01.wav` | No | -6.0 dB | 0.05s | ✅ Exists |
| `footstep_metal` | `SFX` | `res://assets/audio/sfx/sfx_footstep_metal_01.wav` | No | -6.0 dB | 0.05s | ✅ Exists |
| `footstep_wood` | `SFX` | `res://assets/audio/sfx/sfx_footstep_wood_01.wav` | No | -6.0 dB | 0.05s | ✅ Exists |
| `med_coughing` | `SFX` | `res://assets/audio/sfx/sfx_coughing_fit.mp3` | No | -4.0 dB | 8.0s | ✅ Exists |
| `med_heartbeat` | `SFX` | `res://assets/audio/sfx/sfx_heartbeat_slow.mp3` | No | -6.0 dB | 5.0s | ✅ Exists |
| `rad_geiger_burst` | `SFX` | `res://assets/audio/sfx/sfx_geiger_burst.mp3` | No | 0.0 dB | 2.0s | ✅ Exists |
| `rad_geiger_intense` | `SFX` | `res://assets/audio/sfx/sfx_geiger_intense_crackling.wav` | Yes | -8.0 dB | — | ✅ Exists |
| `rad_geiger_loop` | `SFX` | `res://assets/audio/sfx/geiger.wav` | Yes | -10.0 dB | — | ✅ Exists |
| `sfx_airlock_purge_cycle` | `SFX` | `res://assets/audio/sfx/sfx_airlock_purge_cycle.mp3` | No | -4.0 dB | 2.0s | ✅ Exists |
| `sfx_bullet_whiz_ricochet` | `SFX` | `res://assets/audio/sfx/sfx_bullet_whiz_ricochet.mp3` | No | -4.0 dB | 0.2s | ✅ Exists |
| `sfx_distant_artillery_barrage` | `SFX` | `res://assets/audio/sfx/sfx_distant_artillery_barrage_01.wav` | No | -4.0 dB | 5.0s | ✅ Exists |
| `sfx_distant_gunfire_skirmish` | `SFX` | `res://assets/audio/sfx/sfx_distant_gunfire_skirmish_01.wav` | No | -5.0 dB | 4.0s | ✅ Exists |
| `sfx_distant_mortar_launch` | `SFX` | `res://assets/audio/sfx/sfx_distant_mortar_launch.mp3` | No | -4.0 dB | 6.0s | ✅ Exists |
| `sfx_heavy_impact_fall` | `SFX` | `res://assets/audio/sfx/sfx_heavy_impact_fall.mp3` | No | -3.0 dB | 0.5s | ✅ Exists |
| `sfx_structural_collapse` | `SFX` | `res://assets/audio/sfx/sfx_structural_collapse.mp3` | No | -3.0 dB | 5.0s | ✅ Exists |
| `sfx_weapon_assault_rifle_burst` | `SFX` | `res://assets/audio/sfx/sfx_weapon_assault_rifle_burst_01.wav` | No | -3.0 dB | 0.25s | ✅ Exists |
| `sfx_weapon_bolt_rifle_report` | `SFX` | `res://assets/audio/sfx/sfx_weapon_bolt_rifle_report_01.wav` | No | -2.0 dB | 0.4s | ✅ Exists |
| `sfx_weapon_cz75_report` | `SFX` | `res://assets/audio/sfx/sfx_weapon_cz75_report_01.wav` | No | -3.0 dB | 0.15s | ✅ Exists |
| `sfx_weapon_lmg_burst` | `SFX` | `res://assets/audio/sfx/sfx_weapon_lmg_burst.mp3` | No | -2.0 dB | 0.3s | ✅ Exists |
| `sfx_weapon_pipe_rifle_report` | `SFX` | `res://assets/audio/sfx/sfx_weapon_pipe_rifle_report_01.wav` | No | -2.0 dB | 0.3s | ✅ Exists |
| `sfx_weapon_scrap_shotgun_report` | `SFX` | `res://assets/audio/sfx/sfx_weapon_scrap_shotgun_report_01.wav` | No | -1.0 dB | 0.3s | ✅ Exists |
| `sfx_weapon_shotgun_rack` | `SFX` | `res://assets/audio/sfx/sfx_weapon_shotgun_rack.mp3` | No | -4.0 dB | 0.3s | ✅ Exists |
| `sfx_weapon_sniper_heavy_report` | `SFX` | `res://assets/audio/sfx/sfx_weapon_sniper_heavy_report_01.wav` | No | -1.0 dB | 0.5s | ✅ Exists |
| `shelter_door_open` | `SFX` | `res://assets/audio/sfx/sfx_bunker_door_open.mp3` | No | 0.0 dB | 2.0s | ✅ Exists |
| `shelter_door_seal` | `SFX` | `res://assets/audio/sfx/sfx_bunker_door_seal.mp3` | No | 0.0 dB | 2.0s | ✅ Exists |
| `shelter_pipe_clang` | `SFX` | `res://assets/audio/sfx/sfx_pipe_clang.mp3` | No | -6.0 dB | 5.0s | ✅ Exists |
| `shelter_workshop_tools` | `SFX` | `res://assets/audio/sfx/sfx_workshop_lathe_hum.wav` | Yes | -16.0 dB | — | ✅ Exists |
| `trauma_heartbeat_rapid` | `SFX` | `res://assets/audio/sfx/sfx_trauma_heartbeat_rapid.wav` | Yes | -6.0 dB | — | ✅ Exists |
| `weather_blizzard` | `SFX` | `res://assets/audio/sfx/sfx_weather_blizzard.wav` | No | 0.0 dB | 10.0s | ✅ Exists |
| `weather_fallout_storm` | `SFX` | `res://assets/audio/sfx/sfx_fallout_storm_approach.mp3` | No | 0.0 dB | 10.0s | ✅ Exists |
| `weather_glass_storm` | `SFX` | `res://assets/audio/sfx/sfx_weather_glass_storm.wav` | No | -3.0 dB | 8.0s | ✅ Exists |
| `weather_wind_gust` | `SFX` | `res://assets/audio/sfx/sfx_wind_gust_harsh.mp3` | No | -8.0 dB | 3.0s | ✅ Exists |
| `amb_loc_geothermal_ruins` | `Surface` | `res://assets/audio/ambience/amb_loc_geothermal_ruins.mp3` | Yes | -5.0 dB | — | ✅ Exists |
| `amb_loc_rural_gas_station` | `Surface` | `res://assets/audio/ambience/amb_loc_rural_gas_station.mp3` | Yes | -5.0 dB | — | ✅ Exists |
| `amb_loc_suburban_ruins` | `Surface` | `res://assets/audio/ambience/amb_loc_suburban_ruins.mp3` | Yes | -6.0 dB | — | ✅ Exists |
| `amb_surface` | `Surface` | `res://assets/audio/ambience/surface_ambience.ogg` | Yes | -4.0 dB | — | ✅ Exists |
| `amb_surface_storm` | `Surface` | `res://assets/audio/ambience/amb_surface_storm.wav` | Yes | -7.0 dB | — | ✅ Exists |
| `amb_warzone_distant_shelling` | `Surface` | `res://assets/audio/ambience/amb_warzone_distant_shelling.mp3` | Yes | -6.0 dB | — | ✅ Exists |
| `item_handling_ammo` | `UI` | `res://assets/audio/sfx/sfx_item_handling_ammo_01.wav` | No | -4.0 dB | 0.1s | ✅ Exists |
| `item_handling_meds` | `UI` | `res://assets/audio/sfx/sfx_item_handling_meds_01.wav` | No | -4.0 dB | 0.1s | ✅ Exists |
| `item_handling_ration` | `UI` | `res://assets/audio/sfx/sfx_item_handling_ration_01.wav` | No | -4.0 dB | 0.1s | ✅ Exists |
| `log_tape_button` | `UI` | `res://assets/audio/sfx/sfx_tape_deck_button.wav` | No | -3.0 dB | 0.1s | ✅ Exists |
| `log_tape_insert` | `UI` | `res://assets/audio/sfx/sfx_tape_deck_insert.wav` | No | -3.0 dB | 0.3s | ✅ Exists |
| `log_tape_rewind` | `UI` | `res://assets/audio/sfx/sfx_tape_rewind.wav` | No | -4.0 dB | 0.2s | ✅ Exists |
| `log_tape_stop` | `UI` | `res://assets/audio/sfx/sfx_tape_stop.wav` | No | -3.0 dB | 0.1s | ✅ Exists |
| `save_success` | `UI` | `res://assets/audio/ui/ui_save_success.wav` | No | -4.0 dB | 1.0s | ✅ Exists |
| `ui_cancel` | `UI` | `res://assets/audio/ui/ui_cancel.wav` | No | -2.0 dB | 0.05s | ✅ Exists |
| `ui_click` | `UI` | `res://assets/audio/ui/ui_click.wav` | No | 0.0 dB | 0.05s | ✅ Exists |
| `ui_confirm` | `UI` | `res://assets/audio/ui/ui_confirm.wav` | No | 0.0 dB | 0.1s | ✅ Exists |
| `ui_crt_power_on` | `UI` | `res://assets/audio/ui/ui_crt_power_on.wav` | No | -4.0 dB | 1.0s | ✅ Exists |
| `ui_drawer_slide` | `UI` | `res://assets/audio/ui/ui_drawer_slide.wav` | No | -3.0 dB | 0.1s | ✅ Exists |
| `ui_invalid_action` | `UI` | `res://assets/audio/ui/ui_invalid_action.wav` | No | -2.0 dB | 0.5s | ✅ Exists |
| `ui_modal_close` | `UI` | `res://assets/audio/ui/ui_modal_close.wav` | No | -3.0 dB | 0.1s | ✅ Exists |
| `ui_modal_open` | `UI` | `res://assets/audio/ui/ui_modal_open.wav` | No | -3.0 dB | 0.1s | ✅ Exists |
| `ui_paper_rustle` | `UI` | `res://assets/audio/ui/ui_paper_rustle.wav` | No | -3.0 dB | 0.1s | ✅ Exists |
| `ui_rotary_click` | `UI` | `res://assets/audio/ui/ui_rotary_click.wav` | No | -2.0 dB | 0.05s | ✅ Exists |
| `ui_stamp_heavy` | `UI` | `res://assets/audio/ui/ui_stamp_heavy.wav` | No | -2.0 dB | 0.2s | ✅ Exists |
| `ui_switch_toggle` | `UI` | `res://assets/audio/ui/ui_switch_toggle.wav` | No | -2.0 dB | 0.05s | ✅ Exists |
| `ui_tab_change` | `UI` | `res://assets/audio/ui/ui_tab_change.wav` | No | -2.0 dB | 0.05s | ✅ Exists |
| `ui_warning` | `UI` | `res://assets/audio/ui/ui_warning.wav` | No | 0.0 dB | 0.3s | ✅ Exists |
| `shelter_air_recycler` | `Ventilation` | `res://assets/audio/sfx/sfx_air_recycler_hiss.wav` | Yes | -14.0 dB | — | ✅ Exists |
| `shelter_ventilation` | `Ventilation` | `res://assets/audio/sfx/sfx_ventilation_fan.mp3` | Yes | -12.0 dB | — | ✅ Exists |
| `radio_dead_hand_pulse` | `Voice` | `res://assets/audio/radio/radio_dead_hand_pulse.wav` | Yes | -6.0 dB | — | ✅ Exists |
| `radio_distress_beacon` | `Voice` | `res://assets/audio/radio/radio_distress_beacon.wav` | Yes | -6.0 dB | — | ✅ Exists |
| `radio_morse` | `Voice` | `res://assets/audio/sfx/sfx_morse_key.mp3` | No | 0.0 dB | 0.5s | ✅ Exists |
| `radio_numbers_station` | `Voice` | `res://assets/audio/radio/radio_numbers_station.wav` | Yes | -5.0 dB | — | ✅ Exists |
| `radio_signal_lock` | `Voice` | `res://assets/audio/sfx/sfx_radio_signal_lock.mp3` | No | 0.0 dB | 1.0s | ✅ Exists |
| `radio_static` | `Voice` | `res://assets/audio/radio/radio_static_hiss.wav` | No | -8.0 dB | 0.5s | ✅ Exists |
| `radio_tune` | `Voice` | `res://assets/audio/sfx/sfx_radio_tune.mp3` | No | 0.0 dB | 1.0s | ✅ Exists |
| `radio_vo_ch11_stockpile` | `Voice` | `res://assets/audio/radio/vo_ch11_stockpile.wav` | No | -2.3 dB | 2.0s | ✅ Exists |
| `radio_vo_ch3_ash_road` | `Voice` | `res://assets/audio/radio/vo_ch3_ash_road.wav` | No | -2.5 dB | 2.0s | ✅ Exists |
| `radio_vo_ch7_milband` | `Voice` | `res://assets/audio/radio/vo_ch7_milband.wav` | No | -4.7 dB | 2.0s | ✅ Exists |
| `radio_vo_kind_hatch` | `Voice` | `res://assets/audio/radio/vo_kind_hatch_relay.wav` | No | -6.0 dB | 2.0s | ✅ Exists |
| `radio_vo_kind_parley` | `Voice` | `res://assets/audio/radio/vo_kind_parley_beacon.wav` | No | -6.0 dB | 2.0s | ✅ Exists |
| `radio_vo_verdict_count` | `Voice` | `res://assets/audio/radio/vo_verdict_count.wav` | No | -6.0 dB | 2.0s | ✅ Exists |
| `radio_vo_verdict_eden` | `Voice` | `res://assets/audio/radio/vo_verdict_eden.wav` | No | -6.0 dB | 2.0s | ✅ Exists |
| `radio_vo_verdict_geophone` | `Voice` | `res://assets/audio/radio/vo_verdict_geophone.wav` | No | -6.0 dB | 2.0s | ✅ Exists |
| `radio_vo_verdict_meter` | `Voice` | `res://assets/audio/radio/vo_verdict_meter.wav` | No | -6.0 dB | 2.0s | ✅ Exists |
| `radio_vo_verdict_reckoning` | `Voice` | `res://assets/audio/radio/vo_verdict_reckoning.wav` | No | -6.0 dB | 2.0s | ✅ Exists |

---

## 3. Cue Playback Integration Protocol

```csharp
// Canonical playback in Godot Host views and presentation nodes:
AudioManager.Instance.PlayCue(AudioCueCatalog.UiClick);
AudioManager.Instance.PlayCue(AudioCueCatalog.RadGeigerBurst, pitchScale: 1.1f);
```
