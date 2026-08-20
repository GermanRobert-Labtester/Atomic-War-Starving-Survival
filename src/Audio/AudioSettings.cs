using Godot;
using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AtomicWar.GodotApp.Audio
{
    /// <summary>
    /// Versioned, persisted audio user preferences.
    /// Separate from gameplay saves. Stored at user://audio_settings.json.
    /// Atomic writes, malformed-file recovery, defaults, reset-to-default.
    /// </summary>
    public sealed class AudioSettings
    {
        public const int CurrentVersion = 1;
        private const string FileName = "audio_settings.json";

        private static AudioSettings? _instance;
        public static AudioSettings Instance => _instance ??= Load();

        // ── Persisted fields ────────────────────────────────────

        [JsonPropertyName("version")]
        public int Version { get; set; } = CurrentVersion;

        [JsonPropertyName("master_volume")]
        public float MasterVolume { get; set; } = 100f;

        [JsonPropertyName("music_volume")]
        public float MusicVolume { get; set; } = 70f;

        [JsonPropertyName("ambience_volume")]
        public float AmbienceVolume { get; set; } = 60f;

        [JsonPropertyName("sfx_volume")]
        public float SfxVolume { get; set; } = 80f;

        [JsonPropertyName("ui_volume")]
        public float UiVolume { get; set; } = 50f;

        [JsonPropertyName("voice_volume")]
        public float VoiceVolume { get; set; } = 90f;

        [JsonPropertyName("alert_volume")]
        public float AlertVolume { get; set; } = 100f;

        [JsonPropertyName("generator_volume")]
        public float GeneratorVolume { get; set; } = 70f;

        [JsonPropertyName("ventilation_volume")]
        public float VentilationVolume { get; set; } = 60f;

        [JsonPropertyName("radio_volume")]
        public float RadioVolume { get; set; } = 80f;

        [JsonPropertyName("medical_volume")]
        public float MedicalVolume { get; set; } = 70f;

        [JsonPropertyName("surface_volume")]
        public float SurfaceVolume { get; set; } = 50f;

        [JsonPropertyName("master_mute")]
        public bool MasterMute { get; set; }

        [JsonPropertyName("music_mute")]
        public bool MusicMute { get; set; }

        [JsonPropertyName("sfx_mute")]
        public bool SfxMute { get; set; }

        [JsonPropertyName("voice_mute")]
        public bool VoiceMute { get; set; }

        [JsonPropertyName("alert_mute")]
        public bool AlertMute { get; set; }

        [JsonPropertyName("ambience_mute")]
        public bool AmbienceMute { get; set; }

        [JsonPropertyName("ui_mute")]
        public bool UiMute { get; set; }

        [JsonPropertyName("generator_mute")]
        public bool GeneratorMute { get; set; }

        [JsonPropertyName("ventilation_mute")]
        public bool VentilationMute { get; set; }

        [JsonPropertyName("radio_mute")]
        public bool RadioMute { get; set; }

        [JsonPropertyName("medical_mute")]
        public bool MedicalMute { get; set; }

        [JsonPropertyName("surface_mute")]
        public bool SurfaceMute { get; set; }

        // ── Non-persisted ───────────────────────────────────────

        public event Action? OnSettingsChanged;

        // ── Paths ───────────────────────────────────────────────

        private static string SavePath => Path.Combine(
            ProjectSettings.GlobalizePath("user://"), FileName);

        // ── Load / Save ─────────────────────────────────────────

        public static AudioSettings Load()
        {
            string path = SavePath;
            if (!File.Exists(path))
                return new AudioSettings();

            try
            {
                string json = File.ReadAllText(path);
                var settings = JsonSerializer.Deserialize<AudioSettings>(json);
                if (settings == null)
                    return new AudioSettings();

                // Version migration
                if (settings.Version < CurrentVersion)
                    Migrate(settings);

                return settings;
            }
            catch (Exception e)
            {
                GD.PrintErr($"[AudioSettings] Malformed file, using defaults: {e.Message}");
                return new AudioSettings();
            }
        }

        public void Save()
        {
            try
            {
                string path = SavePath;
                string dir = Path.GetDirectoryName(path)!;
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                // Atomic write: write to temp, then rename
                string tempPath = path + ".tmp";
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
                };
                string json = JsonSerializer.Serialize(this, options);
                File.WriteAllText(tempPath, json);
                File.Move(tempPath, path, overwrite: true);
            }
            catch (Exception e)
            {
                GD.PrintErr($"[AudioSettings] Save failed: {e.Message}");
            }
        }

        public void ResetToDefaults()
        {
            var defaults = new AudioSettings();
            MasterVolume = defaults.MasterVolume;
            MusicVolume = defaults.MusicVolume;
            AmbienceVolume = defaults.AmbienceVolume;
            SfxVolume = defaults.SfxVolume;
            UiVolume = defaults.UiVolume;
            VoiceVolume = defaults.VoiceVolume;
            AlertVolume = defaults.AlertVolume;
            GeneratorVolume = defaults.GeneratorVolume;
            VentilationVolume = defaults.VentilationVolume;
            RadioVolume = defaults.RadioVolume;
            MedicalVolume = defaults.MedicalVolume;
            SurfaceVolume = defaults.SurfaceVolume;
            MasterMute = false;
            MusicMute = false;
            AmbienceMute = false;
            SfxMute = false;
            UiMute = false;
            VoiceMute = false;
            AlertMute = false;
            GeneratorMute = false;
            VentilationMute = false;
            RadioMute = false;
            MedicalMute = false;
            SurfaceMute = false;
            OnSettingsChanged?.Invoke();
        }

        public void NotifyChanged()
        {
            OnSettingsChanged?.Invoke();
        }

        // ── Migration ───────────────────────────────────────────

        private static void Migrate(AudioSettings settings)
        {
            // V0 → V1: no structural changes, just stamp version
            settings.Version = CurrentVersion;
        }

        // ── Volume helpers ──────────────────────────────────────

        public static float ClampVolume(float value) =>
            Math.Clamp(value, 0f, 100f);

        /// <summary>
        /// Convert 0-100 percent to Godot dB (-80 = silent, 0 = full).
        /// </summary>
        public static float PercentToDb(float percent)
        {
            float linear = ClampVolume(percent) / 100f;
            return linear > 0.001f ? Mathf.LinearToDb(linear) : -80f;
        }

        /// <summary>
        /// Effective volume for a category, accounting for master volume and mute.
        /// </summary>
        public float GetEffectiveVolume(float categoryVolume, bool categoryMute)
        {
            if (MasterMute || categoryMute) return 0f;
            return (MasterVolume / 100f) * (categoryVolume / 100f);
        }
    }
}
