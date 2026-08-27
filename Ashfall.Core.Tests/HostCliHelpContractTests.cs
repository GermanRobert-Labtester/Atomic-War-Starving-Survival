// SPDX-License-Identifier: MIT
// ASHFALL gate: every public --...-selftest dispatch flag in HostCli.cs
// must be documented in HostCli.PrintHelp() (--host-help).
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class HostCliHelpContractTests
    {
        private static readonly Regex LineComment = new Regex("//.*", RegexOptions.Compiled);
        private static readonly Regex BlockComment =
            new Regex("/\\*.*?\\*/", RegexOptions.Compiled | RegexOptions.Singleline);
        private static readonly Regex SelfTestFlagRegex =
            new Regex(@"--[a-z0-9-]+-selftest", RegexOptions.Compiled);
        private static readonly Regex AnyFlagRegex =
            new Regex(@"--[a-z0-9-]+", RegexOptions.Compiled);

        private static string FindHostCliPath()
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
                    string probe = Path.Combine(dir.FullName, "src", "Host", "HostCli.cs");
                    if (File.Exists(probe))
                        return probe;
                    dir = dir.Parent;
                }
            }
            throw new FileNotFoundException("Could not locate src/Host/HostCli.cs from the test run");
        }

        private static string StripComments(string text)
        {
            text = BlockComment.Replace(text, string.Empty);
            return LineComment.Replace(text, string.Empty);
        }

        [Fact]
        public void EverySelfTestFlag_IsDocumentedInHostHelp()
        {
            string hostCliPath = FindHostCliPath();
            string code = StripComments(File.ReadAllText(hostCliPath));

            int parseIdx = code.IndexOf("Parse(", StringComparison.Ordinal);
            int helpIdx = code.IndexOf("PrintHelp(", StringComparison.Ordinal);

            Assert.True(parseIdx >= 0, "Could not find Parse method in HostCli.cs");
            Assert.True(helpIdx >= 0, "Could not find PrintHelp method in HostCli.cs");

            string parseBody = code.Substring(parseIdx, helpIdx - parseIdx);
            string helpBody = code.Substring(helpIdx);

            var parseFlags = SelfTestFlagRegex.Matches(parseBody)
                .Select(m => m.Value)
                .Distinct(StringComparer.Ordinal)
                .OrderBy(f => f, StringComparer.Ordinal)
                .ToList();

            Assert.True(parseFlags.Count >= 20,
                $"Expected to discover at least 20 --...-selftest flags in Parse, found only {parseFlags.Count}.");

            var helpFlags = new HashSet<string>(
                AnyFlagRegex.Matches(helpBody).Select(m => m.Value),
                StringComparer.Ordinal);

            var missing = parseFlags.Where(f => !helpFlags.Contains(f)).ToList();

            Assert.True(missing.Count == 0,
                "The following --...-selftest flags are dispatched by HostCli.Parse but missing from HostCli.PrintHelp (--host-help):\n  " +
                string.Join("\n  ", missing) +
                "\nAdd documentation for each flag to HostCli.PrintHelp() to keep host help synchronized with CLI capabilities.");
        }

        [Fact]
        public void EveryParseFlag_IsDocumentedInHostHelp()
        {
            string hostCliPath = FindHostCliPath();
            string code = StripComments(File.ReadAllText(hostCliPath));

            int parseIdx = code.IndexOf("Parse(", StringComparison.Ordinal);
            int helpIdx = code.IndexOf("PrintHelp(", StringComparison.Ordinal);

            Assert.True(parseIdx >= 0, "Could not find Parse method in HostCli.cs");
            Assert.True(helpIdx >= 0, "Could not find PrintHelp method in HostCli.cs");

            string parseBody = code.Substring(parseIdx, helpIdx - parseIdx);
            string helpBody = code.Substring(helpIdx);

            var parseFlags = AnyFlagRegex.Matches(parseBody)
                .Select(m => m.Value)
                .Distinct(StringComparer.Ordinal)
                .OrderBy(f => f, StringComparer.Ordinal)
                .ToList();

            var helpFlags = new HashSet<string>(
                AnyFlagRegex.Matches(helpBody).Select(m => m.Value),
                StringComparer.Ordinal);

            var missing = parseFlags.Where(f => !helpFlags.Contains(f)).ToList();

            Assert.True(missing.Count == 0,
                "The following CLI flags are parsed in HostCli.Parse but missing from HostCli.PrintHelp (--host-help):\n  " +
                string.Join("\n  ", missing));
        }
    }
}
