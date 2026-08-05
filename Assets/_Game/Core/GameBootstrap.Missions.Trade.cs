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

        public bool TryVehicleEscape()
        {
            if (VictoryProject == null || Inventory == null) return false;
            int day = TimeSystem != null ? TimeSystem.CurrentDay : 1;
            var summary = VictoryProject.TryEscapeByVehicle(
                Inventory,
                id => _itemCatalog?.GetById(id) ?? MakeRuntimeItem(id),
                day,
                Survivors);
            return summary != null && summary.State == EndgameState.Escaped;
        }

        public bool StartScavengeMission(Survivor survivor, LocationDefinitionSO location)
        {
            if (ScavengingSystem == null || survivor == null || location == null) return false;
            return ScavengingSystem.StartMission(survivor, location);
        }

        public bool StartExpeditionMission(Survivor survivor, LocationDefinitionSO location, ExpeditionStance stance = ExpeditionStance.Stealth)
        {
            if (ExpeditionSystem == null || survivor == null || location == null) return false;
            return ExpeditionSystem.StartExpedition(survivor, location, stance);
        }

        public bool StartExpeditionToNode(Survivor survivor, string nodeId, ExpeditionStance stance = ExpeditionStance.Stealth)
        {
            if (ExpeditionSystem == null || survivor == null || GeneratedMap == null) return false;
            var node = GeneratedMap.GetNode(nodeId);
            if (node == null) return false;
            return ExpeditionSystem.StartExpedition(survivor, node, stance);
        }

        public bool ExecuteWorkbenchLine(int lineIndex)
        {
            return _hud?.WorkbenchUI != null && _hud.WorkbenchUI.Execute(lineIndex);
        }

        public bool OpenTrade(string factionId, Inventory.Inventory factionStock)
        {
            if (_hud?.TradeScreenUI == null || Inventory == null || factionStock == null)
                return false;
            return _hud.TradeScreenUI.Open(factionId, Inventory, factionStock);
        }

        public bool OpenTradeWithFaction(string factionId)
        {
            if (string.IsNullOrEmpty(factionId) || Inventory == null) return false;
            return OpenTrade(factionId, GetOrCreateFactionStock(factionId));
        }

        public bool DemandTradeParley()
        {
            return _hud?.TradeScreenUI != null && _hud.TradeScreenUI.TryDemandParley();
        }

        public bool DemandParleyForFaction(string factionId)
        {
            if (EconomySystem == null || string.IsNullOrEmpty(factionId)) return false;
            if (OpenTradeWithFaction(factionId))
                return DemandTradeParley();
            return EconomySystem.DemandParley(factionId).Applied;
        }

        private Inventory.Inventory GetOrCreateFactionStock(string factionId)
        {
            if (_factionStocks.TryGetValue(factionId, out var existing) && existing != null)
                return existing;
            var stock = new Inventory.Inventory { Capacity = 40, MaxWeight = 200f };
            // Light seed stock so the screen is not empty after a stand-down.
            var water = _itemCatalog?.GetById("clean_water");
            var scrap = _itemCatalog?.GetById("scrap_metal");
            if (water != null) stock.Add(water, 2);
            if (scrap != null) stock.Add(scrap, 4);
            _factionStocks[factionId] = stock;
            return stock;
        }

        public bool StartSurveyMission(Survivor survivor, LocationDefinitionSO location)
        {
            if (ScavengingSystem == null || survivor == null || location == null) return false;
            bool started = ScavengingSystem.StartSurvey(survivor, location);
            if (started) RefreshMapKnowledgeHUD();
            return started;
        }

        public bool RequestSurveyForSurvivor(Survivor survivor)
        {
            if (survivor == null || !survivor.IsAlive || ScavengingSystem == null) return false;
            if (Inventory == null || !Inventory.HasWorkingGeiger()) return false;
            if (_locationCatalog?.locations == null || _locationCatalog.locations.Count == 0) return false;

            LocationDefinitionSO best = null;
            int bestScore = int.MinValue;
            int day = TimeSystem != null ? TimeSystem.CurrentDay : 0;

            foreach (var loc in _locationCatalog.locations)
            {
                if (loc == null) continue;
                var tile = KnowledgeMap?.GetTile(loc.id);
                int score;
                if (tile == null || !tile.Surveyed) score = 1000;
                else score = day - tile.MeasuredAtDay;
                if (score > bestScore)
                {
                    bestScore = score;
                    best = loc;
                }
            }

            return best != null && StartSurveyMission(survivor, best);
        }

    }
}
