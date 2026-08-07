using System;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class ThumperState
    {
        public string moduleId;
        public float powerRequired = 150f;
        public float noiseGenerated = 80f;
        public bool preventsBurrowers = true;
        public bool stabilizesFaults = true;
        public bool isActive;
    }

    public class ThumperSystem
    {
        private readonly ThumperState _state;

        public ThumperState State => _state;

        public event Action<string, bool> OnThumperToggled;  // moduleId, isActive
        public event Action<string> OnThumperTick;           // moduleId

        public ThumperSystem(string moduleId)
        {
            _state = new ThumperState
            {
                moduleId = moduleId,
                powerRequired = 150f,
                noiseGenerated = 80f,
                preventsBurrowers = true,
                stabilizesFaults = true,
                isActive = false
            };
        }

        /// <summary>
        /// Activate/deactivate the thumper. Requires sufficient power.
        /// </summary>
        public bool Activate(bool hasPower)
        {
            _state.isActive = hasPower;
            OnThumperToggled?.Invoke(_state.moduleId, _state.isActive);
            return _state.isActive;
        }

        /// <summary>
        /// Hourly tick. Raises noise event while active.
        /// </summary>
        public void TickHour()
        {
            if (_state.isActive)
                OnThumperTick?.Invoke(_state.moduleId);
        }

        public float GetNoiseLevel()
        {
            return _state.isActive ? _state.noiseGenerated : 0f;
        }

        public bool IsBurrowerProtected()
        {
            return _state.isActive && _state.preventsBurrowers;
        }
    
        public ThumperState CaptureState()
        {
            return new ThumperState
            {
                moduleId = _state.moduleId,
                powerRequired = _state.powerRequired,
                noiseGenerated = _state.noiseGenerated,
                preventsBurrowers = _state.preventsBurrowers,
                stabilizesFaults = _state.stabilizesFaults,
                isActive = _state.isActive
            };
        }

        public void RestoreState(ThumperState saved)
        {
            if (saved == null) return;
            _state.moduleId = saved.moduleId;
            _state.powerRequired = saved.powerRequired;
            _state.noiseGenerated = saved.noiseGenerated;
            _state.preventsBurrowers = saved.preventsBurrowers;
            _state.stabilizesFaults = saved.stabilizesFaults;
            _state.isActive = saved.isActive;
        }
    }
}

