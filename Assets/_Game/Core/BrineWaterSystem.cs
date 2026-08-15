using System;
using UnityEngine;
using AtomicWar._Game.Environment;
using Ashfall.Core;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// ASHFALL: THE HOLDFAST — District 8 water is plentiful and not potable.
    /// Hooks WaterEconomySystem; does not replace it. Spec §5.2.
    /// </summary>
    [Serializable]
    public class BrineWaterSystemState
    {
        public string systemId = BrineWaterSystem.SystemId;
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
        public float MembraneIntegrity => _state.membraneIntegrity;
        public bool SteamTripped => _state.steamTripped;
        public float ClusterIndoorC => _state.clusterIndoorC;

        public void TickDaily(int day, WeatherKind weather, float outdoorC, bool outfallShifted)
        {
            float load = 3.2f;
            if (outfallShifted) load *= 0.55f;
            if (weather == WeatherKind.FalseSpring || weather == WeatherKind.IceStorm)
                load *= 1.15f;
            _state.membraneIntegrity = Mathf.Clamp(_state.membraneIntegrity - load, 0f, 100f);

            if (!_state.steamTripped && _state.membraneIntegrity < SteamTripIntegrity)
            {
                _state.steamTripped = true;
                _state.steamTripDay = day;
                _state.hoursSinceTrip = 0;
                OnSteamTrip?.Invoke();
                OnWaterStateChanged?.Invoke();
            }

            if (_state.steamTripped && !_state.membraneSaved)
            {
                _state.hoursSinceTrip += 24;
                float t = Mathf.Clamp01(_state.hoursSinceTrip / (float)SteamCollapseHours);
                _state.clusterIndoorC = Mathf.Lerp(16f, outdoorC < 0f ? outdoorC : ClusterOutdoorFallbackC, t);
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
            _state.membraneIntegrity = Mathf.Clamp(_state.membraneIntegrity + drums * 12f, 0f, 100f);
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
            RaiseChanged();
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
            RaiseChanged();
        }

        private void RaiseChanged() => OnStateChanged?.Invoke(_state);
    }
}
