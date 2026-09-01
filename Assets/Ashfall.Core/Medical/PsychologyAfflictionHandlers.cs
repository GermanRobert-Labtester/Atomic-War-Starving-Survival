// SPDX-License-Identifier: MIT
// Task #133 P1c — Observe-only psychology projection handlers.
using System;
using System.Collections.Generic;
using Ashfall.Core.Survivors;

namespace Ashfall.Core.Medical
{
    /// <summary>
    /// Shared behavior of the observe-only Phase-0 psychology handlers: they
    /// project combat trauma, somatic flashbacks, and guilt insomnia into the
    /// patient-record pipeline for display, and nothing else.
    ///
    /// <para>These conditions have no pipeline treatments — every
    /// <see cref="IAfflictionHandler.ValidateTreatment"/> answer is
    /// <c>treatment_not_for_affliction</c> — and no diagnosis traffic (the
    /// Phase-0 conditions are player-facing by design, so the knowledge
    /// store plays no role here; no SuspectFromEvidence /
    /// ConfirmForLegacySave traffic exists). The handlers hold no state of
    /// their own and never mutate their domain: the Phase-0 day owner keeps
    /// every clock exactly as before.</para>
    /// </summary>
    public abstract class PsychologyObserverHandlerBase : IAfflictionHandler
    {
        public abstract AfflictionId DefinitionId { get; }

        public abstract AfflictionEpisodeSnapshot? GetEpisode(Survivors.SurvivorId survivor);

        public abstract IReadOnlyList<SymptomProjection> ProjectSymptoms(Survivors.SurvivorId survivor);

        public abstract bool CouldHaveCondition(Survivors.SurvivorId survivor);

        public abstract bool HasResolved(Survivors.SurvivorId survivor);

        /// <summary>No pipeline treatment exists for psychology conditions.</summary>
        public string? ValidateTreatment(Survivors.SurvivorId survivor, string treatmentId, string? targetItem = null)
        {
            return "treatment_not_for_affliction";
        }

        /// <summary>Never applies: the pipeline refuses every treatment upstream.</summary>
        public bool ApplyTreatment(Survivors.SurvivorId survivor, string treatmentId, string? targetItem = null)
        {
            return false;
        }
    }

    /// <summary>
    /// Observe-only projection of <see cref="CombatTraumaSystem"/>
    /// hypervigilance (Task #133 P1c). Episode exists while hypervigilance is
    /// above zero; the combat-trauma domain keeps the combat reaction, the
    /// false-alarm rolls, and the decay clock.
    /// </summary>
    public sealed class CombatTraumaAfflictionHandler : PsychologyObserverHandlerBase
    {
        public const string SymptomHypervigilance = "symptom_hypervigilance";
        public const string StageHypervigilant = "HYPERVIGILANT";

        private readonly CombatTraumaSystem _trauma;

        public CombatTraumaAfflictionHandler(CombatTraumaSystem trauma)
        {
            _trauma = trauma ?? throw new ArgumentNullException(nameof(trauma));
        }

        public override AfflictionId DefinitionId =>
            new AfflictionId(MedicalTreatmentCatalog.CombatTraumaId);

        public override AfflictionEpisodeSnapshot? GetEpisode(Survivors.SurvivorId survivor)
        {
            if (survivor.IsEmpty) return null;
            float hypervigilance = _trauma.GetHypervigilanceLevel(survivor.Value);
            if (hypervigilance <= 0f) return null;

            return new AfflictionEpisodeSnapshot
            {
                EpisodeId = AfflictionEpisodeId.Create(survivor, DefinitionId),
                DefinitionId = DefinitionId,
                Survivor = survivor,
                SeverityValue = hypervigilance * 100f,
                StageLabel = StageHypervigilant,
                IsActive = true,
                IsSymptomatic = true
            };
        }

        public override IReadOnlyList<SymptomProjection> ProjectSymptoms(Survivors.SurvivorId survivor)
        {
            var symptoms = new List<SymptomProjection>();
            if (survivor.IsEmpty) return symptoms;
            float hypervigilance = _trauma.GetHypervigilanceLevel(survivor.Value);
            if (hypervigilance <= 0f) return symptoms;

            symptoms.Add(new SymptomProjection
            {
                SymptomId = SymptomHypervigilance,
                SourceEpisode = AfflictionEpisodeId.Create(survivor, DefinitionId),
                Presentation = "Hypervigilant — startles at every sound",
                Intensity = Math.Min(1f, hypervigilance)
            });
            return symptoms;
        }

        public override bool CouldHaveCondition(Survivors.SurvivorId survivor)
        {
            return !survivor.IsEmpty && _trauma.GetHypervigilanceLevel(survivor.Value) > 0f;
        }

        public override bool HasResolved(Survivors.SurvivorId survivor)
        {
            return survivor.IsEmpty || _trauma.GetHypervigilanceLevel(survivor.Value) <= 0f;
        }
    }

    /// <summary>
    /// Observe-only projection of <see cref="SomaticFlashbackSystem"/>
    /// (Task #133 P1c). The episode exists while a flashback is running
    /// (stage FLASHBACK, severity = remaining hours) or susceptibility has
    /// been seeded by trauma (stage SUSCEPTIBLE, severity = susceptibility
    /// 0..100). The sensory domain keeps the trigger rolls, grounding, and
    /// decay clocks.
    /// </summary>
    public sealed class SomaticFlashbackAfflictionHandler : PsychologyObserverHandlerBase
    {
        public const string SymptomFlashback = "symptom_flashback";
        public const string StageFlashback = "FLASHBACK";
        public const string StageSusceptible = "SUSCEPTIBLE";

        private readonly SomaticFlashbackSystem _flashbacks;

        public SomaticFlashbackAfflictionHandler(SomaticFlashbackSystem flashbacks)
        {
            _flashbacks = flashbacks ?? throw new ArgumentNullException(nameof(flashbacks));
        }

        public override AfflictionId DefinitionId =>
            new AfflictionId(MedicalTreatmentCatalog.SomaticFlashbackId);

        public override AfflictionEpisodeSnapshot? GetEpisode(Survivors.SurvivorId survivor)
        {
            if (survivor.IsEmpty) return null;
            float hours = _flashbacks.GetActiveFlashbackRemaining(survivor.Value);
            float susceptibility = _flashbacks.GetSusceptibility(survivor.Value);
            if (hours <= 0f && susceptibility <= 0f) return null;

            bool active = hours > 0f;
            return new AfflictionEpisodeSnapshot
            {
                EpisodeId = AfflictionEpisodeId.Create(survivor, DefinitionId),
                DefinitionId = DefinitionId,
                Survivor = survivor,
                SeverityValue = active ? hours : susceptibility * 100f,
                StageLabel = active ? StageFlashback : StageSusceptible,
                IsActive = true,
                IsSymptomatic = true
            };
        }

        public override IReadOnlyList<SymptomProjection> ProjectSymptoms(Survivors.SurvivorId survivor)
        {
            var symptoms = new List<SymptomProjection>();
            if (survivor.IsEmpty) return symptoms;
            if (_flashbacks.GetActiveFlashbackRemaining(survivor.Value) <= 0f) return symptoms;

            symptoms.Add(new SymptomProjection
            {
                SymptomId = SymptomFlashback,
                SourceEpisode = AfflictionEpisodeId.Create(survivor, DefinitionId),
                Presentation = "Flashback — sights and sounds that are not in the room",
                Intensity = 1f
            });
            return symptoms;
        }

        public override bool CouldHaveCondition(Survivors.SurvivorId survivor)
        {
            if (survivor.IsEmpty) return false;
            return _flashbacks.GetActiveFlashbackRemaining(survivor.Value) > 0f
                || _flashbacks.GetSusceptibility(survivor.Value) > 0f;
        }

        public override bool HasResolved(Survivors.SurvivorId survivor)
        {
            if (survivor.IsEmpty) return true;
            return _flashbacks.GetActiveFlashbackRemaining(survivor.Value) <= 0f
                && _flashbacks.GetSusceptibility(survivor.Value) <= 0f;
        }
    }

    /// <summary>
    /// Observe-only projection of <see cref="GuiltInsomniaSystem"/>
    /// (Task #133 P1c). The episode exists while insomnia severity is above
    /// zero; severity at or above
    /// <see cref="GuiltInsomniaSystem.HighSeverityThreshold"/> reads as
    /// CRITICAL INSOMNIA. Sedatives and dialogue stay outside the pipeline:
    /// the guilt domain keeps both levers and the decay clock.
    /// </summary>
    public sealed class GuiltInsomniaAfflictionHandler : PsychologyObserverHandlerBase
    {
        public const string SymptomInsomnia = "symptom_insomnia";
        public const string StageInsomnia = "INSOMNIA";
        public const string StageCriticalInsomnia = "CRITICAL INSOMNIA";

        private readonly GuiltInsomniaSystem _guilt;

        public GuiltInsomniaAfflictionHandler(GuiltInsomniaSystem guilt)
        {
            _guilt = guilt ?? throw new ArgumentNullException(nameof(guilt));
        }

        public override AfflictionId DefinitionId =>
            new AfflictionId(MedicalTreatmentCatalog.GuiltInsomniaId);

        public override AfflictionEpisodeSnapshot? GetEpisode(Survivors.SurvivorId survivor)
        {
            if (survivor.IsEmpty) return null;
            float severity = _guilt.GetInsomniaSeverity(survivor.Value);
            if (severity <= 0f) return null;

            return new AfflictionEpisodeSnapshot
            {
                EpisodeId = AfflictionEpisodeId.Create(survivor, DefinitionId),
                DefinitionId = DefinitionId,
                Survivor = survivor,
                SeverityValue = severity * 100f,
                StageLabel = severity >= GuiltInsomniaSystem.HighSeverityThreshold
                    ? StageCriticalInsomnia
                    : StageInsomnia,
                IsActive = true,
                IsSymptomatic = true
            };
        }

        public override IReadOnlyList<SymptomProjection> ProjectSymptoms(Survivors.SurvivorId survivor)
        {
            var symptoms = new List<SymptomProjection>();
            if (survivor.IsEmpty) return symptoms;
            float severity = _guilt.GetInsomniaSeverity(survivor.Value);
            if (severity <= 0f) return symptoms;

            symptoms.Add(new SymptomProjection
            {
                SymptomId = SymptomInsomnia,
                SourceEpisode = AfflictionEpisodeId.Create(survivor, DefinitionId),
                Presentation = "Sleepless — guilt keeps the ceiling in view",
                Intensity = Math.Min(1f, severity)
            });
            return symptoms;
        }

        public override bool CouldHaveCondition(Survivors.SurvivorId survivor)
        {
            return !survivor.IsEmpty && _guilt.GetInsomniaSeverity(survivor.Value) > 0f;
        }

        public override bool HasResolved(Survivors.SurvivorId survivor)
        {
            return survivor.IsEmpty || _guilt.GetInsomniaSeverity(survivor.Value) <= 0f;
        }
    }
}
