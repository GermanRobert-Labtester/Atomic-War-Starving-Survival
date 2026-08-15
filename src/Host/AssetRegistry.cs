using System;
using System.Collections.Generic;
using System.IO;
using Godot;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Result of an asset lookup operation.
    /// </summary>
    public enum AssetLoadResult
    {
        /// <summary>Asset loaded successfully.</summary>
        Loaded,
        /// <summary>Asset ID was valid, but no file exists at the resolved path.</summary>
        Missing,
        /// <summary>Asset path exists, but ResourceLoader.Load returned null.</summary>
        FailedToLoad,
        /// <summary>Fallback texture was returned (primary asset missing).</summary>
        FallbackUsed,
        /// <summary>Invalid/empty ID provided.</summary>
        InvalidId
    }

    /// <summary>
    /// Holds the result of an asset resolution attempt.
    /// </summary>
    public readonly struct AssetResult
    {
        public readonly Texture2D? Texture;
        public readonly AssetLoadResult Result;
        public readonly string ResolvedPath;
        public readonly string RequestedId;

        public AssetResult(Texture2D? texture, AssetLoadResult result, string resolvedPath, string requestedId)
        {
            Texture = texture;
            Result = result;
            ResolvedPath = resolvedPath;
            RequestedId = requestedId;
        }

        public bool IsValid => Texture != null && (Result == AssetLoadResult.Loaded || Result == AssetLoadResult.FallbackUsed);
        public bool IsMissing => Result == AssetLoadResult.Missing || Result == AssetLoadResult.FailedToLoad;
    }

    /// <summary>
    /// Thin, presentation-only asset registry that maps catalog IDs to Godot Texture2D resources.
    /// 
    /// Path resolution order:
    /// 1. assets/art/{id}.jpg (primary - most items/locations/survivors)
    /// 2. assets/art/{id}.png (alternate format)
    /// 3. assets/sprites/Items/{id}.png (item sprites)
    /// 4. assets/sprites/Portraits/{id}.png (survivor portraits)
    /// 5. assets/sprites/Locations/{id}.png (location sprites)
    /// 
    /// No simulation logic here — this is purely ID → path → Texture2D.
    /// Uses ResourceLoader.Load for Godot-native resource loading.
    /// Logs missing assets once per ID to avoid per-frame spam.
    /// </summary>
    public static class AssetRegistry
    {
        private static readonly string[] ItemSearchPaths = new[]
        {
            "res://assets/art/{0}.jpg",
            "res://assets/art/{0}.png",
            "res://assets/sprites/Items/{0}.png",
            "res://assets/sprites/items/{0}.png"
        };

        private static readonly string[] PortraitSearchPaths = new[]
        {
            "res://assets/art/{0}.jpg",
            "res://assets/art/{0}.png",
            "res://assets/sprites/Portraits/{0}.png",
            "res://assets/sprites/portraits/{0}.png"
        };

        private static readonly string[] LocationSearchPaths = new[]
        {
            "res://assets/art/{0}.jpg",
            "res://assets/art/{0}.png",
            "res://assets/sprites/Locations/{0}.png",
            "res://assets/sprites/locations/{0}.png"
        };

        private static readonly string[] FactionSearchPaths = new[]
        {
            "res://assets/art/{0}.jpg",
            "res://assets/art/{0}.png",
            "res://assets/sprites/Factions/{0}.png",
            "res://assets/sprites/factions/{0}.png"
        };

        private static readonly HashSet<string> _loggedMissing = new HashSet<string>();
        private static Texture2D? _fallbackTexture;
        private static bool _fallbackWarned;

        /// <summary>
        /// Sets a fallback texture to return when an asset is missing.
        /// Optional; if not set, missing assets return null.
        /// </summary>
        public static void SetFallbackTexture(Texture2D? texture)
        {
            _fallbackTexture = texture;
        }

        /// <summary>
        /// Gets an item texture by ID.
        /// </summary>
        public static AssetResult GetItem(string itemId)
        {
            return GetWithPaths(itemId, ItemSearchPaths, "item");
        }

        /// <summary>
        /// Gets a survivor portrait by ID.
        /// </summary>
        public static AssetResult GetPortrait(string survivorId)
        {
            return GetWithPaths(survivorId, PortraitSearchPaths, "portrait");
        }

        /// <summary>
        /// Gets a location image by ID.
        /// </summary>
        public static AssetResult GetLocation(string locationId)
        {
            return GetWithPaths(locationId, LocationSearchPaths, "location");
        }

        /// <summary>
        /// Gets a faction icon by ID.
        /// </summary>
        public static AssetResult GetFaction(string factionId)
        {
            return GetWithPaths(factionId, FactionSearchPaths, "faction");
        }

        /// <summary>
        /// Gets a texture by explicit path (for UI elements, etc.).
        /// </summary>
        public static AssetResult GetByPath(string path)
        {
            if (string.IsNullOrEmpty(path))
                return new AssetResult(null, AssetLoadResult.InvalidId, "", "");

            if (!path.StartsWith("res://"))
                path = "res://" + path;

            return LoadTexture(path, path, "direct");
        }

        /// <summary>
        /// Returns the resolved path for an item without loading it.
        /// Returns null if no matching file exists.
        /// </summary>
        public static string? ResolveItemPath(string itemId)
        {
            return ResolvePath(itemId, ItemSearchPaths);
        }

        /// <summary>
        /// Returns the resolved path for a portrait without loading it.
        /// Returns null if no matching file exists.
        /// </summary>
        public static string? ResolvePortraitPath(string survivorId)
        {
            return ResolvePath(survivorId, PortraitSearchPaths);
        }

        /// <summary>
        /// Returns the resolved path for a location without loading it.
        /// Returns null if no matching file exists.
        /// </summary>
        public static string? ResolveLocationPath(string locationId)
        {
            return ResolvePath(locationId, LocationSearchPaths);
        }

        /// <summary>
        /// Clears the missing-asset log (useful between scenes or for re-testing).
        /// </summary>
        public static void ClearMissingLog()
        {
            _loggedMissing.Clear();
        }

        /// <summary>
        /// Returns the number of unique missing assets that have been logged.
        /// </summary>
        public static int MissingAssetCount => _loggedMissing.Count;

        private static AssetResult GetWithPaths(string id, string[] searchPaths, string category)
        {
            if (string.IsNullOrEmpty(id))
                return new AssetResult(null, AssetLoadResult.InvalidId, "", "");

            string? existingPath = ResolvePath(id, searchPaths);
            if (existingPath != null)
            {
                return LoadTexture(existingPath, id, category);
            }

            // Asset missing at all paths
            string logKey = $"{category}:{id}";
            if (!_loggedMissing.Contains(logKey))
            {
                _loggedMissing.Add(logKey);
                GD.Print($"[AssetRegistry] MISSING {category}: '{id}' (tried {searchPaths.Length} paths)");
            }

            // Return fallback if available
            if (_fallbackTexture != null)
            {
                if (!_fallbackWarned)
                {
                    _fallbackWarned = true;
                    GD.Print("[AssetRegistry] Using fallback texture for missing assets");
                }
                return new AssetResult(_fallbackTexture, AssetLoadResult.FallbackUsed, "(fallback)", id);
            }

            return new AssetResult(null, AssetLoadResult.Missing, "(none)", id);
        }

        private static string? ResolvePath(string id, string[] searchPaths)
        {
            if (string.IsNullOrEmpty(id))
                return null;

            foreach (var pattern in searchPaths)
            {
                string path = string.Format(pattern, id);
                if (ResourceLoader.Exists(path))
                    return path;
            }
            return null;
        }

        private static AssetResult LoadTexture(string path, string id, string category)
        {
            var texture = ResourceLoader.Load<Texture2D>(path);
            if (texture != null)
            {
                return new AssetResult(texture, AssetLoadResult.Loaded, path, id);
            }

            // ResourceLoader.Exists returned true but Load returned null
            string logKey = $"{category}:{id}";
            if (!_loggedMissing.Contains(logKey))
            {
                _loggedMissing.Add(logKey);
                GD.PrintErr($"[AssetRegistry] FAILED TO LOAD {category}: '{id}' at path: {path}");
            }

            if (_fallbackTexture != null)
            {
                return new AssetResult(_fallbackTexture, AssetLoadResult.FallbackUsed, path, id);
            }

            return new AssetResult(null, AssetLoadResult.FailedToLoad, path, id);
        }
    }

    /// <summary>
    /// Headless self-test for AssetRegistry.
    /// Verifies that the top N referenced assets from catalogs actually exist.
    /// Used by --asset-registry-selftest.
    /// </summary>
    public static class AssetRegistrySelfTest
    {
        public struct ResultRow
        {
            public string Id;
            public string Category;
            public string? ResolvedPath;
            public bool Exists;
            public bool Loaded;
            public int ReferenceCount;
        }

        public struct Report
        {
            public int TotalChecked;
            public int Missing;
            public int FailedToLoad;
            public int Passed;
            public List<ResultRow> Rows;
            public string Summary;
            public bool Clean => Missing == 0 && FailedToLoad == 0;
        }

        /// <summary>
        /// Runs the self-test using actual catalog data.
        /// dataDir should be the path to StreamingAssets/Data.
        /// </summary>
        public static Report Run(string dataDir, int topCount = 50)
        {
            GD.Print("[AssetRegistrySelfTest] Starting...");
            GD.Print($"[AssetRegistrySelfTest] Data dir: {dataDir}");
            GD.Print($"[AssetRegistrySelfTest] Checking top {topCount} referenced assets");

            AssetRegistry.ClearMissingLog();

            var rows = new List<ResultRow>();
            var referenceCounts = new Dictionary<string, int>();

            // First, discover referenced assets from catalogs
            // For now, we'll check:
            // 1. All item IDs from items.json
            // 2. All survivor IDs from survivors.json
            // 3. All location IDs from locations.json

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

                // Take up to topCount/3 items
                int itemLimit = Math.Min(topCount / 3, itemIds.Count);
                for (int i = 0; i < itemLimit; i++)
                {
                    string id = itemIds[i];
                    var result = AssetRegistry.GetItem(id);
                    rows.Add(new ResultRow
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
                    rows.Add(new ResultRow
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
                    rows.Add(new ResultRow
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

            // Also test some well-known critical assets
            string[] criticalAssets = new[]
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
                // Check if already checked
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

                // Try as item first
                var result = AssetRegistry.GetItem(asset);
                if (result.Result == AssetLoadResult.Missing)
                {
                    // Try as portrait
                    result = AssetRegistry.GetPortrait(asset);
                }
                if (result.Result == AssetLoadResult.Missing)
                {
                    // Try as location
                    result = AssetRegistry.GetLocation(asset);
                }

                rows.Add(new ResultRow
                {
                    Id = asset,
                    Category = "critical",
                    ResolvedPath = result.ResolvedPath,
                    Exists = result.Result != AssetLoadResult.Missing,
                    Loaded = result.Result == AssetLoadResult.Loaded,
                    ReferenceCount = 999
                });
            }

            // Count results
            int total = rows.Count;
            int missing = 0;
            int failed = 0;
            int passed = 0;

            foreach (var row in rows)
            {
                if (!row.Exists)
                    missing++;
                else if (!row.Loaded)
                    failed++;
                else
                    passed++;
            }

            string summary =
                $"ASSET_REGISTRY_SELFTEST: checked={total} passed={passed} missing={missing} load-failed={failed}";

            GD.Print($"[AssetRegistrySelfTest] --- SUMMARY ---");
            GD.Print($"[AssetRegistrySelfTest] Total checked: {total}");
            GD.Print($"[AssetRegistrySelfTest] Passed: {passed}");
            GD.Print($"[AssetRegistrySelfTest] Missing: {missing}");
            GD.Print($"[AssetRegistrySelfTest] Failed to load: {failed}");
            GD.Print($"[AssetRegistrySelfTest] {summary}");

            if (missing > 0 || failed > 0)
            {
                GD.Print("[AssetRegistrySelfTest] --- ISSUES ---");
                foreach (var row in rows)
                {
                    if (!row.Exists)
                    {
                        GD.Print($"[AssetRegistrySelfTest] MISSING: [{row.Category}] {row.Id}");
                    }
                    else if (!row.Loaded)
                    {
                        GD.Print($"[AssetRegistrySelfTest] LOAD FAILED: [{row.Category}] {row.Id} at {row.ResolvedPath}");
                    }
                }
            }

            bool clean = missing == 0 && failed == 0;
            GD.Print(clean
                ? "ASSET_REGISTRY_SELFTEST PASS"
                : $"ASSET_REGISTRY_SELFTEST FAIL (missing={missing}, failed={failed})");

            return new Report
            {
                TotalChecked = total,
                Missing = missing,
                FailedToLoad = failed,
                Passed = passed,
                Rows = rows,
                Summary = summary
            };
        }

        /// <summary>
        /// Simple JSON ID extractor that doesn't require a full parser.
        /// Looks for "id": "value" patterns.
        /// </summary>
        private static List<string> ExtractIdsFromJson(string path, string fieldName)
        {
            var ids = new List<string>();
            try
            {
                string content = File.ReadAllText(path);
                string pattern = $"\"{fieldName}\":";

                int pos = 0;
                while (true)
                {
                    int idx = content.IndexOf(pattern, pos, StringComparison.Ordinal);
                    if (idx < 0) break;

                    // Move past the pattern
                    pos = idx + pattern.Length;

                    // Skip whitespace
                    while (pos < content.Length && char.IsWhiteSpace(content[pos]))
                        pos++;

                    // Expect "
                    if (pos >= content.Length || content[pos] != '"')
                        continue;
                    pos++;

                    // Extract value until closing "
                    int start = pos;
                    while (pos < content.Length && content[pos] != '"')
                    {
                        if (content[pos] == '\\')
                            pos++; // Skip escape
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
