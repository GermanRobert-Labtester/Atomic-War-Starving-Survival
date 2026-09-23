// SPDX-License-Identifier: MIT
using System;
using System.Text.Json;
using Ashfall.Core.Save;
using Xunit;

namespace Ashfall.Core.Tests.Save
{
    public sealed class SessionDurabilityManagerTests
    {
        [Fact]
        public void SlotRegistration_EnforcesIsolationAndCapacity()
        {
            var manager = new SessionDurabilityManager();

            bool ok1 = manager.RegisterOrUpdateSlot("slot_01", "Campaign Alpha", 10, 5, "chk_alpha_123");
            bool ok2 = manager.RegisterOrUpdateSlot("slot_02", "Campaign Beta", 40, 8, "chk_beta_456");

            Assert.True(ok1);
            Assert.True(ok2);
            Assert.Equal(2, manager.Slots.Count);

            var slot1 = manager.GetSlot("slot_01");
            var slot2 = manager.GetSlot("slot_02");
            Assert.NotNull(slot1);
            Assert.NotNull(slot2);
            Assert.Equal("Campaign Alpha", slot1.DisplayName);
            Assert.Equal("Campaign Beta", slot2.DisplayName);
            Assert.Equal("chk_alpha_123", slot1.Checksum);
            Assert.Equal("chk_beta_456", slot2.Checksum);
        }

        [Fact]
        public void InterruptedWrite_FlagsCorruption_AndInvokesSeam()
        {
            var manager = new SessionDurabilityManager();
            manager.RegisterOrUpdateSlot("slot_01", "Campaign Alpha", 10, 5, "chk_alpha_123");

            string? reportedSlot = null;
            string? reportedReason = null;
            manager.CorruptionDetectedSeam = (id, reason) =>
            {
                reportedSlot = id;
                reportedReason = reason;
            };

            bool ok = manager.RecordInterruptedWrite("slot_01", "Power loss during flush");

            Assert.True(ok);
            Assert.Equal("slot_01", reportedSlot);
            Assert.Equal("Power loss during flush", reportedReason);

            var slot = manager.GetSlot("slot_01");
            Assert.NotNull(slot);
            Assert.True(slot.IsCorrupt);
        }

        [Fact]
        public void BackupRecovery_RestoresCorruptedSlot_WithValidChecksum()
        {
            var manager = new SessionDurabilityManager();
            manager.RegisterOrUpdateSlot("slot_01", "Campaign Alpha", 10, 5, "chk_corrupt");
            manager.CreateBackup("slot_01", "chk_healthy_backup");

            manager.RecordInterruptedWrite("slot_01", "Truncated write");
            Assert.True(manager.GetSlot("slot_01")!.IsCorrupt);

            bool restoredNotification = false;
            manager.BackupRestoredSeam = (id, success) => restoredNotification = success;

            bool recovered = manager.TryRecoverBackup("slot_01", out string recoveredChecksum);

            Assert.True(recovered);
            Assert.True(restoredNotification);
            Assert.Equal("chk_healthy_backup", recoveredChecksum);

            var slot = manager.GetSlot("slot_01");
            Assert.NotNull(slot);
            Assert.False(slot.IsCorrupt);
            Assert.Equal("chk_healthy_backup", slot.Checksum);
        }

        [Fact]
        public void ChecksumValidation_DetectsMismatchAndTriggersCorruption()
        {
            var manager = new SessionDurabilityManager();
            manager.RegisterOrUpdateSlot("slot_01", "Campaign Alpha", 10, 5, "chk_expected");

            bool valid = manager.ValidateSlotChecksum("slot_01", "chk_tampered");

            Assert.False(valid);
            var slot = manager.GetSlot("slot_01");
            Assert.NotNull(slot);
            Assert.True(slot.IsCorrupt);
        }

        [Fact]
        public void SoakStabilityEvaluation_MeasuresP95AndSlopeAccurately()
        {
            var manager = new SessionDurabilityManager();

            int daysMeasured = 0;
            manager.DayAdvanceMeasuredSeam = (d, ms) => daysMeasured++;

            // Record 100 days of bounded performance
            for (int day = 1; day <= 100; day++)
            {
                float duration = 10f + (day % 5) * 2f;
                long bytes = 100000L + day * 100L;
                manager.RecordDayAdvance(day, duration, bytes);
            }

            Assert.Equal(100, daysMeasured);

            var report = manager.EvaluateSoakStability(maxAllowedP95Ms: 100f, maxAllowedSlopeBytesPerDay: 500f);

            Assert.Equal(100, report.TotalDaysSampled);
            Assert.True(report.IsMonotonicallyStable);
            Assert.True(report.P95AdvanceDurationMs < 50f);
            Assert.Contains("STABLE", report.StabilityVerdict);
        }

        [Fact]
        public void StateCaptureAndRestore_RoundTripsSessionDurability()
        {
            var m1 = new SessionDurabilityManager();
            m1.RegisterOrUpdateSlot("slot_01", "Save Alpha", 15, 6, "chk_1");
            m1.CreateBackup("slot_01", "chk_1_bak");
            m1.RecordDayAdvance(1, 15.5f, 2048);

            var state = m1.CaptureState();
            string json = JsonSerializer.Serialize(state);
            var deserialized = JsonSerializer.Deserialize<SessionDurabilityState>(json);
            Assert.NotNull(deserialized);

            var m2 = new SessionDurabilityManager();
            m2.RestoreState(deserialized);

            Assert.Single(m2.Slots);
            var s2 = m2.GetSlot("slot_01");
            Assert.NotNull(s2);
            Assert.Equal("Save Alpha", s2.DisplayName);
            Assert.True(s2.HasBackup);
            Assert.Equal("chk_1_bak", s2.BackupChecksum);
            Assert.Single(m2.SoakSamples);
        }
    }
}
