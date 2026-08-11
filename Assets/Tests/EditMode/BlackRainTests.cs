using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Core;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Shelter.Modules;
using AtomicWar._Game.Survivors;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Prompt #11 — Black Rain: rare oily hyper-radioactive weather.
    /// Catchment ruin, hazmat melt, Dread status, season restriction.
    /// </summary>
    [TestFixture]
    public class BlackRainTests
    {
        private const float Eps = 1e-4f;

        private CatchmentSurfaceModuleSO _catchmentSO;
        private WaterEconomySystem _water;

        [SetUp]
        public void SetUp()
        {
            _catchmentSO = ScriptableObject.CreateInstance<CatchmentSurfaceModuleSO>();
            _catchmentSO.ModuleId = "catchment_surface";
            _catchmentSO.DisplayName = "Roof Catchment";
            _catchmentSO.CollectionRatePerHour = 5f;
            _water = new WaterEconomySystem();
        }

        [TearDown]
        public void TearDown()
        {
            if (_catchmentSO != null) Object.DestroyImmediate(_catchmentSO);
        }

        [Test]
        public void ForceWeather_BlackRain_SetsCurrentAndOutdoorRad()
        {
            var weather = new WeatherSystem();
            weather.ForceWeather(WeatherKind.BlackRain);

            Assert.That(weather.Current, Is.EqualTo(WeatherKind.BlackRain));
            Assert.That(weather.OutdoorRadModifier, Is.EqualTo(WeatherSystem.BlackRainOutdoorRadModifier).Within(Eps));
            Assert.That(weather.OutdoorRadModifier, Is.GreaterThan(WeatherSystem.FalloutStormOutdoorRadModifier));
            Assert.That(weather.VisibilityFactor, Is.EqualTo(0f).Within(Eps));
            Assert.That(weather.HazmatDegradeMultiplier, Is.EqualTo(WeatherSystem.BlackRainHazmatMeltMultiplier).Within(Eps));
            Assert.That(weather.AirFilterDegradationMultiplier, Is.EqualTo(2.5f).Within(Eps));
            Assert.That(weather.IsScavengingBlocked(hasFullSuit: false), Is.True);
            Assert.That(weather.IsScavengingBlocked(hasFullSuit: true), Is.False);
        }

        [Test]
        public void BlackRain_WithOpenCatchment_RuinsCleanWater_AndAddsIrradiated()
        {
            var shelter = new Shelter();
            shelter.AddModule(new ShelterModuleInstance(_catchmentSO, level: 1) { IsEnabled = true });
            var storage = new WaterStorage();
            storage.AddClean(10f);
            storage.AddDirty(4f);

            _water.Tick(1f, WeatherKind.BlackRain, currentDay: 40, shelter, storage);

            Assert.That(storage.CleanWater, Is.EqualTo(0f).Within(Eps));
            Assert.That(storage.DirtyWater, Is.EqualTo(0f).Within(Eps));
            // 10 clean + 4 dirty ruined + 5 collected
            Assert.That(storage.IrradiatedWater, Is.EqualTo(19f).Within(Eps));
        }

        [Test]
        public void BlackRain_WithClosedCatchment_DoesNotRuinOrCollect()
        {
            var shelter = new Shelter();
            shelter.AddModule(new ShelterModuleInstance(_catchmentSO, level: 1) { IsEnabled = false });
            var storage = new WaterStorage();
            storage.AddClean(8f);

            _water.Tick(1f, WeatherKind.BlackRain, currentDay: 40, shelter, storage);

            Assert.That(storage.CleanWater, Is.EqualTo(8f).Within(Eps));
            Assert.That(storage.IrradiatedWater, Is.EqualTo(0f).Within(Eps));
        }

        [Test]
        public void Hazmat_DegradesFaster_UnderBlackRain_ThanFalloutStorm()
        {
            var stormWeather = new WeatherSystem();
            stormWeather.ForceWeather(WeatherKind.FalloutStorm);
            var blackWeather = new WeatherSystem();
            blackWeather.ForceWeather(WeatherKind.BlackRain);

            var stormHaz = new BlackRainHazardSystem(stormWeather);
            var blackHaz = new BlackRainHazardSystem(blackWeather);

            var gearStorm = new WornGear
            {
                RadProtection = 50f,
                MaxDurability = 100f,
                CurrentDurability = 100f,
                DegradeRate = 2f
            };
            var gearBlack = new WornGear
            {
                RadProtection = 50f,
                MaxDurability = 100f,
                CurrentDurability = 100f,
                DegradeRate = 2f
            };

            stormHaz.DegradeHazmat(gearStorm, 1f);
            blackHaz.DegradeHazmat(gearBlack, 1f);

            Assert.That(gearStorm.CurrentDurability, Is.EqualTo(98f).Within(Eps));
            Assert.That(gearBlack.CurrentDurability, Is.EqualTo(90f).Within(Eps));
            Assert.That(gearBlack.CurrentDurability, Is.LessThan(gearStorm.CurrentDurability));
        }

        [Test]
        public void RadiationSystemTick_AppliesBoundHazmatMultiplier_DuringBlackRain()
        {
            // Audit H-6b: DegradeHazmat/GetHazmatDegradeMultiplier existed but nothing
            // called them from RadiationSystem.Tick, so equipped gear degraded at the
            // same rate rain or shine. This proves the bound hook, not just the
            // standalone DegradeHazmat method already covered above.
            var blackWeather = new WeatherSystem();
            blackWeather.ForceWeather(WeatherKind.BlackRain);
            var blackHaz = new BlackRainHazardSystem(blackWeather);

            var profile = ScriptableObject.CreateInstance<NeedsProfile>();
            var needs = new NeedsSystem(profile, sv => true);

            var gear = new WornGear { RadProtection = 50f, MaxDurability = 100f, CurrentDurability = 100f, DegradeRate = 2f };
            var survivor = new Survivor { Id = "sv_hazmat", DisplayName = "Hazmat" };

            var rad = new RadiationSystem(needs, sv => new ExposureContext { WornGear = new List<WornGear> { gear } });
            rad.BindHazmatDegradeMultiplier(() => blackHaz.GetHazmatDegradeMultiplier());
            rad.Register(survivor);

            rad.Tick(1f);

            // DegradeRate 2/hr * BlackRainHazmatMeltMultiplier (5x) * 1 hour = 10 lost.
            Assert.That(gear.CurrentDurability, Is.EqualTo(90f).Within(Eps));
        }

        [Test]
        public void OutdoorSurvivor_GainsDread_DuringBlackRain_AndClearsWhenOver()
        {
            var weather = new WeatherSystem();
            weather.ForceWeather(WeatherKind.BlackRain);
            var haz = new BlackRainHazardSystem(weather);

            var outdoor = new Survivor { Id = "sv_out", DisplayName = "Out" };
            outdoor.Needs.Morale = 80f;
            var bunker = new Survivor { Id = "sv_in", DisplayName = "In" };
            bunker.Needs.Morale = 80f;
            var list = new List<Survivor> { outdoor, bunker };

            haz.TickDread(list, isOutdoor: s => s.Id == "sv_out", isHatchListener: _ => false, gameHours: 1f);

            Assert.That(outdoor.HasDread, Is.True);
            Assert.That(outdoor.HasStatus(SurvivorStatus.Dread), Is.True);
            Assert.That(outdoor.Needs.Morale, Is.EqualTo(80f - BlackRainHazardSystem.DreadMoraleDrainPerHour).Within(Eps));
            Assert.That(bunker.HasDread, Is.False);

            weather.ForceWeather(WeatherKind.Clear);
            haz.TickDread(list, isOutdoor: s => s.Id == "sv_out", isHatchListener: _ => false, gameHours: 0f);

            Assert.That(outdoor.HasDread, Is.False);
            Assert.That(outdoor.HasStatus(SurvivorStatus.Dread), Is.False);
        }

        [Test]
        public void HatchListener_GainsDread_DuringBlackRain()
        {
            var weather = new WeatherSystem();
            weather.ForceWeather(WeatherKind.BlackRain);
            var haz = new BlackRainHazardSystem(weather);
            var listener = new Survivor { Id = "sv_hatch", DisplayName = "Hatch" };
            listener.Needs.Morale = 50f;

            haz.TickDread(
                new List<Survivor> { listener },
                isOutdoor: _ => false,
                isHatchListener: _ => true,
                gameHours: 2f);

            Assert.That(listener.HasDread, Is.True);
            Assert.That(listener.Needs.Morale,
                Is.EqualTo(50f - BlackRainHazardSystem.DreadMoraleDrainPerHour * 2f).Within(Eps));
        }

        [Test]
        public void RestrictToNonHazardWeather_NeverRollsBlackRain()
        {
            var profile = ScriptableObject.CreateInstance<SeasonProfile>();
            profile.weatherCheckIntervalHours = 1f;
            profile.seasons = new[]
            {
                new SeasonWindow
                {
                    id = "hazard_heavy",
                    displayName = "Hazard Heavy",
                    startDay = 0,
                    clearWeight = 0f,
                    rainWeight = 0f,
                    overcastWeight = 0f,
                    ashfallWeight = 0f,
                    falloutStormWeight = 0f,
                    blizzardWeight = 0f,
                    blackRainWeight = 100f
                }
            };

            var weather = new WeatherSystem(profile, seed: 99);
            weather.RestrictToNonHazardWeather = true;

            for (int i = 0; i < 50; i++)
                weather.Tick(1f);

            Assert.That(weather.Current, Is.Not.EqualTo(WeatherKind.BlackRain));
            Assert.That(weather.Current, Is.EqualTo(WeatherKind.Clear));

            Object.DestroyImmediate(profile);
        }

        [Test]
        public void SeasonRoll_CanProduceBlackRain_WhenWeightPositive()
        {
            var profile = ScriptableObject.CreateInstance<SeasonProfile>();
            profile.weatherCheckIntervalHours = 1f;
            profile.seasons = new[]
            {
                new SeasonWindow
                {
                    id = "black_only",
                    displayName = "Black Only",
                    startDay = 0,
                    clearWeight = 0f,
                    rainWeight = 0f,
                    overcastWeight = 0f,
                    ashfallWeight = 0f,
                    falloutStormWeight = 0f,
                    blizzardWeight = 0f,
                    blackRainWeight = 100f
                }
            };

            var weather = new WeatherSystem(profile, seed: 7);
            weather.RestrictToNonHazardWeather = false;
            weather.Tick(1f);

            Assert.That(weather.Current, Is.EqualTo(WeatherKind.BlackRain));

            Object.DestroyImmediate(profile);
        }

        [Test]
        public void SaveLoad_RestoresBlackRainCurrent()
        {
            var profile = ScriptableObject.CreateInstance<SeasonProfile>();
            profile.weatherCheckIntervalHours = 6f;
            profile.seasons = new[]
            {
                new SeasonWindow
                {
                    id = "s",
                    startDay = 0,
                    clearWeight = 1f,
                    blackRainWeight = 0f
                }
            };

            var weather = new WeatherSystem(profile, seed: 42);
            weather.ForceWeather(WeatherKind.BlackRain);
            var state = weather.GetState();

            var restored = new WeatherSystem(profile, seed: 42);
            restored.RestoreState(state);

            Assert.That(restored.Current, Is.EqualTo(WeatherKind.BlackRain));
            Assert.That(restored.OutdoorRadModifier, Is.EqualTo(WeatherSystem.BlackRainOutdoorRadModifier).Within(Eps));

            Object.DestroyImmediate(profile);
        }

        [Test]
        public void ExtremeWeather_IncludesBlackRain_ForHatchEntrapment()
        {
            Assert.That(HatchEntrapmentSystem.IsExtremeWeather(WeatherKind.BlackRain), Is.True);
        }

        [Test]
        public void TravelMultiplier_BlackRain_IsAtLeastFalloutStorm()
        {
            float black = GeneratedMap.WeatherTravelMultiplier(WeatherKind.BlackRain);
            float storm = GeneratedMap.WeatherTravelMultiplier(WeatherKind.FalloutStorm);
            Assert.That(black, Is.GreaterThanOrEqualTo(storm));
        }

        [Test]
        public void RadioSignal_BlackRain_WorseThanFalloutStorm()
        {
            float black = RadioTunerSystem.GetWeatherSignalModifier(WeatherKind.BlackRain);
            float storm = RadioTunerSystem.GetWeatherSignalModifier(WeatherKind.FalloutStorm);
            Assert.That(black, Is.LessThan(storm));
            Assert.That(black, Is.EqualTo(0.1f).Within(Eps));
        }

        [Test]
        public void TemperaturePenalty_BlackRain_IsNegative()
        {
            Assert.That(WeatherSystem.TemperaturePenaltyForWeather(WeatherKind.BlackRain),
                Is.EqualTo(WeatherSystem.BlackRainTemperaturePenaltyC).Within(Eps));
        }
    }
}
