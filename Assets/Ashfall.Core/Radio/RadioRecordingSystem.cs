// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Radio
{
    /// <summary>
    /// Contract and runtime manager for recording radio broadcasts to magnetic cassette tapes.
    /// Invariant: Pure C#, zero engine references. Replaying a tape NEVER re-triggers one-shot
    /// world consequences, evidence duplication, or mission spawns.
    /// </summary>
    public sealed class RadioRecordingSystem
    {
        public const string BlankTapeItemId = "item_blank_magnetic_tape";

        private readonly Dictionary<string, RecordedCassetteEntry> _recordedTapes =
            new Dictionary<string, RecordedCassetteEntry>(StringComparer.OrdinalIgnoreCase);

        public event Action<RecordedCassetteEntry>? OnBroadcastRecorded;

        public IReadOnlyCollection<RecordedCassetteEntry> RecordedTapes => _recordedTapes.Values;

        /// <summary>
        /// Record an active broadcast onto a blank cassette tape.
        /// </summary>
        public RecordedCassetteEntry? RecordBroadcast(ScheduledBroadcastResult broadcast, int day)
        {
            if (broadcast == null || !broadcast.HasTransmission || broadcast.IsSilence)
                return null;

            string tapeId = $"cassette_rec_{broadcast.FrequencyMhz:000.0}_{day}_{_recordedTapes.Count + 1}";
            var entry = new RecordedCassetteEntry
            {
                cassetteId = tapeId,
                broadcastId = string.IsNullOrEmpty(broadcast.BroadcastId) ? $"bcast_{broadcast.FrequencyMhz:000.0}_{day}" : broadcast.BroadcastId,
                title = string.IsNullOrEmpty(broadcast.Headline) ? $"Recorded Signal {broadcast.FrequencyMhz:0.0} MHz" : broadcast.Headline,
                transcript = broadcast.Message,
                recordedDay = day,
                frequencyMhz = broadcast.FrequencyMhz,
                sourceName = broadcast.SourceName,
                audioCue = broadcast.AudioCue
            };

            _recordedTapes[tapeId] = entry;
            OnBroadcastRecorded?.Invoke(entry);
            return entry;
        }

        /// <summary>
        /// Replay a recorded cassette tape safely.
        /// Guaranteed: Read-only presentation, zero mutation to live campaign state.
        /// </summary>
        public RecordedCassetteEntry? ReplayCassette(string cassetteId)
        {
            if (string.IsNullOrEmpty(cassetteId)) return null;
            return _recordedTapes.TryGetValue(cassetteId, out var entry) ? entry : null;
        }

        /// <summary>
        /// Determine barter value of a recorded tape. High value for verified intelligence; zero for routine noise.
        /// </summary>
        public int CalculateTradeValue(string cassetteId)
        {
            if (string.IsNullOrEmpty(cassetteId) || !_recordedTapes.TryGetValue(cassetteId, out var entry))
                return 0;

            // Intelligence and wiretaps carry substantial barter value
            if (entry.title.Contains("Wiretap", StringComparison.OrdinalIgnoreCase) ||
                entry.title.Contains("Mutiny", StringComparison.OrdinalIgnoreCase) ||
                entry.title.Contains("Cipher", StringComparison.OrdinalIgnoreCase) ||
                entry.title.Contains("Reckoning", StringComparison.OrdinalIgnoreCase))
            {
                return 25; // 25 barter tokens / ammo
            }

            if (entry.title.Contains("Emergency", StringComparison.OrdinalIgnoreCase) ||
                entry.title.Contains("Advisory", StringComparison.OrdinalIgnoreCase))
            {
                return 5;
            }

            // Routine chatter has minimal value
            return 1;
        }

        // ── Save / Load ─────────────────────────────────────────────────────────

        public List<RecordedCassetteEntry> CaptureState()
        {
            var list = new List<RecordedCassetteEntry>(_recordedTapes.Values);
            list.Sort((a, b) => string.Compare(a.cassetteId, b.cassetteId, StringComparison.Ordinal));
            return list;
        }

        public void RestoreState(List<RecordedCassetteEntry>? savedEntries)
        {
            _recordedTapes.Clear();
            if (savedEntries == null) return;
            foreach (var entry in savedEntries)
            {
                if (entry == null || string.IsNullOrEmpty(entry.cassetteId)) continue;
                _recordedTapes[entry.cassetteId] = entry;
            }
        }
    }
}
