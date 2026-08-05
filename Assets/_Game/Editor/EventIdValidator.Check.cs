using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;
using AtomicWar._Game.Data;
using AtomicWar._Game.Events;
using AtomicWar._Game.Core;

namespace AtomicWar._Game.Editor
{
    public static partial class EventIdValidator
    {

        private static void CheckDuplicates(List<(string id, string source)> allEvents, List<string> diagnostics)
        {
            // Group by id. The first source in each group is the "winner";
            // every subsequent source is a "loser" that is silently
            // shadowed at runtime.
            //
            // The catalog (StreamingAssets/events.json) and the chain
            // factory (EventRunner.CreateEmissaryChain) are NOT a duplicate
            // pair: the catalog is a fallback for when the chain factory
            // isn't reachable, and the chain factory is the canonical
            // source. They are filtered to two distinct lists before
            // duplicate detection.
            var byId = new Dictionary<string, List<string>>();
            for (int i = 0; i < allEvents.Count; i++)
            {
                var (id, source) = allEvents[i];
                if (!byId.TryGetValue(id, out var list))
                {
                    list = new List<string>();
                    byId[id] = list;
                }
                list.Add(source);
            }
            // Sort duplicate ids deterministically.
            var sortedKeys = new List<string>(byId.Keys);
            sortedKeys.Sort(string.CompareOrdinal);
            for (int i = 0; i < sortedKeys.Count; i++)
            {
                string id = sortedKeys[i];
                var sources = byId[id];
                if (sources.Count > 1)
                {
                    // Group by source-type to filter out cross-type matches.
                    // Two catalog sources ARE a duplicate (catalog may
                    // have multiple entries with the same id by mistake).
                    // One catalog + one factory is NOT a duplicate (the
                    // factory is the canonical source, catalog is a
                    // fallback). Two factories ARE a duplicate.
                    int catalogCount = CountWhere(sources, s => s.StartsWith("StreamingAssets/"));
                    int factoryCount = sources.Count - catalogCount;
                    if (catalogCount > 1 || factoryCount > 1)
                    {
                        diagnostics.Add($"DUPLICATE id='{id}' produced by {sources.Count} sources: " +
                            string.Join(" | ", sources));
                    }
                }
            }
        }

        private static int CountWhere(List<string> items, System.Func<string, bool> pred)
        {
            int c = 0;
            for (int i = 0; i < items.Count; i++) if (pred(items[i])) c++;
            return c;
        }

        private static void CheckEmptyIds(List<(string id, string source)> allEvents, List<string> diagnostics)
        {
            for (int i = 0; i < allEvents.Count; i++)
            {
                var (id, source) = allEvents[i];
                if (string.IsNullOrEmpty(id) || id.StartsWith("<null:"))
                {
                    diagnostics.Add($"EMPTY id produced by {source}.");
                }
            }
        }

        private static void CheckNamingConvention(List<(string id, string source)> allEvents, List<string> diagnostics)
        {
            for (int i = 0; i < allEvents.Count; i++)
            {
                var (id, source) = allEvents[i];
                if (string.IsNullOrEmpty(id)) continue; // already reported
                if (id.StartsWith("<null:")) continue;
                if (!SnakeCasePattern.IsMatch(id))
                {
                    diagnostics.Add($"BAD NAMING id='{id}' (source {source}) — must match snake_case ^[a-z][a-z0-9_]*$");
                }
            }
        }

    }
}
