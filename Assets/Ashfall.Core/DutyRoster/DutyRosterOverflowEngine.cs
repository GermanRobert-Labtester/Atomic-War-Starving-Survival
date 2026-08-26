using System;
using System.Collections.Generic;

namespace Ashfall.Core
{
    /// <summary>
    /// Overflow practice sub-engine for ASHFALL: THE DUTY ROSTER.
    /// Owns the bounded authenticated-void state (access flag + visited ledger)
    /// and enforces the four-node whitelist. Extracted from DutyRosterSystem
    /// to reduce the god class without changing public behavior.
    /// Spec: docs/expansions/expansion_02_the_duty_roster_plan.md §2.4.
    /// </summary>
    internal class DutyRosterOverflowEngine
    {
        private DutyRosterSystemState _state;
        private readonly Action _raiseChanged;

        public bool Access => _state.overflowAccess;
        public IReadOnlyList<string> Visited => _state.overflowVisited;

        public DutyRosterOverflowEngine(Action raiseChanged)
        {
            _raiseChanged = raiseChanged;
        }

        public void Bind(DutyRosterSystemState state)
        {
            _state = state;
        }

        public bool GrantOverflowAccess()
        {
            if (_state.overflowAccess) return false;
            _state.overflowAccess = true;
            _raiseChanged();
            return true;
        }

        public bool WithdrawOverflowAccess()
        {
            if (!_state.overflowAccess) return false;
            _state.overflowAccess = false;
            _raiseChanged();
            return true;
        }

        /// <summary>Register a visit to one of the four authenticated Overflow nodes.</summary>
        public bool RegisterOverflowVisit(string nodeId)
        {
            if (!_state.overflowAccess) return false;
            if (!DutyRosterSystem.IsOverflowNode(nodeId)) return false;
            if (_state.overflowVisited.Contains(nodeId)) return false;
            _state.overflowVisited.Add(nodeId);
            _raiseChanged();
            return true;
        }

        public bool HasVisitedOverflow(string nodeId)
        {
            return !string.IsNullOrEmpty(nodeId) && _state.overflowVisited != null
                && _state.overflowVisited.Contains(nodeId);
        }

        public DutyRosterOverflowState Capture()
        {
            var copy = new DutyRosterOverflowState
            {
                access = _state.overflowAccess,
                visitedNodes = _state.overflowVisited != null
                    ? new List<string>(_state.overflowVisited)
                    : new List<string>()
            };
            return copy;
        }

        /// <summary>Restore the Overflow practice state. Missing state defaults to closed/empty.</summary>
        public void Restore(DutyRosterOverflowState saved)
        {
            if (saved == null) return;
            _state.overflowAccess = saved.access;
            _state.overflowVisited = saved.visitedNodes != null
                ? new List<string>(saved.visitedNodes)
                : new List<string>();
            // Never bless an unauthenticated node into the visited ledger.
            if (_state.overflowVisited != null)
                _state.overflowVisited.RemoveAll(n => !DutyRosterSystem.IsOverflowNode(n));
            _raiseChanged();
        }
    }
}
