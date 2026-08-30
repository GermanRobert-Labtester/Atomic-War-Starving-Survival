// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Xunit;

namespace Ashfall.Core.Tests.PlayerCommand
{
    /// <summary>
    /// Plan #10 (typed command/result adoption) regression gate.
    ///
    /// A high-risk host command must not infer success by parsing the shape
    /// of a user-facing message string (e.g. "does the reply start with
    /// 'Unknown'/'Cannot'/'Not'?"). That pattern breaks silently the moment
    /// wording changes, localizes, or a new failure message is added that
    /// doesn't happen to start with one of the guessed prefixes — and it
    /// treats presentation text as control flow, which AGENTS.md's Plan #10
    /// explicitly forbids ("preserve user-facing messages as presentation
    /// data, not control flow").
    ///
    /// This gate does not forbid string-returning compatibility wrappers
    /// (ActionResult.cs documents them as an intentional, still-migrating
    /// surface). It forbids the specific anti-pattern of branching on
    /// multiple StartsWith/Contains checks against a command's own result
    /// string to decide whether the command "succeeded" — the sign that a
    /// command has not yet migrated to a typed ActionResult/CommandResult.
    /// </summary>
    public class ProseSuccessInferenceSourceGateTests
    {
        private static string? FindSrcRoot()
        {
            string? dir = AppDomain.CurrentDomain.BaseDirectory;
            while (!string.IsNullOrEmpty(dir))
            {
                string candidate = Path.Combine(dir, "src");
                if (Directory.Exists(candidate) && File.Exists(Path.Combine(dir, "project.godot")))
                    return candidate;
                dir = Directory.GetParent(dir)?.FullName;
            }
            return null;
        }

        /// <summary>
        /// A line that combines two or more "!result.StartsWith(...)" /
        /// "!result.Contains(...)" checks — chained with &amp;&amp; across one
        /// or more lines — against known failure-message prefixes is the
        /// exact prose-success-inference shape this gate rejects. A single
        /// StartsWith/Contains check used for something else (e.g. a demo
        /// script matching one known status line) is not flagged; the risk
        /// is specifically stacking several guessed prefixes to fabricate a
        /// success boolean.
        /// </summary>
        private static readonly Regex ProseNegativeCheck = new Regex(
            @"!\s*\w+\.(StartsWith|Contains)\s*\(\s*""(Unknown|Cannot|Not|Failed|Error|Invalid)""",
            RegexOptions.Compiled);

        [Fact]
        public void SourceGate_MainPartialsDoNotInferCommandSuccessFromMessagePrefixes()
        {
            string? srcRoot = FindSrcRoot();
            if (srcRoot == null) return; // Not running in the repo tree.

            var violations = new List<string>();

            foreach (var file in Directory.GetFiles(srcRoot, "Main*.cs", SearchOption.TopDirectoryOnly))
            {
                string[] lines = File.ReadAllLines(file);
                // Track how many ProseNegativeCheck matches occur within the
                // current statement, spanning wrapped lines, and reset the
                // counter once a statement-ending ';' is seen.
                int matchesInStatement = 0;
                int statementStartLine = 0;

                for (int i = 0; i < lines.Length; i++)
                {
                    string trimmed = lines[i].TrimStart();
                    if (trimmed.StartsWith("//") || trimmed.StartsWith("/*") || trimmed.StartsWith("*"))
                        continue;

                    if (matchesInStatement == 0)
                        statementStartLine = i;

                    matchesInStatement += ProseNegativeCheck.Matches(lines[i]).Count;

                    if (lines[i].Contains(";"))
                    {
                        if (matchesInStatement >= 2)
                        {
                            violations.Add(
                                $"{Path.GetFileName(file)}:L{statementStartLine + 1}-{i + 1}: " +
                                "success inferred by stacking multiple message-prefix checks " +
                                "instead of branching on a typed ActionResult/CommandResult status");
                        }
                        matchesInStatement = 0;
                    }
                }
            }

            Assert.True(violations.Count == 0,
                "Prose-success-inference violations found (migrate to ActionResult/CommandResult):\n  " +
                string.Join("\n  ", violations));
        }
    }
}
