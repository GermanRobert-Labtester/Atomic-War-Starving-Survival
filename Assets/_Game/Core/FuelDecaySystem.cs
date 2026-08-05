using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class FuelDecayState
    {
        public int daysElapsed = 0;
        public float fuelEfficiencyMultiplier = 1.0f;
        public int bioFuelStillsActive = 0;
    }

    /// <summary>
    /// Prompt #380: System: Fuel Degradation.
    /// PreWarGasoline degrades into varnish, losing 10% efficiency every 30 days.
    /// Players can build BioFuelStills to refine Moonshine or Fungi into usable biofuel.
    /// </summary>
    public class FuelDecaySystem
    {
        private FuelDecayState _state = new FuelDecayState();

        public event Action<FuelDecayState, float> OnFuelEfficiencyDegraded;
        public event Action<FuelDecayState, int> OnBioFuelRefined;

        public FuelDecayState State => _state;

        public void TickDaily(int currentDay)
        {
            _state.daysElapsed = currentDay;
            int intervals = currentDay / 30;
            float degradation = intervals * 0.10f;
            _state.fuelEfficiencyMultiplier = Mathf.Max(0.20f, 1.0f - degradation);

            OnFuelEfficiencyDegraded?.Invoke(_state, _state.fuelEfficiencyMultiplier);
        }

        public int RefineBioFuel(int moonshineCount, int fungiCount)
        {
            int refinedFuel = moonshineCount + (fungiCount / 2);
            if (refinedFuel > 0)
            {
                OnBioFuelRefined?.Invoke(_state, refinedFuel);
            }
            return refinedFuel;
        }
    }
}
