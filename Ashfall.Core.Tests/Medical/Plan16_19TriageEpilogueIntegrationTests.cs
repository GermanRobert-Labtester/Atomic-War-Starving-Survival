// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.Endgame;
using Ashfall.Core.Medical;
using Xunit;

namespace Ashfall.Core.Tests.Medical
{
    public sealed class Plan16_19TriageEpilogueIntegrationTests
    {
        [Fact]
        public void ClinicalTriage_AssignsAccuratePriorities_AndChecksCapacity()
        {
            var ward = new ClinicalWardState
            {
                WardId = "ward_alpha",
                TotalBeds = 5,
                OccupiedBeds = 4,
                IsolationBedsTotal = 2,
                IsolationBedsOccupied = 1,
                Cleanliness = WardCleanlinessGrade.AntisepticStandard,
                SterileSupplyStockPermille = 850,
                StaffingReadinessPermille = 900
            };

            // Case 1: Lethal trauma with low vital stability -> Expectant or Immediate
            var severe = ClinicalWardTriageEngine.EvaluatePatientTriage(
                traumaSeverityPermille: 950,
                vitalStabilityPermille: 100,
                isContagious: false,
                ward: ward);

            Assert.True(severe.AssignedPriority == TriagePriorityTier.Expectant || severe.AssignedPriority == TriagePriorityTier.Immediate);
            Assert.True(severe.BedAvailable);
            Assert.False(severe.RequiresIsolation);

            // Case 2: Immediate life threat (high trauma, moderate vitals)
            var immediate = ClinicalWardTriageEngine.EvaluatePatientTriage(
                traumaSeverityPermille: 750,
                vitalStabilityPermille: 350,
                isContagious: false,
                ward: ward);

            Assert.Equal(TriagePriorityTier.Immediate, immediate.AssignedPriority);
            Assert.True(immediate.EstimatedUrgencyMinutes <= 30);

            // Case 3: Contagious patient requires isolation
            var contagious = ClinicalWardTriageEngine.EvaluatePatientTriage(
                traumaSeverityPermille: 200,
                vitalStabilityPermille: 800,
                isContagious: true,
                ward: ward);

            Assert.True(contagious.RequiresIsolation);
            Assert.True(contagious.BedAvailable); // 1 of 2 isolation beds occupied
            Assert.Equal(TriagePriorityTier.Minimal, contagious.AssignedPriority);
        }

        [Fact]
        public void SurgicalPreparation_EvaluatesCleanliness_AndConsumesSterileSupplies()
        {
            var cleanWard = new ClinicalWardState
            {
                TotalBeds = 10,
                OccupiedBeds = 2,
                Cleanliness = WardCleanlinessGrade.SterileField,
                SterileSupplyStockPermille = 750,
                StaffingReadinessPermille = 800
            };

            var prep = ClinicalWardTriageEngine.EvaluateSurgicalPreparation(
                ward: cleanWard,
                procedureComplexityPermille: 500,
                patientConditionPermille: 600,
                surgerySeed: 42);

            Assert.True(prep.IsApprovedForSurgery);
            Assert.True(prep.SterileSuppliesConsumedPermille > 0);
            Assert.True(cleanWard.SterileSupplyStockPermille < 750);
            Assert.True(prep.InfectionRiskPermille <= 150);

            // Contaminated ward denies surgery
            var dirtyWard = new ClinicalWardState
            {
                Cleanliness = WardCleanlinessGrade.Contaminated,
                SterileSupplyStockPermille = 800,
                StaffingReadinessPermille = 900
            };

            var deniedPrep = ClinicalWardTriageEngine.EvaluateSurgicalPreparation(
                ward: dirtyWard,
                procedureComplexityPermille: 500,
                patientConditionPermille: 600,
                surgerySeed: 42);

            Assert.False(deniedPrep.IsApprovedForSurgery);
            Assert.Contains("antiseptic standard", deniedPrep.BottleneckReason);
        }

        [Fact]
        public void EpilogueContextFactory_BuildsTruthfulContext_FromCampaignInputs()
        {
            var prosperousInputs = new EpilogueContextInputs(
                Days: 365,
                LivingDwellers: 12,
                DeathsRecorded: 2,
                GrandTreatySigned: true,
                TempestDecommissioned: true,
                DebtLedgersBurned: true,
                ChildrenSurvived: true,
                VelSecretExposed: true);

            var context = EpilogueContextFactory.Build(prosperousInputs);

            Assert.Equal(365, context.totalDaysSurvived);
            Assert.Equal(12, context.livingDwellerCount);
            Assert.Equal(2, context.totalDeathsRecorded);
            Assert.True(context.grandTreatySigned);
            Assert.True(context.tempestDecommissioned);
            Assert.True(context.debtLedgersBurned);
            Assert.True(context.childrenSurvived);
            Assert.True(context.velSecretExposed);

            // Matrix evaluation on prosperous context
            var runtime = new EpilogueMatrixRuntime();
            Assert.Equal(RegionalFate.TrueReconciliation, runtime.EvaluateRegionalFate(context));
            Assert.Equal(DemographicOutcome.ThrivingCommunity, runtime.EvaluateDemographics(context));
            Assert.Equal(MoralStanding.ForgivenAndReconciled, runtime.EvaluateMoralStanding(context));
        }

        [Fact]
        public void EpilogueMatrixRuntime_BranchesDeterministically_OnCampaignDivergence()
        {
            var runtime = new EpilogueMatrixRuntime();

            // Tragic/Hardened scenario: no treaty, severe casualties
            var tragicInputs = new EpilogueContextInputs(
                Days: 180,
                LivingDwellers: 4,
                DeathsRecorded: 55,
                GrandTreatySigned: false,
                TempestDecommissioned: false,
                DebtLedgersBurned: false,
                ChildrenSurvived: false,
                VelSecretExposed: false);

            var tragicContext = EpilogueContextFactory.Build(tragicInputs);

            Assert.Equal(RegionalFate.TempestSterilization, runtime.EvaluateRegionalFate(tragicContext));
            Assert.Equal(DemographicOutcome.HardenedSurvivors, runtime.EvaluateDemographics(tragicContext));
            Assert.Equal(MoralStanding.IndenturedDebtState, runtime.EvaluateMoralStanding(tragicContext));

            // Extinction scenario: 0 living dwellers
            var extinctInputs = new EpilogueContextInputs(
                Days: 60,
                LivingDwellers: 0,
                DeathsRecorded: 20,
                GrandTreatySigned: false,
                TempestDecommissioned: false,
                DebtLedgersBurned: false,
                ChildrenSurvived: false,
                VelSecretExposed: false);

            var extinctContext = EpilogueContextFactory.Build(extinctInputs);
            Assert.Equal(DemographicOutcome.TotalExtinction, runtime.EvaluateDemographics(extinctContext));
        }
    }
}
