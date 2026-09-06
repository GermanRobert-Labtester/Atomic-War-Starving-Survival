using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Xunit;

namespace Ashfall.Core.Tests.Campaign
{
    public class DailyBriefingCadenceTests
    {
        [Fact]
        public void CadenceFilter_SuppressesUnchangedPersistentWarnings()
        {
            var history = new Dictionary<string, DailyBriefingCadenceRecord>(StringComparer.Ordinal);

            // Day 1: Warning with severity 50
            var factsDay1 = new[]
            {
                new BriefingFact("warn_filter_soot", "ventilation", BriefingTaxonomyClass.Warning, 50f, "Filter soot building up", 1, "panel:power_grid")
            };
            var resultDay1 = DailyBriefingCadenceFilter.FilterAndAdvance(factsDay1, history, currentDay: 1);
            Assert.Single(resultDay1);
            Assert.Equal("warn_filter_soot", resultDay1[0].FactId);

            // Day 2: Same warning with severity 55 (< 15 delta) -> suppressed!
            var factsDay2 = new[]
            {
                new BriefingFact("warn_filter_soot", "ventilation", BriefingTaxonomyClass.Warning, 55f, "Filter soot building up", 2, "panel:power_grid")
            };
            var resultDay2 = DailyBriefingCadenceFilter.FilterAndAdvance(factsDay2, history, currentDay: 2);
            Assert.Empty(resultDay2);

            // Day 3: Same warning rises to severity 70 (delta = 20 >= 15) -> emitted!
            var factsDay3 = new[]
            {
                new BriefingFact("warn_filter_soot", "ventilation", BriefingTaxonomyClass.Warning, 70f, "Filter soot critical", 3, "panel:power_grid")
            };
            var resultDay3 = DailyBriefingCadenceFilter.FilterAndAdvance(factsDay3, history, currentDay: 3);
            Assert.Single(resultDay3);
            Assert.Equal(70f, resultDay3[0].Severity);
        }

        [Fact]
        public void CadenceFilter_RepeatsCriticalThreatsEvery3Days()
        {
            var history = new Dictionary<string, DailyBriefingCadenceRecord>(StringComparer.Ordinal);

            var critFact = new BriefingFact("crit_starvation", "needs", BriefingTaxonomyClass.Critical, 90f, "Shelter starving", 1, "panel:medical");

            // Day 1: Emitted
            var res1 = DailyBriefingCadenceFilter.FilterAndAdvance(new[] { critFact }, history, currentDay: 1);
            Assert.Single(res1);

            // Day 2: Suppressed (1 day passed)
            var res2 = DailyBriefingCadenceFilter.FilterAndAdvance(new[] { critFact }, history, currentDay: 2);
            Assert.Empty(res2);

            // Day 3: Suppressed (2 days passed)
            var res3 = DailyBriefingCadenceFilter.FilterAndAdvance(new[] { critFact }, history, currentDay: 3);
            Assert.Empty(res3);

            // Day 4: Emitted! (3 days passed)
            var res4 = DailyBriefingCadenceFilter.FilterAndAdvance(new[] { critFact }, history, currentDay: 4);
            Assert.Single(res4);
        }

        [Fact]
        public void CadenceFilter_EmitsWhenConditionResolved()
        {
            var history = new Dictionary<string, DailyBriefingCadenceRecord>(StringComparer.Ordinal);

            // Day 1: Warning active at 60%
            var active = new BriefingFact("warn_generator_leak", "power", BriefingTaxonomyClass.Warning, 60f, "Coolant leak", 1);
            var res1 = DailyBriefingCadenceFilter.FilterAndAdvance(new[] { active }, history, currentDay: 1);
            Assert.Single(res1);

            // Day 2: Leak repaired, severity 0 -> emitted as resolution
            var resolved = new BriefingFact("warn_generator_leak", "power", BriefingTaxonomyClass.Warning, 0f, "Coolant leak resolved", 2);
            var res2 = DailyBriefingCadenceFilter.FilterAndAdvance(new[] { resolved }, history, currentDay: 2);
            Assert.Single(res2);
            Assert.Equal(0f, res2[0].Severity);
        }

        [Fact]
        public void ReportBuilder_ConstructsCategorizedReportWithDeepLinks()
        {
            var facts = new[]
            {
                new BriefingFact("crit_reactor", "power", BriefingTaxonomyClass.Critical, 95f, "Reactor core overheating", 1, "panel:power_grid"),
                new BriefingFact("warn_radon", "air", BriefingTaxonomyClass.Warning, 55f, "Radon levels elevated", 1, "panel:power_grid"),
                new BriefingFact("intel_silo", "map", BriefingTaxonomyClass.Intel, 25f, "Discovered silo coordinates", 1, "panel:wasteland_map?node=loc_silo"),
                new BriefingFact("flavor_echo", "journal", BriefingTaxonomyClass.Flavor, 5f, "Acoustic rumble heard from sector 4", 1)
            };

            var state = new DailyBriefingState();
            var report = DailyBriefingReportBuilder.BuildFromBriefingFacts(1, 100, facts, state);

            Assert.Equal(4, report.TotalEntries);
            Assert.Equal(4, report.Sections.Count);

            var critSec = report.Sections.FirstOrDefault(s => s.Title == "Critical Alerts");
            Assert.NotNull(critSec);
            Assert.Equal("panel:power_grid", critSec.Entries[0].DeepLinkRoute);

            var intelSec = report.Sections.FirstOrDefault(s => s.Title == "Intelligence & Recon");
            Assert.NotNull(intelSec);
            Assert.Equal("panel:wasteland_map?node=loc_silo", intelSec.Entries[0].DeepLinkRoute);
        }

        [Fact]
        public void SaveLoadRoundTrip_PreservesCadenceRecordsAndChecksum()
        {
            var state = new DailyBriefingState();
            state.CadenceRecords["fact_a"] = new DailyBriefingCadenceRecord("fact_a", 75f, 4);
            state.CadenceRecords["fact_b"] = new DailyBriefingCadenceRecord("fact_b", 40f, 6);

            var save = state.CaptureState();
            Assert.Equal(2, save.CadenceRecords.Count);

            var serializer = new SystemTextJsonSerializer();
            var encoded = DailyBriefingSaveCodec.Encode(save, serializer);
            string json = serializer.Serialize(encoded);

            var decoded = DailyBriefingSaveCodec.Decode(json, serializer);
            Assert.NotNull(decoded);
            Assert.Equal(2, decoded.CadenceRecords.Count);

            var restored = new DailyBriefingState();
            restored.RestoreState(decoded);
            Assert.Equal(2, restored.CadenceRecords.Count);
            Assert.Equal(75f, restored.CadenceRecords["fact_a"].LastReportedSeverity);
            Assert.Equal(4, restored.CadenceRecords["fact_a"].LastReportedDay);
        }

        [Fact]
        public void LegacySaveCompatibility_LoadsSafelyWhenCadenceMissing()
        {
            // Simulate legacy save without CadenceRecords field
            var legacySave = new DailyBriefingSave
            {
                saveVersion = 1,
                simDay = 12,
                PendingReports = new List<DailyBriefingReport>(),
                AcknowledgedDays = new List<int> { 1, 2, 3 }
            };
            legacySave.CadenceRecords = null!; // null in raw JSON

            var state = new DailyBriefingState();
            state.RestoreState(legacySave);

            Assert.NotNull(state.CadenceRecords);
            Assert.Empty(state.CadenceRecords);
            Assert.Contains(1, state.AcknowledgedDays);
            Assert.Contains(2, state.AcknowledgedDays);
        }
    }
}
