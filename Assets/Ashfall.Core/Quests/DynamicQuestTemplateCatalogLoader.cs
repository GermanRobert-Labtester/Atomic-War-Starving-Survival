// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Ashfall.Core.Quests
{
    /// <summary>JSON shape of one authored dynamic quest template.</summary>
    [Serializable]
    public sealed class DynamicQuestTemplateDef
    {
        public string template_id = string.Empty;
        public string type = string.Empty;
        public string title = string.Empty;
        public string description = string.Empty;
        public int base_difficulty = 1;
        public string target_location_id = string.Empty;
        public string target_resource_id = string.Empty;
        public int required_quantity = 1;
        public float morale_reward;
        public float faction_standing_reward;
        public string target_faction_id = string.Empty;
        public string reward_item_id = string.Empty;
        public int reward_item_count;
    }

    [Serializable]
    public sealed class DynamicQuestTemplateCatalogJson
    {
        public int schema_version = 1;
        public List<DynamicQuestTemplateDef> templates = new List<DynamicQuestTemplateDef>();
    }

    /// <summary>
    /// Plan 171 — strict loader for the authored dynamic quest template catalog.
    /// The catalog was registered UNRESOLVED for months (never loaded). This
    /// loader binds it and rejects an unsupported schema, empty/duplicate
    /// template ids, an unknown quest type, empty titles/descriptions, a
    /// difficulty outside 1..5, a non-positive required quantity, negative
    /// rewards, or a reward item declared without a positive count.
    /// </summary>
    public static class DynamicQuestTemplateCatalogLoader
    {
        public const int CurrentSchemaVersion = 1;

        public static IReadOnlyList<ProceduralQuestTemplate> LoadFromJson(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                throw new InvalidOperationException("dynamic_quest_templates: catalog JSON is empty.");

            DynamicQuestTemplateCatalogJson? catalog;
            try
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true, IncludeFields = true };
                catalog = JsonSerializer.Deserialize<DynamicQuestTemplateCatalogJson>(json, options);
            }
            catch (JsonException ex)
            {
                throw new InvalidOperationException("dynamic_quest_templates: malformed JSON (" + ex.Message + ").", ex);
            }

            if (catalog == null)
                throw new InvalidOperationException("dynamic_quest_templates: catalog deserialized to null.");

            var errors = new List<string>();
            if (catalog.schema_version < 1 || catalog.schema_version > CurrentSchemaVersion)
                errors.Add($"unsupported schema_version {catalog.schema_version} (expected 1).");
            if (catalog.templates == null || catalog.templates.Count == 0)
                errors.Add("no templates declared.");

            var ids = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var templates = new List<ProceduralQuestTemplate>();

            if (catalog.templates != null)
            {
                for (int i = 0; i < catalog.templates.Count; i++)
                {
                    var def = catalog.templates[i];
                    if (def == null) { errors.Add($"template {i} is null."); continue; }

                    if (string.IsNullOrWhiteSpace(def.template_id))
                        errors.Add($"template {i} has an empty template_id.");
                    else if (!ids.Add(def.template_id.Trim()))
                        errors.Add($"duplicate template_id '{def.template_id}'.");

                    if (string.IsNullOrWhiteSpace(def.title))
                        errors.Add($"template '{def.template_id}' has an empty title.");
                    if (string.IsNullOrWhiteSpace(def.description))
                        errors.Add($"template '{def.template_id}' has an empty description.");
                    if (!Enum.TryParse<ProceduralQuestType>(def.type, true, out _))
                        errors.Add($"template '{def.template_id}' declares unknown quest type '{def.type}'.");
                    if (def.base_difficulty < 1 || def.base_difficulty > 5)
                        errors.Add($"template '{def.template_id}' has base_difficulty {def.base_difficulty} (expected 1..5).");
                    if (def.required_quantity < 1)
                        errors.Add($"template '{def.template_id}' has required_quantity {def.required_quantity} (must be >= 1).");
                    if (def.morale_reward < 0f || float.IsNaN(def.morale_reward) || float.IsInfinity(def.morale_reward))
                        errors.Add($"template '{def.template_id}' has a negative or non-finite morale_reward.");
                    if (def.faction_standing_reward < 0f || float.IsNaN(def.faction_standing_reward) || float.IsInfinity(def.faction_standing_reward))
                        errors.Add($"template '{def.template_id}' has a negative or non-finite faction_standing_reward.");
                    if (!string.IsNullOrWhiteSpace(def.reward_item_id) && def.reward_item_count <= 0)
                        errors.Add($"template '{def.template_id}' declares reward_item_id without a positive reward_item_count.");

                    if (Enum.TryParse<ProceduralQuestType>(def.type, true, out var questType))
                    {
                        templates.Add(new ProceduralQuestTemplate
                        {
                            TemplateId = def.template_id.Trim(),
                            Type = questType,
                            TitleTemplate = def.title,
                            DescriptionTemplate = def.description,
                            BaseDifficulty = def.base_difficulty,
                            TargetLocationId = def.target_location_id ?? string.Empty,
                            TargetResourceId = def.target_resource_id ?? string.Empty,
                            RequiredQuantity = def.required_quantity,
                            MoraleReward = def.morale_reward,
                            FactionStandingReward = def.faction_standing_reward,
                            TargetFactionId = def.target_faction_id ?? string.Empty,
                            RewardItemId = def.reward_item_id ?? string.Empty,
                            RewardItemCount = def.reward_item_count
                        });
                    }
                }
            }

            if (errors.Count > 0)
                throw new InvalidOperationException("dynamic_quest_templates: " + string.Join(" ", errors));

            return templates;
        }
    }
}
