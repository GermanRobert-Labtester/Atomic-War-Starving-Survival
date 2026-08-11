using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Endgame
{
    [Serializable]
    public class SurfaceDomeState
    {
        public string projectId = "project_surface_dome";
        public bool isBuilt = false;
        public int constructionDays = 15;
        public int daysSpent = 0;
        public float hatchVisibilityBonus = 0.5f;
        public float powerSavings = 50f;
        public bool isShattered = false;
    }

    /// <summary>
    /// Prompt #580: Project: Surface Dome.
    /// Glass/plastic structure over hatch. Hydroponics uses natural sunlight, saving Power.
    /// High HatchVisibility. Shatters in RadHail.
    /// </summary>
    public class Project_SurfaceDome
    {
        private SurfaceDomeState _state = new SurfaceDomeState();

        public event Action<SurfaceDomeState> OnDomeConstructed;
        public event Action<SurfaceDomeState> OnDomeShattered;
        public event Action<SurfaceDomeState, float> OnPowerSavingsApplied;

        public SurfaceDomeState State => _state;

        public void StartConstruction()
        {
            if (_state.isBuilt) return;
            _state.daysSpent = 0;
        }

        public void TickDay()
        {
            if (_state.isBuilt || _state.isShattered) return;

            _state.daysSpent++;
            if (_state.daysSpent >= _state.constructionDays)
            {
                _state.isBuilt = true;
                OnDomeConstructed?.Invoke(_state);
                OnPowerSavingsApplied?.Invoke(_state, _state.powerSavings);
            }
        }

        public void ApplyRadHailDamage()
        {
            if (!_state.isBuilt || _state.isShattered) return;

            _state.isShattered = true;
            OnDomeShattered?.Invoke(_state);
        }

        public float GetPowerSavings()
        {
            if (_state.isBuilt && !_state.isShattered)
                return _state.powerSavings;
            return 0f;
        }

        public float GetHatchVisibility()
        {
            if (_state.isBuilt && !_state.isShattered)
                return _state.hatchVisibilityBonus;
            return 0f;
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public SurfaceDomeState CaptureState() => _state;

        public void RestoreState(SurfaceDomeState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
