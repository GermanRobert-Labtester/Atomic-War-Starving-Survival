// SPDX-License-Identifier: MIT
// ASHFALL UI Gate: Shelter Barter Panel Route & Surface Contract Tests (Plan 147 follow-up)
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Economy;
using Ashfall.Core.Shelter;
using Ashfall.Core.StartingLevel;
using Ashfall.Core.Survivors;
using Ashfall.Core.UI;
using Ashfall.Core.YearOfAsh;
using InventoryContainer = Ashfall.Core.Inventory.Inventory;
using Xunit;

namespace Ashfall.Core.Tests.UI
{
    public sealed class ShelterBarterPanelRouteTests
    {
        public ShelterBarterPanelRouteTests()
        {
            PanelRegistryBootstrap.RegisterAll();
        }

        [Fact]
        public void ShelterBarterRoute_IsRegisteredInPanelRegistry()
        {
            Assert.True(PanelRegistry.IsRegistered("shelter_barter"),
                "Panel 'shelter_barter' must be registered in PanelRegistry.");

            var desc = PanelRegistry.Get("shelter_barter");
            Assert.NotNull(desc);
            Assert.Equal("shelter_barter", desc.Id);
            Assert.Equal("Shelter Barter Terminal", desc.DisplayName);
            Assert.Equal(PanelGroup.Expanded, desc.Group);
            Assert.Equal(PanelMaturity.Live, desc.Maturity);
            Assert.True(desc.IsPlayerNavigable, "shelter_barter must be player navigable.");
            Assert.Contains("inventory", desc.SetupDependencies);
        }

        [Fact]
        public void ShelterBarterRoute_IsIncludedInPlayerSurfaceManifest()
        {
            var manifest = PlayerSurfaceManifest.Generate();
            Assert.NotNull(manifest);

            var contract = manifest.Contracts.FirstOrDefault(c => c.PanelId == "shelter_barter");
            Assert.NotNull(contract);
            Assert.Equal(SurfaceRouteKind.ExpandedShelter, contract.RouteKind);
            Assert.Equal(SurfaceActionCoverage.InteractiveCommands, contract.ActionCoverage);
            Assert.Equal(SurfaceRenderCoverage.ProductionRendered, contract.RenderCoverage);
            Assert.Contains("inventory", contract.SetupDependencies);
        }

        [Fact]
        public void BarterSystem_TradeExecution_FulfillsExpectedContract()
        {
            var rng = new SeededRng(42);
            var inventory = new InventoryContainer();
            var needs = new NeedsSystem();
            var sl = new StartingLevelSystem();
            var df = new YearOfAshDeepFreezeSystem();
            var thermal = new ShelterThermalSystem(rng, needs, sl, df);
            var barter = new ShelterBarterSystem(rng, inventory, thermal);

            // Day 0: caravan_scrap_salvagers arrives
            barter.TickDay(0);
            var cState = barter.State.caravans["caravan_scrap_salvagers"];
            Assert.True(cState.isAtAirlock);

            // Populate player stock
            inventory.TryProduce("item_fuel", 20);

            // Attempt balanced trade
            var offer = new Dictionary<string, int> { { "item_fuel", 5 } }; // value: 5 * 10 = 50
            var req = new Dictionary<string, int> { { "item_scrap_metal", 10 } }; // value: 10 * 3 = 30 * price_mult (~1.1) ~ 33

            var result = barter.ExecuteTrade("caravan_scrap_salvagers", offer, req);
            Assert.True(result.IsSuccess, $"Trade should succeed, but failed with: {result.FailureCode}");
            Assert.True(barter.State.completedTradesCount > 0);
            Assert.True(inventory.CountById("item_scrap_metal") >= 10);
        }
    }
}
