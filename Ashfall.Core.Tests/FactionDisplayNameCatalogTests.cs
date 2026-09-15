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

        [Fact]
        public void Resolve_KnownFactionMappingTable_ReturnsLoreNames()
        {
            var mappings = new[]
            {
                (Id: "iron_garrison", Expected: "The Iron Garrison"),
                (Id: "warlords_sector_4", Expected: "The Warlords of Sector 4"),
                (Id: "faction_central_garrison", Expected: "The Central Garrison"),
                (Id: "faction_railway_guild", Expected: "The Railway Guild"),
                (Id: "faction_hydro_barons", Expected: "The Hydro Barons"),
                (Id: "faction_ordnance_foundry", Expected: "The Ordnance Foundry"),
                (Id: "faction_supply_corps", Expected: "The Supply Corps"),
                (Id: "faction_ash_sign", Expected: "The Ash Sign"),
                (Id: "cult_of_ash_sign", Expected: "The Cult of the Ash Sign"),
                (Id: "faction_black_ops", Expected: "Black Ops (Ex-Military Rebels)"),
                (Id: "faction_scavengers", Expected: "The Scavengers"),
                (Id: "faction_penal_battalion", Expected: "The Penal Battalion")
            };

            // Ensure catalog is loaded
            string path = Path.Combine(_dataDir, "faction_lore.json");
            if (File.Exists(path))
                FactionDisplayNameCatalog.LoadFromJson(File.ReadAllText(path));

            var failures = new List<string>();
            foreach (var mapping in mappings)
            {
                string result = FactionDisplayNameCatalog.Resolve(mapping.Id);
                if (!string.Equals(mapping.Expected, result, StringComparison.Ordinal))
                    failures.Add($"{mapping.Id}: expected '{mapping.Expected}', got '{result}'");
            }

            Assert.True(failures.Count == 0, string.Join(Environment.NewLine, failures));
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
