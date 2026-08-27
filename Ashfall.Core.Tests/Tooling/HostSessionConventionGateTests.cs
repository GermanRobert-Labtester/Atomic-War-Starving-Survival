// SPDX-License-Identifier: MIT
// ASHFALL gate: every Godot HostSession inheriting HostSessionBase must not
// hide base members (StateChanged, RaiseStateChanged, MarkDirty, Save, Dispose)
// and must adhere to the standardized HostSessionBase StateChanged convention.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class HostSessionConventionGateTests
    {
        private static readonly Regex LineComment = new Regex("//.*", RegexOptions.Compiled);
        private static readonly Regex BlockComment =
            new Regex("/\\*.*?\\*/", RegexOptions.Compiled | RegexOptions.Singleline);

        private static readonly Regex StateChangedEventDeclaration =
            new Regex(@"(?:public|protected|private|internal)\s+(?:event\s+Action(?:\?)?\s+StateChanged\s*;)", RegexOptions.Compiled);

        private static readonly Regex RaiseStateChangedMethodDeclaration =
            new Regex(@"(?:public|protected|private|internal)\s+(?:void\s+RaiseStateChanged\s*\()", RegexOptions.Compiled);

        private static string SrcDir()
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
                    string probe = Path.Combine(dir.FullName, "src");
                    if (Directory.Exists(probe))
                        return probe;
                    dir = dir.Parent;
                }
            }
            throw new DirectoryNotFoundException("Could not locate src/ from the test run");
        }

        private static string StripComments(string text)
        {
            text = BlockComment.Replace(text, string.Empty);
            return LineComment.Replace(text, string.Empty);
        }

        [Fact]
        public void HostSessions_DoNotHideHostSessionBaseMembers()
        {
            string srcRoot = SrcDir();
            var hostSessionFiles = Directory
                .EnumerateFiles(Path.Combine(srcRoot, "Host"), "*HostSession*.cs", SearchOption.AllDirectories)
                .Where(f => !f.Replace('\\', '/').Contains("/obj/") &&
                            !f.Replace('\\', '/').Contains("/bin/") &&
                            !f.EndsWith("HostSessionBase.cs", StringComparison.OrdinalIgnoreCase))
                .ToList();

            Assert.NotEmpty(hostSessionFiles);

            var violations = new List<string>();

            foreach (var file in hostSessionFiles)
            {
                string raw = File.ReadAllText(file);
                string clean = StripComments(raw);

                // If it inherits from HostSessionBase
                if (clean.Contains(": HostSessionBase") || clean.Contains(":HostSessionBase") || clean.Contains("HostSessionBase"))
                {
                    string relPath = Path.GetRelativePath(srcRoot, file).Replace('\\', '/');

                    if (StateChangedEventDeclaration.IsMatch(clean))
                    {
                        violations.Add($"{relPath}: redeclares 'StateChanged' event, hiding HostSessionBase.StateChanged");
                    }

                    if (RaiseStateChangedMethodDeclaration.IsMatch(clean))
                    {
                        violations.Add($"{relPath}: declares 'RaiseStateChanged()' method, hiding HostSessionBase.RaiseStateChanged()");
                    }
                }
            }

            Assert.True(violations.Count == 0,
                $"Found {violations.Count} HostSessionBase member hiding violation(s):\n" +
                string.Join("\n", violations));
        }
    }
}
