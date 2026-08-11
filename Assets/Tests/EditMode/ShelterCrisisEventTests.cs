using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Data;
using AtomicWar._Game.Events;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Audit H-6g — GameBootstrap.HandleMutinyChoiceApplied and
    /// HandleCOLeakChoiceApplied are private bootstrap dispatchers that
    /// can't be exercised without a full GameBootstrap instance (no
    /// precedent for standing one up in EditMode — see
    /// BloodForWaterEventTests, which simulates the bootstrap-only side
    /// effects instead of calling the handler directly). These tests pin
    /// the GameEvent contract those dispatchers depend on: the exact
    /// ChoiceIds the mutiny switch matches on, and the exact Effects shape
    /// the CO-leak handler sums into ResolveCoLeakEvent's healthLost.
    /// </summary>
    [TestFixture]
    public class ShelterCrisisEventTests
    {
        private GameEvent _mutiny;
        private GameEvent _coLeak;

        [SetUp]
        public void SetUp()
        {
            _mutiny = EncounterEventFactory.CreateMutinyStandoff();
            _coLeak = EncounterEventFactory.CreateCOLeak();
        }

        [TearDown]
        public void TearDown()
        {
            if (_mutiny != null) Object.DestroyImmediate(_mutiny);
            if (_coLeak != null) Object.DestroyImmediate(_coLeak);
        }

        [Test]
        public void MutinyStandoff_HasExpectedShapeForDispatcherSwitch()
        {
            Assert.AreEqual("shelter_mutiny_standoff", _mutiny.id);
            Assert.AreEqual(0f, _mutiny.weight,
                "Must stay out of the weighted random pool — fired only by MutinySystem.OnMutinyStarted.");
            Assert.IsNotNull(_mutiny.choices);
            Assert.AreEqual(3, _mutiny.choices.Count);

            // GameBootstrap.HandleMutinyChoiceApplied switches on these exact
            // strings; a drift here silently no-ops a resolution path.
            var ids = new List<string>();
            foreach (var c in _mutiny.choices) ids.Add(c.ChoiceId);
            CollectionAssert.AreEquivalent(
                new[] { "mutiny_negotiate", "mutiny_yield", "mutiny_execute" }, ids);
        }

        [Test]
        public void COLeak_VentilateChoice_DerivesZeroHealthLostAndVentilatedTrue()
        {
            var choice = FindChoice(_coLeak, "ventilate");
            Assert.IsNotNull(choice);

            var (healthLost, ventilated) = DeriveCoLeakResolution(choice);

            // Must match the values RebuildersHostTests.Atmosphere_CoLeak_UnlocksDeepDelver
            // already exercises directly against ResolveCoLeakEvent.
            Assert.AreEqual(0f, healthLost);
            Assert.IsTrue(ventilated);
        }

        [Test]
        public void COLeak_IgnoreChoice_DerivesHealthLostAndVentilatedFalse()
        {
            var choice = FindChoice(_coLeak, "ignore_co");
            Assert.IsNotNull(choice);

            var (healthLost, ventilated) = DeriveCoLeakResolution(choice);

            Assert.AreEqual(25f, healthLost);
            Assert.IsFalse(ventilated);
        }

        private static EventChoice FindChoice(GameEvent ev, string choiceId)
        {
            for (int i = 0; i < ev.choices.Count; i++)
                if (ev.choices[i] != null && ev.choices[i].ChoiceId == choiceId)
                    return ev.choices[i];
            return null;
        }

        /// <summary>
        /// Mirrors GameBootstrap.HandleCOLeakChoiceApplied's derivation exactly:
        /// ventilated iff the choice is "ventilate"; healthLost is the summed
        /// magnitude of negative "health" effects.
        /// </summary>
        private static (float healthLost, bool ventilated) DeriveCoLeakResolution(EventChoice choice)
        {
            bool ventilated = choice.ChoiceId == "ventilate";
            float healthLost = 0f;
            if (choice.Effects != null)
            {
                for (int i = 0; i < choice.Effects.Count; i++)
                {
                    var eff = choice.Effects[i];
                    if (eff != null && eff.TargetNeed == "health" && eff.NeedDelta < 0f)
                        healthLost += -eff.NeedDelta;
                }
            }
            return (healthLost, ventilated);
        }
    }
}
