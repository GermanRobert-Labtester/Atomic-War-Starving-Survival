using System;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Audio
{
    /// <summary>
    /// Engine-agnostic serializable audio settings DTO.
    /// Tracks volume percentages (0..100) and mute states across all audio buses.
    /// </summary>
    [Serializable]
    public sealed class AudioSettingsData
    {
        public const int CurrentVersion = 1;

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

        public AudioSettingsData Clone()
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

        public void ResetToDefaults()
        {
            var defaults = new AudioSettingsData();
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
        }

        public static float ClampVolume(float value) => Math.Clamp(value, 0f, 100f);

        /// <summary>
        /// Convert 0-100 percent to dB (-80 = silent, 0 = full).
        /// </summary>
        public static float PercentToDb(float percent)
        {
            float linear = ClampVolume(percent) / 100f;
            return linear > 0.001f ? (float)(20.0 * Math.Log10(linear)) : -80f;
        }

        /// <summary>
        /// Effective volume for a category (0..1), accounting for master volume and mute.
        /// </summary>
        public float GetEffectiveVolume(float categoryVolume, bool categoryMute)
        {
            if (MasterMute || categoryMute) return 0f;
            return (MasterVolume / 100f) * (categoryVolume / 100f);
        }
    }
}
