using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Survivors;

namespace AtomicWar.Tests.EditMode
{
    [TestFixture]
    public class RadiationMutagenesisSystemTests
    {
        private const float Eps = 1e-3f;

        private static NeedsSystem NewNeedsSystem()
        {
            return new NeedsSystem(ScriptableObject.CreateInstance<NeedsProfile>());
        }

        private static Survivor MakeSurvivor(string id, float lifetimeRads)
        {
            return new Survivor { Id = id, DisplayName = id, LifetimeRadiationExposure = lifetimeRads };
        }

        [Test]
        public void Evaluate_BelowThreshold_NoStageChange()
        {
            var sys = new RadiationMutagenesisSystem();
            var sv = MakeSurvivor("sv1", RadiationMutagenesisSystem.Stage1HairLossThreshold - 1f);

            sys.Evaluate(new List<Survivor> { sv });

            Assert.AreEqual(0, sys.GetStage(sv.Id));
            Assert.IsFalse(sys.HasHairLoss(sv.Id));
        }

        [Test]
        public void Evaluate_AtStage1Threshold_TriggersHairLoss_AndMoralePenalty_Once()
        {
            var sys = new RadiationMutagenesisSystem();
            sys.SetNeedsSystem(NewNeedsSystem());
            var sv = MakeSurvivor("sv1", RadiationMutagenesisSystem.Stage1HairLossThreshold);
            sv.Needs.Morale = 80f;

            bool hairLossFired = false;
            sys.OnHairLoss += _ => hairLossFired = true;

            sys.Evaluate(new List<Survivor> { sv });

            Assert.AreEqual(1, sys.GetStage(sv.Id));
            Assert.IsTrue(sys.HasHairLoss(sv.Id));
            Assert.IsTrue(hairLossFired);
            Assert.That(sv.Needs.Morale, Is.EqualTo(80f - RadiationMutagenesisSystem.HairLossMoralePenalty).Within(Eps));

            // Calling Evaluate again at the same exposure must not reapply the morale hit.
            sys.Evaluate(new List<Survivor> { sv });
            Assert.That(sv.Needs.Morale, Is.EqualTo(80f - RadiationMutagenesisSystem.HairLossMoralePenalty).Within(Eps));
        }

        [Test]
        public void Evaluate_AtStage2Threshold_SetsCataractsChronicIllness()
        {
            var sys = new RadiationMutagenesisSystem();
            var sv = MakeSurvivor("sv1", RadiationMutagenesisSystem.Stage2CataractsThreshold);

            bool cataractsFired = false;
            sys.OnCataracts += _ => cataractsFired = true;

            sys.Evaluate(new List<Survivor> { sv });

            Assert.AreEqual(2, sys.GetStage(sv.Id));
            Assert.IsTrue(sys.HasCataracts(sv.Id));
            Assert.IsTrue(cataractsFired);
            Assert.AreEqual(ChronicIllnessKind.RadiationCataracts, sv.ActiveChronicIllness);
        }

        [Test]
        public void Evaluate_AtStage3Threshold_SetsOrganFailure_AndInflictsCellularBreakdown()
        {
            var sys = new RadiationMutagenesisSystem();
            string inflictedId = null;
            sys.Bind(inflictAffliction: (s, id) => inflictedId = id);
            var sv = MakeSurvivor("sv1", RadiationMutagenesisSystem.Stage3CellularThreshold);

            bool cellularFired = false;
            sys.OnCellularBreakdown += _ => cellularFired = true;

            sys.Evaluate(new List<Survivor> { sv });

            Assert.AreEqual(3, sys.GetStage(sv.Id));
            Assert.IsTrue(sys.HasCellularBreakdown(sv.Id));
            Assert.IsTrue(cellularFired);
            Assert.AreEqual(ChronicIllnessKind.OrganFailure, sv.ActiveChronicIllness);
            Assert.AreEqual(RadiationMutagenesisSystem.CellularBreakdownAfflictionId, inflictedId);
        }

        [Test]
        public void Evaluate_JumpingStraightToStage3_AppliesAllIntermediateStages()
        {
            var sys = new RadiationMutagenesisSystem();
            sys.SetNeedsSystem(NewNeedsSystem());
            var sv = MakeSurvivor("sv1", RadiationMutagenesisSystem.Stage3CellularThreshold);

            int advancedCount = 0;
            int lastStage = -1;
            sys.OnStageAdvanced += (_, stage) => { advancedCount++; lastStage = stage; };

            sys.Evaluate(new List<Survivor> { sv });

            // Hair loss and cataracts side effects must both have landed, even though
            // the survivor never lingered at stage 1 or 2.
            Assert.IsTrue(sys.HasHairLoss(sv.Id));
            Assert.IsTrue(sys.HasCataracts(sv.Id));
            Assert.IsTrue(sys.HasCellularBreakdown(sv.Id));
            Assert.AreEqual(1, advancedCount, "OnStageAdvanced fires once per Evaluate call, reporting the final stage.");
            Assert.AreEqual(3, lastStage);
        }

        [Test]
        public void Evaluate_StageNeverRegresses_WhenExposureDrops()
        {
            var sys = new RadiationMutagenesisSystem();
            var sv = MakeSurvivor("sv1", RadiationMutagenesisSystem.Stage2CataractsThreshold);
            sys.Evaluate(new List<Survivor> { sv });
            Assert.AreEqual(2, sys.GetStage(sv.Id));

            sv.LifetimeRadiationExposure = 0f;
            sys.Evaluate(new List<Survivor> { sv });

            Assert.AreEqual(2, sys.GetStage(sv.Id), "Mutagenesis stage is a one-way ratchet, not tied to current dose.");
        }

        [Test]
        public void Tick_Stage3_DrainsHealthOverTime()
        {
            var sys = new RadiationMutagenesisSystem();
            var sv = MakeSurvivor("sv1", RadiationMutagenesisSystem.Stage3CellularThreshold);
            sys.Evaluate(new List<Survivor> { sv });
            sv.Needs.Health = 100f;

            sys.Tick(2f, new List<Survivor> { sv });

            Assert.That(sv.Needs.Health,
                Is.EqualTo(100f - RadiationMutagenesisSystem.CellularBreakdownHealthDrainPerHour * 2f).Within(Eps));
        }

        [Test]
        public void Tick_BelowStage3_NoHealthDrain()
        {
            var sys = new RadiationMutagenesisSystem();
            var sv = MakeSurvivor("sv1", RadiationMutagenesisSystem.Stage2CataractsThreshold);
            sys.Evaluate(new List<Survivor> { sv });
            sv.Needs.Health = 100f;

            sys.Tick(5f, new List<Survivor> { sv });

            Assert.That(sv.Needs.Health, Is.EqualTo(100f).Within(Eps));
        }

        [Test]
        public void ShouldAutoInfect_BelowStage3_AlwaysFalse()
        {
            var sys = new RadiationMutagenesisSystem();
            var sv = MakeSurvivor("sv1", RadiationMutagenesisSystem.Stage2CataractsThreshold);
            sys.Evaluate(new List<Survivor> { sv });

            Assert.IsFalse(sys.ShouldAutoInfect(sv, new System.Random(1)));
        }

        [Test]
        public void ShouldAutoInfect_Stage3_UsesInjectedRngAgainstInfectionChance()
        {
            var sys = new RadiationMutagenesisSystem();
            var sv = MakeSurvivor("sv1", RadiationMutagenesisSystem.Stage3CellularThreshold);
            sys.Evaluate(new List<Survivor> { sv });

            Assert.IsTrue(sys.ShouldAutoInfect(sv, new FixedRandom(0.1)),
                "Roll below the 0.8 infection chance must infect.");
            Assert.IsFalse(sys.ShouldAutoInfect(sv, new FixedRandom(0.95)),
                "Roll above the 0.8 infection chance must not infect.");
        }

        [Test]
        public void SaveRestore_RoundTripsStagesAndHairLossFlags()
        {
            var sys = new RadiationMutagenesisSystem();
            sys.SetNeedsSystem(NewNeedsSystem());
            var sv1 = MakeSurvivor("sv1", RadiationMutagenesisSystem.Stage3CellularThreshold);
            var sv2 = MakeSurvivor("sv2", RadiationMutagenesisSystem.Stage1HairLossThreshold);
            sys.Evaluate(new List<Survivor> { sv1, sv2 });

            var save = sys.CaptureState();
            Assert.Contains("sv2", save.HairLossAppliedIds,
                "The one-time hair-loss-applied flag must be part of the captured save state.");

            var restored = new RadiationMutagenesisSystem();
            restored.RestoreState(save);

            Assert.AreEqual(3, restored.GetStage("sv1"));
            Assert.AreEqual(1, restored.GetStage("sv2"));
        }

        private sealed class FixedRandom : System.Random
        {
            private readonly double _value;
            public FixedRandom(double value) { _value = value; }
            public override double NextDouble() => _value;
        }
    }
}
