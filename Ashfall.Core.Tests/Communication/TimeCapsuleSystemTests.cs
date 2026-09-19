// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Communication;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class TimeCapsuleSystemTests
    {
        [Fact]
        public void CreateCapsule_StoresCapsuleAndEmitsEvent()
        {
            var system = new TimeCapsuleSystem();
            TimeCapsule? emitted = null;
            system.OnCapsuleCreated += c => emitted = c;

            var capsule = system.CreateCapsule(
                capsuleName: "Founders Memorial",
                creatorId: "surv_elena",
                conditionType: OpenConditionType.DateBased,
                createdDay: 1,
                targetOpenDay: 50,
                location: "room_vault",
                message: "Remember our beginning.",
                contents: new[]
                {
                    new CapsuleContent { ContentType = CapsuleContentType.Letter, Text = "Words from Elena" }
                }
            );

            Assert.NotNull(capsule);
            Assert.Equal("Founders Memorial", capsule.CapsuleName);
            Assert.Equal("surv_elena", capsule.CreatorId);
            Assert.Equal(50, capsule.OpenDay);
            Assert.False(capsule.IsOpen);
            Assert.Single(capsule.Contents);
            Assert.Equal(capsule, emitted);
            Assert.Equal(1, system.TotalCapsuleCount);
            Assert.Equal(1, system.UnopenedCapsuleCount);
        }

        [Fact]
        public void OpenCapsule_SetsOpenStatusAndRevealsContents()
        {
            var system = new TimeCapsuleSystem();
            var capsule = system.CreateCapsule("Manual Stash", "surv_marcus", OpenConditionType.Manual, createdDay: 5);

            TimeCapsule? openedCapsule = null;
            string? openedBy = null;
            system.OnCapsuleOpened += (c, by) =>
            {
                openedCapsule = c;
                openedBy = by;
            };

            bool opened = system.OpenCapsule(capsule.CapsuleId, "surv_scout", currentDay: 15);

            Assert.True(opened);
            Assert.True(capsule.IsOpen);
            Assert.Equal(15, capsule.OpenedDay);
            Assert.Equal("surv_scout", capsule.OpenedBy);
            Assert.Equal(capsule, openedCapsule);
            Assert.Equal("surv_scout", openedBy);
            Assert.Equal(0, system.UnopenedCapsuleCount);
        }

        [Fact]
        public void TickDay_AutoOpensDateBasedCapsules()
        {
            var system = new TimeCapsuleSystem();
            var capsule = system.CreateCapsule("Decade Capsule", "surv_leader", OpenConditionType.DateBased, createdDay: 1, targetOpenDay: 10);

            // Day 5: not yet open
            system.TickDay(5);
            Assert.False(capsule.IsOpen);

            // Day 10: auto opened
            system.TickDay(10);
            Assert.True(capsule.IsOpen);
            Assert.Equal(10, capsule.OpenedDay);
        }

        [Fact]
        public void DeliverDeathMessages_DeliversPendingOnDeathMessages()
        {
            var system = new TimeCapsuleSystem();
            var msg = system.WriteMessage(
                authorId: "surv_dying",
                recipientId: "surv_friend",
                content: "Take care of the greenhouse.",
                condition: DeliveryCondition.OnDeath
            );

            Assert.False(msg.IsDelivered);
            Assert.Equal(1, system.PendingMessageCount);

            LegacyMessage? delivered = null;
            system.OnMessageDelivered += m => delivered = m;

            system.DeliverDeathMessages("surv_dying");

            Assert.True(msg.IsDelivered);
            Assert.Equal(msg, delivered);
            Assert.Equal(0, system.PendingMessageCount);
        }

        [Fact]
        public void CaptureAndRestoreState_RoundTripsAccurately()
        {
            var system1 = new TimeCapsuleSystem();
            var cap = system1.CreateCapsule("Secret Box", "surv_alpha", OpenConditionType.DateBased, 2, 20, location: "workshop");
            system1.WriteMessage("surv_alpha", "surv_beta", "Greetings", DeliveryCondition.OnDate, 20, 2);

            var state = system1.CaptureState();

            var system2 = new TimeCapsuleSystem();
            system2.RestoreState(state);

            Assert.Equal(1, system2.TotalCapsuleCount);
            Assert.Equal(1, system2.PendingMessageCount);

            var restoredCap = system2.GetCapsule(cap.CapsuleId);
            Assert.NotNull(restoredCap);
            Assert.Equal("Secret Box", restoredCap.CapsuleName);
            Assert.Equal("surv_alpha", restoredCap.CreatorId);
            Assert.Equal("workshop", restoredCap.Location);
        }
    }
}
