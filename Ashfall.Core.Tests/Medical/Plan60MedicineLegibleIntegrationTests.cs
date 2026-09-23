#nullable enable
// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Disease;
using Ashfall.Core.Memorial;
using Xunit;

namespace Ashfall.Core.Tests.Medical
{
    public sealed class Plan60MedicineLegibleIntegrationTests
    {
        private static string ResolveDataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found)) return found;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found)) return found;
            throw new DirectoryNotFoundException("Data directory not found");
        }

        private static DiseaseCatalog LoadDiseaseCatalog() =>
            DiseaseCatalogLoader.Load(ResolveDataDir(), new FileSystemIO(), new SystemTextJsonSerializer());

        [Fact]
        public void DiseaseCatalog_LoadsCanonical15Diseases_AcrossAllFourVectors()
        {
            var catalog = LoadDiseaseCatalog();
            Assert.True(catalog.Diseases.Count >= 15, $"Expected at least 15 diseases, found {catalog.Diseases.Count}");

            var vectors = catalog.Diseases.Select(d => d.vector).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
            Assert.Contains(DiseaseVectorNames.Water, vectors);
            Assert.Contains(DiseaseVectorNames.Air, vectors);
            Assert.Contains(DiseaseVectorNames.Blood, vectors);
            Assert.Contains(DiseaseVectorNames.Spore, vectors);
        }

        [Fact]
        public void ClinicalStage_DerivesFromCatalogBounds_WithoutParallelAuthority()
        {
            var catalog = LoadDiseaseCatalog();
            var cholera = catalog.GetById("disease_cholera");
            Assert.NotNull(cholera);

            // Incubating stage
            var stage0 = DiseaseTriage.StageOf(cholera, 0);
            Assert.Equal(DiseaseClinicalStage.Incubating, stage0);

            // Ill stage
            var stageActive = DiseaseTriage.StageOf(cholera, cholera!.incubation_days);
            Assert.Equal(DiseaseClinicalStage.Ill, stageActive);

            // OutcomePending at full duration
            var stageEnd = DiseaseTriage.StageOf(cholera, cholera.illness_days);
            Assert.Equal(DiseaseClinicalStage.OutcomePending, stageEnd);
        }

        [Fact]
        public void ClinicalPicture_SurfacesDiagnosticTells_AndProtectsUndiagnosedTruth()
        {
            var catalog = LoadDiseaseCatalog();
            var cholera = catalog.GetById("disease_cholera");
            Assert.NotNull(cholera);

            // Undiagnosed patient does not spoil the disease display name
            var undiagnosedPicture = DiseaseTriage.PictureOf(cholera!, 1, diagnosed: false);
            Assert.False(undiagnosedPicture.Diagnosed);
            Assert.NotEmpty(undiagnosedPicture.Tell);

            // Diagnosed patient reveals clinical guidance
            var diagnosedPicture = DiseaseTriage.PictureOf(cholera!, 2, diagnosed: true);
            Assert.True(diagnosedPicture.Diagnosed);
            Assert.Equal(cholera!.display_name, diagnosedPicture.DisplayName);
            Assert.NotEmpty(diagnosedPicture.Guidance);
        }

        [Fact]
        public void SickList_SeveritySource_DistinguishesDoseFromIllness()
        {
            var sickList = new SickListSystem();
            sickList.Diagnose("patient_alec", DoseLedgerSystem.BandAmber, day: 5, severitySource: SickListSystem.SourceIllness, sourceId: "disease_cholera");

            var entry = sickList.Bands.FirstOrDefault(b => b.survivorId == "patient_alec");
            Assert.NotNull(entry);
            Assert.Equal(SickListSystem.SourceIllness, entry!.severitySource);
            Assert.Equal(DoseLedgerSystem.BandAmber, entry.band);
        }

        [Fact]
        public void GriefSink_Dispersion_CalculatesQualityAndAppliesToMourners()
        {
            var relations = new SurvivorRelationsSystem(new SeededRng(100));
            var griefSink = new RelationsGriefSink(relations);

            // Peaceful death reduces grief multiplier (0.5)
            float peacefulGrief = CapturingGriefSink.QualityScale(DeathQuality.Peaceful);
            Assert.Equal(0.5f, peacefulGrief);

            // Unattended death increases grief multiplier (1.25)
            float unattendedGrief = CapturingGriefSink.QualityScale(DeathQuality.Unattended);
            Assert.Equal(1.25f, unattendedGrief);

            // Disperse grief through the grief sink
            griefSink.ApplyDispersion("dweller_deceased", new[] { "mourner_a", "mourner_b" }, 10f, DeathQuality.Peaceful, 1);
            Assert.Equal(1, griefSink.AppliedEventCount);
        }
    }
}
