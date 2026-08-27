// SPDX-License-Identifier: MIT
// ASHFALL Core Tests: Portable Relative-Link Validation Gate for Documentation.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class DocLinkValidationGateTests
    {
        private static readonly Regex FileUriPattern = new Regex(
            @"\[([^\]]*)\]\((file:///[^)]+|/home/[^)]+)\)",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly Regex MarkdownLinkPattern = new Regex(
            @"\[([^\]]+)\]\(([^)]+)\)",
            RegexOptions.Compiled);

        private static string RepoRoot()
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
                    string probe1 = Path.Combine(dir.FullName, "docs");
                    string probe2 = Path.Combine(dir.FullName, "src");
                    if (Directory.Exists(probe1) && Directory.Exists(probe2))
                        return dir.FullName;
                    dir = dir.Parent;
                }
            }
            throw new DirectoryNotFoundException("Could not locate repository root from test execution context.");
        }

        [Fact]
        public void ZeroMachineSpecificFileUris_InDocumentation()
        {
            string root = RepoRoot();
            var mdFiles = Directory.EnumerateFiles(root, "*.md", SearchOption.AllDirectories)
                .Where(f => !f.Replace('\\', '/').Contains("/.git/") &&
                            !f.Replace('\\', '/').Contains("/bin/") &&
                            !f.Replace('\\', '/').Contains("/obj/"))
                .OrderBy(f => f, StringComparer.Ordinal)
                .ToList();

            Assert.True(mdFiles.Count > 100, $"Expected to find >100 markdown documents, found {mdFiles.Count}.");

            var violations = new List<string>();
            foreach (string file in mdFiles)
            {
                string text = File.ReadAllText(file);
                var matches = FileUriPattern.Matches(text);
                foreach (Match m in matches)
                {
                    string relPath = Path.GetRelativePath(root, file).Replace('\\', '/');
                    violations.Add($"{relPath}: {m.Value}");
                }
            }

            Assert.True(violations.Count == 0,
                $"Discovered {violations.Count} machine-specific file:/// or /home/ links in documentation. " +
                "All doc links must use portable relative paths:\n  " +
                string.Join("\n  ", violations.Take(30)) +
                (violations.Count > 30 ? $"\n  ...and {violations.Count - 30} more" : ""));
        }

        [Fact]
        public void AuthorityDocs_RelativeLinksResolveToExistingFiles()
        {
            string root = RepoRoot();
            string docsDir = Path.Combine(root, "docs");
            if (!Directory.Exists(docsDir)) return;

            // Check high-authority docs
            string[] authorityDocs =
            {
                Path.Combine(root, "docs", "INDEX.md"),
                Path.Combine(root, "docs", "saves", "SAVE_STORE_CONTRACT_MATRIX.md"),
                Path.Combine(root, "docs", "cli", "HOST_CLI_COMMAND_CATALOG.md"),
                Path.Combine(root, "sources.md")
            };

            var brokenLinks = new List<string>();

            foreach (string docPath in authorityDocs)
            {
                if (!File.Exists(docPath)) continue;

                string docDir = Path.GetDirectoryName(docPath)!;
                string content = File.ReadAllText(docPath);
                var matches = MarkdownLinkPattern.Matches(content);

                foreach (Match m in matches)
                {
                    string target = m.Groups[2].Value.Trim();
                    // Skip web URLs, anchors, mailto
                    if (target.StartsWith("http://") || target.StartsWith("https://") ||
                        target.StartsWith("#") || target.StartsWith("mailto:"))
                        continue;

                    // Strip anchor if present
                    string pathOnly = target.Split('#')[0];
                    if (string.IsNullOrWhiteSpace(pathOnly)) continue;

                    string fullTargetPath = Path.GetFullPath(Path.Combine(docDir, pathOnly));
                    if (!File.Exists(fullTargetPath) && !Directory.Exists(fullTargetPath))
                    {
                        string relDoc = Path.GetRelativePath(root, docPath).Replace('\\', '/');
                        brokenLinks.Add($"{relDoc} -> {target} (resolved to: {fullTargetPath})");
                    }
                }
            }

            Assert.True(brokenLinks.Count == 0,
                $"Found {brokenLinks.Count} broken relative links in authority documents:\n  " +
                string.Join("\n  ", brokenLinks));
        }
    }
}
