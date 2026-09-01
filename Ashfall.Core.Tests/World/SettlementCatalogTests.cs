using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Ashfall.Core;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests.World
{
    public class SettlementCatalogTests
    {
        private static string ResolveDataDir()
        {
            string baseDir = AppContext.BaseDirectory;
            string probe = Path.Combine(baseDir, "StreamingAssets", "Data");
            if (Directory.Exists(probe)) return probe;

            probe = Path.Combine(baseDir, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data");
            if (Directory.Exists(probe)) return Path.GetFullPath(probe);

            probe = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "StreamingAssets", "Data");
            if (Directory.Exists(probe)) return Path.GetFullPath(probe);

            return string.Empty;
        }

        [Fact]
        public void SettlementCatalog_LoadsAllTwelveAuthoredSettlements()
        {
            string dataDir = ResolveDataDir();
            var catalog = SettlementCatalog.LoadFromDirectory(dataDir, new FileSystemIO());

            Assert.NotNull(catalog);
            Assert.Equal(12, catalog.SettlementCount);

            var expectedIds = new[]
            {
                "settlement_tinkers_notch",
                "settlement_ferry_crossing",
                "settlement_nine_rails",
                "settlement_iron_siding",
                "settlement_fort_karkov",
                "settlement_lock_seven",
                "settlement_brine_pans",
                "settlement_silo_burrow",
                "settlement_slate_hollow",
                "settlement_pilgrim_hearth",
                "settlement_cape_beacon",
                "settlement_st_nicholas"
            };

            foreach (var id in expectedIds)
            {
                Assert.True(catalog.TryGetSettlement(id, out var settlement), $"Settlement '{id}' should exist in catalog.");
                Assert.NotNull(settlement);
                Assert.False(string.IsNullOrWhiteSpace(settlement.DisplayName));
                Assert.False(string.IsNullOrWhiteSpace(settlement.Description));
                Assert.False(string.IsNullOrWhiteSpace(settlement.Region));
                Assert.True(settlement.GetEffectivePopulation() >= 12 && settlement.GetEffectivePopulation() <= 200);
                Assert.True(settlement.ThreatLevel >= 1 && settlement.ThreatLevel <= 5);
                Assert.NotEmpty(settlement.GetEffectiveLocationId());
                Assert.NotEmpty(settlement.GetEffectiveAllegiance());
            }
        }

        [Fact]
        public void SettlementCatalog_ArchetypeDistribution_ThreePerArchetype()
        {
            string dataDir = ResolveDataDir();
            var catalog = SettlementCatalog.LoadFromDirectory(dataDir, new FileSystemIO());

            var tradePosts = catalog.Settlements.Where(s => s.Archetype.Contains("Trade") || s.Archetype.Contains("Market")).ToList();
            var strongholds = catalog.Settlements.Where(s => s.Archetype.Contains("Stronghold") || s.Archetype.Contains("Town") || s.Archetype.Contains("Yard")).ToList();
            var refugeeCamps = catalog.Settlements.Where(s => s.Archetype.Contains("Camp") || s.Archetype.Contains("Enclave") || s.Archetype.Contains("Collective")).ToList();
            var religiousCommunities = catalog.Settlements.Where(s => s.Archetype.Contains("Religious") || s.Archetype.Contains("Sanctuary") || s.Archetype.Contains("Commune")).ToList();

            Assert.Equal(3, tradePosts.Count);
            Assert.Equal(3, strongholds.Count);
            Assert.Equal(3, refugeeCamps.Count);
            Assert.Equal(3, religiousCommunities.Count);
        }

        [Fact]
        public void SettlementCatalog_AllSettlementIds_UniqueAndPrefixed()
        {
            string dataDir = ResolveDataDir();
            var catalog = SettlementCatalog.LoadFromDirectory(dataDir, new FileSystemIO());

            var idSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var settlement in catalog.Settlements)
            {
                Assert.StartsWith("settlement_", settlement.Id);
                Assert.True(idSet.Add(settlement.Id), $"Duplicate settlement ID found: {settlement.Id}");
            }
        }

        [Fact]
        public void SettlementCatalog_AllLocationLinks_ResolveInLocationsJson()
        {
            string dataDir = ResolveDataDir();
            string locationsPath = Path.Combine(dataDir, "locations.json");
            Assert.True(File.Exists(locationsPath), "locations.json must exist.");

            using var doc = JsonDocument.Parse(File.ReadAllText(locationsPath));
            var validLocationIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            if (doc.RootElement.TryGetProperty("locations", out var locsElem))
            {
                foreach (var loc in locsElem.EnumerateArray())
                {
                    if (loc.TryGetProperty("id", out var idElem))
                    {
                        validLocationIds.Add(idElem.GetString() ?? string.Empty);
                    }
                }
            }

            var catalog = SettlementCatalog.LoadFromDirectory(dataDir, new FileSystemIO());
            foreach (var settlement in catalog.Settlements)
            {
                string locId = settlement.GetEffectiveLocationId();
                Assert.True(validLocationIds.Contains(locId), $"Location '{locId}' for settlement '{settlement.Id}' does not exist in locations.json.");
            }
        }

        [Fact]
        public void SettlementCatalog_AllFactionAllegiances_Resolve()
        {
            string dataDir = ResolveDataDir();
            var knownFactions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "faction_the_office", "faction_the_cutters", "faction_the_fleet", "faction_black_flotilla",
                "faction_archivists", "faction_lamplighters", "faction_quiet_house", "faction_grain_exchange",
                "faction_sun_seekers", "faction_osteophages", "faction_the_tally", "faction_undertow",
                "faction_cold_count", "faction_deserter_coalition", "faction_the_provisioned", "faction_long_walk",
                "faction_scavenger_guild", "faction_iron_raiders", "faction_hydro_barons", "faction_the_tempest",
                "faction_blank_rows", "faction_the_scale", "faction_the_underwrite", "faction_the_compact",
                "faction_the_garrison", "faction_unaligned", "none"
            };

            var catalog = SettlementCatalog.LoadFromDirectory(dataDir, new FileSystemIO());
            foreach (var settlement in catalog.Settlements)
            {
                string faction = settlement.GetEffectiveAllegiance();
                Assert.True(knownFactions.Contains(faction), $"Faction '{faction}' for settlement '{settlement.Id}' is not recognized.");
            }
        }

        [Fact]
        public void SettlementCatalog_AllTradeGoodsAndNeeds_ResolveInItemsJson()
        {
            string dataDir = ResolveDataDir();
            var validItemIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var itemFile in Directory.EnumerateFiles(dataDir, "*item*.json"))
            {
                using var doc = JsonDocument.Parse(File.ReadAllText(itemFile));
                if (doc.RootElement.TryGetProperty("items", out var itemsElem) && itemsElem.ValueKind == JsonValueKind.Array)
                {
                    foreach (var item in itemsElem.EnumerateArray())
                    {
                        if (item.TryGetProperty("id", out var idElem))
                        {
                            validItemIds.Add(idElem.GetString() ?? string.Empty);
                        }
                    }
                }
            }

            var catalog = SettlementCatalog.LoadFromDirectory(dataDir, new FileSystemIO());
            foreach (var settlement in catalog.Settlements)
            {
                foreach (var good in settlement.TradeGoods)
                {
                    Assert.True(validItemIds.Contains(good), $"Export good '{good}' in settlement '{settlement.Id}' not found in items.json.");
                }
                foreach (var need in settlement.TradeNeeds)
                {
                    Assert.True(validItemIds.Contains(need), $"Import need '{need}' in settlement '{settlement.Id}' not found in items.json.");
                }
            }
        }

        [Fact]
        public void SettlementCatalog_TradeGoodsAndNeeds_HaveNoContradictoryOverlaps()
        {
            string dataDir = ResolveDataDir();
            var catalog = SettlementCatalog.LoadFromDirectory(dataDir, new FileSystemIO());

            foreach (var settlement in catalog.Settlements)
            {
                var exports = new HashSet<string>(settlement.TradeGoods, StringComparer.OrdinalIgnoreCase);
                foreach (var need in settlement.TradeNeeds)
                {
                    Assert.False(exports.Contains(need), $"Settlement '{settlement.Id}' exports and imports the same item '{need}'.");
                }
            }
        }

        [Fact]
        public void SettlementCatalog_CaravanIntegration_FourCaravanRoutesIncludeSettlements()
        {
            string dataDir = ResolveDataDir();
            string caravansPath = Path.Combine(dataDir, "caravans.json");
            Assert.True(File.Exists(caravansPath), "caravans.json must exist.");

            using var doc = JsonDocument.Parse(File.ReadAllText(caravansPath));
            var caravanElem = doc.RootElement.GetProperty("caravans");
            Assert.True(caravanElem.GetArrayLength() >= 4);

            var catalog = SettlementCatalog.LoadFromDirectory(dataDir, new FileSystemIO());
            var settlementNodes = catalog.Settlements.Select(s => s.RouteNode)
                .Concat(catalog.Settlements.Select(s => s.GetEffectiveLocationId()))
                .Where(n => !string.IsNullOrEmpty(n))
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            int caravansWithSettlements = 0;
            foreach (var caravan in caravanElem.EnumerateArray())
            {
                if (caravan.TryGetProperty("route_node_ids", out var nodesElem))
                {
                    bool hasSettlement = false;
                    foreach (var node in nodesElem.EnumerateArray())
                    {
                        string nodeId = node.GetString() ?? string.Empty;
                        if (settlementNodes.Contains(nodeId))
                        {
                            hasSettlement = true;
                            break;
                        }
                    }
                    if (hasSettlement) caravansWithSettlements++;
                }
            }

            // All 4 caravans route through at least one settlement node
            Assert.Equal(4, caravansWithSettlements);
        }

        [Fact]
        public void SettlementCatalog_ExpeditionIntegration_ThreeFriendlyStopsExist()
        {
            string dataDir = ResolveDataDir();
            string expeditionsPath = Path.Combine(dataDir, "expeditions.json");
            Assert.True(File.Exists(expeditionsPath), "expeditions.json must exist.");

            using var doc = JsonDocument.Parse(File.ReadAllText(expeditionsPath));
            var expElem = doc.RootElement.GetProperty("expeditions");

            var settlementExpDestinations = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var exp in expElem.EnumerateArray())
            {
                if (exp.TryGetProperty("id", out var idElem))
                {
                    string id = idElem.GetString() ?? string.Empty;
                    if (id.StartsWith("loc_settlement_"))
                    {
                        settlementExpDestinations.Add(id);
                    }
                }
            }

            // At least 3 settlement expedition stops exist
            Assert.True(settlementExpDestinations.Count >= 3, $"Expected at least 3 settlement expedition stops, found {settlementExpDestinations.Count}.");
        }
    }
}
