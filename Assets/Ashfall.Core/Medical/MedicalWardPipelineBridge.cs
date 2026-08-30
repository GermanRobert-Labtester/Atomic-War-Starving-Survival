// SPDX-License-Identifier: MIT
// Task #133 P1b — Ward procedures that have a pipeline treatment route through it.
using System;

namespace Ashfall.Core.Medical
{
    /// <summary>
    /// Bridge between the medical ward's procedure log and the unified
    /// medical pipeline (Task #133 P1b). Ward procedures whose clinical
    /// effect already exists as a pipeline treatment (<c>proc_bandage</c> →
    /// <c>treatment_bandage</c>, <c>proc_chelation</c> →
    /// <c>treatment_anti_rad</c>) execute through the pipeline's validated
    /// transaction (contraindication check → consume → apply) and are only
    /// recorded in the ward log when the pipeline commits. Procedures with
    /// no pipeline treatment (e.g. <c>proc_surgery</c> — no wound system
    /// exists) keep the ward's log-only behaviour, and an unbound pipeline
    /// (headless/CLI sessions) preserves the legacy log-only path exactly.
    ///
    /// <para>The ward never invents clinical outcomes: the clinical effect
    /// comes from the owning domain handler through the pipeline; the ward
    /// only records that the procedure was performed in a bed.</para>
    /// </summary>
    public static class MedicalWardPipelineBridge
    {
        /// <summary>
        /// The authored mapping from ward procedure id to pipeline treatment
        /// id. Only procedures whose clinical effect exists in the pipeline
        /// are mapped; everything else stays log-only.
        /// </summary>
        public static string? TreatmentForProcedure(string procedureId)
        {
            return procedureId switch
            {
                "proc_bandage" => MedicalTreatmentCatalog.TreatmentBandage,
                "proc_chelation" => MedicalTreatmentCatalog.TreatmentAntiRad,
                _ => null
            };
        }

        /// <summary>
        /// Run a ward procedure: admission gate first (same semantics as the
        /// ward domain), then the pipeline treatment when one is mapped and
        /// the pipeline is bound, then the ward log. On pipeline failure the
        /// ward log is NOT written — a procedure that consumed nothing and
        /// treated nothing did not happen.
        /// </summary>
        public static MedicalWardProcedureResult RunProcedure(
            MedicalWardSystem ward,
            MedicalPipelineCoordinator? pipeline,
            string patientId,
            string procedureId,
            int day)
        {
            if (ward == null)
                return MedicalWardProcedureResult.Fail("missing_ward");
            if (string.IsNullOrEmpty(patientId))
                return MedicalWardProcedureResult.Fail("missing_patient_id");
            if (string.IsNullOrEmpty(procedureId))
                return MedicalWardProcedureResult.Fail("missing_procedure_id");

            // Admission gate before any pipeline work — the ward owns bed
            // state, and an unadmitted patient must fail with the ward's own
            // reason code regardless of pipeline state.
            if (ward.GetActiveAdmission(patientId) == null)
                return MedicalWardProcedureResult.Fail("patient_not_admitted");

            string? treatmentId = TreatmentForProcedure(procedureId);
            if (pipeline != null && treatmentId != null)
            {
                // The pipeline is keyed by SurvivorId. A malformed patient id
                // cannot be treated through the pipeline; fall back to the
                // legacy log-only path rather than inventing a rejection the
                // ward domain never produced.
                if (Survivors.SurvivorId.TryParse(patientId, out var survivor))
                {
                    var result = pipeline.ExecuteTreatment(survivor, treatmentId);
                    if (!result.Success)
                        return MedicalWardProcedureResult.Fail(result.ReasonCode);
                }
            }

            // Clinical effect applied (or procedure is log-only): record the
            // ward event through the domain authority.
            return ward.RunProcedure(patientId, procedureId, day);
        }
    }
}
