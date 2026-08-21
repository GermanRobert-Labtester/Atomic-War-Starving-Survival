using System;
using System.Collections.Generic;

namespace Ashfall.Core
{
    [Serializable]
    public sealed class AirlockSecurityState
    {
        public string systemId = AirlockSecuritySystem.SystemId;
        public float blastDoorIntegrity = 100f;
        public AirlockDoorState doorState;
        public string sentryId = string.Empty;
        public float alertness = 100f;
        public string visitorId = string.Empty;
        public string visitorType = string.Empty;
        public bool visitorQuarantined;
        public VisitorDecision pendingDecision;
        public bool hasActiveIncident;
        public List<AirlockIncidentLog> incidentLog = new List<AirlockIncidentLog>();
        public int totalAdmissions;
        public int totalTurnaways;
    }

    public enum AirlockDoorState { Secure, Cycling, Open, Breached }
    public enum VisitorDecision { None, Admit, Inspect, Quarantine, TurnAway, Defend }

    [Serializable]
    public sealed class AirlockIncidentLog
    {
        public int day;
        public string visitorId = string.Empty;
        public VisitorDecision decision;
        public string outcome = string.Empty;
    }

    public sealed class AirlockSecuritySystem
    {
        public const string SystemId = "airlock_security";
        private AirlockSecurityState _state = new AirlockSecurityState();
        private readonly ISeededRng _rng;
        private readonly ILog _log;
        private int _currentDay;

        public AirlockSecurityState State => _state;
        public bool HasPendingIncident => _state.hasActiveIncident;
        public event Action<AirlockIncidentLog> OnIncidentResolved;
        public event Action OnSecurityChanged;

        public AirlockSecuritySystem(ISeededRng rng, ILog log = null!)
        {
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            _log = log ?? NullLog.Instance;
        }

        public void AssignSentry(string dwellerId)
        {
            _state.sentryId = dwellerId ?? string.Empty;
            OnSecurityChanged?.Invoke();
        }

        public ActionResult CycleDoor(AirlockDoorState newState)
        {
            if (_state.doorState == AirlockDoorState.Breached)
                return ActionResult.Blocked("door_breached", "airlock.door_breached");
            _state.doorState = newState;
            OnSecurityChanged?.Invoke();
            return ActionResult.Success("airlock.door_cycled",
                new Dictionary<string, double> { { "state", (int)newState } });
        }

        public ActionResult VisitorArrives(string visitorId, string visitorType)
        {
            _state.visitorId = visitorId ?? string.Empty;
            _state.visitorType = visitorType ?? string.Empty;
            _state.visitorQuarantined = false;
            _state.pendingDecision = VisitorDecision.None;
            _state.hasActiveIncident = true;
            OnSecurityChanged?.Invoke();
            return ActionResult.Success("airlock.visitor_arrived",
                new Dictionary<string, double> { { "type", string.IsNullOrEmpty(visitorType) ? 0 : visitorType.Length } });
        }

        public ActionResult ResolveIncident(VisitorDecision decision)
        {
            if (!_state.hasActiveIncident)
                return ActionResult.Blocked("no_incident", "airlock.no_incident");

            _state.pendingDecision = decision;
            string outcome;
            switch (decision)
            {
                case VisitorDecision.Admit:
                    _state.totalAdmissions++;
                    outcome = $"Admitted {_state.visitorType} '{_state.visitorId}'";
                    break;
                case VisitorDecision.Inspect:
                    _state.visitorQuarantined = _rng.NextDouble() < 0.3f;
                    outcome = _state.visitorQuarantined
                        ? $"Quarantined {_state.visitorId} (contamination detected)"
                        : $"Cleared {_state.visitorId} after inspection";
                    break;
                case VisitorDecision.Quarantine:
                    _state.visitorQuarantined = true;
                    outcome = $"Quarantined {_state.visitorId}";
                    break;
                case VisitorDecision.TurnAway:
                    _state.totalTurnaways++;
                    outcome = $"Turned away {_state.visitorId}";
                    break;
                case VisitorDecision.Defend:
                    _state.blastDoorIntegrity = Math.Max(0, _state.blastDoorIntegrity - 15f);
                    outcome = $"Defended against {_state.visitorId} — door damaged";
                    break;
                default:
                    outcome = $"No action for {_state.visitorId}";
                    break;
            }

            var log = new AirlockIncidentLog
            {
                day = _currentDay, visitorId = _state.visitorId,
                decision = decision, outcome = outcome
            };
            _state.incidentLog.Add(log);
            _state.hasActiveIncident = false;
            _log.Info($"[Airlock] {outcome}");
            OnIncidentResolved?.Invoke(log);
            OnSecurityChanged?.Invoke();
            return ActionResult.Success("airlock.incident_resolved",
                new Dictionary<string, double> { { "decision", (int)decision } });
        }

        public void TickDay(int day)
        {
            _currentDay = day;
            _state.alertness = Math.Min(100f, _state.alertness + 5f);
            if (!string.IsNullOrEmpty(_state.sentryId))
                _state.alertness = Math.Min(100f, _state.alertness + 10f);
        }

        public ActionResult RepairDoor(float amount)
        {
            _state.blastDoorIntegrity = Math.Min(100f, _state.blastDoorIntegrity + amount);
            if (_state.blastDoorIntegrity > 50f && _state.doorState == AirlockDoorState.Breached)
                _state.doorState = AirlockDoorState.Secure;
            OnSecurityChanged?.Invoke();
            return ActionResult.Success("airlock.door_repaired",
                new Dictionary<string, double> { { "integrity", _state.blastDoorIntegrity } });
        }

        public AirlockSecurityState CaptureState() => _state;
        public void RestoreState(AirlockSecurityState saved)
        {
            if (saved == null) return;
            _state = saved;
            OnSecurityChanged?.Invoke();
        }
    }
}
