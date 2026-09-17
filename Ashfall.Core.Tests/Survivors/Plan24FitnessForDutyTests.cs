// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Survivors
{
    public sealed class Plan24FitnessForDutyTests
    {
        private static FitnessThresholds Thresholds() => new FitnessThresholds(
            fatigueImpaired: 60f, fatigueUnfit: 85f,
            healthImpaired: 60f, healthUnfit: 30f,
            hungerImpaired: 60f, hungerUnfit: 85f,
            thirstImpaired: 60f, thirstUnfit: 85f,
            warmthImpaired: 40f, warmthUnfit: 20f,
            daysWithoutSleepImpaired: 2, daysWithoutSleepUnfit: 4,
            doseImpairedMsv: 100f, doseUnfitMsv: 300f);

        private static RoleRequirements Role(
            string roleId = "night_watch",
            float minimumSkill = 0.1f,
            float minimumHealth = 30f,
            bool allowUnfit = false) => new RoleRequirements(
                roleId, "skill_watchful", minimumSkill,
                maximumFatigue: 90f, minimumHealth,
                maximumDoseMsv: 300f, maximumHours: 12f,
                maximumHoursIfImpaired: 8f, allowUnfit,
                lightDuty: false, requiresNotQuarantined: true,
                precisionWork: true, hazardClass: "perimeter");

        [Fact]
        public void HealthySurvivorIsFitAndRoleAssignable()
        {
            var model = new FitnessForDutyModel(Thresholds());
            var facts = new FitnessEvaluationFacts
            {
                SurvivorId = "survivor_a",
                SkillLevels = new Dictionary<string, float> { ["skill_watchful"] = 0.5f }
            };

            var verdict = model.EvaluateForRole(facts, Role());

            Assert.Equal(FitnessLevel.Fit, verdict.BaseVerdict.Level);
            Assert.True(verdict.Allowed);
            Assert.False(verdict.Warning);
            Assert.Empty(verdict.BlockingReasons);
        }

        [Fact]
        public void ImpairedSurvivorIsAllowedWithWarningAndReducedHours()
        {
            var model = new FitnessForDutyModel(Thresholds());
            var facts = new FitnessEvaluationFacts
            {
                SurvivorId = "survivor_a",
                Fatigue = 70f,
                SkillLevels = new Dictionary<string, float> { ["skill_watchful"] = 0.5f }
            };

            var verdict = model.EvaluateForRole(facts, Role());

            Assert.Equal(FitnessLevel.Impaired, verdict.BaseVerdict.Level);
            Assert.True(verdict.Allowed);
            Assert.True(verdict.Warning);
            Assert.Equal(8f, verdict.RecommendedMaxHours);
            Assert.Contains(FitnessReasonIds.SevereFatigue, verdict.WarningReasons);
        }

        [Fact]
        public void QuarantineIsAnIncapacitatingHardBlock()
        {
            var model = new FitnessForDutyModel(Thresholds());
            var verdict = model.EvaluateForRole(new FitnessEvaluationFacts
            {
                SurvivorId = "survivor_a",
                IsQuarantined = true,
                SkillLevels = new Dictionary<string, float> { ["skill_watchful"] = 1f }
            }, Role());

            Assert.Equal(FitnessLevel.Incapacitated, verdict.BaseVerdict.Level);
            Assert.False(verdict.Allowed);
            Assert.Contains(FitnessReasonIds.Quarantined, verdict.BlockingReasons);
            Assert.Contains(FitnessReasonIds.RoleQuarantine, verdict.BlockingReasons);
        }

        [Fact]
        public void RoleSkillAndHealthLimitsBlockWithoutChangingBaseFacts()
        {
            var model = new FitnessForDutyModel(Thresholds());
            var verdict = model.EvaluateForRole(new FitnessEvaluationFacts
            {
                SurvivorId = "survivor_a",
                Health = 40f,
                SkillLevels = new Dictionary<string, float> { ["skill_watchful"] = 0.01f }
            }, Role(minimumSkill: 0.5f, minimumHealth: 50f));

            Assert.Equal(FitnessLevel.Impaired, verdict.BaseVerdict.Level);
            Assert.False(verdict.Allowed);
            Assert.Contains(FitnessReasonIds.RoleSkillBelowMinimum, verdict.BlockingReasons);
            Assert.Contains(FitnessReasonIds.RoleHealthLimit, verdict.BlockingReasons);
        }

        [Fact]
        public void RecentDischargeProducesTimeBoundImpairedRecovery()
        {
            var model = new FitnessForDutyModel(Thresholds());

            var dischargeDay = model.Evaluate(new FitnessEvaluationFacts
            {
                SurvivorId = "survivor_a",
                DaysSinceDischarge = 0
            });
            var nextDay = model.Evaluate(new FitnessEvaluationFacts
            {
                SurvivorId = "survivor_a",
                DaysSinceDischarge = 1
            });
            var recovered = model.Evaluate(new FitnessEvaluationFacts
            {
                SurvivorId = "survivor_a",
                DaysSinceDischarge = 2
            });

            Assert.Equal(FitnessLevel.Impaired, dischargeDay.Level);
            Assert.Equal(FitnessLevel.Impaired, nextDay.Level);
            Assert.Contains(FitnessReasonIds.RecentDischarge, nextDay.DegradedFactors);
            Assert.Equal(FitnessLevel.Fit, recovered.Level);
            Assert.DoesNotContain(FitnessReasonIds.RecentDischarge, recovered.DegradedFactors);
        }

        [Fact]
        public void SleepDebtAttributesFitnessPressureToFatigueNeed()
        {
            var model = new FitnessForDutyModel(Thresholds());
            var verdict = model.Evaluate(new FitnessEvaluationFacts
            {
                SurvivorId = "survivor_a",
                DaysSinceSleep = 2
            });

            Assert.Equal(FitnessLevel.Impaired, verdict.Level);
            Assert.Contains(FitnessReasonIds.SleepDeprived, verdict.DegradedFactors);
            Assert.Contains(NeedKind.Fatigue, verdict.AffectedNeeds);
        }

        [Theory]
        [InlineData(1, FitnessLevel.Impaired, FitnessReasonIds.ActiveIllness)]
        [InlineData(2, FitnessLevel.Unfit, FitnessReasonIds.TerminalIllness)]
        [InlineData(3, FitnessLevel.Incapacitated, FitnessReasonIds.OutcomePending)]
        public void ActiveIllnessBandEscalatesDerivedFitness(
            int band, FitnessLevel expectedLevel, string expectedReason)
        {
            var model = new FitnessForDutyModel(Thresholds());
            var verdict = model.Evaluate(new FitnessEvaluationFacts
            {
                SurvivorId = "survivor_a",
                ActiveIllnessBand = band
            });

            Assert.Equal(expectedLevel, verdict.Level);
            if (expectedLevel == FitnessLevel.Incapacitated)
                Assert.Contains(expectedReason, verdict.BlockingReasons);
            else
                Assert.Contains(expectedReason, verdict.DegradedFactors);
        }

        [Fact]
        public void RepeatedFitnessEvaluationHasStableReasonsNeedsAndHours()
        {
            var model = new FitnessForDutyModel(Thresholds());
            var facts = new FitnessEvaluationFacts
            {
                SurvivorId = "survivor_a",
                Fatigue = 70f,
                Hunger = 65f,
                DaysSinceSleep = 2,
                ActiveIllnessBand = 1,
                SkillLevels = new Dictionary<string, float> { ["skill_watchful"] = 0.5f }
            };

            var first = model.EvaluateForRole(facts, Role());
            var second = model.EvaluateForRole(facts, Role());

            Assert.Equal(first.BaseVerdict.Level, second.BaseVerdict.Level);
            Assert.Equal(first.BaseVerdict.DegradedFactors, second.BaseVerdict.DegradedFactors);
            Assert.Equal(first.BaseVerdict.AffectedNeeds, second.BaseVerdict.AffectedNeeds);
            Assert.Equal(first.WarningReasons, second.WarningReasons);
            Assert.Equal(first.RecommendedMaxHours, second.RecommendedMaxHours);
        }

        [Fact]
        public void DutyRoleCatalogLoadsAllLiveRolesFromAuthoritativeData()
        {
            string dataDir = ResolveDataDirectory();
            var result = FitnessRoleCatalogLoader.LoadDetailed(
                dataDir, new FileSystemIO(), new SystemTextJsonSerializer());

            Assert.True(result.IsSuccess, string.Join("; ", result.Errors));
            Assert.NotNull(result.Catalog);
            Assert.Equal(DutyRosterIds.AssignmentRoles.Length, result.Catalog!.Roles.Count);
            Assert.True(result.Catalog.Roles.ContainsKey(DutyRosterIds.RoleExpedition));
            Assert.Equal(100f, result.Catalog.Thresholds.DoseImpairedMsv);
        }

        private static string ResolveDataDirectory()
        {
            if (CatalogLocator.TryFindDataDirectory(Directory.GetCurrentDirectory(), out string found))
                return found;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found))
                return found;
            throw new DirectoryNotFoundException("StreamingAssets/Data directory not found");
        }
    }
}
