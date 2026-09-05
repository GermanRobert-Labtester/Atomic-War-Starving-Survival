using System;
using System.Collections.Generic;

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
        private readonly Func<int> _seedSaltProvider;
        private readonly Func<IReadOnlyList<DutyRosterRow>> _rowsProvider;

        public DutyRosterAssignmentEngine(
            Dictionary<string, string> assignmentByRole,
            List<DutyRosterAssignmentEntry> assignments,
            Func<string, DutyRosterRow> getRow,
            Action raiseUpdated,
            Action<string, string> onAssignmentChanged,
            Func<int> seedSaltProvider,
            Func<IReadOnlyList<DutyRosterRow>> rowsProvider)
        {
            _assignmentByRole = assignmentByRole;
            _assignments = assignments;
            _getRow = getRow;
            _raiseUpdated = raiseUpdated;
            _onAssignmentChanged = onAssignmentChanged;
            _seedSaltProvider = seedSaltProvider;
            _rowsProvider = rowsProvider;
        }

        public bool Assign(string role, string survivorId)
        {
            return AssignWithResult(role, survivorId).IsSuccess;
        }

        public ActionResult AssignWithResult(string role, string survivorId)
        {
            var validation = ValidateAssign(role, survivorId);
            if (!validation.IsSuccess)
                return validation;

            bool cleared = string.IsNullOrEmpty(survivorId);
            AssignInternal(role, survivorId);
            return cleared
                ? ActionResult.Success("duty_roster.cleared")
                : ActionResult.Success("duty_roster.assigned");
        }

        public Func<string, bool>? IsExternalReserved { get; set; }

        public ActionResult ValidateAssign(string role, string survivorId)
        {
            if (!IsKnownRole(role))
                return ActionResult.Blocked("unknown_role", "duty_roster.unknown_role");
            if (string.IsNullOrEmpty(survivorId))
                return ActionResult.Success("duty_roster.cleared");
            DutyRosterRow row = _getRow(survivorId);
            if (row == null)
                return ActionResult.Blocked("unknown_survivor", "duty_roster.unknown_survivor");
            if (!CanAssign(row))
                return ActionResult.Blocked("cannot_assign", "duty_roster.cannot_assign");
            if (IsExternalReserved != null && IsExternalReserved(survivorId))
                return ActionResult.Blocked("busy", "duty_roster.busy");
            string currentRole = GetRoleOf(survivorId)!;
            if (currentRole != null && currentRole != role)
                return ActionResult.Blocked("already_assigned", "duty_roster.already_assigned");
            return ActionResult.Success("duty_roster.assigned");
        }

        private bool AssignInternal(string role, string survivorId)
        {
            if (!IsKnownRole(role)) return false;
            if (!string.IsNullOrEmpty(survivorId))
            {
                DutyRosterRow row = _getRow(survivorId);
                if (row == null) return false;
                if (!CanAssign(row)) return false;
            }

            if (string.IsNullOrEmpty(survivorId))
            {
                _assignmentByRole.Remove(role);
            }
            else
            {
                _assignmentByRole[role] = survivorId;
            }
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
                _assignmentByRole.Remove(drop[i]);
            if (drop.Count > 0)
                SyncAssignmentList();
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
                if (!used.Contains(eligible[i]))
                    pool.Add(eligible[i]);
            }

            if (pool.Count == 0) return null;
            int salt = _seedSaltProvider() + DutyRosterIds.SeedUtilityOffset + day * 17 + StableHash.Of(role);
            int n = (int)(((long)salt & 0x7FFFFFFF));
            return pool[n % pool.Count];
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
            _assignments.Clear();
            var roles = new List<string>(_assignmentByRole.Count);
            foreach (var kv in _assignmentByRole) roles.Add(kv.Key);
            roles.Sort(string.CompareOrdinal);
            for (int i = 0; i < roles.Count; i++)
            {
                _assignments.Add(new DutyRosterAssignmentEntry
                {
                    role = roles[i],
                    survivorId = _assignmentByRole[roles[i]]
                });
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
