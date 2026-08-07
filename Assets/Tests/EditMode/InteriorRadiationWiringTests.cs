using NUnit.Framework;
using AtomicWar._Game.Shelter;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// RAD-002 / RAD-003 / RAD-004 — bunker contamination, room contamination and
    /// ceiling material were all tracked, decayed and saved, but none of them reached
    /// Shelter.GetInteriorRadsPerHour. Bunker pollution was cosmetic and ceiling
    /// upgrades did nothing. These tests pin each source into the interior dose.
    /// </summary>
    [TestFixture]
    public class InteriorRadiationWiringTests
    {
        private const float Eps = 1e-3f;

        /// <summary>Baseline for a bare shelter: no shielding, unfiltered air leak.</summary>
        private static float Baseline(float exteriorRads) => exteriorRads + 5f;

        [Test]
        public void BunkerContamination_AddsToInteriorDose()
        {
            var shelter = new Shelter();
            float clean = shelter.GetInteriorRadsPerHour(10f);
            Assert.That(clean, Is.EqualTo(Baseline(10f)).Within(Eps));

            // The Day-30 "let them in" dilemma spikes this.
            shelter.AddBunkerContamination(4f);

            Assert.That(shelter.GetInteriorRadsPerHour(10f),
                Is.EqualTo(clean + 4f).Within(Eps),
                "Contamination carried inside the shielding is added, not attenuated");
        }

        [Test]
        public void RoomContamination_AddsToInteriorDose_OnlyAboveThreshold()
        {
            var shelter = new Shelter();
            var stores = new ShelterRoom("stores", null);
            shelter.RegisterRoom(stores);

            float clean = shelter.GetInteriorRadsPerHour(10f);

            // Below the penalty threshold a grubby room doses nobody.
            stores.AmbientContamination = ShelterRoom.RadPenaltyThreshold * 0.5f;
            Assert.That(shelter.GetInteriorRadsPerHour(10f), Is.EqualTo(clean).Within(Eps),
                "Rooms below the penalty threshold contribute nothing");

            // Hot stores dose the bunker.
            stores.AmbientContamination = 1f;
            float expected = clean + stores.GetIndoorRadContribution();
            Assert.That(shelter.GetInteriorRadsPerHour(10f), Is.EqualTo(expected).Within(Eps));
            Assert.That(shelter.GetInteriorRadsPerHour(10f), Is.GreaterThan(clean),
                "A fully contaminated store room must raise the interior dose");
        }

        [Test]
        public void CeilingMaterial_AttenuatesTheExteriorDose()
        {
            var shelter = new Shelter();
            var shielding = new MaterialShieldingSystem();
            shelter.CeilingAttenuationProvider = shielding.GetWeakestCeilingAttenuation;

            // No ceiling built: nothing stops the sky.
            Assert.That(shelter.GetInteriorRadsPerHour(10f),
                Is.EqualTo(Baseline(10f)).Within(Eps));

            shielding.UpgradeCeiling("quarters", MaterialShieldingSystem.WallMaterial.Concrete);

            // Concrete attenuates 80%: 10 exterior becomes 2, plus the 5/hr air leak.
            Assert.That(shelter.GetInteriorRadsPerHour(10f),
                Is.EqualTo(2f + 5f).Within(Eps),
                "Ceiling material must attenuate the exterior dose like any other layer");
        }

        [Test]
        public void UnwiredCeilingProvider_LeavesTheDoseUnchanged()
        {
            var shelter = new Shelter();
            Assert.That(shelter.CeilingAttenuationProvider, Is.Null);
            Assert.That(shelter.GetInteriorRadsPerHour(10f),
                Is.EqualTo(Baseline(10f)).Within(Eps),
                "An unwired provider must not silently shield the bunker");
        }
    }
}
