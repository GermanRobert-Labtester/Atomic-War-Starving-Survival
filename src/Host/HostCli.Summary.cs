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

        /// <summary>
        /// Emits a FAIL summary for a self-test that terminated with an unhandled
        /// exception, and returns a non-zero exit code.
        ///
        /// <para><b>Why this exists.</b> A self-test that threw used to escape
        /// <c>Main._Ready()</c> entirely: Godot logged the exception, but
        /// <c>GetTree().Quit(code)</c> was never reached, so the process emitted no
        /// FAIL line and then hung until the CI timeout killed it. Worse, any
        /// PASS lines a gate had already printed remained the last verdict on
        /// stdout, so a crashing gate could be scraped as green.</para>
        ///
        /// <para>Every self-test dispatch is now wrapped so a throw becomes a
        /// reported FAIL with a non-zero exit instead of a silent hang.</para>
        /// </summary>
        public static int EmitUnhandledSelfTestFailure(string testName, System.Exception ex)
        {
            string name = string.IsNullOrEmpty(testName) ? "selftest" : testName;
            string kind = ex?.GetType().Name ?? "Exception";
            string message = ex?.Message ?? "(no message)";

            GD.PrintErr($"[FAIL] {name}: unhandled {kind}: {message}");
            if (ex != null) GD.PrintErr(ex.ToString());

            return EmitSummary(name, false, 1, 0, 1, $"unhandled {kind}: {message}");
        }

        /// <summary>
        /// The snake_case self-test name for a CLI action, matching the names the
        /// gates pass to <see cref="EmitSummary"/> (e.g. <c>ExpeditionSelfTest</c>
        /// becomes <c>expedition_selftest</c>). Used only for failure reporting, so
        /// an unrecognised action still produces a usable label rather than throwing
        /// inside the exception handler.
        /// </summary>
        public static string SelfTestNameFor(HostCliAction action)
        {
            string name = action.ToString();
            var sb = new System.Text.StringBuilder(name.Length + 8);
            for (int i = 0; i < name.Length; i++)
            {
                char c = name[i];
                if (char.IsUpper(c))
                {
                    if (i > 0) sb.Append('_');
                    sb.Append(char.ToLowerInvariant(c));
                }
                else sb.Append(c);
            }
            // The gates spell these without an underscore before "test".
            return sb.ToString()
                .Replace("_self_test", "_selftest")
                .Replace("_ui_test", "_uitest");
        }
    }
}
