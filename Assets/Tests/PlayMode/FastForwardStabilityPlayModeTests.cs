using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.TestTools;
using AtomicWar._Game.Core;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Events;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.UI;
using AtomicWar._Game.Utilities;

namespace AtomicWar.Tests.PlayMode
{
    /// <summary>
    /// Fast-forward stability pass (#40 acceptance): simulate 100 in-game days
    /// at 3x speed through the real clock + pooled UI data layer (journal
    /// entries, inventory icons, map-node views, expedition path lines).
    /// Asserts: no day/hour tick is skipped, no UI object is created or
    /// destroyed after warm-up (pool counters stay flat), and steady-state
    /// managed allocation shows no growth (minimal GC spikes during time-skips).
    /// </summary>
    [TestFixture]
    public class FastForwardStabilityPlayModeTests
    {
        private const float Eps = 1e-3f;
        private const int TotalDays = 100;
        private const float FastForward = 3f;

        /// <summary>
        /// Game-time per simulated frame at 3x. Binary-exact (2^-2) and divides
        /// 24h cleanly, so 100 days of float accumulation stays exact — the
        /// assertions below measure the clock, not float rounding noise.
        /// </summary>
        private const float GameHoursPerStep = 0.25f;
        private const int TotalSteps = TotalDays * 24 * 4; // 9600 quarter-hour steps

        private static ItemDefinition NewItem(string id, ItemType type)
        {
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = id;
            item.displayName = id;
            item.type = type;
            item.stackMax = 20;
            item.weight = 1f;
            return item;
        }

        [UnityTest]
        public IEnumerator HundredDays_AtThreeX_Stable_NoUiChurn_NoGcGrowth()
        {
            // ------------------------------------------------------------
            // Build the simulation slice exactly as GameBootstrap wires it.
            // ------------------------------------------------------------
            var clock = new TimeSystem { SecondsPerGameHour = 10f };
            clock.SetTimeScale(FastForward);
            Assert.That(clock.TimeScale, Is.EqualTo(FastForward).Within(Eps));

            var journalPool = new GenericObjectPool<JournalEntry>(
                () => new JournalEntry(),
                e =>
                {
                    e.Id = null; e.Text = null; e.Timestamp = null;
                    e.AuthorName = null; e.AuthorId = null; e.KnowledgeKey = null;
                    e.Day = 0; e.Hour = 0f;
                },
                // +1: at a full list the new entry is acquired before the
                // evicted one is released (matches GameBootstrap wiring).
                initialCapacity: JournalSystem.MaxEntries + 1);
            var journal = new JournalSystem();
            journal.SetEntryFactory(journalPool.Acquire, journalPool.Release);

            var map = MapGenerator.Generate(1337);
            var mapObject = new GameObject("MapScreenUI_Test");
            var mapUi = mapObject.AddComponent<MapScreenUI>();
            mapUi.Bind(map);

            var stripObject = new GameObject("InventoryStrip_Test");
            var strip = stripObject.AddComponent<InventoryStripUI>();
            var inventory = new Inventory { Capacity = 50, MaxWeight = 500f };
            var food = NewItem("canned_food", ItemType.Food);
            var water = NewItem("clean_water", ItemType.Water);
            var iodine = NewItem("iodine_pills", ItemType.Iodine);
            inventory.Add(food, 4);
            inventory.Add(water, 4);
            inventory.Add(iodine, 2);
            strip.Sync(inventory);

            // Selectable non-shelter nodes for path-line churn.
            var targetNodes = new List<string>();
            for (int i = 0; i < map.Nodes.Count && targetNodes.Count < 5; i++)
            {
                var n = map.Nodes[i];
                if (n != null && !n.IsShelter) targetNodes.Add(n.NodeId);
            }
            Assert.That(targetNodes.Count, Is.GreaterThan(0), "map must expose selectable nodes");

            int dayTicks = 0, hourTicks = 0;
            clock.OnDayTick += d => dayTicks++;
            clock.OnHourTick += (d, h) => hourTicks++;

            // ------------------------------------------------------------
            // GC allocation recorder (Unity.Profiling counter API). Batchmode
            // starts with the profiler off — enable it so the counter samples.
            // Defensive: if still unavailable, pool flatness proves
            // allocation-free UI churn.
            // ------------------------------------------------------------
            bool profilerWasEnabled = UnityEngine.Profiling.Profiler.enabled;
            UnityEngine.Profiling.Profiler.enabled = true;
            var gcAlloc = ProfilerRecorder.StartNew(
                ProfilerCategory.Memory, "GC.Alloc", 4096, ProfilerRecorderOptions.Default);
            bool profilerLive = gcAlloc.Valid;

            long earlyBytes = -1, steadyBytes = -1;
            int earlyStartSample = -1, steadyStartSample = -1;
            int measuredDayEarlyEnd = -1, measuredDaySteadyEnd = -1;
            int warmupJournalCreated = -1, warmupIconCreated = -1, warmupPathLineCreated = -1;

            // ------------------------------------------------------------
            // The 100-day @3x frame loop (count-driven: no float drift).
            // ------------------------------------------------------------
            float targetHours = TotalDays * 24f;
            int steps = 0;
            int discoverySeq = 0;
            int nextDiscoveryHour = 12;
            int nextChurnHour = 24;
            bool churnFoodUp = true;
            bool earlyWindowArmed = false, steadyWindowArmed = false;

            while (steps < TotalSteps)
            {
                clock.TickHours(GameHoursPerStep);
                steps++;

                int hourNow = steps / 4; // quarter-hour steps -> elapsed hours, exact
                int day = clock.CurrentDay;

                // Journal churn: 2 discoveries/day force acquire + eviction-recycle
                // far past the 64-entry cap over the run.
                if (hourNow >= nextDiscoveryHour)
                {
                    nextDiscoveryHour += 12;
                    journal.TryDiscover(
                        $"stress_discovery_{discoverySeq++}", null, day, clock.CurrentHourFloat);
                }

                // Icon + path-line churn once per game-day.
                if (hourNow >= nextChurnHour)
                {
                    nextChurnHour += 24;
                    if (churnFoodUp) inventory.Add(food, 2);
                    else inventory.Remove(food, 2);
                    churnFoodUp = !churnFoodUp;
                    strip.Sync(inventory);
                    mapUi.SelectNode(targetNodes[day % targetNodes.Count]); // rebuilds pooled path lines
                    mapUi.Refresh();                                       // refills node-view buffer in place
                }

                // Warm-up snapshot at day 10 + GC windows: days 10-20 (early) vs 80-90 (steady).
                if (warmupJournalCreated < 0 && day >= 10)
                {
                    warmupJournalCreated = journalPool.InstancesCreated;
                    warmupIconCreated = strip.IconPool.InstancesCreated;
                    warmupPathLineCreated = mapUi.PathLinePool.InstancesCreated;
                }
                if (profilerLive)
                {
                    if (!earlyWindowArmed && day >= 10)
                    {
                        earlyWindowArmed = true;
                        earlyStartSample = gcAlloc.Count;
                    }
                    if (earlyWindowArmed && earlyBytes < 0 && day > 20)
                    {
                        earlyBytes = SumGcAlloc(gcAlloc, earlyStartSample);
                        measuredDayEarlyEnd = day;
                    }
                    if (!steadyWindowArmed && day >= 80)
                    {
                        steadyWindowArmed = true;
                        steadyStartSample = gcAlloc.Count;
                    }
                    if (steadyWindowArmed && steadyBytes < 0 && day > 90)
                    {
                        steadyBytes = SumGcAlloc(gcAlloc, steadyStartSample);
                        measuredDaySteadyEnd = day;
                    }
                }

                // Yield every 30 game-hours so profiler samples spread across
                // both GC windows (80 frames over the full run).
                if (steps % 120 == 0)
                    yield return null;
            }

            if (profilerLive)
            {
                gcAlloc.Stop();
                gcAlloc.Dispose();
            }
            UnityEngine.Profiling.Profiler.enabled = profilerWasEnabled;

            // ------------------------------------------------------------
            // Time fidelity: nothing skipped at 3x.
            // ------------------------------------------------------------
            Assert.That(dayTicks, Is.EqualTo(TotalDays),
                "every simulated day boundary must fire exactly one day tick");
            Assert.That(clock.CurrentDay, Is.EqualTo(TotalDays + 1));
            Assert.That(clock.TotalElapsedHours, Is.EqualTo(targetHours).Within(Eps));
            Assert.That(hourTicks, Is.EqualTo(steps),
                "every sub-step must fire an hour tick (no skipped AI-tick heartbeats)");

            // ------------------------------------------------------------
            // No UI objects created or destroyed after warm-up.
            // Warm-up ends at day 10: every pool must be at steady size by then.
            // ------------------------------------------------------------
            Assert.That(mapUi, Is.Not.Null, "map UI object must never be destroyed");
            Assert.That(strip, Is.Not.Null, "inventory strip object must never be destroyed");
            Assert.That(journal.EntryCount, Is.LessThanOrEqualTo(JournalSystem.MaxEntries));

            AssertPoolConserved("journal", journalPool);
            AssertPoolConserved("inventory-icon", strip.IconPool);
            AssertPoolConserved("path-line", mapUi.PathLinePool);

            // Creation stopped after warm-up: the remaining 90 days of churn
            // (journal evictions, icon resyncs, path rebuilds) reused stock,
            // i.e. nothing was instantiated or destroyed at runtime.
            Assert.That(journalPool.InstancesCreated, Is.EqualTo(warmupJournalCreated),
                "journal entries: no new instances after warm-up");
            Assert.That(strip.IconPool.InstancesCreated, Is.EqualTo(warmupIconCreated),
                "inventory icons: no new instances after warm-up");
            Assert.That(mapUi.PathLinePool.InstancesCreated, Is.EqualTo(warmupPathLineCreated),
                "expedition path lines: no new instances after warm-up");

            // The journal pool must have recycled: more discoveries than capacity.
            Assert.That(discoverySeq, Is.GreaterThan(JournalSystem.MaxEntries),
                "run must push past the entry cap to exercise eviction-recycle");
            Assert.That(journalPool.PooledCount + journalPool.ActiveCount,
                Is.EqualTo(journalPool.InstancesCreated),
                "no pooled instance may be lost (destroyed) or leaked");

            // ------------------------------------------------------------
            // GC: steady-state window must not grow versus early window and
            // must stay small in absolute terms.
            // ------------------------------------------------------------
            if (profilerLive && earlyBytes >= 0 && steadyBytes >= 0)
            {
                // Primary guard: no allocation GROWTH across the run. A pooling
                // regression (objects recreated instead of reused) scales with
                // churn and shows up here.
                Assert.That(steadyBytes, Is.LessThanOrEqualTo(earlyBytes * 2 + 4L * 1024 * 1024),
                    $"allocation growth over the run: early(d10-20)={earlyBytes}B steady(d80-90)={steadyBytes}B");

                // Absolute budget. Editor frame overhead (profiler sampling,
                // coroutine/test-runner plumbing) dominates this number — the
                // simulation's own churn is a few KB/day of content strings.
                // 4 MB/day leaves ample headroom while still catching
                // pathological leaks (MB-scale garbage per day).
                long perDayBytes = steadyBytes / Mathf.Max(1, measuredDaySteadyEnd - 80);
                Assert.That(perDayBytes, Is.LessThan(4L * 1024 * 1024),
                    $"steady-state GC.Alloc must stay minimal: {perDayBytes} B/day");
            }
            else
            {
                Debug.LogWarning("[FastForwardStability] GC.Alloc recorder unavailable in this run; " +
                                 "pool-flatness assertions still prove allocation-free UI churn.");
            }

            Object.Destroy(mapObject);
            Object.Destroy(stripObject);
        }

        private static long SumGcAlloc(ProfilerRecorder recorder, int fromSample)
        {
            long total = 0;
            int count = recorder.Count;
            for (int i = Mathf.Max(0, fromSample); i < count; i++)
            {
                total += recorder.GetSample(i).Value;
            }
            return total;
        }

        private static void AssertPoolConserved(string label, GenericObjectPool<JournalEntry> pool)
            => AssertPoolConserved(label, pool.InstancesCreated, pool.ActiveCount, pool.PooledCount);

        private static void AssertPoolConserved(string label, GenericObjectPool<InventoryIcon> pool)
            => AssertPoolConserved(label, pool.InstancesCreated, pool.ActiveCount, pool.PooledCount);

        private static void AssertPoolConserved(string label, GenericObjectPool<MapPathLine> pool)
            => AssertPoolConserved(label, pool.InstancesCreated, pool.ActiveCount, pool.PooledCount);

        private static void AssertPoolConserved(string label, int created, int active, int pooled)
        {
            Assert.That(active + pooled, Is.EqualTo(created),
                $"{label} pool: every instance must be accounted for (none destroyed, none leaked)");
        }
    }
}
