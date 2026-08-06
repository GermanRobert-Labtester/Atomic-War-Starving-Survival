using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class ToleranceEntry
    {
        public string chem_id = "";
        public int use_count = 0;
        public float last_use_time = 0f;
    }

    [Serializable]
    public class ToleranceSurvivorState
    {
        public string survivor_id = "";
        public List<ToleranceEntry> entries = new List<ToleranceEntry>();
    }

    [Serializable]
    public class ToleranceState
    {
        public string system_id = "system_tolerance";
        public List<ToleranceSurvivorState> survivors = new List<ToleranceSurvivorState>();
    }

    /// <summary>
    /// Prompt #833: Tolerance.
    /// Morphine / Amphetamines build tolerance with repeated use.
    /// 1st use = 24 h effect, 5th use = ~4 h. After 6+ uses the chemical
    /// only prevents Withdrawal, providing no therapeutic benefit.
    /// Plain C# class, not a MonoBehaviour.
    /// </summary>
    public class System_Tolerance
    {
        // ── Constants ──────────────────────────────────────────────────
        private const float BASE_DURATION = 24f;
        private const float WITHDRAWAL_WINDOW = 48f; // hours before withdrawal kicks in
        private const int TOLERANCE_THRESHOLD = 6;   // 6+ uses → only prevents withdrawal

        // ── Events ─────────────────────────────────────────────────────
        public event Action<string, string, int> OnChemUsed;              // survivorId, chemId, useCount
        public event Action<string, string, float> OnToleranceIncreased; // survivorId, chemId, newDuration
        public event Action<string, string> OnWithdrawalStarted;          // survivorId, chemId

        // ── State ──────────────────────────────────────────────────────
        // survivorId → (chemId → ToleranceEntry)
        private readonly Dictionary<string, Dictionary<string, ToleranceEntry>> _data
            = new Dictionary<string, Dictionary<string, ToleranceEntry>>();

        // ── Public API ─────────────────────────────────────────────────

        /// <summary>
        /// Record a chemical use. Increments the tolerance counter and
        /// fires events.
        /// </summary>
        public void UseChem(string survivorId, string chemId, float gameTime)
        {
            if (string.IsNullOrEmpty(survivorId) || string.IsNullOrEmpty(chemId)) return;

            var entry = GetOrCreate(survivorId, chemId);
            entry.use_count++;
            entry.last_use_time = gameTime;

            float newDuration = GetDuration(survivorId, chemId);

            OnChemUsed?.Invoke(survivorId, chemId, entry.use_count);
            OnToleranceIncreased?.Invoke(survivorId, chemId, newDuration);
        }

        /// <summary>
        /// Returns the effective duration in hours for the next dose.
        /// Formula: 24 * (1 / (1 + useCount * 0.5)).
        /// </summary>
        public float GetDuration(string survivorId, string chemId)
        {
            if (string.IsNullOrEmpty(survivorId) || string.IsNullOrEmpty(chemId))
                return BASE_DURATION;

            var entry = GetEntry(survivorId, chemId);
            if (entry == null) return BASE_DURATION;

            return BASE_DURATION * (1f / (1f + entry.use_count * 0.5f));
        }

        /// <summary>
        /// Returns the therapeutic effectiveness (0-1).
        /// Formula: max(0.1, 1.0 - useCount * 0.15).
        /// After 6+ uses the chem only prevents withdrawal (effectiveness 0).
        /// </summary>
        public float GetEffectiveness(string survivorId, string chemId)
        {
            if (string.IsNullOrEmpty(survivorId) || string.IsNullOrEmpty(chemId))
                return 1f;

            var entry = GetEntry(survivorId, chemId);
            if (entry == null) return 1f;

            if (entry.use_count >= TOLERANCE_THRESHOLD) return 0f;

            return Mathf.Max(0.1f, 1f - entry.use_count * 0.15f);
        }

        /// <summary>
        /// Returns true if the survivor is in withdrawal — i.e. they have
        /// prior uses and more than WITHDRAWAL_WINDOW hours have elapsed
        /// since the last dose.
        /// </summary>
        public bool IsInWithdrawal(string survivorId, string chemId, float gameTime)
        {
            if (string.IsNullOrEmpty(survivorId) || string.IsNullOrEmpty(chemId))
                return false;

            var entry = GetEntry(survivorId, chemId);
            if (entry == null || entry.use_count == 0) return false;

            float elapsed = gameTime - entry.last_use_time;
            bool inWithdrawal = elapsed > WITHDRAWAL_WINDOW;

            if (inWithdrawal)
                OnWithdrawalStarted?.Invoke(survivorId, chemId);

            return inWithdrawal;
        }

        /// <summary>Returns the total use count for a survivor + chemical.</summary>
        public int GetUseCount(string survivorId, string chemId)
        {
            if (string.IsNullOrEmpty(survivorId) || string.IsNullOrEmpty(chemId))
                return 0;

            var entry = GetEntry(survivorId, chemId);
            return entry != null ? entry.use_count : 0;
        }

        // ── Helpers ────────────────────────────────────────────────────

        private ToleranceEntry GetEntry(string survivorId, string chemId)
        {
            if (_data.TryGetValue(survivorId, out var chemMap))
            {
                if (chemMap.TryGetValue(chemId, out var entry))
                    return entry;
            }
            return null;
        }

        private ToleranceEntry GetOrCreate(string survivorId, string chemId)
        {
            if (!_data.TryGetValue(survivorId, out var chemMap))
            {
                chemMap = new Dictionary<string, ToleranceEntry>();
                _data[survivorId] = chemMap;
            }

            if (!chemMap.TryGetValue(chemId, out var entry))
            {
                entry = new ToleranceEntry { chem_id = chemId };
                chemMap[chemId] = entry;
            }

            return entry;
        }

        // ── Save / Load ────────────────────────────────────────────────

        public ToleranceState CaptureState()
        {
            var state = new ToleranceState
            {
                system_id = "system_tolerance",
                survivors = new List<ToleranceSurvivorState>()
            };

            foreach (var kvpSurvivor in _data)
            {
                var survivorState = new ToleranceSurvivorState
                {
                    survivor_id = kvpSurvivor.Key,
                    entries = new List<ToleranceEntry>()
                };

                foreach (var kvpChem in kvpSurvivor.Value)
                {
                    var src = kvpChem.Value;
                    survivorState.entries.Add(new ToleranceEntry
                    {
                        chem_id = src.chem_id,
                        use_count = src.use_count,
                        last_use_time = src.last_use_time
                    });
                }

                state.survivors.Add(survivorState);
            }

            return state;
        }

        public void RestoreState(ToleranceState saved)
        {
            _data.Clear();
            if (saved == null) return;

            foreach (var survivorState in saved.survivors)
            {
                if (string.IsNullOrEmpty(survivorState.survivor_id)) continue;

                var chemMap = new Dictionary<string, ToleranceEntry>();
                foreach (var entry in survivorState.entries)
                {
                    chemMap[entry.chem_id] = new ToleranceEntry
                    {
                        chem_id = entry.chem_id,
                        use_count = entry.use_count,
                        last_use_time = entry.last_use_time
                    };
                }

                _data[survivorState.survivor_id] = chemMap;
            }
        }
    }
}
