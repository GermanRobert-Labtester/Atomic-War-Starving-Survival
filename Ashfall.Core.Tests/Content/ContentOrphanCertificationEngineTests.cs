using System.Collections.Generic;
using Ashfall.Core.Content;
using Xunit;

namespace Ashfall.Core.Tests.Content
{
    public class ContentOrphanCertificationEngineTests
    {
        [Fact]
        public void Certify_AllConsumersComposedAndPrerequisitesMet_PromotesCleanly()
        {
            var candidates = new[]
            {
                new ContentCandidateRow("mem_01", "memorials_expansion_05.json", "MemorialSystem", isPrerequisiteMet: true),
                new ContentCandidateRow("enc_01", "narrative_encounters.json", "NarrativeSystem", isPrerequisiteMet: true)
            };

            var activeConsumers = new HashSet<string> { "MemorialSystem", "NarrativeSystem" };

            var report = ContentOrphanCertificationEngine.Certify(candidates, activeConsumers);

            Assert.True(report.IsCertificationClean);
            Assert.True(report.CanPromotePlan49);
            Assert.Equal(2, report.CertifiedActiveCount);
            Assert.Equal(0, report.OrphanWarningCount);
            Assert.Empty(report.OrphanIds);
        }

        [Fact]
        public void Certify_UnmetPrerequisites_MarksExcludedDormant()
        {
            var candidates = new[]
            {
                new ContentCandidateRow("mem_02", "memorials.json", "MemorialSystem", isPrerequisiteMet: false)
            };

            var activeConsumers = new HashSet<string> { "MemorialSystem" };

            var report = ContentOrphanCertificationEngine.Certify(candidates, activeConsumers);

            Assert.True(report.IsCertificationClean);
            Assert.Equal(1, report.ExcludedDormantCount);
            Assert.Equal(0, report.CertifiedActiveCount);
            Assert.False(report.CanPromotePlan49); // Needs at least 1 active row to promote
        }

        [Fact]
        public void Certify_MissingOrUncomposedConsumer_FlagsOrphanWarning()
        {
            var candidates = new[]
            {
                new ContentCandidateRow("orphan_01", "items.json", "UncomposedSystem", isPrerequisiteMet: true),
                new ContentCandidateRow("orphan_02", "radio.json", "", isPrerequisiteMet: true)
            };

            var activeConsumers = new HashSet<string> { "MemorialSystem" };

            var report = ContentOrphanCertificationEngine.Certify(candidates, activeConsumers);

            Assert.False(report.IsCertificationClean);
            Assert.False(report.CanPromotePlan49);
            Assert.Equal(2, report.OrphanWarningCount);
            Assert.Contains("orphan_01", report.OrphanIds);
            Assert.Contains("orphan_02", report.OrphanIds);
        }

        [Fact]
        public void Certify_EmptyCandidates_ReturnsCleanZeroReport()
        {
            var report = ContentOrphanCertificationEngine.Certify(null, null);

            Assert.True(report.IsCertificationClean);
            Assert.Equal(0, report.TotalCandidatesEvaluated);
            Assert.False(report.CanPromotePlan49);
        }
    }
}
