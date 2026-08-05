using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Data;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Events;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Medical;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Core
{
    public partial class ExpeditionSystem
    {

        public bool StartExpedition(
            Survivor survivor,
            LocationDefinitionSO location,
            ExpeditionStance stance = ExpeditionStance.Stealth,
            float maxLootCapacity = MaxCarryingCapacityDefault)
        {
            if (!CanStartExpedition(survivor) || location == null) return false;

            float trueRad = ResolveTrueRad(location);
            float travelHours = location.travelHours * CurrentWeatherTravelMultiplier();
            return RegisterNewExpedition(survivor, new PathRequest
            {
                NodeId = location.id,
                DisplayName = location.displayName,
                TrueRad = trueRad,
                DangerLevel = location.dangerLevel,
                TravelHours = travelHours,
                Stance = stance,
                MaxLootCapacity = maxLootCapacity
            });
        }

        /// <summary>
        /// Start an expedition to a proc-gen <see cref="MapNode"/>.
        /// Path travel uses weather-scaled hours from the generated map graph.
        /// </summary>
        public bool StartExpedition(
            Survivor survivor,
            MapNode node,
            ExpeditionStance stance = ExpeditionStance.Stealth,
            float maxLootCapacity = MaxCarryingCapacityDefault)
        {
            if (!CanStartExpedition(survivor) || node == null || node.IsShelter) return false;

            return RegisterNewExpedition(survivor, new PathRequest
            {
                NodeId = node.NodeId,
                DisplayName = node.DisplayName,
                TrueRad = ResolveNodeTrueRad(node),
                DangerLevel = node.DangerLevel,
                TravelHours = ResolveNodeTravelHours(node),
                Stance = stance,
                MaxLootCapacity = maxLootCapacity
            });
        }

        public bool StartExpeditionFromPath(Survivor survivor, PathRequest request)
        {
            if (!CanStartExpedition(survivor) || string.IsNullOrEmpty(request.NodeId)) return false;
            if (request.NodeId == GeneratedMap.ShelterNodeId) return false;

            // Prefer live map data when available
            MapNode node = _generatedMap?.GetNode(request.NodeId);
            if (node != null)
                return StartExpedition(survivor, node, request.Stance, request.MaxLootCapacity);

            if (request.MaxLootCapacity <= 0f)
                request.MaxLootCapacity = MaxCarryingCapacityDefault;
            if (string.IsNullOrEmpty(request.DisplayName))
                request.DisplayName = request.NodeId;
            request.TrueRad = Mathf.Max(0f, request.TrueRad);
            request.TravelHours = Mathf.Max(0.1f, request.TravelHours);
            return RegisterNewExpedition(survivor, request);
        }

        private bool CanStartExpedition(Survivor survivor)
        {
            if (HatchBlocksExpeditions) return false;
            if (survivor == null || !survivor.IsAlive) return false;
            return !IsOnExpedition(survivor.Id);
        }

        private float ResolveNodeTrueRad(MapNode node)
        {
            float trueRad = node.TrueRad;
            if (_knowledgeMap == null) return trueRad;
            var tile = _knowledgeMap.GetTile(node.NodeId);
            return tile != null ? tile.TrueRad : trueRad;
        }

        private float ResolveNodeTravelHours(MapNode node)
        {
            if (_generatedMap == null)
                return node.DistanceFromShelter * CurrentWeatherTravelMultiplier();

            var weather = _weatherSystem != null ? _weatherSystem.Current : WeatherKind.Clear;
            float travelHours = _generatedMap.GetTravelHoursFromShelter(node.NodeId, weather);
            if (travelHours <= 0f)
                travelHours = node.DistanceFromShelter * CurrentWeatherTravelMultiplier();
            return travelHours;
        }

        private bool RegisterNewExpedition(Survivor survivor, PathRequest request)
        {
            float capacity = request.MaxLootCapacity > 0f
                ? request.MaxLootCapacity
                : MaxCarryingCapacityDefault;
            // Prompt #206 — Pack Mule +10 kg base carry weight.
            if (_expeditionPerks != null)
                capacity += _expeditionPerks.GetCarryCapacityBonus(survivor);

            float travelHours = request.TravelHours;
            // Prompt #208 — Urban Pathfinder −30% City/Ruin travel (stacks with Bicycle).
            travelHours = ApplyUrbanPathfinderTravel(survivor, request.NodeId, travelHours);

            int distanceTicks = Mathf.Max(1, Mathf.RoundToInt(travelHours));
            var state = new ExpeditionState
            {
                ExpeditionId = Guid.NewGuid().ToString("N"),
                SurvivorId = survivor.Id,
                Survivor = survivor,
                TargetLocationId = request.NodeId,
                TargetLocationName = string.IsNullOrEmpty(request.DisplayName)
                    ? request.NodeId
                    : request.DisplayName,
                Stance = request.Stance,
                Phase = ExpeditionPhase.Outbound,
                TotalDistanceTicks = distanceTicks,
                CarryingCapacity = capacity,
                TrueRadPerHour = request.TrueRad,
                DangerLevel = request.DangerLevel,
                Stamina = 100f,
                SuitDegradation = 0f
            };

            // Prompt #68 — equip bicycle when stocked and weather allows.
            TryEquipBicycle(state);

            survivor.State = SurvivorState.Working;
            _activeExpeditions.Add(state);
            OnExpeditionStarted?.Invoke(state);
            return true;
        }

        private float ApplyUrbanPathfinderTravel(Survivor survivor, string nodeId, float travelHours)
        {
            if (_expeditionPerks == null || survivor == null || travelHours <= 0f)
                return travelHours;

            var node = ResolveMapNode(nodeId);
            bool cityOrRuin = false;
            if (node != null)
            {
                cityOrRuin = ExpeditionPerkSystem.IsCityOrRuinNode(
                    node.Tags, node.Ring.ToString());
            }
            else if (!string.IsNullOrEmpty(nodeId))
            {
                // LocationDefinition path without MapNode — treat city-ish ids.
                cityOrRuin = nodeId.IndexOf("city", StringComparison.OrdinalIgnoreCase) >= 0
                             || nodeId.IndexOf("ruin", StringComparison.OrdinalIgnoreCase) >= 0
                             || nodeId.IndexOf("urban", StringComparison.OrdinalIgnoreCase) >= 0;
            }

            float mult = _expeditionPerks.GetCityRuinTravelMultiplier(survivor, cityOrRuin);
            return Mathf.Max(0.1f, travelHours * mult);
        }

        private void TryEquipBicycle(ExpeditionState state)
        {
            if (state == null || _bicycleSystem == null) return;
            if (_hasItem == null || !_hasItem(BicycleSystem.BicycleItemId)) return;

            var weather = _weatherSystem != null ? _weatherSystem.Current : WeatherKind.Clear;
            if (!BicycleSystem.CanUseBicycle(weather)) return;
            if (!_bicycleSystem.EquipBicycle(state)) return;

            // Half travel time → fewer distance ticks (min 1).
            float mul = BicycleSystem.GetSpeedMultiplier(state, weather);
            if (mul < 1f)
            {
                state.TotalDistanceTicks = Mathf.Max(1,
                    Mathf.RoundToInt(state.TotalDistanceTicks * mul));
            }
        }

        private float CurrentWeatherTravelMultiplier()
        {
            if (_weatherSystem == null) return 1f;
            return GeneratedMap.WeatherTravelMultiplier(_weatherSystem.Current);
        }

        public bool RecallExpedition(string survivorId)
        {
            var state = GetExpeditionBySurvivor(survivorId);
            if (state == null || state.Phase == ExpeditionPhase.Inbound || state.Phase == ExpeditionPhase.Completed)
                return false;

            state.IsRetreating = true;
            state.IsPushingLuck = false;
            state.Phase = ExpeditionPhase.Inbound;
            return true;
        }

        public ExpeditionState GetExpeditionBySurvivor(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return null;
            for (int i = 0; i < _activeExpeditions.Count; i++)
            {
                if (_activeExpeditions[i].SurvivorId == survivorId) return _activeExpeditions[i];
            }
            return null;
        }

        public bool IsOnExpedition(string survivorId)
        {
            return GetExpeditionBySurvivor(survivorId) != null;
        }

        public void Tick(float gameHours)
        {
            if (gameHours <= 0f) return;

            // Process tick steps (1 hour per tick loop)
            int ticksToProcess = Mathf.Max(1, Mathf.FloorToInt(gameHours));
            float hoursPerTick = gameHours / ticksToProcess;

            for (int t = 0; t < ticksToProcess; t++)
            {
                ProcessSingleTick(hoursPerTick);
            }
        }

        private float ResolveTrueRad(LocationDefinitionSO location)
        {
            if (_knowledgeMap != null)
            {
                var tile = _knowledgeMap.GetTile(location.id);
                if (tile != null) return tile.TrueRad;
            }
            return location.baseRadsPerHour;
        }

    }
}
