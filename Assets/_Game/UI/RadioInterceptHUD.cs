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
    /// Bands are supplied from RadioTunerSystem (intel + intercepts share one dial).
    /// Optional <see cref="FactionRadioVoHook"/> plays channel-tag VO stubs.
    /// </summary>
    public class RadioInterceptHUD : MonoBehaviour
    {
        public const int MaxVisibleLinesCollapsed = 6;
        /// <summary>Keep in sync with FactionRadioInterceptSystem.MaxLogEntries.</summary>
        public const int MaxLogEntries = 24;

        /// <summary>
        /// One dial position. Index 0 is always ALL BANDS (detuned).
        /// Core maps these from RadioFrequencySO via Bootstrap — UI must not ref Core.
        /// </summary>
        [Serializable]
        public struct TunerBand
        {
            public string FrequencyId;
            public string Label;
            public string ChannelTag;

            public bool IsAllBands => string.IsNullOrEmpty(FrequencyId);

            public static TunerBand AllBands => new TunerBand
            {
                FrequencyId = string.Empty,
                Label = "ALL BANDS",
                ChannelTag = string.Empty
            };

            public static TunerBand ChannelOnly(string channelTag, string label = null)
            {
                return new TunerBand
                {
                    FrequencyId = string.Empty,
                    Label = !string.IsNullOrEmpty(label) ? label : (channelTag ?? "BAND"),
                    ChannelTag = channelTag ?? string.Empty
                };
            }

            public static TunerBand FromParts(string frequencyId, string label, string channelTag)
            {
                return new TunerBand
                {
                    FrequencyId = frequencyId ?? string.Empty,
                    Label = !string.IsNullOrEmpty(label) ? label : (frequencyId ?? "BAND"),
                    ChannelTag = channelTag ?? string.Empty
                };
            }
        }

        /// <summary>
        /// Fallback presets when Bootstrap has not bound RadioTunerSystem bands yet
        /// (EditMode tests / early Awake). Index 0 = all; then mil / scav / prepper.
        /// </summary>
        public static readonly TunerBand[] DefaultTunerBands =
        {
            TunerBand.AllBands,
            TunerBand.ChannelOnly("CH-7 MILBAND"),
            TunerBand.ChannelOnly("CH-3 ASH ROAD"),
            TunerBand.ChannelOnly("CH-11 STOCKPILE")
        };

        // Back-compat aliases for older tests / call sites.
        public static readonly string[] TunerPresets =
        {
            "",
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
        public string BoundFrequencyId { get; private set; } = string.Empty;
        public string LatestMessage { get; private set; } = string.Empty;
        public string LatestKind { get; private set; } = string.Empty;
        public string LatestChannelTag { get; private set; } = string.Empty;
        public string StatusLine { get; private set; } = "RADIO: —";
        public string TunerLine { get; private set; } = "TUNE: ALL BANDS  [ ]";
        public string DetailSummary { get; private set; } = "No intercepts.";

        /// <summary>
        /// Fired when the dial moves. Args: frequencyId (empty = detune), channelTag.
        /// Bootstrap wires this to RadioTunerSystem.TuneToFrequency / Detune.
        /// </summary>
        public event Action<string, string> OnTunerBandChanged;

        private readonly List<Line> _lines = new List<Line>();
        private readonly List<TunerBand> _bands = new List<TunerBand>(DefaultTunerBands);
        private bool _suppressTunerCallback;

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

        /// <summary>Current dial positions (index 0 = ALL BANDS).</summary>
        public IReadOnlyList<TunerBand> Bands => _bands;

        public int BandCount => _bands.Count;

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
            if (_bands.Count == 0)
                ResetToDefaultBands();
            // Ensure VO stubs exist when the strip is present on a HUD prefab.
            var vo = VoHook;
            vo.EnsureBuiltInStubs();
            Refresh();
        }

        /// <summary>
        /// Replace dial positions from RadioTunerSystem (via Bootstrap).
        /// Preserves current index when possible; clamps otherwise.
        /// </summary>
        public void SetTunerBands(IReadOnlyList<TunerBand> bands)
        {
            _bands.Clear();
            if (bands != null)
            {
                for (int i = 0; i < bands.Count; i++)
                {
                    var b = bands[i];
                    if (string.IsNullOrEmpty(b.Label) && string.IsNullOrEmpty(b.FrequencyId)
                        && string.IsNullOrEmpty(b.ChannelTag) && i > 0)
                        continue;
                    if (string.IsNullOrEmpty(b.Label))
                        b.Label = string.IsNullOrEmpty(b.FrequencyId) ? "ALL BANDS" : b.FrequencyId;
                    _bands.Add(b);
                }
            }
            if (_bands.Count == 0 || !_bands[0].IsAllBands
                || !string.IsNullOrEmpty(_bands[0].ChannelTag))
            {
                _bands.Insert(0, TunerBand.AllBands);
            }
            // Re-apply current index against new band list without double-firing.
            SetTunerIndex(TunerIndex, playBlip: false, notify: false);
        }

        public void ResetToDefaultBands()
        {
            _bands.Clear();
            for (int i = 0; i < DefaultTunerBands.Length; i++)
                _bands.Add(DefaultTunerBands[i]);
            SetTunerIndex(0, playBlip: false, notify: false);
        }

        /// <summary>
        /// Sync dial to a RadioTunerSystem frequency id (empty = ALL / detuned).
        /// Does not re-fire OnTunerBandChanged (avoids feedback loops).
        /// </summary>
        public void SyncFromFrequencyId(string frequencyId)
        {
            if (string.IsNullOrEmpty(frequencyId))
            {
                SetTunerIndex(0, playBlip: false, notify: false);
                return;
            }
            for (int i = 0; i < _bands.Count; i++)
            {
                if (string.Equals(_bands[i].FrequencyId, frequencyId, StringComparison.Ordinal))
                {
                    SetTunerIndex(i, playBlip: false, notify: false);
                    return;
                }
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
        /// Does not re-fire VO. Notifies OnTunerBandChanged so RadioTunerSystem
        /// can re-tune to the restored dial (unless suppressNotify).
        /// </summary>
        public void ApplyUiState(bool isOpen, bool hasUnread, int tunerIndex = 0, bool notifyTuner = true)
        {
            IsOpen = isOpen;
            HasUnread = hasUnread;
            SetTunerIndex(tunerIndex, playBlip: false, notify: notifyTuner);
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

        /// <summary>Cycle frequency filter forward (] key).</summary>
        public void CycleTunerNext() => SetTunerIndex(TunerIndex + 1, playBlip: true);

        /// <summary>Cycle frequency filter backward ([ key).</summary>
        public void CycleTunerPrev() => SetTunerIndex(TunerIndex - 1, playBlip: true);

        public void SetTunerIndex(int index, bool playBlip = false)
        {
            SetTunerIndex(index, playBlip, notify: true);
        }

        private void SetTunerIndex(int index, bool playBlip, bool notify)
        {
            int n = _bands.Count;
            if (n <= 0)
            {
                TunerIndex = 0;
                ActiveChannelFilter = string.Empty;
                BoundFrequencyId = string.Empty;
                Refresh();
                return;
            }
            int wrapped = ((index % n) + n) % n;
            TunerIndex = wrapped;
            var band = _bands[wrapped];
            ActiveChannelFilter = band.ChannelTag ?? string.Empty;
            BoundFrequencyId = band.FrequencyId ?? string.Empty;
            Refresh();
            if (playBlip)
            {
                string tag = ActiveChannelFilter;
                if (string.IsNullOrEmpty(tag))
                    VoHook?.TryPlayChannel(string.Empty, "Succession");
                else
                    VoHook?.TryPlayChannel(tag, string.Empty);
            }
            if (notify && !_suppressTunerCallback)
                OnTunerBandChanged?.Invoke(BoundFrequencyId, ActiveChannelFilter);
        }

        /// <summary>Tune to a specific channel tag (or empty for all).</summary>
        public bool TuneToChannel(string channelTag)
        {
            if (string.IsNullOrEmpty(channelTag))
            {
                SetTunerIndex(0, playBlip: true);
                return true;
            }
            for (int i = 0; i < _bands.Count; i++)
            {
                if (string.Equals(_bands[i].ChannelTag, channelTag, StringComparison.OrdinalIgnoreCase))
                {
                    SetTunerIndex(i, playBlip: true);
                    return true;
                }
            }
            // Fallback: legacy static presets
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

        /// <summary>Tune by RadioFrequencySO id (empty = ALL).</summary>
        public bool TuneToFrequencyId(string frequencyId, bool playBlip = true)
        {
            if (string.IsNullOrEmpty(frequencyId))
            {
                SetTunerIndex(0, playBlip: playBlip);
                return true;
            }
            for (int i = 0; i < _bands.Count; i++)
            {
                if (string.Equals(_bands[i].FrequencyId, frequencyId, StringComparison.Ordinal))
                {
                    SetTunerIndex(i, playBlip: playBlip);
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

            string tuneLabel = ResolveTuneLabel();
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

        private string ResolveTuneLabel()
        {
            if (TunerIndex >= 0 && TunerIndex < _bands.Count)
            {
                var b = _bands[TunerIndex];
                if (!string.IsNullOrEmpty(b.Label)) return b.Label;
                if (!string.IsNullOrEmpty(b.ChannelTag)) return b.ChannelTag;
            }
            if (TunerIndex >= 0 && TunerIndex < TunerLabels.Length)
                return TunerLabels[TunerIndex];
            return "ALL BANDS";
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
