using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Ashfall.Core;
using Ashfall.Core.Economy;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Plan 61 — trade scenario catalog expansion gates (3 → 15).
    /// Data-contract, reference-integrity, fairness, price-consistency,
    /// differentiation, reachability, and determinism gates for the expanded
    /// catalog. The CatalogIntegrityValidator does not scan this file, so these
    /// tests carry the whole validation load.
    /// </summary>
    public class TradeScreenScenarioCatalogTests
    {
        private static string DataPath(string fileName)
        {
            string path = Path.Combine(AppContext.BaseDirectory, "../../../../Assets/StreamingAssets/Data", fileName);
            if (!File.Exists(path))
            {
                path = Path.Combine(Directory.GetCurrentDirectory(), "Assets/StreamingAssets/Data", fileName);
            }
            return path;
        }

        private static IReadOnlyList<TradeScreenScenario> LoadScenarios()
        {
            return TradeScreenScenarioLoader.LoadFromJson(File.ReadAllText(DataPath("trade_screen_scenarios.json")));
        }

        private static TradeTellEngine LoadTells()
        {
            return TradeTellEngine.LoadFromJson(File.ReadAllText(DataPath("trade_tell_lines.json")));
        }

        private static HashSet<string> LoadItemIds()
        {
            using var doc = JsonDocument.Parse(File.ReadAllText(DataPath("items.json")));
            var ids = new HashSet<string>(StringComparer.Ordinal);
            foreach (var it in doc.RootElement.GetProperty("items").EnumerateArray())
            {
                if (it.TryGetProperty("id", out var idEl) && idEl.ValueKind == JsonValueKind.String)
                {
                    ids.Add(idEl.GetString() ?? string.Empty);
                }
            }
            return ids;
        }

        /// <summary>Canonical faction ID set from faction_radio_corpus.json.</summary>
        private static HashSet<string> LoadFactionIds()
        {
            using var doc = JsonDocument.Parse(File.ReadAllText(DataPath("faction_radio_corpus.json")));
            var ids = new HashSet<string>(StringComparer.Ordinal);
            Collect(doc.RootElement, ids);
            return ids;
        }

        private static void Collect(JsonElement el, HashSet<string> ids)
        {
            switch (el.ValueKind)
            {
                case JsonValueKind.Object:
                    foreach (var p in el.EnumerateObject())
                    {
                        if (p.Name == "faction_id" && p.Value.ValueKind == JsonValueKind.String)
                        {
                            ids.Add(p.Value.GetString() ?? string.Empty);
                        }
                        else
                        {
                            Collect(p.Value, ids);
                        }
                    }
                    break;
                case JsonValueKind.Array:
                    foreach (var item in el.EnumerateArray()) Collect(item, ids);
                    break;
            }
        }

        /// <summary>The twelve Plan 61 scenario IDs mapped to their trader archetype.</summary>
        private static readonly Dictionary<string, string> Plan61Archetypes = new()
        {
            ["last_vials"] = "desperate_survivor",
            ["winter_cart"] = "desperate_survivor",
            ["depot_window"] = "faction_quartermaster",
            ["emergency_requisition"] = "faction_quartermaster",
            ["back_room_exchange"] = "black_market",
            ["ledgerless_broker"] = "black_market",
            ["long_road_caravan"] = "caravan_merchant",
            ["salvage_caravan"] = "caravan_merchant",
            ["settlement_of_accounts"] = "debt_collector",
            ["crate_lot"] = "bulk_dealer",
            ["border_runner"] = "smuggler",
            ["road_knowledge"] = "refugee_barter",
        };

        // ── Data contract ────────────────────────────────────────────

        [Fact]
        public void Catalog_ContainsExactlyFifteenScenarios()
        {
            Assert.Equal(15, LoadScenarios().Count);
        }

        [Fact]
        public void Catalog_ScenarioIdsAreUniqueSnakeCase()
        {
            var scenarios = LoadScenarios();
            var ids = scenarios.Select(s => s.Id).ToList();
            Assert.Equal(ids.Count, ids.Distinct(StringComparer.Ordinal).Count());
            Assert.All(ids, id =>
            {
                Assert.False(string.IsNullOrWhiteSpace(id));
                Assert.Matches("^[a-z0-9_]+$", id);
            });
        }

        [Fact]
        public void Catalog_AllTwelvePlan61ScenariosPresent()
        {
            var ids = LoadScenarios().Select(s => s.Id).ToHashSet(StringComparer.Ordinal);
            foreach (var expected in Plan61Archetypes.Keys)
            {
                Assert.True(ids.Contains(expected), $"Missing Plan 61 scenario: {expected}");
            }
        }

        [Fact]
        public void Catalog_AllEightArchetypesRepresentedInPlan61Distribution()
        {
            var scenarios = LoadScenarios().Where(s => Plan61Archetypes.ContainsKey(s.Id));
            var counts = new Dictionary<string, int>();
            foreach (var s in scenarios)
            {
                var archetype = Plan61Archetypes[s.Id];
                counts[archetype] = (counts.TryGetValue(archetype, out var c) ? c : 0) + 1;
            }

            Assert.Equal(2, counts["desperate_survivor"]);
            Assert.Equal(2, counts["faction_quartermaster"]);
            Assert.Equal(2, counts["black_market"]);
            Assert.Equal(2, counts["caravan_merchant"]);
            Assert.Equal(1, counts["debt_collector"]);
            Assert.Equal(1, counts["bulk_dealer"]);
            Assert.Equal(1, counts["smuggler"]);
            Assert.Equal(1, counts["refugee_barter"]);
        }

        // ── Reference integrity ──────────────────────────────────────

        [Fact]
        public void Catalog_EveryItemReferenceResolvesToItemsJson()
        {
            var itemIds = LoadItemIds();
            foreach (var s in LoadScenarios())
            {
                foreach (var line in s.PlayerOffers.Concat(s.FactionDemands))
                {
                    Assert.True(itemIds.Contains(line.ItemId),
                        $"Scenario {s.Id} references unknown item '{line.ItemId}'");
                }
                foreach (var band in s.Scarcity)
                {
                    Assert.True(itemIds.Contains(band.ItemId),
                        $"Scenario {s.Id} scarcity references unknown item '{band.ItemId}'");
                }
            }
        }

        [Fact]
        public void Catalog_EveryFactionReferenceResolvesToCanonicalSet()
        {
            var factions = LoadFactionIds();
            foreach (var s in LoadScenarios())
            {
                Assert.True(factions.Contains(s.FactionId),
                    $"Scenario {s.Id} references faction '{s.FactionId}' not present in faction_radio_corpus.json");
            }
        }

        [Fact]
        public void Catalog_ValidStancesMetersAndShockBounds()
        {
            var validStances = new[] { TradeStance.HostileRaid, TradeStance.Rob, TradeStance.Refuse, TradeStance.Trade, TradeStance.ShareIntel };
            foreach (var s in LoadScenarios())
            {
                Assert.Contains(s.Stance, validStances);
                Assert.InRange(s.Trust, -100f, 100f);
                Assert.InRange(s.Aggression, 0f, 1f);
                Assert.True(s.WorldDay >= 1);
                foreach (var shock in s.PriceShocks)
                {
                    Assert.True(float.IsFinite(shock.Multiplier) && shock.Multiplier > 0f,
                        $"Scenario {s.Id} has invalid shock multiplier");
                }
            }
        }

        [Fact]
        public void Catalog_LineDataIsPositiveAndFinite()
        {
            foreach (var s in LoadScenarios())
            {
                foreach (var line in s.PlayerOffers.Concat(s.FactionDemands))
                {
                    Assert.True(line.Quantity > 0, $"Scenario {s.Id}: {line.ItemId} quantity must be positive");
                    Assert.True(float.IsFinite(line.TotalValue) && line.TotalValue > 0f,
                        $"Scenario {s.Id}: {line.ItemId} total value must be positive");
                }
            }
        }

        // ── Price authority: global unit-price consistency ───────────

        [Fact]
        public void Catalog_UnitPricesAreGloballyConsistentPerItem()
        {
            // Anti-arbitrage rule: the same item_id must carry one authored
            // worth everywhere (both edges, all scenarios). Baseline convention
            // (canned_food = 18 in fair_deal and offer_short) generalized.
            var prices = new Dictionary<string, float>(StringComparer.Ordinal);
            foreach (var s in LoadScenarios())
            {
                foreach (var line in s.PlayerOffers.Concat(s.FactionDemands))
                {
                    float perUnit = line.Quantity > 0 ? line.TotalValue / line.Quantity : 0f;
                    if (prices.TryGetValue(line.ItemId, out var known))
                    {
                        Assert.True(Math.Abs(known - perUnit) < 0.01f,
                            $"Item '{line.ItemId}' has inconsistent unit price: {known} vs {perUnit} (scenario {s.Id})");
                    }
                    else
                    {
                        prices[line.ItemId] = perUnit;
                    }
                }
            }
        }

        [Fact]
        public void Catalog_CrossScenarioPriceSpreadsStayWithinAntiArbitrageBand()
        {
            // Even with consistency enforced, guard the band: no item's worth
            // may differ across scenarios (regression guard if data forks).
            var byItem = new Dictionary<string, List<float>>(StringComparer.Ordinal);
            foreach (var s in LoadScenarios())
            {
                foreach (var line in s.PlayerOffers.Concat(s.FactionDemands))
                {
                    float perUnit = line.TotalValue / line.Quantity;
                    if (!byItem.TryGetValue(line.ItemId, out var list))
                    {
                        list = new List<float>();
                        byItem[line.ItemId] = list;
                    }
                    list.Add(perUnit);
                }
            }
            foreach (var pair in byItem)
            {
                var spread = pair.Value.Max() - pair.Value.Min();
                Assert.True(spread < 0.01f,
                    $"Arbitrage risk: '{pair.Key}' worth spreads by {spread} across scenario tables");
            }
        }

        // ── Fairness & confirm contract ──────────────────────────────

        [Fact]
        public void Catalog_ComputedFairnessMatchesDataExpectation()
        {
            foreach (var s in LoadScenarios())
            {
                var binding = TradeScreenScenarioLoader.CreateBinding(s, LoadTells(), new SeededRng(61));
                var vm = binding.ViewModel;
                var expected = s.ExpectedFairness;
                Assert.True(vm.Fairness == expected,
                    $"Scenario {s.Id}: computed fairness {vm.Fairness} != expected {expected} " +
                    $"(player {vm.PlayerOfferValue} vs ask {vm.FactionAskValue})");
            }
        }

        [Fact]
        public void Catalog_ConfirmSucceedsOnlyForFairTradableTables()
        {
            foreach (var s in LoadScenarios())
            {
                bool willTrade = s.Stance == TradeStance.Trade || s.Stance == TradeStance.ShareIntel;
                bool expectedConfirm = willTrade && s.ExpectedFairness == TradeFairness.Fair;
                Assert.True(s.ConfirmSucceeds == expectedConfirm,
                    $"Scenario {s.Id}: confirm_succeeds={s.ConfirmSucceeds} but fairness/stance imply {expectedConfirm}");
            }
        }

        // ── Reachability & tell integration (Plan 62 — live) ─────────

        [Fact]
        public void Catalog_EveryScenarioBindsWithTellAndLegibleState()
        {
            var tells = LoadTells();
            foreach (var s in LoadScenarios())
            {
                var binding = TradeScreenScenarioLoader.CreateBinding(s, tells, new SeededRng(61));
                var vm = binding.ViewModel;
                Assert.True(vm.IsOpen, $"{s.Id} must open");
                Assert.False(string.IsNullOrWhiteSpace(vm.FactionId), $"{s.Id} missing faction");
                Assert.False(string.IsNullOrWhiteSpace(vm.FactionName), $"{s.Id} missing faction name");
                Assert.False(string.IsNullOrWhiteSpace(vm.LeaderName), $"{s.Id} missing leader");
                Assert.False(string.IsNullOrWhiteSpace(vm.StanceTellLine),
                    $"{s.Id}: tell engine must resolve a line for stance {s.Stance} at trust {s.Trust}");
                Assert.False(string.IsNullOrWhiteSpace(vm.RadioTickerLine), $"{s.Id} missing radio ticker");
                Assert.False(string.IsNullOrWhiteSpace(vm.WorldPhaseLabel), $"{s.Id} missing world phase");
            }
        }

        [Fact]
        public void Catalog_TellSelectionIsSeedDeterministic()
        {
            var tells = LoadTells();
            foreach (var s in LoadScenarios())
            {
                var a = TradeScreenScenarioLoader.CreateBinding(s, tells, new SeededRng(2026));
                var b = TradeScreenScenarioLoader.CreateBinding(s, tells, new SeededRng(2026));
                Assert.Equal(a.ViewModel.StanceTellId, b.ViewModel.StanceTellId);
                Assert.Equal(a.ViewModel.StanceTellLine, b.ViewModel.StanceTellLine);
            }
        }

        // ── Empty-table discipline ───────────────────────────────────

        [Fact]
        public void Catalog_OnlyDeliberateEmptyTablesAreEmpty()
        {
            foreach (var s in LoadScenarios())
            {
                bool bare = s.PlayerOffers.Count == 0 && s.FactionDemands.Count == 0 && s.BiologicalOffers.Count == 0;
                if (bare)
                {
                    Assert.Equal(TradeStance.Refuse, s.Stance);
                    Assert.Equal(TradeFairness.EmptyTable, s.ExpectedFairness);
                }
            }
        }

        [Fact]
        public void Catalog_DebtCollectorTableIsDemandsOnly()
        {
            // Settlement of Accounts: the player's edge is bare, the ledger edge
            // is heavy. Presentation of the outstanding obligation — never a
            // shadow debt ledger.
            var s = LoadScenarios().First(x => x.Id == "settlement_of_accounts");
            Assert.Empty(s.PlayerOffers);
            Assert.NotEmpty(s.FactionDemands);
            Assert.Equal(TradeFairness.Short, s.ExpectedFairness);
            Assert.False(s.ConfirmSucceeds);
        }

        // ── Differentiation audit (plan §61B.14) ─────────────────────

        [Fact]
        public void Catalog_EveryScenarioDiffersFromNearestNeighborInTwoDimensions()
        {
            var scenarios = LoadScenarios();
            string Signature(TradeScreenScenario s) =>
                $"{s.Stance}|{Band(s.Trust)}|{string.Join(';', s.PlayerOffers.Concat(s.FactionDemands).Select(l => l.ItemId).OrderBy(x => x, StringComparer.Ordinal))}|" +
                $"{string.Join(';', s.PriceShocks.Select(p => p.Kind).OrderBy(x => x))}|{string.Join(';', s.Scarcity.Select(x => x.ItemId).OrderBy(x => x, StringComparer.Ordinal))}";

            static string Band(float trust) =>
                trust <= -40f ? "hostile" : trust <= 0f ? "wary" : trust <= 40f ? "neutral" : "warm";

            for (int i = 0; i < scenarios.Count; i++)
            {
                for (int j = i + 1; j < scenarios.Count; j++)
                {
                    var a = scenarios[i];
                    var b = scenarios[j];
                    int dims = 0;
                    if (a.Stance != b.Stance) dims++;
                    if (Band(a.Trust) != Band(b.Trust)) dims++;
                    if (Signature(a) != Signature(b) &&
                        (!a.PlayerOffers.Concat(a.FactionDemands).Select(l => l.ItemId).OrderBy(x => x, StringComparer.Ordinal)
                            .SequenceEqual(b.PlayerOffers.Concat(b.FactionDemands).Select(l => l.ItemId).OrderBy(x => x, StringComparer.Ordinal)))) dims++;
                    if (!a.PriceShocks.Select(p => p.Kind).OrderBy(x => x).SequenceEqual(b.PriceShocks.Select(p => p.Kind).OrderBy(x => x))) dims++;
                    if (!a.Scarcity.Select(x => x.ItemId).OrderBy(x => x, StringComparer.Ordinal)
                            .SequenceEqual(b.Scarcity.Select(x => x.ItemId).OrderBy(x => x, StringComparer.Ordinal))) dims++;
                    if (a.ExpectedFairness != b.ExpectedFairness) dims++;

                    Assert.True(dims >= 2,
                        $"Scenarios '{a.Id}' and '{b.Id}' differ in only {dims} dimension(s) — semantic duplication");
                }
            }
        }

        // ── Old-save / characterization protection ───────────────────

        [Fact]
        public void Catalog_OriginalThreeScenariosKeepLockedContracts()
        {
            var byId = LoadScenarios().ToDictionary(s => s.Id, StringComparer.Ordinal);

            var fair = byId["fair_deal"];
            Assert.Equal("scavenger_camp", fair.FactionId);
            Assert.Equal(TradeStance.Trade, fair.Stance);
            Assert.Equal(22f, fair.Trust, 2);
            Assert.Equal(TradeFairness.Fair, fair.ExpectedFairness);
            Assert.True(fair.ConfirmSucceeds);

            var shortOffer = byId["offer_short"];
            Assert.Equal("upland_militia", shortOffer.FactionId);
            Assert.Equal(TradeFairness.Short, shortOffer.ExpectedFairness);
            Assert.False(shortOffer.ConfirmSucceeds);

            var empty = byId["empty_table"];
            Assert.Equal("rot_farmers", empty.FactionId);
            Assert.Equal(TradeStance.Refuse, empty.Stance);
            Assert.Equal(TradeFairness.EmptyTable, empty.ExpectedFairness);
            Assert.False(empty.ConfirmSucceeds);
        }

        [Fact]
        public void Catalog_RootShapeIsPreserved()
        {
            using var doc = JsonDocument.Parse(File.ReadAllText(DataPath("trade_screen_scenarios.json")));
            var root = doc.RootElement;
            Assert.Equal(1, root.GetProperty("schema_version").GetInt32());
            Assert.Equal(1, root.GetProperty("version").GetInt32());
            Assert.True(root.TryGetProperty("scenarios", out var scenarios));
            Assert.Equal(JsonValueKind.Array, scenarios.ValueKind);
            Assert.Equal(15, scenarios.GetArrayLength());
        }
    }
}
