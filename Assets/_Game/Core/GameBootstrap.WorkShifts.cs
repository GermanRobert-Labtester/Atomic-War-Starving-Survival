using AtomicWar._Game.Shelter;

namespace AtomicWar._Game.Core
{
    public partial class GameBootstrap
    {
        /// <summary>Core adapts existing shelter telemetry to shift-station availability.</summary>
        private bool IsWorkShiftDutySupported(WorkShiftDuty duty)
        {
            switch (duty)
            {
                case WorkShiftDuty.AirFiltration:
                    return AirHeatManagementSystem != null
                        && AirHeatManagementSystem.GetSnapshot().FilterInstalled;
                case WorkShiftDuty.HeaterFuel:
                    return AirHeatManagementSystem != null
                        && AirHeatManagementSystem.GetSnapshot().HeaterInstalled;
                case WorkShiftDuty.WaterPurification:
                    return WaterEconomySystem != null
                        && WaterEconomySystem.GetSnapshot(Shelter, WaterStorage).FilterHealth > 0f;
                case WorkShiftDuty.RationPreparation:
                    return BunkerRationingSystem != null;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Pass read-only station telemetry into the duty recommender. Shelter,
        /// purifier, and ration systems retain ownership of their live state.
        /// </summary>
        private WorkShiftRecommendationContext GetWorkShiftRecommendationContext()
        {
            var climate = AirHeatManagementSystem != null ? AirHeatManagementSystem.GetSnapshot() : null;
            var water = WaterEconomySystem != null
                ? WaterEconomySystem.GetSnapshot(Shelter, WaterStorage)
                : null;
            var rations = BunkerRationingSystem != null
                ? BunkerRationingSystem.GetSnapshot(Survivors)
                : null;
            float foodDaysRemaining = rations != null && rations.FoodRequired > 0
                ? rations.FoodOnHand / (float)rations.FoodRequired
                : -1f;
            float waterDaysRemaining = rations != null && rations.WaterRequired > 0
                ? rations.WaterOnHand / (float)rations.WaterRequired
                : -1f;
            return new WorkShiftRecommendationContext
            {
                FilterOperational = climate != null && climate.FilterOperational,
                AirQuality = climate != null ? climate.AirQuality : 100f,
                FilterHealth = climate != null ? climate.FilterHealth : 100f,
                FilterBurnPerHour = climate != null ? climate.FilterDegradationPerHour : 0f,
                FilterRuntimeHours = climate != null && climate.FilterDegradationPerHour > 0f
                    ? climate.FilterRuntimeHours
                    : -1f,
                HeaterOperational = climate != null && climate.HeaterOperational,
                IndoorTemperatureCelsius = climate != null ? climate.IndoorTemperatureCelsius : 20f,
                HeaterFuel = climate != null ? climate.HeaterFuel : 0f,
                HeaterBurnPerHour = climate != null ? climate.HeaterFuelBurnPerHour : 0f,
                HeaterRuntimeHours = climate != null && climate.HeaterFuelBurnPerHour > 0f
                    ? climate.HeaterRuntimeHours
                    : -1f,
                PurifierOperational = water != null && water.PurifierOperational,
                IrradiatedWater = water != null ? water.IrradiatedWater : 0f,
                PurifierUnitsQueued = water != null ? water.UnitsQueued : 0,
                PurifierFilterBurnPerHour = water != null ? water.FilterBurnPerHour : 0f,
                PurifierRuntimeHours = water != null ? water.FilterRuntimeHours : -1f,
                RationOperational = BunkerRationingSystem != null,
                ProjectedFoodCoverage = rations != null ? rations.ProjectedFoodCoverage : 0f,
                ProjectedWaterCoverage = rations != null ? rations.ProjectedWaterCoverage : 0f,
                FoodDaysRemaining = foodDaysRemaining,
                WaterDaysRemaining = waterDaysRemaining,
                FoodUnitsPerDay = rations != null ? rations.FoodRequired : 0,
                WaterUnitsPerDay = rations != null ? rations.WaterRequired : 0
            };
        }

        /// <summary>
        /// Feed derived shift effects into the systems that own the actual
        /// resource state. No effect has a separate persisted value: restored
        /// staffing recreates the same live multipliers immediately.
        /// </summary>
        private void WireWorkShiftEffects()
        {
            if (SurvivorWorkShiftSystem == null) return;

            Shelter?.SetModuleConsumptionMultiplierProvider(
                SurvivorWorkShiftSystem.GetModuleResourceConsumptionMultiplier);
            AirHeatManagementSystem?.SetResourceConsumptionMultiplierProvider(load =>
            {
                var effects = SurvivorWorkShiftSystem.GetEffectsSnapshot();
                return load == AirHeatLoad.Heater
                    ? effects.HeaterFuelBurnMultiplier
                    : effects.FilterWearMultiplier;
            });
            WaterEconomySystem?.SetPurifierHoursPerUnitMultiplierProvider(() =>
                SurvivorWorkShiftSystem.GetEffectsSnapshot().PurifierHoursPerUnitMultiplier);
            BunkerRationingSystem?.SetRationRestoreMultiplierProvider(() =>
                SurvivorWorkShiftSystem.GetEffectsSnapshot().RationRestoreMultiplier);
        }
    }
}
