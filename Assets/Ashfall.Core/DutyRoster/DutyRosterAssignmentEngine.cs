// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.Survivors;

namespace Ashfall.Core
{
    /// <summary>
    /// Assignment sub-engine for ASHFALL: THE DUTY ROSTER.
    /// Owns role allocation, duplicate-role enforcement, deterministic auto-assign,
    /// and the save-safe assignment list sync. Extracted from DutyRosterSystem
    /// to reduce the god class without changing public behavior.
    /// Spec: docs/expansions/expansion_02_the_duty_roster_plan.md §5.1.
    /// </summary>
    internal class DutyRosterAssignmentEngine
    {
        private readonly Dictionary<string, string> _assignmentByRole;
        private readonly List<DutyRosterAssignmentEntry> _assignments;
        private readonly Func<string, DutyRosterRow> _getRow;
        private readonly Action _raiseUpdated;
        private readonly Action<string, string> _onAssignmentChanged;
        private readonly Action<string, string> _onDutyVacated;
        private readonly Func<int> _seedSaltProvider;
        private readonly Func<IReadOnlyList<DutyRosterRow>> _rowsProvider;

        public DutyRosterAssignmentEngine(
            Dictionary<string, string> assignmentByRole,
            List<DutyRosterAssignmentEntry> assignments,
            Func<string, DutyRosterRow> getRow,
            Action raiseUpdated,
            Action<string, string> onAssignmentChanged,
            Action<string, string> onDutyVacated,
            Func<int> seedSaltProvider,
            Func<IReadOnlyList<DutyRosterRow>> rowsProvider)
        {
            _assignmentByRole = assignmentByRole;
            _assignments = assignments;
            _getRow = getRow;
            _raiseUpdated = raiseUpdated;
            _onAssignmentChanged = onAssignmentChanged;
            _onDutyVacated = onDutyVacated;
            _seedSaltProvider = seedSaltProvider;
            _rowsProvider = rowsProvider;
        }

        public bool Assign(string role, string survivorId)
        {
            return AssignWithResult(role, survivorId).IsSuccess;
        }

        public ActionResult AssignWithResult(string role, string survivorId, bool confirmFitnessWarning = false)
        {
            var validation = ValidateAssign(role, survivorId, confirmFitnessWarning);
            if (!validation.IsSuccess)
                return validation;

            bool cleared = string.IsNullOrEmpty(survivorId);
            AssignInternal(role, survivorId, confirmFitnessWarning);
            return cleared
                ? ActionResult.Success("duty_roster.cleared")
                : ActionResult.Success("duty_roster.assigned");
        }

        public Func<string, bool>? IsExternalReserved { get; set; }
        public Func<string, bool>? IsCandidateEligible { get; set; }
        /// <summary>
        /// Optional health-aware role gate supplied by the host. The assignment
        /// engine remains ignorant of survivor state; it only enforces the
        /// returned role verdict at both preview and commit time.
        /// </summary>
        public Func<string, string, RoleFitnessVerdict>? EvaluateRoleFitness { get; set; }

        public ActionResult ValidateAssign(string role, string survivorId, bool confirmFitnessWarning = false)
        {
            if (!IsKnownRole(role))
                return ActionResult.Blocked("unknown_role", "duty_roster.unknown_role");
            if (string.IsNullOrEmpty(survivorId))
                return ActionResult.Success("duty_roster.cleared");
            if (IsCandidateEligible != null && !IsCandidateEligible(survivorId))
                return ActionResult.Blocked("ineligible_underage", "duty_roster.ineligible_underage");
            DutyRosterRow row = _getRow(survivorId);
            if (row == null)
                return ActionResult.Blocked("unknown_survivor", "duty_roster.unknown_survivor");
            if (!CanAssign(row))
                return ActionResult.Blocked("cannot_assign", "duty_roster.cannot_assign");
            var fitness = EvaluateRoleFitness?.Invoke(survivorId, role);
            if (fitness != null && !fitness.Allowed)
                return ActionResult.Blocked("fitness_blocked", "duty_roster.fitness_blocked");
            if (fitness != null && fitness.RequiresConfirmation && !confirmFitnessWarning)
                return ActionResult.Blocked(
                    "fitness_warning_confirmation_required",
                    "duty_roster.fitness_warning_confirmation_required");
            if (IsExternalReserved != null && IsExternalReserved(survivorId))
                return ActionResult.Blocked("busy", "duty_roster.busy");
            string currentRole = GetRoleOf(survivorId)!;
            if (currentRole != null && currentRole != role)
                return ActionResult.Blocked("already_assigned", "duty_roster.already_assigned");
            return ActionResult.Success("duty_roster.assigned");
        }

        private bool AssignInternal(string role, string survivorId, bool confirmFitnessWarning)
        {
            if (!IsKnownRole(role)) return false;
            if (!string.IsNullOrEmpty(survivorId))
            {
                DutyRosterRow row = _getRow(survivorId);
                if (row == null) return false;
                if (!CanAssign(row)) return false;
                var fitness = EvaluateRoleFitness?.Invoke(survivorId, role);
                if (fitness != null && !fitness.Allowed) return false;
                if (fitness != null && fitness.RequiresConfirmation && !confirmFitnessWarning) return false;
            }

            string previousSurvivorId = GetAssignment(role);
            if (string.IsNullOrEmpty(survivorId))
            {
                _assignmentByRole.Remove(role);
            }
            else
            {
                _assignmentByRole[role] = survivorId;
            }
            if (!string.IsNullOrEmpty(previousSurvivorId) && previousSurvivorId != survivorId)
                _onDutyVacated?.Invoke(role, previousSurvivorId);
            SyncAssignmentList();
            _onAssignmentChanged?.Invoke(role, survivorId);
            _raiseUpdated();
            return true;
        }

        /// <summary>The role a survivor currently holds, or null.</summary>
        public string GetRoleOf(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return null;
            foreach (var kv in _assignmentByRole)
                if (kv.Value == survivorId) return kv.Key;
            return null;
        }

        public string GetAssignment(string role)
        {
            if (string.IsNullOrEmpty(role)) return null;
            _assignmentByRole.TryGetValue(role, out string id);
            return id;
        }

        public void RemoveAssignmentsFor(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return;
            var drop = new List<string>();
            foreach (var kv in _assignmentByRole)
            {
                if (kv.Value == survivorId)
                    drop.Add(kv.Key);
            }

            for (int i = 0; i < drop.Count; i++)
            {
                _onDutyVacated?.Invoke(drop[i], survivorId);
                _assignmentByRole.Remove(drop[i]);
                _onAssignmentChanged?.Invoke(drop[i], string.Empty);
            }
            if (drop.Count > 0)
            {
                SyncAssignmentList();
                _raiseUpdated();
            }
        }

        /// <summary>
        /// Player skipped the night slate. Deterministic Utility AI among home rows.
        /// Same seed + day => same picks. Does not use string.GetHashCode.
        /// </summary>
        public int AutoAssignDefaults(int day)
        {
            var rows = _rowsProvider();
            if (rows == null) return 0;

            var eligible = new List<string>();
            for (int i = 0; i < rows.Count; i++)
            {
                DutyRosterRow row = rows[i];
                if (row != null && CanAssign(row) && row.status == DutyRosterIds.StatusHome)
                    eligible.Add(row.survivorId);
            }

            eligible.Sort(string.CompareOrdinal);
            int assigned = 0;
            var used = new HashSet<string>();
            for (int r = 0; r < DutyRosterIds.AssignmentRoles.Length; r++)
            {
                string role = DutyRosterIds.AssignmentRoles[r];
                if (_assignmentByRole.ContainsKey(role)) continue;
                string pick = PickEligible(eligible, used, day, role)!;
                if (pick == null) continue;
                if (Assign(role, pick))
                {
                    used.Add(pick);
                    assigned++;
                }
            }

            return assigned;
        }

        private bool CanAssign(DutyRosterRow row)
        {
            if (row == null) return false;
            if (IsCandidateEligible != null && !IsCandidateEligible(row.survivorId))
                return false;
            if (row.status == DutyRosterIds.StatusDead || row.status == DutyRosterIds.StatusQuiet || row.status == DutyRosterIds.StatusMissing)
                return false;
            if (row.status == DutyRosterIds.StatusLevy || row.status == DutyRosterIds.StatusWaystation)
                return false;
            return true;
        }

        private string PickEligible(List<string> eligible, HashSet<string> used, int day, string role)
        {
            var pool = new List<string>();
            for (int i = 0; i < eligible.Count; i++)
            {
                if (!used.Contains(eligible[i])
                    && IsFitnessAllowed(eligible[i], role))
                    pool.Add(eligible[i]);
            }

            if (pool.Count == 0) return null;
            int salt = _seedSaltProvider() + DutyRosterIds.SeedUtilityOffset + day * 17 + StableHash.Of(role);
            int n = (int)(((long)salt & 0x7FFFFFFF));
            return pool[n % pool.Count];
        }

        private bool IsFitnessAllowed(string survivorId, string role)
        {
            var verdict = EvaluateRoleFitness?.Invoke(survivorId, role);
            // Utility auto-assignment has no player present to acknowledge a
            // warning. It may choose only cleanly allowed candidates; impaired
            // candidates remain manually assignable through the explicit
            // host confirmation path.
            return verdict == null || (verdict.Allowed && !verdict.RequiresConfirmation);
        }

        private static bool IsKnownRole(string role)
        {
            for (int i = 0; i < DutyRosterIds.AssignmentRoles.Length; i++)
                if (DutyRosterIds.AssignmentRoles[i] == role) return true;
            return false;
        }

        private void SyncAssignmentList()
        {
            // Emit in ordinal role order: dictionary iteration order is not a
            // cross-host guarantee, and the assignments list is part of the save.
            var previous = new Dictionary<string, DutyRosterAssignmentEntry>(StringComparer.Ordinal);
            for (int i = 0; i < _assignments.Count; i++)
            {
                var entry = _assignments[i];
                if (entry != null && !string.IsNullOrEmpty(entry.role))
                    previous[entry.role] = entry;
            }
            _assignments.Clear();
            var roles = new List<string>(_assignmentByRole.Count);
            foreach (var kv in _assignmentByRole) roles.Add(kv.Key);
            roles.Sort(string.CompareOrdinal);
            for (int i = 0; i < roles.Count; i++)
            {
                var entry = new DutyRosterAssignmentEntry
                {
                    role = roles[i],
                    survivorId = _assignmentByRole[roles[i]]
                };
                if (previous.TryGetValue(entry.role, out var old)
                    && old.survivorId == entry.survivorId)
                {
                    entry.fitnessWarningAcknowledged = old.fitnessWarningAcknowledged;
                    entry.fitnessWarningDay = old.fitnessWarningDay;
                    entry.fitnessWarningReasons = old.fitnessWarningReasons != null
                        ? new List<string>(old.fitnessWarningReasons)
                        : new List<string>();
                }
                _assignments.Add(entry);
            }
        }

        /// <summary>Bulk clear used by BurnChart.</summary>
        public void ClearAll()
        {
            _assignmentByRole.Clear();
            _assignments.Clear();
        }
    }
}
