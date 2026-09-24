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
}
