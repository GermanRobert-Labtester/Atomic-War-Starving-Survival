using System.Collections.Generic;
using Ashfall.Core.Campaign;
using Xunit;

namespace Ashfall.Core.Tests.Campaign
{
    public sealed class CampaignProvenanceTests
    {
        [Fact]
        public void ProvenanceRecord_CreationAndClone_PreservesValues()
        {
            var record = new CampaignProvenanceRecord(
                KnowledgeSourceKind.ExpeditionVisit,
                "loc_broadcast_bunker_echo",
                "expedition_system",
                14,
                InformationConfidence.Confirmed,
                "survivor_daniels");

            var clone = record.Clone();

            Assert.Equal(KnowledgeSourceKind.ExpeditionVisit, clone.SourceKind);
            Assert.Equal("loc_broadcast_bunker_echo", clone.SourceId);
            Assert.Equal("expedition_system", clone.ProducerSystemId);
            Assert.Equal(14, clone.DayObserved);
            Assert.Equal(InformationConfidence.Confirmed, clone.Confidence);
            Assert.Equal("survivor_daniels", clone.RelatedEntityId);
        }

        [Fact]
        public void Merge_UnionsRecordsAndPreservesEarliestDayAndHighestConfidence()
        {
            var rec1 = new CampaignProvenanceRecord(
                KnowledgeSourceKind.RadioIntercept,
                "sig_relay_alpha",
                "radio_system",
                10,
                InformationConfidence.Low);

            var rec2 = new CampaignProvenanceRecord(
                KnowledgeSourceKind.RadioIntercept,
                "sig_relay_alpha",
                "radio_system",
                8,
                InformationConfidence.Medium);

            var rec3 = new CampaignProvenanceRecord(
                KnowledgeSourceKind.FieldGuide,
                "flora_glowing_lichen",
                "field_guide",
                12,
                InformationConfidence.Confirmed);

            var merged = CampaignProvenanceEvaluator.Merge(
                new List<CampaignProvenanceRecord> { rec1 },
                new List<CampaignProvenanceRecord> { rec2, rec3 });

            Assert.Equal(2, merged.Count);

            // Radio intercept record was updated with earliest day (8) and highest confidence (Medium)
            var radio = merged.Find(r => r.SourceKind == KnowledgeSourceKind.RadioIntercept);
            Assert.NotNull(radio);
            Assert.Equal(8, radio.DayObserved);
            Assert.Equal(InformationConfidence.Medium, radio.Confidence);

            Assert.Equal(8, CampaignProvenanceEvaluator.EarliestDay(merged));
            Assert.Equal(InformationConfidence.Confirmed, CampaignProvenanceEvaluator.HighestConfidence(merged));
        }
    }
}
