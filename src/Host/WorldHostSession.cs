using System;
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
    {
        public const int DemoSeed = 1234;

        public WeatherSystem Weather { get; }
        public SkyLayerArmorSystem SkyArmor { get; }
        public SeasonProfileDef Profile { get; private set; }

        public string LastEvent { get; private set; } = string.Empty;

        public event Action StateChanged;

        public WorldHostSession(WeatherSystem weather = null, SkyLayerArmorSystem skyArmor = null)
        {
            Weather = weather ?? new WeatherSystem();
            SkyArmor = skyArmor ?? new SkyLayerArmorSystem();
            Weather.OnWeatherChanged += kind =>
            {
                LastEvent = $"Weather: {kind}";
                if (IsHazardWeather(kind))
                    AtomicWar.GodotApp.Audio.AudioManager.Instance?.PlayWeatherAlert();
                StateChanged?.Invoke();
            };
            Weather.OnStateChanged += _ => StateChanged?.Invoke();
        }

        public static WorldHostSession Create(string dataDir)
        {
            var session = new WorldHostSession();
            var profile = !string.IsNullOrEmpty(dataDir)
                ? WeatherProfileLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer())
                : null;
            if (profile != null)
            {
                session.Profile = profile;
                session.Weather.BindProfile(profile, DemoSeed);
            }
            var save = WorldSaveStore.TryLoad();
            if (save != null)
            {
                session.Weather.RestoreState(save);
                session.LastEvent = "World state restored from save.";
            }
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
