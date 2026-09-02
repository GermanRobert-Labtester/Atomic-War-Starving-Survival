using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests.Expeditions
{
    public sealed class NavalExpeditionSystemTests
    {
        private static ExpeditionNavalSystem CreateSystem()
        {
            return new ExpeditionNavalSystem();
        }

        [Fact]
        public void Downstream_TravelIsFasterThanUpstream()
        {
            var sys = CreateSystem();
            var vessel = sys.CreateInstance("vessel_rowboat");

            var downstreamRoute = new MapRoute
            {
                From = "loc_holdfast",
                To = "loc_river_delta",
                DistanceKm = 30f,
                CurrentStrength = 0.5f,
                TravelDomain = "water"
            };

            var upstreamRoute = new MapRoute
            {
                From = "loc_river_delta",
                To = "loc_holdfast",
                DistanceKm = 30f,
                CurrentStrength = -0.5f,
                TravelDomain = "water"
            };

            var estDown = sys.EstimateRoute(vessel, downstreamRoute, "Open");
            var estUp = sys.EstimateRoute(vessel, upstreamRoute, "Open");

            Assert.True(estDown.effectiveSpeedKmH > estUp.effectiveSpeedKmH);
            Assert.True(estDown.travelHours < estUp.travelHours);
        }

        [Fact]
        public void FrozenWaterway_BlocksRouteCompletely()
        {
            var sys = CreateSystem();
            var vessel = sys.CreateInstance("vessel_motorboat");

            var route = new MapRoute
            {
                From = "loc_holdfast",
                To = "loc_river_delta",
                DistanceKm = 25f,
                TravelDomain = "water"
            };

            var est = sys.EstimateRoute(vessel, route, "Frozen");

            Assert.True(est.isClosedByIce);
            Assert.Equal(0f, est.effectiveSpeedKmH);
            Assert.Contains("frozen", est.closureReason, System.StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void RestrictedIce_SlowsVesselNavigation()
        {
            var sys = CreateSystem();
            var vessel = sys.CreateInstance("vessel_improvised_raft");

            var route = new MapRoute
            {
                From = "loc_holdfast",
                To = "loc_river_delta",
                DistanceKm = 20f,
                TravelDomain = "water"
            };

            var estOpen = sys.EstimateRoute(vessel, route, "Open");
            var estRestricted = sys.EstimateRoute(vessel, route, "Restricted");

            Assert.False(estRestricted.isClosedByIce);
            Assert.True(estRestricted.effectiveSpeedKmH < estOpen.effectiveSpeedKmH);
        }

        [Fact]
        public void ToxicWater_InflictsHullDamage()
        {
            var sys = CreateSystem();
            var vessel = sys.CreateInstance("vessel_improvised_raft");
            float startDurability = vessel.hullCondition;

            sys.ApplyWaterCorrosion(vessel, 0.5f);

            Assert.True(vessel.hullCondition < startDurability);
        }

        [Fact]
        public void Motorboat_RequiresFuel_WhileRowboat_RequiresStamina()
        {
            var sys = CreateSystem();
            var motorboat = sys.CreateInstance("vessel_motorboat");
            var rowboat = sys.CreateInstance("vessel_rowboat");

            var route = new MapRoute
            {
                From = "loc_holdfast",
                To = "loc_river_delta",
                DistanceKm = 40f,
                TravelDomain = "water"
            };

            var estMotor = sys.EstimateRoute(motorboat, route, "Open");
            var estRow = sys.EstimateRoute(rowboat, route, "Open");

            Assert.True(estMotor.fuelRequired > 0f);
            Assert.Equal(0f, estMotor.staminaRequired);

            Assert.Equal(0f, estRow.fuelRequired);
            Assert.True(estRow.staminaRequired > 0f);
        }

        [Fact]
        public void ProjectToVehicleProfile_ProducesValidExpeditionProfile()
        {
            var sys = CreateSystem();
            var vessel = sys.CreateInstance("vessel_motorboat");

            var profile = sys.ProjectToVehicleProfile(vessel);

            Assert.NotNull(profile);
            Assert.Equal("vessel_motorboat", profile.vehicleId);
            Assert.True(profile.speedMultiplier > 0f);
            Assert.True(profile.cargoCapacityKg >= 400f);
        }
    }
}
