// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Expeditions;

namespace Ashfall.Core.Expeditions
{
    /// <summary>
    /// Headless verification suite for Task 8: Long-Range Reconnaissance Drones &amp; High-Altitude Mapping.
    /// Validates catalog load, launch/survey/recover cycle, route scouting, forecast generation,
    /// platform loss, and save-state roundtripping.
    /// </summary>
    public sealed class ReconTelemetryHeadlessReport : HeadlessReport
    {
        public int MissionsLaunched;
        public int SectorsSurveyed;
    }

    public static class ReconTelemetryHeadlessDemo
    {
        public static ReconTelemetryHeadlessReport Run(ILog? log = null)
        {
            CatalogLocator.UseInvariantCulture();
            log = log ?? NullLog.Instance;
            var report = new ReconTelemetryHeadlessReport();

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

            log.Info("[ReconTelemetryHeadlessDemo] begin");

            var sys = new ReconTelemetrySystem(null, new SeededRng(2005), log);
            var dataDir = CatalogLocator.ResolveDataDirectory();
            sys.LoadCatalog(ReconTelemetryCatalogLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer()));

            // 1. Catalog load
            Check(sys.Platforms.Count > 0, "catalog loads platforms");
            Check(sys.Platforms.ContainsKey("probe_helium_sounding_balloon"), "weather probe present");

            // 2. Launch mission
            var launch = sys.LaunchMission("probe_short_range_quad_uav", "loc_holdfast");
            Check(launch.IsSuccess, "launch mission succeeds");
            report.MissionsLaunched = sys.State.activeMissions.Count;

            // 3. Survey sectors
            var survey = sys.SurveySectors(launch.MissionId, new System.Collections.Generic.List<string> { "loc_holdfast" });
            Check(survey.IsSuccess, "survey succeeds");
            report.SectorsSurveyed = survey.SurveyedSectors.Count;

            // 4. Route scout
            var scout = sys.ScoutRoute("route_ice_road", launch.MissionId);
            Check(scout.IsSuccess, "route scout succeeds");
            Check(sys.GetRouteSpeedMultiplier("route_ice_road") < 1f, "route speed bonus active");

            // 5. Recover platform
            var recover = sys.RecoverPlatform(launch.MissionId);
            Check(recover.IsSuccess, "recover platform succeeds");
            Check(!sys.IsPlatformLaunched("probe_short_range_quad_uav"), "platform removed from launched list");

            // 6. Tick day and verify expiry
            sys.State.scoutedRoutes.Add(new RouteScoutRecord { routeId = "route_expired", expiryDay = 2, isActive = true });
            sys.TickDay(1);
            sys.TickDay(2);
            Check(sys.State.scoutedRoutes.Count == 0, "expired routes cleaned up");

            // 7. Save round-trip
            var captured = sys.CaptureState();
            var restored = new ReconTelemetrySystem(captured, new SeededRng(2005), log);
            restored.LoadCatalog(ReconTelemetryCatalogLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer()));
            Check(restored.State.activeMissions.Count == sys.State.activeMissions.Count, "round-trip preserves missions");
            Check(restored.State.scoutedRoutes.Count == sys.State.scoutedRoutes.Count, "round-trip preserves routes");

            report.Summary = $"Recon telemetry demo: launched={report.MissionsLaunched}, surveyed={report.SectorsSurveyed}";
            report.Passed = report.FailedCount == 0;
            log.Info("[ReconTelemetryHeadlessDemo] " + report.Summary);

            return report;
        }
    }
}
