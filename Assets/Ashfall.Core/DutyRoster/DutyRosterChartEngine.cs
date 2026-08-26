using System;
using System.Collections.Generic;

namespace Ashfall.Core
{
    /// <summary>
    /// Chart lifecycle sub-engine for ASHFALL: THE DUTY ROSTER.
    /// Owns row creation/erasure, chart script transitions, morning tick,
    /// status/script mutations, and ink ending resolution. Extracted from
    /// DutyRosterSystem to reduce the god class without changing public behavior.
    /// Spec: docs/expansions/expansion_02_the_duty_roster_plan.md §5.1.
    /// </summary>
    internal class DutyRosterChartEngine
    {
        private DutyRosterSystemState _state;
        private readonly Dictionary<string, DutyRosterRow> _byId;
        private readonly HashSet<string> _blankRowsLiving;
        private readonly DutyRosterAssignmentEngine _assignments;
        private readonly Action _raiseUpdated;
        private readonly Action<string> _onNameWritten;
        private readonly Action<string> _onNameErased;
        private readonly Action _onRosterBurned;
        private readonly Action _withdrawBlankRowsAccess;

        public DutyRosterChartEngine(
            Dictionary<string, DutyRosterRow> byId,
            HashSet<string> blankRowsLiving,
            DutyRosterAssignmentEngine assignments,
            Action raiseUpdated,
            Action<string> onNameWritten,
            Action<string> onNameErased,
            Action onRosterBurned,
            Action withdrawBlankRowsAccess)
        {
            _byId = byId;
            _blankRowsLiving = blankRowsLiving;
            _assignments = assignments;
            _raiseUpdated = raiseUpdated;
            _onNameWritten = onNameWritten;
            _onNameErased = onNameErased;
            _onRosterBurned = onRosterBurned;
            _withdrawBlankRowsAccess = withdrawBlankRowsAccess;
        }

        public void Bind(DutyRosterSystemState state)
        {
            _state = state;
        }

        public DutyRosterRow GetRow(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return null;
            _byId.TryGetValue(survivorId, out DutyRosterRow row);
            return row;
        }

        public bool WriteName(
            string survivorId,
            string displayName,
            string occupationObserved,
            string script,
            int day,
            bool sleptHere)
        {
            if (!_state.expansionUnlocked) return false;
            if (_state.chartScript == DutyRosterIds.ScriptBurned || _state.mutationRosterBurned) return false;
            if (string.IsNullOrEmpty(survivorId)) return false;
            if (script != DutyRosterIds.ScriptPencil && script != DutyRosterIds.ScriptInk) return false;

            if (_blankRowsLiving.Contains(survivorId))
            {
                // Kess will not pencil a Blank Rows name. Ink is how the hatch at 11 goes dark.
                if (script != DutyRosterIds.ScriptInk) return false;
            }

            if (!sleptHere && script == DutyRosterIds.ScriptPencil)
                return false;

            DutyRosterRow existing = GetRow(survivorId)!;
            if (existing == null && _state.rows.Count >= DutyRosterIds.ManifestCap)
                return false;

            if (existing == null)
            {
                existing = new DutyRosterRow { survivorId = survivorId, status = DutyRosterIds.StatusHome };
                _state.rows.Add(existing);
                _byId[survivorId] = existing;
            }

            existing.displayName = displayName ?? string.Empty;
            existing.occupationObserved = occupationObserved ?? string.Empty;
            existing.script = script;
            if (sleptHere) existing.lastSleptDay = day;
            if (string.IsNullOrEmpty(existing.status)) existing.status = DutyRosterIds.StatusHome;

            if (script == DutyRosterIds.ScriptInk && _blankRowsLiving.Contains(survivorId))
                _withdrawBlankRowsAccess();

            _onNameWritten?.Invoke(survivorId);
            _raiseUpdated();
            return true;
        }

        public bool EraseName(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return false;
            DutyRosterRow row = GetRow(survivorId)!;
            if (row == null) return false;

            _state.rows.Remove(row);
            _byId.Remove(survivorId);
            _assignments.RemoveAssignmentsFor(survivorId);
            _onNameErased?.Invoke(survivorId);
            _raiseUpdated();
            return true;
        }

        public bool BurnChart(int day)
        {
            if (_state.mutationRosterBurned) return false;
            _state.chartScript = DutyRosterIds.ScriptBurned;
            _state.mutationRosterBurned = true;
            _state.mutationRosterInUse = false;
            _state.kessPencilAllowed = false;
            _state.waitInk = false;
            _state.lastBurnDay = day;
            _state.rows.Clear();
            _byId.Clear();
            _assignments.ClearAll();
            _onRosterBurned?.Invoke();
            _raiseUpdated();
            return true;
        }

        /// <summary>Morning tick. Kess fills pencil if allowed. Ink never auto-fills.</summary>
        public void TickMorning(int day, IReadOnlyList<DutyRosterOccupant> occupants)
        {
            if (!_state.expansionUnlocked) return;
            if (_state.chartScript == DutyRosterIds.ScriptBurned) return;

            if (_state.kessPencilAllowed && _state.chartScript == DutyRosterIds.ScriptPencil)
            {
                if (occupants != null)
                {
                    for (int i = 0; i < occupants.Count; i++)
                    {
                        DutyRosterOccupant occ = occupants[i];
                        if (occ == null || string.IsNullOrEmpty(occ.survivorId) || !occ.sleptHere)
                            continue;
                        DutyRosterRow row = GetRow(occ.survivorId)!;
                        if (row == null)
                            WriteName(occ.survivorId, occ.displayName, occ.occupationObserved, DutyRosterIds.ScriptPencil, day, true);
                        else
                            row.lastSleptDay = day;
                    }
                }
            }
            else if (_state.chartScript == DutyRosterIds.ScriptBlank && !_state.waitInk && !_state.kessPencilAllowed)
            {
                _state.daysLeftBlank++;
                if (_state.daysLeftBlank >= DutyRosterIds.StillBlankDays)
                    _state.mutationRosterStillBlank = true;
            }

            _state.lastMorningDay = day;
            _raiseUpdated();
        }

        public bool ResolveChartChoice(string choiceId, int day)
        {
            if (!_state.expansionUnlocked) return false;
            if (_state.chartScript == DutyRosterIds.ScriptBurned) return false;
            if (string.IsNullOrEmpty(choiceId)) return false;

            if (choiceId == DutyRosterIds.ChoiceWritePencil)
            {
                _state.kessPencilAllowed = true;
                _state.waitInk = false;
                _state.chartScript = DutyRosterIds.ScriptPencil;
                _state.mutationRosterInUse = true;
                _state.mutationRosterStillBlank = false;
                _state.daysLeftBlank = 0;
                _raiseUpdated();
                return true;
            }

            if (choiceId == DutyRosterIds.ChoiceLeaveBlank)
            {
                _state.kessPencilAllowed = false;
                _state.waitInk = false;
                _state.chartScript = DutyRosterIds.ScriptBlank;
                _state.daysLeftBlank = 0;
                _raiseUpdated();
                return true;
            }

            if (choiceId == DutyRosterIds.ChoiceWaitInk)
            {
                _state.kessPencilAllowed = false;
                _state.waitInk = true;
                _state.chartScript = DutyRosterIds.ScriptBlank;
                _raiseUpdated();
                return true;
            }

            return false;
        }

        public bool ResolveLadleChoice(string choiceId, int day)
        {
            if (!_state.expansionUnlocked) return false;
            if (string.IsNullOrEmpty(choiceId)) return false;
            if (choiceId == DutyRosterIds.ChoiceLadleProtocol)
            {
                _state.mutationRationProtocol = true;
                _raiseUpdated();
                return true;
            }

            if (choiceId == DutyRosterIds.ChoiceLadleChild || choiceId == DutyRosterIds.ChoiceLadleHatch || choiceId == DutyRosterIds.ChoiceLadleLeave)
            {
                _raiseUpdated();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Ink ending resolution (spec §3 Endings + §4.1 quest_roster_ink).
        /// The wall has names that do not come off in the morning. Edor's return
        /// is current. 11 goes dark if their living is on it. The hatch reversed
        /// reads your list.
        /// </summary>
        public bool ResolveInkEnding(int day)
        {
            if (!_state.expansionUnlocked) return false;
            if (_state.mutationRosterBurned) return false;
            if (_state.chartScript == DutyRosterIds.ScriptBurned) return false;

            _state.chartScript = DutyRosterIds.ScriptInk;
            _state.mutationRosterInUse = true;
            _state.mutationRosterStillBlank = false;
            _state.daysLeftBlank = 0;
            _state.kessPencilAllowed = false;
            _state.waitInk = false;
            _state.endingId = DutyRosterIds.EndingInk;
            _raiseUpdated();
            return true;
        }

        public bool SetStatus(string survivorId, string status)
        {
            DutyRosterRow row = GetRow(survivorId);
            if (row == null) return false;
            if (!IsKnownStatus(status)) return false;
            row.status = status;
            if (status == DutyRosterIds.StatusDead || status == DutyRosterIds.StatusQuiet || status == DutyRosterIds.StatusMissing
                || status == DutyRosterIds.StatusLevy || status == DutyRosterIds.StatusWaystation)
            {
                _assignments.RemoveAssignmentsFor(survivorId);
            }

            _raiseUpdated();
            return true;
        }

        public bool SetRowScript(string survivorId, string script)
        {
            DutyRosterRow row = GetRow(survivorId);
            if (row == null) return false;
            if (script != DutyRosterIds.ScriptPencil && script != DutyRosterIds.ScriptInk && script != DutyRosterIds.ScriptBlank) return false;
            if (_blankRowsLiving.Contains(survivorId) && script == DutyRosterIds.ScriptPencil)
                return false;
            row.script = script;
            if (script == DutyRosterIds.ScriptInk)
            {
                _state.chartScript = DutyRosterIds.ScriptInk;
                if (_blankRowsLiving.Contains(survivorId))
                    _withdrawBlankRowsAccess();
            }

            _raiseUpdated();
            return true;
        }

        private static bool IsKnownStatus(string status)
        {
            return status == DutyRosterIds.StatusHome || status == DutyRosterIds.StatusLevy || status == DutyRosterIds.StatusWaystation
                || status == DutyRosterIds.StatusQuiet || status == DutyRosterIds.StatusMissing || status == DutyRosterIds.StatusDead;
        }
    }
}
