// SPDX-License-Identifier: MIT
// ============================================================================
// Main Partial : Plans 216 + 202 (interpersonal) integration seams
// Plan 216 — Exercise / physical conditioning
// Plan 202 — Interpersonal conflict projection over canonical relations
// ============================================================================
using System.Collections.Generic;
using System;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Random;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        /// <summary>
        /// Execute a workout through the survivor-social coordinator. Fitness is
        /// checked from the existing duty-fitness model; fatigue is attributed
        /// by the coordinator to the canonical Needs owner and the result is
        /// persisted with the existing survivor_social section.
        /// </summary>
        public WorkoutResult? ExecuteSurvivorWorkout(
            string survivorId,
            WorkoutRoutineType routine,
            float intensity = 1f)
        {
            SetupSurvivorSocial();
            var fitness = EvaluateSurvivorFitness(survivorId);
            if (fitness.IsHardBlocked) return null;

            var rng = _campaignDay.Rng.Fork(CampaignStreamIds.Social, _simDay, (int)routine);
            var result = _survivorSocial.ExecuteWorkout(
                survivorId, routine, _simDay, intensity, rng);
            if (result != null)
            {
                _survivorSocialDirty = true;
                RefreshSurvivorSocialReadModel();
            }
            return result;
        }

        /// <summary>Read-only access to the persisted conditioning profile.</summary>
        public FitnessProfile? GetSurvivorFitnessProfile(string survivorId)
        {
            SetupSurvivorSocial();
            if (_survivorSocial == null || string.IsNullOrEmpty(survivorId)) return null;
            return _survivorSocial.Exercise.TryGetProfile(survivorId);
        }

        /// <summary>
        /// Plan 202 projection. The typed conflict view is derived from the
        /// canonical survivor-relations state and never creates a second ledger.
        /// </summary>
        public IReadOnlyList<InterpersonalConflict> GetInterpersonalConflictProjection()
        {
            SetupSurvivorRelations();
            return InterpersonalConflictSystem.ProjectCanonicalRelations(
                _survivorRelationsCore.State, _simDay);
        }

        /// <summary>Return source-backed relationship grievances as a read model.</summary>
        public IReadOnlyList<SurvivorGrievance> GetInterpersonalGrievanceProjection()
        {
            SetupSurvivorRelations();
            return InterpersonalConflictSystem.ProjectCanonicalGrievances(
                _survivorRelationsCore.State, _simDay);
        }

        /// <summary>
        /// Resolve a projected conflict through the canonical relations owner.
        /// The caller passes the underlying relations conflict id (without the
        /// <c>relations:</c> projection prefix), so affinity and mediation
        /// history are applied exactly once by SurvivorRelationsSystem.
        /// </summary>
        public ActionResult MediateInterpersonalConflict(
            string canonicalConflictId,
            string mediatorId,
            MediationStyle style = MediationStyle.Apology)
        {
            SetupSurvivorRelations();
            if (!string.IsNullOrEmpty(canonicalConflictId)
                && canonicalConflictId.StartsWith("relations:", StringComparison.Ordinal))
            {
                canonicalConflictId = canonicalConflictId.Substring("relations:".Length);
            }
            var result = _survivorRelationsCore.Mediate(canonicalConflictId, mediatorId, style);
            if (result.IsSuccess)
            {
                _survivorRelationsDirty = true;
                _survivorRelationsPanel?.RefreshView();
            }
            return result;
        }

        /// <summary>
        /// Applies the conflict package's typed morale fact to the canonical
        /// Needs owner. SurvivorRelationsSystem raises this once for both the
        /// existing panel command and the Plan 202 host command.
        /// </summary>
        private void ApplyInterpersonalConflictMorale(MediationEntry entry)
        {
            if (entry == null) return;
            SetupSurvivors();
            if (_survivors?.Needs == null || _survivorRelationsCore == null) return;

            var conflict = _survivorRelationsCore.State.activeConflicts.Find(c =>
                c != null && string.Equals(c.conflictId, entry.conflictId, StringComparison.Ordinal));
            if (conflict == null) return;
            if (!Enum.TryParse(entry.outcome, out MediationStyle style)) return;

            float moraleDelta = InterpersonalConflictSystem.MoraleDeltaFor(style);
            if (moraleDelta == 0f) return;
            _survivors.Needs.ApplyAttributedDelta(
                conflict.dwellerA, NeedKind.Morale, moraleDelta,
                InterpersonalConflictSystem.ResolutionMoraleSource);
            _survivors.Needs.ApplyAttributedDelta(
                conflict.dwellerB, NeedKind.Morale, moraleDelta,
                InterpersonalConflictSystem.ResolutionMoraleSource);
        }
    }
}
