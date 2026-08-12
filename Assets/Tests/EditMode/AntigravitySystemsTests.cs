using System;
using NUnit.Framework;
using Random = System.Random;
using AtomicWar._Game.Medical;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Environment;
using AtomicWar._Game.World;
using AtomicWar._Game.Factions;
using AtomicWar._Game.Narrative;
using AtomicWar._Game.Shelter;

namespace AtomicWar.Tests.EditMode
{
    [TestFixture]
    public class AntigravitySystemsTests
    {
        // ── Phase 16: Tinnitus ──────────────────────────────────────

        [Test]
        public void Tinnitus_ExplosionEvent_TriggersHearing()
        {
            var system = new TinnitusSystem { Rng = new Random(42) };
            var survivor = new Survivor { Id = "s1" };
            var survivors = new System.Collections.Generic.List<Survivor> { survivor };

            // Direct exposure with high severity
            system.OnExplosionEvent(1.0f, survivors);

            Assert.IsTrue(survivor.HasTinnitus,
                "High-severity explosion should trigger tinnitus");
            Assert.IsTrue(survivor.IsDeafToWarnings);
            Assert.Greater(survivor.TinnitusHoursRemaining, 0f);
        }

        [Test]
        public void Tinnitus_DecaysOverTime()
        {
            var system = new TinnitusSystem { Rng = new Random(42) };
            var survivor = new Survivor { Id = "s1" };
            var survivors = new System.Collections.Generic.List<Survivor> { survivor };

            system.OnExplosionEvent(1.0f, survivors);
            float initial = survivor.TinnitusHoursRemaining;

            system.Tick(survivor, 12f, false);
            Assert.Less(survivor.TinnitusHoursRemaining, initial);
        }

        // ── Phase 16: Hoarding ──────────────────────────────────────

        [Test]
        public void Hoarding_DiscoverHoard_ReturnsHiddenFood()
        {
            var system = new HoardingBehaviorSystem { Rng = new Random(42) };
            var survivor = new Survivor
            {
                Id = "s1",
                HasHoardingCompulsion = true,
                HiddenFoodCount = 5
            };

            int found = system.DiscoverHoard(survivor);

            Assert.AreEqual(5, found);
            Assert.AreEqual(0, survivor.HiddenFoodCount);
            Assert.IsTrue(survivor.HoardWasDiscovered);
        }

        // ── Phase 16: Nerve Damage ──────────────────────────────────

        [Test]
        public void NerveDamage_HighRadiation_DevelopsDamage()
        {
            var system = new NerveDamageSystem { Rng = new Random(42) };
            var survivor = new Survivor
            {
                Id = "s1",
                LifetimeRadiationExposure = 600f
            };

            system.CheckForNerveDamage(survivor);

            Assert.IsTrue(survivor.HasNerveDamage);
            Assert.Greater(survivor.WeaponAccuracyModifier, 0.2f);
        }

        [Test]
        public void NerveDamage_Stabilizer_ReducesPenalty()
        {
            var system = new NerveDamageSystem { Rng = new Random(42) };
            var survivor = new Survivor
            {
                Id = "s1",
                LifetimeRadiationExposure = 600f
            };

            system.CheckForNerveDamage(survivor);
            float prePenalty = survivor.WeaponAccuracyModifier;

            system.ApplyNerveStabilizer(survivor);

            Assert.Less(survivor.WeaponAccuracyModifier, prePenalty,
                "Stabilizer should reduce accuracy penalty");
            Assert.Greater(survivor.NerveStabilizerHours, 0f);
        }

        // ── Phase 17: Ash Drift Burial ──────────────────────────────

        [Test]
        public void AshDrift_Storm_AccumulatesAsh()
        {
            var system = new AshDriftBurialSystem();

            system.OnAshStorm(1.0f);

            Assert.Greater(system.AshAccumulation, 10f);
        }

        [Test]
        public void AshDrift_ClearAsh_ReducesAccumulation()
        {
            var system = new AshDriftBurialSystem();
            system.OnAshStorm(1.0f);
            float preClear = system.AshAccumulation;

            system.ClearAsh(2f);

            Assert.Less(system.AshAccumulation, preClear);
        }

        // ── Phase 17: Location Evolution ────────────────────────────

        [Test]
        public void LocationEvolution_MarkCleared_ResetsOwner()
        {
            var system = new LocationEvolutionSystem();
            system.RegisterLocation("test_loc", "raiders");

            system.MarkLocationCleared("test_loc");
            var state = system.GetLocationState("test_loc");

            Assert.IsTrue(state.IsCleared);
            Assert.AreEqual("none", state.CurrentOwner);
        }

        // ── Phase 19: Cultural Preservation ─────────────────────────

        [Test]
        public void CulturalPreservation_PreserveArtifacts_ReachesTier1()
        {
            var system = new CulturalPreservationSystem();
            var survivor = new Survivor { Id = "s1" };

            system.PreserveArtifact("book_moby_dick", survivor);
            system.PreserveArtifact("vinyl_beethoven", survivor);
            system.PreserveArtifact("painting_sunflowers", survivor);

            Assert.AreEqual(3, survivor.CulturalArtifactsPreserved);
            Assert.AreEqual(1, system.CurrentTier);
            Assert.Greater(system.BunkerCulturalResilience, 0.1f);
        }

        // ── Phase 19: Peace Treaty ──────────────────────────────────

        [Test]
        public void PeaceTreaty_StartNegotiations_RequiresStanding()
        {
            var system = new PeaceTreatySystem();

            bool canStart = system.StartNegotiations(70f, 70f);

            Assert.IsTrue(canStart);
            Assert.IsTrue(system.IsNegotiating);
        }

        [Test]
        public void PeaceTreaty_ThreeConcessions_SignsTreaty()
        {
            var system = new PeaceTreatySystem();
            system.StartNegotiations(70f, 70f);

            int food = 100, ammo = 100, medical = 100;
            bool c1 = system.MakeConcession("food", ref food, ref ammo, ref medical);
            bool c2 = system.MakeConcession("ammo", ref food, ref ammo, ref medical);
            bool c3 = system.MakeConcession("medical", ref food, ref ammo, ref medical);

            Assert.IsTrue(c1 && c2 && c3);
            Assert.IsTrue(system.IsTreatySigned);
            Assert.AreEqual(80, food);
            Assert.AreEqual(50, ammo);
            Assert.AreEqual(95, medical);
        }

        // ── Phase 19: Deep Aquifer ──────────────────────────────────

        [Test]
        public void DeepAquifer_CompleteDrilling_ProvidesWater()
        {
            var system = new DeepAquiferProjectSystem();
            system.StartProject();

            system.ContributeDrillHours(200f);

            Assert.IsTrue(system.IsComplete);
            Assert.AreEqual(50f, system.GetDailyWaterOutput());
        }
    }
}
