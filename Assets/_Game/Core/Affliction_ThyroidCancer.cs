using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Utilities;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class ThyroidCancerState
    {
        public string affliction_id = "affliction_thyroid_cancer";
        public float max_stamina_cap = 0.5f;
        public float max_health_cap = 0.5f;
        public bool is_progressing = true;
        public List<string> diagnosed_survivor_ids = new List<string>();
        public List<string> halted_survivor_ids = new List<string>();
    }

    public sealed class Affliction_ThyroidCancer
    {
        private ThyroidCancerState _state;

        public event Action<string, float> OnStaminaCapped;
        public event Action<string, float> OnHealthCapped;
        public event Action<string> OnCancerDiagnosed;

        public string AfflictionId => _state.affliction_id;

        public Affliction_ThyroidCancer()
        {
            _state = new ThyroidCancerState();
        }

        public void Diagnose(string survivor_id)
        {
            if (string.IsNullOrEmpty(survivor_id))
            {
                Debug.LogError("[Affliction_ThyroidCancer] survivor_id is null or empty.");
                return;
            }

            if (!_state.diagnosed_survivor_ids.Contains(survivor_id))
            {
                _state.diagnosed_survivor_ids.Add(survivor_id);
            }

            OnCancerDiagnosed?.Invoke(survivor_id);
            OnStaminaCapped?.Invoke(survivor_id, _state.max_stamina_cap);
            OnHealthCapped?.Invoke(survivor_id, _state.max_health_cap);

            GameLog.Log($"[Affliction_ThyroidCancer] Survivor '{survivor_id}' diagnosed — stamina capped at {_state.max_stamina_cap:P0}, health capped at {_state.max_health_cap:P0}.");
        }

        public void HaltSpread(string survivor_id, bool has_surgery, bool has_chemo)
        {
            if (string.IsNullOrEmpty(survivor_id))
            {
                Debug.LogError("[Affliction_ThyroidCancer] survivor_id is null or empty.");
                return;
            }

            if (has_surgery && has_chemo)
            {
                if (!_state.halted_survivor_ids.Contains(survivor_id))
                {
                    _state.halted_survivor_ids.Add(survivor_id);
                }

                GameLog.Log($"[Affliction_ThyroidCancer] Progression halted for '{survivor_id}' (surgery + chemo).");
            }
            else
            {
                GameLog.Log($"[Affliction_ThyroidCancer] Cannot halt progression for '{survivor_id}' — surgery={has_surgery}, chemo={has_chemo}.");
            }
        }

        public bool IsProgressing(string survivor_id)
        {
            if (string.IsNullOrEmpty(survivor_id))
                return false;

            if (_state.halted_survivor_ids.Contains(survivor_id))
                return false;

            return _state.is_progressing;
        }

        public float GetStaminaCap()
        {
            return _state.max_stamina_cap;
        }

        public float GetHealthCap()
        {
            return _state.max_health_cap;
        }

        public ThyroidCancerState CaptureState()
        {
            return new ThyroidCancerState
            {
                affliction_id = _state.affliction_id,
                max_stamina_cap = _state.max_stamina_cap,
                max_health_cap = _state.max_health_cap,
                is_progressing = _state.is_progressing,
                diagnosed_survivor_ids = new List<string>(_state.diagnosed_survivor_ids),
                halted_survivor_ids = new List<string>(_state.halted_survivor_ids)
            };
        }

        public void RestoreState(ThyroidCancerState saved)
        {
            _state = saved ?? new ThyroidCancerState();
        }
    }
}
