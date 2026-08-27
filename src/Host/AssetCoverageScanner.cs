// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Godot;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Audits catalog data files against texture assets, evaluating resolution,
    /// fallbacks, normalization, and deduplication invariants.
    /// </summary>
    public static class AssetCoverageScanner
    {
        // ── Full-catalog coverage catalog manifest ───────────────────────
        private static readonly (string category, string file, string idField)[] CoverageCatalogFiles =
        {
            ("item",     "items.json",                  "id"),
            ("item",     "black_flotilla_items.json",   "id"),
            ("item",     "chemical_dependency_items.json", "id"),
            ("item",     "crossing_items.json",         "id"),
            ("item",     "dose_items.json",             "id"),
            ("item",     "foundry_items.json",          "id"),
            ("item",     "greenhouse_items.json",       "id"),
            ("item",     "holdfast_items.json",         "id"),
            ("item",     "verdict_items.json",          "id"),
            ("item",     "year_of_ash_items.json",      "id"),
            ("portrait", "survivors.json",              "id"),
            ("portrait", "year_of_ash_survivors.json",  "id"),
            ("portrait", "characters.json",             "id"),
            ("portrait", "verdict_npcs.json",           "id"),
            ("location", "locations.json",              "id"),
            ("location", "crossing_locations.json",     "id"),
            ("location", "deep_lore_locations.json",    "id"),
            ("location", "dose_locations.json",         "id"),
            ("location", "duty_roster_locations.json",  "id"),
            ("location", "holdfast_locations.json",     "id"),
            ("location", "locations_expansion3.json",   "id"),
            ("location", "verdict_locations.json",      "id"),
            ("location", "year_of_ash_locations.json",  "id"),
            ("faction",  "currents.json",               "id"),
            ("faction",  "crossing_factions.json",      "id"),
            ("faction",  "holdfast_factions.json",      "id"),
            ("faction",  "standing_record_factions.json", "id"),
            ("faction",  "foundry_faction.json",        "faction_id"),
            ("faction",  "faction_lore.json",           "faction_id"),
        };

        private static bool IsCanonicalId(string id)
        {
            foreach (var c in id)
            {
                if (!(c >= 'a' && c <= 'z') && !(c >= '0' && c <= '9') && c != '_')
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Runs top-N asset probes and invariant checks against catalog data.
        /// </summary>
        public static AssetCoverageSummaryReport RunTopProbes(string dataDir, int topCount = 50)
        {
            GD.Print("[AssetRegistrySelfTest] Starting...");
            GD.Print($"[AssetRegistrySelfTest] Data dir: {dataDir}");
            GD.Print($"[AssetRegistrySelfTest] Checking top {topCount} referenced assets");

            AssetRegistry.ClearMissingLog();

            var rows = new List<AssetCoverageResultRow>();
            var referenceCounts = new Dictionary<string, int>();

            int itemsChecked = 0;
            int portraitsChecked = 0;
            int locationsChecked = 0;

            // Check items
            var itemsPath = Path.Combine(dataDir, "items.json");
            if (File.Exists(itemsPath))
            {
                GD.Print($"[AssetRegistrySelfTest] Found items.json, scanning item assets...");
                var itemIds = ExtractIdsFromJson(itemsPath, "id");
                GD.Print($"[AssetRegistrySelfTest] Found {itemIds.Count} item IDs");

                int itemLimit = Math.Min(topCount / 3, itemIds.Count);
                for (int i = 0; i < itemLimit; i++)
                {
                    string id = itemIds[i];
                    var result = AssetRegistry.GetItem(id);
                    rows.Add(new AssetCoverageResultRow
                    {
                        Id = id,
                        Category = "item",
                        ResolvedPath = result.ResolvedPath,
                        Exists = result.Result != AssetLoadResult.Missing,
                        Loaded = result.Result == AssetLoadResult.Loaded,
                        ReferenceCount = 1
                    });
                    itemsChecked++;
                }
            }
            else
            {
                GD.Print($"[AssetRegistrySelfTest] Warning: items.json not found at {itemsPath}");
            }

            // Check survivors
            var survivorsPath = Path.Combine(dataDir, "survivors.json");
            if (File.Exists(survivorsPath))
            {
                GD.Print($"[AssetRegistrySelfTest] Found survivors.json, scanning portrait assets...");
                var survivorIds = ExtractIdsFromJson(survivorsPath, "id");
                GD.Print($"[AssetRegistrySelfTest] Found {survivorIds.Count} survivor IDs");

                int portraitLimit = Math.Min(topCount / 3, survivorIds.Count);
                for (int i = 0; i < portraitLimit; i++)
                {
                    string id = survivorIds[i];
                    var result = AssetRegistry.GetPortrait(id);
                    rows.Add(new AssetCoverageResultRow
                    {
                        Id = id,
                        Category = "portrait",
                        ResolvedPath = result.ResolvedPath,
                        Exists = result.Result != AssetLoadResult.Missing,
                        Loaded = result.Result == AssetLoadResult.Loaded,
                        ReferenceCount = 1
                    });
                    portraitsChecked++;
                }
            }
            else
            {
                GD.Print($"[AssetRegistrySelfTest] Warning: survivors.json not found at {survivorsPath}");
            }

            // Check locations
            var locationsPath = Path.Combine(dataDir, "locations.json");
            if (File.Exists(locationsPath))
            {
                GD.Print($"[AssetRegistrySelfTest] Found locations.json, scanning location assets...");
                var locationIds = ExtractIdsFromJson(locationsPath, "id");
                GD.Print($"[AssetRegistrySelfTest] Found {locationIds.Count} location IDs");

                int locationLimit = Math.Min(topCount / 3, locationIds.Count);
                for (int i = 0; i < locationLimit; i++)
                {
                    string id = locationIds[i];
                    var result = AssetRegistry.GetLocation(id);
                    rows.Add(new AssetCoverageResultRow
                    {
                        Id = id,
                        Category = "location",
                        ResolvedPath = result.ResolvedPath,
                        Exists = result.Result != AssetLoadResult.Missing,
                        Loaded = result.Result == AssetLoadResult.Loaded,
                        ReferenceCount = 1
                    });
                    locationsChecked++;
                }
            }
            else
            {
                GD.Print($"[AssetRegistrySelfTest] Warning: locations.json not found at {locationsPath}");
            }

            // Check critical named assets
            var criticalAssets = new[]
            {
                "iodine_pills",
                "geiger_counter",
                "gas_mask",
                "clean_water",
                "canned_food",
                "elena_vasquez",
                "marcus_olejnik",
                "suki_tanaka",
                "abandoned_hospital",
                "rural_gas_station"
            };

            GD.Print($"[AssetRegistrySelfTest] Checking {criticalAssets.Length} critical assets...");
            foreach (var asset in criticalAssets)
            {
                bool alreadyChecked = false;
                foreach (var row in rows)
                {
                    if (row.Id == asset)
                    {
                        alreadyChecked = true;
                        break;
                    }
                }
                if (alreadyChecked)
                    continue;

                var result = AssetRegistry.GetItem(asset);
                if (result.Result == AssetLoadResult.Missing)
                {
                    result = AssetRegistry.GetPortrait(asset);
                }
                if (result.Result == AssetLoadResult.Missing)
                {
                    result = AssetRegistry.GetLocation(asset);
                }

                rows.Add(new AssetCoverageResultRow
                {
                    Id = asset,
                    Category = "critical",
                    ResolvedPath = result.ResolvedPath,
                    Exists = result.Result != AssetLoadResult.Missing,
                    Loaded = result.Result == AssetLoadResult.Loaded,
                    ReferenceCount = 999
                });
            }

            // Fallback art probes
            var fallbackArtProbes = new (string id, string path, string category)[]
            {
                ("placeholder_survivor.png", AssetRegistry.FallbackSurvivorPath, "fallback:character"),
                ("icon_placeholder.png",     AssetRegistry.FallbackIconPath,     "fallback:icon"),
            };

            GD.Print($"[AssetRegistrySelfTest] Checking {fallbackArtProbes.Length} fallback art assets...");
            foreach (var (id, path, category) in fallbackArtProbes)
            {
                var result = AssetRegistry.GetByPath(path);
                bool exists = ResourceLoader.Exists(path);
                bool loaded = result.Result == AssetLoadResult.Loaded && result.Texture != null;

                rows.Add(new AssetCoverageResultRow
                {
                    Id = id,
                    Category = category,
                    ResolvedPath = result.ResolvedPath,
                    Exists = exists,
                    Loaded = loaded,
                    ReferenceCount = 1000
                });

                if (!exists || !loaded)
                {
                    GD.PrintErr($"[AssetRegistrySelfTest] FALLBACK ART FAILED: id={id} path={path} exists={exists} loaded={loaded} result={result.Result}");
                }
            }

            int probesChecked = 0;
            int probesFailingAsIntended = 0;
            int probeFailures = 0;
            var probeRows = new List<AssetCoverageResultRow>();

            var normalizationProbes = new (string id, string category, string expectFileStem, bool expectMissing)[]
            {
                ("mechanical_components", "item",    "mechanical_components",       false),
                ("mechanical_parts",      "item",    "mechanical_parts",            false),
                ("blood_bag",             "item",    "item_blood_bag",              false),
                ("encrypted_drive",       "item",    "encrypted_drive",             false),
                ("faraday_pack",          "item",    "faraday_pack",                false),
                ("cigarette_pack_sealed", "item",    "cigarette_pack_sealed",        false),
                ("iodine_pills",          "item",    "iodine_pills",                false),
                ("geiger_counter",        "item",    "geiger_counter",              false),
            };

            int normProbes = 0;
            int normProbesPass = 0;
            foreach (var (id, cat, expectStem, expectMissing) in normalizationProbes)
            {
                AssetResult r = cat switch
                {
                    "item"     => AssetRegistry.GetItem(id),
                    "portrait" => AssetRegistry.GetPortrait(id),
                    "location" => AssetRegistry.GetLocation(id),
                    "faction"  => AssetRegistry.GetFaction(id),
                    _          => default,
                };
                var probe = new AssetCoverageResultRow
                {
                    Id = id,
                    Category = "norm:" + cat,
                    ResolvedPath = r.ResolvedPath,
                    Exists = r.Result != AssetLoadResult.Missing,
                    Loaded = r.Result == AssetLoadResult.Loaded,
                    ReferenceCount = 0,
                };
                probeRows.Add(probe);
                normProbes++;

                bool resolved = r.Result == AssetLoadResult.Loaded
                              || r.Result == AssetLoadResult.FallbackUsed;
                bool correctFile = resolved
                    && !string.IsNullOrEmpty(r.ResolvedPath)
                    && r.ResolvedPath.Contains(expectStem);
                bool matchesExpectation = expectMissing
                    ? (r.Result == AssetLoadResult.Missing)
                    : correctFile;
                probesChecked++;
                if (matchesExpectation)
                {
                    normProbesPass++;
                    probesFailingAsIntended++;
                }
                else
                {
                    probeFailures++;
                    GD.PrintErr(
                        $"[AssetRegistrySelfTest] NORM PROBE FAILED: id={id} cat={cat} "
                        + $"expected substring '{expectStem}' got '{r.ResolvedPath}' "
                        + $"result={r.Result}");
                }
            }
            GD.Print($"[AssetRegistrySelfTest] Normalization probes: {normProbesPass}/{normProbes} match expected outcome");

            var negativeProbes = new (string id, string category)[]
            {
                ("__definitely_not_a_real_asset_xyzzy__", "item"),
                ("__non_existent_portrait_xyzzy__",      "portrait"),
                ("__non_existent_location_xyzzy__",      "location"),
            };
            foreach (var (id, cat) in negativeProbes)
            {
                AssetResult r = cat switch
                {
                    "item"     => AssetRegistry.GetItem(id),
                    "portrait" => AssetRegistry.GetPortrait(id),
                    "location" => AssetRegistry.GetLocation(id),
                    _          => default,
                };
                var probe = new AssetCoverageResultRow
                {
                    Id = id,
                    Category = "neg:" + cat,
                    ResolvedPath = r.ResolvedPath,
                    Exists = r.Result != AssetLoadResult.Missing,
                    Loaded = r.Result == AssetLoadResult.Loaded,
                    ReferenceCount = 0,
                };
                probeRows.Add(probe);
                probesChecked++;
                if (r.Result != AssetLoadResult.Missing && r.Result != AssetLoadResult.FailedToLoad)
                {
                    probeFailures++;
                    GD.PrintErr(
                        $"[AssetRegistrySelfTest] NEGATIVE PROBE FAILED: id={id} cat={cat} "
                        + $"unexpectedly resolved to '{r.ResolvedPath}'");
                }
                else
                {
                    probesFailingAsIntended++;
                }
            }
            GD.Print($"[AssetRegistrySelfTest] Total probes evaluated: {probesChecked}, mismatches against expectation: {probesChecked - probesFailingAsIntended}");

            int dedupBaseline = AssetRegistry.MissingAssetCount;
            AssetRegistry.GetItem("__definitely_not_a_real_asset_xyzzy__");
            if (AssetRegistry.MissingAssetCount != dedupBaseline)
            {
                probeFailures++;
                GD.PrintErr($"[AssetRegistrySelfTest] DEDUPLICATION FAILED: missing count increased on repeated query ({AssetRegistry.MissingAssetCount} vs {dedupBaseline})");
            }
            var itemWarn = AssetRegistry.GetLoggedWarning("item", "__definitely_not_a_real_asset_xyzzy__");
            if (itemWarn == null || itemWarn.Value.Category != "item" || string.IsNullOrEmpty(itemWarn.Value.FallbackUsed))
            {
                probeFailures++;
                GD.PrintErr("[AssetRegistrySelfTest] DIAGNOSTIC RECORD FAILED: missing category or fallback description in record");
            }

            if (probesChecked - probesFailingAsIntended > 0)
            {
                GD.PrintErr($"[AssetRegistrySelfTest] PROBE MISMATCHES: {probesChecked - probesFailingAsIntended} probe(s) produced an outcome different from what the gate expected");
            }

            GD.Print("[AssetRegistrySelfTest] Checking missing-warning deduplication (category × id)...");
            var dedupCases = new (string cat, string id, Action act, string name)[]
            {
                ("item",     "__nonexistent_item_dedup_probe__",     () => AssetRegistry.GetItem("__nonexistent_item_dedup_probe__"),     "GetItem missing warning"),
                ("portrait", "__nonexistent_portrait_dedup_probe__", () => AssetRegistry.GetPortrait("__nonexistent_portrait_dedup_probe__"), "GetPortrait missing warning"),
                ("location", "__nonexistent_location_dedup_probe__", () => AssetRegistry.GetLocation("__nonexistent_location_dedup_probe__"), "GetLocation missing warning"),
                ("faction",  "__nonexistent_faction_dedup_probe__",  () => AssetRegistry.GetFaction("__nonexistent_faction_dedup_probe__"),  "GetFaction missing warning"),
            };

            foreach (var (cat, id, act, name) in dedupCases)
            {
                int preCount = AssetRegistry.LoggedWarnings.Count;
                act();
                int midCount = AssetRegistry.LoggedWarnings.Count;
                act();
                int postCount = AssetRegistry.LoggedWarnings.Count;

                bool recordedOnce = (midCount == preCount + 1) && (postCount == midCount);
                bool hasKey = AssetRegistry.HasLoggedWarning(cat, id);
                var warn = AssetRegistry.GetLoggedWarning(cat, id);
                bool recordValid = warn.HasValue
                    && warn.Value.Category == cat
                    && warn.Value.RequestedId == id
                    && !string.IsNullOrEmpty(warn.Value.FallbackUsed);

                bool casePassed = recordedOnce && hasKey && recordValid;
                probesChecked++;
                if (casePassed)
                {
                    probesFailingAsIntended++;
                }
                else
                {
                    probeFailures++;
                    GD.PrintErr($"[AssetRegistrySelfTest] DEDUP FAILED: {name}");
                }
            }

            var fallbackDescCases = new (string category, string expectedSubstr)[]
            {
                ("item",     "icon_placeholder.png"),
                ("portrait", "placeholder_survivor.png"),
                ("location", "(none)"),
                ("faction",  "icon_placeholder.png"),
                ("unknown",  "(none)"),
            };

            foreach (var (cat, substr) in fallbackDescCases)
            {
                string desc = AssetRegistry.GetEffectiveFallbackDescription(cat);
                bool matches = desc.Contains(substr);
                probesChecked++;
                if (matches)
                {
                    probesFailingAsIntended++;
                }
                else
                {
                    probeFailures++;
                    GD.PrintErr($"[AssetRegistrySelfTest] FALLBACK DESC FAILED: category={cat} expected={substr} got={desc}");
                }
            }

            GD.Print($"[AssetRegistrySelfTest] Total probes evaluated: {probesChecked}, mismatches against expectation: {probeFailures}");

            int total = rows.Count;
            int passed = 0;
            int missing = 0;
            int failed = 0;
            var uniqueMissingIds = new HashSet<string>();

            foreach (var row in rows)
            {
                if (row.Loaded)
                {
                    passed++;
                }
                else if (!row.Exists)
                {
                    missing++;
                    uniqueMissingIds.Add(row.Id);
                }
                else
                {
                    failed++;
                }
            }

            int uniqueMissing = uniqueMissingIds.Count;
            int duplicateFallbackRequests = AssetRegistry.DuplicateFallbackRequestCount;
            string summary = $"Assets: {passed}/{total} loaded, {missing} missing ({uniqueMissing} unique), {duplicateFallbackRequests} dup-fallback-requests, {failed} load failures, {probeFailures} probe failures";

            var report = new AssetCoverageSummaryReport
            {
                TotalChecked = total,
                Missing = missing,
                UniqueMissing = uniqueMissing,
                DuplicateFallbackRequests = duplicateFallbackRequests,
                FailedToLoad = failed,
                Passed = passed,
                ProbeFailures = probeFailures,
                Rows = rows,
                Summary = summary
            };

            AssetCoverageReport.PrintSummary(report);
            return report;
        }

        /// <summary>
        /// Runs full-catalog sweep across every definition ID.
        /// </summary>
        public static void RunFullCoverageSweep(string dataDir)
        {
            GD.Print("[AssetCoverageReport] Full catalog sweep — report-only, non-gating");
            GD.Print($"[AssetCoverageReport] Data dir: {dataDir}");

            var idsByCategory = new Dictionary<string, List<string>>();
            var missingByCategory = new Dictionary<string, List<string>>();

            foreach (var (category, file, idField) in CoverageCatalogFiles)
            {
                var path = Path.Combine(dataDir, file);
                if (!File.Exists(path))
                {
                    GD.PrintErr($"[AssetCoverageReport] catalog file not found: {file}");
                    continue;
                }
                if (!idsByCategory.TryGetValue(category, out var ids))
                    idsByCategory[category] = ids = new List<string>();
                foreach (var raw in ExtractIdsFromJson(path, idField))
                {
                    if (raw.Length == 0 || !IsCanonicalId(raw)) continue;
                    if (!ids.Contains(raw))
                        ids.Add(raw);
                }
            }

            int totalIds = 0, totalMissing = 0;
            foreach (var (category, ids) in idsByCategory)
            {
                var missing = new List<string>();
                foreach (var id in ids)
                {
                    var r = category switch
                    {
                        "item"     => AssetRegistry.GetItem(id),
                        "portrait" => AssetRegistry.GetPortrait(id),
                        "location" => AssetRegistry.GetLocation(id),
                        "faction"  => AssetRegistry.GetFaction(id),
                        _          => default,
                    };
                    bool covered = r.Result == AssetLoadResult.Loaded;
                    if (!covered && category == "faction")
                        covered = Ashfall.Core.UI.FactionIconCatalog.HasExplicitMapping(id);
                    if (!covered)
                        missing.Add(id);
                }
                totalIds += ids.Count;
                totalMissing += missing.Count;
                missingByCategory[category] = missing;
            }

            AssetCoverageReport.PrintFullCoverageSweep(totalIds, totalMissing, missingByCategory, idsByCategory);
        }

        private static List<string> ExtractIdsFromJson(string path, string fieldName)
        {
            var ids = new List<string>();
            try
            {
                string content = File.ReadAllText(path);
                string pattern = $"\"{fieldName}\":";

                int pos = 0;
                int maxIterations = content.Length * 2;
                int iterations = 0;
                while (true)
                {
                    iterations++;
                    if (iterations > maxIterations)
                    {
                        GD.PrintErr($"[AssetRegistrySelfTest] ExtractIdsFromJson exceeded max iterations for {path} field {fieldName}");
                        break;
                    }
                    int idx = content.IndexOf(pattern, pos, StringComparison.Ordinal);
                    if (idx < 0) break;

                    pos = idx + pattern.Length;
                    while (pos < content.Length && char.IsWhiteSpace(content[pos]))
                        pos++;

                    if (pos >= content.Length || content[pos] != '"')
                        continue;
                    pos++;

                    int start = pos;
                    while (pos < content.Length && content[pos] != '"')
                    {
                        if (content[pos] == '\\')
                            pos++;
                        pos++;
                    }

                    if (pos > start && pos < content.Length)
                    {
                        string id = content.Substring(start, pos - start);
                        if (!string.IsNullOrEmpty(id) && !ids.Contains(id))
                            ids.Add(id);
                    }

                    pos++;
                }
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[AssetRegistrySelfTest] Failed to extract IDs from {path}: {ex.Message}");
            }
            return ids;
        }
    }
}
