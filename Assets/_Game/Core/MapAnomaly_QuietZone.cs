using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class QuietZoneState
    {
        public string anomaly_id = "map_anomaly_quiet_zone";
        public bool reads_zero_rad = true;
        public float sanity_drain_per_hour = 0.1f;
        public List<string> survivors_in_zone = new List<string>();
    }

    public sealed class MapAnomaly_QuietZone
    {
        private QuietZoneState _state;

        public event Action<string> OnMasksRemoved;
        public event Action<string, float> OnSanityDrained;

        public string AnomalyId => _state.anomaly_id;

        public MapAnomaly_QuietZone()
        {
            _state = new QuietZoneState();
        }

        public void EnterZone(string survivor_id)
        {
            if (string.IsNullOrEmpty(survivor_id))
            {
                Debug.LogError("[MapAnomaly_QuietZone] survivor_id is null or empty.");
                return;
            }

            if (!_state.survivors_in_zone.Contains(survivor_id))
            {
                _state.survivors_in_zone.Add(survivor_id);
            }

            OnMasksRemoved?.Invoke(survivor_id);
            Debug.Log($"[MapAnomaly_QuietZone] Survivor '{survivor_id}' entered zone — rad reading is 0, masks removed.");
        }

        public void StayInZone(string survivor_id, float hours)
        {
            if (string.IsNullOrEmpty(survivor_id))
            {
                Debug.LogError("[MapAnomaly_QuietZone] survivor_id is null or empty.");
                return;
            }

            if (hours <= 0f)
                return;

            float total_drain = _state.sanity_drain_per_hour * hours;
            OnSanityDrained?.Invoke(survivor_id, total_drain);
            Debug.Log($"[MapAnomaly_QuietZone] Survivor '{survivor_id}' lost {total_drain:F2} sanity over {hours:F1} hours.");
        }

        public float GetRadReading()
        {
            if (_state.reads_zero_rad)
                return 0f;

            return -1f;
        }

        public QuietZoneState CaptureState()
        {
            return new QuietZoneState
            {
                anomaly_id = _state.anomaly_id,
                reads_zero_rad = _state.reads_zero_rad,
                sanity_drain_per_hour = _state.sanity_drain_per_hour,
                survivors_in_zone = new List<string>(_state.survivors_in_zone)
            };
        }

        public void RestoreState(QuietZoneState saved)
        {
            _state = saved ?? new QuietZoneState();
        }
    }
}
