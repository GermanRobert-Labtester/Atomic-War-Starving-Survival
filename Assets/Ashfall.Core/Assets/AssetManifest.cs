// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Assets
{
    /// <summary>
    /// Status of an asset resolution attempt.
    /// Distinguishes explicit manifest bindings from convention guesses and fallbacks.
    /// </summary>
    public enum AssetResolveStatus
    {
        /// <summary>Resolved directly via declared entry in the manifest.</summary>
        LoadedExplicit = 0,

        /// <summary>Resolved via candidate path stem convention.</summary>
        LoadedByConvention = 1,

        /// <summary>Primary asset missing; family scratch fallback used.</summary>
        FallbackUsed = 2,

        /// <summary>No manifest entry, no convention file, and no fallback available.</summary>
        Missing = 3
    }

    /// <summary>
    /// An entry in the authoritative asset manifest.
    /// Maps an authored ID to its actual path, family, provenance, and import preset.
    /// </summary>
    public sealed class AssetManifestEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("family")]
        public string Family { get; set; } = string.Empty;

        [JsonPropertyName("kind")]
        public string Kind { get; set; } = string.Empty;

        [JsonPropertyName("path")]
        public string Path { get; set; } = string.Empty;

        [JsonPropertyName("source")]
        public string Source { get; set; } = string.Empty;

        [JsonPropertyName("is_ai_generated")]
        public bool IsAiGenerated { get; set; }

        [JsonPropertyName("import_preset")]
        public string ImportPreset { get; set; } = string.Empty;

        [JsonPropertyName("is_fallback")]
        public bool IsFallback { get; set; }

        public AssetManifestEntry() { }

        public AssetManifestEntry(
            string id,
            string family,
            string kind,
            string path,
            string source = "human_authored",
            bool isAiGenerated = false,
            string importPreset = "2d_pixel_art",
            bool isFallback = false)
        {
            Id = id ?? string.Empty;
            Family = family ?? string.Empty;
            Kind = kind ?? string.Empty;
            Path = path ?? string.Empty;
            Source = source ?? string.Empty;
            IsAiGenerated = isAiGenerated;
            ImportPreset = importPreset ?? string.Empty;
            IsFallback = isFallback;
        }
    }

    /// <summary>
    /// Result of an asset resolution attempt with provenance and status.
    /// </summary>
    public readonly struct AssetResolveResult
    {
        public string RequestedId { get; }
        public string Family { get; }
        public AssetResolveStatus Status { get; }
        public string ResolvedPath { get; }
        public bool IsFallback => Status == AssetResolveStatus.FallbackUsed;
        public AssetManifestEntry? Entry { get; }

        public AssetResolveResult(
            string requestedId,
            string family,
            AssetResolveStatus status,
            string resolvedPath,
            AssetManifestEntry? entry = null)
        {
            RequestedId = requestedId ?? string.Empty;
            Family = family ?? string.Empty;
            Status = status;
            ResolvedPath = resolvedPath ?? string.Empty;
            Entry = entry;
        }

        /// <summary>
        /// In strict mode, only explicit or convention loads pass. Fallbacks fail strict mode.
        /// In standard mode, anything non-missing passes.
        /// </summary>
        public bool IsSuccess(bool strictMode)
        {
            if (strictMode)
            {
                return Status == AssetResolveStatus.LoadedExplicit || Status == AssetResolveStatus.LoadedByConvention;
            }
            return Status != AssetResolveStatus.Missing;
        }
    }

    /// <summary>
    /// Statistics for a single asset family in a coverage audit.
    /// </summary>
    public sealed class FamilyCoverageStats
    {
        public string Family { get; set; } = string.Empty;
        public int TotalRequested { get; set; }
        public int ExplicitLoaded { get; set; }
        public int ConventionLoaded { get; set; }
        public int FallbackUsed { get; set; }
        public int Missing { get; set; }

        public double CoveragePercentage => TotalRequested > 0
            ? Math.Round(((double)(ExplicitLoaded + ConventionLoaded) / TotalRequested) * 100.0, 2)
            : 0.0;
    }

    /// <summary>
    /// Audit report recording asset truth coverage across all requested IDs and families.
    /// </summary>
    public sealed class AssetCoverageReport
    {
        public int TotalRequested { get; set; }
        public int ExplicitLoadedCount { get; set; }
        public int ConventionLoadedCount { get; set; }
        public int FallbackCount { get; set; }
        public int MissingCount { get; set; }

        public double CoveragePercentage => TotalRequested > 0
            ? Math.Round(((double)(ExplicitLoadedCount + ConventionLoadedCount) / TotalRequested) * 100.0, 2)
            : 0.0;

        /// <summary>
        /// Strict pass criteria: 0 fallbacks and 0 missing assets.
        /// </summary>
        public bool IsCleanStrict => FallbackCount == 0 && MissingCount == 0;

        public Dictionary<string, FamilyCoverageStats> ByFamily { get; } =
            new Dictionary<string, FamilyCoverageStats>(StringComparer.OrdinalIgnoreCase);

        public List<AssetResolveResult> Results { get; } = new List<AssetResolveResult>();
    }

    /// <summary>
    /// Authoritative Asset Manifest Catalog holding registered entries loaded from JSON or code.
    /// </summary>
    public sealed class AssetManifestCatalog
    {
        private readonly Dictionary<string, AssetManifestEntry> _entries =
            new Dictionary<string, AssetManifestEntry>(StringComparer.OrdinalIgnoreCase);

        [JsonPropertyName("schema_version")]
        public int SchemaVersion { get; set; } = 1;

        [JsonPropertyName("total_assets")]
        public int TotalAssets => _entries.Count;

        [JsonPropertyName("assets")]
        public List<AssetManifestEntry> Assets
        {
            get => new List<AssetManifestEntry>(_entries.Values);
            set
            {
                _entries.Clear();
                if (value != null)
                {
                    foreach (var entry in value)
                    {
                        if (entry != null && !string.IsNullOrWhiteSpace(entry.Id))
                        {
                            _entries[entry.Id] = entry;
                        }
                    }
                }
            }
        }

        public int Count => _entries.Count;

        public void RegisterEntry(AssetManifestEntry entry)
        {
            if (entry == null || string.IsNullOrWhiteSpace(entry.Id))
                throw new ArgumentException("Asset entry must have a valid ID.", nameof(entry));

            _entries[entry.Id] = entry;
        }

        public bool TryGetEntry(string id, out AssetManifestEntry? entry)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                entry = null;
                return false;
            }
            return _entries.TryGetValue(id, out entry);
        }

        public IReadOnlyList<AssetManifestEntry> GetByFamily(string family)
        {
            var list = new List<AssetManifestEntry>();
            foreach (var entry in _entries.Values)
            {
                if (string.Equals(entry.Family, family, StringComparison.OrdinalIgnoreCase))
                {
                    list.Add(entry);
                }
            }
            return list;
        }

        public static AssetManifestCatalog ParseJson(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return new AssetManifestCatalog();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true
            };

            var parsed = JsonSerializer.Deserialize<AssetManifestCatalog>(json, options);
            return parsed ?? new AssetManifestCatalog();
        }
    }

    /// <summary>
    /// Deterministic Asset Manifest Resolver adhering to Plan 50 precedence:
    /// 1. Explicit Manifest Lookup
    /// 2. Stem Convention Candidates
    /// 3. Family Fallback
    /// 4. Missing
    /// </summary>
    public sealed class AssetManifestResolver
    {
        private readonly AssetManifestCatalog _catalog;

        // Distinct scratch fallback paths per family
        private readonly Dictionary<string, string> _familyFallbacks =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "portrait",  "assets/sprites/Characters/placeholder_survivor.png" },
                { "character", "assets/sprites/Characters/placeholder_survivor.png" },
                { "survivor",  "assets/sprites/Characters/placeholder_survivor.png" },
                { "item",      "assets/ui/Icons/icon_placeholder.png" },
                { "icon",      "assets/ui/Icons/icon_placeholder.png" },
                { "location",  "assets/ui/Icons/icon_placeholder.png" },
                { "weather",   "assets/ui/Icons/icon_placeholder.png" },
                { "faction",   "assets/ui/Icons/icon_placeholder.png" }
            };

        // Convention templates per family
        private readonly Dictionary<string, string[]> _familyConventionTemplates =
            new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase)
            {
                {
                    "item", new[]
                    {
                        "assets/art/{0}.jpg",
                        "assets/art/{0}.png",
                        "assets/sprites/Items/{0}.png",
                        "assets/sprites/items/{0}.png"
                    }
                },
                {
                    "portrait", new[]
                    {
                        "assets/art/{0}.jpg",
                        "assets/art/{0}.png",
                        "assets/sprites/Portraits/{0}.png",
                        "assets/sprites/portraits/{0}.png"
                    }
                },
                {
                    "location", new[]
                    {
                        "assets/art/{0}.jpg",
                        "assets/art/{0}.png",
                        "assets/sprites/Locations/{0}.png",
                        "assets/sprites/locations/{0}.png"
                    }
                },
                {
                    "faction", new[]
                    {
                        "assets/art/{0}.jpg",
                        "assets/art/{0}.png",
                        "assets/sprites/Factions/{0}.png",
                        "assets/sprites/factions/{0}.png"
                    }
                },
                {
                    "weather", new[]
                    {
                        "assets/art/{0}.jpg",
                        "assets/art/{0}.png",
                        "assets/sprites/Weather/{0}.png",
                        "assets/sprites/weather/{0}.png"
                    }
                }
            };

        /// <summary>Delegate seam fired whenever an asset is resolved.</summary>
        public Action<string, AssetResolveResult>? OnAssetResolvedSeam { get; set; }

        /// <summary>Delegate seam fired whenever a fallback is intercepted/used.</summary>
        public Action<string, string>? OnFallbackInterceptedSeam { get; set; }

        public AssetManifestResolver(AssetManifestCatalog? catalog = null)
        {
            _catalog = catalog ?? new AssetManifestCatalog();
        }

        public void SetFamilyFallback(string family, string path)
        {
            if (!string.IsNullOrWhiteSpace(family) && !string.IsNullOrWhiteSpace(path))
            {
                _familyFallbacks[family] = path;
            }
        }

        public string GetFamilyFallback(string family)
        {
            if (_familyFallbacks.TryGetValue(family, out var fb))
                return fb;
            return "assets/ui/Icons/icon_placeholder.png";
        }

        /// <summary>
        /// Resolves an asset ID with strict Plan 50 precedence:
        /// 1. Manifest explicit mapping
        /// 2. Stem convention candidates
        /// 3. Family fallback
        /// 4. Missing
        /// </summary>
        public AssetResolveResult Resolve(string id, string family, Func<string, bool>? pathExistsPredicate = null)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                var emptyResult = new AssetResolveResult(string.Empty, family, AssetResolveStatus.Missing, string.Empty);
                OnAssetResolvedSeam?.Invoke(string.Empty, emptyResult);
                return emptyResult;
            }

            // 1. Explicit Manifest Lookup
            if (_catalog.TryGetEntry(id, out var entry) && entry != null)
            {
                // If manifest entry points to a fallback or if predicate verifies it
                if (entry.IsFallback)
                {
                    var fbResult = new AssetResolveResult(id, family, AssetResolveStatus.FallbackUsed, entry.Path, entry);
                    OnFallbackInterceptedSeam?.Invoke(id, entry.Path);
                    OnAssetResolvedSeam?.Invoke(id, fbResult);
                    return fbResult;
                }

                if (pathExistsPredicate == null || pathExistsPredicate(entry.Path))
                {
                    var explicitResult = new AssetResolveResult(id, family, AssetResolveStatus.LoadedExplicit, entry.Path, entry);
                    OnAssetResolvedSeam?.Invoke(id, explicitResult);
                    return explicitResult;
                }
            }

            // 2. Convention candidate search
            if (_familyConventionTemplates.TryGetValue(family, out var templates))
            {
                foreach (var template in templates)
                {
                    string candidatePath = string.Format(template, id);
                    if (pathExistsPredicate != null && pathExistsPredicate(candidatePath))
                    {
                        var conventionResult = new AssetResolveResult(id, family, AssetResolveStatus.LoadedByConvention, candidatePath);
                        OnAssetResolvedSeam?.Invoke(id, conventionResult);
                        return conventionResult;
                    }
                }
            }

            // 3. Family Fallback
            if (_familyFallbacks.TryGetValue(family, out var fallbackPath))
            {
                var fallbackResult = new AssetResolveResult(id, family, AssetResolveStatus.FallbackUsed, fallbackPath);
                OnFallbackInterceptedSeam?.Invoke(id, fallbackPath);
                OnAssetResolvedSeam?.Invoke(id, fallbackResult);
                return fallbackResult;
            }

            // 4. Missing
            var missingResult = new AssetResolveResult(id, family, AssetResolveStatus.Missing, string.Empty);
            OnAssetResolvedSeam?.Invoke(id, missingResult);
            return missingResult;
        }

        /// <summary>
        /// Audits coverage across a collection of requested IDs and families.
        /// </summary>
        public AssetCoverageReport AuditCoverage(IEnumerable<(string id, string family)> requests, Func<string, bool>? pathExistsPredicate = null)
        {
            var report = new AssetCoverageReport();

            foreach (var (id, family) in requests)
            {
                report.TotalRequested++;
                var result = Resolve(id, family, pathExistsPredicate);
                report.Results.Add(result);

                if (!report.ByFamily.TryGetValue(family, out var stats))
                {
                    stats = new FamilyCoverageStats { Family = family };
                    report.ByFamily[family] = stats;
                }
                stats.TotalRequested++;

                switch (result.Status)
                {
                    case AssetResolveStatus.LoadedExplicit:
                        report.ExplicitLoadedCount++;
                        stats.ExplicitLoaded++;
                        break;
                    case AssetResolveStatus.LoadedByConvention:
                        report.ConventionLoadedCount++;
                        stats.ConventionLoaded++;
                        break;
                    case AssetResolveStatus.FallbackUsed:
                        report.FallbackCount++;
                        stats.FallbackUsed++;
                        break;
                    case AssetResolveStatus.Missing:
                        report.MissingCount++;
                        stats.Missing++;
                        break;
                }
            }

            return report;
        }
    }
}
