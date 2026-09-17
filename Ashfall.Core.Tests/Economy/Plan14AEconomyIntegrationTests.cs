// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Economy;
using Xunit;

namespace Ashfall.Core.Tests.Economy
{
    /// <summary>
    /// C1 checkpoint C1.3 — Phase 1 combined economy integration gate (Core
    /// harness): embargo blocks/slows the correct caravans, prices respond
    /// through ONE embargo factor that composes after the regional baseline,
    /// shocks decay, regional identity persists, and save/load cannot reset or
    /// duplicate the effect. Plan 212 §10.2 Scenarios I and II + §6.3 quote
    /// equation. Panel presentation is the next wave.
    /// </summary>
    public sealed class Plan14AEconomyIntegrationTests
    {
        private static string GetDataDir()
        {
            string candidate = System.IO.Path.Combine(AppContext.BaseDirectory, "../../../..", "Assets/StreamingAssets/Data");
            if (System.IO.Directory.Exists(candidate)) return System.IO.Path.GetFullPath(candidate);
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null)
            {
                string check = System.IO.Path.Combine(dir.FullName, "Assets/StreamingAssets/Data");
                if (System.IO.Directory.Exists(check)) return check;
                dir = dir.Parent;
            }
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found");
        }

        private static (MarketSystem market, TradeEmbargoSystem embargo, RegionalPriceAtlas atlas, GoodsCatalog goods) BuildBoundEconomy()
        {
            var goodsLoad = GoodsCatalogLoader.Load(GetDataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.False(goodsLoad.HasErrors, string.Join("; ", goodsLoad.Errors));
            var goods = GoodsCatalogLoader.ToCatalog(goodsLoad);

            var embargoLoad = TradeEmbargoCatalogLoader.Load(GetDataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.False(embargoLoad.HasErrors, string.Join("; ", embargoLoad.Errors));
            var embargo = new TradeEmbargoSystem(TradeEmbargoCatalogLoader.ToCatalog(embargoLoad));

            var atlasLoad = RegionalPriceCatalogLoader.Load(GetDataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.False(atlasLoad.HasErrors, string.Join("; ", atlasLoad.Errors));
            var atlas = new RegionalPriceAtlas(RegionalPriceCatalogLoader.ToCatalog(atlasLoad));

            var market = new MarketSystem();
            market.BindCatalog(goods);
            market.BindRegionalPriceAtlas(atlas, marketRegion: "settlement");
            market.BindEmbargoSystem(embargo);
            return (market, embargo, atlas, goods);
        }

        private static TravelingCaravanSystem BuildCaravans(TradeEmbargoSystem embargo)
        {
            var caravans = new TravelingCaravanSystem();
            caravans.Embargoes = embargo;
            caravans.SpawnCaravan("caravan_foundry_test", "Foundry Cart", "faction_wandering_menders",
                new System.Collections.Generic.List<string> { "node_a", "node_b", "node_c" },
                originRegion: "foundry");
            return caravans;
        }

        // ── §6.3 canonical quote equation ─────────────────────────────────

        [Fact]
        public void CanonicalPriceEquation_RegionalThenEmbargo_AppliesEachExactlyOnce()
        {
            var (market, embargo, _, goods) = BuildBoundEconomy();
            // Synthetic rule hitting mechanical_parts in foundry at +30% so the
            // §6.3 example is reproducible: 100 × 0.80 regional × 1.30 embargo.
            Assert.True(embargo.RegisterRule(new EmbargoRule(
                "embargo_equation_test", WeatherKind.EMPStorm,
                new[] { "foundry" }, Array.Empty<string>(), new[] { "mechanical_parts" },
                affectsAllGoods: false, priceMultiplierPermille: 1300,
                caravanBlocked: true, routeSlowPermille: 1000, decayDays: 2)));
            embargo.NotifyWeather(10, WeatherKind.EMPStorm);

            var explanation = market.ExplainPrice("mechanical_parts", MarketTransactionSide.Buy, "foundry");

            // Factor order and count: Regional before Embargo, each exactly once.
            var kinds = explanation.factors.Select(f => f.kind).ToList();
            Assert.Contains(PriceFactorKind.Regional, kinds);
            Assert.Contains(PriceFactorKind.Embargo, kinds);
            Assert.Equal(kinds.IndexOf(PriceFactorKind.Regional), kinds.LastIndexOf(PriceFactorKind.Regional));
            Assert.Equal(kinds.IndexOf(PriceFactorKind.Embargo), kinds.LastIndexOf(PriceFactorKind.Embargo));
            Assert.True(kinds.IndexOf(PriceFactorKind.Regional) < kinds.IndexOf(PriceFactorKind.Embargo));

            // The §6.3 example equation (100 × 0.80 × 1.30 = 104) scaled by the
            // good's real base 5.0: 5.0 → 4.0 regional → 5.2 embargo.
            Assert.Equal(5.2f, explanation.finalPrice, 3);
        }

        [Fact]
        public void LegacyCallers_UnboundCollaborators_ProduceNoNewFactors()
        {
            var goodsLoad = GoodsCatalogLoader.Load(GetDataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
            var market = new MarketSystem();
            market.BindCatalog(GoodsCatalogLoader.ToCatalog(goodsLoad));

            var explanation = market.ExplainPrice("mechanical_parts");
            Assert.DoesNotContain(explanation.factors, f => f.kind == PriceFactorKind.Regional);
            Assert.DoesNotContain(explanation.factors, f => f.kind == PriceFactorKind.Embargo);
            // Backward-compatible region overload: null region, no atlas bound.
            Assert.Equal(market.GetPrice("mechanical_parts"), market.GetPrice("mechanical_parts", "foundry"));
        }

        // ── §10.2 Scenario I — Weather Trade Shock ────────────────────────

        [Fact]
        public void ScenarioI_WeatherTradeShock_BlockShockResumeDecay()
        {
            var (market, embargo, atlas, _) = BuildBoundEconomy();
            var caravans = BuildCaravans(embargo);
            var caravan = caravans.State.activeCaravans.Single();

            int embargoedCount = 0;
            int resumedCount = 0;
            caravans.OnCaravanEmbargoed += (_, _) => embargoedCount++;
            caravans.OnCaravanResumed += _ => resumedCount++;

            // 1–2. Neutral weather: the caravan progresses.
            caravans.DailyTick(currentDay: 10, weather: WeatherKind.Clear);
            Assert.Equal(1, caravan.daysAtCurrentNode);
            Assert.False(caravan.embargoBlocked);

            // 4–5. EMPStorm: foundry route blocked — no movement, ONE event.
            caravans.DailyTick(currentDay: 11, weather: WeatherKind.EMPStorm);
            caravans.DailyTick(currentDay: 12, weather: WeatherKind.EMPStorm);
            Assert.True(caravan.embargoBlocked);
            Assert.Equal(1, caravan.daysAtCurrentNode); // blocked days do not advance the stay clock
            Assert.Equal(1, embargoedCount); // transition-deduplicated
            Assert.Equal("node_a", caravan.currentNodeId);

            // 6. Electronics shock is live in the quote path (Embargo factor).
            embargo.NotifyWeather(12, WeatherKind.EMPStorm);
            var shocked = market.ExplainPrice("solar_cell", MarketTransactionSide.Buy, "foundry");
            Assert.Contains(shocked.factors, f => f.kind == PriceFactorKind.Embargo && f.multiplier > 1.9f);

            // 8–10. Clear weather: route resumes immediately, shock decays.
            caravans.DailyTick(currentDay: 13, weather: WeatherKind.Clear);
            Assert.False(caravan.embargoBlocked);
            Assert.Equal(1, resumedCount);
            embargo.NotifyWeather(13, WeatherKind.Clear);
            embargo.NotifyWeather(14, WeatherKind.Clear);
            embargo.NotifyWeather(15, WeatherKind.Clear);
            Assert.Empty(embargo.ActiveShocks);
            Assert.Equal(1000, embargo.GetCurrentPriceMultiplierPermille("foundry", "solar_cell", "tools"));

            // 11. Regional identity is persistent — foundry stays cheap for
            // production goods (no embargo residue).
            Assert.Equal(800, atlas.GetModifierPermille("mechanical_parts", "materials", "foundry"));
            var calm = market.ExplainPrice("mechanical_parts", MarketTransactionSide.Buy, "foundry");
            Assert.Contains(calm.factors, f => f.kind == PriceFactorKind.Regional && f.multiplier < 1f);
            Assert.DoesNotContain(calm.factors, f => f.kind == PriceFactorKind.Embargo);
        }

        // ── §10.2 Scenario II — Regional Arbitrage (Core quote view) ──────

        [Fact]
        public void ScenarioII_RegionalArbitrage_BestRegionStableUnderShock()
        {
            var (market, embargo, atlas, _) = BuildBoundEconomy();
            string[] regions = { "flotilla", "foundry", "greenhouse", "traplines", "settlement" };

            // Same item, five regions: foundry is the cheapest source for
            // mechanical parts (0.8x vs neutral elsewhere).
            Assert.Equal("foundry", atlas.GetBestRegion("mechanical_parts", "materials", regions));
            var regionalQuotes = regions.ToDictionary(
                r => r, r => market.GetPrice("mechanical_parts", r));
            Assert.Equal(4f, regionalQuotes["foundry"], 3);      // 5.0 × 0.8
            // CONTRACT DRIFT (Plan 14 -> Plan 56): settlement used to be
            // neutral here. regionalSupply is now a live quote factor, so
            // foundry-produced parts are imported at settlement (5.0 x 1.5).
            Assert.Equal(7.5f, regionalQuotes["settlement"], 3);
            Assert.Contains(
                market.ExplainPrice("mechanical_parts", MarketTransactionSide.Buy, "settlement").factors,
                factor => factor.kind == PriceFactorKind.RegionalSupply &&
                    Math.Abs(factor.multiplier - 1.5f) < 0.0001f);

            // A weather shock changes the TEMPORARY quote, not the regional
            // baseline: RadHail (all goods, all regions) lifts every quote.
            embargo.NotifyWeather(10, WeatherKind.RadHail);
            Assert.True(market.GetPrice("mechanical_parts", "foundry") > regionalQuotes["foundry"]);
            Assert.Equal("foundry", atlas.GetBestRegion("mechanical_parts", "materials", regions));
            embargo.NotifyWeather(11, WeatherKind.Clear);
            embargo.NotifyWeather(12, WeatherKind.Clear);
            Assert.Equal(1000, embargo.GetCurrentPriceMultiplierPermille("foundry", "mechanical_parts", "materials"));
        }

        // ── Save / load parity ────────────────────────────────────────────

        [Fact]
        public void SaveLoad_MidBlockAndMidDecay_SameContinuation()
        {
            var (marketA, embargoA, _, _) = BuildBoundEconomy();
            var caravansA = BuildCaravans(embargoA);
            var caravanA = caravansA.State.activeCaravans.Single();

            // Advance to a blocked state with a decaying prior shock.
            embargoA.NotifyWeather(10, WeatherKind.EMPStorm);
            caravansA.DailyTick(10, weather: WeatherKind.EMPStorm);
            Assert.True(caravanA.embargoBlocked);

            var marketSave = marketA.CaptureState();
            var caravanSave = caravansA.CaptureState();
            Assert.Equal(3, marketSave.version); // v3 carries the embargo state
            Assert.NotNull(marketSave.tradeEmbargo);

            // Fresh instances restore the exact state.
            var (marketB, embargoB, _, _) = BuildBoundEconomy();
            marketB.RestoreState(marketSave);
            var caravansB = BuildCaravans(embargoB);
            caravansB.RestoreState(caravanSave);
            var caravanB = caravansB.State.activeCaravans.Single();

            Assert.True(caravanB.embargoBlocked); // blocked state survived
            Assert.Equal(
                embargoA.GetCurrentPriceMultiplierPermille("foundry", "solar_cell", "tools"),
                embargoB.GetCurrentPriceMultiplierPermille("foundry", "solar_cell", "tools"));
            // No free movement day on reload: still blocked at the same node
            // with the same stay-day accumulation (one blocked tick → 0).
            Assert.Equal("node_a", caravanB.currentNodeId);
            Assert.Equal(0, caravanB.daysAtCurrentNode);

            // Identical continuation: one clear day produces identical states.
            caravansA.DailyTick(11, weather: WeatherKind.Clear);
            caravansB.DailyTick(11, weather: WeatherKind.Clear);
            Assert.False(caravanA.embargoBlocked);
            Assert.False(caravanB.embargoBlocked);
            Assert.Equal(caravanA.currentNodeId, caravanB.currentNodeId);
            Assert.Equal(caravanA.daysAtCurrentNode, caravanB.daysAtCurrentNode);
        }

        [Fact]
        public void SaveLoad_V2SaveRestoresEmbargoNeutral_NoPhantomShock()
        {
            var (market, embargo, _, _) = BuildBoundEconomy();
            embargo.NotifyWeather(10, WeatherKind.EMPStorm);

            var v2Save = new MarketState // simulate a pre-C1 save
            {
                version = 2,
                day = 10,
                demand = { new DemandEntry { itemId = "solar_cell", multiplier = 1.2f } }
            };
            market.RestoreState(v2Save);
            Assert.Empty(embargo.ActiveShocks); // no phantom embargo shock
            Assert.Equal(1000, embargo.GetCurrentPriceMultiplierPermille("foundry", "solar_cell", "tools"));
        }

        // ── Determinism ───────────────────────────────────────────────────

        [Fact]
        public void DeterministicMarket_SeededTicks_ProduceIdenticalQuotes()
        {
            var (marketA, embargoA, _, _) = BuildBoundEconomy();
            var (marketB, embargoB, _, _) = BuildBoundEconomy();

            int[] days = { 10, 11, 12, 13 };
            WeatherKind[] weather = { WeatherKind.Clear, WeatherKind.EMPStorm, WeatherKind.EMPStorm, WeatherKind.Clear };
            for (int i = 0; i < days.Length; i++)
            {
                embargoA.NotifyWeather(days[i], weather[i]);
                embargoB.NotifyWeather(days[i], weather[i]);
                marketA.TickDay(days[i], new SeededRng(1000 + days[i]));
                marketB.TickDay(days[i], new SeededRng(1000 + days[i]));
            }
            Assert.Equal(
                marketA.GetPrice("solar_cell", "foundry"),
                marketB.GetPrice("solar_cell", "foundry"), 4);
        }
    }
}
