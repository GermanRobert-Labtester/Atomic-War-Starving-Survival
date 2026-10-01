// SPDX-License-Identifier: MIT
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
            LastEvent = res.IsSuccess
                ? $"Airlock blast door cycled to {newState}"
                : $"Airlock door cycle refused: {res.FailureCode}";
            RaiseStateChanged();
            return res;
        }

        public ActionResult VisitorArrives(string visitorId, string visitorType)
        {
            var res = System.VisitorArrives(visitorId, visitorType);
            LastEvent = res.IsSuccess
                ? $"Visitor arrived at airlock: {visitorType} ({visitorId})"
                : $"Visitor arrival refused: {res.FailureCode}";
            RaiseStateChanged();
            return res;
        }

        public ActionResult ResolveIncident(VisitorDecision decision)
        {
            var res = System.ResolveIncident(decision);
            LastEvent = res.IsSuccess
                ? $"Security incident resolved: {decision}"
                : $"Visitor decision refused: {res.FailureCode}";
            RaiseStateChanged();
            return res;
        }

        public CommandResult RepairDoor(float amount)
        {
            var result = System.ExecuteRepairDoor(amount, expectedStateVersion: StateVersion, currentStateVersion: StateVersion);
            LastEvent = result.IsSuccess
                ? $"Blast door repaired (+{amount:F0})."
                : $"Blast door repair refused: {result.FailureCode}";
            RaiseStateChanged();
            return result;
        }

        public void TickDay(int day)
        {
            System.TickDay(day);
        }

    }
}
