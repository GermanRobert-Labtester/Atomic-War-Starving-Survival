// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Linq;
using Xunit;
using Ashfall.Core.Survivors;
using Ashfall.Core.Random;

namespace Ashfall.Core.Tests.Plan150Romance
{
    public class Plan150RomanceFamilyIntegrationTests
    {
        private static string GetCatalogJson()
        {
            string filename = "romance_courtship.json";
            string[] candidates = new[]
            {
                Path.Combine(AppContext.BaseDirectory, "../../../../Assets/StreamingAssets/Data", filename),
                Path.Combine(AppContext.BaseDirectory, "../../../Assets/StreamingAssets/Data", filename),
                Path.Combine(Directory.GetCurrentDirectory(), "Assets/StreamingAssets/Data", filename),
                Path.Combine(Directory.GetCurrentDirectory(), "../Assets/StreamingAssets/Data", filename)
            };
            foreach (var c in candidates)
            {
                if (File.Exists(c)) return File.ReadAllText(c);
            }
            throw new FileNotFoundException($"Could not find {filename} in candidate paths.");
        }

        [Fact]
        public void CatalogLoad_LoadsCourtshipEventsAndWeights()
        {
            string json = GetCatalogJson();
            var catalog = RomanceCourtshipCatalog.LoadFromJson(json);

            Assert.NotNull(catalog);
            Assert.True(catalog.CourtshipEvents.Count >= 5, $"Expected >= 5 events, found {catalog.CourtshipEvents.Count}");

            Assert.Contains(catalog.CourtshipEvents, e => e.EventId == "shared_meal");
            Assert.Contains(catalog.CourtshipEvents, e => e.EventId == "quiet_walk");
            Assert.Contains(catalog.CourtshipEvents, e => e.EventId == "deep_conversation");
            Assert.Contains(catalog.CourtshipEvents, e => e.EventId == "crafted_gift");
            Assert.Contains(catalog.CourtshipEvents, e => e.EventId == "mutual_vigil");

            Assert.Equal(12, catalog.AgeDifferenceSoftLimit);
            Assert.Equal(15.0f, catalog.BeliefAlignmentBonus);
            Assert.Equal(20.0f, catalog.OpposingBeliefPenalty);
        }

        [Fact]
        public void CompatibilityCalculation_AgeAndBeliefs_ModulatesScore()
        {
            string json = GetCatalogJson();
            var catalog = RomanceCourtshipCatalog.LoadFromJson(json);
            var system = new RomanceFamilySystem(catalog);

            // Similar age, identical beliefs
            float scoreAligned = system.CalculateCompatibility(
                ageA: 28,
                ageB: 30,
                beliefA: "collectivism",
                beliefB: "collectivism",
                sharedTrauma: false);

            // Large age gap (25 years diff: 13 years excess * 2.0 = -26 penalty), opposing beliefs (-20 penalty)
            float scoreOpposed = system.CalculateCompatibility(
                ageA: 20,
                ageB: 45,
                beliefA: "collectivism",
                beliefB: "individualism",
                sharedTrauma: false);

            Assert.True(scoreAligned > scoreOpposed, $"Aligned score {scoreAligned} should exceed opposed score {scoreOpposed}");
            Assert.True(scoreAligned >= 65.0f, $"Aligned score was {scoreAligned}");
            Assert.True(scoreOpposed <= 25.0f, $"Opposed score was {scoreOpposed}");
        }

        [Fact]
        public void AttractionInitiation_DeterministicRng_FormsAttractionStage()
        {
            string json = GetCatalogJson();
            var catalog = RomanceCourtshipCatalog.LoadFromJson(json);
            var system = new RomanceFamilySystem(catalog);
            var rng = new SeededRng(1337);

            bool seamFired = false;
            RomanceStage capturedStage = RomanceStage.Attraction;
            system.OnRomanceStageAdvancedSeam += (a, b, stage) =>
            {
                seamFired = true;
                capturedStage = stage;
            };

            bool success = system.TryInitiateAttraction(
                survivorA: "surv_elena",
                survivorB: "surv_marcus",
                affinity: 70.0f,
                ageA: 30,
                ageB: 32,
                beliefA: "technocracy",
                beliefB: "technocracy",
                sharedTrauma: true,
                rng: rng,
                currentDay: 5,
                force: true);

            Assert.True(success);
            Assert.True(seamFired);
            Assert.Equal(RomanceStage.Attraction, capturedStage);

            var rel = system.GetRelationship("surv_elena", "surv_marcus");
            Assert.NotNull(rel);
            Assert.Equal(RomanceStage.Attraction, rel.Stage);
            Assert.Equal(15, rel.RomanceScore);

            // Attempting duplicate initiation between same survivors should fail
            bool dup = system.TryInitiateAttraction(
                survivorA: "surv_elena",
                survivorB: "surv_marcus",
                affinity: 80.0f,
                ageA: 30,
                ageB: 32,
                beliefA: "technocracy",
                beliefB: "technocracy",
                sharedTrauma: false,
                rng: rng,
                currentDay: 6,
                force: true);

            Assert.False(dup);
        }

        [Fact]
        public void CourtshipProgression_ReachesPartnershipAndBonded()
        {
            string json = GetCatalogJson();
            var catalog = RomanceCourtshipCatalog.LoadFromJson(json);
            var system = new RomanceFamilySystem(catalog);
            var rng = new SeededRng(2026);

            system.TryInitiateAttraction(
                survivorA: "surv_clara",
                survivorB: "surv_jonah",
                affinity: 80.0f,
                ageA: 26,
                ageB: 28,
                beliefA: "traditionalism",
                beliefB: "traditionalism",
                sharedTrauma: false,
                rng: rng,
                currentDay: 1,
                force: true);

            var rel = system.GetRelationship("surv_clara", "surv_jonah")!;
            Assert.Equal(RomanceStage.Attraction, rel.Stage);

            // Courtship event: shared meal (+5) -> score 20
            system.ConductCourtshipEvent("surv_clara", "surv_jonah", "shared_meal", 80.0f, rng, 2, forceSuccess: true);
            Assert.Equal(20, rel.RomanceScore);

            // Courtship event: quiet walk (+8) -> score 28 -> Courtship stage
            system.ConductCourtshipEvent("surv_clara", "surv_jonah", "quiet_walk", 80.0f, rng, 3, forceSuccess: true);
            Assert.Equal(28, rel.RomanceScore);
            Assert.Equal(RomanceStage.Courtship, rel.Stage);

            // Multiple deep conversations and gifts -> push to Partnership (>= 50) and then Bonded (>= 75)
            system.ConductCourtshipEvent("surv_clara", "surv_jonah", "crafted_gift", 80.0f, rng, 4, forceSuccess: true);
            system.ConductCourtshipEvent("surv_clara", "surv_jonah", "deep_conversation", 80.0f, rng, 5, forceSuccess: true);
            Assert.True(rel.RomanceScore >= 50);
            Assert.Equal(RomanceStage.Partnership, rel.Stage);

            system.ConductCourtshipEvent("surv_clara", "surv_jonah", "crafted_gift", 80.0f, rng, 6, forceSuccess: true);
            system.ConductCourtshipEvent("surv_clara", "surv_jonah", "deep_conversation", 80.0f, rng, 7, forceSuccess: true);
            system.ConductCourtshipEvent("surv_clara", "surv_jonah", "mutual_vigil", 80.0f, rng, 8, forceSuccess: true);

            Assert.True(rel.RomanceScore >= 75);
            Assert.Equal(RomanceStage.Bonded, rel.Stage);

            // Advance 30 days while bonded -> soulmate status
            for (int day = 9; day <= 39; day++)
            {
                system.AdvanceDay(day);
            }

            Assert.True(rel.IsSoulmate);
        }

        [Fact]
        public void FamilyUnitFormation_AndChildAdoption_MaintainsLineage()
        {
            string json = GetCatalogJson();
            var catalog = RomanceCourtshipCatalog.LoadFromJson(json);
            var system = new RomanceFamilySystem(catalog);
            var rng = new SeededRng(42);

            system.TryInitiateAttraction(
                survivorA: "surv_rachel",
                survivorB: "surv_samuel",
                affinity: 90.0f,
                ageA: 34,
                ageB: 35,
                beliefA: "collectivism",
                beliefB: "collectivism",
                sharedTrauma: true,
                rng: rng,
                currentDay: 1,
                force: true);

            // Establish family
            string establishedFamilyId = string.Empty;
            system.OnFamilyUnitEstablishedSeam += (famId, parents) =>
            {
                establishedFamilyId = famId;
            };

            var family = system.FormFamilyUnit("surv_rachel", "surv_samuel", "The Ashwood Hearth");
            Assert.NotNull(family);
            Assert.Equal("The Ashwood Hearth", family.FamilyName);
            Assert.Equal(establishedFamilyId, family.FamilyId);
            Assert.True(family.ContainsMember("surv_rachel"));
            Assert.True(family.ContainsMember("surv_samuel"));
            Assert.False(family.ContainsMember("child_toby"));

            // Welcome/Adopt child
            bool childSeamFired = false;
            system.OnChildWelcomedToFamilySeam += (famId, chId, adopted) =>
            {
                childSeamFired = true;
                Assert.Equal(family.FamilyId, famId);
                Assert.Equal("child_toby", chId);
                Assert.True(adopted);
            };

            bool childAdded = system.AddChildToFamily(family.FamilyId, "child_toby", isAdopted: true);
            Assert.True(childAdded);
            Assert.True(childSeamFired);
            Assert.True(family.ContainsMember("child_toby"));
        }

        [Fact]
        public void Persistence_CaptureAndRestoreRoundtrip()
        {
            string json = GetCatalogJson();
            var catalog = RomanceCourtshipCatalog.LoadFromJson(json);
            var system1 = new RomanceFamilySystem(catalog);
            var rng = new SeededRng(777);

            system1.TryInitiateAttraction(
                survivorA: "surv_maya",
                survivorB: "surv_alex",
                affinity: 85.0f,
                ageA: 27,
                ageB: 29,
                beliefA: "militarism",
                beliefB: "militarism",
                sharedTrauma: false,
                rng: rng,
                currentDay: 10,
                force: true);

            var rel = system1.GetRelationship("surv_maya", "surv_alex")!;
            rel.RomanceScore = 80;
            rel.Stage = RomanceStage.Bonded;
            rel.IsSoulmate = true;
            rel.CohabitationQuarters = "bunk_04";

            var fam = system1.FormFamilyUnit("surv_maya", "surv_alex", "House of Iron");
            system1.AddChildToFamily(fam.FamilyId, "child_leo", isAdopted: false);

            string savedJson = system1.CaptureState();
            Assert.Contains("surv_maya", savedJson);
            Assert.Contains("House of Iron", savedJson);
            Assert.Contains("child_leo", savedJson);

            var system2 = new RomanceFamilySystem(catalog);
            system2.RestoreState(savedJson);

            var restoredRel = system2.GetRelationship("surv_maya", "surv_alex");
            Assert.NotNull(restoredRel);
            Assert.Equal(RomanceStage.Bonded, restoredRel.Stage);
            Assert.Equal(80, restoredRel.RomanceScore);
            Assert.True(restoredRel.IsSoulmate);

            var restoredFam = system2.GetFamilyForSurvivor("surv_maya");
            Assert.NotNull(restoredFam);
            Assert.Equal("House of Iron", restoredFam.FamilyName);
            Assert.True(restoredFam.ContainsMember("surv_alex"));
            Assert.True(restoredFam.ContainsMember("child_leo"));
        }
    }
}
