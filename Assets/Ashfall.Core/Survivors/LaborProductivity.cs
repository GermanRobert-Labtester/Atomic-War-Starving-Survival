// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core.Survivors
{
    /// <summary>
    /// Plan 24B (Task A2) — data-authored overwork consequence magnitudes,
    /// loaded from the duty-role catalog's additive <c>overwork</c> block.
    /// Defaults mirror the authored values so unbound/test paths behave
    /// consistently. Legacy parity: no overwork effect existed before A2, so
    /// any application of these magnitudes is a flagged behavior addition.
    /// </summary>
    public sealed class DutyOverworkRules
    {
        public const float DefaultFatiguePerExcessHour = 0.5f;
        public const float DefaultMoralePerExcessHour = -0.25f;
        public const int DefaultYieldPenaltyPermille = 150;

        /// <summary>Fatigue points per excess committed hour per day (≥ 0).</summary>
        public float FatiguePerExcessHour { get; }
        /// <summary>Morale points per excess committed hour per day (≤ 0).</summary>
        public float MoralePerExcessHour { get; }
        /// <summary>Yield-modifier penalty while overworked, in permille (≥ 0,
        /// bounded by the yield band's absolute floor).</summary>
        public int YieldPenaltyPermille { get; }

        public DutyOverworkRules(
            float fatiguePerExcessHour,
            float moralePerExcessHour,
            int yieldPenaltyPermille)
        {
            FatiguePerExcessHour = Math.Max(0f, fatiguePerExcessHour);
            MoralePerExcessHour = Math.Min(0f, moralePerExcessHour);
            YieldPenaltyPermille = Math.Max(0, yieldPenaltyPermille);
        }

        public static DutyOverworkRules Default() => new DutyOverworkRules(
            DefaultFatiguePerExcessHour,
            DefaultMoralePerExcessHour,
            DefaultYieldPenaltyPermille);
    }

    /// <summary>
    /// Plan 24B (Task A2) — data-authored skill-to-yield band for the shared
    /// worker-productivity contract, loaded from the duty-role catalog's
    /// additive <c>worker_yield</c> block.
    /// </summary>
    public sealed class WorkerYieldBand
    {
        public const int DefaultFloorPermille = 750;
        public const int DefaultCapPermille = 1050;
        public const int DefaultImpairedPenaltyPermille = 100;
        public const int DefaultAbsoluteFloorPermille = 500;

        /// <summary>Yield modifier at skill 0 (permille of nominal output).</summary>
        public int FloorPermille { get; }
        /// <summary>Yield modifier at skill 100 with no degradation.</summary>
        public int CapPermille { get; }
        /// <summary>Penalty while the worker's fitness level is Impaired.</summary>
        public int ImpairedPenaltyPermille { get; }
        /// <summary>Hard clamp floor after penalties.</summary>
        public int AbsoluteFloorPermille { get; }

        public WorkerYieldBand(
            int floorPermille,
            int capPermille,
            int impairedPenaltyPermille,
            int absoluteFloorPermille)
        {
            FloorPermille = Math.Max(100, floorPermille);
            CapPermille = Math.Max(FloorPermille, capPermille);
            ImpairedPenaltyPermille = Math.Max(0, impairedPenaltyPermille);
            AbsoluteFloorPermille = Math.Min(FloorPermille, Math.Max(100, absoluteFloorPermille));
        }

        public static WorkerYieldBand Default() => new WorkerYieldBand(
            DefaultFloorPermille, DefaultCapPermille,
            DefaultImpairedPenaltyPermille, DefaultAbsoluteFloorPermille);
    }

    /// <summary>
    /// One worker's productivity projection: skill level + current fitness +
    /// overwork state, translated into a bounded yield modifier. Read-only —
    /// the verdict never mutates needs or producer state.
    /// </summary>
    public sealed class WorkerProductivityVerdict
    {
        public string SurvivorId { get; }
        public string SkillId { get; }
        /// <summary>Campaign skill level, 0..100. Zero when unknown.</summary>
        public float SkillLevel { get; }
        public FitnessLevel Fitness { get; }
        public bool Overworked { get; }
        /// <summary>Bounded yield modifier in permille of nominal output.
        /// Exactly 1000 when nothing is bound (the legacy-equivalent path).</summary>
        public int YieldModifierPermille { get; }
        /// <summary>Stable, ordered note ids (never empty when degraded).</summary>
        public IReadOnlyList<string> Notes { get; }

        public WorkerProductivityVerdict(
            string survivorId,
            string skillId,
            float skillLevel,
            FitnessLevel fitness,
            bool overworked,
            int yieldModifierPermille,
            IReadOnlyList<string> notes)
        {
            SurvivorId = survivorId ?? string.Empty;
            SkillId = skillId ?? string.Empty;
            SkillLevel = Math.Clamp(skillLevel, 0f, 100f);
            Fitness = fitness;
            Overworked = overworked;
            YieldModifierPermille = yieldModifierPermille;
            Notes = notes ?? new List<string>().AsReadOnly();
        }
    }

    /// <summary>
    /// Plan 24B §24B.23–24B.28 (Task A2) — the ONE shared worker-context seam.
    /// Producers pass their worker id and the relevant campaign skill id; the
    /// contract resolves skill level (campaign skill authority), fitness, and
    /// the overwork flag (duty-hour ledger) and returns a bounded yield
    /// modifier plus stable reason ids. There is deliberately NO per-producer
    /// setter: the host binds the three resolvers once and every producer
    /// shares them.
    ///
    /// <para>Legacy parity: when no resolver is bound (or a resolver returns
    /// nothing for the worker), <see cref="Resolve"/> returns null and the
    /// producer MUST keep its exact legacy behavior — the unbound path is the
    /// parity path.</para>
    /// </summary>
    public sealed class WorkerProductivityContract
    {
        public const string NoteSkillLow = "worker_skill_low";
        public const string NoteFitnessImpaired = "worker_fitness_impaired";
        public const string NoteOverworked = "worker_overworked";

        /// <summary>(survivorId, skillId) → skill level 0..100, or null when
        /// either id is unknown to the campaign skill authority.</summary>
        public Func<string, string, float?>? SkillLevelResolver { get; set; }
        /// <summary>survivorId → current base fitness level, or null when
        /// unbound/unknown.</summary>
        public Func<string, FitnessLevel?>? FitnessResolver { get; set; }
        /// <summary>survivorId → overwork flag from the duty-hour ledger.</summary>
        public Func<string, bool>? OverworkResolver { get; set; }

        public DutyOverworkRules OverworkRules { get; set; } = DutyOverworkRules.Default();
        public WorkerYieldBand YieldBand { get; set; } = WorkerYieldBand.Default();

        /// <summary>
        /// Resolve the worker's productivity verdict, or null when the worker
        /// or skill is unknown / no resolvers are bound (the legacy path).
        /// </summary>
        public WorkerProductivityVerdict? Resolve(string survivorId, string skillId)
        {
            if (string.IsNullOrEmpty(survivorId)) return null;

            float? skillLevel = null;
            if (SkillLevelResolver != null && !string.IsNullOrEmpty(skillId))
                skillLevel = SkillLevelResolver(survivorId, skillId);
            if (skillLevel == null && !string.IsNullOrEmpty(skillId))
                return null; // worker has no known level for this skill — producer keeps legacy
            if (skillLevel == null)
                return null; // skillless labor (e.g. intake sleeper) — not a productivity input

            FitnessLevel? fitness = FitnessResolver?.Invoke(survivorId);
            bool overworked = OverworkResolver?.Invoke(survivorId) ?? false;

            var band = YieldBand;
            int modifier = band.FloorPermille
                + (int)Math.Round((band.CapPermille - band.FloorPermille)
                    * Math.Clamp(skillLevel ?? 0f, 0f, 100f) / 100f);

            var notes = new List<string>();
            if ((skillLevel ?? 0f) < 25f) notes.Add(NoteSkillLow);
            if (fitness == FitnessLevel.Impaired)
            {
                modifier -= band.ImpairedPenaltyPermille;
                notes.Add(NoteFitnessImpaired);
            }
            if (fitness >= FitnessLevel.Unfit)
            {
                // Unfit/incapacitated workers are not reachable through allowed
                // duty paths; treat as impaired-equivalent penalty if a caller
                // still resolves them (e.g. direct panel cook).
                modifier -= band.ImpairedPenaltyPermille;
                notes.Add(NoteFitnessImpaired);
            }
            if (overworked)
            {
                modifier -= OverworkRules.YieldPenaltyPermille;
                notes.Add(NoteOverworked);
            }
            if (modifier < band.AbsoluteFloorPermille) modifier = band.AbsoluteFloorPermille;
            if (modifier > band.CapPermille) modifier = band.CapPermille;

            return new WorkerProductivityVerdict(
                survivorId, skillId, skillLevel ?? 0f,
                fitness ?? FitnessLevel.Fit, overworked, modifier, notes);
        }
    }
}
