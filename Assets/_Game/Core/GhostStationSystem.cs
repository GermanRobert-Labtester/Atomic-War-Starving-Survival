using System;
using System.Collections.Generic;
using AtomicWar._Game.Data;
using AtomicWar._Game.Events;
using AtomicWar._Game.Survivors;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Prompt #19 — Ghost Stations: after the EMP (Flashpoint), the dial can
    /// pick up pre-war loops and dead operators. Tuning yields GhostLoop intel
    /// only (morale scrape / journal scrap) — never plume reports, faction
    /// coords, or extraction unlocks.
    /// </summary>
    public class GhostStationSystem
    {
        public const string IdPrefix = "ghost_station_";
        public const float DefaultMoraleHit = 4f;
        public const float DefaultBaseSignal = 0.25f;
        public const float DefaultInterference = 0.7f;

        /// <summary>Knowledge key for the dead-operator diary chain.</summary>
        public const string DiaryDeadOperatorKey = "diary_ghost_dead_operator";

        private readonly List<GhostStationDef> _catalog = new List<GhostStationDef>();
        private readonly HashSet<string> _heard = new HashSet<string>(StringComparer.Ordinal);
        private readonly List<RadioFrequencySO> _runtimeFreqs = new List<RadioFrequencySO>();
        private readonly List<RadioBroadcastSO> _runtimeBroadcasts = new List<RadioBroadcastSO>();

        private RadioTunerSystem _tuner;
        private JournalSystem _journal;
        private Func<IReadOnlyList<Survivor>> _getSurvivors;
        private Func<int> _getDay;
        private bool _unlocked;
        private bool _frequenciesInjected;

        public bool IsUnlocked => _unlocked;
        public IReadOnlyCollection<string> HeardStationIds => _heard;
        public IReadOnlyList<GhostStationDef> Catalog => _catalog;

        public event Action OnUnlocked;
        public event Action<GhostStationDef, IntelNode> OnGhostHeard;
        public event Action OnStateChanged;

        public GhostStationSystem()
        {
            SeedDefaultCatalog();
        }

        public void Bind(
            RadioTunerSystem tuner = null,
            JournalSystem journal = null,
            Func<IReadOnlyList<Survivor>> getSurvivors = null,
            Func<int> getDay = null)
        {
            if (_tuner != null)
                _tuner.OnIntelExtracted -= HandleIntelExtracted;

            _tuner = tuner;
            _journal = journal;
            _getSurvivors = getSurvivors;
            _getDay = getDay ?? (() => 0);

            if (_tuner != null)
                _tuner.OnIntelExtracted += HandleIntelExtracted;

            if (_unlocked)
                EnsureFrequenciesInjected();
        }

        public void Unbind()
        {
            if (_tuner != null)
                _tuner.OnIntelExtracted -= HandleIntelExtracted;
            _tuner = null;
        }

        public int CurrentDay => _getDay != null ? _getDay() : 0;

        /// <summary>
        /// Call when the nuclear-exchange EMP fires (Flashpoint or fallback).
        /// Idempotent — unlocks ghost bands once.
        /// </summary>
        public bool NotifyEmpOccurred()
        {
            if (_unlocked) return false;
            _unlocked = true;
            EnsureFrequenciesInjected();
            OnUnlocked?.Invoke();
            OnStateChanged?.Invoke();
            return true;
        }

        /// <summary>True if frequency id is a registered ghost station.</summary>
        public bool IsGhostFrequencyId(string frequencyId)
        {
            if (string.IsNullOrEmpty(frequencyId)) return false;
            if (frequencyId.StartsWith(IdPrefix, StringComparison.Ordinal)) return true;
            return FindDef(frequencyId) != null;
        }

        public GhostStationDef FindDef(string stationId)
        {
            if (string.IsNullOrEmpty(stationId)) return null;
            for (int i = 0; i < _catalog.Count; i++)
            {
                if (_catalog[i] != null
                    && string.Equals(_catalog[i].Id, stationId, StringComparison.Ordinal))
                    return _catalog[i];
            }
            return null;
        }

        public bool HasHeard(string stationId)
        {
            return !string.IsNullOrEmpty(stationId) && _heard.Contains(stationId);
        }

        /// <summary>
        /// Apply ghost-station hear effects for an intel node (or force by id in tests).
        /// Returns true if effects applied (first hear of this station).
        /// </summary>
        public bool ApplyGhostHear(IntelNode intel)
        {
            if (intel == null || intel.Type != IntelType.GhostLoop) return false;
            return ApplyGhostHear(intel.SourceFrequencyId, intel);
        }

        /// <summary>Force-hear a catalog station (tests / scripted).</summary>
        public bool ApplyGhostHear(string stationId, IntelNode intel = null)
        {
            if (!_unlocked) return false;
            var def = FindDef(stationId);
            if (def == null) return false;
            if (!_heard.Add(def.Id)) return false;

            ApplyMoraleHit(def.MoraleHit);

            if (def.UnlocksDiary && !string.IsNullOrEmpty(def.DiaryKnowledgeKey)
                && !string.IsNullOrEmpty(def.DiaryText))
            {
                var author = PickAuthor();
                _journal?.TryAddRawEntry(
                    def.DiaryKnowledgeKey,
                    def.DiaryText,
                    author,
                    CurrentDay);
            }

            var node = intel ?? IntelNode.CreateGhostLoop(def.Id, CurrentDay, def.LoopText);
            OnGhostHeard?.Invoke(def, node);
            OnStateChanged?.Invoke();
            return true;
        }

        /// <summary>
        /// Build a GhostLoop intel the way the tuner would (for isolated tests).
        /// Never produces PlumeReport / military types.
        /// </summary>
        public static IntelNode CreateGhostIntel(GhostStationDef def, int day)
        {
            if (def == null) return null;
            return IntelNode.CreateGhostLoop(def.Id, day, def.LoopText, confidence: 0.15f);
        }

        /// <summary>
        /// Create runtime RadioFrequencySO assets for all catalog ghosts and
        /// inject them into the bound tuner. Safe to call repeatedly.
        /// </summary>
        public void EnsureFrequenciesInjected()
        {
            if (!_unlocked || _tuner == null) return;

            for (int i = 0; i < _catalog.Count; i++)
            {
                var def = _catalog[i];
                if (def == null || string.IsNullOrEmpty(def.Id)) continue;
                if (_tuner.GetFrequency(def.Id) != null) continue;

                var freq = BuildFrequency(def);
                if (freq == null) continue;
                _runtimeFreqs.Add(freq);
                _tuner.AddFrequency(freq);
            }

            _frequenciesInjected = true;
        }

        public GhostStationSave CaptureState()
        {
            var heard = new string[_heard.Count];
            int i = 0;
            foreach (var id in _heard)
                heard[i++] = id;
            return new GhostStationSave
            {
                Unlocked = _unlocked,
                HeardStationIds = heard
            };
        }

        public void RestoreState(GhostStationSave save)
        {
            _heard.Clear();
            _unlocked = false;
            _frequenciesInjected = false;
            if (save == null) return;
            _unlocked = save.Unlocked;
            if (save.HeardStationIds != null)
            {
                for (int i = 0; i < save.HeardStationIds.Length; i++)
                {
                    string id = save.HeardStationIds[i];
                    if (!string.IsNullOrEmpty(id))
                        _heard.Add(id);
                }
            }
            if (_unlocked)
                EnsureFrequenciesInjected();
        }

        public void Clear()
        {
            _heard.Clear();
            _unlocked = false;
            _frequenciesInjected = false;
            DestroyRuntimeAssets();
        }

        public void DestroyRuntimeAssets()
        {
            for (int i = 0; i < _runtimeFreqs.Count; i++)
            {
                if (_runtimeFreqs[i] != null)
                    UnityEngine.Object.DestroyImmediate(_runtimeFreqs[i]);
            }
            _runtimeFreqs.Clear();
            for (int i = 0; i < _runtimeBroadcasts.Count; i++)
            {
                if (_runtimeBroadcasts[i] != null)
                    UnityEngine.Object.DestroyImmediate(_runtimeBroadcasts[i]);
            }
            _runtimeBroadcasts.Clear();
        }

        // -----------------------------------------------------------------

        private void HandleIntelExtracted(IntelNode intel)
        {
            if (intel == null || intel.Type != IntelType.GhostLoop) return;
            ApplyGhostHear(intel);
        }

        private void ApplyMoraleHit(float amount)
        {
            if (amount <= 0f) return;
            var list = _getSurvivors?.Invoke();
            if (list == null) return;
            for (int i = 0; i < list.Count; i++)
            {
                var s = list[i];
                if (s == null || !s.IsAlive) continue;
                s.Needs.Morale = Mathf.Clamp(s.Needs.Morale - amount, 0f, 100f);
            }
        }

        private Survivor PickAuthor()
        {
            var list = _getSurvivors?.Invoke();
            if (list == null) return null;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] != null && list[i].IsAlive)
                    return list[i];
            }
            return null;
        }

        private RadioFrequencySO BuildFrequency(GhostStationDef def)
        {
            var broadcast = ScriptableObject.CreateInstance<RadioBroadcastSO>();
            broadcast.id = def.Id + "_loop";
            broadcast.message = def.LoopText ?? string.Empty;
            broadcast.minDay = 0;
            broadcast.maxDay = -1;
            _runtimeBroadcasts.Add(broadcast);

            var freq = ScriptableObject.CreateInstance<RadioFrequencySO>();
            freq.id = def.Id;
            freq.displayName = def.DisplayName;
            freq.frequencyMHz = def.FrequencyMHz;
            freq.type = RadioFrequencyType.GhostStation;
            freq.activeFromDay = 0;
            freq.activeUntilDay = -1;
            freq.baseSignalStrength = def.BaseSignalStrength > 0f
                ? def.BaseSignalStrength
                : DefaultBaseSignal;
            freq.interferenceSusceptibility = DefaultInterference;
            freq.interceptChannelTag = string.Empty;
            freq.broadcasts = new List<RadioBroadcastSO> { broadcast };
            return freq;
        }

        private void SeedDefaultCatalog()
        {
            _catalog.Clear();
            _catalog.Add(new GhostStationDef
            {
                Id = RadioFrequencySO.Ids.GhostWeatherLoop,
                DisplayName = "71.2 Ghost Weather",
                FrequencyMHz = 71.2f,
                LoopText =
                    "…ceiling unlimited… winds calm… temperature twenty-two… " +
                    "This is a recorded loop. This is a recorded loop. This is a—",
                MoraleHit = DefaultMoraleHit,
                UnlocksDiary = false
            });
            _catalog.Add(new GhostStationDef
            {
                Id = RadioFrequencySO.Ids.GhostDeadOperator,
                DisplayName = "54.0 Dead Operator",
                FrequencyMHz = 54.0f,
                LoopText =
                    "If anyone is still listening — the shelter list is wrong. " +
                    "Do not go north of the river. I already—  [static]  " +
                    "If anyone is still listening — the shelter list is wrong.",
                MoraleHit = DefaultMoraleHit + 2f,
                UnlocksDiary = true,
                DiaryKnowledgeKey = DiaryDeadOperatorKey,
                DiaryText =
                    "Day note. Found a voice on 54.0. Not live — the breath cuts mid-word " +
                    "and restarts. He said the shelter list is wrong. I wrote it down " +
                    "because the alternative is believing the map."
            });
            _catalog.Add(new GhostStationDef
            {
                Id = RadioFrequencySO.Ids.GhostCivilDefense,
                DisplayName = "162.4 Civil Defense",
                FrequencyMHz = 162.4f,
                LoopText =
                    "This is a civil defense recording. Remain indoors. Seal vents. " +
                    "Await further instructions. Await further instructions. Await—",
                MoraleHit = DefaultMoraleHit,
                UnlocksDiary = false
            });
        }
    }

    [Serializable]
    public class GhostStationDef
    {
        public string Id;
        public string DisplayName;
        public float FrequencyMHz;
        public string LoopText;
        public float MoraleHit = GhostStationSystem.DefaultMoraleHit;
        public float BaseSignalStrength = GhostStationSystem.DefaultBaseSignal;
        public bool UnlocksDiary;
        public string DiaryKnowledgeKey;
        public string DiaryText;
    }

    [Serializable]
    public class GhostStationSave
    {
        public bool Unlocked;
        public string[] HeardStationIds;
    }
}
