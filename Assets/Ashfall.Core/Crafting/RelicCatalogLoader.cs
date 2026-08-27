using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.IO;

namespace Ashfall.Core.Crafting
{
    [Serializable]
    internal sealed class RelicJsonDto
    {
        public string relic_id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public List<string>? required_components { get; set; }
        public float repair_time_hours { get; set; } = 8f;
        public int morale_bonus { get; set; } = 3;
        public string dialogue_event_id { get; set; } = string.Empty;
        public string restoration_text { get; set; } = string.Empty;
        public string world_flag { get; set; } = string.Empty;
        public string research_unlock_id { get; set; } = string.Empty;
        public string dismantle_yield_item { get; set; } = string.Empty;
        public int dismantle_yield_amount { get; set; } = 1;
        public string category { get; set; } = "relic";
    }

    /// <summary>
    /// Core loader for pre-war technical relics and historical artifacts from relic_recipes.json.
    /// Engine-agnostic, zero host dependencies.
    /// </summary>
    public static class RelicCatalogLoader
    {
        public const string FileName = "relic_recipes.json";

        public static RelicCatalog Load(string dataDir, IFileIO fileIO, IJsonSerializer serializer)
        {
            var result = LoadWithResult(dataDir, fileIO, serializer);
            var catalog = new RelicCatalog();
            catalog.relics = result.Entries.ToList();
            return catalog;
        }

        public static CatalogLoadResult<RelicDefinition> LoadWithResult(
            string dataDir, IFileIO fileIO, IJsonSerializer serializer)
        {
            string path = fileIO.Combine(dataDir, FileName);
            var result = new CatalogLoadResult<RelicDefinition>(
                path,
                "Relic recipe catalog",
                CatalogClassification.Required);

            if (fileIO == null || serializer == null || string.IsNullOrEmpty(dataDir))
            {
                result.AddFatal("Required dependencies are null");
                return result;
            }

            if (!fileIO.FileExists(path))
            {
                result.AddFatal("Relic recipes catalog file not found: " + path);
                return result;
            }

            string raw = fileIO.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(raw))
            {
                result.AddError("Relic recipes catalog file is empty: " + path);
                return result;
            }

            try
            {
                var dtos = CatalogLocator.LoadWrappedList<RelicJsonDto>(raw, SystemTextJsonSerializer.Options);
                for (int i = 0; i < dtos.Count; i++)
                {
                    var dto = dtos[i];
                    if (dto == null || string.IsNullOrEmpty(dto.relic_id)) continue;

                    var relic = new RelicDefinition
                    {
                        relic_id = dto.relic_id,
                        display_name = !string.IsNullOrEmpty(dto.display_name) ? dto.display_name : dto.relic_id,
                        description = dto.description ?? string.Empty,
                        required_components = dto.required_components ?? new List<string>(),
                        repair_time_hours = dto.repair_time_hours > 0f ? dto.repair_time_hours : 8f,
                        morale_bonus = dto.morale_bonus,
                        dialogue_event_id = dto.dialogue_event_id ?? string.Empty,
                        restoration_text = dto.restoration_text ?? string.Empty,
                        world_flag = dto.world_flag ?? string.Empty,
                        research_unlock_id = dto.research_unlock_id ?? string.Empty,
                        dismantle_yield_item = dto.dismantle_yield_item ?? string.Empty,
                        dismantle_yield_amount = dto.dismantle_yield_amount > 0 ? dto.dismantle_yield_amount : 1,
                        category = !string.IsNullOrEmpty(dto.category) ? dto.category : "relic"
                    };

                    result.AddEntry(relic);
                }
            }
            catch (Exception ex)
            {
                CatalogDiagnostics.Warn("RelicCatalogLoader", FileName, ex);
                result.AddFatal("Failed to load relic recipes: " + ex.Message, ex);
            }

            return result;
        }
    }
}
