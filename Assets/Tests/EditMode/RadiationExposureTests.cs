using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Survivors;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// EditMode tests for the radiation exposure + contamination model: the exposure
    /// formula (zone - gear - shielding), clamping, suit breakage at zero durability,
    /// shelter shielding capping ambient, pause-aware ticking, lifetime-rad save/load,
    /// and the contamination / decon / Geiger / dosimeter helpers.
    /// </summary>
    [TestFixture]
    public class RadiationExposureTests
    {
        private const float Eps = 1e-4f;

        private static NeedsSystem NewNeedsSystem()
        {
            return new NeedsSystem(ScriptableObject.CreateInstance<NeedsProfile>());
        }

        private static ProtectiveGear NewGear(float protection, float durability = 100f, float degradeRate = 1f)
        {
            var gear = ScriptableObject.CreateInstance<ProtectiveGear>();
            gear.radProtection = protection;
            gear.durability = durability;
            gear.degradeRate = degradeRate;
            return gear;
        }

        private static WornGear NewWorn(ProtectiveGear gear, float currentDurability)
        {
            return new WornGear { Gear = gear, CurrentDurability = currentDurability };
        }

        // ---- Exposure formula ----

        [Test]
        public void Exposure_SubtractsGearAndShielding()
        {
            float exposure = RadiationSystem.ComputeExposurePerHour(zoneRadLevel: 100f, gearProtection: 20f, shelterShielding: 30f);
            Assert.That(exposure, Is.EqualTo(50f).Within(Eps));
        }

        [Test]
        public void Exposure_ClampsNegativeResultToZero()
        {
            Assert.That(RadiationSystem.ComputeExposurePerHour(10f, 50f, 0f), Is.EqualTo(0f).Within(Eps));
            Assert.That(RadiationSystem.ComputeExposurePerHour(100f, 0f, 150f), Is.EqualTo(0f).Within(Eps));
            Assert.That(RadiationSystem.ComputeExposurePerHour(0f, 0f, 0f), Is.EqualTo(0f).Within(Eps));
        }

        // ---- Shelter shielding caps ambient ----

        [Test]
        public void ShelterShielding_CapsAmbientAtZero()
        {
            Assert.That(RadiationSystem.ComputeEffectiveAmbient(50f, 100f), Is.EqualTo(0f).Within(Eps));
            Assert.That(RadiationSystem.ComputeEffectiveAmbient(100f, 30f), Is.EqualTo(70f).Within(Eps));
        }

        // ---- Gear protection + suit breakage ----

        [Test]
        public void GearProtection_SuitAtZeroDurability_GivesZeroProtection()
        {
            var worn = NewWorn(NewGear(protection: 50f, durability: 100f), currentDurability: 0f);

            Assert.That(worn.EffectiveProtection(), Is.EqualTo(0f).Within(Eps));
            Assert.That(RadiationSystem.ComputeGearProtection(new List<WornGear> { worn }), Is.EqualTo(0f).Within(Eps));
        }

        [Test]
        public void GearProtection_ScalesWithDurabilityFraction_AndSums()
        {
            var gear = NewGear(protection: 50f, durability: 100f);
            var half = NewWorn(gear, currentDurability: 50f);   // 50 * 0.5 = 25
            var full = NewWorn(gear, currentDurability: 100f);  // 50 * 1.0 = 50

            Assert.That(half.EffectiveProtection(), Is.EqualTo(25f).Within(Eps));
            Assert.That(RadiationSystem.ComputeGearProtection(new List<WornGear> { half, full }), Is.EqualTo(75f).Within(Eps));
        }

        [Test]
        public void GearProtection_BrokenSuit_LeavesFullExposure()
        {
            // A suit that has lost all durability no longer reduces the exposure rate.
            var broken = NewWorn(NewGear(protection: 80f, durability: 100f), currentDurability: 0f);
            float protection = RadiationSystem.ComputeGearProtection(new List<WornGear> { broken });
            float exposure = RadiationSystem.ComputeExposurePerHour(100f, protection, 0f);

            Assert.That(exposure, Is.EqualTo(100f).Within(Eps));
        }

        // ---- Pause-aware ticking ----

        [Test]
        public void Tick_WhilePaused_AccumulatesNoDose()
        {
            var context = new ExposureContext { ZoneRadLevel = 10f };
            var rad = new RadiationSystem(NewNeedsSystem(), _ => context);
            var survivor = new Survivor { Id = "s1" };
            rad.Register(survivor);

            rad.IsPaused = true;
            rad.Tick(1f);
            Assert.That(survivor.RadiationDose, Is.EqualTo(0f).Within(Eps));

            rad.IsPaused = false;
            rad.Tick(1f);
            Assert.That(survivor.RadiationDose, Is.EqualTo(10f).Within(Eps));
        }

        [Test]
        public void Tick_AppliesExposureFormula_DegradesGear_AndUpdatesDosimeter()
        {
            var worn = NewWorn(NewGear(protection: 20f, durability: 100f, degradeRate: 1f), currentDurability: 100f);
            var context = new ExposureContext { ZoneRadLevel = 100f, ShelterShielding = 30f };
            context.WornGear.Add(worn);

            var rad = new RadiationSystem(NewNeedsSystem(), _ => context);
            var survivor = new Survivor { Id = "s1" };
            rad.Register(survivor);

            rad.Tick(1f); // exposure = max(0, 100 - 20 - 30) = 50/hr over 1h

            Assert.That(survivor.RadiationDose, Is.EqualTo(50f).Within(Eps));
            Assert.That(survivor.LifetimeRadiationExposure, Is.EqualTo(50f).Within(Eps));
            Assert.That(worn.CurrentDurability, Is.EqualTo(99f).Within(Eps));

            var dosimeter = rad.GetDosimeter("s1");
            Assert.That(dosimeter.CurrentRate, Is.EqualTo(50f).Within(Eps));
            Assert.That(dosimeter.LifetimeDose, Is.EqualTo(50f).Within(Eps));
        }

        // ---- Save/load preserves lifetime rad ----

        [Test]
        public void SaveLoad_PreservesLifetimeRadiation()
        {
            var survivor = new Survivor { Id = "s1", DisplayName = "Test" };
            var rad = new RadiationSystem(NewNeedsSystem());
            rad.Expose(survivor, radsPerHour: 7f, hours: 3f); // lifetime += 21 (below acute threshold)

            float before = survivor.LifetimeRadiationExposure;
            Assert.That(before, Is.GreaterThan(0f));

            string json = JsonUtility.ToJson(survivor);
            var restored = JsonUtility.FromJson<Survivor>(json);

            Assert.That(restored.LifetimeRadiationExposure, Is.EqualTo(before).Within(Eps));
            Assert.That(restored.RadiationDose, Is.EqualTo(survivor.RadiationDose).Within(Eps));
        }

        // ---- Contamination + decontamination ----

        [Test]
        public void Contamination_DecaysOverTime_AndDeactivatesAtZero()
        {
            var contamination = new Contamination { RadsPerHour = 100f, DecayPerHour = 10f, IsActive = true };

            contamination.Decay(5f);
            Assert.That(contamination.RadsPerHour, Is.EqualTo(50f).Within(Eps));
            Assert.That(contamination.IsActive, Is.True);

            contamination.Decay(10f);
            Assert.That(contamination.RadsPerHour, Is.EqualTo(0f).Within(Eps));
            Assert.That(contamination.IsActive, Is.False);
        }

        [Test]
        public void Contamination_RaisesBunkerAmbient_UntilDecontaminated()
        {
            var hot = new Contamination { RadsPerHour = 40f, IsActive = true };
            var clean = new Contamination { RadsPerHour = 0f, IsActive = false };

            Assert.That(RadiationSystem.ComputeContaminationAmbient(new[] { hot, clean }), Is.EqualTo(40f).Within(Eps));

            var station = new DecontaminationStation { DeconRatePerHour = 20f };
            station.Decontaminate(hot, 2f); // 40 - 20*2 = 0
            Assert.That(hot.IsActive, Is.False);
            Assert.That(RadiationSystem.ComputeContaminationAmbient(new[] { hot, clean }), Is.EqualTo(0f).Within(Eps));
        }

        [Test]
        public void DecontaminationStation_ReducesContaminationOverTime()
        {
            var station = new DecontaminationStation { DeconRatePerHour = 10f };
            var contamination = new Contamination { RadsPerHour = 50f, IsActive = true };

            station.Decontaminate(contamination, 2f);
            Assert.That(contamination.RadsPerHour, Is.EqualTo(30f).Within(Eps));

            station.IsOperational = false;
            station.Decontaminate(contamination, 2f);
            Assert.That(contamination.RadsPerHour, Is.EqualTo(30f).Within(Eps)); // offline: no change
        }

        // ---- Geiger counter audio hook ----

        [Test]
        public void GeigerCounter_TickRateIsProportionalAndCapped()
        {
            Assert.That(GeigerCounter.ComputeTicksPerSecond(0f), Is.EqualTo(0f).Within(Eps));
            Assert.That(GeigerCounter.ComputeTicksPerSecond(100f), Is.EqualTo(5f).Within(Eps));   // 100 * 0.05
            Assert.That(GeigerCounter.ComputeTicksPerSecond(1000f), Is.EqualTo(30f).Within(Eps)); // capped
        }

        [Test]
        public void GeigerCounter_FiresClicksProportionalToRadLevel()
        {
            var geiger = new GeigerCounter();
            geiger.SetRadLevel(100f); // 5 ticks/sec

            int clicks = 0;
            geiger.OnClick += () => clicks++;
            geiger.Tick(1f);

            Assert.That(clicks, Is.EqualTo(5));
        }

        // ---- Dosimeter read-model ----

        [Test]
        public void Dosimeter_ExposesCurrentRateAndDecayingRecentExposure()
        {
            var dosimeter = new Dosimeter { SurvivorId = "s1" };

            dosimeter.Record(10f, 1f);
            Assert.That(dosimeter.CurrentRate, Is.EqualTo(10f).Within(Eps));
            Assert.That(dosimeter.RecentExposure, Is.EqualTo(10f).Within(Eps));

            dosimeter.Record(10f, 1f);
            // recent = 10 * 0.5 (retention) + 10 = 15
            Assert.That(dosimeter.RecentExposure, Is.EqualTo(15f).Within(Eps));
        }
    }
}
