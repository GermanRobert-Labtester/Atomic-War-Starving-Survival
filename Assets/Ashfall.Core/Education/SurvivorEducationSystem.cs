// SPDX-License-Identifier: MIT
// ============================================================================
// Ashfall Core : Plan 154 — Survivor Education & Knowledge Transfer
// Subsystem    : EducationSystem / Generational Knowledge & Vocational Training
// Authority    : Next-steps-plans/Plan_154_Survivor_Education_Knowledge_Transfer.md
//                UNBLOCK-PROGRAM-WAVE32-BATCH5-PLANS (DEC-151)
// ============================================================================
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Education
{
    public enum EducationStageType
    {
        Childhood     = 0, // 0..12 years
        Adolescence   = 1, // 12..16 years
        YoungAdult    = 2, // 16..18 years
        AdultGraduate = 3  // 18+ years
    }

    public sealed class EducationStageDef
    {
        [JsonPropertyName("stage_id")]
        public string StageId { get; set; } = string.Empty;

        [JsonPropertyName("stage_name")]
        public string StageName { get; set; } = string.Empty;

        [JsonPropertyName("min_age")]
        public int MinAge { get; set; }

        [JsonPropertyName("max_age")]
        public int MaxAge { get; set; }

        [JsonPropertyName("learning_capacity")]
        public int LearningCapacity { get; set; } = 100;

        [JsonPropertyName("available_subjects")]
        public List<string> AvailableSubjects { get; set; } = new();
    }

    public sealed class CurriculumSubjectDef
    {
        [JsonPropertyName("subject_id")]
        public string SubjectId { get; set; } = string.Empty;

        [JsonPropertyName("subject_name")]
        public string SubjectName { get; set; } = string.Empty;

        [JsonPropertyName("stage_id")]
        public string StageId { get; set; } = string.Empty;

        [JsonPropertyName("prerequisites")]
        public List<string> Prerequisites { get; set; } = new();

        [JsonPropertyName("unlocked_skill_id")]
        public string UnlockedSkillId { get; set; } = string.Empty;

        [JsonPropertyName("proficiency_required")]
        public int ProficiencyRequired { get; set; } = 80;

        [JsonPropertyName("teaching_speed_modifier")]
        public double TeachingSpeedModifier { get; set; } = 1.0;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;
    }

    public sealed class SurvivorEducationRecord
    {
        public string SurvivorId { get; set; } = string.Empty;
        public int Age { get; set; }
        public EducationStageType CurrentStage { get; set; } = EducationStageType.Childhood;
        public Dictionary<string, int> SubjectsStudied { get; set; } = new();
        public List<string> Teachers { get; set; } = new();
        public string ActiveSubjectId { get; set; } = string.Empty;
        public string AssignedTeacherId { get; set; } = string.Empty;
        public bool IsParentTeacher { get; set; }
        public int EducationQuality { get; set; } = 100;
        public int GraduationDay { get; set; } = -1;
        public bool IsGraduated { get; set; }
        public List<string> UnlockedSkillIds { get; set; } = new();

        public SurvivorEducationRecord Clone()
        {
            var record = new SurvivorEducationRecord
            {
                SurvivorId = SurvivorId,
                Age = Age,
                CurrentStage = CurrentStage,
                ActiveSubjectId = ActiveSubjectId,
                AssignedTeacherId = AssignedTeacherId,
                IsParentTeacher = IsParentTeacher,
                EducationQuality = EducationQuality,
                GraduationDay = GraduationDay,
                IsGraduated = IsGraduated,
                Teachers = new List<string>(Teachers),
                UnlockedSkillIds = new List<string>(UnlockedSkillIds)
            };
            foreach (var kv in SubjectsStudied)
            {
                record.SubjectsStudied[kv.Key] = kv.Value;
            }
            return record;
        }
    }

    public readonly struct EducationSessionResult
    {
        public string StudentId { get; }
        public string TeacherId { get; }
        public string SubjectId { get; }
        public int ProficiencyGained { get; }
        public int NewProficiency { get; }
        public bool SubjectCompleted { get; }
        public string UnlockedSkillId { get; }
        public bool Graduated { get; }

        public EducationSessionResult(
            string studentId,
            string teacherId,
            string subjectId,
            int proficiencyGained,
            int newProficiency,
            bool subjectCompleted,
            string unlockedSkillId,
            bool graduated)
        {
            StudentId = studentId;
            TeacherId = teacherId;
            SubjectId = subjectId;
            ProficiencyGained = proficiencyGained;
            NewProficiency = newProficiency;
            SubjectCompleted = subjectCompleted;
            UnlockedSkillId = unlockedSkillId;
            Graduated = graduated;
        }
    }

    public sealed class EducationSystemState
    {
        public int SchemaVersion { get; set; } = 1;
        public int ShelterKnowledgeLevel { get; set; } = 10;
        public int TotalGraduates { get; set; }
        public int TotalSessionsConducted { get; set; }
        public List<SurvivorEducationRecord> Records { get; set; } = new();
    }

    /// <summary>
    /// Pure domain authority managing formal shelter education, child development stages,
    /// vocational curriculum progression, and generational knowledge retention.
    /// Zero engine dependencies; deterministic evaluation.
    /// </summary>
    public sealed class SurvivorEducationSystem
    {
        private readonly Dictionary<string, EducationStageDef> _stages = new(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, CurriculumSubjectDef> _subjects = new(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, SurvivorEducationRecord> _records = new(StringComparer.OrdinalIgnoreCase);

        public int ShelterKnowledgeLevel { get; private set; } = 10;
        public int TotalGraduates { get; private set; }
        public int TotalSessionsConducted { get; private set; }

        // Seams for host & presentation bridges
        public Action<EducationSessionResult>? OnSessionCompletedSeam { get; set; }
        public Action<SurvivorEducationRecord>? OnSurvivorGraduatedSeam { get; set; }
        public Action<int>? OnKnowledgeLevelChangedSeam { get; set; }

        public SurvivorEducationSystem()
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

                if (root.TryGetProperty("stages", out var stagesElem) && stagesElem.ValueKind == JsonValueKind.Array)
                {
                    _stages.Clear();
                    foreach (var item in stagesElem.EnumerateArray())
                    {
                        var stageId = item.GetProperty("stage_id").GetString() ?? string.Empty;
                        var stageName = item.GetProperty("stage_name").GetString() ?? string.Empty;
                        var minAge = item.GetProperty("min_age").GetInt32();
                        var maxAge = item.GetProperty("max_age").GetInt32();
                        var learningCap = item.GetProperty("learning_capacity").GetInt32();

                        var subjects = new List<string>();
                        if (item.TryGetProperty("available_subjects", out var subjElem))
                        {
                            foreach (var s in subjElem.EnumerateArray())
                            {
                                subjects.Add(s.GetString() ?? string.Empty);
                            }
                        }

                        _stages[stageId] = new EducationStageDef
                        {
                            StageId = stageId,
                            StageName = stageName,
                            MinAge = minAge,
                            MaxAge = maxAge,
                            LearningCapacity = learningCap,
                            AvailableSubjects = subjects
                        };
                    }
                }

                if (root.TryGetProperty("subjects", out var subjectsElem) && subjectsElem.ValueKind == JsonValueKind.Array)
                {
                    _subjects.Clear();
                    foreach (var item in subjectsElem.EnumerateArray())
                    {
                        var subjectId = item.GetProperty("subject_id").GetString() ?? string.Empty;
                        var subjectName = item.GetProperty("subject_name").GetString() ?? string.Empty;
                        var stageId = item.GetProperty("stage_id").GetString() ?? string.Empty;
                        var unlockedSkill = item.TryGetProperty("unlocked_skill_id", out var sElem) ? sElem.GetString() ?? string.Empty : string.Empty;
                        var profReq = item.TryGetProperty("proficiency_required", out var pElem) ? pElem.GetInt32() : 80;
                        var speedMod = item.TryGetProperty("teaching_speed_modifier", out var spElem) ? spElem.GetDouble() : 1.0;
                        var desc = item.TryGetProperty("description", out var dElem) ? dElem.GetString() ?? string.Empty : string.Empty;

                        var prereqs = new List<string>();
                        if (item.TryGetProperty("prerequisites", out var preElem))
                        {
                            foreach (var p in preElem.EnumerateArray())
                            {
                                prereqs.Add(p.GetString() ?? string.Empty);
                            }
                        }

                        _subjects[subjectId] = new CurriculumSubjectDef
                        {
                            SubjectId = subjectId,
                            SubjectName = subjectName,
                            StageId = stageId,
                            Prerequisites = prereqs,
                            UnlockedSkillId = unlockedSkill,
                            ProficiencyRequired = profReq,
                            TeachingSpeedModifier = speedMod,
                            Description = desc
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
            _stages.Clear();
            _stages["childhood"] = new EducationStageDef
            {
                StageId = "childhood",
                StageName = "Childhood Education",
                MinAge = 0,
                MaxAge = 12,
                LearningCapacity = 80,
                AvailableSubjects = new List<string> { "literacy", "numeracy", "social_cooperation", "basic_survival" }
            };
            _stages["adolescence"] = new EducationStageDef
            {
                StageId = "adolescence",
                StageName = "Adolescent Specialization",
                MinAge = 12,
                MaxAge = 16,
                LearningCapacity = 100,
                AvailableSubjects = new List<string> { "crafting_fundamentals", "first_aid_biology", "scavenging_navigation", "combat_defense" }
            };
            _stages["young_adult"] = new EducationStageDef
            {
                StageId = "young_adult",
                StageName = "Young Adult Apprenticeship",
                MinAge = 16,
                MaxAge = 18,
                LearningCapacity = 120,
                AvailableSubjects = new List<string> { "advanced_engineering", "clinical_medicine", "tactical_command", "scientific_research" }
            };

            _subjects.Clear();
            _subjects["literacy"] = new CurriculumSubjectDef
            {
                SubjectId = "literacy",
                SubjectName = "Basic Literacy & Writing",
                StageId = "childhood",
                UnlockedSkillId = "skill_reading_comprehension",
                ProficiencyRequired = 75,
                TeachingSpeedModifier = 1.0
            };
            _subjects["numeracy"] = new CurriculumSubjectDef
            {
                SubjectId = "numeracy",
                SubjectName = "Basic Numeracy & Logic",
                StageId = "childhood",
                UnlockedSkillId = "skill_mathematical_logic",
                ProficiencyRequired = 75,
                TeachingSpeedModifier = 1.0
            };
            _subjects["crafting_fundamentals"] = new CurriculumSubjectDef
            {
                SubjectId = "crafting_fundamentals",
                SubjectName = "Mechanical Crafting & Repair",
                StageId = "adolescence",
                Prerequisites = new List<string> { "numeracy" },
                UnlockedSkillId = "skill_machining_basics",
                ProficiencyRequired = 80,
                TeachingSpeedModifier = 0.95
            };
            _subjects["advanced_engineering"] = new CurriculumSubjectDef
            {
                SubjectId = "advanced_engineering",
                SubjectName = "Power & Life Support Engineering",
                StageId = "young_adult",
                Prerequisites = new List<string> { "crafting_fundamentals" },
                UnlockedSkillId = "skill_reactor_maintenance",
                ProficiencyRequired = 85,
                TeachingSpeedModifier = 0.85
            };
        }

        public EducationStageType ResolveStageForAge(int age)
        {
            if (age < 12) return EducationStageType.Childhood;
            if (age < 16) return EducationStageType.Adolescence;
            if (age < 18) return EducationStageType.YoungAdult;
            return EducationStageType.AdultGraduate;
        }

        public SurvivorEducationRecord RegisterLearner(string survivorId, int initialAge)
        {
            if (string.IsNullOrWhiteSpace(survivorId))
                throw new ArgumentException("SurvivorId cannot be null or empty.", nameof(survivorId));

            if (_records.TryGetValue(survivorId, out var existing))
                return existing;

            var stage = ResolveStageForAge(initialAge);
            var record = new SurvivorEducationRecord
            {
                SurvivorId = survivorId,
                Age = Math.Max(0, initialAge),
                CurrentStage = stage,
                IsGraduated = (stage == EducationStageType.AdultGraduate)
            };

            _records[survivorId] = record;
            return record;
        }

        public bool UpdateAge(string survivorId, int newAge)
        {
            if (!_records.TryGetValue(survivorId, out var record))
                return false;

            record.Age = Math.Max(0, newAge);
            var newStage = ResolveStageForAge(newAge);
            if (newStage != record.CurrentStage)
            {
                record.CurrentStage = newStage;
                if (newStage == EducationStageType.AdultGraduate && !record.IsGraduated)
                {
                    EvaluateGraduation(survivorId, -1);
                }
                return true;
            }
            return false;
        }

        public bool AssignTeacherAndSubject(string studentId, string teacherId, string subjectId, bool isParentChild = false)
        {
            if (!_records.TryGetValue(studentId, out var record))
                return false;

            if (record.IsGraduated)
                return false;

            if (!_subjects.ContainsKey(subjectId))
                return false;

            record.AssignedTeacherId = teacherId ?? string.Empty;
            record.ActiveSubjectId = subjectId;
            record.IsParentTeacher = isParentChild;

            if (!string.IsNullOrEmpty(teacherId) && !record.Teachers.Contains(teacherId))
            {
                record.Teachers.Add(teacherId);
            }

            return true;
        }

        public EducationSessionResult ConductDailySession(
            string studentId,
            ISeededRng rng,
            bool hasSchoolroom = false,
            int teacherCompetencyPermille = 1000)
        {
            if (rng == null)
                throw new ArgumentNullException(nameof(rng));

            if (!_records.TryGetValue(studentId, out var record))
                return default;

            if (record.IsGraduated || string.IsNullOrEmpty(record.ActiveSubjectId))
                return default;

            if (!_subjects.TryGetValue(record.ActiveSubjectId, out var subjectDef))
                return default;

            // Check prerequisites
            foreach (var pre in subjectDef.Prerequisites)
            {
                if (!record.SubjectsStudied.TryGetValue(pre, out var preProf) || preProf < 60)
                {
                    // Unmet prerequisite: minimal progress
                    return default;
                }
            }

            TotalSessionsConducted++;

            // Calculate base gain (5 to 10 points)
            int roll = rng.Next(5, 11);
            double speedMod = subjectDef.TeachingSpeedModifier;
            if (record.IsParentTeacher)
            {
                speedMod *= 1.20; // +20% parent-child teaching bonus
            }
            if (hasSchoolroom)
            {
                speedMod *= 1.10; // +10% schoolroom facility bonus
            }

            double competencyFactor = Math.Clamp(teacherCompetencyPermille / 1000.0, 0.5, 1.5);
            int gain = Math.Max(1, (int)Math.Round(roll * speedMod * competencyFactor));

            record.SubjectsStudied.TryGetValue(subjectDef.SubjectId, out int curProf);
            int newProf = Math.Min(100, curProf + gain);
            record.SubjectsStudied[subjectDef.SubjectId] = newProf;

            bool subjectCompleted = false;
            string unlockedSkill = string.Empty;

            if (newProf >= subjectDef.ProficiencyRequired && !record.UnlockedSkillIds.Contains(subjectDef.UnlockedSkillId))
            {
                subjectCompleted = true;
                if (!string.IsNullOrEmpty(subjectDef.UnlockedSkillId))
                {
                    unlockedSkill = subjectDef.UnlockedSkillId;
                    record.UnlockedSkillIds.Add(unlockedSkill);
                }

                // Boost shelter knowledge
                AdjustShelterKnowledge(2);
            }

            bool graduated = false;
            if (record.Age >= 18)
            {
                graduated = EvaluateGraduation(studentId, -1);
            }

            var result = new EducationSessionResult(
                studentId,
                record.AssignedTeacherId,
                subjectDef.SubjectId,
                gain,
                newProf,
                subjectCompleted,
                unlockedSkill,
                graduated);

            OnSessionCompletedSeam?.Invoke(result);
            return result;
        }

        public bool EvaluateGraduation(string studentId, int currentDay)
        {
            if (!_records.TryGetValue(studentId, out var record))
                return false;

            if (record.IsGraduated)
                return false;

            record.IsGraduated = true;
            record.GraduationDay = currentDay;
            record.CurrentStage = EducationStageType.AdultGraduate;
            TotalGraduates++;

            // Bonus shelter knowledge
            AdjustShelterKnowledge(5);

            OnSurvivorGraduatedSeam?.Invoke(record);
            return true;
        }

        public void AdjustShelterKnowledge(int delta)
        {
            int old = ShelterKnowledgeLevel;
            ShelterKnowledgeLevel = Math.Clamp(ShelterKnowledgeLevel + delta, 0, 100);
            if (old != ShelterKnowledgeLevel)
            {
                OnKnowledgeLevelChangedSeam?.Invoke(ShelterKnowledgeLevel);
            }
        }

        public SurvivorEducationRecord? GetRecord(string survivorId)
        {
            if (_records.TryGetValue(survivorId, out var rec))
                return rec.Clone();
            return null;
        }

        public IReadOnlyList<SurvivorEducationRecord> GetAllLearners()
        {
            var list = new List<SurvivorEducationRecord>(_records.Count);
            foreach (var rec in _records.Values)
            {
                list.Add(rec.Clone());
            }
            return list;
        }

        public CurriculumSubjectDef? GetSubject(string subjectId)
        {
            if (_subjects.TryGetValue(subjectId, out var subj))
                return subj;
            return null;
        }

        public EducationSystemState CaptureState()
        {
            var state = new EducationSystemState
            {
                SchemaVersion = 1,
                ShelterKnowledgeLevel = ShelterKnowledgeLevel,
                TotalGraduates = TotalGraduates,
                TotalSessionsConducted = TotalSessionsConducted
            };
            foreach (var rec in _records.Values)
            {
                state.Records.Add(rec.Clone());
            }
            return state;
        }

        public void RestoreState(EducationSystemState state)
        {
            if (state == null)
                return;

            ShelterKnowledgeLevel = Math.Clamp(state.ShelterKnowledgeLevel, 0, 100);
            TotalGraduates = Math.Max(0, state.TotalGraduates);
            TotalSessionsConducted = Math.Max(0, state.TotalSessionsConducted);

            _records.Clear();
            if (state.Records != null)
            {
                foreach (var rec in state.Records)
                {
                    _records[rec.SurvivorId] = rec.Clone();
                }
            }
        }
    }
}
