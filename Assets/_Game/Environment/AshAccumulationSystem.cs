using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Environment
{
    /// <summary>
    /// Ash Accumulation (Spec #4 of Section VIII). Ash falls constantly and
    /// accumulates on surface, air intakes, solar panels, and the hatch.
    /// Clearing ash is a daily chore; failing to clear lets the world
    /// bury you. The hatch requires two people after 5 cm accumulation.
    /// </summary>
    public class AshAccumulationSystem
    {
        // Thresholds in centimetres of accumulated ash.
        public const float HatchTwoPersonThresholdCm = 5f;
        public const float SurfaceBurialThresholdCm = 30f;

        [Serializable]
        public class State
        {
            public float SurfaceCm;
            public float AirIntakeCm;
            public float SolarPanelCm;
            public float HatchCm;
            public int LastSimulatedDay;
            public int DailyChoreHoursRemainingToday;   // survivors' free labour
        }

        private State _state = new State();
        public State Current => _state;

        public event Action<string, float> OnAccumulationChanged;       // surface/intake/panel/hatch, cm
        public event Action OnHatchRequiresTwoPeople;
        public event Action<string> OnSurfaceNodeBuried;                // nodeId
        public event Action<float> OnSolarOutputReduced;                // fraction (0..1)
        public event Action OnAirFilterClogAccelerated;

        // Host callbacks.
        public Func<float> GetDay;
        public Func<bool> IsAshStormActive;     // boost rate
        public Action<string, float, float> ApplyFilterClogMultiplier;   // (sourceId, baseRate, multiplier)
        public Action<string> BurySurfaceNode;
        public Action<float> ReduceSolarOutput;                           // 0..1
        public Func<int> GetRosterSize;
        public System.Random Rng;

        public void Tick()
        {
            if (GetDay == null) return;
            int day = Mathf.FloorToInt(GetDay());
            if (day == _state.LastSimulatedDay) return;
            int delta = Mathf.Max(1, day - _state.LastSimulatedDay);
            _state.LastSimulatedDay = day;
            bool storm = IsAshStormActive?.Invoke() ?? false;
            float baseRate = storm ? 0.8f : 0.3f;                        // cm/day

            _state.SurfaceCm += baseRate * delta;
            _state.AirIntakeCm += baseRate * delta;
            _state.SolarPanelCm += baseRate * delta;
            _state.HatchCm += baseRate * delta;

            OnAccumulationChanged?.Invoke("surface", _state.SurfaceCm);
            OnAccumulationChanged?.Invoke("intake", _state.AirIntakeCm);
            OnAccumulationChanged?.Invoke("panel", _state.SolarPanelCm);
            OnAccumulationChanged?.Invoke("hatch", _state.HatchCm);

            if (_state.SolarPanelCm > 0f)
            {
                float reduction = Mathf.Clamp01(_state.SolarPanelCm * 0.04f);
                OnSolarOutputReduced?.Invoke(reduction);
                ReduceSolarOutput?.Invoke(reduction);
            }
            if (storm)
            {
                ApplyFilterClogMultiplier?.Invoke("air_intake", 1f, 2f);
                OnAirFilterClogAccelerated?.Invoke();
            }
            if (_state.HatchCm >= HatchTwoPersonThresholdCm) OnHatchRequiresTwoPeople?.Invoke();
            if (_state.SurfaceCm >= SurfaceBurialThresholdCm)
            {
                int idx = (_state.SurfaceCm - SurfaceBurialThresholdCm) > 0
                    ? Mathf.FloorToInt((_state.SurfaceCm - SurfaceBurialThresholdCm) / 5f) : 0;
                string id = "surface_node_" + (idx % 8);
                OnSurfaceNodeBuried?.Invoke(id);
                BurySurfaceNode?.Invoke(id);
            }
        }

        /// <summary>Survivor clears ash for <paramref name="hours"/> hours.</summary>
        public void ClearAsh(float hours, string surface = null)
        {
            int roster = Mathf.Max(1, GetRosterSize?.Invoke() ?? 1);
            float cmPerHour = 1f / Mathf.Max(1, roster);
            float removed = hours * cmPerHour;
            if (string.IsNullOrEmpty(surface) || surface == "all")
            {
                _state.SurfaceCm = Mathf.Max(0f, _state.SurfaceCm - removed);
                _state.AirIntakeCm = Mathf.Max(0f, _state.AirIntakeCm - removed);
                _state.SolarPanelCm = Mathf.Max(0f, _state.SolarPanelCm - removed);
                _state.HatchCm = Mathf.Max(0f, _state.HatchCm - removed);
            }
            else if (surface == "surface") _state.SurfaceCm = Mathf.Max(0f, _state.SurfaceCm - removed);
            else if (surface == "intake") _state.AirIntakeCm = Mathf.Max(0f, _state.AirIntakeCm - removed);
            else if (surface == "panel") _state.SolarPanelCm = Mathf.Max(0f, _state.SolarPanelCm - removed);
            else if (surface == "hatch") _state.HatchCm = Mathf.Max(0f, _state.HatchCm - removed);

            OnAccumulationChanged?.Invoke("surface", _state.SurfaceCm);
            OnAccumulationChanged?.Invoke("intake", _state.AirIntakeCm);
            OnAccumulationChanged?.Invoke("panel", _state.SolarPanelCm);
            OnAccumulationChanged?.Invoke("hatch", _state.HatchCm);
        }

        public bool HatchRequiresTwoPeople() => _state.HatchCm >= HatchTwoPersonThresholdCm;
        public float SolarOutputFraction => Mathf.Clamp01(1f - _state.SolarPanelCm * 0.04f);

        public State CaptureState() => _state;
        public void RestoreState(State s) { _state = s ?? new State(); }
    }
}
