using System;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Settings
{
    /// <summary>
    /// Serializable user preferences DTO. Kept strictly separate from simulation save files.
    /// Engine-agnostic; no Godot or UnityEngine dependencies.
    /// </summary>
    [Serializable]
    public sealed class UserSettingsData
    {
        [JsonPropertyName("schema_version")]
        public int SchemaVersion { get; set; } = 1;

        // ── Display ───────────────────────────────────────────────────────
        [JsonPropertyName("window_mode")]
        public int WindowMode { get; set; } = 0; // 0: Windowed, 1: Borderless, 2: Fullscreen

        [JsonPropertyName("resolution_width")]
        public int ResolutionWidth { get; set; } = 1920;

        [JsonPropertyName("resolution_height")]
        public int ResolutionHeight { get; set; } = 1080;

        [JsonPropertyName("ui_scale")]
        public float UiScale { get; set; } = 1.0f; // 0.8 to 1.5 (valid 0.5 to 2.5)

        [JsonPropertyName("vsync")]
        public bool VSync { get; set; } = true;

        [JsonPropertyName("max_fps")]
        public int MaxFps { get; set; } = 60; // 30, 60, 120, 144, 0 (unlimited)

        // ── Audio ─────────────────────────────────────────────────────────
        [JsonPropertyName("master_volume")]
        public float MasterVolume { get; set; } = 1.0f; // 0.0 to 1.0

        [JsonPropertyName("music_volume")]
        public float MusicVolume { get; set; } = 0.8f;

        [JsonPropertyName("sfx_volume")]
        public float SfxVolume { get; set; } = 0.8f;

        [JsonPropertyName("radio_volume")]
        public float RadioVolume { get; set; } = 0.9f;

        [JsonPropertyName("ambience_volume")]
        public float AmbienceVolume { get; set; } = 0.7f;

        [JsonPropertyName("mute_all")]
        public bool MuteAll { get; set; } = false;

        // ── Accessibility & Readability ───────────────────────────────────
        [JsonPropertyName("locale")]
        public string Locale { get; set; } = "en"; // "en", "pseudo", etc.

        [JsonPropertyName("high_contrast")]
        public bool HighContrast { get; set; } = false;

        [JsonPropertyName("hazard_text_labels")]
        public bool HazardTextLabels { get; set; } = true;

        [JsonPropertyName("reduced_motion")]
        public bool ReducedMotion { get; set; } = false;

        [JsonPropertyName("large_fonts")]
        public bool LargeFonts { get; set; } = false;

        // ── Gameplay Preferences ──────────────────────────────────────────
        [JsonPropertyName("tutorial_mode")]
        public int TutorialMode { get; set; } = 0; // 0: All, 1: ContextualOnly, 2: Disabled

        [JsonPropertyName("confirm_end_day")]
        public bool ConfirmEndDay { get; set; } = true;

        [JsonPropertyName("verbose_radio_log")]
        public bool VerboseRadioLog { get; set; } = true;

        [JsonPropertyName("auto_save_on_day")]
        public bool AutoSaveOnDay { get; set; } = true;

        public UserSettingsData Clone()
        {
            return new UserSettingsData
            {
                SchemaVersion = SchemaVersion,
                WindowMode = WindowMode,
                ResolutionWidth = ResolutionWidth,
                ResolutionHeight = ResolutionHeight,
                UiScale = UiScale,
                VSync = VSync,
                MaxFps = MaxFps,
                MasterVolume = MasterVolume,
                MusicVolume = MusicVolume,
                SfxVolume = SfxVolume,
                RadioVolume = RadioVolume,
                AmbienceVolume = AmbienceVolume,
                MuteAll = MuteAll,
                Locale = Locale ?? "en",
                HighContrast = HighContrast,
                HazardTextLabels = HazardTextLabels,
                ReducedMotion = ReducedMotion,
                LargeFonts = LargeFonts,
                TutorialMode = TutorialMode,
                ConfirmEndDay = ConfirmEndDay,
                VerboseRadioLog = VerboseRadioLog,
                AutoSaveOnDay = AutoSaveOnDay
            };
        }
    }
}
