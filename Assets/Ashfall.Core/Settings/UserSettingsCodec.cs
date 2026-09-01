using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Ashfall.Core.Settings
{
    /// <summary>
    /// Engine-agnostic codec and recovery validator for UserSettings JSON data.
    /// Guarantees that malformed, corrupted, truncated, or out-of-range user settings
    /// always recover to safe defaults with informative diagnostic logging without throwing.
    /// </summary>
    public static class UserSettingsCodec
    {
        public static readonly JsonSerializerOptions JsonOpts = new()
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true
        };

        /// <summary>
        /// Safely deserializes settings JSON with complete recovery fallback.
        /// Never throws; preserves any warning/diagnostic message in the return tuple.
        /// </summary>
        public static (UserSettingsData Data, string? DiagnosticMessage) DeserializeWithRecovery(string? json)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                return (new UserSettingsData(), "[UserSettingsCodec] Settings JSON is empty or whitespace. Recovered with safe defaults.");
            }

            try
            {
                var data = JsonSerializer.Deserialize<UserSettingsData>(json, JsonOpts);
                if (data == null)
                {
                    return (new UserSettingsData(), "[UserSettingsCodec] Deserialization returned null. Recovered with safe defaults.");
                }

                data = Sanitize(data, out string? sanitizeMsg);
                return (data, sanitizeMsg);
            }
            catch (Exception ex)
            {
                return (new UserSettingsData(), $"[UserSettingsCodec] Invalid settings JSON ({ex.Message}). Recovered with safe defaults.");
            }
        }

        /// <summary>
        /// Sanitizes all settings fields, clamping out-of-range values to valid boundaries.
        /// </summary>
        public static UserSettingsData Sanitize(UserSettingsData? data, out string? diagnosticMessage)
        {
            if (data == null)
            {
                diagnosticMessage = "[UserSettingsCodec] Null data provided for sanitization. Returning fresh defaults.";
                return new UserSettingsData();
            }

            var warnings = new List<string>();

            // Window Mode (0=Windowed, 1=Borderless, 2=Fullscreen)
            if (data.WindowMode < 0 || data.WindowMode > 2)
            {
                warnings.Add($"WindowMode {data.WindowMode} clamped to 0 (Windowed)");
                data.WindowMode = 0;
            }

            // Resolution
            if (data.ResolutionWidth < 640 || data.ResolutionWidth > 7680)
            {
                warnings.Add($"ResolutionWidth {data.ResolutionWidth} clamped to 1920");
                data.ResolutionWidth = 1920;
            }
            if (data.ResolutionHeight < 480 || data.ResolutionHeight > 4320)
            {
                warnings.Add($"ResolutionHeight {data.ResolutionHeight} clamped to 1080");
                data.ResolutionHeight = 1080;
            }

            // UI Scale
            if (float.IsNaN(data.UiScale) || float.IsInfinity(data.UiScale) || data.UiScale < 0.5f || data.UiScale > 2.5f)
            {
                warnings.Add($"UiScale {data.UiScale} clamped to 1.0");
                data.UiScale = 1.0f;
            }

            // Locale
            if (string.IsNullOrWhiteSpace(data.Locale))
            {
                data.Locale = "en";
            }
            else
            {
                data.Locale = data.Locale.Trim().ToLowerInvariant();
                if (data.Locale != "en" && data.Locale != "pseudo")
                {
                    warnings.Add($"Locale '{data.Locale}' not recognized; defaulting to 'en'");
                    data.Locale = "en";
                }
            }

            // Tutorial Mode (0=All, 1=ContextualOnly, 2=Disabled)
            if (data.TutorialMode < 0 || data.TutorialMode > 2)
            {
                warnings.Add($"TutorialMode {data.TutorialMode} clamped to 0");
                data.TutorialMode = 0;
            }

            // Max FPS (0 is valid for unlimited, otherwise capped at 360)
            if (data.MaxFps < 0 || data.MaxFps > 360)
            {
                warnings.Add($"MaxFps {data.MaxFps} clamped to 60");
                data.MaxFps = 60;
            }

            // Audio Volumes
            data.MasterVolume = SanitizeVolume("MasterVolume", data.MasterVolume, 1.0f, warnings);
            data.MusicVolume = SanitizeVolume("MusicVolume", data.MusicVolume, 0.8f, warnings);
            data.SfxVolume = SanitizeVolume("SfxVolume", data.SfxVolume, 0.8f, warnings);
            data.RadioVolume = SanitizeVolume("RadioVolume", data.RadioVolume, 0.9f, warnings);
            data.AmbienceVolume = SanitizeVolume("AmbienceVolume", data.AmbienceVolume, 0.7f, warnings);

            diagnosticMessage = warnings.Count > 0
                ? "[UserSettingsCodec] Sanitized settings: " + string.Join("; ", warnings)
                : null;

            return data;
        }

        private static float SanitizeVolume(string name, float volume, float defaultVal, List<string> warnings)
        {
            if (float.IsNaN(volume) || float.IsInfinity(volume))
            {
                warnings.Add($"{name} (non-finite) reset to {defaultVal:0.0}");
                return defaultVal;
            }
            if (volume < 0.0f || volume > 1.0f)
            {
                float clamped = Math.Clamp(volume, 0.0f, 1.0f);
                warnings.Add($"{name} {volume} clamped to {clamped:0.0}");
                return clamped;
            }
            return volume;
        }

        /// <summary>
        /// Serializes settings data to clean formatted JSON after sanitization.
        /// </summary>
        public static string Serialize(UserSettingsData data)
        {
            var sanitized = Sanitize(data, out _);
            return JsonSerializer.Serialize(sanitized, JsonOpts);
        }
    }
}
