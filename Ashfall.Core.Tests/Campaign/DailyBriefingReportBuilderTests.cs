using System;
using System.Collections.Generic;
using Ashfall.Core.Campaign;
using Xunit;

namespace Ashfall.Core.Tests.Campaign
{
    public class DailyBriefingReportBuilderTests
    {
        [Fact]
        public void Build_ReturnsTitleAndDay()
        {
            var r = DailyBriefingReportBuilder.Build(new DailyBriefingInputs { Day = 12 });
            Assert.Equal(12, r.Day);
            Assert.Equal("DAY 12 BRIEFING", r.Title);
        }

        [Fact]
        public void Build_EmptyInputs_ProducesNoSections()
        {
            var r = DailyBriefingReportBuilder.Build(new DailyBriefingInputs { Day = 1 });
            Assert.Equal(0, r.Sections.Count);
            Assert.True(r.IsEmpty);
            Assert.Equal(0, r.TotalEntries);
        }

        [Fact]
        public void Build_OrdersSectionsDeterministically()
        {
            var inputs = new DailyBriefingInputs
            {
                Day = 1,
                Warnings = new List<DailyBriefingEntry> {
                    new DailyBriefingEntry("warnings", "frost", "frost warning") },
                ResourceConsumption = new List<DailyBriefingEntry> {
                    new DailyBriefingEntry("resources", "clean_water", "-3 clean water") },
                Deaths = new List<DailyBriefingEntry> {
                    new DailyBriefingEntry("deaths", "mira", "Mira Vasquez — radiation") }
            };
            var r = DailyBriefingReportBuilder.Build(inputs);
            Assert.Equal(3, r.Sections.Count);
            // Section order is fixed by the builder: Survivors → Resources → … → Deaths → Warnings.
            Assert.Equal("Resource Consumption", r.Sections[0].Title);
            Assert.Equal("Deaths", r.Sections[1].Title);
            Assert.Equal("Warnings", r.Sections[2].Title);
        }

        [Fact]
        public void Build_SortsEntriesWithinSectionByPrimaryId()
        {
            var inputs = new DailyBriefingInputs
            {
                Day = 1,
                Warnings = new List<DailyBriefingEntry>
                {
                    new DailyBriefingEntry("warnings", "zulu", "z-fall"),
                    new DailyBriefingEntry("warnings", "alpha", "a-fall"),
                    new DailyBriefingEntry("warnings", "mike", "m-fall"),
                }
            };
            var r = DailyBriefingReportBuilder.Build(inputs);
            Assert.Equal("alpha", r.Sections[0].Entries[0].PrimaryId);
            Assert.Equal("mike", r.Sections[0].Entries[1].PrimaryId);
            Assert.Equal("zulu", r.Sections[0].Entries[2].PrimaryId);
        }

        [Fact]
        public void Build_TotalsAreCorrect()
        {
            var inputs = new DailyBriefingInputs
            {
                Day = 5,
                SurvivorChanges = new List<DailyBriefingEntry>
                {
                    new DailyBriefingEntry("survivor_changes", "elena_vasquez", "rested"),
                    new DailyBriefingEntry("survivor_changes", "marcus_olejnik", "wounded")
                },
                Deaths = new List<DailyBriefingEntry>
                {
                    new DailyBriefingEntry("deaths", "haruto_kobayashi", "suffocation")
                }
            };
            var r = DailyBriefingReportBuilder.Build(inputs);
            Assert.Equal(3, r.TotalEntries);
            Assert.False(r.IsEmpty);
        }

        [Fact]
        public void Build_DeterministicForSameInputs()
        {
            var inputs = new DailyBriefingInputs
            {
                Day = 5,
                BuildSeed = 42,
                Warnings = new List<DailyBriefingEntry> {
                    new DailyBriefingEntry("warnings", "b", "b-warn"),
                    new DailyBriefingEntry("warnings", "a", "a-warn") },
                Deaths = new List<DailyBriefingEntry> {
                    new DailyBriefingEntry("deaths", "b", "b-d"),
                    new DailyBriefingEntry("deaths", "a", "a-d") }
            };
            var r1 = DailyBriefingReportBuilder.Build(inputs);
            var r2 = DailyBriefingReportBuilder.Build(inputs);
            Assert.Equal(r1.Sections.Count, r2.Sections.Count);
            for (int i = 0; i < r1.Sections.Count; i++)
            {
                Assert.Equal(r1.Sections[i].Title, r2.Sections[i].Title);
                Assert.Equal(r1.Sections[i].Entries.Length, r2.Sections[i].Entries.Length);
                for (int j = 0; j < r1.Sections[i].Entries.Length; j++)
                {
                    Assert.Equal(r1.Sections[i].Entries[j].PrimaryId,
                                 r2.Sections[i].Entries[j].PrimaryId);
                    Assert.Equal(r1.Sections[i].Entries[j].Text,
                                 r2.Sections[i].Entries[j].Text);
                }
            }
        }

        [Fact]
        public void State_AcknowledgeMovesReportOutOfPending()
        {
            var s = new DailyBriefingState();
            var r = DailyBriefingReportBuilder.Build(new DailyBriefingInputs { Day = 3 });
            s.Enqueue(r);
            Assert.True(s.HasUnacknowledged(3));
            var consumed = s.Consume(3);
            Assert.NotNull(consumed);
            Assert.False(s.HasUnacknowledged(3));
            Assert.Contains(3, s.AcknowledgedDays);
        }

        [Fact]
        public void Save_RoundTrip_ProducesIdenticalChecksum()
        {
            var json = new SystemTextJsonSerializer();
            var save = new DailyBriefingSave
            {
                saveVersion = DailyBriefingSave.CurrentSaveVersion,
                simDay = 4,
                PendingReports = new List<DailyBriefingReport>
                {
                    DailyBriefingReportBuilder.Build(new DailyBriefingInputs
                    {
                        Day = 3,
                        Warnings = new List<DailyBriefingEntry>
                        {
                            new DailyBriefingEntry("warnings", "alpha", "frost")
                        }
                    })
                },
                AcknowledgedDays = new List<int> { 1, 2 }
            };
            string text = DailyBriefingSaveCodec.EncodeToString(save, json);
            var loaded = DailyBriefingSaveCodec.Decode(text, json);
            Assert.Equal(save.Checksum, loaded.Checksum);
            Assert.Single(loaded.PendingReports);
            Assert.Equal(2, loaded.AcknowledgedDays.Count);
        }

        [Fact]
        public void Save_TamperedChecksumRejected()
        {
            var json = new SystemTextJsonSerializer();
            var save = new DailyBriefingSave
            {
                simDay = 7,
                PendingReports = new List<DailyBriefingReport>
                {
                    DailyBriefingReportBuilder.Build(new DailyBriefingInputs
                    {
                        Day = 3,
                        Warnings = new List<DailyBriefingEntry>
                        {
                            new DailyBriefingEntry("warnings", "alpha", "frost")
                        }
                    })
                },
                AcknowledgedDays = new List<int> { 1, 2 }
            };
            string text = DailyBriefingSaveCodec.EncodeToString(save, json);
            // Flip one character inside the AcknowledgedDays array so the saved bytes change.
            int idx = text.IndexOf("AcknowledgedDays", StringComparison.Ordinal);
            char[] arr = text.ToCharArray();
            // Skip the property name itself; advance into the value area.
            int valueStart = text.IndexOf('[', idx);
            arr[valueStart + 2] = arr[valueStart + 2] == '2' ? '9' : '2';
            string tampered = new string(arr);
            Assert.Throws<InvalidOperationException>(() => DailyBriefingSaveCodec.Decode(tampered, json));
        }

        [Fact]
        public void Save_NewerVersionRejected()
        {
            var json = new SystemTextJsonSerializer();
            var save = new DailyBriefingSave { saveVersion = DailyBriefingSave.CurrentSaveVersion + 1, simDay = 1 };
            save.Checksum = SaveChecksum.Compute(save);
            string text = json.Serialize(save);
            Assert.Throws<InvalidOperationException>(() => DailyBriefingSaveCodec.Decode(text, json));
        }

        [Fact]
        public void Save_EmptyChecksumRejected()
        {
            var json = new SystemTextJsonSerializer();
            var save = new DailyBriefingSave { simDay = 1, Checksum = string.Empty };
            string text = json.Serialize(save);
            Assert.Throws<InvalidOperationException>(() => DailyBriefingSaveCodec.Decode(text, json));
        }

        [Fact]
        public void Save_Restore_PopulatesState()
        {
            var json = new SystemTextJsonSerializer();
            var state = new DailyBriefingState();
            var report = DailyBriefingReportBuilder.Build(new DailyBriefingInputs { Day = 7 });
            state.Enqueue(report);
            state.AcknowledgedDays.Add(1);
            var save = state.CaptureState();
            save.Checksum = SaveChecksum.Compute(save);
            string text = json.Serialize(save);
            var loaded = DailyBriefingSaveCodec.Decode(text, json);
            var fresh = new DailyBriefingState();
            fresh.RestoreState(loaded);
            Assert.Single(fresh.Pending);
            Assert.Equal(7, fresh.Pending[0].Day);
            Assert.Contains(1, fresh.AcknowledgedDays);
        }
    }
}
