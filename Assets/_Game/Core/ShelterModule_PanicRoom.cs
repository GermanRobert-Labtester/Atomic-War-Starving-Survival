using System;
using System.Collections.Generic;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class PanicRoomState
    {
        public string moduleId;
        public bool isBuilt;
        public int constructionDays = 15;
        public int maxOccupants = 4;
        public bool isLocked;
        public int siegeLevelRequired = 5;
        public List<string> lockedOccupantIds = new List<string>();
    }

    public class PanicRoomSystem
    {
        private readonly PanicRoomState _state;

        public PanicRoomState State => _state;

        public event Action<string> OnBuilt;                 // moduleId
        public event Action<string> OnLocked;                // moduleId
        public event Action<string> OnReleased;              // moduleId
        public event Action<string, bool> OnSiegeSurvival;   // moduleId, active

        public PanicRoomSystem(string moduleId)
        {
            _state = new PanicRoomState
            {
                moduleId = moduleId,
                isBuilt = false,
                constructionDays = 15,
                maxOccupants = 4,
                isLocked = false,
                siegeLevelRequired = 5,
                lockedOccupantIds = new List<string>()
            };
        }

        /// <summary>
        /// Complete construction of the reinforced panic room.
        /// </summary>
        public void Build()
        {
            _state.isBuilt = true;
            OnBuilt?.Invoke(_state.moduleId);
        }

        /// <summary>
        /// Lock best survivors inside during a siege. Max 4 occupants.
        /// </summary>
        public bool LockOccupants(List<string> survivorIds)
        {
            if (!_state.isBuilt || _state.isLocked)
                return false;

            int count = Math.Min(survivorIds.Count, _state.maxOccupants);
            _state.lockedOccupantIds = new List<string>(survivorIds.GetRange(0, count));
            _state.isLocked = true;
            OnLocked?.Invoke(_state.moduleId);
            return true;
        }

        /// <summary>
        /// Check if siege survival is active (locked during a Level 5+ siege).
        /// </summary>
        public bool IsSiegeSurvivalActive(int currentSiegeLevel)
        {
            bool active = _state.isLocked && currentSiegeLevel >= _state.siegeLevelRequired;
            OnSiegeSurvival?.Invoke(_state.moduleId, active);
            return active;
        }

        /// <summary>
        /// Release occupants after siege ends.
        /// </summary>
        public List<string> ReleaseOccupants()
        {
            var released = new List<string>(_state.lockedOccupantIds);
            _state.lockedOccupantIds.Clear();
            _state.isLocked = false;
            OnReleased?.Invoke(_state.moduleId);
            return released;
        }
    
        public PanicRoomState CaptureState()
        {
            return new PanicRoomState
            {
                moduleId = _state.moduleId,
                isBuilt = _state.isBuilt,
                constructionDays = _state.constructionDays,
                maxOccupants = _state.maxOccupants,
                isLocked = _state.isLocked,
                siegeLevelRequired = _state.siegeLevelRequired,
                lockedOccupantIds = _state.lockedOccupantIds != null ? new System.Collections.Generic.List<string>(_state.lockedOccupantIds) : new System.Collections.Generic.List<string>()
            };
        }

        public void RestoreState(PanicRoomState saved)
        {
            if (saved == null) return;
            _state.moduleId = saved.moduleId;
            _state.isBuilt = saved.isBuilt;
            _state.constructionDays = saved.constructionDays;
            _state.maxOccupants = saved.maxOccupants;
            _state.isLocked = saved.isLocked;
            _state.siegeLevelRequired = saved.siegeLevelRequired;
            _state.lockedOccupantIds.Clear();
            if (saved.lockedOccupantIds != null)
                _state.lockedOccupantIds.AddRange(saved.lockedOccupantIds);
        }
    }
}

