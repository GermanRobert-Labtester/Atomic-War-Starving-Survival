using Godot;
using System;
using System.IO;
using System.Text.Json.Serialization;
using Ashfall.Core.Audio;

namespace AtomicWar.GodotApp.Audio
{
    /// <summary>
    /// Versioned, persisted audio user preferences.
    /// Separate from gameplay saves. Stored at user://audio_settings.json.
    /// Hardened for malformed-file recovery: restores defaults without exception,
    /// while preserving valid values whenever partial parsing is possible.
    /// </summary>
    public sealed class AudioSettings
    {
        public const int CurrentVersion = AudioSettingsData.CurrentVersion;
        private const string FileName = "audio_settings.json";

        private static AudioSettings? _instance;
        private static string? _lastDiagnosticMessage;

        public static AudioSettings Instance => _instance ??= Load();

        /// <summary>
        /// Diagnostic message from the most recent load/save/recovery operation, or null if clean.
        /// </summary>
        public static string? LastDiagnosticMessage => _lastDiagnosticMessage;

        /// <summary>
        /// True if the most recent load or save encountered an issue and had to recover.
        /// </summary>
        public static bool HasDiagnosticError => !string.IsNullOrEmpty(_lastDiagnosticMessage);

        public static void ClearDiagnosticMessage() => _lastDiagnosticMessage = null;

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

        public static string DefaultSavePath => Path.Combine(
            ProjectSettings.GlobalizePath("user://"), FileName);

        // ── Load / Save ─────────────────────────────────────────

        public static AudioSettings Load(string? customPath = null)
        {
            string path = customPath ?? DefaultSavePath;
            if (!File.Exists(path))
            {
                _lastDiagnosticMessage = null;
                return new AudioSettings();
            }

            try
            {
                string json = File.ReadAllText(path);
                var (data, diag) = AudioSettingsCodec.DeserializeWithRecovery(json);
                _lastDiagnosticMessage = diag;
                if (!string.IsNullOrEmpty(diag))
                {
                    GD.PrintErr($"[AudioSettings] {diag}");
                }

                var settings = FromData(data);
                if (settings.Version < CurrentVersion)
                    Migrate(settings);

                return settings;
            }
            catch (Exception e)
            {
                _lastDiagnosticMessage = $"[AudioSettings] Failed to read audio settings from '{path}' ({e.Message}). Restored defaults.";
                GD.PrintErr(_lastDiagnosticMessage);
                return new AudioSettings();
            }
        }

        public void Save(string? customPath = null)
        {
            string path = customPath ?? DefaultSavePath;
            try
            {
                string dir = Path.GetDirectoryName(path)!;
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                // Atomic write: write to temp, then rename
                string tempPath = path + ".tmp";
                var data = ToData();
                string json = AudioSettingsCodec.Serialize(data);
                File.WriteAllText(tempPath, json);
                File.Move(tempPath, path, overwrite: true);
                _lastDiagnosticMessage = null;
            }
            catch (Exception e)
            {
                _lastDiagnosticMessage = $"[AudioSettings] Save failed to '{path}': {e.Message}";
                GD.PrintErr(_lastDiagnosticMessage);
            }
        }

        public AudioSettingsData ToData()
        {
            return new AudioSettingsData
            {
                Version = Version,
                MasterVolume = MasterVolume,
                MusicVolume = MusicVolume,
                AmbienceVolume = AmbienceVolume,
                SfxVolume = SfxVolume,
                UiVolume = UiVolume,
                VoiceVolume = VoiceVolume,
                AlertVolume = AlertVolume,
                GeneratorVolume = GeneratorVolume,
                VentilationVolume = VentilationVolume,
                RadioVolume = RadioVolume,
                MedicalVolume = MedicalVolume,
                SurfaceVolume = SurfaceVolume,
                MasterMute = MasterMute,
                MusicMute = MusicMute,
                SfxMute = SfxMute,
                VoiceMute = VoiceMute,
                AlertMute = AlertMute,
                AmbienceMute = AmbienceMute,
                UiMute = UiMute,
                GeneratorMute = GeneratorMute,
                VentilationMute = VentilationMute,
                RadioMute = RadioMute,
                MedicalMute = MedicalMute,
                SurfaceMute = SurfaceMute
            };
        }

        public static AudioSettings FromData(AudioSettingsData data)
        {
            return new AudioSettings
            {
                Version = data.Version,
                MasterVolume = data.MasterVolume,
                MusicVolume = data.MusicVolume,
                AmbienceVolume = data.AmbienceVolume,
                SfxVolume = data.SfxVolume,
                UiVolume = data.UiVolume,
                VoiceVolume = data.VoiceVolume,
                AlertVolume = data.AlertVolume,
                GeneratorVolume = data.GeneratorVolume,
                VentilationVolume = data.VentilationVolume,
                RadioVolume = data.RadioVolume,
                MedicalVolume = data.MedicalVolume,
                SurfaceVolume = data.SurfaceVolume,
                MasterMute = data.MasterMute,
                MusicMute = data.MusicMute,
                SfxMute = data.SfxMute,
                VoiceMute = data.VoiceMute,
                AlertMute = data.AlertMute,
                AmbienceMute = data.AmbienceMute,
                UiMute = data.UiMute,
                GeneratorMute = data.GeneratorMute,
                VentilationMute = data.VentilationMute,
                RadioMute = data.RadioMute,
                MedicalMute = data.MedicalMute,
                SurfaceMute = data.SurfaceMute
            };
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

        public static float ClampVolume(float value) => AudioSettingsData.ClampVolume(value);

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
