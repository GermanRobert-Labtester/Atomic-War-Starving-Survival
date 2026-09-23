// SPDX-License-Identifier: MIT
// ============================================================================
// Ashfall Core : Plan 158 — Disaster & Emergency Response System
// Subsystem    : DisasterResponseSystem / Crisis Management, Protocols & Resilience
// Authority    : Next-steps-plans/Plan_158_Disaster_Emergency_Response_System.md
//                UNBLOCK-PROGRAM-WAVE32-BATCH6-PLANS (DEC-154)
// ============================================================================
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Shelter
{
    public enum DisasterType
    {
        Earthquake           = 0,
        Flooding             = 1,
        ElectricalFire       = 2,
        RadiationBreach      = 3,
        AirToxinLeak         = 4,
        StructuralSubsidence = 5
    }

    public enum DisasterSeverity
    {
        Minor        = 0,
        Moderate     = 1,
        Severe       = 2,
        Catastrophic = 3
    }

    public enum DisasterStatus
    {
        Active    = 0,
        Contained = 1,
        Resolved  = 2
    }

    public enum EmergencyProtocolType
    {
        Lockdown          = 0,
        FireSuppression   = 1,
        PumpsOverdrive    = 2,
        HazmatPurge       = 3,
        StructuralShoring = 4
    }

    public sealed class DisasterEventDto
    {
        public string DisasterId { get; set; } = string.Empty;
        public DisasterType Type { get; set; } = DisasterType.Earthquake;
        public DisasterSeverity Severity { get; set; } = DisasterSeverity.Moderate;
        public string Name { get; set; } = string.Empty;
        public List<string> AffectedRoomIds { get; set; } = new();
        public int StartedDay { get; set; }
        public int DurationDays { get; set; } = 2;
        public double MitigationInvested { get; set; }
        public double MitigationRequired { get; set; } = 10.0;
        public double DamageAccumulated { get; set; }
        public DisasterStatus Status { get; set; } = DisasterStatus.Active;
        public int ResolvedDay { get; set; } = -1;

        public DisasterEventDto Clone()
        {
            return new DisasterEventDto
            {
                DisasterId = DisasterId,
                Type = Type,
                Severity = Severity,
                Name = Name,
                StartedDay = StartedDay,
                DurationDays = DurationDays,
                MitigationInvested = MitigationInvested,
                MitigationRequired = MitigationRequired,
                DamageAccumulated = DamageAccumulated,
                Status = Status,
                ResolvedDay = ResolvedDay,
                AffectedRoomIds = new List<string>(AffectedRoomIds)
            };
        }
    }

    public sealed class EmergencyProtocolDto
    {
        public string ProtocolId { get; set; } = string.Empty;
        public EmergencyProtocolType Type { get; set; } = EmergencyProtocolType.Lockdown;
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int EffectivenessPermille { get; set; } = 500;
        public Dictionary<string, int> ResourceCosts { get; set; } = new();

        public EmergencyProtocolDto Clone()
        {
            var p = new EmergencyProtocolDto
            {
                ProtocolId = ProtocolId,
                Type = Type,
                Name = Name,
                IsActive = IsActive,
                EffectivenessPermille = EffectivenessPermille
            };
            foreach (var kv in ResourceCosts)
                p.ResourceCosts[kv.Key] = kv.Value;
            return p;
        }
    }

    public sealed class DisasterResponseState
    {
        public int SchemaVersion { get; set; } = 1;
        public double ResilienceRating { get; set; } = 75.0; // 0..100
        public int TotalDisastersTriggered { get; set; }
        public int TotalDisastersResolved { get; set; }
        public List<DisasterEventDto> ActiveDisasters { get; set; } = new();
        public List<EmergencyProtocolDto> Protocols { get; set; } = new();
    }

    /// <summary>
    /// Pure domain authority managing acute shelter emergencies, disaster mitigation,
    /// protocol activation, and architectural resilience against crises.
    /// Zero engine dependencies; deterministic evaluation.
    /// </summary>
    public sealed class DisasterResponseSystem
    {
        private readonly Dictionary<string, DisasterEventDto> _disasters = new(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<EmergencyProtocolType, EmergencyProtocolDto> _protocols = new();

        public double ResilienceRating { get; private set; } = 75.0;
        public int TotalDisastersTriggered { get; private set; }
        public int TotalDisastersResolved { get; private set; }

        // Seams for presentation, alarms, audio, and visual crisis states
        public Action<DisasterEventDto>? OnDisasterTriggeredSeam { get; set; }
        public Action<DisasterEventDto>? OnDisasterContainedSeam { get; set; }
        public Action<DisasterEventDto>? OnDisasterResolvedSeam { get; set; }
        public Action<EmergencyProtocolDto>? OnProtocolActivatedSeam { get; set; }
        public Action<string, double>? OnDamageAssessedSeam { get; set; }

        public DisasterResponseSystem()
        {
            LoadEmbeddedDefaults();
        }

        public void LoadCatalog(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return;

            try
            {
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                if (root.TryGetProperty("emergency_protocols", out var protoElem) && protoElem.ValueKind == JsonValueKind.Array)
                {
                    _protocols.Clear();
                    foreach (var item in protoElem.EnumerateArray())
                    {
                        var protoId = item.GetProperty("protocol_id").GetString() ?? string.Empty;
                        var name = item.GetProperty("name").GetString() ?? string.Empty;
                        var typeStr = item.GetProperty("protocol_type").GetString() ?? string.Empty;
                        var eff = item.TryGetProperty("effectiveness_permille", out var eElem) ? eElem.GetInt32() : 500;

                        var type = EmergencyProtocolType.Lockdown;
                        if (typeStr.Equals("fire_suppression", StringComparison.OrdinalIgnoreCase))
                            type = EmergencyProtocolType.FireSuppression;
                        else if (typeStr.Equals("pumps_overdrive", StringComparison.OrdinalIgnoreCase))
                            type = EmergencyProtocolType.PumpsOverdrive;
                        else if (typeStr.Equals("hazmat_purge", StringComparison.OrdinalIgnoreCase))
                            type = EmergencyProtocolType.HazmatPurge;
                        else if (typeStr.Equals("structural_shoring", StringComparison.OrdinalIgnoreCase))
                            type = EmergencyProtocolType.StructuralShoring;

                        _protocols[type] = new EmergencyProtocolDto
                        {
                            ProtocolId = protoId,
                            Type = type,
                            Name = name,
                            IsActive = false,
                            EffectivenessPermille = eff
                        };
                    }
                }
            }
            catch
            {
                LoadEmbeddedDefaults();
            }
        }

        private void LoadEmbeddedDefaults()
        {
            _protocols.Clear();
            _protocols[EmergencyProtocolType.Lockdown] = new EmergencyProtocolDto
            {
                ProtocolId = "proto_lockdown",
                Type = EmergencyProtocolType.Lockdown,
                Name = "Hermetic Sector Lockdown",
                IsActive = false,
                EffectivenessPermille = 450
            };
            _protocols[EmergencyProtocolType.FireSuppression] = new EmergencyProtocolDto
            {
                ProtocolId = "proto_fire_suppression",
                Type = EmergencyProtocolType.FireSuppression,
                Name = "Halon Gas & Foam Suppression",
                IsActive = false,
                EffectivenessPermille = 750
            };
            _protocols[EmergencyProtocolType.PumpsOverdrive] = new EmergencyProtocolDto
            {
                ProtocolId = "proto_pumps_overdrive",
                Type = EmergencyProtocolType.PumpsOverdrive,
                Name = "Auxiliary Sump Pumps Overdrive",
                IsActive = false,
                EffectivenessPermille = 700
            };
            _protocols[EmergencyProtocolType.HazmatPurge] = new EmergencyProtocolDto
            {
                ProtocolId = "proto_hazmat_purge",
                Type = EmergencyProtocolType.HazmatPurge,
                Name = "Hazmat Scrubber Air Purge",
                IsActive = false,
                EffectivenessPermille = 650
            };
            _protocols[EmergencyProtocolType.StructuralShoring] = new EmergencyProtocolDto
            {
                ProtocolId = "proto_structural_shoring",
                Type = EmergencyProtocolType.StructuralShoring,
                Name = "Rapid Hydraulic Shoring",
                IsActive = false,
                EffectivenessPermille = 600
            };
        }

        public DisasterEventDto TriggerDisaster(
            DisasterType type,
            DisasterSeverity severity,
            List<string> affectedRooms,
            int currentDay)
        {
            string id = $"disaster_{currentDay}_{TotalDisastersTriggered + 1}";
            double baseReq = 10.0;
            switch (severity)
            {
                case DisasterSeverity.Minor:        baseReq = 6.0;  break;
                case DisasterSeverity.Moderate:     baseReq = 12.0; break;
                case DisasterSeverity.Severe:       baseReq = 20.0; break;
                case DisasterSeverity.Catastrophic: baseReq = 35.0; break;
            }

            var disaster = new DisasterEventDto
            {
                DisasterId = id,
                Type = type,
                Severity = severity,
                Name = $"{severity} {type}",
                StartedDay = currentDay,
                DurationDays = (int)severity + 2,
                MitigationInvested = 0.0,
                MitigationRequired = baseReq,
                DamageAccumulated = 0.0,
                Status = DisasterStatus.Active,
                AffectedRoomIds = affectedRooms != null ? new List<string>(affectedRooms) : new List<string>()
            };

            _disasters[id] = disaster;
            TotalDisastersTriggered++;
            OnDisasterTriggeredSeam?.Invoke(disaster);
            return disaster.Clone();
        }

        public bool ActivateProtocol(EmergencyProtocolType type)
        {
            if (!_protocols.TryGetValue(type, out var proto))
                return false;

            if (proto.IsActive)
                return true;

            proto.IsActive = true;
            OnProtocolActivatedSeam?.Invoke(proto);
            return true;
        }

        public bool DeactivateProtocol(EmergencyProtocolType type)
        {
            if (!_protocols.TryGetValue(type, out var proto))
                return false;

            proto.IsActive = false;
            return true;
        }

        public bool IsProtocolActive(EmergencyProtocolType type)
        {
            return _protocols.TryGetValue(type, out var p) && p.IsActive;
        }

        public bool TickDisaster(
            string disasterId,
            double baseMitigationLabor,
            int currentDay,
            ISeededRng rng)
        {
            if (!_disasters.TryGetValue(disasterId, out var disaster))
                return false;

            if (disaster.Status == DisasterStatus.Resolved)
                return true;

            double effectiveLabor = Math.Max(0.0, baseMitigationLabor);

            // Protocol synergy boosts mitigation
            switch (disaster.Type)
            {
                case DisasterType.ElectricalFire:
                    if (IsProtocolActive(EmergencyProtocolType.FireSuppression))
                        effectiveLabor *= 1.75;
                    break;
                case DisasterType.Flooding:
                    if (IsProtocolActive(EmergencyProtocolType.PumpsOverdrive))
                        effectiveLabor *= 1.70;
                    break;
                case DisasterType.RadiationBreach:
                case DisasterType.AirToxinLeak:
                    if (IsProtocolActive(EmergencyProtocolType.HazmatPurge))
                        effectiveLabor *= 1.65;
                    break;
                case DisasterType.Earthquake:
                case DisasterType.StructuralSubsidence:
                    if (IsProtocolActive(EmergencyProtocolType.StructuralShoring))
                        effectiveLabor *= 1.60;
                    break;
            }

            if (IsProtocolActive(EmergencyProtocolType.Lockdown))
            {
                effectiveLabor *= 1.20; // Lockdown seals contain spread
            }

            // Resilience factor
            double resilienceFactor = Math.Clamp(ResilienceRating / 100.0, 0.5, 1.5);
            effectiveLabor *= resilienceFactor;

            disaster.MitigationInvested += effectiveLabor;

            if (disaster.MitigationInvested >= disaster.MitigationRequired)
            {
                disaster.Status = DisasterStatus.Resolved;
                disaster.ResolvedDay = currentDay;
                TotalDisastersResolved++;
                OnDisasterResolvedSeam?.Invoke(disaster);
                return true;
            }
            else if (disaster.MitigationInvested >= disaster.MitigationRequired * 0.5 && disaster.Status == DisasterStatus.Active)
            {
                disaster.Status = DisasterStatus.Contained;
                OnDisasterContainedSeam?.Invoke(disaster);
            }

            return false;
        }

        public double CalculateRoomDamage(DisasterEventDto disaster, string roomId)
        {
            if (disaster == null)
                return 0.0;

            double baseDmg = 15.0 * ((int)disaster.Severity + 1);

            // Active protocols mitigate room damage
            if (disaster.Type == DisasterType.ElectricalFire && IsProtocolActive(EmergencyProtocolType.FireSuppression))
            {
                baseDmg *= 0.30; // 70% damage reduction
            }
            else if (disaster.Type == DisasterType.Flooding && IsProtocolActive(EmergencyProtocolType.PumpsOverdrive))
            {
                baseDmg *= 0.35;
            }
            else if (disaster.Type == DisasterType.Earthquake && IsProtocolActive(EmergencyProtocolType.StructuralShoring))
            {
                baseDmg *= 0.40;
            }

            if (IsProtocolActive(EmergencyProtocolType.Lockdown))
            {
                baseDmg *= 0.85; // Additional containment reduction
            }

            OnDamageAssessedSeam?.Invoke(roomId, baseDmg);
            return baseDmg;
        }

        public void AdjustResilience(double delta)
        {
            ResilienceRating = Math.Clamp(ResilienceRating + delta, 0.0, 100.0);
        }

        public DisasterEventDto? GetDisaster(string disasterId)
        {
            if (_disasters.TryGetValue(disasterId, out var d))
                return d.Clone();
            return null;
        }

        public IReadOnlyList<DisasterEventDto> GetAllDisasters()
        {
            var list = new List<DisasterEventDto>(_disasters.Count);
            foreach (var d in _disasters.Values)
                list.Add(d.Clone());
            return list;
        }

        public DisasterResponseState CaptureState()
        {
            var state = new DisasterResponseState
            {
                SchemaVersion = 1,
                ResilienceRating = ResilienceRating,
                TotalDisastersTriggered = TotalDisastersTriggered,
                TotalDisastersResolved = TotalDisastersResolved
            };
            foreach (var d in _disasters.Values)
                state.ActiveDisasters.Add(d.Clone());
            foreach (var p in _protocols.Values)
                state.Protocols.Add(p.Clone());
            return state;
        }

        public void RestoreState(DisasterResponseState state)
        {
            if (state == null)
                return;

            ResilienceRating = Math.Clamp(state.ResilienceRating, 0.0, 100.0);
            TotalDisastersTriggered = Math.Max(0, state.TotalDisastersTriggered);
            TotalDisastersResolved = Math.Max(0, state.TotalDisastersResolved);

            _disasters.Clear();
            if (state.ActiveDisasters != null)
            {
                foreach (var d in state.ActiveDisasters)
                    _disasters[d.DisasterId] = d.Clone();
            }

            if (state.Protocols != null)
            {
                foreach (var p in state.Protocols)
                    _protocols[p.Type] = p.Clone();
            }
        }
    }
}
