// SPDX-License-Identifier: MIT
// Expansion 38 — The Ward : ClinicalWardTriageEngine focused tests
using Xunit;
using Ashfall.Core.Medical;

namespace Ashfall.Core.Tests.Medical
{
    public sealed class ClinicalWardTriageEngineTests
    {
        // ── 1. Critical trauma assigns Immediate priority and routes to isolation when contagious ──
        [Fact]
        public void EvaluatePatientTriage_AssignsImmediate_AndRoutesIsolationWhenContagious()
        {
            var ward = new ClinicalWardState
            {
                WardId = "ward-alpha",
                TotalBeds = 12,
                OccupiedBeds = 5,
                IsolationBedsTotal = 2,
                IsolationBedsOccupied = 0
            };

            var triage = ClinicalWardTriageEngine.EvaluatePatientTriage(
                traumaSeverityPermille: 750,
                vitalStabilityPermille: 350,
                isContagious: true,
                ward: ward);

            Assert.Equal(TriagePriorityTier.Immediate, triage.AssignedPriority);
            Assert.True(triage.RequiresIsolation);
            Assert.True(triage.BedAvailable);
            Assert.Contains("isolation", triage.RecommendationNotice);
        }

        // ── 2. Surgery denied when operating room lacks antiseptic cleanliness ──
        [Fact]
        public void EvaluateSurgicalPreparation_DeniesSurgery_WhenWardContaminated()
        {
            var ward = new ClinicalWardState
            {
                WardId = "ward-dirty",
                Cleanliness = WardCleanlinessGrade.Contaminated,
                SterileSupplyStockPermille = 800,
                StaffingReadinessPermille = 900
            };

            var prep = ClinicalWardTriageEngine.EvaluateSurgicalPreparation(
                ward: ward,
                procedureComplexityPermille: 500,
                patientConditionPermille: 700,
                surgerySeed: 999);

            Assert.False(prep.IsApprovedForSurgery);
            Assert.Contains("antiseptic standard", prep.BottleneckReason);
        }

        // ── 3. Surgery approved with adequate sterile supplies and staffing ──
        [Fact]
        public void EvaluateSurgicalPreparation_ApprovesAndConsumesSupplies_WhenSterileReady()
        {
            var ward = new ClinicalWardState
            {
                WardId = "ward-clean",
                Cleanliness = WardCleanlinessGrade.SterileField,
                SterileSupplyStockPermille = 700,
                StaffingReadinessPermille = 850
            };

            var prep = ClinicalWardTriageEngine.EvaluateSurgicalPreparation(
                ward: ward,
                procedureComplexityPermille: 600,
                patientConditionPermille: 600,
                surgerySeed: 777);

            Assert.True(prep.IsApprovedForSurgery);
            Assert.True(prep.SterileSuppliesConsumedPermille > 0);
            Assert.True(ward.SterileSupplyStockPermille < 700, "Sterile supplies should be deducted");
            Assert.True(prep.InfectionRiskPermille <= 100, "Infection risk in SterileField should be minimal");
        }

        // ── 4. Bed turnover calculation produces expected throughput ──
        [Fact]
        public void ComputeBedTurnoverCapacity_MatchesExpectedMonthlyTurns()
        {
            var ward = new ClinicalWardState
            {
                TotalBeds = 10
            };

            // 5 days average stay = 6 turns per bed per month * 10 beds = 60 patients/month
            int capacity = ClinicalWardTriageEngine.ComputeBedTurnoverCapacity(ward, averageLengthOfStayDays: 5);
            Assert.Equal(60, capacity);
        }

        // ── 5. Nosocomial infection risk increases with crowding and contamination ──
        [Fact]
        public void CalculateNosocomialInfectionRisk_RisesWithCrowdingAndContamination()
        {
            int lowRisk = ClinicalWardTriageEngine.CalculateNosocomialInfectionRisk(
                WardCleanlinessGrade.SterileField,
                wardOccupancyPermille: 300,
                sterileSupplyPermille: 900);

            int highRisk = ClinicalWardTriageEngine.CalculateNosocomialInfectionRisk(
                WardCleanlinessGrade.Contaminated,
                wardOccupancyPermille: 950,
                sterileSupplyPermille: 100);

            Assert.True(highRisk > lowRisk * 5,
                $"High crowding and contaminated ward ({highRisk}) should be dramatically riskier than sterile field ({lowRisk})");
        }
    }
}
