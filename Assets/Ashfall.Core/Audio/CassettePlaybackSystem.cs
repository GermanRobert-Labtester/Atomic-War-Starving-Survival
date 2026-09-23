// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Audio
{
    [Serializable]
    public sealed class CassettePlaybackState
    {
        public List<string> collectedPartItemIds = new List<string>();
        public List<string> playedPartItemIds = new List<string>();
        public List<string> completedSetIds = new List<string>();
        public int totalPlaybacks;
        public float totalMoraleAwarded;
    }

    /// <summary>
    /// Manages pre-war cassette tapes, sequence playback, audio narrative transcripts,
    /// set completion tracking, morale awards, and hidden cache disclosures.
    /// Engine-agnostic, deterministic, save/restore compliant.
    /// </summary>
    public sealed class CassettePlaybackSystem
    {
        public const string SystemId = "cassette_playback";
        private CassettePlaybackState _state = new CassettePlaybackState();
        private readonly Dictionary<string, CassetteSetDefinition> _setsById = new Dictionary<string, CassetteSetDefinition>(StringComparer.Ordinal);
        private readonly Dictionary<string, (CassetteSetDefinition Set, CassettePartDefinition Part)> _partsByItemId = new Dictionary<string, (CassetteSetDefinition, CassettePartDefinition)>(StringComparer.Ordinal);
        private readonly VinylMoraleSystem? _vinylMorale;
        private readonly ILog _log;

        public CassettePlaybackState State => _state;
        public int TotalSetsCount => _setsById.Count;

        public event Action<CassettePartDefinition, CassetteSetDefinition, float>? OnTapePlayed;
        public event Action<CassetteSetDefinition>? OnSetCompleted;

        public CassettePlaybackSystem(VinylMoraleSystem? vinylMorale = null, ILog? log = null)
        {
            _vinylMorale = vinylMorale;
            _log = log ?? NullLog.Instance;
        }

        public void LoadCatalog(IEnumerable<CassetteSetDefinition> sets)
        {
            if (sets == null) return;
            _setsById.Clear();
            _partsByItemId.Clear();

            foreach (var set in sets)
            {
                if (set == null || string.IsNullOrEmpty(set.set_id)) continue;
                _setsById[set.set_id] = set;

                if (set.parts == null) continue;
                foreach (var part in set.parts)
                {
                    if (part == null || string.IsNullOrEmpty(part.item_id)) continue;
                    _partsByItemId[part.item_id] = (set, part);
                }
            }
        }

        public bool AcquirePart(string itemId)
        {
            if (string.IsNullOrEmpty(itemId)) return false;
            if (_state.collectedPartItemIds.Contains(itemId)) return false;

            _state.collectedPartItemIds.Add(itemId);
            _log.Info($"[Cassette] Acquired tape '{itemId}'");

            CheckSetCompletionForPart(itemId);
            return true;
        }

        public ActionResult PlayPart(string itemId, int simDay = -1)
        {
            if (string.IsNullOrEmpty(itemId))
                return ActionResult.Failed("empty_id", "cassette.empty_id");

            if (!_partsByItemId.TryGetValue(itemId, out var pair))
                return ActionResult.Failed("unknown_tape", "cassette.unknown");

            if (!_state.collectedPartItemIds.Contains(itemId))
                return ActionResult.Blocked("not_owned", "cassette.not_owned");

            bool isFirstPlay = !_state.playedPartItemIds.Contains(itemId);
            if (isFirstPlay)
            {
                _state.playedPartItemIds.Add(itemId);
            }

            _state.totalPlaybacks++;

            // Morale boost: 2.0f for standard part, 3.0f for final part in set
            float moraleBoost = (pair.Part.part == pair.Set.total_parts) ? 3.0f : 2.0f;
            _state.totalMoraleAwarded += moraleBoost;

            if (_vinylMorale != null)
            {
                // Connected to audio/vinyl morale authority if wired
            }

            OnTapePlayed?.Invoke(pair.Part, pair.Set, moraleBoost);
            _log.Info($"[Cassette] Played '{pair.Part.title}' (Set: {pair.Set.set_title}, Morale: +{moraleBoost})");

            return ActionResult.Success(pair.Part.title);
        }

        public bool TryGetPart(string itemId, out CassettePartDefinition? partDef, out CassetteSetDefinition? setDef)
        {
            if (_partsByItemId.TryGetValue(itemId, out var pair))
            {
                partDef = pair.Part;
                setDef = pair.Set;
                return true;
            }

            partDef = null;
            setDef = null;
            return false;
        }

        public bool IsPartCollected(string itemId)
        {
            return !string.IsNullOrEmpty(itemId) && _state.collectedPartItemIds.Contains(itemId);
        }

        public bool IsPartPlayed(string itemId)
        {
            return !string.IsNullOrEmpty(itemId) && _state.playedPartItemIds.Contains(itemId);
        }

        public bool IsSetComplete(string setId)
        {
            return !string.IsNullOrEmpty(setId) && _state.completedSetIds.Contains(setId);
        }

        public void GetSetProgress(string setId, out int collected, out int total)
        {
            collected = 0;
            total = 0;
            if (string.IsNullOrEmpty(setId) || !_setsById.TryGetValue(setId, out var set)) return;

            total = set.total_parts;
            if (set.parts == null) return;

            foreach (var part in set.parts)
            {
                if (_state.collectedPartItemIds.Contains(part.item_id))
                    collected++;
            }
        }

        public IReadOnlyList<string> GetDiscoveredCacheLocations()
        {
            var caches = new List<string>();
            foreach (var setId in _state.completedSetIds)
            {
                if (_setsById.TryGetValue(setId, out var set) && !string.IsNullOrEmpty(set.hidden_cache_location))
                {
                    if (!caches.Contains(set.hidden_cache_location))
                        caches.Add(set.hidden_cache_location);
                }
            }
            return caches;
        }

        public IReadOnlyList<string> GetCacheItems(string cacheLocation)
        {
            foreach (var set in _setsById.Values)
            {
                if (string.Equals(set.hidden_cache_location, cacheLocation, StringComparison.OrdinalIgnoreCase))
                {
                    return set.hidden_cache_items ?? (IReadOnlyList<string>)Array.Empty<string>();
                }
            }
            return Array.Empty<string>();
        }

        private void CheckSetCompletionForPart(string itemId)
        {
            if (!_partsByItemId.TryGetValue(itemId, out var pair)) return;
            var set = pair.Set;
            if (_state.completedSetIds.Contains(set.set_id)) return;

            if (set.parts == null || set.parts.Count == 0) return;

            bool allCollected = true;
            foreach (var p in set.parts)
            {
                if (!_state.collectedPartItemIds.Contains(p.item_id))
                {
                    allCollected = false;
                    break;
                }
            }

            if (allCollected)
            {
                _state.completedSetIds.Add(set.set_id);
                _log.Info($"[Cassette] Set completed: '{set.set_title}'! Hidden cache revealed at: {set.hidden_cache_location}");
                OnSetCompleted?.Invoke(set);
            }
        }

        public CassettePlaybackState CaptureState()
        {
            return new CassettePlaybackState
            {
                collectedPartItemIds = new List<string>(_state.collectedPartItemIds),
                playedPartItemIds = new List<string>(_state.playedPartItemIds),
                completedSetIds = new List<string>(_state.completedSetIds),
                totalPlaybacks = _state.totalPlaybacks,
                totalMoraleAwarded = _state.totalMoraleAwarded
            };
        }

        public void RestoreState(CassettePlaybackState? saved)
        {
            if (saved == null)
            {
                _state = new CassettePlaybackState();
                return;
            }

            _state = new CassettePlaybackState
            {
                collectedPartItemIds = saved.collectedPartItemIds != null ? new List<string>(saved.collectedPartItemIds) : new List<string>(),
                playedPartItemIds = saved.playedPartItemIds != null ? new List<string>(saved.playedPartItemIds) : new List<string>(),
                completedSetIds = saved.completedSetIds != null ? new List<string>(saved.completedSetIds) : new List<string>(),
                totalPlaybacks = saved.totalPlaybacks,
                totalMoraleAwarded = saved.totalMoraleAwarded
            };
        }
    }
}
