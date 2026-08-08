// GameBootstrap.Encounters.cs — most Encounter_* class trackers demoted.
// Live default path: EncounterSO + ExpeditionSystem.ResolveEncounterWithPsychology.
// REPROMOTE-Encounter-001: Encounter_Roadblock is live again (map tag / SO id dispatch).
using UnityEngine;

namespace AtomicWar._Game.Core
{
    public partial class GameBootstrap
    {
        /// <summary>
        /// Construct re-promoted class trackers only. Remaining Encounter_* stay null
        /// (demoted). SO expedition encounters remain the primary live path.
        /// </summary>
        private void BootEncounters()
        {
            // REPROMOTE-Encounter-001 — roadblock class tracker for map-tag / SO dispatch.
            EncounterRoadblock = new Encounter_Roadblock();
            WireEncounters();
            Debug.Log("[GameBootstrap] Encounters: Roadblock live (REPROMOTE-001); 14 others dormant.");
        }

        private void WireEncounters()
        {
            if (EncounterRoadblock == null) return;
            EncounterRoadblock.OnRoadblockResolved += (st, choice) =>
                Debug.Log($"[GameBootstrap] ENCOUNTER: roadblock resolved {choice} (toll={st?.tollFuelCost})");
        }
    }
}
