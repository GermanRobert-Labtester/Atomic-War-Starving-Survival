using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class SterileState
    {
        public string afflictionId = "affliction_sterile";
        public float radThreshold = 750f;
        public bool isPermanent = true;
        public List<string> sterileSurvivorIds = new List<string>();
    }

    public class Affliction_Sterile
    {
        public event Action<string> OnSterilityApplied;

        private readonly SterileState _state;

        public Affliction_Sterile()
        {
            _state = new SterileState();
        }

        public bool CheckSterility(string survivorId, float lifetimeRad)
        {
            if (_state.sterileSurvivorIds.Contains(survivorId))
                return true;

            if (lifetimeRad >= _state.radThreshold)
            {
                ApplySterility(survivorId);
                return true;
            }

            return false;
        }

        public bool CanReproduce(string survivorId)
        {
            return !_state.sterileSurvivorIds.Contains(survivorId);
        }

        public void ApplySterility(string survivorId)
        {
            if (_state.sterileSurvivorIds.Contains(survivorId))
                return;

            _state.sterileSurvivorIds.Add(survivorId);
            OnSterilityApplied?.Invoke(survivorId);
        }

        public SterileState CaptureState() => _state;

        public void RestoreState(SterileState state)
        {
            _state.afflictionId = state.afflictionId;
            _state.radThreshold = state.radThreshold;
            _state.isPermanent = state.isPermanent;
            _state.sterileSurvivorIds = new List<string>(state.sterileSurvivorIds);
        }
    }
}
