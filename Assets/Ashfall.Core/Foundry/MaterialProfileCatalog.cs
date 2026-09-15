// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
#pragma warning disable CS0649
#pragma warning disable CS8618

namespace Ashfall.Core.Foundry
{
    /// <summary>
    /// Plan 213 — abstract material profile for foundry outputs. Authored
    /// gameplay values only; no real metallurgy or alloy formulations.
    /// Modifiers are bp-of-1.0x. The forging_sequence is the authored ideal
    /// abstract command order (heat/shape/finish) used by the deterministic
    /// forging abstraction — never a real smithing procedure.
    /// </summary>
    [Serializable]
    public sealed class MaterialProfileDefinition
    {
        public string material_id = string.Empty;
        public string family = string.Empty;
        public string workability = "medium";
        public int durability_modifier_bp = 1000;
        public int armor_modifier_bp = 1000;
        public int corrosion_modifier_bp = 1000;
        public List<string> forging_sequence = new List<string>();
        public List<string> tags = new List<string>();
    }

    /// <summary>Load outcome with validation errors (domain result, no exceptions).</summary>
    public sealed class MaterialProfileLoadResult
    {
        public List<MaterialProfileDefinition> Materials { get; } = new List<MaterialProfileDefinition>();
        public List<string> Errors { get; } = new List<string>();
        public bool HasErrors => Errors.Count > 0;
    }

    /// <summary>In-memory material profile catalog. Immutable after load.</summary>
    public sealed class MaterialProfileCatalog
    {
        private readonly Dictionary<string, MaterialProfileDefinition> _byId =
            new Dictionary<string, MaterialProfileDefinition>(StringComparer.Ordinal);
        private readonly Dictionary<string, MaterialProfileDefinition> _byOutputItem =
            new Dictionary<string, MaterialProfileDefinition>(StringComparer.Ordinal);

        public IReadOnlyDictionary<string, MaterialProfileDefinition> ById => _byId;
        public int Count => _byId.Count;

        public MaterialProfileDefinition? Find(string materialId)
        {
            return !string.IsNullOrEmpty(materialId) && _byId.TryGetValue(materialId, out var def) ? def : null;
        }

        /// <summary>Profile bound to a foundry output item id (composition-time mapping).</summary>
        public MaterialProfileDefinition? FindByOutputItem(string outputItemId)
        {
            return !string.IsNullOrEmpty(outputItemId) && _byOutputItem.TryGetValue(outputItemId, out var def) ? def : null;
        }

        internal void Add(MaterialProfileDefinition def) => _byId[def.material_id] = def;

        /// <summary>Bind output-item → profile (composition-time, idempotent).</summary>
        public void BindOutputItem(string outputItemId, string materialId)
        {
            if (string.IsNullOrEmpty(outputItemId) || string.IsNullOrEmpty(materialId)) return;
            if (_byId.TryGetValue(materialId, out var def)) _byOutputItem[outputItemId] = def;
        }
    }

    /// <summary>
    /// Engine-agnostic loader for alloys_and_ores.json. MATERIAL PROPERTIES
    /// ONLY — never process recipes (those stay the sole authority in
    /// metallurgy_recipes.json). Closed vocabularies for family/workability
    /// and the forging-command set; bp bounds enforced.
    /// </summary>
    public static class MaterialProfileCatalogLoader
    {
        public const string FileName = "alloys_and_ores.json";
        public const int CurrentSchemaVersion = 1;

        public static readonly IReadOnlyList<string> AcceptedFamilies =
            new[] { "ferrous", "nonferrous", "exotic", "mixed", "organic" };

        public static readonly IReadOnlyList<string> AcceptedWorkability =
            new[] { "low", "medium", "high" };

        /// <summary>Closed forging-command vocabulary (abstract game decisions).</summary>
        public static readonly IReadOnlyList<string> AcceptedForgingCommands =
            new[] { "heat", "shape", "finish", "inspect" };

        public const int ModifierMinBp = 500;
        public const int ModifierMaxBp = 2000;
        public const int MaxSequenceLength = 6;

        public static MaterialProfileLoadResult Load(string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            var result = new MaterialProfileLoadResult();
            if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir))
            {
                result.Errors.Add("loader requires dataDir, IFileIO and IJsonSerializer");
                return result;
            }
            string path = fileIO.Combine(dataDir, FileName);
            if (!fileIO.FileExists(path))
            {
                result.Errors.Add("catalog file missing: " + FileName);
                return result;
            }
            string raw = fileIO.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(raw))
            {
                result.Errors.Add("catalog file empty: " + FileName);
                return result;
            }

            MaterialProfileRoot root;
            try
            {
                root = json.Deserialize<MaterialProfileRoot>(raw);
            }
            catch (Exception e)
            {
                result.Errors.Add("catalog malformed JSON: " + e.Message);
                return result;
            }
            if (root == null) { result.Errors.Add("catalog parsed to null"); return result; }
            if (root.schema_version > CurrentSchemaVersion)
            {
                result.Errors.Add($"catalog schema {root.schema_version} is newer than supported {CurrentSchemaVersion}");
                return result;
            }
            if (root.materials == null) { result.Errors.Add("catalog materials array is null"); return result; }

            var seen = new HashSet<string>(StringComparer.Ordinal);
            for (int i = 0; i < root.materials.Count; i++)
            {
                var row = root.materials[i];
                if (row == null) { result.Errors.Add($"entry [{i}] is null"); continue; }
                string id = row.material_id ?? string.Empty;
                if (string.IsNullOrWhiteSpace(id) || !IsSnakeCase(id))
                {
                    result.Errors.Add($"entry [{i}] material_id '{id}' missing or not snake_case");
                    continue;
                }
                if (!seen.Add(id))
                {
                    result.Errors.Add($"duplicate material_id '{id}' at entry [{i}]");
                    continue;
                }
                if (row.family == null || !AcceptedFamilies.Contains(row.family, StringComparer.Ordinal))
                {
                    result.Errors.Add($"'{id}' unknown family '{row.family}'");
                    continue;
                }
                string workability = row.workability ?? "medium";
                if (!AcceptedWorkability.Contains(workability, StringComparer.Ordinal))
                {
                    result.Errors.Add($"'{id}' unknown workability '{workability}'");
                    continue;
                }
                int durability = row.durability_modifier_bp ?? 1000;
                int armor = row.armor_modifier_bp ?? 1000;
                int corrosion = row.corrosion_modifier_bp ?? 1000;
                foreach (var (name, value) in new[] { ("durability", durability), ("armor", armor), ("corrosion", corrosion) })
                {
                    if (value < ModifierMinBp || value > ModifierMaxBp)
                        result.Errors.Add($"'{id}' {name}_modifier_bp must be in [{ModifierMinBp},{ModifierMaxBp}] (got {value})");
                }
                if (row.forging_sequence == null || row.forging_sequence.Count == 0)
                {
                    result.Errors.Add($"'{id}' must author a forging_sequence");
                    continue;
                }
                if (row.forging_sequence.Count > MaxSequenceLength)
                {
                    result.Errors.Add($"'{id}' forging_sequence exceeds {MaxSequenceLength} commands");
                    continue;
                }
                foreach (var cmd in row.forging_sequence)
                {
                    if (!AcceptedForgingCommands.Contains(cmd ?? string.Empty, StringComparer.Ordinal))
                        result.Errors.Add($"'{id}' unknown forging command '{cmd}'");
                }

                result.Materials.Add(new MaterialProfileDefinition
                {
                    material_id = id,
                    family = row.family!,
                    workability = workability,
                    durability_modifier_bp = durability,
                    armor_modifier_bp = armor,
                    corrosion_modifier_bp = corrosion,
                    forging_sequence = row.forging_sequence.Where(c => AcceptedForgingCommands.Contains(c ?? string.Empty, StringComparer.Ordinal)).ToList(),
                    tags = row.tags ?? new List<string>()
                });
            }
            return result;
        }

        internal static bool IsSnakeCase(string id)
        {
            if (string.IsNullOrEmpty(id)) return false;
            for (int i = 0; i < id.Length; i++)
            {
                char c = id[i];
                bool ok = (c >= 'a' && c <= 'z') || (c >= '0' && c <= '9') || c == '_';
                if (!ok) return false;
            }
            return id[0] != '_' && id[id.Length - 1] != '_';
        }

        public static MaterialProfileCatalog ToCatalog(MaterialProfileLoadResult load)
        {
            var catalog = new MaterialProfileCatalog();
            if (load != null)
                foreach (var m in load.Materials) catalog.Add(m);
            return catalog;
        }

        private class MaterialProfileRoot
        {
            public int schema_version = 1;
            public List<RawMaterialProfile> materials = new List<RawMaterialProfile>();
        }

        private class RawMaterialProfile
        {
            public string material_id;
            public string family;
            public string workability;
            public int? durability_modifier_bp;
            public int? armor_modifier_bp;
            public int? corrosion_modifier_bp;
            public List<string> forging_sequence;
            public List<string> tags;
        }
    }
}
