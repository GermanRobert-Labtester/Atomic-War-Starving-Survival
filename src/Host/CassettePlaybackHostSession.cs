// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : CassettePlaybackSaveStore
// Core State : Ashfall.Core.Audio.CassettePlaybackState
// Host Caller: Main.CassettePlayback
// Purpose    : Cultural cassette set playback — collected tape parts, first-play
//              awards, completed sets, and revealed hidden caches.
//
//              The repo previously declared this consumer an orphan
//              (Main.ContentCertification: "Families whose consumers are still
//              orphans in the host graph"). Tape morale stays with the survivor
//              needs authority and cache items stay with the canonical inventory;
//              this section owns only the tape collection.
// ============================================================================

using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Audio;
using Ashfall.Core.Save;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    public static class CassettePlaybackSaveStore
    {
        public const string FileName = "cassette_playback_save.json";
        public const string SectionName = "cassette_playback";

        private static readonly SaveStore<CassettePlaybackState> s_store =
            SaveStoreHub.Checksummed<CassettePlaybackState>(FileName, nameof(CassettePlaybackSaveStore));

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCapturePersisted(CassettePlaybackState state) => s_store.CaptureBare(state);
        public static CassettePlaybackState? TryRestorePersisted(string json) => s_store.RestoreBare(json);
        public static bool TrySave(CassettePlaybackState state) => s_store.TrySave(state);
        public static CassettePlaybackState? TryLoad() => s_store.TryLoad();
    }

    /// <summary>
    /// Host session over the sealed <see cref="CassettePlaybackSystem"/>.
    ///
    /// <para><b>Authority boundary.</b> The Core system owns only the tape
    /// collection. Tape morale is granted through the canonical survivor needs
    /// authority (<c>NeedsSystem</c>) and only on a part's first play; the hidden
    /// rewards of a completed set enter the world only through the canonical
    /// inventory authority. Acquisition is <em>derived</em> from what the live
    /// inventory actually holds, so there is no second tape ledger.</para>
    ///
    /// <para><b>No invented content.</b> Every set, part, cache location, and cache
    /// item comes from the authored <c>cassette_sets.json</c> through the
    /// previously-uncalled <see cref="CassetteSetCatalogLoader"/>.</para>
    /// </summary>
    public sealed class CassettePlaybackHostSession : HostSessionBase
    {
        private readonly CassettePlaybackSystem _system;
        private readonly Func<Ashfall.Core.Inventory.Inventory?> _inventoryProvider;
        private readonly Func<NeedsSystem?> _needsProvider;
        private readonly Func<string?> _playerProvider;
        private readonly List<CassetteSetDefinition> _catalogSnapshot = new List<CassetteSetDefinition>();
        private bool _awaitingFirstPlayAward;

        /// <summary>Raised once per set, after its cache items entered the inventory.</summary>
        public event Action<CassetteSetDefinition, int>? OnCacheGranted;

        public string LastEvent { get; private set; } = string.Empty;
        public CassettePlaybackSystem System => _system;
        public int SetCount => _system.TotalSetsCount;
        public int CollectedCount => _system.State.collectedPartItemIds?.Count ?? 0;
        public int CompletedSetCount => _system.State.completedSetIds?.Count ?? 0;
        public int TotalPlaybacks => _system.State.totalPlaybacks;

        /// <summary>Cumulative morale actually granted through the needs authority.</summary>
        public float TotalMoraleGranted { get; private set; }

        public CassettePlaybackHostSession(
            CassettePlaybackSystem system,
            Func<Ashfall.Core.Inventory.Inventory?> inventoryProvider,
            Func<NeedsSystem?> needsProvider,
            Func<string?> playerProvider,
            CassettePlaybackState? savedState = null)
        {
            _system = system ?? throw new ArgumentNullException(nameof(system));
            _inventoryProvider = inventoryProvider ?? throw new ArgumentNullException(nameof(inventoryProvider));
            _needsProvider = needsProvider ?? throw new ArgumentNullException(nameof(needsProvider));
            _playerProvider = playerProvider ?? throw new ArgumentNullException(nameof(playerProvider));

            if (savedState != null) _system.RestoreState(savedState);

            _system.OnTapePlayed += HandleTapePlayed;
            _system.OnSetCompleted += HandleSetCompleted;
        }

        /// <summary>Loads the authored cassette set catalog. Returns the set count.</summary>
        public int LoadCatalog(string dataDir)
        {
            var sets = CassetteSetCatalogLoader.Load(dataDir);
            _catalogSnapshot.Clear();
            foreach (var s in sets)
                if (s != null && !string.IsNullOrEmpty(s.set_id)) _catalogSnapshot.Add(s);
            _system.LoadCatalog(sets);
            LastEvent = _system.TotalSetsCount > 0
                ? $"Loaded {_system.TotalSetsCount} authored cassette set(s)."
                : "No authored cassette set catalog found; playback is unavailable.";
            RaiseStateChanged();
            return _system.TotalSetsCount;
        }

        /// <summary>Every authored set, ordinal-sorted by set id.</summary>
        public IReadOnlyList<CassetteSetDefinition> KnownSets()
        {
            var sets = new List<CassetteSetDefinition>(_catalogSnapshot);
            sets.Sort((a, b) => string.CompareOrdinal(a.set_id, b.set_id));
            return sets;
        }

        /// <summary>Every authored part item id, ordinal-sorted (deterministic).</summary>
        public IReadOnlyList<string> AuthoredPartItemIds()
        {
            var ids = new List<string>();
            foreach (var set in KnownSets())
                foreach (var part in set.parts ?? new List<CassettePartDefinition>())
                    if (!string.IsNullOrEmpty(part.item_id)) ids.Add(part.item_id);
            ids.Sort(StringComparer.Ordinal);
            return ids;
        }

        /// <summary>
        /// Derives tape acquisition from the live inventory: a part is acquired only
        /// when the canonical inventory actually holds it. Returns the number newly
        /// acquired. Re-invoking never double-acquires (the engine dedupes).
        /// </summary>
        public int SyncAcquisitionsFromInventory()
        {
            var inventory = _inventoryProvider();
            if (inventory == null || _system.TotalSetsCount == 0) return 0;

            int acquired = 0;
            foreach (string itemId in AuthoredPartItemIds())
            {
                if (_system.IsPartCollected(itemId)) continue;
                if (inventory.CountById(itemId) <= 0) continue;
                if (_system.AcquirePart(itemId)) acquired++;
            }
            if (acquired > 0)
            {
                LastEvent = $"Acquired {acquired} cassette part(s) held by the shelter.";
                RaiseStateChanged();
            }
            return acquired;
        }

        public ActionResult PlayPart(string itemId, int simDay = -1)
        {
            // The Core engine raises OnTapePlayed on EVERY play; it only dedupes its
            // own played-part list. The morale award is therefore gated on the part
            // not having been heard before, so a replay can never re-award it.
            _awaitingFirstPlayAward = !_system.IsPartPlayed(itemId);
            var result = _system.PlayPart(itemId, simDay);
            if (!_system.IsPartPlayed(itemId)) _awaitingFirstPlayAward = false;
            LastEvent = result.IsSuccess
                ? $"Played '{result.EventId}'."
                : $"Playback refused: {result.FailureCode}";
            RaiseStateChanged();
            return result;
        }

        /// <summary>
        /// The Core engine records the award but applies nothing (its vinyl-morale
        /// branch is an explicit stub). The canonical needs authority is therefore
        /// the only writer of tape morale, and only for a part's first play.
        /// </summary>
        private void HandleTapePlayed(CassettePartDefinition part, CassetteSetDefinition set, float moraleBoost)
        {
            if (!_awaitingFirstPlayAward)
            {
                LastEvent = $"Tape '{part.title}' replayed; no further morale lift.";
                return;
            }
            _awaitingFirstPlayAward = false;
            float awarded = AwardMorale(part, moraleBoost);
            if (awarded > 0f)
                LastEvent = $"Tape '{part.title}' lifted morale by {awarded:0.#}.";
        }

        /// <summary>
        /// Grants tape morale through the canonical needs owner. Returns what was
        /// actually granted (0 when no needs owner or no listener is bound).
        /// </summary>
        public float AwardMorale(CassettePartDefinition? part, float moraleBoost)
        {
            if (part == null || moraleBoost <= 0f) return 0f;
            var needs = _needsProvider();
            string survivorId = _playerProvider() ?? string.Empty;
            if (needs == null || string.IsNullOrEmpty(survivorId)) return 0f;

            needs.Modify(survivorId, NeedKind.Morale, moraleBoost);
            TotalMoraleGranted += moraleBoost;
            return moraleBoost;
        }

        private void HandleSetCompleted(CassetteSetDefinition set)
        {
            var inventory = _inventoryProvider();
            int granted = 0;
            if (inventory != null && !string.IsNullOrEmpty(set.hidden_cache_location))
            {
                foreach (string itemId in set.hidden_cache_items ?? new List<string>())
                {
                    if (string.IsNullOrEmpty(itemId)) continue;
                    if (inventory.AddById(itemId, 1)) granted++;
                }
            }
            LastEvent = $"Cassette set '{set.set_title}' complete — cache '{set.hidden_cache_location}' yielded {granted} item(s).";
            OnCacheGranted?.Invoke(set, granted);
            RaiseStateChanged();
        }

        public bool IsPartCollected(string itemId) => _system.IsPartCollected(itemId);
        public bool IsPartPlayed(string itemId) => _system.IsPartPlayed(itemId);
        public bool IsSetComplete(string setId) => _system.IsSetComplete(setId);
        public IReadOnlyList<string> DiscoveredCacheLocations() => _system.GetDiscoveredCacheLocations();
        public IReadOnlyList<string> CacheItems(string cacheLocation) => _system.GetCacheItems(cacheLocation);
        public bool TryGetPart(string itemId, out CassettePartDefinition? part, out CassetteSetDefinition? set)
            => _system.TryGetPart(itemId, out part, out set);
        public void GetSetProgress(string setId, out int collected, out int total)
            => _system.GetSetProgress(setId, out collected, out total);

        /// <summary>Truthful one-line projection of the tape collection.</summary>
        public string StatusLine()
        {
            if (_system.TotalSetsCount == 0) return "no cassette catalog";
            return $"collections {_system.TotalSetsCount} set(s) · parts {CollectedCount}"
                 + $" · completed {CompletedSetCount} · plays {TotalPlaybacks}";
        }

        public CassettePlaybackState CaptureState() => _system.CaptureState();

        public void RestoreState(CassettePlaybackState? state)
        {
            if (state == null) return;
            _system.RestoreState(state);
            LastEvent = "Cassette collection restored from save.";
            RaiseStateChanged();
        }

        public void Clear()
        {
            _system.RestoreState(new CassettePlaybackState());
            TotalMoraleGranted = 0f;
            LastEvent = "Cassette collection cleared.";
            RaiseStateChanged();
        }
    }
}
