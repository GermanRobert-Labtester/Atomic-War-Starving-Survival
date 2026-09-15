// SPDX-License-Identifier: MIT
using Ashfall.Core.Inventory;
using Ashfall.Core.StartingLevel;
using Xunit;

namespace Ashfall.Core.Tests.Shelter
{
    /// <summary>
    /// Plan 52 campaign-scale characterization. The test deliberately uses the
    /// real StartingLevel tick owner and the real inventory-backed maintenance
    /// command; it does not call degradation internals or invent shelter-local
    /// resource state.
    /// </summary>
    public sealed class StartingLevelMaintenance30DayTests
    {
        [Fact]
        public void CompetentThirtyDayPath_IsMaintainable_ButConsumesRealParts()
        {
            var inventory = new Ashfall.Core.Inventory.Inventory { Capacity = 40, MaxWeight = 200f };
            inventory.AddById(StartingLevelSystem.AirFilterServiceItemId, 8);
            var shelter = new StartingLevelSystem();
            shelter.BindMaintenance(inventory, _ => false);

            int firstWarningDay = -1;
            int maintenanceCount = 0;
            for (int day = 0; day < 30; day++)
            {
                shelter.TickDay();
                if (shelter.State.airHazardWarning && firstWarningDay < 0)
                    firstWarningDay = shelter.State.day;

                if (shelter.AirFilterConditionBand == "critical")
                {
                    var result = shelter.MaintainAirFilter();
                    Assert.True(result.IsSuccess, result.FailureCode);
                    maintenanceCount++;
                }
            }

            Assert.Equal(12, firstWarningDay);
            Assert.Equal(4, maintenanceCount);
            Assert.Equal(4, inventory.CountById(StartingLevelSystem.AirFilterServiceItemId));
            Assert.InRange(shelter.State.airFilterHealthPercent, 50f, 100f);
            Assert.NotEqual("failed", shelter.AirFilterConditionBand);
        }

        [Fact]
        public void IgnoringMaintenance_ReachesFailureOnlyAfterActionableWarning()
        {
            var shelter = new StartingLevelSystem();
            int firstWarningDay = -1;
            int firstFailureDay = -1;

            for (int day = 0; day < 30; day++)
            {
                shelter.TickDay();
                if (shelter.State.airHazardWarning && firstWarningDay < 0)
                    firstWarningDay = shelter.State.day;
                if (shelter.AirFilterConditionBand == "failed" && firstFailureDay < 0)
                    firstFailureDay = shelter.State.day;
            }

            Assert.Equal(12, firstWarningDay);
            Assert.Equal(21, firstFailureDay);
            Assert.True(firstWarningDay < firstFailureDay);
        }

        [Fact]
        public void ResearchCapability_ExtendsThirtyDayReferenceLife_WithoutShelterShadowState()
        {
            var baseline = new StartingLevelSystem();
            for (int day = 0; day < 20; day++) baseline.TickDay();

            var researched = new StartingLevelSystem();
                researched.BindMaintenance(new Ashfall.Core.Inventory.Inventory { Capacity = 40, MaxWeight = 200f },
                id => id == StartingLevelSystem.AirFilterResearchId);
            for (int day = 0; day < 30; day++) researched.TickDay();

            Assert.Equal(0f, baseline.State.airFilterHealthPercent);
            Assert.Equal(0f, researched.State.airFilterHealthPercent);
            Assert.Equal("knowledge_air_filtration", StartingLevelSystem.AirFilterResearchId);
            Assert.True(researched.State.journalDirectives.Count >= baseline.State.journalDirectives.Count);
        }
    }
}
