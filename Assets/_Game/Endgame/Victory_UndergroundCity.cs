using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Endgame
{
    [Serializable]
    public class UndergroundCityState
    {
        public string victoryId = "victory_underground_city";
        public bool isActive = false;
        public bool geothermalTapUpgraded = false;
        public bool algaeVatUpgraded = false;
        public int roomsExcavated = 0;
        public int roomsRequired = 20;
        public bool isSelfSustaining = false;
        public bool hatchSealed = false;
    }

    /// <summary>
    /// Prompt #566: Endgame: The Underground City.
    /// Dig deeper. Requires upgrading GeothermalTap, AlgaeVat, and excavating 20 rooms.
    /// Bunker becomes self-sustaining. Hatch welded shut forever.
    /// </summary>
    public class Victory_UndergroundCity
    {
        private UndergroundCityState _state = new UndergroundCityState();

        public event Action<UndergroundCityState, float> OnUndergroundProgress;
        public event Action<UndergroundCityState> OnSelfSustainingReached;
        public event Action<UndergroundCityState> OnHatchSealed;

        public UndergroundCityState State => _state;

        public void CheckProgress(bool hasGeothermal, bool hasAlgaeVat, int excavatedRooms)
        {
            _state.geothermalTapUpgraded = hasGeothermal;
            _state.algaeVatUpgraded = hasAlgaeVat;
            _state.roomsExcavated = Math.Min(excavatedRooms, _state.roomsRequired);

            float progress = GetProgressPercent();
            OnUndergroundProgress?.Invoke(_state, progress);
        }

        public bool TryActivate(bool geoUpgraded, bool algaeUpgraded, int rooms)
        {
            _state.geothermalTapUpgraded = geoUpgraded;
            _state.algaeVatUpgraded = algaeUpgraded;
            _state.roomsExcavated = Math.Min(rooms, _state.roomsRequired);

            if (geoUpgraded && algaeUpgraded && rooms >= _state.roomsRequired)
            {
                _state.isActive = true;
                _state.isSelfSustaining = true;
                OnSelfSustainingReached?.Invoke(_state);
                return true;
            }

            return false;
        }

        public void SealHatch()
        {
            if (!_state.isSelfSustaining) return;

            _state.hatchSealed = true;
            OnHatchSealed?.Invoke(_state);
        }

        public bool IsVictoryAchieved()
        {
            return _state.isSelfSustaining && _state.hatchSealed;
        }

        public float GetProgressPercent()
        {
            float geoWeight = 33.33f;
            float algaeWeight = 33.33f;
            float roomsWeight = 33.34f;

            float progress = 0f;
            if (_state.geothermalTapUpgraded) progress += geoWeight;
            if (_state.algaeVatUpgraded) progress += algaeWeight;
            progress += roomsWeight * ((float)_state.roomsExcavated / _state.roomsRequired);

            return Math.Min(progress, 100f);
        }

        // ── Save / Load ────────────────────────────────────────────────

        public UndergroundCityState CaptureState()
        {
            return new UndergroundCityState
            {
                victoryId = _state.victoryId,
                isActive = _state.isActive,
                geothermalTapUpgraded = _state.geothermalTapUpgraded,
                algaeVatUpgraded = _state.algaeVatUpgraded,
                roomsExcavated = _state.roomsExcavated,
                roomsRequired = _state.roomsRequired,
                isSelfSustaining = _state.isSelfSustaining,
                hatchSealed = _state.hatchSealed,
            };
        }

        public void RestoreState(UndergroundCityState state)
        {
            if (state == null)
            {
                _state = new UndergroundCityState();
                return;
            }
            _state = new UndergroundCityState
            {
                victoryId = state.victoryId,
                isActive = state.isActive,
                geothermalTapUpgraded = state.geothermalTapUpgraded,
                algaeVatUpgraded = state.algaeVatUpgraded,
                roomsExcavated = state.roomsExcavated,
                roomsRequired = state.roomsRequired,
                isSelfSustaining = state.isSelfSustaining,
                hatchSealed = state.hatchSealed,
            };
        }
    }
}
