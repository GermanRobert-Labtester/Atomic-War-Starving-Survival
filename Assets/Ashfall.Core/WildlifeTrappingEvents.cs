// SPDX-License-Identifier: MIT
using System;

namespace Ashfall.Core
{
    /// <summary>
    /// Immutable domain event emitted when bait is stolen from a trap by NPC interference.
    /// </summary>
    [Serializable]
    public sealed class BaitStolenEvent
    {
        public string trapId = string.Empty;
        public string siteId = string.Empty;
        public string zoneId = string.Empty;
        public int campaignDay;

        public string EventIdentity => $"trap-theft:{campaignDay}:{siteId}:{trapId}";
    }

    /// <summary>
    /// Immutable domain event emitted when a trap is sabotaged by NPC interference.
    /// </summary>
    [Serializable]
    public sealed class TrapSabotagedEvent
    {
        public string trapId = string.Empty;
        public string siteId = string.Empty;
        public string zoneId = string.Empty;
        public int campaignDay;
        public int durabilityDamage;
        public int remainingDurability;
        public bool isBroken;

        public string EventIdentity => $"trap-sabotage:{campaignDay}:{siteId}:{trapId}";
    }
}
