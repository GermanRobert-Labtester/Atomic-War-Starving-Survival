// SPDX-License-Identifier: MIT
// ASHFALL C1 Plan 19C-B — Session Continuity Journey & Anti-All-True Regression Gate (INV-19.1 - INV-19.4).

using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Endgame;
using Ashfall.Core.Memorial;
using Ashfall.Core.Verdict;
using Xunit;

namespace Ashfall.Core.Tests.Endgame
{
    public sealed class Plan19SessionContinuityJourneyTests
    {
        private readonly EpilogueMatrixRuntime _runtime = new EpilogueMatrixRuntime();

        [Fact]
        public void SessionContinuityJourney_SaveQuitReload_PreservesEndingProjectionAndBriefing()
        {
            // ── Phase 1: Live Campaign Setup ──
            int initialDay = 150;
            int livingSurvivors = 11;

            // Memorial authority
            var memorialState1 = new MemorialState();
            var memorial1 = new MemorialSystem(memorialState1);
            memorial1.Memorialize(new MemorialInput { SurvivorId = "dweller_lost_1", Cause = "lung_rot", Day = 40 });
            memorial1.Memorialize(new MemorialInput { SurvivorId = "dweller_lost_2", Cause = "ash_storm", Day = 80 });

            // Treaty authority
            var treatyState1 = new RegionalTreatyState
            {
                treaties = new List<TreatyInstance>
                {
                    new TreatyInstance
                    {
                        treatyId = "treaty_16_the_constitution_of_the_valley_of_tessarat",
                        status = TreatyStatus.Ratified
                    }
                }
            };

            // Verdict Reckoning authority (Tempest decommissioned)
            var reckoningState1 = new ReckoningState
            {
                phase = ReckoningPhase.Counted,
                countPresented = true,
                countHeld = false
            };

            // Debt authority (tampered/burned)
            bool ledgerTampered1 = true;
            var debts1 = new List<DebtContract>();

            // Evidence authority
            int evidenceEnrolled1 = 5;

            // Cohort authority (1 alive, 1 lost)
            var cohort1 = new CohortSystem();
            cohort1.BookChild("child_survivor", new List<string>(), "high", birthDay: 20);
            cohort1.BookChild("child_casualty", new List<string>(), "low", birthDay: 10);
            cohort1.MarkChildLost("child_casualty", day: 50, cause: "winter_chill");

            // ── Phase 2: Produce Briefing & Events ──
            var dayEvents = new List<DayStateChangeEvent>
            {
                new DayStateChangeEvent("consumed_child_rations", "rations", "Child Rations", "consumed", 2f),
                new DayStateChangeEvent("child_lost", "cohort", "child_casualty", "winter_chill"),
                new DayStateChangeEvent("treaty_signed", "treaty", "treaty_16", "ratified"),
                new DayStateChangeEvent("tempest_decommissioned", "reckoning", "Tempest", "decommissioned")
            };

            var briefingReport = DailyBriefingReportBuilder.BuildFromDayEvents(day: initialDay, buildSeed: 42, events: dayEvents);
            Assert.NotNull(briefingReport);
            Assert.False(briefingReport.IsEmpty);
            Assert.Contains(briefingReport.Sections, s => Array.Exists(s.Entries, e => e.Text.Contains("child_casualty")));

            // ── Phase 3: Evaluate Pre-Save Outcome ──
            var input1 = new CampaignOutcomeEvaluationInput
            {
                TotalDaysSurvived = initialDay,
                LivingDwellerCount = livingSurvivors,
                TotalDeathsRecorded = memorial1.Entries.Count,
                TreatiesState = treatyState1,
                VerdictReckoningState = reckoningState1,
                LedgerTampered = ledgerTampered1,
                Debts = debts1,
                EnrolledEvidenceCount = evidenceEnrolled1,
                CohortChildren = cohort1.Children
            };

            var snapshot1 = CampaignOutcomeEvaluator.Evaluate(input1);
            var context1 = EpilogueContextFactory.Build(snapshot1.ToInputs());
            string narrative1 = _runtime.GenerateEpilogueNarrative(context1);

            Assert.True(context1.grandTreatySigned);
            Assert.True(context1.tempestDecommissioned);
            Assert.True(context1.debtLedgersBurned);
            Assert.True(context1.childrenSurvived);
            Assert.True(context1.velSecretExposed);
            Assert.Equal(2, context1.totalDeathsRecorded);

            // ── Phase 4: Save & Reconstruct Session (Quit & Reload) ──
            var savedMemorial = memorial1.CaptureState();
            var savedCohort = cohort1.CaptureState();

            // Brand new system instances simulating session reload
            var memorial2 = new MemorialSystem(new MemorialState());
            memorial2.RestoreState(savedMemorial);

            var cohort2 = new CohortSystem();
            cohort2.RestoreState(savedCohort);

            var treatyState2 = new RegionalTreatyState
            {
                treaties = new List<TreatyInstance>
                {
                    new TreatyInstance
                    {
                        treatyId = "treaty_16_the_constitution_of_the_valley_of_tessarat",
                        status = TreatyStatus.Ratified
                    }
                }
            };

            var reckoningState2 = new ReckoningState
            {
                phase = reckoningState1.phase,
                countPresented = reckoningState1.countPresented,
                countHeld = reckoningState1.countHeld
            };

            // ── Phase 5: Post-Load Derivation & Assertion ──
            var input2 = new CampaignOutcomeEvaluationInput
            {
                TotalDaysSurvived = initialDay,
                LivingDwellerCount = livingSurvivors,
                TotalDeathsRecorded = memorial2.Entries.Count,
                TreatiesState = treatyState2,
                VerdictReckoningState = reckoningState2,
                LedgerTampered = ledgerTampered1,
                Debts = debts1,
                EnrolledEvidenceCount = evidenceEnrolled1,
                CohortChildren = cohort2.Children
            };

            var snapshot2 = CampaignOutcomeEvaluator.Evaluate(input2);
            var context2 = EpilogueContextFactory.Build(snapshot2.ToInputs());
            string narrative2 = _runtime.GenerateEpilogueNarrative(context2);

            // Context invariants: all 8 fields match pre-save
            Assert.Equal(context1.totalDaysSurvived, context2.totalDaysSurvived);
            Assert.Equal(context1.livingDwellerCount, context2.livingDwellerCount);
            Assert.Equal(context1.totalDeathsRecorded, context2.totalDeathsRecorded);
            Assert.Equal(context1.grandTreatySigned, context2.grandTreatySigned);
            Assert.Equal(context1.tempestDecommissioned, context2.tempestDecommissioned);
            Assert.Equal(context1.debtLedgersBurned, context2.debtLedgersBurned);
            Assert.Equal(context1.childrenSurvived, context2.childrenSurvived);
            Assert.Equal(context1.velSecretExposed, context2.velSecretExposed);

            // Narrative & outcomes are identical
            Assert.Equal(snapshot1.Fate, snapshot2.Fate);
            Assert.Equal(snapshot1.MoralStanding, snapshot2.MoralStanding);
            Assert.Equal(snapshot1.Demographics, snapshot2.Demographics);
            Assert.Equal(narrative1, narrative2);
        }

        [Fact]
        public void RegressionGuard_AllTrueConstants_DetectedAndRejected()
        {
            // A dark, uncompromised campaign where none of the "all-true" constants occurred:
            // - No Grand Treaty
            // - Tempest still active (secret held, dormant)
            // - Debts unpaid and ledger untampered
            // - No surviving children (all perished)
            // - 18 casualties recorded
            var cohort = new CohortSystem();
            cohort.BookChild("child_lost_1", new List<string>(), "low", birthDay: 5);
            cohort.MarkChildLost("child_lost_1", day: 25, cause: "cold");

            var input = new CampaignOutcomeEvaluationInput
            {
                TotalDaysSurvived = 160,
                LivingDwellerCount = 4,
                TotalDeathsRecorded = 18,
                TreatiesState = new RegionalTreatyState(),
                VerdictReckoningState = new ReckoningState { phase = ReckoningPhase.Knowing, countPresented = false },
                LedgerTampered = false,
                Debts = new List<DebtContract> { new DebtContract { signed = true, paid = false } },
                EnrolledEvidenceCount = 1,
                CohortChildren = cohort.Children
            };

            var snapshot = CampaignOutcomeEvaluator.Evaluate(input);
            var context = EpilogueContextFactory.Build(snapshot.ToInputs());

            // Prove the derived context is NOT the legacy all-true constant
            Assert.False(context.grandTreatySigned);
            Assert.False(context.tempestDecommissioned);
            Assert.False(context.debtLedgersBurned);
            Assert.False(context.childrenSurvived);
            Assert.False(context.velSecretExposed);
            Assert.Equal(18, context.totalDeathsRecorded);

            // Prove the matrix evaluates to harsh outcomes, not TrueReconciliation
            Assert.NotEqual(RegionalFate.TrueReconciliation, snapshot.Fate);
            Assert.Equal(RegionalFate.FracturedWarlords, snapshot.Fate);

            Assert.NotEqual(MoralStanding.ForgivenAndReconciled, snapshot.MoralStanding);
            Assert.Equal(MoralStanding.IndenturedDebtState, snapshot.MoralStanding);

            Assert.NotEqual(DemographicOutcome.ThrivingCommunity, snapshot.Demographics);
            Assert.Equal(DemographicOutcome.HardenedSurvivors, snapshot.Demographics);
        }
    }
}
