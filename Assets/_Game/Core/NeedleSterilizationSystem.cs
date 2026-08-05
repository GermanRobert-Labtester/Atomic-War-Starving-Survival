using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class SyringeItemState
    {
        public string id;
        public bool isBoiledSterile = false;
        public bool isReused = false;
    }

    /// <summary>
    /// Prompt #389: System: Blood-Borne Pathogens.
    /// ImprovisedSyringes must be boiled before use. Re-using unboiled syringes
    /// guarantees Hepatitis (slow, permanent stamina drain) or Sepsis.
    /// </summary>
    public class NeedleSterilizationSystem
    {
        public event Action<string, string> OnInfectionContractedFromSyringe;

        public bool BoilSyringe(SyringeItemState syringe)
        {
            if (syringe == null) return false;
            syringe.isBoiledSterile = true;
            return true;
        }

        public bool InjectMedication(SyringeItemState syringe, string survivorId, out string contractedAffliction)
        {
            contractedAffliction = null;
            if (syringe == null) return false;

            if (!syringe.isBoiledSterile || syringe.isReused)
            {
                contractedAffliction = syringe.isReused ? "sepsis_affliction" : "hepatitis_affliction";
                OnInfectionContractedFromSyringe?.Invoke(survivorId, contractedAffliction);
            }

            syringe.isReused = true;
            syringe.isBoiledSterile = false;
            return true;
        }
    }
}
