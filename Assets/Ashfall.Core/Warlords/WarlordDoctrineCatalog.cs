using System;
using System.Collections.Generic;

using Ashfall.Core.IO;
namespace Ashfall.Core.Warlords
{
    /// <summary>
    /// Warlord identity + config (proposed canon, modeled on faction_lore.json
    /// "warlords_sector_4" and the Unity Warlord Code tribute semantics).
    /// </summary>
    [Serializable]
    public class WarlordDef
    {
        public string faction_id = "warlords_sector_4";
        public string leader_name = "The Tollman";
        public string home_location_id = "loc_toll_house";
        public string starting_doctrine_id = "warlord_doctrine_toll";
        public string tribute_currency_item = "canned_food";
        public int tribute_interval_days = 7;
        public int tribute_base_amount = 6;
        public float tribute_escalation_factor = 1.5f;
        public float tribute_max_multiplier = 8f;
        public float short_payment_threshold = 0.9f;
        public int action_interval_days = 5;
        public int action_cooldown_days = 5;
        public int doctrine_cooldown_days = 10;
        public int report_delay_days = 3;
        public float doctrine_change_margin = 0.15f;
    }

    /// <summary>One node of the warlord territory graph (existing location ids only).</summary>
    [Serializable]
    public class WarlordTerritoryNodeDef
    {
        public string location_id = string.Empty;
        public bool home;
        public int supply_value;
        public bool chokepoint;
        public int defense_value;
        public List<string> neighbors = new List<string>();
    }

    /// <summary>One data-driven doctrine.</summary>
    [Serializable]
    public class WarlordDoctrineDef
    {
        public string id = string.Empty;
        public string display_name = string.Empty;
        public string description = string.Empty;
        public float risk_tolerance = 0.5f;
        public string preferred_goal = string.Empty;
        public List<string> eligible_actions = new List<string>();
        public Dictionary<string, int> action_weights = new Dictionary<string, int>();
        public List<string> resource_priority = new List<string>();
        public string target_rule = "nearest_undefended";
        public string journal_key = string.Empty;
        public string radio_key = string.Empty;
        public List<WarlordDoctrineTransitionDef> transitions = new List<WarlordDoctrineTransitionDef>();
    }

    /// <summary>Data-driven transition rule (signal, condition, threshold).</summary>
    [Serializable]
    public class WarlordDoctrineTransitionDef
    {
        public string to = string.Empty;
        public string signal = string.Empty;   // supply_ratio | failure_streak | success_streak | contested_count | player_tribute_reliability | environment_hazard
        public string condition = "gte";       // gte | lt
        public float threshold = 0f;
    }

    /// <summary>Explicit alias-conflict report — never silently merged.</summary>
    [Serializable]
    public class WarlordAliasWarningDef
    {
        public string canonical = string.Empty;
        public List<string> aliases_not_merged = new List<string>();
        public string notes = string.Empty;
    }

    /// <summary>Loaded warlord catalog: identity, territory graph, doctrines, alias warnings.</summary>
    public sealed class WarlordDoctrineCatalog
    {
        public WarlordDef Warlord { get; set; } = new WarlordDef();
        public List<WarlordTerritoryNodeDef> Territory { get; } = new List<WarlordTerritoryNodeDef>();
        public List<WarlordDoctrineDef> Doctrines { get; } = new List<WarlordDoctrineDef>();
        public List<WarlordAliasWarningDef> AliasWarnings { get; } = new List<WarlordAliasWarningDef>();
        /// <summary>Authored collector voice lines (demand/paid/short/refused) — data-driven prose.</summary>
        public Dictionary<string, List<string>> CollectorVoice { get; } = new Dictionary<string, List<string>>(StringComparer.Ordinal);

        /// <summary>Picks a collector line deterministically from the authored set (seeded by day).</summary>
        public string CollectorLine(string state, int day)
        {
            if (!CollectorVoice.TryGetValue(state, out var lines) || lines == null || lines.Count == 0)
                return string.Empty;
            int n = day < 0 ? -day : day;
            return lines[n % lines.Count];
        }

        public WarlordDoctrineDef? GetDoctrine(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            for (int i = 0; i < Doctrines.Count; i++)
                if (Doctrines[i] != null && Doctrines[i].id == id)
                    return Doctrines[i];
            return null;
        }

        public WarlordTerritoryNodeDef? GetNode(string locationId)
        {
            if (string.IsNullOrEmpty(locationId)) return null;
            for (int i = 0; i < Territory.Count; i++)
                if (Territory[i] != null && Territory[i].location_id == locationId)
                    return Territory[i];
            return null;
        }

        /// <summary>Ordinal-ordered adjacency lookup.</summary>
        public List<string> Neighbors(string locationId)
        {
            var node = GetNode(locationId);
            if (node == null || node.neighbors == null) return new List<string>();
            var list = new List<string>(node.neighbors);
            list.Sort(string.CompareOrdinal);
            return list;
        }
    }

    /// <summary>
    /// Loads warlord_doctrines.json via the engine-agnostic ports. The loader
    /// fails loudly on a missing or malformed catalog — the warlord AI must
    /// never silently run with an empty doctrine set.
    /// </summary>
    public static class WarlordDoctrineCatalogLoader
    {
        public const string FileName = "warlord_doctrines.json";

        public static WarlordDoctrineCatalog Load(string dataDirectory, IFileIO files, IJsonSerializer json)
        {
            if (files == null || json == null || string.IsNullOrEmpty(dataDirectory))
                throw new InvalidOperationException("WarlordDoctrineCatalogLoader: missing ports or data directory.");

            string path = files.Combine(dataDirectory, FileName);
            if (!files.FileExists(path))
                throw new InvalidOperationException("WarlordDoctrineCatalogLoader: " + FileName + " missing in " + dataDirectory);

            string raw = files.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(raw))
                throw new InvalidOperationException("WarlordDoctrineCatalogLoader: " + FileName + " is empty.");

            var catalog = json.Deserialize<WarlordDoctrineContainer>(raw)?.ToCatalog();
            if (catalog == null)
                throw new InvalidOperationException("WarlordDoctrineCatalogLoader: " + FileName + " failed to parse.");
            if (catalog.Doctrines.Count < 3)
                throw new InvalidOperationException("WarlordDoctrineCatalogLoader: " + FileName + " must define at least 3 doctrines.");
            if (catalog.Territory.Count < 2)
                throw new InvalidOperationException("WarlordDoctrineCatalogLoader: " + FileName + " must define a territory graph.");
            return catalog;
        }
    }

    /// <summary>Raw JSON container mirroring warlord_doctrines.json.</summary>
    [Serializable]
    public class WarlordDoctrineContainer
    {
        public int schema_version = 1;
        public WarlordDef warlord = new WarlordDef();
        public List<WarlordTerritoryNodeDef> territory = new List<WarlordTerritoryNodeDef>();
        public List<WarlordDoctrineDef> doctrines = new List<WarlordDoctrineDef>();
        public List<WarlordAliasWarningDef> faction_alias_warnings = new List<WarlordAliasWarningDef>();
        public Dictionary<string, List<string>> collector_voice = new Dictionary<string, List<string>>(StringComparer.Ordinal);

        public WarlordDoctrineCatalog ToCatalog()
        {
            var catalog = new WarlordDoctrineCatalog { Warlord = warlord ?? new WarlordDef() };
            if (territory != null)
                for (int i = 0; i < territory.Count; i++)
                    if (territory[i] != null && !string.IsNullOrEmpty(territory[i].location_id))
                        catalog.Territory.Add(territory[i]);
            if (doctrines != null)
                for (int i = 0; i < doctrines.Count; i++)
                    if (doctrines[i] != null && !string.IsNullOrEmpty(doctrines[i].id))
                        catalog.Doctrines.Add(doctrines[i]);
            if (faction_alias_warnings != null)
                for (int i = 0; i < faction_alias_warnings.Count; i++)
                    if (faction_alias_warnings[i] != null)
                        catalog.AliasWarnings.Add(faction_alias_warnings[i]);
            if (collector_voice != null)
                foreach (var kv in collector_voice)
                    if (kv.Value != null)
                        catalog.CollectorVoice[kv.Key] = new List<string>(kv.Value);
            return catalog;
        }
    }

    /// <summary>
    /// Loud cross-reference validation for the warlord catalog. Every territory
    /// location must exist in a location catalog, the warlord faction must exist
    /// in faction_lore.json, the tribute currency must exist in an item catalog,
    /// and doctrine transitions must target defined doctrines. Faction alias
    /// conflicts are REPORTED (never merged). Returns a report; errors throw so
    /// a malformed catalog can never silently run an empty warlord AI.
    /// </summary>
    public static class WarlordCatalogValidator
    {
        private static readonly string[] LocationFiles =
        {
            "locations.json", "locations_expansion3.json", "year_of_ash_locations.json",
            "holdfast_locations.json", "crossing_locations.json", "dose_locations.json",
            "deep_lore_locations.json", "duty_roster_locations.json", "standing_record_locations.json"
        };

        private static readonly string[] ItemFiles =
        {
            "items.json", "black_flotilla_items.json", "holdfast_items.json",
            "crossing_items.json", "chemical_dependency_items.json", "dose_items.json"
        };

        public sealed class ValidationReport
        {
            public readonly List<string> Errors = new List<string>();
            public readonly List<string> AliasWarnings = new List<string>();
            public bool Clean => Errors.Count == 0;
        }

        public static ValidationReport Validate(WarlordDoctrineCatalog catalog, string dataDirectory, IFileIO files)
        {
            var report = new ValidationReport();
            if (catalog == null) { report.Errors.Add("warlord catalog is null"); return report; }
            if (files == null || string.IsNullOrEmpty(dataDirectory))
            { report.Errors.Add("warlord validation: missing ports or data directory"); return report; }

            var json = new SystemTextJsonSerializer();
            var locationIds = IndexIds(files, dataDirectory, LocationFiles, json);
            var itemIds = IndexIds(files, dataDirectory, ItemFiles, json);
            var factionIds = IndexFactionIds(files, dataDirectory, json);

            // Warlord identity.
            if (!factionIds.Contains(catalog.Warlord.faction_id))
                report.Errors.Add("warlord faction_id '" + catalog.Warlord.faction_id + "' not found in faction_lore.json");
            if (!locationIds.Contains(catalog.Warlord.home_location_id))
                report.Errors.Add("warlord home_location_id '" + catalog.Warlord.home_location_id + "' not found in any location catalog");
            if (!itemIds.Contains(catalog.Warlord.tribute_currency_item))
                report.Errors.Add("warlord tribute_currency_item '" + catalog.Warlord.tribute_currency_item + "' not found in any item catalog");

            // Territory graph: nodes exist; neighbors exist; adjacency is symmetric.
            var nodeIds = new HashSet<string>(StringComparer.Ordinal);
            for (int i = 0; i < catalog.Territory.Count; i++)
            {
                var node = catalog.Territory[i];
                if (node == null || string.IsNullOrEmpty(node.location_id)) continue;
                nodeIds.Add(node.location_id);
                if (!locationIds.Contains(node.location_id))
                    report.Errors.Add("warlord territory location '" + node.location_id + "' not found in any location catalog");
            }
            if (catalog.Territory.Count != nodeIds.Count)
                report.Errors.Add("warlord territory graph contains duplicate location ids");
            for (int i = 0; i < catalog.Territory.Count; i++)
            {
                var node = catalog.Territory[i];
                if (node == null || node.neighbors == null) continue;
                for (int j = 0; j < node.neighbors.Count; j++)
                {
                    string n = node.neighbors[j];
                    if (!nodeIds.Contains(n))
                    {
                        report.Errors.Add("warlord territory neighbor '" + n + "' of '" + node.location_id + "' is not a territory node");
                        continue;
                    }
                    if (!IsMutual(catalog, n, node.location_id))
                        report.Errors.Add("warlord territory adjacency is not symmetric between '" + node.location_id + "' and '" + n + "'");
                }
            }
            if (catalog.GetNode(catalog.Warlord.home_location_id) == null)
                report.Errors.Add("warlord home_location_id is not a territory node");

            // Doctrines: transition targets exist; weights reference eligible actions.
            var doctrineIds = new HashSet<string>(StringComparer.Ordinal);
            for (int i = 0; i < catalog.Doctrines.Count; i++)
                if (catalog.Doctrines[i] != null && !string.IsNullOrEmpty(catalog.Doctrines[i].id))
                    doctrineIds.Add(catalog.Doctrines[i].id);
            for (int i = 0; i < catalog.Doctrines.Count; i++)
            {
                var d = catalog.Doctrines[i];
                if (d == null) continue;
                if (d.transitions != null)
                {
                    for (int j = 0; j < d.transitions.Count; j++)
                    {
                        var t = d.transitions[j];
                        if (t == null) continue;
                        if (!doctrineIds.Contains(t.to))
                            report.Errors.Add("warlord doctrine '" + d.id + "' transitions to unknown doctrine '" + t.to + "'");
                        if (string.IsNullOrEmpty(t.signal))
                            report.Errors.Add("warlord doctrine '" + d.id + "' has a transition without a signal");
                    }
                }
                if (d.action_weights != null)
                {
                    foreach (var kv in d.action_weights)
                    {
                        if (d.eligible_actions == null || !d.eligible_actions.Contains(kv.Key))
                            report.Errors.Add("warlord doctrine '" + d.id + "' weights action '" + kv.Key + "' that is not eligible");
                    }
                }
                // Plan 10 remediation: every resource_priority token must
                // resolve to a known item so the doctrine AI can actually
                // score it. Reject orphans loudly (no silent skips).
                if (d.resource_priority != null)
                {
                    for (int j = 0; j < d.resource_priority.Count; j++)
                    {
                        string rp = d.resource_priority[j];
                        if (string.IsNullOrEmpty(rp)) continue;
                        if (!itemIds.Contains(rp))
                            report.Errors.Add("warlord doctrine '" + d.id + "' resource_priority '" + rp + "' not found in any item catalog");
                    }
                }
            }

            // Plan 10 remediation: validate faction-lore tribute_demands
            // references too. Each non-empty tribute_demands list must
            // resolve to a known item id when the canonical faction is the
            // Warlord (the only player-facing traffic in the catalogue
            // today). For other factions we report rather than reject to
            // keep silent alias flavours; the report is informational.
            if (factionIds.Count > 0)
            {
                string factionPath = files.Combine(dataDirectory, "faction_lore.json");
                if (files.FileExists(factionPath))
                {
                    try
                    {
                        // Use LoadWrappedList — faction_lore.json is shaped as
                        // { "items": [...] } and we want to drill into that
                        // array. A naked Deserialize<List<...>> would fail
                        // because the root is an object, not an array.
                        var lore = CatalogLocator.LoadWrappedList<FactionTributeProbe>(files.ReadAllText(factionPath), SystemTextJsonSerializer.Options);
                        if (lore != null)
                        {
                            for (int i = 0; i < lore.Count; i++)
                            {
                                var entry = lore[i];
                                if (entry == null || entry.tribute_demands == null) continue;
                                bool isCanonical = !string.IsNullOrEmpty(entry.faction_id)
                                    && entry.faction_id == catalog.Warlord.faction_id;
                                for (int j = 0; j < entry.tribute_demands.Count; j++)
                                {
                                    string td = entry.tribute_demands[j];
                                    if (string.IsNullOrEmpty(td)) continue;
                                    bool resolves = itemIds.Contains(td);
                                    if (!resolves && isCanonical)
                                        report.Errors.Add("faction '" + entry.faction_id + "' tribute_demands item '" + td + "' not found in any item catalog");
                                    else if (!resolves)
                                        report.AliasWarnings.Add("faction '" + entry.faction_id
                                            + "' tribute_demands item '" + td
                                            + "' is not present in any item catalog (non-blocking)");
                                }
                            }
                        }
                    }
                    catch (Exception ex_CATDIAG)
                    {
                        CatalogDiagnostics.Warn(factionPath, "FactionTributeProbe", ex_CATDIAG);
                    }
                }
            }

            // Alias conflicts are reported, never silently canonized.
            for (int i = 0; i < catalog.AliasWarnings.Count; i++)
            {
                var w = catalog.AliasWarnings[i];
                if (w == null) continue;
                for (int j = 0; j < w.aliases_not_merged.Count; j++)
                    report.AliasWarnings.Add("alias-not-merged: canonical '" + w.canonical
                        + "' vs '" + w.aliases_not_merged[j] + "': " + w.notes);
            }
            return report;
        }

        private static bool IsMutual(WarlordDoctrineCatalog catalog, string a, string b)
        {
            var neighbors = catalog.Neighbors(a);
            for (int i = 0; i < neighbors.Count; i++)
                if (neighbors[i] == b)
                    return true;
            return false;
        }

        private static HashSet<string> IndexIds(IFileIO files, string dataDir, string[] names, IJsonSerializer json)
        {
            var ids = new HashSet<string>(StringComparer.Ordinal);
            for (int f = 0; f < names.Length; f++)
            {
                string path = files.Combine(dataDir, names[f]);
                if (!files.FileExists(path)) continue;
                string text = files.ReadAllText(path);
                try
                {
                    var list = CatalogLocator.LoadWrappedList<WarlordIdProbe>(text, SystemTextJsonSerializer.Options);
                    if (list != null)
                    {
                        for (int i = 0; i < list.Count; i++)
                            if (list[i] != null && !string.IsNullOrEmpty(list[i].id))
                                ids.Add(list[i].id);
                        continue;
                    }
                }
                catch (Exception) { /* Fallback below */ }

                try
                {
                    var wrap = json.Deserialize<WarlordCatalogWrapperProbe>(text);
                    if (wrap != null)
                    {
                        var list = (wrap.locations != null && wrap.locations.Count > 0) ? wrap.locations : wrap.items;
                        if (list != null)
                        {
                            for (int i = 0; i < list.Count; i++)
                                if (list[i] != null && !string.IsNullOrEmpty(list[i].id))
                                    ids.Add(list[i].id);
                        }
                    }
                }
                catch (Exception ex_CATDIAG)
                {
                    CatalogDiagnostics.Warn(path, "WarlordCatalogWrapperProbe", ex_CATDIAG);
                }
            }
            return ids;
        }

        private static HashSet<string> IndexFactionIds(IFileIO files, string dataDir, IJsonSerializer json)
        {
            var ids = new HashSet<string>(StringComparer.Ordinal);
            string path = files.Combine(dataDir, "faction_lore.json");
            if (!files.FileExists(path)) return ids;
            try
            {
                var list = CatalogLocator.LoadWrappedList<WarlordFactionProbe>(files.ReadAllText(path), SystemTextJsonSerializer.Options);
                if (list == null) return ids;
                for (int i = 0; i < list.Count; i++)
                    if (list[i] != null && !string.IsNullOrEmpty(list[i].faction_id))
                        ids.Add(list[i].faction_id);
            }
            catch (Exception ex_CATDIAG)
            {
                CatalogDiagnostics.Warn(path, "WarlordFactionProbe list", ex_CATDIAG);
            }
            return ids;
        }
    }

    /// <summary>Generic id probe for the flat catalog files.</summary>
    [Serializable]
    public class WarlordIdProbe
    {
        public string id = string.Empty;
    }

    [Serializable]
    public class WarlordFactionProbe
    {
        public string faction_id = string.Empty;
    }

    [Serializable]
    public class FactionTributeProbe
    {
        public string faction_id = string.Empty;
        public List<string> tribute_demands = new List<string>();
    }

    [Serializable]
    public class WarlordCatalogWrapperProbe
    {
        public System.Collections.Generic.List<WarlordIdProbe> locations = new System.Collections.Generic.List<WarlordIdProbe>();
        public System.Collections.Generic.List<WarlordIdProbe> items = new System.Collections.Generic.List<WarlordIdProbe>();
    }
}
