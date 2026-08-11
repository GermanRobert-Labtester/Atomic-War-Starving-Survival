// GameBootstrap.MapAnomalies.cs — boot/wire MapAnomaly_* expedition anomalies.
using System;
using UnityEngine;
using AtomicWar._Game.Utilities;
using AtomicWar._Game.Survivors;

using AtomicWar._Game.World;

namespace AtomicWar._Game.Core
{
    public partial class GameBootstrap
    {
        /// <summary>
        /// Construct all MapAnomaly_* trackers. Host hooks are offline-safe logs;
        /// expedition hosts call Enter/Traverse/Harvest when parties hit nodes.
        /// </summary>
        private void BootMapAnomalies()
        {
            // DEMOTE-MapAnomaly-batch — MapAnomalyAshDunes demoted. Class kept dormant.
            // DEMOTE-MapAnomaly-batch — MapAnomalyBoilingLake demoted. Class kept dormant.
            // DEMOTE-MapAnomaly-batch — MapAnomalyCherenkov demoted. Class kept dormant.
            // DEMOTE-MapAnomaly-batch — MapAnomalyDogDen demoted. Class kept dormant.
            // DEMOTE-MapAnomaly-batch — MapAnomalyDontLook demoted. Class kept dormant.
            MapAnomalyDryCoral = new MapAnomaly_DryCoral();
            // DEMOTE-MapAnomaly-batch — MapAnomalyFloodedSubway demoted. Class kept dormant.
            // DEMOTE-MapAnomaly-batch — MapAnomalyGlassCrater demoted. Class kept dormant.
            // DEMOTE-MapAnomaly-batch — MapAnomalyMassGrave demoted. Class kept dormant.
            // DEMOTE-MapAnomaly-batch — MapAnomalyMirage demoted. Class kept dormant.
            // DEMOTE-MapAnomaly-batch — MapAnomalyPetrifiedForest demoted. Class kept dormant.
            // DEMOTE-MapAnomaly-batch — MapAnomalyQuietZone demoted. Class kept dormant.
            // DEMOTE-MapAnomaly-batch — MapAnomalyRustedTank demoted. Class kept dormant.
            // DEMOTE-MapAnomaly-batch — MapAnomalyServerFarm demoted. Class kept dormant.
            // DEMOTE-MapAnomaly-batch — MapAnomalySinkhole demoted. Class kept dormant.
            // DEMOTE-MapAnomaly-batch — MapAnomalyTangledDrop demoted. Class kept dormant.
            // DEMOTE-MapAnomaly-batch — MapAnomalyTireFire demoted. Class kept dormant.
            // DEMOTE-MapAnomaly-batch — MapAnomalyUxoNuke demoted. Class kept dormant.
            WireMapAnomalies();
            GameLog.Log("[GameBootstrap] Map anomalies: DryCoral live (rad); 17 HANDLERS_ONLY demoted.");
        }

        private void WireMapAnomalies()
        {
            if (MapAnomalyAshDunes != null)
                MapAnomalyAshDunes.OnFirearmJammedByAsh += (_, id) =>
                    GameLog.Log($"[GameBootstrap] ANOMALY: ash dunes jammed firearm for '{id}'");

            if (MapAnomalyBoilingLake != null)
            {
                MapAnomalyBoilingLake.OnBoatSunk += _ =>
                    GameLog.Log("[GameBootstrap] ANOMALY: boiling lake sank boat");
                MapAnomalyBoilingLake.OnCenterIslandLooted += (_, loot) =>
                    GameLog.Log($"[GameBootstrap] ANOMALY: boiling lake loot {loot:F0}");
            }

            if (MapAnomalyCherenkov != null)
                MapAnomalyCherenkov.OnRadStageApplied += (id, stage) =>
                    GameLog.Log($"[GameBootstrap] ANOMALY: cherenkov stage {stage} on '{id}'");

            if (MapAnomalyDogDen != null)
                MapAnomalyDogDen.OnHoarderLootClaimed += (_, loot) =>
                    GameLog.Log($"[GameBootstrap] ANOMALY: dog den loot x{loot?.Count ?? 0}");

            if (MapAnomalyDontLook != null)
                MapAnomalyDontLook.OnCatatonicBreak += id =>
                    GameLog.Log($"[GameBootstrap] ANOMALY: don't-look catatonic break — '{id}'");

            if (MapAnomalyDryCoral != null)
            {
                MapAnomalyDryCoral.OnCrystalHarvested += (node, n) =>
                    GameLog.Log($"[GameBootstrap] ANOMALY: dry coral crystal #{n} at '{node}'");
                // Scavenger harvest spike — route through RadiationSystem (MISC-007).
                // mSv is treated as a one-hour ambient rate so Expose also grows lifetime.
                MapAnomalyDryCoral.OnRadExposure += (id, msv) =>
                {
                    if (msv <= 0f || RadiationSystem == null) return;
                    Survivor sv = FindSurvivorById(id);
                    if (sv == null || !sv.IsAlive) return;
                    RadiationSystem.Expose(sv, msv, 1f);
                    GameLog.Log($"[GameBootstrap] ANOMALY: dry coral {msv:F0} mSv on '{id}' via RadiationSystem");
                };
            }

            if (MapAnomalyFloodedSubway != null)
                MapAnomalyFloodedSubway.OnSubwayWadedHypothermiaInflicted += (_, id) =>
                    GameLog.Log($"[GameBootstrap] ANOMALY: flooded subway hypothermia — '{id}'");

            if (MapAnomalyGlassCrater != null)
                MapAnomalyGlassCrater.OnGlassSlipLacerationContracted += (_, id, aff) =>
                    GameLog.Log($"[GameBootstrap] ANOMALY: glass crater laceration '{aff}' on '{id}'");

            if (MapAnomalyMassGrave != null)
            {
                MapAnomalyMassGrave.OnMassGraveTraversedMoraleDropped += (_, id, drop) =>
                    GameLog.Log($"[GameBootstrap] ANOMALY: mass grave morale -{drop:F0} ({id})");
                MapAnomalyMassGrave.OnCorpsesRobbedKarmaSanityPenalized += (_, id, k, s) =>
                    GameLog.Log($"[GameBootstrap] ANOMALY: grave rob karma -{k:F0} sanity -{s:F0} ({id})");
            }

            if (MapAnomalyMirage != null)
            {
                MapAnomalyMirage.OnPlayerDeceived += node =>
                    GameLog.Log($"[GameBootstrap] ANOMALY: mirage deceived at '{node}'");
                MapAnomalyMirage.OnMirageDissolved += node =>
                    GameLog.Log($"[GameBootstrap] ANOMALY: mirage dissolved '{node}'");
            }

            if (MapAnomalyPetrifiedForest != null)
                MapAnomalyPetrifiedForest.OnTreeHarvested += (node, count, scrap) =>
                    GameLog.Log($"[GameBootstrap] ANOMALY: petrified forest {count} trees → {scrap:F1} carbon at '{node}'");

            if (MapAnomalyQuietZone != null)
            {
                MapAnomalyQuietZone.OnMasksRemoved += id =>
                    GameLog.Log($"[GameBootstrap] ANOMALY: quiet zone masks off — '{id}'");
                MapAnomalyQuietZone.OnSanityDrained += (id, drain) =>
                    GameLog.Log($"[GameBootstrap] ANOMALY: quiet zone sanity -{drain:F2} ({id})");
            }

            if (MapAnomalyRustedTank != null)
                MapAnomalyRustedTank.OnSurvivorsShelteredInsideTank += (_, id) =>
                    GameLog.Log($"[GameBootstrap] ANOMALY: rusted tank shelter — '{id}'");

            if (MapAnomalyServerFarm != null)
            {
                MapAnomalyServerFarm.OnHeatstrokeContracted += (_, id) =>
                    GameLog.Log($"[GameBootstrap] ANOMALY: server farm heatstroke — '{id}'");
                MapAnomalyServerFarm.OnGoldHarvested += (_, gold) =>
                    GameLog.Log($"[GameBootstrap] ANOMALY: server farm gold +{gold}");
            }

            if (MapAnomalySinkhole != null)
            {
                MapAnomalySinkhole.OnRopeSnappedFatal += (_, id) =>
                    GameLog.Log($"[GameBootstrap] ANOMALY: sinkhole rope snap — '{id}'");
                MapAnomalySinkhole.OnCaveInTriggered += (_, id) =>
                    GameLog.Log($"[GameBootstrap] ANOMALY: sinkhole cave-in — '{id}'");
            }

            if (MapAnomalyTangledDrop != null)
                MapAnomalyTangledDrop.OnSupplyDropRetrieved += (_, method) =>
                    GameLog.Log($"[GameBootstrap] ANOMALY: tangled drop retrieved via {method}");

            if (MapAnomalyTireFire != null)
                MapAnomalyTireFire.OnRegionalAirPollutionApplied += (_, dist, pen) =>
                    GameLog.Log($"[GameBootstrap] ANOMALY: tire fire pollution dist={dist} pen={pen:F2}");

            if (MapAnomalyUxoNuke != null)
            {
                MapAnomalyUxoNuke.OnFissileMaterialHarvested += _ =>
                    GameLog.Log("[GameBootstrap] ANOMALY: UXO nuke fissile material harvested");
                MapAnomalyUxoNuke.OnWarheadDetonatedRunEnded += _ =>
                    GameLog.Log("[GameBootstrap] ANOMALY: UXO nuke detonated — run ended");
            }
        }

        private Survivor FindSurvivorById(string id)
        {
            if (string.IsNullOrEmpty(id) || Survivors == null) return null;
            for (int i = 0; i < Survivors.Count; i++)
            {
                Survivor s = Survivors[i];
                if (s != null && string.Equals(s.Id, id, StringComparison.Ordinal))
                    return s;
            }
            return null;
        }
    }
}
