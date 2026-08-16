using System;
using Ashfall.Core;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class TradeSpecialtySystemTests
    {
        private const string SvA = "sv_alpha";

        // ── 1. Milestone progression ──────────────────────────────────

        [Fact]
        public void OnItemCrafted_MatchingCategory_CountsMilestone()
        {
            var sys = new TradeSpecialtySystem();
            int milestoneFired = 0;
            sys.OnSpecialtyMilestone += (_, _, _) => milestoneFired++;

            sys.OnItemCrafted(SvA, "machinist", "wrench_standard");

            Assert.Equal(1, sys.GetMasteryTier(SvA));
            Assert.Equal(1, milestoneFired);
            Assert.False(sys.HasMasteredTrade(SvA));
        }

        [Fact]
        public void OnItemCrafted_NonMatchingCategory_Ignored()
        {
            var sys = new TradeSpecialtySystem();
            sys.OnItemCrafted(SvA, "machinist", "bandage_clean");
            Assert.Equal(0, sys.GetMasteryTier(SvA));
        }

        [Fact]
        public void OnItemCrafted_UnknownProfession_Ignored()
        {
            var sys = new TradeSpecialtySystem();
            sys.OnItemCrafted(SvA, "unlisted_profession", "wrench_standard");
            Assert.Equal(0, sys.GetMasteryTier(SvA));
        }

        [Fact]
        public void OnItemCrafted_DuplicateItem_NotDoubleCounted()
        {
            var sys = new TradeSpecialtySystem();
            sys.OnItemCrafted(SvA, "teacher", "book_childrens");
            sys.OnItemCrafted(SvA, "teacher", "book_childrens");
            Assert.Equal(1, sys.GetMasteryTier(SvA));
        }

        [Fact]
        public void OnItemCrafted_EmptyInputs_NoOp()
        {
            var sys = new TradeSpecialtySystem();
            sys.OnItemCrafted("", "machinist", "wrench_standard");
            sys.OnItemCrafted(SvA, "", "wrench_standard");
            sys.OnItemCrafted(SvA, "machinist", "");
            Assert.Equal(0, sys.GetMasteryTier(SvA));
        }

        // ── 2. Mastery ────────────────────────────────────────────────

        [Fact]
        public void ThreeMilestones_MastersTrade()
        {
            var sys = new TradeSpecialtySystem();
            sys.OnItemCrafted(SvA, "nurse", "bandage_clean");
            sys.OnItemCrafted(SvA, "nurse", "splint_basic");
            sys.OnItemCrafted(SvA, "nurse", "antiseptic_bottle");

            Assert.Equal(TradeSpecialtySystem.MilestonesToMaster, sys.GetMasteryTier(SvA));
            Assert.True(sys.HasMasteredTrade(SvA));
        }

        [Fact]
        public void MasterTrade_AppliesFullSkillBonusAndMorale()
        {
            var sys = new TradeSpecialtySystem();
            float skillBonusTotal = 0f;
            float moraleDelta = 0f;
            int masteredFired = 0;
            sys.GrantSkillBonus = (_, _, bonus) => skillBonusTotal += bonus;
            sys.ApplyMoraleDelta = (_, delta) => moraleDelta += delta;
            sys.OnSpecialtyMastered += (_, _) => masteredFired++;

            sys.OnItemCrafted(SvA, "electrician", "wire_copper");
            sys.OnItemCrafted(SvA, "electrician", "battery_car");
            sys.OnItemCrafted(SvA, "electrician", "generator_small");

            Assert.Equal(1, masteredFired);
            // 2 intermediate milestones (×0.3 each) + full mastery bonus.
            float expectedTotal = TradeSpecialtySystem.MasterySkillBonus
                + 2f * TradeSpecialtySystem.MasterySkillBonus * TradeSpecialtySystem.MilestoneSkillBonusFactor;
            Assert.Equal(expectedTotal, skillBonusTotal, 4);
            Assert.Equal(TradeSpecialtySystem.MasteryMoraleBonus, moraleDelta, 4);
        }

        [Fact]
        public void IntermediateMilestone_GrantsPartialSkillBonus()
        {
            var sys = new TradeSpecialtySystem();
            float skillBonusTotal = 0f;
            sys.GrantSkillBonus = (_, _, bonus) => skillBonusTotal += bonus;

            sys.OnItemCrafted(SvA, "machinist", "wrench_standard");

            Assert.Equal(TradeSpecialtySystem.MasterySkillBonus * TradeSpecialtySystem.MilestoneSkillBonusFactor,
                skillBonusTotal, 4);
        }

        [Fact]
        public void MasterTrade_FiresNarrativeEvent()
        {
            var sys = new TradeSpecialtySystem();
            string firedNarrative = null;
            string firedSurvivor = null;
            sys.GetNarrativeEventId = prof => $"narrative_trade_mastery_{prof}";
            sys.FireNarrativeEvent = (id, sv) => { firedNarrative = id; firedSurvivor = sv; };

            sys.OnItemCrafted(SvA, "teacher", "book_childrens");
            sys.OnItemCrafted(SvA, "teacher", "chalk_piece");
            sys.OnItemCrafted(SvA, "teacher", "slate_small");

            Assert.Equal("narrative_trade_mastery_teacher", firedNarrative);
            Assert.Equal(SvA, firedSurvivor);
        }

        [Fact]
        public void MasteredTrade_StopsFurtherMilestones()
        {
            var sys = new TradeSpecialtySystem();
            sys.OnItemCrafted(SvA, "machinist", "wrench_standard");
            sys.OnItemCrafted(SvA, "machinist", "gear_standard");
            sys.OnItemCrafted(SvA, "machinist", "lever_standard");
            sys.OnItemCrafted(SvA, "machinist", "blade_standard");

            Assert.Equal(TradeSpecialtySystem.MilestonesToMaster, sys.GetMasteryTier(SvA));
        }

        // ── 3. Save / Load ────────────────────────────────────────────

        [Fact]
        public void CaptureRestore_RoundTripsState()
        {
            var sys = new TradeSpecialtySystem();
            sys.OnItemCrafted(SvA, "nurse", "bandage_clean");
            sys.OnItemCrafted(SvA, "nurse", "splint_basic");

            var save = sys.CaptureState();
            var fresh = new TradeSpecialtySystem();
            fresh.RestoreState(save);

            Assert.Equal(2, fresh.GetMasteryTier(SvA));
        }

        [Fact]
        public void Restore_Null_NoThrow()
        {
            var sys = new TradeSpecialtySystem();
            sys.RestoreState(null);
            Assert.Equal(0, sys.GetMasteryTier(SvA));
        }

        [Fact]
        public void Restore_RebuildsMasteryFlag()
        {
            var sys = new TradeSpecialtySystem();
            sys.OnItemCrafted(SvA, "nurse", "bandage_clean");
            sys.OnItemCrafted(SvA, "nurse", "splint_basic");
            sys.OnItemCrafted(SvA, "nurse", "antiseptic_bottle");

            var save = sys.CaptureState();
            var fresh = new TradeSpecialtySystem();
            fresh.RestoreState(save);

            Assert.True(fresh.HasMasteredTrade(SvA));
        }
    }
}
