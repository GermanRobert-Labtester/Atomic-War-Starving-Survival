// SPDX-License-Identifier: MIT
// ASHFALL Core: apply FluidLogistics sink deliveries once to named consumers.

using System;
using Ashfall.Core.Disease;

namespace Ashfall.Core.Shelter
{
    /// <summary>
    /// Plan 168: one-shot delivery applicator. Consumes ephemeral last-delivery
    /// maps from <see cref="FluidLogisticsSystem"/> into Greenhouse moisture and
    /// Disease water protocols. Does not touch inventory bottle packaging.
    /// </summary>
    public static class FluidDeliveryApplicator
    {
        public const string DrinkingExposureSourceId = "fluid_drinking";
        public const float DefaultDailyTransferCapLiters = 50f;

        public sealed class ApplicationReport
        {
            public float greenhouseLiters;
            public bool greenhouseTainted;
            public int greenhousePlotsWatered;
            public float drinkingLiters;
            public bool drinkingPurified;
            public bool drinkingExposureAttempted;
            public string drinkingSurvivorId = string.Empty;
            public string result = string.Empty;
        }

        /// <summary>
        /// Maps WaterTreatment bulk types onto network FluidQuality axes.
        /// Clean → near-zero (Potable); non-clean → elevated pathogen/radiological.
        /// </summary>
        public static FluidQuality QualityFromWaterType(WaterType waterType)
        {
            if (waterType == WaterType.Clean)
                return new FluidQuality();
            return new FluidQuality
            {
                pathogen01 = waterType == WaterType.Irradiated ? 0.35f : 0.45f,
                radiological01 = waterType == WaterType.Irradiated ? 0.55f : 0.05f,
                chemical01 = waterType == WaterType.Brackish ? 0.2f : 0.05f,
                salinity01 = waterType == WaterType.Brackish ? 0.35f : 0f,
                sediment01 = waterType == WaterType.Raw ? 0.25f : 0.05f
            };
        }

        /// <summary>
        /// Bounded clean-water handoff WT → Fluid reservoir. Liters leave WT once.
        /// </summary>
        public static ActionResult TransferBoundedCleanWater(
            WaterTreatmentSystem treatment,
            FluidLogisticsSystem network,
            float dailyCapLiters = DefaultDailyTransferCapLiters,
            string reservoirNodeId = FluidLogisticsSystem.DefaultReservoirId)
        {
            if (treatment == null || network == null)
                return ActionResult.Blocked("invalid_transfer", "fluid.invalid_transfer");
            float available = treatment.GetWater(WaterType.Clean);
            float free = network.GetFreeCapacity(reservoirNodeId);
            float amount = Math.Min(available, Math.Min(free, Math.Max(0f, dailyCapLiters)));
            if (amount <= 0.0001f)
                return ActionResult.Blocked("nothing_to_transfer", "fluid.nothing_to_transfer");
            return FluidWaterTreatmentBridge.TransferTreatedWater(
                treatment,
                network,
                WaterType.Clean,
                reservoirNodeId,
                amount,
                QualityFromWaterType(WaterType.Clean));
        }

        public static ApplicationReport Apply(
            FluidLogisticsSystem fluid,
            GreenhouseSystem? greenhouse,
            DiseaseSystem? disease,
            Func<string?>? pickLivingSurvivorId,
            int day)
        {
            var report = new ApplicationReport { result = "applied" };
            if (fluid == null)
            {
                report.result = "missing_fluid";
                return report;
            }

            float greenhouseLiters = fluid.GetDeliveredVolume(FluidLogisticsSystem.DefaultGreenhouseSinkId);
            if (greenhouseLiters > 0.0001f && greenhouse != null)
            {
                var quality = fluid.GetDeliveredQuality(FluidLogisticsSystem.DefaultGreenhouseSinkId);
                bool tainted = quality.Band != FluidQualityBand.Potable;
                if (greenhouse.PlotCount <= 0)
                    greenhouse.EnsurePlots(1);
                int plots = Math.Max(1, greenhouse.PlotCount);
                float perPlot = greenhouseLiters / plots;
                for (int i = 0; i < plots; i++)
                    greenhouse.Water(i, perPlot, tainted);
                report.greenhouseLiters = greenhouseLiters;
                report.greenhouseTainted = tainted;
                report.greenhousePlotsWatered = plots;
            }

            float drinkingLiters = fluid.GetDeliveredVolume(FluidLogisticsSystem.DefaultDrinkingSinkId);
            if (drinkingLiters > 0.0001f && disease != null)
            {
                var quality = fluid.GetDeliveredQuality(FluidLogisticsSystem.DefaultDrinkingSinkId);
                report.drinkingLiters = drinkingLiters;
                if (quality.Band == FluidQualityBand.Potable)
                {
                    disease.PurifyWater(day);
                    report.drinkingPurified = true;
                }
                else if (quality.Band == FluidQualityBand.Unsafe || quality.Band == FluidQualityBand.Toxic)
                {
                    string? survivorId = pickLivingSurvivorId?.Invoke();
                    if (!string.IsNullOrWhiteSpace(survivorId))
                    {
                        disease.TryExpose(new DiseaseExposureContext
                        {
                            SurvivorId = survivorId,
                            DiseaseId = DiseaseIds.Cholera,
                            SourceId = DrinkingExposureSourceId,
                            Day = day,
                            ProbabilityModifier = quality.Band == FluidQualityBand.Toxic ? 1.5f : 1f
                        });
                        report.drinkingExposureAttempted = true;
                        report.drinkingSurvivorId = survivorId;
                    }
                }
            }

            return report;
        }
    }
}
