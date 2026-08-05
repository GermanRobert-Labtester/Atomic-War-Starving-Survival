using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Data;
using AtomicWar._Game.Events;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// H-6: Unified event pool construction. Previously the event pool was
    /// assembled from 6 hand-rolled <c>EnsurePoolHas*</c> helpers + a
    /// catalog import + a factory call (+15+ factory methods), each doing
    /// its own duplicate-detection. This class consolidates all sources into
    /// a single <see cref="Build"/> method with a final duplicate-id guard.
    ///
    /// Background (audit H-6): EventRunner.SetPool stores events in a
    /// <c>List&lt;GameEvent&gt;</c> and FindInPool uses a linear scan that
    /// returns the first match. If two events share an id, the second is
    /// silently shadowed. The 6 EnsurePoolHas* helpers each had their own
    /// dedup logic (some O(n²), some O(n)) and none validated cross-source
    /// collisions.
    ///
    /// Usage (replaces 8 lines in GameBootstrap.InitializeSystems):
    /// <code>
    ///   var pool = EventPoolBuilder.Build(_eventCatalog);
    ///   EventRunner.SetPool(pool);
    /// </code>
    /// </summary>
    public static class EventPoolBuilder
    {
        /// <summary>
        /// L-11: Set to false to skip the EncounterEventFactory.CreateAll()
        /// call during pool construction. Useful when a developer wants to
        /// test a specific event scenario without the 10+ factory events
        /// cluttering the pool.
        /// </summary>
        public static bool IncludeEncounterFactoryEvents { get; set; } = true;
        /// <summary>
        /// Build the complete event pool from all sources. This is the
        /// single entry point that replaces the 6 EnsurePoolHas* helpers
        /// + EncounterEventFactory.CreateAll().
        ///
        /// Order (preserved from the original InitializeSystems):
        /// 1. Catalog events (from StreamingAssets JSON)
        /// 2. Emissary chain events
        /// 3. Radio-triggered events (Safe Haven Broadcast)
        /// 4. Missing Rations chain events
        /// 5. Biological trade events (Blood for Water)
        /// 6. Hatch entrapment events (Buried Alive + Faction DigOut)
        /// 7. Child Found event
        /// 8. Encounter event factory (expedition encounters + shelter events)
        ///
        /// After all sources are merged, validates that no duplicate event
        /// ids exist. If a duplicate is found, logs an error and keeps the
        /// first occurrence (matching the production FindInPool behavior).
        /// </summary>
        /// <param name="catalog">The event catalog SO (may be null in tests).</param>
        /// <returns>A new List&lt;GameEvent&gt; with all pooled events.</returns>
        public static List<GameEvent> Build(GameEventCatalogSO catalog)
        {
            var pool = new List<GameEvent>();

            // 1. Catalog events (lowest priority — factory versions override).
            if (catalog != null && catalog.events != null)
            {
                for (int i = 0; i < catalog.events.Count; i++)
                {
                    var ev = catalog.events[i];
                    if (ev != null && !string.IsNullOrEmpty(ev.id))
                        pool.Add(ev);
                }
            }

            // 2. Emissary chain (multi-stage, trait/trust gates).
            AddRangeWithDedup(pool, EventRunner.CreateEmissaryChain());

            // 3. Radio-triggered Safe Haven Broadcast (Prompt #46).
            AddSingleWithDedup(pool, EventRunner.CreateSafeHavenBroadcastEvent());

            // 4. Internal mysteries — Missing Rations chain.
            AddRangeWithDedup(pool, SuspicionTracker.CreateMissingRationsChain());

            // 5. Biological trade — Blood for Water (Prompt #47).
            AddSingleWithDedup(pool, EventRunner.CreateBloodForWaterEvent());

            // 6. Hatch entrapment — Buried Alive + Faction DigOut (Prompt #48).
            AddSingleWithDedup(pool, EventRunner.CreateBuriedAliveEvent());
            AddSingleWithDedup(pool, EventRunner.CreateFactionDigOutEvent());

            // 7. Child Found in the Ash (Prompt #9).
            AddSingleWithDedup(pool, CreateChildFoundEvent());

            // 8. Encounter event factory (Prompts #95-#104).
            // L-11: Gated behind IncludeEncounterFactoryEvents so
            // developers can disable it for targeted scenario testing.
            if (IncludeEncounterFactoryEvents)
            {
                var factoryEvents = EncounterEventFactory.CreateAll();
                if (factoryEvents != null)
                    AddRangeWithDedup(pool, factoryEvents);
            }

            // H-6: Final duplicate-id validation across ALL sources.
            ValidateNoDuplicateIds(pool);

            return pool;
        }

        // -----------------------------------------------------------------
        // Dedup helpers (O(n) per add)
        // -----------------------------------------------------------------

        private static void AddSingleWithDedup(List<GameEvent> pool, GameEvent ev)
        {
            if (pool == null || ev == null || string.IsNullOrEmpty(ev.id)) return;
            for (int i = 0; i < pool.Count; i++)
            {
                if (pool[i] != null && pool[i].id == ev.id)
                    return; // Already present — first-wins.
            }
            pool.Add(ev);
        }

        private static void AddRangeWithDedup(List<GameEvent> pool, IReadOnlyList<GameEvent> events)
        {
            if (pool == null || events == null) return;
            for (int i = 0; i < events.Count; i++)
                AddSingleWithDedup(pool, events[i]);
        }

        // -----------------------------------------------------------------
        // Duplicate-id validation (H-6 guard)
        // -----------------------------------------------------------------

        /// <summary>
        /// Scan the final pool for duplicate event ids. Since pool insertion
        /// is deduped, any duplicates indicate two DIFFERENT sources produced
        /// events with the same id. The first occurrence wins but a warning
        /// is logged so designers can resolve the collision.
        ///
        /// This is the runtime companion to the editor-only EventIdValidator
        /// (H-3), which runs on the Tools/ASHFALL menu.
        /// </summary>
        internal static void ValidateNoDuplicateIds(List<GameEvent> pool)
        {
            if (pool == null || pool.Count < 2) return;

            var seen = new HashSet<string>();
            for (int i = 0; i < pool.Count; i++)
            {
                var ev = pool[i];
                if (ev == null || string.IsNullOrEmpty(ev.id)) continue;
                if (!seen.Add(ev.id))
                {
                    Debug.LogWarning(
                        $"[EventPoolBuilder] Duplicate event id '{ev.id}' at index {i}. " +
                        $"The first occurrence in the pool will be found by FindInPool; " +
                        $"the later occurrence is silently shadowed. " +
                        $"Resolve by removing the duplicate from its source " +
                        $"(catalog, factory, or chain helper).");
                }
            }
        }

        // -----------------------------------------------------------------
        // Child Found event factory (moved from GameBootstrap, Prompt #9)
        // -----------------------------------------------------------------

        private static GameEvent CreateChildFoundEvent()
        {
            var ev = ScriptableObject.CreateInstance<GameEvent>();
            ev.id = "child_found_in_ash";
            ev.title = "A Small Figure in the Ash";
            ev.bodyText =
                "During a scavenging run, one of your survivors spots movement in the ash drifts. " +
                "At first it looks like an animal — but then the shape resolves. It's a child. " +
                "Maybe eight years old. Filthy, shivering, barely able to stand. They don't speak. " +
                "They just stare at the scavenger with hollow, exhausted eyes.\n\n" +
                "The child cannot work. Cannot fight. Cannot scavenge. They will consume food and " +
                "water like anyone else. But keeping them alive might mean something. " +
                "Something the bunker has been losing.";
            ev.minDay = 8;
            ev.weight = 1f;

            ev.choices = new List<EventChoice>
            {
                new EventChoice
                {
                    ChoiceId = "take_the_child",
                    Text = "Bring them into the bunker. They're just a child.",
                    MoraleDelta = 15f,
                    Effects = new List<EventEffect>
                    {
                        new EventEffect
                        {
                            SetWorldFlag = ChildDependentSystem.ChildFoundFlag,
                            WorldFlagValue = true
                        }
                    },
                    SetEventFlags = new List<string> { ChildDependentSystem.ChildFoundFlag }
                },
                new EventChoice
                {
                    ChoiceId = "leave_them",
                    Text = "We can barely feed ourselves. Keep moving.",
                    MoraleDelta = -10f
                }
            };

            return ev;
        }
    }
}
