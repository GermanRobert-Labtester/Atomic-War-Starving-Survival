// GameBootstrap.ShelterModules.cs — boot/wire ShelterModule_* with CaptureState.
using UnityEngine;

namespace AtomicWar._Game.Core
{
    public partial class GameBootstrap
    {
        /// <summary>
        /// Construct ShelterModule_* systems that implement Capture/Restore (46 total).
        /// Host hooks are offline-safe logs; shelter UI/combat hosts fire real APIs.
        /// </summary>
        private void BootShelterModules()
        {
            // DEMOTE-ShelterModules-001 — ShelterModule_* trackers are unticked dormant classes.
            Debug.Log("[GameBootstrap] Shelter modules demoted (46 dormant modules).");
        }

        private void WireShelterModules()
        {
        }
    }
}
