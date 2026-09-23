// SPDX-License-Identifier: MIT
using System.IO;
using System.Linq;
using Ashfall.Core.Communication;
using Xunit;

namespace Ashfall.Core.Tests.Communication
{
    public sealed class Plan211InternalCommunicationIntegrationTests
    {
        [Fact]
        public void LoadCatalog_LoadsAllTemplates_FromValidJson()
        {
            var comms = new InternalCommunicationSystem();
            string path = Path.Combine("..", "..", "..", "..", "Assets", "StreamingAssets", "Data", "communication_templates.json");
            if (!File.Exists(path))
            {
                path = Path.Combine("Assets", "StreamingAssets", "Data", "communication_templates.json");
            }
            Assert.True(File.Exists(path), $"Catalog not found at {path}");

            string json = File.ReadAllText(path);
            comms.LoadCatalog(json);

            Assert.Equal(7, comms.Templates.Count);
            var waterWarning = comms.GetTemplate("comm_tpl_water_rationing_notice");
            Assert.NotNull(waterWarning);
            Assert.Equal("Water Consumption Advisory", waterWarning.Title);
            Assert.Equal(CommunicationCategory.Warning, waterWarning.ParseCategory());
            Assert.Equal(MessagePriority.High, waterWarning.ParsePriority());
            Assert.Equal(5, waterWarning.DurationDays);

            var harvest = comms.GetTemplate("comm_tpl_shelter_harvest_celebration");
            Assert.NotNull(harvest);
            Assert.Equal(CommunicationCategory.Celebration, harvest.ParseCategory());
        }

        [Fact]
        public void PostMessage_And_GetPublicNotices_FiltersCorrectly()
        {
            var comms = new InternalCommunicationSystem();

            CommunicationMessage? postedEvent = null;
            comms.OnMessagePosted += m => postedEvent = m;

            var publicMsg = comms.PostMessage(
                authorId: "survivor_alpha",
                category: CommunicationCategory.Notice,
                subject: "Lost Wrench",
                content: "Left in workshop near workbench 2.",
                recipientId: null,
                priority: MessagePriority.Normal,
                currentDay: 3,
                durationDays: 5);

            var privateMsg = comms.PostMessage(
                authorId: "survivor_beta",
                category: CommunicationCategory.PersonalMail,
                subject: "Medical Checkup",
                content: "Come to clinic this afternoon.",
                recipientId: "survivor_gamma",
                priority: MessagePriority.Normal,
                currentDay: 3,
                durationDays: 2,
                channel: CommunicationChannel.PersonalMail);

            Assert.NotNull(publicMsg);
            Assert.NotNull(privateMsg);
            Assert.Equal(postedEvent, privateMsg);

            var publicList = comms.GetPublicNotices();
            Assert.Single(publicList);
            Assert.Equal("Lost Wrench", publicList[0].Subject);

            var gammaInbox = comms.GetMessagesForRecipient("survivor_gamma");
            Assert.Single(gammaInbox);
            Assert.Equal("Medical Checkup", gammaInbox[0].Subject);

            var alphaInbox = comms.GetMessagesForRecipient("survivor_alpha");
            Assert.Empty(alphaInbox);
        }

        [Fact]
        public void PostFromTemplate_AppliesTemplateMetadata()
        {
            var comms = new InternalCommunicationSystem();
            comms.LoadCatalog(@"{
                ""schema_version"": 1,
                ""templates"": [
                    {
                        ""id"": ""comm_tpl_radiation_alert"",
                        ""title"": ""Radiation Spike Alert"",
                        ""category"": ""warning"",
                        ""default_priority"": ""urgent"",
                        ""duration_days"": 4,
                        ""description"": ""Elevated radiation readings outside main blast doors.""
                    }
                ]
            }");

            var msg = comms.PostFromTemplate(
                templateId: "comm_tpl_radiation_alert",
                authorId: "warden_1",
                recipientId: null,
                currentDay: 8,
                customDetails: "Level 3 dosimeter threshold triggered.");

            Assert.NotNull(msg);
            Assert.Equal(CommunicationCategory.Warning, msg.Category);
            Assert.Equal(MessagePriority.Urgent, msg.Priority);
            Assert.Equal("Radiation Spike Alert", msg.Subject);
            Assert.Contains("Elevated radiation readings", msg.Content);
            Assert.Contains("Level 3 dosimeter", msg.Content);
            Assert.Equal(8, msg.PostedDay);
            Assert.Equal(12, msg.ExpiresDay); // 8 + 4
        }

        [Fact]
        public void BroadcastIntercom_DispatchesAnnouncement_AndTracksAcknowledgements()
        {
            var comms = new InternalCommunicationSystem();

            IntercomAnnouncement? broadcastCaptured = null;
            comms.OnIntercomBroadcast += b => broadcastCaptured = b;

            var ann = comms.BroadcastIntercom(
                authorId: "overseer_prime",
                message: "All personnel to muster stations for emergency drill.",
                priority: MessagePriority.Urgent,
                currentDay: 15);

            Assert.NotNull(ann);
            Assert.Equal(broadcastCaptured, ann);
            Assert.Equal("overseer_prime", ann.AuthorId);
            Assert.Equal(MessagePriority.Urgent, ann.Priority);
            Assert.Empty(ann.AcknowledgedSurvivors);

            // Intercom also created an announcement message
            Assert.Single(comms.Messages);
            Assert.Equal(CommunicationChannel.Intercom, comms.Messages[0].Channel);

            // Survivors acknowledge intercom
            bool ack1 = comms.AcknowledgeIntercom(ann.AnnouncementId, "survivor_1");
            bool ack2 = comms.AcknowledgeIntercom(ann.AnnouncementId, "survivor_2");
            bool ackDup = comms.AcknowledgeIntercom(ann.AnnouncementId, "survivor_1");

            Assert.True(ack1);
            Assert.True(ack2);
            Assert.False(ackDup);
            Assert.Equal(2, ann.AcknowledgedSurvivors.Count);
        }

        [Fact]
        public void MarkAsRead_And_AcknowledgeMessage_UpdatesStateAndFiresEvents()
        {
            var comms = new InternalCommunicationSystem();
            var msg = comms.PostMessage(
                authorId: "leader_1",
                category: CommunicationCategory.Announcement,
                subject: "Water Rules",
                content: "Daily allowance capped at 2 liters.",
                recipientId: "dweller_10",
                priority: MessagePriority.High,
                currentDay: 5);

            Assert.False(msg.IsRead);
            Assert.False(msg.IsAcknowledged);

            string? readBy = null;
            comms.OnMessageRead += (m, r) => readBy = r;
            comms.MarkAsRead(msg.MessageId, "dweller_10");

            Assert.True(msg.IsRead);
            Assert.Equal("dweller_10", readBy);

            string? ackBy = null;
            comms.OnMessageAcknowledged += (m, a) => ackBy = a;
            comms.AcknowledgeMessage(msg.MessageId, "dweller_10");

            Assert.True(msg.IsAcknowledged);
            Assert.Equal("dweller_10", ackBy);
        }

        [Fact]
        public void TickDay_PrunesExpiredMessages()
        {
            var comms = new InternalCommunicationSystem();
            var shortLived = comms.PostMessage("author_1", CommunicationCategory.Notice, "Short", "Body", null, MessagePriority.Low, currentDay: 1, durationDays: 2);
            var longLived = comms.PostMessage("author_2", CommunicationCategory.Notice, "Long", "Body", null, MessagePriority.Normal, currentDay: 1, durationDays: 10);

            Assert.Equal(2, comms.Messages.Count);

            // Advance to day 2: short-lived expires on day 3 (1+2)
            comms.TickDay(2);
            Assert.Equal(2, comms.Messages.Count);

            // Advance to day 3: short-lived pruned
            comms.TickDay(3);
            Assert.Single(comms.Messages);
            Assert.Equal(longLived.MessageId, comms.Messages[0].MessageId);
        }

        [Fact]
        public void CaptureAndRestoreState_RoundTripsMessagesBoardsAndBroadcasts()
        {
            var comms = new InternalCommunicationSystem();
            comms.AddBulletinBoard("Workshop Board", "room_workshop", capacity: 15, isLeadershipOnly: false);
            var msg = comms.PostMessage("author_a", CommunicationCategory.Notice, "Notice A", "Content A", null, MessagePriority.Normal, currentDay: 2);
            var ann = comms.BroadcastIntercom("author_b", "Broadcast B", MessagePriority.High, currentDay: 2);
            comms.AcknowledgeIntercom(ann.AnnouncementId, "survivor_x");

            var state = comms.CaptureState();
            Assert.Equal(2, state.Boards.Count); // Default + workshop
            Assert.Equal(2, state.Messages.Count); // 1 post + 1 intercom mirror
            Assert.Single(state.IntercomBroadcasts);

            var restored = new InternalCommunicationSystem();
            restored.RestoreState(state);

            Assert.Equal(2, restored.Boards.Count);
            Assert.Equal(2, restored.Messages.Count);
            Assert.Single(restored.Broadcasts);
            Assert.Equal("Broadcast B", restored.Broadcasts[0].Message);
            Assert.Single(restored.Broadcasts[0].AcknowledgedSurvivors);
            Assert.Equal("survivor_x", restored.Broadcasts[0].AcknowledgedSurvivors[0]);
        }
    }
}
