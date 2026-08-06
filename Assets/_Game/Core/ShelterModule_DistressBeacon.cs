using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    public enum DistressArrivalType
    {
        None,
        Caravan,
        Refugee,
        RaiderArmy
    }

    [Serializable]
    public class DistressBeaconState
    {
        public string moduleId = "shelter_module_distress_beacon";
        public string displayName = "Distress Beacon";
        public bool isActive = false;
        public float broadcastIntervalHours = 12f;
        public float hoursUntilNextArrival = 12f;
    }

    /// <summary>
    /// Prompt #631: Module: Distress Beacon.
    /// SOS broadcast on repeat. Every 12 hours a random arrival shows up at the bunker:
    /// Caravan (trade), Refugee (new survivor), or Raider army (hostile). Pure RNG.
    /// </summary>
    public class ShelterModule_DistressBeacon
    {
        private DistressBeaconState _state = new DistressBeaconState();

        public event Action<DistressBeaconState> OnBeaconActivated;
        public event Action<DistressBeaconState, DistressArrivalType> OnArrival;

        public DistressBeaconState State => _state;

        public void Activate()
        {
            _state.isActive = true;
            _state.hoursUntilNextArrival = _state.broadcastIntervalHours;
            OnBeaconActivated?.Invoke(_state);
        }

        public DistressArrivalType TickHour(System.Random rng)
        {
            if (!_state.isActive) return DistressArrivalType.None;

            _state.hoursUntilNextArrival -= 1f;

            if (_state.hoursUntilNextArrival <= 0f)
            {
                _state.hoursUntilNextArrival = _state.broadcastIntervalHours;
                var arrival = GetArrivalType(rng);
                OnArrival?.Invoke(_state, arrival);
                return arrival;
            }

            return DistressArrivalType.None;
        }

        public DistressArrivalType GetArrivalType(System.Random rng)
        {
            int roll = rng.Next(0, 3);
            switch (roll)
            {
                case 0: return DistressArrivalType.Caravan;
                case 1: return DistressArrivalType.Refugee;
                case 2: return DistressArrivalType.RaiderArmy;
                default: return DistressArrivalType.None;
            }
        }
    }
}
