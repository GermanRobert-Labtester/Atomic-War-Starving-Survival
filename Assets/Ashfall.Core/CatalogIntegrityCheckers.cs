// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Ashfall.Core
{
    public sealed class CatalogValidationContext
    {
        public readonly Dictionary<string, List<string>> Registry =
            new Dictionary<string, List<string>>(StringComparer.Ordinal);
        public readonly List<CatalogPendingReference> PendingRefs = new List<CatalogPendingReference>();
        public readonly Dictionary<string, CatalogRangeMemoEntry> RangeMemo =
            new Dictionary<string, CatalogRangeMemoEntry>(StringComparer.Ordinal);
        public CatalogIntegrityReport Report;
        public string File;
        /// <summary>Ids authored (first-ever occurrence of each value).</summary>
        public int Authored;
        /// <summary>References to an id that is already authored (reuse).</summary>
        public int Reuse;
    }

    public struct CatalogPendingReference
    {
        public string Value;
        public string Path;
        /// <summary>True when the position is a declared reference key (Tier 2:
        /// bare ids must also resolve). False = generic position (Tier 1:
        /// only prefixed ids are checked).</summary>
        public bool Strict;
    }

    public sealed class CatalogRangeMemoEntry
    {
        public int? Min;
        public int? Max;
    }

    /// <summary>
    /// Specialized checker for definition registration, entity root conflicts, and legitimate reuse.
    /// </summary>
    public static class CatalogIntegrityDefinitionChecker
    {
        public static void RegisterOrReference(string key, string value, string path, CatalogValidationContext ctx)
        {
            if (CatalogIntegrityRules.IsDefinitionKey(key))
            {
                Register(key, value, path, ctx);
            }
            if (CatalogIntegrityRules.IsReferenceKey(key))
            {
                ctx.PendingRefs.Add(new CatalogPendingReference { Value = value, Path = path, Strict = true });
            }
            else if (!CatalogIntegrityRules.IsDefinitionKey(key) && CatalogIntegrityRules.StartsWithAnyPrefix(value))
            {
                // Any prefixed string in a non-id position is still a reference
                // (Tier 1) — e.g. a narrative field naming an item id.
                ctx.PendingRefs.Add(new CatalogPendingReference { Value = value, Path = path, Strict = false });
            }
        }

        public static void Register(string key, string value, string path, CatalogValidationContext ctx)
        {
            if (ctx.Registry.TryGetValue(value, out List<string>? existing))
            {
                // The id already has an author. Distinguish a GENUINE within-file
                // entity-id conflict from legitimate id reuse:
                //   • A conflict is a literal `id` registered at entity-root depth
                //     (file.json[N]/id) twice in the SAME file — i.e. two rows of
                //     one catalog claim the same identity (Invariant 6).
                //   • Everything else is reuse: shared stage/choice templates
                //     (stages[N]/id), enrichment *—fields/*—tags foreign keys, and
                //     per-container row rewrites (npcs[i]/id). These are normal
                //     composition across an id's single authority — not an error.
                string firstPath = existing[0];
                if (key == "id"
                    && IsEntityRootId(path)
                    && IsEntityRootId(firstPath)
                    && FileLeaf(path) == FileLeaf(firstPath))
                    ctx.Report.Error("duplicate id '" + value + "' defined at " + path
                        + " (first: " + firstPath + ")");
                else
                    ctx.Reuse++;
            }
            else
            {
                existing = new List<string>();
                ctx.Registry[value] = existing;
                ctx.Authored++;
            }
            existing.Add(path);
        }

        /// <summary>An id is at entity-root depth when its path has exactly one
        /// slash: file.json[N]/id. Deeper paths (stages[N]/id, choices[N]/id,
        /// npcs[i]/id, entries[N]/…/id) are nested template/container ids that
        /// participate in reuse, not entity-root authorship conflicts.</summary>
        public static bool IsEntityRootId(string path)
        {
            int slashes = 0;
            for (int i = 0; i < path.Length; i++)
                if (path[i] == '/') slashes++;
            return slashes == 1;
        }

        /// <summary>Extract the JSON catalog leaf name (strip array indices and
        /// any nested path) so same-file detection compares the actual file.
        /// "a.json[0]/nested[1]/id" → "a.json".</summary>
        public static string FileLeaf(string path)
        {
            int slash = path.IndexOf('/');
            int bracket = path.IndexOf('[');
            int end = int.MaxValue;
            if (slash >= 0 && slash < end) end = slash;
            if (bracket >= 0 && bracket < end) end = bracket;
            return end == int.MaxValue ? path : path.Substring(0, end);
        }
    }

    /// <summary>
    /// Specialized checker for Tier-1 (prefixed) and Tier-2 (strict reference keys) cross-references.
    /// </summary>
    public static class CatalogIntegrityReferenceChecker
    {
        public static void ValidatePendingReferences(CatalogValidationContext ctx, CatalogIntegrityReport report)
        {
            var reported = new HashSet<string>(StringComparer.Ordinal);
            foreach (CatalogPendingReference r in ctx.PendingRefs)
            {
                if (!reported.Add(r.Value + "@" + r.Path)) continue;
                if (CatalogIntegrityRules.IsKnownRuntimeId(r.Value)) continue;
                bool prefixed = CatalogIntegrityRules.StartsWithAnyPrefix(r.Value);
                if (ctx.Registry.ContainsKey(r.Value)) continue;
                if (prefixed || r.Strict)
                    report.Error("unresolved " + (prefixed ? "id '" : "reference '")
                        + r.Value + "' at " + r.Path);
            }
        }
    }

    /// <summary>
    /// Specialized checker for minDay/maxDay numeric ranges and ordering rules.
    /// </summary>
    public static class CatalogIntegrityRangeChecker
    {
        public static void CheckRange(string key, int value, string parentPath, CatalogValidationContext ctx)
        {
            // Memoised per parent object: the min/max pair is checked when both
            // siblings have been seen.
            if (!ctx.RangeMemo.TryGetValue(parentPath, out CatalogRangeMemoEntry? memo))
            {
                memo = new CatalogRangeMemoEntry();
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
    }

    /// <summary>
    /// Specialized checker for wasteland / navigation route graphs.
    /// </summary>
    public static class CatalogIntegrityRouteChecker
    {
        public static void CheckRoutes(JsonElement routesElement, string childPath, CatalogValidationContext ctx)
        {
            var seenFileRoutes = new HashSet<string>(StringComparer.Ordinal);
            int routeIdx = 0;
            foreach (JsonElement routeElem in routesElement.EnumerateArray())
            {
                string rPath = childPath + "[" + routeIdx + "]";
                if (routeElem.ValueKind == JsonValueKind.Object)
                {
                    string? rFrom = null;
                    string? rTo = null;
                    float? dist = null;

                    if (routeElem.TryGetProperty("from", out var fProp) && fProp.ValueKind == JsonValueKind.String)
                        rFrom = fProp.GetString();
                    if (routeElem.TryGetProperty("to", out var tProp) && tProp.ValueKind == JsonValueKind.String)
                        rTo = tProp.GetString();
                    if (routeElem.TryGetProperty("distanceKm", out var dProp) && dProp.TryGetSingle(out float dVal))
                        dist = dVal;

                    if (!string.IsNullOrEmpty(rFrom) && !string.IsNullOrEmpty(rTo))
                    {
                        if (string.Equals(rFrom, rTo, StringComparison.Ordinal))
                        {
                            ctx.Report.Error("self-route detected at " + rPath + ": '" + rFrom + "' -> '" + rTo + "'");
                        }
                        string rKey = rFrom + "->" + rTo;
                        if (!seenFileRoutes.Add(rKey))
                        {
                            ctx.Report.Error("duplicate route '" + rKey + "' at " + rPath);
                        }
                    }

                    if (dist.HasValue && dist.Value <= 0f)
                    {
                        ctx.Report.Error("negative or zero distance (" + dist.Value + ") at " + rPath);
                    }
                }
                routeIdx++;
            }
        }
    }
}
