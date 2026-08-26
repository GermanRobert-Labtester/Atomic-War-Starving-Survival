using System;
using System.Collections;
using System.Collections.Generic;

namespace Ashfall.Core.Factions
{
    /// <summary>One morality-gated ending row nested under an Independent branch definition.</summary>
    public sealed class IndependentBranchEndingEntry
    {
        public string ending_id { get; set; } = string.Empty;
        public string band_min { get; set; } = string.Empty;
        public string band_max { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
    }

    /// <summary>
    /// One base Independent branch row, matching independent_faction_branch.json
    /// shape. Carries three optional gate fields Military/Rebel branches do
    /// not have: requires_prpf_standing_min (IND-3), and
    /// requires_hostile_to_military / requires_hostile_to_rebel (IND-4, the
    /// "enemy of everyone" branch). Nullable so a branch that doesn't use a
    /// given gate simply omits it from the JSON rather than writing an inert
    /// default value.
    /// </summary>
    public sealed class IndependentBranchEntry
    {
        public string id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public string ponr_flag { get; set; } = string.Empty;
        public string ponr_trigger { get; set; } = string.Empty;
        public string entry_band_min { get; set; } = string.Empty;
        public string entry_band_max { get; set; } = string.Empty;
        public int? requires_prpf_standing_min { get; set; }
        public bool? requires_hostile_to_military { get; set; }
        public bool? requires_hostile_to_rebel { get; set; }
        public List<IndependentBranchEndingEntry> endings { get; set; } = new List<IndependentBranchEndingEntry>();
    }

    /// <summary>Root shape of independent_faction_branch.json.</summary>
    public sealed class IndependentBranchDataFile
    {
        public int schema_version { get; set; } = 1;
        public string faction_id { get; set; } = string.Empty;
        public List<IndependentBranchEntry> branches { get; set; } = new List<IndependentBranchEntry>();
    }

    /// <summary>Immutable-after-load catalog of Independent branch/ending definitions.</summary>
    public sealed class IndependentBranchCatalog : IEnumerable<IndependentBranchEntry>
    {
        private readonly Dictionary<string, IndependentBranchEntry> _byId =
            new Dictionary<string, IndependentBranchEntry>(StringComparer.Ordinal);
        private readonly List<IndependentBranchEntry> _order = new List<IndependentBranchEntry>();

        public int Count => _order.Count;
        public IndependentBranchEntry this[int index] => _order[index];

        public static IndependentBranchCatalog Empty() => new IndependentBranchCatalog();

        public void Register(IndependentBranchEntry entry)
        {
            if (entry == null || string.IsNullOrEmpty(entry.id) || _byId.ContainsKey(entry.id)) return;
            _byId[entry.id] = entry;
            _order.Add(entry);
        }

        public IndependentBranchEntry? GetById(string id) =>
            string.IsNullOrEmpty(id) ? null : (_byId.TryGetValue(id, out var e) ? e : null);

        public bool Contains(string id) => GetById(id) != null;

        public IEnumerator<IndependentBranchEntry> GetEnumerator() => _order.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _order.GetEnumerator();

        public static IndependentBranchCatalog LoadAndRegister(string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            if (fileIO == null) throw new ArgumentNullException(nameof(fileIO));
            if (json == null) throw new ArgumentNullException(nameof(json));

            var catalog = new IndependentBranchCatalog();
            string path = fileIO.Combine(dataDir, "independent_faction_branch.json");
            if (!fileIO.FileExists(path)) return catalog;

            string text = fileIO.ReadAllText(path);
            var file = json.Deserialize<IndependentBranchDataFile>(text);
            if (file?.branches == null) return catalog;

            foreach (var entry in file.branches)
                catalog.Register(entry);

            return catalog;
        }
    }
}
