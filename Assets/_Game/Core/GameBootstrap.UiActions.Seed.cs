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

        private void SeedKnowledgeMap()
        {
            if (KnowledgeMap == null) return;

            // Prefer proc-gen map nodes (authoritative per-playthrough layout)
            if (GeneratedMap?.Nodes != null)
            {
                for (int i = 0; i < GeneratedMap.Nodes.Count; i++)
                {
                    var n = GeneratedMap.Nodes[i];
                    if (n == null || string.IsNullOrEmpty(n.NodeId) || n.IsShelter) continue;
                    KnowledgeMap.SeedTile(n.NodeId, n.TrueRad, n.RumoredRad, 1f);
                }
            }

            // Also seed catalog locations if present (legacy / static sites)
            if (_locationCatalog?.locations == null) return;
            var rng = new System.Random(_worldSeed + 17);
            foreach (var loc in _locationCatalog.locations)
            {
                if (loc == null || string.IsNullOrEmpty(loc.id)) continue;
                if (KnowledgeMap.GetTile(loc.id) != null) continue; // already seeded from map
                float rumorScale = 0.4f + (float)rng.NextDouble() * 0.4f;
                KnowledgeMap.SeedTile(loc.id, loc.baseRadsPerHour, loc.baseRadsPerHour * rumorScale, 1f);
            }
        }

    }
}
