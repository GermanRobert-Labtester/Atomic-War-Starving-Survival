// SPDX-License-Identifier: MIT
// ASHFALL CI gate: Python subprocess timeout policy.
//
// TEST_POLICY.md ("Tools and long-running checks are Rust-first") and the tooling
// rules require every Python subprocess to carry an explicit timeout, so a hung
// git/ffmpeg/sox/curl/composio call can never block a CI gate or an agent tool run
// indefinitely.
//
// This gate fails when a timeout-capable subprocess call
// (subprocess.run / check_output / call / check_call) omits `timeout=`, or when a
// subprocess.Popen child is never bounded by communicate(timeout=) / wait(timeout=).
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class PythonSubprocessTimeoutGateTests
    {
        private static readonly Regex TimeoutCapableCall =
            new Regex(@"subprocess\s*\.\s*(run|check_output|call|check_call)\s*\(", RegexOptions.Compiled);

        private static readonly Regex PopenCall =
            new Regex(@"subprocess\s*\.\s*Popen\s*\(", RegexOptions.Compiled);

        private static readonly Regex TimeoutKeyword =
            new Regex(@"\btimeout\s*=", RegexOptions.Compiled);

        private static readonly Regex PopenBounded =
            new Regex(@"\.\s*(communicate|wait)\s*\([^)]*\btimeout\s*=", RegexOptions.Compiled);

        // Dev/tooling roots that must obey the timeout policy.
        private static readonly string[] ScanRoots =
        {
            "scripts", "tools", "tests", "Assets", "docs", "ai-experiments", "data-tools"
        };

        private sealed class Finding
        {
            public string Path { get; set; } = string.Empty;
            public int Line { get; set; }
            public string Api { get; set; } = string.Empty;
        }

        private static string RepoRootDir()
        {
            string[] candidates =
            {
                Directory.GetCurrentDirectory(),
                AppContext.BaseDirectory
            };
            foreach (string start in candidates)
            {
                var dir = new DirectoryInfo(Path.GetFullPath(start));
                while (dir != null)
                {
                    if (Directory.Exists(Path.Combine(dir.FullName, "scripts")))
                        return dir.FullName;
                    dir = dir.Parent;
                }
            }
            throw new DirectoryNotFoundException("Could not locate repository root from the test run");
        }

        private static IEnumerable<string> PythonFiles(string root)
        {
            foreach (string dir in ScanRoots)
            {
                string full = Path.Combine(root, dir);
                if (!Directory.Exists(full)) continue;

                foreach (string file in Directory.EnumerateFiles(full, "*.py", SearchOption.AllDirectories))
                {
                    string norm = file.Replace('\\', '/');
                    if (norm.Contains("/obj/") || norm.Contains("/bin/") || norm.Contains("/.venv/")) continue;
                    yield return file;
                }
            }
        }

        [Fact]
        public void PythonSubprocess_CallsCarryTimeout()
        {
            string root = RepoRootDir();
            List<string> files = PythonFiles(root).ToList();
            Assert.True(files.Count > 0, "Expected to find Python tooling files to scan (scan roots may be wrong).");

            var missing = new List<Finding>();
            int scannedCalls = 0;

            foreach (string file in files)
            {
                string content = File.ReadAllText(file);
                string rel = Path.GetRelativePath(root, file).Replace('\\', '/');

                foreach (Match m in TimeoutCapableCall.Matches(content))
                {
                    if (IsCommentStart(content, m.Index)) continue;

                    scannedCalls++;
                    string callText = ExtractCall(content, m.Index + m.Length - 1);
                    if (!TimeoutKeyword.IsMatch(callText))
                    {
                        missing.Add(new Finding
                        {
                            Path = rel,
                            Line = LineOf(content, m.Index),
                            Api = "subprocess." + m.Groups[1].Value
                        });
                    }
                }

                if (PopenCall.IsMatch(content) && !PopenBounded.IsMatch(content))
                {
                    missing.Add(new Finding
                    {
                        Path = rel,
                        Line = LineOf(content, PopenCall.Match(content).Index),
                        Api = "subprocess.Popen (no communicate/wait timeout)"
                    });
                }
            }

            Assert.True(scannedCalls > 0, "Timeout scan found no subprocess calls; the detector may be broken.");

            Assert.True(
                missing.Count == 0,
                $"Python subprocess timeout policy violation ({missing.Count}):\n  " +
                string.Join("\n  ", missing.Select(f => $"{f.Path}:{f.Line} {f.Api}")) +
                "\nAdd an explicit `timeout=` to every subprocess call (TEST_POLICY.md).");
        }

        private static bool IsCommentStart(string content, int index)
        {
            int lineStart = content.LastIndexOf('\n', index) + 1;
            return content.Substring(lineStart, index - lineStart).TrimStart().StartsWith("#");
        }

        // Returns the full text of a parenthesized call starting at openParenIndex,
        // skipping string literals and comments so nested parens inside them do not
        // unbalance the depth counter.
        private static string ExtractCall(string content, int openParenIndex)
        {
            int depth = 0;
            int i = openParenIndex;
            for (; i < content.Length; i++)
            {
                char c = content[i];

                if (c == '\'' || c == '"')
                {
                    char quote = c;
                    i++;
                    while (i < content.Length && content[i] != quote)
                    {
                        if (content[i] == '\\') i++;
                        i++;
                    }
                    continue;
                }

                if (c == '#')
                {
                    while (i < content.Length && content[i] != '\n') i++;
                    continue;
                }

                if (c == '(') depth++;
                else if (c == ')')
                {
                    depth--;
                    if (depth == 0) break;
                }
            }

            int end = Math.Min(i + 1, content.Length);
            return content.Substring(openParenIndex, end - openParenIndex);
        }

        private static int LineOf(string content, int index)
        {
            int line = 1;
            for (int i = 0; i < index && i < content.Length; i++)
            {
                if (content[i] == '\n') line++;
            }
            return line;
        }
    }
}