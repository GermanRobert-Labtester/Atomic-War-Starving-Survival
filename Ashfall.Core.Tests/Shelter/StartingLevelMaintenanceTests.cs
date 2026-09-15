// SPDX-License-Identifier: MIT
using Ashfall.Core.Inventory;
using Ashfall.Core.StartingLevel;
using Xunit;

namespace Ashfall.Core.Tests.Shelter
{
    public sealed class StartingLevelMaintenanceTests
    {
        [Fact]
        public void Service_ConsumesCanonicalInventoryItem_AndRejectedServiceConsumesNothing()
        {
            var inventory = new Inventory.Inventory();
            inventory.AddById(StartingLevelSystem.AirFilterServiceItemId, 1);
            var system = new StartingLevelSystem();
            system.BindMaintenance(inventory, _ => false);
            system.TickDay();

            var preview = system.PreviewMaintainAirFilter();
            Assert.True(preview.IsAvailable);
            var result = system.MaintainAirFilter();

            Assert.True(result.IsSuccess);
            Assert.Equal(0, inventory.CountById(StartingLevelSystem.AirFilterServiceItemId));
            Assert.Equal(100f, system.State.airFilterHealthPercent);

            float healthBeforeRejected = system.State.airFilterHealthPercent;
            var rejected = system.MaintainAirFilter();
            Assert.False(rejected.IsSuccess);
            Assert.Equal("maintenance_not_needed", rejected.FailureCode);
            Assert.Equal(healthBeforeRejected, system.State.airFilterHealthPercent);
            Assert.Equal(0, inventory.CountById(StartingLevelSystem.AirFilterServiceItemId));

            var missingItemSystem = new StartingLevelSystem();
            missingItemSystem.BindMaintenance(new Inventory.Inventory(), _ => false);
            missingItemSystem.TickDay();
            var missingItem = missingItemSystem.MaintainAirFilter();
            Assert.False(missingItem.IsSuccess);
            Assert.Equal("missing_maintenance_item", missingItem.FailureCode);
        }

        [Fact]
        public void Replacement_RequiresSharedResearchCapability_AndConsumesHepaCoreAtomically()
        {
            var inventory = new Inventory.Inventory();
            inventory.AddById(StartingLevelSystem.AirFilterReplacementItemId, 1);
            var system = new StartingLevelSystem();
            system.BindMaintenance(inventory, _ => false);
            system.TickDay();

            var locked = system.PreviewMaintainAirFilter(replace: true);
            Assert.False(locked.IsAvailable);
            Assert.Equal("research_required", locked.FailureCode);
            Assert.Equal(1, inventory.CountById(StartingLevelSystem.AirFilterReplacementItemId));

            system.BindMaintenance(inventory, id => id == StartingLevelSystem.AirFilterResearchId);
            var result = system.MaintainAirFilter(replace: true);

            Assert.True(result.IsSuccess);
            Assert.Equal(0, inventory.CountById(StartingLevelSystem.AirFilterReplacementItemId));
            Assert.Equal(100f, system.State.airFilterHealthPercent);
            Assert.False(system.State.airHazardWarning);
        }

        [Fact]
        public void AirFiltrationResearch_ChangesOnlyOwningDegradationRate_AndExtendsReferenceLifeByFiftyPercent()
        {
            var baseline = new StartingLevelSystem();
            for (int day = 0; day < 20; day++) baseline.TickDay();
            Assert.Equal(0f, baseline.State.airFilterHealthPercent);

            var researched = new StartingLevelSystem();
            researched.BindMaintenance(new Inventory.Inventory(), id => id == StartingLevelSystem.AirFilterResearchId);
            for (int day = 0; day < 29; day++) researched.TickDay();
            Assert.InRange(researched.State.airFilterHealthPercent, 0f, 5f);
            Assert.True(researched.State.airFilterHealthPercent > 0f);
            researched.TickDay();
            Assert.Equal(0f, researched.State.airFilterHealthPercent);
        }
    }
}
