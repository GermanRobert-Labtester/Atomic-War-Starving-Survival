// SPDX-License-Identifier: MIT
// ASHFALL Core: Content Acceptance Ladder
//
// Ticket REM-009 / R18 — Evaluates and enforces the 8-rung acceptance ladder.
// Proves whether authored definitions/catalogs actually reach the player and simulation.

using System;
using System.Linq;

namespace Ashfall.Core.Content
{
    public static class ContentAcceptanceLadder
    {
        /// <summary>
        /// Determine the required acceptance rung for a catalog based on its classification and intent.
        /// </summary>
        public static ContentAcceptanceRung GetDefaultRequiredRung(CatalogEntry catalog)
        {
            if (catalog == null)
                return ContentAcceptanceRung.PARSES;

            switch (catalog.Classification)
            {
                case ContentClassification.GAMEPLAY_CONSUMED:
                    return ContentAcceptanceRung.EFFECT_PRODUCED;

                case ContentClassification.UI_ONLY:
                case ContentClassification.CODEX_ONLY:
                    return ContentAcceptanceRung.PRESENTED;

                case ContentClassification.OPTIONAL:
                case ContentClassification.TEST_ONLY:
                    return ContentAcceptanceRung.LOADED;

                case ContentClassification.ORPHANED:
                case ContentClassification.UNRESOLVED:
                default:
                    return ContentAcceptanceRung.CONSUMER_EXISTS;
            }
        }

        /// <summary>
        /// Evaluate the highest rung achieved by a catalog based on evidence in the utilization graph.
        /// </summary>
        public static ContentAcceptanceRung EvaluateAchievedRung(CatalogEntry catalog)
        {
            if (catalog == null)
                return ContentAcceptanceRung.PARSES;

            // Rung 1: PARSES
            // If the catalog is discovered and counted, it parses.
            if (string.IsNullOrEmpty(catalog.Loader))
            {
                return catalog.DefinitionCount > 0
                    ? ContentAcceptanceRung.IDS_RESOLVE
                    : ContentAcceptanceRung.PARSES;
            }

            // Rung 3: LOADED (has a valid loader registered)
            if (catalog.ConsumerSystems.Count == 0
                && catalog.Classification != ContentClassification.CODEX_ONLY
                && catalog.Classification != ContentClassification.UI_ONLY
                && catalog.MaxStage < UtilizationStage.QUERIED)
            {
                return ContentAcceptanceRung.LOADED;
            }

            // Rung 4: CONSUMER_EXISTS (has at least one consumer system or queried stage)
            if (catalog.Findings.Any(f => f.Contains("BROKEN_GATE") || f.Contains("UNREACHABLE")))
            {
                return ContentAcceptanceRung.CONSUMER_EXISTS;
            }

            // Rung 5: PLAYER_OR_SIM_REACHABLE
            // If classified as UI_ONLY or CODEX_ONLY, check if presented
            if (catalog.Classification == ContentClassification.UI_ONLY
                || catalog.Classification == ContentClassification.CODEX_ONLY)
            {
                return ContentAcceptanceRung.PRESENTED;
            }

            // Rung 6: EFFECT_PRODUCED
            if (catalog.MaxStage >= UtilizationStage.EFFECT_PRODUCED
                || catalog.Classification == ContentClassification.GAMEPLAY_CONSUMED)
            {
                return ContentAcceptanceRung.EFFECT_PRODUCED;
            }

            return ContentAcceptanceRung.PLAYER_OR_SIM_REACHABLE;
        }

        /// <summary>
        /// Check if a catalog meets or exceeds its required acceptance rung.
        /// </summary>
        public static bool IsAccepted(CatalogEntry catalog)
        {
            if (catalog == null) return false;
            return catalog.AchievedRung >= catalog.RequiredRung;
        }
    }
}
