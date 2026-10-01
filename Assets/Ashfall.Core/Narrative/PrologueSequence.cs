// SPDX-License-Identifier: MIT
// ============================================================================
// Prologue sequence — the authored diegetic opening beats shown on the
// Opening Protocol modal for the first days of a new campaign.
//
// Pure, engine-free authority. It owns no campaign state and no save section:
// the current day selects the beat, so a reload reproduces the same line. The
// host surfaces it through the existing day-goal seam (no second modal).
// ============================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Ashfall.Core.Narrative
{
    public sealed class PrologueBeatDef
    {
        /// <summary>Campaign day this beat opens (1-based; day 1 = first morning).</summary>
        public int day_index { get; set; }
        public string title { get; set; } = string.Empty;
        public string body { get; set; } = string.Empty;
    }

    public sealed class PrologueSequenceCatalog
    {
        public int schema_version { get; set; } = 1;
        public string prologue_id { get; set; } = string.Empty;
        public List<PrologueBeatDef> beats { get; set; } = new List<PrologueBeatDef>();
    }

    /// <summary>
    /// Validated, ordered prologue beats. <see cref="LoadFromJson"/> rejects a
    /// malformed catalog rather than silently inventing an opening line.
    /// </summary>
    public sealed class PrologueSequence
    {
        private readonly List<PrologueBeatDef> _beats;

        public string PrologueId { get; }
        public IReadOnlyList<PrologueBeatDef> Beats => _beats;
        public int BeatCount => _beats.Count;

        private PrologueSequence(string prologueId, List<PrologueBeatDef> beats)
        {
            PrologueId = prologueId;
            _beats = beats;
        }

        public static PrologueSequence? LoadFromJson(string? json)
        {
            if (string.IsNullOrWhiteSpace(json)) return null;
            try
            {
                var catalog = JsonSerializer.Deserialize<PrologueSequenceCatalog>(json);
                if (catalog?.beats == null || catalog.beats.Count == 0) return null;

                var seenDays = new HashSet<int>();
                var beats = new List<PrologueBeatDef>();
                foreach (var beat in catalog.beats)
                {
                    if (beat == null) return null;
                    if (beat.day_index <= 0) return null;
                    if (string.IsNullOrWhiteSpace(beat.title) || string.IsNullOrWhiteSpace(beat.body)) return null;
                    if (!seenDays.Add(beat.day_index)) return null; // duplicate day is ambiguous
                    beats.Add(new PrologueBeatDef
                    {
                        day_index = beat.day_index,
                        title = beat.title.Trim(),
                        body = beat.body.Trim()
                    });
                }

                beats.Sort((a, b) => a.day_index.CompareTo(b.day_index));
                return new PrologueSequence(catalog.prologue_id ?? string.Empty, beats);
            }
            catch (JsonException)
            {
                return null;
            }
        }

        /// <summary>
        /// The authored beat for a campaign day, or false when the prologue has
        /// run out (later days use the live briefing/slice goal instead).
        /// </summary>
        public bool TryGetBeatForDay(int day, out PrologueBeatDef? beat)
        {
            beat = _beats.FirstOrDefault(b => b.day_index == day);
            return beat != null;
        }
    }
}
