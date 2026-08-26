using System;
using System.Collections.Generic;

namespace Ashfall.Core
{
    /// <summary>
    /// Canonical quest ID registry. <c>questline_master.json</c> declares every
    /// legitimate quest ID in the game — both those backed by full quest
    /// definitions in expansion catalogs and those reserved for planned content.
    ///
    /// Two purposes:
    /// 1. <b>Validation</b>: quest IDs in any catalog can be checked against the
    ///    registry. An authored quest whose ID is absent from the registry is
    ///    either a typo or unregistered content — both are authoring errors.
    /// 2. <b>Enumeration</b>: UI and debugging tools can list all known quest IDs,
    ///    including registered-but-contentless IDs that are reserved for future
    ///    expansions.
    ///
    /// The registry does NOT contain quest content (stages, choices, rewards).
    /// It contains only IDs. Content lives in the per-expansion quest catalogs
    /// (holdfast_quests.json, crossing_quests.json, etc.).
    /// </summary>
    public sealed class QuestlineMasterCatalog
    {
        private readonly HashSet<string> _ids = new HashSet<string>(StringComparer.Ordinal);
        private readonly List<string> _ordered = new List<string>();

        /// <summary>Total registered quest IDs.</summary>
        public int Count => _ids.Count;

        /// <summary>All registered IDs in file order.</summary>
        public IReadOnlyList<string> All => _ordered;

        /// <summary>Returns true if the quest ID is registered in the master list.</summary>
        public bool IsRegistered(string questId)
        {
            return !string.IsNullOrEmpty(questId) && _ids.Contains(questId);
        }

        /// <summary>Returns true if the quest ID is registered AND has content in at least one expansion catalog.</summary>
        public bool HasContent(string questId, params IReadOnlyList<string>[] catalogQuestIds)
        {
            if (!IsRegistered(questId)) return false;
            for (int i = 0; i < catalogQuestIds.Length; i++)
            {
                var catalog = catalogQuestIds[i];
                for (int j = 0; j < catalog.Count; j++)
                {
                    if (string.Equals(catalog[j], questId, StringComparison.Ordinal))
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Validates that every quest ID in the provided catalogs is registered.
        /// Returns the list of unregistered IDs (empty if all are registered).
        /// </summary>
        public List<string> FindUnregistered(IEnumerable<string> catalogQuestIds)
        {
            var missing = new List<string>();
            foreach (string id in catalogQuestIds)
            {
                if (!string.IsNullOrEmpty(id) && !_ids.Contains(id))
                    missing.Add(id);
            }
            return missing;
        }

        /// <summary>Registers an ID. Internal — use the loader.</summary>
        internal void Add(string id)
        {
            if (string.IsNullOrEmpty(id)) return;
            if (_ids.Add(id))
                _ordered.Add(id);
        }
    }

    /// <summary>DTO for deserializing questline_master.json entries.</summary>
    [Serializable]
    public sealed class QuestlineMasterEntry
    {
        public string id = string.Empty;
    }

    /// <summary>DTO for deserializing questline_master.json root.</summary>
    [Serializable]
    public sealed class QuestlineMasterRoot
    {
        public int schema_version;
        public List<QuestlineMasterEntry> entries = new List<QuestlineMasterEntry>();
    }

    /// <summary>
    /// Loads <c>questline_master.json</c> into a <see cref="QuestlineMasterCatalog"/>.
    /// Follows the same pattern as other catalog loaders: IFileIO + IJsonSerializer,
    /// tolerant of missing files, warns on parse failure.
    /// </summary>
    public sealed class QuestlineMasterCatalogLoader
    {
        public const string FileName = "questline_master.json";

        private readonly IFileIO _files;
        private readonly IJsonSerializer _json;
        private readonly ILog _log;

        public QuestlineMasterCatalogLoader(IFileIO files, IJsonSerializer json, ILog? log = null)
        {
            _files = files ?? throw new ArgumentNullException(nameof(files));
            _json = json ?? throw new ArgumentNullException(nameof(json));
            _log = log ?? NullLog.Instance;
        }

        public QuestlineMasterCatalog Load(string dataDirectory)
        {
            var catalog = new QuestlineMasterCatalog();
            if (string.IsNullOrEmpty(dataDirectory) || !_files.DirectoryExists(dataDirectory))
            {
                _log.Warn("Questline master directory missing: " + dataDirectory);
                return catalog;
            }

            string path = _files.Combine(dataDirectory, FileName);
            if (!_files.FileExists(path))
            {
                _log.Warn("Questline master file missing: " + path);
                return catalog;
            }

            try
            {
                string raw = _files.ReadAllText(path);
                var root = _json.Deserialize<QuestlineMasterRoot>(raw);
                if (root?.entries == null) return catalog;

                for (int i = 0; i < root.entries.Count; i++)
                {
                    var e = root.entries[i];
                    if (e == null || string.IsNullOrEmpty(e.id)) continue;
                    catalog.Add(e.id);
                }

                _log.Info("Questline master registry loaded: " + catalog.Count + " quest IDs");
            }
            catch (Exception ex)
            {
                _log.Warn("Questline master parse failed: " + ex.Message);
            }

            return catalog;
        }
    }
}
