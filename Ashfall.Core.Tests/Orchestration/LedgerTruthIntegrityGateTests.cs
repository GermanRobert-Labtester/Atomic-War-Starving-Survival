using Ashfall.Core.Orchestration;
using Xunit;

namespace Ashfall.Core.Tests.Orchestration
{
    public class LedgerTruthIntegrityGateTests
    {
        [Fact]
        public void Validate_ValidTerminalAndDeferredDecisions_PassesCleanly()
        {
            var decisions = new[]
            {
                new DecisionEntry("DEC-01", "Survivor Caregiving", "SIGNED"),
                new DecisionEntry("DEC-02", "Radio Program Production", "SIGNED"),
                new DecisionEntry("DEC-03", "Legacy Architecture", "RETIRED"),
                new DecisionEntry("DEC-04", "Future Feature", "DEFERRED-WITH-CONDITION", "Waiting for Wave 12")
            };

            var report = LedgerTruthIntegrityGate.Validate(decisions, quarantinedTests: null);

            Assert.True(report.IsValid);
            Assert.Equal(4, report.TotalDecisionsEvaluated);
            Assert.Equal(3, report.TerminalDecisionsCount);
            Assert.Equal(1, report.ValidDeferredCount);
            Assert.Empty(report.Violations);
        }

        [Fact]
        public void Validate_UnsignedWithoutCondition_FlagsViolation()
        {
            var decisions = new[]
            {
                new DecisionEntry("DEC-05", "Restock Rules", "OPEN", "") // Invalid: open without condition
            };

            var report = LedgerTruthIntegrityGate.Validate(decisions);

            Assert.False(report.IsValid);
            Assert.Single(report.Violations);
            Assert.Contains("invalid or open verdict", report.Violations[0]);
        }

        [Fact]
        public void Validate_QuarantinedTestsPresent_FlagsD21Violation()
        {
            var decisions = new[]
            {
                new DecisionEntry("DEC-01", "Core System", "SIGNED")
            };

            var quarantined = new[] { "StrayFlakyTestClass" };

            var report = LedgerTruthIntegrityGate.Validate(decisions, quarantined);

            Assert.False(report.IsValid);
            Assert.Equal(1, report.QuarantinedTestCount);
            Assert.Contains("Unauthorized test in quarantine manifest", report.Violations[0]);
        }

        [Fact]
        public void Validate_EmptyDecisions_ReturnsValidWithZeroCounts()
        {
            var report = LedgerTruthIntegrityGate.Validate(decisions: null, quarantinedTests: null);

            Assert.True(report.IsValid);
            Assert.Equal(0, report.TotalDecisionsEvaluated);
            Assert.Empty(report.Violations);
        }
    }
}
