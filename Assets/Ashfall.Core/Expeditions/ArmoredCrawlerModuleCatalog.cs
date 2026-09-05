// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core.IO;

namespace Ashfall.Core.Expeditions
{
    [Serializable]
    public sealed class CrawlerModuleDefinition
    {
        public string id = string.Empty;
        public string displayName = string.Empty;
        public string slotType = "Utility";
        public float mass = 100.0f;
        public float powerDraw = 50.0f;
        public int crewBerths = 0;
        public float armorModifier = 0.0f;
        public float fuelModifier = 0.0f;
        public float cargoModifier = 0.0f;
        public bool workshopCapability = false;
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

        public static ArmoredCrawlerModuleCatalog? Load(string dataDir, IFileIO fileIO, IJsonSerializer jsonSerializer)
        {
            string path = Path.Combine(dataDir, DefaultFileName);
            if (!fileIO.FileExists(path)) return null;

            string json = fileIO.ReadAllText(path);
            var dto = jsonSerializer.Deserialize<ArmoredCrawlerModuleCatalogDto>(json);
            if (dto?.modules == null) return null;

            return new ArmoredCrawlerModuleCatalog(dto.modules);
        }
    }
}
