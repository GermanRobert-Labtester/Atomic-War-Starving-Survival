using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class RadHailState
    {
        public string weatherId = "weather_rad_hail";
        public string displayName = "Rad-Hail";
        public bool isActive = false;
        public float bluntTraumaDamage = 35f;
        public float radiationSpikeMillisieverts = 400f;
    }

    /// <summary>
    /// Prompt #377: System: Rad-Hail.
    /// Radioactive ice hail. Instantly destroys CatchmentSurfaces.
    /// Unprotected survivors outside suffer BluntTrauma and massive radiation spikes.
    /// </summary>
    public class Weather_RadHail
    {
        private RadHailState _state = new RadHailState();

        public event Action<RadHailState> OnCatchmentSurfacesDestroyed;
        public event Action<RadHailState, float, float> OnSurvivorStruckOutside;

        public RadHailState State => _state;

        public void TriggerRadHailStorm()
        {
            _state.isActive = true;
            OnCatchmentSurfacesDestroyed?.Invoke(_state);
        }

        public (float damage, float rads) StrikeSurvivorOutside(bool hasHardCover)
        {
            if (!_state.isActive || hasHardCover) return (0f, 0f);

            OnSurvivorStruckOutside?.Invoke(_state, _state.bluntTraumaDamage, _state.radiationSpikeMillisieverts);
            return (_state.bluntTraumaDamage, _state.radiationSpikeMillisieverts);
        }

        public RadHailState CaptureState() => _state;

        public void RestoreState(RadHailState saved)
        {
            _state = saved ?? new RadHailState();
        }
    }
}
