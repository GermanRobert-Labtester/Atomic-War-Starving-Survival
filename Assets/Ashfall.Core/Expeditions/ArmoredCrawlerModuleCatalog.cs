// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json.Serialization;
using Ashfall.Core.IO;

namespace Ashfall.Core.Expeditions
{
    [Serializable]
    public sealed class CrawlerModuleDefinition
    {
        public string id = string.Empty;

        // Plan 47 wave 1: canonical snake_case wire keys; legacy camelCase keys
        // are accepted through CatalogKeyNormalizer (docs/data/SNAKE_CASE_MIGRATION.md).
        [JsonPropertyName("display_name")]
        public string displayName = string.Empty;
        [JsonPropertyName("slot_type")]
        public string slotType = "Utility";
        public float mass = 100.0f;
        [JsonPropertyName("power_draw")]
        public float powerDraw = 50.0f;
        [JsonPropertyName("crew_berths")]
        public int crewBerths = 0;
        [JsonPropertyName("armor_modifier")]
        public float armorModifier = 0.0f;
        [JsonPropertyName("fuel_modifier")]
        public float fuelModifier = 0.0f;
        [JsonPropertyName("cargo_modifier")]
        public float cargoModifier = 0.0f;
        [JsonPropertyName("workshop_capability")]
        public bool workshopCapability = false;
        [JsonPropertyName("life_support_modifier")]
        public float lifeSupportModifier = 0.0f;
        public List<string> tags = new List<string>();

        public bool Validate(out string error)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                error = "Crawler module ID cannot be empty.";
                return false;
            }
            if (slotType != "Cabin" && slotType != "Chassis" && slotType != "Utility" &&
                slotType != "Defense" && slotType != "Treads")
            {
                error = $"Module '{id}' has invalid slotType '{slotType}'.";
                return false;
            }
            if (mass < 0 || powerDraw < 0 || crewBerths < 0)
            {
                error = $"Module '{id}' cannot have negative mass, powerDraw, or crewBerths.";
                return false;
            }
            if (float.IsNaN(armorModifier) || float.IsInfinity(armorModifier) ||
                float.IsNaN(fuelModifier) || float.IsInfinity(fuelModifier) ||
                float.IsNaN(cargoModifier) || float.IsInfinity(cargoModifier) ||
                float.IsNaN(lifeSupportModifier) || float.IsInfinity(lifeSupportModifier))
            {
                error = $"Module '{id}' has non-finite numeric modifiers.";
                return false;
            }

            error = string.Empty;
            return true;
        }
    }

    [Serializable]
    public sealed class ArmoredCrawlerModuleCatalogDto
    {
        public int schema_version { get; set; } = 1;
        public List<CrawlerModuleDefinition> modules { get; set; } = new List<CrawlerModuleDefinition>();
    }

    public sealed class ArmoredCrawlerModuleCatalog
    {
        private readonly Dictionary<string, CrawlerModuleDefinition> _modules = new(StringComparer.OrdinalIgnoreCase);

        public ArmoredCrawlerModuleCatalog(IEnumerable<CrawlerModuleDefinition>? modules)
        {
            if (modules == null) return;
            foreach (var m in modules)
            {
                if (m != null && !string.IsNullOrWhiteSpace(m.id))
                    _modules[m.id] = m;
            }
        }

        public IReadOnlyDictionary<string, CrawlerModuleDefinition> Modules => _modules;

        public CrawlerModuleDefinition? GetModule(string moduleId)
        {
            if (string.IsNullOrEmpty(moduleId)) return null;
            return _modules.TryGetValue(moduleId, out var def) ? def : null;
        }
    }

    public static class ArmoredCrawlerModuleCatalogLoader
    {
        public const string DefaultFileName = "armored_crawler_modules.json";

        /// <summary>Plan 47 wave 1: legacy camelCase → canonical snake_case (spelling-only).</summary>
        public static readonly IReadOnlyDictionary<string, string> KeyAliases = new Dictionary<string, string>
        {
            ["displayName"] = "display_name",
            ["slotType"] = "slot_type",
            ["powerDraw"] = "power_draw",
            ["crewBerths"] = "crew_berths",
            ["armorModifier"] = "armor_modifier",
            ["fuelModifier"] = "fuel_modifier",
            ["cargoModifier"] = "cargo_modifier",
            ["workshopCapability"] = "workshop_capability",
            ["lifeSupportModifier"] = "life_support_modifier",
        };

        public static ArmoredCrawlerModuleCatalog? Load(string dataDir, IFileIO fileIO, IJsonSerializer jsonSerializer)
        {
            string path = Path.Combine(dataDir, DefaultFileName);
            if (!fileIO.FileExists(path)) return null;

            string json = fileIO.ReadAllText(path);
            // Dual-read: legacy camelCase accepted, conflicts fail loudly.
            string normalized = CatalogKeyNormalizer.Normalize(json, KeyAliases, DefaultFileName);
            var dto = jsonSerializer.Deserialize<ArmoredCrawlerModuleCatalogDto>(normalized);
            if (dto?.modules == null) return null;

            return new ArmoredCrawlerModuleCatalog(dto.modules);
        }
    }
}
