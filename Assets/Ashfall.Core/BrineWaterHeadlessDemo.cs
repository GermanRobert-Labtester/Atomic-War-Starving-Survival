using System.Text;

#pragma warning disable CS8618
namespace Ashfall.Core
{
    /// <summary>Headless-report extension for the BrineWater slice.</summary>
    public sealed class BrineWaterHeadlessReport : HeadlessReport
    {
        public BrineWaterSystemState Brine;
    }

    /// <summary>
    /// Vertical-slice smoke for the Holdfast Salt &amp; steam loop: the plant is
    /// silent until visited, daily brine load degrades the membrane, an outfall
    /// shift halves the load, a trip starts the 48-hour freeze clock on the
    /// Cluster, resin above 40 restores steam, and the state survives a JSON
    /// roundtrip through the port (JsonUtility-free).
    /// Invoked by `dotnet test`; wired into the Godot host as `-- --brine-selftest`.
    /// </summary>
    public static class BrineWaterHeadlessDemo
    {
        public static BrineWaterHeadlessReport Run(ILog log = null!)
        {
            CatalogLocator.UseInvariantCulture();
            log = log ?? NullLog.Instance;
            var report = new BrineWaterHeadlessReport();

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

            bool Approx(float a, float b) => System.Math.Abs(a - b) < 0.01f;

            log.Info("[BrineWaterHeadlessDemo] begin");

            // B6: gated on unlock. A boot that never visits the plant must not trip.
            var dormant = new BrineWaterSystem();
            for (int d = 0; d < 40; d++)
                dormant.TickDaily(d, WeatherKind.Blizzard, -20f, outfallShifted: false);
            Check(!dormant.Unlocked && !dormant.SteamTripped, "plant stays dormant before unlock (B6)");
            Check(dormant.MembraneIntegrity == 72f, "no degradation before unlock");

            var brine = new BrineWaterSystem();
            bool tripFired = false;
            brine.OnSteamTrip += () => tripFired = true;
            brine.Unlock();

            // Daily load: 3.2 / day under clear skies.
            for (int d = 0; d < 5; d++)
                brine.TickDaily(d, WeatherKind.Clear, -12f, outfallShifted: false);
            Check(Approx(brine.MembraneIntegrity, 72f - 5f * 3.2f), "daily brine load degrades membrane");

            // False Spring increases load 15%.
            float before = brine.MembraneIntegrity;
            brine.TickDaily(5, WeatherKind.FalseSpring, 4f, outfallShifted: false);
            Check(before - brine.MembraneIntegrity > 3.2f, "false spring raises load");

            // Outfall shift cuts the load to 55%.
            before = brine.MembraneIntegrity;
            brine.TickDaily(6, WeatherKind.Clear, -12f, outfallShifted: true);
            Check(Approx(before - brine.MembraneIntegrity, 3.2f * 0.55f),
                "outfall shift halves the daily load");

            // Run the membrane down into a steam trip, then watch the 48h clock.
            int tripDay = -1;
            brine.OnSteamTrip += () => tripDay = brine.State.steamTripDay;
            for (int d = 7; d < 40 && !brine.SteamTripped; d++)
                brine.TickDaily(d, WeatherKind.Clear, -12f, outfallShifted: false);
            Check(brine.SteamTripped, "steam trip fires below threshold");
            Check(tripFired && tripDay == brine.State.steamTripDay, "OnSteamTrip carries the trip day");
            Check(brine.State.hoursSinceTrip == 0, "48h clock starts at zero");

            for (int d = 0; d < 2; d++)
                brine.TickDaily(40 + d, WeatherKind.Clear, -20f, outfallShifted: false);
            Check(brine.State.hoursSinceTrip == 48, "clock advances 24h per day (48h at two days)");
            Check(brine.ClusterIndoorC < 16f, "cluster indoor C falls after trip");
            // At 48h the tween reaches the outdoor floor exactly (-20 here); it must
            // never jump below it or above 16 on the way.
            Check(brine.State.clusterIndoorC <= 16f && brine.State.clusterIndoorC >= -20f,
                "cluster cooling stays inside the 48h tween range");

            // Resin repair above 40 saves the membrane and restores steam.
            Check(brine.RepairWithResin(4), "resin repair accepted");
            Check(brine.MembraneIntegrity >= 40f && !brine.SteamTripped && brine.State.membraneSaved,
                "repair above 40 clears the trip");
            Check(brine.ClusterIndoorC == 14f, "cluster restored to 14C after repair");
            Check(!brine.RepairWithResin(0), "zero resin rejected");

            // Salt trade unlock implies the plant unlock.
            var trader = new BrineWaterSystem();
            trader.UnlockSaltTrade();
            Check(trader.Unlocked && trader.State.saltTradeUnlocked, "salt trade unlocks the plant");

            // Haul: a quarter of the clean water is lost south.
            Check(Approx(trader.HaulCleanWaterSouth(8f), 6f), "haul south loses 25%");

            // JSON roundtrip through the port preserves the tripped pipeline.
            var json = new SystemTextJsonSerializer();
            var blob = json.Serialize(brine.CaptureState());
            var restored = new BrineWaterSystem();
            restored.RestoreState(json.Deserialize<BrineWaterSystemState>(blob)!);
            Check(restored.SteamTripped == brine.SteamTripped, "roundtrip trip flag");
            Check(restored.State.hoursSinceTrip == brine.State.hoursSinceTrip, "roundtrip 48h clock");
            Check(restored.State.membraneSaved == brine.State.membraneSaved, "roundtrip membrane saved");
            Check(restored.State.membraneIntegrity == brine.State.membraneIntegrity, "roundtrip integrity");

            report.Brine = brine.CaptureState();
            report.Passed = report.FailedCount == 0;
            var sb = new StringBuilder();
            sb.Append("BrineWaterHeadlessDemo ");
            sb.Append(report.Passed ? "PASS" : "FAIL");
            sb.Append(" ").Append(report.PassedCount).Append("/").Append(report.PassedCount + report.FailedCount);
            report.Summary = sb.ToString();
            log.Info(report.Summary);
            return report;
        }
    }
}
