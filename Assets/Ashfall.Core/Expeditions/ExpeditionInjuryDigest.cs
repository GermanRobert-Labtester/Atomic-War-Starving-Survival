// SPDX-License-Identifier: MIT
// P108 follow-up — pure read model for the StatusPanel expedition-injury chip.
//
// One place owns the "who is still hurt from a sortie" rule: living survivors
// only, latest event wins. No state, no save section; the panel renders it.
using System;
using System.Collections.Generic;
using Ashfall.Core.Medical;

namespace Ashfall.Core.Expeditions
{
    /// <summary>One living survivor's most recent expedition injury.</summary>
    public readonly struct ExpeditionInjuryRow
    {
        public string SurvivorId { get; }
        public int Day { get; }
        public string Description { get; }

        public ExpeditionInjuryRow(string survivorId, int day, string description)
        {
            SurvivorId = survivorId ?? string.Empty;
            Day = day;
            Description = description ?? string.Empty;
        }
    }

    /// <summary>Pure projection of health-history events into injury chips.</summary>
    public static class ExpeditionInjuryDigest
    {
        /// <summary>Event-type prefix written by Main.OnExpeditionFailed.</summary>
        public const string EventTypePrefix = "expedition_failure";

        /// <summary>
        /// Latest expedition injury per <b>living</b> roster member, in roster
        /// order. Dead survivors are excluded (their chip is the memorial, not
        /// a health row). <paramref name="latestFor"/> is the health-history
        /// lookup; a null result skips that survivor.
        /// </summary>
        public static List<ExpeditionInjuryRow> Build(
            IEnumerable<(string Id, bool Alive)> roster,
            Func<string, HealthEvent?> latestFor)
        {
            var rows = new List<ExpeditionInjuryRow>();
            if (roster == null || latestFor == null) return rows;

            foreach (var member in roster)
            {
                if (string.IsNullOrEmpty(member.Id) || !member.Alive) continue;
                HealthEvent? e = latestFor(member.Id);
                if (e == null) continue;
                rows.Add(new ExpeditionInjuryRow(member.Id, e.EventDay, e.Description));
            }
            return rows;
        }
    }
}
