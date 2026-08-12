using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Utilities;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Expansion IX — Procedural Scavenge System. Locations never have static spawns.
    /// Uses weighted Poisson distribution to roll quantities between Min and Max,
    /// factoring in WorldPhase (longer game = closer to Min). Environmental
    /// degradation means the same location yields less as time passes.
    /// Save/load safe. Plain C#.
    /// </summary>
    public class ProceduralScavengeSystem
    {
        // ── Degradation constants ─────────────────────────────────────
        public const int DegradationPhase1_Day = 20;  // Pristine
        public const int DegradationPhase2_Day = 50;  // Degraded
        public const int DegradationPhase3_Day = 80;  // Ruined

        // ── Poisson distribution parameters ───────────────────────────
        public const float PoissonLambdaBase = 1.5f;
        public const float DegradationSkewFactor = 0.6f; // Phase 3 skews 60% toward Min

        // ── Contamination thresholds ──────────────────────────────────
        public const float HighRadThreshold = 15f; // mSv/h
        public const float BioHazardThreshold = 0.5f;

        // ── Events ────────────────────────────────────────────────────
        public event Action<string, string, int> OnLootRolled;         // locationId, itemId, qty
        public event Action<string, string> OnItemDegraded;            // locationId, itemId
        public event Action<string, string> OnContaminationApplied;    // locationId, itemId

        private readonly System.Random _rng;
        private int _currentDay;
        private readonly Dictionary<string, int> _locationVisitCounts = new Dictionary<string, int>();

        public ProceduralScavengeSystem(System.Random rng = null)
        {
            _rng = rng ?? new System.Random(9999);
        }

        public void SetCurrentDay(int day) => _currentDay = day;

        // ── Variable loot rolling ─────────────────────────────────────

        /// <summary>
        /// Roll a variable loot table for a location. Returns item yields.
        /// Uses Poisson distribution skewed by world phase.
        /// </summary>
        public List<LootRollResult> RollLootTable(string locationId,
            List<VariableLootNode> lootTable, float locationRads,
            bool hasBioHazard)
        {
            var results = new List<LootRollResult>();

            // Track visits for degradation
            _locationVisitCounts.TryGetValue(locationId, out var visits);
            _locationVisitCounts[locationId] = visits + 1;

            for (int i = 0; i < lootTable.Count; i++)
            {
                var node = lootTable[i];

                // Spawn chance roll
                if (_rng.NextDouble() > node.SpawnChance)
                    continue;

                // Roll quantity using degraded Poisson
                int qty = RollQuantity(node.MinQty, node.MaxQty, _currentDay);

                if (qty <= 0) continue;

                // Degradation check
                bool degraded = false;
                if (node.DegradationChance > 0f && _rng.NextDouble() < node.DegradationChance)
                {
                    degraded = true;
                    qty = Mathf.Max(1, qty / 2); // Half yield on degradation
                    OnItemDegraded?.Invoke(locationId, node.ItemId);
                }

                // Contamination check
                bool contaminated = false;
                if (locationRads >= HighRadThreshold || hasBioHazard)
                {
                    contaminated = true;
                    OnContaminationApplied?.Invoke(locationId, node.ItemId);
                }

                results.Add(new LootRollResult
                {
                    ItemId = node.ItemId,
                    Quantity = qty,
                    IsDegraded = degraded,
                    IsContaminated = contaminated,
                    DegradedItemId = degraded ? node.DegradedItemId : null
                });

                OnLootRolled?.Invoke(locationId, node.ItemId, qty);
            }

            return results;
        }

        /// <summary>
        /// Roll quantity using Poisson distribution, skewed by world phase.
        /// Phase 1: uniform. Phase 2: slight skew toward min. Phase 3: heavy skew.
        /// </summary>
        private int RollQuantity(int min, int max, int currentDay)
        {
            if (min >= max) return min;

            float skew = GetDegradationSkew(currentDay);
            float range = max - min;

            // Poisson-distributed roll with incremental computation.
            // pmf(k) = e^(-lambda) * lambda^k / k!
            // pmf(k+1) = pmf(k) * lambda / (k+1)  (incremental recurrence)
            double u = _rng.NextDouble();
            double lambda = PoissonLambdaBase;
            double pmf = Math.Exp(-lambda);   // pmf(0)
            double cumulative = pmf;
            int rawRoll = 0;
            while (u > cumulative && rawRoll < max)
            {
                rawRoll++;
                pmf = pmf * lambda / rawRoll;  // incremental: avoids Pow + Factorial
                cumulative += pmf;
            }

            // Normalize to range
            float normalized = Mathf.Clamp01((float)rawRoll / Mathf.Max(1, max - min));

            // Apply skew (pushes toward min)
            float skewed = normalized * (1f - skew);

            int result = min + Mathf.RoundToInt(skewed * range);
            return Mathf.Clamp(result, min, max);
        }

        private float GetDegradationSkew(int day)
        {
            if (day >= DegradationPhase3_Day) return DegradationSkewFactor;
            if (day >= DegradationPhase2_Day) return DegradationSkewFactor * 0.5f;
            return 0f;
        }

        // ── Container degradation ─────────────────────────────────────

        /// <summary>
        /// Check if a container is degraded enough to change its loot profile.
        /// </summary>
        public ContainerState GetContainerState(int spawnDay)
        {
            int age = _currentDay - spawnDay;
            if (age < DegradationPhase1_Day) return ContainerState.Pristine;
            if (age < DegradationPhase2_Day) return ContainerState.Degraded;
            return ContainerState.Ruined;
        }

        /// <summary>Get the effective spawn chance multiplier for a container state.</summary>
        public float GetContainerChanceMultiplier(ContainerState state)
        {
            return state switch
            {
                ContainerState.Pristine => 1.0f,
                ContainerState.Degraded => 0.6f,
                ContainerState.Ruined => 0.2f,
                _ => 1f
            };
        }

        // ── Encumbrance calculations ──────────────────────────────────

        /// <summary>
        /// Calculate traversal speed penalty for carried weight.
        /// Over 25kg = massive penalty. Over 60kg = must use sled.
        /// </summary>
        public float GetTraversalSpeedMultiplier(float carriedKg, bool hasSled)
        {
            if (hasSled && carriedKg <= 60f) return 0.8f; // Slight drag
            if (carriedKg > 60f) return 0.1f; // Near-immobile without sled
            if (carriedKg > 25f) return 0.4f; // Heavy
            return 1f;
        }

        /// <summary>Get fatigue multiplier for carried weight.</summary>
        public float GetEncumbranceFatigueMultiplier(float carriedKg)
        {
            if (carriedKg > 60f) return 4f;
            if (carriedKg > 25f) return 2f;
            return 1f;
        }

        // ── Contamination vector ──────────────────────────────────────

        /// <summary>
        /// Check if loot from a location needs decontamination before storage.
        /// </summary>
        public bool NeedsDecontamination(float locationRads, bool hasBioHazard)
        {
            return locationRads >= HighRadThreshold || hasBioHazard;
        }

        /// <summary>
        /// Decontaminate items. Returns how many survive the wash.
        /// </summary>
        public int Decontaminate(int itemCount, bool hasSoap, bool hasCleanWater)
        {
            if (!hasSoap || !hasCleanWater) return itemCount; // Can't wash
            // 40% loss during decontamination
            float survivalRate = 0.60f;
            return Mathf.Max(1, Mathf.RoundToInt(itemCount * survivalRate));
        }

        // ── Save / Load ───────────────────────────────────────────────

        public ProceduralScavengeSave CaptureState()
        {
            var visits = new LocationVisitSave[_locationVisitCounts.Count];
            int i = 0;
            foreach (var kv in _locationVisitCounts)
                visits[i++] = new LocationVisitSave { LocationId = kv.Key, Visits = kv.Value };
            return new ProceduralScavengeSave { CurrentDay = _currentDay, LocationVisits = visits };
        }

        public void RestoreState(ProceduralScavengeSave save)
        {
            _locationVisitCounts.Clear();
            _currentDay = 0;
            if (save == null) return;
            _currentDay = save.CurrentDay;
            if (save.LocationVisits != null)
                for (int i = 0; i < save.LocationVisits.Length; i++)
                    if (save.LocationVisits[i] != null)
                        _locationVisitCounts[save.LocationVisits[i].LocationId] = save.LocationVisits[i].Visits;
        }
    }

    // ── Data types ────────────────────────────────────────────────────

    [Serializable]
    public class LootRollResult
    {
        public string ItemId;
        public int Quantity;
        public bool IsDegraded;
        public bool IsContaminated;
        public string DegradedItemId;
    }

    public enum ContainerState
    {
        Pristine,
        Degraded,
        Ruined
    }

    [Serializable]
    public class ProceduralScavengeSave
    {
        public int CurrentDay;
        public LocationVisitSave[] LocationVisits;
    }

    [Serializable]
    public class LocationVisitSave
    {
        public string LocationId;
        public int Visits;
    }
}
