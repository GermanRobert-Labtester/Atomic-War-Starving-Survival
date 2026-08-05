using System.Collections.Generic;
using NUnit.Framework;
using AtomicWar._Game.Core;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Shelter;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// H-5: SystemRegistry tests. Verifies that the registry correctly
    /// categorizes systems, detects dead (constructed-but-unticked) systems,
    /// and that day-gating works idempotently.
    /// </summary>
    [TestFixture]
    public class SystemRegistryTests
    {
        private SystemRegistry _registry;

        [SetUp]
        public void SetUp()
        {
            _registry = new SystemRegistry();
        }

        [TearDown]
        public void TearDown()
        {
            _registry?.Clear();
        }

        // -----------------------------------------------------------------
        // Registration
        // -----------------------------------------------------------------

        [Test]
        public void RegisterPerSubstep_IncreasesCount()
        {
            _registry.RegisterPerSubstep("test_a", _ => { });
            _registry.RegisterPerSubstep("test_b", _ => { });

            Assert.AreEqual(2, _registry.PerSubstepCount);
        }

        [Test]
        public void RegisterDaily_IncreasesCount()
        {
            _registry.RegisterDaily("daily_a", _ => { });
            _registry.RegisterDaily("daily_b", _ => { });

            Assert.AreEqual(2, _registry.DailyCount);
        }

        [Test]
        public void Register_NullTick_DoesNotAdd()
        {
            _registry.RegisterPerSubstep("null_test", null);

            Assert.AreEqual(0, _registry.PerSubstepCount,
                "Null delegates should be silently ignored.");
        }

        // -----------------------------------------------------------------
        // Tick dispatch
        // -----------------------------------------------------------------

        [Test]
        public void TickAll_RunsPerSubstepTicks()
        {
            int callCount = 0;
            _registry.RegisterPerSubstep("counter", _ => callCount++);

            _registry.TickAll(1f, 1);

            Assert.AreEqual(1, callCount);
        }

        [Test]
        public void TickAll_RunsDailyTicks_OncePerDay()
        {
            int callCount = 0;
            _registry.RegisterDaily("daily_counter", _ => callCount++);

            // Day 1, two substeps — daily should only fire on the first substep.
            _registry.TickAll(1f, 1);
            _registry.TickAll(1f, 1);

            Assert.AreEqual(1, callCount,
                "Daily tick should only fire once per day.");

            // Day 2 — daily should fire again.
            _registry.TickAll(1f, 2);

            Assert.AreEqual(2, callCount,
                "Daily tick should fire again on the next day.");
        }

        [Test]
        public void TickAll_TracksDailyRunCount()
        {
            _registry.RegisterDaily("d", _ => { });

            _registry.TickAll(1f, 1);
            _registry.TickAll(1f, 2);
            _registry.TickAll(1f, 2); // Duplicate day

            Assert.AreEqual(2, _registry.DailyRunCount,
                "DailyRunCount should reflect unique days, not substep count.");
            Assert.AreEqual(2, _registry.LastDailyRunDay);
        }

        [Test]
        public void TickAll_ZeroGameHours_Skips()
        {
            int callCount = 0;
            _registry.RegisterPerSubstep("skip_test", _ => callCount++);

            _registry.TickAll(0f, 1);

            Assert.AreEqual(0, callCount,
                "Zero game-hours should skip all ticks.");
        }

        // -----------------------------------------------------------------
        // DayGated helper
        // -----------------------------------------------------------------

        [Test]
        public void DayGated_FiresOncePerDay()
        {
            int fireCount = 0;
            var gated = _registry.DayGated("gated_test", day => fireCount++);
            _registry.RegisterPerSubstep("gated_test", gated);

            // 3 substeps on day 1 — should fire once.
            _registry.TickAll(1f, 1);
            _registry.TickAll(1f, 1);
            _registry.TickAll(1f, 1);

            Assert.AreEqual(1, fireCount,
                "DayGated delegate should only fire once per day.");

            // Day 2 — should fire again.
            _registry.TickAll(1f, 2);

            Assert.AreEqual(2, fireCount);
        }

        [Test]
        public void DayGated_DifferentKeys_Independent()
        {
            int aFires = 0, bFires = 0;
            _registry.RegisterPerSubstep("ga", _registry.DayGated("a", _ => aFires++));
            _registry.RegisterPerSubstep("gb", _registry.DayGated("b", _ => bFires++));

            _registry.TickAll(1f, 1);

            Assert.AreEqual(1, aFires);
            Assert.AreEqual(1, bFires);
        }

        // -----------------------------------------------------------------
        // Diagnostics: IsSystemTicked
        // -----------------------------------------------------------------

        [Test]
        public void IsSystemTicked_PerSubstep_ReturnsTrue()
        {
            _registry.RegisterPerSubstep("weather", _ => { });

            Assert.IsTrue(_registry.IsSystemTicked("weather"));
        }

        [Test]
        public void IsSystemTicked_Daily_ReturnsTrue()
        {
            _registry.RegisterDaily("ecosystem", _ => { });

            Assert.IsTrue(_registry.IsSystemTicked("ecosystem"));
        }

        [Test]
        public void IsSystemTicked_EventDriven_ReturnsTrue()
        {
            _registry.RegisterEventDriven("excavation");

            Assert.IsTrue(_registry.IsSystemTicked("excavation"));
        }

        [Test]
        public void IsSystemTicked_SaveOnly_ReturnsTrue()
        {
            _registry.RegisterSaveOnly("knowledge_map");

            Assert.IsTrue(_registry.IsSystemTicked("knowledge_map"));
        }

        [Test]
        public void IsSystemTicked_Unknown_ReturnsFalse()
        {
            Assert.IsFalse(_registry.IsSystemTicked("nonexistent_system"));
        }

        // -----------------------------------------------------------------
        // TickedSystemCount
        // -----------------------------------------------------------------

        [Test]
        public void TickedSystemCount_AggregatesAllCategories()
        {
            _registry.RegisterPerSubstep("a", _ => { });
            _registry.RegisterDaily("b", _ => { });
            _registry.RegisterEventDriven("c");
            _registry.RegisterSaveOnly("d");

            Assert.AreEqual(4, _registry.TickedSystemCount);
        }

        // -----------------------------------------------------------------
        // Clear
        // -----------------------------------------------------------------

        [Test]
        public void Clear_ResetsAllState()
        {
            _registry.RegisterPerSubstep("x", _ => { });
            _registry.RegisterDaily("y", _ => { });
            _registry.TickAll(1f, 5);

            _registry.Clear();

            Assert.AreEqual(0, _registry.PerSubstepCount);
            Assert.AreEqual(0, _registry.DailyCount);
            Assert.AreEqual(0, _registry.TickedSystemCount);
            Assert.AreEqual(0, _registry.DailyRunCount);
            Assert.AreEqual(-1, _registry.LastDailyRunDay);
        }

        // -----------------------------------------------------------------
        // Name lists
        // -----------------------------------------------------------------

        [Test]
        public void GetPerSubstepNames_ReturnsRegisteredNames()
        {
            _registry.RegisterPerSubstep("alpha", _ => { });
            _registry.RegisterPerSubstep("beta", _ => { });

            var names = _registry.GetPerSubstepNames();

            CollectionAssert.Contains(names, "alpha");
            CollectionAssert.Contains(names, "beta");
            Assert.AreEqual(2, names.Count);
        }

        [Test]
        public void GetDailyNames_ReturnsRegisteredNames()
        {
            _registry.RegisterDaily("gamma", _ => { });

            var names = _registry.GetDailyNames();

            CollectionAssert.Contains(names, "gamma");
            Assert.AreEqual(1, names.Count);
        }
    }
}
