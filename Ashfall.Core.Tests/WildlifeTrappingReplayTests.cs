// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Ashfall.Core;
using Ashfall.Core.Disease;
using Ashfall.Core.Radiation;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class WildlifeTrappingReplayTests
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

        public sealed record ButcheryReplayRecord(
            string SpeciesId,
            bool DiseaseHit,
            string DiseaseId,
            bool ContaminationHit,
            float ContaminationDose,
            float CarcassYield,
            bool IsToxic,
            int HealthHash
        );

        private static (ButcheryReplayRecord record, TrapSite site) ExecuteButchery(
            WildlifeTrappingSystem sys,
            string siteId,
            string butcherId,
            int currentDay)
        {
            var site = sys.State.trapSites.Find(s => s.siteId == siteId);
            Assert.NotNull(site);
            Assert.True(site!.hasCatch, "Trap must have a catch to butcher");

            var butcherRes = sys.Butcher(siteId, butcherId);
            Assert.True(butcherRes.IsSuccess);

            bool diseaseHit = !string.IsNullOrEmpty(site.diseaseId);
            string diseaseId = site.diseaseId;
            bool contaminationHit = site.contaminationDose > 0f;
            float dose = site.contaminationDose;

            // Deterministic health hash combining survivor, disease, and radiation dose
            int healthHash = HashCode.Combine(
                butcherId,
                diseaseHit ? diseaseId : "none",
                (int)(dose * 100f),
                currentDay);

            var record = new ButcheryReplayRecord(
                site.catchSpecies,
                diseaseHit,
                diseaseId,
                contaminationHit,
                dose,
                site.carcassYield,
                site.isToxic,
                healthHash);

            return (record, site);
        }

        [Fact]
        public void ButcheryReplay_SaveLoadBoundary_Seed42_PreservesDiseaseAndContaminationOutcome()
        {
            const int seed = 42;
            var catalog = LoadCatalog();

            // System A: uninterrupted simulation
            var sysA = new WildlifeTrappingSystem(new SeededRng(seed));
            catalog.RegisterWith(sysA);

            // System B: simulation interrupted by save
            var sysB = new WildlifeTrappingSystem(new SeededRng(seed));
            catalog.RegisterWith(sysB);

            // Set identical traps at site_1
            var trapDef = catalog.Traps["trap_snare"];
            sysA.SetTrap("site_1", "bait_scrap_meat", "hunter_dweller",
                trapDef.trapType, trapDef.trap_id, trapDef.checkIntervalDays, trapDef.durabilityChecks);
            sysB.SetTrap("site_1", "bait_scrap_meat", "hunter_dweller",
                trapDef.trapType, trapDef.trap_id, trapDef.checkIntervalDays, trapDef.durabilityChecks);

            // Advance both systems until a catch occurs
            int catchDay = -1;
            for (int day = 2; day <= 20; day += 2)
            {
                sysA.TickDay(day);
                sysB.TickDay(day);
                if (sysA.State.trapSites[0].hasCatch)
                {
                    catchDay = day;
                    break;
                }
            }

            Assert.True(catchDay > 0, "Seed 42 should generate a catch within 20 days");
            Assert.True(sysB.State.trapSites[0].hasCatch);
            Assert.Equal(sysA.State.trapSites[0].catchSpecies, sysB.State.trapSites[0].catchSpecies);

            // System A butchers immediately
            var (outcomeA, siteA) = ExecuteButchery(sysA, "site_1", "dweller_butcher", catchDay);

            // System B captures state at the exact pre-butchery boundary
            var capturedB = sysB.CaptureState();
            var serializer = new SystemTextJsonSerializer();
            string json = serializer.Serialize(capturedB);

            // System C: fresh instance restoring captured state
            var restoredB = serializer.Deserialize<WildlifeTrappingState>(json);
            Assert.NotNull(restoredB);

            var sysC = new WildlifeTrappingSystem(new SeededRng(seed));
            catalog.RegisterWith(sysC);
            sysC.RestoreState(restoredB!);

            // System C butchers the restored catch
            var (outcomeC, siteC) = ExecuteButchery(sysC, "site_1", "dweller_butcher", catchDay);

            // Assert exact equality between uninterrupted (A) and saved/restored (C) executions
            Assert.Equal(outcomeA.SpeciesId, outcomeC.SpeciesId);
            Assert.Equal(outcomeA.DiseaseHit, outcomeC.DiseaseHit);
            Assert.Equal(outcomeA.DiseaseId, outcomeC.DiseaseId);
            Assert.Equal(outcomeA.ContaminationHit, outcomeC.ContaminationHit);
            Assert.Equal(outcomeA.ContaminationDose, outcomeC.ContaminationDose, precision: 3);
            Assert.Equal(outcomeA.CarcassYield, outcomeC.CarcassYield, precision: 3);
            Assert.Equal(outcomeA.IsToxic, outcomeC.IsToxic);
            Assert.Equal(outcomeA.HealthHash, outcomeC.HealthHash);

            // Ensure site state is not duplicated or double-applied
            Assert.True(siteC.isMeatProcessed);
        }

        [Fact]
        public void ButcheryReplay_SaveLoadBoundary_Seed123_PreservesDiseaseAndContaminationOutcome()
        {
            const int seed = 123;
            var catalog = LoadCatalog();

            var sysA = new WildlifeTrappingSystem(new SeededRng(seed));
            catalog.RegisterWith(sysA);

            var sysB = new WildlifeTrappingSystem(new SeededRng(seed));
            catalog.RegisterWith(sysB);

            var trapDef = catalog.Traps["trap_snare"];
            sysA.SetTrap("site_1", "bait_scrap_meat", "hunter_dweller",
                trapDef.trapType, trapDef.trap_id, trapDef.checkIntervalDays, trapDef.durabilityChecks);
            sysB.SetTrap("site_1", "bait_scrap_meat", "hunter_dweller",
                trapDef.trapType, trapDef.trap_id, trapDef.checkIntervalDays, trapDef.durabilityChecks);

            int catchDay = -1;
            for (int day = 2; day <= 20; day += 2)
            {
                sysA.TickDay(day);
                sysB.TickDay(day);
                if (sysA.State.trapSites[0].hasCatch)
                {
                    catchDay = day;
                    break;
                }
            }

            Assert.True(catchDay > 0, "Seed 123 should generate a catch within 20 days");

            var (outcomeA, _) = ExecuteButchery(sysA, "site_1", "dweller_butcher", catchDay);

            var serializer = new SystemTextJsonSerializer();
            string json = serializer.Serialize(sysB.CaptureState());
            var restored = serializer.Deserialize<WildlifeTrappingState>(json);
            Assert.NotNull(restored);

            var sysC = new WildlifeTrappingSystem(new SeededRng(seed));
            catalog.RegisterWith(sysC);
            sysC.RestoreState(restored!);

            var (outcomeC, _) = ExecuteButchery(sysC, "site_1", "dweller_butcher", catchDay);

            Assert.Equal(outcomeA.SpeciesId, outcomeC.SpeciesId);
            Assert.Equal(outcomeA.DiseaseHit, outcomeC.DiseaseHit);
            Assert.Equal(outcomeA.DiseaseId, outcomeC.DiseaseId);
            Assert.Equal(outcomeA.ContaminationHit, outcomeC.ContaminationHit);
            Assert.Equal(outcomeA.ContaminationDose, outcomeC.ContaminationDose, precision: 3);
            Assert.Equal(outcomeA.HealthHash, outcomeC.HealthHash);
        }

        [Fact]
        public void ButcheryReplay_LowRiskPrey_RemainsDeterministic()
        {
            var catalog = LoadCatalog();
            const int seed = 42;

            // System A with seed 42
            var sysA = new WildlifeTrappingSystem(new SeededRng(seed));
            catalog.RegisterWith(sysA);

            // Configure selection context where only rabbit is eligible
            var trapDef = catalog.Traps["trap_snare"];
            sysA.SetTrap("site_rabbit", "bait_grain_lure", "hunter_rabbit",
                trapDef.trapType, trapDef.trap_id, 1, 10);

            // Force rabbit catch
            sysA.State.trapSites[0].hasCatch = true;
            sysA.State.trapSites[0].catchSpecies = "rabbit";
            sysA.State.trapSites[0].diseaseId = string.Empty; // Rabbit (0.1 risk) resolves to no disease
            sysA.State.trapSites[0].contaminationDose = 0f;

            var (recordA, _) = ExecuteButchery(sysA, "site_rabbit", "dweller_butcher", 2);
            Assert.Equal("rabbit", recordA.SpeciesId);
            Assert.False(recordA.DiseaseHit);
            Assert.Equal(string.Empty, recordA.DiseaseId);

            // Save and restore
            var serializer = new SystemTextJsonSerializer();
            string json = serializer.Serialize(sysA.CaptureState());
            var restored = serializer.Deserialize<WildlifeTrappingState>(json);
            Assert.NotNull(restored);

            var sysC = new WildlifeTrappingSystem(new SeededRng(seed));
            catalog.RegisterWith(sysC);
            sysC.RestoreState(restored!);

            var restoredSite = sysC.State.trapSites[0];
            Assert.Equal("rabbit", restoredSite.catchSpecies);
            Assert.Equal(string.Empty, restoredSite.diseaseId);
            Assert.Equal(0f, restoredSite.contaminationDose);
            Assert.True(restoredSite.isMeatProcessed);
        }

        [Fact]
        public void ButcheryReplay_FinalHealthStateHash_MatchesAfterRestore()
        {
            var catalog = LoadCatalog();
            const int seed = 42;

            var sysA = new WildlifeTrappingSystem(new SeededRng(seed));
            catalog.RegisterWith(sysA);
            var trapDef = catalog.Traps["trap_snare"];
            sysA.SetTrap("site_high_risk", "bait_scrap_meat", "hunter_a",
                trapDef.trapType, trapDef.trap_id, 1, 10);

            // Set up high-risk catch (rat) with authored disease and contamination dose
            var ratPrey = catalog.Prey["rat"];
            sysA.State.trapSites[0].hasCatch = true;
            sysA.State.trapSites[0].catchSpecies = "rat";
            sysA.State.trapSites[0].diseaseId = ratPrey.diseaseId; // disease_typhoid_waterborne
            sysA.State.trapSites[0].contaminationDose = ratPrey.contaminationDose; // 4.0

            var (recordA, _) = ExecuteButchery(sysA, "site_high_risk", "dweller_cook", 5);

            // Save, deserialize, restore into sysB
            var serializer = new SystemTextJsonSerializer();
            string json = serializer.Serialize(sysA.CaptureState());
            var restored = serializer.Deserialize<WildlifeTrappingState>(json);

            var sysB = new WildlifeTrappingSystem(new SeededRng(seed));
            catalog.RegisterWith(sysB);
            sysB.RestoreState(restored!);

            var restoredSite = sysB.State.trapSites[0];
            int hashB = HashCode.Combine(
                "dweller_cook",
                !string.IsNullOrEmpty(restoredSite.diseaseId) ? restoredSite.diseaseId : "none",
                (int)(restoredSite.contaminationDose * 100f),
                5);

            Assert.Equal(recordA.HealthHash, hashB);
            Assert.Equal("disease_typhoid_waterborne", restoredSite.diseaseId);
            Assert.Equal(4.0f, restoredSite.contaminationDose);
        }

        [Fact]
        public void DeterministicReplay_MultiDayCampaignTrace_UninterruptedVsSavedRestored_MatchesExactEventTraceAndHash()
        {
            const int seed = 42;
            var catalog = LoadCatalog();

            // Run A: Uninterrupted run across 10 days
            var rngA = new SeededRng(seed);
            var sysA = new WildlifeTrappingSystem(rngA);
            catalog.RegisterWith(sysA);

            var trapDefWire = catalog.Traps["trap_improvised_wire"];
            var trapDefBox = catalog.Traps["trap_box"];

            sysA.SetTrap("site_wire", "bait_scrap_meat", "hunter_dweller",
                trapDefWire.trapType, trapDefWire.trap_id, trapDefWire.checkIntervalDays, trapDefWire.durabilityChecks);
            sysA.SetTrap("site_box", "bait_grain_lure", "hunter_dweller",
                trapDefBox.trapType, trapDefBox.trap_id, trapDefBox.checkIntervalDays, trapDefBox.durabilityChecks);

            var traceLogA = new StringBuilder();

            for (int day = 2; day <= 10; day++)
            {
                sysA.TickDay(day);
                foreach (var site in sysA.State.trapSites)
                {
                    traceLogA.AppendLine($"D:{day}|S:{site.siteId}|Dur:{site.remainingDurability}|Brk:{site.isBroken}|Catch:{site.hasCatch}|Sp:{site.catchSpecies}|Bycatch:{site.bycatchSpecies}|Yield:{site.carcassYield:F3}|Tox:{site.isToxic}|Dis:{site.diseaseId}|Dose:{site.contaminationDose:F2}");
                    if (site.hasCatch)
                    {
                        var (rec, _) = ExecuteButchery(sysA, site.siteId, "dweller_butcher", day);
                        traceLogA.AppendLine($"Butchered|S:{site.siteId}|Rec:{rec.SpeciesId}|HHash:{rec.HealthHash}");
                        site.hasCatch = false;
                    }
                }
            }

            string traceA = traceLogA.ToString();
            byte[] hashBytesA = SHA256.HashData(Encoding.UTF8.GetBytes(traceA));
            string hashA = Convert.ToHexString(hashBytesA);

            // Run B: Interrupted at Day 5 boundary, saved, deserialized, and resumed with continuing RNG
            var rngB = new SeededRng(seed);
            var sysB = new WildlifeTrappingSystem(rngB);
            catalog.RegisterWith(sysB);

            sysB.SetTrap("site_wire", "bait_scrap_meat", "hunter_dweller",
                trapDefWire.trapType, trapDefWire.trap_id, trapDefWire.checkIntervalDays, trapDefWire.durabilityChecks);
            sysB.SetTrap("site_box", "bait_grain_lure", "hunter_dweller",
                trapDefBox.trapType, trapDefBox.trap_id, trapDefBox.checkIntervalDays, trapDefBox.durabilityChecks);

            var traceLogB = new StringBuilder();

            // Run days 2..5
            for (int day = 2; day <= 5; day++)
            {
                sysB.TickDay(day);
                foreach (var site in sysB.State.trapSites)
                {
                    traceLogB.AppendLine($"D:{day}|S:{site.siteId}|Dur:{site.remainingDurability}|Brk:{site.isBroken}|Catch:{site.hasCatch}|Sp:{site.catchSpecies}|Bycatch:{site.bycatchSpecies}|Yield:{site.carcassYield:F3}|Tox:{site.isToxic}|Dis:{site.diseaseId}|Dose:{site.contaminationDose:F2}");
                    if (site.hasCatch)
                    {
                        var (rec, _) = ExecuteButchery(sysB, site.siteId, "dweller_butcher", day);
                        traceLogB.AppendLine($"Butchered|S:{site.siteId}|Rec:{rec.SpeciesId}|HHash:{rec.HealthHash}");
                        site.hasCatch = false;
                    }
                }
            }

            // Save at Day 5 boundary
            var serializer = new SystemTextJsonSerializer();
            string savedJson = serializer.Serialize(sysB.CaptureState());
            var restoredState = serializer.Deserialize<WildlifeTrappingState>(savedJson);
            Assert.NotNull(restoredState);

            // Resume in fresh system instance with continuing RNG
            var sysC = new WildlifeTrappingSystem(rngB);
            catalog.RegisterWith(sysC);
            sysC.RestoreState(restoredState!);

            // Run days 6..10
            for (int day = 6; day <= 10; day++)
            {
                sysC.TickDay(day);
                foreach (var site in sysC.State.trapSites)
                {
                    traceLogB.AppendLine($"D:{day}|S:{site.siteId}|Dur:{site.remainingDurability}|Brk:{site.isBroken}|Catch:{site.hasCatch}|Sp:{site.catchSpecies}|Bycatch:{site.bycatchSpecies}|Yield:{site.carcassYield:F3}|Tox:{site.isToxic}|Dis:{site.diseaseId}|Dose:{site.contaminationDose:F2}");
                    if (site.hasCatch)
                    {
                        var (rec, _) = ExecuteButchery(sysC, site.siteId, "dweller_butcher", day);
                        traceLogB.AppendLine($"Butchered|S:{site.siteId}|Rec:{rec.SpeciesId}|HHash:{rec.HealthHash}");
                        site.hasCatch = false;
                    }
                }
            }

            string traceB = traceLogB.ToString();
            byte[] hashBytesB = SHA256.HashData(Encoding.UTF8.GetBytes(traceB));
            string hashB = Convert.ToHexString(hashBytesB);

            Assert.Equal(traceA, traceB);
            Assert.Equal(hashA, hashB);
        }

        [Fact]
        public void DeterministicReplay_ThreeConsecutiveRuns_ProduceIdenticalStateAndEventHash()
        {
            const int seed = 42;
            var catalog = LoadCatalog();

            string GenerateRunHash()
            {
                var rng = new SeededRng(seed);
                var sys = new WildlifeTrappingSystem(rng);
                catalog.RegisterWith(sys);

                var trapDefWire = catalog.Traps["trap_improvised_wire"];
                var trapDefBox = catalog.Traps["trap_box"];

                sys.SetTrap("site_wire", "bait_scrap_meat", "hunter_dweller",
                    trapDefWire.trapType, trapDefWire.trap_id, trapDefWire.checkIntervalDays, trapDefWire.durabilityChecks);
                sys.SetTrap("site_box", "bait_grain_lure", "hunter_dweller",
                    trapDefBox.trapType, trapDefBox.trap_id, trapDefBox.checkIntervalDays, trapDefBox.durabilityChecks);

                var log = new StringBuilder();
                for (int day = 2; day <= 12; day++)
                {
                    sys.TickDay(day);
                    foreach (var site in sys.State.trapSites)
                    {
                        log.AppendLine($"D:{day}|S:{site.siteId}|Dur:{site.remainingDurability}|Brk:{site.isBroken}|Catch:{site.hasCatch}|Sp:{site.catchSpecies}|Bycatch:{site.bycatchSpecies}|Yield:{site.carcassYield:F3}|Tox:{site.isToxic}|Dis:{site.diseaseId}|Dose:{site.contaminationDose:F2}");
                        if (site.hasCatch)
                        {
                            var (rec, _) = ExecuteButchery(sys, site.siteId, "dweller_butcher", day);
                            log.AppendLine($"Butchered|S:{site.siteId}|Rec:{rec.SpeciesId}|HHash:{rec.HealthHash}");
                            site.hasCatch = false;
                        }
                    }
                }

                log.AppendLine($"TotalCatch:{sys.State.totalCatch}|TotalToxicRemoved:{sys.State.totalToxicRemoved}");
                return Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(log.ToString())));
            }

            string hash1 = GenerateRunHash();
            string hash2 = GenerateRunHash();
            string hash3 = GenerateRunHash();

            Assert.Equal(hash1, hash2);
            Assert.Equal(hash2, hash3);
        }

        [Fact]
        public void DeterministicReplay_BreakageBoundary_MatchesBreakDayAndPreventsSubsequentCatches()
        {
            const int seed = 99;
            var catalog = LoadCatalog();

            var rng = new SeededRng(seed);
            var sys = new WildlifeTrappingSystem(rng);
            catalog.RegisterWith(sys);

            var trapDef = catalog.Traps["trap_improvised_wire"]; // durability = 3, interval = 1
            sys.SetTrap("site_test_break", "bait_grain_lure", "hunter_dweller",
                trapDef.trapType, trapDef.trap_id, checkIntervalDays: 1, durabilityChecks: 3);

            int breakDay = -1;
            for (int day = 2; day <= 8; day++)
            {
                sys.TickDay(day);
                var site = sys.State.trapSites[0];
                if (site.hasCatch)
                {
                    ExecuteButchery(sys, "site_test_break", "butcher", day);
                }

                if (site.isBroken && breakDay < 0)
                {
                    breakDay = day;
                }
            }

            Assert.True(breakDay > 0, "Trap with 3 durability checks must break within 8 days");

            // Save at broken state
            var serializer = new SystemTextJsonSerializer();
            string json = serializer.Serialize(sys.CaptureState());
            var restored = serializer.Deserialize<WildlifeTrappingState>(json);
            Assert.NotNull(restored);

            var sysRestored = new WildlifeTrappingSystem(rng);
            catalog.RegisterWith(sysRestored);
            sysRestored.RestoreState(restored!);

            // Broken trap must produce 0 catches on subsequent days
            int catchesAfterBreak = 0;
            for (int day = 9; day <= 15; day++)
            {
                sysRestored.TickDay(day);
                if (sysRestored.State.trapSites[0].hasCatch)
                {
                    catchesAfterBreak++;
                }
            }

            Assert.Equal(0, catchesAfterBreak);
            Assert.True(sysRestored.State.trapSites[0].isBroken);
            Assert.Equal(0, sysRestored.State.trapSites[0].remainingDurability);
        }
    }
}
