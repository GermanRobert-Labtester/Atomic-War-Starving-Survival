using System;
using Godot;
using Ashfall.Core;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Host session for AirlockSecuritySystem.
    /// Manages blast door state, sentry assignments, visitor triage/quarantine, and security incidents.
    /// </summary>
    public sealed class AirlockSecurityHostSession
    {
        public AirlockSecuritySystem System { get; }
        public string LastEvent { get; private set; } = string.Empty;

        public event Action? StateChanged;

        public AirlockSecurityHostSession(AirlockSecuritySystem system)
        {
            System = system ?? new AirlockSecuritySystem(new SeededRng(1986), new GodotLog());

            System.OnIncidentResolved += log =>
            {
                LastEvent = $"[Airlock] Incident resolved for {log.visitorId}: Decision {log.decision}, Outcome: {log.outcome}";
                StateChanged?.Invoke();
            };

            System.OnSecurityChanged += () =>
            {
                StateChanged?.Invoke();
            };
        }

        public void AssignSentry(string dwellerId)
        {
            System.AssignSentry(dwellerId);
            LastEvent = $"Assigned sentry: {dwellerId}";
            StateChanged?.Invoke();
        }

        public ActionResult CycleDoor(AirlockDoorState newState)
        {
            var res = System.CycleDoor(newState);
            if (res.IsSuccess)
            {
                LastEvent = $"Airlock blast door cycled to {newState}";
                StateChanged?.Invoke();
            }
            return res;
        }

        public ActionResult VisitorArrives(string visitorId, string visitorType)
        {
            var res = System.VisitorArrives(visitorId, visitorType);
            if (res.IsSuccess)
            {
                LastEvent = $"Visitor arrived at airlock: {visitorType} ({visitorId})";
                StateChanged?.Invoke();
            }
            return res;
        }

        public ActionResult ResolveIncident(VisitorDecision decision)
        {
            var res = System.ResolveIncident(decision);
            if (res.IsSuccess)
            {
                LastEvent = $"Security incident resolved: {decision}";
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
