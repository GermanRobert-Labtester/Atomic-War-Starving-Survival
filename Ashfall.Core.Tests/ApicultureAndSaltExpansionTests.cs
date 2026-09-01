using System;
using System.Collections.Generic;
using Ashfall.Core.Foundry;
using Ashfall.Core.Greenhouse;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class ApicultureAndSaltExpansionTests
    {
        [Fact]
        public void ApicultureSystem_HealthyColony_AccumulatesHoneyAndWax()
        {
            var api = new ApicultureSystem();
            api.InstallHive("hive_bay_alpha", "bay_01", day: 1);

            var hive = api.GetHive("hive_bay_alpha");
            Assert.NotNull(hive);
            Assert.Equal(1.0f, hive.queenVitality);
            Assert.True(hive.colonyPopulation > 0f);

            var rng = new SeededRng(42);

            // Tick 10 days of healthy conditions
            for (int day = 2; day <= 11; day++)
            {
                api.InspectHive("hive_bay_alpha", day: day);
                api.RefillFeed("hive_bay_alpha", 1f);
                api.RefillWater("hive_bay_alpha", 1f);
                api.TickDaily(day, greenhouseTemperatureC: 22f, greenhouseContamination: 0f, radiationLevel: 0f, rng: rng);
            }

            Assert.True(hive.honeyBuffer > 0f);
            Assert.True(hive.waxBuffer > 0f);

            var harvest = api.Harvest("hive_bay_alpha");
            Assert.True(harvest.honey > 0f);
            Assert.True(harvest.wax > 0f);
            Assert.Equal(0f, hive.honeyBuffer);
            Assert.Equal(0f, hive.waxBuffer);
        }

        [Fact]
        public void SaltMineExtractionSystem_ExtractsSaltAndBrine()
        {
            var salt = new SaltMineExtractionSystem();
            salt.RegisterVein(new SaltMineVeinState
            {
                veinId = "vein_deep_halite",
                displayName = "Deep Halite Vein",
                isUnlocked = false,
                remainingOre = 1000f,
                extractionRate = 15f,
                maxWorkers = 4,
                drillCondition = 1.0f,
                pumpPressure = 1.0f
            });

            salt.UnlockVein("vein_deep_halite");
            salt.AssignWorkers("vein_deep_halite", 4);

            var vein = salt.Veins["vein_deep_halite"];
            Assert.NotNull(vein);
            Assert.True(vein.isUnlocked);
            Assert.Equal(4, vein.assignedWorkers);

            var rng = new SeededRng(101);

            // Tick extraction
            for (int day = 1; day <= 5; day++)
            {
                salt.TickDaily(day, rng);
            }

            Assert.True(salt.State.saltStorage > 0f);
            Assert.True(salt.State.brineStorage > 0f);
            Assert.True(salt.State.totalSaltProduced > 0f);
        }

        [Fact]
        public void SaltMineExtractionSystem_TreatyDeliveryFulfillment()
        {
            var salt = new SaltMineExtractionSystem();
            salt.RegisterVein(new SaltMineVeinState
            {
                veinId = "vein_deep_halite",
                displayName = "Deep Halite Vein",
                isUnlocked = false,
                remainingOre = 2000f,
                extractionRate = 20f,
                maxWorkers = 4,
                drillCondition = 1.0f,
                pumpPressure = 1.0f
            });

            salt.UnlockVein("vein_deep_halite");
            salt.AssignWorkers("vein_deep_halite", 4);

            var rng = new SeededRng(202);

            // Build up stock
            for (int day = 1; day <= 10; day++)
            {
                salt.TickDaily(day, rng);
            }

            var delivery = salt.DeliverToTreaty(day: 15);

            Assert.NotNull(delivery);
            Assert.True(delivery.accepted);
            Assert.True(delivery.quantityDelivered > 0f);
            Assert.Equal(SaltMineExtractionSystem.TreatyBrinePipe, delivery.treatyId);
        }
    }
}
