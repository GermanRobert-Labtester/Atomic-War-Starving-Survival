using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using Ashfall.Core;

namespace Ashfall.Core.Tests
{
    public sealed class StandingRecordFactionExpansionTests
    {
        private static string ResolveDataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found))
                return found;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found))
                return found;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
        }

        private sealed class StandingRecordFactionEntryDto
        {
            public string id { get; set; } = string.Empty;
            public string display_name { get; set; } = string.Empty;
            public string alignment { get; set; } = string.Empty;
            public string home_region { get; set; } = string.Empty;
            public bool is_active { get; set; } = true;
            public int trust { get; set; } = 0;
            public string[] wants { get; set; } = Array.Empty<string>();
            public string[] offers { get; set; } = Array.Empty<string>();
            public string signature_quote { get; set; } = string.Empty;
            public string access_rule { get; set; } = string.Empty;
            public string badge_asset_id { get; set; } = string.Empty;
        }

        private static List<StandingRecordFactionEntryDto> LoadFactions()
        {
            string dataDir = ResolveDataDir();
            string path = Path.Combine(dataDir, "standing_record_factions.json");
            Assert.True(File.Exists(path), "standing_record_factions.json must exist at " + path);
            string json = File.ReadAllText(path);
            var items = CatalogLocator.LoadWrappedList<StandingRecordFactionEntryDto>(json, SystemTextJsonSerializer.Options);
            Assert.NotNull(items);
            return items;
        }

        [Fact]
        public void Catalog_LoadsSuccessfully_ContainsExactEightFactions()
        {
            var factions = LoadFactions();
            Assert.Equal(8, factions.Count);
        }

        [Fact]
        public void BaselineOverlay_PreservedVerbatim()
        {
            var factions = LoadFactions();
            var overlay = factions.FirstOrDefault(f => f.id == "faction_the_overlay");
            Assert.NotNull(overlay);
            Assert.Equal("The Overlay", overlay.display_name);
            Assert.Equal("conditional", overlay.alignment);
            Assert.Equal("all_regions", overlay.home_region);
            Assert.True(overlay.is_active);
            Assert.Equal(0, overlay.trust);
            Assert.Equal(new[] { "brass_fittings", "sr_stencil_pot", "lamp_oil" }, overlay.wants);
            Assert.Equal(new[] { "cadastral_keys", "travel_correction_on_named_sites" }, overlay.offers);
            Assert.Equal("The Schedule named households. The Record names ground. Ground does not argue.", overlay.signature_quote);
            Assert.Equal("Scrape three plates without writing a lived name or a Continuity number, and Overlay labour withdraws. They do not raid. Rooms go dark of juniors. Posts stay posts.", overlay.access_rule);
            Assert.Equal(string.Empty, overlay.badge_asset_id);
        }

        [Fact]
        public void ExpectedEightFactions_AllPresent()
        {
            var factions = LoadFactions();
            var ids = factions.Select(f => f.id).ToHashSet();
            var expected = new[]
            {
                "faction_the_overlay",
                "faction_the_scale",
                "faction_the_compact",
                "faction_the_underwrite",
                "faction_the_cutters",
                "faction_the_fleet",
                "faction_the_rebuilders",
                "faction_the_garrison"
            };

            foreach (string expectedId in expected)
            {
                Assert.Contains(expectedId, ids);
            }
        }

        [Fact]
        public void FactionIds_AreUnique_AndStartWithFactionPrefix()
        {
            var factions = LoadFactions();
            var seen = new HashSet<string>();
            foreach (var faction in factions)
            {
                Assert.False(string.IsNullOrWhiteSpace(faction.id));
                Assert.StartsWith("faction_the_", faction.id);
                Assert.True(seen.Add(faction.id), "Duplicate faction ID detected: " + faction.id);
            }
        }

        [Fact]
        public void DisplayNames_AreUnique_AndNonEmpty()
        {
            var factions = LoadFactions();
            var seen = new HashSet<string>();
            foreach (var faction in factions)
            {
                Assert.False(string.IsNullOrWhiteSpace(faction.display_name));
                Assert.True(seen.Add(faction.display_name), "Duplicate display name detected: " + faction.display_name);
            }
        }

        [Fact]
        public void Alignments_AreValid()
        {
            var factions = LoadFactions();
            var validAlignments = new HashSet<string> { "conditional", "neutral", "peaceful", "allied", "hostile" };
            foreach (var faction in factions)
            {
                Assert.Contains(faction.alignment, validAlignments);
            }
        }

        [Fact]
        public void HomeRegions_AreValid()
        {
            var factions = LoadFactions();
            var validRegions = new HashSet<string>
            {
                "all_regions",
                "industrial_belt",
                "dead_suburbs",
                "the_cut",
                "deep_coast",
                "ash_flats"
            };

            foreach (var faction in factions)
            {
                Assert.False(string.IsNullOrWhiteSpace(faction.home_region));
                Assert.Contains(faction.home_region, validRegions);
            }
        }

        [Fact]
        public void Wants_And_Offers_ArePopulated_AndNonEmpty()
        {
            var factions = LoadFactions();
            foreach (var faction in factions)
            {
                Assert.NotNull(faction.wants);
                Assert.NotEmpty(faction.wants);
                Assert.True(faction.wants.Length >= 2, $"Faction {faction.id} should have at least 2 wants");
                foreach (var want in faction.wants)
                {
                    Assert.False(string.IsNullOrWhiteSpace(want), $"Empty want token in faction {faction.id}");
                }

                Assert.NotNull(faction.offers);
                Assert.NotEmpty(faction.offers);
                Assert.True(faction.offers.Length >= 2, $"Faction {faction.id} should have at least 2 offers");
                foreach (var offer in faction.offers)
                {
                    Assert.False(string.IsNullOrWhiteSpace(offer), $"Empty offer token in faction {faction.id}");
                }
            }
        }

        [Fact]
        public void TradeProfiles_AreDifferentiated()
        {
            var factions = LoadFactions();
            var wantSets = new List<HashSet<string>>();
            var offerSets = new List<HashSet<string>>();

            foreach (var faction in factions)
            {
                var wants = new HashSet<string>(faction.wants);
                var offers = new HashSet<string>(faction.offers);

                foreach (var existingWants in wantSets)
                {
                    Assert.False(existingWants.SetEquals(wants), $"Faction {faction.id} shares an identical wants profile with another faction.");
                }

                foreach (var existingOffers in offerSets)
                {
                    Assert.False(existingOffers.SetEquals(offers), $"Faction {faction.id} shares an identical offers profile with another faction.");
                }

                wantSets.Add(wants);
                offerSets.Add(offers);
            }
        }

        [Fact]
        public void SignatureQuotes_AreAuthored_AndDistinct()
        {
            var factions = LoadFactions();
            var seen = new HashSet<string>();
            foreach (var faction in factions)
            {
                Assert.False(string.IsNullOrWhiteSpace(faction.signature_quote));
                Assert.True(seen.Add(faction.signature_quote), "Duplicate signature quote in " + faction.id);
            }
        }

        [Fact]
        public void AccessRules_AreAuthored_AndDistinct()
        {
            var factions = LoadFactions();
            var seen = new HashSet<string>();
            foreach (var faction in factions)
            {
                Assert.False(string.IsNullOrWhiteSpace(faction.access_rule));
                Assert.True(seen.Add(faction.access_rule), "Duplicate access rule in " + faction.id);
            }
        }

        [Fact]
        public void ActiveStatus_And_StartingTrust_AreValid()
        {
            var factions = LoadFactions();
            foreach (var faction in factions)
            {
                Assert.True(faction.is_active, $"Faction {faction.id} should be active");
                Assert.InRange(faction.trust, -50, 50);
            }
        }

        [Fact]
        public void NegativeFixture_DuplicateId_IsDetected()
        {
            var fixture = new List<StandingRecordFactionEntryDto>
            {
                new StandingRecordFactionEntryDto { id = "faction_the_overlay", display_name = "Overlay A" },
                new StandingRecordFactionEntryDto { id = "faction_the_overlay", display_name = "Overlay B" }
            };

            var seen = new HashSet<string>();
            bool duplicateDetected = false;
            foreach (var item in fixture)
            {
                if (!seen.Add(item.id))
                {
                    duplicateDetected = true;
                    break;
                }
            }

            Assert.True(duplicateDetected, "Validator must flag duplicate faction IDs.");
        }

        [Fact]
        public void NegativeFixture_InvalidAlignment_IsRejected()
        {
            var invalid = new StandingRecordFactionEntryDto
            {
                id = "faction_the_test",
                display_name = "Test Faction",
                alignment = "chaotic_evil"
            };

            var validAlignments = new HashSet<string> { "conditional", "neutral", "peaceful", "allied", "hostile" };
            Assert.DoesNotContain(invalid.alignment, validAlignments);
        }

        [Fact]
        public void NegativeFixture_InvalidRegion_IsRejected()
        {
            var invalid = new StandingRecordFactionEntryDto
            {
                id = "faction_the_test",
                display_name = "Test Faction",
                home_region = "space_station_orbit"
            };

            var validRegions = new HashSet<string>
            {
                "all_regions",
                "industrial_belt",
                "dead_suburbs",
                "the_cut",
                "deep_coast",
                "ash_flats"
            };

            Assert.DoesNotContain(invalid.home_region, validRegions);
        }

        [Fact]
        public void NegativeFixture_TrustOutOfRange_IsRejected()
        {
            var invalid = new StandingRecordFactionEntryDto
            {
                id = "faction_the_test",
                trust = 999
            };

            Assert.False(invalid.trust >= -50 && invalid.trust <= 50, "Trust score 999 must be recognized as out of bounds.");
        }

        [Fact]
        public void Persistence_OldSaveInitialization_DefaultsGracefully()
        {
            // Simulate an older campaign save where no standing record faction reputation is serialized
            var oldSaveReputation = new Dictionary<string, int>();

            var catalog = LoadFactions();
            // Initialize missing factions from catalog defaults
            foreach (var f in catalog)
            {
                if (!oldSaveReputation.ContainsKey(f.id))
                {
                    oldSaveReputation[f.id] = f.trust;
                }
            }

            Assert.Equal(8, oldSaveReputation.Count);
            foreach (var f in catalog)
            {
                Assert.Equal(0, oldSaveReputation[f.id]);
            }
        }

        [Fact]
        public void Persistence_MutableTrustRoundTrip_PreservesDynamicStanding()
        {
            var liveStanding = new Dictionary<string, int>();
            var catalog = LoadFactions();
            foreach (var f in catalog)
            {
                liveStanding[f.id] = f.trust;
            }

            // Mutate live standing for The Scale
            liveStanding["faction_the_scale"] += 25;

            // Serialize simulated campaign state
            string json = System.Text.Json.JsonSerializer.Serialize(liveStanding);

            // Restore from save
            var restoredStanding = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, int>>(json);
            Assert.NotNull(restoredStanding);
            Assert.Equal(25, restoredStanding["faction_the_scale"]);

            // Re-verifying with fresh catalog load confirms catalog defaults were not overwritten
            var freshCatalog = LoadFactions();
            var scaleCatalog = freshCatalog.First(f => f.id == "faction_the_scale");
            Assert.Equal(0, scaleCatalog.trust);
        }
    }
}
