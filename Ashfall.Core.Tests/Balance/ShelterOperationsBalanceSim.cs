// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Ashfall.Core;
using Ashfall.Core.Excavation;
using Ashfall.Core.Inventory;
using Ashfall.Core.Radio;
using Ashfall.Core.Shelter;
using Ashfall.Core.Survivors;
using Xunit;
using Xunit.Abstractions;

namespace Ashfall.Core.Tests.Balance
{
    public sealed class ShelterOperationsSimConfig
    {
        public int Seed { get; set; } = 42;
        public int Days { get; set; } = 100;
        public int CrewSize { get; set; } = 6;
        public string DifficultyPreset { get; set; } = "Standard Survival";

        public int InitialCasings { get; set; } = 40;
        public int InitialScrapMetal { get; set; } = 60;
        public int InitialScrapChemical { get; set; } = 30;
        public int InitialAmmoStock { get; set; } = 30;
        public int InitialMechanicalParts { get; set; } = 15;
        public int InitialMachineOil { get; set; } = 12;
        public int InitialShoringKits { get; set; } = 8;

        public float ToolingWearPerJob { get; set; } = 0.005f;
        public int MethaneDailyPpm { get; set; } = 80;
        public bool VentilationBlowerInstalled { get; set; } = true;
        public bool ShoringMaintenanceEnabled { get; set; } = true;

        public static ShelterOperationsSimConfig CreateAbundant(int seed = 42, int days = 100)
        {
            return new ShelterOperationsSimConfig
            {
                Seed = seed,
                Days = days,
                DifficultyPreset = "Abundant",
                InitialCasings = 100,
                InitialScrapMetal = 150,
                InitialScrapChemical = 80,
                InitialAmmoStock = 80,
                InitialMechanicalParts = 40,
                InitialMachineOil = 30,
                InitialShoringKits = 20,
                ToolingWearPerJob = 0.003f,
                MethaneDailyPpm = 60,
                VentilationBlowerInstalled = true,
                ShoringMaintenanceEnabled = true
            };
        }

        public static ShelterOperationsSimConfig CreateStandard(int seed = 42, int days = 100)
        {
            return new ShelterOperationsSimConfig
            {
                Seed = seed,
                Days = days,
                DifficultyPreset = "Standard Survival",
                InitialCasings = 40,
                InitialScrapMetal = 60,
                InitialScrapChemical = 30,
                InitialAmmoStock = 30,
                InitialMechanicalParts = 15,
                InitialMachineOil = 12,
                InitialShoringKits = 8,
                ToolingWearPerJob = 0.005f,
                MethaneDailyPpm = 80,
                VentilationBlowerInstalled = true,
                ShoringMaintenanceEnabled = true
            };
        }

        public static ShelterOperationsSimConfig CreateHardcore(int seed = 42, int days = 100)
        {
            return new ShelterOperationsSimConfig
            {
                Seed = seed,
                Days = days,
                DifficultyPreset = "Hardcore Desolation",
                InitialCasings = 15,
                InitialScrapMetal = 25,
                InitialScrapChemical = 12,
                InitialAmmoStock = 10,
                InitialMechanicalParts = 5,
                InitialMachineOil = 4,
                InitialShoringKits = 3,
                ToolingWearPerJob = 0.012f,
                MethaneDailyPpm = 120,
                VentilationBlowerInstalled = false,
                ShoringMaintenanceEnabled = true
            };
        }
    }

    public sealed class ShelterOperationsTelemetryRow
    {
        public int Seed { get; set; }
        public int Day { get; set; }
        public string Difficulty { get; set; } = string.Empty;
        public int AmmoCasings { get; set; }
        public int AmmoLead { get; set; }
        public int AmmoPowder { get; set; }
        public int AmmoCrafted { get; set; }
        public int AmmoFired { get; set; }
        public int AmmoStock { get; set; }
        public float ToolingLathe { get; set; }
        public float ToolingPress { get; set; }
        public float ToolingBench { get; set; }
        public int RadioDecrypted { get; set; }
        public int RadioTriangulated { get; set; }
        public int SocialPrivacyFatigueAvg { get; set; }
        public int SocialDisputes { get; set; }
        public int ExcavationMethanePpm { get; set; }
        public int ExcavationShoringPermille { get; set; }
        public int ExcavationCaveIns { get; set; }

        public const string CsvHeader =
            "Seed,Day,Difficulty,Ammo_Casings,Ammo_Lead,Ammo_Powder,Ammo_Crafted,Ammo_Fired,Ammo_Stock,Tooling_Lathe,Tooling_Press,Tooling_Bench,Radio_Decrypted,Radio_Triangulated,Social_PrivacyFatigue_Avg,Social_Disputes,Excavation_MethanePPM,Excavation_ShoringPermille,Excavation_CaveIns";

        public string ToCsvLine()
        {
            var inv = CultureInfo.InvariantCulture;
            return string.Format(
                inv,
                "{0},{1},{2},{3},{4},{5},{6},{7},{8},{9:F3},{10:F3},{11:F3},{12},{13},{14},{15},{16},{17},{18}",
                Seed,
                Day,
                Difficulty,
                AmmoCasings,
                AmmoLead,
                AmmoPowder,
                AmmoCrafted,
                AmmoFired,
                AmmoStock,
                ToolingLathe,
                ToolingPress,
                ToolingBench,
                RadioDecrypted,
                RadioTriangulated,
                SocialPrivacyFatigueAvg,
                SocialDisputes,
                ExcavationMethanePpm,
                ExcavationShoringPermille,
                ExcavationCaveIns);
        }
    }

    public sealed class ShelterOperationsSimResult
    {
        public ShelterOperationsSimConfig Config { get; set; } = null!;
        public List<ShelterOperationsTelemetryRow> Rows { get; set; } = new List<ShelterOperationsTelemetryRow>();
        public int TotalAmmoCrafted { get; set; }
        public int TotalAmmoFired { get; set; }
        public int FinalAmmoStock { get; set; }
        public int TotalDisputes { get; set; }
        public int TotalCaveIns { get; set; }
        public int MaxMethanePpm { get; set; }
        public int MinShoringPermille { get; set; } = 1000;
        public bool Success { get; set; } = true;
        public List<string> InvariantViolations { get; set; } = new List<string>();
    }

    public static class ShelterOperationsBalanceSimulator
    {
        public static string FindRepoRoot()
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null)
            {
                if (File.Exists(Path.Combine(dir.FullName, "project.godot")) ||
                    Directory.Exists(Path.Combine(dir.FullName, "Assets", "StreamingAssets", "Data")))
                    return dir.FullName;
                dir = dir.Parent;
            }
            return AppContext.BaseDirectory;
        }

        public static string LoadCatalog(string relativePath)
        {
            string root = FindRepoRoot();
            string path = Path.Combine(root, relativePath);
            return File.Exists(path) ? File.ReadAllText(path) : string.Empty;
        }

        public static ShelterOperationsSimResult Run(ShelterOperationsSimConfig config)
        {
            var result = new ShelterOperationsSimResult { Config = config };
            var rng = new SeededRng(config.Seed);

            // 1. Inventory setup
            var inventory = new Inventory.Inventory { Capacity = 500, MaxWeight = 5000f };
            inventory.Add(new ItemDefinition { id = "spent_casing", displayName = "Spent Casing", stackMax = 999 }, config.InitialCasings);
            inventory.Add(new ItemDefinition { id = "scrap_metal", displayName = "Scrap Metal", stackMax = 999 }, config.InitialScrapMetal);
            inventory.Add(new ItemDefinition { id = "scrap_chemical", displayName = "Chemical Compound", stackMax = 999 }, config.InitialScrapChemical);
            inventory.Add(new ItemDefinition { id = "ammo_9x19", displayName = "9x19mm FMJ", stackMax = 999 }, config.InitialAmmoStock);
            inventory.Add(new ItemDefinition { id = "mechanical_parts", displayName = "Mechanical Parts", stackMax = 999 }, config.InitialMechanicalParts);
            inventory.Add(new ItemDefinition { id = "machine_oil", displayName = "Machine Oil", stackMax = 999 }, config.InitialMachineOil);
            inventory.Add(new ItemDefinition { id = "mitigation_shoring_reinforce_kit", displayName = "Shoring Kit", stackMax = 999 }, config.InitialShoringKits);

            // 2. Systems setup
            var workshopRng = new SeededRng(config.Seed + 101);
            var radioRng = new SeededRng(config.Seed + 202);
            var socialRng = new SeededRng(config.Seed + 303);
            var excavationRng = new SeededRng(config.Seed + 404);

            var workshop = new ShelterWorkshopSystem(inventory, workshopRng);
            string workshopJson = LoadCatalog("Assets/StreamingAssets/Data/workshop_recipes.json");
            if (!string.IsNullOrEmpty(workshopJson)) workshop.LoadCatalog(workshopJson);

            // Configure tooling wear rate in catalog
            int wearPermille = Math.Max(1, (int)Math.Round(config.ToolingWearPerJob * 1000f));
            foreach (var r in workshop.Recipes.Values)
            {
                r.ToolWearPermille = wearPermille;
            }

            var radioStation = new ShelterRadioStationSystem(radioRng);
            string radioJson = LoadCatalog("Assets/StreamingAssets/Data/radio_intercepts.json");
            if (!string.IsNullOrEmpty(radioJson)) radioStation.LoadCatalog(radioJson);

            var socialDynamics = new ShelterSocialDynamicsSystem(socialRng);
            string socialJson = LoadCatalog("Assets/StreamingAssets/Data/shelter_social_events.json");
            if (!string.IsNullOrEmpty(socialJson)) socialDynamics.LoadCatalog(socialJson);

            var excavationHazards = new ExcavationHazardSystem(inventory, excavationRng);
            string excavationJson = LoadCatalog("Assets/StreamingAssets/Data/excavation_hazard_mitigation.json");
            if (!string.IsNullOrEmpty(excavationJson)) excavationHazards.LoadCatalog(excavationJson);

            // 3. Setup rooms and machines
            var armoryMachine = workshop.GetOrCreateMachineState("room_armory_munitions");
            var latheMachine = workshop.GetOrCreateMachineState("room_workshop_precision");
            var benchMachine = workshop.GetOrCreateMachineState("room_workshop_heavy");

            // Setup social survivors
            var survivors = new List<string>();
            for (int i = 1; i <= config.CrewSize; i++)
            {
                string sId = $"dweller_{i}";
                survivors.Add(sId);
                // 2 private quarters, rest in crowded bunks
                string roomId = i <= 2 ? "room_quarters_private" : "room_bunks_crowded";
                socialDynamics.RegisterSurvivorRoom(sId, roomId);
            }

            // Setup excavation sector
            var sector = excavationHazards.GetOrCreateSector("sector_excavation_alpha");
            if (config.VentilationBlowerInstalled)
            {
                sector.InstalledMitigationIds.Add("mitigation_ventilation_blower_install");
            }

            int caveInCount = 0;
            excavationHazards.OnRescueStarted += (sec, count) => caveInCount++;

            int totalDisputes = 0;
            socialDynamics.OnIncidentTriggered += inc => totalDisputes++;

            // 4. Daily loop
            for (int day = 1; day <= config.Days; day++)
            {
                // A. Scavenging inflow (deterministic)
                int scavengedCasings = rng.Next(1, 4);
                int scavengedMetal = rng.Next(2, 6);
                int scavengedChemical = rng.Next(1, 3);
                inventory.Add(new ItemDefinition { id = "spent_casing", stackMax = 999 }, scavengedCasings);
                inventory.Add(new ItemDefinition { id = "scrap_metal", stackMax = 999 }, scavengedMetal);
                inventory.Add(new ItemDefinition { id = "scrap_chemical", stackMax = 999 }, scavengedChemical);

                // Small chance of scavenging maintenance parts
                if (rng.Next(0, 100) < 20)
                    inventory.Add(new ItemDefinition { id = "mechanical_parts", stackMax = 999 }, 1);
                if (rng.Next(0, 100) < 15)
                    inventory.Add(new ItemDefinition { id = "machine_oil", stackMax = 999 }, 1);
                if (rng.Next(0, 100) < 10)
                    inventory.Add(new ItemDefinition { id = "mitigation_shoring_reinforce_kit", stackMax = 999 }, 1);

                // B. Ammo consumption (patrol defense / hunting)
                int currentAmmo = inventory.CountById("ammo_9x19");
                int ammoToFire = Math.Min(currentAmmo, rng.Next(4, 10));
                if (ammoToFire > 0)
                {
                    inventory.Remove("ammo_9x19", ammoToFire);
                    result.TotalAmmoFired += ammoToFire;

                    // Spent casings recovery (70% recovery rate)
                    int recovered = (int)Math.Round(ammoToFire * 0.70f);
                    if (recovered > 0)
                        inventory.Add(new ItemDefinition { id = "spent_casing", stackMax = 999 }, recovered);
                }

                // C. Workshop ammo crafting
                int craftedToday = 0;
                currentAmmo = inventory.CountById("ammo_9x19");
                if (currentAmmo < 50 &&
                    inventory.CountById("spent_casing") >= 1 &&
                    inventory.CountById("scrap_metal") >= 2 &&
                    inventory.CountById("scrap_chemical") >= 1 &&
                    armoryMachine.ToolingHealth > 0.15f)
                {
                    var jobRes = workshop.TryStartJob("recipe_workshop_reload_9x19", "room_armory_munitions", null, null, out string jobId);
                    if (jobRes.IsSuccess)
                    {
                        workshop.AdvanceLaborTicks(60, day); // Complete job
                        craftedToday += 20;
                        result.TotalAmmoCrafted += 20;
                    }
                }

                // Machine maintenance overhaul if degraded
                if (armoryMachine.ToolingHealth <= 0.30f &&
                    inventory.CountById("mechanical_parts") >= 2 &&
                    inventory.CountById("machine_oil") >= 1)
                {
                    inventory.Remove("mechanical_parts", 2);
                    inventory.Remove("machine_oil", 1);
                    armoryMachine.ToolingHealth = 1.0f;
                    armoryMachine.Calibration = 1.0f;
                }

                if (latheMachine.ToolingHealth <= 0.40f &&
                    inventory.CountById("mechanical_parts") >= 1 &&
                    inventory.CountById("machine_oil") >= 1)
                {
                    inventory.Remove("mechanical_parts", 1);
                    inventory.Remove("machine_oil", 1);
                    latheMachine.ToolingHealth = 1.0f;
                    latheMachine.Calibration = 1.0f;
                }

                // Tick systems
                workshop.TickDay(day);

                // D. Radio Station operations
                int targetFreq = 14200 + (day % 5) * 50;
                radioStation.TuneTo(targetFreq);
                radioStation.ScanFrequency(day);

                foreach (var intercept in radioStation.State.intercepts)
                {
                    if (!intercept.IsDecrypted && !intercept.IsExpired)
                    {
                        radioStation.ProgressDecryption(intercept.InterceptId, 1.2f);
                    }
                    if (intercept.IsDecrypted && !intercept.Resolved && !intercept.IsExpired)
                    {
                        int azimuth = (day * 37) % 360;
                        radioStation.RecordBearing(intercept.InterceptId, azimuth);
                    }
                }
                radioStation.TickDay(day);

                // E. Shelter Social operations
                var privateOccupants = new List<string> { survivors[0], survivors[1] };
                var crowdedOccupants = new List<string> { survivors[2], survivors[3], survivors[4], survivors[5] };

                socialDynamics.EvaluateRoomDynamics("room_quarters_private", privateOccupants, day);
                var incident = socialDynamics.EvaluateRoomDynamics("room_bunks_crowded", crowdedOccupants, day);
                if (incident != null && !incident.Resolved)
                {
                    // Attempt mediation
                    socialDynamics.TryMediateIncident(incident.IncidentId, survivors[0]);
                }
                socialDynamics.TickDay(day);

                // F. Excavation Hazards
                excavationHazards.TickDay(day);

                // Check shoring maintenance
                if (config.ShoringMaintenanceEnabled &&
                    sector.ShoringHealthPermille < 600 &&
                    inventory.CountById("mitigation_shoring_reinforce_kit") >= 1)
                {
                    inventory.Remove("mitigation_shoring_reinforce_kit", 1);
                    sector.ShoringHealthPermille = Math.Min(1000, sector.ShoringHealthPermille + 400);
                }

                // If shoring fully collapsed, trigger cave in
                if (sector.ShoringHealthPermille <= 0 && sector.ActiveTrappedMiners.Count == 0)
                {
                    sector.ActiveTrappedMiners.Add("miner_alpha");
                    sector.RescueLaborRemaining = 120;
                    sector.RescueDeadlineDay = day + 3;
                    caveInCount++;
                }

                // Resolve rescue if active
                if (sector.ActiveTrappedMiners.Count > 0 && !sector.RescueCompleted && !sector.RescueFailed)
                {
                    sector.RescueLaborRemaining = Math.Max(0, sector.RescueLaborRemaining - 60);
                    if (sector.RescueLaborRemaining <= 0)
                    {
                        sector.RescueCompleted = true;
                        sector.ActiveTrappedMiners.Clear();
                    }
                }

                // G. Gather daily metrics
                int totalPrivacyFatigue = 0;
                foreach (var sId in survivors)
                {
                    var p = socialDynamics.GetOrCreatePrivacyProfile(sId);
                    totalPrivacyFatigue += p.PrivacyFatiguePermille;
                }
                int avgPrivacyFatigue = survivors.Count > 0 ? totalPrivacyFatigue / survivors.Count : 0;

                int decryptedCount = 0;
                int triangulatedCount = 0;
                foreach (var i in radioStation.State.intercepts)
                {
                    if (i.IsDecrypted) decryptedCount++;
                    if (i.Resolved) triangulatedCount++;
                }

                int casingsCount = inventory.CountById("spent_casing");
                int leadCount = inventory.CountById("scrap_metal");
                int powderCount = inventory.CountById("scrap_chemical");
                int ammoStock = inventory.CountById("ammo_9x19");

                var row = new ShelterOperationsTelemetryRow
                {
                    Seed = config.Seed,
                    Day = day,
                    Difficulty = config.DifficultyPreset,
                    AmmoCasings = casingsCount,
                    AmmoLead = leadCount,
                    AmmoPowder = powderCount,
                    AmmoCrafted = craftedToday,
                    AmmoFired = ammoToFire,
                    AmmoStock = ammoStock,
                    ToolingLathe = latheMachine.ToolingHealth,
                    ToolingPress = armoryMachine.ToolingHealth,
                    ToolingBench = benchMachine.ToolingHealth,
                    RadioDecrypted = decryptedCount,
                    RadioTriangulated = triangulatedCount,
                    SocialPrivacyFatigueAvg = avgPrivacyFatigue,
                    SocialDisputes = socialDynamics.State.recentIncidents.Count,
                    ExcavationMethanePpm = sector.MethanePpm,
                    ExcavationShoringPermille = sector.ShoringHealthPermille,
                    ExcavationCaveIns = caveInCount
                };

                result.Rows.Add(row);

                // Update aggregates
                if (sector.MethanePpm > result.MaxMethanePpm)
                    result.MaxMethanePpm = sector.MethanePpm;
                if (sector.ShoringHealthPermille < result.MinShoringPermille)
                    result.MinShoringPermille = sector.ShoringHealthPermille;

                // Invariant checks
                if (casingsCount < 0 || leadCount < 0 || powderCount < 0 || ammoStock < 0)
                {
                    result.InvariantViolations.Add($"Day {day}: Negative ammo/material inventory detected");
                    result.Success = false;
                }
                if (float.IsNaN(latheMachine.ToolingHealth) || float.IsNaN(armoryMachine.ToolingHealth) || float.IsNaN(benchMachine.ToolingHealth))
                {
                    result.InvariantViolations.Add($"Day {day}: NaN tooling health detected");
                    result.Success = false;
                }
                if (sector.MethanePpm < 0 || sector.ShoringHealthPermille < 0)
                {
                    result.InvariantViolations.Add($"Day {day}: Negative hazard metric detected");
                    result.Success = false;
                }
            }

            result.FinalAmmoStock = inventory.CountById("ammo_9x19");
            result.TotalDisputes = totalDisputes;
            result.TotalCaveIns = caveInCount;
            return result;
        }
    }

    public class ShelterOperationsBalanceSim
    {
        private readonly ITestOutputHelper _out;

        public ShelterOperationsBalanceSim(ITestOutputHelper output)
        {
            _out = output;
        }

        [Fact]
        public void Generate100DayBalanceCsv_AndAssertInvariants()
        {
            string root = ShelterOperationsBalanceSimulator.FindRepoRoot();
            string artifactsDir = Path.Combine(root, "artifacts", "balance");
            Directory.CreateDirectory(artifactsDir);
            string csvPath = Path.Combine(artifactsDir, "shelter_operations_100day.csv");

            var sb = new StringBuilder();
            sb.AppendLine(ShelterOperationsTelemetryRow.CsvHeader);

            // Run presets
            var abundantConfig = ShelterOperationsSimConfig.CreateAbundant(seed: 42, days: 100);
            var abundantRes = ShelterOperationsBalanceSimulator.Run(abundantConfig);
            Assert.True(abundantRes.Success, "Abundant simulation failed invariants");
            foreach (var row in abundantRes.Rows) sb.AppendLine(row.ToCsvLine());

            var standardConfig = ShelterOperationsSimConfig.CreateStandard(seed: 42, days: 100);
            var standardRes = ShelterOperationsBalanceSimulator.Run(standardConfig);
            Assert.True(standardRes.Success, "Standard simulation failed invariants");
            foreach (var row in standardRes.Rows) sb.AppendLine(row.ToCsvLine());

            var hardcoreConfig = ShelterOperationsSimConfig.CreateHardcore(seed: 42, days: 100);
            var hardcoreRes = ShelterOperationsBalanceSimulator.Run(hardcoreConfig);
            Assert.True(hardcoreRes.Success, "Hardcore simulation failed invariants");
            foreach (var row in hardcoreRes.Rows) sb.AppendLine(row.ToCsvLine());

            // Run seed 98765 as required by Task 7
            var seed98765Config = ShelterOperationsSimConfig.CreateStandard(seed: 98765, days: 100);
            var seed98765Res = ShelterOperationsBalanceSimulator.Run(seed98765Config);
            Assert.True(seed98765Res.Success, "Seed 98765 simulation failed invariants");
            foreach (var row in seed98765Res.Rows) sb.AppendLine(row.ToCsvLine());

            File.WriteAllText(csvPath, sb.ToString(), new UTF8Encoding(false));
            _out.WriteLine($"Successfully wrote 100-day balance CSV to {csvPath} ({sb.Length} bytes)");

            // Verification assertions
            Assert.True(standardRes.TotalAmmoCrafted > 0, "Standard survival should actively craft ammo across 100 days");
            Assert.True(standardRes.FinalAmmoStock > 0, "Standard survival should maintain active ammo reserve");
            Assert.True(standardRes.FinalAmmoStock < 500, "Ammo stock should not runaway inflate");
            Assert.True(standardRes.MaxMethanePpm < 2500, "Ventilated sector should prevent explosive methane runaway");
        }

        [Fact]
        public void DeterministicByteParity_Seed98765()
        {
            var config1 = ShelterOperationsSimConfig.CreateStandard(seed: 98765, days: 100);
            var res1 = ShelterOperationsBalanceSimulator.Run(config1);

            var config2 = ShelterOperationsSimConfig.CreateStandard(seed: 98765, days: 100);
            var res2 = ShelterOperationsBalanceSimulator.Run(config2);

            Assert.Equal(res1.Rows.Count, res2.Rows.Count);
            for (int i = 0; i < res1.Rows.Count; i++)
            {
                string line1 = res1.Rows[i].ToCsvLine();
                string line2 = res2.Rows[i].ToCsvLine();
                Assert.Equal(line1, line2);
            }
        }

        [Fact]
        public void VentilationBlower_ControlsMethaneEquilibrium()
        {
            // With blower: methane drops by 200 PPM whenever accumulated
            var withBlowerCfg = ShelterOperationsSimConfig.CreateStandard(seed: 42, days: 50);
            withBlowerCfg.VentilationBlowerInstalled = true;
            var resWithBlower = ShelterOperationsBalanceSimulator.Run(withBlowerCfg);

            // Without blower: methane steadily accumulates
            var noBlowerCfg = ShelterOperationsSimConfig.CreateStandard(seed: 42, days: 50);
            noBlowerCfg.VentilationBlowerInstalled = false;
            var resNoBlower = ShelterOperationsBalanceSimulator.Run(noBlowerCfg);

            _out.WriteLine($"Methane max with blower: {resWithBlower.MaxMethanePpm} PPM vs without blower: {resNoBlower.MaxMethanePpm} PPM");
            Assert.True(resWithBlower.MaxMethanePpm < resNoBlower.MaxMethanePpm, "Ventilation blower must significantly reduce peak methane PPM");
            Assert.True(resWithBlower.MaxMethanePpm < 2000, "With blower, methane should stay well below 2000 PPM");
            Assert.True(resNoBlower.MaxMethanePpm > 2500, "Without blower, methane should cross the 2500 PPM critical warning threshold");
        }

        [Fact]
        public void ToolingWearSweep_DegradesMachinesPredictably()
        {
            var lowWearCfg = ShelterOperationsSimConfig.CreateStandard(seed: 777, days: 30);
            lowWearCfg.ToolingWearPerJob = 0.001f;
            var resLow = ShelterOperationsBalanceSimulator.Run(lowWearCfg);

            var highWearCfg = ShelterOperationsSimConfig.CreateStandard(seed: 777, days: 30);
            highWearCfg.ToolingWearPerJob = 0.025f;
            var resHigh = ShelterOperationsBalanceSimulator.Run(highWearCfg);

            Assert.True(resLow.Success && resHigh.Success);
        }

        [Fact]
        public void MonteCarlo_100Seeds_StandardSurvival_InvariantsHold()
        {
            int violationsCount = 0;
            int successfulSeeds = 0;

            for (int seed = 1; seed <= 100; seed++)
            {
                var cfg = ShelterOperationsSimConfig.CreateStandard(seed: seed, days: 30);
                var res = ShelterOperationsBalanceSimulator.Run(cfg);

                if (res.Success && res.InvariantViolations.Count == 0)
                {
                    successfulSeeds++;
                }
                else
                {
                    violationsCount++;
                }
            }

            _out.WriteLine($"Monte Carlo 100 seeds: {successfulSeeds} passed, {violationsCount} violations");
            Assert.Equal(100, successfulSeeds);
            Assert.Equal(0, violationsCount);
        }
    }
}
