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
            if (currentDay < 0) currentDay = 0;
            if (_state == null) _state = new FuelDecayState();

            float previous = _state.fuelEfficiencyMultiplier;
            _state.daysElapsed = currentDay;
            int intervals = currentDay / 30;
            float degradation = intervals * 0.10f;
            _state.fuelEfficiencyMultiplier = Mathf.Max(0.20f, 1.0f - degradation);

            // Only raise when efficiency actually changes (avoids spam every day after day 0).
            if (!Mathf.Approximately(previous, _state.fuelEfficiencyMultiplier))
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

        // -----------------------------------------------------------------
        // Save / Load
        // -----------------------------------------------------------------

        /// <summary>Deep-copy efficiency state for ISaveable capture (id: "fuel_decay").</summary>
        public FuelDecayState CaptureState()
        {
            if (_state == null) _state = new FuelDecayState();
            return new FuelDecayState
            {
                daysElapsed = _state.daysElapsed,
                fuelEfficiencyMultiplier = _state.fuelEfficiencyMultiplier,
                bioFuelStillsActive = _state.bioFuelStillsActive
            };
        }

        /// <summary>Replace state from snapshot. Null resets to defaults.</summary>
        public void RestoreState(FuelDecayState state)
        {
            if (state == null)
            {
                _state = new FuelDecayState();
                return;
            }
            _state = new FuelDecayState
            {
                daysElapsed = state.daysElapsed,
                fuelEfficiencyMultiplier = state.fuelEfficiencyMultiplier,
                bioFuelStillsActive = state.bioFuelStillsActive
            };
        }
    }
}
