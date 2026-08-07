using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AtomicWar._Game.UI
{
    /// <summary>
    /// Expedition combat / encounter feedback strip. Core pushes lines via
    /// <see cref="Push"/>; no Core assembly reference required.
    /// </summary>
    public class ExpeditionEncounterLogHUD : MonoBehaviour
    {
        public const int MaxLines = 32;

        public bool IsOpen { get; private set; }
        public string StatusLine { get; private set; } = "ENCOUNTER LOG: quiet.";
        public string DetailSummary { get; private set; } = "No field contact logged.";
        public string LatestLine { get; private set; } = string.Empty;
        public int LineCount => _lines.Count;

        private readonly List<string> _lines = new List<string>(MaxLines);

        public IReadOnlyList<string> Lines => _lines;

        public event Action OnChanged;

        public void Push(string line)
        {
            if (string.IsNullOrEmpty(line)) return;
            _lines.Insert(0, line);
            while (_lines.Count > MaxLines)
                _lines.RemoveAt(_lines.Count - 1);
            LatestLine = line;
            Rebuild();
            OnChanged?.Invoke();
        }

        public void SetLines(IReadOnlyList<string> lines)
        {
            _lines.Clear();
            if (lines != null)
            {
                for (int i = 0; i < lines.Count && i < MaxLines; i++)
                {
                    if (!string.IsNullOrEmpty(lines[i]))
                        _lines.Add(lines[i]);
                }
            }
            LatestLine = _lines.Count > 0 ? _lines[0] : string.Empty;
            Rebuild();
            OnChanged?.Invoke();
        }

        public void Clear()
        {
            if (_lines.Count == 0) return;
            _lines.Clear();
            LatestLine = string.Empty;
            Rebuild();
            OnChanged?.Invoke();
        }

        public void Open()
        {
            IsOpen = true;
            Rebuild();
        }

        public void Close() => IsOpen = false;

        public void Toggle()
        {
            if (IsOpen) Close();
            else Open();
        }

        private void Rebuild()
        {
            if (_lines.Count == 0)
            {
                StatusLine = "ENCOUNTER LOG: quiet.";
                DetailSummary = "No field contact logged.";
                return;
            }

            StatusLine = "ENCOUNTER LOG  ·  " + Truncate(LatestLine, 64);
            var sb = new StringBuilder(256);
            sb.AppendLine(StatusLine);
            int n = Math.Min(8, _lines.Count);
            for (int i = 0; i < n; i++)
            {
                sb.Append("  · ");
                sb.AppendLine(_lines[i]);
            }
            if (_lines.Count > 8)
                sb.Append("  … +").Append(_lines.Count - 8).Append(" older");
            DetailSummary = sb.ToString().TrimEnd();
        }

        private static string Truncate(string s, int max)
        {
            if (string.IsNullOrEmpty(s) || s.Length <= max) return s ?? string.Empty;
            return s.Substring(0, max - 1) + "…";
        }
    }
}
