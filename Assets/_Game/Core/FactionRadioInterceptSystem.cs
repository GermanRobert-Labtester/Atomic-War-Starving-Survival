using System;
using System.Collections.Generic;
using AtomicWar._Game.Economy;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Diegetic faction radio traffic: succession banners, parley stand-downs,
    /// and hatch-bounce chatter. Surfaces as short intercept lines for the
    /// radio / log UI without requiring a tuned frequency.
    /// </summary>
    public class FactionRadioInterceptSystem
    {
        public const int MaxLogEntries = 24;

        public enum InterceptKind
        {
            Succession,
            Surrender,
            Parley,
            HatchRepel
        }

        [Serializable]
        public class InterceptEntry
        {
            public string Id;
            public string FactionId;
            public string Kind;
            public string Message;
            public int Day;
        }

        private readonly List<InterceptEntry> _log = new List<InterceptEntry>();
        private DynamicEconomySystem _economy;
        private Func<int> _getDay;
        private int _seq;

        /// <summary>Newest-first intercept log (capped).</summary>
        public IReadOnlyList<InterceptEntry> Log => _log;

        /// <summary>Most recent intercept message, or empty.</summary>
        public string LastInterceptMessage =>
            _log.Count > 0 ? (_log[0].Message ?? string.Empty) : string.Empty;

        public event Action<InterceptEntry> OnIntercept;

        public void Bind(DynamicEconomySystem economy, Func<int> getDay = null)
        {
            Unbind();
            _economy = economy;
            _getDay = getDay ?? (() => 0);
            if (_economy == null) return;

            _economy.OnFactionSuccession += HandleSuccession;
            _economy.OnFactionSurrender += HandleSurrender;
            _economy.OnRaidResolved += HandleRaid;
        }

        public void Unbind()
        {
            if (_economy == null) return;
            _economy.OnFactionSuccession -= HandleSuccession;
            _economy.OnFactionSurrender -= HandleSurrender;
            _economy.OnRaidResolved -= HandleRaid;
            _economy = null;
        }

        /// <summary>Manual push (tests / scripted beats).</summary>
        public InterceptEntry Push(string factionId, InterceptKind kind, string message, int day = -1)
        {
            if (string.IsNullOrEmpty(message)) return null;
            int d = day >= 0 ? day : (_getDay != null ? _getDay() : 0);
            var entry = new InterceptEntry
            {
                Id = $"intercept_{++_seq}_{kind.ToString().ToLowerInvariant()}",
                FactionId = factionId ?? string.Empty,
                Kind = kind.ToString(),
                Message = message,
                Day = d
            };
            _log.Insert(0, entry);
            while (_log.Count > MaxLogEntries)
                _log.RemoveAt(_log.Count - 1);
            OnIntercept?.Invoke(entry);
            return entry;
        }

        /// <summary>Presentation: expanded intercept log open (HUD).</summary>
        public bool HudIsOpen { get; set; }
        /// <summary>Presentation: unread badge on the radio strip (HUD).</summary>
        public bool HudHasUnread { get; set; }
        /// <summary>Presentation: frequency tuner index (0 = all bands).</summary>
        public int HudTunerIndex { get; set; }

        public FactionRadioInterceptSave CaptureState()
        {
            return new FactionRadioInterceptSave
            {
                Entries = _log.ToArray(),
                NextSeq = _seq,
                HudIsOpen = HudIsOpen,
                HudHasUnread = HudHasUnread,
                HudTunerIndex = HudTunerIndex
            };
        }

        /// <summary>
        /// Capture log + optional live HUD presentation (open / unread / tuner).
        /// </summary>
        public FactionRadioInterceptSave CaptureState(
            bool hudIsOpen,
            bool hudHasUnread,
            int hudTunerIndex)
        {
            HudIsOpen = hudIsOpen;
            HudHasUnread = hudHasUnread;
            HudTunerIndex = Math.Max(0, hudTunerIndex);
            return CaptureState();
        }

        public void RestoreState(FactionRadioInterceptSave save)
        {
            _log.Clear();
            _seq = 0;
            HudIsOpen = false;
            HudHasUnread = false;
            HudTunerIndex = 0;
            if (save == null) return;
            _seq = Math.Max(0, save.NextSeq);
            HudIsOpen = save.HudIsOpen;
            HudHasUnread = save.HudHasUnread;
            HudTunerIndex = Math.Max(0, save.HudTunerIndex);
            if (save.Entries == null) return;
            // Restore newest-first order as stored
            for (int i = 0; i < save.Entries.Length && i < MaxLogEntries; i++)
            {
                var e = save.Entries[i];
                if (e == null || string.IsNullOrEmpty(e.Message)) continue;
                _log.Add(e);
            }
        }

        public void Clear()
        {
            _log.Clear();
            _seq = 0;
            HudIsOpen = false;
            HudHasUnread = false;
            HudTunerIndex = 0;
        }

        private void HandleSuccession(FactionSuccessionResult result)
        {
            if (result == null || !result.Applied) return;
            string leader = string.IsNullOrEmpty(result.NewLeader) ? "Someone new" : result.NewLeader;
            string prev = string.IsNullOrEmpty(result.PreviousLeader) ? "the old voice" : result.PreviousLeader;
            string fac = DisplayName(result.FactionId);
            Push(result.FactionId, InterceptKind.Succession,
                $"Band shift. {prev} drops out. {leader} claims the {fac} banner — cold, short, no music.");
        }

        private void HandleSurrender(FactionSurrenderResult result)
        {
            if (result == null || !result.Applied) return;
            string leader = _economy != null
                ? _economy.GetLeaderName(result.FactionId)
                : "Their lead";
            if (string.IsNullOrEmpty(leader)) leader = "Their lead";
            string fac = DisplayName(result.FactionId);
            var kind = result.Auto ? InterceptKind.Surrender : InterceptKind.Parley;
            string how = result.Auto
                ? $"{leader} stops hammering the hatch. {fac} raid traffic goes quiet."
                : $"Parley on the air. {leader} of {fac} calls the raid off. Static, then nothing.";
            Push(result.FactionId, kind, how);
        }

        private void HandleRaid(FactionRaidResult result)
        {
            if (result == null || !result.Launched || !result.Repelled) return;
            // Auto-surrender already emits a surrender intercept; skip double-talk.
            if (result.SurrenderedAfter) return;
            string leader = _economy != null
                ? _economy.GetLeaderName(result.FactionId)
                : "They";
            if (string.IsNullOrEmpty(leader)) leader = "They";
            string fac = DisplayName(result.FactionId);
            Push(result.FactionId, InterceptKind.HatchRepel,
                $"Hatch bounce. {leader} on the {fac} band — short curses, then dead air. Parley window may open.");
        }

        private string DisplayName(string factionId)
        {
            if (_economy == null || string.IsNullOrEmpty(factionId)) return "the camp";
            var fac = _economy.GetFaction(factionId);
            return fac != null && !string.IsNullOrEmpty(fac.displayName) ? fac.displayName : factionId;
        }
    }

    [Serializable]
    public class FactionRadioInterceptSave
    {
        public FactionRadioInterceptSystem.InterceptEntry[] Entries;
        public int NextSeq;
        /// <summary>Expanded intercept log was open when saved.</summary>
        public bool HudIsOpen;
        /// <summary>Unread NEW badge on the radio strip.</summary>
        public bool HudHasUnread;
        /// <summary>Frequency tuner preset index (0 = all bands).</summary>
        public int HudTunerIndex;
    }
}
