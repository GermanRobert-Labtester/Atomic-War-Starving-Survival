// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : PatrolRadioSaveStore
// Core State : Ashfall.Core.Radio.PatrolRadioHooksState
// Host Caller: Main.PatrolRadio
// Purpose    : Queued and consumed patrol-encounter radio signals. The radio
//              owner keeps the intercept log; this section is only the
//              one-shot bridge queue between travel encounters and that log.
// ============================================================================

using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Narrative;
using Ashfall.Core.Radio;
using Ashfall.Core.Save;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public static class PatrolRadioSaveStore
    {
        public const string FileName = "patrol_radio_hooks_save.json";
        public const string SectionName = "patrol_radio_hooks";

        private static readonly SaveStore<PatrolRadioHooksState> s_store =
            SaveStoreHub.Checksummed<PatrolRadioHooksState>(FileName, nameof(PatrolRadioSaveStore));

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCapturePersisted(PatrolRadioHooksState state) => s_store.CaptureBare(state);
        public static PatrolRadioHooksState? TryRestorePersisted(string json) => s_store.RestoreBare(json);
        public static bool TrySave(PatrolRadioHooksState state) => s_store.TrySave(state);
        public static PatrolRadioHooksState? TryLoad() => s_store.TryLoad();
    }

    /// <summary>
    /// Host session over the sealed <see cref="PatrolRadioHooks"/> bridge.
    ///
    /// <para><b>Authority boundary.</b> The authored encounter→broadcast map and
    /// the faction radio-capability table stay in the Core bridge. The travel
    /// encounter owner keeps raising choices; the radio owner keeps the intercept
    /// log. This session only owns the one-shot queue between them.</para>
    /// </summary>
    public sealed class PatrolRadioHostSession : HostSessionBase
    {
        private readonly PatrolRadioHooks _hooks;
        private readonly Func<RadioHostSession?> _radioProvider;

        public string LastEvent { get; private set; } = string.Empty;
        public PatrolRadioHooks Hooks => _hooks;
        public IReadOnlyList<string> PendingSignals => _hooks.PendingSignals;
        public int PendingCount => _hooks.PendingCount;

        public PatrolRadioHostSession(
            Func<RadioHostSession?> radioProvider,
            PatrolRadioHooksState? savedState = null,
            ILog? log = null)
        {
            _radioProvider = radioProvider ?? throw new ArgumentNullException(nameof(radioProvider));
            _hooks = new PatrolRadioHooks(log);
            if (savedState != null) _hooks.RestoreState(savedState);
        }

        /// <summary>Subscribes the live travel-encounter owner.</summary>
        public bool Subscribe(TravelEncounterSystem? travelSystem)
        {
            if (travelSystem == null) return false;
            _hooks.Subscribe(travelSystem);
            LastEvent = "Patrol radio hooks subscribed to the travel encounter owner.";
            RaiseStateChanged();
            return true;
        }

        public void Unsubscribe(TravelEncounterSystem? travelSystem = null)
        {
            _hooks.Unsubscribe(travelSystem);
            LastEvent = "Patrol radio hooks unsubscribed.";
            RaiseStateChanged();
        }

        public static bool IsFactionRadioCapable(string factionId) => PatrolRadioHooks.IsFactionRadioCapable(factionId);

        public static bool TryGetRadioSignalForEncounter(string encounterIdOrGroup, out string radioBroadcastId)
            => PatrolRadioHooks.TryGetRadioSignalForEncounter(encounterIdOrGroup, out radioBroadcastId);

        public bool QueueSignal(string broadcastId) => _hooks.QueueSignal(broadcastId);

        /// <summary>
        /// Drains the queue and delivers every signal through the canonical radio
        /// owner's intercept log. Unknown broadcast ids are still consumed (the
        /// encounter happened) but produce no intercept.
        /// </summary>
        public IReadOnlyList<string> DispatchPending()
        {
            var dispatched = _hooks.TickRadio();
            if (dispatched.Count == 0) return dispatched;

            var radio = _radioProvider();
            int delivered = 0;
            foreach (string id in dispatched)
            {
                if (radio != null && radio.PlayFactionBroadcast(id, radio.Day)) delivered++;
            }
            LastEvent = $"Dispatched {dispatched.Count} patrol radio signal(s), {delivered} delivered to the intercept log.";
            RaiseStateChanged();
            return dispatched;
        }

        public PatrolRadioHooksState CaptureState() => _hooks.CaptureState();

        public void RestoreState(PatrolRadioHooksState? state)
        {
            if (state == null) return;
            _hooks.RestoreState(state);
            LastEvent = "Patrol radio queue restored from save.";
            RaiseStateChanged();
        }

        public void Clear()
        {
            _hooks.ResetForTest();
            LastEvent = "Patrol radio queue cleared.";
            RaiseStateChanged();
        }
    }
}
