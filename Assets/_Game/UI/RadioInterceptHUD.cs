using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using AtomicWar._Game.Economy;

namespace AtomicWar._Game.UI
{
    /// <summary>
    /// Compact radio strip + expandable intercept log ([R]) with a frequency
    /// tuner that filters by channel tag ([ / ]). Push-driven from GameBootstrap.
    /// Optional <see cref="FactionRadioVoHook"/> plays channel-tag VO stubs.
    /// </summary>
    public class RadioInterceptHUD : MonoBehaviour
    {
        public const int MaxVisibleLinesCollapsed = 6;
        /// <summary>Keep in sync with FactionRadioInterceptSystem.MaxLogEntries.</summary>
        public const int MaxLogEntries = 24;

        /// <summary>
        /// Tuner presets: index 0 = all bands; then military / scavenger / prepper.
        /// </summary>
        public static readonly string[] TunerPresets =
        {
            "", // all
            "CH-7 MILBAND",
            "CH-3 ASH ROAD",
            "CH-11 STOCKPILE"
        };

        public static readonly string[] TunerLabels =
        {
            "ALL BANDS",
            "CH-7 MILBAND",
            "CH-3 ASH ROAD",
            "CH-11 STOCKPILE"
        };

        [SerializeField] private FactionRadioVoHook _voHook;
        [SerializeField] private FactionRadioVoLibrarySO _voLibrary;

        public bool IsOpen { get; private set; }
        public bool HasUnread { get; private set; }
        public int LineCount { get; private set; }
        public int FilteredLineCount { get; private set; }
        public int TunerIndex { get; private set; }
        public string ActiveChannelFilter { get; private set; } = string.Empty;
        public string LatestMessage { get; private set; } = string.Empty;
        public string LatestKind { get; private set; } = string.Empty;
        public string LatestChannelTag { get; private set; } = string.Empty;
        public string StatusLine { get; private set; } = "RADIO: —";
        public string TunerLine { get; private set; } = "TUNE: ALL BANDS  [ ]";
        public string DetailSummary { get; private set; } = "No intercepts.";

        private readonly List<Line> _lines = new List<Line>();

        public struct Line
        {
            public string Message;
            public string Kind;
            public string FactionId;
            public int Day;
            public string ChannelTag;
        }

        /// <summary>Newest-first full log (unfiltered).</summary>
        public IReadOnlyList<Line> Lines => _lines;

        public FactionRadioVoHook VoHook
        {
            get
            {
                if (_voHook == null)
                    _voHook = GetComponent<FactionRadioVoHook>()
                              ?? GetComponentInChildren<FactionRadioVoHook>()
                              ?? gameObject.AddComponent<FactionRadioVoHook>();
                if (_voLibrary != null)
                    _voHook.SetLibrary(_voLibrary);
                return _voHook;
            }
        }

        private void Awake()
        {
            // Ensure VO stubs exist when the strip is present on a HUD prefab.
            var vo = VoHook;
            vo.EnsureBuiltInStubs();
            Refresh();
        }

        /// <summary>
        /// Push a new intercept to the top of the strip. Called from bootstrap
        /// when FactionRadioInterceptSystem fires OnIntercept.
        /// </summary>
        public void Push(string message, string kind = "", string factionId = "", int day = 0)
        {
            if (string.IsNullOrEmpty(message)) return;
            string tag = DynamicEconomySystem.GetParleyChannelTag(factionId);
            _lines.Insert(0, new Line
            {
                Message = message,
                Kind = kind ?? string.Empty,
                FactionId = factionId ?? string.Empty,
                Day = day,
                ChannelTag = tag
            });
            while (_lines.Count > MaxLogEntries)
                _lines.RemoveAt(_lines.Count - 1);
            HasUnread = true;
            Refresh();

            // Play VO only if the tuner is on ALL or this channel.
            if (PassesFilter(tag))
                VoHook?.TryPlay(factionId, kind);
        }

        /// <summary>Replace the strip from a save restore / full log snapshot.</summary>
        public void SetLines(IReadOnlyList<Line> lines)
        {
            _lines.Clear();
            if (lines != null)
            {
                for (int i = 0; i < lines.Count && i < MaxLogEntries; i++)
                {
                    var l = lines[i];
                    if (string.IsNullOrEmpty(l.Message)) continue;
                    if (string.IsNullOrEmpty(l.ChannelTag) && !string.IsNullOrEmpty(l.FactionId))
                        l.ChannelTag = DynamicEconomySystem.GetParleyChannelTag(l.FactionId);
                    _lines.Add(l);
                }
            }
            Refresh();
        }

        /// <summary>
        /// Restore presentation state after load (open / unread / tuner index).
        /// Does not re-fire VO.
        /// </summary>
        public void ApplyUiState(bool isOpen, bool hasUnread, int tunerIndex = 0)
        {
            IsOpen = isOpen;
            HasUnread = hasUnread;
            SetTunerIndex(tunerIndex, playBlip: false);
            Refresh();
        }

        public void Clear()
        {
            _lines.Clear();
            HasUnread = false;
            Refresh();
        }

        public void Open()
        {
            IsOpen = true;
            HasUnread = false;
            Refresh();
        }

        public void Close()
        {
            IsOpen = false;
            Refresh();
        }

        public void Toggle()
        {
            if (IsOpen) Close();
            else Open();
        }

        public void MarkRead()
        {
            HasUnread = false;
            Refresh();
        }

        /// <summary>Cycle frequency filter forward ([ key).</summary>
        public void CycleTunerNext() => SetTunerIndex(TunerIndex + 1, playBlip: true);

        /// <summary>Cycle frequency filter backward (] is next; [ is prev — use both).</summary>
        public void CycleTunerPrev() => SetTunerIndex(TunerIndex - 1, playBlip: true);

        public void SetTunerIndex(int index, bool playBlip = false)
        {
            int n = TunerPresets.Length;
            if (n <= 0)
            {
                TunerIndex = 0;
                ActiveChannelFilter = string.Empty;
                Refresh();
                return;
            }
            // wrap
            int wrapped = ((index % n) + n) % n;
            TunerIndex = wrapped;
            ActiveChannelFilter = TunerPresets[wrapped] ?? string.Empty;
            Refresh();
            if (playBlip)
            {
                string tag = ActiveChannelFilter;
                if (string.IsNullOrEmpty(tag))
                    VoHook?.TryPlayChannel(string.Empty, "Succession");
                else
                    VoHook?.TryPlayChannel(tag, string.Empty);
            }
        }

        /// <summary>Tune to a specific channel tag (or empty for all).</summary>
        public bool TuneToChannel(string channelTag)
        {
            if (string.IsNullOrEmpty(channelTag))
            {
                SetTunerIndex(0, playBlip: true);
                return true;
            }
            for (int i = 0; i < TunerPresets.Length; i++)
            {
                if (string.Equals(TunerPresets[i], channelTag, StringComparison.OrdinalIgnoreCase))
                {
                    SetTunerIndex(i, playBlip: true);
                    return true;
                }
            }
            return false;
        }

        public bool PassesFilter(string channelTag)
        {
            if (string.IsNullOrEmpty(ActiveChannelFilter)) return true;
            return string.Equals(ActiveChannelFilter, channelTag, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>Filtered newest-first view for the active tuner preset.</summary>
        public List<Line> GetFilteredLines()
        {
            var result = new List<Line>();
            for (int i = 0; i < _lines.Count; i++)
            {
                var l = _lines[i];
                string ch = !string.IsNullOrEmpty(l.ChannelTag)
                    ? l.ChannelTag
                    : DynamicEconomySystem.GetParleyChannelTag(l.FactionId);
                if (PassesFilter(ch))
                    result.Add(l);
            }
            return result;
        }

        public void Refresh()
        {
            LineCount = _lines.Count;
            var filtered = GetFilteredLines();
            FilteredLineCount = filtered.Count;

            string tuneLabel = TunerIndex >= 0 && TunerIndex < TunerLabels.Length
                ? TunerLabels[TunerIndex]
                : "ALL BANDS";
            TunerLine = $"TUNE: {tuneLabel}  [ / ] cycle";

            if (filtered.Count == 0)
            {
                LatestMessage = string.Empty;
                LatestKind = string.Empty;
                LatestChannelTag = string.Empty;
                StatusLine = IsOpen
                    ? $"RADIO [OPEN] · {tuneLabel}  quiet  [R] close"
                    : $"RADIO · {tuneLabel}  quiet  [R] log";
                var empty = new StringBuilder();
                empty.AppendLine(StatusLine);
                empty.AppendLine(TunerLine);
                if (LineCount > 0 && FilteredLineCount == 0)
                    empty.AppendLine("No intercepts on this frequency. [ / ] retune.");
                else
                    empty.AppendLine("No intercepts on the band.");
                if (IsOpen)
                    empty.AppendLine("[R] close log");
                DetailSummary = empty.ToString().TrimEnd();
                return;
            }

            var top = filtered[0];
            LatestMessage = top.Message ?? string.Empty;
            LatestKind = top.Kind ?? string.Empty;
            LatestChannelTag = !string.IsNullOrEmpty(top.ChannelTag)
                ? top.ChannelTag
                : DynamicEconomySystem.GetParleyChannelTag(top.FactionId);

            string tag = KindTag(LatestKind);
            string unread = HasUnread ? " · NEW" : string.Empty;
            string openMark = IsOpen ? " [OPEN]" : "";
            string shortMsg = LatestMessage.Length > 56
                ? LatestMessage.Substring(0, 53) + "…"
                : LatestMessage;
            StatusLine = $"RADIO{openMark} [{tag}]{unread}  {tuneLabel}  {shortMsg}";
            if (!IsOpen)
                StatusLine += "  [R]";

            int maxLines = IsOpen ? MaxLogEntries : MaxVisibleLinesCollapsed;
            var sb = new StringBuilder();
            sb.AppendLine(StatusLine);
            sb.AppendLine(TunerLine);
            if (IsOpen)
                sb.AppendLine("--- intercept log (newest first) ---");
            int shown = 0;
            for (int i = 0; i < filtered.Count && shown < maxLines; i++)
            {
                var l = filtered[i];
                string day = l.Day > 0 ? $"D{l.Day} " : "";
                string ch = !string.IsNullOrEmpty(l.ChannelTag)
                    ? l.ChannelTag
                    : DynamicEconomySystem.GetParleyChannelTag(l.FactionId);
                sb.AppendLine($"  · {day}[{KindTag(l.Kind)}] [{ch}] {l.Message}");
                shown++;
            }
            if (!IsOpen && filtered.Count > MaxVisibleLinesCollapsed)
                sb.AppendLine($"  … +{filtered.Count - MaxVisibleLinesCollapsed} older  [R] expand");
            if (IsOpen)
                sb.AppendLine("[R] close · [ / ] retune");
            DetailSummary = sb.ToString().TrimEnd();
        }

        private static string KindTag(string kind)
        {
            if (string.IsNullOrEmpty(kind)) return "—";
            switch (kind)
            {
                case "Succession": return "BAND";
                case "Surrender": return "STAND";
                case "Parley": return "PARLEY";
                case "HatchRepel": return "HATCH";
                default: return kind.Length <= 6 ? kind.ToUpperInvariant() : kind.Substring(0, 6).ToUpperInvariant();
            }
        }
    }
}
