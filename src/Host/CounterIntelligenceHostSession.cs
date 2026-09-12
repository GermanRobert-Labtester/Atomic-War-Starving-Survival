// SPDX-License-Identifier: MIT
// ============================================================================
// Host Session : CounterIntelligenceHostSession
// Core System : Ashfall.Core.Factions.CounterIntelligenceSystem
// ============================================================================
using System;
using Godot;
using Ashfall.Core.Factions;

namespace AtomicWar.GodotApp
{
    public sealed class CounterIntelligenceHostSession : HostSessionBase
    {
        public CounterIntelligenceSystem System { get; }
        public CounterIntelligenceState State => System.State;

        public event Action<string, string>? OnCandidateFlagged
        {
            add { System.OnCandidateFlagged += value; }
            remove { System.OnCandidateFlagged -= value; }
        }
        public event Action<string>? OnAgentExposed
        {
            add { System.OnAgentExposed += value; }
            remove { System.OnAgentExposed -= value; }
        }
        public event Action<string, string, string>? OnSabotageDiscovered
        {
            add { System.OnSabotageDiscovered += value; }
            remove { System.OnSabotageDiscovered -= value; }
        }
        public event Action<string>? OnDefectorAccepted
        {
            add { System.OnDefectorAccepted += value; }
            remove { System.OnDefectorAccepted -= value; }
        }

        public CounterIntelligenceHostSession(CounterIntelligenceSystem system)
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
                var catalog = InfiltratorCatalogLoader.Load(dataDir, fileIO, serializer);
                System.LoadCatalog(catalog);
            }
            catch (Exception ex)
            {
                GD.PushWarning($"[Ashfall Godot] CounterIntelligence catalog load failed: {ex.Message}");
            }
        }

        public void TickDay(int day) => System.TickDay(day);
        public CounterIntelligenceState CaptureState() => System.CaptureState();
        public void RestoreState(CounterIntelligenceState state) => System.RestoreState(state);
    }
}
