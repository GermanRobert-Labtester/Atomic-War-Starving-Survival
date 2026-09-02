# ASHFALL Audio Cue Architecture Catalog

> **Living Architecture Authority**: Documents all registered audio cues, target Godot audio buses, asset resource paths, loop behavior, volume trim, and cooldown timers in `src/Audio/AudioCueCatalog.cs`.

**Total Registered Cues:** `97`<br>
**Last Verified:** `2026-09-02`<br>
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
| `combat_start` | `Alerts` | `res://assets/audio/sfx/sfx_combat_start.mp3` | No | -2 dB | 5s | ✅ Exists |
| `danger_alarm_klaxon` | `Alerts` | `res://assets/audio/sfx/sfx_danger_alarm_klaxon.wav` | No | 0 dB | 10s | ✅ Exists |
| `rad_alert_acute` | `Alerts` | `res://assets/audio/sfx/sfx_radiation_alarm.mp3` | No | -2 dB | 5s | ✅ Exists |
| `rad_alert_chronic` | `Alerts` | `res://assets/audio/sfx/sfx_radiation_chronic_alarm.wav` | No | -6 dB | 10s | ✅ Exists |
| `rad_contamination` | `Alerts` | `res://assets/audio/sfx/sfx_contamination_warning.mp3` | No | 0 dB | 5s | ✅ Exists |
| `sfx_artillery_incoming_whistle` | `Alerts` | `res://assets/audio/sfx/sfx_artillery_incoming_whistle.mp3` | No | -3 dB | 8s | ✅ Exists |
| `shelter_air_filter` | `Alerts` | `res://assets/audio/sfx/sfx_air_filter_degrade.mp3` | No | 0 dB | 10s | ✅ Exists |
| `weather_alert` | `Alerts` | `res://assets/audio/sfx/sfx_alarm_klaxon.mp3` | No | -2 dB | 5s | ✅ Exists |
| `weather_black_rain` | `Alerts` | `res://assets/audio/sfx/sfx_weather_black_rain.wav` | No | 0 dB | 10s | ✅ Exists |
| `weather_corrosive_precipitation` | `Alerts` | `res://assets/audio/sfx/sfx_weather_corrosive_precipitation.wav` | No | -4 dB | 8s | ✅ Exists |
| `weather_emp_storm` | `Alerts` | `res://assets/audio/sfx/sfx_weather_emp_storm.wav` | No | -4 dB | 8s | ✅ Exists |
| `amb_bunker` | `Ambience` | `res://assets/audio/ambience/bunker_ambience.ogg` | Yes | -3 dB | — | ✅ Exists |
| `amb_loc_abandoned_hospital` | `Ambience` | `res://assets/audio/ambience/amb_loc_abandoned_hospital.mp3` | Yes | -5 dB | — | ✅ Exists |
| `amb_loc_arcology_sector` | `Ambience` | `res://assets/audio/ambience/amb_loc_arcology_sector.mp3` | Yes | -5 dB | — | ✅ Exists |
| `amb_loc_military_bunker` | `Ambience` | `res://assets/audio/ambience/amb_loc_military_bunker.mp3` | Yes | -4 dB | — | ✅ Exists |
| `shelter_water_drip` | `Ambience` | `res://assets/audio/sfx/sfx_water_drip_cave.mp3` | Yes | -15 dB | — | ✅ Exists |
| `shelter_generator` | `Generator` | `res://assets/audio/sfx/sfx_generator_cough.mp3` | Yes | -16 dB | — | ✅ Exists |
| `med_quarantine_clear` | `Medical` | `res://assets/audio/sfx/sfx_med_quarantine_clear.wav` | No | -8 dB | 0.75s | ✅ Exists |
| `med_quarantine_seal` | `Medical` | `res://assets/audio/sfx/sfx_med_quarantine_seal.wav` | No | -7 dB | 1s | ✅ Exists |
| `med_survivor_death` | `Medical` | `res://assets/audio/sfx/sfx_survivor_death.wav` | No | -6 dB | 3s | ✅ Exists |
| `combat_defeat` | `Music` | `res://assets/audio/sfx/sfx_combat_defeat.mp3` | No | -8 dB | 5s | ✅ Exists |
| `game_over` | `Music` | `res://assets/audio/music/game_over.ogg` | No | -10 dB | — | ✅ Exists |
| `music_gameplay` | `Music` | `res://assets/audio/music/gameplay_underscore.ogg` | No | -8 dB | — | ✅ Exists |
| `music_menu` | `Music` | `res://assets/audio/music/main_menu.ogg` | No | -6 dB | — | ✅ Exists |
| `action_crafting` | `Sfx` | `res://assets/audio/sfx/sfx_crafting_assemble.mp3` | No | 0 dB | 1s | ✅ Exists |
| `action_injection` | `Sfx` | `res://assets/audio/sfx/sfx_injection.mp3` | No | 0 dB | 0.5s | ✅ Exists |
| `action_item_pickup` | `Sfx` | `res://assets/audio/sfx/sfx_item_pickup_metal.mp3` | No | -4 dB | 0.2s | ✅ Exists |
| `action_pill_bottle` | `Sfx` | `res://assets/audio/sfx/sfx_pill_bottle.mp3` | No | 0 dB | 0.3s | ✅ Exists |
| `action_repair` | `Sfx` | `res://assets/audio/sfx/sfx_repair_wrench.mp3` | No | 0 dB | 0.5s | ✅ Exists |
| `action_trade` | `Sfx` | `res://assets/audio/sfx/sfx_trade_exchange.mp3` | No | 0 dB | 0.5s | ✅ Exists |
| `action_water_pour` | `Sfx` | `res://assets/audio/sfx/sfx_water_pour.mp3` | No | 0 dB | 0.5s | ✅ Exists |
| `combat_downed` | `Sfx` | `res://assets/audio/sfx/sfx_combat_downed.mp3` | No | -4 dB | 1s | ✅ Exists |
| `combat_fire` | `Sfx` | `res://assets/audio/sfx/sfx_combat_gunshot.mp3` | No | -4 dB | 0.3s | ✅ Exists |
| `combat_hit` | `Sfx` | `res://assets/audio/sfx/sfx_combat_hit.mp3` | No | -5 dB | 0.3s | ✅ Exists |
| `combat_jam` | `Sfx` | `res://assets/audio/sfx/sfx_combat_jam.mp3` | No | -6 dB | 1s | ✅ Exists |
| `combat_reload` | `Sfx` | `res://assets/audio/sfx/sfx_combat_reload.mp3` | No | -6 dB | 0.5s | ✅ Exists |
| `combat_victory` | `Sfx` | `res://assets/audio/sfx/sfx_combat_victory.mp3` | No | -6 dB | 5s | ✅ Exists |
| `danger_debris` | `Sfx` | `res://assets/audio/sfx/sfx_debris_impact.mp3` | No | 0 dB | 3s | ✅ Exists |
| `danger_explosion` | `Sfx` | `res://assets/audio/sfx/sfx_distant_explosion.mp3` | No | 0 dB | 15s | ✅ Exists |
| `danger_glass_break` | `Sfx` | `res://assets/audio/sfx/sfx_glass_break_small.mp3` | No | 0 dB | 1s | ✅ Exists |
| `day_transition` | `Sfx` | `res://assets/audio/sfx/sfx_day_bell.mp3` | No | -8 dB | 2s | ✅ Exists |
| `med_coughing` | `Sfx` | `res://assets/audio/sfx/sfx_coughing_fit.mp3` | No | -4 dB | 8s | ✅ Exists |
| `med_heartbeat` | `Sfx` | `res://assets/audio/sfx/sfx_heartbeat_slow.mp3` | No | -6 dB | 5s | ✅ Exists |
| `rad_geiger_burst` | `Sfx` | `res://assets/audio/sfx/sfx_geiger_burst.mp3` | No | 0 dB | 2s | ✅ Exists |
| `rad_geiger_loop` | `Sfx` | `res://assets/audio/sfx/geiger.wav` | Yes | -10 dB | — | ✅ Exists |
| `sfx_airlock_purge_cycle` | `Sfx` | `res://assets/audio/sfx/sfx_airlock_purge_cycle.mp3` | No | -4 dB | 2s | ✅ Exists |
| `sfx_bullet_whiz_ricochet` | `Sfx` | `res://assets/audio/sfx/sfx_bullet_whiz_ricochet.mp3` | No | -4 dB | 0.2s | ✅ Exists |
| `sfx_distant_artillery_barrage` | `Sfx` | `res://assets/audio/sfx/sfx_distant_artillery_barrage.mp3` | No | -4 dB | 5s | ✅ Exists |
| `sfx_distant_gunfire_skirmish` | `Sfx` | `res://assets/audio/sfx/sfx_distant_gunfire_skirmish.mp3` | No | -5 dB | 4s | ✅ Exists |
| `sfx_distant_mortar_launch` | `Sfx` | `res://assets/audio/sfx/sfx_distant_mortar_launch.mp3` | No | -4 dB | 6s | ✅ Exists |
| `sfx_heavy_impact_fall` | `Sfx` | `res://assets/audio/sfx/sfx_heavy_impact_fall.mp3` | No | -3 dB | 0.5s | ✅ Exists |
| `sfx_structural_collapse` | `Sfx` | `res://assets/audio/sfx/sfx_structural_collapse.mp3` | No | -3 dB | 5s | ✅ Exists |
| `sfx_weapon_assault_rifle_burst` | `Sfx` | `res://assets/audio/sfx/sfx_weapon_assault_rifle_burst.mp3` | No | -3 dB | 0.25s | ✅ Exists |
| `sfx_weapon_bolt_rifle_report` | `Sfx` | `res://assets/audio/sfx/sfx_weapon_bolt_rifle_report.mp3` | No | -2 dB | 0.4s | ✅ Exists |
| `sfx_weapon_cz75_report` | `Sfx` | `res://assets/audio/sfx/sfx_weapon_cz75_report.mp3` | No | -3 dB | 0.15s | ✅ Exists |
| `sfx_weapon_lmg_burst` | `Sfx` | `res://assets/audio/sfx/sfx_weapon_lmg_burst.mp3` | No | -2 dB | 0.3s | ✅ Exists |
| `sfx_weapon_pipe_rifle_report` | `Sfx` | `res://assets/audio/sfx/sfx_weapon_pipe_rifle_report.mp3` | No | -2 dB | 0.3s | ✅ Exists |
| `sfx_weapon_scrap_shotgun_report` | `Sfx` | `res://assets/audio/sfx/sfx_weapon_scrap_shotgun_report.mp3` | No | -1 dB | 0.3s | ✅ Exists |
| `sfx_weapon_shotgun_rack` | `Sfx` | `res://assets/audio/sfx/sfx_weapon_shotgun_rack.mp3` | No | -4 dB | 0.3s | ✅ Exists |
| `sfx_weapon_sniper_heavy_report` | `Sfx` | `res://assets/audio/sfx/sfx_weapon_sniper_heavy_report.mp3` | No | -1 dB | 0.5s | ✅ Exists |
| `shelter_door_open` | `Sfx` | `res://assets/audio/sfx/sfx_bunker_door_open.mp3` | No | 0 dB | 2s | ✅ Exists |
| `shelter_door_seal` | `Sfx` | `res://assets/audio/sfx/sfx_bunker_door_seal.mp3` | No | 0 dB | 2s | ✅ Exists |
| `shelter_pipe_clang` | `Sfx` | `res://assets/audio/sfx/sfx_pipe_clang.mp3` | No | -6 dB | 5s | ✅ Exists |
| `weather_blizzard` | `Sfx` | `res://assets/audio/sfx/sfx_weather_blizzard.wav` | No | 0 dB | 10s | ✅ Exists |
| `weather_fallout_storm` | `Sfx` | `res://assets/audio/sfx/sfx_fallout_storm_approach.mp3` | No | 0 dB | 10s | ✅ Exists |
| `weather_glass_storm` | `Sfx` | `res://assets/audio/sfx/sfx_weather_glass_storm.wav` | No | -3 dB | 8s | ✅ Exists |
| `weather_wind_gust` | `Sfx` | `res://assets/audio/sfx/sfx_wind_gust_harsh.mp3` | No | -8 dB | 3s | ✅ Exists |
| `amb_loc_geothermal_ruins` | `Surface` | `res://assets/audio/ambience/amb_loc_geothermal_ruins.mp3` | Yes | -5 dB | — | ✅ Exists |
| `amb_loc_rural_gas_station` | `Surface` | `res://assets/audio/ambience/amb_loc_rural_gas_station.mp3` | Yes | -5 dB | — | ✅ Exists |
| `amb_loc_suburban_ruins` | `Surface` | `res://assets/audio/ambience/amb_loc_suburban_ruins.mp3` | Yes | -6 dB | — | ✅ Exists |
| `amb_surface` | `Surface` | `res://assets/audio/ambience/surface_ambience.ogg` | Yes | -4 dB | — | ✅ Exists |
| `amb_surface_storm` | `Surface` | `res://assets/audio/ambience/amb_surface_storm.wav` | Yes | -7 dB | — | ✅ Exists |
| `amb_warzone_distant_shelling` | `Surface` | `res://assets/audio/ambience/amb_warzone_distant_shelling.mp3` | Yes | -6 dB | — | ✅ Exists |
| `save_success` | `Ui` | `res://assets/audio/ui/ui_confirm.wav` | No | -10 dB | 1s | ✅ Exists |
| `ui_cancel` | `Ui` | `res://assets/audio/ui/ui_click.wav` | No | 0 dB | 0.05s | ✅ Exists |
| `ui_click` | `Ui` | `res://assets/audio/ui/ui_click.wav` | No | 0 dB | 0.05s | ✅ Exists |
| `ui_confirm` | `Ui` | `res://assets/audio/ui/ui_confirm.wav` | No | 0 dB | 0.1s | ✅ Exists |
| `ui_invalid_action` | `Ui` | `res://assets/audio/ui/ui_warning.wav` | No | -6 dB | 0.5s | ✅ Exists |
| `ui_modal_close` | `Ui` | `res://assets/audio/ui/ui_click.wav` | No | 0 dB | — | ✅ Exists |
| `ui_modal_open` | `Ui` | `res://assets/audio/ui/ui_confirm.wav` | No | -3 dB | — | ✅ Exists |
| `ui_tab_change` | `Ui` | `res://assets/audio/ui/ui_click.wav` | No | 0 dB | 0.05s | ✅ Exists |
| `ui_warning` | `Ui` | `res://assets/audio/ui/ui_warning.wav` | No | 0 dB | 0.3s | ✅ Exists |
| `shelter_ventilation` | `Ventilation` | `res://assets/audio/sfx/sfx_ventilation_fan.mp3` | Yes | -12 dB | — | ✅ Exists |
| `radio_morse` | `Voice` | `res://assets/audio/sfx/sfx_morse_key.mp3` | No | 0 dB | 0.5s | ✅ Exists |
| `radio_signal_lock` | `Voice` | `res://assets/audio/sfx/sfx_radio_signal_lock.mp3` | No | 0 dB | 1s | ✅ Exists |
| `radio_static` | `Voice` | `res://assets/audio/radio/radio_static_hiss.wav` | No | -8 dB | 0.5s | ✅ Exists |
| `radio_tune` | `Voice` | `res://assets/audio/sfx/sfx_radio_tune.mp3` | No | 0 dB | 1s | ✅ Exists |
| `radio_vo_ch11_stockpile` | `Voice` | `res://assets/audio/radio/vo_ch11_stockpile.wav` | No | -2.3 dB | 2s | ✅ Exists |
| `radio_vo_ch3_ash_road` | `Voice` | `res://assets/audio/radio/vo_ch3_ash_road.wav` | No | -2.5 dB | 2s | ✅ Exists |
| `radio_vo_ch7_milband` | `Voice` | `res://assets/audio/radio/vo_ch7_milband.wav` | No | -4.7 dB | 2s | ✅ Exists |
| `radio_vo_kind_hatch` | `Voice` | `res://assets/audio/radio/vo_kind_hatch_relay.wav` | No | -6 dB | 2s | ✅ Exists |
| `radio_vo_kind_parley` | `Voice` | `res://assets/audio/radio/vo_kind_parley_beacon.wav` | No | -6 dB | 2s | ✅ Exists |
| `radio_vo_verdict_count` | `Voice` | `res://assets/audio/radio/vo_verdict_count.wav` | No | -6 dB | 2s | ✅ Exists |
| `radio_vo_verdict_eden` | `Voice` | `res://assets/audio/radio/vo_verdict_eden.wav` | No | -6 dB | 2s | ✅ Exists |
| `radio_vo_verdict_geophone` | `Voice` | `res://assets/audio/radio/vo_verdict_geophone.wav` | No | -6 dB | 2s | ✅ Exists |
| `radio_vo_verdict_meter` | `Voice` | `res://assets/audio/radio/vo_verdict_meter.wav` | No | -6 dB | 2s | ✅ Exists |
| `radio_vo_verdict_reckoning` | `Voice` | `res://assets/audio/radio/vo_verdict_reckoning.wav` | No | -6 dB | 2s | ✅ Exists |

---

## 3. Cue Playback Integration Protocol

```csharp
// Canonical playback in Godot Host views and presentation nodes:
AudioManager.Instance.PlayCue(AudioCueCatalog.UiClick);
AudioManager.Instance.PlayCue(AudioCueCatalog.RadGeigerBurst, pitchScale: 1.1f);
```
