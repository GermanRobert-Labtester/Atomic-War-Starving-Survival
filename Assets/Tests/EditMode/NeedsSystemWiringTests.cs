using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Events;
using AtomicWar._Game.Core;
using AtomicWar._Game.Shelter;
using System.Collections.Generic;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Verify that systems wired with NeedsSystem route through Modify()
    /// instead of falling back to direct Needs.* writes.
    /// </summary>
    [TestFixture]
    public class NeedsSystemWiringTests
    {
        private NeedsSystem _needsSystem;
        private NeedsProfile _profile;
        private Survivor _survivor;
        private bool _needChangedFired;
        private NeedKind _lastChangedKind;
        private float _lastChangedValue;

        [SetUp]
        public void SetUp()
        {
            _profile = ScriptableObject.CreateInstance<NeedsProfile>();
            _profile.hungerPerHour = 1f;
            _profile.thirstPerHour = 1f;
            _profile.fatiguePerHour = 0.5f;
            _profile.warmthLossPerHourInCold = 2f;
            _profile.healthLossFromHunger = 2f;
            _profile.healthLossFromThirst = 3f;
            _profile.healthLossFromCold = 1f;

            _needsSystem = new NeedsSystem(_profile);
            _needChangedFired = false;
            _lastChangedKind = NeedKind.Hunger;
            _lastChangedValue = 0f;
            _needsSystem.OnNeedChanged += (sv, kind, value) =>
            {
                _needChangedFired = true;
                _lastChangedKind = kind;
                _lastChangedValue = value;
            };

            _survivor = new Survivor
            {
                Id = "sv_test",
                DisplayName = "Test Survivor",
                State = SurvivorState.Idle,
                BaseMaxHealth = 100f
            };
            _survivor.Needs.Hunger = 50f;
            _survivor.Needs.Thirst = 50f;
            _survivor.Needs.Fatigue = 30f;
            _survivor.Needs.Warmth = 80f;
            _survivor.Needs.Morale = 60f;
            _survivor.Needs.Health = 100f;
        }

        // ── EventRunner.ApplyChoice routes through Modify ───────────────

        [Test]
        public void EventRunner_ApplyChoice_WithNeedsSystem_RoutesThroughModify()
        {
            var runner = new EventRunner();
            var gameEvent = ScriptableObject.CreateInstance<GameEvent>();
            gameEvent.id = "test_event";

            var choice = new EventChoice
            {
                ChoiceId = "test_choice",
                MoraleDelta = -25f
            };

            var context = new EventContext
            {
                PrimarySurvivor = _survivor,
                NeedsSystem = _needsSystem
            };

            float before = _survivor.Needs.Morale;
            runner.ApplyChoice(gameEvent, choice, context);
            float after = _survivor.Needs.Morale;

            Assert.Less(after, before, "Morale should decrease via Modify");
            Assert.IsTrue(_needChangedFired, "OnNeedChanged must fire through Modify path");
            Assert.AreEqual(NeedKind.Morale, _lastChangedKind);
        }

        [Test]
        public void EventRunner_ApplyChoice_WithoutNeedsSystem_FallsBackToDirectWrite()
        {
            var runner = new EventRunner();
            var gameEvent = ScriptableObject.CreateInstance<GameEvent>();
            gameEvent.id = "test_event";

            var choice = new EventChoice
            {
                ChoiceId = "test_choice",
                MoraleDelta = -25f
            };

            var context = new EventContext
            {
                PrimarySurvivor = _survivor,
                NeedsSystem = null // No NS → direct write fallback
            };

            _needChangedFired = false;
            float before = _survivor.Needs.Morale;
            runner.ApplyChoice(gameEvent, choice, context);
            float after = _survivor.Needs.Morale;

            Assert.Less(after, before, "Morale should decrease via direct write fallback");
            Assert.IsFalse(_needChangedFired,
                "OnNeedChanged should NOT fire when NeedsSystem is null (direct write path)");
        }

        [Test]
        public void EventRunner_ApplyChoice_MoraleDelta_DoesNotUnderflow()
        {
            var runner = new EventRunner();
            var gameEvent = ScriptableObject.CreateInstance<GameEvent>();
            gameEvent.id = "test_event";

            _survivor.Needs.Morale = 10f;
            var choice = new EventChoice
            {
                ChoiceId = "test_choice",
                MoraleDelta = -30f // Would go to -20 without clamping
            };

            var context = new EventContext
            {
                PrimarySurvivor = _survivor,
                NeedsSystem = _needsSystem
            };

            runner.ApplyChoice(gameEvent, choice, context);

            Assert.GreaterOrEqual(_survivor.Needs.Morale, 0f,
                "Morale must not drop below 0");
        }

        // ── NeedsSystem.Modify core behavior ────────────────────────────

        [Test]
        public void Modify_ClampsMoraleTo100()
        {
            _survivor.Needs.Morale = 95f;
            _needsSystem.Modify(_survivor, NeedKind.Morale, 20f);
            Assert.AreEqual(100f, _survivor.Needs.Morale);
        }

        [Test]
        public void Modify_ClampsMoraleTo0()
        {
            _survivor.Needs.Morale = 5f;
            _needsSystem.Modify(_survivor, NeedKind.Morale, -20f);
            Assert.AreEqual(0f, _survivor.Needs.Morale);
        }

        [Test]
        public void Modify_FiresOnNeedChanged()
        {
            _needChangedFired = false;
            _needsSystem.Modify(_survivor, NeedKind.Hunger, 5f);
            Assert.IsTrue(_needChangedFired, "OnNeedChanged must fire");
            Assert.AreEqual(NeedKind.Hunger, _lastChangedKind);
        }

        [Test]
        public void Modify_ZeroDelta_SkipsEvent()
        {
            _needChangedFired = false;
            float before = _survivor.Needs.Hunger;
            _needsSystem.Modify(_survivor, NeedKind.Hunger, 0f);
            Assert.AreEqual(before, _survivor.Needs.Hunger);
            Assert.IsFalse(_needChangedFired, "Zero delta should not fire OnNeedChanged");
        }

        [Test]
        public void Modify_DeadSurvivor_Skips()
        {
            _survivor.State = SurvivorState.Dead;
            _needChangedFired = false;
            float before = _survivor.Needs.Hunger;
            _needsSystem.Modify(_survivor, NeedKind.Hunger, 10f);
            Assert.AreEqual(before, _survivor.Needs.Hunger);
            Assert.IsFalse(_needChangedFired);
        }

        [Test]
        public void Modify_NullSurvivor_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => _needsSystem.Modify(null, NeedKind.Hunger, 10f));
        }

        // ── System injection smoke tests ────────────────────────────────

        [Test]
        public void MoralDilemmaSystem_AcceptsNeedsSystem()
        {
            var sys = new MoralDilemmaSystem();
            Assert.DoesNotThrow(() => sys.SetNeedsSystem(_needsSystem));
        }

        [Test]
        public void HostageSystem_AcceptsNeedsSystem()
        {
            var sys = new HostageSystem();
            Assert.DoesNotThrow(() => sys.SetNeedsSystem(_needsSystem));
        }

        [Test]
        public void CultMoralDisgustSystem_AcceptsNeedsSystem()
        {
            var sys = new CultMoralDisgustSystem();
            Assert.DoesNotThrow(() => sys.SetNeedsSystem(_needsSystem));
        }

        [Test]
        public void GhostStationSystem_AcceptsNeedsSystem()
        {
            var sys = new GhostStationSystem();
            Assert.DoesNotThrow(() => sys.SetNeedsSystem(_needsSystem));
        }

        [Test]
        public void WasteSystem_AcceptsNeedsSystem()
        {
            var sys = new WasteSystem(new System.Random(42));
            Assert.DoesNotThrow(() => sys.SetNeedsSystem(_needsSystem));
        }

        [Test]
        public void AirlockSystem_AcceptsNeedsSystem()
        {
            var sys = new AirlockSystem();
            Assert.DoesNotThrow(() => sys.SetNeedsSystem(_needsSystem));
        }

        [Test]
        public void ExcavationSystem_AcceptsNeedsSystem()
        {
            var sys = new ExcavationSystem(new System.Random(42));
            Assert.DoesNotThrow(() => sys.SetNeedsSystem(_needsSystem));
        }

        [Test]
        public void ChildDependentSystem_AcceptsNeedsSystem()
        {
            var sys = new ChildDependentSystem();
            Assert.DoesNotThrow(() => sys.SetNeedsSystem(_needsSystem));
        }

        [Test]
        public void EmpathSystem_AcceptsNeedsSystem()
        {
            var sys = new EmpathSystem();
            Assert.DoesNotThrow(() => sys.SetNeedsSystem(_needsSystem));
        }

        [Test]
        public void HatchDefenseSystem_AcceptsNeedsSystem()
        {
            var sys = new HatchDefenseSystem(
                getShelter: () => null,
                getInventory: () => null,
                getSurvivors: () => null,
                getDay: () => 1,
                inflictTrauma: (sv, id) => { },
                rng: new System.Random(42));
            Assert.DoesNotThrow(() => sys.SetNeedsSystem(_needsSystem));
        }

        // ── EventRunner.Save/Restore round-trip ─────────────────────────

        [Test]
        public void EventRunner_CooldownState_RoundTrips()
        {
            var runner = new EventRunner();
            runner.DefaultCooldownHours = 24f;

            var gameEvent = ScriptableObject.CreateInstance<GameEvent>();
            gameEvent.id = "test_event";
            runner.SetPool(new List<GameEvent> { gameEvent });
            runner.Run(gameEvent);

            var save = runner.CaptureCooldownState();
            Assert.IsNotNull(save);
            Assert.Greater(save.CooldownKeys.Count, 0, "Cooldown should be captured");

            var runner2 = new EventRunner();
            runner2.RestoreCooldownState(save);
            var restored = runner2.CaptureCooldownState();
            Assert.AreEqual(save.CooldownKeys.Count, restored.CooldownKeys.Count,
                "Restored cooldowns should match");
        }

        [Test]
        public void EventRunner_ActiveConsequences_RoundTrips()
        {
            var runner = new EventRunner();
            var gameEvent = ScriptableObject.CreateInstance<GameEvent>();
            gameEvent.id = "test_event";

            var choice = new EventChoice
            {
                ChoiceId = "test_choice",
                DelayedConsequence = new DelayedConsequence
                {
                    DelayHours = 48f,
                    Title = "Test",
                    Description = "Test consequence"
                }
            };

            var context = new EventContext { PrimarySurvivor = _survivor };
            runner.ApplyChoice(gameEvent, choice, context);

            var save = runner.CaptureCooldownState();
            Assert.IsNotNull(save);
            Assert.Greater(save.ActiveConsequences.Count, 0,
                "Active consequence should be captured");

            var runner2 = new EventRunner();
            runner2.RestoreCooldownState(save);
            var restored = runner2.CaptureCooldownState();
            Assert.AreEqual(save.ActiveConsequences.Count, restored.ActiveConsequences.Count,
                "Restored consequences should match");
        }

        // ── SeededRandom.WorldSeed propagation ──────────────────────────

        [Test]
        public void SeededRandom_WorldSeed_ProducesDifferentStreams()
        {
            AtomicWar._Game.Utilities.SeededRandom.WorldSeed = 42;
            var a = AtomicWar._Game.Utilities.SeededRandom.CreateFixed("test_system");
            var valA = a.Next(1000);

            AtomicWar._Game.Utilities.SeededRandom.WorldSeed = 99;
            var b = AtomicWar._Game.Utilities.SeededRandom.CreateFixed("test_system");
            var valB = b.Next(1000);

            Assert.AreNotEqual(valA, valB,
                "Different world seeds should produce different RNG streams");

            // Reset for other tests
            AtomicWar._Game.Utilities.SeededRandom.WorldSeed = -1;
        }

        [Test]
        public void SeededRandom_DefaultSeed_IsDeterministic()
        {
            AtomicWar._Game.Utilities.SeededRandom.WorldSeed = -1;
            var a = AtomicWar._Game.Utilities.SeededRandom.CreateFixed("test_system");
            var b = AtomicWar._Game.Utilities.SeededRandom.CreateFixed("test_system");

            Assert.AreEqual(a.Next(1000), b.Next(1000),
                "Same salt with default seed should produce same sequence");

            AtomicWar._Game.Utilities.SeededRandom.WorldSeed = -1;
        }
    }
}
