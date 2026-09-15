// SPDX-License-Identifier: MIT
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Current craft-attribution contract. Crafting owns recipe completion;
    /// Phase0HostSession supplies the survivor, profession, and result item to
    /// TradeSpecialtySystem.OnItemCrafted. The removed CraftContext bridge was
    /// not part of the current Core API and must not be recreated by tests.
    /// </summary>
    public sealed class CraftAttributionTradeSpecialtyTests
    {
        private const string CrafterElena = "survivor_elena_vasquez";
        private const string CrafterMikhail = "survivor_mikhail";

        [Fact]
        public void AttributedCrafts_AdvanceMatchingCurrentProfession()
        {
            var specialty = new TradeSpecialtySystem();
            int milestoneCount = 0;
            specialty.OnSpecialtyMilestone += (survivorId, _, _) =>
            {
                if (survivorId == CrafterElena) milestoneCount++;
            };

            specialty.OnItemCrafted(CrafterElena, "machinist", "wrench_standard");
            specialty.OnItemCrafted(CrafterElena, "machinist", "gear_standard");

            Assert.Equal(2, specialty.GetMasteryTier(CrafterElena));
            Assert.Equal(2, milestoneCount);
            Assert.False(specialty.HasMasteredTrade(CrafterElena));
        }

        [Fact]
        public void AttributedCrafts_IgnoreWrongItemUnknownProfessionAndMissingIdentity()
        {
            var specialty = new TradeSpecialtySystem();
            bool eventFired = false;
            specialty.OnSpecialtyMilestone += (_, _, _) => eventFired = true;

            specialty.OnItemCrafted(CrafterElena, "machinist", "bandage_clean");
            specialty.OnItemCrafted(CrafterMikhail, "unknown_drifter", "wrench_standard");
            specialty.OnItemCrafted(string.Empty, "machinist", "wrench_standard");
            specialty.OnItemCrafted(CrafterElena, string.Empty, "wrench_standard");

            Assert.Equal(0, specialty.GetMasteryTier(CrafterElena));
            Assert.Equal(0, specialty.GetMasteryTier(CrafterMikhail));
            Assert.False(eventFired);
        }

        [Fact]
        public void AttributedCrafts_DeduplicateTheSameMilestone()
        {
            var specialty = new TradeSpecialtySystem();

            specialty.OnItemCrafted(CrafterElena, "machinist", "wrench_standard");
            specialty.OnItemCrafted(CrafterElena, "machinist", "wrench_standard");

            Assert.Equal(1, specialty.GetMasteryTier(CrafterElena));
            Assert.False(specialty.HasMasteredTrade(CrafterElena));
        }

        [Fact]
        public void AttributedCrafts_SaveRestorePreservesProgressAndCanMaster()
        {
            var specialty = new TradeSpecialtySystem();
            specialty.OnItemCrafted(CrafterElena, "machinist", "wrench_standard");
            specialty.OnItemCrafted(CrafterElena, "machinist", "gear_standard");

            var restored = new TradeSpecialtySystem();
            restored.RestoreState(specialty.CaptureState());

            Assert.Equal(2, restored.GetMasteryTier(CrafterElena));
            restored.OnItemCrafted(CrafterElena, "machinist", "spring_standard");
            Assert.Equal(3, restored.GetMasteryTier(CrafterElena));
            Assert.True(restored.HasMasteredTrade(CrafterElena));
        }
    }
}
