using System;
using AtomicWar._Game.Environment;
using Ashfall.Core;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// UNITY ADAPTER — IceRoad logic is canonical in Ashfall.Core.IceRoadSystem
    /// (Assets/Ashfall.Core/IceRoadSystem.cs, asmdef AtomicWar.Ashfall.Core,
    /// noEngineReferences). This type keeps GameBootstrap.Holdfast / ExpeditionSystem
    /// / IceRoadSaveable on the same public events and save-blob field names.
    ///
    /// Unity Editor compile is NOT verified this pass (no batchmode). Godot
    /// Ashfall.csproj compiles both this adapter and the Core source.
    /// Casts Unity WeatherKind → Ashfall.Core.WeatherKind (identical integer values).
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
    }

    public class IceRoadSystem
    {
        public const string SystemId = global::Ashfall.Core.IceRoadSystem.SystemId;
        public const string FlagIceRoadOpen = global::Ashfall.Core.IceRoadSystem.FlagIceRoadOpen;
        public const string FlagExpUnlocked = global::Ashfall.Core.IceRoadSystem.FlagExpUnlocked;
        public const string FlagStuckNorth = global::Ashfall.Core.IceRoadSystem.FlagStuckNorth;
        public const string RegionHoldfast = global::Ashfall.Core.IceRoadSystem.RegionHoldfast;
        public const string LocIceRoadGate = global::Ashfall.Core.IceRoadSystem.LocIceRoadGate;
        public const string LocKilometre19 = global::Ashfall.Core.IceRoadSystem.LocKilometre19;
        public const string LocWeighHut = global::Ashfall.Core.IceRoadSystem.LocWeighHut;
        public const string LocDredgerHulk = global::Ashfall.Core.IceRoadSystem.LocDredgerHulk;
        public const string LocBrinePool = global::Ashfall.Core.IceRoadSystem.LocBrinePool;
        public const string LocWaystationA = global::Ashfall.Core.IceRoadSystem.LocWaystationA;
        public const string LocAccident12 = global::Ashfall.Core.IceRoadSystem.LocAccident12;
        public const string LocSouthBeacon = global::Ashfall.Core.IceRoadSystem.LocSouthBeacon;
        public const string LocShallowsMarket = global::Ashfall.Core.IceRoadSystem.LocShallowsMarket;
        public const float OpenThicknessM = global::Ashfall.Core.IceRoadSystem.OpenThicknessM;
        public const float MaxThicknessM = global::Ashfall.Core.IceRoadSystem.MaxThicknessM;
        public const int MinWindowDays = global::Ashfall.Core.IceRoadSystem.MinWindowDays;
        public const int MaxWindowDays = global::Ashfall.Core.IceRoadSystem.MaxWindowDays;
        public const int LampsOutDays = global::Ashfall.Core.IceRoadSystem.LampsOutDays;
        public const float ClosedBoatTravelMultiplier = global::Ashfall.Core.IceRoadSystem.ClosedBoatTravelMultiplier;
        public const float OpenFatigueMultiplier = global::Ashfall.Core.IceRoadSystem.OpenFatigueMultiplier;
        public const float OpenWarmthDrainPerHour = global::Ashfall.Core.IceRoadSystem.OpenWarmthDrainPerHour;
        public const float IceAlbedoUvMultiplier = global::Ashfall.Core.IceRoadSystem.IceAlbedoUvMultiplier;
        public static readonly string[] CutNodeIds = global::Ashfall.Core.IceRoadSystem.CutNodeIds;

        private readonly global::Ashfall.Core.IceRoadSystem _core;

        public event Action OnIceRoadOpened;
        public event Action OnIceRoadClosed;
        public event Action<string> OnBeaconDark;
        public event Action OnAccidentLogged;
        public event Action<IceRoadSystemState> OnStateChanged;

        public IceRoadSystemState State => FromCore(_core.State);
        public bool IsOpen => _core.IsOpen;
        public bool IsUnlocked => _core.IsUnlocked;
        public bool CuttersAccess => _core.CuttersAccess;
        public float IceThicknessM => _core.IceThicknessM;
        public bool SouthBeaconLit => _core.SouthBeaconLit;
        public int WindowDaysRemaining => _core.WindowDaysRemaining;

        public IceRoadSystem()
        {
            _core = new global::Ashfall.Core.IceRoadSystem();
            Bind();
        }

        public IceRoadSystem(int seedSalt)
        {
            _core = new global::Ashfall.Core.IceRoadSystem(seedSalt);
            Bind();
        }

        public void Initialise(int seedSalt) => _core.Initialise(seedSalt);
        public void Unlock(int day) => _core.Unlock(day);
        public void NotifyClerkStarted() => _core.NotifyClerkStarted();

        public void TickDaily(int day, WeatherKind weather, float outdoorCelsius)
        {
            _core.TickDaily(day, (global::Ashfall.Core.WeatherKind)(int)weather, outdoorCelsius);
        }

        public bool IsCutNode(string nodeId) => _core.IsCutNode(nodeId);
        public bool IsHoldfastNode(string nodeId) => _core.IsHoldfastNode(nodeId);
        public void RegisterHoldfastNode(string nodeId) => _core.RegisterHoldfastNode(nodeId);
        public bool IsTravelBlocked(string nodeId) => _core.IsTravelBlocked(nodeId);
        public float TravelHoursMultiplier(string nodeId) => _core.TravelHoursMultiplier(nodeId);

        public bool ApplyOpenWindowNeeds(string nodeId, out float fatigueMul, out float warmthDrainPerHour) =>
            _core.ApplyOpenWindowNeeds(nodeId, out fatigueMul, out warmthDrainPerHour);

        public void SetBeaconLit(string locId, bool lit) => _core.SetBeaconLit(locId, lit);
        public void LogAccident() => _core.LogAccident();
        public void BeginLampsOut(bool permanentWithdraw) => _core.BeginLampsOut(permanentWithdraw);
        public void Relight() => _core.Relight();
        public void RestoreCuttersAccess() => _core.RestoreCuttersAccess();

        public IceRoadSystemState CaptureState() => FromCore(_core.CaptureState());

        public void RestoreState(IceRoadSystemState saved)
        {
            _core.RestoreState(ToCore(saved));
        }

        private void Bind()
        {
            _core.OnIceRoadOpened += () => OnIceRoadOpened?.Invoke();
            _core.OnIceRoadClosed += () => OnIceRoadClosed?.Invoke();
            _core.OnBeaconDark += loc => OnBeaconDark?.Invoke(loc);
            _core.OnAccidentLogged += () => OnAccidentLogged?.Invoke();
            _core.OnStateChanged += _ => OnStateChanged?.Invoke(CaptureState());
        }

        private static IceRoadSystemState FromCore(global::Ashfall.Core.IceRoadSystemState c)
        {
            if (c == null) return new IceRoadSystemState();
            return new IceRoadSystemState
            {
                systemId = c.systemId,
                expansionUnlocked = c.expansionUnlocked,
                isOpen = c.isOpen,
                iceThicknessM = c.iceThicknessM,
                cuttersAccess = c.cuttersAccess,
                southBeaconLit = c.southBeaconLit,
                windowDaysRemaining = c.windowDaysRemaining,
                windowLengthDays = c.windowLengthDays,
                lampsOutCountdown = c.lampsOutCountdown,
                windowsCompleted = c.windowsCompleted,
                accidentCount = c.accidentCount,
                lastOpenDay = c.lastOpenDay,
                lastCloseDay = c.lastCloseDay,
                clerkStarted = c.clerkStarted,
                yaraWithdrewPermanently = c.yaraWithdrewPermanently,
                seedSalt = c.seedSalt
            };
        }

        private static global::Ashfall.Core.IceRoadSystemState ToCore(IceRoadSystemState u)
        {
            if (u == null) return new global::Ashfall.Core.IceRoadSystemState();
            return new global::Ashfall.Core.IceRoadSystemState
            {
                systemId = u.systemId,
                expansionUnlocked = u.expansionUnlocked,
                isOpen = u.isOpen,
                iceThicknessM = u.iceThicknessM,
                cuttersAccess = u.cuttersAccess,
                southBeaconLit = u.southBeaconLit,
                windowDaysRemaining = u.windowDaysRemaining,
                windowLengthDays = u.windowLengthDays,
                lampsOutCountdown = u.lampsOutCountdown,
                windowsCompleted = u.windowsCompleted,
                accidentCount = u.accidentCount,
                lastOpenDay = u.lastOpenDay,
                lastCloseDay = u.lastCloseDay,
                clerkStarted = u.clerkStarted,
                yaraWithdrewPermanently = u.yaraWithdrewPermanently,
                seedSalt = u.seedSalt
            };
        }
    }
}
