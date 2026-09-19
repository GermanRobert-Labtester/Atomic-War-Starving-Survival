// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Expeditions;
using Xunit;

namespace Ashfall.Core.Tests.Expeditions
{
    public sealed class ColonySystemTests
    {
        [Fact]
        public void EstablishColony_CreatesOutpostWithInitialAttributes()
        {
            var system = new ColonySystem();
            var colony = system.EstablishColony("loc_quarry", "Quarry Outpost", ColonyType.Outpost, new[] { "guard_1", "miner_1" }, 60f, currentDay: 5);

            Assert.NotNull(colony);
            Assert.Equal("loc_quarry", colony.LocationId);
            Assert.Equal(ColonyType.Outpost, colony.Type);
            Assert.Equal(2, colony.PopulationIds.Count);
            Assert.Equal(60f, colony.StoredSupplies);
            Assert.Equal(1, system.TotalColonyCount);
        }

        [Fact]
        public void EstablishSupplyLine_DeliversSuppliesOnTick()
        {
            var system = new ColonySystem();
            var colony = system.EstablishColony("loc_mine", "Iron Mine", ColonyType.Settlement, new[] { "miner_1" }, initialSupplies: 20f, currentDay: 1);
            var line = system.EstablishSupplyLine("shelter", colony.ColonyId, cargoCapacity: 100f, dailyFlow: 15f, currentDay: 1);

            // Tick day 2: flow = 15, consumption for 1 survivor = 1.5, net change = +13.5
            system.TickDay(currentDay: 2);

            Assert.Equal(33.5f, colony.StoredSupplies);
            Assert.Equal(2, line.LastSupplyDay);
        }

        [Fact]
        public void AssignSurvivorToColony_AddsSurvivorToPopulation()
        {
            var system = new ColonySystem();
            var colony = system.EstablishColony("loc_radar", "Radar Post", ColonyType.Outpost, null, 10f);

            bool added = system.AssignSurvivorToColony(colony.ColonyId, "scout_1");

            Assert.True(added);
            Assert.Contains("scout_1", colony.PopulationIds);
        }

        [Fact]
        public void TickDay_StarvationDecreasesMorale_WhenNoSupplies()
        {
            var system = new ColonySystem();
            var colony = system.EstablishColony("loc_camp", "Hungry Camp", ColonyType.Outpost, new[] { "dweller_1" }, initialSupplies: 0f);

            float initialMorale = colony.MoraleRating;
            system.TickDay(currentDay: 2);

            Assert.True(colony.MoraleRating < initialMorale);
        }

        [Fact]
        public void SetSupplyLineStatus_DisruptsSupplyDeliveries()
        {
            var system = new ColonySystem();
            var colony = system.EstablishColony("loc_port", "River Port", ColonyType.TradingPost, new[] { "trader_1" }, initialSupplies: 50f);
            var line = system.EstablishSupplyLine("shelter", colony.ColonyId, 100f, 20f, 1);

            system.SetSupplyLineStatus(line.LineId, SupplyLineStatus.Disrupted);

            system.TickDay(currentDay: 2);

            // Supplies consumed, none delivered
            Assert.True(colony.StoredSupplies < 50f);
        }

        [Fact]
        public void CaptureState_And_RestoreState_RoundTripsAccurately()
        {
            var system1 = new ColonySystem();
            var c = system1.EstablishColony("loc_fort", "Fort Hope", ColonyType.Fortress, new[] { "soldier_1" }, 80f, 3);
            var line = system1.EstablishSupplyLine("shelter", c.ColonyId, 150f, 25f, 3);

            var state = system1.CaptureState();
            Assert.Single(state.Colonies);
            Assert.Single(state.SupplyLines);

            var system2 = new ColonySystem();
            system2.RestoreState(state);

            Assert.Equal(1, system2.TotalColonyCount);
            Assert.Equal(1, system2.ActiveSupplyLineCount);
            var restoredColony = system2.GetColony(c.ColonyId);
            Assert.NotNull(restoredColony);
            Assert.Equal("Fort Hope", restoredColony.Name);
            Assert.Equal(ColonyType.Fortress, restoredColony.Type);
        }
    }
}
