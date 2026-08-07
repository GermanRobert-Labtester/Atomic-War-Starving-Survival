using System;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class CaveMadnessState
    {
        public string survivorId;
        public int daysBelowLevel4;
        public int depthThresholdDays = 10;
        public float moraleDrainPerDay = -5f;
        public bool isMad;
    }

    public class CaveMadnessSystem
    {
        private readonly CaveMadnessState _state;

        public CaveMadnessState State => _state;

        public event Action<string, float> OnMoraleDrained;  // survivorId, amount
        public event Action<string> OnMadnessTriggered;      // survivorId
        public event Action<string> OnMadnessCured;          // survivorId

        public CaveMadnessSystem(string survivorId)
        {
            _state = new CaveMadnessState
            {
                survivorId = survivorId,
                daysBelowLevel4 = 0,
                depthThresholdDays = 10,
                moraleDrainPerDay = -5f,
                isMad = false
            };
        }

        /// <summary>
        /// Daily tick. Accumulates deep-earth days below Level 4 and drains morale.
        /// </summary>
        public void TickDay(int currentBunkerLevel)
        {
            if (currentBunkerLevel >= 4)
            {
                _state.daysBelowLevel4++;
                OnMoraleDrained?.Invoke(_state.survivorId, _state.moraleDrainPerDay);

                if (_state.daysBelowLevel4 >= _state.depthThresholdDays && !_state.isMad)
                {
                    _state.isMad = true;
                    OnMadnessTriggered?.Invoke(_state.survivorId);
                }
            }
            else
            {
                _state.daysBelowLevel4 = 0;
            }
        }

        /// <summary>
        /// Cure by reassigning to a surface-level room.
        /// </summary>
        public void ReassignToSurface()
        {
            if (_state.isMad)
            {
                _state.isMad = false;
                _state.daysBelowLevel4 = 0;
                OnMadnessCured?.Invoke(_state.survivorId);
            }
        }

        public bool IsHallucinating()
        {
            return _state.isMad;
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public CaveMadnessState CaptureState()
        {
            return new CaveMadnessState
            {
                survivorId = _state.survivorId,
                daysBelowLevel4 = _state.daysBelowLevel4,
                depthThresholdDays = _state.depthThresholdDays,
                moraleDrainPerDay = _state.moraleDrainPerDay,
                isMad = _state.isMad,
            };
        }

        public void RestoreState(CaveMadnessState saved)
        {
            if (saved == null) return;
            _state.survivorId = saved.survivorId;
            _state.daysBelowLevel4 = saved.daysBelowLevel4;
            _state.depthThresholdDays = saved.depthThresholdDays;
            _state.moraleDrainPerDay = saved.moraleDrainPerDay;
            _state.isMad = saved.isMad;
        }

}
}
