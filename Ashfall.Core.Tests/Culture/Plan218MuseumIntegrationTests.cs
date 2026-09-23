// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Culture;
using Xunit;

namespace Ashfall.Core.Tests.Culture
{
    public sealed class Plan218MuseumIntegrationTests
    {
        private const string SampleMuseumCatalog = @"{
  ""schema_version"": 1,
  ""artifact_templates"": [
    {
      ""id"": ""artifact_founding_charter"",
      ""name"": ""Shelter Founding Charter"",
      ""artifact_type"": ""document"",
      ""default_significance"": 95.0,
      ""default_origin"": ""Signed by shelter pioneers."",
      ""theme"": ""founding"",
      ""description"": ""Faded governance charter.""
    },
    {
      ""id"": ""artifact_first_geiger"",
      ""name"": ""First Geiger Counter"",
      ""artifact_type"": ""technological"",
      ""default_significance"": 85.0,
      ""default_origin"": ""Pioneer dosimeter."",
      ""theme"": ""technological"",
      ""description"": ""Vintage radiation gauge.""
    }
  ],
  ""exhibition_themes"": [""founding"", ""technological""]
}";

        [Fact]
        public void LoadCatalog_ParsesTemplatesAndTheme()
        {
            var museum = new ShelterMuseumSystem();
            museum.LoadCatalog(SampleMuseumCatalog);

            var template = museum.GetTemplate("artifact_founding_charter");
            Assert.NotNull(template);
            Assert.Equal("Shelter Founding Charter", template.name);
            Assert.Equal(ArtifactType.Document, template.ParseArtifactType());
            Assert.Equal(95.0f, template.default_significance);
        }

        [Fact]
        public void AppointCurator_RecordsEventAndExposesCurator()
        {
            var museum = new ShelterMuseumSystem();
            string? appointed = null;
            museum.OnCuratorAppointed += c => appointed = c;

            museum.AppointCurator("survivor_scholar", currentDay: 1);

            Assert.Equal("survivor_scholar", museum.CuratorId);
            Assert.Equal("survivor_scholar", appointed);
            Assert.Contains(museum.GetEvents(), e => e.EventType == "curator_appointed");
        }

        [Fact]
        public void DonateArtifact_FromTemplate_And_ManualDonation()
        {
            var museum = new ShelterMuseumSystem();
            museum.LoadCatalog(SampleMuseumCatalog);

            MuseumArtifact? donated = null;
            museum.OnArtifactDonated += a => donated = a;

            // Template donation
            var fromTpl = museum.DonateFromTemplate("artifact_founding_charter", "elder_vance", currentDay: 2);
            Assert.NotNull(fromTpl);
            Assert.Equal(fromTpl, donated);
            Assert.Equal("Shelter Founding Charter", fromTpl.ArtifactName);
            Assert.Equal(95f, fromTpl.HistoricalSignificance);

            // Manual donation
            var manual = museum.DonateArtifact(
                itemId: "rusty_wrench",
                name: "The First Sump Wrench",
                type: ArtifactType.Tool,
                originStory: "Used to clear the initial flooding in Sector 4.",
                significance: 65f,
                donorId: "mechanic_bob",
                currentDay: 3);

            Assert.NotNull(manual);
            Assert.Equal(2, museum.TotalArtifactCount);
            Assert.Equal(2, museum.DisplayedArtifactCount);
            Assert.Equal(80.0f, museum.GetHistoricalSignificanceScore()); // (95 + 65) / 2 = 80
        }

        [Fact]
        public void CurateExhibition_And_CloseExhibition_Lifecycle()
        {
            var museum = new ShelterMuseumSystem();
            var art = museum.DonateArtifact("item_x", "Relic", ArtifactType.Historical, "Found in dust", 75f, "scout", 1);

            Exhibition? opened = null;
            museum.OnExhibitionOpened += e => opened = e;

            var exhibition = museum.CurateExhibition(
                name: "Relics of the Dust",
                theme: ExhibitionTheme.Founding,
                artifactIds: new[] { art.ArtifactId },
                startDay: 5,
                durationDays: 10,
                description: "Artifacts from the surface expedition.",
                moraleBoost: 6.0f);

            Assert.NotNull(exhibition);
            Assert.Equal(exhibition, opened);
            Assert.Equal(1, museum.ActiveExhibitionCount);

            bool closed = museum.CloseExhibition(exhibition.ExhibitionId, currentDay: 12);
            Assert.True(closed);
            Assert.Equal(0, museum.ActiveExhibitionCount);
            Assert.Equal(ExhibitionStatus.Completed, exhibition.Status);
        }

        [Fact]
        public void VisitMuseum_IncrementsVisitorCount_AndGrantsMorale()
        {
            var museum = new ShelterMuseumSystem();
            museum.DonateArtifact("relic_1", "Great Banner", ArtifactType.Historical, "Original flag", 90f, "leader", 1);
            museum.CurateExhibition("Founding Echoes", ExhibitionTheme.Founding, new[] { "art_1" }, 1, 10, "Echoes", 8.0f);

            float moraleGained = museum.VisitMuseum("dweller_alice", currentDay: 3);

            Assert.True(moraleGained > 2.0f);
            Assert.Equal(1, museum.TotalVisitors);
            var activeExh = museum.GetActiveExhibitions().First();
            Assert.Equal(1, activeExh.VisitorCount);
        }

        [Fact]
        public void TickDay_AutoClosesExpiredExhibitions()
        {
            var museum = new ShelterMuseumSystem();
            var exh = museum.CurateExhibition("Temporary Showing", ExhibitionTheme.Cultural, Array.Empty<string>(), startDay: 1, durationDays: 3, "Short showing");

            Assert.Equal(1, museum.ActiveExhibitionCount);

            museum.TickDay(currentDay: 2);
            Assert.Equal(1, museum.ActiveExhibitionCount);

            museum.TickDay(currentDay: 4); // >= EndDay (1 + 3 = 4) -> closes
            Assert.Equal(0, museum.ActiveExhibitionCount);
            Assert.Equal(ExhibitionStatus.Completed, exh.Status);
        }

        [Fact]
        public void CaptureState_And_RestoreState_PreservesMuseumCollectionAndExhibitions()
        {
            var museum = new ShelterMuseumSystem();
            museum.AppointCurator("curator_helen", 1);
            museum.DonateArtifact("item_a", "Pioneer Badge", ArtifactType.Historical, "Given at founding", 85f, "dweller_1", 2);
            museum.CurateExhibition("Pioneer Legacy", ExhibitionTheme.Founding, new[] { "art_1" }, 2, 20, "Legacy", 7.0f);
            museum.VisitMuseum("visitor_bob", 3);

            var saved = museum.CaptureState();
            Assert.Equal("curator_helen", saved.CuratorId);
            Assert.Single(saved.Artifacts);
            Assert.Single(saved.Exhibitions);
            Assert.Equal(1, saved.TotalVisitors);

            var restored = new ShelterMuseumSystem();
            restored.RestoreState(saved);

            Assert.Equal("curator_helen", restored.CuratorId);
            Assert.Equal(1, restored.TotalArtifactCount);
            Assert.Equal(1, restored.ActiveExhibitionCount);
            Assert.Equal(1, restored.TotalVisitors);
            Assert.Equal("Pioneer Badge", restored.GetArtifactsOnDisplay().First().ArtifactName);
        }
    }
}
