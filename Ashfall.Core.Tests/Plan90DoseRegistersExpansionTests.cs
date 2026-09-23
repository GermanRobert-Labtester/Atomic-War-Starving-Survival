// SPDX-License-Identifier: MIT
// Plan 90 — Dose Registers Expansion: 4 bands → 12 bands, 3 plans → 8 plans
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class Plan90DoseRegistersExpansionTests : CatalogTestBase
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

        private static DoseRegistersCatalog LoadCatalog()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return new DoseRegistersCatalog();
            return DoseRegistersCatalogLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
        }

        [Fact]
        public void Catalog_HasTwelveBandsAndEightPlans()
        {
            var catalog = LoadCatalog();
            if (catalog.bands.Count == 0) return; // data dir not available

            Assert.Equal(12, catalog.bands.Count);
            Assert.Equal(8, catalog.plans.Count);
        }

        [Fact]
        public void Bands_ThresholdsAreStrictlyIncreasing()
        {
            var catalog = LoadCatalog();
            if (catalog.bands.Count == 0) return;

            for (int i = 1; i < catalog.bands.Count; i++)
            {
                Assert.True(
                    catalog.bands[i].threshold_msv > catalog.bands[i - 1].threshold_msv,
                    $"Band[{i}] '{catalog.bands[i].id}' threshold {catalog.bands[i].threshold_msv} mSv " +
                    $"is not > Band[{i-1}] '{catalog.bands[i-1].id}' threshold {catalog.bands[i-1].threshold_msv} mSv");
            }
        }

        [Fact]
        public void Bands_AllIdsAreUniqueAndNonEmpty()
        {
            var catalog = LoadCatalog();
            if (catalog.bands.Count == 0) return;

            var ids = new HashSet<string>();
            foreach (var band in catalog.bands)
            {
                Assert.False(string.IsNullOrEmpty(band.id), "Band has empty id");
                Assert.False(string.IsNullOrEmpty(band.label), $"Band '{band.id}' has empty label");
                Assert.False(string.IsNullOrEmpty(band.disposition), $"Band '{band.id}' has empty disposition");
                Assert.True(ids.Add(band.id), $"Duplicate band id: {band.id}");
            }
        }

        [Fact]
        public void Bands_NewEntriesExist()
        {
            var catalog = LoadCatalog();
            if (catalog.bands.Count == 0) return;

            // 4 original bands still present
            Assert.Contains(catalog.bands, b => b.id == "band_green");
            Assert.Contains(catalog.bands, b => b.id == "band_amber");
            Assert.Contains(catalog.bands, b => b.id == "band_red");
            Assert.Contains(catalog.bands, b => b.id == "band_black");

            // 8 new bands from Plan 90
            Assert.Contains(catalog.bands, b => b.id == "band_white");
            Assert.Contains(catalog.bands, b => b.id == "band_yellow");
            Assert.Contains(catalog.bands, b => b.id == "band_orange");
            Assert.Contains(catalog.bands, b => b.id == "band_rose");
            Assert.Contains(catalog.bands, b => b.id == "band_crimson");
            Assert.Contains(catalog.bands, b => b.id == "band_violet");
            Assert.Contains(catalog.bands, b => b.id == "band_indigo");
            Assert.Contains(catalog.bands, b => b.id == "band_void");
        }

        [Fact]
        public void Plans_AllIdsAreUniqueAndNonEmpty()
        {
            var catalog = LoadCatalog();
            if (catalog.plans.Count == 0) return;

            var ids = new HashSet<string>();
            foreach (var plan in catalog.plans)
            {
                Assert.False(string.IsNullOrEmpty(plan.id), "Plan has empty id");
                Assert.False(string.IsNullOrEmpty(plan.label), $"Plan '{plan.id}' has empty label");
                Assert.False(string.IsNullOrEmpty(plan.cost), $"Plan '{plan.id}' has empty cost");
                Assert.False(string.IsNullOrEmpty(plan.note), $"Plan '{plan.id}' has empty note");
                Assert.True(ids.Add(plan.id), $"Duplicate plan id: {plan.id}");
            }
        }

        [Fact]
        public void Plans_NewEntriesExist()
        {
            var catalog = LoadCatalog();
            if (catalog.plans.Count == 0) return;

            // 3 original plans
            Assert.Contains(catalog.plans, p => p.id == "plan_morphine_tray");
            Assert.Contains(catalog.plans, p => p.id == "plan_comfort_rounds");
            Assert.Contains(catalog.plans, p => p.id == "plan_nothing");

            // 5 new plans from Plan 90
            Assert.Contains(catalog.plans, p => p.id == "plan_chelation");
            Assert.Contains(catalog.plans, p => p.id == "plan_iodine_prophylaxis");
            Assert.Contains(catalog.plans, p => p.id == "plan_isolation");
            Assert.Contains(catalog.plans, p => p.id == "plan_rest");
            Assert.Contains(catalog.plans, p => p.id == "plan_transfer");
        }

        [Fact]
        public void Plans_CostsAreNonEmpty()
        {
            var catalog = LoadCatalog();
            if (catalog.plans.Count == 0) return;

            foreach (var plan in catalog.plans)
            {
                // Valid cost values: an item id, "time", or "none"
                Assert.False(string.IsNullOrWhiteSpace(plan.cost),
                    $"Plan '{plan.id}' has null/empty cost");
            }
        }

        [Fact]
        public void BandLabel_BackwardCompatibilityForCoreFourBands()
        {
            // The DoseLedgerSystem hardcodes BandGreen=0, BandAmber=1, BandRed=2, BandBlack=3.
            // BandIdFor() maps these integers to band ids by lookup — confirm still resolves correctly.
            var catalog = LoadCatalog();
            if (catalog.bands.Count == 0) return;

            Assert.Equal("Green",  DoseRegistersCatalogLoader.BandLabel(catalog, DoseLedgerSystem.BandGreen));
            Assert.Equal("Amber",  DoseRegistersCatalogLoader.BandLabel(catalog, DoseLedgerSystem.BandAmber));
            Assert.Equal("Red",    DoseRegistersCatalogLoader.BandLabel(catalog, DoseLedgerSystem.BandRed));
            Assert.Equal("Black",  DoseRegistersCatalogLoader.BandLabel(catalog, DoseLedgerSystem.BandBlack));
        }
    }
}
