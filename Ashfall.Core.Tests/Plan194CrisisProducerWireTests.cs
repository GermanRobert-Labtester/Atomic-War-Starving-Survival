// SPDX-License-Identifier: MIT
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Radiation;
using Ashfall.Core.Shelter;
using Ashfall.Core.Survivors;
using Ashfall.Core.UI;
using Xunit;

namespace Ashfall.Core.Tests.Plan194Alerts
{
    /// <summary>
    /// DEBT-194-CRISIS-PRODUCER-WIRE — the presentation aggregator now reads the
    /// fire and flood authorities plus the previously unused radiation and fate
    /// bind facts. Producers own the facts; the aggregator only presents them,
    /// and no alert controller or alert save is introduced.
    /// </summary>
    public sealed class Plan194CrisisProducerWireTests
    {
        private static CrisisPresentationCoordinator CreateCoordinator(
            PowerGridSystem? power = null,
            ShelterFireHazardSystem? fire = null,
            SumpFloodingSystem? sump = null,
            RadiationSystem? radiation = null,
            SurvivorFateSystem? fate = null)
        {
            var coordinator = new CrisisPresentationCoordinator();
            coordinator.Bind(
                power, disease: null, weather: null, startingLevel: null,
                radiation: radiation, fate: fate, fire: fire, sump: sump);
            return coordinator;
        }

        [Fact]
        public void Unresolved_fire_raises_a_critical_fire_crisis()
        {
            var fire = new ShelterFireHazardSystem();
            Assert.True(fire.Ignite("inc_1", "zone_generator", 1, new List<FireZoneState> { new FireZoneState { fireLevel = 0.8f } }));

            var coordinator = CreateCoordinator(fire: fire);
            coordinator.EvaluateCrisisState();

            Assert.True(coordinator.CurrentSnapshot.IsActive);
            Assert.Equal("crisis_shelter_fire", coordinator.CurrentSnapshot.CrisisId);
            Assert.Equal(CrisisSeverity.Critical, coordinator.CurrentSnapshot.Severity);
        }

        [Fact]
        public void Suppressed_fire_does_not_raise_a_crisis()
        {
            var fire = new ShelterFireHazardSystem();
            Assert.True(fire.Ignite("inc_1", "zone_generator", 1, new List<FireZoneState> { new FireZoneState { fireLevel = 0.8f } }));
            var incident = fire.GetIncident("inc_1");
            Assert.NotNull(incident);
            incident!.isSuppressed = true;

            var coordinator = CreateCoordinator(fire: fire);
            coordinator.EvaluateCrisisState();

            Assert.False(coordinator.CurrentSnapshot.IsActive);
        }

        [Fact]
        public void Flooded_sump_node_raises_a_flood_crisis()
        {
            var sump = CreateSump();
            sump.State.nodes.Add(new SumpNode { nodeId = "node_lower", isFlooded = true });

            var coordinator = CreateCoordinator(sump: sump);
            coordinator.EvaluateCrisisState();

            Assert.True(coordinator.CurrentSnapshot.IsActive);
            Assert.Equal("crisis_sump_flooding", coordinator.CurrentSnapshot.CrisisId);
        }

        [Fact]
        public void Dry_sump_nodes_raise_nothing()
        {
            var sump = CreateSump();
            sump.State.nodes.Add(new SumpNode { nodeId = "node_lower", isFlooded = false });

            var coordinator = CreateCoordinator(sump: sump);
            coordinator.EvaluateCrisisState();

            Assert.False(coordinator.CurrentSnapshot.IsActive);
        }

        private static SumpFloodingSystem CreateSump()
        {
            var state = new PowerGridState { GenerationWatts = 800f, FuelUnits = 100f };
            var rooms = new List<PowerGridRoom> { new PowerGridRoom("room_core", "Shelter Core", 200f) };
            var power = new PowerGridSystem(state, rooms, new SeededRng(42));
            return new SumpFloodingSystem(new SeededRng(70), new Ashfall.Core.World.WeatherSystem(), power,
                new Ashfall.Core.YearOfAsh.YearOfAshDeepFreezeSystem());
        }

        [Fact]
        public void Existing_power_priority_still_wins_over_fire()
        {
            // Priority order is not reordered: the append-only producers sit below
            // the original four, so current behaviour and tests are unchanged.
            var state = new PowerGridState { GenerationWatts = 0f, FuelUnits = 0f };
            var rooms = new List<PowerGridRoom> { new PowerGridRoom("room_main", "Main", 100f) };
            var power = new PowerGridSystem(state, rooms, new SeededRng(1));

            var fire = new ShelterFireHazardSystem();
            Assert.True(fire.Ignite("inc_1", "zone_generator", 1, new List<FireZoneState> { new FireZoneState { fireLevel = 0.8f } }));

            var coordinator = CreateCoordinator(power: power, fire: fire);
            coordinator.EvaluateCrisisState();

            Assert.Equal("crisis_power_failure", coordinator.CurrentSnapshot.CrisisId);
        }

        [Fact]
        public void No_producers_bound_means_no_crisis()
        {
            var coordinator = CreateCoordinator();
            coordinator.EvaluateCrisisState();
            Assert.False(coordinator.CurrentSnapshot.IsActive);
        }
    }
}
