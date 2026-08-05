using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.AI;
using AtomicWar._Game.AI.Actions;
using AtomicWar._Game.Crafting;
using AtomicWar._Game.Data;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Events;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Flashpoint;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Shelter.Modules;
using AtomicWar._Game.Simulation;
using AtomicWar._Game.UI;
using AtomicWar._Game.Medical;
using AtomicWar._Game.Economy;
using AtomicWar._Game.Utilities;

namespace AtomicWar._Game.Core
{
    public partial class GameBootstrap
    {
        private float GetMapUncertaintyFor(Survivor survivor)
        {
            if (KnowledgeMap == null || survivor == null) return 0.5f;

            bool hasWorkingGeiger = Inventory != null && Inventory.HasWorkingGeiger();
            int day = TimeSystem != null ? TimeSystem.CurrentDay : 0;

            float missionUncertainty = GetActiveMissionUncertainty(survivor, day, hasWorkingGeiger);
            if (missionUncertainty >= 0f) return missionUncertainty;

            return GetAverageMapUncertainty(day, hasWorkingGeiger);
        }

        private float GetActiveMissionUncertainty(Survivor survivor, int day, bool hasWorkingGeiger)
        {
            if (ScavengingSystem == null) return -1f;
            foreach (var mission in ScavengingSystem.ActiveMissions)
            {
                if (mission?.SurvivorId != survivor.Id) continue;
                var view = KnowledgeMap.GetPlayerView(mission.LocationId, day, hasWorkingGeiger);
                return Mathf.Clamp01(1f - view.Confidence);
            }
            return -1f;
        }

        private float GetAverageMapUncertainty(int day, bool hasWorkingGeiger)
        {
            float totalConfidence = 0f;
            int count = 0;
            foreach (var id in KnowledgeMap.Tiles.Keys)
            {
                var view = KnowledgeMap.GetPlayerView(id, day, hasWorkingGeiger);
                totalConfidence += view.Confidence;
                count++;
            }
            if (count == 0) return hasWorkingGeiger ? 0.5f : 1f;
            return Mathf.Clamp01(1f - (totalConfidence / count));
        }

        private float GetPartyAverageRadiationDose()
        {
            if (Survivors == null || Survivors.Count == 0) return 0f;
            float sum = 0f;
            int n = 0;
            for (int i = 0; i < Survivors.Count; i++)
            {
                var s = Survivors[i];
                if (s == null || !s.IsAlive) continue;
                sum += s.RadiationDose;
                n++;
            }
            return n > 0 ? sum / n : 0f;
        }

        private bool PartyHasAcuteRadiationSyndrome()
        {
            if (Survivors == null) return false;
            for (int i = 0; i < Survivors.Count; i++)
            {
                var s = Survivors[i];
                if (s == null || !s.IsAlive) continue;
                if (s.HasAcuteRadiationSyndrome
                    || s.HasStatus(SurvivorStatus.AcuteRadiationSyndrome))
                    return true;
            }
            return false;
        }

        private bool PartyWearsIntactHazmat()
        {
            if (AnyAliveWithFullSuit()) return true;
            return Inventory != null && Inventory.GetEquippedProtection() > 0f;
        }

        private bool AnyAliveWithFullSuit()
        {
            if (Survivors == null) return false;
            for (int i = 0; i < Survivors.Count; i++)
            {
                var s = Survivors[i];
                if (s != null && s.IsAlive && s.HasFullSuitEquipped) return true;
            }
            return false;
        }
    }
}
