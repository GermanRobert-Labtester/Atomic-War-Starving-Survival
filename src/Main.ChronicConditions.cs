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
using Ashfall.Core;
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
                Ashfall.Core.Medical.MedicalTreatmentCatalog.RespiratoryDegenerationId => ChronicConditionIds.PulmonaryFibrosis,
                Ashfall.Core.Medical.MedicalTreatmentCatalog.RadiationSicknessId => ChronicConditionIds.RadiationCataract,
                Ashfall.Core.Medical.MedicalTreatmentCatalog.ChemicalDependencyId => ChronicConditionIds.NeurotoxicTremors,
                _ => null
            };
        }

        /// <summary>
        /// Stable chronic-condition catalog ids the host producers reference.
        /// These must exist in `chronic_conditions.json`; the
        /// `Plan193ChronicConditionIntegrationTests` reachability gate pins that.
        /// </summary>
        private static class ChronicConditionIds
        {
            public const string Limp = "cond_chronic_limp";
            public const string RadiationCataract = "cond_partial_blindness";
            public const string PulmonaryFibrosis = "cond_respiratory_damage";
            public const string NeurotoxicTremors = "cond_tremors_neurological";
            public const string HearingLoss = "cond_hearing_loss_moderate";
            public const string JointPain = "cond_chronic_joint_pain";
        }

        /// <summary>
        /// Committed-fact producer seam for the chronic authority: a physical or
        /// chronological fact that has already happened records exactly one
        /// condition (idempotent at the Core owner). Producers must never infer
        /// conditions from mood or presentation state. Returns true when the
        /// condition is now tracked.
        /// </summary>
        public bool RecordChronicConditionFact(string survivorId, string conditionId, string cause)
        {
            if (string.IsNullOrWhiteSpace(survivorId) || string.IsNullOrWhiteSpace(conditionId)) return false;
            SetupMedical();
            SetupChronicConditions();
            if (_chronicConditions == null) return false;
            var rec = _chronicConditions.RecordCondition(survivorId, conditionId, _simDay, cause);
            if (rec == null) return false;
            _chronicConditionsDirty = true;
            return true;
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

        /// <summary>
        /// T18 — the duty-facing chronic capability projection. Returns the
        /// most-limiting of the capabilities the duty-fitness authority
        /// reasons about, in [0.10, 1.0]. 1.0 when the session is unbound or
        /// the survivor has no tracked condition. Read-only, derived each
        /// evaluation; never persisted and never a second ledger.
        /// </summary>
        private static readonly string[] DutyChronicCapabilities =
            { "work_speed", "movement_speed", "combat_effectiveness", "crafting_quality" };

        public float GetChronicDutyCapabilityModifier(string survivorId)
        {
            if (_chronicConditions == null) return 1.0f;
            float worst = 1.0f;
            for (int i = 0; i < DutyChronicCapabilities.Length; i++)
            {
                float modifier = _chronicConditions.CalculateCapabilityModifier(survivorId, DutyChronicCapabilities[i]);
                if (modifier < worst) worst = modifier;
            }
            return worst;
        }

        /// <summary>
        /// T18 — the player's accommodation decision. Fitting is gated on the
        /// real inventory authority: every authored maintenance cost item must
        /// be on hand and is consumed exactly once. The chronic authority owns
        /// the record and the capability projection; this command owns only the
        /// cross-owner transaction.
        /// </summary>
        public ActionResult FitChronicAccommodation(string survivorId, string conditionId, string accommodationId)
        {
            SetupChronicConditions();
            if (_chronicConditions == null)
                return ActionResult.Blocked("chronic_session_offline", "chronic.session_offline");

            var def = _chronicConditions.System.GetAccommodationDef(accommodationId);
            if (def == null)
                return ActionResult.Blocked("unknown_accommodation", "chronic.unknown_accommodation");

            if (string.IsNullOrWhiteSpace(survivorId)
                || _chronicConditions.GetSurvivorConditions(survivorId).Count == 0)
                return ActionResult.Blocked("no_tracked_condition", "chronic.no_tracked_condition");

            // Idempotency: a second fit of an already-active accommodation must
            // refuse before consuming materials, so a CLI/script replay cannot
            // double-charge the inventory (the UI hides the button, but the
            // authority must not depend on presentation).
            var active = _chronicConditions.GetSurvivorAccommodations(survivorId);
            for (int i = 0; i < active.Count; i++)
            {
                if (string.Equals(active[i].AccommodationId, accommodationId, StringComparison.OrdinalIgnoreCase))
                    return ActionResult.Blocked("already_fitted", "chronic.already_fitted");
            }

            if (_inventory?.Inventory == null)
                return ActionResult.Blocked("no_inventory", "chronic.inventory_offline");

            // Aggregate duplicate cost rows so the preflight and the commit
            // agree, then consume only after every requirement is proven.
            var required = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            if (def.maintenance_cost_items != null)
            {
                for (int i = 0; i < def.maintenance_cost_items.Count; i++)
                {
                    string itemId = def.maintenance_cost_items[i];
                    if (string.IsNullOrWhiteSpace(itemId)) continue;
                    required.TryGetValue(itemId, out int have);
                    required[itemId] = have + 1;
                }
            }
            foreach (var pair in required)
            {
                if (_inventory.Inventory.CountById(pair.Key) < pair.Value)
                    return ActionResult.Blocked("missing_materials", "chronic.missing_materials");
            }
            foreach (var pair in required)
                _inventory.Inventory.Remove(pair.Key, pair.Value);

            var rec = _chronicConditions.AssignAccommodation(survivorId, accommodationId, conditionId, _simDay);
            if (rec == null)
                return ActionResult.Failed("assign_refused", "chronic.assign_refused");
            _chronicConditionsDirty = true;
            return ActionResult.Success("chronic.accommodation_fitted");
        }

        /// <summary>
        /// T18 — removes a fitted accommodation through the chronic authority.
        /// The authored maintenance items are not refunded (they were consumed
        /// on fitting); the command is idempotent-safe because the authority
        /// refuses an already-removed record.
        /// </summary>
        public ActionResult RemoveChronicAccommodation(string survivorId, string accommodationId)
        {
            SetupChronicConditions();
            if (_chronicConditions == null)
                return ActionResult.Blocked("chronic_session_offline", "chronic.session_offline");
            if (!_chronicConditions.RemoveAccommodation(survivorId, accommodationId))
                return ActionResult.Blocked("not_fitted", "chronic.not_fitted");
            _chronicConditionsDirty = true;
            return ActionResult.Success("chronic.accommodation_removed");
        }

        // Day orchestration (rides the existing expanded-shelter day path).
        // Plan 193: conditions are permanent and accommodations explicit —
        // nothing runs on a tick; the tick only persists player-visible change.
        public void TickChronicConditions(int day)
        {
            if (_chronicConditions == null) return;

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

        public void ResetChronicConditions()
        {
            _chronicConditions = null;
            _chronicProducerBound = false;
            _chronicConditionsDirty = false;
        }
    }
}
