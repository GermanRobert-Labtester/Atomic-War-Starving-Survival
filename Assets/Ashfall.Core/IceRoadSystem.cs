using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core
{
    /// <summary>
    /// ASHFALL: THE HOLDFAST — seasonal gate on District 8 travel.
    /// Daily calendar with teeth. Not ice physics.
    /// Spec: docs/expansions/expansion_the_holdfast_plan.md §5.1.
    /// Engine-agnostic extract of Assets/_Game/Core/IceRoadSystem.cs
    /// (Mathf.Clamp → Math.Clamp; no UnityEngine / Godot).
    /// </summary>
    [Serializable]
    public class IceRoadSystemState
    {
        public string systemId = IceRoadSystem.SystemId;
        public bool expansionUnlocked;
        public bool isOpen;
        public float iceThicknessM;
        public bool cuttersAccess = true;
        public bool southBeaconLit = true;
        public int windowDaysRemaining;
        public int windowLengthDays;
        public int lampsOutCountdown;
        public int windowsCompleted;
        public int accidentCount;
        public int lastOpenDay = -1;
        public int lastCloseDay = -1;
        public bool clerkStarted;
        public bool yaraWithdrewPermanently;
        public int seedSalt;
        /// <summary>Second Winter cap (Duty Roster). 0 = no override.</summary>
        public int windowLengthOverride;
    }

    public class IceRoadSystem
    {
        public const string SystemId = "ice_road_system";
        public const string FlagIceRoadOpen = "ice_road_open";
        public const string FlagExpUnlocked = "exp_holdfast_unlocked";
        public const string FlagStuckNorth = "holdfast_stuck_north";
        public const string RegionHoldfast = "region_holdfast";

        public const string LocIceRoadGate = "loc_ice_road_gate";
        public const string LocKilometre19 = "loc_cut_kilometre_19";
        public const string LocWeighHut = "loc_cut_weigh_hut";
        public const string LocDredgerHulk = "loc_cut_dredger_hulk";
        public const string LocBrinePool = "loc_cut_brine_pool";
        public const string LocWaystationA = "loc_cut_waystation_a";
        public const string LocAccident12 = "loc_cut_accident_12";
        public const string LocSouthBeacon = "loc_cut_south_beacon";
        public const string LocShallowsMarket = "loc_the_shallows_market";

        /// <summary>Thickness at which a freeze window may open (metres).</summary>
        public const float OpenThicknessM = 0.28f;
        public const float MaxThicknessM = 1.20f;
        public const int MinWindowDays = 11;
        public const int MaxWindowDays = 20;
        public const int LampsOutDays = 11;
        public const float ClosedBoatTravelMultiplier = 1.6f;
        public const float OpenFatigueMultiplier = 1.35f;
        /// <summary>Extra warmth drain per expedition-hour while on the Cut (proxy for −8°C).</summary>
        public const float OpenWarmthDrainPerHour = 2.0f;
        public const float IceAlbedoUvMultiplier = 1.35f;

        public static readonly string[] CutNodeIds =
        {
            LocIceRoadGate, LocKilometre19, LocWeighHut, LocDredgerHulk,
            LocBrinePool, LocWaystationA, LocAccident12, LocSouthBeacon
        };

        /// <summary>
        /// Sector 4 flavour ids recast by Holdfast. They sit on the live map
        /// before unlock and must never be ice-gated (B4).
        /// </summary>
        public const string LocAbandonedDesalination = "location_abandoned_desalination";
        public const string LocFrozenRiverBarge = "location_frozen_river_barge";
        public const string LocCrashedIcebreakerConvoy = "location_crashed_icebreaker_convoy";

        public static readonly string[] LegacySector4IdsNotIceGated =
        {
            LocAbandonedDesalination, LocFrozenRiverBarge, LocCrashedIcebreakerConvoy
        };

        private IceRoadSystemState _state = new IceRoadSystemState();
        private readonly HashSet<string> _cutNodes = new HashSet<string>(CutNodeIds);
        private readonly HashSet<string> _holdfastNodes = new HashSet<string>();
        private readonly HashSet<string> _darkBeacons = new HashSet<string>();


        public event Action OnIceRoadOpened;
        public event Action OnIceRoadClosed;
        public event Action<string> OnBeaconDark;
        public event Action OnAccidentLogged;
        public event Action<IceRoadSystemState> OnStateChanged;

        public IceRoadSystemState State => _state;
        public bool IsOpen => _state.isOpen;
        public bool IsUnlocked => _state.expansionUnlocked;
        public bool CuttersAccess => _state.cuttersAccess && !_state.yaraWithdrewPermanently;
        public float IceThicknessM => _state.iceThicknessM;
        public bool SouthBeaconLit => _state.southBeaconLit && !_darkBeacons.Contains(LocSouthBeacon);
        public int WindowDaysRemaining => _state.windowDaysRemaining;

        public IceRoadSystem()
        {
            RegisterDefaultHoldfastNodes();
        }

        public IceRoadSystem(int seedSalt) : this()
        {
            _state.seedSalt = seedSalt;
        }

        public void Initialise(int seedSalt)
        {
            _state.seedSalt = seedSalt;
            RegisterDefaultHoldfastNodes();
        }

        /// <summary>Old saves: road stays dark until the gate/sheet unlocks the district.</summary>
        public void Unlock(int day)
        {
            if (_state.expansionUnlocked) return;
            _state.expansionUnlocked = true;
            RaiseChanged();
        }

        public void NotifyClerkStarted()
        {
            _state.clerkStarted = true;
            RaiseChanged();
        }

        /// <summary>
        /// Second Winter (Duty Roster §5.4): cap future Ice Road windows to
        /// [minDays..maxDays]. 0 clears the override. Not a new weather sim.
        /// </summary>
        public void ShortenWindowLength(int minDays, int maxDays, int seedSalt)
        {
            int span = Math.Max(1, maxDays - minDays + 1);
            int n = seedSalt < 0 ? -seedSalt : seedSalt;
            _state.windowLengthOverride = minDays + (n % span);
            RaiseChanged();
        }

        public void ClearWindowLengthOverride()
        {
            if (_state.windowLengthOverride == 0) return;
            _state.windowLengthOverride = 0;
            RaiseChanged();
        }

        /// <summary>Daily tick from Time + Weather. O(cut nodes), not per frame.</summary>
        public void TickDaily(int day, WeatherKind weather, float outdoorCelsius)
        {
            AdvanceThickness(weather, outdoorCelsius);
            TickLampsOut();

            if (_state.isOpen)
            {
                _state.windowDaysRemaining--;
                if (_state.windowDaysRemaining <= 0 || !CanRemainOpen(weather))
                    Close(day);
            }
            else if (CanOpen(weather, day))
            {
                Open(day);
            }

            RaiseChanged();
        }

        public bool IsCutNode(string nodeId) =>
            !string.IsNullOrEmpty(nodeId) && _cutNodes.Contains(nodeId);

        public bool IsHoldfastNode(string nodeId) =>
            !string.IsNullOrEmpty(nodeId) && _holdfastNodes.Contains(nodeId);

        public void RegisterHoldfastNode(string nodeId)
        {
            if (string.IsNullOrEmpty(nodeId)) return;
            _holdfastNodes.Add(nodeId);
        }

        /// <summary>
        /// Cut travel is blocked while dark/closed. Shallows boat is never a Cut node.
        /// Off-season Cut walking is refused here (brine at −20°C) — host may offer the boat.
        /// </summary>
        public bool IsTravelBlocked(string nodeId)
        {
            if (!IsCutNode(nodeId) && !IsHoldfastSaltOrClusterOrShelf(nodeId))
                return false;
            if (!_state.expansionUnlocked) return true;
            if (IsCutNode(nodeId))
            {
                if (!_state.isOpen) return true;
                if (!CuttersAccess) return true;
                if (nodeId == LocSouthBeacon && !SouthBeaconLit) return true;
                if (IsBeaconDarkFor(nodeId)) return true;
            }
            else
            {
                // Salt / Cluster / Shelf sit past the Cut. Same seasonal gate.
                if (!_state.isOpen) return true;
                if (!CuttersAccess) return true;
            }
            return false;
        }

        /// <summary>Closed-window Shallows run: slower, wet, not a Cut crossing.</summary>
        public float TravelHoursMultiplier(string nodeId)
        {
            if (nodeId == LocShallowsMarket && !_state.isOpen && _state.expansionUnlocked)
                return ClosedBoatTravelMultiplier;
            return 1f;
        }

        public bool ApplyOpenWindowNeeds(string nodeId, out float fatigueMul, out float warmthDrainPerHour)
        {
            fatigueMul = 1f;
            warmthDrainPerHour = 0f;
            if (!_state.isOpen || !IsCutNode(nodeId)) return false;
            fatigueMul = OpenFatigueMultiplier;
            warmthDrainPerHour = OpenWarmthDrainPerHour;
            return true;
        }

        public void SetBeaconLit(string locId, bool lit)
        {
            if (string.IsNullOrEmpty(locId)) return;
            if (locId == LocSouthBeacon) _state.southBeaconLit = lit;
            if (lit) _darkBeacons.Remove(locId);
            else
            {
                _darkBeacons.Add(locId);
                OnBeaconDark?.Invoke(locId);
            }
            if (_state.isOpen && !SouthBeaconLit)
            {
                // Dark beacon: that segment blocked even if ice is thick — road considered closed.
                Close(-1);
            }
            RaiseChanged();
        }

        public void LogAccident()
        {
            _state.accidentCount++;
            OnAccidentLogged?.Invoke();
            RaiseChanged();
        }

        /// <summary>
        /// Levy refuse: delay 11 days, then withdraw, then relight after another 11.
        /// Permanent (Yara blast / trap-lamp): road stays dark. Never call permanent
        /// for levy refuse (B5).
        /// </summary>
        public void BeginLampsOut(bool permanentWithdraw)
        {
            if (permanentWithdraw)
            {
                _state.yaraWithdrewPermanently = true;
                _state.cuttersAccess = false;
                _state.lampsOutCountdown = 0;
                SetBeaconLit(LocSouthBeacon, false);
                if (_state.isOpen) Close(-1);
            }
            else
            {
                if (_state.yaraWithdrewPermanently) return;
                // Delay withdraw. Do not close the window today.
                if (_state.lampsOutCountdown <= 0)
                    _state.lampsOutCountdown = LampsOutDays;
            }
            RaiseChanged();
        }

        /// <summary>Relight path after a non-permanent lamps-out.</summary>
        public void Relight() => RestoreCuttersAccess();

        public void RestoreCuttersAccess()
        {
            if (_state.yaraWithdrewPermanently) return;
            _state.cuttersAccess = true;
            _state.lampsOutCountdown = 0;
            _state.southBeaconLit = true;
            _darkBeacons.Remove(LocSouthBeacon);
            RaiseChanged();
        }

        public IceRoadSystemState CaptureState()
        {
            var copy = new IceRoadSystemState();
            CopyState(_state, copy);
            return copy;
        }

        public void RestoreState(IceRoadSystemState saved)
        {
            // Deep-copy: the deserialized DTO must not become the live state.
            // Otherwise the caller's save object and the running system alias
            // the same fields and a later mutation corrupts the envelope.
            _state = new IceRoadSystemState();
            if (saved != null) CopyState(saved, _state);
            if (string.IsNullOrEmpty(_state.systemId)) _state.systemId = SystemId;
            _darkBeacons.Clear();
            if (!_state.southBeaconLit) _darkBeacons.Add(LocSouthBeacon);
            RegisterDefaultHoldfastNodes();
            RaiseChanged();
        }

        private void Open(int day)
        {
            int len = SeededWindowLength(day);
            if (_state.windowsCompleted > 0 && !_state.cuttersAccess)
                len = MinWindowDays; // betrayed Cutters: shorter, not gone
            if (_state.windowLengthOverride > 0)
                len = Math.Min(len, _state.windowLengthOverride);
            _state.isOpen = true;
            _state.windowLengthDays = len;
            _state.windowDaysRemaining = len;
            _state.lastOpenDay = day;
            OnIceRoadOpened?.Invoke();
        }

        private void Close(int day)
        {
            if (!_state.isOpen) return;
            _state.isOpen = false;
            _state.windowDaysRemaining = 0;
            _state.lastCloseDay = day;
            _state.windowsCompleted++;
            OnIceRoadClosed?.Invoke();
        }

        private bool CanOpen(WeatherKind weather, int day)
        {
            if (!_state.expansionUnlocked) return false;
            if (!CuttersAccess) return false;
            if (!SouthBeaconLit) return false;
            if (_state.iceThicknessM < OpenThicknessM) return false;
            if (weather == WeatherKind.FalloutStorm) return false;
            // First window waits on the clerk (sheet → clerk → window). Later windows do not.
            if (_state.windowsCompleted == 0 && !_state.clerkStarted) return false;
            return true;
        }

        private bool CanRemainOpen(WeatherKind weather)
        {
            if (!CuttersAccess) return false;
            if (!SouthBeaconLit) return false;
            if (weather == WeatherKind.FalseSpring || weather == WeatherKind.Rain)
                return _state.iceThicknessM >= OpenThicknessM * 0.6f;
            return true;
        }

        private void AdvanceThickness(WeatherKind weather, float outdoorCelsius)
        {
            float delta = 0f;
            switch (weather)
            {
                case WeatherKind.Blizzard: delta = 0.045f; break;
                case WeatherKind.IceStorm: delta = 0.055f; break;
                case WeatherKind.BlackSnow: delta = 0.035f; break;
                case WeatherKind.FalseSpring: delta = -0.070f; break;
                case WeatherKind.Rain: delta = -0.040f; break;
                case WeatherKind.ThermalInversion: delta = -0.030f; break;
                case WeatherKind.Silence: delta = -0.025f; break;
                case WeatherKind.FalloutStorm: delta = 0f; break;
                default:
                    delta = outdoorCelsius <= -10f ? 0.018f : outdoorCelsius <= 0f ? 0.006f : -0.012f;
                    break;
            }
            _state.iceThicknessM = Math.Clamp(_state.iceThicknessM + delta, 0f, MaxThicknessM);
        }

        private void TickLampsOut()
        {
            if (_state.yaraWithdrewPermanently) return;
            if (_state.lampsOutCountdown <= 0) return;
            _state.lampsOutCountdown--;
            if (_state.lampsOutCountdown > 0) return;

            if (_state.cuttersAccess)
            {
                // Delay elapsed: withdraw now, start the 11-day dark / relight clock.
                _state.cuttersAccess = false;
                SetBeaconLit(LocSouthBeacon, false);
                _state.lampsOutCountdown = LampsOutDays;
            }
            else
            {
                RestoreCuttersAccess();
            }
        }

        private int SeededWindowLength(int day)
        {
            int salt = _state.seedSalt + day * 17 + _state.windowsCompleted * 808;
            int span = MaxWindowDays - MinWindowDays + 1;
            int n = (int)(((long)salt & 0x7FFFFFFF));
            return MinWindowDays + (n % span);
        }

        private bool IsBeaconDarkFor(string nodeId)
        {
            if (nodeId == LocSouthBeacon || nodeId == LocAccident12)
                return !SouthBeaconLit;
            return false;
        }

        private bool IsHoldfastSaltOrClusterOrShelf(string nodeId)
        {
            if (string.IsNullOrEmpty(nodeId)) return false;
            // B4: never ice-gate the three Sector 4 legacy ids.
            if (nodeId == LocAbandonedDesalination
                || nodeId == LocFrozenRiverBarge
                || nodeId == LocCrashedIcebreakerConvoy)
                return false;
            return nodeId.StartsWith("loc_salt_", StringComparison.Ordinal)
                || nodeId.StartsWith("loc_cluster_", StringComparison.Ordinal)
                || nodeId.StartsWith("loc_shelf_", StringComparison.Ordinal);
        }

        private void RegisterDefaultHoldfastNodes()
        {
            for (int i = 0; i < CutNodeIds.Length; i++)
                _holdfastNodes.Add(CutNodeIds[i]);
        }

        private void RaiseChanged() => OnStateChanged?.Invoke(_state);

        private static void CopyState(IceRoadSystemState from, IceRoadSystemState to)
        {
            to.systemId = from.systemId;
            to.expansionUnlocked = from.expansionUnlocked;
            to.isOpen = from.isOpen;
            to.iceThicknessM = from.iceThicknessM;
            to.cuttersAccess = from.cuttersAccess;
            to.southBeaconLit = from.southBeaconLit;
            to.windowDaysRemaining = from.windowDaysRemaining;
            to.windowLengthDays = from.windowLengthDays;
            to.lampsOutCountdown = from.lampsOutCountdown;
            to.windowsCompleted = from.windowsCompleted;
            to.accidentCount = from.accidentCount;
            to.lastOpenDay = from.lastOpenDay;
            to.lastCloseDay = from.lastCloseDay;
            to.clerkStarted = from.clerkStarted;
            to.yaraWithdrewPermanently = from.yaraWithdrewPermanently;
            to.seedSalt = from.seedSalt;
            to.windowLengthOverride = from.windowLengthOverride;
        }
    }
}
