// SPDX-License-Identifier: MIT
// ============================================================================
// Ashfall Core : Expansion 35 — The Habit
// Subsystem    : Chemical Dependency Taper, Withdrawal Management & Care Policy Engine
// Authority    : docs/expansions/wave5/expansion_35_the_habit_plan.md
//                UNBLOCK-05_EXPANSION_WAVES_C3_EN_GATE.md §3.1, §5.3
// ============================================================================
using System;

namespace Ashfall.Core.Medical
{
    /// <summary>
    /// Dependency severity tier, mapping to ChemicalDependencySystem.DependencyLevel.
    /// </summary>
    public enum DependencySeverityTier
    {
        None       = 0,
        Mild       = 1,  // 1–25% severity (tolerance only, no withdrawal)
        Moderate   = 2,  // 26–55% severity (moderate withdrawal symptoms)
        Severe     = 3,  // 56–80% severity (acute withdrawal, care required)
        Critical   = 4   // >80% severity (life-threatening; full medical detox)
    }

    /// <summary>
    /// Withdrawal symptom severity during taper or cold-turkey.
    /// </summary>
    public enum WithdrawalSymptomBand
    {
        Asymptomatic = 0,   // no noticeable symptoms
        Discomfort   = 1,   // manageable; productivity penalty only
        Distress     = 2,   // significant; work capacity halved
        Acute        = 3,   // medical supervision required
        LifeThreatening = 4 // ICU-level care; mortality risk without staff
    }

    /// <summary>
    /// Care policy posture set by shelter governance.
    /// </summary>
    public enum CarePolicyPosture
    {
        Permissive    = 0,  // no restrictions; substances freely available
        Monitored     = 1,  // dispensing logged; usage tracked
        Controlled    = 2,  // prescription-only; quotas enforced
        Emergency     = 3   // full lockdown; medical rationing only
    }

    /// <summary>
    /// Mutable state for a survivor undergoing a managed taper program.
    /// Does NOT duplicate ChemicalDependencySystem.DependencyLevel — consumes it.
    /// </summary>
    public sealed class TaperProgramState
    {
        public string SurvivorId             { get; set; } = string.Empty;
        /// <summary>Current dependency severity (0..1000 permille, read from ChemicalDependencySystem).</summary>
        public int DependencyPermille        { get; set; } = 0;
        /// <summary>Target dose step-down per day (permille of current dose).</summary>
        public int DailyStepDownPermille     { get; set; } = 50;
        /// <summary>Current substitute dose level (0..1000 permille of full therapeutic dose).</summary>
        public int CurrentSubstituteDosePermille { get; set; } = 1000;
        /// <summary>Days of peer-support sessions completed this taper cycle.</summary>
        public int PeerSupportSessionsCompleted { get; set; } = 0;
        /// <summary>Whether the taper program is medically supervised.</summary>
        public bool IsMedicallySupervised    { get; set; } = false;
        /// <summary>Total taper days elapsed.</summary>
        public int TaperDaysElapsed          { get; set; } = 0;

        public TaperProgramState Clone() => new TaperProgramState
        {
            SurvivorId               = SurvivorId,
            DependencyPermille       = DependencyPermille,
            DailyStepDownPermille    = DailyStepDownPermille,
            CurrentSubstituteDosePermille = CurrentSubstituteDosePermille,
            PeerSupportSessionsCompleted = PeerSupportSessionsCompleted,
            IsMedicallySupervised    = IsMedicallySupervised,
            TaperDaysElapsed         = TaperDaysElapsed
        };
    }

    /// <summary>
    /// Immutable result of one taper day advance.
    /// </summary>
    public readonly struct TaperDayResult
    {
        /// <summary>New substitute dose permille after this day's step-down.</summary>
        public int NewSubstituteDosePermille  { get; }
        /// <summary>Withdrawal symptom band active today.</summary>
        public WithdrawalSymptomBand Symptoms { get; }
        /// <summary>True if the program has reached completion (dose ≤ 0).</summary>
        public bool TaperComplete             { get; }
        /// <summary>True if medical escalation is required immediately.</summary>
        public bool RequiresMedicalEscalation { get; }
        /// <summary>Productivity penalty permille from symptoms (0..1000).</summary>
        public int ProductivityPenaltyPermille { get; }

        public TaperDayResult(
            int newSubstituteDosePermille,
            WithdrawalSymptomBand symptoms,
            bool taperComplete,
            bool requiresMedicalEscalation,
            int productivityPenaltyPermille)
        {
            NewSubstituteDosePermille  = Math.Clamp(newSubstituteDosePermille, 0, 1000);
            Symptoms                   = symptoms;
            TaperComplete              = taperComplete;
            RequiresMedicalEscalation  = requiresMedicalEscalation;
            ProductivityPenaltyPermille = Math.Clamp(productivityPenaltyPermille, 0, 1000);
        }
    }

    /// <summary>
    /// Pure domain engine governing chemical dependency taper schedule computation,
    /// withdrawal symptom severity tiers, peer-support mitigation, and shelter care
    /// policy enforcement.
    /// Extends ChemicalDependencySystem (DependencyLevel, BeginManagedDetox, TickHours)
    /// and PharmaceuticalTabletEngine (substitution dosing) without duplicating the
    /// dependency ledger.
    /// Zero engine references, integer permille determinism.
    /// </summary>
    public static class DependencyTaperWithdrawalEngine
    {
        /// <summary>Substitute dose permille below which the taper program is considered complete.</summary>
        public const int TaperCompletionThreshold = 30;

        /// <summary>Peer-support sessions per cycle that confer full mitigation benefit.</summary>
        public const int FullPeerSupportSessionCount = 5;

        /// <summary>
        /// Computes the recommended daily step-down rate for a given dependency severity and supervision level.
        /// </summary>
        /// <param name="dependencyPermille">Dependency severity (0..1000).</param>
        /// <param name="isMedicallySupervised">True if a medical officer is overseeing the taper.</param>
        /// <param name="substituteMedicineAvailablePermille">
        ///     Available substitute medicine stock (0..1000). Low stock forces faster taper.
        /// </param>
        public static int ComputeRecommendedStepDown(
            int dependencyPermille,
            bool isMedicallySupervised,
            int substituteMedicineAvailablePermille)
        {
            dependencyPermille = Math.Clamp(dependencyPermille, 0, 1000);
            substituteMedicineAvailablePermille = Math.Clamp(substituteMedicineAvailablePermille, 0, 1000);

            // Base step-down: lower dependency = faster taper safe
            int baseStepDown = dependencyPermille switch
            {
                <= 250  => 150,  // Mild: 150 permille/day
                <= 550  => 80,   // Moderate: 80 permille/day
                <= 800  => 40,   // Severe: 40 permille/day
                _       => 20    // Critical: 20 permille/day (very slow to prevent crisis)
            };

            // Medical supervision allows a slightly faster safe taper
            if (isMedicallySupervised)
                baseStepDown = (baseStepDown * 120) / 100;

            // Stock shortage forces faster taper (dangerous)
            if (substituteMedicineAvailablePermille < 300)
            {
                int scarcityBonus = ((300 - substituteMedicineAvailablePermille) * 60) / 300;
                baseStepDown += scarcityBonus;
            }

            return Math.Clamp(baseStepDown, 10, 300);
        }

        /// <summary>
        /// Advances the taper program by one day. Mutates TaperProgramState in-place.
        /// </summary>
        /// <param name="program">Current taper program state (mutated).</param>
        /// <param name="peerSupportRunToday">True if a peer-support session occurred today.</param>
        public static TaperDayResult AdvanceTaperDay(TaperProgramState program, bool peerSupportRunToday)
        {
            if (program == null) throw new ArgumentNullException(nameof(program));

            if (peerSupportRunToday)
                program.PeerSupportSessionsCompleted++;

            // Apply step-down
            int previousDose = program.CurrentSubstituteDosePermille;
            int newDose      = Math.Max(0, previousDose - program.DailyStepDownPermille);
            program.CurrentSubstituteDosePermille = newDose;
            program.TaperDaysElapsed++;

            bool taperComplete = newDose <= TaperCompletionThreshold;

            // Withdrawal symptoms proportional to how far below therapeutic dose the survivor is
            int dosageGap = Math.Max(0, 1000 - newDose - program.DependencyPermille / 2);

            // Peer support mitigation
            int peerMitigation = Math.Min(FullPeerSupportSessionCount,
                                          program.PeerSupportSessionsCompleted);
            int mitigationPermille = (peerMitigation * 1000) / FullPeerSupportSessionCount;
            dosageGap = Math.Max(0, dosageGap - (dosageGap * mitigationPermille / 4) / 1000);

            // Symptom severity tier
            WithdrawalSymptomBand symptoms = dosageGap switch
            {
                <= 50  => WithdrawalSymptomBand.Asymptomatic,
                <= 200 => WithdrawalSymptomBand.Discomfort,
                <= 450 => WithdrawalSymptomBand.Distress,
                <= 700 => WithdrawalSymptomBand.Acute,
                _      => WithdrawalSymptomBand.LifeThreatening
            };

            // Medical escalation required for Acute+ on critical dependency without supervision
            bool escalation = symptoms >= WithdrawalSymptomBand.Acute &&
                              !program.IsMedicallySupervised;

            // Productivity penalty
            int productivityPenalty = symptoms switch
            {
                WithdrawalSymptomBand.Asymptomatic   => 0,
                WithdrawalSymptomBand.Discomfort     => 150,
                WithdrawalSymptomBand.Distress       => 500,
                WithdrawalSymptomBand.Acute          => 800,
                WithdrawalSymptomBand.LifeThreatening => 1000,
                _                                    => 0
            };

            return new TaperDayResult(newDose, symptoms, taperComplete, escalation, productivityPenalty);
        }

        /// <summary>
        /// Derives the dependency severity tier from a raw dependency permille score.
        /// Mirrors ChemicalDependencySystem.DependencyLevel without duplicating state ownership.
        /// </summary>
        public static DependencySeverityTier ClassifyDependencySeverity(int dependencyPermille) =>
            dependencyPermille switch
            {
                <= 0   => DependencySeverityTier.None,
                <= 250 => DependencySeverityTier.Mild,
                <= 550 => DependencySeverityTier.Moderate,
                <= 800 => DependencySeverityTier.Severe,
                _      => DependencySeverityTier.Critical
            };

        /// <summary>
        /// Evaluates whether a shelter care policy posture is compliant with the current
        /// population dependency burden.
        /// </summary>
        /// <param name="criticalCaseCount">Number of survivors in Critical tier.</param>
        /// <param name="totalShelterPopulation">Total shelter population.</param>
        /// <param name="currentPosture">Current shelter policy posture.</param>
        /// <returns>
        ///     Recommended policy posture — may be stricter than current if burden is high.
        /// </returns>
        public static CarePolicyPosture RecommendCarePolicy(
            int criticalCaseCount,
            int totalShelterPopulation,
            CarePolicyPosture currentPosture)
        {
            totalShelterPopulation = Math.Max(1, totalShelterPopulation);
            criticalCaseCount      = Math.Max(0, criticalCaseCount);

            int burdenPermille = (criticalCaseCount * 1000) / totalShelterPopulation;

            if (burdenPermille >= 100)       // ≥10% of shelter critically dependent
                return CarePolicyPosture.Emergency;
            if (burdenPermille >= 50)        // ≥5%
                return CarePolicyPosture.Controlled;
            if (burdenPermille >= 20)        // ≥2%
                return (CarePolicyPosture)Math.Max((int)currentPosture, (int)CarePolicyPosture.Monitored);

            return currentPosture; // burden low; current policy is sufficient
        }
    }
}
