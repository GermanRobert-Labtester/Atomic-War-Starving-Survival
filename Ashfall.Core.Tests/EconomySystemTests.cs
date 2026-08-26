using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Economy;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class GoodsCatalogTests
    {
        private static GoodsCatalogLoadResult LoadRaw(string json)
        {
            // Auto-wrap flat arrays in schema envelope.
            string trimmed = json.TrimStart();
            if (trimmed.Length > 0 && trimmed[0] == '[')
                json = @"{""schema_version"": 1, ""goods"": " + json + "}";
            return GoodsCatalogLoader.Load(
                "/tmp", new FakeFileIO(json), new SystemTextJsonSerializer());
        }

        private sealed class FakeFileIO : IFileIO
        {
            private readonly string _content;
            public FakeFileIO(string content) { _content = content; }
            public bool DirectoryExists(string path) => true;
            public bool FileExists(string path) => true;
            public string ReadAllText(string path) => _content;
            public void WriteAllText(string path, string contents) { }
            public string Combine(params string[] parts) => string.Join("/", parts);
        }

        [Fact]
        public void ValidGoods_LoadWithoutErrors()
        {
            var result = LoadRaw(@"[
                { ""id"": ""clean_water"", ""displayName"": ""Water"", ""category"": ""water"",
                  ""basePrice"": 8, ""volatility"": 0.1, ""elasticity"": 1.0, ""stackSize"": 5, ""weightKg"": 1 } ]");
            Assert.False(result.HasErrors);
            Assert.Single(result.Goods);
            Assert.Equal("clean_water", result.Goods[0].id);
        }

        [Fact]
        public void DuplicateIds_AreErrors()
        {
            var result = LoadRaw(@"[
                { ""id"": ""clean_water"", ""displayName"": ""A"", ""category"": ""water"", ""basePrice"": 1 },
                { ""id"": ""clean_water"", ""displayName"": ""B"", ""category"": ""water"", ""basePrice"": 2 } ]");
            Assert.True(result.HasErrors);
            Assert.Contains(result.Errors, e => e.Contains("duplicate id 'clean_water'"));
            Assert.Single(result.Goods); // first wins
        }

        [Fact]
        public void MissingRequiredFields_AreErrors()
        {
            // First-error-wins per entry: the loader reports the first missing
            // required field and stops validating that entry.
            var result = LoadRaw(@"[ { ""id"": ""x"" } ]");
            Assert.True(result.HasErrors);
            Assert.Contains(result.Errors, e => e.Contains("displayName"));
            Assert.Empty(result.Goods);

            // A second entry with displayName but no category reports category.
            var result2 = LoadRaw(@"[ { ""id"": ""x"", ""displayName"": ""X"", ""basePrice"": 1 } ]");
            Assert.True(result2.HasErrors);
            Assert.Contains(result2.Errors, e => e.Contains("category"));
        }

        [Fact]
        public void InvalidRanges_AreErrors()
        {
            Assert.Contains(LoadRaw(@"[{ ""id"": ""x"", ""displayName"": ""X"", ""category"": ""water"", ""basePrice"": 0 }]").Errors,
                e => e.Contains("basePrice"));
            Assert.Contains(LoadRaw(@"[{ ""id"": ""x"", ""displayName"": ""X"", ""category"": ""water"", ""basePrice"": 1, ""volatility"": 2 }]").Errors,
                e => e.Contains("volatility"));
            Assert.Contains(LoadRaw(@"[{ ""id"": ""x"", ""displayName"": ""X"", ""category"": ""water"", ""basePrice"": 1, ""elasticity"": 0 }]").Errors,
                e => e.Contains("elasticity"));
            Assert.Contains(LoadRaw(@"[{ ""id"": ""x"", ""displayName"": ""X"", ""category"": ""water"", ""basePrice"": 1, ""stackSize"": 0 }]").Errors,
                e => e.Contains("stackSize"));
        }

        [Fact]
        public void UnknownCategory_IsError()
        {
            var result = LoadRaw(@"[{ ""id"": ""x"", ""displayName"": ""X"", ""category"": ""alchemy"", ""basePrice"": 1 }]");
            Assert.True(result.HasErrors);
            Assert.Contains(result.Errors, e => e.Contains("unknown category 'alchemy'"));
        }

        [Fact]
        public void NonSnakeCaseId_IsError()
        {
            var result = LoadRaw(@"[{ ""id"": ""Clean Water"", ""displayName"": ""X"", ""category"": ""water"", ""basePrice"": 1 }]");
            Assert.True(result.HasErrors);
            Assert.Contains(result.Errors, e => e.Contains("not snake_case"));
        }

        [Fact]
        public void MalformedJson_IsError()
        {
            var result = LoadRaw("{ not json");
            Assert.True(result.HasErrors);
            Assert.Contains(result.Errors, e => e.Contains("malformed"));
        }

        [Fact]
        public void MissingFile_IsError()
        {
            var result = GoodsCatalogLoader.Load(
                "/nonexistent", new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.True(result.HasErrors);
        }

        [Fact]
        public void Catalog_AllSortedOrdinal()
        {
            var result = LoadRaw(@"[
                { ""id"": ""zeta"", ""displayName"": ""Z"", ""category"": ""water"", ""basePrice"": 1 },
                { ""id"": ""alpha"", ""displayName"": ""A"", ""category"": ""water"", ""basePrice"": 1 } ]");
            var catalog = GoodsCatalogLoader.ToCatalog(result);
            var all = catalog.All();
            Assert.Equal("alpha", all[0].id);
            Assert.Equal("zeta", all[1].id);
        }
    }

    public class MarketSystemTests
    {
        private static GoodsCatalog Catalog(params (string id, float price, float vol, float el)[] goods)
        {
            var result = new GoodsCatalogLoadResult();
            foreach (var (id, price, vol, el) in goods)
            {
                result.Goods.Add(new GoodDefinition
                {
                    id = id,
                    displayName = id,
                    category = "misc",
                    basePrice = price,
                    volatility = vol,
                    elasticity = el
                });
            }
            return GoodsCatalogLoader.ToCatalog(result);
        }

        private static MarketSystem NewMarket(GoodsCatalog? catalog = null)
        {
            var sys = new MarketSystem();
            sys.BindCatalog(catalog ?? Catalog(("water", 8f, 0.1f, 1f)));
            return sys;
        }

        private static SeededRng Rng(int seed) => new SeededRng(seed);

        [Fact]
        public void Price_BoundsInvariant_AllTicks()
        {
            var sys = NewMarket(Catalog(
                ("a", 10f, 0.5f, 2f), ("b", 5f, 0.2f, 0.5f), ("c", 1f, 1f, 1f)));
            for (int day = 1; day <= 200; day++)
            {
                sys.TickDay(day, Rng(day));
                foreach (var good in new[] { "a", "b", "c" })
                {
                    float price = sys.GetPrice(good);
                    float basePrice = sys.FindGood(good).basePrice;
                    Assert.True(price >= basePrice * MarketSystem.PriceFloorFraction,
                        $"day {day} {good}: price {price} below floor");
                    Assert.True(price <= basePrice * MarketSystem.PriceCeilingFraction,
                        $"day {day} {good}: price {price} above ceiling");
                }
            }
        }

        [Fact]
        public void Demand_StaysWithinUnityClamps()
        {
            var sys = NewMarket();
            sys.AdjustDemand("water", 100f);
            Assert.Equal(MarketSystem.MaxDemandMult, sys.GetDemandMultiplier("water"));
            sys.AdjustDemand("water", -1000f);
            Assert.Equal(MarketSystem.MinDemandMult, sys.GetDemandMultiplier("water"));
        }

        [Fact]
        public void DeterministicReplay_SameSeedSameTrajectory()
        {
            var a = NewMarket(Catalog(("w", 8f, 0.3f, 1f), ("s", 3f, 0.2f, 0.8f)));
            var b = NewMarket(Catalog(("w", 8f, 0.3f, 1f), ("s", 3f, 0.2f, 0.8f)));
            for (int day = 1; day <= 30; day++)
            {
                a.TickDay(day, Rng(99));
                b.TickDay(day, Rng(99));
                Assert.Equal(a.GetPrice("w"), b.GetPrice("w"));
                Assert.Equal(a.GetPrice("s"), b.GetPrice("s"));
            }
        }

        [Fact]
        public void DifferentSeeds_Diverge()
        {
            var a = NewMarket(Catalog(("w", 8f, 0.4f, 1f)));
            var b = NewMarket(Catalog(("w", 8f, 0.4f, 1f)));
            for (int day = 1; day <= 20; day++)
            {
                a.TickDay(day, Rng(1));
                b.TickDay(day, Rng(2));
            }
            Assert.NotEqual(a.GetPrice("w"), b.GetPrice("w"));
        }

        [Fact]
        public void SaveLoad_RoundTripEquality()
        {
            var sys = NewMarket(Catalog(("w", 8f, 0.3f, 1f), ("s", 3f, 0.2f, 0.8f)));
            for (int day = 1; day <= 10; day++) sys.TickDay(day, Rng(7));
            sys.Buy("w", 3, 10, "faction_a");
            sys.Sell("s", 5, 11, "market");

            var restored = NewMarket();
            restored.RestoreState(sys.CaptureState());
            restored.BindCatalog(sys.State != null ? Catalog(("w", 8f, 0.3f, 1f), ("s", 3f, 0.2f, 0.8f)) : null);

            Assert.Equal(sys.Day, restored.Day);
            Assert.Equal(sys.TickCount, restored.TickCount);
            Assert.Equal(sys.GetDemandMultiplier("w"), restored.GetDemandMultiplier("w"));
            Assert.Equal(sys.GetPrice("w"), restored.GetPrice("w"));
            Assert.NotNull(restored.State);
            Assert.Equal(sys.State!.ledger.Count, restored.State!.ledger.Count);
        }

        [Fact]
        public void SaveLoad_ResumesIdenticalTrajectory()
        {
            var sys = NewMarket(Catalog(("w", 8f, 0.3f, 1f)));
            for (int day = 1; day <= 10; day++) sys.TickDay(day, Rng(11));

            var restored = NewMarket(Catalog(("w", 8f, 0.3f, 1f)));
            restored.RestoreState(sys.CaptureState());

            for (int day = 11; day <= 20; day++)
            {
                sys.TickDay(day, Rng(11));
                restored.TickDay(day, Rng(11));
                Assert.Equal(sys.GetPrice("w"), restored.GetPrice("w"));
                Assert.Equal(sys.GetDemandMultiplier("w"), restored.GetDemandMultiplier("w"));
            }
        }

        [Fact]
        public void CorruptState_NewerVersionFailsLoudly()
        {
            var sys = NewMarket();
            var future = new MarketState { version = MarketState.Version + 1 };
            Assert.Throws<InvalidOperationException>(() => sys.RestoreState(future));
        }

        [Fact]
        public void CorruptState_OldVersionMigratesPredictably()
        {
            var sys = NewMarket(Catalog(("w", 8f, 0.1f, 1f)));
            var old = new MarketState { version = 0, day = 5, tickCount = 5 }; // no demand rows
            sys.RestoreState(old);
            Assert.Equal(1, sys.State.version);
            Assert.Equal(5, sys.Day);
            Assert.Equal(1f, sys.GetDemandMultiplier("w")); // missing rows read 1.0
        }

        [Fact]
        public void Transactions_BookAtCurrentPrice()
        {
            var sys = NewMarket(Catalog(("w", 8f, 0f, 1f)));
            var t = sys.Buy("w", 3, 1);
            Assert.True(t.Accepted);
            Assert.Equal(24f, t.TotalValue);
            Assert.Equal(8f, t.UnitPrice);
            Assert.Single(sys.State.ledger);
        }

        [Fact]
        public void Transactions_UnknownOrZeroRejected()
        {
            var sys = NewMarket();
            Assert.False(sys.Buy("missing", 1, 1).Accepted);
            Assert.False(sys.Buy("w", 0, 1).Accepted);
            Assert.False(sys.Buy("w", -2, 1).Accepted);
            Assert.Empty(sys.State.ledger);
        }

        [Fact]
        public void Barter_ExchangesEqualValue()
        {
            var sys = NewMarket(Catalog(("w", 10f, 0f, 1f), ("s", 4f, 0f, 1f)));
            var result = sys.Barter("w", 4, "s", 1); // 40 value -> 10 scrap
            Assert.True(result.Accepted);
            Assert.Equal(10, result.Quantity);
            Assert.Equal(2, sys.State.ledger.Count);
            Assert.Equal(40f, sys.State.ledger[0].totalValue);
            Assert.Equal(40f, sys.State.ledger[1].totalValue);
        }

        [Fact]
        public void Barter_TooValuableRejected()
        {
            var sys = NewMarket(Catalog(("w", 1f, 0f, 1f), ("s", 100f, 0f, 1f)));
            Assert.False(sys.Barter("w", 1, "s", 1).Accepted);
            Assert.Empty(sys.State.ledger);
        }

        [Fact]
        public void Barter_NonExactRatio_KeepsEqualLedgerAndReportsRemainder()
        {
            var sys = NewMarket(Catalog(("w", 10f, 0f, 1f), ("s", 3f, 0f, 1f)));
            // Give 4 water = 40 value; scrap at 3 -> 13 whole items (39), remainder 1.
            var result = sys.Barter("w", 4, "s", 1);
            Assert.True(result.Accepted);
            Assert.Equal(13, result.Quantity);
            Assert.Equal(39f, result.TotalValue);
            Assert.Equal(1f, result.RemainderValue);
            Assert.Equal(2, sys.State.ledger.Count);
            // Equal-value invariant: both legs book the SAME exchanged total.
            Assert.Equal(sys.State.ledger[0].totalValue, sys.State.ledger[1].totalValue);
        }

        [Fact]
        public void RestoreState_DeduplicatesDemandRows()
        {
            var sys = NewMarket(Catalog(("w", 8f, 0f, 1f)));
            var corrupt = new MarketState
            {
                version = MarketState.Version,
                demand = new List<DemandEntry>
                {
                    new DemandEntry { itemId = "w", multiplier = 1.5f },
                    new DemandEntry { itemId = "w", multiplier = 2.5f }
                }
            };
            sys.RestoreState(corrupt);
            Assert.Single(sys.State.demand);
            Assert.Equal(1.5f, sys.GetDemandMultiplier("w"));
            // GetDemandMultiplier and IsSuppliesShort now agree (single row).
            Assert.Equal(1.5f >= MarketSystem.ShortageThreshold, sys.IsSuppliesShort());
        }

        [Fact]
        public void TickDay_RaisesEconomyChanged()
        {
            var sys = NewMarket(Catalog(("w", 8f, 0.1f, 1f)));
            int changed = 0;
            sys.OnEconomyChanged += () => changed++;
            sys.TickDay(1, Rng(1));
            Assert.True(changed >= 1);
        }

        [Fact]
        public void CaptureState_ReturnsSnapshotNotLiveState()
        {
            var sys = NewMarket(Catalog(("w", 8f, 0.1f, 1f)));
            sys.AdjustDemand("w", 0.5f);
            var snapshot = sys.CaptureState();
            snapshot.demand[0].multiplier = 99f;
            snapshot.ledger.Add(new LedgerEntry { itemId = "injected" });
            Assert.Equal(1.5f, sys.GetDemandMultiplier("w"));
            Assert.Empty(sys.State.ledger);
        }

        [Fact]
        public void SaveLoad_ChecksumStable()
        {
            var sys = NewMarket(Catalog(("w", 8f, 0.3f, 1f), ("s", 3f, 0.2f, 0.8f)));
            for (int day = 1; day <= 6; day++) sys.TickDay(day, Rng(3));
            sys.Buy("w", 2, 6);
            string before = SaveChecksum.Compute(sys.CaptureState());

            var restored = NewMarket();
            restored.RestoreState(sys.CaptureState());
            string after = SaveChecksum.Compute(restored.CaptureState());
            Assert.Equal(before, after);
        }

        [Fact]
        public void IsSuppliesShort_UnityThresholdParity()
        {
            var sys = NewMarket(Catalog(("w", 8f, 0f, 1f)));
            sys.AdjustDemand("w", 0.4f); // 1.4 >= 1.35
            Assert.True(sys.IsSuppliesShort());
            sys.AdjustDemand("w", -0.1f); // 1.3 < 1.35
            Assert.False(sys.IsSuppliesShort());
        }

        [Fact]
        public void Ledger_Conservation_BarterLegsAreEqualValue()
        {
            // Buy/Sell only record the player side; conservation only applies
            // to barter where both legs are explicitly booked.
            var sys = NewMarket(Catalog(
                ("water", 10f, 0f, 1f),
                ("scrap", 4f, 0f, 1f),
                ("food", 12f, 0f, 1f)));

            sys.Buy("water", 2, 1, "faction_a");
            sys.Sell("scrap", 3, 2, "market");
            sys.Barter("food", 1, "water", 3); // 12 value -> 1 water

            // Barter legs must book the SAME exchanged total.
            var barterEntries = sys.State.ledger.FindAll(e => e.counterparty == "barter");
            Assert.Equal(2, barterEntries.Count);
            Assert.Equal(barterEntries[0].totalValue, barterEntries[1].totalValue);

            // Non-barter entries reflect player-side value change, not conservation.
            Assert.Equal(4, sys.State.ledger.Count); // 2 barter + 1 buy + 1 sell
        }
    }
}

// ── HardcoreEconomyTuning tests ──────────────────────────────────────
public class HardcoreEconomyTuningTests
{
    private const string MinimalValidJson = @"{
        ""version"": 1,
        ""scarcity_tiers"": [
            {""tier"": ""Critical"", ""multiplier"": 2.5, ""day_range_label"": ""Days 1-15"", ""affected_item_ids"": [""water""], ""rationale"": ""test""}
        ],
        ""faction_preferences"": [
            {""faction_id"": ""fac_a"", ""buys_at_premium"": [""ammo""], ""refuses"": [""junk""], ""trade_currency"": ""food""}
        ],
        ""price_shock_rules"": [
            {""kind"": ""PlumePassing"", ""multiplier"": 1.8, ""duration_days"": 3, ""affected_item_ids"": [""*""], ""trigger"": ""test""}
        ]
    }";

    [Fact]
    public void Load_ValidJson_ReturnsSuccess()
    {
        var result = HardcoreEconomyTuningLoader.Load(MinimalValidJson);
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
        Assert.NotNull(result.Bundle);
        Assert.Single(result.Bundle.ScarcityTiers);
        Assert.Single(result.Bundle.FactionPreferences);
        Assert.Single(result.Bundle.PriceShockRules);
    }

    [Fact]
    public void Load_EmptyJson_ReturnsFailure()
    {
        var result = HardcoreEconomyTuningLoader.Load("");
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);
        Assert.Null(result.Bundle);
    }

    [Fact]
    public void Load_MalformedJson_ReturnsFailure()
    {
        var result = HardcoreEconomyTuningLoader.Load("{bad json!");
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);
    }

    [Fact]
    public void Overlay_Default_ReturnsUnityParity()
    {
        var overlay = new HardcoreEconomyTuning();
        Assert.False(overlay.IsActive);
        Assert.Equal(1.0f, overlay.GetScarcityMultiplier(1, "water"));
        Assert.False(overlay.TryGetFactionPreference("any", out _));
        Assert.False(overlay.TryGetPriceShock(PriceShockKind.PlumePassing, 0, out _));
    }

    [Fact]
    public void Overlay_AppliedBundle_ReturnsScarcityMultiplier()
    {
        var result = HardcoreEconomyTuningLoader.Load(MinimalValidJson);
        Assert.True(result.IsValid);

        var overlay = new HardcoreEconomyTuning();
        overlay.Apply(result.Bundle!);
        Assert.True(overlay.IsActive);

        // Day 5 is in "Days 1-15" range, water is affected.
        Assert.Equal(2.5f, overlay.GetScarcityMultiplier(5, "water"));
        // Day 20 is outside the range.
        Assert.Equal(1.0f, overlay.GetScarcityMultiplier(20, "water"));
        // Food is not in the affected list.
        Assert.Equal(1.0f, overlay.GetScarcityMultiplier(5, "food"));
    }

    [Fact]
    public void Overlay_AppliedBundle_FactionPreferenceFound()
    {
        var result = HardcoreEconomyTuningLoader.Load(MinimalValidJson);
        Assert.True(result.IsValid);

        var overlay = new HardcoreEconomyTuning();
        overlay.Apply(result.Bundle!);
        Assert.True(overlay.TryGetFactionPreference("fac_a", out var pref));
        Assert.Equal("fac_a", pref.FactionId);
        Assert.Equal("food", pref.TradeCurrency);
        Assert.Single(pref.BuysAtPremium);
        Assert.Single(pref.Refuses);
    }

    [Fact]
    public void Overlay_AppliedBundle_PriceShockFoundWithinDuration()
    {
        var result = HardcoreEconomyTuningLoader.Load(MinimalValidJson);
        Assert.True(result.IsValid);

        var overlay = new HardcoreEconomyTuning();
        overlay.Apply(result.Bundle!);

        Assert.True(overlay.TryGetPriceShock(PriceShockKind.PlumePassing, 0, out var rule));
        Assert.Equal(1.8f, rule.Multiplier);
        Assert.Equal(3, rule.DurationDays);

        // Day offset 2 is within duration.
        Assert.True(overlay.TryGetPriceShock(PriceShockKind.PlumePassing, 2, out rule));
        // Day offset 3 is past duration.
        Assert.False(overlay.TryGetPriceShock(PriceShockKind.PlumePassing, 3, out _));
    }
}


