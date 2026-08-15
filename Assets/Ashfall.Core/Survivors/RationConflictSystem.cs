using System;
using System.Collections.Generic;

namespace Ashfall.Core.Survivors
{
    [Serializable]
    public sealed class RationConflictSurvivorState
    {
        public string survivorId = string.Empty;
        public float perceivedFairness;
        public string resentmentTargetId = string.Empty;
        public float resentmentLevel;
    }

    [Serializable]
    public sealed class RationConflictSaveState
    {
        public List<RationConflictSurvivorState> survivors = new List<RationConflictSurvivorState>();
    }

    /// <summary>
    /// ASHFALL: THE MASSIVE CONTENT EXPANSION — Ration Conflict System.
    /// Unequal food/water distribution creates targeted resentment between
    /// survivors, potentially sparking stolen rations or verbal confrontations.
    /// Engine-agnostic: uses string IDs, ISeededRng, save/load safe.
    /// </summary>
    public class RationConflictSystem
    {
        public const float FairnessDeviationThreshold = 0.20f;
        public const float ResentmentGainPerDay = 0.10f;
        public const float ResentmentDecayPerDay = 0.03f;
        public const float ConfrontationThreshold = 0.70f;
        public const float TheftThreshold = 0.85f;
        public const float ConfrontationMoraleHit = -10f;
        public const float TheftMoraleHit = -15f;

        public event Action<string, string, float> OnResentmentBuilt;
        public event Action<string, string> OnRationConfrontation;
        public event Action<string, string> OnRationsStolen;
        public event Action<string, float> OnMoraleDelta;
        public event Action OnStateChanged;

        private readonly ISeededRng _rng;
        private readonly Dictionary<string, RationConflictSurvivorState> _bySurvivor =
            new Dictionary<string, RationConflictSurvivorState>(StringComparer.Ordinal);
        private readonly Dictionary<string, float> _allocations =
            new Dictionary<string, float>(StringComparer.Ordinal);

        public RationConflictSystem(ISeededRng rng = null)
        {
            _rng = rng ?? new SeededRng(31415);
        }

        public void RegisterSurvivor(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return;
            if (!_bySurvivor.ContainsKey(survivorId))
                _bySurvivor[survivorId] = new RationConflictSurvivorState { survivorId = survivorId };
        }

        public void SetAllocation(string survivorId, float allocation)
        {
            _allocations[survivorId] = MathfCompat.Clamp01(allocation);
        }

        public float GetAllocation(string survivorId)
        {
            return _allocations.TryGetValue(survivorId, out var a) ? a : 0.5f;
        }

        public float GetAverageAllocation()
        {
            if (_allocations.Count == 0) return 0.5f;
            float sum = 0f;
            foreach (var kv in _allocations) sum += kv.Value;
            return sum / _allocations.Count;
        }

        public RationConflictSurvivorState GetState(string survivorId)
        {
            return _bySurvivor.TryGetValue(survivorId, out var s) ? s : null;
        }

        public void Tick(string survivorId, float gameHours)
        {
            if (!_bySurvivor.TryGetValue(survivorId, out var state)) return;

            float myAlloc = GetAllocation(survivorId);
            float average = GetAverageAllocation();
            state.perceivedFairness = MathfCompat.Clamp01(1f - Math.Abs(myAlloc - average));

            float deficit = average - myAlloc;
            if (deficit + 0.001f >= FairnessDeviationThreshold)
            {
                string mostOverId = null;
                float maxAlloc = 0f;
                foreach (var kv in _allocations)
                {
                    if (kv.Key == survivorId) continue;
                    if (kv.Value > maxAlloc)
                    {
                        maxAlloc = kv.Value;
                        mostOverId = kv.Key;
                    }
                }

                if (mostOverId != null)
                {
                    state.resentmentTargetId = mostOverId;
                    state.resentmentLevel = Math.Min(1f,
                        state.resentmentLevel + ResentmentGainPerDay * (gameHours / 24f));
                    OnResentmentBuilt?.Invoke(survivorId, mostOverId, state.resentmentLevel);

                    if (state.resentmentLevel >= TheftThreshold)
                        AttemptTheft(survivorId, mostOverId, state);
                    else if (state.resentmentLevel >= ConfrontationThreshold)
                        TriggerConfrontation(survivorId, mostOverId);
                }
            }
            else
            {
                state.resentmentLevel = Math.Max(0f,
                    state.resentmentLevel - ResentmentDecayPerDay * (gameHours / 24f));
                if (state.resentmentLevel <= 0f)
                    state.resentmentTargetId = string.Empty;
            }
            OnStateChanged?.Invoke();
        }

        private void TriggerConfrontation(string resenterId, string targetId)
        {
            OnMoraleDelta?.Invoke(resenterId, ConfrontationMoraleHit);
            OnMoraleDelta?.Invoke(targetId, ConfrontationMoraleHit * 0.5f);
            OnRationConfrontation?.Invoke(resenterId, targetId);
        }

        private void AttemptTheft(string thiefId, string victimId, RationConflictSurvivorState state)
        {
            double roll = _rng.NextDouble();
            if (roll < 0.3)
            {
                OnMoraleDelta?.Invoke(victimId, TheftMoraleHit);
                OnRationsStolen?.Invoke(thiefId, victimId);
                state.resentmentLevel = Math.Max(0f, state.resentmentLevel - 0.3f);
            }
        }

        public RationConflictSaveState CaptureState()
        {
            var save = new RationConflictSaveState();
            foreach (var kv in _bySurvivor)
            {
                var s = kv.Value;
                save.survivors.Add(new RationConflictSurvivorState
                {
                    survivorId = s.survivorId,
                    perceivedFairness = s.perceivedFairness,
                    resentmentTargetId = s.resentmentTargetId,
                    resentmentLevel = s.resentmentLevel
                });
            }
            return save;
        }

        public void RestoreState(RationConflictSaveState save)
        {
            _bySurvivor.Clear();
            if (save?.survivors == null) return;
            foreach (var s in save.survivors)
            {
                if (s == null || string.IsNullOrEmpty(s.survivorId)) continue;
                _bySurvivor[s.survivorId] = new RationConflictSurvivorState
                {
                    survivorId = s.survivorId,
                    perceivedFairness = s.perceivedFairness,
                    resentmentTargetId = s.resentmentTargetId ?? string.Empty,
                    resentmentLevel = s.resentmentLevel
                };
            }
        }
    }
}
