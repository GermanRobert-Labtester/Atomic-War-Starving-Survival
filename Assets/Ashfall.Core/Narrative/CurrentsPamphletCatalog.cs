using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core.Narrative
{
    [Serializable]
    public sealed class CurrentsPamphletEntry
    {
        public string faction_id;
        public string pamphlet_id;
        public string title;
        public string doctrine;
        public string liturgy;
        public string ideological_alignment;
        public string key_figure;
    }

    [Serializable]
    public sealed class CurrentsPamphletBatchFile
    {
        public int schema_version;
        public List<CurrentsPamphletEntry> pamphlets = new List<CurrentsPamphletEntry>();
    }

    /// <summary>
    /// Catalog loader and query interface for the 16 Currents faction ideological pamphlets.
    /// </summary>
    public sealed class CurrentsPamphletCatalog
    {
        private readonly Dictionary<string, CurrentsPamphletEntry> _byPamphletId =
            new Dictionary<string, CurrentsPamphletEntry>(StringComparer.OrdinalIgnoreCase);

        private readonly Dictionary<string, CurrentsPamphletEntry> _byFactionId =
            new Dictionary<string, CurrentsPamphletEntry>(StringComparer.OrdinalIgnoreCase);

        public IReadOnlyCollection<CurrentsPamphletEntry> AllPamphlets => _byPamphletId.Values;

        public void Load(string json, IJsonSerializer serializer)
        {
            if (string.IsNullOrEmpty(json) || serializer == null) return;
            var batch = serializer.Deserialize<CurrentsPamphletBatchFile>(json);
            if (batch?.pamphlets == null) return;

            foreach (var pmp in batch.pamphlets)
            {
                if (pmp == null) continue;
                if (!string.IsNullOrEmpty(pmp.pamphlet_id))
                    _byPamphletId[pmp.pamphlet_id] = pmp;
                if (!string.IsNullOrEmpty(pmp.faction_id))
                    _byFactionId[pmp.faction_id] = pmp;
            }
        }

        public CurrentsPamphletEntry? GetByPamphletId(string pamphletId)
        {
            if (string.IsNullOrEmpty(pamphletId)) return null;
            _byPamphletId.TryGetValue(pamphletId, out var entry);
            return entry;
        }

        public CurrentsPamphletEntry? GetByFactionId(string factionId)
        {
            if (string.IsNullOrEmpty(factionId)) return null;
            _byFactionId.TryGetValue(factionId, out var entry);
            return entry;
        }

        public bool ContainsFaction(string factionId)
        {
            if (string.IsNullOrEmpty(factionId)) return false;
            return _byFactionId.ContainsKey(factionId);
        }
    }
}
