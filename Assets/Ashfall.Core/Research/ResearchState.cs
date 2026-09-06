using System;
using System.Collections.Generic;

namespace Ashfall.Core
{
    /// <summary>
    /// Unified Research state envelope. Engine-agnostic.
    /// </summary>
    [Serializable]
    public sealed class ResearchState
    {
        public string systemId = ResearchSystem.SystemId;
        public bool expansionUnlocked;
        public int currentDay;
        public List<string> unlockedIds = new List<string>();
        public string activeResearchId = string.Empty;
        public int activeResearchDays;
        public List<string> completedIds = new List<string>();

        // Plan 166: this is the sole research-point wallet. Producers such as
        // the workshop must use ResearchSystem APIs rather than maintaining a
        // parallel currency in a host or UI.
        public int researchPointsAvailable;
        public int researchPointsLifetimeEarned;
        public List<BlueprintProgressState> blueprintProgress = new List<BlueprintProgressState>();
    }

    /// <summary>Persisted progress for one authored blueprint.</summary>
    [Serializable]
    public sealed class BlueprintProgressState
    {
        public string blueprintId = string.Empty;
        public int progressPoints;
        public int requiredPoints;
        public string discoveryState = "unknown";
        public int completedDay;
        public List<string> sourceTechIds = new List<string>();

        public BlueprintProgressState Clone()
        {
            return new BlueprintProgressState
            {
                blueprintId = blueprintId,
                progressPoints = progressPoints,
                requiredPoints = requiredPoints,
                discoveryState = discoveryState,
                completedDay = completedDay,
                sourceTechIds = sourceTechIds != null
                    ? new List<string>(sourceTechIds)
                    : new List<string>()
            };
        }
    }
}
