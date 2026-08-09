using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Utilities;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class CultBlessingState
    {
        public string event_id = "event_cult_blessing";
        public float rad_emitted_per_hour = 10f;
        public bool is_glowing = false;
        public List<string> captured_survivor_ids = new List<string>();
        public List<string> chelated_survivor_ids = new List<string>();
    }

    public sealed class Event_CultBlessing
    {
        private CultBlessingState _state;

        public event Action<string> OnSurvivorReturned;
        public event Action<string, float> OnRadEmitted;
        public event Action<string, string> OnAllyPoisoned;
        public event Action<string> OnChelationApplied;

        public string EventId => _state.event_id;

        public Event_CultBlessing()
        {
            _state = new CultBlessingState();
        }

        public void SurvivorCaptured(string survivor_id)
        {
            if (string.IsNullOrEmpty(survivor_id))
            {
                Debug.LogError("[Event_CultBlessing] survivor_id is null or empty.");
                return;
            }

            if (!_state.captured_survivor_ids.Contains(survivor_id))
            {
                _state.captured_survivor_ids.Add(survivor_id);
            }

            _state.is_glowing = true;

            OnSurvivorReturned?.Invoke(survivor_id);
            GameLog.Log($"[Event_CultBlessing] Survivor '{survivor_id}' returned — force-fed isotopes, now glowing as mobile rad emitter.");
        }

        public void TickHour(string survivor_id, List<string> nearby_ally_ids)
        {
            if (string.IsNullOrEmpty(survivor_id))
            {
                Debug.LogError("[Event_CultBlessing] survivor_id is null or empty.");
                return;
            }

            if (_state.chelated_survivor_ids.Contains(survivor_id))
                return;

            if (!_state.captured_survivor_ids.Contains(survivor_id))
                return;

            OnRadEmitted?.Invoke(survivor_id, _state.rad_emitted_per_hour);

            if (nearby_ally_ids != null)
            {
                foreach (string ally_id in nearby_ally_ids)
                {
                    if (string.IsNullOrEmpty(ally_id))
                        continue;

                    if (ally_id == survivor_id)
                        continue;

                    OnAllyPoisoned?.Invoke(survivor_id, ally_id);
                    GameLog.Log($"[Event_CultBlessing] Ally '{ally_id}' poisoned by emitter '{survivor_id}'.");
                }
            }
        }

        public void ApplyChelation(string survivor_id)
        {
            if (string.IsNullOrEmpty(survivor_id))
            {
                Debug.LogError("[Event_CultBlessing] survivor_id is null or empty.");
                return;
            }

            if (!_state.chelated_survivor_ids.Contains(survivor_id))
            {
                _state.chelated_survivor_ids.Add(survivor_id);
            }

            _state.is_glowing = false;

            OnChelationApplied?.Invoke(survivor_id);
            GameLog.Log($"[Event_CultBlessing] Chelation applied to '{survivor_id}' — radiation emission stopped.");
        }

        public bool IsGlowing(string survivor_id)
        {
            if (string.IsNullOrEmpty(survivor_id))
                return false;

            if (_state.chelated_survivor_ids.Contains(survivor_id))
                return false;

            return _state.is_glowing && _state.captured_survivor_ids.Contains(survivor_id);
        }

        public CultBlessingState CaptureState()
        {
            return new CultBlessingState
            {
                event_id = _state.event_id,
                rad_emitted_per_hour = _state.rad_emitted_per_hour,
                is_glowing = _state.is_glowing,
                captured_survivor_ids = new List<string>(_state.captured_survivor_ids),
                chelated_survivor_ids = new List<string>(_state.chelated_survivor_ids)
            };
        }

        public void RestoreState(CultBlessingState saved)
        {
            _state = saved ?? new CultBlessingState();
        }
    }
}
