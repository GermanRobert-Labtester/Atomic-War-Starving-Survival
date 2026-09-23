// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Ashfall.Core.Propaganda;
using Xunit;

namespace Ashfall.Core.Tests.Propaganda
{
    public sealed class Plan168PropagandaIntegrationTests
    {
        [Fact]
        public void MessageCreation_AdjustsQualityByTruthfulness_AndEmitsEvent()
        {
            var system = new PropagandaSystem();
            PropagandaMessage? emitted = null;
            system.OnMessageCreated += m => emitted = m;

            var msgTruth = system.CreateMessage(
                authorId: "surv_scholar",
                medium: PropagandaMedium.RadioBroadcast,
                truthfulness: MessageTruthfulness.Truth,
                theme: PropagandaTheme.Hope,
                targetFactionId: "faction_coalition",
                content: "Open borders for medical treatment.",
                authorSkill: 80f,
                currentDay: 1
            );

            Assert.NotNull(msgTruth);
            Assert.Equal(emitted, msgTruth);
            Assert.Equal(75f, msgTruth.Quality); // 80 - 5 for truth

            var msgLie = system.CreateMessage(
                authorId: "surv_scholar",
                medium: PropagandaMedium.Leaflet,
                truthfulness: MessageTruthfulness.Lie,
                theme: PropagandaTheme.Triumph,
                targetFactionId: "faction_coalition",
                content: "Enemy general has fled.",
                authorSkill: 80f,
                currentDay: 1
            );

            Assert.Equal(90f, msgLie.Quality); // 80 + 10 for lie
            Assert.Equal(2, system.MessageCount);
        }

        [Fact]
        public void CampaignLifecycle_AccumulatesEffectiveness_AndAppliesFactionMoraleImpact()
        {
            var system = new PropagandaSystem();
            var msg = system.CreateMessage("surv_1", PropagandaMedium.RadioBroadcast, MessageTruthfulness.Truth, PropagandaTheme.Unity, "faction_raiders", "Peace is possible.");

            var campaign = system.LaunchCampaign(
                campaignName: "Operation Peace",
                targetFactionId: "faction_raiders",
                objective: PropagandaObjective.UndermineFaction,
                messageIds: new[] { msg.MessageId },
                durationDays: 3,
                currentDay: 1
            );

            Assert.Equal(CampaignStatus.Active, campaign.Status);
            Assert.Equal(1, system.ActiveCampaignCount);

            // Tick 3 days without detection
            system.TickDay(2, randomRoll: 0.99f);
            Assert.True(campaign.AccumulatedEffectiveness > 0f);

            system.TickDay(3, randomRoll: 0.99f);
            system.TickDay(4, randomRoll: 0.99f);

            Assert.Equal(CampaignStatus.Completed, campaign.Status);
            Assert.Equal(0, system.ActiveCampaignCount);

            // UndermineFaction causes negative morale impact
            float impact = system.GetFactionMoraleImpact("faction_raiders");
            Assert.True(impact < 0f);
        }

        [Fact]
        public void CompromisedCampaign_WithLies_SeverelyDamagesShelterCredibility()
        {
            var system = new PropagandaSystem();
            var msg = system.CreateMessage("surv_agent", PropagandaMedium.WallPosting, MessageTruthfulness.Lie, PropagandaTheme.Fear, "faction_mercs", "Plague in your camp.");

            var campaign = system.LaunchCampaign(
                campaignName: "Disinformation Wave",
                targetFactionId: "faction_mercs",
                objective: PropagandaObjective.DestabilizeRegion,
                messageIds: new[] { msg.MessageId },
                durationDays: 4,
                currentDay: 1
            );

            float initialCredibility = system.ShelterCredibility;

            // Low roll triggers detection
            system.TickDay(2, randomRoll: 0.01f);

            Assert.True(campaign.WasDetected);
            Assert.Equal(CampaignStatus.Compromised, campaign.Status);
            // Lie penalty is 20f
            Assert.Equal(initialCredibility - 20f, system.ShelterCredibility);
        }

        [Fact]
        public void StateSerialization_RoundTrips_MessagesCampaignsAndMoraleImpacts()
        {
            var original = new PropagandaSystem();
            var m1 = original.CreateMessage("surv_1", PropagandaMedium.RadioBroadcast, MessageTruthfulness.Truth, PropagandaTheme.Hope, "faction_settlers", "Food available.", 70f, "Civilians", 1);
            var m2 = original.CreateMessage("surv_2", PropagandaMedium.Leaflet, MessageTruthfulness.HalfTruth, PropagandaTheme.Unity, "faction_militia", "Alliance offer.", 60f, "Soldiers", 2);

            var c1 = original.LaunchCampaign("Op Alliance", "faction_militia", PropagandaObjective.BoostMorale, new[] { m2.MessageId }, 5, 2);
            original.TickDay(3, 0.99f);

            var captured = original.CaptureState();
            string json = JsonSerializer.Serialize(captured);
            var restoredState = JsonSerializer.Deserialize<PropagandaState>(json);

            Assert.NotNull(restoredState);
            var restored = new PropagandaSystem(restoredState);

            Assert.Equal(original.ShelterCredibility, restored.ShelterCredibility);
            Assert.Equal(original.MessageCount, restored.MessageCount);
            Assert.Equal(original.ActiveCampaignCount, restored.ActiveCampaignCount);

            var restoredCmp = restored.Campaigns.First(c => c.CampaignId == c1.CampaignId);
            Assert.Equal(c1.AccumulatedEffectiveness, restoredCmp.AccumulatedEffectiveness);
            Assert.Equal(c1.Status, restoredCmp.Status);
        }

        [Fact]
        public void BroadcastMediums_YieldHigherDailyEffectiveness_ThanPrintMediums()
        {
            var systemRadio = new PropagandaSystem();
            var msgRadio = systemRadio.CreateMessage("surv_1", PropagandaMedium.RadioBroadcast, MessageTruthfulness.Truth, PropagandaTheme.Hope, "faction_target", "Message", 70f);
            var cmpRadio = systemRadio.LaunchCampaign("Radio Op", "faction_target", PropagandaObjective.BoostMorale, new[] { msgRadio.MessageId }, 3, 1);
            systemRadio.TickDay(2, 0.99f);

            var systemPrint = new PropagandaSystem();
            var msgPrint = systemPrint.CreateMessage("surv_1", PropagandaMedium.WallPosting, MessageTruthfulness.Truth, PropagandaTheme.Hope, "faction_target", "Message", 70f);
            var cmpPrint = systemPrint.LaunchCampaign("Print Op", "faction_target", PropagandaObjective.BoostMorale, new[] { msgPrint.MessageId }, 3, 1);
            systemPrint.TickDay(2, 0.99f);

            Assert.True(cmpRadio.AccumulatedEffectiveness > cmpPrint.AccumulatedEffectiveness);
        }

        [Fact]
        public void StartCampaignFromTemplate_CreatesCampaign_WithTemplateParameters()
        {
            var system = new PropagandaSystem();
            system.RegisterTemplate(new PropagandaTemplateDef
            {
                id = "psyops_campaign_scavenger_lantern_hour",
                display_name = "Scavenger Lantern Hour",
                description = "Evening broadcasts for wasteland scavengers.",
                target_faction_id = "faction_drifters",
                objective = "BoostMorale",
                preferred_medium = "RadioBroadcast",
                duration_days = 4,
                base_effectiveness = 25f,
                suggested_theme = "Hope"
            });

            bool started = system.StartCampaignFromTemplate("psyops_campaign_scavenger_lantern_hour", 1);
            Assert.True(started);
            Assert.Equal(1, system.ActiveCampaignCount);

            var campaign = system.Campaigns[0];
            Assert.Equal("Scavenger Lantern Hour", campaign.CampaignName);
            Assert.Equal("faction_drifters", campaign.TargetFactionId);
            Assert.Equal(PropagandaObjective.BoostMorale, campaign.Objective);
            Assert.Equal(4, campaign.DurationDays);
            Assert.Equal(25f, campaign.AccumulatedEffectiveness);
        }

        [Fact]
        public void AuthoredData_PropagandaTemplatesJson_LoadsSuccessfully()
        {
            var dataDir = System.IO.Path.GetFullPath(
                System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory,
                    "..", "..", "..", "..", "Assets", "StreamingAssets", "Data"));
            if (!System.IO.File.Exists(System.IO.Path.Combine(dataDir, "propaganda_templates.json")))
                dataDir = System.IO.Path.GetFullPath("Assets/StreamingAssets/Data");

            string filePath = System.IO.Path.Combine(dataDir, "propaganda_templates.json");
            Assert.True(System.IO.File.Exists(filePath), $"File not found: {filePath}");

            string json = System.IO.File.ReadAllText(filePath);
            var catalog = JsonSerializer.Deserialize<PropagandaTemplateCatalogData>(json);
            Assert.NotNull(catalog);
            Assert.True(catalog!.templates.Count >= 4);

            var system = new PropagandaSystem();
            system.LoadTemplates(catalog.templates);
            Assert.True(system.Templates.Count >= 4);

            var lantern = system.GetTemplate("psyops_campaign_scavenger_lantern_hour");
            Assert.NotNull(lantern);
            Assert.Equal("Scavenger Lantern Hour", lantern!.display_name);
        }
    }
}
