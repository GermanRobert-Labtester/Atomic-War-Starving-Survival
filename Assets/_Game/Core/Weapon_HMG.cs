using System;
using System.Collections.Generic;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class HMGState
    {
        public string weaponId = "weapon_hmg";
        public int requiresOperators = 2;
        public bool isMounted = false;
        public bool isJammed = false;
        public int shredsRaidLevel = 3;
        public string mountLocation;
    }

    public class Weapon_HMG
    {
        public event Action<string, int> OnRaidShredded;
        public event Action<string> OnHMGJammed;

        private HMGState _state;

        public Weapon_HMG()
        {
            _state = new HMGState();
        }

        public Weapon_HMG(HMGState state)
        {
            _state = state ?? new HMGState();
        }

        public HMGState CaptureState() => _state;

        public void RestoreState(HMGState state)
        {
            _state = state ?? new HMGState();
        }

        /// <summary>
        /// Mounts the HMG at a fixed defensive position (airlock or hatch).
        /// The weapon cannot be carried once mounted.
        /// </summary>
        public void Mount(string locationId)
        {
            if (string.IsNullOrEmpty(locationId))
                return;

            _state.isMounted = true;
            _state.mountLocation = locationId;
        }

        /// <summary>
        /// Attempts to fire the HMG at an incoming raid.
        /// Requires 2 operators (1 fires, 1 loads) and the weapon must be oiled.
        /// Shreds raids up to level 3. Jams if not oiled.
        /// </summary>
        /// <param name="locationId">Where the HMG is mounted.</param>
        /// <param name="operatorCount">Number of survivors crewing the weapon.</param>
        /// <param name="isOiled">Whether the weapon has been oiled this cycle.</param>
        /// <param name="raidLevel">The incoming raid's threat level.</param>
        /// <returns>True if the raid was shredded; false otherwise.</returns>
        public bool Fire(string locationId, int operatorCount, bool isOiled, int raidLevel)
        {
            if (string.IsNullOrEmpty(locationId))
                return false;

            if (!_state.isMounted || _state.isJammed)
                return false;

            if (operatorCount < _state.requiresOperators)
                return false;

            // If not oiled the weapon jams — no shot fired
            if (!isOiled)
            {
                _state.isJammed = true;
                OnHMGJammed?.Invoke(locationId);
                return false;
            }

            // Can shred raids up to and including the configured level
            if (raidLevel > _state.shredsRaidLevel)
                return false;

            OnRaidShredded?.Invoke(locationId, raidLevel);
            return true;
        }

        /// <summary>
        /// Clears a jam, allowing the HMG to fire again on the next cycle.
        /// </summary>
        public void Unjam(string locationId)
        {
            if (string.IsNullOrEmpty(locationId))
                return;

            _state.isJammed = false;
        }

        /// <summary>
        /// Returns true when the weapon is mounted, not jammed, and ready to fire.
        /// </summary>
        public bool CanFire()
        {
            return _state.isMounted && !_state.isJammed;
        }

        public bool IsMounted => _state.isMounted;

        public bool IsJammed => _state.isJammed;
    }
}
