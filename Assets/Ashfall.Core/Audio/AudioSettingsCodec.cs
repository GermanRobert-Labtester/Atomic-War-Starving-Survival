using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Ashfall.Core.Audio
{
    /// <summary>
    /// Codec and resilient recovery parser for audio settings.
    /// Restores defaults without throwing on malformed or missing settings,
    /// while preserving valid values whenever partial parsing is possible.
    /// </summary>
    public static class AudioSettingsCodec
    {
        public static readonly JsonSerializerOptions JsonOpts = new()
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true
        };

        /// <summary>
        /// Deserializes audio settings JSON with resilient recovery.
        /// Preserves valid fields even when other fields are corrupt or invalid types.
        /// </summary>
        public static (AudioSettingsData Data, string? DiagnosticMessage) DeserializeWithRecovery(string? json)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                return (new AudioSettingsData(), "[AudioSettingsCodec] Settings JSON is empty or null. Restored safe defaults.");
            }

            // 1. Try standard fast deserialization
            try
            {
                var data = JsonSerializer.Deserialize<AudioSettingsData>(json, JsonOpts);
                if (data != null)
                {
                    data = Sanitize(data, out string? sanitizeMsg);
                    return (data, sanitizeMsg);
                }
            }
            catch
            {
                // Fall through to resilient element-by-element parsing
            }

            // 2. Resilient partial parsing: preserve valid values, restore defaults for invalid fields
            try
            {
                using var doc = JsonDocument.Parse(json);
                if (doc.RootElement.ValueKind != JsonValueKind.Object)
                {
                    return (new AudioSettingsData(), "[AudioSettingsCodec] JSON root is not an object. Restored safe defaults.");
                }

                var data = new AudioSettingsData();
                var warnings = new List<string>();

                // Parse volume numbers
                data.MasterVolume = ExtractVolume(doc.RootElement, "master_volume", data.MasterVolume, warnings);
                data.MusicVolume = ExtractVolume(doc.RootElement, "music_volume", data.MusicVolume, warnings);
                data.AmbienceVolume = ExtractVolume(doc.RootElement, "ambience_volume", data.AmbienceVolume, warnings);
                data.SfxVolume = ExtractVolume(doc.RootElement, "sfx_volume", data.SfxVolume, warnings);
                data.UiVolume = ExtractVolume(doc.RootElement, "ui_volume", data.UiVolume, warnings);
                data.VoiceVolume = ExtractVolume(doc.RootElement, "voice_volume", data.VoiceVolume, warnings);
                data.AlertVolume = ExtractVolume(doc.RootElement, "alert_volume", data.AlertVolume, warnings);
                data.GeneratorVolume = ExtractVolume(doc.RootElement, "generator_volume", data.GeneratorVolume, warnings);
                data.VentilationVolume = ExtractVolume(doc.RootElement, "ventilation_volume", data.VentilationVolume, warnings);
                data.RadioVolume = ExtractVolume(doc.RootElement, "radio_volume", data.RadioVolume, warnings);
                data.MedicalVolume = ExtractVolume(doc.RootElement, "medical_volume", data.MedicalVolume, warnings);
                data.SurfaceVolume = ExtractVolume(doc.RootElement, "surface_volume", data.SurfaceVolume, warnings);

                // Parse mute booleans
                data.MasterMute = ExtractBool(doc.RootElement, "master_mute", data.MasterMute, warnings);
                data.MusicMute = ExtractBool(doc.RootElement, "music_mute", data.MusicMute, warnings);
                data.SfxMute = ExtractBool(doc.RootElement, "sfx_mute", data.SfxMute, warnings);
                data.VoiceMute = ExtractBool(doc.RootElement, "voice_mute", data.VoiceMute, warnings);
                data.AlertMute = ExtractBool(doc.RootElement, "alert_mute", data.AlertMute, warnings);
                data.AmbienceMute = ExtractBool(doc.RootElement, "ambience_mute", data.AmbienceMute, warnings);
                data.UiMute = ExtractBool(doc.RootElement, "ui_mute", data.UiMute, warnings);
                data.GeneratorMute = ExtractBool(doc.RootElement, "generator_mute", data.GeneratorMute, warnings);
                data.VentilationMute = ExtractBool(doc.RootElement, "ventilation_mute", data.VentilationMute, warnings);
                data.RadioMute = ExtractBool(doc.RootElement, "radio_mute", data.RadioMute, warnings);
                data.MedicalMute = ExtractBool(doc.RootElement, "medical_mute", data.MedicalMute, warnings);
                data.SurfaceMute = ExtractBool(doc.RootElement, "surface_mute", data.SurfaceMute, warnings);

                data = Sanitize(data, out string? sanitizeMsg);

                string diag = warnings.Count > 0
                    ? "[AudioSettingsCodec] Partially recovered settings: " + string.Join("; ", warnings)
                    : sanitizeMsg ?? "[AudioSettingsCodec] Recovered settings with valid values.";

                return (data, diag);
            }
            catch (Exception ex)
            {
                return (new AudioSettingsData(), $"[AudioSettingsCodec] Malformed JSON syntax ({ex.Message}). Restored safe defaults.");
            }
        }

        private static float ExtractVolume(JsonElement root, string propName, float defaultVal, List<string> warnings)
        {
            if (root.TryGetProperty(propName, out var elem))
            {
                if (elem.ValueKind == JsonValueKind.Number && elem.TryGetSingle(out float val))
                {
                    if (float.IsNaN(val) || float.IsInfinity(val))
                    {
                        warnings.Add($"{propName} (non-finite) reset to {defaultVal}");
                        return defaultVal;
                    }
                    return Math.Clamp(val, 0f, 100f);
                }
                warnings.Add($"Invalid {propName} type '{elem.ValueKind}', reset to default {defaultVal}");
            }
            return defaultVal;
        }

        private static bool ExtractBool(JsonElement root, string propName, bool defaultVal, List<string> warnings)
        {
            if (root.TryGetProperty(propName, out var elem))
            {
                if (elem.ValueKind == JsonValueKind.True) return true;
                if (elem.ValueKind == JsonValueKind.False) return false;
                warnings.Add($"Invalid {propName} type '{elem.ValueKind}', reset to default {defaultVal}");
            }
            return defaultVal;
        }

        /// <summary>
        /// Sanitizes all volumes to [0..100] and cleans up non-finite numbers.
        /// </summary>
        public static AudioSettingsData Sanitize(AudioSettingsData? data, out string? diagnosticMessage)
        {
            if (data == null)
            {
                diagnosticMessage = "[AudioSettingsCodec] Null data provided for sanitization. Returning defaults.";
                return new AudioSettingsData();
            }

            var warnings = new List<string>();

            data.MasterVolume = SanitizeVolume("MasterVolume", data.MasterVolume, 100f, warnings);
            data.MusicVolume = SanitizeVolume("MusicVolume", data.MusicVolume, 70f, warnings);
            data.AmbienceVolume = SanitizeVolume("AmbienceVolume", data.AmbienceVolume, 60f, warnings);
            data.SfxVolume = SanitizeVolume("SfxVolume", data.SfxVolume, 80f, warnings);
            data.UiVolume = SanitizeVolume("UiVolume", data.UiVolume, 50f, warnings);
            data.VoiceVolume = SanitizeVolume("VoiceVolume", data.VoiceVolume, 90f, warnings);
            data.AlertVolume = SanitizeVolume("AlertVolume", data.AlertVolume, 100f, warnings);
            data.GeneratorVolume = SanitizeVolume("GeneratorVolume", data.GeneratorVolume, 70f, warnings);
            data.VentilationVolume = SanitizeVolume("VentilationVolume", data.VentilationVolume, 60f, warnings);
            data.RadioVolume = SanitizeVolume("RadioVolume", data.RadioVolume, 80f, warnings);
            data.MedicalVolume = SanitizeVolume("MedicalVolume", data.MedicalVolume, 70f, warnings);
            data.SurfaceVolume = SanitizeVolume("SurfaceVolume", data.SurfaceVolume, 50f, warnings);

            diagnosticMessage = warnings.Count > 0
                ? "[AudioSettingsCodec] Sanitized volumes: " + string.Join("; ", warnings)
                : null;

            return data;
        }

        private static float SanitizeVolume(string name, float volume, float defaultVal, List<string> warnings)
        {
            if (float.IsNaN(volume) || float.IsInfinity(volume))
            {
                warnings.Add($"{name} (non-finite) reset to {defaultVal}");
                return defaultVal;
            }
            if (volume < 0f || volume > 100f)
            {
                float clamped = Math.Clamp(volume, 0f, 100f);
                warnings.Add($"{name} {volume} clamped to {clamped}");
                return clamped;
            }
            return volume;
        }

        /// <summary>
        /// Serializes audio settings to formatted JSON string.
        /// </summary>
        public static string Serialize(AudioSettingsData data)
        {
            var sanitized = Sanitize(data, out _);
            return JsonSerializer.Serialize(sanitized, JsonOpts);
        }
    }
}
