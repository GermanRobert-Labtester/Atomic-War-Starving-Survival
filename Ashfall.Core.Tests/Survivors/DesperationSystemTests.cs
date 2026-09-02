// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Disease;
using Ashfall.Core.Inventory;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Survivors
{
    public class DesperationSystemTests
    {
        [Fact]
        public void DesperationSystem_StarvationCrisisGating_BlocksWhenBelowThreshold()
        {
            var rng = new SeededRng(187);
            var inv = new Inventory.Inventory();
            var needs = new NeedsSystem();

            var survivor = new SurvivorNeedsState { Id = "dweller_1", Hunger = 70f, Morale = 80f, Health = 100f };
            needs.Register(survivor);

            var system = new DesperationSystem(rng, inv, needs);
            system.RegisterCorpse("corpse_dweller_2");

            bool eligible = system.IsActionEligible("dweller_1", "desperation_consume_corpse", "corpse_dweller_2");
            Assert.False(eligible);

            var result = system.HarvestCorpse("dweller_1", "corpse_dweller_2", "desperation_consume_corpse", 5);
            Assert.False(result.IsSuccess);
            Assert.Equal("crisis_not_reached", result.FailureCode);
            Assert.Equal(0, inv.CountById("raw_meat"));
        }

        [Fact]
        public void DesperationSystem_HarvestCorpse_WhenStarving_GrantsMeatAndAppliesShockwave()
        {
            var rng = new SeededRng(187);
            var inv = new Inventory.Inventory();
            var needs = new NeedsSystem();

            var actor = new SurvivorNeedsState { Id = "dweller_1", Hunger = 95f, Morale = 80f, Health = 100f };
            var innocent = new SurvivorNeedsState { Id = "dweller_2", Hunger = 92f, Morale = 80f, Health = 100f };
            needs.Register(actor);
            needs.Register(innocent);

            var system = new DesperationSystem(rng, inv, needs);
            system.RegisterCorpse("corpse_dweller_3");

            bool tabooFired = false;
            system.OnTabooBroken += (record) => tabooFired = true;

            var result = system.HarvestCorpse("dweller_1", "corpse_dweller_3", "desperation_consume_corpse", 10);

            Assert.True(result.IsSuccess);
            Assert.True(tabooFired);

            // Meat yield granted
            Assert.True(inv.CountById("raw_meat") > 0);

            // Corpse marked harvested
            Assert.Contains("corpse_dweller_3", system.State.harvestedCorpseIds);
            Assert.DoesNotContain("corpse_dweller_3", system.State.unburiedCorpseIds);

            // Cannibal trait tracked
            Assert.Contains("dweller_1", system.State.cannibalSurvivorIds);

            // Morale shockwave applied
            Assert.True(innocent.Morale < 80f);
            Assert.True(actor.Morale < 80f);

            // Mutiny pressure increased
            Assert.True(system.MutinyPressure > 0f);
        }

        [Fact]
        public void DesperationSystem_HarvestCorpse_CannotHarvestSameCorpseTwice()
        {
            var rng = new SeededRng(187);
            var inv = new Inventory.Inventory();
            var needs = new NeedsSystem();

            var actor = new SurvivorNeedsState { Id = "dweller_1", Hunger = 95f, Morale = 80f, Health = 100f };
            needs.Register(actor);

            var system = new DesperationSystem(rng, inv, needs);
            system.RegisterCorpse("corpse_dweller_alpha");

            var first = system.HarvestCorpse("dweller_1", "corpse_dweller_alpha", "desperation_consume_corpse", 10);
            Assert.True(first.IsSuccess);
            int firstMeat = inv.CountById("raw_meat");

            var second = system.HarvestCorpse("dweller_1", "corpse_dweller_alpha", "desperation_consume_corpse", 11);
            Assert.False(second.IsSuccess);
            Assert.Equal(firstMeat, inv.CountById("raw_meat"));
        }
    }
}
