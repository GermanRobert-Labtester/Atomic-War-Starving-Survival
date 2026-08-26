using System;
using System.Collections;
using System.Collections.Generic;

namespace Ashfall.Core.Factions
{
    /// <summary>One morality-gated ending row nested under a Rebel branch definition.</summary>
    public sealed class RebelBranchEndingEntry
    {
        public string ending_id { get; set; } = string.Empty;
        public string band_min { get; set; } = string.Empty;
        public string band_max { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
    }

    /// <summary>One base Rebel branch row, matching rebel_faction_branch.json shape.</summary>
    public sealed class RebelBranchEntry
    {
        public string id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public string ponr_flag { get; set; } = string.Empty;
        public string ponr_trigger { get; set; } = string.Empty;
        public string entry_band_min { get; set; } = string.Empty;
        public string entry_band_max { get; set; } = string.Empty;
        public List<RebelBranchEndingEntry> endings { get; set; } = new List<RebelBranchEndingEntry>();
    }

    /// <summary>Root shape of rebel_faction_branch.json.</summary>
    public sealed class RebelBranchDataFile
    {
        public int schema_version { get; set; } = 1;
        public string faction_id { get; set; } = string.Empty;
        public List<RebelBranchEntry> branches { get; set; } = new List<RebelBranchEntry>();
    }

    /// <summary>Immutable-after-load catalog of Rebel branch/ending definitions.</summary>
    public sealed class RebelBranchCatalog : IEnumerable<RebelBranchEntry>
    {
        private readonly Dictionary<string, RebelBranchEntry> _byId =
            new Dictionary<string, RebelBranchEntry>(StringComparer.Ordinal);
        private readonly List<RebelBranchEntry> _order = new List<RebelBranchEntry>();

        public int Count => _order.Count;
        public RebelBranchEntry this[int index] => _order[index];

        public static RebelBranchCatalog Empty() => new RebelBranchCatalog();

        public void Register(RebelBranchEntry entry)
        {
            if (entry == null || string.IsNullOrEmpty(entry.id) || _byId.ContainsKey(entry.id)) return;
            _byId[entry.id] = entry;
            _order.Add(entry);
        }

        public RebelBranchEntry? GetById(string id) =>
            string.IsNullOrEmpty(id) ? null : (_byId.TryGetValue(id, out var e) ? e : null);

        public bool Contains(string id) => GetById(id) != null;

        public IEnumerator<RebelBranchEntry> GetEnumerator() => _order.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _order.GetEnumerator();

        public static RebelBranchCatalog LoadAndRegister(string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            if (fileIO == null) throw new ArgumentNullException(nameof(fileIO));
            if (json == null) throw new ArgumentNullException(nameof(json));

            var catalog = new RebelBranchCatalog();
            string path = fileIO.Combine(dataDir, "rebel_faction_branch.json");
            if (!fileIO.FileExists(path)) return catalog;

            string text = fileIO.ReadAllText(path);
            var file = json.Deserialize<RebelBranchDataFile>(text);
            if (file?.branches == null) return catalog;

            foreach (var entry in file.branches)
                catalog.Register(entry);

            return catalog;
        }
    }
}
