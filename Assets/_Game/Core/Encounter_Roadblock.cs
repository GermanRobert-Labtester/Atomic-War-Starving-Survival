using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    public enum RoadblockChoice
    {
        PayToll,
        ReverseDetour,
        RamBarricade
    }

    [Serializable]
    public class RoadblockState
    {
        public string id = "encounter_roadblock";
        public string displayName = "Faction Roadblock & Ambush";
        public int tollFuelCost = 5;
        public float reverseDetourHours = 2.0f;
        public float ramChassisDamage = 45f;
    }

    /// <summary>
    /// Prompt #387: Encounter: Roadblocks & Ambushes.
    /// Barricades across main map paths forcing vehicles to stop.
    /// Choices: Pay toll, Reverse detour (costs time/fuel), or Ram barricade (damages vehicle Chassis).
    /// </summary>
    public class Encounter_Roadblock
    {
        private RoadblockState _state = new RoadblockState();

        public event Action<RoadblockState, RoadblockChoice> OnRoadblockResolved;

        public RoadblockState State => _state;

        public bool ResolveChoice(RoadblockChoice choice, ref int playerFuel, ref float vehicleChassisDurability, out float timeSpentHours)
        {
            timeSpentHours = 0f;
            switch (choice)
            {
                case RoadblockChoice.PayToll:
                    if (playerFuel >= _state.tollFuelCost)
                    {
                        playerFuel -= _state.tollFuelCost;
                        OnRoadblockResolved?.Invoke(_state, choice);
                        return true;
                    }
                    return false;

                case RoadblockChoice.ReverseDetour:
                    timeSpentHours = _state.reverseDetourHours;
                    OnRoadblockResolved?.Invoke(_state, choice);
                    return true;

                case RoadblockChoice.RamBarricade:
                    vehicleChassisDurability = Mathf.Max(0f, vehicleChassisDurability - _state.ramChassisDamage);
                    OnRoadblockResolved?.Invoke(_state, choice);
                    return true;
            }
            return false;
        }

        public RoadblockState CaptureState() => _state;

        public void RestoreState(RoadblockState saved)
        {
            _state = saved ?? new RoadblockState();
        }
    }
}
