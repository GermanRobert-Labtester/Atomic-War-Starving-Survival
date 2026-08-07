using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class TangledDropState
    {
        public string anomalyId = "map_anomaly_tangled_drop";
        public string displayName = "Tangled Supply Drop";
        public bool isLooted = false;
        public List<string> pristineLootCrate = new List<string> { "prewar_ration_case", "medical_trauma_kit", "556_ammo_can" };
    }

    /// <summary>
    /// Prompt #455: Anomaly: Tangled Supply Drop.
    /// Parachute caught in high-voltage power lines holding a pristine supply crate.
    /// Requires ClimbingGear (#427) or shooting the cord with a SniperRifle (consumes ammo, alerts factions).
    /// </summary>
    public class MapAnomaly_TangledDrop
    {
        private TangledDropState _state = new TangledDropState();

        public event Action<TangledDropState, string> OnSupplyDropRetrieved;

        public TangledDropState State => _state;

        public List<string> RetrieveDropWithClimbingGear(bool hasClimbingGear)
        {
            if (hasClimbingGear && !_state.isLooted)
            {
                _state.isLooted = true;
                OnSupplyDropRetrieved?.Invoke(_state, "climbing_gear");
                return new List<string>(_state.pristineLootCrate);
            }
            return null;
        }

        public List<string> ShootDownDropWithSniper(bool hasSniperRifle, ref int ammoCount, out bool alertedFactions)
        {
            alertedFactions = false;
            if (hasSniperRifle && ammoCount > 0 && !_state.isLooted)
            {
                ammoCount--;
                _state.isLooted = true;
                alertedFactions = true;
                OnSupplyDropRetrieved?.Invoke(_state, "sniper_rifle");
                return new List<string>(_state.pristineLootCrate);
            }
            return null;
        }

        // ── Save / Load ────────────────────────────────────────────────

        public TangledDropState CaptureState()
        {
            return new TangledDropState
            {
                anomalyId = _state.anomalyId,
                displayName = _state.displayName,
                isLooted = _state.isLooted,
                pristineLootCrate = _state.pristineLootCrate != null ? new System.Collections.Generic.List<string>(_state.pristineLootCrate) : new System.Collections.Generic.List<string>(),
            };
        }

        public void RestoreState(TangledDropState saved)
        {
            if (saved == null)
            {
                _state = new TangledDropState();
                return;
            }
            _state = new TangledDropState
            {
                anomalyId = saved.anomalyId,
                displayName = saved.displayName,
                isLooted = saved.isLooted,
                pristineLootCrate = saved.pristineLootCrate != null ? new System.Collections.Generic.List<string>(saved.pristineLootCrate) : new System.Collections.Generic.List<string>(),
            };
        }
    }
}
