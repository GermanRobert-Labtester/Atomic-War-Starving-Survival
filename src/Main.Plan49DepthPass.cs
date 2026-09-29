// SPDX-License-Identifier: MIT
// ASHFALL Plan 49 (C1[16]) — depth passes: the four remaining orphaned
// authored catalogs are bound through their existing Core loaders into
// constructed owners at campaign setup. Authored data, no mutable state, no
// save section; re-derived from the data authority on every setup.

using System.Collections.Generic;
using Ashfall.Core.Campaign;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private Plan49DepthPassHostSession? _depthPass;

        public Plan49DepthPassHostSession? DepthPass => _depthPass;

        public void SetupPlan49DepthPass()
        {
            if (_depthPass != null) return;
            _depthPass = Plan49DepthPassHostSession.Create(_dataDir);
        }

        /// <summary>
        /// Plan 49 depth pass — an authored audio log reaches the player as a journal
        /// entry on the day it was recorded (the journal is the display surface and
        /// dedupes by key, so this is idempotent). The recorded text is never altered;
        /// the optional listening note follows it as a separate line.
        /// </summary>
        private void JournalAudioLogsForDay(int day)
        {
            SetupPlan49DepthPass();
            if (_depthPass == null || _journal == null) return;
            foreach (var log in _depthPass.GetAudioLogsForDay(day))
            {
                string text = $"{log.title}\n{log.bodyText}";
                if (!string.IsNullOrWhiteSpace(log.listening_note))
                    text += $"\n({log.listening_note})";
                _journal.TryAddRawEntry("audio_log_" + log.id, text, null!, day);
            }
        }

        private sealed class AudioLogsDayOwner : IDayAdvanceOwner
        {
            private readonly Main _m;
            public AudioLogsDayOwner(Main m) => _m = m;
            public void CapturePreDaySnapshot(int day) { }
            public void TickDay(int day, List<DayStateChangeEvent> events) => _m.JournalAudioLogsForDay(day);
        }

        public void ResetPlan49DepthPass()
        {
            _depthPass = null;
        }
    }
}
