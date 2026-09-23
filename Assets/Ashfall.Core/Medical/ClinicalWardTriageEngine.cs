// SPDX-License-Identifier: MIT
// ============================================================================
// Ashfall Core : Expansion 38 — The Ward
// Subsystem    : Clinical Triage Priority, Surgical Roster & Sterile Supply Engine
// Authority    : docs/expansions/wave6/expansion_38_the_ward_plan.md
//                WAVE6_INDEX.md
// ============================================================================
using System;

namespace Ashfall.Core.Medical
{
    /// <summary>
    /// Clinical triage prioritization category.
    /// </summary>
    public enum TriagePriorityTier
    {
        Minimal   = 0, // Minor wounds/walking wounded; treatment can be delayed without hazard
        Delayed   = 1, // Serious condition; stable vitals, care can be queued safely
        Immediate = 2, // Life/limb threat; surgical or intensive stabilization required immediately
        Expectant = 3  // Lethal trauma/terminal; palliative focus under severe resource scarcity
    }

    /// <summary>
    /// Sanitation and sterility status of a medical ward or surgical suite.
    /// </summary>
    public enum WardCleanlinessGrade
    {
        Contaminated       = 0, // Severe bio-burden; extreme nosocomial risk
        BasicSanitation    = 1, // Basic antiseptic washdown; routine inpatient care
        AntisepticStandard = 2, // Cleaned ward; suitable for minor procedures and post-op
        SterileField       = 3  // Autoclaved instruments and scrubbed theater; surgical ready
    }

    /// <summary>
    /// Mutable state of a hospital or shelter medical ward.
    /// Extends MedicalWardSystem and AdvancedSurgicalWardSystem without duplicating beds or admission records.
    /// </summary>
    public sealed class ClinicalWardState
    {
        public string WardId                     { get; set; } = string.Empty;
        public int TotalBeds                     { get; set; } = 10;
        public int OccupiedBeds                  { get; set; } = 0;
        public int SterileSupplyStockPermille    { get; set; } = 800;
        public int StaffingReadinessPermille     { get; set; } = 750;
        public WardCleanlinessGrade Cleanliness  { get; set; } = WardCleanlinessGrade.AntisepticStandard;
        public int IsolationBedsTotal           { get; set; } = 2;
        public int IsolationBedsOccupied         { get; set; } = 0;

        public ClinicalWardState Clone() => new ClinicalWardState
        {
            WardId                  = WardId,
            TotalBeds               = TotalBeds,
            OccupiedBeds            = OccupiedBeds,
            SterileSupplyStockPermille = SterileSupplyStockPermille,
            StaffingReadinessPermille = StaffingReadinessPermille,
            Cleanliness             = Cleanliness,
            IsolationBedsTotal      = IsolationBedsTotal,
            IsolationBedsOccupied   = IsolationBedsOccupied
        };
    }

    /// <summary>
    /// Immutable result of a patient triage assessment.
    /// </summary>
    public readonly struct TriageEvaluationResult
    {
        public TriagePriorityTier AssignedPriority    { get; }
        public int EstimatedUrgencyMinutes            { get; }
        public bool BedAvailable                      { get; }
        public bool RequiresIsolation                 { get; }
        public string RecommendationNotice            { get; }

        public TriageEvaluationResult(
            TriagePriorityTier assignedPriority,
            int estimatedUrgencyMinutes,
            bool bedAvailable,
            bool requiresIsolation,
            string recommendationNotice)
        {
            AssignedPriority       = assignedPriority;
            EstimatedUrgencyMinutes = Math.Max(0, estimatedUrgencyMinutes);
            BedAvailable           = bedAvailable;
            RequiresIsolation      = requiresIsolation;
            RecommendationNotice   = recommendationNotice ?? string.Empty;
        }
    }

    /// <summary>
    /// Immutable result of surgical preparation evaluation.
    /// </summary>
    public readonly struct SurgicalReadinessResult
    {
        public bool IsApprovedForSurgery               { get; }
        public int ShockRiskPermille                   { get; }
        public int InfectionRiskPermille               { get; }
        public int SterileSuppliesConsumedPermille     { get; }
        public string BottleneckReason                 { get; }

        public SurgicalReadinessResult(
            bool isApprovedForSurgery,
            int shockRiskPermille,
            int infectionRiskPermille,
            int sterileSuppliesConsumedPermille,
            string bottleneckReason)
        {
            IsApprovedForSurgery           = isApprovedForSurgery;
            ShockRiskPermille              = Math.Clamp(shockRiskPermille, 0, 1000);
            InfectionRiskPermille          = Math.Clamp(infectionRiskPermille, 0, 1000);
            SterileSuppliesConsumedPermille = Math.Clamp(sterileSuppliesConsumedPermille, 0, 1000);
            BottleneckReason               = bottleneckReason ?? string.Empty;
        }
    }

    /// <summary>
    /// Pure domain engine governing clinical triage, surgical roster readiness,
    /// sterile consumable utilization, and nosocomial infection risks.
    /// Extends MedicalWardSystem and AdvancedSurgicalWardSystem seams.
    /// Zero engine references, integer permille determinism.
    /// </summary>
    public static class ClinicalWardTriageEngine
    {
        public const int MinimumSterileStockForSurgeryPermille = 300;
        public const int MinimumStaffingForSurgeryPermille = 500;

        /// <summary>
        /// Evaluates a patient upon arrival to assign triage priority tier and bed routing.
        /// </summary>
        /// <param name="traumaSeverityPermille">Severity of injury/illness (0..1000).</param>
        /// <param name="vitalStabilityPermille">Stability of heart rate, breathing, perfusion (0..1000).</param>
        /// <param name="isContagious">Whether the patient presents airborne/contact pathogen symptoms.</param>
        /// <param name="ward">Current ward state.</param>
        public static TriageEvaluationResult EvaluatePatientTriage(
            int traumaSeverityPermille,
            int vitalStabilityPermille,
            bool isContagious,
            ClinicalWardState ward)
        {
            if (ward == null) throw new ArgumentNullException(nameof(ward));

            traumaSeverityPermille = Math.Clamp(traumaSeverityPermille, 0, 1000);
            vitalStabilityPermille = Math.Clamp(vitalStabilityPermille, 0, 1000);

            TriagePriorityTier priority;
            int urgencyMinutes;

            if (traumaSeverityPermille > 850 && vitalStabilityPermille < 150)
            {
                priority = TriagePriorityTier.Expectant;
                urgencyMinutes = 15;
            }
            else if (traumaSeverityPermille > 600 || vitalStabilityPermille < 400)
            {
                priority = TriagePriorityTier.Immediate;
                urgencyMinutes = 30;
            }
            else if (traumaSeverityPermille > 300 || vitalStabilityPermille < 700)
            {
                priority = TriagePriorityTier.Delayed;
                urgencyMinutes = 180;
            }
            else
            {
                priority = TriagePriorityTier.Minimal;
                urgencyMinutes = 720;
            }

            bool bedAvailable;
            string notice;

            if (isContagious)
            {
                bedAvailable = (ward.IsolationBedsTotal - ward.IsolationBedsOccupied) > 0;
                notice = bedAvailable
                    ? "Patient routed to negative-pressure isolation bed."
                    : "ALERT: Isolation ward saturated; cohort containment required.";
            }
            else
            {
                bedAvailable = (ward.TotalBeds - ward.OccupiedBeds) > 0;
                notice = bedAvailable
                    ? "Patient assigned to general clinical ward bed."
                    : "Ward capacity reached; overflow observation required.";
            }

            return new TriageEvaluationResult(
                priority,
                urgencyMinutes,
                bedAvailable,
                isContagious,
                notice);
        }

        /// <summary>
        /// Preflights an operating suite and surgical team for a procedure.
        /// </summary>
        public static SurgicalReadinessResult EvaluateSurgicalPreparation(
            ClinicalWardState ward,
            int procedureComplexityPermille,
            int patientConditionPermille,
            int surgerySeed)
        {
            if (ward == null) throw new ArgumentNullException(nameof(ward));

            procedureComplexityPermille = Math.Clamp(procedureComplexityPermille, 0, 1000);
            patientConditionPermille    = Math.Clamp(patientConditionPermille, 0, 1000);

            if (ward.Cleanliness < WardCleanlinessGrade.AntisepticStandard)
            {
                return new SurgicalReadinessResult(
                    false, 700, 850, 0,
                    "Operating theater lacks necessary antiseptic standard; surgery denied.");
            }

            if (ward.SterileSupplyStockPermille < MinimumSterileStockForSurgeryPermille)
            {
                return new SurgicalReadinessResult(
                    false, 500, 750, 0,
                    "Sterile dressings, suture, and anesthetic supply below safety threshold.");
            }

            if (ward.StaffingReadinessPermille < MinimumStaffingForSurgeryPermille)
            {
                return new SurgicalReadinessResult(
                    false, 600, 500, 0,
                    "Surgical surgical team staffing insufficient for procedure execution.");
            }

            // Supply consumption scales with complexity
            int suppliesConsumed = Math.Clamp(
                (procedureComplexityPermille * 150) / 1000 + 100,
                50,
                ward.SterileSupplyStockPermille);

            // Deduct supply
            ward.SterileSupplyStockPermille = Math.Max(0, ward.SterileSupplyStockPermille - suppliesConsumed);

            // Shock calculation
            int baseShock = (procedureComplexityPermille * 50 + (1000 - patientConditionPermille) * 50) / 100;
            int staffingMitigation = (ward.StaffingReadinessPermille * 30) / 100;
            int shockRisk = Math.Clamp(baseShock - staffingMitigation, 50, 950);

            // Infection risk derived from ward cleanliness
            int infectionRisk = ward.Cleanliness switch
            {
                WardCleanlinessGrade.SterileField       => 40,
                WardCleanlinessGrade.AntisepticStandard => 140,
                WardCleanlinessGrade.BasicSanitation    => 350,
                _                                       => 750
            };

            // Seeded variance ±5%
            int hash = HashCode.Combine(surgerySeed, procedureComplexityPermille, (int)ward.Cleanliness);
            int variance = ((hash & 0x7FFFFFFF) % 11) - 5;
            shockRisk = Math.Clamp(shockRisk + variance * 5, 20, 980);

            return new SurgicalReadinessResult(
                true,
                shockRisk,
                infectionRisk,
                suppliesConsumed,
                string.Empty);
        }

        /// <summary>
        /// Computes projected monthly patient admission capacity.
        /// </summary>
        public static int ComputeBedTurnoverCapacity(ClinicalWardState ward, int averageLengthOfStayDays)
        {
            if (ward == null) throw new ArgumentNullException(nameof(ward));
            averageLengthOfStayDays = Math.Clamp(averageLengthOfStayDays, 1, 60);

            int turnsPerMonth = 30 / averageLengthOfStayDays;
            return ward.TotalBeds * Math.Max(1, turnsPerMonth);
        }

        /// <summary>
        /// Calculates nosocomial infection probability permille for long-stay inpatients.
        /// </summary>
        public static int CalculateNosocomialInfectionRisk(
            WardCleanlinessGrade cleanliness,
            int wardOccupancyPermille,
            int sterileSupplyPermille)
        {
            wardOccupancyPermille = Math.Clamp(wardOccupancyPermille, 0, 1000);
            sterileSupplyPermille = Math.Clamp(sterileSupplyPermille, 0, 1000);

            int baseRisk = cleanliness switch
            {
                WardCleanlinessGrade.SterileField       => 15,
                WardCleanlinessGrade.AntisepticStandard => 60,
                WardCleanlinessGrade.BasicSanitation    => 180,
                _                                       => 450
            };

            int crowdingPenalty = (wardOccupancyPermille > 800)
                ? ((wardOccupancyPermille - 800) * 150) / 200
                : 0;

            int supplyDeficit = (sterileSupplyPermille < 400)
                ? ((400 - sterileSupplyPermille) * 100) / 400
                : 0;

            return Math.Clamp(baseRisk + crowdingPenalty + supplyDeficit, 10, 950);
        }
    }
}
