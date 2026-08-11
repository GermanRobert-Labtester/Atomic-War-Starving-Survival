using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.World
{
    [Serializable]
    public class AshDunesState
    {
        public string anomalyId = "map_anomaly_ash_dunes";
        public string displayName = "Ash Dunes";
        public float speedMultiplier = 0.50f; // Halves travel speed
        public bool causesWeaponJamming = true;
    }

    /// <summary>
    /// Prompt #454: Anomaly: Ash Dunes.
    /// 10-foot radioactive ash drifts that halve travel speed.
    /// Moving through dunes clogs equipped Firearms with ash, dropping Durability to 0 (jammed) until cleaned.
    /// </summary>
    public class MapAnomaly_AshDunes
    {
        private AshDunesState _state = new AshDunesState();

        public event Action<AshDunesState, string> OnFirearmJammedByAsh;

        public AshDunesState State => _state;

        public float TraverseAshDunes(string survivorId, ref float firearmDurability)
        {
            firearmDurability = 0f; // Jammed
            OnFirearmJammedByAsh?.Invoke(_state, survivorId);
            return _state.speedMultiplier;
        }

        // ── Save / Load ────────────────────────────────────────────────

        public AshDunesState CaptureState()
        {
            return new AshDunesState
            {
                anomalyId = _state.anomalyId,
                displayName = _state.displayName,
                speedMultiplier = _state.speedMultiplier,
                causesWeaponJamming = _state.causesWeaponJamming,
            };
        }

        public void RestoreState(AshDunesState saved)
        {
            if (saved == null)
            {
                _state = new AshDunesState();
                return;
            }
            _state = new AshDunesState
            {
                anomalyId = saved.anomalyId,
                displayName = saved.displayName,
                speedMultiplier = saved.speedMultiplier,
                causesWeaponJamming = saved.causesWeaponJamming,
            };
        }
    }
}
