// SPDX-License-Identifier: MIT
using Ashfall.Core.Medical;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Medical
{
    public sealed class AfflictionDutyBridgeTests
    {
        private static FitnessForDutyModel CreateModel() => new FitnessForDutyModel(
            new FitnessThresholds(
                fatigueImpaired: 70f,
                fatigueUnfit: 90f,
                healthImpaired: 50f,
                healthUnfit: 20f,
                hungerImpaired: 70f,
                hungerUnfit: 90f,
                thirstImpaired: 70f,
                thirstUnfit: 90f,
                warmthImpaired: 40f,
                warmthUnfit: 20f,
                daysWithoutSleepImpaired: 2,
                daysWithoutSleepUnfit: 4,
                doseImpairedMsv: 200f,
                doseUnfitMsv: 500f));

        private static FitnessEvaluationFacts HealthyFacts() => new FitnessEvaluationFacts
        {
            SurvivorId = "survivor_1",
            IsAlive = true,
            Health = 100f,
            Warmth = 100f
        };

        private static RoleRequirements Role(string hazardClass, bool precision = false) =>
            new RoleRequirements(
                roleId: "role_test",
                skillId: string.Empty,
                minimumSkill: 0f,
                maximumFatigue: 100f,
                minimumHealth: 0f,
                maximumDoseMsv: 0f,
                maximumHours: 12f,
                maximumHoursIfImpaired: 8f,
                allowUnfit: false,
                lightDuty: false,
                requiresNotQuarantined: false,
                precisionWork: precision,
                hazardClass: hazardClass);

        [Fact]
        public void EvaluateForRole_InfectiousFoodHazard_HardBlocksAssignment()
        {
            var facts = HealthyFacts();
            facts.IsInfectious = true;

            var verdict = CreateModel().EvaluateForRole(facts, Role(DutyHazardClassIds.Food));

            Assert.False(verdict.Allowed);
            Assert.Contains(AfflictionDutyBridge.ReasonInfectiousFoodHazard, verdict.BlockingReasons);
            Assert.Equal(0f, verdict.RecommendedMaxHours);
        }

        [Fact]
        public void EvaluateForRole_InfectiousMedicalHazard_HardBlocksAssignment()
        {
            var facts = HealthyFacts();
            facts.IsInfectious = true;

            var verdict = CreateModel().EvaluateForRole(facts, Role(DutyHazardClassIds.Medical));

            Assert.False(verdict.Allowed);
            Assert.Contains(AfflictionDutyBridge.ReasonMedicalContagionRisk, verdict.BlockingReasons);
        }

        [Fact]
        public void EvaluateForRole_CombatTraumaPerimeter_AddsTypedWarning()
        {
            var facts = HealthyFacts();
            facts.HasCombatTrauma = true;

            var verdict = CreateModel().EvaluateForRole(facts, Role(DutyHazardClassIds.Perimeter));

            Assert.True(verdict.Allowed);
            Assert.True(verdict.RequiresConfirmation);
            Assert.Contains(AfflictionDutyBridge.ReasonTraumaPerimeterHazard, verdict.WarningReasons);
            Assert.Equal(6f, verdict.RecommendedMaxHours);
        }

        [Fact]
        public void EvaluateForRole_RespiratoryImpairment_OnlyRestrictsOutdoorHazards()
        {
            var facts = HealthyFacts();
            facts.HasRespiratoryImpairment = true;

            var surface = CreateModel().EvaluateForRole(facts, Role(DutyHazardClassIds.Surface));
            var food = CreateModel().EvaluateForRole(facts, Role(DutyHazardClassIds.Food));

            Assert.Contains(AfflictionDutyBridge.ReasonRespiratoryOutdoorHazard, surface.WarningReasons);
            Assert.DoesNotContain(AfflictionDutyBridge.ReasonRespiratoryOutdoorHazard, food.WarningReasons);
        }

        [Fact]
        public void EvaluateForRole_WithdrawalOnlyAddsDutyWarningForPrecisionWork()
        {
            var facts = HealthyFacts();
            facts.HasActiveWithdrawal = true;

            var precision = CreateModel().EvaluateForRole(facts, Role(DutyHazardClassIds.Intake, precision: true));
            var routine = CreateModel().EvaluateForRole(facts, Role(DutyHazardClassIds.Intake));

            Assert.Contains(AfflictionDutyBridge.ReasonDependencyImpairment, precision.WarningReasons);
            Assert.DoesNotContain(AfflictionDutyBridge.ReasonDependencyImpairment, routine.WarningReasons);
        }

        [Fact]
        public void EvaluateForRole_BaseIncapacity_RemainsBlocked()
        {
            var facts = HealthyFacts();
            facts.IsAlive = false;
            facts.IsDead = true;

            var verdict = CreateModel().EvaluateForRole(facts, Role(DutyHazardClassIds.Food));

            Assert.False(verdict.Allowed);
            Assert.Contains(FitnessReasonIds.Dead, verdict.BlockingReasons);
        }
    }
}
