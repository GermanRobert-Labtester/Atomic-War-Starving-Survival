// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Ashfall.Core.IO;

namespace Ashfall.Core
{
    [Serializable]
    public sealed class TechSalvageYieldDef
    {
        [JsonPropertyName("item_id")]
        public string ItemId { get; set; } = string.Empty;

        [JsonPropertyName("amount")]
        public int Amount { get; set; } = 1;
    }

    /// <summary>One explicitly cataloged pre-war device that can teach a blueprint.</summary>
    [Serializable]
    public sealed class PreWarTechDef
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("source_item_id")]
        public string SourceItemId { get; set; } = string.Empty;

        [JsonPropertyName("display_name_key")]
        public string DisplayNameKey { get; set; } = string.Empty;

        [JsonPropertyName("complexity")]
        public int Complexity { get; set; } = 1;

        [JsonPropertyName("base_research_points")]
        public int BaseResearchPoints { get; set; } = 1;

        [JsonPropertyName("base_scrap_yields")]
        public List<TechSalvageYieldDef> BaseScrapYields { get; set; } = new List<TechSalvageYieldDef>();

        [JsonPropertyName("possible_blueprint_ids")]
        public List<string> PossibleBlueprintIds { get; set; } = new List<string>();

        [JsonPropertyName("blueprint_required_points")]
        public int BlueprintRequiredPoints { get; set; } = 10;

        [JsonPropertyName("required_research_equipment_tags")]
        public List<string> RequiredResearchEquipmentTags { get; set; } = new List<string>();

        [JsonPropertyName("required_skill_discipline")]
        public string RequiredSkillDiscipline { get; set; } = string.Empty;

        [JsonPropertyName("base_success_chance")]
        public float BaseSuccessChance { get; set; } = 0.65f;

        [JsonPropertyName("catastrophic_failure_chance")]
        public float CatastrophicFailureChance { get; set; } = 0.05f;

        [JsonPropertyName("preservation_value")]
        public float PreservationValue { get; set; } = 0.5f;

        [JsonPropertyName("research_notes_pool")]
        public List<string> ResearchNotesPool { get; set; } = new List<string>();

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();
    }

    [Serializable]
    public sealed class TechSalvageCatalog
    {
        [JsonPropertyName("schema_version")]
        public int SchemaVersion { get; set; } = 1;

        [JsonPropertyName("technologies")]
        public List<PreWarTechDef> Technologies { get; set; } = new List<PreWarTechDef>();
    }

    /// <summary>Loads the Plan 166 catalog from the JSON data authority.</summary>
    public static class TechSalvageCatalogLoader
    {
        public const string FileName = "tech_salvage.json";

        public static List<PreWarTechDef> Load(string dataDir, IFileIO fileIO, IJsonSerializer serializer)
        {
            var result = new List<PreWarTechDef>();
            if (fileIO == null || serializer == null || string.IsNullOrWhiteSpace(dataDir)) return result;

            string path = fileIO.Combine(dataDir, FileName);
            if (!fileIO.FileExists(path)) return result;
            string raw = fileIO.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(raw)) return result;

            try
            {
                var catalog = JsonSerializer.Deserialize<TechSalvageCatalog>(raw, SystemTextJsonSerializer.Options);
                if (catalog?.Technologies == null) return result;
                var seen = new HashSet<string>(StringComparer.Ordinal);
                foreach (var def in catalog.Technologies)
                {
                    if (def == null || string.IsNullOrWhiteSpace(def.Id) || !seen.Add(def.Id)) continue;
                    result.Add(def);
                }
            }
            catch (Exception ex_CATDIAG)
            {
                CatalogDiagnostics.Warn(FileName, "TechSalvageCatalogLoader", ex_CATDIAG);
            }
            return result;
        }

        public static bool Validate(IEnumerable<PreWarTechDef> definitions, out string error)
        {
            error = string.Empty;
            if (definitions == null) return true;
            var ids = new HashSet<string>(StringComparer.Ordinal);
            foreach (var def in definitions)
            {
                if (def == null || string.IsNullOrWhiteSpace(def.Id))
                {
                    error = "Tech salvage definition has an empty id.";
                    return false;
                }
                if (!ids.Add(def.Id))
                {
                    error = "Duplicate tech salvage id: " + def.Id;
                    return false;
                }
                if (string.IsNullOrWhiteSpace(def.SourceItemId) || def.Complexity <= 0
                    || def.BaseResearchPoints <= 0 || def.BlueprintRequiredPoints <= 0)
                {
                    error = "Tech salvage definition has invalid source, complexity, or point budget: " + def.Id;
                    return false;
                }
                if (def.BaseSuccessChance < 0f || def.BaseSuccessChance > 1f
                    || def.CatastrophicFailureChance < 0f || def.CatastrophicFailureChance > 1f)
                {
                    error = "Tech salvage probabilities must be between 0 and 1: " + def.Id;
                    return false;
                }
                if (def.PossibleBlueprintIds == null || def.PossibleBlueprintIds.Count == 0)
                {
                    error = "Tech salvage definition has no blueprint targets: " + def.Id;
                    return false;
                }
            }
            return true;
        }
    }

    [Serializable]
    public sealed class ResearchFacilityContext
    {
        public bool IsAvailable = true;
        public bool PowerStable = true;
        public float EquipmentQuality01;
        public List<string> EquipmentTags = new List<string>();

        public ResearchFacilityContext Clone()
        {
            return new ResearchFacilityContext
            {
                IsAvailable = IsAvailable,
                PowerStable = PowerStable,
                EquipmentQuality01 = Math.Clamp(EquipmentQuality01, 0f, 1f),
                EquipmentTags = EquipmentTags != null
                    ? new List<string>(EquipmentTags)
                    : new List<string>()
            };
        }
    }

    [Serializable]
    public sealed class TechDismantlePreview
    {
        public string techId = string.Empty;
        public string sourceItemId = string.Empty;
        public float successChance;
        public float catastrophicFailureChance;
        public int researchPoints;
        public int blueprintRequiredPoints;
        public bool isAvailable;
        public string failureCode = string.Empty;
    }

    [Serializable]
    public sealed class TechSalvageFailure
    {
        public string techId = string.Empty;
        public string sourceItemId = string.Empty;
        public string failureType = string.Empty;
        public int day;
    }
}
