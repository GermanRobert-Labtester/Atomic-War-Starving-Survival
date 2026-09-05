using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Inventory;
using Ashfall.Core.World;

namespace Ashfall.Core.Content
{
    public sealed class CollectibleIntegrityFinding
    {
        public string SourceCatalog { get; }
        public string SourceId { get; }
        public string FieldPath { get; }
        public string TargetId { get; }
        public string TargetCatalog { get; }
        public string ErrorCode { get; }
        public string Message { get; }

        public CollectibleIntegrityFinding(
            string sourceCatalog,
            string sourceId,
            string fieldPath,
            string targetId,
            string targetCatalog,
            string errorCode,
            string message)
        {
            SourceCatalog = sourceCatalog ?? string.Empty;
            SourceId = sourceId ?? string.Empty;
            FieldPath = fieldPath ?? string.Empty;
            TargetId = targetId ?? string.Empty;
            TargetCatalog = targetCatalog ?? string.Empty;
            ErrorCode = errorCode ?? string.Empty;
            Message = message ?? string.Empty;
        }

        public override string ToString() =>
            $"[{ErrorCode}] {SourceCatalog}:{SourceId} ({FieldPath}) -> {TargetCatalog}:{TargetId} : {Message}";
    }

    /// <summary>
    /// Pure, deterministic cross-catalog integrity and reachability validator
    /// for ASHFALL's collectible subsystem (Task 8). Consumes zero gameplay RNG.
    /// </summary>
    public static class CollectibleCatalogIntegrityValidator
    {
        public static readonly HashSet<string> ValidCategories = new HashSet<string>(StringComparer.Ordinal)
        {
            "vinyl", "photograph", "poster", "book", "magazine", "technical_manual",
            "military_document", "personal_letter", "badge", "patch", "toy",
            "religious_object", "sports_memorabilia", "cultural_artifact", "newspaper", "map"
        };

        public static readonly HashSet<string> ValidRarities = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "common", "uncommon", "rare", "unique"
        };

        public static readonly HashSet<string> ValidEffectTypes = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "none", "morale", "knowledge", "journal_unlock", "faction_info", "location_clue", "recipe"
        };

        public static List<CollectibleIntegrityFinding> Validate(
            string dataDir,
            IFileIO fileIO,
            IJsonSerializer json,
            ILog? log = null)
        {
            var findings = new List<CollectibleIntegrityFinding>();
            if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir))
            {
                findings.Add(new CollectibleIntegrityFinding(
                    "context", "root", "", "", "", "ERR_NULL_CONTEXT", "Data directory, fileIO, or serializer was null."));
                return findings;
            }

            // 1. Load collectibles.json
            string colPath = fileIO.Combine(dataDir, "collectibles.json");
            if (!fileIO.FileExists(colPath))
            {
                findings.Add(new CollectibleIntegrityFinding(
                    "collectibles.json", "file", "", "", "", "ERR_FILE_MISSING", "collectibles.json does not exist."));
                return findings;
            }

            CollectibleCatalogFileRaw? colFile = null;
            try
            {
                colFile = json.Deserialize<CollectibleCatalogFileRaw>(fileIO.ReadAllText(colPath));
            }
            catch (Exception ex)
            {
                log?.Warn($"CollectibleCatalogIntegrityValidator: failed to parse collectibles.json: {ex.Message}");
                findings.Add(new CollectibleIntegrityFinding(
                    "collectibles.json", "file", "", "", "", "ERR_JSON_PARSE", $"Failed to parse collectibles.json: {ex.Message}"));
                return findings;
            }

            if (colFile?.collectibles == null)
            {
                findings.Add(new CollectibleIntegrityFinding(
                    "collectibles.json", "file", "collectibles", "", "", "ERR_EMPTY_COLLECTIBLES", "collectibles list is null or empty."));
                return findings;
            }

            // 2. Load items.json
            string itemsPath = fileIO.Combine(dataDir, "items.json");
            ItemCatalog? itemCatalog = null;
            var itemRawMap = new Dictionary<string, bool>(StringComparer.Ordinal);
            if (fileIO.FileExists(itemsPath))
            {
                try
                {
                    itemCatalog = ItemCatalogLoader.LoadCatalog(dataDir, fileIO, json);
                    var itemsDto = json.Deserialize<ItemFileRootDto>(fileIO.ReadAllText(itemsPath));
                    if (itemsDto?.items != null)
                    {
                        foreach (var it in itemsDto.items)
                        {
                            if (!string.IsNullOrEmpty(it.id))
                                itemRawMap[it.id] = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    log?.Warn($"CollectibleCatalogIntegrityValidator: failed to parse items.json: {ex.Message}");
                    findings.Add(new CollectibleIntegrityFinding(
                        "items.json", "file", "", "", "", "ERR_JSON_PARSE", $"Failed to parse items.json: {ex.Message}"));
                }
            }
            else
            {
                findings.Add(new CollectibleIntegrityFinding(
                    "items.json", "file", "", "", "", "ERR_FILE_MISSING", "items.json does not exist."));
            }

            // 3. Load research_knowledge.json
            var knowledgeIds = new HashSet<string>(StringComparer.Ordinal);
            string rkPath = fileIO.Combine(dataDir, "research_knowledge.json");
            if (fileIO.FileExists(rkPath))
            {
                try
                {
                    var rk = json.Deserialize<ResearchKnowledgeCatalogContainer>(fileIO.ReadAllText(rkPath));
                    if (rk?.KnowledgeNodes != null)
                    {
                        foreach (var k in rk.KnowledgeNodes)
                            if (!string.IsNullOrEmpty(k.Id)) knowledgeIds.Add(k.Id);
                    }
                }
                catch (Exception ex)
                {
                    log?.Warn($"CollectibleCatalogIntegrityValidator: failed to parse research_knowledge.json: {ex.Message}");
                    findings.Add(new CollectibleIntegrityFinding(
                        "research_knowledge.json", "file", "", "", "", "ERR_JSON_PARSE", ex.Message));
                }
            }

            // 4. Load wasteland_map_v1.json
            var mapNodeIds = new HashSet<string>(StringComparer.Ordinal);
            string mapPath = fileIO.Combine(dataDir, "wasteland_map_v1.json");
            if (fileIO.FileExists(mapPath))
            {
                try
                {
                    var mapFile = WastelandMapCatalogLoader.Load(dataDir, fileIO, json);
                    if (mapFile.nodes != null)
                    {
                        foreach (var n in mapFile.nodes)
                            if (!string.IsNullOrEmpty(n.Id)) mapNodeIds.Add(n.Id);
                    }
                }
                catch (Exception ex)
                {
                    log?.Warn($"CollectibleCatalogIntegrityValidator: failed to parse wasteland_map_v1.json: {ex.Message}");
                    findings.Add(new CollectibleIntegrityFinding(
                        "wasteland_map_v1.json", "file", "", "", "", "ERR_JSON_PARSE", ex.Message));
                }
            }

            // 5. Load journal_voice_prose.json
            var proseKeys = new HashSet<string>(StringComparer.Ordinal);
            string prosePath = fileIO.Combine(dataDir, "journal_voice_prose.json");
            if (fileIO.FileExists(prosePath))
            {
                try
                {
                    var proseFile = json.Deserialize<JournalVoiceProseFileRaw>(fileIO.ReadAllText(prosePath));
                    if (proseFile?.prose_variants != null)
                    {
                        foreach (var kv in proseFile.prose_variants)
                            proseKeys.Add(kv.Key);
                    }
                }
                catch (Exception ex)
                {
                    log?.Warn($"CollectibleCatalogIntegrityValidator: failed to parse journal_voice_prose.json: {ex.Message}");
                    findings.Add(new CollectibleIntegrityFinding(
                        "journal_voice_prose.json", "file", "", "", "", "ERR_JSON_PARSE", ex.Message));
                }
            }

            // 6. Build acquisition source graph
            var sources = new Dictionary<string, List<string>>(StringComparer.Ordinal);
            foreach (var c in colFile.collectibles)
            {
                if (!string.IsNullOrEmpty(c.item_id))
                    sources[c.item_id] = new List<string>();
            }

            // 6a. Scavenging tables
            string scavPath = fileIO.Combine(dataDir, "scavenging_tables.json");
            if (fileIO.FileExists(scavPath))
            {
                try
                {
                    var scavFile = json.Deserialize<ScavengingTableCatalogContainer>(fileIO.ReadAllText(scavPath));
                    if (scavFile?.tables != null)
                    {
                        foreach (var t in scavFile.tables)
                        {
                            if (t.entries == null) continue;
                            foreach (var e in t.entries)
                            {
                                if (string.IsNullOrEmpty(e.item_id)) continue;
                                if (itemCatalog != null && !itemCatalog.Contains(e.item_id) && !itemRawMap.ContainsKey(e.item_id))
                                {
                                    findings.Add(new CollectibleIntegrityFinding(
                                        "scavenging_tables.json", t.id, "entries.item_id", e.item_id, "items.json",
                                        "ERR_SCAV_ITEM_MISSING", $"Scavenging table '{t.id}' references unknown item '{e.item_id}'."));
                                }
                                if (sources.TryGetValue(e.item_id, out var srcList))
                                {
                                    srcList.Add($"scavenging_table:{t.id}");
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    log?.Warn($"CollectibleCatalogIntegrityValidator: failed to parse scavenging_tables.json: {ex.Message}");
                    findings.Add(new CollectibleIntegrityFinding(
                        "scavenging_tables.json", "file", "", "", "", "ERR_JSON_PARSE", ex.Message));
                }
            }

            // 6b. Check questlines, damaged map zones, radio signals
            void CheckFileForItems(string relFile, string labelPrefix)
            {
                string p = fileIO.Combine(dataDir, relFile);
                if (!fileIO.FileExists(p)) return;
                try
                {
                    string text = fileIO.ReadAllText(p);
                    foreach (var kv in sources)
                    {
                        if (text.Contains(kv.Key))
                            kv.Value.Add(labelPrefix);
                    }
                }
                catch { /* best-effort: probe-only text scan, tolerate unreadable files */ }
            }
            CheckFileForItems("narrative_questlines.json", "quest:narrative_questlines");
            CheckFileForItems("damaged_map_zones.json", "map_zone:damaged_map_zones");
            CheckFileForItems("radio_distress_signals.json", "radio:radio_distress_signals");

            // 7. Validate all collectible definitions
            var seenIds = new HashSet<string>(StringComparer.Ordinal);
            foreach (var c in colFile.collectibles)
            {
                if (c == null) continue;
                string cid = c.item_id ?? string.Empty;

                if (string.IsNullOrEmpty(cid))
                {
                    findings.Add(new CollectibleIntegrityFinding(
                        "collectibles.json", "unknown", "item_id", "", "", "ERR_ITEM_ID_EMPTY", "Collectible item_id is null or empty."));
                    continue;
                }

                if (!seenIds.Add(cid))
                {
                    findings.Add(new CollectibleIntegrityFinding(
                        "collectibles.json", cid, "item_id", cid, "collectibles.json", "ERR_DUPLICATE_ID", $"Duplicate collectible definition for '{cid}'."));
                }

                // 7a. Item authority resolution (Section 8.3)
                if (itemCatalog != null && !itemCatalog.Contains(cid) && !itemRawMap.ContainsKey(cid))
                {
                    findings.Add(new CollectibleIntegrityFinding(
                        "collectibles.json", cid, "item_id", cid, "items.json", "ERR_ITEM_NOT_FOUND", $"Collectible '{cid}' has no corresponding entry in items.json."));
                }

                // 7b. Category validation (Section 8.12)
                if (string.IsNullOrEmpty(c.category) || !ValidCategories.Contains(c.category))
                {
                    findings.Add(new CollectibleIntegrityFinding(
                        "collectibles.json", cid, "category", c.category ?? "", "", "ERR_INVALID_CATEGORY", $"Collectible '{cid}' has invalid category '{c.category}'."));
                }

                // 7c. Rarity validation (Section 8.12)
                if (string.IsNullOrEmpty(c.rarity) || !ValidRarities.Contains(c.rarity))
                {
                    findings.Add(new CollectibleIntegrityFinding(
                        "collectibles.json", cid, "rarity", c.rarity ?? "", "", "ERR_INVALID_RARITY", $"Collectible '{cid}' has invalid rarity '{c.rarity}'."));
                }

                // 7d. Effect type validation
                if (string.IsNullOrEmpty(c.effect_type) || !ValidEffectTypes.Contains(c.effect_type))
                {
                    findings.Add(new CollectibleIntegrityFinding(
                        "collectibles.json", cid, "effect_type", c.effect_type ?? "", "", "ERR_INVALID_EFFECT_TYPE", $"Collectible '{cid}' has invalid effect_type '{c.effect_type}'."));
                }

                // 7e. Effect target validation (Sections 8.7, 8.8, 8.9, 8.10)
                if (c.effect_type == "knowledge")
                {
                    if (string.IsNullOrEmpty(c.effect_target))
                    {
                        findings.Add(new CollectibleIntegrityFinding(
                            "collectibles.json", cid, "effect_target", "", "research_knowledge.json", "ERR_TARGET_REQUIRED", "Knowledge collectible missing effect_target."));
                    }
                    else if (!knowledgeIds.Contains(c.effect_target))
                    {
                        findings.Add(new CollectibleIntegrityFinding(
                            "collectibles.json", cid, "effect_target", c.effect_target, "research_knowledge.json", "ERR_KNOWLEDGE_TARGET_MISSING", $"Knowledge target '{c.effect_target}' not found in research_knowledge.json."));
                    }
                }
                else if (c.effect_type == "location_clue")
                {
                    if (string.IsNullOrEmpty(c.effect_target))
                    {
                        findings.Add(new CollectibleIntegrityFinding(
                            "collectibles.json", cid, "effect_target", "", "wasteland_map_v1.json", "ERR_TARGET_REQUIRED", "Location clue collectible missing effect_target."));
                    }
                    else if (!mapNodeIds.Contains(c.effect_target))
                    {
                        findings.Add(new CollectibleIntegrityFinding(
                            "collectibles.json", cid, "effect_target", c.effect_target, "wasteland_map_v1.json", "ERR_LOCATION_TARGET_MISSING", $"Location clue target '{c.effect_target}' not found in wasteland_map_v1.json."));
                    }
                }
                else if (c.effect_type == "journal_unlock")
                {
                    if (string.IsNullOrEmpty(c.effect_target))
                    {
                        findings.Add(new CollectibleIntegrityFinding(
                            "collectibles.json", cid, "effect_target", "", "journal_voice_prose.json", "ERR_TARGET_REQUIRED", "Journal unlock collectible missing effect_target."));
                    }
                    else if (!proseKeys.Contains(c.effect_target))
                    {
                        findings.Add(new CollectibleIntegrityFinding(
                            "collectibles.json", cid, "effect_target", c.effect_target, "journal_voice_prose.json", "ERR_JOURNAL_TARGET_MISSING", $"Journal unlock target '{c.effect_target}' not found in journal_voice_prose.json."));
                    }
                }
                else if (c.effect_type == "faction_info")
                {
                    if (string.IsNullOrEmpty(c.effect_target))
                    {
                        findings.Add(new CollectibleIntegrityFinding(
                            "collectibles.json", cid, "effect_target", "", "journal_voice_prose.json", "ERR_TARGET_REQUIRED", "Faction info collectible missing effect_target."));
                    }
                    else if (!proseKeys.Contains(c.effect_target))
                    {
                        findings.Add(new CollectibleIntegrityFinding(
                            "collectibles.json", cid, "effect_target", c.effect_target, "journal_voice_prose.json", "ERR_FACTION_TARGET_MISSING", $"Faction info target '{c.effect_target}' not found in journal_voice_prose.json."));
                    }
                }

                // 7f. Structural reachability: sourceCount >= 1 (Section 8.5)
                if (sources.TryGetValue(cid, out var edgeList) && edgeList.Count == 0)
                {
                    findings.Add(new CollectibleIntegrityFinding(
                        "collectibles.json", cid, "acquisition_source", "", "scavenging_tables/quests/map", "ERR_NO_ACQUISITION_SOURCE", $"Collectible '{cid}' has 0 acquisition sources across all catalogs."));
                }
            }

            // 8. Reverse bijection: Every collectible-prefixed item in items.json must map to collectibles.json
            foreach (var kv in itemRawMap)
            {
                if (kv.Key.StartsWith("item_collectible_", StringComparison.Ordinal))
                {
                    if (!seenIds.Contains(kv.Key))
                    {
                        findings.Add(new CollectibleIntegrityFinding(
                            "items.json", kv.Key, "id", kv.Key, "collectibles.json", "ERR_ITEM_ORPHAN", $"Collectible item '{kv.Key}' has no definition in collectibles.json."));
                    }
                }
            }

            // Sort deterministically (Section 8.17): source file, source ID, field path, error code
            findings.Sort((a, b) =>
            {
                int c = string.Compare(a.SourceCatalog, b.SourceCatalog, StringComparison.Ordinal);
                if (c != 0) return c;
                c = string.Compare(a.SourceId, b.SourceId, StringComparison.Ordinal);
                if (c != 0) return c;
                c = string.Compare(a.FieldPath, b.FieldPath, StringComparison.Ordinal);
                if (c != 0) return c;
                return string.Compare(a.ErrorCode, b.ErrorCode, StringComparison.Ordinal);
            });

            return findings;
        }
    }

    [Serializable]
    public sealed class ItemFileRootDto
    {
        public int schema_version { get; set; } = 1;
        public List<ItemHeaderDto> items { get; set; } = new List<ItemHeaderDto>();
    }

    [Serializable]
    public sealed class ItemHeaderDto
    {
        public string id { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class JournalVoiceProseFileRaw
    {
        public int schema_version { get; set; } = 1;
        public Dictionary<string, Dictionary<string, string>> prose_variants { get; set; } =
            new Dictionary<string, Dictionary<string, string>>(StringComparer.Ordinal);
    }
}
