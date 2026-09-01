using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core.Journal
{
    /// <summary>
    /// Canonical discovery keys for the diegetic journal / tutorial engine.
    /// snake_case ids only — never invent keys outside this list. The dynamic
    /// namespaced keys are always derived from a real master-list id.
    /// </summary>
    public static class KnowledgeKeys
    {
        public const string HighCo2 = "high_co2";
        public const string HasSeenRadiation = "has_seen_radiation";
        public const string HasExperiencedStorm = "has_experienced_storm";
        public const string FilterFailing = "filter_failing";
        public const string FreezingShelter = "freezing_shelter";

        // Expansion 06 — The Muster (world history, Section XI).
        public const string ContinuityReclamationDecree = "history_continuity_reclamation_decree";
        public const string HydroBaronRateCardOrigin = "history_hydro_baron_rate_card_origin";
        public const string DeserterCoalitionFounding = "history_deserter_coalition_founding";
        public const string ColdCountBeforeTheLab = "history_cold_count_before_the_lab";
        public const string ProvisionedAdvanceKnowledge = "history_the_provisioned_advance_knowledge";
        public const string CheckpointConscriptsConfession = "history_checkpoint_conscripts_confession";
        public const string QuartermastersPaperwork = "history_quartermasters_paperwork";
        public const string InterceptedCipher = "history_the_intercepted_cipher";
        public const string LedgerNobodySigned = "history_the_ledger_nobody_signed";

        public static readonly string[] All =
        {
            HighCo2,
            HasSeenRadiation,
            HasExperiencedStorm,
            FilterFailing,
            FreezingShelter,
            ContinuityReclamationDecree,
            HydroBaronRateCardOrigin,
            DeserterCoalitionFounding,
            ColdCountBeforeTheLab,
            ProvisionedAdvanceKnowledge,
            CheckpointConscriptsConfession,
            QuartermastersPaperwork,
            InterceptedCipher,
            LedgerNobodySigned
        };

        // -----------------------------------------------------------------
        // Journal codex unlock keys (docs/ui/JOURNAL_UI_PLAN.md §7).
        // Dynamic namespaced keys derived from master-list ids — the id side
        // is always a real item/location/survivor/event id, never invented.
        // -----------------------------------------------------------------

        /// <summary>"item_seen_" + itemId — first time an item def is revealed.</summary>
        public static string ItemSeen(string itemId) => "item_seen_" + itemId;

        /// <summary>"location_visited_" + locationId — a return from that node.</summary>
        public static string LocationVisited(string locationId) => "location_visited_" + locationId;

        /// <summary>"survivor_met_" + survivorId — a survivor joined the bunker.</summary>
        public static string SurvivorMet(string survivorId) => "survivor_met_" + survivorId;

        /// <summary>"event_fired_" + eventId — a narrative event triggered this run.</summary>
        public static string EventFired(string eventId) => "event_fired_" + eventId;

        /// <summary>"room_history_seen_" + vignetteId — a shelter room-history vignette was discovered (Plan 29 29A).</summary>
        public static string RoomHistorySeen(string vignetteId) => "room_history_seen_" + vignetteId;

        /// <summary>"glitch_noted_" + glitchId — a machine glitch event was observed/journalised (Plan 29 29B).</summary>
        public static string GlitchNoted(string glitchId) => "glitch_noted_" + glitchId;
    }

    /// <summary>
    /// Tracks what the player/survivors have already learned so each discovery
    /// only writes once. Save/load safe.
    /// </summary>
    [Serializable]
    public class KnowledgeBase
    {
        private readonly HashSet<string> _discovered = new HashSet<string>(StringComparer.Ordinal);

        public int Count => _discovered.Count;

        public bool Has(string key)
        {
            if (string.IsNullOrEmpty(key)) return false;
            return _discovered.Contains(key);
        }

        /// <summary>
        /// Mark a discovery. Returns true only the first time this key is learned.
        /// </summary>
        public bool Discover(string key)
        {
            if (string.IsNullOrEmpty(key)) return false;
            return _discovered.Add(key);
        }

        public void Clear() => _discovered.Clear();

        public IReadOnlyCollection<string> Snapshot()
        {
            return new List<string>(_discovered);
        }

        public KnowledgeBaseSave CaptureState()
        {
            // Ordinal-sorted: HashSet enumeration order is not a cross-host
            // guarantee, and the checksum walks this array.
            var keys = new string[_discovered.Count];
            _discovered.CopyTo(keys, 0);
            System.Array.Sort(keys, System.StringComparer.Ordinal);
            return new KnowledgeBaseSave { DiscoveredKeys = keys };
        }

        public void RestoreState(KnowledgeBaseSave save)
        {
            _discovered.Clear();
            if (save?.DiscoveredKeys == null) return;
            for (int i = 0; i < save.DiscoveredKeys.Length; i++)
            {
                string k = save.DiscoveredKeys[i];
                if (!string.IsNullOrEmpty(k))
                    _discovered.Add(k);
            }
        }
    }

    [Serializable]
    public class KnowledgeBaseSave
    {
        public string[] DiscoveredKeys;
    }
}
