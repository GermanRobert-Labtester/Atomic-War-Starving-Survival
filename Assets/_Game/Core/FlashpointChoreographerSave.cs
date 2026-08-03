using System;
using System.Collections.Generic;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Save/load snapshot of the Day-30 Flashpoint Choreographer. Lives
    /// in Core so the SaveSystem can hold a reference to the type
    /// without taking a dependency on the Flashpoint module (which
    /// already depends on Core). The actual choreographer implementation
    /// reads/writes this DTO via CaptureState / RestoreState methods.
    /// </summary>
    [Serializable]
    public class FlashpointChoreographerSave
    {
        /// <summary>Campaign days whose buildup side effects have applied.</summary>
        public List<int> BuildupDaysProcessed = new List<int>();

        /// <summary>Last completed step index in the choreography; -1 if not started.</summary>
        public int ChoreographyStepIndex = -1;

        /// <summary>Real seconds elapsed since the choreography started.</summary>
        public float ElapsedRealSeconds;

        /// <summary>True if the choreography has run to completion.</summary>
        public bool ChoreographyCompleted;
    }
}
