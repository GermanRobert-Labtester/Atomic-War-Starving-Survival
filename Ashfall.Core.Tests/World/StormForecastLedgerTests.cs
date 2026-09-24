// SPDX-License-Identifier: MIT
// Expansion 33 — storm forecast ledger (stateful owner over DEC-87).

using System;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests.World
{
    public sealed class StormForecastLedgerTests
    {
        [Fact]
        public void EvaluateForecast_UsesLiveObservationSkill()
        {
            var ledger = new StormForecastLedger();
            var strong = new StormForecastResult(900, ForecastConfidenceTier.HighConfidence, StormSeverityClass.AshStorm, 6, true);
            Assert.True(strong.IssueWarning);

            ledger.AddObservationSkill(1000); // clamps to a full-skill observation post
            var result = ledger.EvaluateForecast(6, StormSeverityClass.AshStorm, 1);
            Assert.Equal(ForecastConfidenceTier.HighConfidence, result.ConfidenceTier);
            Assert.Equal(ForecastConfidenceTier.HighConfidence, ledger.LastTier);
        }

        [Fact]
        public void TickDay_DecaysSkillAndDrill_AndRunDrillResets()
        {
            var ledger = new StormForecastLedger();
            ledger.RunDrill();
            Assert.Equal(1000, ledger.DrillRecencyPermille);

            int skillBefore = ledger.ObservationPostSkillPermille;
            ledger.TickDay(1, 100);
            Assert.True(ledger.ObservationPostSkillPermille < skillBefore);
            Assert.True(ledger.DrillRecencyPermille < 1000);
        }

        [Fact]
        public void IssueWarning_CountsOnlyQualifyingForecasts()
        {
            var ledger = new StormForecastLedger();
            var quiet = new StormForecastResult(900, ForecastConfidenceTier.HighConfidence, StormSeverityClass.Clear, 6, false);
            var loud = new StormForecastResult(900, ForecastConfidenceTier.HighConfidence, StormSeverityClass.AshStorm, 6, true);

            Assert.False(ledger.IssueWarning(quiet));
            Assert.True(ledger.IssueWarning(loud));
            Assert.Equal(1, ledger.WarningsIssued);
        }

        [Fact]
        public void CaptureRestore_RoundTripsAndSchemaGates()
        {
            var ledger = new StormForecastLedger();
            ledger.RunDrill();
            ledger.AddObservationSkill(50);
            var state = ledger.CaptureState();

            var restored = new StormForecastLedger();
            restored.RestoreState(state);
            Assert.Equal(ledger.ObservationPostSkillPermille, restored.ObservationPostSkillPermille);
            Assert.Equal(ledger.DrillRecencyPermille, restored.DrillRecencyPermille);

            var newer = ledger.CaptureState();
            newer.SchemaVersion = 99;
            Assert.Throws<InvalidOperationException>(() => restored.RestoreState(newer));
        }

        [Fact]
        public void AssessReadiness_IsReadOnlyPassthrough()
        {
            var ledger = new StormForecastLedger();
            var result = ledger.AssessReadiness(1000, 1000, 1000, 1000, StormSeverityClass.BlackRain);
            Assert.True(result.CanAbsorbBlackRain);
            Assert.Equal(SeasonalReadinessBand.Fortified, result.ReadinessBand);
            Assert.Equal(0, ledger.WarningsIssued);
        }
    }
}
