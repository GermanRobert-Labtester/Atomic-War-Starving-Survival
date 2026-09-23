// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Content;
using Ashfall.Core.Journal;
using Ashfall.Core.Memorial;
using Ashfall.Core.Narrative;
using Ashfall.Core.Phantoms;
using Xunit;

namespace Ashfall.Core.Tests.Content
{
    public sealed class Plan41_45MemoryAcceptanceIntegrationTests
    {
        [Fact]
        public void Plan41_MemorialEulogyAndHeirloomModifiers_EndToEnd()
        {
            // 1. MemorialSystem with ProceduralEulogyEngine
            var eulogyEngine = new ProceduralEulogyEngine();
            var memorialState = new MemorialState();
            var memorialSystem = new MemorialSystem(memorialState)
            {
                EulogyEngine = eulogyEngine
            };

            var lifeRecord = new DwellerLifeRecord
            {
                dwellerId = "dweller_elias",
                dwellerName = "Elias Vance",
                preWarProfession = "Archivist",
                daysSurvived = 145,
                shiftsCompleted = 82,
                causeOfDeath = "radiation_sickness",
                favoriteRelicName = "Brass Monocular",
                memorableBarkSnippets = new List<string> { "The records survive us." }
            };

            var memorialInput = new MemorialInput
            {
                SurvivorId = "dweller_elias",
                Cause = "radiation_sickness",
                Day = 150,
                BirthDay = 5,
                HeirloomItemId = "item_heirloom_brass_monocular",
                LifeRecord = lifeRecord
            };

            var entry = memorialSystem.Memorialize(memorialInput);
            Assert.NotNull(entry);
            Assert.False(string.IsNullOrEmpty(entry.EulogyText));
            Assert.Contains("ELIAS VANCE", entry.EulogyText);
            Assert.Contains("Archivist", entry.EulogyText);
            Assert.Contains("Brass Monocular", entry.EulogyText);

            // Save/Restore roundtrip preserves eulogy
            var memorialCapture = memorialSystem.CaptureState();
            var restoredMemorial = new MemorialSystem(new MemorialState());
            restoredMemorial.RestoreState(memorialCapture);
            Assert.Single(restoredMemorial.Entries);
            Assert.Equal(entry.EulogyText, restoredMemorial.Entries[0].EulogyText);

            // 2. HeirloomSystem: holder morale and fatigue relief
            var heirloomCatalog = new HeirloomCatalog();
            var serializer = new SystemTextJsonSerializer();
            string heirloomJson = @"{
                ""schema_version"": 1,
                ""items"": [
                    {
                        ""heirloom_id"": ""item_heirloom_brass_monocular"",
                        ""title"": ""Brass Monocular"",
                        ""holder_memories"": [
                            { ""affinity_key"": ""generic"", ""memory_text"": ""Observing horizons."", ""morale_effect"": 8.0, ""guilt_effect"": 0.0 }
                        ]
                    }
                ]
            }";
            heirloomCatalog.Load(heirloomJson, serializer);

            var heirloomSystem = new HeirloomSystem(heirloomCatalog);
            heirloomSystem.CreateInstance("item_heirloom_brass_monocular", "dweller_protege", 150);

            float moraleMod = heirloomSystem.GetHolderMoraleModifier("dweller_protege");
            float fatigueRelief = heirloomSystem.GetHolderFatigueRelief("dweller_protege");

            Assert.Equal(8.0f, moraleMod);
            Assert.Equal(0.5f, fatigueRelief); // 1 heirloom * 0.5f

            // 3. LocationMemorySystem: site visit tracking
            var files = new FileSystemIO();
            var locMemory = new LocationMemorySystem(files, serializer);

            Assert.False(locMemory.HasVisited("site_observatory"));
            Assert.Equal(0, locMemory.GetSiteVisitCount("site_observatory"));

            locMemory.RecordSiteVisit("site_observatory", day: 152);
            Assert.True(locMemory.HasVisited("site_observatory"));
            Assert.Equal(1, locMemory.GetSiteVisitCount("site_observatory"));
            Assert.Equal(152, locMemory.GetLastVisitDay("site_observatory"));

            locMemory.RecordSiteVisit("site_observatory", day: 160);
            Assert.Equal(2, locMemory.GetSiteVisitCount("site_observatory"));
            Assert.Equal(160, locMemory.GetLastVisitDay("site_observatory"));

            // 4. CohortSystem: calendar-driven maturation
            var cohortSystem = new CohortSystem();
            cohortSystem.BookChild("child_maya", new List<string> { "parent_a" }, "medium", birthDay: 10);

            // Day 80: with 100-day maturation age, still a child
            int maturedAt80 = cohortSystem.CheckCohortMaturation(currentDay: 80, maturationAgeDays: 100);
            Assert.Equal(0, maturedAt80);
            Assert.False(cohortSystem.GetChild("child_maya")!.isMatured);

            // Day 120: reaches 110 days old (>= 100) -> matures!
            int maturedAt120 = cohortSystem.CheckCohortMaturation(currentDay: 120, maturationAgeDays: 100);
            Assert.Equal(1, maturedAt120);
            Assert.True(cohortSystem.GetChild("child_maya")!.isMatured);
            Assert.Equal(120, cohortSystem.GetChild("child_maya")!.maturationDay);
        }

        [Fact]
        public void Plan45_ContentAcceptancePipeline_Orchestration()
        {
            // Verify full 8-rung content acceptance ladder
            var rungs = ContentAcceptancePipeline.OrderedRungs;
            Assert.Equal(8, rungs.Count);

            // 1. Valid gameplay consumed catalog
            var validCatalog = new CatalogEntry
            {
                Path = "heirlooms.json",
                DefinitionCount = 12,
                Classification = ContentClassification.GAMEPLAY_CONSUMED,
                RequiredRung = ContentAcceptanceRung.EFFECT_PRODUCED,
                Loader = "HeirloomCatalogLoader",
                MaxStage = UtilizationStage.EFFECT_PRODUCED
            };
            validCatalog.ConsumerSystems.Add("HeirloomSystem");
            validCatalog.ConsumerSystems.Add("NeedsSystem");

            var validResult = ContentAcceptancePipeline.Evaluate(validCatalog, failFast: true);
            Assert.True(validResult.IsSuccess);
            Assert.Null(validResult.FailedRung);
            Assert.Equal(6, validResult.RungResults.Count); // Rungs 1..6 evaluated up to required

            // 2. Catalog missing required consumers fails at CONSUMER_EXISTS rung
            var missingConsumerCatalog = new CatalogEntry
            {
                Path = "dead_catalog.json",
                DefinitionCount = 5,
                Classification = ContentClassification.GAMEPLAY_CONSUMED,
                RequiredRung = ContentAcceptanceRung.EFFECT_PRODUCED,
                Loader = "DeadCatalogLoader",
                MaxStage = UtilizationStage.LOADED
            };
            // ConsumerSystems is empty!

            var failResult = ContentAcceptancePipeline.Evaluate(missingConsumerCatalog, failFast: true);
            Assert.False(failResult.IsSuccess);
            Assert.Equal(ContentAcceptanceRung.CONSUMER_EXISTS, failResult.FailedRung);
        }

        [Fact]
        public void Plan41_Plan45_CombinedEcosystem_MemorialHeirloomAcceptanceJourney()
        {
            // Scenario:
            // 1. Content pipeline validates that the memorial and heirloom catalogs meet full acceptance criteria.
            // 2. An expedition encounters a hazardous faction border patrol.
            // 3. A dweller falls during the hazardous wasteland border journey.
            // 4. Shelter completes funeral rites: ProceduralEulogyEngine crafts a permanent memorial.
            // 5. Deceased's heirloom is passed to an apprentice survivor, providing tangible comfort and fatigue relief.
            // 6. Location memory registers the expedition revisit count at the border crossroads.

            // Step 1: Content Acceptance verification of the heirloom system
            var heirloomCatalogEntry = new CatalogEntry
            {
                Path = "items_heirlooms.json",
                DefinitionCount = 20,
                Classification = ContentClassification.GAMEPLAY_CONSUMED,
                RequiredRung = ContentAcceptanceRung.EFFECT_PRODUCED,
                Loader = "HeirloomCatalog",
                MaxStage = UtilizationStage.EFFECT_PRODUCED
            };
            heirloomCatalogEntry.ConsumerSystems.Add("HeirloomSystem");

            var acceptance = ContentAcceptancePipeline.Evaluate(heirloomCatalogEntry);
            Assert.True(acceptance.IsSuccess);

            // Step 2: Faction border encounter on wasteland route
            var encounter = new TravelEncounterDefinition
            {
                Id = "enc_iron_garrison_border_checkpoint",
                Category = "Human",
                FactionId = "iron_coalition",
                TerritoryState = "controlled",
                MinDangerLevel = 2f,
                MaxDangerLevel = 5f
            };
            encounter.Choices.Add(new TravelEncounterChoice
            {
                ChoiceId = "choice_pay_tariff",
                Text = "Pay Iron Garrison border transit toll",
                FactionId = "iron_coalition",
                FactionStandingDelta = 5,
                MoraleDelta = -2
            });
            encounter.Choices.Add(new TravelEncounterChoice
            {
                ChoiceId = "choice_force_crossing",
                Text = "Break through the barricade",
                FactionId = "iron_coalition",
                FactionStandingDelta = -20,
                MoraleDelta = 5
            });

            Assert.Equal("iron_coalition", encounter.FactionId);
            Assert.Equal("controlled", encounter.TerritoryState);
            Assert.Equal(2, encounter.Choices.Count);

            // Step 3 & 4: Memorialize fallen traveler
            var memorialSystem = new MemorialSystem(new MemorialState())
            {
                EulogyEngine = new ProceduralEulogyEngine()
            };
            var entry = memorialSystem.Memorialize(new MemorialInput
            {
                SurvivorId = "dweller_scout_kane",
                Cause = "border_skirmish",
                Day = 85,
                BirthDay = 1,
                HeirloomItemId = "item_heirloom_scout_compass",
                LifeRecord = new DwellerLifeRecord
                {
                    dwellerId = "dweller_scout_kane",
                    dwellerName = "Kane",
                    preWarProfession = "Surveyor",
                    daysSurvived = 84,
                    shiftsCompleted = 45,
                    causeOfDeath = "border_skirmish",
                    favoriteRelicName = "Scout Compass",
                    memorableBarkSnippets = new List<string> { "Watch the tree line." }
                }
            });
            Assert.NotNull(entry);
            Assert.Contains("Surveyor", entry.EulogyText);

            // Step 5: Heirloom inheritance
            var catalog = new HeirloomCatalog();
            catalog.Load(@"{
                ""schema_version"": 1,
                ""items"": [
                    {
                        ""heirloom_id"": ""item_heirloom_scout_compass"",
                        ""title"": ""Scout Compass"",
                        ""holder_memories"": [
                            { ""affinity_key"": ""generic"", ""memory_text"": ""Steady needle."", ""morale_effect"": 6.0, ""guilt_effect"": 0.0 }
                        ]
                    }
                ]
            }", new SystemTextJsonSerializer());

            var heirloomSys = new HeirloomSystem(catalog);
            heirloomSys.CreateInstance("item_heirloom_scout_compass", "dweller_apprentice_tara", 85);
            Assert.Equal(6.0f, heirloomSys.GetHolderMoraleModifier("dweller_apprentice_tara"));
            Assert.Equal(0.5f, heirloomSys.GetHolderFatigueRelief("dweller_apprentice_tara"));

            // Step 6: Site revisit tracking
            var locMemory = new LocationMemorySystem(new FileSystemIO(), new SystemTextJsonSerializer());
            locMemory.RecordSiteVisit("loc_iron_border_outpost", 85);
            Assert.Equal(1, locMemory.GetSiteVisitCount("loc_iron_border_outpost"));
            Assert.Equal(85, locMemory.GetLastVisitDay("loc_iron_border_outpost"));
        }
    }
}
