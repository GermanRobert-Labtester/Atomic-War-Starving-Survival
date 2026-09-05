// SPDX-License-Identifier: MIT
// Comprehensive unit tests for Plan 96 — Epilogue Chronicle Slides Expansion (5 -> 20 slides).

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core.Endgame;
using Ashfall.Core.Muster;
using Xunit;

namespace Ashfall.Core.Tests.Endgame
{
    public class EpilogueChronicleCatalogTests
    {
        private static string FindDataDir()
        {
            string candidate = Path.Combine(AppContext.BaseDirectory, "Assets", "StreamingAssets", "Data");
            if (Directory.Exists(candidate)) return candidate;

            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null)
            {
                string check = Path.Combine(dir.FullName, "Assets", "StreamingAssets", "Data");
                if (Directory.Exists(check)) return check;
                dir = dir.Parent;
            }
            return string.Empty;
        }

        private static EpilogueChronicleCatalogData LoadCatalog()
        {
            string dataDir = FindDataDir();
            Assert.False(string.IsNullOrEmpty(dataDir), "Could not find StreamingAssets/Data directory");
            var io = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var catalog = EpilogueChronicleLoader.Load(dataDir, io, json);
            Assert.NotNull(catalog);
            return catalog!;
        }

        [Fact]
        public void Catalog_LoadsSuccessfully_WithSchemaVersionOne()
        {
            var catalog = LoadCatalog();
            Assert.Equal(1, catalog.schema_version);
            Assert.NotNull(catalog.default_slides);
        }

        [Fact]
        public void SlideCount_ContainsExactlyTwentySlides()
        {
            var catalog = LoadCatalog();
            Assert.Equal(20, catalog.default_slides.Count);
        }

        [Fact]
        public void Parity_BaselineFiveSlidesArePreserved()
        {
            var catalog = LoadCatalog();
            var slides = catalog.default_slides;

            var opening = slides.FirstOrDefault(s => s.title == "Opening");
            Assert.NotNull(opening);
            Assert.Equal("epilogue_opening_placeholder", opening.art_asset_id);

            var bunker = slides.FirstOrDefault(s => s.title == "The Bunker");
            Assert.NotNull(bunker);
            Assert.Equal("epilogue_bunker_placeholder", bunker.art_asset_id);

            var remains = slides.FirstOrDefault(s => s.title == "What Remains");
            Assert.NotNull(remains);
            Assert.Equal("epilogue_remains_placeholder", remains.art_asset_id);

            var survivors = slides.FirstOrDefault(s => s.title == "Survivors");
            Assert.NotNull(survivors);
            Assert.Equal("epilogue_survivors_placeholder", survivors.art_asset_id);

            var finalWord = slides.FirstOrDefault(s => s.title == "Final Word");
            Assert.NotNull(finalWord);
            Assert.Equal("epilogue_final_placeholder", finalWord.art_asset_id);
        }

        [Fact]
        public void SlideOrders_AreUniqueAndSequentialZeroToNineteen()
        {
            var catalog = LoadCatalog();
            var orders = catalog.default_slides.Select(s => s.order).ToList();

            Assert.Equal(20, orders.Distinct().Count());
            for (int i = 0; i < 20; i++)
            {
                Assert.Contains(i, orders);
            }
        }

        [Fact]
        public void SlideTitles_AreNonEmptyAndConciseOneToFourWords()
        {
            var catalog = LoadCatalog();
            foreach (var slide in catalog.default_slides)
            {
                Assert.False(string.IsNullOrWhiteSpace(slide.title), $"Slide order {slide.order} has empty title");

                var words = slide.title.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                Assert.InRange(words.Length, 1, 4);
            }
        }

        [Fact]
        public void ArtAssetIds_FollowPlaceholderGrammarAndAreUnique()
        {
            var catalog = LoadCatalog();
            var artIds = new HashSet<string>(StringComparer.Ordinal);

            foreach (var slide in catalog.default_slides)
            {
                Assert.False(string.IsNullOrWhiteSpace(slide.art_asset_id), $"Slide {slide.title} has empty art_asset_id");
                Assert.StartsWith("epilogue_", slide.art_asset_id);
                Assert.EndsWith("_placeholder", slide.art_asset_id);
                Assert.True(artIds.Add(slide.art_asset_id), $"Duplicate art_asset_id found: {slide.art_asset_id}");
            }
        }

        [Fact]
        public void SemanticCollisions_NoRedundantOverlappingRoles()
        {
            var catalog = LoadCatalog();
            var titles = catalog.default_slides.Select(s => s.title).ToHashSet(StringComparer.OrdinalIgnoreCase);

            // Avoided duplicate synonyms
            Assert.DoesNotContain("The Shelter", titles);
            Assert.DoesNotContain("The Last Word", titles);
            Assert.DoesNotContain("The Ending", titles);
            Assert.DoesNotContain("The Future", titles);
        }

        [Fact]
        public void PillarCoverage_SpansAllMajorCampaignThemes()
        {
            var catalog = LoadCatalog();
            var titles = catalog.default_slides.Select(s => s.title).ToList();

            // Opening / Catastrophe
            Assert.Contains("Opening", titles);
            Assert.Contains("After the Flash", titles);
            Assert.Contains("The Bunker", titles);
            Assert.Contains("First Winter", titles);

            // Sustenance & Demographics
            Assert.Contains("Water and Heat", titles);
            Assert.Contains("Survivors", titles);
            Assert.Contains("Empty Bunks", titles);

            // World & Social Systems
            Assert.Contains("The Factions", titles);
            Assert.Contains("Lines on the Map", titles);
            Assert.Contains("Voices in Static", titles);

            // Forensic & Historical Records
            Assert.Contains("The Verdict", titles);
            Assert.Contains("The Witnesses", titles);
            Assert.Contains("Restored Relics", titles);
            Assert.Contains("What We Chose", titles);

            // Culmination & Legacy
            Assert.Contains("The Muster", titles);
            Assert.Contains("The Resolution", titles);
            Assert.Contains("The Census", titles);
            Assert.Contains("What Remains", titles);
            Assert.Contains("After Us", titles);
            Assert.Contains("Final Word", titles);
        }

        [Fact]
        public void BuilderIntegration_SortsAllTwentySlidesDeterministically()
        {
            string dataDir = FindDataDir();
            var io = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var slides = EpilogueChronicleLoader.LoadDefaultSlides(dataDir, io, json);

            Assert.Equal(20, slides.Count);

            // Shuffle input to test builder sorting
            var shuffled = slides.OrderBy(_ => Guid.NewGuid()).ToList();

            var builder = new EpilogueChronicleBuilder();
            var chronicle = builder.Build(new EpilogueChronicleInput
            {
                EndingKey = EpilogueMatrix.TheOpenMuster,
                Day = 360,
                BuildSeed = 42,
                Slides = shuffled
            });

            Assert.Equal(20, chronicle.Slides.Count);
            for (int i = 0; i < 20; i++)
            {
                Assert.Equal(i, chronicle.Slides[i].Order);
            }
            Assert.Equal("Opening", chronicle.Slides[0].Title);
            Assert.Equal("Final Word", chronicle.Slides[19].Title);
        }

        [Theory]
        [InlineData(EpilogueMatrix.TheOpenMuster, "The Muster", "epilogue_coalition_placeholder")]
        [InlineData(EpilogueMatrix.WaterPlantHeld, "Water and Heat", "epilogue_resources_placeholder")]
        [InlineData(EpilogueMatrix.GarrisonAbsorbsCoalition, "The Factions", "epilogue_factions_placeholder")]
        [InlineData(EpilogueMatrix.VerdictSectorRecounts, "The Verdict", "epilogue_investigations_placeholder")]
        [InlineData(EpilogueMatrix.MercyRoad, "What We Chose", "epilogue_key_decisions_placeholder")]
        public void Plan89Bindings_KeyOutcomesMapToRelevantSlides(string endingKey, string expectedTitle, string expectedArtId)
        {
            Assert.Contains(endingKey, EpilogueMatrix.AllKeys);
            var catalog = LoadCatalog();
            var matchingSlide = catalog.default_slides.FirstOrDefault(s => s.title == expectedTitle);

            Assert.NotNull(matchingSlide);
            Assert.Equal(expectedArtId, matchingSlide.art_asset_id);
        }
    }
}
