using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Ashfall.Core;
using Ashfall.Core.Radiation;
using Ashfall.Core.World;

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

            // Key cues exist
            string[] requiredCues = {
                AudioCueCatalog.UiClick, AudioCueCatalog.UiConfirm, AudioCueCatalog.UiWarning,
                AudioCueCatalog.RadAlertAcute, AudioCueCatalog.WeatherAlert,
                AudioCueCatalog.AmbBunker, AudioCueCatalog.MusicMenu,
                AudioCueCatalog.RadioStatic, AudioCueCatalog.ShelterDoorOpen,
                AudioCueCatalog.ActionItemPickup, AudioCueCatalog.DangerAlarmKlaxon,
                AudioCueCatalog.SaveSuccess, AudioCueCatalog.DayTransition,
                AudioCueCatalog.GameOver,
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
            string[] expectedBuses = { "Master", "Music", "Ambience", "SFX", "UI", "Voice", "Alerts" };
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
                "res://assets/audio/ui/ui_confirm.wav",
                "res://assets/audio/ui/ui_warning.wav",
                "res://assets/audio/sfx/sfx_radiation_alarm.mp3",
                "res://assets/audio/sfx/sfx_alarm_klaxon.mp3",
                "res://assets/audio/sfx/geiger.wav",
                "res://assets/audio/ambience/bunker_ambience.ogg",
                "res://assets/audio/ambience/surface_ambience.ogg",
                "res://assets/audio/music/main_menu.ogg",
                "res://assets/audio/music/gameplay_underscore.ogg",
                "res://assets/audio/radio/radio_static_hiss.wav",
                "res://assets/audio/sfx/sfx_bunker_door_open.mp3",
                "res://assets/audio/sfx/sfx_crafting_assemble.mp3",
                "res://assets/audio/sfx/sfx_ventilation_fan.mp3",
                "res://assets/audio/sfx/sfx_fallout_storm_approach.mp3",
                "res://assets/audio/sfx/sfx_geiger_burst.mp3",
                "res://assets/audio/sfx/sfx_heartbeat_slow.mp3",
                "res://assets/audio/sfx/sfx_alarm_klaxon.mp3",
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

                var stream = ResourceLoader.Load<AudioStream>(kvp.Value.ResourcePath);
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

            string[] radCues = { AudioCueCatalog.RadAlertAcute, AudioCueCatalog.RadAlertChronic, AudioCueCatalog.RadGeigerBurst };
            foreach (string cueId in radCues)
                Check($"Radiation cue '{cueId}' exists", AudioCueCatalog.Contains(cueId), ref pass, ref fail);

            string[] weatherCues = { AudioCueCatalog.WeatherAlert, AudioCueCatalog.WeatherFalloutStorm, AudioCueCatalog.WeatherBlackRain, AudioCueCatalog.WeatherBlizzard };
            foreach (string cueId in weatherCues)
                Check($"Weather cue '{cueId}' exists", AudioCueCatalog.Contains(cueId), ref pass, ref fail);

            string[] shelterCues = { AudioCueCatalog.ShelterDoorOpen, AudioCueCatalog.ShelterDoorSeal, AudioCueCatalog.ShelterVentilation, AudioCueCatalog.ShelterAirFilter };
            foreach (string cueId in shelterCues)
                Check($"Shelter cue '{cueId}' exists", AudioCueCatalog.Contains(cueId), ref pass, ref fail);

            string[] actionCues = { AudioCueCatalog.ActionItemPickup, AudioCueCatalog.ActionCrafting, AudioCueCatalog.ActionRepair, AudioCueCatalog.ActionTrade };
            foreach (string cueId in actionCues)
                Check($"Action cue '{cueId}' exists", AudioCueCatalog.Contains(cueId), ref pass, ref fail);

            string[] radioCues = { AudioCueCatalog.RadioStatic, AudioCueCatalog.RadioTune, AudioCueCatalog.RadioSignalLock };
            foreach (string cueId in radioCues)
                Check($"Radio cue '{cueId}' exists", AudioCueCatalog.Contains(cueId), ref pass, ref fail);

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

            var radiation = new RadiationSystem();
            var survivor = new SurvivorRadState { Id = "audio_selftest_survivor" };
            radiation.Register(survivor);
            bridge.BindRadiation(radiation);
            radiation.Expose(survivor, 100f, 1f);
            Check("Bridge maps acute radiation status to its alert cue",
                emittedCues.Count == 4 && emittedCues[3] == AudioCueCatalog.RadAlertAcute,
                ref pass, ref fail);
            radiation.SeedLifetimeExposure(survivor, RadiationSystem.ChronicLifetimeThreshold);
            Check("Bridge maps chronic radiation status to its alert cue",
                emittedCues.Count == 5 && emittedCues[4] == AudioCueCatalog.RadAlertChronic,
                ref pass, ref fail);

            bridge.Dispose();
            secondWeather.ForceWeather(WeatherKind.Blizzard);
            radiation.AdministerIodine(survivor);
            Check("Disposing the bridge detaches every domain handler",
                emittedCues.Count == 5, ref pass, ref fail);

            // ── Summary ─────────────────────────────────────────
            GD.Print($"[AudioSelfTest] --- SUMMARY ---");
            GD.Print($"[AudioSelfTest] Pass: {pass}, Fail: {fail}, Total: {pass + fail}");
            GD.Print($"[AudioSelfTest] Cues: {cueCount} ({resolved} resolved, {fallback} fallback, {silent} silent)");
            GD.Print($"[AudioSelfTest] Assets checked: {keyAssets.Length}");

            bool allPass = fail == 0 && (pass + fail) > 0;
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
}
