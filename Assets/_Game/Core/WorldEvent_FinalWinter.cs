using System;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class FinalWinterState
    {
        public string eventId = "world_event_final_winter";
        public int triggerDay = 100;
        public bool isActive = false;
        public float currentTemperature = -40f;
        public int daysRemaining = 30;
        public bool cropsDestroyed = false;
        public bool surfaceWaterFrozen = false;
        public int bunkerFreezeDeadline = -1;
    }

    /// <summary>
    /// Prompt #560: World Event — The Final Winter.
    /// Triggers on Day 100. Global temperature drops to -40°C and never recovers.
    /// All surface water freezes solid; all crops die. The player has a 30-day
    /// ticking clock to achieve Victory before fuel runs out and the bunker
    /// freezes forever. Save/load safe. Plain C#.
    /// </summary>
    public class WorldEvent_FinalWinter
    {
        private FinalWinterState _state = new FinalWinterState();

        // -- Events --
        public event Action<FinalWinterState> OnFinalWinterTriggered;
        public event Action<FinalWinterState, int> OnFreezeDeadlineWarning;
        public event Action<FinalWinterState> OnBunkerFrozen;

        public FinalWinterState State => _state;

        /// <summary>
        /// Checks whether the Final Winter should activate. Activates on the
        /// configured trigger day if not already active.
        /// </summary>
        public void CheckActivation(int currentDay)
        {
            if (_state.isActive) return;

            if (currentDay >= _state.triggerDay)
            {
                _state.isActive = true;
                _state.cropsDestroyed = true;
                _state.surfaceWaterFrozen = true;
                _state.bunkerFreezeDeadline = currentDay + _state.daysRemaining;

                OnFinalWinterTriggered?.Invoke(_state);
            }
        }

        /// <summary>
        /// Called once per game-day while active. Decrements the remaining days
        /// and fires warning/freeze events as the deadline approaches.
        /// </summary>
        public void TickDay(int currentDay)
        {
            if (!_state.isActive) return;

            _state.daysRemaining = Math.Max(0, _state.bunkerFreezeDeadline - currentDay);

            // Warning at 7 days remaining.
            if (_state.daysRemaining <= 7 && _state.daysRemaining > 0)
            {
                OnFreezeDeadlineWarning?.Invoke(_state, _state.daysRemaining);
            }

            if (_state.daysRemaining <= 0)
            {
                OnBunkerFrozen?.Invoke(_state);
            }
        }

        /// <summary>Returns -40°C when active; otherwise returns a no-override sentinel.</summary>
        public float GetTemperatureOverride()
        {
            return _state.isActive ? _state.currentTemperature : float.MinValue;
        }

        /// <summary>Returns false when the Final Winter has destroyed all crops.</summary>
        public bool AreCropsViable() => !_state.isActive || !_state.cropsDestroyed;

        /// <summary>Returns false when surface water is frozen solid.</summary>
        public bool IsSurfaceWaterAccessible() => !_state.isActive || !_state.surfaceWaterFrozen;

        /// <summary>Returns true when the 30-day deadline has been reached.</summary>
        public bool IsDeadlineReached() => _state.isActive && _state.daysRemaining <= 0;

        // -----------------------------------------------------------------
        // Save / Load
        // -----------------------------------------------------------------

        public FinalWinterState GetState() => _state;

        public void RestoreState(FinalWinterState state)
        {
            _state = state ?? new FinalWinterState();
        }
    }
}
