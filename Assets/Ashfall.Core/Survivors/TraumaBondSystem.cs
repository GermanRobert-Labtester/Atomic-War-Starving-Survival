#pragma warning disable CS0067 // Public API event surface; subscribers arrive with feature wiring
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Survivors
{
    // ── Save-state DTOs ────────────────────────────────────────────────

    [Serializable]
    public class TraumaBondSaveState
    {
        public List<TraumaBondSurvivorState> Survivors = new List<TraumaBondSurvivorState>();
    }

    [Serializable]
    public class TraumaBondSurvivorState
    {
        public string SurvivorId;
        public List<TraumaBondRecordState> Bonds = new List<TraumaBondRecordState>();
    }

    [Serializable]
    public class TraumaBondRecordState
    {
        public string BondedSurvivorId;
        public float BondStrength;
        public string SharedHazardId;
        public int DayFormed;
    }

    // ── System ─────────────────────────────────────────────────────────

    /// <summary>
    /// Trauma Bond System — survivors who endure extreme hazards together
    /// form deep bonds, boosting work efficiency when assigned to identical
    /// shifts. Bonds decay without shared activity.
    ///
    /// Engine-agnostic port. Survivors are identified by string IDs.
    /// </summary>
    public class TraumaBondSystem
    {
        // ── Constants ──────────────────────────────────────────────────
        public const float BondStrengthPerSharedHazard = 0.30f;
        public const float BondDecayPerDay = 0.01f;
        public const float BondAffinityBonus = 15f;
        public const float CoShiftEfficiencyBonus = 0.25f;
        public const float MinBondStrengthForBonus = 0.3f;

        // ── Events ─────────────────────────────────────────────────────
        /// <summary>Fired when a new bond is created: (survivorA, survivorB, hazardId).</summary>
        public event Action<string, string, string> OnTraumaBondFormed;

        /// <summary>Fired when a bond fully decays: (survivorA, survivorB).</summary>
        public event Action<string, string> OnTraumaBondDecayed;

        /// <summary>Fired when co-shift bonus is queried and applied: (survivorA, survivorB, bonus).</summary>
        public event Action<string, string, float> OnCoShiftBonusApplied;

        /// <summary>Generic state-changed event for save/UI.</summary>
        public event Action OnStateChanged;

        // ── Host hooks ─────────────────────────────────────────────────

        /// <summary>Adjust affinity between two survivors: (a, b, delta).</summary>
        public Action<string, string, float> AdjustAffinity;

        /// <summary>Check if two survivors are on the same shift.</summary>
        public Func<string, string, bool> AreOnSameShift;

        /// <summary>Get the current game day (1-based).</summary>
        public Func<float> GetDay;

        // ── Internal state ─────────────────────────────────────────────
        readonly Dictionary<string, List<TraumaBondRecordState>> _bondsBySurvivor
            = new Dictionary<string, List<TraumaBondRecordState>>();

        // ── Public API ─────────────────────────────────────────────────

        /// <summary>
        /// Call when two or more survivors endure a shared hazard
        /// (fallout storm, raid, starvation day).
        /// </summary>
        public void OnSharedHazardEndured(List<string> participantIds, string hazardId)
        {
            if (participantIds == null || participantIds.Count < 2) return;
            int day = MathfCompat.Max(1, (int)(GetDay?.Invoke() ?? 1f));

            for (int i = 0; i < participantIds.Count; i++)
            {
                for (int j = i + 1; j < participantIds.Count; j++)
                {
                    var aId = participantIds[i];
                    var bId = participantIds[j];
                    if (string.IsNullOrEmpty(aId) || string.IsNullOrEmpty(bId)) continue;
                    if (aId == bId) continue;

                    ProcessBondPair(aId, bId, hazardId, day);
                    ProcessBondPair(bId, aId, hazardId, day);

                    // Boost affinity
                    AdjustAffinity?.Invoke(aId, bId, BondAffinityBonus);
                }
            }

            OnStateChanged?.Invoke();
        }

        /// <summary>
        /// Get the bonus work efficiency for two survivors on the same shift.
        /// Returns 0 when no qualifying bond exists.
        /// </summary>
        public float GetCoShiftEfficiencyBonus(string survivorA, string survivorB)
        {
            if (string.IsNullOrEmpty(survivorA) || string.IsNullOrEmpty(survivorB))
                return 0f;
            if (survivorA == survivorB) return 0f;

            float bondStrength = GetBondStrength(survivorA, survivorB);
            if (bondStrength < MinBondStrengthForBonus) return 0f;

            float bonus = CoShiftEfficiencyBonus * bondStrength;
            OnCoShiftBonusApplied?.Invoke(survivorA, survivorB, bonus);
            return bonus;
        }

        /// <summary>
        /// Tick — decay bond strengths for a survivor over elapsed game hours.
        /// </summary>
        public void Tick(string survivorId, float gameHours)
        {
            if (string.IsNullOrEmpty(survivorId)) return;
            if (!_bondsBySurvivor.TryGetValue(survivorId, out var bonds)) return;
            if (bonds.Count == 0) return;

            float dailyDecay = BondDecayPerDay * (gameHours / 24f);
            bool changed = false;

            for (int i = bonds.Count - 1; i >= 0; i--)
            {
                var bond = bonds[i];
                bond.BondStrength -= dailyDecay;
                if (bond.BondStrength <= 0f)
                {
                    string partnerId = bond.BondedSurvivorId;
                    bonds.RemoveAt(i);
                    OnTraumaBondDecayed?.Invoke(survivorId, partnerId);
                    changed = true;
                }
                else
                {
                    bonds[i] = bond;
                    changed = true;
                }
            }

            if (changed)
                OnStateChanged?.Invoke();
        }

        /// <summary>
        /// Get the bond strength between two survivors. Returns 0 if no bond exists.
        /// </summary>
        public float GetBondStrength(string survivorA, string survivorB)
        {
            if (string.IsNullOrEmpty(survivorA) || string.IsNullOrEmpty(survivorB))
                return 0f;
            if (!_bondsBySurvivor.TryGetValue(survivorA, out var bonds)) return 0f;

            for (int i = 0; i < bonds.Count; i++)
            {
                if (string.Equals(bonds[i].BondedSurvivorId, survivorB, StringComparison.Ordinal))
                    return bonds[i].BondStrength;
            }
            return 0f;
        }

        /// <summary>
        /// Get the number of active bonds for a survivor.
        /// </summary>
        public int GetBondCount(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return 0;
            if (!_bondsBySurvivor.TryGetValue(survivorId, out var bonds)) return 0;
            return bonds.Count;
        }

        /// <summary>
        /// Check whether a bond exists between two survivors (regardless of strength).
        /// </summary>
        public bool HasBond(string survivorA, string survivorB)
        {
            return GetBondStrength(survivorA, survivorB) > 0f;
        }

        // ── Save / Load ────────────────────────────────────────────────

        /// <summary>
        /// Capture a deep copy of all bond state for serialization.
        /// </summary>
        public TraumaBondSaveState CaptureState()
        {
            var save = new TraumaBondSaveState();
            foreach (var kvp in _bondsBySurvivor)
            {
                var svState = new TraumaBondSurvivorState
                {
                    SurvivorId = kvp.Key,
                    Bonds = new List<TraumaBondRecordState>(kvp.Value.Count)
                };
                foreach (var bond in kvp.Value)
                {
                    svState.Bonds.Add(new TraumaBondRecordState
                    {
                        BondedSurvivorId = bond.BondedSurvivorId,
                        BondStrength = bond.BondStrength,
                        SharedHazardId = bond.SharedHazardId,
                        DayFormed = bond.DayFormed
                    });
                }
                save.Survivors.Add(svState);
            }
            return save;
        }

        /// <summary>
        /// Restore state from a previously captured save. Deep copies all data.
        /// Pass null to clear all state.
        /// </summary>
        public void RestoreState(TraumaBondSaveState save)
        {
            _bondsBySurvivor.Clear();
            if (save == null || save.Survivors == null) return;

            foreach (var svState in save.Survivors)
            {
                if (svState == null || string.IsNullOrEmpty(svState.SurvivorId)) continue;
                var bonds = new List<TraumaBondRecordState>(
                    svState.Bonds?.Count ?? 0);
                if (svState.Bonds != null)
                {
                    foreach (var bond in svState.Bonds)
                    {
                        if (bond == null) continue;
                        bonds.Add(new TraumaBondRecordState
                        {
                            BondedSurvivorId = bond.BondedSurvivorId,
                            BondStrength = bond.BondStrength,
                            SharedHazardId = bond.SharedHazardId,
                            DayFormed = bond.DayFormed
                        });
                    }
                }
                _bondsBySurvivor[svState.SurvivorId] = bonds;
            }

            OnStateChanged?.Invoke();
        }

        // ── Private helpers ────────────────────────────────────────────

        void ProcessBondPair(string ownerAId, string ownerBId, string hazardId, int day)
        {
            if (!_bondsBySurvivor.TryGetValue(ownerAId, out var bonds))
            {
                bonds = new List<TraumaBondRecordState>();
                _bondsBySurvivor[ownerAId] = bonds;
            }

            int idx = FindBondIndex(bonds, ownerBId);
            if (idx >= 0)
            {
                var bond = bonds[idx];
                bond.BondStrength = MathfCompat.Min(1f,
                    bond.BondStrength + BondStrengthPerSharedHazard);
                bonds[idx] = bond;
            }
            else
            {
                bonds.Add(new TraumaBondRecordState
                {
                    BondedSurvivorId = ownerBId,
                    BondStrength = BondStrengthPerSharedHazard,
                    SharedHazardId = hazardId,
                    DayFormed = day
                });
                OnTraumaBondFormed?.Invoke(ownerAId, ownerBId, hazardId);
            }
        }

        static int FindBondIndex(List<TraumaBondRecordState> bonds, string otherId)
        {
            if (bonds == null) return -1;
            for (int i = 0; i < bonds.Count; i++)
            {
                if (string.Equals(bonds[i].BondedSurvivorId, otherId, StringComparison.Ordinal))
                    return i;
            }
            return -1;
        }
    }
}
