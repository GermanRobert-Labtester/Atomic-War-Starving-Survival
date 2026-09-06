using System;
using System.Collections.Generic;

namespace Ashfall.Core.Campaign
{
    /// <summary>
    /// Evaluates candidate dawn facts against persisted cadence state to suppress spam
    /// and unchanged warnings while re-alerting on critical threats or significant delta.
    /// </summary>
    public static class DailyBriefingCadenceFilter
    {
        public const float WarningSeverityThresholdDelta = 15f;
        public const float CriticalSeverityThresholdDelta = 10f;
        public const int CriticalRepeatCadenceDays = 3;

        /// <summary>
        /// Filters facts according to the cadence rules and updates history in-place.
        /// </summary>
        public static List<BriefingFact> FilterAndAdvance(
            IEnumerable<BriefingFact> facts,
            IDictionary<string, DailyBriefingCadenceRecord> cadenceRecords,
            int currentDay)
        {
            if (facts == null) return new List<BriefingFact>();
            if (cadenceRecords == null) throw new ArgumentNullException(nameof(cadenceRecords));

            var passed = new List<BriefingFact>();

            foreach (var fact in facts)
            {
                if (fact == null || string.IsNullOrEmpty(fact.FactId)) continue;

                if (!cadenceRecords.TryGetValue(fact.FactId, out var record))
                {
                    // Brand new fact: always emit and record
                    cadenceRecords[fact.FactId] = new DailyBriefingCadenceRecord(fact.FactId, fact.Severity, currentDay);
                    passed.Add(fact);
                    continue;
                }

                bool shouldEmit = false;

                switch (fact.Taxonomy)
                {
                    case BriefingTaxonomyClass.Critical:
                        // Critical facts repeat every 3 days if unresolved or if severity moved by >= 10
                        if (currentDay - record.LastReportedDay >= CriticalRepeatCadenceDays ||
                            Math.Abs(fact.Severity - record.LastReportedSeverity) >= CriticalSeverityThresholdDelta)
                        {
                            shouldEmit = true;
                        }
                        break;

                    case BriefingTaxonomyClass.Warning:
                        // Warnings emit only if severity changed by >= 15 or condition resolved
                        if (Math.Abs(fact.Severity - record.LastReportedSeverity) >= WarningSeverityThresholdDelta ||
                            (record.LastReportedSeverity > 0f && fact.Severity <= 0f))
                        {
                            shouldEmit = true;
                        }
                        break;

                    case BriefingTaxonomyClass.Intel:
                    case BriefingTaxonomyClass.Flavor:
                        // Intel and flavor emit only once per distinct triggering day
                        if (record.LastReportedDay < currentDay)
                        {
                            shouldEmit = true;
                        }
                        break;
                }

                if (shouldEmit)
                {
                    record.LastReportedSeverity = fact.Severity;
                    record.LastReportedDay = currentDay;
                    passed.Add(fact);
                }
            }

            return passed;
        }
    }
}
