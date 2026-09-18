// SPDX-License-Identifier: MIT
// ASHFALL Core: Content Acceptance Pipeline
//
// Plan 45 / C1[14]: Deterministic, fail-fast orchestrator for the 8-rung content acceptance ladder.
// Evaluates catalogs against required acceptance rungs and provides structured diagnostics.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Ashfall.Core.Content
{
    public sealed class ContentAcceptanceRungResult
    {
        public ContentAcceptanceRung Rung { get; }
        public bool Passed { get; }
        public string Message { get; }

        public ContentAcceptanceRungResult(ContentAcceptanceRung rung, bool passed, string message)
        {
            Rung = rung;
            Passed = passed;
            Message = message ?? string.Empty;
        }
    }

    public sealed class ContentAcceptancePipelineResult
    {
        public bool IsSuccess => FailedRung == null;
        public ContentAcceptanceRung? FailedRung { get; }
        public IReadOnlyList<ContentAcceptanceRungResult> RungResults { get; }
        public string Summary { get; }

        public ContentAcceptancePipelineResult(
            ContentAcceptanceRung? failedRung,
            IReadOnlyList<ContentAcceptanceRungResult> rungResults,
            string summary)
        {
            FailedRung = failedRung;
            RungResults = rungResults ?? Array.Empty<ContentAcceptanceRungResult>();
            Summary = summary ?? string.Empty;
        }

        public static ContentAcceptancePipelineResult Success(IReadOnlyList<ContentAcceptanceRungResult> results)
        {
            return new ContentAcceptancePipelineResult(null, results, "All content acceptance rungs satisfied.");
        }

        public static ContentAcceptancePipelineResult Failure(ContentAcceptanceRung failedRung, IReadOnlyList<ContentAcceptanceRungResult> results, string reason)
        {
            return new ContentAcceptancePipelineResult(failedRung, results, $"Content acceptance rejected at {failedRung}: {reason}");
        }
    }

    /// <summary>
    /// Programmatic acceptance pipeline orchestrator.
    /// Evaluates content catalogs against required rungs in strict dependency order (1..8) with fail-fast semantics.
    /// </summary>
    public static class ContentAcceptancePipeline
    {
        private static readonly ContentAcceptanceRung[] s_orderedRungs = new[]
        {
            ContentAcceptanceRung.PARSES,
            ContentAcceptanceRung.IDS_RESOLVE,
            ContentAcceptanceRung.LOADED,
            ContentAcceptanceRung.CONSUMER_EXISTS,
            ContentAcceptanceRung.PLAYER_OR_SIM_REACHABLE,
            ContentAcceptanceRung.EFFECT_PRODUCED,
            ContentAcceptanceRung.PRESENTED,
            ContentAcceptanceRung.SAVE_ROUNDTRIP
        };

        public static IReadOnlyList<ContentAcceptanceRung> OrderedRungs => s_orderedRungs;

        /// <summary>
        /// Evaluates a catalog against its required rung in strict ladder order.
        /// When <paramref name="failFast"/> is true, evaluation stops at the first failing rung.
        /// </summary>
        public static ContentAcceptancePipelineResult Evaluate(CatalogEntry catalog, bool failFast = true)
        {
            if (catalog == null)
                throw new ArgumentNullException(nameof(catalog));

            var rungResults = new List<ContentAcceptanceRungResult>();
            var requiredRung = catalog.RequiredRung != 0 ? catalog.RequiredRung : ContentAcceptanceLadder.GetDefaultRequiredRung(catalog);
            var achievedRung = ContentAcceptanceLadder.EvaluateAchievedRung(catalog);

            foreach (var rung in s_orderedRungs)
            {
                if (rung > requiredRung)
                    break;

                bool passed = achievedRung >= rung;
                string message = passed
                    ? $"Rung {rung} satisfied (achieved: {achievedRung})"
                    : $"Rung {rung} failed: required {requiredRung} but only achieved {achievedRung}";

                var rungResult = new ContentAcceptanceRungResult(rung, passed, message);
                rungResults.Add(rungResult);

                if (!passed && failFast)
                {
                    return ContentAcceptancePipelineResult.Failure(rung, rungResults, message);
                }
            }

            var firstFail = rungResults.FirstOrDefault(r => !r.Passed);
            if (firstFail != null)
            {
                return ContentAcceptancePipelineResult.Failure(firstFail.Rung, rungResults, firstFail.Message);
            }

            return ContentAcceptancePipelineResult.Success(rungResults);
        }

        /// <summary>
        /// Evaluates a batch of catalogs. Stops at the first catalog failure when <paramref name="failFast"/> is true.
        /// </summary>
        public static ContentAcceptancePipelineResult EvaluateBatch(IEnumerable<CatalogEntry> catalogs, bool failFast = true)
        {
            if (catalogs == null)
                throw new ArgumentNullException(nameof(catalogs));

            var combinedResults = new List<ContentAcceptanceRungResult>();

            foreach (var catalog in catalogs)
            {
                var result = Evaluate(catalog, failFast);
                combinedResults.AddRange(result.RungResults);

                if (!result.IsSuccess && failFast)
                {
                    return ContentAcceptancePipelineResult.Failure(
                        result.FailedRung!.Value,
                        combinedResults,
                        $"Catalog '{catalog.Path}' failed at rung {result.FailedRung.Value}: {result.Summary}");
                }
            }

            var firstFail = combinedResults.FirstOrDefault(r => !r.Passed);
            if (firstFail != null)
            {
                return ContentAcceptancePipelineResult.Failure(firstFail.Rung, combinedResults, firstFail.Message);
            }

            return ContentAcceptancePipelineResult.Success(combinedResults);
        }
    }
}
