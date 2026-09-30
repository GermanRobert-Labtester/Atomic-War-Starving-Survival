// SPDX-License-Identifier: MIT
// ============================================================================
// ASHFALL Cultural Cassette Sets — host wiring over the sealed
// Ashfall.Core.Audio.CassettePlaybackSystem and the previously-uncalled
// CassetteSetCatalogLoader.
//
// Before this wiring the repo's own content certification declared the consumer
// an orphan (Main.ContentCertification: MarkConsumerActive("CassettePlaybackSystem",
// false)) and PlayPart recorded morale without ever applying it. Tape morale now
// goes through the canonical survivor needs authority and cache rewards through
// the canonical inventory; nothing else is owned here.
// ============================================================================

using System;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Audio;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private CassettePlaybackHostSession? _cassettePlayback;
        private bool _cassettePlaybackDirty;

        public CassettePlaybackHostSession? CassettePlayback => _cassettePlayback;

        public void SetupCassettePlayback()
        {
            if (_cassettePlayback != null) return;

            var vinyl = _vinylMorale?.System;
            var system = new CassettePlaybackSystem(vinyl, new GodotLog());
            var saved = CassettePlaybackSaveStore.TryLoad();

            _cassettePlayback = new CassettePlaybackHostSession(
                system,
                () => _inventory?.Inventory,
                () => _survivors?.Needs,
                () => _holdfastRuntime?.PlayerSurvivorId ?? PlayerFallbackSurvivorId(),
                saved);
            _cassettePlayback.StateChanged += () => _cassettePlaybackDirty = true;
            _cassettePlayback.LoadCatalog(_dataDir ?? CatalogPath.ResolveDataDir());

            // A completed collection becomes a chronicle line, once per set.
            _cassettePlayback.OnCacheGranted += (set, granted) =>
            {
                _journal?.TryAddRawEntry(
                    "cassette_set_completed",
                    $"CASSETTE: '{set.set_title}' assembled. {set.completion_narrative}"
                    + $" The cache at '{set.hidden_cache_location}' yielded {granted} item(s).",
                    null!,
                    CassetteJournalDay());
                _cassettePlaybackDirty = true;
            };
        }

        private string? PlayerFallbackSurvivorId()
        {
            var roster = _survivors?.RosterState;
            if (roster == null) return null;
            foreach (var s in roster)
                if (s != null && s.IsAliveState && !string.IsNullOrEmpty(s.Id)) return s.Id;
            return null;
        }

        private int CassetteJournalDay() => _core?.Clock.Day ?? _simDay;

        /// <summary>Derives tape acquisition from the live inventory. Returns newly acquired.</summary>
        public int SyncCassetteAcquisitions()
        {
            SetupCassettePlayback();
            return _cassettePlayback?.SyncAcquisitionsFromInventory() ?? 0;
        }

        /// <summary>Canonical day-owner body: acquire what the shelter now holds.</summary>
        public int TickCassettePlayback()
        {
            if (_cassettePlayback == null) return 0;
            int acquired = _cassettePlayback.SyncAcquisitionsFromInventory();
            if (acquired > 0) _cassettePlaybackDirty = true;
            return acquired;
        }

        public string PlayCassettePart(string itemId)
        {
            SetupCassettePlayback();
            if (_cassettePlayback == null) return "cassette owner unavailable";
            var result = _cassettePlayback.PlayPart(itemId, CassetteJournalDay());
            _cassettePlaybackDirty = true;
            return _cassettePlayback.LastEvent;
        }

        public string CassettePlaybackStatusLine() =>
            _cassettePlayback?.StatusLine() ?? "no cassette catalog";

        /// <summary>True when the authored catalog is loaded and the consumer is live.</summary>
        public bool IsCassetteConsumerActive() =>
            _cassettePlayback != null && _cassettePlayback.SetCount > 0;

        public void SaveCassettePlayback()
        {
            if (_cassettePlayback == null) return;
            var state = _cassettePlayback.CaptureState();
            if (CaptureSection(CassettePlaybackSaveStore.SectionName, CassettePlaybackSaveStore.TryCapturePersisted(state)))
                _cassettePlaybackDirty = false;
        }

        public void FlushCassettePlaybackIfDirty()
        {
            if (_cassettePlaybackDirty) SaveCassettePlayback();
        }

        public void ResetCassettePlayback()
        {
            _cassettePlayback = null;
            _cassettePlaybackDirty = false;
        }
    }
}
