// SPDX-License-Identifier: MIT
using System.Collections.Generic;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests.World
{
    public class ModalTravelDispatchEngineTests
    {
        [Fact]
        public void EvaluateDispatch_FootExcursion_FloodedRoute_RefusesDispatch()
        {
            var route = new MapRoute
            {
                From = "loc_shelter",
                To = "loc_swamp",
                DistanceKm = 15,
                WeatherHazard = 0.2f,
                Tags = new List<string> { "flooded" }
            };

            var result = ModalTravelDispatchEngine.EvaluateDispatch(
                route: route,
                modality: TravelModality.FootExcursion
            );

            Assert.False(result.CanDispatch);
            Assert.Contains("flood hazard impassable on foot", result.RefusalReason);
        }

        [Fact]
        public void EvaluateDispatch_AmphibiousRig_TraversesFloodedRoute_Succeeds()
        {
            var route = new MapRoute
            {
                From = "loc_shelter",
                To = "loc_swamp",
                DistanceKm = 20,
                WeatherHazard = 0.2f,
                Tags = new List<string> { "flooded" }
            };

            var result = ModalTravelDispatchEngine.EvaluateDispatch(
                route: route,
                modality: TravelModality.AmphibiousRig,
                vehicleConditionPermille: 800,
                fuelAvailableUnits: 10
            );

            Assert.True(result.CanDispatch);
            Assert.Equal(TravelModality.AmphibiousRig, result.Modality);
            Assert.True(result.FuelRequiredUnits > 0);
            Assert.True(result.EstimatedDurationHours > 0);
            Assert.Contains("AmphibiousRig dispatch", result.Summary);
        }

        [Fact]
        public void EvaluateDispatch_GroundConvoy_InsufficientFuel_RefusesDispatch()
        {
            var route = new MapRoute
            {
                From = "loc_shelter",
                To = "loc_outpost",
                DistanceKm = 50,
                WeatherHazard = 0.1f
            };

            // Distance 50 km * 0.08 units/km = 4 fuel units required
            var result = ModalTravelDispatchEngine.EvaluateDispatch(
                route: route,
                modality: TravelModality.GroundConvoy,
                vehicleConditionPermille: 900,
                fuelAvailableUnits: 2 // Insufficient
            );

            Assert.False(result.CanDispatch);
            Assert.Contains("Insufficient fuel", result.RefusalReason);
        }

        [Fact]
        public void EvaluateDispatch_AerialRecon_SevereWeather_GroundedRefusal()
        {
            var route = new MapRoute
            {
                From = "loc_shelter",
                To = "loc_crater",
                DistanceKm = 60,
                WeatherHazard = 0.8f
            };

            var result = ModalTravelDispatchEngine.EvaluateDispatch(
                route: route,
                modality: TravelModality.AerialReconFlight,
                vehicleConditionPermille: 1000,
                fuelAvailableUnits: 50,
                weatherWindowPermille: 250 // Severe weather window (< 400)
            );

            Assert.False(result.CanDispatch);
            Assert.Contains("Flight grounded", result.RefusalReason);
        }

        [Fact]
        public void EvaluateDispatch_AerialRecon_OptimalWeather_FastestTransit()
        {
            var route = new MapRoute
            {
                From = "loc_shelter",
                To = "loc_far_peak",
                DistanceKm = 60,
                WeatherHazard = 0.05f,
                Tags = new List<string> { "mountain", "flooded" } // Aerial ignores ground terrain
            };

            var aerialResult = ModalTravelDispatchEngine.EvaluateDispatch(
                route: route,
                modality: TravelModality.AerialReconFlight,
                vehicleConditionPermille: 1000,
                fuelAvailableUnits: 50,
                weatherWindowPermille: 950
            );

            var footResult = ModalTravelDispatchEngine.EvaluateDispatch(
                route: new MapRoute { DistanceKm = 60 },
                modality: TravelModality.FootExcursion
            );

            Assert.True(aerialResult.CanDispatch);
            Assert.True(aerialResult.EstimatedDurationHours < footResult.EstimatedDurationHours);
            Assert.Contains("AerialReconFlight dispatch", aerialResult.Summary);
        }
    }
}
