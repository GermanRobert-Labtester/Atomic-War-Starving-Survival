using System;

namespace Ashfall.Core.Muster
{
    /// <summary>Serialized state of the Deserter Coalition holding ground.</summary>
    public class CoalitionCampState
    {
        public string systemId = CoalitionCampSystem.SystemId;
        public bool formed;
        public int formedDay;
        public int membersRallied;
        public string chosenStrategy = string.Empty;
        public int garrisonLockoutRisk;
        public string holdingGroundId = CoalitionCampSystem.HoldingGroundId;
        public bool vaskWithCamp = true;
    }

    /// <summary>
    /// Section VI.2/VI.4 state machine: the Deserter Coalition's holding ground
    /// at loc_denial_cut_substation. Tracks members rallied, the chosen campaign
    /// strategy (which shapes every beat to Day 320), and the Garrison lockout
    /// risk. Engine-agnostic; hosts only present it.
    /// </summary>
    public class CoalitionCampSystem
    {
        public const string SystemId = "coalition_camp_system";
        public const string HoldingGroundId = "loc_denial_cut_substation";
        public const int BaseMembers = 9;

        private readonly CoalitionCampState _state;

        public event Action<CoalitionCampState> OnStateChanged;
        public event Action<int> OnCampFormed;          // formedDay
        public event Action<string> OnStrategySet;      // "A".."D"
        public event Action<int> OnLockoutShifted;      // delta

        public CoalitionCampSystem(CoalitionCampState state = null)
        {
            _state = state ?? new CoalitionCampState();
            if (_state.systemId != SystemId) _state.systemId = SystemId;
        }

        public CoalitionCampState State => _state;
        public bool Formed => _state.formed;
        public int MembersRallied => _state.membersRallied;
        public string ChosenStrategy => _state.chosenStrategy;
        public int GarrisonLockoutRisk => _state.garrisonLockoutRisk;
        public bool VaskWithCamp => _state.vaskWithCamp;

        // ── Lifecycle ──────────────────────────────────────────────────

        /// <summary>The camp forms once, only once the Muster is open (Day 260+).</summary>
        public bool Form(int day)
        {
            if (_state.formed) return false;
            if (day < MusterSystem.MusterOpeningDay) return false;
            _state.formed = true;
            _state.formedDay = day;
            _state.membersRallied = BaseMembers;
            _state.vaskWithCamp = true;
            OnCampFormed?.Invoke(day);
            RaiseChanged();
            return true;
        }

        public bool RallyDeserter()
        {
            if (!_state.formed) return false;
            _state.membersRallied++;
            RaiseChanged();
            return true;
        }

        // ── Strategy (Section VI.4) ────────────────────────────────────

        /// <summary>
        /// Adopt a standing campaign strategy. Effects per the design bible:
        /// B escalates the Garrison counter-raid risk; C quiets the ground;
        /// D — the informant's price — zeroes the lockout risk and costs the
        /// rallied members, Vask included. A single strategy may be chosen.
        /// </summary>
        public bool SetStrategy(QuestApproach strategy)
        {
            if (!_state.formed) return false;
            if (!string.IsNullOrEmpty(_state.chosenStrategy)) return false;

            _state.chosenStrategy = strategy.ToString();
            int delta = 0;
            switch (strategy)
            {
                case QuestApproach.A:
                    delta = -5;
                    break;
                case QuestApproach.B:
                    delta = 15;
                    break;
                case QuestApproach.C:
                    delta = -10;
                    _state.membersRallied = Math.Max(0, _state.membersRallied - 3);
                    break;
                case QuestApproach.D:
                    delta = -_state.garrisonLockoutRisk;
                    _state.membersRallied = 0;
                    _state.vaskWithCamp = false;
                    break;
            }
            _state.garrisonLockoutRisk = Math.Max(0, Math.Min(100, _state.garrisonLockoutRisk + delta));
            OnStrategySet?.Invoke(_state.chosenStrategy);
            if (delta != 0) OnLockoutShifted?.Invoke(delta);
            RaiseChanged();
            return true;
        }

        // ── Save / Load ────────────────────────────────────────────────

        public CoalitionCampState CaptureState()
        {
            return new CoalitionCampState
            {
                systemId = _state.systemId,
                formed = _state.formed,
                formedDay = _state.formedDay,
                membersRallied = _state.membersRallied,
                chosenStrategy = _state.chosenStrategy,
                garrisonLockoutRisk = _state.garrisonLockoutRisk,
                holdingGroundId = _state.holdingGroundId,
                vaskWithCamp = _state.vaskWithCamp
            };
        }

        public void RestoreState(CoalitionCampState saved)
        {
            if (saved == null) return;
            _state.systemId = SystemId;
            _state.formed = saved.formed;
            _state.formedDay = saved.formedDay;
            _state.membersRallied = Math.Max(0, saved.membersRallied);
            _state.chosenStrategy = saved.chosenStrategy ?? string.Empty;
            _state.garrisonLockoutRisk = Math.Max(0, Math.Min(100, saved.garrisonLockoutRisk));
            _state.holdingGroundId = string.IsNullOrEmpty(saved.holdingGroundId)
                ? HoldingGroundId : saved.holdingGroundId;
            _state.vaskWithCamp = saved.vaskWithCamp;
            RaiseChanged();
        }

        private void RaiseChanged() => OnStateChanged?.Invoke(_state);
    }
}
