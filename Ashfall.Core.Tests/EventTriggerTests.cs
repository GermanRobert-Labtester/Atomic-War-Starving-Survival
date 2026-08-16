using System;
using System.Collections.Generic;
using Xunit;
using Ashfall.Core.Events;
using Ashfall.Core.Journal;
using Ashfall.Core.Muster;

namespace Ashfall.Core.Tests
{
    public class EventTriggerTests
    {
        [Fact]
        public void SameSeed_SameConditions_ProducesSameEventTriggers()
        {
            var bus1 = new SimpleEventBus();
            var bus2 = new SimpleEventBus();

            var hydro1 = new HydroBaronsSystem();
            var hydro2 = new HydroBaronsSystem();

            var cold1 = new ColdCountSystem();
            var cold2 = new ColdCountSystem();

            var events1 = new List<string>();
            var events2 = new List<string>();

            bus1.Subscribe("event_the_thin_margin_disclosure", _ => events1.Add("disclosure"));
            bus2.Subscribe("event_the_thin_margin_disclosure", _ => events2.Add("disclosure"));

            bus1.Subscribe("event_measurement_broadcast", _ => events1.Add("broadcast"));
            bus2.Subscribe("event_measurement_broadcast", _ => events2.Add("broadcast"));

            // Simulation 1
            hydro1.ResolveApproach(QuestApproach.B);
            if (hydro1.AdminReform) bus1.Publish("event_the_thin_margin_disclosure");
            cold1.TransmitFindings(250);
            if (cold1.BroadcastSent) bus1.Publish("event_measurement_broadcast");

            // Simulation 2
            hydro2.ResolveApproach(QuestApproach.B);
            if (hydro2.AdminReform) bus2.Publish("event_the_thin_margin_disclosure");
            cold2.TransmitFindings(250);
            if (cold2.BroadcastSent) bus2.Publish("event_measurement_broadcast");

            Assert.Equal(events1, events2);
            Assert.Equal(2, events1.Count);
        }

        [Fact]
        public void GatedConditions_NegativeTest_NoEventTriggeredWhenGatedOut()
        {
            var bus = new SimpleEventBus();
            var hydro = new HydroBaronsSystem();
            var cold = new ColdCountSystem();
            var events = new List<string>();

            bus.Subscribe("event_the_thin_margin_disclosure", _ => events.Add("disclosure"));
            bus.Subscribe("event_the_thirsty_season", _ => events.Add("thirsty"));
            bus.Subscribe("event_measurement_broadcast", _ => events.Add("broadcast"));

            // Resolve Approach A (Undercut) -> neither audit (B) nor seizure (C)
            hydro.ResolveApproach(QuestApproach.A);

            if (hydro.AdminReform) bus.Publish("event_the_thin_margin_disclosure");
            if (hydro.PlantSeized) bus.Publish("event_the_thirsty_season");
            if (cold.BroadcastSent) bus.Publish("event_measurement_broadcast");

            Assert.Empty(events);
        }

        [Fact]
        public void HydroBarons_ApproachC_TriggersThirstySeason()
        {
            var bus = new SimpleEventBus();
            var hydro = new HydroBaronsSystem();
            string fired = string.Empty;

            bus.Subscribe("event_the_thirsty_season", _ => fired = "event_the_thirsty_season");

            hydro.ResolveApproach(QuestApproach.C);
            if (hydro.PlantSeized)
                bus.Publish("event_the_thirsty_season");

            Assert.Equal("event_the_thirsty_season", fired);
            Assert.True(hydro.PlantSeized);
            Assert.Equal(0, hydro.QueuePosition);
        }

        [Fact]
        public void ColdCount_TransmitFindings_TriggersBroadcastEvent()
        {
            var bus = new SimpleEventBus();
            var cold = new ColdCountSystem();
            string fired = string.Empty;

            bus.Subscribe("event_measurement_broadcast", _ => fired = "event_measurement_broadcast");

            cold.SupplyPower(ColdCountState.RequiredPowerDays);
            cold.DeliverShielding(ColdCountState.RequiredShieldingUnits);
            Assert.True(cold.CanCompleteProvenanceRun());
            cold.CompleteProvenanceRun();

            cold.TransmitFindings(250);
            if (cold.BroadcastSent)
                bus.Publish("event_measurement_broadcast");

            Assert.Equal("event_measurement_broadcast", fired);
            Assert.False(cold.BroadcastIsCaveated);
        }

        [Fact]
        public void EventTriggerSaveRoundTrip_PreservesState()
        {
            var hydro = new HydroBaronsSystem();
            hydro.ResolveApproach(QuestApproach.B);
            var save = hydro.CaptureState();

            var hydroRestored = new HydroBaronsSystem();
            hydroRestored.RestoreState(save);

            Assert.True(hydroRestored.AdminReform);
            Assert.True(hydroRestored.RateCardRevised);
            Assert.Equal("B", hydroRestored.ChosenApproach);
        }
    }
}
