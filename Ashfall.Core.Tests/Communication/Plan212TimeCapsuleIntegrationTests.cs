// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Ashfall.Core.Communication;
using Xunit;

namespace Ashfall.Core.Tests.Communication
{
    public sealed class Plan212TimeCapsuleIntegrationTests
    {
        [Fact]
        public void TimeCapsule_CreateAndAutoOpen_OnDate()
        {
            var system = new TimeCapsuleSystem();
            TimeCapsule? opened = null;
            system.OnCapsuleOpened += (c, by) => opened = c;

            var capsule = system.CreateCapsule(
                capsuleName: "Memory Core",
                creatorId: "surv_founder",
                conditionType: OpenConditionType.DateBased,
                createdDay: 1,
                targetOpenDay: 7,
                contents: new[]
                {
                    new CapsuleContent { ContentId = "c1", ContentType = CapsuleContentType.Drawing, Text = "Drawing of green trees" }
                }
            );

            Assert.NotNull(capsule);
            Assert.Equal(1, system.TotalCapsuleCount);
            Assert.Equal(1, system.UnopenedCapsuleCount);
            Assert.False(capsule.IsOpen);

            // Tick before open day
            system.TickDay(5);
            Assert.False(capsule.IsOpen);
            Assert.Null(opened);

            // Tick on open day
            system.TickDay(7);
            Assert.True(capsule.IsOpen);
            Assert.NotNull(opened);
            Assert.Equal(0, system.UnopenedCapsuleCount);
        }

        [Fact]
        public void LegacyMessage_DeliverOnDeath_WhenTriggered()
        {
            var system = new TimeCapsuleSystem();
            var msg = system.WriteMessage("surv_soldier", "surv_child", "Be brave.", DeliveryCondition.OnDeath, currentDay: 1);

            Assert.NotNull(msg);
            Assert.False(msg.IsDelivered);
            Assert.Equal(1, system.PendingMessageCount);

            system.DeliverDeathMessages("surv_soldier");
            Assert.True(msg.IsDelivered);
            Assert.Equal(0, system.PendingMessageCount);
        }

        [Fact]
        public void TimeCapsule_StatePersistence_RoundtripsAccurately()
        {
            var system = new TimeCapsuleSystem();
            system.CreateCapsule("Vault A", "surv_1", OpenConditionType.Manual, 1);
            system.WriteMessage("surv_1", "surv_2", "Secret", DeliveryCondition.Immediate, currentDay: 1);

            var state = system.CaptureState();
            string json = JsonSerializer.Serialize(state);
            var restored = JsonSerializer.Deserialize<TimeCapsuleState>(json);
            Assert.NotNull(restored);

            var newSystem = new TimeCapsuleSystem();
            newSystem.RestoreState(restored!);

            Assert.Equal(1, newSystem.TotalCapsuleCount);
            Assert.Equal("Vault A", newSystem.Capsules[0].CapsuleName);
            Assert.Single(newSystem.Messages);
            Assert.True(newSystem.Messages[0].IsDelivered);
        }
    }
}
