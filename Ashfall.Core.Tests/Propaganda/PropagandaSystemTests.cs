// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Propaganda;
using Xunit;

namespace Ashfall.Core.Tests.Propaganda
{
    public sealed class PropagandaSystemTests
    {
        [Fact]
        public void CreateMessage_SetsPropertiesAndEmitsEvent()
        {
            var system = new PropagandaSystem();
            PropagandaMessage? emitted = null;
            system.OnMessageCreated += m => emitted = m;

            var msg = system.CreateMessage(
                authorId: "surv_writer",
                medium: PropagandaMedium.RadioBroadcast,
                truthfulness: MessageTruthfulness.Truth,
                theme: PropagandaTheme.Hope,
                targetFactionId: "faction_rust_claws",
                content: "We offer medical aid to all seekers.",
                authorSkill: 70f,
                targetAudience: "Civilians",
                currentDay: 2
            );

            Assert.NotNull(msg);
            Assert.Equal("surv_writer", msg.AuthorId);
            Assert.Equal(PropagandaMedium.RadioBroadcast, msg.Medium);
            Assert.Equal(MessageTruthfulness.Truth, msg.Truthfulness);
            Assert.Equal(PropagandaTheme.Hope, msg.Theme);
            Assert.Equal("faction_rust_claws", msg.TargetFactionId);
            Assert.Equal(msg, emitted);
            Assert.Equal(1, system.MessageCount);
        }

        [Fact]
        public void LaunchCampaign_StartsActiveCampaign()
        {
            var system = new PropagandaSystem();
            var msg = system.CreateMessage("surv_1", PropagandaMedium.Leaflet, MessageTruthfulness.HalfTruth, PropagandaTheme.Unity, "faction_militia", "Stand together.");

            PropagandaCampaign? started = null;
            system.OnCampaignStarted += c => started = c;

            var cmp = system.LaunchCampaign(
                campaignName: "Operation Unity",
                targetFactionId: "faction_militia",
                objective: PropagandaObjective.BoostMorale,
                messageIds: new[] { msg.MessageId },
                durationDays: 4,
                currentDay: 1
            );

            Assert.NotNull(cmp);
            Assert.Equal("Operation Unity", cmp.CampaignName);
            Assert.Equal(CampaignStatus.Active, cmp.Status);
            Assert.Equal(cmp, started);
            Assert.Equal(1, system.ActiveCampaignCount);
        }

        [Fact]
        public void TickDay_ProgressesCampaignEffectiveness()
        {
            var system = new PropagandaSystem();
            var msg = system.CreateMessage("surv_1", PropagandaMedium.RadioBroadcast, MessageTruthfulness.Truth, PropagandaTheme.Hope, "faction_drifters", "Welcome.");
            var cmp = system.LaunchCampaign("Voice of Hope", "faction_drifters", PropagandaObjective.BoostMorale, new[] { msg.MessageId }, durationDays: 5, currentDay: 1);

            float progressedDelta = 0f;
            system.OnCampaignProgressed += (c, d) => progressedDelta = d;

            system.TickDay(currentDay: 2, randomRoll: 0.99f); // roll high to avoid detection

            Assert.True(progressedDelta > 0f);
            Assert.True(cmp.AccumulatedEffectiveness > 0f);
            Assert.Equal(CampaignStatus.Active, cmp.Status);
        }

        [Fact]
        public void TickDay_CompromisesCampaign_WhenDetected()
        {
            var system = new PropagandaSystem();
            var msg = system.CreateMessage("surv_1", PropagandaMedium.WallPosting, MessageTruthfulness.Lie, PropagandaTheme.Division, "faction_drifters", "False rumors.");
            var cmp = system.LaunchCampaign("Sow Doubt", "faction_drifters", PropagandaObjective.UndermineFaction, new[] { msg.MessageId }, durationDays: 5, currentDay: 1);

            PropagandaCampaign? compromised = null;
            system.OnCampaignCompromised += c => compromised = c;

            // Wall posting detection risk is 0.35f, roll 0.05f triggers detection
            system.TickDay(currentDay: 2, randomRoll: 0.05f);

            Assert.True(cmp.WasDetected);
            Assert.Equal(CampaignStatus.Compromised, cmp.Status);
            Assert.Equal(cmp, compromised);
            // Credibility reduced because lies were used
            Assert.True(system.ShelterCredibility < 75f);
        }

        [Fact]
        public void TickDay_CompletesCampaign_AndAppliesFactionMoraleImpact()
        {
            var system = new PropagandaSystem();
            var msg = system.CreateMessage("surv_1", PropagandaMedium.RadioBroadcast, MessageTruthfulness.Truth, PropagandaTheme.Fear, "faction_raiders", "Surrender.", authorSkill: 80f);
            var cmp = system.LaunchCampaign("Radio Deterrence", "faction_raiders", PropagandaObjective.UndermineFaction, new[] { msg.MessageId }, durationDays: 2, currentDay: 1);

            PropagandaCampaign? completed = null;
            system.OnCampaignCompleted += c => completed = c;

            // Day 2 (elapsed 1)
            system.TickDay(currentDay: 2, randomRoll: 0.99f);
            // Day 3 (elapsed 2 >= durationDays 2)
            system.TickDay(currentDay: 3, randomRoll: 0.99f);

            Assert.Equal(CampaignStatus.Completed, cmp.Status);
            Assert.Equal(cmp, completed);
            // UndermineFaction causes negative morale impact
            float moraleImpact = system.GetFactionMoraleImpact("faction_raiders");
            Assert.True(moraleImpact < 0f);
        }

        [Fact]
        public void CaptureAndRestoreState_RoundTripsCorrectly()
        {
            var system1 = new PropagandaSystem();
            var msg = system1.CreateMessage("surv_lead", PropagandaMedium.Leaflet, MessageTruthfulness.Truth, PropagandaTheme.Sacrifice, "faction_allies", "Remember.");
            var cmp = system1.LaunchCampaign("Memorial", "faction_allies", PropagandaObjective.BoostMorale, new[] { msg.MessageId }, 3, 1);
            system1.TickDay(2, 0.99f);

            var state = system1.CaptureState();

            var system2 = new PropagandaSystem();
            system2.RestoreState(state);

            Assert.Equal(1, system2.MessageCount);
            Assert.Equal(1, system2.ActiveCampaignCount);
            Assert.Equal(system1.ShelterCredibility, system2.ShelterCredibility);
            var restoredMsg = state.Messages.First();
            Assert.Equal("surv_lead", restoredMsg.AuthorId);
            Assert.Equal(PropagandaTheme.Sacrifice, restoredMsg.Theme);
        }
    }
}
