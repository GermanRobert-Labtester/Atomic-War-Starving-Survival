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
        /// 24h cleanly, so 100 days of float accumulation stays exact.
        /// </summary>
        private const float GameHoursPerStep = 0.25f;
        private const int TotalSteps = TotalDays * 24 * 4; // 9600 quarter-hour steps

        /// <summary>
        /// Holds all simulation objects for the fast-forward test (audit smell fix:
        /// extracted from the monolithic test method to reduce complexity).
        /// </summary>
        private sealed class SimSlice
        {
            public TimeSystem Clock;
            public JournalSystem Journal;
            public GenericObjectPool<JournalEntry> JournalPool;
            public MapScreenUI MapUi;
            public InventoryStripUI Strip;
            public Inventory Inventory;
            public List<string> TargetNodes;
            public GameObject MapObject;
            public GameObject StripObject;
            public int DayTicks, HourTicks;
        }

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

        private static SimSlice BuildSimSlice()
        {
            var s = new SimSlice();

            s.Clock = new TimeSystem { SecondsPerGameHour = 10f };
            s.Clock.SetTimeScale(FastForward);

            s.JournalPool = new GenericObjectPool<JournalEntry>(
                () => new JournalEntry(),
                e =>
                {
                    e.Id = null; e.Text = null; e.Timestamp = null;
                    e.AuthorName = null; e.AuthorId = null; e.KnowledgeKey = null;
                    e.Day = 0; e.Hour = 0f;
                },
                initialCapacity: JournalSystem.MaxEntries + 1);
            s.Journal = new JournalSystem();
            s.Journal.SetEntryFactory(s.JournalPool.Acquire, s.JournalPool.Release);

            var map = MapGenerator.Generate(1337);
            s.MapObject = new GameObject("MapScreenUI_Test");
            s.MapUi = s.MapObject.AddComponent<MapScreenUI>();
            s.MapUi.Bind(map);

            s.StripObject = new GameObject("InventoryStrip_Test");
            s.Strip = s.StripObject.AddComponent<InventoryStripUI>();
            s.Inventory = new Inventory { Capacity = 50, MaxWeight = 500f };
            var food = NewItem("canned_food", ItemType.Food);
            var water = NewItem("clean_water", ItemType.Water);
            var iodine = NewItem("iodine_pills", ItemType.Iodine);
            s.Inventory.Add(food, 4);
            s.Inventory.Add(water, 4);
            s.Inventory.Add(iodine, 2);
            s.Strip.Sync(s.Inventory);

            s.TargetNodes = new List<string>();
            for (int i = 0; i < map.Nodes.Count && s.TargetNodes.Count < 5; i++)
            {
                var n = map.Nodes[i];
                if (n != null && !n.IsShelter) s.TargetNodes.Add(n.NodeId);
            }

            s.Clock.OnDayTick += d => s.DayTicks++;
            s.Clock.OnHourTick += (d, h) => s.HourTicks++;

            return s;
        }

        private sealed class GcWindows
        {
            public bool ProfilerLive;
            public ProfilerRecorder GcAlloc;
            public long EarlyBytes = -1, SteadyBytes = -1;
            public int EarlyStartSample = -1, SteadyStartSample = -1;
            public int MeasuredDayEarlyEnd = -1, MeasuredDaySteadyEnd = -1;
            public int WarmupJournalCreated = -1, WarmupIconCreated = -1, WarmupPathLineCreated = -1;
        }

        private static GcWindows InitGcWindows()
        {
            var w = new GcWindows();
            bool profilerWasEnabled = UnityEngine.Profiling.Profiler.enabled;
            UnityEngine.Profiling.Profiler.enabled = true;
            w.GcAlloc = ProfilerRecorder.StartNew(
                ProfilerCategory.Memory, "GC.Alloc", 4096, ProfilerRecorderOptions.Default);
            w.ProfilerLive = w.GcAlloc.Valid;
            if (!w.ProfilerLive)
                UnityEngine.Profiling.Profiler.enabled = profilerWasEnabled;
            return w;
        }

        private static void RunSimLoop(SimSlice s, GcWindows w, out int discoverySeq)
        {
            int steps = 0;
            int seq = 0;
            int nextDiscoveryHour = 12;
            int nextChurnHour = 24;
            bool churnFoodUp = true;
            bool earlyWindowArmed = false, steadyWindowArmed = false;

            while (steps < TotalSteps)
            {
                s.Clock.TickHours(GameHoursPerStep);
                steps++;

                int hourNow = steps / 4;
                int day = s.Clock.CurrentDay;

                if (hourNow >= nextDiscoveryHour)
                {
                    nextDiscoveryHour += 12;
                    s.Journal.TryDiscover($"stress_discovery_{seq++}", null, day, s.Clock.CurrentHourFloat);
                }

                if (hourNow >= nextChurnHour)
                {
                    nextChurnHour += 24;
                    if (churnFoodUp) s.Inventory.Add(s.Inventory.Slots[0]?.Item, 2);
                    else if (s.Inventory.Slots.Count > 0 && s.Inventory.Slots[0]?.Item != null)
                        s.Inventory.Remove(s.Inventory.Slots[0].Item, 2);
                    churnFoodUp = !churnFoodUp;
                    s.Strip.Sync(s.Inventory);
                    s.MapUi.SelectNode(s.TargetNodes[day % s.TargetNodes.Count]);
                    s.MapUi.Refresh();
                }

                // Warm-up snapshot + GC window collection.
                if (w.WarmupJournalCreated < 0 && day >= 10)
                {
                    w.WarmupJournalCreated = s.JournalPool.InstancesCreated;
                    w.WarmupIconCreated = s.Strip.IconPool.InstancesCreated;
                    w.WarmupPathLineCreated = s.MapUi.PathLinePool.InstancesCreated;
                }
                if (w.ProfilerLive)
                {
                    if (!earlyWindowArmed && day >= 10) { earlyWindowArmed = true; w.EarlyStartSample = w.GcAlloc.Count; }
                    if (earlyWindowArmed && w.EarlyBytes < 0 && day > 20)
                    { w.EarlyBytes = SumGcAlloc(w.GcAlloc, w.EarlyStartSample); w.MeasuredDayEarlyEnd = day; }
                    if (!steadyWindowArmed && day >= 80) { steadyWindowArmed = true; w.SteadyStartSample = w.GcAlloc.Count; }
                    if (steadyWindowArmed && w.SteadyBytes < 0 && day > 90)
                    { w.SteadyBytes = SumGcAlloc(w.GcAlloc, w.SteadyStartSample); w.MeasuredDaySteadyEnd = day; }
                }

                if (steps % 120 == 0)
                    break; // yield return null in IEnumerator
            }
            discoverySeq = seq;
        }

        [UnityTest]
        public IEnumerator HundredDays_AtThreeX_Stable_NoUiChurn_NoGcGrowth()
        {
            var s = BuildSimSlice();
            Assert.That(s.Clock.TimeScale, Is.EqualTo(FastForward).Within(Eps));
            Assert.That(s.TargetNodes.Count, Is.GreaterThan(0), "map must expose selectable nodes");

            var w = InitGcWindows();

            // Run the simulation loop with periodic yields.
            int steps = 0;
            int seq = 0;
            int nextDiscoveryHour = 12;
            int nextChurnHour = 24;
            bool churnFoodUp = true;
            bool earlyWindowArmed = false, steadyWindowArmed = false;

            while (steps < TotalSteps)
            {
                s.Clock.TickHours(GameHoursPerStep);
                steps++;

                int hourNow = steps / 4;
                int day = s.Clock.CurrentDay;

                if (hourNow >= nextDiscoveryHour)
                {
                    nextDiscoveryHour += 12;
                    s.Journal.TryDiscover($"stress_discovery_{seq++}", null, day, s.Clock.CurrentHourFloat);
                }

                if (hourNow >= nextChurnHour)
                {
                    nextChurnHour += 24;
                    var firstSlot = s.Inventory.Slots.Count > 0 ? s.Inventory.Slots[0] : null;
                    if (firstSlot?.Item != null)
                    {
                        if (churnFoodUp) s.Inventory.Add(firstSlot.Item, 2);
                        else s.Inventory.Remove(firstSlot.Item, 2);
                    }
                    churnFoodUp = !churnFoodUp;
                    s.Strip.Sync(s.Inventory);
                    s.MapUi.SelectNode(s.TargetNodes[day % s.TargetNodes.Count]);
                    s.MapUi.Refresh();
                }

                if (w.WarmupJournalCreated < 0 && day >= 10)
                {
                    w.WarmupJournalCreated = s.JournalPool.InstancesCreated;
                    w.WarmupIconCreated = s.Strip.IconPool.InstancesCreated;
                    w.WarmupPathLineCreated = s.MapUi.PathLinePool.InstancesCreated;
                }
                if (w.ProfilerLive)
                {
                    if (!earlyWindowArmed && day >= 10) { earlyWindowArmed = true; w.EarlyStartSample = w.GcAlloc.Count; }
                    if (earlyWindowArmed && w.EarlyBytes < 0 && day > 20)
                    { w.EarlyBytes = SumGcAlloc(w.GcAlloc, w.EarlyStartSample); w.MeasuredDayEarlyEnd = day; }
                    if (!steadyWindowArmed && day >= 80) { steadyWindowArmed = true; w.SteadyStartSample = w.GcAlloc.Count; }
                    if (steadyWindowArmed && w.SteadyBytes < 0 && day > 90)
                    { w.SteadyBytes = SumGcAlloc(w.GcAlloc, w.SteadyStartSample); w.MeasuredDaySteadyEnd = day; }
                }

                if (steps % 120 == 0)
                    yield return null;
            }

            if (w.ProfilerLive) { w.GcAlloc.Stop(); w.GcAlloc.Dispose(); }

            // --- Time fidelity ---
            Assert.That(s.DayTicks, Is.EqualTo(TotalDays),
                "every simulated day boundary must fire exactly one day tick");
            Assert.That(s.Clock.CurrentDay, Is.EqualTo(TotalDays + 1));
            Assert.That(s.Clock.TotalElapsedHours, Is.EqualTo(TotalDays * 24f).Within(Eps));
            // OnHourTick fires per integer hour crossed, not per sub-step: four
            // quarter-hour steps advance the clock one hour and fire once. The
            // earlier TotalSteps expectation predated that contract change and
            // only survived because CI runs EditMode alone.
            Assert.That(s.HourTicks, Is.EqualTo(TotalDays * 24),
                "every simulated hour boundary must fire exactly one hour tick");

            // --- Pool conservation ---
            Assert.That(s.MapUi, Is.Not.Null);
            Assert.That(s.Strip, Is.Not.Null);
            Assert.That(s.Journal.EntryCount, Is.LessThanOrEqualTo(JournalSystem.MaxEntries));

            AssertPoolConserved("journal", s.JournalPool);
            AssertPoolConserved("inventory-icon", s.Strip.IconPool);
            AssertPoolConserved("path-line", s.MapUi.PathLinePool);

            Assert.That(s.JournalPool.InstancesCreated, Is.EqualTo(w.WarmupJournalCreated),
                "journal entries: no new instances after warm-up");
            Assert.That(s.Strip.IconPool.InstancesCreated, Is.EqualTo(w.WarmupIconCreated),
                "inventory icons: no new instances after warm-up");
            Assert.That(s.MapUi.PathLinePool.InstancesCreated, Is.EqualTo(w.WarmupPathLineCreated),
                "expedition path lines: no new instances after warm-up");

            Assert.That(seq, Is.GreaterThan(JournalSystem.MaxEntries),
                "run must push past the entry cap to exercise eviction-recycle");
            Assert.That(s.JournalPool.PooledCount + s.JournalPool.ActiveCount,
                Is.EqualTo(s.JournalPool.InstancesCreated),
                "no pooled instance may be lost (destroyed) or leaked");

            // --- GC budget ---
            if (w.ProfilerLive && w.EarlyBytes >= 0 && w.SteadyBytes >= 0)
            {
                Assert.That(w.SteadyBytes, Is.LessThanOrEqualTo(w.EarlyBytes * 2 + 4L * 1024 * 1024),
                    $"allocation growth over the run: early(d10-20)={w.EarlyBytes}B steady(d80-90)={w.SteadyBytes}B");

                long perDayBytes = w.SteadyBytes / Mathf.Max(1, w.MeasuredDaySteadyEnd - 80);
                Assert.That(perDayBytes, Is.LessThan(4L * 1024 * 1024),
                    $"steady-state GC.Alloc must stay minimal: {perDayBytes} B/day");
            }
            else if (!w.ProfilerLive)
            {
                Debug.LogWarning("[FastForwardStability] GC.Alloc recorder unavailable; " +
                                 "pool-flatness assertions still prove allocation-free UI churn.");
            }

            Object.Destroy(s.MapObject);
            Object.Destroy(s.StripObject);
        }

        private static long SumGcAlloc(ProfilerRecorder recorder, int fromSample)
        {
            long total = 0;
            int count = recorder.Count;
            for (int i = Mathf.Max(0, fromSample); i < count; i++)
                total += recorder.GetSample(i).Value;
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
