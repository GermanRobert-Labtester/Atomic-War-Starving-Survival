// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core.IO;

namespace Ashfall.Core.Crafting
{
    [Serializable]
    public sealed class ChemicalProcessDefinition
    {
        public string id = string.Empty;
        public string displayName = string.Empty;
        public string description = string.Empty;
        public int requiredApparatusTier = 1;
        public Dictionary<string, int> inputItems = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        public Dictionary<string, int> outputItems = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        public int processingTicks = 2;
        public string heatBand = "Nominal";
        public float volatilityRating = 0.2f;
        public float scrubberDemand = 1.0f;
        public float equipmentWear = 2.0f;
        public float corrosionRating; // Plans 90-93 mineral line: normalized apparatus/storage corrosion per tick
        public float skillRequirement = 10.0f;
        public List<string> tags = new List<string>();

        public bool Validate(out string error)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                error = "Process ID cannot be empty.";
                return false;
            }
            if (requiredApparatusTier < 1 || requiredApparatusTier > 3)
            {
                error = $"Process '{id}' has invalid apparatus tier {requiredApparatusTier} (must be 1..3).";
                return false;
            }
            if (processingTicks <= 0)
            {
                error = $"Process '{id}' must have processingTicks > 0.";
                return false;
            }
            if (inputItems == null || inputItems.Count == 0)
            {
                error = $"Process '{id}' has no input items defined.";
                return false;
            }
            foreach (var kv in inputItems)
            {
                if (string.IsNullOrWhiteSpace(kv.Key) || kv.Value <= 0)
                {
                    error = $"Process '{id}' has invalid input item '{kv.Key}' with amount {kv.Value}.";
                    return false;
                }
            }
            if (outputItems == null || outputItems.Count == 0)
            {
                error = $"Process '{id}' has no output items defined.";
                return false;
            }
            foreach (var kv in outputItems)
            {
                if (string.IsNullOrWhiteSpace(kv.Key) || kv.Value <= 0)
                {
                    error = $"Process '{id}' has invalid output item '{kv.Key}' with amount {kv.Value}.";
                    return false;
                }
            }
            if (heatBand != "Low" && heatBand != "Nominal" && heatBand != "High" && heatBand != "Runaway")
            {
                error = $"Process '{id}' has invalid heatBand '{heatBand}'.";
                return false;
            }
            if (volatilityRating < 0 || volatilityRating > 1.0f)
            {
                error = $"Process '{id}' volatilityRating must be within [0, 1.0].";
                return false;
            }
            if (scrubberDemand < 0 || equipmentWear < 0 || skillRequirement < 0)
            {
                error = $"Process '{id}' demands and wear cannot be negative.";
                return false;
            }
            if (corrosionRating < 0)
            {
                error = $"Process '{id}' corrosionRating cannot be negative.";
                return false;
            }

            error = string.Empty;
            return true;
        }
    }

    [Serializable]
    public sealed class ChemicalSynthesisCatalogDto
    {
        public int schema_version { get; set; } = 1;
        public List<ChemicalProcessDefinition> processes { get; set; } = new List<ChemicalProcessDefinition>();
    }

    public sealed class ChemicalSynthesisCatalog
    {
        private readonly Dictionary<string, ChemicalProcessDefinition> _processes = new(StringComparer.OrdinalIgnoreCase);

        public ChemicalSynthesisCatalog(IEnumerable<ChemicalProcessDefinition>? processes)
        {
            if (processes == null) return;
            foreach (var p in processes)
            {
                if (p != null && !string.IsNullOrWhiteSpace(p.id))
                    _processes[p.id] = p;
            }
        }

        public IReadOnlyDictionary<string, ChemicalProcessDefinition> Processes => _processes;

        public ChemicalProcessDefinition? GetProcess(string processId)
        {
            if (string.IsNullOrEmpty(processId)) return null;
            return _processes.TryGetValue(processId, out var def) ? def : null;
        }
    }

    public static class ChemicalSynthesisCatalogLoader
    {
        public const string DefaultFileName = "chemical_syntheses.json";
        // Plans 90-93: mineral-chemical industrial line shares the canonical
        // chemical process authority — same engine, same schema, extra file.
        public const string MineralFileName = "mineral_acid_synthesis_catalog.json";

        public static ChemicalSynthesisCatalog? Load(string dataDir, IFileIO fileIO, IJsonSerializer jsonSerializer)
        {
            var processes = new List<ChemicalProcessDefinition>();

            AppendFile(dataDir, DefaultFileName, fileIO, jsonSerializer, processes);
            AppendFile(dataDir, MineralFileName, fileIO, jsonSerializer, processes);

            if (processes.Count == 0) return null;
            return new ChemicalSynthesisCatalog(processes);
        }

        private static void AppendFile(
            string dataDir,
            string fileName,
            IFileIO fileIO,
            IJsonSerializer jsonSerializer,
            List<ChemicalProcessDefinition> processes)
        {
            string path = Path.Combine(dataDir, fileName);
            if (!fileIO.FileExists(path)) return;

            string json = fileIO.ReadAllText(path);
            var dto = jsonSerializer.Deserialize<ChemicalSynthesisCatalogDto>(json);
            if (dto?.processes == null) return;

            processes.AddRange(dto.processes);
        }
    }
}
