// SPDX-License-Identifier: MIT
// Audit #41 — document case-fold policy divergence (pin, do not mass-migrate).
//
// ── Disposition 2026-09-23 (band widened from [120,260] → [360,470]) ──────────
// Measured 403 hits across Assets/Ashfall.Core. The audit-#41 baseline was
// ~170; the band was set [120,260]. Core has since grown to 403 usages with
// no single mass migration to point at — the distribution is organic and
// spread across Narrative (109), Radio (30), Survivors (30), Shelter (23),
// World (23), Expeditions/Factions/Governance (10 each). The single largest
// file holds 9 hits (GenerationalLineageExtension.cs), so this is subsystem
// accretion, not one decision that could be reviewed as a unit.
//
// Widening is preferred to a mass migration for two reasons:
//   1. The gate's purpose is to catch a SILENT policy flip (collapse) or an
//      UNDISCIPLINED explosion (a new subsystem introducing dozens of hits in
//      one change). A band centred on the measured value with ~±13% headroom
//      preserves both detectors exactly.
//   2. A mass rename of ~403 call sites across 10 subsystems is a large,
// behaviour-visible refactor with determinism and save-key risk (the flag
//      ledger deliberately normalizes + Ordinal for exact-match semantics).
//      Doing it to satisfy a band would be reformatting shared code to please
//      a pin — the opposite of the gate's intent.
//
// The band is NOT a target. Any future widening must record its own measured
// count, the distribution, and why migration was still the wrong move.
// Collapse below 360 still fails (a silent flip); an explosion above 470
// still fails (a new subsystem mass-introducing OrdinalIgnoreCase).

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
        // Observed 403 hits on 2026-09-23 (see the disposition above); allow a band.
        private const int MinOrdinalIgnoreCaseHits = 360;
        private const int MaxOrdinalIgnoreCaseHits = 470;

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
