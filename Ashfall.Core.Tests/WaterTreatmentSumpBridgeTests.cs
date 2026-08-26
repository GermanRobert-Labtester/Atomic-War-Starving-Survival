using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Shelter;
using Ashfall.Core.World;
using Ashfall.Core.YearOfAsh;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class WaterTreatmentSumpBridgeTests
    {
        private static WaterTreatmentSystem CreateWaterSystem()
        {
            return new WaterTreatmentSystem(NullLog.Instance);
        }

        private static SumpFloodingSystem CreateSump(out WeatherSystem weather, out PowerGridSystem power, out YearOfAshDeepFreezeSystem df)
        {
            weather = new WeatherSystem();
            weather.BindProfile(new SeasonProfileDef { id = "default" }, 42);
            var state = new PowerGridState { GenerationWatts = 800, FuelUnits = 100, BatteryCapacityWh = 4000, BatteryReserveWh = 2000 };
            var rooms = new List<PowerGridRoom>
            {
                new PowerGridRoom("sump_a", "Lower Level", 100f)
            };
            power = new PowerGridSystem(state, rooms, new SeededRng(42));
            df = new YearOfAshDeepFreezeSystem();
            return new SumpFloodingSystem(new SeededRng(42), weather, power, df);
        }

        [Fact]
        public void SetIncomingContamination_HighLevel_DegradesFilterOnTick()
        {
            var water = CreateWaterSystem();
            water.AddWater(WaterType.Raw, 20f);
            float beforeFilter = water.State.filterIntegrity;
            water.SetIncomingContamination(0.8f);
            Assert.Equal(0.8f, water.State.incomingContaminationLevel, 2);

            bool pathogenFired = false;
            water.OnPathogenExposure += _ => pathogenFired = true;

            water.TickDay(1);

            // Filter should degrade by 0.8*5 =4, plus passive 0.2 (20*0.01)
            Assert.True(water.State.filterIntegrity < beforeFilter, "Filter should degrade due to flood contamination");
            Assert.True(pathogenFired, "Pathogen exposure should fire when contamination >0.3");
            // Incoming level decays by 0.15 per day
            Assert.Equal(0.65f, water.State.incomingContaminationLevel, 2);
        }

        [Fact]
        public void SetIncomingContamination_LowLevel_DoesNotFirePathogen()
        {
            var water = CreateWaterSystem();
            water.SetIncomingContamination(0.2f);
            bool pathogenFired = false;
            water.OnPathogenExposure += _ => pathogenFired = true;
            water.TickDay(1);
            Assert.False(pathogenFired, "Low contamination (0.2) should not fire pathogen exposure (>0.3 threshold)");
        }

        [Fact]
        public void SumpFloodIncident_WiredToWaterTreatment_SetsContamination()
        {
            // Simulate host wiring: Sump OnIncident FloodStart → Water SetIncomingContamination(0.8)
            var water = CreateWaterSystem();
            var sump = CreateSump(out _, out _, out _);
            sump.AddNode("node_a", "Sump A", 200f);
            // Pre-fill near flood threshold
            var node = sump.State.nodes.Find(n => n.nodeId == "node_a");
            node!.waterLevelCm = 190f; // >80% of max

            // Wire as host does
            sump.OnIncident += incident =>
            {
                if (incident.kind == FloodIncidentKind.FloodStart || incident.kind == FloodIncidentKind.Contamination)
                    water.SetIncomingContamination(0.8f);
            };

            sump.TickDay(10);

            // After tick, node should be flooded and water should have contamination
            Assert.True(node.isFlooded, "Node should be flooded after TickDay with high water level");
            Assert.Equal(0.8f, water.State.incomingContaminationLevel, 2);
        }

        [Fact]
        public void SaveRoundTrip_PreservesIncomingContamination()
        {
            var water = CreateWaterSystem();
            water.SetIncomingContamination(0.75f);
            var state = water.CaptureState();
            var water2 = CreateWaterSystem();
            water2.RestoreState(state);
            Assert.Equal(0.75f, water2.State.incomingContaminationLevel, 2);
        }
    }
}
