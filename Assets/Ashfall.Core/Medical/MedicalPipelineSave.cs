// SPDX-License-Identifier: MIT
// Task #133 — Versioned save envelope for the medical pipeline.
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Medical
{
    /// <summary>
    /// Persisted pipeline state: diagnosis knowledge, reservations, and the
    /// procedure schedule. The patient record is a projection and is never
    /// saved. Versioned; unknown future versions throw on load (house rule:
    /// throw on future, migrate on past).
    /// </summary>
    [Serializable]
    public sealed class MedicalPipelineSaveState
    {
        public const int CurrentVersion = 1;

        public int version = CurrentVersion;
        public DiagnosisKnowledgeSaveState diagnosis = new DiagnosisKnowledgeSaveState();
        public MedicalReservationSaveState reservations = new MedicalReservationSaveState();
        public MedicalProcedureScheduleSaveState procedures = new MedicalProcedureScheduleSaveState();
        /// <summary>Monotonic command/state version for stale-preview rejection.</summary>
        public long stateVersion;
    }

    /// <summary>
    /// Integrity findings for restored pipeline state. A finding is either
    /// repairable (deterministic rule documented on the row) or fatal (the load
    /// must reject with the precise code).
    /// </summary>
    [Serializable]
    public sealed class MedicalPipelineIntegrityFinding
    {
        public string code = string.Empty;
        public string detail = string.Empty;
        public bool fatal;

        public override string ToString() => (fatal ? "[FATAL] " : "[repairable] ") + code + " — " + detail;
    }
}
