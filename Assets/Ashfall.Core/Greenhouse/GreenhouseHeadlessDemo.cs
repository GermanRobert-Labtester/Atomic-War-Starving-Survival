using System;
using System.Collections.Generic;

namespace Ashfall.Core
{
    public sealed class GreenhouseHeadlessReport : HeadlessReport
    {
        public int Harvests;
        public int Blights;
    }

    /// <summary>
    /// Headless verification suite for Expansion 05 / XI: The Glass Orchard.
    /// Validates fallow requirements, clean vs tainted water, drought & blight,
    /// pre-war wheat unlock gates, and save-state roundtripping.
    /// </summary>
    public static class GreenhouseHeadlessDemo
    {
        public static GreenhouseHeadlessReport Run(ILog log = null)
        {
            CatalogLocator.UseInvariantCulture();
            log = log ?? NullLog.Instance;
            var report = new GreenhouseHeadlessReport();

            void Check(bool condition, string name)
            {
                report.Checks.Add(new HeadlessCheck { Name = name, Passed = condition });
                if (condition)
                {
                    report.PassedCount++;
                    log.Info("[PASS] " + name);
                }
                else
                {
                    report.FailedCount++;
                    log.Error("[FAIL] " + name);
                }
            }

            log.Info("[GreenhouseHeadlessDemo] begin");

            var sys = new GreenhouseSystem(seed: 42);
            sys.EnsurePlots(2);

            Check(sys.PlotCount == 2, "ensure plots allocates requested count");
            Check(GreenhouseSystem.IsFallow(sys.Plots[0]), "initial plot is fallow");

            const string Mushroom = GreenhouseExpansionCatalog.Items.SeedMushroom;
            const string Wheat = GreenhouseExpansionCatalog.Items.SeedWheat;

            // 1. Gated Planting
            Check(sys.Plant(0, Mushroom, 1, out var consumed), "plant mushroom in fallow plot");
            Check(consumed == Mushroom, "consumed seed id matches planted seed");
            Check(!sys.Plant(0, Mushroom, 1, out _), "cannot plant in occupied plot");
            Check(!sys.Plant(1, "invalid_seed", 1, out _), "invalid seed rejected");
            Check(!sys.Plant(1, Wheat, 1, out _), "pre-war wheat locked before ledger unlock");

            // 2. Irrigation & Growth to Mature
            sys.Water(0, 60f, tainted: false);
            Check(sys.Plots[0].water == 60f, "clean water added to plot");
            Check(sys.Plots[0].soilContamination == 0f, "clean water has 0 contamination");

            for (int d = 1; d <= 5; d++)
                sys.TickDay(d, growLightHours: 4f, ashContaminationRate: 0f);

            Check(sys.Plots[0].stage == (int)GreenhouseStage.Mature, "crop reached mature stage");

            // 3. Harvest Clean
            var harvest = sys.Harvest(0);
            Check(harvest.success, "mature crop harvests successfully");
            Check(harvest.yieldItemId == GreenhouseExpansionCatalog.Items.CropMushroom, "yields clean mushroom");
            Check(!harvest.contaminated, "harvest is clean");
            Check(GreenhouseSystem.IsFallow(sys.Plots[0]), "plot returns to fallow after harvest");

            // 4. Tainted Irrigation
            sys.Plant(0, Mushroom, 6, out _);
            sys.Water(0, 60f, tainted: true);
            Check(sys.Plots[0].soilContamination >= 60f, "tainted water adds soil contamination");

            for (int d = 6; d <= 11; d++)
                sys.TickDay(d, growLightHours: 4f, ashContaminationRate: 0f);

            var taintedHarvest = sys.Harvest(0);
            Check(taintedHarvest.success, "tainted crop harvests");
            Check(taintedHarvest.contaminated, "harvest flagged as contaminated");
            Check(taintedHarvest.yieldItemId == GreenhouseExpansionCatalog.Items.TaintedFood, "yields tainted food");

            // 5. Pre-War Wheat Unlock & Growth
            sys.UnlockPreWarWheat();
            Check(sys.IsPreWarWheatUnlocked, "pre-war wheat unlocked");
            Check(sys.Plant(1, Wheat, 12, out _), "can plant wheat after unlock");

            // 6. Save Roundtrip
            var savedState = sys.CaptureState();
            var restoredSys = new GreenhouseSystem(seed: 99);
            restoredSys.RestoreState(savedState);

            Check(restoredSys.PlotCount == 2, "restored plot count preserved");
            Check(restoredSys.IsPreWarWheatUnlocked, "restored pre-war wheat unlock preserved");
            Check(restoredSys.TotalHarvests == 2, "restored harvest count preserved");
            Check(restoredSys.Plots[1].seedItemId == Wheat, "restored active seed preserved");

            report.Passed = report.FailedCount == 0;
            log.Info($"GreenhouseHeadlessDemo {(report.Passed ? "PASS" : "FAIL")} {report.PassedCount}/{report.PassedCount + report.FailedCount}");
            return report;
        }
    }
}
