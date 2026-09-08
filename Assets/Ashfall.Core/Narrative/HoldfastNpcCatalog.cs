using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Narrative
{
    /// <summary>
    /// Catalog of Holdfast-specific NPCs with hostile elements, faction interactions,
    /// and creative writing for the District 8 expansion.
    /// </summary>
    public sealed class HoldfastNpcDefinition
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
        [JsonPropertyName("display_name")]
        public string DisplayName { get; set; } = string.Empty;
        [JsonPropertyName("faction_id")]
        public string FactionId { get; set; } = string.Empty;
        [JsonPropertyName("role")]
        public string Role { get; set; } = string.Empty;
        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;
        [JsonPropertyName("dialogue_fragments")]
        public string[] DialogueFragments { get; set; } = Array.Empty<string>();
        [JsonPropertyName("hostile_actions")]
        public string[] HostileActions { get; set; } = Array.Empty<string>();
        [JsonPropertyName("trust_building_requirements")]
        public string[] TrustBuildingRequirements { get; set; } = Array.Empty<string>();
        [JsonPropertyName("base_trust")]
        public float BaseTrust { get; set; } = 0f;
        [JsonPropertyName("is_companion")]
        public bool IsCompanion { get; set; } = false;
        [JsonPropertyName("companion_flags")]
        public string[] CompanionFlags { get; set; } = Array.Empty<string>();
        [JsonPropertyName("companion_requirements")]
        public string[] CompanionRequirements { get; set; } = Array.Empty<string>();
    }

    /// <summary>
    /// Immutable-after-load Holdfast NPC catalog. Mutable during load only.
    /// </summary>
    public sealed class HoldfastNpcCatalog
    {
        private readonly Dictionary<string, HoldfastNpcDefinition> _byId =
            new Dictionary<string, HoldfastNpcDefinition>(StringComparer.Ordinal);
        private readonly List<HoldfastNpcDefinition> _order = new List<HoldfastNpcDefinition>();

        public int Count => _order.Count;
        public bool IsValid => _order.Count > 0;

        public static HoldfastNpcCatalog Empty() => new HoldfastNpcCatalog();

        public void Register(HoldfastNpcDefinition npc)
        {
            if (npc == null || string.IsNullOrEmpty(npc.Id) || _byId.ContainsKey(npc.Id)) return;
            _byId[npc.Id] = npc;
            _order.Add(npc);
        }

        public HoldfastNpcDefinition? GetById(string id)
            => string.IsNullOrEmpty(id) ? null : (_byId.TryGetValue(id, out var n) ? n : null);

        public bool Contains(string id) => GetById(id) != null;

        public IReadOnlyList<HoldfastNpcDefinition> All()
        {
            var list = new List<HoldfastNpcDefinition>(_order);
            list.Sort((a, b) => string.Compare(a.Id, b.Id, StringComparison.Ordinal));
            return list;
        }
    }

    /// <summary>
    /// Engine-agnostic loader for holdfast_npcs.json with load-time validation.
    /// </summary>
    public static class HoldfastNpcCatalogLoader
    {
        public const string FileName = "holdfast_npcs.json";
        public const int CurrentSchemaVersion = 1;

        public static HoldfastNpcCatalog Load(string dataDirectory, IFileIO files, IJsonSerializer json)
        {
            var catalog = new HoldfastNpcCatalog();
            string path = files.Combine(dataDirectory, FileName);
            if (!files.FileExists(path))
                return catalog;

            string raw = files.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(raw))
                return catalog;

            try
            {
                var root = json.Deserialize<HoldfastNpcCatalogRoot>(raw);
                if (root == null || root.npcs == null || root.schema_version > CurrentSchemaVersion)
                    return catalog;

                var seen = new HashSet<string>(StringComparer.Ordinal);
                foreach (var npc in root.npcs)
                {
                    if (npc == null || string.IsNullOrEmpty(npc.Id) || !seen.Add(npc.Id))
                        continue;
                    catalog.Register(npc);
                }
            }
            catch (Exception ex)
            {
                Ashfall.Core.IO.CatalogDiagnostics.Warn(path, "HoldfastNpcCatalogRoot", ex);
            }

            return catalog;
        }
    }

    /// <summary>Schema-envelope root for holdfast_npcs.json.</summary>
    public sealed class HoldfastNpcCatalogRoot
    {
        public int schema_version { get; set; } = 1;
        public List<HoldfastNpcDefinition> npcs { get; set; } = new List<HoldfastNpcDefinition>();
    }
}
