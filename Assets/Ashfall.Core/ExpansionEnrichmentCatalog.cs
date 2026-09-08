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

        /// <summary>
        /// Returns survivors matching a pre-war profession ID.
        /// </summary>
        public List<string> GetSurvivorsByProfession(string professionId)
        {
            var result = new List<string>();
            if (string.IsNullOrEmpty(professionId)) return result;
            foreach (var kvp in _survivorFields)
            {
                if (string.Equals(kvp.Value.pre_war_profession_id, professionId, StringComparison.Ordinal))
                    result.Add(kvp.Key);
            }
            return result;
        }

        /// <summary>
        /// Returns the personal keepsake item ID for a survivor, or empty string.
        /// </summary>
        public string GetKeepsakeItemId(string survivorId)
        {
            var fields = GetSurvivorFields(survivorId);
            return fields?.personal_keepsake_item_id ?? string.Empty;
        }

        /// <summary>
        /// Returns the philosophical stance for a survivor, or empty string.
        /// </summary>
        public string GetPhilosophicalStance(string survivorId)
        {
            var fields = GetSurvivorFields(survivorId);
            return fields?.philosophical_stance ?? string.Empty;
        }

        /// <summary>
        /// Returns the manifesto law code for a survivor, or empty string.
        /// </summary>
        public string GetManifestoLawCode(string survivorId)
        {
            var fields = GetSurvivorFields(survivorId);
            return fields?.manifesto_law_code ?? string.Empty;
        }

        public void AddSurvivorFields(ExpansionSurvivorFields fields)
        {
            MergeSurvivorFields(fields, isSpecialistOverlay: false);
        }

        /// <summary>
        /// Merges survivor enrichment fields deterministically.
        /// Baseline fields take precedence for core fields; specialist overlays
        /// contribute supplemental fields (e.g. stance, manifesto_law_code) without
        /// clobbering existing non-empty values.
        /// </summary>
        public void MergeSurvivorFields(ExpansionSurvivorFields fields, bool isSpecialistOverlay = false)
        {
            if (fields == null || string.IsNullOrEmpty(fields.survivor_id)) return;

            if (!_survivorFields.TryGetValue(fields.survivor_id, out var existing))
            {
                _survivorFields[fields.survivor_id] = fields;
                return;
            }

            // Field-level merge
            if (!string.IsNullOrEmpty(fields.phantom_background_id))
            {
                if (string.IsNullOrEmpty(existing.phantom_background_id) || !isSpecialistOverlay)
                    existing.phantom_background_id = fields.phantom_background_id;
            }

            if (!string.IsNullOrEmpty(fields.pre_war_profession_id))
            {
                if (string.IsNullOrEmpty(existing.pre_war_profession_id) || !isSpecialistOverlay)
                    existing.pre_war_profession_id = fields.pre_war_profession_id;
            }

            if (!string.IsNullOrEmpty(fields.belief_profile_id))
            {
                if (string.IsNullOrEmpty(existing.belief_profile_id) || !isSpecialistOverlay)
                    existing.belief_profile_id = fields.belief_profile_id;
            }

            if (!string.IsNullOrEmpty(fields.personal_keepsake_item_id))
            {
                if (string.IsNullOrEmpty(existing.personal_keepsake_item_id) || !isSpecialistOverlay)
                    existing.personal_keepsake_item_id = fields.personal_keepsake_item_id;
            }

            if (!string.IsNullOrEmpty(fields.philosophical_stance))
            {
                existing.philosophical_stance = fields.philosophical_stance;
            }

            if (!string.IsNullOrEmpty(fields.manifesto_law_code))
            {
                existing.manifesto_law_code = fields.manifesto_law_code;
            }
        }

        public void AddItemTags(ExpansionItemTags tags)
        {
            if (tags == null || string.IsNullOrEmpty(tags.item_id)) return;
            if (_itemTags.TryGetValue(tags.item_id, out var existing))
            {
                if (tags.tags != null && existing.tags != null)
                {
                    for (int i = 0; i < tags.tags.Count; i++)
                    {
                        string t = tags.tags[i];
                        if (!string.IsNullOrEmpty(t) && !existing.tags.Contains(t))
                            existing.tags.Add(t);
                    }
                }
            }
            else
            {
                _itemTags[tags.item_id] = tags;
            }
        }
    }

    // ── DTOs matching the JSON schema ────────────────────────────────────

    /// <summary>Narrative enrichment fields for a single survivor.</summary>
    [Serializable]
    public sealed class ExpansionSurvivorFields
    {
        public string survivor_id { get; set; } = string.Empty;
        public string phantom_background_id { get; set; } = string.Empty;
        public string pre_war_profession_id { get; set; } = string.Empty;
        public string belief_profile_id { get; set; } = string.Empty;
        public string personal_keepsake_item_id { get; set; } = string.Empty;
        public string philosophical_stance { get; set; } = string.Empty;
        /// <summary>Alias for philosophical_stance in specialist overlay schemas.</summary>
        public string stance { get => philosophical_stance; set => philosophical_stance = value; }
        public string manifesto_law_code { get; set; } = string.Empty;
    }

    /// <summary>Narrative tags for a single item.</summary>
    [Serializable]
    public sealed class ExpansionItemTags
    {
        public string item_id { get; set; } = string.Empty;
        public List<string> tags { get; set; } = new List<string>();
    }

    // ── Loader ───────────────────────────────────────────────────────────

    /// <summary>
    /// Loads <c>expansion_survivor_fields.json</c> and <c>expansion_item_tags.json</c>,
    /// along with specialist overlays (<c>deep_lore_survivor_fields.json</c>,
    /// <c>antigravity_survivor_fields.json</c>), into an <see cref="ExpansionEnrichmentCatalog"/>.
    /// </summary>
    public sealed class ExpansionEnrichmentCatalogLoader
    {
        public const string SurvivorFieldsFile = "expansion_survivor_fields.json";
        public const string ItemTagsFile = "expansion_item_tags.json";
        public const string DeepLoreSurvivorFieldsFile = "deep_lore_survivor_fields.json";
        public const string AntigravitySurvivorFieldsFile = "antigravity_survivor_fields.json";

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

            // 1. Primary authored baseline
            LoadSurvivorFields(_files.Combine(dataDirectory, SurvivorFieldsFile), catalog, isSpecialistOverlay: false);

            // 2. Specialist overlays
            LoadSurvivorFields(_files.Combine(dataDirectory, DeepLoreSurvivorFieldsFile), catalog, isSpecialistOverlay: true);
            LoadSurvivorFields(_files.Combine(dataDirectory, AntigravitySurvivorFieldsFile), catalog, isSpecialistOverlay: true);

            // 3. Item tags
            LoadItemTags(_files.Combine(dataDirectory, ItemTagsFile), catalog);

            _log.Info($"Expansion enrichment loaded: {catalog.SurvivorFieldCount} survivors, " +
                      $"{catalog.ItemTagCount} tagged items");

            return catalog;
        }

        private void LoadSurvivorFields(string path, ExpansionEnrichmentCatalog catalog, bool isSpecialistOverlay)
        {
            if (!_files.FileExists(path))
            {
                if (!isSpecialistOverlay) _log.Warn("Missing: " + path);
                return;
            }
            try
            {
                var list = CatalogLocator.LoadWrappedList<ExpansionSurvivorFields>(_files.ReadAllText(path), SystemTextJsonSerializer.Options);
                if (list == null) return;
                var seen = new HashSet<string>(StringComparer.Ordinal);
                for (int i = 0; i < list.Count; i++)
                {
                    var item = list[i];
                    if (item == null || string.IsNullOrEmpty(item.survivor_id)) continue;
                    if (!seen.Add(item.survivor_id))
                    {
                        _log.Warn($"Duplicate survivor_id '{item.survivor_id}' in {path}");
                        continue;
                    }
                    catalog.MergeSurvivorFields(item, isSpecialistOverlay);
                }
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
                var seen = new HashSet<string>(StringComparer.Ordinal);
                for (int i = 0; i < list.Count; i++)
                {
                    var item = list[i];
                    if (item == null || string.IsNullOrEmpty(item.item_id)) continue;
                    seen.Add(item.item_id);
                    catalog.AddItemTags(item);
                }
            }
            catch (Exception ex) { _log.Warn("Parse failed " + path + ": " + ex.Message); }
        }
    }
}
