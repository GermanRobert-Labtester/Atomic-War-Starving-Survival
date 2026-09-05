using System;
#pragma warning disable CS8618
using Ashfall.Core;
using Ashfall.Core.Shelter;
using Ashfall.Core.World;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Thin Godot-host session for the World port (weather core). Loads the
    /// season profile JSON, ticks the weather clock, persists state. No rules
    /// here — hosts only wire and present.
    /// </summary>
    public sealed class WorldHostSession
    : HostSessionBase{
        public const int DemoSeed = 1234;

        public WeatherSystem Weather { get; }
        public SkyLayerArmorSystem SkyArmor { get; }
        public WeatherIntelligenceCoordinator WeatherIntelligence { get; }
        public LocationEvolutionSystem LocationEvolution { get; }
        public WildlifeMigrationSystem Wildlife { get; }
        public LandmarkDegradationSystem Landmarks { get; }
        public WastelandMapSystem WastelandMap { get; }
        public DamagedMapSystem? DamagedMap { get; private set; }
        public SeasonProfileDef Profile { get; private set; }

        /// <summary>Plan 48 weather gate catalog loaded from weather_route_gates.json.</summary>
        public WeatherGateCatalog GateCatalog { get; private set; } = new WeatherGateCatalog();

        /// <summary>Location atmosphere flavor texts loaded from environmental_atmosphere_expansion.json.</summary>
        public AtmosphereTextSystem AtmosphereTexts { get; } = new AtmosphereTextSystem();

        /// <summary>Environmental flavor texts loaded from environmental_texts_expansion_05.json.</summary>
        public EnvironmentalTextSystem EnvironmentalTexts { get; } = new EnvironmentalTextSystem();

        /// <summary>
        /// Seed catalog for the evolving-world trio (loaded once in Create).
        /// Null when no data dir was provided; hosts read the shelter sector
        /// and scarcity goods from here.
        /// </summary>
        public EvolvingWorldSeedContainer? Seeds { get; private set; }

        public string ShelterSectorId => EvolvingWorldSeeder.ShelterSectorId(Seeds);

        /// <summary>
        /// Plan 28 Phase 5 (28L) — coarse wildlife sighting for a location,
        /// via its seed-bound sector. Discovery-gated: unknown ground reads
        /// empty. "holding" | "passing" | "" (never a population count).
        /// </summary>
        public string WildlifeSightingFor(string locationId)
        {
            if (Wildlife == null || Seeds?.location_seeds == null) return string.Empty;
            string? sector = null;
            foreach (var seed in Seeds.location_seeds)
                if (seed != null && string.Equals(seed.location_id, locationId, StringComparison.Ordinal))
                { sector = seed.sector_id; break; }
            if (string.IsNullOrEmpty(sector)) return string.Empty;

            int pop = Wildlife.GetSectorPackPopulation(sector);
            if (pop <= 0) return string.Empty;
            return pop >= 8 ? "wildlife holding" : "wildlife passing";
        }

        /// <summary>28L overview contract — home-sector wildlife band, no counts.</summary>
        public string HomeSectorWildlifeStatus()
        {
            if (Wildlife == null || string.IsNullOrEmpty(ShelterSectorId)) return string.Empty;
            int pop = Wildlife.GetSectorPackPopulation(ShelterSectorId);
            if (pop <= 0) return string.Empty;
            return pop >= 8 ? "Wildlife: herds reported" : "Wildlife: movement reported";
        }

        public string LastEvent { get; private set; } = string.Empty;
        public WorldHostSession(
            WeatherSystem weather = null!,
            SkyLayerArmorSystem skyArmor = null!,
            LocationEvolutionSystem locationEvolution = null!,
            WildlifeMigrationSystem wildlife = null!,
            LandmarkDegradationSystem landmarks = null!,
            WastelandMapSystem wastelandMap = null!,
            DamagedMapSystem? damagedMap = null)
        {
            Weather = weather ?? new WeatherSystem();
            SkyArmor = skyArmor ?? new SkyLayerArmorSystem();
            WeatherIntelligence = new WeatherIntelligenceCoordinator(Weather, SkyArmor, new SeededRng(DemoSeed));
            LocationEvolution = locationEvolution ?? new LocationEvolutionSystem();
            Wildlife = wildlife ?? new WildlifeMigrationSystem();
            Landmarks = landmarks ?? new LandmarkDegradationSystem();
            WastelandMap = wastelandMap ?? WastelandMapCatalogLoader.CreateSystem(string.Empty);
            DamagedMap = damagedMap;
            Weather.OnWeatherChanged += kind =>
            {
                LastEvent = $"Weather: {kind}";
                if (IsHazardWeather(kind))
                    AtomicWar.GodotApp.Audio.AudioManager.Instance?.PlayWeatherAlert();
                RaiseStateChanged();
            };
            Weather.OnStateChanged += _ => RaiseStateChanged();
            WeatherIntelligence.OnIntelligenceChanged += () => RaiseStateChanged();
        }

        public static WorldHostSession Create(string dataDir)
        {
            var mapSystem = !string.IsNullOrEmpty(dataDir)
                ? WastelandMapCatalogLoader.CreateSystem(dataDir)
                : null!;
            var session = new WorldHostSession(wastelandMap: mapSystem);
            if (!string.IsNullOrEmpty(dataDir))
            {
                session.DamagedMap = DamagedMapCatalogLoader.CreateSystem(dataDir, session.WastelandMap);
            }
            var profile = !string.IsNullOrEmpty(dataDir)
                ? WeatherProfileLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer())
                : null;
            if (profile != null)
            {
                session.Profile = profile;
                session.Weather.BindProfile(profile, DemoSeed);
                // Plan 28: the same Plan 19 authority paces wildlife abundance.
                session.Wildlife.BindSeasonProfile(profile);
            }
            var env = WorldSaveStore.TryLoadEnvelope();
            if (env != null)
            {
                if (env.State != null) session.Weather.RestoreState(env.State);
                if (env.SkyArmor != null) session.SkyArmor.RestoreState(env.SkyArmor);
                if (env.WeatherIntelligence != null) session.WeatherIntelligence.RestoreState(env.WeatherIntelligence);
                if (env.LocationEvolution != null) session.LocationEvolution.RestoreState(env.LocationEvolution);
                if (env.Wildlife != null) session.Wildlife.RestoreState(env.Wildlife);
                if (env.Landmark != null) session.Landmarks.RestoreState(env.Landmark);
                session.LastEvent = "World state restored from save.";
            }

            // Evolving-world activation (task 122): load the seed authority,
            // then seed — AFTER restore, and only into empty ledgers, so a
            // restored save is never overwritten by the starting world.
            session.Seeds = !string.IsNullOrEmpty(dataDir)
                ? EvolvingWorldCatalogLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer())
                : null;
            EvolvingWorldSeeder.Seed(session.LocationEvolution, session.Wildlife, session.Landmarks, session.Seeds);

            // Seasonal events activation (Plan 19)
            var seasonalEvents = !string.IsNullOrEmpty(dataDir)
                ? SeasonalEventCatalogLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer())
                : null;
            if (seasonalEvents != null && seasonalEvents.Count > 0)
                session.WeatherIntelligence.Seasonal.BindDefinitions(seasonalEvents);

            // Atmosphere / environmental flavor catalogs — consumed by location
            // presentation (expedition/map detail) via FlavorTextForLocation.
            if (!string.IsNullOrEmpty(dataDir))
            {
                var files = new FileSystemIO();
                var json = new SystemTextJsonSerializer();
                session.GateCatalog = WeatherGateCatalogLoader.LoadFromDirectory(dataDir, files, json);
                session.WeatherIntelligence.Station.GateCatalog = session.GateCatalog;
                AtmosphereCatalogLoader.LoadAndRegister(session.AtmosphereTexts, dataDir, files, json);
                EnvironmentalTextCatalogLoader.LoadAndRegister(session.EnvironmentalTexts, dataDir, files, json);
            }

            var mapSave = WastelandMapSaveStore.TryLoad();
            if (mapSave != null)
                session.WastelandMap.RestoreState(mapSave);
            return session;
        }

        /// <summary>
        /// Prefer atmosphere catalog text for a location; fall back to environmental texts.
        /// Empty string when neither catalog has an entry (presentation must handle silence).
        /// </summary>
        public string FlavorTextForLocation(string locationId, string? weather = null)
        {
            if (string.IsNullOrEmpty(locationId)) return string.Empty;

            if (!string.IsNullOrEmpty(weather))
            {
                var atmWeather = AtmosphereTexts.GetTextForLocationAndWeather(locationId, weather);
                if (atmWeather != null && !string.IsNullOrEmpty(atmWeather.text))
                    return atmWeather.text;
            }

            var atm = AtmosphereTexts.GetTextForLocation(locationId);
            if (atm != null && !string.IsNullOrEmpty(atm.text))
                return atm.text;

            var env = EnvironmentalTexts.GetTextForLocation(locationId);
            if (env != null && !string.IsNullOrEmpty(env.text))
                return env.text;

            return string.Empty;
        }

        // ── Production Runtime Actions ───────────────────────────────
        public void TickHours(float hours)
        {
            Weather.Tick(hours);
        }

        // ── Demo actions ─────────────────────────────────────────────

        public string ForceDemo(WeatherKind kind)
        {
            Weather.ForceWeather(kind);
            return $"Weather forced to {kind}.";
        }

        public string StatusLine()
        {
            return $"Weather: {Weather.Current} · visibility {Weather.VisibilityFactor:P0} · " +
                   $"outdoor rad {Weather.OutdoorRadModifier:0} · " +
                   $"temp penalty {WeatherSystem.TemperaturePenaltyForWeather(Weather.Current):0}°C";
        }

        // ── Save / Load ──────────────────────────────────────────────

        public WorldWeatherState CaptureSave() => Weather.CaptureState();
        public void RestoreSave(WorldWeatherState state) => Weather.RestoreState(state);

        // ── Sky Layer Armor (Exp 11) ────────────────────────────────

        public string SetSkyArmorDemo(int gridX, string material, float thickness)
        {
            var tier = material switch
            {
                "dirt" => CeilingMaterialTier.Dirt,
                "wood" => CeilingMaterialTier.Wood,
                "concrete" => CeilingMaterialTier.ReinforcedConcrete,
                "lead" => CeilingMaterialTier.LeadSheeting,
                "tungsten" => CeilingMaterialTier.TungstenComposite,
                _ => CeilingMaterialTier.Dirt
            };
            SkyArmor.SetCellArmor(gridX, tier, thickness);
            return $"Sky armor set at grid {gridX}: {tier} ({thickness}m). Attenuation: {SkyArmor.GetAttenuationFactor(gridX):F3}.";
        }

        public string ImpactDemo(int gridX, float energyMJ)
        {
            bool breached = SkyArmor.EvaluateKineticImpact(gridX, energyMJ, out float damage);
            return breached ? $"BREACH at grid {gridX}! {damage:F1} MJ through." : $"Impact absorbed at grid {gridX}.";
        }

        public string SkyArmorStatusLine()
        {
            var save = SkyArmor.CaptureState();
            if (save.cells.Count == 0) return "Sky armor: no cells plated";
            return $"Sky armor: {save.cells.Count} cells · avg attenuation {AvgAttenuation():F3}";
        }

        private float AvgAttenuation()
        {
            var save = SkyArmor.CaptureState();
            if (save.cells.Count == 0) return 1f;
            float sum = 0f;
            foreach (var c in save.cells) sum += SkyArmor.GetAttenuationFactor(c.gridX);
            return sum / save.cells.Count;
        }

        public SkyArmorSaveState CaptureSkyArmorSave() => SkyArmor.CaptureState();
        public void RestoreSkyArmorSave(SkyArmorSaveState state) => SkyArmor.RestoreState(state);

        // ── Weather Intelligence (station + orbital telemetry) ────────────

        public WeatherIntelligenceSaveState CaptureWeatherIntelligenceSave()
            => WeatherIntelligence.CaptureState();

        public string InstallWeatherStationDemo(int day)
        {
            var r = WeatherIntelligence.Station.Install(day);
            return r.Status == ActionResult.StatusKind.Success
                ? $"Weather station installed on day {day}."
                : "Station already installed.";
        }

        public string CalibrateWeatherStationDemo(int day)
        {
            var r = WeatherIntelligence.Station.Calibrate(day);
            return r.Status == ActionResult.StatusKind.Success
                ? $"Station calibrated (accuracy {WeatherIntelligence.Station.State.accuracy:P0})."
                : "Cannot calibrate — station not installed or already calibrated.";
        }

        public string ActivateOrbitalTelemetryDemo(int day)
        {
            WeatherIntelligence.Orbital.ActivateTelemetry(day);
            return $"Orbital Harrow telemetry activated on day {day}.";
        }

        public string ScheduleOrbitalImpactDemo(int day, int gridX, float energyMj)
        {
            WeatherIntelligence.Orbital.ScheduleImpact(day, gridX, energyMj);
            return $"Orbital impact scheduled: day {day}, grid {gridX}, {energyMj:F1} MJ. Warning lead: {WeatherIntelligence.Orbital.State.warningLeadDays}d.";
        }

        public string WeatherIntelligenceStatusLine()
        {
            var rm = WeatherIntelligence.BuildReadModel();
            return rm.advisory;
        }

        /// <summary>
        /// Hazard weather kinds that warrant an audio alert on transition.
        /// Matches the Core-rollable hazard set: FalloutStorm, BlackRain, Blizzard.
        /// </summary>
        internal static bool IsHazardWeather(WeatherKind kind)
        {
            return kind == WeatherKind.FalloutStorm
                || kind == WeatherKind.BlackRain
                || kind == WeatherKind.Blizzard;
        }
    }
}
