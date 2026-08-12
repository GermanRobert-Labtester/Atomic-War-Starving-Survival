using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Endgame
{
    /// <summary>
    /// Expansion III — The Architecture of Leaving. The Migration victory is not
    /// "build a car and drive away." It is a grueling, multi-stage logistical
    /// nightmare that requires sacrificing the bunker.
    ///
    /// Phase 1: The Chassis — 30 days rebuilding a snow-crawler. Cannibalizes heater.
    /// Phase 2: The Route — Find Cartographer's Map fragments. Avoid GlassDesert/Sinkholes.
    /// Phase 3: The Fuel — 400L of fuel. Choose: heater for elderly/children, or siphon.
    /// Phase 4: The Culling — 4 seats. 6 survivors. Who stays behind?
    /// </summary>
    public class Victory_Migration
    {
        // ── Phase constants ───────────────────────────────────────────
        public const string VehicleId = "vehicle_snow_crawler";
        public const int BuildDaysRequired = 30;
        public const float FuelRequiredLiters = 400f;
        public const int CrawlerSeats = 4;
        public const int TollResourcePercent = 50; // Warlord toll: 50% of bunker resources

        // ── Required components for snow-crawler ──────────────────────
        public const string Component_Engine = "engine";
        public const string Component_TungstenBar = "tungsten_bar";
        public const string Component_TracksSalvaged = "tracks_salvaged";
        public const string Component_WeldingRod = "welding_rod_pack_20";

        // ── Route requirements ────────────────────────────────────────
        public const int MapFragmentsRequired = 4;
        public const string HazardRouteId = "route_migration";

        // ── Phase enum ────────────────────────────────────────────────
        public enum MigrationPhase
        {
            NotStarted,
            Phase1_Chassis,       // Building the snow-crawler
            Phase2_Route,         // Finding the route
            Phase3_Fuel,          // Gathering 400L of fuel
            Phase4_Culling,       // Choosing who stays
            Phase5_Departure,     // The hatch opens
            Completed
        }

        [Serializable]
        public class MigrationState
        {
            public string victoryId = "victory_migration";
            public MigrationPhase Phase = MigrationPhase.NotStarted;
            public bool requiresArmoredTruck = true;
            public bool bunkerAbandoned = false;

            // Phase 1: Chassis
            public int BuildDaysElapsed;
            public bool HasEngine;
            public bool HasTungstenBar;
            public bool HasTracksSalvaged;
            public bool HasWeldingRod;
            public bool ChassisComplete;

            // Phase 2: Route
            public int MapFragmentsFound;
            public bool RoutePlanned;
            public bool TollPaid;
            public List<string> RouteHazardIds = new List<string>();

            // Phase 3: Fuel
            public float FuelCollectedLiters;
            public float FuelSiphonedFromGenerator;
            public bool GeneratorDrained;
            public bool BunkerFrozen;

            // Phase 4: Culling
            public int SurvivorCount;
            public int SeatsAvailable = CrawlerSeats;
            public List<string> DepartingSurvivorIds = new List<string>();
            public List<string> StayingSurvivorIds = new List<string>();
            public string StayBehindVolunteerId;
            public bool DepartureChosen;

            // Phase 5: Departure
            public bool HasDeparted;
            public int MilesDriven;
            public float LitersBurned;
            public int SoulsLeftBehind;
        }

        private MigrationState _state = new MigrationState();
        private System.Random _rng;

        // ── Events ────────────────────────────────────────────────────
        public event Action<MigrationPhase> OnPhaseChanged;
        public event Action OnChassisComplete;
        public event Action OnRoutePlanned;
        public event Action OnTollDemanded;
        public event Action OnFuelCrisis;             // Must choose: heater or crawler
        public event Action OnBunkerFreezes;          // Generator drained
        public event Action<string> OnVolunteerStay;  // survivorId
        public event Action OnDeparture;
        public event Action OnMigrationComplete;

        public MigrationState State => _state;
        public MigrationPhase Phase => _state.Phase;

        public Victory_Migration(System.Random rng = null)
        {
            _rng = rng ?? new System.Random(2077);
        }

        // ── Phase 1: The Chassis ──────────────────────────────────────

        /// <summary>Begin the migration victory path.</summary>
        public bool BeginMigration()
        {
            if (_state.Phase != MigrationPhase.NotStarted) return false;
            _state.Phase = MigrationPhase.Phase1_Chassis;
            OnPhaseChanged?.Invoke(_state.Phase);
            return true;
        }

        /// <summary>Deliver a required component for the snow-crawler.</summary>
        public bool DeliverComponent(string componentId)
        {
            if (_state.Phase != MigrationPhase.Phase1_Chassis) return false;

            switch (componentId)
            {
                case Component_Engine: _state.HasEngine = true; break;
                case Component_TungstenBar: _state.HasTungstenBar = true; break;
                case Component_TracksSalvaged: _state.HasTracksSalvaged = true; break;
                case Component_WeldingRod: _state.HasWeldingRod = true; break;
                default: return false;
            }
            return true;
        }

        /// <summary>
        /// Tick the chassis build forward (called daily by mechanic work).
        /// After 30 days of work, the snow-crawler is complete.
        /// While building, the bunker is freezing because the heater was cannibalized.
        /// </summary>
        public bool TickBuild(float workDays)
        {
            if (_state.Phase != MigrationPhase.Phase1_Chassis) return false;
            if (!_state.HasEngine || !_state.HasTungstenBar
                || !_state.HasTracksSalvaged || !_state.HasWeldingRod)
                return false;

            _state.BuildDaysElapsed += Mathf.RoundToInt(workDays);

            if (_state.BuildDaysElapsed >= BuildDaysRequired)
            {
                _state.ChassisComplete = true;
                _state.Phase = MigrationPhase.Phase2_Route;
                OnChassisComplete?.Invoke();
                OnPhaseChanged?.Invoke(_state.Phase);
            }
            return _state.ChassisComplete;
        }

        /// <summary>True when all components are delivered and build days met.</summary>
        public bool IsChassisComplete => _state.ChassisComplete;

        // ── Phase 2: The Route ────────────────────────────────────────

        /// <summary>
        /// Find a Cartographer's Map fragment. Need 4 total to plan route.
        /// </summary>
        public bool FindMapFragment()
        {
            if (_state.Phase != MigrationPhase.Phase2_Route) return false;
            _state.MapFragmentsFound++;

            if (_state.MapFragmentsFound >= MapFragmentsRequired)
            {
                // Route must avoid known hazards
                _state.RouteHazardIds.Add("biome_glass_desert");
                _state.RouteHazardIds.Add("map_hazard_sinkhole_collapse");
                _state.RoutePlanned = true;

                // Warlord toll
                OnTollDemanded?.Invoke();
                _state.Phase = MigrationPhase.Phase3_Fuel;
                OnRoutePlanned?.Invoke();
                OnPhaseChanged?.Invoke(_state.Phase);
            }
            return true;
        }

        /// <summary>
        /// Pay the warlord toll: 50% of bunker's total resources.
        /// </summary>
        public bool PayToll()
        {
            if (_state.Phase != MigrationPhase.Phase3_Fuel && !_state.RoutePlanned) return false;
            _state.TollPaid = true;
            return true;
        }

        public int MapFragmentsFound => _state.MapFragmentsFound;
        public bool IsRoutePlanned => _state.RoutePlanned;
        public bool IsTollPaid => _state.TollPaid;

        // ── Phase 3: The Fuel ─────────────────────────────────────────

        /// <summary>
        /// Add fuel to the crawler's reserves. Returns true when 400L collected.
        /// </summary>
        public bool AddFuel(float liters)
        {
            if (_state.Phase != MigrationPhase.Phase3_Fuel) return false;
            if (!_state.TollPaid) return false;

            _state.FuelCollectedLiters = Mathf.Min(
                _state.FuelCollectedLiters + liters, FuelRequiredLiters);

            if (_state.FuelCollectedLiters >= FuelRequiredLiters)
            {
                _state.Phase = MigrationPhase.Phase4_Culling;
                OnPhaseChanged?.Invoke(_state.Phase);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Siphon fuel from the bunker's generator. The bunker freezes.
        /// People die while you prepare to leave.
        /// </summary>
        public float SiphonFromGenerator(float availableLiters)
        {
            if (_state.Phase != MigrationPhase.Phase3_Fuel) return 0f;

            float needed = FuelRequiredLiters - _state.FuelCollectedLiters;
            float siphoned = Mathf.Min(availableLiters, needed);

            _state.FuelSiphonedFromGenerator += siphoned;
            _state.FuelCollectedLiters += siphoned;

            // If generator is drained, the bunker freezes
            if (availableLiters <= siphoned + 0.1f)
            {
                _state.GeneratorDrained = true;
                _state.BunkerFrozen = true;
                OnBunkerFreezes?.Invoke();
                OnFuelCrisis?.Invoke();
            }

            if (_state.FuelCollectedLiters >= FuelRequiredLiters)
            {
                _state.Phase = MigrationPhase.Phase4_Culling;
                OnPhaseChanged?.Invoke(_state.Phase);
            }

            return siphoned;
        }

        public float FuelCollected => _state.FuelCollectedLiters;
        public float FuelNeeded => Mathf.Max(0f, FuelRequiredLiters - _state.FuelCollectedLiters);
        public bool IsGeneratorDrained => _state.GeneratorDrained;

        // ── Phase 4: The Culling ──────────────────────────────────────

        /// <summary>
        /// Choose who departs and who stays. The crawler only has 4 seats.
        /// The the_martyr will volunteer. The the_general will order the weak to stay.
        /// The the_fierce_mother will refuse to leave her child.
        /// </summary>
        public bool ChooseDeparting(List<string> departingIds, List<string> stayingIds,
            string volunteerId = null)
        {
            if (_state.Phase != MigrationPhase.Phase4_Culling) return false;
            if (departingIds == null || departingIds.Count > CrawlerSeats) return false;
            if (stayingIds == null || stayingIds.Count == 0) return false;

            _state.DepartingSurvivorIds.Clear();
            _state.DepartingSurvivorIds.AddRange(departingIds);
            _state.StayingSurvivorIds.Clear();
            _state.StayingSurvivorIds.AddRange(stayingIds);
            _state.SurvivorCount = departingIds.Count + stayingIds.Count;
            _state.SeatsAvailable = CrawlerSeats;
            _state.DepartureChosen = true;

            if (!string.IsNullOrEmpty(volunteerId))
            {
                _state.StayBehindVolunteerId = volunteerId;
                OnVolunteerStay?.Invoke(volunteerId);
            }

            _state.Phase = MigrationPhase.Phase5_Departure;
            OnPhaseChanged?.Invoke(_state.Phase);
            return true;
        }

        /// <summary>
        /// The hatch opens. The crawler drives out into the white.
        /// The survivors left behind close the hatch and seal it from the inside.
        /// </summary>
        public bool ExecuteDeparture()
        {
            if (_state.Phase != MigrationPhase.Phase5_Departure) return false;
            if (!_state.DepartureChosen) return false;

            _state.HasDeparted = true;
            _state.SoulsLeftBehind = _state.StayingSurvivorIds.Count;
            _state.MilesDriven = _rng.Next(80, 200); // Random distance
            _state.LitersBurned = FuelRequiredLiters;

            _state.Phase = MigrationPhase.Completed;
            _state.bunkerAbandoned = true;

            OnDeparture?.Invoke();
            OnMigrationComplete?.Invoke();
            OnEndingTriggered?.Invoke();
            return true;
        }

        // ── Epilogue ──────────────────────────────────────────────────

        /// <summary>
        /// The screen fades to white. The radio crackles. You hear the
        /// crawler's engine. Then, the sound of ice cracking. The screen
        /// cuts to black.
        /// </summary>
        public string GetEpilogueText()
        {
            return "The screen fades to white. The radio crackles. "
                 + "You hear the crawler's engine. Then, the sound of ice cracking. "
                 + "The screen cuts to black.\n\n"
                 + "── THE MORAL CHRONICLE ──\n"
                 + $"Miles Driven: {_state.MilesDriven}\n"
                 + $"Liters Burned: {_state.LitersBurned:F0}\n"
                 + $"Souls Left in the Dark: {_state.SoulsLeftBehind}";
        }

        /// <summary>Get the departure narration.</summary>
        public string GetDepartureText()
        {
            string volunteer = !string.IsNullOrEmpty(_state.StayBehindVolunteerId)
                ? "The martyr volunteered. " : "";

            return "The hatch opens. The crawler drives out into the white. "
                 + $"{_state.StayingSurvivorIds.Count} survivors stand in the doorway. "
                 + "They do not wave. They close the hatch. They seal it from the inside. "
                 + "They wait for the ash to bury them.\n\n"
                 + volunteer
                 + "The engine turns over. The road ahead is ash and silence, "
                 + "but the wheels are moving.";
        }

        // ── Legacy compatibility ──────────────────────────────────────

        public event Action OnEndingTriggered;
        public event Action OnBunkerAbandoned;

        public bool CheckVictory(bool hasArmoredTruck, bool mapDepleted, int survivorCount)
        {
            // Legacy path: if all phases complete, migration is achieved
            if (_state.Phase == MigrationPhase.Completed)
                return true;

            // Legacy fallback: simple check
            if (_state.requiresArmoredTruck && !hasArmoredTruck) return false;
            if (!mapDepleted) return false;
            if (survivorCount < 1) return false;

            _state.bunkerAbandoned = true;
            OnBunkerAbandoned?.Invoke();
            OnEndingTriggered?.Invoke();
            return true;
        }

        public string GetEndingText(int survivorCount, int supplyCount)
        {
            string survivorWord = survivorCount == 1 ? "survivor" : "survivors";
            string supplyWord = supplyCount == 1 ? "crate" : "crates";

            return $"The bunker doors opened for the last time. "
                 + $"{survivorCount} {survivorWord} climbed into the armored truck "
                 + $"with {supplyCount} {supplyWord} of whatever was left. "
                 + "The engine turned over. The road ahead was ash and silence, "
                 + "but the wheels were moving. "
                 + "Maybe there is something out there. Maybe there isn't. "
                 + "They drive anyway.";
        }

        public bool IsVictoryAchieved()
        {
            return _state.bunkerAbandoned;
        }

        // ── Save / Load ───────────────────────────────────────────────

        public MigrationState CaptureState()
        {
            return new MigrationState
            {
                victoryId = _state.victoryId,
                Phase = _state.Phase,
                requiresArmoredTruck = _state.requiresArmoredTruck,
                bunkerAbandoned = _state.bunkerAbandoned,
                BuildDaysElapsed = _state.BuildDaysElapsed,
                HasEngine = _state.HasEngine,
                HasTungstenBar = _state.HasTungstenBar,
                HasTracksSalvaged = _state.HasTracksSalvaged,
                HasWeldingRod = _state.HasWeldingRod,
                ChassisComplete = _state.ChassisComplete,
                MapFragmentsFound = _state.MapFragmentsFound,
                RoutePlanned = _state.RoutePlanned,
                TollPaid = _state.TollPaid,
                RouteHazardIds = new List<string>(_state.RouteHazardIds),
                FuelCollectedLiters = _state.FuelCollectedLiters,
                FuelSiphonedFromGenerator = _state.FuelSiphonedFromGenerator,
                GeneratorDrained = _state.GeneratorDrained,
                BunkerFrozen = _state.BunkerFrozen,
                SurvivorCount = _state.SurvivorCount,
                SeatsAvailable = _state.SeatsAvailable,
                DepartingSurvivorIds = new List<string>(_state.DepartingSurvivorIds),
                StayingSurvivorIds = new List<string>(_state.StayingSurvivorIds),
                StayBehindVolunteerId = _state.StayBehindVolunteerId,
                DepartureChosen = _state.DepartureChosen,
                HasDeparted = _state.HasDeparted,
                MilesDriven = _state.MilesDriven,
                LitersBurned = _state.LitersBurned,
                SoulsLeftBehind = _state.SoulsLeftBehind,
            };
        }

        public void RestoreState(MigrationState state)
        {
            if (state == null)
            {
                _state = new MigrationState();
                return;
            }
            _state = state;
        }
    }
}
