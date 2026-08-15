using System;

namespace Ashfall.Core.Verdict
{
    /// <summary>
    /// ASHFALL: THE VERDICT (Expansion 08) — the three Reckoning phases.
    /// KNOWING → CULPABLE → COUNTED. Transitions are idempotent, driven by the
    /// sim clock + enrolled evidence + the machine's own census schedule, and
    /// persist through VerdictSave. Never reverses (Phase III back to Phase I
    /// is impossible by construction: we only move forward).
    /// </summary>
    public enum ReckoningPhase
    {
        Dormant = 0,
        Knowing = 1,   // Day 160+ — first maintenance log becomes readable
        Culpable = 2,  // Day 210+ — census carrier; the countdown; summit light
        Counted = 3    // Day 240+ — the Reckoning Call resolves; endings open
    }

    [Serializable]
    public sealed class ReckoningState
    {
        public ReckoningPhase phase = ReckoningPhase.Dormant;
        public int phaseChangedDay = -1;
        public bool carrierHeard;          // the 99.0 MHz pilot tone (one-shot)
        public bool callResolved;          // the Call fired (one-shot)
        public bool countPresented;        // PRESENT chosen
        public bool countHeld;             // HOLD chosen
        public bool offerIsLease;          // DISCHARGE chosen
        public int enrolledEvidence;
        public int driftDays = 3;          // the machine's clock disagrees with the wars' by 3 days
        // Chain 1 (Survivor drift / housing-collapse): count of dwellings that
        // did not answer census. Distinct from the lore evidence ledger.
        public int dwellingDriftTotal;     // running count of lost/withdrew/drifted/yielded dwellings
        public int lastDriftDay = -1;
        public int lastDriftDeltaToday;    // delta applied on lastDriftDay
        // Chain 3 (Survivor cumulative dose triggers Reckoning): aggregate Sv.
        public float cumulativeDoseSieverts;
        public bool highDosePromoted;      // one-shot auto-promote past Knowing from dose
    }

    /// <summary>
    /// Three-phase state machine for the Reckoning. Deterministic: transitions
    /// consult day thresholds (>= not ==), evidence, and previously-resolved
    /// flags; Poll(day, currentLivingCount, logReadCount) returns a list of
    /// event names fired this tick so hosts and tests can assert idempotency.
    /// </summary>
    public sealed class ReckoningSystem
    {
        public const int KnowingDay = 160;
        public const int CulpableDay = 210;
        public const int CountedDay = 240;
        public const int EvidenceCulpableGate = 1;   // at least one read entry to open CULPABLE early is allowed
        public const int ExpectedProvincialCount = 211004;
        // Chain 3 doctrine threshold: aggregate dose that forces Reckoning to
        // acknowledge early (CANON §Verdict doctrine §Meter). Crossing this
        // promotes Dormant → Knowing regardless of day. One-shot.
        public const float HighDoseKnowingThresholdSieverts = 4.0f;
        // Chain 3 secondary threshold: at this dose the phase must already be
        // Knowing or deeper; surviving dwellings cannot be a quiet majority.
        public const float HighDoseCulpableFloorSieverts = 8.0f;

        private readonly ReckoningState _state;

        public ReckoningState State => _state;
        public ReckoningPhase Phase => _state.phase;

        public event Action<ReckoningPhase> OnPhaseChanged;
        public event Action OnCarrierHeard;
        public event Action<int> OnReckoningCall;      // payload = observed living count
        public event Action<string> OnVerdictResolved; // payload = ending key

        public ReckoningSystem(ReckoningState state = null)
        {
            _state = state ?? new ReckoningState();
        }

        /// <summary>Step the state machine. Returns event names fired this tick (for tests/observability).</summary>
        public System.Collections.Generic.List<string> Poll(
            int day, int livingCount, int logReadCount, int evidenceCount)
        {
            var fired = new System.Collections.Generic.List<string>();

            if (_state.phase == ReckoningPhase.Dormant && day >= KnowingDay)
                SetPhase(day, ReckoningPhase.Knowing, fired);

            bool evidenceGate = _state.enrolledEvidence > 0 || evidenceCount > 0;
            if (_state.phase == ReckoningPhase.Knowing && day >= CulpableDay && evidenceGate)
            {
                SetPhase(day, ReckoningPhase.Culpable, fired);
                if (!_state.carrierHeard)
                {
                    _state.carrierHeard = true;
                    OnCarrierHeard?.Invoke();
                    fired.Add("carrier_heard");
                }
            }

            if (_state.phase == ReckoningPhase.Culpable && day >= CountedDay && !_state.callResolved)
            {
                _state.callResolved = true;
                _state.phaseChangedDay = day;
                _state.phase = ReckoningPhase.Counted;
                OnPhaseChanged?.Invoke(ReckoningPhase.Counted);
                OnReckoningCall?.Invoke(Math.Max(1, livingCount));
                fired.Add("reckoning_call");
            }

            return fired;
        }

        /// <summary>Enroll an evidence fragment (a read machine-log entry).</summary>
        public void EnrollEvidence(int amount = 1)
        {
            _state.enrolledEvidence += Math.Max(1, amount);
        }

        /// <summary>The census window is open when phase is Culpable+ and the carrier is scheduled.</summary>
        public bool IsCensusWindowOpen(int day)
            => _state.phase >= ReckoningPhase.Culpable && day >= CulpableDay;

        /// <summary>The drift the machine and the wars' calendar disagree by (canon: 3 days).</summary>
        public int ClockDriftDays => _state.driftDays;

        // ── Ending selection ────────────────────────────────────────────────────

        /// <summary>Choose an ending key. Mutually exclusive — only one may be set, once.</summary>
        public bool SelectEnding(string endingKey, int day)
        {
            if (string.IsNullOrEmpty(endingKey)) return false;
            if (_state.countPresented || _state.countHeld || _state.offerIsLease)
                return false; // already resolved
            if (_state.phase < ReckoningPhase.Counted)
                return false; // the count has not been presented yet

            switch (endingKey)
            {
                case "ending_verdict_the_sector_recounts":
                    _state.countPresented = true;
                    break;
                case "ending_verdict_the_count_is_held":
                    _state.countHeld = true;
                    break;
                case "ending_verdict_the_offer_is_a_lease":
                    _state.offerIsLease = true;
                    break;
                default:
                    return false;
            }
            OnVerdictResolved?.Invoke(endingKey);
            return true;
        }

        /// <summary>Null-safe copy of state for persistence.</summary>
        public ReckoningState CaptureState()
        {
            return new ReckoningState
            {
                phase = _state.phase,
                phaseChangedDay = _state.phaseChangedDay,
                carrierHeard = _state.carrierHeard,
                callResolved = _state.callResolved,
                countPresented = _state.countPresented,
                countHeld = _state.countHeld,
                offerIsLease = _state.offerIsLease,
                enrolledEvidence = _state.enrolledEvidence,
                driftDays = _state.driftDays,
                dwellingDriftTotal = _state.dwellingDriftTotal,
                lastDriftDay = _state.lastDriftDay,
                lastDriftDeltaToday = _state.lastDriftDeltaToday,
                cumulativeDoseSieverts = _state.cumulativeDoseSieverts,
                highDosePromoted = _state.highDosePromoted
            };
        }

        public void RestoreState(ReckoningState state)
        {
            if (state == null) return;
            _state.phase = state.phase;
            _state.phaseChangedDay = state.phaseChangedDay;
            _state.carrierHeard = state.carrierHeard;
            _state.callResolved = state.callResolved;
            _state.countPresented = state.countPresented;
            _state.countHeld = state.countHeld;
            _state.offerIsLease = state.offerIsLease;
            _state.enrolledEvidence = state.enrolledEvidence;
            _state.driftDays = state.driftDays > 0 ? state.driftDays : 3;
            _state.dwellingDriftTotal = state.dwellingDriftTotal;
            _state.lastDriftDay = state.lastDriftDay;
            _state.lastDriftDeltaToday = state.lastDriftDeltaToday;
            _state.cumulativeDoseSieverts = state.cumulativeDoseSieverts < 0f ? 0f : state.cumulativeDoseSieverts;
            _state.highDosePromoted = state.highDosePromoted;
        }

        private void SetPhase(int day, ReckoningPhase to, System.Collections.Generic.List<string> fired)
        {
            _state.phase = to;
            _state.phaseChangedDay = day;
            OnPhaseChanged?.Invoke(to);
            fired.Add("phase_" + to.ToString().ToLowerInvariant());
        }

        // ── Phase 6.D Chains 1 + 3: Verdict systemic hazards ──────────────

        /// <summary>
        /// Chain 1 (Census / Human Cost). Records a today-vs-yesterday delta of
        /// dwellings that did not answer — `count` is the number of survivors
        /// who dropped out of coverage today. Multiple deltas on the same day
        /// are summed; never negative; total grows monotonically.
        /// </summary>
        public void RecordDrift(int day, int count)
        {
            if (count <= 0) return;
            if (_state.lastDriftDay == day)
            {
                _state.lastDriftDeltaToday += count;
            }
            else
            {
                _state.lastDriftDay = day;
                _state.lastDriftDeltaToday = count;
            }
            _state.dwellingDriftTotal += count;
        }

        /// <summary>
        /// Chain 3 (Survival Reckoning). Today's aggregate dose in sieverts; if
        /// the cross-threshold has not been recorded yet AND cumulative dose
        /// crosses the doctrine limit, promotes Dormant → Knowing regardless
        /// of day. Idempotent across ticks: once <see cref="ReckoningState.highDosePromoted"/>
        /// is true, this method never re-promotes. The phase floor for
        /// <see cref="HighDoseCulpableFloorSieverts"/> is enforced after this
        /// method returns by whoever holds the host clock.
        /// </summary>
        public void RecordCumulativeDose(int day, float sieverts)
        {
            if (sieverts < 0f) sieverts = 0f;
            _state.cumulativeDoseSieverts = sieverts;
            if (!_state.highDosePromoted
                && _state.cumulativeDoseSieverts >= HighDoseKnowingThresholdSieverts)
            {
                _state.highDosePromoted = true;
                if (_state.phase == ReckoningPhase.Dormant)
                {
                    SetPhase(day, ReckoningPhase.Knowing, _emptyFired);
                }
            }
        }

        private static readonly System.Collections.Generic.List<string> _emptyFired =
            new System.Collections.Generic.List<string>(0);

        /// <summary>
        /// Read-only access to the running dwelling drift count (Chain 1
        /// readout line). Distinct from <see cref="ReckoningState.enrolledEvidence"/>.
        /// </summary>
        public int DwellingDriftTotal => _state.dwellingDriftTotal;
        public int LastDriftDeltaToday => _state.lastDriftDeltaToday;
        public int LastDriftDay => _state.lastDriftDay;
        public float CumulativeDoseSieverts => _state.cumulativeDoseSieverts;
        public bool HighDosePromoted => _state.highDosePromoted;
    }
}
