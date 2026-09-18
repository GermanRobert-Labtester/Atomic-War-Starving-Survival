// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Mods
{
    /// <summary>
    /// Pure Core evaluator for game version and mod contract version compatibility ranges.
    /// Supports exact versions, comparison operators (>=, <=, >, <, ==), ranges (e.g. ">=1.0 <2.0"),
    /// caret (^) and tilde (~) expressions, and wildcards (*).
    /// </summary>
    public static class ModCompatibilityEvaluator
    {
        public const string DefaultGameVersion = "1.0.0";
        public const int DefaultModContractVersion = 1;

        /// <summary>
        /// Evaluates whether the specified current game version satisfies the mod's game range.
        /// Empty, null, or "*" matches any version.
        /// </summary>
        public static bool EvaluateGameVersion(string currentVersion, string? rangeExpression, out string? error)
        {
            error = null;
            if (string.IsNullOrWhiteSpace(rangeExpression) || rangeExpression.Trim() == "*")
                return true;

            if (!TryParseVersion(currentVersion, out Version? current))
            {
                error = $"current game version '{currentVersion}' is not a valid version";
                return false;
            }

            return EvaluateRange(current!, rangeExpression, out error);
        }

        /// <summary>
        /// Evaluates whether the specified current mod contract version satisfies the mod contract range.
        /// Empty, null, or "*" matches any version.
        /// </summary>
        public static bool EvaluateModContractVersion(int currentContractVersion, string? rangeExpression, out string? error)
        {
            error = null;
            if (string.IsNullOrWhiteSpace(rangeExpression) || rangeExpression.Trim() == "*")
                return true;

            var current = new Version(currentContractVersion, 0);
            return EvaluateRange(current, rangeExpression, out error);
        }

        /// <summary>
        /// Evaluates a multi-clause version range against a target version.
        /// </summary>
        public static bool EvaluateRange(Version current, string rangeExpression, out string? error)
        {
            error = null;
            string[] clauses = rangeExpression.Split(new[] { ' ', ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
            if (clauses.Length == 0)
                return true;

            foreach (string rawClause in clauses)
            {
                string clause = rawClause.Trim();
                if (clause == "*")
                    continue;

                // Caret range: ^1.2.3 -> >=1.2.3 <(major+1).0.0
                if (clause.StartsWith("^", StringComparison.Ordinal))
                {
                    string target = clause.Substring(1);
                    if (!TryParseVersion(target, out Version? vTarget))
                    {
                        error = $"malformed caret range '{clause}'";
                        return false;
                    }
                    if (current < vTarget)
                        return false;
                    var upper = new Version(vTarget!.Major + 1, 0);
                    if (current >= upper)
                        return false;
                    continue;
                }

                // Tilde range: ~1.2.3 -> >=1.2.3 <major.(minor+1).0
                if (clause.StartsWith("~", StringComparison.Ordinal))
                {
                    string target = clause.Substring(1);
                    if (!TryParseVersion(target, out Version? vTarget))
                    {
                        error = $"malformed tilde range '{clause}'";
                        return false;
                    }
                    if (current < vTarget)
                        return false;
                    int minor = vTarget!.Minor >= 0 ? vTarget.Minor : 0;
                    var upper = new Version(vTarget.Major, minor + 1);
                    if (current >= upper)
                        return false;
                    continue;
                }

                // Comparison operators
                string op;
                string verStr;
                if (clause.StartsWith(">=", StringComparison.Ordinal))
                {
                    op = ">=";
                    verStr = clause.Substring(2);
                }
                else if (clause.StartsWith("<=", StringComparison.Ordinal))
                {
                    op = "<=";
                    verStr = clause.Substring(2);
                }
                else if (clause.StartsWith(">", StringComparison.Ordinal))
                {
                    op = ">";
                    verStr = clause.Substring(1);
                }
                else if (clause.StartsWith("<", StringComparison.Ordinal))
                {
                    op = "<";
                    verStr = clause.Substring(1);
                }
                else if (clause.StartsWith("==", StringComparison.Ordinal))
                {
                    op = "==";
                    verStr = clause.Substring(2);
                }
                else if (clause.StartsWith("=", StringComparison.Ordinal))
                {
                    op = "==";
                    verStr = clause.Substring(1);
                }
                else
                {
                    op = "==";
                    verStr = clause;
                }

                if (!TryParseVersion(verStr, out Version? targetVer))
                {
                    error = $"malformed version in clause '{clause}'";
                    return false;
                }

                bool satisfied = op switch
                {
                    ">=" => current >= targetVer,
                    "<=" => current <= targetVer,
                    ">" => current > targetVer,
                    "<" => current < targetVer,
                    "==" => CompareVersionsEqual(current, targetVer!),
                    _ => false
                };

                if (!satisfied)
                    return false;
            }

            return true;
        }

        private static bool CompareVersionsEqual(Version a, Version b)
        {
            int aMajor = a.Major;
            int aMinor = a.Minor >= 0 ? a.Minor : 0;
            int aBuild = a.Build >= 0 ? a.Build : 0;

            int bMajor = b.Major;
            int bMinor = b.Minor >= 0 ? b.Minor : 0;
            int bBuild = b.Build >= 0 ? b.Build : 0;

            return aMajor == bMajor && aMinor == bMinor && aBuild == bBuild;
        }

        public static bool TryParseVersion(string input, out Version? version)
        {
            version = null;
            if (string.IsNullOrWhiteSpace(input))
                return false;

            string clean = input.Trim().TrimStart('v', 'V');
            if (int.TryParse(clean, out int major) && major >= 0)
            {
                version = new Version(major, 0);
                return true;
            }

            if (Version.TryParse(clean, out Version? parsed))
            {
                version = parsed;
                return true;
            }

            return false;
        }
    }
}
