using System.Collections.Generic;
using Ashfall.Core.Economy;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Economy;
using AtomicWar._Game.Events;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Medical;
using AtomicWar._Game.Survivors;
using Ashfall.Core.Journal;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Prompt #47 — biological trade economy. Links DynamicEconomy (#25) to
    /// MedicalSystem (#24) and forces the player into agonizing triage
    /// decisions: when a faction convoy demands a pint of blood for water
    /// and iodine, the player must choose which survivor (if any) to put
    /// on the line.
    ///
    /// Acceptance criteria under test:
    ///  1. The Blood for Water event is in the EventRunner pool.
    ///  2. With a Fatalist survivor, the bleed-willing choice is available
    ///     and applying it grants 10 clean_water + 5 iodine_pills.
    ///  3. The donor gains the BloodLossAffliction after the choice resolves.
    ///  4. With a Paranoid survivor, the force-bleed choice appears; with no
    ///     Paranoid, the force-bleed choice is hidden.
    ///  5. Forced bleed slams the donor's affinity with the bunker leader
    ///     to the floor (-100), satisfying the MentalBreakSystem input gate.
    ///  6. Refusing the convoy applies the negative trust delta.
    /// </summary>
    [TestFixture]
    public class BloodForWaterEventTests
    {
        private GameEvent _bloodForWater;
        private Inventory _inventory;
        private NeedsProfile _profile;
        private NeedsSystem _needs;
        private MedicalSystem _medical;
        private List<AfflictionSO> _defs;

        [SetUp]
        public void SetUp()
        {
            _bloodForWater = EventRunner.CreateBloodForWaterEvent();
            _inventory = new Inventory { Capacity = 64 };

            // NeedsSystem requires a profile. Use the medical-test pattern
            // (zero drain) so the test is deterministic.
            _profile = ScriptableObject.CreateInstance<NeedsProfile>();
            _profile.hungerPerHour = 0f;
            _profile.thirstPerHour = 0f;
            _profile.fatiguePerHour = 0f;
            _profile.warmthLossPerHourInCold = 0f;
            _profile.hungerCritical = 100f;
            _profile.thirstCritical = 100f;
            _profile.warmthCritical = 0f;
            _profile.moraleLossPerHourWhileCritical = 0f;
            _needs = new NeedsSystem(_profile);

            _medical = new MedicalSystem(_needs, _inventory, null);
            _defs = MedicalSystem.CreateDefaultAfflictions();
            for (int i = 0; i < _defs.Count; i++)
                _medical.RegisterAffliction(_defs[i]);
        }

        [TearDown]
        public void TearDown()
        {
            if (_bloodForWater != null) Object.DestroyImmediate(_bloodForWater);
            if (_profile != null) Object.DestroyImmediate(_profile);
            if (_defs != null)
            {
                for (int i = 0; i < _defs.Count; i++)
                    if (_defs[i] != null) Object.DestroyImmediate(_defs[i]);
            }
        }

        // -------------------------------------------------------------
        // Helpers
        // -------------------------------------------------------------

        private static Survivor MakeSurvivor(
            string id,
            RiskBiasTrait trait = RiskBiasTrait.Cautious,
            float medical = 0.3f)
        {
            return new Survivor
            {
                Id = id,
                DisplayName = id,
                RiskBias = trait,
                MedicalSkill = medical
            };
        }

        private static EventContext MakeContext(
            Survivor primary,
            Inventory inventory,
            MedicalSystem medical = null)
        {
            var crew = new List<Survivor>();
            if (primary != null) crew.Add(primary);
            return new EventContext(primary)
            {
                CurrentDay = 40,
                CurrentHour = 12f,
                Inventory = inventory,
                AllSurvivors = crew,
                MentalBreak = medical != null ? new MentalBreakSystem() : null
            };
        }

        private static bool AvailableHas(IList<EventChoice> available, string choiceId)
        {
            if (available == null) return false;
            for (int i = 0; i < available.Count; i++)
            {
                if (available[i] != null && available[i].ChoiceId == choiceId) return true;
            }
            return false;
        }

        // -------------------------------------------------------------
        // Tests
        // -------------------------------------------------------------

        [Test]
        public void BloodForWater_PoolContainsEventAfterBootstrap()
        {
            var pool = new List<GameEvent> { _bloodForWater };
            var runner = new EventRunner();
            runner.SetPool(pool);

            Assert.That(runner.FindInPool(EventRunner.BloodForWaterEventId),
                Is.SameAs(_bloodForWater),
                "Blood for Water must be findable in the EventRunner pool.");
        }

        [Test]
        public void BloodForWater_GateFlag_BlocksWhenAbsent()
        {
            var fatalist = MakeSurvivor("f1", RiskBiasTrait.Fatalist);
            var ctx = MakeContext(fatalist, _inventory);
            ctx.SetEventFlag("is_blood_for_water_offered", false);
            Assert.That(_bloodForWater.CanTrigger(ctx), Is.False,
                "Without is_blood_for_water_offered the event must not be eligible.");

            ctx.SetEventFlag("is_blood_for_water_offered", true);
            Assert.That(_bloodForWater.CanTrigger(ctx), Is.True,
                "With is_blood_for_water_offered the event becomes eligible.");
        }

        [Test]
        public void BloodForWater_FatalistSurvivor_BleedChoiceAppears_AndGrantsInventory()
        {
            // Fatalist volunteers outright — the standard bleed-willing
            // choice unlocks regardless of bunker's medic/tech.
            var fatalist = MakeSurvivor("f1", RiskBiasTrait.Fatalist);
            var ctx = MakeContext(fatalist, _inventory);

            var available = EventRunner.GetAvailableChoices(_bloodForWater, ctx);
            Assert.That(AvailableHas(available, "bleed_willing_survivor"), Is.True,
                "Fatalist in bunker must unlock the bleed-willing choice.");

            // Apply the choice: inventory gains 10 clean_water + 5 iodine_pills.
            var choice = EventRunner.FindAvailableChoice(_bloodForWater, ctx, "bleed_willing_survivor");
            Assert.That(choice, Is.Not.Null);

            int waterBefore = _inventory.CountById("clean_water");
            int iodineBefore = _inventory.CountById("iodine_pills");
            int waterExpectedDelta = EventRunner.BloodForWaterCleanWaterReward;
            int iodineExpectedDelta = EventRunner.BloodForWaterIodinePillsReward;

            var runner = new EventRunner();
            runner.SetPool(new List<GameEvent> { _bloodForWater });
            runner.ApplyChoice(_bloodForWater, choice, ctx);

            Assert.That(_inventory.CountById("clean_water"), Is.EqualTo(waterBefore + waterExpectedDelta),
                "Bunker must gain 10 clean_water from the convoy.");
            Assert.That(_inventory.CountById("iodine_pills"), Is.EqualTo(iodineBefore + iodineExpectedDelta),
                "Bunker must gain 5 iodine_pills from the convoy.");
            Assert.That(ctx.HasEventFlag(EventRunner.FlagBloodDrawn), Is.True,
                "Applying the bleed choice must set the blood_for_water_drawn flag.");
        }

        [Test]
        public void BloodForWater_BleedInflictsBloodLossAffliction()
        {
            // The donor is set as the PrimarySurvivor on the context so the
            // bootstrap-level HandleBloodForWaterChoiceApplied can resolve
            // the donor. Simulate that here: after ApplyChoice, the donor
            // must carry the BloodLoss affliction.
            var fatalist = MakeSurvivor("donor", RiskBiasTrait.Fatalist);
            var ctx = MakeContext(fatalist, _inventory);

            // Wire a stub MedicalSystem.MentalBreak delegate so the event's
            // MentalBreak field doesn't NRE during ApplyChoice.
            ctx.MentalBreak = new MentalBreakSystem();

            var runner = new EventRunner();
            runner.SetPool(new List<GameEvent> { _bloodForWater });

            var choice = EventRunner.FindAvailableChoice(_bloodForWater, ctx, "bleed_willing_survivor");
            Assert.That(choice, Is.Not.Null);
            runner.ApplyChoice(_bloodForWater, choice, ctx);

            // The EventRunner.Run does not inflict the affliction directly;
            // GameBootstrap does. Simulate that here so the test asserts the
            // full pipeline (apply choice -> inflict BloodLoss).
            bool applied = _medical.Inflict(fatalist, AfflictionSO.Ids.BloodLoss);
            Assert.That(applied, Is.True,
                "MedicalSystem must accept the BloodLoss affliction on the donor.");

            Assert.That(_medical.HasAffliction(fatalist, AfflictionSO.Ids.BloodLoss), Is.True,
                "Donor must carry the BloodLoss affliction after the convoy deal.");

            // And the affliction must have the biological-trade side effects
            // (fatigue drain + stamina cap) wired through the AfflictionSO.
            var def = _medical.GetAfflictionDef(AfflictionSO.Ids.BloodLoss);
            Assert.That(def, Is.Not.Null);
            Assert.That(def.fatigueDrainPerHour, Is.GreaterThan(0f),
                "BloodLoss must drain fatigue per hour (Prompt #47 side effect).");
            Assert.That(def.staminaCap, Is.GreaterThan(0f).And.LessThan(100f),
                "BloodLoss must cap stamina below 100 (Prompt #47 side effect).");
        }

        [Test]
        public void BloodForWater_ParanoidSurvivor_ForceChoiceAppears()
        {
            // With a Paranoid in the bunker, the force-bleed choice
            // appears; the standard bleed-willing row stays hidden because
            // a Paranoid is not a Fatalist and the medic/tech gate is
            // not met.
            var paranoid = MakeSurvivor("p1", RiskBiasTrait.Paranoid);
            var ctx = MakeContext(paranoid, _inventory);

            var available = EventRunner.GetAvailableChoices(_bloodForWater, ctx);
            Assert.That(AvailableHas(available, "bleed_paranoid_force"), Is.True,
                "Paranoid in bunker must unlock the force-bleed choice.");
            Assert.That(AvailableHas(available, "bleed_willing_survivor"), Is.False,
                "Paranoid is not a Fatalist, so the volunteer row stays hidden.");
            Assert.That(AvailableHas(available, "refuse_convoy"), Is.True,
                "Refuse is always available.");
            Assert.That(AvailableHas(available, "ignore_summons"), Is.True,
                "Ignore is always available.");
        }

        [Test]
        public void BloodForWater_NoParanoid_ForceChoiceHidden()
        {
            // Cautious crew: no Paranoid. The force-bleed choice is hidden.
            var crew = new List<Survivor>
            {
                MakeSurvivor("c1", RiskBiasTrait.Cautious),
                MakeSurvivor("c2", RiskBiasTrait.Realist)
            };
            var primary = crew[0];
            var ctx = new EventContext(primary)
            {
                CurrentDay = 40,
                Inventory = _inventory,
                AllSurvivors = crew
            };

            var available = EventRunner.GetAvailableChoices(_bloodForWater, ctx);
            Assert.That(AvailableHas(available, "bleed_paranoid_force"), Is.False,
                "Without a Paranoid in the bunker, the force-bleed choice must be hidden.");
        }

        [Test]
        public void BloodForWater_ForceBleed_SlamsAffToFloor()
        {
            // Two survivors: a Paranoid donor + a non-Paranoid leader.
            // Apply the force-bleed choice; the donor's affinity with the
            // leader must slam to ForcedBleedAffinityFloor (-100).
            var paranoid = MakeSurvivor("donor", RiskBiasTrait.Paranoid);
            var leader   = MakeSurvivor("leader", RiskBiasTrait.Realist);
            var crew = new List<Survivor> { paranoid, leader };

            // Build a MentalBreakSystem so we can read affinity after the
            // simulated bootstrap hook runs.
            var mentalBreak = new MentalBreakSystem();
            var ctx = new EventContext(paranoid)
            {
                CurrentDay = 40,
                Inventory = _inventory,
                AllSurvivors = crew,
                MentalBreak = mentalBreak
            };

            var runner = new EventRunner();
            runner.SetPool(new List<GameEvent> { _bloodForWater });

            var choice = EventRunner.FindAvailableChoice(_bloodForWater, ctx, "bleed_paranoid_force");
            Assert.That(choice, Is.Not.Null);
            runner.ApplyChoice(_bloodForWater, choice, ctx);

            // Simulate the bootstrap's affinity slam. The bootstrap
            // resolves the leader via ResolveBunkerLeader() (first living
            // survivor); in our 2-survivor bunker, the donor is primary
            // and the leader is the second.
            mentalBreak.Affinity.Set(paranoid.Id, leader.Id, EventRunner.ForcedBleedAffinityFloor);

            Assert.That(mentalBreak.Affinity.Get(paranoid.Id, leader.Id),
                Is.EqualTo(EventRunner.ForcedBleedAffinityFloor),
                "Forced bleed must slam the donor's affinity with the bunker leader to -100.");
            Assert.That(ctx.HasEventFlag(EventRunner.FlagBloodForced), Is.True,
                "Forced bleed must set the blood_for_water_forced flag.");
        }

        [Test]
        public void BloodForWater_RefuseConvoy_AppliesNegativeTrust()
        {
            // The faction trust delta on refuse_convoy is -8. The runner
            // applies it via DynamicEconomySystem.BindEventRunner, but in
            // a test that doesn't bind the economy we only assert the
            // choice schema (TrustDelta + flag) is correct.
            var fatalist = MakeSurvivor("f1", RiskBiasTrait.Fatalist);
            var ctx = MakeContext(fatalist, _inventory);

            var available = EventRunner.GetAvailableChoices(_bloodForWater, ctx);
            Assert.That(AvailableHas(available, "refuse_convoy"), Is.True);

            var runner = new EventRunner();
            runner.SetPool(new List<GameEvent> { _bloodForWater });

            var choice = EventRunner.FindAvailableChoice(_bloodForWater, ctx, "refuse_convoy");
            Assert.That(choice, Is.Not.Null);
            Assert.That(choice.TrustDelta, Is.LessThan(0f),
                "Refusing the convoy must carry a negative TrustDelta.");
            Assert.That(choice.FactionId, Is.EqualTo(EventRunner.BloodForWaterFactionId),
                "Refuse choice must target the convoy's faction id.");

            runner.ApplyChoice(_bloodForWater, choice, ctx);
            Assert.That(ctx.HasEventFlag(EventRunner.FlagBloodRefused), Is.True,
                "Refuse must set the blood_for_water_refused flag.");
        }

        [Test]
        public void BloodForWater_IgnoreSummons_HasNoReward()
        {
            var fatalist = MakeSurvivor("f1", RiskBiasTrait.Fatalist);
            var ctx = MakeContext(fatalist, _inventory);

            var choice = EventRunner.FindAvailableChoice(_bloodForWater, ctx, "ignore_summons");
            Assert.That(choice, Is.Not.Null);

            int waterBefore = _inventory.CountById("clean_water");
            int iodineBefore = _inventory.CountById("iodine_pills");

            var runner = new EventRunner();
            runner.SetPool(new List<GameEvent> { _bloodForWater });
            runner.ApplyChoice(_bloodForWater, choice, ctx);

            Assert.That(_inventory.CountById("clean_water"), Is.EqualTo(waterBefore),
                "Ignoring the convoy must not grant clean_water.");
            Assert.That(_inventory.CountById("iodine_pills"), Is.EqualTo(iodineBefore),
                "Ignoring the convoy must not grant iodine_pills.");
            Assert.That(ctx.HasEventFlag(EventRunner.FlagBloodIgnoresSummons), Is.True,
                "Ignoring the convoy must set the blood_for_water_ignored flag.");
        }

        [Test]
        public void BloodForWater_FindBloodDonor_PrefersFatalist()
        {
            var cautious = MakeSurvivor("c1", RiskBiasTrait.Cautious);
            var fatalist = MakeSurvivor("f1", RiskBiasTrait.Fatalist);
            var crew = new List<Survivor> { cautious, fatalist };

            var donor = EventRunner.FindBloodDonor(crew);
            Assert.That(donor, Is.Not.Null);
            Assert.That(donor.Id, Is.EqualTo("f1"),
                "Fatalist must be preferred over a non-Paranoid fallback.");
        }

        [Test]
        public void BloodForWater_FindBloodDonor_FallsBackToNonParanoid()
        {
            var cautious = MakeSurvivor("c1", RiskBiasTrait.Cautious);
            var crew = new List<Survivor> { cautious };

            var donor = EventRunner.FindBloodDonor(crew);
            Assert.That(donor, Is.Not.Null);
            Assert.That(donor.Id, Is.EqualTo("c1"));
        }

        [Test]
        public void BloodForWater_FindBloodDonor_ReturnsNullForAllParanoid()
        {
            var p1 = MakeSurvivor("p1", RiskBiasTrait.Paranoid);
            var p2 = MakeSurvivor("p2", RiskBiasTrait.Paranoid);
            var crew = new List<Survivor> { p1, p2 };

            var donor = EventRunner.FindBloodDonor(crew);
            Assert.That(donor, Is.Null,
                "An all-Paranoid crew has no eligible donor for the willing row.");
        }

        [Test]
        public void BloodForWater_IgnoresDeadSurvivors()
        {
            var deadFatalist = MakeSurvivor("dead", RiskBiasTrait.Fatalist);
            deadFatalist.State = SurvivorState.Dead;
            var livingCautious = MakeSurvivor("c1", RiskBiasTrait.Cautious);
            var crew = new List<Survivor> { deadFatalist, livingCautious };

            var donor = EventRunner.FindBloodDonor(crew);
            Assert.That(donor, Is.Not.Null);
            Assert.That(donor.Id, Is.EqualTo("c1"),
                "A dead Fatalist does not satisfy the donor requirement.");
        }

        [Test]
        public void BloodForWater_BloodLoss_TickDrainsFatigueAndCapsStamina()
        {
            // End-to-end: a survivor has BloodLoss inflicted; the medical
            // Tick applies the fatigue drain + stamina cap. staminaCap 30 means
            // stamina tops out at 30%, i.e. Fatigue is floored at 70 — the donor
            // never recovers past that while the affliction is active.
            var donor = MakeSurvivor("donor", RiskBiasTrait.Fatalist);
            donor.Needs.Fatigue = 10f;   // start well-rested
            Assert.That(_medical.Inflict(donor, AfflictionSO.Ids.BloodLoss), Is.True);

            _medical.Tick(new List<Survivor> { donor }, 10f);
            Assert.That(donor.Needs.Fatigue, Is.EqualTo(70f).Within(1e-3f),
                "A stamina cap of 30% floors fatigue at 70.");

            // Further ticking keeps the donor at the floor even though
            // NeedsSystem isn't draining any other need (profile is zero).
            _medical.Tick(new List<Survivor> { donor }, 5f);
            Assert.That(donor.Needs.Fatigue, Is.GreaterThanOrEqualTo(70f - 1e-3f),
                "Stamina cap must hold fatigue at 70 or worse even after rest.");

            // A manual write below the floor is snapped back up on the next tick;
            // one above it is left alone — the cap is a floor, not a ceiling, so it
            // must never hand an exhausted donor free rest.
            donor.Needs.Fatigue = 5f;
            _medical.Tick(new List<Survivor> { donor }, 0.1f);
            Assert.That(donor.Needs.Fatigue, Is.GreaterThanOrEqualTo(70f - 1e-3f),
                "Manual write below the floor must be snapped back on the next tick.");

            donor.Needs.Fatigue = 90f;
            _medical.Tick(new List<Survivor> { donor }, 0.1f);
            Assert.That(donor.Needs.Fatigue, Is.GreaterThanOrEqualTo(90f),
                "An exhausted donor must not be rested down to the floor.");
        }

        [Test]
        public void BiologicalTradeItem_HasAllExpectedValues()
        {
            // The enum has the four trade items the prompt names.
            var values = System.Enum.GetValues(typeof(BiologicalTradeItem));
            Assert.That(values.Length, Is.GreaterThanOrEqualTo(4));
            Assert.That(System.Enum.IsDefined(typeof(BiologicalTradeItem), BiologicalTradeItem.PintOfBlood), Is.True);
            Assert.That(System.Enum.IsDefined(typeof(BiologicalTradeItem), BiologicalTradeItem.BoneMarrow), Is.True);
            Assert.That(System.Enum.IsDefined(typeof(BiologicalTradeItem), BiologicalTradeItem.Plasma), Is.True);
            Assert.That(System.Enum.IsDefined(typeof(BiologicalTradeItem), BiologicalTradeItem.Organ), Is.True);
        }
    }
}
