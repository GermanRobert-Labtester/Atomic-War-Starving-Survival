using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class RadioArrayState
    {
        public string projectId = "project_radio_array";
        public bool isBuilt = false;
        public int constructionDays = 35;
        public int daysSpent = 0;
        public int towersBuilt = 0;
        public int towersRequired = 3;
        public bool tracksCaravans = true;
        public bool tracksRaids = true;
        public bool tracksWeather = true;
    }

    /// <summary>
    /// Prompt #588: Project: Radio Array.
    /// 3 massive surface towers. Tracks all Faction Caravans, Raids, Weather in real-time.
    /// Ultimate info dominance.
    /// </summary>
    public class Project_RadioArray
    {
        private RadioArrayState _state = new RadioArrayState();

        public event Action<RadioArrayState, int> OnTowerBuilt;
        public event Action<RadioArrayState> OnRadioArrayCompleted;
        public event Action<RadioArrayState, string> OnFactionMovementDetected;
        public event Action<RadioArrayState, string> OnWeatherPatternDetected;

        public RadioArrayState State => _state;

        public void StartConstruction()
        {
            if (_state.isBuilt) return;
            _state.daysSpent = 0;
            _state.towersBuilt = 0;
        }

        public void TickDay()
        {
            if (_state.isBuilt) return;

            _state.daysSpent++;
        }

        public void BuildTower()
        {
            if (_state.isBuilt) return;
            if (_state.towersBuilt >= _state.towersRequired) return;

            _state.towersBuilt++;
            OnTowerBuilt?.Invoke(_state, _state.towersBuilt);

            if (_state.towersBuilt >= _state.towersRequired)
            {
                _state.isBuilt = true;
                OnRadioArrayCompleted?.Invoke(_state);
            }
        }

        public bool IsOmniscienceActive()
        {
            return _state.isBuilt && _state.towersBuilt >= _state.towersRequired;
        }

        public List<string> GetTrackedDataTypes()
        {
            var types = new List<string>();
            if (!IsOmniscienceActive()) return types;

            if (_state.tracksCaravans) types.Add("caravans");
            if (_state.tracksRaids) types.Add("raids");
            if (_state.tracksWeather) types.Add("weather");
            return types;
        }
    }
}
