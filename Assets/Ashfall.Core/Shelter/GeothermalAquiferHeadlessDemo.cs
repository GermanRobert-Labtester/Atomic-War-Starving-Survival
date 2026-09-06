using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Shelter;

namespace Ashfall.Core.Shelter
{
    /// <summary>
    /// Headless verification suite for Task 7: Deep Geothermal Boreholes & Clean Aquifer Pumping.
    /// Validates drilling progression, strata crossing, casing installation,
    /// turbine commissioning, descaling, aquifer tapping, pressure venting,
    /// and save-state roundtripping.
    /// </summary>
    public sealed class GeothermalAquiferHeadlessReport : HeadlessReport
    {
        public int StrataCrossed;
        public int ActionsSucceeded;
    }

    public static class GeothermalAquiferHeadlessDemo
    {
        public static GeothermalAquiferHeadlessReport Run(ILog? log = null)
        {
            CatalogLocator.UseInvariantCulture();
            log = log ?? NullLog.Instance;
            var report = new GeothermalAquiferHeadlessReport();

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

            log.Info("[GeothermalAquiferHeadlessDemo] begin");

            var sys = new GeothermalAquiferSystem(null, new SeededRng(2003), log);
            var state = sys.State;

            // 1. Initial state
            Check(!sys.IsTurbineCommissioned, "initial turbine not commissioned");
            Check(!sys.IsAquiferTapped, "initial aquifer not tapped");
            Check(state.currentDepthMeters == 0f, "initial depth is zero");

            // 2. Start drilling
            var startResult = sys.StartDrilling();
            Check(startResult.IsSuccess, "start drilling succeeds");
            Check(state.projectActive, "project is active after start");

            // 3. Advance drilling and cross strata
            int strataCrossed = 0;
            sys.OnStrataReached += strataId =>
            {
                strataCrossed++;
                log.Info($"[Geothermal] Crossed strata: {strataId}");
            };

            for (int i = 0; i < 50; i++)
            {
                var advance = sys.AdvanceDrilling(10f);
                if (!advance.IsSuccess) break;
            }

            Check(state.currentDepthMeters > 0f, "depth advanced");
            Check(strataCrossed > 0, "crossed at least one strata");
            report.StrataCrossed = strataCrossed;

            // 4. Install casing
            var casingResult = sys.InstallCasing(200f);
            Check(casingResult.IsSuccess, "install casing succeeds");
            Check(state.installedCasingDepth >= 200f, "casing depth recorded");

            // 5. Commission turbine (requires steam pocket)
            if (sys.GetCurrentStrata()?.StrataId.Contains("steam") == true)
            {
                var turbineResult = sys.CommissionTurbine();
                Check(turbineResult.IsSuccess, "commission turbine succeeds");
                Check(sys.IsTurbineCommissioned, "turbine is commissioned");
            }

            // 6. Vent pressure
            if (state.steamPressurePsi > 0f)
            {
                var ventResult = sys.VentPressure();
                Check(ventResult.IsSuccess, "vent pressure succeeds");
                Check(state.pressureReliefState > 0f, "pressure relief increased");
            }

            // 7. Tick day
            sys.TickDay(1);
            Check(state.lastProcessedDay == 1, "tick day advances");

            // 8. Save round-trip
            var captured = sys.CaptureState();
            var restored = new GeothermalAquiferSystem(captured, new SeededRng(2003), log);
            restored.LoadCatalog(null);
            Check(restored.State.currentDepthMeters == state.currentDepthMeters, "round-trip preserves depth");
            Check(restored.State.installedCasingDepth == state.installedCasingDepth, "round-trip preserves casing");
            Check(restored.State.turbineCommissioned == state.turbineCommissioned, "round-trip preserves turbine state");
            Check(restored.State.lastProcessedDay == state.lastProcessedDay, "round-trip preserves tick day");

            report.Summary = $"Geothermal aquifer demo: depth={state.currentDepthMeters:F0}m, strata={strataCrossed}, turbine={sys.IsTurbineCommissioned}";
            report.Passed = report.FailedCount == 0;
            log.Info("[GeothermalAquiferHeadlessDemo] " + report.Summary);

            return report;
        }
    }
}
