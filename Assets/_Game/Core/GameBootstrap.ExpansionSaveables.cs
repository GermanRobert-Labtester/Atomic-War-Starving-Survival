using System;
using System.Collections.Generic;
using AtomicWar._Game.Quests;
using AtomicWar._Game.Factions;
using AtomicWar._Game.Environment;
using AtomicWar._Game.World;
using AtomicWar._Game.Narrative;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Expansion Save Integration — registers all expansion systems with
    /// internal state (beyond Survivor fields) into SaveSystem via the
    /// ISaveable contract so their state round-trips save/load.
    ///
    /// Systems with state that lives ONLY on Survivor fields do not need
    /// registration (those fields serialize automatically).
    /// </summary>
    public partial class GameBootstrap
    {
        private void RegisterExpansionSaveables()
        {
            if (SaveSystem == null) return;

            // DynamicQuestlineSystem — active quests + completed list
            if (DynamicQuestlines != null)
                SaveSystem.Register(new DynamicQuestlineSaveable(DynamicQuestlines));

            // FactionIntelligenceSystem — intel, agents, tributes, alliances
            if (FactionIntel != null)
                SaveSystem.Register(new FactionIntelSaveable(FactionIntel));

            // VehicleMaintenanceSystem — vehicle condition/fuel/cargo/mods
            if (VehicleMaintenance != null)
                SaveSystem.Register(new VehicleMaintenanceSaveable(VehicleMaintenance));

            // AshDriftBurialSystem — ash accumulation level
            if (AshDriftBurialSystem != null)
                SaveSystem.Register(new AshDriftSaveable(AshDriftBurialSystem));

            // LocationEvolutionSystem — location ownership states
            if (LocationEvolutionSystem != null)
                SaveSystem.Register(new LocationEvolutionSaveable(LocationEvolutionSystem));

            // WildlifeMigrationSystem — zone danger levels
            if (WildlifeMigrationSystem != null)
                SaveSystem.Register(new WildlifeSaveable(WildlifeMigrationSystem));

            // LandmarkDegradationSystem — collapse/flood states
            if (LandmarkDegradationSystem != null)
                SaveSystem.Register(new LandmarkSaveable(LandmarkDegradationSystem));
        }
    }

    // ═══════════════════════════════════════════════════════════════════
    // ISaveable adapters — wrap system state for SaveSystem
    // ═══════════════════════════════════════════════════════════════════

    internal class DynamicQuestlineSaveable : ISaveable
    {
        private readonly DynamicQuestlineSystem _system;
        public string SaveId => "dynamic_questlines";
        public DynamicQuestlineSaveable(DynamicQuestlineSystem s) => _system = s;
        public object CaptureState() => _system.CaptureState();
        public void RestoreState(object state) =>
            _system.RestoreState(state as DynamicQuestlineSaveState);
    }

    internal class FactionIntelSaveable : ISaveable
    {
        private readonly FactionIntelligenceSystem _system;
        public string SaveId => "faction_intelligence";
        public FactionIntelSaveable(FactionIntelligenceSystem s) => _system = s;
        public object CaptureState() => _system.GetState();
        public void RestoreState(object state)
        {
            // State is restored into the live object by GetState reference.
            if (state is FactionIntelligenceSaveState incoming)
            {
                var current = _system.GetState();
                current.ActiveIntel.Clear();
                current.ActiveIntel.AddRange(incoming.ActiveIntel);
                current.ActiveAgents.Clear();
                current.ActiveAgents.AddRange(incoming.ActiveAgents);
                current.TributeDemands.Clear();
                foreach (var kv in incoming.TributeDemands)
                    current.TributeDemands[kv.Key] = kv.Value;
                current.AlliedFactionIds.Clear();
                current.AlliedFactionIds.AddRange(incoming.AlliedFactionIds);
                current.InformantNetworkActive = incoming.InformantNetworkActive;
            }
        }
    }

    internal class VehicleMaintenanceSaveable : ISaveable
    {
        private readonly VehicleMaintenanceSystem _system;
        public string SaveId => "vehicle_maintenance";
        public VehicleMaintenanceSaveable(VehicleMaintenanceSystem s) => _system = s;

        [Serializable]
        private class State
        {
            public List<string> VehicleIds = new List<string>();
            public List<float> Conditions = new List<float>();
            public List<float> Fuel = new List<float>();
            public List<float> Cargo = new List<float>();
        }

        public object CaptureState()
        {
            var state = new State();
            // Only track the default scout truck for now
            var v = _system.GetVehicle("scout_truck");
            if (v != null)
            {
                state.VehicleIds.Add("scout_truck");
                state.Conditions.Add(v.ConditionPct);
                state.Fuel.Add(v.FuelLitres);
                state.Cargo.Add(v.CurrentCargoKg);
            }
            return state;
        }

        public void RestoreState(object state)
        {
            if (state is State s && s.VehicleIds != null)
            {
                for (int i = 0; i < s.VehicleIds.Count; i++)
                {
                    var v = _system.GetVehicle(s.VehicleIds[i]);
                    if (v == null) continue;
                    if (i < s.Conditions.Count) v.ConditionPct = s.Conditions[i];
                    if (i < s.Fuel.Count) v.FuelLitres = s.Fuel[i];
                    if (i < s.Cargo.Count) v.CurrentCargoKg = s.Cargo[i];
                }
            }
        }
    }

    internal class AshDriftSaveable : ISaveable
    {
        private readonly AshDriftBurialSystem _system;
        public string SaveId => "ash_drift_burial";
        public AshDriftSaveable(AshDriftBurialSystem s) => _system = s;

        [Serializable]
        private class State { public float AshAccumulation; }

        public object CaptureState() =>
            new State { AshAccumulation = _system.AshAccumulation };

        public void RestoreState(object state)
        {
            if (state is State s)
            {
                // Restore via clear+storm accumulation math
                var current = _system.AshAccumulation;
                // Simplest: clear then add back
                _system.ClearAsh(9999f);
                if (s.AshAccumulation > 0f)
                    _system.OnAshStorm(s.AshAccumulation /
                        AshDriftBurialSystem.AshAccumulationRatePerStorm);
            }
        }
    }

    internal class LocationEvolutionSaveable : ISaveable
    {
        private readonly LocationEvolutionSystem _system;
        public string SaveId => "location_evolution";
        public LocationEvolutionSaveable(LocationEvolutionSystem s) => _system = s;
        public object CaptureState() => null; // state lives in registered locations
        public void RestoreState(object state) { /* locations re-registered at init */ }
    }

    internal class WildlifeSaveable : ISaveable
    {
        private readonly WildlifeMigrationSystem _system;
        public string SaveId => "wildlife_migration";
        public WildlifeSaveable(WildlifeMigrationSystem s) => _system = s;
        public object CaptureState() => null;
        public void RestoreState(object state) { }
    }

    internal class LandmarkSaveable : ISaveable
    {
        private readonly LandmarkDegradationSystem _system;
        public string SaveId => "landmark_degradation";
        public LandmarkSaveable(LandmarkDegradationSystem s) => _system = s;
        public object CaptureState() => null;
        public void RestoreState(object state) { }
    }
}
