// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 188 — Individual Survivor Daily Routines
// Pure domain authority for survivor daily activity schedules, time blocks,
// chronotype preferences, routine satisfaction, and interpersonal conflicts.
// ============================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Ashfall.Core.Survivors
{
    // ── Catalog DTOs ────────────────────────────────────────────────────────

    [Serializable]
    public sealed class TimeBlockDef
    {
        public string block_id { get; set; } = string.Empty;
        public int start_hour { get; set; } // 0 - 23
        public int end_hour { get; set; }   // 0 - 23
        public string activity_type { get; set; } = "Work"; // Sleep, Work, Meal, Social, Personal, Leisure
        public string flexibility { get; set; } = "flexible"; // rigid, flexible
    }

    [Serializable]
    public sealed class RoutineTemplateDef
    {
        public string template_id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public int wake_hour { get; set; } = 7;
        public int sleep_hour { get; set; } = 23;
        public List<int> meal_hours { get; set; } = new List<int>();
        public int work_start_hour { get; set; } = 9;
        public int work_end_hour { get; set; } = 17;
        public string preferred_chronotype { get; set; } = "intermediate"; // early_riser, night_owl, intermediate
        public string description { get; set; } = string.Empty;
        public List<TimeBlockDef> default_blocks { get; set; } = new List<TimeBlockDef>();
    }

    [Serializable]
    public sealed class RoutineTemplatesCatalog
    {
        public int schema_version { get; set; } = 1;
        public List<RoutineTemplateDef> templates { get; set; } = new List<RoutineTemplateDef>();
    }

    // ── Persistent State DTOs ───────────────────────────────────────────────

    [Serializable]
    public sealed class SurvivorRoutineRecord
    {
        public string SurvivorId { get; set; } = string.Empty;
        public string TemplateId { get; set; } = string.Empty;
        public int WakeHour { get; set; } = 7;
        public int SleepHour { get; set; } = 23;
        public List<int> MealHours { get; set; } = new List<int>();
        public List<TimeBlockDef> TimeBlocks { get; set; } = new List<TimeBlockDef>();
    }

    [Serializable]
    public sealed class RoutinePreferenceRecord
    {
        public string SurvivorId { get; set; } = string.Empty;
        public string Chronotype { get; set; } = "intermediate"; // early_riser, night_owl, intermediate
        public string WorkShiftPreference { get; set; } = "morning"; // morning, afternoon, night
        public string SocialPreference { get; set; } = "balanced"; // introvert, extrovert, balanced
    }

    [Serializable]
    public sealed class RoutineSatisfactionRecord
    {
        public string SurvivorId { get; set; } = string.Empty;
        public int Day { get; set; }
        public float SleepSatisfaction { get; set; } = 100.0f;
        public float MealSatisfaction { get; set; } = 100.0f;
        public float WorkSatisfaction { get; set; } = 100.0f;
        public float SocialSatisfaction { get; set; } = 100.0f;
        public float OverallSatisfaction { get; set; } = 100.0f;
    }

    [Serializable]
    public sealed class RoutineConflictRecord
    {
        public string ConflictId { get; set; } = string.Empty;
        public string SurvivorA { get; set; } = string.Empty;
        public string SurvivorB { get; set; } = string.Empty;
        public string ConflictType { get; set; } = "sleep_disturbance"; // workspace_overlap, sleep_disturbance, meal_conflict, social_clash
        public int Day { get; set; }
        public string Severity { get; set; } = "minor"; // minor, moderate, major
        public bool IsResolved { get; set; }
    }

    [Serializable]
    public sealed class SurvivorRoutineState
    {
        public int SchemaVersion { get; set; } = 1;
        public int NextSequence { get; set; } = 1;
        public string EnforcementLevel { get; set; } = "flexible"; // strict, flexible, none
        public List<SurvivorRoutineRecord> Routines { get; set; } = new List<SurvivorRoutineRecord>();
        public List<RoutinePreferenceRecord> Preferences { get; set; } = new List<RoutinePreferenceRecord>();
        public List<RoutineSatisfactionRecord> DailySatisfactions { get; set; } = new List<RoutineSatisfactionRecord>();
        public List<RoutineConflictRecord> Conflicts { get; set; } = new List<RoutineConflictRecord>();
    }

    // ── Domain System ───────────────────────────────────────────────────────

    public sealed class SurvivorRoutineSystem
    {
        private readonly SurvivorRoutineState _state;
        private readonly Dictionary<string, RoutineTemplateDef> _templates =
            new Dictionary<string, RoutineTemplateDef>(StringComparer.OrdinalIgnoreCase);

        public event Action<string, string>? OnRoutineAssigned;     // (survivorId, templateId)
        public event Action<RoutineSatisfactionRecord>? OnSatisfactionEvaluated;
        public event Action<RoutineConflictRecord>? OnConflictDetected;
        public event Action<string>? OnConflictResolved;            // (conflictId)

        public string EnforcementLevel => _state.EnforcementLevel;
        public int TrackedRoutineCount => _state.Routines.Count;

        public SurvivorRoutineSystem()
        {
            _state = new SurvivorRoutineState();
        }

        public SurvivorRoutineSystem(SurvivorRoutineState state)
        {
            _state = state ?? new SurvivorRoutineState();
        }

        // ── Catalog Loading ────────────────────────────────────────────────

        public void LoadCatalog(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return;
            try
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var catalog = JsonSerializer.Deserialize<RoutineTemplatesCatalog>(json, options);
                if (catalog?.templates == null) return;

                _templates.Clear();
                foreach (var t in catalog.templates)
                {
                    if (string.IsNullOrWhiteSpace(t.template_id)) continue;
                    _templates[t.template_id] = t;
                }
            }
            catch (Exception) { /* malformed catalog falls back to built-in defaults; authoring errors are enforced by the data-integrity gate */ }
        }

        public IReadOnlyCollection<RoutineTemplateDef> GetAllTemplates() => _templates.Values;

        public RoutineTemplateDef? GetTemplate(string templateId)
        {
            return _templates.TryGetValue(templateId, out var def) ? def : null;
        }

        public void SetEnforcementLevel(string level)
        {
            if (!string.IsNullOrWhiteSpace(level))
                _state.EnforcementLevel = level.Trim().ToLowerInvariant();
        }

        // ── Routine Management ─────────────────────────────────────────────

        public SurvivorRoutineRecord AssignRoutine(string survivorId, string templateId)
        {
            if (string.IsNullOrWhiteSpace(survivorId)) throw new ArgumentException("survivorId is required");

            var template = GetTemplate(templateId);
            var record = _state.Routines.FirstOrDefault(r =>
                string.Equals(r.SurvivorId, survivorId, StringComparison.OrdinalIgnoreCase));

            if (record == null)
            {
                record = new SurvivorRoutineRecord { SurvivorId = survivorId };
                _state.Routines.Add(record);
            }

            record.TemplateId = templateId;
            if (template != null)
            {
                record.WakeHour = template.wake_hour;
                record.SleepHour = template.sleep_hour;
                record.MealHours = new List<int>(template.meal_hours);
                record.TimeBlocks = template.default_blocks.Select(b => new TimeBlockDef
                {
                    block_id = b.block_id,
                    start_hour = b.start_hour,
                    end_hour = b.end_hour,
                    activity_type = b.activity_type,
                    flexibility = b.flexibility
                }).ToList();
            }

            OnRoutineAssigned?.Invoke(survivorId, templateId);
            return record;
        }

        public SurvivorRoutineRecord? GetRoutine(string survivorId)
        {
            return _state.Routines.FirstOrDefault(r =>
                string.Equals(r.SurvivorId, survivorId, StringComparison.OrdinalIgnoreCase));
        }

        public void SetPreference(string survivorId, string chronotype, string workShift = "morning", string social = "balanced")
        {
            var pref = _state.Preferences.FirstOrDefault(p =>
                string.Equals(p.SurvivorId, survivorId, StringComparison.OrdinalIgnoreCase));

            if (pref == null)
            {
                pref = new RoutinePreferenceRecord { SurvivorId = survivorId };
                _state.Preferences.Add(pref);
            }

            pref.Chronotype = chronotype;
            pref.WorkShiftPreference = workShift;
            pref.SocialPreference = social;
        }

        public RoutinePreferenceRecord? GetPreference(string survivorId)
        {
            return _state.Preferences.FirstOrDefault(p =>
                string.Equals(p.SurvivorId, survivorId, StringComparison.OrdinalIgnoreCase));
        }

        public string GetActivityAtHour(string survivorId, int hour)
        {
            hour = (hour % 24 + 24) % 24;
            var routine = GetRoutine(survivorId);
            if (routine == null || routine.TimeBlocks.Count == 0) return "Idle";

            foreach (var block in routine.TimeBlocks)
            {
                if (block.start_hour <= block.end_hour)
                {
                    if (hour >= block.start_hour && hour < block.end_hour)
                        return block.activity_type;
                }
                else
                {
                    // Block wraps across midnight (e.g. 21 to 5)
                    if (hour >= block.start_hour || hour < block.end_hour)
                        return block.activity_type;
                }
            }

            return "Personal";
        }

        // ── Satisfaction Evaluation ────────────────────────────────────────

        public RoutineSatisfactionRecord EvaluateDailySatisfaction(
            string survivorId, int day, int hoursWorked, int hoursSlept, int mealsHad, int socialHours)
        {
            var routine = GetRoutine(survivorId);
            var pref = GetPreference(survivorId);

            // Sleep satisfaction (8 hours baseline)
            float sleepRatio = Math.Clamp(hoursSlept / 8.0f, 0.0f, 1.5f);
            float sleepScore = sleepRatio <= 1.0f ? sleepRatio * 100.0f : (2.0f - sleepRatio) * 100.0f;

            // Meal satisfaction (3 meals baseline)
            float mealScore = Math.Clamp(mealsHad / 3.0f, 0.0f, 1.0f) * 100.0f;

            // Work satisfaction (8 hours baseline, penalize overtime / undertime)
            float workDiff = Math.Abs(hoursWorked - 8);
            float workScore = Math.Clamp(100.0f - workDiff * 12.0f, 0.0f, 100.0f);

            // Social satisfaction based on preference
            float targetSocial = 3.0f;
            if (pref != null)
            {
                if (string.Equals(pref.SocialPreference, "introvert", StringComparison.OrdinalIgnoreCase)) targetSocial = 1.5f;
                else if (string.Equals(pref.SocialPreference, "extrovert", StringComparison.OrdinalIgnoreCase)) targetSocial = 4.5f;
            }
            float socialDiff = Math.Abs(socialHours - targetSocial);
            float socialScore = Math.Clamp(100.0f - socialDiff * 20.0f, 0.0f, 100.0f);

            // Weighted overall
            float overall = (sleepScore * 0.35f) + (mealScore * 0.25f) + (workScore * 0.25f) + (socialScore * 0.15f);

            var record = new RoutineSatisfactionRecord
            {
                SurvivorId = survivorId,
                Day = day,
                SleepSatisfaction = (float)Math.Round(sleepScore, 1),
                MealSatisfaction = (float)Math.Round(mealScore, 1),
                WorkSatisfaction = (float)Math.Round(workScore, 1),
                SocialSatisfaction = (float)Math.Round(socialScore, 1),
                OverallSatisfaction = (float)Math.Round(overall, 1)
            };

            _state.DailySatisfactions.Add(record);
            OnSatisfactionEvaluated?.Invoke(record);
            return record;
        }

        // ── Conflict Detection & Resolution ────────────────────────────────

        public IReadOnlyList<RoutineConflictRecord> DetectConflicts(
            int day,
            Dictionary<string, string>? roomAssignments = null,
            Dictionary<string, string>? workspaceAssignments = null)
        {
            var detected = new List<RoutineConflictRecord>();

            // 1. Roommate sleep schedule clash
            if (roomAssignments != null)
            {
                var roomGroups = roomAssignments
                    .GroupBy(kv => kv.Value, kv => kv.Key)
                    .Where(g => g.Count() > 1);

                foreach (var group in roomGroups)
                {
                    var roommates = group.ToList();
                    for (int i = 0; i < roommates.Count; i++)
                    {
                        for (int j = i + 1; j < roommates.Count; j++)
                        {
                            var rA = GetRoutine(roommates[i]);
                            var rB = GetRoutine(roommates[j]);
                            if (rA == null || rB == null) continue;

                            // If wake/sleep difference > 3 hours -> major sleep disturbance
                            int sleepDiff = Math.Abs(rA.SleepHour - rB.SleepHour);
                            if (sleepDiff > 12) sleepDiff = 24 - sleepDiff;

                            if (sleepDiff >= 4)
                            {
                                var conflict = new RoutineConflictRecord
                                {
                                    ConflictId = $"conf_{_state.NextSequence++}",
                                    SurvivorA = roommates[i],
                                    SurvivorB = roommates[j],
                                    ConflictType = "sleep_disturbance",
                                    Day = day,
                                    Severity = "major",
                                    IsResolved = false
                                };
                                detected.Add(conflict);
                                _state.Conflicts.Add(conflict);
                                OnConflictDetected?.Invoke(conflict);
                            }
                        }
                    }
                }
            }

            // 2. Workspace overlap
            if (workspaceAssignments != null)
            {
                var workGroups = workspaceAssignments
                    .GroupBy(kv => kv.Value, kv => kv.Key)
                    .Where(g => g.Count() > 1);

                foreach (var group in workGroups)
                {
                    var workers = group.ToList();
                    for (int i = 0; i < workers.Count; i++)
                    {
                        for (int j = i + 1; j < workers.Count; j++)
                        {
                            var rA = GetRoutine(workers[i]);
                            var rB = GetRoutine(workers[j]);
                            if (rA == null || rB == null) continue;

                            // Check if both working at hour 12
                            bool aWorksAt12 = GetActivityAtHour(workers[i], 12) == "Work";
                            bool bWorksAt12 = GetActivityAtHour(workers[j], 12) == "Work";

                            if (aWorksAt12 && bWorksAt12)
                            {
                                var conflict = new RoutineConflictRecord
                                {
                                    ConflictId = $"conf_{_state.NextSequence++}",
                                    SurvivorA = workers[i],
                                    SurvivorB = workers[j],
                                    ConflictType = "workspace_overlap",
                                    Day = day,
                                    Severity = "moderate",
                                    IsResolved = false
                                };
                                detected.Add(conflict);
                                _state.Conflicts.Add(conflict);
                                OnConflictDetected?.Invoke(conflict);
                            }
                        }
                    }
                }
            }

            return detected;
        }

        public bool ResolveConflict(string conflictId)
        {
            var conflict = _state.Conflicts.FirstOrDefault(c =>
                string.Equals(c.ConflictId, conflictId, StringComparison.OrdinalIgnoreCase));
            if (conflict == null || conflict.IsResolved) return false;

            conflict.IsResolved = true;
            OnConflictResolved?.Invoke(conflictId);
            return true;
        }

        public IReadOnlyList<RoutineConflictRecord> GetActiveConflicts()
        {
            return _state.Conflicts.Where(c => !c.IsResolved).ToList();
        }

        // ── Save / Restore ─────────────────────────────────────────────────

        public SurvivorRoutineState CaptureState()
        {
            return new SurvivorRoutineState
            {
                SchemaVersion = _state.SchemaVersion,
                NextSequence = _state.NextSequence,
                EnforcementLevel = _state.EnforcementLevel,
                Routines = _state.Routines.Select(r => new SurvivorRoutineRecord
                {
                    SurvivorId = r.SurvivorId,
                    TemplateId = r.TemplateId,
                    WakeHour = r.WakeHour,
                    SleepHour = r.SleepHour,
                    MealHours = new List<int>(r.MealHours),
                    TimeBlocks = r.TimeBlocks.Select(b => new TimeBlockDef
                    {
                        block_id = b.block_id,
                        start_hour = b.start_hour,
                        end_hour = b.end_hour,
                        activity_type = b.activity_type,
                        flexibility = b.flexibility
                    }).ToList()
                }).ToList(),
                Preferences = _state.Preferences.Select(p => new RoutinePreferenceRecord
                {
                    SurvivorId = p.SurvivorId,
                    Chronotype = p.Chronotype,
                    WorkShiftPreference = p.WorkShiftPreference,
                    SocialPreference = p.SocialPreference
                }).ToList(),
                DailySatisfactions = _state.DailySatisfactions.Select(s => new RoutineSatisfactionRecord
                {
                    SurvivorId = s.SurvivorId,
                    Day = s.Day,
                    SleepSatisfaction = s.SleepSatisfaction,
                    MealSatisfaction = s.MealSatisfaction,
                    WorkSatisfaction = s.WorkSatisfaction,
                    SocialSatisfaction = s.SocialSatisfaction,
                    OverallSatisfaction = s.OverallSatisfaction
                }).ToList(),
                Conflicts = _state.Conflicts.Select(c => new RoutineConflictRecord
                {
                    ConflictId = c.ConflictId,
                    SurvivorA = c.SurvivorA,
                    SurvivorB = c.SurvivorB,
                    ConflictType = c.ConflictType,
                    Day = c.Day,
                    Severity = c.Severity,
                    IsResolved = c.IsResolved
                }).ToList()
            };
        }

        public void RestoreState(SurvivorRoutineState? saved)
        {
            if (saved == null) return;
            _state.SchemaVersion = saved.SchemaVersion;
            _state.NextSequence = saved.NextSequence > 0 ? saved.NextSequence : 1;
            _state.EnforcementLevel = !string.IsNullOrWhiteSpace(saved.EnforcementLevel) ? saved.EnforcementLevel : "flexible";

            _state.Routines = saved.Routines?.Select(r => new SurvivorRoutineRecord
            {
                SurvivorId = r.SurvivorId,
                TemplateId = r.TemplateId,
                WakeHour = r.WakeHour,
                SleepHour = r.SleepHour,
                MealHours = new List<int>(r.MealHours ?? new List<int>()),
                TimeBlocks = r.TimeBlocks?.Select(b => new TimeBlockDef
                {
                    block_id = b.block_id,
                    start_hour = b.start_hour,
                    end_hour = b.end_hour,
                    activity_type = b.activity_type,
                    flexibility = b.flexibility
                }).ToList() ?? new List<TimeBlockDef>()
            }).ToList() ?? new List<SurvivorRoutineRecord>();

            _state.Preferences = saved.Preferences?.Select(p => new RoutinePreferenceRecord
            {
                SurvivorId = p.SurvivorId,
                Chronotype = p.Chronotype,
                WorkShiftPreference = p.WorkShiftPreference,
                SocialPreference = p.SocialPreference
            }).ToList() ?? new List<RoutinePreferenceRecord>();

            _state.DailySatisfactions = saved.DailySatisfactions?.Select(s => new RoutineSatisfactionRecord
            {
                SurvivorId = s.SurvivorId,
                Day = s.Day,
                SleepSatisfaction = s.SleepSatisfaction,
                MealSatisfaction = s.MealSatisfaction,
                WorkSatisfaction = s.WorkSatisfaction,
                SocialSatisfaction = s.SocialSatisfaction,
                OverallSatisfaction = s.OverallSatisfaction
            }).ToList() ?? new List<RoutineSatisfactionRecord>();

            _state.Conflicts = saved.Conflicts?.Select(c => new RoutineConflictRecord
            {
                ConflictId = c.ConflictId,
                SurvivorA = c.SurvivorA,
                SurvivorB = c.SurvivorB,
                ConflictType = c.ConflictType,
                Day = c.Day,
                Severity = c.Severity,
                IsResolved = c.IsResolved
            }).ToList() ?? new List<RoutineConflictRecord>();
        }
    }
}
