// GameBootstrap.MapHazards.cs — boot/wire MapHazard_* expedition hazards.
using AtomicWar._Game.Survivors;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    public partial class GameBootstrap
    {
        private float _mapHazardHourAccum;
        private float _mapHazardMinuteAccum;

        /// <summary>
        /// Construct all MapHazard_* trackers. Host hooks are offline-safe logs;
        /// expedition hosts call Navigate/Attempt* when parties enter nodes.
        /// </summary>
        private void BootMapHazards()
        {
            MapHazardAcidGeyser = new MapHazard_AcidGeyser();
            MapHazardAshlanche = new MapHazard_Ashlanche();
            // DEMOTE-MapHazard-batch — MapHazardBiometricDoor demoted. Class kept dormant.
            // DEMOTE-MapHazard-batch — MapHazardCraterWall demoted. Class kept dormant.
            // DEMOTE-MapHazard-batch — MapHazardCrevice demoted. Class kept dormant.
            // DEMOTE-MapHazard-batch — MapHazardFlammableGas demoted. Class kept dormant.
            // DEMOTE-MapHazard-batch — MapHazardGasPockets demoted. Class kept dormant.
            // DEMOTE-MapHazard-batch — MapHazardMagneticAnomaly demoted. Class kept dormant.
            // DEMOTE-MapHazard-batch — MapHazardSinkholeCollapse demoted. Class kept dormant.
            // DEMOTE-MapHazard-batch — MapHazardVenusTrap demoted. Class kept dormant.
            WireMapHazards();
            Debug.Log("[GameBootstrap] Map hazards: AcidGeyser+Ashlanche live; 8 HANDLERS_ONLY demoted.");
        }

        private void WireMapHazards()
        {
            if (MapHazardAcidGeyser != null)
            {
                MapHazardAcidGeyser.OnEruptionStarted += node =>
                    Debug.Log($"[GameBootstrap] HAZARD: acid geyser erupting at '{node}'");
                MapHazardAcidGeyser.OnChemicalBurnsApplied += (id, dmg) =>
                {
                    // Host: apply health loss when a survivor is caught mid-eruption.
                    if (NeedsSystem == null || Survivors == null) return;
                    for (int i = 0; i < Survivors.Count; i++)
                    {
                        var sv = Survivors[i];
                        if (sv == null || sv.Id != id) continue;
                        NeedsSystem.Modify(sv, NeedKind.Health, -dmg);
                        break;
                    }
                };
            }

            if (MapHazardAshlanche != null)
            {
                MapHazardAshlanche.OnAvalancheTriggered += node =>
                    Debug.Log($"[GameBootstrap] HAZARD: ashlanche at '{node}'");
                MapHazardAshlanche.OnSuffocation += id =>
                    Debug.Log($"[GameBootstrap] HAZARD: ashlanche suffocation — '{id}'");
            }

            if (MapHazardBiometricDoor != null)
            {
                MapHazardBiometricDoor.OnDoorUnlocked += id =>
                    Debug.Log($"[GameBootstrap] HAZARD: biometric door unlocked by '{id}'");
                MapHazardBiometricDoor.OnDoorRejected += id =>
                    Debug.Log($"[GameBootstrap] HAZARD: biometric door rejected '{id}'");
            }

            if (MapHazardCraterWall != null)
            {
                MapHazardCraterWall.OnClimbCompleted += id =>
                    Debug.Log($"[GameBootstrap] HAZARD: crater wall climbed by '{id}'");
                MapHazardCraterWall.OnClimbFailed += id =>
                    Debug.Log($"[GameBootstrap] HAZARD: crater wall climb failed for '{id}'");
            }

            if (MapHazardCrevice != null)
            {
                MapHazardCrevice.OnJumpFailed += id =>
                    Debug.Log($"[GameBootstrap] HAZARD: crevice jump failed — '{id}'");
                MapHazardCrevice.OnBridgeBuilt += id =>
                    Debug.Log($"[GameBootstrap] HAZARD: crevice bridge built by '{id}'");
            }

            if (MapHazardFlammableGas != null)
            {
                MapHazardFlammableGas.OnSparkIgnited += id =>
                    Debug.Log($"[GameBootstrap] HAZARD: flammable gas ignited by '{id}'");
            }

            if (MapHazardGasPockets != null)
            {
                MapHazardGasPockets.OnIgnition += (node, dmg) =>
                    Debug.Log($"[GameBootstrap] HAZARD: gas pocket ignited at '{node}' ({dmg:F0} burn)");
            }

            if (MapHazardMagneticAnomaly != null)
            {
                MapHazardMagneticAnomaly.OnCompassScrambled += () =>
                    Debug.Log("[GameBootstrap] HAZARD: magnetic anomaly scrambled compass");
                MapHazardMagneticAnomaly.OnFogOfWarExpanded += () =>
                    Debug.Log("[GameBootstrap] HAZARD: magnetic anomaly expanded fog of war");
            }

            if (MapHazardSinkholeCollapse != null)
            {
                MapHazardSinkholeCollapse.OnCollapseTriggered += (id, subway) =>
                    Debug.Log($"[GameBootstrap] HAZARD: sinkhole dropped '{id}' to '{subway}'");
            }

            if (MapHazardVenusTrap != null)
            {
                MapHazardVenusTrap.OnArmLost += (id, arm) =>
                    Debug.Log($"[GameBootstrap] HAZARD: venus trap amputated {arm} from '{id}'");
                MapHazardVenusTrap.OnDisguiseSpotted += id =>
                    Debug.Log($"[GameBootstrap] HAZARD: venus trap spotted by '{id}'");
            }
        }

        /// <summary>
        /// Accumulate game hours: acid geyser TickHour; ashlanche TickMinute (60× per hour when buried).
        /// </summary>
        private void TickMapHazardsHourly(float gameHours)
        {
            if (gameHours <= 0f) return;

            _mapHazardHourAccum += gameHours;
            while (_mapHazardHourAccum >= 1f)
            {
                _mapHazardHourAccum -= 1f;
                MapHazardAcidGeyser?.TickHour();
            }

            // Ashlanche suffocation is minute-resolution; 1 game-hour → 60 minute ticks.
            _mapHazardMinuteAccum += gameHours * 60f;
            while (_mapHazardMinuteAccum >= 1f)
            {
                _mapHazardMinuteAccum -= 1f;
                MapHazardAshlanche?.TickMinute();
            }
        }
    }
}
