using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class ToothDecayState
    {
        public string survivorId;
        public bool hasToothache = false;
        public float painAmount = 50f;
        public bool preventsSleep = true;
    }

    /// <summary>
    /// Prompt #392: System: Dentistry & Tooth Decay.
    /// JunkFood and low Hygiene cause Toothache (inflicts massive Pain and blocks Sleep).
    /// Cured by PullTooth action at the Medical Bed (requires Pliers and Whiskey, traumatizes the patient).
    /// </summary>
    public class ToothDecaySystem
    {
        private readonly Dictionary<string, ToothDecayState> _teethMap = new Dictionary<string, ToothDecayState>();

        public event Action<string> OnToothacheContracted;
        public event Action<string, float> OnToothPulledTraumatized;

        public IReadOnlyDictionary<string, ToothDecayState> TeethMap => _teethMap;

        public void ContractToothache(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return;
            var state = new ToothDecayState { survivorId = survivorId, hasToothache = true };
            _teethMap[survivorId] = state;

            OnToothacheContracted?.Invoke(survivorId);
        }

        public bool PullTooth(string survivorId, bool hasPliers, bool hasWhiskey, ref float survivorTrauma)
        {
            if (_teethMap.TryGetValue(survivorId, out var state) && state.hasToothache && hasPliers && hasWhiskey)
            {
                state.hasToothache = false;
                survivorTrauma += 25f; // Pulling tooth without anesthesia traumatizes patient

                OnToothPulledTraumatized?.Invoke(survivorId, survivorTrauma);
                return true;
            }
            return false;
        }
    }
}
