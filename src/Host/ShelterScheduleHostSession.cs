using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Host session for ShelterScheduleSystem.
    /// Manages shelter work shifts, sleep assignments, curfews, lighting demand, and emergency overrides.
    /// </summary>
    public sealed class ShelterScheduleHostSession
    : HostSessionBase{
        public ShelterScheduleSystem System { get; }
        public string LastEvent { get; private set; } = string.Empty;
        public ShelterScheduleHostSession(ShelterScheduleSystem system)
        {
            if (system == null)
            {
                var state = new PowerGridState { GenerationWatts = 800, FuelUnits = 100, BatteryCapacityWh = 4000, BatteryReserveWh = 2000 };
                var rooms = new List<PowerGridRoom> { new PowerGridRoom("room_main", "Main Vault", 100f) };
                var grid = new PowerGridSystem(state, rooms, new SeededRng(1986));
                system = new ShelterScheduleSystem(grid, new GodotLog());
            }
            System = system;

            System.OnPhaseChanged += phase =>
            {
                LastEvent = $"[Schedule] Phase changed to {phase}";
                RaiseStateChanged();
            };

            System.OnScheduleChanged += () =>
            {
                RaiseStateChanged();
            };
        }

        public ActionResult SetCurfew(bool active)
        {
            var res = System.SetCurfew(active);
            if (res.IsSuccess)
            {
                LastEvent = $"Shelter curfew set to: {(active ? "ACTIVE" : "INACTIVE")}";
                RaiseStateChanged();
            }
            return res;
        }

        public ActionResult SetEmergencyOverride(bool active)
        {
            var res = System.SetEmergencyOverride(active);
            if (res.IsSuccess)
            {
                LastEvent = $"Emergency schedule override set to: {(active ? "ACTIVE" : "OFF")}";
                RaiseStateChanged();
            }
            return res;
        }

        public ActionResult AssignBed(string survivorId, string bedId)
        {
            var res = System.AssignBed(survivorId, bedId);
            if (res.IsSuccess)
            {
                LastEvent = $"Assigned dweller {survivorId} to bunk {bedId}";
                RaiseStateChanged();
            }
            return res;
        }

        /// <summary>Load the shelter_schedules.json catalog into the Core system (the authority).</summary>
        public void LoadCatalog(string dataDir)
        {
            if (string.IsNullOrEmpty(dataDir)) return;
            var fileIO = new FileSystemIO();
            var serializer = new SystemTextJsonSerializer();
            int count = ShelterScheduleCatalogLoader.LoadAndRegister(System, dataDir, fileIO, serializer);
            if (count > 0)
            {
                LastEvent = $"Shelter schedule catalog loaded: {count} schedules";
                RaiseStateChanged();
            }
        }

        public void TickDay(int day)
        {
            System.TickDay(day);
            RaiseStateChanged();
        }
    }
}
