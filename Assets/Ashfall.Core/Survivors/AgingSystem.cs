// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 176 — Aging & Elderly Survivor System
// Pure domain authority for survivor aging progression, life stage transitions,
// retirement management, elder mentorship detection, and birthday/age milestones.
// Leverages SurvivorAgingProgressionEngine for pure mathematical evaluation.
// ============================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Ashfall.Core.Survivors
{
    // ── Catalog DTOs ────────────────────────────────────────────────────────

    [Serializable]
    public sealed class LifeStageDef
    {
        public string stage_id { get; set; } = string.Empty;
        public string stage_enum { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public int min_age { get; set; }
        public int max_age { get; set; }
        public float physical_labor_multiplier { get; set; } = 1.0f;
        public float fatigue_accumulation_multiplier { get; set; } = 1.0f;
        public float mentorship_xp_bonus { get; set; }
        public bool can_retire { get; set; }
        public string description { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class AgingMilestoneDef
    {
        public int age { get; set; }
        public string label { get; set; } = string.Empty;
        public int morale_bonus { get; set; }
    }

    [Serializable]
    public sealed class LifeStagesCatalog
    {
        public int schema_version { get; set; } = 1;
        public int days_per_year { get; set; } = 30;
        public int min_retirement_age_years { get; set; } = 65;
        public List<LifeStageDef> life_stages { get; set; } = new List<LifeStageDef>();
        public List<AgingMilestoneDef> milestones { get; set; } = new List<AgingMilestoneDef>();
    }

    // ── Persistent State ────────────────────────────────────────────────────

    [Serializable]
    public sealed class SurvivorAgingRecord
    {
        public string SurvivorId { get; set; } = string.Empty;
        public int BaseAgeYears { get; set; } = 28;
        public int JoinedDay { get; set; } = 1;
        public bool IsRetired { get; set; }
        public int RetiredDay { get; set; } = -1;
        public int LastEvaluatedAgeYears { get; set; } = 28;
        public int LastEvaluatedStage { get; set; } = (int)SurvivorLifeStage.YoungAdult;
        public List<int> CelebratedMilestoneAges { get; set; } = new List<int>();
    }

    [Serializable]
    public sealed class AgingState
    {
        public int SchemaVersion { get; set; } = 1;
        public int DaysPerYear { get; set; } = 30;
        public int MinRetirementAgeYears { get; set; } = 65;
        public List<SurvivorAgingRecord> Records { get; set; } = new List<SurvivorAgingRecord>();
    }

    // ── Domain System ───────────────────────────────────────────────────────

    public sealed class AgingSystem
    {
        private readonly AgingState _state;
        private readonly List<LifeStageDef> _stages = new List<LifeStageDef>();
        private readonly List<AgingMilestoneDef> _milestones = new List<AgingMilestoneDef>();

        public event Action<string, int>? OnSurvivorRetired; // (survivorId, retiredDay)
        public event Action<string, int, string>? OnMilestoneReached; // (survivorId, age, label)
        public event Action<string, SurvivorLifeStage, SurvivorLifeStage>? OnStageTransitioned; // (survivorId, old, new)

        public int TrackedSurvivorCount => _state.Records.Count;
        public int RetiredSurvivorCount => _state.Records.Count(r => r.IsRetired);
        public int DaysPerYear => _state.DaysPerYear;
        public int MinRetirementAgeYears => _state.MinRetirementAgeYears;

        public AgingSystem()
        {
            _state = new AgingState();
        }

        public AgingSystem(AgingState state)
        {
            _state = state ?? new AgingState();
        }

        // ── Catalog Loading ────────────────────────────────────────────────

        public void LoadCatalog(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return;
            try
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var catalog = JsonSerializer.Deserialize<LifeStagesCatalog>(json, options);
                if (catalog == null) return;

                if (catalog.days_per_year > 0)
                    _state.DaysPerYear = catalog.days_per_year;
                if (catalog.min_retirement_age_years > 0)
                    _state.MinRetirementAgeYears = catalog.min_retirement_age_years;

                _stages.Clear();
                if (catalog.life_stages != null)
                {
                    foreach (var s in catalog.life_stages)
                        if (!string.IsNullOrWhiteSpace(s.stage_id))
                            _stages.Add(s);
                }

                _milestones.Clear();
                if (catalog.milestones != null)
                {
                    foreach (var m in catalog.milestones)
                        _milestones.Add(m);
                }
            }
            catch (Exception) { /* malformed catalog falls back to built-in defaults; authoring errors are enforced by the data-integrity gate */ }
        }

        public IReadOnlyList<LifeStageDef> GetAllLifeStages() => _stages;
        public IReadOnlyList<AgingMilestoneDef> GetAllMilestones() => _milestones;

        // ── Survivor Registration & Evaluation ─────────────────────────────

        public SurvivorAgingRecord RegisterSurvivor(string survivorId, int baseAgeYears, int joinedDay)
        {
            if (string.IsNullOrWhiteSpace(survivorId)) throw new ArgumentException("survivorId is required");

            var existing = _state.Records.FirstOrDefault(r =>
                string.Equals(r.SurvivorId, survivorId, StringComparison.OrdinalIgnoreCase));
            if (existing != null)
                return existing;

            int initialAge = Math.Max(0, baseAgeYears);
            var record = new SurvivorAgingRecord
            {
                SurvivorId = survivorId,
                BaseAgeYears = initialAge,
                JoinedDay = Math.Max(1, joinedDay),
                LastEvaluatedAgeYears = initialAge,
                LastEvaluatedStage = (int)SurvivorAgingProgressionEngine.EvaluateStage(initialAge)
            };

            _state.Records.Add(record);
            return record;
        }

        public SurvivorAgeProfile EvaluateSurvivor(string survivorId, int currentDay)
        {
            var record = _state.Records.FirstOrDefault(r =>
                string.Equals(r.SurvivorId, survivorId, StringComparison.OrdinalIgnoreCase));

            int baseAge = record?.BaseAgeYears ?? SurvivorAgingProgressionEngine.DefaultRecruitmentAgeYears;
            int joinedDay = record?.JoinedDay ?? 1;
            bool isRetired = record?.IsRetired ?? false;

            return SurvivorAgingProgressionEngine.CalculateProfile(
                survivorId,
                joinedDay,
                currentDay,
                baseAge,
                isRetired,
                _state.DaysPerYear);
        }

        public bool IsRetired(string survivorId)
        {
            var record = _state.Records.FirstOrDefault(r =>
                string.Equals(r.SurvivorId, survivorId, StringComparison.OrdinalIgnoreCase));
            return record?.IsRetired ?? false;
        }

        public bool RetireSurvivor(string survivorId, int currentDay)
        {
            var record = _state.Records.FirstOrDefault(r =>
                string.Equals(r.SurvivorId, survivorId, StringComparison.OrdinalIgnoreCase));
            if (record == null || record.IsRetired)
                return false;

            int age = SurvivorAgingProgressionEngine.EvaluateAgeYears(
                record.JoinedDay, currentDay, record.BaseAgeYears, _state.DaysPerYear);

            if (age < _state.MinRetirementAgeYears)
                return false; // not eligible

            record.IsRetired = true;
            record.RetiredDay = currentDay;
            OnSurvivorRetired?.Invoke(survivorId, currentDay);
            return true;
        }

        // ── Daily Tick & Milestone Processing ──────────────────────────────

        public void TickDay(int currentDay, IEnumerable<string>? livingSurvivorIds = null)
        {
            var survivorList = livingSurvivorIds != null
                ? new HashSet<string>(livingSurvivorIds, StringComparer.OrdinalIgnoreCase)
                : null;

            foreach (var record in _state.Records)
            {
                if (survivorList != null && !survivorList.Contains(record.SurvivorId))
                    continue;

                int newAge = SurvivorAgingProgressionEngine.EvaluateAgeYears(
                    record.JoinedDay, currentDay, record.BaseAgeYears, _state.DaysPerYear);
                var newStage = SurvivorAgingProgressionEngine.EvaluateStage(newAge);

                // Stage transition check
                if ((int)newStage != record.LastEvaluatedStage)
                {
                    var oldStage = (SurvivorLifeStage)record.LastEvaluatedStage;
                    record.LastEvaluatedStage = (int)newStage;
                    OnStageTransitioned?.Invoke(record.SurvivorId, oldStage, newStage);
                }

                // Milestone / birthday check
                if (newAge > record.LastEvaluatedAgeYears)
                {
                    foreach (var m in _milestones)
                    {
                        if (newAge >= m.age && !record.CelebratedMilestoneAges.Contains(m.age))
                        {
                            record.CelebratedMilestoneAges.Add(m.age);
                            OnMilestoneReached?.Invoke(record.SurvivorId, m.age, m.label);
                        }
                    }
                }

                record.LastEvaluatedAgeYears = newAge;
            }
        }

        public bool HasLivingElderMentor(int currentDay, IEnumerable<string> livingSurvivorIds)
        {
            if (livingSurvivorIds == null) return false;
            foreach (var id in livingSurvivorIds)
            {
                var profile = EvaluateSurvivor(id, currentDay);
                if (profile.Stage == SurvivorLifeStage.Elderly)
                    return true;
            }
            return false;
        }

        // ── Save / Restore ─────────────────────────────────────────────────

        public AgingState CaptureState()
        {
            return new AgingState
            {
                SchemaVersion = _state.SchemaVersion,
                DaysPerYear = _state.DaysPerYear,
                MinRetirementAgeYears = _state.MinRetirementAgeYears,
                Records = _state.Records.Select(r => new SurvivorAgingRecord
                {
                    SurvivorId = r.SurvivorId,
                    BaseAgeYears = r.BaseAgeYears,
                    JoinedDay = r.JoinedDay,
                    IsRetired = r.IsRetired,
                    RetiredDay = r.RetiredDay,
                    LastEvaluatedAgeYears = r.LastEvaluatedAgeYears,
                    LastEvaluatedStage = r.LastEvaluatedStage,
                    CelebratedMilestoneAges = new List<int>(r.CelebratedMilestoneAges)
                }).ToList()
            };
        }

        public void RestoreState(AgingState? saved)
        {
            if (saved == null) return;
            _state.SchemaVersion = saved.SchemaVersion;
            _state.DaysPerYear = saved.DaysPerYear > 0 ? saved.DaysPerYear : 30;
            _state.MinRetirementAgeYears = saved.MinRetirementAgeYears > 0 ? saved.MinRetirementAgeYears : 65;
            _state.Records = saved.Records?.Select(r => new SurvivorAgingRecord
            {
                SurvivorId = r.SurvivorId,
                BaseAgeYears = r.BaseAgeYears,
                JoinedDay = r.JoinedDay,
                IsRetired = r.IsRetired,
                RetiredDay = r.RetiredDay,
                LastEvaluatedAgeYears = r.LastEvaluatedAgeYears,
                LastEvaluatedStage = r.LastEvaluatedStage,
                CelebratedMilestoneAges = new List<int>(r.CelebratedMilestoneAges ?? new List<int>())
            }).ToList() ?? new List<SurvivorAgingRecord>();
        }
    }
}
