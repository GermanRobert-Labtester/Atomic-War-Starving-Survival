// GameBootstrap.ShelterModules.cs — boot/wire ShelterModule_* with CaptureState.
using UnityEngine;

namespace AtomicWar._Game.Core
{
    public partial class GameBootstrap
    {
        /// <summary>
        /// Construct ShelterModule_* systems that already implement Capture/Restore.
        /// Remaining modules without CR land in a follow-up batch.
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

            WireShelterModules();
            Debug.Log("[GameBootstrap] Shelter modules ready (18 CaptureState modules).");
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
        }
    }
}
