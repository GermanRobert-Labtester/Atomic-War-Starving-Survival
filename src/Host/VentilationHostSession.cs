using System;
#pragma warning disable CS8618
using Ashfall.Core;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Host session for the Plan 72 electrostatic ventilation stage. Thin
    /// adapter: exposes the canonical VentilationSystem stage surface for Godot
    /// panel binding. Core owns all filtration math; this session only adapts
    /// events and formats last-event feedback.
    /// </summary>
    public sealed class VentilationHostSession : HostSessionBase
    {
        public VentilationSystem System { get; }
        public string LastEvent { get; private set; } = string.Empty;

        public VentilationHostSession(VentilationSystem system)
        {
            System = system ?? throw new ArgumentNullException(nameof(system));
            System.OnHazardWarning += entry =>
            {
                LastEvent = entry.message;
                RaiseStateChanged();
            };
            System.OnVentilationChanged += () =>
            {
                RaiseStateChanged();
            };
        }

        public ActionResult InstallStage(string stageId, string roomId)
        {
            var res = System.InstallElectrostaticStage(stageId, roomId);
            LastEvent = res.IsSuccess ? "Electrostatic stage installed" : $"Install blocked: {res.FailureCode}";
            RaiseStateChanged();
            return res;
        }

        public ActionResult SetProfile(string profileId)
        {
            var res = System.SetStageProfile(profileId);
            LastEvent = res.IsSuccess ? $"Profile set: {profileId}" : $"Profile blocked: {res.FailureCode}";
            RaiseStateChanged();
            return res;
        }

        public ActionResult RapPlates()
        {
            var res = System.RapPlates();
            LastEvent = res.IsSuccess ? "Plates rapped into hopper" : $"Rapping blocked: {res.FailureCode}";
            RaiseStateChanged();
            return res;
        }

        public ActionResult EmptyHopper(int maxDrums = 4)
        {
            var res = System.EmptyHopperToDrums(maxDrums);
            LastEvent = res.IsSuccess ? "Dust drum sealed" : $"Emptying blocked: {res.FailureCode}";
            RaiseStateChanged();
            return res;
        }

        public ActionResult ServiceStage()
        {
            var res = System.ServiceElectrostaticStage();
            LastEvent = res.IsSuccess ? "Stage serviced" : $"Service blocked: {res.FailureCode}";
            RaiseStateChanged();
            return res;
        }
    }
}
