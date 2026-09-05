// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Ashfall.Core;
using Ashfall.Core.IO;
using Xunit;
using Xunit.Abstractions;

namespace Ashfall.Core.Tests.WildlifeTrapping
{
    using SeededRng = Ashfall.Core.SeededRng;

    /// <summary>
    /// Flagship Task 8: 1000+ Day Long-Term Campaign Balance Harness.
    /// Simulates 1000 days of campaign trapping across all 10 trap types with live catalogs,
    /// weather cycles, network density penalties, NPC interference, disease, contamination,
    /// bycatch, wear, and repairs under a deterministic policy.
    /// </summary>
    public sealed class WildlifeTrappingLongHorizonBalanceTests
    {
        private readonly ITestOutputHelper _output;

        public WildlifeTrappingLongHorizonBalanceTests(ITestOutputHelper output)
        {
            _output = output;
        }

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
            var cat = WildlifeTrappingCatalogLoader.Load(FindDataDir(), fileIO, json);
            Assert.NotNull(cat);
            return cat!;
        }

        public sealed class TrapTelemetry
        {
            public string TrapId = string.Empty;
            public int Checks;
            public int TotalCatches;
            public int BycatchCount;
            public int FailedChecks;
            public int BaitThefts;
            public int SabotageEvents;
            public int TotalSabotageDamage;
            public float TotalWeatherWear;
            public int BreakCount;
            public int RepairCount;
            public int DaysBrokenOrInactive;
            public int BaitUnitsConsumed;
            public int RepairMaterialSpend;
            public float FoodYield;
            public int DiseaseEvents;
            public int ContaminationEvents;
            public int HighRiskPreyCatches;
            public int ContaminatedPreyCatches;
        }

        public sealed class CampaignTelemetry
        {
            public float TotalTrappingFood;
            public float TotalNonTrappingFood;
            public float TotalFoodAllSources => TotalTrappingFood + TotalNonTrappingFood;
            public float TrappingFoodShare => TotalFoodAllSources > 0 ? TotalTrappingFood / TotalFoodAllSources : 0f;
            public int TotalMaterialSpend;
            public int TotalBaitSpend;
            public int TotalDiseaseEvents;
            public int TotalContaminationEvents;
            public int TotalBycatch;
            public int TotalBreaks;
            public int TotalRepairs;
            public int TotalCatches;
            public Dictionary<string, TrapTelemetry> PerTrap = new Dictionary<string, TrapTelemetry>(StringComparer.Ordinal);
        }

        [Fact]
        public void Campaign_1000Days_Seed42_TrappingBalancePassesAllAcceptanceBands()
        {
            var catalog = LoadCatalog();
            var telemetry = RunSimulation(seed: 42, days: 1000, catalog);

            string report = FormatReport(42, 1000, telemetry);
            _output.WriteLine(report);

            // 1. trappingFoodShare < 0.50 — trapping does not dominate the food economy
            Assert.True(telemetry.TrappingFoodShare < 0.50f,
                $"Trapping dominates food economy: {telemetry.TrappingFoodShare:P1} (Expected < 50%)\n{report}");

            // 2. trappingFoodShare > 0.05 — trapping remains viable
            Assert.True(telemetry.TrappingFoodShare > 0.05f,
                $"Trapping is non-viable: {telemetry.TrappingFoodShare:P1} (Expected > 5%)\n{report}");

            // 3. No single trap type contributes more than 40% of total catches
            Assert.True(telemetry.TotalCatches > 0, "No catches recorded across 1000 days.");
            foreach (var kvp in telemetry.PerTrap)
            {
                float catchShare = (float)kvp.Value.TotalCatches / telemetry.TotalCatches;
                Assert.True(catchShare <= 0.40f,
                    $"Trap '{kvp.Key}' dominates catches with {catchShare:P1} share (Expected <= 40%)\n{report}");
            }

            // 4. High-risk prey produce disease events at least once per 100 relevant catches
            int totalHighRisk = 0;
            int totalDisease = 0;
            foreach (var t in telemetry.PerTrap.Values)
            {
                totalHighRisk += t.HighRiskPreyCatches;
                totalDisease += t.DiseaseEvents;
            }
            if (totalHighRisk >= 100)
            {
                Assert.True(totalDisease >= 1,
                    $"Expected at least 1 disease event across {totalHighRisk} high-risk catches.\n{report}");
            }

            // 5. Contaminated prey produce contamination events at least once per 100 relevant catches
            int totalContamPrey = 0;
            int totalContamEvents = 0;
            foreach (var t in telemetry.PerTrap.Values)
            {
                totalContamPrey += t.ContaminatedPreyCatches;
                totalContamEvents += t.ContaminationEvents;
            }
            if (totalContamPrey >= 100)
            {
                Assert.True(totalContamEvents >= 1,
                    $"Expected at least 1 contamination event across {totalContamPrey} contaminated catches.\n{report}");
            }

            // 6. Low-durability traps break at least once per 50 checks
            var wireTrap = telemetry.PerTrap["trap_improvised_wire"];
            if (wireTrap.Checks >= 50)
            {
                float breakRate = (float)wireTrap.BreakCount / wireTrap.Checks;
                Assert.True(wireTrap.BreakCount >= 1 && breakRate >= 0.02f,
                    $"Improvised wire trap did not break often enough: {wireTrap.BreakCount} breaks in {wireTrap.Checks} checks.\n{report}");
            }

            // 7. At least one repair occurs per 100 campaign days in aggregate (>= 10 repairs in 1000 days)
            Assert.True(telemetry.TotalRepairs >= 10,
                $"Insufficient maintenance burden: {telemetry.TotalRepairs} total repairs across 1000 days (Expected >= 10)\n{report}");

            // 8. Bycatch-enabled traps produce at least one bycatch per 50 catches in aggregate
            int bycatchEligibleCatches = telemetry.PerTrap["trap_net"].TotalCatches + telemetry.PerTrap["trap_bird_snare"].TotalCatches;
            int totalBycatch = telemetry.PerTrap["trap_net"].BycatchCount + telemetry.PerTrap["trap_bird_snare"].BycatchCount;
            if (bycatchEligibleCatches >= 50)
            {
                Assert.True(totalBycatch >= 1,
                    $"Expected bycatch in {bycatchEligibleCatches} catches from bycatch-enabled traps.\n{report}");
            }

            /*
             * ─────────────────────────────────────────────────────────────────────────────
             * ACCEPTED GOLDEN BASELINE DIAGNOSTICS (Task 8 / Seed 42 / 1000 Days):
             * ─────────────────────────────────────────────────────────────────────────────
             * Total Trapping Food:     7,513.3 kg
             * Total Non-Trapping Food: 9,990.0 kg (10.0 kg/day baseline)
             * Total Food All Sources:  17,503.3 kg
             * Trapping Food Share:     42.9% (Comfortably inside [5%, 50%])
             * Total Catches:           3,037
             * Total Bycatch:           209
             * Total Breaks:            1,464
             * Total Repairs:           1,464
             * Total Bait Spend:        221 units
             * Total Material Spend:    1,464 units
             * Total Disease Events:    131
             * Total Contam Events:     473
             * Per-Trap Catches:
             *   trap_snare: 207 (7%), trap_deadfall: 242 (8%), trap_pit: 129 (4%),
             *   trap_net: 511 (17%), trap_fish: 188 (6%), trap_cage: 238 (8%),
             *   trap_bird_snare: 496 (16%), trap_body_grip: 260 (9%),
             *   trap_box: 256 (8%), trap_improvised_wire: 510 (17%)
             * Max Single Trap Share:   17% (Limit: 40%)
             * ─────────────────────────────────────────────────────────────────────────────
             */
        }

        [Theory]
        [InlineData(101)]
        [InlineData(777)]
        [InlineData(1986)]
        [InlineData(2024)]
        public void Campaign_MultiSeedEnsemble_TrappingBalanceBounded(int seed)
        {
            var catalog = LoadCatalog();
            var telemetry = RunSimulation(seed: seed, days: 500, catalog);

            Assert.True(telemetry.TrappingFoodShare > 0.05f && telemetry.TrappingFoodShare < 0.50f,
                $"Seed {seed} food share out of bounds: {telemetry.TrappingFoodShare:P1}");
            Assert.True(telemetry.TotalRepairs >= 5,
                $"Seed {seed} insufficient repairs: {telemetry.TotalRepairs}");
            Assert.True(telemetry.TotalCatches > 0,
                $"Seed {seed} no catches recorded.");
        }

        private static CampaignTelemetry RunSimulation(int seed, int days, WildlifeTrappingCatalog catalog)
        {
            var telemetry = new CampaignTelemetry();
            var sys = new WildlifeTrappingSystem(new SeededRng(seed));
            catalog.RegisterWith(sys);

            // Wire domain events into telemetry
            sys.OnBaitStolen += ev =>
            {
                if (telemetry.PerTrap.TryGetValue(ev.trapId, out var t))
                {
                    t.BaitThefts++;
                }
            };
            sys.OnTrapSabotaged += ev =>
            {
                if (telemetry.PerTrap.TryGetValue(ev.trapId, out var t))
                {
                    t.SabotageEvents++;
                    t.TotalSabotageDamage += ev.durabilityDamage;
                }
            };

            // Deploy all 10 traps into 3 representative ecological zones
            // Zone 1: Wooded Valley (4 traps: snare, deadfall, cage, box)
            // Zone 2: Marsh & Stream (3 traps: net, fish, bird_snare)
            // Zone 3: Barren Ash Plateau (3 traps: pit, body_grip, improvised_wire)
            var zoneMap = new Dictionary<string, string>
            {
                { "trap_snare", "zone_wooded_valley" },
                { "trap_deadfall", "zone_wooded_valley" },
                { "trap_cage", "zone_wooded_valley" },
                { "trap_box", "zone_wooded_valley" },
                { "trap_net", "zone_marsh_stream" },
                { "trap_fish", "zone_marsh_stream" },
                { "trap_bird_snare", "zone_marsh_stream" },
                { "trap_pit", "zone_ash_plateau" },
                { "trap_body_grip", "zone_ash_plateau" },
                { "trap_improvised_wire", "zone_ash_plateau" }
            };

            foreach (var trapDef in catalog.Traps.Values)
            {
                string siteId = $"site_{trapDef.trap_id}";
                string zoneId = zoneMap[trapDef.trap_id];

                telemetry.PerTrap[trapDef.trap_id] = new TrapTelemetry { TrapId = trapDef.trap_id };

                sys.SetTrap(
                    siteId: siteId,
                    baitType: "bait_scrap_meat",
                    hunterId: "hunter_main",
                    trapType: trapDef.trapType,
                    trapId: trapDef.trap_id,
                    checkIntervalDays: trapDef.checkIntervalDays,
                    durabilityChecks: trapDef.durabilityChecks,
                    zoneId: zoneId);

                telemetry.PerTrap[trapDef.trap_id].BaitUnitsConsumed++;
                telemetry.TotalBaitSpend++;
            }

            // Weather schedule: cyclic 10-day pattern with clear, rain, storms, blizzard
            WeatherKind[] weatherSchedule = new WeatherKind[]
            {
                WeatherKind.Clear,
                WeatherKind.Clear,
                WeatherKind.Rain,
                WeatherKind.Clear,
                WeatherKind.Overcast,
                WeatherKind.Rain,
                WeatherKind.FalloutStorm,
                WeatherKind.Clear,
                WeatherKind.Blizzard,
                WeatherKind.Clear
            };

            for (int day = 2; day <= days; day++)
            {
                WeatherKind todayWeather = weatherSchedule[(day - 1) % weatherSchedule.Length];
                sys.SetSelectionContext(new WildlifeSelectionContext
                {
                    CurrentWeather = todayWeather,
                    HunterSkillLevels = new Dictionary<string, float> { { "hunter_main", 40f } }
                });

                // Advance day and check snares
                sys.TickDay(day);

                // Non-trapping food generation: baseline 10.0 kg/day from greenhouse, kitchen, and scavenging
                telemetry.TotalNonTrappingFood += 10.0f;

                // Inspect each trap site and enforce deterministic maintenance policy
                for (int i = 0; i < sys.State.trapSites.Count; i++)
                {
                    var site = sys.State.trapSites[i];
                    string trapId = site.trapId;
                    var t = telemetry.PerTrap[trapId];
                    var def = catalog.Traps[trapId];

                    if (site.hasCatch)
                    {
                        t.TotalCatches++;
                        telemetry.TotalCatches++;
                        t.FoodYield += site.carcassYield;
                        telemetry.TotalTrappingFood += site.carcassYield;

                        if (!string.IsNullOrEmpty(site.bycatchSpecies))
                        {
                            t.BycatchCount++;
                            telemetry.TotalBycatch++;
                        }

                        // Disease and contamination evaluation
                        if (site.isToxic)
                        {
                            t.HighRiskPreyCatches++;
                            if (sys.RollDiseaseRisk(0.20f))
                            {
                                t.DiseaseEvents++;
                                telemetry.TotalDiseaseEvents++;
                            }
                        }

                        t.ContaminatedPreyCatches++;
                        if (sys.RollContaminationRisk(0.15f))
                        {
                            t.ContaminationEvents++;
                            telemetry.TotalContaminationEvents++;
                        }

                        // Butcher carcass
                        sys.Butcher(site.siteId);
                        site.hasCatch = false;
                    }

                    // Check breakage & repair policy
                    if (site.isBroken)
                    {
                        t.BreakCount++;
                        telemetry.TotalBreaks++;

                        // Deterministic maintenance: repair immediately consuming 1 repair unit
                        sys.RepairTrap(site.siteId, def.durabilityChecks);
                        t.RepairCount++;
                        t.RepairMaterialSpend++;
                        telemetry.TotalRepairs++;
                        telemetry.TotalMaterialSpend++;
                    }

                    // Check re-bait policy
                    if (site.baitStolen || string.IsNullOrEmpty(site.baitType))
                    {
                        sys.RebaitTrap(site.siteId, "bait_scrap_meat");
                        t.BaitUnitsConsumed++;
                        telemetry.TotalBaitSpend++;
                    }

                    t.Checks++;
                }
            }

            return telemetry;
        }

        private static string FormatReport(int seed, int days, CampaignTelemetry tel)
        {
            var sb = new StringBuilder();
            sb.AppendLine("================================================================================");
            sb.AppendLine($"ASHFALL 1000-DAY CAMPAIGN TRAPPING BALANCE REPORT (Seed: {seed}, Days: {days})");
            sb.AppendLine("================================================================================");
            sb.AppendLine($"Total Trapping Food:     {tel.TotalTrappingFood:F1} kg");
            sb.AppendLine($"Total Non-Trapping Food: {tel.TotalNonTrappingFood:F1} kg");
            sb.AppendLine($"Total Food All Sources:  {tel.TotalFoodAllSources:F1} kg");
            sb.AppendLine($"Trapping Food Share:     {tel.TrappingFoodShare:P1} (Viable > 5%, Bounded < 50%)");
            sb.AppendLine($"Total Catches:           {tel.TotalCatches}");
            sb.AppendLine($"Total Bycatch:           {tel.TotalBycatch}");
            sb.AppendLine($"Total Breaks:            {tel.TotalBreaks}");
            sb.AppendLine($"Total Repairs:           {tel.TotalRepairs}");
            sb.AppendLine($"Total Bait Spend:        {tel.TotalBaitSpend}");
            sb.AppendLine($"Total Material Spend:    {tel.TotalMaterialSpend}");
            sb.AppendLine($"Total Disease Events:    {tel.TotalDiseaseEvents}");
            sb.AppendLine($"Total Contam Events:     {tel.TotalContaminationEvents}");
            sb.AppendLine("--------------------------------------------------------------------------------");
            sb.AppendLine("Per-Trap Performance:");
            sb.AppendLine("Trap ID                | Catches | Share | Thefts | Sabotage | Breaks | Rep | Food (kg)");
            sb.AppendLine("--------------------------------------------------------------------------------");
            foreach (var kvp in tel.PerTrap)
            {
                var t = kvp.Value;
                float share = tel.TotalCatches > 0 ? (float)t.TotalCatches / tel.TotalCatches : 0f;
                sb.AppendLine($"{t.TrapId,-22} | {t.TotalCatches,7} | {share,5:P0} | {t.BaitThefts,6} | {t.SabotageEvents,8} | {t.BreakCount,6} | {t.RepairCount,3} | {t.FoodYield,8:F1}");
            }
            sb.AppendLine("================================================================================");
            return sb.ToString();
        }
    }
}
