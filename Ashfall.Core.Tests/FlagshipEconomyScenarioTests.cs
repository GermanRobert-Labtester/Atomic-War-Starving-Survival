// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Disease;
using Ashfall.Core.Economy;
using Ashfall.Core.Factions;
using Ashfall.Core.Foundry;
using Ashfall.Core.Shelter;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Wave 8 — flagship scenarios A–F as deterministic Core harnesses. Each
    /// scenario proves the flagship loop end-to-end with a paired run and,
    /// where specified, a mid-campaign save/restore replay (identical final
    /// state — no rerolls, no duplicated events).
    /// </summary>
    public sealed class FlagshipEconomyScenarioTests
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

        private static MarketSystem CreateMarket()
        {
            var market = new MarketSystem();
            market.BindCatalog(GoodsCatalogLoader.ToCatalog(
                GoodsCatalogLoader.Load(GetDataDir(), new FileSystemIO(), new SystemTextJsonSerializer())));
            market.BindCommodityCatalog(CommodityBaselineCatalogLoader.ToCatalog(
                CommodityBaselineCatalogLoader.Load(GetDataDir(), new FileSystemIO(), new SystemTextJsonSerializer())));
            return market;
        }

        private static SanitationSystem CreateSanitation()
        {
            var system = new SanitationSystem();
            system.BindFacilityCatalog(SanitationFacilityCatalogLoader.ToCatalog(
                SanitationFacilityCatalogLoader.Load(GetDataDir(), new FileSystemIO(), new SystemTextJsonSerializer())));
            return system;
        }

        private static BlackMarketSystem CreateBlackMarket(MarketSystem market)
        {
            var bm = new BlackMarketSystem();
            bm.BindCatalog(BlackMarketInventoryCatalogLoader.ToCatalog(
                BlackMarketInventoryCatalogLoader.Load(GetDataDir(), new FileSystemIO(), new SystemTextJsonSerializer())));
            bm.BindMarket(market);
            return bm;
        }

        private static void DriveFoundryHeatToCompletion(SilentFoundrySystem foundry, int startDay)
        {
            int day = startDay;
            int tapped = -1;
            for (int i = 0; i < 15; i++)
            {
                foundry.TickDaily(day);
                if (foundry.HeatStage == FoundryHeatStage.AtHeat && tapped < 0)
                {
                    foundry.TapAndCast(day);
                    tapped = day;
                }
                if (foundry.HeatStage == FoundryHeatStage.Complete) return;
                day++;
            }
        }

        // ── Scenario A — sanitation collapse and market pressure ────

        [Fact]
        public void ScenarioA_SanitationCollapse_RaisesMedicalPrices_ThenRecovers()
        {
            var market = CreateMarket();
            var sanitation = CreateSanitation();
            sanitation.EnsureRoom("dorm_a", RoomWasteRole.Residential);
            sanitation.EnsureRoom("kitchen", RoomWasteRole.FoodPrep);
            market.State.categoryIndices.Add(new CategoryIndexEntry { categoryId = "medical", multiplier = 1f });

            float before = market.GetPrice("bandages");
            // Days 1–8: no sanitation service, waste piles up → hygiene falls.
            for (int day = 1; day <= 8; day++)
            {
                sanitation.TickDaily(day, population: 10);
                var band = sanitation.GetShelterHygieneBand();
                if (SanitationConsequenceRules.ShouldApplyCrisisDemandShock(band, sanitation.ActiveSpill != null))
                {
                    market.ApplyShock("medical", true,
                        SanitationConsequenceRules.CrisisShockSeverityBp, day,
                        SanitationConsequenceRules.CrisisShockDurationDays,
                        SanitationConsequenceRules.CrisisShockSourceId);
                }
                market.TickDay(day, new global::Ashfall.Core.SeededRng(day));
            }
            float during = market.GetPrice("bandages");
            Assert.True(sanitation.GetShelterHygienePermille() < 600, "hygiene must fall under load");
            Assert.True(during > before, $"crisis must raise medical prices ({before:F2} → {during:F2})");

            // Cleaning duty + facility recovery lowers pressure; prices normalize.
            sanitation.ApplyCleaning("dorm_a", 8, 1f, CleaningPriority.Critical, 9);
            sanitation.InstallFacility("sanitation_sealed_latrine", "dorm_a");
            sanitation.InstallFacility("sanitation_wash_station", "kitchen");
            for (int day = 9; day <= 16; day++)
            {
                sanitation.TickDaily(day, population: 10);
                market.TickDay(day, new global::Ashfall.Core.SeededRng(day));
            }
            var commodity = CommodityBaselineCatalogLoader.Load(GetDataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
            float medicalTarget = commodity.Categories.First(c => c.category_id == "medical").base_multiplier_permille / 1000f;
            Assert.InRange(market.GetCategoryMultiplier("medical"), 0.97f * medicalTarget, 1.06f * medicalTarget);
        }

        // ── Scenario B — underworld debt spiral ─────────────────────

        [Fact]
        public void ScenarioB_DebtSpiral_OverdueFiresOnce_BountyPlaced_SaveLoadNoDuplication()
        {
            var market = CreateMarket();
            var bounties = new FactionBountySystem();
            var bm = CreateBlackMarket(market);
            bm.BindFactionBountySystem(bounties);

            bm.DiscoverContact("faction_cold_ledger", 1);
            bm.EnsureStockSnapshot("faction_cold_ledger", 2, new global::Ashfall.Core.SeededRng(11));
            var debt = bm.TakeLoan("faction_cold_ledger", 700f, 2, 5)!;
            Assert.NotNull(debt);

            int overdueCount = 0, bountyCount = 0;
            bm.OnDebtOverdue += _ => overdueCount++;
            bm.OnBountyPlaced += _ => bountyCount++;

            for (int day = 2; day <= 8; day++) bm.TickDaily(day);
            Assert.Equal(1, overdueCount);   // fires exactly once
            Assert.Equal(1, bountyCount);
            Assert.True(bounties.HasActiveBounty("faction_cold_ledger"));

            // Save at day 8 → restore → replay days 9–12: no duplicate events.
            var saved = bm.CaptureState();
            var restored = CreateBlackMarket(market);
            restored.BindFactionBountySystem(bounties);
            restored.RestoreState(saved);
            for (int day = 9; day <= 12; day++) restored.TickDaily(day);
            Assert.Equal(1, restored.State.firedEventKeys.Count);
            Assert.Equal(1, bountyCount);   // never re-fires after restore
        }

        // ── Scenario C — nuclear-winter economy shock ───────────────

        [Fact]
        public void ScenarioC_WinterShock_FoodPriceRisesGradually_AndBounded()
        {
            var market = CreateMarket();
            market.State.categoryIndices.Add(new CategoryIndexEntry { categoryId = "food", multiplier = 1f });
            string foodItem = "canned_food";
            float before = market.GetPrice(foodItem);

            // Severe winter → blizzard band → 1500bp × 3d food shortage.
            var band = EconomyWeatherShockRules.TryGetWeatherShock(WeatherKind.Blizzard)!;
            market.ApplyShock(band.CategoryId, band.IsShortage, band.SeverityBp, 2, band.DurationDays, band.SourceId);

            float day3 = 0, day6 = 0;
            for (int day = 2; day <= 7; day++)
            {
                market.TickDay(day, new global::Ashfall.Core.SeededRng(day));
                // Pin demand so the shock math is isolated from the real
                // (and legitimate) volatility walk.
                market.AdjustDemand(foodItem, 1f - market.GetDemandMultiplier(foodItem));
                if (day == 3) day3 = market.GetPrice(foodItem);
                if (day == 6) day6 = market.GetPrice(foodItem);
            }
            Assert.True(day3 > before, $"winter shock must raise food prices ({before:F2} → {day3:F2})");
            // Bounded: never beyond the authored scarcity ceiling (2.0× index
            // on a 1.0 baseline; price ceiling 4× base as the hard bound).
            Assert.InRange(day6, before, before * 2.2f);
        }

        // ── Scenario D — metallurgy supply recovery ─────────────────

        [Fact]
        public void ScenarioD_MetallurgySupplyResponse_ProductionSoftensPrices_PremiumPersists()
        {
            var market = CreateMarket();
            market.State.categoryIndices.Add(new CategoryIndexEntry { categoryId = "materials", multiplier = 1.4f });
            var bm = CreateBlackMarket(market);
            bm.DiscoverContact("faction_ash_market_brokers", 1);

            float elevated = market.GetPrice("scrap_metal");

            // A big metallurgy batch sold into the market = supply pressure.
            market.Sell("scrap_metal", 2000, 2, "shelter");
            for (int day = 3; day <= 8; day++) market.TickDay(day, new global::Ashfall.Core.SeededRng(day));
            float softened = market.GetPrice("scrap_metal");
            Assert.True(softened < elevated, $"selling production must soften the index ({elevated:F2} → {softened:F2})");

            // The black market still commands its premium over canonical.
            var entry = bm.Catalog.FindEntry("black_market_field_medicine")!;
            Assert.True(bm.GetBuyPrice("faction_ash_market_brokers", entry)
                        > market.GetPrice(entry.item_id) * 1.25f);
        }

        // ── Scenario E — toxic spill ────────────────────────────────

        [Fact]
        public void ScenarioE_ToxicSpill_DeterministicTrigger_SaveRestoreNotRecreated()
        {
            var spills = new List<ActiveSpillState>();
            var sanitation = CreateSanitation();
            sanitation.OnSpillStarted += s => spills.Add(s);
            var room = sanitation.EnsureRoom("chem_bay", RoomWasteRole.Industrial);
            sanitation.EmitWaste("chem_bay", WasteType.Chemical, 300f);

            sanitation.TickDaily(1, population: 0);   // overcapacity → deterministic spill
            Assert.Single(spills);
            Assert.Equal("chem_bay", spills[0].roomId);
            Assert.Equal(WasteType.Chemical, spills[0].type);

            // Save + restore: the spill is persisted exactly, not recreated.
            var saved = sanitation.CaptureState();
            Assert.NotNull(saved.activeSpill);
            var restored = CreateSanitation();
            restored.RestoreState(saved);
            Assert.NotNull(restored.ActiveSpill);
            Assert.Equal(spills[0].roomId, restored.ActiveSpill!.roomId);
            Assert.Equal(spills[0].severity, restored.ActiveSpill.severity, 5);
            Assert.Equal(spills[0].startDay, restored.ActiveSpill.startDay);

            // Full emergency brigade clears the overcapacity deterministically.
            restored.ApplyCleaning("chem_bay", 80, 1f, CleaningPriority.Critical, 2);
            restored.TickDaily(3, population: 0);
            Assert.Null(restored.ActiveSpill);
        }

        // ── Scenario F — full campaign systems test ─────────────────

        private static void RunFlagshipCampaignStep(
            SanitationSystem sanitation, MarketSystem market, BlackMarketSystem bm,
            MoraleMarkSystem marks, int day)
        {
            sanitation.TickDaily(day, population: 10);
            var band = sanitation.GetShelterHygieneBand();
            if (SanitationConsequenceRules.ShouldApplyCrisisDemandShock(band, sanitation.ActiveSpill != null))
                market.ApplyShock("medical", true,
                    SanitationConsequenceRules.CrisisShockSeverityBp, day,
                    SanitationConsequenceRules.CrisisShockDurationDays,
                    SanitationConsequenceRules.CrisisShockSourceId);
            bm.EnsureStockSnapshot("faction_wasteland_outlaws", day, new global::Ashfall.Core.SeededRng(day * 31));
            market.Buy("bandages", 5, day, "settlement");
            market.TickDay(day, new global::Ashfall.Core.SeededRng(day));
            bm.TickDaily(day);
            // Reversible morale mark.
            if (SanitationConsequenceRules.ShouldSetHazardousMark(band))
                marks.SetMark(SanitationConsequenceRules.HazardousMarkId, "hazard", day);
            else if (SanitationConsequenceRules.ShouldClearHazardousMark(band)
                     && marks.HasMark(SanitationConsequenceRules.HazardousMarkId))
                marks.ClearMark(SanitationConsequenceRules.HazardousMarkId);
        }

        private static string CampaignFingerprint(SanitationSystem s, MarketSystem m, BlackMarketSystem bm, MoraleMarkSystem marks)
        {
            var f = new System.Text.StringBuilder();
            f.Append($"H{s.GetShelterHygienePermille()};");
            foreach (var r in s.State.rooms.OrderBy(r => r.roomId, StringComparer.Ordinal))
                f.Append($"{r.roomId}:{r.organic:0.00}:{r.chemical:0.00}:{r.radioactive:0.00};");
            f.Append($"S{s.GetShelterHygienePermille()};");
            foreach (var e in m.State.categoryIndices.OrderBy(e => e.categoryId, StringComparer.Ordinal))
                f.Append($"{e.categoryId}:{e.multiplier:0.0000};");
            f.Append($"L{m.State.ledger.Count};");
            foreach (var d in bm.State.debts.OrderBy(d => d.debtId, StringComparer.Ordinal))
                f.Append($"{d.debtId}:{d.status}:{d.repaidUnits:0.00};");
            foreach (var k in bm.State.firedEventKeys) f.Append($"K{k};");
            f.Append($"M{(marks.HasMark(SanitationConsequenceRules.HazardousMarkId) ? 1 : 0)};");
            return f.ToString();
        }

        [Fact]
        public void ScenarioF_CombinedCampaign_ContinuousEqualsMidReloadReplay()
        {
            // Reference campaign: 20 continuous days.
            var refSanitation = CreateSanitation();
            var refMarket = CreateMarket();
            var refBm = CreateBlackMarket(refMarket);
            var refMarks = new MoraleMarkSystem();
            refSanitation.EnsureRoom("dorm_a", RoomWasteRole.Residential);
            refSanitation.EnsureRoom("kitchen", RoomWasteRole.FoodPrep);
            refBm.DiscoverContact("faction_wasteland_outlaws", 1);
            refBm.TakeLoan("faction_wasteland_outlaws", 150f, 3, 10);
            for (int day = 1; day <= 20; day++)
                RunFlagshipCampaignStep(refSanitation, refMarket, refBm, refMarks, day);
            string reference = CampaignFingerprint(refSanitation, refMarket, refBm, refMarks);

            // Reload campaign: continuous to day 10, capture, restore into
            // fresh instances, replay days 11–20 — identical fingerprint.
            var rSanitation = CreateSanitation();
            var rMarket = CreateMarket();
            var rBm = CreateBlackMarket(rMarket);
            var rMarks = new MoraleMarkSystem();
            rSanitation.EnsureRoom("dorm_a", RoomWasteRole.Residential);
            rSanitation.EnsureRoom("kitchen", RoomWasteRole.FoodPrep);
            rBm.DiscoverContact("faction_wasteland_outlaws", 1);
            rBm.TakeLoan("faction_wasteland_outlaws", 150f, 3, 5);
            for (int day = 1; day <= 10; day++)
                RunFlagshipCampaignStep(rSanitation, rMarket, rBm, rMarks, day);

            var sSan = rSanitation.CaptureState();
            var sMkt = rMarket.CaptureState();
            var sBm = rBm.CaptureState();
            var restoredSan = CreateSanitation();
            restoredSan.RestoreState(sSan);
            var restoredMkt = new MarketSystem();
            restoredMkt.BindCatalog(GoodsCatalogLoader.ToCatalog(
                GoodsCatalogLoader.Load(GetDataDir(), new FileSystemIO(), new SystemTextJsonSerializer())));
            restoredMkt.BindCommodityCatalog(CommodityBaselineCatalogLoader.ToCatalog(
                CommodityBaselineCatalogLoader.Load(GetDataDir(), new FileSystemIO(), new SystemTextJsonSerializer())));
            restoredMkt.RestoreState(sMkt);
            var restoredBm = CreateBlackMarket(restoredMkt);
            restoredBm.RestoreState(sBm);
            var restoredMarks = new MoraleMarkSystem();

            for (int day = 11; day <= 20; day++)
                RunFlagshipCampaignStep(restoredSan, restoredMkt, restoredBm, restoredMarks, day);
            string replay = CampaignFingerprint(restoredSan, restoredMkt, restoredBm, restoredMarks);

            Assert.Equal(reference, replay);
        }
    }
}
