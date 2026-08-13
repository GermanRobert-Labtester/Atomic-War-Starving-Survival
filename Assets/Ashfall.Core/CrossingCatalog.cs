using System;
using System.Collections.Generic;

namespace Ashfall.Core
{
    public class CrossingFactionEntry
    {
        public string id;
        public string display_name;
        public string alignment;
        public string home_region;
        public bool is_active;
        public float trust;
        public string[] wants;
        public string[] offers;
        public string signature_quote;
        public string access_rule;
        public string badge_asset_id;
    }

    public class CrossingLocationEntry
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

    public class CrossingQuestStageEntry
    {
        public string id;
        public string text;
    }

    public class CrossingQuestChoiceEntry
    {
        public string id;
        public string text;
        public string set_flag;
    }

    public class CrossingQuestEntry
    {
        public string id;
        public string display_name;
        public string type;
        public string briefing;
        public string prereq_quest_id;
        public int min_day;
        public CrossingQuestStageEntry[] stages;
        public CrossingQuestChoiceEntry[] choices;
        public string knowledge_key;
        public string target_location_id;

        public int StageCount => stages != null ? stages.Length : 0;
    }

    public class CrossingItemEntry
    {
        public string id;
        public string displayName;
        public string description;
        public string type;
        public int stackMax;
        public float weight;
        public float tradeValue;
        public float thirstRestore;
        public float hungerRestore;
        public float moraleEffect;
    }

    public class CrossingChoiceEntry
    {
        public string text;
        public string[] cost_items;
        public string result;
    }

    public class CrossingEncounterEntry
    {
        public string id;
        public string name;
        public string target_location;
        public string description;
        public string threat_level;
        public CrossingChoiceEntry[] choices;
    }

    public class CrossingCrisisEntry
    {
        public string id;
        public string name;
        public string[] phases;
        public string description;
        public string resolution;
    }

    public class CrossingEncountersContainer
    {
        public CrossingEncounterEntry[] encounters;
        public CrossingCrisisEntry[] crises;
    }

    public sealed class CrossingCatalog
    {
        public List<CrossingFactionEntry> Factions { get; } = new List<CrossingFactionEntry>();
        public List<CrossingLocationEntry> Locations { get; } = new List<CrossingLocationEntry>();
        public List<CrossingQuestEntry> Quests { get; } = new List<CrossingQuestEntry>();
        public List<CrossingItemEntry> Items { get; } = new List<CrossingItemEntry>();
        public List<CrossingEncounterEntry> Encounters { get; } = new List<CrossingEncounterEntry>();
        public List<CrossingCrisisEntry> Crises { get; } = new List<CrossingCrisisEntry>();

        public CrossingFactionEntry GetFaction(string id) => Find(Factions, id, f => f.id);
        public CrossingLocationEntry GetLocation(string id) => Find(Locations, id, e => e.id);
        public CrossingQuestEntry GetQuest(string id) => Find(Quests, id, q => q.id);
        public CrossingItemEntry GetItem(string id) => Find(Items, id, item => item.id);
        public CrossingEncounterEntry GetEncounter(string id) => Find(Encounters, id, enc => enc.id);
        public CrossingCrisisEntry GetCrisis(string id) => Find(Crises, id, c => c.id);

        private static T Find<T>(List<T> list, string id, Func<T, string> getId) where T : class
        {
            if (string.IsNullOrEmpty(id) || list == null) return null;
            for (int i = 0; i < list.Count; i++)
                if (list[i] != null && getId(list[i]) == id)
                    return list[i];
            return null;
        }
    }

    /// <summary>
    /// Loads crossing_*.json from StreamingAssets/Data.
    /// Scale fix (2026-08-14): live catalogs use danger ~1–10 and rads ~18–52.
    /// Crossing cards were authored on a 0–1 / sub-1 scale; they are rescaled
    /// to danger ~3–6 and rads ~8–25 so the depot is not the safest site in the game.
    /// loc_crossing_weighbridge stays a distinct id; display is "The Deck Scale"
    /// so it is not a third Weighbridge thesis beside loc_weighbridge / loc_cut_weigh_hut.
    /// </summary>
    public sealed class CrossingCatalogLoader
    {
        public const string FactionsFile = "crossing_factions.json";
        public const string LocationsFile = "crossing_locations.json";
        public const string QuestsFile = "crossing_quests.json";
        public const string ItemsFile = "crossing_items.json";
        public const string EncountersFile = "crossing_encounters.json";

        /// <summary>Live schema: danger 3–6 after the unit fix.</summary>
        public const float MinDanger = 3f;
        public const float MaxDanger = 6f;
        /// <summary>Live schema: rads 8–25 after the unit fix.</summary>
        public const float MinRads = 8f;
        public const float MaxRads = 25f;

        private readonly IFileIO _files;
        private readonly IJsonSerializer _json;
        private readonly ILog _log;

        public CrossingCatalogLoader(IFileIO files, IJsonSerializer json, ILog log = null)
        {
            _files = files ?? throw new ArgumentNullException(nameof(files));
            _json = json ?? throw new ArgumentNullException(nameof(json));
            _log = log ?? NullLog.Instance;
        }

        public CrossingCatalog Load(string dataDirectory)
        {
            var catalog = new CrossingCatalog();
            if (string.IsNullOrEmpty(dataDirectory) || !_files.DirectoryExists(dataDirectory))
            {
                _log.Warn("Crossing catalog directory missing: " + dataDirectory);
                return catalog;
            }

            LoadList(_files.Combine(dataDirectory, FactionsFile), catalog.Factions, "factions");
            LoadList(_files.Combine(dataDirectory, LocationsFile), catalog.Locations, "locations");
            LoadList(_files.Combine(dataDirectory, QuestsFile), catalog.Quests, "quests");
            LoadList(_files.Combine(dataDirectory, ItemsFile), catalog.Items, "items");
            LoadEncountersAndCrises(_files.Combine(dataDirectory, EncountersFile), catalog);
            return catalog;
        }

        private void LoadEncountersAndCrises(string path, CrossingCatalog catalog)
        {
            if (!_files.FileExists(path))
            {
                _log.Warn("Crossing encounters file missing: " + path);
                return;
            }

            try
            {
                string json = _files.ReadAllText(path);
                var container = _json.Deserialize<CrossingEncountersContainer>(json);
                if (container?.encounters != null)
                {
                    for (int i = 0; i < container.encounters.Length; i++)
                    {
                        if (container.encounters[i] != null)
                            catalog.Encounters.Add(container.encounters[i]);
                    }
                }
                if (container?.crises != null)
                {
                    for (int i = 0; i < container.crises.Length; i++)
                    {
                        if (container.crises[i] != null)
                            catalog.Crises.Add(container.crises[i]);
                    }
                }
            }
            catch (Exception e)
            {
                _log.Error("Crossing encounters parse failed: " + e.Message);
            }
        }

        private void LoadList<T>(string path, List<T> dest, string label) where T : class
        {
            if (!_files.FileExists(path))
            {
                _log.Warn("Crossing " + label + " file missing: " + path);
                return;
            }

            try
            {
                string json = _files.ReadAllText(path);
                var items = _json.Deserialize<List<T>>(json);
                if (items == null) return;
                for (int i = 0; i < items.Count; i++)
                    if (items[i] != null)
                        dest.Add(items[i]);
            }
            catch (Exception e)
            {
                _log.Error("Crossing " + label + " parse failed: " + e.Message);
            }
        }
    }

    public static class CrossingIds
    {
        public const string Expansion = "expansion_nobodys_charter";
        public const string Region = "region_crossing";
        public const string TheVouch = "quest_crossing_the_vouch";
        public const string FirstWeigh = "quest_crossing_first_weigh";
        public const string ScaleIntegrity = "quest_crossing_scale_integrity";
        public const string TheStanding = "quest_crossing_the_standing";
        public const string TheTerms = "quest_crossing_the_terms";
        public const string ViaductGate = "loc_crossing_viaduct_gate";
        public const string Scalehouse = "loc_crossing_scalehouse";
        public const string Weighbridge = "loc_crossing_weighbridge";
        public const string NpcMattis = "npc_mattis_cray";
        public const string NpcOsran = "npc_osran_kell";
        public const string FactionScale = "faction_the_scale";
        public const string FactionUnderwrite = "faction_the_underwrite";
        public const string FactionCompact = "faction_the_compact";
    }
}
