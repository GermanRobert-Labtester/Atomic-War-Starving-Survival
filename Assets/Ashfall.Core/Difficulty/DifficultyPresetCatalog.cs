// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Difficulty
{
    /// <summary>
    /// Authored scalar bundle for a campaign difficulty. Values multiply an
    /// already-owned system's base calculation; the director never applies the
    /// values itself.
    /// </summary>
    public sealed class DifficultyScalars
    {
        public float hunger_rate_mult { get; set; }
        public float thirst_rate_mult { get; set; }
        public float radiation_gain_mult { get; set; }
        public float disease_onset_mult { get; set; }
        public float hostile_encounter_mult { get; set; }
        public float market_price_mult { get; set; }
        public float equipment_decay_mult { get; set; }
        public float crisis_deadline_mult { get; set; }

        public static DifficultyScalars Legacy()
        {
            return new DifficultyScalars
            {
                hunger_rate_mult = 1f,
                thirst_rate_mult = 1f,
                radiation_gain_mult = 1f,
                disease_onset_mult = 1f,
                hostile_encounter_mult = 1f,
                market_price_mult = 1f,
                equipment_decay_mult = 1f,
                crisis_deadline_mult = 1f
            };
        }

        public DifficultyScalars Clone()
        {
            return new DifficultyScalars
            {
                hunger_rate_mult = hunger_rate_mult,
                thirst_rate_mult = thirst_rate_mult,
                radiation_gain_mult = radiation_gain_mult,
                disease_onset_mult = disease_onset_mult,
                hostile_encounter_mult = hostile_encounter_mult,
                market_price_mult = market_price_mult,
                equipment_decay_mult = equipment_decay_mult,
                crisis_deadline_mult = crisis_deadline_mult
            };
        }

        public bool Validate(out string error)
        {
            if (!ValidateMultiplier(nameof(hunger_rate_mult), hunger_rate_mult, out error) ||
                !ValidateMultiplier(nameof(thirst_rate_mult), thirst_rate_mult, out error) ||
                !ValidateMultiplier(nameof(radiation_gain_mult), radiation_gain_mult, out error) ||
                !ValidateMultiplier(nameof(disease_onset_mult), disease_onset_mult, out error) ||
                !ValidateMultiplier(nameof(hostile_encounter_mult), hostile_encounter_mult, out error) ||
                !ValidateMultiplier(nameof(market_price_mult), market_price_mult, out error) ||
                !ValidateMultiplier(nameof(equipment_decay_mult), equipment_decay_mult, out error) ||
                !ValidateMultiplier(nameof(crisis_deadline_mult), crisis_deadline_mult, out error))
                return false;

            error = string.Empty;
            return true;
        }

        private static bool ValidateMultiplier(string name, float value, out string error)
        {
            if (float.IsNaN(value) || float.IsInfinity(value) || value < 0.25f || value > 2.5f)
            {
                error = name + " must be finite and within [0.25, 2.5]";
                return false;
            }

            error = string.Empty;
            return true;
        }
    }

    public sealed class DifficultyPreset
    {
        public string id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public string description_key { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public DifficultyScalars scalars { get; set; } = new DifficultyScalars();
        public List<string> starting_bonus_item_ids { get; set; } = new List<string>();

        public bool Validate(out string error)
        {
            if (!IsDifficultyId(id))
            {
                error = "id must be a snake_case difficulty_ id";
                return false;
            }
            if (string.IsNullOrWhiteSpace(display_name))
            {
                error = "display_name is required";
                return false;
            }
            if (string.IsNullOrWhiteSpace(description))
            {
                error = "description is required";
                return false;
            }
            if (scalars == null)
            {
                error = "scalars are required";
                return false;
            }
            if (!scalars.Validate(out error)) return false;
            if (starting_bonus_item_ids == null)
            {
                error = "starting_bonus_item_ids are required";
                return false;
            }
            for (int i = 0; i < starting_bonus_item_ids.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(starting_bonus_item_ids[i]))
                {
                    error = "starting_bonus_item_ids must not contain empty ids";
                    return false;
                }
            }

            error = string.Empty;
            return true;
        }

        private static bool IsDifficultyId(string value)
        {
            if (string.IsNullOrEmpty(value) || !value.StartsWith("difficulty_", StringComparison.Ordinal))
                return false;
            for (int i = 0; i < value.Length; i++)
            {
                char c = value[i];
                if (!((c >= 'a' && c <= 'z') || (c >= '0' && c <= '9') || c == '_'))
                    return false;
            }
            return true;
        }
    }

    /// <summary>Single catalog authority for campaign difficulty IDs and scalars.</summary>
    public sealed class DifficultyPresetCatalog
    {
        public const int CurrentSchemaVersion = 1;

        public int schema_version { get; set; } = CurrentSchemaVersion;
        public List<DifficultyPreset> presets { get; set; } = new List<DifficultyPreset>();
        public string default_preset_id { get; set; } = string.Empty;

        private readonly Dictionary<string, DifficultyPreset> _byId =
            new Dictionary<string, DifficultyPreset>(StringComparer.Ordinal);

        [JsonIgnore]
        public IReadOnlyList<DifficultyPreset> AllPresets => presets.AsReadOnly();

        public bool TryGet(string id, out DifficultyPreset preset)
        {
            return _byId.TryGetValue(id ?? string.Empty, out preset!);
        }

        public void Index()
        {
            _byId.Clear();
            for (int i = 0; i < presets.Count; i++)
            {
                DifficultyPreset preset = presets[i];
                if (preset != null && !string.IsNullOrEmpty(preset.id))
                    _byId[preset.id] = preset;
            }
        }

        public bool Validate(out string error)
        {
            if (schema_version != CurrentSchemaVersion)
            {
                error = "unsupported schema_version " + schema_version;
                return false;
            }
            if (presets == null || presets.Count == 0)
            {
                error = "at least one preset is required";
                return false;
            }

            var seen = new HashSet<string>(StringComparer.Ordinal);
            for (int i = 0; i < presets.Count; i++)
            {
                DifficultyPreset preset = presets[i];
                if (preset == null)
                {
                    error = "preset " + i + " is null";
                    return false;
                }
                if (!preset.Validate(out error))
                {
                    error = "preset '" + preset.id + "': " + error;
                    return false;
                }
                if (!seen.Add(preset.id))
                {
                    error = "duplicate preset id '" + preset.id + "'";
                    return false;
                }
            }

            if (string.IsNullOrWhiteSpace(default_preset_id) || !seen.Contains(default_preset_id))
            {
                error = "default_preset_id must resolve to an authored preset";
                return false;
            }

            error = string.Empty;
            return true;
        }
    }

    public static class DifficultyPresetCatalogLoader
    {
        public const string FileName = "difficulty_presets.json";

        public static DifficultyPresetCatalog Load(string dataDirectory, IFileIO fileIo)
        {
            if (fileIo == null) throw new ArgumentNullException(nameof(fileIo));
            if (string.IsNullOrWhiteSpace(dataDirectory))
                throw new InvalidOperationException("difficulty catalog data directory is required");

            string path = fileIo.Combine(dataDirectory, FileName);
            if (!fileIo.FileExists(path))
                throw new InvalidOperationException("difficulty catalog is missing");
            return LoadFromJson(fileIo.ReadAllText(path));
        }

        public static DifficultyPresetCatalog LoadFromJson(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                throw new InvalidOperationException("difficulty catalog is empty");

            DifficultyPresetCatalog? catalog;
            try
            {
                catalog = JsonSerializer.Deserialize<DifficultyPresetCatalog>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (JsonException ex)
            {
                throw new InvalidOperationException("difficulty catalog is malformed: " + ex.Message, ex);
            }

            if (catalog == null)
                throw new InvalidOperationException("difficulty catalog is empty");
            if (!catalog.Validate(out string error))
                throw new InvalidOperationException(error);
            catalog.Index();
            return catalog;
        }
    }
}
