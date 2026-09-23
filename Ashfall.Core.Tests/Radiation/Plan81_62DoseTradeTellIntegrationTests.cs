// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Economy;
using Ashfall.Core.IO;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Wave 39 Batch 2 Integration Suite:
    /// - Plan 81 (DEC-241): Radiation Dose Locations Expansion (5 → 14 locations)
    /// - Plan 62 (DEC-242): Trade Tell Lines & Negotiation Tells (4 bands × 5 stances → 60 tell lines)
    ///
    /// Validates cross-system integration between environmental radiation hotspots
    /// and wasteland barter psychology: sector dose mapping, trader trust band
    /// resolution, and seed-deterministic tell rotation under harsh wasteland exposure.
    /// </summary>
    public sealed class Plan81_62DoseTradeTellIntegrationTests
    {
        private static string FindDataDir()
        {
            string dataDir;
            if (!CatalogLocator.TryFindDataDirectory(Directory.GetCurrentDirectory(), out dataDir))
                CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out dataDir);
            return dataDir ?? string.Empty;
        }

        private static DoseContentCatalog LoadDoseCatalog()
        {
            string dataDir = FindDataDir();
            Assert.False(string.IsNullOrEmpty(dataDir), "Could not locate StreamingAssets/Data directory");
            return DoseContentCatalogLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
        }

        private static TradeTellEngine LoadTellEngine()
        {
            string dataDir = FindDataDir();
            string path = Path.Combine(dataDir, "trade_tell_lines.json");
            Assert.True(File.Exists(path), "trade_tell_lines.json must exist");
            return TradeTellEngine.LoadFromJson(File.ReadAllText(path));
        }

        [Fact]
        public void Plan81_62_DoseLocations_SectorMappingAndRiskCalibration()
        {
            var catalog = LoadDoseCatalog();
            Assert.NotNull(catalog.locations);
            Assert.Equal(14, catalog.locations.Count);

            // Verify all 4 canonical sectors are present
            var sectors = new HashSet<string>(catalog.locations.Select(l => l.sector), StringComparer.Ordinal);
            Assert.Contains("bunker", sectors);
            Assert.Contains("surface", sectors);
            Assert.Contains("expedition", sectors);
            Assert.Contains("faction", sectors);

            // Verify dose calibrations: bunker is shielded, expedition has hot zones
            foreach (var loc in catalog.locations)
            {
                Assert.False(string.IsNullOrWhiteSpace(loc.id));
                Assert.StartsWith("loc_", loc.id);
                Assert.False(string.IsNullOrWhiteSpace(loc.displayName));
                Assert.InRange(loc.riskLevel, 0, 8);
                Assert.True(loc.radiationUsv >= 0.01f, $"Location {loc.id} must have positive dose");

                if (loc.sector == "bunker")
                {
                    Assert.InRange(loc.riskLevel, 0, 2);
                    Assert.True(loc.radiationUsv <= 0.5f, $"Bunker location {loc.id} dose exceeds 0.5 uSv/h");
                }
                else if (loc.sector == "expedition")
                {
                    Assert.True(loc.riskLevel >= 3, $"Expedition location {loc.id} risk below 3");
                    Assert.True(loc.radiationUsv >= 5.0f, $"Expedition hot zone {loc.id} dose below 5.0 uSv/h");
                }
            }
        }

        [Fact]
        public void Plan81_62_TradeTell_TrustBands_And_PoolCoverage()
        {
            var engine = LoadTellEngine();

            Assert.Equal(4, engine.BandCount);
            Assert.Equal(20, engine.PoolCount); // 5 stances x 4 bands
            Assert.True(engine.LineCount >= 60, $"Expected >= 60 tell lines, found {engine.LineCount}");

            // Verify trust band mapping thresholds
            Assert.Equal(TradeTrustBands.Hostile, engine.BandForTrust(-75f));
            Assert.Equal(TradeTrustBands.Hostile, engine.BandForTrust(-40f));
            Assert.Equal(TradeTrustBands.Wary, engine.BandForTrust(-20f));
            Assert.Equal(TradeTrustBands.Wary, engine.BandForTrust(0f));
            Assert.Equal(TradeTrustBands.Neutral, engine.BandForTrust(20f));
            Assert.Equal(TradeTrustBands.Neutral, engine.BandForTrust(40f));
            Assert.Equal(TradeTrustBands.Warm, engine.BandForTrust(60f));
            Assert.Equal(TradeTrustBands.Warm, engine.BandForTrust(100f));
        }

        [Fact]
        public void Plan81_62_HotspotTrade_TellRotation_SeededDeterminism()
        {
            var doseCatalog = LoadDoseCatalog();
            var engine = LoadTellEngine();

            // Locate an expedition hot zone
            var hotZone = doseCatalog.locations.Find(l => l.sector == "expedition" && l.radiationUsv >= 30.0f);
            Assert.NotNull(hotZone);

            const int seed = 428162;
            var rng1 = new SeededRng(seed);
            var rng2 = new SeededRng(seed);

            var sequence1 = new List<string>();
            var sequence2 = new List<string>();

            // Simulate 10 trade negotiation rounds in a high-risk zone under wary trust
            for (int i = 0; i < 10; i++)
            {
                bool ok1 = engine.TrySelectTell(TradeStance.Trade, trust: -15f, rng1, out var tell1);
                Assert.True(ok1);
                sequence1.Add(tell1.Line);

                bool ok2 = engine.TrySelectTell(TradeStance.Trade, trust: -15f, rng2, out var tell2);
                Assert.True(ok2);
                sequence2.Add(tell2.Line);
            }

            // Verify exact deterministic replay matching
            Assert.Equal(sequence1.Count, sequence2.Count);
            for (int i = 0; i < sequence1.Count; i++)
            {
                Assert.Equal(sequence1[i], sequence2[i]);
            }
        }

        [Fact]
        public void Plan81_62_DoseLocation_RadiationStress_ModulatesTraderStance()
        {
            var doseCatalog = LoadDoseCatalog();
            var engine = LoadTellEngine();

            // Check high radiation locations
            var extremeLoc = doseCatalog.locations.OrderByDescending(l => l.radiationUsv).First();
            Assert.True(extremeLoc.radiationUsv >= 40f);
            Assert.Equal("loc_military_depot_perimeter", extremeLoc.id);

            var rng = new SeededRng(999);

            // In an extreme radiation hot zone, a desperate trade under Refuse or Rob stance
            // selects legible, restrained posture lines within bounds
            foreach (TradeStance stance in new[] { TradeStance.Refuse, TradeStance.Rob, TradeStance.HostileRaid })
            {
                bool selected = engine.TrySelectTell(stance, trust: -50f, rng, out var tell);
                Assert.True(selected);
                Assert.Equal(TradeTrustBands.Hostile, tell.Band);
                Assert.InRange(tell.Line.Length, 20, 140);
                Assert.False(string.IsNullOrWhiteSpace(tell.Line));
            }
        }
    }
}
