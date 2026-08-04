using UnityEngine;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Shelter.Modules;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Weather-driven bunker water economy: advances the roof catchment (fills
    /// WaterStorage from Rain/FalloutStorm while open) and the water purifier's
    /// 3-tier conversion queue (irradiated -&gt; dirty -&gt; clean). Mirrors
    /// RadioTunerSystem's shape (plain Tick(gameHours, weather, day) call from
    /// GameBootstrap) rather than living in ShelterModuleInstance.Tick, since it
    /// needs current weather/day/WaterStorage that generic module ticking
    /// doesn't have access to.
    /// </summary>
    public class WaterEconomySystem
    {
        /// <summary>Day on which Rain collection turns bacterial (dirty) instead of clean.</summary>
        public const int ContaminationOnsetDay = 30;

        public const string CatchmentModuleId = "catchment_surface";
        public const string PurifierModuleId = "water_purifier";

        /// <summary>Default catchment collection rate used when no module definition is attached.</summary>
        private const float DefaultCollectionRatePerHour = 5f;
        private const float DefaultConversionHoursPerUnit = 2f;
        private const float DefaultFilterDegradationPerUnitConverted = 5f;

        public void Tick(float gameHours, WeatherKind weather, int currentDay, Shelter.Shelter shelter, WaterStorage storage)
        {
            if (gameHours <= 0f || shelter == null || storage == null) return;

            CollectCatchment(gameHours, weather, currentDay, shelter, storage);
            RunPurifier(gameHours, shelter, storage);
        }

        private static void CollectCatchment(float gameHours, WeatherKind weather, int currentDay, Shelter.Shelter shelter, WaterStorage storage)
        {
            var catchment = shelter.GetModule(CatchmentModuleId);
            if (catchment == null || !catchment.IsOperational) return; // trap closed or not installed

            // Black Rain (Prompt #11): open catchment is instantly ruined — any
            // clean/dirty cistern water becomes irradiated, then oily rain fills
            // only the irradiated tier.
            if (weather == WeatherKind.BlackRain)
            {
                storage.RuinCleanAndDirtyToIrradiated();
                var blackDef = catchment.Definition as CatchmentSurfaceModuleSO;
                float blackRate = blackDef != null ? blackDef.CollectionRatePerHour : DefaultCollectionRatePerHour;
                float blackCollected = blackRate * gameHours;
                if (blackCollected > 0f) storage.AddIrradiated(blackCollected);
                return;
            }

            if (weather != WeatherKind.Rain && weather != WeatherKind.FalloutStorm) return;

            var def = catchment.Definition as CatchmentSurfaceModuleSO;
            float rate = def != null ? def.CollectionRatePerHour : DefaultCollectionRatePerHour;
            float collected = rate * gameHours;
            if (collected <= 0f) return;

            if (weather == WeatherKind.FalloutStorm)
            {
                storage.AddIrradiated(collected);
            }
            else if (currentDay < ContaminationOnsetDay)
            {
                storage.AddClean(collected);
            }
            else
            {
                storage.AddDirty(collected);
            }
        }

        private static void RunPurifier(float gameHours, Shelter.Shelter shelter, WaterStorage storage)
        {
            var purifier = shelter.GetModule(PurifierModuleId);
            if (purifier == null || !purifier.IsOperational || purifier.FilterHealth <= 0f) return;

            var def = purifier.Definition as WaterPurifierModuleSO;
            float hoursPerUnit = def != null && def.ConversionHoursPerUnit > 0f
                ? def.ConversionHoursPerUnit
                : DefaultConversionHoursPerUnit;
            float degradePerUnit = def != null ? def.FilterDegradationPerUnitConverted : DefaultFilterDegradationPerUnitConverted;

            purifier.WaterConversionProgress += gameHours;

            int safety = 0;
            while (purifier.WaterConversionProgress >= hoursPerUnit && purifier.FilterHealth > 0f && safety < 10000)
            {
                if (storage.IrradiatedWater > 0f)
                {
                    storage.ConsumeIrradiated(1f);
                    storage.AddDirty(1f);
                }
                else if (storage.DirtyWater > 0f)
                {
                    storage.ConsumeDirty(1f);
                    storage.AddClean(1f);
                }
                else
                {
                    break; // nothing left to convert this tick
                }

                purifier.WaterConversionProgress -= hoursPerUnit;
                purifier.FilterHealth = Mathf.Max(0f, purifier.FilterHealth - degradePerUnit);
                safety++;
            }
        }
    }
}
