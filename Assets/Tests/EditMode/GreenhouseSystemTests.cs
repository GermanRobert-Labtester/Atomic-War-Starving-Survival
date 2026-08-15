using System.Collections.Generic;
using NUnit.Framework;
using AtomicWar._Game.Core;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Expansion XI — "The Glass Orchard": unit tests for the pure greenhouse
    /// simulation (growth, water, contamination, blight, harvest, save round-trip,
    /// and the pre-war-wheat unlock gate). These exercise the plain-C# system
    /// directly — no host, no inventory — the way the audit's "core logic must be
    /// testable without Unity gameplay" guidance recommends.
    /// </summary>
    [TestFixture]
    public class GreenhouseSystemTests
    {
        // Mushroom: 96 growth-hours → 4 days to mature at full (4h) light.
        private const string Mushroom = GreenhouseExpansionCatalog.Items.SeedMushroom;
        private const string Wheat = GreenhouseExpansionCatalog.Items.SeedWheat;

        private static GreenhouseSystem FreshSystem(int seed = 1)
        {
            var sys = new GreenhouseSystem(seed);
            sys.EnsurePlots(2);
            return sys;
        }

        [Test]
        public void Plant_Requires_FallowPlot_AndKnownSeed()
        {
            var sys = FreshSystem();
            Assert.IsTrue(sys.Plant(0, Mushroom, 1, out var consumed));
            Assert.AreEqual(Mushroom, consumed);

            // Unknown seed rejected.
            Assert.IsFalse(sys.Plant(1, "not_a_real_seed", 1, out _));
            // Occupied plot rejected.
            Assert.IsFalse(sys.Plant(0, Mushroom, 1, out _));
            // Out-of-range rejected.
            Assert.IsFalse(sys.Plant(99, Mushroom, 1, out _));
        }

        [Test]
        public void WellTendedCrop_Matures_AndHarvests_Clean()
        {
            var sys = FreshSystem();
            var matured = new List<string>();
            sys.OnCropMatured += (plot, seed) => matured.Add(seed);

            Assert.IsTrue(sys.Plant(0, Mushroom, 1, out _));
            sys.Water(0, 60f, tainted: false);

            // 4 days at full light matures a mushroom (25 growth/day → 100).
            for (int i = 0; i < 5; i++)
                sys.TickDay(2 + i, growLightHours: 4f, ashContaminationRate: 0f);

            CollectionAssert.Contains(matured, Mushroom);
            var harvest = sys.Harvest(0);
            Assert.IsTrue(harvest.success, "Mature plot should harvest");
            Assert.AreEqual(GreenhouseExpansionCatalog.Items.CropMushroom, harvest.yieldItemId);
            Assert.AreEqual(2, harvest.amount);
            Assert.IsFalse(harvest.contaminated, "Clean soil should yield a clean crop");
            Assert.AreEqual(1, sys.TotalHarvests);
            // Plot resets to fallow after harvest.
            Assert.IsTrue(GreenhouseSystem.IsFallow(sys.Plots[0]));
        }

        [Test]
        public void TaintedIrrigation_ContaminatesHarvest()
        {
            var sys = FreshSystem();
            sys.Plant(0, Mushroom, 1, out _);
            // Tainted water grows the crop but poisons the soil past tolerance.
            sys.Water(0, 60f, tainted: true);
            Assert.GreaterOrEqual(sys.Plots[0].soilContamination, 60f);

            for (int i = 0; i < 5; i++)
                sys.TickDay(2 + i, growLightHours: 4f, ashContaminationRate: 0f);

            var harvest = sys.Harvest(0);
            Assert.IsTrue(harvest.success);
            Assert.IsTrue(harvest.contaminated, "Contaminated soil should taint the harvest");
            Assert.AreEqual(GreenhouseExpansionCatalog.Items.TaintedFood, harvest.yieldItemId);
        }

        [Test]
        public void Drought_StallsGrowth_FlagsDriedOut_AndKillsCrop()
        {
            var sys = FreshSystem();
            var dried = new List<int>();
            var failed = new List<int>();
            sys.OnPlotDriedOut += dried.Add;
            sys.OnCropFailed += failed.Add;

            sys.Plant(0, Mushroom, 1, out _);
            sys.Water(0, 8f, tainted: false); // exactly one day of water

            // Day 1: water hits zero → DriedOut fires, growth stalls, drought
            // blight begins to accrue.
            sys.TickDay(2, growLightHours: 4f, ashContaminationRate: 0f);
            CollectionAssert.Contains(dried, 0);
            Assert.AreEqual(0f, sys.Plots[0].growth, "Drought should stall growth");
            Assert.Greater(sys.Plots[0].blight, 0f, "Drought should start blight");

            // Continue drought until blight is fatal (0.25/day → 1.0 after ~4 days).
            for (int i = 0; i < 5; i++)
                sys.TickDay(3 + i, growLightHours: 4f, ashContaminationRate: 0f);

            Assert.AreEqual((int)GreenhouseStage.Failed, sys.Plots[0].stage);
            CollectionAssert.Contains(failed, 0);

            // A failed crop yields nothing and cannot be treated (only cleared).
            var harvest = sys.Harvest(0);
            Assert.IsFalse(harvest.success);
            Assert.IsFalse(sys.TreatBlight(0, out _));
            Assert.IsTrue(sys.Clear(0));
            Assert.IsTrue(GreenhouseSystem.IsFallow(sys.Plots[0]));
        }

        [Test]
        public void BlightTreatment_ClearsActiveBlight_ButConsumeOnlyOnSuccess()
        {
            var sys = FreshSystem();
            sys.Plant(0, Mushroom, 1, out _);
            sys.Water(0, 8f, tainted: false);
            // One day of drought seeds some (non-fatal) blight.
            sys.TickDay(2, growLightHours: 4f, ashContaminationRate: 0f);
            Assert.Greater(sys.Plots[0].blight, 0f);
            Assert.AreNotEqual((int)GreenhouseStage.Failed, sys.Plots[0].stage);

            Assert.IsTrue(sys.TreatBlight(0, out var treatmentId));
            Assert.AreEqual(GreenhouseExpansionCatalog.Items.BlightTreatment, treatmentId);
            Assert.AreEqual(0f, sys.Plots[0].blight, "Treatment should clear active blight");
        }

        [Test]
        public void EnsurePlots_NeverDestroysGrowingCrop()
        {
            var sys = new GreenhouseSystem(7);
            sys.EnsurePlots(2);
            sys.Plant(0, Mushroom, 1, out _);

            // Removing a planter box trims the trailing fallow plot but keeps
            // the plot with a growing crop.
            sys.EnsurePlots(1);
            Assert.AreEqual(1, sys.PlotCount);
            Assert.AreEqual(Mushroom, sys.Plots[0].seedItemId);
        }

        [Test]
        public void AshDrift_RaisesContamination_AndSurgeApplies()
        {
            var sys = FreshSystem();
            sys.Plant(0, Mushroom, 1, out _);
            sys.Water(0, 80f, tainted: false);
            float before = sys.Plots[0].soilContamination;
            sys.TickDay(2, growLightHours: 4f, ashContaminationRate: 5f);
            Assert.Greater(sys.Plots[0].soilContamination, before, "Ash should drift into the soil");

            sys.SurgeContamination(20f);
            Assert.GreaterOrEqual(sys.Plots[0].soilContamination, 20f);
        }

        [Test]
        public void SaveLoad_RoundTrip_PreservesState()
        {
            var sys1 = FreshSystem(11);
            sys1.Plant(0, Mushroom, 1, out _);
            sys1.Water(0, 40f, tainted: false);
            sys1.TickDay(2, growLightHours: 4f, ashContaminationRate: 0f);
            sys1.UnlockPreWarWheat();
            object snapshot = sys1.CaptureState();

            var sys2 = new GreenhouseSystem(99);
            sys2.RestoreState(snapshot);

            Assert.AreEqual(sys1.PlotCount, sys2.PlotCount);
            Assert.IsTrue(sys2.IsPreWarWheatUnlocked, "Wheat unlock should round-trip");
            Assert.AreEqual(Mushroom, sys2.Plots[0].seedItemId);
            Assert.AreEqual(sys1.Plots[0].growth, sys2.Plots[0].growth);
            Assert.AreEqual(sys1.Plots[0].water, sys2.Plots[0].water);
            Assert.AreEqual(sys1.TotalHarvests, sys2.TotalHarvests);
        }

        [Test]
        public void PreWarWheat_RequiresUnlock()
        {
            var sys = FreshSystem();
            Assert.IsFalse(sys.Plant(0, Wheat, 1, out _), "Wheat should be locked before unlock");

            sys.UnlockPreWarWheat();
            Assert.IsTrue(sys.Plant(1, Wheat, 1, out var consumed), "Wheat should plant after unlock");
            Assert.AreEqual(Wheat, consumed);
        }

        [Test]
        public void SaveId_IsStable()
        {
            var sys = new GreenhouseSystem(1);
            Assert.AreEqual(GreenhouseExpansionCatalog.SaveId, sys.SaveId);
        }
    }
}
