using System;
using System.Collections.Generic;

namespace Ashfall.Core.Muster
{
    /// <summary>Serialized state of the Coastal Hydro-Barons (Section II, "The Rate
    /// Card War") — faction_hydro_barons, the fifteenth Current at the three plants.</summary>
    public class HydroBaronsState
    {
        public string systemId = HydroBaronsSystem.SystemId;
        public bool isActive;
        public int queuePosition;            // the player's chit position on the rate card
        public float trust;
        public bool rateCardRevised;         // fixed published price (Approach A/B/D)
        public bool plantSeized;             // Approach C — player runs Unit 4
        public bool adminReform;             // transparent regulated pricing (Approach B)
        public string approach = string.Empty; // A..D
    }

    /// <summary>
    /// Engine-agnostic state machine for faction_hydro_barons (Section II). Mirrors
    /// the contract/ledger shape of the Tally to track the queue-chit economy.
    /// Approach A/D fix the card (rateCardRevised), B imposes audited transparent
    /// pricing (adminReform), C seizes the plant (plantSeized). No engine refs.
    /// </summary>
    public class HydroBaronsSystem
    {
        public const string SystemId = "hydro_barons_system";
        public const int MaxQueuePosition = 100;

        private readonly HydroBaronsState _state;

        public event Action<HydroBaronsState> OnStateChanged;
        public event Action<string> OnApproachResolved; // "A".."D"

        public HydroBaronsSystem(HydroBaronsState state = null)
        {
            _state = state ?? new HydroBaronsState();
            if (_state.systemId != SystemId) _state.systemId = SystemId;
        }

        public HydroBaronsState State => _state;
        public int QueuePosition => _state.queuePosition;
        public bool RateCardRevised => _state.rateCardRevised;
        public bool PlantSeized => _state.plantSeized;
        public bool AdminReform => _state.adminReform;
        public string ChosenApproach => _state.approach;

        /// <summary>Move up the queue by paying in fittings/coolant/labor (Stage 1).</summary>
        public bool AdvanceQueue(int positions)
        {
            if (positions <= 0 || _state.approach.Length > 0) return false;
            _state.queuePosition = Math.Max(0, Math.Min(MaxQueuePosition, _state.queuePosition + positions));
            _state.trust += positions * 0.25f;
            RaiseChanged();
            return true;
        }

        /// <summary>Resolve the Rate Card War via one of four Approaches (Section II Stage 3).</summary>
        public bool ResolveApproach(QuestApproach approach)
        {
            if (_state.approach.Length > 0) return false;
            _state.approach = approach.ToString();
            switch (approach)
            {
                case QuestApproach.A: // Undercut — flooded brine-salt substitute
                    _state.rateCardRevised = true;
                    _state.trust += 6f;
                    break;
                case QuestApproach.B: // Audit — Cold Count instruments
                    _state.adminReform = true;
                    _state.rateCardRevised = true;
                    _state.trust += 8f;
                    break;
                case QuestApproach.C: // Seize — player runs the plant
                    _state.plantSeized = true;
                    _state.queuePosition = 0;     // the queue is destroyed in the process
                    break;
                case QuestApproach.D: // Broker — three-way rotation via the Tally
                    _state.rateCardRevised = true;
                    _state.trust += 6f;
                    break;
            }
            OnApproachResolved?.Invoke(_state.approach);
            RaiseChanged();
            return true;
        }

        /// <summary>Whether the player's iron chit is a live currency (seized plant) or a
        /// collector's relic (reformed/fixed card).</summary>
        public bool QueueChitIsLiveCurrency => _state.plantSeized;

        public void Activate() { _state.isActive = true; RaiseChanged(); }

        // ── Save / Load ────────────────────────────────────────────────

        public HydroBaronsState CaptureState() => new HydroBaronsState
        {
            systemId = _state.systemId,
            isActive = _state.isActive,
            queuePosition = _state.queuePosition,
            trust = _state.trust,
            rateCardRevised = _state.rateCardRevised,
            plantSeized = _state.plantSeized,
            adminReform = _state.adminReform,
            approach = _state.approach
        };

        public void RestoreState(HydroBaronsState saved)
        {
            if (saved == null) return;
            _state.systemId = SystemId;
            _state.isActive = saved.isActive;
            _state.queuePosition = Math.Max(0, Math.Min(MaxQueuePosition, saved.queuePosition));
            _state.trust = Math.Max(0f, saved.trust);
            _state.rateCardRevised = saved.rateCardRevised;
            _state.plantSeized = saved.plantSeized;
            _state.adminReform = saved.adminReform;
            _state.approach = saved.approach ?? string.Empty;
            RaiseChanged();
        }

        private void RaiseChanged() => OnStateChanged?.Invoke(_state);
    }
}
