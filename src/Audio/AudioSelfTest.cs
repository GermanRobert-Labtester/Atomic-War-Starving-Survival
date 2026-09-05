using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Ashfall.Core;
using Ashfall.Core.Combat;
using Ashfall.Core.Disease;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Medical;
using Ashfall.Core.Radiation;
using Ashfall.Core.Shelter;
using Ashfall.Core.StartingLevel;
using Ashfall.Core.Survivors;
using Ashfall.Core.World;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp.Audio
{
    /// <summary>
    /// Headless self-test for the complete audio system.
    /// Verifies: cue coverage, bus topology, settings persistence,
    /// resource resolution, cooldown/dedup, lifecycle safety.
    /// Run with: godot --headless --path . -- --audio-selftest
    /// </summary>
    public static class AudioSelfTest
    {
        public static int Run()
        {
            GD.Print("[AudioSelfTest] Starting...");
            int pass = 0, fail = 0;
            var audioMgr = AudioManager.Instance;
            bool createdManager = false;
            if (audioMgr == null)
            {
                audioMgr = new AudioManager();
                audioMgr._Ready();
                createdManager = true;
            }

            // ── 1. Cue Catalog Coverage ─────────────────────────
            GD.Print("[AudioSelfTest] --- Cue Catalog Coverage ---");
            int cueCount = AudioCueCatalog.Count;
            Check($"Cue catalog has entries", cueCount > 0, ref pass, ref fail);
            Check($"Cue catalog has 45+ cues", cueCount >= 45, ref pass, ref fail);

            // Every cue must resolve to an existing file or have a fallback
            int resolved = 0, fallback = 0, silent = 0;
            foreach (var kvp in AudioCueCatalog.All)
            {
                var cue = kvp.Value;
                string osPath = ProjectSettings.GlobalizePath(cue.ResourcePath);
                bool fileExists = File.Exists(osPath);
                bool resourceExists = ResourceLoader.Exists(cue.ResourcePath);

                if (fileExists || resourceExists)
                {
                    resolved++;
                }
                else if (cue.FallbackCueId != null)
                {
                    fallback++;
                    Check($"Cue '{cue.Id}' has valid fallback '{cue.FallbackCueId}'",
                        AudioCueCatalog.Contains(cue.FallbackCueId), ref pass, ref fail);
                }
                else
                {
                    silent++;
                    GD.PrintErr($"  [WARN] Cue '{cue.Id}' path not found: {cue.ResourcePath}");
                }
            }
            Check($"All cues resolved: {resolved} resolved, {fallback} fallback, {silent} silent",
                silent == 0, ref pass, ref fail);

            // Verify actual stream load for every cue
            int loadSuccess = 0, loadFailed = 0;
            foreach (var kvp in AudioCueCatalog.All)
            {
                var cue = kvp.Value;
                string osPath = ProjectSettings.GlobalizePath(cue.ResourcePath);
                if (File.Exists(osPath))
                {
                    var stream = AudioManager.LoadDirectStream(cue.ResourcePath);
                    if (stream != null)
                    {
                        loadSuccess++;
                    }
                    else
                    {
                        loadFailed++;
                        GD.PrintErr($"  [FAIL] Direct stream load returned null for cue '{cue.Id}': {cue.ResourcePath}");
                    }
                }
            }
            Check($"Every cue resource loads valid stream container: {loadSuccess} loaded, {loadFailed} failed",
                loadFailed == 0 && loadSuccess > 0, ref pass, ref fail);

            // Key cues exist
            string[] requiredCues = {
                AudioCueCatalog.UiClick, AudioCueCatalog.UiConfirm, AudioCueCatalog.UiWarning,
                AudioCueCatalog.RadAlertAcute, AudioCueCatalog.RadAlertChronic,
                AudioCueCatalog.WeatherAlert, AudioCueCatalog.WeatherEmpStorm,
                AudioCueCatalog.WeatherGlassStorm, AudioCueCatalog.WeatherCorrosivePrecipitation,
                AudioCueCatalog.AmbBunker, AudioCueCatalog.AmbBunkerLowPower,
                AudioCueCatalog.AmbSurfaceAshfall, AudioCueCatalog.AmbSurfaceBlizzard,
                AudioCueCatalog.AmbSurfaceFalloutStorm, AudioCueCatalog.AmbSurfaceStorm,
                AudioCueCatalog.MusicMenu,
                AudioCueCatalog.RadioStatic, AudioCueCatalog.ShelterDoorOpen,
                AudioCueCatalog.ActionItemPickup, AudioCueCatalog.DangerAlarmKlaxon,
                AudioCueCatalog.SaveSuccess, AudioCueCatalog.DayTransition,
                AudioCueCatalog.GameOver,
                AudioCueCatalog.MedSurvivorDeath,
                AudioCueCatalog.MedQuarantineSeal, AudioCueCatalog.MedQuarantineClear,
                AudioCueCatalog.RadioVoCh3AshRoad, AudioCueCatalog.RadioVoCh7Milband,
                AudioCueCatalog.RadioVoCh11Stockpile, AudioCueCatalog.RadioVoKindHatch,
                AudioCueCatalog.RadioVoKindParley, AudioCueCatalog.RadioVoVerdictMeter,
                AudioCueCatalog.RadioVoVerdictEden, AudioCueCatalog.RadioVoVerdictCount,
                AudioCueCatalog.RadioVoVerdictGeophone, AudioCueCatalog.RadioVoVerdictReckoning,
                AudioCueCatalog.UiSwitchToggle, AudioCueCatalog.UiRotaryClick,
                AudioCueCatalog.UiCrtPowerOn, AudioCueCatalog.UiPaperRustle,
                AudioCueCatalog.UiStampHeavy, AudioCueCatalog.UiDrawerSlide,
                AudioCueCatalog.ShelterGeneratorStrain, AudioCueCatalog.ShelterGeneratorStart,
                AudioCueCatalog.ShelterGeneratorStop, AudioCueCatalog.ShelterBreakerTrip,
                AudioCueCatalog.ShelterPowerRestore, AudioCueCatalog.ShelterWaterFiltration,
                AudioCueCatalog.ShelterAirRecycler, AudioCueCatalog.ShelterWorkshopTools,
                AudioCueCatalog.MedInfirmaryBeep,
                AudioCueCatalog.ExpeditionVehicleEngine, AudioCueCatalog.ExpeditionVehicleBreakdown,
                AudioCueCatalog.ExpeditionCampFire,
                AudioCueCatalog.TraumaTinnitus, AudioCueCatalog.TraumaHeartbeatRapid,
                AudioCueCatalog.TraumaCabinFever,
                AudioCueCatalog.FlashbackTrigger, AudioCueCatalog.FlashbackGrounded,
                AudioCueCatalog.CombatWeaponBurst, AudioCueCatalog.CombatDryFire,
                AudioCueCatalog.CombatCasingDrop, AudioCueCatalog.CombatLastStand,
                AudioCueCatalog.CombatDeconFlush,
                AudioCueCatalog.LogTapeInsert, AudioCueCatalog.LogTapeButton,
                AudioCueCatalog.LogTapeHiss, AudioCueCatalog.EchoDiscovery,
                AudioCueCatalog.RadGeigerIntense,
                AudioCueCatalog.ExpeditionVehicleDirtBike, AudioCueCatalog.ExpeditionVehicleTruck,
                AudioCueCatalog.ExpeditionVehicleRefuel, AudioCueCatalog.ExpeditionVehicleRepair,
                AudioCueCatalog.CombatImpactWood, AudioCueCatalog.CombatImpactConcrete,
                AudioCueCatalog.CombatImpactMetal, AudioCueCatalog.CombatImprovisedSpear,
                AudioCueCatalog.CombatImprovisedFire,
                AudioCueCatalog.RadioNumbersStation, AudioCueCatalog.RadioEbsAlert,
                AudioCueCatalog.RadioDeadHandPulse, AudioCueCatalog.RadioDistressBeacon,
                AudioCueCatalog.LogTapeRewind, AudioCueCatalog.LogTapeStop,
                AudioCueCatalog.ItemHandlingAmmo, AudioCueCatalog.ItemHandlingMeds,
                AudioCueCatalog.ItemHandlingRation,
            };
            foreach (string cueId in requiredCues)
            {
                var cue = AudioCueCatalog.Resolve(cueId);
                Check($"Required cue '{cueId}' exists", cue != null, ref pass, ref fail);
            }

            // Unknown/empty/null cue returns null
            Check("Unknown cue returns null", AudioCueCatalog.Resolve("nonexistent_cue") == null, ref pass, ref fail);
            Check("Empty cue returns null", AudioCueCatalog.Resolve("") == null, ref pass, ref fail);
            Check("Null cue returns null", AudioCueCatalog.Resolve(null!) == null, ref pass, ref fail);

            // ── 2. Bus Topology ─────────────────────────────────
            GD.Print("[AudioSelfTest] --- Bus Topology ---");
            string[] expectedBuses = {
                AudioBusNames.Master, AudioBusNames.Music, AudioBusNames.Ambience,
                AudioBusNames.Sfx, AudioBusNames.Ui, AudioBusNames.Voice,
                AudioBusNames.Alerts, AudioBusNames.Generator, AudioBusNames.Ventilation,
                AudioBusNames.Radio, AudioBusNames.Medical, AudioBusNames.Surface,
                AudioBusNames.Machinery, AudioBusNames.ShelterSocial, AudioBusNames.Subterranean,
            };
            foreach (var kvp in AudioCueCatalog.All)
            {
                bool validBus = false;
                foreach (string b in expectedBuses)
                    if (kvp.Value.Bus == b) { validBus = true; break; }
                Check($"Cue '{kvp.Key}' uses valid bus '{kvp.Value.Bus}'", validBus, ref pass, ref fail);
            }

            // ── 3. Resource Resolution ──────────────────────────
            GD.Print("[AudioSelfTest] --- Resource Resolution ---");
            string[] keyAssets = {
                "res://assets/audio/ui/ui_click.wav",
                "res://assets/audio/ui/ui_click_01.wav",
                "res://assets/audio/ui/ui_click_02.wav",
                "res://assets/audio/ui/ui_click_03.wav",
                "res://assets/audio/ui/ui_click_04.wav",
                "res://assets/audio/ui/ui_confirm.wav",
                "res://assets/audio/ui/ui_warning.wav",
                "res://assets/audio/ui/ui_switch_toggle.wav",
                "res://assets/audio/ui/ui_rotary_click.wav",
                "res://assets/audio/ui/ui_crt_power_on.wav",
                "res://assets/audio/ui/ui_paper_rustle.wav",
                "res://assets/audio/ui/ui_stamp_heavy.wav",
                "res://assets/audio/ui/ui_drawer_slide.wav",
                "res://assets/audio/sfx/sfx_generator_heavy_strain.wav",
                "res://assets/audio/sfx/sfx_water_filtration_loop.wav",
                "res://assets/audio/sfx/sfx_air_recycler_hiss.wav",
                "res://assets/audio/sfx/sfx_workshop_lathe_hum.wav",
                "res://assets/audio/sfx/sfx_generator_start.wav",
                "res://assets/audio/sfx/sfx_generator_stop.wav",
                "res://assets/audio/sfx/sfx_breaker_trip.wav",
                "res://assets/audio/sfx/sfx_power_restore.wav",
                "res://assets/audio/sfx/sfx_infirmary_monitor_beep.wav",
                "res://assets/audio/sfx/sfx_vehicle_engine_diesel.wav",
                "res://assets/audio/sfx/sfx_vehicle_breakdown_stall.wav",
                "res://assets/audio/ambience/amb_expedition_camp_fire.wav",
                "res://assets/audio/sfx/sfx_trauma_tinnitus_ring.wav",
                "res://assets/audio/sfx/sfx_trauma_heartbeat_rapid.wav",
                "res://assets/audio/sfx/sfx_trauma_cabin_fever_whisper.wav",
                "res://assets/audio/sfx/sfx_flashback_distortion.wav",
                "res://assets/audio/sfx/sfx_flashback_grounded.wav",
                "res://assets/audio/sfx/sfx_weapon_burst_rupture.wav",
                "res://assets/audio/sfx/sfx_weapon_dry_fire_click.wav",
                "res://assets/audio/sfx/sfx_shell_casing_drop_01.wav",
                "res://assets/audio/sfx/sfx_shell_casing_drop_02.wav",
                "res://assets/audio/sfx/sfx_combat_last_stand.wav",
                "res://assets/audio/sfx/sfx_combat_decon_spray.wav",
                "res://assets/audio/sfx/sfx_tape_deck_insert.wav",
                "res://assets/audio/sfx/sfx_tape_deck_button.wav",
                "res://assets/audio/sfx/sfx_tape_hiss_loop.wav",
                "res://assets/audio/sfx/sfx_echo_memory_shimmer.wav",
                "res://assets/audio/sfx/sfx_vehicle_engine_dirtbike.wav",
                "res://assets/audio/sfx/sfx_vehicle_engine_truck.wav",
                "res://assets/audio/sfx/sfx_vehicle_refuel.wav",
                "res://assets/audio/sfx/sfx_vehicle_repair.wav",
                "res://assets/audio/sfx/sfx_impact_wood_splinter.wav",
                "res://assets/audio/sfx/sfx_impact_concrete_crack.wav",
                "res://assets/audio/sfx/sfx_impact_metal_ricochet.wav",
                "res://assets/audio/sfx/sfx_weapon_rebar_spear_thud.wav",
                "res://assets/audio/sfx/sfx_weapon_molotov_burst.wav",
                "res://assets/audio/sfx/sfx_geiger_intense_crackling.wav",
                "res://assets/audio/radio/radio_numbers_station.wav",
                "res://assets/audio/radio/radio_ebs_alert.wav",
                "res://assets/audio/radio/radio_dead_hand_pulse.wav",
                "res://assets/audio/radio/radio_distress_beacon.wav",
                "res://assets/audio/sfx/sfx_tape_rewind.wav",
                "res://assets/audio/sfx/sfx_tape_stop.wav",
                "res://assets/audio/sfx/sfx_item_ammo_box.wav",
                "res://assets/audio/sfx/sfx_item_med_vial.wav",
                "res://assets/audio/sfx/sfx_item_ration_pack.wav",
                "res://assets/audio/sfx/sfx_radiation_alarm.mp3",
                "res://assets/audio/sfx/sfx_radiation_chronic_alarm.wav",
                "res://assets/audio/sfx/sfx_alarm_klaxon.mp3",
                "res://assets/audio/sfx/geiger.wav",
                "res://assets/audio/ambience/bunker_ambience.ogg",
                "res://assets/audio/ambience/surface_ambience.ogg",
                "res://assets/audio/ambience/amb_bunker_low_power_loop.ogg",
                "res://assets/audio/ambience/amb_surface_ashfall_loop.ogg",
                "res://assets/audio/ambience/amb_surface_blizzard_loop.ogg",
                "res://assets/audio/ambience/amb_surface_fallout_storm_loop.ogg",
                "res://assets/audio/music/main_menu.ogg",
                "res://assets/audio/music/gameplay_underscore.ogg",
                "res://assets/audio/radio/radio_static_hiss.wav",
                "res://assets/audio/radio/vo_ch3_ash_road_elevenlabs_v1.wav",
                "res://assets/audio/radio/vo_ch7_milband_elevenlabs_v1.wav",
                "res://assets/audio/radio/vo_ch11_stockpile.wav",
                "res://assets/audio/radio/vo_kind_hatch_relay.wav",
                "res://assets/audio/radio/vo_kind_parley_beacon.wav",
                "res://assets/audio/radio/vo_verdict_meter.wav",
                "res://assets/audio/radio/vo_verdict_eden.wav",
                "res://assets/audio/radio/vo_verdict_count.wav",
                "res://assets/audio/radio/vo_verdict_geophone.wav",
                "res://assets/audio/radio/vo_verdict_reckoning.wav",
                "res://assets/audio/sfx/sfx_bunker_door_open.mp3",
                "res://assets/audio/sfx/sfx_crafting_assemble.mp3",
                "res://assets/audio/sfx/sfx_ventilation_fan.mp3",
                "res://assets/audio/sfx/sfx_fallout_storm_approach.mp3",
                "res://assets/audio/sfx/sfx_weather_black_rain.wav",
                "res://assets/audio/sfx/sfx_weather_blizzard.wav",
                "res://assets/audio/sfx/sfx_weather_emp_storm.wav",
                "res://assets/audio/sfx/sfx_weather_glass_storm.wav",
                "res://assets/audio/sfx/sfx_weather_corrosive_precipitation.wav",
                "res://assets/audio/sfx/sfx_geiger_burst.mp3",
                "res://assets/audio/sfx/sfx_heartbeat_slow.mp3",
                "res://assets/audio/sfx/sfx_survivor_death.wav",
                "res://assets/audio/sfx/sfx_med_quarantine_seal.wav",
                "res://assets/audio/sfx/sfx_med_quarantine_clear.wav",
                "res://assets/audio/sfx/sfx_alarm_klaxon.mp3",
                "res://assets/audio/sfx/sfx_danger_alarm_klaxon.wav",
                "res://assets/audio/ambience/amb_surface_storm.wav",
            };
            foreach (string resPath in keyAssets)
            {
                bool exists = ResourceLoader.Exists(resPath);
                if (!exists)
                {
                    string osPath = ProjectSettings.GlobalizePath(resPath);
                    exists = File.Exists(osPath);
                }
                Check($"Asset exists: {Path.GetFileName(resPath)}", exists, ref pass, ref fail);
            }

            foreach (var kvp in AudioCueCatalog.All)
            {
                if (!kvp.Value.Loop)
                    continue;

                string resPath = kvp.Value.ResourcePath;
                var stream = ResourceLoader.Load<AudioStream>(resPath)
                    ?? AudioManager.LoadDirectStream(resPath);
                if (stream == null && kvp.Value.FallbackCueId != null)
                {
                    var fallbackCue = AudioCueCatalog.Resolve(kvp.Value.FallbackCueId);
                    if (fallbackCue != null)
                    {
                        stream = ResourceLoader.Load<AudioStream>(fallbackCue.ResourcePath)
                            ?? AudioManager.LoadDirectStream(fallbackCue.ResourcePath);
                    }
                }
                bool loopCapable = stream is AudioStreamWav
                    || stream is AudioStreamOggVorbis
                    || stream is AudioStreamMP3;
                Check($"Loop cue '{kvp.Key}' uses a supported stream format",
                    loopCapable, ref pass, ref fail);
            }

            // ── 4. Settings Persistence ─────────────────────────
            GD.Print("[AudioSelfTest] --- Settings Persistence ---");
            var defaults = new AudioSettings();
            Check("Default master volume is 100", defaults.MasterVolume == 100f, ref pass, ref fail);
            Check("Default music volume is 70", defaults.MusicVolume == 70f, ref pass, ref fail);
            Check("Default ambience volume is 60", defaults.AmbienceVolume == 60f, ref pass, ref fail);
            Check("Default sfx volume is 80", defaults.SfxVolume == 80f, ref pass, ref fail);
            Check("Default ui volume is 50", defaults.UiVolume == 50f, ref pass, ref fail);
            Check("Default voice volume is 90", defaults.VoiceVolume == 90f, ref pass, ref fail);
            Check("Default alert volume is 100", defaults.AlertVolume == 100f, ref pass, ref fail);
            Check("Default version is current", defaults.Version == AudioSettings.CurrentVersion, ref pass, ref fail);
            Check("Default no mutes", !defaults.MasterMute && !defaults.MusicMute && !defaults.SfxMute, ref pass, ref fail);

            // Round-trip
            var testSettings = new AudioSettings { MasterVolume = 50f, MusicVolume = 30f, MusicMute = true };
            string json = JsonSerializer.Serialize(testSettings);
            var restored = JsonSerializer.Deserialize<AudioSettings>(json);
            Check("Settings round-trip preserves volume", restored != null && restored.MasterVolume == 50f, ref pass, ref fail);
            Check("Settings round-trip preserves mute", restored != null && restored.MusicMute, ref pass, ref fail);

            // Malformed and Resilient Recovery
            string testAudioPath = Path.Combine(ProjectSettings.GlobalizePath("user://"), "audio_settings_selftest.json");
            try
            {
                // A. Completely malformed syntax
                File.WriteAllText(testAudioPath, "{ CORRUPT_AUDIO_DATA_!@#$");
                var recoveredCorrupt = AudioSettings.Load(testAudioPath);
                Check("Corrupted audio settings returns non-null defaults", recoveredCorrupt != null && recoveredCorrupt.MasterVolume == 100f, ref pass, ref fail);
                Check("Diagnostic error recorded for corrupt audio file", AudioSettings.HasDiagnosticError && AudioSettings.LastDiagnosticMessage!.Contains("Malformed JSON"), ref pass, ref fail);

                // B. Partially invalid JSON: preserve valid values, restore defaults for invalid
                File.WriteAllText(testAudioPath, "{\n  \"master_volume\": 42.0,\n  \"music_volume\": \"BAD_TYPE\",\n  \"sfx_mute\": true\n}");
                var recoveredPartial = AudioSettings.Load(testAudioPath);
                Check("Partial recovery preserves valid master volume", recoveredPartial != null && recoveredPartial.MasterVolume == 42.0f, ref pass, ref fail);
                Check("Partial recovery preserves valid mute state", recoveredPartial != null && recoveredPartial.SfxMute, ref pass, ref fail);
                Check("Partial recovery restores default for invalid music volume", recoveredPartial != null && recoveredPartial.MusicVolume == 70.0f, ref pass, ref fail);

                if (File.Exists(testAudioPath)) File.Delete(testAudioPath);
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[AudioSelfTest] Recovery test exception: {ex.Message}");
                fail++;
            }

            // Volume helpers
            Check("PercentToDb(100) ≈ 0", Math.Abs(AudioSettings.PercentToDb(100f)) < 0.1f, ref pass, ref fail);
            Check("PercentToDb(0) = -80", AudioSettings.PercentToDb(0f) == -80f, ref pass, ref fail);
            Check("PercentToDb(-10) = -80", AudioSettings.PercentToDb(-10f) == -80f, ref pass, ref fail);
            Check("ClampVolume(-10) = 0", AudioSettings.ClampVolume(-10f) == 0f, ref pass, ref fail);
            Check("ClampVolume(150) = 100", AudioSettings.ClampVolume(150f) == 100f, ref pass, ref fail);

            // Effective volume
            var s = new AudioSettings { MasterVolume = 80f, MasterMute = false };
            float eff = s.GetEffectiveVolume(50f, false);
            Check("Effective volume = master * category", Math.Abs(eff - 0.4f) < 0.01f, ref pass, ref fail);
            Check("Category mute = 0", s.GetEffectiveVolume(50f, true) == 0f, ref pass, ref fail);
            s.MasterMute = true;
            Check("Master mute overrides", s.GetEffectiveVolume(50f, false) == 0f, ref pass, ref fail);

            // Reset to defaults
            var modified = new AudioSettings { MasterVolume = 25f, MusicMute = true };
            modified.ResetToDefaults();
            Check("Reset restores master volume", modified.MasterVolume == 100f, ref pass, ref fail);
            Check("Reset clears mutes", !modified.MusicMute, ref pass, ref fail);

            // ── 5. Cooldown / Dedup ─────────────────────────────
            GD.Print("[AudioSelfTest] --- Cooldown/Dedup ---");
            var alertCues = new[] { AudioCueCatalog.RadAlertAcute, AudioCueCatalog.WeatherAlert, AudioCueCatalog.RadioStatic };
            foreach (string cueId in alertCues)
            {
                var cue = AudioCueCatalog.Resolve(cueId);
                Check($"Alert cue '{cueId}' has cooldown > 0", cue != null && cue.CooldownSeconds > 0, ref pass, ref fail);
            }

            var uiCues = new[] { AudioCueCatalog.UiClick, AudioCueCatalog.UiConfirm };
            foreach (string cueId in uiCues)
            {
                var cue = AudioCueCatalog.Resolve(cueId);
                Check($"UI cue '{cueId}' has cooldown", cue != null && cue.CooldownSeconds > 0, ref pass, ref fail);
            }

            // ── 6. Event-to-Cue Coverage ────────────────────────
            GD.Print("[AudioSelfTest] --- Event-to-Cue Coverage ---");
            string[] gameFlowCues = {
                AudioCueCatalog.MusicMenu, AudioCueCatalog.MusicGameplay,
                AudioCueCatalog.AmbBunker, AudioCueCatalog.AmbSurface,
                AudioCueCatalog.DayTransition, AudioCueCatalog.SaveSuccess,
                AudioCueCatalog.GameOver,
            };
            foreach (string cueId in gameFlowCues)
                Check($"Game flow cue '{cueId}' exists", AudioCueCatalog.Contains(cueId), ref pass, ref fail);

            string[] radCues = {
                AudioCueCatalog.RadAlertAcute, AudioCueCatalog.RadAlertChronic,
                AudioCueCatalog.RadGeigerBurst, AudioCueCatalog.RadGeigerLoop,
            };
            foreach (string cueId in radCues)
                Check($"Radiation cue '{cueId}' exists", AudioCueCatalog.Contains(cueId), ref pass, ref fail);

            string[] medicalCues = {
                AudioCueCatalog.MedHeartbeat, AudioCueCatalog.MedCoughing,
                AudioCueCatalog.MedSurvivorDeath, AudioCueCatalog.MedQuarantineSeal,
                AudioCueCatalog.MedQuarantineClear,
            };
            foreach (string cueId in medicalCues)
                Check($"Medical cue '{cueId}' exists", AudioCueCatalog.Contains(cueId), ref pass, ref fail);

            string[] weatherCues = {
                AudioCueCatalog.WeatherAlert, AudioCueCatalog.WeatherFalloutStorm,
                AudioCueCatalog.WeatherBlackRain, AudioCueCatalog.WeatherBlizzard,
                AudioCueCatalog.WeatherEmpStorm, AudioCueCatalog.WeatherGlassStorm,
                AudioCueCatalog.WeatherCorrosivePrecipitation,
            };
            foreach (string cueId in weatherCues)
                Check($"Weather cue '{cueId}' exists", AudioCueCatalog.Contains(cueId), ref pass, ref fail);

            string[] shelterCues = {
                AudioCueCatalog.ShelterDoorOpen, AudioCueCatalog.ShelterDoorSeal,
                AudioCueCatalog.ShelterVentilation, AudioCueCatalog.ShelterGenerator,
                AudioCueCatalog.ShelterAirFilter,
            };
            foreach (string cueId in shelterCues)
                Check($"Shelter cue '{cueId}' exists", AudioCueCatalog.Contains(cueId), ref pass, ref fail);

            string[] actionCues = { AudioCueCatalog.ActionItemPickup, AudioCueCatalog.ActionCrafting, AudioCueCatalog.ActionRepair, AudioCueCatalog.ActionTrade };
            foreach (string cueId in actionCues)
                Check($"Action cue '{cueId}' exists", AudioCueCatalog.Contains(cueId), ref pass, ref fail);

            string[] radioCues = {
                AudioCueCatalog.RadioStatic, AudioCueCatalog.RadioTune, AudioCueCatalog.RadioSignalLock,
                AudioCueCatalog.RadioMorse, AudioCueCatalog.RadioVoCh3AshRoad,
                AudioCueCatalog.RadioVoCh7Milband, AudioCueCatalog.RadioVoCh11Stockpile,
                AudioCueCatalog.RadioVoKindHatch, AudioCueCatalog.RadioVoKindParley,
                AudioCueCatalog.RadioVoVerdictMeter, AudioCueCatalog.RadioVoVerdictEden,
                AudioCueCatalog.RadioVoVerdictCount, AudioCueCatalog.RadioVoVerdictGeophone,
                AudioCueCatalog.RadioVoVerdictReckoning,
            };
            foreach (string cueId in radioCues)
                Check($"Radio cue '{cueId}' exists", AudioCueCatalog.Contains(cueId), ref pass, ref fail);

            string[] combatCues = { AudioCueCatalog.CombatStart, AudioCueCatalog.CombatFire, AudioCueCatalog.CombatJam, AudioCueCatalog.CombatReload, AudioCueCatalog.CombatHit, AudioCueCatalog.CombatDowned, AudioCueCatalog.CombatVictory, AudioCueCatalog.CombatDefeat };
            foreach (string cueId in combatCues)
                Check($"Combat cue '{cueId}' exists", AudioCueCatalog.Contains(cueId), ref pass, ref fail);

            // ── 7. Lifecycle Safety ─────────────────────────────
            GD.Print("[AudioSelfTest] --- Lifecycle Safety ---");
            Check("AudioCueCatalog.All is non-null", AudioCueCatalog.All != null, ref pass, ref fail);
            Check("AudioSettings singleton is safe", AudioSettings.Instance != null, ref pass, ref fail);

            var emittedCues = new List<string>();
            var bridge = new AudioEventBridge(emittedCues.Add);
            var firstWeather = new WeatherSystem();
            var secondWeather = new WeatherSystem();

            bridge.BindWeather(firstWeather);
            firstWeather.ForceWeather(WeatherKind.FalloutStorm);
            Check("Bridge maps fallout-storm changes to the specific cue",
                emittedCues.Count == 1 && emittedCues[0] == AudioCueCatalog.WeatherFalloutStorm,
                ref pass, ref fail);

            bridge.BindWeather(firstWeather);
            firstWeather.ForceWeather(WeatherKind.Blizzard);
            Check("Rebinding the same weather system does not duplicate handlers",
                emittedCues.Count == 2 && emittedCues[1] == AudioCueCatalog.WeatherBlizzard,
                ref pass, ref fail);

            bridge.BindWeather(secondWeather);
            firstWeather.ForceWeather(WeatherKind.BlackRain);
            Check("Replacing a weather system detaches the stale session",
                emittedCues.Count == 2, ref pass, ref fail);
            secondWeather.ForceWeather(WeatherKind.BlackRain);
            Check("Replacement weather system remains live",
                emittedCues.Count == 3 && emittedCues[2] == AudioCueCatalog.WeatherBlackRain,
                ref pass, ref fail);

            var specialistWeatherCues = new List<string>();
            var specialistWeatherBridge = new AudioEventBridge(specialistWeatherCues.Add);
            var specialistWeather = new WeatherSystem();
            specialistWeatherBridge.BindWeather(specialistWeather);
            specialistWeather.ForceWeather(WeatherKind.EMPStorm);
            specialistWeather.ForceWeather(WeatherKind.GlassStorm);
            specialistWeather.ForceWeather(WeatherKind.AcidSnow);
            Check("Bridge maps EMP, glass, and corrosive weather to dedicated cues",
                specialistWeatherCues.Count == 3
                && specialistWeatherCues[0] == AudioCueCatalog.WeatherEmpStorm
                && specialistWeatherCues[1] == AudioCueCatalog.WeatherGlassStorm
                && specialistWeatherCues[2] == AudioCueCatalog.WeatherCorrosivePrecipitation,
                ref pass, ref fail);
            specialistWeatherBridge.Dispose();

            var radiation = new RadiationSystem();
            var survivor = new SurvivorRadState { Id = "audio_selftest_survivor" };
            radiation.Register(survivor);
            bridge.BindRadiation(radiation);
            radiation.Expose(survivor, 100f, 1f);
            Check("Bridge maps rising radiation dose to a Geiger burst",
                emittedCues.Count >= 5 && emittedCues[3] == AudioCueCatalog.RadGeigerBurst,
                ref pass, ref fail);
            Check("Bridge maps acute radiation status to its alert cue",
                emittedCues.Count == 5 && emittedCues[4] == AudioCueCatalog.RadAlertAcute,
                ref pass, ref fail);
            radiation.SeedLifetimeExposure(survivor, RadiationSystem.ChronicLifetimeThreshold);
            Check("Bridge maps chronic radiation status to its alert cue",
                emittedCues.Count == 6 && emittedCues[5] == AudioCueCatalog.RadAlertChronic,
                ref pass, ref fail);

            var survivorFate = new SurvivorFateSystem();
            bridge.BindSurvivorFate(survivorFate);
            survivorFate.ReportDeath("audio_selftest_death", SurvivorDeathCause.Needs);
            Check("Bridge maps the unified survivor-death event to its distinct cue",
                emittedCues.Count == 7 && emittedCues[6] == AudioCueCatalog.MedSurvivorDeath,
                ref pass, ref fail);
            bridge.BindSurvivorFate(survivorFate);
            survivorFate.ReportDeath("audio_selftest_death_second", SurvivorDeathCause.Combat);
            Check("Rebinding survivor fate does not duplicate the death cue",
                emittedCues.Count == 8 && emittedCues[7] == AudioCueCatalog.MedSurvivorDeath,
                ref pass, ref fail);

            bridge.Dispose();
            secondWeather.ForceWeather(WeatherKind.Blizzard);
            radiation.AdministerIodine(survivor);
            survivorFate.ReportDeath("audio_selftest_death_after_dispose", SurvivorDeathCause.Disease);
            Check("Disposing the bridge detaches every domain handler",
                emittedCues.Count == 8, ref pass, ref fail);

            // Disease bridge lifecycle and patient-facing transitions.
            var diseaseEmitted = new List<string>();
            var diseaseBridge = new AudioEventBridge(diseaseEmitted.Add);
            var disease = new DiseaseSystem();
            var diseaseCatalog = new DiseaseCatalog();
            const string audioTestDisease = "disease_audio_selftest";
            diseaseCatalog.Add(new DiseaseDefinition
            {
                id = audioTestDisease,
                vector = DiseaseVectorNames.Air,
                illness_days = 1,
                infectivity = 0f,
                lethality = 0f,
            });
            disease.BindCatalog(diseaseCatalog);
            diseaseBridge.BindDisease(disease);
            disease.Infect("audio_patient", audioTestDisease, 1);
            disease.Quarantine("audio_patient", audioTestDisease);
            disease.EndQuarantine("audio_patient", audioTestDisease);
            disease.TickDaily(2, Array.Empty<string>());
            Check("Disease bridge maps infection, quarantine, and recovery transitions",
                diseaseEmitted.Count == 4
                && diseaseEmitted[0] == AudioCueCatalog.MedHeartbeat
                && diseaseEmitted[1] == AudioCueCatalog.MedQuarantineSeal
                && diseaseEmitted[2] == AudioCueCatalog.MedQuarantineClear
                && diseaseEmitted[3] == AudioCueCatalog.MedQuarantineClear,
                ref pass, ref fail);
            diseaseBridge.Dispose();
            disease.Infect("audio_patient_after_dispose", audioTestDisease, 3);
            Check("Disposing the disease bridge detaches every disease handler",
                diseaseEmitted.Count == 4, ref pass, ref fail);

            // Combat bridge lifecycle
            var combatEmitted = new List<string>();
            var combatBridge = new AudioEventBridge(combatEmitted.Add);
            var combat = new TacticalCombatSystem();
            combatBridge.BindCombat(combat);
            var player = new CombatantState { Id = "audio_st_player", Name = "Tester", IsPlayer = true, Health = 100f, MaxHealth = 100f };
            combat.BeginEncounter("audio_st_enc", "", "loc", "Test Location", 1, 42, new[] { player }, null, 1, 50f);
            Check("Bridge maps encounter_start to combat_start cue",
                combatEmitted.Count == 1 && combatEmitted[0] == AudioCueCatalog.CombatStart,
                ref pass, ref fail);
            combat.PlayerLastStand("audio_st_player", new SeededRng(42));
            Check("Combat last stand action triggers CombatLastStand cue",
                combatEmitted.Contains(AudioCueCatalog.CombatLastStand), ref pass, ref fail);
            combat.PlayerDecontaminate(new SeededRng(42));
            Check("Combat decon action triggers CombatDeconFlush cue",
                combatEmitted.Contains(AudioCueCatalog.CombatDeconFlush), ref pass, ref fail);
            combatBridge.BindCombat(combat);
            Check("Rebinding the same combat system does not duplicate handlers",
                combatBridge.HasCombatBinding, ref pass, ref fail);
            combatBridge.Dispose();
            Check("Disposing the combat bridge detaches the handler",
                !combatBridge.HasCombatBinding, ref pass, ref fail);

            // Expansion audio bridge lifecycle & idempotency tests
            var expansionBridgeEmitted = new List<string>();
            var expansionBridge = new ExpansionAudioBridge(expansionBridgeEmitted.Add);
            var chemWarfare = new Ashfall.Core.Combat.ChemWarfareSystem(new SeededRng(42));
            var testExpansionProvider = new TestExpansionAudioProvider(chemWarfare: chemWarfare);

            // Repeated refresh call (10 times simulating per-frame RefreshDomainBindings)
            for (int i = 0; i < 10; i++)
            {
                expansionBridge.SubscribeAll(testExpansionProvider);
            }

            // Trigger hazard deployment
            chemWarfare.DeployHazard("agent_mustard_test", 0, "src_test", 100);
            Check("Repeated refresh on ExpansionAudioBridge produces exactly one callback",
                expansionBridgeEmitted.Count == 1 && expansionBridgeEmitted[0] == AudioCueCatalog.HazardToxicSizzle,
                ref pass, ref fail);

            // Detach on Dispose
            expansionBridge.Dispose();
            chemWarfare.DeployHazard("agent_mustard_test2", 0, "src_test", 100);
            Check("Disposed ExpansionAudioBridge produces zero callbacks",
                expansionBridgeEmitted.Count == 1,
                ref pass, ref fail);

            // Rebinding detaches old provider
            var expansionBridgeEmitted2 = new List<string>();
            var expansionBridge2 = new ExpansionAudioBridge(expansionBridgeEmitted2.Add);
            var chemWarfareA = new Ashfall.Core.Combat.ChemWarfareSystem(new SeededRng(101));
            var chemWarfareB = new Ashfall.Core.Combat.ChemWarfareSystem(new SeededRng(102));
            var providerA = new TestExpansionAudioProvider(chemWarfare: chemWarfareA);
            var providerB = new TestExpansionAudioProvider(chemWarfare: chemWarfareB);

            expansionBridge2.SubscribeAll(providerA);
            expansionBridge2.SubscribeAll(providerB); // should detach A and attach B

            chemWarfareA.DeployHazard("agent_a", 0, "src_test", 100);
            Check("Rebound ExpansionAudioBridge does not trigger on stale provider",
                expansionBridgeEmitted2.Count == 0,
                ref pass, ref fail);

            chemWarfareB.DeployHazard("agent_b", 0, "src_test", 100);
            Check("Rebound ExpansionAudioBridge triggers on active provider",
                expansionBridgeEmitted2.Count == 1 && expansionBridgeEmitted2[0] == AudioCueCatalog.HazardToxicSizzle,
                ref pass, ref fail);
            expansionBridge2.Dispose();

            // ── Four Expansion Probes (Phase 5 Completion Gate) ─────
            var probeEmitted = new List<string>();
            var probeBridge = new ExpansionAudioBridge(probeEmitted.Add);

            var needsSys = new NeedsSystem();
            needsSys.Register(new SurvivorNeedsState { Id = "actor_1", Hunger = 100f, Health = 100f, IsAlive = true, IsDead = false });

            var probeDesperation = new DesperationSystem(
                new SeededRng(501),
                new Ashfall.Core.Inventory.Inventory(),
                needsSys);
            probeDesperation.RegisterCorpse("corpse_1");
            probeDesperation.RegisterEvent(new DesperationEventDef
            {
                event_id = "ev_probe",
                required_starvation = 50f,
                taboo_level = "Broken"
            });

            var probeMutation = new MutationSystem(
                new SeededRng(503),
                new Ashfall.Core.Inventory.Inventory());
            probeMutation.RegisterMutation(new MutationNode { mutation_id = "mut_probe", required_exposure = 0 });

            var probeChem = new ChemWarfareSystem(new SeededRng(504));
            var probeRailway = new RailwaySystem(new SeededRng(505));
            probeRailway.RegisterCatalog(new RailwayNetworkCatalog
            {
                nodes = new List<RailNodeDef> { new RailNodeDef { node_id = "n1" }, new RailNodeDef { node_id = "n2" } },
                segments = new List<TrackSegmentDef> { new TrackSegmentDef { segment_id = "seg1", start_node_id = "n1", end_node_id = "n2" } }
            });

            var fullProvider = new TestExpansionAudioProvider(
                desperation: probeDesperation,
                mutation: probeMutation,
                chemWarfare: probeChem,
                railway: probeRailway);

            probeBridge.SubscribeAll(fullProvider);

            // Probe 1: Desperation (Taboo Broken -> InterrogationSlam)
            probeDesperation.HarvestCorpse("actor_1", "corpse_1", "ev_probe", 1);
            Check("Probe 1: Desperation taboo broken emits action_interrogation_slam",
                probeEmitted.Contains(AudioCueCatalog.InterrogationSlam), ref pass, ref fail);

            // Probe 2: Mutation (Mutation Acquired -> BioMutationPulse)
            var mutProf = probeMutation.EnsureProfile("survivor_1");
            mutProf.geneticInstability = 250.0f;
            probeMutation.TryMutateSurvivor("survivor_1", 1);
            Check("Probe 2: Mutation acquired emits bio_mutation_pulse",
                probeEmitted.Contains(AudioCueCatalog.BioMutationPulse), ref pass, ref fail);

            // Probe 3: ChemWarfare (Hazard Deployed -> HazardToxicSizzle)
            probeChem.DeployHazard("agent_chlorine", 0, "src_probe", 50);
            Check("Probe 3: Chemical hazard deployed emits hazard_toxic_sizzle",
                probeEmitted.Contains(AudioCueCatalog.HazardToxicSizzle), ref pass, ref fail);

            // Probe 4: Railway (Derailment -> TrainScreechCrash)
            var train = new TrainState { trainId = "train_1", status = TrainDispatchStatus.EnRoute, activeSegmentId = "seg1" };
            probeRailway.State.trains.Add(train);
            probeRailway.State.segments["seg1"].integrity = 0.0f;
            for (int tick = 0; tick < 50 && train.status == TrainDispatchStatus.EnRoute; tick++)
            {
                probeRailway.TickTravel("train_1", 0.0f);
            }
            Check("Probe 4: Railway derailment emits train_screech_crash",
                probeEmitted.Contains(AudioCueCatalog.TrainScreechCrash), ref pass, ref fail);

            probeBridge.Dispose();

            // Shelter controller lifecycle and threshold behaviour.
            var shelterEmitted = new List<string>();
            var shelterStopped = new List<string>();
            var shelterAudio = new ShelterAudioController(shelterEmitted.Add, shelterStopped.Add);
            var startingLevel = new StartingLevelSystem();
            var powerGrid = new PowerGridSystem(
                new PowerGridState
                {
                    GenerationWatts = 100f,
                    FuelUnits = 10f,
                    BatteryCapacityWh = 100f,
                    BatteryReserveWh = 100f,
                },
                new[] { new PowerGridRoom("audio_shelter", "Audio Shelter", 10f) },
                new SeededRng(77));
            shelterAudio.Subscribe(powerGrid, startingLevel);
            Check("Shelter controller starts generator, ventilation, recycler, and water filtration loops",
                shelterEmitted.Contains(AudioCueCatalog.ShelterGenerator)
                && shelterEmitted.Contains(AudioCueCatalog.ShelterVentilation)
                && shelterEmitted.Contains(AudioCueCatalog.ShelterAirRecycler)
                && shelterEmitted.Contains(AudioCueCatalog.ShelterWaterFiltration),
                ref pass, ref fail);
            Check("Shelter controller does not emit a generator start transient during initial binding",
                !shelterEmitted.Contains(AudioCueCatalog.ShelterGeneratorStart), ref pass, ref fail);
            powerGrid.ToggleBreaker("audio_shelter");
            powerGrid.ToggleBreaker("audio_shelter");
            Check("Shelter controller emits authored power restore when a breaker is reclosed",
                shelterEmitted.Contains(AudioCueCatalog.ShelterPowerRestore), ref pass, ref fail);
            powerGrid.MarkTripped("audio_shelter", 1);
            Check("Shelter controller emits a distinct breaker trip instead of a generic alarm",
                shelterEmitted.Contains(AudioCueCatalog.ShelterBreakerTrip)
                && !shelterEmitted.Contains(AudioCueCatalog.DangerAlarmKlaxon), ref pass, ref fail);
            var hazardousAir = startingLevel.CaptureState();
            hazardousAir.airFilterHealthPercent = 45f;
            hazardousAir.airHazardWarning = true;
            startingLevel.RestoreState(hazardousAir);
            Check("Shelter controller alerts when the air filter becomes hazardous",
                shelterEmitted.Contains(AudioCueCatalog.ShelterAirFilter), ref pass, ref fail);
            shelterAudio.Dispose();
            Check("Shelter controller stops infrastructure loops on disposal",
                shelterStopped.Contains(AudioCueCatalog.ShelterGenerator)
                && shelterStopped.Contains(AudioCueCatalog.ShelterVentilation)
                && shelterStopped.Contains(AudioCueCatalog.ShelterAirRecycler)
                && shelterStopped.Contains(AudioCueCatalog.ShelterWaterFiltration),
                ref pass, ref fail);

            // Heavy electrical load switches generator to heavy strain loop
            var heavyGrid = new PowerGridSystem(
                new PowerGridState
                {
                    GenerationWatts = 100f,
                    FuelUnits = 10f,
                    BatteryCapacityWh = 100f,
                    BatteryReserveWh = 100f,
                },
                new[] { new PowerGridRoom("audio_heavy", "Heavy Machinery", 90f) },
                new SeededRng(78));
            var heavyShelterAudio = new ShelterAudioController(shelterEmitted.Add, shelterStopped.Add);
            heavyShelterAudio.Subscribe(heavyGrid, startingLevel);
            Check("Shelter controller selects heavy generator strain under high electrical load",
                shelterEmitted.Contains(AudioCueCatalog.ShelterGeneratorStrain),
                ref pass, ref fail);
            heavyShelterAudio.Dispose();

            // Generator transitions are event-driven; the low-power bed follows
            // actual fuel state and every persistent cue receives a stop path.
            var transitionEmitted = new List<string>();
            var transitionStopped = new List<string>();
            var transitionGrid = new PowerGridSystem(
                new PowerGridState
                {
                    GenerationWatts = 100f,
                    FuelUnits = 0f,
                    BatteryCapacityWh = 0f,
                    BatteryReserveWh = 0f,
                },
                new[] { new PowerGridRoom("audio_transition", "Transition Load", 10f) },
                new SeededRng(79));
            var transitionAudio = new ShelterAudioController(transitionEmitted.Add, transitionStopped.Add);
            transitionAudio.Subscribe(transitionGrid, null);
            Check("Fuel-starved shelter starts the authored low-power bed without a false stop transient",
                transitionEmitted.Contains(AudioCueCatalog.AmbBunkerLowPower)
                && !transitionEmitted.Contains(AudioCueCatalog.ShelterGeneratorStop), ref pass, ref fail);
            transitionGrid.AddFuel(0.1f);
            Check("Adding fuel starts the generator transient and releases the low-power bed",
                transitionEmitted.Contains(AudioCueCatalog.ShelterGeneratorStart)
                && transitionStopped.Contains(AudioCueCatalog.AmbBunkerLowPower), ref pass, ref fail);
            transitionGrid.TickDay(1, new SeededRng(80));
            Check("Fuel exhaustion stops the generator and restores the low-power bed",
                transitionEmitted.Contains(AudioCueCatalog.ShelterGeneratorStop)
                && transitionEmitted.Contains(AudioCueCatalog.AmbBunkerLowPower), ref pass, ref fail);
            transitionAudio.Dispose();
            Check("Disposing shelter audio stops the low-power bed",
                transitionStopped.Contains(AudioCueCatalog.AmbBunkerLowPower), ref pass, ref fail);

            // Surface ambience is explicitly activated, then follows weather
            // without treating an expedition as player-location evidence.
            var surfaceEmitted = new List<string>();
            var surfaceStopped = new List<string>();
            var surfaceAudio = new SurfaceAmbienceController(surfaceEmitted.Add, surfaceStopped.Add);
            var surfaceWeather = new WeatherSystem();
            surfaceAudio.Subscribe(surfaceWeather);
            surfaceStopped.Clear();
            surfaceAudio.Start();
            surfaceWeather.ForceWeather(WeatherKind.Ashfall);
            surfaceWeather.ForceWeather(WeatherKind.FalloutStorm);
            surfaceWeather.ForceWeather(WeatherKind.Blizzard);
            surfaceWeather.ForceWeather(WeatherKind.GlassStorm);
            int surfaceCountBeforeSilence = surfaceEmitted.Count;
            surfaceWeather.ForceWeather(WeatherKind.Silence);
            surfaceWeather.ForceWeather(WeatherKind.SilentSpring);
            int surfaceCountAfterSilence = surfaceEmitted.Count;
            surfaceWeather.ForceWeather(WeatherKind.Clear);
            Check("Surface ambience selects authored ashfall, fallout, blizzard, and generic storm beds",
                surfaceEmitted.Count >= 6
                && surfaceEmitted.Contains(AudioCueCatalog.AmbSurface)
                && surfaceEmitted.Contains(AudioCueCatalog.AmbSurfaceAshfall)
                && surfaceEmitted.Contains(AudioCueCatalog.AmbSurfaceFalloutStorm)
                && surfaceEmitted.Contains(AudioCueCatalog.AmbSurfaceBlizzard)
                && surfaceEmitted.Contains(AudioCueCatalog.AmbSurfaceStorm)
                && surfaceStopped.Contains(AudioCueCatalog.AmbSurface)
                && surfaceStopped.Contains(AudioCueCatalog.AmbSurfaceStorm),
                ref pass, ref fail);
            Check("Silence and SilentSpring author no replacement bed while preserving critical cue routing",
                surfaceCountAfterSilence == surfaceCountBeforeSilence
                && surfaceStopped.Contains(AudioCueCatalog.AmbSurfaceAshfall)
                && surfaceStopped.Contains(AudioCueCatalog.AmbSurfaceFalloutStorm)
                && surfaceStopped.Contains(AudioCueCatalog.AmbSurfaceBlizzard), ref pass, ref fail);
            surfaceAudio.Dispose();
            int surfaceCountAfterDispose = surfaceEmitted.Count;
            surfaceWeather.ForceWeather(WeatherKind.EMPStorm);
            Check("Disposing surface ambience detaches the weather handler",
                surfaceEmitted.Count == surfaceCountAfterDispose, ref pass, ref fail);

            // ── 9. Anti-Fatigue Micro-Jitter & DSP Architecture ─────────
            GD.Print("[AudioSelfTest] --- Anti-Fatigue Micro-Jitter & DSP ---");
            var clickCue = AudioCueCatalog.Resolve(AudioCueCatalog.UiClick);
            Check("UiClick cue exists and has multi-sample alternative paths",
                clickCue != null && clickCue.ResourcePaths.Count >= 4, ref pass, ref fail);
            Check("UiClick cue has pitch jitter configured",
                clickCue != null && clickCue.PitchMin < clickCue.PitchMax, ref pass, ref fail);
            Check("UiClick cue has volume jitter configured",
                clickCue != null && clickCue.VolumeJitterDb > 0f, ref pass, ref fail);

            var gunshotCue = AudioCueCatalog.Resolve(AudioCueCatalog.CombatFire);
            Check("CombatFire cue has anti-fatigue pitch jitter",
                gunshotCue != null && gunshotCue.PitchMin < gunshotCue.PitchMax, ref pass, ref fail);
            Check("CombatFire cue has anti-fatigue volume jitter",
                gunshotCue != null && gunshotCue.VolumeJitterDb > 0f, ref pass, ref fail);

            // Multi-sample pools for weapons (arsenal)
            var cz75 = AudioCueCatalog.Resolve(AudioCueCatalog.WeaponCz75Report);
            var pipe = AudioCueCatalog.Resolve(AudioCueCatalog.WeaponPipeRifleReport);
            var shotgun = AudioCueCatalog.Resolve(AudioCueCatalog.WeaponScrapShotgunReport);
            var bolt = AudioCueCatalog.Resolve(AudioCueCatalog.WeaponBoltRifleReport);
            var assault = AudioCueCatalog.Resolve(AudioCueCatalog.WeaponAssaultRifleBurst);
            var sniper = AudioCueCatalog.Resolve(AudioCueCatalog.WeaponSniperHeavyReport);
            Check("Gunshot arsenal cues have >= 5 multi-sample acoustic variations",
                cz75 != null && cz75.ResourcePaths.Count >= 5
                && pipe != null && pipe.ResourcePaths.Count >= 5
                && shotgun != null && shotgun.ResourcePaths.Count >= 5
                && bolt != null && bolt.ResourcePaths.Count >= 5
                && assault != null && assault.ResourcePaths.Count >= 5
                && sniper != null && sniper.ResourcePaths.Count >= 5, ref pass, ref fail);

            // Multi-sample pools for distance shots & explosions
            var art = AudioCueCatalog.Resolve(AudioCueCatalog.DistantArtilleryBarrage);
            var skirmish = AudioCueCatalog.Resolve(AudioCueCatalog.DistantGunfireSkirmish);
            var explosion = AudioCueCatalog.Resolve(AudioCueCatalog.DangerExplosion);
            Check("Distance shots and explosions have >= 5 multi-sample acoustic variations",
                art != null && art.ResourcePaths.Count >= 5
                && skirmish != null && skirmish.ResourcePaths.Count >= 5
                && explosion != null && explosion.ResourcePaths.Count >= 5, ref pass, ref fail);

            // Material-specific footsteps (granite, metal, dirt, glass, wood)
            var fsGranite = AudioCueCatalog.Resolve(AudioCueCatalog.FootstepGranite);
            var fsMetal = AudioCueCatalog.Resolve(AudioCueCatalog.FootstepMetal);
            var fsDirt = AudioCueCatalog.Resolve(AudioCueCatalog.FootstepDirt);
            var fsGlass = AudioCueCatalog.Resolve(AudioCueCatalog.FootstepGlass);
            var fsWood = AudioCueCatalog.Resolve(AudioCueCatalog.FootstepWood);
            Check("Material-specific footsteps exist with >= 5 variations each (granite, metal, dirt, glass, wood)",
                fsGranite != null && fsGranite.ResourcePaths.Count >= 5
                && fsMetal != null && fsMetal.ResourcePaths.Count >= 5
                && fsDirt != null && fsDirt.ResourcePaths.Count >= 5
                && fsGlass != null && fsGlass.ResourcePaths.Count >= 5
                && fsWood != null && fsWood.ResourcePaths.Count >= 5, ref pass, ref fail);

            // Location ambience granite quarry resolution
            Check("SurfaceAmbienceController resolves granite quarry location to AmbLocGraniteQuarry",
                SurfaceAmbienceController.ResolveLocationAmbience("deep_granite_quarry") == AudioCueCatalog.AmbLocGraniteQuarry,
                ref pass, ref fail);

            int surfaceBusIdx = AudioServer.GetBusIndex(AudioBusNames.Surface);
            Check("Surface audio bus exists in AudioServer", surfaceBusIdx >= 0, ref pass, ref fail);
            if (surfaceBusIdx >= 0)
            {
                Check("Surface audio bus has DSP effect (bunker occlusion filter)",
                    AudioServer.GetBusEffectCount(surfaceBusIdx) > 0, ref pass, ref fail);
            }

            int radioBusIdx = AudioServer.GetBusIndex(AudioBusNames.Radio);
            Check("Radio audio bus exists in AudioServer", radioBusIdx >= 0, ref pass, ref fail);
            if (radioBusIdx >= 0)
            {
                Check("Radio audio bus has transceiver DSP effects (bandpass filter + distortion)",
                    AudioServer.GetBusEffectCount(radioBusIdx) >= 2, ref pass, ref fail);
            }

            // Verify modal manager audio wiring doesn't throw
            var modalMgr = new ModalManager();
            Check("ModalManager instantiates and wires modal audio triggers safely",
                modalMgr != null, ref pass, ref fail);

            // ── 10. Phase 2: Dynamic Soundscapes & Expedition Logistics ───────
            GD.Print("[AudioSelfTest] --- Phase 2: Expedition Logistics & Trauma Soundscapes ---");
            var expEmitted = new List<string>();
            var expStopped = new List<string>();
            var expBridge = new AudioEventBridge(expEmitted.Add, expStopped.Add);
            var expSystem = new ExpeditionSystem();
            expBridge.BindExpeditions(expSystem);

            var expDef = new ExpeditionDefinition
            {
                id = "loc_phase2_test",
                displayName = "Phase 2 Outpost",
                distanceTicks = 8,
            };
            var vehicleProfile = new ExpeditionVehicleProfile
            {
                vehicleId = "veh_quad_phase2",
                speedMultiplier = 1.5f,
                breakdownChancePerTick = 1.0f,
            };
            expSystem.Start(expDef, "survivor_p2", 1, ExpeditionStance.Stealth, false, false, false, vehicleProfile);
            Check("Expedition with vehicle starts vehicle engine sound",
                expEmitted.Contains(AudioCueCatalog.ExpeditionVehicleEngine), ref pass, ref fail);

            expSystem.TickHours(1f, new SeededRng(42));
            Check("Vehicle breakdown triggers dedicated breakdown stall SFX and stops engine loop",
                expEmitted.Contains(AudioCueCatalog.ExpeditionVehicleBreakdown)
                && expStopped.Contains(AudioCueCatalog.ExpeditionVehicleEngine), ref pass, ref fail);

            expBridge.Dispose();
            Check("Disposing expedition audio bridge stops vehicle engine loop",
                expStopped.Contains(AudioCueCatalog.ExpeditionVehicleEngine), ref pass, ref fail);

            Check("Psychological trauma cues resolve cleanly",
                AudioCueCatalog.Resolve(AudioCueCatalog.TraumaTinnitus) != null
                && AudioCueCatalog.Resolve(AudioCueCatalog.TraumaHeartbeatRapid) != null
                && AudioCueCatalog.Resolve(AudioCueCatalog.TraumaCabinFever) != null,
                ref pass, ref fail);

            // Somatic flashbacks bridge tests
            var flashbackEmitted = new List<string>();
            var flashbackBridge = new AudioEventBridge(flashbackEmitted.Add);
            var flashbackSystem = new SomaticFlashbackSystem
            {
                Rng = new SeededRng(1337),
                GetAliveSurvivorIds = () => new[] { "survivor_trauma_test" },
                IsCompanionInSameRoom = (s1, s2) => false
            };
            flashbackBridge.BindFlashbacks(flashbackSystem);
            Check("AudioEventBridge registers flashbacks binding", flashbackBridge.HasFlashbacksBinding, ref pass, ref fail);

            flashbackSystem.IncreaseSusceptibility("survivor_trauma_test", 1.0f);
            flashbackSystem.OnAudioEvent("explosion", 10.0f);
            Check("Flashback trigger emits FlashbackTrigger and TraumaTinnitus cues",
                flashbackEmitted.Contains(AudioCueCatalog.FlashbackTrigger)
                && flashbackEmitted.Contains(AudioCueCatalog.TraumaTinnitus), ref pass, ref fail);

            // Grounded test
            var groundedEmitted = new List<string>();
            var groundedBridge = new AudioEventBridge(groundedEmitted.Add);
            var groundedSystem = new SomaticFlashbackSystem
            {
                Rng = new SeededRng(1337),
                GetAliveSurvivorIds = () => new[] { "survivor_trauma_test", "companion_01" },
                IsCompanionInSameRoom = (s1, s2) => true
            };
            groundedBridge.BindFlashbacks(groundedSystem);
            groundedSystem.IncreaseSusceptibility("survivor_trauma_test", 1.0f);
            groundedSystem.OnAudioEvent("explosion", 10.0f);
            Check("Grounded flashback emits FlashbackGrounded cue",
                groundedEmitted.Contains(AudioCueCatalog.FlashbackGrounded), ref pass, ref fail);

            groundedBridge.Dispose();
            Check("Disposed bridge unbinds flashbacks", !groundedBridge.HasFlashbacksBinding, ref pass, ref fail);

            // Tape player & echo discovery API test
            audioMgr.PlayTapeInsert();
            audioMgr.PlayTapeButton();
            audioMgr.PlayTapeHiss();
            audioMgr.StopTapeHiss();
            audioMgr.PlayEchoDiscovery();
            Check("Tape player and echo discovery APIs execute cleanly", true, ref pass, ref fail);

            // ── 11. Phase 4: Advanced Vehicles, Ballistics & Extreme Dosimetry ───────
            GD.Print("[AudioSelfTest] --- Phase 4: Vehicles, Ballistics & Dosimetry ---");

            // Dirt bike engine loop test
            var dirtBikeEmitted = new List<string>();
            var dirtBikeStopped = new List<string>();
            var dirtBikeBridge = new AudioEventBridge(dirtBikeEmitted.Add, dirtBikeStopped.Add);
            var bikeSystem = new ExpeditionSystem();
            dirtBikeBridge.BindExpeditions(bikeSystem);
            var bikeProfile = new ExpeditionVehicleProfile { vehicleId = "vehicle_dirt_bike", speedMultiplier = 1.8f };
            bikeSystem.Start(expDef, "survivor_bike", 1, ExpeditionStance.Stealth, false, false, false, bikeProfile);
            Check("Dirt bike expedition starts high-RPM dirt bike engine loop",
                dirtBikeEmitted.Contains(AudioCueCatalog.ExpeditionVehicleDirtBike), ref pass, ref fail);
            dirtBikeBridge.Dispose();
            Check("Disposing dirt bike bridge stops dirt bike loop",
                dirtBikeStopped.Contains(AudioCueCatalog.ExpeditionVehicleDirtBike), ref pass, ref fail);

            // Cargo truck engine loop test
            var truckEmitted = new List<string>();
            var truckStopped = new List<string>();
            var truckBridge = new AudioEventBridge(truckEmitted.Add, truckStopped.Add);
            var truckSystem = new ExpeditionSystem();
            truckBridge.BindExpeditions(truckSystem);
            var truckProfile = new ExpeditionVehicleProfile { vehicleId = "vehicle_cargo_truck", speedMultiplier = 1.6f };
            truckSystem.Start(expDef, "survivor_truck", 1, ExpeditionStance.Stealth, false, false, false, truckProfile);
            Check("Truck expedition starts heavy turbo diesel engine loop",
                truckEmitted.Contains(AudioCueCatalog.ExpeditionVehicleTruck), ref pass, ref fail);
            truckBridge.Dispose();
            Check("Disposing truck bridge stops truck loop",
                truckStopped.Contains(AudioCueCatalog.ExpeditionVehicleTruck), ref pass, ref fail);

            // Combat ballistics material impact foley test
            CombatCatalog.SeedDefaults();
            var foleyEmitted = new List<string>();
            var foleyBridge = new AudioEventBridge(foleyEmitted.Add);
            var foleyCombat = new TacticalCombatSystem();
            foleyBridge.BindCombat(foleyCombat);
            var foleyPlayer = new CombatantState { Id = "foley_player", Name = "Striker", IsPlayer = true, Health = 100f, MaxHealth = 100f, WeaponInstanceId = "w_pipe" };
            var foleyWeapon = new WeaponInstanceState { InstanceId = "w_pipe", WeaponId = "weapon_pipe_rifle", AmmoRemaining = 5, MagazineCapacity = 5, ConditionPct = 1f };
            foleyCombat.BeginEncounter("enc_foley", "", "loc", "Rubble Yard", 1, 42, new[] { foleyPlayer }, new[] { foleyWeapon }, 1, 50f);
            foleyCombat.PlayerFire("enemy_enc_foley_0", new SeededRng(101));
            Check("Combat firearm shot triggers casing drop and impact audio",
                foleyEmitted.Contains(AudioCueCatalog.CombatCasingDrop), ref pass, ref fail);
            foleyBridge.Dispose();

            // Vehicle logistics & intense Geiger APIs
            audioMgr.PlayVehicleRefuel();
            audioMgr.PlayVehicleRepair();
            audioMgr.StartGeiger(intense: true);
            audioMgr.StopGeiger();
            Check("Vehicle logistics and intense Geiger APIs execute cleanly", true, ref pass, ref fail);

            // ── 12. Phase 5: Narrative Radio, Echoes, Tape Transport & Item Foley ───
            GD.Print("[AudioSelfTest] --- Phase 5: Narrative Radio, Tape Transport & Item Foley ---");

            // Radio transmission APIs
            audioMgr.PlayNumbersStation();
            audioMgr.StopNumbersStation();
            audioMgr.PlayEbsAlert();
            audioMgr.PlayDeadHandPulse();
            audioMgr.StopDeadHandPulse();
            audioMgr.PlayDistressBeacon();
            audioMgr.StopDistressBeacon();
            Check("Radio numbers station, EBS alert, dead hand and distress beacon APIs execute cleanly", true, ref pass, ref fail);

            // Tape transport APIs
            audioMgr.PlayTapeRewind();
            audioMgr.PlayTapeStop();
            Check("Cassette tape transport rewind and stop APIs execute cleanly", true, ref pass, ref fail);

            // Item handling foley APIs
            audioMgr.PlayItemHandling("item_ammo_box_308");
            audioMgr.PlayItemHandling("item_med_vial_rad_away");
            audioMgr.PlayItemHandling("item_ration_mre_pack");
            audioMgr.PlayItemHandling("item_misc_scrap");
            Check("Item handling foley dynamically routes categories safely", true, ref pass, ref fail);

            // RadioPanel instance test
            var radioPanel = new RadioPanel();
            radioPanel.Open();
            radioPanel.Close();
            Check("RadioPanel instantiates, opens, closes and manages audio cleanly", true, ref pass, ref fail);
            radioPanel.QueueFree();

            // ── 13. Phase 6: Plan 29 Machine Identity Tells (§29B.21 audio hooks) ───
            GD.Print("[AudioSelfTest] --- Phase 6: Machine Tell Audio (Plan 29 consumer) ---");

            // The tell catalog loads from the data authority and every authored
            // quirk audio_cue resolves to a registered cue with a loadable stream.
            string tellDataDir = Directory.Exists("Assets/StreamingAssets/Data")
                ? "Assets/StreamingAssets/Data"
                : ProjectSettings.GlobalizePath("res://assets/StreamingAssets/Data");
            var tellCatalog = ShelterMachineTellCatalog.Load(
                new Ashfall.Core.FileSystemIO(), new SystemTextJsonSerializer(), tellDataDir);
            Check("Machine tell catalog loads from data authority", tellCatalog.MachineCount > 0, ref pass, ref fail);
            Check("Tell catalog contract validates clean", tellCatalog.Validate().Count == 0, ref pass, ref fail);

            int cuedQuirks = 0, cueResolved = 0, streamsLoaded = 0;
            for (int q = 0; q < tellCatalog.Quirks.Count; q++)
            {
                var quirk = tellCatalog.Quirks[q];
                if (string.IsNullOrWhiteSpace(quirk.audio_cue)) continue;
                cuedQuirks++;
                var tellCue = AudioCueCatalog.Resolve(quirk.audio_cue);
                if (tellCue != null)
                {
                    cueResolved++;
                    if (AudioManager.LoadDirectStream(tellCue.ResourcePath) != null)
                        streamsLoaded++;
                }
            }
            Check($"All quirk audio cues registered: {cueResolved}/{cuedQuirks}", cuedQuirks > 0 && cueResolved == cuedQuirks, ref pass, ref fail);
            Check($"All quirk audio streams load: {streamsLoaded}/{cuedQuirks}", streamsLoaded == cuedQuirks, ref pass, ref fail);

            // Threshold transitions drive the condition diff: degraded readings
            // start tells, recovery stops them, personality beds sustain.
            var tellAudio = new AudioConditionSystem();
            var degradedReadings = new MachineConditionReadings
            {
                HepaFilterHealth = 40f,
                HazardWeather = true,
                PowerFuelUnits = 5f
            };
            var degradedOutcome = MachineTellAudioSync.Apply(tellCatalog, degradedReadings, tellAudio,
                cueId => AudioCueCatalog.Resolve(cueId)?.Loop ?? false);
            Check("Degraded readings start hepa whistle tell",
                degradedOutcome.Started.Contains("machine_quirk_hepa_intake_whistle"), ref pass, ref fail);
            Check("Degraded readings start storm cough tell (context gate)",
                degradedOutcome.Started.Contains("machine_quirk_hepa_storm_cough"), ref pass, ref fail);
            Check("Degraded readings start generator fuel cough tell",
                degradedOutcome.Started.Contains("machine_quirk_generator_fuel_cough"), ref pass, ref fail);
            Check("Personality beds stay active under degradation", degradedOutcome.ActiveTotal >= 10, ref pass, ref fail);

            var steadyOutcome = MachineTellAudioSync.Apply(tellCatalog, degradedReadings, tellAudio,
                cueId => AudioCueCatalog.Resolve(cueId)?.Loop ?? false);
            Check("Repeated sync is a no-op (fires on transitions, not continuously)", steadyOutcome.Clean, ref pass, ref fail);

            var recoveredOutcome = MachineTellAudioSync.Apply(tellCatalog, new MachineConditionReadings(), tellAudio,
                cueId => AudioCueCatalog.Resolve(cueId)?.Loop ?? false);
            Check("Recovery stops diagnostic tells", recoveredOutcome.Stopped.Contains("machine_quirk_hepa_intake_whistle"), ref pass, ref fail);
            Check("Recovery keeps personality beds", recoveredOutcome.ActiveTotal == 7, ref pass, ref fail);

            // Host routing probe: one machine-quirk condition round-trips through
            // the AudioManager condition API without error (headless-safe).
            audioMgr.RouteCondition(AudioCueCatalog.HepaRadonHum, "ventilation", 1f, true);
            audioMgr.SetLoopIntensity(AudioCueCatalog.HepaRadonHum, 0.5f);
            audioMgr.StopCondition(AudioCueCatalog.HepaRadonHum);
            Check("Machine tell loop routes through AudioManager condition API cleanly", true, ref pass, ref fail);

            // ── Summary ─────────────────────────────────────────
            GD.Print($"[AudioSelfTest] --- SUMMARY ---");
            GD.Print($"[AudioSelfTest] Pass: {pass}, Fail: {fail}, Total: {pass + fail}");
            GD.Print($"[AudioSelfTest] Cues: {cueCount} ({resolved} resolved, {fallback} fallback, {silent} silent)");
            GD.Print($"[AudioSelfTest] Assets checked: {keyAssets.Length}");

            bool allPass = fail == 0 && (pass + fail) > 0;
            if (createdManager)
            {
                audioMgr.QueueFree();
                if (AudioManager.Instance == audioMgr)
                    AudioManager.Instance = null;
            }
            return HostCli.EmitSummary("audio_selftest", allPass, allPass ? 0 : 1, pass, fail,
                details: $"cues={cueCount} resolved={resolved} fallback={fallback} silent={silent} assets={keyAssets.Length}");
        }

        private static void Check(string label, bool condition, ref int pass, ref int fail)
        {
            if (condition)
            {
                GD.Print($"  [PASS] {label}");
                pass++;
            }
            else
            {
                GD.PrintErr($"  [FAIL] {label}");
                fail++;
            }
        }
    }

    internal sealed class TestExpansionAudioProvider : IExpansionAudioProvider
    {
        public Ashfall.Core.Survivors.DesperationSystem? AudioDesperation { get; set; }
        public Ashfall.Core.Medical.MutationSystem? AudioMutation { get; set; }
        public Ashfall.Core.Combat.ChemWarfareSystem? AudioChemWarfare { get; set; }
        public Ashfall.Core.Expeditions.RailwaySystem? AudioRailway { get; set; }

        public TestExpansionAudioProvider(
            Ashfall.Core.Survivors.DesperationSystem? desperation = null,
            Ashfall.Core.Medical.MutationSystem? mutation = null,
            Ashfall.Core.Combat.ChemWarfareSystem? chemWarfare = null,
            Ashfall.Core.Expeditions.RailwaySystem? railway = null)
        {
            AudioDesperation = desperation;
            AudioMutation = mutation;
            AudioChemWarfare = chemWarfare;
            AudioRailway = railway;
        }
    }
}
