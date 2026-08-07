using System;
using System.Collections.Generic;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Ring buffer of expedition encounter / combat feedback lines for the HUD.
    /// Not save-backed (ephemeral session log); Core raises events for UI.
    /// </summary>
    public class ExpeditionEncounterLog
    {
        public const int DefaultCapacity = 32;

        private readonly List<string> _lines = new List<string>(DefaultCapacity);
        private readonly int _capacity;

        public event Action<string> OnLineAdded;
        public event Action OnChanged;

        public IReadOnlyList<string> Lines => _lines;
        public int Count => _lines.Count;
        public string Latest => _lines.Count > 0 ? _lines[0] : string.Empty;

        public ExpeditionEncounterLog(int capacity = DefaultCapacity)
        {
            _capacity = Math.Max(4, capacity);
        }

        public void Add(string line)
        {
            if (string.IsNullOrEmpty(line)) return;
            _lines.Insert(0, line);
            while (_lines.Count > _capacity)
                _lines.RemoveAt(_lines.Count - 1);
            OnLineAdded?.Invoke(line);
            OnChanged?.Invoke();
        }

        public void Clear()
        {
            if (_lines.Count == 0) return;
            _lines.Clear();
            OnChanged?.Invoke();
        }

        public string FormatSummary(int maxLines = 6)
        {
            if (_lines.Count == 0) return "ENCOUNTER LOG: quiet.";
            maxLines = Math.Max(1, maxLines);
            var parts = new System.Text.StringBuilder(200);
            parts.Append("ENCOUNTER LOG");
            parts.Append(" (").Append(_lines.Count).Append(')');
            parts.AppendLine();
            int n = Math.Min(maxLines, _lines.Count);
            for (int i = 0; i < n; i++)
            {
                parts.Append("  · ");
                parts.AppendLine(_lines[i]);
            }
            if (_lines.Count > maxLines)
                parts.Append("  … +").Append(_lines.Count - maxLines).Append(" older");
            return parts.ToString().TrimEnd();
        }
    }
}
