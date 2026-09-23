using Ashfall.Core.Medical;
using Xunit;

namespace Ashfall.Core.Tests.Medical
{
    public class ProstheticConditionWearEngineTests
    {
        [Fact]
        public void EvaluateDailyWear_SimpleProsthetic_HighMaintenance_LowWear()
        {
            var result = ProstheticConditionWearEngine.EvaluateDailyWear(
                currentConditionPermille: 900,
                complexity: ProstheticComplexityClass.SimpleImprovised,
                laborIntensityPermille: 300,
                maintenanceQualityPermille: 800
            );

            Assert.True(result.WearDeltaPermille <= 10);
            Assert.Equal(600, result.BiomechanicalEfficiencyPermille); // Simple cap is 600
            Assert.Equal(0, result.FailureRiskPermille);
            Assert.False(result.RequiresImmediateMaintenance);
            Assert.Contains("Optimal alignment", result.MaintenanceStatus);
        }

        [Fact]
        public void EvaluateDailyWear_AdvancedArticulated_HeavyLabor_HighWear()
        {
            var result = ProstheticConditionWearEngine.EvaluateDailyWear(
                currentConditionPermille: 800,
                complexity: ProstheticComplexityClass.AdvancedArticulated,
                laborIntensityPermille: 900, // Heavy labor
                maintenanceQualityPermille: 100 // Poor maintenance
            );

            Assert.True(result.WearDeltaPermille >= 50);
            Assert.True(result.BiomechanicalEfficiencyPermille > 700);
            Assert.Equal(0, result.FailureRiskPermille);
        }

        [Fact]
        public void EvaluateDailyWear_CriticalCondition_TriggersFailureRiskAndServiceNotice()
        {
            var result = ProstheticConditionWearEngine.EvaluateDailyWear(
                currentConditionPermille: 150, // Critical
                complexity: ProstheticComplexityClass.StandardMechanical,
                laborIntensityPermille: 500,
                maintenanceQualityPermille: 0
            );

            Assert.True(result.FailureRiskPermille > 500);
            Assert.True(result.RequiresImmediateMaintenance);
            Assert.True(result.BiomechanicalEfficiencyPermille < 200);
            Assert.Contains("Imminent mechanical breakdown", result.MaintenanceStatus);
        }

        [Fact]
        public void EvaluateDailyWear_ZeroCondition_ZeroEfficiencyAndMaxFailureRisk()
        {
            var result = ProstheticConditionWearEngine.EvaluateDailyWear(
                currentConditionPermille: 0,
                complexity: ProstheticComplexityClass.AdvancedArticulated,
                laborIntensityPermille: 500,
                maintenanceQualityPermille: 500
            );

            Assert.Equal(0, result.NetConditionPermille);
            Assert.Equal(0, result.BiomechanicalEfficiencyPermille);
            Assert.Equal(1000, result.FailureRiskPermille);
            Assert.True(result.RequiresImmediateMaintenance);
            Assert.Contains("Imminent mechanical breakdown", result.MaintenanceStatus);
        }
    }
}
