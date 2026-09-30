// SPDX-License-Identifier: MIT
// ============================================================================
// ASHFALL Patrol Radio Hooks — host wiring over the sealed
// Ashfall.Core.Radio.PatrolRadioHooks bridge.
//
// The travel encounter owner keeps raising patrol choices; the radio owner keeps
// the intercept log. This host only subscribes the one and drains the queue into
// the other on the canonical day tick.
// ============================================================================

using Ashfall.Core;
using Ashfall.Core.Narrative;
using Ashfall.Core.Radio;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private PatrolRadioHostSession? _patrolRadio;
        private bool _patrolRadioDirty;
        private TravelEncounterSystem? _patrolRadioSubscribed;

        public PatrolRadioHostSession? PatrolRadio => _patrolRadio;

        public void SetupPatrolRadio()
        {
            if (_patrolRadio != null) return;

            var saved = PatrolRadioSaveStore.TryLoad();
            _patrolRadio = new PatrolRadioHostSession(() => _radio, saved);
            _patrolRadio.StateChanged += () => _patrolRadioDirty = true;
        }

        /// <summary>
        /// Subscribes the LIVE travel encounter owner exactly once. Re-invoking
        /// after a reset re-subscribes; the bridge itself is idempotent.
        /// </summary>
        public void BindPatrolRadioToTravel(TravelEncounterSystem? travelSystem)
        {
            if (travelSystem == null) return;
            SetupPatrolRadio();
            if (_patrolRadio == null) return;
            if (ReferenceEquals(_patrolRadioSubscribed, travelSystem)) return;
            _patrolRadio.Subscribe(travelSystem);
            _patrolRadioSubscribed = travelSystem;
        }

        /// <summary>
        /// Canonical day-owner body: drain the queued patrol signals into the
        /// radio owner's intercept log. Returns the number delivered.
        /// </summary>
        public int TickPatrolRadio()
        {
            if (_patrolRadio == null) return 0;
            var dispatched = _patrolRadio.DispatchPending();
            return dispatched.Count;
        }

        public string QueuePatrolRadioSignal(string broadcastId)
        {
            SetupPatrolRadio();
            if (_patrolRadio == null) return "patrol radio bridge unavailable";
            bool queued = _patrolRadio.QueueSignal(broadcastId);
            return queued ? $"Queued patrol radio signal '{broadcastId}'." : $"Signal '{broadcastId}' already handled.";
        }

        public string PatrolRadioStatusLine()
        {
            if (_patrolRadio == null) return "bridge unbound";
            return _patrolRadio.PendingCount > 0
                ? $"{_patrolRadio.PendingCount} patrol signal(s) queued"
                : "no patrol signals queued";
        }

        public void SavePatrolRadioHooks()
        {
            if (_patrolRadio == null) return;
            var state = _patrolRadio.CaptureState();
            if (CaptureSection(PatrolRadioSaveStore.SectionName, PatrolRadioSaveStore.TryCapturePersisted(state)))
                _patrolRadioDirty = false;
        }

        public void FlushPatrolRadioIfDirty()
        {
            if (_patrolRadioDirty) SavePatrolRadioHooks();
        }

        public void ResetPatrolRadioHooks()
        {
            if (_patrolRadio != null && _patrolRadioSubscribed != null)
                _patrolRadio.Unsubscribe(_patrolRadioSubscribed);
            _patrolRadioSubscribed = null;
            _patrolRadio = null;
            _patrolRadioDirty = false;
        }
    }
}
