// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 195 — Survivor Specialization Roles host wiring.
// DEC-185: SurvivorRoleSystem owns role identity, bonus scaling, and role XP
// progression. Duty shift assignments stay owned by DutyRosterSystem; skill
// levels stay owned by SkillProgressionSystem. Role practice XP is awarded
// only from verified completed-work facts raised by the shared skill
// progression authority (OnXpGained) — exactly once per fact, no RNG.
// ============================================================================

using System;
using System.Collections.Generic;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private SurvivorRoleHostSession? _survivorRoles;
        private bool _survivorRolesDirty;
        private bool _survivorRolesSkillBound;

        /// <summary>Handler for the skill owner's verified work facts; kept to unsubscribe on reset.</summary>
        private Action<SkillActor, string, float>? _survivorRolesSkillHandler;

        public SurvivorRoleHostSession? SurvivorRoles => _survivorRoles;

        public void SetupSurvivorRoles()
        {
            if (_survivorRoles != null) return;

            var saved = SurvivorRoleSaveStore.TryLoad();
            _survivorRoles = SurvivorRoleHostSession.Create(saved);
            _survivorRoles.StateChanged += () => _survivorRolesDirty = true;

            string catalogPath = CatalogPath.ResolveCatalog("survivor_roles.json");
            var catalogIo = CatalogPath.CreateFileIOForDataDir(CatalogPath.ResolveDataDir());
            if (catalogIo.FileExists(catalogPath))
            {
                _survivorRoles.LoadCatalog(catalogIo.ReadAllText(catalogPath));
            }

            BindSurvivorRolePracticeProducer();
        }

        /// <summary>
        /// Plan 195 — the named work-completion producer is the shared
        /// SkillProgression authority's <see cref="SkillProgressionSystem.OnXpGained"/>
        /// event: every verified completed-work fact fires it exactly once.
        /// Role XP advances only when the practiced discipline is the assigned
        /// role's governing discipline.
        /// </summary>
        private void BindSurvivorRolePracticeProducer()
        {
            if (_survivorRoles == null || _survivorRolesSkillBound) return;
            var skills = EnsureSharedSkillProgression();

            _survivorRolesSkillHandler = (actor, disciplineId, _) =>
            {
                if (actor == null || string.IsNullOrEmpty(actor.Id)) return;
                var assignment = _survivorRoles!.GetRoleAssignment(actor.Id);
                if (assignment == null) return;
                var def = _survivorRoles.GetRoleDef(assignment.RoleId);
                if (def == null) return;
                if (!string.Equals(def.required_discipline, disciplineId, StringComparison.OrdinalIgnoreCase))
                    return;

                // Fixed deterministic practice award per verified fact.
                _survivorRoles.AwardPracticeXp(actor.Id, 10);
            };
            skills.OnXpGained += _survivorRolesSkillHandler;
            _survivorRolesSkillBound = true;
        }

        /// <summary>
        /// Normalized discipline levels (0–100) for the survivor, read from the
        /// SkillProgression owner — never copied or cached here.
        /// </summary>
        public IReadOnlyDictionary<string, float> GetSurvivorDisciplineLevels(string survivorId)
        {
            var skills = EnsureSharedSkillProgression();
            var levels = new Dictionary<string, float>(StringComparer.OrdinalIgnoreCase);
            foreach (var discipline in SkillProgressionSystem.Disciplines)
            {
                float progress = skills.GetDisciplineProgress01(survivorId, discipline)
                    + skills.GetCachedBonus(survivorId, discipline);
                levels[discipline] = Math.Clamp(progress, 0f, 1f) * 100f;
            }
            return levels;
        }

        /// <summary>
        /// Assigns a specialization role after the skill-owner eligibility gate.
        /// Returns the refusal code on failure (null assignment).
        /// </summary>
        public SurvivorRoleAssignment? AssignSurvivorRole(string survivorId, string roleId, out string failureReason)
        {
            failureReason = string.Empty;
            SetupSurvivors();
            SetupSurvivorRoles();
            var assignment = _survivorRoles!.AssignRole(
                survivorId, roleId, GetSurvivorDisciplineLevels(survivorId), _simDay, out failureReason);
            if (assignment != null) _survivorRolesDirty = true;
            return assignment;
        }

        public bool UnassignSurvivorRole(string survivorId)
        {
            SetupSurvivorRoles();
            bool removed = _survivorRoles!.UnassignRole(survivorId);
            if (removed) _survivorRolesDirty = true;
            return removed;
        }

        public SurvivorRoleAssignment? GetSurvivorRoleAssignment(string survivorId)
        {
            SetupSurvivorRoles();
            return _survivorRoles!.GetRoleAssignment(survivorId);
        }

        /// <summary>Read-only role readout for the existing survivor detail route.</summary>
        public (string RoleId, string DisplayName, int Level, int ExperiencePoints)? GetSurvivorRoleReadout(string survivorId)
        {
            SetupSurvivorRoles();
            var assignment = _survivorRoles!.GetRoleAssignment(survivorId);
            if (assignment == null) return null;
            var def = _survivorRoles.GetRoleDef(assignment.RoleId);
            return (assignment.RoleId, def?.display_name ?? assignment.RoleId, assignment.Level, assignment.ExperiencePoints);
        }

        public void SaveSurvivorRoles()
        {
            if (_survivorRoles == null) return;
            var state = _survivorRoles.System.CaptureState();
            SurvivorRoleSaveStore.TrySave(state);
            if (CaptureSection(
                    SurvivorRoleSaveStore.SectionName,
                    SurvivorRoleSaveStore.TryCapturePersisted(state)))
            {
                _survivorRolesDirty = false;
            }
        }

        public void FlushSurvivorRolesIfDirty()
        {
            if (_survivorRolesDirty)
            {
                SaveSurvivorRoles();
            }
        }

        public void ResetSurvivorRoles()
        {
            if (_survivorRolesSkillBound && _survivorRolesSkillHandler != null)
            {
                EnsureSharedSkillProgression().OnXpGained -= _survivorRolesSkillHandler;
            }
            _survivorRolesSkillHandler = null;
            _survivorRolesSkillBound = false;
            _survivorRoles = null;
            _survivorRolesDirty = false;
        }
    }
}
