using System;

namespace Ashfall.Core
{
    /// <summary>
    /// ASHFALL: THE HOLDFAST — District 8 water is plentiful and not potable.
    /// Spec §5.2. Tick is gated on Unlock (B6): must not auto-trip around day 18 from boot.
    /// </summary>
    [Serializable]
    public class BrineWaterSystemState
    {
        public string systemId = BrineWaterSystem.SystemId;
        public bool unlocked;
        public float membraneIntegrity = 72f;
        public float processWaterBarrels;
        public float clusterIndoorC = 16f;
        public bool steamTripped;
        public int steamTripDay = -1;
        public int hoursSinceTrip;
        public bool saltTradeUnlocked;
        public bool membraneSaved;
        public bool membraneLetDrop;
        public bool membraneSector4Strip;
    }

    public class BrineWaterSystem
    {
        public const string SystemId = "brine_water_system";
        public const string FlagMembraneSector4 = "holdfast_membrane_sector4";
        public const string FlagMembraneLetDrop = "holdfast_membrane_let_drop";
        public const string ItemRoResin = "item_ro_resin";
        public const string ItemProcessBarrel = "item_process_barrel";
        public const float SteamTripIntegrity = 15f;
        public const int SteamCollapseHours = 48;
        public const float TransportLoss = 0.25f;
        public const float ClusterOutdoorFallbackC = -18f;

        private BrineWaterSystemState _state = new BrineWaterSystemState();

        public event Action OnSteamTrip;
        public event Action OnWaterStateChanged;
        public event Action<BrineWaterSystemState> OnStateChanged;

        public BrineWaterSystemState State => _state;
        public bool Unlocked => _state.unlocked;
        public float MembraneIntegrity => _state.membraneIntegrity;
        public bool SteamTripped => _state.steamTripped;
        public float ClusterIndoorC => _state.clusterIndoorC;

        /// <summary>Plant visited / salt trade opened. Daily brine load starts here, not at boot.</summary>
        public void Unlock()
        {
            if (_state.unlocked) return;
            _state.unlocked = true;
            RaiseChanged();
        }

        public void TickDaily(int day, WeatherKind weather, float outdoorC, bool outfallShifted)
        {
            if (!_state.unlocked) return;

            float load = 3.2f;
            if (outfallShifted) load *= 0.55f;
            if (weather == WeatherKind.FalseSpring || weather == WeatherKind.IceStorm)
                load *= 1.15f;
            _state.membraneIntegrity = Math.Clamp(_state.membraneIntegrity - load, 0f, 100f);

            if (!_state.steamTripped && _state.membraneIntegrity < SteamTripIntegrity)
            {
                _state.steamTripped = true;
                _state.steamTripDay = day;
                _state.hoursSinceTrip = 0;
                OnSteamTrip?.Invoke();
                OnWaterStateChanged?.Invoke();
            }
            else if (_state.steamTripped && !_state.membraneSaved)
            {
                _state.hoursSinceTrip += 24;
                float t = Math.Clamp(_state.hoursSinceTrip / (float)SteamCollapseHours, 0f, 1f);
                float floor = outdoorC < 0f ? outdoorC : ClusterOutdoorFallbackC;
                _state.clusterIndoorC = 16f + (floor - 16f) * t;
            }
            else if (!_state.steamTripped)
            {
                _state.clusterIndoorC = 16f;
            }

            RaiseChanged();
        }

        public bool RepairWithResin(int drums)
        {
            if (drums <= 0) return false;
            _state.membraneIntegrity = Math.Clamp(_state.membraneIntegrity + drums * 12f, 0f, 100f);
            if (_state.membraneIntegrity >= 40f && _state.steamTripped)
            {
                _state.membraneSaved = true;
                _state.steamTripped = false;
                _state.clusterIndoorC = 14f;
            }
            OnWaterStateChanged?.Invoke();
            RaiseChanged();
            return true;
        }

        public void ResolveMembraneStripSector4()
        {
            _state.membraneSector4Strip = true;
            _state.membraneLetDrop = false;
            RepairWithResin(4);
            RaiseChanged();
        }

        public void ResolveMembraneLetDrop()
        {
            _state.membraneLetDrop = true;
            _state.membraneSaved = false;
            RaiseChanged();
        }

        public void UnlockSaltTrade()
        {
            _state.saltTradeUnlocked = true;
            Unlock();
        }

        public float HaulCleanWaterSouth(float barrels)
        {
            if (barrels <= 0f) return 0f;
            return barrels * (1f - TransportLoss);
        }

        public BrineWaterSystemState CaptureState()
        {
            return new BrineWaterSystemState
            {
                systemId = _state.systemId,
                unlocked = _state.unlocked,
                membraneIntegrity = _state.membraneIntegrity,
                processWaterBarrels = _state.processWaterBarrels,
                clusterIndoorC = _state.clusterIndoorC,
                steamTripped = _state.steamTripped,
                steamTripDay = _state.steamTripDay,
                hoursSinceTrip = _state.hoursSinceTrip,
                saltTradeUnlocked = _state.saltTradeUnlocked,
                membraneSaved = _state.membraneSaved,
                membraneLetDrop = _state.membraneLetDrop,
                membraneSector4Strip = _state.membraneSector4Strip
            };
        }

        public void RestoreState(BrineWaterSystemState saved)
        {
            _state = saved ?? new BrineWaterSystemState();
            if (string.IsNullOrEmpty(_state.systemId)) _state.systemId = SystemId;
            if (_state.saltTradeUnlocked) _state.unlocked = true;
            RaiseChanged();
        }

        private void RaiseChanged() => OnStateChanged?.Invoke(_state);
    }
}
