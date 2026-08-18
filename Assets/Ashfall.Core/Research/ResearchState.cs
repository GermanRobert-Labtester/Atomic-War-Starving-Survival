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
    }
}
