// SPDX-License-Identifier: MIT
using Godot;
using Ashfall.Core;

namespace AtomicWar.GodotApp
{
    public static partial class HostCli
    {
        /// <summary>
        /// Emits standardized, machine-readable PASS/FAIL summary lines for host self-tests.
        /// Outputs:
        ///   1. [HOST_SELFTEST] <name> <PASS|FAIL>
        ///   2. [HOST_SELFTEST_SUMMARY] test=<name> status=<PASS|FAIL> exit_code=<code> ...
        ///   3. [HOST_SELFTEST_JSON] {"test":"...","status":"PASS","exit_code":0,...}
        ///   4. SELFTEST <PASS|FAIL>: <name> (standardized format)
        ///   5. <NAME>_SELFTEST <PASS|FAIL> (legacy backward compatibility)
        /// </summary>
        public static int EmitSummary(string testName, bool passed, int exitCode = -1, int passedCount = -1, int failedCount = -1, string details = "")
        {
            int code = exitCode >= 0 ? exitCode : (passed ? 0 : 1);
            foreach (var line in HostTestSummary.FormatAll(testName, passed, code, passedCount, failedCount, details))
            {
                GD.Print(line);
            }
            return code;
        }

        /// <summary>
        /// Emits standardized summary lines from an engine-agnostic HeadlessReport.
        /// </summary>
        public static int EmitSummaryFromHeadlessReport(string testName, HeadlessReport report)
        {
            if (report == null)
                return EmitSummary(testName, false, 1, 0, 1, "null report");
            return EmitSummary(testName, report.Passed, report.ExitCode, report.PassedCount, report.FailedCount, report.Summary ?? string.Empty);
        }
    }
}
