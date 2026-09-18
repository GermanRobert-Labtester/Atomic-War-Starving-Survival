// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Campaign
{
    /// <summary>
    /// Plan 31C — machine-readable record of one simulated day, built from the
    /// same owner reports / event stream the briefing consumes. Engine-free,
    /// deterministic, no wall-clock timestamps (durations only).
    /// </summary>
    [Serializable]
    public sealed class DayRecord
    {
        public int schemaVersion = DayRecordBuilder.CurrentSchemaVersion;
        public string sessionId = string.Empty;
        public long seed;
        public int day;
        public string[] ownerOrder = Array.Empty<string>();
        public List<DayRecordOwner> owners = new List<DayRecordOwner>();
        public List<DayRecordEvent> events = new List<DayRecordEvent>();
    }

    [Serializable]
    public sealed class DayRecordOwner
    {
        public string ownerId = string.Empty;
        public double durationMs;
        public bool failed;
        public string? failureCode;
    }

    [Serializable]
    public sealed class DayRecordEvent
    {
        public string kind = string.Empty;
        public string sourceOwnerId = string.Empty;
        public string primaryId = string.Empty;
        public string secondaryId = string.Empty;
        public float numeric;
        public string causeId = string.Empty;
        public string actorId = string.Empty;
    }

    /// <summary>Pure builder for <see cref="DayRecord"/>.</summary>
    public static class DayRecordBuilder
    {
        public const int CurrentSchemaVersion = 1;

        /// <summary>Assemble a day record from the coordinator's day-advance
        /// result. Owner order is the actual execution order (31C.7);
        /// per-owner duration is observational (31C.8).</summary>
        public static DayRecord FromDay(long seed, string? sessionId, int day, DayAdvancedEventArgs? args)
        {
            var record = new DayRecord
            {
                sessionId = sessionId ?? string.Empty,
                seed = seed,
                day = day
            };
            var reports = args?.OwnerReports;
            if (reports == null) return record;

            var order = new string[reports.Length];
            for (int i = 0; i < reports.Length; i++)
            {
                var r = reports[i];
                if (r == null) { order[i] = string.Empty; continue; }
                order[i] = r.OwnerId;
                record.owners.Add(new DayRecordOwner
                {
                    ownerId = r.OwnerId,
                    durationMs = r.DurationMs,
                    failed = !r.Succeeded,
                    failureCode = r.Succeeded || string.IsNullOrEmpty(r.FailureMessage) ? null : r.FailureMessage
                });

                if (r.Events == null) continue;
                for (int e = 0; e < r.Events.Length; e++)
                {
                    var evt = r.Events[e];
                    if (evt == null) continue;
                    record.events.Add(new DayRecordEvent
                    {
                        kind = evt.Kind,
                        sourceOwnerId = evt.SourceOwnerId,
                        primaryId = evt.PrimaryId,
                        secondaryId = evt.SecondaryId,
                        numeric = evt.Numeric
                    });
                }
            }

            record.ownerOrder = order;
            return record;
        }

        /// <summary>One JSONL line (append-safe, grep-friendly). Uses the
        /// project serializer (IncludeFields, compact).</summary>
        public static string ToJsonLine(DayRecord record) =>
            new SystemTextJsonSerializer().Serialize(record);
    }
}
