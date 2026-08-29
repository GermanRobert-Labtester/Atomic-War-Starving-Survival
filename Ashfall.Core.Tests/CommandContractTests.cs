// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.Crafting;
using Ashfall.Core.Expeditions;
using Ashfall.Core.PlayerCommand;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class CommandContractTests
    {
        // ── CommandPreview factories ──────────────────────────────────

        [Fact]
        public void Preview_Available_HasExpectedFields()
        {
            long version = 42L;
            var deltas = new Dictionary<string, double> { ["wood"] = -2, ["plank"] = 1 };
            var preview = CommandPreview.Available(
                PlayerCommandCode.CraftStart,
                version,
                deltas,
                1.5f,
                riskCodes: new[] { "injury" },
                isIrreversible: false,
                messageKey: "craft.ok");

            Assert.True(preview.IsAvailable);
            Assert.Equal(PlayerCommandCode.CraftStart, preview.CommandCode);
            Assert.Equal(version, preview.StateVersion);
            Assert.Equal(1.5f, preview.EstimatedDurationHours);
            Assert.False(preview.IsIrreversible);
            Assert.Equal("craft.ok", preview.MessageKey);
            Assert.Single(preview.RiskCodes);
            Assert.Equal("injury", preview.RiskCodes[0]);
            Assert.Equal(-2, preview.ProjectedDeltas["wood"]);
            Assert.Equal(1, preview.ProjectedDeltas["plank"]);
        }

        [Fact]
        public void Preview_Unavailable_HasExpectedFields()
        {
            long version = 7L;
            var preview = CommandPreview.Unavailable(
                PlayerCommandCode.ExpeditionDispatch,
                "already_active",
                "expedition.already_active",
                version);

            Assert.False(preview.IsAvailable);
            Assert.Equal("already_active", preview.FailureCode);
            Assert.Equal(version, preview.StateVersion);
            Assert.True(string.IsNullOrEmpty(preview.MessageKey) || preview.MessageKey == "expedition.already_active");
        }

        [Fact]
        public void Preview_ToString_IsStable()
        {
            var preview = CommandPreview.Available(PlayerCommandCode.CraftStart, 1L);
            Assert.Equal("[craft.start] available=True version=1 failure=", preview.ToString());
        }

        // ── CommandResult factories ───────────────────────────────────

        [Fact]
        public void Result_FromSuccess_HasExpectedFields()
        {
            var inner = ActionResult.Success("craft.started", new Dictionary<string, double> { ["wood"] = -1 });
            var result = CommandResult.FromSuccess(PlayerCommandCode.CraftStart, inner, 10L, 11L, 100L);

            Assert.True(result.IsSuccess);
            Assert.Equal(PlayerCommandCode.CraftStart, result.CommandCode);
            Assert.Equal(10L, result.ExpectedStateVersion);
            Assert.Equal(11L, result.ActualStateVersion);
            Assert.Equal(100L, result.ActionLogSequence);
            Assert.Equal("craft.started", result.MessageKey);
            Assert.Equal(-1, result.Deltas["wood"]);
        }

        [Fact]
        public void Result_FromPreview_PreservesPreviewState()
        {
            var preview = CommandPreview.Unavailable(PlayerCommandCode.RepairPipe, "not_burst", "thermal.not_burst", 5L);
            var result = CommandResult.FromPreview(preview, "thermal.not_burst");

            Assert.False(result.IsSuccess);
            Assert.Equal(PlayerCommandCode.RepairPipe, result.CommandCode);
            Assert.Equal("not_burst", result.FailureCode);
            Assert.Equal(5L, result.ExpectedStateVersion);
            Assert.Equal(5L, result.ActualStateVersion);
        }

        [Fact]
        public void Result_StalePreview_HasStaleCode()
        {
            var result = CommandResult.StalePreview(PlayerCommandCode.ExpeditionDispatch, 3L, 5L);

            Assert.False(result.IsSuccess);
            Assert.Equal("stale_preview", result.FailureCode);
            Assert.Equal(3L, result.ExpectedStateVersion);
            Assert.Equal(5L, result.ActualStateVersion);
        }

        [Fact]
        public void Result_ContextBlocked_HasContextCode()
        {
            var result = CommandResult.ContextBlocked(PlayerCommandCode.TradeConfirm, "paused", "command.paused", 9L);

            Assert.False(result.IsSuccess);
            Assert.Equal("paused", result.FailureCode);
            Assert.Equal(9L, result.ExpectedStateVersion);
        }

        // ── CommandContext ────────────────────────────────────────────

        [Fact]
        public void Context_ReturnsFirstBlockingCode()
        {
            var ctx = new CommandContext { IsPaused = true, IsModalOpen = true, IsTutorialActive = true };
            Assert.Equal("paused", ctx.GetBlockingFailureCode());
        }

        [Fact]
        public void Context_NullWhenAllClear()
        {
            var ctx = new CommandContext();
            Assert.Null(ctx.GetBlockingFailureCode());
        }

        // ── CampaignActionLog ─────────────────────────────────────────

        [Fact]
        public void ActionLog_Append_IsMonotonic()
        {
            var log = new CampaignActionLog();
            var e1 = new CampaignActionLogEntry { Day = 1, CommandCode = PlayerCommandCode.CraftStart };
            var e2 = new CampaignActionLogEntry { Day = 1, CommandCode = PlayerCommandCode.CraftStart };

            long s1 = log.Append(e1);
            long s2 = log.Append(e2);

            Assert.Equal(1L, s1);
            Assert.Equal(2L, s2);
            Assert.Equal(2, log.Entries.Count);
        }

        [Fact]
        public void ActionLog_Restore_PreservesNextSequence()
        {
            var log = new CampaignActionLog();
            log.Append(new CampaignActionLogEntry { Day = 1, CommandCode = PlayerCommandCode.CraftStart });
            log.Append(new CampaignActionLogEntry { Day = 1, CommandCode = PlayerCommandCode.CraftStart });

            var save = log.Capture();
            var restored = new CampaignActionLog();
            restored.Restore(save);

            Assert.Equal(2, restored.Entries.Count);
            Assert.Equal(3L, restored.Append(new CampaignActionLogEntry { Day = 2, CommandCode = PlayerCommandCode.ExpeditionDispatch }));
        }

        [Fact]
        public void ActionLog_Clear_ResetsSequence()
        {
            var log = new CampaignActionLog();
            log.Append(new CampaignActionLogEntry { Day = 1, CommandCode = PlayerCommandCode.CraftStart });
            log.Clear();
            long s = log.Append(new CampaignActionLogEntry { Day = 2, CommandCode = PlayerCommandCode.ExpeditionDispatch });
            Assert.Equal(1L, s);
        }
    }
}
