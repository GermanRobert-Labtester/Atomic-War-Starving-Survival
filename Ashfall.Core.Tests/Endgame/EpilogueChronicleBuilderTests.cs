using System;
using System.Collections.Generic;
using Ashfall.Core.Endgame;
using Xunit;

namespace Ashfall.Core.Tests.Endgame
{
    public class EpilogueChronicleBuilderTests
    {
        [Fact]
        public void Build_ProducesTitleForKnownEnding()
        {
            var b = new EpilogueChronicleBuilder();
            var c = b.Build(new EpilogueChronicleInput
            {
                EndingKey = "knowing",
                Day = 365,
                BuildSeed = 7,
                Slides = new List<EpilogueSlide>(),
                FateCards = new List<SurvivorFateCard>(),
                Metrics = new List<EpilogueMetric>()
            });
            Assert.Equal("Knowing", c.Title);
            Assert.Equal("knowing", c.EndingKey);
        }

        [Fact]
        public void Build_SortsSlidesByOrder()
        {
            var b = new EpilogueChronicleBuilder();
            var c = b.Build(new EpilogueChronicleInput
            {
                EndingKey = "knowing",
                Day = 100,
                BuildSeed = 1,
                Slides = new List<EpilogueSlide>
                {
                    new EpilogueSlide(2, "Second", "..."),
                    new EpilogueSlide(0, "First", "..."),
                    new EpilogueSlide(1, "Middle", "...")
                },
                FateCards = new List<SurvivorFateCard>(),
                Metrics = new List<EpilogueMetric>()
            });
            Assert.Equal(3, c.Slides.Count);
            Assert.Equal("First", c.Slides[0].Title);
            Assert.Equal("Middle", c.Slides[1].Title);
            Assert.Equal("Second", c.Slides[2].Title);
        }

        [Fact]
        public void Build_SortsFateCardsBySurvivorId()
        {
            var b = new EpilogueChronicleBuilder();
            var c = b.Build(new EpilogueChronicleInput
            {
                EndingKey = "knowing",
                Day = 100,
                BuildSeed = 1,
                Slides = new List<EpilogueSlide>(),
                FateCards = new List<SurvivorFateCard>
                {
                    new SurvivorFateCard { SurvivorId = "zulu", DisplayName = "Zulu", Fate = "Survived", Survived = true },
                    new SurvivorFateCard { SurvivorId = "alpha", DisplayName = "Alpha", Fate = "Died", Survived = false }
                },
                Metrics = new List<EpilogueMetric>()
            });
            Assert.Equal("alpha", c.FateCards[0].SurvivorId);
            Assert.Equal("zulu", c.FateCards[1].SurvivorId);
        }

        [Fact]
        public void Build_SortsMetricsByMetricId()
        {
            var b = new EpilogueChronicleBuilder();
            var c = b.Build(new EpilogueChronicleInput
            {
                EndingKey = "knowing",
                Day = 100,
                BuildSeed = 1,
                Slides = new List<EpilogueSlide>(),
                FateCards = new List<SurvivorFateCard>(),
                Metrics = new List<EpilogueMetric>
                {
                    new EpilogueMetric("total_deaths", 5, "Deaths"),
                    new EpilogueMetric("days_survived", 365, "Days"),
                    new EpilogueMetric("morale_final", 75, "Morale")
                }
            });
            Assert.Equal("days_survived", c.Metrics[0].MetricId);
            Assert.Equal("morale_final", c.Metrics[1].MetricId);
            Assert.Equal("total_deaths", c.Metrics[2].MetricId);
        }

        [Fact]
        public void Build_DeterministicForSameInput()
        {
            var b = new EpilogueChronicleBuilder();
            var input = new EpilogueChronicleInput
            {
                EndingKey = "culpable",
                Day = 211,
                BuildSeed = 99,
                Slides = new List<EpilogueSlide>
                {
                    new EpilogueSlide(1, "Slide A", "Prose A"),
                    new EpilogueSlide(0, "Slide B", "Prose B")
                },
                FateCards = new List<SurvivorFateCard>
                {
                    new SurvivorFateCard { SurvivorId = "s2", DisplayName = "S2" },
                    new SurvivorFateCard { SurvivorId = "s1", DisplayName = "S1" }
                },
                Metrics = new List<EpilogueMetric>
                {
                    new EpilogueMetric("m1", 1f, "M1")
                }
            };
            var c1 = b.Build(input);
            var c2 = b.Build(input);
            Assert.Equal(c1.Title, c2.Title);
            Assert.Equal(c1.Slides[0].Title, c2.Slides[0].Title);
            Assert.Equal(c1.FateCards[0].SurvivorId, c2.FateCards[0].SurvivorId);
            Assert.Equal(c1.Metrics[0].MetricId, c2.Metrics[0].MetricId);
        }

        [Fact]
        public void Build_HandlesEmptyInput()
        {
            var b = new EpilogueChronicleBuilder();
            var c = b.Build(new EpilogueChronicleInput { EndingKey = "" });
            Assert.Equal("UNKNOWN ENDING", c.Title);
            Assert.Empty(c.Slides);
            Assert.Empty(c.FateCards);
            Assert.Empty(c.Metrics);
        }

        [Fact]
        public void Build_UnknownEndingFallsBackToKey()
        {
            var b = new EpilogueChronicleBuilder();
            var c = b.Build(new EpilogueChronicleInput { EndingKey = "novel_ending" });
            Assert.Equal("novel_ending", c.Title);
        }
    }
}
