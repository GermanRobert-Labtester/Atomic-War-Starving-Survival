using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class NutrientDripState
    {
        public string moduleId = "shelter_module_nutrient_drip";
        public string displayName = "Hydroponic Nutrient Drip";
        public bool isAutomationActive = false;
        public float cropGrowthSpeedMultiplier = 2.0f; // 2x speed
        public string cropYieldType = "tainted_food";
        public float moralePenaltyPerTaintedFood = 5f;
    }

    /// <summary>
    /// Prompt #403: Module: Hydroponic Nutrient Drip.
    /// Replaces CleanWater with ChemicalScrap synthesized into liquid nutrients.
    /// Crops grow 2x faster, but yield TaintedFood (lowers Morale, tastes like plastic).
    /// </summary>
    public class NutrientDripAutomation
    {
        private NutrientDripState _state = new NutrientDripState();

        public event Action<NutrientDripState, string, int> OnTaintedCropsHarvested;

        public NutrientDripState State => _state;

        public bool SynthesizeAndFeed(ref int chemicalScrapCount, int PlanterBoxCount, out int harvestedTaintedFood)
        {
            harvestedTaintedFood = 0;
            if (chemicalScrapCount >= 2 && PlanterBoxCount > 0)
            {
                chemicalScrapCount -= 2;
                _state.isAutomationActive = true;
                harvestedTaintedFood = PlanterBoxCount * 3; // Double yield

                OnTaintedCropsHarvested?.Invoke(_state, _state.cropYieldType, harvestedTaintedFood);
                return true;
            }
            return false;
        }
    }
}
