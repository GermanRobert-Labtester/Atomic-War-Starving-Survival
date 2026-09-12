// SPDX-License-Identifier: MIT
namespace Ashfall.Core.Culture
{
    /// <summary>
    /// Plans 178/190 — canonical milestone identities for vault chronicle
    /// creation (DEBT-178-CREATION-TO-VAULT). Keeps the mapping out of the host
    /// so it is testable and so the same milestone cannot be recorded under two
    /// different keys by accident.
    ///
    /// The vault already owns the append + dedup; this type only names events.
    /// </summary>
    public static class ArchiveChronicleMilestones
    {
        public const string Memorial = "memorial";
        public const string ExpeditionReturn = "expedition_return";

        /// <summary>Stable chronicle key for a survivor entering the memorial.</summary>
        public static string MemorialKey(string survivorId) => $"chronicle_memorial_{survivorId}";

        /// <summary>Stable chronicle key for a completed expedition.</summary>
        public static string ExpeditionKey(string expeditionId) => $"chronicle_expedition_{expeditionId}";
    }
}
