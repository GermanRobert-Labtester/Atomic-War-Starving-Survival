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
    public partial class GhostStationSystem
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
#pragma warning disable CS0414 // State flag retained for future save/load and diagnostics.
        private bool _frequenciesInjected;
#pragma warning restore CS0414

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

        public void Clear()
        {
            _heard.Clear();
            _unlocked = false;
            _frequenciesInjected = false;
            DestroyRuntimeAssets();
        }

        public void DestroyRuntimeAssets()
        {
            // MISC-003: DestroyImmediate is for editor/import time; play mode should
            // use Destroy so assets are cleaned up at end-of-frame safely.
            for (int i = 0; i < _runtimeFreqs.Count; i++)
            {
                if (_runtimeFreqs[i] != null)
                {
#if UNITY_EDITOR
                    if (!Application.isPlaying) UnityEngine.Object.DestroyImmediate(_runtimeFreqs[i]);
                    else UnityEngine.Object.Destroy(_runtimeFreqs[i]);
#else
                    UnityEngine.Object.Destroy(_runtimeFreqs[i]);
#endif
                }
            }
            _runtimeFreqs.Clear();
            for (int i = 0; i < _runtimeBroadcasts.Count; i++)
            {
                if (_runtimeBroadcasts[i] != null)
                {
#if UNITY_EDITOR
                    if (!Application.isPlaying) UnityEngine.Object.DestroyImmediate(_runtimeBroadcasts[i]);
                    else UnityEngine.Object.Destroy(_runtimeBroadcasts[i]);
#else
                    UnityEngine.Object.Destroy(_runtimeBroadcasts[i]);
#endif
                }
            }
            _runtimeBroadcasts.Clear();
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
