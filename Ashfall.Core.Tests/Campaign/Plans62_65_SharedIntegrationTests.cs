// SPDX-License-Identifier: MIT
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Inventory;
using Ashfall.Core.Research;
using Ashfall.Core.Shelter;
using Xunit;

namespace Ashfall.Core.Tests.Campaign
{
    public sealed class Plans62_65_SharedIntegrationTests
    {
        private static (FoodPreservationCatalog FoodCat, PrewarArchiveCatalog ArchiveCat, CaptiveInterrogationCatalog CaptiveCat, CampaignEpilogueCatalog EpilogueCat) CreateCatalogs()
        {
            var foodCat = new FoodPreservationCatalog
            {
                preservation_tiers = new List<PreservationTierDef>
                {
                    new PreservationTierDef { id = "preservation_ambient", shelf_life_days = 5 },
                    new PreservationTierDef { id = "preservation_root_cellar", shelf_life_days = 15 },
                    new PreservationTierDef { id = "preservation_salt_cured", shelf_life_days = 30 },
                    new PreservationTierDef { id = "preservation_cryogenic", shelf_life_days = 120, power_draw_watts = 150 }
                },
                curing_recipes = new List<CuringRecipeDef>
                {
                    new CuringRecipeDef
                    {
                        id = "recipe_cure_salt_rations",
                        target_tier = "preservation_salt_cured",
                        input_item_id = "raw_meat",
                        input_quantity = 2,
                        preservative_item_id = "chemicals",
                        preservative_quantity = 1,
                        output_item_id = "dried_rations",
                        output_quantity = 2,
                        work_ticks_required = 2
                    }
                }
            };
            foodCat.Index();

            var archiveCat = new PrewarArchiveCatalog
            {
                archives = new List<PrewarArchiveDef>
                {
                    new PrewarArchiveDef
                    {
                        id = "archive_orbital_telemetry_array",
                        display_name = "Orbital Telemetry Array Spool",
                        cleaning_solvent_id = "chemicals",
                        cleaning_solvent_count = 2,
                        base_effort_points = 60.0f,
                        reward_research_ids = new List<string> { "knowledge_encrypted_radio_blueprint" }
                    }
                }
            };
            archiveCat.Index();

            var captiveCat = new CaptiveInterrogationCatalog
            {
                captive_archetypes = new List<CaptiveArchetypeDef>
                {
                    new CaptiveArchetypeDef
                    {
                        id = "captive_raider_scout",
                        display_name = "Iron Raider Scout",
                        faction_origin = "faction_iron_raiders",
                        base_resistance = 30f,
                        base_hostility = 50f
                    }
                },
                interrogation_topics = new List<InterrogationTopicDef>
                {
                    new InterrogationTopicDef
                    {
                        id = "topic_arms_cache",
                        display_name = "Arms Depot Coordinates",
                        resistance_cost = 20f,
                        reward_item_id = "dried_rations"
                    }
                }
            };
            captiveCat.Index();

            var epilogueCat = new CampaignEpilogueCatalog
            {
                vignettes = new List<EpilogueVignetteDef>
                {
                    new EpilogueVignetteDef
                    {
                        id = "epilogue_demo_thriving",
                        category = "demographics",
                        priority = 10,
                        min_survivors = 10,
                        max_survivors = 999,
                        max_starvation_deaths = 0,
                        title = "A Beacon in the Ash",
                        narrative = "The shelter grew into a thriving bastion."
                    },
                    new EpilogueVignetteDef
                    {
                        id = "epilogue_gov_reconciliation",
                        category = "governance",
                        priority = 10,
                        min_paroled_captives = 1,
                        max_penal_shifts = 50,
                        title = "The Covenant of Common Ground",
                        narrative = "Paroled adversaries took up toolboxes beside their former jailers."
                    },
                    new EpilogueVignetteDef
                    {
                        id = "epilogue_tech_renaissance",
                        category = "technology",
                        priority = 10,
                        min_archives_decrypted = 1,
                        title = "The Rediscovered Light",
                        narrative = "The shelter resurrected lost pre-war sciences."
                    },
                    new EpilogueVignetteDef
                    {
                        id = "epilogue_food_granary",
                        category = "sustenance",
                        priority = 10,
                        min_starvation_deaths = 0,
                        max_starvation_deaths = 0,
                        title = "The Granary of the Deep",
                        narrative = "Every survivor sat down to a nutritious meal."
                    }
                }
            };
            epilogueCat.Index();

            return (foodCat, archiveCat, captiveCat, epilogueCat);
        }

        [Fact]
        public void ThirtyDay_FlagshipIntegrationScenario_SimulatesAllPillarsAndEvaluatesEpilogue()
        {
            var (foodCat, archiveCat, captiveCat, epilogueCat) = CreateCatalogs();

            var inv = new Inventory.Inventory();
            var rng = new SeededRng(4242);

            var foodSys = new FoodPreservationSystem(rng, inv, foodCat);
            var archiveSys = new PrewarArchiveDecryptionSystem(rng, inv, archiveCat);
            var prisonerSys = new ShelterPrisonerSystem(rng, inv, captiveCat);
            var epilogueEng = new CampaignEpilogueEngine(epilogueCat);

            // ── Phase 1: Days 1–5 (Early Setup & Stocking) ────────────────
            inv.AddById("chemicals", 5);
            inv.AddById("raw_meat", 6);

            // Store provisions in cryogenic freeze and root cellar
            foodSys.AddCohort("frozen_protein", 20, "preservation_cryogenic", 1);
            foodSys.AddCohort("cellar_protein", 10, "preservation_root_cellar", 1);

            // Excavation uncovers pre-war archive
            archiveSys.DiscoverArchive("archive_orbital_telemetry_array", 2);

            // Raider captured in skirmish
            prisonerSys.CapturePrisoner("captive_raider_scout", "Prisoner Vance", 3);
            prisonerSys.AssignGuards(new[] { "survivor_guard_ash" });

            for (int day = 1; day <= 5; day++)
            {
                foodSys.TickDay(day);
                archiveSys.TickDay(day);
                prisonerSys.TickDay(day);
            }

            Assert.Equal(30, foodSys.GetTotalFood());
            Assert.Equal(1, prisonerSys.PrisonerCount);

            // ── Phase 2: Days 6–10 (Stabilization & Interrogation) ─────────
            // Apply chemical solvent to archive and begin decryption
            var stabRes = archiveSys.StabilizeArchive("archive_orbital_telemetry_array", "survivor_lead_doc", 6);
            Assert.Equal(ActionResult.StatusKind.Success, stabRes.Status);
            archiveSys.StartDecryption("archive_orbital_telemetry_array", "survivor_lead_doc", 6);

            // Interrogate Vance via Rapport Building
            prisonerSys.Interrogate("captive_2", "topic_arms_cache", InterrogationApproach.RapportBuilding, "survivor_interrogator");
            prisonerSys.Interrogate("captive_2", "topic_arms_cache", InterrogationApproach.RapportBuilding, "survivor_interrogator");

            var vance = prisonerSys.GetPrisoner("captive_2");
            Assert.NotNull(vance);
            Assert.Contains("topic_arms_cache", vance.ExtractedTopics);

            // Start salt curing job
            foodSys.StartCuringJob("recipe_cure_salt_rations", "survivor_cook", 7);

            for (int day = 6; day <= 10; day++)
            {
                foodSys.TickDay(day);
                archiveSys.TickDay(day);
                prisonerSys.TickDay(day);
            }

            Assert.Equal(3, inv.CountById("dried_rations")); // Curing produced 2 + topic extraction gave 1

            // ── Phase 3: Days 11–15 (Power Blackout & Mid-Campaign Save) ──
            foodSys.SetPowerStatus(false);
            archiveSys.SetPowerStatus(false);

            for (int day = 11; day <= 15; day++)
            {
                foodSys.TickDay(day);
                archiveSys.TickDay(day); // Stalled
                prisonerSys.TickDay(day);

                // Daily consumption from lowest freshness
                foodSys.ConsumeFood("frozen_protein", 1, out _);
            }

            // Save campaign snapshot at Day 15
            var foodSaved = foodSys.CaptureState();
            var archiveSaved = archiveSys.CaptureState();
            var prisonerSaved = prisonerSys.CaptureState();

            // Recreate systems from save (Midgame Roundtrip Verification)
            var foodSys2 = new FoodPreservationSystem(rng, inv, foodCat);
            var archiveSys2 = new PrewarArchiveDecryptionSystem(rng, inv, archiveCat);
            var prisonerSys2 = new ShelterPrisonerSystem(rng, inv, captiveCat);

            foodSys2.RestoreState(foodSaved);
            archiveSys2.RestoreState(archiveSaved);
            prisonerSys2.RestoreState(prisonerSaved);

            Assert.False(foodSys2.IsPowerOnline);
            Assert.False(archiveSys2.IsPowerOnline);
            Assert.Equal(1, prisonerSys2.PrisonerCount);

            // ── Phase 4: Days 16–25 (Power Restored & Archive Decrypted) ───
            foodSys2.SetPowerStatus(true);
            archiveSys2.SetPowerStatus(true);

            for (int day = 16; day <= 25; day++)
            {
                foodSys2.TickDay(day);
                archiveSys2.TickDay(day);
                prisonerSys2.TickDay(day);
            }

            var proj = archiveSys2.GetProject("archive_orbital_telemetry_array");
            Assert.NotNull(proj);
            Assert.Equal(ArchiveDecryptionStatus.Completed, proj.Status);
            Assert.Equal(1, archiveSys2.TotalDecrypted);

            // Vance is now fully reformed
            var vance2 = prisonerSys2.GetPrisoner("captive_2");
            Assert.NotNull(vance2);
            vance2.Compliance = 90f;
            vance2.Hostility = 10f;
            vance2.Resistance = 0f;

            var paroleRes = prisonerSys2.GrantParole("captive_2", out string newCitizenId);
            Assert.Equal(ActionResult.StatusKind.Success, paroleRes.Status);
            Assert.Equal(1, prisonerSys2.TotalParoled);
            Assert.Equal(0, prisonerSys2.PrisonerCount);

            // ── Phase 5: Days 26–30 (Endgame Wrap & Grand Epilogue) ────────
            for (int day = 26; day <= 30; day++)
            {
                foodSys2.TickDay(day);
                archiveSys2.TickDay(day);
                prisonerSys2.TickDay(day);
            }

            // Assemble Campaign Epilogue Snapshot
            var snapshot = new CampaignEpilogueSnapshot
            {
                FinalDay = 30,
                SurvivorsAlive = 12,
                TotalCasualties = 0,
                StarvationDeaths = 0,
                DiseaseDeaths = 0,
                ArchivesDecrypted = archiveSys2.TotalDecrypted,
                TechNodesCompleted = 10,
                CaptivesParoled = prisonerSys2.TotalParoled,
                CaptivesInterrogated = 1,
                PenalLaborShiftsRun = 0,
                CampaignSeed = 4242
            };

            var chronicle = epilogueEng.GenerateChronicle(snapshot);
            Assert.NotNull(chronicle);
            Assert.Equal(30, chronicle.TotalDays);
            Assert.Equal(4, chronicle.Chapters.Count);

            var demoChap = chronicle.Chapters.Find(c => c.Category == "Demographics");
            var govChap = chronicle.Chapters.Find(c => c.Category == "Governance");
            var techChap = chronicle.Chapters.Find(c => c.Category == "Technology");
            var foodChap = chronicle.Chapters.Find(c => c.Category == "Sustenance");

            Assert.NotNull(demoChap);
            Assert.Equal("A Beacon in the Ash", demoChap.Title);

            Assert.NotNull(govChap);
            Assert.Equal("The Covenant of Common Ground", govChap.Title);

            Assert.NotNull(techChap);
            Assert.Equal("The Rediscovered Light", techChap.Title);

            Assert.NotNull(foodChap);
            Assert.Equal("The Granary of the Deep", foodChap.Title);

            // Verify report generation
            string formattedReport = chronicle.ToFormattedReport();
            Assert.Contains("CHRONICLE OF THE VAULT — DAY 30", formattedReport);
            Assert.Contains("Survivors Living: 12", formattedReport);

            // Verify JSON serialization
            string json = chronicle.ToJson();
            Assert.Contains("\"CampaignId\": \"ASHFALL_CAMPAIGN\"", json);
        }
    }
}
