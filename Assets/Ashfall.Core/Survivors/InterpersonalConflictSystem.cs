// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Ashfall.Core;

namespace Ashfall.Core.Survivors
{
    // ── Catalog DTOs ────────────────────────────────────────────────────────

    [Serializable]
    public sealed class ConflictTemplateDef
    {
        public string template_id { get; set; } = string.Empty;
        public string conflict_type { get; set; } = "argument";
        public string display_name { get; set; } = string.Empty;
        public string trigger_description { get; set; } = string.Empty;
        public string default_severity { get; set; } = "mild";
        public float base_escalation { get; set; } = 20.0f;
        public float grievance_intensity { get; set; } = 25.0f;
        public string description { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class ConflictTemplatesCatalog
    {
        public int schema_version { get; set; } = 1;
        public List<ConflictTemplateDef> templates { get; set; } = new List<ConflictTemplateDef>();
    }

    // ── State Enums & DTOs ──────────────────────────────────────────────────

    public enum ConflictType
    {
        Argument = 0,
        Grudge = 1,
        PersonalityClash = 2,
        ResourceDispute = 3,
        ShiftConflict = 4,
        FairnessGrievance = 5,
        PersonalSlight = 6,
        Betrayal = 7
    }

    public enum ConflictSeverity
    {
        Mild = 0,
        Moderate = 1,
        Severe = 2,
        Crisis = 3
    }

    public enum ConflictResolutionMethod
    {
        Apology = 0,
        Mediation = 1,
        Compensation = 2,
        Disciplinary = 3,
        NaturalDecay = 4
    }

    [Serializable]
    public sealed class InterpersonalConflict
    {
        public string ConflictId { get; set; } = string.Empty;
        public string InitiatorId { get; set; } = string.Empty;
        public string TargetId { get; set; } = string.Empty;
        public ConflictType Type { get; set; } = ConflictType.Argument;
        public string TriggerDescription { get; set; } = string.Empty;
        public ConflictSeverity Severity { get; set; } = ConflictSeverity.Mild;
        public float EscalationScore { get; set; } = 20f; // 0 to 100
        public bool IsResolved { get; set; } = false;
        public int StartedDay { get; set; } = 1;
        public int ResolvedDay { get; set; } = -1;
        public ConflictResolutionMethod? ResolutionMethod { get; set; }
    }

    [Serializable]
    public sealed class SurvivorGrievance
    {
        public string GrievanceId { get; set; } = string.Empty;
        public string HolderId { get; set; } = string.Empty;
        public string AccusedId { get; set; } = string.Empty;
        public string GrievanceReason { get; set; } = string.Empty;
        public float Intensity { get; set; } = 30f; // 0 to 100
        public int CreatedDay { get; set; } = 1;
    }

    [Serializable]
    public sealed class ConflictResolutionRecord
    {
        public string ResolutionId { get; set; } = string.Empty;
        public string ConflictId { get; set; } = string.Empty;
        public ConflictResolutionMethod Method { get; set; } = ConflictResolutionMethod.Apology;
        public string MediatorId { get; set; } = string.Empty;
        public int Day { get; set; } = 1;
        public float AffinityChange { get; set; } = 0f;
    }

    [Serializable]
    public sealed class InterpersonalConflictState
    {
        public int SchemaVersion { get; set; } = 1;
        public int NextSequence { get; set; } = 1;
        public List<InterpersonalConflict> Conflicts { get; set; } = new List<InterpersonalConflict>();
        public List<SurvivorGrievance> Grievances { get; set; } = new List<SurvivorGrievance>();
        public List<ConflictResolutionRecord> Resolutions { get; set; } = new List<ConflictResolutionRecord>();
    }

    /// <summary>
    /// Plan 202 — Survivor Interpersonal Conflict & Grievance System.
    /// Manages non-ideological disputes between survivors (arguments, grudges, resource disputes),
    /// conflict escalation ladders, grievances, mediation, and relationship resolution.
    /// </summary>
    public sealed class InterpersonalConflictSystem
    {
        public const string ResolutionMoraleSource = "relations.conflict_resolution";

        private readonly InterpersonalConflictState _state;

        public event Action<InterpersonalConflict>? OnConflictInitiated;
        public event Action<InterpersonalConflict, float>? OnConflictEscalated;
        public event Action<InterpersonalConflict, ConflictResolutionRecord>? OnConflictResolved;
        public event Action<InterpersonalConflict>? OnPhysicalFightRisk;

        public int ActiveConflictCount => _state.Conflicts.Count(c => !c.IsResolved);
        public int TotalGrievancesCount => _state.Grievances.Count;

        /// <summary>
        /// Typed morale outcome for a canonical relations mediation. Needs
        /// remains the mutation owner; this value is only the conflict package's
        /// deterministic outcome fact.
        /// </summary>
        public static float MoraleDeltaFor(MediationStyle style)
        {
            return style switch
            {
                MediationStyle.Apology => 2f,
                MediationStyle.ResourceSettlement => 3f,
                MediationStyle.Discipline => -1f,
                MediationStyle.Refusal => -3f,
                _ => 0f
            };
        }

        /// <summary>
        /// Plan 202 integration seam: project the canonical survivor-relations
        /// conflict ledger into this package's typed read model. The projection
        /// never creates, resolves, or mutates a relations entry; callers must
        /// route mediation back through <see cref="SurvivorRelationsSystem"/>.
        /// </summary>
        public static IReadOnlyList<InterpersonalConflict> ProjectCanonicalRelations(
            SurvivorRelationsState relations,
            int currentDay)
        {
            var projected = new List<InterpersonalConflict>();
            if (relations?.activeConflicts == null) return projected;

            for (int i = 0; i < relations.activeConflicts.Count; i++)
            {
                var source = relations.activeConflicts[i];
                if (source == null || source.isResolved
                    || string.IsNullOrEmpty(source.conflictId)
                    || string.IsNullOrEmpty(source.dwellerA)
                    || string.IsNullOrEmpty(source.dwellerB)) continue;

                float stress = RelationshipStress(relations, source.dwellerA, source.dwellerB);
                projected.Add(new InterpersonalConflict
                {
                    ConflictId = "relations:" + source.conflictId,
                    InitiatorId = source.dwellerA,
                    TargetId = source.dwellerB,
                    Type = ConflictType.Argument,
                    TriggerDescription = string.IsNullOrEmpty(source.cause)
                        ? "canonical relationship conflict"
                        : source.cause,
                    Severity = SeverityFor(stress),
                    EscalationScore = stress,
                    IsResolved = false,
                    StartedDay = Math.Max(1, source.dayStarted > 0 ? source.dayStarted : currentDay),
                    ResolvedDay = -1,
                    ResolutionMethod = null
                });
            }

            projected.Sort((a, b) => string.CompareOrdinal(a.ConflictId, b.ConflictId));
            return projected;
        }

        /// <summary>
        /// Project relationship stress into typed grievances. Only persisted
        /// affinity/resentment facts are eligible; no random or wall-clock
        /// source is introduced by this read model.
        /// </summary>
        public static IReadOnlyList<SurvivorGrievance> ProjectCanonicalGrievances(
            SurvivorRelationsState relations,
            int currentDay)
        {
            var projected = new List<SurvivorGrievance>();
            if (relations?.relationships == null) return projected;

            for (int i = 0; i < relations.relationships.Count; i++)
            {
                var source = relations.relationships[i];
                if (source == null || string.IsNullOrEmpty(source.dwellerA)
                    || string.IsNullOrEmpty(source.dwellerB)) continue;

                float intensity = RelationshipStress(source);
                if (intensity < 25f) continue;

                string first = string.CompareOrdinal(source.dwellerA, source.dwellerB) <= 0
                    ? source.dwellerA : source.dwellerB;
                string second = string.CompareOrdinal(source.dwellerA, source.dwellerB) <= 0
                    ? source.dwellerB : source.dwellerA;
                projected.Add(new SurvivorGrievance
                {
                    GrievanceId = "relations:" + first + "|" + second,
                    HolderId = first,
                    AccusedId = second,
                    GrievanceReason = "canonical relationship stress",
                    Intensity = Math.Clamp(intensity, 0f, 100f),
                    CreatedDay = Math.Max(1, currentDay)
                });
            }

            projected.Sort((a, b) => string.CompareOrdinal(a.GrievanceId, b.GrievanceId));
            return projected;
        }

        private static float RelationshipStress(
            SurvivorRelationsState relations, string first, string second)
        {
            if (relations?.relationships == null) return 0f;
            for (int i = 0; i < relations.relationships.Count; i++)
            {
                var relation = relations.relationships[i];
                if (relation == null) continue;
                bool samePair = (string.Equals(relation.dwellerA, first, StringComparison.Ordinal)
                    && string.Equals(relation.dwellerB, second, StringComparison.Ordinal))
                    || (string.Equals(relation.dwellerA, second, StringComparison.Ordinal)
                    && string.Equals(relation.dwellerB, first, StringComparison.Ordinal));
                if (samePair) return RelationshipStress(relation);
            }
            return 0f;
        }

        private static float RelationshipStress(RelationshipEntry relation)
        {
            return Math.Clamp(Math.Max(relation.resentment, Math.Max(0f, -relation.affinity)), 0f, 100f);
        }

        private static ConflictSeverity SeverityFor(float stress)
        {
            if (stress >= 80f) return ConflictSeverity.Crisis;
            if (stress >= 50f) return ConflictSeverity.Severe;
            if (stress >= 25f) return ConflictSeverity.Moderate;
            return ConflictSeverity.Mild;
        }

        private readonly Dictionary<string, ConflictTemplateDef> _templateDefs =
            new Dictionary<string, ConflictTemplateDef>(StringComparer.OrdinalIgnoreCase);

        public void LoadCatalog(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                throw new ArgumentException("Catalog JSON cannot be null or empty", nameof(json));

            var catalog = JsonSerializer.Deserialize<ConflictTemplatesCatalog>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (catalog?.templates == null) return;

            _templateDefs.Clear();
            foreach (var template in catalog.templates)
            {
                if (!string.IsNullOrEmpty(template.template_id))
                {
                    _templateDefs[template.template_id] = template;
                }
            }
        }

        public IReadOnlyList<ConflictTemplateDef> GetAllTemplates() => _templateDefs.Values.ToList();

        public ConflictTemplateDef? GetTemplate(string templateId)
        {
            _templateDefs.TryGetValue(templateId, out var template);
            return template;
        }

        public InterpersonalConflict? InitiateConflictFromTemplate(
            string templateId, string initiatorId, string targetId, int currentDay = 1)
        {
            if (!_templateDefs.TryGetValue(templateId, out var template)) return null;

            ConflictType type = ParseConflictType(template.conflict_type);
            ConflictSeverity severity = ParseSeverity(template.default_severity);

            var conflict = InitiateConflict(initiatorId, targetId, type, template.trigger_description, severity, currentDay);
            conflict.EscalationScore = template.base_escalation;

            if (template.grievance_intensity > 0f)
            {
                AddGrievance(initiatorId, targetId, template.trigger_description, template.grievance_intensity, currentDay);
            }

            return conflict;
        }

        private static ConflictType ParseConflictType(string type) => type.ToLowerInvariant() switch
        {
            "grudge" => ConflictType.Grudge,
            "personality_clash" => ConflictType.PersonalityClash,
            "resource_dispute" => ConflictType.ResourceDispute,
            "shift_conflict" => ConflictType.ShiftConflict,
            "fairness_grievance" => ConflictType.FairnessGrievance,
            "personal_slight" => ConflictType.PersonalSlight,
            "betrayal" => ConflictType.Betrayal,
            _ => ConflictType.Argument
        };

        private static ConflictSeverity ParseSeverity(string sev) => sev.ToLowerInvariant() switch
        {
            "moderate" => ConflictSeverity.Moderate,
            "severe" => ConflictSeverity.Severe,
            "crisis" => ConflictSeverity.Crisis,
            _ => ConflictSeverity.Mild
        };

        public InterpersonalConflictSystem(InterpersonalConflictState? state = null)
        {
            _state = state ?? new InterpersonalConflictState();
        }

        public InterpersonalConflict InitiateConflict(
            string initiatorId,
            string targetId,
            ConflictType type,
            string triggerDescription,
            ConflictSeverity severity = ConflictSeverity.Mild,
            int currentDay = 1)
        {
            if (string.IsNullOrWhiteSpace(initiatorId)) throw new ArgumentNullException(nameof(initiatorId));
            if (string.IsNullOrWhiteSpace(targetId)) throw new ArgumentNullException(nameof(targetId));

            float baseEscalation = severity switch
            {
                ConflictSeverity.Crisis => 85f,
                ConflictSeverity.Severe => 60f,
                ConflictSeverity.Moderate => 35f,
                _ => 15f
            };

            var conflict = new InterpersonalConflict
            {
                ConflictId = $"cnf_{_state.NextSequence++}",
                InitiatorId = initiatorId.Trim(),
                TargetId = targetId.Trim(),
                Type = type,
                TriggerDescription = triggerDescription ?? string.Empty,
                Severity = severity,
                EscalationScore = baseEscalation,
                IsResolved = false,
                StartedDay = Math.Max(1, currentDay),
                ResolvedDay = -1
            };

            _state.Conflicts.Add(conflict);
            OnConflictInitiated?.Invoke(conflict);

            if (conflict.EscalationScore >= 80f)
            {
                OnPhysicalFightRisk?.Invoke(conflict);
            }

            return conflict;
        }

        public SurvivorGrievance AddGrievance(
            string holderId,
            string accusedId,
            string reason,
            float intensity = 30f,
            int currentDay = 1)
        {
            if (string.IsNullOrWhiteSpace(holderId)) throw new ArgumentNullException(nameof(holderId));
            if (string.IsNullOrWhiteSpace(accusedId)) throw new ArgumentNullException(nameof(accusedId));

            var grievance = new SurvivorGrievance
            {
                GrievanceId = $"grv_{_state.NextSequence++}",
                HolderId = holderId.Trim(),
                AccusedId = accusedId.Trim(),
                GrievanceReason = reason ?? string.Empty,
                Intensity = Math.Clamp(intensity, 0f, 100f),
                CreatedDay = Math.Max(1, currentDay)
            };

            _state.Grievances.Add(grievance);
            return grievance;
        }

        public float EscalateConflict(string conflictId, float delta, int currentDay)
        {
            var conflict = _state.Conflicts.FirstOrDefault(c => string.Equals(c.ConflictId, conflictId, StringComparison.OrdinalIgnoreCase));
            if (conflict == null || conflict.IsResolved) return 0f;

            conflict.EscalationScore = Math.Clamp(conflict.EscalationScore + delta, 0f, 100f);
            OnConflictEscalated?.Invoke(conflict, conflict.EscalationScore);

            if (conflict.EscalationScore >= 80f)
            {
                conflict.Severity = ConflictSeverity.Crisis;
                OnPhysicalFightRisk?.Invoke(conflict);
            }
            else if (conflict.EscalationScore >= 50f)
            {
                conflict.Severity = ConflictSeverity.Severe;
            }

            return conflict.EscalationScore;
        }

        public bool MediateConflict(string conflictId, string mediatorId, int currentDay)
        {
            var conflict = _state.Conflicts.FirstOrDefault(c => string.Equals(c.ConflictId, conflictId, StringComparison.OrdinalIgnoreCase));
            if (conflict == null || conflict.IsResolved) return false;

            conflict.IsResolved = true;
            conflict.ResolvedDay = currentDay;
            conflict.ResolutionMethod = ConflictResolutionMethod.Mediation;
            conflict.EscalationScore = 0f;

            var rec = new ConflictResolutionRecord
            {
                ResolutionId = $"res_{_state.NextSequence++}",
                ConflictId = conflictId,
                Method = ConflictResolutionMethod.Mediation,
                MediatorId = mediatorId ?? "mediator",
                Day = currentDay,
                AffinityChange = 10f
            };
            _state.Resolutions.Add(rec);

            // Remove or reduce matching grievances between the two
            _state.Grievances.RemoveAll(g =>
                (string.Equals(g.HolderId, conflict.InitiatorId, StringComparison.OrdinalIgnoreCase) && string.Equals(g.AccusedId, conflict.TargetId, StringComparison.OrdinalIgnoreCase)) ||
                (string.Equals(g.HolderId, conflict.TargetId, StringComparison.OrdinalIgnoreCase) && string.Equals(g.AccusedId, conflict.InitiatorId, StringComparison.OrdinalIgnoreCase)));

            OnConflictResolved?.Invoke(conflict, rec);
            return true;
        }

        public bool ApologizeAndResolve(string conflictId, int currentDay)
        {
            var conflict = _state.Conflicts.FirstOrDefault(c => string.Equals(c.ConflictId, conflictId, StringComparison.OrdinalIgnoreCase));
            if (conflict == null || conflict.IsResolved) return false;

            conflict.IsResolved = true;
            conflict.ResolvedDay = currentDay;
            conflict.ResolutionMethod = ConflictResolutionMethod.Apology;
            conflict.EscalationScore = 0f;

            var rec = new ConflictResolutionRecord
            {
                ResolutionId = $"res_{_state.NextSequence++}",
                ConflictId = conflictId,
                Method = ConflictResolutionMethod.Apology,
                MediatorId = string.Empty,
                Day = currentDay,
                AffinityChange = 5f
            };
            _state.Resolutions.Add(rec);

            OnConflictResolved?.Invoke(conflict, rec);
            return true;
        }

        public void TickDay(int currentDay)
        {
            // 1. Grievance natural decay
            for (int i = _state.Grievances.Count - 1; i >= 0; i--)
            {
                var g = _state.Grievances[i];
                g.Intensity -= 2f;
                if (g.Intensity <= 0f)
                {
                    _state.Grievances.RemoveAt(i);
                }
            }

            // 2. Active conflict progression or decay
            foreach (var conflict in _state.Conflicts)
            {
                if (conflict.IsResolved) continue;

                // Check if either party holds active grievance against the other
                bool hasGrievance = _state.Grievances.Any(g =>
                    (string.Equals(g.HolderId, conflict.InitiatorId, StringComparison.OrdinalIgnoreCase) && string.Equals(g.AccusedId, conflict.TargetId, StringComparison.OrdinalIgnoreCase)) ||
                    (string.Equals(g.HolderId, conflict.TargetId, StringComparison.OrdinalIgnoreCase) && string.Equals(g.AccusedId, conflict.InitiatorId, StringComparison.OrdinalIgnoreCase)));

                if (hasGrievance)
                {
                    // Conflict simmers
                    conflict.EscalationScore = Math.Clamp(conflict.EscalationScore + 1.5f, 0f, 100f);
                }
                else
                {
                    // Conflict naturally cools down
                    conflict.EscalationScore = Math.Clamp(conflict.EscalationScore - 3.0f, 0f, 100f);
                    if (conflict.EscalationScore <= 0f)
                    {
                        conflict.IsResolved = true;
                        conflict.ResolvedDay = currentDay;
                        conflict.ResolutionMethod = ConflictResolutionMethod.NaturalDecay;
                    }
                }
            }
        }

        public IReadOnlyList<InterpersonalConflict> GetActiveConflicts()
        {
            return _state.Conflicts.Where(c => !c.IsResolved).ToList();
        }

        public IReadOnlyList<SurvivorGrievance> GetGrievancesForSurvivor(string survivorId)
        {
            return _state.Grievances.Where(g => string.Equals(g.HolderId, survivorId, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public InterpersonalConflictState CaptureState()
        {
            var state = new InterpersonalConflictState
            {
                SchemaVersion = _state.SchemaVersion,
                NextSequence = _state.NextSequence,
                Conflicts = new List<InterpersonalConflict>(_state.Conflicts.Count),
                Grievances = new List<SurvivorGrievance>(_state.Grievances.Count),
                Resolutions = new List<ConflictResolutionRecord>(_state.Resolutions.Count)
            };

            foreach (var c in _state.Conflicts)
            {
                state.Conflicts.Add(new InterpersonalConflict
                {
                    ConflictId = c.ConflictId,
                    InitiatorId = c.InitiatorId,
                    TargetId = c.TargetId,
                    Type = c.Type,
                    TriggerDescription = c.TriggerDescription,
                    Severity = c.Severity,
                    EscalationScore = c.EscalationScore,
                    IsResolved = c.IsResolved,
                    StartedDay = c.StartedDay,
                    ResolvedDay = c.ResolvedDay,
                    ResolutionMethod = c.ResolutionMethod
                });
            }

            foreach (var g in _state.Grievances)
            {
                state.Grievances.Add(new SurvivorGrievance
                {
                    GrievanceId = g.GrievanceId,
                    HolderId = g.HolderId,
                    AccusedId = g.AccusedId,
                    GrievanceReason = g.GrievanceReason,
                    Intensity = g.Intensity,
                    CreatedDay = g.CreatedDay
                });
            }

            foreach (var r in _state.Resolutions)
            {
                state.Resolutions.Add(new ConflictResolutionRecord
                {
                    ResolutionId = r.ResolutionId,
                    ConflictId = r.ConflictId,
                    Method = r.Method,
                    MediatorId = r.MediatorId,
                    Day = r.Day,
                    AffinityChange = r.AffinityChange
                });
            }

            return state;
        }

        public void RestoreState(InterpersonalConflictState state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));

            _state.SchemaVersion = state.SchemaVersion;
            _state.NextSequence = state.NextSequence;
            _state.Conflicts.Clear();
            _state.Grievances.Clear();
            _state.Resolutions.Clear();

            if (state.Conflicts != null)
            {
                foreach (var c in state.Conflicts)
                {
                    _state.Conflicts.Add(new InterpersonalConflict
                    {
                        ConflictId = c.ConflictId,
                        InitiatorId = c.InitiatorId,
                        TargetId = c.TargetId,
                        Type = c.Type,
                        TriggerDescription = c.TriggerDescription,
                        Severity = c.Severity,
                        EscalationScore = c.EscalationScore,
                        IsResolved = c.IsResolved,
                        StartedDay = c.StartedDay,
                        ResolvedDay = c.ResolvedDay,
                        ResolutionMethod = c.ResolutionMethod
                    });
                }
            }

            if (state.Grievances != null)
            {
                foreach (var g in state.Grievances)
                {
                    _state.Grievances.Add(new SurvivorGrievance
                    {
                        GrievanceId = g.GrievanceId,
                        HolderId = g.HolderId,
                        AccusedId = g.AccusedId,
                        GrievanceReason = g.GrievanceReason,
                        Intensity = g.Intensity,
                        CreatedDay = g.CreatedDay
                    });
                }
            }

            if (state.Resolutions != null)
            {
                foreach (var r in state.Resolutions)
                {
                    _state.Resolutions.Add(new ConflictResolutionRecord
                    {
                        ResolutionId = r.ResolutionId,
                        ConflictId = r.ConflictId,
                        Method = r.Method,
                        MediatorId = r.MediatorId,
                        Day = r.Day,
                        AffinityChange = r.AffinityChange
                    });
                }
            }
        }
    }
}
