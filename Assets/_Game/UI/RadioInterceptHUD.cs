using System.Collections.Generic;
using System.Text;
using UnityEngine;
using AtomicWar._Game.Economy;

namespace AtomicWar._Game.UI
{
    /// <summary>
    /// Compact radio strip + expandable intercept log ([R]). Newest faction
    /// intercept lines (succession, hatch bounce, parley, surrender).
    /// Push-driven from GameBootstrap so UI stays free of a Core assembly ref.
    /// Optional <see cref="FactionRadioVoHook"/> plays channel-tag VO when clips exist.
    /// </summary>
    public class RadioInterceptHUD : MonoBehaviour
    {
        public const int MaxVisibleLinesCollapsed = 6;
        /// <summary>Keep in sync with FactionRadioInterceptSystem.MaxLogEntries.</summary>
        public const int MaxLogEntries = 24;

        [SerializeField] private FactionRadioVoHook _voHook;

        public bool IsOpen { get; private set; }
        public bool HasUnread { get; private set; }
        public int LineCount { get; private set; }
        public string LatestMessage { get; private set; } = string.Empty;
        public string LatestKind { get; private set; } = string.Empty;
        public string LatestChannelTag { get; private set; } = string.Empty;
        public string StatusLine { get; private set; } = "RADIO: —";
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

        /// <summary>Newest-first snapshot for tests / expanded log.</summary>
        public IReadOnlyList<Line> Lines => _lines;

        public FactionRadioVoHook VoHook
        {
            get
            {
                if (_voHook == null)
                    _voHook = GetComponent<FactionRadioVoHook>()
                              ?? GetComponentInChildren<FactionRadioVoHook>()
                              ?? gameObject.AddComponent<FactionRadioVoHook>();
                return _voHook;
            }
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

            // Diegetic VO when clips are assigned; no-op without assets.
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
                    {
                        l.ChannelTag = DynamicEconomySystem.GetParleyChannelTag(l.FactionId);
                    }
                    _lines.Add(l);
                }
            }
            // Restore is not "new" traffic — leave unread false unless empty→nonempty
            // was already unread; load should not flash NEW after a quiet restore.
            HasUnread = false;
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

        /// <summary>Mark the strip as seen without opening the expanded log.</summary>
        public void MarkRead()
        {
            HasUnread = false;
            Refresh();
        }

        public void Refresh()
        {
            LineCount = _lines.Count;
            if (_lines.Count == 0)
            {
                LatestMessage = string.Empty;
                LatestKind = string.Empty;
                LatestChannelTag = string.Empty;
                StatusLine = IsOpen
                    ? "RADIO [OPEN] quiet  [R] close"
                    : "RADIO: quiet  [R] log";
                DetailSummary = IsOpen
                    ? "No intercepts on the band.\n[R] close"
                    : "No intercepts on the band.";
                return;
            }

            var top = _lines[0];
            LatestMessage = top.Message ?? string.Empty;
            LatestKind = top.Kind ?? string.Empty;
            LatestChannelTag = !string.IsNullOrEmpty(top.ChannelTag)
                ? top.ChannelTag
                : DynamicEconomySystem.GetParleyChannelTag(top.FactionId);

            string tag = KindTag(LatestKind);
            string unread = HasUnread ? " · NEW" : string.Empty;
            string openMark = IsOpen ? " [OPEN]" : "";
            string shortMsg = LatestMessage.Length > 64
                ? LatestMessage.Substring(0, 61) + "…"
                : LatestMessage;
            StatusLine = $"RADIO{openMark} [{tag}]{unread}  {LatestChannelTag}  {shortMsg}";
            if (!IsOpen)
                StatusLine += "  [R]";

            int maxLines = IsOpen ? MaxLogEntries : MaxVisibleLinesCollapsed;
            var sb = new StringBuilder();
            sb.AppendLine(StatusLine);
            if (IsOpen)
                sb.AppendLine("--- intercept log (newest first) ---");
            int shown = 0;
            for (int i = 0; i < _lines.Count && shown < maxLines; i++)
            {
                var l = _lines[i];
                string day = l.Day > 0 ? $"D{l.Day} " : "";
                string ch = !string.IsNullOrEmpty(l.ChannelTag)
                    ? l.ChannelTag
                    : DynamicEconomySystem.GetParleyChannelTag(l.FactionId);
                sb.AppendLine($"  · {day}[{KindTag(l.Kind)}] [{ch}] {l.Message}");
                shown++;
            }
            if (!IsOpen && _lines.Count > MaxVisibleLinesCollapsed)
                sb.AppendLine($"  … +{_lines.Count - MaxVisibleLinesCollapsed} older  [R] expand");
            if (IsOpen)
                sb.AppendLine("[R] close log");
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
