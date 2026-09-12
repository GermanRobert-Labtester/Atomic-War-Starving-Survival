// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ashfall.Core.Narrative
{
    /// <summary>Serialized first-heard bookkeeping: lore IDs only — never lyrics (Plan 155 §15).</summary>
    [Serializable]
    public sealed class OralLoreDiscoveryState
    {
        public string systemId = OralLorePerformanceSystem.SystemId;
        public List<string> heardLoreIds = new List<string>();
    }

    /// <summary>A discovered song projected for display: metadata + provenance, lyrics by reference.</summary>
    public sealed class OralLoreHeardEntry
    {
        public string LoreId { get; }
        public string ProducerId { get; }
        public int DayHeard { get; }

        public OralLoreHeardEntry(string loreId, string producerId, int dayHeard)
        {
            LoreId = loreId;
            ProducerId = producerId;
            DayHeard = dayHeard;
        }
    }

    /// <summary>
    /// PLAN 155 Tasks D/E — discovery and performance surface for the oral-lore
    /// corpus. Cultural knowledge only: the system's entire mutable state is
    /// the first-heard ledger. There is no API here that touches morale,
    /// radiation, medicine, navigation, faction control, water output or map
    /// topology — a song may help survivors remember, coordinate, grieve, joke
    /// and belong; it never shortcuts the systems that govern survival.
    ///
    /// Producer model (Task C/E): one primary producer per activated piece —
    ///   - shelter-room contexts (room_* ids from shelter_room_identities.json);
    ///   - expedition/travel contexts (location_* ids from deep_lore_locations.json,
    ///     driven by the same location-discovery event Plan 152 uses);
    ///   - typed social contexts (context_nursery, context_memorial,
    ///     context_faction_culture, context_radio_archive) surfaced by the
    ///     memorial, radio and codex surfaces.
    /// Discovery is idempotent: first-heard fires once per piece; revisits and
    /// reloads never replay it. Unknown future lore IDs in a save are
    /// tolerated and preserved inertly (§15).
    /// </summary>
    public sealed class OralLorePerformanceSystem
    {
        public const string SystemId = "oral_lore_performance";

        private readonly OralLoreCatalog _catalog;
        private readonly Dictionary<string, string> _producerByLoreId =
            new Dictionary<string, string>(StringComparer.Ordinal);
        private readonly Dictionary<string, List<string>> _loreIdsByProducer =
            new Dictionary<string, List<string>>(StringComparer.Ordinal);
        private readonly Dictionary<string, int> _dayFirstHeard =
            new Dictionary<string, int>(StringComparer.Ordinal);
        private OralLoreDiscoveryState _state = new OralLoreDiscoveryState();

        /// <summary>Raised exactly once per piece, on first hear only.</summary>
        public event Action<string, string, int>? OnSongFirstHeard; // loreId, producerId, day
        public event Action? OnStateChanged;

        public OralLorePerformanceSystem(OralLoreCatalog catalog)
        {
            _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
        }

        public OralLoreCatalog Catalog => _catalog;
        public OralLoreDiscoveryState State => _state;

        // ── Producer registration ───────────────────────────────────────

        /// <summary>
        /// Registers the primary producer for one piece. Fails closed for
        /// unknown lore ids, empty producers, or pieces that already have a
        /// producer (one primary producer per piece; secondary surfaces are
        /// display-only).
        /// </summary>
        public bool TryRegisterProducer(string loreId, string producerId)
        {
            if (string.IsNullOrWhiteSpace(loreId) || string.IsNullOrWhiteSpace(producerId))
                return false;
            if (_catalog.GetById(loreId) == null) return false;
            if (_producerByLoreId.ContainsKey(loreId)) return false;

            _producerByLoreId[loreId] = producerId;
            if (!_loreIdsByProducer.TryGetValue(producerId, out var list))
            {
                list = new List<string>();
                _loreIdsByProducer[producerId] = list;
            }
            list.Add(loreId);
            return true;
        }

        /// <summary>
        /// Plan 155 §9 first-pass activation: 18 of 26 pieces across 8 context
        /// families (4 shelter labour, 3 social, 3 nursery, 2 memorial,
        /// 2 expedition, 2 faction/local, 1 medical-comfort, 1 radio/archive).
        /// Room producers use canonical room ids (shelter_room_identities.json);
        /// expedition producers use existing deep-lore location ids; social
        /// contexts use typed context ids. The 8 deferred pieces have explicit
        /// reasons in docs/content/ORAL_LORE_PERFORMANCE_MATRIX.md.
        /// </summary>
        public static IReadOnlyList<(string loreId, string producerId)> DefaultProducerMap()
        {
            return new List<(string, string)>
            {
                // Shelter labour — real labour contexts
                ("song_01_blower_crank_cadence", "room_filtration"),
                ("oral_b2_the_pump_room_shanty", "room_water_pump"),
                ("song_05_smiths_striking_chant", "room_foundry"),
                ("song_09_syndicate_scales_shanty", "room_storage_bay"),
                // Common-room / social gathering
                ("song_06_bakers_sawdust_tune", "room_kitchen"),
                ("oral_b2_the_geiger_counter_waltz", "room_main"),
                ("oral_b2_the_bunker_is_my_body", "room_main"),
                // Nursery / education
                ("song_02_hazard_rhyme_colors", "context_nursery"),
                ("oral_b2_nursery_rhyme_the_ash_falls_down", "context_nursery"),
                ("song_13_childrens_skipping_rhyme", "context_nursery"),
                // Memorial / funeral
                ("song_10_crypt_chiseler_dirge", "context_memorial"),
                ("oral_b2_hymn_of_the_settling_dust", "context_memorial"),
                // Expedition / scout travel (existing deep-lore sites)
                ("song_08_ice_road_courier_cadence", "location_frozen_wetland"),
                ("oral_b2_the_cartographers_song", "location_metro_tunnel"),
                // Faction / local culture (canonical faction + foundry workers)
                ("oral_b2_the_salt_freeholders_march", "faction_salt_freeholders"),
                ("song_15_strike_anthem_iron", "room_foundry"),
                // Medical comfort (display-only; no healing effect exists)
                ("oral_b2_surgeons_lullaby", "room_clinic"),
                // Radio / archive-recovered
                ("song_14_keepers_sky_chant", "context_radio_archive")
            };
        }

        // ── Queries (pure reads) ────────────────────────────────────────

        public bool IsHeard(string loreId)
            => !string.IsNullOrEmpty(loreId) && _state.heardLoreIds.Contains(loreId);

        public bool HasProducer(string loreId) => _producerByLoreId.ContainsKey(loreId);

        public string? GetProducer(string loreId)
            => _producerByLoreId.TryGetValue(loreId, out var p) ? p : null;

        public IReadOnlyList<OralLoreHeardEntry> HeardSongs()
        {
            return _state.heardLoreIds
                .Select(id => new OralLoreHeardEntry(
                    id,
                    _producerByLoreId.TryGetValue(id, out var p) ? p : string.Empty,
                    _dayFirstHeard.TryGetValue(id, out var d) ? d : 0))
                .ToList();
        }

        /// <summary>Pieces with no producer — the explicit deferred list (§9).</summary>
        public IReadOnlyList<string> DeferredLoreIds()
        {
            return _catalog.AllSongs
                .Where(s => !_producerByLoreId.ContainsKey(s.lore_id))
                .Select(s => s.lore_id)
                .ToList();
        }

        // ── Discovery (idempotent, fail-closed) ─────────────────────────

        /// <summary>
        /// First-hears all pieces assigned to a producer context. Returns the
        /// NEWLY heard lore ids in deterministic ordinal order; repeat calls
        /// return empty (no rediscovery spam — revisits are silent).
        /// </summary>
        public IReadOnlyList<string> DiscoverFromProducer(string producerId, int day)
        {
            var newlyHeard = new List<string>();
            if (string.IsNullOrWhiteSpace(producerId)
                || !_loreIdsByProducer.TryGetValue(producerId, out var loreIds))
            {
                return newlyHeard;
            }

            foreach (var loreId in loreIds.OrderBy(x => x, StringComparer.Ordinal))
            {
                if (DiscoverSong(loreId, producerId, day)) newlyHeard.Add(loreId);
            }
            return newlyHeard;
        }

        /// <summary>First-hears one piece. Returns true only on first discovery.</summary>
        public bool DiscoverSong(string loreId, string producerId, int day)
        {
            if (string.IsNullOrWhiteSpace(loreId) || _catalog.GetById(loreId) == null) return false;
            if (_state.heardLoreIds.Contains(loreId)) return false;

            _state.heardLoreIds.Add(loreId);
            _state.heardLoreIds.Sort(StringComparer.Ordinal);
            _dayFirstHeard[loreId] = day;
            OnSongFirstHeard?.Invoke(loreId, producerId, day);
            OnStateChanged?.Invoke();
            return true;
        }

        // ── Persistence (§15) ───────────────────────────────────────────

        public OralLoreDiscoveryState CaptureState()
        {
            var copy = new OralLoreDiscoveryState
            {
                heardLoreIds = new List<string>(_state.heardLoreIds)
            };
            copy.heardLoreIds.Sort(StringComparer.Ordinal);
            return copy;
        }

        public void RestoreState(OralLoreDiscoveryState? saved)
        {
            if (saved == null)
            {
                _state = new OralLoreDiscoveryState();
                _dayFirstHeard.Clear();
                OnStateChanged?.Invoke();
                return;
            }

            // Tolerant restore: unknown future lore ids preserved inertly.
            _state = new OralLoreDiscoveryState
            {
                heardLoreIds = new List<string>(saved.heardLoreIds ?? new List<string>())
            };
            _state.heardLoreIds.RemoveAll(string.IsNullOrEmpty);
            _state.heardLoreIds.Sort(StringComparer.Ordinal);
            OnStateChanged?.Invoke();
        }
    }
}
