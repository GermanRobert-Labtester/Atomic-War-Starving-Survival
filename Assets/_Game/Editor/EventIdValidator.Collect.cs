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

        private static int CountAllEvents() => CollectAllEvents().Count;

        private static void CollectFromEncounterFactory(List<(string, string)> result)
        {
            // H-6: EncounterEventFactory.CreateAll() is now called inside
            // EventPoolBuilder.Build(), so CollectFromEventPoolBuilder already
            // captures these events. Skip direct collection to avoid duplicates.
        }

        private static void CollectFromChainFactories(List<(string, string)> result)
        {
            // H-6: Chain factories are now invoked inside EventPoolBuilder.Build().
            // CollectFromEventPoolBuilder already captures them. This method is
            // a no-op to avoid double-counting.
        }

        private static void CollectFromEventPoolBuilder(List<(string, string)> result)
        {
            try
            {
                var pool = EventPoolBuilder.Build(null); // null catalog = factory events only
                if (pool != null)
                {
                    for (int i = 0; i < pool.Count; i++)
                    {
                        var ev = pool[i];
                        if (ev != null && !string.IsNullOrEmpty(ev.id))
                            result.Add((ev.id, "factory"));
                    }
                }
            }
            catch (System.Exception ex)
            {
                UnityEngine.Debug.LogWarning(
                    $"[EventIdValidator] EventPoolBuilder.Build() threw: {ex.Message}. " +
                    "Falling back to EnsurePoolHas* reflection.");
            }
        }

        private static void CollectFromEnsureHelpers(List<(string, string)> result)
        {
            // H-6: EnsurePoolHas* methods were migrated to EventPoolBuilder.Build().
            // The old reflection approach is kept for backward compat (older branches
            // may still have the methods). The EventPoolBuilder path is preferred.
            CollectFromEventPoolBuilder(result);

            // Fallback: if EnsurePoolHas* methods still exist (pre-H-6 branches),
            // also collect from them via reflection.
            var gbType = typeof(GameBootstrap);
            var instance = new GameObject("EventIdValidatorTemp").AddComponent<GameBootstrap>();
            try
            {
                var ensureMethods = gbType.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                for (int i = 0; i < ensureMethods.Length; i++)
                {
                    var m = ensureMethods[i];
                    if (!m.Name.StartsWith("EnsurePoolHas")) continue;
                    if (m.ReturnType != typeof(void)) continue;
                    var parms = m.GetParameters();
                    if (parms.Length != 1 || parms[0].ParameterType != typeof(List<GameEvent>)) continue;
                    var fresh = new List<GameEvent>();
                    try
                    {
                        m.Invoke(instance, new object[] { fresh });
                    }
                    catch (System.Exception ex)
                    {
                        Debug.LogWarning($"[EventIdValidator] {m.Name} threw: {ex.Message}");
                        continue;
                    }
                    for (int j = 0; j < fresh.Count; j++)
                    {
                        var ev = fresh[j];
                        if (ev == null || string.IsNullOrEmpty(ev.id)) continue;
                        result.Add((ev.id, $"GameBootstrap.{m.Name}[{j}]"));
                    }
                }
            }
            finally
            {
                if (instance != null) UnityEngine.Object.DestroyImmediate(instance.gameObject);
            }
        }

        private static void CollectFromGameBootstrapDirect(List<(string, string)> result)
        {
            // GameBootstrap creates events through factory methods which
            // are already captured by CollectFromEnsureHelpers. We don't
            // double-count here. This method is reserved for future
            // event sources that don't fit the Ensure* pattern.
        }

        private static void CollectFromStreamingAssetsCatalog(List<(string, string)> result)
        {
            // Look in Assets/StreamingAssets/Data/events.json for
            // user-authored events. The file is a JSON array of objects
            // with an "id" field. We don't import the file (Unity owns that)
            // — we just grep for ids to surface collisions.
            string path = "Assets/StreamingAssets/Data/events.json";
            if (!System.IO.File.Exists(path)) return;
            string text;
            try { text = System.IO.File.ReadAllText(path); }
            catch { return; }
            // Id strings are quoted. Capture them.
            var idRegex = new Regex("\"id\"\\s*:\\s*\"([^\"]+)\"");
            var matches = idRegex.Matches(text);
            for (int i = 0; i < matches.Count; i++)
            {
                string id = matches[i].Groups[1].Value;
                if (string.IsNullOrEmpty(id)) continue;
                result.Add((id, $"StreamingAssets/events.json[{i}]"));
            }
        }

    }
}
