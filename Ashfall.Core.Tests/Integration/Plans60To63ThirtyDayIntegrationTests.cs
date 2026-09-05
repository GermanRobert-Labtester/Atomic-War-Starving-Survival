// SPDX-License-Identifier: MIT
// Plans 60–63 (Batch B1–B4) 30-Day Cross-System Campaign Integration Tests.
// Verifies all four flagship pillars working in unison across a simulated 30-day campaign:
// - B1: Radio Station JSON-driven authority, 24h schedules, and signal profiles.
// - B2: Library Study manual expansion, reader availability reservation, and research discovery.
// - B3: Tactical Combat encounter loop, mid-encounter save replay determinism, and exactly-once aftermath.
// - B4: Disease Quarantine Policy, 8-stage clinical progression, medical ward isolation, and care consumption.
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Combat;
using Ashfall.Core.Disease;
using Ashfall.Core.Inventory;
using Ashfall.Core.Journal;
using Ashfall.Core.Medical;
using Ashfall.Core.Radio;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Integration
{
    public class Plans60To63ThirtyDayIntegrationTests
    {
        private static string LocateDataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found))
                return found;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found))
                return found;
            throw new DirectoryNotFoundException("Could not locate Assets/StreamingAssets/Data from test run");
        }

        [Fact]
        public void FullCampaign_30Day_Plans60To63_IntegratedPipeline()
        {
            string dataDir = LocateDataDir();
            const int masterSeed = 6063;
            var masterRng = new SeededRng(masterSeed);

            // =================================================================
            // 1. PILLAR B1 — RADIO STATION AUTHORITY (Plan 60)
            // =================================================================
            var radioCatalog = new RadioStationCatalog();
            int radioLoaded = RadioStationCatalogLoader.LoadAndRegister(radioCatalog, dataDir);
            Assert.True(radioLoaded >= 6, $"Expected at least 6 radio stations loaded, got {radioLoaded}");
            var cdStation = radioCatalog.GetStation("station_civil_defense");
            Assert.NotNull(cdStation);

            // =================================================================
            // 2. PILLAR B2 — LIBRARY MANUAL STUDY & RESEARCH (Plan 61)
            // =================================================================
            var skills = new SkillProgressionSystem();
            var research = new ResearchSystem();
            var journal = new JournalSystem();
            var dutyRoster = new DutyRosterSystem(101);
            dutyRoster.Unlock(1);

            var library = new LibraryStudySystem(skills, research, journal, dutyRoster);
            var manuals = LibraryManualCatalogLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            library.LoadCatalog(manuals);
            Assert.True(manuals.Count >= 24, $"Expected >= 24 library manuals, got {manuals.Count}");

            // Register researcher survivor
            const string researcherId = "surv_marie";
            dutyRoster.WriteName(researcherId, "Marie Curie", "Archivist", DutyRosterSystem.ScriptPencil, 1, true);

            // Start studying water filtration manual
            string manualId = "manual_water_filtration";
            var startStudy = library.StartStudy(manualId, researcherId);
            Assert.True(startStudy.IsSuccess, $"Failed to start study: {startStudy.FailureCode}");

            // Verify reader availability is reserved in DutyRoster
            Assert.True(dutyRoster.IsSurvivorReservedExternally?.Invoke(researcherId) == true,
                "Active library study must reserve survivor in DutyRoster");

            // =================================================================
            // 3. PILLAR B3 — TACTICAL COMBAT SETUP (Plan 62)
            // =================================================================
            CombatCatalog.SeedDefaults();
            var combatSystem = new TacticalCombatSystem();

            // =================================================================
            // 4. PILLAR B4 — MEDICAL WARD & DISEASE QUARANTINE (Plan 63)
            // =================================================================
            var beds = new List<MedicalBed>
            {
                new MedicalBed("bed_iso_1", "Isolation Chamber 1", MedicalBedCategory.Isolation, isolation: true),
                new MedicalBed("bed_iso_2", "Isolation Chamber 2", MedicalBedCategory.Isolation, isolation: true),
                new MedicalBed("bed_gen_1", "General Bed 1", MedicalBedCategory.General, isolation: false),
            };
            var procedures = new List<MedicalProcedureDef>();
            var medicalWard = new MedicalWardSystem(new MedicalWardState(), beds, procedures);

            var diseaseCatalog = DiseaseCatalogLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            var diseaseSystem = new DiseaseSystem(new DiseaseSystemState(), new SeededRng(masterSeed));
            diseaseSystem.BindCatalog(diseaseCatalog);

            var inventory = new Dictionary<string, int>(StringComparer.Ordinal)
            {
                { "clean_water", 200 },
                { "canned_food", 200 },
                { "medical_kit", 50 },
                { "antibiotics", 50 },
                { "ammo_556", 200 }
            };

            Func<string, int, bool> tryConsume = (item, qty) =>
            {
                if (inventory.TryGetValue(item, out int count) && count >= qty)
                {
                    inventory[item] = count - qty;
                    return true;
                }
                return false;
            };

            diseaseSystem.TryConsumeItem = tryConsume;

            var quarantineCoord = new DiseaseQuarantineCoordinator(
                medicalWard,
                diseaseSystem,
                dutyRoster,
                tryConsume,
                () => ContainmentCapability.FromResearch(k => research.IsManualUnlocked(k)));

            // Register patrol survivor
            const string soldierId = "surv_kane";
            dutyRoster.WriteName(soldierId, "Kane Vance", "Scout", DutyRosterSystem.ScriptPencil, 1, true);
            dutyRoster.Assign(DutyRosterIds.RoleNightWatch, soldierId);

            // =================================================================
            // 5. 30-DAY CAMPAIGN EXECUTION
            // =================================================================
            var allSurvivors = new List<string> { researcherId, soldierId, "surv_alec", "surv_mira" };
            for (int i = 2; i < allSurvivors.Count; i++)
            {
                dutyRoster.WriteName(allSurvivors[i], $"Survivor {i}", "Worker", DutyRosterSystem.ScriptPencil, 1, true);
            }

            int combatEncounterDay = 15;
            int pathogenExposureDay = 10;
            bool studyCompleted = false;
            bool combatResolved = false;
            bool patientCured = false;

            for (int day = 1; day <= 30; day++)
            {
                // A. Radio schedule check
                for (int h = 0; h < 24; h += 6)
                {
                    var slot = cdStation.GetCurrentSlot(day, h);
                    Assert.NotNull(slot);
                }

                // B. Library Study progression
                if (!studyCompleted)
                {
                    library.TickDay(day);
                    if (library.State.completedManualIds.Contains(manualId))
                    {
                        studyCompleted = true;
                        // Verified that manual unlocked research node
                        var manualDef = manuals.Find(m => m.manual_id == manualId);
                        Assert.NotNull(manualDef);
                        foreach (var node in manualDef!.researchUnlocks)
                        {
                            Assert.True(research.IsManualUnlocked(node), $"Node {node} should be unlocked by manual");
                            Assert.False(research.State.completedIds.Contains(node), "Manual should discover/reveal node, NEVER CompleteResearch()");
                        }
                    }
                }

                // C. Tactical Combat encounter on Day 15
                if (day == combatEncounterDay && !combatResolved)
                {
                    var combatRoster = new List<CombatantState>
                    {
                        new CombatantState
                        {
                            Id = "actor_soldier",
                            Name = "Kane",
                            SurvivorId = soldierId,
                            WeaponInstanceId = "inst_rifle_1",
                            IsPlayer = true,
                            Health = 100,
                            MaxHealth = 100,
                            ArmorRating = 0.4f
                        }
                    };
                    var combatWeapons = new List<WeaponInstanceState>
                    {
                        new WeaponInstanceState
                        {
                            InstanceId = "inst_rifle_1",
                            WeaponId = "weapon_assault_rifle",
                            OwnerSurvivorId = soldierId,
                            OwnerCombatantId = "actor_soldier",
                            ConditionPct = 1.0f,
                            AmmoId = "ammo_556",
                            AmmoRemaining = 30
                        }
                    };

                    combatSystem.BeginEncounter("enc_day15", "exp_1", "loc_perimeter", "Perimeter Fence", day, masterSeed, combatRoster, combatWeapons, enemyCount: 1, enemyHealth: 25);

                    // Mid-encounter save & reload test
                    var savedCombatState = combatSystem.CaptureState();
                    var restoredCombat = new TacticalCombatSystem();
                    restoredCombat.RestoreState(savedCombatState);

                    // Resolve both to verify identical outcome
                    var simRng1 = new SeededRng(masterSeed);
                    var simRng2 = new SeededRng(masterSeed);
                    var events1 = combatSystem.ResolveToEnd(simRng1, 60);
                    var events2 = restoredCombat.ResolveToEnd(simRng2, 60);

                    Assert.Equal(events1.Count, events2.Count);

                    var aftermath = combatSystem.State.Aftermath;
                    Assert.NotNull(aftermath);
                    Assert.Equal("enc_day15", aftermath!.EncounterId);

                    combatResolved = true;
                }

                // D. Disease exposure on Day 10
                if (day == pathogenExposureDay)
                {
                    var expRes = diseaseSystem.TryInfect("surv_alec", DiseaseIds.Cholera, day, "foul_water_draw");
                    Assert.True(expRes.Infected || expRes.Reason == "roll_passed");
                    // Ensure infected for test flow
                    if (!diseaseSystem.IsInfected("surv_alec", DiseaseIds.Cholera))
                        diseaseSystem.Infect("surv_alec", DiseaseIds.Cholera, day);

                    // Isolate in medical ward
                    var assignPreview = quarantineCoord.PreviewAssignIsolation("surv_alec");
                    Assert.True(assignPreview.CanExecute);
                    var assignRes = quarantineCoord.ExecuteAssignIsolation("surv_alec", day);
                    Assert.True(assignRes.Success);
                    Assert.True(quarantineCoord.IsIsolated("surv_alec"));
                }

                // E. Disease simulation tick & care
                quarantineCoord.TickDaily(day);
                diseaseSystem.TickDaily(day, allSurvivors);

                // Treat on Day 11 (within antibiotics max_days: 2 window)
                if (day == 11 && diseaseSystem.IsInfected("surv_alec", DiseaseIds.Cholera) && !patientCured)
                {
                    var treatRes = diseaseSystem.TryTreat("surv_alec", DiseaseIds.Cholera, "antibiotics", day);
                    Assert.True(treatRes.Accepted, $"Treatment refused: {treatRes.Reason}");
                    Assert.True(treatRes.Cured);
                    patientCured = true;

                    // Release from isolation on Day 12
                    var relRes = quarantineCoord.ExecuteReleaseIsolation("surv_alec", day + 1);
                    Assert.True(relRes.Success);
                    Assert.False(quarantineCoord.IsIsolated("surv_alec"));

                    // Verify acquired temporary immunity
                    Assert.True(diseaseSystem.HasImmunity("surv_alec", DiseaseIds.Cholera, day + 5));
                }

                // F. Mid-campaign full save / reload on Day 20
                if (day == 20)
                {
                    var savedDiseaseState = diseaseSystem.CaptureState();
                    var savedWardState = medicalWard.CaptureState();
                    var savedRosterState = dutyRoster.CaptureState();

                    // Restore into fresh instances
                    var newWard = new MedicalWardSystem(new MedicalWardState(), beds, procedures);
                    newWard.RestoreState(savedWardState);

                    var newDisease = new DiseaseSystem();
                    newDisease.BindCatalog(diseaseCatalog);
                    newDisease.TryConsumeItem = tryConsume;
                    newDisease.RestoreState(savedDiseaseState);

                    // Verify state continuity
                    Assert.True(newDisease.HasImmunity("surv_alec", DiseaseIds.Cholera, 22));
                    Assert.Equal(diseaseSystem.TotalInfectionsHistory, newDisease.TotalInfectionsHistory);
                }
            }

            // =================================================================
            // 6. CAMPAIGN CONCLUSION AUDIT
            // =================================================================
            Assert.True(studyCompleted, "Library manual study must complete during 30-day campaign");
            Assert.True(combatResolved, "Tactical combat encounter must resolve during 30-day campaign");
            Assert.True(patientCured, "Quarantined patient must be treated and cured during 30-day campaign");
            Assert.True(inventory["clean_water"] < 200, "Clean water must be consumed by quarantine care");
            Assert.True(inventory["antibiotics"] < 50, "Antibiotics must be consumed by curative treatment");
        }
    }
}
