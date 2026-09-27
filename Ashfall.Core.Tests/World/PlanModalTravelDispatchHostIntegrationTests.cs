// SPDX-License-Identifier: MIT
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests.World
{
    /// <summary>
    /// Host-integration tests for the multi-modal travel dispatch read model.
    /// The engine is a pure projection over live map routes: it mutates nothing
    /// and persists nothing.
    /// </summary>
    public sealed class PlanModalTravelDispatchHostIntegrationTests
    {
        private static MapRoute Route(string from, string to, float km, float hazard, params string[] tags)
            => new MapRoute
            {
                From = from,
                To = to,
                DistanceKm = km,
                WeatherHazard = hazard,
                Tags = new List<string>(tags)
            };

        [Fact]
        public void Foot_CrossesAPlainLandRouteWithNoFuel()
        {
            var result = ModalTravelDispatchEngine.EvaluateDispatch(
                Route("a", "b", 24f, 0.2f), TravelModality.FootExcursion);

            Assert.True(result.CanDispatch);
            Assert.Equal(0, result.FuelRequiredUnits);
        }

        [Fact]
        public void FloodedRoutes_RefuseFootAndGroundButNotTheAmphibiousRig()
        {
            var flooded = Route("a", "b", 12f, 0.1f, "flooded");
            const int Fuel = 50;

            Assert.False(ModalTravelDispatchEngine.EvaluateDispatch(flooded, TravelModality.FootExcursion).CanDispatch);
            Assert.False(ModalTravelDispatchEngine.EvaluateDispatch(
                flooded, TravelModality.GroundConvoy, fuelAvailableUnits: Fuel).CanDispatch);
            Assert.True(ModalTravelDispatchEngine.EvaluateDispatch(
                flooded, TravelModality.AmphibiousRig, fuelAvailableUnits: Fuel).CanDispatch);
        }

        [Fact]
        public void MountainRoutes_RefuseTheAmphibiousRig()
        {
            var mountain = Route("a", "b", 30f, 0.3f, "mountain");

            Assert.False(ModalTravelDispatchEngine.EvaluateDispatch(
                mountain, TravelModality.AmphibiousRig, fuelAvailableUnits: 50).CanDispatch);
        }

        [Fact]
        public void AGroundedFlightWindow_RefusesAerialDispatch()
        {
            var plain = Route("a", "b", 24f, 0.2f);

            var grounded = ModalTravelDispatchEngine.EvaluateDispatch(
                plain, TravelModality.AerialReconFlight, vehicleConditionPermille: 1000, fuelAvailableUnits: 50, weatherWindowPermille: 200);

            Assert.False(grounded.CanDispatch);
        }

        [Fact]
        public void InsufficientFuel_RefusesAGroundConvoy()
        {
            var plain = Route("a", "b", 24f, 0.2f);

            var dry = ModalTravelDispatchEngine.EvaluateDispatch(
                plain, TravelModality.GroundConvoy, vehicleConditionPermille: 1000, fuelAvailableUnits: 0, weatherWindowPermille: 1000);

            Assert.False(dry.CanDispatch);
            Assert.Contains("fuel", dry.RefusalReason, System.StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void CriticalVehicleCondition_RefusesEveryVehicleModality()
        {
            var plain = Route("a", "b", 24f, 0.2f);
            bool allRefused = true;
            foreach (TravelModality modality in new[]
                     {
                         TravelModality.GroundConvoy,
                         TravelModality.AmphibiousRig,
                         TravelModality.AerialReconFlight
                     })
            {
                var result = ModalTravelDispatchEngine.EvaluateDispatch(
                    plain, modality, vehicleConditionPermille: 100, fuelAvailableUnits: 50, weatherWindowPermille: 1000);
                if (result.CanDispatch) allRefused = false;
            }

            Assert.True(allRefused);
            Assert.True(ModalTravelDispatchEngine.EvaluateDispatch(plain, TravelModality.FootExcursion).CanDispatch);
        }

        [Fact]
        public void ANullRoute_IsARefusalNotACrash()
        {
            var result = ModalTravelDispatchEngine.EvaluateDispatch(null, TravelModality.FootExcursion);

            Assert.False(result.CanDispatch);
            Assert.False(string.IsNullOrEmpty(result.RefusalReason));
        }

        [Fact]
        public void IdenticalInputs_ProduceIdenticalDeterministicOutputs()
        {
            var mountain = Route("a", "b", 30f, 0.3f, "mountain");

            const int Fuel2 = 50;
            var a = ModalTravelDispatchEngine.EvaluateDispatch(mountain, TravelModality.GroundConvoy, fuelAvailableUnits: Fuel2);
            var b = ModalTravelDispatchEngine.EvaluateDispatch(mountain, TravelModality.GroundConvoy, fuelAvailableUnits: Fuel2);

            Assert.Equal(a.EstimatedDurationHours, b.EstimatedDurationHours);
            Assert.Equal(a.FuelRequiredUnits, b.FuelRequiredUnits);
            Assert.Equal(a.TerrainAttritionRiskPermille, b.TerrainAttritionRiskPermille);
            Assert.Equal(a.WeatherHazardRiskPermille, b.WeatherHazardRiskPermille);
        }

        [Fact]
        public void Evaluation_NeverWritesBackToTheRoute()
        {
            var route = Route("a", "b", 24f, 0.2f);
            float before = route.DistanceKm;
            int tagsBefore = route.Tags.Count;

            foreach (TravelModality modality in System.Enum.GetValues(typeof(TravelModality)))
                ModalTravelDispatchEngine.EvaluateDispatch(route, modality);

            Assert.Equal(before, route.DistanceKm);
            Assert.Equal(tagsBefore, route.Tags.Count);
        }
    }
}
