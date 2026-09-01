// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Radio
{
    /// <summary>
    /// Persistent signal intelligence log and station memory.
    /// Invariant: Pure C#, zero engine references, deterministic sorting for SaveChecksum.
    /// </summary>
    public sealed class RadioSignalLog
    {
        private readonly Dictionary<string, SignalLogEntry> _logEntries =
            new Dictionary<string, SignalLogEntry>(StringComparer.OrdinalIgnoreCase);

        private readonly HashSet<string> _discoveredStations =
            new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        private readonly List<float> _customPresets = new List<float>();

        public event Action<SignalLogEntry>? OnSignalLogged;
        public event Action<string>? OnStationDiscovered;

        public IReadOnlyCollection<SignalLogEntry> Entries => _logEntries.Values;
        public IReadOnlyCollection<string> DiscoveredStations => _discoveredStations;
        public IReadOnlyList<float> Presets => _customPresets;

        public bool DiscoverStation(string stationId)
        {
            if (string.IsNullOrEmpty(stationId)) return false;
            if (_discoveredStations.Add(stationId))
            {
                OnStationDiscovered?.Invoke(stationId);
                return true;
            }
            return false;
        }

        public bool IsStationDiscovered(string stationId)
        {
            if (string.IsNullOrEmpty(stationId)) return false;
            return _discoveredStations.Contains(stationId);
        }

        public void AddPreset(float frequencyMhz)
        {
            if (!_customPresets.Contains(frequencyMhz))
            {
                _customPresets.Add(frequencyMhz);
            }
        }

        public void RemovePreset(float frequencyMhz)
        {
            _customPresets.Remove(frequencyMhz);
        }

        public SignalLogEntry LogIntercept(ScheduledBroadcastResult broadcast, int day)
        {
            if (broadcast == null || string.IsNullOrEmpty(broadcast.Headline))
                return new SignalLogEntry();

            string logId = string.IsNullOrEmpty(broadcast.BroadcastId)
                ? $"log_{broadcast.FrequencyMhz:000.0}_{day}_{_logEntries.Count + 1}"
                : $"log_{broadcast.BroadcastId}";

            if (_logEntries.TryGetValue(logId, out var existing))
            {
                return existing;
            }

            var entry = new SignalLogEntry
            {
                id = logId,
                title = broadcast.Headline,
                stationId = broadcast.StationId,
                frequencyMhz = broadcast.FrequencyMhz,
                dayLogged = day,
                summary = broadcast.Message.Length > 120 ? broadcast.Message.Substring(0, 117) + "..." : broadcast.Message,
                isDecoded = broadcast.Genre == BroadcastGenre.NumbersStation,
                isTriangulated = false
            };

            _logEntries[logId] = entry;
            if (!string.IsNullOrEmpty(broadcast.StationId))
            {
                DiscoverStation(broadcast.StationId);
            }

            OnSignalLogged?.Invoke(entry);
            return entry;
        }

        // ── Save / Load ─────────────────────────────────────────────────────────

        public List<SignalLogEntry> CaptureEntries()
        {
            var list = new List<SignalLogEntry>(_logEntries.Values);
            list.Sort((a, b) => string.Compare(a.id, b.id, StringComparison.Ordinal));
            return list;
        }

        public List<string> CaptureDiscoveredStations()
        {
            var list = new List<string>(_discoveredStations);
            list.Sort(StringComparer.Ordinal);
            return list;
        }

        public List<float> CapturePresets()
        {
            return new List<float>(_customPresets);
        }

        public void RestoreState(List<SignalLogEntry>? entries, List<string>? discoveredStations, List<float>? presets)
        {
            _logEntries.Clear();
            _discoveredStations.Clear();
            _customPresets.Clear();

            if (entries != null)
            {
                foreach (var e in entries)
                {
                    if (e == null || string.IsNullOrEmpty(e.id)) continue;
                    _logEntries[e.id] = e;
                }
            }

            if (discoveredStations != null)
            {
                foreach (var s in discoveredStations)
                {
                    if (!string.IsNullOrEmpty(s)) _discoveredStations.Add(s);
                }
            }

            if (presets != null)
            {
                foreach (var p in presets)
                {
                    if (p > 0f && !_customPresets.Contains(p))
                        _customPresets.Add(p);
                }
            }
        }
    }
}
