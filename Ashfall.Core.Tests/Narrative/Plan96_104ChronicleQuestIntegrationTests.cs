// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Endgame;
using Ashfall.Core.IO;
using Ashfall.Core.Quests;
using Xunit;

namespace Ashfall.Core.Tests.Narrative
{
    /// <summary>
    /// Wave 40 Batch 8 Cross-System Integration Test:
    /// Validates Plan 96 (Epilogue Chronicle Expansion - 20 ending presentation slides)
    /// alongside Plan 104 (Narrative Questlines Expansion - 12 survivor-specific personal arcs).
    /// </summary>
    public sealed class Plan96_104ChronicleQuestIntegrationTests : CatalogTestBase
    {
        private static readonly (int order, string title, string artToken)[] BaselineFiveSlides =
        {
            (0, "Opening", "epilogue_opening_placeholder"),
            (2, "The Bunker", "epilogue_bunker_placeholder"),
            (5, "Survivors", "epilogue_survivors_placeholder"),
            (17, "What Remains", "epilogue_remains_placeholder"),
            (19, "Final Word", "epilogue_final_placeholder")
        };

        [Fact]
        public void Plan96_EpilogueChronicle_LoadsAllTwentySlides_WithStrictContiguityAndPlaceholderConventions()
        {
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            var catalogData = EpilogueChronicleLoader.Load(DataDirectory, files, json);
            Assert.NotNull(catalogData);
            Assert.Equal(1, catalogData!.schema_version);
            Assert.NotNull(catalogData.default_slides);
            Assert.Equal(20, catalogData.default_slides.Count);

            var slides = EpilogueChronicleLoader.LoadDefaultSlides(DataDirectory, files, json);
            Assert.NotNull(slides);
            Assert.Equal(20, slides.Count);

            var seenOrders = new HashSet<int>();
            for (int i = 0; i < 20; i++)
            {
                var slide = slides[i];
                Assert.NotNull(slide);
                Assert.Equal(i, slide.Order);
                Assert.True(seenOrders.Add(slide.Order), $"Duplicate slide order: {slide.Order}");
                Assert.False(string.IsNullOrWhiteSpace(slide.Title), $"Slide order {i} has empty title");
                Assert.False(string.IsNullOrWhiteSpace(slide.ArtAssetId), $"Slide order {i} has empty art_asset_id");
                Assert.StartsWith("epilogue_", slide.ArtAssetId, StringComparison.Ordinal);
                Assert.EndsWith("_placeholder", slide.ArtAssetId, StringComparison.Ordinal);
            }

            // Verify the 5 baseline slides remain preserved byte-for-byte in their designated sequence slots
            foreach (var (expectedOrder, expectedTitle, expectedToken) in BaselineFiveSlides)
            {
                var slide = slides.FirstOrDefault(s => s.Order == expectedOrder);
                Assert.NotNull(slide);
                Assert.Equal(expectedTitle, slide.Title);
                Assert.Equal(expectedToken, slide.ArtAssetId);
            }
        }

        [Fact]
        public void Plan104_NarrativeQuestlines_LoadsAllTwelveArcs_WithValidFourStageChainsAndBinaryCrisis()
        {
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            var questlines = NarrativeQuestlineCatalogLoader.LoadEntries(DataDirectory, files, json);
            Assert.NotNull(questlines);
            Assert.Equal(12, questlines.Count);

            var seenQuestIds = new HashSet<string>(StringComparer.Ordinal);
            var seenSurvivorIds = new HashSet<string>(StringComparer.Ordinal);

            foreach (var arc in questlines)
            {
                Assert.NotNull(arc);
                Assert.StartsWith("quest_", arc.questId, StringComparison.Ordinal);
                Assert.True(seenQuestIds.Add(arc.questId), $"Duplicate questId detected: {arc.questId}");

                Assert.False(string.IsNullOrWhiteSpace(arc.survivorId), $"Quest '{arc.questId}' missing survivorId");
                Assert.True(seenSurvivorIds.Add(arc.survivorId), $"Multiple questlines bound to survivorId: {arc.survivorId}");

                Assert.False(string.IsNullOrWhiteSpace(arc.title), $"Quest '{arc.questId}' missing title");
                Assert.StartsWith("loc_", arc.targetLocationId, StringComparison.Ordinal);

                Assert.NotNull(arc.stages);
                Assert.Equal(4, arc.stages.Count);

                // Stage 0: Discovery
                var stage0 = arc.FindStage(0);
                Assert.NotNull(stage0);
                Assert.Equal("Discovery", stage0!.name);
                Assert.False(string.IsNullOrWhiteSpace(stage0.description));
                Assert.NotNull(stage0.objectiveItems);
                Assert.NotEmpty(stage0.objectiveItems);

                // Stage 1: Investigation
                var stage1 = arc.FindStage(1);
                Assert.NotNull(stage1);
                Assert.Equal("Investigation", stage1!.name);
                Assert.False(string.IsNullOrWhiteSpace(stage1.description));

                // Stage 2: Crisis (Binary Branch)
                var stage2 = arc.FindStage(2);
                Assert.NotNull(stage2);
                Assert.Equal("Crisis", stage2!.name);
                Assert.True(stage2.HasBranch, $"Quest '{arc.questId}' stage 2 must carry binary crisis branch");
                Assert.NotNull(stage2.branchA);
                Assert.NotNull(stage2.branchB);
                Assert.NotEqual(stage2.branchA!.id, stage2.branchB!.id);

                Assert.False(string.IsNullOrWhiteSpace(stage2.branchA.label));
                Assert.False(string.IsNullOrWhiteSpace(stage2.branchA.description));
                Assert.False(string.IsNullOrWhiteSpace(stage2.branchA.traitGranted));

                Assert.False(string.IsNullOrWhiteSpace(stage2.branchB.label));
                Assert.False(string.IsNullOrWhiteSpace(stage2.branchB.description));
                Assert.False(string.IsNullOrWhiteSpace(stage2.branchB.traitGranted));

                // Stage 3: Resolution
                var stage3 = arc.FindStage(3);
                Assert.NotNull(stage3);
                Assert.Equal("Resolution", stage3!.name);
                Assert.False(string.IsNullOrWhiteSpace(stage3.description));
            }
        }

        [Fact]
        public void CrossSystem_SurvivorArcResolutionAndEndgameChronicle_ExhibitNarrativeCoherence()
        {
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            // 1. Load both catalogs
            var questlines = NarrativeQuestlineCatalogLoader.LoadEntries(DataDirectory, files, json);
            var slides = EpilogueChronicleLoader.LoadDefaultSlides(DataDirectory, files, json);
            Assert.Equal(12, questlines.Count);
            Assert.Equal(20, slides.Count);

            // 2. Initialize NarrativeQuestlineSystem and advance a representative arc (Marcus Olejnik - Machinist)
            var questSystem = new NarrativeQuestlineSystem(questlines);
            string survivorId = "marcus_olejnik";
            var arcDef = questSystem.GetDefinitionForSurvivor(survivorId);
            Assert.NotNull(arcDef);
            Assert.Equal("quest_the_machinists_regret", arcDef!.questId);

            // Start arc
            bool started = questSystem.TryBegin(survivorId, day: 10);
            Assert.True(started);
            var arcState = questSystem.GetArc(survivorId);
            Assert.NotNull(arcState);
            Assert.Equal(NarrativeArcStatus.Active, arcState!.status);
            Assert.Equal(0, arcState.currentStage);

            // Deliver Stage 0 objective item: blueprint_roll
            bool delivered0 = questSystem.TryDeliverItem(survivorId, "blueprint_roll", day: 11);
            Assert.True(delivered0, "Delivering blueprint_roll must advance stage 0");
            Assert.Equal(1, arcState.currentStage);

            // Deliver Stage 1 objective items: scrap_metal and soldering_kit
            bool delivered1a = questSystem.TryDeliverItem(survivorId, "scrap_metal", day: 12);
            Assert.True(delivered1a);
            Assert.Equal(1, arcState.currentStage);
            bool delivered1b = questSystem.TryDeliverItem(survivorId, "soldering_kit", day: 13);
            Assert.True(delivered1b);
            Assert.Equal(2, arcState.currentStage);
            Assert.Equal(NarrativeArcStatus.AwaitingBranch, arcState.status);

            // Choose branch A (Scrupulous Machinist: dismantle_weapon)
            bool branchChosen = questSystem.TryChooseBranch(survivorId, "dismantle_weapon", day: 25, out var chosenBranch);
            Assert.True(branchChosen);
            Assert.NotNull(chosenBranch);
            Assert.Equal("trait_scrupulous_machinist", chosenBranch!.traitGranted);
            Assert.Equal(NarrativeArcStatus.Resolved, arcState.status);
            Assert.Equal(3, arcState.currentStage);

            // 3. Construct Endgame Chronicle reflecting the resolved survivor state
            var fateCards = new List<SurvivorFateCard>
            {
                new SurvivorFateCard
                {
                    SurvivorId = survivorId,
                    DisplayName = "Marcus Olejnik",
                    Fate = $"Marcus dismantled the siege weapon, earning the trait {chosenBranch.traitGranted}.",
                    Survived = true
                }
            };

            var metrics = new List<EpilogueMetric>
            {
                new EpilogueMetric("survivors_living", 12f, "Living Dwellers"),
                new EpilogueMetric("arcs_resolved", 1f, "Survivor Arcs Completed"),
                new EpilogueMetric("days_survived", 365f, "Days Survived")
            };

            var builder = new EpilogueChronicleBuilder();
            var chronicle = builder.Build(new EpilogueChronicleInput
            {
                EndingKey = "knowing",
                Day = 365,
                BuildSeed = 42,
                Slides = slides,
                FateCards = fateCards,
                Metrics = metrics
            });

            Assert.NotNull(chronicle);
            Assert.Equal("Knowing", chronicle.Title);
            Assert.Equal(20, chronicle.Slides.Count);
            Assert.Single(chronicle.FateCards);
            Assert.Equal(survivorId, chronicle.FateCards[0].SurvivorId);
            Assert.Equal(3, chronicle.Metrics.Count);

            // Verify slide ordering and narrative milestones
            Assert.Equal("Opening", chronicle.Slides[0].Title);
            Assert.Equal("The Bunker", chronicle.Slides[2].Title);
            Assert.Equal("Survivors", chronicle.Slides[5].Title);
            Assert.Equal("Empty Bunks", chronicle.Slides[6].Title);
            Assert.Equal("The Resolution", chronicle.Slides[15].Title);
            Assert.Equal("Final Word", chronicle.Slides[19].Title);
        }

        [Fact]
        public void CrossSystem_DeterministicExecution_UnderRepeatedReloadsPasses()
        {
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            for (int i = 0; i < 50; i++)
            {
                var slides = EpilogueChronicleLoader.LoadDefaultSlides(DataDirectory, files, json);
                Assert.Equal(20, slides.Count);

                var questlines = NarrativeQuestlineCatalogLoader.LoadEntries(DataDirectory, files, json);
                Assert.Equal(12, questlines.Count);

                var arc = questlines[0];
                Assert.Equal(4, arc.stages.Count);
            }
        }
    }
}
