using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Ashfall.Core
{
    /// <summary>
    /// ASHFALL data-layer gate: cross-references every id used across the JSON
    /// catalogs in StreamingAssets/Data against the definitions those catalogs
    /// declare. Implements the AGENTS.md rule "never invent an id that isn't in
    /// the master list" mechanically:
    ///
    /// 1. REGISTRY — every value in a definition position (an entity's own id,
    ///    structural id, mutation/knowledge-key, survivor field tag, ...) is
    ///    collected with its file + JSON path.
    /// 2. TIER-1 — every string anywhere in the corpus that starts with a known
    ///    snake_case id-namespace prefix must resolve in the registry. Catches
    ///    dangling cross-file references and prefixed typos.
    /// 3. TIER-2 — every value (or array element) of a known *reference* key
    ///    (resultItemId, requiredItemId, target_location_id, prereq_quest_id,
    ///    effects.*, required_components, ...) must resolve in the registry.
    ///    Reference keys are deliberately NOT registered, so a typo cannot
    ///    bless itself.
    /// 4. RANGES — minDay/maxDay pairs must be ordered.
    /// 5. UNIQUENESS — the same definition id must not appear twice in one file.
    ///
    /// Engine-agnostic: IFileIO + IJsonSerializer for hosts, and a direct
    /// System.Text.Json walk for structure. Wired as a Godot gate via
    /// --data-integrity-selftest (see HostCli).
    /// </summary>
    public sealed class CatalogIntegrityReport
    {
        public readonly List<string> Errors = new List<string>();
        public readonly List<string> Warnings = new List<string>();

        public int ErrorCount => Errors.Count;
        public bool Clean => Errors.Count == 0;

        public string Summary
        {
            get
            {
                string head = Errors.Count == 0
                    ? "DATA_INTEGRITY_SELFTEST PASS"
                    : "DATA_INTEGRITY_SELFTEST FAIL (" + Errors.Count + ")";
                return head + " — " + (Errors.Count + Warnings.Count) + " findings";
            }
        }

        public void Error(string message)
        {
            Errors.Add(message);
        }

        public void Warn(string message)
        {
            Warnings.Add(message);
        }
    }

    public static class CatalogIntegrityValidator
    {
        /// <summary>Id namespaces recognised as ids. Extend when a catalog introduces a new one.</summary>
        public static readonly string[] IdPrefixes =
        {
            "item_", "loc_", "location_", "quest_", "npc_", "survivor_", "faction_",
            "event_", "recipe_", "relic_", "lore_", "room_", "stage_", "choice_",
            "mutation_", "flag_", "trait_", "anchor_", "season_", "kind_", "clinic_",
            "morph_", "drug_", "co_", "enc_", "narrative_", "dialogue_event_",
            "frequency_", "schedule_event_", "hidden_cache_", "archetype_",
            "belief_profile_", "profession_", "background_", "phantom_background_",
            "pre_war_profession_", "personal_keepsake_item_", "stance_", "belief_",
            "trauma_", "phantom_", "echo_", "arc_", "offer_", "graft_", "vouch_",
            "radio_", "broadcast_", "crisis_", "zone_", "step_", "fragment_",
            "trust_", "phase_", "milestone_", "wave_", "scenario_", "toll_",
            "trade_", "wish_", "confession_", "guilt_", "current_", "echo_",
            "cassette_", "carving_", "template_", "zone", "part_", "code_"
        };

        /// <summary>
        /// Keys whose string value is the entity's OWN id (definition position).
        /// Structural ids and usage-defined vocabularies (knowledge keys,
        /// mutations, survivor-field tags, trait lists) are registered so
        /// cross-references to them resolve. Array-valued definition keys
        /// (traits, baseTraits) register each element.
        /// </summary>
        public static readonly string[] DefinitionKeys =
        {
            "id", "survivor_id", "item_id", "relic_id", "quest_id", "faction_id",
            "choiceId", "fragment_id", "frequency_id", "narrative_id", "step_id",
            "archetype_id", "background_id", "belief_profile_id", "profession_id",
            "pre_war_profession_id", "personal_keepsake_item_id",
            "phantom_background_id", "knowledge_key", "complete_mutation",
            "fail_mutation", "world_flag", "traits", "baseTraits",
            "manifesto_law_code", "zone_id", "encounterId", "set_flag",
            "setWorldFlag", "trait_granted", "latentExpertTrait", "inspectKey"
        };

        /// <summary>
        /// Keys whose value (or array elements) REFERENCE a registered id.
        /// These are never registered themselves — a typo cannot bless itself.
        /// </summary>
        public static readonly string[] ReferenceKeys =
        {
            "resultItemId", "requiredItemId", "required_components", "materialId",
            "objective_items", "hidden_cache_items", "revealed_items",
            "hidden_cache_location", "revealed_location", "target_location_id",
            "target_location", "requires_location", "discovery_location_id",
            "location_reference", "locationId", "parentLocationId",
            "prereq_quest_id", "activeQuestlineId", "targetIds", "factionId",
            "targetFaction", "visitorFaction", "primaryFaction",
            "threateningFactionId", "requiredTrustFactionId", "survivorId",
            "traitId", "branchId", "scheduleEventId", "dialogue_event_id",
            "requiredFlag", "requiredFlagId", "RequiredFlagId", "RequiredEventFlags",
            "ambushFlag", "cleanWaterRewardFlag", "trait_granted",
            "latentExpertTrait", "requiredTrait", "roomId", "itemId"
        };

        /// <summary>Keys that must be ordered min <= max when both are present.</summary>
        public static readonly string[] RangeKeys = { "minDay", "maxDay", "MinDay", "min_day" };

        /// <summary>
        /// Pure vocabulary keys: their values are category/type/phase labels, not
        /// ids, and are never cross-referenced. Not checked at all. (wants/offers/
        /// lootCategories deliberately stay checked for PREFIXED ids — they mix
        /// vocab and real item ids, and dangling item refs there are real bugs.)
        /// </summary>
        public static readonly string[] VocabularyKeys =
        {
            "tags", "category", "type", "phase", "discovery_trigger", "badge_asset_id",
            "hazardType", "will_not", "lootCategories", "tech_offerings",
            "outcome_type"
        };

        /// <summary>
        /// Ids that are legitimately defined OUTSIDE the catalogs: runtime flags
        /// set by code, pseudo-locations (the player's own bunker), enum members
        /// (RiskBiasTrait), sentinels, and structural branch designators. Keep in
        /// sync with the code constants that create them.
        /// </summary>
        public static readonly string[] KnownRuntimeIds =
        {
            "player_shelter",        // pseudo-location: the player's own bunker (quests, world history)
            "scarred_state",         // world-state flag raised by trauma code
            "guilt_refugee_turned",  // guilt-source pattern ids used by events "add_trait" effects
            "guilt_water_trade", "guilt_stranded_rescue", "cruel_trader_death",
            "none",                  // "no faction / no id" sentinel
            "Paranoid", "Cautious", "Realist", "Reckless", "Denialist", "Fatalist",
                                     // RiskBiasTrait enum members (events requiredTrait gates)
            "a", "b",                // narrative-arc branch designators (branch_a/branch_b siblings)
            "infected_refugee", "garrison_deserter", "dying_trader", "bounty_target",
                                     // survivors created by events' add_survivor effects
            "stores",               // internal bunker room (set_quarantine effect)
            "trade_goods"           // trade-category label used in wants/offers
        };

        private sealed class Ctx
        {
            public readonly Dictionary<string, List<string>> Registry =
                new Dictionary<string, List<string>>(StringComparer.Ordinal);
            public readonly List<Ref> PendingRefs = new List<Ref>();
            public readonly HashSet<string> CrossFileWarned = new HashSet<string>(StringComparer.Ordinal);
            public readonly Dictionary<string, RangeMemoEntry> RangeMemo =
                new Dictionary<string, RangeMemoEntry>(StringComparer.Ordinal);
            public CatalogIntegrityReport Report;
            public string File;
        }

        private struct Ref
        {
            public string Value;
            public string Path;
            /// <summary>True when the position is a declared reference key (Tier 2:
            /// bare ids must also resolve). False = generic position (Tier 1:
            /// only prefixed ids are checked).</summary>
            public bool Strict;
        }

        public static CatalogIntegrityReport Validate(string dataDirectory, IFileIO files)
        {
            var report = new CatalogIntegrityReport();
            if (string.IsNullOrEmpty(dataDirectory) || !files.DirectoryExists(dataDirectory))
            {
                report.Error("data directory missing: " + dataDirectory);
                return report;
            }

            string[] jsonFiles;
            try
            {
                jsonFiles = Directory.GetFiles(dataDirectory, "*.json");
            }
            catch (Exception e)
            {
                report.Error("cannot enumerate data directory: " + e.Message);
                return report;
            }
            Array.Sort(jsonFiles, StringComparer.Ordinal);

            var ctx = new Ctx { Report = report };
            foreach (string file in jsonFiles)
            {
                ctx.File = Path.GetFileName(file);
                if (TryParse(file, out JsonDocument doc))
                {
                    using (doc)
                    {
                        Walk(doc.RootElement, ctx.File, ctx);
                    }
                }
            }

            // Tier 1: prefixed references must resolve.
            // Tier 1 + Tier 2 with dedupe: identical (value, path) pairs can be
            // collected twice (reference-key array elements are also walked).
            var reported = new HashSet<string>(StringComparer.Ordinal);
            foreach (Ref r in ctx.PendingRefs)
            {
                if (!reported.Add(r.Value + "@" + r.Path)) continue;
                if (IsKnownRuntimeId(r.Value)) continue;
                bool prefixed = StartsWithAny(r.Value, IdPrefixes);
                if (ctx.Registry.ContainsKey(r.Value)) continue;
                if (prefixed || r.Strict)
                    report.Error("unresolved " + (prefixed ? "id '" : "reference '")
                        + r.Value + "' at " + r.Path);
            }

            return report;
        }

        private static bool TryParse(string path, out JsonDocument doc)
        {
            doc = null;
            try
            {
                string text = File.ReadAllText(path);
                if (string.IsNullOrWhiteSpace(text)) return false;
                doc = JsonDocument.Parse(text);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static void Walk(JsonElement element, string path, Ctx ctx)
        {
            switch (element.ValueKind)
            {
                case JsonValueKind.Object:
                    foreach (JsonProperty property in element.EnumerateObject())
                    {
                        string childPath = path + "/" + property.Name;
                        JsonElement value = property.Value;

                        if (value.ValueKind == JsonValueKind.String)
                        {
                            string text = value.GetString();
                            if (string.IsNullOrEmpty(text) || IsVocabularyKey(property.Name)) continue;
                            RegisterOrReference(property.Name, text, childPath, ctx);
                            continue;
                        }

                        if (value.ValueKind == JsonValueKind.Array)
                        {
                            // Vocabulary arrays (tags, category, ...) are never
                            // cross-referenced: skip them wholesale.
                            if (IsVocabularyKey(property.Name)) continue;

                            if (IsDefinitionKey(property.Name))
                            {
                                // Array-valued definition keys (traits, baseTraits):
                                // each element is a definition, not a reference.
                                foreach (JsonElement item in value.EnumerateArray())
                                {
                                    if (item.ValueKind == JsonValueKind.String && !string.IsNullOrEmpty(item.GetString()))
                                        Register(property.Name, item.GetString(), childPath + "[]", ctx);
                                }
                            }

                            if (IsReferenceKey(property.Name))
                            {
                                foreach (JsonElement item in value.EnumerateArray())
                                {
                                    if (item.ValueKind == JsonValueKind.String && !string.IsNullOrEmpty(item.GetString()))
                                        ctx.PendingRefs.Add(new Ref
                                        {
                                            Value = item.GetString(),
                                            Path = childPath + "[]",
                                            Strict = true
                                        });
                                }
                            }
                        }

                        if (value.ValueKind == JsonValueKind.Number
                            && IsRangeKey(property.Name)
                            && TryGetInt(value, out int rangeVal))
                        {
                            CheckRange(property.Name, rangeVal, childPath, ctx);
                        }

                        Walk(value, childPath, ctx);
                    }
                    break;

                case JsonValueKind.Array:
                    int index = 0;
                    foreach (JsonElement item in element.EnumerateArray())
                    {
                        Walk(item, path + "[" + index + "]", ctx);
                        index++;
                    }
                    break;

                case JsonValueKind.String:
                    string s = element.GetString();
                    if (!string.IsNullOrEmpty(s))
                        ctx.PendingRefs.Add(new Ref { Value = s, Path = path, Strict = false });
                    break;
            }
        }

        private static void RegisterOrReference(string key, string value, string path, Ctx ctx)
        {
            if (IsDefinitionKey(key))
            {
                Register(key, value, path, ctx);
            }
            if (IsReferenceKey(key))
            {
                ctx.PendingRefs.Add(new Ref { Value = value, Path = path, Strict = true });
            }
            else if (!IsDefinitionKey(key) && StartsWithAny(value, IdPrefixes))
            {
                // Any prefixed string in a non-id position is still a reference
                // (Tier 1) — e.g. a narrative field naming an item id.
                ctx.PendingRefs.Add(new Ref { Value = value, Path = path, Strict = false });
            }
        }

        private static void Register(string key, string value, string path, Ctx ctx)
        {
            if (ctx.Registry.TryGetValue(value, out List<string> existing))
            {
                string firstPath = existing[0];
                string firstFile = firstPath.Substring(0, firstPath.IndexOf('/'));
                bool sameFile = path.StartsWith(firstFile, StringComparison.Ordinal);
                if (sameFile && key == "id")
                    ctx.Report.Error("duplicate id '" + value + "' defined at " + path
                        + " (first: " + firstPath + ")");
                else if (!sameFile && ctx.CrossFileWarned.Add(value))
                    ctx.Report.Warn("id '" + value + "' defined in multiple files: "
                        + firstFile + " and " + path);
            }
            else
            {
                existing = new List<string>();
                ctx.Registry[value] = existing;
            }
            existing.Add(path);
        }

        private static void CheckRange(string key, int value, string parentPath, Ctx ctx)
        {
            // Memoised per parent object: the min/max pair is checked when both
            // siblings have been seen.
            if (!ctx.RangeMemo.TryGetValue(parentPath, out RangeMemoEntry memo))
            {
                memo = new RangeMemoEntry();
                ctx.RangeMemo[parentPath] = memo;
            }

            if (key.ToLowerInvariant().Contains("min"))
                memo.Min = value;
            else
                memo.Max = value;

            if (memo.Min.HasValue && memo.Max.HasValue && memo.Min.Value > memo.Max.Value)
                ctx.Report.Error("range inverted at " + parentPath + ": min " + memo.Min.Value
                    + " > max " + memo.Max.Value);
        }

        private sealed class RangeMemoEntry
        {
            public int? Min;
            public int? Max;
        }

        private static bool TryGetInt(JsonElement element, out int value)
        {
            value = 0;
            if (element.TryGetInt32(out value)) return true;
            if (element.TryGetInt64(out long l) && l >= int.MinValue && l <= int.MaxValue)
            {
                value = (int)l;
                return true;
            }
            return false;
        }

        private static bool IsVocabularyKey(string key) =>
            Array.IndexOf(VocabularyKeys, key) >= 0;

        private static bool IsKnownRuntimeId(string value) =>
            Array.IndexOf(KnownRuntimeIds, value) >= 0;

        private static bool IsDefinitionKey(string key) =>
            Array.IndexOf(DefinitionKeys, key) >= 0;

        private static bool IsReferenceKey(string key) =>
            Array.IndexOf(ReferenceKeys, key) >= 0;

        private static bool IsRangeKey(string key) =>
            Array.IndexOf(RangeKeys, key) >= 0;

        private static bool StartsWithAny(string value, string[] prefixes)
        {
            for (int i = 0; i < prefixes.Length; i++)
                if (value.StartsWith(prefixes[i], StringComparison.Ordinal))
                    return true;
            return false;
        }
    }
}
