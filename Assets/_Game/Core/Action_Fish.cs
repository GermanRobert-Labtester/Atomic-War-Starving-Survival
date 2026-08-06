using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class CaughtFish
    {
        public bool isIrradiated;
        public bool isParasiteCarrier;
        public bool vettedIrradiation;
        public bool vettedParasite;
        public float calories = 350f;
    }

    [Serializable]
    public class FishActionResult
    {
        public int totalCaught;
        public List<CaughtFish> fish = new List<CaughtFish>();
        public int irradiatedDetected;
        public int parasiteDetected;
    }

    [Serializable]
    public class FishActionState
    {
        public string actionId = "action_fish";
        public float hoursRequired = 3f;
        public int baseYieldCount = 2;
        public float irradiatedFishChance = 0.10f;
        public float parasiteCarrierChance = 0.05f;
        public float caloriesPerFish = 350f;
    }

    /// <summary>
    /// Prompt #575: Action: River Fishing.
    /// Executed at River nodes. Yields MutatedFish — high calorie protein, but each
    /// fish has a 10% chance to be highly irradiated and a 5% chance to carry parasites.
    /// Players must use a Microbiologist or Geiger counter to vet the food.
    /// </summary>
    public class Action_Fish
    {
        private readonly FishActionState _config = new FishActionState();

        public event Action<int> OnFishCaught;                      // (count)
        public event Action<int> OnIrradiatedFishDetected;          // (count)
        public event Action<int> OnParasiteDetected;                // (count)
        public event Action<string> OnUnsafeFishConsumed;           // (hazardType: "irradiated" or "parasite")

        public FishActionState Config => _config;

        /// <summary>
        /// Execute a fishing action. Must be at a river node to yield fish.
        /// Each fish is individually rolled for irradiation and parasite carriage.
        /// </summary>
        public FishActionResult ExecuteFishing(bool isRiverNode, System.Random rng, bool hasGeigerCounter, bool hasMicrobiologist)
        {
            var result = new FishActionResult();

            if (!isRiverNode || rng == null)
                return result;

            int count = _config.baseYieldCount;
            result.totalCaught = count;
            OnFishCaught?.Invoke(count);

            for (int i = 0; i < count; i++)
            {
                var fish = new CaughtFish
                {
                    isIrradiated = (float)rng.NextDouble() < _config.irradiatedFishChance,
                    isParasiteCarrier = (float)rng.NextDouble() < _config.parasiteCarrierChance,
                    calories = _config.caloriesPerFish
                };

                // Auto-vet if tools are available
                if (fish.isIrradiated && hasGeigerCounter)
                {
                    fish.vettedIrradiation = true;
                    result.irradiatedDetected++;
                }

                if (fish.isParasiteCarrier && hasMicrobiologist)
                {
                    fish.vettedParasite = true;
                    result.parasiteDetected++;
                }

                result.fish.Add(fish);
            }

            if (result.irradiatedDetected > 0)
                OnIrradiatedFishDetected?.Invoke(result.irradiatedDetected);

            if (result.parasiteDetected > 0)
                OnParasiteDetected?.Invoke(result.parasiteDetected);

            return result;
        }

        /// <summary>
        /// Vet a single fish for hazards. Geiger counter detects irradiation;
        /// microbiologist detects parasites. Returns updated vet flags.
        /// </summary>
        public void VetFish(CaughtFish fish, bool hasGeigerCounter, bool hasMicrobiologist)
        {
            if (fish == null) return;

            if (fish.isIrradiated && hasGeigerCounter)
                fish.vettedIrradiation = true;

            if (fish.isParasiteCarrier && hasMicrobiologist)
                fish.vettedParasite = true;
        }

        /// <summary>
        /// Determine the food safety outcome of eating a fish.
        /// Returns a hazard string ("irradiated", "parasite", "both", or "safe").
        /// Vetted hazards are flagged and can be avoided; unvetted hazards cause damage.
        /// Irradiated fish (unvetted): +20 rad dose.
        /// Parasite fish (unvetted): causes dysentery.
        /// </summary>
        public string GetFoodSafety(CaughtFish fish)
        {
            if (fish == null) return "safe";

            bool irradiatedDanger = fish.isIrradiated && !fish.vettedIrradiation;
            bool parasiteDanger = fish.isParasiteCarrier && !fish.vettedParasite;

            if (irradiatedDanger && parasiteDanger)
            {
                OnUnsafeFishConsumed?.Invoke("both");
                return "both";
            }
            if (irradiatedDanger)
            {
                OnUnsafeFishConsumed?.Invoke("irradiated");
                return "irradiated";
            }
            if (parasiteDanger)
            {
                OnUnsafeFishConsumed?.Invoke("parasite");
                return "parasite";
            }

            return "safe";
        }

        /// <summary>
        /// Returns the radiation dose from consuming an unvetted irradiated fish.
        /// </summary>
        public float GetIrradiatedDose()
        {
            return 20f;
        }
    }
}
