using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class DecoyState
    {
        public string itemId = "item_decoy";
        public string displayName = "Decoy Grenade";
        public float distractionRadius = 20f;
        public float distractionDurationSeconds = 30f;
        public int requiresBatteries = 1;
        public int requiresElectronics = 1;
    }

    /// <summary>
    /// Prompt #601: Item: Decoy Grenade.
    /// Thrown during Encounter. Draws hostiles to the opposite side,
    /// allowing safe looting or fleeing. Requires Batteries + Electronics to craft.
    /// </summary>
    public class Item_Decoy
    {
        private DecoyState _state = new DecoyState();
        private bool _isDistracting = false;
        private float _remainingSeconds = 0f;
        private string _activeEncounterId;

        public event Action<DecoyState, string, float> OnDecoyDeployed;
        public event Action<DecoyState, string> OnDecoyExpired;

        public DecoyState State => _state;

        public bool CanDeploy(int batteries, int electronics)
        {
            return batteries >= _state.requiresBatteries && electronics >= _state.requiresElectronics;
        }

        public bool Deploy(string encounterId)
        {
            _isDistracting = true;
            _remainingSeconds = _state.distractionDurationSeconds;
            _activeEncounterId = encounterId;

            OnDecoyDeployed?.Invoke(_state, encounterId, _state.distractionRadius);
            return true;
        }

        public bool IsDistracting()
        {
            return _isDistracting;
        }

        public void TickSeconds(float deltaSeconds)
        {
            if (!_isDistracting) return;

            _remainingSeconds -= deltaSeconds;
            if (_remainingSeconds <= 0f)
            {
                _isDistracting = false;
                _remainingSeconds = 0f;
                OnDecoyExpired?.Invoke(_state, _activeEncounterId);
                _activeEncounterId = null;
            }
        }
    }
}
