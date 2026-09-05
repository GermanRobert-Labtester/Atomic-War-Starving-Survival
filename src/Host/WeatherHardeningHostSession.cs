// ============================================================================
// Host Session : WeatherHardeningHostSession
// Core System : Ashfall.Core.World.WeatherHardeningSystem
// ============================================================================
using System;
using Godot;
using Ashfall.Core.World;

namespace AtomicWar.GodotApp
{
    public sealed class WeatherHardeningHostSession
    {
        public WeatherHardeningSystem System { get; }
        public WeatherHardeningState State => System.State;

        public event Action? OnIntakeBlocked
        {
            add { System.OnIntakeBlocked += value; }
            remove { System.OnIntakeBlocked -= value; }
        }
        public event Action? OnIntakeCleared
        {
            add { System.OnIntakeCleared += value; }
            remove { System.OnIntakeCleared -= value; }
        }
        public event Action<string, int>? OnPipeFrozen
        {
            add { System.OnPipeFrozen += value; }
            remove { System.OnPipeFrozen -= value; }
        }
        public event Action<string, int, float>? OnPipeBurst
        {
            add { System.OnPipeBurst += value; }
            remove { System.OnPipeBurst -= value; }
        }
        public event Action<string>? OnInsulationCritical
        {
            add { System.OnInsulationCritical += value; }
            remove { System.OnInsulationCritical -= value; }
        }

        public WeatherHardeningHostSession(WeatherHardeningSystem system)
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
                var catalog = WeatherHardeningCatalogLoader.Load(dataDir, fileIO, serializer);
                System.LoadCatalog(catalog);
            }
            catch (Exception ex)
            {
                GD.PushWarning($"[Ashfall Godot] WeatherHardening catalog load failed: {ex.Message}");
            }
        }

        public void TickDay(int day) => System.TickDay(day);

        public WeatherHardeningState CaptureState() => System.CaptureState();
        public void RestoreState(WeatherHardeningState state) => System.RestoreState(state);
    }
}
