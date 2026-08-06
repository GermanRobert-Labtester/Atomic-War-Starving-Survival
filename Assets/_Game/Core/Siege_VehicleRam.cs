using System;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class SiegeVehicleRamState
    {
        public string siegeId = "siege_vehicle_ram";
        public bool truckApproaching;
        public int distanceNodes;
        public float vaultDoorDurability;
        public bool breached;
    }

    /// <summary>
    /// Prompt #822: Vehicle Ram. An armored truck smashes the VaultDoor,
    /// bypassing lockpicking timers entirely. The hatch instantly drops to
    /// 0% durability, initiating close-quarters combat immediately.
    /// Plain C#. Save/load safe.
    /// </summary>
    public class Siege_VehicleRam
    {
        private SiegeVehicleRamState _state = new SiegeVehicleRamState();

        private const int ApproachTurns = 2;

        // -- Events --
        public event Action<int> OnTruckSpotted;     // distance in nodes
        public event Action OnTruckArrived;
        public event Action OnDoorRammed;
        public event Action OnBreachInitiated;

        public SiegeVehicleRamState State => _state;

        /// <summary>
        /// The armored truck is spotted approaching from 2 nodes away.
        /// </summary>
        public void StartApproach()
        {
            _state.truckApproaching = true;
            _state.distanceNodes = ApproachTurns;
            _state.vaultDoorDurability = 100f;
            _state.breached = false;

            OnTruckSpotted?.Invoke(_state.distanceNodes);
        }

        /// <summary>
        /// Advance one turn. The truck closes distance. On arrival, the
        /// door is rammed automatically.
        /// </summary>
        public void TickTurn()
        {
            if (!_state.truckApproaching || _state.breached) return;

            _state.distanceNodes--;

            if (_state.distanceNodes <= 0)
            {
                _state.truckApproaching = false;
                OnTruckArrived?.Invoke();
                RamDoor();
            }
        }

        /// <summary>
        /// The truck smashes through the VaultDoor. Durability drops to
        /// zero immediately. No lockpicking phase — straight to CQC.
        /// </summary>
        public void RamDoor()
        {
            if (_state.breached) return;

            _state.vaultDoorDurability = 0f;
            _state.breached = true;

            OnDoorRammed?.Invoke();
            OnBreachInitiated?.Invoke();
        }

        /// <summary>True when the VaultDoor has been destroyed by the ram.</summary>
        public bool IsBreached()
        {
            return _state.breached;
        }

        // -----------------------------------------------------------------
        // Save / Load
        // -----------------------------------------------------------------

        public SiegeVehicleRamState CaptureState()
        {
            return new SiegeVehicleRamState
            {
                siegeId = _state.siegeId,
                truckApproaching = _state.truckApproaching,
                distanceNodes = _state.distanceNodes,
                vaultDoorDurability = _state.vaultDoorDurability,
                breached = _state.breached
            };
        }

        public void RestoreState(SiegeVehicleRamState saved)
        {
            _state = saved ?? new SiegeVehicleRamState();
        }
    }
}
