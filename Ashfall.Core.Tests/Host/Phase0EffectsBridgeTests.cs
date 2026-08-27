// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Medical;
using Ashfall.Core.Phantoms;
using Ashfall.Core.Radiation;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class Phase0EffectsBridgeTests
    {
        [Fact]
        public void PhantomMemory_Motivation_BoostsWorkSpeedAndDecays()
        {
            var phantom = new PhantomMemoryEngine();
            phantom.RegisterRule("former_soldier", "military", 1.0f, "test", "Motivated", "Breakdown");

            var sv = new PhantomSurvivorSnapshot
            {
                survivorId = "sv_soldier",
                displayName = "Soldier",
                backgroundId = "former_soldier",
                isAlive = true
            };

            var rng = new SeededRng(12345);
            var outcome = phantom.OnItemScavenged(sv, "item_dog_tags", rng);

            Assert.Equal(TriggerOutcome.Motivation, outcome);
            float workMult = phantom.GetWorkEfficiencyMultiplier("sv_soldier");
            Assert.Equal(1f + PhantomMemoryEngine.MotivationWorkSpeedBonus, workMult);

            // Tick past motivation duration (8h)
            phantom.TickHour("sv_soldier", 9f);
            Assert.Equal(1f, phantom.GetWorkEfficiencyMultiplier("sv_soldier"));
        }

        [Fact]
        public void TradeSpecialty_CraftingItems_AdvancesTierAndMasters()
        {
            var specialty = new TradeSpecialtySystem
            {
                GetNarrativeEventId = prof => $"narrative_trade_mastery_{prof}"
            };
            int narrativeFired = 0;
            string lastNarrativeId = null!;

            specialty.FireNarrativeEvent = (id, sv) =>
            {
                narrativeFired++;
                lastNarrativeId = id;
            };

            specialty.OnItemCrafted("elena_vasquez", "machinist", "wrench_standard");
            specialty.OnItemCrafted("elena_vasquez", "machinist", "gear_standard");
            Assert.Equal(2, specialty.GetMasteryTier("elena_vasquez"));

            specialty.OnItemCrafted("elena_vasquez", "machinist", "lever_standard");
            Assert.True(specialty.HasMasteredTrade("elena_vasquez"));
            Assert.Equal(1, narrativeFired);
            Assert.Equal("narrative_trade_mastery_machinist", lastNarrativeId);
        }

        [Fact]
        public void FinalWish_CompletedWish_GrantsPermanentShelterMoraleBuff()
        {
            float shelterMoraleDelta = 0f;
            var finalWish = new FinalWishSystem
            {
                Rng = new SeededRng(42),
                ApplyPermanentShelterMoraleBuff = delta => shelterMoraleDelta += delta
            };

            finalWish.RegisterWish("parent", FinalWishSystem.WishBuildMemorial);
            finalWish.DeclareTerminalPrognosis("survivor_parent", "parent", true);

            Assert.True(finalWish.HasActiveWish("survivor_parent"));
            finalWish.AdvanceWishStep("survivor_parent", "find_keepsake");
            finalWish.AdvanceWishStep("survivor_parent", "build_shrine");
            finalWish.AdvanceWishStep("survivor_parent", "inscribe_names");

            Assert.True(finalWish.HasCompletedWish("survivor_parent"));
            Assert.True(shelterMoraleDelta > 0f);
        }

        [Fact]
        public void GuiltInsomnia_RecordedGuilt_RaisesInsomniaSeverity()
        {
            var guilt = new GuiltInsomniaSystem();
            guilt.RecordGuilt("sv_scout", "abandon_refugees", 0.8f, currentDay: 1);

            float severity = guilt.GetInsomniaSeverity("sv_scout");
            Assert.True(severity > 0f);

            // Tick 24 hours
            guilt.Tick("sv_scout", 24f, currentDay: 2);
            float severityDay2 = guilt.GetInsomniaSeverity("sv_scout");
            Assert.True(severityDay2 > 0f);
        }

        [Fact]
        public void RespiratoryDegeneration_AshZoneExposure_ReducesStamina()
        {
            bool inAshZone = true;
            var respiratory = new RespiratoryDegenerationSystem
            {
                GetFilterHealth = () => 0f,
                IsInFalloutStorm = () => false,
                IsInAshZone = () => inAshZone
            };

            // Ash zone accumulates 0.25f per hour; 205 hours = 51.25f >= 50f (SevereCoughThreshold)
            respiratory.TickHours("sv_explorer", 205f);

            float stamina = respiratory.GetStaminaMultiplier("sv_explorer");
            Assert.True(stamina < 1f, "Ash zone exposure must reduce stamina multiplier");
        }
    }
}
