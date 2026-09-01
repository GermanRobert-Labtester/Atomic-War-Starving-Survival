using System;
using Godot;
using Ashfall.Core;
using Ashfall.Core.PlayerCommand;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Host session for AirlockSecuritySystem.
    /// Manages blast door state, sentry assignments, visitor triage/quarantine, and security incidents.
    /// </summary>
    public sealed class AirlockSecurityHostSession
    : HostSessionBase{
        public AirlockSecuritySystem System { get; }
        public string LastEvent { get; private set; } = string.Empty;
        public AirlockSecurityHostSession(AirlockSecuritySystem system)
        {
            System = system ?? new AirlockSecuritySystem(new SeededRng(1986), new GodotLog());

            System.OnIncidentResolved += log =>
            {
                LastEvent = $"[Airlock] Incident resolved for {log.visitorId}: Decision {log.decision}, Outcome: {log.outcome}";
            };

            System.OnSecurityChanged += () =>
            {
                RaiseStateChanged();
            };
        }

        public void AssignSentry(string dwellerId)
        {
            System.AssignSentry(dwellerId);
            LastEvent = $"Assigned sentry: {dwellerId}";
        }

        public ActionResult CycleDoor(AirlockDoorState newState)
        {
            var res = System.CycleDoor(newState);
            if (res.IsSuccess)
            {
                LastEvent = $"Airlock blast door cycled to {newState}";
            }
            return res;
        }

        public ActionResult VisitorArrives(string visitorId, string visitorType)
        {
            var res = System.VisitorArrives(visitorId, visitorType);
            if (res.IsSuccess)
            {
                LastEvent = $"Visitor arrived at airlock: {visitorType} ({visitorId})";
            }
            return res;
        }

        public ActionResult ResolveIncident(VisitorDecision decision)
        {
            var res = System.ResolveIncident(decision);
            if (res.IsSuccess)
            {
                LastEvent = $"Security incident resolved: {decision}";
            }
            return res;
        }

        public CommandResult RepairDoor(float amount)
        {
            var result = System.ExecuteRepairDoor(amount, expectedStateVersion: StateVersion, currentStateVersion: StateVersion);
            if (result.IsSuccess)
            {
                LastEvent = $"Blast door repaired: {result.FailureCode}";
                RaiseStateChanged();
            }
            return result;
        }

        public void TickDay(int day)
        {
            System.TickDay(day);
        }

        public override void Save()
        {
            if (!IsDirty) return;
            AirlockSecuritySaveStore.TrySave(System.CaptureState());
            base.Save();
        }
    }
}
