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
        public SeasonProfileDef Profile { get; private set; }

        public string LastEvent { get; private set; } = string.Empty;
        public WorldHostSession(
            WeatherSystem weather = null!,
            SkyLayerArmorSystem skyArmor = null!,
            LocationEvolutionSystem locationEvolution = null!,
            WildlifeMigrationSystem wildlife = null!,
            LandmarkDegradationSystem landmarks = null!,
            WastelandMapSystem wastelandMap = null!)
        {
            Weather = weather ?? new WeatherSystem();
            SkyArmor = skyArmor ?? new SkyLayerArmorSystem();
            WeatherIntelligence = new WeatherIntelligenceCoordinator(Weather, SkyArmor, new SeededRng(DemoSeed));
            LocationEvolution = locationEvolution ?? new LocationEvolutionSystem();
            Wildlife = wildlife ?? new WildlifeMigrationSystem();
            Landmarks = landmarks ?? new LandmarkDegradationSystem();
            WastelandMap = wastelandMap ?? WastelandMapCatalogLoader.CreateSystem(string.Empty);
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
            var profile = !string.IsNullOrEmpty(dataDir)
                ? WeatherProfileLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer())
                : null;
            if (profile != null)
            {
                session.Profile = profile;
                session.Weather.BindProfile(profile, DemoSeed);
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
            var mapSave = WastelandMapSaveStore.TryLoad();
            if (mapSave != null)
                session.WastelandMap.RestoreState(mapSave);
            return session;
        }

        // ── Demo actions ─────────────────────────────────────────────

        public string TickDemo(float hours)
        {
            Weather.Tick(hours);
            return $"Tick {hours}h: {Weather.Current} (rolls {Weather.State.rollCount}).";
        }

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
