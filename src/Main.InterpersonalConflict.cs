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
    }
}
