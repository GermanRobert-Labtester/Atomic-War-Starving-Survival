using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AtomicWar._Game.UI
{
    /// <summary>
    /// Compact radio strip: newest faction intercept lines (succession, hatch
    /// bounce, parley, surrender). Push-driven from GameBootstrap so UI stays
    /// free of a Core assembly reference.
    /// </summary>
    public class RadioInterceptHUD : MonoBehaviour
    {
        public const int MaxVisibleLines = 6;
        /// <summary>Keep in sync with FactionRadioInterceptSystem.MaxLogEntries.</summary>
        public const int MaxLogEntries = 24;

        public bool IsOpen { get; private set; }
        public bool HasUnread { get; private set; }
        public int LineCount { get; private set; }
        public string LatestMessage { get; private set; } = string.Empty;
        public string LatestKind { get; private set; } = string.Empty;
        public string StatusLine { get; private set; } = "RADIO: —";
        public string DetailSummary { get; private set; } = "No intercepts.";

        private readonly List<Line> _lines = new List<Line>();

        public struct Line
        {
            public string Message;
            public string Kind;
            public string FactionId;
            public int Day;
        }

        /// <summary>Newest-first snapshot for tests / expanded log.</summary>
        public IReadOnlyList<Line> Lines => _lines;

        /// <summary>
        /// Push a new intercept to the top of the strip. Called from bootstrap
        /// when FactionRadioInterceptSystem fires OnIntercept.
        /// </summary>
        public void Push(string message, string kind = "", string factionId = "", int day = 0)
        {
            if (string.IsNullOrEmpty(message)) return;
            _lines.Insert(0, new Line
            {
                Message = message,
                Kind = kind ?? string.Empty,
                FactionId = factionId ?? string.Empty,
                Day = day
            });
            while (_lines.Count > MaxLogEntries)
                _lines.RemoveAt(_lines.Count - 1);
            HasUnread = true;
            Refresh();
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
                    _lines.Add(l);
                }
            }
            HasUnread = _lines.Count > 0;
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

        public void Close() => IsOpen = false;

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
                StatusLine = "RADIO: quiet";
                DetailSummary = "No intercepts on the band.";
                return;
            }

            var top = _lines[0];
            LatestMessage = top.Message ?? string.Empty;
            LatestKind = top.Kind ?? string.Empty;

            string tag = KindTag(LatestKind);
            string unread = HasUnread ? " · NEW" : string.Empty;
            string shortMsg = LatestMessage.Length > 72
                ? LatestMessage.Substring(0, 69) + "…"
                : LatestMessage;
            StatusLine = $"RADIO [{tag}]{unread}  {shortMsg}";

            var sb = new StringBuilder();
            sb.AppendLine(StatusLine);
            int shown = 0;
            for (int i = 0; i < _lines.Count && shown < MaxVisibleLines; i++)
            {
                var l = _lines[i];
                string day = l.Day > 0 ? $"D{l.Day} " : "";
                sb.AppendLine($"  · {day}[{KindTag(l.Kind)}] {l.Message}");
                shown++;
            }
            if (_lines.Count > MaxVisibleLines)
                sb.AppendLine($"  … +{_lines.Count - MaxVisibleLines} older");
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
