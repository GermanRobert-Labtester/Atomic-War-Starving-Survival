using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core.Muster
{
    /// <summary>Serialized state of the Cold Count (Section V.1) — four researchers
    /// at loc_low_background_lab holding the isotopic provenance of who fired first.</summary>
    public class ColdCountState
    {
        public string systemId = ColdCountSystem.SystemId;
        public bool isActive;
        public int powerSuppliedDays;
        public int shieldingDelivered;
        public bool provenanceDataComplete;
        public bool broadcastSent;
        public int broadcastDay = -1;
        public float trust;

        public const int RequiredPowerDays = 30;
        public const int RequiredShieldingUnits = 4;
    }

    /// <summary>
    /// Engine-agnostic state machine for faction_cold_count (Section V.1). Ticks
    /// supply of power/shielding toward the provenance run, then transmits on
    /// 142.850 MHz ("The Measured Truth"). Supply is voluntarily stopped, never
    /// revoked — the Count do not falsify. No engine references.
    /// </summary>
    public class ColdCountSystem
    {
        public const string SystemId = "cold_count_system";

        private readonly ColdCountState _state;

        public event Action<ColdCountState> OnStateChanged;
        public event Action OnBroadcast;
        public event Action OnProvenanceComplete;

        public ColdCountSystem(ColdCountState state = null!)
        {
            _state = state ?? new ColdCountState();
            if (_state.systemId != SystemId) _state.systemId = SystemId;
        }

        public ColdCountState State => _state;
        public int PowerSuppliedDays => _state.powerSuppliedDays;
        public int ShieldingDelivered => _state.shieldingDelivered;
        public bool ProvenanceDataComplete => _state.provenanceDataComplete;
        public bool BroadcastSent => _state.broadcastSent;
        public bool IsActive => _state.isActive;

        /// <summary>Begin supplying power (the sustained Schedule path; the only
        /// route that allows the provenance run to finish before Day 300).</summary>
        public void SupplyPower(int days)
        {
            if (days <= 0) return;
            _state.powerSuppliedDays += days;
            _state.trust += days * 0.5f;
            TryCompleteProvenance();
            RaiseChanged();
        }

        /// <summary>Deliver lead/boron shielding tiles toward the safe-reading stockpile.</summary>
        public bool DeliverShielding(int qty)
        {
            if (qty <= 0) return false;
            _state.shieldingDelivered += qty;
            _state.trust += qty * 1f;
            TryCompleteProvenance();
            RaiseChanged();
            return true;
        }

        /// <summary>True once the equipment is calibrated (power+dose threshold).</summary>
        public bool CanCompleteProvenanceRun() =>
            _state.powerSuppliedDays >= ColdCountState.RequiredPowerDays &&
            _state.shieldingDelivered >= ColdCountState.RequiredShieldingUnits;

        /// <summary>Completes the provenance data set (if equipment allows). No
        /// falsification — a partial set stays partial until supply returns.</summary>
        public bool CompleteProvenanceRun()
        {
            if (_state.provenanceDataComplete) return false;
            if (!CanCompleteProvenanceRun()) return false;
            _state.provenanceDataComplete = true;
            _state.trust += 2f;
            OnProvenanceComplete?.Invoke();
            RaiseChanged();
            return true;
        }

        /// <summary>Transmit on 142.850 MHz. A complete data set broadcasts at full
        /// credibility; a partial one still fires but caveated. Fires once.</summary>
        public bool TransmitFindings(int day)
        {
            if (_state.broadcastSent) return false;
            _state.broadcastSent = true;
            _state.broadcastDay = day;
            OnBroadcast?.Invoke();
            RaiseChanged();
            return true;
        }

        public bool BroadcastIsCaveated => _state.broadcastSent && !_state.provenanceDataComplete;

        public void Activate() { _state.isActive = true; RaiseChanged(); }

        // ── Save / Load ────────────────────────────────────────────────

        public ColdCountState CaptureState()
        {
            return new ColdCountState
            {
                systemId = _state.systemId,
                isActive = _state.isActive,
                powerSuppliedDays = _state.powerSuppliedDays,
                shieldingDelivered = _state.shieldingDelivered,
                provenanceDataComplete = _state.provenanceDataComplete,
                broadcastSent = _state.broadcastSent,
                broadcastDay = _state.broadcastDay,
                trust = _state.trust
            };
        }

        public void RestoreState(ColdCountState saved)
        {
            if (saved == null) return;
            _state.systemId = SystemId;
            _state.isActive = saved.isActive;
            _state.powerSuppliedDays = Math.Max(0, saved.powerSuppliedDays);
            _state.shieldingDelivered = Math.Max(0, saved.shieldingDelivered);
            _state.provenanceDataComplete = saved.provenanceDataComplete;
            _state.broadcastSent = saved.broadcastSent;
            _state.broadcastDay = saved.broadcastDay;
            _state.trust = Math.Max(0f, saved.trust);
            RaiseChanged();
        }

        private void TryCompleteProvenance()
        {
            if (!_state.provenanceDataComplete && CanCompleteProvenanceRun())
            {
                _state.provenanceDataComplete = true;
                OnProvenanceComplete?.Invoke();
            }
        }

        private void RaiseChanged() => OnStateChanged?.Invoke(_state);
    }
}
