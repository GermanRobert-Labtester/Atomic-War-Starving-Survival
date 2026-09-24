// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;

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
        public Dictionary<string, SurvivorMentalHealthRecord> survivorRecords =
            new Dictionary<string, SurvivorMentalHealthRecord>(SurvivorMentalHealthSystem.IdentityComparer);
        public int totalCatharsisBreakthroughs;
        public int lastTickDay = -1;
    }

    public sealed class SurvivorMentalHealthSystem
    {
        public const string SystemId = "survivor_mental_health";
        internal static readonly StringComparer IdentityComparer = StringComparer.OrdinalIgnoreCase;

        private readonly Dictionary<string, TraumaTypeDefinition> _traumas =
            new Dictionary<string, TraumaTypeDefinition>(IdentityComparer);
        private readonly Dictionary<string, RecoveryActionDefinition> _therapies =
            new Dictionary<string, RecoveryActionDefinition>(IdentityComparer);
        private readonly Dictionary<string, CrisisEventDefinition> _crises =
            new Dictionary<string, CrisisEventDefinition>(IdentityComparer);

        private readonly ISeededRng _rng;
        private SurvivorMentalHealthState _state = new SurvivorMentalHealthState();

        public SurvivorMentalHealthSystem(PsychologicalTraumaCatalog? catalog = null, ISeededRng? rng = null)
        {
            _rng = rng ?? new SeededRng(5252);
            if (catalog != null)
                LoadCatalog(catalog);
        }

        public SurvivorMentalHealthState State => CaptureState();

        public IReadOnlyDictionary<string, TraumaTypeDefinition> Traumas =>
            _traumas.ToDictionary(pair => pair.Key, pair => CloneTrauma(pair.Value), IdentityComparer);

        public IReadOnlyDictionary<string, RecoveryActionDefinition> Therapies =>
            _therapies.ToDictionary(pair => pair.Key, pair => CloneRecovery(pair.Value), IdentityComparer);

        public IReadOnlyDictionary<string, CrisisEventDefinition> Crises =>
            _crises.ToDictionary(pair => pair.Key, pair => CloneCrisis(pair.Value), IdentityComparer);

        public void LoadCatalog(PsychologicalTraumaCatalog catalog)
        {
            if (catalog == null) throw new ArgumentNullException(nameof(catalog));

            var traumas = new Dictionary<string, TraumaTypeDefinition>(IdentityComparer);
            var therapies = new Dictionary<string, RecoveryActionDefinition>(IdentityComparer);
            var crises = new Dictionary<string, CrisisEventDefinition>(IdentityComparer);

            foreach (var source in catalog.trauma_types ?? new List<TraumaTypeDefinition>())
            {
                if (source == null || string.IsNullOrWhiteSpace(source.id)) continue;
                string id = NormalizeId(source.id);
                traumas[id] = NormalizeTrauma(source, id);
            }
            foreach (var source in catalog.recovery_actions ?? new List<RecoveryActionDefinition>())
            {
                if (source == null || string.IsNullOrWhiteSpace(source.id)) continue;
                string id = NormalizeId(source.id);
                therapies[id] = NormalizeRecovery(source, id);
            }
            foreach (var source in catalog.crisis_events ?? new List<CrisisEventDefinition>())
            {
                if (source == null || string.IsNullOrWhiteSpace(source.id)) continue;
                string id = NormalizeId(source.id);
                crises[id] = NormalizeCrisis(source, id);
            }

            _traumas.Clear();
            foreach (var pair in traumas) _traumas[pair.Key] = pair.Value;
            _therapies.Clear();
            foreach (var pair in therapies) _therapies[pair.Key] = pair.Value;
            _crises.Clear();
            foreach (var pair in crises) _crises[pair.Key] = pair.Value;
            foreach (var trauma in _traumas.Values)
            {
                if (_crises.TryGetValue(trauma.crisis_affinity, out var crisis))
                    trauma.crisis_affinity = crisis.id;
            }
        }

        public SurvivorMentalHealthRecord GetOrCreateRecord(string survivorId, int initialStress = 200)
        {
            string id = NormalizeId(survivorId);
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Survivor ID cannot be null or empty.", nameof(survivorId));

            return CloneRecord(GetOrCreateLiveRecord(id, initialStress));
        }

        public bool HasRecord(string survivorId)
        {
            string id = NormalizeId(survivorId);
            return !string.IsNullOrWhiteSpace(id) && _state.survivorRecords.ContainsKey(id);
        }

        public int GetStressFloor(string survivorId)
        {
            if (!TryGetLiveRecord(survivorId, out var record)) return 0;
            long floor = 0;
            foreach (var traumaId in record.activeTraumaIds)
            {
                if (_traumas.TryGetValue(traumaId, out var definition))
                    floor = Math.Min(750L, floor + Math.Max(0, definition.stress_floor_permille));
            }
            return (int)Math.Clamp(floor, 0L, 750L);
        }

        public void AddStress(string survivorId, int permilleDelta, string triggerTag = "")
        {
            if (permilleDelta <= 0) return;
            string id = NormalizeId(survivorId);
            if (string.IsNullOrWhiteSpace(id)) return;
            var record = GetOrCreateLiveRecord(id);
            record.stressPermille = ClampPermille((long)record.stressPermille + permilleDelta);

            if (record.stressPermille >= 750 && string.IsNullOrWhiteSpace(record.currentCrisisId))
                TriggerPotentialCrisis(record, triggerTag);
        }

        public void ReduceStress(string survivorId, int permilleDelta)
        {
            if (permilleDelta <= 0) return;
            string id = NormalizeId(survivorId);
            if (string.IsNullOrWhiteSpace(id)) return;
            var record = GetOrCreateLiveRecord(id);
            int floor = GetStressFloor(id);
            record.stressPermille = (int)Math.Clamp((long)record.stressPermille - permilleDelta, floor, 1000L);
        }

        public bool InflictTrauma(string survivorId, string traumaId, out string reason)
        {
            reason = string.Empty;
            string normalizedTraumaId = NormalizeId(traumaId);
            if (!_traumas.TryGetValue(normalizedTraumaId, out var definition))
            {
                reason = $"Trauma '{traumaId}' not found in catalog.";
                return false;
            }

            var record = GetOrCreateLiveRecord(survivorId);
            if (record.activeTraumaIds.Contains(normalizedTraumaId, IdentityComparer))
            {
                reason = $"Survivor already suffers from '{normalizedTraumaId}'.";
                return false;
            }

            record.activeTraumaIds.Add(normalizedTraumaId);
            AddStress(record.survivorId, definition.stress_floor_permille);
            return true;
        }

        public bool ResolveTrauma(string survivorId, string traumaId, out string reason)
        {
            reason = string.Empty;
            if (!TryGetLiveRecord(survivorId, out var record))
            {
                reason = $"Survivor '{NormalizeId(survivorId)}' has no mental health record.";
                return false;
            }

            string normalizedTraumaId = NormalizeId(traumaId);
            int index = record.activeTraumaIds.FindIndex(id =>
                string.Equals(id, normalizedTraumaId, StringComparison.OrdinalIgnoreCase));
            if (index < 0)
            {
                reason = $"Survivor '{record.survivorId}' does not have active trauma '{traumaId}'.";
                return false;
            }

            record.activeTraumaIds.RemoveAt(index);
            SaturatingIncrement(ref _state.totalCatharsisBreakthroughs);
            ReduceStress(record.survivorId, 150);
            return true;
        }

        public bool PrescribeTherapy(string survivorId, string recoveryActionId, bool hasCounselor, out string outcome)
        {
            outcome = string.Empty;
            string normalizedActionId = NormalizeId(recoveryActionId);
            if (!_therapies.TryGetValue(normalizedActionId, out var action))
            {
                outcome = $"Recovery action '{recoveryActionId}' not found in catalog.";
                return false;
            }

            var record = GetOrCreateLiveRecord(survivorId);
            SaturatingIncrement(ref record.therapySessionCount);

            int reduction = ClampAdd(action.stress_reduction_permille,
                hasCounselor ? action.counselor_bonus_permille : 0);
            ReduceStress(record.survivorId, reduction);

            if (record.activeTraumaIds.Count > 0)
            {
                long chanceValue = (long)action.daily_resolution_chance_permille
                    + (hasCounselor ? (long)action.counselor_bonus_permille * 2L : 0L);
                int chance = (int)Math.Clamp(chanceValue, 0L, 1000L);
                if (_rng.Next(0, 1000) < chance)
                {
                    string targetTrauma = record.activeTraumaIds[_rng.Next(0, record.activeTraumaIds.Count)];
                    ResolveTrauma(record.survivorId, targetTrauma, out _);
                    outcome = $"Therapy was a breakthrough! {record.survivorId} successfully processed and resolved '{targetTrauma}'.";
                    return true;
                }
            }

            outcome = $"{record.survivorId} completed {action.display_name}. Stress reduced by {reduction / 10f}%.";
            return true;
        }

        public void TickDay(int currentDay)
        {
            if (currentDay < 0)
                throw new ArgumentOutOfRangeException(nameof(currentDay), "Campaign day cannot be negative.");
            if (currentDay <= _state.lastTickDay) return;
            _state.lastTickDay = currentDay;

            foreach (var record in _state.survivorRecords.Values.ToList())
            {
                if (record == null) continue;
                if (!string.IsNullOrWhiteSpace(record.currentCrisisId))
                {
                    record.crisisDaysRemaining = Math.Max(0, record.crisisDaysRemaining - 1);
                    if (record.crisisDaysRemaining == 0)
                    {
                        record.currentCrisisId = string.Empty;
                        ReduceStress(record.survivorId, 100);
                    }
                }

                if (record.insomniaDaysRemaining > 0)
                    record.insomniaDaysRemaining--;

                foreach (var traumaId in record.activeTraumaIds)
                {
                    if (_traumas.TryGetValue(traumaId, out var definition)
                        && definition.insomnia_chance_permille > 0
                        && _rng.Next(0, 1000) < definition.insomnia_chance_permille)
                    {
                        record.insomniaDaysRemaining = Math.Max(record.insomniaDaysRemaining, 1);
                    }
                }
            }
        }

        public int GetProductivityPenaltyPermille(string survivorId)
        {
            if (!TryGetLiveRecord(survivorId, out var record)) return 0;
            long penalty = 0;
            if (!string.IsNullOrWhiteSpace(record.currentCrisisId)
                && _crises.TryGetValue(record.currentCrisisId, out var crisis))
                penalty += Math.Max(0, crisis.productivity_penalty_permille);
            if (record.insomniaDaysRemaining > 0) penalty += 200;
            return (int)Math.Clamp(penalty, 0L, 1000L);
        }

        public SurvivorMentalHealthState CaptureState() => NormalizeState(_state);

        public void RestoreState(SurvivorMentalHealthState? state)
        {
            _state = state == null ? new SurvivorMentalHealthState() : NormalizeState(state);
        }

        private SurvivorMentalHealthRecord GetOrCreateLiveRecord(string survivorId, int initialStress = 200)
        {
            string id = NormalizeId(survivorId);
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Survivor ID cannot be null or empty.", nameof(survivorId));
            if (!_state.survivorRecords.TryGetValue(id, out var record))
            {
                record = new SurvivorMentalHealthRecord
                {
                    survivorId = id,
                    stressPermille = ClampPermille(initialStress)
                };
                _state.survivorRecords[id] = record;
            }
            return record;
        }

        private bool TryGetLiveRecord(string survivorId, out SurvivorMentalHealthRecord record)
        {
            string id = NormalizeId(survivorId);
            return _state.survivorRecords.TryGetValue(id, out record!);
        }

        private void TriggerPotentialCrisis(SurvivorMentalHealthRecord record, string triggerTag)
        {
            string chosenCrisis = string.Empty;
            foreach (var traumaId in record.activeTraumaIds)
            {
                if (_traumas.TryGetValue(traumaId, out var definition)
                    && !string.IsNullOrWhiteSpace(definition.crisis_affinity)
                    && _crises.ContainsKey(definition.crisis_affinity))
                {
                    chosenCrisis = definition.crisis_affinity;
                    break;
                }
            }

            if (string.IsNullOrWhiteSpace(chosenCrisis))
                chosenCrisis = _crises.ContainsKey("crisis_withdrawal") ? "crisis_withdrawal" : "crisis_panic";

            if (_crises.TryGetValue(chosenCrisis, out var crisis)
                && record.stressPermille >= crisis.threshold_stress_permille)
            {
                record.currentCrisisId = chosenCrisis;
                record.crisisDaysRemaining = Math.Max(1, crisis.duration_days);
            }
        }

        private SurvivorMentalHealthState NormalizeState(SurvivorMentalHealthState source)
        {
            var normalized = new SurvivorMentalHealthState
            {
                systemId = SystemId,
                survivorRecords = new Dictionary<string, SurvivorMentalHealthRecord>(IdentityComparer),
                totalCatharsisBreakthroughs = Math.Max(0, source.totalCatharsisBreakthroughs),
                lastTickDay = Math.Max(-1, source.lastTickDay)
            };
            if (source.survivorRecords == null) return normalized;

            foreach (var pair in source.survivorRecords)
            {
                if (pair.Value == null) continue;
                string id = NormalizeId(pair.Value.survivorId);
                if (string.IsNullOrWhiteSpace(id)) id = NormalizeId(pair.Key);
                if (string.IsNullOrWhiteSpace(id) || normalized.survivorRecords.ContainsKey(id)) continue;
                var record = NormalizeRecord(pair.Value, id);
                for (int i = 0; i < record.activeTraumaIds.Count; i++)
                {
                    if (_traumas.TryGetValue(record.activeTraumaIds[i], out var definition))
                        record.activeTraumaIds[i] = definition.id;
                }
                if (_crises.TryGetValue(record.currentCrisisId, out var crisis))
                    record.currentCrisisId = crisis.id;
                normalized.survivorRecords[id] = record;
            }
            return normalized;
        }

        private static SurvivorMentalHealthRecord NormalizeRecord(SurvivorMentalHealthRecord source, string id)
        {
            var record = new SurvivorMentalHealthRecord
            {
                survivorId = id,
                stressPermille = ClampPermille(source.stressPermille),
                activeTraumaIds = source.activeTraumaIds?
                    .Where(traumaId => !string.IsNullOrWhiteSpace(traumaId))
                    .Select(traumaId => NormalizeId(traumaId))
                    .Distinct(IdentityComparer)
                    .ToList() ?? new List<string>(),
                insomniaDaysRemaining = Math.Max(0, source.insomniaDaysRemaining),
                currentCrisisId = NormalizeId(source.currentCrisisId),
                crisisDaysRemaining = Math.Max(0, source.crisisDaysRemaining),
                therapySessionCount = Math.Max(0, source.therapySessionCount)
            };
            if (string.IsNullOrWhiteSpace(record.currentCrisisId)) record.crisisDaysRemaining = 0;
            return record;
        }

        private static SurvivorMentalHealthRecord CloneRecord(SurvivorMentalHealthRecord source) =>
            NormalizeRecord(source, NormalizeId(source.survivorId));

        private static TraumaTypeDefinition NormalizeTrauma(TraumaTypeDefinition source, string id) => new TraumaTypeDefinition
        {
            id = id,
            display_name = NormalizeText(source.display_name, string.Empty),
            description = NormalizeText(source.description, string.Empty),
            stress_floor_permille = Math.Max(0, source.stress_floor_permille),
            insomnia_chance_permille = Math.Clamp(source.insomnia_chance_permille, 0, 1000),
            trigger_tags = source.trigger_tags?
                .Where(tag => !string.IsNullOrWhiteSpace(tag))
                .Select(tag => tag.Trim())
                .Distinct(IdentityComparer)
                .ToList() ?? new List<string>(),
            crisis_affinity = NormalizeId(source.crisis_affinity)
        };

        private static RecoveryActionDefinition NormalizeRecovery(RecoveryActionDefinition source, string id) => new RecoveryActionDefinition
        {
            id = id,
            display_name = NormalizeText(source.display_name, string.Empty),
            description = NormalizeText(source.description, string.Empty),
            required_room = NormalizeId(source.required_room),
            stress_reduction_permille = Math.Max(0, source.stress_reduction_permille),
            daily_resolution_chance_permille = Math.Clamp(source.daily_resolution_chance_permille, 0, 1000),
            counselor_bonus_permille = Math.Max(0, source.counselor_bonus_permille)
        };

        private static CrisisEventDefinition NormalizeCrisis(CrisisEventDefinition source, string id) => new CrisisEventDefinition
        {
            id = id,
            display_name = NormalizeText(source.display_name, string.Empty),
            description = NormalizeText(source.description, string.Empty),
            threshold_stress_permille = Math.Clamp(source.threshold_stress_permille, 0, 1000),
            duration_days = Math.Max(0, source.duration_days),
            productivity_penalty_permille = Math.Clamp(source.productivity_penalty_permille, 0, 1000),
            journal_entry_key = NormalizeId(source.journal_entry_key)
        };

        private static TraumaTypeDefinition CloneTrauma(TraumaTypeDefinition source) => NormalizeTrauma(source, source.id);
        private static RecoveryActionDefinition CloneRecovery(RecoveryActionDefinition source) => NormalizeRecovery(source, source.id);
        private static CrisisEventDefinition CloneCrisis(CrisisEventDefinition source) => NormalizeCrisis(source, source.id);

        private static void SaturatingIncrement(ref int value)
        {
            if (value < 0) value = 0;
            else if (value < int.MaxValue) value++;
        }

        private static int ClampPermille(int value) => Math.Clamp(value, 0, 1000);
        private static int ClampPermille(long value) => (int)Math.Clamp(value, 0L, 1000L);

        private static int ClampAdd(int first, int second, int maximum = 1000) =>
            (int)Math.Clamp((long)first + second, 0L, maximum);

        private static string NormalizeId(string? value) => value?.Trim() ?? string.Empty;
        private static string NormalizeText(string? value, string fallback) =>
            string.IsNullOrWhiteSpace(value) ? fallback : value.Trim();
    }
}
