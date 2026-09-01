using System;
using System.Collections.Generic;

namespace Ashfall.Core
{
    /// <summary>
    /// Read-only runtime consumer for atmosphere text entries.
    /// Indexes entries by location for fast lookup and provides filtered queries
    /// by weather, condition/state, time-phase, and sense.
    /// No save/load needed — this is a static catalog (like OralLoreCatalog).
    /// Engine-agnostic: zero engine references (Invariant 1).
    /// </summary>
    public sealed class AtmosphereTextSystem
    {
        /// <summary>All entries keyed by location id (case-insensitive).</summary>
        private readonly Dictionary<string, List<AtmosphereTextEntry>> _byLocation =
            new Dictionary<string, List<AtmosphereTextEntry>>(StringComparer.OrdinalIgnoreCase);

        /// <summary>Flat list of every loaded entry.</summary>
        private readonly List<AtmosphereTextEntry> _all = new List<AtmosphereTextEntry>();

        /// <summary>All entries keyed by their unique id (case-insensitive).</summary>
        private readonly Dictionary<string, AtmosphereTextEntry> _byId =
            new Dictionary<string, AtmosphereTextEntry>(StringComparer.OrdinalIgnoreCase);

        /// <summary>Read-only access to the full catalog.</summary>
        public IReadOnlyList<AtmosphereTextEntry> AllEntries => _all;

        /// <summary>Total number of loaded entries.</summary>
        public int Count => _all.Count;

        /// <summary>
        /// Constructs the system from a pre-loaded list of entries.
        /// Builds internal indexes for efficient querying.
        /// </summary>
        public AtmosphereTextSystem(List<AtmosphereTextEntry> entries)
        {
            if (entries != null)
                LoadCatalog(entries);
        }

        /// <summary>
        /// Default constructor — call <see cref="LoadCatalog"/> afterwards.
        /// </summary>
        public AtmosphereTextSystem() { }

        /// <summary>
        /// (Re)populates the system from a list of entries, rebuilding all indexes.
        /// </summary>
        public void LoadCatalog(List<AtmosphereTextEntry> entries)
        {
            _byLocation.Clear();
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
                        list = new List<AtmosphereTextEntry>();
                        _byLocation[entry.location] = list;
                    }
                    list.Add(entry);
                }
            }
        }

        /// <summary>
        /// Returns the first atmosphere text for the given location, or null if none exists.
        /// Prefers entries with weather="any" and condition="intact"/"normal" as the default.
        /// </summary>
        public AtmosphereTextEntry GetTextForLocation(string locationId)
        {
            if (string.IsNullOrEmpty(locationId)) return null;
            if (!_byLocation.TryGetValue(locationId, out var list) || list.Count == 0)
                return null;

            // Prefer a "default" entry: weather=any, condition=intact or normal
            for (int i = 0; i < list.Count; i++)
            {
                var e = list[i];
                bool anyWeather = string.IsNullOrEmpty(e.weather) ||
                                  string.Equals(e.weather, "any", StringComparison.OrdinalIgnoreCase);
                bool normalCondition = string.IsNullOrEmpty(e.condition) ||
                                       string.Equals(e.condition, "intact", StringComparison.OrdinalIgnoreCase) ||
                                       string.Equals(e.condition, "normal", StringComparison.OrdinalIgnoreCase);
                if (anyWeather && normalCondition)
                    return e;
            }

            // Fallback: first entry for this location
            return list[0];
        }

        /// <summary>
        /// Returns the first atmosphere text matching both location and weather.
        /// Falls back to entries with weather="any" if no exact weather match.
        /// Returns null if no entries exist for the location.
        /// </summary>
        public AtmosphereTextEntry GetTextForLocationAndWeather(string locationId, string weather)
        {
            if (string.IsNullOrEmpty(locationId)) return null;
            if (!_byLocation.TryGetValue(locationId, out var list) || list.Count == 0)
                return null;

            // Try exact weather match first
            if (!string.IsNullOrEmpty(weather))
            {
                for (int i = 0; i < list.Count; i++)
                {
                    var e = list[i];
                    if (string.Equals(e.weather, weather, StringComparison.OrdinalIgnoreCase))
                        return e;
                }
            }

            // Fall back to weather="any"
            for (int i = 0; i < list.Count; i++)
            {
                var e = list[i];
                if (string.IsNullOrEmpty(e.weather) ||
                    string.Equals(e.weather, "any", StringComparison.OrdinalIgnoreCase))
                    return e;
            }

            // Last resort: first entry
            return list[0];
        }

        /// <summary>
        /// Returns the first atmosphere text matching location and condition/state.
        /// Falls back to entries with condition="intact"/"normal" if no exact match.
        /// Returns null if no entries exist for the location.
        /// </summary>
        public AtmosphereTextEntry GetTextForLocationAndState(string locationId, string state)
        {
            if (string.IsNullOrEmpty(locationId)) return null;
            if (!_byLocation.TryGetValue(locationId, out var list) || list.Count == 0)
                return null;

            // Try exact condition/state match
            if (!string.IsNullOrEmpty(state))
            {
                for (int i = 0; i < list.Count; i++)
                {
                    var e = list[i];
                    if (string.Equals(e.condition, state, StringComparison.OrdinalIgnoreCase))
                        return e;
                }
            }

            // Fall back to "intact" or "normal" condition
            for (int i = 0; i < list.Count; i++)
            {
                var e = list[i];
                if (string.IsNullOrEmpty(e.condition) ||
                    string.Equals(e.condition, "intact", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(e.condition, "normal", StringComparison.OrdinalIgnoreCase))
                    return e;
            }

            // Last resort: first entry
            return list[0];
        }

        /// <summary>
        /// Returns all atmosphere texts for the given location.
        /// Returns an empty list if no entries match.
        /// </summary>
        public List<AtmosphereTextEntry> GetAllTextsForLocation(string locationId)
        {
            if (string.IsNullOrEmpty(locationId))
                return new List<AtmosphereTextEntry>();

            if (_byLocation.TryGetValue(locationId, out var list))
                return new List<AtmosphereTextEntry>(list);

            return new List<AtmosphereTextEntry>();
        }

        /// <summary>
        /// Looks up a single entry by its unique id.
        /// Returns null if not found.
        /// </summary>
        public AtmosphereTextEntry GetById(string entryId)
        {
            if (string.IsNullOrEmpty(entryId)) return null;
            _byId.TryGetValue(entryId, out var entry);
            return entry;
        }

        /// <summary>
        /// Returns all entries that carry the given tag (case-insensitive).
        /// </summary>
        public List<AtmosphereTextEntry> GetByTag(string tag)
        {
            var results = new List<AtmosphereTextEntry>();
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
        /// Returns all entries that carry the given atmosphere keyword (case-insensitive).
        /// </summary>
        public List<AtmosphereTextEntry> GetByAtmosphere(string atmosphereKeyword)
        {
            var results = new List<AtmosphereTextEntry>();
            if (string.IsNullOrEmpty(atmosphereKeyword)) return results;

            for (int i = 0; i < _all.Count; i++)
            {
                var e = _all[i];
                if (e.atmosphere == null) continue;
                for (int j = 0; j < e.atmosphere.Length; j++)
                {
                    if (string.Equals(e.atmosphere[j], atmosphereKeyword, StringComparison.OrdinalIgnoreCase))
                    {
                        results.Add(e);
                        break;
                    }
                }
            }
            return results;
        }
    }
}
