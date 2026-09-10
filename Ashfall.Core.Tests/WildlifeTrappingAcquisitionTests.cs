// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class WildlifeTrappingAcquisitionTests
    {
        private static string FindDataDir()
        {
            var dir = Directory.GetCurrentDirectory();
            for (int i = 0; i < 10; i++)
            {
                string candidate = Path.Combine(dir, "Assets", "StreamingAssets", "Data");
                if (Directory.Exists(candidate)) return candidate;
                candidate = Path.Combine(dir, "assets", "StreamingAssets", "Data");
                if (Directory.Exists(candidate)) return candidate;
                dir = Path.GetDirectoryName(dir) ?? dir;
            }
            return "Assets/StreamingAssets/Data";
        }

        private static WildlifeTrappingCatalog LoadCatalog()
        {
            var fileIO = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var catalog = WildlifeTrappingCatalogLoader.Load(FindDataDir(), fileIO, json);
            Assert.NotNull(catalog);
            return catalog!;
        }

        // ====================================================================
        // Track 3: Expedition Loot Integration
        // ====================================================================

        [Fact]
        public void ExpeditionsLoot_CatalogAuthoritativePresence()
        {
            string dataDir = FindDataDir();
            string expeditionsPath = Path.Combine(dataDir, "expeditions.json");
            Assert.True(File.Exists(expeditionsPath), $"expeditions.json must exist at {expeditionsPath}");

            string expJson = File.ReadAllText(expeditionsPath);
            using var expDoc = JsonDocument.Parse(expJson);

            bool foundFishWaterStation = false;
            bool foundFishMarshHollow = false;
            bool foundBodyGripGarage = false;

            if (expDoc.RootElement.TryGetProperty("expeditions", out var locations))
            {
                foreach (var loc in locations.EnumerateArray())
                {
                    string id = loc.GetProperty("id").GetString() ?? string.Empty;
                    if (loc.TryGetProperty("lootCategories", out var loot))
                    {
                        foreach (var item in loot.EnumerateArray())
                        {
                            string itemStr = item.GetString() ?? string.Empty;
                            if (id == "loc_water_station" && itemStr == "trap_fish") foundFishWaterStation = true;
                            if (id == "loc_marsh_hollow" && itemStr == "trap_fish") foundFishMarshHollow = true;
                            if (id == "ruined_garage" && itemStr == "trap_body_grip") foundBodyGripGarage = true;
                        }
                    }
                }
            }

            Assert.True(foundFishWaterStation, "loc_water_station must contain trap_fish in lootCategories");
            Assert.True(foundFishMarshHollow, "loc_marsh_hollow must contain trap_fish in lootCategories");
            Assert.True(foundBodyGripGarage, "ruined_garage must contain trap_body_grip in lootCategories");

            // Verify items.json presence
            string itemsPath = Path.Combine(dataDir, "items.json");
            string itemsJson = File.ReadAllText(itemsPath);
            using var itemsDoc = JsonDocument.Parse(itemsJson);
            var itemsArray = itemsDoc.RootElement.GetProperty("items");

            bool itemFishFound = false;
            bool itemBodyGripFound = false;
            foreach (var item in itemsArray.EnumerateArray())
            {
                string id = item.GetProperty("id").GetString() ?? string.Empty;
                if (id == "trap_fish")
                {
                    itemFishFound = true;
                    Assert.Equal("Tool", item.GetProperty("type").GetString());
                }
                if (id == "trap_body_grip")
                {
                    itemBodyGripFound = true;
                    Assert.Equal("Tool", item.GetProperty("type").GetString());
                }
            }

            Assert.True(itemFishFound, "items.json must define trap_fish");
            Assert.True(itemBodyGripFound, "items.json must define trap_body_grip");

            // Verify catalog definitions
            var catalog = LoadCatalog();
            Assert.True(catalog.Traps.ContainsKey("trap_fish"));
            Assert.True(catalog.Traps.ContainsKey("trap_body_grip"));
            Assert.Equal("fish_trap", catalog.Traps["trap_fish"].trapType);
            Assert.Equal("body_grip", catalog.Traps["trap_body_grip"].trapType);
        }

        [Fact]
        public void ExpeditionsLoot_AwardToInventory_DeploysAuthoritativeTrap()
        {
            var catalog = LoadCatalog();
            var inv = new Inventory.Inventory();

            // Simulate expedition loot award
            inv.AddById("trap_fish", 1);
            Assert.Equal(1, inv.CountById("trap_fish"));

            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            catalog.RegisterWith(sys);

            var trapDef = catalog.Traps["trap_fish"];

            // Atomic deployment transaction consuming finished item
            var bill = new InventoryBill();
            bill.AddCost("trap_fish", 1);

            using (var tx = inv.BeginTransaction(bill))
            {
                Assert.True(tx.Validation.IsValid);
                var setRes = sys.SetTrap("site_expedition_pond", "bait_fish_guts", "hunter_fisher",
                    trapType: trapDef.trapType, trapId: trapDef.trap_id,
                    checkIntervalDays: trapDef.checkIntervalDays, durabilityChecks: trapDef.durabilityChecks);
                Assert.True(setRes.IsSuccess);
                tx.TryCommit();
            }

            // Inventory item was consumed
            Assert.Equal(0, inv.CountById("trap_fish"));

            // Site deployed with catalog attributes
            var site = sys.State.trapSites.Find(s => s.siteId == "site_expedition_pond");
            Assert.NotNull(site);
            Assert.Equal("trap_fish", site!.trapId);
            Assert.Equal("fish_trap", site.trapType);
            Assert.Equal(trapDef.durabilityChecks, site.remainingDurability);
            Assert.Equal(trapDef.checkIntervalDays, site.checkIntervalDays);
            Assert.False(site.isBroken);
        }

        // ====================================================================
        // Track 4: Trade/Barter Integration
        // ====================================================================

        [Fact]
        public void TradeGoods_AuthoritativeTierMatrix()
        {
            string dataDir = FindDataDir();
            string goodsPath = Path.Combine(dataDir, "economy_goods.json");
            Assert.True(File.Exists(goodsPath), $"economy_goods.json must exist at {goodsPath}");

            string goodsJson = File.ReadAllText(goodsPath);
            using var doc = JsonDocument.Parse(goodsJson);
            var goodsArray = doc.RootElement.GetProperty("goods");

            var prices = new Dictionary<string, float>(StringComparer.Ordinal);
            foreach (var g in goodsArray.EnumerateArray())
            {
                string id = g.GetProperty("id").GetString() ?? string.Empty;
                float price = g.GetProperty("basePrice").GetSingle();
                prices[id] = price;
            }

            // Tier validation
            Assert.True(prices.ContainsKey("trap_improvised_wire"), "Low-tier trap_improvised_wire must be in economy_goods.json");
            Assert.True(prices.ContainsKey("trap_box"), "Mid-tier trap_box must be in economy_goods.json");
            Assert.True(prices.ContainsKey("trap_fish"), "Specialist trap_fish must be in economy_goods.json");

            // Balance note: trap_improvised_wire was re-priced 4.0 -> 10.0 in
            // the data authority (economy_goods.json is canonical).
            Assert.Equal(10.0f, prices["trap_improvised_wire"]);
            Assert.Equal(22.0f, prices["trap_box"]);
            Assert.Equal(16.0f, prices["trap_fish"]);

            // Craft-only traps must NOT be listed in general trade goods
            Assert.False(prices.ContainsKey("trap_snare"), "trap_snare must remain craft-only / specialist");
            Assert.False(prices.ContainsKey("trap_deadfall"), "trap_deadfall must remain craft-only / specialist");
            Assert.False(prices.ContainsKey("trap_pit"), "trap_pit must remain craft-only / specialist");
        }

        [Fact]
        public void TradeGoods_PurchaseToInventory_DeploysAuthoritativeTrap()
        {
            var catalog = LoadCatalog();
            var inv = new Inventory.Inventory();

            // Simulate barter purchase of mid-tier box trap
            inv.AddById("trap_box", 1);
            Assert.Equal(1, inv.CountById("trap_box"));

            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            catalog.RegisterWith(sys);

            var trapDef = catalog.Traps["trap_box"];
            var bill = new InventoryBill();
            bill.AddCost("trap_box", 1);

            using (var tx = inv.BeginTransaction(bill))
            {
                Assert.True(tx.Validation.IsValid);
                var setRes = sys.SetTrap("site_trade_box", "bait_grain_lure", "hunter_dweller",
                    trapType: trapDef.trapType, trapId: trapDef.trap_id,
                    checkIntervalDays: trapDef.checkIntervalDays, durabilityChecks: trapDef.durabilityChecks);
                Assert.True(setRes.IsSuccess);
                tx.TryCommit();
            }

            Assert.Equal(0, inv.CountById("trap_box"));
            var site = sys.State.trapSites.Find(s => s.siteId == "site_trade_box");
            Assert.NotNull(site);
            Assert.Equal("trap_box", site!.trapId);
            Assert.Equal("box", site.trapType);
            Assert.Equal(trapDef.durabilityChecks, site.remainingDurability);
        }

        // ====================================================================
        // Track 5: Bycatch & Narrative Event Hooks
        // ====================================================================

        [Fact]
        public void Bycatch_OnBycatchOccurred_FiresWithExpectedArguments()
        {
            var catalog = LoadCatalog();
            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            catalog.RegisterWith(sys);

            // Create a custom trap definition guaranteeing 100% bycatch
            var customTrap = new TrapDefinition
            {
                trap_id = "trap_guaranteed_bycatch",
                displayName = "Experimental Dual-Snare",
                trapType = "snare",
                checkIntervalDays = 1,
                durabilityChecks = 5,
                bycatchChance = 1.0f,
                bycatchSpecies = new List<BycatchCandidate>
                {
                    new BycatchCandidate { speciesId = "rat", weight = 100f }
                }
            };
            sys.RegisterTrapDefinition(customTrap);

            // Pre-seed catch candidates so primary catch succeeds
            sys.SetTrap("site_bycatch_test", "bait_grain_lure", "hunter_marina",
                customTrap.trapType, customTrap.trap_id, customTrap.checkIntervalDays, customTrap.durabilityChecks);

            bool eventFired = false;
            string firedSiteId = string.Empty;
            string firedTrapId = string.Empty;
            string firedPrimarySpecies = string.Empty;
            string firedBycatchSpecies = string.Empty;
            int firedDay = -1;
            string firedHunterId = string.Empty;

            sys.OnBycatchOccurred += (siteId, trapId, primarySpecies, bycatchSpecies, day, hunterId) =>
            {
                eventFired = true;
                firedSiteId = siteId;
                firedTrapId = trapId;
                firedPrimarySpecies = primarySpecies;
                firedBycatchSpecies = bycatchSpecies;
                firedDay = day;
                firedHunterId = hunterId;
            };

            // Advance days until catch occurs
            for (int d = 2; d <= 10; d++)
            {
                sys.TickDay(d);
                if (sys.State.trapSites[0].hasCatch) break;
            }

            Assert.True(sys.State.trapSites[0].hasCatch, "Trap must produce a primary catch");
            Assert.True(eventFired, "OnBycatchOccurred event must fire when bycatch is rolled");
            Assert.Equal("site_bycatch_test", firedSiteId);
            Assert.Equal("trap_guaranteed_bycatch", firedTrapId);
            Assert.NotEmpty(firedPrimarySpecies);
            Assert.Equal("rat", firedBycatchSpecies);
            Assert.True(firedDay >= 2);
            Assert.Equal("hunter_marina", firedHunterId);
        }

        [Fact]
        public void Bycatch_NarrativeIncident_PresentInEventsJson()
        {
            string dataDir = FindDataDir();
            string eventsPath = Path.Combine(dataDir, "events.json");
            Assert.True(File.Exists(eventsPath), $"events.json must exist at {eventsPath}");

            string eventsJson = File.ReadAllText(eventsPath);
            using var doc = JsonDocument.Parse(eventsJson);
            var eventsArray = doc.RootElement.GetProperty("events");

            bool found = false;
            foreach (var evt in eventsArray.EnumerateArray())
            {
                string id = evt.GetProperty("id").GetString() ?? string.Empty;
                if (id == "event_trapping_bycatch_entanglement")
                {
                    found = true;
                    Assert.True(evt.TryGetProperty("title", out var title) && !string.IsNullOrEmpty(title.GetString()));
                    Assert.True(evt.TryGetProperty("bodyText", out var body) && !string.IsNullOrEmpty(body.GetString()));
                    Assert.True(evt.TryGetProperty("choices", out var choices) && choices.GetArrayLength() >= 2);
                    Assert.True(evt.TryGetProperty("minDay", out var minDay) && minDay.GetInt32() >= 1);
                    break;
                }
            }

            Assert.True(found, "event_trapping_bycatch_entanglement must exist in events.json");
        }
    }
}
