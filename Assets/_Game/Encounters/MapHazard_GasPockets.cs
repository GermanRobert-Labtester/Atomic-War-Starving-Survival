using System;
using System.Collections.Generic;

namespace AtomicWar._Game.Encounters
{
    [Serializable]
    public class GasPocketState
    {
        public string hazardId = "map_hazard_gas_pockets";
        public float burnDamage = 80f;
        public bool isIgnited = false;
        public List<string> gasNodes = new List<string>();
        public List<string> ignitedNodes = new List<string>();
    }

    /// <summary>DEMOTE-MapHazard-batch — dormant ghost. Re-promote with Boot+Save+host.</summary>
    public class MapHazard_GasPockets
    {
        public event Action<string, float> OnIgnition; // nodeId, damageToAll
        public event Action<string> OnGasDetected; // survivorId

        private GasPocketState _state;
        private HashSet<string> _gasNodes = new HashSet<string>();
        private HashSet<string> _ignitedNodes = new HashSet<string>();

        public MapHazard_GasPockets()
        {
            _state = new GasPocketState();
        }

        public MapHazard_GasPockets(GasPocketState state)
        {
            _state = state ?? new GasPocketState();
            RebuildSetsFromState();
        }

        public GasPocketState State => _state;

        public void RegisterGasNode(string nodeId)
        {
            if (string.IsNullOrEmpty(nodeId)) return;
            _gasNodes.Add(nodeId);
        }

        public void UnregisterGasNode(string nodeId)
        {
            _gasNodes.Remove(nodeId);
            _ignitedNodes.Remove(nodeId);
        }

        public float FireWeapon(string survivorId, string nodeId, string weaponType, int combatantCount)
        {
            if (weaponType == "firearm" && _gasNodes.Contains(nodeId) && !_ignitedNodes.Contains(nodeId))
            {
                _ignitedNodes.Add(nodeId);
                _state.isIgnited = true;
                float damageToAll = _state.burnDamage;
                OnIgnition?.Invoke(nodeId, damageToAll);
                return damageToAll;
            }

            return 0f;
        }

        public bool DetectGas(string survivorId, bool hasGasDetector)
        {
            if (hasGasDetector)
            {
                OnGasDetected?.Invoke(survivorId);
                return true;
            }

            return false;
        }

        public void ResetIgnition(string nodeId)
        {
            _ignitedNodes.Remove(nodeId);
            _state.isIgnited = _ignitedNodes.Count > 0;
        }

        // ── Save / Load ────────────────────────────────────────────────

        public GasPocketState CaptureState()
        {
            return new GasPocketState
            {
                hazardId = string.IsNullOrEmpty(_state.hazardId) ? "map_hazard_gas_pockets" : _state.hazardId,
                burnDamage = _state.burnDamage,
                isIgnited = _ignitedNodes.Count > 0,
                gasNodes = new List<string>(_gasNodes),
                ignitedNodes = new List<string>(_ignitedNodes)
            };
        }

        public void RestoreState(GasPocketState saved)
        {
            _state = saved != null
                ? new GasPocketState
                {
                    hazardId = string.IsNullOrEmpty(saved.hazardId) ? "map_hazard_gas_pockets" : saved.hazardId,
                    burnDamage = saved.burnDamage,
                    isIgnited = saved.isIgnited,
                    gasNodes = saved.gasNodes != null ? new List<string>(saved.gasNodes) : new List<string>(),
                    ignitedNodes = saved.ignitedNodes != null ? new List<string>(saved.ignitedNodes) : new List<string>()
                }
                : new GasPocketState();
            RebuildSetsFromState();
        }

        private void RebuildSetsFromState()
        {
            _gasNodes.Clear();
            _ignitedNodes.Clear();
            if (_state.gasNodes != null)
            {
                for (int i = 0; i < _state.gasNodes.Count; i++)
                    if (!string.IsNullOrEmpty(_state.gasNodes[i]))
                        _gasNodes.Add(_state.gasNodes[i]);
            }
            if (_state.ignitedNodes != null)
            {
                for (int i = 0; i < _state.ignitedNodes.Count; i++)
                    if (!string.IsNullOrEmpty(_state.ignitedNodes[i]))
                        _ignitedNodes.Add(_state.ignitedNodes[i]);
            }
            _state.isIgnited = _ignitedNodes.Count > 0;
        }
    }
}
