// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Visitors
{
    public enum VisitorType
    {
        Refugee = 0,
        Trader = 1,
        Defector = 2,
        Guest = 3,
        Envoy = 4,
        Deserter = 5,
        Exile = 6
    }

    public enum VisitorStatus
    {
        Processing = 0,
        Integrated = 1,
        TemporaryResident = 2,
        Departing = 3,
        Departed = 4,
        Recruited = 5
    }

    public enum HousingType
    {
        TemporaryBunk = 0,
        SharedQuarter = 1,
        PrivateRoom = 2,
        GuestSuite = 3
    }

    public enum DepartureType
    {
        Voluntary = 0,
        Invited = 1,
        Forced = 2,
        Deported = 3,
        Escaped = 4,
        Recruited = 5
    }

    public enum MonitoringLevel
    {
        None = 0,
        Low = 1,
        Medium = 2,
        High = 3
    }

    [Serializable]
    public sealed class VisitorTemplateDefinition
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("type")]
        public string Type { get; set; } = "refugee";

        [JsonPropertyName("base_integration_days")]
        public int BaseIntegrationDays { get; set; } = 14;

        [JsonPropertyName("default_housing")]
        public string DefaultHousing { get; set; } = "temporary_bunk";

        [JsonPropertyName("daily_food_consumption")]
        public float DailyFoodConsumption { get; set; } = 1.0f;

        [JsonPropertyName("daily_water_consumption")]
        public float DailyWaterConsumption { get; set; } = 1.5f;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        public VisitorType ParseType() => Type?.ToLowerInvariant() switch
        {
            "trader" => VisitorType.Trader,
            "defector" => VisitorType.Defector,
            "guest" => VisitorType.Guest,
            "envoy" => VisitorType.Envoy,
            "deserter" => VisitorType.Deserter,
            "exile" => VisitorType.Exile,
            _ => VisitorType.Refugee
        };

        public HousingType ParseHousing() => DefaultHousing?.ToLowerInvariant() switch
        {
            "shared_quarter" => HousingType.SharedQuarter,
            "private_room" => HousingType.PrivateRoom,
            "guest_suite" => HousingType.GuestSuite,
            _ => HousingType.TemporaryBunk
        };
    }

    [Serializable]
    public sealed class VisitorCatalogData
    {
        [JsonPropertyName("schema_version")]
        public int SchemaVersion { get; set; } = 1;

        [JsonPropertyName("templates")]
        public List<VisitorTemplateDefinition> Templates { get; set; } = new List<VisitorTemplateDefinition>();
    }

    [Serializable]
    public sealed class VisitorRecord
    {
        public string VisitorId { get; set; } = string.Empty;

        /// <summary>
        /// Stable external/source identity this stay was opened from (for
        /// example an AirlockSecuritySystem admission visitor id). Empty when
        /// the stay has no external provenance. Used to make admission handoff
        /// idempotent without duplicating the airlock's own incident ledger.
        /// </summary>
        public string SourceVisitorId { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;
        public VisitorType Type { get; set; } = VisitorType.Refugee;
        public int ArrivalDay { get; set; } = 1;
        public VisitorStatus Status { get; set; } = VisitorStatus.Processing;
        public string AdmittedBy { get; set; } = string.Empty;
        public string AssignedRoomId { get; set; } = string.Empty;
        public HousingType Housing { get; set; } = HousingType.TemporaryBunk;
        public float IntegrationProgress { get; set; } = 0f;
        public int DepartureDay { get; set; } = -1;
        public MonitoringLevel Monitoring { get; set; } = MonitoringLevel.None;
        public float DailyFoodRate { get; set; } = 1.0f;
        public float DailyWaterRate { get; set; } = 1.5f;
        public string Notes { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class IntegrationTask
    {
        public string TaskId { get; set; } = string.Empty;
        public string VisitorId { get; set; } = string.Empty;
        public string TaskType { get; set; } = "orientation";
        public string AssignedTo { get; set; } = string.Empty;
        public int AssignedDay { get; set; } = 1;
        public int DueDay { get; set; } = 4;
        public bool IsCompleted { get; set; } = false;
        public int CompletedDay { get; set; } = -1;
    }

    [Serializable]
    public sealed class VisitorDepartureRecord
    {
        public string DepartureId { get; set; } = string.Empty;
        public string VisitorId { get; set; } = string.Empty;
        public string VisitorName { get; set; } = string.Empty;
        public DepartureType DepartureType { get; set; } = DepartureType.Voluntary;
        public int DepartureDay { get; set; } = 1;
        public string Reason { get; set; } = string.Empty;
        public string FinalStanding { get; set; } = "good_standing";
    }

    [Serializable]
    public sealed class VisitorIntegrationSaveState
    {
        public int SchemaVersion { get; set; } = 1;
        public int NextSequence { get; set; } = 1;
        public List<VisitorRecord> Visitors { get; set; } = new List<VisitorRecord>();
        public List<IntegrationTask> Tasks { get; set; } = new List<IntegrationTask>();
        public List<VisitorDepartureRecord> Departures { get; set; } = new List<VisitorDepartureRecord>();
    }

    /// <summary>
    /// Plan 214 — Visitor Integration & Temporary Housing System.
    /// Manages admitted visitors (refugees, traders, defectors, guests, envoys),
    /// temporary housing assignments, integration tasks, and departure/recruitment lifecycles.
    /// </summary>
    public sealed class VisitorIntegrationSystem
    {
        private readonly VisitorIntegrationSaveState _state;
        private readonly Dictionary<string, VisitorTemplateDefinition> _templates =
            new Dictionary<string, VisitorTemplateDefinition>(StringComparer.OrdinalIgnoreCase);

        public event Action<VisitorRecord>? OnVisitorAdmitted;
        public event Action<VisitorRecord, string, HousingType>? OnHousingAssigned;
        public event Action<IntegrationTask>? OnIntegrationTaskCompleted;
        public event Action<VisitorRecord>? OnVisitorIntegrated;
        public event Action<VisitorRecord>? OnVisitorRecruited;
        public event Action<VisitorDepartureRecord>? OnVisitorDeparted;
        public event Action? OnStateChanged;

        public IReadOnlyList<VisitorRecord> Visitors => _state.Visitors;
        public IReadOnlyList<IntegrationTask> Tasks => _state.Tasks;
        public IReadOnlyList<VisitorDepartureRecord> Departures => _state.Departures;
        public IReadOnlyCollection<VisitorTemplateDefinition> Templates => _templates.Values;

        public VisitorIntegrationSystem(VisitorIntegrationSaveState? state = null)
        {
            _state = state ?? new VisitorIntegrationSaveState();
        }

        public void LoadCatalog(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return;
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var data = JsonSerializer.Deserialize<VisitorCatalogData>(json, options);
            if (data != null)
            {
                LoadCatalog(data);
            }
        }

        public void LoadCatalog(VisitorCatalogData catalog)
        {
            if (catalog?.Templates == null) return;
            foreach (var t in catalog.Templates)
            {
                if (!string.IsNullOrWhiteSpace(t.Id))
                {
                    _templates[t.Id] = t;
                }
            }
        }

        public VisitorTemplateDefinition? GetTemplate(string templateId)
        {
            if (string.IsNullOrWhiteSpace(templateId)) return null;
            return _templates.TryGetValue(templateId, out var t) ? t : null;
        }

        public VisitorRecord AdmitVisitor(
            string name,
            VisitorType type,
            string admittedBy,
            int currentDay = 1,
            string? notes = null,
            float dailyFood = 1.0f,
            float dailyWater = 1.5f,
            int plannedDurationDays = -1,
            string sourceVisitorId = "")
        {
            var visitor = new VisitorRecord
            {
                VisitorId = $"vis_{_state.NextSequence++}",
                SourceVisitorId = sourceVisitorId ?? string.Empty,
                Name = string.IsNullOrWhiteSpace(name) ? "Unknown Visitor" : name.Trim(),
                Type = type,
                ArrivalDay = currentDay,
                Status = VisitorStatus.Processing,
                AdmittedBy = admittedBy ?? string.Empty,
                AssignedRoomId = string.Empty,
                Housing = HousingType.TemporaryBunk,
                IntegrationProgress = 0f,
                DepartureDay = plannedDurationDays > 0 ? currentDay + plannedDurationDays : -1,
                Monitoring = MonitoringLevel.None,
                DailyFoodRate = dailyFood,
                DailyWaterRate = dailyWater,
                Notes = notes ?? string.Empty
            };

            _state.Visitors.Add(visitor);
            OnVisitorAdmitted?.Invoke(visitor);
            OnStateChanged?.Invoke();
            return visitor;
        }

        public VisitorRecord? AdmitFromTemplate(
            string templateId,
            string name,
            string admittedBy,
            int currentDay = 1,
            string sourceVisitorId = "")
        {
            if (string.IsNullOrWhiteSpace(templateId)) return null;
            if (!_templates.TryGetValue(templateId, out var tpl)) return null;

            int duration = tpl.ParseType() switch
            {
                VisitorType.Trader => 3,
                VisitorType.Envoy => 5,
                VisitorType.Guest => 7,
                _ => -1
            };

            var visitor = AdmitVisitor(
                name: string.IsNullOrWhiteSpace(name) ? tpl.Name : name,
                type: tpl.ParseType(),
                admittedBy: admittedBy,
                currentDay: currentDay,
                notes: tpl.Description,
                dailyFood: tpl.DailyFoodConsumption,
                dailyWater: tpl.DailyWaterConsumption,
                plannedDurationDays: duration,
                sourceVisitorId: sourceVisitorId);

            visitor.Housing = tpl.ParseHousing();
            return visitor;
        }

        public bool AssignHousing(string visitorId, string roomId, HousingType housing, int currentDay = 1)
        {
            var visitor = _state.Visitors.FirstOrDefault(v => v.VisitorId == visitorId);
            if (visitor == null || visitor.Status == VisitorStatus.Departed)
                return false;

            visitor.AssignedRoomId = roomId ?? string.Empty;
            visitor.Housing = housing;

            OnHousingAssigned?.Invoke(visitor, roomId ?? string.Empty, housing);
            OnStateChanged?.Invoke();
            return true;
        }

        public IntegrationTask? AssignIntegrationTask(
            string visitorId,
            string taskType,
            string assignedTo,
            int currentDay = 1,
            int durationDays = 3)
        {
            var visitor = _state.Visitors.FirstOrDefault(v => v.VisitorId == visitorId);
            if (visitor == null || visitor.Status == VisitorStatus.Departed)
                return null;

            var task = new IntegrationTask
            {
                TaskId = $"vtsk_{_state.NextSequence++}",
                VisitorId = visitorId,
                TaskType = taskType ?? "orientation",
                AssignedTo = assignedTo ?? string.Empty,
                AssignedDay = currentDay,
                DueDay = currentDay + Math.Max(1, durationDays),
                IsCompleted = false,
                CompletedDay = -1
            };

            _state.Tasks.Add(task);
            OnStateChanged?.Invoke();
            return task;
        }

        public bool CompleteIntegrationTask(string taskId, int currentDay = 1)
        {
            var task = _state.Tasks.FirstOrDefault(t => t.TaskId == taskId);
            if (task == null || task.IsCompleted)
                return false;

            task.IsCompleted = true;
            task.CompletedDay = currentDay;

            var visitor = _state.Visitors.FirstOrDefault(v => v.VisitorId == task.VisitorId);
            if (visitor != null && visitor.Status != VisitorStatus.Departed)
            {
                // Completing tasks boosts integration progress
                visitor.IntegrationProgress = Math.Clamp(visitor.IntegrationProgress + 25f, 0f, 100f);
                if (visitor.IntegrationProgress >= 100f && visitor.Status == VisitorStatus.Processing)
                {
                    visitor.Status = VisitorStatus.Integrated;
                    OnVisitorIntegrated?.Invoke(visitor);
                }
            }

            OnIntegrationTaskCompleted?.Invoke(task);
            OnStateChanged?.Invoke();
            return true;
        }

        public bool SetMonitoringLevel(string visitorId, MonitoringLevel level)
        {
            var visitor = _state.Visitors.FirstOrDefault(v => v.VisitorId == visitorId);
            if (visitor == null) return false;

            visitor.Monitoring = level;
            OnStateChanged?.Invoke();
            return true;
        }

        public bool RecruitVisitor(string visitorId, int currentDay = 1)
        {
            var visitor = _state.Visitors.FirstOrDefault(v => v.VisitorId == visitorId);
            if (visitor == null || visitor.Status == VisitorStatus.Departed || visitor.Status == VisitorStatus.Recruited)
                return false;

            visitor.Status = VisitorStatus.Recruited;
            visitor.DepartureDay = -1;

            var departure = new VisitorDepartureRecord
            {
                DepartureId = $"vdep_{_state.NextSequence++}",
                VisitorId = visitor.VisitorId,
                VisitorName = visitor.Name,
                DepartureType = DepartureType.Recruited,
                DepartureDay = currentDay,
                Reason = "Fully inducted into shelter citizenship",
                FinalStanding = "good_standing"
            };
            _state.Departures.Add(departure);

            OnVisitorRecruited?.Invoke(visitor);
            OnStateChanged?.Invoke();
            return true;
        }

        public VisitorDepartureRecord? DepartVisitor(
            string visitorId,
            DepartureType type,
            string reason,
            int currentDay = 1,
            string finalStanding = "good_standing")
        {
            var visitor = _state.Visitors.FirstOrDefault(v => v.VisitorId == visitorId);
            if (visitor == null || visitor.Status == VisitorStatus.Departed)
                return null;

            visitor.Status = VisitorStatus.Departed;
            visitor.DepartureDay = currentDay;

            var departure = new VisitorDepartureRecord
            {
                DepartureId = $"vdep_{_state.NextSequence++}",
                VisitorId = visitor.VisitorId,
                VisitorName = visitor.Name,
                DepartureType = type,
                DepartureDay = currentDay,
                Reason = reason ?? "Scheduled departure",
                FinalStanding = finalStanding ?? "good_standing"
            };
            _state.Departures.Add(departure);

            OnVisitorDeparted?.Invoke(departure);
            OnStateChanged?.Invoke();
            return departure;
        }

        public void TickDay(int currentDay)
        {
            bool changed = false;

            foreach (var visitor in _state.Visitors)
            {
                if (visitor.Status == VisitorStatus.Departed || visitor.Status == VisitorStatus.Recruited)
                    continue;

                // Natural integration progression: +5% per day
                if (visitor.Status == VisitorStatus.Processing)
                {
                    visitor.IntegrationProgress = Math.Clamp(visitor.IntegrationProgress + 5.0f, 0f, 100f);
                    if (visitor.IntegrationProgress >= 100f)
                    {
                        visitor.Status = VisitorStatus.Integrated;
                        OnVisitorIntegrated?.Invoke(visitor);
                        changed = true;
                    }
                }

                // Check planned departure
                if (visitor.DepartureDay > 0 && currentDay >= visitor.DepartureDay)
                {
                    DepartVisitor(visitor.VisitorId, DepartureType.Voluntary, "Planned stay concluded", currentDay);
                    changed = true;
                }
            }

            if (changed)
            {
                OnStateChanged?.Invoke();
            }
        }

        public IReadOnlyList<VisitorRecord> GetActiveVisitors()
        {
            return _state.Visitors
                .Where(v => v.Status != VisitorStatus.Departed && v.Status != VisitorStatus.Recruited)
                .ToList();
        }

        public VisitorRecord? GetVisitor(string visitorId)
        {
            if (string.IsNullOrWhiteSpace(visitorId)) return null;
            return _state.Visitors.FirstOrDefault(v => v.VisitorId == visitorId);
        }

        public VisitorRecord? GetVisitorBySource(string sourceVisitorId)
        {
            if (string.IsNullOrWhiteSpace(sourceVisitorId)) return null;
            return _state.Visitors.FirstOrDefault(v =>
                string.Equals(v.SourceVisitorId, sourceVisitorId, StringComparison.Ordinal));
        }

        public VisitorIntegrationSaveState CaptureState()
        {
            var capture = new VisitorIntegrationSaveState
            {
                SchemaVersion = _state.SchemaVersion,
                NextSequence = _state.NextSequence,
                Visitors = new List<VisitorRecord>(_state.Visitors.Count),
                Tasks = new List<IntegrationTask>(_state.Tasks.Count),
                Departures = new List<VisitorDepartureRecord>(_state.Departures.Count)
            };

            foreach (var v in _state.Visitors)
            {
                capture.Visitors.Add(new VisitorRecord
                {
                    VisitorId = v.VisitorId,
                    SourceVisitorId = v.SourceVisitorId,
                    Name = v.Name,
                    Type = v.Type,
                    ArrivalDay = v.ArrivalDay,
                    Status = v.Status,
                    AdmittedBy = v.AdmittedBy,
                    AssignedRoomId = v.AssignedRoomId,
                    Housing = v.Housing,
                    IntegrationProgress = v.IntegrationProgress,
                    DepartureDay = v.DepartureDay,
                    Monitoring = v.Monitoring,
                    DailyFoodRate = v.DailyFoodRate,
                    DailyWaterRate = v.DailyWaterRate,
                    Notes = v.Notes
                });
            }

            foreach (var t in _state.Tasks)
            {
                capture.Tasks.Add(new IntegrationTask
                {
                    TaskId = t.TaskId,
                    VisitorId = t.VisitorId,
                    TaskType = t.TaskType,
                    AssignedTo = t.AssignedTo,
                    AssignedDay = t.AssignedDay,
                    DueDay = t.DueDay,
                    IsCompleted = t.IsCompleted,
                    CompletedDay = t.CompletedDay
                });
            }

            foreach (var d in _state.Departures)
            {
                capture.Departures.Add(new VisitorDepartureRecord
                {
                    DepartureId = d.DepartureId,
                    VisitorId = d.VisitorId,
                    VisitorName = d.VisitorName,
                    DepartureType = d.DepartureType,
                    DepartureDay = d.DepartureDay,
                    Reason = d.Reason,
                    FinalStanding = d.FinalStanding
                });
            }

            return capture;
        }

        public void RestoreState(VisitorIntegrationSaveState state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));

            _state.SchemaVersion = state.SchemaVersion;
            _state.NextSequence = state.NextSequence;
            _state.Visitors.Clear();
            _state.Tasks.Clear();
            _state.Departures.Clear();

            if (state.Visitors != null)
            {
                foreach (var v in state.Visitors)
                {
                    _state.Visitors.Add(new VisitorRecord
                    {
                        VisitorId = v.VisitorId,
                        SourceVisitorId = v.SourceVisitorId,
                        Name = v.Name,
                        Type = v.Type,
                        ArrivalDay = v.ArrivalDay,
                        Status = v.Status,
                        AdmittedBy = v.AdmittedBy,
                        AssignedRoomId = v.AssignedRoomId,
                        Housing = v.Housing,
                        IntegrationProgress = v.IntegrationProgress,
                        DepartureDay = v.DepartureDay,
                        Monitoring = v.Monitoring,
                        DailyFoodRate = v.DailyFoodRate,
                        DailyWaterRate = v.DailyWaterRate,
                        Notes = v.Notes
                    });
                }
            }

            if (state.Tasks != null)
            {
                foreach (var t in state.Tasks)
                {
                    _state.Tasks.Add(new IntegrationTask
                    {
                        TaskId = t.TaskId,
                        VisitorId = t.VisitorId,
                        TaskType = t.TaskType,
                        AssignedTo = t.AssignedTo,
                        AssignedDay = t.AssignedDay,
                        DueDay = t.DueDay,
                        IsCompleted = t.IsCompleted,
                        CompletedDay = t.CompletedDay
                    });
                }
            }

            if (state.Departures != null)
            {
                foreach (var d in state.Departures)
                {
                    _state.Departures.Add(new VisitorDepartureRecord
                    {
                        DepartureId = d.DepartureId,
                        VisitorId = d.VisitorId,
                        VisitorName = d.VisitorName,
                        DepartureType = d.DepartureType,
                        DepartureDay = d.DepartureDay,
                        Reason = d.Reason,
                        FinalStanding = d.FinalStanding
                    });
                }
            }

            OnStateChanged?.Invoke();
        }
    }
}
