using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Data;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Drives scavenging missions to LocationDefinitionSO sites. A survivor
    /// travels for travelHours, accumulates baseRadsPerHour, and rolls for loot
    /// weighted by dangerLevel. Integrates with RadiationSystem and Inventory.
    /// </summary>
    public class LocationScavengingSystem
    {
        private readonly RadiationSystem _radSystem;
        private readonly Inventory _inventory;
        private readonly ItemCatalogSO _itemCatalog;
        private readonly System.Random _rng;
        private readonly List<ActiveMission> _active = new List<ActiveMission>();

        public event Action<ActiveMission> OnMissionStarted;
        public event Action<ActiveMission, List<ItemDefinition>> OnMissionCompleted;

        public LocationScavengingSystem(RadiationSystem radSystem, Inventory inventory, ItemCatalogSO itemCatalog, int seed = 42)
        {
            _radSystem = radSystem;
            _inventory = inventory;
            _itemCatalog = itemCatalog;
            _rng = new System.Random(seed);
        }

        public IReadOnlyList<ActiveMission> ActiveMissions => _active;

        /// <summary>Start a scavenging mission to a location. Returns false if survivor is dead or already on mission.</summary>
        public bool StartMission(Survivor survivor, LocationDefinitionSO location)
        {
            if (survivor == null || !survivor.IsAlive || location == null) return false;

            // Check if survivor is already on a mission
            for (int i = 0; i < _active.Count; i++)
            {
                if (_active[i].SurvivorId == survivor.Id) return false;
            }

            var mission = new ActiveMission
            {
                SurvivorId = survivor.Id,
                LocationId = location.id,
                LocationName = location.displayName,
                HoursRemaining = location.travelHours,
                TotalHours = location.travelHours,
                RadPerHour = location.baseRadsPerHour,
                DangerLevel = location.dangerLevel,
                Survivor = survivor
            };

            _active.Add(mission);
            OnMissionStarted?.Invoke(mission);
            return true;
        }

        /// <summary>Advance active missions over elapsed game hours.</summary>
        public void Tick(float gameHours)
        {
            if (gameHours <= 0f) return;

            for (int i = _active.Count - 1; i >= 0; i--)
            {
                var mission = _active[i];
                mission.HoursRemaining -= gameHours;

                // Accumulate radiation during travel
                if (_radSystem != null && mission.Survivor != null && mission.Survivor.IsAlive)
                {
                    _radSystem.Expose(mission.Survivor, mission.RadPerHour, gameHours);
                }

                if (mission.HoursRemaining <= 0f)
                {
                    CompleteMission(mission);
                    _active.RemoveAt(i);
                }
            }
        }

        private void CompleteMission(ActiveMission mission)
        {
            var loot = RollLoot(mission.DangerLevel);
            if (_inventory != null && loot.Count > 0)
            {
                foreach (var item in loot)
                {
                    _inventory.Add(item, 1);
                }
            }
            OnMissionCompleted?.Invoke(mission, loot);
        }

        private List<ItemDefinition> RollLoot(float dangerLevel)
        {
            var loot = new List<ItemDefinition>();
            if (_itemCatalog == null || _itemCatalog.items.Count == 0) return loot;

            // More danger = more loot (1-3 items, scaled by danger)
            int itemCount = 1 + (int)(dangerLevel / 3f);
            itemCount = Mathf.Clamp(itemCount, 1, 4);

            for (int i = 0; i < itemCount; i++)
            {
                // 60% + (danger * 3%) chance per item slot
                float chance = 0.6f + dangerLevel * 0.03f;
                if (_rng.NextDouble() < chance)
                {
                    var item = _itemCatalog.items[_rng.Next(_itemCatalog.items.Count)];
                    if (item != null)
                    {
                        loot.Add(item);
                    }
                }
            }

            return loot;
        }
    }

    [Serializable]
    public class ActiveMission
    {
        public string SurvivorId;
        public string LocationId;
        public string LocationName;
        public float HoursRemaining;
        public float TotalHours;
        public float RadPerHour;
        public float DangerLevel;
        [NonSerialized] public Survivor Survivor;
    }
}
