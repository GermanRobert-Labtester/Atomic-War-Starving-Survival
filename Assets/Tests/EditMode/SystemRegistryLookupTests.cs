using NUnit.Framework;
using AtomicWar._Game.Core;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Pins <see cref="SystemRegistry.IsSystemTicked"/> across all four registration
    /// categories. The lookup was changed from a linear scan of the two tick lists to a
    /// single set membership test, because the C-1 diagnostic calls it once per
    /// GameBootstrap system property — making the whole check quadratic in system count.
    /// These tests exist so that optimisation cannot quietly change the answer.
    /// </summary>
    [TestFixture]
    public class SystemRegistryLookupTests
    {
        private SystemRegistry _registry;

        [SetUp]
        public void SetUp() => _registry = new SystemRegistry();

        [Test]
        public void IsSystemTicked_TrueForEveryRegistrationCategory()
        {
            _registry.RegisterPerSubstep("per_substep_one", _ => { });
            _registry.RegisterDaily("daily_one", _ => { });
            _registry.RegisterEventDriven("event_one");
            _registry.RegisterSaveOnly("save_one");

            Assert.IsTrue(_registry.IsSystemTicked("per_substep_one"));
            Assert.IsTrue(_registry.IsSystemTicked("daily_one"));
            Assert.IsTrue(_registry.IsSystemTicked("event_one"));
            Assert.IsTrue(_registry.IsSystemTicked("save_one"));
        }

        [Test]
        public void IsSystemTicked_FalseForUnknownNullAndCaseMismatch()
        {
            _registry.RegisterPerSubstep("weather_events_hourly", _ => { });

            Assert.IsFalse(_registry.IsSystemTicked("never_registered"));
            Assert.IsFalse(_registry.IsSystemTicked(null));
            Assert.IsFalse(_registry.IsSystemTicked("Weather_Events_Hourly"),
                "Lookup is ordinal; registry keys are snake_case by convention.");
        }

        [Test]
        public void RegisterPerSubstep_WithNullTick_DoesNotRegisterTheName()
        {
            // The guard clause returns before adding to the list; the name set must not
            // drift out of step with it, or a system with a null delegate would report
            // as ticked while doing nothing.
            _registry.RegisterPerSubstep("null_tick", null);

            Assert.IsFalse(_registry.IsSystemTicked("null_tick"));
            Assert.AreEqual(0, _registry.PerSubstepCount);
        }

        [Test]
        public void Clear_ResetsMembership()
        {
            _registry.RegisterPerSubstep("a", _ => { });
            _registry.RegisterDaily("b", _ => { });
            _registry.RegisterEventDriven("c");
            _registry.RegisterSaveOnly("d");
            Assume.That(_registry.IsSystemTicked("a"));

            _registry.Clear();

            foreach (string n in new[] { "a", "b", "c", "d" })
                Assert.IsFalse(_registry.IsSystemTicked(n), $"'{n}' must not survive Clear().");
            Assert.AreEqual(0, _registry.TickedSystemCount);
        }

        [Test]
        public void DuplicateRegistration_IsIdempotentForMembership()
        {
            _registry.RegisterPerSubstep("dup", _ => { });
            _registry.RegisterPerSubstep("dup", _ => { });

            Assert.IsTrue(_registry.IsSystemTicked("dup"));
            Assert.AreEqual(2, _registry.PerSubstepCount,
                "Both delegates still dispatch; only the name lookup is deduplicated.");
        }
    }
}
