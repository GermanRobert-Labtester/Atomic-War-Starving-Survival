using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.UI;

namespace Ashfall.Core.Tests
{
    public sealed class CrossingFactionExpansionTests
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

        private static CrossingCatalog LoadCatalog()
        {
            string dataDir = ResolveDataDir();
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var loader = new CrossingCatalogLoader(files, json);
            return loader.Load(dataDir);
        }

        [Fact]
        public void Catalog_LoadsSuccessfully_ContainsExactEightFactions()
        {
            var catalog = LoadCatalog();
            Assert.NotNull(catalog);
            Assert.Equal(8, catalog.Factions.Count);
        }

        [Fact]
        public void BaselineThreeFactions_PreservedVerbatim()
        {
            var catalog = LoadCatalog();

            // 1. The Scale
            var scale = catalog.GetFaction(CrossingIds.FactionScale);
            Assert.NotNull(scale);
            Assert.Equal("The Scale", scale.display_name);
            Assert.Equal("conditional", scale.alignment);
            Assert.Equal(CrossingIds.Region, scale.home_region);
            Assert.True(scale.is_active);
            Assert.Equal(0, scale.trust);
            Assert.Equal(new[] { "trade_goods" }, scale.wants);
            Assert.Equal(new[] { "stallrow_trade_access", "verification" }, scale.offers);
            Assert.Equal("The brass scales do not lie, and the numbers have no conscience. What people infer from their poverty is not my problem.", scale.signature_quote);
            Assert.Equal("An agonizingly precise weigh-in is the price of doing business at Stallrow. Contest a true reading without cause, and the market stays open—but your name goes on the slate.", scale.access_rule);
            Assert.Equal(string.Empty, scale.badge_asset_id);

            // 2. The Underwrite
            var underwrite = catalog.GetFaction(CrossingIds.FactionUnderwrite);
            Assert.NotNull(underwrite);
            Assert.Equal("The Underwrite", underwrite.display_name);
            Assert.Equal("conditional", underwrite.alignment);
            Assert.Equal(CrossingIds.Region, underwrite.home_region);
            Assert.True(underwrite.is_active);
            Assert.Equal(0, underwrite.trust);
            Assert.Equal(new[] { "pledged_goods" }, underwrite.wants);
            Assert.Equal(new[] { "seed_stock", "covered_loss", "favour_bank" }, underwrite.offers);
            Assert.Equal("Read it twice, under the sodium glare. I'll say it twice. After the second time, there is only the ink, and the debt it binds you to.", underwrite.signature_quote);
            Assert.Equal("Their 'help' is genuine, offered against a plainly named, brutal forfeit—a child's labor, a pound of flesh, a year of servitude. Sign, negotiate, or walk away; no hidden clause survives a second reading.", underwrite.access_rule);
            Assert.Equal(string.Empty, underwrite.badge_asset_id);

            // 3. The Compact
            var compact = catalog.GetFaction(CrossingIds.FactionCompact);
            Assert.NotNull(compact);
            Assert.Equal("The Compact", compact.display_name);
            Assert.Equal("peaceful", compact.alignment);
            Assert.Equal(CrossingIds.Region, compact.home_region);
            Assert.True(compact.is_active);
            Assert.Equal(0, compact.trust);
            Assert.Equal(new[] { "signatories" }, compact.wants);
            Assert.Equal(new[] { "charter_draft", "ratification" }, compact.offers);
            Assert.Equal("There is a document now, stained with ash and thumbprints. Let them argue with the paper instead of each other's throats.", compact.signature_quote);
            Assert.Equal("Sign the blood-flecked draft and you are on the record. Perrin will not ratify a clause he knows will break a man—but he has not yet seen every way the words can be twisted.", compact.access_rule);
            Assert.Equal(string.Empty, compact.badge_asset_id);
        }

        [Fact]
        public void NewFiveFactions_ArePresentWithExpectedAttributes()
        {
            var catalog = LoadCatalog();

            // 4. The Lamplighters
            var lamplighters = catalog.GetFaction(CrossingIds.FactionLamplighters);
            Assert.NotNull(lamplighters);
            Assert.Equal("The Lamplighters", lamplighters.display_name);
            Assert.Equal("conditional", lamplighters.alignment);
            Assert.Equal(CrossingIds.Region, lamplighters.home_region);
            Assert.True(lamplighters.is_active);
            Assert.Equal(0, lamplighters.trust);
            Assert.Contains("fuel_stores", lamplighters.wants);
            Assert.Contains("street_lighting", lamplighters.offers);
            Assert.False(string.IsNullOrWhiteSpace(lamplighters.signature_quote));
            Assert.False(string.IsNullOrWhiteSpace(lamplighters.access_rule));

            // 5. The Granary Wardens
            var granary = catalog.GetFaction(CrossingIds.FactionGranaryWardens);
            Assert.NotNull(granary);
            Assert.Equal("The Granary Wardens", granary.display_name);
            Assert.Equal("conditional", granary.alignment);
            Assert.Equal(CrossingIds.Region, granary.home_region);
            Assert.True(granary.is_active);
            Assert.Equal(0, granary.trust);
            Assert.Contains("staple_grain", granary.wants);
            Assert.Contains("ration_distribution", granary.offers);
            Assert.False(string.IsNullOrWhiteSpace(granary.signature_quote));
            Assert.False(string.IsNullOrWhiteSpace(granary.access_rule));

            // 6. The Water Committee
            var water = catalog.GetFaction(CrossingIds.FactionWaterCommittee);
            Assert.NotNull(water);
            Assert.Equal("The Water Committee", water.display_name);
            Assert.Equal("conditional", water.alignment);
            Assert.Equal(CrossingIds.Region, water.home_region);
            Assert.True(water.is_active);
            Assert.Equal(0, water.trust);
            Assert.Contains("filter_media", water.wants);
            Assert.Contains("clean_water_rights", water.offers);
            Assert.False(string.IsNullOrWhiteSpace(water.signature_quote));
            Assert.False(string.IsNullOrWhiteSpace(water.access_rule));

            // 7. The Quarantine Post
            var quarantine = catalog.GetFaction(CrossingIds.FactionQuarantinePost);
            Assert.NotNull(quarantine);
            Assert.Equal("The Quarantine Post", quarantine.display_name);
            Assert.Equal("neutral", quarantine.alignment);
            Assert.Equal(CrossingIds.Region, quarantine.home_region);
            Assert.True(quarantine.is_active);
            Assert.Equal(0, quarantine.trust);
            Assert.Contains("medical_tinctures", quarantine.wants);
            Assert.Contains("gate_health_screening", quarantine.offers);
            Assert.False(string.IsNullOrWhiteSpace(quarantine.signature_quote));
            Assert.False(string.IsNullOrWhiteSpace(quarantine.access_rule));

            // 8. The Smugglers' Court
            var smugglers = catalog.GetFaction(CrossingIds.FactionSmugglersCourt);
            Assert.NotNull(smugglers);
            Assert.Equal("The Smugglers' Court", smugglers.display_name);
            Assert.Equal("conditional", smugglers.alignment);
            Assert.Equal(CrossingIds.Region, smugglers.home_region);
            Assert.True(smugglers.is_active);
            Assert.Equal(0, smugglers.trust);
            Assert.Contains("unchartered_salvage", smugglers.wants);
            Assert.Contains("off_ledger_trade", smugglers.offers);
            Assert.False(string.IsNullOrWhiteSpace(smugglers.signature_quote));
            Assert.False(string.IsNullOrWhiteSpace(smugglers.access_rule));
        }

        [Fact]
        public void FactionIds_AreUnique_AndStartWithPrefix()
        {
            var catalog = LoadCatalog();
            var seen = new HashSet<string>(StringComparer.Ordinal);

            foreach (var f in catalog.Factions)
            {
                Assert.NotNull(f.id);
                Assert.StartsWith("faction_the_", f.id);
                Assert.DoesNotContain(" ", f.id);
                Assert.True(seen.Add(f.id), $"Duplicate faction ID '{f.id}' found.");
            }

            Assert.Equal(8, seen.Count);
        }

        [Fact]
        public void DisplayNames_AreNonEmpty_AndDistinct()
        {
            var catalog = LoadCatalog();
            var names = new HashSet<string>(StringComparer.Ordinal);

            foreach (var f in catalog.Factions)
            {
                Assert.False(string.IsNullOrWhiteSpace(f.display_name));
                Assert.True(names.Add(f.display_name), $"Duplicate display name '{f.display_name}' found.");
            }

            Assert.Equal(8, names.Count);
        }

        [Fact]
        public void Alignments_AreValid()
        {
            var catalog = LoadCatalog();
            var validAlignments = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "conditional", "peaceful", "neutral", "allied", "hostile"
            };

            foreach (var f in catalog.Factions)
            {
                Assert.Contains(f.alignment, validAlignments);
            }
        }

        [Fact]
        public void HomeRegions_AreValidCrossingRegion()
        {
            var catalog = LoadCatalog();
            foreach (var f in catalog.Factions)
            {
                Assert.Equal(CrossingIds.Region, f.home_region);
            }
        }

        [Fact]
        public void IsActive_IsTrueForAll()
        {
            var catalog = LoadCatalog();
            foreach (var f in catalog.Factions)
            {
                Assert.True(f.is_active, $"Faction '{f.id}' should be active.");
            }
        }

        [Fact]
        public void Trust_ValuesAreWithinValidRange()
        {
            var catalog = LoadCatalog();
            foreach (var f in catalog.Factions)
            {
                Assert.InRange(f.trust, -50f, 50f);
            }
        }

        [Fact]
        public void Wants_AreNonEmptyArrays_WithValidNonEmptyItems()
        {
            var catalog = LoadCatalog();
            foreach (var f in catalog.Factions)
            {
                Assert.NotNull(f.wants);
                Assert.NotEmpty(f.wants);
                foreach (string want in f.wants)
                {
                    Assert.False(string.IsNullOrWhiteSpace(want));
                    Assert.Equal(want.ToLowerInvariant(), want);
                }
            }
        }

        [Fact]
        public void Offers_AreNonEmptyArrays_WithValidNonEmptyItems()
        {
            var catalog = LoadCatalog();
            foreach (var f in catalog.Factions)
            {
                Assert.NotNull(f.offers);
                Assert.NotEmpty(f.offers);
                foreach (string offer in f.offers)
                {
                    Assert.False(string.IsNullOrWhiteSpace(offer));
                    Assert.Equal(offer.ToLowerInvariant(), offer);
                }
            }
        }

        [Fact]
        public void SignatureQuotes_AreNonEmpty_SingleSentences()
        {
            var catalog = LoadCatalog();
            foreach (var f in catalog.Factions)
            {
                Assert.False(string.IsNullOrWhiteSpace(f.signature_quote));
                string quote = f.signature_quote.Trim();
                Assert.True(quote.EndsWith(".") || quote.EndsWith("!") || quote.EndsWith("?"),
                    $"Quote for '{f.id}' does not end with sentence-final punctuation: {quote}");
            }
        }

        [Fact]
        public void AccessRules_AreNonEmpty_AndSubstantive()
        {
            var catalog = LoadCatalog();
            foreach (var f in catalog.Factions)
            {
                Assert.False(string.IsNullOrWhiteSpace(f.access_rule));
                Assert.True(f.access_rule.Length >= 40,
                    $"Access rule for '{f.id}' is suspiciously short ({f.access_rule.Length} chars).");
            }
        }

        [Fact]
        public void TradeProfiles_AreDistinctAcrossAllEight()
        {
            var catalog = LoadCatalog();
            var wantsProfiles = new HashSet<string>(StringComparer.Ordinal);
            var offersProfiles = new HashSet<string>(StringComparer.Ordinal);

            foreach (var f in catalog.Factions)
            {
                string sortedWants = string.Join(",", f.wants.OrderBy(w => w, StringComparer.Ordinal));
                string sortedOffers = string.Join(",", f.offers.OrderBy(o => o, StringComparer.Ordinal));

                Assert.True(wantsProfiles.Add(sortedWants),
                    $"Duplicate wants profile '{sortedWants}' in faction '{f.id}'.");
                Assert.True(offersProfiles.Add(sortedOffers),
                    $"Duplicate offers profile '{sortedOffers}' in faction '{f.id}'.");
            }

            Assert.Equal(8, wantsProfiles.Count);
            Assert.Equal(8, offersProfiles.Count);
        }

        [Fact]
        public void CrossingIds_ConstantsMatchDataCatalog()
        {
            var catalog = LoadCatalog();
            string[] expectedIds =
            {
                CrossingIds.FactionScale,
                CrossingIds.FactionUnderwrite,
                CrossingIds.FactionCompact,
                CrossingIds.FactionLamplighters,
                CrossingIds.FactionGranaryWardens,
                CrossingIds.FactionWaterCommittee,
                CrossingIds.FactionQuarantinePost,
                CrossingIds.FactionSmugglersCourt
            };

            foreach (string expectedId in expectedIds)
            {
                var faction = catalog.GetFaction(expectedId);
                Assert.NotNull(faction);
                Assert.Equal(expectedId, faction.id);
            }
        }

        [Fact]
        public void FactionIconCatalog_ResolvesOrFallsBackSafely()
        {
            var catalog = LoadCatalog();
            foreach (var f in catalog.Factions)
            {
                string iconPath = FactionIconCatalog.Resolve(f.id);
                Assert.False(string.IsNullOrWhiteSpace(iconPath));
                Assert.StartsWith("assets/ui/Icons/", iconPath);
                Assert.EndsWith(".png", iconPath);
            }
        }
    }
}
