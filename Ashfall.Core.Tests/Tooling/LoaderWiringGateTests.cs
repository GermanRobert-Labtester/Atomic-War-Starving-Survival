// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Unwired-code audit tripwire: every catalog loader exposing
    /// <c>LoadAndRegister</c> must be invoked from production host code
    /// (<c>src/</code>) or appear on the dispositioned allowlist below.
    ///
    /// This class of misswiring shipped three times before this gate existed:
    /// the regional-treaty catalog (fixed by the Plan 25 25G.7 host feed), the
    /// research knowledge catalog (fixed by Plan 34 — LoadCatalog had no
    /// production caller), and the trade-specialty catalog (fixed 2026-09-02 —
    /// the loader existed, its data existed, tests proved it, and nothing in
    /// production ever called it, leaving the wired Phase-0 specialty loop
    /// patternless). A loader passing this gate is provably reachable from the
    /// running game; an allowlisted one carries a disposition and an owner.
    /// </summary>
    public sealed class LoaderWiringGateTests
    {
        private static string RepoRoot()
        {
            string dir = new DirectoryInfo(AppContext.BaseDirectory).FullName;
            for (int i = 0; i < 8 && dir != null; i++)
            {
                if (Directory.Exists(Path.Combine(dir, "Assets", "Ashfall.Core")))
                    return dir;
                dir = Directory.GetParent(dir)?.FullName;
            }
            throw new DirectoryNotFoundException("repo root not found from test context");
        }

        /// <summary>
        /// Loaders deliberately without a production call site today. Every
        /// entry states the disposition; removing an entry requires actually
        /// wiring the loader (or deleting dead code).
        /// </summary>
        private static readonly Dictionary<string, string> Allowlist =
            new Dictionary<string, string>(StringComparer.Ordinal)
            {
                // Designed-dormant expansion content awaiting activation.
                ["SkyLayerArmorCatalogLoader"] = "Expansion 11 'Orbital Harrow' system is designed-dormant; wire on expansion activation.",
                ["SpiritualCatalogLoader"] = "Plan 30 spiritual-meaning coordinator is designed-dormant; wire on plan activation.",
                // Unwired-feature backlog — systems/catalogs exist, host wiring needs a design decision.
                ["AtmosphereCatalogLoader"] = "AtmosphereTextSystem has no consumer surface yet (flavor text venue undecided).",
                ["EnvironmentalTextCatalogLoader"] = "EnvironmentalTextSystem has no consumer surface yet (flavor text venue undecided).",
                ["DebtTemplateCatalogLoader"] = "Ledger-debt templates await economy trade-session integration.",
                ["HoldfastNpcCatalogLoader"] = "Holdfast NPC definitions await holdfast quest-loop integration.",
                // Foreign concurrent stream landed the catalog 2026-09-01 (7738facc); wiring presumed in-flight.
                ["CollectibleCatalogLoader"] = "Collectibles catalog is fresh concurrent-stream work; do not wire from this stream.",
                ["DynamicQuestlineCatalogLoader"] = "Dynamic questlines catalog awaits narrative quest-loop host wiring.",
            };

        [Fact]
        public void EveryLoadAndRegisterLoader_IsCalledFromProduction_OrAllowlisted()
        {
            string root = RepoRoot();
            var coreFiles = Directory.GetFiles(Path.Combine(root, "Assets", "Ashfall.Core"), "*.cs", SearchOption.AllDirectories);
            var srcFiles = Directory.GetFiles(Path.Combine(root, "src"), "*.cs", SearchOption.AllDirectories);
            var allFiles = coreFiles.Concat(srcFiles).ToDictionary(f => f, f => File.ReadAllText(f), StringComparer.Ordinal);

            // 1. Find every LoadAndRegister declaration and its enclosing class.
            var loaders = new List<(string Class, string File)>();
            foreach (var (file, text) in allFiles)
            {
                foreach (Match m in Regex.Matches(text, @"public static int LoadAndRegister\("))
                {
                    var classMatches = Regex.Matches(text.Substring(0, m.Index), @"(?:class|struct)\s+(\w+)");
                    var cls = classMatches.Count > 0 ? classMatches[classMatches.Count - 1].Groups[1].Value : null;
                    if (cls != null)
                        loaders.Add((cls, file));
                }
            }

            Assert.True(loaders.Count >= 10, $"expected the repo's loader set, found only {loaders.Count} — scan rotted?");

            // 2. Require a non-defining production reference, else allowlist.
            var failures = new List<string>();
            foreach (var (cls, file) in loaders.OrderBy(l => l.Class, StringComparer.Ordinal))
            {
                bool wired = allFiles.Any(kv =>
                    kv.Key != file
                    && !kv.Key.Contains("Tests", StringComparison.Ordinal)
                    && Regex.IsMatch(kv.Value, @"\b" + Regex.Escape(cls) + @"\s*\.\s*LoadAndRegister\b"));
                if (wired) continue;
                if (Allowlist.TryGetValue(cls, out var disposition))
                    continue;
                failures.Add($"{cls} ({Path.GetFileName(file)}) — no production LoadAndRegister call site and no allowlist disposition");
            }

            Assert.True(failures.Count == 0,
                "Unwired catalog loaders detected (the 'loader landed, feeder never wired' defect class):\n  "
                + string.Join("\n  ", failures));
        }

        [Fact]
        public void AllowlistEntries_StillExist_AsLoaders()
        {
            // An allowlist entry whose loader was deleted must be pruned — the
            // list documents living dispositions, not history.
            string root = RepoRoot();
            var coreFiles = Directory.GetFiles(Path.Combine(root, "Assets", "Ashfall.Core"), "*.cs", SearchOption.AllDirectories);
            var srcFiles = Directory.GetFiles(Path.Combine(root, "src"), "*.cs", SearchOption.AllDirectories);
            var allText = string.Join("\n", coreFiles.Concat(srcFiles).Select(File.ReadAllText));

            var stale = Allowlist.Keys
                .Where(cls => !Regex.IsMatch(allText, @"class\s+" + Regex.Escape(cls) + @"\b"))                .OrderBy(k => k, StringComparer.Ordinal)
                .ToList();

            Assert.True(stale.Count == 0, "prune stale allowlist entries: " + string.Join(", ", stale));
        }
    }
}
