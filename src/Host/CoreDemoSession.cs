using Ashfall.Core;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Thin Godot-host wrapper around existing Ashfall.Core types.
    /// Does not invent IceRoad / Census / Crossing rules.
    /// </summary>
    public sealed class CoreDemoSession
    {
        public const int DefaultSeed = 808;
        public const int DefaultStartDay = 90;

        private static readonly WeatherKind[] WeatherCycle =
        {
            WeatherKind.Blizzard,
            WeatherKind.IceStorm,
            WeatherKind.Clear,
            WeatherKind.FalloutStorm,
            WeatherKind.Rain,
            WeatherKind.FalseSpring
        };

        private int _weatherIndex;
        private bool _outfallShifted;

        public IceRoadSystem IceRoad { get; }
        public SimClock Clock { get; }
        public HoldfastCatalog Catalog { get; }
        public CensusClaimSystem Census { get; }
        public BrineWaterSystem Brine { get; }
        public LocationLayoutSystem Layouts { get; }
        public WeatherKind Weather { get; private set; }
        public float OutdoorCelsius { get; private set; }
        public int QuestIndex { get; private set; }
        public string LastEvent { get; private set; } = string.Empty;

        public int LocationCount => Catalog.Locations.Count;
        public int QuestCount => Catalog.Quests.Count;

        public bool OutfallShifted => _outfallShifted;

        public HoldfastQuestEntry? CurrentQuest =>
            Catalog.Quests.Count == 0 ? null : Catalog.Quests[QuestIndex];

        public CoreDemoSession(
            IceRoadSystem iceRoad,
            SimClock clock,
            HoldfastCatalog catalog,
            CensusClaimSystem census,
            BrineWaterSystem brine,
            LocationLayoutSystem layouts)
        {
            IceRoad = iceRoad;
            Clock = clock;
            Catalog = catalog;
            Census = census;
            Brine = brine;
            Layouts = layouts;
            Weather = WeatherKind.Blizzard;
            OutdoorCelsius = TempFor(Weather);

            IceRoad.OnIceRoadOpened += () => LastEvent = "WINDOW OPENED";
            IceRoad.OnIceRoadClosed += () => LastEvent = "WINDOW CLOSED";
            IceRoad.OnBeaconDark += loc => LastEvent = "beacon dark: " + loc;
            IceRoad.OnAccidentLogged += () => LastEvent = "accident logged";
            Census.OnLevyResolved += flag => LastEvent = "levy: " + flag;
            Brine.OnSteamTrip += () => LastEvent = "STEAM TRIP";
        }

        public static CoreDemoSession Create(string dataDirectory, ILog? log = null)
        {
            CatalogLocator.UseInvariantCulture();
            log ??= new GodotLog();
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var loader = new HoldfastCatalogLoader(files, json, log);
            var catalog = loader.Load(dataDirectory);
            var ice = new IceRoadSystem(DefaultSeed);
            var clock = new SimClock(DefaultStartDay);
            var census = new CensusClaimSystem();
            var brine = new BrineWaterSystem();
            var layouts = new LocationLayoutSystem(files, json, log);
            layouts.Load(dataDirectory);
            return new CoreDemoSession(ice, clock, catalog, census, brine, layouts);
        }

        public void UnlockAndClerk()
        {
            if (!IceRoad.IsUnlocked)
                IceRoad.Unlock(Clock.Day);
            if (!IceRoad.State.clerkStarted)
                IceRoad.NotifyClerkStarted();
        }

        /// <summary>Cross-host save envelope. Shape and checksum owned by HoldfastSaveCodec.</summary>
        public HoldfastSave CaptureSave() =>
            HoldfastSaveCodec.Capture(IceRoad, Census, Brine, Clock);

        public void RestoreSave(HoldfastSave save) =>
            HoldfastSaveCodec.Restore(save, IceRoad, Census, Brine, Clock);

        public string TickDay()
        {
            UnlockAndClerk();
            LastEvent = string.Empty;
            bool wasOpen = IceRoad.IsOpen;
            Clock.AdvanceDays(1);
            IceRoad.TickDaily(Clock.Day, Weather, OutdoorCelsius);
            Census.TickDaily(Clock.Day);
            Brine.TickDaily(Clock.Day, Weather, OutdoorCelsius, _outfallShifted);
            if (!string.IsNullOrEmpty(LastEvent))
                return LastEvent;
            return IceRoad.IsOpen == wasOpen
                ? (IceRoad.IsOpen ? "window holds" : "still closed")
                : (IceRoad.IsOpen ? "WINDOW OPENED" : "WINDOW CLOSED");
        }

        public void UnlockPlant() => Brine.UnlockSaltTrade();

        public void ToggleOutfallShift() => _outfallShifted = !_outfallShifted;

        public bool RepairMembrane(int drums) => Brine.RepairWithResin(drums);

        public void CycleWeather()
        {
            _weatherIndex = (_weatherIndex + 1) % WeatherCycle.Length;
            Weather = WeatherCycle[_weatherIndex];
            OutdoorCelsius = TempFor(Weather);
        }

        public void AdvanceQuest()
        {
            if (Catalog.Quests.Count == 0) return;
            QuestIndex = (QuestIndex + 1) % Catalog.Quests.Count;
        }

        public bool GateBlocked => IceRoad.IsTravelBlocked(IceRoadSystem.LocIceRoadGate);

        public string StatusLine()
        {
            string gate = Catalog.GetLocation(IceRoadSystem.LocIceRoadGate)?.displayName
                ?? IceRoadSystem.LocIceRoadGate;
            string open = IceRoad.IsOpen ? "OPEN" : "CLOSED";
            string unlocked = IceRoad.IsUnlocked ? "unlocked" : "dark";
            string blocked = GateBlocked ? "gate blocked" : "gate passable";
            return
                $"Ice road ({gate}): {open} · {unlocked} · ice {IceRoad.IceThicknessM:0.00} m · " +
                $"window {IceRoad.WindowDaysRemaining}d · day {Clock.Day} · " +
                $"{Weather} {OutdoorCelsius:0}°C · {blocked} · " +
                $"locs {LocationCount} quests {QuestCount}";
        }

        public string BrineLine()
        {
            string steam = Brine.SteamTripped
                ? "TRIPPED " + Brine.State.hoursSinceTrip + "h"
                : "on";
            string plant = Brine.Unlocked ? "unlocked" : "dormant";
            return
                $"Brine: {plant} · membrane {Brine.MembraneIntegrity:0.0}% · steam {steam} · " +
                $"cluster {Brine.ClusterIndoorC:0.0}°C · outfall {(OutfallShifted ? "shifted" : "normal")} · " +
                $"salt trade {(Brine.State.saltTradeUnlocked ? "open" : "closed")}";
        }

        public string HonourDemoLevy()
        {
            string[] ids = { "elena_vasquez", "marcus_olejnik", "suki_tanaka" };
            if (!Census.IssueLevy(ids, Clock.Day))
            {
                if (Census.ActiveLevy != null && Census.ActiveLevy.active)
                    return "census levy already active (" + Census.ActiveLevy.remainingDays + "d remaining)";
                return "census levy not issued";
            }

            Census.HonourLevy();
            return "census levy honoured · " + Census.AssignedAwayIds().Count + " assigned away";
        }

        public string CatalogLine()
        {
            if (LocationCount == 0 && QuestCount == 0)
                return "Holdfast catalog: empty — check ASHFALL_DATA / Assets/StreamingAssets/Data";
            return $"Holdfast catalog: {LocationCount} locations · {QuestCount} quests · layouts {Layouts.LayoutCount}";
        }

        public string CensusLine()
        {
            if (Census.ActiveLevy != null && Census.ActiveLevy.active)
                return $"Census: levy active {Census.ActiveLevy.remainingDays}d · away {Census.AssignedAwayIds().Count}";
            if (Census.LevyRefuse)
                return "Census: refused · Edor at hatch";
            return $"Census: ledger {Census.State.ledger.Count} · trust {Census.State.officeTrust:0}";
        }

        private static float TempFor(WeatherKind weather)
        {
            switch (weather)
            {
                case WeatherKind.Blizzard: return -22f;
                case WeatherKind.IceStorm: return -25f;
                case WeatherKind.Clear: return -12f;
                case WeatherKind.FalloutStorm: return -30f;
                case WeatherKind.Rain: return 2f;
                case WeatherKind.FalseSpring: return 4f;
                default: return -18f;
            }
        }
    }
}
