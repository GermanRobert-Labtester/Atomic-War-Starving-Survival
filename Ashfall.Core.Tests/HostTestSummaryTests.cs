using System;
using System.Text.Json;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class HostTestSummaryTests
    {
        [Fact]
        public void FormatBanner_ReturnsStandardBanner()
        {
            var summary = new HostTestSummary
            {
                TestName = "data_integrity_selftest",
                Passed = true,
                ExitCode = 0
            };

            string banner = summary.FormatBanner();
            Assert.Equal("[HOST_SELFTEST] data_integrity_selftest PASS", banner);

            var failSummary = new HostTestSummary
            {
                TestName = "data_integrity_selftest",
                Passed = false,
                ExitCode = 1
            };

            string failBanner = failSummary.FormatBanner();
            Assert.Equal("[HOST_SELFTEST] data_integrity_selftest FAIL", failBanner);
        }

        [Fact]
        public void FormatKeyValue_IncludesAllFieldsWhenPresent()
        {
            var summary = new HostTestSummary
            {
                TestName = "audio_selftest",
                Passed = true,
                ExitCode = 0,
                PassedCount = 42,
                FailedCount = 0,
                Details = "cues=40 resolved=40"
            };

            string kv = summary.FormatKeyValue();
            Assert.Equal("[HOST_SELFTEST_SUMMARY] test=audio_selftest status=PASS exit_code=0 passed=42 failed=0 total=42 details=\"cues=40 resolved=40\"", kv);
        }

        [Fact]
        public void FormatKeyValue_MinimalFields()
        {
            var summary = new HostTestSummary
            {
                TestName = "simple_test",
                Passed = false,
                ExitCode = 2
            };

            string kv = summary.FormatKeyValue();
            Assert.Equal("[HOST_SELFTEST_SUMMARY] test=simple_test status=FAIL exit_code=2", kv);
        }

        [Fact]
        public void FormatJson_ProducesValidJsonWithMatchingFields()
        {
            var summary = new HostTestSummary
            {
                TestName = "expeditions_selftest",
                Passed = true,
                ExitCode = 0,
                PassedCount = 10,
                FailedCount = 0,
                Details = "all sorties green"
            };

            string jsonLine = summary.FormatJson();
            Assert.StartsWith("[HOST_SELFTEST_JSON] ", jsonLine);

            string jsonPayload = jsonLine.Substring("[HOST_SELFTEST_JSON] ".Length);
            using var doc = JsonDocument.Parse(jsonPayload);
            var root = doc.RootElement;

            Assert.Equal("expeditions_selftest", root.GetProperty("test").GetString());
            Assert.Equal("PASS", root.GetProperty("status").GetString());
            Assert.Equal(0, root.GetProperty("exit_code").GetInt32());
            Assert.Equal(10, root.GetProperty("passed").GetInt32());
            Assert.Equal(0, root.GetProperty("failed").GetInt32());
            Assert.Equal(10, root.GetProperty("total").GetInt32());
            Assert.Equal("all sorties green", root.GetProperty("details").GetString());
        }

        [Fact]
        public void FormatLegacyToken_FormatsExpectedLegacyStyle()
        {
            var summary = new HostTestSummary
            {
                TestName = "deep_coast_selftest",
                Passed = true,
                ExitCode = 0
            };

            string legacy = summary.FormatLegacyToken();
            Assert.Equal("DEEP_COAST_SELFTEST PASS", legacy);

            var failSummary = new HostTestSummary
            {
                TestName = "custom_test",
                Passed = false,
                ExitCode = 1
            };

            Assert.Equal("CUSTOM_TEST_SELFTEST FAIL", failSummary.FormatLegacyToken());
        }

        [Fact]
        public void FormatStandardLine_ReturnsStandardizedLine()
        {
            var summary = new HostTestSummary
            {
                TestName = "data_integrity_selftest",
                Passed = true,
                ExitCode = 0
            };

            Assert.Equal("SELFTEST PASS: data_integrity_selftest", summary.FormatStandardLine());

            var failSummary = new HostTestSummary
            {
                TestName = "data_integrity_selftest",
                Passed = false,
                ExitCode = 1
            };

            Assert.Equal("SELFTEST FAIL: data_integrity_selftest", failSummary.FormatStandardLine());
        }

        [Fact]
        public void FormatAll_ContainsAllFiveLines()
        {
            var summary = new HostTestSummary
            {
                TestName = "warlord_selftest",
                Passed = true,
                ExitCode = 0,
                PassedCount = 5,
                FailedCount = 0
            };

            string all = summary.FormatAll();
            Assert.Contains("[HOST_SELFTEST] warlord_selftest PASS", all);
            Assert.Contains("[HOST_SELFTEST_SUMMARY] test=warlord_selftest status=PASS exit_code=0 passed=5 failed=0 total=5", all);
            Assert.Contains("[HOST_SELFTEST_JSON] {\"test\":\"warlord_selftest\"", all);
            Assert.Contains("SELFTEST PASS: warlord_selftest", all);
            Assert.Contains("WARLORD_SELFTEST PASS", all);
        }

        [Fact]
        public void TryParseKeyValue_RoundtripsSuccessfully()
        {
            var original = new HostTestSummary
            {
                TestName = "muster_selftest",
                Passed = true,
                ExitCode = 0,
                PassedCount = 12,
                FailedCount = 0,
                Details = "camp active"
            };

            string kvLine = original.FormatKeyValue();
            bool ok = HostTestSummary.TryParseKeyValue(kvLine, out var parsed);

            Assert.True(ok);
            Assert.NotNull(parsed);
            Assert.Equal(original.TestName, parsed!.TestName);
            Assert.Equal(original.Passed, parsed.Passed);
            Assert.Equal(original.ExitCode, parsed.ExitCode);
            Assert.Equal(original.PassedCount, parsed.PassedCount);
            Assert.Equal(original.FailedCount, parsed.FailedCount);
            Assert.Equal(original.TotalCount, parsed.TotalCount);
            Assert.Equal(original.Details, parsed.Details);
        }

        [Fact]
        public void TryParseJson_RoundtripsSuccessfully()
        {
            var original = new HostTestSummary
            {
                TestName = "weather_save_selftest",
                Passed = false,
                ExitCode = 1,
                PassedCount = 3,
                FailedCount = 1,
                Details = "forecast diverged"
            };

            string jsonLine = original.FormatJson();
            bool ok = HostTestSummary.TryParseJson(jsonLine, out var parsed);

            Assert.True(ok);
            Assert.NotNull(parsed);
            Assert.Equal(original.TestName, parsed!.TestName);
            Assert.Equal(original.Passed, parsed.Passed);
            Assert.Equal(original.ExitCode, parsed.ExitCode);
            Assert.Equal(original.PassedCount, parsed.PassedCount);
            Assert.Equal(original.FailedCount, parsed.FailedCount);
            Assert.Equal(original.TotalCount, parsed.TotalCount);
            Assert.Equal(original.Details, parsed.Details);
        }

        [Fact]
        public void TryParseAny_ParsesAllFormats()
        {
            var summary = new HostTestSummary
            {
                TestName = "holdfast_selftest",
                Passed = true,
                ExitCode = 0,
                PassedCount = 10,
                FailedCount = 0
            };

            // Banner
            Assert.True(HostTestSummary.TryParseAny(summary.FormatBanner(), out var fromBanner));
            Assert.Equal("holdfast_selftest", fromBanner!.TestName);
            Assert.True(fromBanner.Passed);

            // KeyValue
            Assert.True(HostTestSummary.TryParseAny(summary.FormatKeyValue(), out var fromKv));
            Assert.Equal("holdfast_selftest", fromKv!.TestName);
            Assert.True(fromKv.Passed);

            // JSON
            Assert.True(HostTestSummary.TryParseAny(summary.FormatJson(), out var fromJson));
            Assert.Equal("holdfast_selftest", fromJson!.TestName);
            Assert.True(fromJson.Passed);

            // Standard line (SELFTEST PASS: <name>)
            Assert.True(HostTestSummary.TryParseAny(summary.FormatStandardLine(), out var fromStandard));
            Assert.Equal("holdfast_selftest", fromStandard!.TestName);
            Assert.True(fromStandard.Passed);

            // Legacy token
            Assert.True(HostTestSummary.TryParseAny("HOLDFAST_SELFTEST PASS", out var fromLegacy));
            Assert.Equal("holdfast_selftest", fromLegacy!.TestName);
            Assert.True(fromLegacy.Passed);

            // Invalid line returns false
            Assert.False(HostTestSummary.TryParseAny("Some arbitrary log message", out _));
        }

        [Fact]
        public void TryParseAny_HandlesEscapedQuotesInDetails()
        {
            var summary = new HostTestSummary
            {
                TestName = "escaping_test",
                Passed = true,
                ExitCode = 0,
                Details = "value with \"quotes\" and \\ slashes"
            };

            string kv = summary.FormatKeyValue();
            Assert.True(HostTestSummary.TryParseKeyValue(kv, out var parsedKv));
            Assert.Equal("value with \"quotes\" and \\ slashes", parsedKv!.Details);

            string json = summary.FormatJson();
            Assert.True(HostTestSummary.TryParseJson(json, out var parsedJson));
            Assert.Equal("value with \"quotes\" and \\ slashes", parsedJson!.Details);
        }
    }
}
