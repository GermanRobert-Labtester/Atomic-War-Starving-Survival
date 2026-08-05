using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class SevereBurnState
    {
        public string survivorId;
        public bool hasSevereBurns = false;
        public bool isSkinGraftCompleted = false;
        public float infectionRateMultiplier = 3.0f;
    }

    /// <summary>
    /// Prompt #395: System: The Burn Ward.
    /// FireEntities inflict SevereBurns. Medical Bed must be upgraded to a SterileEnvironment
    /// to perform SkinGraft surgery, or the patient dies of burn shock.
    /// </summary>
    public class BurnWardSystem
    {
        private readonly Dictionary<string, SevereBurnState> _burnMap = new Dictionary<string, SevereBurnState>();

        public event Action<string> OnSevereBurnsContracted;
        public event Action<string> OnSkinGraftSuccessful;
        public event Action<string> OnPatientDiedOfBurnShock;

        public IReadOnlyDictionary<string, SevereBurnState> BurnMap => _burnMap;

        public void InflictSevereBurns(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return;
            var state = new SevereBurnState { survivorId = survivorId, hasSevereBurns = true };
            _burnMap[survivorId] = state;

            OnSevereBurnsContracted?.Invoke(survivorId);
        }

        public bool PerformSkinGraftSurgery(string survivorId, bool isMedicalBedSterile, float roomHygienePercent)
        {
            if (_burnMap.TryGetValue(survivorId, out var state) && state.hasSevereBurns)
            {
                if (isMedicalBedSterile && roomHygienePercent >= 1.0f)
                {
                    state.isSkinGraftCompleted = true;
                    state.hasSevereBurns = false;
                    OnSkinGraftSuccessful?.Invoke(survivorId);
                    return true;
                }
                else
                {
                    OnPatientDiedOfBurnShock?.Invoke(survivorId);
                    return false;
                }
            }
            return false;
        }
    }
}
