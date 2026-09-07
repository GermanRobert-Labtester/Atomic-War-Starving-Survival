using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

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

        [JsonPropertyName("display_name")]
        public string DisplayName { get; set; } = string.Empty;

        [JsonPropertyName("version")]
        public string Version { get; set; } = "1.0.0";

        [JsonPropertyName("load_order")]
        public int LoadOrder { get; set; }

        [JsonPropertyName("allow_overrides")]
        public bool AllowOverrides { get; set; }

        [JsonPropertyName("catalogs")]
        public List<string> Catalogs { get; set; } = new List<string>();
    }

    public sealed class ModDiagnostic
    {
        public string ModId { get; }
        public string Message { get; }

        public ModDiagnostic(string modId, string message)
        {
            ModId = modId;
            Message = message;
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
        private static readonly JsonSerializerOptions ManifestOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            IncludeFields = true
        };

        private static readonly HashSet<string> ReservedManifestNames =
            new(StringComparer.OrdinalIgnoreCase) { "manifest.json" };

        public ModLayerResult Build(
            string baseDataDirectory,
            string modsDirectory,
            IFileIO files,
            IJsonSerializer json,
            ISet<string>? enabledModIds = null)
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

            var manifests = new List<(ModManifest Manifest, string Directory)>();
            var seenIds = new HashSet<string>(StringComparer.Ordinal);
            foreach (string modDirectory in files.GetDirectories(modsDirectory).OrderBy(p => p, StringComparer.Ordinal))
            {
                string manifestPath = files.Combine(modDirectory, "manifest.json");
                string directoryName = files.GetFileName(modDirectory);
                if (!files.FileExists(manifestPath))
                {
                    rejected.Add(directoryName);
                    diagnostics.Add(new ModDiagnostic(directoryName, "manifest.json is missing"));
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
                    diagnostics.Add(new ModDiagnostic(directoryName, "manifest JSON is invalid: " + ex.Message));
                    continue;
                }

                string modId = manifest?.ModId?.Trim() ?? string.Empty;
                if (!IsSafeModId(modId))
                {
                    rejected.Add(string.IsNullOrEmpty(modId) ? directoryName : modId);
                    diagnostics.Add(new ModDiagnostic(modIdOrDirectory(modId, directoryName),
                        "mod_id must be lowercase snake_case and path-safe"));
                    continue;
                }
                if (!seenIds.Add(modId))
                {
                    rejected.Add(modId);
                    diagnostics.Add(new ModDiagnostic(modId, "duplicate mod_id"));
                    continue;
                }
                if (manifest == null || manifest.SchemaVersion != 1)
                {
                    rejected.Add(modId);
                    diagnostics.Add(new ModDiagnostic(modId, "unsupported manifest schema_version"));
                    continue;
                }
                if (enabledModIds != null && enabledModIds.Count > 0 && !enabledModIds.Contains(modId))
                    continue;
                if (manifest.Catalogs == null || manifest.Catalogs.Count == 0)
                {
                    rejected.Add(modId);
                    diagnostics.Add(new ModDiagnostic(modId, "catalogs must contain at least one file"));
                    continue;
                }

                manifests.Add((manifest, modDirectory));
            }

            foreach (var entry in manifests
                .OrderBy(item => item.Manifest.LoadOrder)
                .ThenBy(item => item.Manifest.ModId, StringComparer.Ordinal))
            {
                string modId = entry.Manifest.ModId;
                var candidate = new SortedDictionary<string, string>(StringComparer.Ordinal);
                bool valid = true;
                foreach (string catalog in entry.Manifest.Catalogs
                    .OrderBy(value => value, StringComparer.Ordinal))
                {
                    if (!IsSafeCatalogFileName(catalog))
                    {
                        diagnostics.Add(new ModDiagnostic(modId, $"catalog path is not allowed: {catalog}"));
                        valid = false;
                        continue;
                    }

                    string basePath = files.Combine(baseDataDirectory, catalog);
                    string modPath = files.Combine(entry.Directory, catalog);
                    if (!files.FileExists(basePath) || !files.FileExists(modPath))
                    {
                        diagnostics.Add(new ModDiagnostic(modId, $"catalog does not exist in both base and mod: {catalog}"));
                        valid = false;
                        continue;
                    }

                    string current = candidate.TryGetValue(catalog, out var staged)
                        ? staged
                        : overlays.TryGetValue(catalog, out var layered)
                            ? layered
                            : files.ReadAllText(basePath);
                    if (!TryMergeCatalog(current, files.ReadAllText(modPath), entry.Manifest.AllowOverrides,
                        out string merged, out string error))
                    {
                        diagnostics.Add(new ModDiagnostic(modId, $"{catalog}: {error}"));
                        valid = false;
                        continue;
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
            out string error)
        {
            mergedText = string.Empty;
            error = string.Empty;
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
                return false;
            }

            if (baseRoot is not JsonObject baseObject || modRoot is not JsonObject modObject)
            {
                error = "catalog roots must be objects with schema_version";
                return false;
            }

            int? baseSchema = ReadSchemaVersion(baseObject);
            int? modSchema = ReadSchemaVersion(modObject);
            if (baseSchema == null || modSchema == null || baseSchema != modSchema)
            {
                error = "base and mod schema_version must be present and equal";
                return false;
            }

            string? arrayName = FindDefinitionArrayName(baseObject);
            string? modArrayName = FindDefinitionArrayName(modObject);
            if (arrayName == null || !string.Equals(arrayName, modArrayName, StringComparison.Ordinal))
            {
                error = "mod must target the catalog's single definition array";
                return false;
            }

            var baseArray = baseObject[arrayName] as JsonArray;
            var modArray = modObject[modArrayName!] as JsonArray;
            if (baseArray == null || modArray == null)
            {
                error = "definition array is missing";
                return false;
            }

            var positions = new Dictionary<string, int>(StringComparer.Ordinal);
            for (int i = 0; i < baseArray.Count; i++)
            {
                if (!TryGetDefinitionId(baseArray[i], out string id))
                {
                    error = $"base definition at index {i} has no supported id";
                    return false;
                }
                if (!positions.TryAdd(id, i))
                {
                    error = $"base catalog contains duplicate id '{id}'";
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
                    return false;
                }
                if (!patchIds.Add(id))
                {
                    error = $"mod catalog contains duplicate id '{id}'";
                    return false;
                }
                if (!HasAllowedPrefix(id))
                {
                    error = $"id '{id}' uses a prefix outside the catalog whitelist";
                    return false;
                }
                if (positions.ContainsKey(id) && !allowOverrides)
                {
                    error = $"id '{id}' already exists; set allow_overrides=true to replace it";
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

        private static string modIdOrDirectory(string modId, string directoryName)
            => string.IsNullOrWhiteSpace(modId) ? directoryName : modId;
    }
}
