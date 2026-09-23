using Ashfall.Core.Shelter;
using Xunit;

namespace Ashfall.Core.Tests.Shelter
{
    public class EmergencyMusterReadinessEngineTests
    {
        [Fact]
        public void Evaluate_TrainedWardensAndFreshDrill_AchievesHighReadiness()
        {
            var result = EmergencyMusterReadinessEngine.Evaluate(
                populationCount: 50,
                activeWardenCount: 5, // Exactly 1 per 10 people = 100% coverage
                daysSinceLastDrill: 3,
                lastDrillType: EmergencyDrillType.FullAlarmEvacuation,
                routeClearancePermille: 950,
                refugeChamberIntegrityPermille: 900,
                recentDrillCountIn14Days: 1
            );

            Assert.True(result.CompositeReadinessScorePermille >= 800);
            Assert.True(result.IsReadinessCertified);
            Assert.True(result.EstimatedEvacuationMinutes <= 6);
            Assert.True(result.MissingSurvivorRiskPermille <= 100);
            Assert.True(result.CascadeInterventionMarginMinutes >= 20);
            Assert.Equal(0, result.ComplianceFatiguePermille);
            Assert.Contains("High emergency readiness", result.ReadinessAdvisory);
        }

        [Fact]
        public void Evaluate_NoDrillsAndZeroWardens_ProducesCriticalDeficit()
        {
            var result = EmergencyMusterReadinessEngine.Evaluate(
                populationCount: 40,
                activeWardenCount: 0,
                daysSinceLastDrill: 120,
                lastDrillType: EmergencyDrillType.None,
                routeClearancePermille: 300,
                refugeChamberIntegrityPermille: 400
            );

            Assert.False(result.IsReadinessCertified);
            Assert.True(result.CompositeReadinessScorePermille < 350);
            Assert.True(result.MissingSurvivorRiskPermille >= 700);
            Assert.True(result.EstimatedEvacuationMinutes >= 15);
            Assert.True(result.CascadeInterventionMarginMinutes <= 10);
            Assert.Contains("Critical emergency deficit", result.ReadinessAdvisory);
        }

        [Fact]
        public void Evaluate_OverDrilling_InducesComplianceFatigue()
        {
            var normalDrill = EmergencyMusterReadinessEngine.Evaluate(
                populationCount: 30,
                activeWardenCount: 3,
                daysSinceLastDrill: 2,
                lastDrillType: EmergencyDrillType.FireSuppressionBrigade,
                routeClearancePermille: 800,
                refugeChamberIntegrityPermille: 800,
                recentDrillCountIn14Days: 2
            );

            var fatiguedDrill = EmergencyMusterReadinessEngine.Evaluate(
                populationCount: 30,
                activeWardenCount: 3,
                daysSinceLastDrill: 2,
                lastDrillType: EmergencyDrillType.FireSuppressionBrigade,
                routeClearancePermille: 800,
                refugeChamberIntegrityPermille: 800,
                recentDrillCountIn14Days: 5 // 3 excess drills -> 450 permille fatigue
            );

            Assert.Equal(0, normalDrill.ComplianceFatiguePermille);
            Assert.Equal(450, fatiguedDrill.ComplianceFatiguePermille);
            Assert.True(fatiguedDrill.CompositeReadinessScorePermille < normalDrill.CompositeReadinessScorePermille);
        }

        [Fact]
        public void Evaluate_StaleDrill_DecaysReadinessOverTime()
        {
            var fresh = EmergencyMusterReadinessEngine.Evaluate(
                populationCount: 20,
                activeWardenCount: 2,
                daysSinceLastDrill: 5,
                lastDrillType: EmergencyDrillType.ToxicBreachLockdown,
                routeClearancePermille: 800,
                refugeChamberIntegrityPermille: 800
            );

            var stale = EmergencyMusterReadinessEngine.Evaluate(
                populationCount: 20,
                activeWardenCount: 2,
                daysSinceLastDrill: 45, // Decays past 7 and past 30 days
                lastDrillType: EmergencyDrillType.ToxicBreachLockdown,
                routeClearancePermille: 800,
                refugeChamberIntegrityPermille: 800
            );

            Assert.True(stale.CompositeReadinessScorePermille < fresh.CompositeReadinessScorePermille);
        }
    }
}
