using System;
using System.Collections.Generic;

namespace Ashfall.Core.Survivors
{
    [Serializable]
    public sealed class GuiltRecord
    {
        public string sourceId = string.Empty;
        public int dayRecorded;
        public float severity;
    }

    [Serializable]
    public sealed class GuiltInsomniaSaveState
    {
        public List<GuiltSurvivorState> survivors = new List<GuiltSurvivorState>();
    }

    [Serializable]
    public sealed class GuiltSurvivorState
    {
        public string survivorId = string.Empty;
        public float insomniaSeverity;
        public float sedativeCompensationHours;
        public List<GuiltRecord> guiltSources = new List<GuiltRecord>();
    }

    /// <summary>
    /// ASHFALL: THE MASSIVE CONTENT EXPANSION — Guilt-Driven Insomnia System.
    /// Ruthless decisions create guilt records that multiply sleep quality penalties.
    /// Sedatives or interpersonal dialogue can compensate. Engine-agnostic: uses
    /// string survivor IDs, raises events, save/load safe.
    /// </summary>
    public class GuiltInsomniaSystem
    {
        public const float SleepQualityPenaltyPerSeverity = 0.50f;
        public const float SedativeCompensationHours = 12f;
        public const float SedativeSeverityReduction = 0.40f;
        public const float DialogueSeverityReduction = 0.25f;
        public const float NaturalDecayPerDay = 0.05f;
        public const float HighSeverityThreshold = 0.7f;
        public const int GuiltExpiryDays = 30;

        public event Action<string, GuiltRecord> OnGuiltRecorded;
        public event Action<string> OnGuiltResolved;
        public event Action<string> OnGuiltInsomniaCritical;
        public event Action OnStateChanged;

        private readonly Dictionary<string, GuiltSurvivorState> _bySurvivor =
            new Dictionary<string, GuiltSurvivorState>(StringComparer.Ordinal);

        private GuiltSurvivorState GetOrCreate(string survivorId)
        {
            if (!_bySurvivor.TryGetValue(survivorId, out var state))
            {
                state = new GuiltSurvivorState { survivorId = survivorId };
                _bySurvivor[survivorId] = state;
            }
            return state;
        }

        public void RecordGuilt(string survivorId, string sourceId, float severity, int currentDay)
        {
            if (string.IsNullOrEmpty(survivorId) || severity <= 0f) return;
            var state = GetOrCreate(survivorId);
            state.guiltSources.Add(new GuiltRecord
            {
                sourceId = sourceId ?? string.Empty,
                dayRecorded = Math.Max(1, currentDay),
                severity = severity
            });
            UpdateInsomniaSeverity(state);
            OnGuiltRecorded?.Invoke(survivorId, state.guiltSources[state.guiltSources.Count - 1]);
            OnStateChanged?.Invoke();
        }

        public bool ApplySedative(string survivorId)
        {
            if (!_bySurvivor.TryGetValue(survivorId, out var state)) return false;
            if (state.insomniaSeverity <= 0f) return false;
            state.sedativeCompensationHours = SedativeCompensationHours;
            float old = state.insomniaSeverity;
            state.insomniaSeverity = Math.Max(0f, state.insomniaSeverity - SedativeSeverityReduction);
            OnStateChanged?.Invoke();
            return state.insomniaSeverity < old;
        }

        public bool ResolveGuiltThroughDialogue(string survivorId)
        {
            if (!_bySurvivor.TryGetValue(survivorId, out var state)) return false;
            if (state.guiltSources.Count == 0) return false;
            state.guiltSources.RemoveAt(state.guiltSources.Count - 1);
            UpdateInsomniaSeverity(state);
            if (state.guiltSources.Count == 0)
                OnGuiltResolved?.Invoke(survivorId);
            OnStateChanged?.Invoke();
            return true;
        }

        public float GetSleepQualityMultiplier(string survivorId)
        {
            if (!_bySurvivor.TryGetValue(survivorId, out var state)) return 1f;
            float penalty = state.insomniaSeverity * SleepQualityPenaltyPerSeverity;
            if (state.sedativeCompensationHours > 0f) penalty *= 0.5f;
            return Math.Max(0.1f, 1f - penalty);
        }

        public float GetInsomniaSeverity(string survivorId)
        {
            return _bySurvivor.TryGetValue(survivorId, out var state) ? state.insomniaSeverity : 0f;
        }

        public int GetGuiltSourceCount(string survivorId)
        {
            return _bySurvivor.TryGetValue(survivorId, out var state) ? state.guiltSources.Count : 0;
        }

        public void Tick(string survivorId, float gameHours, int currentDay)
        {
            if (!_bySurvivor.TryGetValue(survivorId, out var state)) return;

            if (state.sedativeCompensationHours > 0f)
            {
                state.sedativeCompensationHours = Math.Max(0f, state.sedativeCompensationHours - gameHours);
                if (state.sedativeCompensationHours <= 0f)
                    UpdateInsomniaSeverity(state);
            }

            if (state.guiltSources.Count > 0)
            {
                for (int i = state.guiltSources.Count - 1; i >= 0; i--)
                {
                    if (currentDay - state.guiltSources[i].dayRecorded > GuiltExpiryDays)
                        state.guiltSources.RemoveAt(i);
                }
                if (state.guiltSources.Count == 0)
                {
                    state.insomniaSeverity = 0f;
                    OnGuiltResolved?.Invoke(survivorId);
                }
                else
                {
                    UpdateInsomniaSeverity(state);
                }
            }
            OnStateChanged?.Invoke();
        }

        private void UpdateInsomniaSeverity(GuiltSurvivorState state)
        {
            float total = 0f;
            for (int i = 0; i < state.guiltSources.Count; i++)
                total += state.guiltSources[i].severity;
            state.insomniaSeverity = Math.Min(1f, total);
            if (state.insomniaSeverity >= HighSeverityThreshold)
                OnGuiltInsomniaCritical?.Invoke(state.survivorId);
        }

        public GuiltInsomniaSaveState CaptureState()
        {
            var save = new GuiltInsomniaSaveState();
            foreach (var kv in _bySurvivor)
            {
                var s = kv.Value;
                var copy = new GuiltSurvivorState
                {
                    survivorId = s.survivorId,
                    insomniaSeverity = s.insomniaSeverity,
                    sedativeCompensationHours = s.sedativeCompensationHours
                };
                foreach (var g in s.guiltSources)
                    copy.guiltSources.Add(new GuiltRecord
                    {
                        sourceId = g.sourceId,
                        dayRecorded = g.dayRecorded,
                        severity = g.severity
                    });
                save.survivors.Add(copy);
            }
            return save;
        }

        public void RestoreState(GuiltInsomniaSaveState save)
        {
            _bySurvivor.Clear();
            if (save?.survivors == null) return;
            foreach (var s in save.survivors)
            {
                if (s == null || string.IsNullOrEmpty(s.survivorId)) continue;
                var copy = new GuiltSurvivorState
                {
                    survivorId = s.survivorId,
                    insomniaSeverity = s.insomniaSeverity,
                    sedativeCompensationHours = s.sedativeCompensationHours
                };
                if (s.guiltSources != null)
                    foreach (var g in s.guiltSources)
                        copy.guiltSources.Add(new GuiltRecord
                        {
                            sourceId = g.sourceId,
                            dayRecorded = g.dayRecorded,
                            severity = g.severity
                        });
                _bySurvivor[s.survivorId] = copy;
            }
        }
    }
}
