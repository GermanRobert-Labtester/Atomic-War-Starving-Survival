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
            ShelterModuleAcidTrap = new ShelterModule_AcidTrap();
            ShelterModuleAutodoc = new ShelterModule_Autodoc();
            ShelterModuleCctv = new ShelterModule_CCTV();
            ShelterModuleClassroom = new ShelterModule_Classroom();
            ShelterModuleConfessional = new ShelterModule_Confessional();
            ShelterModuleConveyor = new ShelterModule_Conveyor();
            ShelterModuleDaylightSensor = new ShelterModule_DaylightSensor();
            ShelterModuleDroneStation = new ShelterModule_DroneStation();
            ShelterModuleHoloEmitter = new ShelterModule_HoloEmitter();
            ShelterModuleInsectFarm = new ShelterModule_InsectFarm();
            ShelterModuleLathe = new ShelterModule_Lathe();
            ShelterModuleMortar = new ShelterModule_Mortar();
            ShelterModulePanicButton = new ShelterModule_PanicButton();
            ShelterModulePitfall = new ShelterModule_Pitfall();
            ShelterModuleReloader = new ShelterModule_Reloader();
            ShelterModuleSorter = new ShelterModule_Sorter();
            ShelterModuleThermostat = new ShelterModule_Thermostat();
            ShelterModuleWasteChute = new ShelterModule_WasteChute();
            ShelterModuleAutopsy = new ShelterModule_Autopsy();
            ShelterModuleBatteryBank = new ShelterModule_BatteryBank();
            ShelterModuleBioLatrine = new ShelterModule_BioLatrine();
            ShelterModuleChoreBoard = new ShelterModule_ChoreBoard();
            ShelterModuleDeadManSwitch = new ShelterModule_DeadManSwitch();
            ShelterModuleDeconShower = new ShelterModule_DeconShower();
            ShelterModuleDialysis = new ShelterModule_Dialysis();
            ShelterModuleDistressBeacon = new ShelterModule_DistressBeacon();
            ShelterModuleDronePad = new ShelterModule_DronePad();
            ShelterModuleGarage = new ShelterModule_Garage();
            ShelterModuleGunRack = new ShelterModule_GunRack();
            ShelterModuleHammock = new ShelterModule_Hammock();
            ShelterModuleHandCrank = new ShelterModule_HandCrank();
            ShelterModuleHotShower = new ShelterModule_HotShower();
            ShelterModuleIncinerator = new ShelterModule_Incinerator();
            ShelterModuleMagmaTap = new MagmaTapSystem("shelter_module_magma_tap");
            ShelterModuleMotionSensor = new ShelterModule_MotionSensor();
            ShelterModulePanicRoom = new PanicRoomSystem("shelter_module_panic_room");
            ShelterModulePrintingPress = new ShelterModule_PrintingPress();
            ShelterModulePunchingBag = new ShelterModule_PunchingBag();
            ShelterModuleRainBarrel = new ShelterModule_RainBarrel();
            ShelterModuleRecordPlayer = new ShelterModule_RecordPlayer();
            ShelterModuleSprinklers = new ShelterModule_Sprinklers();
            ShelterModuleThumper = new ThumperSystem("shelter_module_thumper");
            ShelterModuleTreadmillGen = new ShelterModule_TreadmillGen();
            ShelterModuleTurret = new ShelterModule_Turret();
            ShelterModuleVaultDoor = new ShelterModule_VaultDoor();
            ShelterModuleWoodStove = new ShelterModule_WoodStove();

            WireShelterModules();
            Debug.Log("[GameBootstrap] Shelter modules ready (46 CaptureState modules).");
        }

        private void WireShelterModules()
        {
            if (ShelterModuleAcidTrap != null)
            {
                ShelterModuleAcidTrap.OnArmed += () =>
                    Debug.Log("[GameBootstrap] SHELTER_MODULE: acid trap armed");
                ShelterModuleAcidTrap.OnTriggered += (killed, lootLost) =>
                    Debug.Log($"[GameBootstrap] SHELTER_MODULE: acid trap triggered killed={killed} lootLost={lootLost}");
                ShelterModuleAcidTrap.OnToxicityApplied += days =>
                    Debug.Log($"[GameBootstrap] SHELTER_MODULE: acid trap toxic {days}d");
                ShelterModuleAcidTrap.OnRefilled += () =>
                    Debug.Log("[GameBootstrap] SHELTER_MODULE: acid trap refilled");
            }

            if (ShelterModuleAutodoc != null)
            {
                ShelterModuleAutodoc.OnSurgeryCompleted += (patient, affliction) =>
                    Debug.Log($"[GameBootstrap] SHELTER_MODULE: autodoc surgery '{affliction}' on '{patient}'");
                ShelterModuleAutodoc.OnTraumaApplied += (patient, trauma) =>
                    Debug.Log($"[GameBootstrap] SHELTER_MODULE: autodoc trauma {trauma:F2} on '{patient}'");
                ShelterModuleAutodoc.OnParanoiaApplied += (patient, paranoia) =>
                    Debug.Log($"[GameBootstrap] SHELTER_MODULE: autodoc paranoia {paranoia:F2} on '{patient}'");
            }

            if (ShelterModuleCctv != null)
            {
                ShelterModuleCctv.OnCCTVActivated += () =>
                    Debug.Log("[GameBootstrap] SHELTER_MODULE: cctv activated");
                ShelterModuleCctv.OnCCTVDeactivated += () =>
                    Debug.Log("[GameBootstrap] SHELTER_MODULE: cctv deactivated");
                ShelterModuleCctv.OnParanoiaEliminated += id =>
                    Debug.Log($"[GameBootstrap] SHELTER_MODULE: cctv paranoia cleared for '{id}'");
                ShelterModuleCctv.OnUnseenEliminated += id =>
                    Debug.Log($"[GameBootstrap] SHELTER_MODULE: cctv unseen cleared for '{id}'");
            }

            if (ShelterModuleClassroom != null)
            {
                ShelterModuleClassroom.OnChildEnrolled += id =>
                    Debug.Log($"[GameBootstrap] SHELTER_MODULE: classroom enrolled '{id}'");
                ShelterModuleClassroom.OnNoiseHalted += id =>
                    Debug.Log($"[GameBootstrap] SHELTER_MODULE: classroom noise halted '{id}'");
                ShelterModuleClassroom.OnStatIncreased += (id, stat) =>
                    Debug.Log($"[GameBootstrap] SHELTER_MODULE: classroom +{stat} for '{id}'");
            }

            if (ShelterModuleConfessional != null)
            {
                ShelterModuleConfessional.OnSpeakerEntered += id =>
                    Debug.Log($"[GameBootstrap] SHELTER_MODULE: confessional speaker '{id}'");
                ShelterModuleConfessional.OnListenerEntered += id =>
                    Debug.Log($"[GameBootstrap] SHELTER_MODULE: confessional listener '{id}'");
                ShelterModuleConfessional.OnSessionStarted += (sp, li) =>
                    Debug.Log($"[GameBootstrap] SHELTER_MODULE: confessional session '{sp}' / '{li}'");
                ShelterModuleConfessional.OnGuiltCured += (id, guilt) =>
                    Debug.Log($"[GameBootstrap] SHELTER_MODULE: confessional guilt cured '{guilt}' for '{id}'");
                ShelterModuleConfessional.OnSessionEnded += (sp, li, ok) =>
                    Debug.Log($"[GameBootstrap] SHELTER_MODULE: confessional ended ok={ok} '{sp}'/'{li}'");
            }

            if (ShelterModuleConveyor != null)
            {
                ShelterModuleConveyor.OnItemMoved += (item, from, to) =>
                    Debug.Log($"[GameBootstrap] SHELTER_MODULE: conveyor '{item}' {from}→{to}");
                ShelterModuleConveyor.OnSurvivorLacerated += id =>
                    Debug.Log($"[GameBootstrap] SHELTER_MODULE: conveyor lacerated '{id}'");
            }

            if (ShelterModuleDaylightSensor != null)
            {
                ShelterModuleDaylightSensor.OnLightsDisabled += () =>
                    Debug.Log("[GameBootstrap] SHELTER_MODULE: daylight sensor lights off");
                ShelterModuleDaylightSensor.OnLightsEnabled += () =>
                    Debug.Log("[GameBootstrap] SHELTER_MODULE: daylight sensor lights on");
                ShelterModuleDaylightSensor.OnPowerSaved += watts =>
                    Debug.Log($"[GameBootstrap] SHELTER_MODULE: daylight sensor saved {watts:F0}W");
            }

            if (ShelterModuleDroneStation != null)
            {
                ShelterModuleDroneStation.OnDronesDeployed += () =>
                    Debug.Log("[GameBootstrap] SHELTER_MODULE: drones deployed");
                ShelterModuleDroneStation.OnWasteCleaned += room =>
                    Debug.Log($"[GameBootstrap] SHELTER_MODULE: drones cleaned '{room}'");
                ShelterModuleDroneStation.OnModuleRepaired += (room, mod) =>
                    Debug.Log($"[GameBootstrap] SHELTER_MODULE: drones repaired '{mod}' in '{room}'");
            }

            if (ShelterModuleHoloEmitter != null)
            {
                ShelterModuleHoloEmitter.OnEmittersActivated += () =>
                    Debug.Log("[GameBootstrap] SHELTER_MODULE: holo emitters on");
                ShelterModuleHoloEmitter.OnEmittersDeactivated += () =>
                    Debug.Log("[GameBootstrap] SHELTER_MODULE: holo emitters off");
            }

            if (ShelterModuleInsectFarm != null)
            {
                ShelterModuleInsectFarm.OnProteinHarvested += (shelter, amt) =>
                    Debug.Log($"[GameBootstrap] SHELTER_MODULE: insect farm +{amt:F1} protein ({shelter})");
                ShelterModuleInsectFarm.OnChirpingDisturbed += id =>
                    Debug.Log($"[GameBootstrap] SHELTER_MODULE: insect farm chirp disturbs '{id}'");
            }

            if (ShelterModuleLathe != null)
            {
                ShelterModuleLathe.OnPartsProduced += (room, n) =>
                    Debug.Log($"[GameBootstrap] SHELTER_MODULE: lathe produced {n} in '{room}'");
                ShelterModuleLathe.OnNoisePollution += room =>
                    Debug.Log($"[GameBootstrap] SHELTER_MODULE: lathe noise in '{room}'");
            }

            if (ShelterModuleMortar != null)
            {
                ShelterModuleMortar.OnNodeBombarded += (op, node) =>
                    Debug.Log($"[GameBootstrap] SHELTER_MODULE: mortar bombard '{node}' by '{op}'");
                ShelterModuleMortar.OnLootVaporized += node =>
                    Debug.Log($"[GameBootstrap] SHELTER_MODULE: mortar vaporized loot on '{node}'");
            }

            if (ShelterModulePanicButton != null)
            {
                ShelterModulePanicButton.OnLockdownActivated += () =>
                    Debug.Log("[GameBootstrap] SHELTER_MODULE: panic lockdown on");
                ShelterModulePanicButton.OnLockdownDeactivated += () =>
                    Debug.Log("[GameBootstrap] SHELTER_MODULE: panic lockdown off");
                ShelterModulePanicButton.OnSurvivorLockedIn += id =>
                    Debug.Log($"[GameBootstrap] SHELTER_MODULE: panic locked '{id}'");
                ShelterModulePanicButton.OnRaiderHalted += id =>
                    Debug.Log($"[GameBootstrap] SHELTER_MODULE: panic halted raider '{id}'");
            }

            if (ShelterModulePitfall != null)
            {
                ShelterModulePitfall.OnRaiderKilled += total =>
                    Debug.Log($"[GameBootstrap] SHELTER_MODULE: pitfall kill total={total}");
                ShelterModulePitfall.OnPitfallExhausted += () =>
                    Debug.Log("[GameBootstrap] SHELTER_MODULE: pitfall exhausted");
                ShelterModulePitfall.OnLootLost += () =>
                    Debug.Log("[GameBootstrap] SHELTER_MODULE: pitfall loot crushed");
            }

            if (ShelterModuleReloader != null)
            {
                ShelterModuleReloader.OnAmmoReloaded += (id, n) =>
                    Debug.Log($"[GameBootstrap] SHELTER_MODULE: reloader +{n} live for '{id}'");
                ShelterModuleReloader.OnDudProduced += (id, n) =>
                    Debug.Log($"[GameBootstrap] SHELTER_MODULE: reloader +{n} duds for '{id}'");
            }

            if (ShelterModuleSorter != null)
            {
                ShelterModuleSorter.OnItemRouted += (item, dest) =>
                    Debug.Log($"[GameBootstrap] SHELTER_MODULE: sorter '{item}' → {dest}");
                ShelterModuleSorter.OnHaulingFatigueEliminated += () =>
                    Debug.Log("[GameBootstrap] SHELTER_MODULE: sorter hauling fatigue cleared");
            }

            if (ShelterModuleThermostat != null)
            {
                ShelterModuleThermostat.OnHeaterToggled += (room, on) =>
                    Debug.Log($"[GameBootstrap] SHELTER_MODULE: thermostat heater {(on ? "on" : "off")} in '{room}'");
                ShelterModuleThermostat.OnHeatstrokePrevented += () =>
                    Debug.Log("[GameBootstrap] SHELTER_MODULE: thermostat heatstroke prevented");
                ShelterModuleThermostat.OnFuelWastePrevented += () =>
                    Debug.Log("[GameBootstrap] SHELTER_MODULE: thermostat fuel waste prevented");
            }

            if (ShelterModuleWasteChute != null)
            {
                ShelterModuleWasteChute.OnWasteDeposited += (id, dest) =>
                    Debug.Log($"[GameBootstrap] SHELTER_MODULE: waste chute '{id}' → {dest}");
            }

            if (ShelterModuleAutopsy != null)
            {
                ShelterModuleAutopsy.OnAutopsyPerformed += (st, doc, skill, node) =>
                    Debug.Log($"[GameBootstrap] SHELTER_MODULE: autopsy by '{doc}' skill={skill:F2} intel='{node}'");
                ShelterModuleAutopsy.OnRoomEnteredDisgustMoraleDropped += (st, id, drop) =>
                    Debug.Log($"[GameBootstrap] SHELTER_MODULE: autopsy disgust '{id}' morale-{drop:F2}");
            }

            if (ShelterModuleBatteryBank != null)
            {
                ShelterModuleBatteryBank.OnSilentRunningToggled += (st, on) =>
                    Debug.Log($"[GameBootstrap] SHELTER_MODULE: battery silent running {(on ? "on" : "off")}");
                ShelterModuleBatteryBank.OnPowerStored += (st, watts) =>
                    Debug.Log($"[GameBootstrap] SHELTER_MODULE: battery stored {watts:F0}W");
            }

            if (ShelterModuleBioLatrine != null)
            {
                ShelterModuleBioLatrine.OnHighYieldFertilizerProduced += (st, n) =>
                    Debug.Log($"[GameBootstrap] SHELTER_MODULE: bio latrine fertilizer +{n}");
            }

            if (ShelterModuleChoreBoard != null)
            {
                ShelterModuleChoreBoard.OnGlobalChoreEfficiencyBuffApplied += (st, mult) =>
                    Debug.Log($"[GameBootstrap] SHELTER_MODULE: chore board mult={mult:F2}");
            }

            if (ShelterModuleDeadManSwitch != null)
            {
                ShelterModuleDeadManSwitch.OnArmed += (st, op) =>
                    Debug.Log($"[GameBootstrap] SHELTER_MODULE: dead man switch armed by '{op}'");
                ShelterModuleDeadManSwitch.OnTriggered += st =>
                    Debug.Log("[GameBootstrap] SHELTER_MODULE: dead man switch triggered");
                ShelterModuleDeadManSwitch.OnRevengeBroadcast += st =>
                    Debug.Log("[GameBootstrap] SHELTER_MODULE: dead man switch revenge broadcast");
            }

            if (ShelterModuleDeconShower != null)
            {
                ShelterModuleDeconShower.OnInstantDecontaminationExecuted += (st, id) =>
                    Debug.Log($"[GameBootstrap] SHELTER_MODULE: decon shower '{id}'");
            }

            if (ShelterModuleDialysis != null)
            {
                ShelterModuleDialysis.OnTreatmentStarted += (id, hours) =>
                    Debug.Log($"[GameBootstrap] SHELTER_MODULE: dialysis start '{id}' {hours:F0}h");
                ShelterModuleDialysis.OnTreatmentTick += (id, ok) =>
                    Debug.Log($"[GameBootstrap] SHELTER_MODULE: dialysis tick '{id}' ok={ok}");
                ShelterModuleDialysis.OnTreatmentCompleted += id =>
                    Debug.Log($"[GameBootstrap] SHELTER_MODULE: dialysis done '{id}'");
                ShelterModuleDialysis.OnTreatmentFailed += id =>
                    Debug.Log($"[GameBootstrap] SHELTER_MODULE: dialysis failed '{id}'");
            }

            if (ShelterModuleDistressBeacon != null)
            {
                ShelterModuleDistressBeacon.OnBeaconActivated += st =>
                    Debug.Log("[GameBootstrap] SHELTER_MODULE: distress beacon on");
                ShelterModuleDistressBeacon.OnArrival += (st, kind) =>
                    Debug.Log($"[GameBootstrap] SHELTER_MODULE: distress arrival {kind}");
            }

            if (ShelterModuleDronePad != null)
            {
                ShelterModuleDronePad.OnAutomatedMappingCompleted += (st, n) =>
                    Debug.Log($"[GameBootstrap] SHELTER_MODULE: drone pad mapped {n}");
                ShelterModuleDronePad.OnDroneDestroyedInStorm += st =>
                    Debug.Log("[GameBootstrap] SHELTER_MODULE: drone pad storm loss");
            }

            if (ShelterModuleGarage != null)
            {
                ShelterModuleGarage.OnVehicleParkedInGarage += (st, id) =>
                    Debug.Log($"[GameBootstrap] SHELTER_MODULE: garage parked '{id}'");
                ShelterModuleGarage.OnVehicleRetrievedFromGarage += (st, id) =>
                    Debug.Log($"[GameBootstrap] SHELTER_MODULE: garage retrieved '{id}'");
            }

            if (ShelterModuleGunRack != null)
            {
                ShelterModuleGunRack.OnWeaponLockedAway += (st, w) =>
                    Debug.Log($"[GameBootstrap] SHELTER_MODULE: gun rack locked '{w}'");
                ShelterModuleGunRack.OnWeaponIssuedToSurvivor += (st, w, id) =>
                    Debug.Log($"[GameBootstrap] SHELTER_MODULE: gun rack issued '{w}' to '{id}'");
            }

            if (ShelterModuleHammock != null)
            {
                ShelterModuleHammock.OnHammockSleptIn += (st, id) =>
                    Debug.Log($"[GameBootstrap] SHELTER_MODULE: hammock rest '{id}'");
            }

            if (ShelterModuleHandCrank != null)
            {
                ShelterModuleHandCrank.OnPowerCrankedFatigued += (st, id, watts) =>
                    Debug.Log($"[GameBootstrap] SHELTER_MODULE: hand crank '{id}' +{watts:F0}W");
            }

            if (ShelterModuleHotShower != null)
            {
                ShelterModuleHotShower.OnHotShowerTakenMoraleBoosted += (st, id, boost) =>
                    Debug.Log($"[GameBootstrap] SHELTER_MODULE: hot shower '{id}' morale+{boost:F2}");
            }

            if (ShelterModuleIncinerator != null)
            {
                ShelterModuleIncinerator.OnMaterialIncinerated += (st, item, ash) =>
                    Debug.Log($"[GameBootstrap] SHELTER_MODULE: incinerator '{item}' ash={ash:F1}");
                ShelterModuleIncinerator.OnCorpseSmokeHatchVisibilityMaxed += st =>
                    Debug.Log("[GameBootstrap] SHELTER_MODULE: incinerator corpse smoke max");
            }

            if (ShelterModuleMagmaTap != null)
            {
                ShelterModuleMagmaTap.OnInstalled += id =>
                    Debug.Log($"[GameBootstrap] SHELTER_MODULE: magma tap installed '{id}'");
                ShelterModuleMagmaTap.OnHeatChanged += (id, heat) =>
                    Debug.Log($"[GameBootstrap] SHELTER_MODULE: magma tap heat '{id}' +{heat:F1}");
            }

            if (ShelterModuleMotionSensor != null)
            {
                ShelterModuleMotionSensor.OnThreatPingedOnMap += (st, id, n) =>
                    Debug.Log($"[GameBootstrap] SHELTER_MODULE: motion sensor '{id}' threats={n}");
            }

            if (ShelterModulePanicRoom != null)
            {
                ShelterModulePanicRoom.OnBuilt += id =>
                    Debug.Log($"[GameBootstrap] SHELTER_MODULE: panic room built '{id}'");
                ShelterModulePanicRoom.OnLocked += id =>
                    Debug.Log($"[GameBootstrap] SHELTER_MODULE: panic room locked '{id}'");
                ShelterModulePanicRoom.OnReleased += id =>
                    Debug.Log($"[GameBootstrap] SHELTER_MODULE: panic room released '{id}'");
                ShelterModulePanicRoom.OnSiegeSurvival += (id, active) =>
                    Debug.Log($"[GameBootstrap] SHELTER_MODULE: panic room siege survival '{id}' active={active}");
            }

            if (ShelterModulePrintingPress != null)
            {
                ShelterModulePrintingPress.OnPressBuilt += st =>
                    Debug.Log("[GameBootstrap] SHELTER_MODULE: printing press built");
                ShelterModulePrintingPress.OnMoneyForged += (st, n) =>
                    Debug.Log($"[GameBootstrap] SHELTER_MODULE: printing press forged {n}");
                ShelterModulePrintingPress.OnForgeryDetected += st =>
                    Debug.Log("[GameBootstrap] SHELTER_MODULE: printing press forgery detected");
                ShelterModulePrintingPress.OnBloodFeudTriggered += st =>
                    Debug.Log("[GameBootstrap] SHELTER_MODULE: printing press blood feud");
            }

            if (ShelterModulePunchingBag != null)
            {
                ShelterModulePunchingBag.OnAngerVentedSafely += (st, id, amount) =>
                    Debug.Log($"[GameBootstrap] SHELTER_MODULE: punching bag '{id}' vent={amount:F2}");
            }

            if (ShelterModuleRainBarrel != null)
            {
                ShelterModuleRainBarrel.OnBarrelBurstFromFreeze += st =>
                    Debug.Log("[GameBootstrap] SHELTER_MODULE: rain barrel burst freeze");
                ShelterModuleRainBarrel.OnWaterCollected += (st, n) =>
                    Debug.Log($"[GameBootstrap] SHELTER_MODULE: rain barrel +{n} water");
            }

            if (ShelterModuleRecordPlayer != null)
            {
                ShelterModuleRecordPlayer.OnMoraleAuraActive += (st, aura) =>
                    Debug.Log($"[GameBootstrap] SHELTER_MODULE: record player aura={aura:F2}");
                ShelterModuleRecordPlayer.OnRecordScratchedAuraBroken += st =>
                    Debug.Log("[GameBootstrap] SHELTER_MODULE: record player scratched");
            }

            if (ShelterModuleSprinklers != null)
            {
                ShelterModuleSprinklers.OnFireExtinguishedWaterDumped += (st, n) =>
                    Debug.Log($"[GameBootstrap] SHELTER_MODULE: sprinklers dumped {n}");
            }

            if (ShelterModuleThumper != null)
            {
                ShelterModuleThumper.OnThumperToggled += (id, on) =>
                    Debug.Log($"[GameBootstrap] SHELTER_MODULE: thumper '{id}' {(on ? "on" : "off")}");
                ShelterModuleThumper.OnThumperTick += id =>
                    Debug.Log($"[GameBootstrap] SHELTER_MODULE: thumper tick '{id}'");
            }

            if (ShelterModuleTreadmillGen != null)
            {
                ShelterModuleTreadmillGen.OnTreadmillMannedPowerGenerated += (st, id, watts) =>
                    Debug.Log($"[GameBootstrap] SHELTER_MODULE: treadmill '{id}' +{watts:F0}W");
            }

            if (ShelterModuleTurret != null)
            {
                ShelterModuleTurret.OnTurretFiredInRaid += (st, shots, dmg) =>
                    Debug.Log($"[GameBootstrap] SHELTER_MODULE: turret fired shots={shots} dmg={dmg:F1}");
            }

            if (ShelterModuleVaultDoor != null)
            {
                ShelterModuleVaultDoor.OnDoorStateChanged += (st, open) =>
                    Debug.Log($"[GameBootstrap] SHELTER_MODULE: vault door {(open ? "open" : "closed")}");
                ShelterModuleVaultDoor.OnDoorStuckDueToPowerFailure += st =>
                    Debug.Log("[GameBootstrap] SHELTER_MODULE: vault door stuck (no power)");
            }

            if (ShelterModuleWoodStove != null)
            {
                ShelterModuleWoodStove.OnStoveLitHeatGenerated += (st, heat) =>
                    Debug.Log($"[GameBootstrap] SHELTER_MODULE: wood stove heat +{heat:F1}");
                ShelterModuleWoodStove.OnCarbonMonoxidePoisoningTriggered += (st, id) =>
                    Debug.Log($"[GameBootstrap] SHELTER_MODULE: wood stove CO '{id}'");
            }
        }
    }
}
