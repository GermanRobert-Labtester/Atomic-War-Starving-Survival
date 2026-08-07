// GameBootstrap.Encounters.cs — boot/wire Encounter_* expedition set pieces.
using UnityEngine;

namespace AtomicWar._Game.Core
{
    public partial class GameBootstrap
    {
        /// <summary>
        /// Construct all Encounter_* trackers. Host hooks are offline-safe logs;
        /// expedition hosts call Engage/Resolve when parties hit encounter nodes.
        /// </summary>
        private void BootEncounters()
        {
            EncounterAmalgamation = new Encounter_Amalgamation();
            EncounterBurrowers = new BurrowersSystem("encounter_burrowers");
            EncounterFloodedMaze = new Encounter_FloodedMaze();
            EncounterGlowingDead = new Encounter_GlowingDead();
            EncounterGlowingStag = new Encounter_GlowingStag();
            EncounterHitAndRun = new Encounter_HitAndRun();
            EncounterLeeches = new Encounter_Leeches();
            EncounterMirelurker = new Encounter_Mirelurker();
            EncounterPressurePlate = new Encounter_PressurePlate();
            EncounterRiverPirates = new Encounter_RiverPirates();
            EncounterRoadblock = new Encounter_Roadblock();
            EncounterRobotDog = new Encounter_RobotDog();
            EncounterSleepingCamp = new Encounter_SleepingCamp();
            EncounterTripwireMaze = new Encounter_TripwireMaze();
            EncounterWarlordTank = new Encounter_WarlordTank();

            WireEncounters();
            Debug.Log("[GameBootstrap] Encounters ready (15 trackers).");
        }

        private void WireEncounters()
        {
            if (EncounterAmalgamation != null)
            {
                EncounterAmalgamation.OnAmalgamationEngaged += _ =>
                    Debug.Log("[GameBootstrap] ENCOUNTER: amalgamation engaged");
                EncounterAmalgamation.OnAmalgamationDefeated += _ =>
                    Debug.Log("[GameBootstrap] ENCOUNTER: amalgamation defeated");
                EncounterAmalgamation.OnFleeAttempt += (_, ok) =>
                    Debug.Log($"[GameBootstrap] ENCOUNTER: amalgamation flee ok={ok}");
            }

            if (EncounterBurrowers != null)
            {
                EncounterBurrowers.OnBreachTriggered += (id, level) =>
                    Debug.Log($"[GameBootstrap] ENCOUNTER: burrowers breach level={level} ({id})");
                EncounterBurrowers.OnFightOutcome += (id, survived) =>
                    Debug.Log($"[GameBootstrap] ENCOUNTER: burrowers fight survived={survived} ({id})");
                EncounterBurrowers.OnBreachPatched += id =>
                    Debug.Log($"[GameBootstrap] ENCOUNTER: burrowers breach patched ({id})");
            }

            if (EncounterFloodedMaze != null)
            {
                EncounterFloodedMaze.OnRoomEntered += (id, room) =>
                    Debug.Log($"[GameBootstrap] ENCOUNTER: flooded maze room {room} — '{id}'");
                EncounterFloodedMaze.OnLootFound += (id, loot) =>
                    Debug.Log($"[GameBootstrap] ENCOUNTER: flooded maze loot '{loot}' — '{id}'");
                EncounterFloodedMaze.OnDrowningStarted += id =>
                    Debug.Log($"[GameBootstrap] ENCOUNTER: flooded maze drowning — '{id}'");
                EncounterFloodedMaze.OnSurvivorDrowned += id =>
                    Debug.Log($"[GameBootstrap] ENCOUNTER: flooded maze drowned — '{id}'");
            }

            if (EncounterGlowingDead != null)
            {
                EncounterGlowingDead.OnRadTransferredToInventory += (id, rad) =>
                    Debug.Log($"[GameBootstrap] ENCOUNTER: glowing dead rad +{rad:F0} — '{id}'");
                EncounterGlowingDead.OnItemIrradiated += (id, item) =>
                    Debug.Log($"[GameBootstrap] ENCOUNTER: glowing dead irradiated '{item}' — '{id}'");
            }

            if (EncounterGlowingStag != null)
            {
                EncounterGlowingStag.OnStagSpotted += _ =>
                    Debug.Log("[GameBootstrap] ENCOUNTER: glowing stag spotted");
                EncounterGlowingStag.OnStagHunted += _ =>
                    Debug.Log("[GameBootstrap] ENCOUNTER: glowing stag hunted");
                EncounterGlowingStag.OnRadiotrophicMeatConsumed += (_, hp, rad, morale) =>
                    Debug.Log($"[GameBootstrap] ENCOUNTER: stag meat hp={hp:F0} radRes={rad:F0} morale={morale:F0}");
            }

            if (EncounterHitAndRun != null)
                EncounterHitAndRun.OnRunThemDownExecuted += (_, ok) =>
                    Debug.Log($"[GameBootstrap] ENCOUNTER: hit-and-run success={ok}");

            if (EncounterLeeches != null)
            {
                EncounterLeeches.OnLeechesAttached += (id, st) =>
                    Debug.Log($"[GameBootstrap] ENCOUNTER: leeches x{st.attachedLeechCount} on '{id}'");
                EncounterLeeches.OnLeechesBurnedOff += (id, _) =>
                    Debug.Log($"[GameBootstrap] ENCOUNTER: leeches burned off — '{id}'");
                EncounterLeeches.OnBloodLossCritical += (id, _) =>
                    Debug.Log($"[GameBootstrap] ENCOUNTER: leeches blood loss critical — '{id}'");
            }

            if (EncounterMirelurker != null)
            {
                EncounterMirelurker.OnMirelurkerEngaged += _ =>
                    Debug.Log("[GameBootstrap] ENCOUNTER: mirelurker engaged");
                EncounterMirelurker.OnMirelurkerDefeated += _ =>
                    Debug.Log("[GameBootstrap] ENCOUNTER: mirelurker defeated");
                EncounterMirelurker.OnDraggedUnderwater += _ =>
                    Debug.Log("[GameBootstrap] ENCOUNTER: mirelurker drag underwater");
                EncounterMirelurker.OnFleeAttempt += (_, ok) =>
                    Debug.Log($"[GameBootstrap] ENCOUNTER: mirelurker flee ok={ok}");
            }

            if (EncounterPressurePlate != null)
            {
                EncounterPressurePlate.OnTrapResponseChosen += (id, resp) =>
                    Debug.Log($"[GameBootstrap] ENCOUNTER: pressure plate {resp} — '{id}'");
                EncounterPressurePlate.OnTrapSurvived += id =>
                    Debug.Log($"[GameBootstrap] ENCOUNTER: pressure plate survived — '{id}'");
                EncounterPressurePlate.OnTrapKilled += id =>
                    Debug.Log($"[GameBootstrap] ENCOUNTER: pressure plate killed — '{id}'");
            }

            if (EncounterRiverPirates != null)
            {
                EncounterRiverPirates.OnPiratesEngaged += _ =>
                    Debug.Log("[GameBootstrap] ENCOUNTER: river pirates engaged");
                EncounterRiverPirates.OnPiratesDefeated += _ =>
                    Debug.Log("[GameBootstrap] ENCOUNTER: river pirates defeated");
                EncounterRiverPirates.OnLootRetrieved += _ =>
                    Debug.Log("[GameBootstrap] ENCOUNTER: river pirates loot retrieved");
                EncounterRiverPirates.OnLootSunk += _ =>
                    Debug.Log("[GameBootstrap] ENCOUNTER: river pirates loot sunk");
                EncounterRiverPirates.OnFleeFailed += _ =>
                    Debug.Log("[GameBootstrap] ENCOUNTER: river pirates flee failed");
            }

            if (EncounterRoadblock != null)
                EncounterRoadblock.OnRoadblockResolved += (_, choice) =>
                    Debug.Log($"[GameBootstrap] ENCOUNTER: roadblock {choice}");

            if (EncounterRobotDog != null)
            {
                EncounterRobotDog.OnRobotDogEngaged += (_, dmg) =>
                    Debug.Log($"[GameBootstrap] ENCOUNTER: robot dog hit for {dmg:F1}");
                EncounterRobotDog.OnRobotDogDefeated += _ =>
                    Debug.Log("[GameBootstrap] ENCOUNTER: robot dog defeated");
                EncounterRobotDog.OnRobotDogHacked += _ =>
                    Debug.Log("[GameBootstrap] ENCOUNTER: robot dog hacked");
                EncounterRobotDog.OnLootDropped += (_, scrap, motors) =>
                    Debug.Log($"[GameBootstrap] ENCOUNTER: robot dog loot scrap={scrap} motors={motors}");
            }

            if (EncounterSleepingCamp != null)
            {
                EncounterSleepingCamp.OnCampDiscovered += _ =>
                    Debug.Log("[GameBootstrap] ENCOUNTER: sleeping camp discovered");
                EncounterSleepingCamp.OnLootStolen += (_, val) =>
                    Debug.Log($"[GameBootstrap] ENCOUNTER: sleeping camp loot {val:F0}");
                EncounterSleepingCamp.OnFirefightInitiated += (_, sev) =>
                    Debug.Log($"[GameBootstrap] ENCOUNTER: sleeping camp firefight {sev}");
                EncounterSleepingCamp.OnFleeSuccessful += _ =>
                    Debug.Log("[GameBootstrap] ENCOUNTER: sleeping camp flee ok");
            }

            if (EncounterTripwireMaze != null)
            {
                EncounterTripwireMaze.OnMazeEntered += _ =>
                    Debug.Log("[GameBootstrap] ENCOUNTER: tripwire maze entered");
                EncounterTripwireMaze.OnPathChosen += (_, path, ok) =>
                    Debug.Log($"[GameBootstrap] ENCOUNTER: tripwire path {path} ok={ok}");
                EncounterTripwireMaze.OnExplosionTriggered += (_, dmg) =>
                    Debug.Log($"[GameBootstrap] ENCOUNTER: tripwire explosion {dmg:F0}");
                EncounterTripwireMaze.OnMazeSolved += _ =>
                    Debug.Log("[GameBootstrap] ENCOUNTER: tripwire maze solved");
                EncounterTripwireMaze.OnMazeBypassed += _ =>
                    Debug.Log("[GameBootstrap] ENCOUNTER: tripwire maze bypassed");
            }

            if (EncounterWarlordTank != null)
            {
                EncounterWarlordTank.OnTreadsDestroyed += id =>
                    Debug.Log($"[GameBootstrap] ENCOUNTER: warlord tank treads destroyed by '{id}'");
                EncounterWarlordTank.OnCrewFlushed += id =>
                    Debug.Log($"[GameBootstrap] ENCOUNTER: warlord tank crew flushed by '{id}'");
                EncounterWarlordTank.OnTankDestroyed += id =>
                    Debug.Log($"[GameBootstrap] ENCOUNTER: warlord tank destroyed by '{id}'");
            }
        }
    }
}
