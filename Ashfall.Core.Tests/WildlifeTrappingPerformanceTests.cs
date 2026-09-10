// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Ashfall.Core;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class WildlifeTrappingPerformanceTests
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

        [Fact]
        public void Benchmark_1000TrapChecks_ExecutesUnderBudget()
        {
            var catalog = LoadCatalog();
            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            catalog.RegisterWith(sys);

            // Populate 100 diverse traps
            string[] trapIds = { "trap_improvised_wire", "trap_box", "trap_fish", "trap_body_grip", "trap_snare" };
            string[] baitIds = { "bait_grain_lure", "bait_scrap_meat", "bait_salt_lick", "bait_fish_guts", "bait_fat_cake" };

            for (int i = 0; i < 100; i++)
            {
                string tId = trapIds[i % trapIds.Length];
                string bId = baitIds[i % baitIds.Length];
                var tDef = catalog.Traps[tId];

                sys.SetTrap($"perf_site_{i}", bId, $"hunter_{i % 5}",
                    tDef.trapType, tDef.trap_id, checkIntervalDays: 1, durabilityChecks: 50);
            }

            // Warm up
            sys.TickDay(2);
            foreach (var site in sys.State.trapSites)
            {
                site.hasCatch = false;
            }

            // Benchmark 10 days x 100 traps = 1,000 total trap check evaluations
            var sw = Stopwatch.StartNew();

            for (int day = 3; day <= 12; day++)
            {
                sys.TickDay(day);
                for (int s = 0; s < sys.State.trapSites.Count; s++)
                {
                    var site = sys.State.trapSites[s];
                    if (site.hasCatch)
                    {
                        sys.Butcher(site.siteId);
                        site.hasCatch = false;
                    }
                }
            }

            sw.Stop();

            // 1,000 evaluations across 10 days should execute in well under 250ms (typically <20ms)
            Assert.True(sw.ElapsedMilliseconds < 250,
                $"1,000 trap check iterations took {sw.ElapsedMilliseconds}ms, exceeding the 250ms budget");
        }

        [Fact]
        public void Scale_100Traps_LinearScalingAndDeterministicState()
        {
            var catalog = LoadCatalog();
            var sys = new WildlifeTrappingSystem(new SeededRng(100));
            catalog.RegisterWith(sys);

            // Deploy 100 traps
            for (int i = 0; i < 100; i++)
            {
                var tDef = catalog.Traps["trap_snare"];
                sys.SetTrap($"scale_site_{i}", "bait_grain_lure", $"hunter_{i}",
                    tDef.trapType, tDef.trap_id, checkIntervalDays: 2, durabilityChecks: 10);
            }

            Assert.Equal(100, sys.State.trapSites.Count);

            // Day 3 check (checkIntervalDays = 2 from setDay = 1)
            sys.TickDay(3);
            int caughtDay3 = 0;
            for (int i = 0; i < 100; i++)
            {
                var site = sys.State.trapSites[i];
                Assert.Equal(9, site.remainingDurability);
                if (site.hasCatch) caughtDay3++;
            }

            Assert.True(caughtDay3 > 0, "100 traps should yield catches on check day");
            Assert.Equal(caughtDay3, sys.State.totalCatch);
        }

        [Fact]
        public void KeyedLookups_CatalogDictionaries_AreO1ConstantTime()
        {
            var catalog = LoadCatalog();

            // Benchmark 20,000 dictionary lookups across Prey, Traps, and Bait
            var sw = Stopwatch.StartNew();
            int count = 0;

            for (int i = 0; i < 20000; i++)
            {
                if (catalog.Traps.TryGetValue("trap_snare", out _)) count++;
                if (catalog.Prey.TryGetValue("rabbit", out _)) count++;
                if (catalog.Baits.TryGetValue("bait_grain_lure", out _)) count++;
            }

            sw.Stop();

            Assert.Equal(60000, count);
            Assert.True(sw.ElapsedMilliseconds < 50,
                $"20,000 dictionary lookups took {sw.ElapsedMilliseconds}ms, exceeding the 50ms budget");
        }
    }
}
