// SPDX-License-Identifier: MIT
using System.Collections.Generic;
using Ashfall.Core.Localization;
using Xunit;

namespace Ashfall.Core.Tests.Localization
{
    public sealed class StringFreezePolicyTests
    {
        [Fact]
        public void ValidateKeyFormat_ValidUiChromeKey_Passes()
        {
            bool valid = StringFreezePolicy.ValidateKeyFormat(
                StringFreezeClass.UiChrome,
                "ui.dashboard.btn_craft",
                out string? error);

            Assert.True(valid);
            Assert.Null(error);
        }

        [Fact]
        public void ValidateKeyFormat_InvalidPrefix_Fails()
        {
            bool valid = StringFreezePolicy.ValidateKeyFormat(
                StringFreezeClass.Settings,
                "audio.master_volume", // expected settings.*
                out string? error);

            Assert.False(valid);
            Assert.Contains("settings.", error);
        }

        [Fact]
        public void ValidateStringSubmission_RawStringInFrozenClass_Rejected()
        {
            var policy = new StringFreezePolicy();

            bool valid = policy.ValidateStringSubmission(
                StringFreezeClass.WarningsAlerts,
                "Radiation critical!",
                isKey: false,
                out string? rejectionReason);

            Assert.False(valid);
            Assert.Contains("string freeze", rejectionReason);
        }

        [Fact]
        public void ValidateStringSubmission_AllowlistedDebt_Allowed()
        {
            var allowlist = new List<string> { "System Activity", "Back" };
            var policy = new StringFreezePolicy(allowlist);

            bool valid = policy.ValidateStringSubmission(
                StringFreezeClass.UiChrome,
                "Back",
                isKey: false,
                out string? rejectionReason);

            Assert.True(valid);
            Assert.Null(rejectionReason);
        }

        [Fact]
        public void ValidateStringSubmission_ValidKey_Allowed()
        {
            var policy = new StringFreezePolicy();

            bool valid = policy.ValidateStringSubmission(
                StringFreezeClass.ItemMetadata,
                "item.canned_beans.name",
                isKey: true,
                out string? rejectionReason);

            Assert.True(valid);
            Assert.Null(rejectionReason);
        }
    }
}
