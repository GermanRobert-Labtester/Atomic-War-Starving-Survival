using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Survivors;

namespace AtomicWar.Tests.PlayMode
{
    [TestFixture]
    public class ChronicDiseasePlayModeTests
    {
        private AfflictionPipeline _pipeline;
        private Survivor _survivor;

        [SetUp]
        public void SetUp()
        {
            _pipeline = new AfflictionPipeline(new System.Random(39));
            _survivor = new Survivor { Id = "test_survivor", DisplayName = "Subject Alpha" };
        }

        [UnityTest]
        public IEnumerator Accumulate300PlusLifetimeRadExposure_TriggersChronicIllnessAssignment()
        {
            _survivor.LifetimeRadiationExposure = 350f;

            bool assigned = _pipeline.Evaluate(_survivor, (s, status) => { });
            yield return null;

            Assert.That(assigned, Is.True, "300+ lifetime radiation exposure must trigger chronic illness assignment.");
            Assert.That(_survivor.HasChronicIllness, Is.True);
            Assert.That(_survivor.ActiveChronicIllness.HasValue, Is.True);
        }

        [UnityTest]
        public IEnumerator ChronicIllness_AppliesStatPenalties_And_CanBeManagedWithMedicalSupplies()
        {
            _survivor.LifetimeRadiationExposure = 320f;
            _pipeline.Evaluate(_survivor);
            yield return null;

            Assert.That(_survivor.ActiveChronicIllness.HasValue, Is.True);

            // Assert stat penalty is active when unmanaged
            var illness = _survivor.ActiveChronicIllness.Value;
            switch (illness)
            {
                case ChronicIllnessKind.LungFibrosis:
                    Assert.That(_survivor.MaxStaminaCap, Is.EqualTo(60f));
                    Assert.That(_survivor.FatigueDrainMultiplier, Is.EqualTo(1.5f));
                    break;

                case ChronicIllnessKind.RadiationCataracts:
                    Assert.That(_survivor.ScavengingYieldMultiplier, Is.EqualTo(0.5f));
                    break;

                case ChronicIllnessKind.BoneMarrowDepression:
                    Assert.That(_survivor.FatigueDrainMultiplier, Is.EqualTo(2.0f));
                    break;
            }

            Assert.That(_survivor.IsChronicIllnessManaged, Is.False);

            // Manage illness with medical supplies
            bool managed = _pipeline.ManageIllness(_survivor, "anti_rad", 24f);
            yield return null;

            Assert.That(managed, Is.True);
            Assert.That(_survivor.IsChronicIllnessManaged, Is.True);
            Assert.That(_survivor.FatigueDrainMultiplier, Is.EqualTo(1.0f), "Managed illness must temporarily normalize fatigue drain multiplier.");
            Assert.That(_survivor.ScavengingYieldMultiplier, Is.EqualTo(1.0f), "Managed illness must temporarily normalize yield multiplier.");
        }
    }
}
