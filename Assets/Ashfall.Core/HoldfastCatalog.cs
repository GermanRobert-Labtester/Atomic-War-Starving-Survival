using System;
using System.Collections.Generic;
using Ashfall.Core.Inventory;

namespace Ashfall.Core
{
    public class HoldfastLocationEntry
    {
        public string id;
        public string displayName;
        public string inspect;
        public string description;
        public float dangerLevel;
        public float travelHours;
        public float baseRadsPerHour;
        public string region;
        public bool overlay_on_unlock;
        public bool recast_always;
    }

    public class HoldfastQuestStageEntry
    {
        public string id;
        public string text;
    }

    public class HoldfastQuestChoiceEntry
    {
        public string id;
        public string text;
        public string set_flag;
    }

    public class HoldfastQuestEntry
    {
        public string id;
        public string display_name;
        public string type;
        public string briefing;
        public string prereq_quest_id;
        public int min_day;
        public HoldfastQuestStageEntry[] stages;
        public HoldfastQuestChoiceEntry[] choices;
        public string knowledge_key;
        public string target_location_id;

        public int StageCount => stages != null ? stages.Length : 0;
    }

    public sealed class HoldfastCatalog
    {
        public List<HoldfastLocationEntry> Locations { get; } = new List<HoldfastLocationEntry>();
        public List<HoldfastQuestEntry> Quests { get; } = new List<HoldfastQuestEntry>();
        public HoldfastItemsCatalog Items { get; set; } = HoldfastItemsCatalog.Empty();
        public HoldfastFactionsCatalog Factions { get; set; } = HoldfastFactionsCatalog.Empty();

        public HoldfastItemDefinition GetItem(string id) => Items != null ? Items.GetById(id) : null;
        public HoldfastFactionEntry GetFaction(string id) => Factions != null ? Factions.GetById(id) : null;

        public HoldfastLocationEntry GetLocation(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            for (int i = 0; i < Locations.Count; i++)
                if (Locations[i] != null && Locations[i].id == id)
                    return Locations[i];
            return null;
        }

        public HoldfastQuestEntry GetQuest(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            for (int i = 0; i < Quests.Count; i++)
                if (Quests[i] != null && Quests[i].id == id)
                    return Quests[i];
            return null;
        }
    }

    /// <summary>
    /// JSON DTO for holdfast_items.json. HoldfastItemDefinition is immutable
    /// (no setters), so deserialise into this DTO and convert in the loader.
    /// </summary>
    public sealed class HoldfastItemDto
    {
        public string id { get; set; } = string.Empty;
        public string displayName { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public double tradeValue { get; set; } = 0.0;
        public double weight { get; set; } = 1.0;
        public string type { get; set; } = "resource";
        public int stackMax { get; set; } = 99;
        public double thirstRestore { get; set; } = 0.0;
        public double hungerRestore { get; set; } = 0.0;
        public double moraleEffect { get; set; } = 0.0;
    }

    /// <summary>
    /// JSON DTO for holdfast_factions.json. Avoids the alias collision in
    /// HoldfastFactionEntry (it defines both `id` and `Id`, which fall over
    /// when PropertyNameCaseInsensitive is on), so deserialise into this DTO
    /// and convert in the loader.
    /// </summary>
    public sealed class HoldfastFactionDto
    {
        public string id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public string alignment { get; set; } = string.Empty;
        public string home_region { get; set; } = string.Empty;
        public bool is_active { get; set; } = true;
        public float trust { get; set; } = 0f;
        public string[] wants { get; set; } = Array.Empty<string>();
        public string[] offers { get; set; } = Array.Empty<string>();
        public string signature_quote { get; set; } = string.Empty;
        public string access_rule { get; set; } = string.Empty;
        public string badge_asset_id { get; set; } = string.Empty;
    }

    /// <summary>
    /// Loads holdfast_locations.json / holdfast_quests.json / holdfast_items.json /
    /// holdfast_factions.json from the shared StreamingAssets/Data directory.
    /// No ScriptableObject materialisation.
    /// </summary>
    public sealed class HoldfastCatalogLoader
    {
        public const string LocationsFile = "holdfast_locations.json";
        public const string QuestsFile = "holdfast_quests.json";
        public const string ItemsFile = "holdfast_items.json";
        public const string FactionsFile = "holdfast_factions.json";

        private readonly IFileIO _files;
        private readonly IJsonSerializer _json;
        private readonly ILog _log;

        public HoldfastCatalogLoader(IFileIO files, IJsonSerializer json, ILog log = null)
        {
            _files = files ?? throw new ArgumentNullException(nameof(files));
            _json = json ?? throw new ArgumentNullException(nameof(json));
            _log = log ?? NullLog.Instance;
        }

        /// <param name="expansionUnlocked">
        /// When false, the 26 District 8 cards stay dark. recast_always (3 Sector 4 ids)
        /// still load so copy overlays can apply. overlay_on_unlock rows wait on unlock.
        /// </param>
        public HoldfastCatalog Load(string dataDirectory, bool expansionUnlocked = true)
        {
            var catalog = new HoldfastCatalog();
            if (string.IsNullOrEmpty(dataDirectory) || !_files.DirectoryExists(dataDirectory))
            {
                _log.Warn("Holdfast catalog directory missing: " + dataDirectory);
                return catalog;
            }

            LoadLocations(_files.Combine(dataDirectory, LocationsFile), catalog.Locations, expansionUnlocked);
            LoadList(_files.Combine(dataDirectory, QuestsFile), catalog.Quests, "quests");
            LoadItems(_files.Combine(dataDirectory, ItemsFile), catalog.Items, "items");
            LoadFactions(_files.Combine(dataDirectory, FactionsFile), catalog.Factions, "factions");
            return catalog;
        }

        public static bool IncludeLocation(HoldfastLocationEntry e, bool expansionUnlocked)
        {
            if (e == null || string.IsNullOrEmpty(e.id)) return false;
            if (e.recast_always) return true;
            return expansionUnlocked;
        }

        /// <summary>Strips authoring notes such as "(existing)" / "(recast; existing id)" from display names.</summary>
        public static string StripAuthorNotes(string displayName)
        {
            if (string.IsNullOrEmpty(displayName)) return displayName;
            int idx = displayName.IndexOf(" (existing", StringComparison.Ordinal);
            if (idx > 0) return displayName.Substring(0, idx);
            idx = displayName.IndexOf(" (recast", StringComparison.Ordinal);
            if (idx > 0) return displayName.Substring(0, idx);
            return displayName;
        }

        private void LoadLocations(string path, List<HoldfastLocationEntry> dest, bool expansionUnlocked)
        {
            if (!_files.FileExists(path))
            {
                _log.Warn("Holdfast locations file missing: " + path);
                return;
            }

            try
            {
                string json = _files.ReadAllText(path);
                var items = _json.Deserialize<List<HoldfastLocationEntry>>(json);
                if (items == null) return;
                for (int i = 0; i < items.Count; i++)
                {
                    var e = items[i];
                    if (e == null) continue;
                    e.displayName = StripAuthorNotes(e.displayName);
                    if (!IncludeLocation(e, expansionUnlocked)) continue;
                    dest.Add(e);
                }
            }
            catch (Exception e)
            {
                _log.Error("Holdfast locations parse failed: " + e.Message);
            }
        }

        private void LoadList<T>(string path, List<T> dest, string label) where T : class
        {
            if (!_files.FileExists(path))
            {
                _log.Warn("Holdfast " + label + " file missing: " + path);
                return;
            }

            try
            {
                string json = _files.ReadAllText(path);
                var items = _json.Deserialize<List<T>>(json);
                if (items == null) return;
                for (int i = 0; i < items.Count; i++)
                {
                    if (items[i] != null)
                        dest.Add(items[i]);
                }
            }
            catch (Exception e)
            {
                _log.Error("Holdfast " + label + " parse failed: " + e.Message);
            }
        }

        /// <summary>
        /// Loads items from holdfast_items.json into the item catalog.
        /// JSON uses camelCase fields (displayName, tradeValue, stackMax, ...).
        /// </summary>
        private void LoadItems(string path, HoldfastItemsCatalog dest, string label)
        {
            if (!_files.FileExists(path))
            {
                _log.Warn("Holdfast " + label + " file missing: " + path);
                return;
            }

            try
            {
                string json = _files.ReadAllText(path);
                var dtos = _json.Deserialize<List<HoldfastItemDto>>(json);
                if (dtos == null) return;
                for (int i = 0; i < dtos.Count; i++)
                {
                    var dto = dtos[i];
                    if (dto == null || string.IsNullOrEmpty(dto.id)) continue;
                    dest.Register(new HoldfastItemDefinition(
                        dto.id,
                        dto.displayName,
                        dto.description,
                        (float)dto.tradeValue,
                        (float)dto.weight,
                        dto.type,
                        dto.stackMax));
                }
            }
            catch (Exception e)
            {
                _log.Error("Holdfast " + label + " parse failed: " + e.Message);
            }
        }

        private void LoadFactions(string path, HoldfastFactionsCatalog dest, string label)
        {
            if (!_files.FileExists(path))
            {
                _log.Warn("Holdfast " + label + " file missing: " + path);
                return;
            }

            try
            {
                string json = _files.ReadAllText(path);
                var dtos = _json.Deserialize<List<HoldfastFactionDto>>(json);
                if (dtos == null) return;
                for (int i = 0; i < dtos.Count; i++)
                {
                    var dto = dtos[i];
                    if (dto == null || string.IsNullOrEmpty(dto.id)) continue;
                    dest.Register(new HoldfastFactionEntry(
                        dto.id,
                        dto.display_name,
                        dto.alignment,
                        dto.home_region,
                        dto.is_active,
                        dto.trust,
                        dto.wants,
                        dto.offers,
                        dto.signature_quote,
                        dto.access_rule,
                        dto.badge_asset_id));
                }
            }
            catch (Exception e)
            {
                _log.Error("Holdfast " + label + " parse failed: " + e.Message);
            }
        }
    }
}

