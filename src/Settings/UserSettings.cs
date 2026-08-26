using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Godot;

namespace AtomicWar.GodotApp.Settings
{
    /// <summary>
    /// Serializable user preferences DTO. Kept strictly separate from simulation save files.
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
        public float UiScale { get; set; } = 1.0f; // 0.8 to 1.5

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
        [JsonPropertyName("high_contrast")]
        public bool HighContrast { get; set; } = false;

        [JsonPropertyName("hazard_text_labels")]
        public bool HazardTextLabels { get; set; } = true;

        [JsonPropertyName("reduced_motion")]
        public bool ReducedMotion { get; set; } = false;

        [JsonPropertyName("large_fonts")]
        public bool LargeFonts { get; set; } = false;

        // ── Gameplay Preferences ──────────────────────────────────────────
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
                HighContrast = HighContrast,
                HazardTextLabels = HazardTextLabels,
                ReducedMotion = ReducedMotion,
                LargeFonts = LargeFonts,
                ConfirmEndDay = ConfirmEndDay,
                VerboseRadioLog = VerboseRadioLog,
                AutoSaveOnDay = AutoSaveOnDay
            };
        }
    }

    /// <summary>
    /// Manages loading, saving, recovery, and immediate engine application of UserSettings.
    /// </summary>
    public static class UserSettingsStore
    {
        private const string SettingsPath = "user://settings.json";
        private static readonly JsonSerializerOptions JsonOpts = new()
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true
        };

        private static UserSettingsData? _current;
        public static UserSettingsData Current => _current ??= Load();

        public static UserSettingsData Load(string path = SettingsPath)
        {
            string globalPath = ProjectSettings.GlobalizePath(path);
            if (!File.Exists(globalPath))
            {
                var defaults = new UserSettingsData();
                Save(defaults, path);
                return defaults;
            }

            try
            {
                string json = File.ReadAllText(globalPath);
                var data = JsonSerializer.Deserialize<UserSettingsData>(json, JsonOpts);
                if (data == null)
                {
                    GD.PrintErr($"[UserSettingsStore] Deserialized null from {path}, falling back to defaults.");
                    return new UserSettingsData();
                }
                return data;
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[UserSettingsStore] Failed to load settings from {path} ({ex.Message}), recovering with defaults.");
                return new UserSettingsData();
            }
        }

        public static bool Save(UserSettingsData data, string path = SettingsPath)
        {
            if (data == null) return false;
            _current = data.Clone();

            string globalPath = ProjectSettings.GlobalizePath(path);
            string tempPath = globalPath + ".tmp";

            try
            {
                string? dir = Path.GetDirectoryName(globalPath);
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                string json = JsonSerializer.Serialize(data, JsonOpts);
                File.WriteAllText(tempPath, json);

                if (File.Exists(globalPath))
                {
                    File.Replace(tempPath, globalPath, null);
                }
                else
                {
                    File.Move(tempPath, globalPath);
                }
                return true;
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[UserSettingsStore] Failed to save settings to {path}: {ex.Message}");
                try { if (File.Exists(tempPath)) File.Delete(tempPath); } catch (Exception cleanupEx) { GD.PrintErr($"[UserSettings] Failed to clean temp file: {cleanupEx.Message}"); }
                return false;
            }
        }

        /// <summary>
        /// Applies settings immediately to Godot's audio buses, display server, and engine limits.
        /// Safely catches headless/unsupported display exceptions.
        /// </summary>
        public static void Apply(UserSettingsData data)
        {
            if (data == null) return;
            _current = data.Clone();

            // 1. Audio Application
            ApplyAudio("Master", data.MasterVolume, data.MuteAll);
            ApplyAudio("Music", data.MusicVolume, data.MuteAll);
            ApplyAudio("SFX", data.SfxVolume, data.MuteAll);
            ApplyAudio("Radio", data.RadioVolume, data.MuteAll);
            ApplyAudio("Ambience", data.AmbienceVolume, data.MuteAll);

            // 2. Engine FPS Cap
            Engine.MaxFps = Math.Max(0, data.MaxFps);

            // 3. Display Application (Safe for headless)
            try
            {
                if (DisplayServer.GetName() != "headless")
                {
                    // Window Mode
                    DisplayServer.WindowMode mode = data.WindowMode switch
                    {
                        1 => DisplayServer.WindowMode.ExclusiveFullscreen,
                        2 => DisplayServer.WindowMode.Fullscreen,
                        _ => DisplayServer.WindowMode.Windowed
                    };

                    if (DisplayServer.WindowGetMode() != mode)
                    {
                        DisplayServer.WindowSetMode(mode);
                    }

                    // Resolution (in windowed mode)
                    if (mode == DisplayServer.WindowMode.Windowed && data.ResolutionWidth > 0 && data.ResolutionHeight > 0)
                    {
                        DisplayServer.WindowSetSize(new Vector2I(data.ResolutionWidth, data.ResolutionHeight));
                    }

                    // VSync
                    DisplayServer.VSyncMode vsync = data.VSync
                        ? DisplayServer.VSyncMode.Enabled
                        : DisplayServer.VSyncMode.Disabled;
                    DisplayServer.WindowSetVsyncMode(vsync);
                }
            }
            catch (Exception ex)
            {
                GD.Print($"[UserSettingsStore] Display apply notice (headless/unsupported): {ex.Message}");
            }
        }

        private static void ApplyAudio(string busName, float linearVolume, bool muteAll)
        {
            int busIdx = AudioServer.GetBusIndex(busName);
            if (busIdx < 0) return;

            float clamped = Math.Clamp(linearVolume, 0f, 1f);
            float db = clamped <= 0.0001f ? -80f : Mathf.LinearToDb(clamped);
            AudioServer.SetBusVolumeDb(busIdx, db);
            AudioServer.SetBusMute(busIdx, muteAll || clamped <= 0.0001f);
        }
    }
}
