// SPDX-License-Identifier: MIT
// ============================================================================
// Ashfall Core : Expansion 30 — The Press
// Subsystem    : Public Broadsheet, Almanac Printing & Information Distribution Engine
// Authority    : docs/expansions/wave4/expansion_30_the_press_plan.md
//                UNBLOCK-05_EXPANSION_WAVES_C3_EN_GATE.md §3.1, §5.3
// ============================================================================
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Print
{
    /// <summary>
    /// Classification of a printed publication run.
    /// </summary>
    public enum PublicationKind
    {
        Broadsheet  = 0,  // news, public notices — high reach, low retention
        Almanac     = 1,  // seasonal almanac — lower reach, high morale/knowledge value
        Pamphlet    = 2,  // short-run polemics / rumor debunking
        Wanted      = 3,  // bounty notices — targeted distribution
        Proclamation = 4  // official shelter governance notice
    }

    /// <summary>
    /// Mutable state of the shelter's printing press type tray.
    /// </summary>
    public sealed class TypeTrayState
    {
        /// <summary>Total movable type pieces available (0..n).</summary>
        public int TypePiecesAvailable      { get; set; } = 2000;
        /// <summary>Wear accumulated on the type tray (0..1000 permille; ≥800 = replacement needed).</summary>
        public int TypeWearPermille         { get; set; } = 0;
        /// <summary>Ink reservoir remaining (0..1000 permille).</summary>
        public int InkReservoirPermille     { get; set; } = 1000;
        /// <summary>Paper stock remaining (0..1000 permille of a full ream).</summary>
        public int PaperStockPermille       { get; set; } = 1000;

        public TypeTrayState Clone() => new TypeTrayState
        {
            TypePiecesAvailable = TypePiecesAvailable,
            TypeWearPermille    = TypeWearPermille,
            InkReservoirPermille = InkReservoirPermille,
            PaperStockPermille  = PaperStockPermille
        };
    }

    /// <summary>
    /// Immutable result of a print run publication.
    /// </summary>
    public readonly struct PrintRunResult
    {
        /// <summary>Actual copies printed this run.</summary>
        public int CopiesPrinted           { get; }
        /// <summary>Audience reach permille of shelter population (0..1000).</summary>
        public int AudienceReachPermille   { get; }
        /// <summary>Morale stabilization granted (0..500 permille).</summary>
        public int MoraleStabilizationPermille { get; }
        /// <summary>Ink consumed permille (deduct from reservoir).</summary>
        public int InkConsumedPermille     { get; }
        /// <summary>Paper consumed permille (deduct from stock).</summary>
        public int PaperConsumedPermille   { get; }
        /// <summary>Type wear incurred permille (add to TypeWearPermille).</summary>
        public int TypeWearIncurredPermille { get; }
        /// <summary>True if the print run was blocked by resource shortage.</summary>
        public bool BlockedByShortage      { get; }
        /// <summary>Human-readable shortage reason if BlockedByShortage is true.</summary>
        public string ShortageReason       { get; }

        public PrintRunResult(
            int copiesPrinted,
            int audienceReachPermille,
            int moraleStabilizationPermille,
            int inkConsumedPermille,
            int paperConsumedPermille,
            int typeWearIncurredPermille,
            bool blockedByShortage,
            string shortageReason)
        {
            CopiesPrinted              = Math.Max(0, copiesPrinted);
            AudienceReachPermille      = Math.Clamp(audienceReachPermille, 0, 1000);
            MoraleStabilizationPermille = Math.Clamp(moraleStabilizationPermille, 0, 500);
            InkConsumedPermille        = Math.Clamp(inkConsumedPermille, 0, 1000);
            PaperConsumedPermille      = Math.Clamp(paperConsumedPermille, 0, 1000);
            TypeWearIncurredPermille   = Math.Clamp(typeWearIncurredPermille, 0, 1000);
            BlockedByShortage          = blockedByShortage;
            ShortageReason             = shortageReason ?? string.Empty;
        }
    }

    /// <summary>
    /// Pure domain engine governing movable type composition, type tray wear,
    /// broadsheet publication runs, public notice distribution, rumor debunking,
    /// and shelter morale stabilization.
    /// Extends ArchiveDeskSystem seam without duplicating narrative registries.
    /// Zero engine references, integer permille determinism.
    /// </summary>
    public static class PublicBroadsheetPressEngine
    {
        /// <summary>Type wear permille above which print quality degrades noticeably.</summary>
        public const int TypeWearDegradationThreshold = 800;

        /// <summary>Minimum ink permille required to begin any print run.</summary>
        public const int MinimumInkPermille = 50;

        /// <summary>Minimum paper permille required to begin any print run.</summary>
        public const int MinimumPaperPermille = 30;

        /// <summary>
        /// Executes one publication print run, consuming ink, paper, and type wear.
        /// Mutates the TypeTrayState in-place with consumption.
        /// </summary>
        /// <param name="tray">Current press type tray state (mutated in-place).</param>
        /// <param name="kind">Publication kind (affects reach and morale impact).</param>
        /// <param name="targetCopies">Number of copies requested (1..5000).</param>
        /// <param name="compositorSkillPermille">Compositor operator skill (0..1000).</param>
        /// <param name="shelterPopulation">Current shelter population count (used for reach calc).</param>
        public static PrintRunResult ExecutePrintRun(
            TypeTrayState tray,
            PublicationKind kind,
            int targetCopies,
            int compositorSkillPermille,
            int shelterPopulation)
        {
            if (tray == null) throw new ArgumentNullException(nameof(tray));

            targetCopies         = Math.Clamp(targetCopies, 1, 5000);
            compositorSkillPermille = Math.Clamp(compositorSkillPermille, 0, 1000);
            shelterPopulation    = Math.Max(1, shelterPopulation);

            // Resource sufficiency checks
            if (tray.InkReservoirPermille < MinimumInkPermille)
                return new PrintRunResult(0, 0, 0, 0, 0, 0, true, "Ink reservoir critically low");
            if (tray.PaperStockPermille < MinimumPaperPermille)
                return new PrintRunResult(0, 0, 0, 0, 0, 0, true, "Paper stock depleted");
            if (tray.TypePiecesAvailable < 200)
                return new PrintRunResult(0, 0, 0, 0, 0, 0, true, "Insufficient movable type pieces");

            // Per-copy resource requirements
            int inkPerCopy   = kind switch
            {
                PublicationKind.Almanac      => 4,
                PublicationKind.Broadsheet   => 2,
                PublicationKind.Pamphlet     => 1,
                PublicationKind.Wanted       => 1,
                PublicationKind.Proclamation => 2,
                _                            => 2
            };
            int paperPerCopy = kind switch
            {
                PublicationKind.Almanac      => 3,
                PublicationKind.Broadsheet   => 2,
                PublicationKind.Pamphlet     => 1,
                PublicationKind.Wanted       => 1,
                PublicationKind.Proclamation => 1,
                _                            => 2
            };

            // Available capacity from current stock
            int inkCapacity   = (tray.InkReservoirPermille  * 1250) / (inkPerCopy   * 1000);
            int paperCapacity = (tray.PaperStockPermille * 1250) / (paperPerCopy * 1000);
            int actualCopies  = Math.Min(targetCopies, Math.Min(inkCapacity, paperCapacity));
            actualCopies      = Math.Max(0, actualCopies);

            // Consumption
            int inkConsumed   = Math.Min(tray.InkReservoirPermille,
                                    (actualCopies * inkPerCopy * 1000) / 1250);
            int paperConsumed = Math.Min(tray.PaperStockPermille,
                                    (actualCopies * paperPerCopy * 1000) / 1250);

            // Type wear per run
            int wearPerRun = kind switch
            {
                PublicationKind.Almanac      => 25,
                PublicationKind.Broadsheet   => 15,
                PublicationKind.Pamphlet     => 8,
                PublicationKind.Wanted       => 5,
                PublicationKind.Proclamation => 10,
                _                            => 12
            };
            // High wear type degrades quality; skilled compositor can slow wear
            int compositorWearReduction = (compositorSkillPermille * 5) / 1000;
            int netWear = Math.Max(1, wearPerRun - compositorWearReduction);

            // Apply consumption
            tray.InkReservoirPermille  = Math.Max(0, tray.InkReservoirPermille  - inkConsumed);
            tray.PaperStockPermille    = Math.Max(0, tray.PaperStockPermille    - paperConsumed);
            tray.TypeWearPermille      = Math.Min(1000, tray.TypeWearPermille    + netWear);

            // Audience reach: copies as fraction of shelter population, capped at 1000
            int rawReach = Math.Min(1000, (actualCopies * 1000) / shelterPopulation);

            // Quality degradation from worn type
            if (tray.TypeWearPermille >= TypeWearDegradationThreshold)
            {
                int degradation = (tray.TypeWearPermille - TypeWearDegradationThreshold) * 2;
                rawReach = Math.Max(0, rawReach - degradation / 10);
            }

            // Morale stabilization by kind
            int basesMorale = kind switch
            {
                PublicationKind.Almanac      => 300,
                PublicationKind.Broadsheet   => 150,
                PublicationKind.Pamphlet     => 80,
                PublicationKind.Proclamation => 120,
                PublicationKind.Wanted       => 40,
                _                            => 100
            };
            int moraleStab = (basesMorale * rawReach) / 1000;
            moraleStab = Math.Clamp(moraleStab, 0, 500);

            return new PrintRunResult(
                actualCopies,
                rawReach,
                moraleStab,
                inkConsumed,
                paperConsumed,
                netWear,
                false,
                string.Empty);
        }

        /// <summary>
        /// Calculates audience reach for a rumor debunking pamphlet and the
        /// credibility correction it applies to the shelter's information climate.
        /// Does NOT mutate narrative registries — returns plain integers for the
        /// host to route through the RumorSystem.
        /// </summary>
        /// <param name="rumorStrengthPermille">Current rumor strength (0..1000).</param>
        /// <param name="pamphletAudienceReachPermille">Reach achieved by the print run (0..1000).</param>
        /// <param name="evidenceQualityPermille">Evidence quality backing the debunk (0..1000).</param>
        /// <returns>Credibility reduction applied to the rumor (0..rumorStrengthPermille).</returns>
        public static int CalculateRumorDebunkingCorrection(
            int rumorStrengthPermille,
            int pamphletAudienceReachPermille,
            int evidenceQualityPermille)
        {
            rumorStrengthPermille        = Math.Clamp(rumorStrengthPermille, 0, 1000);
            pamphletAudienceReachPermille = Math.Clamp(pamphletAudienceReachPermille, 0, 1000);
            evidenceQualityPermille       = Math.Clamp(evidenceQualityPermille, 0, 1000);

            // Correction is proportional to reach × evidence quality
            int rawCorrection = (pamphletAudienceReachPermille * evidenceQualityPermille) / 1000;

            // Stubborn rumors resist debunking when they are very strong
            int resistance = (rumorStrengthPermille * 300) / 1000;
            int netCorrection = Math.Max(0, rawCorrection - resistance);

            return Math.Min(rumorStrengthPermille, netCorrection);
        }

        /// <summary>
        /// Applies type resetting maintenance — replaces worn type pieces with fresh stock.
        /// Mutates TypeWearPermille and TypePiecesAvailable.
        /// </summary>
        /// <param name="tray">Type tray to maintain.</param>
        /// <param name="freshTypePiecesAdded">Number of new type pieces added from foundry stock.</param>
        public static void RestoreTypeTray(TypeTrayState tray, int freshTypePiecesAdded)
        {
            if (tray == null) throw new ArgumentNullException(nameof(tray));
            freshTypePiecesAdded = Math.Max(0, freshTypePiecesAdded);

            tray.TypePiecesAvailable += freshTypePiecesAdded;

            // Each 200 fresh pieces reduces wear by ~100 permille (proportional)
            int wearReduction = (freshTypePiecesAdded * 100) / 200;
            tray.TypeWearPermille = Math.Max(0, tray.TypeWearPermille - wearReduction);
        }
    }
}
