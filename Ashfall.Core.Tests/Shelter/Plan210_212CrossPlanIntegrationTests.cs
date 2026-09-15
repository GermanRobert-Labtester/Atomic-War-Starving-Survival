// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Disease;
using Ashfall.Core.Economy;
using Ashfall.Core.Shelter;
using Xunit;

namespace Ashfall.Core.Tests.Shelter
{
    /// <summary>
    /// Wave 6 — cross-plan integration contracts: sanitation → disease
    /// (authored foul_water_draw sweep, infection stays with the disease
    /// authority), sanitation → economy (bounded crisis demand shock),
    /// economy → black market (the SAME shock raises both canonical and
    /// illicit prices — one pricing pipeline), and the reversible morale
    /// mark. All policy is Core-owned and deterministic.
    /// </summary>
    public sealed class Plan210_212CrossPlanIntegrationTests
    {
        private static string GetDataDir()
        {
            string candidate = Path.Combine(AppContext.BaseDirectory, "../../../..", "Assets/StreamingAssets/Data");
            if (Directory.Exists(candidate)) return Path.GetFullPath(candidate);
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null)
            {
                string check = Path.Combine(dir.FullName, "Assets/StreamingAssets/Data");
                if (Directory.Exists(check)) return check;
                dir = dir.Parent;
            }
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found");
        }

        // ── Rules (deterministic bands) ─────────────────────────────

        [Fact]
        public void Rules_ExposureSweep_RunsOnlyWhenSanitationIsPoorOrSpilled()
        {
            Assert.False(SanitationConsequenceRules.ShouldRunDailyExposureSweep(HygieneBand.Excellent, hasActiveSpill: false));
            Assert.False(SanitationConsequenceRules.ShouldRunDailyExposureSweep(HygieneBand.Acceptable, hasActiveSpill: false));
            Assert.True(SanitationConsequenceRules.ShouldRunDailyExposureSweep(HygieneBand.Poor, hasActiveSpill: false));
            Assert.True(SanitationConsequenceRules.ShouldRunDailyExposureSweep(HygieneBand.Squalid, hasActiveSpill: false));
            // A spill alone forces the sweep even in an otherwise clean shelter.
            Assert.True(SanitationConsequenceRules.ShouldRunDailyExposureSweep(HygieneBand.Excellent, hasActiveSpill: true));
        }

        [Fact]
        public void Rules_MoraleMark_IsReversible_HazardousOnly()
        {
            // Only Hazardous sets the mark...
            Assert.True(SanitationConsequenceRules.ShouldSetHazardousMark(HygieneBand.Hazardous));
            Assert.False(SanitationConsequenceRules.ShouldSetHazardousMark(HygieneBand.Squalid));
            Assert.False(SanitationConsequenceRules.ShouldSetHazardousMark(HygieneBand.Poor));
            // ...and recovery to Acceptable or better clears it (no permanent
            // morale damage from a temporary mess).
            Assert.True(SanitationConsequenceRules.ShouldClearHazardousMark(HygieneBand.Acceptable));
            Assert.True(SanitationConsequenceRules.ShouldClearHazardousMark(HygieneBand.Excellent));
            Assert.False(SanitationConsequenceRules.ShouldClearHazardousMark(HygieneBand.Squalid));
        }

        [Fact]
        public void Rules_CrisisDemandShock_IsBoundedAndRefreshSafe()
        {
            Assert.False(SanitationConsequenceRules.ShouldApplyCrisisDemandShock(HygieneBand.Acceptable, hasActiveSpill: false));
            Assert.False(SanitationConsequenceRules.ShouldApplyCrisisDemandShock(HygieneBand.Poor, hasActiveSpill: false));
            Assert.True(SanitationConsequenceRules.ShouldApplyCrisisDemandShock(HygieneBand.Squalid, hasActiveSpill: false));
            Assert.True(SanitationConsequenceRules.ShouldApplyCrisisDemandShock(HygieneBand.Hazardous, hasActiveSpill: false));
            Assert.True(SanitationConsequenceRules.ShouldApplyCrisisDemandShock(HygieneBand.Acceptable, hasActiveSpill: true));
            Assert.Equal(1000f, SanitationConsequenceRules.CrisisShockSeverityBp, 5);
            Assert.Equal(2, SanitationConsequenceRules.CrisisShockDurationDays);
        }

        // ── Sanitation → economy → prices ───────────────────────────

        [Fact]
        public void SanitationCrisis_RaisesMedicalPrices_ThroughTheCanonicalShock()
        {
            var load = GoodsCatalogLoader.Load(GetDataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
            var market = new MarketSystem();
            market.BindCatalog(GoodsCatalogLoader.ToCatalog(load));
            var commodity = CommodityBaselineCatalogLoader.Load(GetDataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
            market.BindCommodityCatalog(CommodityBaselineCatalogLoader.ToCatalog(commodity));
            market.State.categoryIndices.Add(new CategoryIndexEntry { categoryId = "medical", multiplier = 1f });

            string medItem = "bandages";
            float before = market.GetPrice(medItem);
            Assert.True(before > 0);

            // Crisis (Squalid shelter) → bounded 2-day medical shortage shock.
            market.ApplyShock("medical", isShortage: true,
                severityBp: SanitationConsequenceRules.CrisisShockSeverityBp,
                startDay: 2, durationDays: SanitationConsequenceRules.CrisisShockDurationDays,
                sourceId: SanitationConsequenceRules.CrisisShockSourceId);
            float shocked = market.GetPrice(medItem);
            Assert.True(shocked > before, $"crisis must raise medical prices (got {before:F2} → {shocked:F2})");

            // Shock expiry returns the price to the category path.
            market.TickDay(4, new SeededRng(3));
            Assert.Empty(market.ActiveShocks);
        }

        // ── Economy → black market (one price pipeline) ─────────────

        [Fact]
        public void MedicineShortage_RaisesBothCanonicalAndBlackMarketPrices_OnePipeline()
        {
            var goodsLoad = GoodsCatalogLoader.Load(GetDataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
            var market = new MarketSystem();
            market.BindCatalog(GoodsCatalogLoader.ToCatalog(goodsLoad));
            var commodity = CommodityBaselineCatalogLoader.Load(GetDataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
            market.BindCommodityCatalog(CommodityBaselineCatalogLoader.ToCatalog(commodity));

            var bmLoad = Ashfall.Core.Economy.BlackMarketInventoryCatalogLoader.Load(GetDataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
            var blackMarket = new Ashfall.Core.Economy.BlackMarketSystem();
            blackMarket.BindCatalog(Ashfall.Core.Economy.BlackMarketInventoryCatalogLoader.ToCatalog(bmLoad));
            blackMarket.BindMarket(market);
            blackMarket.DiscoverContact("faction_wasteland_outlaws", 1);

            var entry = blackMarket.Catalog.FindEntry("black_market_field_medicine")!;
            float canonicalBefore = market.GetPrice(entry.item_id);
            float bmBefore = blackMarket.GetBuyPrice("faction_wasteland_outlaws", entry);

            // The SAME shock (no second price authority anywhere).
            market.ApplyShock("medical", true, 2000f, 2, 3, "weather_winter");

            float canonicalAfter = market.GetPrice(entry.item_id);
            float bmAfter = blackMarket.GetBuyPrice(entry: entry, syndicateId: "faction_wasteland_outlaws");
            Assert.True(canonicalAfter > canonicalBefore);
            Assert.True(bmAfter > bmBefore);
            // Black market stays strictly above canonical (premium preserved).
            Assert.True(bmAfter > canonicalAfter);
        }

        // ── Sanitation → disease (ownership contract) ──────────────

        [Fact]
        public void DiseaseSweep_FeedsTheAuthoredSource_AndProbabilityModifierChangesOutcomes()
        {
            // The authored foul_water_draw source exists and targets cholera.
            string raw = File.ReadAllText(Path.Combine(GetDataDir(), "disease_catalog.json"));
            Assert.Contains("\"source_id\": \"foul_water_draw\"", raw);
            Assert.Contains("disease_cholera", raw);

            // The bounded modifier measurably changes TryExpose outcomes
            // under the same seeded disease RNG (infection stays owned here).
            var catalog = DiseaseCatalogLoader.Load(GetDataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
            var engine1 = new DiseaseSystem(rng: new global::Ashfall.Core.SeededRng(77));
            engine1.BindCatalog(catalog);
            var engine2 = new DiseaseSystem(rng: new global::Ashfall.Core.SeededRng(77));
            engine2.BindCatalog(DiseaseCatalogLoader.Load(GetDataDir(), new FileSystemIO(), new SystemTextJsonSerializer()));

            int infectedBaseline = 0, infectedElevated = 0;
            for (int i = 0; i < 120; i++)
            {
                string id1 = $"s1_{i}";
                var r1 = engine1.TryExpose(new DiseaseExposureContext
                {
                    SurvivorId = id1, DiseaseId = DiseaseIds.Cholera,
                    SourceId = SanitationConsequenceRules.CholeraSourceId,
                    Day = 1, ProbabilityModifier = 1.0f
                });
                if (r1.Infected) infectedBaseline++;
            }
            for (int i = 0; i < 120; i++)
            {
                string id2 = $"s2_{i}";
                var r2 = engine2.TryExpose(new DiseaseExposureContext
                {
                    SurvivorId = id2, DiseaseId = DiseaseIds.Cholera,
                    SourceId = SanitationConsequenceRules.CholeraSourceId,
                    Day = 1, ProbabilityModifier = 2.0f   // fouled sanitation ceiling
                });
                if (r2.Infected) infectedElevated++;
            }
            Assert.True(infectedElevated > infectedBaseline,
                $"elevated sanitation modifier must raise infections ({infectedBaseline} → {infectedElevated})");
        }

        // ── Morale mark reversibility (mark authority untouched) ────

        [Fact]
        public void HazardousMark_SetOnCollapse_ClearedOnRecovery()
        {
            var marks = new MoraleMarkSystem();
            const string markId = SanitationConsequenceRules.HazardousMarkId;

            Assert.False(marks.HasMark(markId));
            // Hazardous shelter sets the mark.
            if (SanitationConsequenceRules.ShouldSetHazardousMark(HygieneBand.Hazardous))
                marks.SetMark(markId, "hazard", 10);
            Assert.True(marks.HasMark(markId));
            // Recovery clears it — a temporary mess is never permanent damage.
            if (SanitationConsequenceRules.ShouldClearHazardousMark(HygieneBand.Acceptable))
                marks.ClearMark(markId);
            Assert.False(marks.HasMark(markId));
        }

        // ── Metallurgy → economy supply response (structural) ──────

        [Fact]
        public void MetallurgySupply_SellingOutputSoftensTheIndex_Structurally()
        {
            // §Cross-5 — production supply reaches prices through the EXISTING
            // trade-pressure model (selling adds supply pressure that lowers
            // the category index); no direct price mutation at batch completion.
            var goodsLoad = GoodsCatalogLoader.Load(GetDataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
            var market = new MarketSystem();
            market.BindCatalog(GoodsCatalogLoader.ToCatalog(goodsLoad));
            var commodity = CommodityBaselineCatalogLoader.Load(GetDataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
            market.BindCommodityCatalog(CommodityBaselineCatalogLoader.ToCatalog(commodity));
            market.State.categoryIndices.Add(new CategoryIndexEntry { categoryId = "materials", multiplier = 1f });

            // Player sells a large metallurgy output batch into the market.
            market.Sell("scrap_metal", 1500, 1, "shelter");
            market.TickDay(2, new SeededRng(7));
            Assert.True(market.GetCategoryMultiplier("materials") < 1f,
                "supply pressure from selling production must lower the category index");
        }
    }
}
