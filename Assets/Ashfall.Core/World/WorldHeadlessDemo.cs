using System.Collections.Generic;

namespace Ashfall.Core.World
{
    /// <summary>
    /// Headless verification of the weather core port.
    /// Invoked by `dotnet test` and by Godot `-- --world-selftest`.
    /// </summary>
    public static class WorldHeadlessDemo
    {
        public static HeadlessReport Run(ILog? log = null)
        {
            CatalogLocator.UseInvariantCulture();
            log = log ?? NullLog.Instance;
            var report = new HeadlessReport();

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

            log.Info("[WorldHeadlessDemo] begin");

            var profile = new SeasonProfileDef
            {
                id = "demo",
                weatherCheckIntervalHours = 6f,
                seasons = new List<SeasonWindowDef>
                {
                    new SeasonWindowDef
                    {
                        id = "demo_window", startDay = 0,
                        clearWeight = 1f, rainWeight = 1f, overcastWeight = 1f,
                        ashfallWeight = 1f, falloutStormWeight = 1f,
                        blizzardWeight = 1f, blackRainWeight = 1f
                    }
                }
            };

            var sys = new WeatherSystem();
            sys.BindProfile(profile, 42);
            Check(sys.Current == WeatherKind.Clear, "starts clear");

            int changes = 0;
            sys.OnWeatherChanged += k => changes++;
            sys.Tick(48f);
            Check(changes >= 1, "weather rolls over 48h");
            Check(sys.State.rollCount >= 1, "roll count advances");

            var a = new WeatherSystem();
            a.BindProfile(profile, 7);
            var b = new WeatherSystem();
            b.BindProfile(profile, 7);
            for (int i = 0; i < 4; i++) { a.Tick(6f); b.Tick(6f); }
            Check(a.Current == b.Current, "same seed, same weather sequence");

            var restored = new WeatherSystem();
            restored.BindProfile(profile, 7);
            restored.RestoreState(a.CaptureState());
            for (int i = 0; i < 4; i++) { a.Tick(6f); restored.Tick(6f); }
            Check(a.Current == restored.Current, "save/load resumes the identical sequence");

            sys.ForceWeather(WeatherKind.FalloutStorm);
            Check(sys.OutdoorRadModifier == WeatherSystem.FalloutStormOutdoorRadModifier,
                "storm spikes outdoor rad");
            Check(sys.IsScavengingBlocked(false) && !sys.IsScavengingBlocked(true),
                "storm blocks scavenging without a full suit");
            Check(WeatherSystem.TemperaturePenaltyForWeather(WeatherKind.Blizzard) ==
                WeatherSystem.BlizzardTemperaturePenaltyC, "blizzard temperature penalty");

            string before = SaveChecksum.Compute(sys.CaptureState());
            var r2 = new WeatherSystem();
            r2.BindProfile(profile, 42);
            r2.RestoreState(sys.CaptureState());
            string after = SaveChecksum.Compute(r2.CaptureState());
            Check(before == after, "save/load checksum stable");

            var snapshot = sys.CaptureState();
            snapshot.currentKind = "Blizzard";
            Check(sys.Current == WeatherKind.FalloutStorm, "capture returns snapshot, not live state");

            report.Passed = report.FailedCount == 0;
            report.Summary =
                $"[WorldHeadlessDemo] {(report.Passed ? "PASS" : "FAIL")} " +
                $"{report.PassedCount}/{report.PassedCount + report.FailedCount}";
            log.Info(report.Summary);
            return report;
        }
    }
}
