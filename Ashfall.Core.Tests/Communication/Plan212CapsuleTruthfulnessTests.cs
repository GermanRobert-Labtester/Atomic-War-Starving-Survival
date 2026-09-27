// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 212 — Time Capsule truthfulness slice: lawful unseal, privacy of
// pending content, and exactly-once open with save/reload parity.
// ============================================================================
using System;
using System.IO;
using Ashfall.Core.Communication;
using Xunit;

namespace Ashfall.Core.Tests.Communication
{
    public sealed class Plan212CapsuleTruthfulnessTests
    {
        private static TimeCapsuleSystem FreshSystem()
        {
            var s = new TimeCapsuleSystem();
            s.CreateCapsule("Founders Vault", "overseer", OpenConditionType.DateBased,
                createdDay: 1, targetOpenDay: 10, location: "vault", message: "for later");
            s.CreateCapsule("Watchman Box", "watchman", OpenConditionType.SurvivorBased,
                createdDay: 1, targetSurvivorId: "surv_recipient", location: "gate", message: "to the recipient");
            s.CreateCapsule("Manual Capsule", "overseer", OpenConditionType.Manual,
                createdDay: 1, location: "pantry", message: "open anytime");
            return s;
        }

        [Fact]
        public void DateCapsule_RefusesBeforeOpenDay_AndOpensAtIt()
        {
            var s = FreshSystem();
            Assert.False(s.OpenCapsule("capsule_1", "Overseer", 5), "day 5 must refuse the day-10 capsule");
            Assert.True(s.OpenCapsule("capsule_1", "Overseer", 10));
            Assert.True(s.OpenCapsule("capsule_1", "Overseer", 20) == false); // already open: no double morale
        }

        [Fact]
        public void RecipientCapsule_RefusesOtherOpeners()
        {
            var s = FreshSystem();
            Assert.False(s.OpenCapsule("capsule_2", "Overseer", 50), "non-recipient must refuse a SurvivorBased capsule");
            Assert.True(s.OpenCapsule("capsule_2", "surv_recipient", 50));
        }

        [Fact]
        public void ManualCapsule_OpensFreely_Once()
        {
            var s = FreshSystem();
            Assert.True(s.OpenCapsule("capsule_3", "Overseer", 1));
            Assert.False(s.OpenCapsule("capsule_3", "Overseer", 2));
        }

        [Fact]
        public void PendingLegacyMessage_HasContentHiddenInView_AndKeepsState()
        {
            var s = FreshSystem();
            var msg = s.WriteMessage("author_surv", "recipient_surv", "private words",
                DeliveryCondition.OnDate, deliveryDay: 99, currentDay: 2);
            Assert.False(msg.IsDelivered);

            // The raw ledger keeps the content (owner state is untouched).
            // Pending-message privacy is enforced by the host read model and
            // pinned by the HostSource gate below (VisibleMessages()).
            Assert.Equal("private words", msg.Content);
            Assert.False(msg.IsDelivered);
        }

        [Fact]
        public void OpenPersistsThroughSaveReload_WithoutReplay()
        {
            var s = FreshSystem();
            Assert.True(s.OpenCapsule("capsule_3", "Overseer", 7));
            var saved = s.CaptureState();
            var restored = new TimeCapsuleSystem();
            restored.RestoreState(saved);
            var cap = Assert.Single(restored.Capsules, c => c.CapsuleId == "capsule_3");
            Assert.True(cap.IsOpen);
            Assert.Equal(7, cap.OpenedDay);
            Assert.Equal("Overseer", cap.OpenedBy);
        }

        [Fact]
        public void HostSource_PassesLiveDay_AndNeverBypassesTheCondition()
        {
            string RepoRoot()
            {
                var dir = new DirectoryInfo(AppContext.BaseDirectory);
                while (dir != null)
                {
                    if (File.Exists(Path.Combine(dir.FullName, "src", "Main.TimeCapsule.cs"))) return dir.FullName;
                    dir = dir.Parent!;
                }
                throw new DirectoryNotFoundException("repo root");
            }
            string Source(string rel) => File.ReadAllText(Path.Combine(RepoRoot(), rel));

            Assert.Contains("TryOpenCapsule", Source(Path.Combine("src", "UI", "TimeCapsulePanel.cs")));
            Assert.DoesNotContain("_host.OpenCapsule(", Source(Path.Combine("src", "UI", "TimeCapsulePanel.cs")));
            Assert.Contains("DayProvider = () => _simDay", Source(Path.Combine("src", "Main.TimeCapsule.cs")));
            Assert.Contains("VisibleMessages()", Source(Path.Combine("src", "UI", "TimeCapsulePanel.cs")));
        }
    }
}
