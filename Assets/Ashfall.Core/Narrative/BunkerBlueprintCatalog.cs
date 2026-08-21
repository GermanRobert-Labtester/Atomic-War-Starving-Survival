using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core.Narrative
{
    [Serializable]
    public sealed class BunkerBlueprintEntry
    {
        public string room_id;
        public string room_name;
        public string category;
        public float optimal_depth_meters;
        public int max_dweller_capacity;
        public float base_power_draw_kw;
        public float water_flow_lpm;
        public float acoustic_noise_db;
        public float thermal_r_value;
        public float radiation_attenuation_factor;
        public string structural_header_spec;
        public string catastrophic_failure_mode;
        public int maintenance_cycle_days;
        public string chief_engineer_note;
        public string[] tags;
    }

    [Serializable]
    public sealed class BunkerBlueprintsFile
    {
        public int schema_version;
        public string collection_id;
        public List<BunkerBlueprintEntry> blueprints = new List<BunkerBlueprintEntry>();
    }

    /// <summary>
    /// Engine-agnostic loader and query interface for The 24 Underground Architectural Blueprints & Engineering Codex.
    /// </summary>
    public sealed class BunkerBlueprintCatalog
    {
        private readonly Dictionary<string, BunkerBlueprintEntry> _byId =
            new Dictionary<string, BunkerBlueprintEntry>(StringComparer.OrdinalIgnoreCase);

        private readonly List<BunkerBlueprintEntry> _allBlueprints = new List<BunkerBlueprintEntry>();

        public IReadOnlyList<BunkerBlueprintEntry> AllBlueprints => _allBlueprints;

        public void Load(string json, IJsonSerializer serializer)
        {
            if (string.IsNullOrEmpty(json) || serializer == null) return;
            var file = serializer.Deserialize<BunkerBlueprintsFile>(json);
            if (file?.blueprints == null) return;

            foreach (var bp in file.blueprints)
            {
                if (bp == null || string.IsNullOrEmpty(bp.room_id)) continue;
                _byId[bp.room_id] = bp;
                _allBlueprints.Add(bp);
            }
        }

        public BunkerBlueprintEntry? GetById(string roomId)
        {
            if (string.IsNullOrEmpty(roomId)) return null;
            _byId.TryGetValue(roomId, out var entry);
            return entry;
        }

        public List<BunkerBlueprintEntry> GetByCategory(string categorySnippet)
        {
            var results = new List<BunkerBlueprintEntry>();
            if (string.IsNullOrEmpty(categorySnippet)) return results;

            for (int i = 0; i < _allBlueprints.Count; i++)
            {
                var bp = _allBlueprints[i];
                if (bp.category != null && bp.category.IndexOf(categorySnippet, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    results.Add(bp);
                }
            }
            return results;
        }

        public List<BunkerBlueprintEntry> GetPowerProducers()
        {
            var results = new List<BunkerBlueprintEntry>();
            for (int i = 0; i < _allBlueprints.Count; i++)
            {
                var bp = _allBlueprints[i];
                if (bp.base_power_draw_kw < 0f)
                {
                    results.Add(bp);
                }
            }
            return results;
        }

        public List<BunkerBlueprintEntry> GetByTag(string tag)
        {
            var results = new List<BunkerBlueprintEntry>();
            if (string.IsNullOrEmpty(tag)) return results;

            for (int i = 0; i < _allBlueprints.Count; i++)
            {
                var bp = _allBlueprints[i];
                if (bp.tags == null) continue;
                for (int j = 0; j < bp.tags.Length; j++)
                {
                    if (string.Equals(bp.tags[j], tag, StringComparison.OrdinalIgnoreCase))
                    {
                        results.Add(bp);
                        break;
                    }
                }
            }
            return results;
        }
    }
}
