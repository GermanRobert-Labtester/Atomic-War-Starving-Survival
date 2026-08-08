// GameBootstrap.EventTrackers.cs — boot/wire Event_* narrative systems with CaptureState.
// (Separate from GameBootstrap.Events.cs which hosts Safe Haven choice handlers.)
using UnityEngine;

namespace AtomicWar._Game.Core
{
    public partial class GameBootstrap
    {
        /// <summary>
        /// Construct Event_* trackers that already implement Capture/Restore.
        /// Remaining events without CR land in a follow-up batch.
        /// Host hooks are offline-safe logs; event hosts fire real APIs.
        /// </summary>
        private void BootEvents()
        {
            // DEMOTE-Events-001 — Event_* trackers are unticked dormant classes.
            Debug.Log("[GameBootstrap] Events demoted (27 dormant trackers).");
        }

        private void WireEvents()
        {
        }
    }
}
