// SPDX-License-Identifier: MIT
// ============================================================================
// Host Session : VisitorIntegrationHostSession
// Core System  : Ashfall.Core.Visitors.VisitorIntegrationSystem
// Host Caller  : Main.VisitorIntegration
// Purpose      : Plan 214 — temporary occupant orchestration for admitted
//                visitors: stable stay identity, temporary housing, processing
//                requirements, monitoring references, departure, and the
//                recruitment handoff. Permanent survivors, room topology,
//                inventory, rations, and security stay with their owners.
// ============================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Visitors;

namespace AtomicWar.GodotApp
{
    public sealed class VisitorIntegrationHostSession
    {
        private static readonly string[] DefaultRequirementTasks =
        {
            "orientation",
            "medical_check",
            "security_clearance"
        };

        private readonly VisitorIntegrationSystem _system;

        public VisitorIntegrationSystem System => _system;

        public event Action? StateChanged;

        /// <summary>
        /// Hands a fully-integrated visitor to the permanent-survivor owner.
        /// Returns true only when the canonical roster accepted the resident.
        /// The stay is converted to <see cref="VisitorStatus.Recruited"/> only
        /// after this succeeds, so a refused handoff leaves the stay active.
        /// </summary>
        public Func<VisitorRecord, bool>? RecruitToSurvivor { get; set; }

        /// <summary>
        /// Consumes one day of visitor rations through the canonical inventory
        /// owner. Returns false when the shelter cannot cover the total; the
        /// stay is never auto-ejected for scarcity.
        /// </summary>
        public Func<float, float, bool>? ConsumeDailyRations { get; set; }

        /// <summary>Optional journal/notice sink for committed stay facts.</summary>
        public Action<string>? JournalEvent { get; set; }

        public VisitorIntegrationHostSession(VisitorIntegrationSystem system)
        {
            _system = system ?? throw new ArgumentNullException(nameof(system));

            _system.OnVisitorAdmitted += _ => StateChanged?.Invoke();
            _system.OnHousingAssigned += (_, _, _) => StateChanged?.Invoke();
            _system.OnIntegrationTaskCompleted += _ => StateChanged?.Invoke();
            _system.OnVisitorIntegrated += _ => StateChanged?.Invoke();
            _system.OnVisitorRecruited += _ => StateChanged?.Invoke();
            _system.OnVisitorDeparted += _ => StateChanged?.Invoke();
            _system.OnStateChanged += () => StateChanged?.Invoke();
        }

        public IReadOnlyList<VisitorRecord> Visitors => _system.Visitors;
        public IReadOnlyList<IntegrationTask> Tasks => _system.Tasks;
        public IReadOnlyList<VisitorDepartureRecord> Departures => _system.Departures;
        public IReadOnlyCollection<VisitorTemplateDefinition> Templates => _system.Templates;

        public void LoadCatalog(string json)
        {
            _system.LoadCatalog(json);
        }

        public VisitorRecord? AdmitFromTemplate(
            string templateId,
            string name,
            string admittedBy,
            int currentDay,
            string sourceVisitorId = "")
        {
            var visitor = _system.AdmitFromTemplate(templateId, name, admittedBy, currentDay, sourceVisitorId);
            if (visitor != null)
            {
                EnsureStandardRequirements(visitor, currentDay);
                AssignDefaultHousing(visitor, templateId, currentDay);
            }
            return visitor;
        }

        public VisitorRecord? Admit(
            string name,
            VisitorType type,
            string admittedBy,
            int currentDay,
            string notes = "",
            int plannedDurationDays = -1,
            string sourceVisitorId = "")
        {
            var visitor = _system.AdmitVisitor(
                name: name,
                type: type,
                admittedBy: admittedBy,
                currentDay: currentDay,
                notes: notes,
                plannedDurationDays: plannedDurationDays,
                sourceVisitorId: sourceVisitorId);
            EnsureStandardRequirements(visitor, currentDay);
            return visitor;
        }

        /// <summary>
        /// Idempotent AirlockSecurity admission handoff. A committed admission
        /// opens exactly one stay for the source visitor id; replayed incidents
        /// or reloads do not create a second stay.
        /// </summary>
        public VisitorRecord? HandleAirlockAdmission(
            string sourceVisitorId,
            string visitorType,
            string admittedBy,
            int currentDay)
        {
            if (string.IsNullOrWhiteSpace(sourceVisitorId)) return null;
            if (string.Equals(visitorType, "decon_subject", StringComparison.OrdinalIgnoreCase)) return null;

            var existing = _system.GetVisitorBySource(sourceVisitorId);
            if (existing != null) return existing;

            var type = ParseVisitorType(visitorType);
            int duration = DurationFor(type);
            var visitor = _system.AdmitVisitor(
                name: BuildVisitorName(sourceVisitorId, type),
                type: type,
                admittedBy: admittedBy ?? "airlock_operator",
                currentDay: currentDay,
                notes: $"Admitted through the airlock ({visitorType}).",
                plannedDurationDays: duration,
                sourceVisitorId: sourceVisitorId);

            EnsureStandardRequirements(visitor, currentDay);
            JournalEvent?.Invoke($"A {type} was admitted to temporary shelter: {visitor.Name}.");
            return visitor;
        }

        public bool AssignHousing(string visitorId, string roomId, HousingType housing, int currentDay)
        {
            return _system.AssignHousing(visitorId, roomId, housing, currentDay);
        }

        public IntegrationTask? AssignTask(string visitorId, string taskType, string assignedTo, int currentDay, int durationDays = 2)
        {
            return _system.AssignIntegrationTask(visitorId, taskType, assignedTo, currentDay, durationDays);
        }

        public bool CompleteTask(string taskId, int currentDay)
        {
            bool ok = _system.CompleteIntegrationTask(taskId, currentDay);
            if (ok) JournalEvent?.Invoke("A visitor processing requirement was satisfied.");
            return ok;
        }

        public bool SetMonitoring(string visitorId, MonitoringLevel level)
        {
            return _system.SetMonitoringLevel(visitorId, level);
        }

        /// <summary>
        /// Converts an integrated visitor into a permanent resident through the
        /// roster owner. The temporary stay closes only after the roster accepts
        /// the resident; a refused handoff leaves the visitor active.
        /// </summary>
        public bool Recruit(string visitorId, int currentDay)
        {
            var visitor = _system.GetVisitor(visitorId);
            if (visitor == null || visitor.Status == VisitorStatus.Departed || visitor.Status == VisitorStatus.Recruited)
                return false;

            if (RecruitToSurvivor != null && !RecruitToSurvivor(visitor))
                return false;

            bool ok = _system.RecruitVisitor(visitorId, currentDay);
            if (ok) JournalEvent?.Invoke($"{visitor.Name} became a permanent resident.");
            return ok;
        }

        public VisitorDepartureRecord? Depart(string visitorId, DepartureType type, string reason, int currentDay, string finalStanding = "good_standing")
        {
            var record = _system.DepartVisitor(visitorId, type, reason, currentDay, finalStanding);
            if (record != null) JournalEvent?.Invoke($"{record.VisitorName} departed the shelter ({type}).");
            return record;
        }

        public IReadOnlyList<VisitorRecord> GetActiveVisitors() => _system.GetActiveVisitors();

        public IReadOnlyList<IntegrationTask> GetPendingTasks(string visitorId)
        {
            return _system.Tasks
                .Where(t => string.Equals(t.VisitorId, visitorId, StringComparison.Ordinal) && !t.IsCompleted)
                .ToList();
        }

        public bool IsIntegrated(VisitorRecord visitor)
        {
            return visitor != null && visitor.Status == VisitorStatus.Integrated;
        }

        /// <summary>
        /// Advances one campaign day. Routine processing is deterministic and
        /// uses no RNG. Active visitors draw their authored daily ration through
        /// the canonical inventory owner; a shortage is reported but never
        /// silently ejects a visitor.
        /// </summary>
        public void TickDay(int currentDay)
        {
            var before = _system.GetActiveVisitors();
            float food = 0f;
            float water = 0f;
            foreach (var visitor in before)
            {
                food += Math.Max(0f, visitor.DailyFoodRate);
                water += Math.Max(0f, visitor.DailyWaterRate);
            }

            _system.TickDay(currentDay);

            if (ConsumeDailyRations != null && before.Count > 0 && (food > 0f || water > 0f))
            {
                bool covered = ConsumeDailyRations(food, water);
                if (!covered)
                    JournalEvent?.Invoke("Visitor rations ran short this day.");
            }
        }

        public VisitorIntegrationSaveState CaptureState() => _system.CaptureState();

        public void RestoreState(VisitorIntegrationSaveState state)
        {
            _system.RestoreState(state);
            StateChanged?.Invoke();
        }

        private void EnsureStandardRequirements(VisitorRecord visitor, int currentDay)
        {
            if (visitor == null) return;
            foreach (string taskType in DefaultRequirementTasks)
            {
                if (_system.Tasks.Any(t => string.Equals(t.VisitorId, visitor.VisitorId, StringComparison.Ordinal)
                    && string.Equals(t.TaskType, taskType, StringComparison.OrdinalIgnoreCase)))
                    continue;
                _system.AssignIntegrationTask(visitor.VisitorId, taskType, visitor.AdmittedBy, currentDay, durationDays: 2);
            }
        }

        private void AssignDefaultHousing(VisitorRecord visitor, string templateId, int currentDay)
        {
            var template = _system.GetTemplate(templateId);
            if (template == null || visitor == null) return;
            if (!string.IsNullOrEmpty(visitor.AssignedRoomId)) return;
            _system.AssignHousing(visitor.VisitorId, "visitor_berth_" + visitor.VisitorId, template.ParseHousing(), currentDay);
        }

        private static VisitorType ParseVisitorType(string visitorType)
        {
            return visitorType?.ToLowerInvariant() switch
            {
                "trader" => VisitorType.Trader,
                "merchant" => VisitorType.Trader,
                "defector" => VisitorType.Defector,
                "guest" => VisitorType.Guest,
                "envoy" => VisitorType.Envoy,
                "deserter" => VisitorType.Deserter,
                "exile" => VisitorType.Exile,
                "refugee" => VisitorType.Refugee,
                _ => VisitorType.Refugee
            };
        }

        private static int DurationFor(VisitorType type)
        {
            return type switch
            {
                VisitorType.Trader => 3,
                VisitorType.Envoy => 5,
                VisitorType.Guest => 7,
                _ => -1
            };
        }

        private static string BuildVisitorName(string sourceVisitorId, VisitorType type)
        {
            if (string.IsNullOrWhiteSpace(sourceVisitorId)) return type.ToString() + " Visitor";
            string trimmed = sourceVisitorId.Trim();
            // Humanize the source id ("trader_meridian_01" -> "Trader Meridian 01")
            // without inventing authored canon.
            var builder = new System.Text.StringBuilder(trimmed.Length);
            bool upper = true;
            foreach (char c in trimmed)
            {
                if (c == '_' || c == '-' || c == ' ')
                {
                    builder.Append(' ');
                    upper = true;
                }
                else if (upper)
                {
                    builder.Append(char.ToUpperInvariant(c));
                    upper = false;
                }
                else
                {
                    builder.Append(c);
                }
            }
            return builder.ToString();
        }
    }
}
