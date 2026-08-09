using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Utilities;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class BlackSnowState
    {
        public string weather_id = "weather_black_snow";
        public bool is_sticky = true;
        public bool requires_ammonia = true;
        public List<string> ruined_suit_survivor_ids = new List<string>();
        public List<string> contaminated_suit_survivor_ids = new List<string>();
    }

    /// <summary>DEMOTE-Weather-batch — dormant ghost. Re-promote with Boot+Save+host.</summary>
    public sealed class Weather_BlackSnow
    {
        private BlackSnowState _state;

        public event Action<string> OnSuitContaminated;
        public event Action<string> OnSuitRuined;
        public event Action<string> OnSuitCleaned;

        public string WeatherId => _state.weather_id;

        public Weather_BlackSnow()
        {
            _state = new BlackSnowState();
        }

        public void HitBySnow(string survivor_id, bool has_ammonia)
        {
            if (string.IsNullOrEmpty(survivor_id))
            {
                Debug.LogError("[Weather_BlackSnow] survivor_id is null or empty.");
                return;
            }

            OnSuitContaminated?.Invoke(survivor_id);

            if (has_ammonia)
            {
                if (_state.contaminated_suit_survivor_ids.Contains(survivor_id))
                {
                    _state.contaminated_suit_survivor_ids.Remove(survivor_id);
                }

                OnSuitCleaned?.Invoke(survivor_id);
                GameLog.Log($"[Weather_BlackSnow] Survivor '{survivor_id}' suit cleaned with ammonia.");
            }
            else
            {
                if (!_state.ruined_suit_survivor_ids.Contains(survivor_id))
                {
                    _state.ruined_suit_survivor_ids.Add(survivor_id);
                }

                if (_state.contaminated_suit_survivor_ids.Contains(survivor_id))
                {
                    _state.contaminated_suit_survivor_ids.Remove(survivor_id);
                }

                OnSuitRuined?.Invoke(survivor_id);
                GameLog.Log($"[Weather_BlackSnow] Survivor '{survivor_id}' suit permanently ruined — no ammonia available.");
            }
        }

        public bool IsSuitRuined(string survivor_id)
        {
            if (string.IsNullOrEmpty(survivor_id))
                return false;

            return _state.ruined_suit_survivor_ids.Contains(survivor_id);
        }

        public BlackSnowState CaptureState()
        {
            return new BlackSnowState
            {
                weather_id = _state.weather_id,
                is_sticky = _state.is_sticky,
                requires_ammonia = _state.requires_ammonia,
                ruined_suit_survivor_ids = new List<string>(_state.ruined_suit_survivor_ids),
                contaminated_suit_survivor_ids = new List<string>(_state.contaminated_suit_survivor_ids)
            };
        }

        public void RestoreState(BlackSnowState saved)
        {
            _state = saved ?? new BlackSnowState();
        }
    }
}
