// SPDX-License-Identifier: MIT
using System;
using Ashfall.Core;
using Ashfall.Core.Disease;
using Ashfall.Core.Shelter;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class Plan168WaterDeliveryTests
    {
        [Fact]
        public void EnsureDefaultShelterTopologyIsIdempotent()
        {
            var fluid = new FluidLogisticsSystem();
            Assert.True(fluid.EnsureDefaultShelterTopology());
            Assert.True(fluid.EnsureDefaultShelterTopology());

            Assert.Equal(3, fluid.State.nodes.Count);
            Assert.Equal(2, fluid.State.edges.Count);
            Assert.NotNull(fluid.State.nodes.Find(n => n.nodeId == FluidLogisticsSystem.DefaultReservoirId));
            Assert.NotNull(fluid.State.nodes.Find(n => n.nodeId == FluidLogisticsSystem.DefaultGreenhouseSinkId));
            Assert.NotNull(fluid.State.nodes.Find(n => n.nodeId == FluidLogisticsSystem.DefaultDrinkingSinkId));
        }

        [Fact]
        public void CleanWaterTransfersOnceIntoReservoirWithoutDoubleLedger()
        {
            var treatment = new WaterTreatmentSystem();
            treatment.AddWater(WaterType.Clean, 40f);
            var fluid = new FluidLogisticsSystem();
            Assert.True(fluid.EnsureDefaultShelterTopology());

            var first = FluidDeliveryApplicator.TransferBoundedCleanWater(treatment, fluid, dailyCapLiters: 25f);
            Assert.True(first.IsSuccess);
            Assert.Equal(15f, treatment.GetWater(WaterType.Clean), 3);
            Assert.Equal(25f, fluid.State.nodes.Find(n => n.nodeId == FluidLogisticsSystem.DefaultReservoirId)!.volume, 3);

            var second = FluidDeliveryApplicator.TransferBoundedCleanWater(treatment, fluid, dailyCapLiters: 25f);
            Assert.True(second.IsSuccess);
            Assert.Equal(0f, treatment.GetWater(WaterType.Clean), 3);
            Assert.Equal(40f, fluid.State.nodes.Find(n => n.nodeId == FluidLogisticsSystem.DefaultReservoirId)!.volume, 3);
        }

        [Fact]
        public void PotableDeliveryWatersGreenhouseAndPurifiesDrinking()
        {
            var treatment = new WaterTreatmentSystem();
            treatment.AddWater(WaterType.Clean, 60f);
            var fluid = new FluidLogisticsSystem();
            Assert.True(fluid.EnsureDefaultShelterTopology(greenhouseDemand: 20f, drinkingDemand: 30f));
            Assert.True(FluidDeliveryApplicator.TransferBoundedCleanWater(treatment, fluid, dailyCapLiters: 60f).IsSuccess);

            var greenhouse = new GreenhouseSystem();
            greenhouse.EnsurePlots(2);
            var disease = new DiseaseSystem();

            var report = fluid.Solve(3);
            Assert.True(report.deliveredVolume > 0f);

            var applied = FluidDeliveryApplicator.Apply(fluid, greenhouse, disease, () => "survivor_1", day: 3);
            Assert.True(applied.greenhouseLiters > 0f);
            Assert.False(applied.greenhouseTainted);
            Assert.Equal(2, applied.greenhousePlotsWatered);
            Assert.True(greenhouse.Plots[0].water > 0f);
            Assert.True(applied.drinkingLiters > 0f);
            Assert.True(applied.drinkingPurified);
            Assert.True(disease.IsVectorBlocked(DiseaseVectorNames.Water));
        }

        [Fact]
        public void UnsafeDrinkingDeliveryExposesLivingSurvivor()
        {
            var fluid = new FluidLogisticsSystem();
            Assert.True(fluid.EnsureDefaultShelterTopology(greenhouseDemand: 0f, drinkingDemand: 20f));
            Assert.True(fluid.InjectWater(
                FluidLogisticsSystem.DefaultReservoirId,
                20f,
                new FluidQuality { pathogen01 = 0.5f }));

            var disease = new DiseaseSystem();
            var report = fluid.Solve(7);
            Assert.Equal(20f, report.DeliveredTo(FluidLogisticsSystem.DefaultDrinkingSinkId), 3);

            var applied = FluidDeliveryApplicator.Apply(
                fluid,
                greenhouse: null,
                disease,
                () => "survivor_water",
                day: 7);
            Assert.True(applied.drinkingExposureAttempted);
            Assert.Equal("survivor_water", applied.drinkingSurvivorId);
            Assert.False(applied.drinkingPurified);
        }

        [Fact]
        public void ReloadPreservesVolumesThenSameDayCadenceReapplies()
        {
            var treatment = new WaterTreatmentSystem();
            treatment.AddWater(WaterType.Clean, 50f);
            var fluid = new FluidLogisticsSystem();
            Assert.True(fluid.EnsureDefaultShelterTopology());
            Assert.True(FluidDeliveryApplicator.TransferBoundedCleanWater(treatment, fluid, dailyCapLiters: 40f).IsSuccess);

            var wtSaved = treatment.CaptureState();
            var fluidSaved = fluid.CaptureState();

            var treatment2 = new WaterTreatmentSystem();
            treatment2.RestoreState(wtSaved);
            var fluid2 = new FluidLogisticsSystem();
            fluid2.RestoreState(fluidSaved);
            Assert.True(fluid2.EnsureDefaultShelterTopology());

            Assert.Equal(10f, treatment2.GetWater(WaterType.Clean), 3);
            Assert.Equal(40f, fluid2.State.nodes.Find(n => n.nodeId == FluidLogisticsSystem.DefaultReservoirId)!.volume, 3);

            var greenhouse = new GreenhouseSystem();
            greenhouse.EnsurePlots(1);
            var disease = new DiseaseSystem();
            fluid2.Solve(9);
            var applied = FluidDeliveryApplicator.Apply(fluid2, greenhouse, disease, () => "survivor_1", 9);
            Assert.True(applied.greenhouseLiters + applied.drinkingLiters > 0f);
        }
    }
}
