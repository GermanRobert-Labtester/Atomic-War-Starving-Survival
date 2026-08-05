using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class CigarettesState
    {
        public string itemId = "item_cigarettes";
        public string displayName = "Cigarettes (Carton)";
        public float stressReliefAmount = 40f;
        public float maxStaminaPenaltyPerSmoke = 2f;
        public int barterValueValue = 50;
    }

    /// <summary>
    /// Prompt #438: Item: Cigarettes (Carton).
    /// Ultimate barter currency. Smoking provides instant Stress/Anxiety relief,
    /// but permanently reduces MaxStamina over time due to lung damage.
    /// </summary>
    public class Item_Cigarettes
    {
        private CigarettesState _state = new CigarettesState();

        public event Action<CigarettesState, string, float, float> OnCigaretteSmoked;

        public CigarettesState State => _state;

        public bool SmokeCigarette(string survivorId, ref float survivorStress, ref float maxStamina)
        {
            survivorStress = Mathf.Max(0f, survivorStress - _state.stressReliefAmount);
            maxStamina = Mathf.Max(20f, maxStamina - _state.maxStaminaPenaltyPerSmoke);

            OnCigaretteSmoked?.Invoke(_state, survivorId, _state.stressReliefAmount, _state.maxStaminaPenaltyPerSmoke);
            return true;
        }
    }
}
