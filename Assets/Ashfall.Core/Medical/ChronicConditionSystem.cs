// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 193 — Chronic Conditions & Disabilities System
// Pure domain authority for tracking long-term physical and sensory impairments,
// calculating capability modifiers, managing assistive accommodations,
// and measuring survivor functional capacity.
// ============================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Ashfall.Core.Medical
{
    // ── Catalog DTOs ────────────────────────────────────────────────────────

    [Serializable]
    public sealed class ChronicConditionDef
    {
        public string condition_id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public string condition_type { get; set; } = "mobility"; // mobility, vision, respiratory, neurological, hearing, physical
        public string severity { get; set; } = "mild";           // mild, moderate, severe
        public string cause { get; set; } = "injury";            // injury, radiation, environmental, chemical, age
        public bool is_permanent { get; set; } = true;
        public Dictionary<string, float> capability_penalties { get; set; } = new Dictionary<string, float>(StringComparer.OrdinalIgnoreCase);
        public string recommended_accommodation_id { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class AccommodationDef
    {
        public string accommodation_id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public string accommodation_type { get; set; } = "physical"; // physical, sensory, medical, communication
        public float effective_bonus { get; set; } = 0.15f;
        public List<string> maintenance_cost_items { get; set; } = new List<string>();
        public string description { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class ChronicConditionsCatalog
    {
        public int schema_version { get; set; } = 1;
        public List<ChronicConditionDef> conditions { get; set; } = new List<ChronicConditionDef>();
        public List<AccommodationDef> accommodations { get; set; } = new List<AccommodationDef>();
    }

    // ── Persistent State DTOs ───────────────────────────────────────────────

    [Serializable]
    public sealed class SurvivorConditionRecord
    {
        public string ConditionId { get; set; } = string.Empty;
        public string SurvivorId { get; set; } = string.Empty;
        public int OnsetDay { get; set; } = 1;
        public string Cause { get; set; } = "injury";
        public string Severity { get; set; } = "mild";
        public bool IsTreated { get; set; }
    }

    [Serializable]
    public sealed class SurvivorAccommodationRecord
    {
        public string AccommodationId { get; set; } = string.Empty;
        public string SurvivorId { get; set; } = string.Empty;
        public int InstalledDay { get; set; } = 1;
        public string ConditionRefId { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }

    [Serializable]
    public sealed class ChronicConditionState
    {
        public int SchemaVersion { get; set; } = 1;
        public int NextSequence { get; set; } = 1;
        public List<SurvivorConditionRecord> Conditions { get; set; } = new List<SurvivorConditionRecord>();
        public List<SurvivorAccommodationRecord> Accommodations { get; set; } = new List<SurvivorAccommodationRecord>();
    }

    // ── Domain System ───────────────────────────────────────────────────────

    public sealed class ChronicConditionSystem
    {
        private readonly ChronicConditionState _state;
        private readonly Dictionary<string, ChronicConditionDef> _conditionDefs =
            new Dictionary<string, ChronicConditionDef>(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, AccommodationDef> _accommodationDefs =
            new Dictionary<string, AccommodationDef>(StringComparer.OrdinalIgnoreCase);

        public event Action<SurvivorConditionRecord>? OnConditionAdded;
        public event Action<SurvivorAccommodationRecord>? OnAccommodationAssigned;
        public event Action<string, string>? OnAccommodationRemoved; // (survivorId, accommodationId)

        public int TrackedConditionCount => _state.Conditions.Count;
        public int ActiveAccommodationCount => _state.Accommodations.Count(a => a.IsActive);

        public ChronicConditionSystem()
        {
            _state = new ChronicConditionState();
        }

        public ChronicConditionSystem(ChronicConditionState state)
        {
            _state = state ?? new ChronicConditionState();
        }

        // ── Catalog Loading ────────────────────────────────────────────────

        public void LoadCatalog(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return;
            try
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var catalog = JsonSerializer.Deserialize<ChronicConditionsCatalog>(json, options);
                if (catalog == null) return;

                if (catalog.conditions != null)
                {
                    _conditionDefs.Clear();
                    foreach (var c in catalog.conditions)
                    {
                        if (string.IsNullOrWhiteSpace(c.condition_id)) continue;
                        _conditionDefs[c.condition_id] = c;
                    }
                }

                if (catalog.accommodations != null)
                {
                    _accommodationDefs.Clear();
                    foreach (var a in catalog.accommodations)
                    {
                        if (string.IsNullOrWhiteSpace(a.accommodation_id)) continue;
                        _accommodationDefs[a.accommodation_id] = a;
                    }
                }
            }
            catch (Exception) { /* malformed catalog falls back to built-in defaults; authoring errors are enforced by the data-integrity gate */ }
        }

        public IReadOnlyCollection<ChronicConditionDef> GetAllConditionDefs() => _conditionDefs.Values;
        public IReadOnlyCollection<AccommodationDef> GetAllAccommodationDefs() => _accommodationDefs.Values;

        public ChronicConditionDef? GetConditionDef(string id)
        {
            return _conditionDefs.TryGetValue(id, out var def) ? def : null;
        }

        public AccommodationDef? GetAccommodationDef(string id)
        {
            return _accommodationDefs.TryGetValue(id, out var def) ? def : null;
        }

        // ── Condition Management ───────────────────────────────────────────

        public SurvivorConditionRecord? AddCondition(string survivorId, string conditionId, int day, string cause = "injury")
        {
            if (string.IsNullOrWhiteSpace(survivorId) || string.IsNullOrWhiteSpace(conditionId)) return null;

            var existing = _state.Conditions.FirstOrDefault(c =>
                string.Equals(c.SurvivorId, survivorId, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(c.ConditionId, conditionId, StringComparison.OrdinalIgnoreCase));

            if (existing != null) return existing;

            var def = GetConditionDef(conditionId);
            var record = new SurvivorConditionRecord
            {
                ConditionId = conditionId,
                SurvivorId = survivorId,
                OnsetDay = day,
                Cause = !string.IsNullOrWhiteSpace(cause) ? cause : (def?.cause ?? "injury"),
                Severity = def?.severity ?? "mild",
                IsTreated = false
            };

            _state.Conditions.Add(record);
            OnConditionAdded?.Invoke(record);
            return record;
        }

        public IReadOnlyList<SurvivorConditionRecord> GetSurvivorConditions(string survivorId)
        {
            return _state.Conditions
                .Where(c => string.Equals(c.SurvivorId, survivorId, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        // ── Accommodation Management ───────────────────────────────────────

        public SurvivorAccommodationRecord? AssignAccommodation(string survivorId, string accommodationId, string conditionId, int day)
        {
            if (string.IsNullOrWhiteSpace(survivorId) || string.IsNullOrWhiteSpace(accommodationId)) return null;

            var record = _state.Accommodations.FirstOrDefault(a =>
                string.Equals(a.SurvivorId, survivorId, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(a.AccommodationId, accommodationId, StringComparison.OrdinalIgnoreCase));

            if (record == null)
            {
                record = new SurvivorAccommodationRecord
                {
                    AccommodationId = accommodationId,
                    SurvivorId = survivorId,
                    InstalledDay = day,
                    ConditionRefId = conditionId,
                    IsActive = true
                };
                _state.Accommodations.Add(record);
            }
            else
            {
                record.IsActive = true;
                record.ConditionRefId = conditionId;
            }

            OnAccommodationAssigned?.Invoke(record);
            return record;
        }

        public bool RemoveAccommodation(string survivorId, string accommodationId)
        {
            var record = _state.Accommodations.FirstOrDefault(a =>
                string.Equals(a.SurvivorId, survivorId, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(a.AccommodationId, accommodationId, StringComparison.OrdinalIgnoreCase));

            if (record == null || !record.IsActive) return false;

            record.IsActive = false;
            OnAccommodationRemoved?.Invoke(survivorId, accommodationId);
            return true;
        }

        public IReadOnlyList<SurvivorAccommodationRecord> GetSurvivorAccommodations(string survivorId)
        {
            return _state.Accommodations
                .Where(a => string.Equals(a.SurvivorId, survivorId, StringComparison.OrdinalIgnoreCase) && a.IsActive)
                .ToList();
        }

        // ── Capability & Impairment Calculation ─────────────────────────────

        public float CalculateCapabilityModifier(string survivorId, string capabilityName)
        {
            var conditions = GetSurvivorConditions(survivorId);
            if (conditions.Count == 0) return 1.0f;

            var accommodations = GetSurvivorAccommodations(survivorId);
            float totalPenalty = 0.0f;

            foreach (var condRecord in conditions)
            {
                if (!_conditionDefs.TryGetValue(condRecord.ConditionId, out var def)) continue;
                if (!def.capability_penalties.TryGetValue(capabilityName, out var basePenalty)) continue;

                float netPenalty = basePenalty;

                // Check if survivor has the recommended accommodation active
                if (!string.IsNullOrWhiteSpace(def.recommended_accommodation_id))
                {
                    var accom = accommodations.FirstOrDefault(a =>
                        string.Equals(a.AccommodationId, def.recommended_accommodation_id, StringComparison.OrdinalIgnoreCase));

                    if (accom != null && _accommodationDefs.TryGetValue(accom.AccommodationId, out var aDef))
                    {
                        netPenalty = Math.Max(0.0f, basePenalty - aDef.effective_bonus);
                    }
                }

                totalPenalty += netPenalty;
            }

            return (float)Math.Round(Math.Clamp(1.0f - totalPenalty, 0.10f, 1.0f), 3);
        }

        public float GetTotalImpairmentScore(string survivorId)
        {
            var standardCapabilities = new[]
            {
                "movement_speed", "work_speed", "combat_effectiveness",
                "crafting_quality", "learning_rate", "social_interaction"
            };

            float sumLoss = standardCapabilities.Sum(cap => 1.0f - CalculateCapabilityModifier(survivorId, cap));
            float averageImpairment = (sumLoss / standardCapabilities.Length) * 100.0f;
            return (float)Math.Round(averageImpairment, 1);
        }

        // ── Save / Restore ─────────────────────────────────────────────────

        public ChronicConditionState CaptureState()
        {
            return new ChronicConditionState
            {
                SchemaVersion = _state.SchemaVersion,
                NextSequence = _state.NextSequence,
                Conditions = _state.Conditions.Select(c => new SurvivorConditionRecord
                {
                    ConditionId = c.ConditionId,
                    SurvivorId = c.SurvivorId,
                    OnsetDay = c.OnsetDay,
                    Cause = c.Cause,
                    Severity = c.Severity,
                    IsTreated = c.IsTreated
                }).ToList(),
                Accommodations = _state.Accommodations.Select(a => new SurvivorAccommodationRecord
                {
                    AccommodationId = a.AccommodationId,
                    SurvivorId = a.SurvivorId,
                    InstalledDay = a.InstalledDay,
                    ConditionRefId = a.ConditionRefId,
                    IsActive = a.IsActive
                }).ToList()
            };
        }

        public void RestoreState(ChronicConditionState? saved)
        {
            if (saved == null) return;
            _state.SchemaVersion = saved.SchemaVersion;
            _state.NextSequence = saved.NextSequence > 0 ? saved.NextSequence : 1;

            _state.Conditions = saved.Conditions?.Select(c => new SurvivorConditionRecord
            {
                ConditionId = c.ConditionId,
                SurvivorId = c.SurvivorId,
                OnsetDay = c.OnsetDay,
                Cause = c.Cause,
                Severity = c.Severity,
                IsTreated = c.IsTreated
            }).ToList() ?? new List<SurvivorConditionRecord>();

            _state.Accommodations = saved.Accommodations?.Select(a => new SurvivorAccommodationRecord
            {
                AccommodationId = a.AccommodationId,
                SurvivorId = a.SurvivorId,
                InstalledDay = a.InstalledDay,
                ConditionRefId = a.ConditionRefId,
                IsActive = a.IsActive
            }).ToList() ?? new List<SurvivorAccommodationRecord>();
        }
    }
}
