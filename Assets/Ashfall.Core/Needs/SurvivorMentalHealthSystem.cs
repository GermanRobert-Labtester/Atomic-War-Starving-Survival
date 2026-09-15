// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Needs
{
    [Serializable]
    public sealed class SurvivorMentalHealthRecord
    {
        public string survivorId = string.Empty;
        public int stressPermille = 200;
        public List<string> activeTraumaIds = new List<string>();
        public int insomniaDaysRemaining;
        public string currentCrisisId = string.Empty;
        public int crisisDaysRemaining;
        public int therapySessionCount;
    }

    [Serializable]
    public sealed class SurvivorMentalHealthState
    {
        public string systemId = SurvivorMentalHealthSystem.SystemId;
        public Dictionary<string, SurvivorMentalHealthRecord> survivorRecords = new Dictionary<string, SurvivorMentalHealthRecord>(StringComparer.Ordinal);
        public int totalCatharsisBreakthroughs;
    }

    public sealed class SurvivorMentalHealthSystem
    {
        public const string SystemId = "survivor_mental_health";

        private readonly Dictionary<string, TraumaTypeDefinition> _traumas =
            new Dictionary<string, TraumaTypeDefinition>(StringComparer.Ordinal);
        private readonly Dictionary<string, RecoveryActionDefinition> _therapies =
            new Dictionary<string, RecoveryActionDefinition>(StringComparer.Ordinal);
        private readonly Dictionary<string, CrisisEventDefinition> _crises =
            new Dictionary<string, CrisisEventDefinition>(StringComparer.Ordinal);

        private readonly ISeededRng _rng;
        private SurvivorMentalHealthState _state = new SurvivorMentalHealthState();

        public SurvivorMentalHealthSystem(PsychologicalTraumaCatalog? catalog = null, ISeededRng? rng = null)
        {
            _rng = rng ?? new SeededRng(5252);
            if (catalog != null)
            {
                LoadCatalog(catalog);
            }
        }

        public void LoadCatalog(PsychologicalTraumaCatalog catalog)
        {
            if (catalog == null) throw new ArgumentNullException(nameof(catalog));

            _traumas.Clear();
            foreach (var t in catalog.trauma_types)
            {
                if (!string.IsNullOrEmpty(t.id))
                    _traumas[t.id] = t;
            }

            _therapies.Clear();
            foreach (var r in catalog.recovery_actions)
            {
                if (!string.IsNullOrEmpty(r.id))
                    _therapies[r.id] = r;
            }

            _crises.Clear();
            foreach (var c in catalog.crisis_events)
            {
                if (!string.IsNullOrEmpty(c.id))
                    _crises[c.id] = c;
            }
        }

        public IReadOnlyDictionary<string, TraumaTypeDefinition> Traumas => _traumas;
        public IReadOnlyDictionary<string, RecoveryActionDefinition> Therapies => _therapies;
        public IReadOnlyDictionary<string, CrisisEventDefinition> Crises => _crises;

        public SurvivorMentalHealthRecord GetOrCreateRecord(string survivorId, int initialStress = 200)
        {
            if (string.IsNullOrEmpty(survivorId))
                throw new ArgumentException("Survivor ID cannot be null or empty.", nameof(survivorId));

            if (!_state.survivorRecords.TryGetValue(survivorId, out var rec))
            {
                rec = new SurvivorMentalHealthRecord
                {
                    survivorId = survivorId,
                    stressPermille = Math.Clamp(initialStress, 0, 1000)
                };
                _state.survivorRecords[survivorId] = rec;
            }
            return rec;
        }

        public bool HasRecord(string survivorId) =>
            !string.IsNullOrEmpty(survivorId) && _state.survivorRecords.ContainsKey(survivorId);

        public int GetStressFloor(string survivorId)
        {
            if (!_state.survivorRecords.TryGetValue(survivorId, out var rec)) return 0;
            int floor = 0;
            foreach (var tid in rec.activeTraumaIds)
            {
                if (_traumas.TryGetValue(tid, out var tdef))
                    floor += tdef.stress_floor_permille;
            }
            return Math.Clamp(floor, 0, 750);
        }

        public void AddStress(string survivorId, int permilleDelta, string triggerTag = "")
        {
            if (string.IsNullOrEmpty(survivorId) || permilleDelta <= 0) return;
            var rec = GetOrCreateRecord(survivorId);
            rec.stressPermille = Math.Clamp(rec.stressPermille + permilleDelta, 0, 1000);

            // Check if acute crisis triggers
            if (rec.stressPermille >= 750 && string.IsNullOrEmpty(rec.currentCrisisId))
            {
                TriggerPotentialCrisis(rec, triggerTag);
            }
        }

        public void ReduceStress(string survivorId, int permilleDelta)
        {
            if (string.IsNullOrEmpty(survivorId) || permilleDelta <= 0) return;
            var rec = GetOrCreateRecord(survivorId);
            int floor = GetStressFloor(survivorId);
            rec.stressPermille = Math.Clamp(rec.stressPermille - permilleDelta, floor, 1000);
        }

        public bool InflictTrauma(string survivorId, string traumaId, out string reason)
        {
            reason = string.Empty;
            if (!_traumas.TryGetValue(traumaId, out var def))
            {
                reason = $"Trauma '{traumaId}' not found in catalog.";
                return false;
            }

            var rec = GetOrCreateRecord(survivorId);
            if (rec.activeTraumaIds.Contains(traumaId))
            {
                reason = $"Survivor already suffers from '{traumaId}'.";
                return false;
            }

            rec.activeTraumaIds.Add(traumaId);
            AddStress(survivorId, def.stress_floor_permille);
            return true;
        }

        public bool ResolveTrauma(string survivorId, string traumaId, out string reason)
        {
            reason = string.Empty;
            if (!_state.survivorRecords.TryGetValue(survivorId, out var rec))
            {
                reason = $"Survivor '{survivorId}' has no mental health record.";
                return false;
            }

            if (!rec.activeTraumaIds.Remove(traumaId))
            {
                reason = $"Survivor '{survivorId}' does not have active trauma '{traumaId}'.";
                return false;
            }

            _state.totalCatharsisBreakthroughs++;
            ReduceStress(survivorId, 150);
            return true;
        }

        public bool PrescribeTherapy(string survivorId, string recoveryActionId, bool hasCounselor, out string outcome)
        {
            outcome = string.Empty;
            if (!_therapies.TryGetValue(recoveryActionId, out var action))
            {
                outcome = $"Recovery action '{recoveryActionId}' not found in catalog.";
                return false;
            }

            var rec = GetOrCreateRecord(survivorId);
            rec.therapySessionCount++;

            int reduction = action.stress_reduction_permille;
            if (hasCounselor)
                reduction += action.counselor_bonus_permille;

            ReduceStress(survivorId, reduction);

            // Roll catharsis resolution for one active trauma
            if (rec.activeTraumaIds.Count > 0)
            {
                int chance = action.daily_resolution_chance_permille;
                if (hasCounselor)
                    chance += action.counselor_bonus_permille * 2;

                if (_rng.Next(0, 1000) < chance)
                {
                    string targetTrauma = rec.activeTraumaIds[_rng.Next(0, rec.activeTraumaIds.Count)];
                    ResolveTrauma(survivorId, targetTrauma, out _);
                    outcome = $"Therapy was a breakthrough! {survivorId} successfully processed and resolved '{targetTrauma}'.";
                    return true;
                }
            }

            outcome = $"{survivorId} completed {action.display_name}. Stress reduced by {reduction / 10f}%.";
            return true;
        }

        private void TriggerPotentialCrisis(SurvivorMentalHealthRecord rec, string triggerTag)
        {
            // Look for matching trauma affinity first
            string chosenCrisis = string.Empty;
            foreach (var tid in rec.activeTraumaIds)
            {
                if (_traumas.TryGetValue(tid, out var tdef) && !string.IsNullOrEmpty(tdef.crisis_affinity))
                {
                    if (_crises.ContainsKey(tdef.crisis_affinity))
                    {
                        chosenCrisis = tdef.crisis_affinity;
                        break;
                    }
                }
            }

            if (string.IsNullOrEmpty(chosenCrisis))
            {
                // Fallback to withdrawal or panic
                chosenCrisis = _crises.ContainsKey("crisis_withdrawal") ? "crisis_withdrawal" : "crisis_panic";
            }

            if (_crises.TryGetValue(chosenCrisis, out var cdef) && rec.stressPermille >= cdef.threshold_stress_permille)
            {
                rec.currentCrisisId = chosenCrisis;
                rec.crisisDaysRemaining = Math.Max(1, cdef.duration_days);
            }
        }

        public void TickDay(int currentDay)
        {
            foreach (var kvp in _state.survivorRecords)
            {
                var rec = kvp.Value;

                // 1. Tick down crisis
                if (!string.IsNullOrEmpty(rec.currentCrisisId))
                {
                    rec.crisisDaysRemaining--;
                    if (rec.crisisDaysRemaining <= 0)
                    {
                        rec.currentCrisisId = string.Empty;
                        // Coming out of crisis gives slight stress relief
                        ReduceStress(rec.survivorId, 100);
                    }
                }

                // 2. Tick down insomnia
                if (rec.insomniaDaysRemaining > 0)
                {
                    rec.insomniaDaysRemaining--;
                }

                // 3. Roll insomnia from active traumas
                foreach (var tid in rec.activeTraumaIds)
                {
                    if (_traumas.TryGetValue(tid, out var tdef) && tdef.insomnia_chance_permille > 0)
                    {
                        if (_rng.Next(0, 1000) < tdef.insomnia_chance_permille)
                        {
                            rec.insomniaDaysRemaining = Math.Max(rec.insomniaDaysRemaining, 1);
                        }
                    }
                }
            }
        }

        public int GetProductivityPenaltyPermille(string survivorId)
        {
            if (!_state.survivorRecords.TryGetValue(survivorId, out var rec)) return 0;
            int penalty = 0;

            if (!string.IsNullOrEmpty(rec.currentCrisisId) && _crises.TryGetValue(rec.currentCrisisId, out var cdef))
            {
                penalty += cdef.productivity_penalty_permille;
            }

            if (rec.insomniaDaysRemaining > 0)
            {
                penalty += 200;
            }

            return Math.Clamp(penalty, 0, 1000);
        }

        public SurvivorMentalHealthState CaptureState()
        {
            var s = new SystemTextJsonSerializer();
            var json = s.Serialize(_state);
            return s.Deserialize<SurvivorMentalHealthState>(json) ?? new SurvivorMentalHealthState();
        }

        public void RestoreState(SurvivorMentalHealthState? state)
        {
            if (state == null)
            {
                _state = new SurvivorMentalHealthState();
                return;
            }
            var s = new SystemTextJsonSerializer();
            var json = s.Serialize(state);
            _state = s.Deserialize<SurvivorMentalHealthState>(json) ?? new SurvivorMentalHealthState();
        }
    }
}
