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
        // ── Centralized Fallback Paths ──────────────────────────────────
        /// <summary>Canonical resource path for the fallback survivor sprite/portrait.</summary>
        public const string FallbackSurvivorPath = "res://assets/sprites/Characters/placeholder_survivor.png";

        /// <summary>Relative asset path for the fallback survivor sprite/portrait.</summary>
        public const string FallbackSurvivorRelativePath = "assets/sprites/Characters/placeholder_survivor.png";

        /// <summary>Canonical resource path for the fallback UI placeholder icon.</summary>
        public const string FallbackIconPath = "res://assets/ui/Icons/icon_placeholder.png";

        /// <summary>Relative asset path for the fallback UI placeholder icon.</summary>
        public const string FallbackIconRelativePath = "assets/ui/Icons/icon_placeholder.png";

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

        // ── ID Aliases ──────────────────────────────────────────────────
        // Catalog IDs may not match the on-disk filename. Aliases are kept as
        // semantic fallback candidates after the literal stem, so a single
        // asset can satisfy multiple IDs when no direct file exists. Direct
        // catalog stems intentionally win when both forms are present.
        // Adding a fallback entry here is cheaper than renaming source art.
        // The alias value is the file stem (without extension) under assets/art/.
        private static readonly Dictionary<string, string> ItemIdAliases = new(StringComparer.Ordinal)
        {
            { "mechanical_components", "scrap_mechanical" },
            { "mechanical_parts",      "scrap_mechanical" },
            { "scrap_mechanical",      "scrap_mechanical" }, // self-alias for safety
        };

        // ── Prefix-add normalization ─────────────────────────────────
        // Some catalog IDs are bare stems ("blood_bag") while the asset on
        // disk is prefixed ("item_blood_bag.jpg"). This is the *opposite*
        // direction of the canonical "item_X" -> "X.jpg" assumption that an
        // earlier audit hypothesised, and was verified against the actual
        // filesystem layout (Phase-13 wiring-truth reconciliation).
        //
        // Resolution policy (in priority order, deterministic, category-aware):
        //   1. Direct stem  ({id}.jpg / .png across the four category roots)
        //   2. Explicit semantic alias from ItemIdAliases (for "item" only)
        //   3. Prefix-add candidate (per-category; see PrefixAddMap below)
        //
        // Strict ordering guarantees: collisions cannot silently choose wrong
        // assets — direct stem wins, then alias (semantic, hand-curated), then
        // prefix-add (mechanical fall-back). No filesystem-wide recursion.
        //
        // Prefix-strip is intentionally NOT supported. Stripping would create
        // ambiguous resolution when, e.g., the asset side does already use
        // "item_X" (the dominant convention in this codebase). Only prefix-ADD
        // is enabled, and only for stems that lack the corresponding prefix
        // in {id}, which is the verified failure mode.
        //
        // Format: { category, prefix_to_add_if_missing }.
        private static readonly (string category, string prefix)[] PrefixAddRules = new[]
        {
            ("item",     "item_"),
            ("portrait", "survivor_"),
            ("portrait", "npc_"),
            ("location", "loc_"),
            ("faction",  "faction_"),
        };

        /// <summary>
        /// Structured diagnostic record for a missing or failed-to-load asset lookup.
        /// </summary>
        public readonly struct MissingAssetWarning
        {
            public readonly string Category;
            public readonly string RequestedId;
            public readonly string FallbackUsed;
            public readonly string Message;

            public MissingAssetWarning(string category, string requestedId, string fallbackUsed, string message)
            {
                Category = category;
                RequestedId = requestedId;
                FallbackUsed = fallbackUsed;
                Message = message;
            }

            public override string ToString() => Message;
        }

        private static readonly HashSet<string> _loggedMissing = new HashSet<string>(StringComparer.Ordinal);
        private static readonly Dictionary<string, MissingAssetWarning> _loggedWarnings = new(StringComparer.Ordinal);
        private static int _totalFallbackRequests;
        private static int _duplicateFallbackRequests;
        private static Texture2D? _fallbackTexture;

        /// <summary>
        /// Total count of duplicate / repeated fallback requests for assets already identified as missing.
        /// </summary>
        public static int DuplicateFallbackRequestCount => _duplicateFallbackRequests;

        /// <summary>
        /// Total count of all missing or fallback asset lookup requests (unique + duplicates).
        /// </summary>
        public static int TotalFallbackRequestCount => _totalFallbackRequests;

        /// <summary>
        /// Returns the active or default fallback asset path/description for the given category.
        /// </summary>
        public static string GetEffectiveFallbackDescription(string category)
        {
            if (_fallbackTexture != null)
            {
                return !string.IsNullOrEmpty(_fallbackTexture.ResourcePath)
                    ? _fallbackTexture.ResourcePath
                    : "(custom fallback texture)";
            }

            return category switch
            {
                "portrait" or "character" or "survivor" => FallbackSurvivorPath,
                "item" or "icon" or "faction" => FallbackIconPath,
                _ => "(none)"
            };
        }

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
            var candidates = ResolveStemCandidates(itemId, "item");
            return ResolveByCandidates(itemId, candidates, ItemSearchPaths, "item");
        }

        private static bool TryResolveAlias(string itemId, out string aliasStem)
        {
            return ItemIdAliases.TryGetValue(itemId, out aliasStem!);
        }

        /// <summary>
        /// Gets a survivor portrait by ID.
        /// </summary>
        public static AssetResult GetPortrait(string survivorId)
        {
            var candidates = ResolveStemCandidates(survivorId, "portrait");
            return ResolveByCandidates(survivorId, candidates, PortraitSearchPaths, "portrait");
        }

        /// <summary>
        /// Gets a location image by ID.
        /// </summary>
        public static AssetResult GetLocation(string locationId)
        {
            var candidates = ResolveStemCandidates(locationId, "location");
            return ResolveByCandidates(locationId, candidates, LocationSearchPaths, "location");
        }

        /// <summary>
        /// Gets a faction icon by ID.
        /// </summary>
        public static AssetResult GetFaction(string factionId)
        {
            var candidates = ResolveStemCandidates(factionId, "faction");
            return ResolveByCandidates(factionId, candidates, FactionSearchPaths, "faction");
        }

        /// <summary>
        /// Walks the candidate stem list (in deterministic order) until one
        /// resolves to a real resource. Reports whether the result was
        /// arrived at via the literal stem, the semantic alias, or the
        /// prefix-add normalization. Returns AssetLoadResult.Loaded on a real
        /// hit, FallbackUsed if the fallback texture is active, Missing if no
        /// candidate resolved, InvalidId if the input was empty.
        /// </summary>
        private static AssetResult ResolveByCandidates(
            string originalId,
            IReadOnlyList<(string stem, string origin)> candidates,
            string[] searchPaths,
            string category)
        {
            if (string.IsNullOrEmpty(originalId) || candidates.Count == 0)
            {
                return new AssetResult(null, AssetLoadResult.InvalidId, "", originalId ?? "");
            }

            for (int i = 0; i < candidates.Count; i++)
            {
                var (stem, origin) = candidates[i];
                string? path = ResolvePath(stem, searchPaths);
                if (path == null) continue;

                var result = LoadTexture(path, originalId, category, origin);
                if (result.Result == AssetLoadResult.Loaded ||
                    result.Result == AssetLoadResult.FallbackUsed)
                {
                    return result;
                }
            }

            // Nothing matched. Surface as before.
            string logKey = $"{category}:{originalId}";
            _totalFallbackRequests++;
            if (!_loggedMissing.Contains(logKey))
            {
                _loggedMissing.Add(logKey);
                string fallbackUsed = GetEffectiveFallbackDescription(category);
                string msg = $"[AssetRegistry] MISSING {category}: '{originalId}' (fallback: '{fallbackUsed}', tried {candidates.Count} candidate stems × {searchPaths.Length} paths)";
                _loggedWarnings[logKey] = new MissingAssetWarning(category, originalId, fallbackUsed, msg);
                GD.Print(msg);
            }
            else
            {
                _duplicateFallbackRequests++;
            }
            if (_fallbackTexture != null)
            {
                return new AssetResult(_fallbackTexture, AssetLoadResult.FallbackUsed, "(fallback)", originalId);
            }
            return new AssetResult(null, AssetLoadResult.Missing, "(none)", originalId);
        }

        // ── Candidate stem resolution for category-aware prefix-add ────
        //
        // Given a requested id + kind, produce the ordered list of file
        // stems we should attempt to load. Order is deterministic:
        //   1. The literal requested stem (e.g. "blood_bag")
        //   2. The semantic-alias stem if one exists (item-only, e.g.
        //      "mechanical_components" → "scrap_mechanical")
        //   3. Each prefix-add rule's prefixed stem (e.g. "item_blood_bag")
        //      — but ONLY if the prefix doesn't already appear in the
        //      requested id, so we never produce duplicates.
        //
        // Returns a non-null list of (stem, origin) tuples for the caller to
        // try in order, with no I/O and no recursion. The caller performs
        // ResourceLoader.Exists per stem per search root.
        internal static IReadOnlyList<(string stem, string origin)> ResolveStemCandidates(
            string id, string kind)
        {
            var candidates = new List<(string, string)>(8);
            if (string.IsNullOrEmpty(id))
                return candidates;

            candidates.Add((id, "literal"));

            if (kind == "item" && ItemIdAliases.TryGetValue(id, out var aliasStem))
            {
                // Semantic alias: use the alias stem itself; it is the
                // canonical filename in assets/art/.
                candidates.Add((aliasStem, "semantic-alias"));
            }

            foreach (var (cat, prefix) in PrefixAddRules)
            {
                if (cat != kind) continue;
                if (id.StartsWith(prefix, StringComparison.Ordinal)) continue;
                candidates.Add((prefix + id, "prefix-add"));
            }

            return candidates;
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
        /// Clears the missing-asset log and diagnostic warning records (useful between scenes or for re-testing).
        /// </summary>
        public static void ClearMissingLog()
        {
            _loggedMissing.Clear();
            _loggedWarnings.Clear();
            _totalFallbackRequests = 0;
            _duplicateFallbackRequests = 0;
        }

        /// <summary>
        /// Returns the number of unique missing assets that have been logged.
        /// </summary>
        public static int MissingAssetCount => _loggedMissing.Count;

        /// <summary>
        /// Gets all unique missing-asset warning records logged during this session.
        /// </summary>
        public static IReadOnlyCollection<MissingAssetWarning> LoggedWarnings => _loggedWarnings.Values;

        /// <summary>
        /// Checks if a warning was already logged for the specified category and ID.
        /// </summary>
        public static bool HasLoggedWarning(string category, string id) => _loggedWarnings.ContainsKey($"{category}:{id}");

        /// <summary>
        /// Retrieves the logged warning for a specific category and ID, or null if none was logged.
        /// </summary>
        public static MissingAssetWarning? GetLoggedWarning(string category, string id) =>
            _loggedWarnings.TryGetValue($"{category}:{id}", out var warning) ? warning : null;

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
            _totalFallbackRequests++;
            if (!_loggedMissing.Contains(logKey))
            {
                _loggedMissing.Add(logKey);
                string fallbackUsed = GetEffectiveFallbackDescription(category);
                string msg = $"[AssetRegistry] MISSING {category}: '{id}' (fallback: '{fallbackUsed}', tried {searchPaths.Length} paths)";
                _loggedWarnings[logKey] = new MissingAssetWarning(category, id, fallbackUsed, msg);
                GD.Print(msg);
            }
            else
            {
                _duplicateFallbackRequests++;
            }

            // Return fallback if available
            if (_fallbackTexture != null)
            {
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

        private static AssetResult LoadTexture(string path, string id, string category,
            string origin = "literal")
        {
            var texture = ResourceLoader.Load<Texture2D>(path);
            if (texture != null)
            {
                return new AssetResult(texture, AssetLoadResult.Loaded, path, id);
            }

            // ResourceLoader.Exists returned true but Load returned null
            string logKey = $"{category}:{id}";
            _totalFallbackRequests++;
            if (!_loggedMissing.Contains(logKey))
            {
                _loggedMissing.Add(logKey);
                string fallbackUsed = GetEffectiveFallbackDescription(category);
                string msg = $"[AssetRegistry] FAILED TO LOAD {category}: '{id}' at path: {path} (origin={origin}, fallback: '{fallbackUsed}')";
                _loggedWarnings[logKey] = new MissingAssetWarning(category, id, fallbackUsed, msg);
                GD.PrintErr(msg);
            }
            else
            {
                _duplicateFallbackRequests++;
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
            public int UniqueMissing;
            public int DuplicateFallbackRequests;
            public int FailedToLoad;
            public int Passed;
            /// <summary>Normalization/negative/dedup probe mismatches (gate-blocking).</summary>
            public int ProbeFailures;
            public List<ResultRow> Rows;
            public string Summary;
            public bool Clean => Missing == 0 && FailedToLoad == 0 && ProbeFailures == 0;
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

            // ── Fallback-art smoke assertions ───────────────────────────────
            // Smoke-test that key fallback/placeholder art assets resolve and load:
            // - placeholder_survivor.png (used by SurvivorActorView and character fallbacks)
            // - icon_placeholder.png (used across UI sidebars and icon fallbacks)
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

                rows.Add(new ResultRow
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

            // ── Phase 13 normalization / prefix-add assertions ──────────────
            // Verify the new category-aware prefix-add normalization against
            // canonical, named, expected outcomes. Each assertion checks the
            // actual resolved path, not just existence (a generic/texture
            // fallback would otherwise pass this gate by accident).

            // Phase-13 expected outcomes. Each tuple:
            //   (id, category, expected_filename_in_resolved_path)
            // We use the filename of the file actually on disk so this test
            // never goes stale as the asset library grows.
            // Negative probes (and normalisation probes that are designed to
            // fail) are part of the gate's auditing logic; they are NOT
            // counted toward the production-readiness Missing/Failed sum.
            // We track them on a separate counter so the summary line stays
            // comparable to the Phase 11/12 baseline.
            int probesChecked = 0;
            int probesFailingAsIntended = 0;
            // Gate-blocking probe failures: norm-probe mismatches, negative-probe
            // resolutions, and deduplication contract violations. These surface
            // in Report.ProbeFailures and fail the selftest, not just stderr.
            int probeFailures = 0;
            // Scratch pad for probes that failed beyond what was expected —
            // those become real Missing rows below.
            var probeRows = new List<ResultRow>();

            var normalizationProbes = new (string id, string category, string expectFileStem, bool expectMissing)[]
            {
                // Direct stems exist for both material IDs, so literal
                // resolution correctly wins over the dormant semantic alias.
                ("mechanical_components", "item",    "mechanical_components",       false),
                ("mechanical_parts",      "item",    "mechanical_parts",            false),
                ("blood_bag",             "item",    "item_blood_bag",              false),
                ("encrypted_drive",       "item",    "encrypted_drive",             false),
                ("faraday_pack",          "item",    "faraday_pack",                false),
                // Phase 14: the sealed cigarette pack asset has shipped.
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
                var probe = new ResultRow
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

            // ── Negative / collision safety probes ────────────────────
            // These IDs should NOT resolve to any production asset. If
            // prefix-add normalization ever fires when it shouldn't, these
            // will start resolving silently and we'll detect that here.
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
                var probe = new ResultRow
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

            // ── Fallback diagnostic deduplication checks ──────────────
            int dedupBaseline = AssetRegistry.MissingAssetCount;
            // Re-query the negative probe to prove deduplication per session
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

            // ── Missing-warning dedup matrix: category AND id ───────────
            // The warning log is keyed "{category}:{id}". Contract under test:
            //   1. same (category, id) queried repeatedly → logged exactly once
            //   2. same id in a different category       → logs separately
            //   3. distinct ids in the same category     → each logs once
            //   4. never-queried pairs                   → no record
            //   5. every record carries category, id, fallback, and a
            //      message naming both so warnings stay actionable
            GD.Print("[AssetRegistrySelfTest] Checking missing-warning deduplication (category × id)...");
            void DedupCheck(bool cond, string name)
            {
                if (cond) return;
                probeFailures++;
                GD.PrintErr($"[AssetRegistrySelfTest] DEDUP FAILED: {name}");
            }

            const string DedupIdA = "__dedup_probe_a__";
            const string DedupIdB = "__dedup_probe_b__";
            const string DedupNeverQueried = "__never_queried__";

            int dupBaseline = AssetRegistry.DuplicateFallbackRequestCount;

            // 1. First query of (item, A) logs exactly one warning.
            AssetRegistry.GetItem(DedupIdA);
            DedupCheck(AssetRegistry.MissingAssetCount == dedupBaseline + 1,
                $"first query of item '{DedupIdA}' should add exactly one warning");
            // 2. Repeated queries of the same (category, id) add to duplicate fallback request count.
            AssetRegistry.GetItem(DedupIdA);
            AssetRegistry.GetItem(DedupIdA);
            DedupCheck(AssetRegistry.MissingAssetCount == dedupBaseline + 1,
                $"repeated queries of item '{DedupIdA}' must not re-log unique warning");
            DedupCheck(AssetRegistry.DuplicateFallbackRequestCount == dupBaseline + 2,
                $"repeated queries of item '{DedupIdA}' should increment duplicate fallback request count by 2");
            DedupCheck(AssetRegistry.HasLoggedWarning("item", DedupIdA),
                $"item '{DedupIdA}' should have a logged warning");
            // 3. Same id in a different category logs separately — the dedup
            //    key includes the category, not just the id.
            AssetRegistry.GetPortrait(DedupIdA);
            DedupCheck(AssetRegistry.MissingAssetCount == dedupBaseline + 2,
                $"same id in a different category ('{DedupIdA}' as portrait) must log separately");
            DedupCheck(AssetRegistry.HasLoggedWarning("portrait", DedupIdA),
                $"portrait '{DedupIdA}' should have its own warning");
            // 4. Distinct id in the same category logs its own warning.
            AssetRegistry.GetItem(DedupIdB);
            DedupCheck(AssetRegistry.MissingAssetCount == dedupBaseline + 3,
                $"distinct id ('{DedupIdB}') in the same category must log separately");
            DedupCheck(AssetRegistry.HasLoggedWarning("item", DedupIdB),
                $"item '{DedupIdB}' should have a logged warning");
            // 5. Never-queried pairs have no record.
            DedupCheck(!AssetRegistry.HasLoggedWarning("item", DedupNeverQueried),
                "unqueried pair must not be flagged");
            DedupCheck(AssetRegistry.GetLoggedWarning("portrait", DedupIdB) == null,
                $"portrait '{DedupIdB}' was never queried and must have no record");
            // 6. Records carry category, id, fallback, and an actionable message.
            var recordA = AssetRegistry.GetLoggedWarning("item", DedupIdA);
            DedupCheck(recordA != null
                && recordA.Value.Category == "item"
                && recordA.Value.RequestedId == DedupIdA
                && !string.IsNullOrEmpty(recordA.Value.FallbackUsed)
                && recordA.Value.Message.Contains("MISSING item", StringComparison.Ordinal)
                && recordA.Value.Message.Contains(DedupIdA, StringComparison.Ordinal),
                $"warning record for (item, '{DedupIdA}') must carry category, id, fallback and message");
            var recordP = AssetRegistry.GetLoggedWarning("portrait", DedupIdA);
            DedupCheck(recordP != null && recordP.Value.Category == "portrait"
                && recordP.Value.RequestedId == DedupIdA,
                $"warning record for (portrait, '{DedupIdA}') must carry its own category");
            // 7. LoggedWarnings holds exactly one entry per unique category:id.
            DedupCheck(AssetRegistry.LoggedWarnings.Count == dedupBaseline + 3,
                $"LoggedWarnings should hold one entry per unique category:id "
                + $"({AssetRegistry.LoggedWarnings.Count} vs expected {dedupBaseline + 3})");

            GD.Print($"[AssetRegistrySelfTest] Total probes evaluated: {probesChecked}, mismatches against expectation: {probeFailures}");
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

            int total = rows.Count;
            int uniqueMissing = AssetRegistry.MissingAssetCount;
            int duplicateFallbackRequests = AssetRegistry.DuplicateFallbackRequestCount;

            string summary =
                $"ASSET_REGISTRY_SELFTEST: checked={total} passed={passed} missing={missing} (unique={uniqueMissing}, duplicate_fallback_requests={duplicateFallbackRequests}) load-failed={failed} probe-failures={probeFailures}";

            GD.Print($"[AssetRegistrySelfTest] --- SUMMARY ---");
            GD.Print($"[AssetRegistrySelfTest] Total checked: {total}");
            GD.Print($"[AssetRegistrySelfTest] Passed: {passed}");
            GD.Print($"[AssetRegistrySelfTest] Missing (checked entries): {missing}");
            GD.Print($"[AssetRegistrySelfTest] Unique missing assets: {uniqueMissing}");
            GD.Print($"[AssetRegistrySelfTest] Duplicate fallback requests: {duplicateFallbackRequests}");
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

            bool clean = missing == 0 && failed == 0 && probeFailures == 0;
            GD.Print(clean
                ? "ASSET_REGISTRY_SELFTEST PASS"
                : $"ASSET_REGISTRY_SELFTEST FAIL (missing={missing}, failed={failed}, probe-failures={probeFailures})");

            return new Report
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
        }

        // ── Full-catalog coverage report ─────────────────────────────────
        //
        // The gating selftest above samples the top N referenced ids; this
        // sweep walks EVERY definition id in every catalog file (core +
        // expansions) so asset-generation batches can be tracked to zero.
        // Report-only by design: it prints per-category coverage and the
        // missing-id list, but never fails the run. The gate remains
        // --asset-registry-selftest.
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

        public static void RunFullCoverage(string dataDir)
        {
            GD.Print("[AssetCoverageReport] Full catalog sweep — report-only, non-gating");
            GD.Print($"[AssetCoverageReport] Data dir: {dataDir}");

            var idsByCategory = new Dictionary<string, List<string>>();
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
                    // The extractor scans raw text, so narrative strings can
                    // yield pseudo-ids ('faction_x', "npc_y ..."). Only
                    // canonical snake_case stems are real catalog ids.
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
                    // Lore-namespace factions count as covered when an emblem
                    // exists even without a full assets/art illustration.
                    if (!covered && category == "faction")
                        covered = Ashfall.Core.UI.FactionIconCatalog.HasExplicitMapping(id);
                    if (!covered)
                        missing.Add(id);
                }
                totalIds += ids.Count;
                totalMissing += missing.Count;
                GD.Print($"[AssetCoverageReport] {category,-9}: {ids.Count,4} ids, {ids.Count - missing.Count,4} resolved, {missing.Count,4} missing");
                foreach (var id in missing)
                    GD.Print($"[AssetCoverageReport]   MISSING {category}: {id}");
            }

            GD.Print($"ASSET_COVERAGE_REPORT: ids={totalIds} resolved={totalIds - totalMissing} missing={totalMissing} (report-only; gate remains --asset-registry-selftest)");
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
                int maxIterations = content.Length * 2; // pathological guard: no JSON field should need more scans than chars
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
