// SPDX-License-Identifier: MIT
// ASHFALL Core: Catalog key normalization for the snake_case migration
// (Plans 47+ / Wave 1).
//
// Strategy (per docs/data/SNAKE_CASE_MIGRATION.md):
//   Canonical DTO/property mapping is snake_case; legacy camelCase wire keys
//   remain accepted through a pre-deserialization key normalization pass.
//   - Both spellings with identical values  → accepted, legacy key dropped.
//   - Both spellings with differing values  → hard ambiguity error (never an
//     arbitrary winner).
//
// This is a spelling migration only: ID values, schema versions and value
// semantics are never modified.

using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Ashfall.Core.IO
{
    /// <summary>
    /// Raised when a catalog contains both the legacy and canonical spelling of a
    /// property with conflicting values. The ambiguity must fail loudly rather
    /// than silently pick a winner.
    /// </summary>
    public sealed class CatalogKeyConflictException : Exception
    {
        public string SourceName { get; }
        public string JsonPath { get; }
        public string LegacyKey { get; }
        public string CanonicalKey { get; }

        public CatalogKeyConflictException(
            string sourceName, string jsonPath, string legacyKey, string canonicalKey, string detail)
            : base($"{sourceName}: ambiguous dual key at {jsonPath}: '{legacyKey}' and '{canonicalKey}' both present with conflicting values. {detail}")
        {
            SourceName = sourceName;
            JsonPath = jsonPath;
            LegacyKey = legacyKey;
            CanonicalKey = canonicalKey;
        }
    }

    /// <summary>
    /// Pre-deserialization key normalization for catalogs migrating from
    /// camelCase wire keys to canonical snake_case (Plan 47 wave 1).
    /// Engine-agnostic: System.Text.Json only, no host references.
    /// </summary>
    public static class CatalogKeyNormalizer
    {
        /// <summary>
        /// Returns <paramref name="json"/> with every legacy key in
        /// <paramref name="aliasMap"/> renamed to its canonical snake_case
        /// spelling. Objects without any alias keys are returned untouched.
        /// Deterministic: keys keep their original relative order.
        /// </summary>
        /// <param name="json">Raw catalog text.</param>
        /// <param name="aliasMap">legacy key → canonical key (exact spelling).</param>
        /// <param name="sourceName">File name used in diagnostics.</param>
        public static string Normalize(string json, IReadOnlyDictionary<string, string> aliasMap, string sourceName)
        {
            if (string.IsNullOrEmpty(json) || aliasMap == null || aliasMap.Count == 0)
                return json;

            JsonNode? root;
            try
            {
                root = JsonNode.Parse(json);
            }
            catch (JsonException ex)
            {
                // Malformed input is passed through so the typed loader surfaces
                // its standard parse diagnostic; we still log context here so the
                // normalization seam is never a silent failure point.
                CatalogDiagnostics.Warn(sourceName, "CatalogKeyNormalizer pre-parse", ex);
                return json;
            }
            if (root == null)
                return json;

            bool changed = NormalizeNode(root, aliasMap, sourceName, "$");
            if (!changed)
                return json;

            return root.ToJsonString(new JsonSerializerOptions
            {
                WriteIndented = false
            });
        }

        private static bool NormalizeNode(
            JsonNode node,
            IReadOnlyDictionary<string, string> aliasMap,
            string sourceName,
            string path)
        {
            bool changed = false;

            if (node is JsonObject obj)
            {
                // Collect renames first so we rebuild at most once per object.
                string?[] keys = new string?[obj.Count];
                int i = 0;
                bool hasAliasKey = false;
                foreach (var (key, _) in obj)
                {
                    keys[i++] = key;
                    if (key != null && aliasMap.ContainsKey(key))
                        hasAliasKey = true;
                }

                if (hasAliasKey)
                {
                    // Rebuild preserving order; detect dual-key conflicts.
                    // Pair the final key with the ORIGINAL node (no clone here —
                    // nodes are unparented by obj.Clear() before re-adding).
                    var rebuiltKeys = new List<string>();
                    var rebuiltValues = new List<JsonNode?>();
                    // Final key → the legacy/canonical key it originated from.
                    var origin = new Dictionary<string, string>(StringComparer.Ordinal);

                    foreach (var key in keys)
                    {
                        if (key == null)
                            continue;
                        var value = obj[key];

                        if (aliasMap.TryGetValue(key, out var canonical))
                        {
                            int idx = rebuiltKeys.IndexOf(canonical);
                            if (idx >= 0)
                            {
                                if (CanonicalEquals(rebuiltValues[idx], value))
                                    continue; // same value — drop the legacy duplicate
                                throw new CatalogKeyConflictException(
                                    sourceName, path, key, origin[canonical],
                                    $"legacy '{key}' vs '{origin[canonical]}': " +
                                    $"'{rebuiltValues[idx]?.ToJsonString()}' vs '{value?.ToJsonString() ?? "null"}'");
                            }
                            rebuiltKeys.Add(canonical);
                            rebuiltValues.Add(value);
                            origin[canonical] = key;
                        }
                        else
                        {
                            int idx = rebuiltKeys.IndexOf(key);
                            if (idx >= 0)
                            {
                                if (CanonicalEquals(rebuiltValues[idx], value))
                                    continue;
                                throw new CatalogKeyConflictException(
                                    sourceName, path, key, origin[key],
                                    $"duplicate key '{key}' with conflicting values");
                            }
                            rebuiltKeys.Add(key);
                            rebuiltValues.Add(value);
                            origin[key] = key;
                        }
                    }

                    obj.Clear();
                    for (int ri = 0; ri < rebuiltKeys.Count; ri++)
                        obj.Add(rebuiltKeys[ri], rebuiltValues[ri]);
                    changed = true;
                }

                foreach (var (key, value) in obj)
                {
                    if (value == null)
                        continue;
                    if (NormalizeNode(value, aliasMap, sourceName, path + "." + key))
                        changed = true;
                }
            }
            else if (node is JsonArray arr)
            {
                for (int arrayIndex = 0; arrayIndex < arr.Count; arrayIndex++)
                {
                    var item = arr[arrayIndex];
                    if (item == null)
                        continue;
                    if (NormalizeNode(item, aliasMap, sourceName, path + "[" + arrayIndex + "]"))
                        changed = true;
                }
            }

            return changed;
        }

        /// <summary>Deterministic deep-equality via canonical serialized text.</summary>
        private static bool CanonicalEquals(JsonNode? a, JsonNode? b)
        {
            string sa = a?.ToJsonString(new JsonSerializerOptions { WriteIndented = false }) ?? "null";
            string sb = b?.ToJsonString(new JsonSerializerOptions { WriteIndented = false }) ?? "null";
            return string.Equals(sa, sb, StringComparison.Ordinal);
        }
    }
}
