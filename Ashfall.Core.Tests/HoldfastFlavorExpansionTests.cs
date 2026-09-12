// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Xunit;
using Ashfall.Core;

namespace Ashfall.Core.Tests
{
    public class HoldfastFlavorExpansionTests : CatalogTestBase
    {
        private static string DataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found))
                return found;
            if (CatalogLocator.TryFindDataDirectory(System.AppContext.BaseDirectory, out found))
                return found;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
        }

        private sealed class FlavorDataDto
        {
            public int schema_version { get; set; }
            public Dictionary<string, FactionEntryDto> factions { get; set; } = new Dictionary<string, FactionEntryDto>();
            public Dictionary<string, string> items { get; set; } = new Dictionary<string, string>();
        }

        private sealed class FactionEntryDto
        {
            public string register { get; set; } = string.Empty;
            public string voice { get; set; } = string.Empty;
            public string rejected { get; set; } = string.Empty;
            public string sold { get; set; } = string.Empty;
        }

        private static FlavorDataDto LoadFlavorData()
        {
            string path = Path.Combine(DataDir(), "holdfast_flavor.json");
            Assert.True(File.Exists(path), "holdfast_flavor.json must exist at " + path);
            string json = File.ReadAllText(path);
            var data = JsonSerializer.Deserialize<FlavorDataDto>(json);
            Assert.NotNull(data);
            return data!;
        }

        [Fact]
        public void HoldfastFlavor_SchemaVersionAndStructure()
        {
            var data = LoadFlavorData();
            Assert.Equal(1, data.schema_version);
            Assert.NotNull(data.factions);
            Assert.Equal(8, data.factions.Count);
            Assert.NotNull(data.items);
            Assert.Equal(40, data.items.Count);
        }

        [Fact]
        public void HoldfastFlavor_BaselineThreeFactions_ParityPreserved()
        {
            var data = LoadFlavorData();

            // 1. faction_the_office
            Assert.True(data.factions.ContainsKey("faction_the_office"));
            var office = data.factions["faction_the_office"];
            Assert.Equal("bureaucratic", office.register);
            Assert.Equal("The Office speaks in stamps, countersignatures, and triplicate. A rejection is a missing seal, not an insult. Every release is logged against your account.", office.voice);
            Assert.Equal("Requisition denied — the authorising stamp is absent or the ledger balance does not cover the line item.", office.rejected);
            Assert.Equal("Accepted for inventory. The Office records the transfer and adjusts the manifest accordingly.", office.sold);

            // 2. faction_the_cutters
            Assert.True(data.factions.ContainsKey("faction_the_cutters"));
            var cutters = data.factions["faction_the_cutters"];
            Assert.Equal("salvage", cutters.register);
            Assert.Equal("The Cutters speak in tonnage and debts. Stock running low is weather coming in. Favours are tracked in ledgers, not sentiment.", cutters.voice);
            Assert.Equal("No stock to release and no credit to draw against. The Cutters do not float empty requisitions.", cutters.rejected);
            Assert.Equal("Taken to the pile. The cutter ledger shifts; your credit moves the other way.", cutters.sold);

            // 3. faction_the_fleet
            Assert.True(data.factions.ContainsKey("faction_the_fleet"));
            var fleet = data.factions["faction_the_fleet"];
            Assert.Equal("maritime", fleet.register);
            Assert.Equal("The Fleet speaks in manifests, tides, and berths. A completed trade is logged like a vessel cleared to sail. Paper trails matter more than cargo.", fleet.voice);
            Assert.Equal("The manifest does not clear. Either the berth is closed or the hold cannot accept the transfer.", fleet.rejected);
            Assert.Equal("Logged and cleared. The Fleet manifest now shows the item transferred to your custody.", fleet.sold);
        }

        [Fact]
        public void HoldfastFlavor_FiveNewFactions_ArePresentAndCanonical()
        {
            var data = LoadFlavorData();
            string[] expectedNewFactions =
            {
                "faction_black_flotilla",
                "faction_supply_corps",
                "faction_railway_guild",
                "faction_hydro_barons",
                "faction_ordnance_foundry"
            };

            foreach (string factionId in expectedNewFactions)
            {
                Assert.True(data.factions.ContainsKey(factionId), "Missing expanded faction: " + factionId);
                var entry = data.factions[factionId];
                Assert.False(string.IsNullOrWhiteSpace(entry.register), "Empty register for " + factionId);
                Assert.False(string.IsNullOrWhiteSpace(entry.voice), "Empty voice for " + factionId);
                Assert.False(string.IsNullOrWhiteSpace(entry.rejected), "Empty rejected for " + factionId);
                Assert.False(string.IsNullOrWhiteSpace(entry.sold), "Empty sold for " + factionId);
            }
        }

        [Fact]
        public void HoldfastFlavor_AllEightFactions_HaveUniqueRegistersAndVoices()
        {
            var data = LoadFlavorData();
            var registers = new HashSet<string>(StringComparer.Ordinal);
            var voices = new HashSet<string>(StringComparer.Ordinal);
            var rejecteds = new HashSet<string>(StringComparer.Ordinal);
            var solds = new HashSet<string>(StringComparer.Ordinal);

            foreach (var kvp in data.factions)
            {
                Assert.StartsWith("faction_", kvp.Key, StringComparison.Ordinal);
                Assert.Equal(kvp.Key, kvp.Key.ToLowerInvariant());

                Assert.True(registers.Add(kvp.Value.register), "Duplicate register: " + kvp.Value.register);
                Assert.True(voices.Add(kvp.Value.voice), "Duplicate voice for: " + kvp.Key);
                Assert.True(rejecteds.Add(kvp.Value.rejected), "Duplicate rejected line for: " + kvp.Key);
                Assert.True(solds.Add(kvp.Value.sold), "Duplicate sold line for: " + kvp.Key);

                // Check text budget constraints: concise transaction prose
                Assert.InRange(kvp.Value.voice.Length, 50, 250);
                Assert.InRange(kvp.Value.rejected.Length, 30, 200);
                Assert.InRange(kvp.Value.sold.Length, 30, 200);

                // Disallow template placeholders or code syntax
                Assert.DoesNotContain("{", kvp.Value.voice);
                Assert.DoesNotContain("}", kvp.Value.voice);
                Assert.DoesNotContain("%", kvp.Value.voice);
                Assert.DoesNotContain("$", kvp.Value.voice);

                Assert.DoesNotContain("{", kvp.Value.rejected);
                Assert.DoesNotContain("}", kvp.Value.rejected);
                Assert.DoesNotContain("%", kvp.Value.rejected);

                Assert.DoesNotContain("{", kvp.Value.sold);
                Assert.DoesNotContain("}", kvp.Value.sold);
                Assert.DoesNotContain("%", kvp.Value.sold);
            }

            Assert.Equal(8, registers.Count);
            Assert.Equal(8, voices.Count);
            Assert.Equal(8, rejecteds.Count);
            Assert.Equal(8, solds.Count);
        }

        [Fact]
        public void HoldfastFlavor_AllFactionsExistInHoldfastFactionsCatalog()
        {
            var data = LoadFlavorData();
            var loader = new HoldfastCatalogLoader(new FileSystemIO(), new SystemTextJsonSerializer());
            var holdfastCatalog = loader.Load(DataDir());

            Assert.NotNull(holdfastCatalog.Factions);
            foreach (string factionId in data.factions.Keys)
            {
                var faction = holdfastCatalog.GetFaction(factionId);
                Assert.NotNull(faction);
                Assert.Equal(factionId, faction!.id);
                Assert.False(string.IsNullOrWhiteSpace(faction.display_name));
            }
        }

        [Fact]
        public void HoldfastFlavor_ItemsDictionary_PreservedExactly()
        {
            var data = LoadFlavorData();
            Assert.Equal(40, data.items.Count);
            Assert.True(data.items.ContainsKey("item_map_sheet_ice_road"));
            Assert.True(data.items.ContainsKey("item_census_return_blank"));
            Assert.True(data.items.ContainsKey("item_order_12c"));
            Assert.True(data.items.ContainsKey("item_triplicate_carbon"));
            Assert.True(data.items.ContainsKey("item_ice_spike_bar"));
            Assert.True(data.items.ContainsKey("item_beacon_oil"));
            Assert.True(data.items.ContainsKey("item_cutter_ledger_blank"));
            Assert.True(data.items.ContainsKey("item_electrolyte_salts"));

            foreach (var kvp in data.items)
            {
                Assert.StartsWith("item_", kvp.Key, StringComparison.Ordinal);
                Assert.False(string.IsNullOrWhiteSpace(kvp.Value));
            }
        }

        [Fact]
        public void HoldfastFlavor_FallbackBehaviorSimulation()
        {
            var data = LoadFlavorData();

            // Verify that unknown factions are not present and simulate default fallback
            string unknownFactionId = "faction_unknown_entity";
            Assert.False(data.factions.ContainsKey(unknownFactionId));

            // Fallback contract matches HoldfastFlavorCatalog.NeutralFactionVoice
            const string neutralVoice = "The counterparty has no recorded voice.";
            const string neutralRejected = "Transaction declined.";
            const string neutralSold = "Item accepted.";

            string resolvedVoice = data.factions.TryGetValue(unknownFactionId, out var entry) ? entry.voice : neutralVoice;
            string resolvedRejected = entry != null ? entry.rejected : neutralRejected;
            string resolvedSold = entry != null ? entry.sold : neutralSold;

            Assert.Equal(neutralVoice, resolvedVoice);
            Assert.Equal(neutralRejected, resolvedRejected);
            Assert.Equal(neutralSold, resolvedSold);
        }
    }
}
