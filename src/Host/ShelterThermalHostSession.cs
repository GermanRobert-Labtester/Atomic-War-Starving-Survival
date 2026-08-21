using System;
using Godot;
using Ashfall.Core;
using Ashfall.Core.StartingLevel;
using Ashfall.Core.Survivors;
using Ashfall.Core.YearOfAsh;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Host session for ShelterThermalSystem.
    /// Manages boiler fuel, heating zones, radiator valves, pipe freeze/burst risks, and thermal incidents.
    /// </summary>
    public sealed class ShelterThermalHostSession
    {
        public ShelterThermalSystem System { get; }
        public string LastEvent { get; private set; } = string.Empty;

        public event Action? StateChanged;

        public ShelterThermalHostSession(ShelterThermalSystem system)
        {
            if (system == null)
            {
                var rng = new SeededRng(1986);
                var needs = new NeedsSystem();
                var starting = new StartingLevelSystem();
                var deepFreeze = new YearOfAshDeepFreezeSystem(new YearOfAshDeepFreezeState());
                system = new ShelterThermalSystem(rng, needs, starting, deepFreeze, new GodotLog());
            }
            System = system;

            System.OnIncident += inc =>
            {
                LastEvent = $"[Thermal] INCIDENT: {inc.kind} in {inc.roomId} (Pipe {inc.pipeId})";
                StateChanged?.Invoke();
            };

            System.OnThermalChanged += () =>
            {
                StateChanged?.Invoke();
            };
        }

        public ActionResult SetBoilerActive(bool active)
        {
            var res = System.SetBoilerActive(active);
            if (res.IsSuccess)
            {
                LastEvent = $"Boiler status set to: {(active ? "ACTIVE" : "OFF")}";
                StateChanged?.Invoke();
            }
            return res;
        }

        public ActionResult SetRadiatorValve(string roomId, float openRatio)
        {
            var res = System.SetRadiatorValve(roomId, openRatio);
            if (res.IsSuccess)
            {
                StateChanged?.Invoke();
            }
            return res;
        }

        public void TickDay(int day)
        {
            System.TickDay(day);
            StateChanged?.Invoke();
        }
    }
}
