using System;
using System.Collections.Generic;

namespace Ashfall.Core
{
    /// <summary>
    /// Read-only runtime consumer for environmental text entries.
    /// Indexes entries by location, type, and tags for fast lookup.
    /// No save/load needed — this is a static catalog (like AtmosphereTextSystem).
    /// Engine-agnostic: zero engine references (Invariant 1).
    /// </summary>
    public sealed class EnvironmentalTextSystem
    {
        /// <summary>All entries keyed by location id (case-insensitive).</summary>
        private readonly Dictionary<string, List<EnvironmentalTextEntry>> _byLocation =
            new Dictionary<string, List<EnvironmentalTextEntry>>(StringComparer.OrdinalIgnoreCase);

        /// <summary>All entries keyed by type (case-insensitive).</summary>
        private readonly Dictionary<string, List<EnvironmentalTextEntry>> _byType =
            new Dictionary<string, List<EnvironmentalTextEntry>>(StringComparer.OrdinalIgnoreCase);

        /// <summary>Flat list of every loaded entry.</summary>
        private readonly List<EnvironmentalTextEntry> _all = new List<EnvironmentalTextEntry>();

        /// <summary>All entries keyed by their unique id (case-insensitive).</summary>
        private readonly Dictionary<string, EnvironmentalTextEntry> _byId =
            new Dictionary<string, EnvironmentalTextEntry>(StringComparer.OrdinalIgnoreCase);

        /// <summary>Read-only access to the full catalog.</summary>
        public IReadOnlyList<EnvironmentalTextEntry> AllEntries => _all;

        /// <summary>Total number of loaded entries.</summary>
        public int Count => _all.Count;

        /// <summary>
        /// Constructs the system from a pre-loaded list of entries.
        /// Builds internal indexes for efficient querying.
        /// </summary>
        public EnvironmentalTextSystem(List<EnvironmentalTextEntry> entries)
        {
            if (entries != null)
                LoadCatalog(entries);
        }

        /// <summary>
        /// Default constructor — call <see cref="LoadCatalog"/> afterwards.
        /// </summary>
        public EnvironmentalTextSystem() { }

        /// <summary>
        /// (Re)populates the system from a list of entries, rebuilding all indexes.
        /// </summary>
        public void LoadCatalog(List<EnvironmentalTextEntry> entries)
        {
            _byLocation.Clear();
            _byType.Clear();
            _all.Clear();
            _byId.Clear();

            if (entries == null) return;

            for (int i = 0; i < entries.Count; i++)
            {
                var entry = entries[i];
                if (entry == null) continue;

                _all.Add(entry);

                // Index by unique id
                if (!string.IsNullOrEmpty(entry.id))
                    _byId[entry.id] = entry;

                // Index by location
                if (!string.IsNullOrEmpty(entry.location))
                {
                    if (!_byLocation.TryGetValue(entry.location, out var list))
                    {
                        list = new List<EnvironmentalTextEntry>();
                        _byLocation[entry.location] = list;
                    }
                    list.Add(entry);
                }

                // Index by type
                if (!string.IsNullOrEmpty(entry.type))
                {
                    if (!_byType.TryGetValue(entry.type, out var list))
                    {
                        list = new List<EnvironmentalTextEntry>();
                        _byType[entry.type] = list;
                    }
                    list.Add(entry);
                }
            }
        }

        /// <summary>
        /// Returns the first environmental text for the given location, or null if none exists.
        /// </summary>
        public EnvironmentalTextEntry GetTextForLocation(string locationId)
        {
            if (string.IsNullOrEmpty(locationId)) return null;
            if (!_byLocation.TryGetValue(locationId, out var list) || list.Count == 0)
                return null;

            return list[0];
        }

        /// <summary>
        /// Returns all environmental texts for the given location.
        /// Returns an empty list if no entries match.
        /// </summary>
        public List<EnvironmentalTextEntry> GetAllTextsForLocation(string locationId)
        {
            if (string.IsNullOrEmpty(locationId))
                return new List<EnvironmentalTextEntry>();

            if (_byLocation.TryGetValue(locationId, out var list))
                return new List<EnvironmentalTextEntry>(list);

            return new List<EnvironmentalTextEntry>();
        }

        /// <summary>
        /// Returns all entries of the given type (e.g. "warning", "note", "diary", "broadcast").
        /// Returns an empty list if no entries match.
        /// </summary>
        public List<EnvironmentalTextEntry> GetTextByType(string type)
        {
            if (string.IsNullOrEmpty(type))
                return new List<EnvironmentalTextEntry>();

            if (_byType.TryGetValue(type, out var list))
                return new List<EnvironmentalTextEntry>(list);

            return new List<EnvironmentalTextEntry>();
        }

        /// <summary>
        /// Returns all entries that carry the given tag (case-insensitive).
        /// </summary>
        public List<EnvironmentalTextEntry> GetTextByTag(string tag)
        {
            var results = new List<EnvironmentalTextEntry>();
            if (string.IsNullOrEmpty(tag)) return results;

            for (int i = 0; i < _all.Count; i++)
            {
                var e = _all[i];
                if (e.tags == null) continue;
                for (int j = 0; j < e.tags.Length; j++)
                {
                    if (string.Equals(e.tags[j], tag, StringComparison.OrdinalIgnoreCase))
                    {
                        results.Add(e);
                        break;
                    }
                }
            }
            return results;
        }

        /// <summary>
        /// Looks up a single entry by its unique id.
        /// Returns null if not found.
        /// </summary>
        public EnvironmentalTextEntry GetById(string entryId)
        {
            if (string.IsNullOrEmpty(entryId)) return null;
            _byId.TryGetValue(entryId, out var entry);
            return entry;
        }
    }
}
