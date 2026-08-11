using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Utilities;

namespace AtomicWar._Game.World
{
    [Serializable]
    public class CherenkovState
    {
        public string anomaly_id = "map_anomaly_cherenkov";
        public int instant_rad_stage = 2;
        public bool negates_darkness = true;
    }

    public sealed class MapAnomaly_Cherenkov
    {
        private CherenkovState _state;

        public event Action<string, int> OnRadStageApplied;
        public event Action<string> OnDarknessNegated;

        public string AnomalyId => _state.anomaly_id;

        public MapAnomaly_Cherenkov()
        {
            _state = new CherenkovState();
        }

        public void EnterPool(string survivor_id)
        {
            if (string.IsNullOrEmpty(survivor_id))
            {
                Debug.LogError("[MapAnomaly_Cherenkov] survivor_id is null or empty.");
                return;
            }

            OnRadStageApplied?.Invoke(survivor_id, _state.instant_rad_stage);
            GameLog.Log($"[MapAnomaly_Cherenkov] Survivor '{survivor_id}' entered pool — instant Stage {_state.instant_rad_stage} radiation applied.");
        }

        public bool IsLit(string node_id)
        {
            if (string.IsNullOrEmpty(node_id))
            {
                Debug.LogError("[MapAnomaly_Cherenkov] node_id is null or empty.");
                return false;
            }

            if (_state.negates_darkness)
            {
                OnDarknessNegated?.Invoke(node_id);
            }

            return _state.negates_darkness;
        }

        public CherenkovState CaptureState()
        {
            return new CherenkovState
            {
                anomaly_id = _state.anomaly_id,
                instant_rad_stage = _state.instant_rad_stage,
                negates_darkness = _state.negates_darkness
            };
        }

        public void RestoreState(CherenkovState saved)
        {
            _state = saved ?? new CherenkovState();
        }
    }
}
