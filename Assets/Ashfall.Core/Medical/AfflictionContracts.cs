// SPDX-License-Identifier: MIT
// Task #133 — Common affliction contract: episodes, symptoms, treatment.
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Medical
{
    /// <summary>How the player currently knows about an episode (Phase 10).</summary>
    public enum DiagnosisStatus
    {
        /// <summary>No knowledge recorded. The condition may still exist and progress.</summary>
        Unknown = 0,
        /// <summary>Observable presentation suggests the condition; not confirmed.</summary>
        Suspected = 1,
        /// <summary>Confirmed through an explicit diagnose operation or an existing clinical record.</summary>
        Confirmed = 2,
        /// <summary>Explicitly excluded. Kept so re-suspecting is a visible state change.</summary>
        RuledOut = 3
    }

    /// <summary>Read-only snapshot of one active affliction episode for a survivor.</summary>
    [Serializable]
    public sealed class AfflictionEpisodeSnapshot
    {
        public AfflictionEpisodeId EpisodeId;
        public AfflictionId DefinitionId;
        public Survivors.SurvivorId Survivor;

        /// <summary>Domain-specific severity value (0..100 lung degradation, phase index, dose band...). Not comparable across afflictions.</summary>
        public float SeverityValue;

        /// <summary>Human-readable stage label owned by the domain ("SEVERE COUGH", "Prodromal", ...).</summary>
        public string StageLabel = string.Empty;

        /// <summary>True when the domain considers the condition active (progressing or requiring care).</summary>
        public bool IsActive;

        /// <summary>True when the condition has crossed into a state the player cannot miss (symptom-presenting).</summary>
        public bool IsSymptomatic;

        public AfflictionEpisodeSnapshot Clone() => new AfflictionEpisodeSnapshot
        {
            EpisodeId = EpisodeId,
            DefinitionId = DefinitionId,
            Survivor = Survivor,
            SeverityValue = SeverityValue,
            StageLabel = StageLabel,
            IsActive = IsActive,
            IsSymptomatic = IsSymptomatic
        };
    }

    /// <summary>One observable symptom projected by an affliction.</summary>
    [Serializable]
    public sealed class SymptomProjection
    {
        /// <summary>Stable snake_case symptom id, e.g. <c>symptom_severe_cough</c>.</summary>
        public string SymptomId = string.Empty;

        /// <summary>The episode producing this symptom.</summary>
        public AfflictionEpisodeId SourceEpisode;

        /// <summary>Player-facing presentation text owned by the domain.</summary>
        public string Presentation = string.Empty;

        /// <summary>Hidden symptoms exist in the model but are never shown (reserved; current symptoms are all observable).</summary>
        public bool Observable = true;

        /// <summary>Severity weight 0..1 for prognosis derivation (domain-defined meaning).</summary>
        public float Intensity;

        public SymptomProjection Clone() => new SymptomProjection
        {
            SymptomId = SymptomId,
            SourceEpisode = SourceEpisode,
            Presentation = Presentation,
            Observable = Observable,
            Intensity = Intensity
        };
    }

    /// <summary>
    /// Per-domain adapter the medical pipeline coordinates through (Phase 5).
    /// A handler owns one affliction definition: it reads its domain's state,
    /// projects symptoms, validates contraindications, and applies treatments.
    /// The pipeline never reaches into domain internals and never re-implements
    /// domain rules with <c>if (id == X)</c> branches.
    /// </summary>
    public interface IAfflictionHandler
    {
        /// <summary>The affliction definition this handler owns.</summary>
        AfflictionId DefinitionId { get; }

        /// <summary>
        /// Snapshot the survivor's current episode, or null when the condition
        /// is absent. Read-only; must not mutate domain state.
        /// </summary>
        AfflictionEpisodeSnapshot? GetEpisode(Survivors.SurvivorId survivor);

        /// <summary>Project observable symptoms for the survivor (empty when none).</summary>
        IReadOnlyList<SymptomProjection> ProjectSymptoms(Survivors.SurvivorId survivor);

        /// <summary>
        /// Whether this survivor can plausibly have (or be suspected of) this
        /// condition at all — used by the diagnose command to reject diagnosing
        /// a healthy survivor. Must not mutate domain state.
        /// </summary>
        bool CouldHaveCondition(Survivors.SurvivorId survivor);

        /// <summary>
        /// Treatment contraindication check. Returns null when the treatment may
        /// be applied, otherwise a stable snake_case reason code. Must not mutate.
        ///
        /// <para><paramref name="targetItem"/> is an optional domain-specific
        /// selector for afflictions that span several sub-cases under one
        /// definition (e.g. the substance item id for chemical dependency).
        /// Handlers that model one episode per survivor ignore it.</para>
        /// </summary>
        string? ValidateTreatment(Survivors.SurvivorId survivor, string treatmentId, string? targetItem = null);

        /// <summary>Apply the treatment to domain state. Called only after the pipeline has validated and consumed resources. Returns false if the domain rejects the application (the pipeline then reports a failure). <paramref name="targetItem"/> selects the sub-case exactly as in <see cref="ValidateTreatment"/>.</summary>
        bool ApplyTreatment(Survivors.SurvivorId survivor, string treatmentId, string? targetItem = null);

        /// <summary>Whether the condition has resolved (e.g. severity reached 0) and a PatientRecovered event should be considered.</summary>
        bool HasResolved(Survivors.SurvivorId survivor);
    }
}
