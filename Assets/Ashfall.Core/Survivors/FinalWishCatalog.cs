#pragma warning disable CS8618
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Survivors
{
    /// <summary>
    /// Authored final-wish entry loaded from <c>final_wishes.json</c>.
    /// Field names are snake_case to match the JSON data authority verbatim
    /// (System.Text.Json is configured with PropertyNameCaseInsensitive and no
    /// naming policy, so C# field names must equal the JSON keys).
    /// </summary>
    [Serializable]
    public sealed class FinalWishEntry
    {
        /// <summary>Unique wish id, <c>wish_&lt;archetype&gt;_&lt;slug&gt;</c>. Registered as a DefinitionKey by the integrity validator.</summary>
        public string id = string.Empty;
        /// <summary>Survivor archetype this wish belongs to (the pool key), e.g. <c>the_surgeon</c>.</summary>
        public string archetype_id = string.Empty;
        /// <summary>Wish type drives step-count fallback when no catalog step list is available (matches FinalWishSystem constants).</summary>
        public string wish_type = string.Empty;
        public string wish_title = string.Empty;
        public string wish_description = string.Empty;
        public List<FinalWishStep> steps = new List<FinalWishStep>();
        public string completion_text = string.Empty;
        public float morale_bonus;
        public string buff_id = string.Empty;
    }

    /// <summary>
    /// One step of a final-wish questline. Mirrors the authored JSON shape; the
    /// runtime system tracks completion by step count, not by matching ids.
    /// </summary>
    [Serializable]
    public sealed class FinalWishStep
    {
        public string step_id = string.Empty;
        public string description = string.Empty;
        public List<string> required_items = new List<string>();
        public string requires_location = string.Empty;
        public bool requires_patient;
    }

    /// <summary>
    /// Wrapper envelope for <c>final_wishes.json</c>: <c>{ schema_version, items:[...] }</c>.
    /// </summary>
    [Serializable]
    public sealed class FinalWishContainer
    {
        public int schema_version = 1;
        public List<FinalWishEntry> items = new List<FinalWishEntry>();
    }

    /// <summary>
    /// Engine-agnostic lookup over loaded final-wish entries. Implemented by
    /// <see cref="FinalWishCatalog"/>; the host injects an instance into
    /// <see cref="FinalWishSystem.Catalog"/>. Core defines the port so the system
    /// never couples to JSON or file IO.
    /// </summary>
    public interface IFinalWishCatalog
    {
        /// <summary>All authored wish ids for the given archetype (the pool). Empty if none.</summary>
        IReadOnlyList<string> GetWishIdsForArchetype(string archetypeId);

        /// <summary>The full entry for a wish id, or null if unknown.</summary>
        FinalWishEntry? GetEntry(string wishId);
    }

    /// <summary>
    /// In-memory catalog: archetype → pool of wish ids, and wish id → entry.
    /// Lookups are ordinal-comparison, O(1). Built by <see cref="FinalWishCatalogLoader"/>.
    /// </summary>
    public sealed class FinalWishCatalog : IFinalWishCatalog
    {
        private readonly Dictionary<string, List<string>> _byArchetype =
            new Dictionary<string, List<string>>(StringComparer.Ordinal);
        private readonly Dictionary<string, FinalWishEntry> _byId =
            new Dictionary<string, FinalWishEntry>(StringComparer.Ordinal);

        public int Count => _byId.Count;

        public void Add(FinalWishEntry entry)
        {
            if (entry == null || string.IsNullOrEmpty(entry.id)) return;

            // First write wins for id→entry (dedupe); archetype pool accumulates distinct ids.
            if (!_byId.ContainsKey(entry.id))
                _byId[entry.id] = entry;

            string key = entry.archetype_id ?? string.Empty;
            if (!_byArchetype.TryGetValue(key, out var pool))
            {
                pool = new List<string>();
                _byArchetype[key] = pool;
            }
            if (!pool.Contains(entry.id))
                pool.Add(entry.id);
        }

        public IReadOnlyList<string> GetWishIdsForArchetype(string archetypeId)
        {
            if (string.IsNullOrEmpty(archetypeId)) return Array.Empty<string>();
            return _byArchetype.TryGetValue(archetypeId, out var pool)
                ? pool
                : (IReadOnlyList<string>)Array.Empty<string>();
        }

        public FinalWishEntry? GetEntry(string wishId)
        {
            if (string.IsNullOrEmpty(wishId)) return null;
            return _byId.TryGetValue(wishId, out var entry) ? entry : null;
        }
    }
}
