// GameBootstrap.Weather.cs — boot/wire Weather_* special weather event trackers.
// Distinct from core WeatherSystem (daily/seasonal ambient).
using UnityEngine;
using AtomicWar._Game.Utilities;
using AtomicWar._Game.Environment;

namespace AtomicWar._Game.Core
{
    public partial class GameBootstrap
    {
        /// <summary>
        /// Construct all Weather_* special-event trackers. Host hooks are offline-safe logs;
        /// weather hosts call Trigger/Tick when storms fire.
        /// </summary>
        private void BootWeather()
        {
            // DEMOTE-Weather-batch — WeatherAcidSnow demoted. Class kept dormant.
            // DEMOTE-Weather-batch — WeatherBioFog demoted. Class kept dormant.
            // DEMOTE-Weather-batch — WeatherBlackSnow demoted. Class kept dormant.
            WeatherBloodRain = new Weather_BloodRain();
            // DEMOTE-Weather-batch — WeatherDeadWind demoted. Class kept dormant.
            WeatherDeepFreeze = new Weather_DeepFreeze();
            // DEMOTE-Weather-batch — WeatherDustDevil demoted. Class kept dormant.
            WeatherEmpStorm = new Weather_EMPStorm();
            WeatherFalseSpring = new Weather_FalseSpring();
            // DEMOTE-Weather-batch — WeatherGlassStorm demoted. Class kept dormant.
            WeatherOzoneHole = new Weather_OzoneHole();
            // DEMOTE-Weather-batch — WeatherRadHail demoted. Class kept dormant.
            WeatherSilentSpring = new Weather_SilentSpring();
            WeatherSolarFlare = new Weather_SolarFlare();
            WeatherStaticCharge = new Weather_StaticCharge();

            WireWeather();
            GameLog.Log("[GameBootstrap] Weather events: hourly trackers live; 7 HANDLERS_ONLY demoted.");
        }

        private void WireWeather()
        {
            if (WeatherAcidSnow != null)
                WeatherAcidSnow.OnAcidCorrosionApplied += (_, drain) =>
                    GameLog.Log($"[GameBootstrap] WEATHER: acid snow filter drain {drain:F1}");

            if (WeatherBioFog != null)
            {
                WeatherBioFog.OnBioFogStarted += _ =>
                    GameLog.Log("[GameBootstrap] WEATHER: bio fog started");
                WeatherBioFog.OnRadiationAnxietyInflicted += (_, anxiety) =>
                    GameLog.Log($"[GameBootstrap] WEATHER: bio fog anxiety +{anxiety:F1}");
            }

            if (WeatherBlackSnow != null)
            {
                WeatherBlackSnow.OnSuitContaminated += id =>
                    GameLog.Log($"[GameBootstrap] WEATHER: black snow contaminated suit — '{id}'");
                WeatherBlackSnow.OnSuitRuined += id =>
                    GameLog.Log($"[GameBootstrap] WEATHER: black snow ruined suit — '{id}'");
                WeatherBlackSnow.OnSuitCleaned += id =>
                    GameLog.Log($"[GameBootstrap] WEATHER: black snow cleaned suit — '{id}'");
            }

            if (WeatherBloodRain != null)
            {
                WeatherBloodRain.OnBloodRainTriggered += _ =>
                    GameLog.Log("[GameBootstrap] WEATHER: blood rain triggered");
                WeatherBloodRain.OnBloodRainEnded += _ =>
                    GameLog.Log("[GameBootstrap] WEATHER: blood rain ended");
                WeatherBloodRain.OnMoraleDebuffApplied += (_, panic, despair) =>
                    GameLog.Log($"[GameBootstrap] WEATHER: blood rain morale panic={panic:F0} despair={despair:F0}");
            }

            if (WeatherDeadWind != null)
            {
                WeatherDeadWind.OnDeadWindStarted += _ =>
                    GameLog.Log("[GameBootstrap] WEATHER: dead wind started");
                WeatherDeadWind.OnDeadWindEnded += _ =>
                    GameLog.Log("[GameBootstrap] WEATHER: dead wind ended");
            }

            if (WeatherDeepFreeze != null)
            {
                WeatherDeepFreeze.OnDeepFreezeStarted += _ =>
                    GameLog.Log("[GameBootstrap] WEATHER: deep freeze started");
                WeatherDeepFreeze.OnCropsKilledByFreeze += _ =>
                    GameLog.Log("[GameBootstrap] WEATHER: deep freeze killed crops");
                WeatherDeepFreeze.OnFrostbiteRiskInflicted += (_, temp) =>
                    GameLog.Log($"[GameBootstrap] WEATHER: deep freeze frostbite risk at {temp:F0}°C");
            }

            if (WeatherDustDevil != null)
            {
                WeatherDustDevil.OnAirFilterRuined += node =>
                    GameLog.Log($"[GameBootstrap] WEATHER: dust devil ruined air filter at '{node}'");
                WeatherDustDevil.OnWindTurbineClogged += node =>
                    GameLog.Log($"[GameBootstrap] WEATHER: dust devil clogged turbine at '{node}'");
                WeatherDustDevil.OnDustDevilSpotted += (from, to) =>
                    GameLog.Log($"[GameBootstrap] WEATHER: dust devil {from} → {to}");
            }

            if (WeatherEmpStorm != null)
            {
                WeatherEmpStorm.OnEMPBurstTrippedPower += (_, hours) =>
                    GameLog.Log($"[GameBootstrap] WEATHER: EMP storm blackout {hours}h");
                WeatherEmpStorm.OnPowerRestored += _ =>
                    GameLog.Log("[GameBootstrap] WEATHER: EMP storm power restored");
            }

            if (WeatherFalseSpring != null)
            {
                WeatherFalseSpring.OnFalseSpringStarted += _ =>
                    GameLog.Log("[GameBootstrap] WEATHER: false spring started");
                WeatherFalseSpring.OnFalseSpringEnded += _ =>
                    GameLog.Log("[GameBootstrap] WEATHER: false spring ended");
            }

            if (WeatherGlassStorm != null)
            {
                WeatherGlassStorm.OnGlassStormTriggered += _ =>
                    GameLog.Log("[GameBootstrap] WEATHER: glass storm triggered");
                WeatherGlassStorm.OnGlassStormEnded += _ =>
                    GameLog.Log("[GameBootstrap] WEATHER: glass storm ended");
                WeatherGlassStorm.OnHazmatShredded += (_, dmg) =>
                    GameLog.Log($"[GameBootstrap] WEATHER: glass storm hazmat -{dmg:F0}");
                WeatherGlassStorm.OnHatchSandblasted += (_, dmg) =>
                    GameLog.Log($"[GameBootstrap] WEATHER: glass storm hatch -{dmg:F0}");
            }

            if (WeatherOzoneHole != null)
            {
                WeatherOzoneHole.OnOzoneHoleTriggered += _ =>
                    GameLog.Log("[GameBootstrap] WEATHER: ozone hole triggered");
                WeatherOzoneHole.OnOzoneHoleEnded += _ =>
                    GameLog.Log("[GameBootstrap] WEATHER: ozone hole ended");
                WeatherOzoneHole.OnDaylightExposure += (_, rad, burn) =>
                    GameLog.Log($"[GameBootstrap] WEATHER: ozone hole rad={rad:F0} burn={burn:F0}");
            }

            if (WeatherRadHail != null)
            {
                WeatherRadHail.OnCatchmentSurfacesDestroyed += _ =>
                    GameLog.Log("[GameBootstrap] WEATHER: rad hail destroyed catchments");
                WeatherRadHail.OnSurvivorStruckOutside += (_, dmg, rads) =>
                    GameLog.Log($"[GameBootstrap] WEATHER: rad hail strike dmg={dmg:F0} rads={rads:F0}");
            }

            if (WeatherSilentSpring != null)
            {
                WeatherSilentSpring.OnSilentSpringTriggered += _ =>
                    GameLog.Log("[GameBootstrap] WEATHER: silent spring triggered");
                WeatherSilentSpring.OnSilentSpringEnded += _ =>
                    GameLog.Log("[GameBootstrap] WEATHER: silent spring ended");
                WeatherSilentSpring.OnHordeImminent += _ =>
                    GameLog.Log("[GameBootstrap] WEATHER: silent spring horde imminent");
            }

            if (WeatherSolarFlare != null)
            {
                WeatherSolarFlare.OnSolarFlareTriggered += _ =>
                    GameLog.Log("[GameBootstrap] WEATHER: solar flare triggered");
                WeatherSolarFlare.OnSolarFlareEnded += _ =>
                    GameLog.Log("[GameBootstrap] WEATHER: solar flare ended");
                WeatherSolarFlare.OnTick += (_, remaining) =>
                    GameLog.Log($"[GameBootstrap] WEATHER: solar flare {remaining:F0}h remaining");
            }

            if (WeatherStaticCharge != null)
            {
                WeatherStaticCharge.OnStaticChargeTriggered += _ =>
                    GameLog.Log("[GameBootstrap] WEATHER: static charge triggered");
                WeatherStaticCharge.OnStaticChargeEnded += _ =>
                    GameLog.Log("[GameBootstrap] WEATHER: static charge ended");
                WeatherStaticCharge.OnShockDamageDealt += (_, module, dmg) =>
                    GameLog.Log($"[GameBootstrap] WEATHER: static charge shock '{module}' -{dmg:F0}");
            }
        }

        private float _weatherEventHourAccum;

        /// <summary>
        /// Advance the self-contained Weather_* event trackers on a whole-game-hour
        /// cadence.
        ///
        /// These were constructed, save-wired and event-wired but never ticked, which
        /// meant any weather event that got triggered would stay active forever — its
        /// hoursRemaining never counted down, so the "ended" event never fired and the
        /// debuff never lifted. Each Tick* below is self-gating (it returns immediately
        /// when its state is inactive), so driving them unconditionally is safe.
        ///
        /// Only trackers whose tick is time-based and whose inputs are available here
        /// are driven. AcidSnow, GlassStorm and DustDevil take ref/contextual arguments
        /// (filter durability, hazmat wear, current map node) that this host cannot yet
        /// supply, and remain in the C-1 baseline until those owners exist.
        /// </summary>
        private void TickWeatherEventsHourly(float gameHours)
        {
            if (gameHours <= 0f) return;

            _weatherEventHourAccum += gameHours;

            // Guard against a pathological delta (e.g. a long editor pause) turning this
            // into a multi-thousand iteration stall on one frame.
            const int MaxHoursPerCall = 48;
            int hours = 0;
            while (_weatherEventHourAccum >= 1f && hours < MaxHoursPerCall)
            {
                _weatherEventHourAccum -= 1f;
                hours++;

                WeatherSolarFlare?.TickHour();
                WeatherSilentSpring?.TickHour();
                WeatherStaticCharge?.TickHour();
                WeatherBloodRain?.TickHour();
                WeatherEmpStorm?.TickHourly(1);
                WeatherFalseSpring?.TickHourly(1f);

                if (WeatherDeepFreeze != null)
                {
                    float indoorTemp = TemperatureSystem != null && Shelter != null
                        ? TemperatureSystem.GetIndoorTemperature(Shelter)
                        : 15f;
                    WeatherDeepFreeze.TickHourly(1f, indoorTemp);
                }
            }

            if (hours >= MaxHoursPerCall)
                _weatherEventHourAccum = 0f;
        }

        /// <summary>
        /// Per game-day weather trackers. Ozone hole measures its duration in days
        /// rather than hours, so it is driven from the daily pass.
        /// </summary>
        private void TickWeatherEventsDaily(int currentDay)
        {
            WeatherOzoneHole?.TickDay();
        }
    }
}
