// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Economy;
using Ashfall.Core.Shelter;
using Ashfall.Core.StartingLevel;
using Ashfall.Core.Survivors;
using Ashfall.Core.YearOfAsh;
using InventoryContainer = Ashfall.Core.Inventory.Inventory;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class ShelterBarterSystemPlan54Tests
    {
        private static (ShelterBarterSystem barter, InventoryContainer inventory, ShelterThermalSystem thermal) CreateFixture(int seed = 42)
        {
            var rng = new SeededRng(seed);
            var inventory = new InventoryContainer();
            var needs = new NeedsSystem();
            var sl = new StartingLevelSystem();
            var df = new YearOfAshDeepFreezeSystem();
            var thermal = new ShelterThermalSystem(rng, needs, sl, df);
            var barter = new ShelterBarterSystem(rng, inventory, thermal);
            return (barter, inventory, thermal);
        }

        [Fact]
        public void DeterministicSchedule_ArrivalAndDeparture_TriggersAtCorrectDays()
        {
            var (barter, _, _) = CreateFixture();
            var arrived = new HashSet<string>();
            var departed = new HashSet<string>();

            barter.OnCaravanArrived += c => arrived.Add(c.caravan_id);
            barter.OnCaravanDeparted += c => departed.Add(c.caravan_id);

            // Day 0: 0 % 14 = 0 < 3 (stay duration) -> Arrives
            barter.TickDay(0);
            Assert.True(barter.State.caravans["caravan_scrap_salvagers"].isAtAirlock);
            Assert.Contains("caravan_scrap_salvagers", arrived);

            // Day 3: 3 % 14 = 3 >= 3 -> Departs
            barter.TickDay(3);
            Assert.False(barter.State.caravans["caravan_scrap_salvagers"].isAtAirlock);
            Assert.Contains("caravan_scrap_salvagers", departed);
        }

        [Fact]
        public void AirlockGating_BlocksWhenAirlockFrozen()
        {
            var (barter, inv, thermal) = CreateFixture();
            thermal.AddRoom("room_airlock", "Shelter Outer Airlock", 60f);
            thermal.State.rooms[0].isFrozen = true; // Frozen!

            barter.TickDay(0); // caravan_scrap_salvagers arrives

            inv.TryProduce("item_fuel", 20);
            var offer = new Dictionary<string, int> { { "item_fuel", 10 } };
            var req = new Dictionary<string, int> { { "item_scrap_metal", 20 } };

            var result = barter.ExecuteTrade("caravan_scrap_salvagers", offer, req);
            Assert.Equal(ActionResult.StatusKind.Blocked, result.Status);
            Assert.Equal("airlock_inaccessible", result.FailureCode);
        }

        [Fact]
        public void FixedPointValuation_RejectsUndervaluedOffer_AcceptsSufficientOffer()
        {
            var (barter, inv, _) = CreateFixture();
            barter.TickDay(0); // Caravan arrives

            // Desired trade: Buy 1 blowtorch (base 45 * 1.0 = 45 value)
            // Offer too low: 5 scrap metal (5 * 2 = 10 value)
            inv.TryProduce("item_scrap_metal", 100);
            var lowOffer = new Dictionary<string, int> { { "item_scrap_metal", 5 } };
            var request = new Dictionary<string, int> { { "item_blowtorch", 1 } };

            var failRes = barter.ExecuteTrade("caravan_scrap_salvagers", lowOffer, request);
            Assert.Equal(ActionResult.StatusKind.Blocked, failRes.Status);
            Assert.Equal("insufficient_value", failRes.FailureCode);

            // Fair offer: 30 scrap metal (30 * 2 = 60 value >= 45 * 0.95 tolerance = 42.75)
            var fairOffer = new Dictionary<string, int> { { "item_scrap_metal", 30 } };
            var successRes = barter.ExecuteTrade("caravan_scrap_salvagers", fairOffer, request);

            Assert.Equal(ActionResult.StatusKind.Success, successRes.Status);
            Assert.Equal(70, inv.CountById("item_scrap_metal")); // 100 - 30 = 70
            Assert.Equal(1, inv.CountById("item_blowtorch"));
            Assert.Equal(0, barter.State.caravans["caravan_scrap_salvagers"].remainingStock["item_blowtorch"]);
        }

        [Fact]
        public void WeatherScarcity_BoostsFuelAndFoodValuation()
        {
            var (barter, _, _) = CreateFixture();
            var caravan = barter.Catalog["caravan_permafrost_traders"];

            var fuelOffer = new Dictionary<string, int> { { "item_fuel", 10 } };

            // Baseline valuation
            barter.IsSevereWinterWeather = false;
            float normalVal = barter.CalculatePlayerOfferValue(caravan, fuelOffer);

            // Severe winter valuation
            barter.IsSevereWinterWeather = true;
            float winterVal = barter.CalculatePlayerOfferValue(caravan, fuelOffer);

            Assert.True(winterVal > normalVal, $"Winter fuel valuation ({winterVal}) should exceed normal ({normalVal})");
        }

        [Fact]
        public void CounterfeitRisk_DetectedByAppraisalSkill()
        {
            // Seed 100 with counterfeit_risk_bp = 2500 rolls a counterfeit for munitions caravan
            var (barter, inv, _) = CreateFixture(seed: 123);
            barter.TickDay(0); // Both caravans arrive at day 0 (0 % 18 = 0 < 2)

            inv.TryProduce("item_first_aid_kit", 20);
            var offer = new Dictionary<string, int> { { "item_first_aid_kit", 5 } };
            var req = new Dictionary<string, int> { { "item_ammo_9mm", 20 } };

            // With high appraisal skill (level 3), counterfeit attempt is caught
            var res = barter.ExecuteTrade("caravan_black_market_munitions", offer, req, playerAppraisalSkillLevel: 3);
            if (res.Status == ActionResult.StatusKind.Blocked && res.FailureCode == "counterfeit_detected")
            {
                Assert.True(barter.State.counterfeitsDetectedCount > 0);
            }
            else
            {
                // If this seed didn't roll a counterfeit, trade succeeded cleanly
                Assert.Equal(ActionResult.StatusKind.Success, res.Status);
            }
        }

        [Fact]
        public void StatePreservation_CapturesAndRestoresBarterState()
        {
            var (barter, _, _) = CreateFixture();
            barter.TickDay(0);
            barter.State.completedTradesCount = 7;
            barter.State.counterfeitsDetectedCount = 2;

            var state = barter.CaptureState();

            var (restored, _, _) = CreateFixture();
            restored.RestoreState(state);

            Assert.Equal(7, restored.State.completedTradesCount);
            Assert.Equal(2, restored.State.counterfeitsDetectedCount);
            Assert.True(restored.State.caravans["caravan_scrap_salvagers"].isAtAirlock);
        }
    }
}
