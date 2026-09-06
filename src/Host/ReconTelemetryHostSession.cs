// ============================================================================
// Host Session : ReconTelemetryHostSession
// Core System : Ashfall.Core.Expeditions.ReconTelemetrySystem
// ============================================================================
using System;
using Godot;
using Ashfall.Core.Expeditions;

namespace AtomicWar.GodotApp
{
    public sealed class ReconTelemetryHostSession : HostSessionBase
    {
        public ReconTelemetrySystem System { get; }
        public ReconTelemetryState State => System.State;

        public event Action<string>? OnReconLaunched
        {
            add { System.OnReconLaunched += value; }
            remove { System.OnReconLaunched -= value; }
        }
        public event Action<string>? OnSurveyCompleted
        {
            add { System.OnSurveyCompleted += value; }
            remove { System.OnSurveyCompleted -= value; }
        }
        public event Action<string, string>? OnPlatformLost
        {
            add { System.OnPlatformLost += value; }
            remove { System.OnPlatformLost -= value; }
        }
        public event Action<string>? OnPlatformRecovered
        {
            add { System.OnPlatformRecovered += value; }
            remove { System.OnPlatformRecovered -= value; }
        }
        public event Action<string>? OnFalloutForecastGenerated
        {
            add { System.OnFalloutForecastGenerated += value; }
            remove { System.OnFalloutForecastGenerated -= value; }
        }
        public event Action<string>? OnRouteScouted
        {
            add { System.OnRouteScouted += value; }
            remove { System.OnRouteScouted -= value; }
        }

        public ReconTelemetryHostSession(ReconTelemetrySystem system)
        {
            System = system ?? throw new ArgumentNullException(nameof(system));
        }

        public void LoadCatalog(string dataDir)
        {
            if (string.IsNullOrEmpty(dataDir)) return;
            try
            {
                var fileIO = new Ashfall.Core.FileSystemIO();
                var serializer = new Ashfall.Core.SystemTextJsonSerializer();
                var catalog = ReconTelemetryCatalogLoader.Load(dataDir, fileIO, serializer);
                System.LoadCatalog(catalog);
            }
            catch (Exception ex)
            {
                GD.PushWarning($"[Ashfall Godot] ReconTelemetry catalog load failed: {ex.Message}");
            }
        }

        public void TickDay(int day) => System.TickDay(day);
        public ReconTelemetryState CaptureState() => System.CaptureState();
        public void RestoreState(ReconTelemetryState state) => System.RestoreState(state);
    }
}
