// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
#pragma warning disable CS8618

using Ashfall.Core.PlayerCommand;

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
        public int lastSecurityDay = -1;
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
        public const int MaxIncidentLogEntries = 256;
        private AirlockSecurityState _state = new AirlockSecurityState();
        private readonly ISeededRng _rng;
        private readonly ILog _log;
        private int _currentDay;

        public AirlockSecurityState State => CaptureState();
        public bool HasPendingIncident => _state.hasActiveIncident;
        public event Action<AirlockIncidentLog> OnIncidentResolved;
        public event Action OnSecurityChanged;

        public AirlockSecuritySystem(ISeededRng rng, ILog? log = null)
        {
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            _log = log ?? NullLog.Instance;
        }

        public void AssignSentry(string dwellerId)
        {
            _state.sentryId = NormalizeId(dwellerId);
            OnSecurityChanged?.Invoke();
        }

        public ActionResult CycleDoor(AirlockDoorState newState)
        {
            if (!IsDefinedDoorState(newState))
                return ActionResult.Blocked("invalid_door_state", "airlock.invalid_door_state");
            if (_state.doorState == AirlockDoorState.Breached)
                return ActionResult.Blocked("door_breached", "airlock.door_breached");
            _state.doorState = newState;
            OnSecurityChanged?.Invoke();
            return ActionResult.Success("airlock.door_cycled",
                new Dictionary<string, double> { { "state", (int)newState } });
        }

        public ActionResult VisitorArrives(string visitorId, string visitorType)
        {
            if (_state.hasActiveIncident)
                return ActionResult.Blocked("incident_active", "airlock.incident_active");

            visitorId = NormalizeId(visitorId);
            if (string.IsNullOrWhiteSpace(visitorId))
                return ActionResult.Blocked("invalid_visitor", "airlock.invalid_visitor");
            visitorType = NormalizeId(visitorType);
            _state.visitorId = visitorId;
            _state.visitorType = visitorType;
            _state.visitorQuarantined = false;
            _state.pendingDecision = VisitorDecision.None;
            _state.hasActiveIncident = true;
            OnSecurityChanged?.Invoke();
            return ActionResult.Success("airlock.visitor_arrived",
                new Dictionary<string, double> { { "type", visitorType.Length } });
        }

        public ActionResult ResolveIncident(VisitorDecision decision)
        {
            if (!_state.hasActiveIncident)
                return ActionResult.Blocked("no_incident", "airlock.no_incident");
            if (!IsResolvedDecision(decision))
                return ActionResult.Blocked("invalid_decision", "airlock.invalid_decision");

            _state.pendingDecision = decision;
            string outcome;
            switch (decision)
            {
                case VisitorDecision.Admit:
                    SaturatingIncrement(ref _state.totalAdmissions);
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
                    SaturatingIncrement(ref _state.totalTurnaways);
                    outcome = $"Turned away {_state.visitorId}";
                    break;
                case VisitorDecision.Defend:
                    _state.blastDoorIntegrity = Math.Max(0, _state.blastDoorIntegrity - 15f);
                    outcome = $"Defended against {_state.visitorId} — door damaged";
                    break;
                default:
                    return ActionResult.Blocked("invalid_decision", "airlock.invalid_decision");
            }

            var log = new AirlockIncidentLog
            {
                day = _currentDay, visitorId = _state.visitorId,
                decision = decision, outcome = outcome
            };
            _state.incidentLog.Add(CloneIncident(log));
            TrimIncidentHistory(_state.incidentLog);
            _state.hasActiveIncident = false;
            _log.Info($"[Airlock] {outcome}");
            OnIncidentResolved?.Invoke(CloneIncident(log));
            OnSecurityChanged?.Invoke();
            return ActionResult.Success("airlock.incident_resolved",
                new Dictionary<string, double> { { "decision", (int)decision } });
        }

        public void TickDay(int day)
        {
            if (day < 0)
                throw new ArgumentOutOfRangeException(nameof(day), "Campaign day cannot be negative.");
            if (day <= _state.lastSecurityDay) return;

            float gain = string.IsNullOrWhiteSpace(_state.sentryId) ? 5f : 15f;
            _state.alertness = SanitizePercent(_state.alertness + gain);
            _state.lastSecurityDay = day;
            _currentDay = day;
            OnSecurityChanged?.Invoke();
        }

        public ActionResult RepairDoor(float amount)
        {
            if (!IsFinite(amount) || amount <= 0f)
                return ActionResult.Blocked("invalid_amount", "airlock.invalid_amount");

            _state.blastDoorIntegrity = SanitizePercent(_state.blastDoorIntegrity + amount);
            if (_state.blastDoorIntegrity > 50f && _state.doorState == AirlockDoorState.Breached)
                _state.doorState = AirlockDoorState.Secure;
            OnSecurityChanged?.Invoke();
            return ActionResult.Success("airlock.door_repaired",
                new Dictionary<string, double> { { "integrity", _state.blastDoorIntegrity } });
        }

        /// <summary>
        /// Side-effect-free preview of a door repair action.
        /// Shares the same validation path as <see cref="RepairDoor"/>.
        /// </summary>
        public CommandPreview PreviewRepairDoor(float amount, long stateVersion = 0)
        {
            if (!IsFinite(amount) || amount <= 0f)
                return CommandPreview.Unavailable(PlayerCommandCode.RepairDoor, "invalid_amount", "airlock.invalid_amount", stateVersion);

            float current = SanitizePercent(_state.blastDoorIntegrity);
            float projected = SanitizePercent(current + amount);
            bool restoresBreachedDoor = _state.doorState == AirlockDoorState.Breached && projected > 50f;

            var deltas = new Dictionary<string, double>
            {
                { "integrity", projected - current }
            };
            if (restoresBreachedDoor)
                deltas["door_state"] = (int)AirlockDoorState.Secure;

            return CommandPreview.Available(
                PlayerCommandCode.RepairDoor,
                stateVersion,
                deltas,
                isIrreversible: false,
                messageKey: "airlock.preview_repair");
        }

        /// <summary>
        /// Execute a door repair using the same validation path as <see cref="PreviewRepairDoor"/>.
        /// Stale previews (state version mismatch) are rejected without mutation.
        /// </summary>
        public CommandResult ExecuteRepairDoor(float amount, long expectedStateVersion = 0, long currentStateVersion = 0)
        {
            var preview = PreviewRepairDoor(amount, expectedStateVersion);
            if (!preview.IsAvailable)
                return CommandResult.FromPreview(preview);

            if (preview.StateVersion != currentStateVersion)
                return CommandResult.StalePreview(PlayerCommandCode.RepairDoor, preview.StateVersion, currentStateVersion);

            var result = RepairDoor(amount);
            if (!result.IsSuccess)
                return new CommandResult(
                    PlayerCommandCode.RepairDoor,
                    result,
                    expectedStateVersion,
                    currentStateVersion);

            return CommandResult.FromSuccess(
                PlayerCommandCode.RepairDoor,
                result,
                expectedStateVersion,
                currentStateVersion + 1);
        }

        public AirlockSecurityState CaptureState() => NormalizeState(_state);

        public void RestoreState(AirlockSecurityState saved)
        {
            if (saved == null) return;
            _state = NormalizeState(saved);
            _currentDay = Math.Max(0, _state.lastSecurityDay);
        }

        private static AirlockSecurityState NormalizeState(AirlockSecurityState source)
        {
            var normalized = new AirlockSecurityState
            {
                systemId = SystemId,
                blastDoorIntegrity = SanitizePercent(source.blastDoorIntegrity),
                doorState = IsDefinedDoorState(source.doorState) ? source.doorState : AirlockDoorState.Secure,
                sentryId = NormalizeId(source.sentryId),
                alertness = SanitizePercent(source.alertness),
                visitorId = NormalizeId(source.visitorId),
                visitorType = NormalizeId(source.visitorType),
                visitorQuarantined = source.visitorQuarantined,
                pendingDecision = IsDefinedDecision(source.pendingDecision) ? source.pendingDecision : VisitorDecision.None,
                hasActiveIncident = source.hasActiveIncident && !string.IsNullOrWhiteSpace(source.visitorId),
                totalAdmissions = Math.Max(0, source.totalAdmissions),
                totalTurnaways = Math.Max(0, source.totalTurnaways),
                lastSecurityDay = Math.Max(-1, source.lastSecurityDay),
                incidentLog = new List<AirlockIncidentLog>()
            };

            if (source.incidentLog != null)
            {
                foreach (var incident in source.incidentLog)
                {
                    if (incident == null || incident.day < 0 || !IsResolvedDecision(incident.decision))
                        continue;
                    normalized.incidentLog.Add(CloneIncident(incident));
                }
            }
            TrimIncidentHistory(normalized.incidentLog);
            if (normalized.lastSecurityDay < 0 && normalized.incidentLog.Count > 0)
            {
                foreach (var incident in normalized.incidentLog)
                    normalized.lastSecurityDay = Math.Max(normalized.lastSecurityDay, incident.day);
            }
            return normalized;
        }

        private static AirlockIncidentLog CloneIncident(AirlockIncidentLog source) => new AirlockIncidentLog
        {
            day = Math.Max(0, source.day),
            visitorId = NormalizeId(source.visitorId),
            decision = source.decision,
            outcome = source.outcome?.Trim() ?? string.Empty
        };

        private static void TrimIncidentHistory(List<AirlockIncidentLog> incidentLog)
        {
            int excess = incidentLog.Count - MaxIncidentLogEntries;
            if (excess > 0) incidentLog.RemoveRange(0, excess);
        }

        private static void SaturatingIncrement(ref int value)
        {
            if (value < 0) value = 0;
            else if (value < int.MaxValue) value++;
        }

        private static bool IsFinite(float value) =>
            !float.IsNaN(value) && !float.IsInfinity(value);

        private static float SanitizePercent(float value) =>
            IsFinite(value) ? Math.Clamp(value, 0f, 100f) : 0f;

        private static string NormalizeId(string? value) => value?.Trim() ?? string.Empty;

        private static bool IsDefinedDoorState(AirlockDoorState value) =>
            Enum.IsDefined(typeof(AirlockDoorState), value);

        private static bool IsDefinedDecision(VisitorDecision value) =>
            Enum.IsDefined(typeof(VisitorDecision), value);

        private static bool IsResolvedDecision(VisitorDecision value) =>
            IsDefinedDecision(value) && value != VisitorDecision.None;
    }
}
