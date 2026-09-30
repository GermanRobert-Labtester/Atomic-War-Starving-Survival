// SPDX-License-Identifier: MIT
// ============================================================================
// Main partial : Main.ChronicConditions
// Core State   : Ashfall.Core.Medical.ChronicConditionSystem (Plan 193)
// Host Caller  : SetupChronicConditions / TickChronicConditions
// Purpose      : Plan 193 — Chronic Conditions & Accommodations host wiring.
//                One truthful path from a committed clinical producer fact to
//                a tracked condition record, and a read-only capability
//                projection onto the duty/needs owners. Lifecycle rides the
//                existing medical setup/day-orchestration.
// ============================================================================

using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core.Medical;

namespace AtomicWar.GodotApp
{
    public partial class Main : Control
    {
        private ChronicConditionHostSession? _chronicConditions;
        private bool _chronicConditionsDirty;

        public ChronicConditionHostSession? ChronicConditions => _chronicConditions;

        private void SetupChronicConditions()
        {
            if (_chronicConditions != null) return;

            _chronicConditions = ChronicConditionHostSession.Create(_dataDir);
            var saved = ChronicConditionSaveStore.TryLoad();
            if (saved != null)
            {
                _chronicConditions.RestoreState(saved);
            }

            // Producer seam (Plan 193 Phase 1): a committed clinical producer
            // fact — a CONFIRMED diagnosis from the existing pipeline — records
            // the chronic condition for that survivor. Suspisions never write;
            // permanence comes from the authored catalog. Exactly-once is held
            // by the Core owner (idempotent on survivor + condition).
            _medical.StateChanged += BindChronicProducerOnce;
            if (_chronicConditions != null) BindChronicProducer();
        }

        /// <summary>
        /// Subscription is installed once Pipeline is bound; setup order never
        /// matters because the pipeline bind raises StateChanged.
        /// </summary>
        private void BindChronicProducer()
        {
            if (_chronicConditions == null || _medical?.Pipeline == null || _chronicProducerBound) return;
            _chronicProducerBound = true;

            _medical.Pipeline.OnDiagnosisConfirmed += (definitionId, survivorId) =>
            {
                var mapped = MapDiagnosisToChronicCondition(definitionId);
                if (mapped == null) return; // not a chronic-member diagnosis
                _chronicConditions!.RecordCondition(survivorId.Value, mapped, _simDay, "diagnosis_confirmed");
                _chronicConditionsDirty = true;
            };
        }

        private void BindChronicProducerOnce()
        {
            if (_chronicProducerBound) return;
            BindChronicProducer();
        }
        private bool _chronicProducerBound;

        /// <summary>
        /// Authored diagnosis → chronic-condition crosswalk. Only diagnoses with
        /// a real permanent clinical consequence map; everything else stays a
        /// regular diagnosis row (no mood meters, no generic penalty).
        /// </summary>
        private static string? MapDiagnosisToChronicCondition(string diagnosisId)
        {
            if (string.IsNullOrWhiteSpace(diagnosisId)) return null;
            // Authored pairing (documented in the plan): permanent respiratory
            // damage → pulmonary ash fibrosis; lifetime radiation illness →
            // radiation cataract amblyopia at old age. One row each, keyed by
            // the clinical owner's stable ids.
            return diagnosisId switch
            {
                Ashfall.Core.Medical.MedicalTreatmentCatalog.RespiratoryDegenerationId => "cond_respiratory_damage",
                Ashfall.Core.Medical.MedicalTreatmentCatalog.RadiationSicknessId => "cond_partial_blindness",
                Ashfall.Core.Medical.MedicalTreatmentCatalog.ChemicalDependencyId => "cond_tremors_neurological",
                _ => null
            };
        }

        /// <summary>
        /// Plan 193 read model: capability multiplier projected onto the duty
        /// owner's capability names. Null-safe when unbound (unarmed
        /// projection reads 1.0).
        /// </summary>
        public float GetChronicCapabilityModifier(string survivorId, string capability)
        {
            if (_chronicConditions == null) return 1.0f;
            return _chronicConditions.CalculateCapabilityModifier(survivorId, capability);
        }

        public string GetChronicConditionsReadout(string survivorId)
        {
            if (_chronicConditions == null) return "Chronic conditions session not ready.";
            var conds = _chronicConditions.GetSurvivorConditions(survivorId);
            var accs = _chronicConditions.GetSurvivorAccommodations(survivorId);
            var sb = new System.Text.StringBuilder();
            sb.Append($"Chronic conditions: {conds.Count} tracked / {accs.Count} active accommodation(s)");
            foreach (var c in conds)
            {
                var def = _chronicConditions.System.GetConditionDef(c.ConditionId);
                sb.Append($"\n  • {def?.display_name ?? c.ConditionId} (since day {c.OnsetDay}, {c.Cause})");
            }
            foreach (var a in accs)
            {
                var def = _chronicConditions.System.GetAccommodationDef(a.AccommodationId);
                sb.Append($"\n  + {def?.display_name ?? a.AccommodationId} (fitted day {a.InstalledDay})");
            }
            sb.Append($"\nImpairment score: {_chronicConditions.GetTotalImpairmentScore(survivorId):0.0}%");
            return sb.ToString();
        }

        // Day orchestration (rides the existing expanded-shelter day path).
        // Plan 193: conditions are permanent and accommodations explicit —
        // nothing runs on a tick; the tick only persists player-visible change.
        public void TickChronicConditions(int day)
        {
            if (_chronicConditions == null) return;
            FlushChronicConditionsIfDirty();
        }

        // Save participant + lifecycle (rides the existing medical save path)
        public void SaveChronicConditions()
        {
            if (_chronicConditions == null) return;
            var state = _chronicConditions.CaptureState();
            if (CaptureSection(
                    ChronicConditionSaveStore.SectionName,
                    ChronicConditionSaveStore.TryCapturePersisted(state)))
            {
                _chronicConditionsDirty = false;
            }
        }

        public void FlushChronicConditionsIfDirty()
        {
            if (_chronicConditionsDirty)
            {
                SaveChronicConditions();
            }
        }

        public void ResetChronicConditions()
        {
            _chronicConditions = null;
            _chronicProducerBound = false;
            _chronicConditionsDirty = false;
        }
    }
}
