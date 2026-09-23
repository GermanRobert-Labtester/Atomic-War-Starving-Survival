using System;
using System.Collections.Generic;

namespace Ashfall.Core.Orchestration
{
    /// <summary>
    /// Represents a decision entry in the authoritative DECISION_REGISTER.md.
    /// </summary>
    public readonly struct DecisionEntry
    {
        public string DecisionId { get; }
        public string Title { get; }
        public string Verdict { get; }
        public string BlockingCondition { get; }

        public DecisionEntry(string decisionId, string title, string verdict, string blockingCondition = null)
        {
            DecisionId = decisionId ?? string.Empty;
            Title = title ?? string.Empty;
            Verdict = verdict ?? string.Empty;
            BlockingCondition = blockingCondition ?? string.Empty;
        }

        public bool IsTerminal =>
            string.Equals(Verdict, "SIGNED", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(Verdict, "DECLINED", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(Verdict, "RETIRED", StringComparison.OrdinalIgnoreCase);

        public bool IsValidDeferred =>
            string.Equals(Verdict, "DEFERRED-WITH-CONDITION", StringComparison.OrdinalIgnoreCase) &&
            !string.IsNullOrWhiteSpace(BlockingCondition);
    }

    /// <summary>
    /// Verification report produced by <see cref="LedgerTruthIntegrityGate"/>.
    /// </summary>
    public readonly struct LedgerTruthReport
    {
        public bool IsValid { get; }
        public int TotalDecisionsEvaluated { get; }
        public int TerminalDecisionsCount { get; }
        public int ValidDeferredCount { get; }
        public int QuarantinedTestCount { get; }
        public IReadOnlyList<string> Violations { get; }

        public LedgerTruthReport(
            bool isValid,
            int totalDecisionsEvaluated,
            int terminalDecisionsCount,
            int validDeferredCount,
            int quarantinedTestCount,
            IReadOnlyList<string> violations)
        {
            IsValid = isValid;
            TotalDecisionsEvaluated = totalDecisionsEvaluated;
            TerminalDecisionsCount = terminalDecisionsCount;
            ValidDeferredCount = validDeferredCount;
            QuarantinedTestCount = quarantinedTestCount;
            Violations = violations ?? Array.Empty<string>();
        }
    }

    /// <summary>
    /// Pure domain gate for verifying decision register invariants, quarantine truth,
    /// and census consistency (EN-08 / UNBLOCK-04 / UNBLOCK-05).
    /// </summary>
    public static class LedgerTruthIntegrityGate
    {
        /// <summary>
        /// Validates decision register invariants and ensures zero unauthorized quarantine entries.
        /// </summary>
        /// <param name="decisions">Decisions parsed or loaded from registry.</param>
        /// <param name="quarantinedTests">Currently quarantined test class names.</param>
        /// <returns>Immutable <see cref="LedgerTruthReport"/>.</returns>
        public static LedgerTruthReport Validate(
            IEnumerable<DecisionEntry> decisions,
            IEnumerable<string> quarantinedTests = null)
        {
            var violations = new List<string>();
            int total = 0;
            int terminal = 0;
            int deferred = 0;
            int quarantineCount = 0;

            if (decisions != null)
            {
                foreach (var d in decisions)
                {
                    total++;
                    if (string.IsNullOrWhiteSpace(d.DecisionId))
                    {
                        violations.Add("Decision entry missing DecisionId.");
                        continue;
                    }

                    if (d.IsTerminal)
                    {
                        terminal++;
                    }
                    else if (d.IsValidDeferred)
                    {
                        deferred++;
                    }
                    else
                    {
                        violations.Add($"Decision {d.DecisionId} ('{d.Title}') has invalid or open verdict '{d.Verdict}' without a named blocking condition.");
                    }
                }
            }

            if (quarantinedTests != null)
            {
                foreach (var test in quarantinedTests)
                {
                    if (!string.IsNullOrWhiteSpace(test))
                    {
                        quarantineCount++;
                        // Per D21 truth: quarantine is empty; any active quarantine entry is flagged
                        violations.Add($"Unauthorized test in quarantine manifest: '{test}'. Per D21, quarantine must remain empty.");
                    }
                }
            }

            bool isValid = violations.Count == 0;

            return new LedgerTruthReport(
                isValid: isValid,
                totalDecisionsEvaluated: total,
                terminalDecisionsCount: terminal,
                validDeferredCount: deferred,
                quarantinedTestCount: quarantineCount,
                violations: violations
            );
        }
    }
}
