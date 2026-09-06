// SPDX-License-Identifier: MIT
using System.Collections.Generic;
using Ashfall.Core.Campaign;
using Xunit;

namespace Ashfall.Core.Tests.Campaign
{
    public sealed class CampaignEpilogueEngineTests
    {
        private static CampaignEpilogueCatalog CreateMockCatalog()
        {
            var catalog = new CampaignEpilogueCatalog
            {
                vignettes = new List<EpilogueVignetteDef>
                {
                    new EpilogueVignetteDef
                    {
                        id = "epilogue_demo_thriving",
                        category = "demographics",
                        priority = 10,
                        min_survivors = 12,
                        max_survivors = 999,
                        max_starvation_deaths = 0,
                        title = "A Beacon in the Ash",
                        narrative = "The shelter grew into a thriving bastion."
                    },
                    new EpilogueVignetteDef
                    {
                        id = "epilogue_demo_bleak",
                        category = "demographics",
                        priority = 1,
                        min_survivors = 0,
                        max_survivors = 4,
                        max_starvation_deaths = 999,
                        title = "The Hollow Vault",
                        narrative = "Silence reclaimed the corridors."
                    },
                    new EpilogueVignetteDef
                    {
                        id = "epilogue_gov_reconciliation",
                        category = "governance",
                        priority = 10,
                        min_paroled_captives = 2,
                        max_penal_shifts = 10,
                        title = "The Covenant of Common Ground",
                        narrative = "Paroled adversaries took up toolboxes beside their former jailers."
                    },
                    new EpilogueVignetteDef
                    {
                        id = "epilogue_tech_renaissance",
                        category = "technology",
                        priority = 10,
                        min_archives_decrypted = 3,
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
            catalog.Index();
            return catalog;
        }

        [Fact]
        public void GenerateChronicle_ThrivingScenario()
        {
            var catalog = CreateMockCatalog();
            var engine = new CampaignEpilogueEngine(catalog);

            var snapshot = new CampaignEpilogueSnapshot
            {
                FinalDay = 180,
                SurvivorsAlive = 15,
                TotalCasualties = 2,
                StarvationDeaths = 0,
                DiseaseDeaths = 1,
                ArchivesDecrypted = 4,
                TechNodesCompleted = 18,
                CaptivesParoled = 3,
                CaptivesInterrogated = 5,
                PenalLaborShiftsRun = 2,
                CampaignSeed = 1337
            };

            var chronicle = engine.GenerateChronicle(snapshot);
            Assert.NotNull(chronicle);
            Assert.Equal(180, chronicle.TotalDays);
            Assert.Equal(4, chronicle.Chapters.Count);

            var demo = chronicle.Chapters.Find(c => c.Category == "Demographics");
            var gov = chronicle.Chapters.Find(c => c.Category == "Governance");
            var tech = chronicle.Chapters.Find(c => c.Category == "Technology");
            var food = chronicle.Chapters.Find(c => c.Category == "Sustenance");

            Assert.NotNull(demo);
            Assert.Equal("A Beacon in the Ash", demo.Title);

            Assert.NotNull(gov);
            Assert.Equal("The Covenant of Common Ground", gov.Title);

            Assert.NotNull(tech);
            Assert.Equal("The Rediscovered Light", tech.Title);

            Assert.NotNull(food);
            Assert.Equal("The Granary of the Deep", food.Title);
        }

        [Fact]
        public void GenerateChronicle_DesolationScenario()
        {
            var catalog = CreateMockCatalog();
            var engine = new CampaignEpilogueEngine(catalog);

            var snapshot = new CampaignEpilogueSnapshot
            {
                FinalDay = 45,
                SurvivorsAlive = 1,
                TotalCasualties = 12,
                StarvationDeaths = 8,
                ArchivesDecrypted = 0,
                CaptivesParoled = 0,
                CampaignSeed = 777
            };

            var chronicle = engine.GenerateChronicle(snapshot);
            Assert.NotNull(chronicle);

            var demo = chronicle.Chapters.Find(c => c.Category == "Demographics");
            Assert.NotNull(demo);
            Assert.Equal("The Hollow Vault", demo.Title);
        }

        [Fact]
        public void GenerateChronicle_DeterministicReplay()
        {
            var catalog = CreateMockCatalog();
            var engine = new CampaignEpilogueEngine(catalog);

            var snap1 = new CampaignEpilogueSnapshot
            {
                FinalDay = 100,
                SurvivorsAlive = 14,
                ArchivesDecrypted = 5,
                CaptivesParoled = 2,
                CampaignSeed = 424242
            };

            var snap2 = new CampaignEpilogueSnapshot
            {
                FinalDay = 100,
                SurvivorsAlive = 14,
                ArchivesDecrypted = 5,
                CaptivesParoled = 2,
                CampaignSeed = 424242
            };

            var c1 = engine.GenerateChronicle(snap1);
            var c2 = engine.GenerateChronicle(snap2);

            Assert.Equal(c1.ToJson(), c2.ToJson());
        }

        [Fact]
        public void FormattedReport_ContainsExpectedHeaderAndChapters()
        {
            var catalog = CreateMockCatalog();
            var engine = new CampaignEpilogueEngine(catalog);

            var snapshot = new CampaignEpilogueSnapshot
            {
                FinalDay = 200,
                SurvivorsAlive = 16,
                ArchivesDecrypted = 4,
                CaptivesParoled = 2,
                CampaignSeed = 99
            };

            var chronicle = engine.GenerateChronicle(snapshot);
            string report = chronicle.ToFormattedReport();

            Assert.Contains("CHRONICLE OF THE VAULT — DAY 200", report);
            Assert.Contains("A Beacon in the Ash", report);
            Assert.Contains("Survivors Living: 16", report);
        }
    }
}
