using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Shelter.Modules
{
    [Serializable]
    public class GunRackState
    {
        public string moduleId = "shelter_module_gun_rack";
        public string displayName = "The Weapon Rack";
        public bool isBuilt = false;
        public bool isLocked = true;
        public List<string> lockedWeapons = new List<string>();
    }

    /// <summary>
    /// Prompt #446: Module: The Weapon Rack.
    /// Provides secure locked weapon storage. Prevents survivors with Paranoia or Fugue breaks
    /// from stealing firearms. Requires explicit player authorization to arm the crew.
    /// </summary>
    public class ShelterModule_GunRack
    {
        private GunRackState _state = new GunRackState();

        public event Action<GunRackState, string> OnWeaponLockedAway;
        public event Action<GunRackState, string, string> OnWeaponIssuedToSurvivor;

        public GunRackState State => _state;

        public void StoreWeaponInRack(string weaponId)
        {
            if (!_state.isBuilt) return;
            _state.lockedWeapons.Add(weaponId);
            OnWeaponLockedAway?.Invoke(_state, weaponId);
        }

        public bool IssueWeaponToSurvivor(string weaponId, string survivorId)
        {
            if (_state.isBuilt && _state.isLocked && _state.lockedWeapons.Remove(weaponId))
            {
                OnWeaponIssuedToSurvivor?.Invoke(_state, weaponId, survivorId);
                return true;
            }
            return false;
        }

        public bool PreventParanoiaTheft(string weaponId)
        {
            return _state.isBuilt && _state.isLocked && _state.lockedWeapons.Contains(weaponId);
        }
    
        public GunRackState CaptureState()
        {
            return _state;
        }

        public void RestoreState(GunRackState saved)
        {
            _state = saved ?? new GunRackState();
            if (_state.lockedWeapons == null)
                _state.lockedWeapons = new System.Collections.Generic.List<string>();
        }
    }
}

