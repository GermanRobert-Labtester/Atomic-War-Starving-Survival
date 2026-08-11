using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Medical;
using AtomicWar._Game.Survivors;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Addiction & Withdrawal Pipeline (Prompt #7): 3 doses in 7 days addicts,
    /// 48h without a dose starts withdrawal, 336h continuous withdrawal breaks it.
    /// </summary>
    [TestFixture]
    public class AddictionSystemTests
    {
        private const float Eps = 1e-3f;

        private static NeedsSystem NewNeedsSystem()
        {
            return new NeedsSystem(ScriptableObject.CreateInstance<NeedsProfile>());
        }

        private static AddictionSystem MakeSystem(System.Random rng = null)
        {
            var sys = new AddictionSystem(rng ?? new FixedRandom(1.0)); // no PanicDestroyHandler set by default, so this rng is inert
            sys.RegisterAddictiveItem("morphine");
            return sys;
        }

        private static Survivor MakeSurvivor(string id = "sv1")
        {
            return new Survivor { Id = id, DisplayName = id };
        }

        [Test]
        public void OnItemConsumed_NonAddictiveItem_NoHistoryRecorded()
        {
            var sys = MakeSystem();
            var sv = MakeSurvivor();

            sys.OnItemConsumed(sv, "bandage", currentDay: 1);

            Assert.AreEqual(0, sv.ConsumptionHistory.Count);
            Assert.IsFalse(sv.HasTrait(AddictionSystem.AddictedTraitId));
        }

        [Test]
        public void OnItemConsumed_TwoDosesInWindow_DoesNotAddictYet()
        {
            var sys = MakeSystem();
            var sv = MakeSurvivor();

            sys.OnItemConsumed(sv, "morphine", currentDay: 1);
            sys.OnItemConsumed(sv, "morphine", currentDay: 3);

            Assert.IsFalse(sv.HasTrait(AddictionSystem.AddictedTraitId));
        }

        [Test]
        public void OnItemConsumed_ThirdDoseWithinWindow_TriggersAddiction()
        {
            var sys = MakeSystem();
            var sv = MakeSurvivor();
            bool addictedFired = false;
            sys.OnAddicted += _ => addictedFired = true;

            sys.OnItemConsumed(sv, "morphine", currentDay: 1);
            sys.OnItemConsumed(sv, "morphine", currentDay: 3);
            sys.OnItemConsumed(sv, "morphine", currentDay: 6);

            Assert.IsTrue(sv.HasTrait(AddictionSystem.AddictedTraitId));
            Assert.IsTrue(addictedFired);
        }

        [Test]
        public void OnItemConsumed_ThirdDoseOutsideRollingWindow_DoesNotAddict()
        {
            var sys = MakeSystem();
            var sv = MakeSurvivor();

            sys.OnItemConsumed(sv, "morphine", currentDay: 1);
            sys.OnItemConsumed(sv, "morphine", currentDay: 3);
            // Day 9 is 8 days after day 1 — outside the 7-day rolling window,
            // so day 1's dose should have been pruned already.
            sys.OnItemConsumed(sv, "morphine", currentDay: 9);

            Assert.IsFalse(sv.HasTrait(AddictionSystem.AddictedTraitId));
        }

        [Test]
        public void OnItemConsumed_ResetsHoursSinceLastDose()
        {
            var sys = MakeSystem();
            var sv = MakeSurvivor();
            sv.HoursSinceLastDose = 30f;

            sys.OnItemConsumed(sv, "morphine", currentDay: 1);

            Assert.AreEqual(0f, sv.HoursSinceLastDose);
        }

        [Test]
        public void Tick_NonAddictedSurvivor_NeverEntersWithdrawal()
        {
            var sys = MakeSystem();
            var sv = MakeSurvivor();

            sys.Tick(1000f, new List<Survivor> { sv }, currentDay: 50);

            Assert.IsFalse(sv.IsInWithdrawal);
        }

        [Test]
        public void Tick_AddictedSurvivor_BelowThreshold_NoWithdrawal()
        {
            var sys = MakeSystem();
            var sv = MakeSurvivor();
            Addict(sys, sv);

            sys.Tick(AddictionSystem.WithdrawalThresholdHours - 1f, new List<Survivor> { sv }, currentDay: 10);

            Assert.IsFalse(sv.IsInWithdrawal);
        }

        [Test]
        public void Tick_AddictedSurvivor_PastThreshold_EntersWithdrawal()
        {
            var sys = MakeSystem();
            var sv = MakeSurvivor();
            Addict(sys, sv);
            bool withdrawalStarted = false;
            sys.OnWithdrawalStarted += _ => withdrawalStarted = true;

            sys.Tick(AddictionSystem.WithdrawalThresholdHours + 1f, new List<Survivor> { sv }, currentDay: 10);

            Assert.IsTrue(sv.IsInWithdrawal);
            Assert.IsTrue(withdrawalStarted);
        }

        [Test]
        public void Tick_Withdrawal_DrainsMoraleHealthAndFatigue()
        {
            var sys = MakeSystem();
            sys.SetNeedsSystem(NewNeedsSystem());
            var sv = MakeSurvivor();
            Addict(sys, sv);
            sv.Needs.Morale = 80f;
            sv.Needs.Health = 100f;
            sv.Needs.Fatigue = 0f;

            // Stay just under the threshold first: no drain yet.
            sys.Tick(AddictionSystem.WithdrawalThresholdHours - 1f, new List<Survivor> { sv }, currentDay: 10);
            Assert.IsFalse(sv.IsInWithdrawal);
            float moraleBefore = sv.Needs.Morale;
            float healthBefore = sv.Needs.Health;
            float fatigueBefore = sv.Needs.Fatigue;

            // A small 1h tick crosses the threshold: drain is proportional to this tick only.
            sys.Tick(1f, new List<Survivor> { sv }, currentDay: 10);

            Assert.IsTrue(sv.IsInWithdrawal);
            Assert.That(sv.Needs.Morale, Is.LessThan(moraleBefore));
            Assert.That(sv.Needs.Health, Is.LessThan(healthBefore));
            Assert.That(sv.Needs.Fatigue, Is.GreaterThan(fatigueBefore));
        }

        [Test]
        public void OnItemConsumed_DuringWithdrawal_EndsWithdrawal_AndResetsRecovery()
        {
            var sys = MakeSystem();
            var sv = MakeSurvivor();
            Addict(sys, sv);
            sys.Tick(AddictionSystem.WithdrawalThresholdHours + 1f, new List<Survivor> { sv }, currentDay: 10);
            Assert.IsTrue(sv.IsInWithdrawal);
            bool withdrawalEnded = false;
            sys.OnWithdrawalEnded += _ => withdrawalEnded = true;

            sys.OnItemConsumed(sv, "morphine", currentDay: 12);

            Assert.IsFalse(sv.IsInWithdrawal);
            Assert.IsTrue(withdrawalEnded);
            Assert.AreEqual(0f, sys.GetRecoveryHours(sv.Id));
        }

        [Test]
        public void Tick_NotYetWithdrawing_ClearsAnyStaleRecoveryProgress()
        {
            var sys = MakeSystem();
            var sv = MakeSurvivor();
            Addict(sys, sv);

            // Not past threshold: recovery hours must stay at zero.
            sys.Tick(1f, new List<Survivor> { sv }, currentDay: 10);

            Assert.AreEqual(0f, sys.GetRecoveryHours(sv.Id));
        }

        [Test]
        public void Tick_FullRecoveryDuration_BreaksAddiction_AndAppliesWithdrawalTrauma()
        {
            var sys = MakeSystem();
            var sv = MakeSurvivor();
            Addict(sys, sv);
            bool addictionBroken = false;
            sys.OnAddictionBroken += _ => addictionBroken = true;

            // Enter withdrawal, then accumulate a full RecoveryHours worth of continuous withdrawal.
            sys.Tick(AddictionSystem.WithdrawalThresholdHours + 1f, new List<Survivor> { sv }, currentDay: 10);
            sys.Tick(AddictionSystem.RecoveryHours, new List<Survivor> { sv }, currentDay: 40);

            Assert.IsFalse(sv.HasTrait(AddictionSystem.AddictedTraitId));
            Assert.IsFalse(sv.IsInWithdrawal);
            Assert.IsTrue(sv.HasTrauma(AddictionSystem.WithdrawalTraumaId));
            Assert.IsTrue(addictionBroken);
            Assert.AreEqual(0f, sys.GetRecoveryHours(sv.Id));
        }

        [Test]
        public void Tick_PanicDestroyHandler_InvokedDuringWithdrawal_WhenRollSucceeds()
        {
            // FixedRandom(0.0) always beats the per-hour panic-destroy chance.
            var sys = new AddictionSystem(new FixedRandom(0.0));
            sys.RegisterAddictiveItem("morphine");
            var sv = MakeSurvivor();
            Addict(sys, sv);

            bool panicked = false;
            sys.PanicDestroyHandler = (s, rng) => { panicked = true; return true; };

            sys.Tick(AddictionSystem.WithdrawalThresholdHours + 1f, new List<Survivor> { sv }, currentDay: 10);

            Assert.IsTrue(panicked);
        }

        [Test]
        public void SaveRestore_RoundTripsRecoveryProgress()
        {
            var sys = MakeSystem();
            var sv = MakeSurvivor();
            Addict(sys, sv);
            sys.Tick(AddictionSystem.WithdrawalThresholdHours + 10f, new List<Survivor> { sv }, currentDay: 10);
            float before = sys.GetRecoveryHours(sv.Id);
            Assert.That(before, Is.GreaterThan(0f));

            var save = sys.CaptureState();
            var restored = new AddictionSystem();
            restored.RestoreState(save);

            Assert.That(restored.GetRecoveryHours(sv.Id), Is.EqualTo(before).Within(Eps));
        }

        private static void Addict(AddictionSystem sys, Survivor sv)
        {
            sys.OnItemConsumed(sv, "morphine", currentDay: 1);
            sys.OnItemConsumed(sv, "morphine", currentDay: 2);
            sys.OnItemConsumed(sv, "morphine", currentDay: 3);
            Assert.IsTrue(sv.HasTrait(AddictionSystem.AddictedTraitId), "Precondition: survivor must be addicted.");
        }

        private sealed class FixedRandom : System.Random
        {
            private readonly double _value;
            public FixedRandom(double value) { _value = value; }
            public override double NextDouble() => _value;
        }
    }
}
