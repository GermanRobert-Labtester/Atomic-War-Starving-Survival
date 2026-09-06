using System;
using Ashfall.Core;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    /// <summary>Thin Godot adapter for shelter fluid topology and delivery.</summary>
    public sealed class FluidLogisticsHostSession : HostSessionBase
    {
        public FluidLogisticsSystem System { get; }
        public string LastEvent { get; private set; } = string.Empty;

        public static FluidLogisticsHostSession Create(string dataDir, FluidLogisticsSystem? system = null)
        {
            var session = new FluidLogisticsHostSession(system ?? new FluidLogisticsSystem(new GodotLog()));
            if (!string.IsNullOrWhiteSpace(dataDir))
            {
                var catalog = FluidInfrastructureCatalogLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
                session.System.LoadCatalog(catalog);
                session.LastEvent = $"Fluid infrastructure catalog loaded: {catalog.Pipes.Count} pipes, {catalog.Pumps.Count} pumps";
            }
            return session;
        }

        public FluidLogisticsHostSession(FluidLogisticsSystem system)
        {
            System = system ?? throw new ArgumentNullException(nameof(system));
            System.OnPipeBurst += edge =>
            {
                LastEvent = "Pipe burst: " + edge;
                RaiseStateChanged();
            };
            System.OnDistributionSolved += report =>
            {
                LastEvent = $"Fluid delivery @ day {report.day}: {report.deliveredVolume:F1} units";
                RaiseStateChanged();
            };
            System.OnStateChanged += () => RaiseStateChanged();
        }

        public FluidDistributionReport AdvanceDay(int day, float temperatureC, float powerAvailability01 = 1f)
        {
            var report = System.Tick(day, temperatureC, powerAvailability01);
            LastEvent = $"Fluid tick @ day {day}: {report.deliveredVolume:F1} units delivered";
            RaiseStateChanged();
            return report;
        }

        public FluidLogisticsState CaptureState() => System.CaptureState();
        public void RestoreState(FluidLogisticsState state) => System.RestoreState(state);
    }
}
