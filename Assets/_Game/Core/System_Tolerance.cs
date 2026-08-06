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
    /// Morphine / Amphetamines / Anti-Rad build tolerance with repeated use.
    /// Callers must query <see cref="GetDuration"/> / <see cref="GetEffectiveness"/>
    /// BEFORE <see cref="UseChem"/> so the current dose uses pre-increment counts
    /// (1st use = 24 h / full effect; after 6 uses = no therapeutic benefit).
    /// Plain C# class, not a MonoBehaviour.
    /// </summary>
    public class System_Tolerance
    {
        // ── Constants ──────────────────────────────────────────────────
        public const float BaseDurationHours = 24f;
        private const float WITHDRAWAL_WINDOW = 48f; // hours before withdrawal kicks in
        private const int TOLERANCE_THRESHOLD = 6;   // 6+ uses → only prevents withdrawal

        /// <summary>Chems that build tolerance (Prompt #833 + anti_rad for rad meds).</summary>
        private static readonly HashSet<string> TrackedChemIds =
            new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "morphine",
                "amphetamines",
                "anti_rad"
            };

        // ── Events ─────────────────────────────────────────────────────
        public event Action<string, string, int> OnChemUsed;              // survivorId, chemId, useCount
        public event Action<string, string, float> OnToleranceIncreased; // survivorId, chemId, newDuration
        public event Action<string, string> OnWithdrawalStarted;          // survivorId, chemId

        // ── State ──────────────────────────────────────────────────────
        // survivorId → (chemId → ToleranceEntry) — chem keys are lower-invariant
        private readonly Dictionary<string, Dictionary<string, ToleranceEntry>> _data
            = new Dictionary<string, Dictionary<string, ToleranceEntry>>();

        // ── Public API ─────────────────────────────────────────────────

        public static bool IsToleranceChem(string chemId) =>
            !string.IsNullOrEmpty(chemId) && TrackedChemIds.Contains(chemId);

        /// <summary>
        /// Record a chemical use. Increments the tolerance counter and
        /// fires events. No-op for non-tracked chem ids.
        /// </summary>
        public void UseChem(string survivorId, string chemId, float gameTime)
        {
            if (string.IsNullOrEmpty(survivorId) || string.IsNullOrEmpty(chemId)) return;
            if (!IsToleranceChem(chemId)) return;

            string key = NormalizeChemId(chemId);
            var entry = GetOrCreate(survivorId, key);
            entry.use_count++;
            entry.last_use_time = gameTime;

            float newDuration = DurationFromUseCount(entry.use_count);

            OnChemUsed?.Invoke(survivorId, key, entry.use_count);
            OnToleranceIncreased?.Invoke(survivorId, key, newDuration);
        }

        /// <summary>
        /// Effective duration in hours for the <em>next</em> dose (pre-increment).
        /// Formula: 24 * (1 / (1 + useCount * 0.5)). First use → 24 h.
        /// </summary>
        public float GetDuration(string survivorId, string chemId)
        {
            if (string.IsNullOrEmpty(survivorId) || string.IsNullOrEmpty(chemId))
                return BaseDurationHours;
            if (!IsToleranceChem(chemId)) return BaseDurationHours;

            var entry = GetEntry(survivorId, NormalizeChemId(chemId));
            if (entry == null) return BaseDurationHours;

            return DurationFromUseCount(entry.use_count);
        }

        /// <summary>
        /// Therapeutic effectiveness (0-1) for the <em>next</em> dose (pre-increment).
        /// Formula: max(0.1, 1.0 - useCount * 0.15). After 6+ prior uses → 0.
        /// </summary>
        public float GetEffectiveness(string survivorId, string chemId)
        {
            if (string.IsNullOrEmpty(survivorId) || string.IsNullOrEmpty(chemId))
                return 1f;
            if (!IsToleranceChem(chemId)) return 1f;

            var entry = GetEntry(survivorId, NormalizeChemId(chemId));
            if (entry == null) return 1f;

            if (entry.use_count >= TOLERANCE_THRESHOLD) return 0f;

            return Mathf.Max(0.1f, 1f - entry.use_count * 0.15f);
        }

        public static float DurationFromUseCount(int useCount)
        {
            if (useCount < 0) useCount = 0;
            return BaseDurationHours * (1f / (1f + useCount * 0.5f));
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
            if (!IsToleranceChem(chemId)) return false;

            string key = NormalizeChemId(chemId);
            var entry = GetEntry(survivorId, key);
            if (entry == null || entry.use_count == 0) return false;

            float elapsed = gameTime - entry.last_use_time;
            bool inWithdrawal = elapsed > WITHDRAWAL_WINDOW;

            if (inWithdrawal)
                OnWithdrawalStarted?.Invoke(survivorId, key);

            return inWithdrawal;
        }

        /// <summary>Returns the total use count for a survivor + chemical.</summary>
        public int GetUseCount(string survivorId, string chemId)
        {
            if (string.IsNullOrEmpty(survivorId) || string.IsNullOrEmpty(chemId))
                return 0;
            if (!IsToleranceChem(chemId)) return 0;

            var entry = GetEntry(survivorId, NormalizeChemId(chemId));
            return entry != null ? entry.use_count : 0;
        }

        // ── Helpers ────────────────────────────────────────────────────

        private static string NormalizeChemId(string chemId) =>
            chemId != null ? chemId.Trim().ToLowerInvariant() : "";

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
                chemMap = new Dictionary<string, ToleranceEntry>(StringComparer.OrdinalIgnoreCase);
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
            if (saved == null || saved.survivors == null) return;

            foreach (var survivorState in saved.survivors)
            {
                if (survivorState == null || string.IsNullOrEmpty(survivorState.survivor_id)) continue;

                var chemMap = new Dictionary<string, ToleranceEntry>(StringComparer.OrdinalIgnoreCase);
                if (survivorState.entries != null)
                {
                    foreach (var entry in survivorState.entries)
                    {
                        if (entry == null || string.IsNullOrEmpty(entry.chem_id)) continue;
                        string key = NormalizeChemId(entry.chem_id);
                        chemMap[key] = new ToleranceEntry
                        {
                            chem_id = key,
                            use_count = entry.use_count,
                            last_use_time = entry.last_use_time
                        };
                    }
                }

                _data[survivorState.survivor_id] = chemMap;
            }
        }
    }
}
