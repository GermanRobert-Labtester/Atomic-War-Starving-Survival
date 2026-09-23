// SPDX-License-Identifier: MIT
// Plan 90 × Plan 78 cross-system integration test
// Plan 90 — Dose Register Bands & Plans Expansion (4 bands → 12 bands, 3 plans → 8 plans)
// Plan 78 — Archive Inks Expansion (3 → 12 ink types) [data already complete]
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Xunit;

namespace Ashfall.Core.Tests.Culture
{
    public sealed class Plan90_78DoseInksIntegrationTests : CatalogTestBase
    {
        private static string FindDataDir()
        {
            string search = Directory.GetCurrentDirectory();
            for (int i = 0; i < 6; i++)
            {
                string candidate = Path.Combine(search, "Assets", "StreamingAssets", "Data");
                if (Directory.Exists(candidate)) return candidate;
                string parent = Directory.GetParent(search)?.FullName;
                if (parent == null) break;
                search = parent;
            }
            return string.Empty;
        }

        // ── Dose Registers (Plan 90) ──────────────────────────────────────────

        [Fact]
        public void DoseRegisters_HasTwelveBandsAndEightPlansWithFourNpcs()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            var catalog = DoseRegistersCatalogLoader.Load(
                dataDir, new FileSystemIO(), new SystemTextJsonSerializer());

            Assert.Equal(12, catalog.bands.Count);
            Assert.Equal(8, catalog.plans.Count);
            Assert.Equal(3, catalog.guesses.Count);
            Assert.Equal(4, catalog.npcs.Count);
        }

        [Fact]
        public void DoseRegisters_BandsAreStrictlyIncreasingAndBackwardCompatible()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            var catalog = DoseRegistersCatalogLoader.Load(
                dataDir, new FileSystemIO(), new SystemTextJsonSerializer());

            // Strictly increasing thresholds
            for (int i = 1; i < catalog.bands.Count; i++)
            {
                Assert.True(
                    catalog.bands[i].threshold_msv > catalog.bands[i - 1].threshold_msv,
                    $"Band '{catalog.bands[i].id}' threshold {catalog.bands[i].threshold_msv} mSv " +
                    $"not > '{catalog.bands[i-1].id}' threshold {catalog.bands[i-1].threshold_msv} mSv");
            }

            // The 4 hardcoded DoseLedgerSystem band integers must still resolve
            Assert.Equal("Green", DoseRegistersCatalogLoader.BandLabel(catalog, DoseLedgerSystem.BandGreen));
            Assert.Equal("Amber", DoseRegistersCatalogLoader.BandLabel(catalog, DoseLedgerSystem.BandAmber));
            Assert.Equal("Red",   DoseRegistersCatalogLoader.BandLabel(catalog, DoseLedgerSystem.BandRed));
            Assert.Equal("Black", DoseRegistersCatalogLoader.BandLabel(catalog, DoseLedgerSystem.BandBlack));
        }

        // ── Archive Inks (Plan 78) ────────────────────────────────────────────

        [Fact]
        public void ArchiveInks_HasTwelveEntriesWithUniqueIds()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            var inks = ArchiveInkCatalogLoader.Load(
                dataDir, new FileSystemIO(), new SystemTextJsonSerializer());

            Assert.Equal(12, inks.Count);

            var ids = new HashSet<string>();
            foreach (var ink in inks)
            {
                Assert.False(string.IsNullOrEmpty(ink.ink_id),
                    $"Archive ink has empty ink_id");
                Assert.True(ids.Add(ink.ink_id),
                    $"Duplicate archive ink id: {ink.ink_id}");
                // Legibility in valid range
                Assert.True(ink.legibilityScore >= 0.1f && ink.legibilityScore <= 1.0f,
                    $"Ink '{ink.ink_id}' legibilityScore {ink.legibilityScore} out of range [0.1, 1.0]");
                // Required item id must be non-empty
                Assert.False(string.IsNullOrEmpty(ink.requiredItemId),
                    $"Ink '{ink.ink_id}' has empty requiredItemId");
            }
        }

        // ── Cross-system coherence ────────────────────────────────────────────

        [Fact]
        public void CrossSystem_DoseAndInkCatalogsLoadIndependently()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            // Both catalogs must load cleanly in the same data directory context
            var doseCatalog = DoseRegistersCatalogLoader.Load(
                dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            var inks = ArchiveInkCatalogLoader.Load(
                dataDir, new FileSystemIO(), new SystemTextJsonSerializer());

            Assert.True(doseCatalog.bands.Count >= 12,
                $"Dose catalog must have >= 12 bands; got {doseCatalog.bands.Count}");
            Assert.True(doseCatalog.plans.Count >= 8,
                $"Dose catalog must have >= 8 plans; got {doseCatalog.plans.Count}");
            Assert.True(inks.Count >= 12,
                $"Archive ink catalog must have >= 12 inks; got {inks.Count}");

            // Both catalogs share the radiation management pillar — neither
            // should interfere with the other's data directory resolution.
            Assert.Equal(4, doseCatalog.npcs.Count);
        }
    }
}
