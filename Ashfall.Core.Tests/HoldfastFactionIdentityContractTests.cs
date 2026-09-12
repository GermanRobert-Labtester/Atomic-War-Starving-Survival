// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Ashfall.Core;
using Ashfall.Core.Factions;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Plan 131 identity boundary tests. The Holdfast trade catalog contains
    /// eight authored faction identities plus the pre-existing Scavengers
    /// compatibility entry. Static catalog trust is not mutable save state.
    /// </summary>
    public sealed class HoldfastFactionIdentityContractTests
    {
        private static string DataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found))
                return found;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found))
                return found;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
        }

        private static HoldfastCatalog LoadCatalog()
            => new HoldfastCatalogLoader(new FileSystemIO(), new SystemTextJsonSerializer()).Load(DataDir());

        private static readonly string[] CanonicalHoldfastFactionIds =
        {
            "faction_the_office",
            "faction_the_cutters",
            "faction_the_fleet",
            "faction_black_flotilla",
            "faction_supply_corps",
            "faction_railway_guild",
            "faction_hydro_barons",
            "faction_ordnance_foundry"
        };

        [Fact]
        public void Roster_PreservesThreeBaselineAndEightAuthoredIdentities()
        {
            var catalog = LoadCatalog();
            Assert.Equal(9, catalog.Factions.Count);

            foreach (string id in CanonicalHoldfastFactionIds)
            {
                var faction = catalog.GetFaction(id);
                Assert.NotNull(faction);
                Assert.Equal(id, faction!.Id);
            }

            // This entry predates the eight-profile flavor expansion and is
            // retained for existing trade/travel content, not promoted into a
            // tenth authored Holdfast identity.
            Assert.NotNull(catalog.GetFaction("faction_scavengers"));
        }

        [Fact]
        public void RosterEntries_HaveValidatedIdentityAndTradeFields()
        {
            var catalog = LoadCatalog();
            var ids = new HashSet<string>(StringComparer.Ordinal);
            var names = new HashSet<string>(StringComparer.Ordinal);
            var validAlignments = new HashSet<string>(StringComparer.Ordinal)
            {
                "conditional", "peaceful", "neutral", "allied", "hostile"
            };

            foreach (var faction in catalog.Factions)
            {
                Assert.True(ids.Add(faction.Id), "Duplicate faction ID: " + faction.Id);
                Assert.Matches("^[a-z0-9_]+$", faction.Id);
                Assert.StartsWith("faction_", faction.Id, StringComparison.Ordinal);
                Assert.False(string.IsNullOrWhiteSpace(faction.DisplayName));
                Assert.True(names.Add(faction.DisplayName), "Duplicate display name: " + faction.DisplayName);
                Assert.Contains(faction.Alignment, validAlignments);
                Assert.False(string.IsNullOrWhiteSpace(faction.HomeRegion));
                Assert.InRange(faction.Trust, -100f, 100f);
                Assert.NotEmpty(faction.Wants);
                Assert.NotEmpty(faction.Offers);
                Assert.All(faction.Wants, value =>
                {
                    Assert.False(string.IsNullOrWhiteSpace(value));
                    Assert.Equal(value, value.ToLowerInvariant());
                });
                Assert.All(faction.Offers, value =>
                {
                    Assert.False(string.IsNullOrWhiteSpace(value));
                    Assert.Equal(value, value.ToLowerInvariant());
                });
                Assert.False(string.IsNullOrWhiteSpace(faction.SignatureQuote));
                Assert.False(string.IsNullOrWhiteSpace(faction.AccessRule));
            }
        }

        [Fact]
        public void FlavorProfiles_AreExactlyTheEightAuthoredIdentities()
        {
            using var document = JsonDocument.Parse(
                File.ReadAllText(Path.Combine(DataDir(), "holdfast_flavor.json")));
            var factions = document.RootElement.GetProperty("factions");
            var actual = factions.EnumerateObject().Select(property => property.Name).ToHashSet(StringComparer.Ordinal);

            Assert.Equal(CanonicalHoldfastFactionIds.Length, actual.Count);
            Assert.Equal(
                CanonicalHoldfastFactionIds.OrderBy(id => id, StringComparer.Ordinal),
                actual.OrderBy(id => id, StringComparer.Ordinal));
            Assert.DoesNotContain("faction_scavengers", actual);
        }

        [Fact]
        public void HoldfastNpcReferences_UseCanonicalFactionIds()
        {
            string dataDir = DataDir();
            var catalog = HoldfastNpcCatalogLoader.Load(
                dataDir,
                new FileSystemIO(),
                new SystemTextJsonSerializer());

            Assert.True(catalog.IsValid);
            foreach (var npc in catalog.All())
            {
                if (string.IsNullOrWhiteSpace(npc.FactionId))
                    continue;

                if (FactionStandingIdResolver.IsKnownFaction(npc.FactionId))
                {
                    string canonical = FactionStandingIdResolver.ToSystemsId(npc.FactionId);
                    Assert.StartsWith("faction_", canonical, StringComparison.Ordinal);
                    Assert.Equal(canonical, npc.FactionId);
                }
            }
        }

        [Fact]
        public void TradeSaveState_DoesNotDuplicateStaticFactionTrust()
        {
            var catalog = LoadCatalog();
            var office = catalog.GetFaction("faction_the_office");
            Assert.NotNull(office);
            Assert.Equal(0f, office!.Trust);

            var session = new HoldfastTradeSession(catalog, 100);
            session.SelectFaction("faction_the_office");
            var state = session.CaptureState();
            string serialized = new SystemTextJsonSerializer().Serialize(state);

            Assert.DoesNotContain("trust", serialized, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("faction_the_office", serialized, StringComparison.Ordinal);
            Assert.Equal(2, state.schemaVersion);
        }

        [Fact]
        public void LegacyHoldfastAliases_DoNotRemainInCanonicalSources()
        {
            string[] sourceFiles =
            {
                Path.Combine(DataDir(), "holdfast_npcs.json"),
                FindProjectFile("Assets/Ashfall.Core/HoldfastFactionsCatalog.cs"),
                FindProjectFile("Assets/Ashfall.Core/Economy/MercenarySystem.cs")
            };

            foreach (string path in sourceFiles)
            {
                string contents = File.ReadAllText(path);
                Assert.DoesNotContain("faction_holdfast_", contents, StringComparison.Ordinal);
            }
        }

        private static string FindProjectFile(string relativePath)
        {
            string start = Directory.GetCurrentDirectory();
            var directory = new DirectoryInfo(start);
            while (directory != null)
            {
                string candidate = Path.Combine(directory.FullName, relativePath);
                if (File.Exists(candidate))
                    return candidate;
                directory = directory.Parent;
            }
            throw new FileNotFoundException("Could not locate project file", relativePath);
        }
    }
}
