using System;
using System.Collections.Generic;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class GasPocketState
    {
        public string hazardId = "map_hazard_gas_pockets";
        public float burnDamage = 80f;
        public bool isIgnited = false;
    }

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
        }

        public GasPocketState State => _state;

        public void RegisterGasNode(string nodeId)
        {
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
            _state.isIgnited = false;
        }
    }
}
