// SPDX-License-Identifier: MIT
// Audit #41 — document case-fold policy divergence (pin, do not mass-migrate).
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Flag ledger normalizes to lower-invariant + Ordinal. Much of Core still
    /// uses <c>StringComparer.OrdinalIgnoreCase</c>. Mass unification is deferred;
    /// this gate fails if OrdinalIgnoreCase usage collapses unexpectedly
    /// (silent policy flip) or explodes without disposition.
    /// </summary>
    public sealed class CaseFoldPolicyPinTests
    {
        // Observed ~170 hits at audit #41 remediation; allow a band.
        private const int MinOrdinalIgnoreCaseHits = 120;
        private const int MaxOrdinalIgnoreCaseHits = 260;

        private static string RepoRoot()
        {
            string dir = new DirectoryInfo(AppContext.BaseDirectory).FullName;
            for (int i = 0; i < 8 && dir != null; i++)
            {
                if (Directory.Exists(Path.Combine(dir, "Assets", "Ashfall.Core")))
                    return dir;
                dir = Directory.GetParent(dir)?.FullName;
            }
            throw new DirectoryNotFoundException("repo root not found");
        }

        [Fact]
        public void Core_OrdinalIgnoreCase_UsageStaysInDocumentedBand()
        {
            string core = Path.Combine(RepoRoot(), "Assets", "Ashfall.Core");
            int hits = Directory.GetFiles(core, "*.cs", SearchOption.AllDirectories)
                .Sum(f => Regex.Matches(File.ReadAllText(f), @"StringComparer\.OrdinalIgnoreCase").Count);

            Assert.True(hits >= MinOrdinalIgnoreCaseHits && hits <= MaxOrdinalIgnoreCaseHits,
                $"OrdinalIgnoreCase hits={hits} outside pin band [{MinOrdinalIgnoreCaseHits},{MaxOrdinalIgnoreCaseHits}] — "
                + "update band with disposition if intentional mass migrate/expand (audit #41).");
        }

        [Fact]
        public void FlagLedger_UsesNormalizePlusOrdinal_NotIgnoreCaseComparer()
        {
            string path = Path.Combine(RepoRoot(), "Assets", "Ashfall.Core", "Flags", "IFlagLedger.cs");
            Assert.True(File.Exists(path), "Flags/IFlagLedger.cs missing");
            string text = File.ReadAllText(path);
            Assert.Contains("class InMemoryFlagLedger", text, StringComparison.Ordinal);
            Assert.Contains("ToLowerInvariant", text, StringComparison.Ordinal);
            // Must not reintroduce IgnoreCase comparer as the primary store key policy.
            Assert.DoesNotContain("new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase)", text, StringComparison.Ordinal);
        }
    }
}
