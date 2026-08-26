using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Ashfall.Core.Factions
{
    /// <summary>One morality-gated ending row nested under a branch definition.</summary>
    public sealed class MilitaryBranchEndingEntry
    {
        public string ending_id { get; set; } = string.Empty;
        public string band_min { get; set; } = string.Empty;
        public string band_max { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
    }

    /// <summary>One base branch row, matching military_faction_branch.json shape.</summary>
    public sealed class MilitaryBranchEntry
    {
        public string id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public string ponr_flag { get; set; } = string.Empty;
        public string ponr_trigger { get; set; } = string.Empty;
        public string entry_band_min { get; set; } = string.Empty;
        public string entry_band_max { get; set; } = string.Empty;
        public List<MilitaryBranchEndingEntry> endings { get; set; } = new List<MilitaryBranchEndingEntry>();
    }

    /// <summary>Root shape of military_faction_branch.json.</summary>
    public sealed class MilitaryBranchDataFile
    {
        public int schema_version { get; set; } = 1;
        public string faction_id { get; set; } = string.Empty;
        public List<MilitaryBranchEntry> branches { get; set; } = new List<MilitaryBranchEntry>();
    }

    /// <summary>Immutable-after-load catalog of Military branch/ending definitions.</summary>
    public sealed class MilitaryBranchCatalog : IEnumerable<MilitaryBranchEntry>
    {
        private readonly Dictionary<string, MilitaryBranchEntry> _byId =
            new Dictionary<string, MilitaryBranchEntry>(StringComparer.Ordinal);
        private readonly List<MilitaryBranchEntry> _order = new List<MilitaryBranchEntry>();

        public int Count => _order.Count;
        public MilitaryBranchEntry this[int index] => _order[index];

        public static MilitaryBranchCatalog Empty() => new MilitaryBranchCatalog();

        public void Register(MilitaryBranchEntry entry)
        {
            if (entry == null || string.IsNullOrEmpty(entry.id) || _byId.ContainsKey(entry.id)) return;
            _byId[entry.id] = entry;
            _order.Add(entry);
        }

        public MilitaryBranchEntry? GetById(string id) =>
            string.IsNullOrEmpty(id) ? null : (_byId.TryGetValue(id, out var e) ? e : null);

        public bool Contains(string id) => GetById(id) != null;

        public IEnumerator<MilitaryBranchEntry> GetEnumerator() => _order.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _order.GetEnumerator();

        public static MilitaryBranchCatalog LoadAndRegister(string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            if (fileIO == null) throw new ArgumentNullException(nameof(fileIO));
            if (json == null) throw new ArgumentNullException(nameof(json));

            var catalog = new MilitaryBranchCatalog();
            string path = fileIO.Combine(dataDir, "military_faction_branch.json");
            if (!fileIO.FileExists(path)) return catalog;

            string text = fileIO.ReadAllText(path);
            var file = json.Deserialize<MilitaryBranchDataFile>(text);
            if (file?.branches == null) return catalog;

            foreach (var entry in file.branches)
                catalog.Register(entry);

            return catalog;
        }
    }
}
