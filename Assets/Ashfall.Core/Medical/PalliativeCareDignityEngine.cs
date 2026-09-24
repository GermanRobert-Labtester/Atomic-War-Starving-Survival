// SPDX-License-Identifier: MIT
// ============================================================================
// Ashfall Core : Expansion 24 — The Long Goodbye
// Subsystem    : Palliative Care, Dignity & Grief Stage Engine
// Authority    : docs/expansions/wave3/expansion_24_the_long_goodbye_plan.md
//                UNBLOCK-05_EXPANSION_WAVES_C3_EN_GATE.md §3.1, §5.3
// ============================================================================
using System;

namespace Ashfall.Core.Medical
{
    /// <summary>
    /// Kübler-Ross grief and dying stages experienced by terminal survivors and their caregivers.
    /// </summary>
    public enum GriefStage
    {
        Denial = 0,
        Anger = 1,
        Bargaining = 2,
        Depression = 3,
        Acceptance = 4
    }

    /// <summary>
    /// Palliative medical protocol balancing pain alleviation against cognitive lucidity.
    /// </summary>
    public enum PalliativeCareProtocol
    {
        MinimalSedation = 0,   // High lucidity, moderate pain relief
        BalancedAnalgesia = 1, // Balanced pain relief and awareness
        DeepSymptomControl = 2 // Complete pain relief, low lucidity
    }

    /// <summary>
    /// Record of a terminal survivor receiving palliative care in the shelter medical ward.
    /// </summary>
    public sealed class PalliativePatientRecord
    {
        public string SurvivorId { get; set; } = string.Empty;
        public int DaysRemainingPrognosis { get; set; } = 7;
        public int PainLevelPermille { get; set; } = 600; // 0..1000
        public int LucidityPermille { get; set; } = 800;  // 0..1000
        public int DignityIndexPermille { get; set; } = 750; // 0..1000
        public PalliativeCareProtocol ActiveProtocol { get; set; } = PalliativeCareProtocol.BalancedAnalgesia;
        public string FinalWishQuestId { get; set; } = string.Empty;
        public bool FinalWishFulfilled { get; set; }
        public GriefStage CurrentGriefStage { get; set; } = GriefStage.Denial;
        public int DaysInCurrentGriefStage { get; set; }

        public PalliativePatientRecord Clone() => new PalliativePatientRecord
        {
            SurvivorId = SurvivorId,
            DaysRemainingPrognosis = DaysRemainingPrognosis,
            PainLevelPermille = PainLevelPermille,
            LucidityPermille = LucidityPermille,
            DignityIndexPermille = DignityIndexPermille,
            ActiveProtocol = ActiveProtocol,
            FinalWishQuestId = FinalWishQuestId,
            FinalWishFulfilled = FinalWishFulfilled,
            CurrentGriefStage = CurrentGriefStage,
            DaysInCurrentGriefStage = DaysInCurrentGriefStage
        };
    }

    /// <summary>
    /// Daily palliative treatment outcome for a terminal patient.
    /// </summary>
    public readonly struct DailyPalliativeOutcome
    {
        public int PainDeltaPermille { get; }
        public int LucidityDeltaPermille { get; }
        public int DignityDeltaPermille { get; }
        public bool PrognosisExpired { get; }

        public DailyPalliativeOutcome(
            int painDeltaPermille,
            int lucidityDeltaPermille,
            int dignityDeltaPermille,
            bool prognosisExpired)
        {
            PainDeltaPermille = painDeltaPermille;
            LucidityDeltaPermille = lucidityDeltaPermille;
            DignityDeltaPermille = dignityDeltaPermille;
            PrognosisExpired = prognosisExpired;
        }
    }

    /// <summary>
    /// Morale echo left by a terminal survivor upon their passing.
    /// </summary>
    public readonly struct MemorialLegacyEcho
    {
        public string SurvivorId { get; }
        public int MoraleDelta { get; }
        public string MemorialJournalKey { get; }
        public bool DiedInDignity { get; }

        public MemorialLegacyEcho(
            string survivorId,
            int moraleDelta,
            string memorialJournalKey,
            bool diedInDignity)
        {
            SurvivorId = survivorId ?? string.Empty;
            MoraleDelta = moraleDelta;
            MemorialJournalKey = memorialJournalKey ?? string.Empty;
            DiedInDignity = diedInDignity;
        }
    }

    /// <summary>
    /// Pure domain engine governing terminal palliative care, symptom management,
    /// dignity scoring, and grief stage transitions.
    /// Zero engine references, integer permille determinism.
    /// </summary>
    public static class PalliativeCareDignityEngine
    {
        public const int HighDignityThresholdPermille = 700;

        /// <summary>
        /// Advances one day of palliative care, adjusting pain, lucidity, and dignity based on available supplies and protocols.
        /// </summary>
        public static DailyPalliativeOutcome AdvanceDailyCare(
            PalliativePatientRecord patient,
            int medicineAvailabilityPermille,
            int caregiverSkillPermille)
        {
            if (patient == null) throw new ArgumentNullException(nameof(patient));

            int med = Math.Clamp(medicineAvailabilityPermille, 0, 1000);
            int skill = Math.Clamp(caregiverSkillPermille, 0, 1000);

            // Protocol targets for pain suppression and lucidity penalty
            int basePainRelief = patient.ActiveProtocol switch
            {
                PalliativeCareProtocol.MinimalSedation => 200,
                PalliativeCareProtocol.BalancedAnalgesia => 450,
                PalliativeCareProtocol.DeepSymptomControl => 750,
                _ => 300
            };

            int lucidityPenalty = patient.ActiveProtocol switch
            {
                PalliativeCareProtocol.MinimalSedation => 50,
                PalliativeCareProtocol.BalancedAnalgesia => 150,
                PalliativeCareProtocol.DeepSymptomControl => 400,
                _ => 100
            };

            // Medicine availability scales actual relief
            int effectivePainRelief = (basePainRelief * med) / 1000;
            int painDelta = -effectivePainRelief;
            patient.PainLevelPermille = Math.Clamp(patient.PainLevelPermille + painDelta, 0, 1000);

            // Lucidity adjusted by protocol
            int lucidityDelta = -lucidityPenalty;
            patient.LucidityPermille = Math.Clamp(patient.LucidityPermille + lucidityDelta, 100, 1000);

            // Dignity improves when pain is controlled, lucidity is preserved, and caregiver skill is high
            int painControlScore = 1000 - patient.PainLevelPermille;
            long rawDignity = ((long)painControlScore * 500 + (long)patient.LucidityPermille * 300 + (long)skill * 200) / 1000;
            int dignityTarget = (int)rawDignity;

            int dignityDelta = (dignityTarget - patient.DignityIndexPermille) / 3;
            patient.DignityIndexPermille = Math.Clamp(patient.DignityIndexPermille + dignityDelta, 0, 1000);

            // Advance prognosis and days in current stage
            patient.DaysRemainingPrognosis = Math.Max(0, patient.DaysRemainingPrognosis - 1);
            patient.DaysInCurrentGriefStage++;

            bool expired = patient.DaysRemainingPrognosis == 0;
            return new DailyPalliativeOutcome(painDelta, lucidityDelta, dignityDelta, expired);
        }

        /// <summary>
        /// Evaluates progression between grief stages toward Acceptance.
        /// High dignity and fulfilled final wishes accelerate Acceptance; severe pain stalls the patient in Anger/Depression.
        /// </summary>
        public static void EvaluateGriefStageProgression(
            PalliativePatientRecord patient,
            long simTick,
            int worldSeed)
        {
            if (patient == null) throw new ArgumentNullException(nameof(patient));
            if (patient.CurrentGriefStage == GriefStage.Acceptance) return;

            // Minimum 2 days per stage
            if (patient.DaysInCurrentGriefStage < 2) return;

            // Transition likelihood enhanced by dignity and final wish
            int transitionScore = patient.DignityIndexPermille / 2;
            if (patient.FinalWishFulfilled)
            {
                transitionScore += 300;
            }

            // Severe pain imposes resistance to acceptance
            if (patient.PainLevelPermille > 600)
            {
                transitionScore -= 200;
            }

            transitionScore = Math.Clamp(transitionScore, 100, 900);

            int hash = StableHash.Combine(worldSeed, patient.SurvivorId);
            hash = StableHash.Combine(hash, simTick);
            hash = StableHash.Combine(hash, (int)patient.CurrentGriefStage);
            int roll = StableHash.NonNegativeRemainder(hash, 1000);

            if (roll < transitionScore)
            {
                // Advance to next grief stage
                patient.CurrentGriefStage = (GriefStage)((int)patient.CurrentGriefStage + 1);
                patient.DaysInCurrentGriefStage = 0;
            }
        }

        /// <summary>
        /// Calculates the permanent memorial legacy echo left across the shelter upon death.
        /// </summary>
        public static MemorialLegacyEcho CalculateMemorialEcho(PalliativePatientRecord deceased)
        {
            if (deceased == null) throw new ArgumentNullException(nameof(deceased));

            bool diedInDignity = deceased.DignityIndexPermille >= HighDignityThresholdPermille &&
                                 deceased.CurrentGriefStage == GriefStage.Acceptance;

            if (diedInDignity)
            {
                int buff = deceased.FinalWishFulfilled ? 15 : 10;
                return new MemorialLegacyEcho(
                    deceased.SurvivorId,
                    buff,
                    "memorial_died_in_peace_and_dignity",
                    true);
            }
            else if (deceased.PainLevelPermille > 700 || deceased.DignityIndexPermille < 300)
            {
                return new MemorialLegacyEcho(
                    deceased.SurvivorId,
                    -15,
                    "memorial_agonizing_neglect",
                    false);
            }
            else
            {
                return new MemorialLegacyEcho(
                    deceased.SurvivorId,
                    0,
                    "memorial_quiet_passing",
                    false);
            }
        }
    }
}
