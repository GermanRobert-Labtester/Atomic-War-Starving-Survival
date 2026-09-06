// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;
using Ashfall.Core.Factions;

namespace Ashfall.Core.Tests
{
    public class FactionDisplayNameCatalogTests
    {
        private readonly string _dataDir;

        public FactionDisplayNameCatalogTests()
        {
            _dataDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "StreamingAssets", "Data");
            if (!Directory.Exists(_dataDir))
                _dataDir = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data"));
        }

        [Theory]
        [InlineData("iron_garrison", "The Iron Garrison")]
        [InlineData("warlords_sector_4", "The Warlords of Sector 4")]
        [InlineData("faction_central_garrison", "The Central Garrison")]
        [InlineData("faction_railway_guild", "The Railway Guild")]
        [InlineData("faction_hydro_barons", "The Hydro Barons")]
        [InlineData("faction_ordnance_foundry", "The Ordnance Foundry")]
        [InlineData("faction_supply_corps", "The Supply Corps")]
        [InlineData("faction_ash_sign", "The Ash Sign")]
        [InlineData("cult_of_ash_sign", "The Cult of the Ash Sign")]
        [InlineData("faction_black_ops", "Black Ops (Ex-Military Rebels)")]
        [InlineData("faction_scavengers", "The Scavengers")]
        [InlineData("faction_penal_battalion", "The Penal Battalion")]
        public void Resolve_KnownFaction_ReturnsLoreName(string factionId, string expected)
        {
            // Ensure catalog is loaded
            string path = Path.Combine(_dataDir, "faction_lore.json");
            if (File.Exists(path))
                FactionDisplayNameCatalog.LoadFromJson(File.ReadAllText(path));

            string result = FactionDisplayNameCatalog.Resolve(factionId);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Resolve_UnknownFaction_FallsBackToHumanized()
        {
            string result = FactionDisplayNameCatalog.Resolve("faction_some_unknown_thing");
            Assert.Equal("Some Unknown Thing", result);
        }

        [Fact]
        public void Resolve_EmptyOrNull_ReturnsDefault()
        {
            Assert.Equal("Unknown Faction", FactionDisplayNameCatalog.Resolve(null));
            Assert.Equal("Unknown Faction", FactionDisplayNameCatalog.Resolve(""));
            Assert.Equal("Unknown Faction", FactionDisplayNameCatalog.Resolve("  "));
        }

        [Fact]
        public void HasLoreName_ReturnsTrueForKnown()
        {
            string path = Path.Combine(_dataDir, "faction_lore.json");
            if (File.Exists(path))
                FactionDisplayNameCatalog.LoadFromJson(File.ReadAllText(path));

            Assert.True(FactionDisplayNameCatalog.HasLoreName("iron_garrison"));
            Assert.True(FactionDisplayNameCatalog.HasLoreName("faction_central_garrison"));
            Assert.False(FactionDisplayNameCatalog.HasLoreName("faction_nonexistent_xyz"));
        }

        [Fact]
        public void LoadFromJson_MultipleCallsAreIdempotent()
        {
            string path = Path.Combine(_dataDir, "faction_lore.json");
            if (!File.Exists(path)) return;

            string json = File.ReadAllText(path);
            FactionDisplayNameCatalog.LoadFromJson(json);
            FactionDisplayNameCatalog.LoadFromJson(json); // second call should be no-op

            Assert.Equal("The Iron Garrison", FactionDisplayNameCatalog.Resolve("iron_garrison"));
        }

        [Fact]
        public void Resolve_WithSystemsPrefix_StripsAndResolves()
        {
            // "faction_central_garrison" should resolve to "The Central Garrison"
            // even though the lore entry uses "central_garrison" as the key
            string path = Path.Combine(_dataDir, "faction_lore.json");
            if (File.Exists(path))
                FactionDisplayNameCatalog.LoadFromJson(File.ReadAllText(path));

            string result = FactionDisplayNameCatalog.Resolve("faction_central_garrison");
            Assert.Equal("The Central Garrison", result);
        }
    }
}
