using System;
using UnityEngine;

namespace AtomicWar._Game.Narrative
{
    [Serializable]
    public class MudslideState
    {
        public string eventId = "event_mudslide";
        public string displayName = "Toxic Mudslide";
        public float burialChance = 0.30f;
        public bool isHatchBuried = false;
        public float contaminationPerDigHour = 10f;
        public float digHoursRequired = 8f;
        public float digHoursCompleted = 0f;
    }

    /// <summary>
    /// Prompt #657: Event — Mudslide.
    /// Heavy toxic rain causes terrain shifts. 30% chance to bury the shelter hatch
    /// under toxic mud. Digging out exposes survivors to severe Contamination.
    /// </summary>
    public class Event_Mudslide
    {
        private MudslideState _state = new MudslideState();

        // -- Events --
        public event Action<MudslideState> OnHatchBuried;
        public event Action<MudslideState> OnHatchCleared;
        public event Action<MudslideState, float> OnDigProgress;
        public event Action<MudslideState, float> OnContaminationApplied;

        public MudslideState State => _state;

        /// <summary>
        /// Rolls for hatch burial. Returns true if the hatch is now buried.
        /// </summary>
        public bool CheckBurial(System.Random rng)
        {
            if (_state.isHatchBuried) return true;

            if (rng != null && rng.NextDouble() < _state.burialChance)
            {
                _state.isHatchBuried = true;
                _state.digHoursCompleted = 0f;
                OnHatchBuried?.Invoke(_state);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Work on digging out the hatch. Returns total contamination accumulated
        /// during this work session.
        /// </summary>
        public float DigOut(float hoursWorked)
        {
            if (!_state.isHatchBuried) return 0f;
            if (hoursWorked <= 0f) return 0f;

            _state.digHoursCompleted = Mathf.Min(
                _state.digHoursRequired,
                _state.digHoursCompleted + hoursWorked);

            float contamination = hoursWorked * _state.contaminationPerDigHour;
            OnContaminationApplied?.Invoke(_state, contamination);
            OnDigProgress?.Invoke(_state, _state.digHoursCompleted);

            // Check if fully dug out
            if (_state.digHoursCompleted >= _state.digHoursRequired)
            {
                _state.isHatchBuried = false;
                _state.digHoursCompleted = 0f;
                OnHatchCleared?.Invoke(_state);
            }

            return contamination;
        }

        /// <summary>
        /// Returns whether the hatch is currently accessible (not buried).
        /// </summary>
        public bool IsHatchAccessible()
        {
            return !_state.isHatchBuried;
        }

        // -----------------------------------------------------------------
        // Save / Load
        // -----------------------------------------------------------------

        public MudslideState CaptureState() => _state;

        public MudslideState GetState() => CaptureState();

        public void RestoreState(MudslideState state)
        {
            _state = state ?? new MudslideState();
        }
    }
}
