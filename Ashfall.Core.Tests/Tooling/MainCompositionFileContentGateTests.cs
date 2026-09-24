// SPDX-License-Identifier: MIT
// ASHFALL gate: every src/Main*.cs composition file must contain actual code.
// An empty (or whitespace/comment-only) Main partial is a paper-wiring
// artifact: it suggests a composition seam that was never written while
// contributing nothing to the build.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class MainCompositionFileContentGateTests
    {
        private static readonly Regex LineComment = new Regex("//.*", RegexOptions.Compiled);
        private static readonly Regex BlockComment =
            new Regex("/\\*.*?\\*/", RegexOptions.Compiled | RegexOptions.Singleline);

        private static readonly Regex ReferencedFile =
            new Regex(@"[\w.]+\.cs", RegexOptions.Compiled);

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

        [Fact]
        public void MainCompositionFiles_AreNeverEmptyScaffolds()
        {
            string srcRoot = SrcDir();
            var files = Directory
                .EnumerateFiles(srcRoot, "Main*.cs", SearchOption.AllDirectories)
                .Where(f => !f.Replace('\\', '/').Contains("/obj/") &&
                            !f.Replace('\\', '/').Contains("/bin/"))
                .ToList();

            Assert.NotEmpty(files);

            var violations = new List<string>();
            foreach (var file in files)
            {
                string raw = File.ReadAllText(file);
                string clean = BlockComment.Replace(raw, string.Empty);
                clean = LineComment.Replace(clean, string.Empty);
                if (!string.IsNullOrWhiteSpace(clean)) continue;

                // A comment-only file is a legitimate decomposition pointer only
                // when every sibling file it names actually exists on disk.
                var dir = Path.GetDirectoryName(file)!;
                var missing = ReferencedFile.Matches(raw)
                    .Select(m => m.Value)
                    .Distinct()
                    .Where(name => !File.Exists(Path.Combine(dir, name)))
                    .ToList();
                string rel = Path.GetRelativePath(srcRoot, file).Replace('\\', '/');
                if (missing.Count > 0 || ReferencedFile.Matches(raw).Count == 0)
                {
                    violations.Add(missing.Count == 0
                        ? rel
                        : rel + " (dangling refs: " + string.Join(", ", missing) + ")");
                }
            }

            Assert.True(violations.Count == 0,
                "Empty Main composition scaffold(s) — wire the seam or delete the file:\n" +
                string.Join("\n", violations));
        }
    }
}
