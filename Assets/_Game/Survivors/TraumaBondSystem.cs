using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Survivors
{
    /// <summary>
    /// Trauma Bond System — survivors who endure extreme hazards together
    /// form deep bonds, boosting work efficiency when assigned to identical
    /// shifts. Bonds decay without shared activity.
    ///
    /// Owns: Survivor.TraumaBonds (List of TraumaBondRecord).
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
        public event Action<Survivor, Survivor, string> OnTraumaBondFormed;
        public event Action<Survivor, Survivor> OnTraumaBondDecayed;
        public event Action<Survivor, Survivor, float> OnCoShiftBonusApplied;

        // ── Host hooks ─────────────────────────────────────────────────
        public Action<string, string, float> AdjustAffinity;
        // a, b, delta
        public Func<string, string, bool> AreOnSameShift;
        public Func<string, string, float> GetWorkEfficiencyMultiplier;
        // survivorA, survivorB → 1.0 normal
        public Func<float> GetDay;
        public System.Random Rng;

        /// <summary>
        /// Call when two or more survivors endure a shared hazard
        /// (fallout storm, raid, starvation day).
        /// </summary>
        public void OnSharedHazardEndured(List<Survivor> participants, string hazardId)
        {
            if (participants == null || participants.Count < 2) return;
            int day = Math.Max(1, (int)(GetDay?.Invoke() ?? 1));

            for (int i = 0; i < participants.Count; i++)
            {
                for (int j = i + 1; j < participants.Count; j++)
                {
                    var a = participants[i];
                    var b = participants[j];
                    if (a == null || b == null || !a.IsAlive || !b.IsAlive) continue;

                    // Find or create bond
                    int aIdx = FindBondIndex(a, b.Id);
                    int bIdx = FindBondIndex(b, a.Id);

                    if (aIdx >= 0)
                    {
                        var bond = a.TraumaBonds[aIdx];
                        bond.BondStrength = Math.Min(1f,
                            bond.BondStrength + BondStrengthPerSharedHazard);
                        a.TraumaBonds[aIdx] = bond;
                    }
                    else
                    {
                        a.TraumaBonds.Add(new TraumaBondRecord(
                            b.Id, BondStrengthPerSharedHazard, hazardId, day));
                        OnTraumaBondFormed?.Invoke(a, b, hazardId);
                    }

                    if (bIdx >= 0)
                    {
                        var bond = b.TraumaBonds[bIdx];
                        bond.BondStrength = Math.Min(1f,
                            bond.BondStrength + BondStrengthPerSharedHazard);
                        b.TraumaBonds[bIdx] = bond;
                    }
                    else
                    {
                        b.TraumaBonds.Add(new TraumaBondRecord(
                            a.Id, BondStrengthPerSharedHazard, hazardId, day));
                    }

                    // Boost affinity
                    AdjustAffinity?.Invoke(a.Id, b.Id, BondAffinityBonus);
                }
            }
        }

        /// <summary>
        /// Get the bonus work efficiency for two survivors on the same shift.
        /// </summary>
        public float GetCoShiftEfficiencyBonus(string survivorA, string survivorB,
            IReadOnlyList<Survivor> survivors)
        {
            if (string.IsNullOrEmpty(survivorA) || string.IsNullOrEmpty(survivorB))
                return 0f;
            if (survivorA == survivorB) return 0f;

            var svA = FindSurvivor(survivorA, survivors);
            if (svA == null) return 0f;

            float bondStrength = GetBondStrength(svA, survivorB);
            if (bondStrength < MinBondStrengthForBonus) return 0f;

            return CoShiftEfficiencyBonus * bondStrength;
        }

        /// <summary>
        /// Tick — decay bond strengths over time.
        /// </summary>
        public void Tick(Survivor sv, float gameHours)
        {
            if (sv == null || sv.TraumaBonds == null) return;

            float dailyDecay = BondDecayPerDay * (gameHours / 24f);
            for (int i = sv.TraumaBonds.Count - 1; i >= 0; i--)
            {
                var bond = sv.TraumaBonds[i];
                bond.BondStrength -= dailyDecay;
                if (bond.BondStrength <= 0f)
                {
                    sv.TraumaBonds.RemoveAt(i);
                }
                else
                {
                    sv.TraumaBonds[i] = bond;
                }
            }
        }

        private float GetBondStrength(Survivor sv, string otherId)
        {
            int idx = FindBondIndex(sv, otherId);
            return idx >= 0 ? sv.TraumaBonds[idx].BondStrength : 0f;
        }

        private int FindBondIndex(Survivor sv, string otherId)
        {
            if (sv?.TraumaBonds == null) return -1;
            for (int i = 0; i < sv.TraumaBonds.Count; i++)
            {
                if (string.Equals(sv.TraumaBonds[i].BondedSurvivorId, otherId,
                    StringComparison.Ordinal))
                    return i;
            }
            return -1;
        }

        private Survivor FindSurvivor(string id, IReadOnlyList<Survivor> survivors)
        {
            if (survivors == null) return null;
            for (int i = 0; i < survivors.Count; i++)
            {
                if (survivors[i] != null && survivors[i].Id == id)
                    return survivors[i];
            }
            return null;
        }
    }
}
