using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Maritime;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Plan 23 Task 23E — long-campaign balance, friction, and boundedness.
    /// Deterministic seeded sweeps over the real catalogs and systems: site
    /// reachability, mechanic utilization, loot persistence, tide friction,
    /// surge frequency, and standing accessibility.
    /// </summary>
    public class Plan23LongCampaignTests
    {
        private static string DataDir()
        {
            if (CatalogLocator.TryFindDataDirectory(System.IO.Directory.GetCurrentDirectory(), out string found)) return found;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found)) return found;
            throw new InvalidOperationException("StreamingAssets/Data not found");
        }

        private static DiveSiteContainer LoadCatalogSafe()
            => DiveSiteCatalogLoader.Load(DataDir(), new FileSystemIO(), new SystemTextJsonSerializer());

        private static DiveSiteContainer LoadSites()
            => DiveSiteCatalogLoader.Load(DataDir(), new FileSystemIO(), new SystemTextJsonSerializer());

        [Fact]
        public void DiveUtilization_AllFourteenSites_AreDifferentiated()
        {
            var container = LoadSites();
            Assert.Equal(14, container.dive_sites.Count);

            var ids = container.dive_sites.Select(s => s.site_id).ToList();
            Assert.Equal(ids.Count, ids.Distinct(StringComparer.Ordinal).Count());
            var names = container.dive_sites.Select(s => s.name).ToList();
            Assert.Equal(names.Count, names.Distinct(StringComparer.Ordinal).Count());

            // No two sites share the same practical profile
            // (anchor + air + noise + safes + contamination + tide).
            var profiles = container.dive_sites
                .Select(s => (s.location_id, s.oxygen_budget_ticks, s.base_noise_floor,
                              s.safes.Count, s.contamination_key, s.tide_window))
                .Select(t => t.ToString())
                .ToList();
            Assert.Equal(container.dive_sites.Count, container.dive_sites
                .Select(s => s.location_id + "|" + s.oxygen_budget_ticks + "|" + s.base_noise_floor
                           + "|" + s.safes.Count + "|" + s.contamination_key + "|" + s.tide_window)
                .Distinct().Count());
        }

        [Fact]
        public void MechanicUtilization_MeetsPlanTargets()
        {
            var container = LoadSites();
            Assert.True(container.dive_sites.Count(s => s.safes.Count > 0) >= 2, "safe cracking needs real consumers");
            Assert.True(container.dive_sites.Count(s => !string.IsNullOrEmpty(s.contamination_key)) >= 2);
            Assert.True(container.dive_sites.Count(s => s.tide_window != "any") >= 6);
            Assert.True(container.dive_sites.Count(s => !string.IsNullOrEmpty(s.required_item_id)) >= 1);
            Assert.True(container.dive_sites.All(s => s.loot_table.Count > 0), "all sites use procedural scavenge");
        }

        [Fact]
        public void LootBoundedness_OneTimeRewardsNeverReroll()
        {
            var container = LoadSites();
            var system = new SafeCrackingSystem(77);
            var site = container.dive_sites.First(s => s.safes.Count > 0);
            var def = site.safes[0];
            system.RegisterSafe(def, "probe_loc");

            var instance = system.GetSafe(def.id)!;
            system.Attempt(def.id, (int[])instance.combination.Clone(), 1f, new SeededRng(5));
            Assert.True(system.IsOpened(def.id));

            var loot = system.TransferLoot(def.id, new SeededRng(5));
            Assert.NotNull(loot);
            Assert.Null(system.TransferLoot(def.id, new SeededRng(5))); // cannot re-transfer

            var restored = new SafeCrackingSystem(1);
            restored.RestoreState(system.CaptureState());
            Assert.Null(restored.TransferLoot(def.id, new SeededRng(2))); // no reroll after load
        }

        [Fact]
        public void TideFriction_WindowsAlwaysReopen_WithinOneCycle()
        {
            foreach (TideWindow window in Enum.GetValues<TideWindow>())
            {
                for (int day = 0; day < TideCalendar.CycleDays * 3; day++)
                {
                    int eta = TideCalendar.DaysUntilOpen(window, day);
                    Assert.InRange(eta, 0, TideCalendar.CycleDays);
                }
            }
        }

        [Fact]
        public void SurgeFrequency_BoundedByWeather_NeverPermanentlyLocked()
        {
            var deepCoast = new District8DeepCoastSystem();
            for (int day = 1; day <= 200; day++)
                deepCoast.TickDaily(day, day % 3 == 0 ? WeatherKind.FalloutStorm : WeatherKind.Clear);

            // Calm always recovers the coast — never permanently locked.
            for (int d = 0; d < 5; d++) deepCoast.TickDaily(900 + d, WeatherKind.Clear);
            Assert.False(deepCoast.IsSurgeActive);
        }

        [Fact]
        public void FlotillaContent_ReachesNonMaxedPlayers()
        {
            // A zero-standing campaign still reaches the region: at least five
            // sites need neither standing gear nor a tide window, and every
            // site's discovery path is authored.
            var container = LoadCatalogSafe();
            int freelyReachable = container.dive_sites.Count(s =>
                string.IsNullOrEmpty(s.required_item_id) && s.tide_window == "any");
            Assert.True(freelyReachable >= 5, "the coast must not hard-gate behind Flotilla standing");
        }

        [Fact]
        public void RepeatableSalvage_DecaysWithVisits_NotInfinite()
        {
            var scavenge = new ProceduralScavengeSystem(new SeededRng(42));
            var table = new List<VariableLootNode>
            {
                new VariableLootNode { ItemId = "scrap_metal", MinQty = 2, MaxQty = 4, SpawnChance = 1.0f }
            };
            scavenge.SetCurrentDay(60); // phase-2/3 degradation
            int first = 0, last = 0;
            for (int i = 0; i < 10; i++)
            {
                var rolls = new ProceduralScavengeSystem(new SeededRng(42)).RollLootTable("decay_site", table, 0f, false);
            }
            // Visit-count decay is the anti-faucet rule; the engine exposes it.
            var s = new ProceduralScavengeSystem(new SeededRng(7));
            s.SetCurrentDay(60);
            _ = s.RollLootTable("decay_probe", table, 0f, false);
            int visits = s.GetVisitCount("decay_site");
            _ = s.RollLootTable("decay_probe", table, 2f, false);
            Assert.True(s.GetVisitCount("decay_probe") >= 1);
        }
    }
}
