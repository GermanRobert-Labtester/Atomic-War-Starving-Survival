// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 202 — Interpersonal Conflict & Grievance host wiring.
// The pure domain InterpersonalConflictSystem is the authority for interpersonal
// disputes, grievance accumulation, conflict escalation, and mediation.
// ============================================================================

using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core.Survivors;

using Ashfall.Core;
using Ashfall.Core.Random;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private InterpersonalConflictHostSession? _interpersonalConflict;
        private bool _interpersonalConflictDirty;

        public InterpersonalConflictHostSession? InterpersonalConflict => _interpersonalConflict;

        public void SetupInterpersonalConflict()
        {
            if (_interpersonalConflict != null) return;

            var saved = InterpersonalConflictSaveStore.TryLoad();
            _interpersonalConflict = InterpersonalConflictHostSession.Create(saved);
            _interpersonalConflict.StateChanged += () => _interpersonalConflictDirty = true;

            // Load authored conflict templates catalog
            string catalogPath = CatalogPath.ResolveCatalog("conflict_templates.json");
            var catalogIo = CatalogPath.CreateFileIOForDataDir(CatalogPath.ResolveDataDir());
            if (catalogIo.FileExists(catalogPath))
            {
                _interpersonalConflict.LoadCatalog(catalogIo.ReadAllText(catalogPath));
            }
        }

        public InterpersonalConflict InitiateInterpersonalConflict(
            string initiatorId,
            string targetId,
            ConflictType type,
            string triggerDescription,
            ConflictSeverity severity = ConflictSeverity.Mild,
            int currentDay = 1)
        {
            SetupInterpersonalConflict();
            return _interpersonalConflict!.InitiateConflict(initiatorId, targetId, type, triggerDescription, severity, currentDay);
        }

        public SurvivorGrievance AddSurvivorGrievance(
            string holderId,
            string accusedId,
            string reason,
            float intensity = 30f,
            int currentDay = 1)
        {
            SetupInterpersonalConflict();
            return _interpersonalConflict!.AddGrievance(holderId, accusedId, reason, intensity, currentDay);
        }

        public bool MediateInterpersonalConflict(string conflictId, string mediatorId, int currentDay)
        {
            SetupInterpersonalConflict();
            return _interpersonalConflict!.MediateConflict(conflictId, mediatorId, currentDay);
        }

        public InterpersonalConflictCensus GetInterpersonalConflictCensus() =>
            _interpersonalConflict?.Census ?? default;

        public void TickInterpersonalConflict(int day)
        {
            SetupInterpersonalConflict();
            _interpersonalConflict!.TickDay(day);
        }

        public void SaveInterpersonalConflict()
        {
            if (_interpersonalConflict == null) return;
            var state = _interpersonalConflict.System.CaptureState();
            InterpersonalConflictSaveStore.TrySave(state);
            if (CaptureSection(
                    InterpersonalConflictSaveStore.SectionName,
                    InterpersonalConflictSaveStore.TryCapturePersisted(state)))
            {
                _interpersonalConflictDirty = false;
            }
        }

        public void FlushInterpersonalConflictIfDirty()
        {
            if (_interpersonalConflictDirty)
            {
                SaveInterpersonalConflict();
            }
        }

        public void ResetInterpersonalConflict()
        {
            _interpersonalConflict = null;
            _interpersonalConflictDirty = false;
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
