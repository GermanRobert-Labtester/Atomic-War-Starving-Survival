// SPDX-License-Identifier: MIT
// Plan 26A Phase G — forbidden data-path gate. Every runtime catalog reader
// must resolve through CatalogPath; new private resolvers fail this gate.
// The allowlist is the explicit migration remainder and must shrink as sites
// move onto the authority (a stale allowlist entry also fails).
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Xunit;

namespace Ashfall.Core.Tests.Tooling
{
    public sealed class CatalogPathForbiddenGateTests
    {
        private static string RepoRoot()
        {
            string dir = new DirectoryInfo(AppContext.BaseDirectory).FullName;
            for (int i = 0; i < 8 && dir != null; i++)
            {
                if (Directory.Exists(Path.Combine(dir, "src"))) return dir;
                dir = Directory.GetParent(dir)?.FullName;
            }
            throw new DirectoryNotFoundException("repo root not found");
        }

        private static readonly string[] Forbidden =
        {
            "Assets/StreamingAssets/Data",
            "GlobalizePath(\"res://Assets/",
            "Directory.GetCurrentDirectory()",
        };

        /// <summary>Plan 26A tranche-2 complete — all sites migrated onto CatalogPath.
        /// Only the authority itself remains (it legitimately contains the patterns
        /// as internal constants and comments).</summary>
        private static readonly HashSet<string> Allowed = new HashSet<string>(StringComparer.Ordinal)
        {
            "src/Host/CatalogPath.cs",
        };

        private static List<(string file, string pattern)> Scan()
        {
            var hits = new List<(string, string)>();
            string src = Path.Combine(RepoRoot(), "src");
            foreach (string path in Directory.GetFiles(src, "*.cs", SearchOption.AllDirectories))
            {
                string rel = Path.GetRelativePath(RepoRoot(), path).Replace('\\', '/');
                string text = File.ReadAllText(path);
                foreach (string p in Forbidden)
                    if (Regex.IsMatch(text, Regex.Escape(p)))
                        hits.Add((rel, p));
            }
            return hits;
        }

        [Fact]
        public void NoNewPrivateDataResolvers()
        {
            var offenders = new List<string>();
            foreach (var (file, pattern) in Scan())
                if (!Allowed.Contains(file))
                    offenders.Add($"{file} :: {pattern}");
            Assert.True(offenders.Count == 0,
                "new private data resolver(s) — route through CatalogPath:\n  " + string.Join("\n  ", offenders));
        }

        [Fact]
        public void AllowlistShrinksAsSitesMigrate()
        {
            var present = new HashSet<string>(StringComparer.Ordinal);
            foreach (var (file, _) in Scan()) present.Add(file);
            var stale = new List<string>();
            foreach (string file in Allowed)
                if (!present.Contains(file))
                    stale.Add(file);
            Assert.True(stale.Count == 0,
                "allowlist entries no longer carry a forbidden pattern — remove them:\n  " + string.Join("\n  ", stale));
        }
    }
}
