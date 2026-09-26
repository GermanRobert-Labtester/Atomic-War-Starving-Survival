// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : SurvivorRoleSaveStore
// Core State : Ashfall.Core.Survivors.SurvivorRoleState
// Host Caller: Main.SurvivorRoles
// Purpose    : Plan 195 — Survivor Specialization Roles host session & persistence.
//              DEC-185 ownership: role identity, bonus scaling, and role XP
//              progression live here; duty shift assignments stay owned by
//              DutyRosterSystem and skill levels by SkillProgressionSystem.
// ============================================================================

using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Save;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    public static class SurvivorRoleSaveStore
    {
        public const string FileName = "survivor_roles_save.json";
        public const string SectionName = "survivor_roles";

        private static readonly SaveStore<SurvivorRoleState> s_store =
            SaveStoreHub.Checksummed<SurvivorRoleState>(FileName, nameof(SurvivorRoleSaveStore));

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCapturePersisted(SurvivorRoleState state) => s_store.CaptureBare(state);
        public static SurvivorRoleState? TryRestorePersisted(string json) => s_store.RestoreBare(json);
        public static bool TrySave(SurvivorRoleState state) => s_store.TrySave(state);
        public static SurvivorRoleState? TryLoad() => s_store.TryLoad();
    }

    /// <summary>
    /// Plan 195 host session. Wraps <see cref="SurvivorRoleSystem"/>.
    /// Exposes role assignment (gated through the shared SkillProgression
    /// owner's discipline levels), earned role practice, level progression,
    /// and bonus readout. The Core auto-action bookkeeping is intentionally
    /// NOT exposed here as an operational command — it executes nothing.
    /// </summary>
    public sealed class SurvivorRoleHostSession : HostSessionBase
    {
        private readonly SurvivorRoleSystem _system;
        private int _authoredRoleCount;

        public SurvivorRoleSystem System => _system;
        public int AuthoredRoleCount => _authoredRoleCount;
        public string LastEvent { get; private set; } = string.Empty;

        public SurvivorRoleHostSession(SurvivorRoleState? state = null)
        {
            _system = new SurvivorRoleSystem(state);
            _system.OnRoleAssigned += a =>
            {
                LastEvent = $"Role assigned: {a.SurvivorId} is now {a.RoleId} (day {a.AssignedDay}).";
                RaiseStateChanged();
            };
            _system.OnRoleUnassigned += survivorId =>
            {
                LastEvent = $"Role unassigned: {survivorId}.";
                RaiseStateChanged();
            };
            _system.OnRoleLeveledUp += (survivorId, level) =>
            {
                LastEvent = $"Role leveled: {survivorId} reached role level {level}.";
                RaiseStateChanged();
            };
        }

        public static SurvivorRoleHostSession Create(SurvivorRoleState? state = null) =>
            new SurvivorRoleHostSession(state);

        public void LoadCatalog(string json)
        {
            _system.LoadCatalog(json);
            _authoredRoleCount = _system.GetAllRoleDefs().Count;
            LastEvent = $"Loaded {_authoredRoleCount} survivor role definitions.";
            RaiseStateChanged();
        }

        /// <summary>
        /// Assigns a role after checking the SkillProgression owner's
        /// normalized discipline levels (0–100). Returns null with the refusal
        /// code when the survivor does not meet the gate or the role cap.
        /// </summary>
        public SurvivorRoleAssignment? AssignRole(
            string survivorId,
            string roleId,
            IReadOnlyDictionary<string, float> disciplineLevels,
            int day,
            out string failureReason)
        {
            if (!_system.CanAssignRoleByDiscipline(survivorId, roleId, disciplineLevels, out failureReason))
            {
                LastEvent = $"Role refused: {survivorId} → {roleId} ({failureReason}).";
                return null;
            }

            failureReason = string.Empty;
            return _system.AssignRole(survivorId, roleId, skills: null, day: day, force: true);
        }

        public bool UnassignRole(string survivorId) => _system.UnassignRole(survivorId);

        public SurvivorRoleAssignment? GetRoleAssignment(string survivorId) =>
            _system.GetRoleAssignment(survivorId);

        public SurvivorRoleDef? GetRoleDef(string roleId) => _system.GetRoleDef(roleId);

        public IReadOnlyCollection<SurvivorRoleDef> GetAllRoleDefs() => _system.GetAllRoleDefs();

        public float GetRoleBonus(string survivorId, string bonusType) =>
            _system.GetRoleBonus(survivorId, bonusType);

        /// <summary>
        /// Awards earned role practice for one verified completed-work fact.
        /// The host awards a fixed 10 XP per fact — deterministic, no RNG.
        /// Exactly-once per fact: the producer event fires once per fact.
        /// </summary>
        public void AwardPracticeXp(string survivorId, int xpAmount = 10)
        {
            if (_system.AddRoleXp(survivorId, xpAmount))
            {
                RaiseStateChanged();
            }
        }

        public SurvivorRoleState CaptureState() => _system.CaptureState();

        public void RestoreState(SurvivorRoleState state)
        {
            _system.RestoreState(state);
            LastEvent = "Restored survivor role state.";
            RaiseStateChanged();
        }

        public bool TrySave() => SurvivorRoleSaveStore.TrySave(CaptureState());

        public bool TryLoad()
        {
            var state = SurvivorRoleSaveStore.TryLoad();
            if (state == null) return false;
            RestoreState(state);
            return true;
        }
    }
}
