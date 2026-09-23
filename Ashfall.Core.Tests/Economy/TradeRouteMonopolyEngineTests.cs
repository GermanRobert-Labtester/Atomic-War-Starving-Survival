// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.Economy;
using Xunit;

namespace Ashfall.Core.Tests.Economy
{
    public class TradeRouteMonopolyEngineTests
    {
        [Fact]
        public void ValidateExclusiveGood_TierAndItemRules_EnforcesDiscipline()
        {
            var engine = new TradeRouteMonopolyEngine();
            var forbiddenGoods = new HashSet<string> { "item_strictly_superior_battery", "item_op_rifle" };

            // Contract with no exclusive good
            var noExclusiveContract = new TradeRouteContract("route_1", "holdfast_a", 5, 10, 1, exclusiveGoodId: null);
            var res1 = engine.ValidateExclusiveGood(noExclusiveContract, forbiddenGoods);
            Assert.False(res1.IsValid);
            Assert.Equal("NoExclusiveGoodConfigured", res1.FailureReason);

            // Contract with forbidden strictly superior good
            var forbiddenContract = new TradeRouteContract("route_2", "holdfast_b", 5, 10, 1, exclusiveGoodId: "item_op_rifle");
            var res2 = engine.ValidateExclusiveGood(forbiddenContract, forbiddenGoods);
            Assert.False(res2.IsValid);
            Assert.Equal("ForbiddenStrictlySuperiorGood", res2.FailureReason);

            // Contract with valid exclusive good, but reliability is Tier 1 (threshold 20 not reached)
            var validGoodTier1Contract = new TradeRouteContract("route_3", "holdfast_c", 5, 10, 1, exclusiveGoodId: "item_rare_medical_salve");
            var res3 = engine.ValidateExclusiveGood(validGoodTier1Contract, forbiddenGoods);
            Assert.False(res3.IsValid);
            Assert.Equal("Tier4ReliabilityRequired", res3.FailureReason);

            // Raise reliability to Tier 4 (20 successful runs)
            for (int i = 0; i < 20; i++)
            {
                validGoodTier1Contract.RecordRunOutcome(TradeRouteRunOutcome.OnTime, 1 + i * 5);
            }
            Assert.Equal(TradeRouteReliabilityTier.Tier4, validGoodTier1Contract.Tier);

            var res4 = engine.ValidateExclusiveGood(validGoodTier1Contract, forbiddenGoods);
            Assert.True(res4.IsValid);
            Assert.Empty(res4.FailureReason);
        }

        [Fact]
        public void MonopolyPremium_DegradesWithMarketSaturation_FloorsAtZeroPremium()
        {
            var engine = new TradeRouteMonopolyEngine();
            string routeId = "route_shelter_krasthold";
            string goodId = "item_pure_antibiotic";

            // Baseline: 0 deliveries -> +250 permille (+25%)
            int baselinePremium = engine.GetCurrentMonopolyPremiumPermille(routeId);
            Assert.Equal(250, baselinePremium);

            int baseValue = 100;
            int unitVal0 = engine.CalculateExclusiveGoodUnitValue(baseValue, routeId);
            Assert.Equal(125, unitVal0); // 100 + 25%

            // Deliver 10 units: 10 * 25 permille = 250 permille saturation
            engine.RecordDelivery(routeId, goodId, 10, currentDay: 5);
            int sat1 = engine.RouteStates[routeId].SaturationPermille;
            Assert.Equal(250, sat1);

            // Remaining margin: (1000 - 250) / 1000 = 750 / 1000. Premium = 250 * 750 / 1000 = 187 permille (18.7%)
            int premium1 = engine.GetCurrentMonopolyPremiumPermille(routeId);
            Assert.Equal(187, premium1);

            // Deliver 30 more units: 30 * 25 = 750 -> Total 1000 permille saturation (completely saturated)
            engine.RecordDelivery(routeId, goodId, 30, currentDay: 10);
            int sat2 = engine.RouteStates[routeId].SaturationPermille;
            Assert.Equal(1000, sat2);

            int premium2 = engine.GetCurrentMonopolyPremiumPermille(routeId);
            Assert.Equal(0, premium2); // 0% premium

            int unitValSaturated = engine.CalculateExclusiveGoodUnitValue(baseValue, routeId);
            Assert.Equal(100, unitValSaturated); // Sells at baseline price, no infinite money glitch
        }

        [Fact]
        public void CargoQuota_ScaledByCaravanSlots_CappedAtMaxUnits()
        {
            var engine = new TradeRouteMonopolyEngine();

            var singleSlot = new TradeRouteContract("route_small", "h_1", 3, 5, caravanSlotsRequired: 1);
            Assert.Equal(3, engine.CalculateCargoQuota(singleSlot)); // 1 * 3 = 3

            var doubleSlot = new TradeRouteContract("route_med", "h_2", 3, 5, caravanSlotsRequired: 2);
            Assert.Equal(5, engine.CalculateCargoQuota(doubleSlot)); // min(5, 2 * 3 = 6) -> 5

            var tripleSlot = new TradeRouteContract("route_large", "h_3", 3, 5, caravanSlotsRequired: 3);
            Assert.Equal(5, engine.CalculateCargoQuota(tripleSlot)); // min(5, 9) -> 5
        }

        [Fact]
        public void ProcessDailyRecovery_RestoresMarketDemand_FloorsAtZeroSaturation()
        {
            var engine = new TradeRouteMonopolyEngine();
            string routeId = "route_north";

            // Saturated to 400 permille on day 10
            engine.RecordDelivery(routeId, "item_rare_seeds", 16, currentDay: 10);
            Assert.Equal(400, engine.RouteStates[routeId].SaturationPermille);

            // Advance 4 days to day 14: 4 * 50 permille recovery = 200 permille restored
            engine.ProcessDailyRecovery(currentDay: 14);
            Assert.Equal(200, engine.RouteStates[routeId].SaturationPermille);

            // Advance 10 more days to day 24: 10 * 50 = 500 recovery -> floors at 0
            engine.ProcessDailyRecovery(currentDay: 24);
            Assert.Equal(0, engine.RouteStates[routeId].SaturationPermille);
            Assert.Equal(250, engine.GetCurrentMonopolyPremiumPermille(routeId)); // Fully restored to 25% premium
        }

        [Fact]
        public void SaveRestoreRoundTrip_PreservesAllRouteSaturationStates()
        {
            var engine1 = new TradeRouteMonopolyEngine();
            engine1.RecordDelivery("route_alpha", "item_special_alloy", 8, currentDay: 12);
            engine1.RecordDelivery("route_beta", "item_rare_herb", 4, currentDay: 15);

            var saveState = engine1.CaptureState();
            Assert.NotNull(saveState);
            Assert.Equal(2, saveState.RouteStates.Count);

            var engine2 = new TradeRouteMonopolyEngine();
            engine2.RestoreState(saveState);

            Assert.Equal(2, engine2.RouteStates.Count);
            Assert.True(engine2.RouteStates.ContainsKey("route_alpha"));
            Assert.True(engine2.RouteStates.ContainsKey("route_beta"));

            var stateAlpha = engine2.RouteStates["route_alpha"];
            Assert.Equal(8, stateAlpha.TotalUnitsDelivered);
            Assert.Equal(200, stateAlpha.SaturationPermille); // 8 * 25 = 200
            Assert.Equal(12, stateAlpha.LastDeliveryDay);
        }
    }
}
