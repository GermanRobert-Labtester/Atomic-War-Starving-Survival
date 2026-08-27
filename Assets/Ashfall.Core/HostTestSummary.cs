// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Ashfall.Core
{
    /// <summary>
    /// Machine-readable summary record for host self-tests, UI tests, and headless demos.
    /// Provides standardized formatters and parsers for CI, automated gates, and test runners.
    /// </summary>
    public sealed class HostTestSummary
    {
        public const string BannerPrefix = "[HOST_SELFTEST]";
        public const string KeyValuePrefix = "[HOST_SELFTEST_SUMMARY]";
        public const string JsonPrefix = "[HOST_SELFTEST_JSON]";

        public string TestName { get; set; } = string.Empty;
        public bool Passed { get; set; }
        public int ExitCode { get; set; }
        public int PassedCount { get; set; } = -1;
        public int FailedCount { get; set; } = -1;
        public int TotalCount => (PassedCount >= 0 && FailedCount >= 0) ? PassedCount + FailedCount : (PassedCount >= 0 ? PassedCount : -1);
        public string Details { get; set; } = string.Empty;

        public static string NormalizeTestName(string name)
        {
            if (string.IsNullOrEmpty(name)) return "unknown_selftest";
            string trimmed = name.Trim().TrimStart('-').Replace('-', '_').ToLowerInvariant();
            return trimmed;
        }

        public static string FormatBanner(string testName, bool passed)
        {
            string normalized = NormalizeTestName(testName);
            return $"{BannerPrefix} {normalized} {(passed ? "PASS" : "FAIL")}";
        }

        public static string FormatKeyValue(string testName, bool passed, int exitCode, int passedCount = -1, int failedCount = -1, string details = "")
        {
            string normalized = NormalizeTestName(testName);
            string status = passed ? "PASS" : "FAIL";
            int code = exitCode >= 0 ? exitCode : (passed ? 0 : 1);

            var sb = new StringBuilder();
            sb.Append($"{KeyValuePrefix} test={normalized} status={status} exit_code={code}");
            if (passedCount >= 0) sb.Append($" passed={passedCount}");
            if (failedCount >= 0) sb.Append($" failed={failedCount}");
            if (passedCount >= 0 && failedCount >= 0) sb.Append($" total={passedCount + failedCount}");
            if (!string.IsNullOrEmpty(details)) sb.Append($" details=\"{EscapeString(details)}\"");
            return sb.ToString();
        }

        public static string FormatJson(string testName, bool passed, int exitCode, int passedCount = -1, int failedCount = -1, string details = "")
        {
            string normalized = NormalizeTestName(testName);
            string status = passed ? "PASS" : "FAIL";
            int code = exitCode >= 0 ? exitCode : (passed ? 0 : 1);

            var sb = new StringBuilder();
            sb.Append($"{JsonPrefix} {{\"test\":\"{EscapeJson(normalized)}\",\"status\":\"{status}\",\"exit_code\":{code}");
            if (passedCount >= 0) sb.Append($",\"passed\":{passedCount}");
            if (failedCount >= 0) sb.Append($",\"failed\":{failedCount}");
            if (passedCount >= 0 && failedCount >= 0) sb.Append($",\"total\":{passedCount + failedCount}");
            if (!string.IsNullOrEmpty(details)) sb.Append($",\"details\":\"{EscapeJson(details)}\"");
            sb.Append("}");
            return sb.ToString();
        }

        public static string FormatStandardLine(string testName, bool passed)
        {
            string normalized = NormalizeTestName(testName);
            return $"SELFTEST {(passed ? "PASS" : "FAIL")}: {normalized}";
        }

        public static string FormatLegacyToken(string testName, bool passed)
        {
            string normalized = NormalizeTestName(testName);
            string token = normalized.ToUpperInvariant();
            if (!token.EndsWith("_SELFTEST") && !token.EndsWith("_UITEST") && !token.EndsWith("_DEMO") && !token.EndsWith("_REPORT") && !token.EndsWith("_BRIEFING"))
                token += "_SELFTEST";
            return $"{token} {(passed ? "PASS" : "FAIL")}";
        }

        public string FormatBanner() => FormatBanner(TestName, Passed);
        public string FormatKeyValue() => FormatKeyValue(TestName, Passed, ExitCode, PassedCount, FailedCount, Details);
        public string FormatJson() => FormatJson(TestName, Passed, ExitCode, PassedCount, FailedCount, Details);
        public string FormatStandardLine() => FormatStandardLine(TestName, Passed);
        public string FormatLegacyToken() => FormatLegacyToken(TestName, Passed);
        public string FormatAll() => string.Join("\n", FormatAll(TestName, Passed, ExitCode, PassedCount, FailedCount, Details));

        public static IEnumerable<string> FormatAll(string testName, bool passed, int exitCode = -1, int passedCount = -1, int failedCount = -1, string details = "")
        {
            int code = exitCode >= 0 ? exitCode : (passed ? 0 : 1);
            yield return FormatBanner(testName, passed);
            yield return FormatKeyValue(testName, passed, code, passedCount, failedCount, details);
            yield return FormatJson(testName, passed, code, passedCount, failedCount, details);
            yield return FormatStandardLine(testName, passed);
            yield return FormatLegacyToken(testName, passed);
        }

        public static bool TryParseKeyValue(string line, out HostTestSummary? summary)
        {
            summary = null;
            if (string.IsNullOrWhiteSpace(line)) return false;

            int idx = line.IndexOf(KeyValuePrefix, StringComparison.Ordinal);
            if (idx < 0) return false;

            string content = line.Substring(idx + KeyValuePrefix.Length).Trim();
            if (string.IsNullOrEmpty(content)) return false;

            var result = new HostTestSummary();
            var matches = Regex.Matches(content, @"(?<key>\w+)=(?:""(?<val>(?:\\.|[^""\\])*)""|(?<val>\S+))");
            if (matches.Count == 0) return false;

            foreach (Match m in matches)
            {
                string key = m.Groups["key"].Value.ToLowerInvariant();
                string val = m.Groups["val"].Value;

                switch (key)
                {
                    case "test":
                        result.TestName = val;
                        break;
                    case "status":
                        result.Passed = string.Equals(val, "PASS", StringComparison.OrdinalIgnoreCase);
                        break;
                    case "exit_code":
                    case "exit":
                    case "code":
                        if (int.TryParse(val, out int ec)) result.ExitCode = ec;
                        break;
                    case "passed":
                        if (int.TryParse(val, out int pc)) result.PassedCount = pc;
                        break;
                    case "failed":
                        if (int.TryParse(val, out int fc)) result.FailedCount = fc;
                        break;
                    case "details":
                        result.Details = UnescapeString(val);
                        break;
                }
            }

            if (string.IsNullOrEmpty(result.TestName)) return false;
            summary = result;
            return true;
        }

        public static bool TryParseJson(string line, out HostTestSummary? summary)
        {
            summary = null;
            if (string.IsNullOrWhiteSpace(line)) return false;

            int idx = line.IndexOf(JsonPrefix, StringComparison.Ordinal);
            if (idx < 0) return false;

            string json = line.Substring(idx + JsonPrefix.Length).Trim();
            if (string.IsNullOrEmpty(json) || !json.StartsWith("{") || !json.EndsWith("}")) return false;

            var result = new HostTestSummary();

            var testMatch = Regex.Match(json, @"""test""\s*:\s*""([^""]+)""");
            if (testMatch.Success) result.TestName = testMatch.Groups[1].Value;

            var statusMatch = Regex.Match(json, @"""status""\s*:\s*""([^""]+)""");
            if (statusMatch.Success) result.Passed = string.Equals(statusMatch.Groups[1].Value, "PASS", StringComparison.OrdinalIgnoreCase);

            var exitMatch = Regex.Match(json, @"""exit_code""\s*:\s*(-?\d+)");
            if (exitMatch.Success && int.TryParse(exitMatch.Groups[1].Value, out int ec)) result.ExitCode = ec;

            var passedMatch = Regex.Match(json, @"""passed""\s*:\s*(\d+)");
            if (passedMatch.Success && int.TryParse(passedMatch.Groups[1].Value, out int p)) result.PassedCount = p;

            var failedMatch = Regex.Match(json, @"""failed""\s*:\s*(\d+)");
            if (failedMatch.Success && int.TryParse(failedMatch.Groups[1].Value, out int f)) result.FailedCount = f;

            var detailsMatch = Regex.Match(json, @"""details""\s*:\s*""((?:\\.|[^""\\])*)""");
            if (detailsMatch.Success) result.Details = UnescapeJson(detailsMatch.Groups[1].Value);

            if (string.IsNullOrEmpty(result.TestName)) return false;
            summary = result;
            return true;
        }

        public static bool TryParseStandardLine(string line, out HostTestSummary? summary)
        {
            summary = null;
            if (string.IsNullOrWhiteSpace(line)) return false;

            var match = Regex.Match(line.Trim(), @"^SELFTEST\s+(?<status>PASS|FAIL):\s*(?<name>[A-Za-z0-9_.-]+)$", RegexOptions.IgnoreCase);
            if (!match.Success) return false;

            string name = NormalizeTestName(match.Groups["name"].Value);
            bool passed = string.Equals(match.Groups["status"].Value, "PASS", StringComparison.OrdinalIgnoreCase);

            summary = new HostTestSummary
            {
                TestName = name,
                Passed = passed,
                ExitCode = passed ? 0 : 1
            };
            return true;
        }

        public static bool TryParseAny(string line, out HostTestSummary? summary)
        {
            if (TryParseKeyValue(line, out summary)) return true;
            if (TryParseJson(line, out summary)) return true;
            if (TryParseStandardLine(line, out summary)) return true;

            // Fallback: check banner line [HOST_SELFTEST] <name> <PASS|FAIL>
            if (!string.IsNullOrWhiteSpace(line))
            {
                int bannerIdx = line.IndexOf(BannerPrefix, StringComparison.Ordinal);
                if (bannerIdx >= 0)
                {
                    string content = line.Substring(bannerIdx + BannerPrefix.Length).Trim();
                    string[] parts = content.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length >= 2)
                    {
                        summary = new HostTestSummary
                        {
                            TestName = parts[0],
                            Passed = string.Equals(parts[1], "PASS", StringComparison.OrdinalIgnoreCase),
                            ExitCode = string.Equals(parts[1], "PASS", StringComparison.OrdinalIgnoreCase) ? 0 : 1
                        };
                        return true;
                    }
                }

                // Fallback: check legacy token line <TOKEN> PASS|FAIL
                var legacyMatch = Regex.Match(line.Trim(), @"^(?<name>[A-Za-z0-9_]+)_(?<kind>SELFTEST|UITEST|DEMO|REPORT|BRIEFING)\s+(?<status>PASS|FAIL)$", RegexOptions.IgnoreCase);
                if (legacyMatch.Success)
                {
                    string rawName = legacyMatch.Groups["name"].Value.ToLowerInvariant() + "_" + legacyMatch.Groups["kind"].Value.ToLowerInvariant();
                    bool passed = string.Equals(legacyMatch.Groups["status"].Value, "PASS", StringComparison.OrdinalIgnoreCase);
                    summary = new HostTestSummary
                    {
                        TestName = rawName,
                        Passed = passed,
                        ExitCode = passed ? 0 : 1
                    };
                    return true;
                }
            }

            summary = null;
            return false;
        }

        private static string EscapeString(string str)
        {
            if (string.IsNullOrEmpty(str)) return string.Empty;
            return str.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\n", " ").Replace("\r", " ");
        }

        private static string UnescapeString(string str)
        {
            if (string.IsNullOrEmpty(str)) return string.Empty;
            return str.Replace("\\\"", "\"").Replace("\\\\", "\\");
        }

        private static string EscapeJson(string str)
        {
            if (string.IsNullOrEmpty(str)) return string.Empty;
            return str.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\n", "\\n").Replace("\r", "\\r").Replace("\t", "\\t");
        }

        private static string UnescapeJson(string str)
        {
            if (string.IsNullOrEmpty(str)) return string.Empty;
            return str.Replace("\\n", "\n").Replace("\\r", "\r").Replace("\\t", "\t").Replace("\\\"", "\"").Replace("\\\\", "\\");
        }
    }
}
