// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Economy;
using Ashfall.Core.Save;
using Xunit;

namespace Ashfall.Core.Tests.Economy
{
    /// <summary>
    /// Plan 212 Wave 2 host-wiring contract without a Godot runtime: the
    /// Core-side weather→shock policy, the commodity catalog walk from the
    /// real StreamingAssets dir (the file the EconomyHostSession binds), and
    /// the economy save-section regression (no new section — additive v2 only).
    /// Host session binding itself is verified through the headless selftests.
    /// </summary>
    public sealed class Plan212EconomyHostWiringTests
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

        // ── Weather shock policy (Core) ───────────────────────────────

        [Fact]
        public void WeatherShockRules_BlizzardBand_IsStrongestAndLongest()
        {
            var band = EconomyWeatherShockRules.TryGetWeatherShock(WeatherKind.Blizzard);
            Assert.NotNull(band);
            Assert.Equal("food", band!.CategoryId);
            Assert.True(band.IsShortage);
            Assert.Equal(1500f, band.SeverityBp, 5);
            Assert.Equal(3, band.DurationDays);
            Assert.Equal("weather_blizzard", band.SourceId);
        }

        [Fact]
        public void WeatherShockRules_StormBand_IsMilderAndShorter()
        {
            var band = EconomyWeatherShockRules.TryGetWeatherShock(WeatherKind.Ashfall);
            Assert.NotNull(band);
            Assert.Equal("food", band!.CategoryId);
            Assert.Equal(1000f, band.SeverityBp, 5);
            Assert.Equal(2, band.DurationDays);
            Assert.Equal("weather_storm", band.SourceId);
        }

        [Fact]
        public void WeatherShockRules_CalmWeather_AppliesNoShock()
        {
            Assert.Null(EconomyWeatherShockRules.TryGetWeatherShock(WeatherKind.Clear));
            Assert.Null(EconomyWeatherShockRules.TryGetWeatherShock(WeatherKind.Overcast));
        }

        [Fact]
        public void WeatherShock_BandsAreBoundedAndDistinct()
        {
            // Severity stays within the MarketSystem clamp envelope and the
            // bands never invert (storm must not exceed blizzard).
            var blizzard = EconomyWeatherShockRules.TryGetWeatherShock(WeatherKind.Blizzard)!;
            var storm = EconomyWeatherShockRules.TryGetWeatherShock(WeatherKind.Ashfall)!;
            Assert.InRange(blizzard.SeverityBp, MarketSystem.ShockSeverityMinBp, MarketSystem.ShockSeverityMaxBp);
            Assert.InRange(storm.SeverityBp, MarketSystem.ShockSeverityMinBp, MarketSystem.ShockSeverityMaxBp);
            Assert.True(storm.SeverityBp < blizzard.SeverityBp);
            Assert.True(storm.DurationDays <= blizzard.DurationDays);
            // Distinct source ids keep both shocks independently refreshable.
            Assert.NotEqual(blizzard.SourceId, storm.SourceId);
        }

        // ── Commodity catalog reachable through the market ───────────

        [Fact]
        public void CommodityCatalog_BindsThroughMarket_FromRealDataDir()
        {
            var load = CommodityBaselineCatalogLoader.Load(
                GetDataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.False(load.HasErrors, string.Join("; ", load.Errors));
            var market = new MarketSystem();
            market.BindCommodityCatalog(CommodityBaselineCatalogLoader.ToCatalog(load));
            // Every authored category is queryable through the market API —
            // no orphan rows, no dead catalog.
            foreach (var baseline in load.Categories)
                Assert.NotNull(market.FindCommodityBaseline(baseline.category_id));
        }

        // ── Save registration regression ─────────────────────────────

        [Fact]
        public void EconomySaveSection_RemainsSingleAuthority()
        {
            // Plan 212 is additive to the existing economy section — exactly
            // one market save section may exist, never a second price store.
            Assert.Equal(1, SaveSectionRegistry.All.Count(s => s.SectionKey == "economy"));
            Assert.Equal(1, SaveSectionRegistry.All.Count(s => s.SaveMethod == "SaveEconomy"));
        }
    }
}
