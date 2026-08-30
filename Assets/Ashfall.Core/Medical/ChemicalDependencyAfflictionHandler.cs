// SPDX-License-Identifier: MIT
// Task #133 P1b — Chemical-dependency pipeline handler: detox starts via the pipeline.
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Medical
{
    /// <summary>
    /// Pipeline adapter over <see cref="ChemicalDependencySystem"/>. The
    /// dependency domain keeps every clinical rule (dependency levels,
    /// withdrawal profiles, the 120h managed / 72h cold-turkey clocks and
    /// their TickHours advancement); this handler only projects ledger state
    /// into the shared episode/symptom contract and forwards validated detox
    /// <b>starts</b> to the domain's existing Begin* APIs.
    ///
    /// <para>Behavioral parity rule: progression is never touched here — the
    /// medical_disease day owner keeps calling
    /// <c>ChemicalDependencySystem.TickHours</c> exactly as before. This
    /// handler reads and starts programs, it does not tick and it never
    /// schedules on <see cref="MedicalProcedureSchedule"/> (a second detox
    /// clock would double-advance withdrawal).</para>
    ///
    /// <para>Sub-case selection: one definition
    /// (<c>affliction_chemical_dependency</c>) covers every substance; the
    /// substance is chosen per call through the pipeline's
    /// <c>targetItem</c> (the catalog item id, e.g. <c>painkillers</c>).
    /// The episode identity stays <c>{survivor}:{definition}:0</c> — the
    /// substance is never stuffed into the affliction id.</para>
    ///
    /// <para>Diagnosis: the ledger is player-facing by design, so no
    /// SuspectFromEvidence / ConfirmForLegacySave traffic exists for this
    /// affliction; the knowledge store simply plays no role here.</para>
    /// </summary>
    public sealed class ChemicalDependencyAfflictionHandler : IAfflictionHandler
    {
        public const string SymptomCraving = "symptom_craving";
        public const string SymptomWithdrawalTremor = "symptom_withdrawal_tremor";
        public const string SymptomManagedWithdrawal = "symptom_managed_withdrawal";

        private readonly ChemicalDependencySystem _dependency;

        public ChemicalDependencyAfflictionHandler(ChemicalDependencySystem dependency)
        {
            _dependency = dependency ?? throw new ArgumentNullException(nameof(dependency));
        }

        public AfflictionId DefinitionId =>
            new AfflictionId(MedicalTreatmentCatalog.ChemicalDependencyId);

        public AfflictionEpisodeSnapshot? GetEpisode(Survivors.SurvivorId survivor)
        {
            if (survivor.IsEmpty) return null;
            ChemicalDependencyState? worst = WorstDependency(survivor.Value);
            if (worst == null) return null;

            // A dependency episode exists once a dependency has actually formed
            // (level >= threshold) or a withdrawal program is running; one dose
            // below the threshold is habituation, not an affliction.
            bool formed = worst.dependencyLevel >= ChemicalDependencySystem.DependencyThreshold;
            bool inProgram = worst.inManagedDetox || worst.inColdTurkey;
            if (!formed && !inProgram) return null;

            return new AfflictionEpisodeSnapshot
            {
                EpisodeId = AfflictionEpisodeId.Create(survivor, DefinitionId),
                DefinitionId = DefinitionId,
                Survivor = survivor,
                SeverityValue = worst.dependencyLevel * 100f,
                StageLabel = StageLabel(worst),
                IsActive = true,
                IsSymptomatic = true
            };
        }

        public IReadOnlyList<SymptomProjection> ProjectSymptoms(Survivors.SurvivorId survivor)
        {
            var symptoms = new List<SymptomProjection>();
            if (survivor.IsEmpty) return symptoms;
            var deps = _dependency.DependenciesFor(survivor.Value);
            if (deps == null || deps.Count == 0) return symptoms;

            // Aggregate across substances: one craving row at the worst
            // intensity, one withdrawal row for the strongest active program.
            float craving = 0f;
            bool coldTurkey = false;
            bool managed = false;
            foreach (var d in deps)
            {
                if (d == null || string.IsNullOrEmpty(d.itemId)) continue;
                craving = Math.Max(craving, d.dependencyLevel);
                if (d.inColdTurkey) coldTurkey = true;
                else if (d.inManagedDetox) managed = true;
            }

            if (craving <= 0f && !coldTurkey && !managed) return symptoms;
            var episode = AfflictionEpisodeId.Create(survivor, DefinitionId);
            if (craving > 0f)
            {
                symptoms.Add(new SymptomProjection
                {
                    SymptomId = SymptomCraving,
                    SourceEpisode = episode,
                    Presentation = "Craving",
                    Intensity = Math.Min(1f, craving)
                });
            }
            if (coldTurkey)
            {
                symptoms.Add(new SymptomProjection
                {
                    SymptomId = SymptomWithdrawalTremor,
                    SourceEpisode = episode,
                    Presentation = "Withdrawal tremors",
                    Intensity = 1f
                });
            }
            else if (managed)
            {
                symptoms.Add(new SymptomProjection
                {
                    SymptomId = SymptomManagedWithdrawal,
                    SourceEpisode = episode,
                    Presentation = "Managed withdrawal",
                    Intensity = 0.4f
                });
            }
            return symptoms;
        }

        public bool CouldHaveCondition(Survivors.SurvivorId survivor)
        {
            if (survivor.IsEmpty) return false;
            var deps = _dependency.DependenciesFor(survivor.Value);
            return deps != null && deps.Count > 0;
        }

        public string? ValidateTreatment(Survivors.SurvivorId survivor, string treatmentId, string? targetItem = null)
        {
            switch (treatmentId)
            {
                case MedicalTreatmentCatalog.TreatmentManagedDetox:
                case MedicalTreatmentCatalog.TreatmentColdTurkey:
                    if (string.IsNullOrEmpty(targetItem))
                        return "target_item_required";
                    var dep = Find(survivor.Value, targetItem);
                    if (dep == null)
                        return "missing_dependency";
                    if (dep.dependencyLevel < ChemicalDependencySystem.DependencyThreshold)
                        return "below_threshold";
                    if (treatmentId == MedicalTreatmentCatalog.TreatmentManagedDetox && dep.inManagedDetox)
                        return "already_in_treatment";
                    if (treatmentId == MedicalTreatmentCatalog.TreatmentColdTurkey && dep.inColdTurkey)
                        return "already_in_treatment";
                    return null;
                default:
                    return "treatment_not_for_affliction";
            }
        }

        public bool ApplyTreatment(Survivors.SurvivorId survivor, string treatmentId, string? targetItem = null)
        {
            if (string.IsNullOrEmpty(targetItem)) return false;
            return treatmentId switch
            {
                MedicalTreatmentCatalog.TreatmentManagedDetox =>
                    _dependency.BeginManagedDetox(survivor.Value, targetItem),
                MedicalTreatmentCatalog.TreatmentColdTurkey =>
                    _dependency.BeginColdTurkey(survivor.Value, targetItem),
                _ => false
            };
        }

        public bool HasResolved(Survivors.SurvivorId survivor)
        {
            if (survivor.IsEmpty) return true;
            var deps = _dependency.DependenciesFor(survivor.Value);
            if (deps == null || deps.Count == 0) return true;
            foreach (var d in deps)
            {
                if (d == null) continue;
                if (d.dependencyLevel >= ChemicalDependencySystem.DependencyThreshold
                    || d.inManagedDetox || d.inColdTurkey)
                    return false;
            }
            return true;
        }

        /// <summary>The dependency row a projection surfaces: highest level, ties broken by ordinal item id.</summary>
        private ChemicalDependencyState? WorstDependency(string survivorId)
        {
            var deps = _dependency.DependenciesFor(survivorId);
            if (deps == null || deps.Count == 0) return null;
            ChemicalDependencyState? worst = null;
            foreach (var d in deps)
            {
                if (d == null || string.IsNullOrEmpty(d.itemId)) continue;
                if (worst == null
                    || d.dependencyLevel > worst.dependencyLevel
                    || (d.dependencyLevel == worst.dependencyLevel
                        && string.CompareOrdinal(d.itemId, worst.itemId) < 0))
                    worst = d;
            }
            return worst;
        }

        private ChemicalDependencyState? Find(string survivorId, string itemId)
        {
            var deps = _dependency.DependenciesFor(survivorId);
            if (deps == null) return null;
            foreach (var d in deps)
            {
                if (d != null && string.Equals(d.itemId, itemId, StringComparison.Ordinal))
                    return d;
            }
            return null;
        }

        /// <summary>Stage label mirroring the existing ChemicalDependencyPanel projection.</summary>
        private static string StageLabel(ChemicalDependencyState d)
        {
            if (d.inManagedDetox) return "MANAGED DETOX";
            if (d.inColdTurkey) return "COLD TURKEY";
            return d.dependencyLevel >= ChemicalDependencySystem.DependencyThreshold
                ? "ADDICTED"
                : "HABITUATED";
        }
    }
}
