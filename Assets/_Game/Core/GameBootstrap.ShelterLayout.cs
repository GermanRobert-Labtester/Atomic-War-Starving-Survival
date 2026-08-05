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
        private void ApplyShelterLayout(Shelter.ShelterMapSO layout)
        {
            if (layout == null || Shelter == null) return;

            RegisterLayoutRooms(layout);
            InstallLayoutModules(layout);
            ApplyLayoutTraits(layout);

            if (layout.hasWaterHeater && layout.startingCleanWater > 0f && WaterStorage != null)
                WaterStorage.AddClean(layout.startingCleanWater);

            // Initial hatch damage (security reduction) reserved for future wiring.
            if (layout.startingHatchDamage > 0f && HatchDefenseSystem != null)
            {
            }
        }

        private void RegisterLayoutRooms(Shelter.ShelterMapSO layout)
        {
            if (layout.roomIds == null) return;
            for (int i = 0; i < layout.roomIds.Length; i++)
            {
                var room = new Shelter.ShelterRoom(layout.roomIds[i], null);
                Shelter.RegisterRoom(room);
                AtmosphereSystem?.RegisterRoom(room);
            }
        }

        private void InstallLayoutModules(Shelter.ShelterMapSO layout)
        {
            if (layout.startingModules == null) return;
            for (int i = 0; i < layout.startingModules.Length; i++)
            {
                var m = layout.startingModules[i];
                Shelter.AddModule(new Shelter.ShelterModuleInstance(m.moduleId, m.level)
                {
                    IsEnabled = m.isEnabled,
                    Fuel = m.fuel,
                    FilterHealth = m.filterHealth,
                    RoomId = m.roomId
                });
            }
        }

        private void ApplyLayoutTraits(Shelter.ShelterMapSO layout)
        {
            if (layout.traits == null) return;
            for (int i = 0; i < layout.traits.Length; i++)
                ApplyLayoutTrait(layout.traits[i]);
        }

    }
}
