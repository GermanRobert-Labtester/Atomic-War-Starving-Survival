using Godot;
using System;
using System.IO;
using System.Text.Json;

namespace AtomicWar.GodotApp.Audio
{
    /// <summary>
    /// Headless self-test for the audio system.
    /// Verifies: cue catalog, settings persistence, bus topology, resource resolution.
    /// Run with: godot --headless --path . -- --audio-selftest
    /// </summary>
    public static class AudioSelfTest
    {
        public static int Run()
        {
            GD.Print("[AudioSelfTest] Starting...");
            int pass = 0, fail = 0;

            // 1. Cue catalog
            GD.Print("[AudioSelfTest] --- Cue Catalog ---");
            int cueCount = AudioCueCatalog.Count;
            Check($"Cue catalog has entries", cueCount > 0, ref pass, ref fail);
            Check($"Cue catalog has 45+ cues", cueCount >= 45, ref pass, ref fail);

            // Verify key cues exist
            string[] requiredCues = {
                AudioCueCatalog.UiClick, AudioCueCatalog.UiConfirm, AudioCueCatalog.UiWarning,
                AudioCueCatalog.RadAlertAcute, AudioCueCatalog.WeatherAlert,
                AudioCueCatalog.AmbBunker, AudioCueCatalog.MusicMenu,
                AudioCueCatalog.RadioStatic, AudioCueCatalog.ShelterDoorOpen,
                AudioCueCatalog.ActionItemPickup, AudioCueCatalog.DangerAlarmKlaxon,
            };
            foreach (string cueId in requiredCues)
            {
                var cue = AudioCueCatalog.Resolve(cueId);
                Check($"Cue '{cueId}' resolves", cue != null, ref pass, ref fail);
                if (cue != null)
                {
                    Check($"Cue '{cueId}' has resource path", !string.IsNullOrEmpty(cue.ResourcePath), ref pass, ref fail);
                    Check($"Cue '{cueId}' has valid bus", !string.IsNullOrEmpty(cue.Bus), ref pass, ref fail);
                }
            }

            // Unknown cue returns null
            Check("Unknown cue returns null", AudioCueCatalog.Resolve("nonexistent_cue") == null, ref pass, ref fail);
            Check("Empty cue returns null", AudioCueCatalog.Resolve("") == null, ref pass, ref fail);
            Check("Null cue returns null", AudioCueCatalog.Resolve(null!) == null, ref pass, ref fail);

            // 2. Audio settings
            GD.Print("[AudioSelfTest] --- Audio Settings ---");
            var defaults = new AudioSettings();
            Check("Default master volume is 100", defaults.MasterVolume == 100f, ref pass, ref fail);
            Check("Default music volume is 70", defaults.MusicVolume == 70f, ref pass, ref fail);
            Check("Default version is current", defaults.Version == AudioSettings.CurrentVersion, ref pass, ref fail);
            Check("Default no mutes", !defaults.MasterMute && !defaults.MusicMute && !defaults.SfxMute, ref pass, ref fail);

            // Settings round-trip
            var testSettings = new AudioSettings { MasterVolume = 50f, MusicVolume = 30f, MusicMute = true };
            string json = JsonSerializer.Serialize(testSettings);
            var restored = JsonSerializer.Deserialize<AudioSettings>(json);
            Check("Settings round-trip preserves volume", restored != null && restored.MasterVolume == 50f, ref pass, ref fail);
            Check("Settings round-trip preserves mute", restored != null && restored.MusicMute, ref pass, ref fail);

            // Malformed JSON recovery
            var recovered = AudioSettings.Load(); // Should not throw even if file is missing/malformed
            Check("Settings load returns non-null", recovered != null, ref pass, ref fail);

            // Volume helpers
            Check("PercentToDb(100) ≈ 0", Math.Abs(AudioSettings.PercentToDb(100f)) < 0.1f, ref pass, ref fail);
            Check("PercentToDb(0) = -80", AudioSettings.PercentToDb(0f) == -80f, ref pass, ref fail);
            Check("ClampVolume(-10) = 0", AudioSettings.ClampVolume(-10f) == 0f, ref pass, ref fail);
            Check("ClampVolume(150) = 100", AudioSettings.ClampVolume(150f) == 100f, ref pass, ref fail);

            // Effective volume
            var s = new AudioSettings { MasterVolume = 80f, MasterMute = false };
            float eff = s.GetEffectiveVolume(50f, false);
            Check("Effective volume = master * category", Math.Abs(eff - 0.4f) < 0.01f, ref pass, ref fail);
            float effMuted = s.GetEffectiveVolume(50f, true);
            Check("Effective volume muted = 0", effMuted == 0f, ref pass, ref fail);
            s.MasterMute = true;
            float effMasterMuted = s.GetEffectiveVolume(50f, false);
            Check("Master mute overrides category", effMasterMuted == 0f, ref pass, ref fail);

            // 3. Resource resolution (check that key asset files exist on disk)
            GD.Print("[AudioSelfTest] --- Resource Resolution ---");
            string[] keyAssets = {
                "res://assets/audio/ui/ui_click.wav",
                "res://assets/audio/ui/ui_confirm.wav",
                "res://assets/audio/ui/ui_warning.wav",
                "res://assets/audio/sfx/radiation_alert.wav",
                "res://assets/audio/sfx/weather_alert.wav",
                "res://assets/audio/sfx/geiger.wav",
                "res://assets/audio/ambience/bunker_ambience.wav",
                "res://assets/audio/ambience/surface_ambience.wav",
                "res://assets/audio/music/main_menu.wav",
                "res://assets/audio/music/gameplay_underscore.wav",
                "res://assets/audio/radio/radio_static_hiss.wav",
            };
            foreach (string resPath in keyAssets)
            {
                bool exists = ResourceLoader.Exists(resPath);
                if (!exists)
                {
                    // Fallback: check filesystem
                    string osPath = ProjectSettings.GlobalizePath(resPath);
                    exists = File.Exists(osPath);
                }
                Check($"Asset exists: {Path.GetFileName(resPath)}", exists, ref pass, ref fail);
            }

            // 4. Bus topology
            GD.Print("[AudioSelfTest] --- Bus Topology ---");
            string[] expectedBuses = { "Master", "Music", "Ambience", "SFX", "UI", "Voice", "Alerts" };
            // Note: buses are created at runtime by AudioManager._Ready(), so in selftest
            // they may not exist yet. We verify the catalog references valid bus names instead.
            foreach (string cueId in requiredCues)
            {
                var cue = AudioCueCatalog.Resolve(cueId);
                if (cue != null)
                {
                    bool validBus = false;
                    foreach (string b in expectedBuses)
                        if (cue.Bus == b) { validBus = true; break; }
                    Check($"Cue '{cueId}' uses valid bus '{cue.Bus}'", validBus, ref pass, ref fail);
                }
            }

            // 5. Cooldown behavior (test the concept, not the runtime)
            GD.Print("[AudioSelfTest] --- Cooldown/Dedup ---");
            var cooldownCues = new[] { AudioCueCatalog.RadAlertAcute, AudioCueCatalog.WeatherAlert, AudioCueCatalog.RadioStatic };
            foreach (string cueId in cooldownCues)
            {
                var cue = AudioCueCatalog.Resolve(cueId);
                Check($"Alert cue '{cueId}' has cooldown > 0", cue != null && cue.CooldownSeconds > 0, ref pass, ref fail);
            }

            // Summary
            GD.Print($"[AudioSelfTest] --- SUMMARY ---");
            GD.Print($"[AudioSelfTest] Pass: {pass}, Fail: {fail}, Total: {pass + fail}");
            GD.Print($"[AudioSelfTest] Cues: {cueCount}, Assets checked: {keyAssets.Length}");

            bool allPass = fail == 0;
            GD.Print(allPass ? "AUDIO_SELFTEST PASS" : $"AUDIO_SELFTEST FAIL (fail={fail})");
            return allPass ? 0 : 1;
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
