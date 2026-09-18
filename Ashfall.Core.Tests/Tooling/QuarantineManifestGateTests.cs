// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Xunit;

namespace Ashfall.Core.Tests.Tooling
{
    /// <summary>
    /// Prevents the test project from accumulating ghost quarantine entries.
    /// A Compile Remove is meaningful only when the referenced source file
    /// actually exists in the worktree; otherwise it is stale debt metadata.
    /// </summary>
    public sealed class QuarantineManifestGateTests
    {
        private static string RepoRoot()
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null)
            {
                if (File.Exists(Path.Combine(dir.FullName, "Ashfall.sln")) ||
                    Directory.Exists(Path.Combine(dir.FullName, "Assets", "Ashfall.Core")))
                    return dir.FullName;
                dir = dir.Parent;
            }
            return Directory.GetCurrentDirectory();
        }

        [Fact]
        public void CompileRemoveQuarantines_MustReferenceExistingSourceFiles()
        {
            string root = RepoRoot();
            string projectPath = Path.Combine(root, "Ashfall.Core.Tests", "Ashfall.Core.Tests.csproj");
            Assert.True(File.Exists(projectPath), $"test project missing at {projectPath}");

            var doc = XDocument.Load(projectPath);
            var removals = doc
                .Descendants("Compile")
                .Select(node => (string?)node.Attribute("Remove"))
                .Where(value => !string.IsNullOrWhiteSpace(value))
                .Select(value => value!.Replace('\\', Path.DirectorySeparatorChar)
                                      .Replace('/', Path.DirectorySeparatorChar))
                .ToArray();

            var missing = new List<string>();
            foreach (string relative in removals)
            {
                string full = Path.GetFullPath(Path.Combine(root, "Ashfall.Core.Tests", relative));
                if (!File.Exists(full))
                    missing.Add(relative);
            }

            Assert.True(
                missing.Count == 0,
                "Ghost Compile Remove quarantine entries must be deleted or restored as real source files:\n  " +
                string.Join("\n  ", missing));
        }
    }
}
