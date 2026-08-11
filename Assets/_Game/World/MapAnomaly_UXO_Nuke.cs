using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.World
{
    [Serializable]
    public class UXONukeState
    {
        public string anomalyId = "map_anomaly_uxo_nuke";
        public string displayName = "Unexploded ICBM";
        public bool isDetonated = false;
        public string fissileMaterialItem = "fissile_nuclear_material";
    }

    /// <summary>
    /// Prompt #452: Anomaly: Unexploded ICBM.
    /// Dud missile lodged in a ruined building.
    /// Players can attempt to harvest FissileMaterial. Failing the Engineering check detonates the warhead, ending the run.
    /// </summary>
    public class MapAnomaly_UXO_Nuke
    {
        private UXONukeState _state = new UXONukeState();

        public event Action<UXONukeState> OnFissileMaterialHarvested;
        public event Action<UXONukeState> OnWarheadDetonatedRunEnded;

        public UXONukeState State => _state;

        public string HarvestFissileMaterial(int engineerSkill, System.Random rng, out bool lethalDetonation)
        {
            lethalDetonation = false;
            if (_state.isDetonated) return null;

            bool success = engineerSkill >= 15 && rng.NextDouble() > 0.35;
            if (success)
            {
                OnFissileMaterialHarvested?.Invoke(_state);
                return _state.fissileMaterialItem;
            }

            _state.isDetonated = true;
            lethalDetonation = true;
            OnWarheadDetonatedRunEnded?.Invoke(_state);
            return null;
        }

        // ── Save / Load ────────────────────────────────────────────────

        public UXONukeState CaptureState()
        {
            return new UXONukeState
            {
                anomalyId = _state.anomalyId,
                displayName = _state.displayName,
                isDetonated = _state.isDetonated,
                fissileMaterialItem = _state.fissileMaterialItem,
            };
        }

        public void RestoreState(UXONukeState saved)
        {
            if (saved == null)
            {
                _state = new UXONukeState();
                return;
            }
            _state = new UXONukeState
            {
                anomalyId = saved.anomalyId,
                displayName = saved.displayName,
                isDetonated = saved.isDetonated,
                fissileMaterialItem = saved.fissileMaterialItem,
            };
        }
    }
}
