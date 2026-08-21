using System;
using System.Collections.Generic;

namespace Ashfall.Core.Maritime
{
    /// <summary>
    /// ASHFALL: THE BLACK FLOTILLA (Expansion 09) — procedural scavenge system.
    /// Locations never have static spawns. Uses weighted Poisson distribution
    /// to roll quantities between Min and Max, factoring in world phase (longer
    /// game = closer to Min) and per-location visit count (picked-over effect).
    /// Environmental degradation means the same location yields less over time.
    /// Engine-agnostic, deterministic via ISeededRng, save/load safe.
    /// </summary>
    public class ProceduralScavengeSystem
    {
        public const int DegradationPhase1_Day = 20;
        public const int DegradationPhase2_Day = 50;
        public const int DegradationPhase3_Day = 80;

        public const float PoissonLambdaBase = 1.5f;
        public const float DegradationSkewFactor = 0.6f;

        public const float HighRadThreshold = 15f;
        public const float BioHazardThreshold = 0.5f;

        public event Action<string, string, int> OnLootRolled;
        public event Action<string, string> OnItemDegraded;
        public event Action<string, string> OnContaminationApplied;

        private readonly ISeededRng _rng;
        private int _currentDay;
        private readonly Dictionary<string, int> _locationVisitCounts = new Dictionary<string, int>(StringComparer.Ordinal);

        public ProceduralScavengeSystem(ISeededRng rng = null)
        {
            _rng = rng ?? new SeededRng(9999);
        }

        public void SetCurrentDay(int day) => _currentDay = day;
        public int GetVisitCount(string locationId) => _locationVisitCounts.TryGetValue(locationId, out var v) ? v : 0;

        public List<LootRollResult> RollLootTable(string locationId,
            List<VariableLootNode> lootTable, float locationRads, bool hasBioHazard)
        {
            var results = new List<LootRollResult>();
            if (string.IsNullOrEmpty(locationId) || lootTable == null) return results;

            _locationVisitCounts.TryGetValue(locationId, out var visits);
            _locationVisitCounts[locationId] = visits + 1;

            for (int i = 0; i < lootTable.Count; i++)
            {
                var node = lootTable[i];
                if (node == null || string.IsNullOrEmpty(node.ItemId)) continue;

                if (_rng.NextDouble() > node.SpawnChance) continue;

                int qty = RollQuantity(node.MinQty, node.MaxQty, _currentDay, visits);
                if (qty <= 0) continue;

                bool degraded = false;
                if (node.DegradationChance > 0f && _rng.NextDouble() < node.DegradationChance)
                {
                    degraded = true;
                    qty = MathfCompat.Max(1, qty / 2);
                    OnItemDegraded?.Invoke(locationId, node.ItemId);
                }

                bool contaminated = locationRads >= HighRadThreshold || hasBioHazard;
                if (contaminated) OnContaminationApplied?.Invoke(locationId, node.ItemId);

                results.Add(new LootRollResult
                {
                    ItemId = node.ItemId,
                    Quantity = qty,
                    IsDegraded = degraded,
                    IsContaminated = contaminated,
                    DegradedItemId = degraded && !string.IsNullOrEmpty(node.DegradedItemId)
                        ? node.DegradedItemId
                        : null!
                });

                OnLootRolled?.Invoke(locationId, node.ItemId, qty);
            }
            return results;
        }

        private int RollQuantity(int min, int max, int currentDay, int visitCount)
        {
            if (min >= max) return min;

            float skew = GetDegradationSkew(currentDay);
            float visitSkew = MathfCompat.Clamp01(visitCount * 0.05f) * 0.30f;
            skew = MathfCompat.Clamp(skew + visitSkew, 0f, 0.9f);
            float range = max - min;

            double u = _rng.NextDouble();
            double lambda = PoissonLambdaBase;
            double pmf = Math.Exp(-lambda);
            double cumulative = pmf;
            int rawRoll = 0;
            while (u > cumulative && rawRoll < max)
            {
                rawRoll++;
                pmf = pmf * lambda / rawRoll;
                cumulative += pmf;
            }

            float normalized = MathfCompat.Clamp01((float)rawRoll / MathfCompat.Max(1, max - min));
            float skewed = normalized * (1f - skew);
            int result = min + MathfCompat.RoundToInt(skewed * range);
            return MathfCompat.Clamp(result, min, max);
        }

        private float GetDegradationSkew(int day)
        {
            if (day >= DegradationPhase3_Day) return DegradationSkewFactor;
            if (day >= DegradationPhase2_Day) return DegradationSkewFactor * 0.5f;
            return 0f;
        }

        public ContainerState GetContainerState(int spawnDay)
        {
            int age = _currentDay - spawnDay;
            if (age < DegradationPhase1_Day) return ContainerState.Pristine;
            if (age < DegradationPhase2_Day) return ContainerState.Degraded;
            return ContainerState.Ruined;
        }

        public float GetContainerChanceMultiplier(ContainerState state)
        {
            switch (state)
            {
                case ContainerState.Pristine: return 1.0f;
                case ContainerState.Degraded: return 0.6f;
                case ContainerState.Ruined: return 0.2f;
                default: return 1f;
            }
        }

        public float GetTraversalSpeedMultiplier(float carriedKg, bool hasSled)
        {
            if (hasSled && carriedKg <= 60f) return 0.8f;
            if (carriedKg > 60f) return 0.1f;
            if (carriedKg > 25f) return 0.4f;
            return 1f;
        }

        public float GetEncumbranceFatigueMultiplier(float carriedKg)
        {
            if (carriedKg > 60f) return 4f;
            if (carriedKg > 25f) return 2f;
            return 1f;
        }

        public bool NeedsDecontamination(float locationRads, bool hasBioHazard)
            => locationRads >= HighRadThreshold || hasBioHazard;

        public int Decontaminate(int itemCount, bool hasSoap, bool hasCleanWater)
        {
            if (!hasSoap || !hasCleanWater) return 0;
            return MathfCompat.Max(1, MathfCompat.RoundToInt(itemCount * 0.60f));
        }

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
                    if (save.LocationVisits[i] != null && !string.IsNullOrEmpty(save.LocationVisits[i].LocationId))
                        _locationVisitCounts[save.LocationVisits[i].LocationId] = save.LocationVisits[i].Visits;
        }
    }

    [Serializable]
    public class LootRollResult
    {
        public string ItemId = string.Empty;
        public int Quantity;
        public bool IsDegraded;
        public bool IsContaminated;
        public string DegradedItemId = string.Empty;
    }

    public enum ContainerState { Pristine, Degraded, Ruined }

    [Serializable]
    public class ProceduralScavengeSave
    {
        public int CurrentDay;
        public LocationVisitSave[] LocationVisits;
    }

    [Serializable]
    public class LocationVisitSave
    {
        public string LocationId = string.Empty;
        public int Visits;
    }
}
