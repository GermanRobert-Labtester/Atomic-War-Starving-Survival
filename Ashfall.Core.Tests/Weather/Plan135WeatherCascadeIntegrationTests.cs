// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Weather;
using Xunit;

namespace Ashfall.Core.Tests.Weather
{
    public sealed class Plan135WeatherCascadeIntegrationTests
    {
        private static string GetDataDir()
        {
            string candidate = Path.Combine(AppContext.BaseDirectory, "../../../..", "Assets/StreamingAssets/Data");
            if (Directory.Exists(candidate)) return Path.GetFullPath(candidate);
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null)
            {
                string check = Path.Combine(dir.FullName, "Assets/StreamingAssets/Data");
                if (Directory.Exists(check)) return check;
                dir = dir.Parent!;
            }
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found");
        }

        [Fact]
        public void WeatherCascadeCatalog_LoadsAuthoredTemplates_Cleanly()
        {
            string dataDir = GetDataDir();
            var engine = WeatherGameplayCascadeEngine.LoadFromDirectory(dataDir, new FileSystemIO());

            Assert.NotNull(engine.Templates);
            Assert.True(engine.Templates.Count >= 15, $"Expected at least 15 templates, found {engine.Templates.Count}");

            // Verify specific key templates exist
            Assert.Contains(engine.Templates, t => t.id == "blizzard_shelter_freeze");
            Assert.Contains(engine.Templates, t => t.id == "fallout_storm_filtration_stress");
            Assert.Contains(engine.Templates, t => t.id == "black_rain_structural_corrosion");
            Assert.Contains(engine.Templates, t => t.id == "emp_storm_power_shedding");
            Assert.Contains(engine.Templates, t => t.id == "false_spring_morale_respite");
        }

        [Fact]
        public void SevereBlizzard_EvaluatesCrossSystemCascade_WithFortificationMitigation()
        {
            string dataDir = GetDataDir();
            var engine = WeatherGameplayCascadeEngine.LoadFromDirectory(dataDir, new FileSystemIO());

            // 1. Evaluate with 0 fortification
            var unfortifiedEvent = engine.EvaluateWeatherCascade(
                WeatherKind.Blizzard,
                severity: 85f,
                currentDay: 10,
                fortificationLevel: 0);

            Assert.NotNull(unfortifiedEvent);
            Assert.Equal(WeatherKind.Blizzard, unfortifiedEvent.weatherKind);
            Assert.Contains(unfortifiedEvent.effects, e => e.targetSystem == CascadeTargetSystem.Shelter && e.effectType == CascadeEffectType.ThermalLoad);
            Assert.Contains(unfortifiedEvent.effects, e => e.targetSystem == CascadeTargetSystem.Expedition && e.effectType == CascadeEffectType.Delay);
            Assert.Contains(unfortifiedEvent.effects, e => e.targetSystem == CascadeTargetSystem.Economy && e.effectType == CascadeEffectType.PriceChange);

            // 2. Evaluate BlackRain with Fortification >= 2 mitigating structural damage
            var fortifiedRain = engine.EvaluateWeatherCascade(
                WeatherKind.BlackRain,
                severity: 80f,
                currentDay: 12,
                fortificationLevel: 2);

            var dmgEffect = fortifiedRain.effects.FirstOrDefault(e => e.targetSystem == CascadeTargetSystem.Shelter && e.effectType == CascadeEffectType.Damage);
            Assert.NotNull(dmgEffect);
            Assert.Equal(0f, dmgEffect.magnitude); // Fortification 2 eliminates structural damage
        }

        [Fact]
        public void DomainSeams_FireCorrectly_WhenCascadeEvaluated()
        {
            var engine = new WeatherGameplayCascadeEngine();
            string dataDir = GetDataDir();
            string path = Path.Combine(dataDir, WeatherGameplayCascadeEngine.DefaultCatalogFileName);
            if (File.Exists(path))
            {
                engine.LoadCatalog(File.ReadAllText(path));
            }

            bool cascadeEvaluatedFired = false;
            float recordedDelay = 0f;
            float recordedMarketShock = 0f;
            float recordedMoraleDelta = 0f;

            engine.OnWeatherCascadeEvaluatedSeam = (ev, effs) => cascadeEvaluatedFired = true;
            engine.OnExpeditionDelayForecastedSeam = (region, delay) => recordedDelay = delay;
            engine.OnMarketShockTriggeredSeam = (category, shock, dur) => recordedMarketShock = shock;
            engine.OnMentalHealthStressSurgedSeam = (desc, delta) => recordedMoraleDelta = delta;

            var ev = engine.EvaluateWeatherCascade(WeatherKind.Blizzard, 90f, currentDay: 5);

            Assert.True(cascadeEvaluatedFired);
            Assert.True(recordedDelay > 0f);
            Assert.True(recordedMarketShock > 1.0f);
        }

        [Fact]
        public void DailyProgression_TickDay_TransitionsExpiredEventsToHistory()
        {
            var engine = new WeatherGameplayCascadeEngine();
            engine.EvaluateWeatherCascade(WeatherKind.EMPStorm, 70f, currentDay: 1); // 1-day duration

            Assert.Single(engine.State.activeEvents);
            Assert.Empty(engine.State.eventHistory);

            // Advance by 1 day
            engine.TickDay(2);

            // Expired event moved to history
            Assert.Empty(engine.State.activeEvents);
            Assert.Single(engine.State.eventHistory);
            Assert.Equal(WeatherKind.EMPStorm, engine.State.eventHistory[0].weatherKind);
        }

        [Fact]
        public void WeatherCascadeSystem_CaptureRestore_PreservesFullState()
        {
            var system = new WeatherCascadeSystem();
            system.FortificationLevel = 1;
            system.TriggerCascade(WeatherKind.FalloutStorm, 75f, day: 3);

            var state = system.CaptureState();
            Assert.Equal(1, state.schema_version);
            Assert.Single(state.activeEvents);
            Assert.True(state.activeEffects.Count > 0);

            var restoredSystem = new WeatherCascadeSystem();
            restoredSystem.RestoreState(state);

            Assert.Single(restoredSystem.State.activeEvents);
            Assert.Equal(WeatherKind.FalloutStorm, restoredSystem.State.activeEvents[0].weatherKind);
            Assert.Equal(state.activeEffects.Count, restoredSystem.State.activeEffects.Count);
        }
    }
}
