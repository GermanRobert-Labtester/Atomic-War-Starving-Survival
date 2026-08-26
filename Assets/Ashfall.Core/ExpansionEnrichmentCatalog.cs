using System;
using System.Collections.Generic;

namespace Ashfall.Core
{
    /// <summary>
    /// Loads and indexes the expansion enrichment data: survivor background fields
    /// (belief profiles, keepsakes, phantom backgrounds, professions) and item tags
    /// (narrative markers for keepsakes, phantom triggers, restorable photos, etc.).
    ///
    /// These files enrich existing survivors and items with narrative depth.
    /// They do NOT define new survivors or items — they annotate the ones that
    /// already exist in <c>survivors.json</c> and the item catalogs.
    /// </summary>
    public sealed class ExpansionEnrichmentCatalog
    {
        private readonly Dictionary<string, ExpansionSurvivorFields> _survivorFields =
            new Dictionary<string, ExpansionSurvivorFields>(StringComparer.Ordinal);
        private readonly Dictionary<string, ExpansionItemTags> _itemTags =
            new Dictionary<string, ExpansionItemTags>(StringComparer.Ordinal);

        /// <summary>Total survivors with enrichment data.</summary>
        public int SurvivorFieldCount => _survivorFields.Count;

        /// <summary>Total items with tag data.</summary>
        public int ItemTagCount => _itemTags.Count;

        /// <summary>Returns enrichment fields for a survivor, or null if none exist.</summary>
        public ExpansionSurvivorFields GetSurvivorFields(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return null;
            return _survivorFields.TryGetValue(survivorId, out var f) ? f : null;
        }

        /// <summary>Returns tags for an item, or null if none exist.</summary>
        public ExpansionItemTags GetItemTags(string itemId)
        {
            if (string.IsNullOrEmpty(itemId)) return null;
            return _itemTags.TryGetValue(itemId, out var t) ? t : null;
        }

        /// <summary>Returns true if the item has the specified tag.</summary>
        public bool HasTag(string itemId, string tag)
        {
            var tags = GetItemTags(itemId);
            if (tags?.tags == null) return false;
            for (int i = 0; i < tags.tags.Count; i++)
            {
                if (string.Equals(tags.tags[i], tag, StringComparison.Ordinal))
                    return true;
            }
            return false;
        }

        /// <summary>Returns all survivor IDs that have enrichment data.</summary>
        public IEnumerable<string> GetEnrichedSurvivorIds() => _survivorFields.Keys;

        /// <summary>Returns all item IDs that have tag data.</summary>
        public IEnumerable<string> GetTaggedItemIds() => _itemTags.Keys;

        /// <summary>
        /// Returns survivors matching a belief profile.
        /// </summary>
        public List<string> GetSurvivorsByBeliefProfile(string beliefProfileId)
        {
            var result = new List<string>();
            foreach (var kvp in _survivorFields)
            {
                if (string.Equals(kvp.Value.belief_profile_id, beliefProfileId, StringComparison.Ordinal))
                    result.Add(kvp.Key);
            }
            return result;
        }

        /// <summary>
        /// Returns survivors with a specific phantom background.
        /// </summary>
        public List<string> GetSurvivorsByPhantomBackground(string phantomBackgroundId)
        {
            var result = new List<string>();
            foreach (var kvp in _survivorFields)
            {
                if (string.Equals(kvp.Value.phantom_background_id, phantomBackgroundId, StringComparison.Ordinal))
                    result.Add(kvp.Key);
            }
            return result;
        }

        /// <summary>
        /// Returns items tagged as personal keepsake candidates.
        /// </summary>
        public List<string> GetKeepsakeCandidates()
        {
            var result = new List<string>();
            foreach (var kvp in _itemTags)
            {
                if (kvp.Value.tags != null && kvp.Value.tags.Contains("personal_keepsake_candidate"))
                    result.Add(kvp.Key);
            }
            return result;
        }

        internal void AddSurvivorFields(ExpansionSurvivorFields fields)
        {
            if (fields == null || string.IsNullOrEmpty(fields.survivor_id)) return;
            _survivorFields[fields.survivor_id] = fields;
        }

        internal void AddItemTags(ExpansionItemTags tags)
        {
            if (tags == null || string.IsNullOrEmpty(tags.item_id)) return;
            _itemTags[tags.item_id] = tags;
        }
    }

    // ── DTOs matching the JSON schema ────────────────────────────────────

    /// <summary>Narrative enrichment fields for a single survivor.</summary>
    [Serializable]
    public sealed class ExpansionSurvivorFields
    {
        public string survivor_id = string.Empty;
        public string phantom_background_id = string.Empty;
        public string pre_war_profession_id = string.Empty;
        public string belief_profile_id = string.Empty;
        public string personal_keepsake_item_id = string.Empty;
    }

    /// <summary>Narrative tags for a single item.</summary>
    [Serializable]
    public sealed class ExpansionItemTags
    {
        public string item_id = string.Empty;
        public List<string> tags = new List<string>();
    }

    // ── Loader ───────────────────────────────────────────────────────────

    /// <summary>
    /// Loads <c>expansion_survivor_fields.json</c> and <c>expansion_item_tags.json</c>
    /// into an <see cref="ExpansionEnrichmentCatalog"/>.
    /// </summary>
    public sealed class ExpansionEnrichmentCatalogLoader
    {
        public const string SurvivorFieldsFile = "expansion_survivor_fields.json";
        public const string ItemTagsFile = "expansion_item_tags.json";

        private readonly IFileIO _files;
        private readonly IJsonSerializer _json;
        private readonly ILog _log;

        public ExpansionEnrichmentCatalogLoader(IFileIO files, IJsonSerializer json, ILog? log = null)
        {
            _files = files ?? throw new ArgumentNullException(nameof(files));
            _json = json ?? throw new ArgumentNullException(nameof(json));
            _log = log ?? NullLog.Instance;
        }

        public ExpansionEnrichmentCatalog Load(string dataDirectory)
        {
            var catalog = new ExpansionEnrichmentCatalog();
            if (string.IsNullOrEmpty(dataDirectory) || !_files.DirectoryExists(dataDirectory))
            {
                _log.Warn("Expansion enrichment directory missing: " + dataDirectory);
                return catalog;
            }

            LoadSurvivorFields(_files.Combine(dataDirectory, SurvivorFieldsFile), catalog);
            LoadItemTags(_files.Combine(dataDirectory, ItemTagsFile), catalog);

            _log.Info($"Expansion enrichment loaded: {catalog.SurvivorFieldCount} survivors, " +
                      $"{catalog.ItemTagCount} tagged items");

            return catalog;
        }

        private void LoadSurvivorFields(string path, ExpansionEnrichmentCatalog catalog)
        {
            if (!_files.FileExists(path)) { _log.Warn("Missing: " + path); return; }
            try
            {
                var list = CatalogLocator.LoadWrappedList<ExpansionSurvivorFields>(_files.ReadAllText(path), SystemTextJsonSerializer.Options);
                if (list == null) return;
                for (int i = 0; i < list.Count; i++)
                    catalog.AddSurvivorFields(list[i]);
            }
            catch (Exception ex) { _log.Warn("Parse failed " + path + ": " + ex.Message); }
        }

        private void LoadItemTags(string path, ExpansionEnrichmentCatalog catalog)
        {
            if (!_files.FileExists(path)) { _log.Warn("Missing: " + path); return; }
            try
            {
                var list = CatalogLocator.LoadWrappedList<ExpansionItemTags>(_files.ReadAllText(path), SystemTextJsonSerializer.Options);
                if (list == null) return;
                for (int i = 0; i < list.Count; i++)
                    catalog.AddItemTags(list[i]);
            }
            catch (Exception ex) { _log.Warn("Parse failed " + path + ": " + ex.Message); }
        }
    }
}
