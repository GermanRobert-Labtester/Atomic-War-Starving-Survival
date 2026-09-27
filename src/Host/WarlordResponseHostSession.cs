// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : WarlordResponseSaveStore
// Core State : Ashfall.Core.Warlords.WarlordResponseState
// Host Caller: Main.WarlordResponse
// Purpose    : One-shot warlord tribute responses (Pay / Contest / Submit) per
//              canonical tribute id. WarlordDoctrineSystem stays the sole
//              authority for ask escalation, reliability, and standing.
// ============================================================================

using System;
using Ashfall.Core;
using Ashfall.Core.Save;
using Ashfall.Core.Warlords;

namespace AtomicWar.GodotApp
{
    public static class WarlordResponseSaveStore
    {
        public const string FileName = "warlord_response_save.json";
        public const string SectionName = "warlord_response";

        private static readonly SaveStore<WarlordResponseState> s_store =
            SaveStoreHub.Checksummed<WarlordResponseState>(FileName, nameof(WarlordResponseSaveStore));

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCapturePersisted(WarlordResponseState state) => s_store.CaptureBare(state);
        public static WarlordResponseState? TryRestorePersisted(string json) => s_store.RestoreBare(json);
        public static bool TrySave(WarlordResponseState state) => s_store.TrySave(state);
        public static WarlordResponseState? TryLoad() => s_store.TryLoad();
    }

    /// <summary>
    /// Host session over the sealed <see cref="WarlordResponseActions"/> command
    /// surface and the LIVE <see cref="WarlordDoctrineSystem"/>.
    ///
    /// <para><b>Authority boundary.</b> Ask escalation, reliability, and standing
    /// stay with <c>WarlordDoctrineSystem</c>. This session owns only which
    /// responses have already been issued for a given tribute.</para>
    ///
    /// <para><b>Determinism.</b> The canonical tribute id is derived from the live
    /// doctrine state (<c>totalWeeksAsked</c>) — no second tribute ledger and no
    /// wall-clock source.</para>
    /// </summary>
    public sealed class WarlordResponseHostSession : HostSessionBase
    {
        private readonly Func<WarlordDoctrineSystem?> _doctrineProvider;
        private WarlordResponseState _state = new WarlordResponseState();

        /// <summary>Raised after any accepted response record is appended.</summary>
        public event Action<WarlordResponseRecord>? OnResponded;

        public string LastEvent { get; private set; } = string.Empty;
        public WarlordResponseState State => _state;
        public int ResponseCount => _state.Responses.Count;

        public WarlordResponseHostSession(
            Func<WarlordDoctrineSystem?> doctrineProvider,
            WarlordResponseState? savedState = null)
        {
            _doctrineProvider = doctrineProvider ?? throw new ArgumentNullException(nameof(doctrineProvider));
            if (savedState != null) _state = savedState;
        }

        /// <summary>
        /// The canonical tribute id for the ask the doctrine owner currently has
        /// outstanding. Derived, never stored twice.
        /// </summary>
        public string CurrentTributeId()
        {
            var doctrine = _doctrineProvider();
            if (doctrine == null) return "tribute_week_0";
            return TributeIdForWeek(doctrine.State.totalWeeksAsked);
        }

        public static string TributeIdForWeek(int week) => $"tribute_week_{week}";

        private WarlordResponseActions? BuildActions()
        {
            var doctrine = _doctrineProvider();
            return doctrine == null ? null : new WarlordResponseActions(_state, doctrine);
        }

        public bool IsResponded(string tributeId)
        {
            for (int i = 0; i < _state.Responses.Count; i++)
                if (_state.Responses[i] != null && _state.Responses[i].TributeId == tributeId) return true;
            return false;
        }

        public bool HasRespondedToCurrentAsk() => IsResponded(CurrentTributeId());

        public WarlordResponseResult Pay(int amountPaid, int day, string? tributeId = null)
        {
            var actions = BuildActions();
            if (actions == null) return WarlordResponseResult.Fail("doctrine_unavailable");
            var result = actions.Pay(tributeId ?? CurrentTributeId(), amountPaid, day);
            Finish(result, "paid");
            return result;
        }

        public WarlordResponseResult Contest(int day, string? tributeId = null)
        {
            var actions = BuildActions();
            if (actions == null) return WarlordResponseResult.Fail("doctrine_unavailable");
            var result = actions.Contest(tributeId ?? CurrentTributeId(), day);
            Finish(result, "contested");
            return result;
        }

        public WarlordResponseResult Submit(int day, string? tributeId = null)
        {
            var actions = BuildActions();
            if (actions == null) return WarlordResponseResult.Fail("doctrine_unavailable");
            var result = actions.Submit(tributeId ?? CurrentTributeId(), day);
            Finish(result, "submitted");
            return result;
        }

        private void Finish(WarlordResponseResult result, string verb)
        {
            if (result == null || !result.Succeeded) return;
            LastEvent = $"Tribute {result.Record.TributeId} {verb} on day {result.Record.Day}.";
            OnResponded?.Invoke(result.Record);
            RaiseStateChanged();
        }

        /// <summary>
        /// Drops response records for tribute ids that are no longer current, so a
        /// new ask is never blocked by last week's already-issued response. The
        /// live doctrine state is the only source of "which week is current".
        /// </summary>
        public int PruneSuperseded(int currentWeek)
        {
            string current = TributeIdForWeek(currentWeek);
            int removed = _state.Responses.RemoveAll(r =>
                r == null || !string.Equals(r.TributeId, current, StringComparison.Ordinal));
            if (removed > 0)
            {
                LastEvent = $"Pruned {removed} superseded tribute response(s); ask {current} is open.";
                RaiseStateChanged();
            }
            return removed;
        }

        /// <summary>Truthful projection of the current ask's response state.</summary>
        public string StatusLine()
        {
            if (HasRespondedToCurrentAsk())
            {
                var rec = _state.Responses.Find(r => r != null && r.TributeId == CurrentTributeId());
                return rec == null
                    ? "responded"
                    : $"responded · {rec.Kind} · day {rec.Day}";
            }
            return "awaiting response";
        }

        public WarlordResponseState CaptureState()
        {
            var serializer = new SystemTextJsonSerializer();
            return serializer.Deserialize<WarlordResponseState>(serializer.Serialize(_state))
                ?? new WarlordResponseState();
        }

        public void RestoreState(WarlordResponseState? state)
        {
            if (state == null) return;
            var serializer = new SystemTextJsonSerializer();
            _state = serializer.Deserialize<WarlordResponseState>(serializer.Serialize(state))
                ?? new WarlordResponseState();
            LastEvent = "Tribute response ledger restored from save.";
            RaiseStateChanged();
        }

        public void Clear()
        {
            _state = new WarlordResponseState();
            LastEvent = "Tribute response ledger cleared.";
            RaiseStateChanged();
        }
    }
}
