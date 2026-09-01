// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Ashfall.Core.IO;

namespace Ashfall.Core.Survivors
{
    [Serializable]
    public sealed class SkillWireDto
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("display_name")]
        public string DisplayName { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("discipline_id")]
        public string DisciplineId { get; set; } = string.Empty;

        [JsonPropertyName("xp_threshold")]
        public float XpThreshold { get; set; } = 0f;

        [JsonPropertyName("skill_bonus")]
        public float SkillBonus { get; set; } = 0f;

        [JsonPropertyName("is_expert_skill")]
        public bool IsExpertSkill { get; set; } = false;

        public SkillDef ToDomain()
        {
            return new SkillDef
            {
                id = Id,
                displayName = DisplayName,
                description = Description,
                disciplineId = DisciplineId,
                xpThreshold = XpThreshold,
                skillBonus = SkillBonus,
                isExpertSkill = IsExpertSkill
            };
        }
    }

    [Serializable]
    public sealed class SkillCatalogContainer
    {
        [JsonPropertyName("schema_version")]
        public int SchemaVersion { get; set; } = 1;

        [JsonPropertyName("collection_id")]
        public string CollectionId { get; set; } = "skills";

        [JsonPropertyName("skills")]
        public List<SkillWireDto> Skills { get; set; } = new List<SkillWireDto>();
    }

    /// <summary>
    /// Loads skill definitions from JSON (Assets/StreamingAssets/Data/skills.json).
    /// Pure C#, engine-agnostic: uses IFileIO and IJsonSerializer ports.
    /// Authored definitions in skills.json are the sole production authority.
    /// </summary>
    public static class SkillCatalogLoader
    {
        public const string DefaultFileName = "skills.json";

        public static List<SkillDef> Load(string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir))
                return new List<SkillDef>();

            string path = fileIO.Combine(dataDir, DefaultFileName);
            if (!fileIO.FileExists(path))
                return new List<SkillDef>();

            string rawText = fileIO.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(rawText))
                return new List<SkillDef>();

            try
            {
                var container = JsonSerializer.Deserialize<SkillCatalogContainer>(rawText, SystemTextJsonSerializer.Options);
                if (container?.Skills == null) return new List<SkillDef>();

                var list = new List<SkillDef>(container.Skills.Count);
                var seenIds = new HashSet<string>(StringComparer.Ordinal);

                foreach (var dto in container.Skills)
                {
                    if (dto != null && !string.IsNullOrEmpty(dto.Id))
                    {
                        if (seenIds.Add(dto.Id))
                        {
                            list.Add(dto.ToDomain());
                        }
                    }
                }
                return list;
            }
            catch (Exception ex_CATDIAG)
            {
                CatalogDiagnostics.Warn(DefaultFileName, "SkillCatalogLoader", ex_CATDIAG);
                return new List<SkillDef>();
            }
        }

        public static int LoadAndRegister(
            SkillProgressionSystem system,
            string dataDir,
            IFileIO fileIO,
            IJsonSerializer json)
        {
            if (system == null) return 0;
            var defs = Load(dataDir, fileIO, json);
            if (defs.Count > 0)
            {
                foreach (var def in defs)
                {
                    system.RegisterSkill(def);
                }
                return defs.Count;
            }

            return 0;
        }
    }
}
