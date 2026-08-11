// GameBootstrap.Weather.NewContent.cs — wire the 5 new weather systems
// from Prompts #319–#325 (Section X) and the 10 new recipes from
// Section XI. Kept in a separate partial so the existing
// GameBootstrap.Weather.cs dormant-ghost batch remains reviewable.
using System;
using UnityEngine;
using AtomicWar._Game.Crafting;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Events;
using AtomicWar._Game.Flashpoint;
using AtomicWar._Game.Utilities;

namespace AtomicWar._Game.Core
{
    public partial class GameBootstrap
    {
        private System.Random _newWeatherRng;

        /// <summary>
        /// Construct + save-wire the 5 new weather systems. All five are
        /// registered through SaveSystem like the dormant ghost batch
        /// (Weather_AcidSnow etc.); the host calls
        /// <see cref="SetActive"/> on whichever system is firing.
        /// </summary>
        private void BootNewWeatherSystems()
        {
            _newWeatherRng = CreateSaltedRng(_worldSeed, "new_weather_x");
            WeatherAshLightning = new Weather_AshLightning();
            WeatherFogOfParticulate = new Weather_FogOfParticulate();
            WeatherThermalInversion = new Weather_ThermalInversion();
            WeatherIceStorm = new Weather_IceStorm();
            WeatherSilence = new Weather_Silence();

            SaveSystem.SetWeatherAshLightning(WeatherAshLightning);
            SaveSystem.SetWeatherFogOfParticulate(WeatherFogOfParticulate);
            SaveSystem.SetWeatherThermalInversion(WeatherThermalInversion);
            SaveSystem.SetWeatherIceStorm(WeatherIceStorm);
            SaveSystem.SetWeatherSilence(WeatherSilence);

            WireNewWeatherSystems();
            WireNewWeatherEventBridge();
            GameLog.Log("[GameBootstrap] New weather: 5 (Section X) wired & save-registered.");
        }

        /// <summary>
        /// Prompts #319–#325 — bridge from <see cref="FlashpointWeatherEventTriggered"/>
        /// (raised by a Flashpoint step with actionId "weather_event_trigger" and
        /// a non-empty <c>weatherEventId</c>) to the right <c>Trigger()</c> call on
        /// the matching new Weather_* system. Tracked through
        /// <c>_subscriptions</c> so OnDestroy can dispose it without leaks.
        /// </summary>
        private void WireNewWeatherEventBridge()
        {
            Action<FlashpointWeatherEventTriggered> handler = OnFlashpointWeatherEventTriggered;
            EventBus.Subscribe(handler);
            _subscriptions.Track(() => EventBus.Unsubscribe(handler));
        }

        private void OnFlashpointWeatherEventTriggered(FlashpointWeatherEventTriggered evt)
        {
            if (string.IsNullOrEmpty(evt.WeatherEventId)) return;
            switch (evt.WeatherEventId)
            {
                case "weather_ash_lightning":
                    WeatherAshLightning?.Trigger();
                    GameLog.Log("[GameBootstrap] WEATHER: flashpoint triggered weather_ash_lightning");
                    break;
                case "weather_fog_of_particulate":
                    WeatherFogOfParticulate?.Trigger();
                    GameLog.Log("[GameBootstrap] WEATHER: flashpoint triggered weather_fog_of_particulate");
                    break;
                case "weather_thermal_inversion":
                    WeatherThermalInversion?.Trigger();
                    GameLog.Log("[GameBootstrap] WEATHER: flashpoint triggered weather_thermal_inversion");
                    break;
                case "weather_ice_storm":
                    WeatherIceStorm?.Trigger();
                    GameLog.Log("[GameBootstrap] WEATHER: flashpoint triggered weather_ice_storm");
                    break;
                case "weather_silence":
                    WeatherSilence?.Trigger();
                    GameLog.Log("[GameBootstrap] WEATHER: flashpoint triggered weather_silence");
                    break;
                default:
                    GameLog.Log($"[GameBootstrap] WEATHER: flashpoint weather_event_trigger has unknown id '{evt.WeatherEventId}'");
                    break;
            }
        }

        private void WireNewWeatherSystems()
        {
            if (WeatherAshLightning != null)
            {
                WeatherAshLightning.OnStaticDischarge += (state, drain) =>
                    GameLog.Log($"[GameBootstrap] WEATHER: ash lightning static discharge (fire risk {drain:F2})");
                WeatherAshLightning.OnFlickerOrange += state =>
                    GameLog.Log("[GameBootstrap] WEATHER: ash lightning — sky flickers orange");
            }

            if (WeatherFogOfParticulate != null)
            {
                WeatherFogOfParticulate.OnFilterLoadDoubled += (state, mult) =>
                    GameLog.Log($"[GameBootstrap] WEATHER: particulate fog filter load +{mult:F1}");
                WeatherFogOfParticulate.OnUnmaskedDoseApplied += (state, msv) =>
                    GameLog.Log($"[GameBootstrap] WEATHER: particulate fog unmasked dose +{msv:F1} mSv");
            }

            if (WeatherThermalInversion != null)
            {
                WeatherThermalInversion.OnSoundCarriedFar += state =>
                    GameLog.Log("[GameBootstrap] WEATHER: thermal inversion — sounds carry for miles");
                WeatherThermalInversion.OnSurfaceRadiationDoubled += (state, hours) =>
                    GameLog.Log($"[GameBootstrap] WEATHER: thermal inversion surface rad tick ({hours:F1}h)");
            }

            if (WeatherIceStorm != null)
            {
                WeatherIceStorm.OnHatchFrozen += state =>
                    GameLog.Log("[GameBootstrap] WEATHER: ice storm — hatch frozen shut");
                WeatherIceStorm.OnFuelBurnIncreased += (state, extra) =>
                    GameLog.Log($"[GameBootstrap] WEATHER: ice storm fuel burn +{extra:F2}");
            }

            if (WeatherSilence != null)
            {
                WeatherSilence.OnClearSkyObserved += state =>
                    GameLog.Log("[GameBootstrap] WEATHER: The Silence — clear sky observed");
                WeatherSilence.OnTemptationToSurface += (state, urgency) =>
                    GameLog.Log($"[GameBootstrap] WEATHER: The Silence temptation +{urgency:F1}");
                // Prompts #319–#325 — push a MoralChronicle entry the moment a
                // survivor ventures out. The rad is still there; the chronicle
                // remembers it. The bridge will paint a "Dead — acute radiation
                // sickness" line in the fate summary when the venture kills.
                WeatherSilence.OnSurfaceVentured += (state, svId) =>
                {
                    GameLog.Log($"[GameBootstrap] WEATHER: The Silence — '{svId}' went outside. The rad was still there.");
                    EventBus.Raise(new MoralChronicleEntryRequested(
                        day: CurrentDaySafe(),
                        description: $"The Silence: {svId} walked out into the clear sky. The radiation was still there.",
                        kind: MoralChronicleEntryKind.SurvivorLost,
                        survivorName: svId));
                };
            }
        }

        /// <summary>
        /// Tick the 5 new weather systems. They are self-gating (Tick is a
        /// no-op when isActive is false) so calling every hour is safe.
        /// </summary>
        private void TickNewWeatherSystemsHourly(float gameHours)
        {
            if (gameHours <= 0f) return;

            // These three read static flags or apply rate multipliers; they are
            // cheap and safe to call every hour.
            WeatherFogOfParticulate?.Tick(1f,
                isOutside: false, hasMask: true, rng: null);
            WeatherThermalInversion?.Tick(1f, 0f, 0f);
            WeatherIceStorm?.Tick(1f, 0f);

            // Ash lightning has a per-tick fire roll that needs an RNG.
            // The host passes a system RNG so saves remain deterministic.
            if (WeatherAshLightning != null && WeatherAshLightning.State.isActive)
            {
                WeatherAshLightning.Tick(1f,
                    isVentilationActive: true,
                    hasUnshieldedElectronics: false,
                    rng: _newWeatherRng);
            }

            // The Silence builds temptation every hour the sky is clear.
            WeatherSilence?.Tick(1f);
        }

        /// <summary>
        /// Materialise and merge the 10 new recipes (Section XI) into
        /// <c>_recipeCatalog</c>. Called once from
        /// <c>WireCraftingAndPerkBindings</c> after the catalog asset is
        /// loaded; safe to call again (re-materialise) on hot-reload.
        /// </summary>
        private void BootNewRecipes()
        {
            if (_recipeCatalog == null)
            {
                GameLog.LogWarning("[GameBootstrap] RecipeCatalogSO is null; skipping new recipes.");
                return;
            }
            var lookup = new System.Func<string, AtomicWar._Game.Inventory.ItemDefinition>(id =>
                _itemCatalog != null ? _itemCatalog.GetById(id) : null);
            var newRecipes = NewRecipesCatalog.MaterialiseAll(lookup);
            int added = 0;
            for (int i = 0; i < newRecipes.Count; i++)
            {
                var r = newRecipes[i];
                if (r == null || string.IsNullOrEmpty(r.id)) continue;
                if (ContainsRecipeId(r.id)) continue;
                _recipeCatalog.recipes.Add(r);
                added++;
            }
            GameLog.Log($"[GameBootstrap] New recipes (Section XI): merged {added} of {newRecipes.Count}.");
        }

        private bool ContainsRecipeId(string id)
        {
            if (_recipeCatalog?.recipes == null) return false;
            for (int i = 0; i < _recipeCatalog.recipes.Count; i++)
            {
                var r = _recipeCatalog.recipes[i];
                if (r != null && r.id == id) return true;
            }
            return false;
        }

        /// <summary>
        /// Prompts #319–#325 — small helper for the new weather systems to
        /// stamp a moral-chronicle entry with the current campaign day. Returns
        /// 0 when <c>TimeSystem</c> is null (e.g. in a partial-init test), so
        /// callers don't have to null-check.
        /// </summary>
        private int CurrentDaySafe() =>
            TimeSystem != null ? TimeSystem.CurrentDay : 0;
    }
}
