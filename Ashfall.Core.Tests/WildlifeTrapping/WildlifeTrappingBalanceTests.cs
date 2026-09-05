using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;
using Ashfall.Core;

namespace Ashfall.Core.Tests.WildlifeTrapping
{
    public class WildlifeTrappingBalanceTests
    {
        private static string DataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found))
                return found;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found))
                return found;
            return string.Empty;
        }

        private static (WildlifeTrappingSystem sys, WildlifeTrappingCatalog cat) CreateFixture(int seed = 42)
        {
            var rng = new SeededRng(seed);
            var sys = new WildlifeTrappingSystem(rng, new NullLog());
            string dataDir = DataDir();
            Assert.False(string.IsNullOrEmpty(dataDir), "StreamingAssets/Data directory must exist");
            var cat = WildlifeTrappingCatalogLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer(), new NullLog());
            Assert.NotNull(cat);
            cat!.RegisterWith(sys);
            return (sys, cat);
        }

        private static string FormatDiagnosticTable(string trapType, int checks, int catches, double catchRate, int breaks, double avgLife, int diseaseHits = 0, int contamHits = 0)
        {
            var sb = new StringBuilder();
            sb.AppendLine("\nTRAPPING BALANCE DIAGNOSTIC TABLE:");
            sb.AppendLine("| Trap Type            | Checks | Catches | Catch Rate | Breaks | Avg Life | Disease | Contam |");
            sb.AppendLine("|----------------------|--------|---------|------------|--------|----------|---------|--------|");
            sb.AppendLine($"| {trapType,-20} | {checks,6} | {catches,7} | {catchRate,9:P1} | {breaks,6} | {avgLife,8:F1} | {diseaseHits,7} | {contamHits,6} |");
            return sb.ToString();
        }

        [Fact]
        public void C1_WireSnare_CatchRate_WithinAntiFrustrationAndAntiDominanceBounds()
        {
            var (sys, cat) = CreateFixture(seed: 42);

            // Deploy 10 wire snares
            const int trapCount = 10;
            const int cycles = 100; // 100 checks per trap = 1,000 total trap-checks
            for (int i = 0; i < trapCount; i++)
            {
                sys.State.trapSites.Add(new TrapSite
                {
                    siteId = $"snare_{i}",
                    trapId = "trap_snare",
                    trapType = "snare",
                    baitType = "bait_grain_lure",
                    assignedHunterId = "Hunter",
                    setDay = 0,
                    checkDay = 0,
                    checkIntervalDays = 2,
                    remainingDurability = 8,
                    isBroken = false
                });
            }

            int totalChecks = 0;
            int totalCatches = 0;
            int totalBreaks = 0;
            var completedLifetimes = new List<int>();
            var currentLife = new int[trapCount];

            for (int day = 0; day < cycles * 2; day += 2)
            {
                sys.TickDay(day);
                var result = sys.CheckTraps(densityMultiplier: 1.0f);

                for (int i = 0; i < sys.State.trapSites.Count; i++)
                {
                    var site = sys.State.trapSites[i];
                    totalChecks++;
                    currentLife[i]++;

                    if (site.hasCatch)
                    {
                        totalCatches++;
                        // Process / butcher catch and reset for next run
                        site.hasCatch = false;
                        site.catchSpecies = string.Empty;
                        site.baitStolen = false;
                        site.baitType = "bait_grain_lure";
                    }

                    if (site.isBroken)
                    {
                        totalBreaks++;
                        completedLifetimes.Add(currentLife[i]);
                        currentLife[i] = 0;

                        // Redeploy fresh trap
                        site.isBroken = false;
                        site.remainingDurability = 8;
                        site.pendingWeatherWear = 0f;
                        site.baitStolen = false;
                        site.baitType = "bait_grain_lure";
                    }
                }
            }

            double catchRate = (double)totalCatches / totalChecks;
            double avgLife = completedLifetimes.Count > 0
                ? (double)System.Linq.Enumerable.Average(completedLifetimes)
                : 0.0;

            string table = FormatDiagnosticTable("Wire Snare", totalChecks, totalCatches, catchRate, totalBreaks, avgLife);

            // Anti-frustration floor: player must not starve unconditionally (>= 3%)
            Assert.True(catchRate >= 0.03, $"Catch rate too low ({catchRate:P1} < 3%):{table}");

            // Anti-dominance ceiling: trapping must not obsolete agriculture/foraging (<= 35%)
            Assert.True(catchRate <= 0.35, $"Catch rate too high ({catchRate:P1} > 35%):{table}");
        }

        [Fact]
        public void C2_C3_SpecialistTrap_CageVsSnare_DurabilityAndDifferentiation()
        {
            var (sys, cat) = CreateFixture(seed: 42);

            const int trapsPerType = 5;
            const int cycles = 150;

            // 5 Snares (initial durability 8, high weather sensitivity/degradation)
            for (int i = 0; i < trapsPerType; i++)
            {
                sys.State.trapSites.Add(new TrapSite
                {
                    siteId = $"snare_{i}",
                    trapId = "trap_snare",
                    trapType = "snare",
                    baitType = "bait_scrap_meat",
                    setDay = 0,
                    checkDay = 0,
                    checkIntervalDays = 2,
                    remainingDurability = 8,
                    isBroken = false
                });
            }

            // 5 Cages (initial durability 15, low weather degradation)
            for (int i = 0; i < trapsPerType; i++)
            {
                sys.State.trapSites.Add(new TrapSite
                {
                    siteId = $"cage_{i}",
                    trapId = "trap_cage",
                    trapType = "cage",
                    baitType = "bait_scrap_meat",
                    setDay = 0,
                    checkDay = 0,
                    checkIntervalDays = 2,
                    remainingDurability = 15,
                    isBroken = false
                });
            }

            int snareChecks = 0, snareCatches = 0, snareBreaks = 0;
            int cageChecks = 0, cageCatches = 0, cageBreaks = 0;
            var snareLives = new List<int>();
            var cageLives = new List<int>();
            var snareCurLife = new int[trapsPerType];
            var cageCurLife = new int[trapsPerType];

            for (int day = 0; day < cycles * 2; day += 2)
            {
                sys.TickDay(day);
                sys.CheckTraps(densityMultiplier: 1.0f);

                for (int i = 0; i < trapsPerType; i++)
                {
                    var s = sys.State.trapSites[i];
                    snareChecks++;
                    snareCurLife[i]++;
                    if (s.hasCatch)
                    {
                        snareCatches++;
                        s.hasCatch = false;
                        s.catchSpecies = string.Empty;
                        s.baitStolen = false;
                        s.baitType = "bait_scrap_meat";
                    }
                    if (s.isBroken)
                    {
                        snareBreaks++;
                        snareLives.Add(snareCurLife[i]);
                        snareCurLife[i] = 0;
                        s.isBroken = false;
                        s.remainingDurability = 8;
                        s.pendingWeatherWear = 0f;
                        s.baitStolen = false;
                        s.baitType = "bait_scrap_meat";
                    }
                }

                for (int i = 0; i < trapsPerType; i++)
                {
                    var c = sys.State.trapSites[trapsPerType + i];
                    cageChecks++;
                    cageCurLife[i]++;
                    if (c.hasCatch)
                    {
                        cageCatches++;
                        c.hasCatch = false;
                        c.catchSpecies = string.Empty;
                        c.baitStolen = false;
                        c.baitType = "bait_scrap_meat";
                    }
                    if (c.isBroken)
                    {
                        cageBreaks++;
                        cageLives.Add(cageCurLife[i]);
                        cageCurLife[i] = 0;
                        c.isBroken = false;
                        c.remainingDurability = 15;
                        c.pendingWeatherWear = 0f;
                        c.baitStolen = false;
                        c.baitType = "bait_scrap_meat";
                    }
                }
            }

            double snareAvgLife = snareLives.Count > 0 ? System.Linq.Enumerable.Average(snareLives) : 0.0;
            double cageAvgLife = cageLives.Count > 0 ? System.Linq.Enumerable.Average(cageLives) : 0.0;

            string diagnostic = FormatDiagnosticTable("Wire Snare", snareChecks, snareCatches, (double)snareCatches / snareChecks, snareBreaks, snareAvgLife)
                + FormatDiagnosticTable("Cage Trap", cageChecks, cageCatches, (double)cageCatches / cageChecks, cageBreaks, cageAvgLife);

            // Cage trap should survive significantly longer per deployment than wire snare
            Assert.True(cageAvgLife > snareAvgLife * 1.3,
                $"Cage trap average lifetime ({cageAvgLife:F1}) should exceed snare ({snareAvgLife:F1}) by at least 30%:\n{diagnostic}");
        }

        [Fact]
        public void C4_DurabilityRealism_WireSnare_BreaksWithinExpectedBounds()
        {
            var (sys, cat) = CreateFixture(seed: 42);

            const int trapCount = 10;
            const int cycles = 100;
            for (int i = 0; i < trapCount; i++)
            {
                sys.State.trapSites.Add(new TrapSite
                {
                    siteId = $"snare_{i}",
                    trapId = "trap_snare",
                    trapType = "snare",
                    baitType = "bait_grain_lure",
                    setDay = 0,
                    checkDay = 0,
                    checkIntervalDays = 2,
                    remainingDurability = 8,
                    isBroken = false
                });
            }

            int breaks = 0;
            var lifetimes = new List<int>();
            var currentLife = new int[trapCount];

            for (int day = 0; day < cycles * 2; day += 2)
            {
                sys.TickDay(day);
                sys.CheckTraps(densityMultiplier: 1.0f);

                for (int i = 0; i < trapCount; i++)
                {
                    var s = sys.State.trapSites[i];
                    currentLife[i]++;
                    if (s.hasCatch)
                    {
                        s.hasCatch = false;
                        s.baitStolen = false;
                        s.baitType = "bait_grain_lure";
                    }
                    if (s.isBroken)
                    {
                        breaks++;
                        lifetimes.Add(currentLife[i]);
                        currentLife[i] = 0;
                        s.isBroken = false;
                        s.remainingDurability = 8;
                        s.pendingWeatherWear = 0f;
                        s.baitStolen = false;
                        s.baitType = "bait_grain_lure";
                    }
                }
            }

            Assert.True(breaks > 0, "Traps must experience wear and break over extended cycles");
            double avgLife = System.Linq.Enumerable.Average(lifetimes);

            // Wire snare initial durability is 8; with weather and sabotage wear, average lifetime is typically 5 to 15 checks
            Assert.True(avgLife >= 4.0 && avgLife <= 16.0,
                $"Wire snare average lifetime ({avgLife:F1} checks) outside believable bounds [4, 16]");
        }

        [Fact]
        public void C5_MeatRiskBalance_RatCatches_ProduceProportionalDiseaseAndContamination()
        {
            var (sys, cat) = CreateFixture(seed: 42);

            // Rat has diseaseRisk = 0.35, contaminationRisk = 0.20
            const float diseaseRisk = 0.35f;
            const float contaminationRisk = 0.20f;
            const int sampleCatches = 100;

            int diseaseHits = 0;
            int contaminationHits = 0;

            for (int i = 0; i < sampleCatches; i++)
            {
                if (sys.RollDiseaseRisk(diseaseRisk))
                    diseaseHits++;
                if (sys.RollContaminationRisk(contaminationRisk))
                    contaminationHits++;
            }

            string diag = FormatDiagnosticTable("Rat Harvest Risk", sampleCatches, sampleCatches, 1.0, 0, 0, diseaseHits, contaminationHits);

            // Both hazards must be non-zero (risks are real) and non-100% (not guaranteed death)
            Assert.True(diseaseHits >= 20 && diseaseHits <= 50,
                $"Disease hits ({diseaseHits}/100) outside expected [20, 50] range for 35% risk:\n{diag}");
            Assert.True(contaminationHits >= 10 && contaminationHits <= 35,
                $"Contamination hits ({contaminationHits}/100) outside expected [10, 35] range for 20% risk:\n{diag}");
            Assert.True(diseaseHits > contaminationHits,
                $"Higher risk disease (35%) should produce more hits than lower risk contamination (20%):\n{diag}");
        }
    }
}
