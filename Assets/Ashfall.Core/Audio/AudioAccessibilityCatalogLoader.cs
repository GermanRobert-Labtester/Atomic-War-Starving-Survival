// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Ashfall.Core.Audio
{
    /// <summary>
    /// Plan 169 — strict loader for the authored audio-accessibility catalog.
    /// The lenient <see cref="AudioAccessibilityCoordinator.LoadCatalog"/> path
    /// remains for legacy callers; the host binds through this loader so a
    /// malformed cue or preset is a loud load error, never a silent fallback to
    /// ambient audio with no visual equivalent.
    /// </summary>
    public static class AudioAccessibilityCatalogLoader
    {
        public const int CurrentSchemaVersion = 1;

        private static readonly HashSet<string> ValidSeverities =
            new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "Info", "Warning", "Critical" };

        public static AudioAccessibilityCatalogData LoadFromJson(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                throw new InvalidOperationException("audio_accessibility_cues: catalog JSON is empty.");

            AudioAccessibilityCatalogData? catalog;
            try
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                catalog = JsonSerializer.Deserialize<AudioAccessibilityCatalogData>(json, options);
            }
            catch (JsonException ex)
            {
                throw new InvalidOperationException("audio_accessibility_cues: malformed JSON (" + ex.Message + ").", ex);
            }

            if (catalog == null)
                throw new InvalidOperationException("audio_accessibility_cues: catalog deserialized to null.");

            var errors = new List<string>();

            if (catalog.SchemaVersion < 1 || catalog.SchemaVersion > CurrentSchemaVersion)
                errors.Add($"unsupported schema_version {catalog.SchemaVersion} (expected 1).");

            if (catalog.Cues == null || catalog.Cues.Count == 0)
                errors.Add("no cues declared.");
            if (catalog.MixPresets == null || catalog.MixPresets.Count == 0)
                errors.Add("no mix presets declared.");

            var cueIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            if (catalog.Cues != null)
            {
                for (int i = 0; i < catalog.Cues.Count; i++)
                {
                    var cue = catalog.Cues[i];
                    if (cue == null) { errors.Add($"cue {i} is null."); continue; }

                    if (string.IsNullOrWhiteSpace(cue.CueId))
                        errors.Add($"cue {i} has an empty cue_id.");
                    else if (!cueIds.Add(cue.CueId.Trim()))
                        errors.Add($"duplicate cue_id '{cue.CueId}'.");

                    if (string.IsNullOrWhiteSpace(cue.BusName))
                        errors.Add($"cue '{cue.CueId}' has an empty bus_name.");
                    if (string.IsNullOrWhiteSpace(cue.VisualLabel))
                        errors.Add($"cue '{cue.CueId}' has an empty visual_label.");
                    if (!ValidSeverities.Contains(cue.Severity))
                        errors.Add($"cue '{cue.CueId}' has severity '{cue.Severity}' (expected Info, Warning, or Critical).");
                    if (float.IsNaN(cue.DuckLevelDb) || cue.DuckLevelDb < -24f || cue.DuckLevelDb > 0f)
                        errors.Add($"cue '{cue.CueId}' has duck_level_db {cue.DuckLevelDb} (expected -24..0).");
                    if (float.IsNaN(cue.CoalesceWindowSeconds) || cue.CoalesceWindowSeconds < 0f)
                        errors.Add($"cue '{cue.CueId}' has coalesce_window_seconds {cue.CoalesceWindowSeconds} (must be >= 0).");
                }
            }

            var presetIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            if (catalog.MixPresets != null)
            {
                for (int i = 0; i < catalog.MixPresets.Count; i++)
                {
                    var preset = catalog.MixPresets[i];
                    if (preset == null) { errors.Add($"mix preset {i} is null."); continue; }

                    if (string.IsNullOrWhiteSpace(preset.PresetId))
                        errors.Add($"mix preset {i} has an empty preset_id.");
                    else if (!presetIds.Add(preset.PresetId.Trim()))
                        errors.Add($"duplicate preset_id '{preset.PresetId}'.");

                    if (string.IsNullOrWhiteSpace(preset.DisplayName))
                        errors.Add($"preset '{preset.PresetId}' has an empty display_name.");
                    if (float.IsNaN(preset.MasterLimiterThresholdDb) || preset.MasterLimiterThresholdDb < -24f || preset.MasterLimiterThresholdDb > 0f)
                        errors.Add($"preset '{preset.PresetId}' has master_limiter_threshold_db {preset.MasterLimiterThresholdDb} (expected -24..0).");
                    if (float.IsNaN(preset.DialogueDuckingAttenuationDb) || preset.DialogueDuckingAttenuationDb < -24f || preset.DialogueDuckingAttenuationDb > 0f)
                        errors.Add($"preset '{preset.PresetId}' has dialogue_ducking_attenuation_db {preset.DialogueDuckingAttenuationDb} (expected -24..0).");
                    if (float.IsNaN(preset.HighFrequencyAttenuationDb) || preset.HighFrequencyAttenuationDb < -24f || preset.HighFrequencyAttenuationDb > 0f)
                        errors.Add($"preset '{preset.PresetId}' has high_frequency_attenuation_db {preset.HighFrequencyAttenuationDb} (expected -24..0).");
                }
            }

            if (errors.Count > 0)
                throw new InvalidOperationException("audio_accessibility_cues: " + string.Join(" ", errors));

            return catalog;
        }
    }
}
