// SPDX-License-Identifier: MIT
// ASHFALL C1 Plan 19 — Ending Continuity Integration Tests (INV-19.1 through INV-19.4).

using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Endgame;
using Ashfall.Core.Flags;
using Ashfall.Core.Memorial;
using Ashfall.Core.Verdict;
using Xunit;

namespace Ashfall.Core.Tests.Endgame
{
    public sealed class Plan19EndingContinuityTests
    {
        private readonly EpilogueMatrixRuntime _runtime = new EpilogueMatrixRuntime();

        // ── 19A.1 Failing Proof / Differential Context ────────────────────────

        [Fact]
        public void CampaignA_Vs_CampaignB_YieldsDistinctContexts()
        {
            // Campaign A: low casualties, no treaty, active Tempest, intact debt, no kids, secret hidden
            var inputA = new CampaignOutcomeEvaluationInput
            {
                TotalDaysSurvived = 120,
                LivingDwellerCount = 12,
                TotalDeathsRecorded = 2,
                VerdictReckoningState = new ReckoningState { phase = ReckoningPhase.Knowing, countPresented = false },
                EnrolledEvidenceCount = 1,
                Debts = new List<DebtContract>
                {
                    new DebtContract { signed = true, paid = false, forgiven = false, principal = 500f }
                }
            };

            // Campaign B: high casualties, grand treaty, decommissioned Tempest, burned debt, children alive, secret exposed
            var inputB = new CampaignOutcomeEvaluationInput
            {
                TotalDaysSurvived = 200,
                LivingDwellerCount = 15,
                TotalDeathsRecorded = 8,
                TreatiesState = new RegionalTreatyState
                {
                    treaties = new List<TreatyInstance>
                    {
                        new TreatyInstance { treatyId = "treaty_16_the_constitution_of_the_valley_of_tessarat", status = TreatyStatus.Ratified }
                    }
                },
                VerdictReckoningState = new ReckoningState { phase = ReckoningPhase.Counted, countPresented = true },
                EnrolledEvidenceCount = 5,
                LedgerTampered = true,
                CohortChildren = new List<CohortChild>
                {
                    new CohortChild { survivorId = "child_01", guessBand = "low", birthDay = 30 }
                }
            };

            var snapshotA = CampaignOutcomeEvaluator.Evaluate(inputA);
            var snapshotB = CampaignOutcomeEvaluator.Evaluate(inputB);

            var ctxA = EpilogueContextFactory.Build(snapshotA.ToInputs());
            var ctxB = EpilogueContextFactory.Build(snapshotB.ToInputs());

            // Assert states are not identical
            Assert.NotEqual(ctxA.totalDaysSurvived, ctxB.totalDaysSurvived);
            Assert.NotEqual(ctxA.grandTreatySigned, ctxB.grandTreatySigned);
            Assert.NotEqual(ctxA.tempestDecommissioned, ctxB.tempestDecommissioned);
            Assert.NotEqual(ctxA.debtLedgersBurned, ctxB.debtLedgersBurned);
            Assert.NotEqual(ctxA.childrenSurvived, ctxB.childrenSurvived);
            Assert.NotEqual(ctxA.velSecretExposed, ctxB.velSecretExposed);

            // Assert evaluated outcomes differ
            Assert.NotEqual(snapshotA.Fate, snapshotB.Fate);
            Assert.NotEqual(snapshotA.MoralStanding, snapshotB.MoralStanding);
        }

        // ── 19A.2 EpilogueContextFactory Unit Tests ───────────────────────────

        [Fact]
        public void EpilogueContextFactory_Build_MapsValidInputsCleanly()
        {
            var trace = new List<string> { "source_1", "source_2" };
            var inputs = new EpilogueContextInputs(
                Days: 150,
                LivingDwellers: 10,
                DeathsRecorded: 3,
                GrandTreatySigned: true,
                TempestDecommissioned: true,
                DebtLedgersBurned: true,
                ChildrenSurvived: true,
                VelSecretExposed: true,
                SourceIds: trace
            );

            var ctx = EpilogueContextFactory.Build(inputs);

            Assert.NotNull(ctx);
            Assert.Equal(150, ctx.totalDaysSurvived);
            Assert.Equal(10, ctx.livingDwellerCount);
            Assert.Equal(3, ctx.totalDeathsRecorded);
            Assert.True(ctx.grandTreatySigned);
            Assert.True(ctx.tempestDecommissioned);
            Assert.True(ctx.debtLedgersBurned);
            Assert.True(ctx.childrenSurvived);
            Assert.True(ctx.velSecretExposed);
        }

        [Theory]
        [InlineData(-1, 5, 0)]
        [InlineData(10, -1, 0)]
        [InlineData(10, 5, -1)]
        public void EpilogueContextFactory_Build_RejectsNegativeCounts(int days, int living, int dead)
        {
            var inputs = new EpilogueContextInputs(
                Days: days,
                LivingDwellers: living,
                DeathsRecorded: dead,
                GrandTreatySigned: false,
                TempestDecommissioned: false,
                DebtLedgersBurned: false,
                ChildrenSurvived: false,
                VelSecretExposed: false
            );

            Assert.Throws<ArgumentOutOfRangeException>(() => EpilogueContextFactory.Build(inputs));
        }

        [Fact]
        public void EpilogueContextFactory_Build_NullInputs_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => EpilogueContextFactory.Build(null!));
        }

        // ── 19A.4 Deaths Recorded & Memorial Consistency ──────────────────────

        [Fact]
        public void DeathsRecorded_ZeroDeaths_MapsToZero()
        {
            var input = new CampaignOutcomeEvaluationInput { TotalDeathsRecorded = 0, LivingDwellerCount = 5 };
            var snapshot = CampaignOutcomeEvaluator.Evaluate(input);
            var ctx = EpilogueContextFactory.Build(snapshot.ToInputs());
            Assert.Equal(0, ctx.totalDeathsRecorded);
        }

        [Fact]
        public void DeathsRecorded_MatchesMemorialCount()
        {
            var memorialState = new MemorialState();
            var memorial = new MemorialSystem(memorialState);

            memorial.Memorialize(new MemorialInput { SurvivorId = "s1", Day = 10 });
            memorial.Memorialize(new MemorialInput { SurvivorId = "s2", Day = 25 });
            memorial.Memorialize(new MemorialInput { SurvivorId = "s3", Day = 40 });

            var input = new CampaignOutcomeEvaluationInput
            {
                TotalDeathsRecorded = memorial.Entries.Count,
                LivingDwellerCount = 10,
                TotalDaysSurvived = 50
            };

            var snapshot = CampaignOutcomeEvaluator.Evaluate(input);
            var ctx = EpilogueContextFactory.Build(snapshot.ToInputs());

            Assert.Equal(3, ctx.totalDeathsRecorded);
            Assert.Equal(memorial.Entries.Count, ctx.totalDeathsRecorded);
        }

        [Fact]
        public void DeathsRecorded_HeavyCasualties_ExceedsFiftyBranch()
        {
            var input = new CampaignOutcomeEvaluationInput
            {
                TotalDeathsRecorded = 55,
                LivingDwellerCount = 2,
                TotalDaysSurvived = 180,
                VerdictReckoningState = new ReckoningState { phase = ReckoningPhase.Knowing, countPresented = false }
            };

            var snapshot = CampaignOutcomeEvaluator.Evaluate(input);
            var ctx = EpilogueContextFactory.Build(snapshot.ToInputs());

            Assert.Equal(55, ctx.totalDeathsRecorded);
            Assert.Equal(RegionalFate.TempestSterilization, _runtime.EvaluateRegionalFate(ctx));
        }

        // ── 19A.5 Tempest Decommissioned Reuses VerdictEndingEvaluator ────────

        [Fact]
        public void TempestDecommissioned_ReusesVerdictEvaluator()
        {
            var satisfyingState = new ReckoningState { countPresented = true };
            Assert.True(VerdictEndingEvaluator.IsTempestDecommissioned(satisfyingState));

            var nonSatisfyingState = new ReckoningState { countHeld = true };
            Assert.False(VerdictEndingEvaluator.IsTempestDecommissioned(nonSatisfyingState));

            var inputSatisfying = new CampaignOutcomeEvaluationInput { VerdictReckoningState = satisfyingState };
            var snapshotA = CampaignOutcomeEvaluator.Evaluate(inputSatisfying);
            var ctxA = EpilogueContextFactory.Build(snapshotA.ToInputs());
            Assert.True(ctxA.tempestDecommissioned);

            var inputNonSatisfying = new CampaignOutcomeEvaluationInput { VerdictReckoningState = nonSatisfyingState };
            var snapshotB = CampaignOutcomeEvaluator.Evaluate(inputNonSatisfying);
            var ctxB = EpilogueContextFactory.Build(snapshotB.ToInputs());
            Assert.False(ctxB.tempestDecommissioned);
        }

        // ── 19A.6 Grand Treaty Derivation ─────────────────────────────────────

        [Fact]
        public void GrandTreaty_DerivesFromTreatyStateOrFlag()
        {
            // Ordinary regional treaty does not qualify as Grand Treaty
            var regionalOnly = new RegionalTreatyState
            {
                treaties = new List<TreatyInstance>
                {
                    new TreatyInstance { treatyId = "treaty_iron_pact", status = TreatyStatus.Ratified }
                }
            };
            var inputRegional = new CampaignOutcomeEvaluationInput { TreatiesState = regionalOnly };
            var snapRegional = CampaignOutcomeEvaluator.Evaluate(inputRegional);
            Assert.False(snapRegional.GrandTreatySigned);

            // Grand Constitution treaty qualifies
            var grandTreatyState = new RegionalTreatyState
            {
                treaties = new List<TreatyInstance>
                {
                    new TreatyInstance { treatyId = "treaty_16_the_constitution_of_the_valley_of_tessarat", status = TreatyStatus.Ratified }
                }
            };
            var inputGrand = new CampaignOutcomeEvaluationInput { TreatiesState = grandTreatyState };
            var snapGrand = CampaignOutcomeEvaluator.Evaluate(inputGrand);
            Assert.True(snapGrand.GrandTreatySigned);

            // Flag fallback qualifies
            var flags = new InMemoryFlagLedger();
            flags.Set("flag_grand_treaty_signed");
            var inputFlag = new CampaignOutcomeEvaluationInput { Flags = flags };
            var snapFlag = CampaignOutcomeEvaluator.Evaluate(inputFlag);
            Assert.True(snapFlag.GrandTreatySigned);
        }

        // ── 19A.7 Debt Ledgers Burned Derivation ──────────────────────────────

        [Fact]
        public void DebtLedgersBurned_DerivesFromTamperOrAllSettled()
        {
            // Unpaid signed debts -> false
            var inputUnpaid = new CampaignOutcomeEvaluationInput
            {
                Debts = new List<DebtContract>
                {
                    new DebtContract { signed = true, paid = false, forgiven = false }
                }
            };
            Assert.False(CampaignOutcomeEvaluator.Evaluate(inputUnpaid).DebtLedgersBurned);

            // Tampered ledger -> true
            var inputTampered = new CampaignOutcomeEvaluationInput { LedgerTampered = true };
            Assert.True(CampaignOutcomeEvaluator.Evaluate(inputTampered).DebtLedgersBurned);

            // All settled -> true
            var inputSettled = new CampaignOutcomeEvaluationInput
            {
                Debts = new List<DebtContract>
                {
                    new DebtContract { signed = true, paid = true },
                    new DebtContract { signed = true, forgiven = true }
                }
            };
            Assert.True(CampaignOutcomeEvaluator.Evaluate(inputSettled).DebtLedgersBurned);
        }

        // ── 19A.8 Children Survived Derivation ────────────────────────────────

        [Fact]
        public void ChildrenSurvived_DerivesFromCohort()
        {
            // No living dwellers -> false even if cohort records exist (shelter extinct)
            var inputExtinct = new CampaignOutcomeEvaluationInput
            {
                LivingDwellerCount = 0,
                CohortChildren = new List<CohortChild> { new CohortChild { survivorId = "c1" } }
            };
            Assert.False(CampaignOutcomeEvaluator.Evaluate(inputExtinct).ChildrenSurvived);

            // Living dwellers + cohort child -> true
            var inputAlive = new CampaignOutcomeEvaluationInput
            {
                LivingDwellerCount = 10,
                CohortChildren = new List<CohortChild> { new CohortChild { survivorId = "c1" } }
            };
            Assert.True(CampaignOutcomeEvaluator.Evaluate(inputAlive).ChildrenSurvived);

            // Living dwellers + zero children -> false
            var inputNoKids = new CampaignOutcomeEvaluationInput
            {
                LivingDwellerCount = 10,
                CohortChildren = new List<CohortChild>()
            };
            Assert.False(CampaignOutcomeEvaluator.Evaluate(inputNoKids).ChildrenSurvived);
        }

        // ── 19A.9 Vel Secret Exposed Derivation ───────────────────────────────

        [Fact]
        public void VelSecretExposed_DerivesFromReckoningOrEvidence()
        {
            // 4+ evidence documents enrolled -> true
            var inputEvidence = new CampaignOutcomeEvaluationInput { EnrolledEvidenceCount = 4 };
            Assert.True(CampaignOutcomeEvaluator.Evaluate(inputEvidence).VelSecretExposed);

            // Under threshold and dormant Reckoning -> false
            var inputLow = new CampaignOutcomeEvaluationInput
            {
                EnrolledEvidenceCount = 2,
                VerdictReckoningState = new ReckoningState { phase = ReckoningPhase.Dormant }
            };
            Assert.False(CampaignOutcomeEvaluator.Evaluate(inputLow).VelSecretExposed);

            // Reckoning count presented -> true
            var inputCounted = new CampaignOutcomeEvaluationInput
            {
                EnrolledEvidenceCount = 1,
                VerdictReckoningState = new ReckoningState { phase = ReckoningPhase.Counted, countPresented = true }
            };
            Assert.True(CampaignOutcomeEvaluator.Evaluate(inputCounted).VelSecretExposed);
        }

        // ── 19A.14 Full Matrix Branch Reachability Suite ──────────────────────

        [Theory]
        // RegionalFate outcomes
        [InlineData(true, true, true, 10, 10, 10, RegionalFate.TrueReconciliation, DemographicOutcome.ThrivingCommunity, MoralStanding.ForgivenAndReconciled)]
        [InlineData(true, false, true, 10, 10, 10, RegionalFate.CommonwealthFounded, DemographicOutcome.ThrivingCommunity, MoralStanding.ForgivenAndReconciled)]
        [InlineData(true, false, false, 10, 10, 10, RegionalFate.GarrisonMartialLaw, DemographicOutcome.ThrivingCommunity, MoralStanding.IndenturedDebtState)]
        [InlineData(false, false, false, 55, 5, 0, RegionalFate.TempestSterilization, DemographicOutcome.HardenedSurvivors, MoralStanding.IndenturedDebtState)]
        [InlineData(false, false, true, 10, 2, 0, RegionalFate.FracturedWarlords, DemographicOutcome.GhostShelter, MoralStanding.RuthlessPragmatists)]
        [InlineData(false, false, false, 20, 0, 0, RegionalFate.FracturedWarlords, DemographicOutcome.TotalExtinction, MoralStanding.IndenturedDebtState)]
        public void MatrixBranches_AllReachableFromCampaignInputs(
            bool treaty,
            bool decommissioned,
            bool burnedDebt,
            int deaths,
            int living,
            int childrenCount,
            RegionalFate expectedFate,
            DemographicOutcome expectedDemo,
            MoralStanding expectedMoral)
        {
            var cohortList = new List<CohortChild>();
            for (int i = 0; i < childrenCount; i++)
                cohortList.Add(new CohortChild { survivorId = $"child_{i}" });

            var input = new CampaignOutcomeEvaluationInput
            {
                TotalDaysSurvived = 200,
                LivingDwellerCount = living,
                TotalDeathsRecorded = deaths,
                TreatiesState = treaty
                    ? new RegionalTreatyState { treaties = new List<TreatyInstance> { new TreatyInstance { treatyId = "treaty_16_the_constitution_of_the_valley_of_tessarat", status = TreatyStatus.Ratified } } }
                    : new RegionalTreatyState(),
                VerdictReckoningState = decommissioned
                    ? new ReckoningState { countPresented = true }
                    : new ReckoningState { countHeld = true },
                LedgerTampered = burnedDebt,
                Debts = burnedDebt
                    ? new List<DebtContract>()
                    : new List<DebtContract> { new DebtContract { signed = true, paid = false } },
                CohortChildren = cohortList
            };

            var snapshot = CampaignOutcomeEvaluator.Evaluate(input);
            var ctx = EpilogueContextFactory.Build(snapshot.ToInputs());

            Assert.Equal(expectedFate, _runtime.EvaluateRegionalFate(ctx));
            Assert.Equal(expectedDemo, _runtime.EvaluateDemographics(ctx));
            Assert.Equal(expectedMoral, _runtime.EvaluateMoralStanding(ctx));

            string narrative = _runtime.GenerateEpilogueNarrative(ctx);
            Assert.False(string.IsNullOrEmpty(narrative));
            Assert.Contains("SAGA EPILOGUE", narrative);
        }

        // ── 19A.17 Deterministic Ending Replay ────────────────────────────────

        [Fact]
        public void Replay_IdenticalInputs_ProducesIdenticalOutputs()
        {
            var input = new CampaignOutcomeEvaluationInput
            {
                TotalDaysSurvived = 250,
                LivingDwellerCount = 14,
                TotalDeathsRecorded = 6,
                TreatiesState = new RegionalTreatyState
                {
                    treaties = new List<TreatyInstance>
                    {
                        new TreatyInstance { treatyId = "treaty_16_the_constitution_of_the_valley_of_tessarat", status = TreatyStatus.Ratified }
                    }
                },
                VerdictReckoningState = new ReckoningState { countPresented = true },
                LedgerTampered = true,
                CohortChildren = new List<CohortChild> { new CohortChild { survivorId = "c_replay" } }
            };

            var snap1 = CampaignOutcomeEvaluator.Evaluate(input);
            var snap2 = CampaignOutcomeEvaluator.Evaluate(input);

            var ctx1 = EpilogueContextFactory.Build(snap1.ToInputs());
            var ctx2 = EpilogueContextFactory.Build(snap2.ToInputs());

            Assert.Equal(_runtime.GenerateEpilogueNarrative(ctx1), _runtime.GenerateEpilogueNarrative(ctx2));
            Assert.Equal(snap1.Fate, snap2.Fate);
            Assert.Equal(snap1.Demographics, snap2.Demographics);
            Assert.Equal(snap1.MoralStanding, snap2.MoralStanding);
        }

        // ── 19A.18 Three-Policy 200-Day Proof ─────────────────────────────────

        [Fact]
        public void ThreePolicies_200DayCampaign_YieldsThreeDistinctEndings()
        {
            // Policy A: Reconciliation / Grand Treaty / Low Casualty
            var policyA = new CampaignOutcomeEvaluationInput
            {
                TotalDaysSurvived = 200,
                LivingDwellerCount = 18,
                TotalDeathsRecorded = 3,
                TreatiesState = new RegionalTreatyState
                {
                    treaties = new List<TreatyInstance> { new TreatyInstance { treatyId = "treaty_16_the_constitution_of_the_valley_of_tessarat", status = TreatyStatus.Ratified } }
                },
                VerdictReckoningState = new ReckoningState { countPresented = true },
                LedgerTampered = true,
                CohortChildren = new List<CohortChild> { new CohortChild { survivorId = "c_a" } }
            };

            // Policy B: Authoritarian Garrison / Martial Law / Heavy Debt Unresolved
            var policyB = new CampaignOutcomeEvaluationInput
            {
                TotalDaysSurvived = 200,
                LivingDwellerCount = 6,
                TotalDeathsRecorded = 12,
                TreatiesState = new RegionalTreatyState
                {
                    treaties = new List<TreatyInstance> { new TreatyInstance { treatyId = "treaty_16_the_constitution_of_the_valley_of_tessarat", status = TreatyStatus.Ratified } }
                },
                VerdictReckoningState = new ReckoningState { countHeld = true },
                LedgerTampered = false,
                Debts = new List<DebtContract> { new DebtContract { signed = true, paid = false } },
                CohortChildren = new List<CohortChild>()
            };

            // Policy C: Resistance / Catastrophic Casualties / Tempest Sterilization
            var policyC = new CampaignOutcomeEvaluationInput
            {
                TotalDaysSurvived = 200,
                LivingDwellerCount = 1,
                TotalDeathsRecorded = 52,
                TreatiesState = new RegionalTreatyState(),
                VerdictReckoningState = new ReckoningState { phase = ReckoningPhase.Knowing, countPresented = false },
                LedgerTampered = false,
                Debts = new List<DebtContract> { new DebtContract { signed = true, paid = false } },
                CohortChildren = new List<CohortChild>()
            };

            var snapA = CampaignOutcomeEvaluator.Evaluate(policyA);
            var snapB = CampaignOutcomeEvaluator.Evaluate(policyB);
            var snapC = CampaignOutcomeEvaluator.Evaluate(policyC);

            Assert.Equal(RegionalFate.TrueReconciliation, snapA.Fate);
            Assert.Equal(RegionalFate.GarrisonMartialLaw, snapB.Fate);
            Assert.Equal(RegionalFate.TempestSterilization, snapC.Fate);

            Assert.NotEqual(snapA.Fate, snapB.Fate);
            Assert.NotEqual(snapB.Fate, snapC.Fate);
            Assert.NotEqual(snapA.Fate, snapC.Fate);

            Assert.Equal(DemographicOutcome.ThrivingCommunity, snapA.Demographics);
            Assert.Equal(DemographicOutcome.HardenedSurvivors, snapB.Demographics);
            Assert.Equal(DemographicOutcome.GhostShelter, snapC.Demographics);

            Assert.Equal(MoralStanding.ForgivenAndReconciled, snapA.MoralStanding);
            Assert.Equal(MoralStanding.IndenturedDebtState, snapB.MoralStanding);
            Assert.Equal(MoralStanding.IndenturedDebtState, snapC.MoralStanding);

            string textA = _runtime.GenerateEpilogueNarrative(snapA.ToContext());
            string textB = _runtime.GenerateEpilogueNarrative(snapB.ToContext());
            string textC = _runtime.GenerateEpilogueNarrative(snapC.ToContext());

            Assert.NotEqual(textA, textB);
            Assert.NotEqual(textB, textC);
            Assert.NotEqual(textA, textC);
        }
    }
}
