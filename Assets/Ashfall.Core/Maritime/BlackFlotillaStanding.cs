using System;
using Ashfall.Core.Economy;

namespace Ashfall.Core.Maritime
{
    /// <summary>
    /// ASHFALL: THE BLACK FLOTILLA (Expansion 09) — standing thresholds for the
    /// Black Flotilla, expressed entirely on the existing <see cref="FactionStanceEngine"/>
    /// semantics (one trust track, existing threshold fields — no new meter).
    ///
    /// Canonical disposition tiers read from the same trust value the stance
    /// engine already stores for <see cref="FactionId"/>:
    ///   Hostile     — trust below 0: hailed, inspected, hostile at raid threshold.
    ///   Tolerated   — 0..29: flagged traffic, standard rates, no privileges.
    ///   Trading     — 30..54: exchange access, claim-tag courtesy, bulletins.
    ///   Trusted     — 55..79: charts/coordinates/tide intel, specialist stock.
    ///   Cooperation — trust ≥ 75: deep-dive cooperation, launch rights, kin berths.
    /// </summary>
    public static class BlackFlotillaStanding
    {
        /// <summary>Canonical faction id (holdfast_factions.json roster).</summary>
        public const string FactionId = "faction_black_flotilla";

        // Thresholds on the existing FactionStanceEngine semantics.
        public const float RaidThreshold = -50f;
        public const float RobThreshold = -20f;
        public const float MinTrustToTrade = 0f;
        public const float IntelShareThreshold = 40f;
        public const float RaidAggression = 0.35f;

        // Plan 23 tier boundaries (deepened semantics on the same trust scale).
        public const float SalvageTrustedTrust = 30f;
        public const float DeepCooperationTrust = 55f;

        /// <summary>Canonical Flotilla thresholds (single source for hosts/tests).</summary>
        public static FactionThresholds Thresholds => new FactionThresholds(
            factionId: FactionId,
            raidThreshold: RaidThreshold,
            robThreshold: RobThreshold,
            minTrustToTrade: MinTrustToTrade,
            intelShareThreshold: IntelShareThreshold,
            raidAggression: RaidAggression,
            trustInversion: false);

        /// <summary>Convenience registration on any live stance engine.</summary>
        public static void Register(FactionStanceEngine engine)
        {
            engine?.RegisterFaction(Thresholds);
        }

        /// <summary>True when flagged traffic may trade (exchange open).</summary>
        public static bool CanTrade(float trust) => trust >= MinTrustToTrade;

        /// <summary>True when charts/coordinates/tide-table intel may be shared.</summary>
        public static bool CanShareIntel(float trust) => trust >= IntelShareThreshold;

        /// <summary>True when claim-tag cooperation and salvage trust apply.</summary>
        public static bool IsSalvageTrusted(float trust) => trust >= SalvageTrustedTrust;

        /// <summary>True when deep-dive cooperation access is granted.</summary>
        public static bool CanCooperateOnDeepDives(float trust) => trust >= DeepCooperationTrust;

        /// <summary>Canonical disposition tier for a trust value.</summary>
        public static BlackFlotillaTier TierFor(float trust)
        {
            if (trust < MinTrustToTrade) return BlackFlotillaTier.Hostile;
            if (trust < SalvageTrustedTrust) return BlackFlotillaTier.Tolerated;
            if (trust < DeepCooperationTrust) return BlackFlotillaTier.Trading;
            if (trust < FactionStanceConstants.MaxTrust) return BlackFlotillaTier.SalvageTrusted;
            return BlackFlotillaTier.DeepCooperation;
        }
    }

    /// <summary>Canonical Flotilla disposition tiers (over the existing trust track).</summary>
    public enum BlackFlotillaTier
    {
        Hostile = 0,
        Tolerated = 1,
        Trading = 2,
        SalvageTrusted = 3,
        DeepCooperation = 4
    }
}
