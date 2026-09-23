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

        [Fact]
        public void AuthoredData_TimeCapsulesJson_LoadsSuccessfully()
        {
            var dataDir = System.IO.Path.GetFullPath(
                System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory,
                    "..", "..", "..", "..", "Assets", "StreamingAssets", "Data"));
            if (!System.IO.File.Exists(System.IO.Path.Combine(dataDir, "time_capsules.json")))
                dataDir = System.IO.Path.GetFullPath("Assets/StreamingAssets/Data");

            string filePath = System.IO.Path.Combine(dataDir, "time_capsules.json");
            Assert.True(System.IO.File.Exists(filePath), $"File not found: {filePath}");

            string json = System.IO.File.ReadAllText(filePath);
            var catalog = JsonSerializer.Deserialize<TimeCapsuleCatalogData>(json);
            Assert.NotNull(catalog);
            Assert.True(catalog!.capsules.Count >= 4);

            foreach (var cap in catalog.capsules)
            {
                Assert.False(string.IsNullOrWhiteSpace(cap.capsule_id));
                Assert.False(string.IsNullOrWhiteSpace(cap.capsule_name));
                Assert.NotEmpty(cap.contents);
                Assert.All(cap.contents, c => Assert.True(c.sentimental_value > 0f));
            }

            var system = new TimeCapsuleSystem();
            system.LoadCatalog(catalog);
            Assert.True(system.TotalCapsuleCount >= 4);
        }

        [Fact]
        public void CapsuleOpening_AndBereavementComfort_ProvideSolace()
        {
            var dataDir = System.IO.Path.GetFullPath(
                System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory,
                    "..", "..", "..", "..", "Assets", "StreamingAssets", "Data"));
            if (!System.IO.File.Exists(System.IO.Path.Combine(dataDir, "time_capsules.json")))
                dataDir = System.IO.Path.GetFullPath("Assets/StreamingAssets/Data");

            string filePath = System.IO.Path.Combine(dataDir, "time_capsules.json");
            string json = System.IO.File.ReadAllText(filePath);
            var catalog = JsonSerializer.Deserialize<TimeCapsuleCatalogData>(json);

            var system = new TimeCapsuleSystem();
            system.LoadCatalog(catalog);

            // Unopened capsule gives 0 morale bonus
            Assert.Equal(0f, system.GetOpenedCapsuleMoraleBonus("capsule_childhood_memories"));

            // Open capsule
            bool opened = system.OpenCapsule("capsule_childhood_memories", "surv_orphan_child", 12);
            Assert.True(opened);

            float moraleBonus = system.GetOpenedCapsuleMoraleBonus("capsule_childhood_memories");
            // Sentimental values: 85 + 90 = 175 -> 17.5 morale bonus
            Assert.True(moraleBonus >= 15f && moraleBonus <= 20f);

            // Test bereavement comfort from legacy messages
            var msg = system.WriteMessage("surv_parent", "surv_child", "Stay strong, we love you.", DeliveryCondition.OnDeath, currentDay: 5);
            Assert.Equal(0f, system.CalculateBereavementComfort("surv_child", "surv_parent"));

            // Parent passes away, message is delivered
            system.DeliverDeathMessages("surv_parent");
            Assert.True(msg.IsDelivered);

            float comfort = system.CalculateBereavementComfort("surv_child", "surv_parent");
            Assert.Equal(10.0f, comfort);
        }

        [Fact]
        public void EventBasedCapsule_OpensExactlyOnce_OnMatchingEvent()
        {
            var dataDir = System.IO.Path.GetFullPath(
                System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory,
                    "..", "..", "..", "..", "Assets", "StreamingAssets", "Data"));
            if (!System.IO.File.Exists(System.IO.Path.Combine(dataDir, "time_capsules.json")))
                dataDir = System.IO.Path.GetFullPath("Assets/StreamingAssets/Data");

            string filePath = System.IO.Path.Combine(dataDir, "time_capsules.json");
            var catalog = JsonSerializer.Deserialize<TimeCapsuleCatalogData>(System.IO.File.ReadAllText(filePath));

            var system = new TimeCapsuleSystem();
            system.LoadCatalog(catalog!);

            // The shipped Fallen Watchman's Footlocker binds to the defense-repelled event.
            var footlocker = system.GetCapsule("capsule_bunker_echo_memorial");
            Assert.NotNull(footlocker);
            Assert.Equal(OpenConditionType.EventBased, footlocker!.ConditionType);
            Assert.Equal("defense_assault_repelled", footlocker.TargetEventId);
            Assert.False(footlocker.IsOpen);

            // Unrelated events must not open it.
            Assert.Equal(0, system.TryOpenEventCapsules("defense_perimeter_breached", 40));
            Assert.False(footlocker.IsOpen);

            // The matching event opens it exactly once; replays are no-ops.
            Assert.Equal(1, system.TryOpenEventCapsules("defense_assault_repelled", 41));
            Assert.True(footlocker.IsOpen);
            Assert.Equal(41, footlocker.OpenedDay);
            Assert.Equal(0, system.TryOpenEventCapsules("defense_assault_repelled", 42));
        }
    }
}
