// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Ashfall.Core.IO;

namespace Ashfall.Core.Survivors
{
    [Serializable]
    public sealed class TradeSpecialtyMilestoneDto
    {
        [JsonPropertyName("tier")]
        public int Tier { get; set; } = 1;

        [JsonPropertyName("item_patterns")]
        public List<string> ItemPatterns { get; set; } = new List<string>();

        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("narrative")]
        public string Narrative { get; set; } = string.Empty;

        [JsonPropertyName("skill_bonus")]
        public float SkillBonus { get; set; } = 0.05f;
    }

    [Serializable]
    public sealed class TradeSpecialtyItemDto
    {
        [JsonPropertyName("profession_id")]
        public string ProfessionId { get; set; } = string.Empty;

        [JsonPropertyName("display_name")]
        public string DisplayName { get; set; } = string.Empty;

        [JsonPropertyName("milestones")]
        public List<TradeSpecialtyMilestoneDto> Milestones { get; set; } = new List<TradeSpecialtyMilestoneDto>();

        [JsonPropertyName("mastery_narrative")]
        public string MasteryNarrative { get; set; } = string.Empty;

        [JsonPropertyName("mastery_bonus_text")]
        public string MasteryBonusText { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class TradeSpecialtyCatalogContainer
    {
        [JsonPropertyName("schema_version")]
        public int SchemaVersion { get; set; } = 1;

        [JsonPropertyName("items")]
        public List<TradeSpecialtyItemDto> Items { get; set; } = new List<TradeSpecialtyItemDto>();
    }

    /// <summary>
    /// Loads trade specialty configurations from JSON (Assets/StreamingAssets/Data/trade_specialties.json).
    /// Pure C#, engine-agnostic: uses IFileIO and IJsonSerializer ports.
    /// </summary>
    public static class TradeSpecialtyCatalogLoader
    {
        public const string DefaultFileName = "trade_specialties.json";

        public static List<TradeSpecialtyItemDto> Load(string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir))
                return new List<TradeSpecialtyItemDto>();

            string path = fileIO.Combine(dataDir, DefaultFileName);
            if (!fileIO.FileExists(path))
                return new List<TradeSpecialtyItemDto>();

            string rawText = fileIO.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(rawText))
                return new List<TradeSpecialtyItemDto>();

            try
            {
                var container = JsonSerializer.Deserialize<TradeSpecialtyCatalogContainer>(rawText, SystemTextJsonSerializer.Options);
                return container?.Items ?? new List<TradeSpecialtyItemDto>();
            }
            catch (Exception ex_CATDIAG)
            {
                CatalogDiagnostics.Warn(DefaultFileName, "TradeSpecialtyCatalogLoader", ex_CATDIAG);
                return new List<TradeSpecialtyItemDto>();
            }
        }

        public static int LoadAndRegister(
            TradeSpecialtySystem system,
            string dataDir,
            IFileIO fileIO,
            IJsonSerializer json)
        {
            if (system == null) return 0;
            var items = Load(dataDir, fileIO, json);
            if (items.Count > 0)
            {
                foreach (var item in items)
                {
                    if (string.IsNullOrEmpty(item.ProfessionId)) continue;
                    var patterns = new List<string>();
                    if (item.Milestones != null)
                    {
                        foreach (var m in item.Milestones)
                        {
                            if (m.ItemPatterns != null)
                                patterns.AddRange(m.ItemPatterns);
                        }
                    }
                    TradeSpecialtySystem.RegisterProfessionPatterns(item.ProfessionId, patterns);
                }
                return items.Count;
            }
            // Honest count: a missing/empty catalog must surface as 0 so callers
            // can diagnose it — never a fake default that masks dead data wiring.
            return items.Count;
        }
    }
}
