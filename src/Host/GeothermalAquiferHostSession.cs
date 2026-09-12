// SPDX-License-Identifier: MIT
// ============================================================================
// Host Session : GeothermalAquiferHostSession
// Core System : Ashfall.Core.Shelter.GeothermalAquiferSystem
// ============================================================================
using System;
using Godot;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    public sealed class GeothermalAquiferHostSession : HostSessionBase
    {
        public GeothermalAquiferSystem System { get; }
        public GeothermalAquiferState State => System.State;

        public event Action<string>? OnStrataReached
        {
            add { System.OnStrataReached += value; }
            remove { System.OnStrataReached -= value; }
        }
        public event Action? OnSteamPocketReached
        {
            add { System.OnSteamPocketReached += value; }
            remove { System.OnSteamPocketReached -= value; }
        }
        public event Action? OnAquiferReached
        {
            add { System.OnAquiferReached += value; }
            remove { System.OnAquiferReached -= value; }
        }
        public event Action? OnTurbineCommissioned
        {
            add { System.OnTurbineCommissioned += value; }
            remove { System.OnTurbineCommissioned -= value; }
        }
        public event Action? OnTurbineOffline
        {
            add { System.OnTurbineOffline += value; }
            remove { System.OnTurbineOffline -= value; }
        }

        public GeothermalAquiferHostSession(GeothermalAquiferSystem system)
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
                var catalog = GeothermalCatalogLoader.Load(dataDir, fileIO, serializer);
                System.LoadCatalog(catalog);
            }
            catch (Exception ex)
            {
                GD.PushWarning($"[Ashfall Godot] Geothermal catalog load failed: {ex.Message}");
            }
        }

        public void TickDay(int day) => System.TickDay(day);
        public GeothermalAquiferState CaptureState() => System.CaptureState();
        public void RestoreState(GeothermalAquiferState state) => System.RestoreState(state);
    }
}
