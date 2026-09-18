// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Ashfall.Core.Content;

namespace Ashfall.Core.Mods
{
    /// <summary>
    /// User-installed JSON mod manifest. Mods may replace or extend existing
    /// catalog definitions, but they may not add executable code or arbitrary
    /// files to the data authority.
    /// </summary>
    [Serializable]
    public sealed class ModManifest
    {
        [JsonPropertyName("schema_version")]
        public int SchemaVersion { get; set; } = 1;

        [JsonPropertyName("mod_id")]
        public string ModId { get; set; } = string.Empty;

        [JsonPropertyName("id")]
        public string? IdAlias { get; set; }

        [JsonPropertyName("display_name")]
        public string DisplayName { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string? NameAlias { get; set; }

        [JsonPropertyName("version")]
        public string Version { get; set; } = "1.0.0";

        [JsonPropertyName("load_order")]
        public int LoadOrder { get; set; }

        [JsonPropertyName("allow_overrides")]
        public bool AllowOverrides { get; set; }

        [JsonPropertyName("catalogs")]
        public List<string> Catalogs { get; set; } = new List<string>();

        [JsonPropertyName("content_roots")]
        public List<string>? ContentRootsAlias { get; set; }

        [JsonPropertyName("game_range")]
        public string? GameRange { get; set; }

        [JsonPropertyName("supported_game_range")]
        public string? SupportedGameRangeAlias { get; set; }

        [JsonPropertyName("mod_contract_range")]
        public string? ModContractRange { get; set; }

        [JsonPropertyName("supported_mod_contract_range")]
        public string? SupportedModContractRangeAlias { get; set; }

        [JsonPropertyName("pack_type")]
        public string PackType { get; set; } = "mod";

        [JsonPropertyName("dependencies")]
        public List<string> Dependencies { get; set; } = new List<string>();

        [JsonIgnore]
        public string EffectiveModId => !string.IsNullOrWhiteSpace(ModId) ? ModId : (IdAlias ?? string.Empty);

        [JsonIgnore]
        public string EffectiveDisplayName => !string.IsNullOrWhiteSpace(DisplayName) ? DisplayName : (NameAlias ?? string.Empty);

        [JsonIgnore]
        public string? EffectiveGameRange => !string.IsNullOrWhiteSpace(GameRange) ? GameRange : SupportedGameRangeAlias;

        [JsonIgnore]
        public string? EffectiveModContractRange => !string.IsNullOrWhiteSpace(ModContractRange) ? ModContractRange : SupportedModContractRangeAlias;

        [JsonIgnore]
        public IReadOnlyList<string> EffectiveCatalogs => Catalogs != null && Catalogs.Count > 0 ? Catalogs : (ContentRootsAlias ?? (IReadOnlyList<string>)Array.Empty<string>());
    }

    public enum ModRejectionCode
    {
        MissingManifest,
        InvalidManifestJson,
        UnsafeModId,
        DuplicateModId,
        UnsupportedSchemaVersion,
        NoCatalogs,
        IncompatibleGameVersion,
        IncompatibleModContract,
        MalformedVersionRange,
        MissingDependency,
        CircularDependency,
        UnsafeCatalogPath,
        MissingCatalogFile,
        DuplicateDefinitionId,
        DisallowedPrefix,
        OverrideNotAllowed,
        MalformedCatalogJson,
        AcceptancePipelineFailed
    }

    public sealed class ModDiagnostic
    {
        public string ModId { get; }
        public string Message { get; }
        public ModRejectionCode Code { get; }

        public ModDiagnostic(string modId, string message, ModRejectionCode code = ModRejectionCode.InvalidManifestJson)
        {
            ModId = modId;
            Message = message;
            Code = code;
        }

        public override string ToString() => $"{ModId}: {Message}";
    }

    /// <summary>
    /// Deterministic result of validating and layering installed mods.
    /// Rejected mods are isolated; a valid mod never depends on a rejected one.
    /// </summary>
    public sealed class ModLayerResult
    {
        public IReadOnlyDictionary<string, string> CatalogOverlays { get; }
        public IReadOnlyList<string> AcceptedModIds { get; }
        public IReadOnlyList<string> RejectedModIds { get; }
        public IReadOnlyList<ModDiagnostic> Diagnostics { get; }
        public bool Success => Diagnostics.Count == 0 || AcceptedModIds.Count > 0;

        internal ModLayerResult(
            IDictionary<string, string> catalogOverlays,
            IList<string> acceptedModIds,
            IList<string> rejectedModIds,
            IList<ModDiagnostic> diagnostics)
        {
            CatalogOverlays = new Dictionary<string, string>(catalogOverlays, StringComparer.Ordinal);
            AcceptedModIds = new List<string>(acceptedModIds);
            RejectedModIds = new List<string>(rejectedModIds);
            Diagnostics = new List<ModDiagnostic>(diagnostics);
        }
    }

    /// <summary>
    /// Validates manifests and produces virtual catalog overlays. It does not
    /// write files or mutate the base data directory. A host can commit the
    /// returned overlays to an isolated staging directory after all validation
    /// and catalog-integrity checks pass.
    /// </summary>
    public sealed class JsonModLayering
    {
        private static readonly HashSet<string> ReservedManifestNames =
            new(StringComparer.OrdinalIgnoreCase) { "manifest.json" };

        public ModLayerResult Build(
            string baseDataDirectory,
            string modsDirectory,
            IFileIO files,
            IJsonSerializer json,
            ISet<string>? enabledModIds = null,
            bool validatePackAcceptance = false,
            Func<string, string, bool>? customPackAcceptanceValidator = null,
            string currentGameVersion = ModCompatibilityEvaluator.DefaultGameVersion,
            int currentContractVersion = ModCompatibilityEvaluator.DefaultModContractVersion)
        {
            if (files == null) throw new ArgumentNullException(nameof(files));
            if (json == null) throw new ArgumentNullException(nameof(json));

            var overlays = new SortedDictionary<string, string>(StringComparer.Ordinal);
            var accepted = new List<string>();
            var rejected = new List<string>();
            var diagnostics = new List<ModDiagnostic>();

            if (string.IsNullOrWhiteSpace(modsDirectory) || !files.DirectoryExists(modsDirectory))
            {
                return new ModLayerResult(overlays, accepted, rejected, diagnostics);
            }

            var candidateManifests = new Dictionary<string, (ModManifest Manifest, string Directory)>(StringComparer.Ordinal);
            var seenIds = new HashSet<string>(StringComparer.Ordinal);

            // Phase 1: Enumerate and parse manifests in deterministic filesystem path order
            foreach (string modDirectory in files.GetDirectories(modsDirectory).OrderBy(p => p, StringComparer.Ordinal))
            {
                string manifestPath = files.Combine(modDirectory, "manifest.json");
                string directoryName = files.GetFileName(modDirectory);
                if (!files.FileExists(manifestPath))
                {
                    rejected.Add(directoryName);
                    diagnostics.Add(new ModDiagnostic(directoryName, "manifest.json is missing", ModRejectionCode.MissingManifest));
                    continue;
                }

                ModManifest? manifest;
                try
                {
                    manifest = json.Deserialize<ModManifest>(files.ReadAllText(manifestPath));
                }
                catch (Exception ex)
                {
                    rejected.Add(directoryName);
                    diagnostics.Add(new ModDiagnostic(directoryName, "manifest JSON is invalid: " + ex.Message, ModRejectionCode.InvalidManifestJson));
                    continue;
                }

                string modId = manifest?.EffectiveModId?.Trim() ?? string.Empty;
                if (!IsSafeModId(modId))
                {
                    rejected.Add(string.IsNullOrEmpty(modId) ? directoryName : modId);
                    diagnostics.Add(new ModDiagnostic(ModIdOrDirectory(modId, directoryName),
                        "mod_id must be lowercase snake_case and path-safe", ModRejectionCode.UnsafeModId));
                    continue;
                }
                if (!seenIds.Add(modId))
                {
                    rejected.Add(modId);
                    diagnostics.Add(new ModDiagnostic(modId, "duplicate mod_id", ModRejectionCode.DuplicateModId));
                    continue;
                }
                if (manifest == null || manifest.SchemaVersion != 1)
                {
                    rejected.Add(modId);
                    diagnostics.Add(new ModDiagnostic(modId, "unsupported manifest schema_version", ModRejectionCode.UnsupportedSchemaVersion));
                    continue;
                }
                if (enabledModIds != null && enabledModIds.Count > 0 && !enabledModIds.Contains(modId))
                    continue;
                if (manifest.EffectiveCatalogs == null || manifest.EffectiveCatalogs.Count == 0)
                {
                    rejected.Add(modId);
                    diagnostics.Add(new ModDiagnostic(modId, "catalogs must contain at least one file", ModRejectionCode.NoCatalogs));
                    continue;
                }

                // Phase 2: Compatibility Governance Evaluation
                if (!ModCompatibilityEvaluator.EvaluateGameVersion(currentGameVersion, manifest.EffectiveGameRange, out string? gameError))
                {
                    rejected.Add(modId);
                    if (gameError != null && gameError.StartsWith("malformed", StringComparison.Ordinal))
                    {
                        diagnostics.Add(new ModDiagnostic(modId, gameError, ModRejectionCode.MalformedVersionRange));
                    }
                    else
                    {
                        diagnostics.Add(new ModDiagnostic(modId,
                            $"incompatible game version: current '{currentGameVersion}' does not satisfy '{manifest.EffectiveGameRange}'",
                            ModRejectionCode.IncompatibleGameVersion));
                    }
                    continue;
                }

                if (!ModCompatibilityEvaluator.EvaluateModContractVersion(currentContractVersion, manifest.EffectiveModContractRange, out string? contractError))
                {
                    rejected.Add(modId);
                    if (contractError != null && contractError.StartsWith("malformed", StringComparison.Ordinal))
                    {
                        diagnostics.Add(new ModDiagnostic(modId, contractError, ModRejectionCode.MalformedVersionRange));
                    }
                    else
                    {
                        diagnostics.Add(new ModDiagnostic(modId,
                            $"incompatible mod contract version: current '{currentContractVersion}' does not satisfy '{manifest.EffectiveModContractRange}'",
                            ModRejectionCode.IncompatibleModContract));
                    }
                    continue;
                }

                candidateManifests[modId] = (manifest, modDirectory);
            }

            // Phase 3: Dependency Resolution & Cycle Detection
            bool dependencyRemoved;
            do
            {
                dependencyRemoved = false;
                foreach (var kvp in candidateManifests.ToList())
                {
                    string modId = kvp.Key;
                    var manifest = kvp.Value.Manifest;
                    if (manifest.Dependencies == null || manifest.Dependencies.Count == 0)
                        continue;

                    string? missingDep = manifest.Dependencies.FirstOrDefault(dep => !candidateManifests.ContainsKey(dep));
                    if (missingDep != null)
                    {
                        candidateManifests.Remove(modId);
                        rejected.Add(modId);
                        diagnostics.Add(new ModDiagnostic(modId, $"missing required dependency '{missingDep}'", ModRejectionCode.MissingDependency));
                        dependencyRemoved = true;
                    }
                }
            } while (dependencyRemoved);

            // Topological sort with deterministic tie-breaking (LoadOrder ascending, ModId ordinal ascending)
            var inDegree = new Dictionary<string, int>(StringComparer.Ordinal);
            var dependents = new Dictionary<string, List<string>>(StringComparer.Ordinal);
            foreach (var kvp in candidateManifests)
            {
                inDegree[kvp.Key] = 0;
                dependents[kvp.Key] = new List<string>();
            }

            foreach (var kvp in candidateManifests)
            {
                string modId = kvp.Key;
                if (kvp.Value.Manifest.Dependencies != null)
                {
                    foreach (string dep in kvp.Value.Manifest.Dependencies)
                    {
                        if (candidateManifests.ContainsKey(dep))
                        {
                            dependents[dep].Add(modId);
                            inDegree[modId]++;
                        }
                    }
                }
            }

            var orderedManifests = new List<(ModManifest Manifest, string Directory)>();
            var readyQueue = new SortedSet<string>(Comparer<string>.Create((a, b) =>
            {
                var entryA = candidateManifests[a].Manifest;
                var entryB = candidateManifests[b].Manifest;
                int cmp = entryA.LoadOrder.CompareTo(entryB.LoadOrder);
                return cmp != 0 ? cmp : string.CompareOrdinal(a, b);
            }));

            foreach (var kvp in inDegree)
            {
                if (kvp.Value == 0)
                    readyQueue.Add(kvp.Key);
            }

            while (readyQueue.Count > 0)
            {
                string currentId = readyQueue.Min!;
                readyQueue.Remove(currentId);
                orderedManifests.Add(candidateManifests[currentId]);

                foreach (string dependent in dependents[currentId])
                {
                    inDegree[dependent]--;
                    if (inDegree[dependent] == 0)
                        readyQueue.Add(dependent);
                }
            }

            // Any candidates with inDegree > 0 participate in a cycle
            if (orderedManifests.Count < candidateManifests.Count)
            {
                foreach (var kvp in inDegree.OrderBy(p => p.Key, StringComparer.Ordinal))
                {
                    if (kvp.Value > 0)
                    {
                        rejected.Add(kvp.Key);
                        diagnostics.Add(new ModDiagnostic(kvp.Key, "circular dependency detected", ModRejectionCode.CircularDependency));
                    }
                }
            }

            // Phase 4: Deterministic Overlay Merge & Content Acceptance
            foreach (var entry in orderedManifests)
            {
                string modId = entry.Manifest.EffectiveModId;
                var candidate = new SortedDictionary<string, string>(StringComparer.Ordinal);
                bool valid = true;

                foreach (string catalog in entry.Manifest.EffectiveCatalogs
                    .OrderBy(value => value, StringComparer.Ordinal))
                {
                    if (!IsSafeCatalogFileName(catalog))
                    {
                        diagnostics.Add(new ModDiagnostic(modId, $"catalog path is not allowed: {catalog}", ModRejectionCode.UnsafeCatalogPath));
                        valid = false;
                        break;
                    }

                    string basePath = files.Combine(baseDataDirectory, catalog);
                    string modPath = files.Combine(entry.Directory, catalog);
                    if (!files.FileExists(basePath) || !files.FileExists(modPath))
                    {
                        diagnostics.Add(new ModDiagnostic(modId, $"catalog does not exist in both base and mod: {catalog}", ModRejectionCode.MissingCatalogFile));
                        valid = false;
                        break;
                    }

                    string current = candidate.TryGetValue(catalog, out var staged)
                        ? staged
                        : overlays.TryGetValue(catalog, out var layered)
                            ? layered
                            : files.ReadAllText(basePath);

                    if (!TryMergeCatalog(current, files.ReadAllText(modPath), entry.Manifest.AllowOverrides,
                        out string merged, out string error, out ModRejectionCode mergeCode))
                    {
                        diagnostics.Add(new ModDiagnostic(modId, $"{catalog}: {error}", mergeCode));
                        valid = false;
                        break;
                    }

                    // Content Acceptance Pipeline integration
                    if ((validatePackAcceptance || customPackAcceptanceValidator != null) &&
                        string.Equals(entry.Manifest.PackType, "content_pack", StringComparison.OrdinalIgnoreCase))
                    {
                        if (customPackAcceptanceValidator != null && !customPackAcceptanceValidator(catalog, merged))
                        {
                            diagnostics.Add(new ModDiagnostic(modId,
                                $"{catalog} failed custom acceptance validator",
                                ModRejectionCode.AcceptancePipelineFailed));
                            valid = false;
                            break;
                        }

                        if (validatePackAcceptance)
                        {
                            var catalogEntry = new CatalogEntry
                            {
                                Path = catalog,
                                Loader = "JsonModLayering",
                                DefinitionCount = 1,
                                Classification = ContentClassification.GAMEPLAY_CONSUMED,
                                RequiredRung = ContentAcceptanceRung.LOADED
                            };
                            var pipelineResult = ContentAcceptancePipeline.Evaluate(catalogEntry);
                            if (!pipelineResult.IsSuccess)
                            {
                                diagnostics.Add(new ModDiagnostic(modId,
                                    $"{catalog} failed acceptance pipeline [{pipelineResult.FailedRung}]: {pipelineResult.Summary}",
                                    ModRejectionCode.AcceptancePipelineFailed));
                                valid = false;
                                break;
                            }
                        }
                    }

                    candidate[catalog] = merged;
                }

                if (!valid)
                {
                    rejected.Add(modId);
                    continue;
                }

                foreach (var pair in candidate)
                    overlays[pair.Key] = pair.Value;
                accepted.Add(modId);
            }

            return new ModLayerResult(overlays, accepted, rejected, diagnostics);
        }

        public static bool IsSafeModId(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || value.Length > 48) return false;
            if (value[0] == '_' || value[^1] == '_') return false;
            return value.All(c => c == '_' || c == '-' || c >= 'a' && c <= 'z' || c >= '0' && c <= '9');
        }

        public static bool IsSafeCatalogFileName(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || ReservedManifestNames.Contains(value))
                return false;
            if (!value.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
                return false;
            if (value.Contains('/') || value.Contains('\\') || value.Contains("..", StringComparison.Ordinal))
                return false;
            return value.All(c => c == '_' || c == '-' || c == '.' || c >= 'a' && c <= 'z' || c >= 'A' && c <= 'Z' || c >= '0' && c <= '9');
        }

        private static bool TryMergeCatalog(
            string baseText,
            string modText,
            bool allowOverrides,
            out string mergedText,
            out string error,
            out ModRejectionCode rejectionCode)
        {
            mergedText = string.Empty;
            error = string.Empty;
            rejectionCode = ModRejectionCode.MalformedCatalogJson;
            JsonNode? baseRoot;
            JsonNode? modRoot;
            try
            {
                baseRoot = JsonNode.Parse(baseText);
                modRoot = JsonNode.Parse(modText);
            }
            catch (Exception ex)
            {
                error = "invalid JSON: " + ex.Message;
                rejectionCode = ModRejectionCode.MalformedCatalogJson;
                return false;
            }

            if (baseRoot is not JsonObject baseObject || modRoot is not JsonObject modObject)
            {
                error = "catalog roots must be objects with schema_version";
                rejectionCode = ModRejectionCode.MalformedCatalogJson;
                return false;
            }

            int? baseSchema = ReadSchemaVersion(baseObject);
            int? modSchema = ReadSchemaVersion(modObject);
            if (baseSchema == null || modSchema == null || baseSchema != modSchema)
            {
                error = "base and mod schema_version must be present and equal";
                rejectionCode = ModRejectionCode.UnsupportedSchemaVersion;
                return false;
            }

            string? arrayName = FindDefinitionArrayName(baseObject);
            string? modArrayName = FindDefinitionArrayName(modObject);
            if (arrayName == null || !string.Equals(arrayName, modArrayName, StringComparison.Ordinal))
            {
                error = "mod must target the catalog's single definition array";
                rejectionCode = ModRejectionCode.MalformedCatalogJson;
                return false;
            }

            var baseArray = baseObject[arrayName] as JsonArray;
            var modArray = modObject[modArrayName!] as JsonArray;
            if (baseArray == null || modArray == null)
            {
                error = "definition array is missing";
                rejectionCode = ModRejectionCode.MalformedCatalogJson;
                return false;
            }

            var positions = new Dictionary<string, int>(StringComparer.Ordinal);
            for (int i = 0; i < baseArray.Count; i++)
            {
                if (!TryGetDefinitionId(baseArray[i], out string id))
                {
                    error = $"base definition at index {i} has no supported id";
                    rejectionCode = ModRejectionCode.MalformedCatalogJson;
                    return false;
                }
                if (!positions.TryAdd(id, i))
                {
                    error = $"base catalog contains duplicate id '{id}'";
                    rejectionCode = ModRejectionCode.DuplicateDefinitionId;
                    return false;
                }
            }

            var patchIds = new HashSet<string>(StringComparer.Ordinal);
            var patches = new List<(string Id, JsonNode Node)>();
            for (int i = 0; i < modArray.Count; i++)
            {
                JsonNode? node = modArray[i];
                if (!TryGetDefinitionId(node, out string id))
                {
                    error = $"mod definition at index {i} has no supported id";
                    rejectionCode = ModRejectionCode.MalformedCatalogJson;
                    return false;
                }
                if (!patchIds.Add(id))
                {
                    error = $"mod catalog contains duplicate id '{id}'";
                    rejectionCode = ModRejectionCode.DuplicateDefinitionId;
                    return false;
                }
                if (!HasAllowedPrefix(id))
                {
                    error = $"id '{id}' uses a prefix outside the catalog whitelist";
                    rejectionCode = ModRejectionCode.DisallowedPrefix;
                    return false;
                }
                if (positions.ContainsKey(id) && !allowOverrides)
                {
                    error = $"id '{id}' already exists; set allow_overrides=true to replace it";
                    rejectionCode = ModRejectionCode.OverrideNotAllowed;
                    return false;
                }
                patches.Add((id, node!));
            }

            foreach (var patch in patches.OrderBy(item => item.Id, StringComparer.Ordinal))
            {
                JsonNode clone = patch.Node.DeepClone();
                if (positions.TryGetValue(patch.Id, out int index))
                    baseArray[index] = clone;
                else
                {
                    positions[patch.Id] = baseArray.Count;
                    baseArray.Add(clone);
                }
            }

            mergedText = baseRoot.ToJsonString(new JsonSerializerOptions { WriteIndented = false });
            return true;
        }

        private static int? ReadSchemaVersion(JsonObject root)
        {
            try
            {
                return root["schema_version"]?.GetValue<int>();
            }
            catch
            {
                return null;
            }
        }

        private static string? FindDefinitionArrayName(JsonObject root)
        {
            string? found = null;
            foreach (var pair in root)
            {
                if (pair.Value is not JsonArray) continue;
                if (found != null) return null;
                found = pair.Key;
            }
            return found;
        }

        private static bool TryGetDefinitionId(JsonNode? node, out string id)
        {
            id = string.Empty;
            if (node is not JsonObject obj) return false;
            foreach (string key in CatalogIntegrityRules.DefinitionKeys)
            {
                if (obj[key] is JsonValue value && value.TryGetValue<string>(out string? candidate)
                    && !string.IsNullOrWhiteSpace(candidate))
                {
                    id = candidate;
                    return true;
                }
            }
            return false;
        }

        private static bool HasAllowedPrefix(string id)
            => CatalogIntegrityRules.IdPrefixes.Any(prefix =>
                id.StartsWith(prefix, StringComparison.Ordinal));

        private static string ModIdOrDirectory(string modId, string directoryName)
            => string.IsNullOrWhiteSpace(modId) ? directoryName : modId;
    }
}
