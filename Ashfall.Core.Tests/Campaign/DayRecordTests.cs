// SPDX-License-Identifier: MIT
using System.Collections.Generic;
using System.Text.Json;
using Ashfall.Core.Campaign;
using Xunit;

namespace Ashfall.Core.Tests.Campaign
{
    /// <summary>
    /// Plan 31C — the day record is built from the same owner reports / events
    /// the briefing consumes, preserves execution order and failures, and
    /// serializes with a pinned schema version. No wall-clock, no simulation
    /// coupling.
    /// </summary>
    public sealed class DayRecordTests
    {
        private static DayAdvancedEventArgs Args()
        {
            var reports = new List<DayOwnerReport>
            {
                new DayOwnerReport("power", true,
                    new List<DayStateChangeEvent>
                    {
                        new DayStateChangeEvent("power_shed_automatic", "power", "room_ward", "shed", 0f)
                    }, string.Empty) { DurationMs = 0.42 },
                new DayOwnerReport("radio", false,
                    new List<DayStateChangeEvent>(), "owner exploded") { DurationMs = 1.5 }
            };
            return new DayAdvancedEventArgs(7, reports);
        }

        [Fact]
        public void FromDay_CapturesOrder_Events_AndFailures()
        {
            var record = DayRecordBuilder.FromDay(12345, "slot-1", 7, Args());

            Assert.Equal(DayRecordBuilder.CurrentSchemaVersion, record.schemaVersion);
            Assert.Equal(12345, record.seed);
            Assert.Equal("slot-1", record.sessionId);
            Assert.Equal(7, record.day);

            Assert.Equal(new[] { "power", "radio" }, record.ownerOrder);

            Assert.Equal("power", record.owners[0].ownerId);
            Assert.Equal(0.42, record.owners[0].durationMs, 2);
            Assert.False(record.owners[0].failed);
            Assert.Null(record.owners[0].failureCode);

            Assert.True(record.owners[1].failed);
            Assert.Equal("owner exploded", record.owners[1].failureCode);

            var evt = Assert.Single(record.events);
            Assert.Equal("power_shed_automatic", evt.kind);
            Assert.Equal("power", evt.sourceOwnerId);
            Assert.Equal("room_ward", evt.primaryId);
            Assert.Equal("shed", evt.secondaryId);
        }

        [Fact]
        public void FromDay_NullArgs_IsEmptyButVersioned()
        {
            var record = DayRecordBuilder.FromDay(1, null, 0, null);
            Assert.Equal(DayRecordBuilder.CurrentSchemaVersion, record.schemaVersion);
            Assert.Empty(record.owners);
            Assert.Empty(record.events);
        }

        [Fact]
        public void ToJsonLine_PreservesSchemaFieldNames_AndRoundTrips()
        {
            var record = DayRecordBuilder.FromDay(9, "s", 3, Args());
            string line = DayRecordBuilder.ToJsonLine(record);

            Assert.DoesNotContain('\n', line); // one JSONL line
            using var doc = JsonDocument.Parse(line);
            var root = doc.RootElement;
            Assert.Equal(1, root.GetProperty("schemaVersion").GetInt32());
            Assert.True(root.TryGetProperty("ownerOrder", out _));
            Assert.True(root.TryGetProperty("owners", out _));
            Assert.True(root.TryGetProperty("events", out _));
            Assert.Equal(2, root.GetProperty("owners").GetArrayLength());
        }

        [Fact]
        public void SchemaFieldSet_IsPinned()
        {
            // A field-set change must be a deliberate schema-version bump.
            var props = typeof(DayRecord).GetFields();
            var names = new List<string>();
            foreach (var p in props) names.Add(p.Name);
            Assert.Contains("schemaVersion", names);
            Assert.Contains("sessionId", names);
            Assert.Contains("seed", names);
            Assert.Contains("day", names);
            Assert.Contains("ownerOrder", names);
            Assert.Contains("owners", names);
            Assert.Contains("events", names);
        }
    }
}