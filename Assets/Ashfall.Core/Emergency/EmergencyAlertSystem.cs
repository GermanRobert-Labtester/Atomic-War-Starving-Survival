// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 194 — Emergency Alert & Warning System
// Pure domain authority for centralized threat detection, alert prioritization,
// response window tracking, early evacuation protocols, and resolution logging.
// ============================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Ashfall.Core.Emergency
{
    // ── Catalog DTOs ────────────────────────────────────────────────────────

    [Serializable]
    public sealed class EmergencyAlertTypeDef
    {
        public string type_id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public string severity { get; set; } = "warning"; // info, warning, critical, emergency
        public int base_priority { get; set; } = 5;       // 1 - 10
        public int response_window_hours { get; set; } = 6;
        public string recommended_protocol { get; set; } = "shelter_in_place";
        public string description { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class EmergencyAlertsCatalog
    {
        public int schema_version { get; set; } = 1;
        public List<EmergencyAlertTypeDef> alert_types { get; set; } = new List<EmergencyAlertTypeDef>();
    }

    // ── Persistent State DTOs ───────────────────────────────────────────────

    [Serializable]
    public sealed class ActiveEmergencyAlert
    {
        public string AlertId { get; set; } = string.Empty;
        public string TypeId { get; set; } = string.Empty;
        public string Severity { get; set; } = "warning";
        public int DetectedDay { get; set; } = 1;
        public int DetectedHour { get; set; }
        public int RemainingHours { get; set; } = 6;
        public string SourceSystem { get; set; } = string.Empty;
        public string AffectedZone { get; set; } = string.Empty;
        public bool IsAcknowledged { get; set; }
        public bool IsResolved { get; set; }
        public int CurrentPriority { get; set; } = 5;
    }

    [Serializable]
    public sealed class EvacuationProtocolState
    {
        public string ProtocolId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Status { get; set; } = "standby"; // standby, active, completed, cancelled
        public string AssemblyZone { get; set; } = string.Empty;
        public int AssignedPersonnelCount { get; set; }
    }

    [Serializable]
    public sealed class EmergencyAlertState
    {
        public int SchemaVersion { get; set; } = 1;
        public int NextSequence { get; set; } = 1;
        public List<ActiveEmergencyAlert> Alerts { get; set; } = new List<ActiveEmergencyAlert>();
        public List<EvacuationProtocolState> ActiveProtocols { get; set; } = new List<EvacuationProtocolState>();
        public List<ActiveEmergencyAlert> AlertHistory { get; set; } = new List<ActiveEmergencyAlert>();
    }

    // ── Domain System ───────────────────────────────────────────────────────

    public sealed class EmergencyAlertSystem
    {
        private readonly EmergencyAlertState _state;
        private readonly Dictionary<string, EmergencyAlertTypeDef> _definitions =
            new Dictionary<string, EmergencyAlertTypeDef>(StringComparer.OrdinalIgnoreCase);

        public event Action<ActiveEmergencyAlert>? OnAlertRaised;
        public event Action<string>? OnAlertAcknowledged;   // (alertId)
        public event Action<ActiveEmergencyAlert>? OnAlertEscalated;
        public event Action<string>? OnAlertResolved;       // (alertId)
        public event Action<EvacuationProtocolState>? OnProtocolActivated;

        public int ActiveAlertCount => _state.Alerts.Count(a => !a.IsResolved);
        public int HistoryCount => _state.AlertHistory.Count;

        public EmergencyAlertSystem()
        {
            _state = new EmergencyAlertState();
        }

        public EmergencyAlertSystem(EmergencyAlertState state)
        {
            _state = state ?? new EmergencyAlertState();
        }

        // ── Catalog Loading ────────────────────────────────────────────────

        public void LoadCatalog(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return;
            try
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var catalog = JsonSerializer.Deserialize<EmergencyAlertsCatalog>(json, options);
                if (catalog?.alert_types == null) return;

                _definitions.Clear();
                foreach (var a in catalog.alert_types)
                {
                    if (string.IsNullOrWhiteSpace(a.type_id)) continue;
                    _definitions[a.type_id] = a;
                }
            }
            catch (Exception) { /* malformed catalog falls back to built-in defaults; authoring errors are enforced by the data-integrity gate */ }
        }

        public IReadOnlyCollection<EmergencyAlertTypeDef> GetAllAlertTypes() => _definitions.Values;

        public EmergencyAlertTypeDef? GetAlertType(string typeId)
        {
            return _definitions.TryGetValue(typeId, out var def) ? def : null;
        }

        // ── Alert Lifecycle ────────────────────────────────────────────────

        public ActiveEmergencyAlert RaiseAlert(string typeId, string sourceSystem, string affectedZone, int day, int hour)
        {
            var def = GetAlertType(typeId);
            var alert = new ActiveEmergencyAlert
            {
                AlertId = $"alert_{_state.NextSequence++}",
                TypeId = typeId,
                Severity = def?.severity ?? "warning",
                DetectedDay = day,
                DetectedHour = hour,
                RemainingHours = def?.response_window_hours ?? 6,
                SourceSystem = sourceSystem,
                AffectedZone = affectedZone,
                IsAcknowledged = false,
                IsResolved = false,
                CurrentPriority = def?.base_priority ?? 5
            };

            _state.Alerts.Add(alert);
            OnAlertRaised?.Invoke(alert);
            return alert;
        }

        public bool AcknowledgeAlert(string alertId)
        {
            var alert = _state.Alerts.FirstOrDefault(a =>
                string.Equals(a.AlertId, alertId, StringComparison.OrdinalIgnoreCase));

            if (alert == null || alert.IsAcknowledged) return false;

            alert.IsAcknowledged = true;
            OnAlertAcknowledged?.Invoke(alertId);
            return true;
        }

        public bool ResolveAlert(string alertId)
        {
            var alert = _state.Alerts.FirstOrDefault(a =>
                string.Equals(a.AlertId, alertId, StringComparison.OrdinalIgnoreCase));

            if (alert == null || alert.IsResolved) return false;

            alert.IsResolved = true;
            _state.Alerts.Remove(alert);

            _state.AlertHistory.Add(alert);
            if (_state.AlertHistory.Count > 50)
                _state.AlertHistory.RemoveAt(0);

            OnAlertResolved?.Invoke(alertId);
            return true;
        }

        public void TickHour()
        {
            foreach (var alert in _state.Alerts.Where(a => !a.IsResolved).ToList())
            {
                alert.RemainingHours = Math.Max(0, alert.RemainingHours - 1);

                // Priority escalation if unacknowledged or time expired
                bool escalated = false;
                if (!alert.IsAcknowledged && (alert.Severity == "critical" || alert.Severity == "emergency"))
                {
                    if (alert.CurrentPriority < 10)
                    {
                        alert.CurrentPriority++;
                        escalated = true;
                    }
                }

                if (alert.RemainingHours == 0 && alert.CurrentPriority < 10)
                {
                    alert.CurrentPriority = 10;
                    escalated = true;
                }

                if (escalated)
                    OnAlertEscalated?.Invoke(alert);
            }
        }

        // ── Evacuation Protocols ───────────────────────────────────────────

        public EvacuationProtocolState ActivateProtocol(string protocolName, string assemblyZone, int assignedCount)
        {
            var protocol = new EvacuationProtocolState
            {
                ProtocolId = $"proto_{_state.NextSequence++}",
                Name = protocolName,
                Status = "active",
                AssemblyZone = assemblyZone,
                AssignedPersonnelCount = assignedCount
            };

            _state.ActiveProtocols.Add(protocol);
            OnProtocolActivated?.Invoke(protocol);
            return protocol;
        }

        public bool DeactivateProtocol(string protocolId)
        {
            var proto = _state.ActiveProtocols.FirstOrDefault(p =>
                string.Equals(p.ProtocolId, protocolId, StringComparison.OrdinalIgnoreCase));

            if (proto == null || proto.Status == "cancelled") return false;

            proto.Status = "cancelled";
            return true;
        }

        // ── Queries ────────────────────────────────────────────────────────

        public ActiveEmergencyAlert? GetHighestPriorityAlert()
        {
            return _state.Alerts
                .Where(a => !a.IsResolved)
                .OrderByDescending(a => a.CurrentPriority)
                .ThenBy(a => a.RemainingHours)
                .FirstOrDefault();
        }

        public IReadOnlyList<ActiveEmergencyAlert> GetActiveAlerts()
        {
            return _state.Alerts
                .Where(a => !a.IsResolved)
                .OrderByDescending(a => a.CurrentPriority)
                .ToList();
        }

        public IReadOnlyList<ActiveEmergencyAlert> GetAlertHistory() => _state.AlertHistory;

        // ── Save / Restore ─────────────────────────────────────────────────

        public EmergencyAlertState CaptureState()
        {
            return new EmergencyAlertState
            {
                SchemaVersion = _state.SchemaVersion,
                NextSequence = _state.NextSequence,
                Alerts = _state.Alerts.Select(a => new ActiveEmergencyAlert
                {
                    AlertId = a.AlertId,
                    TypeId = a.TypeId,
                    Severity = a.Severity,
                    DetectedDay = a.DetectedDay,
                    DetectedHour = a.DetectedHour,
                    RemainingHours = a.RemainingHours,
                    SourceSystem = a.SourceSystem,
                    AffectedZone = a.AffectedZone,
                    IsAcknowledged = a.IsAcknowledged,
                    IsResolved = a.IsResolved,
                    CurrentPriority = a.CurrentPriority
                }).ToList(),
                ActiveProtocols = _state.ActiveProtocols.Select(p => new EvacuationProtocolState
                {
                    ProtocolId = p.ProtocolId,
                    Name = p.Name,
                    Status = p.Status,
                    AssemblyZone = p.AssemblyZone,
                    AssignedPersonnelCount = p.AssignedPersonnelCount
                }).ToList(),
                AlertHistory = _state.AlertHistory.Select(h => new ActiveEmergencyAlert
                {
                    AlertId = h.AlertId,
                    TypeId = h.TypeId,
                    Severity = h.Severity,
                    DetectedDay = h.DetectedDay,
                    DetectedHour = h.DetectedHour,
                    RemainingHours = h.RemainingHours,
                    SourceSystem = h.SourceSystem,
                    AffectedZone = h.AffectedZone,
                    IsAcknowledged = h.IsAcknowledged,
                    IsResolved = h.IsResolved,
                    CurrentPriority = h.CurrentPriority
                }).ToList()
            };
        }

        public void RestoreState(EmergencyAlertState? saved)
        {
            if (saved == null) return;
            _state.SchemaVersion = saved.SchemaVersion;
            _state.NextSequence = saved.NextSequence > 0 ? saved.NextSequence : 1;

            _state.Alerts = saved.Alerts?.Select(a => new ActiveEmergencyAlert
            {
                AlertId = a.AlertId,
                TypeId = a.TypeId,
                Severity = a.Severity,
                DetectedDay = a.DetectedDay,
                DetectedHour = a.DetectedHour,
                RemainingHours = a.RemainingHours,
                SourceSystem = a.SourceSystem,
                AffectedZone = a.AffectedZone,
                IsAcknowledged = a.IsAcknowledged,
                IsResolved = a.IsResolved,
                CurrentPriority = a.CurrentPriority
            }).ToList() ?? new List<ActiveEmergencyAlert>();

            _state.ActiveProtocols = saved.ActiveProtocols?.Select(p => new EvacuationProtocolState
            {
                ProtocolId = p.ProtocolId,
                Name = p.Name,
                Status = p.Status,
                AssemblyZone = p.AssemblyZone,
                AssignedPersonnelCount = p.AssignedPersonnelCount
            }).ToList() ?? new List<EvacuationProtocolState>();

            _state.AlertHistory = saved.AlertHistory?.Select(h => new ActiveEmergencyAlert
            {
                AlertId = h.AlertId,
                TypeId = h.TypeId,
                Severity = h.Severity,
                DetectedDay = h.DetectedDay,
                DetectedHour = h.DetectedHour,
                RemainingHours = h.RemainingHours,
                SourceSystem = h.SourceSystem,
                AffectedZone = h.AffectedZone,
                IsAcknowledged = h.IsAcknowledged,
                IsResolved = h.IsResolved,
                CurrentPriority = h.CurrentPriority
            }).ToList() ?? new List<ActiveEmergencyAlert>();
        }
    }
}
