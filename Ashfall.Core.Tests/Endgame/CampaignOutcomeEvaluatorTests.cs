// SPDX-License-Identifier: MIT
// Unit tests for Ashfall.Core.Endgame.CampaignOutcomeEvaluator (FX-01 / Plan 19).

using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Endgame;
using Ashfall.Core.Flags;
using Ashfall.Core.Legacy;
using Ashfall.Core.Verdict;
using Xunit;

namespace Ashfall.Core.Tests.Endgame
{
    public class CampaignOutcomeEvaluatorTests
    {
        [Fact]
        public void TotalExtinction_WhenLivingCountZero_DemographicsTotalExtinction()
        {
            var input = new CampaignOutcomeEvaluationInput
            {
                TotalDaysSurvived = 120,
                LivingDwellerCount = 0,
                TotalDeathsRecorded = 14,
                GrandTreatySignedOverride = false,
                TempestDecommissionedOverride = false,
                DebtLedgersBurnedOverride = false,
                ChildrenCount = 2 // Children cannot survive alone in an empty shelter
            };

            var snapshot = CampaignOutcomeEvaluator.Evaluate(input);

            Assert.Equal(DemographicOutcome.TotalExtinction, snapshot.Demographics);
            Assert.False(snapshot.ChildrenSurvived);
            Assert.Contains(snapshot.OutcomeTrace, t => t.Contains("total dweller extinction"));
        }

        [Fact]
        public void TrueReconciliation_WhenTreatyTempestDebtsBurned_EvaluatesTrueReconciliation()
        {
            var treatyState = new RegionalTreatyState
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

            var reckoning = new ReckoningState
            {
                phase = ReckoningPhase.Counted,
                countPresented = true
            };

            var cohort = new List<CohortChild>
            {
                new CohortChild { survivorId = "child_sonya", birthDay = 50 }
            };

            var input = new CampaignOutcomeEvaluationInput
            {
                TotalDaysSurvived = 365,
                LivingDwellerCount = 10,
                TotalDeathsRecorded = 4,
                TreatiesState = treatyState,
                VerdictReckoningState = reckoning,
                LedgerTampered = true,
                CohortChildren = cohort
            };

            var snapshot = CampaignOutcomeEvaluator.Evaluate(input);

            Assert.True(snapshot.GrandTreatySigned);
            Assert.True(snapshot.TempestDecommissioned);
            Assert.True(snapshot.DebtLedgersBurned);
            Assert.True(snapshot.ChildrenSurvived);
            Assert.Equal(RegionalFate.TrueReconciliation, snapshot.Fate);
            Assert.Equal(DemographicOutcome.ThrivingCommunity, snapshot.Demographics);
            Assert.Equal(MoralStanding.ForgivenAndReconciled, snapshot.MoralStanding);
            Assert.Contains("The treaty was ratified without an execution clause", snapshot.NarrativeProse);
        }

        [Fact]
        public void GrandTreatySigned_WhenOnlyOrdinaryRegionalTreatyRatified_IsFalse()
        {
            var treatyState = new RegionalTreatyState
            {
                treaties = new List<TreatyInstance>
                {
                    new TreatyInstance
                    {
                        treatyId = "treaty_04_water_rights_compact",
                        status = TreatyStatus.Ratified
                    }
                }
            };

            var input = new CampaignOutcomeEvaluationInput
            {
                TotalDaysSurvived = 200,
                LivingDwellerCount = 6,
                TotalDeathsRecorded = 3,
                TreatiesState = treatyState,
                TempestDecommissionedOverride = true,
                DebtLedgersBurnedOverride = true,
                ChildrenSurvivedOverride = true
            };

            var snapshot = CampaignOutcomeEvaluator.Evaluate(input);

            Assert.False(snapshot.GrandTreatySigned);
            Assert.Equal(1, snapshot.RatifiedTreatiesCount);
            Assert.Contains(snapshot.OutcomeTrace, t => t.Contains("none is a grand/constitution treaty"));
            Assert.NotEqual(RegionalFate.TrueReconciliation, snapshot.Fate);
        }

        [Fact]
        public void GrandTreatySigned_WhenOnlyGenericTreatyFlag_IsFalse()
        {
            var flags = new CampaignConsequenceLedger();
            flags.Set("flag_treaty_ratified");
            flags.Set("flag_peace_treaty_ratified");

            var input = new CampaignOutcomeEvaluationInput
            {
                TotalDaysSurvived = 200,
                LivingDwellerCount = 6,
                TotalDeathsRecorded = 3,
                Flags = flags,
                TempestDecommissionedOverride = true,
                DebtLedgersBurnedOverride = true,
                ChildrenSurvivedOverride = true
            };

            var snapshot = CampaignOutcomeEvaluator.Evaluate(input);

            Assert.False(snapshot.GrandTreatySigned);
            Assert.NotEqual(RegionalFate.TrueReconciliation, snapshot.Fate);
        }

        [Fact]
        public void GarrisonMartialLaw_WhenTreatySigned_AndDebtsRemain_EvaluatesGarrisonMartialLaw()
        {
            var flags = new CampaignConsequenceLedger();
            flags.Set("flag_grand_treaty_signed");

            var debts = new List<DebtContract>
            {
                new DebtContract { debtorId = "dweller_1", signed = true, paid = false, principal = 200f }
            };

            var input = new CampaignOutcomeEvaluationInput
            {
                TotalDaysSurvived = 200,
                LivingDwellerCount = 5,
                TotalDeathsRecorded = 8,
                Flags = flags,
                Debts = debts,
                LedgerTampered = false,
                TempestDecommissionedOverride = true
            };

            var snapshot = CampaignOutcomeEvaluator.Evaluate(input);

            Assert.True(snapshot.GrandTreatySigned);
            Assert.False(snapshot.DebtLedgersBurned);
            Assert.Equal(RegionalFate.GarrisonMartialLaw, snapshot.Fate);
            Assert.Equal(DemographicOutcome.HardenedSurvivors, snapshot.Demographics);
            Assert.Equal(MoralStanding.IndenturedDebtState, snapshot.MoralStanding);
        }

        [Fact]
        public void TempestSterilization_WhenOverFiftyDeaths_AndTempestActive_EvaluatesTempestSterilization()
        {
            var reckoning = new ReckoningState
            {
                phase = ReckoningPhase.Knowing,
                countPresented = false,
                countHeld = true
            };

            var input = new CampaignOutcomeEvaluationInput
            {
                TotalDaysSurvived = 400,
                LivingDwellerCount = 1,
                TotalDeathsRecorded = 55,
                GrandTreatySignedOverride = false,
                VerdictReckoningState = reckoning
            };

            var snapshot = CampaignOutcomeEvaluator.Evaluate(input);

            Assert.False(snapshot.GrandTreatySigned);
            Assert.False(snapshot.TempestDecommissioned);
            Assert.True(snapshot.TotalDeathsRecorded > 50);
            Assert.Equal(RegionalFate.TempestSterilization, snapshot.Fate);
            Assert.Equal(DemographicOutcome.GhostShelter, snapshot.Demographics);
        }

        [Fact]
        public void FracturedWarlords_WhenNoTreaty_AndRuthlessPragmatists()
        {
            var debts = new List<DebtContract>
            {
                new DebtContract { debtorId = "dweller_1", signed = true, paid = true, principal = 100f }
            };

            var input = new CampaignOutcomeEvaluationInput
            {
                TotalDaysSurvived = 180,
                LivingDwellerCount = 4,
                TotalDeathsRecorded = 12,
                GrandTreatySignedOverride = false,
                TempestDecommissionedOverride = true,
                Debts = debts,
                ChildrenCount = 0
            };

            var snapshot = CampaignOutcomeEvaluator.Evaluate(input);

            Assert.False(snapshot.GrandTreatySigned);
            Assert.True(snapshot.DebtLedgersBurned);
            Assert.False(snapshot.ChildrenSurvived);
            Assert.Equal(RegionalFate.FracturedWarlords, snapshot.Fate);
            Assert.Equal(DemographicOutcome.HardenedSurvivors, snapshot.Demographics);
            Assert.Equal(MoralStanding.RuthlessPragmatists, snapshot.MoralStanding);
        }

        [Fact]
        public void ConditionSensitivity_ChangingSingleAuthority_MutatesOutcome()
        {
            var input = new CampaignOutcomeEvaluationInput
            {
                TotalDaysSurvived = 250,
                LivingDwellerCount = 8,
                TotalDeathsRecorded = 10,
                GrandTreatySignedOverride = true,
                TempestDecommissionedOverride = true,
                LedgerTampered = false,
                ChildrenSurvivedOverride = true
            };

            // Initially, debt ledgers not burned -> Garrison Martial Law
            var initial = CampaignOutcomeEvaluator.Evaluate(input);
            Assert.Equal(RegionalFate.GarrisonMartialLaw, initial.Fate);
            Assert.Equal(MoralStanding.IndenturedDebtState, initial.MoralStanding);

            // Mutate single condition: tamper / burn debt ledger
            input.LedgerTampered = true;
            var mutated = CampaignOutcomeEvaluator.Evaluate(input);

            Assert.Equal(RegionalFate.TrueReconciliation, mutated.Fate);
            Assert.Equal(MoralStanding.ForgivenAndReconciled, mutated.MoralStanding);
        }

        [Fact]
        public void OutcomeTrace_ContainsDetailedForensicProvenance()
        {
            var flags = new CampaignConsequenceLedger();
            flags.Set("flag_grand_treaty_signed");
            flags.Set("flag_tempest_decommissioned");

            var input = new CampaignOutcomeEvaluationInput
            {
                TotalDaysSurvived = 300,
                LivingDwellerCount = 6,
                TotalDeathsRecorded = 2,
                Flags = flags,
                LedgerTampered = true,
                ChildrenSurvivedOverride = false
            };

            var snapshot = CampaignOutcomeEvaluator.Evaluate(input);

            Assert.NotEmpty(snapshot.OutcomeTrace);
            Assert.Contains(snapshot.OutcomeTrace, t => t.StartsWith("[Census]"));
            Assert.Contains(snapshot.OutcomeTrace, t => t.StartsWith("[Treaty]"));
            Assert.Contains(snapshot.OutcomeTrace, t => t.StartsWith("[Tempest]"));
            Assert.Contains(snapshot.OutcomeTrace, t => t.StartsWith("[Debt]"));
            Assert.Contains(snapshot.OutcomeTrace, t => t.StartsWith("[Children]"));
            Assert.Contains(snapshot.OutcomeTrace, t => t.StartsWith("[Secret]"));
            Assert.Contains(snapshot.OutcomeTrace, t => t.StartsWith("[Resolution]"));
        }

        [Fact]
        public void Determinism_IdenticalInputs_ProduceIdenticalOutputs()
        {
            var input1 = new CampaignOutcomeEvaluationInput
            {
                TotalDaysSurvived = 150,
                LivingDwellerCount = 5,
                TotalDeathsRecorded = 3,
                GrandTreatySignedOverride = true,
                TempestDecommissionedOverride = false,
                DebtLedgersBurnedOverride = true,
                ChildrenSurvivedOverride = false
            };

            var input2 = new CampaignOutcomeEvaluationInput
            {
                TotalDaysSurvived = 150,
                LivingDwellerCount = 5,
                TotalDeathsRecorded = 3,
                GrandTreatySignedOverride = true,
                TempestDecommissionedOverride = false,
                DebtLedgersBurnedOverride = true,
                ChildrenSurvivedOverride = false
            };

            var s1 = CampaignOutcomeEvaluator.Evaluate(input1);
            var s2 = CampaignOutcomeEvaluator.Evaluate(input2);

            Assert.Equal(s1.Fate, s2.Fate);
            Assert.Equal(s1.Demographics, s2.Demographics);
            Assert.Equal(s1.MoralStanding, s2.MoralStanding);
            Assert.Equal(s1.NarrativeProse, s2.NarrativeProse);
            Assert.Equal(s1.OutcomeTrace.Count, s2.OutcomeTrace.Count);
            for (int i = 0; i < s1.OutcomeTrace.Count; i++)
            {
                Assert.Equal(s1.OutcomeTrace[i], s2.OutcomeTrace[i]);
            }
        }

        [Fact]
        public void ProductionSourceCode_DoesNotContainLiteralEpilogueArguments()
        {
            string baseDir = AppContext.BaseDirectory;
            string rootDir = baseDir;
            while (!string.IsNullOrEmpty(rootDir) && !File.Exists(Path.Combine(rootDir, "project.godot")))
            {
                var parent = Directory.GetParent(rootDir);
                if (parent == null) break;
                rootDir = parent.FullName;
            }

            if (File.Exists(Path.Combine(rootDir, "src/Main.GameFlow.cs")))
            {
                string gameFlow = File.ReadAllText(Path.Combine(rootDir, "src/Main.GameFlow.cs"));
                Assert.DoesNotContain("0, true, true, true, true, true", gameFlow);
            }

            if (File.Exists(Path.Combine(rootDir, "src/Main.PlayerSurfaces.cs")))
            {
                string playerSurfaces = File.ReadAllText(Path.Combine(rootDir, "src/Main.PlayerSurfaces.cs"));
                Assert.DoesNotContain("0, true, true, true, true, true", playerSurfaces);
            }
        }
    }
}
